using System;

namespace ZumasRevenge
{
	// Token: 0x02000024 RID: 36
	public class EggFragment
	{
		// Token: 0x06000565 RID: 1381 RVA: 0x0001A7F4 File Offset: 0x000189F4
		public void SyncState(DataSync sync)
		{
			sync.SyncFloat(ref this.mVX);
			sync.SyncFloat(ref this.mVY);
			sync.SyncFloat(ref this.mDecVX);
			sync.SyncFloat(ref this.mDecVY);
			sync.SyncFloat(ref this.mAlpha);
			sync.SyncLong(ref this.mCol);
			sync.SyncFloat(ref this.mX);
			sync.SyncFloat(ref this.mY);
		}

		// Token: 0x04000227 RID: 551
		public float mVX;

		// Token: 0x04000228 RID: 552
		public float mVY;

		// Token: 0x04000229 RID: 553
		public float mDecVX;

		// Token: 0x0400022A RID: 554
		public float mDecVY;

		// Token: 0x0400022B RID: 555
		public float mAlpha;

		// Token: 0x0400022C RID: 556
		public int mCol;

		// Token: 0x0400022D RID: 557
		public float mX;

		// Token: 0x0400022E RID: 558
		public float mY;
	}
}
