using System;
using System.Collections.Generic;
using System.Linq;
using JeffLib;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000043 RID: 67
	public class ZumaDialog : DialogEx
	{
		// Token: 0x0600063B RID: 1595 RVA: 0x00028D84 File Offset: 0x00026F84
		public ZumaDialog(int id, bool isModal, string header, string lines, string footer, int btn_mode)
			: base(Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BUTTON), Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BUTTON), id, isModal, header, lines, footer, btn_mode)
		{
			this.IMAGE_GUI_D11 = Res.GetImageByID(ResID.IMAGE_GUI_D11);
			this.IMAGE_GUI_D12 = Res.GetImageByID(ResID.IMAGE_GUI_D12);
			this.IMAGE_GUI_D13 = Res.GetImageByID(ResID.IMAGE_GUI_D13);
			this.IMAGE_GUI_D01 = Res.GetImageByID(ResID.IMAGE_GUI_D01);
			this.IMAGE_GUI_D02 = Res.GetImageByID(ResID.IMAGE_GUI_D02);
			this.IMAGE_GUI_D03 = Res.GetImageByID(ResID.IMAGE_GUI_D03);
			this.IMAGE_GUI_D04 = Res.GetImageByID(ResID.IMAGE_GUI_D04);
			this.IMAGE_GUI_D05 = Res.GetImageByID(ResID.IMAGE_GUI_D05);
			this.IMAGE_GUI_D06 = Res.GetImageByID(ResID.IMAGE_GUI_D06);
			this.IMAGE_GUI_D07 = Res.GetImageByID(ResID.IMAGE_GUI_D07);
			this.IMAGE_GUI_D08 = Res.GetImageByID(ResID.IMAGE_GUI_D08);
			this.IMAGE_GUI_D09 = Res.GetImageByID(ResID.IMAGE_GUI_D09);
			this.IMAGE_GUI_D10 = Res.GetImageByID(ResID.IMAGE_GUI_D10);
			this.mMinWidth = this.IMAGE_GUI_D10.mWidth + this.IMAGE_GUI_D12.mWidth + this.IMAGE_GUI_D02.mWidth;
			this.mMinHeight = this.IMAGE_GUI_D12.mHeight + this.IMAGE_GUI_D13.mHeight;
			this.mTargetWidth = this.mMinWidth;
			this.mTargetHeight = this.mMinHeight;
			this.mCenterInitially = true;
			this.mNumWidthSpacers = 0;
			this.mNumHeightSpacers = 0;
			this.mLastFocusWidget = null;
			if (!GameApp.gApp.mResourceManager.IsGroupLoaded("CommonGame") && !GameApp.gApp.mResourceManager.LoadResources("CommonGame"))
			{
				GameApp.gApp.Shutdown();
			}
			this.mAllowDrag = false;
			this.mPriority = 2;
			this.mBackgroundInsets = new Insets(ZumasRevenge.Common._S(ZumasRevenge.Common._M(16)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(61)), ZumasRevenge.Common._S(ZumasRevenge.Common._M2(18)), ZumasRevenge.Common._S(ZumasRevenge.Common._M3(50)));
			this.mContentInsets = new Insets(ZumasRevenge.Common._S(ZumasRevenge.Common._M(14)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(50)), ZumasRevenge.Common._S(ZumasRevenge.Common._M2(14)), ZumasRevenge.Common._S(ZumasRevenge.Common._M3(10)));
			this.mHasAlpha = (this.mHasTransparencies = true);
			this.mDrawScale.SetCurve(ZumasRevenge.Common._MP("b+0,2,0.033333,1,####        cY### >P###"));
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00028FE8 File Offset: 0x000271E8
		~ZumaDialog()
		{
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00029010 File Offset: 0x00027210
		public override void Resize(int x, int y, int w, int h)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_D11);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_GUI_D09);
			int num = Math.Max(0, w - this.mMinWidth);
			int num2 = imageByID.mWidth * 2;
			this.mNumWidthSpacers = ((num % num2 == 0) ? (num / num2) : (num / num2 + 1));
			w = this.mMinWidth + this.mNumWidthSpacers * imageByID.mWidth * 2;
			num = Math.Max(0, h - this.mMinHeight);
			num2 = imageByID2.mHeight;
			this.mNumHeightSpacers = ((num % num2 == 0) ? (num / num2) : (num / num2 + 1));
			h = this.mMinHeight + this.mNumHeightSpacers * imageByID2.mHeight;
			if (this.mCenterInitially)
			{
				x = (GlobalMembers.gSexyApp.mWidth - w) / 2;
				y = (GlobalMembers.gSexyApp.mHeight - h) / 2;
				this.mCenterInitially = false;
			}
			this.mTargetWidth = w;
			this.mTargetHeight = h;
			this.mButtonSidePadding = ZumasRevenge.Common._S(ZumasRevenge.Common._M(30));
			this.mButtonHorzSpacing = ZumasRevenge.Common._S(ZumasRevenge.Common._M(100));
			base.Resize(x, y, w, h);
			this.SizeButtons();
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0002912E File Offset: 0x0002732E
		public override void Update()
		{
			base.Update();
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00029138 File Offset: 0x00027338
		public override void Draw(Graphics g)
		{
			g.ClearClipRect();
			g.PushState();
			g.Translate(-this.mX, -this.mY);
			g.SetColor(0, 0, 0, 130);
			g.FillRect(ZumasRevenge.Common._S(-80), 0, GameApp.gApp.mWidth + ZumasRevenge.Common._S(160), GameApp.gApp.mHeight);
			g.PopState();
			base.Draw(g);
			if (Enumerable.Count<ZumaDialogLine>(this.mCustomLines) > 0)
			{
				this.mDialogLines = "";
				this.mDialogHeader = "";
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BOX_MAINMENU_FRAME_WOOD);
			g.ClearClipRect();
			g.ClipRect(this.IMAGE_GUI_D11.mWidth, this.IMAGE_GUI_D12.mHeight / 2 + 10, this.mWidth - this.IMAGE_GUI_D11.mWidth * 2, this.mHeight - this.IMAGE_GUI_D12.mHeight);
			int i = 0;
			int j = 0;
			bool flag = false;
			while (j <= this.mHeight)
			{
				while (i < this.mWidth)
				{
					if (flag)
					{
						g.DrawImageMirror(imageByID, i, j);
					}
					else
					{
						g.DrawImage(imageByID, i, j);
					}
					i += imageByID.GetWidth();
					flag = !flag;
				}
				i = 0;
				j += imageByID.GetHeight();
			}
			g.ClearClipRect();
			g.ClipRect(0, 0, this.mWidth, this.mHeight + ZumasRevenge.Common._S(10));
			int num = (this.mWidth - this.IMAGE_GUI_D12.mWidth) / 2;
			g.DrawImage(this.IMAGE_GUI_D12, num, ZumasRevenge.Common._S(ZumasRevenge.Common._M(7)));
			g.DrawImage(this.IMAGE_GUI_D13, (this.mWidth - this.IMAGE_GUI_D13.mWidth) / 2, this.mHeight - this.IMAGE_GUI_D13.mHeight + ZumasRevenge.Common._S(ZumasRevenge.Common._M(8)));
			int num2 = this.mHeight - this.IMAGE_GUI_D13.mHeight - ZumasRevenge.Common._S(ZumasRevenge.Common._M(13));
			g.DrawImage(this.IMAGE_GUI_D06, num, num2);
			int num3 = num;
			for (int k = 0; k < this.mNumWidthSpacers; k++)
			{
				num3 -= this.IMAGE_GUI_D11.mWidth;
				g.DrawImage(this.IMAGE_GUI_D11, num3, ZumasRevenge.Common._S(ZumasRevenge.Common._M(54)));
				g.DrawImage(this.IMAGE_GUI_D07, num3, num2 + this.IMAGE_GUI_D06.mHeight - this.IMAGE_GUI_D07.mHeight);
			}
			g.DrawImage(this.IMAGE_GUI_D10, num3 - this.IMAGE_GUI_D10.mWidth, ZumasRevenge.Common._S(ZumasRevenge.Common._M(54)));
			g.DrawImage(this.IMAGE_GUI_D08, num3 - this.IMAGE_GUI_D08.mWidth, num2 + this.IMAGE_GUI_D06.mHeight - this.IMAGE_GUI_D08.mHeight);
			num3 = num + this.IMAGE_GUI_D12.mWidth;
			for (int l = 0; l < this.mNumWidthSpacers; l++)
			{
				g.DrawImage(this.IMAGE_GUI_D01, num3, ZumasRevenge.Common._S(ZumasRevenge.Common._M(54)));
				g.DrawImage(this.IMAGE_GUI_D05, num3, num2 + this.IMAGE_GUI_D06.mHeight - this.IMAGE_GUI_D05.mHeight);
				num3 += this.IMAGE_GUI_D01.mWidth;
			}
			g.DrawImage(this.IMAGE_GUI_D02, num3, ZumasRevenge.Common._S(ZumasRevenge.Common._M(54)));
			g.DrawImage(this.IMAGE_GUI_D04, num3, num2 + this.IMAGE_GUI_D06.mHeight - this.IMAGE_GUI_D04.mHeight);
			int num4 = ZumasRevenge.Common._S(ZumasRevenge.Common._M(54)) + this.IMAGE_GUI_D10.mHeight;
			for (int m = 0; m < this.mNumHeightSpacers; m++)
			{
				g.DrawImage(this.IMAGE_GUI_D09, 0, num4);
				g.DrawImage(this.IMAGE_GUI_D03, this.mWidth - this.IMAGE_GUI_D03.mWidth, num4);
				num4 += this.IMAGE_GUI_D09.mHeight;
			}
			if (Enumerable.Count<ZumaDialogLine>(this.mCustomLines) > 0)
			{
				int num5 = this.mContentInsets.mTop + this.mBackgroundInsets.mTop + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(0));
				for (int n = 0; n < Enumerable.Count<ZumaDialogLine>(this.mCustomLines); n++)
				{
					ZumaDialogLine zumaDialogLine = this.mCustomLines[n];
					num5 += zumaDialogLine.mYPadding;
					g.SetFont(zumaDialogLine.mFont);
					g.SetColor(zumaDialogLine.mColor);
					g.WriteString(zumaDialogLine.mLine, this.mContentInsets.mLeft + this.mBackgroundInsets.mLeft, num5 + zumaDialogLine.mFont.GetAscent(), this.mWidth - this.mContentInsets.mLeft - this.mContentInsets.mRight - this.mBackgroundInsets.mLeft - this.mBackgroundInsets.mRight, 0);
					num5 += zumaDialogLine.mFont.GetHeight();
				}
				return;
			}
			int num6 = this.mContentInsets.mTop + this.mBackgroundInsets.mTop;
			if (this.mDialogHeader.Length > 0)
			{
				num6 += this.mHeaderFont.GetAscent() - this.mHeaderFont.GetAscentPadding();
				g.SetFont(this.mHeaderFont);
				g.SetColor(this.mColors[0]);
				this.WriteCenteredLine(g, num6, this.mDialogHeader);
				num6 += this.mHeaderFont.GetHeight() - this.mHeaderFont.GetAscent();
				num6 += this.mSpaceAfterHeader;
			}
			g.SetFont(this.mLinesFont);
			g.SetColor(this.mColors[1]);
			Rect theRect = new Rect(this.mBackgroundInsets.mLeft + this.mContentInsets.mLeft + 2, num6, this.mWidth - this.mContentInsets.mLeft - this.mContentInsets.mRight - this.mBackgroundInsets.mLeft - this.mBackgroundInsets.mRight - 4, 0);
			num6 += this.WriteWordWrapped(g, theRect, this.mDialogLines, this.mLinesFont.GetLineSpacing() + this.mLineSpacingOffset, this.mTextAlign);
			if (this.mDialogFooter.Length != 0 && this.mButtonMode != 3)
			{
				num6 += 8;
				num6 += this.mHeaderFont.GetLineSpacing();
				g.SetFont(this.mHeaderFont);
				g.SetColor(this.mColors[2]);
				this.WriteCenteredLine(g, num6, this.mDialogFooter);
			}
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x000297C0 File Offset: 0x000279C0
		public override bool IsPointVisible(int x, int y)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_D12);
			int num = (this.mWidth - imageByID.mWidth) / 2;
			return (y >= ZumasRevenge.Common._S(ZumasRevenge.Common._M(54)) && y <= this.mHeight - ZumasRevenge.Common._S(ZumasRevenge.Common._M1(30))) || (x >= num && x <= num + imageByID.mWidth);
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00029820 File Offset: 0x00027A20
		public virtual void GetSize(ref int w, ref int h)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BUTTON);
			int num = this.mContentInsets.mLeft + this.mContentInsets.mRight + this.mBackgroundInsets.mLeft + this.mBackgroundInsets.mRight + 4;
			int num2 = this.mBackgroundInsets.mTop + this.mBackgroundInsets.mBottom + this.mContentInsets.mTop + this.mContentInsets.mBottom + this.mSpaceAfterHeader + imageByID.GetCelHeight() + ZumasRevenge.Common._S(ZumasRevenge.Common._M(40));
			w += num;
			h += num2;
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x000298C1 File Offset: 0x00027AC1
		public override void KeyDown(KeyCode key)
		{
			base.KeyDown(key);
			if (this.mButtonMode == 0)
			{
				return;
			}
			if (key == KeyCode.KEYCODE_ESCAPE)
			{
				this.ButtonDepress(1001);
				return;
			}
			if (key == KeyCode.KEYCODE_RETURN)
			{
				this.ButtonDepress(1000);
			}
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x000298F4 File Offset: 0x00027AF4
		public override void AddedToManager(WidgetManager wm)
		{
			base.AddedToManager(wm);
			this.mLastFocusWidget = wm.mFocusWidget;
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00029909 File Offset: 0x00027B09
		public override void RemovedFromManager(WidgetManager wm)
		{
			base.RemovedFromManager(wm);
			if (this.mLastFocusWidget != wm.mFocusWidget && !GlobalMembers.gSexyApp.mShutdown && this.mLastFocusWidget != null)
			{
				wm.SetFocus(this.mLastFocusWidget);
			}
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00029940 File Offset: 0x00027B40
		public override void MouseDrag(int x, int y)
		{
			if (this.mAllowDrag)
			{
				base.MouseDrag(x, y);
			}
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00029952 File Offset: 0x00027B52
		public override void ButtonPress(int inButtonID)
		{
			base.ButtonPress(inButtonID);
			GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BUTTON1));
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0002996F File Offset: 0x00027B6F
		public void SetFocusWidgetToBoard()
		{
			this.mLastFocusWidget = ((GameApp)GlobalMembers.gSexyApp).GetBoard();
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00029988 File Offset: 0x00027B88
		public void SizeButtons()
		{
			int inWidth = ZumasRevenge.Common._S(ZumasRevenge.Common._M(120));
			if (this.mYesButton != null)
			{
				this.EnsureButtonMeetsWidth(this.mYesButton, inWidth);
				if (this.mNoButton == null)
				{
					int num = ZumasRevenge.Common._S(ZumasRevenge.Common._M(120));
					this.mYesButton.Resize((this.mWidth - num) / 2, this.mHeight - this.mContentInsets.mBottom - this.mBackgroundInsets.mBottom - this.mButtonHeight - ZumasRevenge.Common._S(ZumasRevenge.Common._M(7)), num, this.mButtonHeight);
				}
			}
			if (this.mNoButton != null)
			{
				this.EnsureButtonMeetsWidth(this.mNoButton, inWidth);
			}
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00029A2E File Offset: 0x00027C2E
		public void EnsureButtonMeetsWidth(DialogButton inButton, int inWidth)
		{
			if (inButton.mWidth < inWidth)
			{
				inButton.Resize((int)((float)inButton.mX - (float)(inWidth - inButton.mWidth) * 0.5f), inButton.mY, inWidth, inButton.mHeight);
			}
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00029A64 File Offset: 0x00027C64
		public int GetLeft()
		{
			return this.mX + this.mContentInsets.mLeft + this.mBackgroundInsets.mLeft;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00029A84 File Offset: 0x00027C84
		public int GetTop()
		{
			return this.mY + this.mContentInsets.mTop + this.mBackgroundInsets.mTop + ZumasRevenge.Common._S(54);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00029AAC File Offset: 0x00027CAC
		public int GetWidth()
		{
			return this.mWidth - this.mContentInsets.mLeft - this.mContentInsets.mRight - this.mBackgroundInsets.mLeft - this.mBackgroundInsets.mRight;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00029AE4 File Offset: 0x00027CE4
		public void Kill()
		{
			this.mDrawScale.SetCurve(ZumasRevenge.Common._MP("b+0,1,0.05,1,~###         ~#A5t"));
			this.mWidgetFlagsMod.mRemoveFlags |= 16;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00029B0F File Offset: 0x00027D0F
		internal void WaitForResult()
		{
		}

		// Token: 0x0400035B RID: 859
		public bool mCenterInitially;

		// Token: 0x0400035C RID: 860
		public bool mAllowDrag;

		// Token: 0x0400035D RID: 861
		public List<ZumaDialogLine> mCustomLines = new List<ZumaDialogLine>();

		// Token: 0x0400035E RID: 862
		protected int mMinWidth;

		// Token: 0x0400035F RID: 863
		protected int mMinHeight;

		// Token: 0x04000360 RID: 864
		protected int mTargetWidth;

		// Token: 0x04000361 RID: 865
		protected int mTargetHeight;

		// Token: 0x04000362 RID: 866
		protected int mNumWidthSpacers;

		// Token: 0x04000363 RID: 867
		protected int mNumHeightSpacers;

		// Token: 0x04000364 RID: 868
		protected Widget mLastFocusWidget;

		// Token: 0x04000365 RID: 869
		private Image IMAGE_GUI_D11;

		// Token: 0x04000366 RID: 870
		private Image IMAGE_GUI_D12;

		// Token: 0x04000367 RID: 871
		private Image IMAGE_GUI_D13;

		// Token: 0x04000368 RID: 872
		private Image IMAGE_GUI_D01;

		// Token: 0x04000369 RID: 873
		private Image IMAGE_GUI_D02;

		// Token: 0x0400036A RID: 874
		private Image IMAGE_GUI_D03;

		// Token: 0x0400036B RID: 875
		private Image IMAGE_GUI_D04;

		// Token: 0x0400036C RID: 876
		private Image IMAGE_GUI_D05;

		// Token: 0x0400036D RID: 877
		private Image IMAGE_GUI_D06;

		// Token: 0x0400036E RID: 878
		private Image IMAGE_GUI_D07;

		// Token: 0x0400036F RID: 879
		private Image IMAGE_GUI_D08;

		// Token: 0x04000370 RID: 880
		private Image IMAGE_GUI_D09;

		// Token: 0x04000371 RID: 881
		private Image IMAGE_GUI_D10;
	}
}
