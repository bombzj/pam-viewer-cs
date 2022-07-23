using System;
using System.Collections.Generic;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000B2 RID: 178
	public class Graphics3D
	{
		// Token: 0x06000596 RID: 1430 RVA: 0x000123C3 File Offset: 0x000105C3
		public Graphics3D(Graphics inGraphics, RenderDevice3D inRenderDevice, HRenderContext inRenderContext)
		{
			this.mGraphics = inGraphics;
			this.mRenderDevice = inRenderDevice;
			this.mRenderContext = inRenderContext;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x000123E0 File Offset: 0x000105E0
		protected void SetAsCurrentContext()
		{
			this.mRenderDevice.SetCurrentContext(this.mRenderContext);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x000123F3 File Offset: 0x000105F3
		public Graphics Get2D()
		{
			return this.mGraphics;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x000123FB File Offset: 0x000105FB
		public RenderDevice3D GetRenderDevice()
		{
			return this.mRenderDevice;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00012403 File Offset: 0x00010603
		public bool SupportsPixelShaders()
		{
			return this.mRenderDevice.SupportsPixelShaders();
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00012410 File Offset: 0x00010610
		public bool SupportsVertexShaders()
		{
			return this.mRenderDevice.SupportsVertexShaders();
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001241D File Offset: 0x0001061D
		public bool SupportsCubeMaps()
		{
			return this.mRenderDevice.SupportsCubeMaps();
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001242A File Offset: 0x0001062A
		public bool SupportsVolumeMaps()
		{
			return this.mRenderDevice.SupportsVolumeMaps();
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00012437 File Offset: 0x00010637
		public bool SupportsImageRenderTargets()
		{
			return this.mRenderDevice.SupportsImageRenderTargets();
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00012444 File Offset: 0x00010644
		public uint GetMaxTextureStages()
		{
			return this.mRenderDevice.GetMaxTextureStages();
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00012451 File Offset: 0x00010651
		public void AdjustVertexUVsEx(uint theVertexFormat, SexyVertex[] theVertices, int theVertexCount, int theVertexSize)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.AdjustVertexUVsEx(theVertexFormat, theVertices, theVertexCount, theVertexSize);
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001246C File Offset: 0x0001066C
		public void DrawPrimitiveEx(uint theVertexFormat, Graphics3D.EPrimitiveType thePrimitiveType, SexyVertex2D[] theVertices, int thePrimitiveCount, SexyColor theColor, int theDrawMode, float tx, float ty, bool blend, uint theFlags)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.DrawPrimitiveEx(theVertexFormat, thePrimitiveType, theVertices, thePrimitiveCount, theColor, theDrawMode, tx, ty, blend, theFlags);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001249C File Offset: 0x0001069C
		public void DrawPrimitive(uint theVertexFormat, Graphics3D.EPrimitiveType thePrimitiveType, SexyVertex2D[] theVertices, int thePrimitiveCount, SexyColor theColor, int theDrawMode, float tx, float ty, bool blend, uint theFlags)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.DrawPrimitiveEx((uint)SexyVertex2D.FVF, thePrimitiveType, theVertices, thePrimitiveCount, theColor, theDrawMode, tx, ty, blend, theFlags);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x000124CF File Offset: 0x000106CF
		public void SetBltDepth(float inDepth)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetBltDepth(inDepth);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x000124E3 File Offset: 0x000106E3
		public void PushTransform(SexyMatrix3 theTransform)
		{
			this.PushTransform(theTransform, false);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x000124ED File Offset: 0x000106ED
		public void PushTransform(SexyMatrix3 theTransform, bool concatenate)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.PushTransform(theTransform, concatenate);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00012502 File Offset: 0x00010702
		public void PopTransform()
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.PopTransform();
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00012515 File Offset: 0x00010715
		public void PopTransform(ref SexyMatrix3 theTransform)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.PopTransform(ref theTransform);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00012529 File Offset: 0x00010729
		public void ClearColorBuffer()
		{
			this.ClearColorBuffer(SexyColor.Black);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00012536 File Offset: 0x00010736
		public void ClearColorBuffer(SexyColor inColor)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.ClearColorBuffer(inColor);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001254A File Offset: 0x0001074A
		public void ClearDepthBuffer()
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.ClearDepthBuffer();
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001255D File Offset: 0x0001075D
		public void ClearStencilBuffer(int inStencil)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.ClearStencilBuffer(inStencil);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00012571 File Offset: 0x00010771
		public void SetDepthState(Graphics3D.ECompareFunc inDepthTestFunc, bool inDepthWriteEnabled)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetDepthState(inDepthTestFunc, inDepthWriteEnabled);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00012586 File Offset: 0x00010786
		public void SetAlphaTest(Graphics3D.ECompareFunc inAlphaTestFunc, int inRefAlpha)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetAlphaTest(inAlphaTestFunc, inRefAlpha);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001259B File Offset: 0x0001079B
		public void SetColorWriteState(int inWriteRedEnabled, int inWriteGreenEnabled, int inWriteBlueEnabled, int inWriteAlphaEnabled)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetColorWriteState(inWriteRedEnabled, inWriteGreenEnabled, inWriteBlueEnabled, inWriteAlphaEnabled);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x000125B3 File Offset: 0x000107B3
		public void SetWireframe(int inWireframe)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetWireframe(inWireframe);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x000125C7 File Offset: 0x000107C7
		public void SetBlend(Graphics3D.EBlendMode inSrcBlend, Graphics3D.EBlendMode inDestBlend)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetBlend(inSrcBlend, inDestBlend);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x000125DC File Offset: 0x000107DC
		public void SetBackfaceCulling(int inCullClockwise, int inCullCounterClockwise)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetBackfaceCulling(inCullClockwise, inCullCounterClockwise);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x000125F4 File Offset: 0x000107F4
		public void SetLightingEnabled(int inLightingEnabled, bool inSetDefaultMaterialState)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetLightingEnabled(inLightingEnabled);
			if (inLightingEnabled != 0 && inSetDefaultMaterialState)
			{
				this.SetMaterialAmbient(SexyColor.White);
				this.SetMaterialDiffuse(SexyColor.White, 0);
				this.SetMaterialSpecular(SexyColor.White);
				this.SetMaterialEmissive(SexyColor.Black);
			}
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00012646 File Offset: 0x00010846
		public void SetLightEnabled(int inLightIndex, int inEnabled)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetLightEnabled(inLightIndex, inEnabled);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001265B File Offset: 0x0001085B
		public void SetPointLight(int inLightIndex, SexyVector3 inPos, Graphics3D.LightColors inColors, float inRange, SexyVector3 inAttenuation)
		{
			this.SetAsCurrentContext();
			throw new NotSupportedException();
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00012668 File Offset: 0x00010868
		public void SetDirectionalLight(int inLightIndex, SexyVector3 inDir, Graphics3D.LightColors inColors)
		{
			this.SetAsCurrentContext();
			throw new NotSupportedException();
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00012675 File Offset: 0x00010875
		public void SetGlobalAmbient(SexyColor inColor)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetGlobalAmbient(inColor);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00012689 File Offset: 0x00010889
		public void SetMaterialAmbient(SexyColor inColor)
		{
			this.SetMaterialAmbient(inColor, -1);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00012693 File Offset: 0x00010893
		public void SetMaterialAmbient(SexyColor inColor, int inVertexColorComponent)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetMaterialAmbient(inColor, inVertexColorComponent);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x000126A8 File Offset: 0x000108A8
		public void SetMaterialDiffuse(SexyColor inColor)
		{
			this.SetMaterialDiffuse(inColor, -1);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x000126B2 File Offset: 0x000108B2
		public void SetMaterialDiffuse(SexyColor inColor, int inVertexColorComponent)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetMaterialDiffuse(inColor, inVertexColorComponent);
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x000126C7 File Offset: 0x000108C7
		public void SetMaterialSpecular(SexyColor inColor, int inVertexColorComponent)
		{
			this.SetMaterialSpecular(inColor, inVertexColorComponent, 0f);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x000126D6 File Offset: 0x000108D6
		public void SetMaterialSpecular(SexyColor inColor)
		{
			this.SetMaterialSpecular(inColor, -1, 0f);
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x000126E5 File Offset: 0x000108E5
		public void SetMaterialSpecular(SexyColor inColor, int inVertexColorComponent, float inPower)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetMaterialSpecular(inColor, inVertexColorComponent, inPower);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x000126FB File Offset: 0x000108FB
		public void SetMaterialEmissive(SexyColor inColor)
		{
			this.SetMaterialEmissive(inColor, -1);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00012705 File Offset: 0x00010905
		public void SetMaterialEmissive(SexyColor inColor, int inVertexColorComponent)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetMaterialEmissive(inColor, inVertexColorComponent);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001271A File Offset: 0x0001091A
		public void SetWorldTransform(SexyMatrix4 inMatrix)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetWorldTransform(inMatrix);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001272E File Offset: 0x0001092E
		public void SetViewTransform(SexyMatrix4 inMatrix)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetViewTransform(inMatrix);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00012742 File Offset: 0x00010942
		public void SetProjectionTransform(SexyMatrix4 inMatrix)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetProjectionTransform(inMatrix);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00012756 File Offset: 0x00010956
		public void SetTextureTransform(int inTextureIndex, SexyMatrix4 inMatrix)
		{
			this.SetTextureTransform(inTextureIndex, inMatrix, 2);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00012761 File Offset: 0x00010961
		public void SetTextureTransform(int inTextureIndex, SexyMatrix4 inMatrix, int inNumDimensions)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetTextureTransform(inTextureIndex, inMatrix, inNumDimensions);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00012777 File Offset: 0x00010977
		public bool SetTexture(int inTextureIndex, Image inImage)
		{
			this.SetAsCurrentContext();
			return this.mRenderDevice.SetTexture(inTextureIndex, inImage);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001278C File Offset: 0x0001098C
		public void SetTextureWrap(int inTextureIndex, bool inWrap)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetTextureWrap(inTextureIndex, inWrap, inWrap);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x000127A2 File Offset: 0x000109A2
		public void SetTextureWrap(int inTextureIndex, bool inWrapU, bool inWrapV)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetTextureWrap(inTextureIndex, inWrapU, inWrapV);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000127B8 File Offset: 0x000109B8
		public void SetTextureLinearFilter(int inTextureIndex, bool inLinear)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetTextureLinearFilter(inTextureIndex, inLinear);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x000127CD File Offset: 0x000109CD
		public void SetTextureCoordSource(int inTextureIndex, int inUVComponent)
		{
			this.SetTextureCoordSource(inTextureIndex, inUVComponent, Graphics3D.ETexCoordGen.TEXCOORDGEN_NONE);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000127D8 File Offset: 0x000109D8
		public void SetTextureCoordSource(int inTextureIndex, int inUVComponent, Graphics3D.ETexCoordGen inTexGen)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetTextureCoordSource(inTextureIndex, inUVComponent, inTexGen);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000127EE File Offset: 0x000109EE
		public void SetTextureFactor(int inTextureFactor)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetTextureFactor(inTextureFactor);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00012802 File Offset: 0x00010A02
		public void SetViewport(int theX, int theY, int theWidth, int theHeight, float theMinZ)
		{
			this.SetViewport(theX, theY, theWidth, theHeight, theMinZ, 1f);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00012816 File Offset: 0x00010A16
		public void SetViewport(int theX, int theY, int theWidth, int theHeight)
		{
			this.SetViewport(theX, theY, theWidth, theHeight, 0f, 1f);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001282D File Offset: 0x00010A2D
		public void SetViewport(int theX, int theY, int theWidth, int theHeight, float theMinZ, float theMaxZ)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.SetViewport((int)this.mGraphics.mTransX + theX, (int)this.mGraphics.mTransY + theY, theWidth, theHeight, theMinZ, theMaxZ);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00012863 File Offset: 0x00010A63
		public RenderEffect GetEffect(RenderEffectDefinition inDefinition)
		{
			return this.mRenderDevice.GetEffect(inDefinition);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00012874 File Offset: 0x00010A74
		public void Set3DTransformState(SexyCoords3 inWorldCoords, Graphics3D.Camera inCamera)
		{
			SexyMatrix4 sexyMatrix = new SexyMatrix4();
			inWorldCoords.GetOutboundMatrix(sexyMatrix);
			this.SetWorldTransform(sexyMatrix);
			inCamera.GetViewMatrix(sexyMatrix);
			this.SetViewTransform(sexyMatrix);
			inCamera.GetProjectionMatrix(sexyMatrix);
			this.SetProjectionTransform(sexyMatrix);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x000128B1 File Offset: 0x00010AB1
		public void SetMasking(Graphics3D.EMaskMode inMaskMode, int inAlphaRef, float inFrontDepth)
		{
			this.SetMasking(inMaskMode, inAlphaRef, inFrontDepth, 0.5f);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x000128C1 File Offset: 0x00010AC1
		public void SetMasking(Graphics3D.EMaskMode inMaskMode, int inAlphaRef)
		{
			this.SetMasking(inMaskMode, inAlphaRef, 0.25f, 0.5f);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x000128D5 File Offset: 0x00010AD5
		public void SetMasking(Graphics3D.EMaskMode inMaskMode)
		{
			this.SetMasking(inMaskMode, 0, 0.25f, 0.5f);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x000128EC File Offset: 0x00010AEC
		public void SetMasking(Graphics3D.EMaskMode inMaskMode, int inAlphaRef, float inFrontDepth, float inBackDepth)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.Flush(0U);
			switch (inMaskMode)
			{
			case Graphics3D.EMaskMode.MASKMODE_NONE:
				this.mRenderDevice.SetStencilState(Graphics3D.ECompareFunc.COMPARE_ALWAYS, 1, false, Graphics3D.ETestResultFunc.TEST_Replace, Graphics3D.ETestResultFunc.TEST_Replace);
				return;
			case Graphics3D.EMaskMode.MASKMODE_WRITE_MASKONLY:
				this.mRenderDevice.SetStencilState(Graphics3D.ECompareFunc.COMPARE_NEVER, 1, true, Graphics3D.ETestResultFunc.TEST_Replace, Graphics3D.ETestResultFunc.TEST_Replace);
				return;
			case Graphics3D.EMaskMode.MASKMODE_WRITE_MASKANDCOLOR:
				this.mRenderDevice.SetStencilState(Graphics3D.ECompareFunc.COMPARE_ALWAYS, 1, true, Graphics3D.ETestResultFunc.TEST_Replace, Graphics3D.ETestResultFunc.TEST_Replace);
				return;
			case Graphics3D.EMaskMode.MASKMODE_TEST_INSIDE:
			case Graphics3D.EMaskMode.MASKMODE_TEST_OUTSIDE:
				if (inMaskMode == Graphics3D.EMaskMode.MASKMODE_TEST_OUTSIDE)
				{
					this.mRenderDevice.SetStencilState(Graphics3D.ECompareFunc.COMPARE_NOTEQUAL, 1, true, Graphics3D.ETestResultFunc.TEST_Keep, Graphics3D.ETestResultFunc.TEST_Keep);
					return;
				}
				this.mRenderDevice.SetStencilState(Graphics3D.ECompareFunc.COMPARE_EQUAL, 1, true, Graphics3D.ETestResultFunc.TEST_Keep, Graphics3D.ETestResultFunc.TEST_Keep);
				return;
			default:
				return;
			}
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00012981 File Offset: 0x00010B81
		public void ClearMask()
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.ClearDepthBuffer();
		}

		// Token: 0x04000488 RID: 1160
		protected Graphics mGraphics;

		// Token: 0x04000489 RID: 1161
		protected RenderDevice3D mRenderDevice;

		// Token: 0x0400048A RID: 1162
		protected HRenderContext mRenderContext;

		// Token: 0x020000B3 RID: 179
		public enum EBlendMode
		{
			// Token: 0x0400048C RID: 1164
			BLEND_ZERO = 1,
			// Token: 0x0400048D RID: 1165
			BLEND_ONE,
			// Token: 0x0400048E RID: 1166
			BLEND_SRCCOLOR,
			// Token: 0x0400048F RID: 1167
			BLEND_INVSRCCOLOR,
			// Token: 0x04000490 RID: 1168
			BLEND_SRCALPHA,
			// Token: 0x04000491 RID: 1169
			BLEND_INVSRCALPHA,
			// Token: 0x04000492 RID: 1170
			BLEND_DESTCOLOR = 9,
			// Token: 0x04000493 RID: 1171
			BLEND_INVDESTCOLOR,
			// Token: 0x04000494 RID: 1172
			BLEND_SRCALPHASAT,
			// Token: 0x04000495 RID: 1173
			BLEND_DEFAULT = 65535
		}

		// Token: 0x020000B4 RID: 180
		public enum ECompareFunc
		{
			// Token: 0x04000497 RID: 1175
			COMPARE_NEVER = 1,
			// Token: 0x04000498 RID: 1176
			COMPARE_LESS,
			// Token: 0x04000499 RID: 1177
			COMPARE_EQUAL,
			// Token: 0x0400049A RID: 1178
			COMPARE_LESSEQUAL,
			// Token: 0x0400049B RID: 1179
			COMPARE_GREATER,
			// Token: 0x0400049C RID: 1180
			COMPARE_NOTEQUAL,
			// Token: 0x0400049D RID: 1181
			COMPARE_GREATEREQUAL,
			// Token: 0x0400049E RID: 1182
			COMPARE_ALWAYS
		}

		// Token: 0x020000B5 RID: 181
		public enum ETestResultFunc
		{
			// Token: 0x040004A0 RID: 1184
			TEST_Keep,
			// Token: 0x040004A1 RID: 1185
			TEST_Zero,
			// Token: 0x040004A2 RID: 1186
			TEST_Replace,
			// Token: 0x040004A3 RID: 1187
			TEST_Increment,
			// Token: 0x040004A4 RID: 1188
			TEST_Decrement,
			// Token: 0x040004A5 RID: 1189
			TEST_IncrementSaturation,
			// Token: 0x040004A6 RID: 1190
			TEST_DecrementSaturation,
			// Token: 0x040004A7 RID: 1191
			TEST_Invert
		}

		// Token: 0x020000B6 RID: 182
		public enum ETexCoordGen
		{
			// Token: 0x040004A9 RID: 1193
			TEXCOORDGEN_NONE,
			// Token: 0x040004AA RID: 1194
			TEXCOORDGEN_CAMERASPACENORMAL,
			// Token: 0x040004AB RID: 1195
			TEXCOORDGEN_CAMERASPACEPOSITION,
			// Token: 0x040004AC RID: 1196
			TEXCOORDGEN_CAMERASPACEREFLECTIONVECTOR
		}

		// Token: 0x020000B7 RID: 183
		public enum EPrimitiveType
		{
			// Token: 0x040004AE RID: 1198
			PT_PointList = 1,
			// Token: 0x040004AF RID: 1199
			PT_LineList,
			// Token: 0x040004B0 RID: 1200
			PT_LineStrip,
			// Token: 0x040004B1 RID: 1201
			PT_TriangleList,
			// Token: 0x040004B2 RID: 1202
			PT_TriangleStrip,
			// Token: 0x040004B3 RID: 1203
			PT_TriangleFan
		}

		// Token: 0x020000B8 RID: 184
		public enum EDrawPrimitiveFlags
		{
			// Token: 0x040004B5 RID: 1205
			DPF_NoAdjustUVs = 1,
			// Token: 0x040004B6 RID: 1206
			DPF_NoHalfPixelOffset,
			// Token: 0x040004B7 RID: 1207
			DPF_DiscardVerts = 4
		}

		// Token: 0x020000B9 RID: 185
		public enum EMaskMode
		{
			// Token: 0x040004B9 RID: 1209
			MASKMODE_NONE,
			// Token: 0x040004BA RID: 1210
			MASKMODE_WRITE_MASKONLY,
			// Token: 0x040004BB RID: 1211
			MASKMODE_WRITE_MASKANDCOLOR,
			// Token: 0x040004BC RID: 1212
			MASKMODE_TEST_INSIDE,
			// Token: 0x040004BD RID: 1213
			MASKMODE_TEST_OUTSIDE
		}

		// Token: 0x020000BA RID: 186
		public class LightColors
		{
			// Token: 0x060005D6 RID: 1494 RVA: 0x00012994 File Offset: 0x00010B94
			public LightColors()
			{
				this.mDiffuse = new SexyColor(SexyColor.White);
				this.mSpecular = new SexyColor(SexyColor.Black);
				this.mAmbient = new SexyColor(SexyColor.Black);
				this.mAutoScale = 1f;
			}

			// Token: 0x040004BE RID: 1214
			public SexyColor mDiffuse = default(SexyColor);

			// Token: 0x040004BF RID: 1215
			public SexyColor mSpecular = default(SexyColor);

			// Token: 0x040004C0 RID: 1216
			public SexyColor mAmbient = default(SexyColor);

			// Token: 0x040004C1 RID: 1217
			public float mAutoScale;
		}

		// Token: 0x020000BB RID: 187
		public abstract class Camera
		{
			// Token: 0x060005D7 RID: 1495 RVA: 0x00012A06 File Offset: 0x00010C06
			public Camera()
			{
				this.mZNear = 1f;
				this.mZFar = 10000f;
			}

			// Token: 0x060005D8 RID: 1496 RVA: 0x00012A24 File Offset: 0x00010C24
			public float GetZNear()
			{
				return this.mZNear;
			}

			// Token: 0x060005D9 RID: 1497 RVA: 0x00012A2C File Offset: 0x00010C2C
			public float GetZFar()
			{
				return this.mZFar;
			}

			// Token: 0x060005DA RID: 1498 RVA: 0x00012A34 File Offset: 0x00010C34
			public void GetViewMatrix(SexyMatrix4 outM)
			{
			}

			// Token: 0x060005DB RID: 1499
			public abstract void GetProjectionMatrix(SexyMatrix4 outM);

			// Token: 0x060005DC RID: 1500
			public abstract bool IsOrtho();

			// Token: 0x060005DD RID: 1501
			public abstract bool IsPerspective();

			// Token: 0x040004C2 RID: 1218
			protected float mZNear;

			// Token: 0x040004C3 RID: 1219
			protected float mZFar;
		}

		// Token: 0x020000BC RID: 188
		public class PerspectiveCamera : Graphics3D.Camera
		{
			// Token: 0x060005DE RID: 1502 RVA: 0x00012A38 File Offset: 0x00010C38
			public PerspectiveCamera()
			{
				this.mProjS = new SexyVector3(0f, 0f, 0f);
				this.mProjT = 0f;
			}

			// Token: 0x060005DF RID: 1503 RVA: 0x00012A71 File Offset: 0x00010C71
			public PerspectiveCamera(float inFovDegrees, float inAspectRatio, float inZNear)
				: this(inFovDegrees, inAspectRatio, inZNear, 10000f)
			{
			}

			// Token: 0x060005E0 RID: 1504 RVA: 0x00012A81 File Offset: 0x00010C81
			public PerspectiveCamera(float inFovDegrees, float inAspectRatio)
				: this(inFovDegrees, inAspectRatio, 1f, 10000f)
			{
			}

			// Token: 0x060005E1 RID: 1505 RVA: 0x00012A95 File Offset: 0x00010C95
			public PerspectiveCamera(float inFovDegrees, float inAspectRatio, float inZNear, float inZFar)
			{
				this.Init(inFovDegrees, inAspectRatio, inZNear, inZFar);
			}

			// Token: 0x060005E2 RID: 1506 RVA: 0x00012AB4 File Offset: 0x00010CB4
			public void Init(float inFovDegrees, float inAspectRatio, float inZNear)
			{
				this.Init(inFovDegrees, inAspectRatio, inZNear, 10000f);
			}

			// Token: 0x060005E3 RID: 1507 RVA: 0x00012AC4 File Offset: 0x00010CC4
			public void Init(float inFovDegrees, float inAspectRatio)
			{
				this.Init(inFovDegrees, inAspectRatio, 1f, 10000f);
			}

			// Token: 0x060005E4 RID: 1508 RVA: 0x00012AD8 File Offset: 0x00010CD8
			public void Init(float inFovDegrees, float inAspectRatio, float inZNear, float inZFar)
			{
				float num = SexyMath.DegToRad(inFovDegrees * 0.5f);
				float num2 = num / inAspectRatio;
				this.mProjS.y = (float)(Math.Cos((double)num2) / Math.Sin((double)num2));
				this.mProjS.x = this.mProjS.y / inAspectRatio;
				this.mProjS.z = inZFar / (inZFar - inZNear);
				this.mProjT = -this.mProjS.z * inZNear;
				this.mZNear = inZNear;
				this.mZFar = inZFar;
			}

			// Token: 0x060005E5 RID: 1509 RVA: 0x00012B5E File Offset: 0x00010D5E
			public override void GetProjectionMatrix(SexyMatrix4 outM)
			{
			}

			// Token: 0x060005E6 RID: 1510 RVA: 0x00012B62 File Offset: 0x00010D62
			public override bool IsOrtho()
			{
				return false;
			}

			// Token: 0x060005E7 RID: 1511 RVA: 0x00012B65 File Offset: 0x00010D65
			public override bool IsPerspective()
			{
				return true;
			}

			// Token: 0x060005E8 RID: 1512 RVA: 0x00012B68 File Offset: 0x00010D68
			public SexyVector3 EyeToScreen(SexyVector3 inEyePos)
			{
				SexyVector3 result = default(SexyVector3);
				float num = -inEyePos.z;
				result.x = inEyePos.x * this.mProjS.x / num;
				result.y = inEyePos.y * this.mProjS.y / num;
				result.z = (num * this.mProjS.z + this.mProjT) / this.mZFar;
				result.x = result.x * 0.5f + 0.5f;
				result.y = result.y * -0.5f + 0.5f;
				return result;
			}

			// Token: 0x060005E9 RID: 1513 RVA: 0x00012C18 File Offset: 0x00010E18
			public SexyVector3 ScreenToEye(SexyVector3 inScreenPos)
			{
				float num = (inScreenPos.x - 0.5f) * 2f;
				float num2 = (inScreenPos.y - 0.5f) * -2f;
				SexyVector3 sexyVector = new SexyVector3(num * this.mZNear / this.mProjS.x, num2 * this.mZNear / this.mProjS.y, -this.mZNear);
				SexyVector3 impliedObject = new SexyVector3(num * this.mZFar / this.mProjS.x, num2 * this.mZFar / this.mProjS.y, -this.mZFar);
				return sexyVector + (impliedObject - sexyVector) * inScreenPos.z;
			}

			// Token: 0x040004C4 RID: 1220
			protected SexyVector3 mProjS = default(SexyVector3);

			// Token: 0x040004C5 RID: 1221
			protected float mProjT;
		}

		// Token: 0x020000BD RID: 189
		public class OffCenterPerspectiveCamera : Graphics3D.Camera
		{
			// Token: 0x060005EA RID: 1514 RVA: 0x00012CD2 File Offset: 0x00010ED2
			public OffCenterPerspectiveCamera()
			{
				this.mProjS = new SexyVector3(0f, 0f, 0f);
				this.mProjT = 0f;
			}

			// Token: 0x060005EB RID: 1515 RVA: 0x00012D0B File Offset: 0x00010F0B
			public OffCenterPerspectiveCamera(float inFovDegrees, float inAspectRatio, float inOffsetX, float inOffsetY, float inZNear)
				: this(inFovDegrees, inAspectRatio, inOffsetX, inOffsetY, inZNear, 10000f)
			{
			}

			// Token: 0x060005EC RID: 1516 RVA: 0x00012D1F File Offset: 0x00010F1F
			public OffCenterPerspectiveCamera(float inFovDegrees, float inAspectRatio, float inOffsetX, float inOffsetY)
				: this(inFovDegrees, inAspectRatio, inOffsetX, inOffsetY, 1f, 10000f)
			{
			}

			// Token: 0x060005ED RID: 1517 RVA: 0x00012D36 File Offset: 0x00010F36
			public OffCenterPerspectiveCamera(float inFovDegrees, float inAspectRatio, float inOffsetX, float inOffsetY, float inZNear, float inZFar)
			{
				this.Init(inFovDegrees, inAspectRatio, inOffsetX, inOffsetY, inZNear, inZFar);
			}

			// Token: 0x060005EE RID: 1518 RVA: 0x00012D59 File Offset: 0x00010F59
			public void Init(float inFovDegrees, float inAspectRatio, float inOffsetX, float inOffsetY, float inZNear)
			{
				this.Init(inFovDegrees, inAspectRatio, inOffsetX, inOffsetY, inZNear, 10000f);
			}

			// Token: 0x060005EF RID: 1519 RVA: 0x00012D6D File Offset: 0x00010F6D
			public void Init(float inFovDegrees, float inAspectRatio, float inOffsetX, float inOffsetY)
			{
				this.Init(inFovDegrees, inAspectRatio, inOffsetX, inOffsetY, 1f, 10000f);
			}

			// Token: 0x060005F0 RID: 1520 RVA: 0x00012D84 File Offset: 0x00010F84
			public void Init(float inFovDegrees, float inAspectRatio, float inOffsetX, float inOffsetY, float inZNear, float inZFar)
			{
				float num = SexyMath.DegToRad(inFovDegrees * 0.5f);
				float num2 = num / inAspectRatio;
				float num3 = (float)(Math.Cos((double)num2) / Math.Sin((double)num2));
				float num4 = num3 / inAspectRatio;
				float num5 = inZNear / num4;
				float num6 = inZNear / num3;
				this.mLeft = inOffsetX - num5;
				this.mRight = inOffsetX + num5;
				this.mTop = inOffsetY + num6;
				this.mBottom = inOffsetY - num6;
				this.mProjS.y = num3;
				this.mProjS.x = num4;
				this.mProjS.z = inZFar / (inZFar - inZNear);
				this.mProjT = -this.mProjS.z * inZNear;
				this.mZNear = inZNear;
				this.mZFar = inZFar;
			}

			// Token: 0x060005F1 RID: 1521 RVA: 0x00012E3D File Offset: 0x0001103D
			public override void GetProjectionMatrix(SexyMatrix4 outM)
			{
			}

			// Token: 0x060005F2 RID: 1522 RVA: 0x00012E41 File Offset: 0x00011041
			public override bool IsOrtho()
			{
				return false;
			}

			// Token: 0x060005F3 RID: 1523 RVA: 0x00012E44 File Offset: 0x00011044
			public override bool IsPerspective()
			{
				return true;
			}

			// Token: 0x060005F4 RID: 1524 RVA: 0x00012E48 File Offset: 0x00011048
			public SexyVector3 EyeToScreen(SexyVector3 inEyePos)
			{
				SexyVector3 result = default(SexyVector3);
				float num = -inEyePos.z;
				result.x = inEyePos.x * this.mProjS.x / num;
				result.y = inEyePos.y * this.mProjS.y / num;
				result.z = (num * this.mProjS.z + this.mProjT) / this.mZFar;
				result.x = result.x * 0.5f + 0.5f;
				result.y = result.y * -0.5f + 0.5f;
				return result;
			}

			// Token: 0x060005F5 RID: 1525 RVA: 0x00012EF8 File Offset: 0x000110F8
			public SexyVector3 ScreenToEye(SexyVector3 inScreenPos)
			{
				float num = (inScreenPos.x - 0.5f) * 2f;
				float num2 = (inScreenPos.y - 0.5f) * -2f;
				SexyVector3 sexyVector = new SexyVector3(num * this.mZNear / this.mProjS.x, num2 * this.mZNear / this.mProjS.y, -this.mZNear);
				SexyVector3 impliedObject = new SexyVector3(num * this.mZFar / this.mProjS.x, num2 * this.mZFar / this.mProjS.y, -this.mZFar);
				return sexyVector + (impliedObject - sexyVector) * inScreenPos.z;
			}

			// Token: 0x040004C6 RID: 1222
			protected SexyVector3 mProjS = default(SexyVector3);

			// Token: 0x040004C7 RID: 1223
			protected float mProjT;

			// Token: 0x040004C8 RID: 1224
			protected float mLeft;

			// Token: 0x040004C9 RID: 1225
			protected float mRight;

			// Token: 0x040004CA RID: 1226
			protected float mTop;

			// Token: 0x040004CB RID: 1227
			protected float mBottom;
		}

		// Token: 0x020000BE RID: 190
		public class OrthoCamera : Graphics3D.Camera
		{
			// Token: 0x060005F6 RID: 1526 RVA: 0x00012FB4 File Offset: 0x000111B4
			public OrthoCamera()
			{
				this.mProjS = new SexyVector3(0f, 0f, 0f);
				this.mProjT = 0f;
				this.mWidth = 0f;
				this.mHeight = 0f;
			}

			// Token: 0x060005F7 RID: 1527 RVA: 0x0001300E File Offset: 0x0001120E
			public OrthoCamera(float inWidth, float inHeight, float inZNear)
				: this(inWidth, inHeight, inZNear, 10000f)
			{
			}

			// Token: 0x060005F8 RID: 1528 RVA: 0x0001301E File Offset: 0x0001121E
			public OrthoCamera(float inWidth, float inHeight)
				: this(inWidth, inHeight, 1f, 10000f)
			{
			}

			// Token: 0x060005F9 RID: 1529 RVA: 0x00013032 File Offset: 0x00011232
			public OrthoCamera(float inWidth, float inHeight, float inZNear, float inZFar)
			{
				this.Init(inWidth, inHeight, inZNear, inZFar);
			}

			// Token: 0x060005FA RID: 1530 RVA: 0x00013051 File Offset: 0x00011251
			public void Init(float inWidth, float inHeight, float inZNear)
			{
				this.Init(inWidth, inHeight, inZNear, 10000f);
			}

			// Token: 0x060005FB RID: 1531 RVA: 0x00013061 File Offset: 0x00011261
			public void Init(float inWidth, float inHeight)
			{
				this.Init(inWidth, inHeight, 1f, 10000f);
			}

			// Token: 0x060005FC RID: 1532 RVA: 0x00013078 File Offset: 0x00011278
			public void Init(float inWidth, float inHeight, float inZNear, float inZFar)
			{
				this.mWidth = inWidth;
				this.mHeight = inHeight;
				this.mProjS.y = 2f / this.mHeight;
				this.mProjS.x = 2f / this.mWidth;
				this.mProjS.z = 1f / (inZFar - inZNear);
				this.mProjT = -this.mProjS.z * inZNear;
				this.mZNear = inZNear;
				this.mZFar = inZFar;
			}

			// Token: 0x060005FD RID: 1533 RVA: 0x000130F9 File Offset: 0x000112F9
			public override void GetProjectionMatrix(SexyMatrix4 outM)
			{
			}

			// Token: 0x060005FE RID: 1534 RVA: 0x000130FD File Offset: 0x000112FD
			public override bool IsOrtho()
			{
				return true;
			}

			// Token: 0x060005FF RID: 1535 RVA: 0x00013100 File Offset: 0x00011300
			public override bool IsPerspective()
			{
				return false;
			}

			// Token: 0x06000600 RID: 1536 RVA: 0x00013104 File Offset: 0x00011304
			public SexyVector3 EyeToScreen(SexyVector3 inEyePos)
			{
				SexyVector3 result = default(SexyVector3);
				result.x = inEyePos.x * this.mProjS.x;
				result.y = inEyePos.y * this.mProjS.y;
				result.z = (this.mProjS.z + this.mProjT) / this.mZFar;
				result.x = result.x * 0.5f + 0.5f;
				result.y = result.y * -0.5f + 0.5f;
				return result;
			}

			// Token: 0x06000601 RID: 1537 RVA: 0x000131A4 File Offset: 0x000113A4
			public SexyVector3 ScreenToEye(SexyVector3 inScreenPos)
			{
				float num = (inScreenPos.x - 0.5f) * 2f;
				float num2 = (inScreenPos.y - 0.5f) * -2f;
				SexyVector3 sexyVector = new SexyVector3(num / this.mProjS.x, num2 / this.mProjS.y, -this.mZNear);
				SexyVector3 impliedObject = new SexyVector3(sexyVector.x, sexyVector.y, -this.mZFar);
				return sexyVector + (impliedObject - sexyVector) * inScreenPos.z;
			}

			// Token: 0x040004CC RID: 1228
			protected SexyVector3 mProjS = default(SexyVector3);

			// Token: 0x040004CD RID: 1229
			protected float mProjT;

			// Token: 0x040004CE RID: 1230
			protected float mWidth;

			// Token: 0x040004CF RID: 1231
			protected float mHeight;
		}

		// Token: 0x020000BF RID: 191
		public interface Spline
		{
			// Token: 0x06000602 RID: 1538
			SexyVector3 Evaluate(float inTime);
		}

		// Token: 0x020000C0 RID: 192
		public class CatmullRomSpline : Graphics3D.Spline
		{
			// Token: 0x06000603 RID: 1539 RVA: 0x00013236 File Offset: 0x00011436
			public CatmullRomSpline()
			{
			}

			// Token: 0x06000604 RID: 1540 RVA: 0x00013249 File Offset: 0x00011449
			public CatmullRomSpline(Graphics3D.CatmullRomSpline inSpline)
			{
				this.mPoints = inSpline.mPoints;
			}

			// Token: 0x06000605 RID: 1541 RVA: 0x00013268 File Offset: 0x00011468
			public CatmullRomSpline(List<SexyVector3> inPoints)
			{
				this.mPoints = inPoints;
			}

			// Token: 0x06000606 RID: 1542 RVA: 0x00013284 File Offset: 0x00011484
			public SexyVector3 Evaluate(float inTime)
			{
				return default(SexyVector3);
			}

			// Token: 0x040004D0 RID: 1232
			public List<SexyVector3> mPoints = new List<SexyVector3>();
		}
	}
}
