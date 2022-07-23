using System;
using System.Collections.Generic;

namespace SexyFramework.PIL
{
	// Token: 0x02000153 RID: 339
	public class SortColorKeys : Comparer<ColorKeyTimeEntry>
	{
		// Token: 0x06000BCD RID: 3021 RVA: 0x000393A9 File Offset: 0x000375A9
		public override int Compare(ColorKeyTimeEntry x, ColorKeyTimeEntry y)
		{
			if (x.first < y.first)
			{
				return -1;
			}
			if (x.first > y.first)
			{
				return 1;
			}
			return 0;
		}
	}
}
