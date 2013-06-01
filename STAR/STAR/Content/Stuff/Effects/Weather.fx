
texture ScreenTexture;
sampler2D SrcColour = sampler_state
{
	Texture = <ScreenTexture>;
};
texture effectTexture;
sampler effect = sampler_state
{
	Texture = <effectTexture>;
};
float pos1x = 0;
float pos1y = 0;
float pos2x = 0;
float pos2y = 0;

float4 PixelShaderFunction(float2 Tex: TEXCOORD0,float2 Tex2:TexCoord1) : COLOR
{
    // TODO: add your pixel shader code here.
	float4 color;
	float2 pos1 = 0;
	pos1.x = pos1x;
	pos1.y = pos1y;
	float2 pos2 = 0;
	pos2.x = pos2x;
	pos2.y = pos2y;
	//color = tex2D(SrcColour,Tex);
	//color += tex2D(effect,Tex);
	color = tex2D(SrcColour,Tex);
	color +=  tex2D(effect,pos1+Tex*5) * tex2D(effect,pos1+Tex*5).a;
	
	color +=  tex2D(effect,pos2+Tex*5) * tex2D(effect,pos2+Tex*5).a;
	//color /=2;
    return color;
}

technique Technique1
{
    pass Pass0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
