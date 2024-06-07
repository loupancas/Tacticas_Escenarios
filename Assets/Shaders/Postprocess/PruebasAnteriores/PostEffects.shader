Shader "Main/PostEffects"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    CGINCLUDE
        #include "UnityCG.cginc"

        struct appdata  
        {
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};
        struct v2f
        {
			float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
		};
        sampler2D _MainTex;
		v2f vert (appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			return o;
		}
    ENDCG

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        { 			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col*fixed4(1,0,0,1);
			}
			ENDCG
		}        
    }    
}
