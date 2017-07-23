//--------------------------------------------------------------------------------------
// 
// WPF ShaderEffect HLSL -- Bloom
//
//--------------------------------------------------------------------------------------

float bloom_threshold = 0.5f;
float bloom_multiply = 5.0f;
float bloom_blursize = 1.0f;
float bloom_colour = 0.5f;

float PixelSizeX = 0.001f;
float PixelSizeY = 0.001f;


static const int kernelSize = 11;

static const float BlurWeights[kernelSize] = 
{
    0.002216,
    0.008764,
    0.026995,
    0.064759,
    0.150985,
    0.199471,
    0.176033,
    0.064759,
    0.026995,
    0.008764,
    0.002216,
};


float2 DownCoords[16] =
{
    { 0.0025,  -0.0025 },
    { 0.0025,  -0.0008 },
    { 0.0025,   0.0008 },
    { 0.0025,   0.0025 },

    { 0.0008,  -0.0025 },
    { 0.0008,  -0.0008 },
    { 0.0008,   0.0008 },
    { 0.0008,   0.0025 },

    {-0.0008,  -0.0025 },
    {-0.0008,  -0.0008 },
    {-0.0008,   0.0008 },
    {-0.0008,   0.0025 },

    {-0.0025,  -0.0025 },
    {-0.0025,  -0.0008 },
    {-0.0025,   0.0008 },
    {-0.0025,   0.0025 },
};

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

float4 colorFilter : register(C0);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including ImplicitInput)
//--------------------------------------------------------------------------------------

sampler2D g_samSrcColor : register(S0);


//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main( float2 Tex : TEXCOORD0 ) : COLOR0
{
    float4 Color=0;
       
    for (int i = 0; i < 16; i++)
    {
        Color += tex2D( g_samSrcColor, Tex + DownCoords[i]);
    }
    
    Color = Color / 16.0f;
    
    Color = tex2D( g_samSrcColor, Tex);
    
    Color -= 0.5f; // use of bloom_threshold value here also causes weird things
    Color *= 5.0f; // use of the bloom_multiply variable here causes it to go all black

	float sum = Color.r+Color.g+Color.b;
    Color.r = (((sum)/3.0f)*(1.0f-0.5f)) + (Color.r*0.5f);
    Color.g = (((sum)/3.0f)*(1.0f-0.5f)) + (Color.g*0.5f);
    Color.b = (((sum)/3.0f)*(1.0f-0.5f)) + (Color.b*0.5f);
   
   
    for (int i = -5; i <= 5; i++)
    {    
        Color += tex2D( g_samSrcColor, Tex + (float2(((float)i)*0.0016667,0.0f))) * BlurWeights[i+5];
        Color += tex2D( g_samSrcColor, Tex + (float2(0.0f,((float)i)*0.0016667))) * BlurWeights[i+5];
    }
   
   
    Color.a = 1.0f;
          
    return Color;
}


