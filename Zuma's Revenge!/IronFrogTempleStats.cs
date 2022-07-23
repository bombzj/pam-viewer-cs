using System;

namespace ZumasRevenge
{
	// Token: 0x0200012A RID: 298
	public class IronFrogTempleStats
	{
		// Token: 0x06000F38 RID: 3896 RVA: 0x0009D3A8 File Offset: 0x0009B5A8
		public void CopyFrom(IronFrogTempleStats rhs)
		{
			this.mNumAttempts = rhs.mNumAttempts;
			this.mNumVictories = rhs.mNumVictories;
			this.mBestTime = rhs.mBestTime;
			this.mCurTime = rhs.mCurTime;
			this.mBestScore = rhs.mBestScore;
			this.mHighestLevel = rhs.mHighestLevel;
			for (int i = 0; i < 10; i++)
			{
				this.mLevelDeaths[i] = rhs.mLevelDeaths[i];
			}
			this.mTotalTimePlayed = rhs.mTotalTimePlayed;
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0009D428 File Offset: 0x0009B628
		public void Sync(DataSync theSync)
		{
			theSync.SyncLong(ref this.mNumAttempts);
			theSync.SyncLong(ref this.mNumVictories);
			theSync.SyncLong(ref this.mBestTime);
			theSync.SyncLong(ref this.mCurTime);
			theSync.SyncLong(ref this.mBestScore);
			theSync.SyncLong(ref this.mHighestLevel);
			for (int i = 0; i < 10; i++)
			{
				theSync.SyncLong(ref this.mLevelDeaths[i]);
			}
			theSync.SyncLong(ref this.mTotalTimePlayed);
		}

		// Token: 0x04000EE0 RID: 3808
		public int mNumAttempts;

		// Token: 0x04000EE1 RID: 3809
		public int mNumVictories;

		// Token: 0x04000EE2 RID: 3810
		public int mBestTime;

		// Token: 0x04000EE3 RID: 3811
		public int mCurTime;

		// Token: 0x04000EE4 RID: 3812
		public int mBestScore;

		// Token: 0x04000EE5 RID: 3813
		public int mHighestLevel;

		// Token: 0x04000EE6 RID: 3814
		public int[] mLevelDeaths = new int[10];

		// Token: 0x04000EE7 RID: 3815
		public int mTotalTimePlayed;
	}
}
