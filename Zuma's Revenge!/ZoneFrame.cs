using System;
using System.Collections.Generic;
using System.Text;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000045 RID: 69
	public class ZoneFrame : Widget, ButtonListener
	{
		// Token: 0x06000658 RID: 1624 RVA: 0x0002A410 File Offset: 0x00028610
		public ZoneFrame(ChallengeMenu aChallengeMenu, int aZone, int aDebugBGColor)
		{
			this.mChallengeMenu = aChallengeMenu;
			this.mZoneNum = aZone;
			this.mDebugBGColor = aDebugBGColor;
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES);
			this.Resize(0, 0, imageByID.GetWidth(), imageByID.GetHeight());
			this.IMAGE_UI_CHALLENGESCREEN_LARGE_CROWN = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_LARGE_CROWN);
			this.IMAGE_UI_CHALLENGESCREEN_LARGE_ACECROWN = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_LARGE_ACECROWN);
			this.IMAGE_UI_MAIN_MENU_CS_LOCK_ANIMATION = Res.GetImageByID(ResID.IMAGE_UI_MAIN_MENU_CS_LOCK_ANIMATION);
			for (int i = 0; i < 10; i++)
			{
				if (this.mChallengeLevelBtns[i] != null)
				{
					this.mChallengeLevelBtns[i].mLevel = -1;
				}
			}
			for (int j = 0; j < 10; j++)
			{
				if (this.mChallengeLevelBtns[j] == null)
				{
					this.mChallengeLevelBtns[j] = new CSButton(3 + j + this.mZoneNum * 10, this.mChallengeMenu, this);
					this.mChallengeLevelBtns[j].mDoFinger = true;
					this.AddWidget(this.mChallengeLevelBtns[j]);
				}
			}
			int theWidth = Common._DS(GlobalChallenge.CS_BTN_WIDTH);
			int theHeight = Common._DS(GlobalChallenge.CS_BTN_HEIGHT);
			for (int k = 0; k < GlobalChallenge.NUM_CHALLENGE_BUTTON_ROWS; k++)
			{
				for (int l = 0; l < GlobalChallenge.NUM_CHALLENGE_BUTTON_COLS; l++)
				{
					int num = k * GlobalChallenge.NUM_CHALLENGE_BUTTON_COLS + l;
					if (num != 8 && num != 11)
					{
						int num2 = ((num > 8) ? (num - 1) : num);
						CSButton csbutton = this.mChallengeLevelBtns[num2];
						if (csbutton != null)
						{
							int theX = Common._DS(GlobalChallenge.FIRST_X + l * GlobalChallenge.HORIZ_SPACE) - Common._DS(160) + GameApp.gApp.GetScreenRect().mX / 2;
							int theY = Common._DS(GlobalChallenge.FIRST_Y + k * GlobalChallenge.VERT_SPACE);
							csbutton.Resize(theX, theY, theWidth, theHeight);
						}
					}
				}
			}
			this.SetupChallengeZone(this.mZoneNum);
			this.mZoneName = GameApp.gApp.GetLevelMgr().mZones[this.mZoneNum].mCupName;
			this.mZoneDifficulty = GameApp.gApp.GetLevelMgr().mZones[this.mZoneNum].mDifficulty;
			this.mZoneNameStrWidth = -1;
			this.mZoneDifficultyWidth = -1;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0002A640 File Offset: 0x00028840
		public override void Dispose()
		{
			for (uint num = 0U; num < 10U; num += 1U)
			{
				this.RemoveWidget(this.mChallengeLevelBtns[(int)((UIntPtr)num)]);
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0002A66C File Offset: 0x0002886C
		public override void Draw(Graphics g)
		{
			if (g.mClipRect.mWidth <= 0 || g.mClipRect.mHeight <= 0)
			{
				return;
			}
			Common._S(0);
			int gScreenShake = GlobalChallenge.gScreenShake;
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_GAUNTLET));
			g.SetColor(SexyColor.White);
			if (this.mZoneNameStrWidth == -1)
			{
				this.mZoneNameStrWidth = g.GetFont().StringWidth(this.mZoneName);
			}
			g.WriteString(this.mZoneName, Common._DS(100), Common._DS(Common._M(120)), this.mZoneNameStrWidth, 0);
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_GAUNTLET));
			g.SetColor(SexyColor.White);
			string text = TextManager.getInstance().getString(423) + " " + this.mZoneDifficulty;
			float num = (float)(Common._DS(1280) + GameApp.gApp.GetScreenRect().mX / 2);
			float num2 = (float)g.GetFont().StringWidth(text);
			float num3 = num - num2;
			if (num3 <= 450f)
			{
				Rect theRect = new Rect(450, 38, 250, 300);
				g.WriteWordWrapped(theRect, text, 20);
				return;
			}
			g.DrawString(text, (int)num3, Common._DS(120));
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0002A7A8 File Offset: 0x000289A8
		public ButtonWidget GetButton(int id)
		{
			for (int i = 0; i < 10; i++)
			{
				if (this.mChallengeLevelBtns[i] != null && this.mChallengeLevelBtns[i].mId == id)
				{
					return this.mChallengeLevelBtns[i];
				}
			}
			return null;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0002A7E8 File Offset: 0x000289E8
		public virtual void ButtonDepress(int id)
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BUTTON1));
			int num = this.mZoneNum * 10 + 3;
			int num2 = this.mZoneNum * 10 + 13;
			if (id >= num && id < num2)
			{
				CSButton csbutton = (CSButton)this.GetButton(id);
				if (csbutton.mMedal == this.IMAGE_UI_MAIN_MENU_CS_LOCK_ANIMATION)
				{
					GameApp.gApp.DoGenericDialog("", csbutton.mLevelStr, true, null, Common._DS(100));
					GameApp.gApp.mWidgetManager.SetFocus(this.mChallengeMenu);
					return;
				}
				int num3 = id - 3;
				int num4 = num3 - this.mZoneNum * 10;
				this.mChallengeMenu.mSelectedLevel = num3;
				this.mChallengeMenu.ShowChallengeLevelInfo(this.mZoneNum, num3, this.mChallengeLevelBtns[num4].mLevelId);
				this.mChallengeMenu.mChallengeLevelInfoWidget.SetLevelInfo(this.mChallengeLevelBtns[num4].mLevelStr, this.mChallengeLevelBtns[num4].mScoreStr, this.mChallengeLevelBtns[num4].mAceStr, this.mChallengeLevelBtns[num4].mId);
				GameApp.gLastZone = this.mZoneNum;
				this.mChallengeMenu.mChallengeScrollWidget.SetPageHorizontal(this.mZoneNum + 1, true);
			}
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0002A946 File Offset: 0x00028B46
		public void InitCS()
		{
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0002A948 File Offset: 0x00028B48
		public void RehupChallengeButtons()
		{
			int num = Common._S(0);
			if (GameApp.gApp.mUserProfile.mUnlockSparklesIdx1 != -1)
			{
				int num2 = this.mZoneNum * 10;
				int num3 = num2 + 9;
				if (GameApp.gApp.mUserProfile.mUnlockSparklesIdx1 >= num2 && GameApp.gApp.mUserProfile.mUnlockSparklesIdx1 <= num3)
				{
					int num4;
					if (this.mZoneNum > 0)
					{
						num4 = GameApp.gApp.mUserProfile.mUnlockSparklesIdx1 % (this.mZoneNum * 10);
					}
					else
					{
						num4 = GameApp.gApp.mUserProfile.mUnlockSparklesIdx1;
					}
					PIEffect pieffect = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_GOLDSPARKLE_CHALLENGE).Duplicate();
					CSButton csbutton = this.mChallengeLevelBtns[num4];
					csbutton.mUnlockSparkles = pieffect;
					csbutton.mUnlockAlpha = 255;
					float num5 = GameApp.DownScaleNum(1f);
					pieffect.mDrawTransform.Scale(num5, num5);
					pieffect.mDrawTransform.Translate((float)(csbutton.mX - num + Common._DS(GlobalChallenge.CS_BTN_WIDTH) / 2), (float)(csbutton.mY + Common._DS(GlobalChallenge.CS_BTN_HEIGHT) / 2));
					GameApp.gApp.mUserProfile.mUnlockSparklesIdx1 = -1;
				}
			}
			if (GameApp.gApp.mUserProfile.mUnlockSparklesIdx2 != -1)
			{
				int num6 = this.mZoneNum * 10;
				int num7 = num6 + 10;
				if (GameApp.gApp.mUserProfile.mUnlockSparklesIdx2 >= num6 && GameApp.gApp.mUserProfile.mUnlockSparklesIdx2 <= num7)
				{
					int num8;
					if (this.mZoneNum > 0)
					{
						num8 = GameApp.gApp.mUserProfile.mUnlockSparklesIdx2 % (this.mZoneNum * 10);
					}
					else
					{
						num8 = GameApp.gApp.mUserProfile.mUnlockSparklesIdx2;
					}
					PIEffect pieffect2 = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_GOLDSPARKLE_CHALLENGE).Duplicate();
					CSButton csbutton2 = this.mChallengeLevelBtns[num8];
					csbutton2.mUnlockSparkles = pieffect2;
					csbutton2.mUnlockAlpha = 255;
					float num9 = GameApp.DownScaleNum(1f);
					pieffect2.mDrawTransform.Scale(num9, num9);
					pieffect2.mDrawTransform.Translate((float)(csbutton2.mX - num + Common._DS(GlobalChallenge.CS_BTN_WIDTH) / 2), (float)(csbutton2.mY + Common._DS(GlobalChallenge.CS_BTN_HEIGHT) / 2));
					GameApp.gApp.mUserProfile.mUnlockSparklesIdx2 = -1;
				}
			}
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0002AB98 File Offset: 0x00028D98
		public void PreLoadButtonsImage()
		{
			for (int i = 0; i < this.mChallengeLevelBtns.Length; i++)
			{
				if (this.mChallengeLevelBtns[i] != null)
				{
					this.mChallengeLevelBtns[i].PreLoadImage();
				}
			}
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0002ABD0 File Offset: 0x00028DD0
		private void SetupChallengeZone(int zone)
		{
			this.mChallengeMenu.mLoopTrophyFlare = false;
			this.mChallengeMenu.mTrophyFlare = null;
			bool flag = GameApp.gApp.mUserProfile.mChallengeUnlockState[zone, 0] == 0;
			this.mChallengeMenu.mShowFullAceFX = false;
			if (flag)
			{
				this.mChallengeMenu.mDefaultStringContainer.mDefaultStr = ((zone == 7) ? this.mChallengeMenu.mDefaultStringContainer.IfLocked() : this.mChallengeMenu.mDefaultStringContainer.NonIfLocked());
			}
			else if (GameApp.gApp.mUserProfile.mChallengeUnlockState[zone, 0] == 1)
			{
				this.mChallengeMenu.mDefaultStringContainer.mDefaultStr = ((zone == 7) ? this.mChallengeMenu.mDefaultStringContainer.IfLocked() : this.mChallengeMenu.mDefaultStringContainer.ZoneUnlocked());
			}
			else
			{
				this.mChallengeMenu.mDefaultStringContainer.mDefaultStr = ((zone == 7) ? this.mChallengeMenu.mDefaultStringContainer.IfLocked() : this.mChallengeMenu.mDefaultStringContainer.CanPlayZone());
			}
			Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_TROPHYFLARE_Z1);
			Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_TROPHYFLARE_Z2);
			Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_TROPHYFLARE_Z3);
			Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_TROPHYFLARE_Z4);
			Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_TROPHYFLARE_Z5);
			Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_TROPHYFLARE_Z6);
			Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_TROPHYFLARE_Z7);
			for (int i = 0; i < 10; i++)
			{
				int num = zone * 10 + i;
				this.mChallengeLevelBtns[i].mLevel = num;
				string text = "";
				int first = GameApp.gApp.mLevelThumbnails[num].first;
				GameApp.gApp.GetLevelMgr().GetLevelStrData(first, ref this.mChallengeLevelBtns[i].mLevelId, ref text);
				int num2 = 0;
				this.mChallengeLevelBtns[i].mUnlockSparkles = null;
				if (GameApp.gApp.mUserProfile != null)
				{
					List<GauntletHSInfo> list = new List<GauntletHSInfo>();
					int num3 = 0;
					GameApp.gApp.mUserProfile.GetGauntletHighScores(num + 1, ref list);
					if (list.Count > 0)
					{
						for (int j = 0; j < list.Count; j++)
						{
							if (list[j].mProfileName == GameApp.gApp.mUserProfile.GetName() && list[j].mScore > num2)
							{
								num2 = list[j].mScore;
							}
							if (list[j].mScore > num3)
							{
								num3 = list[j].mScore;
							}
						}
					}
					int num4 = GameApp.gApp.mUserProfile.mChallengeUnlockState[zone, i];
					if (num4 < 2)
					{
						this.mChallengeLevelBtns[i].mMedal = this.IMAGE_UI_MAIN_MENU_CS_LOCK_ANIMATION;
					}
					else if (num4 == 4)
					{
						this.mChallengeLevelBtns[i].mMedal = this.IMAGE_UI_CHALLENGESCREEN_LARGE_CROWN;
					}
					else if (num4 == 5)
					{
						this.mChallengeLevelBtns[i].mMedal = this.IMAGE_UI_CHALLENGESCREEN_LARGE_ACECROWN;
					}
					else
					{
						this.mChallengeLevelBtns[i].mMedal = null;
					}
					this.mChallengeLevelBtns[i].mOpaque = flag;
					this.mChallengeLevelBtns[i].mUnlockAlpha = 0;
					if (this.mChallengeLevelBtns[i].mMedal != this.IMAGE_UI_MAIN_MENU_CS_LOCK_ANIMATION)
					{
						Level levelById = GameApp.gApp.GetLevelMgr().GetLevelById(this.mChallengeLevelBtns[i].mLevelId);
						if (num2 > 9999999)
						{
							num2 = 9999999;
						}
						this.mChallengeLevelBtns[i].mScoreStr = SexyFramework.Common.CommaSeperate(num2);
						this.mChallengeLevelBtns[i].mLevelStr = SexyFramework.Common.CommaSeperate(levelById.mChallengePoints);
						this.mChallengeLevelBtns[i].mAceStr = SexyFramework.Common.CommaSeperate(levelById.mChallengeAcePoints);
					}
					else if (flag)
					{
						StringBuilder stringBuilder = new StringBuilder(TextManager.getInstance().getString(424));
						stringBuilder.Replace("$1", ((this.mZoneNum + 1) * 10).ToString());
						this.mChallengeLevelBtns[i].mLevelStr = stringBuilder.ToString();
					}
					else
					{
						this.mChallengeLevelBtns[i].mLevelStr = TextManager.getInstance().getString(425);
					}
				}
			}
			this.MarkDirty();
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0002AFE6 File Offset: 0x000291E6
		public virtual void ButtonDownTick(int x)
		{
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0002AFE8 File Offset: 0x000291E8
		public virtual void ButtonMouseEnter(int x)
		{
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0002AFEA File Offset: 0x000291EA
		public virtual void ButtonMouseLeave(int x)
		{
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0002AFEC File Offset: 0x000291EC
		public virtual void ButtonMouseMove(int x, int y, int z)
		{
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0002AFEE File Offset: 0x000291EE
		public virtual void ButtonPress(int id)
		{
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0002AFF0 File Offset: 0x000291F0
		public virtual void ButtonPress(int id, int count)
		{
		}

		// Token: 0x0400037B RID: 891
		private CSButton[] mChallengeLevelBtns = new CSButton[10];

		// Token: 0x0400037C RID: 892
		private ChallengeMenu mChallengeMenu;

		// Token: 0x0400037D RID: 893
		private int mDebugBGColor;

		// Token: 0x0400037E RID: 894
		private int mZoneNum;

		// Token: 0x0400037F RID: 895
		private string mZoneName;

		// Token: 0x04000380 RID: 896
		private string mZoneDifficulty;

		// Token: 0x04000381 RID: 897
		private int mZoneNameStrWidth;

		// Token: 0x04000382 RID: 898
		private int mZoneDifficultyWidth;

		// Token: 0x04000383 RID: 899
		private Image IMAGE_UI_MAIN_MENU_CS_LOCK_ANIMATION;

		// Token: 0x04000384 RID: 900
		private Image IMAGE_UI_CHALLENGESCREEN_LARGE_CROWN;

		// Token: 0x04000385 RID: 901
		private Image IMAGE_UI_CHALLENGESCREEN_LARGE_ACECROWN;
	}
}
