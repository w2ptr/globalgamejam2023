Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #define PI 3.1415926538

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPos = ComputeScreenPos(o.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 screenPoso = i.screenPos.xy;

                // Syncs up with music quite well
                fixed4 timeColor = _Time;
                timeColor.w = 1.0;
                timeColor /= 1.7;
                timeColor = clamp(timeColor, 0.0, 1.0);
                


                screenPoso.y += sin(screenPoso.x*4*PI)*0.1;
                screenPoso.x += cos(screenPoso.y*3*PI + _SinTime.w)*0.5;
                fixed4 col = tex2D(_MainTex, screenPoso);
                col.r *= 1.4;

                if (length(col) < 1.1)
                {
                        col.b *= 0.7f;
                        col.g *= 1.4f;
                }

                col = lerp(col, timeColor, 0.2);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
