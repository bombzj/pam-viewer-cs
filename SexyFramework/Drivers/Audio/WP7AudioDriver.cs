using System;
using SexyFramework.Drivers.App;
using SexyFramework.Sound;

namespace SexyFramework.Drivers.Audio
{
	// Token: 0x02000015 RID: 21
	public class WP7AudioDriver : IAudioDriver
	{
		// Token: 0x06000117 RID: 279 RVA: 0x000052EE File Offset: 0x000034EE
		public static WP7AudioDriver CreateAudioDriver(SexyAppBase app)
		{
			return new WP7AudioDriver(app);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000052F6 File Offset: 0x000034F6
		public WP7AudioDriver(SexyAppBase app)
		{
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000052FE File Offset: 0x000034FE
		public override void Dispose()
		{
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005300 File Offset: 0x00003500
		public override bool InitAudioDriver()
		{
			return true;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005303 File Offset: 0x00003503
		public override SoundManager CreateSoundManager()
		{
			return new XSoundManager(WP7AppDriver.sWP7AppDriverInstance.mContentManager);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005314 File Offset: 0x00003514
		public override MusicInterface CreateMusicInterface()
		{
			return null;
		}
	}
}
