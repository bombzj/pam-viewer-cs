using System;
using JeffLib;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x02000023 RID: 35
	public class Feather
	{
		// Token: 0x06000563 RID: 1379 RVA: 0x0001A6EC File Offset: 0x000188EC
		public void SyncState(DataSync sync)
		{
			sync.SyncFloat(ref this.mX);
			sync.SyncFloat(ref this.mY);
			sync.SyncFloat(ref this.mVX);
			sync.SyncFloat(ref this.mVY);
			sync.SyncFloat(ref this.mDecVX);
			sync.SyncFloat(ref this.mDecVY);
			sync.SyncFloat(ref this.mAlpha);
			sync.SyncLong(ref this.mImgNum);
			if (sync.isRead())
			{
				this.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_LAME_FEATHER1 + (this.mImgNum - 1));
			}
			sync.SyncFloat(ref this.mAngleOsc.mVal);
			sync.SyncFloat(ref this.mAngleOsc.mMinVal);
			sync.SyncFloat(ref this.mAngleOsc.mMaxVal);
			sync.SyncFloat(ref this.mAngleOsc.mInc);
			sync.SyncFloat(ref this.mAngleOsc.mAccel);
			sync.SyncBoolean(ref this.mAngleOsc.mForward);
		}

		// Token: 0x0400021D RID: 541
		public Image mImage;

		// Token: 0x0400021E RID: 542
		public float mX;

		// Token: 0x0400021F RID: 543
		public float mY;

		// Token: 0x04000220 RID: 544
		public float mVX;

		// Token: 0x04000221 RID: 545
		public float mVY;

		// Token: 0x04000222 RID: 546
		public float mDecVX;

		// Token: 0x04000223 RID: 547
		public float mDecVY;

		// Token: 0x04000224 RID: 548
		public float mAlpha;

		// Token: 0x04000225 RID: 549
		public int mImgNum;

		// Token: 0x04000226 RID: 550
		public Oscillator mAngleOsc = new Oscillator();
	}
}
