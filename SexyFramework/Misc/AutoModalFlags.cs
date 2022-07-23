using System;

namespace SexyFramework.Misc
{
	// Token: 0x02000130 RID: 304
	public class AutoModalFlags
	{
		// Token: 0x06000A1A RID: 2586 RVA: 0x00034936 File Offset: 0x00032B36
		public AutoModalFlags(ModalFlags theFlags, FlagsMod mWidgetFlagsMod)
		{
			this.theFlags = theFlags;
			this.mWidgetFlagsMod = mWidgetFlagsMod;
		}

		// Token: 0x04000899 RID: 2201
		private ModalFlags theFlags;

		// Token: 0x0400089A RID: 2202
		private FlagsMod mWidgetFlagsMod;
	}
}
