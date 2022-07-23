using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x02000106 RID: 262
	public class LevelsXmlReader : ContentTypeReader<LevelMgr>
	{
		// Token: 0x06000DE7 RID: 3559 RVA: 0x0008CAE4 File Offset: 0x0008ACE4
		protected override LevelMgr Read(ContentReader input, LevelMgr existingInstance)
		{
			LevelMgr levelMgr = new LevelMgr();
			this.mLevelMgr = levelMgr;
			this.ReadLevels(input, levelMgr);
			input.ReadString();
			return levelMgr;
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0008CB10 File Offset: 0x0008AD10
		private void ReadLevels(ContentReader input, LevelMgr mgr)
		{
			this.readDot(input, mgr);
			this.readDDS(input, mgr);
			this.readHandheldBalance(input, mgr);
			this.readGauntletMode(input, mgr);
			this.readTips(input, mgr);
			this.readScoreTip(input, mgr);
			this.readZone(input, mgr);
			this.readDefaults(input, mgr);
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				Level level = new Level();
				this.readLevel(input, level);
				mgr.mLevels.Add(level);
			}
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0008CB8C File Offset: 0x0008AD8C
		private void readDot(ContentReader input, LevelMgr mgr)
		{
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				SexyPoint point = new SexyPoint();
				point.mX = input.ReadInt32();
				point.mY = input.ReadInt32();
				mgr.mMapPoints.Add(point);
			}
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0008CBD8 File Offset: 0x0008ADD8
		private void readDDS(ContentReader input, LevelMgr mgr)
		{
			mgr.mNumDDSTiers = input.ReadInt32();
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				mgr.mDDSPowerupPctInc.Add(input.ReadSingle());
			}
			num = input.ReadInt32();
			for (int j = 0; j < num; j++)
			{
				mgr.mDDSSlowAdd.Add(input.ReadInt32());
			}
			num = input.ReadInt32();
			for (int k = 0; k < num; k++)
			{
				mgr.mDDSSpeedPct.Add(input.ReadSingle());
			}
			num = input.ReadInt32();
			for (int l = 0; l < num; l++)
			{
				mgr.mDDSZumaPointDecPct.Add(input.ReadSingle());
			}
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0008CC88 File Offset: 0x0008AE88
		private void readHandheldBalance(ContentReader input, LevelMgr mgr)
		{
			GameApp.gDDS.mHandheldBalance.mFruitPowerupAdditionalDuration = input.ReadSingle();
			GameApp.gDDS.mHandheldBalance.mChanceOfSameColorBallIncrease = input.ReadSingle();
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				GameApp.gDDS.mHandheldBalance.mAdventureModeSpeedDelta[i] = input.ReadSingle();
			}
			num = input.ReadInt32();
			for (int j = 0; j < num; j++)
			{
				GameApp.gDDS.mHandheldBalance.mChallengeModeSpeedDelta[j] = input.ReadSingle();
			}
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0008CD14 File Offset: 0x0008AF14
		private void readLevel(ContentReader input, Level level)
		{
			level.mId = input.ReadString();
			level.mNum = input.ReadInt32();
			level.mZone = input.ReadInt32();
			level.mChallengePoints = input.ReadInt32();
			level.mChallengeAcePoints = input.ReadInt32();
			level.mOffscreenClearBonus = input.ReadBoolean();
			level.mStartingGauntletLevel = input.ReadInt32();
			level.mDisplayNameId = input.ReadInt32();
			level.mDisplayName = TextManager.getInstance().getString(level.mDisplayNameId);
			level.mEndSequence = input.ReadInt32();
			level.mHurryToRolloutAmt = input.ReadSingle();
			level.mIronFrog = input.ReadBoolean();
			level.mTorchTimer = input.ReadInt32();
			level.mBossFreezePowerupTime = input.ReadInt32();
			level.mFrogShieldPowerupCount = input.ReadInt32();
			level.mDrawCurves = input.ReadBoolean();
			level.mImagePath = input.ReadString();
			level.mBGFromPSD = input.ReadBoolean();
			level.mNoBackground = input.ReadBoolean();
			level.mNoFlip = input.ReadBoolean();
			level.mSliderEdgeRotate = input.ReadBoolean();
			level.mSuckMode = input.ReadBoolean();
			level.mTreasureFreq = input.ReadInt32();
			level.mTimeToComplete = input.ReadInt32();
			level.mParTime = input.ReadInt32();
			level.mIsEndless = input.ReadBoolean();
			level.mLoopAtEnd = input.ReadBoolean();
			level.mMaxInvertMouseTimer = input.ReadInt32();
			level.mPotPct = input.ReadSingle();
			level.mPreviewTextId = input.ReadInt32();
			level.mPreviewText = TextManager.getInstance().getString(level.mPreviewTextId);
			level.mFinalLevel = input.ReadBoolean();
			level.mPopupText = input.ReadString();
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				level.mEffectNames.Add(input.ReadString());
			}
			int num2 = input.ReadInt32();
			for (int j = 0; j < num2; j++)
			{
				int num3 = input.ReadInt32();
				string text = input.ReadString();
				if (level.mCurveMgr[num3] == null)
				{
					CurveMgr curveMgr = (level.mCurveMgr[num3] = new CurveMgr(null, num3));
					curveMgr.mLevel = level;
					if (this.mLevelMgr.mIsHardConfig)
					{
						text += "_hard";
					}
					curveMgr.SetPath(text);
				}
			}
			level.mNumCurves = num2;
			num = input.ReadInt32();
			for (int k = 0; k < num; k++)
			{
				level.mCurveSkullAngleOverrides[k] = input.ReadSingle();
			}
			this.readTreasurePoint(input, level);
			this.readEffectParams(input, level);
			this.readTunnel(input, level);
			this.readGun(input, level);
			this.readPowerupRegion(input, level);
			this.readTorch(input, level);
			this.readWall(input, level);
			this.readMovingWall(input, level);
			this.readBoss(input, level);
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0008CFCC File Offset: 0x0008B1CC
		private void readGauntletMode(ContentReader input, LevelMgr mgr)
		{
			List<Gauntlet_Vals>[] gauntletVals = GameApp.gDDS.getGauntletVals();
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				if (gauntletVals[i] == null)
				{
					gauntletVals[i] = new List<Gauntlet_Vals>();
				}
				List<Gauntlet_Vals> list = gauntletVals[i];
				int num2 = input.ReadInt32();
				for (int j = 0; j < num2; j++)
				{
					Gauntlet_Vals gauntlet_Vals = new Gauntlet_Vals();
					list.Add(gauntlet_Vals);
					gauntlet_Vals.mDifficultyLevel = input.ReadInt32();
					gauntlet_Vals.mTimeBaseDifficulty = input.ReadBoolean();
					gauntlet_Vals.mSpeed = input.ReadSingle();
					gauntlet_Vals.mStartDistance = input.ReadInt32();
					gauntlet_Vals.mZumaScore = input.ReadInt32();
					gauntlet_Vals.mBallRepeat = input.ReadInt32();
					gauntlet_Vals.mPowerupChance = input.ReadInt32();
					gauntlet_Vals.mNumColors = input.ReadInt32();
					gauntlet_Vals.mRollbackPct = input.ReadInt32();
					gauntlet_Vals.mSlowFactor = input.ReadSingle();
					gauntlet_Vals.mMaxClumpSize = input.ReadInt32();
					gauntlet_Vals.mMaxSingle = input.ReadInt32();
					gauntlet_Vals.mRollbackTime = input.ReadInt32();
					gauntlet_Vals.mHurryDist = input.ReadInt32();
					gauntlet_Vals.mHurryMaxSpeed = input.ReadSingle();
				}
			}
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0008D100 File Offset: 0x0008B300
		private void readTips(ContentReader input, LevelMgr mgr)
		{
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				int num2 = input.ReadInt32();
				mgr.mLevelTipIds.Add(num2);
				mgr.mLevelTips.Add(TextManager.getInstance().getString(num2));
			}
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0008D14C File Offset: 0x0008B34C
		private void readScoreTip(ContentReader input, LevelMgr mgr)
		{
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				ScoreTip scoreTip = default(ScoreTip);
				mgr.mScoreTips.Add(scoreTip);
				scoreTip.mMinLevel = input.ReadInt32();
				scoreTip.mTipId = input.ReadInt32();
				scoreTip.mTip = TextManager.getInstance().getString(scoreTip.mTipId);
			}
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0008D1B4 File Offset: 0x0008B3B4
		private void readZone(ContentReader input, LevelMgr mgr)
		{
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				if (mgr.mZones[i] == null)
				{
					mgr.mZones[i] = new ZoneInfo();
				}
				ZoneInfo zoneInfo = mgr.mZones[i];
				zoneInfo.mBossPrefix = input.ReadString();
				zoneInfo.mStartLevel = input.ReadString();
				zoneInfo.mNum = input.ReadInt32();
				zoneInfo.mChallengePoints = input.ReadInt32();
				zoneInfo.mChallengeAcePoints = input.ReadInt32();
				zoneInfo.mDifficultyLocId = input.ReadInt32();
				zoneInfo.mDifficulty = TextManager.getInstance().getString(zoneInfo.mDifficultyLocId);
				zoneInfo.mCupNameLocId = input.ReadInt32();
				zoneInfo.mCupName = TextManager.getInstance().getString(zoneInfo.mCupNameLocId);
				int num2 = input.ReadInt32();
				for (int j = 0; j < num2; j++)
				{
					zoneInfo.mBossTauntIds[j] = input.ReadInt32();
					zoneInfo.mBossTaunts[j] = TextManager.getInstance().getString(zoneInfo.mBossTauntIds[j]);
				}
				zoneInfo.mFruitId = input.ReadString();
			}
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0008D2C8 File Offset: 0x0008B4C8
		private void readDefaults(ContentReader input, LevelMgr mgr)
		{
			mgr.mCannonShots = input.ReadInt32();
			mgr.mCannonAngle = input.ReadSingle();
			mgr.mMaxZumaPctForColorNuke = input.ReadSingle();
			mgr.mBeatGamePointsForLife = input.ReadInt32();
			mgr.mPointsForLife = input.ReadInt32();
			mgr.mBossesCanAttackFuckedFrog = input.ReadBoolean();
			mgr.mAttackDelayAfterHittingFrog = input.ReadInt32();
			mgr.mPointsForBronze = input.ReadInt32();
			mgr.mPointsForSilver = input.ReadInt32();
			mgr.mPointsForGold = input.ReadInt32();
			mgr.mPowerupIncAtZumaPct = input.ReadSingle();
			mgr.mPowerIncPct = input.ReadSingle();
			mgr.mClearCurvePoints = input.ReadInt32();
			mgr.mClearCurveRolloutPct = input.ReadSingle();
			mgr.mClearCurveSpeedMult = input.ReadSingle();
			mgr.mGauntletSessionLength = input.ReadInt32();
			mgr.mGauntletNumForMultBase = input.ReadInt32();
			mgr.mGauntletNumForMultInc = input.ReadInt32();
			mgr.mMultiplierDuration = input.ReadInt32();
			mgr.mMultiplierTimeAdd = input.ReadInt32();
			mgr.mMaxGauntletNumForMult = input.ReadInt32();
			mgr.mPointTimeAdd = input.ReadInt32();
			mgr.mNumPointsForTimeAdd = input.ReadInt32();
			mgr.mGauntletTFreq = input.ReadInt32();
			mgr.mCannonStacks = input.ReadBoolean();
			mgr.mMinMultBallDistance = input.ReadSingle();
			mgr.mMultBallPoints = input.ReadInt32();
			mgr.mMultBallLife = input.ReadInt32();
			mgr.mBossTauntChance = input.ReadInt32();
			mgr.mPowerupSpawnDelay = input.ReadInt32();
			mgr.mLazerShots = input.ReadInt32();
			mgr.mLazerStacks = input.ReadBoolean();
			mgr.mPowerDelay = input.ReadInt32();
			mgr.mPowerCooldown = input.ReadInt32();
			mgr.mAllowColorNukeAfterZuma = input.ReadBoolean();
			mgr.mColorNukeTimeAfterZuma = input.ReadInt32();
			mgr.mUniquePowerupColor = input.ReadBoolean();
			mgr.mCapAffectsPowerupsSpawned = input.ReadBoolean();
			mgr.mPostZumaTime = input.ReadInt32();
			mgr.mPostZumaTimeSpeedInc = input.ReadSingle();
			mgr.mPostZumaTimeSlowInc = input.ReadSingle();
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0008D4C4 File Offset: 0x0008B6C4
		private void readTreasurePoint(ContentReader input, Level level)
		{
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				TreasurePoint treasurePoint = new TreasurePoint();
				level.mTreasurePoints.Add(treasurePoint);
				treasurePoint.x = input.ReadInt32();
				treasurePoint.y = input.ReadInt32();
				int num2 = input.ReadInt32();
				for (int j = 0; j < num2; j++)
				{
					treasurePoint.mCurveDist[j] = input.ReadInt32();
				}
			}
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0008D538 File Offset: 0x0008B738
		private void readEffectParams(ContentReader input, Level level)
		{
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				EffectParams effectParams = new EffectParams();
				level.mEffectParams.Add(effectParams);
				effectParams.mKey = input.ReadString();
				effectParams.mValue = input.ReadString();
				effectParams.mEffectIndex = input.ReadInt32();
			}
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0008D590 File Offset: 0x0008B790
		private void readTunnel(ContentReader input, Level level)
		{
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				TunnelData tunnelData = new TunnelData();
				level.mTunnelData.Add(tunnelData);
				tunnelData.mImageName = input.ReadString();
				tunnelData.mLayerId = input.ReadString();
				tunnelData.mAboveShadows = input.ReadBoolean();
				tunnelData.mX = input.ReadInt32();
				tunnelData.mY = input.ReadInt32();
				tunnelData.mPriority = input.ReadInt32();
				tunnelData.mNoThumb = input.ReadBoolean();
			}
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0008D618 File Offset: 0x0008B818
		private void readGun(ContentReader input, Level level)
		{
			level.mMoveType = input.ReadInt32();
			level.mNumFrogPoints = input.ReadInt32();
			for (int i = 0; i < level.mNumFrogPoints; i++)
			{
				level.mFrogX[i] = input.ReadInt32();
				level.mFrogY[i] = input.ReadInt32();
			}
			level.mMoveSpeed = input.ReadInt32();
			int num = input.ReadInt32();
			for (int j = 0; j < num; j++)
			{
				level.mFrogImages[j].mFilename = input.ReadString();
				level.mFrogImages[j].mResId = input.ReadString();
			}
			level.mBarWidth = input.ReadInt32();
			level.mBarHeight = input.ReadInt32();
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0008D6C8 File Offset: 0x0008B8C8
		private void readPowerupRegion(ContentReader input, Level level)
		{
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				PowerupRegion powerupRegion = new PowerupRegion();
				level.mPowerupRegions.Add(powerupRegion);
				powerupRegion.mCurvePctStart = input.ReadSingle();
				powerupRegion.mCurvePctEnd = input.ReadSingle();
				powerupRegion.mChance = input.ReadInt32();
				powerupRegion.mCurveNum = input.ReadInt32();
				powerupRegion.mDebugDraw = input.ReadBoolean();
			}
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0008D738 File Offset: 0x0008B938
		private void readTorch(ContentReader input, Level level)
		{
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				Torch torch = new Torch();
				level.mTorches.Add(torch);
				torch.mX = input.ReadInt32();
				torch.mY = input.ReadInt32();
				torch.mWidth = input.ReadInt32();
				torch.mHeight = input.ReadInt32();
			}
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0008D79C File Offset: 0x0008B99C
		private void readWall(ContentReader input, Level level)
		{
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				Wall wall = new Wall();
				level.mWalls.Add(wall);
				wall.mX = input.ReadSingle();
				wall.mY = input.ReadSingle();
				wall.mWidth = input.ReadSingle();
				wall.mHeight = input.ReadSingle();
				wall.mColor = new SexyColor(input.ReadInt32());
				wall.mMinRespawnTimer = input.ReadInt32();
				wall.mMaxRespawnTimer = input.ReadInt32();
				wall.mMinLifeTimer = input.ReadInt32();
				wall.mMaxLifeTimer = input.ReadInt32();
				wall.mCurLifeTimer = input.ReadInt32();
				wall.mStrength = input.ReadInt32();
				wall.mOrgStrength = input.ReadInt32();
				wall.mId = input.ReadInt32();
				wall.mVX = input.ReadSingle();
				wall.mVY = input.ReadSingle();
			}
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0008D890 File Offset: 0x0008BA90
		private void readMovingWall(ContentReader input, Level level)
		{
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				Wall wall = new Wall();
				level.mMovingWallDefaults.Add(wall);
				wall.mX = (float)input.ReadInt32();
				wall.mY = (float)input.ReadInt32();
				wall.mWidth = (float)input.ReadInt32();
				wall.mHeight = (float)input.ReadInt32();
				wall.mColor = new SexyColor(input.ReadInt32());
				wall.mMinRespawnTimer = input.ReadInt32();
				wall.mMaxRespawnTimer = input.ReadInt32();
				wall.mMinLifeTimer = input.ReadInt32();
				wall.mMaxLifeTimer = input.ReadInt32();
				wall.mCurLifeTimer = input.ReadInt32();
				wall.mStrength = input.ReadInt32();
				wall.mOrgStrength = input.ReadInt32();
				wall.mId = input.ReadInt32();
				wall.mVX = input.ReadSingle();
				wall.mVY = input.ReadSingle();
				wall.mSpacing = input.ReadInt32();
			}
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0008D994 File Offset: 0x0008BB94
		private void readBoss(ContentReader input, Level level)
		{
			int num = input.ReadInt32();
			if (num > 0)
			{
				level.mBoss = this.readBossDefine(input, level);
				if (num > 1)
				{
					level.mSecondaryBoss = this.readBossDefine(input, level);
				}
			}
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0008D9CC File Offset: 0x0008BBCC
		private Boss readBossDefine(ContentReader input, Level level)
		{
			Boss boss = null;
			BossType bossType = (BossType)input.ReadInt32();
			switch (bossType)
			{
			case BossType.BOSS_TIGER:
				boss = new BossTiger();
				break;
			case BossType.BOSS_SKELETON:
				boss = new BossSkeleton();
				break;
			case BossType.BOSS_DOCTOR:
				boss = new BossDoctor();
				break;
			case BossType.BOSS_SQUID:
				boss = new BossSquid();
				break;
			case BossType.BOSS_MOSQUITO:
				boss = new BossMosquito();
				break;
			case BossType.BOSS_STONE_HEAD:
				boss = new BossStoneHead();
				break;
			case BossType.BOSS_LAME:
				boss = new BossLame();
				break;
			case BossType.BOSS_VOLCANO:
				boss = new BossVolcano();
				break;
			case BossType.BOSS_DARK_FROG:
				boss = new BossDarkFrog();
				break;
			}
			boss.mSepiaImagePath = input.ReadString();
			boss.mNum = input.ReadInt32();
			boss.mWordBubbleText = input.ReadString();
			boss.mHPDecPerHit = input.ReadSingle();
			boss.mHPDecPerProxBomb = input.ReadSingle();
			boss.mAllowCompacting = input.ReadBoolean();
			if (bossType == BossType.BOSS_SKELETON)
			{
				BossSkeleton bossSkeleton = boss as BossSkeleton;
				bossSkeleton.mSpecialWpnHPDec = input.ReadInt32();
				bossSkeleton.mSpawnPowerupWhilePoweredUp = input.ReadBoolean();
				bossSkeleton.mChanceToSpawnPowerup = input.ReadInt32();
			}
			boss.mDrawRadius = input.ReadBoolean();
			boss.mRadiusColorChangeMode = input.ReadInt32();
			boss.mAllowLevelDDS = input.ReadBoolean();
			boss.mShakeXAmt = input.ReadInt32();
			boss.mShakeYAmt = input.ReadInt32();
			boss.mHeartXOff = input.ReadInt32();
			boss.mHeartYOff = input.ReadInt32();
			boss.mImpatientTimer = input.ReadInt32();
			boss.mResetWallsOnBossHit = input.ReadBoolean();
			boss.mResetWallTimerOnTikiHit = input.ReadBoolean();
			boss.mWallDownTime = input.ReadInt32();
			boss.mTikiHealthRespawnAmt = input.ReadInt32();
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				BossWall bossWall = new BossWall();
				boss.getWalls().Add(bossWall);
				bossWall.mX = input.ReadInt32();
				bossWall.mY = input.ReadInt32();
				bossWall.mWidth = input.ReadInt32();
				bossWall.mHeight = input.ReadInt32();
				bossWall.mId = input.ReadInt32();
				bossWall.mAlphaFadeDir = 1;
				bossWall.mAlpha = 0;
			}
			num = input.ReadInt32();
			for (int j = 0; j < num; j++)
			{
				Tiki tiki = new Tiki();
				boss.mTikis.Add(tiki);
				tiki.mId = input.ReadInt32();
				tiki.mX = input.ReadSingle();
				tiki.mY = input.ReadSingle();
				tiki.mRailStartX = input.ReadInt32();
				tiki.mRailStartY = input.ReadInt32();
				tiki.mRailEndX = input.ReadInt32();
				tiki.mRailEndY = input.ReadInt32();
				tiki.mTravelTime = input.ReadInt32();
				tiki.mVX = input.ReadSingle();
			}
			BossShoot bossShoot = boss as BossShoot;
			bossShoot.mBossRadius = input.ReadInt32();
			bossShoot.mMaxShotBounces = input.ReadInt32();
			bossShoot.mBallShieldDamage = input.ReadInt32();
			bossShoot.mShieldHP = input.ReadInt32();
			bossShoot.mTeleportMinTime = input.ReadInt32();
			bossShoot.mTeleportMaxTime = input.ReadInt32();
			bossShoot.mBombFreqMin = input.ReadInt32();
			bossShoot.mBombFreqMax = input.ReadInt32();
			bossShoot.mBombDuration = input.ReadInt32();
			bossShoot.mMinSpots = input.ReadInt32();
			bossShoot.mMaxSpots = input.ReadInt32();
			bossShoot.mMinSpotRad = input.ReadInt32();
			bossShoot.mMaxSpotRad = input.ReadInt32();
			bossShoot.mMinSpotFade = input.ReadSingle();
			bossShoot.mMaxSpotFade = input.ReadSingle();
			bossShoot.mSpotFadeDelay = input.ReadInt32();
			bossShoot.mInkTargetMode = input.ReadInt32();
			bossShoot.mEnrageDelay = input.ReadInt32();
			bossShoot.mBombAppearDelay = input.ReadInt32();
			bossShoot.mStartX = input.ReadInt32();
			bossShoot.mEndX = input.ReadInt32();
			bossShoot.mStartY = input.ReadInt32();
			bossShoot.mEndY = input.ReadInt32();
			bossShoot.SetX((float)input.ReadInt32());
			bossShoot.SetY((float)input.ReadInt32());
			bossShoot.mUseShield = input.ReadBoolean();
			bossShoot.mShieldRotateSpeed = input.ReadSingle();
			bossShoot.mShieldQuadRespawnTime = input.ReadInt32();
			bossShoot.mShieldPauseTime = input.ReadInt32();
			bossShoot.mEnrageShieldRestore = input.ReadBoolean();
			num = input.ReadInt32();
			for (int k = 0; k < num; k++)
			{
				SexyPoint point = new SexyPoint();
				bossShoot.mPoints.Add(point);
				point.mX = input.ReadInt32();
				point.mY = input.ReadInt32();
			}
			bossShoot.mCanShootBullets = input.ReadBoolean();
			bossShoot.mShotType = input.ReadInt32();
			bossShoot.mVolcanoOffscreenDelay = input.ReadInt32();
			bossShoot.mHomingCorrectionAmt = input.ReadSingle();
			bossShoot.mMinRetalSpeed = input.ReadSingle();
			bossShoot.mMaxRetalSpeed = input.ReadSingle();
			bossShoot.mShotDelay = input.ReadInt32();
			bossShoot.mRetalShotDelay = input.ReadInt32();
			bossShoot.mEndHoverOnHit = input.ReadBoolean();
			bossShoot.mFlightSpeed = input.ReadSingle();
			bossShoot.mFlightMinDist = input.ReadInt32();
			bossShoot.mColorVampire = input.ReadBoolean();
			bossShoot.mStrafe = input.ReadBoolean();
			bossShoot.mIncMaxShotHealthAmt = input.ReadInt32();
			bossShoot.mIncRetalMaxShotHealthAmt = input.ReadInt32();
			bossShoot.mAvoidColor = input.ReadBoolean();
			bossShoot.mColorVampHealthInc = input.ReadInt32();
			bossShoot.mMinColorChangeTime = input.ReadInt32();
			bossShoot.mMaxColorChangeTime = input.ReadInt32();
			bossShoot.mColorVampChanceToMatch2ndBall = input.ReadInt32();
			bossShoot.mMovementMode = input.ReadInt32();
			bossShoot.mMovementAccel = input.ReadSingle();
			bossShoot.mDefaultMovementUpdateDelay = input.ReadInt32();
			bossShoot.mMinHoverTime = input.ReadInt32();
			bossShoot.mMaxHoverTime = input.ReadInt32();
			bossShoot.mMinFireDelay = input.ReadInt32();
			bossShoot.mMaxFireDelay = input.ReadInt32();
			bossShoot.mEatsBalls = input.ReadBoolean();
			bossShoot.mFrogStunTime = input.ReadInt32();
			bossShoot.mFrogPoisonTime = input.ReadInt32();
			bossShoot.mFrogHallucinateTime = input.ReadInt32();
			bossShoot.mFrogSlowTimer = input.ReadInt32();
			bossShoot.mMinBulletSpeed = input.ReadSingle();
			bossShoot.mMaxBulletSpeed = input.ReadSingle();
			bossShoot.mSubType = input.ReadInt32();
			bossShoot.mMaxBulletsToFire = input.ReadInt32();
			bossShoot.mMaxRetaliationBullets = input.ReadInt32();
			bossShoot.mSineShotsTargetPlayer = input.ReadBoolean();
			bossShoot.mMinSineShotTime = input.ReadInt32();
			bossShoot.mMaxSineShotTime = input.ReadInt32();
			bossShoot.mSinusoidalRetaliation = input.ReadBoolean();
			bossShoot.mMinAmp = input.ReadSingle();
			bossShoot.mMaxAmp = input.ReadSingle();
			bossShoot.mMinFreq = input.ReadSingle();
			bossShoot.mMaxFreq = input.ReadSingle();
			bossShoot.mMinYInc = input.ReadSingle();
			bossShoot.mMaxYInc = input.ReadSingle();
			bossShoot.mMinXInc = input.ReadSingle();
			bossShoot.mMaxXInc = input.ReadSingle();
			bossShoot.mSpeed = input.ReadSingle();
			bossShoot.mDefaultSpeed = input.ReadSingle();
			bossShoot.mDecMinHover = input.ReadInt32();
			bossShoot.mDecMaxHover = input.ReadInt32();
			bossShoot.mDecMinFire = input.ReadInt32();
			bossShoot.mDecMaxFire = input.ReadInt32();
			this.readBossDDS(input, boss, level);
			this.readBossBerserk(input, boss);
			this.readBossSkeletonEmitter(input, boss);
			this.readBossHintText(input, boss);
			this.readBossHula(input, boss);
			this.readDeathText(input, boss);
			return boss;
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0008E15C File Offset: 0x0008C35C
		private void readBossDDS(ContentReader input, Boss boss, Level level)
		{
			Dictionary<string, Boss_DDS_Vals> bossParams = GameApp.gDDS.getBossParams();
			string mId = level.mId;
			int num = input.ReadInt32();
			if (num > 0)
			{
				Boss_DDS_Vals boss_DDS_Vals;
				if (bossParams.ContainsKey(mId))
				{
					boss_DDS_Vals = bossParams[mId];
				}
				else
				{
					boss_DDS_Vals = new Boss_DDS_Vals();
					bossParams[mId] = boss_DDS_Vals;
				}
				for (int i = 0; i < num; i++)
				{
					string text = input.ReadString();
					List<Boss_Param_Range> list = new List<Boss_Param_Range>();
					int num2 = input.ReadInt32();
					for (int j = 0; j < num2; j++)
					{
						list.Add(new Boss_Param_Range
						{
							mMin = input.ReadSingle(),
							mMax = input.ReadSingle(),
							mRatingMin = input.ReadSingle(),
							mRatingMax = input.ReadSingle()
						});
					}
					boss_DDS_Vals.mParams.Add(text, list);
				}
			}
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0008E244 File Offset: 0x0008C444
		private void readBossBerserk(ContentReader input, Boss boss)
		{
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				BerserkTier berserkTier = new BerserkTier();
				boss.getBerserkTiers().Add(berserkTier);
				berserkTier.mHealthLimit = input.ReadInt32();
				int num2 = input.ReadInt32();
				berserkTier.mParams = new List<BerserkModifier>();
				for (int j = 0; j < num2; j++)
				{
					string p = input.ReadString();
					string value = input.ReadString();
					string minval = input.ReadString();
					string maxval = input.ReadString();
					bool @override = input.ReadBoolean();
					BerserkModifier berserkModifier = new BerserkModifier(p, value, minval, maxval, @override);
					berserkTier.mParams.Add(berserkModifier);
				}
			}
			BossShoot bossShoot = boss as BossShoot;
			num = input.ReadInt32();
			for (int k = 0; k < num; k++)
			{
				BossBerserkMovement bossBerserkMovement = new BossBerserkMovement();
				bossShoot.getBerserkMovementList().Add(bossBerserkMovement);
				bossBerserkMovement.mStartX = input.ReadInt32();
				bossBerserkMovement.mEndX = input.ReadInt32();
				bossBerserkMovement.mStartY = input.ReadInt32();
				bossBerserkMovement.mEndY = input.ReadInt32();
				bossBerserkMovement.mX = input.ReadInt32();
				bossBerserkMovement.mY = input.ReadInt32();
				int num3 = input.ReadInt32();
				for (int l = 0; l < num3; l++)
				{
					SexyPoint point = new SexyPoint();
					point.mX = input.ReadInt32();
					point.mY = input.ReadInt32();
					bossBerserkMovement.mPoints.Add(point);
				}
				bossBerserkMovement.mHealthLimit = input.ReadInt32();
			}
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0008E3D8 File Offset: 0x0008C5D8
		private void readBossSkeletonEmitter(ContentReader input, Boss boss)
		{
			if (boss is BossSkeleton)
			{
				BossSkeleton bossSkeleton = boss as BossSkeleton;
				bossSkeleton.mNumSkeleToEmit = input.ReadInt32();
				bossSkeleton.mSkeleDelay = input.ReadInt32();
				bossSkeleton.mDelayAfterSkeleEmit = input.ReadInt32();
				bossSkeleton.mSkeletonVX = input.ReadSingle();
				bossSkeleton.mSkeletonVY = input.ReadSingle();
				bossSkeleton.mSkeletonEmitX = input.ReadSingle();
				bossSkeleton.mSkeletonEmitY = input.ReadSingle();
			}
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0008E448 File Offset: 0x0008C648
		private void readBossHintText(ContentReader input, Boss boss)
		{
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				TauntText tauntText = new TauntText();
				boss.mTauntText.Add(tauntText);
				tauntText.mTextId = input.ReadInt32();
				tauntText.mText = TextManager.getInstance().getString(tauntText.mTextId);
				tauntText.mCondition = input.ReadInt32();
				tauntText.mMinDeaths = input.ReadInt32();
				tauntText.mDelay = input.ReadInt32();
				tauntText.mMinTime = input.ReadInt32();
			}
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0008E4D0 File Offset: 0x0008C6D0
		private void readBossHula(ContentReader input, Boss boss)
		{
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				HulaEntry hulaEntry = new HulaEntry();
				boss.getHulaEntryList().Add(hulaEntry);
				hulaEntry.mBerserkAmt = input.ReadInt32();
				hulaEntry.mAmnesty = input.ReadInt32();
				hulaEntry.mProjVY = input.ReadSingle();
				hulaEntry.mSpawnRate = input.ReadInt32();
				hulaEntry.mVX = input.ReadSingle();
				hulaEntry.mSpawnY = input.ReadInt32();
				hulaEntry.mProjChance = input.ReadInt32();
				hulaEntry.mAttackTime = input.ReadInt32();
				hulaEntry.mAttackType = input.ReadInt32();
				hulaEntry.mProjRange = input.ReadInt32();
			}
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0008E584 File Offset: 0x0008C784
		private void readDeathText(ContentReader input, Boss boss)
		{
			int num = input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				BossText bossText = new BossText();
				bossText.mTextId = input.ReadInt32();
				bossText.mText = TextManager.getInstance().getString(bossText.mTextId);
				boss.mDeathText.Add(bossText);
			}
		}

		// Token: 0x04000D0A RID: 3338
		protected LevelMgr mLevelMgr;
	}
}
