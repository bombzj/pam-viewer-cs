using System;
using System.Collections.Generic;

namespace ZumasRevenge
{
	// Token: 0x0200007A RID: 122
	public class BerserkTier
	{
		// Token: 0x060008A2 RID: 2210 RVA: 0x0004CD40 File Offset: 0x0004AF40
		public BerserkTier()
		{
			this.mHealthLimit = 0;
			this.mParams = new List<BerserkModifier>();
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0004CD5A File Offset: 0x0004AF5A
		public BerserkTier(int hl)
		{
			this.mHealthLimit = hl;
			this.mParams = new List<BerserkModifier>();
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0004CD74 File Offset: 0x0004AF74
		public BerserkTier(BerserkTier rhs)
		{
			this.mHealthLimit = rhs.mHealthLimit;
			this.mParams = new List<BerserkModifier>();
			for (int i = 0; i < rhs.mParams.Count; i++)
			{
				this.mParams.Add(new BerserkModifier(rhs.mParams[i]));
			}
		}

		// Token: 0x04000674 RID: 1652
		public int mHealthLimit;

		// Token: 0x04000675 RID: 1653
		public List<BerserkModifier> mParams;
	}
}
