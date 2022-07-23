using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x02000036 RID: 54
	public class Steam
	{
		// Token: 0x060005CC RID: 1484 RVA: 0x0002067C File Offset: 0x0001E87C
		public void SyncState(DataSync sync)
		{
			sync.SyncFloat(ref this.mAlpha);
			sync.SyncFloat(ref this.mAlphaDec);
			sync.SyncFloat(ref this.mAngle);
			sync.SyncFloat(ref this.mAngleInc);
			sync.SyncFloat(ref this.mXOff);
			sync.SyncFloat(ref this.mYOff);
			sync.SyncFloat(ref this.mSize);
			sync.SyncFloat(ref this.mVX);
			sync.SyncFloat(ref this.mVY);
			sync.SyncLong(ref this.mImgNum);
			if (sync.isRead())
			{
				this.mImage = ((this.mImage == null) ? Res.GetImageByID(ResID.IMAGE_BOSS_STONEHEAD_FOG1) : Res.GetImageByID(ResID.IMAGE_BOSS_STONEHEAD_FOG2));
			}
		}

		// Token: 0x040002AD RID: 685
		public float mAlpha = 255f;

		// Token: 0x040002AE RID: 686
		public float mAlphaDec;

		// Token: 0x040002AF RID: 687
		public float mAngle;

		// Token: 0x040002B0 RID: 688
		public float mAngleInc;

		// Token: 0x040002B1 RID: 689
		public float mXOff;

		// Token: 0x040002B2 RID: 690
		public float mYOff;

		// Token: 0x040002B3 RID: 691
		public float mSize = 0.1f;

		// Token: 0x040002B4 RID: 692
		public float mVX;

		// Token: 0x040002B5 RID: 693
		public float mVY;

		// Token: 0x040002B6 RID: 694
		public int mImgNum;

		// Token: 0x040002B7 RID: 695
		public Image mImage;
	}
}
