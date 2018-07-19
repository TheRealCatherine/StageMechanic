// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

 Shader "UMotion Editor/Unlit Transparent Cutout"
 {
	 Properties
	 {
	     _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	 }
	 
	 SubShader
	 {
	     Tags { "RenderType"="Opaque" "IgnoreProjector"="True" }
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
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 c = tex2D(_MainTex, i.uv) * i.color;
                clip(c.a - 0.5);

                return c;
            }
            ENDCG
         }
	 }
}