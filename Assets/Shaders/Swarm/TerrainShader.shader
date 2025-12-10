Shader "Custom/TerrainShader"
{
    Properties
    {
        _Texture0 ("Texture 0", 2D) = "white" {}
        _Texture1 ("Texture 1", 2D) = "white" {}
        _Texture2 ("Texture 2", 2D) = "white" {}
        _Texture3 ("Texture 3", 2D) = "white" {}
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _Tiling ("Texture Tiling", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _Texture0;
        sampler2D _Texture1;
        sampler2D _Texture2;
        sampler2D _Texture3;
        sampler2D _BumpMap;
        float _Tiling;

        struct Input
        {
            float2 uv_Texture0;
            float3 worldPos;
            float3 worldNormal;
            INTERNAL_DATA
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = IN.uv_Texture0 * _Tiling;
            
            float height = IN.worldPos.y / 30.0;
            float steepness = 1.0 - saturate(dot(float3(0,1,0), WorldNormalVector(IN, float3(0,1,0))));
            
            float4 t0 = tex2D(_Texture0, uv);
            float4 t1 = tex2D(_Texture1, uv);
            float4 t2 = tex2D(_Texture2, uv);
            float4 t3 = tex2D(_Texture3, uv);
            float3 nm = UnpackNormal(tex2D(_BumpMap, uv));
            
            float m1 = smoothstep(0.3, 0.7, height);
            float m2 = smoothstep(0.6, 0.9, height);
            float m3 = smoothstep(0.8, 1.0, steepness);
            
            float4 mixedColor = lerp(t0, t1, m1);
            mixedColor = lerp(mixedColor, t2, m2);
            mixedColor = lerp(mixedColor, t3, m3);
            
            o.Albedo = mixedColor.rgb;
            o.Normal = nm;
            o.Metallic = 0.1;
            o.Smoothness = 0.2;
        }
        ENDCG
    }
    FallBack "Diffuse"
}