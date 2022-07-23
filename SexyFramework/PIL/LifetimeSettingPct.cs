using System;

namespace SexyFramework.PIL
{
	// Token: 0x0200016C RID: 364
	public class LifetimeSettingPct
	{
		// Token: 0x06000CF2 RID: 3314 RVA: 0x0003F817 File Offset: 0x0003DA17
		public LifetimeSettingPct()
		{
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0003F81F File Offset: 0x0003DA1F
		public LifetimeSettingPct(float f, LifetimeSettings s)
		{
			this.first = f;
			this.second = s;
		}

		// Token: 0x040009F6 RID: 2550
		public float first;

		// Token: 0x040009F7 RID: 2551
		public LifetimeSettings second;
	}
}
