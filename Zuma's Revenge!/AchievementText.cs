using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x02000004 RID: 4
	public class AchievementText
	{
		// Token: 0x0600000D RID: 13 RVA: 0x000023F0 File Offset: 0x000005F0
		public AchievementText()
		{
			this.mAlpha = 0f;
			this.mFadeIn = true;
			this.mX = 0;
			this.mY = 0;
			this.mUnlocked = false;
		}

		// Token: 0x0400000B RID: 11
		public string mHeaderStr = "";

		// Token: 0x0400000C RID: 12
		public string mValueStr = "";

		// Token: 0x0400000D RID: 13
		public string mDescStr = "";

		// Token: 0x0400000E RID: 14
		public string mPointStr = "";

		// Token: 0x0400000F RID: 15
		public float mAlpha;

		// Token: 0x04000010 RID: 16
		public bool mFadeIn;

		// Token: 0x04000011 RID: 17
		public int mX;

		// Token: 0x04000012 RID: 18
		public int mY;

		// Token: 0x04000013 RID: 19
		public Image mIcon;

		// Token: 0x04000014 RID: 20
		public bool mUnlocked;
	}
}
