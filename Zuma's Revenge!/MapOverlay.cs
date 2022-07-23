using System;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x0200011F RID: 287
	public class MapOverlay
	{
		// Token: 0x04000E67 RID: 3687
		public float mAlpha;

		// Token: 0x04000E68 RID: 3688
		public bool mUnlocked;

		// Token: 0x04000E69 RID: 3689
		public FPoint[] mCloudPoints = new FPoint[]
		{
			new FPoint(),
			new FPoint(),
			new FPoint()
		};

		// Token: 0x04000E6A RID: 3690
		public FPoint[] mCloudSizes = new FPoint[]
		{
			new FPoint(),
			new FPoint(),
			new FPoint()
		};
	}
}
