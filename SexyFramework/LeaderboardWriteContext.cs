using System;

namespace SexyFramework
{
	// Token: 0x0200005A RID: 90
	public abstract class LeaderboardWriteContext : IAsyncTask
	{
		// Token: 0x060003A9 RID: 937 RVA: 0x0000C375 File Offset: 0x0000A575
		public override void Dispose()
		{
			base.Dispose();
		}

		// Token: 0x060003AA RID: 938
		public abstract uint GetEstimatedRank();
	}
}
