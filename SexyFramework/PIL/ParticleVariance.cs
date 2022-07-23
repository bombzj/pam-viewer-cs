using System;

namespace SexyFramework.PIL
{
	// Token: 0x02000171 RID: 369
	public class ParticleVariance : KeyFrameData
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000D26 RID: 3366 RVA: 0x00040664 File Offset: 0x0003E864
		// (set) Token: 0x06000D27 RID: 3367 RVA: 0x0004066E File Offset: 0x0003E86E
		public int mLifeVar
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

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000D28 RID: 3368 RVA: 0x00040679 File Offset: 0x0003E879
		// (set) Token: 0x06000D29 RID: 3369 RVA: 0x00040683 File Offset: 0x0003E883
		public int mNumberVar
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

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x0004068E File Offset: 0x0003E88E
		// (set) Token: 0x06000D2B RID: 3371 RVA: 0x00040698 File Offset: 0x0003E898
		public int mSizeXVar
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

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000D2C RID: 3372 RVA: 0x000406A3 File Offset: 0x0003E8A3
		// (set) Token: 0x06000D2D RID: 3373 RVA: 0x000406AD File Offset: 0x0003E8AD
		public int mSizeYVar
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

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000D2E RID: 3374 RVA: 0x000406B8 File Offset: 0x0003E8B8
		// (set) Token: 0x06000D2F RID: 3375 RVA: 0x000406C2 File Offset: 0x0003E8C2
		public int mVelocityVar
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

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000D30 RID: 3376 RVA: 0x000406CD File Offset: 0x0003E8CD
		// (set) Token: 0x06000D31 RID: 3377 RVA: 0x000406D7 File Offset: 0x0003E8D7
		public int mWeightVar
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

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000D32 RID: 3378 RVA: 0x000406E2 File Offset: 0x0003E8E2
		// (set) Token: 0x06000D33 RID: 3379 RVA: 0x000406EC File Offset: 0x0003E8EC
		public int mBounceVar
		{
			get
			{
				return this.mIntData[6];
			}
			set
			{
				this.mIntData[6] = value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000D34 RID: 3380 RVA: 0x000406F7 File Offset: 0x0003E8F7
		// (set) Token: 0x06000D35 RID: 3381 RVA: 0x00040701 File Offset: 0x0003E901
		public float mMotionRandVar
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

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000D36 RID: 3382 RVA: 0x0004070C File Offset: 0x0003E90C
		// (set) Token: 0x06000D37 RID: 3383 RVA: 0x00040716 File Offset: 0x0003E916
		public float mSpinVar
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

		// Token: 0x06000D38 RID: 3384 RVA: 0x00040721 File Offset: 0x0003E921
		public override void Init()
		{
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00040723 File Offset: 0x0003E923
		public override KeyFrameData Clone()
		{
			return new ParticleVariance(this);
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0004072C File Offset: 0x0003E92C
		protected void Reset()
		{
			this.mNumInts = 7;
			this.mNumFloats = 2;
			this.mIntData = new int[this.mNumInts];
			this.mFloatData = new float[this.mNumFloats];
			this.mLifeVar = 0;
			this.mNumberVar = 0;
			this.mSizeXVar = 0;
			this.mSizeYVar = 0;
			this.mVelocityVar = 0;
			this.mWeightVar = 0;
			this.mSpinVar = 0f;
			this.mMotionRandVar = 0f;
			this.mBounceVar = 0;
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x000407B0 File Offset: 0x0003E9B0
		public ParticleVariance()
		{
			this.Reset();
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x000407BE File Offset: 0x0003E9BE
		public ParticleVariance(ParticleVariance rhs)
		{
			this.Reset();
			base.CopyFrom(rhs);
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x000407D4 File Offset: 0x0003E9D4
		public new static KeyFrameData Instantiate()
		{
			return new ParticleVariance();
		}
	}
}
