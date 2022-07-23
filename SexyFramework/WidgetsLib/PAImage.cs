using System;
using System.Collections.Generic;
using SexyFramework.GraphicsLib;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001BA RID: 442
	public class PAImage
	{
		// Token: 0x04000CF1 RID: 3313
		public List<SharedImageRef> mImages = new List<SharedImageRef>();

		// Token: 0x04000CF2 RID: 3314
		public int mOrigWidth;

		// Token: 0x04000CF3 RID: 3315
		public int mOrigHeight;

		// Token: 0x04000CF4 RID: 3316
		public int mCols;

		// Token: 0x04000CF5 RID: 3317
		public int mRows;

		// Token: 0x04000CF6 RID: 3318
		public string mImageName;

		// Token: 0x04000CF7 RID: 3319
		public int mDrawMode;

		// Token: 0x04000CF8 RID: 3320
		public PATransform mTransform = new PATransform();
	}
}
