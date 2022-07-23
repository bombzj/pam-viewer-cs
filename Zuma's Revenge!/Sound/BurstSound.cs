using System;
using SexyFramework.Sound;

namespace ZumasRevenge.Sound
{
	// Token: 0x0200013A RID: 314
	internal class BurstSound : BasicSound
	{
		// Token: 0x06000F8D RID: 3981 RVA: 0x0009FE51 File Offset: 0x0009E051
		public BurstSound(int inSoundID, SoundManager inSoundManager, bool inAutoRelease)
		{
			this.m_SoundID = inSoundID;
			this.m_SoundManager = inSoundManager;
			this.mAutoRelease = inAutoRelease;
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0009FE79 File Offset: 0x0009E079
		public override void Dispose()
		{
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0009FE7B File Offset: 0x0009E07B
		public override void Play()
		{
			if (this.mPaused || !this.ReleaseInstance() || !this.FindFreeSoundInstance(ref this.mSoundInstance))
			{
				return;
			}
			this.SetAttributes(this.mSoundInstance);
			this.mSoundInstance.Play(false, this.mAutoRelease);
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x0009FEBC File Offset: 0x0009E0BC
		protected override bool FindFreeSoundInstance(ref SoundInstance outInstance)
		{
			SoundInstance soundInstance = this.m_SoundManager.GetSoundInstance(this.m_SoundID);
			if (soundInstance != null)
			{
				outInstance = soundInstance;
			}
			return soundInstance != null;
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0009FEE8 File Offset: 0x0009E0E8
		private void SetAttributes(SoundInstance inInstance)
		{
			if (this.mPan != 0)
			{
				inInstance.SetPan(this.mPan);
			}
			inInstance.AdjustPitch((double)this.mPitch);
			inInstance.SetVolume(this.m_SoundManager.GetMasterVolume());
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0009FF1C File Offset: 0x0009E11C
		private bool ReleaseInstance()
		{
			if (this.mSoundInstance != null && !this.mAutoRelease)
			{
				if (this.mSoundInstance.IsPlaying())
				{
					return false;
				}
				this.mSoundInstance.Release();
				this.mSoundInstance = null;
			}
			return true;
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0009FF50 File Offset: 0x0009E150
		public override void Fade()
		{
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0009FF52 File Offset: 0x0009E152
		public override void Update()
		{
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0009FF54 File Offset: 0x0009E154
		public override float GetOptionVolume()
		{
			return 0f;
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0009FF5B File Offset: 0x0009E15B
		public override void Pause(bool inPauseOn)
		{
			this.mPaused = inPauseOn;
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0009FF64 File Offset: 0x0009E164
		public override bool IsFree()
		{
			return this.mAutoRelease || this.mSoundInstance == null || !this.mSoundInstance.IsPlaying();
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0009FF86 File Offset: 0x0009E186
		public override bool IsFading()
		{
			return false;
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0009FF89 File Offset: 0x0009E189
		public override bool IsLooping()
		{
			return false;
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0009FF8C File Offset: 0x0009E18C
		public override float GetVolume()
		{
			if (!this.mPaused)
			{
				return this.mVolume;
			}
			return 0f;
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0009FFA2 File Offset: 0x0009E1A2
		public override void SetPan(int inPan)
		{
			this.mPan = inPan;
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x0009FFAB File Offset: 0x0009E1AB
		public override void SetPitch(float inPitch)
		{
			this.mPitch = inPitch;
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x0009FFB4 File Offset: 0x0009E1B4
		public override void SetVolume(float inVolume)
		{
			this.mVolume = inVolume;
			this.m_SoundManager.SetVolume((double)this.mVolume);
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0009FFCF File Offset: 0x0009E1CF
		public override void EnableAutoUnload()
		{
			this.mUnloadSource = true;
		}

		// Token: 0x040016C1 RID: 5825
		private SoundInstance mSoundInstance;

		// Token: 0x040016C2 RID: 5826
		private bool mAutoRelease;

		// Token: 0x040016C3 RID: 5827
		private bool mPaused;

		// Token: 0x040016C4 RID: 5828
		private bool mUnloadSource;

		// Token: 0x040016C5 RID: 5829
		private int mPan;

		// Token: 0x040016C6 RID: 5830
		private float mPitch;

		// Token: 0x040016C7 RID: 5831
		private float mVolume = 1f;
	}
}
