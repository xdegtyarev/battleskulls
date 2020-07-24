Shader "Custom/Unlit-Masked" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Mask ("Alpha (A8)", 2D) = "white" {}
	}
	Category{	
	Tags { "RenderType"="Opaque" }
	Blend SrcAlpha OneMinusSrcAlpha
	SubShader {
		

		Pass {
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
			
				struct v2f {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float2 uv2 : TEXCOORD1;
				};
			
				float4 _MainTex_ST;
				float4 _Mask_ST;
			
				v2f vert(appdata_base v) {
					v2f o;
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.uv2= TRANSFORM_TEX(v.texcoord, _Mask);
					return o;
				}
			
				sampler2D _MainTex;
				sampler2D _Mask;
			
				float4 frag(v2f IN) : COLOR {
					half4 c = tex2D (_MainTex, IN.uv);
					c.a *= tex2D(_Mask, IN.uv2).a;
					return c;
				}
			ENDCG
		}}
	}
}