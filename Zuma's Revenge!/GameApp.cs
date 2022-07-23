using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using JeffLib;
//using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using SexyFramework;
using SexyFramework.AELib;
using SexyFramework.Drivers.App;
using SexyFramework.Drivers.File;
using SexyFramework.Drivers.Profile;
using SexyFramework.File;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;
using ZumasRevenge.Profile;
using ZumasRevenge.Sound;

namespace ZumasRevenge
{
	// Token: 0x020000D0 RID: 208
	public class GameApp : SexyApp, NewUserDialogListener, ProfileEventListener
	{
		// Token: 0x06000AF6 RID: 2806 RVA: 0x0006B454 File Offset: 0x00069654
		protected void PreShowLoadingScreen()
		{
			if (!this.mResourceManager.IsGroupLoaded("LoadScreen"))
			{
				this.mResourceManager.LoadResources("LoadScreen");
			}
			this.mResourceManager.LoadImage("ATLASIMAGE_ATLAS_GAMEPLAY_640_00");
			this.mResourceManager.LoadImage("ATLASIMAGE_ATLAS_MENURELATED_640_00");
			this.mMusic.LoadMusic(1, "music/MUSIC_LOADING");
			this.mMusic.LoadMusic(0, "music/MUSIC_HAWAIIAN");
			this.mMusic.Enable(true);
			this.StartLoading();
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0006B4DC File Offset: 0x000696DC
		public void ShowLoadingScreen()
		{
			this.mLoadingScreen = new LoadingScreen();
			this.mLoadingScreen.Resize(0, 0, this.mWidth, this.mHeight);
			this.mWidgetManager.AddWidget(this.mLoadingScreen);
			if (this.mMusic.IsUserMusicPlaying())
			{
				this.mLoadingScreen.ProcessBGM();
			}
			this.mUnderDialogWidget.CreateImages();
			this.mUnderDialogWidget.Resize(0, 0, this.mWidth, this.mHeight);
			this.mWidgetManager.AddWidget(this.mUnderDialogWidget);
			this.mUnderDialogWidget.SetVisible(false);
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0006B577 File Offset: 0x00069777
		protected void LoadingScreenCallback()
		{
			this.mWidgetManager.BringToFront(this.mLoadingScreen);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0006B58C File Offset: 0x0006978C
		protected void SetupMainMenuDefaults(bool do_load_thread)
		{
			if (do_load_thread)
			{
				this.mLoadType = 4;
				this.mStartInGameModeThreadProcRunning = true;
				int num = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(700));
				int num2 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(650));
				Ratio aspectRatio = this.mGraphicsDriver.GetAspectRatio();
				if (aspectRatio.mNumerator != 4 && aspectRatio.mDenominator != 3)
				{
					ZumasRevenge.Common._DS(ZumasRevenge.Common._M(160));
				}
				this.StartMMThreadProc();
				this.DoCommonInGameLoadThread(new Rect((this.mWidth - num) / 2, (this.mHeight - num2) / 2, num, num2));
				this.mReturnToMMDlg = null;
			}
			else
			{
				this.StartMMThreadProc();
			}
			this.mWidgetManager.AddWidget(this.mMainMenu);
			this.ClearUpdateBacklog(true);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0006B649 File Offset: 0x00069849
		protected void SetupMainMenuDefaults()
		{
			this.SetupMainMenuDefaults(true);
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0006B654 File Offset: 0x00069854
		protected void DoCommonInGameLoadThread(Rect aRect)
		{
			this.mLoadRect = aRect;
			bool flag = false;
			while (this.mStartInGameModeThreadProcRunning)
			{
				this.UpdateAppStep(ref flag);
				SexyFramework.Common.SexySleep(0);
			}
			this.mWidgetManager.MarkAllDirty();
			if (this.mInGameLoadThreadProcFailed)
			{
				this.Popup("There was an error initializing the game.");
				this.mBoard.Dispose();
				this.mBoard = null;
				for (int i = 0; i < this.mNormalLevelMgr.mLevels.size<Level>(); i++)
				{
					this.mNormalLevelMgr.mLevels[i].mBoard = null;
				}
				if (this.mWidescreenBoardWidget != null)
				{
					this.mWidgetManager.RemoveWidget(this.mWidescreenBoardWidget);
					base.SafeDeleteWidget(this.mWidescreenBoardWidget);
					this.mWidescreenBoardWidget = null;
				}
				this.Shutdown();
				return;
			}
			if (this.mLoadType != 4)
			{
				this.mWidgetManager.AddWidget(this.mBoard);
				this.mWidgetManager.SetFocus(this.mBoard);
				if (this.mWidescreenBoardWidget == null)
				{
					this.mWidescreenBoardWidget = new WidescreenBoardWidget();
					this.mWidescreenBoardWidget.Resize(ZumasRevenge.Common._S(-80), 0, this.mWidth + ZumasRevenge.Common._S(160), this.mHeight);
					this.mWidgetManager.AddWidget(this.mWidescreenBoardWidget);
				}
			}
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0006B794 File Offset: 0x00069994
		public GameApp(Game xnaGame, bool from_reinit)
		{
			GameApp.gApp = this;
			this.mGameMain = xnaGame;
			((WP7AppDriver)this.mAppDriver).InitXNADriver(xnaGame);
			this.SetBoolean("drivers.ios.use_gles20", true);
			this.SetBoolean("drivers.ios.use_multitouch", false);
			this.SetInteger("compat_AppOrigScreenWidth", this.mOrigScreenWidth);
			this.SetInteger("compat_AppOrigScreenHeight", this.mOrigScreenHeight);
			this.mSavingOrLoadingProfile = false;
			this.mWideScreenXOffset = 0;
			this.mUpsell = null;
			this.mDoingDRM = false;
			this.mTrialType = 0;
			this.mShotCorrectionAngleToWidthDist = 1500f;
			this.mShotCorrectionAngleMax = 13f;
			this.mShotCorrectionWidthMax = 65f;
			this.mGuideStyle = 1;
			this.mShotCorrectionDebugStyle = 3;
			this.mIronFrogModeIncluded = false;
			this.mGenericHelp = null;
			this.mLegalInfo = null;
			this.mAboutInfo = null;
			this.mProdName = "ZumasRevenge";
			this.mRegKey = "PopCap\\ZumasRevenge";
			this.mLevelXML = "levels/levels";
			this.mHardLevelXML = "levels/levels_hard";
			this.mBoard = null;
			this.mDebugKeysEnabled = false;
			this.mAllowSwapScreenImage = false;
			this.mLoadType = -1;
			this.mCredits = null;
			this.mIFLoadingAnimStartCel = 0;
			this.mDelayIntro = false;
			this.mReturnToMMDlg = null;
			this.mDoingAdvModeLoad = false;
			this.mConfTime = 1500;
			GameApp.mGameRes = 640;
			this.mHiRes = false;
			this.mWidescreenAware = true;
			this.mWidescreenTranslate = true;
			this.mAllowWindowResize = true;
			this.mReInit = false;
			this.mFromReInit = from_reinit;
			this.mMapScreen = null;
			this.mMapScreenHackWidget = null;
			this.mInGameLoadThreadProcFailed = false;
			this.mForceZoneRestart = -1;
			this.mStartInGameModeThreadProcRunning = false;
			this.mClickedHardMode = false;
			this.mContinuedGame = false;
			GameApp.gNeedsPreCache = true;
			this.mAutoMonkey = null;
			GameApp.initResolution(640);
			this.mAutoStartLoadingThread = false;
			this.mLoadingScreen = null;
			this.mFramesPlayed = 0;
			this.mAutoEnable3D = true;
			this.mNoVSync = true;
			this.mCachedLoadState = 0;
			this.mCachedLoad = false;
			this.mNormalLevelMgr = null;
			this.mCustomCursorsEnabled = true;
			this.mCursorTarget = true;
			this.mColorblind = false;
			this.mUserProfile = null;
			this.mProfileMgr = null;
			this.mMainMenu = null;
			this.mMoreGames = null;
			this.mNewUserDlg = null;
			this.mUnderDialogWidget = new UnderDialogWidget();
			this.mDialogObscurePct = 0f;
			this.mFullscreenBits = 32;
			this.mInitialLoad = true;
			GameApp.gDDS = new DDS();
			GameApp.gDDS.mMinLevel = int.MaxValue;
			this.mBambooTransition = null;
			this.mProductVersion = this.GetProductVersion("");
			if (!this.mFileDriver.InitFileDriver(this))
			{
				this.Shutdown();
			}
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0006BAD8 File Offset: 0x00069CD8
		public string GetProductVersion(string thePath)
		{
			string fullName = Assembly.GetCallingAssembly().FullName;
			string text = "v" + fullName.Split(new char[] { '=' })[1].Split(new char[] { ',' })[0];
			return text.Substring(0, text.Length - 2);
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0006BB34 File Offset: 0x00069D34
		public override void Dispose()
		{
			if (this.mSoundManager != null)
			{
				this.mSoundManager.ReleaseChannels();
				this.mSoundManager.ReleaseSounds();
			}
			if (this.mGenericHelp != null)
			{
				this.KillDialog(this.mGenericHelp);
				this.mGenericHelp = null;
			}
			if (this.mLegalInfo != null)
			{
				this.KillDialog(this.mLegalInfo);
				this.mLegalInfo = null;
			}
			if (this.mAboutInfo != null)
			{
				this.KillDialog(this.mAboutInfo);
				this.mAboutInfo = null;
			}
			if (this.mBambooTransition != null)
			{
				this.mWidgetManager.RemoveWidget(this.mBambooTransition);
				this.mBambooTransition = null;
			}
			if (this.mUpsell != null)
			{
				this.mWidgetManager.RemoveWidget(this.mUpsell);
				this.mUpsell = null;
			}
			if (this.gCreditsHackWidget != null)
			{
				this.mWidgetManager.RemoveWidget(this.gCreditsHackWidget);
			}
			this.gCreditsHackWidget = null;
			this.mWidgetManager.RemoveWidget(this.mUnderDialogWidget);
			this.mUnderDialogWidget = null;
			this.mCredits = null;
			Ball.DeleteBallGlobals();
			if (this.mBoard != null)
			{
				if (this.mBoard.NeedSaveGame() && this.mUserProfile != null)
				{
					this.mBoard.SaveGame(this.mUserProfile.GetSaveGameName(this.IsHardMode()), null);
				}
				this.mWidgetManager.RemoveWidget(this.mBoard);
			}
			this.mReturnToMMDlg = null;
			this.mProxBombManager = null;
			this.mLevelThumbnails.Clear();
			this.mMusic = null;
			this.mSoundPlayer = null;
			this.mBoard = null;
			if (this.mNormalLevelMgr != null)
			{
				for (int i = 0; i < this.mNormalLevelMgr.mLevels.size<Level>(); i++)
				{
					this.mNormalLevelMgr.mLevels[i].mBoard = null;
				}
			}
			if (this.mMapScreen != null)
			{
				this.mMapScreen.CleanButtons();
			}
			if (this.mMapScreenHackWidget != null)
			{
				this.mWidgetManager.RemoveWidget(this.mMapScreenHackWidget);
			}
			this.mMapScreenHackWidget = null;
			this.mMapScreen = null;
			if (this.mMainMenu != null)
			{
				this.mWidgetManager.RemoveWidget(this.mMainMenu);
			}
			this.mMainMenu = null;
			if (this.mMoreGames != null)
			{
				this.mWidgetManager.RemoveWidget(this.mMoreGames);
			}
			this.mMoreGames = null;
			if (this.mLoadingScreen != null)
			{
				this.mWidgetManager.RemoveWidget(this.mLoadingScreen);
			}
			this.mLoadingScreen = null;
			this.mNormalLevelMgr = null;
			if (this.mNewUserDlg != null)
			{
				this.KillDialog(this.mNewUserDlg.mId, true, false);
			}
			this.mNewUserDlg = null;
			GameApp.gDDS = null;
			for (int j = 0; j < this.mCachedTorchEffects.size<CachedTorchEffect>(); j++)
			{
				this.mCachedTorchEffects[j].mTorchFlame = null;
				this.mCachedTorchEffects[j].mTorchFlameOut = null;
			}
			for (int k = 0; k < this.mCachedVolcanoEffects.size<CachedVolcanoEffect>(); k++)
			{
				this.mCachedVolcanoEffects[k].mExplosion = null;
				this.mCachedVolcanoEffects[k].mProjectile = null;
			}
			this.mResourceManager.DeleteResources("");
			this.mProfileMgr = null;
			this.RegistryWriteBoolean("LastShutdownOK", true);
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0006BE4C File Offset: 0x0006A04C
		public bool IsWideScreen()
		{
			return true;
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x0006BE4F File Offset: 0x0006A04F
		public int GetWideScreenAdjusted(int x)
		{
			return x;
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0006BE52 File Offset: 0x0006A052
		public int GetWidthAdjusted(int x)
		{
			return x - ZumasRevenge.Common._DS(125);
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0006BE60 File Offset: 0x0006A060
		public bool LoadMoreGamesInfo()
		{
			SexyBuffer buffer = new SexyBuffer();
			if (base.ReadBufferFromFile(SexyFramework.Common.GetAppDataFolder() + "users/mg.dat", ref buffer))
			{
				this.mLastMoreGamesUpdate = buffer.ReadLong();
				return true;
			}
			return false;
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0006BE9B File Offset: 0x0006A09B
		public void SaveMoreGamesInfo()
		{
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0006BE9D File Offset: 0x0006A09D
		public void ConsoleCallback(string cmd, List<string> _params)
		{
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0006BE9F File Offset: 0x0006A09F
		public void SaveProfile()
		{
			if (!this.mSavingOrLoadingProfile && this.mUserProfile != null)
			{
				this.mSavingOrLoadingProfile = true;
				this.mUserProfile.SaveDetails();
				this.mSavingOrLoadingProfile = false;
			}
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x0006BECC File Offset: 0x0006A0CC
		public bool HasSaveGame()
		{
			if (this.mUserProfile == null)
			{
				return false;
			}
			string saveGameName = this.mUserProfile.GetSaveGameName(this.IsHardMode());
			return StorageFile.FileExists(saveGameName);
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0006BF00 File Offset: 0x0006A100
		public void HandleCrash(bool from_assert)
		{
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x0006BF04 File Offset: 0x0006A104
		public void SaveGlobalConfig()
		{
			SexyBuffer buffer = new SexyBuffer();
			buffer.WriteDouble(this.mMusicVolume);
			buffer.WriteDouble(this.mSfxVolume);
			buffer.WriteBoolean(this.mColorblind);
			StorageFile.WriteBufferToFile("users/OptionConfig.sav", buffer);
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0006BF48 File Offset: 0x0006A148
		public void LoadGlobalConfig()
		{
			SexyBuffer buffer = new SexyBuffer();
			if (StorageFile.ReadBufferFromFile("users/OptionConfig.sav", buffer))
			{
				this.mMusicVolume = buffer.ReadDouble();
				this.mSfxVolume = buffer.ReadDouble();
				this.mColorblind = buffer.ReadBoolean();
				this.SetMusicVolume(this.mMusicVolume);
				this.SetSfxVolume(this.mSfxVolume);
			}
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0006BFA4 File Offset: 0x0006A1A4
		public void RevertOptionsChanges()
		{
			bool theValue = false;
			bool is3d = false;
			bool wantWindowed = false;
			this.RegistryReadBoolean("PreHiRes", ref theValue);
			this.RegistryReadBoolean("Pre3D", ref is3d);
			this.RegistryReadBoolean("PreWindowed", ref wantWindowed);
			this.RegistryWriteBoolean("NeedsConfirmation", false);
			this.SwitchScreenMode(wantWindowed, is3d, true);
			this.mPreferredWidth = (this.mPreferredHeight = -1);
			this.RegistryWriteBoolean("HiRes", theValue);
			this.mReInit = true;
			this.Shutdown();
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x0006C024 File Offset: 0x0006A224
		public void InGameLoadThread_DrawFunc()
		{
			GameApp.InGameLoadThread_DrawFunc_CallCounter++;
			Font fontByName = this.GetFontByName("FONT_SHAGLOUNGE38_STROKE");
			Image imageByName = this.GetImageByName("IMAGE_BLUE_BALL");
			Graphics graphics = new Graphics(this.mWidgetManager.mImage);
			string text = ((this.mLoadType == 4) ? TextManager.getInstance().getString(726) : TextManager.getInstance().getString(581));
			string text2 = "";
			int num = GameApp.InGameLoadThread_DrawFunc_CallCounter % 40;
			if (num >= 30)
			{
				text2 = "...";
			}
			else if (num >= 20)
			{
				text2 = "..";
			}
			else if (num >= 10)
			{
				text2 = ".";
			}
			text += text2;
			if (this.mLoadType == 1 || this.mLoadType == 0)
			{
				Ratio aspectRatio = this.mGraphicsDriver.GetAspectRatio();
				int num2 = ((aspectRatio.mNumerator != 4 && aspectRatio.mDenominator != 3) ? ZumasRevenge.Common._DS(ZumasRevenge.Common._M(160)) : 0);
				if (this.mLoadType == 1)
				{
					int num3 = ((aspectRatio.mNumerator != 4 && aspectRatio.mDenominator != 3) ? ZumasRevenge.Common._DS(ZumasRevenge.Common._M(80)) : 0);
					int num4 = this.mLoadRect.mX + (this.mLoadRect.mWidth - fontByName.StringWidth("Loading...")) / 2;
					num4 += num3;
					int num5 = this.mLoadRect.mY + (this.mLoadRect.mHeight - fontByName.mHeight) / 2 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(50));
					graphics.SetColor(250, 124, 0);
					graphics.SetFont(fontByName);
					graphics.DrawString(text, num4, num5);
					return;
				}
				if (this.mLoadType == 0)
				{
					int num4 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(656)) + (ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(330)) - fontByName.StringWidth("Loading...")) / 2 - 2;
					num4 += num2;
					int num5 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(697)) + (ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(500)) - fontByName.mHeight) / 2 - 2;
					graphics.SetColor(SexyColor.White);
					graphics.SetFont(fontByName);
					graphics.DrawString(text, num4 + 2, num5 + graphics.GetFont().GetAscent() + 2);
					return;
				}
			}
			else
			{
				if (this.mLoadType == 2)
				{
					graphics.SetFont(fontByName);
					graphics.SetColor(250, 124, 0);
					graphics.DrawString(text, this.mLoadRect.mX + (this.mLoadRect.mWidth - graphics.GetFont().StringWidth("Loading...")) / 2, this.mLoadRect.mY + (this.mLoadRect.mHeight - graphics.GetFont().mHeight) / 2 + graphics.GetFont().GetAscent());
					return;
				}
				if (this.mLoadType == 3)
				{
					graphics.SetFont(fontByName);
					graphics.SetColor(SexyColor.White);
					return;
				}
				if (this.mLoadType == 4)
				{
					graphics.Translate(this.mReturnToMMDlg.mX, this.mReturnToMMDlg.mY);
					this.mReturnToMMDlg.Draw(graphics);
					graphics.SetFont(fontByName);
					graphics.SetColor(250, 124, 0);
					graphics.DrawString(text, (this.mLoadRect.mWidth - graphics.GetFont().StringWidth("Returning to Menu...")) / 2 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(20)), (this.mLoadRect.mHeight - graphics.GetFont().mHeight) / 2 - ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(30)) + graphics.GetFont().GetAscent());
					int theY = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(400));
					int num6 = this.mLoadRect.mWidth - imageByName.GetCelWidth() * 4 - ZumasRevenge.Common._DS(ZumasRevenge.Common._M(-100));
					int num7 = num6 / 4;
					Image[] array = new Image[]
					{
						this.GetImageByName("IMAGE_BLUE_BALL"),
						this.GetImageByName("IMAGE_RED_BALL"),
						this.GetImageByName("IMAGE_YELLOW_BALL"),
						this.GetImageByName("IMAGE_GREEN_BALL")
					};
					int[] array2 = new int[]
					{
						SexyFramework.Common.Rand(50),
						SexyFramework.Common.Rand(50),
						SexyFramework.Common.Rand(50),
						SexyFramework.Common.Rand(50)
					};
					for (int i = 0; i < 4; i++)
					{
						int num8 = array[i].mNumCols * array[i].mNumRows;
						int num9 = (array2[i] + GameApp.InGameLoadThread_DrawFunc_CallCounter) % num8;
						if (num9 < 0)
						{
							num9 = -num9;
						}
						else if (num9 >= num8)
						{
							num9 = num8 - 1;
						}
						Rect theSrcRect = new Rect(array[i].GetCelRect(num9));
						graphics.DrawImageRotated(array[i], num7 + num6 / 4 * i, theY, -1.5707963705062866, theSrcRect);
					}
				}
			}
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0006C4F4 File Offset: 0x0006A6F4
		public void StartAdvModeThreadProc()
		{
			this.mBoard = new Board(this, -1);
			this.mBoard.mAdventureMode = true;
			this.mBoard.mIsHardMode = this.mClickedHardMode;
			if (!this.mBoard.Init())
			{
				this.mInGameLoadThreadProcFailed = true;
				this.mStartInGameModeThreadProcRunning = false;
				return;
			}
			this.mBoard.Resize(0, 0, this.mWidth, this.mHeight);
			this.mContinuedGame = false;
			if (this.HasSaveGame() && this.mForceZoneRestart == -1)
			{
				if (!this.mBoard.LoadGame(this.mUserProfile.GetSaveGameName(this.IsHardMode())))
				{
					StorageFile.DeleteFile(this.mUserProfile.GetSaveGameName(this.IsHardMode()));
					this.mUserProfile.ClearAdventureModeDetails();
				}
				else
				{
					this.mContinuedGame = true;
				}
			}
			else
			{
				this.PlaySong(12);
				if (this.mForceZoneRestart != -1)
				{
					this.mBoard.RestartFromZone(this.mForceZoneRestart);
				}
				else if (!this.mBoard.StartLevel(1))
				{
					this.mInGameLoadThreadProcFailed = true;
					this.mStartInGameModeThreadProcRunning = false;
					return;
				}
			}
			this.mBoard.MakeCachedBackground();
			this.mInGameLoadThreadProcFailed = false;
			this.mForceZoneRestart = -1;
			this.mStartInGameModeThreadProcRunning = false;
			this.mAutoMonkey.SetState(MonkeyState.Playing);
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0006C630 File Offset: 0x0006A830
		public void StartChallengeModeThreadProc()
		{
			this.mBoard = new Board(this, this.mNormalLevelMgr.GetStartingGauntletLevel(this.mChallengeLevelId));
			if (!this.mBoard.Init())
			{
				this.mInGameLoadThreadProcFailed = true;
				this.mStartInGameModeThreadProcRunning = false;
				return;
			}
			this.mBoard.Resize(0, 0, this.mWidth, this.mHeight);
			if (!this.mBoard.StartLevel(this.mChallengeLevelId))
			{
				this.mInGameLoadThreadProcFailed = true;
				this.mStartInGameModeThreadProcRunning = false;
				return;
			}
			this.mInGameLoadThreadProcFailed = false;
			this.mStartInGameModeThreadProcRunning = false;
			this.mAutoMonkey.SetState(MonkeyState.Playing);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x0006C6CC File Offset: 0x0006A8CC
		public void StartIronFrogModeThreadProc()
		{
			this.mBoard = new Board(this, -1);
			if (!this.mBoard.Init())
			{
				this.mInGameLoadThreadProcFailed = true;
				this.mStartInGameModeThreadProcRunning = false;
				return;
			}
			this.mBoard.Resize(0, 0, this.mWidth, this.mHeight);
			if (!this.mBoard.StartLevel(this.mNormalLevelMgr.GetFirstIronFrogLevel() + 1))
			{
				this.mInGameLoadThreadProcFailed = true;
				this.mStartInGameModeThreadProcRunning = false;
				return;
			}
			this.mInGameLoadThreadProcFailed = false;
			this.mStartInGameModeThreadProcRunning = false;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0006C754 File Offset: 0x0006A954
		public void StartMMThreadProc()
		{
			if (!this.mResourceManager.IsGroupLoaded("MenuRelated") && !this.mResourceManager.LoadResources("MenuRelated"))
			{
				this.mStartInGameModeThreadProcRunning = false;
				this.mInGameLoadThreadProcFailed = true;
				return;
			}
			if (this.mResourceManager.IsGroupLoaded("GrottoSounds"))
			{
				this.mResourceManager.DeleteResources("GrottoSounds");
			}
			if (this.mResourceManager.IsGroupLoaded("Boss6Common"))
			{
				this.mResourceManager.DeleteResources("Boss6Common");
			}
			this.mMainMenu = new MainMenu(this);
			this.mMainMenu.Init();
			this.mMainMenu.Resize(this.GetScreenRect());
			this.mInGameLoadThreadProcFailed = false;
			this.mStartInGameModeThreadProcRunning = false;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0006C80E File Offset: 0x0006AA0E
		public void DoUpsell(bool from_exit)
		{
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0006C810 File Offset: 0x0006AA10
		public bool IsRegistered()
		{
			return true;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0006C813 File Offset: 0x0006AA13
		public bool IsSafeForLockout()
		{
			return !this.mLoadingThreadStarted || this.mLoadingThreadCompleted;
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x0006C825 File Offset: 0x0006AA25
		public void DoLockout()
		{
			this.mDoingDRM = true;
			if (this.mBoard != null)
			{
				this.mBoard.DoShutdownSaveGame();
			}
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0006C844 File Offset: 0x0006AA44
		public void DoCredits(bool isFromMainMenu)
		{
			if (!this.mResourceManager.IsGroupLoaded("Credits") && !this.mResourceManager.LoadResources("Credits"))
			{
				this.ShowResourceError(true);
				this.Shutdown();
				return;
			}
			this.mCredits = new Credits(isFromMainMenu);
			this.mCredits.Init(this.mBoard != null && !this.mBoard.IsHardAdventureMode());
			this.gCreditsHackWidget = new CreditsHackWidget();
			this.gCreditsHackWidget.Resize(0, 0, this.mWidth, this.mHeight);
			this.gCreditsHackWidget.mClip = false;
			this.mWidgetManager.AddWidget(this.gCreditsHackWidget);
			if (!isFromMainMenu)
			{
				this.EndCurrentGame();
			}
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0006C900 File Offset: 0x0006AB00
		public void ReturnFromCredits()
		{
			if (!this.mCredits.mFromMainMenu)
			{
				this.ShowMainMenu();
			}
			this.mWidgetManager.RemoveWidget(this.gCreditsHackWidget);
			base.SafeDeleteWidget(this.gCreditsHackWidget);
			this.mCredits.Dispose();
			this.mCredits = null;
			this.gCreditsHackWidget = null;
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0006C956 File Offset: 0x0006AB56
		public void GenericHelpClosed()
		{
			this.mGenericHelp = null;
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0006C95F File Offset: 0x0006AB5F
		public void SetStat(string stat_name, int val)
		{
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x0006C961 File Offset: 0x0006AB61
		public int GetStat(string stat_name)
		{
			return 0;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0006C964 File Offset: 0x0006AB64
		public void SetAchievement(string achievement_name)
		{
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0006C966 File Offset: 0x0006AB66
		public void ResetAchievements()
		{
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0006C968 File Offset: 0x0006AB68
		public void RehupAchievements()
		{
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0006C96A File Offset: 0x0006AB6A
		public virtual void ConvertResources()
		{
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0006C96C File Offset: 0x0006AB6C
		public virtual void ConvertLevels()
		{
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0006C96E File Offset: 0x0006AB6E
		public void OnHardwareBackButtonPressed()
		{
			GlobalMembers.IsBackButtonPressed = true;
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0006C976 File Offset: 0x0006AB76
		public void OnHardwareBackButtonPressProcessed()
		{
			GlobalMembers.IsBackButtonPressed = false;
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0006C97E File Offset: 0x0006AB7E
		public void OnExiting()
		{
			if (this.mBoard != null)
			{
				this.mBoard.ProcessExitingEvent();
			}
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0006C993 File Offset: 0x0006AB93
		public void OnDeactivated()
		{
			if (this.mMusicInterface != null)
			{
				this.mMusicInterface.PauseAllMusic();
				this.mMusicInterface.OnDeactived();
			}
			if (this.mBoard != null)
			{
				this.mBoard.ProcessOnDeactiveEvent();
			}
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0006C9C6 File Offset: 0x0006ABC6
		public void OnActivated()
		{
			if (this.mMusicInterface != null)
			{
				this.mMusicInterface.OnActived();
				this.mMusicInterface.ResumeAllMusic();
			}
			GameApp.USE_TRIAL_VERSION = Guide.IsTrialMode;
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0006C9F0 File Offset: 0x0006ABF0
		public void OnServiceActivated()
		{
			if (this.mMusicInterface != null)
			{
				this.mMusicInterface.OnServiceActived();
			}
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0006CA05 File Offset: 0x0006AC05
		public void OnServiceDeactivated()
		{
			if (this.mMusicInterface != null)
			{
				this.mMusicInterface.OnServiceDeactived();
			}
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0006CA1A File Offset: 0x0006AC1A
		public bool IsHardwareBackButtonPressed()
		{
			return GlobalMembers.IsBackButtonPressed;
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0006CA21 File Offset: 0x0006AC21
		public void InitText()
		{
			TextManager.getInstance().init();
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0006CA30 File Offset: 0x0006AC30
		public override void Init()
		{
			int num = GameApp.mGameRes;
			if (num != 320 && num != 640)
			{
				if (num == 768)
				{
					this.mWideScreenXOffset = ZumasRevenge.Common._DS(160);
				}
			}
			else
			{
				this.mWideScreenXOffset = 0;
			}
			this.mProfileMgr = new ZumaProfileMgr();
			this.mProfileManager = this.mProfileMgr;
			this.mAutoMonkey = new AutoMonkey(this);
			base.Init();
			Res.InitResources(this);
			this.mResourceManager.mBaseArtRes = GameApp.mGameRes;
			this.mResourceManager.mLeadArtRes = 1200;
			this.mResourceManager.mCurArtRes = GameApp.mGameRes;
			this.SetString("DIALOG_BUTTON_YES", TextManager.getInstance().getString(446));
			this.SetString("DIALOG_BUTTON_NO", TextManager.getInstance().getString(447));
			this.SetString("DIALOG_BUTTON_OK", TextManager.getInstance().getString(675));
			this.SetString("DIALOG_BUTTON_CANCEL", TextManager.getInstance().getString(454));
			this.mCachedLoad = false;
			this.InitAudio();
			this.PreShowLoadingScreen();
			this.LoadGlobalConfig();
			this.mInitFinished = true;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0006CB60 File Offset: 0x0006AD60
		public void StartThreadInit()
		{
			ThreadStart threadStart = new ThreadStart(this.Init);
			this.mInitThread = new Thread(threadStart);
			this.mInitThread.Start();
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0006CB92 File Offset: 0x0006AD92
		public override void InitHook()
		{
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0006CB94 File Offset: 0x0006AD94
		public override string NotifyCrashHook()
		{
			return base.NotifyCrashHook();
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0006CB9C File Offset: 0x0006AD9C
		public string GetCrashZipName(int num_override)
		{
			return "";
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0006CBA3 File Offset: 0x0006ADA3
		public string GetCrashZipName()
		{
			return this.GetCrashZipName(-1);
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0006CBAC File Offset: 0x0006ADAC
		protected void GamerSignedInCallback(object sender, SignedInEventArgs args)
		{
			SignedInGamer gamer = args.Gamer;
			if (gamer != null)
			{
				this.m_DefaultProfileName = gamer.Gamertag;
			}
			if (gamer.IsSignedInToLive)
			{
				if (this.m_XLiveState == GameApp.EXLiveWaiting.E_WaitingForSignIn)
				{
					gamer.BeginGetAchievements(new AsyncCallback(this.GetAchievementsCallback), gamer);
					this.m_XLiveState = GameApp.EXLiveWaiting.E_WaitingForAchivements;
				}
			}
			else
			{
				this.m_XLiveState = GameApp.EXLiveWaiting.E_NONE;
				if (this.IsFirstGameLoad(this.m_DefaultProfileName))
				{
					GameApp.gInitialProfLoadSuccessful = true;
					this.mUserProfile = (ZumaProfile)this.mProfileMgr.AddProfile(this.m_DefaultProfileName);
					GameApp.gDDS.ChangeProfile(this.mUserProfile);
				}
				else
				{
					this.mUserProfile = (ZumaProfile)GameApp.gApp.mProfileMgr.GetProfile(GameApp.gApp.m_DefaultProfileName);
				}
			}
			GameApp.USE_TRIAL_VERSION = Guide.IsTrialMode;
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0006CC78 File Offset: 0x0006AE78
		protected void GetAchievementsCallback(IAsyncResult result)
		{
			SignedInGamer signedInGamer = result.AsyncState as SignedInGamer;
			if (signedInGamer == null)
			{
				return;
			}
			if (this.mUserProfile == null)
			{
				this.mUserProfile = (ZumaProfile)GameApp.gApp.mProfileMgr.GetProfile(0);
			}
			try
			{
				this.mUserProfile.m_AchievementMgr.m_AchievementsXLive = signedInGamer.EndGetAchievements(result);
			}
			catch (Exception)
			{
			}
			this.m_XLiveState = GameApp.EXLiveWaiting.E_Ready;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0006CCEC File Offset: 0x0006AEEC
		public override void LoadingThreadProc()
		{
			if (this.mCachedLoadState > 1)
			{
				return;
			}
			GameApp.gInitialProfLoadSuccessful = this.mProfileMgr.Init();
			SignedInGamer.SignedIn += new EventHandler<SignedInEventArgs>(this.GamerSignedInCallback);
			int num = 70;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				Level levelByIndex;
				do
				{
					levelByIndex = this.mNormalLevelMgr.GetLevelByIndex(num2++);
				}
				while (levelByIndex != null && (levelByIndex.mBoss != null || JeffLib.Common.StrFindNoCase(levelByIndex.mId, "boss") != -1));
				if (levelByIndex == null)
				{
					break;
				}
				IdxThumbPair idxThumbPair = new IdxThumbPair();
				idxThumbPair.first = num2 - 1;
				idxThumbPair.second = null;
				this.mLevelThumbnails.Add(idxThumbPair);
			}
			this.mResourceManager.PrepareLoadResourcesList(GameApp.gInitialLoadGroups);
			this.mMusic.LoadMusic(12, "music/MUSIC_TUNE1");
			this.mMusic.LoadMusic(24, "music/MUSIC_TUNE2");
			this.mMusic.LoadMusic(35, "music/MUSIC_TUNE3");
			this.mMusic.LoadMusic(45, "music/MUSIC_TUNE4");
			this.mMusic.LoadMusic(58, "music/MUSIC_TUNE5");
			this.mMusic.LoadMusic(71, "music/MUSIC_TUNE6");
			this.mMusic.LoadMusic(120, "music/MUSIC_WON1");
			this.mMusic.LoadMusic(121, "music/MUSIC_WON2");
			this.mMusic.LoadMusic(122, "music/MUSIC_WON3");
			this.mMusic.LoadMusic(123, "music/MUSIC_WON4");
			this.mMusic.LoadMusic(124, "music/MUSIC_WON5");
			this.mMusic.LoadMusic(125, "music/MUSIC_WON6");
			this.mMusic.LoadMusic(127, "music/MUSIC_BOSS");
			this.mMusic.LoadMusic(144, "music/MUSIC_WON_GAME");
			this.mMusic.LoadMusic(126, "music/MUSIC_GAME_OVER");
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0006CECC File Offset: 0x0006B0CC
		public override void LoadingThreadCompleted()
		{
			base.LoadingThreadCompleted();
			Enumerable.Count<string>(GameApp.gInitialLoadGroups);
			this.mBambooTransition = new BambooTransition();
			this.mProxBombManager = new ProxBombManager();
			if (this.mCachedLoad)
			{
				this.mLoadingThreadCompleted = true;
				this.mLoaded = true;
				this.ShowMainMenu();
				return;
			}
			if (this.mLoadingFailed || this.mCachedLoadState > 1)
			{
				return;
			}
			this.mLoadingScreen.LoadingComplete();
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0006CF3A File Offset: 0x0006B13A
		public bool IsFinishedLoading()
		{
			return true;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0006CF3D File Offset: 0x0006B13D
		public void GameFinishedLoading()
		{
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0006CF40 File Offset: 0x0006B140
		public void StartLoading()
		{
			if (!this.mResourceManager.IsGroupLoaded("MainSounds"))
			{
				this.mResourceManager.LoadResources("MainSounds");
			}
			if (!this.mResourceManager.IsGroupLoaded("Text"))
			{
				this.mResourceManager.LoadResources("Text");
			}
			Font fontByName = this.GetFontByName("FONT_SHAGEXOTICA68_BASE");
			((ImageFont)fontByName).PushLayerColor("Stroke", new SexyColor(0, 0, 0, 255));
			((ImageFont)fontByName).PushLayerColor("Shadow", new SexyColor(0, 0, 0, 255));
			Font fontByName2 = this.GetFontByName("FONT_SHAGEXOTICA68_BLACK");
			((ImageFont)fontByName2).PushLayerColor("Main", new SexyColor(0, 0, 0, 255));
			Font fontByName3 = this.GetFontByName("FONT_SHAGEXOTICA68_STROKE");
			((ImageFont)fontByName3).PushLayerColor("Stroke", new SexyColor(0, 0, 0, 255));
			Font fontByName4 = this.GetFontByName("FONT_SHAGLOUNGE28_STROKE");
			((ImageFont)fontByName4).PushLayerColor("Stroke", new SexyColor(0, 0, 0, 255));
			Font fontByName5 = this.GetFontByName("FONT_SHAGEXOTICA38_BASE");
			((ImageFont)fontByName5).PushLayerColor("Stroke", new SexyColor(0, 0, 0, 255));
			((ImageFont)fontByName5).PushLayerColor("Shadow", new SexyColor(0, 0, 0, 255));
			Font fontByName6 = this.GetFontByName("FONT_SHAGEXOTICA38_BLACK");
			((ImageFont)fontByName6).PushLayerColor("Main", new SexyColor(0, 0, 0, 255));
			Font fontByName7 = this.GetFontByName("FONT_SHAGEXOTICA38_BLACK_GLOW");
			((ImageFont)fontByName7).PushLayerColor("Glow", new SexyColor(0, 0, 0, 255));
			Font fontByName8 = this.GetFontByName("FONT_SHAGEXOTICA38_GREEN_STROKE");
			((ImageFont)fontByName8).PushLayerColor("Stroke", new SexyColor(79, 91, 66, 255));
			Font fontByName9 = this.GetFontByName("FONT_SHAGEXOTICA100_BASE");
			((ImageFont)fontByName9).PushLayerColor("Stroke", new SexyColor(0, 0, 0, 255));
			((ImageFont)fontByName9).PushLayerColor("Shadow", new SexyColor(0, 0, 0, 255));
			Font fontByName10 = this.GetFontByName("FONT_SHAGEXOTICA100_STROKE");
			((ImageFont)fontByName10).PushLayerColor("Stroke", new SexyColor(0, 0, 0, 255));
			((ImageFont)fontByName10).PushLayerColor("Shadow", new SexyColor(0, 0, 0, 255));
			Font fontByName11 = this.GetFontByName("FONT_SHAGEXOTICA100_GAUNTLET");
			((ImageFont)fontByName11).PushLayerColor("Main", new SexyColor(85, 50, 160, 255));
			((ImageFont)fontByName11).PushLayerColor("Stroke", new SexyColor(248, 238, 195, 255));
			((ImageFont)fontByName11).PushLayerColor("Shadow", new SexyColor(235, 131, 130, 255));
			Font fontByName12 = this.GetFontByName("FONT_SHAGLOUNGE28_BASE");
			((ImageFont)fontByName12).PushLayerColor("Stroke", new SexyColor(0, 0, 0, 255));
			if (Localization.GetCurrentLanguage() != Localization.LanguageType.Language_RU && Localization.GetCurrentLanguage() != Localization.LanguageType.Language_PL)
			{
				((ImageFont)fontByName12).PushLayerColor("Shadow", new SexyColor(0, 0, 0, 255));
			}
			Font fontByName13 = this.GetFontByName("FONT_SHAGLOUNGE28_SHADOW");
			((ImageFont)fontByName13).PushLayerColor("Shadow", new SexyColor(0, 0, 0, 255));
			Font fontByName14 = this.GetFontByName("FONT_SHAGLOUNGE28_STROKE_GREEN");
			((ImageFont)fontByName14).PushLayerColor("Stroke", new SexyColor(80, 92, 67, 255));
			Font fontByName15 = this.GetFontByName("FONT_SHAGLOUNGE28_BROWN");
			((ImageFont)fontByName15).PushLayerColor("Main", new SexyColor(193, 145, 54, 255));
			((ImageFont)fontByName15).PushLayerColor("Stroke", new SexyColor(66, 45, 14, 255));
			((ImageFont)fontByName15).PushLayerColor("Shadow", new SexyColor(0, 0, 0, 255));
			Font fontByName16 = this.GetFontByName("FONT_SHAGLOUNGE28_GREEN");
			((ImageFont)fontByName16).PushLayerColor("Main", new SexyColor(165, 232, 25, 255));
			((ImageFont)fontByName16).PushLayerColor("Glow", new SexyColor(0, 0, 0, 255));
			Font fontByName17 = this.GetFontByName("FONT_SHAGLOUNGE38_BASE");
			((ImageFont)fontByName17).PushLayerColor("Stroke", new SexyColor(0, 0, 0, 255));
			((ImageFont)fontByName17).PushLayerColor("Shadow", new SexyColor(0, 0, 0, 255));
			Font fontByName18 = this.GetFontByName("FONT_SHAGLOUNGE38_STROKE");
			((ImageFont)fontByName18).PushLayerColor("Stroke", new SexyColor(0, 0, 0, 255));
			Font fontByName19 = this.GetFontByName("FONT_SHAGLOUNGE38_RED_STROKE_YELLOW");
			((ImageFont)fontByName19).PushLayerColor("Main", new SexyColor(218, 10, 9, 255));
			((ImageFont)fontByName19).PushLayerColor("Stroke", new SexyColor(248, 241, 135, 255));
			Font fontByName20 = this.GetFontByName("FONT_SHAGLOUNGE38_YELLOW");
			((ImageFont)fontByName20).PushLayerColor("Stroke", new SexyColor(247, 207, 0, 255));
			Font fontByName21 = this.GetFontByName("FONT_SHAGLOUNGE38_GAUNTLET");
			((ImageFont)fontByName21).PushLayerColor("Main", new SexyColor(249, 245, 188, 255));
			((ImageFont)fontByName21).PushLayerColor("Stroke", new SexyColor(88, 51, 159, 255));
			Font fontByName22 = this.GetFontByName("FONT_SHAGLOUNGE38_GAUNTLET2");
			((ImageFont)fontByName22).PushLayerColor("Main", new SexyColor(251, 245, 189, 255));
			((ImageFont)fontByName22).PushLayerColor("Stroke", new SexyColor(228, 39, 226, 255));
			Font fontByName23 = this.GetFontByName("FONT_SHAGLOUNGE45_BASE");
			((ImageFont)fontByName23).PushLayerColor("Stroke", new SexyColor(0, 0, 0, 255));
			((ImageFont)fontByName23).PushLayerColor("Shadow", new SexyColor(0, 0, 0, 255));
			Font fontByName24 = this.GetFontByName("FONT_SHAGLOUNGE45_GAUNTLET");
			((ImageFont)fontByName24).PushLayerColor("Main", new SexyColor(249, 245, 188, 255));
			((ImageFont)fontByName24).PushLayerColor("Stroke", new SexyColor(88, 51, 159, 255));
			Font fontByName25 = this.GetFontByName("FONT_SHAGLOUNGE45_RED");
			((ImageFont)fontByName25).PushLayerColor("Main", new SexyColor(183, 61, 47, 255));
			Font fontByName26 = this.GetFontByName("FONT_SHAGLOUNGE45_YELLOW");
			((ImageFont)fontByName26).PushLayerColor("Main", new SexyColor(222, 180, 8, 255));
			this.StartLoadingComplete = true;
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0006D679 File Offset: 0x0006B879
		public override void LostFocus()
		{
			if (this.mBoard != null && Board.gPauseOnLostFocus)
			{
				this.mBoard.Pause(true);
			}
			this.mMusic.Enable(false);
			this.SaveProfile();
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0006D6A8 File Offset: 0x0006B8A8
		public override void GotFocus()
		{
			this.DetectMusicSettings();
			if (this.mBoard != null && Board.gPauseOnLostFocus)
			{
				this.mBoard.Pause(false);
				this.mBoard.mNumPauseUpdatesToDo = ZumasRevenge.Common._M(50);
				this.mBoard.MarkDirty();
			}
			this.ReportAppLaunchInfo(4);
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0006D6FA File Offset: 0x0006B8FA
		public override bool DebugKeyDown(int key)
		{
			return false;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0006D700 File Offset: 0x0006B900
		public override void UpdateFrames()
		{
			this.mMusic.Update();
			this.mSoundPlayer.Update();
			base.UpdateFrames();
			this.TransitionFromLoadingScreen();
			if (this.mDialogMap.Count > 0)
			{
				if (this.mMainMenu != null && this.mMainMenu.mUserSelDlg != null)
				{
					this.mWidgetManager.PutBehind(this.mUnderDialogWidget, this.mMainMenu.mUserSelDlg);
				}
				else
				{
					this.mWidgetManager.PutBehind(this.mUnderDialogWidget, this.mDialogList.Last.Value);
				}
				if (this.mDialogObscurePct < 1f)
				{
					if (this.mBoard != null && this.mBoard.mDoingFirstTimeIntro)
					{
						this.mDialogObscurePct = Math.Min(ZumasRevenge.Common._M(0.9f), this.mDialogObscurePct + ZumasRevenge.Common._M1(0.06f));
					}
					else
					{
						this.mDialogObscurePct = Math.Min(1f, this.mDialogObscurePct + ZumasRevenge.Common._M(0.06f));
					}
				}
			}
			else
			{
				if (this.mBoard != null && this.mBoard.mDoingFirstTimeIntro)
				{
					this.mDialogObscurePct = Math.Max(0f, this.mDialogObscurePct - ZumasRevenge.Common._M(0.015f));
				}
				else
				{
					this.mDialogObscurePct = Math.Max(0f, this.mDialogObscurePct - ZumasRevenge.Common._M(0.06f));
				}
				if (this.mDialogObscurePct == 0f && this.mUnderDialogWidget.mVisible)
				{
					this.mUnderDialogWidget.SetVisible(false);
				}
			}
			if (this.m_XLiveState == GameApp.EXLiveWaiting.E_Ready)
			{
				this.m_XLiveState = GameApp.EXLiveWaiting.E_NONE;
				SignedInGamer signedInGamer = Gamer.SignedInGamers[0];
				if (signedInGamer != null)
				{
					this.m_DefaultProfileName = signedInGamer.Gamertag;
				}
				if (!this.IsFirstGameLoad(this.m_DefaultProfileName) || !this.IsFirstGameLoad(this.m_DefaultName))
				{
					if (!this.IsFirstGameLoad(this.m_DefaultName))
					{
						GameApp.gApp.mProfileMgr.RenameProfile(this.m_DefaultName, GameApp.gApp.m_DefaultProfileName);
					}
					this.mUserProfile = (ZumaProfile)GameApp.gApp.mProfileMgr.GetProfile(GameApp.gApp.m_DefaultProfileName);
					return;
				}
				GameApp.gInitialProfLoadSuccessful = true;
				this.mUserProfile = (ZumaProfile)this.mProfileMgr.AddProfile(this.m_DefaultProfileName);
				GameApp.gDDS.ChangeProfile(this.mUserProfile);
			}
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0006D958 File Offset: 0x0006BB58
		public virtual void PlaySamplePan(int theSoundNum, int thePan, int min_time)
		{
			SoundAttribs soundAttribs = new SoundAttribs();
			soundAttribs.pan = thePan;
			this.mSoundPlayer.Play(theSoundNum, soundAttribs);
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0006D97F File Offset: 0x0006BB7F
		public virtual void PlaySamplePan(int theSoundNum, int thePan)
		{
			this.PlaySamplePan(theSoundNum, thePan, 5);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0006D98A File Offset: 0x0006BB8A
		public override void PlaySample(int theSoundNum, int min_time)
		{
			this.mSoundPlayer.Play(theSoundNum);
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0006D998 File Offset: 0x0006BB98
		public override void PlaySample(int theSoundNum)
		{
			this.PlaySample(theSoundNum, 5);
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0006D9A4 File Offset: 0x0006BBA4
		public override void DialogButtonDepress(int dialog_id, int button_id)
		{
			if (dialog_id == 1)
			{
				if (this.mYesNoDialogDelegate != null)
				{
					this.mYesNoDialogDelegate(button_id);
					this.mDialog.Kill();
					if (this.mBoard != null)
					{
						this.mBoard.Pause(false, true);
					}
					if (Enumerable.Count<KeyValuePair<int, Dialog>>(this.mDialogMap) == 1)
					{
						this.mDialog.SetFocusWidgetToBoard();
					}
					this.mDialog.Kill();
					return;
				}
			}
			else if (dialog_id == 0)
			{
				((ZumaDialog)base.GetDialog(dialog_id)).Kill();
				if (this.mBoard != null)
				{
					this.mBoard.Pause(false, true);
				}
				if (this.mDialogCallBack != null)
				{
					this.mDialogCallBack();
					this.mDialogCallBack = null;
				}
			}
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0006DA54 File Offset: 0x0006BC54
		public override void SwitchScreenMode(bool wantWindowed, bool is3d, bool force)
		{
			base.SwitchScreenMode(wantWindowed, is3d, force);
			this.RegistryWriteBoolean("Is3D", is3d);
			if (this.mBoard != null)
			{
				this.mBoard.mNumPauseUpdatesToDo = ZumasRevenge.Common._M(10);
				this.mBoard.MarkDirty();
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0006DA91 File Offset: 0x0006BC91
		public override MusicInterface CreateMusicInterface()
		{
			if (this.mNoSoundNeeded)
			{
				return new MusicInterface();
			}
			return base.CreateMusicInterface();
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0006DAA7 File Offset: 0x0006BCA7
		public override void HandleCmdLineParam(string theParamName, string theParamValue)
		{
			base.HandleCmdLineParam(theParamName, theParamValue);
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0006DAB4 File Offset: 0x0006BCB4
		public override void AddDialog(int id, Dialog d)
		{
			GameApp.gAddingDlgID = id;
			base.AddDialog(id, d);
			GameApp.gAddingDlgID = -12345;
			if (id != 6)
			{
				foreach (Dialog dialog in this.mDialogList)
				{
					if (dialog != d)
					{
						DialogHideInfo dialogHideInfo = new DialogHideInfo();
						dialogHideInfo.mDialog = dialog;
						dialogHideInfo.mHideCount = 1;
						new KeyValuePair<int, DialogHideInfo>(dialog.mId, dialogHideInfo);
						DialogHideInfo dialogHideInfo2 = null;
						if (this.mDialogHideInfoMap != null)
						{
							if (this.mDialogHideInfoMap.TryGetValue(dialog.mId, out dialogHideInfo2))
							{
								dialogHideInfo2.mHideCount++;
							}
							else
							{
								this.mDialogHideInfoMap.Add(dialog.mId, dialogHideInfo);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0006DB88 File Offset: 0x0006BD88
		public override void AddDialog(Dialog theDialog)
		{
			base.AddDialog(theDialog);
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0006DB94 File Offset: 0x0006BD94
		public override bool KillDialog(int id, bool removeWidget, bool deleteWidget)
		{
			if (id != GameApp.gAddingDlgID)
			{
				List<int> list = new List<int>();
				if (this.mDialogHideInfoMap != null)
				{
					foreach (KeyValuePair<int, DialogHideInfo> keyValuePair in this.mDialogHideInfoMap)
					{
						if (--keyValuePair.Value.mHideCount == 0)
						{
							list.Add(keyValuePair.Key);
						}
					}
					for (int i = 0; i < Enumerable.Count<int>(list); i++)
					{
						this.mDialogHideInfoMap.Remove(list[i]);
					}
				}
			}
			return base.KillDialog(id, removeWidget, deleteWidget);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0006DC50 File Offset: 0x0006BE50
		public override bool KillDialog(int theDialogId)
		{
			return base.KillDialog(theDialogId);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0006DC59 File Offset: 0x0006BE59
		public override bool KillDialog(Dialog theDialog)
		{
			return base.KillDialog(theDialog);
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0006DC62 File Offset: 0x0006BE62
		public void InitAudio()
		{
			this.mMusic = new Music(this.mMusicInterface);
			this.mMusic.RegisterCallBack();
			this.mSoundPlayer = new SoundEffects(this.mSoundManager);
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0006DC91 File Offset: 0x0006BE91
		public bool MusicEnabled()
		{
			return !this.mMusicInterface.isPlayingUserMusic();
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0006DCA4 File Offset: 0x0006BEA4
		public void DetectMusicSettings()
		{
			Dialog dialog = base.GetDialog(2);
			if (dialog != null)
			{
				((OptionsDialog)dialog).DetectMusicSettings();
				return;
			}
			this.mMusic.Enable(this.MusicEnabled() && this.GetMusicVolume() > 0.0);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x0006DCF0 File Offset: 0x0006BEF0
		public void TransitionFromLoadingScreen()
		{
			if (this.mLoadingScreen == null)
			{
				return;
			}
			if (this.mDelayIntro)
			{
				this.LoadBoard();
				return;
			}
			if (this.mLoadingScreen.CanShowMenu() && !this.TriggerFirstProfileDialog())
			{
				this.ShowMainMenu();
				this.mSoundPlayer.Stop(Res.GetSoundByID(ResID.SOUND_SEAGULLS));
				this.mWidgetManager.BringToFront(this.mLoadingScreen);
				return;
			}
			if (this.mLoadingScreen.Done() && this.mNewUserDlg == null)
			{
				this.KillLoadingScreen();
				this.mSoundPlayer.Stop(this.GetSoundIDByName("SOUND_SEAGULLS"), true);
			}
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x0006DD89 File Offset: 0x0006BF89
		public void LoadBoard()
		{
			this.mDelayIntro = false;
			if (this.mBoard != null)
			{
				this.mWidgetManager.AddWidget(this.mBoard);
			}
			this.KillLoadingScreen();
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x0006DDB4 File Offset: 0x0006BFB4
		public void KillLoadingScreen()
		{
			if (this.mLoadingScreen == null)
			{
				return;
			}
			this.mWidgetManager.RemoveWidget(this.mLoadingScreen);
			this.mLoadingScreen.Dispose();
			this.mLoadingScreen = null;
			if (this.mResourceManager.IsGroupLoaded("LoadScreen"))
			{
				this.mResourceManager.DeleteResources("LoadScreen");
			}
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0006DE0F File Offset: 0x0006C00F
		public bool TriggerFirstProfileDialog()
		{
			return false;
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0006DE12 File Offset: 0x0006C012
		public bool IsFirstGameLoad()
		{
			return this.mProfileMgr.GetNumProfiles() == 0U;
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0006DE22 File Offset: 0x0006C022
		public bool IsFirstGameLoad(string name)
		{
			return !this.mProfileMgr.HasProfile(name);
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0006DE33 File Offset: 0x0006C033
		public LevelMgr GetLevelMgr()
		{
			if (this.mUserProfile == null || this.mBoard == null)
			{
				return this.mNormalLevelMgr;
			}
			return this.mNormalLevelMgr;
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0006DE52 File Offset: 0x0006C052
		public void ResetAllLevelMgrs()
		{
			this.mNormalLevelMgr.Reset();
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0006DE60 File Offset: 0x0006C060
		public bool ReloadAllLevelMgrs()
		{
			LevelMgr[] array = new LevelMgr[] { this.mNormalLevelMgr };
			for (int i = 0; i < 1; i++)
			{
				if (!array[i].LoadLevels(array[i].mLevelXML))
				{
					this.Popup(array[i].GetErrorText());
					this.Popup("Your boss DDS parameters were all reset. You should quit and restart.");
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0006DEBC File Offset: 0x0006C0BC
		public void ShowMainMenu(bool do_load_thread)
		{
			this.mClickedHardMode = false;
			this.PlaySong(1);
			if (this.mInitialLoad)
			{
				if (!GameApp.gApp.mResourceManager.IsGroupLoaded("MenuRelated") && !this.mResourceManager.LoadResources("MenuRelated"))
				{
					this.mStartInGameModeThreadProcRunning = false;
					this.mInGameLoadThreadProcFailed = true;
					return;
				}
				this.mMainMenu = new MainMenu(this);
				this.mMainMenu.Init();
				this.mMainMenu.Resize(this.GetScreenRect());
				this.mWidgetManager.AddWidget(this.mMainMenu);
				this.mWidgetManager.SetFocus(this.mMainMenu);
				this.mLoadingScreen.mMouseVisible = false;
				this.mInitialLoad = false;
				this.CheckForAppUpdate();
			}
			else
			{
				if (this.mUserProfile != null)
				{
					this.mUserProfile.mDoChallengeTrophyZoom = (this.mUserProfile.mDoChallengeAceTrophyZoom = false);
					this.mUserProfile.mDoChallengeAceCupComplete = (this.mUserProfile.mDoChallengeCupComplete = false);
					this.mUserProfile.mUnlockSparklesIdx1 = (this.mUserProfile.mUnlockSparklesIdx2 = -1);
				}
				this.SetupMainMenuDefaults(do_load_thread);
			}
			if (!GameApp.gApp.mResourceManager.IsGroupLoaded("Map"))
			{
				this.mResourceManager.PrepareLoadResources("Map");
			}
			if (!GameApp.gApp.mResourceManager.IsGroupLoaded("CommonGame"))
			{
				this.mResourceManager.PrepareLoadResources("CommonGame");
			}
			if (this.mUserProfile == null)
			{
				ZumaProfile zumaProfile = (ZumaProfile)this.mProfileMgr.GetAnyProfile();
				string text = "";
				if (zumaProfile != null)
				{
					text = zumaProfile.GetName();
				}
				this.RegistryReadString("LastUser", ref text);
				if (text.Length > 0)
				{
					if (!GameApp.gInitialProfLoadSuccessful || !this.ChangeUser(text))
					{
						if (this.mProfileMgr.GetNumProfiles() > 0U)
						{
							zumaProfile = (ZumaProfile)this.mProfileMgr.GetAnyProfile();
							if (zumaProfile != null)
							{
								this.mUserProfile = zumaProfile;
								this.ChangeUser(zumaProfile.GetName());
							}
							this.mMainMenu.DoChangeUserDialog();
							this.ClearUpdateBacklog(false);
						}
						else
						{
							if (!GameApp.gInitialProfLoadSuccessful && !this.mCachedLoad)
							{
								this.DoGenericDialog("ERROR", "One or more of your saved game files is\nincompatible with this version of the game.\nThey have been deleted.", true, null, 0);
							}
							this.DoNewUserDialog();
						}
					}
					this.mMainMenu.MarkDirty();
				}
				else
				{
					this.DoNewUserDialog();
					this.mMainMenu.MarkDirty();
				}
				this.mMainMenu.RehupButtons();
			}
			if (this.mUserProfile != null && this.mMainMenu.mChallengeMenu != null)
			{
				this.mMainMenu.mChallengeMenu.InitCS();
			}
			this.mMainMenu.RehupButtons();
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0006E14A File Offset: 0x0006C34A
		public void ShowMainMenu()
		{
			this.ShowMainMenu(true);
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0006E153 File Offset: 0x0006C353
		public void HideChallengeMenu()
		{
			if (this.mMainMenu != null && this.mMainMenu.mChallengeMenu != null)
			{
				this.mMainMenu.HideChallengeMenu();
			}
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0006E178 File Offset: 0x0006C378
		public void HideMainMenu(bool delete_resources)
		{
			if (this.mMainMenu != null)
			{
				if (this.mMainMenu.mChallengeMenu != null)
				{
					this.mMainMenu.HideChallengeMenu();
				}
				this.mWidgetManager.RemoveWidget(this.mMainMenu);
				base.SafeDeleteWidget(this.mMainMenu);
				this.mMainMenu = null;
			}
			this.HideAdventureModeMapScreen();
			if (this.mResourceManager.IsGroupLoaded("MenuRelated"))
			{
				this.mResourceManager.DeleteResources("MenuRelated");
			}
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0006E1F4 File Offset: 0x0006C3F4
		public void ShowMoreGames()
		{
			this.mMoreGames = new MoreGames(this);
			this.mMoreGames.Init();
			this.mMoreGames.Resize(GameApp.gApp.GetScreenRect());
			this.mWidgetManager.AddWidget(this.mMoreGames);
			this.mMainMenu.DoMoreGamesSlide(false);
			this.mMoreGames.DoSlide(true);
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0006E256 File Offset: 0x0006C456
		public void HideMoreGames()
		{
			if (this.mMoreGames != null)
			{
				this.mMoreGames.DoSlide(false);
			}
			this.mMainMenu.DoMoreGamesSlide(true);
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x0006E278 File Offset: 0x0006C478
		public void DeleteMoreGames(bool delete_resources)
		{
			if (this.mMoreGames != null)
			{
				this.mWidgetManager.RemoveWidget(this.mMoreGames);
				base.SafeDeleteWidget(this.mMoreGames);
				this.mMoreGames = null;
			}
			if (delete_resources)
			{
				this.mResourceManager.DeleteResources("MoreGames");
			}
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0006E2C4 File Offset: 0x0006C4C4
		public void ShowIronFrog()
		{
			this.SetupMainMenuDefaults();
			this.mMainMenu.mChallengeMenu.InitCS();
			this.mMainMenu.RehupButtons();
			this.mMainMenu.DoIronFrog(false);
			this.PlaySong(1);
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0006E2FA File Offset: 0x0006C4FA
		public void ShowChallengeSelector()
		{
			this.SetupMainMenuDefaults();
			this.mMainMenu.ShowChallengeMenu();
			this.mMainMenu.mChallengeMenu.mCueMainSong = true;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0006E320 File Offset: 0x0006C520
		public void ShowAdventureModeMapScreen()
		{
			if (!this.mResourceManager.IsGroupLoaded("Map") && !this.mResourceManager.LoadResources("Map"))
			{
				this.ShowResourceError(true);
				this.Shutdown();
				return;
			}
			if (!this.mResourceManager.IsGroupLoaded("GamePlay"))
			{
				this.mResourceManager.PrepareLoadResources("GamePlay");
			}
			this.mMapScreen = new MapScreen();
			this.mMapScreenHackWidget = new MapScreenHackWidget();
			this.mMapScreen.mParent = this.mMapScreenHackWidget;
			this.mWidgetManager.AddWidget(this.mMapScreenHackWidget);
			this.mMapScreenHackWidget.Resize(0, 0, this.mWidth, this.mHeight);
			this.mMapScreen.Init(false, this.mUserProfile.GetAdvModeVars().mCurrentAdvZone, this.mUserProfile.GetAdvModeVars().mCurrentAdvLevel, false, true);
			this.mWidgetManager.SetFocus(this.mMapScreenHackWidget);
			this.mMapScreen.DoSlide(true);
			if (this.mMainMenu != null)
			{
				this.mMainMenu.HideScrollButtons();
			}
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0006E430 File Offset: 0x0006C630
		public void HideAdventureModeMapScreen()
		{
			if (this.mMapScreenHackWidget != null)
			{
				this.mWidgetManager.RemoveWidget(this.mMapScreenHackWidget);
				base.SafeDeleteWidget(this.mMapScreenHackWidget);
				this.mMapScreenHackWidget = null;
			}
			if (this.mMapScreen != null)
			{
				this.mMapScreen.Dispose();
				this.mMapScreen = null;
			}
			if ((this.mUserProfile == null || !this.mUserProfile.mNeedsFirstTimeIntro) && this.mResourceManager.IsGroupLoaded("Map"))
			{
				this.mResourceManager.DeleteResources("Map");
			}
			if (this.mMainMenu != null)
			{
				this.mMainMenu.ShowScrollButtons();
				this.PlaySong(1);
			}
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0006E4DC File Offset: 0x0006C6DC
		public void StartAdventureMode()
		{
			if (this.mStartInGameModeThreadProcRunning)
			{
				return;
			}
			this.PlaySong(12);
			this.mLoadType = ((this.mForceZoneRestart == -1) ? 0 : 1);
			if (this.IsHardMode())
			{
				this.mUserProfile.mFirstTimeReplayingHardMode = false;
			}
			else
			{
				this.mUserProfile.mFirstTimeReplayingNormalMode = false;
			}
			this.mStartInGameModeThreadProcRunning = true;
			this.StartAdvModeThreadProc();
			Rect aRect;
			if (this.mLoadType == 1)
			{
				int mX = this.mMapScreen.mCards[this.mMapScreen.mSelectedZone - 1].mX;
				int mY = this.mMapScreen.mCards[this.mMapScreen.mSelectedZone - 1].mY;
				Image imageByName = this.GetImageByName("IMAGE_UI_CHALLENGESCREEN_HOME_SELECT");
				aRect = new Rect(mX, mY, (int)(0.4f * (float)imageByName.mWidth), (int)(0.4f * (float)imageByName.mHeight));
			}
			else
			{
				aRect = new Rect(ZumasRevenge.Common._DS(ZumasRevenge.Common._M(624)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(697)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M2(700)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M3(500)));
			}
			Ratio aspectRatio = this.mGraphicsDriver.GetAspectRatio();
			int num = ((aspectRatio.mNumerator != 4 && aspectRatio.mDenominator != 3) ? ZumasRevenge.Common._DS(ZumasRevenge.Common._M(160)) : 0);
			aRect.mWidth += num;
			this.DoCommonInGameLoadThread(aRect);
			this.mBoard.AdventureModeSetupComplete(this.mContinuedGame);
			this.HideMainMenu(true);
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x0006E668 File Offset: 0x0006C868
		public void StartAdvModeFirstTime()
		{
			if (!this.mResourceManager.IsGroupLoaded("MapZoom") && !this.mResourceManager.LoadResources("MapZoom"))
			{
				this.ShowResourceError(true);
				this.Shutdown();
				return;
			}
			if (!this.mResourceManager.IsGroupLoaded("Text") && !this.mResourceManager.LoadResources("Text"))
			{
				this.ShowResourceError(true);
				this.Shutdown();
				return;
			}
			this.mMusic.FadeOut();
			this.HideMainMenu(true);
			this.mBoard = new Board(this, -1);
			if (this.mLoadingScreen == null)
			{
				this.mWidgetManager.AddWidget(this.mBoard);
			}
			this.mBoard.mAdventureMode = true;
			if (!this.mBoard.Init(true))
			{
				this.mInGameLoadThreadProcFailed = true;
				this.mStartInGameModeThreadProcRunning = false;
				return;
			}
			this.mBoard.Resize(0, 0, this.mWidth, this.mHeight);
			this.mContinuedGame = false;
			this.mBoard.StartLevel(1);
			this.mBoard.MakeCachedBackground();
			this.mWidgetManager.SetFocus(this.mBoard);
			if (this.mWidescreenBoardWidget == null)
			{
				this.mWidescreenBoardWidget = new WidescreenBoardWidget();
				this.mWidescreenBoardWidget.Resize(ZumasRevenge.Common._S(-80), 0, this.mWidth + ZumasRevenge.Common._S(160), this.mHeight);
				this.mWidgetManager.AddWidget(this.mWidescreenBoardWidget);
			}
			this.mBoard.SetMenuBtnEnabled(false);
			this.mAutoMonkey.SetState(MonkeyState.Playing);
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0006E7E9 File Offset: 0x0006C9E9
		public void DoDeferredEndGame()
		{
			if (this.mBoard != null)
			{
				this.mBoard.mNumDrawFramesLeft = ZumasRevenge.Common._M(2);
				this.mBoard.mReturnToMainMenu = true;
			}
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x0006E810 File Offset: 0x0006CA10
		public void EndCurrentGame()
		{
			this.mBoard.DoShutdownSaveGame();
			this.mBoard.mSkipShutdownSave = true;
			this.mWidgetManager.RemoveWidget(this.mBoard);
			base.SafeDeleteWidget(this.mBoard);
			this.mBoard = null;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0006E850 File Offset: 0x0006CA50
		public void StartGauntletMode(string normal_level_id, Rect thumb_rect)
		{
			if (this.mStartInGameModeThreadProcRunning)
			{
				return;
			}
			this.mLoadType = 2;
			this.mChallengeLevelId = normal_level_id;
			this.mStartInGameModeThreadProcRunning = true;
			this.StartChallengeModeThreadProc();
			Rect aRect = new Rect(thumb_rect);
			Ratio aspectRatio = this.mGraphicsDriver.GetAspectRatio();
			int num = ((aspectRatio.mNumerator != 4 && aspectRatio.mDenominator != 3) ? ZumasRevenge.Common._DS(ZumasRevenge.Common._M(320)) : 0);
			aRect.mWidth += num;
			this.DoCommonInGameLoadThread(aRect);
			this.HideMainMenu(true);
			this.PlaySong(12);
			this.mBoard.GauntletModeSetupComplete();
			this.mAutoMonkey.SetState(MonkeyState.Playing);
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x0006E8F8 File Offset: 0x0006CAF8
		public void StartIronFrogMode()
		{
			if (this.mStartInGameModeThreadProcRunning)
			{
				return;
			}
			this.mUserProfile.mIronFrogStats.mCurTime = 0;
			this.mLoadType = 3;
			this.mStartInGameModeThreadProcRunning = true;
			this.StartIronFrogModeThreadProc();
			int num = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(700));
			int num2 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(650));
			this.DoCommonInGameLoadThread(new Rect((this.mWidth - num) / 2, (this.mHeight - num2) / 2, num, num2));
			this.HideMainMenu(true);
			this.PlaySong(12);
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0006E984 File Offset: 0x0006CB84
		public void PlaySong(int song, float fade_speed)
		{
			bool inLoop = true;
			if (song != 0)
			{
				switch (song)
				{
				case 120:
				case 121:
				case 122:
				case 123:
				case 124:
				case 125:
					goto IL_37;
				case 126:
					break;
				default:
					if (song != 137)
					{
						goto IL_3D;
					}
					break;
				}
				inLoop = false;
				goto IL_3D;
			}
			IL_37:
			inLoop = false;
			IL_3D:
			this.mMusic.PlaySongNoDelay(song, inLoop);
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0006E9DB File Offset: 0x0006CBDB
		public void PlaySong(int song)
		{
			this.PlaySong(song, 0.005f);
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0006E9EC File Offset: 0x0006CBEC
		public void DoOptionsDialog(bool ingame)
		{
			if (this.mBoard != null)
			{
				this.mBoard.Pause(true, true);
			}
			OptionsDialog optionsDialog = new OptionsDialog(ingame);
			ZumasRevenge.Common.SetupDialog(optionsDialog);
			this.AddDialog(optionsDialog);
			if (ingame)
			{
				optionsDialog.Move(optionsDialog.mX, optionsDialog.mY + ZumasRevenge.Common._S(30));
			}
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0006EA40 File Offset: 0x0006CC40
		public void FinishOptionsDialog(bool doSave)
		{
			OptionsDialog optionsDialog = base.GetDialog(2) as OptionsDialog;
			bool wantWindowed = false;
			bool flag = true;
			bool flag2 = true;
			bool mIsWindowed = this.mIsWindowed;
			this.Is3DAccelerated();
			if (flag2)
			{
				flag = true;
			}
			bool flag3 = false;
			this.EnableCustomCursors(false);
			this.mCursorTarget = false;
			this.RegistryWriteBoolean("Z2Cursor", this.mCursorTarget);
			if (doSave)
			{
				this.mColorblind = optionsDialog.mColorBlindSlider.IsOn();
				this.SaveGlobalConfig();
			}
			if (flag3)
			{
				this.RegistryWriteBoolean("PreHiRes", this.mHiRes);
				this.RegistryWriteBoolean("Pre3D", this.Is3DAccelerated());
				this.RegistryWriteBoolean("PreWindowed", this.mIsWindowed);
				this.RegistryWriteBoolean("NeedsConfirmation", true);
				this.mPreferredWidth = (this.mPreferredHeight = -1);
				this.RegistryWriteBoolean("HiRes", true);
				this.mReInit = true;
				this.Shutdown();
				if (!flag)
				{
					this.RegistryWriteBoolean("Is3D", false);
				}
				else
				{
					this.RegistryEraseValue("Is3D");
				}
			}
			else
			{
				this.SwitchScreenMode(wantWindowed, flag, true);
				this.ClearUpdateBacklog(false);
			}
			optionsDialog.mDrawScale.SetCurve(ZumasRevenge.Common._MP("b+0,1,0.05,1,~###         ~#A5t"));
			optionsDialog.mWidgetFlagsMod.mRemoveFlags |= 16;
			optionsDialog.Kill();
			if (this.mBoard != null)
			{
				this.mBoard.Pause(false, true);
				if (this.mBoard.mMenuButton != null)
				{
					this.mBoard.mMenuButton.mDisabled = false;
				}
			}
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0006EBB7 File Offset: 0x0006CDB7
		public int DoQuitPromptDialog()
		{
			return this.DoYesNoDialog(TextManager.getInstance().getString(448), TextManager.getInstance().getString(453), true);
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0006EBDE File Offset: 0x0006CDDE
		public void TakeScreenshot(string prefix)
		{
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0006EBE0 File Offset: 0x0006CDE0
		public static SharedImageRef CompositionLoadFunc(string file_dir, string file_name)
		{
			int num = file_name.IndexOf('\\');
			string text = "";
			string text2 = "";
			if (num != -1)
			{
				text = file_name.Substring(0, num);
				text2 = file_name.Substring(num + 1);
			}
			string text3;
			string text4;
			if (text.Length == 0)
			{
				text3 = JeffLib.Common.PathToResName(file_dir, "images", "IMAGE") + GameApp.mCompositionResPrefix + "_" + JeffLib.Common.StripFileExtension(file_name).ToUpper();
				text4 = JeffLib.Common.PathToResName(file_dir, "images", "IMAGE") + "_" + JeffLib.Common.StripFileExtension(file_name).ToUpper();
			}
			else
			{
				text3 = JeffLib.Common.PathToResName(file_dir, "images", "IMAGE") + GameApp.mCompositionResPrefix + "_" + (text + "_" + text2).ToUpper();
				text4 = JeffLib.Common.PathToResName(file_dir, "images", "IMAGE") + "_" + (text + "_" + text2).ToUpper();
			}
			text3 = text3.Replace(' ', '_');
			text3 = text3.Replace('-', '_');
			text4 = text4.Replace(' ', '_');
			text4 = text4.Replace('-', '_');
			SharedImageRef sharedImageRef = GameApp.gApp.mResourceManager.LoadImage(text3);
			if (sharedImageRef == null || (sharedImageRef != null && sharedImageRef.GetImage() == null))
			{
				sharedImageRef = GameApp.gApp.mResourceManager.LoadImage(text4);
				sharedImageRef.mSharedImage.mImage.mFilePath = text4;
			}
			else
			{
				sharedImageRef.mSharedImage.mImage.mFilePath = text3;
			}
			return sharedImageRef;
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0006ED64 File Offset: 0x0006CF64
		public static void CompositionPostLoadFunc(SharedImageRef img, Layer l)
		{
			l.mXOff = ZumasRevenge.Common._DS(GameApp.gApp.mResourceManager.GetImageOffset(l.GetImage().mFilePath).mX);
			l.mYOff = ZumasRevenge.Common._DS(GameApp.gApp.mResourceManager.GetImageOffset(l.GetImage().mFilePath).mY);
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0006EDC5 File Offset: 0x0006CFC5
		public bool ChangeUser(string user_name)
		{
			return true;
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0006EDC8 File Offset: 0x0006CFC8
		public bool DeleteUser(string user_name)
		{
			return true;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0006EDCB File Offset: 0x0006CFCB
		public bool ShadersSupported()
		{
			return true;
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0006EDCE File Offset: 0x0006CFCE
		public void DoNewUserDialog(int button_mode, bool isIntro)
		{
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0006EDD0 File Offset: 0x0006CFD0
		public void DoNewUserDialog()
		{
			this.DoNewUserDialog(3, false);
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0006EDDA File Offset: 0x0006CFDA
		public void DoNewUserDialog(int button_mode)
		{
			this.DoNewUserDialog(button_mode, false);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0006EDE4 File Offset: 0x0006CFE4
		public Rect GetNewUserDialogFrame()
		{
			return Rect.ZERO_RECT;
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0006EDEB File Offset: 0x0006CFEB
		public void BlankNameEntered()
		{
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0006EDED File Offset: 0x0006CFED
		public void NameIsAllSpaces()
		{
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0006EDEF File Offset: 0x0006CFEF
		public void FinishedNewUser(bool canceled)
		{
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0006EDF4 File Offset: 0x0006CFF4
		public void DoGenericDialog(string header, string message, bool block, GameApp.PreBlockCallback pre_block_callback, int width_pad)
		{
			Font fontByName = this.GetFontByName("FONT_SHAGLOUNGE38_YELLOW");
			ZumaDialog zumaDialog = new ZumaDialog(0, true, "", message, TextManager.getInstance().getString(483), 3);
			zumaDialog.mSpaceAfterHeader = 0;
			if (this.mBoard != null)
			{
				this.mBoard.Pause(true, true);
			}
			zumaDialog.mContentInsets.mTop += ZumasRevenge.Common._S(ZumasRevenge.Common._M(30));
			int num = 0;
			int num2 = 0;
			JeffLib.Common.StringDimensions(message, fontByName, out num, out num2);
			zumaDialog.mAllowDrag = false;
			zumaDialog.GetSize(ref num, ref num2);
			num += width_pad;
			zumaDialog.Resize((this.mWidth - num) / 2, (this.mHeight - num2) / 2, num, num2);
			ZumasRevenge.Common.SetupDialog(zumaDialog);
			this.AddDialog(zumaDialog);
			this.mDialogCallBack = pre_block_callback;
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0006EEBC File Offset: 0x0006D0BC
		public int DoYesNoDialog(string header, string message, bool block, string btn_yes, string btn_no, bool drag, int header_space, int id)
		{
			return this.DoYesNoDialog(header, message, block, btn_yes, btn_no, drag, header_space, id, 0);
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0006EEE0 File Offset: 0x0006D0E0
		public int DoYesNoDialog(string header, string message, bool block, string btn_yes, string btn_no, bool drag, int header_space)
		{
			return this.DoYesNoDialog(header, message, block, btn_yes, btn_no, drag, header_space, 1, 0);
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0006EF00 File Offset: 0x0006D100
		public int DoYesNoDialog(string header, string message, bool block, string btn_yes, string btn_no, bool drag)
		{
			return this.DoYesNoDialog(header, message, block, btn_yes, btn_no, drag, -1, 1, 0);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0006EF20 File Offset: 0x0006D120
		public int DoYesNoDialog(string header, string message, bool block, string btn_yes, string btn_no)
		{
			return this.DoYesNoDialog(header, message, block, btn_yes, btn_no, true, -1, 1, 0);
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0006EF40 File Offset: 0x0006D140
		public int DoYesNoDialog(string header, string message, bool block, string btn_yes)
		{
			return this.DoYesNoDialog(header, message, block, btn_yes, TextManager.getInstance().getString(447), true, -1, 1, 0);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0006EF6C File Offset: 0x0006D16C
		public int DoYesNoDialog(string header, string message, bool block)
		{
			return this.DoYesNoDialog(header, message, block, TextManager.getInstance().getString(446), TextManager.getInstance().getString(447), true, -1, 1, 0);
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x0006EFA4 File Offset: 0x0006D1A4
		public int DoYesNoDialog(string header, string message)
		{
			return this.DoYesNoDialog(header, message, false, TextManager.getInstance().getString(446), TextManager.getInstance().getString(447), true, -1, 1, 0);
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0006EFDC File Offset: 0x0006D1DC
		public int DoYesNoDialog(string header, string message, bool block, string btn_yes, string btn_no, bool drag, int header_space, int id, int width_pad)
		{
			Font fontByName = this.GetFontByName("FONT_SHAGLOUNGE38_YELLOW");
			this.mDialog = new ZumaDialog(id, true, "", message, "", 1);
			this.mDialog.mSpaceAfterHeader = 0;
			if (this.mBoard != null)
			{
				this.mBoard.Pause(true, true);
			}
			this.mDialog.mContentInsets.mTop += ZumasRevenge.Common._S(ZumasRevenge.Common._M(30));
			int num;
			int num2;
			JeffLib.Common.StringDimensions(message, fontByName, out num, out num2);
			this.mDialog.mAllowDrag = false;
			this.mDialog.GetSize(ref num, ref num2);
			num += width_pad;
			this.mDialog.Resize((this.mWidth - num) / 2, (this.mHeight - num2) / 2, num, num2);
			this.mDialog.mYesButton.mLabel = btn_yes;
			this.mDialog.mNoButton.mLabel = btn_no;
			this.mDialog.mAllowDrag = false;
			ZumasRevenge.Common.SetupDialog(this.mDialog);
			this.AddDialog(this.mDialog);
			this.mWidgetManager.SetFocus(this.mDialog);
			if (block)
			{
				return this.mDialog.WaitForResult(false);
			}
			return -1;
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0006F10A File Offset: 0x0006D30A
		public void EndYesNoDialog(int ButtonId)
		{
			if (this.mYesNoDialogDelegate != null)
			{
				this.mYesNoDialogDelegate(ButtonId);
			}
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0006F120 File Offset: 0x0006D320
		public int GetPan(int thePos)
		{
			return 3000 * (thePos - 400) / 400;
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0006F138 File Offset: 0x0006D338
		public CompositionMgr LoadComposition(string file_name, string res_prefix)
		{
			string text = SexyLocale.StringToUpper(file_name);
			if (this.mPreloadedComps.ContainsKey(text) && this.mPreloadedComps[text].isValid())
			{
				return new CompositionMgr(this.mPreloadedComps[text]);
			}
			CompositionMgr compositionMgr = new CompositionMgr();
			compositionMgr.mLoadImageFunc = new AECommon.LoadCompImageFunc(GameApp.CompositionLoadFunc);
			compositionMgr.mPostLoadImageFunc = new AECommon.PostLoadCompImageFunc(GameApp.CompositionPostLoadFunc);
			GameApp.mCompositionResPrefix = res_prefix;
			bool flag = compositionMgr.LoadFromFile(file_name);
			GameApp.mCompositionResPrefix = "";
			if (!flag)
			{
				compositionMgr = null;
			}
			this.mPreloadedComps[text] = compositionMgr;
			return new CompositionMgr(compositionMgr);
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0006F1DC File Offset: 0x0006D3DC
		public PIEffect GetPIEffect(string file_name, bool create_copy)
		{
			if (this.mLoadingThreadCompleted)
			{
				if (file_name == "TorchFlame")
				{
					for (int i = 0; i < this.mCachedTorchEffects.size<CachedTorchEffect>(); i++)
					{
						CachedTorchEffect cachedTorchEffect = this.mCachedTorchEffects[i];
						if (!cachedTorchEffect.mFlameInUse)
						{
							cachedTorchEffect.mFlameInUse = true;
							cachedTorchEffect.mTorchFlame.ResetAnim();
							return cachedTorchEffect.mTorchFlame;
						}
					}
				}
				else if (file_name == "TorchFlameOut")
				{
					for (int j = 0; j < this.mCachedTorchEffects.size<CachedTorchEffect>(); j++)
					{
						CachedTorchEffect cachedTorchEffect2 = this.mCachedTorchEffects[j];
						if (!cachedTorchEffect2.mFlameOutInUse)
						{
							cachedTorchEffect2.mFlameOutInUse = true;
							cachedTorchEffect2.mTorchFlameOut.ResetAnim();
							return cachedTorchEffect2.mTorchFlameOut;
						}
					}
				}
				else if (file_name == "Devil Projectile")
				{
					for (int k = 0; k < this.mCachedVolcanoEffects.size<CachedVolcanoEffect>(); k++)
					{
						CachedVolcanoEffect cachedVolcanoEffect = this.mCachedVolcanoEffects[k];
						if (!cachedVolcanoEffect.mProjectileInUse)
						{
							cachedVolcanoEffect.mProjectileInUse = true;
							cachedVolcanoEffect.mProjectile.ResetAnim();
							cachedVolcanoEffect.mProjectile.mEmitAfterTimeline = true;
							return cachedVolcanoEffect.mProjectile;
						}
					}
				}
				else if (file_name == "Devil Explosion")
				{
					for (int l = 0; l < this.mCachedVolcanoEffects.size<CachedVolcanoEffect>(); l++)
					{
						CachedVolcanoEffect cachedVolcanoEffect2 = this.mCachedVolcanoEffects[l];
						if (!cachedVolcanoEffect2.mExplosionInUse)
						{
							cachedVolcanoEffect2.mExplosionInUse = true;
							cachedVolcanoEffect2.mExplosion.ResetAnim();
							return cachedVolcanoEffect2.mExplosion;
						}
					}
				}
			}
			if (this.mCachedPIEffects.ContainsKey(file_name))
			{
				for (int m = 0; m < this.mCachedPIEffects[file_name].Count; m++)
				{
					GenericCachedEffect genericCachedEffect = this.mCachedPIEffects[file_name][m];
					if (!genericCachedEffect.mInUse)
					{
						genericCachedEffect.mInUse = true;
						genericCachedEffect.mEffect.ResetAnim();
						return genericCachedEffect.mEffect;
					}
				}
			}
			string theFileName = string.Concat(new string[]
			{
				this.GetBaseResImagesDir(),
				"particles\\",
				file_name,
				"\\",
				file_name,
				".ppf"
			});
			PIEffect pieffect = new PIEffect();
			PIEffect pieffect2 = new PIEffect();
			if (!pieffect2.LoadEffect(theFileName))
			{
				return null;
			}
			pieffect = pieffect2;
			if (!create_copy)
			{
				return pieffect;
			}
			return pieffect.Duplicate();
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0006F43D File Offset: 0x0006D63D
		public PIEffect GetPIEffect(string file_name)
		{
			return this.GetPIEffect(file_name, true);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0006F447 File Offset: 0x0006D647
		public bool IsHardMode()
		{
			return false;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0006F44A File Offset: 0x0006D64A
		public MemoryImage GenerateLevelThumbnail(string thumb_path, Level l)
		{
			return null;
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0006F44D File Offset: 0x0006D64D
		public bool IronFrogUnlocked()
		{
			return this.mUserProfile != null && this.mUserProfile.mAdvModeVars.mNumTimesZoneBeat[5] > 0;
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0006F46E File Offset: 0x0006D66E
		public bool ChallengeModeUnlocked()
		{
			return this.mUserProfile != null && this.mUserProfile.mChallengeUnlockState[0, 0] > 0;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0006F48F File Offset: 0x0006D68F
		public bool HSScreenUnlocked()
		{
			return this.mUserProfile != null && this.mUserProfile.mAdvModeVars.mNumTimesZoneBeat[0] >= 1;
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0006F4B4 File Offset: 0x0006D6B4
		public void ReleaseTorchEffect(PIEffect fx)
		{
			if (fx == null)
			{
				return;
			}
			for (int i = 0; i < this.mCachedTorchEffects.size<CachedTorchEffect>(); i++)
			{
				CachedTorchEffect cachedTorchEffect = this.mCachedTorchEffects[i];
				if (cachedTorchEffect.mTorchFlame == fx)
				{
					cachedTorchEffect.mFlameInUse = false;
					return;
				}
				if (cachedTorchEffect.mTorchFlameOut == fx)
				{
					cachedTorchEffect.mFlameOutInUse = false;
					return;
				}
			}
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0006F50C File Offset: 0x0006D70C
		public void ReleaseVolcanoEffect(PIEffect fx)
		{
			if (fx == null)
			{
				return;
			}
			for (int i = 0; i < this.mCachedVolcanoEffects.size<CachedVolcanoEffect>(); i++)
			{
				CachedVolcanoEffect cachedVolcanoEffect = this.mCachedVolcanoEffects[i];
				if (cachedVolcanoEffect.mProjectile == fx)
				{
					cachedVolcanoEffect.mProjectileInUse = false;
					return;
				}
				if (cachedVolcanoEffect.mExplosion == fx)
				{
					cachedVolcanoEffect.mExplosionInUse = false;
					return;
				}
			}
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0006F564 File Offset: 0x0006D764
		public void ReleaseGenericCachedEffect(PIEffect fx)
		{
			if (fx == null)
			{
				return;
			}
			foreach (KeyValuePair<string, List<GenericCachedEffect>> keyValuePair in this.mCachedPIEffects)
			{
				foreach (GenericCachedEffect genericCachedEffect in keyValuePair.Value)
				{
					if (genericCachedEffect.mEffect == fx)
					{
						genericCachedEffect.mInUse = false;
						break;
					}
				}
			}
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0006F5E8 File Offset: 0x0006D7E8
		public Board GetBoard()
		{
			return this.mBoard;
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0006F5F0 File Offset: 0x0006D7F0
		public bool ShowingLoadingScreen()
		{
			return this.mLoadingScreen != null;
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0006F5FE File Offset: 0x0006D7FE
		public void IncFramesPlayed()
		{
			this.mFramesPlayed++;
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0006F60E File Offset: 0x0006D80E
		public string GetResImagesDir()
		{
			return string.Format("images\\{0}\\", GameApp.mGameRes);
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0006F624 File Offset: 0x0006D824
		public string GetBaseResImagesDir()
		{
			return string.Format("images\\{0}\\", this.mResourceManager.mBaseArtRes);
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0006F640 File Offset: 0x0006D840
		public static int ScaleNum(int theNum, int theAdd)
		{
			return (int)((float)theNum * GameApp.mGameUpScale) + theAdd;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0006F64D File Offset: 0x0006D84D
		public static int ScaleNum(int theNum)
		{
			return GameApp.ScaleNum(theNum, 0);
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0006F656 File Offset: 0x0006D856
		public static float ScaleNum(float theNum, float theAdd)
		{
			return theNum * GameApp.mGameUpScale + theAdd;
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0006F661 File Offset: 0x0006D861
		public static float ScaleNum(float theNum)
		{
			return GameApp.ScaleNum(theNum, 0f);
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0006F66E File Offset: 0x0006D86E
		public static double ScaleNum(double theNum, double theAdd)
		{
			return theNum * (double)GameApp.mGameUpScale + theAdd;
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0006F67A File Offset: 0x0006D87A
		public static double ScaleNum(double theNum)
		{
			return GameApp.ScaleNum(theNum, 0.0);
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x0006F68B File Offset: 0x0006D88B
		public static int DownScaleNum(int theNum, int theAdd)
		{
			return (int)((float)theNum * GameApp.mGameDownScale) + theAdd;
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0006F698 File Offset: 0x0006D898
		public static int DownScaleNum(int theNum)
		{
			return GameApp.DownScaleNum(theNum, 0);
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0006F6A1 File Offset: 0x0006D8A1
		public static float DownScaleNum(float theNum, float theAdd)
		{
			return theNum * GameApp.mGameDownScale + theAdd;
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0006F6AC File Offset: 0x0006D8AC
		public static float DownScaleNum(float theNum)
		{
			return GameApp.DownScaleNum(theNum, 0f);
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0006F6B9 File Offset: 0x0006D8B9
		public static double DownScaleNum(double theNum, double theAdd)
		{
			return theNum * (double)GameApp.mGameDownScale + theAdd;
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0006F6C5 File Offset: 0x0006D8C5
		public static double DownScaleNum(double theNum)
		{
			return GameApp.DownScaleNum(theNum, 0.0);
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0006F6D6 File Offset: 0x0006D8D6
		public static int ScreenScaleNum(int theNum, int theAdd)
		{
			return (int)((float)theNum * GameApp.mGameScreenScale) + theAdd;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0006F6E3 File Offset: 0x0006D8E3
		public static int ScreenScaleNum(int theNum)
		{
			return GameApp.ScreenScaleNum(theNum, 0);
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0006F6EC File Offset: 0x0006D8EC
		public static float ScreenScaleNum(float theNum, float theAdd)
		{
			return theNum * GameApp.mGameScreenScale + theAdd;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0006F6F7 File Offset: 0x0006D8F7
		public static float ScreenScaleNum(float theNum)
		{
			return GameApp.ScreenScaleNum(theNum, 0f);
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x0006F704 File Offset: 0x0006D904
		public static double ScreenScaleNum(double theNum, double theAdd)
		{
			return theNum * (double)GameApp.mGameScreenScale + theAdd;
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0006F710 File Offset: 0x0006D910
		public static double ScreenScaleNum(double theNum)
		{
			return GameApp.ScreenScaleNum(theNum, 0.0);
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0006F721 File Offset: 0x0006D921
		public virtual uint GetProfileVersion()
		{
			return 0U;
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0006F724 File Offset: 0x0006D924
		public virtual void NotifyProfileChanged(UserProfile player)
		{
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0006F726 File Offset: 0x0006D926
		public virtual UserProfile CreateUserProfile()
		{
			return new ZumaProfile();
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0006F72D File Offset: 0x0006D92D
		public virtual void OnProfileLoad(UserProfile player, SexyBuffer buffer)
		{
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0006F72F File Offset: 0x0006D92F
		public virtual void OnProfileSave(UserProfile player, SexyBuffer buffer)
		{
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0006F731 File Offset: 0x0006D931
		public Rect GetScreenRect()
		{
			return this.mWidgetManager.mMouseDestRect;
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0006F73E File Offset: 0x0006D93E
		public int GetScreenWidth()
		{
			return this.mWidgetManager.mMouseDestRect.mWidth - this.mWidgetManager.mMouseDestRect.mX;
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0006F761 File Offset: 0x0006D961
		public static bool IsTablet()
		{
			return true;
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0006F764 File Offset: 0x0006D964
		public Image GetLevelThumbnail(int theLevelNum)
		{
			Image second = this.mLevelThumbnails[theLevelNum].second;
			string[] array = new string[] { "jungle", "village", "city", "coast", "grotto", "volcano" };
			if (second == null)
			{
				int num = theLevelNum / 10;
				int num2 = theLevelNum % 10 + 1;
				string text = array[num] + string.Format("{0}", num2);
				string theFileName = "levelthumbs\\" + text + "_thumb";
				IdxThumbPair idxThumbPair = this.mLevelThumbnails[theLevelNum];
				idxThumbPair.second = this.GetImage(theFileName, true, true, false);
				if (idxThumbPair.second != null)
				{
					second = idxThumbPair.second;
				}
			}
			return second;
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x0006F834 File Offset: 0x0006DA34
		public void DeleteLevelThumbnail(int theLevel)
		{
			if (theLevel >= 0 && theLevel <= Enumerable.Count<IdxThumbPair>(this.mLevelThumbnails))
			{
				IdxThumbPair idxThumbPair = this.mLevelThumbnails[theLevel];
				if (idxThumbPair.second != null)
				{
					idxThumbPair.second.Dispose();
					idxThumbPair.second = null;
				}
			}
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0006F87C File Offset: 0x0006DA7C
		public void DeleteZoneThumbnails(int theZone)
		{
			if (theZone >= 0 && theZone <= 6)
			{
				int num = theZone * 10;
				for (int i = 0; i < 10; i++)
				{
					IdxThumbPair idxThumbPair = this.mLevelThumbnails[num + i];
					if (idxThumbPair.second != null)
					{
						idxThumbPair.second.Dispose();
						idxThumbPair.second = null;
					}
				}
			}
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x0006F8CC File Offset: 0x0006DACC
		public void LoadAllThumbnails()
		{
			for (int i = 0; i < 6; i++)
			{
				int num = i * 10;
				for (int j = 0; j < 10; j++)
				{
					this.GetLevelThumbnail(num + j);
				}
			}
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x0006F901 File Offset: 0x0006DB01
		public void AppEnteredBackground()
		{
			if (this.mBoard != null && this.mBoard.NeedSaveGame() && this.mUserProfile != null)
			{
				this.mBoard.SaveGame(this.mUserProfile.GetSaveGameName(this.IsHardMode()), null);
			}
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x0006F93D File Offset: 0x0006DB3D
		public override double GetLoadingThreadProgress()
		{
			return (double)this.mResourceManager.GetLoadResourcesListProgress(GameApp.gInitialLoadGroups);
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x0006F950 File Offset: 0x0006DB50
		public void ToggleBambooTransition()
		{
			if (this.mBambooTransition == null)
			{
				return;
			}
			if (!this.mBambooTransition.IsInProgress())
			{
				this.mBambooTransition.Reset();
				this.mBambooTransition.SetVisible(true);
				this.mBambooTransition.SetDisabled(false);
				this.mWidgetManager.AddWidget(this.mBambooTransition);
				this.mWidgetManager.BringToFront(this.mBambooTransition);
				this.mBambooTransition.StartTransition();
			}
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x0006F9C3 File Offset: 0x0006DBC3
		public void BambooTransitionOpened()
		{
			this.mBambooTransition.Reset();
			this.mBambooTransition.SetVisible(false);
			this.mBambooTransition.SetDisabled(true);
			this.mWidgetManager.RemoveWidget(this.mBambooTransition);
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0006F9F9 File Offset: 0x0006DBF9
		public void EndChallengeModeGame()
		{
			this.EndCurrentGame();
			this.ShowChallengeSelector();
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0006FA07 File Offset: 0x0006DC07
		public void InitMetricsManager()
		{
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0006FA0C File Offset: 0x0006DC0C
		public void HideHelp()
		{
			OptionsDialog optionsDialog = base.GetDialog(2) as OptionsDialog;
			if (optionsDialog != null)
			{
				optionsDialog.OnHelpHided();
			}
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0006FA2F File Offset: 0x0006DC2F
		public void ShowAbout()
		{
			this.mAboutInfo = new AboutInfo();
			this.AddDialog(this.mAboutInfo);
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0006FA48 File Offset: 0x0006DC48
		public void HideAbout()
		{
			this.mAboutInfo.mDrawScale.SetCurve(ZumasRevenge.Common._MP("b+0,1,0.05,1,~###         ~#A5t"));
			this.mAboutInfo.mWidgetFlagsMod.mRemoveFlags |= 16;
			this.mAboutInfo = null;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0006FA84 File Offset: 0x0006DC84
		public void ShowLegal()
		{
			this.mLegalInfo = new LegalInfo();
			this.AddDialog(this.mLegalInfo);
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0006FA9D File Offset: 0x0006DC9D
		public void HideLegal()
		{
			this.mLegalInfo.mDrawScale.SetCurve(ZumasRevenge.Common._MP("b+0,1,0.05,1,~###         ~#A5t"));
			this.mLegalInfo.mWidgetFlagsMod.mRemoveFlags |= 16;
			this.mLegalInfo = null;
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0006FAD9 File Offset: 0x0006DCD9
		public void ShowMetricsDebug()
		{
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0006FADB File Offset: 0x0006DCDB
		public void HideMetricsDebug()
		{
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0006FADD File Offset: 0x0006DCDD
		public void ReportAppLaunchInfo(int theAppEvent)
		{
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x0006FADF File Offset: 0x0006DCDF
		public void ReportEndOfLevelMetrics(Board theBoard, bool theLevelSuccess, bool theAcedLevel)
		{
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0006FAE1 File Offset: 0x0006DCE1
		public void ReportEndOfLevelMetrics(Board theBoard, bool theLevelSuccess)
		{
			this.ReportEndOfLevelMetrics(theBoard, theLevelSuccess, false);
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0006FAEC File Offset: 0x0006DCEC
		public void ReportEndOfLevelMetrics(Board theBoard)
		{
			this.ReportEndOfLevelMetrics(theBoard, false, false);
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0006FAF7 File Offset: 0x0006DCF7
		public void CheckForAppUpdate()
		{
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0006FAF9 File Offset: 0x0006DCF9
		public void GetTouchInputOffset(ref int x, ref int y)
		{
			x = this.mTouchOffsetX;
			y = this.mTouchOffsetY;
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0006FB0B File Offset: 0x0006DD0B
		public void SetTouchInputOffset(int x, int y)
		{
			this.mTouchOffsetX = x;
			this.mTouchOffsetY = y;
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0006FB1B File Offset: 0x0006DD1B
		public void LoadLevelXML()
		{
			this.mLoadingProc = new ThreadStart(this.LoadingLevel);
			this.mLoadingThread = new Thread(this.mLoadingProc);
			this.mLoadLevelSuccess = false;
			this.mLoadingThread.Start();
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x0006FB54 File Offset: 0x0006DD54
		private void LoadingLevel()
		{
			this.mNormalLevelMgr = ((XNAFileDriver)this.mFileDriver).GetContentManager().Load<LevelMgr>(this.mLevelXML);
			this.mNormalLevelMgr.Init();
			this.mNormalLevelMgr.mLevelXML = this.mLevelXML;
			this.mLoadLevelSuccess = true;
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0006FBA8 File Offset: 0x0006DDA8
		public void OpenURL(string url)
		{
			System.Diagnostics.Process.Start(url);
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0006FBE4 File Offset: 0x0006DDE4
		public void HandleGameUpdateRequired(GameUpdateRequiredException ex)
		{
			GameApp.UN_UPDATE_VERSION = true;
			GameApp.USE_XBOX_SERVICE = false;
			GameApp.mDisplayTitleUpdateMessage = true;
			GameApp.DisplayTitleUpdateMessage();
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0006FC00 File Offset: 0x0006DE00
		public static void DisplayTitleUpdateMessage()
		{
			List<string> list = new List<string>();
			string @string = TextManager.getInstance().getString(446);
			string string2 = TextManager.getInstance().getString(447);
			list.Add(string2);
			list.Add(@string);
			if (GameApp.mDisplayTitleUpdateMessage && !Guide.IsVisible)
			{
				GameApp.mDisplayTitleUpdateMessage = false;
				string string3 = TextManager.getInstance().getString(62);
				Guide.BeginShowMessageBox("   ", string3, list, 1, MessageBoxIcon.Warning, new AsyncCallback(GameApp.UpdateDialogGetMBResult), null);
			}
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x0006FC80 File Offset: 0x0006DE80
		public static void UpdateDialogGetMBResult(IAsyncResult userResult)
		{
			int? num = Guide.EndShowMessageBox(userResult);
			if (num != null && num.Value > 0)
			{
				if (Guide.IsTrialMode)
				{
					Guide.ShowMarketplace(0);
					return;
				}
				/*new MarketplaceDetailTask
				{
					ContentType = 1,
					ContentIdentifier = "43f34364-9df4-4d95-b9cf-e48b3c85cda9"
				}.Show();*/
			}
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x0006FCD4 File Offset: 0x0006DED4
		public void ToMarketPlace()
		{
			/*if (Guide.IsTrialMode)
			{
				Guide.ShowMarketplace(0);
				return;
			}
			new MarketplaceDetailTask
			{
				ContentType = 1
			}.Show();*/
			}

			// Token: 0x06000BC8 RID: 3016 RVA: 0x0006FD04 File Offset: 0x0006DF04
			public static void initResolution(int param1)
		{
			GameApp.mGameRes = param1;
			int num = GameApp.mGameRes;
			if (num <= 640)
			{
				if (num == 320)
				{
					GameApp.mGameUpScale = 0.5333334f;
					GameApp.mGameDownScale = 0.2666667f;
					GameApp.mGameScreenScale = 1f / GameApp.mGameUpScale;
					return;
				}
				if (num == 600)
				{
					GameApp.mGameUpScale = 1f;
					GameApp.mGameDownScale = 0.5f;
					GameApp.mGameScreenScale = 1f;
					return;
				}
				if (num == 640)
				{
					GameApp.mGameUpScale = 1.0666668f;
					GameApp.mGameDownScale = 0.5333334f;
					GameApp.mGameScreenScale = 1f / GameApp.mGameUpScale;
					return;
				}
			}
			else
			{
				if (num == 720)
				{
					GameApp.mGameUpScale = 1.2f;
					GameApp.mGameDownScale = 0.6f;
					GameApp.mGameScreenScale = 1f / GameApp.mGameUpScale;
					return;
				}
				if (num == 768)
				{
					GameApp.mGameUpScale = 1.28f;
					GameApp.mGameDownScale = 0.64f;
					GameApp.mGameScreenScale = 1f / GameApp.mGameUpScale;
					return;
				}
			}
			GameApp.mGameUpScale = 2f;
			GameApp.mGameDownScale = 1f;
			GameApp.mGameScreenScale = 0.5f;
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0006FE31 File Offset: 0x0006E031
		public void SetOrientation(int Orientation)
		{
			((WP7AppDriver)this.mAppDriver).SetOrientation(Orientation);
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0006FE44 File Offset: 0x0006E044
		// Note: this type is marked as 'beforefieldinit'.
		static GameApp()
		{
			string[] array = new string[5];
			array[0] = "Init";
			array[1] = "CommonGame";
			array[2] = "GamePlay";
			array[3] = "MenuRelated";
			GameApp.gInitialLoadGroups = array;
		}

		// Token: 0x04000996 RID: 2454
		public static bool USE_TRIAL_VERSION = false;

		// Token: 0x04000997 RID: 2455
		public static bool NONE_XBOX_LIVE = false;

		// Token: 0x04000998 RID: 2456
		public static bool UN_UPDATE_VERSION = false;

		// Token: 0x04000999 RID: 2457
		public static bool USE_XBOX_SERVICE = false;

		// Token: 0x0400099A RID: 2458
		//public WebBrowserTask mWbt;

		// Token: 0x0400099B RID: 2459
		public bool mWaitForActive;

		// Token: 0x0400099C RID: 2460
		public bool mInitFinished;

		// Token: 0x0400099D RID: 2461
		public static bool mDisplayTitleUpdateMessage = false;

		// Token: 0x0400099E RID: 2462
		public static bool mExit = false;

		// Token: 0x0400099F RID: 2463
		public Image mBackgroundLayer;

		// Token: 0x040009A0 RID: 2464
		public Thread mInitThread;

		// Token: 0x040009A1 RID: 2465
		public static GameApp gApp = null;

		// Token: 0x040009A2 RID: 2466
		public static DDS gDDS = null;

		// Token: 0x040009A3 RID: 2467
		public static int gSaveGameVersion = 197;

		// Token: 0x040009A4 RID: 2468
		public static int gNumOptionalGroups = 8;

		// Token: 0x040009A5 RID: 2469
		public static string[] gOptionalGroups = new string[] { "CommonBoss", "Boss1", "Boss2", "Boss3", "Boss4", "Boss5", "Boss6Common", "GrottoSounds" };

		// Token: 0x040009A6 RID: 2470
		public static string gOrgTitle = "";

		// Token: 0x040009A7 RID: 2471
		public static bool gDidCrashHandler = false;

		// Token: 0x040009A8 RID: 2472
		public static string gMetricsVersion = "1.0";

		// Token: 0x040009A9 RID: 2473
		public static int gScreenShakeX = 0;

		// Token: 0x040009AA RID: 2474
		public static int gScreenShakeY = 0;

		// Token: 0x040009AB RID: 2475
		public static int gLastLevel = 0;

		// Token: 0x040009AC RID: 2476
		public static int gLastZone = -1;

		// Token: 0x040009AD RID: 2477
		public static bool gNeedsPreCache = true;

		// Token: 0x040009AE RID: 2478
		private static int InGameLoadThread_DrawFunc_CallCounter = 0;

		// Token: 0x040009AF RID: 2479
		private static int gAddingDlgID = -12345;

		// Token: 0x040009B0 RID: 2480
		public static bool gInitialProfLoadSuccessful;

		// Token: 0x040009B1 RID: 2481
		private static string[] gInitialLoadGroups;

		// Token: 0x040009B2 RID: 2482
		private GameApp.PreBlockCallback mDialogCallBack;

		// Token: 0x040009B3 RID: 2483
		public Game mGameMain;

		// Token: 0x040009B4 RID: 2484
		public AutoMonkey mAutoMonkey;

		// Token: 0x040009B5 RID: 2485
		public bool mSavingOrLoadingProfile;

		// Token: 0x040009B6 RID: 2486
		public float mShotCorrectionAngleToWidthDist;

		// Token: 0x040009B7 RID: 2487
		public float mShotCorrectionAngleMax;

		// Token: 0x040009B8 RID: 2488
		public float mShotCorrectionWidthMax;

		// Token: 0x040009B9 RID: 2489
		public int mGuideStyle;

		// Token: 0x040009BA RID: 2490
		public int mShotCorrectionDebugStyle;

		// Token: 0x040009BB RID: 2491
		public bool mIronFrogModeIncluded;

		// Token: 0x040009BC RID: 2492
		public Board mBoard;

		// Token: 0x040009BD RID: 2493
		public LoadingScreen mLoadingScreen;

		// Token: 0x040009BE RID: 2494
		public LevelMgr mNormalLevelMgr;

		// Token: 0x040009BF RID: 2495
		public Dictionary<string, List<GenericCachedEffect>> mCachedPIEffects = new Dictionary<string, List<GenericCachedEffect>>();

		// Token: 0x040009C0 RID: 2496
		public MapScreenHackWidget mMapScreenHackWidget;

		// Token: 0x040009C1 RID: 2497
		public Rect mLoadRect = default(Rect);

		// Token: 0x040009C2 RID: 2498
		public ZumaDialog mReturnToMMDlg;

		// Token: 0x040009C3 RID: 2499
		public Dictionary<int, DialogHideInfo> mDialogHideInfoMap;

		// Token: 0x040009C4 RID: 2500
		public bool mDoingDRM;

		// Token: 0x040009C5 RID: 2501
		public int mTrialType;

		// Token: 0x040009C6 RID: 2502
		public int mFramesPlayed;

		// Token: 0x040009C7 RID: 2503
		public int mCachedLoadState;

		// Token: 0x040009C8 RID: 2504
		public bool mCachedLoad;

		// Token: 0x040009C9 RID: 2505
		public bool mInitialLoad;

		// Token: 0x040009CA RID: 2506
		public bool mDelayIntro;

		// Token: 0x040009CB RID: 2507
		public int mWideScreenXOffset;

		// Token: 0x040009CC RID: 2508
		public long mLastMoreGamesUpdate;

		// Token: 0x040009CD RID: 2509
		public int mIFLoadingAnimStartCel;

		// Token: 0x040009CE RID: 2510
		public Upsell mUpsell;

		// Token: 0x040009CF RID: 2511
		public List<CachedTorchEffect> mCachedTorchEffects = new List<CachedTorchEffect>();

		// Token: 0x040009D0 RID: 2512
		public List<CachedVolcanoEffect> mCachedVolcanoEffects = new List<CachedVolcanoEffect>();

		// Token: 0x040009D1 RID: 2513
		public Dictionary<string, PIEffect> mPIEffectMap = new Dictionary<string, PIEffect>();

		// Token: 0x040009D2 RID: 2514
		public bool mClickedHardMode;

		// Token: 0x040009D3 RID: 2515
		public bool mInGameLoadThreadProcFailed;

		// Token: 0x040009D4 RID: 2516
		public bool mStartInGameModeThreadProcRunning;

		// Token: 0x040009D5 RID: 2517
		public bool mContinuedGame;

		// Token: 0x040009D6 RID: 2518
		public int mForceZoneRestart;

		// Token: 0x040009D7 RID: 2519
		public string mChallengeLevelId = "";

		// Token: 0x040009D8 RID: 2520
		public UnderDialogWidget mUnderDialogWidget;

		// Token: 0x040009D9 RID: 2521
		public float mDialogObscurePct;

		// Token: 0x040009DA RID: 2522
		public Dictionary<string, CompositionMgr> mPreloadedComps = new Dictionary<string, CompositionMgr>();

		// Token: 0x040009DB RID: 2523
		public Credits mCredits;

		// Token: 0x040009DC RID: 2524
		public CreditsHackWidget gCreditsHackWidget;

		// Token: 0x040009DD RID: 2525
		public GenericHelp mGenericHelp;

		// Token: 0x040009DE RID: 2526
		public MapScreen mMapScreen;

		// Token: 0x040009DF RID: 2527
		public List<IdxThumbPair> mLevelThumbnails = new List<IdxThumbPair>();

		// Token: 0x040009E0 RID: 2528
		public ProxBombManager mProxBombManager;

		// Token: 0x040009E1 RID: 2529
		public Music mMusic;

		// Token: 0x040009E2 RID: 2530
		public SoundEffects mSoundPlayer;

		// Token: 0x040009E3 RID: 2531
		public ZumaProfile mUserProfile;

		// Token: 0x040009E4 RID: 2532
		public ZumaProfileMgr mProfileMgr;

		// Token: 0x040009E5 RID: 2533
		public MainMenu mMainMenu;

		// Token: 0x040009E6 RID: 2534
		public MoreGames mMoreGames;

		// Token: 0x040009E7 RID: 2535
		public NewUserDialog mNewUserDlg;

		// Token: 0x040009E8 RID: 2536
		public string mLevelXML;

		// Token: 0x040009E9 RID: 2537
		public string mHardLevelXML;

		// Token: 0x040009EA RID: 2538
		public static string mCompositionResPrefix;

		// Token: 0x040009EB RID: 2539
		public bool mHiRes;

		// Token: 0x040009EC RID: 2540
		public static int mGameRes;

		// Token: 0x040009ED RID: 2541
		public static float mGameDownScale;

		// Token: 0x040009EE RID: 2542
		public static float mGameUpScale;

		// Token: 0x040009EF RID: 2543
		public static float mGameScreenScale;

		// Token: 0x040009F0 RID: 2544
		public bool mReInit;

		// Token: 0x040009F1 RID: 2545
		public bool mFromReInit;

		// Token: 0x040009F2 RID: 2546
		public bool mDoingAdvModeLoad;

		// Token: 0x040009F3 RID: 2547
		public int mConfTime;

		// Token: 0x040009F4 RID: 2548
		public int mLoadType;

		// Token: 0x040009F5 RID: 2549
		public bool mColorblind;

		// Token: 0x040009F6 RID: 2550
		public bool mCursorTarget;

		// Token: 0x040009F7 RID: 2551
		public string mTimeStamp;

		// Token: 0x040009F8 RID: 2552
		public BambooTransition mBambooTransition;

		// Token: 0x040009F9 RID: 2553
		public string m_DefaultProfileName = "Player 1";

		// Token: 0x040009FA RID: 2554
		public string m_DefaultName = "Player 1";

		// Token: 0x040009FB RID: 2555
		public LegalInfo mLegalInfo;

		// Token: 0x040009FC RID: 2556
		public AboutInfo mAboutInfo;

		// Token: 0x040009FD RID: 2557
		public WidescreenBoardWidget mWidescreenBoardWidget;

		// Token: 0x040009FE RID: 2558
		public Profile.Profile m_Profile = new Profile.Profile();

		// Token: 0x040009FF RID: 2559
		public GameApp.YesNoDialogDelegate mYesNoDialogDelegate;

		// Token: 0x04000A00 RID: 2560
		public ZumaDialog mDialog;

		// Token: 0x04000A01 RID: 2561
		public int mTouchOffsetX;

		// Token: 0x04000A02 RID: 2562
		public int mTouchOffsetY;

		// Token: 0x04000A03 RID: 2563
		private Thread mLoadingThread;

		// Token: 0x04000A04 RID: 2564
		private ThreadStart mLoadingProc;

		// Token: 0x04000A05 RID: 2565
		public bool mLoadLevelSuccess;

		// Token: 0x04000A06 RID: 2566
		public bool StartLoadingComplete;

		// Token: 0x04000A07 RID: 2567
		public int mBoardOffsetX = 85;

		// Token: 0x04000A08 RID: 2568
		public int mBoardUIOffsetX = 53;

		// Token: 0x04000A09 RID: 2569
		public int mOffset160X = 160;

		// Token: 0x04000A0A RID: 2570
		protected GameApp.EXLiveWaiting m_XLiveState = GameApp.EXLiveWaiting.E_WaitingForSignIn;

		// Token: 0x020000D1 RID: 209
		public enum Metrics_AppEventType
		{
			// Token: 0x04000A0C RID: 2572
			Metrics_AppEvent_StartNormal = 1,
			// Token: 0x04000A0D RID: 2573
			Metrics_AppEvent_StartUpgrade,
			// Token: 0x04000A0E RID: 2574
			Metrics_AppEvent_StartInstall,
			// Token: 0x04000A0F RID: 2575
			Metrics_AppEvent_MovedToForeground,
			// Token: 0x04000A10 RID: 2576
			Metrics_AppEvent_StartFromPushNotification
		}

		// Token: 0x020000D2 RID: 210
		// (Invoke) Token: 0x06000BCC RID: 3020
		public delegate void PreBlockCallback();

		// Token: 0x020000D3 RID: 211
		// (Invoke) Token: 0x06000BD0 RID: 3024
		public delegate void YesNoDialogDelegate(int buttonId);

		// Token: 0x020000D4 RID: 212
		public enum EXLiveWaiting
		{
			// Token: 0x04000A12 RID: 2578
			E_NONE,
			// Token: 0x04000A13 RID: 2579
			E_WaitingForSignIn,
			// Token: 0x04000A14 RID: 2580
			E_WaitingForAchivements,
			// Token: 0x04000A15 RID: 2581
			E_Ready
		}
	}
}
