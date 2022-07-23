using System;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x02000116 RID: 278
	public interface ButtonListener
	{
		// Token: 0x060008BE RID: 2238
		void ButtonPress(int theId);

		// Token: 0x060008BF RID: 2239
		void ButtonPress(int theId, int theClickCount);

		// Token: 0x060008C0 RID: 2240
		void ButtonDepress(int theId);

		// Token: 0x060008C1 RID: 2241
		void ButtonDownTick(int theId);

		// Token: 0x060008C2 RID: 2242
		void ButtonMouseEnter(int theId);

		// Token: 0x060008C3 RID: 2243
		void ButtonMouseLeave(int theId);

		// Token: 0x060008C4 RID: 2244
		void ButtonMouseMove(int theId, int theX, int theY);
	}
}
