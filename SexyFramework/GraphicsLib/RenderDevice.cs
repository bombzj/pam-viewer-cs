using System;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x02000035 RID: 53
	public abstract class RenderDevice
	{
		// Token: 0x06000212 RID: 530
		public abstract RenderDevice3D Get3D();

		// Token: 0x06000213 RID: 531
		public abstract bool CanFillPoly();

		// Token: 0x06000214 RID: 532 RVA: 0x00007BD5 File Offset: 0x00005DD5
		public HRenderContext CreateContext(Image theDestImage)
		{
			return this.CreateContext(theDestImage, null);
		}

		// Token: 0x06000215 RID: 533
		public abstract HRenderContext CreateContext(Image theDestImage, HRenderContext theSourceContext);

		// Token: 0x06000216 RID: 534
		public abstract void DeleteContext(HRenderContext theContext);

		// Token: 0x06000217 RID: 535
		public abstract void SetCurrentContext(HRenderContext theContext);

		// Token: 0x06000218 RID: 536
		public abstract HRenderContext GetCurrentContext();

		// Token: 0x06000219 RID: 537
		public abstract void PushState();

		// Token: 0x0600021A RID: 538
		public abstract void PopState();

		// Token: 0x0600021B RID: 539
		public abstract void ClearRect(Rect theRect);

		// Token: 0x0600021C RID: 540
		public abstract void FillRect(Rect theRect, SexyColor theColor, int theDrawMode);

		// Token: 0x0600021D RID: 541
		public abstract void FillScanLinesWithCoverage(RenderDevice.Span theSpans, int theSpanCount, SexyColor theColor, int theDrawMode, string theCoverage, int theCoverX, int theCoverY, int theCoverWidth, int theCoverHeight);

		// Token: 0x0600021E RID: 542 RVA: 0x00007BDF File Offset: 0x00005DDF
		public virtual void FillPoly(SexyPoint[] theVertices, int theNumVertices, Rect theClipRect, SexyColor theColor, int theDrawMode, int tx, int ty)
		{
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00007BE1 File Offset: 0x00005DE1
		public void DrawLine(double theStartX, double theStartY, double theEndX, double theEndY, SexyColor theColor, int theDrawMode)
		{
			this.DrawLine(theStartX, theStartY, theEndX, theEndY, theColor, theDrawMode, false);
		}

		// Token: 0x06000220 RID: 544
		public abstract void DrawLine(double theStartX, double theStartY, double theEndX, double theEndY, SexyColor theColor, int theDrawMode, bool antiAlias);

		// Token: 0x06000221 RID: 545
		public abstract void Blt(Image theImage, int theX, int theY, Rect theSrcRect, SexyColor theColor, int theDrawMode);

		// Token: 0x06000222 RID: 546
		public abstract void BltF(Image theImage, float theX, float theY, Rect theSrcRect, Rect theClipRect, SexyColor theColor, int theDrawMode);

		// Token: 0x06000223 RID: 547
		public abstract void BltRotated(Image theImage, float theX, float theY, Rect theSrcRect, Rect theClipRect, SexyColor theColor, int theDrawMode, double theRot, float theRotCenterX, float theRotCenterY);

		// Token: 0x06000224 RID: 548
		public abstract void BltMatrix(Image theImage, float x, float y, SexyMatrix3 theMatrix, Rect theClipRect, SexyColor theColor, int theDrawMode, Rect theSrcRect, bool blend);

		// Token: 0x06000225 RID: 549
		public abstract void DrawSprite(Image theImage, SexyColor theColor, int theDrawMode, SexyTransform2D theTransform, Rect theSrcRect, bool center);

		// Token: 0x06000226 RID: 550
		public abstract void BeginSprite();

		// Token: 0x06000227 RID: 551
		public abstract void EndSprite();

		// Token: 0x06000228 RID: 552 RVA: 0x00007BF4 File Offset: 0x00005DF4
		public void BltTriangles(Image theImage, SexyVertex2D[,] theVertices, int theNumTriangles, SexyColor theColor, int theDrawMode, float tx, float ty, bool blend)
		{
			this.BltTriangles(theImage, theVertices, theNumTriangles, theColor, theDrawMode, tx, ty, blend, Rect.INVALIDATE_RECT);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00007C1C File Offset: 0x00005E1C
		public void BltTriangles(Image theImage, SexyVertex2D[,] theVertices, int theNumTriangles, SexyColor theColor, int theDrawMode, float tx, float ty)
		{
			this.BltTriangles(theImage, theVertices, theNumTriangles, theColor, theDrawMode, tx, ty, true, Rect.INVALIDATE_RECT);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00007C40 File Offset: 0x00005E40
		public void BltTriangles(Image theImage, SexyVertex2D[,] theVertices, int theNumTriangles, SexyColor theColor, int theDrawMode, float tx)
		{
			this.BltTriangles(theImage, theVertices, theNumTriangles, theColor, theDrawMode, tx, 0f, true, Rect.INVALIDATE_RECT);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00007C68 File Offset: 0x00005E68
		public void BltTriangles(Image theImage, SexyVertex2D[,] theVertices, int theNumTriangles, SexyColor theColor, int theDrawMode)
		{
			this.BltTriangles(theImage, theVertices, theNumTriangles, theColor, theDrawMode, 0f, 0f, true, Rect.INVALIDATE_RECT);
		}

		// Token: 0x0600022C RID: 556
		public abstract void BltTriangles(Image theImage, SexyVertex2D[,] theVertices, int theNumTriangles, SexyColor theColor, int theDrawMode, float tx, float ty, bool blend, Rect theClipRect);

		// Token: 0x0600022D RID: 557
		public abstract void BltMirror(Image theImage, int theX, int theY, Rect theSrcRect, SexyColor theColor, int theDrawMode);

		// Token: 0x0600022E RID: 558 RVA: 0x00007C94 File Offset: 0x00005E94
		public void BltStretched(Image theImage, Rect theDestRect, Rect theSrcRect, Rect theClipRect, SexyColor theColor, int theDrawMode, bool fastStretch)
		{
			this.BltStretched(theImage, theDestRect, theSrcRect, theClipRect, theColor, theDrawMode, fastStretch, false);
		}

		// Token: 0x0600022F RID: 559
		public abstract void BltStretched(Image theImage, Rect theDestRect, Rect theSrcRect, Rect theClipRect, SexyColor theColor, int theDrawMode, bool fastStretch, bool mirror);

		// Token: 0x06000230 RID: 560 RVA: 0x00007CB4 File Offset: 0x00005EB4
		public virtual void DrawRect(Rect theRect, SexyColor theColor, int theDrawMode)
		{
			this.FillRect(new Rect(theRect.mX, theRect.mY, theRect.mWidth + 1, 1), theColor, theDrawMode);
			this.FillRect(new Rect(theRect.mX, theRect.mY + theRect.mHeight, theRect.mWidth + 1, 1), theColor, theDrawMode);
			this.FillRect(new Rect(theRect.mX, theRect.mY + 1, 1, theRect.mHeight - 1), theColor, theDrawMode);
			this.FillRect(new Rect(theRect.mX + theRect.mWidth, theRect.mY + 1, 1, theRect.mHeight - 1), theColor, theDrawMode);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00007D6C File Offset: 0x00005F6C
		public virtual void FillScanLines(RenderDevice.Span[] theSpans, int theSpanCount, SexyColor theColor, int theDrawMode)
		{
			for (int i = 0; i < theSpanCount; i++)
			{
				RenderDevice.Span span = theSpans[i];
				this.FillRect(new Rect(span.mX, span.mY, span.mWidth, 1), theColor, theDrawMode);
			}
		}

		// Token: 0x02000036 RID: 54
		public class Span
		{
			// Token: 0x04000131 RID: 305
			public int mY;

			// Token: 0x04000132 RID: 306
			public int mX;

			// Token: 0x04000133 RID: 307
			public int mWidth;
		}
	}
}
