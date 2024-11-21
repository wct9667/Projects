Shader "Custom/EquirectangularURP_Rotatable" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Diffuse (RGB) Alpha (A)", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" "LightMode"="UniversalForward" }
        Pass {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
            };

            // Properties
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _Color;

            Varyings vert(Attributes v) {
                Varyings o;
                // Transform vertex position to clip space
                o.positionHCS = TransformObjectToHClip(v.positionOS);
                // Transform normal to world space to handle object rotation
                o.normalWS = TransformObjectToWorldDir(v.normalOS);
                return o;
            }

            #define PI 3.141592653589793

            float2 RadialCoords(float3 coords) {
                float3 normalizedCoords = normalize(coords);
                float lon = atan2(normalizedCoords.z, normalizedCoords.x);
                float lat = acos(normalizedCoords.y);
                float2 sphereCoords = float2(lon, lat) * (1.0 / PI);
                return float2(sphereCoords.x * 0.5 + 0.5, 1 - sphereCoords.y);
            }

            float4 frag(Varyings IN) : SV_Target {
                // Compute equirectangular UV coordinates
                float2 equiUV = RadialCoords(IN.normalWS);
                // Sample the texture and apply the color
                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, equiUV);
                return texColor * _Color;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}
