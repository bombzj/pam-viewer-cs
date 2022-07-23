using System;

namespace SexyFramework.PIL
{
	// Token: 0x02000164 RID: 356
	public class ForceSettings : KeyFrameData
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x0003EB7E File Offset: 0x0003CD7E
		// (set) Token: 0x06000C92 RID: 3218 RVA: 0x0003EB88 File Offset: 0x0003CD88
		public float mWidth
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

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x0003EB93 File Offset: 0x0003CD93
		// (set) Token: 0x06000C94 RID: 3220 RVA: 0x0003EB9D File Offset: 0x0003CD9D
		public float mHeight
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

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x0003EBA8 File Offset: 0x0003CDA8
		// (set) Token: 0x06000C96 RID: 3222 RVA: 0x0003EBB2 File Offset: 0x0003CDB2
		public float mStrength
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x0003EBBD File Offset: 0x0003CDBD
		// (set) Token: 0x06000C98 RID: 3224 RVA: 0x0003EBC7 File Offset: 0x0003CDC7
		public float mDirection
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

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x0003EBD2 File Offset: 0x0003CDD2
		// (set) Token: 0x06000C9A RID: 3226 RVA: 0x0003EBDC File Offset: 0x0003CDDC
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

		// Token: 0x06000C9B RID: 3227 RVA: 0x0003EBE7 File Offset: 0x0003CDE7
		public override void Init()
		{
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0003EBE9 File Offset: 0x0003CDE9
		public override KeyFrameData Clone()
		{
			return new ForceSettings(this);
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0003EBF4 File Offset: 0x0003CDF4
		protected void Reset()
		{
			this.mNumFloats = 5;
			this.mFloatData = new float[this.mNumFloats];
			this.mWidth = 0f;
			this.mHeight = 0f;
			this.mStrength = 0f;
			this.mDirection = 0f;
			this.mAngle = 0f;
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0003EC50 File Offset: 0x0003CE50
		public ForceSettings()
		{
			this.Reset();
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x0003EC5E File Offset: 0x0003CE5E
		public ForceSettings(ForceSettings rhs)
		{
			this.Reset();
			base.CopyFrom(rhs);
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0003EC74 File Offset: 0x0003CE74
		public new static KeyFrameData Instantiate()
		{
			return new ForceSettings();
		}
	}
}
