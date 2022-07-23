using System;
using SexyFramework.Misc;

namespace SexyFramework.PIL
{
	// Token: 0x0200015D RID: 349
	public class PointKeyFrame
	{
		// Token: 0x06000C11 RID: 3089 RVA: 0x0003A9F2 File Offset: 0x00038BF2
		public PointKeyFrame(int f, SexyPoint s)
		{
			this.first = f;
			this.second = s;
		}

		// Token: 0x0400099B RID: 2459
		public int first;

		// Token: 0x0400099C RID: 2460
		public SexyPoint second;
	}
}
