using System;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001D3 RID: 467
	public interface SliderListener
	{
		// Token: 0x060010EF RID: 4335
		void SliderVal(int theId, double theVal);

		// Token: 0x060010F0 RID: 4336
		void SliderReleased(int theId, double theVal);
	}
}
