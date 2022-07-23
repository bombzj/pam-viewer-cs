using System;

namespace SexyFramework.Misc
{
	// Token: 0x02000143 RID: 323
	public class SexyQuat3
	{
		// Token: 0x06000AEC RID: 2796 RVA: 0x000371B6 File Offset: 0x000353B6
		public SexyQuat3()
		{
			this.v = new SexyVector3(0f, 0f, 0f);
			this.s = 1f;
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x000371EF File Offset: 0x000353EF
		public SexyQuat3(SexyQuat3 inQ)
		{
			this.v = inQ.v;
			this.s = inQ.s;
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0003721B File Offset: 0x0003541B
		public SexyQuat3(SexyVector3 inV, float inS)
		{
			this.v = inV;
			this.s = inS;
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x00037240 File Offset: 0x00035440
		public SexyQuat3(SexyAxes3 inAxes)
		{
			SexyAxes3 sexyAxes = inAxes.OrthoNormalize();
			uint[] array = new uint[3];
			array[0] = 1U;
			array[1] = 2U;
			uint[] array2 = array;
			float[,] array3 = new float[3, 3];
			array3[0, 0] = sexyAxes.vX.x;
			array3[0, 1] = sexyAxes.vX.y;
			array3[0, 2] = sexyAxes.vX.z;
			array3[1, 0] = sexyAxes.vY.x;
			array3[1, 1] = sexyAxes.vY.y;
			array3[1, 2] = sexyAxes.vY.z;
			array3[2, 0] = sexyAxes.vZ.x;
			array3[2, 1] = sexyAxes.vZ.y;
			array3[2, 2] = sexyAxes.vZ.z;
			float[,] array4 = array3;
			float[] array5 = new float[4];
			float num = array4[0, 0] + array4[1, 1] + array4[2, 2];
			float num2;
			if (num > 0f)
			{
				num2 = (float)Math.Sqrt((double)(num + 1f));
				this.s = num2 * 0.5f;
				num2 = 0.5f / num2;
				this.v.x = (array4[1, 2] - array4[2, 1]) * num2;
				this.v.y = (array4[2, 0] - array4[0, 2]) * num2;
				this.v.z = (array4[0, 1] - array4[1, 0]) * num2;
				return;
			}
			uint num3 = 0U;
			if (array4[1, 1] > array4[0, 0])
			{
				num3 = 1U;
			}
			if (array4[2, 2] > array4[(int)((UIntPtr)num3), (int)((UIntPtr)num3)])
			{
				num3 = 2U;
			}
			uint num4 = array2[(int)((UIntPtr)num3)];
			uint num5 = array2[(int)((UIntPtr)num4)];
			num2 = (float)Math.Sqrt((double)(array4[(int)((UIntPtr)num3), (int)((UIntPtr)num3)] - (array4[(int)((UIntPtr)num4), (int)((UIntPtr)num4)] + array4[(int)((UIntPtr)num5), (int)((UIntPtr)num5)]) + 1f));
			array5[(int)((UIntPtr)num3)] = num2 * 0.5f;
			if (num2 != 0f)
			{
				num2 = 0.5f / num2;
			}
			this.s = (array4[(int)((UIntPtr)num4), (int)((UIntPtr)num5)] - array4[(int)((UIntPtr)num5), (int)((UIntPtr)num4)]) * num2;
			array5[(int)((UIntPtr)num4)] = (array4[(int)((UIntPtr)num3), (int)((UIntPtr)num4)] + array4[(int)((UIntPtr)num4), (int)((UIntPtr)num3)]) * num2;
			array5[(int)((UIntPtr)num5)] = (array4[(int)((UIntPtr)num3), (int)((UIntPtr)num5)] + array4[(int)((UIntPtr)num5), (int)((UIntPtr)num3)]) * num2;
			this.v.x = array5[0];
			this.v.y = array5[1];
			this.v.z = array5[2];
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00037510 File Offset: 0x00035710
		public static implicit operator SexyAxes3(SexyQuat3 ImpliedObject)
		{
			float x = ImpliedObject.v.x;
			float y = ImpliedObject.v.y;
			float z = ImpliedObject.v.z;
			float num = ImpliedObject.s;
			float num2 = x * 2f;
			float num3 = y * 2f;
			float num4 = z * 2f;
			float num5 = num * 2f;
			float num6 = x * num2;
			float num7 = y * num3;
			float num8 = z * num4;
			float num9 = x * num3;
			float num10 = x * num4;
			float num11 = x * num5;
			float num12 = y * num4;
			float num13 = y * num5;
			float num14 = z * num5;
			return new SexyAxes3(new SexyVector3(1f - (num7 + num8), num9 + num14, num10 - num13), new SexyVector3(num9 - num14, 1f - (num6 + num8), num12 + num11), new SexyVector3(num10 + num13, num12 - num11, 1f - (num6 + num7)));
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x000375F8 File Offset: 0x000357F8
		public static SexyQuat3 AxisAngle(SexyVector3 inAxis, float inAngleRad)
		{
			SexyQuat3 sexyQuat = new SexyQuat3();
			inAngleRad *= 0.5f;
			sexyQuat.v = inAxis.Normalize();
			sexyQuat.v *= (float)Math.Sin((double)inAngleRad);
			sexyQuat.s = (float)Math.Cos((double)inAngleRad);
			return sexyQuat;
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0003764C File Offset: 0x0003584C
		public static SexyQuat3 Slerp(SexyQuat3 inL, SexyQuat3 inR, float inAlpha, bool inLerpOnly)
		{
			if (inL.ApproxEquals(new SexyQuat3(inR), GlobalMembers.SEXYMATH_EPSILONSQ))
			{
				return inL;
			}
			float num = Math.Min(Math.Max(inAlpha, 0f), 1f);
			float num2 = 1f - num;
			SexyQuat3 sexyQuat = new SexyQuat3(new SexyQuat3(inR));
			float num3 = inL.Dot(new SexyQuat3(inR));
			if (num3 < 0f)
			{
				num3 = -num3;
				sexyQuat = new SexyQuat3(-sexyQuat.v, -sexyQuat.s);
			}
			if (1f - num3 > GlobalMembers.SEXYMATH_EPSILON && !inLerpOnly)
			{
				float num4 = (float)Math.Acos((double)num3);
				if (num4 >= GlobalMembers.SEXYMATH_EPSILON && GlobalMembers.SEXYMATH_PI - num4 >= GlobalMembers.SEXYMATH_EPSILON)
				{
					float num5 = (float)Math.Sin((double)num4);
					float num6 = 1f / num5;
					float num7 = (float)Math.Sin((double)num2 * (double)num4) * num6;
					float num8 = (float)Math.Sin((double)num * (double)num4) * num6;
					float inS = inL.s * num7 + sexyQuat.s * num8;
					SexyVector3 inV = inL.v * num7 + sexyQuat.v * num8;
					return new SexyQuat3(inV, inS).Normalize();
				}
			}
			float inS2 = inL.s * num2 + sexyQuat.s * num;
			SexyVector3 inV2 = inL.v * num2 + sexyQuat.v * num;
			return new SexyQuat3(inV2, inS2).Normalize();
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x000377C0 File Offset: 0x000359C0
		public void CopyFrom(SexyQuat3 inQ)
		{
			this.v = inQ.v;
			this.s = inQ.s;
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x000377DA File Offset: 0x000359DA
		public static SexyQuat3 operator +(SexyQuat3 ImpliedObject, SexyQuat3 inQ)
		{
			return new SexyQuat3(ImpliedObject.v + inQ.v, ImpliedObject.s + inQ.s);
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x000377FF File Offset: 0x000359FF
		public static SexyQuat3 operator -(SexyQuat3 ImpliedObject, SexyQuat3 inQ)
		{
			return new SexyQuat3(ImpliedObject.v - inQ.v, ImpliedObject.s - inQ.s);
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x00037824 File Offset: 0x00035A24
		public static SexyQuat3 operator *(SexyQuat3 ImpliedObject, SexyQuat3 inQ)
		{
			return new SexyQuat3(inQ.v * ImpliedObject.s + ImpliedObject.v * inQ.s + ImpliedObject.v.Cross(inQ.v), ImpliedObject.s * inQ.s - ImpliedObject.v.Dot(inQ.v));
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x00037892 File Offset: 0x00035A92
		public static SexyQuat3 operator *(SexyQuat3 ImpliedObject, float inT)
		{
			return new SexyQuat3(ImpliedObject.v * inT, ImpliedObject.s * inT);
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x000378AD File Offset: 0x00035AAD
		public static SexyQuat3 operator /(SexyQuat3 ImpliedObject, float inT)
		{
			return new SexyQuat3(ImpliedObject.v / inT, ImpliedObject.s / inT);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x000378C8 File Offset: 0x00035AC8
		public float Dot(SexyQuat3 inQ)
		{
			return this.v.Dot(inQ.v) + this.s * inQ.s;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x000378EC File Offset: 0x00035AEC
		public float Magnitude()
		{
			return (float)Math.Sqrt((double)this.v.x * (double)this.v.x + (double)(this.v.y * this.v.y) + (double)(this.v.z * this.v.z) + (double)(this.s * this.s));
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0003795C File Offset: 0x00035B5C
		public SexyQuat3 Normalize()
		{
			float num = this.Magnitude();
			if (num == 0f)
			{
				return this;
			}
			return this / num;
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x00037981 File Offset: 0x00035B81
		public bool ApproxEquals(SexyQuat3 inQ, float inTol)
		{
			return SexyMath.ApproxEquals(this.s, inQ.s, inTol) && this.v.ApproxEquals(inQ.v, inTol);
		}

		// Token: 0x0400093C RID: 2364
		public SexyVector3 v = default(SexyVector3);

		// Token: 0x0400093D RID: 2365
		public float s;
	}
}
