using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x02000053 RID: 83
	public class WakeStruct
	{
		// Token: 0x040003F7 RID: 1015
		public uint mBallId;

		// Token: 0x040003F8 RID: 1016
		public SexyVector2 mVel = default(SexyVector2);

		// Token: 0x040003F9 RID: 1017
		public float mX;

		// Token: 0x040003FA RID: 1018
		public float mY;

		// Token: 0x040003FB RID: 1019
		public float mAngle;

		// Token: 0x040003FC RID: 1020
		public float mSize = 1f;

		// Token: 0x040003FD RID: 1021
		public float mAlpha = 255f;

		// Token: 0x040003FE RID: 1022
		public float mAlphaInc;

		// Token: 0x040003FF RID: 1023
		public bool mAdditive;

		// Token: 0x04000400 RID: 1024
		public bool mExpanding;

		// Token: 0x04000401 RID: 1025
		public bool mIsHead;

		// Token: 0x04000402 RID: 1026
		public int mUpdateCount;

		// Token: 0x04000403 RID: 1027
		public Image mImage;
	}
}
