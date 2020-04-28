Shader "Costum/WallShaderNew"
{
    Properties
    {
        [HDR]_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
        [HDR]_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
        [HDR]_RimColor("Rim Color", Color) = (1,1,1,1)
        [HDR]_LightColor("Light Color", Color)= (0.1,0.1,0.1,1)
        _RimAmount("Rim Amount", Range(0,1))=0.716
        _RimTreshold("Rim Treshold", Range(0,1))=0.1
        _Glossiness("Glossiness",Float)=32
        _NormalMap("Normal Map", 2D)= "normal"
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags
            {
                "LightMode" = "UniversalForward"
	            "PassFlags" = "OnlyDirectional"
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog

            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                float2 uv : TEXCOORD0;
                float3 viewDir:TEXCOORD1;
                SHADOW_COORDS(2)
                UNITY_FOG_COORDS(3)
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                o.uv = v.uv;
                TRANSFER_SHADOW(o)
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            CBUFFER_START(Unity_Per_Material)
            float4 _NormalMap_ST;
            sampler2D _NormalMap;
            float4 _AmbientColor;
            float _Glossiness;
            float4 _SpecularColor;
            float _RimAmount;
            float4 _RimColor;
            float4 _LightColor;
            float _RimTreshold;
            CBUFFER_END

            fixed4 frag (v2f i) : SV_Target
            {


                float2 uv_NormalMap = TRANSFORM_TEX(i.uv,_NormalMap);
                float3 normal = UnpackNormal(tex2D(_NormalMap, uv_NormalMap));
                i.worldNormal = normal;


                float NdotL = dot(_WorldSpaceLightPos0, normal);
                float lightIntensity = smoothstep(0,0.01,NdotL);
                float4 light = lightIntensity*_LightColor;

                float3 viewDir =normalize(i.viewDir);

                float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
                float NdotH = dot(normal, halfVector);

                float specularIntensity = pow(NdotH* lightIntensity, _Glossiness*_Glossiness);
                float specularIntensitySmooth = smoothstep(0.005,0.01,specularIntensity);
                float4 specular = specularIntensitySmooth *_SpecularColor;

                float4 rimDot =1-dot(viewDir,normal);
                float rimIntensity = rimDot*pow(NdotL,_RimTreshold);
                rimIntensity = smoothstep(_RimAmount -0.01,_RimAmount+0.01,rimIntensity);
                float4 rim = rimIntensity*_RimColor;

                float4 color =(_AmbientColor + light + specular+rim);
                UNITY_APPLY_FOG(i.fogCoord,color);

                return color;
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
