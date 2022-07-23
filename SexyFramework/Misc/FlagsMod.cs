using System;

namespace SexyFramework.Misc
{
	// Token: 0x0200012E RID: 302
	public class FlagsMod
	{
		// Token: 0x06000A14 RID: 2580 RVA: 0x000348CE File Offset: 0x00032ACE
		public static int GetModFlags(int theFlags, FlagsMod theFlagMod)
		{
			return (theFlags | theFlagMod.mAddFlags) & ~theFlagMod.mRemoveFlags;
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x000348E0 File Offset: 0x00032AE0
		public static void ModFlags(ref int theFlags, FlagsMod theFlagMod)
		{
			theFlags = (theFlags | theFlagMod.mAddFlags) & ~theFlagMod.mRemoveFlags;
		}

		// Token: 0x04000894 RID: 2196
		public int mRemoveFlags;

		// Token: 0x04000895 RID: 2197
		public int mAddFlags;
	}
}
