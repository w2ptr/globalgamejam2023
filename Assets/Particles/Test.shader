Shader "Unlit/Test"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScreenWidth("Screen Width", Float) = 0
        _ScreenHeight("Screen Height",Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"      ="Transparent"
               "IgnoreProjector" = "True"
               "Queue"           = "Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert alpha
            #pragma fragment frag alpha
            // make fog work
            #pragma multi_compile_fog

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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ScreenWidth;
            float _ScreenHeight;

            float random(in float2 st) {
                return frac(sin(dot(st.xy,
                    float2(12.9898, 78.233)))
                    * 43758.5453123);
            }

            float noise(in float2 st) {
                float2 i = floor(st);
                float2 f = frac(st);

                // Four corners in 2D of a tile
                float a = random(i);
                float b = random(i + float2(1.0, 0.0));
                float c = random(i + float2(0.0, 1.0));
                float d = random(i + float2(1.0, 1.0));

                // Smooth Interpolation

                // Cubic Hermine Curve.  Same as SmoothStep()
                float2 u = f * f * (3.0 - 2.0 * f);
                // u = smoothstep(0.,1.,f);

                // Mix 4 coorners percentages
                return lerp(a, b, u.x) +
                    (c - a) * u.y * (1.0 - u.x) +
                    (d - b) * u.x * u.y;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                float2 ownUV = i.uv;
                fixed2 test = ComputeScreenPos(i.vertex);
                test.y = -test.y;
                test.y /= _ScreenHeight;
                test.x /= _ScreenWidth;
                fixed4 col = fixed4(test, 0.0, 1.0);

                ownUV.x = (ownUV.x - 0.5) * 2;
                ownUV.y = (ownUV.y - 0.5) * 2;

                float radial = 1.0 - length(ownUV);
                radial = clamp(radial, 0.0, 1.0);

                col = fixed4(1.0, 0.0, 0.0, 1.0);
                col *= noise(test*100.0);
                col.a *= radial;


                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
