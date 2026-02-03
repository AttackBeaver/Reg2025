#ifndef W_RIM
#define W_RIM

#ifdef RIM_ON
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Includes/Lighting.hlsl"
    #include "Includes/RimLighting.hlsl"
#endif

#ifdef RIM_ON
    half4 _RimColor;
    half _RimMin;
    half _RimMax;
    half _DirRim;
#endif

void GetRim(inout half4 color, v2f input)
{
#ifdef RIM_ON
    // Исправлено: явно преобразуем half3 в half4
    half3 dirRimRGB = RimDirLightingHalf(_RimMin, _RimMax, _RimColor, input.viewDir, input.normWorld, input.worldPos);
    half3 rimRGB = RimLightingHalf(_RimMin, _RimMax, _RimColor, input.viewDir, input.normWorld);
    
    half3 finalRimRGB = lerp(rimRGB, dirRimRGB, _DirRim);
    color.rgb += finalRimRGB;
#endif
}

#endif