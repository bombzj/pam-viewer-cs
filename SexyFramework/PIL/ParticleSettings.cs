using System;

namespace SexyFramework.PIL
{
	// Token: 0x02000170 RID: 368
	public class ParticleSettings : KeyFrameData
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000D0C RID: 3340 RVA: 0x000404BC File Offset: 0x0003E6BC
		// (set) Token: 0x06000D0D RID: 3341 RVA: 0x000404C6 File Offset: 0x0003E6C6
		public float mWeight
		{
			get
			{
				return this.mFloatData[0];
			}
			set
			{
				this.mFloatData[0] = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x000404D1 File Offset: 0x0003E6D1
		// (set) Token: 0x06000D0F RID: 3343 RVA: 0x000404DB File Offset: 0x0003E6DB
		public float mSpin
		{
			get
			{
				return this.mFloatData[1];
			}
			set
			{
				this.mFloatData[1] = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x000404E6 File Offset: 0x0003E6E6
		// (set) Token: 0x06000D11 RID: 3345 RVA: 0x000404F0 File Offset: 0x0003E6F0
		public float mMotionRand
		{
			get
			{
				return this.mFloatData[2];
			}
			set
			{
				this.mFloatData[2] = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x000404FB File Offset: 0x0003E6FB
		// (set) Token: 0x06000D13 RID: 3347 RVA: 0x00040505 File Offset: 0x0003E705
		public float mGlobalVisibility
		{
			get
			{
				return this.mFloatData[3];
			}
			set
			{
				this.mFloatData[3] = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x00040510 File Offset: 0x0003E710
		// (set) Token: 0x06000D15 RID: 3349 RVA: 0x0004051A File Offset: 0x0003E71A
		public int mLife
		{
			get
			{
				return this.mIntData[0];
			}
			set
			{
				this.mIntData[0] = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x00040525 File Offset: 0x0003E725
		// (set) Token: 0x06000D17 RID: 3351 RVA: 0x0004052F File Offset: 0x0003E72F
		public int mXSize
		{
			get
			{
				return this.mIntData[1];
			}
			set
			{
				this.mIntData[1] = value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000D18 RID: 3352 RVA: 0x0004053A File Offset: 0x0003E73A
		// (set) Token: 0x06000D19 RID: 3353 RVA: 0x00040544 File Offset: 0x0003E744
		public int mYSize
		{
			get
			{
				return this.mIntData[2];
			}
			set
			{
				this.mIntData[2] = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000D1A RID: 3354 RVA: 0x0004054F File Offset: 0x0003E74F
		// (set) Token: 0x06000D1B RID: 3355 RVA: 0x00040559 File Offset: 0x0003E759
		public int mVelocity
		{
			get
			{
				return this.mIntData[3];
			}
			set
			{
				this.mIntData[3] = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000D1C RID: 3356 RVA: 0x00040564 File Offset: 0x0003E764
		// (set) Token: 0x06000D1D RID: 3357 RVA: 0x0004056E File Offset: 0x0003E76E
		public int mBounce
		{
			get
			{
				return this.mIntData[4];
			}
			set
			{
				this.mIntData[4] = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000D1E RID: 3358 RVA: 0x00040579 File Offset: 0x0003E779
		// (set) Token: 0x06000D1F RID: 3359 RVA: 0x00040583 File Offset: 0x0003E783
		public int mNumber
		{
			get
			{
				return this.mIntData[5];
			}
			set
			{
				this.mIntData[5] = value;
			}
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0004058E File Offset: 0x0003E78E
		public override void Init()
		{
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00040590 File Offset: 0x0003E790
		public override KeyFrameData Clone()
		{
			return new ParticleSettings(this);
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00040598 File Offset: 0x0003E798
		protected void Reset()
		{
			this.mNumInts = 6;
			this.mNumFloats = 4;
			this.mIntData = new int[this.mNumInts];
			this.mFloatData = new float[this.mNumFloats];
			this.mLife = 0;
			this.mXSize = 0;
			this.mYSize = 0;
			this.mVelocity = 0;
			this.mBounce = 0;
			this.mNumber = 0;
			this.mWeight = 0f;
			this.mSpin = 0f;
			this.mMotionRand = 0f;
			this.mGlobalVisibility = 1f;
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0004062B File Offset: 0x0003E82B
		public ParticleSettings()
		{
			this.Reset();
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00040639 File Offset: 0x0003E839
		public ParticleSettings(ParticleSettings rhs)
		{
			this.Reset();
			base.CopyFrom(rhs);
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00040650 File Offset: 0x0003E850
		public new static KeyFrameData Instantiate()
		{
			return new ParticleSettings();
		}
	}
}
