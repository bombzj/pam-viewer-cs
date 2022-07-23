using System;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000069 RID: 105
	public class NotificationWidget : Widget
	{
		// Token: 0x0600073F RID: 1855 RVA: 0x00030DA4 File Offset: 0x0002EFA4
		public NotificationWidget(Board theBoard, string theStringInfo)
		{
			this.mBoard = theBoard;
			this.mDisplayTime = 2000UL;
			this.mDisplayStart = ulong.MaxValue;
			this.mSoundID = -1;
			this.mSlideState = NotificationWidget.SLIDE_STATE.SLIDE_ON;
			this.mIsFinished = false;
			this.mSlideVal.mAppUpdateCountSrc = this.mBoard.mUpdateCnt;
			this.mSlideVal.SetCurve(Common._MP("b70,1,0.02,1,#     $P    }~"));
			this.mFont = Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_YELLOW);
			this.mNotification = theStringInfo;
			this.mNotificationStringWidth = this.mFont.StringWidth(theStringInfo);
			int num = Common._DS(600);
			int num2 = Common._DS(100);
			int num3 = ((this.mNotificationStringWidth + num2 > num) ? (this.mNotificationStringWidth + num2) : num);
			int num4 = Common._DS(150);
			this.mYStart = (float)(GameApp.gApp.GetScreenRect().mHeight + num4);
			this.mYEnd = (float)(GameApp.gApp.GetScreenRect().mHeight - num4 / 2);
			this.Resize(GameApp.gApp.GetScreenRect().mX + (GameApp.gApp.GetScreenRect().mWidth - num3) / 2, (int)this.mYStart, num3, num4);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00030EDC File Offset: 0x0002F0DC
		public override void Draw(Graphics g)
		{
			Common.DrawCommonDialogBacking(g, 0, 0, this.mWidth, this.mHeight * 2);
			g.SetFont(this.mFont);
			g.SetColor(SexyColor.White);
			if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CHT)
			{
				g.DrawString(this.mNotification, (this.mWidth - this.mNotificationStringWidth) / 2, Common._DS(60) + 3);
				return;
			}
			g.DrawString(this.mNotification, (this.mWidth - this.mNotificationStringWidth) / 2, Common._DS(60));
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00030F70 File Offset: 0x0002F170
		public override void Update()
		{
			switch (this.mSlideState)
			{
			case NotificationWidget.SLIDE_STATE.SLIDE_ON:
				if (this.mSlideVal.IsDoingCurve())
				{
					float num = (float)this.mSlideVal.GetOutVal() * (this.mYEnd - this.mYStart);
					this.Move(this.mX, (int)(this.mYStart + num));
					return;
				}
				this.PlaySound();
				this.mSlideState = NotificationWidget.SLIDE_STATE.SLIDE_ONSCREEN;
				this.mDisplayStart = (ulong)SexyFramework.Common.SexyTime();
				return;
			case NotificationWidget.SLIDE_STATE.SLIDE_ONSCREEN:
			{
				ulong num2 = (ulong)SexyFramework.Common.SexyTime();
				if (num2 - this.mDisplayStart >= this.mDisplayTime)
				{
					this.mSlideVal.SetCurve(Common._MP("b70,1,0.02,1,#     $P    }~"));
					this.mSlideState = NotificationWidget.SLIDE_STATE.SLIDE_OFF;
					this.mYStart = (float)this.mY;
					this.mYEnd = (float)(GameApp.gApp.GetScreenRect().mHeight + this.mHeight);
					return;
				}
				break;
			}
			case NotificationWidget.SLIDE_STATE.SLIDE_OFF:
				if (this.mSlideVal.IsDoingCurve())
				{
					float num3 = (float)this.mSlideVal.GetOutVal() * (this.mYEnd - this.mYStart);
					this.Move(this.mX, (int)(this.mYStart + num3));
					return;
				}
				this.mSlideState = NotificationWidget.SLIDE_STATE.SLIDE_OFFSCREEN;
				return;
			case NotificationWidget.SLIDE_STATE.SLIDE_OFFSCREEN:
				this.mIsFinished = true;
				break;
			default:
				return;
			}
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x000310A0 File Offset: 0x0002F2A0
		public override void MouseDown(int x, int y, int theClickCount)
		{
			if (this.mBoard == null)
			{
				return;
			}
			this.mBoard.MouseDown(x, y, theClickCount);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x000310B9 File Offset: 0x0002F2B9
		public override void MouseUp(int x, int y, int theClickCount)
		{
			if (this.mBoard == null)
			{
				return;
			}
			this.mBoard.MouseDown(x, y, theClickCount);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x000310D2 File Offset: 0x0002F2D2
		public override void MouseMove(int x, int y)
		{
			if (this.mBoard == null)
			{
				return;
			}
			this.mBoard.MouseMove(x, y);
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x000310EA File Offset: 0x0002F2EA
		public override void MouseDrag(int x, int y)
		{
			if (this.mBoard == null)
			{
				return;
			}
			this.mBoard.MouseMove(x, y);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00031102 File Offset: 0x0002F302
		public bool IsFinished()
		{
			return this.mIsFinished;
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0003110A File Offset: 0x0002F30A
		private void PlaySound()
		{
			if (this.mSoundID == -1)
			{
				return;
			}
			this.mBoard.mApp.mSoundPlayer.Play(this.mSoundID);
			this.mSoundID = -1;
		}

		// Token: 0x040004A4 RID: 1188
		private Board mBoard;

		// Token: 0x040004A5 RID: 1189
		private float mYStart;

		// Token: 0x040004A6 RID: 1190
		private float mYEnd;

		// Token: 0x040004A7 RID: 1191
		private ulong mDisplayTime;

		// Token: 0x040004A8 RID: 1192
		private ulong mDisplayStart;

		// Token: 0x040004A9 RID: 1193
		private NotificationWidget.SLIDE_STATE mSlideState;

		// Token: 0x040004AA RID: 1194
		private bool mIsFinished;

		// Token: 0x040004AB RID: 1195
		private CurvedVal mSlideVal = new CurvedVal();

		// Token: 0x040004AC RID: 1196
		private string mNotification;

		// Token: 0x040004AD RID: 1197
		private int mNotificationStringWidth;

		// Token: 0x040004AE RID: 1198
		private Font mFont;

		// Token: 0x040004AF RID: 1199
		public int mSoundID;

		// Token: 0x0200006A RID: 106
		private enum SLIDE_STATE
		{
			// Token: 0x040004B1 RID: 1201
			SLIDE_ON,
			// Token: 0x040004B2 RID: 1202
			SLIDE_ONSCREEN,
			// Token: 0x040004B3 RID: 1203
			SLIDE_OFF,
			// Token: 0x040004B4 RID: 1204
			SLIDE_OFFSCREEN,
			// Token: 0x040004B5 RID: 1205
			NUM_SLIDE_STATES
		}
	}
}
