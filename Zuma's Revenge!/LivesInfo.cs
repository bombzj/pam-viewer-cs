using System;
using JeffLib;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x02000109 RID: 265
	public class LivesInfo : IDisposable
	{
		// Token: 0x06000E10 RID: 3600 RVA: 0x0008EFD8 File Offset: 0x0008D1D8
		public LivesInfo(Board board, int theLivesDelta)
		{
			this.mFont = Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_BASE);
			this.mLivesDelta = theLivesDelta;
			this.mDisplayTime = 1200UL;
			this.mDisplayStart = ulong.MaxValue;
			this.mWaitTime = 150UL;
			this.mLivesCount = board.GetNumLives() - 1;
			this.mSlideVal.SetConstant(0.0);
			this.mSlideVal.mAppUpdateCountSrc = board.mUpdateCnt;
			this.InitLayout();
			this.StartSliding(LivesInfo.SLIDE_STATE.SLIDE_ON, 0);
			SoundAttribs soundAttribs = new SoundAttribs();
			soundAttribs.delay = 50;
			GameApp.gApp.mSoundPlayer.Play(Res.GetSoundByID(ResID.SOUND_NEW_EXTRA_LIFE), soundAttribs);
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0008F0B6 File Offset: 0x0008D2B6
		public virtual void Dispose()
		{
			if (this.mLivesText.mImage != null)
			{
				this.mLivesText.mImage.Dispose();
			}
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0008F0D8 File Offset: 0x0008D2D8
		public void Draw(Graphics g)
		{
			this.mFrame.mX = (this.mInset.mX = (int)((float)this.mFrame.mX - g.mTransX));
			this.DrawPlank(g);
			this.DrawLivesCount(g);
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0008F120 File Offset: 0x0008D320
		public void Update()
		{
			switch (this.mSlideState)
			{
			case LivesInfo.SLIDE_STATE.SLIDE_ON:
				this.DisplayOldCount();
				break;
			case LivesInfo.SLIDE_STATE.SLIDE_ONSCREEN:
				this.SlideOff();
				break;
			case LivesInfo.SLIDE_STATE.SLIDE_OFF:
				if (!this.IsSliding())
				{
					this.mSlideState = LivesInfo.SLIDE_STATE.SLIDE_OFFSCREEN;
				}
				break;
			case LivesInfo.SLIDE_STATE.SLIDE_WAIT:
				this.DisplayCount();
				break;
			}
			if (this.mLivesText.mImage != null)
			{
				this.mLivesText.Update();
			}
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0008F18F File Offset: 0x0008D38F
		public bool IsDone()
		{
			return this.mSlideState == LivesInfo.SLIDE_STATE.SLIDE_OFFSCREEN;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0008F19C File Offset: 0x0008D39C
		private void InitLayout()
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_LIVESFRAME);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_FROG_LIVES);
			Image imageByID3 = Res.GetImageByID(ResID.IMAGE_UI_POLE);
			this.mXOffset = imageByID3.GetWidth();
			int height = imageByID.GetHeight();
			int theX = GameApp.gApp.GetScreenRect().mX - GameApp.gApp.mWideScreenXOffset;
			int theY = GameApp.gApp.GetScreenRect().mHeight - height;
			int num = (int)((float)height * 0.13f);
			this.mTextXOffset = imageByID2.GetWidth() + ZumasRevenge.Common._S(50);
			this.mFrame = new Rect(theX, theY, 0, height);
			this.mFrame.mWidth = this.mTextXOffset + this.mFont.StringWidth("x 00");
			this.mFrame.mX = this.mFrame.mX - this.mFrame.mWidth;
			this.mInset = this.mFrame;
			this.mInset.mWidth = this.mInset.mWidth - num;
			this.mInset.mHeight = this.mInset.mHeight - num;
			this.mInset.mY = this.mInset.mY + num;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0008F2C4 File Offset: 0x0008D4C4
		private void StartSliding(LivesInfo.SLIDE_STATE inSlideState, int inXPos)
		{
			this.mSlideState = inSlideState;
			this.mXStart = (float)(this.mFrame.mX + this.mXOffset);
			this.mXEnd = (float)(GameApp.gApp.GetScreenRect().mX + inXPos + this.mXOffset);
			this.mSlideVal.SetCurve(ZumasRevenge.Common._MP("b70,1,0.04,1,#     $P    }~"));
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0008F328 File Offset: 0x0008D528
		private void DrawPlank(Graphics g)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_FROG_LIVES);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_LIVESFRAME);
			int mX = this.mInset.mX;
			int theY = (int)((float)this.mInset.mY + (float)(this.mInset.mHeight - imageByID.GetHeight()) * 0.5f);
			g.DrawImageBox(this.mFrame, imageByID2);
			g.DrawImage(imageByID, mX, theY);
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0008F398 File Offset: 0x0008D598
		private void DrawLivesCount(Graphics g)
		{
			if (this.mSlideState != LivesInfo.SLIDE_STATE.SLIDE_ONSCREEN)
			{
				int num = this.CapAt99((this.mSlideState == LivesInfo.SLIDE_STATE.SLIDE_ON || this.mSlideState == LivesInfo.SLIDE_STATE.SLIDE_WAIT) ? (this.mLivesCount - this.mLivesDelta) : this.mLivesCount);
				string theString = "x  " + num;
				g.SetFont(this.mFont);
				g.SetColor(SexyColor.White);
				g.WriteString(theString, this.mInset.mX + this.mTextXOffset, this.mInset.mY + this.mFont.GetHeight());
				return;
			}
			if (this.mLivesText.mImage != null)
			{
				this.mLivesText.Draw(g);
			}
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0008F458 File Offset: 0x0008D658
		private void DisplayOldCount()
		{
			if (this.IsSliding())
			{
				return;
			}
			this.mSlideState = LivesInfo.SLIDE_STATE.SLIDE_WAIT;
			this.mDisplayStart = (ulong)SexyFramework.Common.SexyTime();
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0008F478 File Offset: 0x0008D678
		private void DisplayCount()
		{
			if ((ulong)SexyFramework.Common.SexyTime() - this.mDisplayStart < this.mWaitTime)
			{
				return;
			}
			this.mSlideState = LivesInfo.SLIDE_STATE.SLIDE_ONSCREEN;
			this.mDisplayStart = (ulong)SexyFramework.Common.SexyTime();
			this.InitLivesText();
			this.PreDrawLivesText(this.CapAt99(this.mLivesCount));
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0008F4C6 File Offset: 0x0008D6C6
		private void SlideOff()
		{
			if ((ulong)SexyFramework.Common.SexyTime() - this.mDisplayStart < this.mDisplayTime)
			{
				return;
			}
			this.mLivesText.mImage = null;
			this.StartSliding(LivesInfo.SLIDE_STATE.SLIDE_OFF, -this.mFrame.mWidth);
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0008F500 File Offset: 0x0008D700
		private bool IsSliding()
		{
			if (this.mSlideVal.IsDoingCurve())
			{
				float num = (float)this.mSlideVal.GetOutVal() * (this.mXEnd - this.mXStart);
				this.mFrame.mX = (this.mInset.mX = (int)(this.mXStart + num));
				return true;
			}
			return false;
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0008F55B File Offset: 0x0008D75B
		private int CapAt99(int inLivesCount)
		{
			if (inLivesCount < 0)
			{
				return 0;
			}
			if (inLivesCount > 99)
			{
				return 99;
			}
			return inLivesCount;
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0008F56C File Offset: 0x0008D76C
		private void InitLivesText()
		{
			int theWidth = this.mFont.StringWidth("x 00") + ZumasRevenge.Common._S(20);
			int theHeight = this.mFont.GetHeight() + ZumasRevenge.Common._S(10);
			this.mLivesText = new FwooshImage();
			this.mLivesText.mAlphaDec = 0f;
			this.mLivesText.mImage = new DeviceImage();
			this.mLivesText.mImage.mApp = GameApp.gApp;
			this.mLivesText.mImage.SetImageMode(true, true);
			this.mLivesText.mImage.AddImageFlags(16U);
			this.mLivesText.mImage.Create(theWidth, theHeight);
			this.mLivesText.mX = this.mInset.mX + this.mTextXOffset;
			this.mLivesText.mY = this.mInset.mY + (int)((float)this.mInset.mHeight * 0.5f) + ZumasRevenge.Common._S(5);
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0008F66C File Offset: 0x0008D86C
		private void PreDrawLivesText(int inLivesCount)
		{
			Graphics graphics = new Graphics(this.mLivesText.mImage);
			graphics.Get3D().ClearColorBuffer(new SexyColor(0, 0, 0, 0));
			graphics.SetFont(this.mFont);
			graphics.SetColor(SexyColor.White);
			graphics.WriteString("x " + inLivesCount + " ", 0, this.mFont.GetAscent(), this.mLivesText.mImage.GetWidth());
			graphics.ClearRenderContext();
		}

		// Token: 0x04000D21 RID: 3361
		private Font mFont;

		// Token: 0x04000D22 RID: 3362
		private Rect mFrame = default(Rect);

		// Token: 0x04000D23 RID: 3363
		private Rect mInset = default(Rect);

		// Token: 0x04000D24 RID: 3364
		private int mTextXOffset;

		// Token: 0x04000D25 RID: 3365
		private int mXOffset;

		// Token: 0x04000D26 RID: 3366
		private FwooshImage mLivesText = new FwooshImage();

		// Token: 0x04000D27 RID: 3367
		private int mLivesCount;

		// Token: 0x04000D28 RID: 3368
		private int mLivesDelta;

		// Token: 0x04000D29 RID: 3369
		private float mXStart;

		// Token: 0x04000D2A RID: 3370
		private float mXEnd;

		// Token: 0x04000D2B RID: 3371
		private ulong mDisplayTime;

		// Token: 0x04000D2C RID: 3372
		private ulong mDisplayStart;

		// Token: 0x04000D2D RID: 3373
		private ulong mWaitTime;

		// Token: 0x04000D2E RID: 3374
		private LivesInfo.SLIDE_STATE mSlideState;

		// Token: 0x04000D2F RID: 3375
		private CurvedVal mSlideVal = new CurvedVal();

		// Token: 0x0200010A RID: 266
		private enum SLIDE_STATE
		{
			// Token: 0x04000D31 RID: 3377
			SLIDE_ON,
			// Token: 0x04000D32 RID: 3378
			SLIDE_ONSCREEN,
			// Token: 0x04000D33 RID: 3379
			SLIDE_OFF,
			// Token: 0x04000D34 RID: 3380
			SLIDE_OFFSCREEN,
			// Token: 0x04000D35 RID: 3381
			SLIDE_WAIT,
			// Token: 0x04000D36 RID: 3382
			NUM_SLIDE_STATES
		}
	}
}
