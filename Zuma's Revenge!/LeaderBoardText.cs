using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x020000EF RID: 239
	public class LeaderBoardText
	{
		// Token: 0x06000CE8 RID: 3304 RVA: 0x0007CFF5 File Offset: 0x0007B1F5
		public LeaderBoardText()
		{
			this.mAlpha = 0f;
			this.mFadeIn = true;
			this.mX = 0;
			this.mY = 0;
		}

		// Token: 0x04000B72 RID: 2930
		public string mHeaderStr = "";

		// Token: 0x04000B73 RID: 2931
		public string mValueStr = "";

		// Token: 0x04000B74 RID: 2932
		public float mAlpha;

		// Token: 0x04000B75 RID: 2933
		public bool mFadeIn;

		// Token: 0x04000B76 RID: 2934
		public int mX;

		// Token: 0x04000B77 RID: 2935
		public int mY;

		// Token: 0x04000B78 RID: 2936
		public Image mIcon;

		// Token: 0x04000B79 RID: 2937
		public bool mShowIcon;
	}
}
