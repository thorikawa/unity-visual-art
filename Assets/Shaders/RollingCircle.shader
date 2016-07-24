Shader "Custom/Circle" {
    Properties {
    	_MainTex ("Texture", 2D) = "white" { }
    }
    SubShader {
    	Tags {
            "Queue"           = "Transparent"
            "RenderType"      = "Transparent"
        }
        Pass {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha 

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define PI 3.14159

            #include "UnityCG.cginc"

            struct v2f {
                float4 position : SV_POSITION;
                float2 uv       : TEXCOORD0;
            };

            float4 _MainTex_ST;

            v2f vert(appdata_base v) {
                v2f o;
			    o.position = mul (UNITY_MATRIX_MVP, v.vertex);
			    o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
			    return o;
            }

            fixed4 frag(v2f i) : COLOR {
            	float x2 = 2.0 * (i.uv.x - 0.5) / _CosTime.w;
            	float2 nuv = float2(x2 / 2.0 + 0.5, i.uv.y);
            	half4 texcol = float4(0.0, 0.5, 1.0, 1.0);
                float dist = distance(nuv, float2(0.5, 0.5));
                if (dist < 0.50 && dist > 0.48) {
                	texcol.r = dist;
                } else {
                	texcol.a = 0.0;
                }
                return texcol;
            }
            ENDCG
        }
    }
    Fallback "VertexLit"
}