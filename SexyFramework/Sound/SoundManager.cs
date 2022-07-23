using System;

namespace SexyFramework.Sound
{
	// Token: 0x020001B3 RID: 435
	public abstract class SoundManager
	{
		// Token: 0x06001007 RID: 4103
		public abstract bool Initialized();

		// Token: 0x06001008 RID: 4104
		public abstract bool LoadSound(uint theSfxID, string theFilename);

		// Token: 0x06001009 RID: 4105
		public abstract int LoadSound(string theFilename);

		// Token: 0x0600100A RID: 4106 RVA: 0x0004CC95 File Offset: 0x0004AE95
		public virtual void Dispose()
		{
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0004CC97 File Offset: 0x0004AE97
		public virtual void ReleaseSound(int theSfxID)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0004CC9E File Offset: 0x0004AE9E
		public virtual double GetVolume(int theVolIdx)
		{
			return 0.0;
		}

		// Token: 0x0600100D RID: 4109
		public abstract void SetVolume(double theVolume);

		// Token: 0x0600100E RID: 4110
		public abstract void SetBaseVolume(uint mSoundId, double mVolume);

		// Token: 0x0600100F RID: 4111
		public abstract void SetBasePan(uint theSfxID, int theBasePan);

		// Token: 0x06001010 RID: 4112
		public abstract SoundInstance GetSoundInstance(int theSfxID);

		// Token: 0x06001011 RID: 4113
		public abstract SoundInstance GetExistSoundInstance(int theSfxID);

		// Token: 0x06001012 RID: 4114
		public abstract void ReleaseSounds();

		// Token: 0x06001013 RID: 4115
		public abstract void ReleaseChannels();

		// Token: 0x06001014 RID: 4116
		public abstract double GetMasterVolume();

		// Token: 0x06001015 RID: 4117
		public abstract void SetMasterVolume(double theVolume);

		// Token: 0x06001016 RID: 4118
		public abstract void Flush();

		// Token: 0x06001017 RID: 4119
		public abstract void StopAllSounds();

		// Token: 0x06001018 RID: 4120
		public abstract int GetFreeSoundId();

		// Token: 0x06001019 RID: 4121
		public abstract int GetNumSounds();

		// Token: 0x0600101A RID: 4122
		public abstract void Update();

		// Token: 0x04000CDD RID: 3293
		public float m_MasterVolume = 1f;
	}
}
