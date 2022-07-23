using System;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x0200008A RID: 138
	public class Checkpoint : Widget, ButtonListener, IDisposable
	{
		// Token: 0x06000919 RID: 2329 RVA: 0x000513E8 File Offset: 0x0004F5E8
		public Checkpoint(Level l, int score, bool game_over)
		{
			this.mScore = score;
			this.mFromGameOver = game_over;
			this.mDone = false;
			this.mContinuePressed = false;
			this.mPostcardGroupName = "";
			this.mState = 1;
			this.mAlpha = 0f;
			this.mSize = Common._DS(Common._M(8f));
			this.mClip = false;
			this.mShowMap = false;
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARD_BACK);
			this.mPostCardX = (GameApp.gApp.GetScreenRect().mWidth - imageByID.mWidth) / 2 + GameApp.gApp.mWideScreenXOffset;
			this.mPostCardY = (GameApp.gApp.GetScreenRect().mHeight - imageByID.mHeight) / 2;
			if (this.mFromGameOver)
			{
				int num = GameApp.gApp.GetLevelMgr().GetLevelIndex(l.mId);
				if (l.mNum <= 5)
				{
					num -= l.mNum - 1;
				}
				else
				{
					num -= l.mNum - 6;
				}
				Level levelByIndex = GameApp.gApp.GetLevelMgr().GetLevelByIndex(num);
				this.mZone = LevelMgr.GetZoneName(levelByIndex.mZone - 1);
				this.mLevelNum = (levelByIndex.mZone - 1) * 10 + levelByIndex.mNum;
				int num2 = (GameApp.gApp.GetScreenRect().mWidth + Common._S(Common._M(160)) - Res.GetImageByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARDTEXT_BASETEXT).mWidth) / 2;
				int num3 = (GameApp.gApp.GetScreenRect().mHeight - Res.GetImageByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARDTEXT_BASETEXT).mHeight) / 2;
				this.mButtons[0] = new ButtonWidget(0, this);
				this.mButtons[0].mDoFinger = true;
				this.mButtons[0].mButtonImage = Res.GetImageByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARDTEXT_CONTINUE_BUTTON);
				this.mButtons[0].mDownImage = Res.GetImageByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARDTEXT_CONTINUE_BUTTON_CLICK);
				this.mButtons[0].mOverImage = Res.GetImageByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARDTEXT_CONTINUE_BUTTON_CLICK);
				this.mButtons[0].mDisabled = true;
				this.mButtons[0].Resize(this.mPostCardX + Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARDTEXT_CONTINUE_BUTTON)), this.mPostCardY + Common._DS(Res.GetOffsetYByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARDTEXT_CONTINUE_BUTTON)), this.mButtons[0].mOverImage.mWidth, this.mButtons[0].mOverImage.mHeight);
				this.AddWidget(this.mButtons[0]);
				this.mButtons[1] = new ButtonWidget(1, this);
				this.mButtons[1].mDoFinger = true;
				this.mButtons[1].mButtonImage = Res.GetImageByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARDTEXT_MM_BUTTON);
				this.mButtons[1].mDownImage = Res.GetImageByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARDTEXT_MM_BUTTON_CLICK);
				this.mButtons[1].mOverImage = Res.GetImageByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARDTEXT_MM_BUTTON_CLICK);
				this.mButtons[1].mDisabled = true;
				this.mButtons[1].Resize(this.mPostCardX + Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARDTEXT_MM_BUTTON_CLICK)), this.mPostCardY + Common._DS(Res.GetOffsetYByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARDTEXT_MM_BUTTON_CLICK)), this.mButtons[1].mOverImage.mWidth, this.mButtons[1].mOverImage.mHeight);
				this.AddWidget(this.mButtons[1]);
				return;
			}
			this.mZone = LevelMgr.GetZoneName(l.mZone);
			this.mLevelNum = (l.mZone - 1) * 10 + l.mNum;
			if (l.mBoss != null)
			{
				this.mBossName = "\"" + l.mBoss.mName + "\"";
			}
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x000517A6 File Offset: 0x0004F9A6
		public override void Dispose()
		{
			base.RemoveAllWidgets(true, false);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x000517B0 File Offset: 0x0004F9B0
		public override void Update()
		{
			int num = Common._M(75);
			this.mUpdateCnt++;
			if (this.mState == 1)
			{
				float num2 = 128f / (float)num;
				this.mAlpha += num2;
				if (this.mAlpha > 128f)
				{
					this.mAlpha = 128f;
				}
				num2 = Common._M(7f) / (float)num;
				this.mSize -= num2;
				if (this.mSize < 1f)
				{
					this.mSize = 1f;
				}
				if (this.mUpdateCnt >= num)
				{
					if (this.mFromGameOver)
					{
						for (int i = 0; i < 2; i++)
						{
							if (this.mButtons[i] != null)
							{
								this.mButtons[i].mDisabled = false;
							}
						}
					}
					this.mState = 0;
					return;
				}
			}
			else if (this.mState == -1)
			{
				float num3 = 128f / (float)num;
				this.mAlpha -= num3;
				if (this.mAlpha <= 0f)
				{
					this.mAlpha = 0f;
					this.mDone = true;
					this.mState = 0;
				}
			}
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x000518C8 File Offset: 0x0004FAC8
		public override void Draw(Graphics g)
		{
			int num = (int)(this.mAlpha * 2f);
			if (num > 255)
			{
				num = 255;
			}
			else if (num < 0)
			{
				num = 0;
			}
			if (num != 255)
			{
				g.SetColorizeImages(true);
			}
			g.SetColor(255, 255, 255, num);
			GameApp gApp = GameApp.gApp;
			if (!this.mFromGameOver)
			{
				Font fontByID = Res.GetFontByID(ResID.FONT_SHAGLOUNGE45_GAUNTLET);
				Font fontByID2 = Res.GetFontByID(ResID.FONT_SHAGLOUNGE28_STROKE);
				int theX = gApp.GetScreenRect().mWidth / 2;
				int num2 = this.mHeight / 2;
				g.SetFont(fontByID);
				g.SetColor(255, 255, 255, num);
				string @string = TextManager.getInstance().getString(432);
				g.WriteString(@string, 0, num2 - g.GetFont().GetHeight() - Common._S(Common._M(-30)), this.mWidth, 0);
				int num3 = Common._S(Common._M(20));
				g.SetFont(fontByID2);
				g.SetColor(Common._M(240), Common._M1(200), Common._M2(0), num);
				g.WriteString(this.mZone, theX, num2 + Common._S(Common._M(35)), this.mWidth - num3 * 2, -1);
				g.WriteString((this.mLevelNum < int.MaxValue) ? (TextManager.getInstance().getString(683) + " " + this.mLevelNum) : this.mBossName, 0, num2 + Common._S(Common._M(15)), this.mWidth - num3 * 2, 0);
				g.WriteString(SexyFramework.Common.CommaSeperate(this.mScore), theX, num2 + Common._S(Common._M(30)), this.mWidth - num3 * 2, 1);
				g.DrawString(TextManager.getInstance().getString(433), theX, num2 + Common._S(75));
				g.SetFont(fontByID2);
				g.SetColor(Common._M(255), Common._M1(0), Common._M2(0), num);
				g.WriteString(TextManager.getInstance().getString(434), 0, num2 + Common._S(Common._M(60)), this.mWidth, 0);
			}
			else
			{
				Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARD_BACK);
				Image imageByID2 = Res.GetImageByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARDTEXT_BASETEXT);
				g.DrawImage(imageByID, this.mPostCardX, this.mPostCardY);
				g.DrawImage(imageByID2, this.mPostCardX + Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARDTEXT_BASETEXT)), this.mPostCardY + Common._DS(Res.GetOffsetYByID(ResID.IMAGE_GUI_CHECKPOINT_POSTCARDTEXT_BASETEXT)));
				int num4 = gApp.GetLevelMgr().GetLevelIndex(gApp.mBoard.mLevel.mId);
				if (gApp.mBoard.mLevel.mNum <= 5)
				{
					num4 -= gApp.mBoard.mLevel.mNum - 1;
				}
				else
				{
					num4 -= gApp.mBoard.mLevel.mNum - 6;
				}
				int num5 = gApp.mBoard.mLevel.mZone;
				int theLevelNum = this.mLevelNum - 1;
				g.PushState();
				Image levelThumbnail = gApp.GetLevelThumbnail(theLevelNum);
				float num6 = 2f;
				int value = 1225;
				int value2 = 332;
				int num7 = -27;
				int value3 = (int)(num6 * (float)levelThumbnail.mWidth);
				int value4 = (int)(num6 * (float)levelThumbnail.mHeight);
				if (GameApp.mGameRes != 768)
				{
					g.DrawImage(levelThumbnail, Common._DS(value) + gApp.GetScreenRect().mX, Common._DS(value2), Common._DS(value3), Common._DS(value4));
				}
				else
				{
					g.DrawImage(levelThumbnail, Common._DS(value) + gApp.GetScreenRect().mX + num7, Common._DS(value2), Common._DS(value3), Common._DS(value4));
				}
				Font fontByID3 = Res.GetFontByID(ResID.FONT_CHECKPOINT_CURSIVE);
				g.SetFont(fontByID3);
				g.SetColor(Common._M(0), Common._M1(0), Common._M2(0), num);
				int theX2 = Common._S(Common._M(480));
				int num8 = Common._S(Common._M(320));
				g.DrawString(this.mZone, theX2, num8);
				g.DrawString(TextManager.getInstance().getString(683) + " " + this.mLevelNum, theX2, num8 + Common._S(Common._M(25)));
				g.DrawString(SexyFramework.Common.CommaSeperate(this.mScore) + " pts", theX2, num8 + Common._S(Common._M(50)));
			}
			g.SetColorizeImages(false);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x00051D70 File Offset: 0x0004FF70
		public void ButtonDepress(int id)
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			if (id == 1)
			{
				GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.DoDeferredEndGame);
				GameApp.gApp.ToggleBambooTransition();
				return;
			}
			if (id == 0)
			{
				this.mContinuePressed = true;
				this.mDone = true;
			}
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00051DDA File Offset: 0x0004FFDA
		public override void MouseDown(int x, int y, int cc)
		{
			if (this.mState != 0)
			{
				return;
			}
			if (!this.mFromGameOver)
			{
				this.mUpdateCnt = 0;
				this.mState = -1;
			}
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00051DFC File Offset: 0x0004FFFC
		public void Disable(bool d)
		{
			this.SetDisabled(d);
			this.SetVisible(!d);
			for (int i = 0; i < 2; i++)
			{
				if (this.mButtons[i] != null)
				{
					this.mButtons[i].SetDisabled(d);
					this.mButtons[i].SetVisible(!d);
				}
			}
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00051E50 File Offset: 0x00050050
		public virtual void PreDraw(Graphics g)
		{
			g.SetColor(0, 0, 0, (int)this.mAlpha);
			g.FillRect(0, 0, this.mWidth, this.mHeight);
			Graphics3D graphics3D = g.Get3D();
			if (!MathUtils._eq(this.mSize, 1f) && graphics3D != null)
			{
				this.mTransform.Translate((float)(-(float)GameApp.gApp.mWidth / 2), (float)(-(float)GameApp.gApp.mHeight / 2));
				this.mTransform.Scale(this.mSize, this.mSize);
				this.mTransform.Translate((float)(GameApp.gApp.mWidth / 2), (float)(GameApp.gApp.mHeight / 2));
				graphics3D.PushTransform(this.mTransform);
			}
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00051F14 File Offset: 0x00050114
		public override void DrawAll(ModalFlags theFlags, Graphics g)
		{
			this.PreDraw(g);
			this.Draw(g);
			for (int i = 0; i < 2; i++)
			{
				if (this.mButtons[i] != null)
				{
					g.Translate(this.mButtons[i].mX, this.mButtons[i].mY);
					this.mButtons[i].Draw(g);
					g.Translate(-this.mButtons[i].mX, -this.mButtons[i].mY);
				}
			}
			this.PostDraw(g);
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x00051F9C File Offset: 0x0005019C
		public virtual void PostDraw(Graphics g)
		{
			Graphics3D graphics3D = g.Get3D();
			if (!MathUtils._eq(this.mSize, 1f) && graphics3D != null)
			{
				graphics3D.PopTransform();
			}
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00051FCB File Offset: 0x000501CB
		public void ButtonDownTick(int theId)
		{
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x00051FCD File Offset: 0x000501CD
		public void ButtonMouseEnter(int theId)
		{
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x00051FCF File Offset: 0x000501CF
		public void ButtonMouseLeave(int theId)
		{
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x00051FD1 File Offset: 0x000501D1
		public void ButtonMouseMove(int theId, int theX, int theY)
		{
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x00051FD3 File Offset: 0x000501D3
		public void ButtonPress(int theId)
		{
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00051FD5 File Offset: 0x000501D5
		public void ButtonPress(int theId, int theClickCount)
		{
		}

		// Token: 0x0400070F RID: 1807
		protected ButtonWidget[] mButtons = new ButtonWidget[2];

		// Token: 0x04000710 RID: 1808
		protected string mZone;

		// Token: 0x04000711 RID: 1809
		protected string mBossName;

		// Token: 0x04000712 RID: 1810
		protected string mPostcardGroupName = "";

		// Token: 0x04000713 RID: 1811
		protected int mScore;

		// Token: 0x04000714 RID: 1812
		protected float mAlpha;

		// Token: 0x04000715 RID: 1813
		protected float mSize;

		// Token: 0x04000716 RID: 1814
		protected int mState;

		// Token: 0x04000717 RID: 1815
		protected int mPostCardX;

		// Token: 0x04000718 RID: 1816
		protected int mPostCardY;

		// Token: 0x04000719 RID: 1817
		public int mLevelNum;

		// Token: 0x0400071A RID: 1818
		public bool mFromGameOver;

		// Token: 0x0400071B RID: 1819
		public bool mDone;

		// Token: 0x0400071C RID: 1820
		public bool mContinuePressed;

		// Token: 0x0400071D RID: 1821
		public bool mShowMap;

		// Token: 0x0400071E RID: 1822
		private SexyTransform2D mTransform = new SexyTransform2D(false);

		// Token: 0x0200008B RID: 139
		public enum ButtonId
		{
			// Token: 0x04000720 RID: 1824
			Button_Continue,
			// Token: 0x04000721 RID: 1825
			Button_MainMenu,
			// Token: 0x04000722 RID: 1826
			Max_Buttons
		}
	}
}
