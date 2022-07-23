using System;

namespace SexyFramework.PIL
{
	// Token: 0x0200016B RID: 363
	public class LifetimeSettingKeyFrame
	{
		// Token: 0x06000CF0 RID: 3312 RVA: 0x0003F7F9 File Offset: 0x0003D9F9
		public LifetimeSettingKeyFrame()
		{
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0003F801 File Offset: 0x0003DA01
		public LifetimeSettingKeyFrame(int f, LifetimeSettings s)
		{
			this.first = f;
			this.second = s;
		}

		// Token: 0x040009F4 RID: 2548
		public int first;

		// Token: 0x040009F5 RID: 2549
		public LifetimeSettings second;
	}
}
