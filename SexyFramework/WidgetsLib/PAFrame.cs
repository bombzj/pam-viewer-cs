using System;
using System.Collections.Generic;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001BD RID: 445
	public class PAFrame
	{
		// Token: 0x06001046 RID: 4166 RVA: 0x0004D7C8 File Offset: 0x0004B9C8
		public PAFrame()
		{
			this.mHasStop = false;
		}

		// Token: 0x04000D07 RID: 3335
		public List<PAObjectPos> mFrameObjectPosVector = new List<PAObjectPos>();

		// Token: 0x04000D08 RID: 3336
		public bool mHasStop;

		// Token: 0x04000D09 RID: 3337
		public List<PACommand> mCommandVector = new List<PACommand>();
	}
}
