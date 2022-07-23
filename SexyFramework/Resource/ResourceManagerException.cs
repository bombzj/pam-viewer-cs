using System;

namespace SexyFramework.Resource
{
	// Token: 0x0200019B RID: 411
	public class ResourceManagerException : Exception
	{
		// Token: 0x06000E74 RID: 3700 RVA: 0x000498D1 File Offset: 0x00047AD1
		public ResourceManagerException(string p)
			: base(p)
		{
			this.msg = p;
		}

		// Token: 0x04000BAC RID: 2988
		private string msg;
	}
}
