using System;
using SexyFramework;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x0200005D RID: 93
	public class BetaStats
	{
		// Token: 0x06000709 RID: 1801 RVA: 0x0002FD8C File Offset: 0x0002DF8C
		protected void Reset()
		{
			this.mBossHP = 0;
			this.mLives = 0;
			this.mNumDeathsThisLevel = 0;
			this.mLevelTime = (this.mAceTime = 0);
			this.mHighestGapShotScore = 0;
			this.mHighestChainShotPoints = 0;
			this.mHighestComboPoints = 0;
			this.mFurthestRolloutPct = 0f;
			this.mMaxFruitMultiplier = 0;
			this.mPerfectLevelBonus = 0;
			this.mAceBonus = 0;
			this.mLevelScore = (this.mTotalScore = 0);
			this.mLargestGapShot = 0;
			this.mNumGapShots = 0;
			this.mPointsFromGapShots = 0;
			this.mLargestChainShot = 0;
			this.mPointsFromChainShots = 0;
			this.mLargestCombo = 0;
			this.mPointsFromCombos = 0;
			this.mNumClearCurveBonuses = 0;
			this.mPointsFromClearCurve = 0;
			this.mNumFruits = 0;
			this.mPointsFromFruit = 0;
			this.mNumTimesLaserCanceled = 0;
			this.mPointsFromLaser = 0;
			this.mPointsFromCannon = 0;
			this.mPointsFromColorNuke = 0;
			this.mPointsFromProxBomb = 0;
			this.mWasFromCheckpoint = (this.mWasFromZoneRestart = false);
			this.mNumTimesActivatedPowerup = new int[14];
			this.mNumTimesSpawnedPowerup = new int[14];
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0002FE9D File Offset: 0x0002E09D
		protected void SaveCSVFile(int challenge_level, int challenge_mult)
		{
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0002FE9F File Offset: 0x0002E09F
		protected void SaveCSVFile(int challenge_level)
		{
			this.SaveCSVFile(challenge_level, -1);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0002FEA9 File Offset: 0x0002E0A9
		protected void SaveCSVFile()
		{
			this.SaveCSVFile(-1, -1);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0002FEB3 File Offset: 0x0002E0B3
		protected void Serialize(SexyBuffer b)
		{
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0002FEB5 File Offset: 0x0002E0B5
		protected bool Deserialize(SexyBuffer b)
		{
			return true;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0002FEB8 File Offset: 0x0002E0B8
		public BetaStats()
		{
			this.mSessionID = 0;
			this.mMode = BetaStats.Mode.Mode_None;
			this.mLevelZone = -1;
			this.mLevelNum = -1;
			this.mProfile = null;
			this.Reset();
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0002FF10 File Offset: 0x0002E110
		public void CopyFrom(BetaStats rhs)
		{
			this.mProfile = rhs.mProfile;
			this.mSessionID = rhs.mSessionID;
			this.mMode = rhs.mMode;
			this.mNumDeathsThisLevel = rhs.mNumDeathsThisLevel;
			this.mLevelName = rhs.mLevelName;
			this.mLevelZone = rhs.mLevelZone;
			this.mLevelNum = rhs.mLevelNum;
			this.mLevelTime = rhs.mLevelTime;
			this.mAceTime = rhs.mAceTime;
			this.mNumGapShots = rhs.mNumGapShots;
			this.mLargestGapShot = rhs.mLargestGapShot;
			this.mHighestGapShotScore = rhs.mHighestGapShotScore;
			this.mLargestChainShot = rhs.mLargestChainShot;
			this.mHighestChainShotPoints = rhs.mHighestChainShotPoints;
			this.mLargestCombo = rhs.mLargestCombo;
			this.mHighestComboPoints = rhs.mHighestComboPoints;
			this.mFurthestRolloutPct = rhs.mFurthestRolloutPct;
			this.mNumClearCurveBonuses = rhs.mNumClearCurveBonuses;
			this.mPerfectLevelBonus = rhs.mPerfectLevelBonus;
			this.mAceBonus = rhs.mAceBonus;
			this.mPointsFromClearCurve = rhs.mPointsFromClearCurve;
			this.mPointsFromGapShots = rhs.mPointsFromGapShots;
			this.mPointsFromCombos = rhs.mPointsFromCombos;
			this.mPointsFromChainShots = rhs.mPointsFromChainShots;
			this.mNumFruits = rhs.mNumFruits;
			this.mLives = rhs.mLives;
			this.mBossHP = rhs.mBossHP;
			this.mPointsFromFruit = rhs.mPointsFromFruit;
			this.mMaxFruitMultiplier = rhs.mMaxFruitMultiplier;
			this.mNumTimesLaserCanceled = rhs.mNumTimesLaserCanceled;
			this.mWasFromCheckpoint = rhs.mWasFromCheckpoint;
			this.mWasFromZoneRestart = rhs.mWasFromZoneRestart;
			this.mLevelScore = rhs.mLevelScore;
			this.mTotalScore = rhs.mTotalScore;
			this.mPointsFromLaser = rhs.mPointsFromLaser;
			this.mPointsFromCannon = rhs.mPointsFromCannon;
			this.mPointsFromColorNuke = rhs.mPointsFromColorNuke;
			this.mPointsFromProxBomb = rhs.mPointsFromProxBomb;
			for (int i = 0; i < 14; i++)
			{
				this.mNumTimesActivatedPowerup[i] = rhs.mNumTimesActivatedPowerup[i];
				this.mNumTimesSpawnedPowerup[i] = rhs.mNumTimesSpawnedPowerup[i];
			}
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00030114 File Offset: 0x0002E314
		public string GetCSVFileName()
		{
			switch (this.mMode)
			{
			case BetaStats.Mode.Mode_Challenge:
				return SexyFramework.Common.GetAppDataFolder() + "CHALLENGE STATS DO NOT DELETE.csv";
			case BetaStats.Mode.Mode_IronFrog:
				return SexyFramework.Common.GetAppDataFolder() + "IRON FROG STATS DO NOT DELETE.csv";
			case BetaStats.Mode.Mode_Adventure:
				return SexyFramework.Common.GetAppDataFolder() + "ADVENTURE STATS DO NOT DELETE.csv";
			case BetaStats.Mode.Mode_HardAdventure:
				return SexyFramework.Common.GetAppDataFolder() + "HEROIC STATS DO NOT DELETE.csv";
			default:
				return "ERROR.csv";
			}
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00030188 File Offset: 0x0002E388
		public string GetDATFileName()
		{
			string text = "";
			switch (this.mMode)
			{
			case BetaStats.Mode.Mode_Challenge:
				text = "challenge";
				break;
			case BetaStats.Mode.Mode_IronFrog:
				text = "if";
				break;
			case BetaStats.Mode.Mode_Adventure:
				text = "adv";
				break;
			case BetaStats.Mode.Mode_HardAdventure:
				text = "hard_adv";
				break;
			}
			return SexyFramework.Common.GetAppDataFolder() + string.Format("users/user{0}_{1}_stats.dat", this.mProfile.GetId(), text);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x000301FD File Offset: 0x0002E3FD
		public void Init(ZumaProfile p, int session_id, int mode)
		{
			this.mProfile = p;
			this.mSessionID = session_id;
			this.Reset();
			this.mMode = (BetaStats.Mode)mode;
			this.mLevelZone = -1;
			this.mLevelNum = -1;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00030228 File Offset: 0x0002E428
		public void LevelStarted(string level_name, int zone, int num, bool from_checkpoint, bool zone_restart)
		{
			this.Reset();
			this.mLevelName = level_name;
			this.mLevelZone = zone;
			this.mLevelNum = num;
			this.mWasFromCheckpoint = from_checkpoint;
			this.mWasFromZoneRestart = zone_restart;
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x00030258 File Offset: 0x0002E458
		public void BeatLevel(int level_time, int ace_time, int ace_bonus, int perfect_bonus, float rollout_pct, int level_score, int total_score, int lives)
		{
			this.mLives = lives;
			this.mLevelTime = level_time;
			this.mAceTime = ace_time;
			this.mAceBonus = ace_bonus;
			this.mPerfectLevelBonus = perfect_bonus;
			this.mFurthestRolloutPct = rollout_pct;
			this.mLevelScore = level_score;
			this.mTotalScore = total_score;
			this.SaveCSVFile();
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x000302A8 File Offset: 0x0002E4A8
		public void DiedOnLevel(int level_time, int level_score, int total_score, int lives_left, int challenge_level, int challenge_multiplier, int boss_hp)
		{
			this.mBossHP = boss_hp;
			this.mLevelScore = level_score;
			this.mTotalScore = total_score;
			this.mLives = lives_left;
			this.mFurthestRolloutPct = 1f;
			this.mNumDeathsThisLevel++;
			this.mLevelTime = level_time;
			this.SaveCSVFile(challenge_level, challenge_multiplier);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x000302FD File Offset: 0x0002E4FD
		public void DiedOnLevel(int level_time, int level_score, int total_score, int lives_left, int challenge_level, int challenge_multiplier)
		{
			this.DiedOnLevel(level_time, level_score, total_score, lives_left, challenge_level, challenge_multiplier, 0);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0003030F File Offset: 0x0002E50F
		public void DiedOnLevel(int level_time, int level_score, int total_score, int lives_left, int challenge_level)
		{
			this.DiedOnLevel(level_time, level_score, total_score, lives_left, challenge_level, -1);
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0003031F File Offset: 0x0002E51F
		public void DiedOnLevel(int level_time, int level_score, int total_score, int lives_left)
		{
			this.DiedOnLevel(level_time, level_score, total_score, lives_left, -1, -1);
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00030330 File Offset: 0x0002E530
		public void LoadData()
		{
			string datfileName = this.GetDATFileName();
			SexyBuffer b = new SexyBuffer();
			if (GameApp.gApp.ReadBufferFromFile(datfileName, ref b) && !this.Deserialize(b))
			{
				GameApp.gApp.EraseFile(datfileName);
				GameApp.gApp.EraseFile(this.GetCSVFileName());
			}
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00030380 File Offset: 0x0002E580
		public void SaveData()
		{
			SexyBuffer buffer = new SexyBuffer();
			this.Serialize(buffer);
			GameApp.gApp.WriteBufferToFile(this.GetDATFileName(), buffer);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x000303AC File Offset: 0x0002E5AC
		public void SetFruitMultiplier(int m)
		{
			if (m > this.mMaxFruitMultiplier)
			{
				this.mMaxFruitMultiplier = m;
			}
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x000303C0 File Offset: 0x0002E5C0
		public void GapShot(int points, int size)
		{
			this.mNumGapShots++;
			this.mProfile.mNumGapShots++;
			if (size > this.mLargestGapShot)
			{
				this.mLargestGapShot = size;
			}
			if (points > this.mHighestGapShotScore)
			{
				this.mHighestGapShotScore = points;
			}
			if (points > this.mProfile.mHighestGapShotScore)
			{
				this.mProfile.mHighestGapShotScore = points;
			}
			if (this.mLargestGapShot > this.mProfile.mLargestGapShot)
			{
				this.mProfile.mLargestGapShot = this.mLargestGapShot;
			}
			this.mPointsFromGapShots += points;
			this.mProfile.mPointsFromGapShots += points;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00030470 File Offset: 0x0002E670
		public void ChainShot(int points, int size)
		{
			if (size > this.mLargestChainShot)
			{
				this.mLargestChainShot = size;
			}
			if (points > this.mHighestChainShotPoints)
			{
				this.mHighestChainShotPoints = points;
			}
			this.mPointsFromChainShots += points;
			this.mProfile.mPointsFromChainShots += points;
			if (this.mLargestChainShot > this.mProfile.mLargestChainShot)
			{
				this.mProfile.mLargestChainShot = this.mLargestChainShot;
			}
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x000304E4 File Offset: 0x0002E6E4
		public void Combo(int points, int size)
		{
			if (points > this.mHighestComboPoints)
			{
				this.mHighestComboPoints = points;
			}
			if (size > this.mLargestCombo)
			{
				this.mLargestCombo = size;
			}
			this.mPointsFromCombos += points;
			this.mProfile.mPointsFromCombos += points;
			if (this.mLargestCombo > this.mProfile.mLargestCombo)
			{
				this.mProfile.mLargestCombo = this.mLargestCombo;
			}
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00030558 File Offset: 0x0002E758
		public void ClearedCurve(int points)
		{
			this.mNumClearCurveBonuses++;
			this.mPointsFromClearCurve += points;
			this.mProfile.mNumClearCurveBonuses++;
			this.mProfile.mPointsFromClearCurve += points;
			if (this.mNumClearCurveBonuses >= 2)
			{
				GameApp.gApp.SetAchievement("clear_2x");
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x000305C0 File Offset: 0x0002E7C0
		public void HitFruit(int points)
		{
			this.mNumFruits++;
			this.mPointsFromFruit += points;
			this.mProfile.mNumFruits++;
			this.mProfile.mPointsFromFruit += points;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0003060F File Offset: 0x0002E80F
		public void CanceledLaser()
		{
			this.mNumTimesLaserCanceled++;
			this.mProfile.mNumTimesLaserCanceled++;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00030634 File Offset: 0x0002E834
		public void BallExplodedFromPowerup(int power_type)
		{
			if (power_type == 0)
			{
				this.mPointsFromProxBomb += 10;
				this.mProfile.mPointsFromProxBomb += 10;
				return;
			}
			switch (power_type)
			{
			case 7:
				this.mPointsFromCannon += 10;
				this.mProfile.mPointsFromCannon += 10;
				return;
			case 8:
				this.mPointsFromColorNuke += 10;
				this.mProfile.mPointsFromColorNuke += 10;
				return;
			case 9:
				this.mPointsFromLaser += 10;
				this.mProfile.mPointsFromLaser += 10;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x000306EE File Offset: 0x0002E8EE
		public void ActivatedPowerup(int power_type)
		{
			this.mNumTimesActivatedPowerup[power_type]++;
			this.mProfile.mNumTimesActivatedPowerup[power_type]++;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00030727 File Offset: 0x0002E927
		public void SpawnedPowerup(int power_type)
		{
			this.mNumTimesSpawnedPowerup[power_type]++;
		}

		// Token: 0x04000441 RID: 1089
		protected int mSessionID;

		// Token: 0x04000442 RID: 1090
		protected BetaStats.Mode mMode;

		// Token: 0x04000443 RID: 1091
		protected int mNumDeathsThisLevel;

		// Token: 0x04000444 RID: 1092
		protected string mLevelName;

		// Token: 0x04000445 RID: 1093
		protected int mLevelZone;

		// Token: 0x04000446 RID: 1094
		protected int mLevelNum;

		// Token: 0x04000447 RID: 1095
		protected int mLevelTime;

		// Token: 0x04000448 RID: 1096
		protected int mAceTime;

		// Token: 0x04000449 RID: 1097
		protected int mNumGapShots;

		// Token: 0x0400044A RID: 1098
		protected int mLargestGapShot;

		// Token: 0x0400044B RID: 1099
		protected int mHighestGapShotScore;

		// Token: 0x0400044C RID: 1100
		protected int mLargestChainShot;

		// Token: 0x0400044D RID: 1101
		protected int mHighestChainShotPoints;

		// Token: 0x0400044E RID: 1102
		protected int mLargestCombo;

		// Token: 0x0400044F RID: 1103
		protected int mHighestComboPoints;

		// Token: 0x04000450 RID: 1104
		protected float mFurthestRolloutPct;

		// Token: 0x04000451 RID: 1105
		protected int mNumClearCurveBonuses;

		// Token: 0x04000452 RID: 1106
		protected int mPerfectLevelBonus;

		// Token: 0x04000453 RID: 1107
		protected int mAceBonus;

		// Token: 0x04000454 RID: 1108
		protected int mPointsFromClearCurve;

		// Token: 0x04000455 RID: 1109
		protected int mPointsFromGapShots;

		// Token: 0x04000456 RID: 1110
		protected int mPointsFromCombos;

		// Token: 0x04000457 RID: 1111
		protected int mPointsFromChainShots;

		// Token: 0x04000458 RID: 1112
		protected int mNumFruits;

		// Token: 0x04000459 RID: 1113
		protected int mLives;

		// Token: 0x0400045A RID: 1114
		protected int mBossHP;

		// Token: 0x0400045B RID: 1115
		protected int mPointsFromFruit;

		// Token: 0x0400045C RID: 1116
		protected int mMaxFruitMultiplier;

		// Token: 0x0400045D RID: 1117
		protected int mNumTimesLaserCanceled;

		// Token: 0x0400045E RID: 1118
		protected bool mWasFromCheckpoint;

		// Token: 0x0400045F RID: 1119
		protected bool mWasFromZoneRestart;

		// Token: 0x04000460 RID: 1120
		protected int mLevelScore;

		// Token: 0x04000461 RID: 1121
		protected int mTotalScore;

		// Token: 0x04000462 RID: 1122
		protected int mPointsFromLaser;

		// Token: 0x04000463 RID: 1123
		protected int mPointsFromCannon;

		// Token: 0x04000464 RID: 1124
		protected int mPointsFromColorNuke;

		// Token: 0x04000465 RID: 1125
		protected int mPointsFromProxBomb;

		// Token: 0x04000466 RID: 1126
		protected int[] mNumTimesActivatedPowerup = new int[14];

		// Token: 0x04000467 RID: 1127
		protected int[] mNumTimesSpawnedPowerup = new int[14];

		// Token: 0x04000468 RID: 1128
		public ZumaProfile mProfile;

		// Token: 0x0200005E RID: 94
		public enum Mode
		{
			// Token: 0x0400046A RID: 1130
			Mode_Challenge,
			// Token: 0x0400046B RID: 1131
			Mode_IronFrog,
			// Token: 0x0400046C RID: 1132
			Mode_Adventure,
			// Token: 0x0400046D RID: 1133
			Mode_HardAdventure,
			// Token: 0x0400046E RID: 1134
			Mode_None
		}
	}
}
