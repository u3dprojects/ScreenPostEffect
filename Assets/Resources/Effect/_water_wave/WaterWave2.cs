using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 水浪
/// </summary>
public class WaterWave2 : ScreenPostEffectBase
{
	//振幅(最大和最小的幅度)
	[Range(0,1)]
	public float _Amplitude = 0.3f;  

	//角速度(圈数)
	[Range(0,50)]
	public float _AngularVelocity = 10.0f;


	//移动速度
	[Range(0,30)]
	public float _Speed = 1.0f;  

	private float waveStartTime;

	private float sumDistance = 0;

	private int m_maxVal{
		get{
			return Screen.width > Screen.height ? Screen.width : Screen.height;
		}
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination)  
	{  
		if (m_material) {
			//计算波纹移动的距离，根据enable到目前的时间*速度求解  
			float curWaveDistance = (Time.time - waveStartTime) * _Speed;
			sumDistance += curWaveDistance;
			if (sumDistance >= m_maxVal) {
				sumDistance = 0;
				waveStartTime = Time.time;
			}
			//设置一系列参数  
			m_material.SetFloat ("_Amplitude", _Amplitude);  
			m_material.SetFloat ("_AngularVelocity", _AngularVelocity);  
			m_material.SetFloat ("_Speed", _Speed);  

			Graphics.Blit (source, destination, m_material);  
		} else {
			Graphics.Blit (source, destination);  
		}
	}  

	void OnEnable()  
	{  
		//设置startTime  
		waveStartTime = Time.time;
	}
}