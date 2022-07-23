using System;

namespace ZumasRevenge
{
	// Token: 0x020000E6 RID: 230
	public class SkeletonPowerOrb
	{
		// Token: 0x06000C31 RID: 3121 RVA: 0x000718A5 File Offset: 0x0006FAA5
		public void SyncState(DataSync sync)
		{
			sync.SyncFloat(ref this.mSize);
			sync.SyncFloat(ref this.mAlpha);
		}

		// Token: 0x04000AD1 RID: 2769
		public float mSize;

		// Token: 0x04000AD2 RID: 2770
		public float mAlpha = 255f;
	}
}
