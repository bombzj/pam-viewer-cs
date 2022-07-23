using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x0200007B RID: 123
	public class BossText
	{
		// Token: 0x060008A5 RID: 2213 RVA: 0x0004CDD0 File Offset: 0x0004AFD0
		public BossText()
		{
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0004CDF6 File Offset: 0x0004AFF6
		public BossText(string t)
		{
			this.mAlpha = 0f;
			this.mText = t;
		}

		// Token: 0x04000676 RID: 1654
		public string mText = "";

		// Token: 0x04000677 RID: 1655
		public int mTextId = -1;

		// Token: 0x04000678 RID: 1656
		public float mAlpha;

		// Token: 0x04000679 RID: 1657
		public SexyColor mColor = default(SexyColor);
	}
}
