using System;
using SexyFramework.Drivers.Leaderboard;
using SexyFramework.Drivers.Profile;

namespace SexyFramework
{
	// Token: 0x0200005B RID: 91
	public abstract class ILeaderboardDriver
	{
		// Token: 0x060003AC RID: 940 RVA: 0x0000C385 File Offset: 0x0000A585
		public virtual void Dispose()
		{
		}

		// Token: 0x060003AD RID: 941
		public abstract int Init();

		// Token: 0x060003AE RID: 942
		public abstract void Update();

		// Token: 0x060003AF RID: 943
		public abstract void RegisterSchema(string id, LeaderboardSchema schema);

		// Token: 0x060003B0 RID: 944
		public abstract LeaderboardSchema GetSchema(string id);

		// Token: 0x060003B1 RID: 945
		public abstract uint MaxReadEntries();

		// Token: 0x060003B2 RID: 946
		public abstract LeaderboardWriteContext StartWriteScore(UserProfile player, string leaderboardId, string secondaryId, LeaderboardEntry entry);

		// Token: 0x060003B3 RID: 947
		public abstract LeaderboardReadContext StartReadScores(UserProfile player, string leaderboardId, string secondaryId, Leaderboard.Type type, uint startRank, uint maxEntries);
	}
}
