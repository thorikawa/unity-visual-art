Shader "Custom/mandelbulbDraw" {
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "Queue" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard vertex:vert addshadow
        #pragma instancing_options procedural:setup
        
        struct Input
        {
            float2 uv_MainTex;
        };

        #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
        StructuredBuffer<uint> _CountMap;
        #endif

        // sampler2D _MainTex; // テクスチャ
        half   _Glossiness;
        half   _Metallic;
        fixed4 _Color;
        float3 _ObjectScale;

        // 頂点シェーダ
        void vert(inout appdata_full v)
        {
            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED

            uint count = _CountMap[unity_InstanceID];
            // !!!!Depends on the size
            uint x = (float)((unity_InstanceID >> 16) & 0xff);
            uint y = (float)((unity_InstanceID >>  8) & 0xff);
            uint z = (float)((unity_InstanceID >>  0) & 0xff);
            float3 pos = float3(x - 128.0, y - 128.0, z - 128.0);
            pos = pos / 2.0f;
            float3 scl = _ObjectScale;

            // オブジェクト座標からワールド座標に変換する行列を定義
            float4x4 object2world = (float4x4)0; 
            // スケール値を代入
            object2world._11_22_33_44 = float4(scl.xyz, 1.0);
            // 行列に位置（平行移動）を適用
            object2world._14_24_34 += pos.xyz;

            // 頂点を座標変換
            v.vertex = mul(object2world, v.vertex);
            // 法線を座標変換
            v.normal = normalize(mul(object2world, v.normal));
            #endif
        }
        
        void setup()
        {
        }

        // サーフェスシェーダ
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            uint count = _CountMap[unity_InstanceID];
            if (count > 0) {
                discard;
            }
            float c = (float)count/24.0;
            fixed4 col = fixed4(1.0 - c, 0.0, 0.0, 1.0 - c);
            #else
            fixed4 col = fixed4(1, 0, 0, 1);
            #endif
            o.Albedo = col.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
