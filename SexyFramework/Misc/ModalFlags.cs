using System;

namespace SexyFramework.Misc
{
	// Token: 0x0200012F RID: 303
	public class ModalFlags
	{
		// Token: 0x06000A17 RID: 2583 RVA: 0x000348FD File Offset: 0x00032AFD
		public void ModFlags(FlagsMod theFlagsMod)
		{
			FlagsMod.ModFlags(ref this.mOverFlags, theFlagsMod);
			FlagsMod.ModFlags(ref this.mOverFlags, theFlagsMod);
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x00034917 File Offset: 0x00032B17
		public int GetFlags()
		{
			if (!this.mIsOver)
			{
				return this.mUnderFlags;
			}
			return this.mOverFlags;
		}

		// Token: 0x04000896 RID: 2198
		public int mOverFlags;

		// Token: 0x04000897 RID: 2199
		public int mUnderFlags;

		// Token: 0x04000898 RID: 2200
		public bool mIsOver;
	}
}
