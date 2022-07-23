using System;

namespace SexyFramework.Misc
{
	// Token: 0x02000131 RID: 305
	public class Insets
	{
		// Token: 0x06000A1B RID: 2587 RVA: 0x0003494C File Offset: 0x00032B4C
		public Insets()
		{
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x00034954 File Offset: 0x00032B54
		public Insets(int theLeft, int theTop, int theRight, int theBottom)
		{
			this.mLeft = theLeft;
			this.mTop = theTop;
			this.mRight = theRight;
			this.mBottom = theBottom;
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00034979 File Offset: 0x00032B79
		public Insets(Insets rhs)
		{
			this.mLeft = rhs.mLeft;
			this.mTop = rhs.mTop;
			this.mRight = rhs.mRight;
			this.mBottom = rhs.mBottom;
		}

		// Token: 0x0400089B RID: 2203
		public int mLeft;

		// Token: 0x0400089C RID: 2204
		public int mTop;

		// Token: 0x0400089D RID: 2205
		public int mRight;

		// Token: 0x0400089E RID: 2206
		public int mBottom;
	}
}
