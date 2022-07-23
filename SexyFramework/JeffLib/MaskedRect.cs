using System;
using SexyFramework.Misc;

namespace JeffLib
{
	// Token: 0x0200010E RID: 270
	public class MaskedRect
	{
		// Token: 0x06000838 RID: 2104 RVA: 0x0002A141 File Offset: 0x00028341
		public MaskedRect()
		{
			this.r = default(Rect);
			this.a = 0;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0002A15C File Offset: 0x0002835C
		public MaskedRect(Rect _r)
		{
			this.r = new Rect(_r);
			this.a = 0;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0002A177 File Offset: 0x00028377
		public MaskedRect(Rect _r, int alpha)
		{
			this.r = new Rect(_r);
			this.a = alpha;
		}

		// Token: 0x04000794 RID: 1940
		public Rect r;

		// Token: 0x04000795 RID: 1941
		public int a;
	}
}
