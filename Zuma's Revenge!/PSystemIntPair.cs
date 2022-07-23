using System;
using SexyFramework.PIL;

namespace ZumasRevenge
{
	// Token: 0x02000080 RID: 128
	public class PSystemIntPair
	{
		// Token: 0x060008B5 RID: 2229 RVA: 0x0004DD7D File Offset: 0x0004BF7D
		public PSystemIntPair()
		{
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0004DD85 File Offset: 0x0004BF85
		public PSystemIntPair(SexyFramework.PIL.System f, int s)
		{
			this.first = f;
			this.second = s;
		}

		// Token: 0x040006B7 RID: 1719
		public SexyFramework.PIL.System first;

		// Token: 0x040006B8 RID: 1720
		public int second;
	}
}
