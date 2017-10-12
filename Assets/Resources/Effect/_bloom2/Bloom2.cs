using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 饱和度
/// </summary>
public class Bloom2 : ScreenPostEffectBase
{
	//Bloom权值
	[Range(0,4)]
	public int iterations = 2;

	[Range(0.2f,3.0f)]
	public float blurSpread = 2f;

	//分辨率
	[Range(1,8)]
	public int downSample = 2;

	[Range(0.0f,1.0f)]
	public float luminanceThresholdMin = 0.1f;

	[Range(0.0f,1.0f)]
	public float luminanceThresholdMax = 0.6f;

	[Range(0.1f,10.0f)]
	public float luminanceFactor = 1.0f;

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (m_material)
		{
			m_material.SetFloat("_LuminanceThresholdMin", luminanceThresholdMin);
			m_material.SetFloat("_LuminanceThresholdMax", luminanceThresholdMax);
			m_material.SetFloat("_LuminanceFactor", luminanceFactor);

			int rtW = source.width / downSample;
			int rtH = source.height / downSample;

			RenderTexture buffer0 = RenderTexture.GetTemporary (rtW, rtH, 0);
			buffer0.filterMode = FilterMode.Bilinear;
			// shader 中的第一个 Pass
			Graphics.Blit(source, buffer0, m_material, 0);

			for (int i = 0; i < iterations; i++) {
				m_material.SetFloat("_BlurSize", 1.0f + (i + 1) * blurSpread);

				RenderTexture buffer1 = RenderTexture.GetTemporary (rtW, rtH, 0);
				Graphics.Blit(buffer0, buffer1, m_material, 1);
				RenderTexture.ReleaseTemporary(buffer0);
				buffer0 = buffer1;

				buffer1 = RenderTexture.GetTemporary (rtW, rtH, 0);
				Graphics.Blit(buffer0, buffer1, m_material, 2);
				RenderTexture.ReleaseTemporary(buffer0);
				buffer0 = buffer1;
			}

			m_material.SetTexture("_Bloom", buffer0);
			Graphics.Blit(source, destination, m_material,3);
			RenderTexture.ReleaseTemporary(buffer0);
		}
	}
}