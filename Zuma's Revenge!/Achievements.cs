using System;
using System.Threading;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000006 RID: 6
	public class Achievements : Widget, ButtonListener
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002B78 File Offset: 0x00000D78
		public Achievements()
		{
			if (!GameApp.gApp.mResourceManager.IsGroupLoaded("CommonGame") && !GameApp.gApp.mResourceManager.LoadResources("CommonGame"))
			{
				GameApp.gApp.Shutdown();
			}
			this.mDisplayMode = -1;
			this.mClip = false;
			this.mSelectedScreenState = 0;
			this.mHomeButton = null;
			this.mUpButton = null;
			this.mDownButton = null;
			if (GameApp.mGameRes == 768)
			{
				this.mTitleXOffset = 30f;
			}
			else
			{
				this.mTitleXOffset = 20f;
			}
			this.mNeedsInitScroll = true;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002D3F File Offset: 0x00000F3F
		public override void Dispose()
		{
			this.RemoveAllWidgets(true, true);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002D49 File Offset: 0x00000F49
		public void StartLoading()
		{
			this.mLoadingProc = new ThreadStart(this.LoadingRank);
			this.mLoadDataThread = new Thread(this.mLoadingProc);
			this.mLoadingData = true;
			this.mLoadingDataComplete = false;
			this.mLoadDataThread.Start();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002D88 File Offset: 0x00000F88
		private void LoadingRank()
		{
			this.mAchievementsPages.AddPage();
			this.mAchievementsPages.AddPage();
			this.mAchievementsPages.AddPage();
			this.mAchievementsPages.AddPage();
			this.mAchievementsPages.AddPage();
			this.mAchievementsPages.Resize(0, 0, this.mAchievementsPages.IMAGE_UI_LEADERBOARDS_SHADOW.GetWidth(), (this.mAchievementsPages.IMAGE_UI_LEADERBOARDS_SHADOW.GetHeight() + 30) * this.mAchievementsPages.mNumPages - 100);
			this.mAchievementsScrollWidget.AddWidget(this.mAchievementsPages);
			this.mLoadingDataComplete = true;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002E24 File Offset: 0x00001024
		public void Init()
		{
			this.mAchievementsPages = new AchievementsPages(this);
			this.mAchievementsScrollWidget = new ScrollWidget();
			this.mAchievementsScrollWidget.Resize(Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_LEADERBOARDS_SHADOW)) - GameApp.gApp.mWideScreenXOffset + Common._DS(10), 20 + Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_LEADERBOARDS_SHADOW)), this.IMAGE_UI_LEADERBOARDS_SHADOW.GetWidth() + 30, this.IMAGE_UI_LEADERBOARDS_SHADOW.GetHeight() - 40);
			this.mAchievementsScrollWidget.SetScrollMode(ScrollWidget.ScrollMode.SCROLL_VERTICAL);
			this.mAchievementsScrollWidget.EnableBounce(true);
			this.mAchievementsScrollWidget.EnablePaging(true);
			this.mAchievementsPageControl = new PageControl(this.IMAGE_UI_CHALLENGE_PAGE_INDICATOR);
			this.IMAGE_UI_CHALLENGE_PAGE_INDICATOR.GetCelWidth();
			this.mAchievementsPages.NumPages();
			this.mAchievementsPageControl.SetNumberOfPages(this.mAchievementsPages.NumPages());
			this.mAchievementsPageControl.Move((int)this.mTitleXOffset + (this.mWidth - this.mAchievementsPageControl.mWidth) / 2, Common._DS(145));
			this.mAchievementsPageControl.SetCurrentPage(0);
			this.AddWidget(this.mAchievementsPageControl);
			this.mAchievementsScrollWidget.SetPageControl(this.mAchievementsPageControl);
			this.AddWidget(this.mAchievementsScrollWidget);
			this.mUpButton = new ButtonWidget(7, this);
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_LEADERBOARDS_ARROW_LIGHT);
			this.mUpButton.mButtonImage = imageByID;
			this.mUpButton.mDownImage = Res.GetImageByID(ResID.IMAGE_UI_LEADERBOARDS_ARROW_LIGHT_ON);
			float num = 0f;
			float num2 = 0f;
			this.mUpButton.Resize(Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_LEADERBOARDS_ARROW_LIGHT)) - GameApp.gApp.mWideScreenXOffset + this.mAspectOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_LEADERBOARDS_ARROW_LIGHT)), imageByID.GetWidth(), imageByID.GetHeight());
			this.mUpButton.mNormalRect = new Rect(0, 0, imageByID.GetWidth(), imageByID.GetHeight());
			this.mUpButton.mDownRect = new Rect((int)num, (int)num2, imageByID.GetWidth() - (int)num, imageByID.GetHeight() - (int)num2);
			this.mUpButton.mDoFinger = true;
			this.mUpButton.mVisible = true;
			this.AddWidget(this.mUpButton);
			this.mUpButton.SetVisible(false);
			this.mUpButton.SetDisabled(true);
			this.mDownButton = new ButtonWidget(6, this);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_UI_LEADERBOARDS_ARROW_LIGHTF);
			this.mDownButton.mButtonImage = imageByID2;
			this.mDownButton.mDownImage = Res.GetImageByID(ResID.IMAGE_UI_LEADERBOARDS_ARROW_LIGHTF_ON);
			float num3 = 0f;
			float num4 = 0f;
			this.mDownButton.Resize(Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_LEADERBOARDS_ARROW_LIGHTF)) - GameApp.gApp.mWideScreenXOffset + this.mAspectOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_LEADERBOARDS_ARROW_LIGHTF)), imageByID2.GetWidth(), imageByID2.GetHeight());
			this.mDownButton.mNormalRect = new Rect(0, 0, imageByID2.GetWidth(), imageByID2.GetHeight());
			this.mDownButton.mDownRect = new Rect((int)num3, (int)num4, imageByID2.GetWidth() - (int)num3, imageByID2.GetHeight() - (int)num4);
			this.mDownButton.mDoFinger = true;
			this.mDownButton.mVisible = true;
			this.mDownButton.SetDisabled(true);
			this.AddWidget(this.mDownButton);
			this.mAchievementsScrollWidget.SetPageVertical(1, false);
			this.mDownButton.SetVisible(true);
			this.mUpButton.SetVisible(true);
			this.mCurrentPage = 0;
			this.mEnterScreneLoad = false;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000031B4 File Offset: 0x000013B4
		public override void Update()
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			if (!this.mEnterScreneLoad && GameApp.gApp.mBambooTransition != null && !GameApp.gApp.mBambooTransition.IsInProgress())
			{
				this.mEnterScreneLoad = true;
				this.StartLoading();
			}
			if (this.mAchievementsScrollWidget != null)
			{
				if (this.mAchievementsScrollWidget.GetPageVertical() == 0)
				{
					this.mUpButton.SetVisible(false);
				}
				else
				{
					this.mUpButton.SetVisible(true);
				}
				if (this.mAchievementsScrollWidget.GetPageVertical() == 4)
				{
					this.mDownButton.SetVisible(false);
				}
				else
				{
					this.mDownButton.SetVisible(true);
				}
			}
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				this.mAchievementsScrollWidget.SetVisible(false);
			}
			else
			{
				this.mAchievementsScrollWidget.SetVisible(true);
			}
			if (!this.mLoadingDataComplete)
			{
				ulong num = (ulong)SexyFramework.Common.SexyTime();
				if (num - this.mTicker > 500UL)
				{
					if (this.loadingDot.Length < 6)
					{
						this.loadingDot += ".";
					}
					else
					{
						this.loadingDot = "";
					}
					this.mTicker = num;
				}
			}
			if (!GameApp.gApp.mBambooTransition.IsInProgress() && this.mNeedsInitScroll)
			{
				this.mAchievementsScrollWidget.SetPageVertical(0, true);
				this.mNeedsInitScroll = false;
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00003323 File Offset: 0x00001523
		public float GetTitleXOffset()
		{
			return this.mTitleXOffset;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000332C File Offset: 0x0000152C
		public override void Draw(Graphics g)
		{
			Graphics3D graphics3D = ((g != null) ? g.Get3D() : null);
			g.Translate(this.mX / 2, 0);
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE, -GameApp.gApp.mWideScreenXOffset, 0, this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE.GetWidth() + 21, this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE.GetHeight());
			g.DrawImageMirror(this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE, -GameApp.gApp.mWideScreenXOffset + this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE.GetWidth() + 21, 0, this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE.GetWidth() + 21, this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE.GetHeight());
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_BG_FLOOR, 0, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_FLOOR)), GameApp.gApp.GetScreenWidth(), this.IMAGE_UI_CHALLENGESCREEN_BG_FLOOR.GetHeight());
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGLOUNGE28_STROKE));
			int num = Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END));
			int num2 = GameApp.gApp.GetScreenWidth() - num - this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END.GetWidth() + GameApp.gApp.mWideScreenXOffset;
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP, num + this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END.GetWidth(), Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP)), num2 - num - this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END.GetWidth(), this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP.GetHeight());
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END, num, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END)));
			g.DrawImageMirror(this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END, num2, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END)));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES)) - GameApp.gApp.mWideScreenXOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES)), this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth() + 30, this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetHeight());
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_WOOD, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_WOOD)) - GameApp.gApp.mWideScreenXOffset + this.mAspectOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_WOOD)));
			g.DrawImage(this.IMAGE_UI_LEADERBOARDS_SHADOW, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_LEADERBOARDS_SHADOW)) - GameApp.gApp.mWideScreenXOffset + this.mAspectOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_LEADERBOARDS_SHADOW)));
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 195;
			GameApp.gApp.mUserProfile.m_AchievementMgr.getAchievementsInfo(ref num4, ref num3, ref num6, ref num5);
			int num8 = 270;
			Common._DS(Common._M(200));
			int num9 = Common._DS(Common._M(0));
			string theString = string.Concat(new object[]
			{
				num4,
				" / ",
				num3,
				" ",
				TextManager.getInstance().getString(93)
			});
			int theX = num8;
			int theY = num9 + num7;
			g.SetColor(255, 255, 255, 255);
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGEXOTICA38_BLACK));
			g.DrawString(theString, theX, theY);
			theString = string.Concat(new object[] { " G : ", num6, " / ", num5 });
			int num10 = 810;
			theX = num10 - this.mTitleFont.StringWidth(theString);
			theY = num9 + num7;
			num9 += 32;
			g.DrawString(theString, theX, theY);
			graphics3D.SetMasking(Graphics3D.EMaskMode.MASKMODE_WRITE_MASKONLY);
			g.FillRect(260, 160, 588, 40);
			graphics3D.SetMasking(Graphics3D.EMaskMode.MASKMODE_NONE);
			g.Translate(-this.mX / 2, 0);
			base.DeferOverlay(9);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000036F8 File Offset: 0x000018F8
		public override void DrawOverlay(Graphics g)
		{
			g.Translate(this.mX / 2, 0);
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_BG_SIDE, -GameApp.gApp.mWideScreenXOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_SIDE)));
			g.DrawImageMirror(this.IMAGE_UI_CHALLENGESCREEN_BG_SIDE, GameApp.gApp.GetScreenWidth() + GameApp.gApp.mWideScreenXOffset - this.IMAGE_UI_CHALLENGESCREEN_BG_SIDE.GetWidth(), Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_SIDE)));
			g.DrawImageMirror(this.IMAGE_GUI_Achievements_PEDESTAL, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1)) - GameApp.gApp.mWideScreenXOffset - Common._DS(30), Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1)) + this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1.GetHeight() + Common._DS(15));
			g.DrawImage(this.IMAGE_GUI_Achievements_PEDESTAL, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2)) - GameApp.gApp.mWideScreenXOffset - Common._DS(20) + this.mAspectOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2)) + this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2.GetHeight() - Common._DS(15));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1)) - GameApp.gApp.mWideScreenXOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1)) + Common._DS(120));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2)) - GameApp.gApp.mWideScreenXOffset + this.mAspectOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2)) + Common._DS(120));
			g.SetColor(255, 255, 255, 255);
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGEXOTICA100_GAUNTLET));
			string @string = TextManager.getInstance().getString(860);
			float num = (float)g.GetFont().StringWidth(@string);
			int num2 = 0;
			if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_RU || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_PL)
			{
				num2 = 15;
			}
			else if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CHT)
			{
				num2 = 20;
			}
			g.DrawString(@string, (int)this.mTitleXOffset + Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_WOOD)) - GameApp.gApp.mWideScreenXOffset + (int)(((float)this.IMAGE_UI_CHALLENGESCREEN_WOOD.GetWidth() - num) / 2f), Common._DS(135) + num2);
			g.DrawImage(this.IMAGE_UI_LEADERBOARDS_BOSSES, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_LEADERBOARDS_BOSSES)) - GameApp.gApp.mWideScreenXOffset + this.mAspectOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_LEADERBOARDS_BOSSES)));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_FRUIT, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_FRUIT)) - GameApp.gApp.mWideScreenXOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_FRUIT)));
			g.DrawImage(this.IMAGE_UI_LEADERBOARDS_LEAVES2, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_LEADERBOARDS_LEAVES2)) - GameApp.gApp.mWideScreenXOffset + GameApp.gApp.GetScreenRect().mX / 2 + this.mAspectOffset + 10, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_LEADERBOARDS_LEAVES2)));
			g.Translate(-this.mX / 2, 0);
			if (!this.mLoadingDataComplete)
			{
				g.PushState();
				g.Translate(-this.mX, -this.mY);
				g.SetColor(0, 0, 0, 130);
				g.FillRect(Common._S(-80), 0, GameApp.gApp.mWidth + Common._S(160), GameApp.gApp.mHeight);
				g.PopState();
				g.SetFont(this.mLoadingFont);
				g.DrawString(TextManager.getInstance().getString(581) + this.loadingDot, GameApp.gApp.GetScreenWidth() / 2 - 100, GameApp.gApp.mHeight / 2);
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00003ADC File Offset: 0x00001CDC
		public bool ProcessHardwareBackButton()
		{
			GameApp.gApp.OnHardwareBackButtonPressProcessed();
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return true;
			}
			Dialog dialog = GameApp.gApp.GetDialog(0);
			if (dialog != null)
			{
				GameApp.gApp.DialogButtonDepress(0, 0);
				return false;
			}
			GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.mMainMenu.HideAchievements);
			GameApp.gApp.ToggleBambooTransition();
			return true;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003B60 File Offset: 0x00001D60
		public void ButtonDepress(int id)
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			if (this.mHomeButton != null && this.mHomeButton.mId == id)
			{
				GameApp.gApp.ToggleBambooTransition();
				GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.mMainMenu.HideAchievements);
				return;
			}
			if (this.mUpButton != null && this.mUpButton.mId == id)
			{
				this.mAchievementsScrollWidget.PreviousVertPage();
				return;
			}
			if (this.mDownButton != null && this.mDownButton.mId == id)
			{
				this.mAchievementsScrollWidget.NextVertPage();
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00003C13 File Offset: 0x00001E13
		public void ButtonPress(int id)
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BUTTON2));
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003C47 File Offset: 0x00001E47
		public void ButtonPress(int theId, int theClickCount)
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BUTTON2));
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00003C7B File Offset: 0x00001E7B
		public void ButtonMouseEnter(int id)
		{
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003C7D File Offset: 0x00001E7D
		public void ButtonDownTick(int theId)
		{
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00003C7F File Offset: 0x00001E7F
		public void ButtonMouseLeave(int theId)
		{
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003C81 File Offset: 0x00001E81
		public void ButtonMouseMove(int theId, int theX, int theY)
		{
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00003C84 File Offset: 0x00001E84
		public new virtual void TouchEnded(SexyAppBase.Touch touch)
		{
			int mX = touch.location.mX;
			int mY = touch.location.mY;
			this.MouseUp(mX, mY, 1);
		}

		// Token: 0x0400001F RID: 31
		private int mSelectedScreenState;

		// Token: 0x04000020 RID: 32
		protected ButtonWidget mHomeButton;

		// Token: 0x04000021 RID: 33
		protected ButtonWidget mUpButton;

		// Token: 0x04000022 RID: 34
		protected ButtonWidget mDownButton;

		// Token: 0x04000023 RID: 35
		protected int mDisplayMode;

		// Token: 0x04000024 RID: 36
		protected int mBounceCount;

		// Token: 0x04000025 RID: 37
		protected AchievementsPages mAchievementsPages;

		// Token: 0x04000026 RID: 38
		protected PageControl mAchievementsPageControl;

		// Token: 0x04000027 RID: 39
		protected ScrollWidget mAchievementsScrollWidget;

		// Token: 0x04000028 RID: 40
		protected bool mNeedsInitScroll;

		// Token: 0x04000029 RID: 41
		protected float mTitleXOffset;

		// Token: 0x0400002A RID: 42
		protected int mAspectOffset = 30;

		// Token: 0x0400002B RID: 43
		protected Image IMAGE_UI_CHALLENGE_PAGE_INDICATOR = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGE_PAGE_INDICATOR);

		// Token: 0x0400002C RID: 44
		protected Image IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE);

		// Token: 0x0400002D RID: 45
		protected Image IMAGE_UI_CHALLENGESCREEN_BG_FLOOR = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_FLOOR);

		// Token: 0x0400002E RID: 46
		protected Image IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END);

		// Token: 0x0400002F RID: 47
		protected Image IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP);

		// Token: 0x04000030 RID: 48
		protected Image IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES);

		// Token: 0x04000031 RID: 49
		protected Image IMAGE_UI_CHALLENGESCREEN_WOOD = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_WOOD);

		// Token: 0x04000032 RID: 50
		protected Image IMAGE_UI_LEADERBOARDS_LEAVES2 = Res.GetImageByID(ResID.IMAGE_UI_LEADERBOARDS_LEAVES2);

		// Token: 0x04000033 RID: 51
		protected Image IMAGE_UI_CHALLENGESCREEN_BG_SIDE = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_SIDE);

		// Token: 0x04000034 RID: 52
		protected Image IMAGE_GUI_Achievements_PEDESTAL = Res.GetImageByID(ResID.IMAGE_GUI_TIKITEMPLE_PEDESTAL);

		// Token: 0x04000035 RID: 53
		protected Image IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1 = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1);

		// Token: 0x04000036 RID: 54
		protected Image IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2 = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2);

		// Token: 0x04000037 RID: 55
		protected Image IMAGE_UI_LEADERBOARDS_BOSSES = Res.GetImageByID(ResID.IMAGE_UI_LEADERBOARDS_BOSSES);

		// Token: 0x04000038 RID: 56
		protected Image IMAGE_UI_CHALLENGESCREEN_FRUIT = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_FRUIT);

		// Token: 0x04000039 RID: 57
		protected Image IMAGE_UI_LEADERBOARDS_SHADOW = Res.GetImageByID(ResID.IMAGE_UI_LEADERBOARDS_SHADOW2);

		// Token: 0x0400003A RID: 58
		public float mXOff;

		// Token: 0x0400003B RID: 59
		protected int mCurrentPage;

		// Token: 0x0400003C RID: 60
		private Thread mLoadDataThread;

		// Token: 0x0400003D RID: 61
		private ThreadStart mLoadingProc;

		// Token: 0x0400003E RID: 62
		private bool mLoadingData;

		// Token: 0x0400003F RID: 63
		private bool mLoadingDataComplete;

		// Token: 0x04000040 RID: 64
		private Font mLoadingFont = Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_BASE);

		// Token: 0x04000041 RID: 65
		private Font mTitleFont = Res.GetFontByID(ResID.FONT_SHAGEXOTICA38_BLACK);

		// Token: 0x04000042 RID: 66
		private string loadingDot = "";

		// Token: 0x04000043 RID: 67
		private ulong mTicker = (ulong)SexyFramework.Common.SexyTime();

		// Token: 0x04000044 RID: 68
		private bool mEnterScreneLoad;

		// Token: 0x02000007 RID: 7
		private enum ButtonState
		{
			// Token: 0x04000046 RID: 70
			AdvStats_Btn,
			// Token: 0x04000047 RID: 71
			HardAdvStats_Btn,
			// Token: 0x04000048 RID: 72
			Challenge_Btn,
			// Token: 0x04000049 RID: 73
			IronFrog_Btn,
			// Token: 0x0400004A RID: 74
			MoreStats_Btn,
			// Token: 0x0400004B RID: 75
			Back_Btn,
			// Token: 0x0400004C RID: 76
			Next_Btn,
			// Token: 0x0400004D RID: 77
			Prev_Btn
		}
	}
}
