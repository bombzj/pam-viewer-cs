using System;

namespace SexyFramework
{
	// Token: 0x0200001B RID: 27
	internal class GamepadData
	{
		// Token: 0x04000050 RID: 80
		public float[] mAxis = new float[4];

		// Token: 0x04000051 RID: 81
		public bool[] mButton = new bool[20];

		// Token: 0x04000052 RID: 82
		private float[] mButtonPressure = new float[20];

		// Token: 0x04000053 RID: 83
		public int[] mLastRepeat = new int[20];

		// Token: 0x04000054 RID: 84
		public int[] mStartRepeat = new int[20];

		// Token: 0x04000055 RID: 85
		public int[] mRepeatMax = new int[20];
	}
}
