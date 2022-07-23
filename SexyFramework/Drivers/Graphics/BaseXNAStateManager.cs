using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.Drivers.Graphics
{
	// Token: 0x02000027 RID: 39
	public class BaseXNAStateManager : RenderStateManager
	{
		// Token: 0x060001D6 RID: 470 RVA: 0x000060F8 File Offset: 0x000042F8
		public BaseXNAStateManager(ref GraphicsDeviceManager theDevice)
		{
			this.mDevice = theDevice;
			this.mXNABlendState = BlendState.AlphaBlend;
			this.mXNARasterizerState = RasterizerState.CullNone;
			this.mXNADepthStencilState = DepthStencilState.Default;
			this.mXNATextureSlots = null;
			this.mXNASamplerStateSlots = SamplerState.LinearClamp;
			this.mXNAProjectionMatrix = Matrix.CreateOrthographicOffCenter(0f, (float)GlobalMembers.gSexyAppBase.mWidth, (float)GlobalMembers.gSexyAppBase.mHeight, 0f, -1000f, 1000f);
			this.mXNAViewMatrix = Matrix.CreateLookAt(new Vector3(0f, 0f, 300f), Vector3.Zero, Vector3.Up);
			this.mXNAWorldMatrix = Matrix.Identity;
			new Viewport(0, 0, GlobalMembers.gSexyAppBase.mWidth, GlobalMembers.gSexyAppBase.mHeight);
			this.mStatckSrcBlendState = new Stack<Graphics3D.EBlendMode>();
			this.mStatckDestBlendState = new Stack<Graphics3D.EBlendMode>();
			this.mStatckRasterizerState = new Stack<RasterizerState>();
			this.mStatckDepthStencilState = new Stack<DepthStencilState>();
			this.mStatckSamplerState = new Stack<SamplerState>();
			this.mStatckProjectionMatrix = new Stack<Matrix>();
			this.mStatckViewMatrix = new Stack<Matrix>();
			this.mStatckWorldMatrix = new Stack<Matrix>();
			this.mStatckViewPort = new Stack<Viewport>();
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00006265 File Offset: 0x00004465
		public override void Init()
		{
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00006267 File Offset: 0x00004467
		public override void Reset()
		{
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000626C File Offset: 0x0000446C
		protected void InitRenderState(ulong inIndex, ref string inStateName, ulong inHardwareDefaultValue, bool inHasContextDefault, ulong inContextDefaultValue, string inValueEnumName)
		{
			string inName = string.Format("RS:0", inStateName);
			if (inHasContextDefault)
			{
				this.mRenderStates[(int)inIndex].Init(new RenderStateManager.StateValue(inHardwareDefaultValue), new RenderStateManager.StateValue(inContextDefaultValue), inName, inValueEnumName);
				return;
			}
			this.mRenderStates[(int)inIndex].Init(new RenderStateManager.StateValue(inHardwareDefaultValue), inName, inValueEnumName);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000062CE File Offset: 0x000044CE
		protected void InitRenderStateFloat(ulong inIndex, ref string inStateName, float inDefaultValue)
		{
			this.InitRenderState(inIndex, ref inStateName, (ulong)inDefaultValue, false, 0UL, null);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000062E0 File Offset: 0x000044E0
		protected void InitTextureStageState(ulong inFirstStage, ulong inLastStage, ulong inIndex, string inStateName, ulong inDefaultValue, bool inHasContextDefault, ulong inContextDefaultValue, string inValueEnumName)
		{
			for (ulong num = inFirstStage; num <= inLastStage; num += 1UL)
			{
				string inName = string.Format("TSS:0[1]", inStateName, num);
				if (inHasContextDefault)
				{
					this.mTextureStageStates[(int)inIndex][(int)num].Init(new RenderStateManager.StateValue(inDefaultValue), new RenderStateManager.StateValue(inContextDefaultValue), inName, inValueEnumName);
				}
				else
				{
					this.mTextureStageStates[(int)inIndex][(int)num].Init(new RenderStateManager.StateValue(inDefaultValue), inName, inValueEnumName);
				}
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00006368 File Offset: 0x00004568
		protected void InitTextureStageStateFloat(ulong inFirstStage, ulong inLastStage, ulong inIndex, string inStateName, float inDefaultValue)
		{
			this.InitTextureStageState(inFirstStage, inLastStage, inIndex, inStateName, (ulong)inDefaultValue, false, 0UL, null);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00006388 File Offset: 0x00004588
		protected void InitSamplerState(ulong inFirstStage, ulong inLastStage, ulong inIndex, string inStateName, ulong inDefaultValue, bool inHasContextDefault, ulong inContextDefaultValue, string inValueEnumName)
		{
			for (ulong num = inFirstStage; num <= inLastStage; num += 1UL)
			{
				string inName = string.Format("SS:1[2]", inStateName, num);
				if (inHasContextDefault)
				{
					this.mSamplerStates[(int)inIndex][(int)num].Init(new RenderStateManager.StateValue(inDefaultValue), new RenderStateManager.StateValue(inContextDefaultValue), inName, inValueEnumName);
				}
				else
				{
					this.mSamplerStates[(int)inIndex][(int)num].Init(new RenderStateManager.StateValue(inDefaultValue), inName, inValueEnumName);
				}
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00006410 File Offset: 0x00004610
		protected void InitSamplerStateFloat(ulong inFirstStage, ulong inLastStage, ulong inIndex, string inStateName, float inDefaultValue)
		{
			this.InitSamplerState(inFirstStage, inLastStage, inIndex, inStateName, (ulong)inDefaultValue, false, 0UL, null);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00006430 File Offset: 0x00004630
		protected void InitStates()
		{
			for (uint num = 0U; num < 256U; num += 1U)
			{
				this.mRenderStates.Add(new RenderStateManager.State(this, 0U, num));
			}
			this.mRenderStates[7].Init(1UL, 0UL, "ZENABLE", "");
			this.mRenderStates[14].Init(1UL, 0UL, "ZWRITEENABLE", "");
			this.mRenderStates[15].Init(0UL, 0UL, "ALPHATESTENABLE", "");
			this.mRenderStates[23].Init(7UL, "ZFUNC", "");
			this.mRenderStates[24].Init(0UL, 0UL, "ALPHAREF", "");
			this.mRenderStates[25].Init(8UL, 8UL, "ALPHAFUNC", "");
			this.mRenderStates[19].Init(1UL, "SRCBLEND", "Sexy::Graphics3D::EBlendMode");
			this.mRenderStates[20].Init(1UL, "DESTBLEND", "Sexy::Graphics3D::EBlendMode");
			this.mRenderStates[168].Init(15UL, "COLORWRITE", "");
			for (uint num2 = 0U; num2 < 11U; num2 += 1U)
			{
				this.mLightStates.Add(new List<RenderStateManager.State>());
			}
			for (uint num3 = 0U; num3 < 11U; num3 += 1U)
			{
				for (uint num4 = 0U; num4 < 8U; num4 += 1U)
				{
					this.mLightStates[(int)num3].Add(new RenderStateManager.State(this, 3U, num3, num4));
				}
			}
			for (ulong num5 = 0UL; num5 < 8UL; num5 += 1UL)
			{
				this.mLightStates[0][(int)num5].Init(0UL, string.Format("LIGHT:ENABLED[0]", num5));
				this.mLightStates[1][(int)num5].Init(0UL, string.Format("LIGHT:TYPE[0]", num5), "D3DLIGHTTYPE");
				this.mLightStates[2][(int)num5].Init(new RenderStateManager.StateValue(1f, 1f, 10f, 0f), string.Format("LIGHT:DIFFUSE[0]", num5));
				this.mLightStates[3][(int)num5].Init(new RenderStateManager.StateValue(0f, 0f, 0f, 0f), string.Format("LIGHT:SPECULAR[0]", num5));
				this.mLightStates[4][(int)num5].Init(new RenderStateManager.StateValue(0f, 0f, 0f, 0f), string.Format("LIGHT:AMBIENT[0]", num5));
				this.mLightStates[5][(int)num5].Init(new RenderStateManager.StateValue(0f, 0f, 0f, 0f), string.Format("LIGHT:POSITION[0]", num5));
				this.mLightStates[6][(int)num5].Init(new RenderStateManager.StateValue(0f, 0f, 1f, 0f), string.Format("LIGHT:DIRECTION[0]", num5));
				this.mLightStates[7][(int)num5].Init(0UL, string.Format("LIGHT:RANGE[%d]", num5));
				this.mLightStates[8][(int)num5].Init(0UL, string.Format("LIGHT:FALLOFF[%d]", num5));
				this.mLightStates[9][(int)num5].Init(new RenderStateManager.StateValue(0f, 0f, 0f, 0f), string.Format("LIGHT:ATTENUATION[0]", num5));
				this.mLightStates[10][(int)num5].Init(new RenderStateManager.StateValue(0f, 0f, 0f, 0f), string.Format("LIGHT:ANGLES[0]", num5));
			}
			for (uint num6 = 0U; num6 < 512U; num6 += 1U)
			{
				this.mTransformStates.Add(new List<RenderStateManager.State>());
			}
			for (uint num7 = 0U; num7 < 512U; num7 += 1U)
			{
				for (uint num8 = 0U; num8 < 4U; num8 += 1U)
				{
					this.mTransformStates[(int)num7].Add(new RenderStateManager.State(this, 6U, num7, num8));
				}
			}
			for (uint num9 = 0U; num9 < 512U; num9 += 1U)
			{
				string text;
				if (num9 == 0U)
				{
					text = "WORLD";
				}
				else if (num9 == 1U)
				{
					text = "VIEW";
				}
				else if (num9 == 2U)
				{
					text = "PROJECTION";
				}
				else if (num9 == 11U)
				{
					text = "ORTHOPROJECTION";
				}
				else if (num9 >= 3U && num9 <= 10U)
				{
					text = string.Format("TEXTURE0", num9 - 3U);
				}
				else
				{
					text = string.Format("0", num9);
				}
				for (uint num10 = 0U; num10 < 4U; num10 += 1U)
				{
					this.mTransformStates[(int)num9][(int)num10].Init(new RenderStateManager.StateValue(0f, 0f, 0f, 0f), string.Format("TRANSFORM:0[1]", text, num10));
				}
			}
			for (uint num11 = 0U; num11 < 6U; num11 += 1U)
			{
				this.mViewportStates.Add(new RenderStateManager.State(this, 7U, num11));
			}
			this.mViewportStates[0].Init(0UL, "VIEWPORT:X");
			this.mViewportStates[1].Init(0UL, "VIEWPORT:Y");
			this.mViewportStates[2].Init((ulong)((long)GlobalMembers.gSexyAppBase.mWidth), "VIEWPORT:WIDTH");
			this.mViewportStates[3].Init((ulong)((long)GlobalMembers.gSexyAppBase.mHeight), "VIEWPORT:HEIGHT");
			this.mViewportStates[4].Init(0UL, "VIEWPORT_MINZ");
			this.mViewportStates[5].Init(1UL, "VIEWPORT_MAXZ");
			for (uint num12 = 0U; num12 < 21U; num12 += 1U)
			{
				this.mMiscStates.Add(new List<RenderStateManager.State>());
			}
			for (uint num13 = 0U; num13 < 14U; num13 += 1U)
			{
				this.mMiscStates[(int)num13].Add(new RenderStateManager.State(this, 8U, num13));
			}
			for (uint num14 = 0U; num14 < 8U; num14 += 1U)
			{
				this.mMiscStates[14].Add(new RenderStateManager.State(this, 8U, 14U, num14));
			}
			for (uint num15 = 0U; num15 < 32U; num15 += 1U)
			{
				this.mMiscStates[15].Add(new RenderStateManager.State(this, 8U, 15U, num15));
			}
			for (uint num16 = 0U; num16 < 256U; num16 += 1U)
			{
				this.mMiscStates[16].Add(new RenderStateManager.State(this, 8U, 16U, num16));
			}
			for (uint num17 = 0U; num17 < 4U; num17 += 1U)
			{
				this.mMiscStates[17].Add(new RenderStateManager.State(this, 8U, 17U, num17));
			}
			for (uint num18 = 0U; num18 < 8U; num18 += 1U)
			{
				this.mMiscStates[18].Add(new RenderStateManager.State(this, 8U, 18U, num18));
			}
			for (uint num19 = 0U; num19 < 8U; num19 += 1U)
			{
				this.mMiscStates[19].Add(new RenderStateManager.State(this, 8U, 19U, num19));
				this.mMiscStates[20].Add(new RenderStateManager.State(this, 8U, 20U, num19));
			}
			this.mMiscStates[0][0].Init(0UL, "MISC:VERTEXFORMAT");
			this.mMiscStates[1][0].Init(0UL, "MISC:VERTEXSIZE");
			this.mMiscStates[3][0].Init(0UL, "MISC:SHADERPROGRAM_ORTHO");
			this.mMiscStates[4][0].Init(0UL, "MISC:SHADERPROGRAM_3D");
			this.mMiscStates[10][0].Init(0UL, 0UL, "MISC:BLTDEPTH");
			this.mMiscStates[11][0].Init(0UL, "MISC:3DMODE");
			this.mMiscStates[12][0].Init(0UL, "MISC:CULLMODE");
			this.mMiscStates[8][0].Init(65535UL, "MISC:SRCBLENDOVERRIDE", "Sexy::Graphics3D::EBlendMode");
			this.mMiscStates[9][0].Init(65535UL, "MISC:DESTBLENDOVERRIDE", "Sexy::Graphics3D::EBlendMode");
			this.mMiscStates[10][0].Init(0UL, "MISC:BLTDEPTH");
			this.mMiscStates[13][0].Init(0UL, "MISC:USE_TEXSCALE");
			for (uint num20 = 0U; num20 < 8U; num20 += 1U)
			{
				this.mMiscStates[14].Add(new RenderStateManager.State(this, 8U, 14U, num20));
			}
			for (uint num21 = 0U; num21 < 8U; num21 += 1U)
			{
				this.mMiscStates[14][(int)num21].Init(0UL, string.Format("MISC:TEXTURE[0]", num21));
			}
			for (uint num22 = 0U; num22 < 8U; num22 += 1U)
			{
				this.mMiscStates[19].Add(new RenderStateManager.State(this, 8U, 19U, num22));
				this.mMiscStates[20].Add(new RenderStateManager.State(this, 8U, 20U, num22));
			}
			for (uint num23 = 0U; num23 < 8U; num23 += 1U)
			{
				this.mMiscStates[19][(int)num23].Init(new RenderStateManager.StateValue(0f, 0f, 0f, 0f), string.Format("MISC:ATLASENABLEDANDBASE[0]", num23));
				this.mMiscStates[20][(int)num23].Init(new RenderStateManager.StateValue(0f, 0f, 1f, 1f), string.Format("MISC:ATLASUV[0]", num23));
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00006EA8 File Offset: 0x000050A8
		protected void ResetStates(List<RenderStateManager.State> list)
		{
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				list[i].Reset();
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00006ED4 File Offset: 0x000050D4
		protected void ResetStatesList(List<List<RenderStateManager.State>> list)
		{
			foreach (List<RenderStateManager.State> list2 in list)
			{
				this.ResetStates(list2);
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00006F24 File Offset: 0x00005124
		protected void ResetStates()
		{
			this.ResetStates(this.mRenderStates);
			this.ResetStatesList(this.mTextureStageStates);
			this.ResetStatesList(this.mSamplerStates);
			this.ResetStatesList(this.mLightStates);
			this.ResetStates(this.mMaterialStates);
			this.ResetStatesList(this.mStreamStates);
			this.ResetStatesList(this.mTransformStates);
			this.ResetStates(this.mViewportStates);
			this.ResetStatesList(this.mMiscStates);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00006F9D File Offset: 0x0000519D
		public void SetRenderState(ulong inRS, ulong inValue)
		{
			this.mRenderStates[(int)inRS].SetValue(inValue);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00006FB4 File Offset: 0x000051B4
		private void SetTextureStageState(ulong inStage, ulong inTSS, ulong inValue)
		{
			this.mTextureStageStates[(int)inTSS][(int)inStage].SetValue(inValue);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00006FD2 File Offset: 0x000051D2
		public void SetSamplerState(ulong inSampler, ulong inSS, ulong inValue)
		{
			this.mSamplerStates[(int)inSS][(int)inSampler].SetValue(inValue);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00006FF0 File Offset: 0x000051F0
		public void SetSamplerState(SamplerState state)
		{
			if (this.mXNASamplerStateSlots != state)
			{
				this.mStateDirty = true;
			}
			this.mXNALastSamplerStateSlots = this.mXNASamplerStateSlots;
			this.mXNASamplerStateSlots = state;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00007015 File Offset: 0x00005215
		public void SetRasterizerState(RasterizerState state)
		{
			if (this.mXNARasterizerState != state)
			{
				this.mStateDirty = true;
			}
			this.mXNARasterizerState = state;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00007030 File Offset: 0x00005230
		public void SetBlendStateState(BlendState state)
		{
			if (this.mXNABlendState.AlphaDestinationBlend != state.AlphaDestinationBlend || this.mXNABlendState.ColorDestinationBlend != state.ColorDestinationBlend)
			{
				this.mStateDirty = true;
			}
			this.mXNALastBlendState = this.mXNABlendState;
			this.mXNABlendState = state;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000707D File Offset: 0x0000527D
		public void SetBlendOverride(Graphics3D.EBlendMode src, Graphics3D.EBlendMode dest)
		{
			if (this.mSrcBlendMode != src || this.mDestBlendMode != dest)
			{
				this.mStateDirty = true;
			}
			this.mSrcBlendMode = src;
			this.mDestBlendMode = dest;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000070A6 File Offset: 0x000052A6
		public void SetDepthStencilState(DepthStencilState state)
		{
			if (this.mXNADepthStencilState != state)
			{
				this.mStencilStateDirty = true;
			}
			this.mXNALastStencilState = this.mXNADepthStencilState;
			this.mXNADepthStencilState = state;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000070CB File Offset: 0x000052CB
		public void SetProjectionTransform(Matrix mat)
		{
			if (this.mXNAProjectionMatrix != mat)
			{
				this.mProjectMatrixDirty = true;
			}
			this.mXNALastProjectionMatrix = this.mXNAProjectionMatrix;
			this.mXNAProjectionMatrix = mat;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000070F5 File Offset: 0x000052F5
		public void SetViewTransform(Matrix mat)
		{
			this.mXNAViewMatrix = mat;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000070FE File Offset: 0x000052FE
		public void SetWorldTransform(Matrix mat)
		{
			this.mXNALastWorldMatrix = this.mXNAWorldMatrix;
			this.mXNAWorldMatrix = mat;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00007113 File Offset: 0x00005313
		public void SetTexture(Texture2D texture)
		{
			if (texture != this.mXNATextureSlots)
			{
				this.mTextureStateDirty = true;
			}
			else
			{
				this.mTextureStateDirty = false;
			}
			this.mLastXNATextureSlots = this.mXNATextureSlots;
			this.mXNATextureSlots = texture;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00007141 File Offset: 0x00005341
		private void SetLightEnabled(ulong inLightIndex, bool inEnabled)
		{
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00007143 File Offset: 0x00005343
		private void SetMaterialAmbient(SexyColor inColor, int inVertexColorComponent)
		{
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00007145 File Offset: 0x00005345
		private void SetMaterialDiffuse(SexyColor inColor, int inVertexColorComponent)
		{
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00007147 File Offset: 0x00005347
		private void SetMaterialSpecular(SexyColor inColor, int inVertexColorComponent, float inPower)
		{
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00007149 File Offset: 0x00005349
		private void SetMaterialEmissive(SexyColor inColor, int inVertexColorComponent)
		{
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000714B File Offset: 0x0000534B
		public void SetViewport(int inX, int inY, int inWidth, int inHeight, float inMinZ, float inMaxZ)
		{
			this.mXNAViewPort = new Viewport(inX, inY, inWidth, inHeight);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000715D File Offset: 0x0000535D
		private void SetFVF(ulong inFVF)
		{
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000715F File Offset: 0x0000535F
		private void SetCurrentTexturePalette(ulong inPaletteIndex)
		{
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00007161 File Offset: 0x00005361
		private void SetScissorRect(Rect inRect)
		{
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00007163 File Offset: 0x00005363
		private void SetNPatchMode(float inSegments)
		{
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00007165 File Offset: 0x00005365
		private void SetTextureRemap(ulong inLogicalSampler, ulong inPhysicalSampler)
		{
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00007167 File Offset: 0x00005367
		private void SetPixelShaderConstantF(ulong inStartRegister, float[] inConstantData, ulong inVector4fCount)
		{
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00007169 File Offset: 0x00005369
		private void SetVertexShaderConstantF(ulong inStartRegister, float[] inConstantData, ulong inVector4fCount)
		{
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000716B File Offset: 0x0000536B
		private void SetClipPlane(ulong inIndex, float[] inPlane)
		{
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000716D File Offset: 0x0000536D
		private void SetBltDepth(float inDepth)
		{
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00007170 File Offset: 0x00005370
		public new void PushState()
		{
			this.mStatckSrcBlendState.Push(this.mSrcBlendMode);
			this.mStatckDestBlendState.Push(this.mDestBlendMode);
			this.mStatckRasterizerState.Push(this.mXNARasterizerState);
			this.mStatckDepthStencilState.Push(this.mXNADepthStencilState);
			this.mStatckSamplerState.Push(this.mXNASamplerStateSlots);
			this.mStatckProjectionMatrix.Push(this.mXNAProjectionMatrix);
			this.mStatckViewMatrix.Push(this.mXNAViewMatrix);
			this.mStatckWorldMatrix.Push(this.mXNAWorldMatrix);
			this.mStatckViewPort.Push(this.mXNAViewPort);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00007218 File Offset: 0x00005418
		public new void PopState()
		{
			this.mSrcBlendMode = this.mStatckSrcBlendState.Pop();
			this.mDestBlendMode = this.mStatckDestBlendState.Pop();
			this.mXNARasterizerState = this.mStatckRasterizerState.Pop();
			this.mXNADepthStencilState = this.mStatckDepthStencilState.Pop();
			if (this.mXNASamplerStateSlots != this.mStatckSamplerState.Peek())
			{
				this.mStateDirty = true;
			}
			this.mXNALastSamplerStateSlots = this.mXNASamplerStateSlots;
			this.mXNASamplerStateSlots = this.mStatckSamplerState.Pop();
			this.mXNAProjectionMatrix = this.mStatckProjectionMatrix.Pop();
			this.mXNAViewMatrix = this.mStatckViewMatrix.Pop();
			this.mXNAWorldMatrix = this.mStatckWorldMatrix.Pop();
			this.mXNAViewPort = this.mStatckViewPort.Pop();
		}

		// Token: 0x06000200 RID: 512 RVA: 0x000072E4 File Offset: 0x000054E4
		public void SetAtlasState(ulong inSampler, bool inEnabled, SexyVector2 inBase, SexyVector2 inU, SexyVector2 inV)
		{
			this.mAtalasEnabled = inEnabled;
			if (!this.mAtalasEnabled)
			{
				return;
			}
			this.mAtalasBase = inBase;
			this.mAtalasU = inU;
			this.mAtalasV = inV;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000730D File Offset: 0x0000550D
		public bool GetAtlasState(ulong inSampler, ref SexyVector2 outBase, ref SexyVector2 outU, ref SexyVector2 outV)
		{
			if (!this.mAtalasEnabled)
			{
				return false;
			}
			outBase = this.mAtalasBase;
			outU = this.mAtalasU;
			outV = this.mAtalasV;
			return true;
		}

		// Token: 0x04000080 RID: 128
		public BlendState mXNABlendState;

		// Token: 0x04000081 RID: 129
		public RasterizerState mXNARasterizerState;

		// Token: 0x04000082 RID: 130
		public DepthStencilState mXNADepthStencilState;

		// Token: 0x04000083 RID: 131
		public DepthStencilState mXNALastStencilState;

		// Token: 0x04000084 RID: 132
		public BlendState mXNALastBlendState;

		// Token: 0x04000085 RID: 133
		public Texture2D mXNATextureSlots;

		// Token: 0x04000086 RID: 134
		public Texture2D mLastXNATextureSlots;

		// Token: 0x04000087 RID: 135
		public SamplerState mXNASamplerStateSlots;

		// Token: 0x04000088 RID: 136
		public SamplerState mXNALastSamplerStateSlots;

		// Token: 0x04000089 RID: 137
		public Matrix mXNAProjectionMatrix = Matrix.Identity;

		// Token: 0x0400008A RID: 138
		public Matrix mXNALastProjectionMatrix;

		// Token: 0x0400008B RID: 139
		public Matrix mXNAViewMatrix = Matrix.Identity;

		// Token: 0x0400008C RID: 140
		public Matrix mXNAWorldMatrix = Matrix.Identity;

		// Token: 0x0400008D RID: 141
		public Matrix mXNALastWorldMatrix;

		// Token: 0x0400008E RID: 142
		public Viewport mXNAViewPort;

		// Token: 0x0400008F RID: 143
		public List<RenderStateManager.State> mRenderStates;

		// Token: 0x04000090 RID: 144
		public List<List<RenderStateManager.State>> mTextureStageStates;

		// Token: 0x04000091 RID: 145
		public List<List<RenderStateManager.State>> mSamplerStates;

		// Token: 0x04000092 RID: 146
		public List<List<RenderStateManager.State>> mLightStates;

		// Token: 0x04000093 RID: 147
		public List<RenderStateManager.State> mMaterialStates;

		// Token: 0x04000094 RID: 148
		public List<List<RenderStateManager.State>> mStreamStates;

		// Token: 0x04000095 RID: 149
		public List<List<RenderStateManager.State>> mTransformStates;

		// Token: 0x04000096 RID: 150
		public List<RenderStateManager.State> mViewportStates;

		// Token: 0x04000097 RID: 151
		public List<List<RenderStateManager.State>> mMiscStates;

		// Token: 0x04000098 RID: 152
		public GraphicsDeviceManager mDevice;

		// Token: 0x04000099 RID: 153
		public Stack<Graphics3D.EBlendMode> mStatckSrcBlendState;

		// Token: 0x0400009A RID: 154
		public Stack<Graphics3D.EBlendMode> mStatckDestBlendState;

		// Token: 0x0400009B RID: 155
		public Stack<RasterizerState> mStatckRasterizerState;

		// Token: 0x0400009C RID: 156
		public Stack<DepthStencilState> mStatckDepthStencilState;

		// Token: 0x0400009D RID: 157
		public Stack<SamplerState> mStatckSamplerState;

		// Token: 0x0400009E RID: 158
		public Stack<Matrix> mStatckProjectionMatrix;

		// Token: 0x0400009F RID: 159
		public Stack<Matrix> mStatckViewMatrix;

		// Token: 0x040000A0 RID: 160
		public Stack<Matrix> mStatckWorldMatrix;

		// Token: 0x040000A1 RID: 161
		public Stack<Viewport> mStatckViewPort;

		// Token: 0x040000A2 RID: 162
		public Stack<int> mStatckDrawMode;

		// Token: 0x040000A3 RID: 163
		public bool mAtalasEnabled;

		// Token: 0x040000A4 RID: 164
		public bool mStateDirty;

		// Token: 0x040000A5 RID: 165
		public bool mTextureStateDirty;

		// Token: 0x040000A6 RID: 166
		public bool mProjectMatrixDirty;

		// Token: 0x040000A7 RID: 167
		public bool mStencilStateDirty;

		// Token: 0x040000A8 RID: 168
		public int mDrawMode;

		// Token: 0x040000A9 RID: 169
		public SexyVector2 mAtalasBase;

		// Token: 0x040000AA RID: 170
		public SexyVector2 mAtalasU;

		// Token: 0x040000AB RID: 171
		public SexyVector2 mAtalasV;

		// Token: 0x040000AC RID: 172
		public Graphics3D.EBlendMode mSrcBlendMode = Graphics3D.EBlendMode.BLEND_DEFAULT;

		// Token: 0x040000AD RID: 173
		public Graphics3D.EBlendMode mDestBlendMode = Graphics3D.EBlendMode.BLEND_DEFAULT;

		// Token: 0x02000028 RID: 40
		public enum XNA_TRANSFORM
		{
			// Token: 0x040000AF RID: 175
			OGL_TRANSFORM_WORLD,
			// Token: 0x040000B0 RID: 176
			OGL_TRANSFORM_VIEW,
			// Token: 0x040000B1 RID: 177
			OGL_TRANSFORM_PROJECTION,
			// Token: 0x040000B2 RID: 178
			OGL_TRANSFORM_TEXTURE0,
			// Token: 0x040000B3 RID: 179
			OGL_TRANSFORM_TEXTURE1,
			// Token: 0x040000B4 RID: 180
			OGL_TRANSFORM_TEXTURE2,
			// Token: 0x040000B5 RID: 181
			OGL_TRANSFORM_TEXTURE3,
			// Token: 0x040000B6 RID: 182
			OGL_TRANSFORM_TEXTURE4,
			// Token: 0x040000B7 RID: 183
			OGL_TRANSFORM_TEXTURE5,
			// Token: 0x040000B8 RID: 184
			OGL_TRANSFORM_TEXTURE6,
			// Token: 0x040000B9 RID: 185
			OGL_TRANSFORM_TEXTURE7,
			// Token: 0x040000BA RID: 186
			OGL_TRANSFORM_ORTHOPROJ,
			// Token: 0x040000BB RID: 187
			OGL_TRANSFORM_COUNT
		}

		// Token: 0x02000029 RID: 41
		public enum EStateGroup
		{
			// Token: 0x040000BD RID: 189
			SG_RS,
			// Token: 0x040000BE RID: 190
			SG_TSS,
			// Token: 0x040000BF RID: 191
			SG_SS,
			// Token: 0x040000C0 RID: 192
			SG_LIGHT,
			// Token: 0x040000C1 RID: 193
			SG_MATERIAL,
			// Token: 0x040000C2 RID: 194
			SG_STREAM,
			// Token: 0x040000C3 RID: 195
			SG_TRANSFORM,
			// Token: 0x040000C4 RID: 196
			SG_VIEWPORT,
			// Token: 0x040000C5 RID: 197
			SG_MISC,
			// Token: 0x040000C6 RID: 198
			SG_SCISSOR,
			// Token: 0x040000C7 RID: 199
			SG_COUNT
		}

		// Token: 0x0200002A RID: 42
		public enum EXNAStateGroup
		{
			// Token: 0x040000C9 RID: 201
			SG_BLEND,
			// Token: 0x040000CA RID: 202
			SG_Raster,
			// Token: 0x040000CB RID: 203
			SG_Depth,
			// Token: 0x040000CC RID: 204
			SG_Sampler,
			// Token: 0x040000CD RID: 205
			SG_Project,
			// Token: 0x040000CE RID: 206
			SG_View,
			// Token: 0x040000CF RID: 207
			SG_World,
			// Token: 0x040000D0 RID: 208
			SG_ViewPort,
			// Token: 0x040000D1 RID: 209
			SG_Num
		}

		// Token: 0x0200002B RID: 43
		public enum ERenderStateConst
		{
			// Token: 0x040000D3 RID: 211
			ST_COUNT_RS = 256,
			// Token: 0x040000D4 RID: 212
			ST_COUNT_TSS = 48,
			// Token: 0x040000D5 RID: 213
			ST_COUNT_SS = 16,
			// Token: 0x040000D6 RID: 214
			ST_COUNT_TRANSFORM = 512
		}

		// Token: 0x0200002C RID: 44
		public enum ELightState
		{
			// Token: 0x040000D8 RID: 216
			ST_LIGHT_ENABLED,
			// Token: 0x040000D9 RID: 217
			ST_LIGHT_TYPE,
			// Token: 0x040000DA RID: 218
			ST_LIGHT_DIFFUSE,
			// Token: 0x040000DB RID: 219
			ST_LIGHT_SPECULAR,
			// Token: 0x040000DC RID: 220
			ST_LIGHT_AMBIENT,
			// Token: 0x040000DD RID: 221
			ST_LIGHT_POSITION,
			// Token: 0x040000DE RID: 222
			ST_LIGHT_DIRECTION,
			// Token: 0x040000DF RID: 223
			ST_LIGHT_RANGE,
			// Token: 0x040000E0 RID: 224
			ST_LIGHT_FALLOFF,
			// Token: 0x040000E1 RID: 225
			ST_LIGHT_ATTENUATION,
			// Token: 0x040000E2 RID: 226
			ST_LIGHT_ANGLES,
			// Token: 0x040000E3 RID: 227
			ST_COUNT_LIGHT
		}

		// Token: 0x0200002D RID: 45
		public enum EMaterialState
		{
			// Token: 0x040000E5 RID: 229
			ST_MAT_DIFFUSE,
			// Token: 0x040000E6 RID: 230
			ST_MAT_AMBIENT,
			// Token: 0x040000E7 RID: 231
			ST_MAT_SPECULAR,
			// Token: 0x040000E8 RID: 232
			ST_MAT_EMISSIVE,
			// Token: 0x040000E9 RID: 233
			ST_MAT_POWER,
			// Token: 0x040000EA RID: 234
			ST_COUNT_MAT
		}

		// Token: 0x0200002E RID: 46
		public enum EStreamState
		{
			// Token: 0x040000EC RID: 236
			ST_STREAM_DATA,
			// Token: 0x040000ED RID: 237
			ST_STREAM_OFFSET,
			// Token: 0x040000EE RID: 238
			ST_STREAM_STRIDE,
			// Token: 0x040000EF RID: 239
			ST_STREAM_FREQ,
			// Token: 0x040000F0 RID: 240
			ST_COUNT_STREAM
		}

		// Token: 0x0200002F RID: 47
		public enum EViewportState
		{
			// Token: 0x040000F2 RID: 242
			ST_VIEWPORT_X,
			// Token: 0x040000F3 RID: 243
			ST_VIEWPORT_Y,
			// Token: 0x040000F4 RID: 244
			ST_VIEWPORT_WIDTH,
			// Token: 0x040000F5 RID: 245
			ST_VIEWPORT_HEIGHT,
			// Token: 0x040000F6 RID: 246
			ST_VIEWPORT_MINZ,
			// Token: 0x040000F7 RID: 247
			ST_VIEWPORT_MAXZ,
			// Token: 0x040000F8 RID: 248
			ST_COUNT_VIEWPORT
		}

		// Token: 0x02000030 RID: 48
		public enum EScissorState
		{
			// Token: 0x040000FA RID: 250
			ST_SCISSOR_ENABLE,
			// Token: 0x040000FB RID: 251
			ST_SCISSOR_X,
			// Token: 0x040000FC RID: 252
			ST_SCISSOR_Y,
			// Token: 0x040000FD RID: 253
			ST_SCISSOR_WIDTH,
			// Token: 0x040000FE RID: 254
			ST_SCISSOR_HEIGHT,
			// Token: 0x040000FF RID: 255
			ST_COUNT_SCISSOR
		}

		// Token: 0x02000031 RID: 49
		public enum EMiscState
		{
			// Token: 0x04000101 RID: 257
			ST_MISC_VERTEXFORMAT,
			// Token: 0x04000102 RID: 258
			ST_MISC_VERTEXSIZE,
			// Token: 0x04000103 RID: 259
			ST_MISC_INDICES,
			// Token: 0x04000104 RID: 260
			ST_MISC_SHADERPROGRAM_ORTHO,
			// Token: 0x04000105 RID: 261
			ST_MISC_SHADERPROGRAM_3D,
			// Token: 0x04000106 RID: 262
			ST_MISC_TEXTUREPALETTE,
			// Token: 0x04000107 RID: 263
			ST_MISC_SCISSORRECT,
			// Token: 0x04000108 RID: 264
			ST_MISC_NPATCHMODE,
			// Token: 0x04000109 RID: 265
			ST_MISC_SRCBLENDOVERRIDE,
			// Token: 0x0400010A RID: 266
			ST_MISC_DESTBLENDOVERRIDE,
			// Token: 0x0400010B RID: 267
			ST_MISC_BLTDEPTH,
			// Token: 0x0400010C RID: 268
			ST_MISC_3DMODE,
			// Token: 0x0400010D RID: 269
			ST_MISC_CULLMODE,
			// Token: 0x0400010E RID: 270
			ST_MISC_USE_TEXSCALE,
			// Token: 0x0400010F RID: 271
			ST_MISC_TEXTURE,
			// Token: 0x04000110 RID: 272
			ST_MISC_PIXELSHADERCONST,
			// Token: 0x04000111 RID: 273
			ST_MISC_VERTEXSHADERCONST,
			// Token: 0x04000112 RID: 274
			ST_MISC_CLIPPLANE,
			// Token: 0x04000113 RID: 275
			ST_MISC_TEXTUREREMAP,
			// Token: 0x04000114 RID: 276
			ST_MISC_ATLASENABLEDANDBASE,
			// Token: 0x04000115 RID: 277
			ST_MISC_ATLASUV,
			// Token: 0x04000116 RID: 278
			ST_COUNT_MISC,
			// Token: 0x04000117 RID: 279
			ST_COUNT_MISC_SINGLE = 14
		}
	}
}
