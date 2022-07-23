using System;
using SexyFramework.Sound;

namespace ZumasRevenge.Sound
{
	// Token: 0x02000138 RID: 312
	public abstract class BasicSound : Sound
	{
		// Token: 0x06000F8A RID: 3978
		protected abstract bool FindFreeSoundInstance(ref SoundInstance outInstance);

		// Token: 0x040016BE RID: 5822
		protected SoundManager m_SoundManager;

		// Token: 0x040016BF RID: 5823
		protected int m_SoundID = -1;
	}
}
