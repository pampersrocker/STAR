Texture tex;
sampler2D Screens= sampler_state
{
	Texture = <tex>;
}; 

float4 Blur(float offset,float2 Tex) : Color
{
    float4 color;
	float2 pos = Tex;
	pos.x -= offset;
	color = tex2D(Screens,pos);
	pos.y -= offset;
	color+= tex2D(Screens,pos);
	pos.x += offset;
	color+= tex2D(Screens,pos);
	pos.x+= offset;
	color+= tex2D(Screens,pos);
	pos.y +=offset;
	color+= tex2D(Screens,pos);
	pos.y += offset;
	color+= tex2D(Screens,pos);
	pos.x -= offset;
	color+= tex2D(Screens,pos);
	pos.x -= offset;
	color+= tex2D(Screens,pos);
	pos.y -= offset;
	color+= tex2D(Screens,pos);
	color /= 9;
	return color;
}

float4 PixelShaderFunction(float2 Tex: TEXCOORD0) : COLOR
{
	float offset = 0.001;
	float2 pos = Tex;
	float4 color = tex2D(Screens,pos);
	float divider = 1;
	for(float i = 0.010;i<0.030;i+=0.010)
	{
		color += (Blur(i,Tex)/(i*10));
		divider+=1/(i*10);
	}
	color /=divider; 
    return color;
}



technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstat
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
