float Thickness = 1.5;
float Threshold = 0.4;
float2 ScreenSize = float2(800,600);

sampler Texture : register(s0);

float getGray(float4 c)
{
	return(dot(c.rgb,((.33333).xxx)));
}

float MatrixMultiplication(float2 Tex)
{
	float length;
	float2 sum;
	float g[3][3];
	float k[3][3];
	k[0][0]=-2;
	k[0][1]=-1;
	k[0][2]=-2;
	k[1][0]=0;
	k[1][1]=0;
	k[1][2]=0;
	k[2][0]=2;
	k[2][1]=1;
	k[2][2]=2;
	float2 offset_X = float2(Thickness/ScreenSize.x,0.0f);
	float2 offset_Y = float2(0.0,Thickness/ScreenSize.y);
	float2 tempPos = Tex-offset_Y;
	g[0][0]=getGray(tex2D(Texture,tempPos -offset_X));
	g[0][1]=getGray(tex2D(Texture,tempPos ));
	g[0][2]=getGray(tex2D(Texture,tempPos +offset_X));
	tempPos = Tex;
	g[1][0]=getGray(tex2D(Texture,tempPos -offset_X));
	g[1][1]=getGray(tex2D(Texture,tempPos ));
	g[1][2]=getGray(tex2D(Texture,tempPos +offset_X));
	tempPos = Tex+offset_Y;
	g[2][0]=getGray(tex2D(Texture,tempPos -offset_X));
	g[2][1]=getGray(tex2D(Texture,tempPos ));
	g[2][2]=getGray(tex2D(Texture,tempPos +offset_X));

	sum = float2(0,0);
	for(int i=0;i<3;i++)
	{
		for(int j=0;j<3;j++)
		{
			sum += float2(g[i][j]*k[i][j],g[i][j]*k[j][i]);
		}
	}
	length = sqrt(sum.x*sum.x+sum.y*sum.y);

    return length;
}

float4 OutlineFunction(float2 Tex : TEXCOORD0) : COLOR0
{
	float value = MatrixMultiplication(Tex);
	if(value>Threshold)
		value = 0;
	else
		value = 1;
	return float4(value.xxx,1)*tex2D(Texture,Tex);
}

float4 DrawEffectFunction(float2 Tex : TEXCOORD0) : COLOR0
{
	float value = MatrixMultiplication(Tex);
	if(value>Threshold)
		value = 0;
	else
		value = 1;
	return float4(value.xxx,1);
}

float4 BrightEdgesFunction(float2 Tex : TEXCOORD0) : COLOR0
{
	float value = MatrixMultiplication(Tex);
	return float4(value.xxx,1);
}

float4 BrightEdgesColoredFunction(float2 Tex : TEXCOORD0) : COLOR0
{
	float value = MatrixMultiplication(Tex);
	return float4(value.xxx,1)*tex2D(Texture,Tex);
}




technique Outline
{
    pass Pass1
    {
        // TODO: set renderstates here.
        PixelShader = compile ps_2_0 OutlineFunction();
		
    }
}

technique DrawEffect
{
    pass Pass1
    {
        // TODO: set renderstates here.
        PixelShader = compile ps_2_0 DrawEffectFunction();
		
    }
}

technique BrightEdges
{
    pass Pass1
    {
        // TODO: set renderstates here.
        PixelShader = compile ps_2_0 BrightEdgesFunction();
		
    }
}

technique BrightEdgesColored
{
    pass Pass1
    {
        // TODO: set renderstates here.
        PixelShader = compile ps_2_0 BrightEdgesColoredFunction();
		
    }
}
