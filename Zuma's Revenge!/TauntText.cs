using System;

namespace ZumasRevenge
{
	// Token: 0x02000077 RID: 119
	public class TauntText
	{
		// Token: 0x06000898 RID: 2200 RVA: 0x0004C7E8 File Offset: 0x0004A9E8
		public void SyncState(DataSync sync)
		{
			sync.SyncLong(ref this.mTextId);
			sync.SyncLong(ref this.mMinDeaths);
			sync.SyncLong(ref this.mDelay);
			sync.SyncLong(ref this.mCondition);
			sync.SyncLong(ref this.mMinTime);
			sync.SyncLong(ref this.mUpdateCount);
			if (sync.isRead())
			{
				this.mText = TextManager.getInstance().getString(this.mTextId);
			}
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0004C85B File Offset: 0x0004AA5B
		public TauntText()
		{
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0004C880 File Offset: 0x0004AA80
		public TauntText(TauntText rhs)
		{
			this.mText = rhs.mText;
			this.mMinDeaths = rhs.mMinDeaths;
			this.mDelay = rhs.mDelay;
			this.mCondition = rhs.mCondition;
			this.mMinTime = rhs.mMinTime;
			this.mUpdateCount = rhs.mUpdateCount;
		}

		// Token: 0x0400065D RID: 1629
		public string mText;

		// Token: 0x0400065E RID: 1630
		public int mTextId = -1;

		// Token: 0x0400065F RID: 1631
		public int mMinDeaths = -1;

		// Token: 0x04000660 RID: 1632
		public int mDelay = 100;

		// Token: 0x04000661 RID: 1633
		public int mCondition = -1;

		// Token: 0x04000662 RID: 1634
		public int mMinTime;

		// Token: 0x04000663 RID: 1635
		public int mUpdateCount;
	}
}
