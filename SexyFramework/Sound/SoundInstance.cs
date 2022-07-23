using System;

namespace SexyFramework.Sound
{
	// Token: 0x020001B1 RID: 433
	public abstract class SoundInstance
	{
		// Token: 0x06000FE1 RID: 4065 RVA: 0x0004CA8B File Offset: 0x0004AC8B
		public SoundInstance()
		{
		}

		// Token: 0x06000FE2 RID: 4066
		public abstract void Release();

		// Token: 0x06000FE3 RID: 4067
		public abstract void SetBaseVolume(double theBaseVolume);

		// Token: 0x06000FE4 RID: 4068
		public abstract void SetBasePan(int theBasePan);

		// Token: 0x06000FE5 RID: 4069
		public abstract void SetBaseRate(double theBaseRate);

		// Token: 0x06000FE6 RID: 4070
		public abstract void AdjustPitch(double theNumSteps);

		// Token: 0x06000FE7 RID: 4071
		public abstract void SetVolume(double theVolume);

		// Token: 0x06000FE8 RID: 4072
		public abstract void SetMasterVolumeIdx(int theVolumeIdx);

		// Token: 0x06000FE9 RID: 4073
		public abstract void SetPan(int thePosition);

		// Token: 0x06000FEA RID: 4074
		public abstract bool Play(bool looping, bool autoRelease);

		// Token: 0x06000FEB RID: 4075
		public abstract void Stop();

		// Token: 0x06000FEC RID: 4076
		public abstract bool IsPlaying();

		// Token: 0x06000FED RID: 4077
		public abstract bool IsReleased();

		// Token: 0x06000FEE RID: 4078
		public abstract double GetVolume();

		// Token: 0x06000FEF RID: 4079
		public abstract bool IsDormant();

		// Token: 0x06000FF0 RID: 4080
		public abstract void Pause();

		// Token: 0x06000FF1 RID: 4081
		public abstract void Resume();
	}
}
