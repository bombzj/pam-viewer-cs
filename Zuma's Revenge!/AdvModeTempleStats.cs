using System;

namespace ZumasRevenge
{
	// Token: 0x02000129 RID: 297
	public class AdvModeTempleStats
	{
		// Token: 0x06000F35 RID: 3893 RVA: 0x0009D240 File Offset: 0x0009B440
		public void CopyFrom(AdvModeTempleStats rhs)
		{
			this.mHighestLevel = rhs.mHighestLevel;
			this.mBestTime = rhs.mBestTime;
			this.mBestScore = rhs.mBestScore;
			this.mNumLevelsAced = rhs.mNumLevelsAced;
			this.mNumPerfectLevels = rhs.mNumPerfectLevels;
			this.mNumClearCurves = rhs.mNumClearCurves;
			for (int i = 0; i < 6; i++)
			{
				this.mBossDeaths[i] = rhs.mBossDeaths[i];
			}
			for (int j = 0; j < 60; j++)
			{
				this.mLevelDeaths[j] = rhs.mLevelDeaths[j];
			}
			this.mTotalTimePlayed = rhs.mTotalTimePlayed;
			this.mCurrentTime = rhs.mCurrentTime;
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x0009D2E8 File Offset: 0x0009B4E8
		public void Sync(DataSync theSync)
		{
			theSync.SyncLong(ref this.mHighestLevel);
			theSync.SyncLong(ref this.mBestTime);
			theSync.SyncLong(ref this.mBestScore);
			theSync.SyncLong(ref this.mNumLevelsAced);
			theSync.SyncLong(ref this.mNumPerfectLevels);
			theSync.SyncLong(ref this.mNumClearCurves);
			for (int i = 0; i < 6; i++)
			{
				theSync.SyncLong(ref this.mBossDeaths[i]);
			}
			for (int j = 0; j < 60; j++)
			{
				theSync.SyncLong(ref this.mLevelDeaths[j]);
			}
			theSync.SyncLong(ref this.mTotalTimePlayed);
			theSync.SyncLong(ref this.mCurrentTime);
		}

		// Token: 0x04000ED6 RID: 3798
		public int mHighestLevel;

		// Token: 0x04000ED7 RID: 3799
		public int mBestTime = int.MaxValue;

		// Token: 0x04000ED8 RID: 3800
		public int mBestScore;

		// Token: 0x04000ED9 RID: 3801
		public int mNumLevelsAced;

		// Token: 0x04000EDA RID: 3802
		public int mNumPerfectLevels;

		// Token: 0x04000EDB RID: 3803
		public int mNumClearCurves;

		// Token: 0x04000EDC RID: 3804
		public int[] mBossDeaths = new int[6];

		// Token: 0x04000EDD RID: 3805
		public int[] mLevelDeaths = new int[60];

		// Token: 0x04000EDE RID: 3806
		public int mTotalTimePlayed;

		// Token: 0x04000EDF RID: 3807
		public int mCurrentTime;
	}
}
