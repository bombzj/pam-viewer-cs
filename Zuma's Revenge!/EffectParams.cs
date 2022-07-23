using System;

namespace ZumasRevenge
{
	// Token: 0x020000FC RID: 252
	public class EffectParams
	{
		// Token: 0x06000D23 RID: 3363 RVA: 0x0007FFBF File Offset: 0x0007E1BF
		public EffectParams()
		{
			this.mEffectIndex = -1;
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0007FFCE File Offset: 0x0007E1CE
		public EffectParams(string k, string v, int i)
		{
			this.mKey = k;
			this.mValue = v;
			this.mEffectIndex = i;
		}

		// Token: 0x04000C01 RID: 3073
		public string mKey;

		// Token: 0x04000C02 RID: 3074
		public string mValue;

		// Token: 0x04000C03 RID: 3075
		public int mEffectIndex;
	}
}
