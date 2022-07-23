using System;

namespace SexyFramework.PIL
{
	// Token: 0x02000157 RID: 343
	public class DeflectorSettings : KeyFrameData
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x00039E0D File Offset: 0x0003800D
		// (set) Token: 0x06000BF3 RID: 3059 RVA: 0x00039E17 File Offset: 0x00038017
		public int mThickness
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

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x00039E22 File Offset: 0x00038022
		// (set) Token: 0x06000BF5 RID: 3061 RVA: 0x00039E2C File Offset: 0x0003802C
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

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x00039E37 File Offset: 0x00038037
		// (set) Token: 0x06000BF7 RID: 3063 RVA: 0x00039E41 File Offset: 0x00038041
		public float mAngle
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

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x00039E4C File Offset: 0x0003804C
		// (set) Token: 0x06000BF9 RID: 3065 RVA: 0x00039E56 File Offset: 0x00038056
		public float mBounceMult
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

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x00039E61 File Offset: 0x00038061
		// (set) Token: 0x06000BFB RID: 3067 RVA: 0x00039E6B File Offset: 0x0003806B
		public float mHitChance
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

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x00039E76 File Offset: 0x00038076
		// (set) Token: 0x06000BFD RID: 3069 RVA: 0x00039E80 File Offset: 0x00038080
		public float mCollisionMult
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

		// Token: 0x06000BFE RID: 3070 RVA: 0x00039E8B File Offset: 0x0003808B
		public override void Init()
		{
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00039E8D File Offset: 0x0003808D
		public override KeyFrameData Clone()
		{
			return new DeflectorSettings(this);
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00039E98 File Offset: 0x00038098
		protected void Reset()
		{
			this.mNumInts = 1;
			this.mNumBools = 1;
			this.mNumFloats = 4;
			this.mIntData = new int[this.mNumInts];
			this.mFloatData = new float[this.mNumFloats];
			this.mBoolData = new bool[this.mNumBools];
			this.mThickness = 2;
			this.mAngle = 0f;
			this.mBounceMult = 1f;
			this.mHitChance = 1f;
			this.mActive = true;
			this.mCollisionMult = 1f;
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00039F27 File Offset: 0x00038127
		public DeflectorSettings()
		{
			this.Reset();
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00039F35 File Offset: 0x00038135
		public DeflectorSettings(DeflectorSettings rhs)
		{
			this.Reset();
			base.CopyFrom(rhs);
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00039F4C File Offset: 0x0003814C
		public new static KeyFrameData Instantiate()
		{
			return new DeflectorSettings();
		}
	}
}
