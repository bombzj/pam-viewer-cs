using System;

namespace SexyFramework.PIL
{
	// Token: 0x02000160 RID: 352
	public class EmitterScale : KeyFrameData
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000C16 RID: 3094 RVA: 0x0003AB4A File Offset: 0x00038D4A
		// (set) Token: 0x06000C17 RID: 3095 RVA: 0x0003AB54 File Offset: 0x00038D54
		public float mLifeScale
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

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000C18 RID: 3096 RVA: 0x0003AB5F File Offset: 0x00038D5F
		// (set) Token: 0x06000C19 RID: 3097 RVA: 0x0003AB69 File Offset: 0x00038D69
		public float mNumberScale
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

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000C1A RID: 3098 RVA: 0x0003AB74 File Offset: 0x00038D74
		// (set) Token: 0x06000C1B RID: 3099 RVA: 0x0003AB7E File Offset: 0x00038D7E
		public float mSizeXScale
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

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x0003AB89 File Offset: 0x00038D89
		// (set) Token: 0x06000C1D RID: 3101 RVA: 0x0003AB93 File Offset: 0x00038D93
		public float mSizeYScale
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

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x0003AB9E File Offset: 0x00038D9E
		// (set) Token: 0x06000C1F RID: 3103 RVA: 0x0003ABA8 File Offset: 0x00038DA8
		public float mVelocityScale
		{
			get
			{
				return this.mFloatData[4];
			}
			set
			{
				this.mFloatData[4] = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x0003ABB3 File Offset: 0x00038DB3
		// (set) Token: 0x06000C21 RID: 3105 RVA: 0x0003ABBD File Offset: 0x00038DBD
		public float mWeightScale
		{
			get
			{
				return this.mFloatData[5];
			}
			set
			{
				this.mFloatData[5] = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x0003ABC8 File Offset: 0x00038DC8
		// (set) Token: 0x06000C23 RID: 3107 RVA: 0x0003ABD2 File Offset: 0x00038DD2
		public float mSpinScale
		{
			get
			{
				return this.mFloatData[6];
			}
			set
			{
				this.mFloatData[6] = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x0003ABDD File Offset: 0x00038DDD
		// (set) Token: 0x06000C25 RID: 3109 RVA: 0x0003ABE7 File Offset: 0x00038DE7
		public float mMotionRandScale
		{
			get
			{
				return this.mFloatData[7];
			}
			set
			{
				this.mFloatData[7] = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x0003ABF2 File Offset: 0x00038DF2
		// (set) Token: 0x06000C27 RID: 3111 RVA: 0x0003ABFC File Offset: 0x00038DFC
		public float mZoom
		{
			get
			{
				return this.mFloatData[8];
			}
			set
			{
				this.mFloatData[8] = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000C28 RID: 3112 RVA: 0x0003AC07 File Offset: 0x00038E07
		// (set) Token: 0x06000C29 RID: 3113 RVA: 0x0003AC12 File Offset: 0x00038E12
		public float mBounceScale
		{
			get
			{
				return this.mFloatData[9];
			}
			set
			{
				this.mFloatData[9] = value;
			}
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x0003AC1E File Offset: 0x00038E1E
		public override void Init()
		{
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x0003AC20 File Offset: 0x00038E20
		public override KeyFrameData Clone()
		{
			return new EmitterScale(this);
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x0003AC28 File Offset: 0x00038E28
		protected void Reset()
		{
			this.mNumFloats = 10;
			this.mFloatData = new float[this.mNumFloats];
			this.mLifeScale = 1f;
			this.mNumberScale = 1f;
			this.mSizeXScale = 1f;
			this.mSizeYScale = 1f;
			this.mVelocityScale = 1f;
			this.mWeightScale = 1f;
			this.mSpinScale = 1f;
			this.mMotionRandScale = 1f;
			this.mZoom = 1f;
			this.mBounceScale = 1f;
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x0003ACBC File Offset: 0x00038EBC
		public EmitterScale()
		{
			this.Reset();
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x0003ACCA File Offset: 0x00038ECA
		public EmitterScale(EmitterScale rhs)
		{
			this.Reset();
			base.CopyFrom(rhs);
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x0003ACE0 File Offset: 0x00038EE0
		public new static KeyFrameData Instantiate()
		{
			return new EmitterScale();
		}
	}
}
