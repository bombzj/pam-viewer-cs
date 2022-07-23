using System;
using System.Collections.Generic;

namespace SexyFramework.PIL
{
	// Token: 0x0200017B RID: 379
	public class SortOldestParticles : Comparer<Particle>
	{
		// Token: 0x06000D5D RID: 3421 RVA: 0x0004158B File Offset: 0x0003F78B
		public override int Compare(Particle x, Particle y)
		{
			if (x.mUpdateCount < y.mUpdateCount)
			{
				return 1;
			}
			if (x.mUpdateCount > y.mUpdateCount)
			{
				return -1;
			}
			return 0;
		}
	}
}
