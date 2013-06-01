float2 speed = 0;
texture ScreenTexture;
sampler2D Screens= sampler_state
{
	Texture = <ScreenTexture>;
}; 

float4 PixelShaderFunction(float2 Tex: TEXCOORD0) : COLOR
{
	float4 color;
	float i =1.0F;
	color = tex2D(Screens,Tex);
	float2 temp = Tex;
	for(i=1;i<6;i++)
	{
		temp = Tex;
		temp.x += speed.x/i;
		color+= tex2D(Screens,temp);
	}
	for(i=1;i<4;i++)
	{
		temp = Tex;
		temp.y += speed.y/i;
		color+=tex2D(Screens,temp);
	}
	color.rgb /= 9;
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
