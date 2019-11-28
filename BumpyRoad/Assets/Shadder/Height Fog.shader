Shader "Biyo Studios/Height Fog"
{
	Properties
	{
		_MainColor("Main Color", Color) = (0.2,0.8,0.2,1)
		_FogColor("Fog Color", Color) = (0.9,0.9,0.7,1)

		_FogStartHeight("Fog Start Height", Range(-50, 50)) = 0
		_FogFadeHeight("Fog Fade Height", Range(0.01, 500)) = 1

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 worldPosition : TEXCOORD0;
			};


			fixed4 _MainColor;
			fixed4 _FogColor;
			float _FogStartHeight;
			float _FogFadeHeight;

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				// Only really need world y but just in case I'm using the full world position
				o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// figures out the t for lerp then clamps (saturate) from 0 to 1
				float t = saturate(((i.worldPosition.y) - _FogStartHeight) / _FogFadeHeight);
				// for full shader replace _MainColor with the textured and shaded color
				fixed4 color = lerp(_FogColor, _MainColor, t);
				return color;
			}
			ENDCG
		}
	}
}