using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x020000E3 RID: 227
	public class FrogBody
	{
		// Token: 0x06000C2C RID: 3116 RVA: 0x00071770 File Offset: 0x0006F970
		public void SyncState(DataSync sync)
		{
			sync.SyncLong(ref this.mAlpha);
			sync.SyncLong(ref this.mCel);
		}

		// Token: 0x04000AAA RID: 2730
		public Image mShadow;

		// Token: 0x04000AAB RID: 2731
		public Image mLegs;

		// Token: 0x04000AAC RID: 2732
		public Image mMouth;

		// Token: 0x04000AAD RID: 2733
		public Image mBody;

		// Token: 0x04000AAE RID: 2734
		public Image mEyes;

		// Token: 0x04000AAF RID: 2735
		public Image mTongue;

		// Token: 0x04000AB0 RID: 2736
		public Image mLazerEyeLoop;

		// Token: 0x04000AB1 RID: 2737
		public SexyPoint mLegsOffset = new SexyPoint();

		// Token: 0x04000AB2 RID: 2738
		public SexyPoint mMouthOffset = new SexyPoint();

		// Token: 0x04000AB3 RID: 2739
		public SexyPoint mBodyOffset = new SexyPoint();

		// Token: 0x04000AB4 RID: 2740
		public SexyPoint mEyesOffset = new SexyPoint();

		// Token: 0x04000AB5 RID: 2741
		public FrogType mType;

		// Token: 0x04000AB6 RID: 2742
		public int mTongueX;

		// Token: 0x04000AB7 RID: 2743
		public int mCX;

		// Token: 0x04000AB8 RID: 2744
		public int mCY;

		// Token: 0x04000AB9 RID: 2745
		public int mNextBallX;

		// Token: 0x04000ABA RID: 2746
		public int mNextBallY;

		// Token: 0x04000ABB RID: 2747
		public int mAlpha;

		// Token: 0x04000ABC RID: 2748
		public int mCel;
	}
}
