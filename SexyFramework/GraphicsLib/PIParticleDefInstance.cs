using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000E3 RID: 227
	public class PIParticleDefInstance
	{
		// Token: 0x060006B2 RID: 1714 RVA: 0x0001C9EC File Offset: 0x0001ABEC
		public PIParticleDefInstance()
		{
			this.Reset();
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0001C9FA File Offset: 0x0001ABFA
		public void Reset()
		{
			this.mNumberAcc = 0f;
			this.mCurNumberVariation = 0f;
			this.mParticlesEmitted = 0;
			this.mTicks = 0;
		}

		// Token: 0x0400060E RID: 1550
		public float mNumberAcc;

		// Token: 0x0400060F RID: 1551
		public float mCurNumberVariation;

		// Token: 0x04000610 RID: 1552
		public int mParticlesEmitted;

		// Token: 0x04000611 RID: 1553
		public int mTicks;
	}
}
