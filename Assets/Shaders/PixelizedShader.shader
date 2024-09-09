Shader "Unlit/PixelizedShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelSize ("Pixel Size", Float) = 8.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 80

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _PixelSize;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Calculate the pixel size in UV space
                float2 pixelSize = float2(_PixelSize / _ScreenParams.x, _PixelSize / _ScreenParams.y);

                // Snap the UV coordinates to the nearest "pixel"
                float2 coord = i.texcoord;
                coord.x = floor(coord.x / pixelSize.x) * pixelSize.x + pixelSize.x * 0.5;
                coord.y = floor(coord.y / pixelSize.y) * pixelSize.y + pixelSize.y * 0.5;

                // Sample the texture at the snapped coordinates
                half4 col = tex2D(_MainTex, coord);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}



    /*
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelSize ("Pixel Size", Float) = 8.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 80

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _PixelSize;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Calculate the pixel size in UV space
                float2 pixelSize = float2(_PixelSize / _ScreenParams.x, _PixelSize / _ScreenParams.y);

                // Snap the UV coordinates to the nearest "pixel"
                float2 coord = floor(i.texcoord / pixelSize) * pixelSize + pixelSize * 0.5;

                // Sample the texture at the snapped coordinates
                half4 col = tex2D(_MainTex, coord);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"*/
