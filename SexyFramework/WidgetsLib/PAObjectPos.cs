using System;
using SexyFramework.Misc;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001BB RID: 443
	public class PAObjectPos
	{
		// Token: 0x04000CF9 RID: 3321
		public string mName;

		// Token: 0x04000CFA RID: 3322
		public int mObjectNum;

		// Token: 0x04000CFB RID: 3323
		public bool mIsSprite;

		// Token: 0x04000CFC RID: 3324
		public bool mIsAdditive;

		// Token: 0x04000CFD RID: 3325
		public bool mHasSrcRect;

		// Token: 0x04000CFE RID: 3326
		public byte mResNum;

		// Token: 0x04000CFF RID: 3327
		public int mPreloadFrames;

		// Token: 0x04000D00 RID: 3328
		public int mAnimFrameNum;

		// Token: 0x04000D01 RID: 3329
		public float mTimeScale;

		// Token: 0x04000D02 RID: 3330
		public PATransform mTransform = new PATransform();

		// Token: 0x04000D03 RID: 3331
		public Rect mSrcRect = default(Rect);

		// Token: 0x04000D04 RID: 3332
		public int mColorInt;
	}
}
