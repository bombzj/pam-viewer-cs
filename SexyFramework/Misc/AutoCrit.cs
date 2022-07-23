using System;

namespace SexyFramework.Misc
{
	// Token: 0x02000120 RID: 288
	public struct AutoCrit
	{
		// Token: 0x0600090F RID: 2319 RVA: 0x0002EE8D File Offset: 0x0002D08D
		public AutoCrit(CritSect theCritSect)
		{
			this.mCritSec = theCritSect;
			this.mCritSec.Lock();
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0002EEA1 File Offset: 0x0002D0A1
		public void Dispose()
		{
			this.mCritSec.Unlock();
		}

		// Token: 0x04000838 RID: 2104
		private CritSect mCritSec;
	}
}
