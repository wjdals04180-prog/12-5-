Shader "Custom/URP_ConstantOutline"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0.001, 0.1)) = 0.01
        _ScaleFactor ("Distance Scale Factor", Float) = 1.0 // 거리 비례 민감도
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "UniversalForward" }

            // 핵심: 앞면을 끄고 뒷면만 그림 (Inverted Hull)
            Cull Front
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float _OutlineWidth;
                float _ScaleFactor;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;

                // 1. 월드 공간 정보 계산
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);

                // 2. 카메라와의 거리 계산 (Constant Width의 핵심)
                float distToCamera = distance(_WorldSpaceCameraPos, positionWS);

                // 3. 거리에 비례하여 두께 조절
                // 거리가 멀수록(distToCamera가 클수록) 더 많이 부풀립니다.
                float extrusion = _OutlineWidth * (1.0 + distToCamera * _ScaleFactor);

                // 4. 노멀 방향으로 버텍스 확장 (부풀리기)
                positionWS += normalWS * extrusion;

                // 5. 클립 공간으로 변환
                output.positionCS = TransformWorldToHClip(positionWS);
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
    }
}