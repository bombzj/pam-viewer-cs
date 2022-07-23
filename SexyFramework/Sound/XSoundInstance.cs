using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace SexyFramework.Sound
{
	// Token: 0x020001B2 RID: 434
	public class XSoundInstance : SoundInstance
	{
		// Token: 0x06000FF2 RID: 4082 RVA: 0x0004CA94 File Offset: 0x0004AC94
		public static XSoundInstance GetNewXSoundInstance(int id, SoundEffectInstance instance)
		{
			if (XSoundInstance.unusedObjects.Count > 0)
			{
				XSoundInstance xsoundInstance = XSoundInstance.unusedObjects.Pop();
				xsoundInstance.Reset(id, instance);
				return xsoundInstance;
			}
			return new XSoundInstance(id, instance);
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0004CACA File Offset: 0x0004ACCA
		public XSoundInstance(int id, SoundEffectInstance instance)
		{
			this.Reset(id, instance);
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0004CADC File Offset: 0x0004ACDC
		public void Reset(int id, SoundEffectInstance instance)
		{
			this.m_SoundInstance = instance;
			this.m_SoundID = id;
			this.didPlay = false;
			this.mBaseVolume = 1f;
			this.mVolume = 1f;
			this.mBasePan = 0f;
			this.mPan = 0f;
			this.mPitch = 0f;
			this.mIsReleased = false;
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0004CB3C File Offset: 0x0004AD3C
		public override void Release()
		{
			if (this.m_SoundInstance != null)
			{
				this.m_SoundInstance.Stop();
				this.m_SoundInstance.Dispose();
				this.m_SoundInstance = null;
			}
			this.mIsReleased = true;
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0004CB6A File Offset: 0x0004AD6A
		public override void SetBaseVolume(double theBaseVolume)
		{
			this.mBaseVolume = (float)theBaseVolume;
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0004CB74 File Offset: 0x0004AD74
		public override void SetBasePan(int theBasePan)
		{
			this.mBasePan = (float)theBasePan / 100f;
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0004CB84 File Offset: 0x0004AD84
		public override void SetBaseRate(double theBaseRate)
		{
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0004CB86 File Offset: 0x0004AD86
		public override void AdjustPitch(double theNumSteps)
		{
			this.mPitch = (float)theNumSteps;
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0004CB90 File Offset: 0x0004AD90
		public override void SetVolume(double theVolume)
		{
			this.mVolume = (float)theVolume;
			if (this.m_SoundInstance != null)
			{
				this.m_SoundInstance.Volume = (float)theVolume;
			}
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0004CBAF File Offset: 0x0004ADAF
		public override void SetMasterVolumeIdx(int theVolumeIdx)
		{
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0004CBB1 File Offset: 0x0004ADB1
		public override void SetPan(int thePosition)
		{
			this.mPan = (float)thePosition / 10000f;
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0004CBC1 File Offset: 0x0004ADC1
		public override bool Play(bool looping, bool autoRelease)
		{
			this.Stop();
			this.didPlay = true;
			if (this.m_SoundInstance.State == SoundState.Stopped)
			{
				this.m_SoundInstance.IsLooped = looping;
			}
			this.m_SoundInstance.Play();
			return true;
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0004CBF6 File Offset: 0x0004ADF6
		public override void Stop()
		{
			if (this.m_SoundInstance != null)
			{
				this.m_SoundInstance.Stop();
			}
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0004CC0B File Offset: 0x0004AE0B
		public override void Pause()
		{
			if (this.m_SoundInstance != null)
			{
				this.m_SoundInstance.Pause();
			}
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0004CC20 File Offset: 0x0004AE20
		public override void Resume()
		{
			if (this.m_SoundInstance != null)
			{
				this.m_SoundInstance.Resume();
			}
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0004CC35 File Offset: 0x0004AE35
		public override bool IsPlaying()
		{
			return this.m_SoundInstance != null && this.m_SoundInstance.State == 0;
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0004CC4F File Offset: 0x0004AE4F
		public override bool IsReleased()
		{
			return this.mIsReleased;
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0004CC57 File Offset: 0x0004AE57
		public override double GetVolume()
		{
			return (double)this.mVolume;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0004CC60 File Offset: 0x0004AE60
		public override bool IsDormant()
		{
			return this.didPlay && this.m_SoundInstance.State == SoundState.Stopped;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0004CC7A File Offset: 0x0004AE7A
		public void PrepareForReuse()
		{
			XSoundInstance.unusedObjects.Push(this);
		}

		// Token: 0x04000CD3 RID: 3283
		private SoundEffectInstance m_SoundInstance;

		// Token: 0x04000CD4 RID: 3284
		private static Stack<XSoundInstance> unusedObjects = new Stack<XSoundInstance>(20);

		// Token: 0x04000CD5 RID: 3285
		public int m_SoundID;

		// Token: 0x04000CD6 RID: 3286
		public float mBaseVolume;

		// Token: 0x04000CD7 RID: 3287
		public float mBasePan;

		// Token: 0x04000CD8 RID: 3288
		private float mVolume;

		// Token: 0x04000CD9 RID: 3289
		private float mPan;

		// Token: 0x04000CDA RID: 3290
		private float mPitch;

		// Token: 0x04000CDB RID: 3291
		private bool didPlay;

		// Token: 0x04000CDC RID: 3292
		private bool mIsReleased;
	}
}
