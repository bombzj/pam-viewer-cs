using System;
using System.Threading;
using Microsoft.Xna.Framework.GamerServices;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x020000F2 RID: 242
	public class LeaderBoards : Widget, ButtonListener
	{
		// Token: 0x06000CF2 RID: 3314 RVA: 0x0007DBC4 File Offset: 0x0007BDC4
		public LeaderBoards()
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

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0007DD8B File Offset: 0x0007BF8B
		public override void Dispose()
		{
			this.RemoveAllWidgets(true, true);
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0007DD98 File Offset: 0x0007BF98
		public void Init()
		{
			this.mLeaderBoardsPages = new LeaderBoardsPages(this);
			this.mLeaderBoardsScrollWidget = new ScrollWidget();
			this.mLeaderBoardsScrollWidget.Resize(Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_LEADERBOARDS_SHADOW)) - GameApp.gApp.mWideScreenXOffset + Common._DS(10), 20 + Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_LEADERBOARDS_SHADOW)), this.IMAGE_UI_LEADERBOARDS_SHADOW.GetWidth() + 30, this.IMAGE_UI_LEADERBOARDS_SHADOW.GetHeight() - 40);
			this.mLeaderBoardsScrollWidget.SetScrollMode(ScrollWidget.ScrollMode.SCROLL_VERTICAL);
			this.mLeaderBoardsScrollWidget.EnableBounce(true);
			this.mLeaderBoardsScrollWidget.EnablePaging(true);
			this.mLeaderBoardsScrollWidget.mLoadPage = new ScrollWidget.ExternalLoadPage(this.PageLoading);
			this.mLeaderBoardsPageControl = new PageControl(this.IMAGE_UI_CHALLENGE_PAGE_INDICATOR);
			this.IMAGE_UI_CHALLENGE_PAGE_INDICATOR.GetCelWidth();
			this.mLeaderBoardsPages.NumPages();
			this.mLeaderBoardsPageControl.SetNumberOfPages(this.mLeaderBoardsPages.NumPages());
			this.mLeaderBoardsPageControl.Move((int)this.mTitleXOffset + (this.mWidth - this.mLeaderBoardsPageControl.mWidth) / 2, Common._DS(145));
			this.mLeaderBoardsPageControl.SetCurrentPage(0);
			this.AddWidget(this.mLeaderBoardsPageControl);
			this.mLeaderBoardsScrollWidget.SetPageControl(this.mLeaderBoardsPageControl);
			this.AddWidget(this.mLeaderBoardsScrollWidget);
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
			this.mLeaderBoardsScrollWidget.SetPageVertical(1, false);
			this.mDownButton.SetVisible(true);
			this.mUpButton.SetVisible(true);
			this.mCurrentPage = 0;
			this.mDataPage = 0;
			this.mEnterScreneLoad = false;
			this.mFrogStr = TextManager.getInstance().getString(57);
			this.mScoreStr = TextManager.getInstance().getString(669);
			this.mScoreStr = this.mScoreStr.Substring(0, this.mScoreStr.Length - 1);
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0007E18C File Offset: 0x0007C38C
		public void PageLoading(int ranking)
		{
			bool flag;
			if (ranking != 0)
			{
				if (!this.mCanPageDown)
				{
					this.mPageDown = false;
					this.mLeaderBoardsScrollWidget.SetPage(0, 1, true);
					return;
				}
				this.mCurrentPage++;
				this.mDataPage++;
				this.mPageDown = true;
				flag = true;
			}
			else
			{
				if (!this.mCanPageUp)
				{
					this.mPageUp = false;
					this.mLeaderBoardsScrollWidget.SetPage(0, 1, true);
					return;
				}
				this.mCurrentPage--;
				this.mDataPage--;
				this.mPageUp = true;
				flag = true;
			}
			if (flag)
			{
				this.mLoadPage = 1;
			}
			this.mLeaderBoardsScrollWidget.SetPageVertical(ranking, true);
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0007E240 File Offset: 0x0007C440
		public void StartLoading(int ranking)
		{
			this.mCurrentPage = ranking;
			this.mLoadingProc = new ThreadStart(this.LoadingRank);
			this.mLoadDataThread = new Thread(this.mLoadingProc);
			this.mLoadingData = true;
			this.mLoadingDataComplete = false;
			this.mLeaderBoardsScrollWidget.SetVisible(false);
			this.mLoadDataThread.Start();
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0007E29C File Offset: 0x0007C49C
		private void LoadingRank()
		{
			if (GameApp.USE_XBOX_SERVICE && !GameApp.USE_TRIAL_VERSION)
			{
				this.readLeaderboard();
				this.mLeaderBoardsScrollWidget.SetDisabled(true);
				return;
			}
			this.mLoadingDataComplete = true;
			this.mLeaderBoardsScrollWidget.SetVisible(true);
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0007E2D4 File Offset: 0x0007C4D4
		public void LoadOfflineRanking()
		{
			ulong num = (ulong)SexyFramework.Common.SexyTime();
			ulong num2;
			do
			{
				num2 = (ulong)SexyFramework.Common.SexyTime();
			}
			while (num2 - num <= 1000UL);
			if (!this.mLeaderBoardsScrollWidget.HasWidget(this.mLeaderBoardsPages))
			{
				this.mLeaderBoardsPages.AddPage(this.mCurrentPage, false);
				this.mLeaderBoardsPages.Resize(0, 0, this.mLeaderBoardsPages.IMAGE_UI_LEADERBOARDS_SHADOW.GetWidth(), (this.mLeaderBoardsPages.IMAGE_UI_LEADERBOARDS_SHADOW.GetHeight() + 30) * this.mLeaderBoardsPages.mNumPages * 3);
				this.mLeaderBoardsScrollWidget.AddWidget(this.mLeaderBoardsPages);
			}
			else
			{
				this.mLeaderBoardsPages.AddPage(this.mCurrentPage, true);
			}
			this.mLoadingDataComplete = true;
			this.mLeaderBoardsScrollWidget.SetVisible(true);
			this.mLeaderBoardsScrollWidget.SetPageVertical(1, false);
			this.mCurrentPage = 1;
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0007E3AC File Offset: 0x0007C5AC
		public void readLeaderboard()
		{
			try
			{
				SignedInGamer signedInGamer = Gamer.SignedInGamers[0];
				LeaderboardIdentity leaderboardIdentity = LeaderboardIdentity.Create(0, 0);
				if (this.mCurrentPage == 0)
				{
					LeaderboardReader.BeginRead(leaderboardIdentity, null, 4, new AsyncCallback(this.LeaderboardReadCallback), signedInGamer);
				}
				else if (this.mPageUp && this.mLeaderboardReader.CanPageUp)
				{
					this.mPageUp = false;
					this.mLeaderboardReader.BeginPageUp(new AsyncCallback(this.LeaderboardPageUpCallback), signedInGamer);
				}
				else if (this.mPageDown && this.mLeaderboardReader.CanPageDown)
				{
					this.mPageDown = false;
					this.mLeaderboardReader.BeginPageDown(new AsyncCallback(this.LeaderboardPageDownCallback), signedInGamer);
				}
			}
			catch (Exception)
			{
				if (GameApp.gApp.mMainMenu != null && GameApp.gApp.mMainMenu.mState == MainMenu_State.State_LeaderBoards)
				{
					GameApp.gApp.DoGenericDialog("", TextManager.getInstance().getString(59), true, new GameApp.PreBlockCallback(this.ReturnMain), Common._DS(100));
				}
			}
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0007E4BC File Offset: 0x0007C6BC
		protected void LeaderboardPageDownCallback(IAsyncResult result)
		{
			SignedInGamer signedInGamer = result.AsyncState as SignedInGamer;
			if (signedInGamer != null)
			{
				try
				{
					this.mLeaderboardReader.EndPageDown(result);
					this.mCanPageUp = this.mLeaderboardReader.CanPageUp;
					this.mCanPageDown = this.mLeaderboardReader.CanPageDown;
					this.mUpButton.SetVisible(this.mCanPageUp);
					this.mDownButton.SetVisible(this.mCanPageDown);
					if (!this.mLeaderBoardsScrollWidget.HasWidget(this.mLeaderBoardsPages))
					{
						this.mLeaderBoardsPages.AddPage(this.mCurrentPage, false, this.mLeaderboardReader);
						this.mLeaderBoardsPages.Resize(0, 0, this.mLeaderBoardsPages.IMAGE_UI_LEADERBOARDS_SHADOW.GetWidth(), (this.mLeaderBoardsPages.IMAGE_UI_LEADERBOARDS_SHADOW.GetHeight() + 30) * this.mLeaderBoardsPages.mNumPages * 3);
						this.mLeaderBoardsScrollWidget.AddWidget(this.mLeaderBoardsPages);
					}
					else
					{
						this.mLeaderBoardsPages.AddPage(this.mCurrentPage, true, this.mLeaderboardReader);
					}
				}
				catch (Exception)
				{
					if (GameApp.gApp.mMainMenu != null && GameApp.gApp.mMainMenu.mState == MainMenu_State.State_LeaderBoards)
					{
						GameApp.gApp.DoGenericDialog("", TextManager.getInstance().getString(59), true, new GameApp.PreBlockCallback(this.ReturnMain), Common._DS(100));
					}
				}
			}
			this.mLoadingDataComplete = true;
			this.mLeaderBoardsScrollWidget.SetDisabled(false);
			this.mLeaderBoardsScrollWidget.SetVisible(true);
			this.mLeaderBoardsScrollWidget.SetPageVertical(1, false);
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0007E650 File Offset: 0x0007C850
		protected void LeaderboardPageUpCallback(IAsyncResult result)
		{
			SignedInGamer signedInGamer = result.AsyncState as SignedInGamer;
			if (signedInGamer != null)
			{
				try
				{
					this.mLeaderboardReader.EndPageUp(result);
					this.mCanPageUp = this.mLeaderboardReader.CanPageUp;
					this.mCanPageDown = this.mLeaderboardReader.CanPageDown;
					this.mUpButton.SetVisible(this.mCanPageUp);
					this.mDownButton.SetVisible(this.mCanPageDown);
					if (!this.mLeaderBoardsScrollWidget.HasWidget(this.mLeaderBoardsPages))
					{
						this.mLeaderBoardsPages.AddPage(this.mCurrentPage, false, this.mLeaderboardReader);
						this.mLeaderBoardsPages.Resize(0, 0, this.mLeaderBoardsPages.IMAGE_UI_LEADERBOARDS_SHADOW.GetWidth(), (this.mLeaderBoardsPages.IMAGE_UI_LEADERBOARDS_SHADOW.GetHeight() + 30) * this.mLeaderBoardsPages.mNumPages * 3);
						this.mLeaderBoardsScrollWidget.AddWidget(this.mLeaderBoardsPages);
					}
					else
					{
						this.mLeaderBoardsPages.AddPage(this.mCurrentPage, true, this.mLeaderboardReader);
					}
				}
				catch (Exception)
				{
					if (GameApp.gApp.mMainMenu != null && GameApp.gApp.mMainMenu.mState == MainMenu_State.State_LeaderBoards)
					{
						GameApp.gApp.DoGenericDialog("", TextManager.getInstance().getString(59), true, new GameApp.PreBlockCallback(this.ReturnMain), Common._DS(100));
					}
				}
			}
			this.mLoadingDataComplete = true;
			this.mLeaderBoardsScrollWidget.SetDisabled(false);
			this.mLeaderBoardsScrollWidget.SetVisible(true);
			this.mLeaderBoardsScrollWidget.SetPageVertical(1, false);
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x0007E7E4 File Offset: 0x0007C9E4
		protected void LeaderboardReadCallback(IAsyncResult result)
		{
			SignedInGamer signedInGamer = result.AsyncState as SignedInGamer;
			if (signedInGamer != null)
			{
				try
				{
					this.mLeaderboardReader = LeaderboardReader.EndRead(result);
					this.mCanPageUp = this.mLeaderboardReader.CanPageUp;
					this.mCanPageDown = this.mLeaderboardReader.CanPageDown;
					this.mUpButton.SetVisible(this.mCanPageUp);
					this.mDownButton.SetVisible(this.mCanPageDown);
					if (!this.mLeaderBoardsScrollWidget.HasWidget(this.mLeaderBoardsPages))
					{
						this.mLeaderBoardsPages.AddPage(this.mCurrentPage, false, this.mLeaderboardReader);
						this.mLeaderBoardsPages.Resize(0, 0, this.mLeaderBoardsPages.IMAGE_UI_LEADERBOARDS_SHADOW.GetWidth(), (this.mLeaderBoardsPages.IMAGE_UI_LEADERBOARDS_SHADOW.GetHeight() + 30) * this.mLeaderBoardsPages.mNumPages * 3);
						this.mLeaderBoardsScrollWidget.AddWidget(this.mLeaderBoardsPages);
					}
					else
					{
						this.mLeaderBoardsPages.AddPage(this.mCurrentPage, true, this.mLeaderboardReader);
					}
				}
				catch (Exception ex)
				{
					this.ShowXboxErrorMessage();
				}
			}
			this.mLeaderBoardsScrollWidget.SetDisabled(false);
			this.mLoadingDataComplete = true;
			this.mLeaderBoardsScrollWidget.SetVisible(true);
			this.mLeaderBoardsScrollWidget.SetPageVertical(1, false);
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0007E930 File Offset: 0x0007CB30
		private void ShowXboxErrorMessage()
		{
			if (GameApp.gApp.mMainMenu != null && GameApp.gApp.mMainMenu.mState == MainMenu_State.State_LeaderBoards)
			{
				GameApp.gApp.DoGenericDialog("", TextManager.getInstance().getString(59), true, new GameApp.PreBlockCallback(this.ReturnMain), Common._DS(100));
			}
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x0007E98C File Offset: 0x0007CB8C
		public override void Update()
		{
			if (this.mLoadPage > 0 && this.mLoadPage < 60)
			{
				this.mLoadPage++;
				if (this.mLoadPage == 60)
				{
					this.StartLoading(this.mCurrentPage);
					this.mLoadPage = 0;
				}
			}
			if (!this.mEnterScreneLoad && GameApp.gApp.mBambooTransition != null && !GameApp.gApp.mBambooTransition.IsInProgress())
			{
				this.mEnterScreneLoad = true;
				this.StartLoading(0);
			}
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				this.mLeaderBoardsScrollWidget.SetVisible(false);
			}
			else
			{
				this.mLeaderBoardsScrollWidget.SetVisible(true);
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
				this.mLeaderBoardsScrollWidget.SetPageVertical(0, true);
				this.mNeedsInitScroll = false;
			}
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0007EAC5 File Offset: 0x0007CCC5
		public float GetTitleXOffset()
		{
			return this.mTitleXOffset;
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0007EAD0 File Offset: 0x0007CCD0
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
			int num3 = 410;
			int num4 = 165;
			g.SetFont(this.mStatsFont);
			g.SetColor(89, 187, 149);
			g.WriteString("#", num3 - 130 + (int)this.mXOff, num4 + this.mStatsFont.GetAscent(), 0, 1);
			int num5 = 25;
			g.WriteString(this.mFrogStr, num3 + (int)this.mXOff + num5, num4 + this.mStatsFont.GetAscent(), 0, 1);
			int num6 = this.mStatsFont.StringWidth(this.mScoreStr);
			g.WriteString(this.mScoreStr, 800 - num6, num4 + this.mStatsFont.GetAscent(), num6, -1);
			graphics3D.SetMasking(Graphics3D.EMaskMode.MASKMODE_WRITE_MASKONLY);
			g.FillRect(250, 160, 588, 40);
			graphics3D.SetMasking(Graphics3D.EMaskMode.MASKMODE_NONE);
			g.Translate(-this.mX / 2, 0);
			base.DeferOverlay(9);
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0007EE14 File Offset: 0x0007D014
		public override void DrawOverlay(Graphics g)
		{
			g.Translate(this.mX / 2, 0);
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_BG_SIDE, -GameApp.gApp.mWideScreenXOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_SIDE)));
			g.DrawImageMirror(this.IMAGE_UI_CHALLENGESCREEN_BG_SIDE, GameApp.gApp.GetScreenWidth() + GameApp.gApp.mWideScreenXOffset - this.IMAGE_UI_CHALLENGESCREEN_BG_SIDE.GetWidth(), Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_SIDE)));
			g.DrawImageMirror(this.IMAGE_GUI_LeaderBoards_PEDESTAL, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1)) - GameApp.gApp.mWideScreenXOffset - Common._DS(30), Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1)) + this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1.GetHeight() + Common._DS(15));
			g.DrawImage(this.IMAGE_GUI_LeaderBoards_PEDESTAL, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2)) - GameApp.gApp.mWideScreenXOffset - Common._DS(20) + this.mAspectOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2)) + this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2.GetHeight() - Common._DS(15));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1)) - GameApp.gApp.mWideScreenXOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1)) + Common._DS(120));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2)) - GameApp.gApp.mWideScreenXOffset + this.mAspectOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2)) + Common._DS(120));
			g.SetColor(255, 255, 255, 255);
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGEXOTICA100_GAUNTLET));
			string @string = TextManager.getInstance().getString(859);
			float num = (float)g.GetFont().StringWidth(@string);
			int num2 = 0;
			int num3 = 0;
			if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_RU || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_PL)
			{
				num2 = 15;
				num3 = 15;
			}
			else if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CHT)
			{
				num2 = 20;
			}
			g.DrawString(@string, num3 + (int)this.mTitleXOffset + Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_WOOD)) - GameApp.gApp.mWideScreenXOffset + (int)(((float)this.IMAGE_UI_CHALLENGESCREEN_WOOD.GetWidth() - num) / 2f), Common._DS(135) + num2);
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

		// Token: 0x06000D02 RID: 3330 RVA: 0x0007F200 File Offset: 0x0007D400
		public bool ProcessHardwareBackButton()
		{
			GameApp.gApp.OnHardwareBackButtonPressProcessed();
			Dialog dialog = GameApp.gApp.GetDialog(0);
			if (dialog != null)
			{
				GameApp.gApp.DialogButtonDepress(0, 0);
				return false;
			}
			GameApp.gApp.ToggleBambooTransition();
			GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.mMainMenu.HideLeaderBoards);
			return true;
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0007F263 File Offset: 0x0007D463
		public void ReturnMain()
		{
			GameApp.gApp.ToggleBambooTransition();
			GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.mMainMenu.HideLeaderBoards);
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0007F294 File Offset: 0x0007D494
		public void ButtonDepress(int id)
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			if (!this.mLoadingDataComplete)
			{
				return;
			}
			if (this.mHomeButton != null && this.mHomeButton.mId == id)
			{
				GameApp.gApp.ToggleBambooTransition();
				GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.mMainMenu.HideLeaderBoards);
				return;
			}
			if (this.mUpButton != null && this.mUpButton.mId == id)
			{
				if (this.mCanPageUp)
				{
					this.mCurrentPage--;
					this.mPageUp = true;
					this.StartLoading(this.mCurrentPage);
					return;
				}
			}
			else if (this.mDownButton != null && this.mDownButton.mId == id && this.mCanPageDown)
			{
				this.mCurrentPage++;
				this.mPageDown = true;
				this.StartLoading(this.mCurrentPage);
			}
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0007F38C File Offset: 0x0007D58C
		public void ButtonPress(int id)
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			if (!this.mLoadingDataComplete)
			{
				return;
			}
			GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BUTTON2));
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0007F3C9 File Offset: 0x0007D5C9
		public void ButtonPress(int theId, int theClickCount)
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			if (!this.mLoadingDataComplete)
			{
				return;
			}
			GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BUTTON2));
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0007F406 File Offset: 0x0007D606
		public void ButtonMouseEnter(int id)
		{
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x0007F408 File Offset: 0x0007D608
		public void ButtonDownTick(int theId)
		{
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x0007F40A File Offset: 0x0007D60A
		public void ButtonMouseLeave(int theId)
		{
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0007F40C File Offset: 0x0007D60C
		public void ButtonMouseMove(int theId, int theX, int theY)
		{
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0007F40E File Offset: 0x0007D60E
		public override void TouchBegan(SexyAppBase.Touch theTouch)
		{
			if (!this.mLoadingDataComplete)
			{
				return;
			}
			if (this.mDataPage == 0)
			{
				base.TouchMoved(theTouch);
			}
		}

		// Token: 0x04000B89 RID: 2953
		private int mSelectedScreenState;

		// Token: 0x04000B8A RID: 2954
		protected ButtonWidget mHomeButton;

		// Token: 0x04000B8B RID: 2955
		protected ButtonWidget mUpButton;

		// Token: 0x04000B8C RID: 2956
		protected ButtonWidget mDownButton;

		// Token: 0x04000B8D RID: 2957
		protected int mDisplayMode;

		// Token: 0x04000B8E RID: 2958
		protected int mBounceCount;

		// Token: 0x04000B8F RID: 2959
		protected LeaderBoardsPages mLeaderBoardsPages;

		// Token: 0x04000B90 RID: 2960
		protected PageControl mLeaderBoardsPageControl;

		// Token: 0x04000B91 RID: 2961
		protected ScrollWidget mLeaderBoardsScrollWidget;

		// Token: 0x04000B92 RID: 2962
		protected bool mNeedsInitScroll;

		// Token: 0x04000B93 RID: 2963
		protected float mTitleXOffset;

		// Token: 0x04000B94 RID: 2964
		protected int mAspectOffset = 30;

		// Token: 0x04000B95 RID: 2965
		protected Image IMAGE_UI_CHALLENGE_PAGE_INDICATOR = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGE_PAGE_INDICATOR);

		// Token: 0x04000B96 RID: 2966
		protected Image IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE);

		// Token: 0x04000B97 RID: 2967
		protected Image IMAGE_UI_CHALLENGESCREEN_BG_FLOOR = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_FLOOR);

		// Token: 0x04000B98 RID: 2968
		protected Image IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END);

		// Token: 0x04000B99 RID: 2969
		protected Image IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP);

		// Token: 0x04000B9A RID: 2970
		protected Image IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES);

		// Token: 0x04000B9B RID: 2971
		protected Image IMAGE_UI_CHALLENGESCREEN_WOOD = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_WOOD);

		// Token: 0x04000B9C RID: 2972
		protected Image IMAGE_UI_LEADERBOARDS_LEAVES2 = Res.GetImageByID(ResID.IMAGE_UI_LEADERBOARDS_LEAVES2);

		// Token: 0x04000B9D RID: 2973
		protected Image IMAGE_UI_CHALLENGESCREEN_BG_SIDE = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_SIDE);

		// Token: 0x04000B9E RID: 2974
		protected Image IMAGE_GUI_LeaderBoards_PEDESTAL = Res.GetImageByID(ResID.IMAGE_GUI_TIKITEMPLE_PEDESTAL);

		// Token: 0x04000B9F RID: 2975
		protected Image IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1 = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1);

		// Token: 0x04000BA0 RID: 2976
		protected Image IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2 = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2);

		// Token: 0x04000BA1 RID: 2977
		protected Image IMAGE_UI_LEADERBOARDS_BOSSES = Res.GetImageByID(ResID.IMAGE_UI_LEADERBOARDS_BOSSES);

		// Token: 0x04000BA2 RID: 2978
		protected Image IMAGE_UI_CHALLENGESCREEN_FRUIT = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_FRUIT);

		// Token: 0x04000BA3 RID: 2979
		protected Image IMAGE_UI_LEADERBOARDS_SHADOW = Res.GetImageByID(ResID.IMAGE_UI_LEADERBOARDS_SHADOW);

		// Token: 0x04000BA4 RID: 2980
		public float mXOff;

		// Token: 0x04000BA5 RID: 2981
		protected int mCurrentPage;

		// Token: 0x04000BA6 RID: 2982
		protected int mDataPage;

		// Token: 0x04000BA7 RID: 2983
		private Thread mLoadDataThread;

		// Token: 0x04000BA8 RID: 2984
		private ThreadStart mLoadingProc;

		// Token: 0x04000BA9 RID: 2985
		private bool mLoadingData;

		// Token: 0x04000BAA RID: 2986
		private bool mLoadingDataComplete;

		// Token: 0x04000BAB RID: 2987
		private Font mLoadingFont = Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_BASE);

		// Token: 0x04000BAC RID: 2988
		private string loadingDot = "";

		// Token: 0x04000BAD RID: 2989
		private ulong mTicker = (ulong)SexyFramework.Common.SexyTime();

		// Token: 0x04000BAE RID: 2990
		private LeaderboardReader mLeaderboardReader;

		// Token: 0x04000BAF RID: 2991
		private bool mCanPageUp;

		// Token: 0x04000BB0 RID: 2992
		private bool mCanPageDown;

		// Token: 0x04000BB1 RID: 2993
		private bool mEnterScreneLoad;

		// Token: 0x04000BB2 RID: 2994
		private bool mPageUp;

		// Token: 0x04000BB3 RID: 2995
		private bool mPageDown;

		// Token: 0x04000BB4 RID: 2996
		private int mLoadPage;

		// Token: 0x04000BB5 RID: 2997
		private Font mStatsFont = Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_STROKE);

		// Token: 0x04000BB6 RID: 2998
		private string mFrogStr;

		// Token: 0x04000BB7 RID: 2999
		private string mScoreStr;

		// Token: 0x020000F3 RID: 243
		private enum ButtonState
		{
			// Token: 0x04000BB9 RID: 3001
			AdvStats_Btn,
			// Token: 0x04000BBA RID: 3002
			HardAdvStats_Btn,
			// Token: 0x04000BBB RID: 3003
			Challenge_Btn,
			// Token: 0x04000BBC RID: 3004
			IronFrog_Btn,
			// Token: 0x04000BBD RID: 3005
			MoreStats_Btn,
			// Token: 0x04000BBE RID: 3006
			Back_Btn,
			// Token: 0x04000BBF RID: 3007
			Next_Btn,
			// Token: 0x04000BC0 RID: 3008
			Prev_Btn
		}
	}
}
