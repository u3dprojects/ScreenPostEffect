Shader "Custom/Bloom2"
{
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Bloom("Bloom (RGB)", 2D) = "white" {}
		_LuminanceThresholdMin("Luminance Min",Float) = 0.1
		_LuminanceThresholdMax("Luminance Max",Float) = 0.5
		_LuminanceFactor("Luminance Factor",Float) = 0.5
		_BlurSize("Blur Size",Float) = 1.0
	}

	SubShader
	{

	CGINCLUDE
	#include "UnityCG.cginc"
	
	//用于阈值提取高亮部分
	struct v2f_threshold
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	//用于blur
	struct v2f_blur
	{
		float4 pos : SV_POSITION;
		half2 uv[5]  : TEXCOORD0;
	};

	//用于bloom
	struct v2f_bloom
	{
		float4 pos : SV_POSITION;
		half4 uv  : TEXCOORD0;
	};

	sampler2D _MainTex;
	float4 _MainTex_TexelSize;
	sampler2D _Bloom;

	float _LuminanceThresholdMin;
	float _LuminanceThresholdMax;
	float _LuminanceFactor;
	float _BlurSize;

	//高亮部分提取shader
	v2f_threshold vert_threshold(appdata_img v)
	{
		v2f_threshold o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord;

		return o;
	}

	fixed luminance(fixed4 _color){
		return 0.2125 * _color.r + 0.7154 * _color.g + 0.0721 * _color.b;
	}

	fixed4 frag_threshold(v2f_threshold i) : SV_Target
	{
		fixed4 color = tex2D(_MainTex, i.uv);
		fixed lumin = luminance(color);
		lumin *= step(lumin,_LuminanceThresholdMax);
		fixed val = pow(max(0,(lumin - _LuminanceThresholdMin)),_LuminanceFactor);

		return color * val;
	}

	//高斯模糊 vert shader（上一篇文章有详细注释）
	v2f_blur vert_blur_vertical(appdata_img v)
	{
		v2f_blur o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		half2 uv = v.texcoord;
		o.uv[0] = uv;
		o.uv[1] = uv + float2(0.0,_MainTex_TexelSize.y * 1.0) * _BlurSize;
		o.uv[2] = uv - float2(0.0,_MainTex_TexelSize.y * 1.0) * _BlurSize;
		o.uv[3] = uv + float2(0.0,_MainTex_TexelSize.y * 2.0) * _BlurSize;
		o.uv[4] = uv - float2(0.0,_MainTex_TexelSize.y * 2.0) * _BlurSize;

		return o;
	}

	v2f_blur vert_blur_horizontal(appdata_img v)
	{
		v2f_blur o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		half2 uv = v.texcoord;
		o.uv[0] = uv;
		o.uv[1] = uv + float2(_MainTex_TexelSize.x * 1.0,0.0) * _BlurSize;
		o.uv[2] = uv - float2(_MainTex_TexelSize.x * 1.0,0.0) * _BlurSize;
		o.uv[3] = uv + float2(_MainTex_TexelSize.x * 2.0,0.0) * _BlurSize;
		o.uv[4] = uv - float2(_MainTex_TexelSize.x * 2.0,0.0) * _BlurSize;

		return o;
	}

	//高斯模糊 pixel shader（上一篇文章有详细注释）
	fixed4 frag_blur(v2f_blur i) : SV_Target
	{
		float weight[3] = {0.4026,0.2442,0.0545};
		fixed3 sum = tex2D(_MainTex, i.uv[0]).rgb * weight[0];
		for(int it = 1; it < 3; it++){
			sum += tex2D(_MainTex, i.uv[it * 2 - 1]).rgb * weight[it];
			sum += tex2D(_MainTex, i.uv[it * 2]).rgb * weight[it];
		}
		return fixed4(sum,1.0);
	}

		//Bloom效果 vertex shader
	v2f_bloom vert_bloom(appdata_img v)
	{
		v2f_bloom o;
		//mvp矩阵变换
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		//uv坐标传递
		o.uv.xy = v.texcoord;
		o.uv.zw = v.texcoord;
#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			o.uv.w = 1 - o.uv.w;
#endif	
		return o;
	}

	fixed4 frag_bloom(v2f_bloom i) : SV_Target
	{
		return tex2D(_MainTex,i.uv.xy) + tex2D(_Bloom,i.uv.zw);
	}

		ENDCG

		ZTest Always
		Cull Off
		ZWrite Off
		Fog{ Mode Off }

		//pass 0: 提取高亮部分
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_threshold
			#pragma fragment frag_threshold
			ENDCG
		}

		//pass 1: 高斯模糊 横向
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_blur_horizontal
			#pragma fragment frag_blur
			ENDCG
		}

		//pass 2: 高斯模糊 纵向
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_blur_vertical
			#pragma fragment frag_blur
			ENDCG
		}

		//pass 3: Bloom效果
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_bloom
			#pragma fragment frag_bloom
			ENDCG
		}
	} 
    Fallback off
}