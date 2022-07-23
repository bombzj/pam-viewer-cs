using System;

namespace SexyFramework
{
	// Token: 0x0200006C RID: 108
	public class NativeDisplay
	{
		// Token: 0x06000450 RID: 1104 RVA: 0x0000CD5C File Offset: 0x0000AF5C
		public NativeDisplay()
		{
			this.mRedConvTable = new ulong[256];
			this.mGreenConvTable = new ulong[256];
			this.mBlueConvTable = new ulong[256];
			this.mRGBBits = 0;
			this.mRedMask = 0UL;
			this.mGreenMask = 0UL;
			this.mBlueMask = 0UL;
			this.mRedBits = 0;
			this.mGreenBits = 0;
			this.mBlueBits = 0;
			this.mRedShift = 0;
			this.mGreenShift = 0;
			this.mBlueShift = 0;
			this.mRedAddTable = null;
			this.mGreenAddTable = null;
			this.mBlueAddTable = null;
		}

		// Token: 0x04000224 RID: 548
		public int mRGBBits;

		// Token: 0x04000225 RID: 549
		public ulong mRedMask;

		// Token: 0x04000226 RID: 550
		public ulong mGreenMask;

		// Token: 0x04000227 RID: 551
		public ulong mBlueMask;

		// Token: 0x04000228 RID: 552
		public int mRedBits;

		// Token: 0x04000229 RID: 553
		public int mGreenBits;

		// Token: 0x0400022A RID: 554
		public int mBlueBits;

		// Token: 0x0400022B RID: 555
		public int mRedShift;

		// Token: 0x0400022C RID: 556
		public int mGreenShift;

		// Token: 0x0400022D RID: 557
		public int mBlueShift;

		// Token: 0x0400022E RID: 558
		public int[] mRedAddTable;

		// Token: 0x0400022F RID: 559
		public int[] mGreenAddTable;

		// Token: 0x04000230 RID: 560
		public int[] mBlueAddTable;

		// Token: 0x04000231 RID: 561
		public ulong[] mRedConvTable;

		// Token: 0x04000232 RID: 562
		public ulong[] mGreenConvTable;

		// Token: 0x04000233 RID: 563
		public ulong[] mBlueConvTable;
	}
}
