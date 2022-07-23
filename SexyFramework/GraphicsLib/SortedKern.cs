using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000C7 RID: 199
	public class SortedKern : IComparable
	{
		// Token: 0x0600061A RID: 1562 RVA: 0x00015BC1 File Offset: 0x00013DC1
		public SortedKern()
		{
			this.mKey = '0';
			this.mValue = '0';
			this.mOffset = 0;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00015BE0 File Offset: 0x00013DE0
		public SortedKern(char inKey, char inValue, int inOffset)
		{
			this.mKey = inKey;
			this.mValue = inValue;
			this.mOffset = inOffset;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00015C00 File Offset: 0x00013E00
		public int CompareTo(object obj)
		{
			SortedKern sortedKern = obj as SortedKern;
			if (this.mKey < sortedKern.mKey)
			{
				return -1;
			}
			if (this.mKey > sortedKern.mKey)
			{
				return 1;
			}
			if (this.mValue < sortedKern.mValue)
			{
				return -1;
			}
			if (this.mValue > sortedKern.mValue)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00015C58 File Offset: 0x00013E58
		private static int Compare(SortedKern a, SortedKern b)
		{
			if (a.mKey < b.mKey)
			{
				return -1;
			}
			if (a.mKey > b.mKey)
			{
				return 1;
			}
			if (a.mValue < b.mValue)
			{
				return -1;
			}
			if (a.mValue > b.mValue)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x040004FE RID: 1278
		public char mKey;

		// Token: 0x040004FF RID: 1279
		public char mValue;

		// Token: 0x04000500 RID: 1280
		public int mOffset;
	}
}
