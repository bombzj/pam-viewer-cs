using System;

namespace SexyFramework.Resource
{
	// Token: 0x02000187 RID: 391
	public class CharStack
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x000450FD File Offset: 0x000432FD
		public int Count
		{
			get
			{
				return this.mCount;
			}
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x0004511D File Offset: 0x0004331D
		public char Peek()
		{
			return this.mCharStack[this.mStart];
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x0004512C File Offset: 0x0004332C
		public void Pop()
		{
			this.mStart--;
			this.mCount--;
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0004514A File Offset: 0x0004334A
		public void Push(char c)
		{
			this.mStart++;
			this.mCharStack[this.mStart] = c;
			this.mCount++;
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00045178 File Offset: 0x00043378
		public void Clear()
		{
			this.mCount = 0;
			this.mStart = 0;
			for (int i = 0; i < 2097152; i++)
			{
				this.mCharStack[i] = '0';
			}
		}

		// Token: 0x04000B21 RID: 2849
		private char[] mCharStack = new char[2097152];

		// Token: 0x04000B22 RID: 2850
		private int mCount;

		// Token: 0x04000B23 RID: 2851
		private int mStart;
	}
}
