using System;

namespace ZumasRevenge
{
	// Token: 0x02000037 RID: 55
	public class RockChunk
	{
		// Token: 0x060005CD RID: 1485 RVA: 0x00020730 File Offset: 0x0001E930
		public void SyncState(DataSync sync)
		{
			sync.SyncLong(ref this.mCol);
			sync.SyncFloat(ref this.mX);
			sync.SyncFloat(ref this.mY);
			sync.SyncFloat(ref this.mVX);
			sync.SyncFloat(ref this.mVY);
			sync.SyncFloat(ref this.mAlpha);
		}

		// Token: 0x040002B8 RID: 696
		public int mCol;

		// Token: 0x040002B9 RID: 697
		public float mX;

		// Token: 0x040002BA RID: 698
		public float mY;

		// Token: 0x040002BB RID: 699
		public float mVX;

		// Token: 0x040002BC RID: 700
		public float mVY;

		// Token: 0x040002BD RID: 701
		public float mAlpha;
	}
}
