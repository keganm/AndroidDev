Shader "Custom/GlowShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimPower("Rim Power", Range(0.0,10.0)) = 3.0
		_RimContrast("Rim Contrast", Range(1.0,10.0)) = 2.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float4 _Color;
		float4 _RimColor;
		float _RimPower;
		float _RimContrast;

		struct Input {
			float2 uv_MainTex;
			float color : Color;
			float3 viewDir;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			o.Albedo = c;
			
			half rim = pow(1.0 - saturate(normalize(IN.viewDir)),_RimContrast);
			o.Emission = _RimColor.rgb * pow(rim, _RimPower);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
