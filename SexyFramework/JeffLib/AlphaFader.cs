using System;
using SexyFramework;

namespace JeffLib
{
	// Token: 0x0200010D RID: 269
	public class AlphaFader
	{
		// Token: 0x06000836 RID: 2102 RVA: 0x0002A058 File Offset: 0x00028258
		public void Update()
		{
			this.mColor.mAlpha += this.mFadeRate;
			if (SexyFramework.Common._geq(this.mColor.mAlpha, (float)this.mMax) && this.mFadeRate > 0f)
			{
				this.mColor.mAlpha = (float)this.mMax;
				this.mFadeRate *= -1f;
				this.mFadeCount++;
				return;
			}
			if (SexyFramework.Common._leq(this.mColor.mAlpha, (float)this.mMin) && this.mFadeRate < 0f)
			{
				this.mColor.mAlpha = (float)this.mMin;
				this.mFadeRate *= -1f;
				this.mFadeCount++;
			}
		}

		// Token: 0x0400078F RID: 1935
		public FColor mColor;

		// Token: 0x04000790 RID: 1936
		public float mFadeRate;

		// Token: 0x04000791 RID: 1937
		public int mFadeCount;

		// Token: 0x04000792 RID: 1938
		public int mMin;

		// Token: 0x04000793 RID: 1939
		public int mMax = 255;
	}
}
