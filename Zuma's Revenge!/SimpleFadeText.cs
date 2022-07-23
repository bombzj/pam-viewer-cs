using System;

namespace ZumasRevenge
{
	// Token: 0x020000DD RID: 221
	public class SimpleFadeText
	{
		// Token: 0x06000BE0 RID: 3040 RVA: 0x0007010A File Offset: 0x0006E30A
		public SimpleFadeText()
		{
			this.mAlpha = 0f;
			this.mFadeIn = true;
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00070124 File Offset: 0x0006E324
		public SimpleFadeText(string str)
			: this()
		{
			this.mString = str;
		}

		// Token: 0x04000A5C RID: 2652
		public string mString;

		// Token: 0x04000A5D RID: 2653
		public float mAlpha;

		// Token: 0x04000A5E RID: 2654
		public bool mFadeIn;
	}
}
