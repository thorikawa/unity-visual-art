// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Per pixel bumped refraction.
// Uses a normal map to distort the image behind, and
// an additional texture to tint the color.

Shader "Custom/Reflaction" {
Properties {
}

Category {

	// We must be transparent, so other objects are drawn before this one.
	Tags { "Queue"="Transparent" "RenderType"="Opaque" }


	SubShader {

		// This pass grabs the screen behind the object into a texture.
		// We can access the result in the next pass as _GrabTexture
		GrabPass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
		}
		
		// Main pass: Take the texture grabbed above and use the bumpmap to perturb it
		// on to the screen
		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
			
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fog
#include "UnityCG.cginc"
#include "ClassicNoise3D.cginc"

struct appdata_t {
	float4 vertex : POSITION;
	float2 texcoord: TEXCOORD0;
};

struct v2f {
	float4 vertex : SV_POSITION;
	float4 uvgrab : TEXCOORD0;
	float2 uvbump : TEXCOORD1;
	float2 uv      : TEXCOORD2;
	UNITY_FOG_COORDS(3)
};

v2f vert (appdata_t v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	// maybe change scale (-w, w) to (0, w)??
	#if UNITY_UV_STARTS_AT_TOP
	float scale = -1.0;
	#else
	float scale = 1.0;
	#endif
	o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
	o.uvgrab.zw = o.vertex.zw;
	o.uv     = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
	UNITY_TRANSFER_FOG(o, o.vertex);
	return o;
}

sampler2D _GrabTexture;
float4 _GrabTexture_TexelSize;

half4 frag (v2f i) : SV_Target
{
	// calculate perturbed coordinates
	float3 seed = float3(i.uv.x, i.uv.y, _Time.y);
	float noise = cnoise(seed * 1.2);
	i.uvgrab.xy = (noise * 0.6) + i.uvgrab.xy;
	
//	half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab) + noise);
	half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
	UNITY_APPLY_FOG(i.fogCoord, col);
	return col;
}
ENDCG
		}
	}
}

}
