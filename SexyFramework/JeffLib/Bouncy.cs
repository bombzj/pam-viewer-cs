using System;

namespace JeffLib
{
	// Token: 0x02000105 RID: 261
	public class Bouncy
	{
		// Token: 0x060007DD RID: 2013 RVA: 0x0002835C File Offset: 0x0002655C
		public Bouncy()
		{
			this.mCount = 0;
			this.mMaxBounces = 0;
			this.mPct = 0f;
			this.mRate = 0f;
			this.mStartingPct = 0f;
			this.mStartInc = true;
			this.mInc = true;
			this.mDone = false;
			this.mRateDivFactor = 2f;
			this.mStartingRate = 0f;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x000283C9 File Offset: 0x000265C9
		public void Dispose()
		{
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x000283CC File Offset: 0x000265CC
		public void Update()
		{
			if (this.mDone)
			{
				return;
			}
			this.mPct += (this.mInc ? this.mRate : (-this.mRate));
			float num;
			if (this.mCount == this.mMaxBounces)
			{
				num = this.mFinalPct;
			}
			else
			{
				num = (this.mInc ? this.mMaxPct : this.mMinPct);
			}
			if (this.mInc && this.mPct >= num)
			{
				this.mPct = num;
				this.mInc = false;
				this.mCount++;
				this.mRate /= this.mRateDivFactor;
			}
			else if (!this.mInc && this.mPct <= num)
			{
				this.mPct = num;
				this.mInc = true;
				this.mCount++;
				this.mRate /= this.mRateDivFactor;
			}
			if (this.mCount > this.mMaxBounces)
			{
				this.mDone = true;
			}
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x000284CB File Offset: 0x000266CB
		public void Reset()
		{
			this.mCount = 0;
			this.mPct = this.mStartingPct;
			this.mInc = this.mStartInc;
			this.mDone = false;
			this.mRate = this.mStartingRate;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x000284FF File Offset: 0x000266FF
		public float GetPct()
		{
			return this.mPct;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00028507 File Offset: 0x00026707
		public int GetCount()
		{
			return this.mCount;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0002850F File Offset: 0x0002670F
		public bool IsDone()
		{
			return this.mDone;
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00028517 File Offset: 0x00026717
		public void SetNumBounces(int b)
		{
			this.mMaxBounces = b;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00028520 File Offset: 0x00026720
		public void SetPct(float p)
		{
			this.SetPct(p, true);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0002852C File Offset: 0x0002672C
		public void SetPct(float p, bool inc)
		{
			this.mStartingPct = p;
			this.mPct = p;
			this.mStartInc = inc;
			this.mInc = inc;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00028559 File Offset: 0x00026759
		public void SetTargetPercents(float minp, float maxp, float finalp)
		{
			this.mMinPct = minp;
			this.mMaxPct = maxp;
			this.mFinalPct = finalp;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00028570 File Offset: 0x00026770
		public void SetRate(float r)
		{
			this.mStartingRate = r;
			this.mRate = r;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0002858D File Offset: 0x0002678D
		public void SetRateDivFactor(float d)
		{
			this.mRateDivFactor = d;
		}

		// Token: 0x0400074A RID: 1866
		protected int mCount;

		// Token: 0x0400074B RID: 1867
		protected int mMaxBounces;

		// Token: 0x0400074C RID: 1868
		protected float mPct;

		// Token: 0x0400074D RID: 1869
		protected float mMaxPct;

		// Token: 0x0400074E RID: 1870
		protected float mMinPct;

		// Token: 0x0400074F RID: 1871
		protected float mFinalPct;

		// Token: 0x04000750 RID: 1872
		protected float mRate;

		// Token: 0x04000751 RID: 1873
		protected float mRateDivFactor;

		// Token: 0x04000752 RID: 1874
		protected bool mInc;

		// Token: 0x04000753 RID: 1875
		protected bool mDone;

		// Token: 0x04000754 RID: 1876
		protected float mStartingPct;

		// Token: 0x04000755 RID: 1877
		protected float mStartingRate;

		// Token: 0x04000756 RID: 1878
		protected bool mStartInc;
	}
}
