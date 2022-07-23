using System;

namespace ZumasRevenge
{
	// Token: 0x02000061 RID: 97
	public class GameStats
	{
		// Token: 0x0600072B RID: 1835 RVA: 0x00030920 File Offset: 0x0002EB20
		public GameStats()
		{
			this.mTimePlayed = 0;
			this.mNumBallsCleared = 0;
			this.mNumGemsCleared = 0;
			this.mNumGaps = 0;
			this.mNumCombos = 0;
			this.mMaxCombo = -1;
			this.mMaxComboScore = 0;
			this.mMaxInARow = 0;
			this.mMaxInARowScore = 0;
			this.mDangerTimePlayed = 0;
			this.mTotalShots = (this.mNumMisses = 0);
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0003098C File Offset: 0x0002EB8C
		public void Reset()
		{
			this.mTimePlayed = 0;
			this.mNumBallsCleared = 0;
			this.mNumGemsCleared = 0;
			this.mNumGaps = 0;
			this.mNumCombos = 0;
			this.mMaxCombo = -1;
			this.mMaxComboScore = 0;
			this.mMaxInARow = 0;
			this.mMaxInARowScore = 0;
			this.mDangerTimePlayed = 0;
			this.mTotalShots = (this.mNumMisses = 0);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x000309F0 File Offset: 0x0002EBF0
		public void Add(GameStats theStats)
		{
			this.mTimePlayed += theStats.mTimePlayed;
			this.mNumBallsCleared += theStats.mNumBallsCleared;
			this.mNumGemsCleared += theStats.mNumGemsCleared;
			this.mNumCombos += theStats.mNumCombos;
			this.mNumGaps += theStats.mNumGaps;
			if (theStats.mMaxCombo > this.mMaxCombo || (theStats.mMaxCombo == this.mMaxCombo && theStats.mMaxComboScore > this.mMaxComboScore))
			{
				this.mMaxCombo = theStats.mMaxCombo;
				this.mMaxComboScore = theStats.mMaxComboScore;
			}
			if (theStats.mMaxInARow > this.mMaxInARow)
			{
				this.mMaxInARow = theStats.mMaxInARow;
				this.mMaxInARowScore = theStats.mMaxInARowScore;
			}
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00030AC4 File Offset: 0x0002ECC4
		public void SyncState(DataSync theSync)
		{
			theSync.SyncLong(ref this.mTimePlayed);
			theSync.SyncLong(ref this.mNumBallsCleared);
			theSync.SyncLong(ref this.mNumGemsCleared);
			theSync.SyncLong(ref this.mNumGaps);
			theSync.SyncLong(ref this.mNumCombos);
			theSync.SyncLong(ref this.mMaxCombo);
			theSync.SyncLong(ref this.mMaxComboScore);
			theSync.SyncLong(ref this.mMaxInARow);
			theSync.SyncLong(ref this.mMaxInARowScore);
			theSync.SyncLong(ref this.mDangerTimePlayed);
			theSync.SyncLong(ref this.mTotalShots);
			theSync.SyncLong(ref this.mNumMisses);
		}

		// Token: 0x04000475 RID: 1141
		public int mTimePlayed;

		// Token: 0x04000476 RID: 1142
		public int mDangerTimePlayed;

		// Token: 0x04000477 RID: 1143
		public int mNumBallsCleared;

		// Token: 0x04000478 RID: 1144
		public int mNumGemsCleared;

		// Token: 0x04000479 RID: 1145
		public int mNumGaps;

		// Token: 0x0400047A RID: 1146
		public int mNumCombos;

		// Token: 0x0400047B RID: 1147
		public int mMaxCombo;

		// Token: 0x0400047C RID: 1148
		public int mMaxComboScore;

		// Token: 0x0400047D RID: 1149
		public int mMaxInARow;

		// Token: 0x0400047E RID: 1150
		public int mMaxInARowScore;

		// Token: 0x0400047F RID: 1151
		public int mTotalShots;

		// Token: 0x04000480 RID: 1152
		public int mNumMisses;
	}
}
