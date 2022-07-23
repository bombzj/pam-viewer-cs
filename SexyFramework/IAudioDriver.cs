using System;
using SexyFramework.Sound;

namespace SexyFramework
{
	// Token: 0x02000014 RID: 20
	public abstract class IAudioDriver
	{
		// Token: 0x06000112 RID: 274 RVA: 0x000052E4 File Offset: 0x000034E4
		public virtual void Dispose()
		{
		}

		// Token: 0x06000113 RID: 275
		public abstract bool InitAudioDriver();

		// Token: 0x06000114 RID: 276
		public abstract SoundManager CreateSoundManager();

		// Token: 0x06000115 RID: 277
		public abstract MusicInterface CreateMusicInterface();
	}
}
