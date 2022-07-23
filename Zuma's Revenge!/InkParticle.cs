using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x0200002F RID: 47
	public class InkParticle
	{
		// Token: 0x060005A5 RID: 1445 RVA: 0x0001D7A4 File Offset: 0x0001B9A4
		public void SyncState(DataSync s)
		{
			SexyBuffer buffer = s.GetBuffer();
			if (s.isRead())
			{
				buffer.WriteBoolean(this.mImage == Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_GLOBULE1));
			}
			else if (buffer.ReadBoolean())
			{
				this.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_GLOBULE1);
			}
			else
			{
				this.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_GLOBULE2);
			}
			s.SyncFloat(ref this.mWidthPct);
			s.SyncFloat(ref this.mHeightPct);
			s.SyncFloat(ref this.mX);
			s.SyncFloat(ref this.mY);
			s.SyncFloat(ref this.mAngle);
			s.SyncFloat(ref this.mInitSpeed);
			s.SyncFloat(ref this.mVX);
			s.SyncFloat(ref this.mVY);
			s.SyncFloat(ref this.mGravity);
			s.SyncFloat(ref this.mAlpha);
			s.SyncFloat(ref this.mAlphaRate);
			s.SyncFloat(ref this.mJiggleRate);
			s.SyncLong(ref this.mJiggleDir);
			s.SyncLong(ref this.mPostHitCount);
		}

		// Token: 0x04000276 RID: 630
		public float mWidthPct;

		// Token: 0x04000277 RID: 631
		public float mHeightPct;

		// Token: 0x04000278 RID: 632
		public float mAngle;

		// Token: 0x04000279 RID: 633
		public float mX;

		// Token: 0x0400027A RID: 634
		public float mY;

		// Token: 0x0400027B RID: 635
		public Image mImage;

		// Token: 0x0400027C RID: 636
		public float mInitSpeed;

		// Token: 0x0400027D RID: 637
		public float mVX;

		// Token: 0x0400027E RID: 638
		public float mVY;

		// Token: 0x0400027F RID: 639
		public float mGravity;

		// Token: 0x04000280 RID: 640
		public float mAlpha;

		// Token: 0x04000281 RID: 641
		public float mAlphaRate;

		// Token: 0x04000282 RID: 642
		public float mJiggleRate;

		// Token: 0x04000283 RID: 643
		public int mJiggleDir;

		// Token: 0x04000284 RID: 644
		public int mPostHitCount;
	}
}
