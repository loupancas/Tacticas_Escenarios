Shader "Custom/CyanScreenWithNoise"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseStrength ("Noise Strength", Range(0, 1)) = 0.5
        _ScanLineJitter ("Scanline Jitter", Vector) = (0.2, 0.2, 0, 0)
        _Alpha ("Alpha", Range(0, 1)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        LOD 100

        Pass
        {
            Cull Off ZWrite Off ZTest Always Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #pragma target 3.0

            sampler2D _MainTex;
            float _NoiseStrength;
            float4 _ScanLineJitter;
            float _Alpha;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float randomNoise(float x, float y)
            {
                return frac(sin(dot(float2(x, y), float2(12.9898, 78.233))) * 43758.5453);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Calculate jitter based on noise
                float jitter = randomNoise(i.uv.x + _Time.y, i.uv.y) * 2 - 1;
                jitter *= step(_ScanLineJitter.y, abs(jitter)) * _ScanLineJitter.x;

                // Apply cyan color with noise effect
                fixed4 cyanColor = fixed4(0, 1, 1, 1); // Cyan color
                fixed4 sceneColor = tex2D(_MainTex, i.uv + jitter * _NoiseStrength);

                // Blend cyan color with scene color
                fixed4 finalColor = lerp(sceneColor, cyanColor, 0.5); // Adjust blending factor as needed

                // Apply alpha
                finalColor.a = _Alpha;

                return finalColor;
            }

            ENDCG
        }
    }
    Fallback "Diffuse"
}