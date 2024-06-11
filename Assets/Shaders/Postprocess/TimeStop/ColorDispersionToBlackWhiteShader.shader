Shader "Hidden/Custom/ColorDispersionToBlackWhiteShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorDispersionStrength ("Dispersion Strength", Range(0, 1)) = 0.1
        _ColorDispersionU ("Dispersion U", Range(-1, 1)) = 0.5
        _ColorDispersionV ("Dispersion V", Range(-1, 1)) = 0.5
        _BlackWhiteThreshold ("Black White Threshold", Range(0, 1)) = 0.5
        _BlackWhiteWidth ("Black White Width", Range(0, 1)) = 0.1
        _BlackWhiteWhiteColor ("White Color", Color) = (1, 1, 1, 1)
        _BlackWhiteBlackColor ("Black Color", Color) = (0, 0, 0, 1)
        _EnableBlackWhite ("Enable Black White", Float) = 0
        _Transition ("Transition", Range(0, 1)) = 0.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }
        LOD 100

        Pass
        {
            Cull Off ZWrite Off ZTest Always

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #pragma target 3.0

            sampler2D _MainTex;
            float _ColorDispersionStrength;
            float _ColorDispersionU;
            float _ColorDispersionV;
            float _BlackWhiteThreshold;
            float _BlackWhiteWidth;
            float4 _BlackWhiteWhiteColor;
            float4 _BlackWhiteBlackColor;
            float _EnableBlackWhite;
            float _Transition;

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

            fixed4 BlackWhite(fixed4 color)
            {
                half luminosity = dot(color.rgb, half3(0.299, 0.587, 0.114));
                float smoothstepResult = smoothstep(_BlackWhiteThreshold, _BlackWhiteThreshold + _BlackWhiteWidth, luminosity);
                return lerp(_BlackWhiteWhiteColor, _BlackWhiteBlackColor, smoothstepResult);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                half2 deltaUv = half2(_ColorDispersionStrength * _ColorDispersionU, _ColorDispersionStrength * _ColorDispersionV);

                fixed4 colorDispersionResult;
                fixed4 tempScreenColor;

                // Red channel
                tempScreenColor = tex2D(_MainTex, i.uv + deltaUv);
                colorDispersionResult.r = tempScreenColor.r;

                // Green channel
                tempScreenColor = tex2D(_MainTex, i.uv);
                colorDispersionResult.g = tempScreenColor.g;

                // Blue channel
                tempScreenColor = tex2D(_MainTex, i.uv - deltaUv);
                colorDispersionResult.b = tempScreenColor.b;

                colorDispersionResult.a = 1.0; // Set alpha to 1.0

                fixed4 finalColor = colorDispersionResult;

                // Apply black and white filter if enabled
                if (_EnableBlackWhite > 0.5)
                {
                    fixed4 blackWhiteResult = BlackWhite(colorDispersionResult);
                    // Blend between color dispersion and black and white based on _Transition
                    finalColor = lerp(colorDispersionResult, blackWhiteResult, _Transition);
                }

                return finalColor;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
