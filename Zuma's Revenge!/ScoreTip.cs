using System;

namespace ZumasRevenge
{
	// Token: 0x02000104 RID: 260
	public struct ScoreTip
	{
		// Token: 0x06000DA3 RID: 3491 RVA: 0x0008711E File Offset: 0x0008531E
		public ScoreTip(string t, int l)
		{
			this.mTip = t;
			this.mMinLevel = l;
			this.mTipId = -1;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00087135 File Offset: 0x00085335
		public ScoreTip(string t)
		{
			this.mTip = t;
			this.mMinLevel = -1;
			this.mTipId = -1;
		}

		// Token: 0x04000CC4 RID: 3268
		public string mTip;

		// Token: 0x04000CC5 RID: 3269
		public int mTipId;

		// Token: 0x04000CC6 RID: 3270
		public int mMinLevel;
	}
}
