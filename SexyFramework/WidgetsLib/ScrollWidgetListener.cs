using System;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001CC RID: 460
	public interface ScrollWidgetListener
	{
		// Token: 0x060010A6 RID: 4262
		void ScrollTargetReached(ScrollWidget scrollWidget);

		// Token: 0x060010A7 RID: 4263
		void ScrollTargetInterrupted(ScrollWidget scrollWidget);
	}
}
