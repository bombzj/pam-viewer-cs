using System;
using SexyFramework.PIL;

namespace ZumasRevenge
{
	// Token: 0x020000AE RID: 174
	public class TriggeredEffect
	{
		// Token: 0x06000A60 RID: 2656 RVA: 0x00062324 File Offset: 0x00060524
		public TriggeredEffect(bool create)
		{
			if (create)
			{
				this.mRings = new SexyFramework.PIL.System(50, 50);
				this.mRings.mHighWatermark = Common._M(80);
				this.mRings.mLowWatermark = Common._M(60);
				this.mRings.mFPSCallback = new SexyFramework.PIL.System.FPSCallback(SexyFramework.PIL.System.FadeParticlesFPSCallback);
				this.mRings.mScale = Common._S(1f);
				this.mRings.WaitForEmitters(true);
				this.mRainbow = new SexyFramework.PIL.System(50, 50);
				this.mRainbow.mHighWatermark = Common._M(80);
				this.mRainbow.mLowWatermark = Common._M(60);
				this.mRainbow.mFPSCallback = new SexyFramework.PIL.System.FPSCallback(SexyFramework.PIL.System.FadeParticlesFPSCallback);
				this.mRainbow.mScale = Common._S(1f);
				this.mRainbow.WaitForEmitters(true);
				this.mGas = new SexyFramework.PIL.System(50, 50);
				this.mGas.mHighWatermark = Common._M(80);
				this.mGas.mLowWatermark = Common._M(60);
				this.mGas.mFPSCallback = new SexyFramework.PIL.System.FPSCallback(SexyFramework.PIL.System.FadeParticlesFPSCallback);
				this.mGas.mScale = Common._S(1f);
				this.mGas.WaitForEmitters(true);
				this.mFlare = new SexyFramework.PIL.System(50, 50);
				this.mFlare.mHighWatermark = Common._M(80);
				this.mFlare.mLowWatermark = Common._M(60);
				this.mFlare.mFPSCallback = new SexyFramework.PIL.System.FPSCallback(SexyFramework.PIL.System.FadeParticlesFPSCallback);
				this.mFlare.mScale = Common._S(1f);
				this.mFlare.WaitForEmitters(true);
				this.mTrail = new SexyFramework.PIL.System(150, 50);
				this.mTrail.mHighWatermark = Common._M(80);
				this.mTrail.mLowWatermark = Common._M(60);
				this.mTrail.mFPSCallback = new SexyFramework.PIL.System.FPSCallback(SexyFramework.PIL.System.FadeParticlesFPSCallback);
				this.mTrail.mScale = Common._S(1f);
				this.mTrail.WaitForEmitters(true);
			}
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x00062557 File Offset: 0x00060757
		public TriggeredEffect()
			: this(true)
		{
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x00062560 File Offset: 0x00060760
		public virtual void Dispose()
		{
			if (this.mRings != null)
			{
				this.mRings.Dispose();
				this.mRings = null;
			}
			if (this.mRainbow != null)
			{
				this.mRainbow.Dispose();
				this.mRainbow = null;
			}
			if (this.mGas != null)
			{
				this.mGas.Dispose();
				this.mGas = null;
			}
			if (this.mFlare != null)
			{
				this.mFlare.Dispose();
				this.mFlare = null;
			}
			if (this.mTrail != null)
			{
				this.mTrail.Dispose();
				this.mTrail = null;
			}
		}

		// Token: 0x040008C4 RID: 2244
		public SexyFramework.PIL.System mRings;

		// Token: 0x040008C5 RID: 2245
		public SexyFramework.PIL.System mRainbow;

		// Token: 0x040008C6 RID: 2246
		public SexyFramework.PIL.System mGas;

		// Token: 0x040008C7 RID: 2247
		public SexyFramework.PIL.System mFlare;

		// Token: 0x040008C8 RID: 2248
		public SexyFramework.PIL.System mTrail;
	}
}
