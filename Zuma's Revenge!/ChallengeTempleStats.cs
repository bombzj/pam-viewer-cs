using System;

namespace ZumasRevenge
{
	// Token: 0x0200012B RID: 299
	public class ChallengeTempleStats
	{
		// Token: 0x06000F3B RID: 3899 RVA: 0x0009D4C0 File Offset: 0x0009B6C0
		public void CopyFrom(ChallengeTempleStats rhs)
		{
			this.mHighestScore = rhs.mHighestScore;
			this.mNumTimesHitScoreTarget = rhs.mNumTimesHitScoreTarget;
			this.mHighestMult = rhs.mHighestMult;
			for (int i = 0; i < 70; i++)
			{
				this.mNumTimesPlayedCurve[i] = rhs.mNumTimesPlayedCurve[i];
			}
			this.mTotalTime = rhs.mTotalTime;
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x0009D51C File Offset: 0x0009B71C
		public void Sync(DataSync theSync)
		{
			theSync.SyncLong(ref this.mHighestScore);
			theSync.SyncLong(ref this.mNumTimesHitScoreTarget);
			theSync.SyncLong(ref this.mHighestMult);
			for (int i = 0; i < 70; i++)
			{
				theSync.SyncLong(ref this.mNumTimesPlayedCurve[i]);
			}
			theSync.SyncLong(ref this.mTotalTime);
		}

		// Token: 0x04000EE8 RID: 3816
		public int mHighestScore;

		// Token: 0x04000EE9 RID: 3817
		public int mNumTimesHitScoreTarget;

		// Token: 0x04000EEA RID: 3818
		public int mHighestMult;

		// Token: 0x04000EEB RID: 3819
		public int[] mNumTimesPlayedCurve = new int[70];

		// Token: 0x04000EEC RID: 3820
		public int mTotalTime;
	}
}
