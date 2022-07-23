using System;
using System.Collections.Generic;
using SexyFramework.Sound;

namespace ZumasRevenge.Sound
{
	// Token: 0x02000141 RID: 321
	public class SoundEffects : IDisposable
	{
		// Token: 0x06000FF4 RID: 4084 RVA: 0x000A0757 File Offset: 0x0009E957
		public SoundEffects(SoundManager soundManager)
		{
			this.m_SoundManager = soundManager;
			SoundFactory.SetSoundManager(soundManager);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x000A0797 File Offset: 0x0009E997
		public void Dispose()
		{
			this.StopAll();
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x000A079F File Offset: 0x0009E99F
		public void Play(int inSoundID)
		{
			this.Play(inSoundID, new SoundAttribs());
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x000A07B0 File Offset: 0x0009E9B0
		public void Play(int inSoundID, SoundAttribs inAttribs)
		{
			Sound sound = null;
			if (this.mSounds.ContainsKey(inSoundID))
			{
				sound = this.mSounds[inSoundID];
			}
			else
			{
				this.mSounds.Add(inSoundID, null);
			}
			if (sound == null)
			{
				if (inAttribs.stagger > 0)
				{
					sound = SoundFactory.GetStaggeredSound(inSoundID, inAttribs.stagger);
				}
				else
				{
					sound = SoundFactory.GetSound(inSoundID, inAttribs.delay);
				}
				this.mSounds[inSoundID] = sound;
			}
			sound.Play();
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x000A0824 File Offset: 0x0009EA24
		public void Loop(int inSoundID)
		{
			this.Loop(inSoundID, new SoundAttribs());
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x000A0834 File Offset: 0x0009EA34
		public void Loop(int inSoundID, SoundAttribs inAttribs)
		{
			Sound sound = null;
			if (this.mSounds.ContainsKey(inSoundID))
			{
				sound = this.mSounds[inSoundID];
			}
			else
			{
				this.mSounds.Add(inSoundID, null);
			}
			if (sound == null)
			{
				sound = SoundFactory.GetLoopingSound(inSoundID, inAttribs.delay, inAttribs.fadein, inAttribs.fadeout);
				this.mSounds[inSoundID] = sound;
			}
			sound.Play();
			this.mCurrentLoopSound = inSoundID;
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x000A08A4 File Offset: 0x0009EAA4
		public void Update()
		{
			bool flag = false;
			this.mSoundsToDelete.Clear();
			foreach (KeyValuePair<int, Sound> keyValuePair in this.mSounds)
			{
				Sound value = keyValuePair.Value;
				if (value.IsFree())
				{
					if (keyValuePair.Key == this.mChainedSound1)
					{
						flag = true;
					}
					this.mSoundsToDelete.Add(keyValuePair.Key);
				}
				else
				{
					value.Update();
				}
			}
			foreach (int num in this.mSoundsToDelete)
			{
				this.mSounds.Remove(num);
			}
			if (flag)
			{
				this.PlayNextInChain();
			}
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x000A098C File Offset: 0x0009EB8C
		internal bool IsLooping(int p)
		{
			return true;
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x000A0990 File Offset: 0x0009EB90
		internal void Stop(int inSoundID)
		{
			SoundInstance existSoundInstance = this.m_SoundManager.GetExistSoundInstance(inSoundID);
			if (existSoundInstance != null)
			{
				existSoundInstance.Release();
			}
			this.Stop(inSoundID, false);
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x000A09BC File Offset: 0x0009EBBC
		internal void Stop(int inSoundID, bool inUnload)
		{
			Sound sound = null;
			if (!this.FindSound(inSoundID, ref sound))
			{
				return;
			}
			if (inUnload)
			{
				sound.EnableAutoUnload();
			}
			this.mSounds.Remove(inSoundID);
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x000A09ED File Offset: 0x0009EBED
		internal void StopAll()
		{
			if (this.m_SoundManager != null)
			{
				this.m_SoundManager.StopAllSounds();
			}
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x000A0A04 File Offset: 0x0009EC04
		internal void Fade(int inSoundID, bool inUnload)
		{
			Sound sound = null;
			if (!this.FindSound(inSoundID, ref sound))
			{
				return;
			}
			if (inUnload)
			{
				sound.EnableAutoUnload();
			}
			sound.Fade();
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x000A0A2E File Offset: 0x0009EC2E
		internal void Fade(int inSoundID)
		{
			this.Fade(inSoundID, false);
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x000A0A38 File Offset: 0x0009EC38
		internal void PauseLoopingSounds(bool p)
		{
			Sound sound = null;
			if (!this.FindSound(this.mCurrentLoopSound, ref sound))
			{
				return;
			}
			sound.Pause(p);
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x000A0A5F File Offset: 0x0009EC5F
		internal void PlayChained(int p, int p_2, int aDelay)
		{
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x000A0A61 File Offset: 0x0009EC61
		private bool FindSound(int inSoundID, ref Sound outSound)
		{
			if (this.mSounds.ContainsKey(inSoundID))
			{
				outSound = this.mSounds[inSoundID];
				return true;
			}
			return false;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x000A0A84 File Offset: 0x0009EC84
		private void PlayNextInChain()
		{
			SoundAttribs soundAttribs = new SoundAttribs();
			soundAttribs.delay = this.mChainedWait;
			this.Play(this.mChainedSound2, soundAttribs);
			this.mChainedSound1 = -1;
			this.mChainedSound2 = -1;
			this.mChainedWait = 0;
		}

		// Token: 0x040016E1 RID: 5857
		private SoundManager m_SoundManager;

		// Token: 0x040016E2 RID: 5858
		private Dictionary<int, Sound> mSounds = new Dictionary<int, Sound>();

		// Token: 0x040016E3 RID: 5859
		private List<int> mSoundsToDelete = new List<int>();

		// Token: 0x040016E4 RID: 5860
		private int mChainedSound1 = -1;

		// Token: 0x040016E5 RID: 5861
		private int mChainedSound2 = -1;

		// Token: 0x040016E6 RID: 5862
		private int mChainedWait;

		// Token: 0x040016E7 RID: 5863
		private int mCurrentLoopSound = -1;
	}
}
