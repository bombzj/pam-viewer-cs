using System;
using System.Collections.Generic;

namespace ZumasRevenge
{
	// Token: 0x02000028 RID: 40
	public class MosquitoBall : IDisposable
	{
		// Token: 0x06000583 RID: 1411 RVA: 0x0001C297 File Offset: 0x0001A497
		public virtual void Dispose()
		{
			this.mMosquitoes.Clear();
		}

		// Token: 0x04000248 RID: 584
		public List<Mosquito> mMosquitoes = new List<Mosquito>();
	}
}
