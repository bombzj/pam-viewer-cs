using System;

namespace ZumasRevenge
{
	// Token: 0x02000074 RID: 116
	public class BossWall
	{
		// Token: 0x06000894 RID: 2196 RVA: 0x0004C6E4 File Offset: 0x0004A8E4
		public BossWall()
		{
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0004C6EC File Offset: 0x0004A8EC
		public BossWall(BossWall rhs)
		{
			this.mX = rhs.mX;
			this.mY = rhs.mY;
			this.mWidth = rhs.mWidth;
			this.mHeight = rhs.mHeight;
			this.mId = rhs.mId;
			this.mAlphaFadeDir = rhs.mAlphaFadeDir;
			this.mAlpha = rhs.mAlpha;
		}

		// Token: 0x04000646 RID: 1606
		public int mX;

		// Token: 0x04000647 RID: 1607
		public int mY;

		// Token: 0x04000648 RID: 1608
		public int mWidth;

		// Token: 0x04000649 RID: 1609
		public int mHeight;

		// Token: 0x0400064A RID: 1610
		public int mId;

		// Token: 0x0400064B RID: 1611
		public int mAlphaFadeDir;

		// Token: 0x0400064C RID: 1612
		public int mAlpha;
	}
}
