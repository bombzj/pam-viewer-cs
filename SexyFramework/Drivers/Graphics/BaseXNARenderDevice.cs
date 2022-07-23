using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.Drivers.Graphics
{
	// Token: 0x0200003D RID: 61
	public class BaseXNARenderDevice : RenderDevice3D
	{
		// Token: 0x06000289 RID: 649 RVA: 0x00007F58 File Offset: 0x00006158
		public BaseXNARenderDevice(IGraphicsDriver theDriver)
		{
			this.mDevice = new GraphicsDeviceManager((theDriver as XNAGraphicsDriver).GetMainGame());
			this.mDevice.IsFullScreen = false;
			this.mDevice.SynchronizeWithVerticalRetrace = false;
			this.mGame = (theDriver as XNAGraphicsDriver).GetMainGame();
			this.mStateMgr = new BaseXNAStateManager(ref this.mDevice);
			this.mDevice.SynchronizeWithVerticalRetrace = false;
			this.mTransformStack = new Stack<SexyTransform2D>();
			this.mBatchedTriangleBuffer = new VertexPositionColorTexture[BaseXNARenderDevice.mBatchedTriangleSize];
			this.mCurrentContex = null;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00008024 File Offset: 0x00006224
		public BaseXNARenderDevice(Game game)
		{
			this.mGame = game;
			this.mDevice = new GraphicsDeviceManager(game);
			this.mDevice.SynchronizeWithVerticalRetrace = false;
			this.mStateMgr = new BaseXNAStateManager(ref this.mDevice);
			this.mTransformStack = new Stack<SexyTransform2D>();
			this.mBatchedTriangleBuffer = new VertexPositionColorTexture[BaseXNARenderDevice.mBatchedTriangleSize];
			this.mCurrentContex = null;
			this.mDevice.IsFullScreen = false;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000080CD File Offset: 0x000062CD
		public void Init()
		{
			this.SetViewport(0, 0, this.mWidth, this.mHeight, 0f, 1f);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x000080ED File Offset: 0x000062ED
		public override RenderDevice3D Get3D()
		{
			return this;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x000080F0 File Offset: 0x000062F0
		public override bool CanFillPoly()
		{
			return true;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x000080F4 File Offset: 0x000062F4
		public override HRenderContext CreateContext(Image theDestImage, HRenderContext theSourceContext)
		{
			if (theSourceContext != null)
			{
				return theSourceContext;
			}
			if (theDestImage != null)
			{
				HRenderContext hrenderContext = new HRenderContext();
				XNATextureData xnatextureData = (XNATextureData)theDestImage.GetRenderData();
				RenderTarget2D renderTarget2D;
				if (xnatextureData != null && xnatextureData.mTextures[0].mTexture != null)
				{
					renderTarget2D = (RenderTarget2D)xnatextureData.mTextures[0].mTexture;
				}
				else
				{
					renderTarget2D = new RenderTarget2D(this.mDevice.GraphicsDevice, theDestImage.GetWidth(), theDestImage.GetHeight());
					XNATextureData xnatextureData2 = new XNATextureData(null);
					theDestImage.SetRenderData(xnatextureData2);
					xnatextureData2.mWidth = renderTarget2D.Width;
					xnatextureData2.mHeight = renderTarget2D.Height;
					xnatextureData2.mTexPieceWidth = renderTarget2D.Width;
					xnatextureData2.mTexPieceHeight = renderTarget2D.Height;
					xnatextureData2.mTexVecWidth = 1;
					xnatextureData2.mTexVecHeight = 1;
					xnatextureData2.mPixelFormat = PixelFormat.PixelFormat_A8R8G8B8;
					xnatextureData2.mMaxTotalU = 1f;
					xnatextureData2.mMaxTotalV = 1f;
					xnatextureData2.mImageFlags = theDestImage.GetImageFlags();
					xnatextureData2.mOptimizedLoad = true;
					xnatextureData2.mTextures[0].mWidth = renderTarget2D.Width;
					xnatextureData2.mTextures[0].mHeight = renderTarget2D.Height;
					xnatextureData2.mTextures[0].mTexture = renderTarget2D;
				}
				hrenderContext.mHandlePtr = renderTarget2D;
				return hrenderContext;
			}
			return null;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00008224 File Offset: 0x00006424
		public override void DeleteContext(HRenderContext theContext)
		{
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00008228 File Offset: 0x00006428
		public override void SetCurrentContext(HRenderContext theContext)
		{
			if (theContext == this.mCurrentContex)
			{
				return;
			}
			if (this.mBatchedTriangleIndex > 0)
			{
				this.DoCommitAllRenderState();
				this.FlushBufferedTriangles();
			}
			if (theContext == null || theContext.GetPointer() == null)
			{
				this.mDevice.GraphicsDevice.SetRenderTarget(null);
				this.mStateMgr.SetProjectionTransform(Matrix.CreateOrthographicOffCenter(0f, 1066f, 640f, 0f, -1000f, 1000f));
				this.SetViewport(0, 0, 800, 480, 0f, 1f);
				this.mCurrentContex = theContext;
				return;
			}
			RenderTarget2D renderTarget2D = theContext.GetPointer() as RenderTarget2D;
			this.mDevice.GraphicsDevice.SetRenderTarget(renderTarget2D);
			this.mStateMgr.SetProjectionTransform(Matrix.CreateOrthographicOffCenter(0f, (float)renderTarget2D.Width, (float)renderTarget2D.Height, 0f, -1000f, 1000f));
			this.mCurrentContex = theContext;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00008318 File Offset: 0x00006518
		public override HRenderContext GetCurrentContext()
		{
			return this.mCurrentContex;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00008320 File Offset: 0x00006520
		public override void PushState()
		{
			this.mStateMgr.PushState();
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000832D File Offset: 0x0000652D
		public override void PopState()
		{
			this.mStateMgr.PopState();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000833A File Offset: 0x0000653A
		public override int Flush(uint inFlushFlags)
		{
			this.DoCommitAllRenderState();
			this.FlushBufferedTriangles();
			return 0;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00008349 File Offset: 0x00006549
		public override void SetRenderRect(int theX, int theY, int theWidth, int theHeight)
		{
			this.mRenderRect = new Rectangle(theX, theY, theWidth, theHeight);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000835B File Offset: 0x0000655B
		public override int Present(Rect theSrcRect, Rect theDestRect)
		{
			if (this.mBatchedTriangleIndex > 0)
			{
				this.DoCommitAllRenderState();
				this.FlushBufferedTriangles();
			}
			return 0;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00008373 File Offset: 0x00006573
		public override uint GetCapsFlags()
		{
			return 0U;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00008376 File Offset: 0x00006576
		public override uint GetMaxTextureStages()
		{
			return 0U;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00008379 File Offset: 0x00006579
		public override string GetInfoString(RenderDevice3D.EInfoString inInfoStr)
		{
			return "";
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00008380 File Offset: 0x00006580
		public override void GetBackBufferDimensions(ref uint outWidth, ref uint outHeight)
		{
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00008382 File Offset: 0x00006582
		public override int SceneBegun()
		{
			return 0;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00008388 File Offset: 0x00006588
		public override bool CreateImageRenderData(ref MemoryImage inImage)
		{
			if (inImage != null && inImage.mRenderData != null)
			{
				XNATextureData xnatextureData = (XNATextureData)inImage.GetRenderData();
				if (xnatextureData.mOptimizedLoad)
				{
					xnatextureData.mImageFlags = inImage.GetImageFlags();
				}
				return true;
			}
			if (inImage != null)
			{
				SharedImageRef sharedImageRef = GlobalMembers.gSexyApp.mResourceManager.LoadImage(inImage.mNameForRes);
				inImage.Dispose();
				inImage = null;
				inImage = sharedImageRef.GetMemoryImage();
				if (inImage != null && inImage.mRenderData != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00008404 File Offset: 0x00006604
		public override void RemoveImageRenderData(MemoryImage img)
		{
			XNATextureData xnatextureData = img.GetRenderData() as XNATextureData;
			if (xnatextureData == null)
			{
				return;
			}
			for (int i = 0; i < xnatextureData.mTextures.Length; i++)
			{
				if (xnatextureData.mTextures[i] != null && xnatextureData.mTextures[i].mTexture != null && this.mStateMgr.mLastXNATextureSlots == xnatextureData.mTextures[i].mTexture)
				{
					this.mStateMgr.mLastXNATextureSlots = null;
				}
			}
			xnatextureData.Dispose();
			img.SetRenderData(null);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00008480 File Offset: 0x00006680
		public override int RecoverImageBitsFromRenderData(MemoryImage inImage)
		{
			return 0;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00008483 File Offset: 0x00006683
		public override int GetTextureMemorySize(MemoryImage theImage)
		{
			return 0;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00008486 File Offset: 0x00006686
		public override PixelFormat GetTextureFormat(MemoryImage theImage)
		{
			return PixelFormat.PixelFormat_A4R4G4B4;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00008489 File Offset: 0x00006689
		public override void AdjustVertexUVsEx(uint theVertexFormat, SexyVertex[] theVertices, int theVertexCount, int theVertexSize)
		{
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000848B File Offset: 0x0000668B
		public void AdjustVertsForAtlas(int inTextureIndex, ref VertexPositionColorTexture[] inVerts, int inStartIndex, int inVertCount, uint inVertFormat, int inStride, int inTexUVOfs)
		{
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000848D File Offset: 0x0000668D
		public Image SetupAtlasState(int inTextureIndex, Image inImage)
		{
			if (inImage == null)
			{
				return null;
			}
			if (inImage.mAtlasImage != null)
			{
				return inImage.mAtlasImage;
			}
			return inImage;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x000084A4 File Offset: 0x000066A4
		public override void DrawPrimitiveEx(uint theVertexFormat, Graphics3D.EPrimitiveType thePrimitiveType, SexyVertex2D[] theVertices, int thePrimitiveCount, SexyColor theColor, int theDrawMode, float tx, float ty, bool blend, uint theFlags)
		{
			int num = 0;
			switch (thePrimitiveType)
			{
			case Graphics3D.EPrimitiveType.PT_PointList:
				num = thePrimitiveCount;
				break;
			case Graphics3D.EPrimitiveType.PT_LineList:
				num = thePrimitiveCount * 2;
				break;
			case Graphics3D.EPrimitiveType.PT_LineStrip:
				num = 1 + thePrimitiveCount;
				break;
			case Graphics3D.EPrimitiveType.PT_TriangleList:
				num = thePrimitiveCount * 3;
				break;
			case Graphics3D.EPrimitiveType.PT_TriangleStrip:
				num = 2 + thePrimitiveCount;
				break;
			case Graphics3D.EPrimitiveType.PT_TriangleFan:
				num = 2 + thePrimitiveCount;
				break;
			}
			if (num == 0 || thePrimitiveCount == 0)
			{
				return;
			}
			if (!this.PreDraw())
			{
				return;
			}
			this.mStateMgr.PushState();
			Color color = new Color(theColor.mRed, theColor.mGreen, theColor.mBlue, theColor.mAlpha);
			this.SetupDrawMode(theDrawMode);
			this.mImage.InitAtalasState();
			VertexPositionColorTexture[] array = new VertexPositionColorTexture[num];
			if ((theVertexFormat & 4U) != 0U && (color.PackedValue != 0U || tx != 0f || ty != 0f || this.mTransformStack.Count != 0))
			{
				for (int i = 0; i < num; i++)
				{
					int num2 = i;
					theVertices[num2].x = theVertices[num2].x + tx;
					int num3 = i;
					theVertices[num3].y = theVertices[num3].y + ty;
					if (theVertices[i].color == SexyColor.Zero)
					{
						theVertices[i].color = theColor;
					}
					if (this.mTransformStack.Count != 0)
					{
						SexyVector2 theVec = new SexyVector2(theVertices[i].x, theVertices[i].y);
						theVec = this.mTransformStack.Peek() * theVec;
						theVertices[i].x = theVec.x;
						theVertices[i].y = theVec.y;
					}
				}
			}
			for (int j = 0; j < num; j++)
			{
				array[j].Position.X = theVertices[j].x;
				array[j].Position.Y = theVertices[j].y;
				array[j].Position.Z = 0f;
				array[j].TextureCoordinate = this.mImage.mVectorBase + this.mImage.mVectorU * theVertices[j].u + this.mImage.mVectorV * theVertices[j].v;
				if (theVertices[j].color == SexyColor.Zero)
				{
					array[j].Color = color;
				}
				else
				{
					array[j].Color = this.GetXNAColor(theVertices[j].color);
				}
			}
			this.mStateMgr.SetWorldTransform(Matrix.Identity);
			this.mStateMgr.mStateDirty = true;
			this.DrawPrimitiveInternal<VertexPositionColorTexture>((int)thePrimitiveType, thePrimitiveCount, array, 0UL, (ulong)theVertexFormat, true, Matrix.Identity);
			this.mStateMgr.mStateDirty = false;
			this.mStateMgr.PopState();
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x000087AA File Offset: 0x000069AA
		public override void SetBltDepth(float inDepth)
		{
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x000087AC File Offset: 0x000069AC
		public override void PushTransform(SexyMatrix3 theTransform, bool concatenate)
		{
			if (this.mTransformStack.Count == 0 || !concatenate)
			{
				this.mTransformStack.Push((SexyTransform2D)theTransform);
				return;
			}
			SexyTransform2D impliedObject = this.mTransformStack.Pop();
			this.mTransformStack.Push(impliedObject * (SexyTransform2D)theTransform);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x000087FE File Offset: 0x000069FE
		public override void PopTransform()
		{
			if (this.mTransformStack.Count != 0)
			{
				this.mTransformStack.Pop();
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000881C File Offset: 0x00006A1C
		public override void PopTransform(ref SexyMatrix3 theTransform)
		{
			if (this.mTransformStack.Count != 0)
			{
				theTransform = this.mTransformStack.Pop();
				return;
			}
			SexyTransform2D sexyTransform2D = default(SexyTransform2D);
			sexyTransform2D.LoadIdentity();
			theTransform = sexyTransform2D;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00008860 File Offset: 0x00006A60
		public override void ClearColorBuffer(SexyColor inColor)
		{
			this.mDevice.GraphicsDevice.Clear(new Color(inColor.mRed, inColor.mGreen, inColor.mBlue, inColor.mAlpha));
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00008893 File Offset: 0x00006A93
		public override void ClearStencilBuffer(int inStencil)
		{
			this.mDevice.GraphicsDevice.Clear(ClearOptions.Stencil, Color.White, 0f, 0);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x000088B1 File Offset: 0x00006AB1
		public override void SetMaterialAmbient(SexyColor inColor, int inVertexColorComponent)
		{
		}

		// Token: 0x060002AC RID: 684 RVA: 0x000088B3 File Offset: 0x00006AB3
		public override void SetMaterialDiffuse(SexyColor inColor, int inVertexColorComponent)
		{
		}

		// Token: 0x060002AD RID: 685 RVA: 0x000088B5 File Offset: 0x00006AB5
		public override void SetMaterialSpecular(SexyColor inColor, int inVertexColorComponent, float inPower)
		{
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000088B7 File Offset: 0x00006AB7
		public override void SetMaterialEmissive(SexyColor inColor, int inVertexColorComponent)
		{
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000088B9 File Offset: 0x00006AB9
		public override void SetWorldTransform(SexyMatrix4 inMatrix)
		{
			this.mStateMgr.SetWorldTransform(this.GetXNAMatrix(inMatrix));
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x000088CD File Offset: 0x00006ACD
		public override void SetViewTransform(SexyMatrix4 inMatrix)
		{
			this.mStateMgr.SetViewTransform(this.GetXNAMatrix(inMatrix));
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x000088E1 File Offset: 0x00006AE1
		public override void SetProjectionTransform(SexyMatrix4 inMatrix)
		{
			this.mStateMgr.SetProjectionTransform(this.GetXNAMatrix(inMatrix));
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x000088F5 File Offset: 0x00006AF5
		public override void SetTextureTransform(int inTextureIndex, SexyMatrix4 inMatrix, int inNumDimensions)
		{
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x000088F7 File Offset: 0x00006AF7
		public override void SetTextureWrap(int inTextureIndex, bool inWrapU, bool inWrapV)
		{
			if (inWrapU || inWrapV)
			{
				this.mStateMgr.SetSamplerState(SamplerState.LinearWrap);
				return;
			}
			this.mStateMgr.SetSamplerState(SamplerState.LinearClamp);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00008920 File Offset: 0x00006B20
		public override void SetTextureLinearFilter(int inTextureIndex, bool inLinear)
		{
			if (!inLinear)
			{
				this.mStateMgr.SetSamplerState(SamplerState.PointClamp);
				return;
			}
			this.mStateMgr.SetSamplerState(SamplerState.LinearClamp);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00008946 File Offset: 0x00006B46
		public override void SetTextureCoordSource(int inTextureIndex, int inUVComponent, Graphics3D.ETexCoordGen inTexGen)
		{
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00008948 File Offset: 0x00006B48
		public override void SetTextureFactor(int inTextureFactor)
		{
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000894A File Offset: 0x00006B4A
		public override void ClearDepthBuffer()
		{
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000894C File Offset: 0x00006B4C
		public override void SetStencilState(Graphics3D.ECompareFunc inStencilTestFunc, int inRefStencil, bool inStencilEnable, Graphics3D.ETestResultFunc passFunc, Graphics3D.ETestResultFunc failFunc)
		{
			DepthStencilState mXNADepthStencilState = this.mStateMgr.mXNADepthStencilState;
			DepthStencilState depthStencilState = new DepthStencilState();
			depthStencilState.DepthBufferEnable = mXNADepthStencilState.DepthBufferEnable;
			depthStencilState.DepthBufferFunction = mXNADepthStencilState.DepthBufferFunction;
			depthStencilState.DepthBufferWriteEnable = mXNADepthStencilState.DepthBufferWriteEnable;
			depthStencilState.StencilEnable = inStencilEnable;
			depthStencilState.ReferenceStencil = inRefStencil;
			depthStencilState.StencilFunction = this.GetXNACompareFunc(inStencilTestFunc);
			depthStencilState.StencilPass = (StencilOperation)passFunc;
			depthStencilState.StencilFail = (StencilOperation)failFunc;
			this.mStateMgr.SetDepthStencilState(depthStencilState);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x000089C8 File Offset: 0x00006BC8
		public override void SetDepthState(Graphics3D.ECompareFunc inDepthTestFunc, bool inDepthWriteEnabled)
		{
			DepthStencilState depthStencilState = new DepthStencilState();
			depthStencilState.DepthBufferFunction = this.GetXNACompareFunc(inDepthTestFunc);
			depthStencilState.DepthBufferEnable = inDepthWriteEnabled;
			depthStencilState.DepthBufferWriteEnable = inDepthWriteEnabled;
			this.mStateMgr.SetDepthStencilState(depthStencilState);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00008A02 File Offset: 0x00006C02
		public override void SetAlphaTest(Graphics3D.ECompareFunc inAlphaTestFunc, int inRefAlpha)
		{
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00008A04 File Offset: 0x00006C04
		public override void SetColorWriteState(int inWriteRedEnabled, int inWriteGreenEnabled, int inWriteBlueEnabled, int inWriteAlphaEnabled)
		{
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00008A06 File Offset: 0x00006C06
		public override void SetWireframe(int inWireframe)
		{
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00008A08 File Offset: 0x00006C08
		public override void SetBlend(Graphics3D.EBlendMode inSrcBlend, Graphics3D.EBlendMode inDestBlend)
		{
			this.mStateMgr.SetBlendOverride(inSrcBlend, inDestBlend);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00008A17 File Offset: 0x00006C17
		public override void SetBackfaceCulling(int inCullClockwise, int inCullCounterClockwise)
		{
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00008A19 File Offset: 0x00006C19
		public override void SetLightingEnabled(int inLightingEnabled)
		{
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00008A1B File Offset: 0x00006C1B
		public override void SetLightEnabled(int inLightIndex, int inEnabled)
		{
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00008A20 File Offset: 0x00006C20
		public void DoCommitAllRenderState()
		{
			this.mBasicEffect.Projection = this.mStateMgr.mXNAProjectionMatrix;
			this.mBasicEffect.View = this.mStateMgr.mXNAViewMatrix;
			this.mBasicEffect.World = this.mStateMgr.mXNAWorldMatrix;
			this.mBasicEffect.VertexColorEnabled = true;
			if (this.mStateMgr.mXNATextureSlots != null)
			{
				this.mBasicEffect.Texture = this.mStateMgr.mXNATextureSlots;
				this.mBasicEffect.TextureEnabled = true;
			}
			else
			{
				this.mBasicEffect.TextureEnabled = false;
			}
			this.mBasicEffect.GraphicsDevice.RasterizerState = this.mStateMgr.mXNARasterizerState;
			this.mBasicEffect.GraphicsDevice.BlendState = this.mStateMgr.mXNABlendState;
			this.mBasicEffect.GraphicsDevice.DepthStencilState = this.mStateMgr.mXNADepthStencilState;
			this.mBasicEffect.GraphicsDevice.SamplerStates[0] = this.mStateMgr.mXNASamplerStateSlots;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00008B2C File Offset: 0x00006D2C
		public void DoCommitLastAllRenderState()
		{
			if (this.mStateMgr.mProjectMatrixDirty)
			{
				this.mBasicEffect.Projection = this.mStateMgr.mXNALastProjectionMatrix;
			}
			else
			{
				this.mBasicEffect.Projection = this.mStateMgr.mXNAProjectionMatrix;
			}
			this.mBasicEffect.View = this.mStateMgr.mXNAViewMatrix;
			this.mBasicEffect.World = this.mStateMgr.mXNALastWorldMatrix;
			this.mBasicEffect.VertexColorEnabled = true;
			if (this.mStateMgr.mLastXNATextureSlots != null)
			{
				this.mBasicEffect.Texture = this.mStateMgr.mLastXNATextureSlots;
				this.mBasicEffect.TextureEnabled = true;
			}
			else
			{
				this.mBasicEffect.TextureEnabled = false;
			}
			this.mBasicEffect.GraphicsDevice.RasterizerState = this.mStateMgr.mXNARasterizerState;
			this.mBasicEffect.GraphicsDevice.BlendState = this.mStateMgr.mXNALastBlendState;
			if (this.mStateMgr.mStencilStateDirty)
			{
				this.mBasicEffect.GraphicsDevice.DepthStencilState = this.mStateMgr.mXNALastStencilState;
			}
			else
			{
				this.mBasicEffect.GraphicsDevice.DepthStencilState = this.mStateMgr.mXNADepthStencilState;
			}
			this.mBasicEffect.GraphicsDevice.SamplerStates[0] = this.mStateMgr.mXNALastSamplerStateSlots;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00008C85 File Offset: 0x00006E85
		public override void ClearRect(Rect theRect)
		{
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00008C88 File Offset: 0x00006E88
		public override void FillRect(Rect theRect, SexyColor theColor, int theDrawMode)
		{
			if (!this.PreDraw())
			{
				return;
			}
			this.SetupDrawMode(theDrawMode);
			float num = (float)theRect.mX + this.mPixelOffset;
			float num2 = (float)theRect.mY + this.mPixelOffset;
			float num3 = (float)theRect.mWidth;
			float num4 = (float)theRect.mHeight;
			float num5 = 0f;
			Color color = new Color(theColor.mRed, theColor.mGreen, theColor.mBlue, theColor.mAlpha);
			this.mTmpVPCBuffer[0].Position = new Vector3(num, num2, num5);
			this.mTmpVPCBuffer[0].Color = color;
			this.mTmpVPCBuffer[1].Position = new Vector3(num, num2 + num4, num5);
			this.mTmpVPCBuffer[1].Color = color;
			this.mTmpVPCBuffer[2].Position = new Vector3(num + num3, num2, num5);
			this.mTmpVPCBuffer[2].Color = color;
			this.mTmpVPCBuffer[3].Position = new Vector3(num + num3, num2 + num4, num5);
			this.mTmpVPCBuffer[3].Color = color;
			this.SetTextureDirect(0, null);
			this.mStateMgr.SetWorldTransform(Matrix.Identity);
			this.DrawPrimitiveInternal<VertexPositionColor>(5, 2, this.mTmpVPCBuffer, 32UL, this.mDefaultVertexFVF, true, Matrix.Identity);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00008DF2 File Offset: 0x00006FF2
		public override void FillScanLinesWithCoverage(RenderDevice.Span theSpans, int theSpanCount, SexyColor theColor, int theDrawMode, string theCoverage, int theCoverX, int theCoverY, int theCoverWidth, int theCoverHeight)
		{
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00008DF4 File Offset: 0x00006FF4
		public override void FillPoly(SexyPoint[] theVertices, int theNumVertices, Rect theClipRect, SexyColor theColor, int theDrawMode, int tx, int ty)
		{
			if (theNumVertices < 3)
			{
				return;
			}
			if (!this.PreDraw())
			{
				return;
			}
			this.SetupDrawMode(theDrawMode);
			Color color = new Color(theColor.mRed, theColor.mGreen, theColor.mBlue, theColor.mAlpha);
			float num = 0f;
			VertexPositionColorTexture[] array = new VertexPositionColorTexture[theNumVertices];
			for (int i = 0; i < theNumVertices; i++)
			{
				array[i].Position = new Vector3((float)theVertices[i].mX + (float)tx, (float)theVertices[i].mY + (float)ty, num);
				array[i].Color = color;
				if (this.mTransformStack.Count != 0)
				{
					SexyVector2 theVec = new SexyVector2(array[i].Position.X, array[i].Position.Y);
					theVec = this.mTransformStack.Peek() * theVec;
					array[i].Position.X = theVec.x;
					array[i].Position.Y = theVec.y;
				}
			}
			this.DrawPolyClipped(theClipRect, array);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00008F14 File Offset: 0x00007114
		public void DrawPolyClipped(Rect theClipRect, VertexPositionColorTexture[] theList)
		{
			List<VertexPositionColorTexture> list = new List<VertexPositionColorTexture>();
			List<VertexPositionColorTexture> list2 = new List<VertexPositionColorTexture>(theList);
			int mX = theClipRect.mX;
			int num = mX + theClipRect.mWidth;
			int mY = theClipRect.mY;
			int num2 = mY + theClipRect.mHeight;
			this.ClipPoints(0, (float)mX, BaseXNARenderDevice.ClipperType.Clipper_Less, list2, list);
			list2.Clear();
			this.ClipPoints(1, (float)mY, BaseXNARenderDevice.ClipperType.Clipper_Less, list, list2);
			list.Clear();
			this.ClipPoints(0, (float)num, BaseXNARenderDevice.ClipperType.Clipper_GreaterEqual, list2, list);
			list2.Clear();
			this.ClipPoints(1, (float)num2, BaseXNARenderDevice.ClipperType.Clipper_GreaterEqual, list, list2);
			this.CheckBatchAndCommit();
			if (list2.Count >= 3)
			{
				this.BufferedDrawPrimitive(6, list2.Count - 2, list2.ToArray(), (int)this.mDefaultVertexSize, this.mDefaultVertexFVF, Matrix.Identity);
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00008FD0 File Offset: 0x000071D0
		public void ClipPoint(int index, float clipValue, BaseXNARenderDevice.ClipperType type, VertexPositionColorTexture vertex1, VertexPositionColorTexture vertex2, List<VertexPositionColorTexture> outList)
		{
			float vertexValue = this.GetVertexValue(index, vertex1);
			float vertexValue2 = this.GetVertexValue(index, vertex2);
			switch (type)
			{
			case BaseXNARenderDevice.ClipperType.Clipper_Less:
				if (vertexValue >= clipValue)
				{
					if (vertexValue2 >= clipValue)
					{
						outList.Add(vertex2);
						return;
					}
					float t = (clipValue - vertexValue) / (vertexValue2 - vertexValue);
					outList.Add(this.Interpolate(vertex1, vertex2, t));
					return;
				}
				else if (vertexValue2 >= clipValue)
				{
					float t2 = (clipValue - vertexValue) / (vertexValue2 - vertexValue);
					outList.Add(this.Interpolate(vertex1, vertex2, t2));
					outList.Add(vertex2);
					return;
				}
				break;
			case BaseXNARenderDevice.ClipperType.Clipper_Greater:
			case BaseXNARenderDevice.ClipperType.Clipper_Equal:
				break;
			case BaseXNARenderDevice.ClipperType.Clipper_GreaterEqual:
				if (vertexValue < clipValue)
				{
					if (vertexValue2 < clipValue)
					{
						outList.Add(vertex2);
						return;
					}
					float t3 = (clipValue - vertexValue) / (vertexValue2 - vertexValue);
					outList.Add(this.Interpolate(vertex1, vertex2, t3));
					return;
				}
				else if (vertexValue2 < clipValue)
				{
					float t4 = (clipValue - vertexValue) / (vertexValue2 - vertexValue);
					outList.Add(this.Interpolate(vertex1, vertex2, t4));
					outList.Add(vertex2);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x000090BC File Offset: 0x000072BC
		public void ClipPoints(int index, float clipValue, BaseXNARenderDevice.ClipperType type, List<VertexPositionColorTexture> inList, List<VertexPositionColorTexture> outList)
		{
			if (inList.Count < 2)
			{
				return;
			}
			this.ClipPoint(index, clipValue, type, inList[inList.Count - 1], inList[0], outList);
			for (int i = 0; i < inList.Count - 1; i++)
			{
				this.ClipPoint(index, clipValue, type, inList[i], inList[i + 1], outList);
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00009128 File Offset: 0x00007328
		public float GetVertexValue(int index, VertexPositionColorTexture vertex)
		{
			switch (index)
			{
			case 0:
				return vertex.Position.X;
			case 1:
				return vertex.Position.Y;
			case 2:
				return vertex.Position.Z;
			default:
				return 0f;
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00009178 File Offset: 0x00007378
		private VertexPositionColorTexture Interpolate(VertexPositionColorTexture v1, VertexPositionColorTexture v2, float t)
		{
			VertexPositionColorTexture result = v1;
			result.Position.X = v1.Position.X + t * (v2.Position.X - v1.Position.X);
			result.Position.Y = v1.Position.Y + t * (v2.Position.Y - v1.Position.Y);
			result.TextureCoordinate.X = v1.TextureCoordinate.X + t * (v2.TextureCoordinate.X - v1.TextureCoordinate.X);
			result.TextureCoordinate.Y = v1.TextureCoordinate.Y + t * (v2.TextureCoordinate.Y - v1.TextureCoordinate.Y);
			if (v1.Color != v2.Color)
			{
				Vector4 vector = Vector4.Lerp(v1.Color.ToVector4(), v2.Color.ToVector4(), t);
				Color color = new Color(vector);
				result.Color = color;
			}
			return result;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000929C File Offset: 0x0000749C
		public override void DrawLine(double theStartX, double theStartY, double theEndX, double theEndY, SexyColor theColor, int theDrawMode, bool antiAlias)
		{
			if (!this.PreDraw())
			{
				return;
			}
			this.SetupDrawMode(theDrawMode);
			float num = (float)theStartX;
			float num2 = (float)theStartY;
			float num3 = (float)theEndX;
			float num4 = (float)theEndY;
			float num5 = 0f;
			Color color = new Color(theColor.mRed, theColor.mGreen, theColor.mBlue, theColor.mAlpha);
			VertexPositionColor[] inVertData = new VertexPositionColor[]
			{
				new VertexPositionColor(new Vector3(num, num2, num5), color),
				new VertexPositionColor(new Vector3(num3, num4, num5), color)
			};
			this.SetTextureDirect(0, null);
			this.mStateMgr.SetWorldTransform(Matrix.Identity);
			this.DoCommitAllRenderState();
			this.DrawPrimitiveInternal<VertexPositionColor>(3, 1, inVertData, 32UL, this.mDefaultVertexFVF, false, Matrix.Identity);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000936E File Offset: 0x0000756E
		public override void Blt(Image theImage, int theX, int theY, Rect theSrcRect, SexyColor theColor, int theDrawMode)
		{
			this.BltNoClipF(theImage, (float)theX, (float)theY, theSrcRect, theColor, theDrawMode, false);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00009384 File Offset: 0x00007584
		public override void BltF(Image theImage, float theX, float theY, Rect theSrcRect, Rect theClipRect, SexyColor theColor, int theDrawMode)
		{
			FRect theTRect = new FRect((float)theClipRect.mX, (float)theClipRect.mY, (float)theClipRect.mWidth, (float)theClipRect.mHeight);
			FRect frect = new FRect(theX, theY, (float)theSrcRect.mWidth, (float)theSrcRect.mHeight);
			FRect frect2 = frect.Intersection(theTRect);
			if (frect2.mWidth != frect.mWidth || frect2.mHeight != frect.mHeight)
			{
				if (frect2.mWidth != 0f && frect2.mHeight != 0f)
				{
					this.BltClipF(theImage, theX, theY, theSrcRect, theClipRect, theColor, theDrawMode);
					return;
				}
			}
			else
			{
				this.BltNoClipF(theImage, theX, theY, theSrcRect, theColor, theDrawMode, true);
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00009438 File Offset: 0x00007638
		public override void BltRotated(Image theImage, float theX, float theY, Rect theSrcRect, Rect theClipRect, SexyColor theColor, int theDrawMode, double theRot, float theRotCenterX, float theRotCenterY)
		{
			this.mTransform.Reset();
			this.mTransform.Translate(-theRotCenterX, -theRotCenterY);
			this.mTransform.RotateRad((float)theRot);
			this.mTransform.Translate(theX + theRotCenterX, theY + theRotCenterY);
			this.BltTransformed(theImage, theClipRect, theColor, theDrawMode, theSrcRect, this.mTransform.GetMatrix(), true, 0f, 0f, false);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x000094B0 File Offset: 0x000076B0
		private void BltTransformed(Image theImage, Rect theClipRect, SexyColor theColor, int theDrawMode, Rect theSrcRect, SexyMatrix3 theTransform, bool linearFilter, float theX, float theY, bool center)
		{
			if (!this.PreDraw())
			{
				return;
			}
			SexyTransform2D sexyTransform2D = (SexyTransform2D)theTransform;
			if (this.mTransformStack.Count == 0)
			{
				this.BltTransformHelper(theImage, theClipRect, theColor, theDrawMode, theSrcRect, sexyTransform2D, linearFilter, theX, theY, center);
				return;
			}
			if (theX != 0f || theY != 0f)
			{
				SexyTransform2D sexyTransform2D2 = new SexyTransform2D(false);
				if (center)
				{
					sexyTransform2D2.Translate((float)(-(float)theSrcRect.mWidth) / 2f, (float)(-(float)theSrcRect.mHeight) / 2f);
				}
				sexyTransform2D2 = sexyTransform2D * sexyTransform2D2;
				sexyTransform2D2.Translate(theX, theY);
				sexyTransform2D2 = this.mTransformStack.Peek() * sexyTransform2D2;
				this.BltTransformHelper(theImage, theClipRect, theColor, theDrawMode, theSrcRect, sexyTransform2D2, linearFilter, theX, theY, center);
				return;
			}
			SexyTransform2D theTransform2 = this.mTransformStack.Peek() * sexyTransform2D;
			this.BltTransformHelper(theImage, theClipRect, theColor, theDrawMode, theSrcRect, theTransform2, linearFilter, theX, theY, center);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000959C File Offset: 0x0000779C
		public override void BltMatrix(Image theImage, float x, float y, SexyMatrix3 theMatrix, Rect theClipRect, SexyColor theColor, int theDrawMode, Rect theSrcRect, bool blend)
		{
			this.BltTransformed(theImage, theClipRect, theColor, theDrawMode, theSrcRect, theMatrix, blend, x, y, true);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x000095C0 File Offset: 0x000077C0
		public override void BltTriangles(Image theImage, SexyVertex2D[,] theVertices, int theNumTriangles, SexyColor theColor, int theDrawMode, float tx, float ty, bool blend, Rect theClipRect)
		{
			this.BltTrianglesHelper(theImage, theVertices, theNumTriangles, theColor, theDrawMode, tx, ty, blend, theClipRect);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x000095E4 File Offset: 0x000077E4
		public void BltTrianglesHelper(Image theImage, SexyVertex2D[,] theVertices, int theNumTriangles, SexyColor theColor, int theDrawMode, float tx, float ty, bool blend, Rect theClipRect)
		{
			theImage = this.SetupAtlasState(0, theImage);
			MemoryImage memoryImage = theImage as MemoryImage;
			if (!this.CreateImageRenderData(ref memoryImage))
			{
				return;
			}
			this.SetupDrawMode(theDrawMode);
			XNATextureData xnatextureData = (XNATextureData)memoryImage.GetRenderData();
			if ((double)xnatextureData.mMaxTotalU <= 1.0 && (double)xnatextureData.mMaxTotalV <= 1.0)
			{
				this.SetTextureDirect(0, xnatextureData.mTextures[0].mTexture);
				float num = 0f;
				bool flag = this.mTransformStack.Count != 0;
				bool flag2 = theClipRect.mX != 0 || theClipRect.mY != 0 || theClipRect.mWidth != this.mScreenWidth || theClipRect.mHeight != this.mScreenHeight;
				this.CheckBatchAndCommit();
				if (flag)
				{
					SexyMatrix3 sexyMatrix = this.mTransformStack.Peek();
					for (int i = 0; i < theNumTriangles; i++)
					{
						if (this.mBatchedTriangleIndex > BaseXNARenderDevice.mBatchedTriangleSize - 3)
						{
							this.DoCommitAllRenderState();
							this.FlushBufferedTriangles();
						}
						SexyVector2[] array = new SexyVector2[3];
						array[0].x = theVertices[i, 0].x + tx;
						array[0].y = theVertices[i, 0].y + ty;
						array[1].x = theVertices[i, 1].x + tx;
						array[1].y = theVertices[i, 1].y + ty;
						array[2].x = theVertices[i, 2].x + tx;
						array[2].y = theVertices[i, 2].y + ty;
						array[0].x = array[0].x * sexyMatrix.m00 + array[0].y * sexyMatrix.m01 + sexyMatrix.m02;
						array[0].y = array[0].x * sexyMatrix.m10 + array[0].y * sexyMatrix.m11 + sexyMatrix.m12;
						array[1].x = array[1].x * sexyMatrix.m00 + array[1].y * sexyMatrix.m01 + sexyMatrix.m02;
						array[1].y = array[1].x * sexyMatrix.m10 + array[1].y * sexyMatrix.m11 + sexyMatrix.m12;
						array[2].x = array[2].x * sexyMatrix.m00 + array[2].y * sexyMatrix.m01 + sexyMatrix.m02;
						array[2].y = array[2].x * sexyMatrix.m10 + array[2].y * sexyMatrix.m11 + sexyMatrix.m12;
						this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = new VertexPositionColorTexture(new Vector3(array[0].x, array[0].y, num), (theVertices[i, 0].color != SexyColor.Zero) ? this.GetXNAColor(theVertices[i, 0].color) : this.GetXNAColor(theColor), new Vector2(theVertices[i, 0].u * xnatextureData.mMaxTotalU, theVertices[i, 0].v * xnatextureData.mMaxTotalV));
						this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = new VertexPositionColorTexture(new Vector3(array[1].x, array[1].y, num), (theVertices[i, 1].color != SexyColor.Zero) ? this.GetXNAColor(theVertices[i, 1].color) : this.GetXNAColor(theColor), new Vector2(theVertices[i, 1].u * xnatextureData.mMaxTotalU, theVertices[i, 1].v * xnatextureData.mMaxTotalV));
						this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = new VertexPositionColorTexture(new Vector3(array[2].x, array[2].y, num), (theVertices[i, 2].color != SexyColor.Zero) ? this.GetXNAColor(theVertices[i, 2].color) : this.GetXNAColor(theColor), new Vector2(theVertices[i, 2].u * xnatextureData.mMaxTotalU, theVertices[i, 2].v * xnatextureData.mMaxTotalV));
						new SexyVector2(this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex - 3].TextureCoordinate.X, this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex - 3].TextureCoordinate.Y);
						new SexyVector2(this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex - 2].TextureCoordinate.X, this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex - 2].TextureCoordinate.Y);
						new SexyVector2(this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex - 1].TextureCoordinate.X, this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex - 1].TextureCoordinate.Y);
						this.AdjustVertsForAtlas(0, ref this.mBatchedTriangleBuffer, this.mBatchedTriangleIndex - 3, 3, 0U, 32, 0);
						if (!BaseXNARenderDevice.SUPPORT_HW_CLIP && flag2)
						{
							VertexPositionColorTexture[] array2 = new VertexPositionColorTexture[3];
							for (int j = 0; j < 3; j++)
							{
								array2[j] = this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex - (3 - j)];
							}
							this.mBatchedTriangleIndex -= 3;
							this.DrawPolyClipped(theClipRect, array2);
						}
					}
					return;
				}
				if (!BaseXNARenderDevice.SUPPORT_HW_CLIP && flag2)
				{
					for (int k = 0; k < theNumTriangles; k++)
					{
						if (this.mBatchedTriangleIndex > BaseXNARenderDevice.mBatchedTriangleSize - 3)
						{
							this.DoCommitAllRenderState();
							this.FlushBufferedTriangles();
						}
						this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = new VertexPositionColorTexture(new Vector3(theVertices[k, 0].x, theVertices[k, 0].y, num), (theVertices[k, 0].color != SexyColor.Zero) ? this.GetXNAColor(theVertices[k, 0].color) : this.GetXNAColor(theColor), new Vector2(theVertices[k, 0].u * xnatextureData.mMaxTotalU, theVertices[k, 0].v * xnatextureData.mMaxTotalV));
						this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = new VertexPositionColorTexture(new Vector3(theVertices[k, 1].x, theVertices[k, 1].y, num), (theVertices[k, 1].color != SexyColor.Zero) ? this.GetXNAColor(theVertices[k, 1].color) : this.GetXNAColor(theColor), new Vector2(theVertices[k, 1].u * xnatextureData.mMaxTotalU, theVertices[k, 1].v * xnatextureData.mMaxTotalV));
						this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = new VertexPositionColorTexture(new Vector3(theVertices[k, 2].x, theVertices[k, 2].y, num), (theVertices[k, 2].color != SexyColor.Zero) ? this.GetXNAColor(theVertices[k, 2].color) : this.GetXNAColor(theColor), new Vector2(theVertices[k, 2].u * xnatextureData.mMaxTotalU, theVertices[k, 2].v * xnatextureData.mMaxTotalV));
						this.AdjustVertsForAtlas(0, ref this.mBatchedTriangleBuffer, this.mBatchedTriangleIndex - 3, 3, 0U, 32, 0);
						if (!BaseXNARenderDevice.SUPPORT_HW_CLIP && flag2)
						{
							VertexPositionColorTexture[] array3 = new VertexPositionColorTexture[3];
							for (int l = 0; l < 3; l++)
							{
								array3[l] = this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex - (3 - l)];
							}
							this.mBatchedTriangleIndex -= 3;
							this.DrawPolyClipped(theClipRect, array3);
						}
					}
				}
				else
				{
					int m = 0;
					while (m < theNumTriangles)
					{
						if (this.mBatchedTriangleIndex >= BaseXNARenderDevice.mBatchedTriangleSize)
						{
							this.DoCommitAllRenderState();
							this.FlushBufferedTriangles();
						}
						int inStartIndex = this.mBatchedTriangleIndex;
						int n = 0;
						int num2 = Math.Min(BaseXNARenderDevice.mBatchedTriangleSize - this.mBatchedTriangleIndex, theNumTriangles - m);
						while (n < num2)
						{
							this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = new VertexPositionColorTexture(new Vector3(theVertices[m, 0].x, theVertices[m, 0].y, num), (theVertices[m, 0].color != SexyColor.Zero) ? this.GetXNAColor(theVertices[m, 0].color) : this.GetXNAColor(theColor), new Vector2(theVertices[m, 0].u * xnatextureData.mMaxTotalU, theVertices[m, 0].v * xnatextureData.mMaxTotalV));
							this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = new VertexPositionColorTexture(new Vector3(theVertices[m, 1].x, theVertices[m, 1].y, num), (theVertices[m, 1].color != SexyColor.Zero) ? this.GetXNAColor(theVertices[m, 1].color) : this.GetXNAColor(theColor), new Vector2(theVertices[m, 1].u * xnatextureData.mMaxTotalU, theVertices[m, 1].v * xnatextureData.mMaxTotalV));
							this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = new VertexPositionColorTexture(new Vector3(theVertices[m, 2].x, theVertices[m, 2].y, num), (theVertices[m, 2].color != SexyColor.Zero) ? this.GetXNAColor(theVertices[m, 2].color) : this.GetXNAColor(theColor), new Vector2(theVertices[m, 2].u * xnatextureData.mMaxTotalU, theVertices[m, 2].v * xnatextureData.mMaxTotalV));
							n += 3;
							m++;
						}
						this.AdjustVertsForAtlas(0, ref this.mBatchedTriangleBuffer, inStartIndex, n, 0U, 32, 0);
					}
				}
				if (this.mBatchedTriangleIndex > 0 && (this.mRenderModeFlags & 1U) != 0U)
				{
					this.DrawPrimitiveInternal<VertexPositionColorTexture>(4, this.mBatchedTriangleIndex / 3, this.mBatchedTriangleBuffer, 32UL, 32UL, false, Matrix.Identity);
					this.mBatchedTriangleIndex = 0;
				}
			}
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000A228 File Offset: 0x00008428
		private void CheckBatchAndCommit()
		{
			if (this.mSceneBegun && this.mBatchedTriangleIndex >= 0 && (this.mStateMgr.mStateDirty || this.mStateMgr.mTextureStateDirty || this.mStateMgr.mProjectMatrixDirty || this.mStateMgr.mStencilStateDirty))
			{
				if (this.mBatchedTriangleIndex > 0)
				{
					this.DoCommitLastAllRenderState();
					this.FlushBufferedTriangles();
				}
				this.mStateMgr.mStateDirty = false;
				this.mStateMgr.mTextureStateDirty = false;
				this.mStateMgr.mProjectMatrixDirty = false;
				this.mStateMgr.mStencilStateDirty = false;
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000A2C4 File Offset: 0x000084C4
		private void FlushBufferedTriangles()
		{
			if (this.mSceneBegun && this.mBatchedTriangleIndex > 0)
			{
				int inPrimCount = this.mBatchedTriangleIndex / 3;
				this.DrawPrimitiveInternal<VertexPositionColorTexture>(4, inPrimCount, this.mBatchedTriangleBuffer, 32UL, this.mDefaultVertexFVF, false, Matrix.Identity);
				this.mBatchedTriangleIndex = 0;
				this.mBatchedIndexIndex = 0;
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000A318 File Offset: 0x00008518
		public override void BltMirror(Image theImage, int theX, int theY, Rect theSrcRect, SexyColor theColor, int theDrawMode)
		{
			this.mTransform.Reset();
			this.mTransform.Translate(-(float)theSrcRect.mWidth, 0f);
			this.mTransform.Scale(-1f, 1f);
			this.mTransform.Translate((float)theX, (float)theY);
			this.BltTransformed(theImage, Rect.INVALIDATE_RECT, theColor, theDrawMode, theSrcRect, this.mTransform.GetMatrix(), false, 0f, 0f, false);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000A39C File Offset: 0x0000859C
		public override void BltStretched(Image theImage, Rect theDestRect, Rect theSrcRect, Rect theClipRect, SexyColor theColor, int theDrawMode, bool fastStretch, bool mirror)
		{
			float num = (float)theDestRect.mWidth / (float)theSrcRect.mWidth;
			float sy = (float)theDestRect.mHeight / (float)theSrcRect.mHeight;
			this.mTransform.Reset();
			if (mirror)
			{
				this.mTransform.Translate(-(float)theSrcRect.mWidth, 0f);
				this.mTransform.Scale(-num, sy);
			}
			else
			{
				this.mTransform.Scale(num, sy);
			}
			this.mTransform.Translate((float)theDestRect.mX, (float)theDestRect.mY);
			this.BltTransformed(theImage, theClipRect, theColor, theDrawMode, theSrcRect, this.mTransform.GetMatrix(), !fastStretch, 0f, 0f, false);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000A45B File Offset: 0x0000865B
		public override void SetGlobalAmbient(SexyColor inColor)
		{
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000A460 File Offset: 0x00008660
		public void Init(int width, int height)
		{
			this.mScreenWidth = (this.mDevice.PreferredBackBufferWidth = 1066);
			this.mScreenHeight = (this.mDevice.PreferredBackBufferHeight = 640);
			this.mWidth = width;
			this.mHeight = height;
			this.mDevice.SupportedOrientations = DisplayOrientation.LandscapeLeft|DisplayOrientation.LandscapeRight;
			this.mDevice.ApplyChanges();
			this.mTmpVPCTBuffer = new VertexPositionColorTexture[4];
			this.mTmpVPCBuffer = new VertexPositionColor[4];
			this.mBasicEffect = new BasicEffect(this.mDevice.GraphicsDevice);
			this.mSpriteBatch = new SpriteBatch(this.mDevice.GraphicsDevice);
			this.mAlphaTestEffect = new AlphaTestEffect(this.mDevice.GraphicsDevice);
			this.mAdditiveState.AlphaDestinationBlend = 0;
			this.mAdditiveState.ColorDestinationBlend = 0;
			this.mAdditiveState.AlphaSourceBlend = Blend.SourceAlpha;
			this.mAdditiveState.ColorSourceBlend = Blend.SourceAlpha;
			this.mNormalState.AlphaDestinationBlend = Blend.InverseSourceAlpha;
			this.mNormalState.ColorDestinationBlend = Blend.InverseSourceAlpha;
			this.mNormalState.AlphaSourceBlend = Blend.SourceAlpha;
			this.mNormalState.ColorSourceBlend = Blend.SourceAlpha;
			this.SetSamplerState(0, 0);
			this.SetBlend(Graphics3D.EBlendMode.BLEND_DEFAULT, Graphics3D.EBlendMode.BLEND_DEFAULT);
			this.SetDepthState(Graphics3D.ECompareFunc.COMPARE_NEVER, false);
			this.SetRasterizerState(0, 0);
			this.SetDefaultState(null, false);
			this.mStateMgr.mStateDirty = false;
			this.mCurDrawMode = 0;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000A5C4 File Offset: 0x000087C4
		public void SetDefaultState(Image theImage, bool isInScene)
		{
			int num = this.mWidth;
			int num2 = this.mHeight;
			if (theImage != null)
			{
				num = theImage.mWidth;
				num2 = theImage.mHeight;
			}
			this.SetViewport(0, 0, 800, 480, 0f, 1f);
			this.mStateMgr.SetProjectionTransform(Matrix.CreateOrthographicOffCenter(0f, (float)num, (float)num2, 0f, -1000f, 1000f));
			this.mStateMgr.SetViewTransform(Matrix.CreateLookAt(new Vector3(0f, 0f, 300f), Vector3.Zero, Vector3.Up));
			this.mStateMgr.SetWorldTransform(Matrix.Identity);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000A672 File Offset: 0x00008872
		public override void SetViewport(int theX, int theY, int theWidth, int theHeight, float theMinZ, float theMaxZ)
		{
			this.mStateMgr.SetViewport(theX, theY, theWidth, theHeight, theMinZ, theMaxZ);
			this.mDevice.GraphicsDevice.Viewport = this.mStateMgr.mXNAViewPort;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000A6A3 File Offset: 0x000088A3
		public void SetTextureDirect(int theStage, Texture2D theTexture)
		{
			this.mStateMgr.SetTexture(theTexture);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000A6B4 File Offset: 0x000088B4
		public void SetRenderState(SEXY3DRSS theRenderState, uint theValue)
		{
			DepthStencilState depthStencilState = new DepthStencilState();
			depthStencilState.DepthBufferEnable = false;
			depthStencilState.DepthBufferWriteEnable = false;
			this.mDevice.GraphicsDevice.DepthStencilState = depthStencilState;
			RasterizerState rasterizerState = new RasterizerState();
			rasterizerState.CullMode = 0;
			this.mDevice.GraphicsDevice.RasterizerState = rasterizerState;
			this.mDevice.GraphicsDevice.BlendState = BlendState.AlphaBlend;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000A719 File Offset: 0x00008919
		public void SetSamplerState(int theSampler, int theValue)
		{
			this.mStateMgr.SetSamplerState(SamplerState.LinearClamp);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000A72C File Offset: 0x0000892C
		public void SetRasterizerState(int fillMode, int cullMode)
		{
			RasterizerState rasterizerState = new RasterizerState();
			rasterizerState.FillMode = (FillMode)fillMode;
			rasterizerState.CullMode = (CullMode)cullMode;
			this.mStateMgr.SetRasterizerState(rasterizerState);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000A75C File Offset: 0x0000895C
		public override bool SetTexture(int inTextureIndex, Image inImage)
		{
			if (inImage == null)
			{
				this.mImage = null;
				this.SetTextureDirect(inTextureIndex, null);
				return true;
			}
			this.mImage = inImage;
			inImage = this.SetupAtlasState(inTextureIndex, inImage);
			MemoryImage memoryImage = inImage.AsMemoryImage();
			if (memoryImage == null)
			{
				return false;
			}
			if (!this.CreateImageRenderData(ref memoryImage))
			{
				return false;
			}
			XNATextureData xnatextureData = (XNATextureData)memoryImage.GetRenderData();
			if ((xnatextureData.mImageFlags & 32UL) == 0UL && (xnatextureData.mImageFlags & 64UL) == 0UL)
			{
				this.SetTextureDirect(inTextureIndex, xnatextureData.mTextures[0].mTexture);
			}
			return true;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000A7E4 File Offset: 0x000089E4
		private void BltClipF(Image theImage, float theX, float theY, Rect theSrcRect, Rect theClipRect, SexyColor theColor, int theDrawMode)
		{
			SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
			sexyTransform2D.Translate(theX, theY);
			this.BltTransformed(theImage, theClipRect, theColor, theDrawMode, theSrcRect, sexyTransform2D, true, 0f, 0f, false);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000A823 File Offset: 0x00008A23
		public void BltNoClipF(Image theImage, float theX, float theY, Rect theSrcRect, SexyColor theColor, int theDrawMode, bool linearFilter)
		{
			if (this.mTransformStack.Count != 0)
			{
				this.BltClipF(theImage, theX, theY, theSrcRect, Rect.INVALIDATE_RECT, theColor, theDrawMode);
				return;
			}
			if (!this.PreDraw())
			{
				return;
			}
			this.BltHelper(theImage, theX, theY, theSrcRect, theColor, theDrawMode, linearFilter);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000A864 File Offset: 0x00008A64
		public void BltTransformHelper(Image theImage, Rect theClipRect, SexyColor theColor, int theDrawMode, Rect theSrcRect, SexyTransform2D theTransform, bool linearFilter, float theX, float theY, bool center)
		{
			Image image = theImage;
			image.InitAtalasState();
			theImage = this.SetupAtlasState(0, theImage);
			int mX = theSrcRect.mX;
			int mY = theSrcRect.mY;
			int num = mX + theSrcRect.mWidth;
			int num2 = mY + theSrcRect.mHeight;
			int num3 = 0;
			int num4 = 0;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			float num8 = 0f;
			float num9 = 0f;
			float num10 = 0f;
			MemoryImage memoryImage = theImage as MemoryImage;
			if (!this.CreateImageRenderData(ref memoryImage))
			{
				return;
			}
			this.SetupDrawMode(theDrawMode);
			bool flag = false;
			if (theDrawMode == 0 && !memoryImage.mHasAlpha && theColor.mAlpha >= 255 && image.mWidth * image.mHeight > 40000)
			{
				this.SetBlend(Graphics3D.EBlendMode.BLEND_ONE, Graphics3D.EBlendMode.BLEND_ZERO);
				flag = true;
			}
			XNATextureData xnatextureData = (XNATextureData)memoryImage.GetRenderData();
			if (center)
			{
				num9 = (float)(-(float)theSrcRect.mWidth) / 2f;
				num10 = (float)(-(float)theSrcRect.mHeight) / 2f;
			}
			int num11 = mY;
			float num12 = num10;
			if (mX >= num || mY >= num2)
			{
				return;
			}
			theTransform.Translate(theX, theY);
			float z = 0f;
			Color color = new Color(theColor.mRed, theColor.mGreen, theColor.mBlue, theColor.mAlpha);
			int num13 = mX;
			float num14 = num9;
			num3 = num - num13;
			num4 = num2 - num11;
			Texture2D texture = xnatextureData.GetTexture(image as MemoryImage, num13, num11, ref num3, ref num4, ref num5, ref num6, ref num7, ref num8);
			this.SetTextureDirect(0, texture);
			float num15 = num14;
			float num16 = num12;
			this.mTmpVPCTBuffer[0].Position.X = num15;
			this.mTmpVPCTBuffer[0].Position.Y = num16;
			this.mTmpVPCTBuffer[0].Position.Z = z;
			this.mTmpVPCTBuffer[0].Color = color;
			this.mTmpVPCTBuffer[0].TextureCoordinate = image.mVectorBase + image.mVectorU * num5 + image.mVectorV * num6;
			this.mTmpVPCTBuffer[1].Position.X = num15;
			this.mTmpVPCTBuffer[1].Position.Y = num16 + (float)num4;
			this.mTmpVPCTBuffer[1].Position.Z = z;
			this.mTmpVPCTBuffer[1].Color = color;
			this.mTmpVPCTBuffer[1].TextureCoordinate = image.mVectorBase + image.mVectorU * num5 + image.mVectorV * num8;
			this.mTmpVPCTBuffer[2].Position.X = num15 + (float)num3;
			this.mTmpVPCTBuffer[2].Position.Y = num16;
			this.mTmpVPCTBuffer[2].Position.Z = z;
			this.mTmpVPCTBuffer[2].Color = color;
			this.mTmpVPCTBuffer[2].TextureCoordinate = image.mVectorBase + image.mVectorU * num7 + image.mVectorV * num6;
			this.mTmpVPCTBuffer[3].Position.X = num15 + (float)num3;
			this.mTmpVPCTBuffer[3].Position.Y = num16 + (float)num4;
			this.mTmpVPCTBuffer[3].Position.Z = z;
			this.mTmpVPCTBuffer[3].Color = color;
			this.mTmpVPCTBuffer[3].TextureCoordinate = image.mVectorBase + image.mVectorU * num7 + image.mVectorV * num8;
			Matrix mMatrix = theTransform.mMatrix;
			for (int i = 0; i < 4; i++)
			{
				Vector3.Transform(ref this.mTmpVPCTBuffer[i].Position, ref mMatrix, out this.mTmpVPCTBuffer[i].Position);
			}
			Rect rect = theClipRect;
			bool flag2 = false;
			if (rect != Rect.INVALIDATE_RECT && (rect.mX != 0 || rect.mY != 0 || rect.mWidth != this.mWidth || rect.mHeight != this.mHeight))
			{
				SexyVector2 sexyVector = new SexyVector2((float)rect.mX, (float)rect.mY);
				SexyVector2 sexyVector2 = new SexyVector2((float)(rect.mX + rect.mWidth), (float)(rect.mY + rect.mHeight));
				for (int j = 0; j < 4; j++)
				{
					if (this.mTmpVPCTBuffer[j].Position.X < sexyVector.x || this.mTmpVPCTBuffer[j].Position.X >= sexyVector2.x || this.mTmpVPCTBuffer[j].Position.Y < sexyVector.y || this.mTmpVPCTBuffer[j].Position.Y >= sexyVector2.y)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (flag2)
			{
				VertexPositionColorTexture vertexPositionColorTexture = this.mTmpVPCTBuffer[2];
				this.mTmpVPCTBuffer[2] = this.mTmpVPCTBuffer[3];
				this.mTmpVPCTBuffer[3] = vertexPositionColorTexture;
				this.DrawPolyClipped(rect, this.mTmpVPCTBuffer);
			}
			else
			{
				this.BufferedDrawPrimitive(5, 2, this.mTmpVPCTBuffer, 32, this.mDefaultVertexFVF, Matrix.Identity);
			}
			if (flag)
			{
				this.SetBlend(Graphics3D.EBlendMode.BLEND_DEFAULT, Graphics3D.EBlendMode.BLEND_DEFAULT);
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000AE48 File Offset: 0x00009048
		public override void DrawSprite(Image theImage, SexyColor theColor, int theDrawMode, SexyTransform2D theTransform, Rect theSrcRect, bool center)
		{
			Image image = theImage;
			image.InitAtalasState();
			theImage = this.SetupAtlasState(0, theImage);
			MemoryImage memoryImage = theImage as MemoryImage;
			if (!this.CreateImageRenderData(ref memoryImage))
			{
				return;
			}
			this.SetupDrawMode(theDrawMode);
			XNATextureData xnatextureData = (XNATextureData)memoryImage.GetRenderData();
			new SexyColor(theColor.mRed, theColor.mGreen, theColor.mBlue, theColor.mAlpha);
			Rectangle rectangle = new Rectangle(image.mAtlasStartX + theSrcRect.mX, image.mAtlasStartY + theSrcRect.mY, theSrcRect.mWidth, theSrcRect.mHeight);
			Texture2D texture2D = xnatextureData.mTextures[0].mTexture;
			Vector2 vector = new Vector2(theTransform.mMatrix.M41, theTransform.mMatrix.M42);
			Vector2 vector2;
			if (center)
			{
				vector2 = new Vector2((float)rectangle.Width / 2f, (float)rectangle.Height / 2f);
			}
			else
			{
				vector2 = new Vector2(0f, 0f);
			}
			float num = (float)Math.Sqrt((double)(theTransform.mMatrix.M11 * theTransform.mMatrix.M11 + theTransform.mMatrix.M12 * theTransform.mMatrix.M12)) * (float)Math.Sqrt((double)(theTransform.mMatrix.M21 * theTransform.mMatrix.M21 + theTransform.mMatrix.M22 * theTransform.mMatrix.M22));
			float num2 = (float)Math.Asin((double)theTransform.mMatrix.M21);
			this.mSpriteBatch.Draw(texture2D, vector, new Rectangle?(rectangle), Color.White, num2, vector2, num, 0, 0f);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000AFF9 File Offset: 0x000091F9
		public override void BeginSprite()
		{
			this.FlushBufferedTriangles();
			this.mSpriteBatch.Begin();
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000B00C File Offset: 0x0000920C
		public override void EndSprite()
		{
			this.mSpriteBatch.End();
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000B01C File Offset: 0x0000921C
		public void BltHelper(Image theImage, float theX, float theY, Rect theSrcRect, SexyColor theColor, int theDrawMode, bool linearFilter)
		{
			Image image = theImage;
			image.InitAtalasState();
			theImage = this.SetupAtlasState(0, theImage);
			MemoryImage memoryImage = theImage as MemoryImage;
			if (!this.CreateImageRenderData(ref memoryImage))
			{
				return;
			}
			this.SetupDrawMode(theDrawMode);
			XNATextureData xnatextureData = (XNATextureData)memoryImage.GetRenderData();
			int mX = theSrcRect.mX;
			int mY = theSrcRect.mY;
			int num = mX + theSrcRect.mWidth;
			int num2 = mY + theSrcRect.mHeight;
			int num3 = 0;
			int num4 = 0;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			float num8 = 0f;
			int num9 = mY;
			if (mX >= num || mY >= num2)
			{
				return;
			}
			float z = 0f;
			Color color = new Color(theColor.mRed, theColor.mGreen, theColor.mBlue, theColor.mAlpha);
			int num10 = mX;
			num3 = num - num10;
			num4 = num2 - num9;
			Texture2D texture = xnatextureData.GetTexture((MemoryImage)image, num10, num9, ref num3, ref num4, ref num5, ref num6, ref num7, ref num8);
			this.mTmpVPCTBuffer[0].Position.X = theX;
			this.mTmpVPCTBuffer[0].Position.Y = theY;
			this.mTmpVPCTBuffer[0].Position.Z = z;
			this.mTmpVPCTBuffer[0].Color = color;
			this.mTmpVPCTBuffer[0].TextureCoordinate = image.mVectorBase + image.mVectorU * num5 + image.mVectorV * num6;
			this.mTmpVPCTBuffer[1].Position.X = theX;
			this.mTmpVPCTBuffer[1].Position.Y = theY + (float)num4;
			this.mTmpVPCTBuffer[1].Position.Z = z;
			this.mTmpVPCTBuffer[1].Color = color;
			this.mTmpVPCTBuffer[1].TextureCoordinate = image.mVectorBase + image.mVectorU * num5 + image.mVectorV * num8;
			this.mTmpVPCTBuffer[2].Position.X = theX + (float)num3;
			this.mTmpVPCTBuffer[2].Position.Y = theY;
			this.mTmpVPCTBuffer[2].Position.Z = z;
			this.mTmpVPCTBuffer[2].Color = color;
			this.mTmpVPCTBuffer[2].TextureCoordinate = image.mVectorBase + image.mVectorU * num7 + image.mVectorV * num6;
			this.mTmpVPCTBuffer[3].Position.X = theX + (float)num3;
			this.mTmpVPCTBuffer[3].Position.Y = theY + (float)num4;
			this.mTmpVPCTBuffer[3].Position.Z = z;
			this.mTmpVPCTBuffer[3].Color = color;
			this.mTmpVPCTBuffer[3].TextureCoordinate = image.mVectorBase + image.mVectorU * num7 + image.mVectorV * num8;
			this.SetTextureDirect(0, texture);
			this.BufferedDrawPrimitive(5, 2, this.mTmpVPCTBuffer, 32, this.mDefaultVertexFVF, Matrix.Identity);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000B3B0 File Offset: 0x000095B0
		public bool PreDraw()
		{
			if (!this.mSceneBegun)
			{
				this.mSceneBegun = true;
				RenderStateManager.Context context = this.mStateMgr.GetContext();
				this.mStateMgr.SetContext(null);
				this.mStateMgr.RevertState();
				this.mStateMgr.ApplyContextDefaults();
				this.mStateMgr.PushState();
				if (!this.mStateMgr.CommitState())
				{
					this.mStateMgr.SetContext(context);
					return false;
				}
			}
			return true;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000B421 File Offset: 0x00009621
		public void SetupDrawMode(int theDrawMode)
		{
			if (theDrawMode == 0)
			{
				this.mStateMgr.SetBlendStateState(this.mNormalState);
				return;
			}
			this.mStateMgr.SetBlendStateState(this.mAdditiveState);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000B44C File Offset: 0x0000964C
		public Texture2D CreateTexture2D(int theWidth, int theHeight, PixelFormat theFormat, bool renderTarget, XNATextureData theTexData, XNATextureDataPiece[] theTexDataPiece)
		{
			GlobalMembers.gTotalGraphicsMemory += theWidth * theHeight * 4;
			SurfaceFormat xnaFormat = this.GetXnaFormat(theFormat);
			if (renderTarget)
			{
				return new RenderTarget2D(this.mDevice.GraphicsDevice, theWidth, theHeight);
			}
			return new Texture2D(this.mDevice.GraphicsDevice, theWidth, theHeight, false, xnaFormat);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000B4A0 File Offset: 0x000096A0
		public Texture2D CreateTexture2DFromData(byte[] data)
		{
			MemoryStream memoryStream = new MemoryStream(data);
			Texture2D texture2D = Texture2D.FromStream(this.mDevice.GraphicsDevice, memoryStream);
			GlobalMembers.gTotalGraphicsMemory += texture2D.Width * texture2D.Height * 4;
			return texture2D;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000B4E4 File Offset: 0x000096E4
		public DeviceImage GetOptimizedImage(Texture2D texture, bool commitBits, bool allowTriReps)
		{
			GlobalMembers.gTotalGraphicsMemory += texture.Width * texture.Height * 4;
			DeviceImage deviceImage = new DeviceImage();
			deviceImage.mApp = GlobalMembers.gSexyAppBase;
			deviceImage.mFileName = texture.Name;
			deviceImage.mWidth = texture.Width;
			deviceImage.mHeight = texture.Height;
			deviceImage.mHasAlpha = true;
			XNATextureData xnatextureData = new XNATextureData(this);
			deviceImage.SetRenderData(xnatextureData);
			xnatextureData.mWidth = texture.Width;
			xnatextureData.mHeight = texture.Height;
			xnatextureData.mTexPieceWidth = texture.Width;
			xnatextureData.mTexPieceHeight = texture.Height;
			xnatextureData.mTexVecWidth = 1;
			xnatextureData.mTexVecHeight = 1;
			xnatextureData.mPixelFormat = this.GetSexyFormat(texture.Format);
			xnatextureData.mMaxTotalU = 1f;
			xnatextureData.mMaxTotalV = 1f;
			xnatextureData.mImageFlags = deviceImage.GetImageFlags();
			xnatextureData.mOptimizedLoad = true;
			xnatextureData.mTextures[0].mWidth = texture.Width;
			xnatextureData.mTextures[0].mHeight = texture.Height;
			xnatextureData.mTextures[0].mTexture = texture;
			return deviceImage;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000B604 File Offset: 0x00009804
		public SurfaceFormat GetXnaFormat(PixelFormat theFormat)
		{
			switch (theFormat)
			{
			case PixelFormat.PixelFormat_A8R8G8B8:
				return SurfaceFormat.Color;
			case PixelFormat.PixelFormat_A4R4G4B4:
				return SurfaceFormat.Bgra4444;
			case (PixelFormat)3:
				break;
			case PixelFormat.PixelFormat_R5G6B5:
				return SurfaceFormat.Bgr565;
			default:
				if (theFormat == PixelFormat.PixelFormat_X8R8G8B8)
				{
					return SurfaceFormat.Color;
				}
				break;
			}
			return SurfaceFormat.Color;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000B63C File Offset: 0x0000983C
		public PixelFormat GetSexyFormat(SurfaceFormat theFormat)
		{
			switch (theFormat)
			{
			case SurfaceFormat.Color:
				return PixelFormat.PixelFormat_A8R8G8B8;
			case SurfaceFormat.Bgr565:
				return PixelFormat.PixelFormat_R5G6B5;
			case SurfaceFormat.Bgra4444:
				return PixelFormat.PixelFormat_A4R4G4B4;
			}
			return PixelFormat.PixelFormat_A8R8G8B8;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000B66C File Offset: 0x0000986C
		public CompareFunction GetXNACompareFunc(Graphics3D.ECompareFunc func)
		{
			switch (func)
			{
			case Graphics3D.ECompareFunc.COMPARE_NEVER:
				return CompareFunction.Never;
			case Graphics3D.ECompareFunc.COMPARE_LESS:
				return CompareFunction.Less;
			case Graphics3D.ECompareFunc.COMPARE_EQUAL:
				return CompareFunction.Equal;
			case Graphics3D.ECompareFunc.COMPARE_LESSEQUAL:
				return CompareFunction.LessEqual;
			case Graphics3D.ECompareFunc.COMPARE_GREATER:
				return CompareFunction.Greater;
			case Graphics3D.ECompareFunc.COMPARE_NOTEQUAL:
				return CompareFunction.NotEqual;
			case Graphics3D.ECompareFunc.COMPARE_GREATEREQUAL:
				return CompareFunction.GreaterEqual;
			case Graphics3D.ECompareFunc.COMPARE_ALWAYS:
				return CompareFunction.Always;
			default:
				return CompareFunction.Never;
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000B6B8 File Offset: 0x000098B8
		public Blend GetXNABlendMode(Graphics3D.EBlendMode mode)
		{
			switch (mode)
			{
			case Graphics3D.EBlendMode.BLEND_ZERO:
				return Blend.Zero;
			case Graphics3D.EBlendMode.BLEND_ONE:
				return Blend.One;
			case Graphics3D.EBlendMode.BLEND_SRCCOLOR:
				return Blend.SourceColor;
			case Graphics3D.EBlendMode.BLEND_INVSRCCOLOR:
				return Blend.InverseSourceColor;
			case Graphics3D.EBlendMode.BLEND_SRCALPHA:
				return Blend.SourceAlpha;
			case Graphics3D.EBlendMode.BLEND_INVSRCALPHA:
				return Blend.InverseSourceAlpha;
			case (Graphics3D.EBlendMode)7:
			case (Graphics3D.EBlendMode)8:
				break;
			case Graphics3D.EBlendMode.BLEND_DESTCOLOR:
				return Blend.DestinationColor;
			case Graphics3D.EBlendMode.BLEND_INVDESTCOLOR:
				return Blend.InverseDestinationColor;
			case Graphics3D.EBlendMode.BLEND_SRCALPHASAT:
				return Blend.SourceAlphaSaturation;
			default:
				if (mode == Graphics3D.EBlendMode.BLEND_DEFAULT)
				{
					return Blend.SourceAlpha;
				}
				break;
			}
			return Blend.One;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000B71C File Offset: 0x0000991C
		public Matrix GetXNAMatrix(SexyMatrix4 mat)
		{
			return new Matrix(mat.m00, mat.m01, mat.m02, mat.m03, mat.m10, mat.m11, mat.m12, mat.m13, mat.m20, mat.m21, mat.m22, mat.m23, mat.m30, mat.m31, mat.m32, mat.m33);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000B794 File Offset: 0x00009994
		public Matrix GetXNAMatrix(SexyMatrix3 mat)
		{
			return new Matrix(mat.m00, mat.m10, mat.m20, 0f, mat.m01, mat.m11, mat.m21, 0f, 0f, 0f, mat.m22, 0f, mat.m02, mat.m12, 0f, 1f);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000B802 File Offset: 0x00009A02
		public Color GetXNAColor(SexyColor color)
		{
			return new Color(color.mRed, color.mGreen, color.mBlue, color.mAlpha);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000B825 File Offset: 0x00009A25
		public void CopyImageToTexture(ref Texture2D theTexture, int theTextureFormat, MemoryImage theImage, int offx, int offy, int texWidth, int texHeight, PixelFormat theFormat)
		{
			if (theTexture == null)
			{
				return;
			}
			theTexture.SetData<uint>(theImage.GetBits());
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000B83C File Offset: 0x00009A3C
		public void BufferedDrawPrimitive(int thePrimType, int thePrimCount, VertexPositionColorTexture[] theVertices, int theVertexSize, ulong theVertexFormat, Matrix transform)
		{
			this.CheckBatchAndCommit();
			int num = 0;
			switch (thePrimType)
			{
			case 4:
				while (thePrimCount > 0)
				{
					if (this.mBatchedTriangleIndex > BaseXNARenderDevice.mBatchedTriangleSize - 3)
					{
						this.DoCommitAllRenderState();
						this.FlushBufferedTriangles();
					}
					this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = theVertices[num++];
					this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = theVertices[num++];
					this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = theVertices[num++];
					thePrimCount--;
				}
				break;
			case 5:
				if (thePrimCount * 3 > BaseXNARenderDevice.mBatchedTriangleSize - this.mBatchedTriangleIndex)
				{
					this.DoCommitAllRenderState();
					this.FlushBufferedTriangles();
				}
				this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = theVertices[num++];
				this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = theVertices[num++];
				this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = theVertices[num++];
				for (thePrimCount--; thePrimCount > 0; thePrimCount--)
				{
					this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex] = this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex - 2];
					this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex + 1] = this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex - 1];
					this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex + 2] = theVertices[num++];
					this.mBatchedTriangleIndex += 3;
				}
				break;
			case 6:
			{
				if (thePrimCount * 3 > BaseXNARenderDevice.mBatchedTriangleSize - this.mBatchedTriangleIndex)
				{
					this.DoCommitAllRenderState();
					this.FlushBufferedTriangles();
				}
				int num2 = this.mBatchedTriangleIndex;
				this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = theVertices[num++];
				this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = theVertices[num++];
				this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex++] = theVertices[num++];
				for (thePrimCount--; thePrimCount > 0; thePrimCount--)
				{
					this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex] = this.mBatchedTriangleBuffer[num2];
					this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex + 1] = this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex - 1];
					this.mBatchedTriangleBuffer[this.mBatchedTriangleIndex + 2] = theVertices[num++];
					this.mBatchedTriangleIndex += 3;
				}
				break;
			}
			}
			if (this.mBatchedTriangleIndex + 3 > BaseXNARenderDevice.mBatchedTriangleSize)
			{
				this.DoCommitAllRenderState();
				this.FlushBufferedTriangles();
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000BC10 File Offset: 0x00009E10
		public void DrawPrimitiveInternal<T>(int inPrimType, int inPrimCount, T[] inVertData, ulong inVertStride, ulong inVertFormat, bool inDoCommit, Matrix transform) where T : struct, IVertexType
		{
			int num = 0;
			if (inPrimType == 4)
			{
				num = inPrimCount * 3;
			}
			else if (inPrimType == 5 || inPrimType == 6)
			{
				num = inPrimCount + 2;
			}
			else if (inPrimType == 3)
			{
				num = inPrimCount + 1;
			}
			if (num != 0)
			{
				if (inDoCommit)
				{
					this.CheckBatchAndCommit();
					this.DoCommitAllRenderState();
				}
				PrimitiveType primitiveType = PrimitiveType.TriangleList;
				if (inPrimType == 4)
				{
					primitiveType = PrimitiveType.TriangleList;
				}
				else if (inPrimType == 5)
				{
					primitiveType = PrimitiveType.TriangleStrip;
				}
				else
				{
					if (inPrimType == 6)
					{
						return;
					}
					if (inPrimType == 3)
					{
						primitiveType = PrimitiveType.LineStrip;
					}
				}
				foreach (EffectPass effectPass in this.mBasicEffect.CurrentTechnique.Passes)
				{
					effectPass.Apply();
					try
					{
						this.mBasicEffect.GraphicsDevice.DrawUserPrimitives<T>(primitiveType, inVertData, 0, inPrimCount);
					}
					catch (Exception)
					{
					}
				}
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000BCE8 File Offset: 0x00009EE8
		public void DrawIndexPrimitiveInternal<T>(int inPrimType, int inPrimCount, T[] inVertData, ulong inVertStride, ulong inVertFormat, bool inDoCommit, Matrix transform) where T : struct, IVertexType
		{
			if (inDoCommit)
			{
				this.CheckBatchAndCommit();
				this.DoCommitAllRenderState();
			}
			PrimitiveType primitiveType = PrimitiveType.TriangleList;
			if (inPrimType == 4)
			{
				primitiveType = PrimitiveType.TriangleList;
			}
			else if (inPrimType == 5)
			{
				primitiveType = PrimitiveType.TriangleStrip;
			}
			else
			{
				if (inPrimType == 6)
				{
					return;
				}
				if (inPrimType == 3)
				{
					primitiveType = PrimitiveType.LineStrip;
				}
			}
			foreach (EffectPass effectPass in this.mBasicEffect.CurrentTechnique.Passes)
			{
				effectPass.Apply();
				try
				{
					this.mBasicEffect.GraphicsDevice.DrawUserIndexedPrimitives<T>(primitiveType, inVertData, 0, this.mBatchedTriangleIndex, this.mBatchedIndexBuffer, 0, inPrimCount);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000BDA4 File Offset: 0x00009FA4
		public VertexBuffer InternalCreateVertexBuffer(int inCount, VertexDeclaration vDec, BufferUsage usage)
		{
			return new VertexBuffer(this.mDevice.GraphicsDevice, vDec, inCount, usage);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000BDB9 File Offset: 0x00009FB9
		public IndexBuffer InternalCreateIndexBuffer(int indexCount, IndexElementSize size, BufferUsage usage)
		{
			return new IndexBuffer(this.mDevice.GraphicsDevice, size, indexCount, usage);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000BDCE File Offset: 0x00009FCE
		public override Image SwapScreenImage(ref DeviceImage ioSrcImage, ref RenderSurface ioSrcSurface, uint flags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000BDD5 File Offset: 0x00009FD5
		public override void CopyScreenImage(DeviceImage ioDstImage, uint flags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000BDDC File Offset: 0x00009FDC
		public override RenderEffect GetEffect(RenderEffectDefinition inDefinition)
		{
			return null;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000BDDF File Offset: 0x00009FDF
		public void SetOrientation(int Orientation)
		{
			this.mDevice.ApplyChanges();
		}

		// Token: 0x0400014C RID: 332
		public GraphicsDeviceManager mDevice;

		// Token: 0x0400014D RID: 333
		protected BasicEffect mBasicEffect;

		// Token: 0x0400014E RID: 334
		protected AlphaTestEffect mAlphaTestEffect;

		// Token: 0x0400014F RID: 335
		public BaseXNAStateManager mStateMgr;

		// Token: 0x04000150 RID: 336
		public BlendState mBlendState;

		// Token: 0x04000151 RID: 337
		public Matrix mProjectionMatrix;

		// Token: 0x04000152 RID: 338
		public Matrix mViewMatrix;

		// Token: 0x04000153 RID: 339
		private float mPixelOffset;

		// Token: 0x04000154 RID: 340
		private int mMinTextureWidth;

		// Token: 0x04000155 RID: 341
		private int mMinTextureHeight;

		// Token: 0x04000156 RID: 342
		private int mMaxTextureWidth;

		// Token: 0x04000157 RID: 343
		private int mMaxTextureHeight;

		// Token: 0x04000158 RID: 344
		private int mMaxTextureAspectRatio;

		// Token: 0x04000159 RID: 345
		private uint mRenderModeFlags;

		// Token: 0x0400015A RID: 346
		public uint mSupportedTextureFormats;

		// Token: 0x0400015B RID: 347
		public bool mTextureSizeMustBePow2;

		// Token: 0x0400015C RID: 348
		public bool mRenderTargetMustBePow2;

		// Token: 0x0400015D RID: 349
		private ulong mDefaultVertexSize;

		// Token: 0x0400015E RID: 350
		public ulong mDefaultVertexFVF;

		// Token: 0x0400015F RID: 351
		private int mWidth;

		// Token: 0x04000160 RID: 352
		private int mHeight;

		// Token: 0x04000161 RID: 353
		private int mScreenWidth;

		// Token: 0x04000162 RID: 354
		private int mScreenHeight;

		// Token: 0x04000163 RID: 355
		private bool mSceneBegun;

		// Token: 0x04000164 RID: 356
		private VertexPositionColorTexture[] mBatchedTriangleBuffer;

		// Token: 0x04000165 RID: 357
		private short[] mBatchedIndexBuffer;

		// Token: 0x04000166 RID: 358
		private int mBatchedTriangleIndex;

		// Token: 0x04000167 RID: 359
		private int mBatchedIndexIndex;

		// Token: 0x04000168 RID: 360
		private static int mBatchedTriangleSize = 1200;

		// Token: 0x04000169 RID: 361
		private IGraphicsDriver mGraphicsDriver;

		// Token: 0x0400016A RID: 362
		private Texture2D mTexture;

		// Token: 0x0400016B RID: 363
		private Game mGame;

		// Token: 0x0400016C RID: 364
		public Image mImage;

		// Token: 0x0400016D RID: 365
		public Transform mTransform = new Transform();

		// Token: 0x0400016E RID: 366
		private HRenderContext mCurrentContex;

		// Token: 0x0400016F RID: 367
		private HRenderContext GlobalRenderContex;

		// Token: 0x04000170 RID: 368
		private Stack<SexyTransform2D> mTransformStack;

		// Token: 0x04000171 RID: 369
		private static bool SUPPORT_HW_CLIP = false;

		// Token: 0x04000172 RID: 370
		public SpriteBatch mSpriteBatch;

		// Token: 0x04000173 RID: 371
		public VertexPositionColorTexture[] mTmpVPCTBuffer;

		// Token: 0x04000174 RID: 372
		public VertexPositionColor[] mTmpVPCBuffer;

		// Token: 0x04000175 RID: 373
		private BlendState mNormalState = new BlendState();

		// Token: 0x04000176 RID: 374
		private BlendState mAdditiveState = new BlendState();

		// Token: 0x04000177 RID: 375
		private int mCurDrawMode;

		// Token: 0x04000178 RID: 376
		private RenderTarget2D mScreenTarget;

		// Token: 0x04000179 RID: 377
		public Rectangle mRenderRect = new Rectangle(0, 0, 800, 480);

		// Token: 0x0200003E RID: 62
		public enum ClipperType
		{
			// Token: 0x0400017B RID: 379
			Clipper_Less,
			// Token: 0x0400017C RID: 380
			Clipper_Greater,
			// Token: 0x0400017D RID: 381
			Clipper_Equal,
			// Token: 0x0400017E RID: 382
			Clipper_GreaterEqual,
			// Token: 0x0400017F RID: 383
			Clipper_LessEqual
		}
	}
}
