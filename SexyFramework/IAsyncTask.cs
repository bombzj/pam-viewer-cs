using System;

namespace SexyFramework
{
	// Token: 0x02000044 RID: 68
	public abstract class IAsyncTask
	{
		// Token: 0x0600034B RID: 843 RVA: 0x0000C265 File Offset: 0x0000A465
		public virtual void Dispose()
		{
		}

		// Token: 0x0600034C RID: 844
		public abstract int IsDone();

		// Token: 0x0600034D RID: 845
		public abstract int HasError();

		// Token: 0x0600034E RID: 846
		public abstract void Destroy();
	}
}
