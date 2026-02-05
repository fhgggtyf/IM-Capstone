Shader "Unlit/NoiseRadialFade"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,0.25)
        _FadeStart ("Fade Start (0¨C1)", Range(0,1)) = 0.7
        _FadeEnd   ("Fade End (0¨C1)", Range(0,1)) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float _FadeStart;
            float _FadeEnd;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Distance from center (0 at center, ~1 at edge)
                float2 p = i.uv - float2(0.5, 0.5);
                float d = length(p) * 2.0;

                // Alpha is fully opaque inside FadeStart,
                // then smoothly fades to 0 at FadeEnd
                float edgeFade = 1.0 - smoothstep(_FadeStart, _FadeEnd, d);

                return fixed4(_Color.rgb, _Color.a * edgeFade);
            }
            ENDCG
        }
    }
}
