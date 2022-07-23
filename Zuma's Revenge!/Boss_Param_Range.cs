using System;

namespace ZumasRevenge
{
	// Token: 0x0200009A RID: 154
	public class Boss_Param_Range
	{
		// Token: 0x060009E7 RID: 2535 RVA: 0x0005E6F3 File Offset: 0x0005C8F3
		public void Init()
		{
			this.mMin = 0f;
			this.mMax = 0f;
			this.mRatingMin = -1f;
			this.mRatingMax = -1f;
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0005E721 File Offset: 0x0005C921
		public bool InRange(float amt)
		{
			return this.mRatingMin < 0f || this.mRatingMax < 0f || (amt >= this.mRatingMin && amt < this.mRatingMax);
		}

		// Token: 0x0400080C RID: 2060
		public float mMin;

		// Token: 0x0400080D RID: 2061
		public float mMax;

		// Token: 0x0400080E RID: 2062
		public float mRatingMin;

		// Token: 0x0400080F RID: 2063
		public float mRatingMax;
	}
}
