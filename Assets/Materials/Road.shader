Shader "Custom/Road" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		LaneNum ("LaneNum", Int) = 1
		Length ("Length", Float) = 1.0
		LineWidth ("LineWidth", Float) = 0.03
		LineLength ("LineLength", Float) = 1.0

		CrosswalkCount ("CrosswalkCount", Int) = 10
		CrosswalkLength ("CrosswalkLength", Float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float Length;
		int LaneNum;
		float LineWidth;
		float LineLength;
		int CrosswalkCount;
		float CrosswalkLength;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			float x = (IN.uv_MainTex.x-0.5) * LaneNum * 2;
			float dist2center = IN.uv_MainTex.y * Length;

			o.Albedo = float3(0.4,0.4,0.4);
			if (dist2center < CrosswalkLength) {
				float n = IN.uv_MainTex.x * (CrosswalkCount * 2 - 1);
				if (0 == fmod(floor(n), 2)){
					o.Albedo = float3(1,1,1);
				}
			}
			else if (abs(x) < LineWidth) {
				o.Albedo = float3(1,1,0);
			}
			else if (abs(x) + 2*LineWidth > LaneNum) {
				o.Albedo = float3(1,1,1);
			}
			else if (x + LineWidth - floor(x+LineWidth) < 2 * LineWidth && fmod(floor(dist2center / LineLength), 2) == 0) {
				o.Albedo = float3(1,1,1);
			}

			//o.Albedo = o.Albedo * 0.7 + 0.3 * tex2D (_MainTex, float2(x*2, dist2center));

			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
