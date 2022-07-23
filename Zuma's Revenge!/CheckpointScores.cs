using System;

namespace ZumasRevenge
{
	// Token: 0x02000128 RID: 296
	public class CheckpointScores
	{
		// Token: 0x06000F33 RID: 3891 RVA: 0x0009D1EE File Offset: 0x0009B3EE
		public void CopyFrom(CheckpointScores rhs)
		{
			this.mZoneStart = rhs.mZoneStart;
			this.mMidpoint = rhs.mMidpoint;
			this.mBoss = rhs.mBoss;
		}

		// Token: 0x04000ED3 RID: 3795
		public int mZoneStart;

		// Token: 0x04000ED4 RID: 3796
		public int mMidpoint;

		// Token: 0x04000ED5 RID: 3797
		public int mBoss;
	}
}
