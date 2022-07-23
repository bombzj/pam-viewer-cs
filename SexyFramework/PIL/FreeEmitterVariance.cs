using System;

namespace SexyFramework.PIL
{
	// Token: 0x02000167 RID: 359
	public class FreeEmitterVariance : KeyFrameData
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x0003F120 File Offset: 0x0003D320
		// (set) Token: 0x06000CC2 RID: 3266 RVA: 0x0003F12A File Offset: 0x0003D32A
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

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x0003F135 File Offset: 0x0003D335
		// (set) Token: 0x06000CC4 RID: 3268 RVA: 0x0003F13F File Offset: 0x0003D33F
		public int mSizeXVar
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

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x0003F14A File Offset: 0x0003D34A
		// (set) Token: 0x06000CC6 RID: 3270 RVA: 0x0003F154 File Offset: 0x0003D354
		public int mSizeYVar
		{
			get
			{
				return this.mIntData[7];
			}
			set
			{
				this.mIntData[7] = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0003F15F File Offset: 0x0003D35F
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x0003F169 File Offset: 0x0003D369
		public int mVelocityVar
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

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x0003F174 File Offset: 0x0003D374
		// (set) Token: 0x06000CCA RID: 3274 RVA: 0x0003F17E File Offset: 0x0003D37E
		public int mWeightVar
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

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x0003F189 File Offset: 0x0003D389
		// (set) Token: 0x06000CCC RID: 3276 RVA: 0x0003F193 File Offset: 0x0003D393
		public float mSpinVar
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

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x0003F19E File Offset: 0x0003D39E
		// (set) Token: 0x06000CCE RID: 3278 RVA: 0x0003F1A8 File Offset: 0x0003D3A8
		public int mMotionRandVar
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

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x0003F1B3 File Offset: 0x0003D3B3
		// (set) Token: 0x06000CD0 RID: 3280 RVA: 0x0003F1BD File Offset: 0x0003D3BD
		public int mBounceVar
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

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x0003F1C8 File Offset: 0x0003D3C8
		// (set) Token: 0x06000CD2 RID: 3282 RVA: 0x0003F1D2 File Offset: 0x0003D3D2
		public int mZoomVar
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

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0003F1DD File Offset: 0x0003D3DD
		public override void Init()
		{
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0003F1DF File Offset: 0x0003D3DF
		public override KeyFrameData Clone()
		{
			return new FreeEmitterVariance(this);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0003F1E8 File Offset: 0x0003D3E8
		protected void Reset()
		{
			this.mNumInts = 8;
			this.mIntData = new int[this.mNumInts];
			this.mNumFloats = 1;
			this.mFloatData = new float[this.mNumFloats];
			this.mLifeVar = 0;
			this.mSizeXVar = 0;
			this.mSizeYVar = 0;
			this.mVelocityVar = 0;
			this.mWeightVar = 0;
			this.mSpinVar = 0f;
			this.mMotionRandVar = 0;
			this.mBounceVar = 0;
			this.mZoomVar = 0;
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0003F268 File Offset: 0x0003D468
		public FreeEmitterVariance()
		{
			this.Reset();
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0003F276 File Offset: 0x0003D476
		public FreeEmitterVariance(FreeEmitterVariance rhs)
		{
			this.Reset();
			base.CopyFrom(rhs);
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0003F28C File Offset: 0x0003D48C
		public new static KeyFrameData Instantiate()
		{
			return new FreeEmitterVariance();
		}
	}
}
