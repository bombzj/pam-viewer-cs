using System;

namespace ZumasRevenge.Sound
{
	// Token: 0x0200013E RID: 318
	internal class StaggeredSound : UpdatedSound
	{
		// Token: 0x06000FCF RID: 4047 RVA: 0x000A04D5 File Offset: 0x0009E6D5
		public StaggeredSound(Sound inSound, int inStaggerTime)
		{
			this.mSound = inSound;
			this.mStaggerTime = inStaggerTime;
			this.mStaggerCount = inStaggerTime;
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x000A04F2 File Offset: 0x0009E6F2
		public override void Dispose()
		{
			this.mSound.Dispose();
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x000A04FF File Offset: 0x0009E6FF
		public override void Play()
		{
			if (this.mStaggerCount < this.mStaggerTime)
			{
				return;
			}
			this.mStaggerCount = 0;
			this.mSound.Play();
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x000A0522 File Offset: 0x0009E722
		public override void Fade()
		{
			this.mSound.Fade();
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x000A052F File Offset: 0x0009E72F
		public override void Update()
		{
			this.mStaggerCount++;
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x000A053F File Offset: 0x0009E73F
		public override float GetOptionVolume()
		{
			return 0f;
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x000A0546 File Offset: 0x0009E746
		public override void Pause(bool inPauseOn)
		{
			this.mSound.Pause(inPauseOn);
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x000A0554 File Offset: 0x0009E754
		public override bool IsFree()
		{
			return this.mStaggerCount > this.mStaggerTime * 1000 && this.mSound.IsFree();
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x000A0577 File Offset: 0x0009E777
		public override bool IsFading()
		{
			return this.mSound.IsFading();
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x000A0584 File Offset: 0x0009E784
		public override bool IsLooping()
		{
			return this.mSound.IsLooping();
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x000A0591 File Offset: 0x0009E791
		public override float GetVolume()
		{
			return this.mSound.GetVolume();
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x000A059E File Offset: 0x0009E79E
		public override void SetPan(int inPan)
		{
			this.mSound.SetPan(inPan);
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x000A05AC File Offset: 0x0009E7AC
		public override void SetPitch(float inPitch)
		{
			this.mSound.SetPitch(inPitch);
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x000A05BA File Offset: 0x0009E7BA
		public override void SetVolume(float inVolume)
		{
			this.mSound.SetVolume(inVolume);
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x000A05C8 File Offset: 0x0009E7C8
		public override void EnableAutoUnload()
		{
			this.mSound.EnableAutoUnload();
		}

		// Token: 0x040016D7 RID: 5847
		private int mStaggerTime;

		// Token: 0x040016D8 RID: 5848
		private int mStaggerCount;
	}
}
