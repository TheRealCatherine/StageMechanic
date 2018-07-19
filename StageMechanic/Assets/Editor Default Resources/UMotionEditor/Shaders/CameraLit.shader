// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UMotion Editor/Camera Lit"
{
	Properties
	{
	    _Color("Main Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "IgnoreProjector"="True" }
		Lighting Off
		LOD 100

	    Pass
	    {
		    CGPROGRAM
		    #pragma vertex vert
		    #pragma fragment frag
		    #include "UnityCG.cginc"

		    struct v2f
		    {
				float4 pos : SV_POSITION;
				float3 lambert : TEXCOORD1;
			};

		    v2f vert(float4 vertex : POSITION, float3 normal : NORMAL)
		    {
				v2f o;
				o.pos = UnityObjectToClipPos(vertex);
				float3 viewDir = normalize(WorldSpaceViewDir(vertex));
				float3 worldNormal = normalize(UnityObjectToWorldNormal(normal));
				o.lambert = saturate(dot(viewDir, worldNormal));

				return o;
		    }

		    fixed4 _Color;

		    fixed4 frag(v2f i) : SV_Target
		    {
				fixed4 outColor;

				outColor.rgb = i.lambert * _Color;
				outColor.a = 1.0;

				return outColor;
		    }
		    ENDCG
	    }
	}
}