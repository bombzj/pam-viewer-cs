using System;

namespace SexyFramework.PIL
{
	// Token: 0x0200017E RID: 382
	public class KeyFrame
	{
		// Token: 0x06000D89 RID: 3465 RVA: 0x00043C76 File Offset: 0x00041E76
		public KeyFrame()
		{
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00043C7E File Offset: 0x00041E7E
		public KeyFrame(int k, KeyFrameData data)
		{
			this.first = k;
			this.second = data;
		}

		// Token: 0x04000B08 RID: 2824
		public int first;

		// Token: 0x04000B09 RID: 2825
		public KeyFrameData second;
	}
}
