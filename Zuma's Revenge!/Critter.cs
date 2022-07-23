using System;
using JeffLib;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x020000A4 RID: 164
	public class Critter
	{
		// Token: 0x0400086E RID: 2158
		public float mInitVel;

		// Token: 0x0400086F RID: 2159
		public float mVX;

		// Token: 0x04000870 RID: 2160
		public float mVY;

		// Token: 0x04000871 RID: 2161
		public float mAX;

		// Token: 0x04000872 RID: 2162
		public float mAY;

		// Token: 0x04000873 RID: 2163
		public int mTimer;

		// Token: 0x04000874 RID: 2164
		public float mAngle;

		// Token: 0x04000875 RID: 2165
		public float mTargetAngle;

		// Token: 0x04000876 RID: 2166
		public float mAngleInc;

		// Token: 0x04000877 RID: 2167
		public float mX;

		// Token: 0x04000878 RID: 2168
		public float mY;

		// Token: 0x04000879 RID: 2169
		public Image mImage;

		// Token: 0x0400087A RID: 2170
		public int mCel;

		// Token: 0x0400087B RID: 2171
		public int mState;

		// Token: 0x0400087C RID: 2172
		public int mAnimRate;

		// Token: 0x0400087D RID: 2173
		public float mAlpha;

		// Token: 0x0400087E RID: 2174
		public bool mFadeOut;

		// Token: 0x0400087F RID: 2175
		public float mSize;

		// Token: 0x04000880 RID: 2176
		public int mRotateDelay;

		// Token: 0x04000881 RID: 2177
		public int mUpdateCount;

		// Token: 0x04000882 RID: 2178
		public CommonColorFader mFader = new CommonColorFader();
	}
}
