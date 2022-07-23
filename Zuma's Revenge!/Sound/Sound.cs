using System;

namespace ZumasRevenge.Sound
{
	// Token: 0x02000137 RID: 311
	public abstract class Sound : IDisposable
	{
		// Token: 0x06000F7B RID: 3963
		public abstract void Dispose();

		// Token: 0x06000F7C RID: 3964
		public abstract void Play();

		// Token: 0x06000F7D RID: 3965
		public abstract void Fade();

		// Token: 0x06000F7E RID: 3966
		public abstract void Update();

		// Token: 0x06000F7F RID: 3967
		public abstract void Pause(bool inPauseOn);

		// Token: 0x06000F80 RID: 3968
		public abstract bool IsFree();

		// Token: 0x06000F81 RID: 3969
		public abstract bool IsFading();

		// Token: 0x06000F82 RID: 3970
		public abstract bool IsLooping();

		// Token: 0x06000F83 RID: 3971
		public abstract float GetVolume();

		// Token: 0x06000F84 RID: 3972
		public abstract float GetOptionVolume();

		// Token: 0x06000F85 RID: 3973
		public abstract void SetPan(int inPan);

		// Token: 0x06000F86 RID: 3974
		public abstract void SetPitch(float inPitch);

		// Token: 0x06000F87 RID: 3975
		public abstract void SetVolume(float inVolume);

		// Token: 0x06000F88 RID: 3976
		public abstract void EnableAutoUnload();
	}
}
