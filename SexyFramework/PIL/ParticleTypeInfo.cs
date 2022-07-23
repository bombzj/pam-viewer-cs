using System;

namespace SexyFramework.PIL
{
	// Token: 0x0200015C RID: 348
	public class ParticleTypeInfo
	{
		// Token: 0x06000C10 RID: 3088 RVA: 0x0003A9DC File Offset: 0x00038BDC
		public ParticleTypeInfo(ParticleType f, int s)
		{
			this.first = f;
			this.second = s;
		}

		// Token: 0x04000999 RID: 2457
		public ParticleType first;

		// Token: 0x0400099A RID: 2458
		public int second;
	}
}
