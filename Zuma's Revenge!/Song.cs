using System;

namespace ZumasRevenge
{
	// Token: 0x02000123 RID: 291
	internal struct Song
	{
		// Token: 0x06000F03 RID: 3843 RVA: 0x0009BA0C File Offset: 0x00099C0C
		public Song(int inID, bool inLoop, float inFadeSpeed)
		{
			this.mID = inID;
			this.mLoop = inLoop;
			this.mFadeSpeed = inFadeSpeed;
		}

		// Token: 0x04000EA6 RID: 3750
		public int mID;

		// Token: 0x04000EA7 RID: 3751
		public bool mLoop;

		// Token: 0x04000EA8 RID: 3752
		public float mFadeSpeed;

		// Token: 0x04000EA9 RID: 3753
		public static Song DefaultSong = new Song(-1, false, 1f);
	}
}
