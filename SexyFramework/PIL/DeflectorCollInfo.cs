using System;

namespace SexyFramework.PIL
{
	// Token: 0x0200016E RID: 366
	public class DeflectorCollInfo
	{
		// Token: 0x06000CF9 RID: 3321 RVA: 0x0003FAB1 File Offset: 0x0003DCB1
		public DeflectorCollInfo()
		{
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0003FAB9 File Offset: 0x0003DCB9
		public DeflectorCollInfo(int f, bool b)
		{
			this.mLastCollFrame = f;
			this.mIgnoresDeflector = b;
		}

		// Token: 0x04000A02 RID: 2562
		public int mLastCollFrame;

		// Token: 0x04000A03 RID: 2563
		public bool mIgnoresDeflector;
	}
}
