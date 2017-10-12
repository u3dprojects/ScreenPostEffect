using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 水波
/// </summary>
public class WaterWave : ScreenPostEffectBase
{
	//距离系数  
	public float distanceFactor = 60.0f;  
	//时间系数  
	public float timeFactor = -30.0f;  
	//sin函数结果系数  
	public float totalFactor = 1.0f;  

	//波纹宽度  
	public float waveWidth = 0.3f;  
	//波纹扩散的速度  
	public float waveSpeed = 0.3f;  

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
			float curWaveDistance = (Time.time - waveStartTime) * waveSpeed;
			sumDistance += curWaveDistance;
			if (sumDistance >= m_maxVal) {
				sumDistance = 0;
				waveStartTime = Time.time;
			}
			//设置一系列参数  
			m_material.SetFloat ("_distanceFactor", distanceFactor);  
			m_material.SetFloat ("_timeFactor", timeFactor);  
			m_material.SetFloat ("_totalFactor", totalFactor);  
			m_material.SetFloat ("_waveWidth", waveWidth);  
			m_material.SetFloat ("_curWaveDis", curWaveDistance);  

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