using System;

namespace ZumasRevenge
{
	// Token: 0x02000135 RID: 309
	public class RollerDigit
	{
		// Token: 0x06000F6C RID: 3948 RVA: 0x0009F410 File Offset: 0x0009D610
		public void SyncState(DataSync sync)
		{
			sync.SyncLong(ref this.mNum);
			sync.SyncFloat(ref this.mX);
			sync.SyncFloat(ref this.mY);
			sync.SyncFloat(ref this.mVY);
			sync.SyncLong(ref this.mDelay);
			sync.SyncLong(ref this.mBounceState);
			sync.SyncLong(ref this.mRestingY);
		}

		// Token: 0x040016AD RID: 5805
		public int mNum = -1;

		// Token: 0x040016AE RID: 5806
		public float mX;

		// Token: 0x040016AF RID: 5807
		public float mY;

		// Token: 0x040016B0 RID: 5808
		public float mVY;

		// Token: 0x040016B1 RID: 5809
		public int mDelay;

		// Token: 0x040016B2 RID: 5810
		public int mBounceState;

		// Token: 0x040016B3 RID: 5811
		public int mRestingY;
	}
}
