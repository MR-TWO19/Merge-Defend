Shader "UI/SquareHole"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,0.8)       // màu nền tối
        _HoleCenter ("Hole Center", Vector) = (0.5,0.5,0,0)
        _HoleRadius ("Hole Radius", Float) = 0.2    // bán cạnh của hình vuông
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _HoleCenter;
            float _HoleRadius;
            fixed4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy * 0.5 + 0.5; // map từ -1..1 sang 0..1
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 delta = abs(uv - _HoleCenter.xy);

                // Nếu nằm trong hình vuông thì discard
                if (delta.x < _HoleRadius && delta.y < _HoleRadius)
                    discard;

                return _Color;
            }
            ENDCG
        }
    }
}
