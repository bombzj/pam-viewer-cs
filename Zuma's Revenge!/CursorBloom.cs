using System;

namespace ZumasRevenge
{
	// Token: 0x02000063 RID: 99
	public class CursorBloom
	{
		// Token: 0x06000730 RID: 1840 RVA: 0x00030B74 File Offset: 0x0002ED74
		public void Reset()
		{
			this.mScale = 0f;
			this.mX = 0;
			this.mY = 0;
			this.mAlpha = 255;
		}

		// Token: 0x04000486 RID: 1158
		public float mScale;

		// Token: 0x04000487 RID: 1159
		public int mX;

		// Token: 0x04000488 RID: 1160
		public int mY;

		// Token: 0x04000489 RID: 1161
		public int mAlpha = 255;
	}
}
