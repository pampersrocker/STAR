#include <NvidiaShaderLibrary\\color_spaces.fxh>

float offset;
float saturation;
float a,b;
Texture Screen;
sampler2D ScreenS = sampler_state
{
	Texture = <Screen>;
};

float4 HueOffset(float2 tex: TEXCOORD0) : Color0
{
	float4 color = tex2D(ScreenS,tex);
	float3 hsv = rgb_to_hsv(color.rgb);
	hsv.x = offset;
	color.rgb = hsv_to_rgb(hsv);
	return color;
}

float4 SatOffset(float2 tex: TEXCOORD0) : Color0
{
	float4 color = tex2D(ScreenS,tex);
	float3 hsv = rgb_to_hsv(color.rgb);
	//float4 hsvColor = getHSVColor(tex);
	hsv.y = saturation;
	//return float4(1,1,1,1);
	color.rgb = hsv_to_rgb(hsv);
	return color;
}

float4 HueSatOffset(float2 tex: TEXCOORD0) : Color0
{
	float4 color = tex2D(ScreenS,tex);
	float3 hsv = rgb_to_hsv(color.rgb);
	//float4 hsvColor = getHSVColor(tex);
	hsv.x = offset;
	hsv.y = saturation;
	color.rgb = hsv_to_rgb(hsv);
	return color;
}

float4 HueSatOffsetContrast(float2 tex: TEXCOORD0) : Color0
{
	float4 color = tex2D(ScreenS,tex);
	float3 hsv = rgb_to_hsv(color.rgb);
	//float4 hsvColor = getHSVColor(tex);
	hsv.x = offset;
	hsv.y = saturation;
	color.rgb = hsv_to_rgb(hsv) *a +b;
	return color;
}

float4 HueOffsetContrast(float2 tex: TEXCOORD0) : Color0
{
	float4 color = tex2D(ScreenS,tex);
	float3 hsv = rgb_to_hsv(color.rgb);
	//float4 hsvColor = getHSVColor(tex);
	hsv.x = offset;
	color.rgb = hsv_to_rgb(hsv) * a + b;
	return color;
}

float4 SatOffsetContrast(float2 tex: TEXCOORD0) : Color0
{
	float4 color = tex2D(ScreenS,tex);
	float3 hsv = rgb_to_hsv(color.rgb);
	//float4 hsvColor = getHSVColor(tex);
	hsv.y = saturation;
	color.rgb = hsv_to_rgb(hsv) * a + b;
	return color;
}

float4 Contrast1(float2 tex: TEXCOORD0) : Color0
{
	float4 color = tex2D(ScreenS,tex);
	color.rgb = color.rgb * a + b;
	return color;
}

technique Hue
{
    pass Pass1
    {
        
        //PixelShader = compile ps_3_0 PixelShaderFunction();
        //PixelShader = compile ps_3_0 HueOffset();
    }
}

technique Sat
{
	pass Pass1
	{
		//PixelShader = compile ps_3_0 SatOffset();
	}
}

technique HueSat
{
	pass Pass1
	{
		//PixelShader = compile ps_3_0 HueSatOffset();
	}
}

technique Contrast
{
	pass Pass1
	{
		//PixelShader = compile ps_3_0 Contrast1();
	}
}

technique HueContrast
{
	pass Pass1
	{
		//PixelShader = compile ps_3_0 HueOffsetContrast();
	}
}

technique HueSatContrast
{
	pass Pass1
	{
		//PixelShader = compile ps_3_0 HueSatOffsetContrast();
	}
}

technique SatContrast
{
	pass Pass1
	{
		//PixelShader = compile ps_2_0 SatOffsetContrast();
	}
}
