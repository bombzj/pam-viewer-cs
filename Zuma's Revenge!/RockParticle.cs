using System;

namespace ZumasRevenge
{
	// Token: 0x02000029 RID: 41
	public class RockParticle
	{
		// Token: 0x06000584 RID: 1412 RVA: 0x0001C2A4 File Offset: 0x0001A4A4
		public RockParticle()
		{
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0001C2AC File Offset: 0x0001A4AC
		public RockParticle(RockParticle rhs)
		{
			this.mAlpha = rhs.mAlpha;
			this.mCel = rhs.mCel;
			this.mX = rhs.mX;
			this.mY = rhs.mY;
			this.mVX = rhs.mVX;
			this.mVY = rhs.mVY;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0001C308 File Offset: 0x0001A508
		public void SyncState(DataSync sync)
		{
			sync.SyncLong(ref this.mCel);
			sync.SyncFloat(ref this.mAlpha);
			sync.SyncFloat(ref this.mX);
			sync.SyncFloat(ref this.mY);
			sync.SyncFloat(ref this.mVX);
			sync.SyncFloat(ref this.mVY);
		}

		// Token: 0x04000249 RID: 585
		public float mAlpha;

		// Token: 0x0400024A RID: 586
		public int mCel;

		// Token: 0x0400024B RID: 587
		public float mX;

		// Token: 0x0400024C RID: 588
		public float mY;

		// Token: 0x0400024D RID: 589
		public float mVX;

		// Token: 0x0400024E RID: 590
		public float mVY;
	}
}
