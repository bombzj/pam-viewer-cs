using System;
using System.Collections.Generic;

namespace JeffLib
{
	// Token: 0x02000106 RID: 262
	public class DataSyncBase
	{
		// Token: 0x060007EA RID: 2026 RVA: 0x00028596 File Offset: 0x00026796
		public virtual void SyncLong(ref int theInt)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0002859D File Offset: 0x0002679D
		public virtual void SyncFloat(ref float theInt)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x000285A4 File Offset: 0x000267A4
		public virtual void SyncListInt(List<int> theList)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x000285AB File Offset: 0x000267AB
		public virtual void SyncListFloat(List<float> theList)
		{
			throw new NotSupportedException();
		}
	}
}
