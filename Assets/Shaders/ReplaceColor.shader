Shader "Custom/ReplaceColor"
{
    Properties
    {
        _MainTex ("Textura", 2D) = "white" {}
        _ColorOriginal ("Color original", Color) = (0,1,0,1)  // Verde a reemplazar
        _ColorRemplazo ("Color remplazo", Color) = (1,0,0,1)  // Rojo
        _Umbral ("Umbral", Range(0, 1)) = 0.1  // Cuán cercano debe ser el color para empezar a reemplazarlo
        _Suavizado ("Suavizado", Range(0, 1)) = 0.5  // Cuán suave será la transición
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _ColorOriginal;
            float4 _ColorRemplazo;
            float _Umbral;
            float _Suavizado;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv);
                
                // Calcular la diferencia entre el color de la textura y el color original a reemplazar
                float diff = distance(texColor.rgb, _ColorOriginal.rgb);

                if (diff > _Umbral)
                {
                    return texColor;  // Si la diferencia es mayor que el umbral, no se reemplaza el color
                }

                // Calcular la escala de gris (no afectando alpha)
                float grayScale = dot(texColor.rgb, float3(0.299, 0.587, 0.114));

                // Umbrales para el suavizado
                float inicioTransicion = _Umbral - _Suavizado;
                float finTransicion = _Umbral + _Suavizado;

                // Factor de suavizado entre el color original y el de reemplazo
                float blendFactor = smoothstep(inicioTransicion, finTransicion, diff);

                // Interpolación solo en RGB, sin alterar el canal alpha
                float4 finalColor = lerp(float4(grayScale, grayScale, grayScale, texColor.a), _ColorRemplazo, 1.0 - blendFactor);

                // Devolver el color final manteniendo el valor alpha original
                finalColor.a = texColor.a;  // Asegurarse de que alpha no se vea afectado

                return finalColor;
            }
            ENDCG
        }
    }
}
