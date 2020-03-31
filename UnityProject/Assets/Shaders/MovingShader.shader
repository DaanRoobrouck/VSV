Shader "Unlit/CustomShader"
{

    Properties
    {
		//[HDR]_Color("Color", Color) = (1,1,1,1)


       _BaseMap ("Texture", 2D) = "white" {}
		
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"
				"RenderPipeline" = "UniversalPipeline"
				"IgnoreProjector" = "true"
				"Queue" = "Geometry" //welke volgorde voert unity deze uit
				}
        LOD 100

        Pass
        {
		Tags{
			"Lightmode" = "UniversalForward"
		}
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma excluse_renderers d3d11_9x

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
  
            #pragma vertex vert
			#pragma fragment frag

			//unload de shader niet helemaal, verandert de kleur gewoon
			CBUFFER_START(unityPerMaterial)
			//float4 _Color;
			float4 _BaseMap_ST;
			CBUFFER_END

			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_BaseMap);

			struct Attributes
			{
				float4 positionOS : POSITION0; //position van model
				float4 color : COLOR0;
				float4 normal : NORMAL0;
				float4 texCoord : TEXCOORD0;
			};
			struct Varyings
			{
				float4 positionCS : SV_POSITION; //position die shader verwacht System Value
				float4 color : COLOR0;
				float4 normal : NORMAL0;
				float2 uv : TEXCOORD0;

			};

			Varyings vert( Attributes i/*float4 objectPos: POSITION0, float4 objColor : COLOR0*/)
			{
				Varyings o;
				o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
				o.color = i.color;
				o.normal = float4(TransformObjectToWorldNormal(i.normal.xyz),1.0);
				o.uv = TRANSFORM_TEX(i.texCoord, _BaseMap);
				o.uv.x += _SinTime.x * 0.3;

				return o;
			}

			float4 frag( Varyings i/*float4 hClip: SV_POSITION*/) : SV_TARGET
			{

				//return float4(_Color) * 300;
				//return i.normal;
				return SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, /*i.positionCS/_ScreenParams.xy*/ i.uv);
			}
            ENDHLSL
        }
    }
	Fallback "Hidden/Universal Render Pipeline/FallbackError"
}
