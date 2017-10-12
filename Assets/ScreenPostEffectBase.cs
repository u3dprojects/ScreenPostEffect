using UnityEngine;
using System.Collections;

//非运行时也触发效果
[ExecuteInEditMode]
//屏幕后处理特效一般都需要绑定在摄像机上
[RequireComponent(typeof(Camera))]
public class ScreenPostEffectBase : MonoBehaviour {

	public Shader m_shader = null;

	private bool m_isNewMat = false;

	private Material _material = null;
	public Material m_material
	{
		get
		{
			if (_material == null)
				_material = GenerateMaterial(m_shader,_material);
			return _material;
		}
	}

	protected void Start () {
		CheckSupported();
	}

	// protected void OnValidate(){
	// }

	protected void Reset(){
		m_shader = null;
		_material = null;
	}

	protected void OnDisable () {
		if (m_isNewMat) {
			DestroyImmediate (_material);
			_material = null;
			m_isNewMat = false;
		}
	}

	bool IsSystemSupported(){
		if (SystemInfo.supportsImageEffects && SystemInfo.supportsRenderTextures)
			return true;
		return false;
	}

	public void CheckSupported(){
		bool isSupported = IsSystemSupported ();
		this.enabled = isSupported;
	}

	//根据shader创建用于屏幕特效的材质
	private Material GenerateMaterial(Shader shader,Material material)
	{
		//需要判断shader是否支持
		if (shader == null || !shader.isSupported)
			return null;

		if (material && material.shader == shader) {
			return material;
		}
		
		material = new Material(shader);
		material.hideFlags = HideFlags.DontSave; // HideAndDontSave
		if (material) {
			m_isNewMat = true;
			return material;
		}
		return null;
	}

	protected void ReGenerateMaterial(Shader shader,Material material)
	{
		_material = GenerateMaterial (shader, material);
	}

	protected void ReGenerateMaterial(string shaderName,Material material)
	{
		ReGenerateMaterial(Shader.Find (shaderName),material);
	}

	protected void ReGenerateMaterial(string shaderName)
	{
		ReGenerateMaterial (shaderName,_material);
	}
}