// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/slime_moving"
{
	Properties
	{
		_slime_texture("slime_texture", 2D) = "white" {}
		_slime_movement_map("slime_movement_map", 2D) = "white" {}
		_texture_movement_vector("texture_movement_vector", Vector) = (0,0,0,0)
		_smoothness("smoothness", Float) = 0
		_opacity("opacity", Float) = 0
		_time_multiplier("time_multiplier", Float) = 1
		_intensity("intensity", Range( 0 , 1)) = 0.04651547
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			half2 uv_texcoord;
		};

		uniform sampler2D _slime_texture;
		uniform float4 _slime_texture_ST;
		uniform half _smoothness;
		uniform half _opacity;
		uniform sampler2D _slime_movement_map;
		uniform half3 _texture_movement_vector;
		uniform half _time_multiplier;
		uniform half _intensity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_TexCoord14 = v.texcoord.xy * float2( 1,1 ) + ( _texture_movement_vector * ( _Time.y * _time_multiplier ) ).xy;
			float3 ase_vertexNormal = v.normal.xyz;
			float dotResult18 = dot( ( tex2Dlod( _slime_movement_map, half4( uv_TexCoord14, 0, 0.0) ) * _intensity ) , half4( ase_vertexNormal , 0.0 ) );
			half3 temp_cast_2 = (dotResult18).xxx;
			v.vertex.xyz += temp_cast_2;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_slime_texture = i.uv_texcoord * _slime_texture_ST.xy + _slime_texture_ST.zw;
			o.Albedo = tex2D( _slime_texture, uv_slime_texture ).rgb;
			o.Smoothness = _smoothness;
			o.Alpha = _opacity;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14001
29;48;1337;693;1131.69;315.1911;1.158525;True;True
Node;AmplifyShaderEditor.SimpleTimeNode;1;-1278.611,328.4315;Float;False;1;0;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1266.15,460.8711;Float;False;Property;_time_multiplier;time_multiplier;5;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1076.186,333.0096;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;15;-1412.481,-25.12315;Float;False;Property;_texture_movement_vector;texture_movement_vector;2;0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-1044.714,68.21043;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-837.5228,87.53453;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-519.2357,385.4872;Float;False;Property;_intensity;intensity;6;0;0.04651547;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-536.5068,87.71103;Float;True;Property;_slime_movement_map;slime_movement_map;1;0;Assets/SlimeBlock/Resources/slime_move_map.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-38.96407,240.8356;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;17;-1.496537,501.5745;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-142.0648,-4.68486;Float;False;Property;_smoothness;smoothness;3;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-100.4862,98.37105;Float;False;Property;_opacity;opacity;4;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-421.4237,-184.6987;Float;True;Property;_slime_texture;slime_texture;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;18;332.0205,346.0399;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;587.4954,-42.53019;Half;False;True;2;Half;ASEMaterialInspector;0;0;Standard;Custom/slime_moving;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;1;0
WireConnection;5;1;6;0
WireConnection;16;0;15;0
WireConnection;16;1;5;0
WireConnection;14;1;16;0
WireConnection;13;1;14;0
WireConnection;7;0;13;0
WireConnection;7;1;8;0
WireConnection;18;0;7;0
WireConnection;18;1;17;0
WireConnection;0;0;10;0
WireConnection;0;4;11;0
WireConnection;0;9;12;0
WireConnection;0;11;18;0
ASEEND*/
//CHKSM=999FEF68D35A4F362B6E198C835A741D095C23B3