using System;

namespace ZumasRevenge
{
	// Token: 0x02000022 RID: 34
	public class Egg
	{
		// Token: 0x06000562 RID: 1378 RVA: 0x0001A6CF File Offset: 0x000188CF
		public void SyncState(DataSync sync)
		{
			sync.SyncFloat(ref this.mAngle);
			sync.SyncFloat(ref this.mSize);
		}

		// Token: 0x0400021B RID: 539
		public float mAngle = 1.570795f;

		// Token: 0x0400021C RID: 540
		public float mSize;
	}
}
