using System;
using SexyFramework.Sound;

namespace ZumasRevenge.Sound
{
	// Token: 0x0200013F RID: 319
	public class SoundFactory
	{
		// Token: 0x06000FDE RID: 4062 RVA: 0x000A05D5 File Offset: 0x0009E7D5
		public static void SetSoundManager(SoundManager inSoundManager)
		{
			SoundFactory.m_SoundManager = inSoundManager;
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x000A05DD File Offset: 0x0009E7DD
		public static Sound GetSound(int inSoundID, int inDelay)
		{
			return SoundFactory.GetSound(inSoundID, inDelay, true);
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x000A05E8 File Offset: 0x0009E7E8
		public static Sound GetSound(int inSoundID, int inDelay, bool inAutoRelease)
		{
			BurstSound burstSound = new BurstSound(inSoundID, SoundFactory.m_SoundManager, inAutoRelease);
			if (inDelay > 0)
			{
				return new DelayedSound(burstSound, inDelay);
			}
			return burstSound;
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x000A060F File Offset: 0x0009E80F
		public static Sound GetStaggeredSound(int inSoundID, int inStaggerTime)
		{
			return new StaggeredSound(new BurstSound(inSoundID, SoundFactory.m_SoundManager, true), inStaggerTime);
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x000A0624 File Offset: 0x0009E824
		public static Sound GetLoopingSound(int inSoundID, int inDelay, float inFadeInSpeed, float inFadeOutSpeed)
		{
			LoopingSound loopingSound = new LoopingSound(inSoundID, SoundFactory.m_SoundManager);
			if (inDelay > 0)
			{
				if (inFadeInSpeed < 1f || inFadeOutSpeed < 1f)
				{
					return new DelayedSound(new FadedSound(loopingSound, inFadeInSpeed, inFadeOutSpeed), inDelay);
				}
				return new DelayedSound(loopingSound, inDelay);
			}
			else
			{
				if (inFadeInSpeed < 1f || inFadeOutSpeed < 1f)
				{
					return new FadedSound(loopingSound, inFadeInSpeed, inFadeOutSpeed);
				}
				return loopingSound;
			}
		}

		// Token: 0x040016D9 RID: 5849
		private static SoundManager m_SoundManager;
	}
}
