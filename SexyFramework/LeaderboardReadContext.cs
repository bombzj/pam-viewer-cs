using System;
using System.Collections.Generic;
using SexyFramework.Drivers.Leaderboard;

namespace SexyFramework
{
	// Token: 0x02000059 RID: 89
	public abstract class LeaderboardReadContext : IAsyncTask
	{
		// Token: 0x060003A2 RID: 930 RVA: 0x0000C331 File Offset: 0x0000A531
		public override void Dispose()
		{
			base.Dispose();
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000C339 File Offset: 0x0000A539
		public uint GetStartRow()
		{
			return this.mStartRank;
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000C341 File Offset: 0x0000A541
		public uint GetNumRows()
		{
			return this.mNumEntries;
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000C349 File Offset: 0x0000A549
		public uint GetTotalNumRows()
		{
			return this.mTotalNumEntries;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000C351 File Offset: 0x0000A551
		public virtual int GetUserRow()
		{
			return -1;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000C354 File Offset: 0x0000A554
		public LeaderboardEntry GetRow(int index)
		{
			return this.mData[index];
		}

		// Token: 0x0400020A RID: 522
		protected uint mStartRank;

		// Token: 0x0400020B RID: 523
		protected uint mNumEntries;

		// Token: 0x0400020C RID: 524
		protected uint mTotalNumEntries;

		// Token: 0x0400020D RID: 525
		protected List<LeaderboardEntry> mData = new List<LeaderboardEntry>();
	}
}
