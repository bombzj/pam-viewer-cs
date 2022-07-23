using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JeffLib;
using Microsoft.Xna.Framework.GamerServices;
using SexyFramework;
using SexyFramework.AELib;
using SexyFramework.Drivers;
using SexyFramework.File;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;
using ZumasRevenge.Achievement;

namespace ZumasRevenge
{
	// Token: 0x0200006C RID: 108
	public class Board : Widget, ButtonListener
	{
		// Token: 0x06000750 RID: 1872 RVA: 0x0003114E File Offset: 0x0002F34E
		protected void ConsoleCallback(string cmd, List<string> @params)
		{
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00031150 File Offset: 0x0002F350
		protected void DoHitTreasure()
		{
			this.mTreasureWasHit = true;
			this.mTreasureGlowAlphaRate = ZumasRevenge.Common._M(12);
			this.mLevelStats.mNumGemsCleared++;
			int num = (this.mScoreTarget - this.mLevelBeginScore) / 600 * 100;
			if (this.mIsEndless)
			{
				if (num < 200)
				{
					num = 200;
				}
			}
			else if (num < 500)
			{
				num = 500;
			}
			this.mFruitExplodeEffect.Reset();
			StringBuilder stringBuilder = new StringBuilder(TextManager.getInstance().getString(95));
			if (this.GauntletMode())
			{
				num = ZumasRevenge.Common._M(500);
				stringBuilder.Replace("$1", num.ToString());
				if (this.mFruitMultiplier > 1)
				{
					this.AddText(stringBuilder.ToString(), this.mCurTreasure.x, this.mCurTreasure.y, ZumasRevenge.Common._M(1.5f), -1, null);
				}
				else
				{
					this.AddText(stringBuilder.ToString(), this.mCurTreasure.x, this.mCurTreasure.y, ZumasRevenge.Common._M(1.5f), -1, null);
				}
				this.mFruitMultiplier++;
				this.GetBetaStats().SetFruitMultiplier(this.mFruitMultiplier);
			}
			else
			{
				stringBuilder.Replace("$1", num.ToString());
				this.AddText(stringBuilder.ToString(), this.mCurTreasure.x, this.mCurTreasure.y, ZumasRevenge.Common._M(1.5f), -1, null);
			}
			this.GetBetaStats().HitFruit(num);
			this.IncScore(num, false);
			this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_TIKI_HIT));
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00031300 File Offset: 0x0002F500
		protected void SetupEndOfGauntletTransition(bool hit_max_time)
		{
			if (!GameApp.gApp.mResourceManager.IsGroupLoaded("AdventureStats") && !GameApp.gApp.mResourceManager.LoadResources("AdventureStats"))
			{
				GameApp.gApp.ShowResourceError(true);
			}
			this.mGauntletHSIndex = this.mApp.mUserProfile.AddGauntletHighScore((this.mLevel.mZone - 1) * 10 + this.mLevel.mNum, this.mScore, this.mApp.mUserProfile.GetName());
			this.mNewGauntletHS = this.mGauntletHSIndex == 0;
			this.mScoreDisplayPos = ZumasRevenge.Common._M(5f);
			this.mGauntletWidestNameLen = 0;
			this.mGauntletAlpha = 0f;
			this.mGauntletTrophyBounceCount = 0;
			this.mGauntletTrophyDropRate = 0f;
			this.mGauntletTrophyImg = null;
			this.mChallengeTextAlpha = 0f;
			this.CreateChallengeStatsButtons();
			this.CreateChallengeScoreImage();
			this.CreateChallengeHeaderImage();
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x000313F4 File Offset: 0x0002F5F4
		protected void CreateChallengeStatsButtons()
		{
			int num = (int)((float)this.mCStatsFrame.mWidth * 0.5f);
			int num2 = (int)((float)this.mCStatsFrame.mHeight * 0.18f);
			int mX = this.mCStatsFrame.mX;
			int theY = this.mCStatsFrame.mY + (this.mCStatsFrame.mHeight - num2);
			this.mGauntletQuitBtn = this.CreateChallengeStatsButton(11, TextManager.getInstance().getString(456), new Rect(mX, theY, num, num2));
			this.mGauntletRetryBtn = this.CreateChallengeStatsButton(10, TextManager.getInstance().getString(457), new Rect(mX + num, theY, num, num2));
			ButtonWidget[] inButtons = new ButtonWidget[] { this.mGauntletQuitBtn, this.mGauntletRetryBtn };
			ZumasRevenge.Common.SizeButtonsToLabel(inButtons, 2, ZumasRevenge.Common._S(30));
			this.SetMenuBtnEnabled(false);
			this.AddWidget(this.mGauntletRetryBtn);
			this.AddWidget(this.mGauntletQuitBtn);
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x000314EC File Offset: 0x0002F6EC
		protected ButtonWidget CreateChallengeStatsButton(int inButtonID, string inButtonText, Rect inFrame)
		{
			ButtonWidget buttonWidget = ZumasRevenge.Common.MakeButton(inButtonID, this, inButtonText);
			buttonWidget.mBtnNoDraw = true;
			buttonWidget.mVisible = false;
			buttonWidget.mPriority = 2;
			buttonWidget.mDoFinger = true;
			buttonWidget.mHasAlpha = true;
			buttonWidget.mHasTransparencies = true;
			int num = ZumasRevenge.Common._DS(330);
			int num2 = ZumasRevenge.Common._DS(140);
			int theX = (int)((float)inFrame.mX + (float)(inFrame.mWidth - num) * 0.5f);
			int theY = (int)((float)inFrame.mY + (float)(inFrame.mHeight - num2) * 0.5f);
			buttonWidget.Resize(theX, theY, num, num2);
			return buttonWidget;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x00031588 File Offset: 0x0002F788
		protected void CreateChallengeScoreImage()
		{
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGEXOTICA100_GAUNTLET);
			if (this.mChallengePtsText != null)
			{
				if (this.mChallengePtsText.mImage != null)
				{
					this.mChallengePtsText.mImage.Dispose();
					this.mChallengePtsText.mImage = null;
				}
				this.mChallengePtsText = null;
			}
			this.mChallengePtsText = new FwooshImage();
			string theString = SexyFramework.Common.CommaSeperate(this.mScore);
			int num = fontByID.StringWidth(theString) + 5;
			this.mChallengePtsText.mImage = new DeviceImage();
			this.mChallengePtsText.mImage.mApp = this.mApp;
			this.mChallengePtsText.mImage.SetImageMode(true, true);
			this.mChallengePtsText.mImage.AddImageFlags(16U);
			this.mChallengePtsText.mImage.Create(num + 15, fontByID.GetHeight() + 5);
			this.mChallengePtsText.mAlphaDec = 0f;
			this.mChallengePtsText.mMaxSize = ZumasRevenge.Common._M(1.8f);
			this.mChallengePtsText.mSizeInc = ZumasRevenge.Common._M(0.13f);
			Graphics graphics = new Graphics(this.mChallengePtsText.mImage);
			graphics.Get3D().ClearColorBuffer(new SexyColor(0, 0));
			graphics.SetFont(fontByID);
			graphics.SetColor(SexyColor.White);
			graphics.DrawString(theString, 5, fontByID.GetAscent());
			graphics.ClearRenderContext();
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x000316E0 File Offset: 0x0002F8E0
		protected void CreateChallengeHeaderImage()
		{
			this.mChallengeHeaderText = new FwooshImage();
			this.mChallengeHeaderText.mMaxSize = ZumasRevenge.Common._M(1.8f);
			this.mChallengeHeaderText.mSizeInc = ZumasRevenge.Common._M(0.13f);
			if (this.mScore < this.mLevel.mChallengePoints)
			{
				if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_RU)
				{
					this.mChallengeHeaderText.mMaxSize = ZumasRevenge.Common._M(0.6f);
					this.mChallengeHeaderText.mSizeInc = ZumasRevenge.Common._M(0.008f);
				}
				this.mChallengeHeaderText.mImage = Res.GetImageByID(ResID.IMAGE_UI_GAUNTLET_TRYAGAIN) as MemoryImage;
			}
			else if (this.mScore < this.mLevel.mChallengeAcePoints)
			{
				this.mChallengeHeaderText.mImage = Res.GetImageByID(ResID.IMAGE_UI_GAUNTLET_SUCCESS) as MemoryImage;
			}
			else
			{
				this.mChallengeHeaderText.mImage = Res.GetImageByID(ResID.IMAGE_UI_GAUNTLET_VICTORY) as MemoryImage;
			}
			this.mChallengeHeaderText.mAlphaDec = 0f;
			this.mChallengeHeaderText.mDelay = (this.mChallengePtsText.mDelay = ZumasRevenge.Common._M(20));
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x000317FC File Offset: 0x0002F9FC
		protected bool ShouldShowCheckpointPostcard()
		{
			return this.mLevel.mNum == 6;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0003180C File Offset: 0x0002FA0C
		protected void UpdateVortex(bool do_frog_fly_off)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_VORTEX_FACE1);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_BOSS_VORTEX_FACE2);
			Image imageByID3 = Res.GetImageByID(ResID.IMAGE_BOSS_VORTEX_STREAK);
			if (this.mVortexAppear)
			{
				this.mVortexBGAlpha += ZumasRevenge.Common._M(0.75f);
				if (this.mVortexBGAlpha >= 255f)
				{
					this.mVortexBGAlpha = 0f;
					this.mVortexAppear = false;
				}
			}
			if (!this.mVortexAppear && this.mUpdateCnt % ZumasRevenge.Common._M(75) == 0)
			{
				VortexFace vortexFace = new VortexFace();
				this.mVortexFaces.Add(vortexFace);
				vortexFace.mUpdateCount = 0;
				vortexFace.mAngle = MathUtils.FloatRange(0f, 6.2831855f);
				vortexFace.mPct = 0f;
				vortexFace.mImage = ((SexyFramework.Common.Rand() % 2 == 0) ? imageByID : imageByID2);
			}
			float num = ZumasRevenge.Common._M(5f);
			float num2 = ZumasRevenge.Common._M(0.035f);
			for (int i = 0; i < Enumerable.Count<VortexFace>(this.mVortexFaces); i++)
			{
				this.mVortexFaces[i].mUpdateCount++;
				if ((this.mVortexFaces[i].mPct += num2) > num)
				{
					this.mVortexFaces.RemoveAt(i);
					i--;
				}
				else
				{
					this.mVortexFaces[i].mAngle += ZumasRevenge.Common._M(0.01f);
				}
			}
			int num3 = (GameApp.gApp.Is3DAccelerated() ? ZumasRevenge.Common._M(2) : ZumasRevenge.Common._M1(8));
			if (this.mUpdateCnt % num3 == 0)
			{
				VortexBeam vortexBeam = new VortexBeam();
				this.mVortexBeams.Add(vortexBeam);
				float num4 = MathUtils.Distance((float)(this.mWidth / 2), (float)(this.mHeight / 2), (float)ZumasRevenge.Common._S(-80), 0f);
				vortexBeam.mPct = num4 / (float)imageByID3.mHeight;
				vortexBeam.mColor = new SexyColor((int)this.mApp.HSLToRGB(SexyFramework.Common.Rand() % 255, 255, 128));
				float num5 = MathUtils.FloatRange(0f, 6.2831855f);
				if (Math.Abs(num5 - Board.UpdateVortex_last_angle) <= ZumasRevenge.Common._M(0.35f))
				{
					num5 += ZumasRevenge.Common._M(0.35f);
				}
				Board.UpdateVortex_last_angle = num5;
				float num6 = MathUtils.FloatRange(ZumasRevenge.Common._M(5f), ZumasRevenge.Common._M1(10f));
				vortexBeam.mVX = num6 * -(float)Math.Cos((double)num5);
				vortexBeam.mVY = num6 * (float)Math.Sin((double)num5);
				vortexBeam.mAngle = num5;
				vortexBeam.mX = (num4 + num4 * 0.5f) * (float)Math.Cos((double)num5);
				vortexBeam.mY = (num4 + num4 * 0.5f) * -(float)Math.Sin((double)num5);
				vortexBeam.mPctDec = vortexBeam.mPct / ((num4 + num4 * 0.5f) / num6);
			}
			for (int j = 0; j < Enumerable.Count<VortexBeam>(this.mVortexBeams); j++)
			{
				VortexBeam vortexBeam2 = this.mVortexBeams[j];
				vortexBeam2.mX += vortexBeam2.mVX;
				vortexBeam2.mY += vortexBeam2.mVY;
				vortexBeam2.mPct -= vortexBeam2.mPctDec;
				if (vortexBeam2.mPct <= 0f)
				{
					this.mVortexBeams.RemoveAt(j);
					j--;
				}
			}
			if (Board.gNeedsVortexSound && (this.mFrogFlyOff == null || !do_frog_fly_off) && this.mVortexAppear)
			{
				Board.gNeedsVortexSound = false;
			}
			if ((this.mFrogFlyOff == null || !do_frog_fly_off) && (!this.mVortexAppear || this.mVortexBGAlpha >= 255f))
			{
				float num7 = (float)(this.mAdventureWinScreen ? ZumasRevenge.Common._DS(ZumasRevenge.Common._M(340)) : ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(600)));
				float num8 = ZumasRevenge.Common._M(500f);
				float num9 = num7 / num8;
				this.mVortexFrogAngle += ZumasRevenge.Common._M(0.05f);
				if (this.mVortexFrogRadiusExpand)
				{
					if ((this.mVortexFrogRadius += (float)ZumasRevenge.Common._M(4)) >= num7)
					{
						this.mVortexFrogRadius = num7;
						this.mVortexFrogRadiusExpand = false;
						return;
					}
				}
				else if (this.mVortexFrogScale > 0f)
				{
					this.mVortexFrogRadius -= num9;
					this.mVortexFrogScale -= 1f / num8;
					if (this.mVortexFrogScale <= 0f)
					{
						this.mVortexFrogScale = 0f;
						return;
					}
				}
				else if (this.mVortexBGAlpha < 255f && !this.mVortexAppear)
				{
					this.mVortexBGAlpha += ZumasRevenge.Common._M(1.5f);
					if (this.mVortexBGAlpha >= 255f)
					{
						this.mVortexBGAlpha = 255f;
						this.mApp.PlaySong(144, ZumasRevenge.Common._M(0.005f));
						if (do_frog_fly_off)
						{
							this.ContinueToNextLevel();
							if (this.mLevel.mBossIntroBG.GetImage() != null)
							{
								this.InitBossIntroState();
							}
							this.mGameState = GameState.GameState_FinalBossPart1Finished;
							this.mFrog.SetAngle(-3.1415927f);
							this.mFrogFlyOff = new FrogFlyOff();
							this.mFrogFlyOff.JumpIn(this.mFrog, this.mLevel.mFrog.GetCenterX(), this.mLevel.mFrog.GetCenterY(), false);
							return;
						}
					}
				}
			}
			else if (this.mFrogFlyOff != null)
			{
				float num10 = 255f / (float)this.mFrogFlyOff.mFrogJumpTime;
				this.mFrogFlyOff.Update();
				this.mVortexBGAlpha -= num10;
				if (this.mVortexBGAlpha < 0f)
				{
					this.mVortexBGAlpha = 0f;
				}
				if (this.mFrogFlyOff.mTimer > this.mFrogFlyOff.mFrogJumpTime)
				{
					this.mGameState = GameState.GameState_Playing;
					this.mFrogFlyOff.Dispose();
					this.mFrogFlyOff = null;
					this.mVortexFaces.Clear();
					this.mVortexBeams.Clear();
					this.UpdateGunPos(true, ZumasRevenge.Common._SS(this.mWidth) / 2, ZumasRevenge.Common._SS(this.mHeight) / 2);
				}
			}
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00031E50 File Offset: 0x00030050
		protected void InitVortex()
		{
			this.mFrog.SetSlowTimer(ZumasRevenge.Common._M(300));
			this.mVortexAppear = true;
			this.mVortexBGAlpha = 0f;
			this.mVortexFrogRadius = 0f;
			this.mVortexFrogAngle = this.mFrog.GetAngle();
			this.mVortexFrogScale = 1f;
			this.mVortexFrogRadiusExpand = true;
			Board.gNeedsVortexSound = true;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00031EB8 File Offset: 0x000300B8
		protected void InitEndOfTorchLevel()
		{
			this.mFullScreenAlpha = 0;
			this.mFullScreenAlphaRate = ZumasRevenge.Common._M(2);
			this.mLevel.mTorchStageState = 7;
			this.mLevel.mTorchStageAlpha = 0f;
			this.mLevel.mTorchStageTimer = ZumasRevenge.Common._M(150);
			this.mContinueNextLevelOnLoadProfile = true;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00031F10 File Offset: 0x00030110
		protected void UpdateEndOfTorchLevel()
		{
			this.mLevel.Update(1f);
			if (this.mLevel.mTorchStageState == 9)
			{
				this.mFullScreenAlpha += this.mFullScreenAlphaRate;
				if (this.mFullScreenAlpha >= 255 && this.mFullScreenAlphaRate > 0)
				{
					this.mFullScreenAlpha = ZumasRevenge.Common._M(305);
					this.mFullScreenAlphaRate *= -1;
					GameState gameState = this.mGameState;
					this.ContinueToNextLevel();
					this.mLevel.mTorchStageState = 9;
					this.mGameState = gameState;
					this.mContinueNextLevelOnLoadProfile = false;
					return;
				}
				if (this.mFullScreenAlpha <= 0 && this.mFullScreenAlphaRate < 0)
				{
					this.mFullScreenAlphaRate = (this.mFullScreenAlpha = 0);
					if (this.mLevel.mFrogFlyOff == null)
					{
						this.mLevel.mTorchStageState = 10;
						this.mLevel.mFrogFlyOff = new FrogFlyOff();
						this.mLevel.mFrogFlyOff.JumpIn(this.mFrog, ZumasRevenge.Common._SS(this.mWidth / 2) - GameApp.gApp.mBoardOffsetX, this.mFrog.GetCenterY(), false, ZumasRevenge.Common._SS(this.mWidth / 2));
						return;
					}
				}
			}
			else if (this.mLevel.mTorchStageState == 13)
			{
				this.mGameState = GameState.GameState_Playing;
			}
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0003205F File Offset: 0x0003025F
		protected void UpdateFinalBossPart1Finished()
		{
			this.UpdateEndOfTorchLevel();
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00032067 File Offset: 0x00030267
		protected void UpdateBoss6Transition()
		{
			if (this.mStateCount == 255)
			{
				this.ContinueToNextLevel();
				this.mStateCount = 255;
				this.mGameState = GameState.GameState_Boss6Transition;
				return;
			}
			if (this.mStateCount == 510)
			{
				this.mGameState = GameState.GameState_Playing;
			}
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x000320A4 File Offset: 0x000302A4
		protected void UpdateBoss6FakeCredits()
		{
			this.mFakeCredits.Update();
			if (this.mFakeCredits.CanStartNextLevel())
			{
				this.ContinueToNextLevel();
				this.mLevel.mBoss.SetX((float)(ZumasRevenge.Common._SS(this.mFakeCredits.mBossX) + ZumasRevenge.Common._M(60)));
				this.mGameState = GameState.GameState_Boss6FakeCredits;
				return;
			}
			if (this.mFakeCredits.Done())
			{
				this.SetMenuBtnEnabled(true);
				this.mGameState = GameState.GameState_Playing;
				this.mFakeCredits = null;
				this.mApp.mMusic.PlaySong(127, 0.005f, true, true);
				this.mHasDoneIntroSounds = false;
			}
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00032144 File Offset: 0x00030344
		protected void UpdateBoss6DarkFrog()
		{
			if (this.mBoss6VolcanoMelt != null)
			{
				this.mVolcanoBossEssence.mDrawTransform.LoadIdentity();
				float num = GameApp.DownScaleNum(1f);
				this.mVolcanoBossEssence.mDrawTransform.Scale(num, num);
				if (this.mBoss6VolcanoMelt.mUpdateCount >= Board.gEssenceDrawFrame)
				{
					int num2 = ZumasRevenge.Common._M(20);
					int num3 = ZumasRevenge.Common._M(32);
					float num4 = 2.64f;
					float num5 = 0.28f;
					if (this.mEssenceScaleTimer < num2)
					{
						this.mEssenceXScale += (num4 - 0.74f) / (float)num2;
						this.mEssenceYScale += (num5 - 0.74f) / (float)num2;
					}
					else if (this.mEssenceScaleTimer < num3)
					{
						this.mEssenceXScale += (1f - num4) / (float)(num3 - num2);
						this.mEssenceYScale += (1f - num5) / (float)(num3 - num2);
					}
					else
					{
						this.mEssenceXScale = (this.mEssenceYScale = 1f);
					}
					this.mVolcanoBossEssence.mDrawTransform.Scale(this.mEssenceXScale, this.mEssenceYScale);
					this.mEssenceScaleTimer++;
				}
				this.mVolcanoBossEssence.mDrawTransform.Translate(this.mDarkFrogBulletX, this.mDarkFrogBulletY);
				this.mVolcanoBossEssence.Update();
				this.mBoss6VolcanoMelt.Update();
				if (this.mBoss6VolcanoMelt.Done())
				{
					if (this.mDarkFrogTimer <= ZumasRevenge.Common._M(50))
					{
						this.mDarkFrogBulletX += this.mDarkFrogBulletVX;
						this.mDarkFrogBulletY += this.mDarkFrogBulletVY;
					}
					if (--this.mDarkFrogTimer == 0)
					{
						this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BOSS_DEVIL_ESSENCE_HIT));
						this.mBoss6VolcanoMelt.Dispose();
						this.mBoss6VolcanoMelt = null;
						this.UpdateVolcanoBossEssenceShit();
						this.mDarkFrogBulletX = 0f;
						this.mDarkFrogBulletY = 0f;
						return;
					}
				}
			}
			else
			{
				this.UpdateVolcanoBossEssenceShit();
				if (this.mDarkFrogSequence.mInitialDelay < this.mDarkFrogSequence.mInitialDelayTarget || this.mDarkFrogSequence.FadingOut())
				{
					this.mLevel.mBoss.Update();
				}
				this.mDarkFrogSequence.Update();
				if (this.mDarkFrogSequence.CanStartNextLevel())
				{
					float num6 = (float)this.mFrog.GetCenterX();
					float num7 = (float)this.mFrog.GetCenterY();
					this.ContinueToNextLevel();
					this.mGameState = GameState.GameState_Boss6DarkFrog;
					this.mFrog.SetPos((int)num6, (int)num7);
					return;
				}
				if (this.mDarkFrogSequence.Done())
				{
					this.SetMenuBtnEnabled(true);
					this.mGameState = GameState.GameState_Playing;
					this.mDarkFrogSequence.Dispose();
					this.mDarkFrogSequence = null;
					this.mDrawBossUI = true;
					this.mApp.mMusic.PlaySong(127, 0.005f, true, true);
				}
			}
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00032424 File Offset: 0x00030624
		protected void UpdateBoss6StoneHeadBurst()
		{
			this.mLevel.UpdateEffects();
			if (this.mBoss6StoneBurst != null)
			{
				BossStoneHead bossStoneHead = this.mLevel.mBoss as BossStoneHead;
				bool flag = true;
				if (bossStoneHead != null)
				{
					flag = bossStoneHead.UpdateDeathSequence();
				}
				if (flag)
				{
					this.mBoss6StoneBurst.Update();
					if (this.mBoss6StoneBurst.GetUpdateCount() == ZumasRevenge.Common._M(35))
					{
						bossStoneHead.DoDeathRockExplosionThing();
					}
					if (this.mBoss6StoneBurst.Done())
					{
						this.ContinueToNextLevel();
						this.mBoss6StoneBurst.Dispose();
						this.mBoss6StoneBurst = null;
						this.mGameState = GameState.GameState_Boss6StoneHeadBurst;
						this.mSimpleFadeText.Clear();
						string[] array = new string[]
						{
							TextManager.getInstance().getString(137),
							TextManager.getInstance().getString(138)
						};
						for (int i = 0; i < Enumerable.Count<string>(array); i++)
						{
							this.mSimpleFadeText.Add(new SimpleFadeText());
							SimpleFadeText simpleFadeText = Enumerable.Last<SimpleFadeText>(this.mSimpleFadeText);
							simpleFadeText.mString = array[i];
							simpleFadeText.mAlpha = (float)((i == 0) ? 255 : 0);
							simpleFadeText.mFadeIn = true;
							this.mStateCount = 0;
							this.ShakeScreen(ZumasRevenge.Common._M(100), ZumasRevenge.Common._M1(10), ZumasRevenge.Common._M2(10));
						}
						return;
					}
				}
			}
			else
			{
				if (this.mStateCount == ZumasRevenge.Common._M(400))
				{
					Enumerable.Last<SimpleFadeText>(this.mSimpleFadeText).mAlpha = 255f;
					this.ShakeScreen(ZumasRevenge.Common._M(100), ZumasRevenge.Common._M1(10), ZumasRevenge.Common._M2(10));
					return;
				}
				if (this.mStateCount == ZumasRevenge.Common._M(800))
				{
					for (int j = 0; j < Enumerable.Count<SimpleFadeText>(this.mSimpleFadeText); j++)
					{
						this.mSimpleFadeText[j].mFadeIn = false;
					}
					return;
				}
				if (this.mStateCount > ZumasRevenge.Common._M(800))
				{
					for (int k = 0; k < this.mSimpleFadeText.size<SimpleFadeText>(); k++)
					{
						this.mSimpleFadeText[k].mAlpha -= ZumasRevenge.Common._M(2f);
						if (this.mSimpleFadeText[k].mAlpha <= 0f)
						{
							this.mGameState = GameState.GameState_Playing;
						}
					}
				}
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00032664 File Offset: 0x00030864
		protected void UpdateBossDeath()
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_DOOR);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_CONTINUE_BUTTON);
			Image imageByID3 = Res.GetImageByID(ResID.IMAGE_LARGE_FROG);
			PIEffect pieffectByID = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_TORCHFLAME);
			Board.END_BOSS_FROG_JUMP_TIME = ZumasRevenge.Common._M(50);
			this.mLevel.mBoss.Update();
			if (this.mStateCount == ZumasRevenge.Common._M(400))
			{
				this.mLevel.mBoss.mDoDeathExplosions = false;
			}
			int num = this.mLevel.mBoss.mDeathText.size<BossText>();
			if (num == 0)
			{
				return;
			}
			if (this.mLevel.mBoss.mDeathText[num - 1].mAlpha > 0f && this.mLevel.mFinalLevel && this.mAdventureWinScreen)
			{
				this.UpdateVortex(false);
				pieffectByID.mDrawTransform.LoadIdentity();
				float num2 = GameApp.DownScaleNum(1f);
				pieffectByID.mDrawTransform.Scale(num2, num2);
				pieffectByID.mDrawTransform.Translate((float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(370)), (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(300)));
				pieffectByID.Update();
				if (!this.mVortexAppear && this.mVortexBGAlpha >= 255f)
				{
					if (this.mAdventureWinAlpha < 255f)
					{
						this.mAdventureWinAlpha += ZumasRevenge.Common._M(3f);
						if (this.mAdventureWinAlpha >= 255f)
						{
							this.mAdventureWinAlpha = 255f;
						}
					}
					if (this.mAdventureWinAlpha >= 255f)
					{
						if (this.mAdventureWinTimer == (float)ZumasRevenge.Common._M(0))
						{
							this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_STONE_DRAG));
						}
						if ((this.mAdventureWinTimer -= 1f) <= 0f)
						{
							this.mAdventureWinDoorYOff -= ZumasRevenge.Common._M(2f);
							if (this.mAdventureWinDoorYOff <= (float)(-(float)imageByID.mHeight))
							{
								this.mAdventureWinDoorYOff = (float)(-(float)imageByID.mHeight);
								if (this.mAdventureWinExtraAlpha < 255f)
								{
									this.mAdventureWinExtraAlpha += ZumasRevenge.Common._M(8f);
									if (this.mAdventureWinExtraAlpha >= 255f)
									{
										this.mAdventureWinExtraAlpha = 255f;
										this.mAdvWinBtn = new ButtonWidget(5, this);
										this.mAdvWinBtn.mButtonImage = (this.mAdvWinBtn.mOverImage = (this.mAdvWinBtn.mDownImage = imageByID2));
										this.mAdvWinBtn.mNormalRect = this.mAdvWinBtn.mButtonImage.GetCelRect(0);
										this.mAdvWinBtn.mOverRect = this.mAdvWinBtn.mButtonImage.GetCelRect(1);
										this.mAdvWinBtn.mDownRect = this.mAdvWinBtn.mButtonImage.GetCelRect(2);
										this.mAdvWinBtn.mDoFinger = true;
										this.mAdvWinBtn.mPriority = (this.mAdvWinBtn.mZOrder = int.MaxValue);
										int num3 = 15;
										if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CHT)
										{
											num3 = 25;
										}
										this.mAdvWinBtn.Resize(ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_CONTINUE_BUTTON) + num3), ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_CONTINUE_BUTTON)), this.mAdvWinBtn.mButtonImage.GetCelWidth(), this.mAdvWinBtn.mButtonImage.GetCelHeight());
										this.AddWidget(this.mAdvWinBtn);
									}
								}
								else if (this.mAdventureWinTimer <= (float)ZumasRevenge.Common._M(0))
								{
									int num4 = this.mBeatGameLives * this.mApp.GetLevelMgr().mBeatGamePointsForLife;
									int num5 = (num4 + this.mBeatGameNormalScore) / ZumasRevenge.Common._M(400);
									if (num5 == 0)
									{
										num5 = 1;
									}
									this.mBeatGameTotalScoreTally += num5;
									if (this.mBeatGameTotalScoreTally > this.mBeatGameNormalScore + num4)
									{
										this.mBeatGameTotalScoreTally = this.mBeatGameNormalScore + num4;
									}
								}
							}
						}
					}
				}
			}
			if (this.mDoingEndBossFrogEffect)
			{
				this.mEndBossFrogTimer++;
				if (this.mEndBossFrogTimer > Board.END_BOSS_FROG_JUMP_TIME)
				{
					if (this.mBossSmokePoof.mFrameNum < (float)this.mBossSmokePoof.mLastFrameNum || this.mBossSmokePoof.mCurNumParticles > 0)
					{
						this.CheckForExtraLifeFromBoss();
						this.mBossSmokePoof.mDrawTransform.LoadIdentity();
						float num6 = GameApp.DownScaleNum(1f);
						this.mBossSmokePoof.mDrawTransform.Scale(num6, num6);
						this.mBossSmokePoof.mDrawTransform.Translate((float)ZumasRevenge.Common._S(this.mLevel.mBoss.GetX() + ZumasRevenge.Common._M(0)), (float)ZumasRevenge.Common._S(this.mLevel.mBoss.GetY() + ZumasRevenge.Common._M1(0)));
						this.mBossSmokePoof.Update();
					}
					if (this.mBossSmokePoof.mFrameNum >= (float)ZumasRevenge.Common._M(79) && this.mFrogFlyOff == null)
					{
						this.mFrogFlyOff = new FrogFlyOff();
						this.mFrogFlyOff.mFrogJumpTime = ZumasRevenge.Common._M(200);
						this.mFrogFlyOff.JumpOut(this.mFrog, this.mWidth + ZumasRevenge.Common._S(80) + imageByID3.mWidth, this.mHeight);
					}
				}
				else
				{
					float num7 = (float)(this.mLevel.mBoss.GetX() - Board.FROG_DEATH_X + ZumasRevenge.Common._M(0)) / (float)Board.END_BOSS_FROG_JUMP_TIME;
					float num8 = (float)(this.mLevel.mBoss.GetY() - Board.FROG_DEATH_Y + ZumasRevenge.Common._M(20)) / (float)Board.END_BOSS_FROG_JUMP_TIME;
					this.mFrog.ForceX(this.mFrog.GetCenterX() + (int)num7);
					this.mFrog.ForceY(this.mFrog.GetCenterY() + (int)num8);
				}
				this.UpdateBossOutTransition();
			}
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00032C3C File Offset: 0x00030E3C
		protected void UpdateBossOutTransition()
		{
			if (this.mFrogFlyOff == null)
			{
				return;
			}
			this.mFrogFlyOff.Update();
			if (this.mFrogFlyOff.mTimer <= 40)
			{
				return;
			}
			this.mEndBossFadeAmt += 5f;
			if (this.mEndBossFadeAmt < 265f)
			{
				return;
			}
			if (this.mLivesInfo != null)
			{
				this.mLivesInfo.Dispose();
				this.mLivesInfo = null;
			}
			this.mEndBossFadeAmt = 255f;
			this.mDoingEndBossFrogEffect = false;
			this.ContinueToNextLevel();
			this.mFrogFlyOff.Dispose();
			this.mFrogFlyOff = null;
			if (this.mLevel.mZone <= 6)
			{
				this.SetupMapScreen(true);
				return;
			}
			this.mEndBossFadeAmt = 0f;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00032CF4 File Offset: 0x00030EF4
		protected bool DisplayingEndOfLevelStats()
		{
			int num = ((this.mLevelTransition == null) ? (-1) : this.mLevelTransition.GetState());
			return this.mLevelTransition != null && ((this.mLevelTransition.mTransitionToStats && num >= 1) || (!this.mLevelTransition.mTransitionToStats && num <= 1));
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00032D4C File Offset: 0x00030F4C
		protected void DrawGauntletWidget(Graphics g)
		{
			if (this.mLevel.mMaxMultiplierTime <= 0)
			{
				return;
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGE_GAUGE_EMPTY);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGE_GAUGE_FILL);
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_STROKE);
			if (this.GauntletMode() && (this.mLevel.mCurMultiplierTimeLeft > 0 || this.mGauntletMultBarAlpha > 0f || this.mStateCount < this.mGauntletMultTextMoveLastFrame))
			{
				float num = (float)this.mLevel.mCurMultiplierTimeLeft / (float)this.mLevel.mMaxMultiplierTime;
				int num2 = ZumasRevenge.Common._S(this.mFrog.GetCurX()) - imageByID.mWidth / 2 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(0));
				int num3 = ZumasRevenge.Common._S(this.mFrog.GetCurY()) - imageByID.mHeight / 2 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(-108));
				if (this.mGauntletMultBarAlpha > 0f || this.mLevel.mCurMultiplierTimeLeft > 0)
				{
					if ((int)this.mGauntletMultBarAlpha != 255)
					{
						g.SetColorizeImages(true);
					}
					g.SetColor(255, 255, 255, (int)Math.Min(this.mGauntletMultBarAlpha, 255f));
					g.DrawImage(imageByID, num2, num3);
					int num4 = ZumasRevenge.Common._M(150);
					if (this.mLevel.mCurMultiplierTimeLeft <= num4 && this.mGameState != GameState.GameState_Losing && this.mEndGauntletTimer <= 0 && this.mLevel.mCurMultiplierTimeLeft > 0 && this.mLevel.mCurMultiplierTimeLeft % ZumasRevenge.Common._M(12) <= ZumasRevenge.Common._M1(6))
					{
						g.SetDrawMode(1);
						g.DrawImage(imageByID, num2, num3);
						g.SetDrawMode(0);
					}
					if (num > 0f)
					{
						Rect theSrcRect = new Rect(0, 0, (int)((float)imageByID2.mWidth * num), imageByID2.mHeight);
						g.DrawImage(imageByID2, num2, num3, theSrcRect);
					}
					g.SetFont(fontByID);
					g.SetColor(ZumasRevenge.Common._M(255), ZumasRevenge.Common._M1(255), ZumasRevenge.Common._M2(255), (int)Math.Max(0f, Math.Min(this.mGauntletMultBarAlpha, 255f)));
					g.DrawString(this.mScoreMultiplier + "x", num2 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(110)), num3 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(20)));
					g.SetColorizeImages(false);
				}
			}
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00032FB0 File Offset: 0x000311B0
		protected int GetAceTimeBonus()
		{
			if (this.mLevelStats.mTimePlayed > this.mLevel.mParTime + 99)
			{
				return 0;
			}
			int num = (int)(1f - (float)this.mLevelStats.mTimePlayed / (float)this.mLevel.mParTime) * ZumasRevenge.Common._M(25000);
			num = num / 100 * 100;
			num += 100;
			if (num < 100)
			{
				num = 100;
			}
			return num;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0003301C File Offset: 0x0003121C
		protected void SetupMapScreen(bool completed, bool from_load)
		{
			this.mShowMapScreen = true;
			this.SetMenuBtnEnabled(false);
			if (from_load)
			{
				this.mEndBossFadeAmt = 0f;
			}
			if (!GameApp.gApp.mResourceManager.IsGroupLoaded("Map") && !GameApp.gApp.mResourceManager.LoadResources("Map"))
			{
				GameApp.gApp.ShowResourceError(true);
				GameApp.gApp.Shutdown();
			}
			this.mMapScreen.Init(completed, this.mLevel.mZone, this.mLevel.mNum, this.mWasShowingCheckpoint, from_load);
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x000330AF File Offset: 0x000312AF
		protected void SetupMapScreen(bool completed)
		{
			this.SetupMapScreen(completed, false);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x000330BC File Offset: 0x000312BC
		protected void CheckIfGotExtraLife(int theInc)
		{
			if (this.GauntletMode() || this.IronFrogMode())
			{
				return;
			}
			while (theInc > 0)
			{
				if (this.mPointsLeftForExtraLife - theInc <= 0)
				{
					int num = this.mPointsLeftForExtraLife;
					this.mPointsLeftForExtraLife = this.mApp.GetLevelMgr().mPointsForLife;
					theInc -= num;
					this.LivesChanged(1);
				}
				else
				{
					this.mPointsLeftForExtraLife -= theInc;
					theInc = 0;
				}
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00033128 File Offset: 0x00031328
		protected void DoReload()
		{
			for (int i = 0; i < this.mZumaTips.Count; i++)
			{
				this.mZumaTips[i] = null;
			}
			this.mZumaTips.Clear();
			GameApp.gDDS.Reset();
			string mId = this.mLevel.mId;
			this.mLevel.NukeEffects();
			this.mApp.ResetAllLevelMgrs();
			this.mScore = (this.mLevelBeginScore = 0);
			this.mRollerScore.Reset(this.mGauntletMode);
			if (!this.mApp.ReloadAllLevelMgrs())
			{
				this.mFrog.LevelReset();
			}
			else
			{
				this.Reset(false, true, true, true);
				Board.gCheatReload = true;
				this.StartLevel(mId);
				Board.gCheatReload = false;
				this.UpdateGunPos(true);
				this.DoAccuracy(false);
			}
			this.MakeUIWidgets();
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x000331FC File Offset: 0x000313FC
		protected void MakeUIWidgets()
		{
			if (this.mMenuButton != null)
			{
				this.RemoveWidget(this.mMenuButton);
				this.mMenuButton.Dispose();
				this.mMenuButton = null;
			}
			this.mMenuButton = new ButtonWidget(1, this);
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_CHALLENGE_UI_PAUSE);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_CHALLENGE_UI_PAUSEDOWN);
			this.mMenuButton.mButtonImage = imageByID;
			this.mMenuButton.mDownImage = imageByID2;
			this.mMenuButton.Resize(this.mApp.GetWideScreenAdjusted(this.mMenuButtonX), 0, imageByID.GetWidth(), imageByID.GetHeight());
			this.mMenuButton.mDoFinger = true;
			this.AddWidget(this.mMenuButton);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x000332AC File Offset: 0x000314AC
		protected void EraseTunnels()
		{
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < Enumerable.Count<Tunnel>(this.mTunnels[i]); j++)
				{
					this.mTunnels[i][j].mImage = null;
				}
				this.mTunnels[i].Clear();
			}
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00033300 File Offset: 0x00031500
		protected void UpdateVolcanoBossEssenceShit()
		{
			if (this.mEssenceExplBottom.mFrameNum < (float)this.mEssenceExplBottom.mLastFrameNum)
			{
				this.mEssenceExplBottom.mDrawTransform.LoadIdentity();
				float num = GameApp.DownScaleNum(1f);
				this.mEssenceExplBottom.mDrawTransform.Scale(num, num);
				this.mEssenceExplBottom.mDrawTransform.Translate(ZumasRevenge.Common._S(this.mDarkFrogSequence.GetMoveXAmt()), ZumasRevenge.Common._S(this.mDarkFrogSequence.GetMoveYAmt()));
				this.mEssenceExplBottom.Update();
			}
			if (this.mEssenceExplTop.mFrameNum < (float)this.mEssenceExplTop.mLastFrameNum)
			{
				this.mEssenceExplTop.mDrawTransform.LoadIdentity();
				float num2 = GameApp.DownScaleNum(1f);
				this.mEssenceExplTop.mDrawTransform.Scale(num2, num2);
				this.mEssenceExplTop.mDrawTransform.Translate(ZumasRevenge.Common._S(this.mDarkFrogSequence.GetMoveXAmt()), ZumasRevenge.Common._S(this.mDarkFrogSequence.GetMoveYAmt()));
				this.mEssenceExplTop.Update();
			}
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0003340F File Offset: 0x0003160F
		protected bool NeedsLillyPadHint()
		{
			return this.mLevel.mNumFrogPoints > 1 && !this.mApp.mUserProfile.HasSeenHint(ZumaProfile.LILLY_PAD_HINT) && this.mGameState == GameState.GameState_Playing;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00033444 File Offset: 0x00031644
		protected void DoCheckpointEffect(bool game_over)
		{
			if (!game_over)
			{
				this.ToggleNotification(TextManager.getInstance().getString(432), Res.GetSoundByID(ResID.SOUND_MIDZONE_NOTIFY));
				if (this.mLevel.mBoss == null || this.mApp.IsHardMode() || this.mLevel.mZone > 1)
				{
					this.mPreventBallAdvancement = false;
					return;
				}
			}
			else
			{
				this.mCheckpointEffect = new Checkpoint(this.mLevel, this.GetCheckpointScore(), game_over);
				this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvScore = this.GetCheckpointScore();
				this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvLevel = this.mCheckpointEffect.mLevelNum - (this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvZone - 1) * 10;
				this.mCheckpointEffect.Resize(ZumasRevenge.Common._S(-80), 0, this.mApp.mWidth + ZumasRevenge.Common._S(this.mApp.mOffset160X), this.mApp.mHeight);
				this.AddWidget(this.mCheckpointEffect);
			}
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0003355C File Offset: 0x0003175C
		protected void DoAccuracy(bool accuracy)
		{
			this.mRecalcGuide = accuracy;
			this.mRecalcLazerGuide = accuracy;
			this.mShowGuide = accuracy;
			this.mDoGuide = accuracy;
			if (!accuracy)
			{
				if (this.mGuideBall != null)
				{
					this.mGuideBall.mHilightPulse = false;
				}
				this.mGuideBall = null;
				this.mAccuracyCount = 0;
				if (this.mLevel != null)
				{
					this.mFrog.SetFireSpeed(this.mLevel.mFireSpeed);
				}
				return;
			}
			if ((this.mFrog.LaserMode() && this.mFrog.GetLazerCount() > 0) || this.mFrog.LightningMode() || this.mFrog.CannonMode())
			{
				this.mAccuracyBackupCount = this.mAccuracyCount;
				this.mAccuracyCount = 0;
				return;
			}
			if (this.mAccuracyBackupCount > 0)
			{
				this.mAccuracyBackupCount = this.mAccuracyCount;
			}
			this.mFrog.SetFireSpeed(19f);
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00033640 File Offset: 0x00031840
		protected void DestroyAllBalls()
		{
			this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_EXPLODE));
			for (int i = 0; i < this.mLevel.mNumCurves; i++)
			{
				this.mLevel.mCurveMgr[i].DetonateBalls();
			}
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0003368C File Offset: 0x0003188C
		protected bool MakeBoss6StoneBurstComp()
		{
			this.mBoss6StoneBurst = new Composition();
			this.mBoss6StoneBurst.mLoadImageFunc = new AECommon.LoadCompImageFunc(GameApp.CompositionLoadFunc);
			this.mBoss6StoneBurst.mPostLoadImageFunc = new AECommon.PostLoadCompImageFunc(GameApp.CompositionPostLoadFunc);
			return this.mBoss6StoneBurst.LoadFromFile("pax\\BreakEasterIsland_FINAL");
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x000336E4 File Offset: 0x000318E4
		protected bool MakeBoss6VolcanoMeltComp()
		{
			this.mVolcanoBossEssence = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_BOSSESSENCE").Duplicate();
			this.mVolcanoBossEssence.mEmitAfterTimeline = true;
			this.mVolcanoBossEssence.ResetAnim();
			this.mEssenceExplTop = this.mApp.mResourceManager.GetPIEffect(this.mApp.Is3DAccelerated() ? "PIEFFECT_NONRESIZE_ESSENCEEXPLTOP" : "PIEFFECT_NONRESIZE_ESSENCEEXPLTOP2D").Duplicate();
			this.mEssenceExplTop.ResetAnim();
			this.mEssenceExplTop.GetLayer("Main").GetEmitter("Essence Area front").mMaskImage = this.mApp.mResourceManager.LoadImage("IMAGE_BOSS_DARKFROG_FRAME_4");
			this.mEssenceExplBottom = this.mApp.mResourceManager.GetPIEffect(this.mApp.Is3DAccelerated() ? "PIEFFECT_NONRESIZE_ESSENCEEXPLBOTTOM" : "PIEFFECT_NONRESIZE_ESSENCEEXPLBOTTOM2D").Duplicate();
			this.mEssenceExplBottom.ResetAnim();
			ZumasRevenge.Common.SetFXNumScale(this.mEssenceExplTop, this.mApp.Is3DAccelerated() ? 1f : ZumasRevenge.Common._M(0.3f));
			ZumasRevenge.Common.SetFXNumScale(this.mEssenceExplBottom, this.mApp.Is3DAccelerated() ? 1f : ZumasRevenge.Common._M(0.3f));
			this.mBoss6VolcanoMelt = new Composition();
			this.mBoss6VolcanoMelt.mLoadImageFunc = new AECommon.LoadCompImageFunc(GameApp.CompositionLoadFunc);
			this.mBoss6VolcanoMelt.mPostLoadImageFunc = new AECommon.PostLoadCompImageFunc(GameApp.CompositionPostLoadFunc);
			return this.mBoss6VolcanoMelt.LoadFromFile("pax\\Boss6_MELT");
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00033878 File Offset: 0x00031A78
		protected void UpdateBullets()
		{
			int i = 0;
			while (i < this.mBulletList.Count)
			{
				this.AdvanceFreeBullet(ref i);
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x000338A0 File Offset: 0x00031AA0
		protected void UpdateGuide(bool lazer)
		{
			this.mGuideT = -1f;
			float num = this.mFrog.GetAngle() - 1.570795f;
			float num2 = (float)Math.Sin((double)num);
			float num3 = (float)Math.Cos((double)num);
			float num4 = num2;
			float num5 = num3;
			float num6 = num2 * (float)ZumasRevenge.Common._M(28);
			float num7 = num3 * (float)ZumasRevenge.Common._M(28);
			int num8 = ZumasRevenge.Common._M(33);
			if (GameApp.gApp.Is3DAccelerated())
			{
				num4 *= (float)ZumasRevenge.Common._M(15);
				num5 *= (float)ZumasRevenge.Common._M(15);
			}
			SexyVector3 impliedObject = new SexyVector3((float)this.mFrog.GetCenterX() - (float)ZumasRevenge.Common._M(-1) * num2 + (float)num8 * num3, (float)this.mFrog.GetCenterY() - (float)ZumasRevenge.Common._M1(-1) * num3 - (float)num8 * num2, 0f);
			SexyVector3 sexyVector = new SexyVector3(impliedObject.x - num6, impliedObject.y - num7, 0f);
			SexyVector3 sexyVector2 = new SexyVector3(impliedObject.x + num6, impliedObject.y + num7, 0f);
			SexyVector3 sexyVector3 = new SexyVector3((float)Math.Cos((double)num), (float)(-(float)Math.Sin((double)num)), 0f);
			int num9 = ZumasRevenge.Common._M(5);
			SexyVector3 sexyVector4 = new SexyVector3(impliedObject.x + (float)num9 * num2, impliedObject.y, 0f);
			SexyVector3 sexyVector5 = new SexyVector3(impliedObject.x - (float)num9 * num2, impliedObject.y, 0f);
			SexyVector3 sexyVector6 = new SexyVector3(impliedObject.x, impliedObject.y, 0f);
			SexyVector3 p;
			SexyVector3 p2;
			if (lazer)
			{
				p = sexyVector4;
				p2 = sexyVector5;
			}
			else
			{
				p = sexyVector;
				p2 = sexyVector2;
			}
			Board.gPt1 = new SexyPoint((int)p.x, (int)p.y);
			Board.gPt2 = new SexyPoint((int)p2.x, (int)p2.y);
			Board.gCenter = new SexyPoint((int)sexyVector6.x, (int)sexyVector6.y);
			this.mLazerHitTreasure = false;
			float num10 = 10000000f;
			Ball ball = null;
			if (ZumasRevenge.Common.gSuckMode && this.mFrog.GetBullet() == null)
			{
				for (int i = 0; i < this.mLevel.mNumCurves; i++)
				{
					Ball ball2 = this.mLevel.mCurveMgr[i].CheckBallIntersection(sexyVector6, sexyVector3, ref num10, this.mFrog.LaserMode());
					if (ball2 != null && !ball2.GetIsExploding())
					{
						ball = ball2;
					}
				}
			}
			else
			{
				for (int i = 0; i < this.mLevel.mNumCurves; i++)
				{
					Ball ball3 = this.mLevel.mCurveMgr[i].CheckBallIntersection(sexyVector6, sexyVector3, ref num10, true);
					if (ball3 != null && !ball3.GetIsExploding())
					{
						ball = ball3;
					}
				}
				if (ZumasRevenge.Common._M(1) == 0 && ball == null)
				{
					for (int i = 0; i < this.mLevel.mNumCurves; i++)
					{
						Ball ball4 = this.mLevel.mCurveMgr[i].CheckBallIntersection(p, sexyVector3, ref num10, true);
						if (ball4 != null && !ball4.GetIsExploding())
						{
							ball = ball4;
						}
					}
					for (int i = 0; i < this.mLevel.mNumCurves; i++)
					{
						Ball ball5 = this.mLevel.mCurveMgr[i].CheckBallIntersection(p2, sexyVector3, ref num10, true);
						if (ball5 != null && !ball5.GetIsExploding())
						{
							ball = ball5;
						}
					}
				}
				if (this.mCurTreasure != null && !this.mTreasureWasHit && this.mFrog.LaserMode() && (this.LazerHitTreasure(p, sexyVector3, ref num10) || this.LazerHitTreasure(p2, sexyVector3, ref num10)))
				{
					this.mLazerHitTreasure = true;
				}
			}
			if (!this.mLazerHitTreasure)
			{
				if (ball == null)
				{
					num10 = 1000f / sexyVector3.Magnitude();
				}
				if (this.mGuideBall != null)
				{
					this.mGuideBall.mHilightPulse = false;
					this.mGuideBall.DoLaserAnim(false);
				}
				for (float num11 = 0f; num11 < num10; num11 += ZumasRevenge.Common._M(5f))
				{
					SexyVector3 sexyVector7 = impliedObject + sexyVector3 * num11;
					if (this.mLevel.PointIntersectsWall(sexyVector7.x, sexyVector7.y))
					{
						num10 = (this.mGuideT = num11);
						this.mGuideWallPoint = sexyVector7;
						ball = null;
						break;
					}
				}
				if (this.mFrog.LightningMode())
				{
					int num12 = ((this.mGuideBall != null) ? this.mGuideBall.GetColorType() : (-1));
					int num13 = ((ball != null) ? ball.GetColorType() : (-1));
					int num14 = ((this.mGuideBall != null) ? this.mLevel.GetOwningCurve(this.mGuideBall) : (-1));
					int num15 = ((ball != null) ? this.mLevel.GetOwningCurve(ball) : (-1));
					if (num12 != num13 || num14 != num15)
					{
						if (num12 != -1)
						{
							for (int j = 0; j < this.mLevel.mNumCurves; j++)
							{
								this.mLevel.mCurveMgr[j].ElectrifyBalls(num12, false);
							}
						}
						if (num13 != -1)
						{
							this.mLevel.mCurveMgr[num15].ElectrifyBalls(num13, true);
						}
					}
				}
				this.mGuideBall = ball;
				this.mGuideBallPoint = sexyVector6 + sexyVector3 * num10;
				if (this.mGuideBall != null)
				{
					this.mGuideBall.mHilightPulse = false;
					if (this.mFrog.LaserMode())
					{
						this.mGuideBall.DoLaserAnim(true, this.mFrog);
					}
				}
			}
			else if (this.mGuideBall != null)
			{
				this.mGuideBall.mHilightPulse = false;
				this.mGuideBall.DoLaserAnim(false);
				this.mGuideBall = null;
			}
			SexyVector3 sexyVector8 = impliedObject + sexyVector3 * (num10 + ZumasRevenge.Common._M(20f));
			SexyVector3 impliedObject2 = (lazer ? this.mLazerGuideCenter : this.mGuideCenter);
			if (!(lazer ? this.mRecalcLazerGuide : this.mRecalcGuide) && this.mShowGuide && (impliedObject2 - sexyVector8).Magnitude() < 20f)
			{
				return;
			}
			new SexyVector3(impliedObject.x + sexyVector3.x * num10, impliedObject.y + sexyVector3.y * num10, 0f);
			new SexyVector3(impliedObject.x + sexyVector3.x * num10 + num2 * (float)num9, impliedObject.y + sexyVector3.y * num10, 0f);
			impliedObject2.CopyFrom(sexyVector8);
			this.mShowGuide = true;
			if (lazer)
			{
				this.mLazerGuideCenter = sexyVector8;
				this.mRecalcLazerGuide = false;
			}
			else
			{
				this.mGuideCenter = sexyVector8;
				this.mRecalcGuide = false;
			}
			SexyPoint[] array = (lazer ? this.mLazerGuide : this.mGuide);
			array[0].mX = (int)ZumasRevenge.Common._S(p.x + num6 / 2f);
			array[0].mY = (int)ZumasRevenge.Common._S(p.y + num7 / 2f);
			array[1].mX = (int)ZumasRevenge.Common._S(p2.x - num6 / 2f);
			array[1].mY = (int)ZumasRevenge.Common._S(p2.y - num7 / 2f);
			if (this.mApp.mGuideStyle == 0)
			{
				SexyVector3 sexyVector9 = impliedObject + sexyVector3 * (num10 * 0.95f + ZumasRevenge.Common._M(20f));
				array[2].mX = (int)ZumasRevenge.Common._S(sexyVector9.x);
				array[2].mY = (int)ZumasRevenge.Common._S(sexyVector9.y);
				array[3].mX = (int)ZumasRevenge.Common._S(sexyVector9.x);
				array[3].mY = (int)ZumasRevenge.Common._S(sexyVector9.y);
				return;
			}
			if (this.mApp.mGuideStyle == 1)
			{
				bool flag = (new SexyVector3(this.mFrog.mCenterX, this.mFrog.mCenterY, 0f) - this.mGuideBallPoint).Magnitude() < this.mApp.mShotCorrectionAngleToWidthDist;
				SexyVector3 sexyVector10;
				SexyVector3 sexyVector11;
				if (flag)
				{
					float num16 = this.mApp.mShotCorrectionAngleMax * 0.01745328f;
					sexyVector10 = impliedObject + new SexyVector3((float)Math.Cos((double)(num + num16)), -(float)Math.Sin((double)(num + num16)), 0f) * (num10 + ZumasRevenge.Common._M(20f));
					sexyVector11 = impliedObject + new SexyVector3((float)Math.Cos((double)(num - num16)), -(float)Math.Sin((double)(num - num16)), 0f) * (num10 + ZumasRevenge.Common._M(20f));
				}
				else
				{
					float mShotCorrectionWidthMax = this.mApp.mShotCorrectionWidthMax;
					sexyVector10 = sexyVector8 - new SexyVector3(num2 * mShotCorrectionWidthMax, num3 * mShotCorrectionWidthMax, 0f);
					sexyVector11 = sexyVector8 + new SexyVector3(num2 * mShotCorrectionWidthMax, num3 * mShotCorrectionWidthMax, 0f);
				}
				array[2].mX = (int)ZumasRevenge.Common._S(sexyVector10.x);
				array[2].mY = (int)ZumasRevenge.Common._S(sexyVector10.y);
				array[3].mX = (int)ZumasRevenge.Common._S(sexyVector11.x);
				array[3].mY = (int)ZumasRevenge.Common._S(sexyVector11.y);
			}
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x000341B0 File Offset: 0x000323B0
		protected void UpdateTreasure()
		{
			if (this.mLevel.mNum == 1 && this.mLevel.mZone == 1 && !this.GauntletMode() && !this.IronFrogMode())
			{
				return;
			}
			if (this.mCurTreasure != null && this.mStateCount >= this.mTreasureEndFrame)
			{
				this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_FRUITDISAPPEARS));
				this.mCurTreasure = null;
				this.mCurTreasureNum = 0;
				return;
			}
			int num = (this.GauntletMode() ? this.mApp.GetLevelMgr().mGauntletTFreq : this.mLevel.mTreasureFreq);
			if (Board.gForceTreasure || (this.mCurTreasure == null && (this.mScore < this.mScoreTarget || this.GauntletMode()) && this.mStateCount - this.mTreasureEndFrame > num))
			{
				if (this.mLevel.CheckFruitActivation(-1))
				{
					this.mApp.PlaySamplePan(Res.GetSoundByID(ResID.SOUND_TIKI_APPEAR), this.mApp.GetPan(this.mCurTreasure.x), 5);
					this.mTreasureEndFrame = this.mStateCount + (int)((float)Board.TREASURE_LIFE * GameApp.gDDS.mHandheldBalance.mFruitPowerupAdditionalDuration);
					this.mTreasureStarAlpha = 255;
					this.mTreasureGlowAlpha = 0;
					this.mTreasureGlowAlphaRate = ZumasRevenge.Common._M(12);
					this.mTreasureWasHit = false;
					this.mTreasureVY = (this.mTreasureDefaultVY = ZumasRevenge.Common._M(0.25f));
					this.mMinTreasureY = (this.mMaxTreasureY = float.MaxValue);
					this.mTreasureYBob = 0f;
					this.mTreasureAccel = ZumasRevenge.Common._M(-0.01f);
					this.mFruitBounceEffect.Reset();
				}
				Board.gForceTreasure = false;
			}
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00034368 File Offset: 0x00032568
		protected void UpdateTreasureAnim()
		{
			if (this.mStateCount == this.mTreasureEndFrame - ZumasRevenge.Common._M(200))
			{
				this.mTreasureGlowAlphaRate *= ZumasRevenge.Common._M(4);
			}
			this.mTreasureStarAngle += ZumasRevenge.Common._M(0.01f);
			if (this.mUpdateCnt % ZumasRevenge.Common._M(3) == 0)
			{
				this.mTreasureCel = (this.mTreasureCel + 1) % (this.mFruitImg.mNumCols * this.mFruitImg.mNumRows);
			}
			if (this.mTreasureWasHit)
			{
				this.mFruitExplodeEffect.Update();
				if (this.mTreasureStarAlpha > 0)
				{
					this.mTreasureStarAlpha -= ZumasRevenge.Common._M(8);
					if (this.mTreasureStarAlpha < 0)
					{
						this.mTreasureStarAlpha = 0;
					}
				}
				this.mTreasureGlowAlpha += this.mTreasureGlowAlphaRate;
				if (this.mTreasureGlowAlphaRate > 0 && this.mTreasureGlowAlpha >= 255)
				{
					this.mTreasureGlowAlpha = 255;
					this.mTreasureGlowAlphaRate *= -1;
				}
				else if (this.mTreasureGlowAlphaRate < 0 && this.mTreasureGlowAlpha <= 0)
				{
					this.mTreasureGlowAlphaRate = (this.mTreasureGlowAlpha = 0);
				}
				if (this.mFruitExplodeEffect.mDone)
				{
					this.mCurTreasure = null;
					this.mCurTreasureNum = 0;
					return;
				}
			}
			else
			{
				this.mTreasureVY += this.mTreasureAccel;
				if (this.mTreasureAccel < 0f && this.mTreasureVY <= -this.mTreasureDefaultVY)
				{
					this.mTreasureAccel *= -1f;
					if (MathUtils._eq(this.mMinTreasureY, 3.4028235E+38f, 1f))
					{
						this.mMinTreasureY = this.mTreasureYBob;
					}
					else
					{
						this.mTreasureYBob = this.mMinTreasureY;
					}
				}
				else if (this.mTreasureAccel > 0f && this.mTreasureVY > this.mTreasureDefaultVY)
				{
					this.mTreasureAccel *= -1f;
					if (MathUtils._eq(this.mMaxTreasureY, 3.4028235E+38f, 1f))
					{
						this.mMaxTreasureY = this.mTreasureYBob;
					}
					else
					{
						this.mTreasureYBob = this.mMaxTreasureY;
					}
				}
				this.mTreasureYBob += this.mTreasureVY;
				this.mTreasureGlowAlpha += this.mTreasureGlowAlphaRate;
				if (this.mTreasureGlowAlphaRate > 0 && this.mTreasureGlowAlpha >= 255)
				{
					this.mTreasureGlowAlpha = 255;
					this.mTreasureGlowAlphaRate *= -1;
					return;
				}
				if (this.mTreasureGlowAlphaRate < 0 && this.mTreasureGlowAlpha <= 0)
				{
					this.mTreasureGlowAlphaRate *= -1;
					this.mTreasureGlowAlpha = 0;
				}
			}
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00034604 File Offset: 0x00032804
		protected void UpdateSuckMode()
		{
			if (!this.mIsEndless)
			{
				if (this.mLevel.mHaveReachedTarget && this.mDestroyCount == 0)
				{
					bool flag = false;
					for (int i = 0; i < 6; i++)
					{
						if (this.mBallColorMap[i] >= 3)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						this.mDestroyCount = 1;
					}
				}
				if (this.mDestroyCount > 0)
				{
					this.mDestroyCount++;
					if (this.mDestroyCount == 50)
					{
						this.DestroyAllBalls();
					}
				}
			}
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0003467C File Offset: 0x0003287C
		protected void UpdatePlaying()
		{
			this.ResetBallColorMap();
			if (this.mFrog != null && this.mApp.mDialogMap.Count == 0 && this.mGameState == GameState.GameState_Playing)
			{
				this.mFrog.GetBullet();
			}
			if (this.GauntletMode() && this.mNewBallDelay[0] > 0)
			{
				this.mNewBallDelay[0]--;
			}
			if (this.GauntletMode() && this.mNewBallDelay[1] > 0)
			{
				this.mNewBallDelay[1]--;
			}
			if (!this.mLevel.DoingInitialPathHilite() && this.mZumaTips.Count == 0 && !this.mPreventBallAdvancement && !this.IsPaused())
			{
				if (this.mAdventureMode)
				{
					AdvModeTempleStats advModeTempleStats = this.GetAdvModeTempleStats();
					advModeTempleStats.mCurrentTime++;
					advModeTempleStats.mTotalTimePlayed++;
				}
				else if (this.IronFrogMode())
				{
					this.mApp.mUserProfile.mIronFrogStats.mCurTime++;
					this.mApp.mUserProfile.mIronFrogStats.mTotalTimePlayed++;
				}
				else if (this.GauntletMode())
				{
					this.mApp.mUserProfile.mChallengeStats.mTotalTime++;
				}
			}
			if (this.mGauntletMultTextFlashTimer > 0)
			{
				this.mGauntletMultTextFlashTimer--;
				if (this.mGauntletMultTextFlashTimer % ZumasRevenge.Common._M(5) == 0)
				{
					this.mGauntletMultTextFlashOn = !this.mGauntletMultTextFlashOn;
				}
			}
			if (this.mLevel.mCurMultiplierTimeLeft <= 0 && this.mGauntletMultBarAlpha > 0f)
			{
				this.mGauntletMultBarAlpha -= ZumasRevenge.Common._M(5f);
			}
			float barPercent = this.mLevel.GetBarPercent();
			if (this.mCurrentSatPct < barPercent)
			{
				this.mCurrentSatPct += ZumasRevenge.Common._M(0.01f);
				if (this.mCurrentSatPct > barPercent)
				{
					this.mCurrentSatPct = barPercent;
				}
			}
			if (this.mHallucinateTimer > 0)
			{
				this.mHallucinateTimer--;
			}
			if (this.mLevel.mNumFrogPoints > 1 && this.mIntroPadHopCount < ZumasRevenge.Common._M(6) && !this.mFrog.IsHopping() && !this.NeedsLillyPadHint() && this.mLevel.DoingInitialPathHilite() && ++this.mLastIntroPadDelay >= ZumasRevenge.Common._M1(25))
			{
				this.mIntroPadHopCount++;
				this.mLastIntroPadDelay = 0;
				this.mLastIntroPad = (this.mLastIntroPad + 1) % this.mLevel.mNumFrogPoints;
				this.mFrog.SetDestPos(this.mLevel.mFrogX[this.mLastIntroPad], this.mLevel.mFrogY[this.mLastIntroPad], this.mLevel.mMoveSpeed, true);
				this.mLevel.mCurFrogPoint = this.mLastIntroPad;
			}
			this.PlayUnderwaterSound();
			Bullet firedBullet;
			while ((firedBullet = this.mFrog.GetFiredBullet()) != null)
			{
				this.AddFiredBullet(firedBullet);
				this.mLevel.BulletFired(firedBullet);
			}
			float num = ZumasRevenge.Common._M(0.05f);
			int num2 = ZumasRevenge.Common._M(2);
			float num3 = ZumasRevenge.Common._M(2f);
			if (this.mCursorBlooms[0].mScale == 0f && this.mCursorBlooms[1].mScale == 0f)
			{
				this.mCursorBlooms[0].mScale += num;
				this.mCursorBlooms[0].mX = this.mApp.mWidgetManager.mLastMouseX;
				this.mCursorBlooms[0].mY = this.mApp.mWidgetManager.mLastMouseY;
			}
			else if (this.mCursorBlooms[0].mScale > this.mCursorBlooms[1].mScale)
			{
				this.mCursorBlooms[0].mScale += num;
				if (this.mCursorBlooms[0].mScale > num3)
				{
					this.mCursorBlooms[0].mAlpha -= num2;
					if (this.mCursorBlooms[1].mScale == 0f)
					{
						this.mCursorBlooms[1].mX = this.mApp.mWidgetManager.mLastMouseX;
						this.mCursorBlooms[1].mY = this.mApp.mWidgetManager.mLastMouseY;
					}
					this.mCursorBlooms[1].mScale += num;
					if (this.mCursorBlooms[1].mScale > num3)
					{
						this.mCursorBlooms[1].mAlpha -= num2;
					}
				}
				if (this.mCursorBlooms[0].mAlpha <= 0)
				{
					this.mCursorBlooms[0].Reset();
				}
			}
			else if (this.mCursorBlooms[1].mScale > this.mCursorBlooms[0].mScale)
			{
				this.mCursorBlooms[1].mScale += num;
				if (this.mCursorBlooms[1].mScale > num3)
				{
					this.mCursorBlooms[1].mAlpha -= num2;
					if (this.mCursorBlooms[0].mScale == 0f)
					{
						this.mCursorBlooms[0].mX = this.mApp.mWidgetManager.mLastMouseX;
						this.mCursorBlooms[0].mY = this.mApp.mWidgetManager.mLastMouseY;
					}
					this.mCursorBlooms[0].mScale += num;
					if (this.mCursorBlooms[0].mScale > num3)
					{
						this.mCursorBlooms[0].mAlpha -= num2;
					}
				}
				if (this.mCursorBlooms[1].mAlpha <= 0)
				{
					this.mCursorBlooms[1].Reset();
				}
			}
			this.UpdatePlayingFX();
			this.UpdateBullets();
			this.UpdateTreasure();
			if (this.mLevelBeginning)
			{
				bool flag = false;
				for (int i = 0; i < this.mLevel.mNumCurves; i++)
				{
					if (!this.mLevel.mCurveMgr[i].HasReachedCruisingSpeed())
					{
						flag = true;
					}
				}
				if (!flag || this.mStateCount > 500)
				{
					this.mLevelBeginning = false;
				}
			}
			bool flag2 = false;
			if (this.mDbgHurry)
			{
				for (int j = 0; j < 10; j++)
				{
					for (int i = 0; i < this.mLevel.mNumCurves; i++)
					{
						this.mLevel.mCurveMgr[i].UpdatePlaying();
						if (this.mDbgHurry && this.mLevel.mCurveMgr[i].IsInDanger())
						{
							flag2 = true;
						}
					}
				}
			}
			else if (Board.gUpdateBalls)
			{
				this.mLevel.UpdatePlaying();
			}
			this.mLevel.Update(1f);
			if (!this.mApp.mUserProfile.HasSeenHint(ZumaProfile.FIRST_SHOT_HINT) && !this.GauntletMode() && this.mLevel.mZone == 1 && this.mLevel.mNum == 1 && this.mZumaTips.Count == 0 && this.mLevel.HasReachedCruisingSpeed())
			{
				this.mPreventBallAdvancement = true;
				Rect cutout_region = new Rect(ZumasRevenge.Common._DS(-22), ZumasRevenge.Common._DS(350), 0, 0);
				ZumaTip zumaTip = new ZumaTip(TextManager.getInstance().getString(825), ZumasRevenge.Common._S(ZumasRevenge.Common._M(150)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(100)), cutout_region, ZumaProfile.FIRST_SHOT_HINT);
				zumaTip.mBlockUpdates = false;
				zumaTip.mClickDismiss = false;
				zumaTip.mDoArrowAnim = true;
				zumaTip.PointAt(ZumasRevenge.Common._S(ZumasRevenge.Common._M(43)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(268)), 3);
				this.mZumaTips.Add(zumaTip);
				cutout_region = new Rect(ZumasRevenge.Common._S(190), 0, 0, 0);
				zumaTip = new ZumaTip(TextManager.getInstance().getString(826), ZumasRevenge.Common._S(250), ZumasRevenge.Common._S(175), cutout_region, ZumaProfile.ZUMA_BAR_HINT);
				zumaTip.mBlockUpdates = false;
				zumaTip.AutoPointAtCutoutRegion();
				this.mZumaTips.Add(zumaTip);
			}
			if (flag2)
			{
				this.mDbgHurry = false;
			}
			this.CheckEndConditions();
			this.CheckReload();
			if (ZumasRevenge.Common.gSuckMode)
			{
				this.UpdateSuckMode();
			}
			this.mLevel.UpdateUI();
			if (this.GauntletMode() && this.mApp.GetLevelMgr().mGauntletSessionLength - this.mLevel.mGauntletCurTime == 1100)
			{
				this.AddText(TextManager.getInstance().getString(96), ZumasRevenge.Common._SS(this.mWidth) / 2, ZumasRevenge.Common._SS(this.mHeight) / 2, ZumasRevenge.Common._M(2f), -1, Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_STROKE));
			}
			if (this.mRollingInDangerZone)
			{
				this.mLevelStats.mDangerTimePlayed++;
			}
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00034F68 File Offset: 0x00033168
		protected void UpdateLosing()
		{
			if (this.mWasShowingCheckpoint)
			{
				return;
			}
			this.UpdatePlayingFX();
			if (this.mLevel.mBoss != null)
			{
				this.mLevel.mBoss.Update();
			}
			this.mLevel.mHoleMgr.Update();
			Board.gMultTimeLeftDecAmt++;
			this.mLevel.mCurMultiplierTimeLeft -= Board.gMultTimeLeftDecAmt;
			if (this.GauntletMode() && this.mGauntletMultBarAlpha > 0f)
			{
				this.mGauntletMultBarAlpha -= ZumasRevenge.Common._M(5f);
			}
			this.mGauntletMultTextFlashTimer--;
			bool flag = true;
			for (int i = 0; i < this.mLevel.mNumCurves; i++)
			{
				this.mLevel.mCurveMgr[i].UpdateLosing();
				if (!this.mLevel.mCurveMgr[i].CanRestart())
				{
					flag = false;
				}
			}
			float angle = this.mFrog.GetAngle();
			if (this.mDeathSkull != null && flag)
			{
				this.mDeathSkull.Update();
				if (this.mLevel.mBoss != null)
				{
					this.mDeathSkull.mFrogTX = this.mLevel.mFrogX[this.mLevel.mCurFrogPoint] + this.mLevel.mBarWidth / 2;
					this.mDeathSkull.mFrogTY = this.mLevel.mFrogY[this.mLevel.mCurFrogPoint] + ZumasRevenge.Common._M(6);
				}
				if (this.mDeathSkull.mDone)
				{
					if (this.mLevel.mBoss != null)
					{
						this.mFrog.ForceX(this.mLevel.mFrogX[this.mLevel.mCurFrogPoint] + this.mLevel.mBarWidth / 2);
						this.mFrog.ForceY(this.mLevel.mFrogY[this.mLevel.mCurFrogPoint] + ZumasRevenge.Common._M(6));
					}
					angle = this.mDeathSkull.mLastFrogAngle;
					this.mDeathSkull.Dispose();
					this.mDeathSkull = null;
					Board.delay_after_skull = ZumasRevenge.Common._M(20);
				}
			}
			if (flag && this.mStateCount > 150 && this.mDeathSkull == null)
			{
				this.mLevel.PlayerLostLevel();
				this.mHasDoneIntroSounds = false;
				if (!this.GauntletMode())
				{
					if (this.mLevel.mBoss != null)
					{
						Board.gTuneNum = 6;
					}
					this.PlayLevelMusic(0.008f);
				}
				if (!this.mGauntletMode && !this.IronFrogMode())
				{
					if (this.mLevel.mBoss == null && !this.mLevel.IsFinalBossLevel())
					{
						if (!this.IsCheckpointLevel() || this.mLives > 3)
						{
							this.LivesChanged(-1);
						}
						else
						{
							this.mPreCheckpointLives = (this.mLives = 3);
						}
						this.mApp.mUserProfile.GetAdvModeVars().mDDSTier++;
						int num = (this.mLevel.mZone - 1) * 10 + this.mLevel.mNum - 1;
						this.GetAdvModeTempleStats().mLevelDeaths[num]++;
					}
					else if (this.mLevel.mBoss != null && !this.mLevel.IsFinalBossLevel())
					{
						this.GetAdvModeTempleStats().mBossDeaths[this.mLevel.mZone - 1]++;
					}
					if (this.mLevel.mBoss == null && !this.mLevel.IsFinalBossLevel() && this.mLives <= 0)
					{
						this.DoCheckpointEffect(true);
						return;
					}
					this.RestartLevel(false, this.mLevel);
					this.mFrog.SetAngle(angle);
					this.mApp.ClearUpdateBacklog(true);
					for (int j = 0; j < this.mLevel.mNumCurves; j++)
					{
						this.mLevel.mCurveMgr[j].mInkSpots.Clear();
						this.mLevel.mHoleMgr.GetHole(j).mDoDeathFade = false;
						for (int k = 0; k < 3; k++)
						{
							this.mLevel.mHoleMgr.GetHole(j).mRing[k].mAlpha = 0f;
						}
					}
					if (this.mLevel.mBoss != null)
					{
						this.mFrog.ForceX(this.mLevel.mFrogX[this.mLevel.mCurFrogPoint] + this.mLevel.mBarWidth / 2);
						this.mFrog.ForceY(this.mLevel.mFrogY[this.mLevel.mCurFrogPoint]);
						return;
					}
				}
				else
				{
					if (this.mGauntletMode && --Board.delay_after_skull <= 0)
					{
						this.EndGauntletMode(false);
						return;
					}
					if (this.IronFrogMode())
					{
						this.mApp.DoGenericDialog("", "", false, null, 0);
						ZumaDialog zumaDialog = (ZumaDialog)this.mApp.GetDialog(0);
						ZumaDialogLine zumaDialogLine = new ZumaDialogLine();
						zumaDialogLine.mFont = Res.GetFontByID(ResID.FONT_SHAGEXOTICA38_BLACK_GLOW);
						zumaDialogLine.mColor = new SexyColor(240, 117, 0);
						zumaDialogLine.mYPadding = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(0));
						if (this.mLevel.mNum == 1)
						{
							zumaDialogLine.mLine = TextManager.getInstance().getString(130);
						}
						else
						{
							StringBuilder stringBuilder = new StringBuilder(TextManager.getInstance().getString(131));
							stringBuilder.Replace("$1", (this.mLevel.mNum - 1).ToString());
							zumaDialogLine.mLine = stringBuilder.ToString();
						}
						zumaDialog.mCustomLines.Add(zumaDialogLine);
						zumaDialogLine = new ZumaDialogLine();
						zumaDialogLine.mFont = Res.GetFontByID(ResID.FONT_SHAGEXOTICA38_BLACK_GLOW);
						zumaDialogLine.mColor = new SexyColor(3, 239, 66);
						zumaDialogLine.mYPadding = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(0));
						StringBuilder stringBuilder2 = new StringBuilder(TextManager.getInstance().getString(132));
						stringBuilder2.Replace("$1", SexyFramework.Common.CommaSeperate(this.mRollerScore.GetTargetScore()));
						zumaDialogLine.mLine = stringBuilder2.ToString();
						zumaDialog.mCustomLines.Add(zumaDialogLine);
						zumaDialogLine = new ZumaDialogLine();
						zumaDialogLine.mFont = Res.GetFontByID(ResID.FONT_SHAGEXOTICA38_BLACK_GLOW);
						zumaDialogLine.mColor = new SexyColor(240, 117, 0);
						zumaDialogLine.mYPadding = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(0));
						int num2 = this.mPrevIFBestScore;
						if (this.mRollerScore.GetTargetScore() > num2)
						{
							zumaDialogLine.mLine = TextManager.getInstance().getString(133);
						}
						else if (num2 == 0)
						{
							zumaDialogLine.mLine = TextManager.getInstance().getString(134);
						}
						else
						{
							StringBuilder stringBuilder3 = new StringBuilder(TextManager.getInstance().getString(135));
							stringBuilder3.Replace("$1", SexyFramework.Common.CommaSeperate(num2));
							zumaDialogLine.mLine = stringBuilder3.ToString();
						}
						zumaDialog.mCustomLines.Add(zumaDialogLine);
						int num3 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(700));
						int num4 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(760));
						zumaDialog.Resize((this.mWidth - num3) / 2, (this.mHeight - num4) / 2, num3, num4);
						zumaDialog.WaitForResult();
						this.mApp.DoDeferredEndGame();
					}
				}
			}
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x000356C8 File Offset: 0x000338C8
		protected void UpdateMiscStuff()
		{
			this.mRollerScore.Update();
			if (this.mAccuracyCount > 0)
			{
				if (this.mGameState == GameState.GameState_Losing)
				{
					this.mAccuracyCount -= 3;
				}
				else if (this.mAccuracyBackupCount == 0)
				{
					this.mAccuracyCount--;
				}
				else
				{
					this.mAccuracyCount -= 2;
				}
				if (this.mAccuracyCount <= 0)
				{
					this.DoAccuracy(false);
				}
			}
			if (this.mFlashAlpha > 0 && (this.mFlashAlpha -= ZumasRevenge.Common._M(4)) < 0)
			{
				this.mFlashAlpha = 0;
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00035764 File Offset: 0x00033964
		protected void UpdateBossIntro()
		{
			if (Board.gNeedBossIntroSound && this.mBossIntroAlpha >= (float)ZumasRevenge.Common._M(20))
			{
				Board.gNeedBossIntroSound = false;
				this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_BOSS_BATTLE_INTRO));
			}
			if (this.mDoingBossIntroText)
			{
				if (--this.mBossIntroDelay <= 0)
				{
					if (this.mBossIntroFramesLeft > 0)
					{
						this.mBossIntroAlpha += this.mBossIntroAlphaRate;
						this.mBossTextY += this.mBossTextVY;
						this.mBattleTextY += this.mBattleTextVY;
					}
					this.mBossIntroFramesLeft--;
					if (this.mBossIntroDirection == 1 && this.mBossIntroFramesLeft <= 0)
					{
						for (int i = 0; i < this.mSmokeParticles.size<LTSmokeParticle>(); i++)
						{
							if (BambooTransition.UpdateSmokeParticle(this.mSmokeParticles[i]))
							{
								this.mSmokeParticles[i] = null;
								this.mSmokeParticles.RemoveAt(i);
								i--;
							}
						}
						this.mBossIntroAlpha = 255f;
						if (this.mSmokeParticles.size<LTSmokeParticle>() == 0)
						{
							ulong num = (ulong)SexyFramework.Common.SexyTime();
							this.ContinueToNextLevel();
							if (this.mLevel.mBossIntroBG.GetImage() != null)
							{
								this.InitBossIntroState();
							}
							ulong num2 = (ulong)SexyFramework.Common.SexyTime();
							this.mApp.ClearUpdateBacklog(true);
							int num3 = ZumasRevenge.Common._M(150);
							if (num2 - num < (ulong)((long)(10 * num3)))
							{
								this.mBossIntroDelay = num3 - (int)(num2 - num) / 10;
							}
							this.mDoingBossIntroText = true;
							this.mBossIntroDirection = -1;
							this.mBossTextVY *= -1f;
							this.mBattleTextVY *= -1f;
							this.mBossIntroAlphaRate *= -1f;
							this.mBossIntroFramesLeft = ZumasRevenge.Common._M(20);
							this.mContinueNextLevelOnLoadProfile = false;
						}
					}
					else if (this.mBossIntroDirection == -1 && this.mBossIntroFramesLeft == 0)
					{
						this.mDoingBossIntroText = false;
						this.mGameState = GameState.GameState_Playing;
						this.SetMenuBtnEnabled(true);
						if (this.mLevelTransition != null)
						{
							this.mLevelTransition.Dispose();
						}
						this.mLevelTransition = null;
					}
				}
			}
			else if (this.mDoingBossIntroFightText)
			{
				this.mFightImage.Update();
				this.mBossIntroAlpha += this.mBossIntroAlphaRate;
				if (this.mFightImage.mForward)
				{
					if (this.mFightImage.mDelay <= 0)
					{
						this.mFightImage.Reverse();
						this.mBossIntroAlphaRate *= -1f;
						this.mBossIntroAlpha = 255f;
					}
				}
				else if (this.mFightImage.mSize <= 0f)
				{
					this.mDoingBossIntroFightText = false;
					this.mGameState = GameState.GameState_Playing;
					this.SetMenuBtnEnabled(true);
				}
			}
			this.mLevel.UpdateBossIntro();
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00035A34 File Offset: 0x00033C34
		protected void UpdateBeatLevelBonus()
		{
			this.UpdatePlayingFX();
			this.AnimateBonus100s();
			if (this.mEndLevelExplosions.size<EndLevelExplosion>() == 0)
			{
				if (ZumasRevenge.Common.StrEquals(this.mLevel.mId, this.mApp.GetLevelMgr().GetLevelId(this.mApp.GetLevelMgr().GetLastIronFrogLevel())))
				{
					this.mLevelPoints = this.mScore - this.mLevelBeginScore;
					this.GetBetaStats().BeatLevel(this.mLevelStats.mTimePlayed, this.mLevel.mParTime, this.GetAceTimeBonus(), this.GetPerfectBonus(), (float)this.mLevel.mFurthestBallDistance / 100f, this.mScore - this.mLevelBeginScore, this.mScore, -1);
					if (this.mScore > this.mApp.mUserProfile.mHighestIronFrogScore)
					{
						this.mNewIronFrogHS = true;
						this.mApp.mUserProfile.mHighestIronFrogScore = this.mScore;
						this.mApp.mUserProfile.mHighestIronFrogLevel = this.mApp.GetLevelMgr().GetLevelIndex(this.mLevel.mId) - this.mApp.GetLevelMgr().GetFirstIronFrogLevel() + 1;
					}
					this.mApp.SaveProfile();
					this.mDoingIronFrogWin = true;
					this.mIronFrogAlpha = 0f;
					this.mIronFrogBtn = new ExtraSexyButton(3234, this);
					this.mIronFrogBtn.mUsesAnimators = false;
					this.mIronFrogBtn.mNormalRect = this.mIronFrogBtn.mButtonImage.GetCelRect(0);
					this.mIronFrogBtn.mOverRect = this.mIronFrogBtn.mButtonImage.GetCelRect(1);
					this.mIronFrogBtn.mDownRect = this.mIronFrogBtn.mButtonImage.GetCelRect(2);
					this.mIronFrogBtn.mDoFinger = true;
					this.mIronFrogBtn.SetVisible(false);
					this.mIronFrogBtn.SetDisabled(true);
					this.mIronFrogBtn.mBtnNoDraw = true;
					this.mIronFrogWinDelay = ZumasRevenge.Common._M(10);
					this.AddWidget(this.mIronFrogBtn);
					this.SetMenuBtnEnabled(false);
					return;
				}
				bool theAcedLevel = false;
				if (this.GauntletMode())
				{
					if (this.mScore > this.mLevel.mChallengeAcePoints)
					{
						theAcedLevel = true;
					}
				}
				else if (this.mEndLevelStats.mTimePlayed < this.mLevel.mParTime)
				{
					theAcedLevel = true;
				}
				GameApp.gApp.ReportEndOfLevelMetrics(this, true, theAcedLevel);
				this.CueLevelTransition();
			}
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00035C98 File Offset: 0x00033E98
		protected void UpdateHole()
		{
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00035C9C File Offset: 0x00033E9C
		protected void AdvanceFreeBullet(ref int theBulletItr)
		{
			Bullet bullet = this.mBulletList[theBulletItr];
			int i = 0;
			while (i < 2)
			{
				if (bullet.GetJustFired())
				{
					bullet.SetJustFired(false);
					goto IL_37;
				}
				if (i != 0)
				{
					bullet.Update(1f);
					goto IL_37;
				}
				IL_381:
				i++;
				continue;
				IL_37:
				if (this.mCurTreasure != null && !this.mTreasureWasHit)
				{
					float num = bullet.GetX() - (float)(this.mCurTreasure.x + ZumasRevenge.Common._SS(this.mFruitImg.GetCelWidth()) / 2);
					float num2 = bullet.GetY() - ((float)(this.mCurTreasure.y + ZumasRevenge.Common._SS(this.mFruitImg.GetCelHeight()) / 2) + this.mTreasureYBob);
					float num3 = (float)(bullet.GetRadius() + (ZumasRevenge.Common._SS(this.mFruitImg.GetCelHeight()) / 2 - ZumasRevenge.Common._M(0)));
					if (num * num + num2 * num2 < num3 * num3)
					{
						this.DoHitTreasure();
						this.mLevel.BulletHit(bullet);
						if (!bullet.GetIsCannon())
						{
							bullet.Dispose();
							this.mBulletList.RemoveAt(theBulletItr);
							return;
						}
					}
				}
				if (Board.gCheckCollision)
				{
					int j = 0;
					while (j < this.mLevel.mNumCurves)
					{
						if (this.mLevel.mCurveMgr[j].CheckCollision(bullet))
						{
							this.mLevel.BulletHit(bullet);
							if (!bullet.GetIsCannon())
							{
								bullet.Dispose();
								this.mBulletList.RemoveAt(theBulletItr);
								return;
							}
							break;
						}
						else
						{
							j++;
						}
					}
					if (this.mLevel.CollidedWithWall(bullet))
					{
						this.mApp.mUserProfile.mBallsTossed++;
						this.mLevel.BulletHit(bullet);
						this.ResetInARowBonus();
						bullet.Dispose();
						this.mBulletList.RemoveAt(theBulletItr);
						return;
					}
					if (this.mLevel.mBoss != null && this.mLevel.mBoss.AllowFrogToFire() && this.mLevel.mBoss.Collides(bullet))
					{
						this.mLevel.BulletHit(bullet);
						this.ResetInARowBonus();
						bullet.Dispose();
						this.mBulletList.RemoveAt(theBulletItr);
						return;
					}
				}
				for (int j = 0; j < this.mLevel.mNumCurves; j++)
				{
					this.mLevel.mCurveMgr[j].CheckGapShot(bullet);
				}
				int num4 = 800;
				int num5 = 600;
				if (bullet.GetX() < (float)ZumasRevenge.Common._S(-80) || bullet.GetY() < 0f || bullet.GetX() - (float)bullet.GetRadius() > (float)(num4 + ZumasRevenge.Common._S(80)) || bullet.GetY() - (float)bullet.GetRadius() > (float)num5)
				{
					if (!bullet.GetIsCannon())
					{
						this.mApp.mUserProfile.mBallsTossed++;
						this.ResetInARowBonus();
					}
					if (ZumasRevenge.Common.gSuckMode && !bullet.GetIsCannon() && this.mLevel.mHaveReachedTarget && !this.mIsEndless && this.mLevel.mNumCurves > 0)
					{
						int num6 = MathUtils.SafeRand() % this.mLevel.mNumCurves;
						Ball ball = new Ball();
						ball.SetColorType(bullet.GetColorType());
						ball.SetPowerType(bullet.GetPowerType(), false);
						ball.SetSpeedy(true);
						this.mLevel.mCurveMgr[num6].AddPendingBall(ball);
					}
					this.mLevelStats.mNumMisses++;
					bullet.Dispose();
					this.mBulletList.RemoveAt(theBulletItr);
					return;
				}
				this.mBallColorMap[bullet.GetColorType()]++;
				goto IL_381;
			}
			theBulletItr++;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0003603C File Offset: 0x0003423C
		protected void PlayUnderwaterSound()
		{
			if (this.mLevel.mZone != 5 || this.mUpdateCnt - Board.gLastUnderwaterSound <= 1000 || MathUtils.SafeRand() % 2500 != 0)
			{
				return;
			}
			int id = 1658 + SexyFramework.Common.Rand() % 3;
			this.mApp.PlaySample(Res.GetSoundByID((ResID)id));
			Board.gLastUnderwaterSound = this.mUpdateCnt;
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x000360A4 File Offset: 0x000342A4
		protected void AnimateBonus100s()
		{
			bool flag = false;
			for (int i = 0; i < this.mEndLevelExplosions.Count; i++)
			{
				EndLevelExplosion endLevelExplosion = this.mEndLevelExplosions[i];
				if (endLevelExplosion.mDelay > 0)
				{
					flag = true;
					this.StartBonus(endLevelExplosion);
				}
				else if (this.EndBonus(endLevelExplosion))
				{
					this.mEndLevelExplosionPool.Free(endLevelExplosion);
					this.mEndLevelExplosions.Remove(endLevelExplosion);
					this.mEffectBatch.Remove(endLevelExplosion.mPIEffect);
					i--;
				}
			}
			if (!flag && this.mApp.mSoundPlayer.IsLooping(Res.GetSoundByID(ResID.SOUND_BONUS100LOOP)))
			{
				this.mApp.mSoundPlayer.Stop(Res.GetSoundByID(ResID.SOUND_BONUS100LOOP));
			}
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0003615C File Offset: 0x0003435C
		protected void StartBonus(EndLevelExplosion inBonus)
		{
			if (--inBonus.mDelay != 0)
			{
				return;
			}
			int num = 100;
			this.IncScore(num, false);
			this.mRollerScore.ForceScore(this.mScore + this.mCurveClearBonus);
			this.AddText(string.Format("+{0}", num), inBonus.mX, inBonus.mY);
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x000361C4 File Offset: 0x000343C4
		protected bool EndBonus(EndLevelExplosion inBonus)
		{
			inBonus.mPIEffect.Update();
			return !inBonus.mPIEffect.IsActive();
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x000361E4 File Offset: 0x000343E4
		protected void InitBossIntroState()
		{
			this.mGameState = GameState.GameState_BossIntro;
			this.mStateCount = 0;
			this.mMenuButton.SetDisabled(true);
			if (this.mSwapBallButton != null)
			{
				this.mSwapBallButton.SetDisabled(true);
			}
			if (this.mLevel.mZone == 6)
			{
				if (this.mBoss6StoneBurst != null)
				{
					this.mBoss6StoneBurst.Dispose();
					this.mBoss6StoneBurst = null;
				}
				this.MakeBoss6StoneBurstComp();
			}
			if (this.mLevel.mBoss != null && this.mLevel.mBoss.mSepiaImage == null && this.mLevel.mBoss.mSepiaImagePath.Length > 0)
			{
				this.mLevel.mBoss.mSepiaImage = this.mApp.GetImage(this.mLevel.mBoss.mSepiaImagePath, true, true, false);
			}
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x000362B4 File Offset: 0x000344B4
		protected void SetLosing(int from_curve)
		{
			GameApp.gApp.mSoundPlayer.Fade(Res.GetSoundByID(ResID.SOUND_NEW_BURNINGFROGLOOP));
			if (!this.GauntletMode())
			{
				this.mApp.PlaySong(126, ZumasRevenge.Common._M(0.006f));
			}
			if (this.IronFrogMode())
			{
				this.mApp.mUserProfile.mIronFrogStats.mLevelDeaths[this.mLevel.mNum - 1]++;
			}
			if (this.mAdventureMode && this.HasAchievedZuma())
			{
				this.mApp.mUserProfile.mDeathsAfterZuma++;
			}
			this.mPreventBallAdvancement = false;
			this.mDeathSkull = null;
			if (from_curve != -1)
			{
				this.mDeathSkull = new DeathSkull();
				HoleInfo hole = this.mLevel.mHoleMgr.GetHole(from_curve);
				this.mDeathSkull.Init(hole.mRotation + 3.1415927f, (float)hole.mX, (float)hole.mY, (float)this.mFrog.GetCenterX(), (float)this.mFrog.GetCenterY());
				if (this.mLevel.mBoss != null)
				{
					((BossShoot)this.mLevel.mBoss).DeleteAllBullets();
				}
				this.mLevel.mInvertMouseTimer = 0;
				this.mHallucinateTimer = 0;
				this.DoAccuracy(false);
				this.mFrog.ClearStun();
			}
			this.mCurTreasure = null;
			this.mCurTreasureNum = 0;
			this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_GAME_OVER));
			this.ResetInARowBonus();
			this.mLevelStats.mTimePlayed = this.mStateCount - this.mIgnoreCount;
			this.mGameStats.Add(this.mLevelStats);
			bool theAcedLevel = false;
			if (this.GauntletMode() && this.mScore > this.mLevel.mChallengeAcePoints)
			{
				theAcedLevel = true;
			}
			this.mWasPerfectLevel = false;
			GameApp.gApp.ReportEndOfLevelMetrics(this, false, theAcedLevel);
			this.mLevelStats.Reset();
			this.DeleteBullets();
			this.mShowGuide = false;
			this.mFrog.EmptyBullets();
			this.mFrog.mFrogStack.Clear();
			this.mFrog.PlayerDied();
			this.mAccuracyBackupCount = 0;
			if (this.mAccuracyCount > 300)
			{
				this.mAccuracyCount = 300;
			}
			if (!this.GauntletMode())
			{
				this.mApp.mUserProfile.GetAdvModeVars().mNumDeathsCurLevel++;
			}
			for (int i = 0; i < this.mLevel.mNumCurves; i++)
			{
				this.mLevel.mHoleMgr.GetHole(i).mDoDeathFade = true;
				this.mLevel.mCurveMgr[i].SetLosing();
			}
			if (this.mLevel.mBoss != null)
			{
				GameApp.gDDS.UserLostBossLevel(this.mLevel.mBoss.GetHP());
			}
			int challenge_multiplier = (this.GauntletMode() ? this.mScoreMultiplier : (-1));
			this.GetBetaStats().DiedOnLevel(this.mStateCount, this.mScore - this.mLevelBeginScore, this.GauntletMode() ? this.mRollerScore.GetTargetScore() : this.mScore, this.mLives - 1, -1, challenge_multiplier, (int)((this.mLevel.mBoss != null) ? this.mLevel.mBoss.GetHP() : 0f));
			this.mGameState = GameState.GameState_Losing;
			this.mStateCount = 0;
			if (this.mLevel.mCurMultiplierTimeLeft > 0 && this.GauntletMode())
			{
				this.GauntletMultiplierEnded();
				this.mGauntletMultBarAlpha = (float)ZumasRevenge.Common._M(700);
			}
			if (this.mGauntletMode)
			{
				this.mApp.SaveProfile();
				if (this.mRollerScore.GetTargetScore() > this.mApp.mUserProfile.mChallengeStats.mHighestScore)
				{
					this.mApp.mUserProfile.mChallengeStats.mHighestScore = this.mRollerScore.GetTargetScore();
					return;
				}
			}
			else if (this.IronFrogMode() && this.mScore > this.mApp.mUserProfile.mHighestIronFrogScore)
			{
				this.mNewIronFrogHS = true;
				this.mApp.mUserProfile.mHighestIronFrogScore = this.mScore;
				this.mApp.mUserProfile.mHighestIronFrogLevel = this.mApp.GetLevelMgr().GetLevelIndex(this.mLevel.mId) - this.mApp.GetLevelMgr().GetFirstIronFrogLevel() + 1;
				this.mApp.SaveProfile();
			}
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00036714 File Offset: 0x00034914
		protected void SetLosing()
		{
			this.SetLosing(-1);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00036720 File Offset: 0x00034920
		protected void SetupLevelText()
		{
			for (int i = 0; i < this.mLevelNameText.Length; i++)
			{
				if (this.mLevelNameText[i] != null)
				{
					if (this.mLevelNameText[i].mImage != null)
					{
						this.mLevelNameText[i].mImage.Dispose();
					}
					this.mLevelNameText[i].mImage = null;
				}
			}
			if (this.mLevel.mBoss == null && !this.IronFrogMode() && this.mLevel.mNum < 2147483647 && !this.GauntletMode())
			{
				FwooshImage fwooshImage = this.mLevelNameText[0];
				string theString;
				if (this.GauntletMode())
				{
					theString = "x" + this.mScoreMultiplier + "!";
				}
				else
				{
					theString = TextManager.getInstance().getString(136) + "  " + ((this.mLevel.mZone - 1) * 10 + this.mLevel.mNum);
				}
				Font fontByID = Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_STROKE);
				Font fontByID2 = Res.GetFontByID(ResID.FONT_SHAGEXOTICA100_STROKE);
				fwooshImage.mDelay = ZumasRevenge.Common._M(200);
				int num = fontByID.StringWidth(theString) + 5;
				fwooshImage.mImage = new DeviceImage();
				fwooshImage.mImage.mApp = this.mApp;
				fwooshImage.mImage.SetImageMode(true, true);
				fwooshImage.mImage.AddImageFlags(16U);
				fwooshImage.mImage.Create(num + 15, fontByID.GetHeight() + 5);
				fwooshImage.mAlpha = 255f;
				fwooshImage.mIncText = true;
				fwooshImage.mSize = 0f;
				fwooshImage.mDelay = 0;
				if (!this.GauntletMode())
				{
					fwooshImage.mX = this.mWidth / 2;
					fwooshImage.mY = this.mHeight - ZumasRevenge.Common._M(10) - fontByID2.mHeight / 2 - ZumasRevenge.Common._M1(40) - fontByID2.mHeight / 4;
				}
				else
				{
					fwooshImage.mX = ZumasRevenge.Common._M(50);
					fwooshImage.mY = ZumasRevenge.Common._M(100);
				}
				Graphics graphics = new Graphics(fwooshImage.mImage);
				graphics.Get3D().ClearColorBuffer(new SexyColor(0, 0, 0, 0));
				graphics.SetFont(fontByID);
				graphics.SetColor(SexyColor.White);
				graphics.DrawString(theString, 5, fontByID.GetAscent());
				graphics.ClearRenderContext();
				if (!this.GauntletMode())
				{
					FwooshImage fwooshImage2 = this.mLevelNameText[1];
					theString = this.mLevel.mDisplayName;
					num = fontByID2.StringWidth(theString);
					fwooshImage2.mDelay = ZumasRevenge.Common._M(200);
					fwooshImage2.mImage = new DeviceImage();
					fwooshImage.mImage.mApp = this.mApp;
					fwooshImage2.mImage.SetImageMode(true, true);
					fwooshImage2.mImage.AddImageFlags(16U);
					fwooshImage2.mImage.Create(num + 15, fontByID2.GetHeight() + 5);
					fwooshImage2.mAlpha = 255f;
					fwooshImage2.mIncText = true;
					fwooshImage2.mSize = 0f;
					fwooshImage2.mDelay = 0;
					fwooshImage2.mX = this.mWidth / 2;
					if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CHT)
					{
						fwooshImage2.mY = this.mHeight - ZumasRevenge.Common._M(15) - fontByID2.mHeight / 2;
					}
					else
					{
						fwooshImage2.mY = this.mHeight - ZumasRevenge.Common._M(25) - fontByID2.mHeight / 2;
					}
					Graphics graphics2 = new Graphics(fwooshImage2.mImage);
					graphics2.Get3D().ClearColorBuffer(new SexyColor(0, 0, 0, 0));
					graphics2.SetFont(fontByID2);
					graphics2.SetColor(SexyColor.White);
					graphics2.DrawString(theString, 5, fontByID2.GetAscent());
					graphics2.ClearRenderContext();
					return;
				}
			}
			else if (this.mLevel.mBossIntroBG != null && this.mLevel.mBossIntroBG.GetImage() != null)
			{
				Board.gNeedBossIntroSound = true;
			}
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00036AFC File Offset: 0x00034CFC
		protected void SetupTunnels(Level theLevel)
		{
			for (int i = 0; i < Enumerable.Count<TunnelData>(theLevel.mTunnelData); i++)
			{
				int mPriority = theLevel.mTunnelData[i].mPriority;
				Tunnel tunnel = new Tunnel();
				this.mTunnels[mPriority].Add(tunnel);
				TunnelData tunnelData = theLevel.mTunnelData[i];
				if (tunnelData.mImageName.Length > 0)
				{
					string theFileName = string.Concat(new string[]
					{
						this.mApp.GetResImagesDir(),
						"levels/",
						theLevel.mId,
						"/",
						theLevel.mTunnelData[i].mImageName
					});
					tunnel.mImage = this.mApp.GetImage(theFileName, true, true, false);
					tunnel.mX = theLevel.mTunnelData[i].mX;
					tunnel.mY = theLevel.mTunnelData[i].mY;
				}
				else
				{
					SharedImageRef sharedImageRef = this.mApp.mResourceManager.LoadImage(tunnelData.mLayerId);
					if (sharedImageRef != null)
					{
						tunnel.mImage = sharedImageRef.GetImage();
					}
					tunnel.mLayerId = tunnelData.mLayerId;
					string text = tunnelData.mLayerId;
					int num = text.IndexOf("BOSS6PART4");
					if (num != -1)
					{
						text = text.Substring(0, 8) + '3' + text.Substring(10);
					}
					SexyPoint imageOffset = this.mApp.mResourceManager.GetImageOffset(text);
					tunnel.mX = imageOffset.mX;
					tunnel.mY = imageOffset.mY;
				}
				tunnel.mAboveShadows = tunnelData.mAboveShadows;
			}
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00036CAD File Offset: 0x00034EAD
		protected void DrawBoss6FakeCredits(Graphics g)
		{
			if (!this.mFakeCredits.HasClosedScene())
			{
				this.mLevel.mBoss.Draw(g);
			}
			this.mFakeCredits.Draw(g);
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00036CDC File Offset: 0x00034EDC
		protected void DrawBoss6DarkFrog(Graphics g)
		{
			if (this.mBoss6VolcanoMelt != null)
			{
				if (this.mBoss6VolcanoMelt.Done() || this.mBoss6VolcanoMelt.mUpdateCount >= Board.gEssenceDrawFrame)
				{
					this.mVolcanoBossEssence.Draw(g);
				}
				if (!this.mBoss6VolcanoMelt.Done())
				{
					int theTransX = -(ZumasRevenge.Common._DS(this.mBoss6VolcanoMelt.mWidth) / 2 - ZumasRevenge.Common._S(this.mLevel.mBoss.GetX())) + ZumasRevenge.Common._S(ZumasRevenge.Common._M(5));
					int theTransY = -(ZumasRevenge.Common._DS(this.mBoss6VolcanoMelt.mHeight) / 2 - ZumasRevenge.Common._S(this.mLevel.mBoss.GetY())) + ZumasRevenge.Common._S(ZumasRevenge.Common._M1(35));
					g.PushState();
					g.Translate(theTransX, theTransY);
					this.mBoss6VolcanoMelt.Draw(g, null, -1, ZumasRevenge.Common._DS(1f));
					g.PopState();
					return;
				}
				int num = this.mDarkFrogTimer;
				if (num > 255)
				{
					num = 255;
				}
				if (num > 0)
				{
					Font fontByID = Res.GetFontByID(ResID.FONT_BOSS_TAUNT);
					if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH)
					{
						fontByID.mAscent = 25;
					}
					g.SetFont(fontByID);
					g.SetColor(0, 0, 0, num);
					string[] array = new string[3];
					if (this.IsHardAdventureMode())
					{
						array[0] = TextManager.getInstance().getString(108);
						array[1] = TextManager.getInstance().getString(109);
						array[2] = TextManager.getInstance().getString(110);
					}
					else
					{
						array[0] = TextManager.getInstance().getString(106);
						array[1] = TextManager.getInstance().getString(107);
						array[2] = "";
					}
					for (int i = 0; i < array.Length; i++)
					{
						g.WriteString(array[i], -GameApp.gApp.mBoardOffsetX, ZumasRevenge.Common._DS(ZumasRevenge.Common._M(550)) + g.GetFont().GetHeight() * i, this.mWidth);
					}
					return;
				}
			}
			else
			{
				if (this.mDarkFrogSequence.FadingOut())
				{
					this.mLevel.mBoss.Draw(g);
				}
				g.PushState();
				this.mEssenceExplBottom.Draw(g);
				g.PopState();
				this.mDarkFrogSequence.Draw(g);
				g.PushState();
				this.mEssenceExplTop.Draw(g);
				g.PopState();
			}
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00036F24 File Offset: 0x00035124
		protected void DrawBoss6StoneHeadBurst(Graphics g)
		{
			if (this.mBoss6StoneBurst != null)
			{
				BossStoneHead bossStoneHead = (BossStoneHead)this.mLevel.mBoss;
				bool flag = true;
				if (bossStoneHead != null)
				{
					flag = bossStoneHead.mStretchPct >= Board.MAX_STONE_HEAD_STRETCH;
				}
				if (flag)
				{
					int num = this.mWidth / 2 - ZumasRevenge.Common._S(ZumasRevenge.Common._M(45));
					int num2 = ZumasRevenge.Common._S(this.mApp.GetLevelMgr().GetLevelByIndex(this.mLevel.mIndex + 1).mBoss.GetY()) - ZumasRevenge.Common._S(ZumasRevenge.Common._M(5));
					int num3 = ZumasRevenge.Common._M(56);
					int maxDuration = this.mBoss6StoneBurst.GetMaxDuration();
					int num4 = -(this.mWidth / 2 - ZumasRevenge.Common._S(this.mLevel.mBoss.GetX())) + ZumasRevenge.Common._S(ZumasRevenge.Common._M(110));
					int num5 = -(this.mHeight / 2 - ZumasRevenge.Common._S(this.mLevel.mBoss.GetY())) + ZumasRevenge.Common._S(ZumasRevenge.Common._M(110));
					if (this.mBoss6StoneBurst.GetUpdateCount() >= num3)
					{
						num4 -= (int)((float)(ZumasRevenge.Common._S(this.mLevel.mBoss.GetX()) - num) * ((float)(this.mBoss6StoneBurst.GetUpdateCount() - num3) / (float)(maxDuration - num3)));
						num5 -= (int)((float)(ZumasRevenge.Common._S(this.mLevel.mBoss.GetY()) - num2) * ((float)(this.mBoss6StoneBurst.GetUpdateCount() - num3) / (float)(maxDuration - num3)));
					}
					g.PushState();
					g.Translate(num4, num5);
					this.mBoss6StoneBurst.Draw(g, null, -1, ZumasRevenge.Common._DS(1f));
					g.PopState();
				}
			}
			else
			{
				Font fontByID = Res.GetFontByID(ResID.FONT_BOSS_TAUNT);
				if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH)
				{
					fontByID.mAscent = 25;
				}
				g.SetFont(fontByID);
				for (int i = 0; i < this.mSimpleFadeText.size<SimpleFadeText>(); i++)
				{
					SimpleFadeText simpleFadeText = this.mSimpleFadeText[i];
					if (simpleFadeText.mAlpha > 0f)
					{
						g.SetColor(0, 0, 0, (int)(simpleFadeText.mAlpha * 2f / 3f));
						int num6 = ZumasRevenge.Common._S(ZumasRevenge.Common._M(250)) + i * (fontByID.mHeight + ZumasRevenge.Common._S(ZumasRevenge.Common._M1(6)));
						g.FillRect(ZumasRevenge.Common._S(-80), num6, this.mWidth + ZumasRevenge.Common._S(160), fontByID.mHeight + ZumasRevenge.Common._S(ZumasRevenge.Common._M(6)));
						g.SetColor(0, 0, 0, (int)simpleFadeText.mAlpha);
						g.DrawString(simpleFadeText.mString, (this.mWidth - fontByID.StringWidth(simpleFadeText.mString)) / 2, num6 + ZumasRevenge.Common._S(ZumasRevenge.Common._M(3)) + fontByID.GetAscent());
					}
				}
			}
			if (this.mLevel.mCanDrawBoss)
			{
				this.mLevel.mBoss.Draw(g);
			}
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00037218 File Offset: 0x00035418
		protected void DrawBullets(Graphics g)
		{
			foreach (Bullet bullet in this.mBulletList)
			{
				bullet.DrawShadow(g);
			}
			foreach (Bullet bullet2 in this.mBulletList)
			{
				bullet2.Draw(g);
			}
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00037270 File Offset: 0x00035470
		protected void DrawTreasure(Graphics g)
		{
			if (this.IsPaused())
			{
				return;
			}
			if (this.mCurTreasure != null)
			{
				Image imageByID = Res.GetImageByID(ResID.IMAGE_FRUIT_GENERIC_GLOW);
				int num = ZumasRevenge.Common._S(this.mCurTreasure.x);
				int num2 = ZumasRevenge.Common._S(this.mCurTreasure.y);
				int num3 = (imageByID.mWidth - this.mFruitImg.GetCelWidth()) / 2;
				int num4 = (imageByID.mHeight - this.mFruitImg.GetCelHeight()) / 2;
				float pct = this.mFruitBounceEffect.GetPct();
				if (this.mFruitBounceEffect.GetCount() > 0)
				{
					g.SetDrawMode(1);
					SexyColor color = (this.mTreasureWasHit ? new SexyColor(255, 0, 0) : new SexyColor(SexyColor.White));
					color.mAlpha = this.mTreasureStarAlpha;
					if (color.mAlpha != 255)
					{
						g.SetColorizeImages(true);
						g.SetColor(color);
					}
					if (g.Is3D())
					{
						g.DrawImageRotatedF(imageByID, (float)(num - num3), (float)(num2 - num4) + this.mTreasureYBob, (double)this.mTreasureStarAngle);
						g.DrawImageRotatedF(imageByID, (float)(num - num3), (float)(num2 - num4) + this.mTreasureYBob, (double)(-(double)this.mTreasureStarAngle));
					}
					else
					{
						g.DrawImageRotated(imageByID, num - num3, (int)((float)(num2 - num4) + this.mTreasureYBob), (double)this.mTreasureStarAngle);
						g.DrawImageRotated(imageByID, num - num3, (int)((float)(num2 - num4) + this.mTreasureYBob), (double)(-(double)this.mTreasureStarAngle));
					}
					g.SetColorizeImages(false);
					g.SetDrawMode(0);
				}
				Rect celRect = this.mFruitImg.GetCelRect(this.mTreasureCel % this.mFruitImg.mNumCols, this.mTreasureCel / this.mFruitImg.mNumCols);
				float num5 = pct * (float)this.mFruitImg.GetCelWidth();
				float num6 = pct * (float)this.mFruitImg.GetCelHeight();
				Rect theDestRect = new Rect((int)((float)num - num5 / 2f + (float)(this.mFruitImg.GetCelWidth() / 2)), (int)((float)num2 - num6 / 2f + this.mTreasureYBob + (float)(this.mFruitImg.GetCelHeight() / 2)), (int)num5, (int)num6);
				if (!this.mTreasureWasHit)
				{
					g.DrawImage(this.mFruitImg, theDestRect, celRect);
					if (this.mTreasureGlowAlpha != 0 && this.mFruitGlow != null)
					{
						g.SetColorizeImages(true);
						g.SetColor(255, 255, 255, this.mTreasureGlowAlpha);
						g.DrawImage(this.mFruitGlow, theDestRect, celRect);
						g.SetColorizeImages(false);
						return;
					}
				}
				else
				{
					this.mFruitExplodeEffect.Draw(g);
				}
			}
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x000374F8 File Offset: 0x000356F8
		protected void DrawPlaying(Graphics g)
		{
			if (this.mBackgroundImage != null)
			{
				if ((this.mGameState != GameState.GameState_BossDead || this.mStateCount < 255) && (this.mGameState != GameState.GameState_Boss6DarkFrog || (this.mDarkFrogSequence != null && this.mDarkFrogSequence.GetBGAlpha() < 255f)) && (this.mFakeCredits == null || !this.mFakeCredits.IsFullyOpaque()))
				{
					this.mLevel.DrawUnderBackground(g);
					if (!this.mLevel.mNoBackground && !this.mDoMuMuMode)
					{
						int num = 1024;
						int theStretchedHeight = ZumasRevenge.Common._DS(1200);
						g.DrawImage(this.mBackgroundImage, (ZumasRevenge.Common._S(800) - num) / 2 + GameApp.gScreenShakeX, GameApp.gScreenShakeY, num, theStretchedHeight);
					}
				}
			}
			else
			{
				g.SetColor(0, 128, 128);
				g.FillRect(0, 0, this.mWidth, this.mHeight);
			}
			this.mLevel.DrawBottomLevel(g);
			if (this.mLevel.mDrawCurves)
			{
				g.DrawImage(this.mCachedCurveImage, 0, 0);
			}
			this.DrawTunnels(g, 0, true);
			this.mLevel.DrawSkullPit(g);
			this.mLevel.Draw(g);
			if (this.mLevel.mNum == 5 && this.mLevel.mZone == 1)
			{
				this.DrawGauntletWidget(g);
			}
			if (Board.drawer == null)
			{
				Board.drawer = new BallDrawer();
			}
			Board.drawer.Reset();
			if ((this.mPauseCount == 0 || this.mShowBallsDuringPause || this.mZumaTips.size<ZumaTip>() > 0) && this.mChallengeHelp == null && this.mApp.mGenericHelp == null)
			{
				for (int i = 0; i < this.mLevel.mNumCurves; i++)
				{
					this.mLevel.mCurveMgr[i].DrawBalls(Board.drawer);
				}
			}
			Board.drawer.Draw(g, this);
			for (int j = 0; j < this.mLevel.mWalls.size<Wall>(); j++)
			{
				Wall wall = this.mLevel.mWalls[j];
				wall.Draw(g);
			}
			this.mLevel.DrawGunPoints(g);
			if (this.mLevel.mNum != 5 || this.mLevel.mZone != 1)
			{
				this.DrawGauntletWidget(g);
			}
			if (!this.IsPaused())
			{
				for (int k = 0; k < this.mBallExplosions.size<BallExplosion>(); k++)
				{
					this.mBallExplosions[k].Draw(g);
				}
				for (int l = 0; l < this.mLazerBlasts.size<PIEffect>(); l++)
				{
					this.mLazerBlasts[l].Draw(g);
				}
			}
			this.mLevel.DrawFullSceneNoFrog(g);
			if (this.mLevel.mBoss == null || this.mDeathSkull == null)
			{
				if (this.mDeathSkull != null)
				{
					this.mDeathSkull.DrawBelowFrog(g);
				}
				if (this.mGameState != GameState.GameState_BossDead && (this.mGameState != GameState.GameState_Losing || this.mDeathSkull != null) && this.mFrogFlyOff == null && !this.mDoingEndBossFrogEffect && (this.mCheckpointEffect == null || !this.mCheckpointEffect.mFromGameOver) && (this.mGameState != GameState.GameState_FinalBossPart1Finished || this.mLevel.mTorchStageState >= 11) && this.mLevel.CanDrawFrog())
				{
					SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
					bool flag = GameApp.gApp.Is3DAccelerated() && this.mTransitionFrogScale > 0.0 && this.mTransitionFrogScale != 1.0 && this.mTransitionFrogScale.GetInVal() < 1.0;
					if (flag)
					{
						if (this.mPlayThud && this.mTransitionFrogScale.GetInVal() > 0.800000011920929)
						{
							this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_FROG_FALL));
							this.mPlayThud = false;
						}
						sexyTransform2D.Translate(-ZumasRevenge.Common._S(this.mFrog.mCenterX), -ZumasRevenge.Common._S(this.mFrog.mCenterY));
						sexyTransform2D.Scale((float)this.mTransitionFrogScale.GetOutVal(), (float)this.mTransitionFrogScale.GetOutVal());
						sexyTransform2D.Translate(ZumasRevenge.Common._S(this.mFrog.mCenterX), ZumasRevenge.Common._S(this.mFrog.mCenterY));
						g.Get3D().PushTransform(sexyTransform2D);
					}
					this.mFrog.Draw(g, (this.mDeathSkull != null) ? this.mDeathSkull.mFrogClipHeight : 0);
					if (flag)
					{
						g.Get3D().PopTransform();
					}
				}
				if (this.mDeathSkull != null)
				{
					this.mDeathSkull.DrawAboveFrog(g);
				}
			}
			this.DrawTreasure(g);
			this.DrawBullets(g);
			this.mLevel.DrawAboveBalls(g);
			if (this.mLevel.mBoss != null && this.mLevel.mCanDrawBoss && this.mGameState != GameState.GameState_BossDead && (this.mGameState != GameState.GameState_Boss6StoneHeadBurst || this.mBoss6StoneBurst == null) && (this.mGameState != GameState.GameState_Boss6DarkFrog || (this.mBoss6VolcanoMelt == null && this.mDarkFrogSequence != null && !this.mDarkFrogSequence.FadingIn())))
			{
				this.mLevel.mBoss.Draw(g);
			}
			if (this.mLevel.mBoss != null && this.mDeathSkull != null)
			{
				if (this.mDeathSkull != null)
				{
					this.mDeathSkull.DrawBelowFrog(g);
				}
				if (this.mGameState != GameState.GameState_BossDead && this.mFrogFlyOff == null && !this.mDoingEndBossFrogEffect && (this.mCheckpointEffect == null || !this.mCheckpointEffect.mFromGameOver) && (this.mGameState != GameState.GameState_FinalBossPart1Finished || this.mLevel.mTorchStageState >= 11) && this.mLevel.CanDrawFrog())
				{
					this.mFrog.Draw(g, (this.mDeathSkull != null) ? this.mDeathSkull.mFrogClipHeight : 0);
				}
				if (this.mDeathSkull != null)
				{
					this.mDeathSkull.DrawAboveFrog(g);
				}
			}
			if (g.Is3D() && !this.IsPaused())
			{
				for (int m = 0; m < this.mPowerEffects.size<PowerEffect>(); m++)
				{
					this.mPowerEffects[m].Draw(g);
				}
			}
			if (!this.mPreventBallAdvancement && this.mCheckpointEffect == null && this.mGameState == GameState.GameState_Playing && ((this.mHasSeenCheckpointIntro && this.ShouldShowCheckpointPostcard()) || !this.ShouldShowCheckpointPostcard()))
			{
				this.SetBoardOffset(g, false);
				for (int n = 0; n < 2; n++)
				{
					if (this.mLevelNameText[n] != null && this.mLevelNameText[n].mImage != null)
					{
						this.mLevelNameText[n].Draw(g);
					}
				}
				this.SetBoardOffset(g, true);
			}
			if (!this.IsPaused() && this.mApp.mProxBombManager != null)
			{
				this.mApp.mProxBombManager.Draw(g);
				this.mApp.mProxBombManager.DrawOverlay(g);
			}
			if ((this.mCheckpointEffect == null && this.mGameState == GameState.GameState_Playing && this.mHasSeenCheckpointIntro && this.ShouldShowCheckpointPostcard() && !this.IronFrogMode() && !this.GauntletMode() && this.mPreCheckpointLives < 3) || (this.mLevel.mNum == 1 && this.mLevel.mZone > 1 && this.mPreCheckpointLives < 3 && this.mLives == 3))
			{
				int num2 = ((this.mStateCount < ZumasRevenge.Common._M(200)) ? 255 : (255 - (int)((float)(this.mStateCount - ZumasRevenge.Common._M1(200)) / ZumasRevenge.Common._M2(1f))));
				if (num2 > 0)
				{
					g.SetColor(255, 0, 0, (num2 > 255) ? 255 : num2);
					g.SetFont(Res.GetFontByID(ResID.FONT_SHAGEXOTICA100_STROKE));
					g.WriteString(TextManager.getInstance().getString(124), 0, (this.mApp.mHeight - g.GetFont().mHeight) / 2, this.mApp.mWidth, 0);
				}
			}
			this.mLevel.DrawFullScene(g);
			if (this.mLevel.mBoss != null && this.mLevel.mCanDrawBoss)
			{
				this.mLevel.mBoss.DrawWordBubble(g);
			}
			this.mLevel.DrawTorchLighting(g);
			if (!this.mLevel.mBGFromPSD)
			{
				g.SetColor(SexyColor.Black);
				g.FillRect(ZumasRevenge.Common._S(-80), 0, ZumasRevenge.Common._S(80), this.mHeight);
				g.FillRect(this.mWidth, 0, ZumasRevenge.Common._S(80), this.mHeight);
			}
			if (this.gDrawAutoAimAssistInfo && this.mApp.mShotCorrectionDebugStyle == 3)
			{
				g.SetFont(Res.GetFontByID(ResID.FONT_MAIN22));
				g.SetColor(255, 255, 255, 255);
				if (this.mApp.mBoard.mGuideBall != null)
				{
					Ball ball = this.mApp.mBoard.mGuideBall;
					SexyVector3 sexyVector = this.mApp.mBoard.mGuideBallPoint;
					SexyVector3 speed = ball.GetSpeed();
					float wayPoint = ball.GetWayPoint();
					float wayPointProgress = ball.GetWayPointProgress();
					float x = ball.GetX();
					float y = ball.GetY();
					float fireSpeed = this.mApp.mBoard.GetGun().GetFireSpeed();
					SexyVector3 sexyVector2 = new SexyVector3(ball.GetX() - (float)this.mApp.mBoard.mFrog.GetCenterX(), ball.GetY() - (float)this.mApp.mBoard.mFrog.GetCenterY(), 0f);
					float num3 = (float)Math.Sqrt((double)(sexyVector2.x * sexyVector2.x + sexyVector2.y * sexyVector2.y));
					string theLine = string.Format("WayPoint = {0}\nWayPointProgress = {0}\nX = {0}\nY = {0}\nMoveVec = {0}, {0}\nFireSpeed = {0}\nGuideLength = {0}", new object[] { wayPoint, wayPointProgress, x, y, speed.x, speed.y, fireSpeed, num3 });
					g.WriteWordWrapped(new Rect(ZumasRevenge.Common._S(20), ZumasRevenge.Common._S(100), ZumasRevenge.Common._S(500), ZumasRevenge.Common._S(500)), theLine);
				}
			}
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00037F6C File Offset: 0x0003616C
		protected void DrawBossIntro(Graphics g)
		{
			Level level = ((this.mNextLevel != null) ? this.mNextLevel : this.mLevel);
			if ((!this.mDoingBossIntroText || this.mBossIntroDirection == 1) && (!this.mDoingBossIntroFightText || this.mFightImage.mForward))
			{
				g.SetColorizeImages(true);
				g.SetColor(this.mBossIntroBGAlpha);
				g.DrawImage(level.mBossIntroBG.GetImage(), ZumasRevenge.Common._S(-80), 0, 1024, 640);
			}
			if (this.mDoingBossIntroText)
			{
				int num = (int)Math.Min(Math.Max(this.mBossIntroAlpha, 0f), 255f);
				g.SetColor(0, 0, 0, num);
				g.FillRect(this.mApp.GetScreenRect());
				g.PushState();
				int alpha = num * ZumasRevenge.Common._M(40) / 255;
				g.SetColor(255, 255, 255, alpha);
				g.SetColorizeImages(true);
				for (int i = 0; i < 13; i++)
				{
					int id = 1268 + i;
					Image imageByID = Res.GetImageByID((ResID)id);
					int num2 = ZumasRevenge.Common._DS(Res.GetOffsetXByID((ResID)id) - 160);
					int num3 = ZumasRevenge.Common._DS(Res.GetOffsetYByID((ResID)id));
					g.DrawImage(imageByID, num2, num3);
					int theX = num2 + imageByID.GetWidth();
					int theY = num3;
					g.DrawImageMirror(imageByID, theX, theY);
				}
				g.PopState();
				g.SetColorizeImages(false);
				for (int j = 0; j < this.mSmokeParticles.size<LTSmokeParticle>(); j++)
				{
					BambooTransition.DrawSmokeParticle(g, this.mSmokeParticles[j]);
				}
				return;
			}
			if (this.mDoingBossIntroFightText)
			{
				int alpha2 = (int)Math.Min(Math.Max(this.mBossIntroAlpha, 0f), 255f);
				g.SetColor(0, 0, 0, alpha2);
				g.FillRect(this.mApp.GetScreenRect());
				this.mFightImage.mX = this.mWidth / 2;
				this.mFightImage.mY = this.mHeight / 2 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(0));
				this.mFightImage.Draw(g);
			}
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0003818C File Offset: 0x0003638C
		protected void DrawVortex(Graphics g, bool draw_overlay)
		{
			g.SetColor(0, 0, 0, this.mVortexAppear ? ((int)this.mVortexBGAlpha) : 255);
			g.mTransX = 0f;
			g.FillRect(this.mApp.GetScreenRect());
			g.mTransX = (float)this.mApp.mBoardOffsetX;
			for (int i = this.mVortexFaces.size<VortexFace>() - 1; i >= 0; i--)
			{
				float mPct = this.mVortexFaces[i].mPct;
				float mAngle = this.mVortexFaces[i].mAngle;
				this.mGlobalTranform.Reset();
				this.mGlobalTranform.Scale(mPct, mPct);
				this.mGlobalTranform.RotateRad(mAngle);
				int num;
				int num2;
				int num3;
				int num4;
				if (this.mVortexFaces[i].mImage == Res.GetImageByID(ResID.IMAGE_BOSS_VORTEX_FACE1))
				{
					num = ZumasRevenge.Common._M(800);
					num2 = ZumasRevenge.Common._M(600);
					num3 = ZumasRevenge.Common._M(400);
					num4 = ZumasRevenge.Common._M(500);
				}
				else
				{
					num = ZumasRevenge.Common._M(600);
					num2 = ZumasRevenge.Common._M(650);
					num3 = ZumasRevenge.Common._M(400);
					num4 = ZumasRevenge.Common._M(650);
				}
				float num5 = (float)this.mVortexFaces[i].mUpdateCount / ZumasRevenge.Common._M(100f);
				if (num5 < 1f)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, 255 - (int)(255f * num5));
					num = (int)((1f - num5) * (float)num + num5 * (float)num3);
					num2 = (int)((1f - num5) * (float)num2 + num5 * (float)num4);
					if (g.Is3D())
					{
						g.DrawImageTransformF(this.mVortexFaces[i].mImage, this.mGlobalTranform, (float)ZumasRevenge.Common._DS(num), (float)ZumasRevenge.Common._DS(num2));
					}
					else
					{
						g.DrawImageTransform(this.mVortexFaces[i].mImage, this.mGlobalTranform, (float)ZumasRevenge.Common._DS(num), (float)ZumasRevenge.Common._DS(num2));
					}
					g.SetColorizeImages(false);
				}
			}
			int centerX = this.mLevel.mFrog.GetCenterX();
			int centerY = this.mLevel.mFrog.GetCenterY();
			Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_VORTEX_STREAK);
			for (int j = 0; j < this.mVortexBeams.size<VortexBeam>(); j++)
			{
				VortexBeam vortexBeam = this.mVortexBeams[j];
				this.mGlobalTranform.Reset();
				this.mGlobalTranform.Scale(vortexBeam.mPct, vortexBeam.mPct);
				this.mGlobalTranform.RotateRad(vortexBeam.mAngle - 1.5707964f);
				g.SetColorizeImages(true);
				SexyColor mColor = vortexBeam.mColor;
				mColor.mAlpha = (this.mVortexAppear ? ((int)this.mVortexBGAlpha) : 255);
				g.SetColor(mColor);
				g.SetDrawMode(1);
				if (g.Is3D())
				{
					g.DrawImageTransformF(imageByID, this.mGlobalTranform, vortexBeam.mX + (float)ZumasRevenge.Common._S(centerX), vortexBeam.mY + (float)ZumasRevenge.Common._S(centerY));
				}
				else
				{
					g.DrawImageTransform(imageByID, this.mGlobalTranform, vortexBeam.mX + (float)ZumasRevenge.Common._S(centerX), vortexBeam.mY + (float)ZumasRevenge.Common._S(centerY));
				}
				g.SetDrawMode(0);
				g.SetColorizeImages(false);
			}
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_1);
			this.mGlobalTranform.Reset();
			this.mGlobalTranform.Scale(this.mVortexFrogScale, this.mVortexFrogScale);
			this.mGlobalTranform.RotateRad(this.mVortexFrogAngle);
			float x = (float)((double)this.mVortexFrogRadius * Math.Cos((double)this.mVortexFrogAngle) + (double)ZumasRevenge.Common._S(centerX));
			float y = (float)((double)this.mVortexFrogRadius * -(float)Math.Sin((double)this.mVortexFrogAngle) + (double)ZumasRevenge.Common._S(centerY));
			if (g.Is3D())
			{
				g.DrawImageTransformF(imageByID2, this.mGlobalTranform, x, y);
			}
			else
			{
				g.DrawImageTransform(imageByID2, this.mGlobalTranform, x, y);
			}
			this.mLevel.mFrog.DrawConfusionMarks(g);
			if (draw_overlay && !this.mVortexAppear && this.mVortexBGAlpha > 0f)
			{
				g.SetColor(0, 0, 0, (int)this.mVortexBGAlpha);
				g.mTransX = 0f;
				g.FillRect(this.mApp.GetScreenRect());
				g.mTransX = (float)this.mApp.mBoardOffsetX;
			}
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00038614 File Offset: 0x00036814
		protected void DrawIronFrogWin(Graphics g)
		{
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGEXOTICA100_STROKE);
			Font fontByID2 = Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_STROKE);
			Font fontByID3 = Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_RED_STROKE_YELLOW);
			g.PushState();
			if (this.mIronFrogAlpha < 255f)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)this.mIronFrogAlpha);
			}
			g.SetFont(fontByID);
			g.SetColor(255, 255, 255, (int)this.mIronFrogAlpha);
			g.WriteWordWrapped(new Rect(ZumasRevenge.Common._DS(ZumasRevenge.Common._M(316)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(0)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M2(700)), 1000), "VICTORY!", -1, 0);
			g.SetFont(fontByID2);
			g.SetColor(50, 255, 162, (int)this.mIronFrogAlpha);
			g.WriteWordWrapped(new Rect(ZumasRevenge.Common._DS(ZumasRevenge.Common._M(316)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(170)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M2(700)), 1000), "You have beaten the\nIron Frog Gauntlet!", ZumasRevenge.Common._DS(ZumasRevenge.Common._M3(70)), 0);
			g.SetFont(fontByID2);
			g.SetColor(255, 252, 157, (int)this.mIronFrogAlpha);
			g.WriteString(TextManager.getInstance().getString(114), ZumasRevenge.Common._DS(ZumasRevenge.Common._M(650)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(640)));
			g.SetColor(255, 255, 255, (int)this.mIronFrogAlpha);
			g.WriteString(JeffLib.Common.UpdateToTimeStr(this.mApp.mUserProfile.mIronFrogStats.mCurTime, true, 1), ZumasRevenge.Common._DS(ZumasRevenge.Common._M(780)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(640)), -1, -1);
			g.SetColor(255, 252, 157, (int)this.mIronFrogAlpha);
			g.WriteString(TextManager.getInstance().getString(115), ZumasRevenge.Common._DS(ZumasRevenge.Common._M(846)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(790)));
			g.SetColor(255, 255, 255, (int)this.mIronFrogAlpha);
			g.WriteString(SexyFramework.Common.CommaSeperate(this.mScore), ZumasRevenge.Common._DS(ZumasRevenge.Common._M(1000)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(790)), -1, -1);
			g.SetFont(fontByID3);
			if (this.mApp.mUserProfile.mIronFrogStats.mBestTime == this.mApp.mUserProfile.mIronFrogStats.mCurTime)
			{
				g.DrawString(TextManager.getInstance().getString(139), ZumasRevenge.Common._DS(ZumasRevenge.Common._M(780)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(690)));
			}
			if (this.mApp.mUserProfile.mIronFrogStats.mBestScore == this.mScore)
			{
				g.DrawString(TextManager.getInstance().getString(140), ZumasRevenge.Common._DS(ZumasRevenge.Common._M(1000)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(850)));
			}
			if (this.mIronFrogAlpha < 255f)
			{
				g.SetColor(255, 255, 255, (int)this.mIronFrogAlpha);
				this.mIronFrogBtn.mBtnNoDraw = false;
				g.Translate(this.mIronFrogBtn.mX, this.mIronFrogBtn.mY);
				this.mIronFrogBtn.Draw(g);
				this.mIronFrogBtn.mBtnNoDraw = true;
			}
			g.SetColorizeImages(false);
			g.PopState();
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x000389C0 File Offset: 0x00036BC0
		protected void DrawAdventureStats(Graphics g)
		{
			if (this.mDoingTransition || this.mLevel.mBoss != null || this.mLevel.mNum == 2147483647 || !this.DisplayingEndOfLevelStats() || this.IronFrogMode())
			{
				return;
			}
			bool flag = g.Is3D() && this.mBossSmScale > 1.0;
			SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
			if (flag)
			{
				SexyPoint zoomPoint = this.GetZoomPoint();
				sexyTransform2D.Translate(-g.mTransX - (float)zoomPoint.mX, (float)(-(float)zoomPoint.mY));
				sexyTransform2D.Scale((float)(1.0 + (this.mBossSmScale - 1.0) * 0.25), (float)(1.0 + (this.mBossSmScale - 1.0) * 0.25));
				sexyTransform2D.Translate(g.mTransX + (float)zoomPoint.mX, (float)zoomPoint.mY);
				g.Get3D().PushTransform(sexyTransform2D);
			}
			this.SetBaseBossOffset();
			ZumasRevenge.Common.DrawCommonDialogBacking(g, this.mAStatsFrame.mX, this.mAStatsFrame.mY, this.mAStatsFrame.mWidth, this.mAStatsFrame.mHeight);
			this.DrawAdventureStatsBanner(g);
			this.DrawLilyPadTrail(g);
			this.DrawBossCircle(g);
			this.DrawAdventureStatsFrog(g);
			this.DrawStatsData(g);
			this.DrawPointsData(g);
			if (flag)
			{
				this.DrawBossTaunt(g);
				this.ZoomInOnBoss(g);
			}
			else
			{
				this.ZoomInOnBoss(g);
				this.DrawBossTaunt(g);
			}
			if (flag)
			{
				g.Get3D().PopTransform();
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00038B78 File Offset: 0x00036D78
		protected SexyPoint GetZoomPoint()
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_ADVENTURE_STATS_RED_BOSS_FLASH);
			return new SexyPoint
			{
				mX = this.mApp.GetWideScreenAdjusted(ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_ADVENTURE_STATS_RED_BOSS_FLASH)) + imageByID.GetWidth()),
				mY = ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_ADVENTURE_STATS_RED_BOSS_FLASH)) + imageByID.GetHeight() / 2
			};
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00038BDC File Offset: 0x00036DDC
		protected void SetBaseBossOffset()
		{
			if (this.mBossOffset.mX != 0 || this.mBossOffset.mY != 0)
			{
				return;
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_ADVENTURE_STATS_PIP);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_UI_ADVENTURE_STATS_BOSS_CIRCLE);
			int mGameRes = GameApp.mGameRes;
			if (mGameRes == 320)
			{
				this.mBossOffset.mX = (int)((float)imageByID2.GetHeight() * 0.05f);
				this.mBossOffset.mY = (int)((float)imageByID2.GetHeight() * 0.02f);
				return;
			}
			if (mGameRes == 640)
			{
				this.mBossOffset.mX = (int)((float)imageByID2.GetHeight() * -0.012f);
				this.mBossOffset.mY = (int)((float)imageByID2.GetHeight() * 0.01f);
				return;
			}
			if (mGameRes != 768)
			{
				return;
			}
			this.mBossOffset.mX = (int)((float)imageByID.GetWidth() * 0.52f);
			this.mBossOffset.mY = (int)((float)imageByID2.GetHeight() * 0.02f);
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00038CD4 File Offset: 0x00036ED4
		protected void DrawAdventureStatsBanner(Graphics g)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_ADVENTURE_STATS_BANNER);
			int num = ZumasRevenge.Common._DS(10);
			Rect bannerFrame = this.GetBannerFrame();
			Rect rect = new Rect(bannerFrame);
			rect.mY += num;
			rect.mHeight = (int)((float)rect.mHeight * 0.5f);
			Rect theRect = new Rect(rect);
			theRect.mY += theRect.mHeight;
			string theLine = TextManager.getInstance().getString(683) + " " + ((this.mLevel.mZone - 1) * 10 + this.mLevel.mNum);
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_STROKE);
			Font fontByID2 = Res.GetFontByID(ResID.FONT_SHAGEXOTICA38_BLACK);
			g.DrawImage(imageByID, bannerFrame.mX, bannerFrame.mY, bannerFrame.mWidth, bannerFrame.mHeight);
			g.SetFont(fontByID);
			g.SetColor(SexyColor.White);
			g.WriteWordWrapped(rect, this.mEndLevelDisplayName, -1, 0);
			g.SetFont(fontByID2);
			g.WriteWordWrapped(theRect, theLine, -1, 0);
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00038DF0 File Offset: 0x00036FF0
		protected Rect GetBannerFrame()
		{
			float num = (float)Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_STROKE).StringWidth(this.mEndLevelDisplayName) * 1.25f;
			float num2 = (float)Res.GetImageByID(ResID.IMAGE_UI_ADVENTURE_STATS_BANNER).GetWidth();
			float num3 = ((num > num2) ? num : num2);
			float num4 = (float)Res.GetImageByID(ResID.IMAGE_UI_ADVENTURE_STATS_BANNER).GetHeight();
			float num5 = (float)this.mAStatsFrame.mX + ((float)this.mAStatsFrame.mWidth - num3) / 2f - (float)ZumasRevenge.Common._DS(150);
			float num6 = (float)this.mAStatsFrame.mY - num4 * 0.33f;
			return new Rect((int)num5, (int)num6, (int)num3, (int)num4);
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00038E98 File Offset: 0x00037098
		protected void DrawContinueButton(Graphics g)
		{
			if (this.mStatsContinueBtn != null)
			{
				return;
			}
			Rect continueButtonRect = this.GetContinueButtonRect();
			g.DrawImage(Res.GetImageByID(ResID.IMAGE_GUI_ADVENTURESTATS_CONTINUE), continueButtonRect.mX, continueButtonRect.mY);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00038ED4 File Offset: 0x000370D4
		protected void DrawLilyPadTrail(Graphics g)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_ADVENTURE_STATS_PIP);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_UI_ADVENTURE_STATS_PIP_END);
			int theY = ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_ADVENTURE_STATS_PIP));
			g.DrawImage(imageByID2, this.mAStatsFrame.mX + ZumasRevenge.Common._DS(10), theY);
			for (int i = 0; i < 10; i++)
			{
				g.DrawImage(imageByID, this.mAStatsFrame.mX + imageByID.mWidth / 2 + i * imageByID.mWidth, theY);
			}
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00038F54 File Offset: 0x00037154
		protected void DrawBossCircle(Graphics g)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_ADVENTURE_STATS_BOSS_CIRCLE);
			int x = ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_ADVENTURE_STATS_BOSS_CIRCLE)) + this.mBossOffset.mX;
			int theY = ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_ADVENTURE_STATS_BOSS_CIRCLE)) + this.mBossOffset.mY;
			g.DrawImage(imageByID, GameApp.gApp.GetWideScreenAdjusted(x) - 63, theY);
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00038FBC File Offset: 0x000371BC
		protected void DrawAdventureStatsFrog(Graphics g)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_ADVENTURE_STATS_PIP);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_UI_ADVENTURE_STATS_FROG);
			int num = (int)((float)(this.mAStatsFrame.mX + imageByID.mWidth / 2 + (this.mLevel.mNum - 1) * imageByID.mWidth) - (float)imageByID2.mWidth * 0.5f);
			int num2 = ((this.mLevel.mNum == 10) ? num : (num + imageByID.mWidth));
			float frogLeapProgress = this.GetFrogLeapProgress();
			int theX = (int)((float)num + (float)(num2 - num) * frogLeapProgress);
			int theY = ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_ADVENTURE_STATS_FROG));
			float num3 = 1.2f;
			float num4;
			if (frogLeapProgress < 0.5f)
			{
				num4 = 1f + (num3 - 1f) * frogLeapProgress * 2f;
			}
			else
			{
				num4 = num3 - (num3 - 1f) * (frogLeapProgress - 0.5f) * 2f;
			}
			g.DrawImage(imageByID2, theX, theY, (int)(num4 * (float)imageByID2.mWidth), (int)(num4 * (float)imageByID2.mHeight));
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x000390CC File Offset: 0x000372CC
		protected float GetFrogLeapProgress()
		{
			float num = 50f;
			float num2 = 40f;
			float num3 = ((float)this.mAdvStatsTime - num) / num2;
			if (num3 > 1f)
			{
				num3 = 1f;
			}
			else if (num3 < 0f)
			{
				num3 = 0f;
			}
			if (this.mLevel.mNum == 10)
			{
				num3 = 0f;
			}
			return num3;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00039128 File Offset: 0x00037328
		protected void DrawBossTaunt(Graphics g)
		{
			if (this.mAdvStatsTime < 100)
			{
				return;
			}
			Font fontByID = Res.GetFontByID(ResID.FONT_MAIN22);
			string text = this.mApp.GetLevelMgr().mZones[this.mLevel.mZone - 1].mBossTaunts[this.mLevel.mNum - 1];
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_ADVENTURE_SPEECHBALLOON_YELLOW);
			int width = imageByID.GetWidth();
			int height = imageByID.GetHeight();
			int num = (int)((float)(this.mAStatsFrame.mX + this.mAStatsFrame.mWidth) - (float)width * 0.95f);
			int num2 = (int)((float)this.mAStatsFrame.mY - (float)height * 0.2f);
			float num3 = 0.4f;
			int num4 = (int)((float)height * num3);
			int num5 = ZumasRevenge.Common._DS(30);
			Rect rect = new Rect(num, num2, width, height);
			Rect theRect = new Rect(num + num5, num2 + num5, width - num5 * 2, height - num4 - num5 * 2);
			int num6 = ZumasRevenge.Common._GetWordWrappedHeight(text, fontByID, theRect.mWidth);
			if (num6 > theRect.mHeight)
			{
				int num7 = num6 - theRect.mHeight;
				theRect.mHeight += num7;
				rect.mHeight += (int)((float)num7 + (float)num7 * num3);
			}
			theRect.mY += (int)((float)(theRect.mHeight - num6) * 0.5f);
			g.DrawImage(imageByID, rect.mX, rect.mY, rect.mWidth, rect.mHeight);
			g.SetFont(fontByID);
			g.SetColor(SexyColor.Black);
			g.WriteWordWrapped(theRect, text, -1, 0);
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x000392CC File Offset: 0x000374CC
		protected void DrawStatsData(Graphics g)
		{
			List<Rect> statsDataTable = this.GetStatsDataTable();
			string @string = TextManager.getInstance().getString(147);
			string theLine = string.Concat(new object[]
			{
				"x",
				this.mEndLevelStats.mMaxCombo + 1,
				"\nx",
				this.mEndLevelStats.mMaxInARow,
				"\nx",
				this.mEndLevelStats.mNumGaps,
				"\nx",
				this.mEndLevelStats.mNumGemsCleared
			});
			string string2 = TextManager.getInstance().getString(148);
			string theLine2 = string.Concat(new string[]
			{
				JeffLib.Common.UpdateToTimeStr(this.mEndLevelParTime),
				"\n",
				JeffLib.Common.UpdateToTimeStr(this.mEndLevelStats.mTimePlayed),
				"\n",
				this.GetBonusPointsString(),
				"\n",
				JeffLib.Common.UpdateToTimeStr(this.mApp.mUserProfile.GetAdvModeVars().mBestLevelTime[this.mEndLevelNum - 1])
			});
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGEXOTICA38_BLACK_GLOW);
			g.SetFont(fontByID);
			g.SetColor(255, 253, 98);
			g.WriteWordNoAutoWrapped(@string, statsDataTable[0].mX, statsDataTable[0].mY);
			g.WriteWordNoAutoWrapped(string2, statsDataTable[2].mX, statsDataTable[2].mY);
			g.SetColor(253, 126, 0);
			g.WriteWordWrapped(statsDataTable[1], theLine, -1, 1);
			g.WriteWordWrapped(statsDataTable[3], theLine2, -1, 1);
			this.DrawRedBurst(g, statsDataTable);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x000394A4 File Offset: 0x000376A4
		protected List<Rect> GetStatsDataTable()
		{
			int num = (int)((float)this.mAStatsFrame.mWidth * 0.7f);
			int num2 = (int)((float)this.mAStatsFrame.mHeight * 0.25f);
			int theX = (int)((float)this.mAStatsFrame.mX + (float)(this.mAStatsFrame.mWidth - num) * 0.5f);
			int theY = (int)((float)this.mAStatsFrame.mY + (float)(this.mAStatsFrame.mHeight - num2) * 0.5f);
			int theWidth = (int)((float)num * 0.245f);
			int theWidth2 = (int)((float)num * 0.25f);
			int num3 = (int)((float)num * 0.05f);
			List<Rect> list = new List<Rect>();
			for (int i = 0; i < 4; i++)
			{
				list.Add(default(Rect));
			}
			for (int j = 0; j < Enumerable.Count<Rect>(list); j++)
			{
				if (j == 0)
				{
					list[j] = new Rect(theX, theY, theWidth, num2);
				}
				else if (j % 2 == 0)
				{
					list[j] = new Rect(list[j - 1].mX + list[j - 1].mWidth + num3, theY, theWidth, num2);
				}
				else
				{
					list[j] = new Rect(list[j - 1].mX + list[j - 1].mWidth, theY, theWidth2, num2);
				}
			}
			return list;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00039610 File Offset: 0x00037810
		protected string GetBonusPointsString()
		{
			int theValue;
			if (this.mStatsState < 0)
			{
				theValue = 0;
			}
			else if (this.mStatsState == 0)
			{
				theValue = this.mCurStatsPointCounter;
			}
			else
			{
				theValue = this.mEndLevelAceTimeBonus;
			}
			return SexyFramework.Common.CommaSeperate(theValue);
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00039648 File Offset: 0x00037848
		protected void DrawRedBurst(Graphics g, List<Rect> aColumns)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_STATSCREEN_BURST);
			int theX = (int)((float)aColumns[2].mX + ((float)(aColumns[2].mWidth + aColumns[3].mWidth) - (float)imageByID.GetWidth() * 1.5f) / 2f);
			int num = aColumns[1].mY + aColumns[1].mHeight / 5;
			int num2 = 0;
			int num3 = 0;
			if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CHT || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_PL || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_RU)
			{
				num2 = -10;
				num3 = 10;
			}
			g.SetDrawMode(1);
			g.DrawImage(imageByID, theX, num + num2, num3 + (int)((float)imageByID.GetWidth() * 1.5f), imageByID.GetHeight());
			g.SetDrawMode(0);
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x00039718 File Offset: 0x00037918
		protected void DrawPointsData(Graphics g)
		{
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGEXOTICA38_BLACK_GLOW);
			int num = fontByID.GetHeight() + ZumasRevenge.Common._DS(10);
			int num2 = 100;
			int num3 = fontByID.StringWidth(TextManager.getInstance().getString(146)) + num2;
			int num4 = (int)((float)this.mAStatsFrame.mWidth * 0.5f);
			num4 = ((num4 > num3) ? num4 : num3);
			int num5 = (int)((float)this.mAStatsFrame.mHeight * 0.33f);
			int theX = (int)((float)this.mAStatsFrame.mX + (float)(this.mAStatsFrame.mWidth - num4) * 0.5f);
			int num6 = this.mAStatsFrame.mY + num5 * 2;
			Rect[] array = new Rect[3];
			for (int i = 0; i < 3; i++)
			{
				array[i] = new Rect(theX, num6 + num * i, num4, num);
			}
			g.SetFont(fontByID);
			this.DrawPerfectLevelBonus(g, array[0]);
			this.DrawPointsThisLevel(g, array[1]);
			this.DrawPointsDataString(g, array[2], TextManager.getInstance().getString(145), this.mScore, new SexyColor(254, 255, 101), SexyColor.White);
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0003986C File Offset: 0x00037A6C
		protected void DrawPerfectLevelBonus(Graphics g, Rect inFrame)
		{
			if (this.mLevel.mNum > 10 || !this.mWasPerfectLevel)
			{
				return;
			}
			this.DrawPointsDataString(g, inFrame, TextManager.getInstance().getString(146), this.GetPerfectBonus(), new SexyColor(255, 231, 40), new SexyColor(233, 105, 61));
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x000398CD File Offset: 0x00037ACD
		protected void DrawPointsDataString(Graphics g, Rect inFrame, string inLabel, int inPoints, SexyColor inLabelColor, SexyColor inPointsColor)
		{
			g.SetColor(inLabelColor);
			g.WriteWordWrapped(inFrame, inLabel);
			g.SetColor(inPointsColor);
			g.WriteWordWrapped(inFrame, SexyFramework.Common.CommaSeperate(inPoints), -1, 1);
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x000398FC File Offset: 0x00037AFC
		protected void DrawPointsThisLevel(Graphics g, Rect inFrame)
		{
			int num = 0;
			if (this.mStatsState == 1)
			{
				num = this.mCurStatsPointCounter;
			}
			else if (this.mStatsState == 2)
			{
				num = this.mLevelPoints + this.mEndLevelAceTimeBonus + this.GetPerfectBonus(this.mLevel.mZone, this.mLevel.mNum);
			}
			num += this.mCurveClearBonus;
			this.DrawPointsDataString(g, inFrame, TextManager.getInstance().getString(144), num, new SexyColor(255, 215, 0), new SexyColor(233, 96, 0));
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00039990 File Offset: 0x00037B90
		protected void ZoomInOnBoss(Graphics g)
		{
			g.SetColorizeImages(false);
			SexyPoint point = new SexyPoint(ZumasRevenge.Common._DS(70), ZumasRevenge.Common._DS(50)) * this.mBossSmPosPct;
			SexyPoint point2 = this.GetZoomPoint();
			point2 += point;
			g.PushState();
			SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
			if (g.Is3D())
			{
				sexyTransform2D.Translate(-g.mTransX - (float)point2.mX, (float)(-(float)point2.mY));
				sexyTransform2D.Scale((float)(1.0 + (this.mBossSmScale - 1.0) * 0.30000001192092896), (float)(1.0 + (this.mBossSmScale - 1.0) * 0.30000001192092896));
				sexyTransform2D.Translate(g.mTransX + (float)point2.mX, (float)point2.mY);
				g.Get3D().PushTransform(sexyTransform2D);
			}
			else
			{
				g.SetScale((float)this.mBossSmScale, (float)this.mBossSmScale, (float)point2.mX, (float)point2.mY);
			}
			point.mX -= 63;
			this.DrawRedBossCircle(g, point);
			this.DrawBossPortrait(g, point);
			if (g.Is3D())
			{
				g.Get3D().PopTransform();
			}
			g.PopState();
			g.mTransX = 0f;
			this.ReddenScreen(g);
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00039B14 File Offset: 0x00037D14
		protected void ReddenScreen(Graphics g)
		{
			if (!g.Is3D() || this.mBossRedPct <= 0.0)
			{
				return;
			}
			g.PushState();
			g.SetColor(255, 0, 0, (int)(255.0 * this.mBossRedPct));
			g.mClipRect = new Rect(0, 0, GameApp.gApp.GetScreenRect().mWidth, GameApp.gApp.GetScreenRect().mHeight);
			g.FillRect(g.mClipRect);
			g.PopState();
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00039BA8 File Offset: 0x00037DA8
		protected void DrawRedBossCircle(Graphics g, SexyPoint inOffset)
		{
			if (this.mLevel.mNum != 10)
			{
				return;
			}
			int x = ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_ADVENTURE_STATS_RED_BOSS_FLASH)) + this.mBossOffset.mX + inOffset.mX;
			int theY = ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_ADVENTURE_STATS_RED_BOSS_FLASH)) + this.mBossOffset.mY + inOffset.mY + ZumasRevenge.Common._DS(10);
			g.PushState();
			g.SetColorizeImages(true);
			if (this.mBossRedPct > 0.0)
			{
				g.SetColor(255, 255, 255, (int)(255.0 * this.mBossRedPct));
			}
			else
			{
				int alpha = Math.Min(255, 127 + JeffLib.Common.GetAlphaFromUpdateCount(this.mUpdateCnt, 128) + ((int)this.mBossSmScale - 1) * 64);
				g.SetColor(255, 255, 255, alpha);
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_ADVENTURE_STATS_RED_BOSS_FLASH);
			g.DrawImage(imageByID, GameApp.gApp.GetWideScreenAdjusted(x), theY);
			g.PopState();
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x00039CD0 File Offset: 0x00037ED0
		protected void DrawBossPortrait(Graphics g, SexyPoint inOffset)
		{
			ResID id = ResID.RESOURCE_MAX;
			switch (this.mLevel.mZone)
			{
			case 1:
				id = ResID.IMAGE_UI_ADVENTURE_STATS_TIGER;
				break;
			case 2:
				id = ResID.IMAGE_UI_ADVENTURE_STATS_DOCTOR;
				break;
			case 3:
				id = ResID.IMAGE_UI_ADVENTURE_STATS_SKULL;
				break;
			case 4:
				id = ResID.IMAGE_UI_ADVENTURE_STATS_MOSQUITO;
				break;
			case 5:
				id = ResID.IMAGE_UI_ADVENTURE_STATS_SQUID;
				break;
			case 6:
				id = ResID.IMAGE_UI_ADVENTURE_STATS_CLOAK;
				break;
			}
			int x = ZumasRevenge.Common._DS(Res.GetOffsetXByID(id)) + this.mBossOffset.mX + inOffset.mX;
			int theY = ZumasRevenge.Common._DS(Res.GetOffsetYByID(id)) + this.mBossOffset.mY + inOffset.mY;
			Image imageByID = Res.GetImageByID(id);
			g.DrawImage(imageByID, GameApp.gApp.GetWideScreenAdjusted(x), theY);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00039D9C File Offset: 0x00037F9C
		protected void CheckEndConditions()
		{
			if (this.mFrog.IsFiring() || this.mBulletList.Count > 0)
			{
				return;
			}
			if (this.mLevel.IsFinalBossLevel() && this.mLevel.AllTorchesOut() && this.mLevel.AllCurvesAtRolloutPoint())
			{
				this.mGameState = GameState.GameState_FinalBossPart1Finished;
				this.GetBetaStats().BeatLevel(this.mLevelStats.mTimePlayed, this.mLevel.mParTime, this.GetAceTimeBonus(), this.GetPerfectBonus(), (float)this.mLevel.mFurthestBallDistance / 100f, this.mScore - this.mLevelBeginScore, this.mScore, this.mLives);
				this.InitEndOfTorchLevel();
				this.mStateCount = 0;
				this.DeleteBullets();
				return;
			}
			int num = 0;
			bool flag = false;
			if (this.mIsWinning)
			{
				this.mGameState = GameState.GameState_BeatLevelBonus;
				return;
			}
			int num2 = 0;
			while (num2 < this.mLevel.mNumCurves && this.mLevel.mCurveMgr[num2].IsWinning())
			{
				num2++;
				num++;
			}
			int num3 = 1;
			if (num == this.mLevel.mNumCurves && (this.mLevel.mNumCurves > 0 || this.mLevel.mHaveReachedTarget))
			{
				this.mIsWinning = true;
				this.mBossRedPct.SetConstant(0.0);
				this.mBossIntroBGAlpha.SetConstant(0.0);
				this.mBossSmScale.SetConstant(1.0);
				this.mBossSmPosPct.SetConstant(0.0);
				this.mBossRedPct.SetConstant(0.0);
				this.mLevelStats.mTimePlayed = this.mStateCount - this.mIgnoreCount;
				if (this.mLevel.mBoss == null && !this.IronFrogMode() && !this.GauntletMode() && !this.mLevel.IsFinalBossLevel())
				{
					if (this.mLevel.mZone < 7 && this.mLevel.mNum <= 10)
					{
						int num4 = this.mApp.mUserProfile.GetAdvModeVars().mBestLevelTime[(this.mLevel.mZone - 1) * 10 + this.mLevel.mNum - 1];
						if (this.mLevelStats.mTimePlayed < num4)
						{
							this.mApp.mUserProfile.GetAdvModeVars().mBestLevelTime[(this.mLevel.mZone - 1) * 10 + this.mLevel.mNum - 1] = this.mLevelStats.mTimePlayed;
						}
					}
					this.SetupLevelCompleteText();
				}
				this.mLevel.AllBallsDestroyed();
				this.mFrog.EmptyBullets();
				if ((this.mLevel.mNum == 5 && !this.IronFrogMode() && !this.GauntletMode()) || this.mLevel.mNum == 10 || this.mLevel.mBoss != null)
				{
					this.mApp.mUserProfile.GetAdvModeVars().mDDSTier = (this.mApp.mUserProfile.GetAdvModeVars().mRestartDDSTier = -1);
				}
				int num5 = (this.mLevel.mZone - 1) * 10 + this.mLevel.mNum;
				if (num5 > this.mApp.mUserProfile.GetAdvModeVars().mHighestLevelBeat && !this.IronFrogMode() && this.mLevel.mNum != 2147483647 && !this.mLevel.IsFinalBossLevel())
				{
					this.mApp.mUserProfile.GetAdvModeVars().mHighestLevelBeat = num5;
				}
				if (ZumasRevenge.Common.StrEquals(this.mLevel.mId, this.mApp.GetLevelMgr().GetLevelId(this.mApp.GetLevelMgr().GetLastIronFrogLevel())))
				{
					this.mApp.mUserProfile.mHasBeatIronFrogMode = true;
				}
				this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_LEVEL_COMPLETE));
				this.mApp.mSoundPlayer.Loop(Res.GetSoundByID(ResID.SOUND_BONUS100LOOP));
				this.mGameState = GameState.GameState_BeatLevelBonus;
				if (!this.mApp.mResourceManager.IsGroupLoaded("AdventureStats"))
				{
					this.mApp.mResourceManager.PrepareLoadResources("AdventureStats");
				}
				if (ZumasRevenge.Common.StrEquals(this.mLevel.mId, this.mApp.GetLevelMgr().GetLevelId(this.mApp.GetLevelMgr().GetLastIronFrogLevel())))
				{
					this.mApp.PlaySong(144);
				}
				else if (!this.IronFrogMode())
				{
					int song = 120;
					switch (Board.gTuneNum)
					{
					case 0:
						song = 120;
						break;
					case 1:
						song = 121;
						break;
					case 2:
						song = 122;
						break;
					case 3:
						song = 123;
						break;
					case 4:
						song = 124;
						break;
					case 5:
						song = 125;
						break;
					}
					this.mApp.PlaySong(song, ZumasRevenge.Common._M(0.0045f));
				}
				if (this.mAdventureMode)
				{
					if (this.GetAceTimeBonus() > 0)
					{
						this.GetAdvModeTempleStats().mNumLevelsAced++;
					}
					if (this.GetPerfectBonus() > 0)
					{
						this.GetAdvModeTempleStats().mNumPerfectLevels++;
					}
					if (this.mLevel.mNum != 2147483647 && !flag)
					{
						this.mEndLevelNum = (this.mLevel.mZone - 1) * 10 + this.mLevel.mNum;
						this.mEndLevelAceTimeBonus = this.GetAceTimeBonus();
						this.mEndLevelDisplayName = this.mLevel.mDisplayName;
						this.mEndLevelParTime = this.mLevel.mParTime;
						this.mEndLevelStats = this.mLevelStats;
					}
					this.UnlockAchievement(EAchievementType.YOU_WIN);
					if (this.mLevel.m_canGetAchievementNoMove)
					{
						this.UnlockAchievement(EAchievementType.FROZEN_FROG);
					}
					if (this.mLevel.m_canGetAchievementNoJump)
					{
						this.UnlockAchievement(EAchievementType.FROG_STATUE);
					}
				}
				else if (this.IronFrogMode() && this.mLevel.mNum == 10)
				{
					this.mApp.mUserProfile.mIronFrogStats.mNumVictories++;
					if (this.mApp.mUserProfile.mIronFrogStats.mCurTime < this.mApp.mUserProfile.mIronFrogStats.mBestTime || this.mApp.mUserProfile.mIronFrogStats.mBestTime == 0)
					{
						this.mApp.mUserProfile.mIronFrogStats.mBestTime = this.mApp.mUserProfile.mIronFrogStats.mCurTime;
					}
					if (this.mApp.mUserProfile.mIronFrogStats.mCurTime <= 150099)
					{
						this.mApp.SetAchievement("iron_will");
					}
				}
				if (this.mApp.GetLevelMgr().mScoreTips.size<ScoreTip>() > 0)
				{
					this.mScoreTipIdx = this.mApp.GetLevelMgr().GetScoreTipIdx((this.mLevel.mZone - 1) * 10 + this.mLevel.mNum);
				}
				this.DoAccuracy(false);
				for (int i = 0; i < this.mLevel.mNumCurves; i++)
				{
					CurveMgr curveMgr = this.mLevel.mCurveMgr[i];
					int j = curveMgr.mLastClearedBallPoint;
					int curveLength = curveMgr.GetCurveLength();
					int num6 = (curveLength - j) % 70;
					if (num6 != 0)
					{
						j += num6;
					}
					while (j <= curveLength)
					{
						int num7 = 0;
						int num8 = 0;
						int num9 = 0;
						curveMgr.GetPoint(j, out num7, out num8, out num9);
						EndLevelExplosion endLevelExplosion = this.mEndLevelExplosionPool.Alloc();
						endLevelExplosion.mPIEffect.ResetAnim();
						endLevelExplosion.mPIEffect.mOptimizeValue = 2;
						this.mEndLevelExplosions.Add(endLevelExplosion);
						this.mEffectBatch.AddEffect(endLevelExplosion.mPIEffect);
						endLevelExplosion.SetPos(num7, num8);
						endLevelExplosion.mDelay = num3;
						endLevelExplosion.mX = num7;
						endLevelExplosion.mY = num8;
						num3 += ZumasRevenge.Common._M(6);
						j += 70;
					}
				}
				return;
			}
			num = 0;
			int losing = -1;
			int k = 0;
			while (k < this.mLevel.mNumCurves)
			{
				if (this.mLevel.mCurveMgr[k].IsLosing())
				{
					losing = k;
					break;
				}
				k++;
				num++;
			}
			if (num != this.mLevel.mNumCurves)
			{
				this.SetLosing(losing);
				return;
			}
			if (this.mLevel.mTimer == 0)
			{
				this.SetLosing();
			}
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x0003A600 File Offset: 0x00038800
		public void UnlockAchievement(EAchievementType type)
		{
			if (!this.mApp.mUserProfile.m_AchievementMgr.isAchievementUnlocked(type))
			{
				this.mApp.mUserProfile.m_AchievementMgr.UnlockAchievement(type);
				if (GameApp.UN_UPDATE_VERSION)
				{
					return;
				}
				if (!GameApp.USE_XBOX_SERVICE && !GameApp.USE_TRIAL_VERSION)
				{
					AchievementEntry achievementEntry = this.mApp.mUserProfile.m_AchievementMgr.GetAchievementEntry(type);
					this.ToggleNotification(TextManager.getInstance().getString(92) + TextManager.getInstance().getString(achievementEntry.m_NameResID), Res.GetSoundByID(ResID.SOUND_MIDZONE_NOTIFY));
				}
			}
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0003A69C File Offset: 0x0003889C
		protected void SetupLevelCompleteText()
		{
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGEXOTICA100_STROKE);
			string theString = ((this.GetAceTimeBonus() > 0) ? TextManager.getInstance().getString(141) : TextManager.getInstance().getString(143));
			int num = fontByID.StringWidth(theString) + 5;
			if (this.mLevelCompleteText.mImage != null)
			{
				this.mLevelCompleteText.mImage.Dispose();
			}
			this.mLevelCompleteText = new FwooshImage();
			this.mLevelCompleteText.mImage = new DeviceImage();
			this.mLevelCompleteText.mImage.mApp = this.mApp;
			this.mLevelCompleteText.mImage.SetImageMode(true, true);
			this.mLevelCompleteText.mImage.AddImageFlags(16U);
			this.mLevelCompleteText.mImage.Create(num + 15, fontByID.GetHeight() + 5);
			this.mLevelCompleteText.mDelay = ZumasRevenge.Common._M(150);
			Graphics graphics = new Graphics(this.mLevelCompleteText.mImage);
			graphics.Get3D().ClearColorBuffer(new SexyColor(0, 0));
			graphics.SetFont(fontByID);
			graphics.SetColor(ZumasRevenge.Common._M(255), ZumasRevenge.Common._M1(255), ZumasRevenge.Common._M2(255));
			graphics.DrawString(theString, 5, fontByID.GetAscent());
			graphics.ClearRenderContext();
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0003A7EA File Offset: 0x000389EA
		protected void SetSuckMode(bool s)
		{
			ZumasRevenge.Common.gSuckMode = s;
			if (ZumasRevenge.Common.gSuckMode)
			{
				this.mFrog.EmptyBullets();
				this.mFrog.SetCannonCount(0, false, -1);
			}
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0003A814 File Offset: 0x00038A14
		protected void RestartLevel(bool from_checkpoint, Level copy_effects_from)
		{
			this.mIsRestarting = true;
			Board.gNeedsGauntletHSSound = true;
			string text = this.mLevel.mId;
			int num = 0;
			this.mNeedsBossExtraLife = true;
			bool flag = false;
			if (from_checkpoint)
			{
				this.mApp.mUserProfile.GetAdvModeVars().mDDSTier = ++this.mApp.mUserProfile.GetAdvModeVars().mRestartDDSTier;
				this.mLives = 3;
				int num2 = this.mApp.GetLevelMgr().GetLevelIndex(this.mLevel.mId);
				if (this.mLevel.mNum <= 5)
				{
					num2 -= this.mLevel.mNum - 1;
				}
				else
				{
					num2 -= this.mLevel.mNum - 6;
				}
				text = this.mApp.GetLevelMgr().GetLevelId(num2);
				this.mLevelNum = num2 + 1;
				num = this.mApp.GetLevelMgr().GetLevelById(text).mNum;
				this.mApp.mUserProfile.GetAdvModeVars().mNumDeathsCurLevel = 0;
				this.mApp.mUserProfile.GetAdvModeVars().mNumZumasCurLevel = 0;
			}
			else if (this.mLevel.mZone == 1 && this.mLevel.mNum <= 5 && this.mLives <= 0)
			{
				this.mApp.mUserProfile.GetAdvModeVars().mDDSTier = ++this.mApp.mUserProfile.GetAdvModeVars().mRestartDDSTier;
				this.mLives = 3;
				text = "jungle1";
				this.mLevelNum = 1;
				flag = true;
			}
			bool flag2 = !from_checkpoint && this.mLevel != null && ZumasRevenge.Common.StrEquals(this.mLevel.mId, text);
			this.Reset(true, flag2, false, !flag2);
			this.mFrog.LevelReset();
			if (from_checkpoint)
			{
				int num3 = num;
				if (num3 != 1)
				{
					if (num3 == 2147483647)
					{
						this.mScore = this.mApp.mUserProfile.GetAdvModeVars().mCheckpointScores[this.mLevel.mZone - 1].mBoss;
					}
					else
					{
						this.mScore = this.mApp.mUserProfile.GetAdvModeVars().mCheckpointScores[this.mLevel.mZone - 1].mMidpoint;
					}
				}
				else
				{
					this.mScore = this.mApp.mUserProfile.GetAdvModeVars().mCheckpointScores[this.mLevel.mZone - 1].mZoneStart;
				}
				this.mLevelBeginScore = this.mScore;
				this.mRollerScore.ForceScore(this.mScore);
				int mPointsForLife = this.mApp.GetLevelMgr().mPointsForLife;
				int num4 = this.mScore / mPointsForLife;
				this.mPointsLeftForExtraLife = (num4 + 1) * mPointsForLife - this.mScore;
			}
			else if (flag)
			{
				this.mScore = 0;
				this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvScore = 0;
				this.mLevelBeginScore = this.mScore;
				this.mRollerScore.ForceScore(this.mScore);
				this.mPointsLeftForExtraLife = this.mApp.GetLevelMgr().mPointsForLife;
			}
			this.StartLevel(text, false, from_checkpoint, false, copy_effects_from);
			this.mHasSeenCheckpointIntro = true;
			if (this.mLevel.mBoss != null)
			{
				this.mLevel.mBoss.mAlphaOverride = 0f;
			}
			this.UpdateGunPos(true);
			this.MakeCachedBackground();
			this.mIsRestarting = false;
			this.mTheNextLevel = this.mLevelNum + 1;
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0003AB81 File Offset: 0x00038D81
		protected void RestartLevel()
		{
			this.RestartLevel(false, null);
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0003AB8C File Offset: 0x00038D8C
		protected bool ShouldBlockInput()
		{
			bool flag = false;
			flag |= this.mGauntletRetryBtn != null;
			flag |= this.mGauntletQuitBtn != null;
			flag |= this.mDoingIronFrogWin;
			flag |= this.mApp.mCredits != null;
			flag |= this.mDoingBossIntroText;
			flag |= this.mDoingEndBossFrogEffect;
			if (this.mLevelTransition != null)
			{
				flag |= !this.mDoingTransition;
				flag |= this.mLevelTransition.mState != 1;
			}
			flag |= this.mCheckpointEffect != null;
			flag |= this.DoingIntros();
			flag |= this.mGameState == GameState.GameState_Boss6DarkFrog;
			flag |= this.mGameState == GameState.GameState_FinalBossPart1Finished;
			flag |= this.mGameState == GameState.GameState_Boss6StoneHeadBurst;
			flag |= this.mGameState == GameState.GameState_Losing;
			flag |= this.mGameState == GameState.GameState_BeatLevelBonus;
			if (this.mLevel != null)
			{
				flag |= this.mLevel.mFrogFlyOff != null;
			}
			flag |= this.mChallengeHelp != null;
			flag |= this.mApp.mGenericHelp != null;
			if (this.mFrog != null)
			{
				flag |= this.mFrog.IsHopping();
			}
			return flag | (this.mFakeCredits != null);
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0003ACCC File Offset: 0x00038ECC
		protected void GetMultTextXY(out int x, out int y, ref int w, ref string comma_seperated_str)
		{
			string text = "";
			if (comma_seperated_str == null)
			{
				comma_seperated_str = text;
			}
			comma_seperated_str = SexyFramework.Common.CommaSeperate(this.mGauntletPointsFromMult);
			int num = 0;
			if (w == 0)
			{
				w = num;
			}
			Font fontByID = Res.GetFontByID(ResID.FONT_MAIN22);
			w = fontByID.StringWidth(comma_seperated_str);
			x = ZumasRevenge.Common._S(this.mFrog.GetCenterX()) - w / 2;
			y = ZumasRevenge.Common._S(this.mFrog.GetCenterY());
			if (this.mLevel.mMoveType == 1 && y > this.mHeight / 2)
			{
				y -= ZumasRevenge.Common._S(this.mFrog.GetHeight() / 2) + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(-190));
				return;
			}
			y += ZumasRevenge.Common._S(this.mFrog.GetHeight() / 2) + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(60));
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0003ADA0 File Offset: 0x00038FA0
		protected void GetMultTextXY(out int x, out int y)
		{
			int num = 0;
			string text = "";
			this.GetMultTextXY(out x, out y, ref num, ref text);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0003ADC4 File Offset: 0x00038FC4
		protected void ClearPIEffects()
		{
			for (int i = 0; i < this.mEndLevelExplosions.Count; i++)
			{
				if (this.mEndLevelExplosions[i] != null)
				{
					this.mEndLevelExplosionPool.Free(this.mEndLevelExplosions[i]);
				}
			}
			this.mEndLevelExplosions.Clear();
			for (int j = 0; j < this.mBallExplosions.Count; j++)
			{
				if (this.mBallExplosions[j] != null)
				{
					this.mBallExplosions[j].Dispose();
				}
			}
			this.mBallExplosions.Clear();
			for (int k = 0; k < this.mLazerBlasts.Count; k++)
			{
				if (this.mLazerBlasts[k] != null)
				{
					this.mLazerBlasts[k].Dispose();
				}
			}
			this.mLazerBlasts.Clear();
			this.mEffectBatch.Clear();
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0003AEA4 File Offset: 0x000390A4
		public Board(GameApp app, int gauntlet_level)
		{
			if (!app.mResourceManager.IsGroupLoaded("CommonGame") && !app.mResourceManager.LoadResources("CommonGame"))
			{
				app.ShowResourceError(true);
				app.Shutdown();
			}
			if (!app.mResourceManager.IsGroupLoaded("GamePlay") && !app.mResourceManager.LoadResources("GamePlay"))
			{
				app.ShowResourceError(true);
				app.Shutdown();
			}
			this.mIsMouseDown = false;
			this.mLivesInfo = null;
			this.mRollerScore = new RollerScore(gauntlet_level != -1);
			this.mNewBallDelay[0] = (this.mNewBallDelay[1] = -1);
			this.mEndLevelNum = 0;
			this.mEndLevelAceTimeBonus = 0;
			this.mIsRestarting = false;
			this.mAdvWinBtn = null;
			this.mBeatGameTotalScoreTally = (this.mBeatGameLives = (this.mBeatGameNormalScore = 0));
			this.mTimeToBeatAdvMode = 0;
			this.mNeedsBossExtraLife = true;
			this.mDoMuMuMode = false;
			this.mDoingBossIntroText = false;
			this.mDoingBossIntroFightText = false;
			this.mBossIntroAlpha = (this.mBossIntroAlphaRate = (this.mBossTextY = (this.mBattleTextY = (this.mBossTextVY = (this.mBattleTextVY = 0f)))));
			this.mBossIntroDirection = (this.mBossIntroFramesLeft = 0);
			this.mCloakBossIntroAlpha = 255;
			this.mChallengeTextAlpha = 0f;
			this.mIronFrogAlpha = 0f;
			this.mIronFrogBtn = null;
			this.mDoingIronFrogWin = false;
			this.mIronFrogWinDelay = 0;
			this.mBossIntroDelay = 0;
			this.mReturnToMainMenu = false;
			this.mNumDrawFramesLeft = 0;
			this.mAdvStatsTime = 0;
			this.mLevelPoints = 0;
			this.mVortexBGAlpha = 0f;
			this.mStatsContinueBtn = null;
			this.mNumPauseUpdatesToDo = 0;
			this.mDoingFirstTimeIntro = false;
			this.mDoingFirstTimeIntroZoomToGame = false;
			this.mChallengeCupUnlockedFX = null;
			this.mGauntletLastFrogX = (this.mGauntletLastFrogY = 0);
			this.mGauntletMultBarAlpha = 255f;
			this.mGauntletPointsForDiffInc = 0;
			this.mDoingEndBossFrogEffect = false;
			this.mEndBossFadeAmt = 0f;
			this.mEndBossFrogTimer = 0;
			this.mBossSmokePoof = app.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_GENERICBOSSPOOF").Duplicate();
			this.mGauntletPointsFromMult = 0;
			this.mVolcanoBossEssence = null;
			this.mEssenceExplBottom = (this.mEssenceExplTop = null);
			this.mDarkFrogBulletX = 0f;
			this.mDarkFrogBulletY = 0f;
			this.mDarkFrogBulletVX = 0f;
			this.mDarkFrogBulletVY = 0f;
			this.mDarkFrogTimer = 0;
			this.mEssenceScaleTimer = 0;
			this.mEssenceXScale = (this.mEssenceYScale = 0f);
			this.mPreCheckpointLives = 3;
			this.mChallengeHelp = null;
			this.mSmokePoof = null;
			this.mIntroBG = null;
			this.mIntroWater = null;
			this.mIntroFadeAmt = 0f;
			this.mGauntletModeOver = false;
			this.mEndGauntletTimer = 0;
			this.mFullScreenAlpha = (this.mFullScreenAlphaRate = 0);
			this.mForceRestartInAdvMode = false;
			this.mGuideT = 2000f;
			this.mMinTreasureY = (this.mMaxTreasureY = float.MaxValue);
			this.mCanDeleteEffectResources = true;
			this.mDeathSkull = null;
			this.mDisplayAceTime = false;
			this.mIgnoreCount = 0;
			this.mScoreTipIdx = -1;
			this.mGauntletAlpha = 0f;
			this.mFrogFlyOff = null;
			this.mTransitionScreenImage = null;
			this.mDoingTransition = false;
			this.mPlayThud = false;
			this.mAdventureWinScreen = false;
			this.mAdventureWinAlpha = (this.mAdventureWinExtraAlpha = 0f);
			this.mAdventureWinDoorYOff = 0f;
			this.mNeedsCheckpointIntro = false;
			this.mWasShowingCheckpoint = false;
			this.mCheckpointEffect = null;
			this.mFakeCredits = null;
			this.mDarkFrogSequence = null;
			this.mScreenShakeXMax = (this.mScreenShakeYMax = (this.mScreenShakeTime = 0));
			this.mWasPerfectLevel = true;
			this.mIsLoading = false;
			this.mGauntletMode = gauntlet_level >= 1;
			this.mApp = app;
			this.mFrog = null;
			this.mLevel = null;
			this.mNextLevel = null;
			this.mCachedCurveImage = null;
			this.mBackgroundImage = null;
			this.mBoss6StoneBurst = null;
			this.mBoss6VolcanoMelt = null;
			this.mContinueNextLevelOnLoadProfile = false;
			this.mNextLevelOverrideOnLoadProfile = -1;
			this.mCurrentSatPct = 0f;
			this.mDoPostBossMapScreen = false;
			this.mNewIronFrogHS = false;
			this.mHallucinateTimer = 0;
			this.mHasDoneIntroSounds = false;
			this.mLastIntroPad = 0;
			this.mLastIntroPadDelay = 99999;
			this.mCurStatsPointCounter = (this.mCurStatsPointTarget = (this.mCurStatsPointInc = 0));
			this.mStatsState = -1;
			this.mStatsDelay = 0;
			this.mStatsHue = 0;
			this.mNumDeaths = 0;
			this.mUnpauseFrame = 0;
			this.mFruitMultiplier = 1;
			this.mScoreMultiplier = 1;
			this.mGauntletHSTarget = 0;
			this.mHasSeenCheckpointIntro = false;
			this.mAdventureMode = false;
			this.mIsHardMode = false;
			this.mFruitBounceEffect.SetTargetPercents(ZumasRevenge.Common._M(0.5f), ZumasRevenge.Common._M1(1.2f), 1f);
			this.mFruitBounceEffect.SetRate(ZumasRevenge.Common._M(0.15f));
			this.mFruitBounceEffect.SetNumBounces(ZumasRevenge.Common._M(6));
			this.mFruitBounceEffect.SetPct(0f, true);
			this.mFruitBounceEffect.SetRateDivFactor(ZumasRevenge.Common._M(1.25f));
			this.mPreventBallAdvancement = false;
			this.mIntroPadHopCount = 0;
			this.mMenuButton = null;
			this.mGameState = GameState.GameState_None;
			this.mQRand = null;
			for (int i = 0; i < 5; i++)
			{
				this.mCachedTunnelImages[i] = null;
			}
			for (int j = 0; j < 2; j++)
			{
				this.mLazerBeam[j] = app.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_LAZER_BEAM").Duplicate();
				this.mLazerBeam[j].mEmitAfterTimeline = true;
			}
			PIEffect pieffectByID = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_LAZER_BURN);
			this.mLazerBurn = pieffectByID.Duplicate();
			this.mLazerBurn.mEmitAfterTimeline = false;
			ZumasRevenge.Common.SetFXNumScale(this.mLazerBurn, 5f);
			this.mRecalcLazerGuide = (this.mDoGuide = (this.mShowGuide = (this.mRecalcGuide = false)));
			this.mGuideBall = null;
			this.mMapScreen = new MapScreen();
			this.mShowMapScreen = false;
			this.mFruitExplodeEffect = new FruitExplode(this);
			this.mShowBossDDSWindow = (this.mShowDDSWindow = false);
			this.Reset(false, false, true, true);
			this.mScore = 0;
			this.mLevelBeginScore = 0;
			this.mShowBallsDuringPause = false;
			this.mDialogCount = 0;
			this.mPauseUpdateCnt = 0;
			this.mLives = 3;
			this.mLevelNum = 1;
			this.mPointsLeftForExtraLife = this.mApp.GetLevelMgr().mPointsForLife;
			this.mPauseCount = 0;
			this.mCurTreasureNum = -1;
			this.mPauseUpdateCnt = 0;
			this.mLastPauseTick = 0;
			this.mDbgHurry = false;
			this.mSkipToNextLevelOnNextUpdate = false;
			this.mForceTreasure = false;
			this.mLevelTransition = null;
			this.mTreasureStarAlpha = 255;
			this.mTreasureGlowAlpha = (this.mTreasureGlowAlphaRate = 0);
			this.mTreasureCel = 0;
			this.mTreasureStarAngle = 0f;
			this.mGauntletTikiUnlocked = 0;
			this.mNewGauntletHS = false;
			this.mGauntletFinalScorePreBonus = 0;
			this.mGauntletHSIndex = 0;
			this.mTreasureWasHit = false;
			this.mTreasureVY = (this.mTreasureDefaultVY = ZumasRevenge.Common._M(0.25f));
			this.mTreasureAccel = ZumasRevenge.Common._M(-0.01f);
			this.mTreasureYBob = 0f;
			this.mZoneTipIdx = 0;
			this.mScoreTipIdx = 0;
			GameApp.gDDS.mBoard = this;
			this.mStartingGauntletLevel = gauntlet_level;
			this.mSwapBallButton = null;
			this.MakeUIWidgets();
			this.mGauntletRetryBtn = (this.mGauntletQuitBtn = null);
			this.mClip = false;
			this.mSkipShutdownSave = false;
			this.mFatFingerGuideAlpha = 0;
			this.mFatFingerGuideEnabled = false;
			this.mIsHotFrogEnabled = false;
			this.gDrawAutoAimAssistInfo = false;
			this.mInvalidateTouchUp = false;
			this.DisableHaloSwap(false);
			this.mFinishHaloSwap = false;
			this.DisableBallPowerupCheat();
			this.mAimGuide = null;
			this.mNotificationWidget = null;
			this.prevLevelID = "";
			this.mInitialTouchPoint.mX = 0;
			this.mInitialTouchPoint.mY = 0;
			this.mControlMode = Board.CONTROL_MODE.CONTROL_MODE_NONE;
			this.mCurveClearBonus = 0;
			this.mAllowBulletDetection = true;
			this.mAStatsFrame = new Rect(ZumasRevenge.Common._DS(50), ZumasRevenge.Common._DS(75), ZumasRevenge.Common._DS(1500), ZumasRevenge.Common._DS(1050));
			this.mCStatsFrame = new Rect(0, ZumasRevenge.Common._DS(100), 965, ZumasRevenge.Common._DS(1000));
			this.mCStatsFrame.mX = 50;
			this.mMenuButtonX = ZumasRevenge.Common._DS(1480);
			this.mDrawBossUI = true;
			for (int k = 0; k < this.mTunnels.Length; k++)
			{
				this.mTunnels[k] = new List<Tunnel>();
			}
			for (int l = 0; l < this.mLevelNameText.Length; l++)
			{
				this.mLevelNameText[l] = new FwooshImage();
			}
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0003BB14 File Offset: 0x00039D14
		public override void Dispose()
		{
			this.mApp.mSoundPlayer.StopAll();
			this.DoShutdownSaveGame();
			for (int i = 0; i < this.mSmokeParticles.Count; i++)
			{
				this.mSmokeParticles[i] = null;
			}
			this.ClearPIEffects();
			if (this.mNotificationWidget != null)
			{
				this.RemoveWidget(this.mNotificationWidget);
				this.mNotificationWidget.Dispose();
				this.mNotificationWidget = null;
			}
			this.mLazerBeam[0].Dispose();
			this.mLazerBeam[0] = null;
			this.mLazerBeam[1].Dispose();
			this.mLazerBeam[1] = null;
			this.mLazerBurn = null;
			this.mBallExplosionsPool.Dispose();
			this.mEndLevelExplosionPool.Dispose();
			if (this.mApp.mResourceManager.IsGroupLoaded("BossIntro"))
			{
				this.mApp.mResourceManager.DeleteResources("BossIntro");
			}
			if (this.mApp.mResourceManager.IsGroupLoaded("CloakedBoss"))
			{
				this.mApp.mResourceManager.DeleteResources("CloakedBoss");
			}
			if (this.mApp.mResourceManager.IsGroupLoaded("GamePlay"))
			{
				this.mApp.mResourceManager.DeleteResources("GamePlay");
			}
			if (this.mApp.mResourceManager.IsGroupLoaded("Bosses"))
			{
				this.mApp.mResourceManager.DeleteResources("Bosses");
			}
			if (this.mApp.mResourceManager.IsGroupLoaded("CommonBoss"))
			{
				this.mApp.mResourceManager.DeleteResources("CommonBoss");
			}
			if (this.mApp.mResourceManager.IsGroupLoaded("Underwater"))
			{
				this.mApp.mResourceManager.DeleteResources("Underwater");
			}
			if (this.prevLevelID.Length != 0)
			{
				string theGroup = "Levels_" + this.prevLevelID.ToUpper()[0] + this.prevLevelID.Substring(1);
				if (this.mApp.mResourceManager.IsGroupLoaded(theGroup))
				{
					this.mApp.mResourceManager.DeleteResources(theGroup);
				}
			}
			if (this.mLevelCompleteText.mImage != null)
			{
				this.mLevelCompleteText.mImage.Dispose();
			}
			if (this.mChallengePtsText.mImage != null)
			{
				this.mChallengePtsText.mImage.Dispose();
			}
			if (this.mSmokePoof != null)
			{
				this.mSmokePoof.Dispose();
			}
			if (this.mBossSmokePoof != null)
			{
				this.mBossSmokePoof.Dispose();
			}
			if (this.mVolcanoBossEssence != null)
			{
				this.mVolcanoBossEssence.Dispose();
			}
			if (this.mEssenceExplTop != null)
			{
				this.mEssenceExplTop.Dispose();
			}
			if (this.mEssenceExplBottom != null)
			{
				this.mEssenceExplBottom.Dispose();
			}
			if (this.mFakeCredits != null)
			{
				this.mFakeCredits.Dispose();
			}
			if (this.mDarkFrogSequence != null)
			{
				this.mDarkFrogSequence.Dispose();
			}
			for (int j = 0; j < this.mMultiplierBallEffects.Count; j++)
			{
				if (this.mMultiplierBallEffects[j] != null)
				{
					this.mMultiplierBallEffects[j].Dispose();
				}
				this.mMultiplierBallEffects[j] = null;
			}
			if (this.mDarkFrogSequence != null)
			{
				this.mDarkFrogSequence.Dispose();
			}
			if (this.mFruitExplodeEffect != null)
			{
				this.mFruitExplodeEffect.Dispose();
			}
			if (this.mBoss6StoneBurst != null)
			{
				this.mBoss6StoneBurst.Dispose();
			}
			if (this.mBoss6VolcanoMelt != null)
			{
				this.mBoss6VolcanoMelt.Dispose();
			}
			if (this.mDeathSkull != null)
			{
				this.mDeathSkull.Dispose();
			}
			if (this.mAimGuide != null)
			{
				this.mAimGuide.Dispose();
			}
			if (this.mApp.GetBoard() == null)
			{
				GameApp.gDDS.mBoard = null;
			}
			if (this.mFrog != null)
			{
				this.mFrog.Dispose();
			}
			this.mFrog = null;
			if (this.mNextLevel != null)
			{
				this.mNextLevel.Dispose();
			}
			this.mNextLevel = null;
			if (this.mLevel != null)
			{
				this.mLevel.Dispose();
			}
			this.mLevel = null;
			this.mBackgroundImage = null;
			if (Board.gDebugCurveData != null)
			{
				Board.gDebugCurveData.Dispose();
			}
			Board.gDebugCurveData = null;
			if (this.mQRand != null)
			{
				this.mQRand = null;
			}
			if (this.mLevelTransition != null)
			{
				this.mLevelTransition.Dispose();
			}
			this.mLevelTransition = null;
			if (this.mCachedCurveImage != null)
			{
				this.mCachedCurveImage = null;
			}
			this.mMapScreen.Dispose();
			if (this.mLevelNameText[0] != null)
			{
				this.mLevelNameText[0].mImage = null;
			}
			if (this.mLevelNameText[1] != null)
			{
				this.mLevelNameText[1].mImage = null;
			}
			this.RemoveAllWidgets(true, true);
			for (int k = 0; k < this.mPowerEffects.size<PowerEffect>(); k++)
			{
				this.mPowerEffects[k] = null;
			}
			for (int l = 0; l < this.mZumaTips.size<ZumaTip>(); l++)
			{
				this.mZumaTips[l] = null;
			}
			for (int m = 0; m < this.mText.size<BonusTextElement>(); m++)
			{
				this.mText[m].mBonus = null;
			}
			this.EraseTunnels();
			this.DeleteBullets();
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0003C02B File Offset: 0x0003A22B
		public void GauntletModeSetupComplete()
		{
			if (this.mApp.mUserProfile.mWantsChallengeHelp)
			{
				this.ShowChallengeHelpScreen();
			}
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0003C048 File Offset: 0x0003A248
		public void ChallengeHelpClosed()
		{
			this.mApp.mUserProfile.mWantsChallengeHelp = false;
			this.mApp.mWidgetManager.RemoveWidget(this.mChallengeHelp);
			this.mApp.SafeDeleteWidget(this.mChallengeHelp);
			this.mChallengeHelp = null;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0003C094 File Offset: 0x0003A294
		public void ShowChallengeHelpScreen()
		{
			if (this.mChallengeHelp != null)
			{
				return;
			}
			this.mChallengeHelp = new ChallengeHelp(this.mApp.mDialogMap.ContainsKey(2));
			this.mChallengeHelp.mBoard = this;
			this.mApp.mWidgetManager.AddWidget(this.mChallengeHelp);
			this.MarkDirty();
			this.mApp.mWidgetManager.SetFocus(this.mChallengeHelp);
			Dialog dialog = this.mApp.GetDialog(2);
			if (dialog != null)
			{
				this.mApp.mWidgetManager.PutInfront(this.mChallengeHelp, this.mApp.GetDialog(2));
				return;
			}
			this.mApp.mWidgetManager.PutInfront(this.mChallengeHelp, this);
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0003C14E File Offset: 0x0003A34E
		public bool IsCheckpointLevel()
		{
			return (this.mLevel.mNum == 1 && this.mLevel.mZone > 1) || this.mLevel.mNum == int.MaxValue || this.mLevel.mNum == 6;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0003C190 File Offset: 0x0003A390
		public void AddBallExplosionParticleEffect(Ball b, float angle, float range)
		{
			BallExplosion ballExplosion = this.mBallExplosionsPool.Alloc();
			this.mBallExplosions.Add(ballExplosion);
			ballExplosion.SetPos((int)b.GetX(), (int)b.GetY());
			PIEffect mPIEffect = ballExplosion.mPIEffect;
			mPIEffect.ResetAnim();
			PILayer pilayer = null;
			for (int i = 1; i < 7; i++)
			{
				PILayer layer = mPIEffect.GetLayer(i);
				if (!b.GetIsCannon() && 6 - b.GetColorType() == i)
				{
					layer.SetVisible(true);
					pilayer = layer;
				}
				else
				{
					layer.SetVisible(false);
				}
			}
			if (b.GetIsCannon())
			{
				pilayer = mPIEffect.GetLayer("Cannon");
			}
			else
			{
				mPIEffect.GetLayer("Cannon").SetVisible(false);
			}
			if (range >= 0f)
			{
				float mNumberScale = 1f;
				PIEmitterInstance emitter = pilayer.GetEmitter("Large fragments");
				emitter.mEmitterInstanceDef.mValues[11].mValuePointVector[0].mValue = angle;
				emitter.mEmitterInstanceDef.mValues[12].mValuePointVector[0].mValue = range;
				emitter.mNumberScale = mNumberScale;
				emitter = pilayer.GetEmitter("Small fragments");
				emitter.mEmitterInstanceDef.mValues[11].mValuePointVector[0].mValue = angle;
				emitter.mEmitterInstanceDef.mValues[12].mValuePointVector[0].mValue = range;
				emitter.mNumberScale = mNumberScale;
			}
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0003C2FA File Offset: 0x0003A4FA
		public void AddBallExplosionParticleEffect(Ball b)
		{
			this.AddBallExplosionParticleEffect(b, 0f, -1f);
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0003C310 File Offset: 0x0003A510
		public bool Init(bool do_first_time_intro)
		{
			this.mIntroMidAlpha = new CurvedVal();
			this.mIntroMidScale = new CurvedVal();
			this.mIntroMidTransX = new CurvedVal();
			this.mIntroMapAlpha = new CurvedVal();
			this.mIntroMapPinAlpha = new CurvedVal();
			this.mIntroMapScale = new CurvedVal();
			this.mIntroMapTransX = new CurvedVal();
			this.mIntroRotate = new CurvedVal();
			this.mIntroFrogScale = new CurvedVal();
			if (this.mApp.mUserProfile != null)
			{
				GameApp.gDDS.ChangeProfile(this.mApp.mUserProfile);
			}
			this.mDoingFirstTimeIntro = do_first_time_intro;
			if (do_first_time_intro)
			{
				this.mDoingFirstTimeIntroZoomToGame = false;
				if (!this.mApp.mResourceManager.IsGroupLoaded("CloakedBoss") && !this.mApp.mResourceManager.LoadResources("CloakedBoss"))
				{
					this.mApp.ShowResourceError(true);
					this.mApp.Shutdown();
				}
				if (!this.mApp.mResourceManager.IsGroupLoaded("IntroScreen") && !this.mApp.mResourceManager.LoadResources("IntroScreen"))
				{
					this.mApp.ShowResourceError(true);
					this.mApp.Shutdown();
				}
				if (!this.mApp.mResourceManager.IsGroupLoaded("Map") && !this.mApp.mResourceManager.LoadResources("Map"))
				{
					this.mApp.ShowResourceError(true);
					this.mApp.Shutdown();
				}
				Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_LAME_CLOAKEDBOSS_CLAP);
				Image imageByID2 = Res.GetImageByID(ResID.IMAGE_LEVELS_INTROSCREEN_BKGRND);
				this.mCloakBossIntroAlpha = 0;
				Board.gIntroRibbitTimer = ZumasRevenge.Common._M(50);
				this.mIntroTimer = 0;
				this.mSmokePoof = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_CLOAKTOLAMEEXPLOSION01).Duplicate();
				this.mCloakedBossFrame = imageByID.mNumRows * imageByID.mNumCols;
				this.mIntroFadeAmt = 255f;
				this.mIntroBG = imageByID2;
				ZumasRevenge.Common.SetFXNumScale(this.mSmokePoof, 4f);
				this.mDoIntroFrogJump = false;
				this.mIntroDialog.Add(new SimpleFadeText(TextManager.getInstance().getString(111)));
				this.mIntroDialog.Add(new SimpleFadeText(TextManager.getInstance().getString(112)));
				this.mIntroDialog.Add(new SimpleFadeText(TextManager.getInstance().getString(113)));
				this.mFrogFlyOff = new FrogFlyOff();
				this.mFrogFlyOff.JumpOut(this.mFrog, int.MaxValue, int.MaxValue, ZumasRevenge.Common._SS(this.mApp.mWidth / 2), ZumasRevenge.Common._M(505), 3.1415927f);
				this.PlaySeagulls();
			}
			this.mHaloSwapCurve.SetConstant(0.0);
			this.mHaloSwapCurve.mAppUpdateCountSrc = this.mUpdateCnt;
			Image imageByID3 = Res.GetImageByID(ResID.IMAGE_GUI_GUIDE);
			this.mAimGuide = new DeviceImage();
			this.mAimGuide.mApp = this.mApp;
			this.mAimGuide.AddImageFlags(16U);
			this.mAimGuide.SetImageMode(true, true);
			this.mAimGuide.Create(this.mApp.GetScreenRect().mWidth, imageByID3.GetHeight());
			Graphics graphics = new Graphics(this.mAimGuide);
			for (int i = 0; i < this.mAimGuide.mWidth; i += imageByID3.GetWidth())
			{
				graphics.DrawImage(imageByID3, i, 0);
			}
			graphics.ClearRenderContext();
			return true;
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0003C66A File Offset: 0x0003A86A
		public bool Init()
		{
			return this.Init(false);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0003C674 File Offset: 0x0003A874
		public void AdventureModeSetupComplete(bool continued_game)
		{
			JeffLib.Common.StrFindNoCase(this.mLevel.mId, "debug");
			if (this.mGameState != GameState.GameState_BossDead)
			{
				this.SetMenuBtnEnabled(true);
				return;
			}
			this.mFrog.SetPos(ZumasRevenge.Common._M(396), ZumasRevenge.Common._M1(460));
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0003C6C7 File Offset: 0x0003A8C7
		public void MakeCachedBackground()
		{
			this.mCachedCurveImage = null;
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0003C6D0 File Offset: 0x0003A8D0
		public void PlaySeagulls()
		{
			int soundByID = Res.GetSoundByID(ResID.SOUND_SEAGULLS);
			if (this.mApp.mSoundPlayer.IsLooping(soundByID))
			{
				return;
			}
			SoundAttribs soundAttribs = new SoundAttribs();
			soundAttribs.fadeout = 0.1f;
			this.mApp.mSoundPlayer.Loop(soundByID, soundAttribs);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0003C71F File Offset: 0x0003A91F
		public bool ShouldBypassFinalSequenceOnLoad()
		{
			return this.mLevel.mFinalLevel && (this.mVortexAppear || this.mVortexBGAlpha >= 255f || this.mAdventureWinScreen) && this.mGameState == GameState.GameState_BossDead;
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0003C758 File Offset: 0x0003A958
		public void UpdatePlayingFX()
		{
			this.UpdateTreasureAnim();
			for (int i = 0; i < this.mText.size<BonusTextElement>(); i++)
			{
				this.mText[i].mBonus.Update();
				if (this.mText[i].mBonus.IsDone())
				{
					this.mText[i].mBonus = null;
					this.mText.RemoveAt(i);
					i--;
				}
			}
			for (int j = 0; j < this.mBallExplosions.size<BallExplosion>(); j++)
			{
				BallExplosion ballExplosion = this.mBallExplosions[j];
				if (ballExplosion.Update())
				{
					this.mBallExplosionsPool.Free(ballExplosion);
					this.mBallExplosions.RemoveAt(j);
					j--;
				}
			}
			for (int k = 0; k < this.mLazerBlasts.size<PIEffect>(); k++)
			{
				this.mLazerBlasts[k].Update();
				if (!this.mLazerBlasts[k].IsActive())
				{
					this.mLazerBlasts[k] = null;
					this.mLazerBlasts.RemoveAt(k);
					k--;
				}
			}
			this.mLazerBurn.Update();
			if (this.mFrog.LaserMode())
			{
				this.mLazerBeam[0].Update();
				this.mLazerBeam[1].Update();
				this.mLazerBurn.mEmitAfterTimeline = true;
			}
			else
			{
				this.mLazerBurn.mEmitAfterTimeline = false;
			}
			this.mFruitBounceEffect.Update();
			this.UpdateLevelCompleteText();
			if (!this.mPreventBallAdvancement)
			{
				for (int l = 0; l < 2; l++)
				{
					FwooshImage fwooshImage = this.mLevelNameText[l];
					if (fwooshImage != null && fwooshImage.mImage != null && fwooshImage.mAlpha != 0f)
					{
						fwooshImage.Update();
						if (l == 0 && (MathUtils._geq(fwooshImage.mAlpha, 255f, 0.01f) || fwooshImage.mIncText) && !fwooshImage.mIsDelaying)
						{
							break;
						}
					}
				}
			}
			for (int m = 0; m < this.mMultiplierBallEffects.size<MultiplierBallEffect>(); m++)
			{
				this.mMultiplierBallEffects[m].Update();
				if (this.mMultiplierBallEffects[m].Done())
				{
					this.mMultiplierBallEffects[m] = null;
					this.mMultiplierBallEffects.RemoveAt(m);
					m--;
				}
			}
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0003C9A0 File Offset: 0x0003ABA0
		public void UpdateLevelCompleteText()
		{
			if (this.mGameState != GameState.GameState_BeatLevelBonus || this.mLevelCompleteText.mImage == null)
			{
				return;
			}
			if (this.mLevelCompleteText.mAlpha <= 0f || (this.mLevelTransition != null && !this.mLevelTransition.IsDone()))
			{
				this.mLevelCompleteText.mImage = null;
				return;
			}
			this.mLevelCompleteText.Update();
			this.mLevelCompleteText.mX = this.mWidth / 2 - this.mApp.mBoardOffsetX;
			this.mLevelCompleteText.mY = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(600));
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0003CA40 File Offset: 0x0003AC40
		public void Reset(bool game_over, bool level_reset, bool first_time_init, bool delete_bg)
		{
			this.mCurveClearBonus = 0;
			this.ClearPIEffects();
			this.mPrevIFBestScore = this.mApp.mUserProfile.mIronFrogStats.mBestScore;
			this.mNewBallDelay[0] = (this.mNewBallDelay[1] = -1);
			this.mNeedsBossExtraLife = true;
			this.mDoingBossIntroText = false;
			this.mAdvStatsTime = 0;
			Board.gEndGauntletExtraTime = 0;
			Board.gMultTimeLeftDecAmt = 0;
			this.mGauntletPointsForDiffInc = 0;
			this.mGauntletMultTextFlashOn = true;
			this.mGauntletMultTextFlashTimer = 0;
			this.mGauntletMultTextVX = (this.mGauntletMultTextVY = 0f);
			this.mGauntletMultTextMoveLastFrame = 0;
			this.mDoMuMuMode = false;
			Buffer.SetByte(this.mMuMuMode, 0, 0);
			this.mGauntletModeOver = false;
			this.mEndGauntletTimer = 0;
			this.mGauntletPointsFromMult = 0;
			this.mHallucinateTimer = 0;
			this.mCurrentSatPct = 0f;
			this.mTreasureCel = 0;
			if (this.mApp.mProxBombManager != null)
			{
				this.mApp.mProxBombManager.Clear();
			}
			this.mIgnoreCount = 0;
			this.mMultiplierBallEffects.Clear();
			if (!level_reset)
			{
				this.mFrog = new Gun(this);
			}
			this.mText.Clear();
			this.mFruitExplodeEffect.Reset();
			this.mNumZumaBalls = 0;
			this.mCursorBlooms[0] = new CursorBloom();
			this.mCursorBlooms[1] = new CursorBloom();
			if (delete_bg)
			{
				this.mBackgroundImage = null;
			}
			this.mNumClearsInARow = (this.mCurInARowBonus = (this.mCurComboScore = (this.mNumCleared = (this.mCurComboCount = 0))));
			this.mIsEndless = false;
			this.mStateCount = 0;
			this.mScoreTarget = 0;
			this.mLastBallClickTick = (this.mLastSmallExplosionTick = 0U);
			this.mAccuracyCount = 0;
			this.mAccuracyBackupCount = 0;
			this.mFlashAlpha = 0;
			for (int i = 0; i < 6; i++)
			{
				this.mBallColorMap[i] = 0;
			}
			this.DeleteBullets();
			if (!first_time_init)
			{
				this.DoAccuracy(false);
			}
			if (game_over)
			{
				this.mGameStats.Reset();
				this.mLevel.mCurBarSize = 0;
				this.mFrog.EmptyBullets();
				if (this.GauntletMode() || this.IronFrogMode())
				{
					this.mScore = this.mLevelBeginScore;
				}
				this.mRollerScore.ForceScore(this.mScore);
			}
			else if (!level_reset)
			{
				this.mLevelNum++;
			}
			else
			{
				this.mFrog.LevelReset();
			}
			if (first_time_init)
			{
				this.mFruitMultiplier = 1;
				this.mScoreMultiplier = 1;
			}
			this.mForceTreasure = false;
			this.mLazerHitTreasure = false;
			this.mGameState = GameState.GameState_Playing;
			this.mLevelStats.Reset();
			this.mQRand = new QRand();
			this.mLevelBeginScore = this.mScore;
			ZumasRevenge.Common.gDieAtEnd = true;
			ZumasRevenge.Common.gSuckMode = false;
			ZumasRevenge.Common.gAddBalls = true;
			this.mLevelEndFrame = 0;
			if (!first_time_init)
			{
				this.UpdateGunPos(true);
			}
			this.mIsWinning = false;
			this.mCurTreasure = null;
			this.mTreasureEndFrame = 0;
			this.mDestroyCount = 0;
			this.mLastExplosionTick = 0U;
			this.mDestroyAll = true;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0003CD6A File Offset: 0x0003AF6A
		public void Reset(bool game_over, bool level_reset)
		{
			this.Reset(game_over, level_reset, false, true);
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0003CD78 File Offset: 0x0003AF78
		public void Pause(bool pause, bool becauseOfDialog)
		{
			if (this.mShowMapScreen && !this.mMapScreen.mClosing && !this.mMapScreen.mIntroClosing)
			{
				return;
			}
			if (pause)
			{
				if (this.mPauseCount == 0)
				{
					this.mPauseFade = 0;
				}
				if (becauseOfDialog)
				{
					this.mDialogCount++;
				}
				this.mPauseCount++;
				if (this.mPauseCount == 1)
				{
					this.mApp.mSoundPlayer.PauseLoopingSounds(true);
				}
			}
			else
			{
				if (becauseOfDialog)
				{
					this.mDialogCount--;
				}
				this.mPauseCount--;
				if (this.mPauseCount < 0)
				{
					this.mPauseCount = 0;
				}
				if (this.mDialogCount < 0)
				{
					this.mDialogCount = 0;
				}
				this.mUnpauseFrame = this.mUpdateCnt;
				if (this.mPauseCount == 0 && this.mDialogCount == 0)
				{
					this.mApp.mSoundPlayer.PauseLoopingSounds(false);
				}
			}
			if (this.mPauseCount != 0)
			{
				this.mShowBallsDuringPause = false;
				this.mPauseUpdateCnt = this.mUpdateCnt;
				this.MouseMove(this.mApp.mWidgetManager.mLastMouseX, this.mApp.mWidgetManager.mLastMouseY);
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0003CEA2 File Offset: 0x0003B0A2
		public void Pause(bool pause)
		{
			this.Pause(pause, false);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0003CEAC File Offset: 0x0003B0AC
		public void LoadLevelBkg(Level theLevel, bool bg_was_from_psd, string psd_bg_id)
		{
			string theName = "IMAGE_LEVELS_" + theLevel.mId.ToUpper() + "_BKGRND";
			if (!bg_was_from_psd)
			{
				this.mBackgroundImage = null;
			}
			if (!theLevel.mBGFromPSD)
			{
				this.mBackgroundImage = this.mApp.mResourceManager.LoadImage(this.mApp.mResourceManager.GetIdByPath(theLevel.mImagePath)).GetImage();
				return;
			}
			this.mBackgroundImage = this.mApp.mResourceManager.LoadImage(theName).GetImage();
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0003CF34 File Offset: 0x0003B134
		public bool StartLevel(string level_id, bool from_load, bool from_checkpoint, bool zone_restart, Level copy_effects_from)
		{
			this.mCurveClearBonus = 0;
			if (!this.mApp.mResourceManager.IsGroupLoaded("GamePlay") && !this.mApp.mResourceManager.LoadResources("GamePlay"))
			{
				this.mApp.ShowResourceError(true);
				this.mApp.Shutdown();
				return false;
			}
			if (this.prevLevelID.Length != 0 && this.prevLevelID != level_id)
			{
				string theGroup = "Levels_" + this.prevLevelID.ToUpper()[0] + this.prevLevelID.Substring(1);
				if (this.mApp.mResourceManager.IsGroupLoaded(theGroup))
				{
					this.mApp.mResourceManager.DeleteResources(theGroup);
				}
			}
			this.prevLevelID = level_id;
			string theGroup2 = "Levels_" + level_id.ToUpper()[0] + level_id.Substring(1);
			if (!this.mApp.mResourceManager.IsGroupLoaded(theGroup2))
			{
				this.mApp.mResourceManager.LoadResources(theGroup2);
			}
			bool flag = !from_checkpoint && !zone_restart && !Board.gCheatReload && this.mLevel != null && ZumasRevenge.Common.StrEquals(this.mLevel.mId, level_id);
			if (flag)
			{
				this.mLevel.ReInit();
			}
			this.mNeedsBossExtraLife = true;
			this.RemoveWidget(this.mStatsContinueBtn);
			this.mApp.SafeDeleteWidget(this.mStatsContinueBtn);
			this.mStatsContinueBtn = null;
			if (this.mLevelCompleteText.mImage != null)
			{
				this.mLevelCompleteText.mImage.Dispose();
				this.mLevelCompleteText.mImage = null;
			}
			this.mPlayThud = true;
			this.mChallengeCupUnlockedFX = null;
			this.mCurTreasure = null;
			this.mCurTreasureNum = 0;
			this.mLastIntroPadDelay = 99999;
			this.mIntroPadHopCount = 0;
			this.mLastIntroPad = 0;
			for (int i = 0; i < this.mEndLevelExplosions.Count; i++)
			{
				this.mEndLevelExplosionPool.Free(this.mEndLevelExplosions[i]);
			}
			this.mEndLevelExplosions.Clear();
			this.mEffectBatch.Clear();
			this.mVortexFrogRadiusExpand = true;
			this.mVortexFrogRadius = 0f;
			this.mVortexFrogAngle = 0f;
			this.mVortexFrogScale = 1f;
			this.mHasSeenCheckpointIntro = false;
			bool bg_was_from_psd = false;
			string psd_bg_id = "";
			if (this.mLevel != null && !flag && this.mLevel.mBGFromPSD)
			{
				bg_was_from_psd = true;
				psd_bg_id = "IMAGE_LEVELS_" + this.mLevel.mId.ToUpper() + "_BKGRND";
			}
			int num = ((this.mLevel == null) ? (-1) : this.mLevel.mEndSequence);
			if (this.mLevel != copy_effects_from && !flag)
			{
				this.mLevel.Dispose();
				this.mLevel = null;
			}
			for (int j = 0; j < this.mPowerEffects.size<PowerEffect>(); j++)
			{
				this.mPowerEffects[j] = null;
			}
			this.mPowerEffects.Clear();
			this.mQRand.Clear();
			bool flag2 = false;
			if (this.mNextLevel != null)
			{
				this.mLevel = this.mNextLevel;
				this.mNextLevel = null;
			}
			else if (!flag && !this.mApp.GetLevelMgr().GetLevelById(level_id, ref this.mLevel, this))
			{
				copy_effects_from = null;
				return false;
			}
			this.mLevel.mBoard = this;
			this.mLevel.mApp = this.mApp;
			string text = this.mApp.GetLevelMgr().GetZoneFruitId(this.mLevel.mZone).ToUpper();
			this.mFruitImg = this.mApp.mResourceManager.LoadImage("IMAGE_FRUIT_" + text).GetImage();
			SharedImageRef sharedImageRef = this.mApp.mResourceManager.LoadImage("IMAGE_FRUIT_" + text + "_GLOW");
			this.mFruitGlow = ((sharedImageRef != null) ? sharedImageRef.GetImage() : null);
			if (this.mDarkFrogSequence == null)
			{
				this.mMenuButton.SetDisabled(false);
			}
			if (this.mSwapBallButton != null)
			{
				this.mSwapBallButton.SetDisabled(false);
			}
			GameApp.gDDS.SetGauntletTime(0);
			GameApp.gDDS.SetGauntletPoints(0);
			GameApp.gDDS.StartLevel(this.mLevel);
			this.SetSuckMode(this.mLevel.mSuckMode);
			if (this.mLevel.mZone == 6 && (this.mLevel.IsFinalBossLevel() || this.mLevel.mEndSequence != -1 || this.mLevel.mBoss != null))
			{
				if (!this.mApp.mResourceManager.IsGroupLoaded("Boss6Common") && !this.mApp.mResourceManager.LoadResources("Boss6Common"))
				{
					this.mApp.ShowResourceError(true);
					this.mApp.Shutdown();
				}
				if (!this.mApp.mResourceManager.IsGroupLoaded("Bosses") && !this.mApp.mResourceManager.LoadResources("Bosses"))
				{
					this.mApp.ShowResourceError(true);
					this.mApp.Shutdown();
				}
				if (this.mLevel.mEndSequence <= 2 && !this.mApp.mResourceManager.IsGroupLoaded("CloakedBoss") && !this.mApp.mResourceManager.LoadResources("CloakedBoss"))
				{
					this.mApp.ShowResourceError(true);
					this.mApp.Shutdown();
				}
			}
			if (!flag && !flag2 && (num != 3 || this.mLevel.mEndSequence != 4))
			{
				this.LoadLevelBkg(this.mLevel, bg_was_from_psd, psd_bg_id);
			}
			if (!flag && !flag2)
			{
				this.EraseTunnels();
				this.SetupTunnels(this.mLevel);
			}
			this.mLevel.StartLevel(from_load, flag);
			if (this.mLevel.mZone == 6 && this.mLevel.mEndSequence == 3)
			{
				this.MakeBoss6VolcanoMeltComp();
			}
			this.mLevelBeginning = true;
			int num2 = 0;
			for (int k = 0; k < this.mLevel.mNumCurves; k++)
			{
				if (GameApp.gDDS.GetZumaScore(k) > num2)
				{
					num2 = GameApp.gDDS.GetZumaScore(k);
				}
				this.mDestroyAll = this.mLevel.mCurveMgr[k].mCurveDesc.mVals.mDestroyAll;
				if (ZumasRevenge.Common.gSuckMode)
				{
					this.mLevel.mCurveMgr[k].mCurveDesc.mVals.mPowerUpFreq[2] = 0;
				}
			}
			if (this.GauntletMode())
			{
				this.mScoreTarget = int.MaxValue;
			}
			else if (num2 > 0)
			{
				this.mScoreTarget = this.mScore + num2;
			}
			else
			{
				this.mScoreTarget = 0;
			}
			this.MakeCachedBackground();
			if (this.mLevel.mBoss != null)
			{
				this.mIsEndless = true;
				if (this.mLevel.mBoss.mResGroup != "Boss6_DarkFrog")
				{
					this.mApp.PlaySong(127);
				}
			}
			else
			{
				this.mIsEndless = this.mLevel.mIsEndless;
			}
			if (!this.mIsRestarting)
			{
				this.UpdateGunPos(true);
			}
			this.mLevel.SetFrog(this.mFrog);
			this.SetupLevelText();
			if (this.mLevel.mBoss == null)
			{
				if (!this.mApp.mUserProfile.m_AchievementMgr.isAchievementUnlocked(EAchievementType.FROZEN_FROG) && (this.mLevel.mMoveType == 1 || this.mLevel.mMoveType == 2))
				{
					this.mLevel.m_canGetAchievementNoMove = true;
					this.mLevel.m_OriginX = this.mLevel.mFrog.mDestX2;
					this.mLevel.m_OriginY = this.mLevel.mFrog.mDestY2;
				}
				else
				{
					this.mLevel.m_canGetAchievementNoMove = false;
				}
				if (!this.mApp.mUserProfile.m_AchievementMgr.isAchievementUnlocked(EAchievementType.FROG_STATUE) && this.mLevel.mNumFrogPoints > 1 && this.mLevel.CanRotateFrog())
				{
					this.mLevel.m_canGetAchievementNoJump = true;
				}
				else
				{
					this.mLevel.m_canGetAchievementNoJump = false;
				}
			}
			int mNum = this.mLevel.mNum;
			int mZone = this.mLevel.mZone;
			int num3 = (this.mLevel.mNum - this.mLevel.mZone) % 10;
			if (this.GauntletMode() && this.mLevel != null)
			{
				List<GauntletHSInfo> list = new List<GauntletHSInfo>();
				this.mApp.mUserProfile.GetGauntletHighScores((this.mLevel.mZone - 1) * 10 + this.mLevel.mNum, ref list);
				if (list.size<GauntletHSInfo>() == 0)
				{
					this.mGauntletHSTarget = 0;
				}
				else
				{
					this.mGauntletHSTarget = list[0].mScore;
				}
			}
			if (!this.mIsLoading)
			{
				this.GetBetaStats().LevelStarted(this.mLevel.mId, this.mLevel.mZone, this.mLevel.mNum, from_checkpoint, zone_restart);
			}
			if (this.mAdventureMode)
			{
				this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvLevel = this.mLevel.mNum;
				this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvZone = this.mLevel.mZone;
				if (this.mLevel.mNum != 2147483647 && !this.mLevel.IsFinalBossLevel())
				{
					int num4 = (this.mLevel.mZone - 1) * 10 + this.mLevel.mNum;
					AdvModeTempleStats advModeTempleStats = this.GetAdvModeTempleStats();
					if (num4 > advModeTempleStats.mHighestLevel)
					{
						advModeTempleStats.mHighestLevel = num4;
					}
				}
			}
			else if (this.IronFrogMode())
			{
				if (this.mLevel.mNum > this.mApp.mUserProfile.mIronFrogStats.mHighestLevel)
				{
					this.mApp.mUserProfile.mIronFrogStats.mHighestLevel = this.mLevel.mNum;
				}
				if (this.mLevel.mNum == 1)
				{
					this.mApp.mUserProfile.mIronFrogStats.mNumAttempts++;
				}
			}
			else if (this.GauntletMode())
			{
				this.mApp.mUserProfile.mChallengeStats.mNumTimesPlayedCurve[(this.mLevel.mZone - 1) * 10 + this.mLevel.mNum - 1]++;
			}
			if (!this.GauntletMode() && !this.IronFrogMode() && this.IsCheckpointLevel() && this.mLevel.mNum != 2147483647 && this.mLives < 3)
			{
				this.mPreCheckpointLives = this.mLives;
				this.mLives = 3;
			}
			else
			{
				this.mPreCheckpointLives = this.mLives;
			}
			if (!this.IronFrogMode() && !this.GauntletMode())
			{
				if (this.mLevel.mNum == 10)
				{
					if (!this.mApp.mResourceManager.IsGroupLoaded("BossIntro"))
					{
						this.mApp.mResourceManager.LoadResources("BossIntro");
					}
				}
				else if (this.mLevel.mNum != 2147483647 && this.mApp.mResourceManager.IsGroupLoaded("BossIntro"))
				{
					this.mApp.mResourceManager.DeleteResources("BossIntro");
				}
			}
			else if (this.IronFrogMode() && this.mLevel.mNum == 10)
			{
				this.mApp.mResourceManager.LoadResources("IronFrogWin");
			}
			if (this.IronFrogMode() && this.mApp.mUserProfile.mChallengeUnlockState[this.mLevel.mZone - 1, this.mLevel.mNum - 1] < 2)
			{
				this.mApp.mUserProfile.mChallengeUnlockState[this.mLevel.mZone - 1, this.mLevel.mNum - 1] = 2;
			}
			this.mApp.mUserProfile.GetAdvModeVars().mFirstTimeInZone[this.mLevel.mZone - 1] = false;
			this.PositionMenuButton();
			if (!ZumasRevenge.Common.BossLevel(this.mLevel) && this.mApp.mResourceManager.IsGroupLoaded("Bosses"))
			{
				this.mApp.mResourceManager.DeleteResources("Bosses");
			}
			GC.Collect();
			return true;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0003DB5D File Offset: 0x0003BD5D
		public bool StartLevel(string level_id)
		{
			return this.StartLevel(level_id, false, false, false, null);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0003DB6C File Offset: 0x0003BD6C
		public bool StartLevel(int level_num, bool from_load, bool from_checkpoint, bool zone_restart)
		{
			string levelId = this.mApp.GetLevelMgr().GetLevelId(level_num - 1);
			if (levelId.Length == 0)
			{
				return false;
			}
			this.mLevelNum = level_num;
			this.mTheNextLevel = this.mLevelNum + 1;
			return this.StartLevel(levelId, from_load, from_checkpoint, zone_restart, null);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0003DBB8 File Offset: 0x0003BDB8
		public bool StartLevel(int level_num)
		{
			return this.StartLevel(level_num, false, false, false);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0003DBC4 File Offset: 0x0003BDC4
		public bool RestartFromZone(int zone)
		{
			this.mApp.mUserProfile.GetAdvModeVars().mCheckpointScores[zone - 1].mBoss = (this.mApp.mUserProfile.GetAdvModeVars().mCheckpointScores[zone - 1].mMidpoint = (this.mApp.mUserProfile.GetAdvModeVars().mCheckpointScores[zone - 1].mZoneStart = 0));
			if (this.mApp.IsHardMode())
			{
				this.mApp.mUserProfile.mFirstTimeReplayingHardMode = false;
			}
			else
			{
				this.mApp.mUserProfile.mFirstTimeReplayingNormalMode = false;
			}
			Board.gNeedsGauntletHSSound = true;
			this.Reset(false, true, true, true);
			this.mLives = 3;
			this.mScore = 0;
			this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvScore = 0;
			this.mApp.mUserProfile.GetAdvModeVars().mNumDeathsCurLevel = 0;
			this.mApp.mUserProfile.GetAdvModeVars().mNumZumasCurLevel = 0;
			this.mApp.mUserProfile.GetAdvModeVars().mDDSTier = (this.mApp.mUserProfile.GetAdvModeVars().mRestartDDSTier = -1);
			this.mPointsLeftForExtraLife = this.mApp.GetLevelMgr().mPointsForLife;
			this.mLevelBeginScore = this.mScore;
			this.mRollerScore.ForceScore(this.mScore);
			bool result = this.StartLevel(this.mApp.GetLevelMgr().GetZoneStartId(zone), false, false, true, null);
			this.mLevelNum = this.mApp.GetLevelMgr().GetLevelIndex(this.mLevel.mId) + 1;
			this.UpdateGunPos(true);
			this.MakeCachedBackground();
			this.mFrog.DeleteBullet();
			this.mFrog.DeleteBullet();
			this.mHasDoneIntroSounds = false;
			this.mTheNextLevel = this.mLevelNum + 1;
			return result;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0003DD9C File Offset: 0x0003BF9C
		public void PositionMenuButton()
		{
			int num = 0;
			if (this.mApp.IsWideScreen())
			{
				if (!this.GauntletMode())
				{
					num = ZumasRevenge.Common._DS(-40);
				}
				else
				{
					num = 42;
				}
			}
			else if (this.GauntletMode())
			{
				num = ZumasRevenge.Common._DS(23);
			}
			this.mMenuButton.Move(this.mApp.GetWideScreenAdjusted(this.mMenuButtonX + num), 0);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0003DE00 File Offset: 0x0003C000
		public void DoShutdownSaveGame()
		{
			if (this.mSkipShutdownSave)
			{
				return;
			}
			if (!this.IronFrogMode() && !this.GauntletMode())
			{
				if (this.mLevel.mBoss != null && this.mLevel.mZone == 1)
				{
					BossTiger bossTiger = (BossTiger)this.mLevel.mBoss;
					if (bossTiger != null && bossTiger.ShouldEraseBullets())
					{
						this.DeleteBullets();
					}
				}
				if (this.mLevel != null)
				{
					this.mApp.mUserProfile.GetAdvModeVars().mFirstTimeInZone[this.mLevel.mZone - 1] = false;
				}
				this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvScore = this.mRollerScore.GetTargetScore();
				this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvLives = this.mLives;
				this.SaveGame(this.mApp.mUserProfile.GetSaveGameName(this.mIsHardMode), null);
				if (GameApp.USE_XBOX_SERVICE && !GameApp.USE_TRIAL_VERSION)
				{
					SignedInGamer signedInGamer = Gamer.SignedInGamers[0];
					LeaderboardIdentity leaderboardIdentity = LeaderboardIdentity.Create(0, 0);
					LeaderboardEntry leaderboard = signedInGamer.LeaderboardWriter.GetLeaderboard(leaderboardIdentity);
					leaderboard.Rating = (long)this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvScore;
					return;
				}
			}
			else
			{
				this.mApp.mUserProfile.SaveDetails();
			}
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0003DF4C File Offset: 0x0003C14C
		public void UpdateGunPos(bool level_begin, int _x, int _y)
		{
			if (this.mLevel.mBoss != null && this.mLevel.mBoss.mAlphaOverride <= 254f)
			{
				return;
			}
			if (this.mControlMode == Board.CONTROL_MODE.CONTROL_MODE_SWAPPING)
			{
				return;
			}
			if (level_begin)
			{
				int mCurFrogPoint = this.mLevel.mCurFrogPoint;
				if (this.mLevel.mMoveType == 0)
				{
					this.mFrog.SetPos(this.mLevel.mFrogX[mCurFrogPoint], this.mLevel.mFrogY[mCurFrogPoint]);
				}
				else if (this.mLevel.mMoveType == 1)
				{
					this.mFrog.SetPos(this.mLevel.mFrogX[mCurFrogPoint] + this.mLevel.mBarWidth / 2, this.mLevel.mFrogY[mCurFrogPoint]);
				}
				else
				{
					this.mFrog.SetPos(this.mLevel.mFrogX[mCurFrogPoint], this.mLevel.mFrogY[mCurFrogPoint] + this.mLevel.mBarHeight / 2);
				}
				if (this.mLevelTransition != null)
				{
					this.mLevelTransition.RehupFrogPosition();
				}
			}
			int num = ((this.mLevel.mBoss == null) ? (-1) : ((BossShoot)this.mLevel.mBoss).mMovementMode);
			int num2 = ((_x == -1) ? ZumasRevenge.Common._SS(this.mApp.mWidgetManager.mLastMouseX) : _x);
			int num3 = ((_y == -1) ? ZumasRevenge.Common._SS(this.mApp.mWidgetManager.mLastMouseY) : _y);
			int num4 = this.mFrog.GetCenterX() + this.mApp.mBoardOffsetX;
			int centerY = this.mFrog.GetCenterY();
			IGamepad gamepad = this.mApp.mGamepadDriver.GetGamepad(this.mApp.mUserProfile.GetGamepadIndex());
			if (gamepad != null && gamepad.IsConnected())
			{
				if (this.mLevel.mMoveType == 0)
				{
					num2 = (int)((float)num4 + gamepad.GetAxisXPosition() * 256f);
					num3 = (int)((float)centerY + gamepad.GetAxisYPosition() * -256f);
				}
				else if (this.mLevel.mMoveType == 1)
				{
					Board.GamepadControls gamepadControls = this.mGamepadControls;
					gamepadControls.axis = new SexyVector2(gamepad.GetAxisXPosition(), gamepad.GetAxisYPosition());
					float num5 = gamepadControls.axis.Dot(gamepadControls.lastAxis);
					if (Math.Abs(gamepadControls.axis.x) <= 0.2f || num5 < 0f)
					{
						gamepadControls.accel = 1f;
					}
					else
					{
						gamepadControls.accel += gamepadControls.accel * 0.05f;
					}
					gamepadControls.accel = Math.Min((float)Board.GamepadControls.ACCEL_MAX, gamepadControls.accel);
					if (Math.Abs(gamepadControls.axis.y) <= 0.8f)
					{
						gamepadControls.axis.y = 1f;
					}
					num2 = (int)((float)num4 + gamepadControls.axis.x * gamepadControls.accel * 2f);
					num3 = (int)((float)centerY - gamepadControls.axis.y * 2f);
					gamepadControls.lastAxis = gamepadControls.axis;
				}
			}
			int num6 = num2 - num4;
			int num7 = centerY - num3;
			float num8 = (float)Math.Atan2((double)num7, (double)num6) + 1.570795f;
			if (this.mLevel.mInvertMouseTimer > 0)
			{
				num8 *= -1f;
			}
			bool flag = !this.mLevel.mNoFlip;
			if ((this.mLevel.mBoss != null && this.LevelIsSkeletonBoss() && this.mFrog.GetBullet() != null && this.mFrog.GetBullet().GetIsCannon()) || (this.mFrog.IsStunned() && !this.mFrog.StunnedFromBoss6()))
			{
				flag = false;
			}
			if (this.mLevel.mBoss != null && this.mLevel.mZone == 2 && this.mFrog.IsPoisoned())
			{
				flag = false;
			}
			if (this.mLevel.mBoss != null && (this.mLevel.mZone == 4 || this.mLevel.mZone == 6) && this.mFrog.IsSlow())
			{
				flag = false;
			}
			if ((this.mGameState != GameState.GameState_Playing && this.mGameState != GameState.GameState_LevelBegin && this.mGameState != GameState.GameState_Boss6Transition && this.mGameState != GameState.GameState_BossDead) || this.mLevel.mMoveType == 0)
			{
				this.mFrog.SetDestAngle(num8);
			}
			else if (this.mLevel.mMoveType == 1)
			{
				int num9 = this.mLevel.mFrogX[0];
				int num10 = num9 + this.mLevel.mBarWidth;
				num2 -= this.mApp.mBoardOffsetX;
				if (flag)
				{
					if (this.mLevel.mSliderEdgeRotate && num9 >= 0 && ((num2 < num9 && num4 < num9 + 10) || (num2 > num10 && num4 > num10 - 10)))
					{
						this.mFrog.SetDestAngle(num8);
					}
					else if (num3 < centerY)
					{
						this.mFrog.SetDestAngle(-3.14159f);
					}
					else
					{
						this.mFrog.SetDestAngle(0f);
					}
				}
				if (num9 >= 0)
				{
					if (num2 < num9)
					{
						num2 = num9;
					}
					else if (num2 > num10)
					{
						num2 = num10;
					}
				}
				int num11 = this.mLevel.mFrogY[this.mLevel.mCurFrogPoint];
				float destX = (float)num2;
				float num12 = (float)num2;
				if (this.mLevel.mInvertMouseTimer > 0 || num == 2)
				{
					float num13 = (num12 - (float)num9) / (float)this.mLevel.mBarWidth;
					num12 = (1f - num13) * (float)this.mLevel.mBarWidth + (float)num9;
					if (num9 >= 0)
					{
						if (num12 < (float)num9)
						{
							num12 = (float)num9;
						}
						else if (num12 > (float)num10)
						{
							num12 = (float)num10;
						}
					}
					if (num == 2)
					{
						destX = num12;
					}
					if (this.mLevel.mInvertMouseTimer > 0)
					{
						num2 = (int)num12;
					}
				}
				if (_x == -1 || _y == -1)
				{
					this.mFrog.SetDestPos(num2, num11, (int)((float)this.mLevel.mMoveSpeed * Board.SLIDER_FROG_DEVICE_SPEEDUP));
					if (this.mLevel.m_canGetAchievementNoMove && Math.Abs(num2 - this.mLevel.m_OriginX) > 0)
					{
						this.mLevel.m_canGetAchievementNoMove = false;
					}
				}
				else
				{
					this.mFrog.SetPos(num2, num11);
				}
				if (num != -1)
				{
					((BossShoot)this.mLevel.mBoss).SetDestX(destX);
				}
			}
			else if (this.mLevel.mMoveType == 2)
			{
				int num14 = this.mLevel.mFrogY[0];
				int num15 = num14 + this.mLevel.mBarHeight;
				if (flag)
				{
					if (this.mLevel.mSliderEdgeRotate && num14 >= 0 && ((num3 < num14 && centerY < num14 + 10) || (num3 > num15 && centerY > num15 - 10)))
					{
						this.mFrog.SetDestAngle(num8);
					}
					else if (num2 < num4)
					{
						this.mFrog.SetDestAngle(-1.570795f);
					}
					else
					{
						this.mFrog.SetDestAngle(1.570795f);
					}
				}
				if (num14 >= 0)
				{
					if (num3 < num14)
					{
						num3 = num14;
					}
					else if (num3 > num15)
					{
						num3 = num15;
					}
				}
				if (_x == -1 || _y == -1)
				{
					this.mFrog.SetDestPos(this.mLevel.mFrogX[this.mLevel.mCurFrogPoint], num3, (int)((float)this.mLevel.mMoveSpeed * Board.SLIDER_FROG_DEVICE_SPEEDUP));
					if (this.mLevel.m_canGetAchievementNoMove && Math.Abs(num3 - this.mLevel.m_OriginY) > 0)
					{
						this.mLevel.m_canGetAchievementNoMove = false;
					}
				}
				else
				{
					this.mFrog.SetPos(this.mLevel.mFrogX[this.mLevel.mCurFrogPoint], num3);
				}
			}
			this.mRecalcLazerGuide = (this.mRecalcGuide = true);
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0003E6DA File Offset: 0x0003C8DA
		public void UpdateGunPos(bool level_begin)
		{
			this.UpdateGunPos(level_begin, -1, -1);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0003E6E5 File Offset: 0x0003C8E5
		public void UpdateGunPos()
		{
			this.UpdateGunPos(false, -1, -1);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0003E6F0 File Offset: 0x0003C8F0
		public void GauntletMultiplierEnded()
		{
			int num = ((this.mGameState == GameState.GameState_Losing || this.mEndGauntletTimer > 0) ? ZumasRevenge.Common._M(75) : ZumasRevenge.Common._M1(1));
			this.mGauntletMultTextFlashOn = true;
			this.mGauntletMultTextFlashTimer = num;
			this.mGauntletLastFrogX = ZumasRevenge.Common._S(this.mFrog.GetCenterX());
			this.mGauntletLastFrogY = ZumasRevenge.Common._S(this.mFrog.GetCenterY());
			int num2 = ZumasRevenge.Common._M(35);
			this.mGauntletMultTextMoveLastFrame = this.mStateCount + num + num2;
			this.mGauntletMultTextVX = (float)((ZumasRevenge.Common._DS(ZumasRevenge.Common._M(740)) - this.mGauntletLastFrogX) / num2);
			this.mGauntletMultTextVY = (float)((ZumasRevenge.Common._DS(ZumasRevenge.Common._M(0)) - this.mGauntletLastFrogY) / num2);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0003E7AC File Offset: 0x0003C9AC
		public void DeleteBullets()
		{
			foreach (Bullet bullet in this.mBulletList)
			{
				bullet.Dispose();
			}
			this.mBulletList.Clear();
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0003E7E8 File Offset: 0x0003C9E8
		public void ClearFirstIntroForBack()
		{
			if (!this.mDoingFirstTimeIntro)
			{
				return;
			}
			if (this.mSmokePoof != null)
			{
				this.mSmokePoof.Dispose();
				this.mSmokePoof = null;
			}
			this.mIntroDialog.Clear();
			if (this.mFrogFlyOff != null)
			{
				this.mFrogFlyOff.Dispose();
				this.mFrogFlyOff = null;
			}
			GameApp.gApp.ShowMainMenu();
			GameApp.gApp.mBoard.mSkipShutdownSave = true;
			this.mWidgetManager.RemoveWidget(GameApp.gApp.mBoard);
			GameApp.gApp.SafeDeleteWidget(GameApp.gApp.mBoard);
			GameApp.gApp.mBoard = null;
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0003E88C File Offset: 0x0003CA8C
		public override void Update()
		{
			if (GameApp.gApp.IsHardwareBackButtonPressed())
			{
				this.ProcessHardwareBackButton();
			}
			IGamepad gamepad = this.mApp.mGamepadDriver.GetGamepad(this.mApp.mUserProfile.GetGamepadIndex());
			if (gamepad != null && gamepad.IsConnected())
			{
				SexyVector2 sexyVector = new SexyVector2(gamepad.GetAxisXPosition(), gamepad.GetAxisYPosition());
				if (sexyVector.Magnitude() < 0.2f)
				{
					this.mGamepadControls.accel = 1f;
				}
			}
			if (this.mApp.mDoingDRM || this.mApp.mUpsell != null)
			{
				return;
			}
			base.Update();
			this.UpdateExtraLivesInfo();
			if (this.mApp.mMapScreen != null && !this.mApp.mMapScreen.mDirty)
			{
				return;
			}
			if (this.mNumDrawFramesLeft > 0)
			{
				this.MarkDirty();
				return;
			}
			if (this.mReturnToMainMenu)
			{
				this.mReturnToMainMenu = false;
				if (this.mLevel.mBoss == null && this.isResultPageInAdvMode())
				{
					this.mForceToNextLevelInAdvMode = true;
					if (this.mTheNextLevel > this.mLevelNum)
					{
						this.mLevelNum++;
					}
				}
				else if (this.mGameState == GameState.GameState_BossIntro)
				{
					this.mForceToNextLevelInAdvMode = true;
					if (this.mTheNextLevel > this.mLevelNum)
					{
						this.mLevelNum++;
					}
				}
				this.mApp.EndCurrentGame();
				this.mForceToNextLevelInAdvMode = false;
				if (!this.IronFrogMode())
				{
					if (this.mApp.mResourceManager.IsGroupLoaded("Bosses"))
					{
						this.mApp.mResourceManager.DeleteResources("Bosses");
					}
					if (this.mApp.mResourceManager.IsGroupLoaded("CommonBoss"))
					{
						this.mApp.mResourceManager.DeleteResources("CommonBoss");
					}
					if (this.mApp.mResourceManager.IsGroupLoaded("CloakedBoss"))
					{
						this.mApp.mResourceManager.DeleteResources("CloakedBoss");
					}
					this.mApp.ShowMainMenu();
					return;
				}
				this.mApp.ShowIronFrog();
				return;
			}
			else
			{
				this.mRollingInDangerZone = false;
				if (Board.gHideBalls)
				{
					this.MarkDirty();
					return;
				}
				if (this.mChallengeHelp != null || this.mApp.mGenericHelp != null || this.mApp.mDialogMap.ContainsKey(2))
				{
					return;
				}
				if (this.mZumaTips.size<ZumaTip>() > 0)
				{
					this.mZumaTips[0].Update();
				}
				if (this.mZumaTips.size<ZumaTip>() > 0 && this.mZumaTips[0].mBlockUpdates)
				{
					return;
				}
				if (this.mPauseCount > 0)
				{
					if (this.mPauseFade <= 50)
					{
						this.mPauseFade++;
						this.MarkDirty();
					}
					if (this.mNumPauseUpdatesToDo > 0)
					{
						this.MarkDirty();
						this.mNumPauseUpdatesToDo--;
					}
					return;
				}
				if (this.mTransitionScreenImage != null && !this.mTransitionScreenHolePct.IncInVal())
				{
					this.mTransitionScreenImage = null;
				}
				if (!this.mTransitionScreenScale.IncInVal())
				{
					if (this.mDoingTransition)
					{
						this.mFrog.mDestCount = 0;
						this.mFrog.ForceX(this.mFrog.GetCurX());
						this.mFrog.ForceY(this.mFrog.GetCurY());
						if (this.mLevel.mMoveType == 1)
						{
							this.mFrog.SetDestAngle(-3.14159f);
						}
					}
					this.mDoingTransition = false;
				}
				this.mIntroMidAlpha.IncInVal();
				this.mTransitionFrogRotPct.IncInVal();
				if (this.mTransitionFrogRotPct.CheckInThreshold((double)ZumasRevenge.Common._M(0.35f)))
				{
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_FROG_MENU_LEAP));
				}
				this.mBossIntroBGAlpha.IncInVal();
				this.mStatsBubbleScale.IncInVal();
				if (this.mDoingFirstTimeIntro)
				{
					if (this.mShowMapScreen)
					{
						this.mMapScreen.Update();
					}
					this.MarkDirty();
					if (this.mIntroFadeAmt > 0f && !this.mDoIntroFrogJump)
					{
						if (this.mApp.mNewUserDlg == null || this.mIntroFadeAmt >= (float)ZumasRevenge.Common._M(180))
						{
							this.mIntroFadeAmt -= ZumasRevenge.Common._M(1f);
							return;
						}
					}
					else if (this.mSmokePoof != null)
					{
						int num = 175;
						this.mSmokePoof.mDrawTransform.LoadIdentity();
						float num2 = GameApp.DownScaleNum(1f);
						this.mSmokePoof.mDrawTransform.Scale(num2, num2);
						this.mSmokePoof.mDrawTransform.Translate((float)(this.mWidth / 2 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(0))), (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(180)));
						this.mSmokePoof.Update();
						if (this.mSmokePoof.mFrameNum >= (float)ZumasRevenge.Common._M(108))
						{
							this.mCloakBossIntroAlpha += 2;
						}
						if (this.mCloakBossIntroAlpha > 255)
						{
							this.mCloakBossIntroAlpha = 255;
						}
						if (MathUtils._eq(this.mSmokePoof.mFrameNum, (float)ZumasRevenge.Common._M(80), 0.1f))
						{
							this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_SHADOW_GUY_APPEARS));
						}
						if (this.mSmokePoof.mFrameNum >= (float)num && this.mUpdateCnt % ZumasRevenge.Common._M(6) == 0 && this.mCloakedBossFrame > 0)
						{
							this.mCloakedBossFrame--;
						}
						if (this.mSmokePoof.mFrameNum >= (float)this.mSmokePoof.mLastFrameNum)
						{
							this.mSmokePoof.Dispose();
							this.mSmokePoof = null;
							return;
						}
					}
					else
					{
						if (this.mUpdateCnt % ZumasRevenge.Common._M(6) == 0 && this.mCloakedBossFrame > 0)
						{
							this.mCloakedBossFrame--;
						}
						if (this.mCloakedBossFrame == 0)
						{
							for (int i = 0; i < this.mIntroDialog.size<SimpleFadeText>(); i++)
							{
								SimpleFadeText simpleFadeText = this.mIntroDialog[i];
								if (simpleFadeText.mAlpha < 255f)
								{
									simpleFadeText.mAlpha += ZumasRevenge.Common._M(1.5f);
									if (simpleFadeText.mAlpha >= 255f)
									{
										simpleFadeText.mAlpha = 255f;
										if (i == this.mIntroDialog.size<SimpleFadeText>() - 1)
										{
										}
									}
									else if (simpleFadeText.mAlpha < (float)ZumasRevenge.Common._M(200))
									{
										break;
									}
								}
							}
							if (this.mIntroDialog.back<SimpleFadeText>().mAlpha >= 255f && !this.mShowMapScreen)
							{
								Board.gIntroRibbitTimer--;
							}
							if (this.mDoIntroFrogJump)
							{
								if (this.mFrogFlyOff.mJumpOut)
								{
									this.mFrogFlyOff.Update();
									this.mIntroFadeAmt += 255f / (float)this.mFrogFlyOff.mFrogJumpTime;
									if (this.mIntroFadeAmt > 255f)
									{
										this.mIntroFadeAmt = 255f;
									}
									if (this.mFrogFlyOff.mTimer >= this.mFrogFlyOff.mFrogJumpTime)
									{
										this.mShowMapScreen = false;
										this.mFrogFlyOff.JumpIn(this.mFrog, this.mFrog.GetCenterX() + this.mApp.mBoardOffsetX, this.mFrog.GetCenterY(), false);
										return;
									}
								}
								else
								{
									this.mLevel.UpdateEffects();
									this.mFrogFlyOff.Update();
									this.mIntroFadeAmt -= 255f / (float)this.mFrogFlyOff.mFrogJumpTime;
									if (this.mIntroFadeAmt < 0f)
									{
										this.mIntroFadeAmt = 0f;
									}
									if (this.mFrogFlyOff.mTimer >= this.mFrogFlyOff.mFrogJumpTime)
									{
										this.mDoingFirstTimeIntro = false;
										this.mDoingFirstTimeIntroZoomToGame = false;
										this.mShowMapScreen = false;
										this.mFrogFlyOff.Dispose();
										this.mFrogFlyOff = null;
										this.mDoIntroFrogJump = false;
										this.SetMenuBtnEnabled(true);
										this.mApp.mWidgetManager.SetFocus(this);
										if (this.mApp.mResourceManager.IsGroupLoaded("IntroScreen"))
										{
											this.mApp.mResourceManager.DeleteResources("IntroScreen");
										}
										if (this.mApp.mResourceManager.IsGroupLoaded("MapZoom"))
										{
											this.mApp.mResourceManager.DeleteResources("MapZoom");
										}
										if (this.mApp.mResourceManager.IsGroupLoaded("Map"))
										{
											this.mApp.mResourceManager.DeleteResources("Map");
										}
										if (this.mApp.mResourceManager.IsGroupLoaded("CloakedBoss"))
										{
											this.mApp.mResourceManager.DeleteResources("CloakedBoss");
										}
										if (this.mApp.mResourceManager.IsGroupLoaded("MenuRelated"))
										{
											this.mApp.mResourceManager.DeleteResources("MenuRelated");
										}
									}
								}
							}
						}
					}
					return;
				}
				if (this.DisplayingEndOfLevelStats() && this.mChallengeCupUnlockedFX != null)
				{
					this.mChallengeCupUnlockedFX.mDrawTransform.LoadIdentity();
					float num3 = GameApp.DownScaleNum(1f);
					this.mChallengeCupUnlockedFX.mDrawTransform.Scale(num3, num3);
					this.mChallengeCupUnlockedFX.mDrawTransform.Translate((float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(840)), (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(49)));
					this.mChallengeCupUnlockedFX.Update();
				}
				if (this.mScreenShakeTime > 0)
				{
					if (--this.mScreenShakeTime == 0)
					{
						GameApp.gScreenShakeX = (GameApp.gScreenShakeY = 0);
					}
					else
					{
						GameApp.gScreenShakeX = -this.mScreenShakeXMax + SexyFramework.Common.Rand(this.mScreenShakeXMax * 2);
						GameApp.gScreenShakeY = -this.mScreenShakeYMax + SexyFramework.Common.Rand(this.mScreenShakeYMax * 2);
					}
				}
				if (this.mNotificationWidget != null)
				{
					if (this.mNotificationWidget.IsFinished())
					{
						this.RemoveWidget(this.mNotificationWidget);
						this.mNotificationWidget.Dispose();
						this.mNotificationWidget = null;
					}
				}
				else if (this.m_NotificationQuene.Count > 0)
				{
					this.mNotificationWidget = new NotificationWidget(this, this.m_NotificationQuene[0].Key);
					this.mNotificationWidget.mSoundID = this.m_NotificationQuene[0].Value;
					this.AddWidget(this.mNotificationWidget);
					this.m_NotificationQuene.RemoveAt(0);
				}
				if (this.mGameState != GameState.GameState_ScorePage)
				{
					this.UpdateHaloSwap();
				}
				this.mApp.mProxBombManager.Update();
				for (int j = 0; j < this.mPowerEffects.size<PowerEffect>(); j++)
				{
					this.mPowerEffects[j].Update();
					if (this.mPowerEffects[j].IsDone())
					{
						this.mPowerEffects[j] = null;
						this.mPowerEffects.RemoveAt(j);
						j--;
					}
				}
				if (this.mGameState != GameState.GameState_Playing && this.mGameState != GameState.GameState_FinalBossPart1Finished && this.mGameState != GameState.GameState_Boss6StoneHeadBurst && this.mCheckpointEffect == null)
				{
					this.mLevel.UpdateEffects();
				}
				if (this.mLevelTransition != null)
				{
					this.mStatsHue = (this.mStatsHue + ZumasRevenge.Common._M(5)) % 255;
					if (this.mLevelTransition.Update())
					{
						if (this.mLevelTransition.mTransitionToStats)
						{
							this.SetMenuBtnEnabled(false);
							this.SetupStatsScreen();
							if (this.mLevel.mBoss == null && !this.mLevel.IsFinalBossLevel())
							{
								string levelId = this.mApp.GetLevelMgr().GetLevelId(this.mLevelNum);
								this.mApp.GetLevelMgr().GetLevelById(levelId, ref this.mNextLevel, this);
								this.mNextLevel.Preload();
							}
						}
						else
						{
							this.RemoveWidget(this.mStatsContinueBtn);
							this.mApp.SafeDeleteWidget(this.mStatsContinueBtn);
							this.mStatsContinueBtn = null;
							if (this.mLevel.mBoss == null && !this.mLevel.IsFinalBossLevel())
							{
								if (this.IronFrogMode())
								{
									this.ContinueToNextLevel();
								}
								this.SetMenuBtnEnabled(true);
							}
							if (this.mDoPostBossMapScreen)
							{
								if (this.mApp.mResourceManager.IsGroupLoaded("Bosses"))
								{
									this.mApp.mResourceManager.DeleteResources("Bosses");
								}
								if (this.mApp.mResourceManager.IsGroupLoaded("CommonBoss"))
								{
									this.mApp.mResourceManager.DeleteResources("CommonBoss");
								}
								this.SetupMapScreen(true);
								this.MarkDirty();
							}
							this.mDoPostBossMapScreen = false;
						}
					}
					else if (!this.mLevelTransition.mTransitionToStats)
					{
						this.mLevel.UpdateEffects();
					}
					else if (this.mLevelTransition.mTransitionToStats)
					{
						for (int k = 0; k < this.mText.size<BonusTextElement>(); k++)
						{
							this.mText[k].mBonus.Update();
							if (this.mText[k].mBonus.IsDone())
							{
								this.mText[k].mBonus = null;
								this.mText.RemoveAt(k);
								k--;
							}
						}
					}
					if (!this.mLevelTransition.mTransitionToStats && GlobalMembers.gIs3D && !this.IronFrogMode() && !this.mLevel.IsFinalBossLevel())
					{
						float num4 = this.mFrog.mDestAngle;
						if (num4 >= 3.14159f)
						{
							num4 -= 6.28318f;
						}
						this.mFrog.mAngle = (float)((double)num4 * this.mTransitionFrogRotPct.GetOutVal()) + ZumasRevenge.Common._M(7.853975f) * (float)(1.0 - this.mTransitionFrogRotPct.GetOutVal());
						this.mFrog.mCenterX = (this.mFrog.mCurX = (float)((double)this.mFrog.mDestX2 * this.mTransitionFrogPosPct.GetOutVal() + (double)(this.mTransitionCenter.mX - ZumasRevenge.Common._SS(this.mApp.mScreenBounds.mX) / 2) * (1.0 - this.mTransitionFrogPosPct.GetOutVal())));
						this.mFrog.mCenterY = (this.mFrog.mCurY = (float)((double)this.mFrog.mDestY2 * this.mTransitionFrogPosPct.GetOutVal() + (double)this.mTransitionCenter.mY * (1.0 - this.mTransitionFrogPosPct.GetOutVal())));
					}
					if (this.mLevelTransition.IsDone() && !this.mLevelTransition.mTransitionToStats && !this.mDoingTransition)
					{
						this.mLevelTransition.Dispose();
						this.mLevelTransition = null;
						if (this.mGameState != GameState.GameState_BossIntro)
						{
							this.mMenuButton.SetDisabled(false);
							if (this.mSwapBallButton != null)
							{
								this.mSwapBallButton.SetDisabled(false);
							}
						}
					}
					else
					{
						this.MarkDirty();
						if (this.DisplayingEndOfLevelStats() && !this.mDoingTransition)
						{
							if (this.DisplayingEndOfLevelStats() && !this.IronFrogMode())
							{
								this.mAdvStatsTime++;
								if (this.mAdvStatsTime == ZumasRevenge.Common._M(50) && this.mLevel.mBoss == null && this.mLevel.mNum != 10 && this.mLevel.mNum != 2147483647)
								{
									this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_LILLYPAD_JUMP));
								}
							}
							this.UnlockChallengeMode();
							if (this.mStatsDelay > 0)
							{
								this.mStatsDelay--;
							}
							else if (this.mGameState != GameState.GameState_BossIntro)
							{
								this.mCurStatsPointCounter += this.mCurStatsPointInc;
								this.mCurStatsPointInc++;
								if (this.mCurStatsPointCounter < this.mCurStatsPointTarget && !this.mApp.mSoundPlayer.IsLooping(Res.GetSoundByID(ResID.SOUND_NEW_ADV_STATS_TALLY)))
								{
									this.mApp.mSoundPlayer.Loop(Res.GetSoundByID(ResID.SOUND_NEW_ADV_STATS_TALLY));
								}
								if (this.mCurStatsPointCounter >= this.mCurStatsPointTarget)
								{
									this.mCurStatsPointCounter = this.mCurStatsPointTarget;
									if (this.mStatsState == 0)
									{
										this.mStatsState = 1;
										this.mCurStatsPointCounter = 0;
										this.mCurStatsPointTarget = this.mLevelPoints + this.GetAceTimeBonus() + this.GetPerfectBonus();
										this.mStatsDelay = 50;
										this.mCurStatsPointInc = this.mCurStatsPointTarget / ZumasRevenge.Common._M(300);
									}
									else
									{
										this.mStatsState = 2;
									}
									this.mApp.mSoundPlayer.Stop(Res.GetSoundByID(ResID.SOUND_NEW_ADV_STATS_TALLY));
								}
							}
						}
						if (!this.mShowMapScreen)
						{
							if (this.mGameState == GameState.GameState_BossIntro)
							{
								this.UpdateBossIntro();
							}
							return;
						}
					}
				}
				if (this.mShowMapScreen)
				{
					this.mMapScreen.Update();
					if (this.mMapScreen.mDirty)
					{
						this.MarkDirty();
					}
					if (this.mMapScreen.mClosing)
					{
						this.mIntroFadeAmt = (float)Math.Min(255.0, (double)(this.mIntroFadeAmt + ZumasRevenge.Common._M(3.5f)));
						this.mMapScreen.mRemove |= this.mIntroMapScale.HasBeenTriggered();
					}
					if (this.mMapScreen.mRemove)
					{
						if (this.mMapScreen.mClosing)
						{
							if (!this.mApp.IsRegistered() && this.mApp.mTrialType == 1 && this.mLevel.mZone == 3 && this.mLevel.mNum == 1)
							{
								this.mApp.DoUpsell(false);
							}
							this.mMapScreen.CloseDone();
							if (this.mFrog.GetType() == 3)
							{
								this.mApp.mSoundPlayer.Loop(Res.GetSoundByID(ResID.SOUND_LIGHTNING_LOOP));
							}
							this.SetMenuBtnEnabled(true);
							if (this.mLevel.mNum == 1 && this.mLevel.mZone != 1)
							{
								this.ToggleNotification(TextManager.getInstance().getString(432));
							}
							if (this.ShouldShowCheckpointPostcard() && !this.GauntletMode() && !this.IronFrogMode() && this.mLevel.mNum == 1 && this.mLevel.mZone > 1)
							{
								this.mNeedsCheckpointIntro = true;
								this.mPreventBallAdvancement = true;
							}
						}
						if (this.mDarkFrogSequence == null)
						{
							this.SetMenuBtnEnabled(true);
						}
						this.mShowMapScreen = false;
						if (this.mFrog.GetType() == 3)
						{
							this.mApp.mSoundPlayer.Loop(Res.GetSoundByID(ResID.SOUND_LIGHTNING_LOOP));
						}
						this.mWasShowingCheckpoint = false;
						if (this.mApp.mResourceManager.IsGroupLoaded("Map"))
						{
							this.mApp.mResourceManager.DeleteResources("Map");
						}
					}
					return;
				}
				if (!this.mDoingFirstTimeIntro)
				{
					this.mIntroFadeAmt = (float)Math.Max(0.0, (double)(this.mIntroFadeAmt - ZumasRevenge.Common._M(5f)));
				}
				if (this.mCheckpointEffect != null)
				{
					this.mCheckpointEffect.Update();
					this.MarkDirty();
					if (this.mCheckpointEffect.mDone)
					{
						bool mContinuePressed = this.mCheckpointEffect.mContinuePressed;
						bool mShowMap = this.mCheckpointEffect.mShowMap;
						this.mCheckpointEffect.mContinuePressed = (this.mCheckpointEffect.mShowMap = false);
						if (this.mLevel.mBoss == null || this.mApp.IsHardMode() || this.mLevel.mZone > 1)
						{
							this.mPreventBallAdvancement = false;
						}
						if (mContinuePressed)
						{
							this.RemoveWidget(this.mCheckpointEffect);
							this.mCheckpointEffect.Dispose();
							this.mCheckpointEffect = null;
							this.RestartLevel(true, null);
						}
						else if (mShowMap)
						{
							this.mWasShowingCheckpoint = true;
							this.SetupMapScreen(false);
							this.mCheckpointEffect.Disable(true);
						}
						else if (!this.mCheckpointEffect.mFromGameOver)
						{
							this.RemoveWidget(this.mCheckpointEffect);
							this.mCheckpointEffect.Dispose();
							this.mCheckpointEffect = null;
						}
						this.mWasShowingCheckpoint = false;
					}
					return;
				}
				if (this.mGauntletRetryBtn != null)
				{
					if (this.mGauntletAlpha < 255f && (this.mGauntletAlpha += ZumasRevenge.Common._M(10f)) >= 255f)
					{
						this.mGauntletRetryBtn.mVisible = true;
						this.mGauntletQuitBtn.mVisible = true;
						if (this.mScore >= this.mLevel.mChallengeAcePoints)
						{
							this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_CHALLENGE_ACE_VICTORY));
							this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_CHALLENGE_SCORE_VICTORY));
						}
						else if (this.mScore >= this.mLevel.mChallengePoints)
						{
							this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_CHALLENGE_SCORE_VICTORY));
						}
					}
					else if (this.mGauntletAlpha >= 255f)
					{
						this.mGauntletAlpha += ZumasRevenge.Common._M(10f);
					}
					if (this.mGauntletHSIndex <= 4 && this.mScoreDisplayPos != (float)this.mGauntletHSIndex && this.mGauntletAlpha >= (float)ZumasRevenge.Common._M(600))
					{
						int num5 = (int)this.mScoreDisplayPos;
						this.mScoreDisplayPos += ZumasRevenge.Common._M(-0.025f) + ((float)this.mGauntletHSIndex - this.mScoreDisplayPos) * ZumasRevenge.Common._M1(0.0025f);
						int num6 = (int)this.mScoreDisplayPos;
						if (num6 != num5 || this.mScoreDisplayPos < 0f)
						{
							if (num5 == 4)
							{
								Font fontByID = Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_GAUNTLET2);
								for (int l = 0; l < 2; l++)
								{
									int num7 = this.mScoreBreakPositions[l].mX;
									char thePrevChar = '\0';
									for (int m = 0; m < this.mScoreBreakStrings[l].Length; m++)
									{
										ScoreLetterEffect scoreLetterEffect = new ScoreLetterEffect();
										scoreLetterEffect.mChar = this.mScoreBreakStrings[l][m];
										scoreLetterEffect.mX = (float)num7;
										scoreLetterEffect.mY = (float)this.mScoreBreakPositions[l].mY;
										scoreLetterEffect.mVelX = (float)((double)(((float)m / Math.Max(1f, (float)this.mScoreBreakStrings[l].Length - 1f) - 0.5f) * ZumasRevenge.Common._S(ZumasRevenge.Common._M(1.5f))) + ((double)(SexyFramework.Common.Rand() % 1000) - 500.0) / 500.0 * (double)ZumasRevenge.Common._S(ZumasRevenge.Common._M(0.25f)));
										scoreLetterEffect.mVelY = (float)((double)(SexyFramework.Common.Rand() % 1000) / 1000.0 * (double)ZumasRevenge.Common._S(ZumasRevenge.Common._M(-1f)));
										scoreLetterEffect.mRot = 0f;
										scoreLetterEffect.mRotAdd = (float)((double)(SexyFramework.Common.Rand() % 1000) / 500.0 * (double)ZumasRevenge.Common._S(ZumasRevenge.Common._M(0.005f)));
										scoreLetterEffect.mUpdateCnt = 0f;
										num7 += fontByID.CharWidthKern(scoreLetterEffect.mChar, thePrevChar);
										thePrevChar = scoreLetterEffect.mChar;
										this.mScoreLetterEffectVector.Add(scoreLetterEffect);
									}
								}
							}
							if (num5 == this.mGauntletHSIndex)
							{
								this.mScoreDisplayPos = (float)this.mGauntletHSIndex;
							}
							if (num5 < 5)
							{
								this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_CANNON_FIRE));
							}
						}
					}
					else if (this.mGauntletAlpha >= (float)ZumasRevenge.Common._M(10))
					{
						this.mChallengeHeaderText.Update();
						this.mChallengePtsText.Update();
						this.mChallengeHeaderText.mX = (this.mWidth - this.mChallengeHeaderText.mImage.mWidth) / 2 + this.mChallengeHeaderText.mImage.mWidth / 2;
						this.mChallengeHeaderText.mY = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(150));
						this.mChallengePtsText.mX = (this.mWidth - this.mChallengePtsText.mImage.mWidth) / 2 + this.mChallengePtsText.mImage.mWidth / 2;
						if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CHT || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_PL)
						{
							this.mChallengePtsText.mY = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(370));
						}
						else
						{
							this.mChallengePtsText.mY = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(350));
						}
						if (MathUtils._eq(this.mChallengePtsText.mSize, 1f) && this.mChallengePtsText.mDelay == 0)
						{
							this.mChallengeTextAlpha += ZumasRevenge.Common._M(20f);
							if (this.mChallengeTextAlpha > 255f)
							{
								this.mChallengeTextAlpha = 255f;
							}
						}
					}
					for (int n = 0; n < this.mScoreLetterEffectVector.size<ScoreLetterEffect>(); n++)
					{
						ScoreLetterEffect scoreLetterEffect2 = this.mScoreLetterEffectVector[n];
						scoreLetterEffect2.mVelY += ZumasRevenge.Common._S(ZumasRevenge.Common._M(0.05f));
						scoreLetterEffect2.mX += scoreLetterEffect2.mVelX;
						scoreLetterEffect2.mY += scoreLetterEffect2.mVelY;
						scoreLetterEffect2.mRot += scoreLetterEffect2.mRotAdd;
						if (scoreLetterEffect2.mY > (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M(610)))
						{
							this.mScoreLetterEffectVector.RemoveAt(n);
							n--;
						}
					}
					this.MarkDirty();
					return;
				}
				if (!this.mLevel.CanUpdate())
				{
					return;
				}
				if (this.mLevel.mZone == 1 && this.mLevel.mBoss == null && !this.GauntletMode() && !this.mLevel.DoingInitialPathHilite() && this.mGameState == GameState.GameState_Playing && this.mApp.mUserProfile.GetAdvModeVars().mNumDeathsCurLevel == 1 && !this.mApp.mUserProfile.HasSeenHint(ZumaProfile.SKULL_PIT_HINT))
				{
					HoleInfo hole = this.mLevel.mHoleMgr.GetHole(0);
					int theX = ZumasRevenge.Common._S(hole.mX + ZumasRevenge.Common._M(-28));
					int theY = ZumasRevenge.Common._S(hole.mY + ZumasRevenge.Common._M(-30));
					int num8 = ZumasRevenge.Common._S(ZumasRevenge.Common._M(150));
					ZumaTip zumaTip = new ZumaTip(TextManager.getInstance().getString(827), ZumasRevenge.Common._S(ZumasRevenge.Common._M(200)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(100)), new Rect(theX, theY, num8, num8), ZumaProfile.SKULL_PIT_HINT);
					zumaTip.AutoPointAtCutoutRegion();
					this.mZumaTips.Add(zumaTip);
					this.mApp.mUserProfile.MarkHintAsSeen(ZumaProfile.SKULL_PIT_HINT);
					this.MarkDirty();
					return;
				}
				if (this.NeedsLillyPadHint() && !this.GauntletMode() && this.mZumaTips.size<ZumaTip>() == 0 && !this.mLevel.DoingInitialPathHilite())
				{
					int num9 = ((this.mLevel.mCurFrogPoint == 0) ? 1 : 0);
					int num10 = ZumasRevenge.Common._S(this.mLevel.mFrogX[num9]);
					int num11 = ZumasRevenge.Common._S(this.mLevel.mFrogY[num9]);
					ZumaTip zumaTip2 = new ZumaTip(TextManager.getInstance().getString(828), ZumasRevenge.Common._S(ZumasRevenge.Common._M(200)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(100)), new Rect(num10 - ZumasRevenge.Common._S(ZumasRevenge.Common._M2(100)), num11 - ZumasRevenge.Common._S(ZumasRevenge.Common._M3(100)), ZumasRevenge.Common._S(ZumasRevenge.Common._M4(200)), ZumasRevenge.Common._S(ZumasRevenge.Common._M5(200))), ZumaProfile.LILLY_PAD_HINT);
					zumaTip2.AutoPointAtCutoutRegion();
					zumaTip2.mBlockUpdates = false;
					this.mPreventBallAdvancement = true;
					this.mZumaTips.Add(zumaTip2);
					this.MarkDirty();
					return;
				}
				if (this.mLevel.mZone == 1 && !this.GauntletMode() && this.mLevel.mNum == 3 && !this.mApp.mUserProfile.HasSeenHint(ZumaProfile.SWAP_BALL_HINT) && this.mZumaTips.size<ZumaTip>() == 0 && !this.mLevel.DoingInitialPathHilite())
				{
					this.mPreventBallAdvancement = true;
					ZumaTip zumaTip3 = new ZumaTip(TextManager.getInstance().getString(829), ZumasRevenge.Common._DS(this.mFrog.mWidth * 3), ZumasRevenge.Common._DS(this.mFrog.mHeight * 2), new Rect((int)ZumasRevenge.Common._S(this.mFrog.mCurX - 75f), (int)ZumasRevenge.Common._S(this.mFrog.mCurY - 100f), ZumasRevenge.Common._S(150), ZumasRevenge.Common._S(200)), ZumaProfile.SWAP_BALL_HINT);
					zumaTip3.AutoPointAtCutoutRegion();
					zumaTip3.mBlockUpdates = false;
					this.mZumaTips.Add(zumaTip3);
					this.MarkDirty();
					return;
				}
				if (this.mLevel.mZone == 1 && !this.GauntletMode() && this.mLevel.mNum == 2 && !this.mApp.mUserProfile.HasSeenHint(ZumaProfile.FRUIT_HINT) && this.mZumaTips.size<ZumaTip>() == 0 && !this.mLevel.DoingInitialPathHilite())
				{
					this.mPreventBallAdvancement = true;
					this.mLevel.ForceTreasure(ZumasRevenge.Common._M(0));
					this.mApp.PlaySamplePan(Res.GetSoundByID(ResID.SOUND_TIKI_APPEAR), this.mApp.GetPan(this.mCurTreasure.x), 5);
					this.mTreasureEndFrame = 2147483646;
					this.mTreasureStarAlpha = 255;
					this.mTreasureGlowAlpha = 0;
					this.mTreasureGlowAlphaRate = ZumasRevenge.Common._M(12);
					this.mTreasureWasHit = false;
					this.mTreasureVY = (this.mTreasureDefaultVY = ZumasRevenge.Common._M(0.25f));
					this.mTreasureYBob = 0f;
					this.mTreasureAccel = ZumasRevenge.Common._M(-0.01f);
					this.mFruitBounceEffect.Reset();
					ZumaTip zumaTip4 = new ZumaTip(TextManager.getInstance().getString(830), ZumasRevenge.Common._S(ZumasRevenge.Common._M(200)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(140)), new Rect(ZumasRevenge.Common._S(this.mCurTreasure.x - ZumasRevenge.Common._M2(50)), ZumasRevenge.Common._S(this.mCurTreasure.y - ZumasRevenge.Common._M3(40)), ZumasRevenge.Common._S(ZumasRevenge.Common._M4(150)), ZumasRevenge.Common._S(ZumasRevenge.Common._M5(150))), ZumaProfile.FRUIT_HINT);
					zumaTip4.AutoPointAtCutoutRegion();
					zumaTip4.mBlockUpdates = false;
					this.mPreventBallAdvancement = true;
					this.mZumaTips.Add(zumaTip4);
					this.MarkDirty();
					return;
				}
				if (!this.mHasDoneIntroSounds)
				{
					if (this.mLevel.mBoss != null)
					{
						this.mLevel.mBoss.mNeedsIntroSound = true;
						if (!this.mLevel.DoingInitialPathHilite())
						{
							this.mHasDoneIntroSounds = true;
							SoundAttribs soundAttribs = new SoundAttribs();
							soundAttribs.fadeout = 0.008f;
							this.mApp.mSoundPlayer.Loop((this.mLevel.mZone == 5) ? Res.GetSoundByID(ResID.SOUND_UNDERWATER_ROLLOUT) : Res.GetSoundByID(ResID.SOUND_ROLLING), soundAttribs);
						}
					}
					else if (!this.mLevel.DoingInitialPathHilite() && this.mGameState != GameState.GameState_Losing && this.mZumaTips.size<ZumaTip>() == 0 && (!this.ShouldShowCheckpointPostcard() || this.mHasSeenCheckpointIntro || this.GauntletMode() || this.IronFrogMode()))
					{
						this.mHasDoneIntroSounds = true;
						SoundAttribs soundAttribs2 = new SoundAttribs();
						soundAttribs2.fadeout = 0.008f;
						this.mApp.mSoundPlayer.Loop((this.mLevel.mZone == 5) ? Res.GetSoundByID(ResID.SOUND_UNDERWATER_ROLLOUT) : Res.GetSoundByID(ResID.SOUND_ROLLING), soundAttribs2);
					}
				}
				if (this.mSkipToNextLevelOnNextUpdate)
				{
					this.mSkipToNextLevelOnNextUpdate = false;
					this.ContinueToNextLevel(-1, true);
				}
				this.mApp.IncFramesPlayed();
				this.mFrog.Update();
				if (this.mAccuracyBackupCount > 0 && !this.mFrog.LaserMode() && this.mFrog.GetLazerCount() == 0 && !this.mFrog.LightningMode() && !this.mFrog.CannonMode() && this.mFrog.GetType() != 1)
				{
					this.mAccuracyCount = this.mAccuracyBackupCount;
					this.mAccuracyBackupCount = 0;
					this.DoAccuracy(true);
				}
				if (this.mFrog.IsMovingToDest())
				{
					this.mRecalcGuide = (this.mRecalcLazerGuide = true);
				}
				if (!this.mHasSeenCheckpointIntro && this.ShouldShowCheckpointPostcard() && !this.GauntletMode() && !this.IronFrogMode() && this.mLevel.mNum > 1 && this.mCheckpointEffect == null)
				{
					if (this.mFrog.HasSmokeParticles())
					{
						this.mPreventBallAdvancement = true;
					}
					else
					{
						this.mNeedsCheckpointIntro = true;
					}
				}
				if (this.mNeedsCheckpointIntro)
				{
					this.mHasSeenCheckpointIntro = true;
					this.mNeedsCheckpointIntro = false;
					this.DoCheckpointEffect(false);
				}
				if (this.mGameState != GameState.GameState_BossIntro)
				{
					this.mStateCount++;
					if (this.mZumaTips.size<ZumaTip>() != 0 || this.mLevel.DoingInitialPathHilite())
					{
						this.mIgnoreCount++;
					}
				}
				if (this.GauntletMode() && this.mEndGauntletTimer >= 0 && this.mGauntletModeOver)
				{
					this.UpdatePlayingFX();
					this.mLevel.UpdateEffects();
					if (this.mEndGauntletTimer > 0)
					{
						Board.gMultTimeLeftDecAmt++;
						this.mLevel.mCurMultiplierTimeLeft -= Board.gMultTimeLeftDecAmt;
						if (this.mGauntletMultBarAlpha > 0f)
						{
							this.mGauntletMultBarAlpha -= ZumasRevenge.Common._M(5f);
						}
						this.mGauntletMultTextFlashTimer--;
						if (this.mGauntletMultBarAlpha <= 0f && --this.mEndGauntletTimer == 0)
						{
							this.mGauntletFinalScorePreBonus = this.mScore;
							this.mScore += (int)((float)this.mScoreMultiplier / 100f * (float)this.mScore);
							this.mRollerScore.SetTargetScore(this.mScore);
						}
						if (this.mEndGauntletTimer == 0)
						{
							Board.end_delay = ZumasRevenge.Common._M(25);
						}
					}
					if (this.mEndGauntletTimer == 0 && this.mRollerScore.mAtTarget && --Board.end_delay == 0)
					{
						this.SetupEndOfGauntletTransition(true);
					}
					Bullet firedBullet;
					while ((firedBullet = this.mFrog.GetFiredBullet()) != null)
					{
						this.AddFiredBullet(firedBullet);
						this.mLevel.BulletFired(firedBullet);
					}
				}
				else if (!this.GauntletMode() || !this.mGauntletModeOver)
				{
					if (this.mDoingIronFrogWin)
					{
						if (--this.mIronFrogWinDelay <= 0 && this.mIronFrogAlpha < 255f)
						{
							this.mIronFrogAlpha += ZumasRevenge.Common._M(10f);
							if (this.mIronFrogAlpha >= 255f)
							{
								this.mIronFrogAlpha = 255f;
								this.mIronFrogBtn.SetVisible(true);
								this.mIronFrogBtn.SetDisabled(false);
								this.mIronFrogBtn.mBtnNoDraw = false;
							}
						}
					}
					else
					{
						switch (this.mGameState)
						{
						case GameState.GameState_Playing:
							this.UpdatePlaying();
							break;
						case GameState.GameState_Losing:
							this.UpdateLosing();
							break;
						case GameState.GameState_LevelUp:
							this.CueLevelTransition();
							break;
						case GameState.GameState_BossDead:
							this.UpdateBossDeath();
							break;
						case GameState.GameState_FinalBossPart1Finished:
							this.UpdateFinalBossPart1Finished();
							break;
						case GameState.GameState_Boss6Transition:
							this.UpdateBoss6Transition();
							break;
						case GameState.GameState_Boss6FakeCredits:
							this.UpdateBoss6FakeCredits();
							break;
						case GameState.GameState_Boss6StoneHeadBurst:
							this.UpdateBoss6StoneHeadBurst();
							break;
						case GameState.GameState_Boss6DarkFrog:
							this.UpdateBoss6DarkFrog();
							break;
						case GameState.GameState_BossIntro:
							this.UpdateBossIntro();
							break;
						case GameState.GameState_BeatLevelBonus:
							this.UpdateBeatLevelBonus();
							break;
						}
					}
				}
				bool flag = this.mIsMouseDown;
				if (flag || this.mDoGuide || this.mAccuracyCount > 0 || ZumasRevenge.Common.gSuckMode || (this.mFrog.GetType() == 2 && this.mFrog.GetLazerCount() > 0) || this.mFrog.GetType() == 3)
				{
					if (flag || this.mDoGuide || ZumasRevenge.Common.gSuckMode || this.mAccuracyCount > 0)
					{
						this.UpdateGuide(false);
					}
					if ((this.mFrog.GetType() == 2 && this.mFrog.GetLazerCount() > 0) || this.mFrog.GetType() == 3)
					{
						this.UpdateGuide(true);
					}
				}
				else
				{
					this.mGuideBall = null;
				}
				this.UpdateMiscStuff();
				this.MarkDirty();
				return;
			}
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00040CC4 File Offset: 0x0003EEC4
		public override void Draw(Graphics g)
		{
			this.SetBoardOffset(g, true);
			if (this.mNumDrawFramesLeft > 0 && this.mApp.mDialogMap.Count == 0)
			{
				this.mNumDrawFramesLeft--;
			}
			bool flag = this.mApp.mCredits != null && MathUtils._geq(this.mApp.mCredits.mAlpha, 255f);
			bool flag2 = (this.mDoingIronFrogWin && MathUtils._geq(this.mIronFrogAlpha, 255f)) || flag;
			if (this.mDoingFirstTimeIntro)
			{
				if (this.mIntroMidAlpha < 1.0 && this.mFrogFlyOff.mJumpOut)
				{
					this.SetBoardOffset(g, false);
					g.DrawImage(this.mIntroBG, 0, 0, this.mApp.GetScreenRect().mWidth, this.mApp.GetScreenRect().mHeight);
				}
				base.DeferOverlay(9);
				if (this.mFrogFlyOff.mJumpOut)
				{
					return;
				}
			}
			bool flag3 = this.mLevelTransition != null && !this.mLevelTransition.IsDone();
			if ((this.mPauseCount > 0 || this.mNumDrawFramesLeft > 0 || this.mReturnToMainMenu || this.mFullScreenAlpha > 0 || this.mFlashAlpha > 0 || this.mShowDDSWindow || this.mShowBossDDSWindow || this.mZumaTips.size<ZumaTip>() > 0 || flag3 || this.mShowMapScreen || this.mIntroFadeAmt > 0f) && !flag2)
			{
				base.DeferOverlay(1);
				if (this.mShowMapScreen && this.mNumPauseUpdatesToDo <= 0)
				{
					return;
				}
			}
			if ((this.mGameState != GameState.GameState_BossIntro || (double)this.mBossIntroAlpha < 255.0 || (this.mDoingBossIntroFightText && !this.mFightImage.mForward)) && !flag2 && (this.mGameState != GameState.GameState_BossDead || this.mStateCount < 255) && this.mGameState != GameState.GameState_ScorePage)
			{
				SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
				if (GlobalMembers.gIs3D && this.mBossSmScale > 1.0)
				{
					sexyTransform2D.Translate(-g.mTransX - (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M(870)), (float)(-(float)ZumasRevenge.Common._S(ZumasRevenge.Common._M1(175))));
					sexyTransform2D.Scale((float)(1.0 + (this.mBossSmScale - 1.0) * (double)ZumasRevenge.Common._M(0.25f)), (float)(1.0 + (this.mBossSmScale - 1.0) * (double)ZumasRevenge.Common._M1(0.25f)));
					sexyTransform2D.Translate(g.mTransX + (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M(870)), (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M1(175)));
					g.Get3D().PushTransform(sexyTransform2D);
				}
				this.DrawPlaying(g);
				if (GlobalMembers.gIs3D && this.mBossSmScale > 1.0)
				{
					g.Get3D().PopTransform();
				}
			}
			if (this.mGameState == GameState.GameState_BeatLevelBonus)
			{
				for (int i = 0; i < this.mEndLevelExplosions.size<EndLevelExplosion>(); i++)
				{
					EndLevelExplosion endLevelExplosion = this.mEndLevelExplosions[i];
					if (endLevelExplosion.mDelay > 0)
					{
						endLevelExplosion.mPIEffect.mInUse = false;
					}
					else
					{
						endLevelExplosion.mPIEffect.mInUse = true;
					}
				}
				if (this.mEndLevelExplosions.Count > 0)
				{
					this.mEffectBatch.DrawBatch(g);
				}
			}
			Font fontByID = Res.GetFontByID(ResID.FONT_MAIN22);
			g.SetFont(fontByID);
			g.SetColor(SexyColor.White);
			if (Board.gShowText && !this.DisplayingEndOfLevelStats() && !this.IsPaused())
			{
				for (int j = 0; j < this.mText.size<BonusTextElement>(); j++)
				{
					this.mText[j].mBonus.Draw(g);
				}
			}
			g.mTransX = (float)this.mApp.mBoardUIOffsetX;
			if (this.mGameState != GameState.GameState_BossDead && !flag2 && (this.mGameState != GameState.GameState_BossIntro || (this.mDoingBossIntroFightText && !this.mFightImage.mForward)) && (this.mLevelTransition == null || this.mDoingTransition || this.IronFrogMode() || (this.mLevelTransition.mTransitionToStats && this.mLevelTransition.GetState() < 1) || (!this.mLevelTransition.mTransitionToStats && !this.mLevelTransition.IsDone() && this.mLevelTransition.GetState() == 2)))
			{
				this.mLevel.DrawUI(g);
			}
			g.mTransX = 0f;
			g.mTransX = (float)this.mApp.mBoardOffsetX;
			g.SetFont(fontByID);
			g.SetColor(SexyColor.White);
			if (!this.IsPaused())
			{
				for (int k = 0; k < this.mMultiplierBallEffects.size<MultiplierBallEffect>(); k++)
				{
					this.mMultiplierBallEffects[k].Draw(g);
				}
			}
			if (!flag2 && this.mGameState != GameState.GameState_ScorePage)
			{
				this.mLevel.DrawToplevel(g);
			}
			if (this.mLevelCompleteText.mImage != null && this.mGameState != GameState.GameState_ScorePage)
			{
				this.mLevelCompleteText.Draw(g);
			}
			if (!flag && this.mGameState != GameState.GameState_ScorePage)
			{
				if (this.mGameState == GameState.GameState_Boss6Transition)
				{
					g.SetColor(0, 0, 0, (this.mStateCount <= 255) ? this.mStateCount : (510 - this.mStateCount));
					g.mTransX = 0f;
					g.FillRect(this.mApp.GetScreenRect());
					g.mTransX = (float)this.mApp.mBoardOffsetX;
				}
				else if (this.mGameState == GameState.GameState_BossIntro)
				{
					this.DrawBossIntro(g);
				}
				else if (this.mGameState == GameState.GameState_Boss6FakeCredits)
				{
					this.DrawBoss6FakeCredits(g);
				}
				else if (this.mGameState == GameState.GameState_Boss6StoneHeadBurst)
				{
					this.DrawBoss6StoneHeadBurst(g);
				}
				else if (this.mGameState == GameState.GameState_Boss6DarkFrog)
				{
					this.DrawBoss6DarkFrog(g);
				}
				else if (this.mDoingIronFrogWin)
				{
					this.DrawIronFrogWin(g);
				}
				else if (this.mGameState == GameState.GameState_BossDead || (this.mGameState == GameState.GameState_LevelUp && this.mApp.mCredits != null))
				{
					int num = this.mLevel.mBoss.mDeathText.size<BossText>();
					g.SetColor(0, 0, 0, (this.mStateCount < 255) ? this.mStateCount : 255);
					g.mTransX = 0f;
					g.FillRect(this.mApp.GetScreenRect());
					g.mTransX = (float)this.mApp.mBoardOffsetX;
					if (this.mLevel.mCanDrawBoss && (!this.mLevel.mFinalLevel || !this.mAdventureWinScreen || this.mVortexAppear))
					{
						this.mLevel.mBoss.DrawDeathBGTikis(g);
					}
					if (!this.mAdventureWinScreen && !this.mDoingEndBossFrogEffect && this.mFrogFlyOff == null)
					{
						this.mFrog.Draw(g);
					}
					int num2 = -1;
					if (this.mLevel.mCanDrawBoss)
					{
						if (!this.mDoingEndBossFrogEffect || this.mBossSmokePoof.mFrameNum < (float)ZumasRevenge.Common._M(96))
						{
							this.mLevel.mBoss.Draw(g);
						}
						if (this.mDoingEndBossFrogEffect)
						{
							num2 = 255 - this.mEndBossFrogTimer * ZumasRevenge.Common._M(25);
							if (num2 < 0)
							{
								num2 = 0;
							}
						}
						g.mTransX = 0f;
						this.mLevel.mBoss.DrawDeathText(g, num2);
						g.mTransX = (float)this.mApp.mBoardOffsetX;
						if (this.mDoingEndBossFrogEffect)
						{
							if (this.mDoingEndBossFrogEffect && this.mBossSmokePoof.mCurNumParticles > 0)
							{
								g.PushState();
								this.mBossSmokePoof.Draw(g);
								g.PopState();
							}
							if (this.mFrogFlyOff == null)
							{
								Graphics3D graphics3D = g.Get3D();
								bool flag4 = graphics3D != null && this.mEndBossFrogTimer <= Board.END_BOSS_FROG_JUMP_TIME;
								float num3 = ZumasRevenge.Common._M(0.5f);
								float num4;
								if (this.mEndBossFrogTimer <= Board.END_BOSS_FROG_JUMP_TIME / 2)
								{
									num4 = 1f + (float)this.mEndBossFrogTimer / ((float)Board.END_BOSS_FROG_JUMP_TIME / 2f) * num3;
								}
								else
								{
									num4 = 1f + num3 - ((float)this.mEndBossFrogTimer - (float)Board.END_BOSS_FROG_JUMP_TIME / 2f) / ((float)Board.END_BOSS_FROG_JUMP_TIME / 2f) * num3;
								}
								SexyTransform2D sexyTransform2D2 = new SexyTransform2D(false);
								if (flag4)
								{
									sexyTransform2D2.Translate((float)(-(float)this.mApp.mWidth / 2), (float)(-(float)this.mApp.mHeight / 2));
									sexyTransform2D2.Scale(num4, num4);
									sexyTransform2D2.Translate((float)(this.mApp.mWidth / 2), (float)(this.mApp.mHeight / 2));
									graphics3D.PushTransform(sexyTransform2D2);
								}
								this.mFrog.Draw(g);
								if (flag4)
								{
									graphics3D.PopTransform();
								}
							}
							else
							{
								this.mFrogFlyOff.Draw(g);
							}
						}
					}
					if ((num == 0 || this.mLevel.mBoss.mDeathText[num - 1].mAlpha >= 254f) && this.mLevel.mFinalLevel && this.mAdventureWinScreen)
					{
						if (this.mAdventureWinAlpha < 255f)
						{
							this.DrawVortex(g, true);
						}
						if (this.mAdventureWinAlpha > 0f)
						{
							Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_BKGRND);
							Image imageByID2 = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_MS_ZUMA);
							Image imageByID3 = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_BURGER);
							Image imageByID4 = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_DOOR_MASK);
							Image imageByID5 = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_DOOR);
							Image imageByID6 = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_STAT);
							int theX = 520;
							int theY = 340;
							g.DrawImage(imageByID, ZumasRevenge.Common._S(-80), 0);
							g.DrawImage(imageByID6, theX, theY);
							if (!this.mApp.IsHardMode())
							{
								g.DrawImage(imageByID2, ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_MS_ZUMA) - 160), ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_MS_ZUMA)));
							}
							else
							{
								g.DrawImage(imageByID3, ZumasRevenge.Common._DS(ZumasRevenge.Common._M(1087) - 160), ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(176)));
							}
							g.DrawImage(imageByID5, ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_DOOR) - 160), (int)((float)ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_DOOR)) + this.mAdventureWinDoorYOff));
							g.DrawImage(imageByID4, ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_DOOR_MASK) - 160), ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_DOOR_MASK)));
							if (g.Is3D())
							{
								Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_TORCHFLAME).Draw(g);
							}
							if (this.mAdventureWinAlpha < 255f)
							{
								g.mTransX = 0f;
								g.SetColor(0, 0, 0, 255 - (int)this.mAdventureWinAlpha);
								g.FillRect(this.mApp.GetScreenRect());
								g.mTransX = (float)this.mApp.mBoardOffsetX;
							}
							List<ResID> list = new List<ResID>();
							list.Add(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_CONGRATS);
							list.Add(ResID.IMAGE_BOSS_DARKFROG_ADVENTURE_WIN_SECRET);
							int num5 = 10;
							int num6 = 4;
							g.PushState();
							if (this.mAdventureWinExtraAlpha < 255f)
							{
								g.SetColorizeImages(true);
							}
							g.SetColor(255, 255, 255, (int)this.mAdventureWinExtraAlpha);
							for (int l = 0; l < list.size<ResID>(); l++)
							{
								g.DrawImage(Res.GetImageByID(list[l]), ZumasRevenge.Common._DS(Res.GetOffsetXByID(list[l]) - 160), ZumasRevenge.Common._DS(Res.GetOffsetYByID(list[l])));
							}
							Font fontByID2 = Res.GetFontByID(ResID.FONT_SHAGLOUNGE45_YELLOW);
							g.SetFont(fontByID2);
							int num7 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(1386) - this.mApp.mOffset160X);
							g.DrawString(JeffLib.Common.UpdateToTimeStr(this.mTimeToBeatAdvMode, true), num7 + num5, num6 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(824)));
							g.DrawString(SexyFramework.Common.CommaSeperate(this.mBeatGameNormalScore), num7 + num5, num6 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(890)));
							g.DrawString("+" + SexyFramework.Common.CommaSeperate(this.mBeatGameLives * this.mApp.GetLevelMgr().mBeatGamePointsForLife), num7 + num5, num6 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(950)));
							Font fontByID3 = Res.GetFontByID(ResID.FONT_SHAGLOUNGE45_RED);
							int num8 = ((this.mBeatGameLives > 99) ? 99 : this.mBeatGameLives);
							g.SetFont(fontByID3);
							g.DrawString("x" + num8, ZumasRevenge.Common._DS(ZumasRevenge.Common._M(1114)), num6 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(950)));
							if (this.mBeatGameTotalScoreTally > 0)
							{
								g.DrawString(SexyFramework.Common.CommaSeperate(this.mBeatGameTotalScoreTally), ZumasRevenge.Common._DS(ZumasRevenge.Common._M(1440) - this.mApp.mOffset160X) + num5, num6 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(1046)));
							}
							g.SetColorizeImages(false);
							g.PopState();
						}
					}
				}
				if (!this.mLevel.IsFinalBossLevel() && this.mFakeCredits == null && this.mLevel.mEndSequence != 5 && this.mLevel.mEndSequence != 3 && this.mGameState != GameState.GameState_BossIntro && !this.mAdventureWinScreen && (this.mGameState != GameState.GameState_BossDead || this.mLevel.mEndSequence != 5))
				{
					g.SetFont(fontByID);
					g.SetColor(0, 0, 0, 128);
					Ratio aspectRatio = this.mApp.mGraphicsDriver.GetAspectRatio();
					int num9 = ((aspectRatio.mNumerator != 4 && aspectRatio.mDenominator != 3) ? ZumasRevenge.Common._S(-80) : 0);
					int num10 = ZumasRevenge.Common._S(ZumasRevenge.Common._M(25));
					int num11 = this.mApp.mHeight - num10;
					if (!this.GauntletMode())
					{
						ZumasRevenge.Common._S(ZumasRevenge.Common._M1(65));
					}
					else
					{
						ZumasRevenge.Common._S(ZumasRevenge.Common._M(70));
					}
					if (this.mDisplayAceTime && !this.GauntletMode() && !this.IronFrogMode() && this.mAdventureMode)
					{
						ZumasRevenge.Common._S(ZumasRevenge.Common._M(35));
					}
					ZumasRevenge.Common._M(1);
					if (!this.IronFrogMode() && this.mDisplayAceTime && this.mLevel.mBoss == null)
					{
						StringBuilder stringBuilder = new StringBuilder(TextManager.getInstance().getString(142));
						stringBuilder.Replace("$1", SexyFramework.Common.CommaSeperate(this.mLevel.mChallengePoints));
						stringBuilder.Replace("$2", SexyFramework.Common.CommaSeperate(this.mLevel.mChallengeAcePoints));
						string theString = stringBuilder.ToString();
						g.GetFont().StringWidth(theString);
						g.SetColor(SexyColor.White);
						g.DrawString(theString, num9 + ZumasRevenge.Common._S(ZumasRevenge.Common._M(0)), num11 + ZumasRevenge.Common._S(ZumasRevenge.Common._M1(20)));
					}
				}
				int num12 = ZumasRevenge.Common._M(300);
				if (this.GauntletMode() && this.mStateCount < num12 && this.mGameState != GameState.GameState_Losing)
				{
					float mTransX = g.mTransX;
					g.mTransX = 0f;
					Font fontByID4 = Res.GetFontByID(ResID.FONT_SHAGEXOTICA100_STROKE);
					Font fontByID5 = Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_STROKE);
					float num13 = (float)this.mStateCount * ZumasRevenge.Common._M(0.5f);
					float num14 = (float)(this.mHeight / 2) - num13;
					int alpha = ((num12 - this.mStateCount > 127) ? 255 : ((num12 - this.mStateCount) * 2));
					g.SetFont(fontByID4);
					g.SetColor(255, 255, 255, alpha);
					g.WriteString(TextManager.getInstance().getString(125), 0, (int)num14, this.mWidth);
					if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CHT)
					{
						num14 += (float)(g.GetFont().GetHeight() + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(10)));
					}
					else
					{
						num14 += (float)(g.GetFont().GetHeight() + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(-80)));
					}
					g.SetFont(fontByID5);
					StringBuilder stringBuilder2 = new StringBuilder(TextManager.getInstance().getString(126));
					stringBuilder2.Replace("$1", SexyFramework.Common.CommaSeperate(this.mLevel.mChallengePoints));
					g.WriteString(stringBuilder2.ToString(), 0, (int)num14, this.mWidth);
					g.mTransX = mTransX;
				}
				else if (this.GauntletMode() && this.mEndGauntletTimer >= 0 && this.mGauntletModeOver && this.mGauntletMultBarAlpha <= 0f)
				{
					float mTransX2 = g.mTransX;
					g.mTransX = 0f;
					int num15 = ((this.mEndGauntletTimer <= 50) ? ((int)((float)this.mEndGauntletTimer * 5.1f)) : 255);
					if (num15 > 255)
					{
						num15 = 255;
					}
					g.SetColor(255, 255, 255, num15);
					int num16 = Board.END_GAUNTLET_TIME + Board.gEndGauntletExtraTime - this.mEndGauntletTimer;
					float num17 = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(500));
					string theString2 = TextManager.getInstance().getString(127);
					Font fontByID6 = Res.GetFontByID(ResID.FONT_SHAGEXOTICA100_STROKE);
					g.SetFont(fontByID6);
					float num18 = (float)((this.mWidth - fontByID6.StringWidth(theString2)) / 2);
					float num19 = (float)num16 * num18 / ZumasRevenge.Common._M(10f);
					if (num19 > num18)
					{
						num19 = num18;
					}
					g.WriteString(theString2, (int)num19, (int)num17, -1, -1);
					num17 += (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(100));
					int num20 = ZumasRevenge.Common._M(50);
					if (num16 > num20)
					{
						fontByID6 = Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_STROKE);
						g.SetFont(fontByID6);
						StringBuilder stringBuilder3 = new StringBuilder(TextManager.getInstance().getString(128));
						stringBuilder3.Replace("$1", this.mScoreMultiplier.ToString());
						stringBuilder3.Replace("%%", "%");
						theString2 = stringBuilder3.ToString();
						num18 = (float)((this.mWidth - fontByID6.StringWidth(theString2)) / 2);
						num19 = (float)this.mWidth - (float)(num16 - num20) * num18 / ZumasRevenge.Common._M(5f);
						if (num19 < num18)
						{
							num19 = num18;
						}
						g.WriteString(theString2, (int)num19, (int)num17, -1, -1);
						num17 += (float)fontByID6.mHeight;
					}
					int num21 = ZumasRevenge.Common._M(50);
					if (num16 > num20 + num21)
					{
						StringBuilder stringBuilder4 = new StringBuilder(TextManager.getInstance().getString(129));
						stringBuilder4.Replace("$1", SexyFramework.Common.CommaSeperate((int)((float)this.mScoreMultiplier / 100f * (float)this.mScore)));
						theString2 = stringBuilder4.ToString();
						num18 = (float)((this.mWidth - fontByID6.StringWidth(theString2)) / 2);
						float num22 = (float)this.mHeight - (float)(num16 - num20 - num21) * num17 / ZumasRevenge.Common._M(15f);
						if (num22 < num17)
						{
							num22 = num17;
						}
						g.SetColor(SexyColor.White);
						int num23 = ZumasRevenge.Common._M(20);
						if (this.mEndGauntletTimer < num23)
						{
							num22 -= (float)(num23 - this.mEndGauntletTimer) * ZumasRevenge.Common._M(40f);
						}
						g.WriteString(theString2, (int)num18, (int)num22, -1, -1);
					}
					g.mTransX = mTransX2;
				}
				if (this.mEndBossFadeAmt > 0f)
				{
					base.DeferOverlay(5);
				}
			}
			if (this.mTransitionScreenHolePct.mRamp == 6 && !this.mTransitionScreenHolePct.HasBeenTriggered())
			{
				base.DeferOverlay(0);
			}
			if (this.mGameState != GameState.GameState_ScorePage)
			{
				this.DrawHaloSwap(g);
			}
			if (this.mGauntletRetryBtn != null)
			{
				base.DeferOverlay(1);
			}
			bool flag5 = false;
			bool flag6 = false;
			if (this.mLevelTransition != null)
			{
				flag6 = true;
				if (this.mLevelTransition.mFrogEffect != null)
				{
					flag5 = !this.mLevelTransition.mFrogEffect.HasCompletedFlyOff();
				}
			}
			if (!this.mShowMapScreen && (!flag6 || flag5 || this.mGameState == GameState.GameState_BossIntro))
			{
				this.SetBoardOffset(g, false);
				Image imageByID7 = Res.GetImageByID(ResID.IMAGE_UI_POLE);
				if (imageByID7 != null)
				{
					g.DrawImage(imageByID7, 0, 0);
					g.DrawImageMirror(imageByID7, 1013, 0);
				}
				this.SetBoardOffset(g, true);
			}
			if (this.mLivesInfo != null)
			{
				g.mTransX = 0f;
				this.mLivesInfo.Draw(g);
				g.mTransX = (float)this.mApp.mBoardOffsetX;
			}
			if (this.mLevelTransition != null && !this.mLevelTransition.IsDone() && !this.mDoingTransition)
			{
				this.SetBoardOffset(g, false);
				if (this.mLevelTransition.GetState() == 2)
				{
					g.SetColor(SexyColor.Black);
					g.FillRect(this.mApp.GetScreenRect());
				}
				this.mLevelTransition.Draw(g);
				if (this.mLevelTransition.mState == 1)
				{
					this.SetBoardOffset(g, true);
					this.DrawAdventureStats(g);
				}
			}
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x000421F1 File Offset: 0x000403F1
		public bool isResultPageInAdvMode()
		{
			return this.mAdventureMode && this.mLevelTransition != null && !this.mLevelTransition.IsDone() && !this.mDoingTransition && this.mLevelTransition.mState == 1;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00042228 File Offset: 0x00040428
		public override void DrawOverlay(Graphics g, int priority)
		{
			this.DrawChallengeStats(g);
			if (this.mFlashAlpha > 0)
			{
				g.SetColor(255, 255, 255, this.mFlashAlpha);
				g.FillRect(this.mApp.GetScreenRect());
			}
			if (this.mShowMapScreen && !this.mDoingFirstTimeIntro)
			{
				Graphics3D graphics3D = g.Get3D();
				SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
				if (this.mMapScreen.mClosing && graphics3D != null)
				{
					SexyPoint zoneCenter = this.mMapScreen.GetZoneCenter(this.mMapScreen.mDisplayedZone);
					int num = ZumasRevenge.Common._DS(zoneCenter.mX) - ZumasRevenge.Common._S(80) + (int)this.mMapScreen.mUnlockScrollAmt - this.mApp.mScreenBounds.mX;
					int num2 = ZumasRevenge.Common._DS(zoneCenter.mY);
					sexyTransform2D.LoadIdentity();
					sexyTransform2D.Translate((float)(-(float)num), (float)(-(float)num2));
					sexyTransform2D.Scale((float)this.mIntroMapScale, (float)this.mIntroMapScale);
					sexyTransform2D.Translate((float)num, (float)num2);
					graphics3D.PushTransform(sexyTransform2D);
				}
				this.mMapScreen.Draw(g);
				if (this.mMapScreen.mClosing && graphics3D != null)
				{
					graphics3D.PopTransform();
				}
			}
			g.mTransX = (float)this.mApp.mBoardOffsetX;
			if (this.mFullScreenAlpha > 0)
			{
				g.mTransX = 0f;
				g.SetColor(0, 0, 0, Math.Min(this.mFullScreenAlpha, 255));
				g.FillRect(this.mApp.GetScreenRect());
				g.mTransX = (float)this.mApp.mBoardOffsetX;
				if (this.mLevel.IsFinalBossLevel() || this.mLevel.mEndSequence == 2)
				{
					this.mLevel.DrawDaisRocks(g);
				}
			}
			if ((this.mPauseCount > 0 || this.mNumDrawFramesLeft > 0 || this.mReturnToMainMenu) && this.mChallengeHelp == null && this.mApp.mGenericHelp == null && !this.mDoingFirstTimeIntro && this.mEndBossFadeAmt <= 0f && this.mGauntletRetryBtn == null && !this.mShowMapScreen && this.mZumaTips.size<ZumaTip>() == 0 && this.mLevel.CanUpdate() && this.mCheckpointEffect == null && this.mLevelTransition == null)
			{
				if (!GameApp.USE_TRIAL_VERSION && !this.mShowBallsDuringPause && !this.IsPaused())
				{
					g.mTransX = 0f;
					g.SetColor(0, 0, 0, 255 * this.mPauseFade / 100);
					g.FillRect(this.mApp.GetScreenRect());
					g.mTransX = (float)this.mApp.mBoardOffsetX;
				}
				if (this.mLevel.mBoss == null && !this.DoingIntros())
				{
					if (this.mLevel.mNum != 2147483647)
					{
						float mTransX = g.mTransX;
						g.mTransX = 0f;
						Font fontByID = Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_STROKE);
						g.SetFont(fontByID);
						g.SetColor(252, 244, 159);
						int theY = (this.GauntletMode() ? ZumasRevenge.Common._S(ZumasRevenge.Common._M(55)) : ZumasRevenge.Common._S(ZumasRevenge.Common._M1(55)));
						int theX = ZumasRevenge.Common._S(ZumasRevenge.Common._M(0));
						StringBuilder stringBuilder = new StringBuilder("$1-$2");
						stringBuilder.Replace("$1", ((this.mLevel.mZone - 1) * 10 + this.mLevel.mNum).ToString());
						stringBuilder.Replace("$2", this.mLevel.mDisplayName);
						g.WriteString(stringBuilder.ToString(), theX, theY, this.mWidth, 0);
						g.mTransX = mTransX;
					}
				}
				else if (this.mLevel.mBoss != null && !this.DoingIntros())
				{
					Boss mBoss = this.mLevel.mBoss;
					float mTransX2 = g.mTransX;
					g.mTransX = 0f;
					Font fontByID2 = Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_STROKE);
					g.SetFont(fontByID2);
					g.SetColor(252, 244, 159);
					int theY2 = (this.GauntletMode() ? ZumasRevenge.Common._S(ZumasRevenge.Common._M(55)) : ZumasRevenge.Common._S(ZumasRevenge.Common._M1(55)));
					int theX2 = ZumasRevenge.Common._S(ZumasRevenge.Common._M(0));
					g.WriteString(mBoss.mName, theX2, theY2, this.mWidth, 0);
					g.mTransX = mTransX2;
				}
				if (this.mDialogCount == 0)
				{
					Image imageByID = Res.GetImageByID(ResID.IMAGE_PAUSED);
					if (this.mLevel.mBoss == null && !this.DoingIntros())
					{
						if (this.IronFrogMode() || this.mLevel.IsFinalBossLevel())
						{
							g.DrawImage(imageByID, (this.mWidth - imageByID.mWidth) / 2, (this.mHeight - imageByID.mHeight) / 2);
						}
						else
						{
							int theY3 = (this.GauntletMode() ? ZumasRevenge.Common._DS(ZumasRevenge.Common._M(320)) : ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(150)));
							g.DrawImage(imageByID, (this.mWidth - imageByID.mWidth) / 2, theY3);
							Font fontByID3 = Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_STROKE);
							g.SetFont(fontByID3);
							g.SetColor(252, 244, 159);
							int num3 = (this.GauntletMode() ? ZumasRevenge.Common._S(ZumasRevenge.Common._M(370)) : ZumasRevenge.Common._S(ZumasRevenge.Common._M1(325)));
							int num4 = ZumasRevenge.Common._S(ZumasRevenge.Common._M(0));
							if (this.mLevel.mNum != 2147483647)
							{
								if (this.mLevel.mIronFrog)
								{
									StringBuilder stringBuilder2 = new StringBuilder(TextManager.getInstance().getString(116));
									stringBuilder2.Replace("$1", this.mLevel.mNum.ToString());
									stringBuilder2.Replace("$2", this.mLevel.mDisplayName);
									g.WriteString(stringBuilder2.ToString(), num4, num3, this.mWidth, 0);
								}
								else
								{
									StringBuilder stringBuilder3 = new StringBuilder(TextManager.getInstance().getString(117));
									stringBuilder3.Replace("$1", ((this.mLevel.mZone - 1) * 10 + this.mLevel.mNum).ToString());
									stringBuilder3.Replace("$2", this.mLevel.mDisplayName);
									g.WriteString(stringBuilder3.ToString(), num4, num3, this.mWidth, 0);
								}
							}
							else
							{
								num3 -= fontByID3.GetHeight();
							}
							if (!this.GauntletMode() && !this.IronFrogMode())
							{
								num4 += ZumasRevenge.Common._S(ZumasRevenge.Common._M(436));
								num3 += fontByID3.GetHeight() + ZumasRevenge.Common._S(ZumasRevenge.Common._M(0));
								int num5 = ((this.mGameState == GameState.GameState_BeatLevelBonus) ? this.mLevelStats.mTimePlayed : (this.mStateCount - this.mIgnoreCount));
								if (num5 < 0)
								{
									num5 = 0;
								}
								if (this.mGameState != GameState.GameState_Losing)
								{
									StringBuilder stringBuilder4 = new StringBuilder("^8683a5^");
									stringBuilder4.Append(TextManager.getInstance().getString(118));
									g.WriteString(stringBuilder4.ToString(), num4, num3, -1, 1);
									g.WriteString("^85e6c3^" + JeffLib.Common.UpdateToTimeStr(num5), num4 + ZumasRevenge.Common._S(ZumasRevenge.Common._M(20)), num3, -1, -1);
									num3 += fontByID3.GetHeight() + ZumasRevenge.Common._S(ZumasRevenge.Common._M(0));
								}
								if (!this.mLevel.IsFinalBossLevel())
								{
									StringBuilder stringBuilder5 = new StringBuilder("^8683a5^");
									stringBuilder5.Append(TextManager.getInstance().getString(119));
									g.WriteString(stringBuilder5.ToString(), num4, num3, -1, 1);
									g.WriteString("^85e6c3^" + JeffLib.Common.UpdateToTimeStr(this.mLevel.mParTime), num4 + ZumasRevenge.Common._S(ZumasRevenge.Common._M(20)), num3, -1, -1);
									num3 += fontByID3.GetHeight() + ZumasRevenge.Common._S(ZumasRevenge.Common._M(0));
								}
								StringBuilder stringBuilder6 = new StringBuilder("^8683a5^");
								stringBuilder6.Append(TextManager.getInstance().getString(120));
								g.WriteString(stringBuilder6.ToString(), num4, num3, -1, 1);
								g.WriteString("^85e6c3^" + this.mApp.mUserProfile.GetAdvModeVars().mNumDeathsCurLevel, num4 + ZumasRevenge.Common._S(ZumasRevenge.Common._M(20)), num3, -1, -1);
								num3 += fontByID3.GetHeight() + ZumasRevenge.Common._S(ZumasRevenge.Common._M(0));
								StringBuilder stringBuilder7 = new StringBuilder("^8683a5^");
								stringBuilder7.Append(TextManager.getInstance().getString(121));
								g.WriteString(stringBuilder7.ToString(), num4, num3, -1, 1);
								g.WriteString("^85e6c3^" + (this.mLives - 1), num4 + ZumasRevenge.Common._S(ZumasRevenge.Common._M(20)), num3, -1, -1);
							}
							else if (this.GauntletMode())
							{
								num4 += ZumasRevenge.Common._S(ZumasRevenge.Common._M(436));
								num3 += fontByID3.GetHeight() + ZumasRevenge.Common._S(ZumasRevenge.Common._M(0));
								StringBuilder stringBuilder8 = new StringBuilder("^8683a5^");
								stringBuilder8.Append(TextManager.getInstance().getString(115));
								g.WriteString(stringBuilder8.ToString(), num4, num3, -1, 1);
								g.WriteString("^85e6c3^" + SexyFramework.Common.CommaSeperate(this.mRollerScore.GetTargetScore()), num4 + ZumasRevenge.Common._S(ZumasRevenge.Common._M(20)), num3, -1, -1);
								num3 += fontByID3.GetHeight() + ZumasRevenge.Common._S(ZumasRevenge.Common._M(0));
								StringBuilder stringBuilder9 = new StringBuilder("^8683a5^");
								stringBuilder9.Append(TextManager.getInstance().getString(122));
								g.WriteString(stringBuilder9.ToString(), num4, num3, -1, 1);
								g.WriteString("^85e6c3^" + SexyFramework.Common.CommaSeperate(this.mLevel.mChallengePoints), num4 + ZumasRevenge.Common._S(ZumasRevenge.Common._M(20)), num3, -1, -1);
								num3 += fontByID3.GetHeight() + ZumasRevenge.Common._S(ZumasRevenge.Common._M(0));
								StringBuilder stringBuilder10 = new StringBuilder("^8683a5^");
								stringBuilder10.Append(TextManager.getInstance().getString(123));
								g.WriteString(stringBuilder10.ToString(), num4, num3, -1, 1);
								g.WriteString("^85e6c3^" + SexyFramework.Common.CommaSeperate(this.mLevel.mChallengeAcePoints), num4 + ZumasRevenge.Common._S(ZumasRevenge.Common._M(20)), num3, -1, -1);
								num3 += fontByID3.GetHeight() + ZumasRevenge.Common._S(ZumasRevenge.Common._M(0));
							}
						}
					}
					else if (this.mLevel.mBoss != null)
					{
						g.DrawImage(imageByID, (this.mWidth - imageByID.mWidth) / 2, (this.mHeight - imageByID.mHeight) / 2);
					}
				}
			}
			else if (this.mZumaTips.size<ZumaTip>() > 0 && this.mPauseCount == 0)
			{
				this.mZumaTips[0].Draw(g);
			}
			if (this.mEndBossFadeAmt > 0f)
			{
				g.mTransX = 0f;
				g.SetColor(0, 0, 0, (int)Math.Min(this.mEndBossFadeAmt, 255f));
				g.FillRect(this.mApp.GetScreenRect());
				g.mTransX = (float)this.mApp.mBoardOffsetX;
			}
			if (this.mShowDDSWindow)
			{
				g.SetColor(0, 0, 255, 200);
				int num6 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(110));
				int theWidth = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(600));
				string text = "";
				if (this.GauntletMode())
				{
					num6 += ZumasRevenge.Common._DS(ZumasRevenge.Common._M(50));
					text = string.Concat(new object[]
					{
						"\nGauntlet Difficulty Tier: ",
						GameApp.gDDS.mCurGauntletDiffIdx + 1,
						" (t=",
						GameApp.gDDS.GetGauntletDiffLevel(),
						")"
					});
				}
				g.FillRect(ZumasRevenge.Common._M(0), this.mHeight - num6, theWidth, num6);
				g.SetFont(Res.GetFontByID(ResID.FONT_MAIN22));
				g.SetColor(SexyColor.White);
				int theX3 = ZumasRevenge.Common._M(5);
				int theY4 = this.mHeight - num6 + g.mFont.GetAscent();
				g.WriteWordWrapped(new Rect(theX3, theY4, theWidth, num6), GameApp.gDDS.GetStatsString() + text);
			}
			if (this.mShowBossDDSWindow)
			{
				g.SetColor(0, 0, 255, 200);
				g.SetFont(Res.GetFontByID(ResID.FONT_MAIN22));
				string theLine = "";
				GameApp.gDDS.GetBossDebugString(ref theLine, true);
				int num7 = ZumasRevenge.Common._M(250);
				int num8 = 0;
				int num9 = g.GetWordWrappedHeight(num7, theLine, -1, ref num8, ref num8) + g.GetFont().GetHeight();
				g.FillRect(this.mWidth - num7, this.mHeight - num9, num7, num9);
				g.SetColor(SexyColor.White);
				int theX4 = this.mWidth - num7 + ZumasRevenge.Common._M(5);
				int theY5 = this.mHeight - num9 + g.mFont.GetAscent();
				g.WriteWordWrapped(new Rect(theX4, theY5, num7, num9), theLine);
			}
			g.mTransX = 0f;
			if (this.mDoingFirstTimeIntro)
			{
				SexyTransform2D sexyTransform2D2 = new SexyTransform2D(false);
				if (this.mFrogFlyOff != null && this.mFrogFlyOff.mJumpOut)
				{
					if (this.mSmokePoof == null || this.mSmokePoof.mFrameNum >= (float)ZumasRevenge.Common._M(108))
					{
						int num10 = this.mWidth / 2 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(-154));
						int num11 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(40));
						g.PushState();
						if (this.mCloakBossIntroAlpha < 255)
						{
							g.SetColorizeImages(true);
							g.SetColor(255, 255, 255, this.mCloakBossIntroAlpha);
						}
						Image imageByID2 = Res.GetImageByID(ResID.IMAGE_BOSS_LAME_CLOAKEDBOSS_ARMDOWN_SHADOW);
						Image imageByID3 = Res.GetImageByID(ResID.IMAGE_BOSS_LAME_CLOAKEDBOSS_ARMDOWN_REST);
						g.DrawImage(imageByID2, num10 + ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_BOSS_LAME_CLOAKEDBOSS_ARMDOWN_SHADOW)), num11 + ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_BOSS_LAME_CLOAKEDBOSS_ARMDOWN_SHADOW)));
						g.DrawImage(imageByID3, num10 + ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_BOSS_LAME_CLOAKEDBOSS_ARMDOWN_REST)), num11 + ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_BOSS_LAME_CLOAKEDBOSS_ARMDOWN_REST)));
						g.SetColorizeImages(false);
						g.PopState();
					}
					if (this.mSmokePoof != null)
					{
						this.mSmokePoof.Draw(g);
					}
					bool flag = false;
					Font fontByID4 = Res.GetFontByID(ResID.FONT_BOSS_TAUNT);
					if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH)
					{
						fontByID4.mAscent = 25;
					}
					g.SetFont(fontByID4);
					for (int i = 0; i < this.mIntroDialog.size<SimpleFadeText>(); i++)
					{
						SimpleFadeText simpleFadeText = this.mIntroDialog[i];
						if (simpleFadeText.mAlpha <= 0f)
						{
							break;
						}
						g.SetColor(255, 255, 255, (int)simpleFadeText.mAlpha);
						g.WriteString(simpleFadeText.mString, 0, this.mHeight / 2 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(-60)) + i * ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(65)), this.mWidth, 0);
						if (i == this.mIntroDialog.size<SimpleFadeText>() - 1 && simpleFadeText.mAlpha >= 255f)
						{
							flag = true;
						}
					}
					if (flag && !this.mDoIntroFrogJump && Board.gIntroRibbitTimer <= 0)
					{
						g.DrawImage(Res.GetImageByID(ResID.IMAGE_FROG_RIBBIT), (int)(ZumasRevenge.Common._S(this.mFrogFlyOff.mFrogX) + (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M(-60))), (int)(ZumasRevenge.Common._S(this.mFrogFlyOff.mFrogY) - (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M1(120))));
					}
				}
				Graphics3D graphics3D2 = g.Get3D();
				g.SetColorizeImages(true);
				bool flag2 = ZumasRevenge.Common._M(1) == 1;
				if (this.mIntroMidAlpha > 0.0 && this.mShowMapScreen && GlobalMembers.gIs3D)
				{
					ZumasRevenge.Common._S(1f);
					ZumasRevenge.Common._DS(1f);
					float num12 = ((GameApp.mGameRes == 768) ? 0.36667f : ((GameApp.mGameRes == 640) ? 0.26667f : 0.53333f));
					float num13 = ((GameApp.mGameRes == 768) ? 360f : ((GameApp.mGameRes == 640) ? 210f : 0f));
					sexyTransform2D2.Translate(ZumasRevenge.Common._S(-469.5f) + (float)this.mApp.mScreenBounds.mX, ZumasRevenge.Common._S(-382.5f));
					sexyTransform2D2.RotateRad((float)this.mIntroRotate.GetOutVal());
					sexyTransform2D2.Scale((float)this.mIntroMidScale.GetOutVal(), (float)this.mIntroMidScale.GetOutVal());
					sexyTransform2D2.Translate(ZumasRevenge.Common._S(469.5f) - (float)this.mApp.mScreenBounds.mX, ZumasRevenge.Common._S(382.5f));
					sexyTransform2D2.Translate((float)(this.mIntroMidTransX.GetOutVal() * (double)num12) + num13, 0f);
					graphics3D2.PushTransform(sexyTransform2D2);
					g.SetColor(this.mIntroMidAlpha);
					g.DrawImage(Res.GetImageByID(ResID.IMAGE_UI_MAP_ZOOM), ZumasRevenge.Common._S(-80), 0);
					graphics3D2.PopTransform();
					if (flag2)
					{
						sexyTransform2D2.LoadIdentity();
						sexyTransform2D2.Translate((float)ZumasRevenge.Common._S(-450), (float)ZumasRevenge.Common._S(-500));
						sexyTransform2D2.Scale((float)this.mIntroFrogScale.GetOutVal(), (float)this.mIntroFrogScale.GetOutVal());
						sexyTransform2D2.Translate((float)ZumasRevenge.Common._S(450), (float)ZumasRevenge.Common._S(500));
						sexyTransform2D2.Translate(ZumasRevenge.Common._S(-468.75f) + (float)this.mApp.mScreenBounds.mX, ZumasRevenge.Common._S(-382.5f));
						sexyTransform2D2.Scale((float)(this.mIntroMidScale.GetOutVal() / 4.0), (float)(this.mIntroMidScale.GetOutVal() / 4.0));
						sexyTransform2D2.RotateRad((float)(this.mIntroRotate.GetOutVal() + (double)ZumasRevenge.Common._M(0.265f)));
						sexyTransform2D2.Translate(ZumasRevenge.Common._S(468.75f) - (float)this.mApp.mScreenBounds.mX, ZumasRevenge.Common._S(382.5f));
						float num14 = ((GameApp.mGameRes == 768) ? 2.5f : ((GameApp.mGameRes == 640) ? 2.5f : 0.53333f));
						sexyTransform2D2.Translate((float)(this.mIntroMidTransX.GetOutVal() * (double)num14), 0f);
						sexyTransform2D2.Translate(ZumasRevenge.Common._S(0.25f), ZumasRevenge.Common._S(0.25f));
						graphics3D2.PushTransform(sexyTransform2D2);
					}
				}
				if (this.mFrogFlyOff != null)
				{
					this.mFrogFlyOff.Draw(g);
				}
				if (this.mIntroMidAlpha > 0.0 && this.mShowMapScreen && GlobalMembers.gIs3D && flag2)
				{
					graphics3D2.PopTransform();
				}
				if (this.mIntroDialog.back<SimpleFadeText>().mAlpha >= 255f && !this.mDoIntroFrogJump && Board.gIntroRibbitTimer <= 0 && !this.mShowMapScreen)
				{
					g.SetFont(Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_STROKE));
					g.SetColor(ZumasRevenge.Common._M(255), ZumasRevenge.Common._M1(255), ZumasRevenge.Common._M2(255));
					g.WriteString(TextManager.getInstance().getString(433), 0, ZumasRevenge.Common._DS(ZumasRevenge.Common._M(1170)), this.mWidth, 0);
				}
				if (this.mIntroMidAlpha > 0.0 && this.mShowMapScreen && !GlobalMembers.gIs3D && this.mIntroMidAlpha > 0.0 && (this.mIntroMidAlpha < 1.0 || this.mIntroMapAlpha == 0.0))
				{
					g.SetColor(0, 0, 0, (int)(255.0 * this.mIntroMidAlpha));
					g.FillRect(this.mApp.GetScreenRect());
				}
				if (this.mShowMapScreen)
				{
					g.mTransX = 0f;
					if (GlobalMembers.gIs3D)
					{
						sexyTransform2D2.LoadIdentity();
						float num16;
						if (this.mDoingFirstTimeIntro && this.mDoingFirstTimeIntroZoomToGame && this.mShowMapScreen)
						{
							float num15 = ((GameApp.mGameRes == 768) ? 0.26667f : ((GameApp.mGameRes == 640) ? 0.26667f : 0.53333f));
							num16 = Math.Max((float)this.mIntroMapScale * (num15 * 5f), 1f);
						}
						else
						{
							float num17 = ((GameApp.mGameRes == 768) ? 0.26667f : ((GameApp.mGameRes == 640) ? 0.26667f : 0.53333f));
							num16 = Math.Max((float)this.mIntroMapScale * (num17 * 16f), 1f);
						}
						int mX = this.mApp.mScreenBounds.mX;
						sexyTransform2D2.Translate(ZumasRevenge.Common._S(-710.75f), ZumasRevenge.Common._S(-372.5f));
						sexyTransform2D2.Scale(num16, num16);
						sexyTransform2D2.Translate(ZumasRevenge.Common._S(710.75f), ZumasRevenge.Common._S(372.5f));
						sexyTransform2D2.Translate(ZumasRevenge.Common._S((float)this.mIntroMapTransX.GetOutVal()), 0f);
						graphics3D2.PushTransform(sexyTransform2D2);
					}
					if (graphics3D2 == null && this.mIntroMapAlpha < 1.0 && this.mIntroMapAlpha > 0.0)
					{
						g.SetColor(0, 0, 0, 255);
						g.FillRect(this.mApp.GetScreenRect());
					}
					if (graphics3D2 != null || this.mIntroMapAlpha.GetOutVal() > 0.0)
					{
						g.SetColorizeImages(GlobalMembers.gIs3D);
						this.mMapScreen.mAlpha.SetConstant(this.mIntroMapAlpha);
						this.mMapScreen.Draw(g);
					}
					if (GlobalMembers.gIs3D)
					{
						graphics3D2.PopTransform();
					}
				}
			}
			if (this.mIntroFadeAmt > 0f)
			{
				g.SetColor(0, 0, 0, (int)this.mIntroFadeAmt);
				g.SetClipRect(new Rect(-1, -1, 1067, 641));
				g.FillRect(new Rect(-1, -1, 1067, 641));
				g.SetClipRect(new Rect(0, 0, 1066, 640));
			}
			if (this.mTransitionScreenHolePct.mRamp == 6 && !this.mTransitionScreenHolePct.HasBeenTriggered())
			{
				Graphics3D graphics3D3 = g.Get3D();
				SexyTransform2D sexyTransform2D3 = new SexyTransform2D(false);
				sexyTransform2D3.Translate((float)(-(float)ZumasRevenge.Common._S(this.mTransitionCenter.mX) - this.mApp.mScreenBounds.mX), (float)(-(float)ZumasRevenge.Common._S(this.mTransitionCenter.mY)));
				sexyTransform2D3.Scale((float)this.mTransitionScreenScale.GetOutVal(), (float)this.mTransitionScreenScale.GetOutVal());
				sexyTransform2D3.Translate((float)(ZumasRevenge.Common._S(this.mTransitionCenter.mX) + this.mApp.mScreenBounds.mX), (float)ZumasRevenge.Common._S(this.mTransitionCenter.mY));
				graphics3D3.PushTransform(sexyTransform2D3);
				int mWidth = this.mApp.mScreenBounds.mWidth;
				int mHeight = this.mApp.mScreenBounds.mHeight;
				int num18 = ZumasRevenge.Common._S(this.mTransitionCenter.mX);
				int num19 = ZumasRevenge.Common._S(this.mTransitionCenter.mY);
				float num20 = (float)((double)ZumasRevenge.Common._M(0.01f) + (double)ZumasRevenge.Common._M1(0.4f) / (this.mTransitionScreenHolePct + (double)ZumasRevenge.Common._M2(0.003f)));
				SexyVertex2D[] theVertices = new SexyVertex2D[]
				{
					new SexyVertex2D(-1f, -1f, (float)(0.5 + (double)(-(double)((float)num18 + g.mTransX)) / (double)mWidth * (double)num20), (float)(0.5 + (double)(-(double)num19) / (double)mWidth * (double)num20)),
					new SexyVertex2D((float)mWidth, -1f, (float)(0.5 + (double)((float)(mWidth - num18) - g.mTransX) / (double)mWidth * (double)num20), (float)(0.5 + (double)(-(double)num19) / (double)mWidth * (double)num20)),
					new SexyVertex2D(-1f, (float)mHeight, (float)(0.5 + (double)(-(double)((float)num18 + g.mTransX)) / (double)mWidth * (double)num20), (float)(0.5 + (double)(mHeight - num19) / (double)mWidth * (double)num20)),
					new SexyVertex2D((float)mWidth, (float)mHeight, (float)(0.5 + (double)((float)(mWidth - num18) - g.mTransX) / (double)mWidth * (double)num20), (float)(0.5 + (double)(mHeight - num19) / (double)mWidth * (double)num20))
				};
				graphics3D3.SetTextureWrap(0, false);
				graphics3D3.SetTexture(0, Res.GetImageByID(ResID.IMAGE_TRANSPARENT_HOLE));
				graphics3D3.SetTextureLinearFilter(0, true);
				graphics3D3.DrawPrimitive(0U, Graphics3D.EPrimitiveType.PT_TriangleStrip, theVertices, 2, new SexyColor(255, 255, 255, 255), 0, 0f, 0f, false, 0U);
				graphics3D3.PopTransform();
			}
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00043C3B File Offset: 0x00041E3B
		public void CheckForExtraLifeFromBoss()
		{
			if (!this.mNeedsBossExtraLife)
			{
				return;
			}
			this.mNeedsBossExtraLife = false;
			if (this.mLevel.mZone == 6)
			{
				return;
			}
			this.LivesChanged(1);
			this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_STOMP));
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00043C78 File Offset: 0x00041E78
		public void UpdateExtraLivesInfo()
		{
			if (this.mLivesInfo == null)
			{
				return;
			}
			if (this.mLivesInfo.IsDone())
			{
				this.mLivesInfo.Dispose();
				this.mLivesInfo = null;
				return;
			}
			this.mLivesInfo.Update();
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00043CB0 File Offset: 0x00041EB0
		public void DrawChallengeStats(Graphics g)
		{
			if (this.mGauntletRetryBtn == null)
			{
				return;
			}
			if (this.mGauntletAlpha < 255f)
			{
				g.SetColorizeImages(true);
			}
			this.DrawChallengeStatsBackground(g);
			this.DrawChallengeStatsButtons(g);
			g.SetColorizeImages(false);
			this.DrawChallengeGoalScores(g);
			this.mChallengeHeaderText.Draw(g);
			this.DrawChallengeScoreDetails(g);
			this.DrawChallengeHighScores(g);
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00043D10 File Offset: 0x00041F10
		public void DrawChallengeStatsBackground(Graphics g)
		{
			g.SetColor(0, 0, 0, 128);
			g.FillRect(this.mApp.GetScreenRect());
			g.SetColor(255, 255, 255, (int)Math.Min(255f, this.mGauntletAlpha));
			ZumasRevenge.Common.DrawCommonDialogBacking(g, this.mCStatsFrame.mX, this.mCStatsFrame.mY, this.mCStatsFrame.mWidth, this.mCStatsFrame.mHeight);
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_ADVENTURE_STATS_BANNER);
			g.DrawImage(imageByID, this.mCStatsFrame.mX + (this.mCStatsFrame.mWidth - imageByID.GetWidth()) / 2, ZumasRevenge.Common._DS(140) - imageByID.GetHeight() / 2);
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00043DDC File Offset: 0x00041FDC
		public void DrawChallengeStatsButtons(Graphics g)
		{
			if ((this.mGauntletRetryBtn == null && this.mGauntletQuitBtn == null) || !MathUtils._geq(this.mGauntletAlpha, 255f))
			{
				return;
			}
			g.PushState();
			g.Translate(this.mGauntletRetryBtn.mX, this.mGauntletRetryBtn.mY);
			this.mGauntletRetryBtn.mBtnNoDraw = false;
			this.mGauntletRetryBtn.Draw(g);
			this.mGauntletRetryBtn.mBtnNoDraw = true;
			g.PopState();
			g.PushState();
			g.Translate(this.mGauntletQuitBtn.mX, this.mGauntletQuitBtn.mY);
			this.mGauntletQuitBtn.mBtnNoDraw = false;
			this.mGauntletQuitBtn.Draw(g);
			this.mGauntletQuitBtn.mBtnNoDraw = true;
			g.PopState();
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00043EA4 File Offset: 0x000420A4
		public void DrawChallengeGoalScores(Graphics g)
		{
			float num = 0.75f;
			int num2 = 0;
			int num3 = 0;
			if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_PL)
			{
				num = 0.55f;
				num2 = 180;
				num3 = 40;
			}
			int num4 = (int)((float)this.mCStatsFrame.mWidth * 0.48f);
			int theHeight = (int)((float)this.mCStatsFrame.mHeight * 0.44f);
			int num5 = (int)((float)this.mCStatsFrame.mX + (float)this.mCStatsFrame.mWidth * 0.07f);
			int num6 = (int)((float)this.mCStatsFrame.mY + (float)this.mCStatsFrame.mHeight * 0.39f);
			g.PushState();
			g.SetColor(255, 255, 255, (int)Math.Min(255f, this.mGauntletAlpha));
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_STROKE));
			g.SetScale(num, num, (float)num5, (float)num6);
			g.WriteWordWrapped(new Rect(num5, num6 + num3, num4 + num2, theHeight), this.GetGoalScoresInfo(), -1, 0);
			g.PopState();
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00043FAC File Offset: 0x000421AC
		public string GetGoalScoresInfo()
		{
			int num = (this.mLevel.mZone - 1) * 10 + this.mLevel.mNum;
			if (this.mLevel.mIronFrog)
			{
				num = 1;
				return string.Concat(new object[]
				{
					"Iron Frog ",
					num,
					"\nChallenge:\n",
					SexyFramework.Common.CommaSeperate(this.mLevel.mChallengePoints),
					"\nAce:\n",
					SexyFramework.Common.CommaSeperate(this.mLevel.mChallengeAcePoints)
				});
			}
			StringBuilder stringBuilder = new StringBuilder(TextManager.getInstance().getString(684));
			stringBuilder.Replace("$1", num.ToString());
			stringBuilder.Replace("$2", SexyFramework.Common.CommaSeperate(this.mLevel.mChallengePoints));
			stringBuilder.Replace("$3", SexyFramework.Common.CommaSeperate(this.mLevel.mChallengeAcePoints));
			return stringBuilder.ToString();
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x000440A0 File Offset: 0x000422A0
		public void DrawChallengeScoreDetails(Graphics g)
		{
			int mWidth = this.mCStatsFrame.mWidth;
			int num = (int)((float)this.mCStatsFrame.mHeight * 0.25f);
			int mX = this.mCStatsFrame.mX;
			int theY = (int)((float)this.mCStatsFrame.mY + (float)this.mCStatsFrame.mHeight * 0.13f);
			Rect rect = new Rect(mX, theY, mWidth, (int)((float)num * 0.25f));
			Rect theRect = new Rect(rect);
			theRect.mY += rect.mHeight * 3;
			string challengeScoreFlavorText = this.GetChallengeScoreFlavorText();
			StringBuilder stringBuilder = new StringBuilder(TextManager.getInstance().getString(685));
			stringBuilder.Replace("$1", SexyFramework.Common.CommaSeperate(this.mGauntletPointsFromMult));
			stringBuilder.Replace("$2", this.mScoreMultiplier.ToString());
			string theLine = stringBuilder.ToString();
			g.SetColor(255, 255, 255, (int)this.mChallengeTextAlpha);
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGLOUNGE45_GAUNTLET));
			g.WriteWordWrapped(rect, challengeScoreFlavorText, -1, 0);
			this.mChallengePtsText.Draw(g);
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_GAUNTLET));
			g.WriteWordWrapped(theRect, theLine, -1, 0);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x000441E0 File Offset: 0x000423E0
		public string GetChallengeScoreFlavorText()
		{
			if (this.mScore < this.mLevel.mChallengePoints)
			{
				return TextManager.getInstance().getString(686);
			}
			if (this.mScore < this.mLevel.mChallengeAcePoints)
			{
				return TextManager.getInstance().getString(687);
			}
			return TextManager.getInstance().getString(688);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00044244 File Offset: 0x00042444
		public void DrawChallengeHighScores(Graphics g)
		{
			List<Rect> challengeScoresTable = this.GetChallengeScoresTable();
			List<GauntletHSInfo> inEntries = new List<GauntletHSInfo>();
			this.mApp.mUserProfile.GetGauntletHighScores((this.mLevel.mZone - 1) * 10 + this.mLevel.mNum, ref inEntries);
			int num = ZumaProfile.MAX_GAUNTLET_HIGH_SCORES;
			if (this.mGauntletHSIndex < 5)
			{
				num++;
			}
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_GAUNTLET2));
			this.DrawHighScoresHeader(g, challengeScoresTable.back<Rect>());
			for (int i = 0; i < num; i++)
			{
				if (i != 5 || this.mScoreDisplayPos > 4f)
				{
					List<Rect> challengeScoresTableRow = this.GetChallengeScoresTableRow(i, challengeScoresTable);
					this.DrawHighScoreEntry(g, i, inEntries, challengeScoresTableRow);
				}
			}
			this.DrawScoreExplosion(g, this.GetScoreRowColor(4));
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x000442FC File Offset: 0x000424FC
		public List<Rect> GetChallengeScoresTable()
		{
			int num = (int)((float)this.mCStatsFrame.mWidth * 0.48f);
			int num2 = (int)((float)this.mCStatsFrame.mHeight * 0.4f);
			int num3 = (int)((float)this.mCStatsFrame.mX + (float)this.mCStatsFrame.mWidth * 0.45f);
			int num4 = (int)((float)this.mCStatsFrame.mY + (float)this.mCStatsFrame.mHeight * 0.42f);
			num3 -= 60;
			if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CHT)
			{
				num3 -= 40;
			}
			Rect rect = new Rect(num3, num4, num, (int)((float)num2 * 0.2f));
			int theY = num4 + rect.mHeight;
			int theHeight = num2 - rect.mHeight;
			int num5 = (int)((float)num * 0.15f);
			int num6 = (int)((float)num * 0.325f);
			int num7 = (int)((float)num * 0.05f);
			List<Rect> list = new List<Rect>();
			list.Resize(4);
			int num8 = 340;
			int num9 = 45;
			if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_RU || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_PL)
			{
				num8 = 350;
				num9 = 55;
			}
			if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CHT)
			{
				num8 = 380;
			}
			for (int i = 0; i < 4; i++)
			{
				if (i == 0)
				{
					list[i] = new Rect(num3, theY, num5, theHeight);
				}
				else
				{
					int num10 = list[i - 1].mX + list[i - 1].mWidth;
					int num11 = ((i == 3) ? num5 : num6);
					num11 = ((i == 1) ? num8 : num11);
					if (i == 1)
					{
						num10 += num7;
					}
					if (i == 2)
					{
						num10 -= num9;
					}
					list[i] = new Rect(num10, theY, num11, theHeight);
				}
			}
			list.Add(rect);
			return list;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x000444CC File Offset: 0x000426CC
		public void DrawHighScoresHeader(Graphics g, Rect inFrame)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_GAUNTLET_HIGHSCORES);
			int num = (int)((float)inFrame.mX + (float)(inFrame.mWidth - imageByID.GetWidth()) * 0.5f);
			int theY = inFrame.mY + (inFrame.mHeight - imageByID.GetHeight());
			g.DrawImage(imageByID, num + 80, theY);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00044528 File Offset: 0x00042728
		public List<Rect> GetChallengeScoresTableRow(int inRowIdx, List<Rect> inColumns)
		{
			int num = (int)((float)inColumns[0].mHeight / (float)ZumaProfile.MAX_GAUNTLET_HIGH_SCORES);
			int scoreRowYPosition = this.GetScoreRowYPosition(inRowIdx, num, inColumns[0].mY);
			List<Rect> list = new List<Rect>();
			for (int i = 0; i < 4; i++)
			{
				list.Add(new Rect(inColumns[i].mX, scoreRowYPosition, inColumns[i].mWidth, num));
			}
			return list;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00044598 File Offset: 0x00042798
		public int GetScoreRowYPosition(int inRowIdx, int inRowHeight, int inTableY)
		{
			float scoreRowBumpOffset = this.GetScoreRowBumpOffset();
			float num = 0f;
			int num2 = inRowIdx;
			if (inRowIdx == this.mGauntletHSIndex)
			{
				num2 = (int)this.mScoreDisplayPos;
				num = scoreRowBumpOffset;
			}
			else if (inRowIdx > this.mGauntletHSIndex)
			{
				int num3 = (int)this.mScoreDisplayPos + 1;
				if (inRowIdx == num3 && inRowIdx < 5)
				{
					num = 1f - scoreRowBumpOffset;
				}
				if (inRowIdx <= num3)
				{
					num2--;
				}
			}
			int result;
			if (inRowIdx == this.mGauntletHSIndex && this.mScoreDisplayPos > 4f)
			{
				result = inTableY + (int)((float)(4 * inRowHeight) + (this.mScoreDisplayPos - 4f) * ZumasRevenge.Common._S(200f));
			}
			else
			{
				result = inTableY + (int)(((float)num2 + num) * (float)inRowHeight);
			}
			return result;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00044640 File Offset: 0x00042840
		public float GetScoreRowBumpOffset()
		{
			float num = this.mScoreDisplayPos - (float)((int)this.mScoreDisplayPos);
			return (float)((1.0 - Math.Cos(Math.Min(0.5, (double)(num * 1.25f)) * 3.14159 * 2.0)) * 0.5);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x000446A4 File Offset: 0x000428A4
		public void DrawHighScoreEntry(Graphics g, int inEntryIdx, List<GauntletHSInfo> inEntries, List<Rect> inCells)
		{
			Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_GAUNTLET2);
			string theLine = (inEntryIdx + 1).ToString();
			string mProfileName = inEntries[inEntryIdx].mProfileName;
			string text = SexyFramework.Common.CommaSeperate(inEntries[inEntryIdx].mScore);
			if (inEntryIdx == 5)
			{
				this.ApplyExplosionEffectToScore(mProfileName, text, inCells);
			}
			g.PushState();
			g.SetColor(this.GetScoreRowColor(inEntryIdx));
			g.WriteWordWrapped(inCells[0], theLine, -1, 1);
			g.WriteWordWrapped(inCells[1], mProfileName);
			g.WriteWordWrapped(inCells[2], text, -1, 1);
			g.PopState();
			this.DrawCrown(g, inCells[3], inEntries[inEntryIdx].mScore);
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0004475C File Offset: 0x0004295C
		public void ApplyExplosionEffectToScore(string inName, string inScore, List<Rect> inCells)
		{
			this.mScoreBreakStrings[0] = inName;
			this.mScoreBreakPositions[0] = new SexyPoint(inCells[1].mX, inCells[1].mY);
			this.mScoreBreakStrings[1] = inScore;
			this.mScoreBreakPositions[1] = new SexyPoint(inCells[2].mX, inCells[2].mY);
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x000447C8 File Offset: 0x000429C8
		public SexyColor GetScoreRowColor(int inRowIdx)
		{
			SexyColor result;
			if (inRowIdx == this.mGauntletHSIndex)
			{
				result = new SexyColor((int)this.mApp.HSLToRGB((int)((float)this.mUpdateCnt / 0.2f) % 255, 255, 220));
				result.mAlpha = (int)Math.Min(255f, this.mGauntletAlpha);
			}
			else
			{
				result = new SexyColor(255, 255, 255, (int)Math.Min(255f, this.mGauntletAlpha));
			}
			return result;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00044854 File Offset: 0x00042A54
		public void DrawCrown(Graphics g, Rect inFrame, int inScore)
		{
			Image imageByID;
			if (inScore >= this.mLevel.mChallengeAcePoints)
			{
				imageByID = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_LARGE_ACECROWN);
			}
			else
			{
				if (inScore < this.mLevel.mChallengePoints)
				{
					return;
				}
				imageByID = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_LARGE_CROWN);
			}
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_LARGE_CROWN);
			float num = 0.3f;
			int num2 = (int)((float)imageByID2.GetWidth() * num);
			int num3 = (int)((float)imageByID2.GetHeight() * num);
			int theX = (int)((float)inFrame.mX + (float)(inFrame.mWidth - num2) * 0.5f);
			int num4 = inFrame.mY + (inFrame.mHeight - num3);
			g.DrawImage(imageByID, theX, num4 - ZumasRevenge.Common._DS(8), num2, num3);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00044908 File Offset: 0x00042B08
		public void DrawScoreExplosion(Graphics g, SexyColor inScoreColor)
		{
			for (int i = 0; i < this.mScoreLetterEffectVector.size<ScoreLetterEffect>(); i++)
			{
				ScoreLetterEffect scoreLetterEffect = this.mScoreLetterEffectVector[i];
				g.SetColor(inScoreColor);
				Graphics3D graphics3D = g.Get3D();
				SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
				if (graphics3D != null)
				{
					sexyTransform2D.Translate(-scoreLetterEffect.mX - ZumasRevenge.Common._S(5f), -scoreLetterEffect.mY - ZumasRevenge.Common._S(-8f));
					sexyTransform2D.RotateRad(scoreLetterEffect.mRot);
					sexyTransform2D.Translate(scoreLetterEffect.mX + ZumasRevenge.Common._S(5f), scoreLetterEffect.mY + ZumasRevenge.Common._S(-8f));
					graphics3D.PushTransform(sexyTransform2D);
				}
				g.DrawString(string.Concat(scoreLetterEffect.mChar), (int)scoreLetterEffect.mX, (int)scoreLetterEffect.mY);
				if (graphics3D != null)
				{
					graphics3D.PopTransform();
				}
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x000449F4 File Offset: 0x00042BF4
		public void DrawGuide(Graphics g)
		{
			if (!this.gDrawAutoAimAssistInfo || this.mGameState != GameState.GameState_Playing || this.mFrog.GetBullet() == null || this.IsPaused() || this.mFrog.GetType() != 0)
			{
				return;
			}
			if (this.mShowGuide || this.mAccuracyCount > 0)
			{
				int num = 128;
				int theColor = 65535;
				Graphics3D graphics3D = g.Get3D();
				if (graphics3D == null || ZumasRevenge.Common._M(1) == 0)
				{
					if (this.mFrog.GetBullet() != null)
					{
						int colorType = this.mFrog.GetBullet().GetColorType();
						theColor = ZumasRevenge.Common.gBallColors[colorType];
						if (this.mApp.mColorblind && colorType == 3)
						{
							theColor = 8421504;
						}
						else if (this.mApp.mColorblind && colorType == 4)
						{
							theColor = 1973790;
						}
					}
					g.SetColor(new SexyColor(theColor, num));
					g.PolyFill(this.mGuide, 4, false);
					return;
				}
				SexyVertex2D[] array = new SexyVertex2D[3];
				array[0].x = (float)this.mGuide[0].mX;
				array[0].y = (float)this.mGuide[0].mY;
				array[1].x = (float)this.mGuide[1].mX;
				array[1].y = (float)this.mGuide[1].mY;
				array[2].x = (float)this.mGuide[2].mX;
				array[2].y = (float)this.mGuide[2].mY;
				for (int i = 0; i < 3; i++)
				{
					array[i].u = 0.5f;
					array[i].v = 0.5f;
				}
				SexyColor[] array2 = new SexyColor[]
				{
					new SexyColor(0, 80, 255),
					new SexyColor(255, 255, 0),
					new SexyColor(255, 0, 0),
					new SexyColor(ZumasRevenge.Common._M(0), ZumasRevenge.Common._M1(200), ZumasRevenge.Common._M2(0)),
					new SexyColor(175, 0, 255),
					new SexyColor(255, 255, 255)
				};
				SexyColor theColor2 = ((this.mFrog.GetBullet() == null) ? new SexyColor(SexyColor.White) : array2[this.mFrog.GetBullet().GetColorType()]);
				if (this.mApp.mColorblind && this.mFrog.GetBullet().GetColorType() == 3)
				{
					theColor2 = new SexyColor(128, 128, 128);
				}
				else if (this.mApp.mColorblind && this.mFrog.GetBullet().GetColorType() == 4)
				{
					theColor2 = new SexyColor(30, 30, 30);
				}
				if (this.mFrog.GetBullet() != null && this.mFrog.GetBullet().GetColorType() == 1)
				{
					num /= ZumasRevenge.Common._M(2);
				}
				theColor2.mAlpha = num;
				Ratio aspectRatio = this.mApp.mGraphicsDriver.GetAspectRatio();
				int num2 = ((aspectRatio.mNumerator != 4 && aspectRatio.mDenominator != 3) ? ZumasRevenge.Common._S(80) : 0);
				num2 += ZumasRevenge.Common._DS(ZumasRevenge.Common._M(0));
				ZumasRevenge.Common._DS(ZumasRevenge.Common._M(0));
				if (this.mFrog.mShotCorrectionTarget.x != 0f || this.mFrog.mShotCorrectionTarget.y != 0f)
				{
					SexyPoint[] theVertexList = new SexyPoint[]
					{
						this.mGuide[0],
						this.mGuide[1],
						new SexyPoint((int)ZumasRevenge.Common._S(this.mFrog.mShotCorrectionTarget.x), (int)ZumasRevenge.Common._S(this.mFrog.mShotCorrectionTarget.y))
					};
					g.SetColor(200, 200, 200, 200);
					g.PolyFill(theVertexList, 3, false);
					g.SetColor(255, 255, 255, 255);
					g.DrawRect((int)ZumasRevenge.Common._S(this.mFrog.mShotCorrectionTarget.x - 2f), (int)ZumasRevenge.Common._S(this.mFrog.mShotCorrectionTarget.y - 2f), (int)ZumasRevenge.Common._S(4f), (int)ZumasRevenge.Common._S(4f));
				}
				Image imageByID = Res.GetImageByID(ResID.IMAGE_FROG_ACCURACY_GUIDE);
				g.DrawTrianglesTexStrip(imageByID, array, 1, theColor2, 0, g.mTransX, g.mTransY, true);
				g.DrawTrianglesTexStrip(imageByID, array, 1, theColor2, 1, g.mTransX, g.mTransY, true);
				array[0].x = (float)this.mGuide[3].mX;
				array[0].y = (float)this.mGuide[3].mY;
				g.DrawTrianglesTexStrip(imageByID, array, 1, theColor2, 0, g.mTransX, g.mTransY, true);
				g.DrawTrianglesTexStrip(imageByID, array, 1, theColor2, 1, g.mTransX, g.mTransY, true);
			}
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00044F60 File Offset: 0x00043160
		public override void MouseLeave()
		{
			base.MouseLeave();
			this.mApp.SetCursor(ECURSOR.CURSOR_POINTER);
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00044F74 File Offset: 0x00043174
		public override void MouseMove(int x, int y)
		{
			base.MouseMove(x, y);
			if (this.ShouldBlockInput())
			{
				return;
			}
			if (this.mShowMapScreen)
			{
				this.mMapScreen.MouseMove(x, y);
			}
			if (this.mPauseCount > 0 || this.mLevel == null || !this.mLevel.CanRotateFrog() || this.ShouldBlockInput() || this.mDoingFirstTimeIntro || this.mGameState == GameState.GameState_BossIntro || this.mGameState == GameState.GameState_Losing || this.mGameState == GameState.GameState_BossDead)
			{
				return;
			}
			if (this.mControlMode == Board.CONTROL_MODE.CONTROL_MODE_DODGING)
			{
				if (this.IsPointAlongSlider(x, y))
				{
					this.mFatFingerGuideEnabled = false;
				}
				else
				{
					this.mControlMode = Board.CONTROL_MODE.CONTROL_MODE_AIMING;
				}
			}
			else if (this.mControlMode == Board.CONTROL_MODE.CONTROL_MODE_SWAPPING)
			{
				if (this.IsTouchOnFrogGun(x, y))
				{
					if (!this.mInvalidateTouchUp)
					{
						this.EnableHaloSwap(false);
					}
					this.mFatFingerGuideEnabled = false;
					return;
				}
				if (this.IsPointAlongSlider(x, y))
				{
					this.mControlMode = Board.CONTROL_MODE.CONTROL_MODE_DODGING;
					this.DisableHaloSwap(false);
				}
				else
				{
					this.mControlMode = Board.CONTROL_MODE.CONTROL_MODE_AIMING;
					this.DisableHaloSwap(false);
				}
			}
			if (this.mControlMode == Board.CONTROL_MODE.CONTROL_MODE_AIMING)
			{
				this.mFatFingerGuideEnabled = true;
				this.DisableHaloSwap(false);
			}
			float num = this.mFrog.GetDestAngle();
			float angle = this.mFrog.GetAngle();
			if (this.mControlMode != Board.CONTROL_MODE.CONTROL_MODE_SWAPPING && this.mControlMode != Board.CONTROL_MODE.CONTROL_MODE_NONE)
			{
				this.UpdateGunPos();
			}
			if (this.mTransitionScreenImage != null)
			{
				if (num >= 3.14159f)
				{
					num -= 6.28318f;
				}
				if (this.mFrog.mDestAngle >= 3.14159f)
				{
					this.mFrog.mDestAngle -= 6.28318f;
				}
				if (num < -3.14159f && this.mFrog.GetDestAngle() > 3.14159f)
				{
					this.mFrog.mDestAngle -= 6.28318f;
				}
				else if (num > 3.14159f && this.mFrog.GetDestAngle() < -3.14159f)
				{
					this.mFrog.mDestAngle += 6.28318f;
				}
				this.mFrog.mAngle = angle;
			}
			if (this.mFatFingerGuideEnabled || this.mInvalidateTouchUp)
			{
				return;
			}
			if (this.mLevel.mNumFrogPoints > 1 && this.mGameState == GameState.GameState_Playing)
			{
				int gunPointFromPos = this.mLevel.GetGunPointFromPos(ZumasRevenge.Common._SS(x), ZumasRevenge.Common._SS(y));
				if (gunPointFromPos >= 0 && gunPointFromPos != this.mLevel.mCurFrogPoint)
				{
					this.mMouseOverGunPos = gunPointFromPos;
				}
			}
			this.mMouseOverGunPos = -1;
			int num2 = ((this.mLevel.mBoss == null) ? 0 : this.mLevel.mBoss.mDeathText.size<BossText>());
			if (this.mGameState != GameState.GameState_BossDead || num2 == 0 || this.mLevel.mBoss.mDeathText[num2 - 1].mAlpha < 255f)
			{
				this.mApp.SetCursor((this.mMouseOverGunPos >= 0) ? ECURSOR.CURSOR_HAND : ECURSOR.CURSOR_POINTER);
			}
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00045231 File Offset: 0x00043431
		public override void MouseDrag(int x, int y)
		{
			base.MouseDrag(x, y);
			this.MouseMove(x, y);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00045244 File Offset: 0x00043444
		public override void MouseDown(int x, int y, int theClickCount)
		{
			if (this.ShouldBlockInput() || this.DoingLilyPadTutorial(x, y))
			{
				return;
			}
			if (this.mGameState == GameState.GameState_BossDead)
			{
				return;
			}
			this.mIsMouseDown = true;
			this.DisableHaloSwap(false);
			if (this.IsTouchOnFrogGun(x, y))
			{
				this.mFatFingerGuideEnabled = false;
				this.EnableHaloSwap(true);
				this.mInitialTouchPoint.mX = x;
				this.mInitialTouchPoint.mY = y;
				this.mControlMode = Board.CONTROL_MODE.CONTROL_MODE_SWAPPING;
			}
			else if (this.IsPointAlongSlider(x - this.mApp.mBoardOffsetX, y))
			{
				this.UpdateGunPos();
				this.mControlMode = Board.CONTROL_MODE.CONTROL_MODE_DODGING;
			}
			else if (this.mLevel.mNumFrogPoints > 1 && this.mLevel.CanRotateFrog())
			{
				this.mFatFingerGuideEnabled = true;
				this.mControlMode = Board.CONTROL_MODE.CONTROL_MODE_AIMING;
				int gunPointFromPos = this.mLevel.GetGunPointFromPos(ZumasRevenge.Common._SS(x - 80), ZumasRevenge.Common._SS(y));
				if (gunPointFromPos >= 0 && gunPointFromPos != this.mLevel.mCurFrogPoint)
				{
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_LILLYPAD_JUMP));
					this.mLevel.mCurFrogPoint = gunPointFromPos;
					this.mFrog.SetDestPos(this.mLevel.mFrogX[gunPointFromPos], this.mLevel.mFrogY[gunPointFromPos], this.mLevel.mMoveSpeed, true);
					this.mLevel.MouseDown(ZumasRevenge.Common._SS(x), ZumasRevenge.Common._SS(y), theClickCount);
					this.mLevel.ChangedPad(gunPointFromPos);
					this.mInvalidateTouchUp = true;
					this.mMouseOverGunPos = gunPointFromPos;
					this.mLevel.m_canGetAchievementNoJump = false;
				}
			}
			else
			{
				this.mFatFingerGuideEnabled = true;
				this.mControlMode = Board.CONTROL_MODE.CONTROL_MODE_AIMING;
			}
			if (this.mControlMode != Board.CONTROL_MODE.CONTROL_MODE_SWAPPING)
			{
				this.UpdateGunPos();
			}
			base.MouseDown(x, y, theClickCount);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x000453FC File Offset: 0x000435FC
		public override void MouseUp(int x, int y, int theClickCount)
		{
			this.mMouseOverGunPos = -1;
			this.mFatFingerGuideEnabled = false;
			if (this.mUpdateCnt < 10)
			{
				return;
			}
			base.MouseUp(x, y, theClickCount);
			if (this.mZumaTips.size<ZumaTip>() > 0 && this.mZumaTips[0].mClickDismiss && this.mZumaTips[0].mId == ZumaProfile.FRUIT_HINT && JeffLib.Common.RightClick(theClickCount))
			{
				return;
			}
			if (this.mPauseCount > 0)
			{
				return;
			}
			if (this.mDoingFirstTimeIntro)
			{
				if (this.mShowMapScreen)
				{
					if (this.mIntroMapScale == 0.0 && !this.mDoIntroFrogJump)
					{
						this.mDoingFirstTimeIntroZoomToGame = true;
						this.mIntroMapScale.SetCurve(ZumasRevenge.Common._MP("b;0,1,0.005714,1,####         ~~]L'"));
						this.mMapScreen.mClickToEnterAlpha.SetCurve(ZumasRevenge.Common._MP("b;0,1,0.04,1,~###         ~####"));
						if (GlobalMembers.gIs3D)
						{
							this.mMapScreen.mExtrasAlpha.SetCurve(ZumasRevenge.Common._MP("b;0,1,0.04,1,~###         ~####"));
						}
						this.mMapScreen.mIntroClosing = true;
						this.mMapScreen.mZoneOver = false;
						this.mApp.SetCursor(ECURSOR.CURSOR_POINTER);
						this.mApp.mUserProfile.mNeedsFirstTimeIntro = false;
						this.mDoIntroFrogJump = true;
						return;
					}
				}
				else if (Board.gIntroRibbitTimer <= 0 && !this.mDoIntroFrogJump)
				{
					this.mApp.mSoundPlayer.Fade(Res.GetSoundByID(ResID.SOUND_SEAGULLS), true);
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_MAPZOOMUP));
					this.mApp.PlaySong(12);
					this.mApp.SetCursor(ECURSOR.CURSOR_POINTER);
					this.SetupMapScreen(false, false);
					this.mIntroMidAlpha.SetCurve(ZumasRevenge.Common._MP("b+0,1,0.01,1,####   @~###      a~###"));
					this.mIntroMidScale.SetCurve(ZumasRevenge.Common._MP("b+0.8,4,0,1,~###  D~###     p-=t>V#### 7#P##"), this.mIntroMidAlpha);
					this.mIntroMidTransX.SetCurve(ZumasRevenge.Common._MP("b+0,410,0,1,####  T####     GLZ_]  (YUq?"), this.mIntroMidAlpha);
					this.mIntroMapTransX.SetCurve(ZumasRevenge.Common._MP("b+0,0,0,1,####         ~~###"), this.mIntroMidAlpha);
					if (GlobalMembers.gIs3D)
					{
						this.mIntroMapAlpha.SetCurve(ZumasRevenge.Common._MP("b+0,1,0,1,####    }####   M~### T~###"), this.mIntroMidAlpha);
					}
					else
					{
						this.mIntroMapAlpha.SetCurve(ZumasRevenge.Common._MP("b+0,1,0,1,####    i#### z~###   <~###"), this.mIntroMidAlpha);
					}
					this.mIntroMapScale.SetCurve(ZumasRevenge.Common._MP("b+0,1,0,1,~###  @~###    [KmD-   (#P##"), this.mIntroMidAlpha);
					this.mIntroRotate.SetCurve(ZumasRevenge.Common._MP("b+0.1,-0.265,0,1,~###  D~###       ]####"), this.mIntroMidAlpha);
					this.mIntroMapPinAlpha.SetCurve(ZumasRevenge.Common._MP("b+0,1,0,1,####       W####  I~###"), this.mIntroMidAlpha);
					this.mIntroFrogScale.SetCurve(ZumasRevenge.Common._MP("b+0,1,0,1,~###      J~###   V9###"), this.mIntroMidAlpha);
				}
				return;
			}
			if (this.mShowMapScreen && !this.mMapScreen.mClosing)
			{
				this.mMapScreen.MouseDown(x, y);
				if (this.mMapScreen.mClosing)
				{
					this.mHasDoneIntroSounds = false;
					this.mIntroFadeAmt = 0f;
					this.mIntroMapScale.SetCurve(ZumasRevenge.Common._MP("b;1,4,0.008,1,####         ~~Z{$"));
					if (this.mMapScreen.mClickToEnterAlpha > 0.0)
					{
						this.mMapScreen.mClickToEnterAlpha.SetCurve(ZumasRevenge.Common._MP("b;0,1,0.04,1,~###         ~####"));
					}
					if (GlobalMembers.gIs3D)
					{
						this.mMapScreen.mExtrasAlpha.SetCurve(ZumasRevenge.Common._MP("b;0,1,0.04,1,~###         ~####"));
					}
					this.mMapScreen.mIntroClosing = true;
					this.mMapScreen.mZoneOver = false;
					this.mApp.SetCursor(ECURSOR.CURSOR_POINTER);
					return;
				}
			}
			if (this.mLevelTransition != null && this.mLevelTransition.IsDone() && this.mLevelTransition.mTransitionToStats)
			{
				if (this.mStatsState < 2)
				{
					this.mStatsState = 2;
				}
				else if ((this.mLevel.mNum < 10 || this.mGameState == GameState.GameState_BossIntro) && this.mGameState == GameState.GameState_BossIntro)
				{
					if (this.mDoingBossIntroText)
					{
						return;
					}
					if (this.mBossIntroBGAlpha >= 0.5)
					{
						this.mDoingBossIntroText = true;
						this.mDoingBossIntroFightText = false;
						this.mBossIntroFramesLeft = ZumasRevenge.Common._M(20);
						this.mBossIntroDirection = 1;
						this.mBossIntroAlpha = 0f;
						this.mBossIntroAlphaRate = 255f / (float)this.mBossIntroFramesLeft;
						this.mBattleTextY = (float)this.mHeight;
						this.mBattleTextVY = ((float)(this.mHeight / 2 - ZumasRevenge.Common._DS(ZumasRevenge.Common._M(0))) - this.mBattleTextY) / (float)this.mBossIntroFramesLeft;
						this.mBossSmScale.SetConstant(1.0);
					}
					this.mLevelTransition.mTransitionToStats = false;
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_BOSS_BATTLE_INTRO));
					this.ContinueToNextLevel();
				}
			}
			bool flag = false;
			if (this.BlockInputForTutorial(x, y, out flag))
			{
				return;
			}
			if (this.mInvalidateTouchUp)
			{
				this.mInvalidateTouchUp = false;
				return;
			}
			if (this.mPauseUpdateCnt == this.mUpdateCnt || this.ShouldBlockInput() || this.mDoingEndBossFrogEffect)
			{
				return;
			}
			if (this.mGameState == GameState.GameState_BossDead && !this.mLevel.mBoss.mDoDeathExplosions)
			{
				int num = this.mLevel.mBoss.mDeathText.size<BossText>();
				if (num == 0 || this.mLevel.mBoss.mDeathText[num - 1].mAlpha >= 254f)
				{
					if (this.mLevel.mFinalLevel)
					{
						if (this.mAdventureWinScreen)
						{
							return;
						}
						this.SetMenuBtnEnabled(false);
						this.InitVortex();
						this.mAdventureWinScreen = true;
						this.mAdventureWinTimer = (float)ZumasRevenge.Common._M(50);
						this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvLevel = 1;
						this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvZone = 1;
					}
					else
					{
						this.mApp.SetCursor(ECURSOR.CURSOR_POINTER);
						this.mDoingEndBossFrogEffect = true;
						this.mEndBossFrogTimer = 0;
						this.mBossSmokePoof.ResetAnim();
						ZumasRevenge.Common.SetFXNumScale(this.mBossSmokePoof, this.mApp.Is3DAccelerated() ? 1f : ZumasRevenge.Common._M(0.25f));
						this.mEndBossFadeAmt = 0f;
					}
				}
				else
				{
					this.mLevel.mBoss.ShowAllDeathText();
					if (this.mStateCount < 400)
					{
						this.mStateCount = 399;
					}
					this.mLevel.mBoss.mDoDeathExplosions = false;
				}
			}
			if (this.mGameState != GameState.GameState_Playing)
			{
				return;
			}
			if (this.mLevel.mBoss != null && !this.mLevel.mBoss.AllowFrogToFire() && !this.GauntletMode())
			{
				this.mLevel.mBoss.MouseDownDuringNoFire(x, y);
				return;
			}
			bool flag2 = false;
			if (!ZumasRevenge.Common.gAddBalls && !this.mNeedsCheckpointIntro && !this.DoingIntros() && !this.mLevel.IsFinalBossLevel() && (this.mLevel.mBoss == null || !this.mLevel.mBoss.AllowFrogToFire()))
			{
				flag2 = true;
				this.mLevel.SkipInitialPathHilite();
				for (int i = 0; i < 2; i++)
				{
					FwooshImage fwooshImage = this.mLevelNameText[i];
					if (fwooshImage.mImage != null && fwooshImage.mAlpha != 0f && fwooshImage.mDelay > 0)
					{
						fwooshImage.mDelay = 1;
					}
				}
			}
			for (int j = 0; j < this.mLevel.mNumCurves; j++)
			{
				if (!this.mLevel.mCurveMgr[j].CanFire())
				{
					this.mLevel.MouseDown(x, y, theClickCount);
					return;
				}
			}
			if (this.mFrog.IsStunned())
			{
				this.mLevel.MouseDown(x, y, theClickCount);
				return;
			}
			if (flag2 && this.mLevel.CanFireBall() && (this.mZumaTips.size<ZumaTip>() == 0 || this.mZumaTips[0].mId != ZumaProfile.FIRST_SHOT_HINT))
			{
				return;
			}
			this.mLevel.MouseDown(x, y, theClickCount);
			if (this.IsTouchOnFrogGun(x, y))
			{
				this.DisableHaloSwap(true);
			}
			else
			{
				this.DisableHaloSwap(false);
			}
			if (this.mGameState != GameState.GameState_Playing || (this.GauntletMode() && this.mGauntletModeOver))
			{
				return;
			}
			if (ZumasRevenge.Common.gSuckMode && this.mFrog.GetBullet() == null && this.mGuideBall != null)
			{
				int colorType = this.mGuideBall.GetColorType();
				PowerType powerType = this.mGuideBall.GetPowerType();
				float x2 = this.mGuideBall.GetX();
				float y2 = this.mGuideBall.GetY();
				for (int k = 0; k < this.mLevel.mNumCurves; k++)
				{
					if (this.mLevel.mCurveMgr[k].RemoveBall(this.mGuideBall))
					{
						this.mFrog.Reload2(colorType, true, powerType, (int)x2, (int)y2);
						this.mGuideBall = null;
						break;
					}
				}
			}
			else if (this.mControlMode == Board.CONTROL_MODE.CONTROL_MODE_AIMING)
			{
				this.UpdateGunPos();
				if ((this.mLevel.CanFireBall() || flag) && this.mFrog.StartFire())
				{
					this.mAllowBulletDetection = true;
					this.mLevelStats.mTotalShots++;
					this.mApp.mUserProfile.mBallsFired++;
					this.mLevel.PlayerStartedFiring();
					this.KillActiveTutorial(ZumaProfile.FIRST_SHOT_HINT);
					if (!this.mFrog.LaserMode() && !this.mFrog.LightningMode())
					{
						this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BALLFIRE));
						if (this.mLevel.mZone == 5)
						{
							this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_UNDERWATER_FROGFIRE));
						}
					}
					else if (this.mGuideBall != null && !this.mGuideBall.GetIsExploding())
					{
						if (this.mFrog.LaserMode())
						{
							this.mFrog.DecLazerCount();
							if (this.mFrog.GetLazerCount() <= 0)
							{
								this.mShowGuide = false;
							}
							int l = 0;
							while (l < this.mLevel.mNumCurves)
							{
								int colorType2 = this.mGuideBall.GetColorType();
								float x3 = this.mGuideBall.GetX();
								float y3 = this.mGuideBall.GetY();
								if (this.mLevel.mCurveMgr[l].DoLazerExplosion(this.mGuideBall))
								{
									this.mFrog.SetBulletType(colorType2);
									this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_LAZER));
									PIEffect pieffect = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_LAZER_BLAST).Duplicate();
									ZumasRevenge.Common.SetFXNumScale(pieffect, 1f);
									pieffect.mDrawTransform.LoadIdentity();
									pieffect.mDrawTransform.Scale(ZumasRevenge.Common._DS(1.4f), ZumasRevenge.Common._DS(1.4f));
									pieffect.mDrawTransform.Translate(ZumasRevenge.Common._S(x3), ZumasRevenge.Common._S(y3));
									this.mLazerBlasts.Add(pieffect);
									if (this.mLevel.mZone == 5 && this.mLevel.mNum != 10 && this.mGameState == GameState.GameState_Playing)
									{
										for (int m = 0; m < ZumasRevenge.Common._M(5); m++)
										{
											Bubble bubble = new Bubble();
											bubble.Init((float)ZumasRevenge.Common._M(0), MathUtils.FloatRange(ZumasRevenge.Common._M1(-1.5f), ZumasRevenge.Common._M2(-0.75f)), MathUtils.FloatRange(ZumasRevenge.Common._M3(0.05f), ZumasRevenge.Common._M4(0.2f)), (int)MathUtils.FloatRange((float)ZumasRevenge.Common._M5(15), (float)ZumasRevenge.Common._M6(25)));
											bubble.SetAlphaFade(ZumasRevenge.Common._M(2f));
											bubble.SetX(x3 + (float)(-10 + MathUtils.SafeRand() % 20));
											bubble.SetY(y3);
											this.mFrog.AddBubble(bubble);
										}
										break;
									}
									break;
								}
								else
								{
									l++;
								}
							}
						}
						else if (this.mFrog.LightningMode())
						{
							this.mFrog.DoLightningFrog(false);
							this.mFrog.FireElectricOrb();
							this.mShowGuide = false;
							this.mLevel.DeactivateLightningEffects();
							int colorType3 = this.mGuideBall.GetColorType();
							int num2 = -1;
							for (int n = 0; n < this.mLevel.mNumCurves; n++)
							{
								if (this.mLevel.mCurveMgr[n].HasBall(this.mGuideBall))
								{
									num2 = n;
									break;
								}
							}
							this.mLevel.mCurveMgr[num2].DetonateBalls(colorType3, true, true);
							this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_LIGHTNING_FIRED));
						}
					}
					else if (this.mGuideBall == null && this.mFrog.LaserMode() && this.mLazerHitTreasure)
					{
						if (this.mFrog.LaserMode())
						{
							this.mFrog.DecLazerCount();
							if (this.mFrog.GetLazerCount() <= 0)
							{
								this.mShowGuide = false;
							}
						}
						this.DoHitTreasure();
					}
				}
			}
			else if (this.mControlMode == Board.CONTROL_MODE.CONTROL_MODE_SWAPPING)
			{
				this.SwapFrogBalls();
			}
			this.mControlMode = Board.CONTROL_MODE.CONTROL_MODE_NONE;
			this.mIsMouseDown = false;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00046100 File Offset: 0x00044300
		public void UnlockChallengeMode()
		{
			if (this.mLevel.mNum == 10 && this.mApp.mUserProfile.mChallengeUnlockState[this.mLevel.mZone - 1, 0] == 0)
			{
				this.mApp.mUserProfile.mNewChallengeCupUnlocked = true;
				for (int i = 0; i < 10; i++)
				{
					if (i < 2 && this.mApp.mUserProfile.mChallengeUnlockState[this.mLevel.mZone - 1, i] == 0)
					{
						this.mApp.mUserProfile.mChallengeUnlockState[this.mLevel.mZone - 1, i] = 2;
					}
					else if (i >= 2 && this.mApp.mUserProfile.mChallengeUnlockState[this.mLevel.mZone - 1, i] == 0)
					{
						this.mApp.mUserProfile.mChallengeUnlockState[this.mLevel.mZone - 1, i] = 1;
					}
				}
				if (!GameApp.USE_TRIAL_VERSION)
				{
					this.mChallengeCupUnlockedFX = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_GOLDSPARKLE_AREA_STATS);
					this.mChallengeCupUnlockedFX.ResetAnim();
					this.mChallengeCupUnlockedFX.mEmitAfterTimeline = true;
					this.mChallengeCupUnlockedFX.mDrawTransform.LoadIdentity();
					float num = GameApp.DownScaleNum(1f);
					this.mChallengeCupUnlockedFX.mDrawTransform.Scale(num, num);
					this.mChallengeCupUnlockedFX.mDrawTransform.Translate((float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(1060)), (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(906)));
					if (this.mLevel.mZone == 1)
					{
						this.ToggleNotification(TextManager.getInstance().getString(689), Res.GetSoundByID(ResID.SOUND_MIDZONE_NOTIFY));
						return;
					}
					this.ToggleNotification(TextManager.getInstance().getString(690), Res.GetSoundByID(ResID.SOUND_MIDZONE_NOTIFY));
				}
			}
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x000462E4 File Offset: 0x000444E4
		public void ButtonPress(int id)
		{
			if (this.mMenuButton != null && this.mMenuButton.mId == id)
			{
				if (this.mLevelTransition == null && this.mApp.mBambooTransition != null && !this.mApp.mBambooTransition.IsInProgress())
				{
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BUTTON1));
				}
				return;
			}
			this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BUTTON2));
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00046359 File Offset: 0x00044559
		public void ButtonPress(int theId, int count)
		{
			this.ButtonPress(theId);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00046364 File Offset: 0x00044564
		public void ButtonDepress(int id)
		{
			if (this.mApp.mBambooTransition != null && this.mApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			if (this.mCheckpointEffect != null)
			{
				return;
			}
			if (this.mLevelTransition == null && this.mMenuButton != null && this.mMenuButton.mId == id)
			{
				this.mApp.DoOptionsDialog(true);
				this.mMenuButton.mDisabled = true;
				return;
			}
			if (this.mIronFrogBtn != null && id == this.mIronFrogBtn.mId)
			{
				this.mApp.DoDeferredEndGame();
				return;
			}
			if (this.mAdvWinBtn != null && id == this.mAdvWinBtn.mId)
			{
				this.mAdvWinBtn = null;
				this.mApp.DoCredits(false);
				this.mForceRestartInAdvMode = true;
				this.mGameState = GameState.GameState_LevelUp;
				this.mLevelNum = 0;
				this.mApp.mUserProfile.GetAdvModeVars().mHighestZoneBeat = 6;
				if (!this.mApp.IsHardMode())
				{
					this.mApp.mUserProfile.mFirstTimeReplayingNormalMode = true;
				}
				else
				{
					this.mApp.mUserProfile.mFirstTimeReplayingHardMode = true;
				}
				this.mLives = 3;
				this.mScore = 0;
				this.mPointsLeftForExtraLife = this.mApp.GetLevelMgr().mPointsForLife;
				this.mRollerScore.ForceScore(0);
				return;
			}
			if (this.mStatsContinueBtn != null && id == this.mStatsContinueBtn.mId)
			{
				this.mApp.mSoundPlayer.Stop(Res.GetSoundByID(ResID.SOUND_NEW_ADV_STATS_TALLY));
				if (this.mLevel.mNum == 10 || this.mGameState == GameState.GameState_BossIntro)
				{
					if (GameApp.USE_TRIAL_VERSION)
					{
						if (GameApp.gApp.mBoard != null)
						{
							GameApp.gApp.mBoard.Pause(true, true);
						}
						string @string = TextManager.getInstance().getString(832);
						int width_pad = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(20));
						GameApp.gApp.DoYesNoDialog(TextManager.getInstance().getString(448), @string, true, TextManager.getInstance().getString(446), TextManager.getInstance().getString(447), false, ZumasRevenge.Common._S(ZumasRevenge.Common._M(50)), 1, width_pad);
						GameApp.gApp.mYesNoDialogDelegate = new GameApp.YesNoDialogDelegate(this.ProcessTrialYesNo);
						this.mIsTryAndBuyDialogShowing = true;
						return;
					}
					for (int i = 0; i < 40; i++)
					{
						int num = ((i < 20) ? ZumasRevenge.Common._M(230) : ZumasRevenge.Common._M1(575));
						int num2 = ZumasRevenge.Common._M(300);
						this.mSmokeParticles.Add(BambooTransition.SpawnSmokeParticle((float)num, (float)num2, false, false));
					}
					if (this.mGameState != GameState.GameState_BossIntro)
					{
						this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_FIGHT));
						Board.gNeedBossIntroSound = true;
						this.mGameState = GameState.GameState_BossIntro;
						this.mStatsState = 2;
						this.mStatsBubbleScale.SetConstant(this.mStatsBubbleScale);
						this.mBossIntroBGAlpha.SetCurve(ZumasRevenge.Common._MP("b+0,1,0.005,1,####    r####    Y~###S~###"));
						this.mBossSmScale.SetCurve(ZumasRevenge.Common._MP("b+1,7,0,1,####         ~~###"), this.mBossIntroBGAlpha);
						this.mBossSmPosPct.SetCurve(ZumasRevenge.Common._MP("b+0,1,0,1,#/05      }~###   $~###"), this.mBossIntroBGAlpha);
						this.mBossRedPct.SetCurve(ZumasRevenge.Common._MP("b+0,1,0,1,####         ~jWDM"), this.mBossIntroBGAlpha);
						this.RemoveWidget(this.mStatsContinueBtn);
						this.mApp.SafeDeleteWidget(this.mStatsContinueBtn);
						this.mStatsContinueBtn = null;
						return;
					}
				}
				else if (this.mLevelTransition.mState == 1)
				{
					this.mLevelTransition.Open();
					if (this.mStatsContinueBtn != null)
					{
						this.mStatsContinueBtn.SetVisible(false);
						return;
					}
				}
			}
			else
			{
				if (this.mGauntletRetryBtn != null && id == this.mGauntletRetryBtn.mId)
				{
					this.SetNextLevelMusic(false);
					this.PlayLevelMusic(0.008f);
					this.mStartingGauntletLevel = this.mApp.GetLevelMgr().GetStartingGauntletLevel(this.mLevel.mId);
					this.mScore = (this.mLevelBeginScore = 0);
					GameApp.gDDS.SetGauntletTime(0);
					GameApp.gDDS.SetGauntletPoints(0);
					this.mLevel.UpdateChallengeModeDifficulty();
					this.RestartLevel();
					this.mFruitMultiplier = 1;
					this.mScoreMultiplier = 1;
					this.RemoveWidget(this.mGauntletRetryBtn);
					this.RemoveWidget(this.mGauntletQuitBtn);
					this.mApp.SafeDeleteWidget(this.mGauntletQuitBtn);
					this.mApp.SafeDeleteWidget(this.mGauntletRetryBtn);
					this.mGauntletRetryBtn = (this.mGauntletQuitBtn = null);
					this.SetMenuBtnEnabled(true);
					this.mHasDoneIntroSounds = false;
					for (int j = 0; j < this.mLevel.mNumCurves; j++)
					{
						this.mLevel.mHoleMgr.GetHole(j).mDoDeathFade = false;
					}
					return;
				}
				if (this.mGauntletQuitBtn != null && id == this.mGauntletQuitBtn.mId)
				{
					this.mApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.EndChallengeModeGame);
					this.mApp.ToggleBambooTransition();
				}
			}
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x0004686B File Offset: 0x00044A6B
		public void ButtonDownTick(int theId)
		{
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0004686D File Offset: 0x00044A6D
		public void ButtonMouseEnter(int theId)
		{
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0004686F File Offset: 0x00044A6F
		public void ButtonMouseLeave(int theId)
		{
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00046871 File Offset: 0x00044A71
		public void ButtonMouseMove(int theId, int theX, int theY)
		{
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00046874 File Offset: 0x00044A74
		public void ProcessHardwareBackButton()
		{
			if (this.mShowMapScreen)
			{
				return;
			}
			if (this.mDoingFirstTimeIntro && !this.mShowMapScreen)
			{
				GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(this.ClearFirstIntroForBack);
				GameApp.gApp.ToggleBambooTransition();
				GameApp.gApp.mMusic.StopAll();
				GameApp.gApp.OnHardwareBackButtonPressProcessed();
				return;
			}
			if (this.mGauntletMode && this.mChallengeHelp != null)
			{
				this.ChallengeHelpClosed();
				GameApp.gApp.OnHardwareBackButtonPressProcessed();
				return;
			}
			if (this.mIsTryAndBuyDialogShowing)
			{
				Dialog dialog = GameApp.gApp.GetDialog(1);
				if (dialog != null)
				{
					dialog.ButtonDepress(1001);
					GameApp.gApp.OnHardwareBackButtonPressProcessed();
					return;
				}
			}
			Dialog dialog2 = GameApp.gApp.GetDialog(2);
			if (dialog2 != null)
			{
				(dialog2 as OptionsDialog).ProcessHardwareBackButton();
				return;
			}
			if (this.mBoardState == Board.BoardState.BoardState_BackToMainMenuPrompt)
			{
				this.mBoardState = Board.BoardState.BoardState_Game;
				this.Pause(false, true);
				GameApp.gApp.GetDialog(1).ButtonDepress(1001);
				GameApp.gApp.OnHardwareBackButtonPressProcessed();
				return;
			}
			if (this.mBoardState == Board.BoardState.BoardState_Game)
			{
				if (this.GauntletMode() && this.mChallengeHelp != null)
				{
					this.ChallengeHelpClosed();
					GameApp.gApp.OnHardwareBackButtonPressProcessed();
					return;
				}
				this.mApp.DoOptionsDialog(true);
				GameApp.gApp.OnHardwareBackButtonPressProcessed();
			}
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x000469BC File Offset: 0x00044BBC
		public void ProcessYesNo(int theId)
		{
			this.Pause(false, true);
			this.mBoardState = Board.BoardState.BoardState_Game;
			if (theId == 1000)
			{
				GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.DoDeferredEndGame);
				GameApp.gApp.ToggleBambooTransition();
				GameApp.gApp.mMusic.StopAll();
			}
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x00046A18 File Offset: 0x00044C18
		public void ProcessExitingEvent()
		{
			if (!this.mShowMapScreen && !this.mDoingFirstTimeIntro && !this.mDoingTransition)
			{
				if (this.mLevel.mBoss == null && this.isResultPageInAdvMode())
				{
					this.mForceToNextLevelInAdvMode = true;
					if (this.mTheNextLevel > this.mLevelNum)
					{
						this.mLevelNum++;
					}
				}
				else if (this.mGameState == GameState.GameState_BossIntro)
				{
					this.mForceToNextLevelInAdvMode = true;
					if (this.mTheNextLevel > this.mLevelNum)
					{
						this.mLevelNum++;
					}
				}
				this.DoShutdownSaveGame();
				this.mForceToNextLevelInAdvMode = false;
			}
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00046AB4 File Offset: 0x00044CB4
		public void ProcessOnDeactiveEvent()
		{
			if (!this.mShowMapScreen && !this.mDoingFirstTimeIntro && !this.mDoingTransition && this.mPauseCount <= 0)
			{
				this.ButtonDepress(this.mMenuButton.mId);
			}
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00046AE8 File Offset: 0x00044CE8
		public override void KeyChar(char c)
		{
			if ((!this.mLevel.CanUseKeyboard() || this.ShouldBlockInput() || this.mDoingFirstTimeIntro || this.mTransitionScreenImage != null) && c != '@')
			{
				return;
			}
			if (c != ' ' && this.IsPaused())
			{
				return;
			}
			if (c == ' ' && this.mDialogCount == 0)
			{
				this.mLastPauseTick = this.mUpdateCnt;
				this.Pause(this.mPauseCount == 0);
				return;
			}
			if (c == 't' || c == 'T')
			{
				this.mDisplayAceTime = !this.mDisplayAceTime;
				return;
			}
			if ((c == 'j' || c == 'J') && this.mLevel.mNumFrogPoints > 1 && !this.mFrog.IsHopping() && this.mZumaTips.size<ZumaTip>() == 0 && this.mLevel.CanRotateFrog() && !this.DoingIntros() && this.mLevelTransition == null && !this.ShouldBlockInput() && this.mGameState == GameState.GameState_Playing)
			{
				int num = 0;
				if (this.mLevel.mCurFrogPoint == 0)
				{
					num = 1;
				}
				this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_LILLYPAD_JUMP));
				this.mLevel.mCurFrogPoint = num;
				this.mFrog.SetDestPos(this.mLevel.mFrogX[num], this.mLevel.mFrogY[num], this.mLevel.mMoveSpeed, true);
				this.mLevel.ChangedPad(num);
			}
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00046C55 File Offset: 0x00044E55
		public new void KeyDown(KeyCode k)
		{
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00046C57 File Offset: 0x00044E57
		public override void AddedToManager(WidgetManager theWidgetManager)
		{
			base.AddedToManager(theWidgetManager);
			if (this.mLevel != null && this.mGameState != GameState.GameState_Boss6DarkFrog)
			{
				if (this.mGameState != GameState.GameState_Losing)
				{
					this.UpdateGunPos(true);
				}
				this.mLevel.AfterBoardAdded();
			}
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00046C8D File Offset: 0x00044E8D
		public override void RemovedFromManager(WidgetManager theWidgetManager)
		{
			base.RemovedFromManager(theWidgetManager);
			if (this.mSwapBallButton != null)
			{
				this.mWidgetManager.RemoveWidget(this.mSwapBallButton);
				this.mSwapBallButton.Dispose();
				this.mSwapBallButton = null;
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00046CC1 File Offset: 0x00044EC1
		public override void GotFocus()
		{
			base.GotFocus();
			this.mWidgetManager.SetGamepadSelection(this, WidgetLinkDir.LINK_DIR_NONE);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00046CD8 File Offset: 0x00044ED8
		public bool DoingLilyPadTutorial(int x, int y)
		{
			if (this.mZumaTips.size<ZumaTip>() == 0)
			{
				return false;
			}
			ZumaTip zumaTip = Enumerable.First<ZumaTip>(this.mZumaTips);
			return zumaTip.mId == ZumaProfile.LILLY_PAD_HINT && !zumaTip.CutoutContainsPoint(x, y);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00046D1A File Offset: 0x00044F1A
		public void KillActiveTutorial(int inTipId)
		{
			if (this.mZumaTips.Count == 0 || Enumerable.First<ZumaTip>(this.mZumaTips).mId != inTipId)
			{
				return;
			}
			this.mZumaTips.RemoveAt(0);
			this.mApp.mUserProfile.MarkHintAsSeen(inTipId);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00046D5C File Offset: 0x00044F5C
		public bool BlockInputForTutorial(int x, int y, out bool outAllowBallFire)
		{
			outAllowBallFire = false;
			if (this.mZumaTips.size<ZumaTip>() == 0 || !Enumerable.First<ZumaTip>(this.mZumaTips).mClickDismiss)
			{
				return false;
			}
			ZumaTip zumaTip = Enumerable.First<ZumaTip>(this.mZumaTips);
			if (zumaTip.mUpdateCount < 50)
			{
				return true;
			}
			bool result = true;
			SoundAttribs soundAttribs = new SoundAttribs();
			soundAttribs.fadeout = 0.008f;
			this.DisableHaloSwap(true);
			if (zumaTip.mId == ZumaProfile.FRUIT_HINT)
			{
				if (this.mFrog.GetAngle() < 4.161289f || (double)this.mFrog.GetAngle() > 4.317596)
				{
					return true;
				}
				outAllowBallFire = true;
				result = false;
				this.mHasDoneIntroSounds = true;
				this.mApp.mSoundPlayer.Loop((this.mLevel.mZone == 5) ? Res.GetSoundByID(ResID.SOUND_UNDERWATER_ROLLOUT) : Res.GetSoundByID(ResID.SOUND_ROLLING), soundAttribs);
			}
			else if (zumaTip.mId == ZumaProfile.LILLY_PAD_HINT)
			{
				if (!zumaTip.CutoutContainsPoint(x, y))
				{
					return true;
				}
				result = false;
			}
			else if (zumaTip.mId == ZumaProfile.SWAP_BALL_HINT)
			{
				if (!this.IsTouchOnFrogGun(x, y))
				{
					return true;
				}
				this.mAllowBulletDetection = false;
				this.SwapFrogBalls();
			}
			this.mApp.mUserProfile.MarkHintAsSeen(zumaTip.mId);
			this.mZumaTips.RemoveAt(0);
			this.MarkDirty();
			if (this.mZumaTips.size<ZumaTip>() == 0)
			{
				this.mPreventBallAdvancement = false;
			}
			return result;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00046EBA File Offset: 0x000450BA
		public new void GamepadButtonDown(GamepadButton theButton, int thePlayer, uint theFlags)
		{
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00046EBC File Offset: 0x000450BC
		public new void GamepadAxisMove(GamepadAxis theAxis, int thePlayer, float theAxisValue)
		{
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00046EC0 File Offset: 0x000450C0
		public BetaStats GetBetaStats()
		{
			if (this.GauntletMode())
			{
				return this.mApp.mUserProfile.mChallengeBetaStats;
			}
			if (this.IronFrogMode())
			{
				return this.mApp.mUserProfile.mIronFrogBetaStats;
			}
			if (this.IsHardAdventureMode())
			{
				return this.mApp.mUserProfile.mHardAdvBetaStats;
			}
			return this.mApp.mUserProfile.mAdvBetaStats;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00046F28 File Offset: 0x00045128
		public void MultiplierBallAdded(Ball b)
		{
			MultiplierBallEffect multiplierBallEffect = new MultiplierBallEffect(b, true);
			this.mMultiplierBallEffects.Add(multiplierBallEffect);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00046F4C File Offset: 0x0004514C
		public void DrawTunnels(Graphics g, int priority, bool below_shadow)
		{
			for (int i = 0; i < this.mTunnels[priority].size<Tunnel>(); i++)
			{
				Tunnel tunnel = this.mTunnels[priority][i];
				if (tunnel.mAboveShadows != below_shadow)
				{
					float num = (float)GameApp.mGameRes / 640f;
					if (tunnel.mImage != null)
					{
						int w = (int)(num * (float)tunnel.mImage.mWidth);
						int h = (int)(num * (float)tunnel.mImage.mHeight);
						if (tunnel.mLayerId.Length == 0)
						{
							this.mLevel.DrawTunnel(g, tunnel.mImage, ZumasRevenge.Common._S(tunnel.mX + GameApp.gScreenShakeX - 160), ZumasRevenge.Common._S(tunnel.mY + GameApp.gScreenShakeY), w, h);
						}
						else
						{
							this.mLevel.DrawTunnel(g, tunnel.mImage, ZumasRevenge.Common._DS(tunnel.mX + GameApp.gScreenShakeX - 160), ZumasRevenge.Common._DS(tunnel.mY + GameApp.gScreenShakeY), w, h);
						}
					}
				}
			}
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00047054 File Offset: 0x00045254
		public bool NeedSaveGame()
		{
			return !this.mGauntletMode && !this.IronFrogMode() && (this.mApp.mUserProfile.HasSeenHint(ZumaProfile.FIRST_SHOT_HINT) || this.mLevelNum != 1) && this.mGameState != GameState.GameState_None;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00047094 File Offset: 0x00045294
		public void ResetInARowBonus()
		{
			if (this.mNumClearsInARow > this.mLevelStats.mMaxInARow)
			{
				this.mLevelStats.mMaxInARow = this.mNumClearsInARow;
				this.mLevelStats.mMaxInARowScore = this.mCurInARowBonus;
			}
			this.mNumClearsInARow = 0;
			this.mCurInARowBonus = 0;
			this.mLevel.ClearedInARowBonus();
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x000470F0 File Offset: 0x000452F0
		public void ResetBallColorMap()
		{
			for (int i = 0; i < 6; i++)
			{
				this.mBallColorMap[i] = 0;
			}
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00047112 File Offset: 0x00045312
		public void ShakeScreen(int t, int xmax, int ymax)
		{
			this.mScreenShakeTime = t;
			this.mScreenShakeXMax = xmax;
			this.mScreenShakeYMax = ymax;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0004712C File Offset: 0x0004532C
		public void SyncState(DataSync theSync, bool onlyLife)
		{
			if (this.mGauntletMode)
			{
				return;
			}
			if (!theSync.isRead() && this.ShouldBypassFinalSequenceOnLoad())
			{
				this.mForceRestartInAdvMode = true;
				this.mLevelNum = 0;
			}
			SexyBuffer buffer = theSync.GetBuffer();
			theSync.SyncLong(ref this.mLives);
			theSync.SyncLong(ref this.mScore);
			theSync.SyncLong(ref this.mScoreTarget);
			theSync.SyncLong(ref this.mLevelBeginScore);
			this.mRollerScore.SyncState(theSync);
			if (onlyLife)
			{
				return;
			}
			this.mLevel.SyncState(theSync);
			if (theSync.isRead() && this.mLevel.mTorchStageState >= 8 && this.mLevel.mTorchStageState <= 9)
			{
				this.mFullScreenAlphaRate = 2;
			}
			this.mQRand.SyncState(theSync);
			this.mFrog.SyncState(theSync);
			if (theSync.isWrite() && this.mPreventBallAdvancement)
			{
				if (this.mZumaTips.Count > 0 && this.mZumaTips[0].mId == ZumaProfile.ZUMA_BAR_HINT)
				{
					buffer.WriteBoolean(false);
				}
				else
				{
					buffer.WriteBoolean(this.mPreventBallAdvancement);
				}
			}
			else
			{
				theSync.SyncBoolean(ref this.mPreventBallAdvancement);
			}
			theSync.SyncBoolean(ref this.mDoingEndBossFrogEffect);
			theSync.SyncLong(ref this.mEndBossFrogTimer);
			theSync.SyncFloat(ref this.mEndBossFadeAmt);
			if (theSync.isWrite())
			{
				ZumasRevenge.Common.SerializePIEffect(this.mBossSmokePoof, theSync);
			}
			else
			{
				ZumasRevenge.Common.DeserializePIEffect(this.mBossSmokePoof, theSync);
			}
			theSync.SyncLong(ref this.mAccuracyBackupCount);
			theSync.SyncFloat(ref this.mAdventureWinAlpha);
			theSync.SyncFloat(ref this.mAdventureWinDoorYOff);
			theSync.SyncBoolean(ref this.mHasSeenCheckpointIntro);
			theSync.SyncBoolean(ref this.mNeedsCheckpointIntro);
			theSync.SyncLong(ref this.mHallucinateTimer);
			theSync.SyncBoolean(ref this.mHasDoneIntroSounds);
			theSync.SyncLong(ref this.mScreenShakeTime);
			theSync.SyncLong(ref this.mScreenShakeXMax);
			theSync.SyncLong(ref this.mScreenShakeYMax);
			theSync.SyncLong(ref GameApp.gScreenShakeX);
			theSync.SyncLong(ref GameApp.gScreenShakeY);
			theSync.SyncLong(ref this.mIgnoreCount);
			if (theSync.isRead())
			{
				this.ClearPIEffects();
				int num = (int)buffer.ReadLong();
				for (int i = 0; i < num; i++)
				{
					EndLevelExplosion endLevelExplosion = this.mEndLevelExplosionPool.Alloc();
					endLevelExplosion.mDelay = (int)buffer.ReadLong();
					endLevelExplosion.mX = (int)buffer.ReadLong();
					endLevelExplosion.mY = (int)buffer.ReadLong();
					this.mEndLevelExplosions.Add(endLevelExplosion);
					ZumasRevenge.Common.DeserializePIEffect(endLevelExplosion.mPIEffect, theSync);
				}
				num = (int)buffer.ReadLong();
				for (int j = 0; j < num; j++)
				{
					BallExplosion ballExplosion = this.mBallExplosionsPool.Alloc();
					ballExplosion.mPIEffect.ResetAnim();
					this.mBallExplosions.Add(ballExplosion);
					ZumasRevenge.Common.DeserializePIEffect(ballExplosion.mPIEffect, theSync);
				}
				num = (int)buffer.ReadLong();
				for (int k = 0; k < num; k++)
				{
					PIEffect pieffect = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_LAZER_BLAST).Duplicate();
					this.mLazerBlasts.Add(pieffect);
					ZumasRevenge.Common.DeserializePIEffect(pieffect, theSync);
					ZumasRevenge.Common.SetFXNumScale(pieffect, 1f);
				}
				this.mPowerEffects.Clear();
				short num2 = buffer.ReadShort();
				for (int l = 0; l < (int)num2; l++)
				{
					int num3 = (int)buffer.ReadLong();
					if (num3 == 2)
					{
						this.mPowerEffects.Add(new ReversePowerEffect());
					}
					else if (num3 == 4)
					{
						this.mPowerEffects.Add(new CannonPowerEffect());
					}
					else
					{
						this.mPowerEffects.Add(new PowerEffect());
					}
					this.mPowerEffects.back<PowerEffect>().SyncState(theSync);
				}
				this.mContinueNextLevelOnLoadProfile = buffer.ReadBoolean();
				bool flag = buffer.ReadBoolean();
				if (flag)
				{
					this.mNextLevelOverrideOnLoadProfile = (int)buffer.ReadLong();
				}
			}
			else
			{
				buffer.WriteLong((long)this.mEndLevelExplosions.Count);
				for (int m = 0; m < this.mEndLevelExplosions.Count; m++)
				{
					EndLevelExplosion endLevelExplosion2 = this.mEndLevelExplosions[m];
					buffer.WriteLong((long)endLevelExplosion2.mDelay);
					buffer.WriteLong((long)endLevelExplosion2.mX);
					buffer.WriteLong((long)endLevelExplosion2.mY);
					ZumasRevenge.Common.SerializePIEffect(endLevelExplosion2.mPIEffect, theSync);
				}
				buffer.WriteLong((long)this.mBallExplosions.Count);
				for (int n = 0; n < this.mBallExplosions.Count; n++)
				{
					ZumasRevenge.Common.SerializePIEffect(this.mBallExplosions[n].mPIEffect, theSync);
				}
				buffer.WriteLong((long)this.mLazerBlasts.Count);
				for (int num4 = 0; num4 < this.mLazerBlasts.Count; num4++)
				{
					ZumasRevenge.Common.SerializePIEffect(this.mLazerBlasts[num4], theSync);
				}
				buffer.WriteShort((short)this.mPowerEffects.Count);
				for (int num5 = 0; num5 < this.mPowerEffects.Count; num5++)
				{
					buffer.WriteLong((long)this.mPowerEffects[num5].GetType());
					this.mPowerEffects[num5].SyncState(theSync);
				}
				bool flag2 = this.isResultPageInAdvMode();
				buffer.WriteBoolean(flag2);
				buffer.WriteBoolean(this.mLevelTransition != null);
				if (this.mLevelTransition != null)
				{
					buffer.WriteLong((long)this.mLevelTransition.mNextLevelOverride);
					if (flag2 && this.mLevel.mNum != 2147483647)
					{
						int num6 = this.mLevel.mNum + 1;
						int num7 = this.mLevel.mZone;
						if (num6 == 11)
						{
							num6 = int.MaxValue;
						}
						else if (num6 > 11 || this.mLevel.mNum == 2147483647)
						{
							num6 = 1;
							num7++;
							if (num7 > 6)
							{
								num7 = 6;
								num6 = int.MaxValue;
							}
						}
						this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvLevel = num6;
						this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvZone = num7;
					}
				}
			}
			if (theSync.isRead())
			{
				int mPointsForLife = this.mApp.GetLevelMgr().mPointsForLife;
				int num8 = this.mScore / mPointsForLife;
				this.mPointsLeftForExtraLife = (num8 + 1) * mPointsForLife - this.mScore;
			}
			theSync.SyncLong(ref this.mFlashAlpha);
			theSync.SyncBoolean(ref this.mIsWinning);
			this.mLevelStats.SyncState(theSync);
			this.mGameStats.SyncState(theSync);
			theSync.SyncLong(ref this.mFruitMultiplier);
			theSync.SyncLong(ref this.mTreasureEndFrame);
			theSync.SyncLong(ref this.mStateCount);
			theSync.SyncLong(ref this.mAccuracyCount);
			theSync.SyncLong(ref this.mDestroyCount);
			theSync.SyncLong(ref this.mPauseFade);
			theSync.SyncLong(ref this.mMouseOverGunPos);
			theSync.SyncLong(ref this.mLevelEndFrame);
			theSync.SyncBoolean(ref this.mWasPerfectLevel);
			theSync.SyncLong(ref this.mNumDeaths);
			theSync.SyncLong(ref this.mLastIntroPad);
			theSync.SyncLong(ref this.mLastIntroPadDelay);
			theSync.SyncLong(ref this.mIntroPadHopCount);
			theSync.SyncLong(ref this.mNumClearsInARow);
			theSync.SyncLong(ref this.mCurInARowBonus);
			theSync.SyncLong(ref this.mCurComboScore);
			theSync.SyncLong(ref this.mCurComboCount);
			theSync.SyncLong(ref this.mNumCleared);
			theSync.SyncLong(ref this.mScoreMultiplier);
			theSync.SyncBoolean(ref this.mIsEndless);
			theSync.SyncBoolean(ref this.mDoGuide);
			theSync.SyncBoolean(ref this.mRecalcGuide);
			theSync.SyncBoolean(ref this.mRecalcLazerGuide);
			theSync.SyncBoolean(ref this.mDestroyAll);
			theSync.SyncBoolean(ref this.mLevelBeginning);
			theSync.SyncBoolean(ref this.mForceTreasure);
			theSync.SyncBoolean(ref this.mLazerHitTreasure);
			theSync.SyncLong(ref this.mNumZumaBalls);
			int num9 = (int)this.mGameState;
			theSync.SyncLong(ref num9);
			this.mGameState = (GameState)num9;
			if (this.mGameState == GameState.GameState_BeatLevelBonus)
			{
				this.mGameState = GameState.GameState_Playing;
			}
			if (this.mGameState == GameState.GameState_Playing && theSync.isRead())
			{
				this.mEndBossFadeAmt = 0f;
			}
			theSync.SyncLong(ref this.mLastBallClickTick);
			theSync.SyncLong(ref this.mLastExplosionTick);
			theSync.SyncLong(ref this.mLastSmallExplosionTick);
			if (theSync.isRead())
			{
				if (this.mGameState == GameState.GameState_BossIntro)
				{
					this.mGameState = GameState.GameState_Playing;
				}
				this.mGuideBall = null;
				this.mShowGuide = false;
				this.mRecalcLazerGuide = (this.mRecalcGuide = true);
				this.DeleteBullets();
				short num10 = buffer.ReadShort();
				for (int num11 = 0; num11 < (int)num10; num11++)
				{
					Bullet bullet = new Bullet();
					bullet.SyncState(theSync);
					bullet.mFrog = this.mFrog;
					this.mBulletList.Add(bullet);
				}
				this.mCurTreasureNum = (int)buffer.ReadShort();
				if (this.mCurTreasureNum > 0)
				{
					this.mMinTreasureY = (this.mMaxTreasureY = float.MaxValue);
					this.mCurTreasure = this.mLevel.mTreasurePoints[this.mCurTreasureNum - 1];
				}
				else
				{
					this.mCurTreasure = null;
				}
			}
			else
			{
				buffer.WriteShort((short)this.mBulletList.Count);
				for (int num12 = 0; num12 < this.mBulletList.Count; num12++)
				{
					this.mBulletList[num12].SyncState(theSync);
				}
				buffer.WriteShort((short)((this.mCurTreasure != null) ? this.mCurTreasureNum : (-1)));
			}
			theSync.SyncLong(ref this.mTreasureGlowAlpha);
			theSync.SyncLong(ref this.mTreasureGlowAlphaRate);
			theSync.SyncLong(ref this.mTreasureStarAlpha);
			theSync.SyncLong(ref this.mTreasureCel);
			theSync.SyncFloat(ref this.mTreasureStarAngle);
			theSync.SyncBoolean(ref this.mTreasureWasHit);
			if (theSync.isRead())
			{
				if (buffer.ReadBoolean())
				{
					this.mDeathSkull = new DeathSkull();
					this.mDeathSkull.SyncState(theSync);
				}
				if (this.mGameState == GameState.GameState_Boss6FakeCredits)
				{
					this.mFakeCredits = null;
					if (this.mLevel.mEndSequence == 3)
					{
						this.mGameState = GameState.GameState_Playing;
					}
					else
					{
						this.mFakeCredits = new FakeCredits();
						this.mFakeCredits.Init(this.mFrog);
						this.SetMenuBtnEnabled(false);
					}
				}
				else if (this.mGameState == GameState.GameState_Boss6StoneHeadBurst)
				{
					this.mBoss6StoneBurst = null;
					if (this.mLevel.mEndSequence == 4)
					{
						this.mGameState = GameState.GameState_Playing;
					}
					else
					{
						this.MakeBoss6StoneBurstComp();
					}
				}
				else if (this.mGameState == GameState.GameState_Boss6DarkFrog)
				{
					this.mBoss6VolcanoMelt = null;
					this.mDarkFrogSequence = null;
					if (this.mLevel.mEndSequence == 5)
					{
						this.mGameState = GameState.GameState_Playing;
					}
					else
					{
						this.mDarkFrogSequence = new DarkFrogSequence();
						this.mDarkFrogSequence.Init();
						this.SetMenuBtnEnabled(false);
					}
				}
				else if (this.mGameState == GameState.GameState_FinalBossPart1Finished)
				{
					if (this.mLevel.mEndSequence == 2)
					{
						this.mGameState = GameState.GameState_Playing;
					}
					else
					{
						this.mFrog.SetSlowTimer(ZumasRevenge.Common._M(300));
						this.mVortexAppear = true;
						this.mVortexBGAlpha = 0f;
						this.mVortexFrogRadius = 0f;
						this.mVortexFrogAngle = this.mFrog.GetAngle();
						this.mVortexFrogScale = 1f;
						this.mVortexFrogRadiusExpand = true;
					}
				}
				this.mWasShowingCheckpoint = buffer.ReadBoolean();
				if (buffer.ReadBoolean())
				{
					this.mPreventBallAdvancement = false;
				}
				if (this.mGameState == GameState.GameState_BossIntro)
				{
					this.InitBossIntroState();
				}
				if (this.mWasShowingCheckpoint && this.mLives == 0)
				{
					this.DoCheckpointEffect(true);
				}
				if (this.mFruitExplodeEffect != null)
				{
					this.mFruitExplodeEffect.Reset();
				}
			}
			else
			{
				buffer.WriteBoolean(this.mDeathSkull != null);
				if (this.mDeathSkull != null)
				{
					this.mDeathSkull.SyncState(theSync);
				}
				buffer.WriteBoolean(this.mCheckpointEffect != null && this.mCheckpointEffect.mFromGameOver);
				if (this.mPreventBallAdvancement && this.mCheckpointEffect != null && !this.mCheckpointEffect.mFromGameOver && (this.mLevel.mBoss == null || this.mApp.IsHardMode() || this.mLevel.mZone > 1))
				{
					buffer.WriteBoolean(true);
				}
				else
				{
					buffer.WriteBoolean(false);
				}
			}
			theSync.SyncBoolean(ref this.mNeedsBossExtraLife);
			theSync.SyncLong(ref this.mEndLevelAceTimeBonus);
			theSync.SyncLong(ref this.mEndLevelNum);
			theSync.SyncLong(ref this.mEndLevelParTime);
			this.mEndLevelStats.SyncState(theSync);
			if (theSync.isRead())
			{
				if (this.mLevel != null && this.mLevel.mNum != 2147483647)
				{
					this.mEndLevelDisplayName = this.mLevel.mDisplayName;
					return;
				}
				this.mEndLevelDisplayName = "";
			}
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00047D90 File Offset: 0x00045F90
		public void SaveGame(string fname, SexyBuffer w)
		{
			if (this.mGauntletMode || this.IronFrogMode())
			{
				return;
			}
			SexyBuffer buffer = new SexyBuffer();
			bool flag = true;
			if (w == null)
			{
				w = buffer;
			}
			else
			{
				flag = false;
			}
			w.WriteLong((long)GameApp.gSaveGameVersion);
			w.WriteLong((long)this.mLevelNum);
			w.WriteString(this.mNextLevelIdOverride);
			if (this.ShouldBypassFinalSequenceOnLoad())
			{
				this.mForceRestartInAdvMode = true;
			}
			w.WriteBoolean(this.mForceRestartInAdvMode);
			w.WriteBoolean(this.mForceToNextLevelInAdvMode);
			if (this.mCheckpointEffect != null && this.mCheckpointEffect.mFromGameOver)
			{
				w.WriteBoolean(true);
				this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvScore = this.GetCheckpointScore();
			}
			else
			{
				w.WriteBoolean(false);
			}
			if (this.ShouldBypassFinalSequenceOnLoad())
			{
				w.WriteString("jungle1");
			}
			else
			{
				w.WriteString(this.mLevel.mId);
			}
			w.WriteBoolean(this.mApp.GetDialog(2) != null);
			DataSync dataSync = new DataSync(w, false);
			this.SyncState(dataSync, false);
			dataSync.SyncPointers();
			if (flag)
			{
				StorageFile.MakeDir(this.mApp.mUserProfile.GetSaveGameNameFolder());
				StorageFile.WriteBufferToFile(fname, buffer);
				this.mApp.SaveProfile();
				this.mApp.ClearUpdateBacklog(false);
			}
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00047ED8 File Offset: 0x000460D8
		public bool LoadGame(string fname)
		{
			if (this.mGauntletMode || (this.mLevel != null && this.IronFrogMode()))
			{
				return false;
			}
			SexyBuffer buffer = new SexyBuffer();
			StorageFile.MakeDir(this.mApp.mUserProfile.GetSaveGameNameFolder());
			return StorageFile.ReadBufferFromFile(fname, buffer) && this.LoadGame(buffer);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00047F2C File Offset: 0x0004612C
		public bool LoadGame(SexyBuffer b)
		{
			if (this.mGauntletMode)
			{
				return false;
			}
			this.mIsLoading = true;
			DataSync dataSync = new DataSync(b, true);
			int num = (int)b.ReadLong();
			if (num != GameApp.gSaveGameVersion)
			{
				this.mLevelNum = 1;
				this.Reset(true, false);
				this.StartLevel(this.mLevelNum);
				this.UpdateGunPos(true);
				this.mIsLoading = false;
				return false;
			}
			this.mLevelNum = (int)b.ReadLong();
			this.mNextLevelIdOverride = b.ReadString();
			bool flag = b.ReadBoolean();
			bool flag2 = b.ReadBoolean();
			b.ReadBoolean();
			string level_id = b.ReadString();
			if (flag)
			{
				this.mLevelNum = 1;
				this.StartLevel(this.mLevelNum);
				this.UpdateGunPos(true);
				this.mIsLoading = false;
				return true;
			}
			if (flag2)
			{
				b.ReadBoolean();
				this.SyncState(dataSync, true);
				this.mLevelBeginScore = this.mScore;
				this.StartLevel(this.mLevelNum);
				this.UpdateGunPos(true);
				this.mIsLoading = false;
				return true;
			}
			this.StartLevel(level_id, true, false, false, null);
			b.ReadBoolean();
			if (this.mLevelNameText[0] != null)
			{
				this.mLevelNameText[0].mDelay = 0;
			}
			if (this.mLevelNameText[1] != null)
			{
				this.mLevelNameText[1].mDelay = 0;
			}
			this.SyncState(dataSync, false);
			dataSync.SyncPointers();
			if (this.mHallucinateTimer > 0)
			{
				Font fontByID = Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_STROKE);
				this.mLevel.mFrog.mTempText = new BonusText(TextManager.getInstance().getString(691), fontByID, (float)ZumasRevenge.Common._S(this.mLevel.mFrog.GetCenterX() - ZumasRevenge.Common._M(30)), (float)ZumasRevenge.Common._S(this.mLevel.mFrog.GetCenterY() - ZumasRevenge.Common._M1(70)), (float)ZumasRevenge.Common._M2(0), ZumasRevenge.Common._M3(0));
				this.mLevel.mFrog.mTempText.SetAlphaDecRate(0f);
			}
			if (!this.mLevel.mCanDrawBoss && this.mGameState == GameState.GameState_Playing)
			{
				this.mLevel.mCanDrawBoss = true;
			}
			this.mIsLoading = false;
			if (this.mGameState == GameState.GameState_BossDead)
			{
				this.SetMenuBtnEnabled(false);
			}
			this.mTheNextLevel = this.mLevelNum + 1;
			return true;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00048164 File Offset: 0x00046364
		public void PlayBallClick(int theSound)
		{
			ulong num = (ulong)SexyFramework.Common.SexyTime();
			if (num - (ulong)this.mLastBallClickTick < 250UL)
			{
				return;
			}
			this.mApp.PlaySample(theSound);
			this.mLastBallClickTick = (uint)num;
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x000481A0 File Offset: 0x000463A0
		public void PlaySmallExplosionSound()
		{
			ulong num = (ulong)SexyFramework.Common.SexyTime();
			if (num - (ulong)this.mLastSmallExplosionTick > 100UL)
			{
				this.mLastSmallExplosionTick = (uint)num;
				this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BALLDESTROYED3));
			}
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x000481E0 File Offset: 0x000463E0
		public void PlayExplosionSound()
		{
			ulong num = (ulong)SexyFramework.Common.SexyTime();
			if (num - (ulong)this.mLastExplosionTick > 250UL)
			{
				this.mLastExplosionTick = (uint)num;
				this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_EXPLODE));
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00048224 File Offset: 0x00046424
		public int GetGoodBallColor()
		{
			int[] array = new int[6];
			int num = 0;
			bool flag = true;
			bool flag2 = false;
			for (int i = 0; i < 6; i++)
			{
				if (this.mBallColorMap[i] > 0)
				{
					if (this.GauntletMode() && i == 4 && this.mNewBallDelay[0] == -1)
					{
						this.mNewBallDelay[0] = ZumasRevenge.Common._M(1000);
					}
					else if (this.GauntletMode() && i == 5 && this.mNewBallDelay[1] == -1)
					{
						this.mNewBallDelay[1] = ZumasRevenge.Common._M(1000);
					}
					else if (!this.GauntletMode() || (i != 5 && i != 4) || this.mNewBallDelay[i - 4] <= 0)
					{
						array[num++] = i;
						if (flag2 && this.mBallColorMap[i] >= 1)
						{
							flag = false;
						}
						if (this.mBallColorMap[i] >= 1)
						{
							flag2 = true;
						}
					}
				}
			}
			if (num > 0)
			{
				bool flag3 = false;
				List<float> list = new List<float>();
				for (int j = 0; j < 6; j++)
				{
					if (this.mBallColorMap[j] > 0)
					{
						if (this.GauntletMode() && (j == 5 || j == 4) && this.mNewBallDelay[j - 4] > 0)
						{
							list.Add(0f);
						}
						else
						{
							list.Add(this.mLevel.GetRandomFrogBulletColor(num, j));
						}
					}
					else
					{
						list.Add(0f);
					}
					if ((this.mBallColorMap[j] > 0 && !this.mQRand.HasWeight(j)) || (this.mBallColorMap[j] == 0 && this.mQRand.HasWeight(j)))
					{
						flag3 = true;
					}
				}
				if (flag3)
				{
					this.mQRand.Clear();
					this.mQRand.SetWeights(list);
				}
			}
			if (this.mLevel.mNumCurves == 0 || (flag && !this.mLevel.DoingInitialPathHilite() && this.mScore == this.mLevelBeginScore))
			{
				Bullet bullet = this.mFrog.GetBullet();
				if (bullet == null)
				{
					bullet = this.mFrog.GetNextBullet();
				}
				if (bullet != null)
				{
					return bullet.GetColorType();
				}
				return MathUtils.SafeRand() % 4;
			}
			else
			{
				if (num <= 0)
				{
					return -1;
				}
				if (Board.gNewStyleBallChooser)
				{
					return this.mQRand.Next();
				}
				return array[MathUtils.SafeRand() % num];
			}
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0004845C File Offset: 0x0004665C
		public int GetNumBallColors()
		{
			int num = 0;
			for (int i = 0; i < 6; i++)
			{
				if (this.mBallColorMap[i] > 0)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00048487 File Offset: 0x00046687
		public void CheckReload()
		{
			if (this.mFrog.GetBullet() != null && this.mFrog.GetNextBullet() != null)
			{
				if (this.AddBulletColorsToBoard())
				{
					return;
				}
				if (this.PrepGunForBallSwapTutorial())
				{
					return;
				}
				this.EnsureBulletsAreUseful();
			}
			this.LoadEmptyGun();
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x000484C4 File Offset: 0x000466C4
		public bool AddBulletColorsToBoard()
		{
			if (!ZumasRevenge.Common.gSuckMode)
			{
				return false;
			}
			this.mBallColorMap[this.mFrog.GetBullet().GetColorType()]++;
			this.mBallColorMap[this.mFrog.GetNextBullet().GetColorType()]++;
			return true;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0004852C File Offset: 0x0004672C
		public bool PrepGunForBallSwapTutorial()
		{
			if (this.GauntletMode() || this.mLevelNum != 3 || this.mLevel.mZone != 1 || this.mApp.mUserProfile.HasSeenHint(ZumaProfile.SWAP_BALL_HINT))
			{
				return false;
			}
			while (this.mFrog.GetBullet().GetColorType() == this.mFrog.GetNextBullet().GetColorType())
			{
				this.mFrog.SetNextBulletType(SexyFramework.Common.Rand() % 4);
			}
			return true;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x000485A8 File Offset: 0x000467A8
		public void EnsureBulletsAreUseful()
		{
			if (!this.mAllowBulletDetection || this.mLevel.mNumCurves <= 0)
			{
				return;
			}
			if (this.mLevel.mNum == 1 && this.mLevel.mZone == 1 && !this.mApp.mUserProfile.HasSeenHint(ZumaProfile.FIRST_SHOT_HINT))
			{
				return;
			}
			if (this.mBallColorMap[this.mFrog.GetBullet().GetColorType()] <= 0)
			{
				int goodBallColor = this.GetGoodBallColor();
				if (goodBallColor == -1)
				{
					return;
				}
				this.mFrog.SetBulletType(goodBallColor);
			}
			if (this.mBallColorMap[this.mFrog.GetNextBullet().GetColorType()] <= 0)
			{
				int goodBallColor2 = this.GetGoodBallColor();
				if (goodBallColor2 == -1)
				{
					return;
				}
				this.mFrog.SetNextBulletType(goodBallColor2);
			}
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00048664 File Offset: 0x00046864
		public void LoadEmptyGun()
		{
			while (this.mFrog.NeedsReload())
			{
				int num = this.mLevel.GetFrogReloadType();
				if (num == -1)
				{
					num = this.GetGoodBallColor();
				}
				if (num == -1)
				{
					return;
				}
				PowerType thePower = PowerType.PowerType_Max;
				this.mFrog.Reload(num, true, thePower);
			}
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x000486B0 File Offset: 0x000468B0
		public void ActivatePower(Ball theBall)
		{
			PowerType powerOrDestType = theBall.GetPowerOrDestType();
			this.ActivatePower(powerOrDestType, theBall.GetColorType(), (int)theBall.GetX(), (int)theBall.GetY());
			if ((powerOrDestType != PowerType.PowerType_ProximityBomb || this.mLevel.mBoss == null) && powerOrDestType != PowerType.PowerType_GauntletMultBall)
			{
				this.GetBetaStats().ActivatedPowerup((int)powerOrDestType);
			}
			for (int i = 0; i < this.mLevel.mNumCurves; i++)
			{
				this.mLevel.mCurveMgr[i].ActivatePower(theBall);
			}
			if (powerOrDestType == PowerType.PowerType_GauntletMultBall)
			{
				this.mGauntletMultBarAlpha = 255f;
				this.mGauntletMultTextFlashOn = true;
				this.mGauntletMultTextFlashTimer = 0;
				this.mGauntletMultTextVX = (this.mGauntletMultTextVY = 0f);
				this.mGauntletMultTextMoveLastFrame = 0;
				if (this.mApp.GetLevelMgr().mMultBallPoints > 0)
				{
					this.IncScore(this.mApp.GetLevelMgr().mMultBallPoints, false);
				}
				MultiplierBallEffect multiplierBallEffect = null;
				for (int j = 0; j < this.mMultiplierBallEffects.size<MultiplierBallEffect>(); j++)
				{
					MultiplierBallEffect multiplierBallEffect2 = this.mMultiplierBallEffects[j];
					if (multiplierBallEffect2.GetBall() == theBall)
					{
						multiplierBallEffect = multiplierBallEffect2;
						break;
					}
				}
				if (multiplierBallEffect == null)
				{
					multiplierBallEffect = new MultiplierBallEffect(theBall, false);
					this.mMultiplierBallEffects.Add(multiplierBallEffect);
				}
				this.mLevel.MultiplierActivated();
				this.mScoreMultiplier++;
				if (this.mScoreMultiplier == 11)
				{
					this.mApp.SetAchievement("score_mult_11x");
				}
				if (this.mScoreMultiplier > this.mApp.mUserProfile.mChallengeStats.mHighestMult)
				{
					this.mApp.mUserProfile.mChallengeStats.mHighestMult = this.mScoreMultiplier;
				}
				multiplierBallEffect.BallDestroyed(theBall);
				this.AddText(this.mScoreMultiplier + TextManager.getInstance().getString(97), this.mFrog.GetCenterX(), this.mFrog.GetCenterY(), ZumasRevenge.Common._M(2f), -1, null);
			}
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00048898 File Offset: 0x00046A98
		public void ActivatePower(PowerType p, int color_type, int x, int y)
		{
			if (x == -1)
			{
				x = this.mWidth / 2;
			}
			if (y == -1)
			{
				y = this.mHeight / 2;
			}
			if ((p == PowerType.PowerType_Laser || p == PowerType.PowerType_Cannon || p == PowerType.PowerType_ColorNuke) && this.mAccuracyCount > 300)
			{
				this.mAccuracyBackupCount = this.mAccuracyCount;
				this.DoAccuracy(false);
				this.mAccuracyCount = 300;
			}
			string t = ZumasRevenge.Common.PowerupToStr(p, true) + "!";
			if (p != PowerType.PowerType_ColorNuke && this.mFrog.LightningMode())
			{
				this.mLevel.DeactivateLightningEffects();
			}
			if (p == PowerType.PowerType_ProximityBomb)
			{
				this.PlayExplosionSound();
				if (this.mCurTreasure != null && MathUtils.CirclesIntersect((float)this.mCurTreasure.x, (float)this.mCurTreasure.y, (float)x, (float)y, 108f))
				{
					this.mApp.mUserProfile.mFruitBombed++;
					if (this.mApp.mUserProfile.mFruitBombed >= 8)
					{
						this.mApp.SetAchievement("fruit_bomb_8x");
					}
					this.DoHitTreasure();
					return;
				}
			}
			else
			{
				if (p == PowerType.PowerType_MoveBackwards)
				{
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BACKWARDS_BALL));
					this.AddText(t, x, y - 40);
					return;
				}
				if (p == PowerType.PowerType_SlowDown)
				{
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_SLOWDOWN_BALL));
					this.AddText(t, x, y - 40);
					return;
				}
				if (p == PowerType.PowerType_Accuracy)
				{
					this.AddText(t, x, y - 40);
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_ACCURACY_BALL));
					this.mAccuracyCount = 2000;
					this.DoAccuracy(true);
					return;
				}
				if (p == PowerType.PowerType_Cannon)
				{
					this.AddText(t, x, y - 40);
					this.mFrog.SetCannonCount(this.mApp.GetLevelMgr().mCannonShots, this.mApp.GetLevelMgr().mCannonStacks, color_type);
					return;
				}
				if (p == PowerType.PowerType_Laser)
				{
					this.AddText(t, x, y - 40);
					this.mFrog.DoLazerFrog(this.mApp.GetLevelMgr().mLazerShots, this.mApp.GetLevelMgr().mLazerStacks);
					return;
				}
				if (p == PowerType.PowerType_ColorNuke)
				{
					this.AddText(t, x, y - 40);
					this.UpdateGuide(true);
					this.mFrog.DoLightningFrog(true);
					return;
				}
				if (p == PowerType.PowerType_GauntletMultBall)
				{
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_MULT_ACTIVATED));
				}
			}
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00048AF2 File Offset: 0x00046CF2
		public void ActivatePower(PowerType p)
		{
			this.ActivatePower(p, -1, -1, -1);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00048B00 File Offset: 0x00046D00
		public int IncScore(int theInc, bool from_balls, bool counts_towards_zuma)
		{
			if (theInc <= 0 || (from_balls && !this.mLevel.AllowPointsFromBalls()) || this.mLevel.IsFinalBossLevel() || (from_balls && this.mLevel.mBoss != null))
			{
				return 0;
			}
			int num = this.mScore;
			if (this.mLevel.mZumaBarState == -1)
			{
				this.mLevel.mZumaBarState = 0;
			}
			if (this.GauntletMode() && this.mLevel.mCurMultiplierTimeLeft > 0)
			{
				theInc *= this.mScoreMultiplier;
				this.mGauntletPointsFromMult += theInc;
			}
			if (counts_towards_zuma || this.GauntletMode())
			{
				this.mScore += theInc;
			}
			int result = theInc;
			if (this.GauntletMode())
			{
				if (num < this.mLevel.mChallengePoints && this.mScore >= this.mLevel.mChallengePoints)
				{
					this.ToggleNotification(TextManager.getInstance().getString(692));
					this.mApp.mUserProfile.mChallengeStats.mNumTimesHitScoreTarget++;
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_CHALLENGE_SCORE_MET));
				}
				else if (num < this.mLevel.mChallengeAcePoints && this.mScore >= this.mLevel.mChallengeAcePoints)
				{
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_CHALLENGE_ACE_MET));
					this.ToggleNotification(TextManager.getInstance().getString(693));
				}
				this.mApp.GetLevelMgr();
				this.mRollerScore.SetTargetScore(this.mScore);
				if (GameApp.gDDS.SetGauntletPoints(this.mScore))
				{
					this.mLevel.UpdateChallengeModeDifficulty();
				}
				if (this.mRollerScore.GetTargetScore() > this.mApp.mUserProfile.mChallengeStats.mHighestScore)
				{
					this.mApp.mUserProfile.mChallengeStats.mHighestScore = this.mRollerScore.GetTargetScore();
				}
				while (theInc > 0)
				{
					if (this.mGauntletPointsForDiffInc + theInc >= this.mApp.GetLevelMgr().mNumPointsForTimeAdd)
					{
						theInc -= this.mApp.GetLevelMgr().mNumPointsForTimeAdd - this.mGauntletPointsForDiffInc;
						this.mGauntletPointsForDiffInc = 0;
						if (GameApp.gDDS.AddMultiplierTime(this.mApp.GetLevelMgr().mPointTimeAdd))
						{
							this.mLevel.UpdateChallengeModeDifficulty();
						}
					}
					else
					{
						this.mGauntletPointsForDiffInc += theInc;
						theInc = 0;
					}
				}
				if (this.mRollerScore.GetTargetScore() > this.mGauntletHSTarget && Board.gNeedsGauntletHSSound && this.mGauntletHSTarget > 0)
				{
					Board.gNeedsGauntletHSSound = false;
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_CHALLENGE_BEST_SCORE));
					this.AddText(TextManager.getInstance().getString(98), this.mFrog.GetCenterX() - 60, this.mFrog.GetCenterY() + 60, 3f, -1, null);
				}
			}
			else
			{
				this.mRollerScore.SetTargetScore(this.mRollerScore.GetTargetScore() + theInc);
				if (this.mAdventureMode)
				{
					this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvScore = this.mRollerScore.GetTargetScore();
					this.CheckIfGotExtraLife(theInc);
				}
				else if (this.IronFrogMode() && this.mScore > this.mApp.mUserProfile.mIronFrogStats.mBestScore)
				{
					this.mPrevIFBestScore = this.mApp.mUserProfile.mIronFrogStats.mBestScore;
					this.mApp.mUserProfile.mIronFrogStats.mBestScore = this.mScore;
				}
			}
			return result;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00048E8C File Offset: 0x0004708C
		public int IncScore(int theInc, bool from_balls)
		{
			return this.IncScore(theInc, from_balls, true);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00048E98 File Offset: 0x00047098
		public int GetPerfectBonus(int zone_override, int level_override)
		{
			if (zone_override == -1)
			{
				zone_override = this.mLevel.mZone;
			}
			if (level_override == -1)
			{
				level_override = this.mLevel.mNum;
			}
			if (level_override <= 10 && this.mWasPerfectLevel && !this.IronFrogMode())
			{
				return 1000 * zone_override;
			}
			return 0;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00048EE5 File Offset: 0x000470E5
		public int GetPerfectBonus()
		{
			return this.GetPerfectBonus(-1, -1);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00048EEF File Offset: 0x000470EF
		public Ball GetGuideBall()
		{
			return this.mGuideBall;
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00048EF7 File Offset: 0x000470F7
		public void GuideBallInvalidated()
		{
			this.mGuideBall = null;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00048F00 File Offset: 0x00047100
		public void GetGuideTargetCenter(out float x, out float y, bool lazer)
		{
			if (!lazer)
			{
				x = this.mGuideCenter.x;
				y = this.mGuideCenter.y;
				return;
			}
			x = this.mLazerGuideCenter.x;
			y = this.mLazerGuideCenter.y;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00048F3C File Offset: 0x0004713C
		public BonusTextElement AddText(string t, int x, int y, float bulge_pct, int attach_handle, Font theFont)
		{
			if (this.mLevel.mBoss != null || this.mLevel.IsFinalBossLevel())
			{
				return null;
			}
			if (bulge_pct > 3f)
			{
				bulge_pct = 3f;
			}
			Font font = ((theFont == null) ? Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_STROKE) : theFont);
			x = ZumasRevenge.Common._S(x);
			y = ZumasRevenge.Common._S(y);
			BonusText bonusText = new BonusText(t, font, (float)x, (float)y, ZumasRevenge.Common._M(1f), ZumasRevenge.Common._M1(200));
			bonusText.SetAlphaDecRate(ZumasRevenge.Common._M(25f));
			bonusText.NoHSL();
			BonusTextElement bonusTextElement = new BonusTextElement();
			this.mText.Add(bonusTextElement);
			bonusTextElement.mParentHandle = attach_handle;
			bonusTextElement.mBonus = bonusText;
			bonusTextElement.mHandle = ++Board.gTextHandle;
			BonusTextElement bonusTextElement2 = null;
			if (attach_handle != -1)
			{
				int i = 0;
				while (i < this.mText.size<BonusTextElement>())
				{
					if (this.mText[i].mHandle == attach_handle)
					{
						bonusTextElement2 = this.mText[i];
						this.mText[i].mAttachedTo.Add(bonusTextElement.mHandle);
						if (bonusTextElement2.mAttachedTo.size<int>() == 1)
						{
							bonusText.SetY(bonusTextElement2.mBonus.GetY() + (float)font.GetHeight());
							break;
						}
						int num = bonusTextElement2.mAttachedTo[bonusTextElement2.mAttachedTo.size<int>() - 2];
						for (int j = 0; j < this.mText.size<BonusTextElement>(); j++)
						{
							if (this.mText[j].mHandle == num)
							{
								bonusText.SetY(this.mText[j].mBonus.GetY() + (float)font.GetHeight());
								break;
							}
						}
						break;
					}
					else
					{
						i++;
					}
				}
				int num2 = (int)bonusTextElement2.mBonus.GetX();
				int num3 = font.StringWidth(bonusTextElement2.mBonus.GetString());
				bonusText.SetX((float)(num2 + (num3 - font.StringWidth(t)) / 2));
			}
			bool flag = true;
			int num4 = (flag ? 0 : ZumasRevenge.Common._S(-80));
			int num5 = (flag ? this.mWidth : (this.mWidth + ZumasRevenge.Common._S(80)));
			List<BonusText> list = new List<BonusText>();
			Rect rect;
			if (bonusTextElement2 == null)
			{
				rect = new Rect(x, y, font.StringWidth(t), font.GetHeight());
				list.Add(bonusText);
			}
			else
			{
				list.Add(bonusTextElement2.mBonus);
				rect = new Rect((int)bonusTextElement2.mBonus.GetX(), (int)bonusTextElement2.mBonus.GetY(), font.StringWidth(bonusTextElement2.mBonus.GetString()), font.GetHeight() * 2);
				for (int k = 0; k < bonusTextElement2.mAttachedTo.size<int>(); k++)
				{
					int num6 = bonusTextElement2.mAttachedTo[k];
					int l = 0;
					while (l < this.mText.size<BonusTextElement>())
					{
						if (this.mText[l].mHandle == num6)
						{
							BonusText mBonus = this.mText[l].mBonus;
							list.Add(mBonus);
							rect.mHeight += font.GetHeight();
							int num7 = font.StringWidth(mBonus.GetString());
							if (num7 > rect.mWidth)
							{
								rect.mWidth = num7;
							}
							if (mBonus.GetX() < (float)rect.mX)
							{
								rect.mX = (int)mBonus.GetX();
								break;
							}
							break;
						}
						else
						{
							l++;
						}
					}
				}
			}
			Rect rect2 = rect;
			rect2.mWidth *= (int)bulge_pct;
			rect2.mHeight *= (int)bulge_pct;
			int num8 = 0;
			int num9 = 0;
			if (rect.mX < num4)
			{
				num8 = num4 - rect.mX;
			}
			else if (rect.mX + rect.mWidth > num5)
			{
				num8 = num5 - (rect.mWidth + rect.mX);
			}
			if (rect.mY < 0)
			{
				num9 = -rect.mY;
			}
			else if (rect.mY + rect.mHeight > this.mHeight)
			{
				num9 = this.mHeight - (rect.mHeight + rect.mY);
			}
			for (int m = 0; m < list.size<BonusText>(); m++)
			{
				list[m].SetX(list[m].GetX() + (float)num8);
				list[m].SetY(list[m].GetY() + (float)num9);
			}
			if (!MathUtils._eq(bulge_pct, 1f))
			{
				float num10 = (float)ZumasRevenge.Common._M(2);
				if (bulge_pct > 1.51f)
				{
					num10 += 1f;
				}
				float num11 = ZumasRevenge.Common._M(1f);
				float num12 = num11 / num10;
				float num13 = (6f * num11 - num12 * 6f) / 0.1f;
				num11 = bulge_pct - 1f;
				num12 = num11 / num10;
				float rate = (6f * num11 - num12 * 6f) / num13;
				bonusText.Bulge(bulge_pct, rate, (int)num10);
				if (attach_handle != -1 && bonusTextElement2 != null)
				{
					bonusTextElement2.mBonus.Bulge(bulge_pct, rate, (int)num10);
					for (int n = 0; n < bonusTextElement2.mAttachedTo.size<int>(); n++)
					{
						for (int num14 = 0; num14 < this.mText.size<BonusTextElement>(); num14++)
						{
							if (this.mText[num14].mHandle == bonusTextElement2.mAttachedTo[n])
							{
								this.mText[num14].mBonus.Bulge(bulge_pct, rate, (int)num10);
								break;
							}
						}
					}
				}
			}
			return bonusTextElement;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x000494E9 File Offset: 0x000476E9
		public BonusTextElement AddText(string t, int x, int y)
		{
			return this.AddText(t, x, y, 1f, -1, null);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x000494FB File Offset: 0x000476FB
		public void DrawRollerScore(Graphics g)
		{
			this.mRollerScore.Draw(g);
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00049509 File Offset: 0x00047709
		public void CheckShouldClearGuideBall(Ball b)
		{
			if (this.mGuideBall != null && this.mGuideBall == b)
			{
				this.mGuideBall = null;
				this.mRecalcGuide = true;
				this.mRecalcLazerGuide = true;
			}
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00049534 File Offset: 0x00047734
		public CurveMgr GetCurve(Ball b)
		{
			if (this.mLevel == null)
			{
				return null;
			}
			for (int i = 0; i < this.mLevel.mNumCurves; i++)
			{
				CurveMgr curveMgr = this.mLevel.mCurveMgr[i];
				if (curveMgr.mBallList.Contains(b))
				{
					return curveMgr;
				}
				if (curveMgr.mPendingBalls.Contains(b))
				{
					return curveMgr;
				}
			}
			return null;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00049590 File Offset: 0x00047790
		public void CueLevelTransition(int next_level_override, bool dont_record_stats)
		{
			this.mBossSmScale.SetConstant(1.0);
			this.mStatsBubbleScale.SetCurve(ZumasRevenge.Common._MP("b+0,2,0.003077,1,####         %####fW###9P###"));
			if (this.mLevelTransition == null)
			{
				this.mFrog.ResetFrogType();
				this.SetMenuBtnEnabled(false);
				if (!this.mLevel.mFinalLevel)
				{
					this.mLevelTransition = new LevelTransition(next_level_override, dont_record_stats);
					this.mLevelTransition.mTransitionToStats = !this.IronFrogMode();
					this.mLevelTransition.mIntroDelay = ZumasRevenge.Common._M(150);
					if (!this.mLevelTransition.mDontRecordStats && this.mLevel.mBoss != null)
					{
						GameApp.gDDS.BossLevelComplete();
					}
				}
				else
				{
					GameApp.gDDS.BossLevelComplete();
				}
				this.mLevelPoints = this.mScore - this.mLevelBeginScore;
				if (this.mLevel.mBoss != null)
				{
					if (this.mLevel.mFinalLevel)
					{
						return;
					}
				}
				else if (this.IronFrogMode() && !this.GauntletMode())
				{
					this.GetBetaStats().BeatLevel(this.mLevelStats.mTimePlayed, this.mLevel.mParTime, this.GetAceTimeBonus(), this.GetPerfectBonus(), (float)this.mLevel.mFurthestBallDistance / 100f, this.mScore - this.mLevelBeginScore, this.mScore, -1);
					if (this.mScore > this.mApp.mUserProfile.mHighestIronFrogScore)
					{
						this.mNewIronFrogHS = true;
						this.mApp.mUserProfile.mHighestIronFrogScore = this.mScore;
						this.mApp.mUserProfile.mHighestIronFrogLevel = this.mApp.GetLevelMgr().GetLevelIndex(this.mLevel.mId) - this.mApp.GetLevelMgr().GetFirstIronFrogLevel() + 1;
						this.mApp.SaveProfile();
					}
					if (ZumasRevenge.Common.StrEquals(this.mLevel.mId, this.mApp.GetLevelMgr().GetLevelId(this.mApp.GetLevelMgr().GetLastIronFrogLevel())))
					{
						return;
					}
				}
				else if (!this.IronFrogMode() && !this.GauntletMode())
				{
					this.mCurStatsPointCounter = (this.mCurStatsPointTarget = (this.mCurStatsPointInc = 0));
					int num = this.mLevel.mParTime - this.mLevelStats.mTimePlayed;
					if (num > 0)
					{
						int aceTimeBonus = this.GetAceTimeBonus();
						this.mCurStatsPointTarget = aceTimeBonus;
						this.mCurStatsPointInc = aceTimeBonus / ZumasRevenge.Common._M(200);
						if (this.mCurStatsPointInc <= 0)
						{
							this.mCurStatsPointInc = 1;
						}
					}
					this.mStatsState = 0;
					this.mStatsDelay = 0;
					if (this.mCurStatsPointInc <= 0)
					{
						this.mCurStatsPointInc = 1;
					}
					int aceTimeBonus2 = this.GetAceTimeBonus();
					int perfectBonus = this.GetPerfectBonus();
					this.CheckIfGotExtraLife(aceTimeBonus2 + perfectBonus);
					this.mScore += aceTimeBonus2;
					this.mScore += perfectBonus;
					this.mScore += this.mCurveClearBonus;
					this.mRollerScore.ForceScore(this.mScore);
				}
				this.mHasDoneIntroSounds = false;
				this.mWasPerfectLevel = this.mApp.mUserProfile.GetAdvModeVars().mNumDeathsCurLevel == 0;
				this.mNumDeaths = this.mApp.mUserProfile.GetAdvModeVars().mNumDeathsCurLevel;
				int mNumZumasCurLevel = this.mApp.mUserProfile.GetAdvModeVars().mNumZumasCurLevel;
				int mNumDeathsCurLevel = this.mApp.mUserProfile.GetAdvModeVars().mNumDeathsCurLevel;
				this.mApp.mUserProfile.GetAdvModeVars().mNumDeathsCurLevel = 0;
				this.mApp.mUserProfile.GetAdvModeVars().mNumZumasCurLevel = 0;
				if (!this.IronFrogMode() && !this.mGauntletMode && this.mScore > this.mApp.mUserProfile.mHighestAdvModeScore && this.mLevel.mBoss == null)
				{
					this.mApp.mUserProfile.mHighestAdvModeScore = this.mScore;
					this.mApp.mUserProfile.mAdvModeHSLevel = this.mLevel.mNum;
					this.mApp.mUserProfile.mAdvModeHSZone = this.mLevel.mZone;
					this.mApp.SaveProfile();
				}
				this.ResetInARowBonus();
				this.mGameStats.Add(this.mLevelStats);
				this.mStatsString = this.mLevel.GetStatsScreenText(this.mLevelStats, this.mLevelPoints);
				this.GetBetaStats().BeatLevel(this.mLevelStats.mTimePlayed, this.mLevel.mParTime, this.GetAceTimeBonus(), this.GetPerfectBonus(), (float)this.mLevel.mFurthestBallDistance / 100f, this.mScore - this.mLevelBeginScore, this.mScore, this.mLives);
				if (this.mLevel.mPreviewText != null && this.mLevel.mPreviewText.Length > 0)
				{
					string text = this.mStatsString;
					this.mStatsString = string.Concat(new string[]
					{
						text,
						"\n\n^FFFFFF^",
						TextManager.getInstance().getString(694),
						"\n",
						this.mLevel.mPreviewText
					});
					return;
				}
			}
			else if (this.mLevelTransition.mTransitionToStats)
			{
				if (GlobalMembers.gIs3D)
				{
					this.mTransitionScreen = this.mApp.mGraphicsDriver.GetScreenImage();
					Graphics graphics = new Graphics(this.mTransitionScreen);
					graphics.Translate(-this.mApp.mScreenBounds.mX, 0);
					this.mWidgetManager.DrawWidgetsTo(graphics);
					this.mTransitionScreenImage = this.mTransitionScreen;
					this.mDoingTransition = true;
					int num2 = ZumasRevenge.Common._M(35);
					int num3 = Res.GetOffsetXByID(ResID.IMAGE_UI_ADVENTURE_STATS_FROG) / 2 - ZumasRevenge.Common._M(63) + (this.mLevel.mNum - 1) * num2;
					int num4 = ((this.mLevel.mNum == 10) ? num3 : (num3 + num2));
					int theX = num4;
					int theY = Res.GetOffsetYByID(ResID.IMAGE_UI_ADVENTURE_STATS_FROG) / 2 + ZumasRevenge.Common._M(20);
					this.mTransitionCenter = new SexyPoint(theX, theY);
					this.mTransitionScreenHolePct.SetCurve(ZumasRevenge.Common._MP("b+0,1,0.006667,1,####    r.-0S     /d_0g"));
					this.mTransitionScreenScale.SetCurve(ZumasRevenge.Common._MP("b+1,1.5,0.005,1,####   T####      L~_T6"));
					this.mTransitionFrogRotPct.SetCurve(ZumasRevenge.Common._MP("b+0,1,0.005,1,####   L####      T~P##"));
					this.mTransitionFrogScale.SetCurve(ZumasRevenge.Common._MP("b+0,4,0.005,1,-###   T.###M53*v   IeZ_]  V:;R-"), this.mTransitionFrogRotPct);
					this.mTransitionFrogPosPct.SetCurve(ZumasRevenge.Common._MP("b+0,1,0,1,####   T#P##   *M2h3   E~###"), this.mTransitionFrogRotPct);
				}
				this.mLevelTransition.mTransitionToStats = false;
				this.mLevelTransition.Reset(false);
				if (GlobalMembers.gIs3D)
				{
					this.mLevelTransition.mSilent = true;
				}
				if (this.mLevel.IsFinalBossLevel())
				{
					this.mLevelTransition.mDrawFrogEffect = false;
				}
				if (this.mLevel.mBoss == null && !this.mLevel.IsFinalBossLevel())
				{
					this.ContinueToNextLevel();
					this.UpdateGunPos();
					if (this.mFrog.mDestCount == 0)
					{
						this.mFrog.mDestX2 = (int)this.mFrog.mCurX;
						this.mFrog.mDestY2 = (int)this.mFrog.mCurY;
					}
					else
					{
						this.mFrog.mDestX1 = this.mFrog.mDestX2;
						this.mFrog.mDestY1 = this.mFrog.mDestY2;
					}
					this.mFrog.mCenterX = (float)this.mFrog.mDestX2;
					this.mFrog.mCenterY = (float)this.mFrog.mDestY2;
				}
			}
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x00049D2E File Offset: 0x00047F2E
		public void CueLevelTransition()
		{
			this.CueLevelTransition(-1, false);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x00049D38 File Offset: 0x00047F38
		public void SetupStatsScreen()
		{
			this.mStatsContinueBtn = new ButtonWidget(2, this);
			this.mStatsContinueBtn.mOverImage = Res.GetImageByID(ResID.IMAGE_GUI_ADVENTURESTATS_CONTINUE);
			this.mStatsContinueBtn.mButtonImage = Res.GetImageByID(ResID.IMAGE_GUI_ADVENTURESTATS_CONTINUE);
			this.mStatsContinueBtn.mDownImage = Res.GetImageByID(ResID.IMAGE_GUI_ADVENTURESTATS_CONTINUE_DOWN);
			this.mStatsContinueBtn.mDoFinger = true;
			Rect continueButtonRect = this.GetContinueButtonRect();
			this.mStatsContinueBtn.Resize(continueButtonRect);
			this.mBossIntroBGAlpha.SetConstant(0.0);
			this.mBossSmScale.SetConstant(1.0);
			this.mBossSmPosPct.SetConstant(0.0);
			this.mBossRedPct.SetConstant(0.0);
			this.AddWidget(this.mStatsContinueBtn);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00049E0C File Offset: 0x0004800C
		public Rect GetContinueButtonRect()
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_ADVENTURESTATS_CONTINUE);
			int width = imageByID.GetWidth();
			int height = imageByID.GetHeight();
			int theX = (int)((float)(this.mAStatsFrame.mX + this.mAStatsFrame.mWidth) - (float)width * 0.84f);
			int theY = (int)((float)(this.mAStatsFrame.mY + this.mAStatsFrame.mHeight) - (float)height * 0.8f);
			return new Rect(theX, theY, width, height);
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00049E84 File Offset: 0x00048084
		public void ContinueToNextLevel(int next_level_override, bool did_level_transition)
		{
			if (!did_level_transition)
			{
				this.ResetInARowBonus();
				this.mLevelStats.mTimePlayed = this.mStateCount - this.mIgnoreCount;
				this.mGameStats.Add(this.mLevelStats);
			}
			else if (this.mDarkFrogSequence == null)
			{
				this.SetMenuBtnEnabled(true);
			}
			this.DoAccuracy(false);
			for (int i = 0; i < this.mLevel.mNumCurves; i++)
			{
				this.mLevel.mCurveMgr[i].DeleteBalls();
			}
			this.DeleteBullets();
			this.mFrog.EmptyBullets();
			this.mFrog.ClearBubbles();
			this.mStateCount = 0;
			this.mFruitMultiplier = 1;
			this.mScoreMultiplier = 1;
			this.Reset(false, true, false, false);
			this.mDoPostBossMapScreen = false;
			if (next_level_override == -1)
			{
				int level_num = this.mLevelNum;
				if (this.mLevelNum < this.mTheNextLevel)
				{
					level_num = this.mLevelNum + 1;
				}
				if (this.mNextLevelIdOverride.Length > 0)
				{
					this.StartLevel(this.mNextLevelIdOverride, this.mIsLoading, false, false, null);
				}
				else if (!this.StartLevel(level_num, this.mIsLoading, false, false))
				{
					this.StartLevel(this.mLevelNum, this.mIsLoading, false, false);
				}
			}
			else
			{
				this.StartLevel(next_level_override, this.mIsLoading, false, false);
			}
			if (this.IsCheckpointLevel() && !this.mGauntletMode && !this.IronFrogMode())
			{
				int mNum = this.mLevel.mNum;
				if (mNum != 1)
				{
					if (mNum == 2147483647)
					{
						this.mApp.mUserProfile.GetAdvModeVars().mCheckpointScores[this.mLevel.mZone - 1].mBoss = this.mScore;
					}
					else
					{
						this.mApp.mUserProfile.GetAdvModeVars().mCheckpointScores[this.mLevel.mZone - 1].mMidpoint = this.mScore;
					}
				}
				else
				{
					this.mApp.mUserProfile.GetAdvModeVars().mCheckpointScores[this.mLevel.mZone - 1].mZoneStart = this.mScore;
				}
			}
			this.mTheNextLevel = this.mLevelNum + 1;
			bool flag = this.mLevel.mBoss != null;
			this.SetNextLevelMusic(flag);
			if (!flag || this.mLevel.mBoss.mResGroup != "Boss6_DarkFrog")
			{
				this.PlayLevelMusic(0.005f);
			}
			this.mApp.mSoundPlayer.Stop(Res.GetSoundByID(ResID.SOUND_NEW_ADV_STATS_TALLY));
			this.SaveGame(this.mApp.mUserProfile.GetSaveGameName(this.mApp.IsHardMode()), null);
			this.UpdateGunPos(true);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0004A120 File Offset: 0x00048320
		public void ContinueToNextLevel()
		{
			this.ContinueToNextLevel(-1, true);
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0004A12C File Offset: 0x0004832C
		public void SetNextLevelMusic(bool isBossLevel)
		{
			if (isBossLevel)
			{
				Board.gTuneNum = 6;
				return;
			}
			int num;
			do
			{
				num = SexyFramework.Common.Rand(6);
			}
			while (Board.gTuneNum == num);
			Board.gTuneNum = num;
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0004A158 File Offset: 0x00048358
		public void PlayLevelMusic(float inFadeSpeed)
		{
			int song = 12;
			switch (Board.gTuneNum)
			{
			case 0:
				song = 12;
				break;
			case 1:
				song = 24;
				break;
			case 2:
				song = 35;
				break;
			case 3:
				song = 45;
				break;
			case 4:
				song = 58;
				break;
			case 5:
				song = 71;
				break;
			case 6:
				song = 127;
				break;
			}
			this.mApp.PlaySong(song, inFadeSpeed);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0004A1C0 File Offset: 0x000483C0
		public void EndGauntletMode(bool hit_max_time)
		{
			if (this.mGauntletRetryBtn != null || this.mGauntletQuitBtn != null || this.mEndGauntletTimer > 0 || this.mGauntletModeOver)
			{
				return;
			}
			this.mApp.mSoundPlayer.Stop(Res.GetSoundByID(ResID.SOUND_LIGHTNING_LOOP));
			this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_TIME_UP_CHALLENGE));
			this.GetBetaStats().BeatLevel(this.mLevelStats.mTimePlayed, this.mLevel.mParTime, this.mScoreMultiplier, hit_max_time ? (this.mScore - this.mGauntletFinalScorePreBonus) : 0, (float)this.mLevel.mFurthestBallDistance / 100f, this.mScore - this.mLevelBeginScore, this.mScore, -1);
			this.mGauntletModeOver = true;
			this.mApp.PlaySong(138);
			int num = 0;
			if (hit_max_time)
			{
				this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_TIME_UP_CHALLENGE));
				this.mEndGauntletTimer = Board.END_GAUNTLET_TIME;
				if (this.mLevel.mCurMultiplierTimeLeft > 0)
				{
					this.GauntletMultiplierEnded();
					this.mGauntletMultBarAlpha = (float)ZumasRevenge.Common._M(600);
					if (this.mRollerScore.GetTargetScore() > this.mApp.mUserProfile.mChallengeStats.mHighestScore)
					{
						this.mApp.mUserProfile.mChallengeStats.mHighestScore = this.mRollerScore.GetTargetScore();
					}
				}
				num = (int)((float)this.mScoreMultiplier / 100f * (float)this.mScore);
				if (num >= 25000)
				{
					this.mApp.SetAchievement("survival_25k");
				}
				int num2 = num + this.mRollerScore.GetTargetScore();
				if (num2 > this.mApp.mUserProfile.mChallengeStats.mHighestScore)
				{
					this.mApp.mUserProfile.mChallengeStats.mHighestScore = num2;
				}
				this.mApp.mUserProfile.mChallengeUnlockState[this.mLevel.mZone - 1, this.mLevel.mNum - 1] = 3;
			}
			int num3 = this.mApp.mUserProfile.mChallengeUnlockState[this.mLevel.mZone - 1, this.mLevel.mNum - 1];
			bool flag = num3 > 3;
			bool flag2 = false;
			int num4 = this.mApp.mUserProfile.ChallengeCupComplete(this.mLevel.mZone);
			if (this.mScore + num >= this.mLevel.mChallengeAcePoints)
			{
				flag2 = true;
				this.mApp.mUserProfile.mChallengeUnlockState[this.mLevel.mZone - 1, this.mLevel.mNum - 1] = 5;
				int num5 = 0;
				for (int i = 1; i <= 7; i++)
				{
					if (this.mApp.mUserProfile.ChallengeCupComplete(i) == 2)
					{
						num5++;
					}
				}
				if (num5 >= 4)
				{
					this.mApp.SetAchievement("ace_4_cups");
				}
			}
			else if (this.mScore + num >= this.mLevel.mChallengePoints && num3 != 5)
			{
				flag2 = true;
				this.mApp.mUserProfile.mChallengeUnlockState[this.mLevel.mZone - 1, this.mLevel.mNum - 1] = 4;
			}
			int num6 = this.mApp.mUserProfile.ChallengeCupComplete(this.mLevel.mZone);
			if (num4 != num6)
			{
				if (num6 == 1)
				{
					this.mApp.mUserProfile.mDoChallengeCupComplete = true;
				}
				else if (num6 == 2)
				{
					this.mApp.mUserProfile.mDoChallengeAceCupComplete = true;
				}
				else if (num6 == 2)
				{
					this.mApp.mUserProfile.mDoAceCupXFade = true;
				}
			}
			if (this.mScore >= this.mLevel.mChallengePoints && num3 <= 2)
			{
				this.mApp.mUserProfile.mDoChallengeTrophyZoom = true;
			}
			if (this.mScore >= this.mLevel.mChallengeAcePoints && num3 <= 4)
			{
				this.mApp.mUserProfile.mDoChallengeAceTrophyZoom = true;
			}
			if (!this.mLevel.mIronFrog && this.mScore + num >= this.mLevel.mChallengePoints && !flag && flag2)
			{
				this.UnlockAchievement(EAchievementType.CHALLENGE_ACCEPTED);
				int num7 = 0;
				int num8 = 0;
				while (num8 < 10 && num7 < 2)
				{
					if (this.mApp.mUserProfile.mChallengeUnlockState[this.mLevel.mZone - 1, num8] == 1)
					{
						this.mApp.mUserProfile.mChallengeUnlockState[this.mLevel.mZone - 1, num8] = 2;
						if (num7 == 0)
						{
							this.mApp.mUserProfile.mUnlockSparklesIdx1 = num8 + (this.mLevel.mZone - 1) * 10;
						}
						else
						{
							this.mApp.mUserProfile.mUnlockSparklesIdx2 = num8 + (this.mLevel.mZone - 1) * 10;
						}
						num7++;
					}
					num8++;
				}
			}
			if (!hit_max_time)
			{
				this.SetupEndOfGauntletTransition(false);
			}
			for (int j = 0; j < 6; j++)
			{
				for (int k = 0; k < 10; k++)
				{
					if (this.mApp.mUserProfile.mChallengeUnlockState[j, k] < 3)
					{
						return;
					}
				}
			}
			this.UnlockAchievement(EAchievementType.FE_RROG);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0004A6F0 File Offset: 0x000488F0
		public void BossDied()
		{
			if (this.mLevel.mBoss != null && this.mLevel.mBoss.GetHP() <= 0f)
			{
				this.mLevelStats.mTimePlayed = this.mStateCount - this.mIgnoreCount;
				this.mApp.ReportEndOfLevelMetrics(this, true, false);
				this.mApp.mMusic.FadeOut();
				this.GetBetaStats().BeatLevel(this.mLevelStats.mTimePlayed, this.mLevel.mParTime, this.GetAceTimeBonus(), this.GetPerfectBonus(), (float)this.mLevel.mFurthestBallDistance / 100f, this.mScore - this.mLevelBeginScore, this.mScore, this.mLives);
				this.SetHallucinateTimer(0);
				this.mFrog.EmptyBullets();
				this.mLevel.mInvertMouseTimer = 0;
				this.mApp.mUserProfile.GetAdvModeVars().mPerfectZone = true;
				this.mApp.mUserProfile.GetAdvModeVars().mNumDeathsCurLevel = 0;
				this.mApp.mUserProfile.GetAdvModeVars().mNumZumasCurLevel = 0;
				int num = this.mLevel.mZone * 10;
				if (num > this.mApp.mUserProfile.GetAdvModeVars().mHighestLevelBeat)
				{
					this.mApp.mUserProfile.GetAdvModeVars().mHighestLevelBeat = num;
				}
				if (this.mLevel.mBoss.mDeathText.size<BossText>() > 0)
				{
					this.mLevel.mBoss.MoveToDeathPosition((float)ZumasRevenge.Common._M(400), (float)ZumasRevenge.Common._M1(100));
					this.mFrog.MoveToBossDeathPosition((float)Board.FROG_DEATH_X, (float)Board.FROG_DEATH_Y);
				}
				if (this.mLevel.mBoss.mDeathText.size<BossText>() == 0)
				{
					if (this.mLevel.mZone == 6)
					{
						this.mStateCount = 0;
						if (this.mLevel.mEndSequence == 2)
						{
							this.mGameState = GameState.GameState_Boss6FakeCredits;
							if (this.mFakeCredits != null)
							{
								this.mFakeCredits.Dispose();
								this.mFakeCredits = null;
							}
							this.mFakeCredits = new FakeCredits();
							this.mFakeCredits.Init(this.mFrog);
							this.SetMenuBtnEnabled(false);
						}
						else if (this.mLevel.mEndSequence == 3)
						{
							this.mGameState = GameState.GameState_Boss6DarkFrog;
							this.mDarkFrogSequence = new DarkFrogSequence();
							this.mDarkFrogSequence.Init();
							this.SetMenuBtnEnabled(false);
							this.mDrawBossUI = false;
							this.mDarkFrogTimer = ZumasRevenge.Common._M(600);
							this.MakeBoss6VolcanoMeltComp();
							this.mDarkFrogBulletX = (float)(-(float)(ZumasRevenge.Common._DS(this.mBoss6VolcanoMelt.mWidth) / 2 - ZumasRevenge.Common._S(this.mLevel.mBoss.GetX()))) + this.mDarkFrogBulletX + (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(315));
							this.mDarkFrogBulletY = (float)(-(float)(ZumasRevenge.Common._DS(this.mBoss6VolcanoMelt.mHeight) / 2 - ZumasRevenge.Common._S(this.mLevel.mBoss.GetY()))) + this.mDarkFrogBulletY + (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(305));
							this.mDarkFrogBulletVX = ((float)ZumasRevenge.Common._S(this.mFrog.GetCenterX()) - this.mDarkFrogBulletX) / (float)ZumasRevenge.Common._M(50);
							this.mDarkFrogBulletVY = ((float)ZumasRevenge.Common._S(this.mFrog.GetCenterY()) - this.mDarkFrogBulletY) / (float)ZumasRevenge.Common._M(50);
							this.mEssenceScaleTimer = 0;
							this.mEssenceXScale = (this.mEssenceYScale = 0.74f);
						}
						else
						{
							this.mGameState = GameState.GameState_Boss6Transition;
						}
					}
					else
					{
						this.mGameState = GameState.GameState_LevelUp;
						if (this.mApp.GetLevelMgr().mScoreTips.size<ScoreTip>() > 0)
						{
							this.mScoreTipIdx = this.mApp.GetLevelMgr().GetScoreTipIdx((this.mLevel.mZone - 1) * 10 + this.mLevel.mNum);
						}
					}
				}
				else
				{
					if (this.mApp.mUserProfile.GetAdvModeVars().mNumTimesZoneBeat[5] == 0 && JeffLib.Common.StrFindNoCase(this.mLevel.mId, "debug") == -1)
					{
						this.mApp.mUserProfile.GetAdvModeVars().mHighestZoneBeat = this.mLevel.mBoss.mNum;
					}
					if (JeffLib.Common.StrFindNoCase(this.mLevel.mId, "debug") == -1)
					{
						this.mApp.mUserProfile.GetAdvModeVars().mNumTimesZoneBeat[this.mLevel.mZone - 1]++;
					}
					if (this.mLevel.mEndSequence == 5)
					{
						AdvModeTempleStats advModeTempleStats = this.GetAdvModeTempleStats();
						if (advModeTempleStats.mCurrentTime < advModeTempleStats.mBestTime)
						{
							advModeTempleStats.mBestTime = advModeTempleStats.mCurrentTime;
						}
						this.mTimeToBeatAdvMode = advModeTempleStats.mCurrentTime;
						advModeTempleStats.mCurrentTime = 0;
						if (this.mApp.mUserProfile.mChallengeUnlockState[6, 0] < 2)
						{
							this.mApp.mUserProfile.mChallengeUnlockState[6, 0] = 2;
						}
						int num2 = this.mScore + this.mLives * this.mApp.GetLevelMgr().mBeatGamePointsForLife;
						if (num2 > advModeTempleStats.mBestScore)
						{
							advModeTempleStats.mBestScore = num2;
						}
						PIEffect pieffectByID = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_TORCHFLAME);
						pieffectByID.ResetAnim();
						pieffectByID.mEmitAfterTimeline = true;
						this.mBeatGameLives = this.mLives - 1;
						if (this.mBeatGameLives < 0)
						{
							this.mBeatGameLives = 0;
						}
						this.mBeatGameNormalScore = this.mScore;
						this.mBeatGameTotalScoreTally = 0;
						this.mForceRestartInAdvMode = true;
						this.mLevelNum = 0;
						this.mApp.mUserProfile.GetAdvModeVars().mHighestZoneBeat = 6;
						if (!this.mApp.IsHardMode())
						{
							this.mApp.SetAchievement("beat_adventure");
							this.mApp.mUserProfile.mFirstTimeReplayingNormalMode = true;
						}
						else
						{
							this.mApp.SetAchievement("beat_heroic");
							this.mApp.mUserProfile.mFirstTimeReplayingHardMode = true;
						}
						this.mLives = 3;
						this.mScore = 0;
						this.mPointsLeftForExtraLife = this.mApp.GetLevelMgr().mPointsForLife;
						this.mRollerScore.ForceScore(0);
						this.mApp.mUserProfile.GetAdvModeVars().mCurrentAdvScore = 0;
						this.mApp.mUserProfile.GetAdvModeVars().mNumTimesZoneBeat[5]++;
					}
					this.SetMenuBtnEnabled(false);
					this.mGameState = GameState.GameState_BossDead;
					this.mLevel.mBoss.mDoDeathExplosions = true;
					this.mStateCount = 0;
					this.mFrog.LevelReset();
				}
				if (this.mAdventureMode)
				{
					switch (this.mLevel.mZone)
					{
					case 1:
						this.UnlockAchievement(EAchievementType.JAW_BREAKER);
						return;
					case 2:
						this.UnlockAchievement(EAchievementType.TIKI_TRAMPLER);
						return;
					case 3:
						this.UnlockAchievement(EAchievementType.BONE_PICKER);
						return;
					case 4:
						this.UnlockAchievement(EAchievementType.PESTILENCE_PACIFIER);
						return;
					case 5:
						this.UnlockAchievement(EAchievementType.CEPHALOPOD_SMASHE);
						return;
					case 6:
						if (this.mLevel.mEndSequence > 3)
						{
							this.UnlockAchievement(EAchievementType.TIME_FOR_TADPOLES);
						}
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0004AE03 File Offset: 0x00049003
		public void CannonDisabled()
		{
			this.mShowGuide = false;
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0004AE0C File Offset: 0x0004900C
		public void DoClearCurveBonus(int curve_num)
		{
			if (this.GauntletMode())
			{
				int mGauntletCurTime = this.mLevel.mGauntletCurTime;
			}
			else if (this.mGameState != GameState.GameState_Playing)
			{
				int mTimePlayed = this.mEndLevelStats.mTimePlayed;
			}
			if (this.mAdventureMode)
			{
				this.GetAdvModeTempleStats().mNumClearCurves++;
			}
			int num = (this.GauntletMode() ? ZumasRevenge.Common._M(1000) : (this.mApp.GetLevelMgr().mClearCurvePoints * this.mLevel.mZone));
			this.IncScore(num, false, false);
			this.mCurveClearBonus += num;
			BonusTextElement bonusTextElement = this.AddText(TextManager.getInstance().getString(99), this.mFrog.GetCenterX(), this.mFrog.GetCenterY());
			if (bonusTextElement == null)
			{
				return;
			}
			bonusTextElement = this.AddText("+" + num + "!", 0, 0, ZumasRevenge.Common._M(1.55f), bonusTextElement.mHandle, null);
			if (!this.GauntletMode() && !this.IronFrogMode())
			{
				this.LivesChanged(1);
			}
			this.GetBetaStats().ClearedCurve(num);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0004AF28 File Offset: 0x00049128
		public void DrawTunnelMasks(Graphics g, int pri)
		{
			Graphics3D graphics3D = g.Get3D();
			if (graphics3D == null || pri >= 5 || this.mTunnels[pri].size<Tunnel>() == 0)
			{
				return;
			}
			for (int i = 0; i < this.mTunnels[pri].size<Tunnel>(); i++)
			{
				Tunnel tunnel = this.mTunnels[pri][i];
				if (tunnel.mLayerId.Length == 0)
				{
					g.DrawImage(tunnel.mImage, ZumasRevenge.Common._S(tunnel.mX + GameApp.gScreenShakeX), ZumasRevenge.Common._S(tunnel.mY + GameApp.gScreenShakeY));
				}
				else
				{
					g.DrawImage(tunnel.mImage, ZumasRevenge.Common._DS(tunnel.mX + GameApp.gScreenShakeX - this.mApp.mOffset160X), ZumasRevenge.Common._DS(tunnel.mY + GameApp.gScreenShakeY));
				}
			}
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0004AFF8 File Offset: 0x000491F8
		public bool DoingMainDarkFrogSequence()
		{
			return this.mDarkFrogSequence != null && this.mGameState == GameState.GameState_Boss6DarkFrog && !this.mDarkFrogSequence.FadingIn() && this.mDarkFrogSequence.mInitialDelay >= this.mDarkFrogSequence.mInitialDelayTarget && !this.mDarkFrogSequence.Done() && !this.mDarkFrogSequence.FadingOut();
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0004B059 File Offset: 0x00049259
		public bool HasDarkFrogSequence()
		{
			return this.mDarkFrogSequence != null;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0004B068 File Offset: 0x00049268
		public void BallDeleted(Ball b)
		{
			for (int i = 0; i < this.mMultiplierBallEffects.size<MultiplierBallEffect>(); i++)
			{
				MultiplierBallEffect multiplierBallEffect = this.mMultiplierBallEffects[i];
				if (multiplierBallEffect.GetBall() == b)
				{
					multiplierBallEffect.BallDestroyed(b);
					return;
				}
			}
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0004B0A9 File Offset: 0x000492A9
		public void ForceFlipFrog()
		{
			this.mFrog.SetDestAngle(-3.14159f);
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0004B0BB File Offset: 0x000492BB
		public int GetDarkFrogLevelFadeInAlpha()
		{
			if (this.mDarkFrogSequence != null && this.mDarkFrogSequence.FadingToLevel())
			{
				return (int)this.mDarkFrogSequence.GetBGAlpha();
			}
			return -1;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0004B0E0 File Offset: 0x000492E0
		public bool LazerHitTreasure(SexyVector3 p1, SexyVector3 v1, ref float t)
		{
			float num = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(40));
			float num2 = p1.x - ((float)this.mCurTreasure.x + ZumasRevenge.Common._SS((float)this.mFruitImg.GetCelWidth() / ZumasRevenge.Common._M(2f)));
			float num3 = p1.y - ((float)this.mCurTreasure.y + ZumasRevenge.Common._SS((float)this.mFruitImg.GetCelHeight() / ZumasRevenge.Common._M(2f)) + this.mTreasureYBob);
			float num4 = v1.x * v1.x + v1.y * v1.y;
			float num5 = 2f * (v1.x * num2 + v1.y * num3);
			float num6 = num2 * num2 + num3 * num3 - num * num;
			float num7 = num5 * num5 - 4f * num4 * num6;
			if (num7 < 0f)
			{
				return false;
			}
			num7 = (float)Math.Sqrt((double)num7);
			float num8 = (-num5 - num7) / (2f * num4);
			if (num8 > 0f && num8 < t)
			{
				t = num8;
				return true;
			}
			return false;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0004B1FC File Offset: 0x000493FC
		public bool LazerHitTorch(SexyVector3 p1, SexyVector3 v1, ref float t, float ballRadius)
		{
			if (this.mLevel == null)
			{
				return false;
			}
			bool result = false;
			for (int i = 0; i < this.mLevel.mTorches.size<Torch>(); i++)
			{
				if (!this.mLevel.mTorches[i].mWasHit)
				{
					float num = (float)((this.mLevel.mTorches[i].mWidth > this.mLevel.mTorches[i].mHeight) ? this.mLevel.mTorches[i].mWidth : this.mLevel.mTorches[i].mHeight);
					num /= 2f;
					num += ballRadius;
					ZumasRevenge.Common._SS((float)this.mLevel.mTorches[i].mWidth / ZumasRevenge.Common._M(2f));
					ZumasRevenge.Common._SS((float)this.mLevel.mTorches[i].mHeight / ZumasRevenge.Common._M(2f));
					float num2 = p1.x - ((float)this.mLevel.mTorches[i].mX + ZumasRevenge.Common._SS((float)this.mLevel.mTorches[i].mWidth / ZumasRevenge.Common._M(2f)));
					float num3 = p1.y - ((float)this.mLevel.mTorches[i].mY + ZumasRevenge.Common._SS((float)this.mLevel.mTorches[i].mHeight / ZumasRevenge.Common._M(2f)));
					float num4 = v1.x * v1.x + v1.y * v1.y;
					float num5 = 2f * (v1.x * num2 + v1.y * num3);
					float num6 = num2 * num2 + num3 * num3 - num * num;
					float num7 = num5 * num5 - 4f * num4 * num6;
					if (num7 >= 0f)
					{
						num7 = (float)Math.Sqrt((double)num7);
						float num8 = (-num5 - num7) / (2f * num4);
						if (num8 > 0f && num8 < t)
						{
							t = num8;
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0004B438 File Offset: 0x00049638
		public int GetCheckpointScore()
		{
			if (this.mLevel.mNum > 5)
			{
				return this.mApp.mUserProfile.GetAdvModeVars().mCheckpointScores[this.mLevel.mZone - 1].mMidpoint;
			}
			return this.mApp.mUserProfile.GetAdvModeVars().mCheckpointScores[this.mLevel.mZone - 1].mZoneStart;
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0004B4A4 File Offset: 0x000496A4
		public Level GetCheckpointLevel()
		{
			int num = this.mApp.GetLevelMgr().GetLevelIndex(this.mLevel.mId);
			if (this.mLevel.mNum <= 5)
			{
				num -= this.mLevel.mNum - 1;
			}
			else
			{
				num -= this.mLevel.mNum - 6;
			}
			return this.mApp.GetLevelMgr().GetLevelByIndex(num);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0004B510 File Offset: 0x00049710
		public int GetLevel()
		{
			return this.mLevelNum;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0004B518 File Offset: 0x00049718
		public int GetNumClearsInARow()
		{
			return this.mNumClearsInARow;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0004B520 File Offset: 0x00049720
		public int GetCurInARowBonus()
		{
			return this.mCurInARowBonus;
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0004B528 File Offset: 0x00049728
		public int GetCurComboScore()
		{
			return this.mCurComboScore;
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0004B530 File Offset: 0x00049730
		public int GetNumCleared()
		{
			return this.mNumCleared;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0004B538 File Offset: 0x00049738
		public int GetCurComboCount()
		{
			return this.mCurComboCount;
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0004B540 File Offset: 0x00049740
		public int GetStateCount()
		{
			return this.mStateCount;
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0004B548 File Offset: 0x00049748
		public int GetTickCount()
		{
			return this.GetStateCount() * 10;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0004B553 File Offset: 0x00049753
		public int GetLevelScore()
		{
			return this.mScore - this.mLevelBeginScore;
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0004B562 File Offset: 0x00049762
		public int GetLevelBeginScore()
		{
			return this.mLevelBeginScore;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0004B56A File Offset: 0x0004976A
		public int GetHallucinateTimer()
		{
			return this.mHallucinateTimer;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0004B572 File Offset: 0x00049772
		public int GetTreasureGlowAlpha()
		{
			return this.mTreasureGlowAlpha;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0004B57A File Offset: 0x0004977A
		public int GetCurRollerScore()
		{
			return this.mRollerScore.GetCurrentScore();
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0004B587 File Offset: 0x00049787
		public int GetNumLives()
		{
			return this.mLives;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0004B58F File Offset: 0x0004978F
		public bool IsEndless()
		{
			return this.mIsEndless;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0004B597 File Offset: 0x00049797
		public bool IsGameOver()
		{
			return this.mGameState == GameState.GameState_Losing || this.mIsWinning;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0004B5AA File Offset: 0x000497AA
		public bool IsPaused()
		{
			return this.mPauseCount != 0;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0004B5B8 File Offset: 0x000497B8
		public bool HasBackgroundArt()
		{
			return this.mBackgroundImage != null;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0004B5C6 File Offset: 0x000497C6
		public bool HasGuideBall()
		{
			return this.mGuideBall != null;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0004B5D4 File Offset: 0x000497D4
		public bool DestroyAll()
		{
			return this.mDestroyAll;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0004B5DC File Offset: 0x000497DC
		public bool HasAchievedZuma()
		{
			return this.mScore >= this.mScoreTarget && !this.GauntletMode();
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0004B5F7 File Offset: 0x000497F7
		public bool GauntletMode()
		{
			return this.mGauntletMode;
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0004B5FF File Offset: 0x000497FF
		public bool IronFrogMode()
		{
			return this.mLevel.mIronFrog && !this.GauntletMode() && !this.mAdventureMode;
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0004B621 File Offset: 0x00049821
		public bool DoingIntros()
		{
			return this.mShowMapScreen;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0004B629 File Offset: 0x00049829
		public bool DoingLevelTransition()
		{
			return this.mLevelTransition != null;
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0004B638 File Offset: 0x00049838
		public bool CanDrawFrog()
		{
			return (!this.DoingLevelTransition() || this.mDoingTransition || (this.IronFrogMode() && this.mLevelTransition.GetState() == 2)) && this.mGameState != GameState.GameState_Boss6FakeCredits && (this.mGameState != GameState.GameState_Boss6DarkFrog || (this.mDarkFrogSequence != null && (this.mDarkFrogSequence.FadingToLevel() || this.mDarkFrogSequence.mInitialDelay < this.mDarkFrogSequence.mInitialDelayTarget)));
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0004B6B3 File Offset: 0x000498B3
		public bool HasTunnels(int pri)
		{
			return Enumerable.Count<Tunnel>(this.mTunnels[pri]) > 0;
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0004B6C5 File Offset: 0x000498C5
		public bool DisplayingTip()
		{
			return Enumerable.Count<ZumaTip>(this.mZumaTips) > 0;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0004B6D5 File Offset: 0x000498D5
		public bool WasShowingCheckpoint()
		{
			return this.mWasShowingCheckpoint;
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0004B6DD File Offset: 0x000498DD
		public bool IsLoading()
		{
			return this.mIsLoading;
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0004B6E8 File Offset: 0x000498E8
		public bool IsAboutToDoCheckpointEffect()
		{
			return (this.mFrog.HasSmokeParticles() && !this.mHasSeenCheckpointIntro && this.ShouldShowCheckpointPostcard() && !this.GauntletMode() && !this.IronFrogMode() && this.mLevel.mNum > 1 && this.mCheckpointEffect == null) || this.mCheckpointEffect != null;
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0004B748 File Offset: 0x00049948
		public bool LevelIsSkeletonBoss()
		{
			return this.mLevel != null && this.mLevel.mBoss != null && this.mLevel.mZone == 3 && this.mLevel.mBoss.GetType() == typeof(BossSkeleton);
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0004B797 File Offset: 0x00049997
		public bool IsHardAdventureMode()
		{
			return this.mApp.IsHardMode();
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0004B7A4 File Offset: 0x000499A4
		public GameStats GetLevelStats()
		{
			return this.mLevelStats;
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0004B7AC File Offset: 0x000499AC
		public GameStats GetGameStats()
		{
			return this.mGameStats;
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0004B7B4 File Offset: 0x000499B4
		public Gun GetGun()
		{
			return this.mFrog;
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0004B7BC File Offset: 0x000499BC
		public bool DoingBossIntro()
		{
			return this.mGameState == GameState.GameState_BossIntro;
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0004B7C8 File Offset: 0x000499C8
		public Image GetFruitImage()
		{
			return this.mFruitImg;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0004B7D0 File Offset: 0x000499D0
		public Image GetFruitGlow()
		{
			return this.mFruitGlow;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0004B7D8 File Offset: 0x000499D8
		public bool CanDeleteEffectResources()
		{
			return this.mCanDeleteEffectResources;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0004B7E0 File Offset: 0x000499E0
		public GameState GetGameState()
		{
			return this.mGameState;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0004B7E8 File Offset: 0x000499E8
		public AdvModeTempleStats GetAdvModeTempleStats()
		{
			if (!this.mAdventureMode)
			{
				return null;
			}
			if (!this.IsHardAdventureMode())
			{
				return this.mApp.mUserProfile.mAdventureStats;
			}
			return this.mApp.mUserProfile.mHeroicStats;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0004B81D File Offset: 0x00049A1D
		public void SetMenuBtnEnabled(bool enabled)
		{
			this.mMenuButton.mDisabled = !enabled;
			this.mMenuButton.mVisible = enabled;
			if (this.mSwapBallButton != null)
			{
				this.mSwapBallButton.mDisabled = !enabled;
				this.mSwapBallButton.mVisible = enabled;
			}
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0004B85D File Offset: 0x00049A5D
		public void AddFiredBullet(Bullet b)
		{
			this.mBulletList.Add(b);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0004B86B File Offset: 0x00049A6B
		public bool HasFiredBullets()
		{
			return Enumerable.Count<Bullet>(this.mBulletList) > 0;
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x0004B87B File Offset: 0x00049A7B
		public void SetNumClearsInARow(int val)
		{
			this.mNumClearsInARow = val;
			if (this.mNumClearsInARow > 1)
			{
				this.mLevel.MadeConsecutiveClear(this.mNumClearsInARow);
			}
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x0004B89E File Offset: 0x00049A9E
		public void SetCurInARowBonus(int val)
		{
			this.mCurInARowBonus = val;
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0004B8A7 File Offset: 0x00049AA7
		public void SetCurComboScore(int val)
		{
			this.mCurComboScore = val;
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0004B8B0 File Offset: 0x00049AB0
		public void IncNumClearsInARow(int val)
		{
			this.mNumClearsInARow += val;
			if (this.mNumClearsInARow >= 20)
			{
				this.mApp.SetAchievement("chain_20x");
			}
			if (this.mNumClearsInARow >= 15)
			{
				this.UnlockAchievement(EAchievementType.C_C_C_C_CHAIN_BONUS);
			}
			if (this.mNumClearsInARow > 1)
			{
				this.mLevel.MadeConsecutiveClear(this.mNumClearsInARow);
			}
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0004B910 File Offset: 0x00049B10
		public void IncCurInARowBonus(int val)
		{
			this.mCurInARowBonus += val;
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0004B920 File Offset: 0x00049B20
		public void IncCurComboScore(int val)
		{
			this.mCurComboScore += val;
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0004B930 File Offset: 0x00049B30
		public void SetNumCleared(int val)
		{
			this.mNumCleared = val;
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0004B939 File Offset: 0x00049B39
		public void IncNumCleared(int val)
		{
			this.mNumCleared += val;
			this.mLevel.IncNumBallsExploded(val);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0004B955 File Offset: 0x00049B55
		public void SetCurComboCount(int val)
		{
			this.mCurComboCount = val;
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0004B95E File Offset: 0x00049B5E
		public void AddPowerEffect(PowerEffect p)
		{
			this.mPowerEffects.Add(p);
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0004B96C File Offset: 0x00049B6C
		public void SetRollingInDangerZone()
		{
			this.mRollingInDangerZone = true;
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0004B978 File Offset: 0x00049B78
		public void SetHallucinateTimer(int t)
		{
			if (t == 0)
			{
				this.mHallucinateTimer = 0;
				return;
			}
			if (this.mHallucinateTimer > 0)
			{
				return;
			}
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_STROKE);
			this.mLevel.mFrog.mTempText = new BonusText(TextManager.getInstance().getString(691), fontByID, (float)ZumasRevenge.Common._S(this.mLevel.mFrog.GetCenterX() - ZumasRevenge.Common._M(30)), (float)ZumasRevenge.Common._S(this.mLevel.mFrog.GetCenterY() - ZumasRevenge.Common._M1(70)), (float)ZumasRevenge.Common._M2(0), ZumasRevenge.Common._M3(0));
			this.mLevel.mFrog.mTempText.SetAlphaDecRate(0f);
			this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_MINDWARP1));
			this.mHallucinateTimer = t;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0004BA46 File Offset: 0x00049C46
		public void AddProxBombExplosion(float x, float y)
		{
			this.mApp.mProxBombManager.AddBomb(x, y);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0004BA5A File Offset: 0x00049C5A
		public void ToggleNotification(string theNotification, int inSoundID)
		{
			this.m_NotificationQuene.Add(new KeyValuePair<string, int>(theNotification, inSoundID));
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0004BA6E File Offset: 0x00049C6E
		public void ToggleNotification(string theNotification)
		{
			this.ToggleNotification(theNotification, -1);
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0004BA78 File Offset: 0x00049C78
		public void DrawFatFingerGuide(Graphics g)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_GUIDE);
			imageByID.GetWidth();
			ZumasRevenge.Common._S(2);
			if (this.mFrog != null && (this.mFrog.LaserMode() || this.mFrog.mLightningEffect != null || this.mFrog.IsCannon()))
			{
				return;
			}
			if (this.mInvalidateTouchUp || this.ShouldBlockInput())
			{
				return;
			}
			if (this.mFrog.IsStunned() || this.mGameState != GameState.GameState_Playing || this.mFrog.LightningMode())
			{
				return;
			}
			if (Enumerable.Count<ZumaTip>(this.mZumaTips) > 0 && this.mZumaTips[0].mClickDismiss && this.mZumaTips[0].mId != ZumaProfile.FIRST_SHOT_HINT)
			{
				return;
			}
			if (this.mPauseCount != 0)
			{
				return;
			}
			if (this.mLevel.mBoss != null && !this.mLevel.mBoss.AllowFrogToFire())
			{
				return;
			}
			if (this.mFatFingerGuideEnabled || this.mFatFingerGuideAlpha > 0)
			{
				if (this.mFatFingerGuideEnabled && this.mFatFingerGuideAlpha < 255)
				{
					this.mFatFingerGuideAlpha += 15;
					if (this.mFatFingerGuideAlpha >= 255)
					{
						this.mFatFingerGuideAlpha = 255;
					}
				}
				else if (!this.mFatFingerGuideEnabled)
				{
					this.mFatFingerGuideAlpha -= 25;
					if (this.mFatFingerGuideAlpha <= 0)
					{
						this.mFatFingerGuideAlpha = 0;
					}
				}
				if (this.mFrog.GetBullet() == null)
				{
					return;
				}
				int colorType = this.mFrog.GetBullet().GetColorType();
				int theColor = ZumasRevenge.Common.gBallColors[colorType];
				if (this.mApp.mColorblind && colorType == 3)
				{
					theColor = 8421504;
				}
				else if (this.mApp.mColorblind && colorType == 4)
				{
					theColor = 1973790;
				}
				g.SetColorizeImages(true);
				g.SetDrawMode(1);
				g.SetColor(new SexyColor(theColor, this.mFatFingerGuideAlpha));
				g.DrawImageRotatedF(this.mAimGuide, ZumasRevenge.Common._S(this.mFrog.mCurX), ZumasRevenge.Common._S(this.mFrog.mCurY) - (float)(this.mAimGuide.mHeight / 2), (double)this.mFrog.GetAngle() - 1.5707963267948966, 0f, (float)(this.mAimGuide.mHeight / 2));
				g.SetColorizeImages(false);
				g.SetDrawMode(0);
			}
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0004BCC8 File Offset: 0x00049EC8
		public bool IsTouchOnFrogGun(int x, int y)
		{
			bool result = false;
			float num = (float)this.mFrog.mWidth / 2f;
			num *= num;
			float num2 = ZumasRevenge.Common._S(this.mFrog.mCurX) + (float)this.mApp.mBoardOffsetX;
			float num3 = ZumasRevenge.Common._S(this.mFrog.mCurY);
			float num4 = (float)x - num2;
			num4 *= num4;
			float num5 = (float)y - num3;
			num5 *= num5;
			if (num4 + num5 < num)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0004BD44 File Offset: 0x00049F44
		public bool CycleBallAt(int x, int y)
		{
			bool result = false;
			if (this.mLevel != null)
			{
				Ball ballAtXY = this.mLevel.GetBallAtXY(ZumasRevenge.Common._SS(x), ZumasRevenge.Common._SS(y));
				if (ballAtXY != null)
				{
					if (this.mBallPowerupCheat != -1)
					{
						if (this.mBallPowerupCheat == -2)
						{
							ballAtXY.GetCurve().RemoveBall(ballAtXY);
						}
						else
						{
							ballAtXY.SetPowerType((PowerType)this.mBallPowerupCheat, false);
						}
						this.mBallPowerupCheat = -1;
					}
					else
					{
						int num = ballAtXY.GetColorType();
						if (++num >= 6)
						{
							num = 0;
						}
						ballAtXY.SetColorType(num);
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0004BDCC File Offset: 0x00049FCC
		public bool CycleFrogPowerupAt(int x, int y)
		{
			bool result = false;
			if (this.mFrog != null && this.IsTouchOnFrogGun(x, y))
			{
				int num = this.mFrog.GetType();
				if (++num >= 5)
				{
					num = 0;
				}
				int p = 0;
				this.mFrog.ResetFrogType();
				switch (num)
				{
				case 1:
					p = 7;
					break;
				case 2:
					p = 9;
					break;
				case 3:
					p = 8;
					break;
				}
				this.ActivatePower((PowerType)p);
				result = true;
			}
			return result;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0004BE44 File Offset: 0x0004A044
		public void SetBallPowerupCheat(char c)
		{
			if (c == '*')
			{
				if (this.GauntletMode())
				{
					this.mBallPowerupCheat = 13;
					return;
				}
			}
			else
			{
				if (c == '#')
				{
					this.mBallPowerupCheat = 0;
					return;
				}
				if (c == '%')
				{
					this.mBallPowerupCheat = 1;
					return;
				}
				if (c == '@')
				{
					this.mBallPowerupCheat = 9;
					return;
				}
				if (c == '<')
				{
					this.mBallPowerupCheat = 3;
					return;
				}
				if (c == '|')
				{
					this.mBallPowerupCheat = 7;
					return;
				}
				if (c == '~')
				{
					this.mBallPowerupCheat = 8;
					return;
				}
				if (c == 'D')
				{
					this.mBallPowerupCheat = -2;
					return;
				}
				this.mBallPowerupCheat = -1;
			}
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0004BECB File Offset: 0x0004A0CB
		public void DisableBallPowerupCheat()
		{
			this.mBallPowerupCheat = -1;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0004BED4 File Offset: 0x0004A0D4
		public void SwapFrogBalls()
		{
			if (this.mLevel.CanSwapBalls() && !this.mFrog.LaserMode())
			{
				Bullet bullet = this.mFrog.GetBullet();
				int num = -1;
				int num2 = -1;
				if (bullet != null)
				{
					num = bullet.GetColorType();
				}
				this.mFrog.SwapBullets();
				bullet = this.mFrog.GetBullet();
				if (bullet != null)
				{
					num2 = bullet.GetColorType();
				}
				if (num != num2)
				{
					this.mApp.mUserProfile.mBallsSwapped++;
				}
				this.SwapBallButtonImage(num);
			}
			if (this.mFrog.LaserMode())
			{
				this.mFrog.ClearLaserState();
				this.GetBetaStats().CanceledLaser();
				this.mShowGuide = false;
				if (this.mGuideBall != null)
				{
					this.mGuideBall.mHilightPulse = false;
					this.mGuideBall.DoLaserAnim(false);
				}
			}
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0004BFA3 File Offset: 0x0004A1A3
		public void SwapBallButtonImage(int theBallColor)
		{
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0004BFA5 File Offset: 0x0004A1A5
		public void LivesChanged(int theLivesDelta)
		{
			if (this.mLivesInfo != null)
			{
				this.mLivesInfo = null;
			}
			this.mLives += theLivesDelta;
			this.mLivesInfo = new LivesInfo(this, theLivesDelta);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0004BFD4 File Offset: 0x0004A1D4
		public bool IsPointAlongSlider(int x, int y)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_FROG_NORMAL_BODY);
			bool result = false;
			int num = (int)ZumasRevenge.Common._S(this.mFrog.mCurX);
			int num2 = (int)ZumasRevenge.Common._S(this.mFrog.mCurY);
			if (this.mLevel.mMoveType == 1)
			{
				int num3 = num2 - imageByID.GetHeight() / 2;
				int num4 = num2 + imageByID.GetHeight() / 2;
				if (y >= num3 && y <= num4)
				{
					result = true;
				}
			}
			else if (this.mLevel.mMoveType == 2)
			{
				int num5 = num - imageByID.GetHeight() / 2;
				int num6 = num + imageByID.GetHeight() / 2;
				if (x >= num5 && x <= num6)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0004C07C File Offset: 0x0004A27C
		public void DrawHaloSwap(Graphics g)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_SWAP_HALO);
			if (this.mGameState != GameState.GameState_Playing || this.mFrog.IsStunned() || (this.GauntletMode() && this.mGauntletModeOver) || this.mFrog.IsCannon() || this.mFrog.LaserMode() || this.mFrog.LightningMode())
			{
				return;
			}
			if (!this.mFrog.IsFrogShowingBall())
			{
				return;
			}
			if (this.mDrawHaloSwap || this.mFinishHaloSwap)
			{
				int num = (int)ZumasRevenge.Common._S(this.mFrog.mCenterX);
				int num2 = (int)ZumasRevenge.Common._S(this.mFrog.mCenterY);
				if (this.mFrog != null && this.mFrog.GetNextBullet() != null)
				{
					float num3 = (float)this.mHaloSwapCurve.GetOutVal();
					g.SetColorizeImages(true);
					g.SetColor(this.mHaloSwapColor.mRed, this.mHaloSwapColor.mGreen, this.mHaloSwapColor.mBlue, (int)(num3 * 255f));
					g.SetDrawMode(1);
					float num4 = num3 * (float)imageByID.GetHeight();
					float num5 = num3 * (float)imageByID.GetHeight();
					float num6 = (float)num - num4 / 2f;
					float num7 = (float)num2 - num5 / 2f;
					g.DrawImage(imageByID, (int)num6, (int)num7, (int)num4, (int)num5);
					g.SetColorizeImages(false);
					g.SetDrawMode(0);
				}
			}
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0004C1E0 File Offset: 0x0004A3E0
		public void EnableHaloSwap(bool forceReset)
		{
			if (this.mFrog == null || this.mFrog.GetNextBullet() == null || this.mGameState != GameState.GameState_Playing || (this.GauntletMode() && this.mGauntletModeOver) || this.mFrog.IsStunned())
			{
				return;
			}
			this.mDrawHaloSwap = true;
			int colorType = this.mFrog.GetNextBullet().GetColorType();
			int theColor = ZumasRevenge.Common.gBallColors[colorType];
			if (this.mApp.mColorblind && colorType == 3)
			{
				theColor = 8421504;
			}
			else if (this.mApp.mColorblind && colorType == 4)
			{
				theColor = 1973790;
			}
			this.mHaloSwapColor = new SexyColor(theColor);
			bool flag = this.mHaloSwapCurve.IsDoingCurve();
			bool flag2 = this.mHaloSwapCurve.HasBeenTriggered();
			if (forceReset || (!flag && !flag2))
			{
				this.mHaloSwapCurve.SetCurve(ZumasRevenge.Common._MP("b#0,1,0.01,1,# ##  *~      w~"));
			}
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0004C2BB File Offset: 0x0004A4BB
		public void DisableHaloSwap(bool finishAnim)
		{
			this.mFinishHaloSwap = finishAnim;
			this.mDrawHaloSwap = false;
			if (!finishAnim)
			{
				this.mHaloSwapCurve.SetConstant(0.0);
				this.mHaloSwapCurve.ClearTrigger();
			}
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0004C2F0 File Offset: 0x0004A4F0
		public void UpdateHaloSwap()
		{
			if (this.mFinishHaloSwap)
			{
				if (!this.mHaloSwapCurve.IncInVal())
				{
					this.mHaloSwapCurve.SetConstant(0.0);
					this.mHaloSwapCurve.ClearTrigger();
					this.mFinishHaloSwap = false;
					return;
				}
			}
			else if (this.mDrawHaloSwap)
			{
				this.mHaloSwapCurve.IncInVal();
			}
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0004C350 File Offset: 0x0004A550
		public string GetLevelDisplayName()
		{
			string result;
			if (this.mLevel.mBoss != null || this.mLevel.IsFinalBossLevel())
			{
				result = this.mLevel.mDisplayName;
			}
			else if (this.mLevel.mZone - 1 > 0)
			{
				if (this.mLevel.mNum >= 10)
				{
					result = string.Format("{0} - {1}", this.mLevel.mZone * this.mLevel.mNum, this.mLevel.mDisplayName);
				}
				else
				{
					result = string.Format("{0}{1} - {2}", this.mLevel.mZone - 1, this.mLevel.mNum, this.mLevel.mDisplayName);
				}
			}
			else
			{
				result = string.Format("{0} - {1}", this.mLevel.mNum, this.mLevel.mDisplayName);
			}
			return result;
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0004C43C File Offset: 0x0004A63C
		public string GetMetricsLevelName()
		{
			string result;
			if (this.mLevel.mBoss != null || this.mLevel.IsFinalBossLevel())
			{
				result = this.mLevel.mDisplayName;
			}
			else if (this.mLevel.mZone - 1 > 0)
			{
				if (this.mLevel.mNum >= 10)
				{
					result = string.Format("{0}", this.mLevel.mZone * this.mLevel.mNum);
				}
				else
				{
					result = string.Format("{0}{1}", this.mLevel.mZone - 1, this.mLevel.mNum);
				}
			}
			else
			{
				result = string.Format("{0}", this.mLevel.mNum);
			}
			return result;
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0004C507 File Offset: 0x0004A707
		public void SetBoardOffset(Graphics g, bool enable)
		{
			if (enable)
			{
				g.mTransX = (float)this.mApp.mBoardOffsetX;
				return;
			}
			g.mTransX = 0f;
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0004C52C File Offset: 0x0004A72C
		public void ProcessTrialYesNo(int theId)
		{
			if (theId == 1000)
			{
				GameApp.gApp.ToMarketPlace();
				GameApp.gApp.mBoard.Pause(false, true);
				this.mIsTryAndBuyDialogShowing = false;
				return;
			}
			if (theId == 1001)
			{
				GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.DoDeferredEndGame);
				GameApp.gApp.ToggleBambooTransition();
				GameApp.gApp.mMusic.StopAll();
				this.mIsTryAndBuyDialogShowing = false;
			}
		}

		// Token: 0x040004B6 RID: 1206
		protected static bool gNeedsVortexSound = true;

		// Token: 0x040004B7 RID: 1207
		protected static float UpdateVortex_last_angle = 0f;

		// Token: 0x040004B8 RID: 1208
		protected static int END_BOSS_FROG_JUMP_TIME = 50;

		// Token: 0x040004B9 RID: 1209
		public static int gNumColors = 3;

		// Token: 0x040004BA RID: 1210
		public static bool gUseGlobalColorNum = false;

		// Token: 0x040004BB RID: 1211
		public static bool gUseChangeColor = false;

		// Token: 0x040004BC RID: 1212
		public static int gTuneNum = 0;

		// Token: 0x040004BD RID: 1213
		public static bool gShowText = true;

		// Token: 0x040004BE RID: 1214
		public static bool gHideBalls = false;

		// Token: 0x040004BF RID: 1215
		public static bool gUpdateBalls = true;

		// Token: 0x040004C0 RID: 1216
		public static bool gMouseCheatMode = false;

		// Token: 0x040004C1 RID: 1217
		public static bool gForceTreasure = false;

		// Token: 0x040004C2 RID: 1218
		public static bool gPauseOnLostFocus = true;

		// Token: 0x040004C3 RID: 1219
		public static int gIntroRibbitTimer = 0;

		// Token: 0x040004C4 RID: 1220
		public static int FROG_DEATH_X = 396;

		// Token: 0x040004C5 RID: 1221
		public static int FROG_DEATH_Y = 460;

		// Token: 0x040004C6 RID: 1222
		public static readonly int TREASURE_LIFE = 1000;

		// Token: 0x040004C7 RID: 1223
		public static CurveData gDebugCurveData = null;

		// Token: 0x040004C8 RID: 1224
		public static string gDebugCurveFile;

		// Token: 0x040004C9 RID: 1225
		public static bool gNewStyleBallChooser = true;

		// Token: 0x040004CA RID: 1226
		public static bool gShowStats = true;

		// Token: 0x040004CB RID: 1227
		private static bool gNeedsGauntletHSSound = true;

		// Token: 0x040004CC RID: 1228
		private static bool gCheatReload = false;

		// Token: 0x040004CD RID: 1229
		private static int gEndGauntletExtraTime = 0;

		// Token: 0x040004CE RID: 1230
		private static int gMultTimeLeftDecAmt = 0;

		// Token: 0x040004CF RID: 1231
		private static int delay_after_skull = 0;

		// Token: 0x040004D0 RID: 1232
		private static int gLastUnderwaterSound = 0;

		// Token: 0x040004D1 RID: 1233
		private static BallDrawer drawer = null;

		// Token: 0x040004D2 RID: 1234
		private static int end_delay = 0;

		// Token: 0x040004D3 RID: 1235
		private static int END_GAUNTLET_TIME = 350;

		// Token: 0x040004D4 RID: 1236
		private static bool gNeedBossIntroSound = true;

		// Token: 0x040004D5 RID: 1237
		private static int gTextHandle = 0;

		// Token: 0x040004D6 RID: 1238
		private static float SLIDER_FROG_DEVICE_SPEEDUP = 3f;

		// Token: 0x040004D7 RID: 1239
		private static int gEssenceDrawFrame = 55;

		// Token: 0x040004D8 RID: 1240
		public static float MAX_STONE_HEAD_STRETCH = 1.001f;

		// Token: 0x040004D9 RID: 1241
		protected Board.BoardState mBoardState;

		// Token: 0x040004DA RID: 1242
		protected Board.GamepadControls mGamepadControls = new Board.GamepadControls();

		// Token: 0x040004DB RID: 1243
		public GameApp mApp;

		// Token: 0x040004DC RID: 1244
		public List<Ball> mNeedComboCount = new List<Ball>();

		// Token: 0x040004DD RID: 1245
		public Level mLevel;

		// Token: 0x040004DE RID: 1246
		public Level mNextLevel;

		// Token: 0x040004DF RID: 1247
		public List<ZumaTip> mZumaTips = new List<ZumaTip>();

		// Token: 0x040004E0 RID: 1248
		public TreasurePoint mCurTreasure;

		// Token: 0x040004E1 RID: 1249
		public char[] mMuMuMode = new char[4];

		// Token: 0x040004E2 RID: 1250
		public bool mDoMuMuMode;

		// Token: 0x040004E3 RID: 1251
		public PIEffect[] mLazerBeam = new PIEffect[2];

		// Token: 0x040004E4 RID: 1252
		public PIEffect mLazerBurn;

		// Token: 0x040004E5 RID: 1253
		public Transform mGlobalTranform = new Transform();

		// Token: 0x040004E6 RID: 1254
		public ObjectPool<BallExplosion> mBallExplosionsPool = new ObjectPool<BallExplosion>(20);

		// Token: 0x040004E7 RID: 1255
		public ObjectPool<EndLevelExplosion> mEndLevelExplosionPool = new ObjectPool<EndLevelExplosion>(20);

		// Token: 0x040004E8 RID: 1256
		public bool mContinueNextLevelOnLoadProfile;

		// Token: 0x040004E9 RID: 1257
		public int mNextLevelOverrideOnLoadProfile;

		// Token: 0x040004EA RID: 1258
		public int mGauntletPointsForDiffInc;

		// Token: 0x040004EB RID: 1259
		public int mGauntletPointsFromMult;

		// Token: 0x040004EC RID: 1260
		public int mFullScreenAlpha;

		// Token: 0x040004ED RID: 1261
		public int mFullScreenAlphaRate;

		// Token: 0x040004EE RID: 1262
		public int[] mBallColorMap = new int[6];

		// Token: 0x040004EF RID: 1263
		public int[] mNewBallDelay = new int[2];

		// Token: 0x040004F0 RID: 1264
		public int mScore;

		// Token: 0x040004F1 RID: 1265
		public int mScoreTarget;

		// Token: 0x040004F2 RID: 1266
		public int mFlashAlpha;

		// Token: 0x040004F3 RID: 1267
		public int mLevelEndFrame;

		// Token: 0x040004F4 RID: 1268
		public int mMouseOverGunPos;

		// Token: 0x040004F5 RID: 1269
		public int mCurTreasureNum;

		// Token: 0x040004F6 RID: 1270
		public int mZoneTipIdx;

		// Token: 0x040004F7 RID: 1271
		public int mScoreTipIdx;

		// Token: 0x040004F8 RID: 1272
		public int mGauntletHSTarget;

		// Token: 0x040004F9 RID: 1273
		public int mScoreMultiplier;

		// Token: 0x040004FA RID: 1274
		public int mFruitMultiplier;

		// Token: 0x040004FB RID: 1275
		public float mMinTreasureY;

		// Token: 0x040004FC RID: 1276
		public float mMaxTreasureY;

		// Token: 0x040004FD RID: 1277
		public float mGuideT;

		// Token: 0x040004FE RID: 1278
		private SexyVector3 mGuideWallPoint = default(SexyVector3);

		// Token: 0x040004FF RID: 1279
		public int mTheNextLevel;

		// Token: 0x04000500 RID: 1280
		public ChallengeHelp mChallengeHelp;

		// Token: 0x04000501 RID: 1281
		public float mEndBossFadeAmt;

		// Token: 0x04000502 RID: 1282
		public bool mAdventureWinScreen;

		// Token: 0x04000503 RID: 1283
		public bool mAdventureMode;

		// Token: 0x04000504 RID: 1284
		public bool mIsHardMode;

		// Token: 0x04000505 RID: 1285
		public bool mTreasureWasHit;

		// Token: 0x04000506 RID: 1286
		public bool mIsWinning;

		// Token: 0x04000507 RID: 1287
		public bool mSkipToNextLevelOnNextUpdate;

		// Token: 0x04000508 RID: 1288
		public bool mPreventBallAdvancement;

		// Token: 0x04000509 RID: 1289
		public bool mRollingInDangerZone;

		// Token: 0x0400050A RID: 1290
		public int mNumZumaBalls;

		// Token: 0x0400050B RID: 1291
		public int mNumPauseUpdatesToDo;

		// Token: 0x0400050C RID: 1292
		public int mNumDrawFramesLeft;

		// Token: 0x0400050D RID: 1293
		public bool mReturnToMainMenu;

		// Token: 0x0400050E RID: 1294
		public bool mDoingFirstTimeIntro;

		// Token: 0x0400050F RID: 1295
		public bool mDoingFirstTimeIntroZoomToGame;

		// Token: 0x04000510 RID: 1296
		public int mEndLevelAceTimeBonus;

		// Token: 0x04000511 RID: 1297
		public int mEndLevelNum;

		// Token: 0x04000512 RID: 1298
		public int mEndLevelParTime;

		// Token: 0x04000513 RID: 1299
		public GameStats mEndLevelStats = new GameStats();

		// Token: 0x04000514 RID: 1300
		public string mEndLevelDisplayName = "";

		// Token: 0x04000515 RID: 1301
		public int mPrevIFBestScore;

		// Token: 0x04000516 RID: 1302
		public bool mSkipShutdownSave;

		// Token: 0x04000517 RID: 1303
		public List<LTSmokeParticle> mSmokeParticles = new List<LTSmokeParticle>();

		// Token: 0x04000518 RID: 1304
		public PIEffect mSmokePoof;

		// Token: 0x04000519 RID: 1305
		public int mCloakBossIntroAlpha;

		// Token: 0x0400051A RID: 1306
		public Image mIntroBG;

		// Token: 0x0400051B RID: 1307
		public Image mIntroWater;

		// Token: 0x0400051C RID: 1308
		public List<SimpleFadeText> mIntroDialog = new List<SimpleFadeText>();

		// Token: 0x0400051D RID: 1309
		public float mIntroFadeAmt;

		// Token: 0x0400051E RID: 1310
		public int mCloakedBossFrame;

		// Token: 0x0400051F RID: 1311
		public CurvedVal mIntroMidAlpha = new CurvedVal();

		// Token: 0x04000520 RID: 1312
		public CurvedVal mIntroMidScale = new CurvedVal();

		// Token: 0x04000521 RID: 1313
		public CurvedVal mIntroMidTransX = new CurvedVal();

		// Token: 0x04000522 RID: 1314
		public CurvedVal mIntroMapAlpha = new CurvedVal();

		// Token: 0x04000523 RID: 1315
		public CurvedVal mIntroMapPinAlpha = new CurvedVal();

		// Token: 0x04000524 RID: 1316
		public CurvedVal mIntroMapScale = new CurvedVal();

		// Token: 0x04000525 RID: 1317
		public CurvedVal mIntroMapTransX = new CurvedVal();

		// Token: 0x04000526 RID: 1318
		public CurvedVal mIntroRotate = new CurvedVal();

		// Token: 0x04000527 RID: 1319
		public CurvedVal mIntroFrogScale = new CurvedVal();

		// Token: 0x04000528 RID: 1320
		public int mTimeToBeatAdvMode;

		// Token: 0x04000529 RID: 1321
		public int mBeatGameTotalScoreTally;

		// Token: 0x0400052A RID: 1322
		public int mBeatGameLives;

		// Token: 0x0400052B RID: 1323
		public int mBeatGameNormalScore;

		// Token: 0x0400052C RID: 1324
		public int mIntroTimer;

		// Token: 0x0400052D RID: 1325
		public bool mDoIntroFrogJump;

		// Token: 0x0400052E RID: 1326
		public bool mIsRestarting;

		// Token: 0x0400052F RID: 1327
		public ButtonWidget mAdvWinBtn;

		// Token: 0x04000530 RID: 1328
		public ButtonWidget mStatsContinueBtn;

		// Token: 0x04000531 RID: 1329
		public PIEffect mChallengeCupUnlockedFX;

		// Token: 0x04000532 RID: 1330
		public DeathSkull mDeathSkull;

		// Token: 0x04000533 RID: 1331
		public List<BallExplosion> mBallExplosions = new List<BallExplosion>();

		// Token: 0x04000534 RID: 1332
		public List<PIEffect> mLazerBlasts = new List<PIEffect>();

		// Token: 0x04000535 RID: 1333
		public PIEffectBatch mEffectBatch = new PIEffectBatch();

		// Token: 0x04000536 RID: 1334
		public Composition mBoss6StoneBurst;

		// Token: 0x04000537 RID: 1335
		public Composition mBoss6VolcanoMelt;

		// Token: 0x04000538 RID: 1336
		public Bouncy mFruitBounceEffect = new Bouncy();

		// Token: 0x04000539 RID: 1337
		public uint mLastBallClickTick;

		// Token: 0x0400053A RID: 1338
		public uint mLastSmallExplosionTick;

		// Token: 0x0400053B RID: 1339
		public uint mLastExplosionTick;

		// Token: 0x0400053C RID: 1340
		public Image mBackgroundImage;

		// Token: 0x0400053D RID: 1341
		public Gun mFrog;

		// Token: 0x0400053E RID: 1342
		public MemoryImage mCachedCurveImage;

		// Token: 0x0400053F RID: 1343
		public Checkpoint mCheckpointEffect;

		// Token: 0x04000540 RID: 1344
		public List<Bullet> mBulletList = new List<Bullet>();

		// Token: 0x04000541 RID: 1345
		public Ball mGuideBall;

		// Token: 0x04000542 RID: 1346
		public SexyVector3 mGuideBallPoint = default(SexyVector3);

		// Token: 0x04000543 RID: 1347
		public GameState mGameState;

		// Token: 0x04000544 RID: 1348
		public SexyVector3 mGuideCenter = default(SexyVector3);

		// Token: 0x04000545 RID: 1349
		public SexyVector3 mLazerGuideCenter = default(SexyVector3);

		// Token: 0x04000546 RID: 1350
		public SexyPoint[] mGuide = new SexyPoint[]
		{
			new SexyPoint(),
			new SexyPoint(),
			new SexyPoint(),
			new SexyPoint()
		};

		// Token: 0x04000547 RID: 1351
		public SexyPoint[] mLazerGuide = new SexyPoint[]
		{
			new SexyPoint(),
			new SexyPoint(),
			new SexyPoint(),
			new SexyPoint()
		};

		// Token: 0x04000548 RID: 1352
		public List<Tunnel>[] mTunnels = new List<Tunnel>[5];

		// Token: 0x04000549 RID: 1353
		public MemoryImage[] mCachedTunnelImages = new MemoryImage[5];

		// Token: 0x0400054A RID: 1354
		public List<SimpleFadeText> mSimpleFadeText = new List<SimpleFadeText>();

		// Token: 0x0400054B RID: 1355
		public List<BonusTextElement> mText = new List<BonusTextElement>();

		// Token: 0x0400054C RID: 1356
		public List<VortexFace> mVortexFaces = new List<VortexFace>();

		// Token: 0x0400054D RID: 1357
		public List<VortexBeam> mVortexBeams = new List<VortexBeam>();

		// Token: 0x0400054E RID: 1358
		public QRand mQRand;

		// Token: 0x0400054F RID: 1359
		public List<PowerEffect> mPowerEffects = new List<PowerEffect>();

		// Token: 0x04000550 RID: 1360
		public RollerScore mRollerScore;

		// Token: 0x04000551 RID: 1361
		public ButtonWidget mMenuButton;

		// Token: 0x04000552 RID: 1362
		public int mMenuButtonX;

		// Token: 0x04000553 RID: 1363
		public string mNextLevelIdOverride = "";

		// Token: 0x04000554 RID: 1364
		public CursorBloom[] mCursorBlooms = new CursorBloom[2];

		// Token: 0x04000555 RID: 1365
		public LevelTransition mLevelTransition;

		// Token: 0x04000556 RID: 1366
		public string mStatsString = "";

		// Token: 0x04000557 RID: 1367
		public MapScreen mMapScreen;

		// Token: 0x04000558 RID: 1368
		public FwooshImage[] mLevelNameText = new FwooshImage[2];

		// Token: 0x04000559 RID: 1369
		public FwooshImage mLevelCompleteText = new FwooshImage();

		// Token: 0x0400055A RID: 1370
		public FwooshImage mChallengeHeaderText = new FwooshImage();

		// Token: 0x0400055B RID: 1371
		public FwooshImage mChallengePtsText = new FwooshImage();

		// Token: 0x0400055C RID: 1372
		public float mChallengeTextAlpha;

		// Token: 0x0400055D RID: 1373
		public Image mFruitImg;

		// Token: 0x0400055E RID: 1374
		public Image mFruitGlow;

		// Token: 0x0400055F RID: 1375
		public FruitExplode mFruitExplodeEffect;

		// Token: 0x04000560 RID: 1376
		public List<EndLevelExplosion> mEndLevelExplosions = new List<EndLevelExplosion>();

		// Token: 0x04000561 RID: 1377
		public List<MultiplierBallEffect> mMultiplierBallEffects = new List<MultiplierBallEffect>();

		// Token: 0x04000562 RID: 1378
		public FrogFlyOff mFrogFlyOff;

		// Token: 0x04000563 RID: 1379
		public DeviceImage mTransitionScreen;

		// Token: 0x04000564 RID: 1380
		public DeviceImage mTransitionScreenImage;

		// Token: 0x04000565 RID: 1381
		public bool mDoingTransition;

		// Token: 0x04000566 RID: 1382
		public bool mPlayThud;

		// Token: 0x04000567 RID: 1383
		public CurvedVal mStatsBubbleScale = new CurvedVal();

		// Token: 0x04000568 RID: 1384
		public CurvedVal mTransitionScreenHolePct = new CurvedVal();

		// Token: 0x04000569 RID: 1385
		public CurvedVal mTransitionScreenScale = new CurvedVal();

		// Token: 0x0400056A RID: 1386
		public CurvedVal mTransitionFrogRotPct = new CurvedVal();

		// Token: 0x0400056B RID: 1387
		public CurvedVal mTransitionFrogScale = new CurvedVal();

		// Token: 0x0400056C RID: 1388
		public CurvedVal mTransitionFrogPosPct = new CurvedVal();

		// Token: 0x0400056D RID: 1389
		public SexyPoint mTransitionCenter = new SexyPoint();

		// Token: 0x0400056E RID: 1390
		public PIEffect mVolcanoBossEssence;

		// Token: 0x0400056F RID: 1391
		public PIEffect mEssenceExplBottom;

		// Token: 0x04000570 RID: 1392
		public PIEffect mEssenceExplTop;

		// Token: 0x04000571 RID: 1393
		public float mEssenceXScale;

		// Token: 0x04000572 RID: 1394
		public float mEssenceYScale;

		// Token: 0x04000573 RID: 1395
		public int mEssenceScaleTimer;

		// Token: 0x04000574 RID: 1396
		public int mEndBossFrogTimer;

		// Token: 0x04000575 RID: 1397
		public bool mDoingEndBossFrogEffect;

		// Token: 0x04000576 RID: 1398
		public PIEffect mBossSmokePoof;

		// Token: 0x04000577 RID: 1399
		public FakeCredits mFakeCredits;

		// Token: 0x04000578 RID: 1400
		public DarkFrogSequence mDarkFrogSequence;

		// Token: 0x04000579 RID: 1401
		public float mCurrentSatPct;

		// Token: 0x0400057A RID: 1402
		public float mIronFrogAlpha;

		// Token: 0x0400057B RID: 1403
		public ExtraSexyButton mIronFrogBtn;

		// Token: 0x0400057C RID: 1404
		public bool mDoingIronFrogWin;

		// Token: 0x0400057D RID: 1405
		public int mIronFrogWinDelay;

		// Token: 0x0400057E RID: 1406
		public bool mNeedsBossExtraLife;

		// Token: 0x0400057F RID: 1407
		public bool mDoingBossIntroText;

		// Token: 0x04000580 RID: 1408
		public float mBossIntroAlpha;

		// Token: 0x04000581 RID: 1409
		public float mBossIntroAlphaRate;

		// Token: 0x04000582 RID: 1410
		public float mBossTextY;

		// Token: 0x04000583 RID: 1411
		public float mBattleTextY;

		// Token: 0x04000584 RID: 1412
		public float mBossTextVY;

		// Token: 0x04000585 RID: 1413
		public float mBattleTextVY;

		// Token: 0x04000586 RID: 1414
		public int mBossIntroDirection;

		// Token: 0x04000587 RID: 1415
		public int mBossIntroFramesLeft;

		// Token: 0x04000588 RID: 1416
		public int mBossIntroDelay;

		// Token: 0x04000589 RID: 1417
		public FwooshImage mFightImage = new FwooshImage();

		// Token: 0x0400058A RID: 1418
		public bool mDoingBossIntroFightText;

		// Token: 0x0400058B RID: 1419
		public CurvedVal mBossIntroBGAlpha = new CurvedVal();

		// Token: 0x0400058C RID: 1420
		public CurvedVal mBossSmScale = new CurvedVal();

		// Token: 0x0400058D RID: 1421
		public CurvedVal mBossSmPosPct = new CurvedVal();

		// Token: 0x0400058E RID: 1422
		public CurvedVal mBossRedPct = new CurvedVal();

		// Token: 0x0400058F RID: 1423
		public int mAdvStatsTime;

		// Token: 0x04000590 RID: 1424
		public int mPreCheckpointLives;

		// Token: 0x04000591 RID: 1425
		public float mDarkFrogBulletX;

		// Token: 0x04000592 RID: 1426
		public float mDarkFrogBulletY;

		// Token: 0x04000593 RID: 1427
		public float mDarkFrogBulletVX;

		// Token: 0x04000594 RID: 1428
		public float mDarkFrogBulletVY;

		// Token: 0x04000595 RID: 1429
		public int mDarkFrogTimer;

		// Token: 0x04000596 RID: 1430
		public bool mForceRestartInAdvMode;

		// Token: 0x04000597 RID: 1431
		public bool mForceToNextLevelInAdvMode;

		// Token: 0x04000598 RID: 1432
		public bool mDisplayAceTime;

		// Token: 0x04000599 RID: 1433
		public float mVortexBGAlpha;

		// Token: 0x0400059A RID: 1434
		public bool mVortexAppear;

		// Token: 0x0400059B RID: 1435
		public float mVortexFrogRadius;

		// Token: 0x0400059C RID: 1436
		public float mVortexFrogAngle;

		// Token: 0x0400059D RID: 1437
		public float mVortexFrogScale;

		// Token: 0x0400059E RID: 1438
		public bool mVortexFrogRadiusExpand;

		// Token: 0x0400059F RID: 1439
		public float mAdventureWinExtraAlpha;

		// Token: 0x040005A0 RID: 1440
		public float mAdventureWinAlpha;

		// Token: 0x040005A1 RID: 1441
		public float mAdventureWinDoorYOff;

		// Token: 0x040005A2 RID: 1442
		public float mAdventureWinTimer;

		// Token: 0x040005A3 RID: 1443
		public float mTreasureVY;

		// Token: 0x040005A4 RID: 1444
		public float mTreasureDefaultVY;

		// Token: 0x040005A5 RID: 1445
		public float mTreasureAccel;

		// Token: 0x040005A6 RID: 1446
		public float mTreasureYBob;

		// Token: 0x040005A7 RID: 1447
		public int mLastIntroPad;

		// Token: 0x040005A8 RID: 1448
		public int mLastIntroPadDelay;

		// Token: 0x040005A9 RID: 1449
		public int mIntroPadHopCount;

		// Token: 0x040005AA RID: 1450
		public int mTreasureGlowAlpha;

		// Token: 0x040005AB RID: 1451
		public int mTreasureGlowAlphaRate;

		// Token: 0x040005AC RID: 1452
		public int mTreasureStarAlpha;

		// Token: 0x040005AD RID: 1453
		public int mTreasureEndFrame;

		// Token: 0x040005AE RID: 1454
		public int mTreasureCel;

		// Token: 0x040005AF RID: 1455
		public float mTreasureStarAngle;

		// Token: 0x040005B0 RID: 1456
		public int mLevelNum;

		// Token: 0x040005B1 RID: 1457
		public int mStateCount;

		// Token: 0x040005B2 RID: 1458
		public int mIgnoreCount;

		// Token: 0x040005B3 RID: 1459
		public int mAccuracyCount;

		// Token: 0x040005B4 RID: 1460
		public int mAccuracyBackupCount;

		// Token: 0x040005B5 RID: 1461
		public int mScreenShakeTime;

		// Token: 0x040005B6 RID: 1462
		public int mScreenShakeXMax;

		// Token: 0x040005B7 RID: 1463
		public int mScreenShakeYMax;

		// Token: 0x040005B8 RID: 1464
		public int mDestroyCount;

		// Token: 0x040005B9 RID: 1465
		public int mPauseCount;

		// Token: 0x040005BA RID: 1466
		public int mLastPauseTick;

		// Token: 0x040005BB RID: 1467
		public int mPauseUpdateCnt;

		// Token: 0x040005BC RID: 1468
		public int mPauseFade;

		// Token: 0x040005BD RID: 1469
		public int mDialogCount;

		// Token: 0x040005BE RID: 1470
		public int mLevelBeginScore;

		// Token: 0x040005BF RID: 1471
		public int mHallucinateTimer;

		// Token: 0x040005C0 RID: 1472
		public int mCurStatsPointCounter;

		// Token: 0x040005C1 RID: 1473
		public int mCurStatsPointTarget;

		// Token: 0x040005C2 RID: 1474
		public int mCurStatsPointInc;

		// Token: 0x040005C3 RID: 1475
		public int mLevelPoints;

		// Token: 0x040005C4 RID: 1476
		public int mStatsState;

		// Token: 0x040005C5 RID: 1477
		public int mStatsDelay;

		// Token: 0x040005C6 RID: 1478
		public bool mWasPerfectLevel;

		// Token: 0x040005C7 RID: 1479
		public bool mNeedsCheckpointIntro;

		// Token: 0x040005C8 RID: 1480
		public bool mHasSeenCheckpointIntro;

		// Token: 0x040005C9 RID: 1481
		public int mNumDeaths;

		// Token: 0x040005CA RID: 1482
		public int mStatsHue;

		// Token: 0x040005CB RID: 1483
		public int mUnpauseFrame;

		// Token: 0x040005CC RID: 1484
		public float mGauntletAlpha;

		// Token: 0x040005CD RID: 1485
		public float mGauntletTrophyDropRate;

		// Token: 0x040005CE RID: 1486
		public int mGauntletTrophyBounceCount;

		// Token: 0x040005CF RID: 1487
		public float mGauntletTrophyY;

		// Token: 0x040005D0 RID: 1488
		public Image mGauntletTrophyImg;

		// Token: 0x040005D1 RID: 1489
		public int mEndGauntletTimer;

		// Token: 0x040005D2 RID: 1490
		public float mGauntletMultBarAlpha;

		// Token: 0x040005D3 RID: 1491
		public int mGauntletLastFrogX;

		// Token: 0x040005D4 RID: 1492
		public int mGauntletLastFrogY;

		// Token: 0x040005D5 RID: 1493
		public bool mGauntletMultTextFlashOn;

		// Token: 0x040005D6 RID: 1494
		public int mGauntletMultTextFlashTimer;

		// Token: 0x040005D7 RID: 1495
		public float mGauntletMultTextVX;

		// Token: 0x040005D8 RID: 1496
		public float mGauntletMultTextVY;

		// Token: 0x040005D9 RID: 1497
		public int mGauntletMultTextMoveLastFrame;

		// Token: 0x040005DA RID: 1498
		public bool mGauntletModeOver;

		// Token: 0x040005DB RID: 1499
		public int mLives;

		// Token: 0x040005DC RID: 1500
		public int mPointsLeftForExtraLife;

		// Token: 0x040005DD RID: 1501
		public ButtonWidget mGauntletRetryBtn;

		// Token: 0x040005DE RID: 1502
		public ButtonWidget mGauntletQuitBtn;

		// Token: 0x040005DF RID: 1503
		public int mStartingGauntletLevel;

		// Token: 0x040005E0 RID: 1504
		public int mGauntletTikiUnlocked;

		// Token: 0x040005E1 RID: 1505
		public bool mNewGauntletHS;

		// Token: 0x040005E2 RID: 1506
		public int mGauntletHSIndex;

		// Token: 0x040005E3 RID: 1507
		public int mGauntletFinalScorePreBonus;

		// Token: 0x040005E4 RID: 1508
		public float mScoreDisplayPos;

		// Token: 0x040005E5 RID: 1509
		public List<ScoreLetterEffect> mScoreLetterEffectVector = new List<ScoreLetterEffect>();

		// Token: 0x040005E6 RID: 1510
		public string[] mScoreBreakStrings = new string[2];

		// Token: 0x040005E7 RID: 1511
		public SexyPoint[] mScoreBreakPositions = new SexyPoint[2];

		// Token: 0x040005E8 RID: 1512
		public int mGauntletWidestNameLen;

		// Token: 0x040005E9 RID: 1513
		public bool mNewIronFrogHS;

		// Token: 0x040005EA RID: 1514
		public int mNumClearsInARow;

		// Token: 0x040005EB RID: 1515
		public int mCurInARowBonus;

		// Token: 0x040005EC RID: 1516
		public int mCurComboScore;

		// Token: 0x040005ED RID: 1517
		public int mCurComboCount;

		// Token: 0x040005EE RID: 1518
		public int mNumCleared;

		// Token: 0x040005EF RID: 1519
		public GameStats mLevelStats = new GameStats();

		// Token: 0x040005F0 RID: 1520
		public GameStats mGameStats = new GameStats();

		// Token: 0x040005F1 RID: 1521
		public bool mGauntletMode;

		// Token: 0x040005F2 RID: 1522
		public bool mIsEndless;

		// Token: 0x040005F3 RID: 1523
		public bool mDoGuide;

		// Token: 0x040005F4 RID: 1524
		public bool mShowGuide;

		// Token: 0x040005F5 RID: 1525
		public bool mRecalcGuide;

		// Token: 0x040005F6 RID: 1526
		public bool mRecalcLazerGuide;

		// Token: 0x040005F7 RID: 1527
		public bool mShowBallsDuringPause;

		// Token: 0x040005F8 RID: 1528
		public bool mDestroyAll;

		// Token: 0x040005F9 RID: 1529
		public bool mLevelBeginning;

		// Token: 0x040005FA RID: 1530
		public bool mForceTreasure;

		// Token: 0x040005FB RID: 1531
		public bool mLazerHitTreasure;

		// Token: 0x040005FC RID: 1532
		public bool mHasDoneIntroSounds;

		// Token: 0x040005FD RID: 1533
		public bool mAllowBulletDetection;

		// Token: 0x040005FE RID: 1534
		public bool mIsLoading;

		// Token: 0x040005FF RID: 1535
		public bool mShowMapScreen;

		// Token: 0x04000600 RID: 1536
		public bool mDoPostBossMapScreen;

		// Token: 0x04000601 RID: 1537
		public bool mWasShowingCheckpoint;

		// Token: 0x04000602 RID: 1538
		public bool mDbgHurry;

		// Token: 0x04000603 RID: 1539
		public bool mShowDDSWindow;

		// Token: 0x04000604 RID: 1540
		public bool mShowBossDDSWindow;

		// Token: 0x04000605 RID: 1541
		public bool mCanDeleteEffectResources;

		// Token: 0x04000606 RID: 1542
		public Rect mAStatsFrame;

		// Token: 0x04000607 RID: 1543
		public SexyPoint mBossOffset = new SexyPoint();

		// Token: 0x04000608 RID: 1544
		public Rect mCStatsFrame = default(Rect);

		// Token: 0x04000609 RID: 1545
		public bool mIsTryAndBuyDialogShowing;

		// Token: 0x0400060A RID: 1546
		public int mFatFingerGuideAlpha;

		// Token: 0x0400060B RID: 1547
		public bool mFatFingerGuideEnabled;

		// Token: 0x0400060C RID: 1548
		public bool mIsHotFrogEnabled;

		// Token: 0x0400060D RID: 1549
		public int mBallPowerupCheat;

		// Token: 0x0400060E RID: 1550
		public ButtonWidget mSwapBallButton;

		// Token: 0x0400060F RID: 1551
		public bool mInvalidateTouchUp;

		// Token: 0x04000610 RID: 1552
		public bool mFinishHaloSwap;

		// Token: 0x04000611 RID: 1553
		public bool mDrawHaloSwap;

		// Token: 0x04000612 RID: 1554
		public CurvedVal mHaloSwapCurve = new CurvedVal();

		// Token: 0x04000613 RID: 1555
		public SexyColor mHaloSwapColor = default(SexyColor);

		// Token: 0x04000614 RID: 1556
		public MemoryImage mAimGuide;

		// Token: 0x04000615 RID: 1557
		public LivesInfo mLivesInfo;

		// Token: 0x04000616 RID: 1558
		public bool gDrawAutoAimAssistInfo;

		// Token: 0x04000617 RID: 1559
		public List<KeyValuePair<string, int>> m_NotificationQuene = new List<KeyValuePair<string, int>>();

		// Token: 0x04000618 RID: 1560
		public NotificationWidget mNotificationWidget;

		// Token: 0x04000619 RID: 1561
		public string prevLevelID = "";

		// Token: 0x0400061A RID: 1562
		private Board.CONTROL_MODE mControlMode;

		// Token: 0x0400061B RID: 1563
		private bool mIsMouseDown;

		// Token: 0x0400061C RID: 1564
		private SexyPoint mInitialTouchPoint = new SexyPoint();

		// Token: 0x0400061D RID: 1565
		private int mCurveClearBonus;

		// Token: 0x0400061E RID: 1566
		private int mTouchCount;

		// Token: 0x0400061F RID: 1567
		public bool mDrawBossUI;

		// Token: 0x04000620 RID: 1568
		private static SexyPoint gPt1;

		// Token: 0x04000621 RID: 1569
		private static SexyPoint gPt2;

		// Token: 0x04000622 RID: 1570
		private static SexyPoint gCenter;

		// Token: 0x04000623 RID: 1571
		private static bool gCheckCollision = true;

		// Token: 0x0200006D RID: 109
		private enum State
		{
			// Token: 0x04000625 RID: 1573
			StatsState_AceTime,
			// Token: 0x04000626 RID: 1574
			StatsState_Points,
			// Token: 0x04000627 RID: 1575
			StatsState_Done
		}

		// Token: 0x0200006E RID: 110
		protected enum BoardState
		{
			// Token: 0x04000629 RID: 1577
			BoardState_Game,
			// Token: 0x0400062A RID: 1578
			BoardState_Option,
			// Token: 0x0400062B RID: 1579
			BoardState_BackToMainMenuPrompt,
			// Token: 0x0400062C RID: 1580
			BoardState_OptionToMainMenuPrompt,
			// Token: 0x0400062D RID: 1581
			BoardState_Credits,
			// Token: 0x0400062E RID: 1582
			BoardState_Help
		}

		// Token: 0x0200006F RID: 111
		protected class GamepadControls
		{
			// Token: 0x0400062F RID: 1583
			public static int ACCEL_MAX = 4;

			// Token: 0x04000630 RID: 1584
			public float accel = 1f;

			// Token: 0x04000631 RID: 1585
			public SexyVector2 axis = default(SexyVector2);

			// Token: 0x04000632 RID: 1586
			public SexyVector2 lastAxis = default(SexyVector2);
		}

		// Token: 0x02000070 RID: 112
		private enum iOS_Button
		{
			// Token: 0x04000634 RID: 1588
			Button_SwapBalls,
			// Token: 0x04000635 RID: 1589
			NUM_BOARD_BUTTONS
		}

		// Token: 0x02000071 RID: 113
		private enum CONTROL_MODE
		{
			// Token: 0x04000637 RID: 1591
			CONTROL_MODE_NONE,
			// Token: 0x04000638 RID: 1592
			CONTROL_MODE_SWAPPING,
			// Token: 0x04000639 RID: 1593
			CONTROL_MODE_DODGING,
			// Token: 0x0400063A RID: 1594
			CONTROL_MODE_AIMING
		}
	}
}
