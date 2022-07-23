using System;

namespace SexyFramework.Resource
{
	// Token: 0x02000184 RID: 388
	public abstract class DataElement
	{
		// Token: 0x06000DAD RID: 3501 RVA: 0x00044EF1 File Offset: 0x000430F1
		public DataElement()
		{
			this.mIsList = false;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00044F00 File Offset: 0x00043100
		public virtual void Dispose()
		{
		}

		// Token: 0x06000DAF RID: 3503
		public abstract DataElement Duplicate();

		// Token: 0x04000B1D RID: 2845
		public bool mIsList;
	}
}
