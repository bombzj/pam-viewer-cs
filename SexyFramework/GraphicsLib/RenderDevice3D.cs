using System;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x02000037 RID: 55
	public abstract class RenderDevice3D : RenderDevice
	{
		// Token: 0x06000234 RID: 564 RVA: 0x00007DBA File Offset: 0x00005FBA
		public int Flush()
		{
			return this.Flush(2U);
		}

		// Token: 0x06000235 RID: 565
		public abstract int Flush(uint inFlushFlags);

		// Token: 0x06000236 RID: 566
		public abstract int Present(Rect theSrcRect, Rect theDestRect);

		// Token: 0x06000237 RID: 567
		public abstract uint GetCapsFlags();

		// Token: 0x06000238 RID: 568 RVA: 0x00007DC3 File Offset: 0x00005FC3
		public bool SupportsPixelShaders()
		{
			return (this.GetCapsFlags() & 2U) != 0U;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00007DD3 File Offset: 0x00005FD3
		public bool SupportsVertexShaders()
		{
			return (this.GetCapsFlags() & 4U) != 0U;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00007DE3 File Offset: 0x00005FE3
		public bool SupportsCubeMaps()
		{
			return (this.GetCapsFlags() & 32U) != 0U;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00007DF4 File Offset: 0x00005FF4
		public bool SupportsVolumeMaps()
		{
			return (this.GetCapsFlags() & 64U) != 0U;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00007E05 File Offset: 0x00006005
		public bool SupportsImageRenderTargets()
		{
			return (this.GetCapsFlags() & 8U) != 0U;
		}

		// Token: 0x0600023D RID: 573
		public abstract uint GetMaxTextureStages();

		// Token: 0x0600023E RID: 574
		public abstract string GetInfoString(RenderDevice3D.EInfoString inInfoStr);

		// Token: 0x0600023F RID: 575
		public abstract void GetBackBufferDimensions(ref uint outWidth, ref uint outHeight);

		// Token: 0x06000240 RID: 576
		public abstract int SceneBegun();

		// Token: 0x06000241 RID: 577
		public abstract bool CreateImageRenderData(ref MemoryImage inImage);

		// Token: 0x06000242 RID: 578
		public abstract void RemoveImageRenderData(MemoryImage inImage);

		// Token: 0x06000243 RID: 579
		public abstract int RecoverImageBitsFromRenderData(MemoryImage inImage);

		// Token: 0x06000244 RID: 580
		public abstract int GetTextureMemorySize(MemoryImage theImage);

		// Token: 0x06000245 RID: 581
		public abstract PixelFormat GetTextureFormat(MemoryImage theImage);

		// Token: 0x06000246 RID: 582
		public abstract Image SwapScreenImage(ref DeviceImage ioSrcImage, ref RenderSurface ioSrcSurface, uint flags);

		// Token: 0x06000247 RID: 583
		public abstract void CopyScreenImage(DeviceImage ioDstImage, uint flags);

		// Token: 0x06000248 RID: 584
		public abstract void AdjustVertexUVsEx(uint theVertexFormat, SexyVertex[] theVertices, int theVertexCount, int theVertexSize);

		// Token: 0x06000249 RID: 585 RVA: 0x00007E18 File Offset: 0x00006018
		public void DrawPrimitiveEx(uint theVertexFormat, Graphics3D.EPrimitiveType thePrimitiveType, SexyVertex2D[] theVertices, int thePrimitiveCount, SexyColor theColor, int theDrawMode, float tx, float ty, bool blend)
		{
			this.DrawPrimitiveEx(theVertexFormat, thePrimitiveType, theVertices, thePrimitiveCount, theColor, theDrawMode, tx, ty, blend, 0U);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00007E3C File Offset: 0x0000603C
		public void DrawPrimitiveEx(uint theVertexFormat, Graphics3D.EPrimitiveType thePrimitiveType, SexyVertex2D[] theVertices, int thePrimitiveCount, SexyColor theColor, int theDrawMode, float tx, float ty)
		{
			this.DrawPrimitiveEx(theVertexFormat, thePrimitiveType, theVertices, thePrimitiveCount, theColor, theDrawMode, tx, ty, true, 0U);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00007E60 File Offset: 0x00006060
		public void DrawPrimitiveEx(uint theVertexFormat, Graphics3D.EPrimitiveType thePrimitiveType, SexyVertex2D[] theVertices, int thePrimitiveCount, SexyColor theColor, int theDrawMode, float tx)
		{
			this.DrawPrimitiveEx(theVertexFormat, thePrimitiveType, theVertices, thePrimitiveCount, theColor, theDrawMode, tx, 0f, true, 0U);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00007E88 File Offset: 0x00006088
		public void DrawPrimitiveEx(uint theVertexFormat, Graphics3D.EPrimitiveType thePrimitiveType, SexyVertex2D[] theVertices, int thePrimitiveCount, SexyColor theColor, int theDrawMode)
		{
			this.DrawPrimitiveEx(theVertexFormat, thePrimitiveType, theVertices, thePrimitiveCount, theColor, theDrawMode, 0f, 0f, true, 0U);
		}

		// Token: 0x0600024D RID: 589
		public abstract void DrawPrimitiveEx(uint theVertexFormat, Graphics3D.EPrimitiveType thePrimitiveType, SexyVertex2D[] theVertices, int thePrimitiveCount, SexyColor theColor, int theDrawMode, float tx, float ty, bool blend, uint theFlags);

		// Token: 0x0600024E RID: 590
		public abstract void SetBltDepth(float inDepth);

		// Token: 0x0600024F RID: 591 RVA: 0x00007EB0 File Offset: 0x000060B0
		public void PushTransform(SexyMatrix3 theTransform)
		{
			this.PushTransform(theTransform, true);
		}

		// Token: 0x06000250 RID: 592
		public abstract void PushTransform(SexyMatrix3 theTransform, bool concatenate);

		// Token: 0x06000251 RID: 593
		public abstract void PopTransform();

		// Token: 0x06000252 RID: 594
		public abstract void PopTransform(ref SexyMatrix3 theTransform);

		// Token: 0x06000253 RID: 595
		public abstract void ClearColorBuffer(SexyColor inColor);

		// Token: 0x06000254 RID: 596
		public abstract void ClearDepthBuffer();

		// Token: 0x06000255 RID: 597
		public abstract void ClearStencilBuffer(int inStencil);

		// Token: 0x06000256 RID: 598
		public abstract void SetDepthState(Graphics3D.ECompareFunc inDepthTestFunc, bool inDepthWriteEnabled);

		// Token: 0x06000257 RID: 599
		public abstract void SetStencilState(Graphics3D.ECompareFunc inStencilTestFunc, int inRefStencil, bool inStencilEnable, Graphics3D.ETestResultFunc passFunc, Graphics3D.ETestResultFunc failFunc);

		// Token: 0x06000258 RID: 600
		public abstract void SetAlphaTest(Graphics3D.ECompareFunc inAlphaTestFunc, int inRefAlpha);

		// Token: 0x06000259 RID: 601
		public abstract void SetColorWriteState(int inWriteRedEnabled, int inWriteGreenEnabled, int inWriteBlueEnabled, int inWriteAlphaEnabled);

		// Token: 0x0600025A RID: 602
		public abstract void SetWireframe(int inWireframe);

		// Token: 0x0600025B RID: 603
		public abstract void SetBlend(Graphics3D.EBlendMode inSrcBlend, Graphics3D.EBlendMode inDestBlend);

		// Token: 0x0600025C RID: 604
		public abstract void SetBackfaceCulling(int inCullClockwise, int inCullCounterClockwise);

		// Token: 0x0600025D RID: 605
		public abstract void SetLightingEnabled(int inLightingEnabled);

		// Token: 0x0600025E RID: 606
		public abstract void SetLightEnabled(int inLightIndex, int inEnabled);

		// Token: 0x0600025F RID: 607
		public abstract void SetGlobalAmbient(SexyColor inColor);

		// Token: 0x06000260 RID: 608 RVA: 0x00007EBA File Offset: 0x000060BA
		public void SetMaterialAmbient(SexyColor inColor)
		{
			this.SetMaterialAmbient(inColor, -1);
		}

		// Token: 0x06000261 RID: 609
		public abstract void SetMaterialAmbient(SexyColor inColor, int inVertexColorComponent);

		// Token: 0x06000262 RID: 610 RVA: 0x00007EC4 File Offset: 0x000060C4
		public void SetMaterialDiffuse(SexyColor inColor)
		{
			this.SetMaterialDiffuse(inColor, -1);
		}

		// Token: 0x06000263 RID: 611
		public abstract void SetMaterialDiffuse(SexyColor inColor, int inVertexColorComponent);

		// Token: 0x06000264 RID: 612 RVA: 0x00007ECE File Offset: 0x000060CE
		public void SetMaterialSpecular(SexyColor inColor, int inVertexColorComponent)
		{
			this.SetMaterialSpecular(inColor, inVertexColorComponent, 0f);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00007EDD File Offset: 0x000060DD
		public void SetMaterialSpecular(SexyColor inColor)
		{
			this.SetMaterialSpecular(inColor, -1, 0f);
		}

		// Token: 0x06000266 RID: 614
		public abstract void SetMaterialSpecular(SexyColor inColor, int inVertexColorComponent, float inPower);

		// Token: 0x06000267 RID: 615 RVA: 0x00007EEC File Offset: 0x000060EC
		public void SetMaterialEmissive(SexyColor inColor)
		{
			this.SetMaterialEmissive(inColor, -1);
		}

		// Token: 0x06000268 RID: 616
		public abstract void SetMaterialEmissive(SexyColor inColor, int inVertexColorComponent);

		// Token: 0x06000269 RID: 617
		public abstract void SetWorldTransform(SexyMatrix4 inMatrix);

		// Token: 0x0600026A RID: 618
		public abstract void SetViewTransform(SexyMatrix4 inMatrix);

		// Token: 0x0600026B RID: 619
		public abstract void SetProjectionTransform(SexyMatrix4 inMatrix);

		// Token: 0x0600026C RID: 620 RVA: 0x00007EF6 File Offset: 0x000060F6
		public void SetTextureTransform(int inTextureIndex, SexyMatrix4 inMatrix)
		{
			this.SetTextureTransform(inTextureIndex, inMatrix, 2);
		}

		// Token: 0x0600026D RID: 621
		public abstract void SetTextureTransform(int inTextureIndex, SexyMatrix4 inMatrix, int inNumDimensions);

		// Token: 0x0600026E RID: 622
		public abstract void SetViewport(int theX, int theY, int theWidth, int theHeight, float theMinZ, float theMaxZ);

		// Token: 0x0600026F RID: 623
		public abstract void SetRenderRect(int theX, int theY, int theWidth, int theHeight);

		// Token: 0x06000270 RID: 624
		public abstract bool SetTexture(int inTextureIndex, Image inImage);

		// Token: 0x06000271 RID: 625
		public abstract void SetTextureWrap(int inTextureIndex, bool inWrapU, bool inWrapV);

		// Token: 0x06000272 RID: 626 RVA: 0x00007F01 File Offset: 0x00006101
		public void SetTextureLinearFilter(int inTextureIndex)
		{
			this.SetTextureLinearFilter(inTextureIndex, true);
		}

		// Token: 0x06000273 RID: 627
		public abstract void SetTextureLinearFilter(int inTextureIndex, bool inLinear);

		// Token: 0x06000274 RID: 628 RVA: 0x00007F0B File Offset: 0x0000610B
		public void SetTextureCoordSource(int inTextureIndex, int inUVComponent)
		{
			this.SetTextureCoordSource(inTextureIndex, inUVComponent, Graphics3D.ETexCoordGen.TEXCOORDGEN_NONE);
		}

		// Token: 0x06000275 RID: 629
		public abstract void SetTextureCoordSource(int inTextureIndex, int inUVComponent, Graphics3D.ETexCoordGen inTexGen);

		// Token: 0x06000276 RID: 630
		public abstract void SetTextureFactor(int inTextureFactor);

		// Token: 0x06000277 RID: 631
		public abstract RenderEffect GetEffect(RenderEffectDefinition inDefinition);

		// Token: 0x06000278 RID: 632 RVA: 0x00007F16 File Offset: 0x00006116
		public virtual bool ReloadEffects()
		{
			return false;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00007F19 File Offset: 0x00006119
		public virtual bool ReloadEffects(int inDebug)
		{
			return this.ReloadEffects();
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00007F21 File Offset: 0x00006121
		public virtual void SetBltFilter(RenderDevice3D.FBltFilter inFilter, IntPtr inContext)
		{
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00007F23 File Offset: 0x00006123
		public virtual void SetDrawPrimFilter(RenderDevice3D.FDrawPrimFilter inFilter, IntPtr inContext)
		{
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00007F25 File Offset: 0x00006125
		public virtual bool LoadMesh(Mesh theMesh)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00007F2C File Offset: 0x0000612C
		public virtual void RenderMesh(Mesh theMesh, SexyMatrix4 theMatrix, SexyColor theColor)
		{
			this.RenderMesh(theMesh, theMatrix, theColor, true);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00007F38 File Offset: 0x00006138
		public virtual void RenderMesh(Mesh theMesh, SexyMatrix4 theMatrix)
		{
			this.RenderMesh(theMesh, theMatrix, SexyColor.White, true);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00007F48 File Offset: 0x00006148
		public virtual void RenderMesh(Mesh theMesh, SexyMatrix4 theMatrix, SexyColor theColor, bool doSetup)
		{
			throw new NotImplementedException();
		}

		// Token: 0x02000038 RID: 56
		public enum EFlushFlags
		{
			// Token: 0x04000135 RID: 309
			FLUSHF_BufferedTris = 1,
			// Token: 0x04000136 RID: 310
			FLUSHF_CurrentScene,
			// Token: 0x04000137 RID: 311
			FLUSHF_ManagedResources_Immediate = 4,
			// Token: 0x04000138 RID: 312
			FLUSHF_ManagedResources_OnPresent = 8,
			// Token: 0x04000139 RID: 313
			FLUSHF_BufferedState = 16
		}

		// Token: 0x02000039 RID: 57
		public enum ECapsFlags
		{
			// Token: 0x0400013B RID: 315
			CAPF_SingleImageTexture = 1,
			// Token: 0x0400013C RID: 316
			CAPF_PixelShaders,
			// Token: 0x0400013D RID: 317
			CAPF_VertexShaders = 4,
			// Token: 0x0400013E RID: 318
			CAPF_ImageRenderTargets = 8,
			// Token: 0x0400013F RID: 319
			CAPF_AutoWindowedVSync = 16,
			// Token: 0x04000140 RID: 320
			CAPF_CubeMaps = 32,
			// Token: 0x04000141 RID: 321
			CAPF_VolumeMaps = 64,
			// Token: 0x04000142 RID: 322
			CAPF_CopyScreenImage = 128,
			// Token: 0x04000143 RID: 323
			CAPF_LastLockScreenImage = 256
		}

		// Token: 0x0200003A RID: 58
		public enum EInfoString
		{
			// Token: 0x04000145 RID: 325
			INFOSTRING_Adapter,
			// Token: 0x04000146 RID: 326
			INFOSTRING_DrvProductVersion,
			// Token: 0x04000147 RID: 327
			INFOSTRING_DisplayMode,
			// Token: 0x04000148 RID: 328
			INFOSTRING_BackBuffer,
			// Token: 0x04000149 RID: 329
			INFOSTRING_TextureMemory,
			// Token: 0x0400014A RID: 330
			INFOSTRING_DrvResourceManager,
			// Token: 0x0400014B RID: 331
			INFOSTRING_DrvProductFeatures
		}

		// Token: 0x0200003B RID: 59
		// (Invoke) Token: 0x06000282 RID: 642
		public delegate int FBltFilter(IntPtr theContext, int thePrimType, uint thePrimCount, SexyVertex2D theVertices, int theVertexSize, Rect[] theClipRect);

		// Token: 0x0200003C RID: 60
		// (Invoke) Token: 0x06000286 RID: 646
		public delegate int FDrawPrimFilter(IntPtr theContext, int thePrimType, uint thePrimCount, SexyVertex2D theVertices, int theVertexSize);
	}
}
