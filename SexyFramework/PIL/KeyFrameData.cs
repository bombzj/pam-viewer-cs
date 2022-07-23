using System;
using SexyFramework.Misc;

namespace SexyFramework.PIL
{
	// Token: 0x02000156 RID: 342
	public class KeyFrameData
	{
		// Token: 0x06000BE9 RID: 3049 RVA: 0x00039B88 File Offset: 0x00037D88
		public void CopyFrom(KeyFrameData k)
		{
			if (this.mNumInts != k.mNumInts)
			{
				this.mNumInts = k.mNumInts;
				if (this.mNumInts > 0)
				{
					this.mIntData = new int[this.mNumInts];
				}
				else
				{
					this.mIntData = null;
				}
			}
			if (this.mNumFloats != k.mNumFloats)
			{
				this.mNumFloats = k.mNumFloats;
				if (this.mNumFloats > 0)
				{
					this.mFloatData = new float[this.mNumFloats];
				}
				else
				{
					this.mFloatData = null;
				}
			}
			if (this.mNumBools != k.mNumBools)
			{
				this.mNumBools = k.mNumBools;
				if (this.mNumBools > 0)
				{
					this.mBoolData = new bool[this.mNumBools];
				}
				else
				{
					this.mBoolData = null;
				}
			}
			if (this.mNumInts > 0)
			{
				Array.Copy(k.mIntData, this.mIntData, this.mNumInts);
			}
			if (this.mNumFloats > 0)
			{
				Array.Copy(k.mFloatData, this.mFloatData, this.mNumFloats);
			}
			if (this.mNumBools > 0)
			{
				Array.Copy(k.mBoolData, this.mBoolData, this.mNumBools);
			}
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00039CAC File Offset: 0x00037EAC
		public virtual void Init()
		{
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00039CAE File Offset: 0x00037EAE
		public virtual KeyFrameData Clone()
		{
			return new KeyFrameData(this);
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00039CB6 File Offset: 0x00037EB6
		public KeyFrameData()
		{
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00039CBE File Offset: 0x00037EBE
		public KeyFrameData(KeyFrameData k)
		{
			this.CopyFrom(k);
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00039CCD File Offset: 0x00037ECD
		public virtual void Dispose()
		{
			this.mIntData = null;
			this.mFloatData = null;
			this.mBoolData = null;
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00039CE4 File Offset: 0x00037EE4
		public virtual void Serialize(SexyBuffer b)
		{
			b.WriteLong((long)this.mNumInts);
			b.WriteLong((long)this.mNumFloats);
			b.WriteLong((long)this.mNumBools);
			for (int i = 0; i < this.mNumInts; i++)
			{
				b.WriteLong((long)this.mIntData[i]);
			}
			for (int j = 0; j < this.mNumFloats; j++)
			{
				b.WriteFloat(this.mFloatData[j]);
			}
			for (int k = 0; k < this.mNumBools; k++)
			{
				b.WriteBoolean(this.mBoolData[k]);
			}
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00039D78 File Offset: 0x00037F78
		public virtual void Deserialize(SexyBuffer b)
		{
			this.mNumInts = (int)b.ReadLong();
			this.mNumFloats = (int)b.ReadLong();
			this.mNumBools = (int)b.ReadLong();
			for (int i = 0; i < this.mNumInts; i++)
			{
				this.mIntData[i] = (int)b.ReadLong();
			}
			for (int j = 0; j < this.mNumFloats; j++)
			{
				this.mFloatData[j] = b.ReadFloat();
			}
			for (int k = 0; k < this.mNumBools; k++)
			{
				this.mBoolData[k] = b.ReadBoolean();
			}
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00039E0A File Offset: 0x0003800A
		public static KeyFrameData Instantiate()
		{
			return null;
		}

		// Token: 0x04000973 RID: 2419
		public int[] mIntData;

		// Token: 0x04000974 RID: 2420
		public float[] mFloatData;

		// Token: 0x04000975 RID: 2421
		public bool[] mBoolData;

		// Token: 0x04000976 RID: 2422
		public int mNumInts;

		// Token: 0x04000977 RID: 2423
		public int mNumFloats;

		// Token: 0x04000978 RID: 2424
		public int mNumBools;
	}
}
