using System;

namespace ZumasRevenge
{
	// Token: 0x0200012E RID: 302
	public class GauntletHSInfo
	{
		// Token: 0x06000F3F RID: 3903 RVA: 0x0009D6DF File Offset: 0x0009B8DF
		public GauntletHSInfo()
		{
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0009D6F2 File Offset: 0x0009B8F2
		public GauntletHSInfo(int score, string n)
		{
			this.mScore = score;
			this.mProfileName = n;
		}

		// Token: 0x04000F03 RID: 3843
		public int mScore;

		// Token: 0x04000F04 RID: 3844
		public string mProfileName = "";
	}
}
