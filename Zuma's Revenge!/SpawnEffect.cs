using System;
using SexyFramework.PIL;

namespace ZumasRevenge
{
	// Token: 0x020000AD RID: 173
	public class SpawnEffect
	{
		// Token: 0x06000A5D RID: 2653 RVA: 0x000621D4 File Offset: 0x000603D4
		public SpawnEffect(bool create)
		{
			if (create)
			{
				this.mRings = new SexyFramework.PIL.System(100, 50);
				this.mRings.mParticleScale2D = 0.3f;
				this.mRings.mScale = Common._S(1f);
				this.mRings.mHighWatermark = Common._M(80);
				this.mRings.mLowWatermark = Common._M(60);
				this.mRings.mFPSCallback = new SexyFramework.PIL.System.FPSCallback(SexyFramework.PIL.System.FadeParticlesFPSCallback);
				this.mRings.WaitForEmitters(true);
				this.mSwirl = new SexyFramework.PIL.System(100, 50);
				this.mSwirl.mHighWatermark = Common._M(80);
				this.mSwirl.mLowWatermark = Common._M(60);
				this.mSwirl.mFPSCallback = new SexyFramework.PIL.System.FPSCallback(SexyFramework.PIL.System.FadeParticlesFPSCallback);
				this.mSwirl.mParticleScale2D = 0.3f;
				this.mSwirl.mScale = Common._S(1f);
				this.mSwirl.WaitForEmitters(true);
			}
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x000622E3 File Offset: 0x000604E3
		public SpawnEffect()
			: this(true)
		{
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x000622EC File Offset: 0x000604EC
		public virtual void Dispose()
		{
			if (this.mRings != null)
			{
				this.mRings.Dispose();
				this.mRings = null;
			}
			if (this.mSwirl != null)
			{
				this.mSwirl.Dispose();
				this.mSwirl = null;
			}
		}

		// Token: 0x040008C2 RID: 2242
		public SexyFramework.PIL.System mRings;

		// Token: 0x040008C3 RID: 2243
		public SexyFramework.PIL.System mSwirl;
	}
}
