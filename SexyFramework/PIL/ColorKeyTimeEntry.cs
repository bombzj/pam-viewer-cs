using System;

namespace SexyFramework.PIL
{
	// Token: 0x02000152 RID: 338
	public class ColorKeyTimeEntry
	{
		// Token: 0x06000BCC RID: 3020 RVA: 0x00039393 File Offset: 0x00037593
		public ColorKeyTimeEntry(float pt, ColorKey key)
		{
			this.first = pt;
			this.second = key;
		}

		// Token: 0x04000966 RID: 2406
		public float first;

		// Token: 0x04000967 RID: 2407
		public ColorKey second;
	}
}
