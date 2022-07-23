using System;
using SexyFramework.Sound;

namespace ZumasRevenge.Sound
{
	// Token: 0x0200013B RID: 315
	internal class LoopingSound : BasicSound
	{
		// Token: 0x06000F9F RID: 3999 RVA: 0x0009FFD8 File Offset: 0x0009E1D8
		public LoopingSound(int inSoundID, SoundManager inSoundManager)
		{
			this.m_SoundID = inSoundID;
			this.m_SoundManager = inSoundManager;
			this.mVolume = (float)this.m_SoundManager.GetMasterVolume();
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x000A000B File Offset: 0x0009E20B
		public override void Dispose()
		{
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x000A0010 File Offset: 0x0009E210
		public override void Play()
		{
			if (this.mSoundInstance != null || !this.FindFreeSoundInstance(ref this.mSoundInstance))
			{
				return;
			}
			this.mVolume = (float)this.m_SoundManager.GetMasterVolume();
			this.mSoundInstance.SetVolume((double)this.GetVolume());
			this.mSoundInstance.Play(true, false);
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x000A0068 File Offset: 0x0009E268
		protected override bool FindFreeSoundInstance(ref SoundInstance outInstance)
		{
			SoundInstance soundInstance = this.m_SoundManager.GetSoundInstance(this.m_SoundID);
			if (soundInstance != null)
			{
				outInstance = soundInstance;
			}
			return soundInstance != null;
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x000A0094 File Offset: 0x0009E294
		public override void Fade()
		{
			if (this.mSoundInstance != null)
			{
				this.mSoundInstance.Stop();
			}
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x000A00A9 File Offset: 0x0009E2A9
		public override void Update()
		{
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x000A00AB File Offset: 0x0009E2AB
		public override float GetOptionVolume()
		{
			return (float)this.m_SoundManager.GetMasterVolume();
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x000A00B9 File Offset: 0x0009E2B9
		public override void Pause(bool inPauseOn)
		{
			this.mPaused = inPauseOn;
			if (this.mSoundInstance == null)
			{
				return;
			}
			if (this.mPaused)
			{
				this.mSoundInstance.Pause();
				return;
			}
			this.mSoundInstance.Resume();
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x000A00EA File Offset: 0x0009E2EA
		public override bool IsFree()
		{
			return this.mSoundInstance == null;
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x000A00F5 File Offset: 0x0009E2F5
		public override bool IsFading()
		{
			return false;
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x000A00F8 File Offset: 0x0009E2F8
		public override bool IsLooping()
		{
			return this.mSoundInstance != null;
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x000A0106 File Offset: 0x0009E306
		public override float GetVolume()
		{
			if (!this.mPaused)
			{
				return this.mVolume;
			}
			return 0f;
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x000A011C File Offset: 0x0009E31C
		public override void SetPan(int inPan)
		{
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x000A011E File Offset: 0x0009E31E
		public override void SetPitch(float inPitch)
		{
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x000A0120 File Offset: 0x0009E320
		public override void SetVolume(float inVolume)
		{
			this.mVolume = inVolume;
			if (this.mSoundInstance != null)
			{
				this.mSoundInstance.SetVolume((double)inVolume);
			}
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x000A013E File Offset: 0x0009E33E
		public override void EnableAutoUnload()
		{
			this.mUnloadSource = true;
		}

		// Token: 0x040016C8 RID: 5832
		private SoundInstance mSoundInstance;

		// Token: 0x040016C9 RID: 5833
		public bool mPaused;

		// Token: 0x040016CA RID: 5834
		private bool mUnloadSource;

		// Token: 0x040016CB RID: 5835
		private float mVolume = 1f;
	}
}
