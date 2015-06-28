Shader "Custom/LEDGlow" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_GlowPower ("Glow", Range(0.0, 10.0)) = 3.0
		_GlowContrast("Glow Contrast", Range(1.0,10.0)) = 2.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert

		
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		float _GlowPower;
		float _GlowContrast;

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			
			half3 glow = half3(pow(c.r,_GlowContrast),pow(c.g,_GlowContrast),pow(c.b,_GlowContrast));
			o.Emission = pow(c.rgb,_GlowContrast) * _GlowPower;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
