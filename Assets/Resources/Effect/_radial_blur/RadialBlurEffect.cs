using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 径向模糊效果
/// </summary>
public class RadialBlurEffect : ScreenPostEffectBase
{
	//模糊程度，不能过高  
	[Range(0,0.1f)]  
	public float blurFactor = 0.1f;  
	//模糊中心（0-1）屏幕空间，默认为中心点  
	public Vector2 blurCenter = new Vector2(0.5f, 0.5f);  

	void OnRenderImage (RenderTexture source, RenderTexture destination)  
	{  
		if (m_material)  
		{  
			m_material.SetFloat("_BlurFactor", blurFactor);  
			m_material.SetVector("_BlurCenter", blurCenter);  
			Graphics.Blit(source, destination, m_material);  
		}  
		else  
		{  
			Graphics.Blit(source, destination);  
		}     

	}
}