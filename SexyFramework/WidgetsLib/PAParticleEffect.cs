using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Resource;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001C0 RID: 448
	public class PAParticleEffect
	{
		// Token: 0x04000D13 RID: 3347
		public ResourceRef mResourceRef = new ResourceRef();

		// Token: 0x04000D14 RID: 3348
		public PIEffect mEffect;

		// Token: 0x04000D15 RID: 3349
		public string mName;

		// Token: 0x04000D16 RID: 3350
		public int mLastUpdated;

		// Token: 0x04000D17 RID: 3351
		public bool mBehind;

		// Token: 0x04000D18 RID: 3352
		public bool mAttachEmitter;

		// Token: 0x04000D19 RID: 3353
		public bool mTransform;

		// Token: 0x04000D1A RID: 3354
		public double mXOfs;

		// Token: 0x04000D1B RID: 3355
		public double mYOfs;
	}
}
