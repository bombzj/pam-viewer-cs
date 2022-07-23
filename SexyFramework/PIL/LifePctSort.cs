using System;
using System.Collections.Generic;

namespace SexyFramework.PIL
{
	// Token: 0x0200016A RID: 362
	public class LifePctSort : Comparer<LifetimeSettingPct>
	{
		// Token: 0x06000CEE RID: 3310 RVA: 0x0003F7CE File Offset: 0x0003D9CE
		public override int Compare(LifetimeSettingPct x, LifetimeSettingPct y)
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
