float rFactor = 1;
float gFactor = 1;
float bFactor = 1;
Texture Screen;
sampler2D ScreenS = sampler_state
{
	Texture = <Screen>;
};

float4 PixelShaderFunction(float2 tex:TEXCOORD0) : COLOR0
{
    // TODO: add your pixel shader code here.
	float4 color = tex2D(ScreenS,tex);
	color.rgb = (color.r * rFactor + color.b*bFactor + color.g * gFactor) / 3;
    return color;
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
