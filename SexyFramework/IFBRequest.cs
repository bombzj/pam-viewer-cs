using System;

namespace SexyFramework
{
	// Token: 0x02000049 RID: 73
	public abstract class IFBRequest
	{
		// Token: 0x06000360 RID: 864 RVA: 0x0000C2AD File Offset: 0x0000A4AD
		public virtual void Dispose()
		{
		}

		// Token: 0x06000361 RID: 865
		public abstract void Cancel();
	}
}
