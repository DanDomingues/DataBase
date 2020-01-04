// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "2D Custom/SpriteColorSwapSimple" 
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Tint Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_ColorBlend("Tint Blend", Range(0.0, 1.0)) = 0.0
		_GrayScaleBlend("Grayscale Blend", Range(0.0, 1.0)) = 0.0
		//_Color("Sprite Color", Color) = (1.0,1.0,1.0,1.0)

		_SeekColor("Seek Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_SwapColor("Swap Color", Color) = (1.0, 1.0, 1.0, 1.0)

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

			fixed _Color;
			float PixelSnap;
			fixed _OutlineSpread;

			v2f vert(appdata_t IN)
			{
				v2f OUT;

				//OUT.coords = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(IN.vertex * (_OutlineSpread));
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;/// * _Color;

				#if PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
				#endif

				if(PixelSnap > 0.1)
					OUT.vertex = UnityPixelSnap(OUT.vertex);
				return OUT;
			}

			sampler2D _MainTex;
			fixed4 _OutlineColor;
			//fixed4 _Color;

			fixed _OutlineActive;
			fixed _OutlineBlend;

			fixed4 frag(v2f IN) : SV_Target
			{
				if(_OutlineActive < 0.5) discard;

				fixed4 baseColor = tex2D(_MainTex, IN.texcoord);
				baseColor.rgb * IN.color;
				fixed alphaLimit = 0.01;
				if(baseColor.a < 0.15)
				{
					discard;
				}

				fixed4 c;
				c.rgb = lerp(_OutlineColor.rgb, baseColor, _OutlineBlend);
				c.a = _OutlineColor.a * baseColor.a;
				//c.rgb *= c.a;
				return c;
			}
			ENDCG

		}
		

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
			fixed4 _SeekColor;
			fixed4 _SwapColor;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 color = tex2D(_MainTex, IN.texcoord);
				if(abs(color.r - _SeekColor.r) + abs(color.g - _SeekColor.g) + abs(color.b - _SeekColor.b) < 0.15)
				{
					color = _SwapColor;
				}
				color *= IN.color;

				if(color.a < 0.15)
				{
					discard;
				}

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
				c.a = color.a;

				return c;
			}

			ENDCG

		}

	}

}




