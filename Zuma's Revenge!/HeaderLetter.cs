using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x020000BF RID: 191
	public class HeaderLetter
	{
		// Token: 0x06000ABB RID: 2747 RVA: 0x0006866E File Offset: 0x0006686E
		public HeaderLetter(Image img)
		{
			this.mImage = img;
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0006867D File Offset: 0x0006687D
		public HeaderLetter()
		{
			this.mImage = null;
		}

		// Token: 0x0400092A RID: 2346
		public Image mImage;

		// Token: 0x0400092B RID: 2347
		public float mAngle;

		// Token: 0x0400092C RID: 2348
		public float mAngleInc;

		// Token: 0x0400092D RID: 2349
		public float mVX;

		// Token: 0x0400092E RID: 2350
		public float mVY;

		// Token: 0x0400092F RID: 2351
		public float mX;

		// Token: 0x04000930 RID: 2352
		public float mY;

		// Token: 0x04000931 RID: 2353
		public float mAngleAccel;

		// Token: 0x04000932 RID: 2354
		public bool mHinge;

		// Token: 0x04000933 RID: 2355
		public int mSwingCount;

		// Token: 0x04000934 RID: 2356
		public int mUpdateCount;
	}
}
