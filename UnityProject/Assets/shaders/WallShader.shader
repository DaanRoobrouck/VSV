Shader "Custom/WallShader"
{
    Properties
    {
     //  [HDR] _Color("Color", Color) = (1,1,1,1)
       _BumpMap("Normal Map", 2D) = "bump" {}

        _Specular("Intensity Specular", Color) = (1,1,1,1)
        _RatioSpecular("Ratio Specular", Range(0,1)) = 1


        _AmbientColor("AmbientColor", Color) = (1,1,1,1)
        _AmbientIntensity("Ambient Intensity", Range(0,1)) = 0


        _DiffuseColor("DiffuseColor", Color) = (1,1,1,1)
        _DiffuseIntensity("Diffuse Intensity", Range(0,1)) = 0

        Shiness("Shininess", Range(0,1)) = 0
    }
        SubShader
        {
            Tags
            {
                "RenderType" = "Opaque"
                "RenderPipeline" = "UniversalPipeline"
                "IgnoreProjector" = "true"
                "Queue" = "Geometry"
            }
            Pass
            {
                Tags
                {
                    "LightMode" = "UniversalForward"
                }
                HLSLPROGRAM
                #pragma prefer _hlslcc gles
                #pragma exclude _renderers d3d11_9x

                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                #pragma vertex vert
                #pragma fragment frag



              CBUFFER_START(UnityPerMaterial)

            float4 _Specular;
            float _RatioSpecular;
            
            float4 _BumpMap_ST;
            sampler2D _BumpMap;

            float4 _AmbientColor;
            float _AmbientIntensity;

            float4 _DiffuseColor;
            float _DiffuseIntensity;

            float Shiness;
    

         CBUFFER_END


          //TEXTURE2D(_BumpMap);
          //SAMPLER(sampler_BumpMap);

          //    Attributes, struct voor vert shader

           struct Attributes
           {
             float4 positionOS : POSITION0;
             float3 color : COLOR0;
             float3 normalWS: NORMAL0;
             float2 uv :TEXCOORD2;
           };

           //Attributs, struct voor frag shader

           struct Varyings  
           {
                float4 positionCS : SV_POSITION;
                float3 positionWS: TEXCOORD0;
                float3 normalWS: TEXCOORD1;
                float3 color : COLOR0;
                float3 normal : NORMAL0;
                float2 uv :TEXCOORD2;
                
           };

            
            //Input Attributs, output Varyings
            // I is in, O is out

            Varyings vert(Attributes i)
            {
                Varyings o;

                o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
                o.color = i.color;

       //        // float3 wordt geconvert naar een float4
       //
              o.normalWS = float4( TransformObjectToWorldNormal(i.normalWS.xyz),1);
              o.positionWS = float4(TransformObjectToWorld(i.positionOS.xyz), 1);
              o.uv = i.uv;      //
       //        o.uv = TRANSFORM_TEX(i.texCoord, _BaseMap);
       //        o.uv += _Time.x;
       //

                

                return o;
            }

            //input varyings, output naar material

            float4 frag(Varyings i) : SV_TARGET
            {
                //_AmbientColor* _AmbientIntensity*

                float2 uv_BumpMap = TRANSFORM_TEX(i.uv,_BumpMap);

                float3 normal = UnpackNormal(tex2D(_BumpMap, uv_BumpMap));
                i.normalWS = normal;

                float3 light = _MainLightPosition;

                float3 reflection = 2 * dot(light, i.normalWS) * i.normalWS - light;

                //vector naar camera

                float3 v =  normalize( GetCameraPositionWS() - i.positionWS);


                float3 a = saturate(_AmbientIntensity * _AmbientColor);

                float3 d = saturate(_DiffuseIntensity *  dot(light, i.normalWS) * _DiffuseColor);

                float3 s = saturate(_RatioSpecular * pow(dot(reflection,v), Shiness) * _Specular);

                float3 c = a + d + s;
                float3 b = c - fmod(c, 0.1);


                return float4(b, 1);

            // return SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);

            //  i.PositionCS ligt over scherm gestretcht, moet gedeeld worden door _ScreenParams want wordt behind the screens terug vermenigvuldigd door de afbeeldingswidth
            //  return SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.positionCS / _ScreenParams.xy);
            }

            ENDHLSL
        }
    }
        Fallback "Hidden/Universal Render Pipeline/FallbackError"
}

