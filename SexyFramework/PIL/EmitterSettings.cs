using System;

namespace SexyFramework.PIL
{
	// Token: 0x02000161 RID: 353
	public class EmitterSettings : KeyFrameData
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x0003ACF4 File Offset: 0x00038EF4
		// (set) Token: 0x06000C31 RID: 3121 RVA: 0x0003ACFE File Offset: 0x00038EFE
		public bool mActive
		{
			get
			{
				return this.mBoolData[0];
			}
			set
			{
				this.mBoolData[0] = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x0003AD09 File Offset: 0x00038F09
		// (set) Token: 0x06000C33 RID: 3123 RVA: 0x0003AD13 File Offset: 0x00038F13
		public float mVisibility
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

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x0003AD1E File Offset: 0x00038F1E
		// (set) Token: 0x06000C35 RID: 3125 RVA: 0x0003AD28 File Offset: 0x00038F28
		public float mEmissionAngle
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

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x0003AD33 File Offset: 0x00038F33
		// (set) Token: 0x06000C37 RID: 3127 RVA: 0x0003AD3D File Offset: 0x00038F3D
		public float mEmissionRange
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

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x0003AD48 File Offset: 0x00038F48
		// (set) Token: 0x06000C39 RID: 3129 RVA: 0x0003AD52 File Offset: 0x00038F52
		public float mTintStrength
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x0003AD5D File Offset: 0x00038F5D
		// (set) Token: 0x06000C3B RID: 3131 RVA: 0x0003AD67 File Offset: 0x00038F67
		public float mAngle
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

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x0003AD72 File Offset: 0x00038F72
		// (set) Token: 0x06000C3D RID: 3133 RVA: 0x0003AD7C File Offset: 0x00038F7C
		public float mXRadius
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

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x0003AD87 File Offset: 0x00038F87
		// (set) Token: 0x06000C3F RID: 3135 RVA: 0x0003AD91 File Offset: 0x00038F91
		public float mYRadius
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

		// Token: 0x06000C40 RID: 3136 RVA: 0x0003AD9C File Offset: 0x00038F9C
		public override void Init()
		{
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x0003AD9E File Offset: 0x00038F9E
		public override KeyFrameData Clone()
		{
			return new EmitterSettings(this);
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x0003ADA8 File Offset: 0x00038FA8
		protected void Reset()
		{
			this.mNumFloats = 7;
			this.mNumBools = 1;
			this.mFloatData = new float[this.mNumFloats];
			this.mBoolData = new bool[this.mNumBools];
			this.mVisibility = 1f;
			this.mEmissionAngle = 0f;
			this.mEmissionRange = 6.2831855f;
			this.mTintStrength = 0f;
			this.mActive = true;
			this.mAngle = 0f;
			this.mXRadius = 0f;
			this.mYRadius = 0f;
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x0003AE39 File Offset: 0x00039039
		public EmitterSettings()
		{
			this.Reset();
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x0003AE47 File Offset: 0x00039047
		public EmitterSettings(EmitterSettings emitterSettings)
		{
			this.Reset();
			base.CopyFrom(emitterSettings);
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0003AE5C File Offset: 0x0003905C
		public new static KeyFrameData Instantiate()
		{
			return new EmitterSettings();
		}
	}
}
