using System;

namespace ZumasRevenge
{
	// Token: 0x02000030 RID: 48
	public class InkCloud
	{
		// Token: 0x060005A6 RID: 1446 RVA: 0x0001D8AC File Offset: 0x0001BAAC
		public void SyncState(DataSync s)
		{
			s.SyncBoolean(ref this.mFadeIn);
			s.SyncFloat(ref this.mAlpha);
			s.SyncFloat(ref this.mSize);
			s.SyncFloat(ref this.mX);
			s.SyncFloat(ref this.mY);
		}

		// Token: 0x04000285 RID: 645
		public bool mFadeIn;

		// Token: 0x04000286 RID: 646
		public float mAlpha;

		// Token: 0x04000287 RID: 647
		public float mSize;

		// Token: 0x04000288 RID: 648
		public float mX;

		// Token: 0x04000289 RID: 649
		public float mY;
	}
}
