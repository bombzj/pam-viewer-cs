using System;
using System.Collections.Generic;
using System.Text;
using JeffLib;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000089 RID: 137
	public class ChallengeHelp : Widget, ButtonListener
	{
		// Token: 0x06000909 RID: 2313 RVA: 0x000504CC File Offset: 0x0004E6CC
		public ChallengeHelp(bool from_help)
		{
			this.mBoard = GameApp.gApp.mBoard;
			int num = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(434));
			int num2 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(80));
			int x = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(518)) - GameApp.gApp.mWideScreenXOffset + GameApp.gApp.GetScreenRect().mX;
			int y = ZumasRevenge.Common._DS(10);
			this.mFromHelp = from_help;
			this.mClip = false;
			this.mHasTransparencies = (this.mHasAlpha = true);
			this.mCutoutImage = new DeviceImage();
			this.mCutoutImage.SetImageMode(true, true);
			this.mCutoutImage.AddImageFlags(16U);
			this.mCutoutImage.Create(num, num2);
			Graphics graphics = new Graphics(this.mCutoutImage);
			graphics.Get3D().ClearColorBuffer(new SexyColor(0, 0));
			float num3 = 128f;
			float num4 = num3 / 10f;
			int num5 = 0;
			while (num3 > 0f)
			{
				graphics.SetColor(new SexyColor(0, 0, 0, (int)num3));
				graphics.FillRect(num5, num5, this.mCutoutImage.mWidth - num5 * 2, 1);
				graphics.FillRect(num5, num5 + 1, 1, this.mCutoutImage.mHeight - 1 - num5 * 2);
				graphics.FillRect(num5 + 1, this.mCutoutImage.mHeight - 1 - num5, this.mCutoutImage.mWidth - 1 - num5 * 2, 1);
				graphics.FillRect(this.mCutoutImage.mWidth - 1 - num5, num5 + 1, 1, this.mCutoutImage.mHeight - 2 - num5 * 2);
				num3 -= num4;
				num5++;
			}
			CommonGraphics.SetNonMaskedArea(x, y, num, num2, this.mMaskedRects, 128);
			this.mPriority = 2147483646;
			this.Resize(0, 0, GameApp.gApp.mWidth, GameApp.gApp.mHeight);
			this.mOKBtn = ZumasRevenge.Common.MakeButton(0, this, from_help ? TextManager.getInstance().getString(483) : TextManager.getInstance().getString(455));
			this.mOKBtn.SetFont(Res.GetFontByID(ResID.FONT_SHAGLOUNGE28_GREEN));
			this.AddWidget(this.mOKBtn);
			int num6 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(254));
			int theHeight = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(125));
			int theY = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(1000));
			this.mOKBtn.Resize((GameApp.gApp.mWidth - num6) / 2, theY, num6, theHeight);
			this.mMultFX = GameApp.gApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_RPI").Duplicate();
			this.mMultFX.mEmitAfterTimeline = true;
			this.mDrawScale = new CurvedVal();
			this.mDrawScale.SetCurve(ZumasRevenge.Common._MP("b+0,2,0.033333,1,####        cY### >P###"));
			this.mClosing = false;
			this.FONT_SHAGLOUNGE28_STROKE = Res.GetFontByID(ResID.FONT_SHAGLOUNGE28_STROKE);
			this.IMAGE_GUI_ARROW_RED = Res.GetImageByID(ResID.IMAGE_GUI_ARROW_RED);
			this.IMAGE_GUI_BARIMAGE = Res.GetImageByID(ResID.IMAGE_GUI_BARIMAGE);
			this.IMAGE_GUI_EQUALIMAGE = Res.GetImageByID(ResID.IMAGE_GUI_EQUALIMAGE);
			this.IMAGE_GUI_BALLIMAGE = Res.GetImageByID(ResID.IMAGE_GUI_BALLIMAGE);
			this.IMAGE_GUI_DIALOG_MARQUE_BOX = Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_MARQUE_BOX);
			this.IMAGE_UI_CHALLENGE_GAUGE_EMPTY = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGE_GAUGE_EMPTY);
			this.IMAGE_UI_CHALLENGE_GAUGE_FILL = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGE_GAUGE_FILL);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0005083F File Offset: 0x0004EA3F
		public override void Dispose()
		{
			this.mMultFX = null;
			this.RemoveAllWidgets(true, true);
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00050850 File Offset: 0x0004EA50
		public override void RemoveAllWidgets(bool doDelete, bool recursive)
		{
			base.RemoveAllWidgets(doDelete, recursive);
			this.mOKBtn = null;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00050864 File Offset: 0x0004EA64
		public override void Update()
		{
			base.Update();
			if (!GameApp.gApp.Is3DAccelerated())
			{
				return;
			}
			if (!this.mDrawScale.HasBeenTriggered())
			{
				this.MarkDirty();
			}
			if (!this.mDrawScale.IncInVal())
			{
				double num = this.mDrawScale;
			}
			this.MarkDirty();
			this.mMultFX.mDrawTransform.LoadIdentity();
			float num2 = GameApp.DownScaleNum(1f);
			this.mMultFX.mDrawTransform.Scale(num2, num2);
			this.mMultFX.mDrawTransform.Translate((float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(988)), (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(470)));
			this.mMultFX.Update();
			if (this.mClosing && this.mDrawScale == 0.0)
			{
				this.mBoard.ChallengeHelpClosed();
			}
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00050954 File Offset: 0x0004EB54
		public override void Draw(Graphics g)
		{
			if (g != null)
			{
				g.Get3D();
			}
			int mWidth = GameApp.gApp.mWidth;
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGEXOTICA38_BLACK_GLOW);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(TextManager.getInstance().getString(410));
			stringBuilder.Append("^d8d8d8^ ");
			stringBuilder.Append(TextManager.getInstance().getString(411));
			stringBuilder.Append("^oldclr^ ");
			stringBuilder.Append(TextManager.getInstance().getString(412));
			stringBuilder.Append("^d8d8d8^ ");
			stringBuilder.Append(TextManager.getInstance().getString(413));
			stringBuilder.Append("^oldclr^ ");
			int num = fontByID.StringWidth(TextManager.getInstance().getString(410)) + fontByID.StringWidth(TextManager.getInstance().getString(411)) + fontByID.StringWidth(TextManager.getInstance().getString(412)) + fontByID.StringWidth(TextManager.getInstance().getString(413)) + fontByID.CharWidth(' ') * 3;
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(TextManager.getInstance().getString(414));
			stringBuilder2.Append("^d8d8d8^ ");
			stringBuilder2.Append(TextManager.getInstance().getString(415));
			stringBuilder2.Append("^oldclr^ ");
			stringBuilder2.Append(TextManager.getInstance().getString(416));
			int num2 = fontByID.StringWidth(TextManager.getInstance().getString(414)) + fontByID.StringWidth(TextManager.getInstance().getString(415)) + fontByID.StringWidth(TextManager.getInstance().getString(416)) + fontByID.CharWidth(' ') * 2;
			StringBuilder stringBuilder3 = new StringBuilder();
			stringBuilder3.Append(TextManager.getInstance().getString(417));
			stringBuilder3.Append("^d8d8d8^ ");
			stringBuilder3.Append(TextManager.getInstance().getString(418));
			stringBuilder3.Append("^oldclr^");
			int num3 = fontByID.StringWidth(TextManager.getInstance().getString(417)) + fontByID.StringWidth(TextManager.getInstance().getString(418)) + fontByID.CharWidth(' ');
			int num4 = ((num > num2) ? num : num2);
			num4 = ((num4 > num3) ? num4 : num3);
			num4 += 40;
			int num5 = ((num4 + 100 < ZumasRevenge.Common._DS(ZumasRevenge.Common._M(1000))) ? ZumasRevenge.Common._DS(ZumasRevenge.Common._M(1000)) : (num4 + 100));
			int height = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(996));
			int x = (mWidth - num5) / 2;
			int num6 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(170));
			int num7 = ((num4 < ZumasRevenge.Common._DS(ZumasRevenge.Common._M(900))) ? ZumasRevenge.Common._DS(ZumasRevenge.Common._M(900)) : num4);
			int num8 = (mWidth - num7) / 2;
			ZumasRevenge.Common.DrawCommonDialogBacking(g, x, num6, num5, height);
			g.SetColorizeImages(true);
			g.SetColor(new SexyColor(255, 255, 255, 200));
			g.DrawImageBox(new Rect(num8, num6 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(168)), num7, ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(200))), this.IMAGE_GUI_DIALOG_MARQUE_BOX);
			g.DrawImageBox(new Rect(num8, num6 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(390)), num7, ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(290))), this.IMAGE_GUI_DIALOG_MARQUE_BOX);
			g.DrawImageBox(new Rect(num8, num6 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(704)), num7, ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(108))), this.IMAGE_GUI_DIALOG_MARQUE_BOX);
			g.SetColorizeImages(false);
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_BASE));
			g.SetColor(new SexyColor(205, 151, 57));
			g.WriteString(TextManager.getInstance().getString(409), 0, num6 - g.GetFont().mHeight / 2 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(190)), GameApp.gApp.mWidth, 0);
			float mTransX = g.mTransX;
			g.mTransX = (float)(GameApp.gApp.mBoardOffsetX + 10);
			int num9 = (int)(-(int)g.mTransX) + 10;
			int num10 = 4;
			ZumasRevenge.Common._DS(ZumasRevenge.Common._M(382));
			ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(420));
			g.SetFont(fontByID);
			g.SetColor(new SexyColor(205, 151, 57));
			g.WriteWordWrapped(new Rect(num8 + num9, num6 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(168)) + num10, num7 - num9 * 2, ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(200)) - num10 * 2), stringBuilder.ToString());
			g.DrawImage(this.IMAGE_GUI_BARIMAGE, ZumasRevenge.Common._DS(ZumasRevenge.Common._M(430)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(436)));
			g.DrawImage(this.IMAGE_GUI_EQUALIMAGE, ZumasRevenge.Common._DS(ZumasRevenge.Common._M(810)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(456)));
			g.DrawImage(this.IMAGE_GUI_BALLIMAGE, ZumasRevenge.Common._DS(ZumasRevenge.Common._M(950)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(434)));
			if (this.mMultFX != null)
			{
				this.mMultFX.Draw(g);
			}
			ZumasRevenge.Common._DS(ZumasRevenge.Common._M(398));
			ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(638));
			g.SetColor(new SexyColor(205, 151, 57));
			g.WriteWordWrapped(new Rect(num8 + num9, num6 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(390)) + num10, num7 - num9 * 2, ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(290)) - num10 * 2), stringBuilder2.ToString());
			int num11 = (mWidth - this.IMAGE_UI_CHALLENGE_GAUGE_EMPTY.mWidth) / 2;
			g.DrawImage(this.IMAGE_UI_CHALLENGE_GAUGE_EMPTY, num11, ZumasRevenge.Common._DS(ZumasRevenge.Common._M(675)));
			g.DrawImage(this.IMAGE_UI_CHALLENGE_GAUGE_FILL, num11, ZumasRevenge.Common._DS(ZumasRevenge.Common._M(675)));
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_STROKE));
			g.SetColor(SexyColor.White);
			g.DrawString("2x", num11 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(105)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(800)));
			ZumasRevenge.Common._DS(ZumasRevenge.Common._M(442));
			ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(944));
			g.SetColor(new SexyColor(205, 151, 57));
			g.SetFont(fontByID);
			g.WriteWordWrapped(new Rect(num8 + num9, num6 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(704)) + num10, num7 - num9 * 2, ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(108)) - num10 * 2), stringBuilder3.ToString());
			g.mTransX = mTransX;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0005106A File Offset: 0x0004F26A
		public virtual void ButtonPress(int id)
		{
			GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BUTTON1));
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00051080 File Offset: 0x0004F280
		public virtual void ButtonDepress(int id)
		{
			this.mDrawScale.SetCurve(ZumasRevenge.Common._MP("b+0,1,0.05,1,~###         ~#A5t"));
			this.mWidgetFlagsMod.mRemoveFlags |= 16;
			this.mClosing = true;
			GameApp.gApp.HideHelp();
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x000510BC File Offset: 0x0004F2BC
		public void PreDraw(Graphics g)
		{
			g.SetDrawMode(1);
			g.DrawImage(this.mCutoutImage, ZumasRevenge.Common._DS(ZumasRevenge.Common._M(400) - 160), ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(0)));
			g.SetDrawMode(0);
			g.SetColor(new SexyColor(0, 0, 0, 128));
			for (int i = 0; i < this.mMaskedRects.Count; i++)
			{
				g.FillRect(this.mMaskedRects[i].r);
			}
			float num = (float)this.mDrawScale;
			if (num > 1f)
			{
			}
			Graphics3D graphics3D = ((g != null) ? g.Get3D() : null);
			if (this.mDrawScale != 1.0 && graphics3D != null)
			{
				SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
				sexyTransform2D.Translate(-g.mTransX - (float)(this.mWidth / 2), -g.mTransY - (float)(this.mHeight / 2));
				sexyTransform2D.Scale((float)this.mDrawScale, (float)this.mDrawScale);
				sexyTransform2D.Translate(g.mTransX + (float)(this.mWidth / 2), g.mTransY + (float)(this.mHeight / 2));
				graphics3D.PushTransform(sexyTransform2D);
			}
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00051214 File Offset: 0x0004F414
		private void DrawBonusBar(Graphics g)
		{
			float num = (float)this.mDrawScale;
			if (num > 1f)
			{
				num = 1f;
			}
			float num2 = num * 255f;
			g.SetFont(this.FONT_SHAGLOUNGE28_STROKE);
			g.SetColor(new SexyColor(255, 0, 0, (int)num2));
			g.DrawString(TextManager.getInstance().getString(419), ZumasRevenge.Common._DS(ZumasRevenge.Common._M(80)) + GameApp.gApp.mBoardOffsetX, (int)((float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(160)) + (float)this.FONT_SHAGLOUNGE28_STROKE.GetHeight() * 0.5f - 10f));
			g.SetColorizeImages(true);
			g.SetColor(new SexyColor(255, 255, 255, (int)num2));
			g.DrawImageRotatedF(this.IMAGE_GUI_ARROW_RED, (float)(ZumasRevenge.Common._DS(ZumasRevenge.Common._M(390) - 160) + GameApp.gApp.mBoardOffsetX), (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(40)), (double)SexyFramework.Common.DegreesToRadians((float)ZumasRevenge.Common._M2(30)));
			g.SetColorizeImages(false);
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0005132C File Offset: 0x0004F52C
		public override void DrawAll(ModalFlags theFlags, Graphics g)
		{
			this.PreDraw(g);
			this.Draw(g);
			if (this.mOKBtn != null)
			{
				g.Translate(this.mOKBtn.mX, this.mOKBtn.mY);
				this.mOKBtn.Draw(g);
				g.Translate(-this.mOKBtn.mX, -this.mOKBtn.mY);
			}
			this.PostDraw(g);
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0005139C File Offset: 0x0004F59C
		public virtual void PostDraw(Graphics g)
		{
			Graphics3D graphics3D = ((g != null) ? g.Get3D() : null);
			if (this.mDrawScale != 1.0 && graphics3D != null)
			{
				graphics3D.PopTransform();
			}
			this.DrawBonusBar(g);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x000513DD File Offset: 0x0004F5DD
		public virtual void ButtonDownTick(int x)
		{
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x000513DF File Offset: 0x0004F5DF
		public virtual void ButtonMouseEnter(int x)
		{
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x000513E1 File Offset: 0x0004F5E1
		public virtual void ButtonMouseLeave(int x)
		{
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x000513E3 File Offset: 0x0004F5E3
		public virtual void ButtonMouseMove(int x, int y, int z)
		{
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x000513E5 File Offset: 0x0004F5E5
		public virtual void ButtonPress(int z, int y)
		{
		}

		// Token: 0x040006FF RID: 1791
		public Board mBoard;

		// Token: 0x04000700 RID: 1792
		public MemoryImage mCutoutImage;

		// Token: 0x04000701 RID: 1793
		public DialogButton mOKBtn;

		// Token: 0x04000702 RID: 1794
		public List<MaskedRect> mMaskedRects = new List<MaskedRect>();

		// Token: 0x04000703 RID: 1795
		public PIEffect mMultFX;

		// Token: 0x04000704 RID: 1796
		public bool mFromHelp;

		// Token: 0x04000705 RID: 1797
		public CurvedVal mDrawScale;

		// Token: 0x04000706 RID: 1798
		public bool mClosing;

		// Token: 0x04000707 RID: 1799
		private Font FONT_SHAGLOUNGE28_STROKE;

		// Token: 0x04000708 RID: 1800
		private Image IMAGE_GUI_ARROW_RED;

		// Token: 0x04000709 RID: 1801
		private Image IMAGE_GUI_BARIMAGE;

		// Token: 0x0400070A RID: 1802
		private Image IMAGE_GUI_EQUALIMAGE;

		// Token: 0x0400070B RID: 1803
		private Image IMAGE_GUI_BALLIMAGE;

		// Token: 0x0400070C RID: 1804
		private Image IMAGE_GUI_DIALOG_MARQUE_BOX;

		// Token: 0x0400070D RID: 1805
		private Image IMAGE_UI_CHALLENGE_GAUGE_EMPTY;

		// Token: 0x0400070E RID: 1806
		private Image IMAGE_UI_CHALLENGE_GAUGE_FILL;
	}
}
