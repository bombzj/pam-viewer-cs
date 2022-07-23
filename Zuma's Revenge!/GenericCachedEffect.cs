using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x020000CB RID: 203
	public class GenericCachedEffect
	{
		// Token: 0x06000AE2 RID: 2786 RVA: 0x0006B03C File Offset: 0x0006923C
		public GenericCachedEffect(PIEffect e)
		{
			this.mInUse = false;
			this.mEffect = e;
		}

		// Token: 0x0400098C RID: 2444
		public bool mInUse;

		// Token: 0x0400098D RID: 2445
		public PIEffect mEffect;
	}
}
