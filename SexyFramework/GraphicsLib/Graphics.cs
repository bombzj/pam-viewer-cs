using System;
using System.Collections.Generic;
using System.Text;
using SexyFramework.Drivers.Graphics;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000B0 RID: 176
	public class Graphics : GraphicsState
	{
		// Token: 0x06000516 RID: 1302 RVA: 0x0000ECC9 File Offset: 0x0000CEC9
		protected static bool PFCompareInd(IntPtr u, IntPtr v)
		{
			return false;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0000ECCC File Offset: 0x0000CECC
		protected static bool PFCompareActive(IntPtr u, IntPtr v)
		{
			return false;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0000ECCF File Offset: 0x0000CECF
		protected void PFDelete(int i)
		{
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0000ECD1 File Offset: 0x0000CED1
		protected void PFInsert(int i, int y)
		{
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0000ECD4 File Offset: 0x0000CED4
		protected void DrawImageTransformHelper(Image theImage, Transform theTransform, Rect theSrcRect, float x, float y, bool useFloat)
		{
			if (theTransform.mComplex || (this.Get3D() != null && useFloat))
			{
				this.DrawImageMatrix(theImage, theTransform.GetMatrix(), theSrcRect, x, y);
				return;
			}
			float num = (float)theSrcRect.mWidth / 2f;
			float num2 = (float)theSrcRect.mHeight / 2f;
			if (theTransform.mHaveRot)
			{
				float num3 = num - theTransform.mTransX1;
				float num4 = num2 - theTransform.mTransY1;
				x = x + theTransform.mTransX2 - num3 + 0.5f;
				y = y + theTransform.mTransY2 - num4 + 0.5f;
				if (useFloat)
				{
					this.DrawImageRotatedF(theImage, x, y, (double)theTransform.mRot, num3, num4, theSrcRect);
					return;
				}
				this.DrawImageRotated(theImage, (int)x, (int)y, (double)theTransform.mRot, (int)num3, (int)num4, theSrcRect);
				return;
			}
			else
			{
				if (theTransform.mHaveScale)
				{
					bool mirror = false;
					if (theTransform.mScaleX == -1f)
					{
						if (theTransform.mScaleY == 1f)
						{
							x = x + theTransform.mTransX1 + theTransform.mTransX2 - num + 0.5f;
							y = y + theTransform.mTransY1 + theTransform.mTransY2 - num2 + 0.5f;
							this.DrawImageMirror(theImage, (int)x, (int)y, theSrcRect);
							return;
						}
						mirror = true;
					}
					float num5 = num * theTransform.mScaleX;
					float num6 = num2 * theTransform.mScaleY;
					x = x + theTransform.mTransX2 - num5;
					y = y + theTransform.mTransY2 - num6;
					this.mDestRect.mX = (int)x;
					this.mDestRect.mY = (int)y;
					this.mDestRect.mWidth = (int)(num5 * 2f);
					this.mDestRect.mHeight = (int)(num6 * 2f);
					this.DrawImageMirror(theImage, this.mDestRect, theSrcRect, mirror);
					return;
				}
				x = x + theTransform.mTransX1 + theTransform.mTransX2 - num + 0.5f;
				y = y + theTransform.mTransY1 + theTransform.mTransY2 - num2 + 0.5f;
				if (useFloat)
				{
					this.DrawImageF(theImage, x, y, theSrcRect);
					return;
				}
				this.DrawImage(theImage, (int)x, (int)y, theSrcRect);
				return;
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0000EEE8 File Offset: 0x0000D0E8
		protected void InitRenderInfo(Graphics theSourceGraphics)
		{
			this.mGraphics3D = null;
			this.mIs3D = false;
			RenderDevice3D renderDevice3D = GlobalMembers.gSexyAppBase.mGraphicsDriver.GetRenderDevice3D();
			if (renderDevice3D != null)
			{
				HRenderContext hrenderContext;
				if (theSourceGraphics != null)
				{
					hrenderContext = renderDevice3D.CreateContext(this.mDestImage, theSourceGraphics.mRenderContext);
				}
				else
				{
					hrenderContext = renderDevice3D.CreateContext(this.mDestImage);
				}
				this.mRenderDevice = renderDevice3D;
				this.mRenderContext = hrenderContext;
				this.mGraphics3D = new Graphics3D(this, renderDevice3D, this.mRenderContext);
				this.mIs3D = true;
				return;
			}
			if (!this.mRenderContext.IsValid())
			{
				RenderDevice renderDevice = GlobalMembers.gSexyAppBase.mGraphicsDriver.GetRenderDevice();
				if (renderDevice != null)
				{
					HRenderContext hrenderContext2 = new HRenderContext();
					if (theSourceGraphics != null)
					{
						hrenderContext2 = renderDevice.CreateContext(this.mDestImage, theSourceGraphics.mRenderContext);
					}
					else
					{
						hrenderContext2 = renderDevice.CreateContext(this.mDestImage);
					}
					if (hrenderContext2.IsValid())
					{
						this.mRenderDevice = renderDevice;
						this.mRenderContext = hrenderContext2;
						this.mGraphics3D = null;
						this.mIs3D = false;
					}
				}
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0000EFD5 File Offset: 0x0000D1D5
		protected void SetAsCurrentContext()
		{
			if (this.mRenderDevice != null)
			{
				this.mRenderDevice.SetCurrentContext(this.mRenderContext);
			}
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0000EFF0 File Offset: 0x0000D1F0
		protected void CalcFinalColor()
		{
			if (this.mPushedColorVector.Count > 0)
			{
				SexyColor color = this.mPushedColorVector[this.mPushedColorVector.Count - 1];
				this.mFinalColor = new SexyColor(Math.Min(255, color.mRed * this.mColor.mRed / 255), Math.Min(255, color.mGreen * this.mColor.mGreen / 255), Math.Min(255, color.mBlue * this.mColor.mBlue / 255), Math.Min(255, color.mAlpha * this.mColor.mAlpha / 255));
				return;
			}
			this.mFinalColor = this.mColor;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0000F0CC File Offset: 0x0000D2CC
		protected SexyColor GetImageColor()
		{
			if (this.mPushedColorVector.Count > 0)
			{
				if (this.mColorizeImages)
				{
					return this.mFinalColor;
				}
				return this.mPushedColorVector[this.mPushedColorVector.Count - 1];
			}
			else
			{
				if (this.mColorizeImages)
				{
					return this.mColor;
				}
				return SexyColor.White;
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0000F124 File Offset: 0x0000D324
		protected bool DrawLineClipHelper(ref double theStartX, ref double theStartY, ref double theEndX, ref double theEndY)
		{
			double num = theStartX;
			double num2 = theStartY;
			double num3 = theEndX;
			double num4 = theEndY;
			if (num > num3)
			{
				this.Swap<double>(ref num, ref num3);
				this.Swap<double>(ref num2, ref num4);
			}
			if (num < (double)this.mClipRect.mX)
			{
				if (num3 < (double)this.mClipRect.mX)
				{
					return false;
				}
				double num5 = (num4 - num2) / (num3 - num);
				num2 += ((double)this.mClipRect.mX - num) * num5;
				num = (double)this.mClipRect.mX;
			}
			if (num3 >= (double)(this.mClipRect.mX + this.mClipRect.mWidth))
			{
				if (num >= (double)(this.mClipRect.mX + this.mClipRect.mWidth))
				{
					return false;
				}
				double num6 = (num4 - num2) / (num3 - num);
				num4 += ((double)(this.mClipRect.mX + this.mClipRect.mWidth - 1) - num3) * num6;
				num3 = (double)(this.mClipRect.mX + this.mClipRect.mWidth - 1);
			}
			if (num2 > num4)
			{
				this.Swap<double>(ref num, ref num3);
				this.Swap<double>(ref num2, ref num4);
			}
			if (num2 < (double)this.mClipRect.mY)
			{
				if (num4 < (double)this.mClipRect.mY)
				{
					return false;
				}
				double num7 = (num3 - num) / (num4 - num2);
				num += ((double)this.mClipRect.mY - num2) * num7;
				num2 = (double)this.mClipRect.mY;
			}
			if (num4 >= (double)(this.mClipRect.mY + this.mClipRect.mHeight))
			{
				if (num2 >= (double)(this.mClipRect.mY + this.mClipRect.mHeight))
				{
					return false;
				}
				double num8 = (num3 - num) / (num4 - num2);
				num3 += ((double)(this.mClipRect.mY + this.mClipRect.mHeight - 1) - num4) * num8;
				num4 = (double)(this.mClipRect.mY + this.mClipRect.mHeight - 1);
			}
			theStartX = num;
			theStartY = num2;
			theEndX = num3;
			theEndY = num4;
			return true;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0000F30C File Offset: 0x0000D50C
		protected void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0000F334 File Offset: 0x0000D534
		public Graphics()
		{
			this.mTransX = 0f;
			this.mTransY = 0f;
			this.mScaleX = 1f;
			this.mScaleY = 1f;
			this.mScaleOrigX = 0f;
			this.mScaleOrigY = 0f;
			this.mDestImage = null;
			this.mDrawMode = 0;
			this.mColorizeImages = false;
			this.mFastStretch = false;
			this.mWriteColoredString = true;
			this.mLinearBlend = false;
			this.mClipRect = new Rect(0, 0, GlobalMembers.gSexyApp.mGraphicsDriver.GetScreenWidth(), GlobalMembers.gSexyApp.mGraphicsDriver.GetScreenHeight());
			this.InitRenderInfo(null);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0000F40C File Offset: 0x0000D60C
		public Graphics(Image theDestImage)
		{
			this.mTransX = 0f;
			this.mTransY = 0f;
			this.mScaleX = 1f;
			this.mScaleY = 1f;
			this.mScaleOrigX = 0f;
			this.mScaleOrigY = 0f;
			this.mDestImage = theDestImage;
			this.mDrawMode = 0;
			this.mColorizeImages = false;
			this.mFastStretch = false;
			this.mWriteColoredString = true;
			this.mLinearBlend = false;
			if (this.mDestImage == null)
			{
				this.mClipRect = new Rect(0, 0, GlobalMembers.gSexyApp.mGraphicsDriver.GetScreenWidth(), GlobalMembers.gSexyApp.mGraphicsDriver.GetScreenHeight());
			}
			else
			{
				this.mClipRect = new Rect(0, 0, this.mDestImage.GetWidth(), this.mDestImage.GetHeight());
			}
			this.InitRenderInfo(null);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0000F510 File Offset: 0x0000D710
		public Graphics(Graphics theGraphics)
		{
			base.CopyStateFrom(theGraphics);
			this.InitRenderInfo(theGraphics);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0000F54C File Offset: 0x0000D74C
		public virtual void Dispose()
		{
			this.mRenderDevice.DeleteContext(this.mRenderContext);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0000F560 File Offset: 0x0000D760
		public void ClearRenderContext()
		{
			XNAGraphicsDriver xnagraphicsDriver = (XNAGraphicsDriver)GlobalMembers.gSexyAppBase.mGraphicsDriver;
			xnagraphicsDriver.mXNARenderDevice.SetCurrentContext(null);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0000F589 File Offset: 0x0000D789
		public Graphics3D Get3D()
		{
			return this.mGraphics3D;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0000F591 File Offset: 0x0000D791
		public RenderDevice GetRenderDevice()
		{
			return this.mRenderDevice;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0000F599 File Offset: 0x0000D799
		public HRenderContext GetRenderContext()
		{
			return this.mRenderContext;
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0000F5A4 File Offset: 0x0000D7A4
		public void PushState()
		{
			GraphicsState graphicsState = GraphicsStatePool.CreateState();
			graphicsState.CopyStateFrom(this);
			this.mStateStack.Push(graphicsState);
			if (this.mRenderDevice != null)
			{
				this.SetAsCurrentContext();
				this.mRenderDevice.PushState();
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0000F5E4 File Offset: 0x0000D7E4
		public void PopState()
		{
			if (this.mStateStack.Count > 0)
			{
				base.CopyStateFrom(this.mStateStack.Peek());
				GraphicsStatePool.ReleaseState(this.mStateStack.Pop());
			}
			if (this.mRenderDevice != null)
			{
				this.SetAsCurrentContext();
				this.mRenderDevice.PopState();
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0000F639 File Offset: 0x0000D839
		public void SetFont(Font theFont)
		{
			this.mFont = theFont;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0000F642 File Offset: 0x0000D842
		public Font GetFont()
		{
			return this.mFont;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0000F64C File Offset: 0x0000D84C
		public void SetColor(SexyColor theColor)
		{
			this.mColor.mRed = theColor.mRed;
			this.mColor.mGreen = theColor.mGreen;
			this.mColor.mBlue = theColor.mBlue;
			this.mColor.mAlpha = theColor.mAlpha;
			this.CalcFinalColor();
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0000F6A7 File Offset: 0x0000D8A7
		public void SetColor(int red, int green, int blue, int alpha)
		{
			this.mColor.mRed = red;
			this.mColor.mGreen = green;
			this.mColor.mBlue = blue;
			this.mColor.mAlpha = alpha;
			this.CalcFinalColor();
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0000F6E0 File Offset: 0x0000D8E0
		public void SetColor(int red, int green, int blue)
		{
			this.mColor.mRed = red;
			this.mColor.mGreen = green;
			this.mColor.mBlue = blue;
			this.mColor.mAlpha = 255;
			this.CalcFinalColor();
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0000F71C File Offset: 0x0000D91C
		public SexyColor GetColor()
		{
			return this.mColor;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0000F724 File Offset: 0x0000D924
		public void PushColorMult()
		{
			this.mPushedColorVector.Add(this.mFinalColor);
			this.CalcFinalColor();
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0000F73D File Offset: 0x0000D93D
		public void PopColorMult()
		{
			this.mPushedColorVector.RemoveAt(this.mPushedColorVector.Count - 1);
			this.CalcFinalColor();
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0000F75D File Offset: 0x0000D95D
		public SexyColor GetFinalColor()
		{
			if (this.mPushedColorVector.Count > 0)
			{
				return this.mFinalColor;
			}
			return this.mColor;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0000F77A File Offset: 0x0000D97A
		public void SetDrawMode(int theDrawMode)
		{
			this.mDrawMode = theDrawMode;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0000F783 File Offset: 0x0000D983
		public int GetDrawMode()
		{
			return this.mDrawMode;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0000F78B File Offset: 0x0000D98B
		public void SetColorizeImages(bool colorizeImages)
		{
			this.mColorizeImages = colorizeImages;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0000F794 File Offset: 0x0000D994
		public bool GetColorizeImages()
		{
			return this.mColorizeImages;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0000F79C File Offset: 0x0000D99C
		public void SetFastStretch(bool fastStretch)
		{
			this.mFastStretch = fastStretch;
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0000F7A5 File Offset: 0x0000D9A5
		public bool GetFastStretch()
		{
			return this.mFastStretch;
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0000F7AD File Offset: 0x0000D9AD
		public void SetLinearBlend(bool linear)
		{
			this.mLinearBlend = linear;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0000F7B6 File Offset: 0x0000D9B6
		public bool GetLinearBlend()
		{
			return this.mLinearBlend;
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0000F7C0 File Offset: 0x0000D9C0
		public void FillRect(int theX, int theY, int theWidth, int theHeight)
		{
			SexyColor finalColor = this.GetFinalColor();
			if (finalColor.mAlpha == 0)
			{
				return;
			}
			this.SetAsCurrentContext();
			if (this.mRenderDevice != null)
			{
				this.mDestRect.mX = theX + (int)this.mTransX;
				this.mDestRect.mY = theY + (int)this.mTransY;
				this.mDestRect.mWidth = theWidth;
				this.mDestRect.mHeight = theHeight;
				Rect theRect = this.mDestRect.Intersection(this.mClipRect);
				this.mRenderDevice.FillRect(theRect, finalColor, this.mDrawMode);
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0000F852 File Offset: 0x0000DA52
		public void FillRect(Rect theRect)
		{
			this.FillRect(theRect.mX, theRect.mY, theRect.mWidth, theRect.mHeight);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0000F878 File Offset: 0x0000DA78
		public void DrawRect(int theX, int theY, int theWidth, int theHeight)
		{
			SexyColor finalColor = this.GetFinalColor();
			if (finalColor.mAlpha == 0)
			{
				return;
			}
			Rect theRect = new Rect(theX + (int)this.mTransX, theY + (int)this.mTransY, theWidth, theHeight);
			Rect rect = new Rect(theX + (int)this.mTransX, theY + (int)this.mTransY, theWidth + 1, theHeight + 1);
			Rect rect2 = rect.Intersection(this.mClipRect);
			if (rect.Equals(rect2))
			{
				this.SetAsCurrentContext();
				this.mRenderDevice.DrawRect(theRect, finalColor, this.mDrawMode);
				return;
			}
			this.FillRect(theX, theY, theWidth + 1, 1);
			this.FillRect(theX, theY + theHeight, theWidth + 1, 1);
			this.FillRect(theX, theY + 1, 1, theHeight - 1);
			this.FillRect(theX + theWidth, theY + 1, 1, theHeight - 1);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0000F94A File Offset: 0x0000DB4A
		public void DrawRect(Rect theRect)
		{
			this.DrawRect(theRect.mX, theRect.mY, theRect.mWidth, theRect.mHeight);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0000F970 File Offset: 0x0000DB70
		public void ClearRect(int theX, int theY, int theWidth, int theHeight)
		{
			this.SetAsCurrentContext();
			this.mDestRect.mX = theX + (int)this.mTransX;
			this.mDestRect.mY = theY + (int)this.mTransY;
			this.mDestRect.mWidth = theWidth;
			this.mDestRect.mHeight = theHeight;
			Rect theRect = this.mDestRect.Intersection(this.mClipRect);
			this.mRenderDevice.ClearRect(theRect);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0000F9E2 File Offset: 0x0000DBE2
		public void ClearRect(Rect theRect)
		{
			this.ClearRect(theRect.mX, theRect.mY, theRect.mWidth, theRect.mHeight);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0000FA06 File Offset: 0x0000DC06
		public void DrawString(string theString, int theX, int theY)
		{
			if (this.mFont != null)
			{
				this.mFont.DrawString(this, theX, theY, theString, this.GetFinalColor(), this.mClipRect);
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0000FA2C File Offset: 0x0000DC2C
		public void DrawLine(int theStartX, int theStartY, int theEndX, int theEndY)
		{
			double theStartX2 = (double)((float)theStartX + this.mTransX);
			double theStartY2 = (double)((float)theStartY + this.mTransY);
			double theEndX2 = (double)((float)theEndX + this.mTransX);
			double theEndY2 = (double)((float)theEndY + this.mTransY);
			if (!this.DrawLineClipHelper(ref theStartX2, ref theStartY2, ref theEndX2, ref theEndY2))
			{
				return;
			}
			this.SetAsCurrentContext();
			this.mRenderDevice.DrawLine(theStartX2, theStartY2, theEndX2, theEndY2, this.GetFinalColor(), this.mDrawMode);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0000FA98 File Offset: 0x0000DC98
		public void DrawLineAA(int theStartX, int theStartY, int theEndX, int theEndY)
		{
			double theStartX2 = (double)((float)theStartX + this.mTransX);
			double theStartY2 = (double)((float)theStartY + this.mTransY);
			double theEndX2 = (double)((float)theEndX + this.mTransX);
			double theEndY2 = (double)((float)theEndY + this.mTransY);
			if (!this.DrawLineClipHelper(ref theStartX2, ref theStartY2, ref theEndX2, ref theEndY2))
			{
				return;
			}
			this.SetAsCurrentContext();
			this.mRenderDevice.DrawLine(theStartX2, theStartY2, theEndX2, theEndY2, this.GetFinalColor(), this.mDrawMode, true);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0000FB08 File Offset: 0x0000DD08
		public void PolyFill(SexyPoint[] theVertexList, int theNumVertices, bool convex)
		{
			this.SetAsCurrentContext();
			if (convex && this.mRenderDevice.CanFillPoly())
			{
				this.mRenderDevice.FillPoly(theVertexList, theNumVertices, this.mClipRect, this.GetFinalColor(), this.mDrawMode, (int)this.mTransX, (int)this.mTransY);
				return;
			}
			throw new NotSupportedException();
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0000FB5E File Offset: 0x0000DD5E
		public void PolyFillAA(SexyPoint[] theVertexList, int theNumVertices)
		{
			this.PolyFillAA(theVertexList, theNumVertices, false);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0000FB6C File Offset: 0x0000DD6C
		public void PolyFillAA(SexyPoint[] theVertexList, int theNumVertices, bool convex)
		{
			this.SetAsCurrentContext();
			if (convex && this.mRenderDevice.CanFillPoly())
			{
				this.mRenderDevice.FillPoly(theVertexList, theNumVertices, this.mClipRect, this.GetFinalColor(), this.mDrawMode, (int)this.mTransX, (int)this.mTransY);
				return;
			}
			throw new NotSupportedException();
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0000FBC4 File Offset: 0x0000DDC4
		public void DrawImage(Image theImage, int theX, int theY)
		{
			if (this.mScaleX != 1f || this.mScaleY != 1f)
			{
				this.DrawImage(theImage, theX, theY, theImage.GetRect());
				return;
			}
			theX += (int)this.mTransX;
			theY += (int)this.mTransY;
			this.mDestRect.mX = theX;
			this.mDestRect.mY = theY;
			this.mDestRect.mWidth = theImage.GetWidth();
			this.mDestRect.mHeight = theImage.GetHeight();
			Rect rect = this.mDestRect.Intersection(this.mClipRect);
			this.mSrcRect.mX = rect.mX - theX;
			this.mSrcRect.mY = rect.mY - theY;
			this.mSrcRect.mWidth = rect.mWidth;
			this.mSrcRect.mHeight = rect.mHeight;
			if (this.mSrcRect.mWidth > 0 && this.mSrcRect.mHeight > 0)
			{
				this.SetAsCurrentContext();
				this.mRenderDevice.Blt(theImage, rect.mX, rect.mY, this.mSrcRect, this.GetImageColor(), this.mDrawMode);
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0000FCF8 File Offset: 0x0000DEF8
		public void DrawImage(Image theImage, int theX, int theY, Rect theSrcRect)
		{
			if (theSrcRect.mX + theSrcRect.mWidth > theImage.GetWidth() || theSrcRect.mY + theSrcRect.mHeight > theImage.GetHeight())
			{
				return;
			}
			theX += (int)this.mTransX;
			theY += (int)this.mTransY;
			if (this.mScaleX != 1f || this.mScaleY != 1f)
			{
				Rect theDestRect = new Rect((int)((double)this.mScaleOrigX + Math.Floor((double)(((float)theX - this.mScaleOrigX) * this.mScaleX))), (int)((double)this.mScaleOrigY + Math.Floor((double)(((float)theY - this.mScaleOrigY) * this.mScaleY))), (int)Math.Ceiling((double)((float)theSrcRect.mWidth * this.mScaleX)), (int)Math.Ceiling((double)((float)theSrcRect.mHeight * this.mScaleY)));
				this.SetAsCurrentContext();
				this.mRenderDevice.BltStretched(theImage, theDestRect, theSrcRect, this.mClipRect, this.GetImageColor(), this.mDrawMode, this.mFastStretch);
				return;
			}
			this.mDestRect.mX = theX;
			this.mDestRect.mY = theY;
			this.mDestRect.mWidth = theSrcRect.mWidth;
			this.mDestRect.mHeight = theSrcRect.mHeight;
			Rect rect = this.mDestRect.Intersection(this.mClipRect);
			this.mSrcRect.mX = theSrcRect.mX + rect.mX - theX;
			this.mSrcRect.mY = theSrcRect.mY + rect.mY - theY;
			this.mSrcRect.mWidth = rect.mWidth;
			this.mSrcRect.mHeight = rect.mHeight;
			if (this.mSrcRect.mWidth > 0 && this.mSrcRect.mHeight > 0)
			{
				this.SetAsCurrentContext();
				this.mRenderDevice.Blt(theImage, rect.mX, rect.mY, this.mSrcRect, this.GetImageColor(), this.mDrawMode);
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0000FF00 File Offset: 0x0000E100
		public void DrawImage(Image theImage, Rect theDestRect, Rect theSrcRect)
		{
			this.mDestRect.mX = theDestRect.mX + (int)this.mTransX;
			this.mDestRect.mY = theDestRect.mY + (int)this.mTransY;
			this.mDestRect.mWidth = theDestRect.mWidth;
			this.mDestRect.mHeight = theDestRect.mHeight;
			if (this.mScaleX != 1f || this.mScaleY != 1f)
			{
				this.mDestRect = new Rect((int)((double)this.mScaleOrigX + Math.Floor((double)(((float)this.mDestRect.mX - this.mScaleOrigX) * this.mScaleX))), (int)((double)this.mScaleOrigY + Math.Floor((double)(((float)this.mDestRect.mY - this.mScaleOrigY) * this.mScaleY))), (int)Math.Ceiling((double)((float)this.mDestRect.mWidth * this.mScaleX)), (int)Math.Ceiling((double)((float)this.mDestRect.mHeight * this.mScaleY)));
			}
			this.SetAsCurrentContext();
			this.mRenderDevice.BltStretched(theImage, this.mDestRect, theSrcRect, this.mClipRect, this.GetImageColor(), this.mDrawMode, this.mFastStretch);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00010044 File Offset: 0x0000E244
		public void DrawImage(Image theImage, int theX, int theY, int theStretchedWidth, int theStretchedHeight)
		{
			this.mDestRect.mX = theX + (int)this.mTransX;
			this.mDestRect.mY = theY + (int)this.mTransY;
			this.mDestRect.mWidth = theStretchedWidth;
			this.mDestRect.mHeight = theStretchedHeight;
			this.mSrcRect.mX = 0;
			this.mSrcRect.mY = 0;
			this.mSrcRect.mWidth = theImage.mWidth;
			this.mSrcRect.mHeight = theImage.mHeight;
			this.SetAsCurrentContext();
			this.mRenderDevice.BltStretched(theImage, this.mDestRect, theImage.GetRect(), this.mClipRect, this.GetImageColor(), this.mDrawMode, this.mFastStretch);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00010104 File Offset: 0x0000E304
		public void DrawImageF(Image theImage, float theX, float theY)
		{
			theX += this.mTransX;
			theY += this.mTransY;
			this.SetAsCurrentContext();
			this.mRenderDevice.BltF(theImage, theX, theY, theImage.GetRect(), this.mClipRect, this.GetImageColor(), this.mDrawMode);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00010151 File Offset: 0x0000E351
		public void DrawImageF(Image theImage, float theX, float theY, Rect theSrcRect)
		{
			theX += this.mTransX;
			theY += this.mTransY;
			this.SetAsCurrentContext();
			this.mRenderDevice.BltF(theImage, theX, theY, theSrcRect, this.mClipRect, this.GetImageColor(), this.mDrawMode);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001018F File Offset: 0x0000E38F
		public void DrawImageMirror(Image theImage, int theX, int theY)
		{
			this.DrawImageMirror(theImage, theX, theY, true);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0001019B File Offset: 0x0000E39B
		public void DrawImageMirror(Image theImage, int theX, int theY, int theStretchedWidth, int theStretchedHeight)
		{
			this.mDestRect.setValue(theX, theY, theStretchedWidth, theStretchedHeight);
			this.DrawImageMirror(theImage, this.mDestRect, theImage.GetRect(), true);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x000101C2 File Offset: 0x0000E3C2
		public void DrawImageMirror(Image theImage, int theX, int theY, bool mirror)
		{
			this.DrawImageMirror(theImage, theX, theY, theImage.GetRect(), mirror);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x000101D5 File Offset: 0x0000E3D5
		public void DrawImageMirror(Image theImage, int theX, int theY, Rect theSrcRect)
		{
			this.DrawImageMirror(theImage, theX, theY, theSrcRect, true);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x000101E4 File Offset: 0x0000E3E4
		public void DrawImageMirror(Image theImage, int theX, int theY, Rect theSrcRect, bool mirror)
		{
			if (!mirror)
			{
				this.DrawImage(theImage, theX, theY, theSrcRect);
				return;
			}
			theX += (int)this.mTransX;
			theY += (int)this.mTransY;
			if (theSrcRect.mX + theSrcRect.mWidth > theImage.GetWidth() || theSrcRect.mY + theSrcRect.mHeight > theImage.GetHeight())
			{
				return;
			}
			this.mDestRect.mX = theX;
			this.mDestRect.mY = theY;
			this.mDestRect.mWidth = theSrcRect.mWidth;
			this.mDestRect.mHeight = theSrcRect.mHeight;
			Rect rect = this.mDestRect.Intersection(this.mClipRect);
			int num = theSrcRect.mWidth - rect.mWidth;
			int num2 = rect.mX - theX;
			int num3 = num - num2;
			this.mSrcRect.mX = theSrcRect.mX + num3;
			this.mSrcRect.mY = theSrcRect.mY + rect.mY - theY;
			this.mSrcRect.mWidth = rect.mWidth;
			this.mSrcRect.mHeight = rect.mHeight;
			if (this.mSrcRect.mWidth > 0 && this.mSrcRect.mHeight > 0)
			{
				this.SetAsCurrentContext();
				this.mRenderDevice.BltMirror(theImage, rect.mX, rect.mY, this.mSrcRect, this.GetImageColor(), this.mDrawMode);
			}
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00010354 File Offset: 0x0000E554
		public void DrawImageMirror(Image theImage, Rect theDestRect, Rect theSrcRect)
		{
			this.DrawImageMirror(theImage, theDestRect, theSrcRect, true);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00010360 File Offset: 0x0000E560
		public void DrawImageMirror(Image theImage, Rect theDestRect, Rect theSrcRect, bool mirror)
		{
			if (!mirror)
			{
				this.DrawImage(theImage, theDestRect, theSrcRect);
				return;
			}
			this.mDestRect.mX = theDestRect.mX + (int)this.mTransX;
			this.mDestRect.mY = theDestRect.mY + (int)this.mTransY;
			this.mDestRect.mWidth = theDestRect.mWidth;
			this.mDestRect.mHeight = theDestRect.mHeight;
			this.SetAsCurrentContext();
			this.mRenderDevice.BltStretched(theImage, this.mDestRect, theSrcRect, this.mClipRect, this.GetImageColor(), this.mDrawMode, this.mFastStretch, true);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00010405 File Offset: 0x0000E605
		public void DrawImageRotated(Image theImage, int theX, int theY, double theRot)
		{
			this.DrawImageRotated(theImage, theX, theY, theRot, Rect.INVALIDATE_RECT);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00010418 File Offset: 0x0000E618
		public void DrawImageRotated(Image theImage, int theX, int theY, double theRot, Rect theSrcRect)
		{
			if (theSrcRect == Rect.INVALIDATE_RECT)
			{
				int num = theImage.GetWidth() / 2;
				int num2 = theImage.GetHeight() / 2;
				this.DrawImageRotatedF(theImage, (float)theX, (float)theY, theRot, (float)num, (float)num2, theSrcRect);
				return;
			}
			int num3 = theSrcRect.mWidth / 2;
			int num4 = theSrcRect.mHeight / 2;
			this.DrawImageRotatedF(theImage, (float)theX, (float)theY, theRot, (float)num3, (float)num4, theSrcRect);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00010480 File Offset: 0x0000E680
		public void DrawImageRotated(Image theImage, int theX, int theY, double theRot, int theRotCenterX, int theRotCenterY)
		{
			this.DrawImageRotated(theImage, theX, theY, theRot, theRotCenterX, theRotCenterY, Rect.INVALIDATE_RECT);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00010496 File Offset: 0x0000E696
		public void DrawImageRotated(Image theImage, int theX, int theY, double theRot, int theRotCenterX, int theRotCenterY, Rect theSrcRect)
		{
			this.DrawImageRotatedF(theImage, (float)theX, (float)theY, theRot, (float)theRotCenterX, (float)theRotCenterY, theSrcRect);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x000104AD File Offset: 0x0000E6AD
		public void DrawImageRotatedF(Image theImage, float theX, float theY, double theRot)
		{
			this.DrawImageRotatedF(theImage, theX, theY, theRot, Rect.INVALIDATE_RECT);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x000104C0 File Offset: 0x0000E6C0
		public void DrawImageRotatedF(Image theImage, float theX, float theY, double theRot, Rect theSrcRect)
		{
			if (theSrcRect == Rect.INVALIDATE_RECT)
			{
				float theRotCenterX = (float)theImage.GetWidth() / 2f;
				float theRotCenterY = (float)theImage.GetHeight() / 2f;
				this.DrawImageRotatedF(theImage, theX, theY, theRot, theRotCenterX, theRotCenterY, theSrcRect);
				return;
			}
			float theRotCenterX2 = (float)theSrcRect.mWidth / 2f;
			float theRotCenterY2 = (float)theSrcRect.mHeight / 2f;
			this.DrawImageRotatedF(theImage, theX, theY, theRot, theRotCenterX2, theRotCenterY2, theSrcRect);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00010534 File Offset: 0x0000E734
		public void DrawImageRotatedF(Image theImage, float theX, float theY, double theRot, float theRotCenterX, float theRotCenterY)
		{
			this.DrawImageRotatedF(theImage, theX, theY, theRot, theRotCenterX, theRotCenterY, Rect.INVALIDATE_RECT);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001054C File Offset: 0x0000E74C
		public void DrawImageRotatedF(Image theImage, float theX, float theY, double theRot, float theRotCenterX, float theRotCenterY, Rect theSrcRect)
		{
			theX += this.mTransX;
			theY += this.mTransY;
			this.SetAsCurrentContext();
			if (theSrcRect == Rect.INVALIDATE_RECT)
			{
				this.mRenderDevice.BltRotated(theImage, theX, theY, theImage.GetRect(), this.mClipRect, this.GetImageColor(), this.mDrawMode, theRot, theRotCenterX, theRotCenterY);
				return;
			}
			this.mRenderDevice.BltRotated(theImage, theX, theY, theSrcRect, this.mClipRect, this.GetImageColor(), this.mDrawMode, theRot, theRotCenterX, theRotCenterY);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x000105D6 File Offset: 0x0000E7D6
		public void DrawImageMatrix(Image theImage, SexyMatrix3 theMatrix, float x)
		{
			this.DrawImageMatrix(theImage, theMatrix, x, 0f);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x000105E6 File Offset: 0x0000E7E6
		public void DrawImageMatrix(Image theImage, SexyMatrix3 theMatrix)
		{
			this.DrawImageMatrix(theImage, theMatrix, 0f, 0f);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x000105FC File Offset: 0x0000E7FC
		public void DrawImageMatrix(Image theImage, SexyMatrix3 theMatrix, float x, float y)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.BltMatrix(theImage, x + this.mTransX, y + this.mTransY, theMatrix, this.mClipRect, this.GetImageColor(), this.mDrawMode, theImage.GetRect(), this.mLinearBlend);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001064B File Offset: 0x0000E84B
		public void DrawImageMatrix(Image theImage, SexyMatrix3 theMatrix, Rect theSrcRect, float x)
		{
			this.DrawImageMatrix(theImage, theMatrix, theSrcRect, x, 0f);
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001065D File Offset: 0x0000E85D
		public void DrawImageMatrix(Image theImage, SexyMatrix3 theMatrix, Rect theSrcRect)
		{
			this.DrawImageMatrix(theImage, theMatrix, theSrcRect, 0f, 0f);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00010674 File Offset: 0x0000E874
		public void DrawImageMatrix(Image theImage, SexyMatrix3 theMatrix, Rect theSrcRect, float x, float y)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.BltMatrix(theImage, x + this.mTransX, y + this.mTransY, theMatrix, this.mClipRect, this.GetImageColor(), this.mDrawMode, theSrcRect, this.mLinearBlend);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x000106BF File Offset: 0x0000E8BF
		public void DrawImageTransform(Image theImage, Transform theTransform, float x, float y)
		{
			this.DrawImageTransformHelper(theImage, theTransform, theImage.GetRect(), x, y, false);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x000106D3 File Offset: 0x0000E8D3
		public void DrawImageTransform(Image theImage, Transform theTransform, Rect theSrcRect, float x, float y)
		{
			this.DrawImageTransformHelper(theImage, theTransform, theSrcRect, x, y, false);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x000106E3 File Offset: 0x0000E8E3
		public void DrawImageTransformF(Image theImage, Transform theTransform, float x, float y)
		{
			this.DrawImageTransformHelper(theImage, theTransform, theImage.GetRect(), x, y, true);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x000106F7 File Offset: 0x0000E8F7
		public void DrawImageTransformF(Image theImage, Transform theTransform, Rect theSrcRect, float x, float y)
		{
			this.DrawImageTransformHelper(theImage, theTransform, theSrcRect, x, y, true);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00010708 File Offset: 0x0000E908
		public void DrawTriangleTex(Image theTexture, SexyVertex2D v1, SexyVertex2D v2, SexyVertex2D v3)
		{
			SexyVertex2D[,] array = new SexyVertex2D[1, 3];
			array[0, 0] = v1;
			array[0, 1] = v2;
			array[0, 2] = v3;
			SexyVertex2D[,] theVertices = array;
			this.SetAsCurrentContext();
			this.mRenderDevice.BltTriangles(theTexture, theVertices, 1, this.GetImageColor(), this.mDrawMode, this.mTransX, this.mTransY, this.mLinearBlend, this.mClipRect);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00010784 File Offset: 0x0000E984
		public void DrawTrianglesTex(Image theTexture, SexyVertex2D[,] theVertices, int theNumTriangles)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.BltTriangles(theTexture, theVertices, theNumTriangles, this.GetImageColor(), this.mDrawMode, this.mTransX, this.mTransY, this.mLinearBlend, this.mClipRect);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x000107CC File Offset: 0x0000E9CC
		public void DrawTrianglesTex(Image theTexture, SexyVertex2D[,] theVertices, int theNumTriangles, SexyColor theColor, int theDrawMode, float tx, float ty, bool blend, Rect theClipRect)
		{
			this.SetAsCurrentContext();
			this.mRenderDevice.BltTriangles(theTexture, theVertices, theNumTriangles, theColor, theDrawMode, tx, ty, blend, theClipRect);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x000107FC File Offset: 0x0000E9FC
		public void DrawTrianglesTexStrip(Image theTexture, SexyVertex2D[] theVertices, int theNumTriangles)
		{
			this.DrawTrianglesTexStrip(theTexture, theVertices, theNumTriangles, this.GetImageColor(), this.mDrawMode, this.mTransX, this.mTransY, this.mLinearBlend);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00010830 File Offset: 0x0000EA30
		public void DrawTrianglesTexStrip(Image theTexture, SexyVertex2D[] theVertices, int theNumTriangles, SexyColor theColor, int theDrawMode, float tx, float ty, bool blend)
		{
			this.SetAsCurrentContext();
			SexyVertex2D[,] array = new SexyVertex2D[100, 3];
			int i = 0;
			while (i < theNumTriangles)
			{
				int num = Math.Min(100, theNumTriangles - i);
				for (int j = 0; j < num; j++)
				{
					array[j, 0] = theVertices[i];
					array[j, 1] = theVertices[i + 1];
					array[j, 2] = theVertices[i + 2];
					i++;
				}
				this.mRenderDevice.BltTriangles(theTexture, array, num, theColor, theDrawMode, tx, ty, blend);
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x000108DC File Offset: 0x0000EADC
		public void DrawImageCel(Image theImageStrip, int theX, int theY, int theCel)
		{
			this.DrawImageCel(theImageStrip, theX, theY, theCel % theImageStrip.mNumCols, theCel / theImageStrip.mNumCols);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x000108F9 File Offset: 0x0000EAF9
		public void DrawImageCel(Image theImageStrip, Rect theDestRect, int theCel)
		{
			this.DrawImageCel(theImageStrip, theDestRect, theCel % theImageStrip.mNumCols, theCel / theImageStrip.mNumCols);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00010914 File Offset: 0x0000EB14
		public void DrawImageCel(Image theImageStrip, int theX, int theY, int theCelCol, int theCelRow)
		{
			if (theCelRow < 0 || theCelCol < 0 || theCelRow >= theImageStrip.mNumRows || theCelCol >= theImageStrip.mNumCols)
			{
				return;
			}
			int num = theImageStrip.mWidth / theImageStrip.mNumCols;
			int num2 = theImageStrip.mHeight / theImageStrip.mNumRows;
			Rect theSrcRect = new Rect(num * theCelCol, num2 * theCelRow, num, num2);
			this.DrawImage(theImageStrip, theX, theY, theSrcRect);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00010978 File Offset: 0x0000EB78
		public void DrawImageCel(Image theImageStrip, Rect theDestRect, int theCelCol, int theCelRow)
		{
			if (theCelRow < 0 || theCelCol < 0 || theCelRow >= theImageStrip.mNumRows || theCelCol >= theImageStrip.mNumCols)
			{
				return;
			}
			int num = theImageStrip.mWidth / theImageStrip.mNumCols;
			int num2 = theImageStrip.mHeight / theImageStrip.mNumRows;
			Rect theSrcRect = new Rect(num * theCelCol, num2 * theCelRow, num, num2);
			this.DrawImage(theImageStrip, theDestRect, theSrcRect);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x000109D7 File Offset: 0x0000EBD7
		public void DrawImageAnim(Image theImageAnim, int theX, int theY, int theTime)
		{
			this.DrawImageCel(theImageAnim, theX, theY, theImageAnim.GetAnimCel(theTime));
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x000109EA File Offset: 0x0000EBEA
		public void BeginDrawSprite()
		{
			this.mRenderDevice.BeginSprite();
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x000109F7 File Offset: 0x0000EBF7
		public void EndDrawSprite()
		{
			this.mRenderDevice.EndSprite();
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00010A04 File Offset: 0x0000EC04
		public void DrawSprite(Image theImage, SexyTransform2D theTransform, Rect theSrcRect)
		{
			this.mRenderDevice.DrawSprite(theImage, this.GetImageColor(), this.mDrawMode, theTransform, theSrcRect, true);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00010A24 File Offset: 0x0000EC24
		public void ClearClipRect()
		{
			if (this.mDestImage != null)
			{
				this.mClipRect.mX = 0;
				this.mClipRect.mY = 0;
				this.mClipRect.mWidth = this.mDestImage.GetWidth();
				this.mClipRect.mHeight = this.mDestImage.GetHeight();
				return;
			}
			this.mClipRect.mX = 0;
			this.mClipRect.mY = 0;
			this.mClipRect.mWidth = GlobalMembers.gSexyAppBase.mWidth;
			this.mClipRect.mHeight = GlobalMembers.gSexyAppBase.mHeight;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00010AC0 File Offset: 0x0000ECC0
		public void SetClipRect(int theX, int theY, int theWidth, int theHeight)
		{
			if (this.mDestImage != null)
			{
				this.mClipRect.mX = 0;
				this.mClipRect.mY = 0;
				this.mClipRect.mWidth = this.mDestImage.GetWidth();
				this.mClipRect.mHeight = this.mDestImage.GetHeight();
				this.mDestRect.mX = theX + (int)this.mTransX;
				this.mDestRect.mY = theY + (int)this.mTransY;
				this.mDestRect.mWidth = theWidth;
				this.mDestRect.mHeight = theHeight;
				this.mClipRect = this.mClipRect.Intersection(this.mDestRect);
				return;
			}
			this.mClipRect.mX = -1;
			this.mClipRect.mY = -1;
			this.mClipRect.mWidth = GlobalMembers.gSexyAppBase.mWidth + 1;
			this.mClipRect.mHeight = GlobalMembers.gSexyAppBase.mHeight + 1;
			this.mDestRect.mX = theX + (int)this.mTransX;
			this.mDestRect.mY = theY + (int)this.mTransY;
			this.mDestRect.mWidth = theWidth;
			this.mDestRect.mHeight = theHeight;
			this.mClipRect = this.mClipRect.Intersection(this.mDestRect);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00010C13 File Offset: 0x0000EE13
		public void SetClipRect(Rect theRect)
		{
			this.SetClipRect(theRect.mX, theRect.mY, theRect.mWidth, theRect.mHeight);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00010C38 File Offset: 0x0000EE38
		public void ClipRect(int theX, int theY, int theWidth, int theHeight)
		{
			this.mDestRect.mX = theX + (int)this.mTransX;
			this.mDestRect.mY = theY + (int)this.mTransY;
			this.mDestRect.mWidth = theWidth;
			this.mDestRect.mHeight = theHeight;
			this.mClipRect = this.mClipRect.Intersection(this.mDestRect);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00010C9D File Offset: 0x0000EE9D
		public void ClipRect(Rect theRect)
		{
			this.ClipRect(theRect.mX, theRect.mY, theRect.mWidth, theRect.mHeight);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00010CC1 File Offset: 0x0000EEC1
		public void Translate(int theTransX, int theTransY)
		{
			this.mTransX += (float)theTransX;
			this.mTransY += (float)theTransY;
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00010CE1 File Offset: 0x0000EEE1
		public void TranslateF(float theTransX, float theTransY)
		{
			this.mTransX += theTransX;
			this.mTransY += theTransY;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00010CFF File Offset: 0x0000EEFF
		public void SetScale(float theScaleX, float theScaleY, float theOrigX, float theOrigY)
		{
			this.mScaleX = theScaleX;
			this.mScaleY = theScaleY;
			this.mScaleOrigX = theOrigX + this.mTransX;
			this.mScaleOrigY = theOrigY + this.mTransY;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00010D2C File Offset: 0x0000EF2C
		public int StringWidth(string theString)
		{
			return this.mFont.StringWidth(theString);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00010D3A File Offset: 0x0000EF3A
		public void DrawImageBox(Rect theDest, Image theComponentImage)
		{
			this.DrawImageBox(theComponentImage.GetRect(), theDest, theComponentImage);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00010D4C File Offset: 0x0000EF4C
		public void DrawImageBox(Rect theSrc, Rect theDest, Image theComponentImage)
		{
			if (theSrc.mWidth <= 0 || theSrc.mHeight <= 0)
			{
				return;
			}
			int num = theSrc.mWidth / 3;
			int num2 = theSrc.mHeight / 3;
			int mX = theSrc.mX;
			int mY = theSrc.mY;
			int num3 = theSrc.mWidth - num * 2;
			int num4 = theSrc.mHeight - num2 * 2;
			int num5 = num;
			int num6 = num2;
			bool flag = false;
			if (theDest.mWidth < num * 2)
			{
				num5 = theDest.mWidth / 2;
				if ((theDest.mWidth & 1) == 1)
				{
					num5++;
				}
				flag = true;
			}
			if (theDest.mHeight < num2 * 2)
			{
				num6 = theDest.mHeight / 2;
				if ((theDest.mHeight & 1) == 1)
				{
					num6++;
				}
				flag = true;
			}
			Rect mClipRect = this.mClipRect;
			if (flag)
			{
				this.mDestRect.setValue(ref theDest.mX, ref theDest.mY, ref num5, ref num6);
				this.mSrcRect.setValue(ref mX, ref mY, ref num, ref num2);
				this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
				this.mDestRect.setValue(theDest.mX + theDest.mWidth - num5, theDest.mY, num5, num6);
				this.mSrcRect.setValue(mX + num + num3, mY, num, num2);
				this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
				this.mDestRect.setValue(theDest.mX, theDest.mY + theDest.mHeight - num6, num5, num6);
				this.mSrcRect.setValue(mX, mY + num2 + num4, num, num2);
				this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
				this.mDestRect.setValue(theDest.mX + theDest.mWidth - num5, theDest.mY + theDest.mHeight - num6, num5, num6);
				this.mSrcRect.setValue(mX + num + num3, mY + num2 + num4, num, num2);
				this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
				this.ClipRect(theDest.mX + num5, theDest.mY, theDest.mWidth - num5 * 2, theDest.mHeight);
				int i;
				for (i = 0; i < (theDest.mWidth - num * 2 + num3 - 1) / num3; i++)
				{
					this.mDestRect.setValue(theDest.mX + num5 + i * num3, theDest.mY, num3, num6);
					this.mSrcRect.setValue(mX + num, mY, num3, num2);
					this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
					this.mDestRect.setValue(theDest.mX + num5 + i * num3, theDest.mY + theDest.mHeight - num6, num3, num6);
					this.mSrcRect.setValue(mX + num, mY + num2 + num4, num3, num2);
					this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
				}
				this.mClipRect = mClipRect;
				this.ClipRect(theDest.mX, theDest.mY + num6, theDest.mWidth, theDest.mHeight - num6 * 2);
				for (int j = 0; j < (theDest.mHeight - num2 * 2 + num4 - 1) / num4; j++)
				{
					this.mDestRect.setValue(theDest.mX + num5 + i * num3, theDest.mY, num3, num6);
					this.mSrcRect.setValue(mX, mY + num2, num, num4);
					this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
					this.mDestRect.setValue(theDest.mX + theDest.mWidth - num5, theDest.mY + num6 + j * num4, num5, num4);
					this.mSrcRect.setValue(mX + num + num3, mY + num2, num, num4);
					this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
				}
				this.mClipRect = mClipRect;
				this.ClipRect(theDest.mX + num5, theDest.mY + num6, theDest.mWidth - num5 * 2, theDest.mHeight - num6 * 2);
				for (i = 0; i < (theDest.mWidth - num5 * 2 + num3 - 1) / num3; i++)
				{
					for (int j = 0; j < (theDest.mHeight - num6 * 2 + num4 - 1) / num4; j++)
					{
						this.mSrcRect.setValue(mX + num5, mY + num6, num3, num4);
						this.DrawImage(theComponentImage, theDest.mX + num5 + i * num3, theDest.mY + num6 + j * num4, this.mSrcRect);
					}
				}
				this.mClipRect = mClipRect;
				return;
			}
			this.mSrcRect.setValue(mX, mY, num, num2);
			this.DrawImage(theComponentImage, theDest.mX, theDest.mY, this.mSrcRect);
			this.mSrcRect.setValue(mX + num + num3, mY, num, num2);
			this.DrawImage(theComponentImage, theDest.mX + theDest.mWidth - num, theDest.mY, this.mSrcRect);
			this.mSrcRect.setValue(mX, mY + num2 + num4, num, num2);
			this.DrawImage(theComponentImage, theDest.mX, theDest.mY + theDest.mHeight - num2, this.mSrcRect);
			this.mSrcRect.setValue(mX + num + num3, mY + num2 + num4, num, num2);
			this.DrawImage(theComponentImage, theDest.mX + theDest.mWidth - num, theDest.mY + theDest.mHeight - num2, this.mSrcRect);
			this.ClipRect(theDest.mX + num, theDest.mY, theDest.mWidth - num * 2, theDest.mHeight);
			for (int k = 0; k < (theDest.mWidth - num * 2 + num3 - 1) / num3; k++)
			{
				this.mSrcRect.setValue(mX + num, mY, num3, num2);
				this.DrawImage(theComponentImage, theDest.mX + num + k * num3, theDest.mY, this.mSrcRect);
				this.mSrcRect.setValue(mX + num, mY + num2 + num4, num3, num2);
				this.DrawImage(theComponentImage, theDest.mX + num + k * num3, theDest.mY + theDest.mHeight - num2, this.mSrcRect);
			}
			this.mClipRect = mClipRect;
			this.ClipRect(theDest.mX, theDest.mY + num2, theDest.mWidth, theDest.mHeight - num2 * 2);
			for (int l = 0; l < (theDest.mHeight - num2 * 2 + num4 - 1) / num4; l++)
			{
				this.mSrcRect.setValue(mX, mY + num2, num, num4);
				this.DrawImage(theComponentImage, theDest.mX, theDest.mY + num2 + l * num4, this.mSrcRect);
				this.mSrcRect.setValue(mX + num + num3, mY + num2, num, num4);
				this.DrawImage(theComponentImage, theDest.mX + theDest.mWidth - num, theDest.mY + num2 + l * num4, this.mSrcRect);
			}
			this.mClipRect = mClipRect;
			this.ClipRect(theDest.mX + num, theDest.mY + num2, theDest.mWidth - num * 2, theDest.mHeight - num2 * 2);
			for (int k = 0; k < (theDest.mWidth - num * 2 + num3 - 1) / num3; k++)
			{
				for (int l = 0; l < (theDest.mHeight - num2 * 2 + num4 - 1) / num4; l++)
				{
					this.mSrcRect.setValue(mX + num, mY + num2, num3, num4);
					this.DrawImage(theComponentImage, theDest.mX + num + k * num3, theDest.mY + num2 + l * num4, this.mSrcRect);
				}
			}
			this.mClipRect = mClipRect;
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001155E File Offset: 0x0000F75E
		public void DrawImageBoxStretch(Rect theDest, Image theComponentImage)
		{
			this.DrawImageBoxStretch(theComponentImage.GetRect(), theDest, theComponentImage);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00011570 File Offset: 0x0000F770
		public void DrawImageBoxStretch(Rect theSrc, Rect theDest, Image theComponentImage)
		{
			if (theSrc.mWidth <= 0 || theSrc.mHeight <= 0)
			{
				return;
			}
			int num = theSrc.mWidth / 3;
			int num2 = theSrc.mHeight / 3;
			int mX = theSrc.mX;
			int mY = theSrc.mY;
			int num3 = theSrc.mWidth - num * 2;
			int num4 = theSrc.mHeight - num2 * 2;
			int num5 = num;
			int num6 = num2;
			if (theDest.mWidth < num * 2)
			{
				num5 = theDest.mWidth / 2;
				if ((theDest.mWidth & 1) == 1)
				{
					num5++;
				}
			}
			if (theDest.mHeight < num2 * 2)
			{
				num6 = theDest.mHeight / 2;
				if ((theDest.mHeight & 1) == 1)
				{
					num6++;
				}
			}
			this.mDestRect.setValue(theDest.mX, theDest.mY, num5, num6);
			this.mSrcRect.setValue(mX, mY, num, num2);
			this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
			this.mDestRect.setValue(theDest.mX + theDest.mWidth - num5, theDest.mY, num5, num6);
			this.mSrcRect.setValue(mX + num + num3, mY, num, num2);
			this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
			this.mDestRect.setValue(theDest.mX, theDest.mY + theDest.mHeight - num6, num5, num6);
			this.mSrcRect.setValue(mX, mY + num2 + num4, num, num2);
			this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
			this.mDestRect.setValue(theDest.mX + theDest.mWidth - num5, theDest.mY + theDest.mHeight - num6, num5, num6);
			this.mSrcRect.setValue(mX + num + num3, mY + num2 + num4, num, num2);
			this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
			if (theDest.mWidth - num5 * 2 > 0)
			{
				this.mDestRect.setValue(theDest.mX + num5, theDest.mY, theDest.mWidth - num5 * 2, num6);
				this.mSrcRect.setValue(mX + num, mY, num3, num2);
				this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
				this.mDestRect.setValue(theDest.mX + num5, theDest.mY + theDest.mHeight - num6, theDest.mWidth - num5 * 2, num6);
				this.mSrcRect.setValue(mX + num, mY + num2 + num4, num3, num2);
				this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
			}
			if (theDest.mHeight - num6 * 2 > 0)
			{
				this.mDestRect.setValue(theDest.mX, theDest.mY + num6, num5, theDest.mHeight - num6 * 2);
				this.mSrcRect.setValue(mX, mY + num2, num, num4);
				this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
				this.mDestRect.setValue(theDest.mX + theDest.mWidth - num5, theDest.mY + num6, num5, theDest.mHeight - num6 * 2);
				this.mSrcRect.setValue(mX + num + num3, mY + num2, num, num4);
				this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
			}
			if (theDest.mWidth - num5 * 2 > 0 && theDest.mHeight - num6 * 2 > 0)
			{
				this.mDestRect.setValue(theDest.mX + num5, theDest.mY + num6, theDest.mWidth - num5 * 2, theDest.mHeight - num6 * 2);
				this.mSrcRect.setValue(mX + num, mY + num2, num3, num4);
				this.DrawImage(theComponentImage, this.mDestRect, this.mSrcRect);
			}
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00011964 File Offset: 0x0000FB64
		public int WriteString(string theString, int theX, int theY, int theWidth, int theJustification, bool drawString, int theOffset, int theLength)
		{
			return this.WriteString(theString, theX, theY, theWidth, theJustification, drawString, theOffset, theLength, -1);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00011988 File Offset: 0x0000FB88
		public int WriteString(string theString, int theX, int theY, int theWidth, int theJustification, bool drawString, int theOffset)
		{
			return this.WriteString(theString, theX, theY, theWidth, theJustification, drawString, theOffset, -1, -1);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x000119A8 File Offset: 0x0000FBA8
		public int WriteString(string theString, int theX, int theY, int theWidth, int theJustification, bool drawString)
		{
			return this.WriteString(theString, theX, theY, theWidth, theJustification, drawString, 0, -1, -1);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x000119C8 File Offset: 0x0000FBC8
		public int WriteString(string theString, int theX, int theY, int theWidth, int theJustification)
		{
			return this.WriteString(theString, theX, theY, theWidth, theJustification, true, 0, -1, -1);
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x000119E8 File Offset: 0x0000FBE8
		public int WriteString(string theString, int theX, int theY, int theWidth)
		{
			return this.WriteString(theString, theX, theY, theWidth, 0, true, 0, -1, -1);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00011A08 File Offset: 0x0000FC08
		public int WriteString(string theString, int theX, int theY)
		{
			return this.WriteString(theString, theX, theY, -1, 0, true, 0, -1, -1);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00011A24 File Offset: 0x0000FC24
		public int WriteString(string theString, int theX, int theY, int theWidth, int theJustification, bool drawString, int theOffset, int theLength, int theOldColor)
		{
			if (theOldColor == -1)
			{
				theOldColor = this.GetFinalColor().ToInt();
			}
			if (drawString)
			{
				switch (theJustification)
				{
				case 0:
					theX += (theWidth - this.WriteString(theString, theX, theY, theWidth, -1, false, theOffset, theLength, theOldColor)) / 2;
					break;
				case 1:
					theX += theWidth - this.WriteString(theString, theX, theY, theWidth, -1, false, theOffset, theLength, theOldColor);
					break;
				}
			}
			if (theLength < 0 || theOffset + theLength > theString.Length)
			{
				theLength = theString.Length;
			}
			else
			{
				theLength = theOffset + theLength;
			}
			this.mStringBuilder.Clear();
			int num = 0;
			for (int i = theOffset; i < theLength; i++)
			{
				if (theString[i] == '^' && this.mWriteColoredString)
				{
					if (i + 1 < theLength && theString[i + 1] == '^')
					{
						this.mStringBuilder.Append("^");
						i++;
					}
					else
					{
						if (i > theLength - 8)
						{
							break;
						}
						int num2 = 0;
						if (theString[i + 1] == 'o')
						{
							if (theString.Substring(i + 1).StartsWith("oldclr"))
							{
								num2 = theOldColor;
							}
						}
						else
						{
							for (int j = 0; j < 6; j++)
							{
								char c = theString[i + j + 1];
								int num3 = 0;
								if (c >= '0' && c <= '9')
								{
									num3 = (int)(c - '0');
								}
								else if (c >= 'A' && c <= 'F')
								{
									num3 = (int)(c - 'A' + '\n');
								}
								else if (c >= 'a' && c <= 'f')
								{
									num3 = (int)(c - 'a' + '\n');
								}
								num2 += num3 << (5 - j) * 4;
							}
						}
						string theString2 = this.mStringBuilder.ToString();
						if (drawString)
						{
							this.DrawString(theString2, theX + num, theY);
							this.SetColor((num2 >> 16) & 255, (num2 >> 8) & 255, num2 & 255, this.GetColor().mAlpha);
						}
						i += 7;
						num += this.mFont.StringWidth(theString2);
						this.mStringBuilder.Clear();
					}
				}
				else
				{
					this.mStringBuilder.Append(theString[i]);
				}
			}
			string theString3 = this.mStringBuilder.ToString();
			if (drawString)
			{
				this.DrawString(theString3, theX + num, theY);
			}
			return num + this.mFont.StringWidth(theString3);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00011C70 File Offset: 0x0000FE70
		public int WriteWordNoAutoWrapped(string theLine, int x, int y)
		{
			SexyColor color = this.GetColor();
			int num = color.ToInt();
			if (((long)num & (long)((-16777216))) == (long)((-16777216)))
			{
				num &= 16777215;
			}
			int length = theLine.Length;
			Font font = this.GetFont();
			int num2 = font.GetAscent() - font.GetAscentPadding();
			int lineSpacing = font.GetLineSpacing();
			int i = 0;
			int num3 = 0;
			char c = '\0';
			char thePrevChar = '\0';
			int num4 = -1;
			int num5 = 0;
			int num6 = 0;
			bool flag = false;
			int num7 = 0;
			int num8 = num7;
			while (i < theLine.Length)
			{
				c = theLine[i];
				if (c == '^' && this.mWriteColoredString)
				{
					if (i + 1 < theLine.Length)
					{
						if (theLine[i + 1] != '^')
						{
							i += 8;
							continue;
						}
						i++;
					}
				}
				else if (c == ' ')
				{
					num4 = i;
				}
				else if (c == '\n')
				{
					flag = true;
					num4 = i;
					i++;
				}
				num8 += font.CharWidthKern(c, thePrevChar);
				thePrevChar = c;
				if (flag)
				{
					flag = false;
					num6++;
					int num10;
					if (num4 != -1)
					{
						int num9 = y + num2 + (int)this.mTransY;
						if (num9 >= this.mClipRect.mY && num9 < this.mClipRect.mY + this.mClipRect.mHeight + lineSpacing)
						{
							GlobalMembersGraphics.WriteWordWrappedHelper(this, theLine, x + num7, y + num2, -1, -1, true, num3, num4 - num3, num, length);
						}
						num10 = num8 + num7;
						if (num10 < 0)
						{
							break;
						}
						i = num4 + 1;
						if (c != '\n')
						{
							while (i < theLine.Length && theLine[i] == ' ')
							{
								i++;
							}
						}
					}
					else
					{
						if (i < num3 + 1)
						{
							i++;
						}
						num10 = GlobalMembersGraphics.WriteWordWrappedHelper(this, theLine, x + num7, y + num2, -1, -1, true, num3, i - num3, num, length);
						if (num10 < 0)
						{
							break;
						}
					}
					if (num10 > num5)
					{
						num5 = num10;
					}
					num3 = i;
					num4 = -1;
					num8 = 0;
					thePrevChar = '\0';
					num7 = 0;
					num2 += lineSpacing;
				}
				else
				{
					i++;
				}
			}
			if (num3 < theLine.Length)
			{
				int num11 = GlobalMembersGraphics.WriteWordWrappedHelper(this, theLine, x + num7, y + num2, -1, -1, true, num3, theLine.Length - num3, num, length);
				if (num11 >= 0)
				{
					if (num11 > num5)
					{
					}
					num2 += lineSpacing;
				}
			}
			else if (c == '\n')
			{
				num2 += lineSpacing;
			}
			this.SetColor(color);
			return num2 + font.GetDescent() - lineSpacing;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00011EE8 File Offset: 0x000100E8
		public int WriteWordWrapped(Rect theRect, string theLine, int theLineSpacing, int theJustification, ref int theMaxWidth, int theMaxChars, ref int theLastWidth, ref int theLineCount, bool drawString)
		{
			SexyColor color = this.GetColor();
			int num = color.ToInt();
			if (((long)num & (long)((-16777216))) == (long)((-16777216)))
			{
				num &= 16777215;
			}
			if (theMaxChars < 0)
			{
				theMaxChars = theLine.Length;
			}
			Font font = this.GetFont();
			int num2 = font.GetAscent() - font.GetAscentPadding();
			if (theLineSpacing == -1)
			{
				theLineSpacing = font.GetLineSpacing();
			}
			int i = 0;
			int num3 = 0;
			char c = '\0';
			char thePrevChar = '\0';
			int num4 = -1;
			int num5 = 0;
			int num6 = 0;
			int num7 = theLastWidth;
			int num8 = num7;
			while (i < theLine.Length)
			{
				c = theLine[i];
				if (c == '^' && this.mWriteColoredString)
				{
					if (i + 1 < theLine.Length)
					{
						if (theLine[i + 1] != '^')
						{
							i += 8;
							continue;
						}
						i++;
					}
				}
				else if (c == ' ')
				{
					num4 = i;
				}
				else if (c == '\n')
				{
					num8 = theRect.mWidth + 1;
					num4 = i;
					i++;
				}
				num8 += font.CharWidthKern(c, thePrevChar);
				thePrevChar = c;
				if (num8 > theRect.mWidth)
				{
					num6++;
					int num10;
					if (num4 != -1)
					{
						int num9 = theRect.mY + num2 + (int)this.mTransY;
						if (num9 >= this.mClipRect.mY && num9 < this.mClipRect.mY + this.mClipRect.mHeight + theLineSpacing)
						{
							GlobalMembersGraphics.WriteWordWrappedHelper(this, theLine, theRect.mX + num7, theRect.mY + num2, theRect.mWidth, theJustification, drawString, num3, num4 - num3, num, theMaxChars);
						}
						num10 = num8 + num7;
						if (num10 < 0)
						{
							break;
						}
						i = num4 + 1;
						if (c != '\n')
						{
							while (i < theLine.Length && theLine[i] == ' ')
							{
								i++;
							}
						}
					}
					else
					{
						if (i < num3 + 1)
						{
							i++;
						}
						num10 = GlobalMembersGraphics.WriteWordWrappedHelper(this, theLine, theRect.mX + num7, theRect.mY + num2, theRect.mWidth, theJustification, drawString, num3, i - num3, num, theMaxChars);
						if (num10 < 0)
						{
							break;
						}
						if (num10 > theMaxWidth)
						{
							theMaxWidth = num10;
						}
						theLastWidth = num10;
					}
					if (num10 > num5)
					{
						num5 = num10;
					}
					num3 = i;
					num4 = -1;
					num8 = 0;
					thePrevChar = '\0';
					num7 = 0;
					num2 += theLineSpacing;
				}
				else
				{
					i++;
				}
			}
			if (num3 < theLine.Length)
			{
				int num11 = GlobalMembersGraphics.WriteWordWrappedHelper(this, theLine, theRect.mX + num7, theRect.mY + num2, theRect.mWidth, theJustification, drawString, num3, theLine.Length - num3, num, theMaxChars);
				if (num11 >= 0)
				{
					if (num11 > num5)
					{
						num5 = num11;
					}
					if (num11 > theMaxWidth)
					{
						theMaxWidth = num11;
					}
					theLastWidth = num11;
					num2 += theLineSpacing;
				}
			}
			else if (c == '\n')
			{
				num2 += theLineSpacing;
				theLastWidth = 0;
			}
			this.SetColor(color);
			theMaxWidth = num5;
			theLineCount = num6;
			return num2 + font.GetDescent() - theLineSpacing;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x000121D4 File Offset: 0x000103D4
		public int WriteWordWrapped(Rect theRect, string theLine, int theLineSpacing, int theJustification, ref int theMaxWidth, int theMaxChars, ref int theLastWidth, ref int theLineCount)
		{
			return this.WriteWordWrapped(theRect, theLine, theLineSpacing, theJustification, ref theMaxWidth, theMaxChars, ref theLastWidth, ref theLineCount, true);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x000121F8 File Offset: 0x000103F8
		public int WriteWordWrapped(Rect theRect, string theLine, int theLineSpacing, int theJustification, ref int theMaxWidth, int theMaxChars, ref int theLastWidth)
		{
			int num = 0;
			return this.WriteWordWrapped(theRect, theLine, theLineSpacing, theJustification, ref theMaxWidth, theMaxChars, ref theLastWidth, ref num, true);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001221C File Offset: 0x0001041C
		public int WriteWordWrapped(Rect theRect, string theLine, int theLineSpacing, int theJustification, ref int theMaxWidth, int theMaxChars)
		{
			int num = 0;
			int num2 = 0;
			return this.WriteWordWrapped(theRect, theLine, theLineSpacing, theJustification, ref theMaxWidth, theMaxChars, ref num2, ref num, true);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00012244 File Offset: 0x00010444
		public int WriteWordWrapped(Rect theRect, string theLine, int theLineSpacing, int theJustification, ref int theMaxWidth)
		{
			int num = 0;
			int num2 = 0;
			int theMaxChars = -1;
			return this.WriteWordWrapped(theRect, theLine, theLineSpacing, theJustification, ref theMaxWidth, theMaxChars, ref num2, ref num, true);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001226C File Offset: 0x0001046C
		public int WriteWordWrapped(Rect theRect, string theLine, int theLineSpacing, int theJustification)
		{
			int num = 0;
			int num2 = 0;
			int theMaxChars = -1;
			int num3 = 0;
			return this.WriteWordWrapped(theRect, theLine, theLineSpacing, theJustification, ref num3, theMaxChars, ref num2, ref num, true);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00012294 File Offset: 0x00010494
		public int WriteWordWrapped(Rect theRect, string theLine, int theLineSpacing)
		{
			int num = 0;
			int num2 = 0;
			int theMaxChars = -1;
			int num3 = 0;
			int theJustification = -1;
			return this.WriteWordWrapped(theRect, theLine, theLineSpacing, theJustification, ref num3, theMaxChars, ref num2, ref num, true);
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x000122C0 File Offset: 0x000104C0
		public int WriteWordWrapped(Rect theRect, string theLine)
		{
			int num = 0;
			int num2 = 0;
			int theMaxChars = -1;
			int num3 = 0;
			int theJustification = -1;
			int theLineSpacing = -1;
			return this.WriteWordWrapped(theRect, theLine, theLineSpacing, theJustification, ref num3, theMaxChars, ref num2, ref num, true);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x000122EF File Offset: 0x000104EF
		public int DrawStringColor(string theLine, int theX, int theY)
		{
			return this.DrawStringColor(theLine, theX, theY, -1);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x000122FC File Offset: 0x000104FC
		public int DrawStringColor(string theLine, int theX, int theY, int theOldColor)
		{
			return this.WriteString(theLine, theX, theY, -1, -1, true, 0, -1, theOldColor);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001231C File Offset: 0x0001051C
		public int DrawStringWordWrapped(string theLine, int theX, int theY, int theWrapWidth, int theLineSpacing, int theJustification, ref int theMaxWidth)
		{
			int num = this.mFont.GetAscent() - this.mFont.GetAscentPadding();
			this.mDestRect.setValue(theX, theY - num, theWrapWidth, 0);
			return this.WriteWordWrapped(this.mDestRect, theLine, theLineSpacing, theJustification, ref theMaxWidth);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00012368 File Offset: 0x00010568
		public int GetWordWrappedHeight(int theWidth, string theLine, int theLineSpacing, ref int theMaxWidth, ref int theLineCount)
		{
			Graphics graphics = new Graphics();
			graphics.SetFont(this.mFont);
			theLineCount = 0;
			int num = 0;
			int theMaxChars = -1;
			theMaxWidth = 0;
			int theJustification = -1;
			this.mDestRect.setValue(0, 0, theWidth, 0);
			return graphics.WriteWordWrapped(this.mDestRect, theLine, theLineSpacing, theJustification, ref theMaxWidth, theMaxChars, ref num, ref theLineCount, false);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x000123BB File Offset: 0x000105BB
		public bool Is3D()
		{
			return this.mIs3D;
		}

		// Token: 0x0400047C RID: 1148
		protected RenderDevice mRenderDevice;

		// Token: 0x0400047D RID: 1149
		protected HRenderContext mRenderContext = new HRenderContext();

		// Token: 0x0400047E RID: 1150
		protected Graphics3D mGraphics3D;

		// Token: 0x0400047F RID: 1151
		public Edge[] mPFActiveEdgeList;

		// Token: 0x04000480 RID: 1152
		public int mPFNumActiveEdges;

		// Token: 0x04000481 RID: 1153
		public new SexyPoint[] mPFPoints;

		// Token: 0x04000482 RID: 1154
		public int mPFNumVertices;

		// Token: 0x04000483 RID: 1155
		public Stack<GraphicsState> mStateStack = new Stack<GraphicsState>();

		// Token: 0x04000484 RID: 1156
		protected StringBuilder mStringBuilder = new StringBuilder("");

		// Token: 0x020000B1 RID: 177
		public enum DrawMode
		{
			// Token: 0x04000486 RID: 1158
			Normal,
			// Token: 0x04000487 RID: 1159
			Additive
		}
	}
}
