using System;
using System.Collections.Generic;

namespace SexyFramework.PIL
{
	// Token: 0x02000169 RID: 361
	public class LifeFrameSort : Comparer<LifetimeSettingKeyFrame>
	{
		// Token: 0x06000CEC RID: 3308 RVA: 0x0003F7A3 File Offset: 0x0003D9A3
		public override int Compare(LifetimeSettingKeyFrame x, LifetimeSettingKeyFrame y)
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
