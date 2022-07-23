using System;
using Microsoft.Xna.Framework.Audio;

namespace SexyFramework.Sound
{
	// Token: 0x020001B4 RID: 436
	internal class XSoundEntry
	{
		// Token: 0x0600101C RID: 4124 RVA: 0x0004CCBC File Offset: 0x0004AEBC
		public void Dispose()
		{
			this.m_SoundEffect.Dispose();
		}

		// Token: 0x04000CDE RID: 3294
		public SoundEffect m_SoundEffect;

		// Token: 0x04000CDF RID: 3295
		public float m_BaseVolume;

		// Token: 0x04000CE0 RID: 3296
		public float m_BasePan;
	}
}
