using System;

namespace SexyFramework.PIL
{
	// Token: 0x0200015E RID: 350
	public class FreeEmitterInfo
	{
		// Token: 0x06000C12 RID: 3090 RVA: 0x0003AA08 File Offset: 0x00038C08
		public FreeEmitterInfo(FreeEmitter f, int s)
		{
			this.first = f;
			this.second = s;
		}

		// Token: 0x0400099D RID: 2461
		public FreeEmitter first;

		// Token: 0x0400099E RID: 2462
		public int second;
	}
}
