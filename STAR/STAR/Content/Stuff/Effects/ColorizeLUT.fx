float Size = 32;

//SOURCE: http://www.messy-mind.net/2008/fast-gpu-color-transforms/

sampler2D tex : register(s0);

texture3D cubeTex;
sampler3D cube = sampler_state
{
Texture = <cubeTex>; 
//We really want triliniear filtering for this sort of thing
MinFilter = linear;
MagFilter = linear;
MipFilter = linear; 
};

float4 PixelShaderFunction(float2 TEX: TEXCOORD0) : COLOR0
{
	float4 inCol = tex2D(tex, TEX);
 
	//Edge offset (see http://http.developer.nvidia.com/GPUGems2/gpugems2_chapter24.html)
	half3 scale = (Size - 1.0) / Size;
	half3 offset = 1.0 / (2.0 * Size);
 
	//Transform
	float4 outCol = tex3D(cube, scale * inCol + offset);
 
	//Lerp between input and transformed in RGB space based on input vertex alpha
	return lerp(inCol, outCol, inCol.a);
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
