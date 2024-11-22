Shader "Custom/Sphere"
{
    Properties
    {
        _MainTex ("Earth Texture", 2D) = "white" {} // The texture for the Earth
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            // Texture sampler
            sampler2D _MainTex;

            // Vertex input structure
            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            // Vertex-to-fragment structure
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // Vertex shader
            v2f vert (appdata_t v)
            {
                v2f o;
                // Transform vertex position to clip space
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Compute UV coordinates for spherical mapping
                float3 normal = normalize(v.normal);
                float u = 0.5 + atan2(normal.z, normal.x) / (2.0 * UNITY_PI);
                float vCoord = 0.5 - asin(normal.y) / UNITY_PI;
                o.uv = float2(u, vCoord);

                return o;
            }

            // Fragment shader
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture using computed UVs
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
