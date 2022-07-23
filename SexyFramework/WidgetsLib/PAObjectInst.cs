using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001C1 RID: 449
	public class PAObjectInst
	{
		// Token: 0x0600104D RID: 4173 RVA: 0x0004D91C File Offset: 0x0004BB1C
		public PAObjectInst()
		{
			this.mName = null;
			this.mSpriteInst = null;
			this.mPredrawCallback = true;
			this.mPostdrawCallback = true;
			this.mImagePredrawCallback = true;
			this.mIsBlending = false;
		}

		// Token: 0x04000D1C RID: 3356
		public string mName;

		// Token: 0x04000D1D RID: 3357
		public PASpriteInst mSpriteInst;

		// Token: 0x04000D1E RID: 3358
		public PATransform mBlendSrcTransform = new PATransform();

		// Token: 0x04000D1F RID: 3359
		public SexyColor mBlendSrcColor = default(SexyColor);

		// Token: 0x04000D20 RID: 3360
		public bool mIsBlending;

		// Token: 0x04000D21 RID: 3361
		public SexyTransform2D mTransform = new SexyTransform2D(false);

		// Token: 0x04000D22 RID: 3362
		public SexyColor mColorMult = default(SexyColor);

		// Token: 0x04000D23 RID: 3363
		public bool mPredrawCallback;

		// Token: 0x04000D24 RID: 3364
		public bool mImagePredrawCallback;

		// Token: 0x04000D25 RID: 3365
		public bool mPostdrawCallback;
	}
}
