float ymotion;
Texture Screen;
float temp;
sampler2D ScreenS = sampler_state
{
	Texture = <Screen>;
};

float4 PixelShaderFunction(float2 tex:TEXCOORD0) : COLOR0
{
	float4 color;
	//temp += 0.1;
    float distort = 0.5*sin((10*tex.y * 3.1415)+temp) * ymotion;
    
    float2 erg = tex;
    erg.x = tex.x + distort;
    
    color = tex2D(ScreenS, erg);
    return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
