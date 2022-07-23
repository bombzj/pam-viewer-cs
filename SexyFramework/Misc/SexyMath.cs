using System;

namespace SexyFramework.Misc
{
	// Token: 0x02000146 RID: 326
	public class SexyMath
	{
		// Token: 0x06000B07 RID: 2823 RVA: 0x00037AF6 File Offset: 0x00035CF6
		public static float Fabs(float inX)
		{
			return Math.Abs(inX);
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00037AFF File Offset: 0x00035CFF
		public static double Fabs(double inX)
		{
			return Math.Abs(inX);
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00037B07 File Offset: 0x00035D07
		public static float DegToRad(float inX)
		{
			return inX * 3.1415927f / 180f;
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00037B16 File Offset: 0x00035D16
		public static float RadToDeg(float inX)
		{
			return inX * 180f / 3.1415927f;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00037B25 File Offset: 0x00035D25
		public static bool ApproxEquals(float inL, float inR, float inTol)
		{
			return SexyMath.Fabs(inL - inR) <= inTol;
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x00037B35 File Offset: 0x00035D35
		public static bool ApproxEquals(double inL, double inR, double inTol)
		{
			return SexyMath.Fabs(inL - inR) <= inTol;
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00037B45 File Offset: 0x00035D45
		public static float Lerp(float inA, float inB, float inAlpha)
		{
			return inA + (inB - inA) * inAlpha;
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00037B4E File Offset: 0x00035D4E
		public static double Lerp(double inA, double inB, double inAlpha)
		{
			return inA + (inB - inA) * inAlpha;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00037B57 File Offset: 0x00035D57
		public static bool IsPowerOfTwo(uint inX)
		{
			return inX != 0U && (inX & (inX - 1U)) == 0U;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00037B66 File Offset: 0x00035D66
		public static float SinF(float value)
		{
			return (float)Math.Sin((double)value);
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00037B70 File Offset: 0x00035D70
		public static float CosF(float value)
		{
			return (float)Math.Cos((double)value);
		}
	}
}
