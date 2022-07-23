using System;
using SexyFramework.Misc;

namespace SexyFramework.PIL
{
	// Token: 0x0200016D RID: 365
	public class LifetimeSettings
	{
		// Token: 0x06000CF4 RID: 3316 RVA: 0x0003F838 File Offset: 0x0003DA38
		public LifetimeSettings()
		{
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0003F8B0 File Offset: 0x0003DAB0
		public LifetimeSettings(LifetimeSettings rhs)
			: this()
		{
			if (rhs == null)
			{
				return;
			}
			this.mSizeXMult = rhs.mSizeXMult;
			this.mVelocityMult = rhs.mVelocityMult;
			this.mWeightMult = rhs.mWeightMult;
			this.mSpinMult = rhs.mSpinMult;
			this.mMotionRandMult = rhs.mMotionRandMult;
			this.mBounceMult = rhs.mBounceMult;
			this.mZoomMult = rhs.mZoomMult;
			this.mNumberMult = rhs.mNumberMult;
			this.mPct = rhs.mPct;
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0003F934 File Offset: 0x0003DB34
		public void Reset()
		{
			this.mSizeXMult = 1f;
			this.mVelocityMult = 1f;
			this.mWeightMult = 1f;
			this.mSpinMult = 1f;
			this.mMotionRandMult = 1f;
			this.mBounceMult = 1f;
			this.mZoomMult = 1f;
			this.mNumberMult = 1f;
			this.mPct = 0f;
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0003F9A4 File Offset: 0x0003DBA4
		public void Serialize(SexyBuffer b)
		{
			b.WriteFloat(this.mSizeXMult);
			b.WriteFloat(this.mSizeYMult);
			b.WriteFloat(this.mVelocityMult);
			b.WriteFloat(this.mWeightMult);
			b.WriteFloat(this.mSpinMult);
			b.WriteFloat(this.mMotionRandMult);
			b.WriteFloat(this.mBounceMult);
			b.WriteFloat(this.mZoomMult);
			b.WriteFloat(this.mNumberMult);
			b.WriteFloat(this.mPct);
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0003FA2C File Offset: 0x0003DC2C
		public void Deserialize(SexyBuffer b)
		{
			this.mSizeXMult = b.ReadFloat();
			this.mSizeYMult = b.ReadFloat();
			this.mVelocityMult = b.ReadFloat();
			this.mWeightMult = b.ReadFloat();
			this.mSpinMult = b.ReadFloat();
			this.mMotionRandMult = b.ReadFloat();
			this.mBounceMult = b.ReadFloat();
			this.mZoomMult = b.ReadFloat();
			this.mNumberMult = b.ReadFloat();
			this.mPct = b.ReadFloat();
		}

		// Token: 0x040009F8 RID: 2552
		public float mSizeXMult = 1f;

		// Token: 0x040009F9 RID: 2553
		public float mSizeYMult = 1f;

		// Token: 0x040009FA RID: 2554
		public float mVelocityMult = 1f;

		// Token: 0x040009FB RID: 2555
		public float mWeightMult = 1f;

		// Token: 0x040009FC RID: 2556
		public float mSpinMult = 1f;

		// Token: 0x040009FD RID: 2557
		public float mMotionRandMult = 1f;

		// Token: 0x040009FE RID: 2558
		public float mBounceMult = 1f;

		// Token: 0x040009FF RID: 2559
		public float mZoomMult = 1f;

		// Token: 0x04000A00 RID: 2560
		public float mNumberMult = 1f;

		// Token: 0x04000A01 RID: 2561
		public float mPct;
	}
}
