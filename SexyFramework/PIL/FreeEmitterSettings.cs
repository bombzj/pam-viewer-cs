using System;

namespace SexyFramework.PIL
{
	// Token: 0x02000166 RID: 358
	public class FreeEmitterSettings : KeyFrameData
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x0003EFB9 File Offset: 0x0003D1B9
		// (set) Token: 0x06000CAC RID: 3244 RVA: 0x0003EFC3 File Offset: 0x0003D1C3
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

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x0003EFCE File Offset: 0x0003D1CE
		// (set) Token: 0x06000CAE RID: 3246 RVA: 0x0003EFD8 File Offset: 0x0003D1D8
		public int mNumber
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

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x0003EFE3 File Offset: 0x0003D1E3
		// (set) Token: 0x06000CB0 RID: 3248 RVA: 0x0003EFED File Offset: 0x0003D1ED
		public int mVelocity
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

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x0003EFF8 File Offset: 0x0003D1F8
		// (set) Token: 0x06000CB2 RID: 3250 RVA: 0x0003F002 File Offset: 0x0003D202
		public int mWeight
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

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x0003F00D File Offset: 0x0003D20D
		// (set) Token: 0x06000CB4 RID: 3252 RVA: 0x0003F017 File Offset: 0x0003D217
		public int mMotionRand
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

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x0003F022 File Offset: 0x0003D222
		// (set) Token: 0x06000CB6 RID: 3254 RVA: 0x0003F02C File Offset: 0x0003D22C
		public int mBounce
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

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x0003F037 File Offset: 0x0003D237
		// (set) Token: 0x06000CB8 RID: 3256 RVA: 0x0003F041 File Offset: 0x0003D241
		public int mZoom
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

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x0003F04C File Offset: 0x0003D24C
		// (set) Token: 0x06000CBA RID: 3258 RVA: 0x0003F056 File Offset: 0x0003D256
		public float mSpin
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

		// Token: 0x06000CBB RID: 3259 RVA: 0x0003F061 File Offset: 0x0003D261
		public override void Init()
		{
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0003F063 File Offset: 0x0003D263
		public override KeyFrameData Clone()
		{
			return new FreeEmitterSettings(this);
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0003F06C File Offset: 0x0003D26C
		protected void Reset()
		{
			this.mNumInts = 7;
			this.mIntData = new int[this.mNumInts];
			this.mNumFloats = 1;
			this.mFloatData = new float[this.mNumFloats];
			this.mLife = 0;
			this.mNumber = 0;
			this.mVelocity = 0;
			this.mWeight = 0;
			this.mSpin = 0f;
			this.mMotionRand = 0;
			this.mBounce = 0;
			this.mZoom = 100;
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0003F0E6 File Offset: 0x0003D2E6
		public FreeEmitterSettings()
		{
			this.Reset();
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0003F0F4 File Offset: 0x0003D2F4
		public FreeEmitterSettings(FreeEmitterSettings rhs)
		{
			this.Reset();
			base.CopyFrom(rhs);
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0003F10C File Offset: 0x0003D30C
		public new static KeyFrameData Instantiate()
		{
			return new FreeEmitterSettings();
		}
	}
}
