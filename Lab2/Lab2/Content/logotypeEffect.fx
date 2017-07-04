#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(SpriteTextureSampler,input.TextureCoordinates);

	float4 red = float4(1, 0, 0, 1);
	float4 orange = float4(1, .5, 0, 1);
	float4 yellow = float4(1, 1, 0, 1);
	float4 green = float4(0, 1, 0, 1);
	float4 blue = float4(0, 0, 1, 1);
	float4 indigo = float4(.3, 0, .8, 1);
	float4 violet = float4(1, .8, 1, 1);

	float step = 1.0 / 7;


	if (!any(color)) return color;

	if (input.TextureCoordinates.x < (step * 1)) color = red;
	else if (input.TextureCoordinates.x < (step * 2)) color = orange;
	else if (input.TextureCoordinates.x < (step * 3)) color = yellow;
	else if (input.TextureCoordinates.x < (step * 4)) color = green;
	else if (input.TextureCoordinates.x < (step * 5)) color = blue;
	else if (input.TextureCoordinates.x < (step * 6)) color = indigo;
	else                            color = violet;

	return color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};