using System;
using System.Collections.Generic;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000DC RID: 220
	public class PITexture
	{
		// Token: 0x0400056C RID: 1388
		public string mName;

		// Token: 0x0400056D RID: 1389
		public string mFileName;

		// Token: 0x0400056E RID: 1390
		public List<SharedImageRef> mImageVector = new List<SharedImageRef>();

		// Token: 0x0400056F RID: 1391
		public SharedImageRef mImageStrip = new SharedImageRef();

		// Token: 0x04000570 RID: 1392
		public int mNumCels;

		// Token: 0x04000571 RID: 1393
		public bool mPadded;
	}
}
