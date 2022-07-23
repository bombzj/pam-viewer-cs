using System;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001A2 RID: 418
	public interface DialogListener
	{
		// Token: 0x06000EA9 RID: 3753
		void DialogButtonPress(int theDialogId, int theButtonId);

		// Token: 0x06000EAA RID: 3754
		void DialogButtonDepress(int theDialogId, int theButtonId);
	}
}
