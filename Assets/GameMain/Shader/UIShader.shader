Shader "UGUI/UIShader"
{
	Properties
	{
		[PerRendererData] _MainTex("MainTex", 2D) = "white" {}
				 _Color("Tint", Color) = (1,1,1,1)

		[Header(Stencil)]
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15
				[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Stencil
			{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}
				Cull Off   //�ر��޳�
			Lighting Off
			ZWrite Off  //�ر����д��
			ZTest[unity_GUIZTestMode]
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask[_ColorMask]

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#pragma multi_compile __ UNITY_UI_ALPHACLIP

				#include "UnityCG.cginc"
				#include "UnityUI.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
									float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};
					struct v2f
				{
					float4 vertex   : SV_POSITION;
									float4 color    : COLOR;
					half2 texcoord  : TEXCOORD0;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				sampler2D _MainTex;
				float4 _MainTex_TexelSize;
							float4 _Color;

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					UNITY_SETUP_INSTANCE_ID(IN);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

					OUT.vertex = UnityObjectToClipPos(IN.vertex);
							OUT.texcoord = IN.texcoord;
								OUT.color = IN.color;
					return OUT;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
								half4 color = tex2D(_MainTex,IN.texcoord) * IN.color * _Color;
					return color;
				}
			ENDCG
			}
		}
}