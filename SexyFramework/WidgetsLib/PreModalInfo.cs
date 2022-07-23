using System;
using SexyFramework.Misc;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001D7 RID: 471
	public class PreModalInfo
	{
		// Token: 0x04000DEF RID: 3567
		public Widget mBaseModalWidget;

		// Token: 0x04000DF0 RID: 3568
		public Widget mPrevBaseModalWidget;

		// Token: 0x04000DF1 RID: 3569
		public Widget mPrevFocusWidget;

		// Token: 0x04000DF2 RID: 3570
		public FlagsMod mPrevBelowModalFlagsMod = new FlagsMod();
	}
}
