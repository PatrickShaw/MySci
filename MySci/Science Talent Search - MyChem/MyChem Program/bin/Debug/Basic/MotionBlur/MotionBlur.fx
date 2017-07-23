//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- MotionBlur
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float blurAngle : register(C0);  // Defines the blurring direction
float blurMagnitude : register(C1);  

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
// effect code courtesy of here: http://windowsclient.net/wpf/wpf35/wpf-35sp1-more-effects.aspx
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float4 output = 0;  
	float2 offset;  
    int count = 16;  

    sincos(blurAngle, offset.y, offset.x);
    offset *= blurMagnitude;

    for(int i=0; i<count; i++)
    {
		output += tex2D(implicitInputSampler, uv - offset * i);
    }
    output /= count; 

    return output;
}


