using UnityEngine;
using System.Collections;
using System;

public class ImageEffect_RadialBlur : ScreenPostEffectBase {
	#region Variables
	[Range(0.0f, 1.0f)]
	public float dist = 0.17f;

	[Range(0.1f, 5.0f)]
	public float strength = 2.09f;
	#endregion

	void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture)
	{
		
		if(m_material != null){
			int rtW = sourceTexture.width/8;
	        int rtH = sourceTexture.height/8;


			m_material.SetFloat ("_SampleDist", dist);
			m_material.SetFloat ("_SampleStrength", strength);	

	        RenderTexture rtTempA = RenderTexture.GetTemporary (rtW, rtH, 0,RenderTextureFormat.Default);
            rtTempA.filterMode = FilterMode.Bilinear;

            Graphics.Blit (sourceTexture, rtTempA);

            RenderTexture rtTempB = RenderTexture.GetTemporary (rtW, rtH, 0,RenderTextureFormat.Default);
            rtTempB.filterMode = FilterMode.Bilinear;
            // RadialBlurMaterial.SetTexture ("_MainTex", rtTempA);
			Graphics.Blit (rtTempA, rtTempB, m_material,0);

			m_material.SetTexture ("_BlurTex", rtTempB);
			Graphics.Blit (sourceTexture, destTexture, m_material,1);

            RenderTexture.ReleaseTemporary(rtTempA);
            RenderTexture.ReleaseTemporary(rtTempB);
		}else{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}
}
