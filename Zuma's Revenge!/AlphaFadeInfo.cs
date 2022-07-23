using System;
using JeffLib;

namespace ZumasRevenge
{
	// Token: 0x020000AC RID: 172
	public class AlphaFadeInfo
	{
		// Token: 0x06000A5B RID: 2651 RVA: 0x000621B4 File Offset: 0x000603B4
		public AlphaFadeInfo()
		{
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x000621BC File Offset: 0x000603BC
		public AlphaFadeInfo(AlphaFader f, bool s)
		{
			this.first = f;
			this.second = s;
		}

		// Token: 0x040008C0 RID: 2240
		public AlphaFader first;

		// Token: 0x040008C1 RID: 2241
		public bool second;
	}
}
