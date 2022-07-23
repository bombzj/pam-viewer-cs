using System;

namespace SexyFramework
{
	// Token: 0x0200004B RID: 75
	public class IFBDialogListener
	{
		// Token: 0x06000367 RID: 871 RVA: 0x0000C2C5 File Offset: 0x0000A4C5
		public virtual void DialogDidComplete(string name, StructuredData results)
		{
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000C2C7 File Offset: 0x0000A4C7
		public virtual void DialogWasCanceled(string name)
		{
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000C2C9 File Offset: 0x0000A4C9
		public virtual void DialogDidFail(string name, StructuredData error)
		{
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000C2CB File Offset: 0x0000A4CB
		public virtual bool DialogShouldOpenURLInExternalBrowser(string name, string url)
		{
			return false;
		}
	}
}
