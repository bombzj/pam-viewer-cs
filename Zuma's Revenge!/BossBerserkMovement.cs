using System;
using System.Collections.Generic;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x0200007D RID: 125
	public class BossBerserkMovement
	{
		// Token: 0x060008AB RID: 2219 RVA: 0x0004D1A4 File Offset: 0x0004B3A4
		public BossBerserkMovement()
		{
			this.mStartX = 0;
			this.mStartY = 0;
			this.mEndX = 0;
			this.mEndY = 0;
			this.mHealthLimit = -1;
			this.mX = int.MaxValue;
			this.mY = int.MaxValue;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0004D1FC File Offset: 0x0004B3FC
		public BossBerserkMovement(BossBerserkMovement rhs)
		{
			this.mStartX = rhs.mStartX;
			this.mStartY = rhs.mStartY;
			this.mEndX = rhs.mEndX;
			this.mEndY = rhs.mEndY;
			this.mHealthLimit = rhs.mHealthLimit;
			this.mX = rhs.mX;
			this.mY = rhs.mY;
			this.mPoints.Clear();
			for (int i = 0; i < rhs.mPoints.Count; i++)
			{
				this.mPoints.Add(new SexyPoint(rhs.mPoints[i]));
			}
		}

		// Token: 0x04000697 RID: 1687
		public int mStartX;

		// Token: 0x04000698 RID: 1688
		public int mEndX;

		// Token: 0x04000699 RID: 1689
		public int mStartY;

		// Token: 0x0400069A RID: 1690
		public int mEndY;

		// Token: 0x0400069B RID: 1691
		public int mX;

		// Token: 0x0400069C RID: 1692
		public int mY;

		// Token: 0x0400069D RID: 1693
		public int mHealthLimit;

		// Token: 0x0400069E RID: 1694
		public List<SexyPoint> mPoints = new List<SexyPoint>();
	}
}
