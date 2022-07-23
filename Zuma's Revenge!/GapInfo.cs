using System;

namespace ZumasRevenge
{
	// Token: 0x02000084 RID: 132
	public class GapInfo
	{
		// Token: 0x060008D9 RID: 2265 RVA: 0x0004F8AF File Offset: 0x0004DAAF
		public void SyncState(DataSync sync)
		{
			sync.SyncLong(ref this.mCurve);
			sync.SyncLong(ref this.mDist);
			sync.SyncLong(ref this.mBallId);
		}

		// Token: 0x040006DD RID: 1757
		public int mCurve;

		// Token: 0x040006DE RID: 1758
		public int mDist;

		// Token: 0x040006DF RID: 1759
		public int mBallId;
	}
}
