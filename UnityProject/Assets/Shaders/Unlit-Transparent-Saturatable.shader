Shader "Custom/Unlit-Transparent-Saturatable" {
  Properties {
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _EffectAmount ("_EffectAmount", float) = 0
    }
 Category
    {
        ZWrite Off
    	SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 200
 
        CGPROGRAM
        #pragma surface surf Lambert alpha
            sampler2D _MainTex;
       		float _EffectAmount;
 
            struct Input {
                float2 uv_MainTex;
            };
 
            void surf (Input IN, inout SurfaceOutput o) {
                half4 c = tex2D(_MainTex, IN.uv_MainTex);
                o.Albedo = lerp(c.rgb, dot(c.rgb, float3(0.3, 0.59, 0.11)), _EffectAmount);
                o.Alpha = c.a;
            }
        ENDCG
    	}
    }
    Fallback "Unlit/Transparent"
}