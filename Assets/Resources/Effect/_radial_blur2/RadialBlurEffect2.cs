using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 径向模糊效果
/// </summary>
public class RadialBlurEffect2 : ScreenPostEffectBase
{
	//模糊程度，不能过高  
	[Range(0,0.15f)]  
	public float blurFactor = 0.1f;  

	//模糊中心（0-1）屏幕空间，默认为中心点  
	public Vector2 blurCenter = new Vector2(0.5f, 0.5f);

	//清晰图像与原图插值  
	[Range(0.0f, 4.0f)]  
	public float lerpFactor = 0.5f;

	//降低分辨率
	public int downSampleFactor = 2;

	void OnRenderImage (RenderTexture source, RenderTexture destination)  
	{  
		if (m_material)  
		{  
			downSampleFactor = Mathf.Max (downSampleFactor, 0);

			//申请两块降低了分辨率的RT  
			RenderTexture rt1 = RenderTexture.GetTemporary(source.width >> downSampleFactor, source.height >> downSampleFactor, 0, source.format);  
			RenderTexture rt2 = RenderTexture.GetTemporary(source.width >> downSampleFactor, source.height >> downSampleFactor, 0, source.format);  
			Graphics.Blit(source, rt1);  

			m_material.EnableKeyword ("UNITY_UV_STARTS_AT_TOP");
			m_material.DisableKeyword ("UNITY_UV_STARTS_AT_TOP");

			//使用降低分辨率的rt进行模糊:pass0  
			m_material.SetFloat("_BlurFactor", blurFactor);  
			m_material.SetVector("_BlurCenter", blurCenter);  
			Graphics.Blit(rt1, rt2, m_material, 0);  

			//使用rt2和原始图像lerp:pass1  
			m_material.SetTexture("_BlurTex", rt2);  
			m_material.SetFloat("_LerpFactor", lerpFactor);  
			Graphics.Blit(source, destination, m_material, 1);  

			//释放RT  
			RenderTexture.ReleaseTemporary(rt1);  
			RenderTexture.ReleaseTemporary(rt2);  
		}  
		else  
		{  
			Graphics.Blit(source, destination);  
		}     

	}   
}