using System;
using Microsoft.Xna.Framework.Media;

namespace SexyFramework
{
	// Token: 0x020001AE RID: 430
	public class SoundEffectWrapper
	{
		// Token: 0x06000FA3 RID: 4003 RVA: 0x0004BF36 File Offset: 0x0004A136
		public SoundEffectWrapper(Song s)
		{
			this.load(s);
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0004BF54 File Offset: 0x0004A154
		public void load(Song s)
		{
			this.m_Song = s;
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0004BF5D File Offset: 0x0004A15D
		public bool isPlaying()
		{
			return this.m_isPlaying;
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x0004BF65 File Offset: 0x0004A165
		public void play()
		{
			MediaPlayer.Play(this.m_Song);
			this.m_isPlaying = true;
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0004BF79 File Offset: 0x0004A179
		public void stop()
		{
			MediaPlayer.Stop();
			this.m_isPlaying = false;
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0004BF87 File Offset: 0x0004A187
		public void setLoop(bool isLooped)
		{
			MediaPlayer.IsRepeating = isLooped;
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0004BF8F File Offset: 0x0004A18F
		public void setVolume(float volume)
		{
			MediaPlayer.Volume = Common.CaculatePowValume(volume);
		}

		// Token: 0x04000CC1 RID: 3265
		public Song m_Song;

		// Token: 0x04000CC2 RID: 3266
		public double mVolume;

		// Token: 0x04000CC3 RID: 3267
		public double mVolumeAdd;

		// Token: 0x04000CC4 RID: 3268
		public double mVolumeCap = 1.0;

		// Token: 0x04000CC5 RID: 3269
		public bool mStopOnFade;

		// Token: 0x04000CC6 RID: 3270
		public bool m_isPlaying;
	}
}
