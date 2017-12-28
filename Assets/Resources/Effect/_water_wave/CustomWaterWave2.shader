Shader "Custom/WaterWave2"
{
	Properties 
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Amplitude ("Amplitude", Range(0, 1)) = 0.3 // 振幅(最大和最小的幅度)
        _AngularVelocity ("Angular", Range(0, 50)) = 10 // 角速度(圈数)
        _Speed ("Speed", Range(0, 30)) = 10 // 移动速度
	}
	SubShader 
	{
		Tags { "LightMode" = "ForwardBase" }
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			sampler2D _MainTex;
			float4 _MainTex_ST;

			float _Amplitude;     
            float _AngularVelocity;     
            float _Speed;   

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0; 
			};  

			v2f vert(appdata_full  v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				i.uv.y += (_Amplitude * sin(_AngularVelocity * i.uv.x + _Speed * _Time.y));  
				float4 c = tex2D(_MainTex, i.uv);
				return c;
			}
			ENDCG
		}
	} 
	// FallBack "Diffuse"
    Fallback off
}