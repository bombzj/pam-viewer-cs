using System;
using System.Collections.Generic;

namespace SexyFramework.Drivers
{
	// Token: 0x02000045 RID: 69
	public abstract class IAchievementContext : IAsyncTask
	{
		// Token: 0x06000350 RID: 848 RVA: 0x0000C26F File Offset: 0x0000A46F
		public override void Dispose()
		{
			base.Dispose();
		}

		// Token: 0x06000351 RID: 849
		public abstract void SetListener(IAchievementListener listener);

		// Token: 0x06000352 RID: 850
		public abstract List<uint> GetUnlockedAchievements();
	}
}
