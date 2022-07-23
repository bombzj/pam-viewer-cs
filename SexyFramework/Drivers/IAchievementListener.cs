using System;

namespace SexyFramework.Drivers
{
	// Token: 0x02000046 RID: 70
	public abstract class IAchievementListener
	{
		// Token: 0x06000354 RID: 852 RVA: 0x0000C27F File Offset: 0x0000A47F
		public virtual void Dispose()
		{
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000C281 File Offset: 0x0000A481
		public virtual void AchievementUnlocked(IAchievementContext context)
		{
			context.Destroy();
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000C289 File Offset: 0x0000A489
		public virtual void AchievementRead(IAchievementContext context)
		{
			context.Destroy();
		}
	}
}
