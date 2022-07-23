using System;

namespace JeffLib
{
	// Token: 0x02000100 RID: 256
	public class FadeData
	{
		// Token: 0x06000770 RID: 1904 RVA: 0x00026878 File Offset: 0x00024A78
		public FadeData()
		{
			this.mFadeState = 0;
			this.mFadeOutTarget = (this.mFadeInTarget = 0);
			this.mFadeOutRate = (this.mFadeInRate = 0);
			this.mVal = 0;
			this.mFadeCount = 0;
			this.mStopWhenDone = true;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x000268C8 File Offset: 0x00024AC8
		public FadeData(FadeData fd)
		{
			this.mFadeState = fd.mFadeState;
			this.mFadeOutTarget = fd.mFadeOutTarget;
			this.mFadeOutRate = fd.mFadeOutRate;
			this.mVal = fd.mVal;
			this.mFadeCount = fd.mFadeCount;
			this.mStopWhenDone = fd.mStopWhenDone;
		}

		// Token: 0x040006FE RID: 1790
		public int mFadeState;

		// Token: 0x040006FF RID: 1791
		public int mFadeOutRate;

		// Token: 0x04000700 RID: 1792
		public int mFadeInRate;

		// Token: 0x04000701 RID: 1793
		public int mFadeOutTarget;

		// Token: 0x04000702 RID: 1794
		public int mFadeInTarget;

		// Token: 0x04000703 RID: 1795
		public int mVal;

		// Token: 0x04000704 RID: 1796
		public int mFadeCount;

		// Token: 0x04000705 RID: 1797
		public bool mStopWhenDone;

		// Token: 0x02000101 RID: 257
		public enum FadeType
		{
			// Token: 0x04000707 RID: 1799
			Fade_None,
			// Token: 0x04000708 RID: 1800
			Fade_Out,
			// Token: 0x04000709 RID: 1801
			Fade_In
		}
	}
}
