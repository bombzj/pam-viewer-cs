using System;

namespace SexyFramework.Misc
{
	// Token: 0x02000137 RID: 311
	public class MTAutoDisallowRand
	{
		// Token: 0x06000A3E RID: 2622 RVA: 0x00034EA0 File Offset: 0x000330A0
		public MTAutoDisallowRand()
		{
			MTRand.SetRandAllowed(false);
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x00034EAE File Offset: 0x000330AE
		public void Dispose()
		{
			MTRand.SetRandAllowed(true);
		}
	}
}
