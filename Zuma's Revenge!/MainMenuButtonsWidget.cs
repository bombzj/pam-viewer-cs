using System;
using System.Collections.Generic;
using System.Linq;
//using Microsoft.Phone.Tasks;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000118 RID: 280
	public class MainMenuButtonsWidget : Widget, ButtonListener
	{
		// Token: 0x06000E5C RID: 3676 RVA: 0x00093E62 File Offset: 0x00092062
		public void ButtonPress(int theId)
		{
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00093E64 File Offset: 0x00092064
		public void ButtonPress(int theId, int theClickCount)
		{
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00093E68 File Offset: 0x00092068
		public void ButtonDepress(int theId)
		{
			GameApp gameApp = this.mApp;
			if (this.mMenu.mFirstTimeAlpha > 0 || this.mMenu.mIFUnlockAnim != null || this.mApp.mGenericHelp != null || this.mMenu.mDelayedIFStartState > 0 || this.mMenu.ShowingTikiTemple() || this.mApp.mMapScreen != null)
			{
				return;
			}
			if (this.mMenu.mState == MainMenu_State.State_Scroll)
			{
				return;
			}
			if (gameApp.mBambooTransition != null && gameApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			if (this.mMenu.mChallengeMenu != null && (this.mMenu.mUpdateCnt - this.mMenu.mChallengeMenu.mCSVisFrame < 10 || this.mMenu.mChallengeMenu.mCrownZoomType >= 0 || this.mMenu.mChallengeMenu.mDoBounceTrophy || this.mMenu.mChallengeMenu.mCrossFadeTrophies))
			{
				return;
			}
			Dialog dialog = gameApp.GetDialog(2);
			if (dialog != null)
			{
				return;
			}
			this.mMenu.mTip = null;
			this.mApp.mClickedHardMode = false;
			GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BUTTON3));
			if (theId == 0)
			{
				if (!this.mApp.IsRegistered() && this.mApp.mTrialType == 1 && (this.mApp.mUserProfile.mAdvModeVars.mCurrentAdvZone > 2 || this.mApp.mUserProfile.mAdvModeVars.mHighestZoneBeat >= 2))
				{
					this.mApp.DoUpsell(false);
					return;
				}
				if (!this.mApp.mUserProfile.mNeedsFirstTimeIntro)
				{
					this.mApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(this.mApp.ShowAdventureModeMapScreen);
					this.mApp.ToggleBambooTransition();
					return;
				}
				this.mMenu.mFirstTimeAlpha = 1;
				return;
			}
			else
			{
				if (theId == 5)
				{
					return;
				}
				if (theId == 1)
				{
					if (GameApp.USE_TRIAL_VERSION)
					{
						this.mMenu.mState = MainMenu_State.State_QuitPrompt;
						string @string = TextManager.getInstance().getString(836);
						int width_pad = Common._DS(Common._M(20));
						GameApp.gApp.DoYesNoDialog(TextManager.getInstance().getString(835), @string, true, TextManager.getInstance().getString(446), TextManager.getInstance().getString(447), false, Common._S(Common._M(50)), 1, width_pad);
						GameApp.gApp.mYesNoDialogDelegate = new GameApp.YesNoDialogDelegate(this.ProcessUnlock);
						return;
					}
					if (!this.mApp.ChallengeModeUnlocked())
					{
						this.mMenu.mState = MainMenu_State.State_UnlockPrompt;
						this.mApp.DoGenericDialog(TextManager.getInstance().getString(837), TextManager.getInstance().getString(838), true, new GameApp.PreBlockCallback(this.ChangeMainMenuState), Common._DS(100));
						this.mMenu.mSkipEnterSound = true;
						return;
					}
					gameApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(this.mMenu.ShowChallengeMenuFromMainMenu);
					gameApp.ToggleBambooTransition();
					this.mApp.mUserProfile.mDoChallengeAceCupComplete = (this.mApp.mUserProfile.mDoChallengeCupComplete = false);
					this.mApp.mUserProfile.mDoChallengeAceTrophyZoom = (this.mApp.mUserProfile.mDoChallengeTrophyZoom = false);
					this.mApp.mUserProfile.mNewChallengeCupUnlocked = false;
					return;
				}
				else
				{
					if (theId == 9)
					{
						gameApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(gameApp.mMainMenu.ShowTikiTemple);
						gameApp.ToggleBambooTransition();
						return;
					}
					if (theId == 3)
					{
						if (GameApp.USE_TRIAL_VERSION)
						{
							this.ProcessLocked(false);
							return;
						}
						if (GameApp.UN_UPDATE_VERSION)
						{
							GameApp.gApp.HandleGameUpdateRequired(null);
							return;
						}
						GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.mMainMenu.ShowAchievements);
						GameApp.gApp.ToggleBambooTransition();
						return;
					}
					else if (theId == 2)
					{
						if (GameApp.USE_TRIAL_VERSION)
						{
							this.ProcessLocked(false);
							return;
						}
						if (GameApp.UN_UPDATE_VERSION)
						{
							GameApp.gApp.HandleGameUpdateRequired(null);
							return;
						}
						GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.mMainMenu.ShowLeaderBoards);
						GameApp.gApp.ToggleBambooTransition();
						return;
					}
					else
					{
						if (theId == 4)
						{
							GameApp.gApp.OpenURL("http://mg.eamobile.com/?rId=1560");
							return;
						}
						if (theId == 14 && GameApp.USE_TRIAL_VERSION)
						{
							this.ProcessLocked(true);
						}
						return;
					}
				}
			}
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x000942B6 File Offset: 0x000924B6
		public void ChangeMainMenuState()
		{
			this.mMenu.mState = MainMenu_State.State_MainMenu;
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x000942C4 File Offset: 0x000924C4
		public void ProcessLocked(bool unlock)
		{
			this.mMenu.mState = MainMenu_State.State_QuitPrompt;
			string @string;
			if (unlock)
			{
				@string = TextManager.getInstance().getString(834);
			}
			else
			{
				@string = TextManager.getInstance().getString(836);
			}
			int width_pad = Common._DS(Common._M(20));
			GameApp.gApp.DoYesNoDialog(TextManager.getInstance().getString(835), @string, true, TextManager.getInstance().getString(446), TextManager.getInstance().getString(447), false, Common._S(Common._M(50)), 1, width_pad);
			GameApp.gApp.mYesNoDialogDelegate = new GameApp.YesNoDialogDelegate(this.ProcessUnlock);
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x00094375 File Offset: 0x00092575
		public void ProcessUnlock(int theId)
		{
			if (theId == 1000 && GameApp.USE_TRIAL_VERSION)
			{
				GameApp.gApp.ToMarketPlace();
			}
			this.mMenu.mState = MainMenu_State.State_MainMenu;
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x0009439C File Offset: 0x0009259C
		public void ProcessUpdateLocked()
		{
			this.mMenu.mState = MainMenu_State.State_QuitPrompt;
			string @string = TextManager.getInstance().getString(62);
			int width_pad = Common._DS(Common._M(20));
			GameApp.gApp.DoYesNoDialog(TextManager.getInstance().getString(62), @string, true, TextManager.getInstance().getString(446), TextManager.getInstance().getString(447), false, Common._S(Common._M(50)), 1, width_pad);
			GameApp.gApp.mYesNoDialogDelegate = new GameApp.YesNoDialogDelegate(this.ProcessUpdateUnlock);
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x0009442C File Offset: 0x0009262C
		public void ProcessUpdateUnlock(int theId)
		{
			/*if (theId == 1000)
			{
				new MarketplaceDetailTask
				{
					ContentType = 1
				}.Show();
			}*/
			this.mMenu.mState = MainMenu_State.State_MainMenu;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x00094460 File Offset: 0x00092660
		public void UpdateLeaderboard()
		{
			this.mMenu.mState = MainMenu_State.State_QuitPrompt;
			string @string = TextManager.getInstance().getString(61);
			int width_pad = Common._DS(Common._M(20));
			GameApp.gApp.DoYesNoDialog(TextManager.getInstance().getString(61), @string, true, TextManager.getInstance().getString(446), TextManager.getInstance().getString(447), false, Common._S(Common._M(50)), 1, width_pad);
			GameApp.gApp.mYesNoDialogDelegate = new GameApp.YesNoDialogDelegate(this.ProcessLeaderboard);
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x000944F0 File Offset: 0x000926F0
		public void ProcessLeaderboard(int theId)
		{
			if (theId == 1000)
			{
				GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.mMainMenu.ShowLeaderBoards);
				GameApp.gApp.ToggleBambooTransition();
			}
			this.mMenu.mState = MainMenu_State.State_MainMenu;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00094540 File Offset: 0x00092740
		public void UpdateAchievement()
		{
			this.mMenu.mState = MainMenu_State.State_QuitPrompt;
			string @string = TextManager.getInstance().getString(61);
			int width_pad = Common._DS(Common._M(20));
			GameApp.gApp.DoYesNoDialog(TextManager.getInstance().getString(61), @string, true, TextManager.getInstance().getString(446), TextManager.getInstance().getString(447), false, Common._S(Common._M(50)), 1, width_pad);
			GameApp.gApp.mYesNoDialogDelegate = new GameApp.YesNoDialogDelegate(this.ProcessAchievement);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x000945D0 File Offset: 0x000927D0
		public void ProcessAchievement(int theId)
		{
			if (theId == 1000)
			{
				GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.mMainMenu.ShowAchievements);
				GameApp.gApp.ToggleBambooTransition();
			}
			this.mMenu.mState = MainMenu_State.State_MainMenu;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0009461F File Offset: 0x0009281F
		public void ButtonDownTick(int theId)
		{
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00094621 File Offset: 0x00092821
		public void ButtonMouseEnter(int theId)
		{
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x00094623 File Offset: 0x00092823
		public void ButtonMouseLeave(int theId)
		{
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00094625 File Offset: 0x00092825
		public void ButtonMouseMove(int theId, int theX, int theY)
		{
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00094628 File Offset: 0x00092828
		public MainMenuButtonsWidget(MainMenu theMenu, GameApp theApp)
		{
			this.mApp = theApp;
			this.mMenu = theMenu;
			this.mWidth = this.mApp.GetScreenWidth();
			this.IMAGE_UI_MAINMENU_TIKI = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_TIKI);
			this.IMAGE_UI_MAINMENU_SCROLLMENU_BORDER = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_BORDER);
			this.IMAGE_UI_MAINMENU_SCROLLMENU_BUTTON = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_BUTTON);
			this.IMAGE_UI_MAINMENU_ADVENTURE_OFF_STATE = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_ADVENTURE_OFF_STATE);
			this.IMAGE_UI_MAINMENU_ADVENTURE_CLICK = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_ADVENTURE_CLICK);
			this.IMAGE_UI_MAINMENU_CHALLENGE_CLICK = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_CHALLENGE_CLICK);
			this.IMAGE_UI_MAINMENU_CHALLENGE_OFF_STATE = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_CHALLENGE_OFF_STATE);
			this.IMAGE_UI_MAINMENU_TIKI_CLICK = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_TIKI_CLICK);
			this.IMAGE_UI_MAINMENU_TIKI_OFF_STATE = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_TIKI_OFF_STATE);
			this.IMAGE_UI_MAINMENU_SCROLLMENU_WOODTEXTURE = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_WOODTEXTURE);
			this.IMAGE_UI_MAINMENU_SCROLLMENU_DECORATION = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_DECORATION);
			this.IMAGE_UI_MAINMENU_SCROLLMENU_BORDERBOT = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_BORDERBOT);
			this.IMAGE_UI_MAINMENU_SCROLLMENU_DECORATIONBOT = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_DECORATIONBOT);
			this.IMAGE_UI_MM_FLOWER = Res.GetImageByID(ResID.IMAGE_UI_MM_FLOWER);
			this.IMAGE_UI_MM_FLOWERBOT = Res.GetImageByID(ResID.IMAGE_UI_MM_FLOWERBOT);
			this.IMAGE_UI_MAINMENU_LEADERBOARD_OFF_STATE = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_LEADERBOARD_OFF_STATE);
			this.IMAGE_UI_MAINMENU_LEADERBOARD_CLICK = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_LEADERBOARD_CLICK);
			this.IMAGE_UI_MAINMENU_ACHIEVEMENT_CLICK = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_ACHIEVEMENT_CLICK);
			this.IMAGE_UI_MAINMENU_ACHIEVEMENT_OFF_STATE = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_ACHIEVEMENT_OFF_STATE);
			this.IMAGE_UI_MAINMENU_HELP_CLICK = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_HELP_CLICK);
			this.IMAGE_UI_MAINMENU_HELP_OFF_STATE = Res.GetImageByID(ResID.IMAGE_UI_MAINMENU_HELP_OFF_STATE);
			this.mHeight = this.IMAGE_UI_MAINMENU_TIKI.GetHeight() + Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_TIKI) - Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_TIKI)) + Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_BORDER) + this.IMAGE_UI_MAINMENU_SCROLLMENU_BORDER.GetHeight() - Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_TIKI) + this.IMAGE_UI_MAINMENU_TIKI.GetHeight())));
			this.mButtonWidth = this.IMAGE_UI_MAINMENU_TIKI.GetWidth() * 2 + this.IMAGE_UI_MAINMENU_SCROLLMENU_BUTTON.GetWidth() + this.IMAGE_UI_MAINMENU_SCROLLMENU_BORDER.GetWidth() / 2;
			this.mButtonOriginX = Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_MAINMENU_CHALLENGE_OFF_STATE)) - Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_MAINMENU_TIKI)) - this.IMAGE_UI_MAINMENU_TIKI.GetWidth();
			this.mButtonOriginY = Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_CHALLENGE_OFF_STATE)) - Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_TIKI));
			this.mPlankHeight = Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_TIKI)) + this.IMAGE_UI_MAINMENU_TIKI.GetHeight() - Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_WOODTEXTURE));
			this.IMAGE_UI_MAINMENU_CHALLENGE_CLICK.GetWidth();
			this.IMAGE_UI_MAINMENU_CHALLENGE_CLICK.GetHeight();
			int num = Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_MAINMENU_CHALLENGE_OFF_STATE));
			int num2 = Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_CHALLENGE_OFF_STATE)) + 8;
			this.AddNewButtonFrame(0, this.IMAGE_UI_MAINMENU_ADVENTURE_OFF_STATE, this.IMAGE_UI_MAINMENU_ADVENTURE_CLICK, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_MAINMENU_ADVENTURE_OFF_STATE)) - num, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_ADVENTURE_OFF_STATE)) - num2);
			this.AddNewButtonFrame(1, this.IMAGE_UI_MAINMENU_CHALLENGE_OFF_STATE, this.IMAGE_UI_MAINMENU_CHALLENGE_CLICK, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_MAINMENU_CHALLENGE_OFF_STATE)) - num, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_CHALLENGE_OFF_STATE)) - num2);
			this.AddNewButtonFrame(9, this.IMAGE_UI_MAINMENU_TIKI_OFF_STATE, this.IMAGE_UI_MAINMENU_TIKI_CLICK, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_MAINMENU_TIKI_OFF_STATE)) - num, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_TIKI_OFF_STATE)) - num2);
			this.AddNewButtonFrame(3, this.IMAGE_UI_MAINMENU_ACHIEVEMENT_OFF_STATE, this.IMAGE_UI_MAINMENU_ACHIEVEMENT_CLICK, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_MAINMENU_ACHIEVEMENT_OFF_STATE)) - num, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_ACHIEVEMENT_OFF_STATE)) - num2);
			this.AddNewButtonFrame(2, this.IMAGE_UI_MAINMENU_LEADERBOARD_OFF_STATE, this.IMAGE_UI_MAINMENU_LEADERBOARD_CLICK, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_MAINMENU_LEADERBOARD_OFF_STATE)) - num, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_LEADERBOARD_OFF_STATE)) - num2);
			this.AddNewButtonFrame(4, this.IMAGE_UI_MAINMENU_HELP_OFF_STATE, this.IMAGE_UI_MAINMENU_HELP_CLICK, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_MAINMENU_HELP_OFF_STATE)) - num, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_HELP_OFF_STATE)) - num2);
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00094A4B File Offset: 0x00092C4B
		public override void Dispose()
		{
			if (this.mButtonFrames.Count > 0)
			{
				this.mButtonFrames.Clear();
			}
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00094A66 File Offset: 0x00092C66
		public int GetButtonCount()
		{
			return this.mButtonFrames.Count;
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x00094A74 File Offset: 0x00092C74
		public void AddNewButtonFrame(int theButtonID, Image theButtonImage, Image theButtonDownImage, int x, int y)
		{
			MainMenuButtonsWidget.MenuButtonFrame menuButtonFrame = default(MainMenuButtonsWidget.MenuButtonFrame);
			if (this.mButtonFrames.Count == 0)
			{
				menuButtonFrame.mX = 0f;
				menuButtonFrame.mY = 0f;
			}
			else
			{
				menuButtonFrame.mX = Enumerable.Last<MainMenuButtonsWidget.MenuButtonFrame>(this.mButtonFrames).mX + (float)this.mButtonWidth;
				menuButtonFrame.mY = 0f;
			}
			menuButtonFrame.mButton = new ButtonWidget(theButtonID, this);
			menuButtonFrame.mButton.mButtonImage = theButtonImage;
			menuButtonFrame.mButton.mDownImage = theButtonDownImage;
			float num = (float)(theButtonDownImage.GetWidth() - theButtonImage.GetWidth()) / 2f;
			float num2 = (float)(theButtonDownImage.GetHeight() - theButtonImage.GetHeight()) / 2f;
			menuButtonFrame.mButton.Resize((int)(menuButtonFrame.mX + (float)this.mButtonOriginX + num) + x, (int)(menuButtonFrame.mY + (float)this.mButtonOriginY + num2) + y, theButtonDownImage.GetWidth(), theButtonDownImage.GetHeight());
			menuButtonFrame.mButton.mNormalRect = new Rect(0, 0, theButtonImage.GetWidth(), theButtonImage.GetHeight());
			menuButtonFrame.mButton.mDownRect = new Rect((int)num, (int)num2, (int)((float)theButtonDownImage.GetWidth() - num), (int)((float)theButtonDownImage.GetHeight() - num2));
			this.AddWidget(menuButtonFrame.mButton);
			this.mButtonFrames.Enqueue(menuButtonFrame);
			this.Resize((int)Enumerable.First<MainMenuButtonsWidget.MenuButtonFrame>(this.mButtonFrames).mX, 0, this.mButtonFrames.Count * this.mButtonWidth, this.mHeight);
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00094C00 File Offset: 0x00092E00
		public void DrawButtonFrame(Graphics g, MainMenuButtonsWidget.MenuButtonFrame theFrame)
		{
			int num = Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_MAINMENU_TIKI)) + this.IMAGE_UI_MAINMENU_TIKI.GetWidth() - Common._DS(11);
			int num2 = Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_TIKI));
			int num3 = Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_WOODTEXTURE)) - num2;
			g.ClearClipRect();
			g.SetClipRect((int)theFrame.mX, num3, this.mButtonWidth, this.mPlankHeight - Common._DS(15));
			int num4 = (int)theFrame.mX;
			int i = num3;
			bool flag = false;
			while (i <= num3 + this.mPlankHeight)
			{
				while ((float)num4 <= theFrame.mX + (float)this.mButtonWidth)
				{
					if (flag)
					{
						g.DrawImageMirror(this.IMAGE_UI_MAINMENU_SCROLLMENU_WOODTEXTURE, num4, i);
					}
					else
					{
						g.DrawImage(this.IMAGE_UI_MAINMENU_SCROLLMENU_WOODTEXTURE, num4, i);
					}
					num4 += this.IMAGE_UI_MAINMENU_SCROLLMENU_WOODTEXTURE.GetWidth();
				}
				num4 = (int)theFrame.mX;
				i += this.IMAGE_UI_MAINMENU_SCROLLMENU_WOODTEXTURE.GetHeight();
				flag = false;
			}
			g.ClearClipRect();
			int num5 = Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_BORDER)) - num2;
			int num6 = Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_DECORATION)) - num;
			int num7 = this.mButtonWidth - this.IMAGE_UI_MAINMENU_TIKI.GetWidth() - this.IMAGE_UI_MAINMENU_SCROLLMENU_DECORATION.GetWidth() + Common._DS(MainMenuButtonsWidget.aTRSideOffsetX);
			int num8 = Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_DECORATION)) - num2;
			g.DrawImage(this.IMAGE_UI_MAINMENU_SCROLLMENU_BORDER, (int)theFrame.mX, (int)(theFrame.mY + (float)num5), this.mButtonWidth, this.IMAGE_UI_MAINMENU_SCROLLMENU_BORDER.GetHeight());
			g.DrawImage(this.IMAGE_UI_MAINMENU_SCROLLMENU_DECORATION, (int)(theFrame.mX + (float)num6), (int)(theFrame.mY + (float)num8));
			g.DrawImageMirror(this.IMAGE_UI_MAINMENU_SCROLLMENU_DECORATION, (int)(theFrame.mX + (float)num7), (int)(theFrame.mY + (float)num8));
			int num9 = Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_BORDERBOT)) - num2;
			int num10 = Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_DECORATIONBOT)) - num2;
			int num11 = Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_DECORATIONBOT)) - num + Common._DS(-10);
			int num12 = this.mButtonWidth - this.IMAGE_UI_MAINMENU_TIKI.GetWidth() - this.IMAGE_UI_MAINMENU_SCROLLMENU_DECORATION.GetWidth() + Common._DS(27);
			g.DrawImage(this.IMAGE_UI_MAINMENU_SCROLLMENU_BORDERBOT, (int)theFrame.mX, (int)(theFrame.mY + (float)num9), this.mButtonWidth, this.IMAGE_UI_MAINMENU_SCROLLMENU_BORDER.GetHeight());
			g.DrawImage(this.IMAGE_UI_MAINMENU_SCROLLMENU_DECORATIONBOT, (int)(theFrame.mX + (float)num11), (int)(theFrame.mY + (float)num10));
			g.DrawImageMirror(this.IMAGE_UI_MAINMENU_SCROLLMENU_DECORATIONBOT, (int)(theFrame.mX + (float)num12), (int)(theFrame.mY + (float)num10));
			int num13 = Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_BUTTON)) - num;
			int num14 = Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAINMENU_SCROLLMENU_BUTTON)) - num2;
			g.DrawImage(this.IMAGE_UI_MAINMENU_SCROLLMENU_BUTTON, (int)(theFrame.mX + (float)num13), (int)(theFrame.mY + (float)num14));
			int num15 = 0;
			g.DrawImageMirror(this.IMAGE_UI_MAINMENU_TIKI, (int)theFrame.mX, (int)(theFrame.mY + (float)num15));
			int num16 = this.mButtonWidth - this.IMAGE_UI_MAINMENU_TIKI.GetWidth();
			g.DrawImage(this.IMAGE_UI_MAINMENU_TIKI, (int)(theFrame.mX + (float)num16), (int)(theFrame.mY + (float)num15));
			g.DrawImage(this.IMAGE_UI_MM_FLOWER, (int)(theFrame.mX + (float)num13 - (float)Common._DS(MainMenuButtonsWidget.flowerXOff)), (int)(theFrame.mY + (float)num14 - (float)Common._DS(MainMenuButtonsWidget.flowerYOff)));
			g.DrawImageMirror(this.IMAGE_UI_MM_FLOWER, (int)(theFrame.mX + (float)num13 + (float)this.IMAGE_UI_MAINMENU_SCROLLMENU_BUTTON.GetWidth() - (float)this.IMAGE_UI_MM_FLOWER.GetWidth() + (float)Common._DS(MainMenuButtonsWidget.flowerXOff)), (int)(theFrame.mY + (float)num14 - (float)Common._DS(MainMenuButtonsWidget.flowerYOff)));
			g.DrawImage(this.IMAGE_UI_MM_FLOWERBOT, (int)(theFrame.mX + (float)num13 - (float)Common._DS(MainMenuButtonsWidget.flowerXOff)), (int)(theFrame.mY + (float)num14 + (float)this.IMAGE_UI_MAINMENU_SCROLLMENU_BUTTON.GetHeight() - (float)this.IMAGE_UI_MM_FLOWERBOT.GetHeight() + (float)Common._DS(MainMenuButtonsWidget.flowerYOff)));
			g.DrawImageMirror(this.IMAGE_UI_MM_FLOWERBOT, (int)(theFrame.mX + (float)num13 + (float)this.IMAGE_UI_MAINMENU_SCROLLMENU_BUTTON.GetWidth() - (float)this.IMAGE_UI_MM_FLOWERBOT.GetWidth() + (float)Common._DS(MainMenuButtonsWidget.flowerXOff)), (int)(theFrame.mY + (float)num14 + (float)this.IMAGE_UI_MAINMENU_SCROLLMENU_BUTTON.GetHeight() - (float)this.IMAGE_UI_MM_FLOWERBOT.GetHeight() + (float)Common._DS(MainMenuButtonsWidget.flowerYOff)));
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x000950BC File Offset: 0x000932BC
		public override void Draw(Graphics g)
		{
			base.Draw(g);
			if (this.mApp != null && this.mApp.mMapScreen != null)
			{
				return;
			}
			if (this.mMenu != null && this.mMenu.mChallengeMenu != null)
			{
				return;
			}
			if (this.mButtonFrames.Count > 0)
			{
				foreach (MainMenuButtonsWidget.MenuButtonFrame theFrame in this.mButtonFrames)
				{
					this.DrawButtonFrame(g, theFrame);
				}
			}
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x00095130 File Offset: 0x00093330
		public override void Update()
		{
			base.Update();
			int pageHorizontal = this.mMenu.mMainMenuButtonsScrollWidget.GetPageHorizontal();
			int num = 0;
			Queue<MainMenuButtonsWidget.MenuButtonFrame>.Enumerator enumerator = this.mButtonFrames.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (num == pageHorizontal)
				{
					enumerator.Current.mButton.SetDisabled(false);
				}
				else
				{
					enumerator.Current.mButton.SetDisabled(true);
				}
				num++;
			}
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x0009519C File Offset: 0x0009339C
		public void HideScrollButtons()
		{
			foreach (MainMenuButtonsWidget.MenuButtonFrame menuButtonFrame in this.mButtonFrames)
			{
				menuButtonFrame.mButton.SetVisible(false);
			}
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x000951D4 File Offset: 0x000933D4
		public void ShowScrollButtons()
		{
			foreach (MainMenuButtonsWidget.MenuButtonFrame menuButtonFrame in this.mButtonFrames)
			{
				menuButtonFrame.mButton.SetVisible(true);
			}
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0009520A File Offset: 0x0009340A
		public int GetNumButtons()
		{
			return this.mButtonFrames.Count;
		}

		// Token: 0x04000DED RID: 3565
		public GameApp mApp;

		// Token: 0x04000DEE RID: 3566
		public MainMenu mMenu;

		// Token: 0x04000DEF RID: 3567
		public int mCurrentlySelectedButton;

		// Token: 0x04000DF0 RID: 3568
		public Queue<MainMenuButtonsWidget.MenuButtonFrame> mButtonFrames = new Queue<MainMenuButtonsWidget.MenuButtonFrame>();

		// Token: 0x04000DF1 RID: 3569
		public int mButtonWidth;

		// Token: 0x04000DF2 RID: 3570
		public int mPlankHeight;

		// Token: 0x04000DF3 RID: 3571
		public int mButtonOriginX;

		// Token: 0x04000DF4 RID: 3572
		public int mButtonOriginY;

		// Token: 0x04000DF5 RID: 3573
		public int mVisibleButtonX;

		// Token: 0x04000DF6 RID: 3574
		private Image IMAGE_UI_MAINMENU_TIKI;

		// Token: 0x04000DF7 RID: 3575
		private Image IMAGE_UI_MAINMENU_SCROLLMENU_BORDER;

		// Token: 0x04000DF8 RID: 3576
		private Image IMAGE_UI_MAINMENU_SCROLLMENU_BUTTON;

		// Token: 0x04000DF9 RID: 3577
		private Image IMAGE_UI_MAINMENU_ADVENTURE_OFF_STATE;

		// Token: 0x04000DFA RID: 3578
		private Image IMAGE_UI_MAINMENU_ADVENTURE_CLICK;

		// Token: 0x04000DFB RID: 3579
		private Image IMAGE_UI_MAINMENU_CHALLENGE_CLICK;

		// Token: 0x04000DFC RID: 3580
		private Image IMAGE_UI_MAINMENU_CHALLENGE_OFF_STATE;

		// Token: 0x04000DFD RID: 3581
		private Image IMAGE_UI_MAINMENU_LEADERBOARD_OFF_STATE;

		// Token: 0x04000DFE RID: 3582
		private Image IMAGE_UI_MAINMENU_LEADERBOARD_CLICK;

		// Token: 0x04000DFF RID: 3583
		private Image IMAGE_UI_MAINMENU_ACHIEVEMENT_CLICK;

		// Token: 0x04000E00 RID: 3584
		private Image IMAGE_UI_MAINMENU_ACHIEVEMENT_OFF_STATE;

		// Token: 0x04000E01 RID: 3585
		private Image IMAGE_UI_MAINMENU_HELP_CLICK;

		// Token: 0x04000E02 RID: 3586
		private Image IMAGE_UI_MAINMENU_HELP_OFF_STATE;

		// Token: 0x04000E03 RID: 3587
		private Image IMAGE_UI_MAINMENU_TIKI_CLICK;

		// Token: 0x04000E04 RID: 3588
		private Image IMAGE_UI_MAINMENU_TIKI_OFF_STATE;

		// Token: 0x04000E05 RID: 3589
		private Image IMAGE_UI_MAINMENU_SCROLLMENU_WOODTEXTURE;

		// Token: 0x04000E06 RID: 3590
		private Image IMAGE_UI_MAINMENU_SCROLLMENU_DECORATION;

		// Token: 0x04000E07 RID: 3591
		private Image IMAGE_UI_MAINMENU_SCROLLMENU_BORDERBOT;

		// Token: 0x04000E08 RID: 3592
		private Image IMAGE_UI_MAINMENU_SCROLLMENU_DECORATIONBOT;

		// Token: 0x04000E09 RID: 3593
		private Image IMAGE_UI_MM_FLOWER;

		// Token: 0x04000E0A RID: 3594
		private Image IMAGE_UI_MM_FLOWERBOT;

		// Token: 0x04000E0B RID: 3595
		private static int aTRSideOffsetX = 20;

		// Token: 0x04000E0C RID: 3596
		private static int flowerXOff = 20;

		// Token: 0x04000E0D RID: 3597
		private static int flowerYOff = 20;

		// Token: 0x02000119 RID: 281
		public struct MenuButtonFrame
		{
			// Token: 0x04000E0E RID: 3598
			public ButtonWidget mButton;

			// Token: 0x04000E0F RID: 3599
			public ButtonWidget mAttachButton;

			// Token: 0x04000E10 RID: 3600
			public float mX;

			// Token: 0x04000E11 RID: 3601
			public float mY;
		}
	}
}
