using System;
using System.Collections.Generic;

namespace SexyFramework.PIL
{
	// Token: 0x0200017F RID: 383
	public class KeyFrameSort : Comparer<KeyFrame>
	{
		// Token: 0x06000D8B RID: 3467 RVA: 0x00043C94 File Offset: 0x00041E94
		public override int Compare(KeyFrame x, KeyFrame y)
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
