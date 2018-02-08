Shader "Legacy Shaders/Lightmapped/SpecLightDiffuse" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
    _Shininess ("Shininess", Range (0.03, 1)) = 0.078125
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _LightMap ("Lightmap (RGB)", 2D) = "black" {}
}
 
SubShader {
    LOD 200
    Tags { "RenderType" = "Opaque" }
CGPROGRAM
#pragma surface surf BlinnPhong
struct Input {
  float2 uv_MainTex;
  float2 uv2_LightMap;
};
sampler2D _MainTex;
sampler2D _LightMap;
float4 _Color;
float _Shininess;
 
void surf (Input IN, inout SurfaceOutput o)
{
  half4 tex = tex2D (_MainTex, IN.uv_MainTex);
  o.Albedo = tex.rgb * _Color;
  half4 lm = tex2D (_LightMap, IN.uv2_LightMap);
  o.Emission = lm.rgb*o.Albedo.rgb;
  o.Gloss = tex.a;
  o.Alpha = lm.a * _Color.a;
  o.Specular = _Shininess;
}
ENDCG
}
FallBack "Legacy Shaders/Lightmapped/VertexLit"
}
 