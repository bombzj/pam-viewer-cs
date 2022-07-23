using System;

namespace SexyFramework.Misc
{
	// Token: 0x02000136 RID: 310
	public class MTRand
	{
		// Token: 0x06000A30 RID: 2608 RVA: 0x00034B27 File Offset: 0x00032D27
		public MTRand(string theSerialData)
		{
			this.SRand(theSerialData);
			this.mti = MTRand.MTRAND_N + 1;
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x00034B53 File Offset: 0x00032D53
		public MTRand(uint seed)
		{
			this.SRand(seed);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x00034B72 File Offset: 0x00032D72
		public MTRand()
		{
			this.SRand(4357U);
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x00034B98 File Offset: 0x00032D98
		public void SRand(string theSerialData)
		{
			if (theSerialData.Length == MTRand.MTRAND_N * 4 + 4)
			{
				string text = theSerialData.Substring(0, 4);
				this.mti = int.Parse(text);
				return;
			}
			this.SRand(4357U);
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x00034BD8 File Offset: 0x00032DD8
		public void SRand(uint seed)
		{
			if (seed == 0U)
			{
				seed = 4357U;
			}
			this.mt[0] = seed & uint.MaxValue;
			this.mti = 1;
			while (this.mti < MTRand.MTRAND_N)
			{
				this.mt[this.mti] = 1812433253U * (this.mt[this.mti - 1] ^ (this.mt[this.mti - 1] >> 30)) + (uint)this.mti;
				this.mt[this.mti] &= uint.MaxValue;
				this.mti++;
			}
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x00034C78 File Offset: 0x00032E78
		private uint NextNoAssert()
		{
			uint num;
			if (this.mti >= MTRand.MTRAND_N)
			{
				int i;
				for (i = 0; i < MTRand.MTRAND_N - MTRand.MTRAND_M; i++)
				{
					num = (this.mt[i] & MTRand.UPPER_MASK) | (this.mt[i + 1] & MTRand.LOWER_MASK);
					this.mt[i] = this.mt[i + MTRand.MTRAND_M] ^ (num >> 1) ^ MTRand.mag01[(int)(checked((IntPtr)(unchecked((ulong)num) & 1UL)))];
				}
				while (i < MTRand.MTRAND_N - 1)
				{
					num = (this.mt[i] & MTRand.UPPER_MASK) | (this.mt[i + 1] & MTRand.LOWER_MASK);
					this.mt[i] = this.mt[i + (MTRand.MTRAND_M - MTRand.MTRAND_N)] ^ (num >> 1) ^ MTRand.mag01[(int)(checked((IntPtr)(unchecked((ulong)num) & 1UL)))];
					i++;
				}
				num = (this.mt[MTRand.MTRAND_N - 1] & MTRand.UPPER_MASK) | (this.mt[0] & MTRand.LOWER_MASK);
				this.mt[MTRand.MTRAND_N - 1] = this.mt[MTRand.MTRAND_M - 1] ^ (num >> 1) ^ MTRand.mag01[(int)(checked((IntPtr)(unchecked((ulong)num) & 1UL)))];
				this.mti = 0;
			}
			num = this.mt[this.mti++];
			num ^= num >> 11;
			num ^= (num << 7) & MTRand.TEMPERING_MASK_B;
			num ^= (num << 15) & MTRand.TEMPERING_MASK_C;
			num ^= num >> 18;
			return num & 2147483647U;
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x00034DF0 File Offset: 0x00032FF0
		public uint Next()
		{
			return this.NextNoAssert();
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x00034DF8 File Offset: 0x00032FF8
		public uint NextNoAssert(uint range)
		{
			return this.NextNoAssert() % range;
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00034E02 File Offset: 0x00033002
		public uint Next(uint range)
		{
			return this.NextNoAssert(range);
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x00034E0B File Offset: 0x0003300B
		public float NextNoAssert(float range)
		{
			return (float)(this.NextNoAssert() / 2147483647.0 * (double)range);
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x00034E23 File Offset: 0x00033023
		public float Next(float range)
		{
			return this.NextNoAssert(range);
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00034E2C File Offset: 0x0003302C
		public string Serialize()
		{
			return "";
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x00034E33 File Offset: 0x00033033
		public static void SetRandAllowed(bool allowed)
		{
		}

		// Token: 0x0400090D RID: 2317
		public static int MTRAND_N = 624;

		// Token: 0x0400090E RID: 2318
		public static int MTRAND_M = 397;

		// Token: 0x0400090F RID: 2319
		public static uint MATRIX_A = 2567483615U;

		// Token: 0x04000910 RID: 2320
		public static uint UPPER_MASK = 2147483648U;

		// Token: 0x04000911 RID: 2321
		public static uint LOWER_MASK = 2147483647U;

		// Token: 0x04000912 RID: 2322
		public static uint TEMPERING_MASK_B = 2636928640U;

		// Token: 0x04000913 RID: 2323
		public static uint TEMPERING_MASK_C = 4022730752U;

		// Token: 0x04000914 RID: 2324
		public static uint[] mag01 = new uint[]
		{
			default(uint),
			MTRand.MATRIX_A
		};

		// Token: 0x04000915 RID: 2325
		private uint[] mt = new uint[MTRand.MTRAND_N];

		// Token: 0x04000916 RID: 2326
		private int mti;
	}
}
