uniform extern texture ScreenTexture;    

sampler ScreenS = sampler_state
{
    Texture = <ScreenTexture>;    
};
                
float distortion;        // 1 is a good default
float2 centerCoord;        // 0.5,0.5 is the screen center
float radius;
float size;
float height_to_width;
float red = 0;

float4 PixelShaderFunction(float2 texCoord: TEXCOORD0) : COLOR
{
	float2 direction;
	direction.x = (texCoord.x - centerCoord.x) ;
	direction.y = (texCoord.y - centerCoord.y) * height_to_width;
	
    float distance = length(direction);
    
    float distort = (sin(clamp(((distance - radius) / size), -1, 1) * 3.1415));
    
    float2 offset = distortion * normalize(direction);
    
    float4 color = tex2D(ScreenS, texCoord + distort * offset);
  //  if(distance>=radius+size && red ==1)
  //  {
		//color.r=0;
		//color.gb = 0;
  //  } 
    return color;
}

technique Wave
{
    pass P0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
