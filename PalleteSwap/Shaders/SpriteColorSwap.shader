// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "2D Custom/SpriteColorSwaper" 
{
	Properties
	{
			_MainTex ("Base (RGB)", 2D) = "white" {}
			_Color("Tint Color", Color) = (1.0, 1.0, 1.0, 1.0)
			_ColorBlend("Tint Blend", Range(0.0, 1.0)) = 0.0
			_GrayScaleBlend("Grayscale Blend", Range(0.0, 1.0)) = 0.0
           //_Color("Sprite Color", Color) = (1.0,1.0,1.0,1.0)

			[Header(Color Replacing)]
			//_ReplaceColor0("Seek Color 0", Color) = (1.0, 1.0, 1.0, 1.0)
			//_ReplaceColor1("Seek Color 1", Color) = (1.0, 1.0, 1.0, 1.0)
			//_OutputColor0("Replaced Color 0", Color) = (1.0, 1.0, 1.0, 1.0)
			//_OutputColor1("Replaced Color 1", Color) = (1.0, 1.0, 1.0, 1.0)

			_ReplaceThreshold("Replace Threshold", float) = 0.1

			[Header(Outline)]
			[MaterialToggle]_OutlineActive("Outline Active", Float) = 1.0
			_OutlineSpread ("Outline Spread", Range(0, 2.0)) = 0.007
			_OutlineColor ("Outline Color", color) = (1, 1, 1, 1)
			_OutlineBlend ("Outline Blend", Range(0, 1.0)) = 0.0
	}

	//Base layer
	Subshader
	{
		Tags
		{
			"Queve" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True" 
		}

		Cull Off 
		Lighting Off
		ZWrite Off
		Fog {Mode Off}
		Blend One OneMinusSrcAlpha

		//Outline Pass
		Pass
		{
			Cull Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color;
				return o;
			}

			sampler2D _MainTex;
			float2 _MainTex_TexelSize;
			float4 _OutlineColor;
			float _OutlineSpread;
			float _OutlineBlend;
			fixed _OutlineActive;

			float Outline(float2 uv, float size)
			{
				float2 Disc[8] =
				{
					float2(0, 1),
					float2(1, 1),
					float2(1, 0),
					float2(1, -1),
					float2(0, -1),
					float2(-1, -1),
					float2(-1, 0),
					float2(-1, 1)

					//float2(0, 1),
					//float2(0.3826835, 0.9238796),
					//float2(0.7071069, 0.7071068),
					//float2(0.9238796, 0.3826834),
					//float2(1, 0),
					//float2(0.9238795, -0.3826835),
					//float2(0.7071068, -0.7071068),
					//float2(0.3826833, -0.9238796),
					//float2(0, -1),
					//float2(-0.3826835, -0.9238796),
					//float2(-0.7071069, -0.7071067),
					//float2(-0.9238797, -0.3826832),
					//float2(-1, 0),
					//float2(-0.9238795, 0.3826835),
					//float2(-0.7071066, 0.707107),
					//float2(-0.3826834, 0.9238796)
				};

				float maxAlpha = 0;

				for (int d = 0; d < 8; d++)
				{
					float sampleAlpha = tex2D(_MainTex, uv + Disc[d] * _MainTex_TexelSize * size).a;
					maxAlpha = max(sampleAlpha, maxAlpha);
				}

				return maxAlpha;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				if (_OutlineActive < 0.5) discard;
				fixed4 sourceColor = tex2D(_MainTex, i.uv);
				fixed4 col = sourceColor;
				
				col.a = max(col.a, (Outline(i.uv, _OutlineSpread)));
				if (col.a < 0.15)
				{
					discard;
				}

				col.rgb = lerp(_OutlineColor, col.rgb * i.color.rgb, _OutlineBlend).rgb;
				return col;
			}
			ENDCG
		}

		//Default Pass
		Pass
		{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
				float2 texcoord: TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				fixed4 color : COLOR;
				half2 texcoord : TEXCOORD0;
			};

			fixed4 _Color;
			fixed _ColorBlend;
			float PixelSnap;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;// * _Color;

				#if PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
				#endif

				if(PixelSnap > 0.1)
					OUT.vertex = UnityPixelSnap(OUT.vertex);
				return OUT;
			}

			sampler2D _MainTex;
			fixed _GrayScaleBlend;

			fixed4 _SeekColors[10];
			fixed4 _OutputColors[10];

			fixed _ReplaceThreshold;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 color = tex2D(_MainTex, IN.texcoord);// *IN.color;
				fixed4 sourceColor = color;

				if(color.a < 0.15)
				{
					discard;
				}

				float seeking = 1.0;
				for(int i = 0; i < 10; i++)
				{
					if(seeking > 0.5 && _SeekColors[i].a > 0.9 && _OutputColors[i].a > 0.9 &&
					(abs(color.r - _SeekColors[i].r) + 
					abs(color.g - _SeekColors[i].g) +
					abs(color.b - _SeekColors[i].b) < _ReplaceThreshold))
					{
						color = _OutputColors[i];
						seeking = 0.0;
					}
				}

				color *= IN.color;

				if(_GrayScaleBlend > 0)
				{
					float luminosity = (0.299 * color.r) + (0.597 * color.g) + (0.114 * color.b);
					fixed3 gray = lerp(color, luminosity, 1);
	
					color.r = lerp(color.r, gray.r, _GrayScaleBlend);
					color.g = lerp(color.g, gray.g, _GrayScaleBlend);
					color.b = lerp(color.b, gray.b, _GrayScaleBlend);
				}

				fixed4 c;
				c.rgb = lerp(color.rgb, _Color.rgb, _ColorBlend) * color.a;
				c.a = sourceColor.a * IN.color.a;
				c.rgb *= c.a;

				return c;
			}

			ENDCG

		}

	}

}




