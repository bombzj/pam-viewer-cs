using System;

namespace ZumasRevenge
{
	// Token: 0x02000039 RID: 57
	public class BossFiringState
	{
		// Token: 0x060005ED RID: 1517 RVA: 0x0002287D File Offset: 0x00020A7D
		public BossFiringState()
		{
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00022890 File Offset: 0x00020A90
		public BossFiringState(BossFiringState rhs)
		{
			this.mState = rhs.mState;
			this.mPawYOffset = rhs.mPawYOffset;
			this.mSkullXOffset = rhs.mSkullXOffset;
			this.mSkullYOffset = rhs.mSkullYOffset;
			this.mSkullAngle = rhs.mSkullAngle;
			this.mHeadAngle = rhs.mHeadAngle;
			this.mSkullGrowPct = rhs.mSkullGrowPct;
			this.mTargetSkullAngle = rhs.mTargetSkullAngle;
			this.mSkullAngleInc = rhs.mSkullAngleInc;
			this.mSwipeFrame = rhs.mSwipeFrame;
			this.mTimer = rhs.mTimer;
			this.mStreaksAlpha = rhs.mStreaksAlpha;
			this.mBulletId = rhs.mBulletId;
		}

		// Token: 0x040002D8 RID: 728
		public int mState;

		// Token: 0x040002D9 RID: 729
		public float mPawYOffset;

		// Token: 0x040002DA RID: 730
		public float mSkullXOffset;

		// Token: 0x040002DB RID: 731
		public float mSkullYOffset;

		// Token: 0x040002DC RID: 732
		public float mSkullAngle;

		// Token: 0x040002DD RID: 733
		public float mHeadAngle;

		// Token: 0x040002DE RID: 734
		public float mSkullGrowPct = 1f;

		// Token: 0x040002DF RID: 735
		public float mTargetSkullAngle;

		// Token: 0x040002E0 RID: 736
		public float mSkullAngleInc;

		// Token: 0x040002E1 RID: 737
		public int mSwipeFrame;

		// Token: 0x040002E2 RID: 738
		public int mTimer;

		// Token: 0x040002E3 RID: 739
		public float mStreaksAlpha;

		// Token: 0x040002E4 RID: 740
		public int mBulletId;
	}
}
