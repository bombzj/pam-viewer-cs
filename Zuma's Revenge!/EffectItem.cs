using System;
using System.Collections.Generic;
using JeffLib;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x020000B3 RID: 179
	public class EffectItem
	{
		// Token: 0x06000A82 RID: 2690 RVA: 0x00065DF8 File Offset: 0x00063FF8
		public void SyncState(DataSync sync)
		{
			sync.SyncLong(ref this.mCel);
			sync.SyncLong(ref this.mColor.mRed);
			sync.SyncLong(ref this.mColor.mGreen);
			sync.SyncLong(ref this.mColor.mBlue);
			sync.SyncLong(ref this.mColor.mAlpha);
			this.SyncListComponents(sync, this.mScale, true);
			this.SyncListComponents(sync, this.mOpacity, true);
			this.SyncListComponents(sync, this.mAngle, true);
			this.SyncListComponents(sync, this.mXOffset, true);
			this.SyncListComponents(sync, this.mYOffset, true);
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00065E9C File Offset: 0x0006409C
		private void SyncListComponents(DataSync sync, List<Component> theList, bool clear)
		{
			if (sync.isRead())
			{
				if (clear)
				{
					theList.Clear();
				}
				long num = sync.GetBuffer().ReadLong();
				int num2 = 0;
				while ((long)num2 < num)
				{
					Component component = new Component();
					component.SyncState(sync);
					theList.Add(component);
					num2++;
				}
				return;
			}
			sync.GetBuffer().WriteLong((long)theList.Count);
			foreach (Component component2 in theList)
			{
				component2.SyncState(sync);
			}
		}

		// Token: 0x040008F1 RID: 2289
		public Image mImage;

		// Token: 0x040008F2 RID: 2290
		public List<Component> mScale = new List<Component>();

		// Token: 0x040008F3 RID: 2291
		public List<Component> mOpacity = new List<Component>();

		// Token: 0x040008F4 RID: 2292
		public List<Component> mAngle = new List<Component>();

		// Token: 0x040008F5 RID: 2293
		public List<Component> mXOffset = new List<Component>();

		// Token: 0x040008F6 RID: 2294
		public List<Component> mYOffset = new List<Component>();

		// Token: 0x040008F7 RID: 2295
		public int mCel;

		// Token: 0x040008F8 RID: 2296
		public SexyColor mColor = default(SexyColor);
	}
}
