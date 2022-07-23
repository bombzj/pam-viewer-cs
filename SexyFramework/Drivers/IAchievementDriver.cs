using System;
using SexyFramework.Drivers.Profile;

namespace SexyFramework.Drivers
{
	// Token: 0x02000047 RID: 71
	public abstract class IAchievementDriver
	{
		// Token: 0x06000358 RID: 856 RVA: 0x0000C299 File Offset: 0x0000A499
		public virtual void Dispose()
		{
		}

		// Token: 0x06000359 RID: 857
		public abstract int Init();

		// Token: 0x0600035A RID: 858
		public abstract void Update();

		// Token: 0x0600035B RID: 859
		public abstract IAchievementContext StartReadUnlockedAchievements(UserProfile p);

		// Token: 0x0600035C RID: 860
		public abstract IAchievementContext StartUnlockAchievement(UserProfile p, uint id);
	}
}
