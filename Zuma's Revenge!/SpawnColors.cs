using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x020000AF RID: 175
	public class SpawnColors
	{
		// Token: 0x06000A63 RID: 2659 RVA: 0x000625EF File Offset: 0x000607EF
		public SpawnColors(SexyColor b, SexyColor r, SexyColor s)
		{
			this.mBeam = b;
			this.mRings = r;
			this.mSwirl = s;
		}

		// Token: 0x040008C9 RID: 2249
		public SexyColor mBeam;

		// Token: 0x040008CA RID: 2250
		public SexyColor mRings;

		// Token: 0x040008CB RID: 2251
		public SexyColor mSwirl;
	}
}
