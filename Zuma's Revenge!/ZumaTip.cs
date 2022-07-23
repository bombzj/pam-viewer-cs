using System;
using System.Collections.Generic;
using JeffLib;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x02000154 RID: 340
	public class ZumaTip
	{
		// Token: 0x0600107E RID: 4222 RVA: 0x000A79EC File Offset: 0x000A5BEC
		public ZumaTip(string text, int width, int height, Rect cutout_region, int id)
		{
			this.mCutoutX = cutout_region.mX;
			this.mCutoutY = cutout_region.mY;
			this.mCutoutW = cutout_region.mWidth;
			this.mCutoutH = cutout_region.mHeight;
			this.mText = text;
			this.mId = id;
			this.mWidth = width + ZumasRevenge.Common._DS(100);
			this.mHeight = height + ZumasRevenge.Common._DS(20);
			if (this.mCutoutX < 0 && id != ZumaProfile.FRUIT_HINT)
			{
				this.mCutoutX = 0;
			}
			if (id != ZumaProfile.CHALLENGE_HINT)
			{
				if (id == ZumaProfile.FIRST_SHOT_HINT)
				{
					this.mMaskImage = Res.GetImageByID(ResID.IMAGE_UI_CONE);
					this.mCutoutW = this.mMaskImage.mWidth * 4;
					this.mCutoutH = this.mMaskImage.mHeight * 4;
				}
				else if (id == ZumaProfile.ZUMA_BAR_HINT)
				{
					this.SetZumaBarBoundingBox();
					this.CreateCutoutImage();
				}
				else
				{
					this.mMaskImage = Res.GetImageByID(ResID.IMAGE_UI_CIRCLE);
				}
			}
			int num = 0;
			Graphics graphics = new Graphics();
			graphics.SetFont(Res.GetFontByID(ResID.FONT_MAIN22));
			this.mTextHeight = graphics.GetWordWrappedHeight(this.mWidth - ZumasRevenge.Common._DS(100), this.mText, -1, ref num, ref num);
			CommonGraphics.SetNonMaskedArea(this.mCutoutX, this.mCutoutY, this.mCutoutW, this.mCutoutH, this.mMaskedRects, ZumaTip.MAX_ALPHA);
			if (this.mMaskedRects.Count == 4)
			{
				this.mMaskedRects[0].r.mX = -GameApp.gApp.mBoardOffsetX;
				MaskedRect maskedRect = this.mMaskedRects[0];
				maskedRect.r.mWidth = maskedRect.r.mWidth + GameApp.gApp.mBoardOffsetX;
				return;
			}
			if (this.mMaskedRects.Count == 3)
			{
				this.mMaskedRects.Add(new MaskedRect(new Rect(-GameApp.gApp.mBoardOffsetX, 0, GameApp.gApp.mBoardOffsetX, GlobalMembers.gSexyApp.mScreenBounds.mHeight), ZumaTip.MAX_ALPHA));
			}
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x000A7C34 File Offset: 0x000A5E34
		public virtual void Dispose()
		{
			this.mCutoutImage = null;
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x000A7C40 File Offset: 0x000A5E40
		public void PointAt(int x, int y, int dir)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_ARROW);
			int num = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(175));
			if (dir == 0)
			{
				this.mArrowAngle = 3.1415927f;
				this.mArrowX = x + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(24));
				this.mArrowY = y - imageByID.mHeight / 2;
				this.mBoxRect = new Rect(x + num, y - this.mHeight / 2, this.mWidth, this.mHeight);
				if (this.mBoxRect.mY < 0)
				{
					this.mBoxRect.mY = 0;
					return;
				}
				if (this.mBoxRect.mY + this.mBoxRect.mHeight > GlobalMembers.gSexyApp.mHeight)
				{
					this.mBoxRect.mY = GlobalMembers.gSexyApp.mHeight - this.mBoxRect.mHeight;
					return;
				}
			}
			else if (dir == 1)
			{
				this.mArrowAngle = 0f;
				this.mArrowX = x - imageByID.mWidth - ZumasRevenge.Common._DS(ZumasRevenge.Common._M(24));
				this.mArrowY = y - imageByID.mHeight / 2;
				this.mBoxRect = new Rect(x - num - this.mWidth, y - this.mHeight / 2, this.mWidth, this.mHeight);
				if (this.mBoxRect.mY < 0)
				{
					this.mBoxRect.mY = 0;
					return;
				}
				if (this.mBoxRect.mY + this.mBoxRect.mHeight > GlobalMembers.gSexyApp.mHeight)
				{
					this.mBoxRect.mY = GlobalMembers.gSexyApp.mHeight - this.mBoxRect.mHeight;
					return;
				}
			}
			else if (dir == 2)
			{
				this.mArrowAngle = 1.5707964f;
				this.mArrowX = x - imageByID.mWidth / 2;
				this.mArrowY = y + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(48));
				this.mBoxRect = new Rect(x - this.mWidth / 2, y + num, this.mWidth, this.mHeight);
				if (this.mBoxRect.mX < 0)
				{
					this.mBoxRect.mX = 0;
					return;
				}
				if (this.mBoxRect.mX + this.mBoxRect.mWidth > GlobalMembers.gSexyApp.mWidth)
				{
					this.mBoxRect.mX = GlobalMembers.gSexyApp.mWidth - this.mBoxRect.mWidth;
					return;
				}
			}
			else if (dir == 3)
			{
				this.mArrowAngle = -1.5707964f;
				this.mArrowX = x - imageByID.mWidth / 2;
				this.mArrowY = y - imageByID.mHeight - ZumasRevenge.Common._DS(ZumasRevenge.Common._M(46));
				this.mBoxRect = new Rect(x - this.mWidth / 2, y - num - this.mHeight, this.mWidth, this.mHeight);
				if (this.mBoxRect.mX < 0)
				{
					this.mBoxRect.mX = 0;
					return;
				}
				if (this.mBoxRect.mX + this.mBoxRect.mWidth > GlobalMembers.gSexyApp.mWidth)
				{
					this.mBoxRect.mX = GlobalMembers.gSexyApp.mWidth - this.mBoxRect.mWidth;
				}
			}
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x000A7F74 File Offset: 0x000A6174
		public void AutoPointAt(int x, int y, int region_w, int region_h)
		{
			int num = GlobalMembers.gSexyApp.mWidth - (x + region_w);
			int num2 = GlobalMembers.gSexyApp.mHeight - (y + region_h);
			int[] array = new int[] { num, x, num2, y };
			int num3 = 0;
			for (int i = 1; i < 4; i++)
			{
				if (array[i] > array[num3])
				{
					num3 = i;
				}
			}
			if (num3 == 0)
			{
				this.PointAt(x + region_w, y + region_h / 2, num3);
				return;
			}
			if (num3 == 1)
			{
				this.PointAt(x, y + region_h / 2, num3);
				return;
			}
			if (num3 == 2)
			{
				this.PointAt(x + region_w / 2, y + region_h, num3);
				return;
			}
			if (num3 == 3)
			{
				this.PointAt(x + region_w / 2, y, num3);
			}
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x000A803C File Offset: 0x000A623C
		public void AutoPointAtCutoutRegion()
		{
			this.AutoPointAt(this.mCutoutX, this.mCutoutY, this.mCutoutW, this.mCutoutH);
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x000A805C File Offset: 0x000A625C
		public void Draw(Graphics g)
		{
			if (this.mUpdateCount < this.mAppearDelay)
			{
				return;
			}
			if (this.mCutoutImage != null)
			{
				g.DrawImage(this.mCutoutImage, this.mCutoutX, this.mCutoutY);
			}
			else if (this.mMaskImage != null)
			{
				g.DrawImage(this.mMaskImage, this.mCutoutX, this.mCutoutY, this.mCutoutW, this.mCutoutH);
			}
			if (this.mMaskImage != null || this.mCutoutImage != null)
			{
				if (this.mCutoutX >= 0)
				{
					ZumasRevenge.Common._S(80);
				}
				else
				{
					ZumasRevenge.Common._S(80);
				}
				g.SetColor(0, 0, 0, ZumaTip.MAX_ALPHA);
				for (int i = 0; i < this.mMaskedRects.size<MaskedRect>(); i++)
				{
					g.FillRect(this.mMaskedRects[i].r);
				}
			}
			ZumasRevenge.Common.DrawCommonDialogBacking(g, this.mBoxRect.mX, this.mBoxRect.mY, this.mBoxRect.mWidth, this.mBoxRect.mHeight);
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_ARROW);
			if (this.mDrawArrow)
			{
				g.DrawImageRotated(imageByID, this.mArrowX, (int)((float)this.mArrowY + this.mArrowYOff), (double)this.mArrowAngle);
				if (this.mDoArrowAnim)
				{
					g.PushState();
					g.SetColorizeImages(true);
					g.SetDrawMode(1);
					g.SetColor(255, 255, 255, (int)this.mArrowAlpha);
					g.DrawImageRotated(imageByID, this.mArrowX, (int)((float)this.mArrowY + this.mArrowYOff), (double)this.mArrowAngle);
					g.PopState();
					if (this.mId == ZumaProfile.FIRST_SHOT_HINT)
					{
						g.SetFont(Res.GetFontByID(ResID.FONT_SHAGLOUNGE45_GAUNTLET));
						g.SetColor(255, 253, 99);
						g.DrawString(TextManager.getInstance().getString(824), ZumasRevenge.Common._DS(ZumasRevenge.Common._M(140)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(540)));
					}
				}
			}
			g.SetColor(255, 220, 135);
			g.SetFont(Res.GetFontByID(ResID.FONT_MAIN22));
			int num = ZumasRevenge.Common._M(50);
			int num2 = ZumasRevenge.Common._M(0);
			num = ZumasRevenge.Common._DS(num);
			num2 = ZumasRevenge.Common._DS(num2);
			Rect theRect = new Rect(this.mBoxRect.mX + num, this.mBoxRect.mY + num2, this.mBoxRect.mWidth - num * 2, this.mBoxRect.mHeight - num2 * 2);
			theRect.mY += (theRect.mHeight - this.mTextHeight) / 2;
			g.WriteWordWrapped(theRect, this.mText, -1, 0);
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x000A8308 File Offset: 0x000A6508
		public void Update()
		{
			this.mUpdateCount++;
			if (this.mDoArrowAnim)
			{
				float num = ZumasRevenge.Common._M(10.5f);
				float num2 = ZumasRevenge.Common._M(0.5f);
				float num3 = (float)ZumasRevenge.Common._M(10);
				this.mArrowAlpha += num * (float)this.mArrowAlphaDir;
				if (this.mArrowAlpha >= 255f && this.mArrowAlphaDir == 1)
				{
					this.mArrowAlpha = 255f;
					this.mArrowAlphaDir = -1;
				}
				else if (this.mArrowAlpha <= 0f && this.mArrowAlphaDir == -1)
				{
					this.mArrowAlphaDir = 1;
					this.mArrowAlpha = 0f;
				}
				this.mArrowYOff += num2 * (float)this.mArrowYOffDir;
				if (this.mArrowYOff >= num3 && this.mArrowYOffDir == 1)
				{
					this.mArrowYOff = num3;
					this.mArrowYOffDir = -1;
					return;
				}
				if (this.mArrowYOff <= 0f && this.mArrowYOffDir == -1)
				{
					this.mArrowYOff = 0f;
					this.mArrowYOffDir = 1;
				}
			}
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x000A8414 File Offset: 0x000A6614
		public bool CutoutContainsPoint(int x, int y)
		{
			Rect rect = new Rect(this.mCutoutX, this.mCutoutY, this.mCutoutW, this.mCutoutH);
			return rect.Contains(x, y);
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x000A844C File Offset: 0x000A664C
		private void SetZumaBarBoundingBox()
		{
			GameApp gApp = GameApp.gApp;
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_WOOD);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_RIGHT_MOUTH_UPPER);
			Image imageByID3 = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_LEFT_MOUTH_LOWER);
			int num = (gApp.IsWideScreen() ? 0 : ((int)((float)imageByID.mWidth * 0.05f)));
			int wideScreenAdjusted = gApp.GetWideScreenAdjusted(ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_INGAME_UI_LEFT_MOUTH_LOWER)) + num);
			int wideScreenAdjusted2 = gApp.GetWideScreenAdjusted(ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_INGAME_UI_RIGHT_MOUTH_UPPER)) - num);
			this.mCutoutX = wideScreenAdjusted + ZumasRevenge.Common._DS(25);
			this.mCutoutY = ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_GUI_INGAME_UI_LEFT_MOUTH_LOWER));
			this.mCutoutW = wideScreenAdjusted2 - wideScreenAdjusted + imageByID2.mWidth - ZumasRevenge.Common._DS(50);
			this.mCutoutH = imageByID3.mHeight;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x000A851C File Offset: 0x000A671C
		private void CreateCutoutImage()
		{
			this.mCutoutImage = new DeviceImage();
			this.mCutoutImage.mApp = GameApp.gApp;
			this.mCutoutImage.SetImageMode(true, true);
			this.mCutoutImage.AddImageFlags(16U);
			this.mCutoutImage.Create(this.mCutoutW, this.mCutoutH);
			Graphics graphics = new Graphics(this.mCutoutImage);
			graphics.Get3D().ClearColorBuffer(new SexyColor(0, 0));
			float num = (float)ZumaTip.MAX_ALPHA;
			float num2 = num / (float)ZumaTip.NUM_LINES;
			int num3 = 0;
			while (num > 0f)
			{
				graphics.SetColor(0, 0, 0, (int)num);
				graphics.FillRect(num3, num3, this.mCutoutW - num3 * 2, 1);
				graphics.FillRect(num3, num3 + 1, 1, this.mCutoutH - 1 - num3 * 2);
				graphics.FillRect(num3 + 1, this.mCutoutH - 1 - num3, this.mCutoutW - 1 - num3 * 2, 1);
				graphics.FillRect(this.mCutoutW - 1 - num3, num3 + 1, 1, this.mCutoutH - 2 - num3 * 2);
				num -= num2;
				num3++;
			}
			graphics.ClearRenderContext();
		}

		// Token: 0x04001AC8 RID: 6856
		public static readonly int MAX_ALPHA = 128;

		// Token: 0x04001AC9 RID: 6857
		private static readonly int NUM_LINES = 10;

		// Token: 0x04001ACA RID: 6858
		protected List<MaskedRect> mMaskedRects = new List<MaskedRect>();

		// Token: 0x04001ACB RID: 6859
		protected MemoryImage mCutoutImage;

		// Token: 0x04001ACC RID: 6860
		protected Image mMaskImage;

		// Token: 0x04001ACD RID: 6861
		protected string mText = "";

		// Token: 0x04001ACE RID: 6862
		protected Rect mBoxRect = default(Rect);

		// Token: 0x04001ACF RID: 6863
		protected float mArrowAngle;

		// Token: 0x04001AD0 RID: 6864
		protected int mArrowX;

		// Token: 0x04001AD1 RID: 6865
		protected int mArrowY;

		// Token: 0x04001AD2 RID: 6866
		protected int mTextHeight;

		// Token: 0x04001AD3 RID: 6867
		protected int mWidth;

		// Token: 0x04001AD4 RID: 6868
		protected int mHeight;

		// Token: 0x04001AD5 RID: 6869
		protected int mCutoutX;

		// Token: 0x04001AD6 RID: 6870
		protected int mCutoutY;

		// Token: 0x04001AD7 RID: 6871
		protected int mCutoutW;

		// Token: 0x04001AD8 RID: 6872
		protected int mCutoutH;

		// Token: 0x04001AD9 RID: 6873
		protected float mArrowAlpha;

		// Token: 0x04001ADA RID: 6874
		protected int mArrowAlphaDir = 1;

		// Token: 0x04001ADB RID: 6875
		protected float mArrowYOff;

		// Token: 0x04001ADC RID: 6876
		protected int mArrowYOffDir = 1;

		// Token: 0x04001ADD RID: 6877
		public bool mDoArrowAnim;

		// Token: 0x04001ADE RID: 6878
		public bool mBlockUpdates = true;

		// Token: 0x04001ADF RID: 6879
		public bool mClickDismiss = true;

		// Token: 0x04001AE0 RID: 6880
		public bool mDrawArrow = true;

		// Token: 0x04001AE1 RID: 6881
		public int mId;

		// Token: 0x04001AE2 RID: 6882
		public int mUpdateCount;

		// Token: 0x04001AE3 RID: 6883
		public int mAppearDelay;

		// Token: 0x02000155 RID: 341
		public enum Dir
		{
			// Token: 0x04001AE5 RID: 6885
			Left,
			// Token: 0x04001AE6 RID: 6886
			Right,
			// Token: 0x04001AE7 RID: 6887
			Up,
			// Token: 0x04001AE8 RID: 6888
			Down
		}
	}
}
