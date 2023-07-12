// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Waoter"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_TextureScale("Texture Scale", Range( 0 , 5)) = 0
		_ScrollSpeed("Scroll Speed", Vector) = (0,0,0,0)
		_Float1("Float 1", Range( 0 , 5)) = 0.5882353
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustomLighting keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float4 vertexToFrag16;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform sampler2D _MainTex;
		uniform float _TextureScale;
		uniform float2 _ScrollSpeed;
		uniform float _Float1;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float3 indirectDiffuse23 = ShadeSH9( float4( ase_worldNormal, 1 ) );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult14 = dot( ase_worldlightDir , ase_worldNormal );
			float clampResult24 = clamp( dotResult14 , 0.0 , 1.0 );
			float4 lerpResult25 = lerp( float4( indirectDiffuse23 , 0.0 ) , ase_lightColor , clampResult24);
			o.vertexToFrag16 = lerpResult25;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float3 ase_worldPos = i.worldPos;
			float2 appendResult27 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_output_28_0 = ( appendResult27 * _TextureScale );
			float mulTime31 = _Time.y * _ScrollSpeed.x;
			float mulTime33 = _Time.y * _ScrollSpeed.y;
			float2 appendResult35 = (float2(mulTime31 , mulTime33));
			float mulTime37 = _Time.y * ( _ScrollSpeed.x * -1.0 );
			float mulTime38 = _Time.y * ( _ScrollSpeed.y * -1.0 );
			float2 appendResult39 = (float2(mulTime37 , mulTime38));
			float cos47 = cos( _Float1 );
			float sin47 = sin( _Float1 );
			float2 rotator47 = mul( temp_output_28_0 - float2( 0.5,0.5 ) , float2x2( cos47 , -sin47 , sin47 , cos47 )) + float2( 0.5,0.5 );
			float4 lerpResult43 = lerp( tex2D( _MainTex, ( temp_output_28_0 + appendResult35 ) ) , tex2D( _MainTex, ( appendResult39 + ( rotator47 * ( _TextureScale * 1.9 ) ) ) ) , 0.5);
			c.rgb = ( lerpResult43 * i.vertexToFrag16 * ase_lightAtten ).rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18934
0;695;1556;344;3145.131;1122.294;1.568461;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;26;-2534.836,-1104.91;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;27;-2202.925,-1068.847;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-2340.26,-943.4055;Inherit;False;Property;_TextureScale;Texture Scale;1;0;Create;True;0;0;0;False;0;False;0;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;34;-2393.118,-644.8354;Inherit;False;Property;_ScrollSpeed;Scroll Speed;2;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;48;-2072.564,-851.6182;Inherit;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;49;-1999.532,-703.7621;Inherit;False;Property;_Float1;Float 1;3;0;Create;True;0;0;0;False;0;False;0.5882353;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-1534.415,-302.7714;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-1535.716,-196.1715;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1876.55,-1055.083;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;12;-2281.25,-58.94609;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleTimeNode;33;-2173.828,-542.599;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;31;-2171.828,-635.5991;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-1674.415,-684.2438;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;38;-1350.599,-203.6671;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;13;-2279.904,142.8495;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleTimeNode;37;-1348.599,-296.6671;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;47;-1708.071,-856.5754;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-1441.241,-517.2211;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;35;-1482.89,-637.9888;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DotProductOpNode;14;-1954.339,48.67833;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;39;-1148.458,-261.2874;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;23;-1694.423,354.6305;Inherit;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexturePropertyNode;10;-1881.053,-464.6261;Inherit;True;Property;_MainTex;Main Texture;0;0;Create;False;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-1116.622,-643.1934;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LightColorNode;22;-1654.003,-39.95687;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.ClampOpNode;24;-1657.872,153.9117;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-1064.606,-488.7265;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-574.1482,69.97034;Inherit;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-679.0912,-425.1011;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;36;-680.7713,-203.9942;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;25;-1324.067,135.4385;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;43;-248.9064,-184.4266;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightAttenuation;21;-974.9552,228.6586;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexToFragmentNode;16;-981.5906,39.61505;Inherit;False;False;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;107.0603,-91.53888;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;18;494.246,-266.073;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;Waoter;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;27;0;26;1
WireConnection;27;1;26;3
WireConnection;40;0;34;1
WireConnection;41;0;34;2
WireConnection;28;0;27;0
WireConnection;28;1;29;0
WireConnection;33;0;34;2
WireConnection;31;0;34;1
WireConnection;46;0;29;0
WireConnection;38;0;41;0
WireConnection;37;0;40;0
WireConnection;47;0;28;0
WireConnection;47;1;48;0
WireConnection;47;2;49;0
WireConnection;45;0;47;0
WireConnection;45;1;46;0
WireConnection;35;0;31;0
WireConnection;35;1;33;0
WireConnection;14;0;12;0
WireConnection;14;1;13;0
WireConnection;39;0;37;0
WireConnection;39;1;38;0
WireConnection;30;0;28;0
WireConnection;30;1;35;0
WireConnection;24;0;14;0
WireConnection;42;0;39;0
WireConnection;42;1;45;0
WireConnection;11;0;10;0
WireConnection;11;1;30;0
WireConnection;36;0;10;0
WireConnection;36;1;42;0
WireConnection;25;0;23;0
WireConnection;25;1;22;0
WireConnection;25;2;24;0
WireConnection;43;0;11;0
WireConnection;43;1;36;0
WireConnection;43;2;44;0
WireConnection;16;0;25;0
WireConnection;17;0;43;0
WireConnection;17;1;16;0
WireConnection;17;2;21;0
WireConnection;18;13;17;0
ASEEND*/
//CHKSM=6121D5F8566132C5FCE748F52101C73EED060F7A