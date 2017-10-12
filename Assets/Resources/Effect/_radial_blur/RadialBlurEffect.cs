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
		if (m_material && blurFactor > 0)  
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

	#region === 加个来回特效 === 
	public float m_duration = 1;
	float _halfDuration = 0;
	float _curtime = 0;
	public bool isRunning = false;
	void Update(){
		if (!isRunning || m_duration <= 0)
			return;

		_halfDuration = m_duration * 0.5f;
		_curtime += Time.deltaTime;
		isRunning = _curtime <= m_duration;
		if (_curtime <= _halfDuration) {
			blurFactor = Mathf.Lerp (0.0f, 0.1f, _curtime / _halfDuration);
		} else {
			blurFactor = Mathf.Lerp (0.1f, 0.0f, (_curtime - _halfDuration) / _halfDuration);
		}

		if (!isRunning) {
			_curtime = 0;
			blurFactor = 0;
		}
	}
	#endregion
}