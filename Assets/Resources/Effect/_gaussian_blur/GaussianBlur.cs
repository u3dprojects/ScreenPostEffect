using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 高斯模糊
/// </summary>
public class GaussianBlur : ScreenPostEffectBase
{
	// 值
	[Range(0,5)]
	public float BlurSize = 0.0f;

	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{  
		if (m_material) {
			int rtW = source.width / 8;  
			int rtH = source.height / 8;  


			RenderTexture rtTempA = RenderTexture.GetTemporary (rtW, rtH, 0, source.format);  
			rtTempA.filterMode = FilterMode.Bilinear;  


			Graphics.Blit (source, rtTempA);  

			for (int i = 0; i < 2; i++) {  

				float iteraionOffs = i * 1.0f;  
				m_material.SetFloat ("_blurSize", BlurSize + iteraionOffs);  

				//vertical blur  
				RenderTexture rtTempB = RenderTexture.GetTemporary (rtW, rtH, 0, source.format);  
				rtTempB.filterMode = FilterMode.Bilinear;  
				Graphics.Blit (rtTempA, rtTempB, m_material, 0);  
				RenderTexture.ReleaseTemporary (rtTempA);  
				rtTempA = rtTempB;  

				//horizontal blur  
				rtTempB = RenderTexture.GetTemporary (rtW, rtH, 0, source.format);  
				rtTempB.filterMode = FilterMode.Bilinear;  
				Graphics.Blit (rtTempA, rtTempB, m_material, 1);  
				RenderTexture.ReleaseTemporary (rtTempA);  
				rtTempA = rtTempB;  

			}       
			Graphics.Blit (rtTempA, destination);  

			RenderTexture.ReleaseTemporary (rtTempA);  
		} else {
			Graphics.Blit (source, destination);  
		}
	}
}