
Shader "Custom/KillLineShader" 
{
	Properties
	{
	_BlackColor("Black Color", Color) = (1,1,1,1)
	_KillLineColor("KillLine Color", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" }

		GrabPass{ "_BackgroundTexture" }

		Pass
		{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

			struct v2f
			{		
				float4 grabPos : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.pos);
				return o;
			}

			sampler2D _BackgroundTexture;
			float4 _BlackColor;
			float4 _KillLineColor;

			half4 frag(v2f i) : SV_Target
			{
				half4 bgcolor = tex2Dproj(_BackgroundTexture, i.grabPos);
				if (any	(bgcolor != _BlackColor))
				return _KillLineColor;

				return bgcolor;
			}
		ENDCG
		}
	}
}