using System;
using System.Collections.Generic;

namespace SexyFramework.PIL
{
	// Token: 0x0200015B RID: 347
	public class SortPointKeyFrames : Comparer<PointKeyFrame>
	{
		// Token: 0x06000C0E RID: 3086 RVA: 0x0003A9B1 File Offset: 0x00038BB1
		public override int Compare(PointKeyFrame x, PointKeyFrame y)
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
