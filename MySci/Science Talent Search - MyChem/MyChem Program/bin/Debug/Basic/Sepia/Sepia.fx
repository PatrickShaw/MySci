//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- Sepia
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float desat : register(C0);
float toned : register(C1);
float4 lightColor : register(C2);
float4 darkColor  : register(C3);	

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float3 scnColor = lightColor * tex2D(implicitInputSampler, uv).xyz;
    float3 grayXfer = float3(0.3,0.59,0.11);
    float gray = dot(grayXfer,scnColor);
    float3 muted = lerp(scnColor,gray.xxx,desat);
    float3 sepia = lerp(darkColor,lightColor,gray);
    float3 result = lerp(muted,sepia,toned);
    return float4(result,tex2D(implicitInputSampler, uv).a);

}


