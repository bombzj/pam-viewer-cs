using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000E4 RID: 228
	public class PIParticleGroup
	{
		// Token: 0x060006B4 RID: 1716 RVA: 0x0001CA20 File Offset: 0x0001AC20
		public PIParticleGroup()
		{
			this.mIsSuperEmitter = false;
			this.mWasEmitted = false;
			this.mHead = null;
			this.mTail = null;
			this.mCount = 0;
		}

		// Token: 0x04000612 RID: 1554
		public PIParticleInstance mHead;

		// Token: 0x04000613 RID: 1555
		public PIParticleInstance mTail;

		// Token: 0x04000614 RID: 1556
		public int mCount;

		// Token: 0x04000615 RID: 1557
		public bool mIsSuperEmitter;

		// Token: 0x04000616 RID: 1558
		public bool mWasEmitted;
	}
}
