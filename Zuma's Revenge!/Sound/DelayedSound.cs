using System;

namespace ZumasRevenge.Sound
{
	// Token: 0x0200013C RID: 316
	internal class DelayedSound : UpdatedSound
	{
		// Token: 0x06000FAF RID: 4015 RVA: 0x000A0147 File Offset: 0x0009E347
		public DelayedSound(Sound inSound, int inDelay)
		{
			this.mSound = inSound;
			this.mDelay = inDelay;
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x000A0164 File Offset: 0x0009E364
		public override void Dispose()
		{
			this.mSound.Dispose();
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x000A0171 File Offset: 0x0009E371
		public override void Play()
		{
			if (this.mUpdateCount > 0)
			{
				return;
			}
			this.mIsFree = false;
			this.mDoCountdown = true;
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x000A018B File Offset: 0x0009E38B
		public override void Fade()
		{
			this.mSound.Fade();
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x000A0198 File Offset: 0x0009E398
		public override void Update()
		{
			if (this.mDoCountdown)
			{
				this.mUpdateCount++;
			}
			if (this.mUpdateCount == this.mDelay)
			{
				this.mSound.Play();
				return;
			}
			if (this.mUpdateCount > this.mDelay)
			{
				this.mIsFree = true;
				this.mDoCountdown = false;
				this.mSound.Update();
			}
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x000A01FC File Offset: 0x0009E3FC
		public override void Pause(bool inPauseOn)
		{
			if (this.mUpdateCount <= this.mDelay)
			{
				this.mDoCountdown = !inPauseOn;
			}
			this.mSound.Pause(inPauseOn);
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x000A0222 File Offset: 0x0009E422
		public override float GetOptionVolume()
		{
			return 0f;
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x000A0229 File Offset: 0x0009E429
		public override bool IsFree()
		{
			return this.mIsFree && this.mSound.IsFree();
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x000A0240 File Offset: 0x0009E440
		public override bool IsFading()
		{
			return this.mSound.IsFading();
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x000A024D File Offset: 0x0009E44D
		public override bool IsLooping()
		{
			return this.mSound.IsLooping();
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x000A025A File Offset: 0x0009E45A
		public override float GetVolume()
		{
			return this.mSound.GetVolume();
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x000A0267 File Offset: 0x0009E467
		public override void SetPan(int inPan)
		{
			this.mSound.SetPan(inPan);
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x000A0275 File Offset: 0x0009E475
		public override void SetPitch(float inPitch)
		{
			this.mSound.SetPitch(inPitch);
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x000A0283 File Offset: 0x0009E483
		public override void SetVolume(float inVolume)
		{
			this.mSound.SetVolume(inVolume);
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x000A0291 File Offset: 0x0009E491
		public override void EnableAutoUnload()
		{
			this.mSound.EnableAutoUnload();
		}

		// Token: 0x040016CC RID: 5836
		private bool mIsFree = true;

		// Token: 0x040016CD RID: 5837
		private bool mDoCountdown;

		// Token: 0x040016CE RID: 5838
		private int mDelay;

		// Token: 0x040016CF RID: 5839
		private int mUpdateCount;
	}
}
