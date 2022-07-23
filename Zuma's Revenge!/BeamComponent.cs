using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x020000E4 RID: 228
	public class BeamComponent
	{
		// Token: 0x06000C2E RID: 3118 RVA: 0x000717C0 File Offset: 0x0006F9C0
		public void SyncState(DataSync sync)
		{
			sync.SyncFloat(ref this.mX);
			sync.SyncFloat(ref this.mY);
			sync.SyncFloat(ref this.mVX);
			sync.SyncFloat(ref this.mVY);
			sync.SyncFloat(ref this.mV0);
			sync.SyncFloat(ref this.mDistTraveled);
			sync.SyncBoolean(ref this.mAdditive);
			sync.SyncLong(ref this.mAlphaDelta);
			sync.SyncLong(ref this.mMinAlpha);
			sync.SyncLong(ref this.mCel);
			sync.SyncLong(ref this.mColor.mAlpha);
			sync.SyncLong(ref this.mColor.mRed);
			sync.SyncLong(ref this.mColor.mGreen);
			sync.SyncLong(ref this.mColor.mBlue);
		}

		// Token: 0x04000ABD RID: 2749
		public MemoryImage mImage;

		// Token: 0x04000ABE RID: 2750
		public float mX;

		// Token: 0x04000ABF RID: 2751
		public float mY;

		// Token: 0x04000AC0 RID: 2752
		public float mVX;

		// Token: 0x04000AC1 RID: 2753
		public float mVY;

		// Token: 0x04000AC2 RID: 2754
		public float mV0;

		// Token: 0x04000AC3 RID: 2755
		public float mDistTraveled;

		// Token: 0x04000AC4 RID: 2756
		public bool mAdditive;

		// Token: 0x04000AC5 RID: 2757
		public int mAlphaDelta;

		// Token: 0x04000AC6 RID: 2758
		public int mMinAlpha;

		// Token: 0x04000AC7 RID: 2759
		public int mCel;

		// Token: 0x04000AC8 RID: 2760
		public SexyColor mColor = default(SexyColor);
	}
}
