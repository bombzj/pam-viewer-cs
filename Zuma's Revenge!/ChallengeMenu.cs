using System;
using System.Collections.Generic;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x0200004B RID: 75
	public class ChallengeMenu : Widget, ButtonListener, PopAnimListener
	{
		// Token: 0x06000683 RID: 1667 RVA: 0x0002C244 File Offset: 0x0002A444
		public ChallengeMenu(GameApp theApp, MainMenu theMainMenu, bool fromMainMenu)
		{
			this.mApp = theApp;
			this.mMainMenu = theMainMenu;
			this.mCurrentChallengeZone = 0;
			this.mCrownSize = 1f;
			this.mCrownAlpha = 255f;
			this.mCrownZoomType = -1;
			this.mCrownZoomDelay = 0;
			this.mTrophy = null;
			this.mTrophyY = 0f;
			this.mDoBounceTrophy = false;
			this.mTrophyVY = 0f;
			this.mTrophyBounceCount = 0;
			this.mAceFX = null;
			this.mRegularTrophy = null;
			this.mIsAceTrophy = false;
			this.mCrossFadeTrophies = false;
			this.mAceTrophyAlpha = 0f;
			this.mFadeInAceTrophy = false;
			this.mTrophyBounceDelay = 0;
			this.mTrophyFlare = null;
			this.mShowFullAceFX = false;
			this.mCSVisFrame = 0;
			this.mLoopTrophyFlare = false;
			this.mSlideDir = 0;
			this.mXFadeAlpha = 255f;
			this.mTimer = 0;
			this.mSelectedLevel = -1;
			this.mButtons = new List<ButtonWidget>();
			this.mDefaultStringContainer = new ChallengeMenu.DefaultStringContainer();
			this.mHomeButton = null;
			this.mChallengeLevelInfoWidget = null;
			this.mChallengeScrollWidget = null;
			if (GameApp.mGameRes == 768)
			{
				this.mTitleXOffset = 30f;
			}
			else
			{
				this.mTitleXOffset = 0f;
			}
			this.mFromMainMenu = fromMainMenu;
			this.mCueMainSong = false;
			this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE);
			this.IMAGE_UI_CHALLENGESCREEN_BG_FLOOR = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_FLOOR);
			this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES);
			this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END);
			this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP);
			this.IMAGE_UI_CHALLENGESCREEN_WOOD = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_WOOD);
			this.IMAGE_UI_CHALLENGESCREEN_BG_SIDE = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_SIDE);
			this.IMAGE_GUI_TIKITEMPLE_PEDESTAL = Res.GetImageByID(ResID.IMAGE_GUI_TIKITEMPLE_PEDESTAL);
			this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1 = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1);
			this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2 = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2);
			this.IMAGE_UI_CHALLENGESCREEN_DRUMS = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_DRUMS);
			this.IMAGE_UI_CHALLENGESCREEN_FRUIT = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_FRUIT);
			this.IMAGE_UI_LEADERBOARDS_LEAVES2 = Res.GetImageByID(ResID.IMAGE_UI_LEADERBOARDS_LEAVES2);
			this.IMAGE_UI_CHALLENGESCREEN_HOME_BACKING = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_HOME_BACKING);
			this.IMAGE_UI_CHALLENGESCREEN_HOME_SELECT = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_HOME_SELECT);
			this.IMAGE_UI_CHALLENGESCREEN_HOME = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_HOME);
			this.IMAGE_UI_CHALLENGE_PAGE_INDICATOR = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGE_PAGE_INDICATOR);
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0002C49C File Offset: 0x0002A69C
		public override void Dispose()
		{
			this.RemoveAllWidgets(true, true);
			this.mChallengePages = null;
			this.mChallengeScrollWidget = null;
			this.mHomeButton = null;
			this.mChallengeLevelInfoWidget = null;
			this.mChallengeScrollPageControl = null;
			for (int i = 0; i < GlobalChallenge.NUM_CHALLENGE_ZONES; i++)
			{
				GameApp.gApp.DeleteZoneThumbnails(i);
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0002C4F0 File Offset: 0x0002A6F0
		public override void Draw(Graphics g)
		{
			g.Translate(-GameApp.gApp.GetScreenRect().mX / 2, 0);
			int gScreenShake = GlobalChallenge.gScreenShake;
			int num = 0;
			if (GameApp.gLastZone != -1)
			{
				num = ((GameApp.gLastZone == 7 && this.mApp.mUserProfile.mChallengeUnlockState[GameApp.gLastZone - 1, 0] == 0) ? Common._DS(Common._M(635)) : Common._DS(Common._M1(608))) + gScreenShake;
			}
			int num2 = Common._DS(Common._M(500));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE, -GameApp.gApp.mWideScreenXOffset, 0);
			g.DrawImageMirror(this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE, -GameApp.gApp.mWideScreenXOffset + this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE.GetWidth() + 21, 0, this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE.GetWidth() + 21, this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE.GetHeight());
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_BG_FLOOR, 0, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_FLOOR)), GameApp.gApp.GetScreenWidth(), this.IMAGE_UI_CHALLENGESCREEN_BG_FLOOR.GetHeight());
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES)) - GameApp.gApp.mWideScreenXOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES)));
			int num3 = Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END));
			int num4 = GameApp.gApp.GetScreenRect().mWidth - GameApp.gApp.GetScreenRect().mX - num3;
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP, num3 + this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END.GetWidth(), Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP)), num4 - (num3 + this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END.GetWidth()), this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP.GetHeight());
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END, num3, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END)));
			g.DrawImageMirror(this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END, num4, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END)));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_WOOD, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_WOOD)) - GameApp.gApp.mWideScreenXOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_WOOD)));
			if (this.mTrophy != null)
			{
				int num5 = (this.mApp.mHiRes ? 2 : 0);
				int[] array = new int[]
				{
					Common._M(194),
					Common._M1(184) + num5,
					Common._M2(188),
					Common._M3(194),
					Common._M4(188),
					Common._M5(194),
					Common._M6(194)
				};
				SexyPoint[] array2 = new SexyPoint[]
				{
					new SexyPoint(Common._M(-250), Common._M1(-830)),
					new SexyPoint(Common._M2(-246), Common._M3(-850)),
					new SexyPoint(Common._M4(-246), Common._M5(-850)),
					new SexyPoint(Common._M6(-252), Common._M7(-876)),
					new SexyPoint(Common._M(-246), Common._M1(-850)),
					new SexyPoint(Common._M2(-252), Common._M3(-860)),
					new SexyPoint(Common._M4(-248), Common._M5(-838))
				};
				g.DrawImage(this.mTrophy, num + Common._DS(array[GameApp.gLastZone - 1]) - this.mTrophy.mWidth / 2, (int)(this.mTrophyY + (float)GlobalChallenge.gScreenShake));
				if (g.Is3D() && this.mTrophyFlare != null && !this.mDoBounceTrophy)
				{
					Transform transform = new Transform();
					transform.Translate((float)(num + Common._DS(array[GameApp.gLastZone - 1] + array2[GameApp.gLastZone - 1].mX)), (float)(num2 + Common._DS(array2[GameApp.gLastZone - 1].mY)));
					this.mTrophyFlare.SetTransform(transform.GetMatrix());
					this.mTrophyFlare.Draw(g);
				}
				if (g.Is3D() && this.mAceFX != null)
				{
					g.PushState();
					g.ClipRect(Common._DS(Common._M(540)) + gScreenShake, Common._DS(Common._M1(40)), Common._DS(Common._M2(530)), Common._DS(Common._M3(1200)));
					if (this.mShowFullAceFX)
					{
						this.mAceFX.Draw(g);
					}
					else
					{
						this.mAceFX.DrawLayer(g, this.mAceFX.GetLayer("mask"));
					}
					g.PopState();
				}
			}
			base.DeferOverlay(9);
			g.Translate(GameApp.gApp.GetScreenRect().mX / 2, 0);
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0002C9F0 File Offset: 0x0002ABF0
		public override void DrawOverlay(Graphics g)
		{
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_BG_SIDE, -GameApp.gApp.mWideScreenXOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_SIDE)));
			g.DrawImageMirror(this.IMAGE_UI_CHALLENGESCREEN_BG_SIDE, GameApp.gApp.GetScreenWidth() + GameApp.gApp.mWideScreenXOffset - this.IMAGE_UI_CHALLENGESCREEN_BG_SIDE.GetWidth() + GameApp.gApp.GetScreenRect().mX, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_SIDE)));
			g.DrawImageMirror(this.IMAGE_GUI_TIKITEMPLE_PEDESTAL, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1)) - GameApp.gApp.mWideScreenXOffset - Common._DS(30), Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1)) + this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1.GetHeight() + Common._DS(15));
			g.DrawImage(this.IMAGE_GUI_TIKITEMPLE_PEDESTAL, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2)) - GameApp.gApp.mWideScreenXOffset - Common._DS(20) + GameApp.gApp.GetScreenRect().mX, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2)) + this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2.GetHeight() - Common._DS(15));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1)) - GameApp.gApp.mWideScreenXOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1)) + Common._DS(120));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2)) - GameApp.gApp.mWideScreenXOffset + GameApp.gApp.GetScreenRect().mX, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2)) + Common._DS(120));
			g.SetColor(new SexyColor(255, 255, 255, 255));
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGEXOTICA100_GAUNTLET));
			string @string = TextManager.getInstance().getString(782);
			int num = g.GetFont().StringWidth(@string);
			g.DrawString(@string, (int)(this.mTitleXOffset + (float)Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_WOOD)) - (float)GameApp.gApp.mWideScreenXOffset + (float)((this.IMAGE_UI_CHALLENGESCREEN_WOOD.GetWidth() - num) / 2)), Common._DS(135));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_DRUMS, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_DRUMS)) - GameApp.gApp.mWideScreenXOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_DRUMS)));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_FRUIT, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_FRUIT)) - GameApp.gApp.mWideScreenXOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_FRUIT)));
			g.DrawImage(this.IMAGE_UI_LEADERBOARDS_LEAVES2, 42 + Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_LEADERBOARDS_LEAVES2)) - GameApp.gApp.mWideScreenXOffset + GameApp.gApp.GetScreenRect().mX, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_LEADERBOARDS_LEAVES2)));
			if (this.mHomeButton != null)
			{
				g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_HOME_BACKING, 0, 0);
				if (this.mHomeButton.IsButtonDown())
				{
					float num2 = (float)((this.IMAGE_UI_CHALLENGESCREEN_HOME.GetWidth() - this.IMAGE_UI_CHALLENGESCREEN_HOME_SELECT.GetWidth()) / 2);
					float num3 = (float)((this.IMAGE_UI_CHALLENGESCREEN_HOME.GetHeight() - this.IMAGE_UI_CHALLENGESCREEN_HOME_SELECT.GetHeight()) / 2);
					g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_HOME_SELECT, (int)((float)this.mHomeButton.mX + num2 + (float)GameApp.gApp.GetScreenRect().mX), (int)((float)this.mHomeButton.mY + num3));
					return;
				}
				g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_HOME, this.mHomeButton.mX + GameApp.gApp.GetScreenRect().mX, this.mHomeButton.mY);
			}
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0002CDB8 File Offset: 0x0002AFB8
		public override void Update()
		{
			Common._M(0);
			if (this.mTrophyFlare != null && GameApp.gApp.Is3DAccelerated() && !this.mDoBounceTrophy)
			{
				this.MarkDirty();
				this.mTrophyFlare.Update();
			}
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				this.mChallengeScrollWidget.SetVisible(false);
			}
			else
			{
				this.mChallengeScrollWidget.SetVisible(true);
			}
			if (this.mFromMainMenu && !GameApp.gApp.mBambooTransition.IsInProgress())
			{
				if (this.mChallengeScrollWidget == null)
				{
					this.Init();
				}
				if (this.mChallengeScrollWidget != null)
				{
					this.mChallengeScrollWidget.SetPageHorizontal(1, false);
					this.mChallengeScrollWidget.SetPageHorizontal(0, true);
					this.mChallengePages.PreloadButtonImage(0);
				}
				this.mFromMainMenu = false;
			}
			if (this.mCueMainSong && GameApp.gApp.mBambooTransition != null && !GameApp.gApp.mBambooTransition.IsInProgress())
			{
				this.mApp.PlaySong(1);
				this.mCueMainSong = false;
			}
			if (GlobalChallenge.gScreenShakeTimer > 0)
			{
				this.MarkDirty();
				GlobalChallenge.gScreenShakeTimer--;
				GlobalChallenge.gScreenShake = SexyFramework.Common.Rand(Common._M(10));
				if (GlobalChallenge.gScreenShakeTimer == 0)
				{
					GlobalChallenge.gScreenShake = 0;
				}
			}
			if (this.mCrownZoomType >= 0 && --this.mCrownZoomDelay <= 0)
			{
				this.MarkDirty();
				this.mTimer++;
				int num = Common._M(75) - this.mTimer;
				float num2 = 255f / (float)num;
				this.mCrownAlpha += (float)((int)num2);
				if (this.mCrownAlpha > 255f)
				{
					this.mCrownAlpha = 255f;
				}
				num2 = Common._M(15f) / (float)num;
				this.mCrownSize -= num2;
				if (this.mCrownSize <= 1f)
				{
					if (this.mCrownZoomType == 0)
					{
						this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_MINI_CROWN_IMPACT));
					}
					else
					{
						this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_ACE_MINI_CROWN_IMPACT));
					}
					GlobalChallenge.gScreenShakeTimer = Common._M(15);
					this.mCrownSize = 1f;
					this.mCrownAlpha = 255f;
					if (this.mApp.mUserProfile.mDoChallengeAceTrophyZoom)
					{
						if (this.mApp.mUserProfile.mDoChallengeTrophyZoom)
						{
							this.mCrownZoomType = 1;
							this.mCrownSize = Common._M(16f);
							this.mCrownAlpha = 0f;
							this.mCrownZoomDelay = Common._M(20);
							this.mTimer = 0;
						}
						else
						{
							this.mCrownZoomType = -1;
						}
						this.mApp.mUserProfile.mDoChallengeTrophyZoom = (this.mApp.mUserProfile.mDoChallengeAceTrophyZoom = false);
						return;
					}
					this.mApp.mUserProfile.mDoChallengeTrophyZoom = false;
					this.mCrownZoomType = -1;
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_LEVELS_UNLOCKED));
					return;
				}
			}
			else if (this.mCrownZoomType == -1 && --this.mCrownZoomDelay <= 0 && this.mDoBounceTrophy)
			{
				this.MarkDirty();
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0002D0D8 File Offset: 0x0002B2D8
		public void ShowChallengeLevelInfo(int theZoneNum, int theLevelNum, string theLevelName)
		{
			if (this.mChallengeLevelInfoWidget != null)
			{
				this.mChallengeState = ChallengeMenu.EChallengeState.State_LevelInfo;
				this.mChallengeLevelInfoWidget.SetLevel(theZoneNum, theLevelNum, theLevelName);
				this.mChallengeLevelInfoWidget.SetVisible(true);
				this.mChallengeLevelInfoWidget.SetDisabled(false);
				this.SetFocus(this.mChallengeLevelInfoWidget);
				int theNewX;
				if (GameApp.gApp.IsWideScreen())
				{
					theNewX = (int)((float)(GameApp.gApp.GetScreenRect().mWidth - GameApp.gApp.GetScreenRect().mX - this.mChallengeLevelInfoWidget.GetWidth()) * 0.5f);
				}
				else
				{
					theNewX = (int)((float)(GameApp.gApp.GetScreenRect().mWidth - this.mChallengeLevelInfoWidget.mWidth) * 0.5f);
				}
				int mY = this.mChallengeLevelInfoWidget.mY;
				this.mChallengeLevelInfoWidget.Move(theNewX, mY);
			}
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0002D1A8 File Offset: 0x0002B3A8
		public void HideChallengeLevelInfo()
		{
			this.mChallengeState = ChallengeMenu.EChallengeState.State_Challenge;
			if (this.mChallengeLevelInfoWidget != null)
			{
				this.mChallengeLevelInfoWidget.SetLevel(-1, -1, "");
				this.mChallengeLevelInfoWidget.SetVisible(false);
				this.mChallengeLevelInfoWidget.SetDisabled(true);
				this.SetFocus(this);
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0002D1F5 File Offset: 0x0002B3F5
		public override void MouseUp(int x, int y)
		{
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0002D1F8 File Offset: 0x0002B3F8
		public void Init()
		{
			if (!GameApp.gApp.mResourceManager.IsGroupLoaded("CommonGame") && !GameApp.gApp.mResourceManager.LoadResources("CommonGame"))
			{
				GameApp.gApp.ShowResourceError(true);
				GameApp.gApp.Shutdown();
			}
			Common._M(0);
			this.mChallengeState = ChallengeMenu.EChallengeState.State_Challenge;
			this.mChallengePages = new ChallengeMenuScrollContainer(this);
			this.mChallengeScrollWidget = new ScrollWidget();
			this.mChallengeScrollWidget.Resize(Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES)) - GameApp.gApp.mWideScreenXOffset - GameApp.gApp.GetScreenRect().mX, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES)), this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth(), this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetHeight());
			this.mChallengeScrollWidget.SetScrollMode(ScrollWidget.ScrollMode.SCROLL_HORIZONTAL);
			this.mChallengeScrollWidget.EnableBounce(true);
			this.mChallengeScrollWidget.EnablePaging(true);
			this.mChallengeScrollWidget.AddWidget(this.mChallengePages);
			this.mChallengeScrollPageControl = new PageControl(this.IMAGE_UI_CHALLENGE_PAGE_INDICATOR);
			this.IMAGE_UI_CHALLENGE_PAGE_INDICATOR.GetCelWidth();
			this.mChallengePages.NumPages();
			this.mChallengeScrollPageControl.SetNumberOfPages(this.mChallengePages.NumPages());
			this.mChallengeScrollPageControl.Move((int)(this.mTitleXOffset + (float)((this.mWidth - this.mChallengeScrollPageControl.mWidth) / 2) - (float)GameApp.gApp.GetScreenRect().mX), Common._DS(145));
			this.mChallengeScrollPageControl.SetCurrentPage(0);
			this.AddWidget(this.mChallengeScrollPageControl);
			this.mChallengeScrollWidget.SetPageControl(this.mChallengeScrollPageControl);
			this.AddWidget(this.mChallengeScrollWidget);
			if (this.mFromMainMenu)
			{
				this.mChallengeScrollWidget.SetPageHorizontal(this.mChallengePages.NumPages(), false);
			}
			else
			{
				this.mChallengeScrollWidget.SetPageHorizontal(0, false);
			}
			this.mChallengeLevelInfoWidget = new ChallengeLevelInfo(this);
			int theWidth = this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth() + Common._DS(100);
			this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetHeight();
			Common._DS(150);
			this.mChallengeLevelInfoWidget.Resize(0, 0, theWidth, GameApp.gApp.GetScreenRect().mHeight);
			Common.SetupDialog(this.mChallengeLevelInfoWidget);
			this.mChallengeLevelInfoWidget.mPriority = 2147483645;
			this.mChallengeLevelInfoWidget.SetVisible(false);
			this.mChallengeLevelInfoWidget.SetDisabled(true);
			this.AddWidget(this.mChallengeLevelInfoWidget);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0002D472 File Offset: 0x0002B672
		public void RehupChallengeButtons()
		{
			if (this.mChallengePages != null)
			{
				this.mChallengePages.RehupChallengeButtons();
			}
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0002D488 File Offset: 0x0002B688
		public void InitCS()
		{
			if (this.mApp.mUserProfile.mAdvModeVars.mHighestLevelBeat < 10)
			{
				return;
			}
			this.mChallengePages.RehupChallengeButtons();
			this.mMainMenu.RehupButtons();
			this.mCrownZoomType = -1;
			if ((this.mApp.mUserProfile.mDoChallengeTrophyZoom || this.mApp.mUserProfile.mDoChallengeAceTrophyZoom) && GameApp.gApp.Is3DAccelerated())
			{
				this.mCrownZoomType = (this.mApp.mUserProfile.mDoChallengeTrophyZoom ? 0 : 1);
				this.mCrownSize = Common._M(16f);
				this.mCrownAlpha = 0f;
				this.mTimer = 0;
				this.mCrownZoomDelay = 0;
			}
			bool flag = GameApp.gApp.mUserProfile.mDoChallengeCupComplete || GameApp.gApp.mUserProfile.mDoChallengeAceCupComplete;
			bool flag2 = GameApp.gApp.mUserProfile.mUnlockSparklesIdx1 != -1 || GameApp.gApp.mUserProfile.mUnlockSparklesIdx2 != -1;
			if (flag && !flag2)
			{
				this.mChallengeScrollWidget.SetPageHorizontal(0, false);
				this.mChallengeScrollPageControl.SetCurrentPage(0);
				this.mChallengePages.AwardMedal(GameApp.gLastZone, GameApp.gApp.mUserProfile.mDoChallengeAceCupComplete);
			}
			else
			{
				this.SetupChallengeZone(GameApp.gLastZone);
			}
			if (this.mFromMainMenu && this.mChallengeScrollWidget != null)
			{
				this.mChallengeScrollWidget.SetPageHorizontal(this.mChallengePages.NumPages(), false);
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0002D602 File Offset: 0x0002B802
		public void StartChallengeGame()
		{
			this.mApp.StartGauntletMode(this.mChallengeLevelInfoWidget.GetChallengeLevelName(), this.mCSOverRect);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0002D620 File Offset: 0x0002B820
		public virtual void ButtonPress(int id)
		{
			this.ButtonPress(id, 1);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0002D62A File Offset: 0x0002B82A
		public virtual void ButtonPress(int id, int cc)
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BUTTON2));
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0002D660 File Offset: 0x0002B860
		public virtual void ButtonDepress(int id)
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			if (this.mSlideDir != 0)
			{
				return;
			}
			if (id == 0)
			{
				GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.HideChallengeMenu);
				GameApp.gApp.ToggleBambooTransition();
			}
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0002D6C0 File Offset: 0x0002B8C0
		public void SetupChallengeZone(int zone)
		{
			if (this.mAceFX != null)
			{
				this.mAceFX.mEmitAfterTimeline = false;
			}
			if (this.mChallengeScrollWidget != null)
			{
				this.mChallengeScrollWidget.SetPageHorizontal(zone + 1, false);
				this.mChallengeScrollPageControl.SetCurrentPage(zone + 1);
			}
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0002D6FC File Offset: 0x0002B8FC
		public bool ProcessHardwareBackButton()
		{
			GameApp.gApp.OnHardwareBackButtonPressProcessed();
			if (GameApp.gApp.GetDialog(0) != null)
			{
				GameApp.gApp.DialogButtonDepress(0, 0);
				return false;
			}
			if (this.mChallengeLevelInfoWidget != null && this.mChallengeLevelInfoWidget.mVisible && !this.mChallengeLevelInfoWidget.mDisabled)
			{
				this.HideChallengeLevelInfo();
				return false;
			}
			if (this.mChallengeScrollWidget.GetPageHorizontal() > 0)
			{
				this.mChallengeScrollWidget.SetPageHorizontal(0, true);
				return false;
			}
			GameApp.gApp.ToggleBambooTransition();
			GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.mMainMenu.HideChallengeMenu);
			return true;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0002D7A4 File Offset: 0x0002B9A4
		public bool IsReceivingAward()
		{
			bool flag = GameApp.gApp.mUserProfile.mUnlockSparklesIdx1 != -1 || GameApp.gApp.mUserProfile.mUnlockSparklesIdx2 != -1;
			bool flag2 = GameApp.gApp.mUserProfile.mDoChallengeCupComplete || GameApp.gApp.mUserProfile.mDoChallengeAceCupComplete;
			return flag || flag2;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0002D808 File Offset: 0x0002BA08
		public bool HasAcedZone(int theZoneNum)
		{
			bool flag = GameApp.gApp.mUserProfile.mChallengeUnlockState[theZoneNum, 0] == 0;
			if (flag)
			{
				return false;
			}
			bool result = true;
			if (GameApp.gApp.mUserProfile.mDoChallengeAceCupComplete && GameApp.gLastZone == theZoneNum)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < 10; i++)
				{
					int num = GameApp.gApp.mUserProfile.mChallengeUnlockState[theZoneNum, i];
					if (num != 5)
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0002D880 File Offset: 0x0002BA80
		public bool HasBeatZone(int theZoneNum)
		{
			bool flag = GameApp.gApp.mUserProfile.mChallengeUnlockState[theZoneNum, 0] == 0;
			if (flag)
			{
				return false;
			}
			bool result = true;
			if ((GameApp.gApp.mUserProfile.mDoChallengeCupComplete || GameApp.gApp.mUserProfile.mDoChallengeAceCupComplete) && GameApp.gLastZone == theZoneNum)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < 10; i++)
				{
					int num = GameApp.gApp.mUserProfile.mChallengeUnlockState[theZoneNum, i];
					if (num != 4 && num != 5)
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0002D90D File Offset: 0x0002BB0D
		public virtual void ButtonDownTick(int x)
		{
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0002D90F File Offset: 0x0002BB0F
		public virtual void ButtonMouseEnter(int x)
		{
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0002D911 File Offset: 0x0002BB11
		public virtual void ButtonMouseLeave(int x)
		{
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0002D913 File Offset: 0x0002BB13
		public virtual void ButtonMouseMove(int x, int y, int z)
		{
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0002D915 File Offset: 0x0002BB15
		public void PopAnimStopped(int id)
		{
			if (this.mTrophyFlare != null && id == this.mTrophyFlare.mId)
			{
				if (this.mLoopTrophyFlare)
				{
					this.mTrophyFlare.Play("Main");
					return;
				}
				this.mTrophyFlare = null;
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0002D94E File Offset: 0x0002BB4E
		public void PopAnimPlaySample(string theSampleName, int thePan, double theVolume, double theNumSteps)
		{
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0002D950 File Offset: 0x0002BB50
		public PIEffect PopAnimLoadParticleEffect(string theEffectName)
		{
			return null;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0002D953 File Offset: 0x0002BB53
		public bool PopAnimObjectPredraw(int theId, Graphics g, PASpriteInst theSpriteInst, PAObjectInst theObjectInst, PATransform theTransform, SexyColor theColor)
		{
			return true;
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0002D956 File Offset: 0x0002BB56
		public bool PopAnimObjectPostdraw(int theId, Graphics g, PASpriteInst theSpriteInst, PAObjectInst theObjectInst, PATransform theTransform, SexyColor theColor)
		{
			return true;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0002D959 File Offset: 0x0002BB59
		public ImagePredrawResult PopAnimImagePredraw(int theId, PASpriteInst theSpriteInst, PAObjectInst theObjectInst, PATransform theTransform, Image theImage, Graphics g, int theDrawCount)
		{
			return ImagePredrawResult.ImagePredraw_Normal;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0002D95C File Offset: 0x0002BB5C
		public void PopAnimCommand(int theId, string theCommand, string theParam)
		{
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0002D95E File Offset: 0x0002BB5E
		public bool PopAnimCommand(int theId, PASpriteInst theSpriteInst, string theCommand, string theParam)
		{
			this.PopAnimCommand(theId, theCommand, theParam);
			return true;
		}

		// Token: 0x040003A3 RID: 931
		protected ChallengeMenu.EChallengeState mChallengeState;

		// Token: 0x040003A4 RID: 932
		public GameApp mApp;

		// Token: 0x040003A5 RID: 933
		public PIEffect mAceFX;

		// Token: 0x040003A6 RID: 934
		public PopAnim mTrophyFlare;

		// Token: 0x040003A7 RID: 935
		public Rect mCSOverRect;

		// Token: 0x040003A8 RID: 936
		public Image mTrophy;

		// Token: 0x040003A9 RID: 937
		public Image mRegularTrophy;

		// Token: 0x040003AA RID: 938
		public float mTrophyY;

		// Token: 0x040003AB RID: 939
		public bool mDoBounceTrophy;

		// Token: 0x040003AC RID: 940
		public float mTrophyVY;

		// Token: 0x040003AD RID: 941
		public int mTrophyBounceCount;

		// Token: 0x040003AE RID: 942
		public int mTrophyBounceDelay;

		// Token: 0x040003AF RID: 943
		public bool mCrossFadeTrophies;

		// Token: 0x040003B0 RID: 944
		public bool mIsAceTrophy;

		// Token: 0x040003B1 RID: 945
		public bool mShowFullAceFX;

		// Token: 0x040003B2 RID: 946
		public int mCurrentChallengeZone;

		// Token: 0x040003B3 RID: 947
		public float mCrownSize;

		// Token: 0x040003B4 RID: 948
		public float mCrownAlpha;

		// Token: 0x040003B5 RID: 949
		public int mCrownZoomType;

		// Token: 0x040003B6 RID: 950
		public int mCrownZoomDelay;

		// Token: 0x040003B7 RID: 951
		public int mCSVisFrame;

		// Token: 0x040003B8 RID: 952
		public bool mLoopTrophyFlare;

		// Token: 0x040003B9 RID: 953
		public float mXFadeAlpha;

		// Token: 0x040003BA RID: 954
		public int mTimer;

		// Token: 0x040003BB RID: 955
		public int mSelectedLevel;

		// Token: 0x040003BC RID: 956
		public List<ButtonWidget> mButtons;

		// Token: 0x040003BD RID: 957
		public MainMenu mMainMenu;

		// Token: 0x040003BE RID: 958
		public float mAceTrophyAlpha;

		// Token: 0x040003BF RID: 959
		public bool mFadeInAceTrophy;

		// Token: 0x040003C0 RID: 960
		public int mSlideDir;

		// Token: 0x040003C1 RID: 961
		public ChallengeMenuScrollContainer mChallengePages;

		// Token: 0x040003C2 RID: 962
		public PageControl mChallengeScrollPageControl;

		// Token: 0x040003C3 RID: 963
		public ScrollWidget mChallengeScrollWidget;

		// Token: 0x040003C4 RID: 964
		public ButtonWidget mHomeButton;

		// Token: 0x040003C5 RID: 965
		public ChallengeLevelInfo mChallengeLevelInfoWidget;

		// Token: 0x040003C6 RID: 966
		public float mTitleXOffset;

		// Token: 0x040003C7 RID: 967
		public bool mFromMainMenu;

		// Token: 0x040003C8 RID: 968
		public bool mCueMainSong;

		// Token: 0x040003C9 RID: 969
		public ChallengeMenu.DefaultStringContainer mDefaultStringContainer;

		// Token: 0x040003CA RID: 970
		private Image IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE;

		// Token: 0x040003CB RID: 971
		private Image IMAGE_UI_CHALLENGESCREEN_BG_FLOOR;

		// Token: 0x040003CC RID: 972
		private Image IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES;

		// Token: 0x040003CD RID: 973
		private Image IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END;

		// Token: 0x040003CE RID: 974
		private Image IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP;

		// Token: 0x040003CF RID: 975
		private Image IMAGE_UI_CHALLENGESCREEN_WOOD;

		// Token: 0x040003D0 RID: 976
		private Image IMAGE_UI_CHALLENGESCREEN_BG_SIDE;

		// Token: 0x040003D1 RID: 977
		private Image IMAGE_GUI_TIKITEMPLE_PEDESTAL;

		// Token: 0x040003D2 RID: 978
		private Image IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1;

		// Token: 0x040003D3 RID: 979
		private Image IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2;

		// Token: 0x040003D4 RID: 980
		private Image IMAGE_UI_CHALLENGESCREEN_DRUMS;

		// Token: 0x040003D5 RID: 981
		private Image IMAGE_UI_CHALLENGESCREEN_FRUIT;

		// Token: 0x040003D6 RID: 982
		private Image IMAGE_UI_LEADERBOARDS_LEAVES2;

		// Token: 0x040003D7 RID: 983
		private Image IMAGE_UI_CHALLENGESCREEN_HOME_BACKING;

		// Token: 0x040003D8 RID: 984
		private Image IMAGE_UI_CHALLENGESCREEN_HOME_SELECT;

		// Token: 0x040003D9 RID: 985
		private Image IMAGE_UI_CHALLENGESCREEN_HOME;

		// Token: 0x040003DA RID: 986
		private Image IMAGE_UI_CHALLENGE_PAGE_INDICATOR;

		// Token: 0x0200004C RID: 76
		public enum Zoom
		{
			// Token: 0x040003DC RID: 988
			Zooming_Crown,
			// Token: 0x040003DD RID: 989
			Zooming_AceCrown
		}

		// Token: 0x0200004D RID: 77
		protected enum EChallengeState
		{
			// Token: 0x040003DF RID: 991
			State_Challenge,
			// Token: 0x040003E0 RID: 992
			State_LevelInfo
		}

		// Token: 0x0200004E RID: 78
		public class DefaultStringContainer
		{
			// Token: 0x060006A3 RID: 1699 RVA: 0x0002D96B File Offset: 0x0002BB6B
			public DefaultStringContainer()
			{
				this.mDefaultStr = this.NonIfLocked();
			}

			// Token: 0x060006A4 RID: 1700 RVA: 0x0002D97F File Offset: 0x0002BB7F
			public string NonIfLocked()
			{
				return TextManager.getInstance().getString(427);
			}

			// Token: 0x060006A5 RID: 1701 RVA: 0x0002D990 File Offset: 0x0002BB90
			public string IfLocked()
			{
				return TextManager.getInstance().getString(428);
			}

			// Token: 0x060006A6 RID: 1702 RVA: 0x0002D9A1 File Offset: 0x0002BBA1
			public string CanPlayZone()
			{
				return TextManager.getInstance().getString(429);
			}

			// Token: 0x060006A7 RID: 1703 RVA: 0x0002D9B2 File Offset: 0x0002BBB2
			public string ZoneUnlocked()
			{
				return TextManager.getInstance().getString(430);
			}

			// Token: 0x060006A8 RID: 1704 RVA: 0x0002D9C3 File Offset: 0x0002BBC3
			public string NothingSelected()
			{
				return TextManager.getInstance().getString(431);
			}

			// Token: 0x040003E1 RID: 993
			public string mDefaultStr;
		}
	}
}
