Shader "IndexedSprite"
{
    Properties
    {
        [PerRendererData] _MainTex("Texture", 2D) = "white" {}
        [PerRendererData] _Palette("Palette", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        _Brightness("Brightness", Float) = 1.0
        _Contrast("Contrast", Float) = 1.0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
    
            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };
    
            struct fragmentOutput
            {
                fixed4 color : SV_Target;
            };
    
            fixed4 _Color;
    
            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                
                #ifdef PIXELSNAP_ON
                    OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif
    
                return OUT;
            }
    
            sampler2D _MainTex;
            sampler2D _Palette;
            float _Brightness;
            float _Contrast;
    
            fragmentOutput frag(v2f IN)
            {
                fragmentOutput o;
                float2 index;
                index.x = tex2D(_MainTex, IN.texcoord).a;
                index.y = 0;
                o.color = tex2D(_Palette, index);
                o.color *= IN.color;
                o.color.rgb *= _Brightness;
                o.color.rgb *= IN.color.a;
                o.color.rgb = (o.color.rgb - 0.5) * _Contrast + 0.5;
                return o;
            }
            
            ENDCG
        }
    }
}
