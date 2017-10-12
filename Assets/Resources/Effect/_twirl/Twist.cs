using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 屏幕扭曲效果
/// </summary>
public class Twist : ScreenPostEffectBase
{
	
	public float twistAngle = 0;

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (m_material) {
			m_material.SetFloat ("_Twist", twistAngle);
			Graphics.Blit (source, destination, m_material);
		} else {
			Graphics.Blit (source, destination);
		}
	}
}