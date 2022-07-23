using System;
using System.Collections.Generic;

namespace ZumasRevenge
{
	// Token: 0x020000A8 RID: 168
	public class ImageSort : Comparer<FogElement>
	{
		// Token: 0x06000A36 RID: 2614 RVA: 0x000618BA File Offset: 0x0005FABA
		public override int Compare(FogElement x, FogElement y)
		{
			if (x.mImage == y.mImage)
			{
				return 0;
			}
			return -1;
		}
	}
}
