// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "TWEWB/WallsShader"{
	Properties{
		_Color("Tint", Color) = (0, 0, 0, 1)
		_AlphaBellow("Alpha Bellow Character",  Range(0.0, 1.0)) = 0.5
		_CurrentPlayerHeight("Current Player Height", float) = 0
		_CurrentWallHeight("Current Player Height", float) = 2
		_MainTex("Texture", 2D) = "white" {}
	}

		SubShader{
			Tags{
				"RenderType" = "Transparent"
				"Queue" = "Transparent"
			}

			Blend SrcAlpha OneMinusSrcAlpha

			ZWrite off
			Cull off

			Pass{

				CGPROGRAM

				#include "UnityCG.cginc"

				#pragma vertex vert
				#pragma fragment frag

				sampler2D _MainTex;
				float4 _MainTex_ST;

				fixed4 _Color;
				float _AlphaBellow;
				float _CurrentPlayerHeight;
				float _CurrentWallHeight;

				struct appdata {
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					fixed4 color : COLOR;
				};

				struct v2f {
					float4 position : SV_POSITION;
					float2 uv : TEXCOORD0;
					bool willDisplay : BOOL;
					fixed4 color : COLOR;
				};

				v2f vert(appdata v) {
					v2f o;
					o.position = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
					o.color = v.color;

					if(o.uv.y > 0.8)
					{
						o.willDisplay = worldPos.y - _CurrentWallHeight < _CurrentPlayerHeight;
					}
					else {
						o.willDisplay = worldPos.y  < _CurrentPlayerHeight;
					}
					return o;
				}

				fixed4 frag(v2f i) : SV_TARGET{

					fixed4 col = tex2D(_MainTex, i.uv);
					col *= _Color;
					col *= i.color;

					
					if (i.willDisplay) {
						col.a = _AlphaBellow;
					}
					else {
						col.a = 1;
					}
					
					/*
					if (i.worldPos.y < _CurrentPlayerHeight) {
						col.a = 0.2;
					}
					else {
						col.a = 1;
					}
					*/
					return col;
				}

				ENDCG
			}
	}
}