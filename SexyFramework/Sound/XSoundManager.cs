using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace SexyFramework.Sound
{
	// Token: 0x020001B5 RID: 437
	public class XSoundManager : SoundManager
	{
		// Token: 0x0600101E RID: 4126 RVA: 0x0004CCD4 File Offset: 0x0004AED4
		public XSoundManager(ContentManager cmgr)
		{
			this.mContent = cmgr;
			this.mInstances = new List<XSoundInstance>();
			for (int i = 0; i < XSoundManager.MAX_SOURCE_SOUNDS; i++)
			{
				this.m_sounds[i] = null;
			}
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0004CD24 File Offset: 0x0004AF24
		public override int GetFreeSoundId()
		{
			for (int i = 0; i < XSoundManager.MAX_SOURCE_SOUNDS; i++)
			{
				if (this.m_sounds[i] == null)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0004CD4E File Offset: 0x0004AF4E
		public override void ReleaseSound(int theSfxID)
		{
			if (this.m_sounds[theSfxID] != null)
			{
				this.m_sounds[theSfxID] = null;
			}
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0004CD64 File Offset: 0x0004AF64
		public override bool LoadSound(uint theSfxID, string theFilename)
		{
			SoundEffect soundEffect = this.mContent.Load<SoundEffect>(theFilename);
			XSoundEntry xsoundEntry = new XSoundEntry();
			xsoundEntry.m_SoundEffect = soundEffect;
			this.m_sounds[(int)((UIntPtr)theSfxID)] = xsoundEntry;
			return true;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0004CD98 File Offset: 0x0004AF98
		public override int LoadSound(string theFilename)
		{
			int i = 0;
			while (i < XSoundManager.MAX_SOURCE_SOUNDS)
			{
				if (this.mInstances[i] == null)
				{
					if (this.LoadSound((uint)i, theFilename))
					{
						return i;
					}
					return -1;
				}
				else
				{
					i++;
				}
			}
			return -1;
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0004CDD4 File Offset: 0x0004AFD4
		public override void Update()
		{
			for (int i = this.mInstances.Count - 1; i >= 0; i--)
			{
				if (this.mInstances[i].IsReleased())
				{
					this.mInstances[i].PrepareForReuse();
					this.mInstances.RemoveAt(i);
				}
				else if (this.mInstances[i].IsDormant())
				{
					if (!this.mInstances[i].IsReleased())
					{
						this.mInstances[i].Release();
					}
					this.mInstances[i].PrepareForReuse();
					this.mInstances.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0004CE85 File Offset: 0x0004B085
		public override bool Initialized()
		{
			return true;
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0004CE88 File Offset: 0x0004B088
		public override void SetVolume(double theVolume)
		{
			this.SetMasterVolume(theVolume);
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0004CE94 File Offset: 0x0004B094
		public override void SetBasePan(uint theSfxID, int theBasePan)
		{
			if (theBasePan < -100 || theBasePan > 100)
			{
				return;
			}
			this.m_sounds[(int)((UIntPtr)theSfxID)].m_BasePan = (float)theBasePan / 100f;
			for (int i = 0; i < this.mInstances.Count; i++)
			{
				if (this.mInstances[i].m_SoundID == (int)theSfxID)
				{
					this.mInstances[i].SetBasePan(theBasePan);
				}
			}
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0004CF00 File Offset: 0x0004B100
		public override void SetBaseVolume(uint theSfxID, double theBaseVolume)
		{
			if (theBaseVolume < 0.0 || theBaseVolume > 1.0)
			{
				return;
			}
			this.m_sounds[(int)((UIntPtr)theSfxID)].m_BaseVolume = (float)theBaseVolume;
			for (int i = 0; i < this.mInstances.Count; i++)
			{
				if (this.mInstances[i].m_SoundID == (int)theSfxID)
				{
					this.mInstances[i].SetBaseVolume((double)this.m_sounds[(int)((UIntPtr)theSfxID)].m_BaseVolume);
				}
			}
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0004CF80 File Offset: 0x0004B180
		public void ReleaseFreeChannels()
		{
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0004CF84 File Offset: 0x0004B184
		public override SoundInstance GetSoundInstance(int theSfxID)
		{
			if (this.mInstances.Count >= XSoundManager.ACTIVE_SOUNDS_LIMIT)
			{
				return null;
			}
			SoundEffectInstance instance = this.m_sounds[theSfxID].m_SoundEffect.CreateInstance();
			XSoundInstance newXSoundInstance = XSoundInstance.GetNewXSoundInstance(theSfxID, instance);
			this.mInstances.Add(newXSoundInstance);
			return newXSoundInstance;
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x0004CFD0 File Offset: 0x0004B1D0
		public override SoundInstance GetExistSoundInstance(int theSfxID)
		{
			if (theSfxID > XSoundManager.MAX_SOURCE_SOUNDS)
			{
				return null;
			}
			if (this.m_sounds[theSfxID] == null)
			{
				return null;
			}
			for (int i = 0; i < this.mInstances.Count; i++)
			{
				if (this.mInstances[i] != null && this.mInstances[i].m_SoundID == theSfxID)
				{
					return this.mInstances[i];
				}
			}
			return null;
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0004D03C File Offset: 0x0004B23C
		public override void ReleaseSounds()
		{
			for (int i = 0; i < XSoundManager.MAX_SOURCE_SOUNDS; i++)
			{
				if (this.m_sounds[i] != null)
				{
					this.m_sounds[i] = null;
				}
			}
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0004D06C File Offset: 0x0004B26C
		public override void ReleaseChannels()
		{
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0004D06E File Offset: 0x0004B26E
		public override double GetMasterVolume()
		{
			return (double)this.m_MasterVolume;
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0004D077 File Offset: 0x0004B277
		public override void SetMasterVolume(double theVolume)
		{
			this.m_MasterVolume = (float)theVolume;
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0004D081 File Offset: 0x0004B281
		public override void Flush()
		{
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x0004D084 File Offset: 0x0004B284
		public override void StopAllSounds()
		{
			for (int i = 0; i < this.mInstances.Count; i++)
			{
				this.mInstances[i].Stop();
			}
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0004D0B8 File Offset: 0x0004B2B8
		public override int GetNumSounds()
		{
			int num = 0;
			for (int i = 0; i < XSoundManager.MAX_SOURCE_SOUNDS; i++)
			{
				if (this.m_sounds[i] != null)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04000CE1 RID: 3297
		public static int MAX_SOURCE_SOUNDS = 4096;

		// Token: 0x04000CE2 RID: 3298
		public static int ACTIVE_SOUNDS_LIMIT = 16;

		// Token: 0x04000CE3 RID: 3299
		private ContentManager mContent;

		// Token: 0x04000CE4 RID: 3300
		private XSoundEntry[] m_sounds = new XSoundEntry[XSoundManager.MAX_SOURCE_SOUNDS];

		// Token: 0x04000CE5 RID: 3301
		private List<XSoundInstance> mInstances;
	}
}
