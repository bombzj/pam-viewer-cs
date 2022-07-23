using System;
using System.Collections.Generic;
using System.Linq;
using JeffLib;
using SexyFramework;
using SexyFramework.AELib;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.Resource;

namespace ZumasRevenge
{
	// Token: 0x02000101 RID: 257
	public class Level : IDisposable
	{
		// Token: 0x06000D29 RID: 3369 RVA: 0x00080044 File Offset: 0x0007E244
		protected void InitFinalBossLevel()
		{
			if (!this.mApp.mResourceManager.IsGroupLoaded("CloakedBoss") && !this.mApp.mResourceManager.LoadResources("CloakedBoss"))
			{
				this.mApp.ShowResourceError(true);
				this.mApp.Shutdown();
			}
			this.mDoTorchCrap = true;
			this.mBoard.mPreventBallAdvancement = true;
			this.mTorchTextAlpha = 700f;
			this.mTorchStageState = 0;
			this.mTorchStageTimer = ZumasRevenge.Common._M(150);
			this.mTorchDaisScale = 1f;
			this.mTorchCompMgr = this.mApp.LoadComposition("pax\\cloakedboss", "_BOSSES");
			Composition composition = this.mTorchCompMgr.GetComposition("squish");
			this.mTorchBossX = (float)(-(float)ZumasRevenge.Common._DS(composition.mWidth) - ZumasRevenge.Common._DS(ZumasRevenge.Common._M(500)));
			this.mTorchBossY = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(-920));
			this.mTorchBossDestX = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(-520));
			this.mTorchBossDestY = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(-462));
			int num = ZumasRevenge.Common._M(50);
			this.mTorchBossVX = (this.mTorchBossDestX - this.mTorchBossX) / (float)num;
			this.mTorchBossVY = (this.mTorchBossDestY - this.mTorchBossY) / (float)num;
			for (int i = 0; i < this.mTorches.Count; i++)
			{
				this.mTorches[i].mActive = (this.mTorches[i].mDraw = true);
			}
			for (int j = 0; j < 3; j++)
			{
				this.mCloakedBossTextAlpha[j] = 0f;
			}
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x000801F4 File Offset: 0x0007E3F4
		public Level()
		{
			this.mCurMultiplierTimeLeft = (this.mMaxMultiplierTime = 0);
			this.mTorchStageState = -1;
			this.mTorchBossX = (this.mTorchBossY = -1f);
			this.mTorchDaisScale = 1f;
			this.mTorchCompMgr = null;
			this.mTorchStageTimer = 0;
			this.mTorchBossVX = (this.mTorchBossVY = (this.mTorchBossDestX = (this.mTorchBossDestY = 0f)));
			this.mFrogFlyOff = null;
			this.mTorchStageShakeAmt = 0;
			this.mNumGauntletBallsBroke = 0;
			this.mBossBGID = "";
			this.mZumaPulseUCStart = 0;
			this.mCurGauntletMultPct = 0f;
			this.mChallengePoints = 100;
			this.mChallengeAcePoints = 1000;
			this.mTorchStageAlpha = 0f;
			this.mGauntletCurTime = 0;
			this.mCloakPoof = null;
			this.mCloakClapFrame = -1;
			this.mCanDrawBoss = true;
			this.mIndex = -1;
			this.mIronFrog = false;
			this.mStartingGauntletLevel = 1;
			this.mAllCurvesAtRolloutPoint = false;
			this.mHasReachedCruisingSpeed = false;
			this.mPotPct = 1f;
			this.mFrog = null;
			this.mUpdateCount = 0;
			this.mFireSpeed = 8f;
			this.mBGFromPSD = false;
			this.mCurBarSizeInc = 1;
			this.mEndSequence = -1;
			this.mDoTorchCrap = false;
			this.mHasDoneTorchCrap = false;
			this.mTorchTextAlpha = 0f;
			this.mReloadDelay = 0;
			this.mTreasureFreq = 300;
			this.mParTime = 0;
			this.mBoss = (this.mOrgBoss = null);
			this.mSecondaryBoss = null;
			for (int i = 0; i < 4; i++)
			{
				this.mCurveSkullAngleOverrides[i] = float.MaxValue;
			}
			this.mLoopAtEnd = false;
			this.mIsEndless = false;
			this.mInvertMouseTimer = (this.mMaxInvertMouseTimer = 0);
			this.mTimer = (this.mTimeToComplete = -1);
			this.mBossFreezePowerupTime = (this.mFrogShieldPowerupCount = 300);
			this.mSliderEdgeRotate = false;
			this.mTorchTimer = 0;
			this.mFinalLevel = false;
			this.mNoBackground = false;
			this.mFurthestBallDistance = 0;
			this.mOffscreenClearBonus = false;
			this.mIntroTorchDelay = 0;
			this.mIntroTorchIndex = -1;
			for (int j = 0; j < 5; j++)
			{
				this.mFrogImages[j] = new LillyPadImageInfo();
				this.mFrogImages[j].mImage = null;
			}
			this.mPostZumaTimeCounter = 0;
			this.mPostZumaTimeSlowInc = (this.mPostZumaTimeSpeedInc = 0f);
			this.mZone = (this.mNum = -1);
			this.mApp = GameApp.gApp;
			this.mBoard = ((this.mApp != null) ? this.mApp.GetBoard() : null);
			this.mHurryToRolloutAmt = 0f;
			this.mTempSpeedupTimer = 0;
			this.mSuckMode = false;
			this.mMoveType = 0;
			this.mMoveSpeed = 25;
			this.mNumFrogPoints = 0;
			this.mCurFrogPoint = 0;
			this.mFrogX[0] = 320;
			this.mFrogY[0] = 240;
			this.mDoingPadHints = false;
			this.mBarWidth = (this.mBarHeight = 0);
			this.mNoFlip = false;
			this.mHoleMgr = new HoleMgr();
			this.mDrawCurves = false;
			for (int k = 0; k < 4; k++)
			{
				this.mCurveMgr[k] = null;
			}
			this.mNumCurves = 0;
			this.mMirrorType = MirrorType.MirrorType_None;
			if (this.mApp != null)
			{
				this.mGingerMouthXStart = (float)this.mApp.GetWideScreenAdjusted(ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_INGAME_UI_LEFT_JAW)));
				this.mFredMouthXStart = (float)this.mApp.GetWideScreenAdjusted(ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_INGAME_UI_RIGHT_JAW)));
			}
			this.mZumaBarX = 344;
			this.mZumaBarWidth = int.MaxValue;
			this.Reset();
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x000806E4 File Offset: 0x0007E8E4
		public virtual Level Clone()
		{
			Level level = (Level)base.MemberwiseClone();
			level.mCurveMgr = new CurveMgr[4];
			level.mCloakedBossTextAlpha = (float[])this.mCloakedBossTextAlpha.Clone();
			level.mDaisRocks = new List<DaisRock>();
			if (this.mDaisRocks != null)
			{
				level.mDaisRocks.AddRange(this.mDaisRocks.ToArray());
			}
			level.mEggs = new List<TorchLevelEgg>();
			if (this.mEggs != null)
			{
				level.mEggs.AddRange(this.mEggs.ToArray());
			}
			level.mMovingWallDefaults = new List<Wall>();
			if (this.mMovingWallDefaults != null)
			{
				level.mMovingWallDefaults.AddRange(this.mMovingWallDefaults.ToArray());
			}
			level.mEffects = new List<Effect>();
			if (this.mEffects != null)
			{
				level.mEffects.AddRange(this.mEffects.ToArray());
			}
			level.mCloakPoof = this.mCloakPoof;
			level.mFrogFlyOff = this.mFrogFlyOff;
			level.mPowerupRegions = new List<PowerupRegion>();
			if (this.mPowerupRegions != null)
			{
				level.mPowerupRegions.AddRange(this.mPowerupRegions.ToArray());
			}
			level.mTorches = new List<Torch>();
			if (this.mTorches != null)
			{
				level.mTorches.AddRange(this.mTorches.ToArray());
			}
			level.mEffectNames = new List<string>();
			if (this.mEffectNames != null)
			{
				level.mEffectNames.AddRange(this.mEffectNames.ToArray());
			}
			level.mEffectParams = new List<EffectParams>();
			if (this.mEffectParams != null)
			{
				level.mEffectParams.AddRange(this.mEffectParams.ToArray());
			}
			level.mTreasurePoints = new List<TreasurePoint>();
			if (this.mTreasurePoints != null)
			{
				level.mTreasurePoints.AddRange(this.mTreasurePoints.ToArray());
			}
			level.mCurveMgr = (CurveMgr[])this.mCurveMgr.Clone();
			level.mCurveSkullAngleOverrides = (float[])this.mCurveSkullAngleOverrides.Clone();
			level.mHoleMgr = this.mHoleMgr;
			level.mTunnelData = new List<TunnelData>();
			if (this.mTunnelData != null)
			{
				level.mTunnelData.AddRange(this.mTunnelData.ToArray());
			}
			level.mWalls = new List<Wall>();
			if (this.mWalls != null)
			{
				level.mWalls.AddRange(this.mWalls.ToArray());
			}
			level.mFrogImages = (LillyPadImageInfo[])this.mFrogImages.Clone();
			level.mFrogX = (int[])this.mFrogX.Clone();
			level.mFrogY = (int[])this.mFrogY.Clone();
			return level;
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00080978 File Offset: 0x0007EB78
		public virtual void Dispose()
		{
			if (this.mCloakPoof != null)
			{
				this.mCloakPoof.Dispose();
				this.mCloakPoof = null;
			}
			if (this.mFrogFlyOff != null)
			{
				this.mFrogFlyOff.Dispose();
				this.mFrogFlyOff = null;
			}
			if (this.mTorchCompMgr != null)
			{
				this.mTorchCompMgr = null;
			}
			for (int i = 0; i < 4; i++)
			{
				if (this.mCurveMgr[i] != null)
				{
					this.mCurveMgr[i].Dispose();
					this.mCurveMgr[i] = null;
				}
			}
			for (int j = 0; j < 5; j++)
			{
				if (this.mFrogImages[j].mFilename.Length > 0)
				{
					if (this.mFrogImages[j].mImage != null)
					{
						this.mFrogImages[j].mImage.Dispose();
					}
				}
				else
				{
					this.mApp.mResourceManager.DeleteImage(this.mFrogImages[j].mResId);
				}
			}
			if (this.mHoleMgr != null)
			{
				this.mHoleMgr = null;
			}
			if (this.mOrgBoss != null && this.mOrgBoss.mResGroup.Length > 0 && !SexyFramework.Common.StrEquals(this.mOrgBoss.mResGroup, "Boss6Common") && GameApp.gApp.mResourceManager.IsGroupLoaded(this.mOrgBoss.mResGroup))
			{
				GameApp.gApp.mResourceManager.DeleteResources(this.mOrgBoss.mResGroup);
			}
			if (this.mSecondaryBoss != null && this.mSecondaryBoss.mResGroup.Length > 0 && !SexyFramework.Common.StrEquals(this.mOrgBoss.mResGroup, "Boss6Common") && GameApp.gApp.mResourceManager.IsGroupLoaded(this.mSecondaryBoss.mResGroup))
			{
				GameApp.gApp.mResourceManager.DeleteResources(this.mSecondaryBoss.mResGroup);
			}
			if (this.mBossBGID != "")
			{
				BaseRes baseRes = GameApp.gApp.mResourceManager.GetBaseRes(0, this.mBossBGID);
				string text = baseRes.mCompositeResGroup;
				if (text.Length == 0)
				{
					text = baseRes.mResGroup;
				}
				if (text.Length > 0 && GameApp.gApp.mResourceManager.IsGroupLoaded(text))
				{
					GameApp.gApp.mResourceManager.DeleteResources(text);
				}
			}
			if (this.mOrgBoss != null)
			{
				this.mOrgBoss.Dispose();
				this.mOrgBoss = null;
			}
			if (this.mSecondaryBoss != null)
			{
				this.mSecondaryBoss.Dispose();
				this.mSecondaryBoss = null;
			}
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x00080BD2 File Offset: 0x0007EDD2
		public virtual int GetNumCurves()
		{
			return this.mNumCurves;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00080BDC File Offset: 0x0007EDDC
		public virtual int GetGunPointFromPos(int x, int y)
		{
			for (int i = 0; i < this.mNumFrogPoints; i++)
			{
				int num = x - this.mFrogX[i];
				int num2 = y - this.mFrogY[i];
				if (num * num + num2 * num2 < 3136)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x00080C24 File Offset: 0x0007EE24
		public virtual void Preload()
		{
			if (this.mZone == 6 && (this.IsFinalBossLevel() || this.mEndSequence != -1) && this.IsFinalBossLevel())
			{
				this.mBossIntroBG = this.mApp.mResourceManager.GetResourceRef(0, "IMAGE_BOSS6_INTRO_BG").GetSharedImageRef();
				this.mBossBGID = "IMAGE_BOSS6_INTRO_BG";
			}
			if (this.mBoss != null && this.mZone != 6)
			{
				this.mBossIntroBG = this.mApp.mResourceManager.GetResourceRef(0, this.mBoss.mResPrefix + "INTRO_BG").GetSharedImageRef();
				this.mBossBGID = this.mBoss.mResPrefix + "INTRO_BG";
			}
			if (this.mBossBGID != "")
			{
				BaseRes baseRes = this.mApp.mResourceManager.GetBaseRes(0, this.mBossBGID);
				string text = baseRes.mCompositeResGroup;
				if (text != "")
				{
					text = baseRes.mResGroup;
				}
				if (text != "" && !this.mApp.mResourceManager.IsGroupLoaded(text))
				{
					this.mApp.mResourceManager.LoadResources(text);
					if (!this.mApp.mResourceManager.LoadResources(text))
					{
						this.mApp.ShowResourceError(true);
					}
				}
			}
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x00080D78 File Offset: 0x0007EF78
		public virtual void StartLevel(bool from_load, bool needs_reinit)
		{
			new Stopwatch("Level::StartLevel");
			this.Preload();
			if (this.mZone == 5 && !this.mApp.mResourceManager.IsGroupLoaded("GrottoSounds") && !this.mApp.mResourceManager.LoadResources("GrottoSounds"))
			{
				this.mApp.ShowResourceError(true);
				this.mApp.Shutdown();
				return;
			}
			if (this.mZone != 5 && this.mApp.mResourceManager.IsGroupLoaded("GrottoSounds"))
			{
				this.mApp.mResourceManager.DeleteResources("GrottoSounds");
			}
			else if (this.mZone != 6 && this.mApp.mResourceManager.IsGroupLoaded("Boss6Common"))
			{
				this.mApp.mResourceManager.DeleteResources("Boss6Common");
			}
			if (!needs_reinit)
			{
				new Stopwatch("Level::StartLevel::GetImage - FrogImages");
				for (int i = 0; i < 5; i++)
				{
					if (this.mFrogImages[i].mFilename.Length != 0)
					{
						string pathFrom = SexyFramework.Common.GetPathFrom(this.mFrogImages[i].mFilename, "");
						string idByPath = GameApp.gApp.mResourceManager.GetIdByPath(pathFrom);
						this.mFrogImages[i].mImage = (DeviceImage)this.mApp.mResourceManager.LoadImage(idByPath).GetImage();
						this.mFrogImages[i].mImage.mNumCols = 2;
					}
					else if (this.mFrogImages[i].mResId.Length != 0)
					{
						this.mFrogImages[i].mImage = (DeviceImage)this.mApp.mResourceManager.LoadImage(this.mFrogImages[i].mResId).GetImage();
						if (this.mFrogImages[i].mImage != null)
						{
							this.mFrogImages[i].mImage.mNumCols = 2;
						}
					}
				}
			}
			new Stopwatch("Level::StartLevel::LoadCurve");
			for (int j = 0; j < this.mNumCurves; j++)
			{
				if (!this.mCurveMgr[j].mIsLoaded && !this.mCurveMgr[j].LoadCurve())
				{
					this.mApp.Popup("Unable to load curve for " + this.mCurveMgr[j].GetPath());
				}
				if (this.mBoard.GauntletMode())
				{
					this.mCurveMgr[j].mCurveDesc.mVals.mNumColors = GameApp.gDDS.GetNumGauntletBalls(this.mNumCurves);
				}
				this.mCurveMgr[j].StartLevel(from_load);
				if (j == 0)
				{
					this.mCurveMgr[j].mInitialPathHilite = true;
				}
			}
			for (int k = 0; k < this.mHoleMgr.GetNumHoles(); k++)
			{
				int l = 0;
				while (l < 4)
				{
					if (this.mHoleMgr.GetHole(k).mCurveNum == l)
					{
						if (this.mCurveSkullAngleOverrides[l] < 3.4028235E+38f)
						{
							this.mHoleMgr.GetHole(k).mRotation = this.mCurveSkullAngleOverrides[l];
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
			ZumasRevenge.Common.gAddBalls = false;
			if (!needs_reinit)
			{
				this.mEffects.Clear();
				this.InitEffects();
				if (this.IsFinalBossLevel() && !this.mHasDoneTorchCrap && !this.mDoTorchCrap)
				{
					this.InitFinalBossLevel();
				}
				else if (this.mEndSequence == 3)
				{
					this.mBoard.mPreventBallAdvancement = false;
				}
				this.ResetEffects();
			}
			this.mPostZumaTimeCounter = this.mApp.GetLevelMgr().mPostZumaTime;
			this.mPostZumaTimeSlowInc = 0f;
			this.mPostZumaTimeSpeedInc = 0f;
			this.mTimer = this.mTimeToComplete;
			if (this.mBoss != null)
			{
				this.mBoss.Init(this);
				if (this.mSecondaryBoss != null)
				{
					this.mSecondaryBoss.Init(this);
				}
				if (!needs_reinit && this.mEndSequence == 2 && this.mBoard.GetGameState() == GameState.GameState_Playing)
				{
					this.mCloakClapFrame = -1;
					this.mCanDrawBoss = false;
					Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_LAME_CLOAKEDBOSS_ARMDOWN_REST);
					this.mTorchBossY = (float)(-(float)imageByID.mHeight - ZumasRevenge.Common._DS(ZumasRevenge.Common._M(100)));
					this.mCloakPoof = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_CLOAKTOLAMEEXPLOSION01").Duplicate();
					ZumasRevenge.Common.SetFXNumScale(this.mCloakPoof, GameApp.gApp.Is3DAccelerated() ? 1f : ZumasRevenge.Common._M(0.15f));
					for (int m = 0; m < 3; m++)
					{
						this.mCloakedBossTextAlpha[m] = 0f;
					}
				}
				if (GameApp.gDDS.HasBossParam("HurryAmt"))
				{
					this.mHurryToRolloutAmt = GameApp.gDDS.GetBossParam("HurryAmt");
				}
				this.mGingerMouthX = this.mGingerMouthXStart + 20f;
				this.mFredMouthX = this.mFredMouthXStart - 20f;
				this.mFredTongueX = 505f;
				this.mTargetBarSize = 330;
				this.mCurBarSize = 0;
			}
			else if (this.mTimeToComplete > 0)
			{
				this.mGingerMouthX = this.mGingerMouthXStart + 20f;
				this.mFredMouthX = this.mFredMouthXStart - 20f;
				this.mFredTongueX = 505f;
				this.mTargetBarSize = 330;
				this.mCurBarSize = 0;
			}
			if (this.IsFinalBossLevel())
			{
				if (!this.mApp.mResourceManager.IsGroupLoaded("Bosses") && !this.mApp.mResourceManager.LoadResources("Bosses"))
				{
					this.mApp.ShowResourceError(true);
					this.mApp.Shutdown();
					return;
				}
				this.mIntroTorchDelay = 0;
				this.mIntroTorchIndex = -1;
			}
			if (!needs_reinit)
			{
				for (int n = 0; n < this.mEffects.size<Effect>(); n++)
				{
					this.mEffects[n].LevelStarted(from_load);
				}
			}
			if (this.mBoard.GauntletMode())
			{
				this.mGauntletCurNumForMult = this.mApp.GetLevelMgr().mGauntletNumForMultBase;
			}
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x00081353 File Offset: 0x0007F553
		public virtual void StartLevel()
		{
			this.StartLevel(false, false);
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00081360 File Offset: 0x0007F560
		public string GetCurvePath(int curve_num)
		{
			string text = "levels/";
			if (this.mCurveMgr[curve_num].mCurveDesc.mPath.IndexOf('/') != -1 || this.mCurveMgr[curve_num].mCurveDesc.mPath.IndexOf('\\') != -1)
			{
				text += this.mCurveMgr[curve_num].mCurveDesc.mPath;
			}
			else
			{
				text = text + this.mId + "/" + this.mCurveMgr[curve_num].mCurveDesc.mPath;
			}
			return text;
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x000813EB File Offset: 0x0007F5EB
		public bool CanDrawFrog()
		{
			return !this.IsFinalBossLevel() || this.mTorchStageState == 6 || this.mTorchStageState > 10;
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0008140C File Offset: 0x0007F60C
		public virtual void Reset(bool reset_effects)
		{
			this.mGauntletTimeRedAmt = 0f;
			this.mCurMultiplierTimeLeft = (this.mMaxMultiplierTime = 0);
			this.mGauntletCurNumForMult = 0;
			this.mGauntletCurTime = 0;
			this.mAllCurvesAtRolloutPoint = false;
			this.mHasReachedCruisingSpeed = false;
			this.mZumaBallPct = 0f;
			this.mZumaBallFrame = 0;
			this.mTargetBarSize = 0;
			this.mCurBarSize = 0;
			this.mBarLightness = 0f;
			this.mHaveReachedTarget = false;
			this.mNumGauntletBallsBroke = 0;
			this.mCurGauntletMultPct = 0f;
			this.mGauntletMultipliersEarned = 0;
			this.mGingerMouthX = this.mGingerMouthXStart;
			this.mFredMouthX = this.mFredMouthXStart;
			this.mGingerMouthVX = 0.5f;
			this.mFredMouthVX = 0f;
			this.mFredTongueX = 541f;
			this.mFredTongueVX = 0f;
			this.mZumaBallPct = 0f;
			this.mZumaBarState = -1;
			this.mFurthestBallDistance = 0;
			this.mGoldBallXOff = 0f;
			for (int i = 0; i < this.mNumCurves; i++)
			{
				this.mCurveMgr[i].Reset();
			}
			if (this.mApp != null && reset_effects && ((this.mBoard != null && !this.mBoard.GauntletMode()) || (this.mBoard == null && this.mApp.mLoadingThreadStarted && !this.mApp.mLoadingThreadCompleted)))
			{
				for (int j = 0; j < Enumerable.Count<Effect>(this.mEffects); j++)
				{
					this.mEffects[j].NukeParams();
				}
				for (int k = 0; k < Enumerable.Count<EffectParams>(this.mEffectParams); k++)
				{
					this.mEffects[this.mEffectParams[k].mEffectIndex].SetParams(this.mEffectParams[k].mKey, this.mEffectParams[k].mValue);
				}
				for (int l = 0; l < Enumerable.Count<Effect>(this.mEffects); l++)
				{
					this.mEffects[l].Reset(this.mId);
				}
			}
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00081621 File Offset: 0x0007F821
		public virtual void Reset()
		{
			this.Reset(true);
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0008162C File Offset: 0x0007F82C
		public virtual void ReInit()
		{
			for (int i = 0; i < this.mNumCurves; i++)
			{
				this.mCurveMgr[i].SetFarthestBall(0);
			}
			this.mPotPct = 1f;
			this.mCurBarSizeInc = 1;
			this.mInvertMouseTimer = (this.mMaxInvertMouseTimer = 0);
			this.mTimer = (this.mTimeToComplete = -1);
			this.mFurthestBallDistance = 0;
			this.mOffscreenClearBonus = false;
			this.mPostZumaTimeCounter = 0;
			this.mPostZumaTimeSlowInc = (this.mPostZumaTimeSpeedInc = 0f);
			this.mTempSpeedupTimer = 0;
			if (this.mOrgBoss != null)
			{
				int x = this.mBoss.GetX();
				int y = this.mBoss.GetY();
				Boss boss = this.mApp.GetLevelMgr().GetLevelById(this.mId).mBoss;
				Boss boss2 = boss.Instantiate();
				boss2.mName = this.mDisplayName;
				boss2.PostInstantiationHook(boss);
				boss2.mLevel = this;
				this.mOrgBoss = null;
				this.mBoss = boss2;
				this.mBoss.SetXY((float)x, (float)y);
				this.mOrgBoss = this.mBoss;
			}
			if (this.mSecondaryBoss != null)
			{
				Boss boss3 = this.mApp.GetLevelMgr().GetLevelById(this.mId).mSecondaryBoss;
				Boss boss2 = boss3.Instantiate();
				boss2.mName = this.mDisplayName;
				boss2.PostInstantiationHook(boss3);
				boss2.mLevel = this;
				this.mSecondaryBoss = null;
				this.mSecondaryBoss = boss2;
			}
			for (int j = 0; j < this.mTorches.size<Torch>(); j++)
			{
				if (this.mTorches[j].mFlame != null)
				{
					this.mTorches[j].mFlame.ResetAnim();
					this.mTorches[j].mFlame.mEmitAfterTimeline = true;
				}
				this.mTorches[j].mActive = true;
				this.mTorches[j].mDraw = true;
				this.mTorches[j].mWasHit = false;
			}
			this.Reset(false);
			for (int k = 0; k < this.mNumCurves; k++)
			{
				this.mCurveMgr[k].Reset();
			}
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0008186A File Offset: 0x0007FA6A
		public virtual void AfterBoardAdded()
		{
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0008186C File Offset: 0x0007FA6C
		public virtual bool CollidedWithWall(Bullet b)
		{
			float num = (float)b.GetRadius() * ZumasRevenge.Common._M(0.75f);
			FRect theTRect = new FRect(b.GetX() - num, b.GetY() - num, num * 2f, num * 2f);
			for (int i = 0; i < this.mWalls.size<Wall>(); i++)
			{
				Wall wall = this.mWalls[i];
				if (wall.mStrength != 0 && wall.mType != 0)
				{
					int num2 = ((wall.mImage == null) ? ((int)wall.mWidth) : wall.mImage.GetCelWidth());
					int num3 = ((wall.mImage == null) ? ((int)wall.mHeight) : wall.mImage.GetCelHeight());
					int num4 = ((wall.mImage == null) ? 0 : (num2 / 2));
					int num5 = ((wall.mImage == null) ? 0 : (num3 / 2));
					FRect frect = new FRect(wall.mX - (float)num4, wall.mY - (float)num5, (float)num2, (float)num3);
					if (frect.Intersects(theTRect))
					{
						if (wall.mStrength > 0)
						{
							wall.mStrength--;
						}
						if (wall.mStrength == 0)
						{
							wall.mCurRespawnTimer = 0;
						}
						frect.Inflate(theTRect.mWidth / 2f, theTRect.mHeight / 2f);
						FPoint a = new FPoint(b.GetX() + b.mVelX, b.GetY() + b.mVelY);
						FPoint a2 = new FPoint(b.GetX() - b.mVelX, b.GetY() - b.mVelY);
						float angle;
						if (ZumasRevenge.Common.LinesIntersect(a, a2, new FPoint(frect.mX, frect.mY), new FPoint(frect.mX + frect.mWidth, frect.mY)))
						{
							angle = 90f;
						}
						else if (ZumasRevenge.Common.LinesIntersect(a, a2, new FPoint(frect.mX, frect.mY + frect.mHeight), new FPoint(frect.mX + frect.mWidth, frect.mY + frect.mHeight)))
						{
							angle = 270f;
						}
						else if (ZumasRevenge.Common.LinesIntersect(a, a2, new FPoint(frect.mX, frect.mY), new FPoint(frect.mX, frect.mY + frect.mHeight)))
						{
							angle = 180f;
						}
						else
						{
							if (!ZumasRevenge.Common.LinesIntersect(a, a2, new FPoint(frect.mX + frect.mWidth, frect.mY), new FPoint(frect.mX + frect.mWidth, frect.mY + frect.mHeight)))
							{
								return true;
							}
							angle = 0f;
						}
						this.mBoard.AddBallExplosionParticleEffect(b, angle, 180f);
						GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_WALLBALL));
						return true;
					}
				}
			}
			Rect r = new Rect((int)theTRect.mX, (int)theTRect.mY, (int)theTRect.mWidth, (int)theTRect.mHeight);
			for (int j = 0; j < this.mTorches.size<Torch>(); j++)
			{
				Torch torch = this.mTorches[j];
				torch.CheckCollision(r);
			}
			return false;
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00081BC4 File Offset: 0x0007FDC4
		public virtual void CopyEffectsFrom(Level l)
		{
			for (int i = 0; i < this.mEffects.size<Effect>(); i++)
			{
				for (int j = 0; j < l.mEffects.size<Effect>(); j++)
				{
					if (ZumasRevenge.Common.StrEquals(l.mEffects[j].GetName(), this.mEffects[i].GetName()))
					{
						this.mEffects[i].CopyFrom(l.mEffects[j]);
						break;
					}
				}
			}
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00081C45 File Offset: 0x0007FE45
		public virtual string GetStatsScreenText(GameStats stats, int score)
		{
			return "";
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00081C4C File Offset: 0x0007FE4C
		public void AddTorch(int x, int y, int w, int h)
		{
			Torch torch = new Torch();
			torch.mX = x;
			torch.mY = y;
			torch.mWidth = w;
			torch.mHeight = h;
			this.mTorches.Add(torch);
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00081C88 File Offset: 0x0007FE88
		public bool PointIntersectsWall(float x, float y)
		{
			if (Enumerable.Count<Wall>(this.mWalls) == 0)
			{
				return false;
			}
			for (int i = 0; i < Enumerable.Count<Wall>(this.mWalls); i++)
			{
				Wall wall = this.mWalls[i];
				Rect rect = new Rect((int)wall.mX, (int)wall.mY, (int)wall.mWidth, (int)wall.mHeight);
				if (wall.mStrength != 0 && rect.Contains((int)x, (int)y))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00081D04 File Offset: 0x0007FF04
		public void DrawDaisRocks(Graphics g)
		{
			for (int i = 0; i < this.mDaisRocks.size<DaisRock>(); i++)
			{
				DaisRock daisRock = this.mDaisRocks[i];
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)daisRock.mAlpha);
				this.mGlobalTranform.Reset();
				this.mGlobalTranform.Scale(daisRock.mSize, daisRock.mSize);
				float rot = (255f - daisRock.mAlpha) / 255f * ZumasRevenge.Common._M(2.5f) * 3.1415927f;
				this.mGlobalTranform.RotateRad(rot);
				g.DrawImageTransform(daisRock.mImg, this.mGlobalTranform, daisRock.mX, daisRock.mY);
			}
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00081DD0 File Offset: 0x0007FFD0
		public void FadeInkSpots()
		{
			for (int i = 0; i < this.mNumCurves; i++)
			{
				this.mCurveMgr[i].QuicklyFadeInkSpots();
			}
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00081DFC File Offset: 0x0007FFFC
		public void MultiplierActivated()
		{
			this.mGauntletCurNumForMult += this.mApp.GetLevelMgr().mGauntletNumForMultInc;
			if (this.mGauntletCurNumForMult > this.mApp.GetLevelMgr().mMaxGauntletNumForMult)
			{
				this.mGauntletCurNumForMult = this.mApp.GetLevelMgr().mMaxGauntletNumForMult;
			}
			int mMultiplierDuration = this.mApp.GetLevelMgr().mMultiplierDuration;
			if (this.mCurMultiplierTimeLeft == 0)
			{
				this.mCurMultiplierTimeLeft = (this.mMaxMultiplierTime = mMultiplierDuration);
			}
			else
			{
				this.mCurMultiplierTimeLeft += mMultiplierDuration;
				this.mMaxMultiplierTime = this.mCurMultiplierTimeLeft;
			}
			if (GameApp.gDDS.AddMultiplierTime(this.mApp.GetLevelMgr().mMultiplierTimeAdd))
			{
				this.UpdateChallengeModeDifficulty();
			}
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00081EBC File Offset: 0x000800BC
		public void UpdateChallengeModeDifficulty()
		{
			for (int i = 0; i < this.mNumCurves; i++)
			{
				this.mCurveMgr[i].mCurveDesc.mVals.mNumColors = GameApp.gDDS.GetNumGauntletBalls(this.mNumCurves);
			}
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x00081F04 File Offset: 0x00080104
		public virtual void SkipInitialPathHilite()
		{
			bool flag = false;
			for (int i = 0; i < this.mNumCurves; i++)
			{
				if (Enumerable.Count<PathSparkle>(this.mCurveMgr[i].mSparkles) > 0 || this.mCurveMgr[i].mInitialPathHilite)
				{
					flag = true;
					this.mCurveMgr[i].mSparkles.Clear();
					this.mCurveMgr[i].mInitialPathHilite = false;
				}
			}
			if (flag)
			{
				ZumasRevenge.Common.gAddBalls = true;
			}
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00081F74 File Offset: 0x00080174
		public virtual bool DoingInitialPathHilite()
		{
			for (int i = 0; i < this.mNumCurves; i++)
			{
				if (this.mCurveMgr[i].mInitialPathHilite)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00081FA4 File Offset: 0x000801A4
		public virtual void SwitchToSecondaryBoss()
		{
			int x = this.mBoss.GetX();
			int y = this.mBoss.GetY();
			float hp = this.mBoss.GetHP();
			this.mBoss = this.mSecondaryBoss;
			this.mBoss.SetHP(hp);
			this.mBoss.SetXY((float)x, (float)y);
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00082020 File Offset: 0x00080220
		public virtual void Update(float f)
		{
			this.mUpdateCount++;
			if (this.mTimer > 0)
			{
				this.mTimer--;
			}
			if (this.mInvertMouseTimer > 0 && --this.mInvertMouseTimer == 0)
			{
				GameApp.gApp.GetBoard().UpdateGunPos();
			}
			if (this.mTorchStageState == 10 || this.mTorchStageState == 11 || (this.mTorchStageState == 9 && this.mBoard.mFullScreenAlphaRate < 0))
			{
				if (this.mTorchStageState != 11 && this.mUpdateCount % ZumasRevenge.Common._M(2) == 0)
				{
					List<Image> list = new List<Image>();
					for (int i = 0; i < 3; i++)
					{
						list.Add(Res.GetImageByID(ResID.IMAGE_LEVELS_BOSS6PART1_ROCKFALL_PEBBLE1 + i));
						list.Add(Res.GetImageByID(ResID.IMAGE_LEVELS_BOSS6PART1_ROCKFALL_SPECK1 + i));
					}
					this.mDaisRocks.Add(new DaisRock());
					DaisRock daisRock = this.mDaisRocks.back<DaisRock>();
					daisRock.mImg = list[SexyFramework.Common.Rand(list.size<Image>())];
					daisRock.mX = (float)ZumasRevenge.Common._DS(MathUtils.IntRange(ZumasRevenge.Common._M(400), ZumasRevenge.Common._M1(1200)));
					daisRock.mY = (float)(-(float)daisRock.mImg.mHeight / 2);
				}
				for (int j = 0; j < this.mDaisRocks.size<DaisRock>(); j++)
				{
					DaisRock daisRock2 = this.mDaisRocks[j];
					daisRock2.mY += ZumasRevenge.Common._M(15f);
					daisRock2.mSize -= ZumasRevenge.Common._M(0.002f);
					daisRock2.mAlpha -= ZumasRevenge.Common._M(0.1f);
					if (daisRock2.mSize <= 0f || daisRock2.mAlpha <= 0f)
					{
						this.mDaisRocks.RemoveAt(j);
						j--;
					}
				}
			}
			if ((this.IsFinalBossLevel() && this.mTorchStageState != 6) || this.mTorchStageState >= 10)
			{
				string[] array = new string[] { "start", "squish", "rattle" };
				Composition composition = null;
				int num;
				switch (this.mTorchStageState)
				{
				case 0:
					num = 0;
					break;
				case 1:
					num = 1;
					break;
				default:
					num = 2;
					break;
				}
				if (this.mTorchStageState < 6)
				{
					composition = this.mTorchCompMgr.GetComposition(array[num]);
				}
				float num2 = ZumasRevenge.Common._M(0.97f);
				int num3 = ZumasRevenge.Common._M(15);
				if (this.mTorchStageState == 0)
				{
					if (--this.mTorchStageTimer <= 0)
					{
						composition.Update();
						this.mTorchBossX += this.mTorchBossVX;
						this.mTorchBossY += this.mTorchBossVY;
						if (this.mTorchBossX >= this.mTorchBossDestX)
						{
							this.mTorchBossX = this.mTorchBossDestX;
							this.mTorchBossVX = 0f;
						}
						if (this.mTorchBossY >= this.mTorchBossDestY)
						{
							this.mTorchBossY = this.mTorchBossDestY;
							this.mTorchBossVY = 0f;
						}
						if (this.mTorchBossVX == 0f && this.mTorchBossVY == 0f)
						{
							this.mTorchStageState = 1;
							this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_CLOAKED_DAIS_LANDING));
						}
					}
				}
				else if (this.mTorchStageState == 1)
				{
					composition.Update();
					if (composition.mUpdateCount == num3)
					{
						this.mTorchDaisScale = num2;
					}
					float num4 = (1f - num2) / (float)(composition.GetMaxDuration() - num3);
					this.mTorchDaisScale += num4;
					if (this.mTorchDaisScale > 1f)
					{
						this.mTorchDaisScale = 1f;
					}
					if (composition.mUpdateCount >= composition.GetMaxDuration() && SexyFramework.Common._eq(this.mTorchDaisScale, 1f))
					{
						this.mTorchStageState = 2;
						this.mTorchStageTimer = ZumasRevenge.Common._M(100);
					}
				}
				else if (this.mTorchStageState == 2)
				{
					int num5 = ZumasRevenge.Common._M(100);
					if (this.mTorchStageTimer > 0)
					{
						this.mTorchStageTimer--;
					}
					if (this.mTorchStageTimer == 0 && this.mEggs.size<TorchLevelEgg>() < 4)
					{
						composition.Update();
					}
					float[] array2 = new float[] { 38f, 38f, 1421f, 1423f };
					float[] array3 = new float[] { 82f, 952f, 85f, 949f };
					if (composition.mUpdateCount >= composition.GetMaxDuration() && this.mTorchStageTimer <= 0 && this.mEggs.size<TorchLevelEgg>() < 4)
					{
						this.mTorchStageTimer = num5;
						composition.Reset();
						this.mEggs.Add(new TorchLevelEgg());
						TorchLevelEgg torchLevelEgg = this.mEggs.back<TorchLevelEgg>();
						torchLevelEgg.mX = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(545));
						torchLevelEgg.mY = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(208));
						torchLevelEgg.mAlpha = 0f;
						torchLevelEgg.mDestX = ZumasRevenge.Common._DS(array2[this.mEggs.Count - 1]);
						torchLevelEgg.mDestY = ZumasRevenge.Common._DS(array3[this.mEggs.Count - 1] + (float)ZumasRevenge.Common._M(60));
						float num6 = ZumasRevenge.Common._M(60f);
						torchLevelEgg.mVX = (torchLevelEgg.mDestX - torchLevelEgg.mX) / num6;
						torchLevelEgg.mVY = (torchLevelEgg.mDestY - torchLevelEgg.mY) / num6;
						torchLevelEgg.mDestAngle = 3.1415927f * ZumasRevenge.Common._M(3f);
						if (torchLevelEgg.mDestX > torchLevelEgg.mX)
						{
							torchLevelEgg.mDestAngle *= -1f;
						}
						torchLevelEgg.mAngleInc = torchLevelEgg.mDestAngle / num6;
					}
					for (int k = 0; k < this.mEggs.size<TorchLevelEgg>(); k++)
					{
						TorchLevelEgg torchLevelEgg2 = this.mEggs[k];
						if (torchLevelEgg2.mAlpha < 255f && (torchLevelEgg2.mVX != 0f || torchLevelEgg2.mVY != 0f))
						{
							torchLevelEgg2.mAlpha += (float)ZumasRevenge.Common._M(8);
							if (torchLevelEgg2.mAlpha > 255f)
							{
								torchLevelEgg2.mAlpha = 255f;
							}
						}
						torchLevelEgg2.mX += torchLevelEgg2.mVX;
						torchLevelEgg2.mY += torchLevelEgg2.mVY;
						int num7 = 0;
						if ((torchLevelEgg2.mVX < 0f && torchLevelEgg2.mX <= torchLevelEgg2.mDestX) || (torchLevelEgg2.mVX > 0f && torchLevelEgg2.mX >= torchLevelEgg2.mDestX))
						{
							num7++;
						}
						if ((torchLevelEgg2.mVY < 0f && torchLevelEgg2.mY <= torchLevelEgg2.mDestY) || (torchLevelEgg2.mVY > 0f && torchLevelEgg2.mY >= torchLevelEgg2.mDestY))
						{
							num7++;
						}
						if (num7 == 2)
						{
							if (this.mTorches[k].mActive)
							{
								this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_TORCH_EXTINGUISHED));
							}
							this.mTorches[k].mActive = false;
						}
						torchLevelEgg2.mAngle += torchLevelEgg2.mAngleInc;
					}
					Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSSES_EGG);
					if (this.mEggs.size<TorchLevelEgg>() == 4 && !new Rect(-80, 0, this.mApp.mWidth + ZumasRevenge.Common._S(160), this.mApp.mHeight).Intersects(new Rect((int)Enumerable.Last<TorchLevelEgg>(this.mEggs).mX, (int)Enumerable.Last<TorchLevelEgg>(this.mEggs).mY, imageByID.mWidth, imageByID.mHeight)))
					{
						this.mTorchStageState = 3;
						this.mTorchStageTimer = ZumasRevenge.Common._M(50);
					}
				}
				else if (this.mTorchStageState == 7 || (this.mTorchStageState == 9 && this.mBoard.mFullScreenAlphaRate < 0))
				{
					this.mBoard.UpdatePlayingFX();
					List<Image> list2 = new List<Image>();
					for (int l = 0; l < 3; l++)
					{
						list2.Add(Res.GetImageByID(ResID.IMAGE_LEVELS_BOSS6PART1_ROCKFALL_PEBBLE1 + l));
						list2.Add(Res.GetImageByID(ResID.IMAGE_LEVELS_BOSS6PART1_ROCKFALL_SPECK1 + l));
					}
					this.mTorchStageAlpha += ZumasRevenge.Common._M(1.5f);
					this.mTorchStageShakeAmt = SexyFramework.Common.Rand(ZumasRevenge.Common._M(5));
					if (this.mUpdateCount % ZumasRevenge.Common._M(10) == 0)
					{
						Image imageByID2 = Res.GetImageByID(ResID.IMAGE_LEVELS_BOSS6PART1_DIAS);
						this.mDaisRocks.Add(new DaisRock());
						DaisRock daisRock3 = this.mDaisRocks.back<DaisRock>();
						float num8 = (float)(ZumasRevenge.Common._DS(660) + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(30)));
						float num9 = num8 + (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(40));
						float num10 = num8 + (float)imageByID2.mWidth - (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(100));
						float num11 = num10 + (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(35));
						float mY = (float)(ZumasRevenge.Common._DS(417) + imageByID2.mHeight - ZumasRevenge.Common._DS(ZumasRevenge.Common._M(100)));
						daisRock3.mImg = list2[SexyFramework.Common.Rand(list2.size<Image>())];
						float num12 = (float)(SexyFramework.Common.IntRange((int)num8, (int)num9) - daisRock3.mImg.mWidth / 2);
						float num13 = (float)(SexyFramework.Common.IntRange((int)num10, (int)num11) + daisRock3.mImg.mWidth / 2);
						daisRock3.mX = ((SexyFramework.Common.Rand(2) == 0) ? num12 : num13);
						daisRock3.mY = mY;
					}
					if (this.mUpdateCount % ZumasRevenge.Common._M(50) == 0)
					{
						this.mApp.mSoundPlayer.Loop(Res.GetSoundByID(ResID.SOUND_NEW_DAIS_RUMBLE));
						if (++Level.last_sound_idx >= 2)
						{
							Level.last_sound_idx = 0;
						}
					}
					for (int m = 0; m < this.mDaisRocks.size<DaisRock>(); m++)
					{
						DaisRock daisRock4 = this.mDaisRocks[m];
						daisRock4.mY += ZumasRevenge.Common._M(1f);
						daisRock4.mSize -= ZumasRevenge.Common._M(0.02f);
						daisRock4.mAlpha -= ZumasRevenge.Common._M(1f);
						if (daisRock4.mSize <= 0f || daisRock4.mAlpha <= 0f)
						{
							this.mDaisRocks.RemoveAt(m);
							m--;
						}
					}
					if (this.mTorchStageAlpha >= 255f && --this.mTorchStageTimer <= 0)
					{
						this.mTorchStageAlpha = 255f;
						this.mTorchStageState = 8;
						this.mTorchStageShakeAmt = 0;
						this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_DAIS_LOWERING));
						this.mApp.SetCursor(ECURSOR.CURSOR_POINTER);
					}
				}
				else if (this.mTorchStageState == 3 || this.mTorchStageState == 8)
				{
					for (int n = 0; n < this.mDaisRocks.size<DaisRock>(); n++)
					{
						DaisRock daisRock5 = this.mDaisRocks[n];
						daisRock5.mY += ZumasRevenge.Common._M(1f);
						daisRock5.mSize -= ZumasRevenge.Common._M(0.02f);
						daisRock5.mAlpha -= ZumasRevenge.Common._M(1f);
						if (daisRock5.mSize <= 0f || daisRock5.mAlpha <= 0f)
						{
							this.mDaisRocks.RemoveAt(n);
							n--;
						}
					}
					if (this.mTorchStageState == 8 && this.mUpdateCount % ZumasRevenge.Common._M(250) == 0)
					{
						this.mApp.mSoundPlayer.Loop(Res.GetSoundByID(ResID.SOUND_NEW_DAIS_RUMBLE));
					}
					if (this.mTorchStageTimer == 1 && this.mTorchStageState == 3)
					{
						this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_DAIS_LOWERING));
					}
					if (--this.mTorchStageTimer <= 0)
					{
						this.mTorchDaisScale -= ZumasRevenge.Common._M(0.01f);
						if (this.mTorchDaisScale <= 0f)
						{
							this.mTorchDaisScale = 0f;
							if (this.mTorchStageState == 3)
							{
								this.mApp.mSoundPlayer.Stop(Res.GetSoundByID(ResID.SOUND_NEW_DAIS_RUMBLE));
								this.mTorchStageState = 4;
								this.mTorchStageTimer = ZumasRevenge.Common._M(150);
							}
							else
							{
								this.mTorchStageState = 9;
							}
						}
					}
				}
				else if (this.mTorchStageState == 4)
				{
					if (--this.mTorchStageTimer <= 0)
					{
						if (this.mTorchStageTimer == 0)
						{
							for (int num14 = 0; num14 < this.mTorches.size<Torch>(); num14++)
							{
								this.mTorches[num14].mFlame.ResetAnim();
								this.mTorches[num14].mActive = true;
								this.mTorches[num14].mDraw = true;
							}
						}
						this.mTorchDaisScale += ZumasRevenge.Common._M(0.02f);
						if (this.mTorchDaisScale >= 1f)
						{
							this.mTorchDaisScale = 1f;
							if (this.mFrogFlyOff != null)
							{
								this.mFrogFlyOff.Dispose();
								this.mFrogFlyOff = null;
							}
							this.mFrogFlyOff = new FrogFlyOff();
							this.mFrogFlyOff.JumpIn(this.mFrog, this.mFrog.GetCenterX(), this.mFrog.GetCenterY(), false);
							this.mTorchStageState = 5;
						}
					}
				}
				else if (this.mTorchStageState == 10)
				{
					this.mFrogFlyOff.Update();
					if (this.mFrogFlyOff.mTimer >= this.mFrogFlyOff.mFrogJumpTime)
					{
						this.mApp.mSoundPlayer.Stop(Res.GetSoundByID(ResID.SOUND_NEW_DAIS_RUMBLE));
						this.mTorchStageState = 11;
						this.mTorchStageTimer = ZumasRevenge.Common._M(100);
						this.mFrog.SetPos((int)this.mFrogFlyOff.mFrogX, this.mFrog.GetCurY());
						this.mFrogFlyOff.Dispose();
						this.mFrogFlyOff = null;
					}
				}
				else if (this.mTorchStageState == 5)
				{
					this.mFrogFlyOff.Update();
					if (this.mFrogFlyOff.mTimer > this.mFrogFlyOff.mFrogJumpTime)
					{
						this.mFrog.SetAngle((float)((int)this.mFrogFlyOff.mFrogAngle));
						this.mFrogFlyOff.Dispose();
						this.mFrogFlyOff = null;
						this.mTorchStageState = 6;
						this.mBoard.mPreventBallAdvancement = false;
						this.mDoTorchCrap = false;
						this.mHasDoneTorchCrap = true;
					}
				}
				else if (this.mTorchStageState == 11)
				{
					if (--this.mTorchStageTimer <= 0 && (this.mTorchBossY += (float)ZumasRevenge.Common._M(10)) >= (float)ZumasRevenge.Common._M1(0))
					{
						this.mTorchStageState = 12;
						this.mTorchStageTimer = 0;
					}
				}
				else if (this.mTorchStageState == 12)
				{
					int num15 = ZumasRevenge.Common._M(500);
					this.mTorchStageTimer++;
					for (int num16 = 0; num16 < 3; num16++)
					{
						if (this.mTorchStageTimer >= num15)
						{
							this.mCloakedBossTextAlpha[num16] -= ZumasRevenge.Common._M(2f);
							if (this.mCloakedBossTextAlpha[num16] < 0f)
							{
								this.mCloakedBossTextAlpha[num16] = 0f;
							}
						}
						else
						{
							this.mCloakedBossTextAlpha[num16] += ZumasRevenge.Common._M(2f);
							if (this.mCloakedBossTextAlpha[num16] > 255f)
							{
								this.mCloakedBossTextAlpha[num16] = 255f;
							}
							else if (this.mCloakedBossTextAlpha[num16] < (float)ZumasRevenge.Common._M(128))
							{
								break;
							}
						}
					}
					Image imageByID3 = Res.GetImageByID(ResID.IMAGE_BOSS_LAME_CLOAKEDBOSS_CLAP);
					int num17 = ZumasRevenge.Common._M(6);
					int num18 = imageByID3.mNumRows * imageByID3.mNumCols;
					if (this.mTorchStageTimer >= num15 && this.mTorchStageTimer % num17 == 0)
					{
						this.mCloakClapFrame++;
						if (this.mCloakClapFrame == ZumasRevenge.Common._M(5))
						{
							this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_CLOAKED_CLAP));
						}
					}
					if (this.mTorchStageTimer >= num15 + ZumasRevenge.Common._M(15))
					{
						this.mCloakPoof.mDrawTransform.LoadIdentity();
						float num19 = GameApp.DownScaleNum(1f);
						this.mCloakPoof.mDrawTransform.Scale(num19, num19);
						this.mCloakPoof.mDrawTransform.Translate((float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(812)), (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(220)));
						this.mCloakPoof.Update();
						if (SexyFramework.Common._eq(this.mCloakPoof.mFrameNum, (float)ZumasRevenge.Common._M(135), 0.5f))
						{
							this.mCanDrawBoss = true;
						}
						else if (this.mCloakPoof.mFrameNum >= (float)this.mCloakPoof.mLastFrameNum)
						{
							this.mBoard.mContinueNextLevelOnLoadProfile = false;
							this.mTorchStageState = 13;
							this.mBoard.mHasDoneIntroSounds = false;
							if (this.mApp.mResourceManager.IsGroupLoaded("CloakedBoss"))
							{
								this.mApp.mResourceManager.DeleteResources("CloakedBoss");
							}
						}
					}
				}
			}
			if (this.mTorchStageState > 4 && this.mBoard.GetGameState() != GameState.GameState_BossIntro && this.mTorchStageState != 12 && this.mTorchTextAlpha > 0f)
			{
				this.mTorchTextAlpha -= ZumasRevenge.Common._M(1.3f);
				if (this.mTorchTextAlpha < 0f)
				{
					this.mTorchTextAlpha = 0f;
				}
			}
			this.UpdateEffects();
			for (int num20 = 0; num20 < this.mWalls.size<Wall>(); num20++)
			{
				Wall wall = this.mWalls[num20];
				wall.Update();
				if ((wall.mVX > 0f && wall.mX > (float)ZumasRevenge.Common._SS(this.mApp.mWidth)) || (wall.mVX < 0f && wall.mX + wall.mWidth < 0f) || (wall.mVY > 0f && wall.mY > (float)ZumasRevenge.Common._SS(this.mApp.mHeight)) || (wall.mVY < 0f && wall.mY + wall.mHeight < 0f))
				{
					this.mWalls.RemoveAt(num20);
					num20--;
				}
			}
			for (int num21 = 0; num21 < this.mMovingWallDefaults.size<Wall>(); num21++)
			{
				Wall wall2 = this.mMovingWallDefaults[num21];
				int num22 = int.MaxValue;
				bool flag = false;
				for (int num23 = 0; num23 < this.mWalls.size<Wall>(); num23++)
				{
					Wall wall3 = this.mWalls[num23];
					if (wall3.mId == wall2.mId)
					{
						flag = true;
						int num24;
						if (wall3.mVX > 0f)
						{
							num24 = (int)((wall3.mX < 0f) ? 0f : (wall3.mX - wall2.mX));
						}
						else
						{
							num24 = (int)((wall3.mX + wall3.mWidth > wall2.mX) ? 0f : (wall2.mX - (wall3.mX + wall3.mWidth)));
						}
						int num25;
						if (wall3.mVY > 0f)
						{
							num25 = (int)((wall3.mY < 0f) ? 0f : (wall3.mY - wall2.mY));
						}
						else
						{
							num25 = (int)((wall3.mY + wall3.mHeight > wall2.mY) ? 0f : (wall2.mY - (wall3.mY + wall3.mHeight)));
						}
						int num26 = num24 * num24 + num25 * num25;
						if (num26 < num22)
						{
							num22 = num26;
						}
					}
				}
				if (num22 > wall2.mSpacing || !flag)
				{
					this.mWalls.Add(wall2);
					this.mWalls.back<Wall>().mCurLifeTimer = MathUtils.IntRange(wall2.mMinLifeTimer, wall2.mMaxLifeTimer);
				}
			}
			this.mHoleMgr.Update();
			if (this.mBoss != null)
			{
				this.mBoss.Update(f);
			}
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x000834F0 File Offset: 0x000816F0
		public virtual void Draw(Graphics g)
		{
			for (int i = 0; i < this.mNumCurves; i++)
			{
				this.mCurveMgr[i].DrawUnderBalls(g);
			}
			for (int j = 0; j < this.mTorches.size<Torch>(); j++)
			{
				this.mTorches[j].Draw(g);
			}
			for (int k = 0; k < this.mEffects.size<Effect>(); k++)
			{
				this.mEffects[k].DrawUnderBalls(g);
			}
			if (this.mBoss != null && this.mCanDrawBoss)
			{
				this.mBoss.DrawBelowBalls(g);
			}
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00083588 File Offset: 0x00081788
		public virtual void DrawBottomLevel(Graphics g)
		{
			if (this.mBoss != null && this.mCanDrawBoss)
			{
				this.mBoss.DrawBottomLevel(g);
			}
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x000835A8 File Offset: 0x000817A8
		public virtual void DrawToplevel(Graphics g)
		{
			if (this.mBoss != null && this.mCanDrawBoss)
			{
				this.mBoss.DrawTopLevel(g);
			}
			for (int i = 0; i < this.mNumCurves; i++)
			{
				this.mCurveMgr[i].DrawTopLevel(g);
			}
			if (this.mTorchTextAlpha > 0f && this.mBoard.GetGameState() != GameState.GameState_BossIntro && this.mTorchStageState > 5)
			{
				int centerX = this.mBoard.GetGun().GetCenterX();
				int centerY = this.mBoard.GetGun().GetCenterY();
				string theString = (this.mBoard.IsHardAdventureMode() ? TextManager.getInstance().getString(495) : TextManager.getInstance().getString(496));
				string theString2 = (this.mBoard.IsHardAdventureMode() ? TextManager.getInstance().getString(497) : TextManager.getInstance().getString(498));
				g.SetFont(Res.GetFontByID(ResID.FONT_BOSS_TAUNT));
				int num = (int)this.mTorchTextAlpha - 350;
				if (num > 0)
				{
					g.SetColor(0, 0, 0, (num > 255) ? 255 : num);
					g.DrawString(theString, ZumasRevenge.Common._S(centerX) - g.GetFont().StringWidth(theString) / 2, ZumasRevenge.Common._S(centerY - ZumasRevenge.Common._M(90)));
				}
				g.SetColor(0, 0, 0, ((int)this.mTorchTextAlpha > 255) ? 255 : ((int)this.mTorchTextAlpha));
				g.DrawString(theString2, ZumasRevenge.Common._S(centerX) - g.GetFont().StringWidth(theString2) / 2, ZumasRevenge.Common._S(centerY + ZumasRevenge.Common._M(120)));
			}
			for (int j = 0; j < this.mPowerupRegions.size<PowerupRegion>(); j++)
			{
				PowerupRegion powerupRegion = this.mPowerupRegions[j];
				if (powerupRegion.mDebugDraw)
				{
					g.SetColor(255, 0, 0);
					int numPoints = this.mCurveMgr[powerupRegion.mCurveNum].mWayPointMgr.GetNumPoints();
					float num2;
					float num3;
					this.mCurveMgr[powerupRegion.mCurveNum].GetXYFromWaypoint((int)(powerupRegion.mCurvePctStart * (float)numPoints), out num2, out num3);
					float num4;
					float num5;
					this.mCurveMgr[powerupRegion.mCurveNum].GetXYFromWaypoint((int)(powerupRegion.mCurvePctEnd * (float)numPoints), out num4, out num5);
					g.FillRect(ZumasRevenge.Common._S((int)num2) - 2, ZumasRevenge.Common._S((int)num3) - 2, 4, 4);
					g.SetColor(0, 255, 0);
					g.FillRect(ZumasRevenge.Common._S((int)num4) - 2, ZumasRevenge.Common._S((int)num5) - 2, 4, 4);
				}
			}
			if (this.mTorchStageState >= 11)
			{
				if (this.mTorchStageTimer > 0 || (this.mCloakPoof != null && this.mCloakPoof.mFrameNum < (float)ZumasRevenge.Common._M(135)))
				{
					int num6 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(32));
					if (this.mTorchStageTimer < ZumasRevenge.Common._M(570))
					{
						g.SetColorizeImages(true);
						g.SetColor(255, 255, 255, 128);
						g.SetColorizeImages(false);
					}
					Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_LAME_CLOAKEDBOSS_ARMDOWN_REST);
					Image imageByID2 = Res.GetImageByID(ResID.IMAGE_BOSS_LAME_CLOAKEDBOSS_CLAP);
					if (this.mCloakClapFrame < 0)
					{
						g.DrawImage(imageByID, ZumasRevenge.Common._S(this.mBoss.GetX()) - imageByID.mWidth / 2 + ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_BOSS_LAME_CLOAKEDBOSS_ARMDOWN_REST)) - num6, ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_BOSS_LAME_CLOAKEDBOSS_ARMDOWN_REST)) + ZumasRevenge.Common._S(this.mBoss.GetY()) + (int)this.mTorchBossY - imageByID.mHeight / 2);
					}
					else if (this.mTorchStageTimer < ZumasRevenge.Common._M(570))
					{
						int theCel = Math.Min(this.mCloakClapFrame, imageByID2.mNumRows * imageByID2.mNumCols - 1);
						g.DrawImageCel(imageByID2, ZumasRevenge.Common._S(this.mBoss.GetX()) - imageByID2.GetCelWidth() / 2 - num6, ZumasRevenge.Common._S(this.mBoss.GetY()) - imageByID2.GetCelHeight() / 2 + (int)this.mTorchBossY, theCel);
					}
					g.SetFont(Res.GetFontByID(ResID.FONT_BOSS_TAUNT));
					bool flag = this.mBoard.IsHardAdventureMode();
					if (this.mTorchStageState == 12)
					{
						string[] array = new string[]
						{
							TextManager.getInstance().getString(490),
							TextManager.getInstance().getString(491),
							TextManager.getInstance().getString(492)
						};
						if (flag)
						{
							array[0] = TextManager.getInstance().getString(493);
							array[1] = TextManager.getInstance().getString(494);
							array[2] = "";
						}
						for (int k = 0; k < array.Length; k++)
						{
							if (this.mCloakedBossTextAlpha[k] > 0f)
							{
								g.SetColor(0, 0, 0, (int)this.mCloakedBossTextAlpha[k]);
								g.WriteString(array[k].ToString(), -GameApp.gApp.mBoardOffsetX, ZumasRevenge.Common._DS(ZumasRevenge.Common._M(550)) + k * g.GetFont().GetHeight(), 1024);
							}
						}
					}
				}
				if (this.mTorchStageState == 12 && this.mCloakPoof.mFrameNum < (float)this.mCloakPoof.mLastFrameNum && this.mCloakPoof.mFrameNum > 0f)
				{
					this.mCloakPoof.Draw(g);
				}
			}
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00083B20 File Offset: 0x00081D20
		public virtual void DrawAboveBalls(Graphics g)
		{
			for (int i = 0; i < this.mEffects.size<Effect>(); i++)
			{
				this.mEffects[i].DrawAboveBalls(g);
			}
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00083B58 File Offset: 0x00081D58
		public virtual void DrawUnderBackground(Graphics g)
		{
			for (int i = 0; i < this.mEffects.size<Effect>(); i++)
			{
				this.mEffects[i].DrawUnderBackground(g);
			}
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00083B90 File Offset: 0x00081D90
		public virtual void DrawFullScene(Graphics g)
		{
			for (int i = 0; i < this.mEffects.size<Effect>(); i++)
			{
				this.mEffects[i].DrawFullScene(g);
			}
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00083BC8 File Offset: 0x00081DC8
		public virtual void DrawFullSceneNoFrog(Graphics g)
		{
			for (int i = 0; i < this.mEffects.size<Effect>(); i++)
			{
				this.mEffects[i].DrawFullSceneNoFrog(g);
			}
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00083C00 File Offset: 0x00081E00
		public virtual void DrawPriority(Graphics g, int priority)
		{
			for (int i = 0; i < this.mEffects.size<Effect>(); i++)
			{
				this.mEffects[i].DrawPriority(g, priority);
			}
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00083C58 File Offset: 0x00081E58
		public virtual void DrawTorchLighting(Graphics g)
		{
			if (this.mTorches.size<Torch>() == 0)
			{
				return;
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_LEVELS_BOSS6PART1_QUADRANT);
			float[] array = new float[] { 1f, 1f, -1f, -1f };
			float[] array2 = new float[] { 1f, -1f, 1f, -1f };
			int[] array3 = new int[]
			{
				ZumasRevenge.Common._DS(-160),
				ZumasRevenge.Common._DS(-160),
				this.mApp.mWidth + ZumasRevenge.Common._DS(320) - imageByID.mWidth,
				this.mApp.mWidth + ZumasRevenge.Common._DS(320) - imageByID.mWidth
			};
			int[] array4 = new int[]
			{
				default(int),
				this.mApp.mHeight - imageByID.mHeight,
				default(int),
				this.mApp.mHeight - imageByID.mHeight
			};
			for (int i = 0; i < this.mTorches.size<Torch>(); i++)
			{
				int mOverlayAlpha = this.mTorches[i].mOverlayAlpha;
				if (mOverlayAlpha != 0)
				{
					if (mOverlayAlpha != 255)
					{
						g.SetColorizeImages(true);
					}
					g.SetColor(255, 255, 255, mOverlayAlpha);
					this.mGlobalTranform.Reset();
					this.mGlobalTranform.Scale(array[i], array2[i]);
					g.DrawImageTransform(imageByID, this.mGlobalTranform, (float)(array3[i] + imageByID.mWidth / 2), (float)(array4[i] + imageByID.mHeight / 2));
					g.SetColorizeImages(false);
				}
			}
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00083DF4 File Offset: 0x00081FF4
		public virtual void DrawSkullPit(Graphics g)
		{
			bool flag = false;
			for (int i = 0; i < this.mEffects.size<Effect>(); i++)
			{
				if (this.mEffects[i].DrawSkullPit(g, this.mHoleMgr))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.mHoleMgr.DrawRings(g);
				this.mHoleMgr.Draw(g);
			}
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00083E54 File Offset: 0x00082054
		public virtual void DrawTunnel(Graphics g, Image img, int x, int y, int w, int h)
		{
			if (this.mNum != 2147483647 || this.mZone != 4 || this.mBoss == null)
			{
				for (int i = 0; i < this.mEffects.size<Effect>(); i++)
				{
					if (!this.mEffects[i].DrawTunnel(g, img, x, y))
					{
						return;
					}
				}
			}
			g.DrawImage(img, x, y, w, h);
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00083EBC File Offset: 0x000820BC
		public void DrawGauntletUI(Graphics g)
		{
			ZumasRevenge.Common._S(ZumasRevenge.Common._M(465));
			ZumasRevenge.Common._S(ZumasRevenge.Common._M(22));
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GAUNTLET_MAIN_BAR_BONUS_OFF);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_GAUNTLET_MAIN_BAR_BONUS_ON);
			Image imageByID3 = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_CHALLENGE_UI_TIMER_FRAME);
			Image imageByID4 = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_CHALLENGE_UI_GAUNTLETFRAMECENTER);
			Image imageByID5 = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_CHALLENGE_UI_GAUNTLETFRAMELEFT);
			g.DrawImage(imageByID, GameApp.gApp.GetWideScreenAdjusted(ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GAUNTLET_MAIN_BAR_BONUS_OFF))) - ZumasRevenge.Common._S(27), ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_GAUNTLET_MAIN_BAR_BONUS_OFF)) + ZumasRevenge.Common._S(7));
			g.DrawImage(imageByID2, GameApp.gApp.GetWideScreenAdjusted(ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GAUNTLET_MAIN_BAR_BONUS_ON))) - ZumasRevenge.Common._S(27), ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_GAUNTLET_MAIN_BAR_BONUS_ON)) + ZumasRevenge.Common._S(7), new Rect(0, 0, (int)((float)imageByID2.mWidth * this.mCurGauntletMultPct), imageByID2.mHeight));
			g.DrawImage(imageByID3, GameApp.gApp.GetWideScreenAdjusted(ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_INGAME_CHALLENGE_UI_TIMER_FRAME))), ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_GUI_INGAME_CHALLENGE_UI_TIMER_FRAME)));
			int num = (int)this.mGauntletTimeRedAmt;
			if (this.mGauntletTimeRedAmt > 0f)
			{
				g.SetColorizeImages(true);
				if (num > 128)
				{
					num = (255 - num) * 2;
				}
				else
				{
					num *= 2;
				}
				if (num > 255)
				{
					num = 255;
				}
				else if (num < 0)
				{
					num = 0;
				}
			}
			int num2 = this.mApp.GetLevelMgr().mGauntletSessionLength - this.mGauntletCurTime;
			if (num2 < 0)
			{
				num2 = 0;
			}
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_STROKE);
			g.SetFont(fontByID);
			g.SetColor(192, 230, 99);
			int theY = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(93)) + (ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(35)) - g.GetFont().mHeight) / 2;
			g.WriteString(JeffLib.Common.UpdateToTimeStr(num2), GameApp.gApp.GetWideScreenAdjusted(ZumasRevenge.Common._DS(ZumasRevenge.Common._M(225))), theY, ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(141)), 0);
			if (num > 0)
			{
				g.SetColor(255, 0, 0, num);
				g.WriteString(JeffLib.Common.UpdateToTimeStr(num2), GameApp.gApp.GetWideScreenAdjusted(ZumasRevenge.Common._DS(ZumasRevenge.Common._M(225))), theY, ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(141)), 0);
			}
			g.SetColorizeImages(false);
			int wideScreenAdjusted = GameApp.gApp.GetWideScreenAdjusted(ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_INGAME_CHALLENGE_UI_GAUNTLETFRAMECENTER)));
			int theY2 = ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_GUI_INGAME_CHALLENGE_UI_GAUNTLETFRAMECENTER));
			int wideScreenAdjusted2 = GameApp.gApp.GetWideScreenAdjusted(ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_INGAME_CHALLENGE_UI_GAUNTLETFRAMELEFT)));
			g.DrawImage(imageByID4, wideScreenAdjusted, theY2);
			g.DrawImage(imageByID5, wideScreenAdjusted2, ZumasRevenge.Common._S(0));
			g.DrawImageMirror(imageByID4, wideScreenAdjusted + imageByID4.GetWidth() + ZumasRevenge.Common._S(60), theY2);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x000841B0 File Offset: 0x000823B0
		public void InitEffects(Level copy_effects_from)
		{
			for (int i = 0; i < this.mEffectNames.size<string>(); i++)
			{
				Effect effect = this.mApp.GetLevelMgr().mEffectManager.GetEffect(this.mEffectNames[i], this.mId, copy_effects_from);
				if (effect != null)
				{
					this.mEffects.Add(effect);
				}
				this.mEffects[i].NukeParams();
			}
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0008421C File Offset: 0x0008241C
		public void InitEffects()
		{
			this.InitEffects(null);
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00084228 File Offset: 0x00082428
		public void ResetEffects()
		{
			for (int i = 0; i < this.mEffects.size<Effect>(); i++)
			{
				this.mEffects[i].LoadResources();
				this.mEffects[i].Reset(this.mId);
			}
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00084274 File Offset: 0x00082474
		public void ForceTreasure(int tnum)
		{
			this.mBoard.mCurTreasureNum = tnum;
			this.mBoard.mCurTreasure = this.mTreasurePoints[tnum];
			this.mBoard.mMinTreasureY = (this.mBoard.mMaxTreasureY = float.MaxValue);
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x000842C4 File Offset: 0x000824C4
		public Ball GetBallById(int id)
		{
			for (int i = 0; i < this.mNumCurves; i++)
			{
				foreach (Ball ball in this.mCurveMgr[i].mBallList)
				{
					if (ball.GetId() == id)
					{
						return ball;
					}
				}
				foreach (Ball ball2 in this.mCurveMgr[i].mPendingBalls)
				{
					if (ball2.GetId() == id)
					{
						return ball2;
					}
				}
			}
			return null;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00084390 File Offset: 0x00082590
		public bool AllTorchesOut()
		{
			if (this.mTorchStageState != 6)
			{
				return false;
			}
			int num = 0;
			for (int i = 0; i < Enumerable.Count<Torch>(this.mTorches); i++)
			{
				if (!this.mTorches[i].mActive)
				{
					num++;
				}
			}
			return num == Enumerable.Count<Torch>(this.mTorches) && Enumerable.Count<Torch>(this.mTorches) > 0;
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x000843F5 File Offset: 0x000825F5
		public bool IsFinalBossLevel()
		{
			return Enumerable.Count<Torch>(this.mTorches) > 0;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00084408 File Offset: 0x00082608
		public virtual void UpdateUI()
		{
			if (this.mZumaBarState >= ZumasRevenge.Common._M(2) && !this.mBoard.GauntletMode() && this.mCurBarSize < this.mTargetBarSize)
			{
				this.mCurBarSize++;
			}
			if (this.mZumaBarState == 0)
			{
				this.mGingerMouthX += this.mGingerMouthVX;
				int num = (int)this.mGingerMouthXStart + ZumasRevenge.Common._S(15);
				if (this.mGingerMouthX >= (float)num)
				{
					this.mGingerMouthX = (float)num;
					this.mZumaBarState++;
					this.mGingerMouthVX = 0f;
				}
			}
			else if (this.mZumaBarState == 1)
			{
				this.mGoldBallXOff += ZumasRevenge.Common._S(0.75f);
				if ((this.mZumaBallPct += 0.05f) >= 1.2f)
				{
					this.mZumaBallPct = 1.2f;
					this.mZumaBarState++;
				}
			}
			else if (this.mZumaBarState == 2)
			{
				if ((this.mZumaBallPct -= 0.05f) <= 1f)
				{
					this.mZumaBallPct = 1f;
				}
				if (this.mGingerMouthVX == 0f && this.mZumaBallPct <= 1f)
				{
					this.mZumaBarState++;
				}
			}
			else if (this.mZumaBarState == 4)
			{
				this.mFredMouthX += this.mFredMouthVX;
				int num2 = ZumasRevenge.Common._S(15);
				if (this.mFredMouthX <= this.mFredMouthXStart - (float)num2)
				{
					this.mFredMouthX = this.mFredMouthXStart - (float)num2;
					this.mFredMouthVX *= -1f;
					this.mZumaBarState++;
					this.mFredTongueVX = ZumasRevenge.Common._S(-2.5f);
				}
			}
			else if (this.mZumaBarState == 5)
			{
				this.mFredTongueX += this.mFredTongueVX;
				int num3 = ZumasRevenge.Common._S(36);
				if (this.mFredTongueX <= (float)(541 - num3))
				{
					this.mFredTongueX = (float)(541 - num3);
					this.mFredTongueVX *= -1f;
					this.mZumaBarState++;
				}
			}
			else if (this.mZumaBarState == 6)
			{
				this.mFredTongueX += this.mFredTongueVX;
				this.mGoldBallXOff += ZumasRevenge.Common._S(2.5f);
				if ((this.mZumaBallPct += 0.05f) >= 1.2f)
				{
					this.mZumaBallPct = 1.2f;
					this.mZumaBarState++;
				}
			}
			else if (this.mZumaBarState == 7)
			{
				this.mFredTongueX += this.mFredTongueVX;
				if ((this.mZumaBallPct -= 0.05f) <= 1f)
				{
					this.mZumaBallPct = 1f;
				}
				this.mGoldBallXOff += ZumasRevenge.Common._S(0.75f);
				if (this.mFredTongueX >= 541f)
				{
					this.mFredTongueX = 541f;
					this.mFredTongueVX = 0f;
				}
				if (this.mFredTongueX >= 541f)
				{
					this.mZumaBarState++;
					int num4 = (int)ZumasRevenge.Common._S(2.5f);
					this.mFredMouthVX = (float)num4;
					this.mGingerMouthVX = (float)(-(float)num4);
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BAR_FULL));
				}
			}
			else if (this.mZumaBarState >= 8 && this.mZumaBarState < 12)
			{
				this.mFredMouthX += this.mFredMouthVX;
				this.mGingerMouthX += this.mGingerMouthVX;
				int num5 = 0;
				if (this.mFredMouthX >= this.mFredMouthXStart && this.mFredMouthVX > 0f)
				{
					num5++;
					this.mFredMouthX = this.mFredMouthXStart;
				}
				else if (this.mFredMouthX <= this.mFredMouthXStart - (float)ZumasRevenge.Common._S(15) && this.mFredMouthVX < 0f)
				{
					num5++;
					this.mFredMouthX = this.mFredMouthXStart - (float)ZumasRevenge.Common._S(15);
				}
				if (this.mGingerMouthX <= this.mGingerMouthXStart && this.mGingerMouthVX < 0f)
				{
					this.mGingerMouthX = this.mGingerMouthXStart;
					num5++;
				}
				else if (this.mGingerMouthX >= this.mGingerMouthXStart + (float)ZumasRevenge.Common._S(15) && this.mGingerMouthVX > 0f)
				{
					this.mGingerMouthX = this.mGingerMouthXStart + (float)ZumasRevenge.Common._S(15);
					num5++;
				}
				if (num5 == 2)
				{
					this.mZumaBarState++;
					this.mFredMouthVX *= -1f;
					this.mGingerMouthVX *= -1f;
				}
			}
			else if (this.mZumaBarState == 12)
			{
				if ((this.mBarLightness += 18f) >= 255f)
				{
					this.mBarLightness = 255f;
					this.mZumaBarState++;
				}
			}
			else if (this.mZumaBarState == 13)
			{
				if ((this.mBarLightness -= 18f) <= 0f)
				{
					this.mBarLightness = 0f;
					this.mZumaBarState++;
					this.mZumaPulseUCStart = this.mUpdateCount;
				}
			}
			else if (this.mZumaBarState == 14 && this.mBoard.GauntletMode())
			{
				this.mCurBarSize -= 2;
				if (this.mCurBarSize <= 0)
				{
					this.Reset();
					this.mCurBarSize = 0;
				}
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_GOLD_BALL);
			if (this.mCurBarSize != this.mTargetBarSize)
			{
				this.mZumaBallFrame = (this.mZumaBallFrame + 1) % imageByID.mNumCols;
			}
			if (!this.mHaveReachedTarget && !this.mBoard.GauntletMode() && this.ShouldUpdateZumaBar() && this.mNumCurves > 0 && this.mCurBarSize == 330 && this.mBoard.mScore >= this.mBoard.mScoreTarget && this.mBoss == null)
			{
				this.mZumaBarState = 4;
				this.mFredMouthVX = ZumasRevenge.Common._S(-2.5f);
				if (!this.mBoard.IsEndless())
				{
					this.mHaveReachedTarget = true;
					for (int i = 0; i < this.mNumCurves; i++)
					{
						this.mCurveMgr[i].ZumaAchieved(true);
						if (!this.mBoard.DestroyAll())
						{
							this.mCurveMgr[i].DetonateBalls();
						}
					}
					this.mApp.mUserProfile.GetAdvModeVars().mNumZumasCurLevel++;
					this.mBoard.mNumZumaBalls = this.GetTotalBallsOnLevel();
				}
				if (!this.mBoard.DestroyAll())
				{
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_EXPLODE));
				}
			}
			int num6 = this.mBoard.mScoreTarget - this.mBoard.GetLevelBeginScore();
			if (num6 > 0)
			{
				int num7 = this.mBoard.mScoreTarget - this.mBoard.mScore;
				if (num7 < 0)
				{
					num7 = 0;
					if (this.mBoard.mLevelEndFrame == 0)
					{
						if (this.mBoard.GetNumBallColors() <= 2)
						{
							this.mBoard.mLevelEndFrame = this.mBoard.GetStateCount();
						}
					}
					else if (this.mBoard.GetStateCount() - this.mBoard.mLevelEndFrame == 3000)
					{
						for (int j = 0; j < this.mNumCurves; j++)
						{
							this.mCurveMgr[j].mCurveDesc.mVals.mPowerUpFreq[0] = 500;
							this.mCurveMgr[j].mCurveDesc.mVals.mPowerUpFreq[1] = 0;
							this.mCurveMgr[j].mCurveDesc.mVals.mPowerUpFreq[2] = 0;
							this.mCurveMgr[j].mCurveDesc.mVals.mPowerUpFreq[3] = 0;
							this.mCurveMgr[j].mCurveDesc.mVals.mAccelerationRate = 0.0003f;
						}
					}
				}
				if (this.mBoss == null && !this.mBoard.GauntletMode())
				{
					this.mTargetBarSize = 330 - 330 * num7 / num6;
				}
			}
			if (this.mBoss != null)
			{
				this.mTargetBarSize = (int)(330f - (1f - this.mBoss.GetHP() / 100f) * 330f);
			}
			if (this.mBoard.GauntletMode() && !this.DoingInitialPathHilite())
			{
				if (this.mGauntletCurTime < this.mApp.GetLevelMgr().mGauntletSessionLength)
				{
					this.mGauntletCurTime++;
					if (this.mGauntletCurTime % 100 == 0 && this.mApp.GetLevelMgr().mGauntletSessionLength - this.mGauntletCurTime <= 1100)
					{
						this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_CHALLENGE_COUNTDOWN));
					}
					if (GameApp.gDDS.SetGauntletTime(this.mGauntletCurTime))
					{
						this.UpdateChallengeModeDifficulty();
					}
					int num8 = this.mApp.GetLevelMgr().mGauntletSessionLength - this.mGauntletCurTime;
					if (num8 <= 1100 && num8 % 100 == 0)
					{
						this.mGauntletTimeRedAmt = 255f;
					}
				}
				if (this.mGauntletCurTime >= this.mApp.GetLevelMgr().mGauntletSessionLength && this.CurvesAtRest())
				{
					this.mBoard.EndGauntletMode(true);
					bool theAcedLevel = false;
					if (this.mBoard.mScore > this.mChallengeAcePoints)
					{
						theAcedLevel = true;
					}
					GameApp.gApp.ReportEndOfLevelMetrics(this.mBoard, true, theAcedLevel);
				}
			}
			if (this.mGauntletTimeRedAmt > 0f)
			{
				this.mGauntletTimeRedAmt -= ZumasRevenge.Common._M(2.6f);
			}
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00084E10 File Offset: 0x00083010
		public virtual void UpdatePlaying()
		{
			if (this.mCurMultiplierTimeLeft > 0 && --this.mCurMultiplierTimeLeft == 0)
			{
				this.mBoard.GauntletMultiplierEnded();
			}
			if (this.mDoingPadHints)
			{
				if (this.mBoard.mZumaTips.size<ZumaTip>() != 0)
				{
					return;
				}
				this.mDoingPadHints = false;
			}
			for (int i = 0; i < this.mTorches.size<Torch>(); i++)
			{
				this.mTorches[i].Update();
				if (!this.mTorches[i].mActive)
				{
					this.mTorches[i].mOverlayAlpha += ZumasRevenge.Common._M(2);
					if (this.mTorches[i].mOverlayAlpha > 255)
					{
						this.mTorches[i].mOverlayAlpha = 255;
					}
				}
				else
				{
					this.mTorches[i].mOverlayAlpha -= ZumasRevenge.Common._M(2);
					if (this.mTorches[i].mOverlayAlpha < 0)
					{
						this.mTorches[i].mOverlayAlpha = 0;
					}
				}
			}
			if (this.mBoard.GauntletMode())
			{
				float num = (float)this.mNumGauntletBallsBroke / (float)this.mGauntletCurNumForMult;
				float num2 = num - this.mCurGauntletMultPct;
				float num3 = this.mCurGauntletMultPct;
				if (this.mCurGauntletMultPct < num || num2 < -0.001f)
				{
					this.mCurGauntletMultPct += ZumasRevenge.Common._M(0.01f);
					if (num3 < num && this.mCurGauntletMultPct > num)
					{
						this.mCurGauntletMultPct = num;
					}
					else if (this.mCurGauntletMultPct > 1f)
					{
						this.mCurGauntletMultPct = 0f;
					}
				}
			}
			bool flag = this.mHasReachedCruisingSpeed;
			this.mHasReachedCruisingSpeed = true;
			this.mAllCurvesAtRolloutPoint = true;
			ZumasRevenge.Common._M(20f);
			bool flag2 = false;
			if (!this.IsFinalBossLevel() || this.mTorchStageState == 6)
			{
				for (int j = 0; j < this.mNumCurves; j++)
				{
					if (this.mCurveMgr[j].UpdatePlaying() && j + 1 < this.mNumCurves)
					{
						this.mCurveMgr[j + 1].mInitialPathHilite = true;
					}
					if (this.mCurveMgr[j].mSparkles.size<PathSparkle>() > 0)
					{
						flag2 = true;
					}
					if (!this.mCurveMgr[j].HasReachedCruisingSpeed())
					{
						this.mHasReachedCruisingSpeed = false;
					}
					if (!this.mCurveMgr[j].HasReachedRolloutPoint())
					{
						this.mAllCurvesAtRolloutPoint = false;
					}
					if (this.mTempSpeedupTimer == 1)
					{
						this.mCurveMgr[j].mOverrideSpeed = -1f;
					}
					int farthestBallPercent = this.mCurveMgr[j].GetFarthestBallPercent();
					if (farthestBallPercent > this.mFurthestBallDistance)
					{
						this.mFurthestBallDistance = farthestBallPercent;
					}
				}
			}
			if (!flag && this.mHasReachedCruisingSpeed)
			{
				this.mApp.mSoundPlayer.Fade((this.mZone == 5) ? Res.GetSoundByID(ResID.SOUND_UNDERWATER_ROLLOUT) : Res.GetSoundByID(ResID.SOUND_ROLLING));
			}
			if (!ZumasRevenge.Common.gAddBalls && !flag2 && !this.mBoard.mPreventBallAdvancement)
			{
				ZumasRevenge.Common.gAddBalls = true;
				for (int k = 0; k < this.mNumCurves; k++)
				{
					this.mCurveMgr[k].mInitialPathHilite = false;
				}
			}
			if (this.mTempSpeedupTimer > 0)
			{
				this.mTempSpeedupTimer--;
			}
			if (this.mBoard.HasAchievedZuma() && this.mPostZumaTimeCounter > 0)
			{
				this.mPostZumaTimeCounter--;
				float num4 = (float)(this.mApp.GetLevelMgr().mPostZumaTime - this.mPostZumaTimeCounter) / (float)this.mApp.GetLevelMgr().mPostZumaTime;
				this.mPostZumaTimeSlowInc = num4 * this.mApp.GetLevelMgr().mPostZumaTimeSlowInc;
				this.mPostZumaTimeSpeedInc = num4 * this.mApp.GetLevelMgr().mPostZumaTimeSlowInc;
			}
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x000851DA File Offset: 0x000833DA
		public virtual void UpdateBossIntro()
		{
			if (this.mBoss != null)
			{
				this.mBoss.Update();
			}
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x000851F0 File Offset: 0x000833F0
		public virtual void DrawUI(Graphics g)
		{
			if (this.mBoss != null || this.IsFinalBossLevel())
			{
				g.mTransX = 0f;
				this.DrawBossUI(g);
				g.mTransX = (float)this.mApp.mBoardOffsetX;
				return;
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_WOOD);
			this.mBarXOffset = (int)((float)imageByID.mWidth * 0.05f);
			this.DrawWoodPanel(g);
			this.mBoard.DrawRollerScore(g);
			if (this.mBoard.GauntletMode())
			{
				this.DrawGauntletUI(g);
			}
			else
			{
				this.DrawScoreFrame(g);
				this.DrawZumaBar(g);
				this.DrawFredAndGinger(g);
			}
			this.DrawTikiEnds(g);
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00085298 File Offset: 0x00083498
		public virtual void DrawGunPoints(Graphics g)
		{
			if (this.mNumFrogPoints > 1)
			{
				for (int i = 0; i < this.mNumFrogPoints; i++)
				{
					if (this.mFrogImages[i].mImage != null)
					{
						int theCel = ((this.mBoard.mMouseOverGunPos == i) ? 1 : 0);
						g.DrawImageCel(this.mFrogImages[i].mImage, ZumasRevenge.Common._S(this.mFrogX[i]) - this.mFrogImages[i].mImage.GetCelWidth() / 2 + GameApp.gScreenShakeX, ZumasRevenge.Common._S(this.mFrogY[i]) - this.mFrogImages[i].mImage.GetCelHeight() / 2 + GameApp.gScreenShakeY, theCel);
					}
				}
			}
			float num = (1f - this.mTorchDaisScale) * (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(189));
			if (this.IsFinalBossLevel() || this.mTorchStageState >= 10)
			{
				for (int j = 0; j < this.mTorches.size<Torch>(); j++)
				{
					this.mTorches[j].DrawAbove(g);
				}
				if (this.mTorchStageAlpha > 0f)
				{
					g.SetColor(0, 0, 0, (int)Math.Min(255f, this.mTorchStageAlpha));
					g.FillRect(ZumasRevenge.Common._S(-80), 0, GameApp.gApp.mWidth + ZumasRevenge.Common._S(160), GameApp.gApp.mHeight);
				}
				Image imageByID = Res.GetImageByID(ResID.IMAGE_LEVELS_BOSS6PART1_BASE);
				Image imageByID2 = Res.GetImageByID(ResID.IMAGE_LEVELS_BOSS6PART1_DIAS);
				Image imageByID3 = Res.GetImageByID(ResID.IMAGE_LARGE_FROG);
				Image imageByID4 = Res.GetImageByID(ResID.IMAGE_FROG_SHADOW);
				if (this.IsFinalBossLevel())
				{
					g.DrawImage(imageByID, ZumasRevenge.Common._DS(690 - this.mApp.mOffset160X), ZumasRevenge.Common._DS(330));
				}
				string[] array = new string[] { "start", "squish", "rattle" };
				int num2 = imageByID2.mWidth * (int)this.mTorchDaisScale;
				int num3 = imageByID2.mHeight * (int)this.mTorchDaisScale;
				int num4 = ZumasRevenge.Common._DS(793 - this.mApp.mOffset160X);
				int num5 = ZumasRevenge.Common._DS(395);
				num4 += (imageByID2.mWidth - num2) / 2;
				num5 += (imageByID2.mHeight - num3) / 2;
				if (this.mTorchStageState < 9)
				{
					g.DrawImage(imageByID2, num4 + ZumasRevenge.Common._DS(this.mTorchStageShakeAmt), num5 - ZumasRevenge.Common._DS(this.mTorchStageShakeAmt) + (int)num, num2, num3);
				}
				for (int k = 0; k < this.mDaisRocks.size<DaisRock>(); k++)
				{
					DaisRock daisRock = this.mDaisRocks[k];
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)daisRock.mAlpha);
					this.mGlobalTranform.Reset();
					this.mGlobalTranform.Scale(daisRock.mSize, daisRock.mSize);
					float rot = (255f - daisRock.mAlpha) / 255f * ZumasRevenge.Common._M(2.5f) * 3.1415927f;
					this.mGlobalTranform.RotateRad(rot);
					g.DrawImageTransform(daisRock.mImg, this.mGlobalTranform, daisRock.mX, daisRock.mY);
					g.SetColorizeImages(false);
				}
				if (this.mTorchStageState < 4)
				{
					int num6;
					switch (this.mTorchStageState)
					{
					case 0:
						num6 = 2;
						break;
					case 1:
						num6 = 2;
						break;
					default:
						num6 = 2;
						break;
					}
					Composition composition = this.mTorchCompMgr.GetComposition(array[num6]);
					int num7 = composition.mUpdateCount;
					if (num7 >= composition.GetMaxDuration())
					{
						num7 = composition.GetMaxDuration() - 1;
					}
					if (this.mTorchStageState == 0)
					{
						num7 = 1;
					}
					if (num7 == 0)
					{
						num7 = 1;
					}
					CumulativeTransform cumulativeTransform = new CumulativeTransform();
					cumulativeTransform.mTrans.Translate(this.mTorchBossX, this.mTorchBossY);
					if (this.mTorchStageState == 3)
					{
						cumulativeTransform.mTrans.Scale(this.mTorchDaisScale, this.mTorchDaisScale);
						cumulativeTransform.mTrans.Translate(((float)ZumasRevenge.Common._DS(composition.mWidth) - (float)ZumasRevenge.Common._DS(composition.mWidth) * this.mTorchDaisScale) / 1.5f + (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(80)) * (1f - this.mTorchDaisScale), ((float)ZumasRevenge.Common._DS(composition.mHeight) - (float)ZumasRevenge.Common._DS(composition.mHeight) * this.mTorchDaisScale) / ZumasRevenge.Common._M1(1.5f) + num);
					}
					composition.Draw(g, cumulativeTransform, num7, ZumasRevenge.Common._DS(1f));
					Image imageByID5 = Res.GetImageByID(ResID.IMAGE_BOSSES_EGG_ADD);
					Image imageByID6 = Res.GetImageByID(ResID.IMAGE_BOSSES_EGG);
					for (int l = 0; l < this.mEggs.size<TorchLevelEgg>(); l++)
					{
						TorchLevelEgg torchLevelEgg = this.mEggs[l];
						int num8 = (int)(torchLevelEgg.mAlpha * this.mTorchDaisScale);
						if (num8 != 255)
						{
							g.SetColorizeImages(true);
						}
						g.SetColor(255, 255, 255, num8);
						g.SetDrawMode(1);
						g.DrawImageRotated(imageByID5, (int)(torchLevelEgg.mX + (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(-30))), (int)(torchLevelEgg.mY + (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(-30))), (double)torchLevelEgg.mAngle);
						g.SetDrawMode(0);
						g.DrawImageRotated(imageByID6, (int)torchLevelEgg.mX, (int)torchLevelEgg.mY, (double)torchLevelEgg.mAngle);
						g.SetColorizeImages(false);
					}
					return;
				}
				if (this.mTorchStageState == 8 || this.mTorchStageState == 7)
				{
					float num9 = this.mTorchDaisScale * ZumasRevenge.Common._M(0.5f);
					SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
					sexyTransform2D.Scale(this.mTorchDaisScale, this.mTorchDaisScale);
					sexyTransform2D.Translate((float)ZumasRevenge.Common._S(ZumasRevenge.Common._M(-2)), (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M1(3)));
					sexyTransform2D.RotateRad(this.mFrog.GetAngle());
					sexyTransform2D.Translate((float)ZumasRevenge.Common._S(ZumasRevenge.Common._M(2)), (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M1(-3)));
					float num10 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(20f)) * (1f - this.mTorchDaisScale);
					g.DrawImageMatrix(imageByID4, sexyTransform2D, imageByID4.GetCelRect(1), (float)ZumasRevenge.Common._S(this.mFrog.GetCurX()) + (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(-2)) * (1f - this.mTorchDaisScale) + (float)ZumasRevenge.Common._DS(this.mTorchStageShakeAmt), (float)ZumasRevenge.Common._S(this.mFrog.GetCurY()) + num10 + num - (float)ZumasRevenge.Common._DS(this.mTorchStageShakeAmt));
					num10 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(30f)) * (1f - this.mTorchDaisScale);
					sexyTransform2D.LoadIdentity();
					sexyTransform2D.Scale(num9, num9);
					sexyTransform2D.RotateRad(this.mFrog.GetAngle());
					g.DrawImageMatrix(imageByID3, sexyTransform2D, (float)(ZumasRevenge.Common._S(this.mFrog.GetCenterX()) - ZumasRevenge.Common._DS(ZumasRevenge.Common._M(2)) + ZumasRevenge.Common._DS(this.mTorchStageShakeAmt)), (float)(ZumasRevenge.Common._S(this.mFrog.GetCenterY()) - ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(10))) + num10 + num - (float)ZumasRevenge.Common._DS(this.mTorchStageShakeAmt));
					return;
				}
				if (this.mFrogFlyOff != null)
				{
					this.mFrogFlyOff.Draw(g);
				}
			}
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00085A1C File Offset: 0x00083C1C
		public void DrawBossUI(Graphics g)
		{
			GameApp gApp = GameApp.gApp;
			if (gApp.mBoard != null && !gApp.mBoard.mDrawBossUI)
			{
				return;
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_BOSSUI);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_RIGHTFRAMESIDE);
			if (gApp.IsWideScreen())
			{
				g.DrawImage(imageByID, (int)((double)gApp.GetScreenRect().mWidth - (double)imageByID.GetWidth() * 1.5 - (double)imageByID2.GetWidth()), 0, (int)((float)imageByID.GetWidth() * 1.5f), imageByID.GetHeight());
				g.DrawImage(imageByID2, gApp.GetWideScreenAdjusted(ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_INGAME_UI_RIGHTFRAMESIDE))) - (gApp.GetScreenWidth() - gApp.mScreenBounds.mWidth), ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_GUI_INGAME_UI_RIGHTFRAMESIDE)));
				return;
			}
			g.DrawImage(imageByID, (int)((double)gApp.GetScreenRect().mWidth - (double)imageByID.GetWidth() * 1.5), 0, (int)((double)imageByID.GetWidth() * 1.5), imageByID.GetHeight());
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00085B28 File Offset: 0x00083D28
		public void DrawWoodPanel(Graphics g)
		{
			int num = ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_INGAME_UI_WOOD));
			int theY = ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_GUI_INGAME_UI_WOOD));
			int x = num - this.mBarXOffset;
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_WOOD);
			g.DrawImage(imageByID, GameApp.gApp.GetWideScreenAdjusted(x), theY);
			g.DrawImageMirror(imageByID, GameApp.gApp.GetWideScreenAdjusted(this.mBarXOffset), theY);
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x00085B94 File Offset: 0x00083D94
		public void DrawScoreFrame(Graphics g)
		{
			int num = ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_INGAME_UI_SCORE_FRAME)) + this.mBarXOffset;
			int theY = ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_GUI_INGAME_UI_SCORE_FRAME));
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_SCORE_FRAME);
			if (!GameApp.gApp.IsWideScreen())
			{
				num -= ZumasRevenge.Common._S(10);
			}
			g.DrawImage(imageByID, GameApp.gApp.GetWideScreenAdjusted(num), theY);
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x00085BFC File Offset: 0x00083DFC
		public void DrawTikiEnds(Graphics g)
		{
			if (!GameApp.gApp.IsWideScreen())
			{
				return;
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_LEFTFRAMESIDE);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_RIGHTFRAMESIDE);
			g.DrawImage(imageByID, GameApp.gApp.GetWideScreenAdjusted(0), 0);
			g.DrawImage(imageByID2, GameApp.gApp.GetWidthAdjusted(ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_INGAME_UI_RIGHTFRAMESIDE))) - (GameApp.gApp.GetScreenWidth() - GameApp.gApp.mScreenBounds.mWidth), 0);
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x00085C7C File Offset: 0x00083E7C
		public void DrawZumaBar(Graphics g)
		{
			if (this.mTimer >= 0)
			{
				return;
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_PROGRESSLITEWOOD);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_GUI_PROGRESS_LIGHT);
			Image imageByID3 = Res.GetImageByID(ResID.IMAGE_GUI_PROGRESS_TOP);
			g.DrawImage(imageByID, this.mZumaBarX, ZumasRevenge.Common._S(9));
			this.SetZumaBarProgress();
			if (this.mZumaBarState < 2)
			{
				return;
			}
			this.DrawZumaBarProgress(g, imageByID2);
			this.DrawZumaBarProgressPulse(g);
			this.DrawZumaBarProgress(g, imageByID3);
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00085CF0 File Offset: 0x00083EF0
		public void DrawFredAndGinger(Graphics g)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_CONNECT_BAR);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_LEFT_JAW);
			Image imageByID3 = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_RIGHT_JAW);
			Image imageByID4 = Res.GetImageByID(ResID.IMAGE_GUI_PROGRESS_TOP);
			Image imageByID5 = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_LEFT_MOUTH_LOWER);
			Image imageByID6 = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_UI_RIGHT_MOUTH_UPPER);
			g.DrawImage(imageByID, GameApp.gApp.GetWideScreenAdjusted(ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_INGAME_UI_CONNECT_BAR)) - this.mBarXOffset), ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_GUI_INGAME_UI_CONNECT_BAR)), imageByID4.GetWidth(), imageByID.GetHeight());
			g.DrawImage(imageByID2, (int)this.mGingerMouthX + this.mBarXOffset, ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_GUI_INGAME_UI_LEFT_JAW)) - ZumasRevenge.Common._S(3));
			g.DrawImage(imageByID3, (int)this.mFredMouthX - this.mBarXOffset, ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_GUI_INGAME_UI_RIGHT_JAW)) - ZumasRevenge.Common._S(2));
			this.DrawGoldBall(g);
			int x = ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_INGAME_UI_LEFT_MOUTH_LOWER)) + this.mBarXOffset;
			int x2 = ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_GUI_INGAME_UI_RIGHT_MOUTH_UPPER)) - this.mBarXOffset;
			g.DrawImage(imageByID5, GameApp.gApp.GetWideScreenAdjusted(x), ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_GUI_INGAME_UI_LEFT_MOUTH_LOWER)));
			g.DrawImage(imageByID6, GameApp.gApp.GetWideScreenAdjusted(x2), ZumasRevenge.Common._DS(Res.GetOffsetYByID(ResID.IMAGE_GUI_INGAME_UI_RIGHT_MOUTH_UPPER)));
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00085E58 File Offset: 0x00084058
		public void SetZumaBarProgress()
		{
			int num = Res.GetImageByID(ResID.IMAGE_GUI_PROGRESS_TOP).mWidth - this.mBarXOffset * 2;
			if (this.mZumaBarState >= 7 && this.mZumaBarState < 14)
			{
				this.mZumaBarWidth = num;
			}
			else
			{
				this.mZumaBarWidth = (int)((float)num * (float)this.mCurBarSize / 330f + (float)ZumasRevenge.Common._S(8));
			}
			this.mZumaBarWidth = Math.Min(this.mZumaBarWidth, num);
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00085ECC File Offset: 0x000840CC
		public void DrawZumaBarProgress(Graphics g, Image inImage)
		{
			g.DrawImage(inImage, this.mZumaBarX, ZumasRevenge.Common._S(9), new Rect(0, 0, this.mZumaBarWidth, inImage.mHeight));
			if (this.mZumaBarState < 12 || this.mZumaBarState > 13 || this.mBarLightness <= 0f)
			{
				return;
			}
			g.SetColorizeImages(true);
			g.SetDrawMode(1);
			g.SetColor(255, 255, 255, (int)this.mBarLightness);
			g.DrawImage(inImage, this.mZumaBarX, ZumasRevenge.Common._S(9), new Rect(0, 0, this.mZumaBarWidth, inImage.mHeight));
			g.SetColorizeImages(false);
			g.SetDrawMode(0);
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x00085F84 File Offset: 0x00084184
		public void DrawZumaBarProgressPulse(Graphics g)
		{
			if (this.mZumaBarState < 14)
			{
				return;
			}
			g.PushState();
			g.SetDrawMode(1);
			int num = ZumasRevenge.Common._M(0) + JeffLib.Common.GetAlphaFromUpdateCount(this.mUpdateCount - this.mZumaPulseUCStart, ZumasRevenge.Common._M1(255));
			if (num > 255)
			{
				num = 255;
			}
			g.SetColorizeImages(true);
			g.SetColor(255, 255, 255, num);
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_PROGRESS_LIGHT);
			g.DrawImage(imageByID, this.mZumaBarX, ZumasRevenge.Common._S(9), new Rect(0, 0, this.mZumaBarWidth, imageByID.mHeight));
			g.PopState();
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00086030 File Offset: 0x00084230
		public void DrawGoldBall(Graphics g)
		{
			if (this.mTimer >= 0 || this.mZumaBarState >= 8 || this.mZumaBallPct <= 0f)
			{
				return;
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_GOLD_BALL);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_GUI_PROGRESS_TOP);
			int theWidth = (int)((float)imageByID.GetCelHeight() * this.mZumaBallPct);
			int num = (int)((float)imageByID.GetCelWidth() * this.mZumaBallPct);
			int num2 = this.mZumaBarX - ZumasRevenge.Common._S(20) + this.mZumaBarWidth - imageByID.mHeight / 2 + ZumasRevenge.Common._S((int)this.mGoldBallXOff);
			if (num2 < this.mZumaBarX)
			{
				num2 = this.mZumaBarX;
			}
			Rect theDestRect = new Rect(num2, ZumasRevenge.Common._S(9) + (imageByID2.mHeight - num) / 2, theWidth, num);
			g.DrawImageCel(imageByID, theDestRect, this.mZumaBallFrame);
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00086100 File Offset: 0x00084300
		public virtual int GetFarthestBallPercent(ref int farthest_curve, bool ignore_gaps)
		{
			int num = 0;
			for (int i = 0; i < this.mNumCurves; i++)
			{
				int farthestBallPercent = this.mCurveMgr[i].GetFarthestBallPercent(ignore_gaps);
				if (farthestBallPercent > num)
				{
					farthest_curve = i;
					num = farthestBallPercent;
				}
			}
			return num;
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0008613C File Offset: 0x0008433C
		public virtual int GetFarthestBallPercent()
		{
			int num = 0;
			return this.GetFarthestBallPercent(ref num, true);
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x00086154 File Offset: 0x00084354
		public virtual void NukeEffects()
		{
			for (int i = 0; i < this.mEffects.size<Effect>(); i++)
			{
				this.mEffects[i].DeleteResources();
			}
			this.mEffects.Clear();
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x00086194 File Offset: 0x00084394
		public virtual void BulletFired(Bullet b)
		{
			for (int i = 0; i < Enumerable.Count<Effect>(this.mEffects); i++)
			{
				this.mEffects[i].BulletFired(b);
			}
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x000861CC File Offset: 0x000843CC
		public virtual void BulletHit(Bullet b)
		{
			for (int i = 0; i < Enumerable.Count<Effect>(this.mEffects); i++)
			{
				this.mEffects[i].BulletHit(b);
			}
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00086204 File Offset: 0x00084404
		public virtual void ReactivateWalls(int wall_id)
		{
			for (int i = 0; i < Enumerable.Count<Wall>(this.mWalls); i++)
			{
				if (this.mWalls[i].mId == wall_id || wall_id == -1)
				{
					this.mWalls[i].mStrength = this.mWalls[i].mOrgStrength;
				}
			}
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x00086261 File Offset: 0x00084461
		public virtual void ReactivateWalls()
		{
			this.ReactivateWalls(-1);
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0008626C File Offset: 0x0008446C
		public virtual bool CompactCurves()
		{
			if (!this.CanCompactCurves())
			{
				return false;
			}
			for (int i = 0; i < this.mNumCurves; i++)
			{
				this.mCurveMgr[i].CompactCurve();
			}
			return true;
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x000862A4 File Offset: 0x000844A4
		public virtual bool CanCompactCurves()
		{
			for (int i = 0; i < this.mNumCurves; i++)
			{
				if (!this.mCurveMgr[i].CanCompact())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x000862D4 File Offset: 0x000844D4
		public virtual void SetupHiddenHoles()
		{
			if (this.mHoleMgr.GetNumHoles() < 2)
			{
				return;
			}
			int num = 0;
			HoleInfo hole;
			while ((hole = this.mHoleMgr.GetHole(num)) != null)
			{
				if (!hole.mVisible)
				{
					int num2 = 0;
					Rect theTRect = new Rect(hole.mX, hole.mY, 96, 96);
					theTRect.Inflate(-4, -4);
					HoleInfo hole2;
					while ((hole2 = this.mHoleMgr.GetHole(num2)) != null)
					{
						if (!hole2.mVisible)
						{
							num2++;
						}
						else
						{
							Rect rect = new Rect(hole2.mX, hole2.mY, 96, 96);
							rect.Inflate(-4, -4);
							if (rect.Intersects(theTRect))
							{
								hole.mShared.Add(num2);
							}
							num2++;
						}
					}
				}
				num++;
			}
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0008639E File Offset: 0x0008459E
		public virtual void PlayerLostLevel()
		{
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x000863A0 File Offset: 0x000845A0
		public void DeactivateLightningEffects()
		{
			for (int i = 0; i < this.mNumCurves; i++)
			{
				this.mCurveMgr[i].ElectrifyBalls(-1, false);
			}
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x000863D0 File Offset: 0x000845D0
		public bool HasPowerup(PowerType p)
		{
			for (int i = 0; i < this.mNumCurves; i++)
			{
				if (this.mCurveMgr[i].HasPowerup(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00086401 File Offset: 0x00084601
		public virtual int GetRandomPendingBallColor(int max_curve_colors)
		{
			return MathUtils.SafeRand() % max_curve_colors;
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0008640A File Offset: 0x0008460A
		public virtual float GetRandomFrogBulletColor(int max_curve_colors, int color_num)
		{
			return 1f / (float)max_curve_colors;
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x00086414 File Offset: 0x00084614
		public virtual Ball GetBallAtXY(int x, int y)
		{
			for (int i = 0; i < this.mNumCurves; i++)
			{
				foreach (Ball ball in this.mCurveMgr[i].mBallList)
				{
					if (ball.Contains(x, y))
					{
						return ball;
					}
				}
			}
			return null;
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0008648C File Offset: 0x0008468C
		public Ball GetRandomBall()
		{
			int num = MathUtils.SafeRand() % this.mNumCurves;
			if (this.mCurveMgr[num].mBallList.Count > 3)
			{
				int num2 = MathUtils.SafeRand() % (this.mCurveMgr[num].mBallList.Count - 2);
				Ball ball = this.mCurveMgr[num].mBallList[num2];
				if (ball.GetPowerType() == PowerType.PowerType_Max)
				{
					return ball;
				}
			}
			return null;
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x000864F9 File Offset: 0x000846F9
		public virtual void ParseUnknownAttribute(string key, string val)
		{
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x000864FC File Offset: 0x000846FC
		public virtual void CopyFrom(Level src)
		{
			for (int i = 0; i < this.mHoleMgr.GetNumHoles(); i++)
			{
				HoleInfo hole = this.mHoleMgr.GetHole(i);
				hole.mCurve = this.mCurveMgr[hole.mCurveNum];
			}
			this.mApp = GameApp.gApp;
			this.mBoard = this.mApp.GetBoard();
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0008655C File Offset: 0x0008475C
		public int GetTotalBallsOnLevel()
		{
			int num = 0;
			for (int i = 0; i < this.mNumCurves; i++)
			{
				num += Enumerable.Count<Ball>(this.mCurveMgr[i].mBallList);
				num += Enumerable.Count<Ball>(this.mCurveMgr[i].mPendingBalls);
			}
			return num;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x000865A8 File Offset: 0x000847A8
		public int GetMaxBallsForLevel()
		{
			int num = 0;
			for (int i = 0; i < this.mNumCurves; i++)
			{
				num += this.mCurveMgr[i].mCurveDesc.mVals.mNumBalls;
			}
			return num;
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x000865E4 File Offset: 0x000847E4
		public bool CheckFruitActivation(int curve_num)
		{
			if (this.mBoard.mPreventBallAdvancement)
			{
				return false;
			}
			int num = (this.mBoard.GauntletMode() ? this.mApp.GetLevelMgr().mGauntletTFreq : this.mTreasureFreq);
			if (!Board.gForceTreasure && (this.mBoard.mCurTreasure != null || MathUtils.SafeRand() % num != 0))
			{
				return false;
			}
			List<int> list = new List<int>();
			int num2;
			int num3;
			if (curve_num == -1)
			{
				num2 = 0;
				num3 = this.mNumCurves;
			}
			else
			{
				num3 = curve_num;
				num2 = curve_num;
			}
			for (int i = num2; i < num3; i++)
			{
				int farthestBallPercent = this.mCurveMgr[i].GetFarthestBallPercent();
				for (int j = 0; j < Enumerable.Count<TreasurePoint>(this.mTreasurePoints); j++)
				{
					TreasurePoint treasurePoint = this.mTreasurePoints[j];
					if (treasurePoint.mCurveDist[i] > 0 && farthestBallPercent >= treasurePoint.mCurveDist[i])
					{
						list.Add(j);
					}
				}
			}
			if (Enumerable.Count<int>(list) == 0)
			{
				return false;
			}
			int num4 = MathUtils.SafeRand() % Enumerable.Count<int>(list);
			this.mBoard.mCurTreasureNum = list[num4];
			this.mBoard.mCurTreasure = this.mTreasurePoints[list[num4]];
			this.mBoard.mMinTreasureY = (this.mBoard.mMaxTreasureY = float.MaxValue);
			return true;
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00086734 File Offset: 0x00084934
		public bool CurvesAtRest()
		{
			if (this.mBoard.HasFiredBullets() || this.mBoard.GetGun().IsFiring())
			{
				return false;
			}
			for (int i = 0; i < this.mNumCurves; i++)
			{
				if (!this.mCurveMgr[i].AtRest())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00086785 File Offset: 0x00084985
		public virtual void MadeCombo(int combo_size)
		{
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x00086787 File Offset: 0x00084987
		public virtual void MadeGapShot(int gap_size)
		{
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00086789 File Offset: 0x00084989
		public virtual void MadeConsecutiveClear(int clear_size)
		{
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0008678B File Offset: 0x0008498B
		public virtual void ClearedInARowBonus()
		{
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0008678D File Offset: 0x0008498D
		public virtual void AllBallsDestroyed()
		{
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0008678F File Offset: 0x0008498F
		public virtual void BallExploded(int ball_type)
		{
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x00086791 File Offset: 0x00084991
		public virtual bool ShouldUpdateZumaBar()
		{
			return true;
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00086794 File Offset: 0x00084994
		public virtual bool AllowPointsFromBalls()
		{
			return true;
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00086797 File Offset: 0x00084997
		public virtual bool CanAdvanceBalls()
		{
			return this.mBoss == null || this.mBoss.CanAdvanceBalls();
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x000867AE File Offset: 0x000849AE
		public virtual bool BeatLevelOverride()
		{
			return false;
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x000867B4 File Offset: 0x000849B4
		public virtual void TemporarilySpeedUpCurves(float max_speed, int time_count)
		{
			this.mTempSpeedupTimer = time_count;
			for (int i = 0; i < this.mNumCurves; i++)
			{
				this.mCurveMgr[i].mOverrideSpeed = max_speed;
			}
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x000867E7 File Offset: 0x000849E7
		public virtual void BallCreatedCallback(Ball b, int num_created)
		{
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x000867E9 File Offset: 0x000849E9
		public virtual void MouseDown(int x, int y, int cc)
		{
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x000867EB File Offset: 0x000849EB
		public virtual void ChangedPad(int new_pad)
		{
			if (!this.mDoingPadHints)
			{
				return;
			}
			this.mBoard.mZumaTips[0] = null;
			this.mBoard.mZumaTips.RemoveAt(0);
			this.mBoard.MarkDirty();
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x00086824 File Offset: 0x00084A24
		public virtual int GetFrogReloadType()
		{
			if (!this.mApp.mUserProfile.HasSeenHint(ZumaProfile.FIRST_SHOT_HINT) && !this.mBoard.GauntletMode() && this.mNum == 1 && this.mZone == 1)
			{
				return 2;
			}
			if (this.mBoss != null)
			{
				return this.mBoss.GetFrogReloadType();
			}
			return -1;
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0008687E File Offset: 0x00084A7E
		public virtual void PlayerStartedFiring()
		{
			if (this.mBoss != null)
			{
				this.mBoss.PlayerStartedFiring();
			}
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x00086894 File Offset: 0x00084A94
		public float GetPowerIncPct()
		{
			if (this.mBoard.IronFrogMode() || this.mBoard.GauntletMode())
			{
				return 0f;
			}
			float num = (float)this.mCurBarSize / 330f;
			if (num >= this.mApp.GetLevelMgr().mPowerupIncAtZumaPct)
			{
				return this.mApp.GetLevelMgr().mPowerIncPct;
			}
			return 0f;
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x000868F8 File Offset: 0x00084AF8
		public void IncNumBallsExploded(int val)
		{
			this.mApp.mUserProfile.mBallsBroken++;
			if (!this.mBoard.GauntletMode())
			{
				return;
			}
			this.mNumGauntletBallsBroke += val;
			if (this.mNumGauntletBallsBroke >= this.mGauntletCurNumForMult)
			{
				this.mNumGauntletBallsBroke %= this.mGauntletCurNumForMult;
				int num = SexyFramework.Common.Rand() % this.mNumCurves;
				this.mCurveMgr[num].mNumMultBallsToSpawn++;
				this.mGauntletMultipliersEarned++;
			}
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0008698A File Offset: 0x00084B8A
		public virtual bool CanUpdate()
		{
			return true;
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00086990 File Offset: 0x00084B90
		public int GetOwningCurve(Ball b)
		{
			for (int i = 0; i < this.mNumCurves; i++)
			{
				if (this.mCurveMgr[i].HasBall(b))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x000869C4 File Offset: 0x00084BC4
		public void UpdateEffects()
		{
			for (int i = 0; i < this.mEffects.size<Effect>(); i++)
			{
				this.mEffects[i].Update();
			}
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x000869F8 File Offset: 0x00084BF8
		public virtual bool CanRotateFrog()
		{
			return this.mEndSequence != 2 || ((this.mTorchStageState == 13 || this.mTorchStageState == -1) && this.mBoss.GetHP() > 0f);
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00086A2C File Offset: 0x00084C2C
		public virtual bool CanFireBall()
		{
			int num = 0;
			int num2 = 0;
			while (num2 < this.mNumCurves && this.mCurveMgr[num2].IsWinning())
			{
				num2++;
				num++;
			}
			return (!this.mBoard.GauntletMode() || this.mGauntletCurTime < this.mApp.GetLevelMgr().mGauntletSessionLength) && num != this.mNumCurves && (!this.mDoTorchCrap || this.mHasDoneTorchCrap) && (this.mApp.mUserProfile.HasSeenHint(ZumaProfile.FIRST_SHOT_HINT) || ((this.HasReachedCruisingSpeed() || this.mBoard.GauntletMode() || (this.mBoss != null && this.mBoss.AllowFrogToFire())) && (Enumerable.Count<ZumaTip>(this.mBoard.mZumaTips) == 0 || (this.mBoard.mZumaTips[0].mId == ZumaProfile.FIRST_SHOT_HINT && this.mFrog.GetAngle() >= ZumasRevenge.Common._M(4.504f) && this.mFrog.GetAngle() <= ZumasRevenge.Common._M1(4.9049f)))));
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00086B4A File Offset: 0x00084D4A
		public virtual bool CanUseKeyboard()
		{
			return true;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00086B4D File Offset: 0x00084D4D
		public virtual bool CanSwapBalls()
		{
			return true;
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00086B50 File Offset: 0x00084D50
		public virtual Level Instantiate()
		{
			Level level = this.Clone();
			level.mHoleMgr = null;
			return level;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00086B6C File Offset: 0x00084D6C
		public virtual void SetFrog(Gun g)
		{
			this.mFrog = g;
			if (this.mBoss != null)
			{
				this.mBoss.FrogInitialized(g);
			}
			if (this.mSecondaryBoss != null)
			{
				this.mSecondaryBoss.FrogInitialized(g);
			}
			if (this.mMoveType != 0 && this.mCurveMgr[0] != null)
			{
				int endPoint = this.mCurveMgr[0].mWayPointMgr.GetEndPoint();
				float num;
				float num2;
				this.mCurveMgr[0].GetXYFromWaypoint(endPoint, out num, out num2);
				if (this.mMoveType == 1)
				{
					if (num2 < (float)g.GetCenterY())
					{
						g.SetDestAngle(-3.14159f);
						return;
					}
					g.SetDestAngle(0f);
					return;
				}
				else
				{
					if (num < (float)g.GetCenterX())
					{
						g.SetDestAngle(-1.570795f);
						return;
					}
					g.SetDestAngle(1.570795f);
				}
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00086C2C File Offset: 0x00084E2C
		public virtual void SyncState(DataSync sync)
		{
			this.SyncWalls(sync, true);
			bool flag = this.mBoss != null;
			bool flag2 = this.mBoss == this.mSecondaryBoss;
			sync.SyncBoolean(ref flag);
			sync.SyncBoolean(ref flag2);
			sync.SyncLong(ref this.mInvertMouseTimer);
			sync.SyncBoolean(ref this.mCanDrawBoss);
			if (flag)
			{
				bool flag3 = false;
				if (this.mBoard.ShouldBypassFinalSequenceOnLoad())
				{
					flag3 = true;
				}
				sync.SyncBoolean(ref flag3);
				if (!flag3)
				{
					if (sync.isWrite())
					{
						this.mBoss.SyncState(sync);
					}
					else
					{
						if (flag2)
						{
							this.mBoss = this.mSecondaryBoss;
						}
						this.mBoss.SyncState(sync);
					}
				}
			}
			sync.SyncFloat(ref this.mTorchDaisScale);
			sync.SyncLong(ref this.mTorchStageState);
			sync.SyncLong(ref this.mTorchStageTimer);
			sync.SyncFloat(ref this.mTorchStageAlpha);
			if (sync.isRead())
			{
				if (this.mTorchStageState >= 9 && this.mTorchStageState < 13)
				{
					this.mTorchStageState = 13;
					this.mBoard.mPreventBallAdvancement = false;
					this.mTorchStageTimer = 0;
					this.mTorchDaisScale = 0f;
					this.mCanDrawBoss = true;
				}
				this.mTorches.Clear();
				SexyBuffer buffer = sync.GetBuffer();
				int num = (int)buffer.ReadLong();
				for (int i = 0; i < num; i++)
				{
					Torch torch = new Torch();
					torch.SyncState(sync);
					this.mTorches.Add(torch);
				}
				if (this.mTorchStageState != -1 && this.mTorchStageState < 6)
				{
					this.InitFinalBossLevel();
				}
				else if (this.mTorchStageState == 6)
				{
					this.mBoard.mPreventBallAdvancement = false;
					this.mDoTorchCrap = false;
					this.mHasDoneTorchCrap = true;
					this.mTorchDaisScale = 1f;
					this.mTorchTextAlpha = 0f;
				}
			}
			else
			{
				SexyBuffer buffer2 = sync.GetBuffer();
				buffer2.WriteLong((long)this.mTorches.Count);
				for (int j = 0; j < this.mTorches.Count; j++)
				{
					this.mTorches[j].SyncState(sync);
				}
			}
			sync.SyncBoolean(ref this.mDoTorchCrap);
			sync.SyncBoolean(ref this.mHasDoneTorchCrap);
			sync.SyncFloat(ref this.mTorchTextAlpha);
			sync.SyncLong(ref this.mFurthestBallDistance);
			sync.SyncLong(ref this.mCurFrogPoint);
			sync.SyncLong(ref this.mTempSpeedupTimer);
			sync.SyncBoolean(ref this.mHaveReachedTarget);
			sync.SyncFloat(ref this.mBarLightness);
			sync.SyncFloat(ref this.mZumaBallPct);
			sync.SyncLong(ref this.mZumaBarState);
			sync.SyncFloat(ref this.mGoldBallXOff);
			sync.SyncFloat(ref this.mGingerMouthX);
			sync.SyncFloat(ref this.mGingerMouthVX);
			sync.SyncFloat(ref this.mFredMouthX);
			sync.SyncFloat(ref this.mFredMouthVX);
			sync.SyncFloat(ref this.mFredTongueX);
			sync.SyncFloat(ref this.mFredTongueVX);
			sync.SyncLong(ref this.mCurBarSize);
			sync.SyncLong(ref this.mTargetBarSize);
			this.SyncWalls(sync, true);
			for (int k = 0; k < this.mNumCurves; k++)
			{
				this.mCurveMgr[k].SyncState(sync);
			}
			sync.SyncBoolean(ref this.m_canGetAchievementNoMove);
			sync.SyncBoolean(ref this.m_canGetAchievementNoJump);
			sync.SyncLong(ref this.m_OriginX);
			sync.SyncLong(ref this.m_OriginY);
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00086F7C File Offset: 0x0008517C
		private void SyncWalls(DataSync sync, bool clear)
		{
			if (sync.isRead())
			{
				if (clear)
				{
					this.mWalls.Clear();
				}
				long num = sync.GetBuffer().ReadLong();
				int num2 = 0;
				while ((long)num2 < num)
				{
					Wall wall = new Wall();
					wall.SyncState(sync);
					this.mWalls.Add(wall);
					num2++;
				}
				return;
			}
			sync.GetBuffer().WriteLong((long)this.mWalls.Count);
			foreach (Wall wall2 in this.mWalls)
			{
				wall2.SyncState(sync);
			}
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00087030 File Offset: 0x00085230
		public bool AllCurvesAtRolloutPoint()
		{
			return this.mAllCurvesAtRolloutPoint;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00087038 File Offset: 0x00085238
		public bool HasReachedCruisingSpeed()
		{
			return this.mHasReachedCruisingSpeed;
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00087040 File Offset: 0x00085240
		public float GetBarPercent()
		{
			return (float)this.mCurBarSize / (float)this.mTargetBarSize;
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00087054 File Offset: 0x00085254
		public int GetBossBombDelay()
		{
			if (this.mBoss == null)
			{
				return 0;
			}
			BossShoot bossShoot = this.mBoss as BossShoot;
			if (bossShoot == null)
			{
				return 0;
			}
			return bossShoot.mBombAppearDelay;
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x00087082 File Offset: 0x00085282
		public void ProximityBombActivated(float x, float y, int radius)
		{
			if (this.mBoss != null && this.mBoss.IsHitByExplosion(x, y, radius))
			{
				this.mBoss.ProximityBombActivated(x, y, radius);
			}
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x000870AC File Offset: 0x000852AC
		public void UserDied()
		{
			for (int i = 0; i < this.mEffects.size<Effect>(); i++)
			{
				this.mEffects[i].UserDied();
			}
		}

		// Token: 0x04000C1B RID: 3099
		public const int TARGET_BAR_SIZE = 330;

		// Token: 0x04000C1C RID: 3100
		public const int FRED_TONGUE_X = 541;

		// Token: 0x04000C1D RID: 3101
		public const int STARTING_TORCH_TEXT_ALPHA = 700;

		// Token: 0x04000C1E RID: 3102
		protected float[] mCloakedBossTextAlpha = new float[3];

		// Token: 0x04000C1F RID: 3103
		public List<DaisRock> mDaisRocks = new List<DaisRock>();

		// Token: 0x04000C20 RID: 3104
		public List<TorchLevelEgg> mEggs = new List<TorchLevelEgg>();

		// Token: 0x04000C21 RID: 3105
		public List<Wall> mMovingWallDefaults = new List<Wall>();

		// Token: 0x04000C22 RID: 3106
		public List<Effect> mEffects = new List<Effect>();

		// Token: 0x04000C23 RID: 3107
		protected bool mAllCurvesAtRolloutPoint;

		// Token: 0x04000C24 RID: 3108
		protected bool mHasReachedCruisingSpeed;

		// Token: 0x04000C25 RID: 3109
		protected float mCurGauntletMultPct;

		// Token: 0x04000C26 RID: 3110
		protected Transform mGlobalTranform = new Transform();

		// Token: 0x04000C27 RID: 3111
		private CompositionMgr mTorchCompMgr;

		// Token: 0x04000C28 RID: 3112
		public float mTorchBossX;

		// Token: 0x04000C29 RID: 3113
		public float mTorchBossY;

		// Token: 0x04000C2A RID: 3114
		public float mTorchBossDestX;

		// Token: 0x04000C2B RID: 3115
		public float mTorchBossDestY;

		// Token: 0x04000C2C RID: 3116
		public float mTorchBossVX;

		// Token: 0x04000C2D RID: 3117
		public float mTorchBossVY;

		// Token: 0x04000C2E RID: 3118
		public float mTorchDaisScale;

		// Token: 0x04000C2F RID: 3119
		public int mChallengePoints;

		// Token: 0x04000C30 RID: 3120
		public int mChallengeAcePoints;

		// Token: 0x04000C31 RID: 3121
		public int mCloakClapFrame;

		// Token: 0x04000C32 RID: 3122
		public PIEffect mCloakPoof;

		// Token: 0x04000C33 RID: 3123
		public FrogFlyOff mFrogFlyOff;

		// Token: 0x04000C34 RID: 3124
		public List<PowerupRegion> mPowerupRegions = new List<PowerupRegion>();

		// Token: 0x04000C35 RID: 3125
		public List<Torch> mTorches = new List<Torch>();

		// Token: 0x04000C36 RID: 3126
		public List<string> mEffectNames = new List<string>();

		// Token: 0x04000C37 RID: 3127
		public List<EffectParams> mEffectParams = new List<EffectParams>();

		// Token: 0x04000C38 RID: 3128
		public List<TreasurePoint> mTreasurePoints = new List<TreasurePoint>();

		// Token: 0x04000C39 RID: 3129
		public string mId = "";

		// Token: 0x04000C3A RID: 3130
		public string mDisplayName = "";

		// Token: 0x04000C3B RID: 3131
		public int mDisplayNameId = -1;

		// Token: 0x04000C3C RID: 3132
		public string mPopupText = "";

		// Token: 0x04000C3D RID: 3133
		public string mImagePath = "";

		// Token: 0x04000C3E RID: 3134
		public string mSoundscapeId = "";

		// Token: 0x04000C3F RID: 3135
		public MirrorType mMirrorType;

		// Token: 0x04000C40 RID: 3136
		public CurveMgr[] mCurveMgr = new CurveMgr[4];

		// Token: 0x04000C41 RID: 3137
		public float[] mCurveSkullAngleOverrides = new float[4];

		// Token: 0x04000C42 RID: 3138
		public HoleMgr mHoleMgr;

		// Token: 0x04000C43 RID: 3139
		public List<TunnelData> mTunnelData = new List<TunnelData>();

		// Token: 0x04000C44 RID: 3140
		public List<Wall> mWalls = new List<Wall>();

		// Token: 0x04000C45 RID: 3141
		public Boss mBoss;

		// Token: 0x04000C46 RID: 3142
		public Boss mSecondaryBoss;

		// Token: 0x04000C47 RID: 3143
		public Boss mOrgBoss;

		// Token: 0x04000C48 RID: 3144
		public Gun mFrog;

		// Token: 0x04000C49 RID: 3145
		public Board mBoard;

		// Token: 0x04000C4A RID: 3146
		public GameApp mApp;

		// Token: 0x04000C4B RID: 3147
		public SharedImageRef mBossIntroBG;

		// Token: 0x04000C4C RID: 3148
		public string mPreviewText = "";

		// Token: 0x04000C4D RID: 3149
		public int mPreviewTextId = -1;

		// Token: 0x04000C4E RID: 3150
		public LillyPadImageInfo[] mFrogImages = new LillyPadImageInfo[5];

		// Token: 0x04000C4F RID: 3151
		public bool mCanDrawBoss;

		// Token: 0x04000C50 RID: 3152
		public int mTorchStageState;

		// Token: 0x04000C51 RID: 3153
		public int mTorchStageTimer;

		// Token: 0x04000C52 RID: 3154
		public float mTorchStageAlpha;

		// Token: 0x04000C53 RID: 3155
		public int mTorchStageShakeAmt;

		// Token: 0x04000C54 RID: 3156
		public int mEndSequence;

		// Token: 0x04000C55 RID: 3157
		public int mIndex;

		// Token: 0x04000C56 RID: 3158
		public bool mOffscreenClearBonus;

		// Token: 0x04000C57 RID: 3159
		public bool mNoBackground;

		// Token: 0x04000C58 RID: 3160
		public bool mFinalLevel;

		// Token: 0x04000C59 RID: 3161
		public bool mBGFromPSD;

		// Token: 0x04000C5A RID: 3162
		public float mPotPct;

		// Token: 0x04000C5B RID: 3163
		public float mFireSpeed;

		// Token: 0x04000C5C RID: 3164
		public float mHurryToRolloutAmt;

		// Token: 0x04000C5D RID: 3165
		public bool mDoTorchCrap;

		// Token: 0x04000C5E RID: 3166
		public bool mHasDoneTorchCrap;

		// Token: 0x04000C5F RID: 3167
		public float mTorchTextAlpha;

		// Token: 0x04000C60 RID: 3168
		public bool mDrawCurves;

		// Token: 0x04000C61 RID: 3169
		public bool mSuckMode;

		// Token: 0x04000C62 RID: 3170
		public bool mIsEndless;

		// Token: 0x04000C63 RID: 3171
		public bool mLoopAtEnd;

		// Token: 0x04000C64 RID: 3172
		public bool mDoingPadHints;

		// Token: 0x04000C65 RID: 3173
		public bool mNoFlip;

		// Token: 0x04000C66 RID: 3174
		public bool mSliderEdgeRotate;

		// Token: 0x04000C67 RID: 3175
		public bool mIronFrog;

		// Token: 0x04000C68 RID: 3176
		public int mReloadDelay;

		// Token: 0x04000C69 RID: 3177
		public int mNumCurves;

		// Token: 0x04000C6A RID: 3178
		public int mNumFrogPoints;

		// Token: 0x04000C6B RID: 3179
		public int mCurFrogPoint;

		// Token: 0x04000C6C RID: 3180
		public int[] mFrogX = new int[5];

		// Token: 0x04000C6D RID: 3181
		public int[] mFrogY = new int[5];

		// Token: 0x04000C6E RID: 3182
		public int mBarWidth;

		// Token: 0x04000C6F RID: 3183
		public int mBarHeight;

		// Token: 0x04000C70 RID: 3184
		public int mTreasureFreq;

		// Token: 0x04000C71 RID: 3185
		public int mParTime;

		// Token: 0x04000C72 RID: 3186
		public int mMoveType;

		// Token: 0x04000C73 RID: 3187
		public int mMoveSpeed;

		// Token: 0x04000C74 RID: 3188
		public int mUpdateCount;

		// Token: 0x04000C75 RID: 3189
		public int mTimer;

		// Token: 0x04000C76 RID: 3190
		public int mTimeToComplete;

		// Token: 0x04000C77 RID: 3191
		public int mInvertMouseTimer;

		// Token: 0x04000C78 RID: 3192
		public int mMaxInvertMouseTimer;

		// Token: 0x04000C79 RID: 3193
		public int mTempSpeedupTimer;

		// Token: 0x04000C7A RID: 3194
		public int mBossFreezePowerupTime;

		// Token: 0x04000C7B RID: 3195
		public int mFrogShieldPowerupCount;

		// Token: 0x04000C7C RID: 3196
		public int mStartingGauntletLevel;

		// Token: 0x04000C7D RID: 3197
		public int mTorchTimer;

		// Token: 0x04000C7E RID: 3198
		public int mFurthestBallDistance;

		// Token: 0x04000C7F RID: 3199
		public int mIntroTorchDelay;

		// Token: 0x04000C80 RID: 3200
		public int mIntroTorchIndex;

		// Token: 0x04000C81 RID: 3201
		public int mGauntletCurTime;

		// Token: 0x04000C82 RID: 3202
		public int mGauntletMultipliersEarned;

		// Token: 0x04000C83 RID: 3203
		public int mNumGauntletBallsBroke;

		// Token: 0x04000C84 RID: 3204
		public int mGauntletCurNumForMult;

		// Token: 0x04000C85 RID: 3205
		public int mCurMultiplierTimeLeft;

		// Token: 0x04000C86 RID: 3206
		public int mMaxMultiplierTime;

		// Token: 0x04000C87 RID: 3207
		public float mGauntletTimeRedAmt;

		// Token: 0x04000C88 RID: 3208
		public int mZone;

		// Token: 0x04000C89 RID: 3209
		public int mNum;

		// Token: 0x04000C8A RID: 3210
		public int mPostZumaTimeCounter;

		// Token: 0x04000C8B RID: 3211
		public float mPostZumaTimeSpeedInc;

		// Token: 0x04000C8C RID: 3212
		public float mPostZumaTimeSlowInc;

		// Token: 0x04000C8D RID: 3213
		public string mBossBGID = "";

		// Token: 0x04000C8E RID: 3214
		public int m_OriginX = -1;

		// Token: 0x04000C8F RID: 3215
		public int m_OriginY = -1;

		// Token: 0x04000C90 RID: 3216
		public bool m_canGetAchievementNoMove;

		// Token: 0x04000C91 RID: 3217
		public bool m_canGetAchievementNoJump;

		// Token: 0x04000C92 RID: 3218
		public bool mHaveReachedTarget;

		// Token: 0x04000C93 RID: 3219
		public int mCurBarSize;

		// Token: 0x04000C94 RID: 3220
		public int mCurBarSizeInc;

		// Token: 0x04000C95 RID: 3221
		public int mTargetBarSize;

		// Token: 0x04000C96 RID: 3222
		public int mZumaBallFrame;

		// Token: 0x04000C97 RID: 3223
		public float mBarLightness;

		// Token: 0x04000C98 RID: 3224
		public int mZumaPulseUCStart;

		// Token: 0x04000C99 RID: 3225
		public float mGingerMouthX;

		// Token: 0x04000C9A RID: 3226
		public float mGingerMouthVX;

		// Token: 0x04000C9B RID: 3227
		public float mGingerMouthXStart;

		// Token: 0x04000C9C RID: 3228
		public float mFredMouthX;

		// Token: 0x04000C9D RID: 3229
		public float mFredMouthVX;

		// Token: 0x04000C9E RID: 3230
		public float mFredMouthXStart;

		// Token: 0x04000C9F RID: 3231
		public float mFredTongueX;

		// Token: 0x04000CA0 RID: 3232
		public float mFredTongueVX;

		// Token: 0x04000CA1 RID: 3233
		public float mZumaBallPct;

		// Token: 0x04000CA2 RID: 3234
		public int mZumaBarState;

		// Token: 0x04000CA3 RID: 3235
		public float mGoldBallXOff;

		// Token: 0x04000CA4 RID: 3236
		public int mBarXOffset;

		// Token: 0x04000CA5 RID: 3237
		public int mZumaBarX;

		// Token: 0x04000CA6 RID: 3238
		public int mZumaBarWidth;

		// Token: 0x04000CA7 RID: 3239
		private static int last_sound_idx;

		// Token: 0x04000CA8 RID: 3240
		private static bool torchChangeState;

		// Token: 0x02000102 RID: 258
		public enum TorchState
		{
			// Token: 0x04000CAA RID: 3242
			TorchState_FlyIn,
			// Token: 0x04000CAB RID: 3243
			TorchState_Bounce,
			// Token: 0x04000CAC RID: 3244
			TorchState_TossEgg,
			// Token: 0x04000CAD RID: 3245
			TorchState_Disappear,
			// Token: 0x04000CAE RID: 3246
			TorchState_RaiseDais,
			// Token: 0x04000CAF RID: 3247
			TorchState_FrogFlyIn,
			// Token: 0x04000CB0 RID: 3248
			TorchState_IntroDone,
			// Token: 0x04000CB1 RID: 3249
			TorchState_ShakeDais,
			// Token: 0x04000CB2 RID: 3250
			TorchState_FrogDisappear,
			// Token: 0x04000CB3 RID: 3251
			TorchState_DoFade,
			// Token: 0x04000CB4 RID: 3252
			TorchState_DropInToNextLevel,
			// Token: 0x04000CB5 RID: 3253
			TorchState_CloakedBossAppear,
			// Token: 0x04000CB6 RID: 3254
			TorchState_CloakedBossTransform,
			// Token: 0x04000CB7 RID: 3255
			TorchState_Complete
		}
	}
}
