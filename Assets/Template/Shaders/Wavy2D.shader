Shader "Unlit/Wavy2D" {
	Properties {
		_BaseMap ("Example Texture", 2D) = "white" {}
		_Color ("Colour", Color) = (0, 0.66, 0.73, 1)
		_Intensity ("Intensity", float) = 0.3
		_Smoothness ("Smoothness", float) = 3
		_Speed ("Speed", float) = 0.1
		_xOffsetIntensity ("XOffsetAmount", float) = 0.1
		//_ExampleVector ("Example Vector", Vector) = (0, 1, 0, 0)
		//_ExampleFloat ("Example Float (Vector1)", Float) = 0.5
	}
	SubShader {
		Tags {
			"RenderPipeline"="UniversalPipeline"
			"RenderType"="Opaque"
			"Queue"="Geometry"
		}

		Cull Off

		HLSLINCLUDE
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		#define TAU 6.28318530718

		CBUFFER_START(UnityPerMaterial)
		float4 _BaseMap_ST;
		float4 _Color;
		float _Intensity;
		float _Smoothness;
		float _Speed;
		float _xOffsetIntensity;
		CBUFFER_END

		ENDHLSL

		Pass {

			HLSLPROGRAM
			#pragma vertex UnlitPassVertex
			#pragma fragment UnlitPassFragment

			// Structs
			struct VertexInput {
				float4 positionOS	: POSITION;
				float2 uv		: TEXCOORD0;
				float4 color		: COLOR;
			};

			struct FragmentInput {
				float4 positionCS 	: SV_POSITION;
				float2 uv		: TEXCOORD0;
				float4 color		: COLOR;
			};

			// Textures, Samplers & Global Properties
			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_BaseMap);

			// Vertex Shader
			FragmentInput UnlitPassVertex(VertexInput IN) {
				FragmentInput OUT;

    			VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
    			OUT.positionCS = positionInputs.positionCS;
				OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
				OUT.color = IN.color;
				return OUT;
			}

			// Fragment Shader
			half4 UnlitPassFragment(FragmentInput IN) : SV_Target {
				half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
				clip(baseMap.a - 0.0001);

				if(_Intensity == 0){
					return baseMap;
				}

				half xOffset = cos(IN.uv.x * TAU * 8) * _xOffsetIntensity;
				float effectOnPixel = -_Time.y * _Speed + IN.uv.y + xOffset;

				half4 t = cos(effectOnPixel * TAU * _Smoothness) * 0.5 + 0.5;

				baseMap += t * _Color * _Intensity;

				return baseMap;
			}
			ENDHLSL
		}
	}
}