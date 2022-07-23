using System;
using SexyFramework.GraphicsLib;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x0200014D RID: 333
	public class Upsell : Widget, ButtonListener, IDisposable
	{
		// Token: 0x06001042 RID: 4162 RVA: 0x000A6124 File Offset: 0x000A4324
		~Upsell()
		{
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x000A614C File Offset: 0x000A434C
		public override void Dispose()
		{
			base.RemoveAllWidgets(true, false);
			this.mBuyBtn.Dispose();
			this.mMenuBtn.Dispose();
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x000A616C File Offset: 0x000A436C
		public Upsell(bool from_exit)
		{
			this.mClip = false;
			this.mPriority = (this.mZOrder = int.MaxValue);
			Upsell.gZoomStart = Common._M(4f);
			this.mBlock2X = (float)(GameApp.gApp.mWidth + Common._DS(160));
			this.mState = 1;
			this.mZoom = Upsell.gZoomStart;
			this.mMenuBtn = new ButtonWidget(1, this);
			this.mMenuBtn.mDoFinger = true;
			this.mMenuBtn.mNormalRect = this.mMenuBtn.mButtonImage.GetCelRect(0);
			this.mMenuBtn.mOverRect = this.mMenuBtn.mButtonImage.GetCelRect(1);
			this.mMenuBtn.mDownRect = this.mMenuBtn.mButtonImage.GetCelRect(2);
			this.AddWidget(this.mMenuBtn);
			this.mBuyBtn = new ButtonWidget(2, this);
			this.mBuyBtn.mDoFinger = true;
			this.mBuyBtn.mNormalRect = this.mBuyBtn.mButtonImage.GetCelRect(0);
			this.mBuyBtn.mOverRect = this.mBuyBtn.mButtonImage.GetCelRect(1);
			this.mBuyBtn.mDownRect = this.mBuyBtn.mButtonImage.GetCelRect(2);
			this.AddWidget(this.mBuyBtn);
			this.mScreenshotTimer = Upsell.gScreenshotTimer;
			this.mScreenshotIdx = 0;
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x000A62DC File Offset: 0x000A44DC
		public override void Update()
		{
			float num = Common._M(50f);
			if (this.mState == 1)
			{
				this.mUpdateCnt++;
				if (this.mUpdateCnt >= Common._M(25))
				{
					this.mBlock1X += num;
					int num2 = 0;
					if (this.mBlock1X >= (float)num2)
					{
						this.mBlock1X = (float)num2;
						this.mState++;
						this.mUpdateCnt = 0;
					}
				}
			}
			else if (this.mState == 2)
			{
				this.mUpdateCnt++;
				int num3 = 0;
				if (this.mUpdateCnt >= Common._M(25))
				{
					this.mBlock2X -= num;
					if (this.mBlock2X <= (float)num3)
					{
						this.mBlock2X = (float)num3;
						this.mState++;
						this.mUpdateCnt = 0;
					}
				}
			}
			else if (this.mState == 3)
			{
				this.mUpdateCnt++;
				if (this.mUpdateCnt >= Common._M(25))
				{
					int num4 = Common._M(20);
					float num5 = (Upsell.gZoomStart - 1f) / (float)num4;
					this.mZoom -= num5;
					if (this.mZoom <= 1f)
					{
						this.mZoom = 1f;
						this.mState++;
						this.mUpdateCnt = 0;
					}
				}
			}
			else if (this.mState == 4)
			{
				this.mUpdateCnt++;
				if (this.mUpdateCnt >= Common._M(25))
				{
					int num6 = Common._M(15);
					this.mMenuBtn.Move(this.mMenuBtn.mX, this.mMenuBtn.mY - num6);
					this.mBuyBtn.Move(this.mBuyBtn.mX, this.mBuyBtn.mY - num6);
					int num7 = 0;
					int num8 = 0;
					int num9 = 0;
					if (this.mMenuBtn.mY <= num7)
					{
						this.mMenuBtn.mY = num7;
						num9++;
					}
					if (this.mBuyBtn.mY <= num8)
					{
						this.mBuyBtn.mY = num8;
						num9++;
					}
					if (num9 == 2)
					{
						this.mState++;
					}
				}
			}
			else if (this.mState == 5)
			{
				this.mScreenshotTimer--;
				if (this.mScreenshotTimer == 0)
				{
					this.mScreenshotTimer = Upsell.gScreenshotTimer;
					this.mScreenshotIdx = (this.mScreenshotIdx + 1) % Upsell.MAX_SCREENSHOTS;
				}
			}
			if (this.mState < 5 || this.mScreenshotTimer <= Upsell.gScreenshotFade)
			{
				this.MarkDirty();
			}
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x000A6582 File Offset: 0x000A4782
		public override void Draw(Graphics g)
		{
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x000A6584 File Offset: 0x000A4784
		public void ButtonDepress(int id)
		{
			if (id == this.mMenuBtn.mId && this.mFromExit)
			{
				GameApp.gApp.mDoingDRM = false;
				GameApp.gApp.Shutdown();
				return;
			}
			if (id == this.mMenuBtn.mId)
			{
				Board mBoard = GameApp.gApp.mBoard;
				GameApp.gApp.mWidgetManager.RemoveWidget(this);
				GameApp.gApp.mDoingDRM = false;
			}
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x000A65F4 File Offset: 0x000A47F4
		public virtual void ButtonPress(int id)
		{
			if (id == this.mMenuBtn.mId && this.mFromExit)
			{
				GameApp.gApp.mDoingDRM = false;
				GameApp.gApp.Shutdown();
				return;
			}
			if (id == this.mMenuBtn.mId)
			{
				Board mBoard = GameApp.gApp.mBoard;
				GameApp.gApp.mWidgetManager.RemoveWidget(this);
				GameApp.gApp.mDoingDRM = false;
			}
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x000A6661 File Offset: 0x000A4861
		public virtual void ButtonPress(int theId, int theClickCount)
		{
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x000A6663 File Offset: 0x000A4863
		public virtual void ButtonDownTick(int theId)
		{
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x000A6665 File Offset: 0x000A4865
		public virtual void ButtonMouseEnter(int theId)
		{
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x000A6667 File Offset: 0x000A4867
		public virtual void ButtonMouseLeave(int theId)
		{
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x000A6669 File Offset: 0x000A4869
		public virtual void ButtonMouseMove(int theId, int theX, int theY)
		{
		}

		// Token: 0x04001AA3 RID: 6819
		private static float gZoomStart = 4f;

		// Token: 0x04001AA4 RID: 6820
		private static int gScreenshotTimer = 300;

		// Token: 0x04001AA5 RID: 6821
		private static int gScreenshotFade = 50;

		// Token: 0x04001AA6 RID: 6822
		private static readonly int MAX_SCREENSHOTS = 9;

		// Token: 0x04001AA7 RID: 6823
		protected float mBlock1X;

		// Token: 0x04001AA8 RID: 6824
		protected float mBlock2X;

		// Token: 0x04001AA9 RID: 6825
		protected float mZoom;

		// Token: 0x04001AAA RID: 6826
		protected ButtonWidget mMenuBtn;

		// Token: 0x04001AAB RID: 6827
		protected ButtonWidget mBuyBtn;

		// Token: 0x04001AAC RID: 6828
		protected int mState;

		// Token: 0x04001AAD RID: 6829
		protected int mScreenshotIdx;

		// Token: 0x04001AAE RID: 6830
		protected int mScreenshotTimer;

		// Token: 0x04001AAF RID: 6831
		public bool mFromExit;
	}
}
