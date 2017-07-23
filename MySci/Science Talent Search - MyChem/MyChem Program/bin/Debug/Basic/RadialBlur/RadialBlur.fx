//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- RadialBlur
//
//--------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float blurCentre : register(C0);
float2 blurMagnitude : register(C1);  

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D implicitInputSampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
	int samples = 16;
	float4 colour = 0;
	
	for(int i = 0; i < samples; ++i)
	{
		// scale the sample based on the number of samples and the magnitude of the
		// blur given from the UI
		float scale = 1.0f + blurMagnitude * (i / (float)(samples - 1));
		// the colour is a combination of the samples, offset by the scale and the
		// direction of the point from the blur centre (starting at the blur centre)
		colour += tex2D(implicitInputSampler, blurCentre + (uv.xy - blurCentre) * scale);
	}
	
	// average the colours before returning.
	return colour / (float)samples;
}
