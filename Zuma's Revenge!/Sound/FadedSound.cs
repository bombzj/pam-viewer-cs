using System;

namespace ZumasRevenge.Sound
{
	// Token: 0x0200013D RID: 317
	internal class FadedSound : UpdatedSound
	{
		// Token: 0x06000FBE RID: 4030 RVA: 0x000A029E File Offset: 0x0009E49E
		public FadedSound(Sound inSound, float inFadeInSpeed, float inFadeOutSpeed)
		{
			this.mSound = inSound;
			this.mFadeInSpeed = inFadeInSpeed;
			this.mFadeOutSpeed = inFadeOutSpeed;
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x000A02CD File Offset: 0x0009E4CD
		public override void Dispose()
		{
			this.mSound.Dispose();
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x000A02DC File Offset: 0x0009E4DC
		public override void Play()
		{
			if (!this.mIsFree)
			{
				return;
			}
			this.mIsFree = false;
			this.mTargetVolume = this.mSound.GetVolume();
			this.mSound.SetVolume(0f);
			this.mSound.Play();
			this.mIsFadeOut = false;
			this.mIsPaused = false;
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x000A0333 File Offset: 0x0009E533
		public override void Fade()
		{
			this.mTargetVolume = 0f;
			this.mIsFadeOut = true;
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x000A0348 File Offset: 0x0009E548
		public override void Update()
		{
			if (this.mIsPaused)
			{
				return;
			}
			float num = this.mSound.GetVolume();
			if (num == this.mTargetVolume && !this.mIsFadeOut)
			{
				return;
			}
			if (this.mTargetVolume == 0f || this.mIsFadeOut)
			{
				num -= this.mFadeOutSpeed;
				if (num < 0f)
				{
					num = 0f;
					this.mIsFadeOut = false;
					this.mSound.Fade();
				}
			}
			else
			{
				num += this.mFadeInSpeed;
				if (num > this.mTargetVolume)
				{
					num = this.mTargetVolume;
				}
			}
			if (num == 0f)
			{
				this.mIsFree = true;
				return;
			}
			this.mSound.SetVolume(num);
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x000A03F1 File Offset: 0x0009E5F1
		public override void Pause(bool inPauseOn)
		{
			if (!inPauseOn)
			{
				this.RestoreTargetVolume();
			}
			this.mIsPaused = inPauseOn;
			this.mSound.Pause(inPauseOn);
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x000A040F File Offset: 0x0009E60F
		public override bool IsFree()
		{
			return this.mIsFree;
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x000A0417 File Offset: 0x0009E617
		public override bool IsFading()
		{
			return this.mTargetVolume == 0f && this.mSound.GetVolume() > 0f;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x000A043A File Offset: 0x0009E63A
		public override bool IsLooping()
		{
			return this.mSound.IsLooping();
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x000A0447 File Offset: 0x0009E647
		public override float GetVolume()
		{
			return this.mSound.GetVolume();
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x000A0454 File Offset: 0x0009E654
		public override void SetPan(int inPan)
		{
			this.mSound.SetPan(inPan);
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x000A0462 File Offset: 0x0009E662
		public override void SetPitch(float inPitch)
		{
			this.mSound.SetPitch(inPitch);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x000A0470 File Offset: 0x0009E670
		public override void SetVolume(float inVolume)
		{
			this.mSound.SetVolume(inVolume);
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x000A047E File Offset: 0x0009E67E
		public override void EnableAutoUnload()
		{
			this.mSound.EnableAutoUnload();
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x000A048B File Offset: 0x0009E68B
		public override float GetOptionVolume()
		{
			return 0f;
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x000A0492 File Offset: 0x0009E692
		protected void CacheTargetVolume()
		{
			if (this.mTargetVolume == 0f)
			{
				return;
			}
			this.mLastTarget = this.mTargetVolume;
			this.mTargetVolume = 0f;
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x000A04B9 File Offset: 0x0009E6B9
		protected void RestoreTargetVolume()
		{
			if (this.mIsFadeOut)
			{
				return;
			}
			this.mTargetVolume = this.mSound.GetOptionVolume();
		}

		// Token: 0x040016D0 RID: 5840
		private bool mIsFree = true;

		// Token: 0x040016D1 RID: 5841
		private float mFadeInSpeed;

		// Token: 0x040016D2 RID: 5842
		private float mFadeOutSpeed;

		// Token: 0x040016D3 RID: 5843
		private float mTargetVolume;

		// Token: 0x040016D4 RID: 5844
		private float mLastTarget = -1f;

		// Token: 0x040016D5 RID: 5845
		private bool mIsPaused;

		// Token: 0x040016D6 RID: 5846
		private bool mIsFadeOut;
	}
}
