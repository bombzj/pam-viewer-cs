using System;
using System.Collections.Generic;
using System.Linq;
using JeffLib;
using SexyFramework;
using SexyFramework.Drivers;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000110 RID: 272
	public class LoadingScreen : Widget, ButtonListener
	{
		// Token: 0x06000E25 RID: 3621 RVA: 0x0008F728 File Offset: 0x0008D928
		protected void DrawLightning(Graphics g, int x, int cloud_num)
		{
			LoadingCloud loadingCloud = this.mClouds[cloud_num];
			if (loadingCloud.mLightning == null)
			{
				return;
			}
			int num = 2;
			int num2 = 2;
			Image imageByID = Res.GetImageByID(ResID.IMAGE_LS_CLOUD1A + cloud_num * 3);
			float num3 = loadingCloud.mLightningScale * (float)(loadingCloud.mLightning.mWidth * num2);
			float num4 = loadingCloud.mLightningScale * (float)(loadingCloud.mLightning.mHeight * num2);
			g.PushState();
			int alpha = (int)(255f * ((float)loadingCloud.mLightningTimer / (float)loadingCloud.mTimerTarget));
			g.SetColor(255, 255, 255, alpha);
			g.SetColorizeImages(true);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_LS_CLOUD1B + cloud_num * 3);
			int num5 = 10;
			g.DrawImage(imageByID2, x, ZumasRevenge.Common._DS((int)loadingCloud.mY + ZumasRevenge.Common._M(0)), imageByID2.GetWidth() * num5, imageByID2.GetHeight() * num5);
			g.DrawImage(loadingCloud.mLightning, x + (imageByID.mWidth * num - (int)num3) / 2, (int)ZumasRevenge.Common._DS(loadingCloud.mY) + imageByID.mHeight * num / 3, (int)num3, (int)num4);
			g.PopState();
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0008F848 File Offset: 0x0008DA48
		public override void DrawOverlay(Graphics g)
		{
			if (this.mBlackFadeAlpha > 0f)
			{
				g.PushState();
				g.SetColor(0, 0, 0, (int)this.mBlackFadeAlpha);
				g.FillRect(GlobalMembers.gSexyApp.mScreenBounds);
				g.PopState();
			}
			g.PushState();
			if (this.mFadeToMainMenu && !this.mBlackFadeIn)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)this.mBlackFadeAlpha);
			}
			g.PopState();
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0008F8CD File Offset: 0x0008DACD
		public bool CanLoad()
		{
			return this.mState == 2 && !this.mWaitingForConfirmation;
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0008F8E3 File Offset: 0x0008DAE3
		public bool Done()
		{
			return this.mFadeToMainMenu && this.mBlackFadeAlpha <= 0f;
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x0008F8FF File Offset: 0x0008DAFF
		public bool CanShowMenu()
		{
			if (this.mFadeToMainMenu && this.mCanShowMenu && (!this.mBlackFadeIn || GameApp.gApp.mMinimized) && this.mUserProfileLoaded)
			{
				this.mCanShowMenu = false;
				return true;
			}
			return false;
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x0008F938 File Offset: 0x0008DB38
		public void LoadingComplete()
		{
			this.mPantalookRipplePct.SetCurve(ZumasRevenge.Common._MP("b;0,1,0.003333,1,~###         ~####"));
			this.mPantaloonFlopPct.SetCurve(ZumasRevenge.Common._MP("b;0,1,0.002857,1,####    b####     ?~d,o"));
			this.mLoadingComplete = true;
			for (int i = 0; i < LoadingScreen.MAX_VOLCANO_PROJECTILES; i++)
			{
				this.mVolcanoProjectiles[i].mProjectile = GameApp.gApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_PARTICLES_VOLCANO_PROJECTILE").Duplicate();
				this.mVolcanoProjectiles[i].mProjectile.mEmitAfterTimeline = true;
				this.mVolcanoProjectiles[i].mProjectile.mOptimizeValue = 2;
				this.mVolcanoProjectiles[i].mProjectile.mInUse = false;
				this.mVolcanoProjectiles[i].mProjectile.mDrawTransform.LoadIdentity();
				this.mVolcanoProjectiles[i].mProjectile.mDrawTransform.Scale(ZumasRevenge.Common._DS(1.4f), ZumasRevenge.Common._DS(1.4f));
				this.mVolcanoProjectiles[i].mProjectile.mDrawTransform.Translate((float)(ZumasRevenge.Common._DS(ZumasRevenge.Common._M(790)) + this.mOffsetParticle), (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(150)));
				ZumasRevenge.Common.SetFXNumScale(this.mVolcanoProjectiles[i].mProjectile, 3f);
				this.mEffectBatch.AddEffect(this.mVolcanoProjectiles[i].mProjectile);
			}
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0008FA9C File Offset: 0x0008DC9C
		public override void MouseDown(int x, int y, int theClickCount)
		{
			if (this.mCompleteLoadingBarAlpha >= 255f && !this.mFadeToMainMenu)
			{
				this.mUserProfileLoaded = true;
				if (GameApp.gApp.mUserProfile == null && this.mLoadingComplete && !this.mFadeToMainMenu)
				{
					GameApp.gApp.mUserProfile = (ZumaProfile)GameApp.gApp.mProfileMgr.GetProfile(GameApp.gApp.m_DefaultProfileName);
					if (GameApp.gApp.mUserProfile != null)
					{
						this.mBlackFadeAlpha = 0.0001f;
					}
				}
				if (GameApp.USE_XBOX_SERVICE && !GameApp.USE_TRIAL_VERSION)
				{
					GameApp.gApp.mUserProfile.m_AchievementMgr.SyncAchievementsXLive();
				}
				this.mFadeToMainMenu = true;
			}
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0008FB4F File Offset: 0x0008DD4F
		public override void GotFocus()
		{
			base.GotFocus();
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0008FB58 File Offset: 0x0008DD58
		public override void Resize(int x, int y, int width, int height)
		{
			base.Resize(x, y, width, height);
			this.pts[0] = new SexyPoint((this.mWidth - this.IMAGE_LS_LOGO1.mWidth) / 2, (this.mHeight - this.IMAGE_LS_LOGO1.mHeight) / 2);
			this.pts[1] = new SexyPoint((this.mWidth - this.IMAGE_LS_LOGO1.mWidth) / 2, (this.mHeight - this.IMAGE_LS_LOGO1.mHeight) / 2 - 77);
			this.pts[2] = new SexyPoint((this.mWidth - this.IMAGE_LS_LOGO1.mWidth) / 2, (this.mHeight - this.IMAGE_LS_LOGO1.mHeight) / 2 - 77);
			this.pts[3] = new SexyPoint((this.mWidth - this.IMAGE_LS_LOGO1.mWidth) / 2, (this.mHeight - this.IMAGE_LS_LOGO1.mHeight) / 2 - 77);
			this.pts[4] = new SexyPoint((this.mWidth - this.IMAGE_LS_LOGO1.mWidth) / 2 - 135, (this.mHeight - this.IMAGE_LS_LOGO1.mHeight) / 2 - 77);
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0008FC8B File Offset: 0x0008DE8B
		public override void GamepadButtonDown(GamepadButton theButton, int thePlayer, uint theFlags)
		{
			uint num = theFlags & 1U;
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0008FC91 File Offset: 0x0008DE91
		private float GetMainMenuAlpha()
		{
			return this.mBlackFadeAlpha;
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0008FC9C File Offset: 0x0008DE9C
		public LoadingScreen()
		{
			this.mDarkIslandAlpha = 255;
			this.mLogoHoldTime = 200;
			this.Init();
			this.mWaitingForConfirmation = false;
			if (GameApp.gApp.mFromReInit)
			{
				this.mState = 2;
				this.mFlashAlpha = 0f;
				SoundAttribs soundAttribs = new SoundAttribs();
				soundAttribs.fadein = 0.01f;
				soundAttribs.fadeout = 0.005f;
				GameApp.gApp.mSoundPlayer.Loop(Res.GetSoundByID(ResID.SOUND_LS_STORM_LOOP), soundAttribs);
			}
			this.mClip = false;
			this.mFirstRun = false;
			this.mLoadingCompleteTime = 0;
			if (this.mFirstRun)
			{
				this.mLoadingTextIdx = 0;
			}
			else
			{
				this.mLoadingTextIdx = SexyFramework.Common.Rand() % Enumerable.Count<string>(this.mLoadingTextContainer.GetLoadingText());
			}
			this.mSeenLoadingTextIndices.Add(this.mLoadingTextIdx);
			this.mLoadingTextTime = LoadingScreen.LOADING_TEXT_TIME;
			this.mCloudUpdateCount = 0;
			this.mRippleCnt = 0f;
			this.mPantalookRipplePct.SetConstant(1.0);
			this.mUserProfileLoaded = true;
			this.mLoading = false;
			this.mLeftTorch = null;
			this.mRightTorch = null;
			this.mVolcanoSmoke = null;
			this.mLoadingYOffset = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(998));
			this.mLoadingTextFrame.mWidth = this.IMAGE_LS_REDLOADINGBAR.GetWidth();
			this.mLoadingTextFrame.mHeight = this.IMAGE_LS_REDLOADINGBAR.GetHeight();
			this.mLoadingTextFrame.mX = ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_LS_REDLOADINGBAR) - this.mLoadingXOffset);
			this.mLoadingTextFrame.mY = ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_LS_REDLOADINGBAR)) + this.mLoadingYOffset;
			for (int i = 0; i < LoadingScreen.MAX_VOLCANO_PROJECTILES; i++)
			{
				this.mVolcanoProjectiles[i] = new VolcanoProjectile();
				this.mVolcanoProjectiles[i].mProjectile = null;
			}
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00090090 File Offset: 0x0008E290
		private void Init()
		{
			int seed = (int)SexyFramework.Common.SexyTime();
			LoadingScreen.RandomNumbers.Seed(seed);
			MathUtils.Seed(seed);
			SexyApp gSexyApp = GlobalMembers.gSexyApp;
			this.mLoadingXOffset = 0;
			this.mLavaAlpha = 0f;
			this.mIncLavaAlpha = true;
			this.mLoadingCompleteDelay = 0;
			this.mState = 0;
			this.mHasShown = false;
			this.mLightningOn = true;
			this.mLightningTimer = 0;
			this.mLightningFrame = 0;
			this.mCanShowMenu = true;
			this.mLoadingBarAlpha = 0f;
			this.mCompleteLoadingBarAlpha = 0f;
			this.mBlackFadeAlpha = 255f;
			this.mLoadingComplete = false;
			this.mBlackFadeIn = true;
			this.mStormTimer = (this.mClearTimer = ZumasRevenge.Common._M(100));
			this.mFadeToMainMenu = false;
			this.mWaves[0] = new LoadingWave();
			this.mWaves[1] = new LoadingWave();
			this.mWaves[2] = new LoadingWave();
			this.mWaves[3] = new LoadingWave();
			this.mWaves[4] = new LoadingWave();
			this.mWaves[0].mRadius = ZumasRevenge.Common._M(20f);
			this.mWaves[0].mAngle = ZumasRevenge.Common._M1(1f);
			this.mWaves[0].mAngleRate = ZumasRevenge.Common._M2(-0.035f);
			this.mWaves[0].mY = (float)ZumasRevenge.Common._M3(860);
			this.mWaves[1].mRadius = ZumasRevenge.Common._M(16f);
			this.mWaves[1].mAngle = ZumasRevenge.Common._M1(-1f);
			this.mWaves[1].mAngleRate = ZumasRevenge.Common._M2(-0.025f);
			this.mWaves[1].mY = (float)ZumasRevenge.Common._M3(700);
			this.mWaves[2].mRadius = ZumasRevenge.Common._M(12f);
			this.mWaves[2].mAngle = ZumasRevenge.Common._M1(2f);
			this.mWaves[2].mAngleRate = ZumasRevenge.Common._M2(-0.015f);
			this.mWaves[2].mY = (float)ZumasRevenge.Common._M3(620);
			this.mWaves[3].mRadius = ZumasRevenge.Common._M(10f);
			this.mWaves[3].mAngle = ZumasRevenge.Common._M1(-2f);
			this.mWaves[3].mAngleRate = ZumasRevenge.Common._M2(-0.01f);
			this.mWaves[3].mY = (float)ZumasRevenge.Common._M3(520);
			this.mWaves[4].mRadius = ZumasRevenge.Common._M(6f);
			this.mWaves[4].mAngle = ZumasRevenge.Common._M1(3f);
			this.mWaves[4].mAngleRate = ZumasRevenge.Common._M2(-0.005f);
			this.mWaves[4].mY = (float)ZumasRevenge.Common._M3(460);
			this.mCalmWaves[0] = new LoadingWave();
			this.mCalmWaves[1] = new LoadingWave();
			this.mCalmWaves[2] = new LoadingWave();
			this.mCalmWaves[3] = new LoadingWave();
			this.mCalmWaves[0].mRadius = ZumasRevenge.Common._M(2f);
			this.mCalmWaves[0].mAngle = ZumasRevenge.Common._M1(1f);
			this.mCalmWaves[0].mAngleRate = ZumasRevenge.Common._M2(-0.035f) / 2f;
			this.mCalmWaves[0].mY = (float)ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_LS_CALM_WAVE_1));
			this.mCalmWaves[1].mRadius = ZumasRevenge.Common._M(8f);
			this.mCalmWaves[1].mAngle = ZumasRevenge.Common._M1(-1f);
			this.mCalmWaves[1].mAngleRate = ZumasRevenge.Common._M2(-0.025f) / 2f;
			this.mCalmWaves[1].mY = (float)ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_LS_CALM_WAVE_2));
			this.mCalmWaves[2].mRadius = ZumasRevenge.Common._M(6f);
			this.mCalmWaves[2].mAngle = ZumasRevenge.Common._M1(2f);
			this.mCalmWaves[2].mAngleRate = ZumasRevenge.Common._M2(-0.015f) / 2f;
			this.mCalmWaves[2].mY = (float)ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_LS_CALM_WAVE_3));
			this.mCalmWaves[3].mRadius = ZumasRevenge.Common._M(5f);
			this.mCalmWaves[3].mAngle = ZumasRevenge.Common._M1(-2f);
			this.mCalmWaves[3].mAngleRate = ZumasRevenge.Common._M2(-0.01f) / 2f;
			this.mCalmWaves[3].mY = (float)ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_LS_CALM_WAVE_4));
			for (int i = 0; i < 4; i++)
			{
				this.mCalmWaves[i].mVX = 0.5f / (float)(i + 1);
				this.mCalmWaves[i].mXOff = 0f;
				this.mCalmWaves[i].mMaxXOff = ZumasRevenge.Common._DS(20f) / (float)(i + 1);
				this.mCalmWaves[i].mY = (float)(gSexyApp.mHeight - ZumasRevenge.Common._DS(452)) + this.mCalmWaves[i].mY;
				if (i % 2 == 0)
				{
					this.mCalmWaves[i].mIncVX = true;
				}
			}
			this.mFrogAngleDivisor = 1f;
			this.mFlashAlpha = 0f;
			this.mExtraProgress = 0f;
			this.mFrogAngle = 0f;
			this.mFrogPitchForward = true;
			this.mFrogAngleDelta = ZumasRevenge.Common._M(0.004f);
			this.mClouds[0] = new LoadingCloud();
			this.mClouds[1] = new LoadingCloud();
			this.mClouds[2] = new LoadingCloud();
			this.mClouds[0].mStartX = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(-400));
			this.mClouds[0].mY = (float)ZumasRevenge.Common._M1(-400);
			this.mClouds[0].mShadowOffset = (float)(ZumasRevenge.Common._DS(ZumasRevenge.Common._M2(-30)) - ZumasRevenge.Common._DS(160));
			this.mClouds[0].mShadowY = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M3(450));
			this.mClouds[1].mStartX = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(0));
			this.mClouds[1].mY = (float)ZumasRevenge.Common._M1(0);
			this.mClouds[1].mShadowOffset = (float)(ZumasRevenge.Common._DS(ZumasRevenge.Common._M2(-19)) - ZumasRevenge.Common._DS(160));
			this.mClouds[1].mShadowY = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M3(550));
			this.mClouds[2].mStartX = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(-320));
			this.mClouds[2].mY = (float)ZumasRevenge.Common._M1(25);
			this.mClouds[2].mShadowOffset = (float)(ZumasRevenge.Common._DS(ZumasRevenge.Common._M2(-70)) - ZumasRevenge.Common._DS(160));
			this.mClouds[2].mShadowY = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M3(650));
			this.mFrogWave = 0;
			this.mFrogPct = 0f;
			this.mFrogScale = 1f;
			this.mEffectBatch = new PIEffectBatch();
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x000907B4 File Offset: 0x0008E9B4
		public override void Update()
		{
			if (GameApp.gApp.IsHardwareBackButtonPressed())
			{
				this.ProcessHardwareBackButton();
			}
			base.Update();
			if (!this.mLoadingComplete && GameApp.gApp.GetLoadingThreadProgress() >= 1.0)
			{
				GameApp.gApp.LoadingThreadCompleted();
			}
			if (GameApp.gApp.StartLoadingComplete)
			{
				GameApp.gApp.LoadLevelXML();
				GameApp.gApp.StartLoadingComplete = false;
			}
			if (this.mBlackFadeIn && this.mState == 0)
			{
				this.mBlackFadeAlpha -= 2f;
				if (this.mBlackFadeAlpha <= 0f)
				{
					this.mBlackFadeAlpha = 0f;
				}
			}
			if (this.mLockBGM)
			{
				return;
			}
			if (this.mFlashAlpha > 0f)
			{
				this.mFlashAlpha -= ((this.mState == 2) ? ZumasRevenge.Common._M(1.5f) : ZumasRevenge.Common._M1(10f));
				if (this.mFlashAlpha < 0f)
				{
					this.mFlashAlpha = 0f;
				}
			}
			if (!this.mLoading && GameApp.gApp.mLoadLevelSuccess)
			{
				GameApp.gApp.LoadingThreadProc();
				this.mLoading = true;
			}
			if (!this.mLoadingComplete)
			{
				if (--this.mLoadingTextTime == 0)
				{
					if (this.mSeenLoadingTextIndices.Capacity == Enumerable.Count<string>(this.mLoadingTextContainer.GetLoadingText()))
					{
						this.mSeenLoadingTextIndices.Clear();
					}
					List<int> list = new List<int>();
					for (int i = 0; i < Enumerable.Count<string>(this.mLoadingTextContainer.GetLoadingText()); i++)
					{
						bool flag = false;
						for (int j = 0; j < Enumerable.Count<int>(this.mSeenLoadingTextIndices); j++)
						{
							if (this.mSeenLoadingTextIndices[j] == i)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							list.Add(i);
						}
					}
					if (list.Count == 0)
					{
						this.mLoadingTextIdx = 0;
						this.mSeenLoadingTextIndices.Clear();
					}
					else if (this.mFirstRun)
					{
						this.mLoadingTextIdx = (this.mLoadingTextIdx + 1) % Enumerable.Count<string>(this.mLoadingTextContainer.GetBackStoryText());
					}
					else
					{
						int num = LoadingScreen.RandomNumbers.NextNumber() % list.Count;
						this.mLoadingTextIdx = list[num];
					}
					this.mSeenLoadingTextIndices.Add(this.mLoadingTextIdx);
					this.mLoadingTextTime = LoadingScreen.LOADING_TEXT_TIME;
				}
			}
			else
			{
				this.mLoadingCompleteTime++;
				if (this.mLoadingCompleteTime == 200)
				{
					GameApp.gApp.PlaySong(0);
				}
			}
			if (this.mFrogPitchForward)
			{
				this.mFrogAngle -= this.mFrogAngleDelta / this.mFrogAngleDivisor;
				if (this.mFrogAngle <= -LoadingScreen.max_angle)
				{
					this.mFrogAngle = -LoadingScreen.max_angle;
					this.mFrogAngleDelta *= -1f;
					this.mFrogPitchForward = false;
				}
			}
			else
			{
				this.mFrogAngle -= this.mFrogAngleDelta / this.mFrogAngleDivisor;
				if (this.mFrogAngle >= LoadingScreen.max_angle)
				{
					this.mFrogAngle = LoadingScreen.max_angle;
					this.mFrogAngleDelta = MathUtils.FloatRange(ZumasRevenge.Common._M(0.0015f), ZumasRevenge.Common._M1(0.003f));
					if (this.mLoadingComplete && MathUtils._geq(this.mExtraProgress, 1f, 0.01f))
					{
						LoadingScreen.max_angle = 0.04313f;
					}
					this.mFrogPitchForward = true;
				}
			}
			if (this.mLoadingComplete && this.mState == 2)
			{
				if (this.mStormTimer > 0)
				{
					if (--this.mStormTimer == 0)
					{
						GameApp.gApp.mSoundPlayer.Fade(Res.GetSoundByID(ResID.SOUND_LS_STORM_LOOP));
					}
				}
				else if (++this.mLoadingCompleteDelay >= ZumasRevenge.Common._M(5))
				{
					this.mExtraProgress += ZumasRevenge.Common._M(0.003f);
					if (this.mExtraProgress > 1f)
					{
						this.mExtraProgress = 1f;
						if (--this.mClearTimer <= 0 && !GameApp.gApp.mFromReInit)
						{
							this.mState++;
							SoundAttribs soundAttribs = new SoundAttribs();
							soundAttribs.fadeout = 0.1f;
							GameApp.gApp.mSoundPlayer.Loop(Res.GetSoundByID(ResID.SOUND_SEAGULLS), soundAttribs);
						}
					}
				}
			}
			if (MathUtils._geq(this.mExtraProgress, 0.5f) && this.mDarkIslandAlpha > 0)
			{
				this.mDarkIslandAlpha -= ZumasRevenge.Common._M(2);
			}
			if (this.mState >= 2 && !this.mFadeToMainMenu)
			{
				if (this.mLoadingBarAlpha < 255f)
				{
					this.mLoadingBarAlpha += ZumasRevenge.Common._M(2f);
				}
				if (this.mLoadingBarAlpha > 255f)
				{
					this.mLoadingBarAlpha = 255f;
				}
				if (this.mFrogPct < ZumasRevenge.Common._M(0.8f))
				{
					this.mFrogPct += ZumasRevenge.Common._M(0.0001f) / (float)(this.mFrogWave + 1);
				}
			}
			if (this.mState == 0)
			{
				if (this.mHasShown)
				{
					if (this.mPartnerLogos.Capacity > 0)
					{
						PartnerLogo partnerLogo = this.mPartnerLogos[0];
						if (partnerLogo.mAlpha < 255 && partnerLogo.mTime == partnerLogo.mOrgTime)
						{
							partnerLogo.mAlpha += ZumasRevenge.Common._M(5);
							if (partnerLogo.mAlpha >= 255)
							{
								partnerLogo.mAlpha = 255;
							}
						}
						else if (--partnerLogo.mTime <= 0)
						{
							partnerLogo.mAlpha -= ZumasRevenge.Common._M(5);
							if (partnerLogo.mAlpha <= 0)
							{
								this.mPartnerLogos.RemoveAt(0);
							}
						}
					}
					else if (--this.mLogoHoldTime == 0)
					{
						GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_LS_THUNDERSTRIKE));
						this.mState++;
						GameApp.gApp.InitMetricsManager();
					}
				}
			}
			else if (this.mState == 1)
			{
				int[] array = new int[]
				{
					ZumasRevenge.Common._M(5),
					ZumasRevenge.Common._M1(10),
					ZumasRevenge.Common._M2(10),
					ZumasRevenge.Common._M3(15),
					ZumasRevenge.Common._M4(10)
				};
				int[] array2 = new int[]
				{
					ZumasRevenge.Common._M(5),
					ZumasRevenge.Common._M1(5),
					ZumasRevenge.Common._M2(15),
					ZumasRevenge.Common._M3(10),
					ZumasRevenge.Common._M4(10)
				};
				int num2 = (this.mLightningOn ? array[this.mLightningFrame] : array2[this.mLightningFrame]);
				if (++this.mLightningTimer == num2)
				{
					this.mLightningTimer = 0;
					this.mLightningOn = !this.mLightningOn;
					if (this.mLightningOn && this.mFlashAlpha <= 0f)
					{
						this.mFlashAlpha = 255f;
					}
					if (this.mLightningOn && ++this.mLightningFrame == 5)
					{
						this.mFlashAlpha = 255f;
						this.mState++;
					}
				}
				if (Enumerable.Count<LogoLightning>(this.mLogoLightning) < ZumasRevenge.Common._M(3) && MathUtils.SafeRand() % ZumasRevenge.Common._M1(20) == 0)
				{
					this.mLogoLightning.Add(new LogoLightning());
					LogoLightning logoLightning = this.mLogoLightning[Enumerable.Count<LogoLightning>(this.mLogoLightning) - 1];
					logoLightning.mImage = Res.GetImageByID(ResID.IMAGE_LS_LIGHT1 + MathUtils.SafeRand() % 2);
					logoLightning.mTimer = (logoLightning.mTimerTarget = MathUtils.IntRange(ZumasRevenge.Common._M(5), ZumasRevenge.Common._M1(25)));
				}
				for (int k = 0; k < Enumerable.Count<LogoLightning>(this.mLogoLightning); k++)
				{
					LogoLightning logoLightning2 = this.mLogoLightning[k];
					if (--logoLightning2.mTimer == 0)
					{
						this.mLogoLightning.RemoveAt(k);
						k--;
					}
				}
			}
			if (this.mLoadingComplete && !GameApp.gApp.mFromReInit && this.mCompleteLoadingBarAlpha < 255f && this.mLoadingCompleteTime >= 400)
			{
				this.mCompleteLoadingBarAlpha += ZumasRevenge.Common._M(3f);
				if (this.mCompleteLoadingBarAlpha > 255f)
				{
					this.mCompleteLoadingBarAlpha = 255f;
				}
			}
			if (this.mFadeToMainMenu)
			{
				if (this.mBlackFadeIn)
				{
					this.mBlackFadeAlpha += ZumasRevenge.Common._M(5f);
					if (this.mBlackFadeAlpha >= 255f)
					{
						this.mBlackFadeAlpha = 255f;
						this.mBlackFadeIn = false;
					}
				}
				else
				{
					this.mBlackFadeAlpha -= ZumasRevenge.Common._M(2f);
					if (this.mBlackFadeAlpha <= 0f)
					{
						this.mBlackFadeAlpha = 0f;
					}
				}
			}
			if (this.mLoadingComplete && this.mExtraProgress < 0.99f)
			{
				this.mFrogAngleDivisor += ZumasRevenge.Common._M(0.018f);
			}
			for (int l = 0; l < 5; l++)
			{
				LoadingWave loadingWave = this.mWaves[l];
				loadingWave.mAngle += loadingWave.mAngleRate;
				if (l < 4)
				{
					float num3 = ZumasRevenge.Common._M(0.0005f) / (float)(l + 1);
					this.mCalmWaves[l].mAngle += this.mCalmWaves[l].mAngleRate;
					if (!this.mCalmWaves[l].mIncVX)
					{
						this.mCalmWaves[l].mVX -= num3;
					}
					else
					{
						this.mCalmWaves[l].mVX += num3;
					}
					float num4 = ZumasRevenge.Common._M(0.2f);
					if (this.mCalmWaves[l].mVX > num4)
					{
						this.mCalmWaves[l].mVX = num4;
					}
					else if (this.mCalmWaves[l].mVX < -num4)
					{
						this.mCalmWaves[l].mVX = -num4;
					}
					this.mCalmWaves[l].mXOff += this.mCalmWaves[l].mVX;
					if (this.mCalmWaves[l].mXOff >= this.mCalmWaves[l].mMaxXOff)
					{
						if (this.mCalmWaves[l].mVX > 0f)
						{
							this.mCalmWaves[l].mVX /= ZumasRevenge.Common._M(4f);
						}
						this.mCalmWaves[l].mIncVX = false;
					}
					else if (this.mCalmWaves[l].mXOff <= -this.mCalmWaves[l].mMaxXOff)
					{
						if (this.mCalmWaves[l].mVX < 0f)
						{
							this.mCalmWaves[l].mVX /= ZumasRevenge.Common._M(4f);
						}
						this.mCalmWaves[l].mIncVX = true;
					}
				}
			}
			for (int m = 0; m < 3; m++)
			{
				LoadingCloud loadingCloud = this.mClouds[m];
				if (loadingCloud.mLightning == null && this.mExtraProgress == 0f && MathUtils.SafeRand() % ZumasRevenge.Common._M(200) == 0)
				{
					loadingCloud.mLightning = Res.GetImageByID(ResID.IMAGE_LS_LIGHT1 + MathUtils.SafeRand() % 1);
					loadingCloud.mLightningTimer = (loadingCloud.mTimerTarget = MathUtils.IntRange(ZumasRevenge.Common._M(10), ZumasRevenge.Common._M1(25)));
					loadingCloud.mLightningScale = ZumasRevenge.Common._M(0.75f) - (float)m * ZumasRevenge.Common._M1(0.15f);
				}
				if (loadingCloud.mLightningTimer > 0 && --loadingCloud.mLightningTimer <= 0)
				{
					loadingCloud.mLightning = null;
				}
			}
			if (this.mLoadingComplete && this.mExtraProgress > 0f)
			{
				if (this.mIncLavaAlpha)
				{
					this.mLavaAlpha += ZumasRevenge.Common._M(0.5f);
					if (this.mLavaAlpha >= 255f)
					{
						this.mLavaAlpha = 255f;
						this.mIncLavaAlpha = false;
					}
				}
				else
				{
					this.mLavaAlpha -= ZumasRevenge.Common._M(0.5f);
					if (this.mLavaAlpha <= 0f)
					{
						this.mLavaAlpha = 0f;
						this.mIncLavaAlpha = true;
					}
				}
			}
			if (this.mLoadingComplete)
			{
				if (this.mLeftTorch == null)
				{
					this.mLeftTorch = GameApp.gApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_PARTICLES_LS_TIKITORCH_FLAME").Duplicate();
					this.mLeftTorch.mEmitAfterTimeline = true;
					this.mLeftTorch.mDrawTransform.LoadIdentity();
					this.mLeftTorch.mDrawTransform.Scale(ZumasRevenge.Common._DS(1.4f), ZumasRevenge.Common._DS(1.4f));
					this.mLeftTorch.mDrawTransform.Translate((float)(ZumasRevenge.Common._S(this.mX) + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(264)) + this.mOffsetParticle), (float)(ZumasRevenge.Common._S(this.mY) + ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(430))));
					ZumasRevenge.Common.SetFXNumScale(this.mLeftTorch, 4f);
					this.mEffectBatch.AddEffect(this.mLeftTorch);
				}
				if (this.mRightTorch == null)
				{
					this.mRightTorch = GameApp.gApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_PARTICLES_LS_TIKITORCH_FLAME").Duplicate();
					this.mRightTorch.mEmitAfterTimeline = true;
					this.mRightTorch.mDrawTransform.LoadIdentity();
					this.mRightTorch.mDrawTransform.Scale(ZumasRevenge.Common._DS(1.4f), ZumasRevenge.Common._DS(1.4f));
					this.mRightTorch.mDrawTransform.RotateDeg((float)ZumasRevenge.Common._M(-20));
					this.mRightTorch.mDrawTransform.Translate((float)(ZumasRevenge.Common._S(this.mX) + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(1357)) + this.mOffsetParticle), (float)(ZumasRevenge.Common._S(this.mY) + ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(430))));
					ZumasRevenge.Common.SetFXNumScale(this.mRightTorch, 4f);
					this.mEffectBatch.AddEffect(this.mRightTorch);
				}
				if (this.mVolcanoSmoke == null)
				{
					this.mVolcanoSmoke = GameApp.gApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_PARTICLES_VOLCANO_SMOKE").Duplicate();
					this.mVolcanoSmoke.mEmitAfterTimeline = true;
					this.mVolcanoSmoke.mDrawTransform.LoadIdentity();
					this.mVolcanoSmoke.mDrawTransform.Scale(ZumasRevenge.Common._DS(1.4f), ZumasRevenge.Common._DS(1.4f));
					this.mVolcanoSmoke.mDrawTransform.Translate((float)(ZumasRevenge.Common._S(this.mX) + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(790)) + this.mOffsetParticle), (float)(ZumasRevenge.Common._S(this.mY) + ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(90))));
					ZumasRevenge.Common.SetFXNumScale(this.mVolcanoSmoke, 3f);
					this.mEffectBatch.AddEffect(this.mVolcanoSmoke);
				}
				if (this.mLeftTorch != null)
				{
					this.mLeftTorch.Update();
				}
				if (this.mRightTorch != null)
				{
					this.mRightTorch.Update();
				}
				if (this.mVolcanoSmoke != null)
				{
					this.mVolcanoSmoke.Update();
				}
			}
			if (SexyFramework.Common.Rand(ZumasRevenge.Common._M(100)) == 0)
			{
				for (int n = 0; n < LoadingScreen.MAX_VOLCANO_PROJECTILES; n++)
				{
					VolcanoProjectile volcanoProjectile = this.mVolcanoProjectiles[n];
					if (volcanoProjectile.mProjectile != null && !volcanoProjectile.mProjectile.mInUse)
					{
						volcanoProjectile.mProjectile.mInUse = true;
						volcanoProjectile.mProjectile.ResetAnim();
						volcanoProjectile.mProjectile.mRandSeeds.Clear();
						volcanoProjectile.mProjectile.mRandSeeds.Add(SexyFramework.Common.Rand(1000));
						break;
					}
				}
			}
			for (int num5 = 0; num5 < LoadingScreen.MAX_VOLCANO_PROJECTILES; num5++)
			{
				VolcanoProjectile volcanoProjectile2 = this.mVolcanoProjectiles[num5];
				if (volcanoProjectile2.mProjectile != null && volcanoProjectile2.mProjectile.mInUse && volcanoProjectile2.mProjectile != null)
				{
					volcanoProjectile2.mProjectile.Update();
					if (volcanoProjectile2.mProjectile.mCurNumParticles == 0 && MathUtils._geq(volcanoProjectile2.mProjectile.mFrameNum, (float)volcanoProjectile2.mProjectile.mLastFrameNum))
					{
						volcanoProjectile2.mProjectile.mInUse = false;
					}
				}
			}
			if (this.mLoadingComplete)
			{
				for (int num6 = 0; num6 < 3; num6++)
				{
					LoadingCloud loadingCloud2 = this.mClouds[num6];
					loadingCloud2.mVX += ZumasRevenge.Common._M(0.0004f);
				}
			}
			if (GameApp.gApp.mUserProfile != null && GameApp.gApp.mUserProfile.IsLoaded())
			{
				this.mUserProfileLoaded = true;
			}
			this.mRippleCnt += (float)this.mPantalookRipplePct;
			this.MarkDirty();
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x000918A0 File Offset: 0x0008FAA0
		public override void Dispose()
		{
			this.RemoveAllWidgets(true, true);
			this.mLeftTorch.Dispose();
			this.mRightTorch.Dispose();
			this.mVolcanoSmoke.Dispose();
			for (int i = 0; i < LoadingScreen.MAX_VOLCANO_PROJECTILES; i++)
			{
				this.mVolcanoProjectiles[i].mProjectile.Dispose();
			}
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x000918F8 File Offset: 0x0008FAF8
		public override void Draw(Graphics g)
		{
			if (this.mState >= 2 && this.mState == 2)
			{
				GameApp.gApp.GetLoadingThreadProgress();
			}
			this.mHasShown = true;
			if (this.mWaitingForConfirmation)
			{
				g.SetColor(SexyColor.Black);
				g.FillRect(GlobalMembers.gSexyApp.mScreenBounds);
				g.DrawImage(this.IMAGE_LS_LOGO1, (this.mWidth - this.IMAGE_LS_LOGO1.mWidth) / 2, (this.mHeight - this.IMAGE_LS_LOGO1.mHeight) / 2);
				return;
			}
			if (this.mState < 2)
			{
				g.SetColor(SexyColor.Black);
				g.FillRect(GlobalMembers.gSexyApp.mScreenBounds);
			}
			float num = 255f - 255f * this.mExtraProgress;
			int num2 = ((num < (float)ZumasRevenge.Common._M(128)) ? ((int)num) : ZumasRevenge.Common._M1(128));
			if (this.mState >= 2 && !this.mFadeToMainMenu)
			{
				g.SetColorizeImages(true);
				int num3 = (int)((float)ZumasRevenge.Common._M(51) + (255f - num) * ZumasRevenge.Common._M1(0.8f));
				g.SetColor(num3, num3, num3, 255);
				g.DrawImage(this.IMAGE_LS_HAPPYSKY_BKGRND, ZumasRevenge.Common._S(0), 0, GameApp.gApp.GetScreenRect().mWidth, this.IMAGE_LS_HAPPYSKY_BKGRND.GetHeight());
				g.SetColorizeImages(false);
				if (num > 0f)
				{
					g.PushState();
					g.PopState();
				}
				for (int i = 0; i < 3; i++)
				{
					if (i == 1)
					{
						g.DrawImage(this.IMAGE_LS_HAPPYSKY_LAVA, ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_LS_HAPPYSKY_LAVA) - this.mLoadingXOffset), ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_LS_HAPPYSKY_LAVA)));
						g.SetColor(255, 255, 255, (int)this.mLavaAlpha);
						g.SetDrawMode(1);
						g.SetColorizeImages(true);
						g.DrawImage(this.IMAGE_LS_HAPPYSKY_LAVA, ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_LS_HAPPYSKY_LAVA) - this.mLoadingXOffset), ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_LS_HAPPYSKY_LAVA)));
						g.SetColorizeImages(false);
						g.SetDrawMode(0);
					}
					ResID id = ResID.IMAGE_LS_HAPPYSKY_ISLAND3 - i;
					int theX = ZumasRevenge.Common._DS(Res.GetOffsetXByID(id) - this.mLoadingXOffset);
					int theY = ZumasRevenge.Common._DS(Res.GetOffsetYByID(id));
					g.PushState();
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, 255 - (int)num);
					if (num < 255f)
					{
						g.DrawImage(Res.GetImageByID(id), theX, theY);
					}
					g.PopState();
				}
				if (this.mLoadingComplete && num != 255f)
				{
					this.mEffectBatch.DrawBatch(g);
				}
				g.PushState();
				if (this.mDarkIslandAlpha < 255)
				{
					g.SetColorizeImages(true);
				}
				g.SetColor(255, 255, 255, this.mDarkIslandAlpha);
				g.PopState();
				if (num != 255f)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)num);
				}
				g.DrawImage(this.IMAGE_LS_CENTER_CLOUD, (int)((float)GameApp.gApp.mWidth - (float)this.IMAGE_LS_CENTER_CLOUD.mWidth * 2f) / 2, ZumasRevenge.Common._DS(ZumasRevenge.Common._M(240)), (int)((float)this.IMAGE_LS_CENTER_CLOUD.GetWidth() * 2f), (int)((float)this.IMAGE_LS_CENTER_CLOUD.GetHeight() * 2f));
				g.SetColorizeImages(false);
				if (num < 255f && this.mLoadingComplete)
				{
					if (255f - num > 0f)
					{
						g.SetColorizeImages(true);
						g.SetColor(255, 255, 255, (int)(255f - num));
					}
					for (int j = 3; j >= 0; j--)
					{
						ResID id2 = ResID.IMAGE_LS_CALM_WAVE_1 + j;
						Image imageByID = Res.GetImageByID(id2);
						LoadingWave loadingWave = this.mCalmWaves[j];
						float num4 = (float)((this.mWidth - imageByID.mWidth) / 2) + ZumasRevenge.Common._DS(loadingWave.mRadius * (float)Math.Cos((double)loadingWave.mAngle)) + loadingWave.mXOff;
						float num5 = loadingWave.mY - ZumasRevenge.Common._DS(loadingWave.mRadius * (float)Math.Sin((double)loadingWave.mAngle));
						g.DrawImage(imageByID, (int)num4, (int)num5);
					}
					g.SetColorizeImages(false);
				}
				for (int k = 2; k >= 0; k--)
				{
					if (k == this.mFrogWave)
					{
						int num6 = 255;
						if (this.mLoadingComplete && this.mLoadingCompleteTime >= 200)
						{
							num6 = 255 - (this.mLoadingCompleteTime - 200);
						}
						if (num6 < 0)
						{
							num6 = 0;
						}
						if (num6 > 0)
						{
							if (this.mLoadingComplete)
							{
								this.mCloudUpdateCount++;
							}
							for (int l = 2; l >= 0; l--)
							{
								int mWidth = this.mWidth;
								float mStartX = this.mClouds[l].mStartX;
								float value = this.mClouds[l].mStartX + (float)((l != 1) ? (-1) : 1) * this.mClouds[l].mVX * (float)this.mCloudUpdateCount;
								this.DrawLightning(g, (int)ZumasRevenge.Common._DS(value), l);
								g.PushState();
								g.SetColorizeImages(true);
								g.SetColor(255, 255, 255, num6);
								Image imageByID2 = Res.GetImageByID(ResID.IMAGE_LS_CLOUD1A + l * 3);
								int num7 = 2;
								g.DrawImage(imageByID2, (int)ZumasRevenge.Common._DS(value), (int)ZumasRevenge.Common._DS(this.mClouds[l].mY), imageByID2.GetWidth() * num7, imageByID2.GetHeight() * num7);
								g.PopState();
							}
						}
						float num8;
						if (this.mFrogWave == 0)
						{
							num8 = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(200)) + this.mFrogPct * (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(1200));
						}
						else if (this.mFrogWave % 2 == 1)
						{
							num8 = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(2000)) - this.mFrogPct * (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(2500));
						}
						else
						{
							num8 = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(-700)) + this.mFrogPct * (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(2800));
						}
						this.mGlobalTransform.Reset();
						if (g.Is3D())
						{
							float num9 = 0f;
							float num10 = 0f;
							this.mGlobalTransform.Translate(-num9, -num10);
							if (g.Is3D())
							{
								this.mGlobalTransform.Scale(this.mFrogScale, this.mFrogScale);
								this.mGlobalTransform.RotateRad(this.mFrogAngle);
							}
							this.mGlobalTransform.Translate(num9, num10);
						}
						float num11 = ZumasRevenge.Common._DS(this.mWaves[this.mFrogWave].mY - (float)this.mFrogYOffs[this.mFrogWave] + (float)ZumasRevenge.Common._M(70) - this.mWaves[this.mFrogWave].mRadius / this.mFrogAngleDivisor * (float)Math.Sin((double)this.mWaves[this.mFrogWave].mAngle));
						num8 -= (float)this.mFrogXOffset;
						num11 -= (float)this.mFrogYOffset;
						double num12 = this.mPantaloonFlopPct;
						num12 *= 18.0;
						ResID[] array = new ResID[]
						{
							ResID.IMAGE_LS_RAFTANIM_RAFT,
							ResID.IMAGE_LS_RAFTANIM_PANTS01 + (int)num12,
							ResID.IMAGE_LS_RAFTANIM_FROG
						};
						for (int m = 0; m < 3; m++)
						{
							ResID resID = array[m];
							if (resID == ResID.IMAGE_LS_RAFTANIM_PANTS01 && GlobalMembers.gIs3D)
							{
								Graphics3D graphics3D = g.Get3D();
								SexyVertex2D[] array2 = new SexyVertex2D[20];
								for (int n = 0; n < 10; n++)
								{
									float num13 = (float)(Math.Sin((double)(this.mRippleCnt * ZumasRevenge.Common._M(0.35f) + (float)n * ZumasRevenge.Common._M1(0.75f))) * (double)ZumasRevenge.Common._S(ZumasRevenge.Common._M2(1.2f)) * (double)n / 9.0 * this.mPantalookRipplePct);
									float theY2 = (float)((double)ZumasRevenge.Common._S(ZumasRevenge.Common._M(10)) + Math.Sin((double)this.mFrogAngle) * (double)ZumasRevenge.Common._S(ZumasRevenge.Common._M1(-50)) + (double)num13 + Math.Sin((double)this.mFrogAngle) * (double)ZumasRevenge.Common._S(ZumasRevenge.Common._M2(-80)) * (double)n / 9.0);
									float num14 = (float)((double)this.IMAGE_LS_RAFTANIM_PANTS01.mWidth + Math.Sin((double)this.mFrogAngle) * (double)ZumasRevenge.Common._S(ZumasRevenge.Common._M(10)) + Math.Sin((double)(this.mRippleCnt * ZumasRevenge.Common._M1(0.2f))) * (double)ZumasRevenge.Common._S(ZumasRevenge.Common._M2(1.2f)) * this.mPantalookRipplePct);
									float num15 = (float)((double)this.IMAGE_LS_RAFTANIM_PANTS01.mWidth + Math.Sin((double)this.mFrogAngle) * (double)ZumasRevenge.Common._S(ZumasRevenge.Common._M(80)) + Math.Sin((double)(this.mRippleCnt * ZumasRevenge.Common._M1(0.2f))) * (double)ZumasRevenge.Common._S(ZumasRevenge.Common._M2(1.2f)) * this.mPantalookRipplePct);
									float num16 = (float)((double)this.IMAGE_LS_RAFTANIM_PANTS01.mHeight + Math.Max(0.0, Math.Sin((double)this.mFrogAngle) * (double)ZumasRevenge.Common._S(ZumasRevenge.Common._M(-70))) * (double)n / 9.0);
									float theY3 = (float)((double)ZumasRevenge.Common._S(ZumasRevenge.Common._M(10)) + Math.Sin((double)this.mFrogAngle) * (double)ZumasRevenge.Common._S(ZumasRevenge.Common._M1(-50)) + (double)Math.Max(0f, num13 - ZumasRevenge.Common._M2(0.1f)) + (double)num16 + Math.Sin((double)this.mFrogAngle) * (double)ZumasRevenge.Common._S(ZumasRevenge.Common._M3(-80)) * (double)n / 9.0);
									array2[n * 2] = new SexyVertex2D((float)((double)ZumasRevenge.Common._S(ZumasRevenge.Common._M(47)) + Math.Sin((double)this.mFrogAngle) * (double)ZumasRevenge.Common._S(ZumasRevenge.Common._M1(20)) + (double)(num14 * (float)n / 9f) + (double)g.mTransX), theY2, (float)n / 9f, 0f);
									array2[n * 2 + 1] = new SexyVertex2D((float)((double)ZumasRevenge.Common._S(ZumasRevenge.Common._M(47)) + Math.Sin((double)this.mFrogAngle) * (double)ZumasRevenge.Common._S(ZumasRevenge.Common._M1(85)) + (double)(num15 * (float)n / 9f) + (double)g.mTransX), theY3, (float)n / 9f, 1f);
								}
								graphics3D.SetTexture(0, this.IMAGE_LS_RAFTANIM_PANTS01);
								graphics3D.DrawPrimitive(0U, Graphics3D.EPrimitiveType.PT_TriangleStrip, array2, ZumasRevenge.Common._M(18), SexyColor.White, 0, num8, num11, true, 0U);
							}
							else
							{
								Image imageByID3 = Res.GetImageByID(resID);
								this.mGlobalTransform.Reset();
								this.mGlobalTransform.Translate((float)(ZumasRevenge.Common._DS(Res.GetOffsetXByID(resID)) + imageByID3.mWidth / 2), (float)(ZumasRevenge.Common._DS(Res.GetOffsetYByID(resID)) + imageByID3.mHeight / 2));
								this.mGlobalTransform.Translate((float)(-(float)this.mCenterOffX), (float)(-(float)this.mCenterOffY));
								if (GlobalMembers.gIs3D)
								{
									this.mGlobalTransform.Scale(this.mFrogScale, this.mFrogScale);
									this.mGlobalTransform.RotateRad(this.mFrogAngle);
								}
								this.mGlobalTransform.Translate((float)this.mCenterOffX, (float)this.mCenterOffY);
								if (GlobalMembers.gIs3D)
								{
									g.DrawImageTransformF(imageByID3, this.mGlobalTransform, num8, num11);
								}
								else
								{
									g.DrawImageTransform(imageByID3, this.mGlobalTransform, (float)((int)num8), (float)((int)num11));
								}
							}
						}
					}
					if ((int)num > 0)
					{
						if (num < 255f)
						{
							g.SetColorizeImages(true);
							g.SetColor(255, 255, 255, (int)num);
						}
						Image imageByID4 = Res.GetImageByID(ResID.IMAGE_LS_WAVE1 + k);
						LoadingWave loadingWave2 = this.mWaves[k];
						float num17 = (float)((this.mWidth - imageByID4.mWidth * this.mWaveImgResScale) / 2) + ZumasRevenge.Common._DS(loadingWave2.mRadius * (float)Math.Cos((double)loadingWave2.mAngle));
						g.DrawImage(imageByID4, (int)num17, (int)ZumasRevenge.Common._DS(loadingWave2.mY - loadingWave2.mRadius * (float)Math.Sin((double)loadingWave2.mAngle)), imageByID4.GetWidth() * this.mWaveImgResScale, imageByID4.GetHeight() * this.mWaveImgResScale);
						g.SetColorizeImages(false);
					}
				}
				if (num2 > 0)
				{
					for (int num18 = 2; num18 >= 0; num18--)
					{
						int num19 = (int)((float)this.mWidth - this.mClouds[num18].mStartX);
						float mStartX2 = this.mClouds[num18].mStartX;
						float num20 = (float)((num18 != 1) ? (-1) : 1) * this.mExtraProgress / 3f;
						g.PushState();
						g.SetColorizeImages(true);
						g.SetColor(255, 255, 255, num2);
						Image imageByID5 = Res.GetImageByID(ResID.IMAGE_LS_CLOUD1C + num18 * 3);
						g.DrawImage(imageByID5, (int)ZumasRevenge.Common._DS(this.mClouds[num18].mStartX + this.mExtraProgress / 3f / (float)(num18 + 1) * (float)ZumasRevenge.Common._M(2000) + this.mClouds[num18].mShadowOffset), (int)ZumasRevenge.Common._DS(this.mClouds[num18].mShadowY), imageByID5.GetWidth() * 10, imageByID5.GetHeight() * 10);
						g.PopState();
					}
				}
				bool flag = this.mLoadingComplete;
			}
			if (!this.mFadeToMainMenu)
			{
				if (this.mState == 0)
				{
					if (Enumerable.Count<PartnerLogo>(this.mPartnerLogos) == 0)
					{
						g.SetColorizeImages(true);
						g.SetColor(255, 255, 255, 255);
						g.DrawImage(this.IMAGE_LS_LOGO1, (this.mWidth - this.IMAGE_LS_LOGO1.mWidth) / 2, (this.mHeight - this.IMAGE_LS_LOGO1.mHeight) / 2);
						g.SetColorizeImages(false);
					}
					else
					{
						g.PushState();
						PartnerLogo partnerLogo = this.mPartnerLogos[0];
						if (partnerLogo.mAlpha != 255)
						{
							g.SetColorizeImages(true);
						}
						g.SetColor(255, 255, 255, partnerLogo.mAlpha);
						g.DrawImage(partnerLogo.mImage, (this.mWidth - partnerLogo.mImage.mWidth) / 2, (this.mHeight - partnerLogo.mImage.mHeight) / 2);
						g.PopState();
					}
				}
				else if (this.mState == 1)
				{
					Image imageByID6 = Res.GetImageByID(ResID.IMAGE_LS_LOGO1 + this.mLightningFrame);
					if (this.mLightningOn)
					{
						g.DrawImage(imageByID6, this.pts[this.mLightningFrame].mX, (this.mLightningFrame == 0) ? this.pts[this.mLightningFrame].mY : 0);
					}
					for (int num21 = 0; num21 < Enumerable.Count<LogoLightning>(this.mLogoLightning); num21++)
					{
						LogoLightning logoLightning = this.mLogoLightning[num21];
						g.SetColor(255, 255, 255, (int)(255f * ((float)logoLightning.mTimer / (float)logoLightning.mTimerTarget)));
						g.SetColorizeImages(true);
						g.DrawImage(logoLightning.mImage, (this.mWidth - logoLightning.mImage.mWidth * 2) / 2, 0, logoLightning.mImage.GetWidth() * 2, logoLightning.mImage.GetHeight() * 2);
						g.SetColorizeImages(false);
					}
				}
			}
			if (this.mLoadingBarAlpha > 0f && (!this.mFadeToMainMenu || this.mBlackFadeIn))
			{
				if (this.mLoadingBarAlpha < 255f)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)this.mLoadingBarAlpha);
				}
				g.DrawImage(this.IMAGE_LS_BACKING, ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_LS_BACKING)), this.mLoadingYOffset + ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_LS_BACKING)), GameApp.gApp.GetScreenRect().mWidth, this.IMAGE_LS_BACKING.GetHeight());
				if (this.mCompleteLoadingBarAlpha < 255f)
				{
					float num22 = (float)GlobalMembers.gSexyApp.GetLoadingThreadProgress();
					int num23 = (int)((float)this.IMAGE_LS_REDLOADINGBAR.mWidth * num22);
					if (num23 > this.IMAGE_LS_REDLOADINGBAR.mWidth)
					{
						num23 = this.IMAGE_LS_REDLOADINGBAR.mWidth;
					}
					Rect theSrcRect = new Rect(this.IMAGE_LS_REDLOADINGBAR.mWidth - num23, 0, num23, this.IMAGE_LS_REDLOADINGBAR.mHeight);
					int num24 = ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_LS_REDLOADINGBAR) - this.mLoadingXOffset);
					g.DrawImage(this.IMAGE_LS_REDLOADINGBAR, num24, this.mLoadingYOffset + ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_LS_REDLOADINGBAR)), theSrcRect);
					this.mLoadStarRotateAngle += -0.1f;
					g.DrawImageRotated(this.IMAGE_LS_STARFISH, num24 + num23 - ZumasRevenge.Common._DS(ZumasRevenge.Common._M(40)), this.mLoadingYOffset + ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_LS_STARFISH)), (double)this.mLoadStarRotateAngle);
				}
				g.SetColorizeImages(false);
				if (this.mCompleteLoadingBarAlpha > 0f)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)this.mCompleteLoadingBarAlpha);
					g.DrawImage(this.IMAGE_LS_GREENLOADEDBAR, ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_LS_GREENLOADEDBAR) - this.mLoadingXOffset), this.mLoadingYOffset + ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_LS_GREENLOADEDBAR)));
					if (this.mCompleteLoadingBarAlpha >= 255f)
					{
						int alpha = 127 + JeffLib.Common.GetAlphaFromUpdateCount(this.mUpdateCnt, ZumasRevenge.Common._M(128));
						g.SetColor(255, 255, 255, alpha);
					}
					g.SetColorizeImages(false);
				}
				if (this.mLoadingBarAlpha < 255f)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)this.mLoadingBarAlpha);
				}
				g.DrawImage(this.IMAGE_LS_BAR, ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_LS_BAR) - this.mLoadingXOffset), this.mLoadingYOffset + ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_LS_BAR)));
				if (GameApp.gApp.GetLoadingThreadProgress() < (double)ZumasRevenge.Common._M(0.06f))
				{
					g.DrawImage(this.IMAGE_LS_L_TIKI01, ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_LS_L_TIKI01) - this.mLoadingXOffset), this.mLoadingYOffset + ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_LS_L_TIKI01)));
				}
				else
				{
					g.DrawImage(this.IMAGE_LS_L_TIKI02, ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_LS_L_TIKI02) - this.mLoadingXOffset), this.mLoadingYOffset + ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_LS_L_TIKI02)));
				}
				if (GameApp.gApp.GetLoadingThreadProgress() < (double)ZumasRevenge.Common._M(0.95f))
				{
					g.DrawImage(this.IMAGE_LS_R_TIKI01, ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_LS_R_TIKI01) - this.mLoadingXOffset), this.mLoadingYOffset + ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_LS_R_TIKI01)));
				}
				else
				{
					g.DrawImage(this.IMAGE_LS_R_TIKI02, ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_LS_R_TIKI02) - this.mLoadingXOffset), this.mLoadingYOffset + ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_LS_R_TIKI02)));
				}
				g.SetColorizeImages(false);
				if (this.mCompleteLoadingBarAlpha < 255f)
				{
					g.PushState();
					g.SetColorizeImages(true);
					int alpha2;
					if (this.mLoadingBarAlpha < 255f)
					{
						alpha2 = (int)this.mLoadingBarAlpha;
					}
					else if (this.mCompleteLoadingBarAlpha > 0f)
					{
						alpha2 = 255 - (int)this.mCompleteLoadingBarAlpha;
					}
					else
					{
						alpha2 = 127 + JeffLib.Common.GetAlphaFromUpdateCount(this.mUpdateCnt, ZumasRevenge.Common._M(128));
					}
					g.SetColor(200, 200, 200, alpha2);
					Font fontByID = Res.GetFontByID(ResID.FONT_SHAGLOUNGE28_STROKE);
					g.SetFont(fontByID);
					string theLine;
					if (this.mFirstRun)
					{
						theLine = this.mLoadingTextContainer.GetBackStoryText()[this.mLoadingTextIdx];
					}
					else
					{
						theLine = this.mLoadingTextContainer.GetLoadingText()[this.mLoadingTextIdx];
					}
					Rect theRect = this.mLoadingTextFrame;
					theRect.mY = 368;
					theRect.mX = 110;
					g.SetScale(1.5f, 1.5f, g.mScaleOrigX, g.mScaleOrigY);
					g.WriteWordWrapped(theRect, theLine, -1, 0);
					g.PopState();
				}
				if (this.mCompleteLoadingBarAlpha > 0f)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)this.mCompleteLoadingBarAlpha);
					int num25 = 369;
					if (Localization.GetCurrentLanguage() != Localization.LanguageType.Language_EN)
					{
						num25 = this.IMAGE_LS_GREENLOADEDBAR.GetWidth() - 51;
					}
					int num26 = 30;
					int num27 = (num25 - this.IMAGE_LS_CLICKTXT.GetWidth()) / 2;
					int num28 = (num26 - this.IMAGE_LS_CLICKTXT.GetHeight()) / 2;
					int num29 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(605));
					if (Localization.GetCurrentLanguage() != Localization.LanguageType.Language_EN)
					{
						num29 = ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_LS_GREENLOADEDBAR)) + 51;
					}
					int num30 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(1042));
					g.DrawImage(this.IMAGE_LS_CLICKTXT, num29 + num27, num30 + num28);
					g.SetColorizeImages(false);
				}
			}
			if (this.mFlashAlpha > 0f)
			{
				g.SetColor(255, 255, 255, (int)this.mFlashAlpha);
				g.FillRect(GlobalMembers.gSexyApp.mScreenBounds);
			}
			base.DeferOverlay(20);
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00092EFB File Offset: 0x000910FB
		public void ButtonPress(int id)
		{
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00092EFD File Offset: 0x000910FD
		public void ButtonPress(int theId, int theClickCount)
		{
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00092EFF File Offset: 0x000910FF
		public void ButtonDepress(int theId)
		{
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00092F01 File Offset: 0x00091101
		public void ButtonDownTick(int theId)
		{
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00092F03 File Offset: 0x00091103
		public void ButtonMouseEnter(int theId)
		{
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00092F05 File Offset: 0x00091105
		public void ButtonMouseLeave(int theId)
		{
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00092F07 File Offset: 0x00091107
		public void ButtonMouseMove(int theId, int theX, int theY)
		{
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00092F0C File Offset: 0x0009110C
		public void ProcessHardwareBackButton()
		{
			if (this.mLockBGM)
			{
				Dialog dialog = GameApp.gApp.GetDialog(1);
				dialog.ButtonDepress(1001);
				GameApp.gApp.OnHardwareBackButtonPressProcessed();
				return;
			}
			if (this.mState <= 1)
			{
				GameApp.gApp.Shutdown();
				GameApp.gApp.OnHardwareBackButtonPressProcessed();
				return;
			}
			if (!this.mLoadingComplete)
			{
				GameApp.gApp.OnHardwareBackButtonPressProcessed();
				return;
			}
			if (GameApp.gApp.GetDialog(1) != null)
			{
				GameApp.gApp.GetDialog(1).ButtonDepress(1001);
				GameApp.gApp.OnHardwareBackButtonPressProcessed();
				return;
			}
			GameApp.gApp.DoQuitPromptDialog();
			GameApp.gApp.mYesNoDialogDelegate = new GameApp.YesNoDialogDelegate(this.ProcessYesNo);
			GameApp.gApp.OnHardwareBackButtonPressProcessed();
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00092FCC File Offset: 0x000911CC
		public void ProcessYesNo(int theId)
		{
			if (theId == 1000)
			{
				if (!GameApp.gApp.IsRegistered() && GameApp.gApp.mTrialType == 1 && GameApp.gApp.GetBoolean("UpsellExit", false))
				{
					GameApp.gApp.DoUpsell(true);
					return;
				}
				GameApp.gApp.Shutdown();
			}
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x00093024 File Offset: 0x00091224
		public void ProcessBGM()
		{
			string @string = TextManager.getInstance().getString(58);
			int width_pad = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(20));
			GameApp.gApp.DoYesNoDialog(TextManager.getInstance().getString(58), @string, true, TextManager.getInstance().getString(446), TextManager.getInstance().getString(447), false, ZumasRevenge.Common._S(ZumasRevenge.Common._M(50)), 1, width_pad);
			GameApp.gApp.mYesNoDialogDelegate = new GameApp.YesNoDialogDelegate(this.ProcessBGMlock);
			this.mLockBGM = true;
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x000930AE File Offset: 0x000912AE
		public void ProcessBGMlock(int theId)
		{
			this.mLockBGM = false;
			if (theId == 1000)
			{
				GameApp.gApp.mMusicInterface.stopUserMusic();
			}
		}

		// Token: 0x04000D51 RID: 3409
		protected const float mCenterCloudImageScale = 2f;

		// Token: 0x04000D52 RID: 3410
		protected long updateTimes;

		// Token: 0x04000D53 RID: 3411
		protected long drawTimes;

		// Token: 0x04000D54 RID: 3412
		public static int MAX_VOLCANO_PROJECTILES = 4;

		// Token: 0x04000D55 RID: 3413
		private static float max_angle = 0.08726f;

		// Token: 0x04000D56 RID: 3414
		private static int LOADING_TEXT_TIME = 200;

		// Token: 0x04000D57 RID: 3415
		protected List<PartnerLogo> mPartnerLogos = new List<PartnerLogo>();

		// Token: 0x04000D58 RID: 3416
		protected float mLoadStarRotateAngle;

		// Token: 0x04000D59 RID: 3417
		protected float mLavaAlpha;

		// Token: 0x04000D5A RID: 3418
		protected bool mIncLavaAlpha;

		// Token: 0x04000D5B RID: 3419
		protected List<LogoLightning> mLogoLightning = new List<LogoLightning>();

		// Token: 0x04000D5C RID: 3420
		protected LoadingWave[] mWaves = new LoadingWave[5];

		// Token: 0x04000D5D RID: 3421
		protected LoadingWave[] mCalmWaves = new LoadingWave[4];

		// Token: 0x04000D5E RID: 3422
		protected LoadingCloud[] mClouds = new LoadingCloud[3];

		// Token: 0x04000D5F RID: 3423
		protected float mLoadingBarAlpha;

		// Token: 0x04000D60 RID: 3424
		protected float mCompleteLoadingBarAlpha;

		// Token: 0x04000D61 RID: 3425
		protected float mFlashAlpha;

		// Token: 0x04000D62 RID: 3426
		protected float mExtraProgress;

		// Token: 0x04000D63 RID: 3427
		protected float mFrogAngle;

		// Token: 0x04000D64 RID: 3428
		protected float mFrogAngleDivisor;

		// Token: 0x04000D65 RID: 3429
		protected float mFrogAngleDelta;

		// Token: 0x04000D66 RID: 3430
		protected float mZumaY;

		// Token: 0x04000D67 RID: 3431
		protected float mRevengeY;

		// Token: 0x04000D68 RID: 3432
		protected float mRevengeStretch;

		// Token: 0x04000D69 RID: 3433
		protected float mBlackFadeAlpha;

		// Token: 0x04000D6A RID: 3434
		protected bool mBlackFadeIn;

		// Token: 0x04000D6B RID: 3435
		protected bool mCanShowMenu;

		// Token: 0x04000D6C RID: 3436
		protected bool mFrogPitchForward;

		// Token: 0x04000D6D RID: 3437
		protected bool mHasShown;

		// Token: 0x04000D6E RID: 3438
		protected bool mLightningOn;

		// Token: 0x04000D6F RID: 3439
		protected bool mLoadingComplete;

		// Token: 0x04000D70 RID: 3440
		protected bool mFadeToMainMenu;

		// Token: 0x04000D71 RID: 3441
		protected bool mFirstRun;

		// Token: 0x04000D72 RID: 3442
		protected int mLightningTimer;

		// Token: 0x04000D73 RID: 3443
		protected int mLightningFrame;

		// Token: 0x04000D74 RID: 3444
		protected int mLogoHoldTime;

		// Token: 0x04000D75 RID: 3445
		protected int mState;

		// Token: 0x04000D76 RID: 3446
		protected int mFrogWave;

		// Token: 0x04000D77 RID: 3447
		protected int mStormTimer;

		// Token: 0x04000D78 RID: 3448
		protected int mClearTimer;

		// Token: 0x04000D79 RID: 3449
		protected int mLoadingCompleteDelay;

		// Token: 0x04000D7A RID: 3450
		protected float mFrogScale;

		// Token: 0x04000D7B RID: 3451
		protected float mFrogPct;

		// Token: 0x04000D7C RID: 3452
		protected int mDarkIslandAlpha;

		// Token: 0x04000D7D RID: 3453
		protected int mLoadingCompleteTime;

		// Token: 0x04000D7E RID: 3454
		protected int mCloudUpdateCount;

		// Token: 0x04000D7F RID: 3455
		protected CurvedVal mPantaloonFlopPct = new CurvedVal();

		// Token: 0x04000D80 RID: 3456
		protected CurvedVal mPantalookRipplePct = new CurvedVal();

		// Token: 0x04000D81 RID: 3457
		protected float mRippleCnt;

		// Token: 0x04000D82 RID: 3458
		protected int mLoadingOffset;

		// Token: 0x04000D83 RID: 3459
		protected int mLoadingTextIdx;

		// Token: 0x04000D84 RID: 3460
		protected int mLoadingTextTime;

		// Token: 0x04000D85 RID: 3461
		protected List<int> mSeenLoadingTextIndices = new List<int>();

		// Token: 0x04000D86 RID: 3462
		protected int mOffsetParticle = 85;

		// Token: 0x04000D87 RID: 3463
		protected bool mUserProfileLoaded;

		// Token: 0x04000D88 RID: 3464
		protected bool mLoading;

		// Token: 0x04000D89 RID: 3465
		protected PIEffect mLeftTorch;

		// Token: 0x04000D8A RID: 3466
		protected PIEffect mRightTorch;

		// Token: 0x04000D8B RID: 3467
		protected PIEffect mVolcanoSmoke;

		// Token: 0x04000D8C RID: 3468
		protected VolcanoProjectile[] mVolcanoProjectiles = new VolcanoProjectile[LoadingScreen.MAX_VOLCANO_PROJECTILES];

		// Token: 0x04000D8D RID: 3469
		public PIEffectBatch mEffectBatch;

		// Token: 0x04000D8E RID: 3470
		protected Transform mGlobalTransform = new Transform();

		// Token: 0x04000D8F RID: 3471
		protected int mLoadingXOffset;

		// Token: 0x04000D90 RID: 3472
		protected int mLoadingYOffset;

		// Token: 0x04000D91 RID: 3473
		protected Rect mLoadingTextFrame = default(Rect);

		// Token: 0x04000D92 RID: 3474
		protected Image IMAGE_LS_LOGO1 = Res.GetImageByID(ResID.IMAGE_LS_LOGO1);

		// Token: 0x04000D93 RID: 3475
		protected Image IMAGE_LS_LIGHT1_ID;

		// Token: 0x04000D94 RID: 3476
		protected Image IMAGE_LS_REDLOADINGBAR = Res.GetImageByID(ResID.IMAGE_LS_REDLOADINGBAR);

		// Token: 0x04000D95 RID: 3477
		protected Image IMAGE_LS_CLICKTXT = Res.GetImageByID(ResID.IMAGE_LS_CLICKTXT);

		// Token: 0x04000D96 RID: 3478
		protected Image IMAGE_LS_BACKING = Res.GetImageByID(ResID.IMAGE_LS_BACKING);

		// Token: 0x04000D97 RID: 3479
		protected Image IMAGE_LS_STARFISH = Res.GetImageByID(ResID.IMAGE_LS_STARFISH);

		// Token: 0x04000D98 RID: 3480
		protected Image IMAGE_LS_R_TIKI02 = Res.GetImageByID(ResID.IMAGE_LS_R_TIKI02);

		// Token: 0x04000D99 RID: 3481
		protected Image IMAGE_LS_R_TIKI01 = Res.GetImageByID(ResID.IMAGE_LS_R_TIKI01);

		// Token: 0x04000D9A RID: 3482
		protected Image IMAGE_LS_L_TIKI02 = Res.GetImageByID(ResID.IMAGE_LS_L_TIKI02);

		// Token: 0x04000D9B RID: 3483
		protected Image IMAGE_LS_L_TIKI01 = Res.GetImageByID(ResID.IMAGE_LS_L_TIKI01);

		// Token: 0x04000D9C RID: 3484
		protected Image IMAGE_LS_GREENLOADEDBAR = Res.GetImageByID(ResID.IMAGE_LS_GREENLOADEDBAR);

		// Token: 0x04000D9D RID: 3485
		protected Image IMAGE_LS_BAR = Res.GetImageByID(ResID.IMAGE_LS_BAR);

		// Token: 0x04000D9E RID: 3486
		protected Image IMAGE_LS_HAPPYSKY_BKGRND = Res.GetImageByID(ResID.IMAGE_LS_HAPPYSKY_BKGRND);

		// Token: 0x04000D9F RID: 3487
		protected Image IMAGE_LS_HAPPYSKY_LAVA = Res.GetImageByID(ResID.IMAGE_LS_HAPPYSKY_LAVA);

		// Token: 0x04000DA0 RID: 3488
		protected Image IMAGE_LS_CENTER_CLOUD = Res.GetImageByID(ResID.IMAGE_LS_CENTER_CLOUD);

		// Token: 0x04000DA1 RID: 3489
		protected Image IMAGE_LS_RAFTANIM_PANTS01 = Res.GetImageByID(ResID.IMAGE_LS_RAFTANIM_PANTS01);

		// Token: 0x04000DA2 RID: 3490
		protected SexyPoint[] pts = new SexyPoint[5];

		// Token: 0x04000DA3 RID: 3491
		protected int mCenterOffX;

		// Token: 0x04000DA4 RID: 3492
		protected int mCenterOffY;

		// Token: 0x04000DA5 RID: 3493
		protected int mFrogXOffset = ZumasRevenge.Common._S(ZumasRevenge.Common._M(100));

		// Token: 0x04000DA6 RID: 3494
		protected int mFrogYOffset = ZumasRevenge.Common._S(ZumasRevenge.Common._M(120));

		// Token: 0x04000DA7 RID: 3495
		protected int mWaveImgResScale = 2;

		// Token: 0x04000DA8 RID: 3496
		public bool mLockBGM;

		// Token: 0x04000DA9 RID: 3497
		protected int[] mFrogYOffs = new int[]
		{
			ZumasRevenge.Common._DS(ZumasRevenge.Common._M(100)),
			ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(20)),
			ZumasRevenge.Common._DS(ZumasRevenge.Common._M2(-40)),
			ZumasRevenge.Common._DS(ZumasRevenge.Common._M3(0)),
			ZumasRevenge.Common._DS(ZumasRevenge.Common._M4(60))
		};

		// Token: 0x04000DAA RID: 3498
		private LoadingScreen.LoadingTextContainer mLoadingTextContainer = new LoadingScreen.LoadingTextContainer();

		// Token: 0x04000DAB RID: 3499
		public bool mWaitingForConfirmation;

		// Token: 0x04000DAC RID: 3500
		private ButtonWidget mHelpButton;

		// Token: 0x04000DAD RID: 3501
		private ButtonWidget mStartButton;

		// Token: 0x02000111 RID: 273
		internal static class RandomNumbers
		{
			// Token: 0x06000E41 RID: 3649 RVA: 0x000930EA File Offset: 0x000912EA
			internal static int NextNumber()
			{
				if (LoadingScreen.RandomNumbers.r == null)
				{
					LoadingScreen.RandomNumbers.Seed();
				}
				return LoadingScreen.RandomNumbers.r.Next();
			}

			// Token: 0x06000E42 RID: 3650 RVA: 0x00093102 File Offset: 0x00091302
			internal static int NextNumber(int ceiling)
			{
				if (LoadingScreen.RandomNumbers.r == null)
				{
					LoadingScreen.RandomNumbers.Seed();
				}
				return LoadingScreen.RandomNumbers.r.Next(ceiling);
			}

			// Token: 0x06000E43 RID: 3651 RVA: 0x0009311B File Offset: 0x0009131B
			internal static void Seed()
			{
				LoadingScreen.RandomNumbers.r = new Random();
			}

			// Token: 0x06000E44 RID: 3652 RVA: 0x00093127 File Offset: 0x00091327
			internal static void Seed(int seed)
			{
				LoadingScreen.RandomNumbers.r = new Random(seed);
			}

			// Token: 0x04000DAE RID: 3502
			private static Random r;
		}

		// Token: 0x02000112 RID: 274
		private enum State
		{
			// Token: 0x04000DB0 RID: 3504
			State_LogoIntro,
			// Token: 0x04000DB1 RID: 3505
			State_Lightning,
			// Token: 0x04000DB2 RID: 3506
			State_Loading,
			// Token: 0x04000DB3 RID: 3507
			State_Final
		}

		// Token: 0x02000113 RID: 275
		internal class LoadingTextContainer
		{
			// Token: 0x06000E45 RID: 3653 RVA: 0x00093134 File Offset: 0x00091334
			private static string _(string s)
			{
				return s;
			}

			// Token: 0x06000E46 RID: 3654 RVA: 0x00093138 File Offset: 0x00091338
			public LoadingTextContainer()
			{
				int num = 582;
				int num2 = 29;
				for (int i = num; i < num + num2; i++)
				{
					this.mLoadingText.Add(TextManager.getInstance().getString(i));
				}
				num += num2;
				num2 = 3;
				for (int j = num; j < num + num2; j++)
				{
					this.mBackStoryText.Add(TextManager.getInstance().getString(j));
				}
			}

			// Token: 0x06000E47 RID: 3655 RVA: 0x000931B8 File Offset: 0x000913B8
			public List<string> GetLoadingText()
			{
				return this.mLoadingText;
			}

			// Token: 0x06000E48 RID: 3656 RVA: 0x000931C0 File Offset: 0x000913C0
			public List<string> GetBackStoryText()
			{
				return this.mBackStoryText;
			}

			// Token: 0x04000DB4 RID: 3508
			private List<string> mLoadingText = new List<string>();

			// Token: 0x04000DB5 RID: 3509
			private List<string> mBackStoryText = new List<string>();
		}
	}
}
