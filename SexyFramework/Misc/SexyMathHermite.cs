using System;
using System.Collections.Generic;

namespace SexyFramework.Misc
{
	// Token: 0x02000147 RID: 327
	public class SexyMathHermite
	{
		// Token: 0x06000B13 RID: 2835 RVA: 0x00037B82 File Offset: 0x00035D82
		public SexyMathHermite()
		{
			this.mIsBuilt = false;
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00037BA7 File Offset: 0x00035DA7
		public void Rebuild()
		{
			this.mIsBuilt = false;
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00037BB0 File Offset: 0x00035DB0
		public float Evaluate(float inX)
		{
			if (!this.mIsBuilt)
			{
				if (!this.BuildCurve())
				{
					return 0f;
				}
				this.mIsBuilt = true;
			}
			int count = this.mPieces.Count;
			for (int i = 0; i < count; i++)
			{
				if (inX < this.mPoints[i + 1].mX)
				{
					return this.EvaluatePiece(inX, new SexyMathHermite.SPoint[]
					{
						this.mPoints[i],
						this.mPoints[i + 1]
					}, this.mPieces[i]);
				}
			}
			return this.mPoints[this.mPoints.Count - 1].mFx;
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00037C64 File Offset: 0x00035E64
		protected void CreatePiece(SexyMathHermite.SPoint[] inPoints, ref SexyMathHermite.SPiece outPiece)
		{
			float[,] array = new float[(int)((UIntPtr)4), (int)((UIntPtr)4)];
			float[] array2 = new float[4];
			for (uint num = 0U; num <= 1U; num += 1U)
			{
				uint num2 = 2U * num;
				array2[(int)((UIntPtr)num2)] = inPoints[(int)((UIntPtr)num)].mX;
				array2[(int)((UIntPtr)(num2 + 1U))] = inPoints[(int)((UIntPtr)num)].mX;
				array[(int)((UIntPtr)num2), (int)((UIntPtr)0)] = inPoints[(int)((UIntPtr)num)].mFx;
				array[(int)((UIntPtr)(num2 + 1U)), (int)((UIntPtr)0)] = inPoints[(int)((UIntPtr)num)].mFx;
				array[(int)((UIntPtr)(num2 + 1U)), (int)((UIntPtr)1)] = inPoints[(int)((UIntPtr)num)].mFxPrime;
				if (num != 0U)
				{
					array[(int)((UIntPtr)num2), (int)((UIntPtr)1)] = (array[(int)((UIntPtr)num2), (int)((UIntPtr)0)] - array[(int)((UIntPtr)(num2 - 1U)), (int)((UIntPtr)0)]) / (array2[(int)((UIntPtr)num2)] - array2[(int)((UIntPtr)(num2 - 1U))]);
				}
			}
			for (uint num3 = 2U; num3 < 4U; num3 += 1U)
			{
				for (uint num4 = 2U; num4 <= num3; num4 += 1U)
				{
					array[(int)((UIntPtr)num3), (int)((UIntPtr)num4)] = (array[(int)((UIntPtr)num3), (int)((UIntPtr)(num4 - 1U))] - array[(int)((UIntPtr)(num3 - 1U)), (int)((UIntPtr)(num4 - 1U))]) / (array2[(int)((UIntPtr)num3)] - array2[(int)((UIntPtr)(num3 - num4))]);
				}
			}
			for (uint num5 = 0U; num5 < 4U; num5 += 1U)
			{
				outPiece.mCoeffs[(int)((UIntPtr)num5)] = array[(int)((UIntPtr)num5), (int)((UIntPtr)num5)];
			}
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00037DA4 File Offset: 0x00035FA4
		protected float EvaluatePiece(float inX, SexyMathHermite.SPoint[] inPoints, SexyMathHermite.SPiece inPiece)
		{
			float[] array = new float[]
			{
				inX - inPoints[0].mX,
				inX - inPoints[1].mX
			};
			float num = 1f;
			float num2 = inPiece.mCoeffs[0];
			for (uint num3 = 1U; num3 < 4U; num3 += 1U)
			{
				num *= array[(int)((UIntPtr)((num3 - 1U) / 2U))];
				num2 += num * inPiece.mCoeffs[(int)((UIntPtr)num3)];
			}
			return num2;
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00037E08 File Offset: 0x00036008
		protected bool BuildCurve()
		{
			this.mPieces.Clear();
			uint count = (uint)this.mPoints.Count;
			if (count < 2U)
			{
				return false;
			}
			uint num = count - 1U;
			int num2 = 0;
			while ((long)num2 < (long)((ulong)num))
			{
				SexyMathHermite.SPiece spiece = new SexyMathHermite.SPiece();
				this.CreatePiece(new SexyMathHermite.SPoint[]
				{
					this.mPoints[num2],
					this.mPoints[num2 + 1]
				}, ref spiece);
				this.mPieces.Add(spiece);
				num2++;
			}
			return true;
		}

		// Token: 0x04000946 RID: 2374
		public List<SexyMathHermite.SPoint> mPoints = new List<SexyMathHermite.SPoint>();

		// Token: 0x04000947 RID: 2375
		protected List<SexyMathHermite.SPiece> mPieces = new List<SexyMathHermite.SPiece>();

		// Token: 0x04000948 RID: 2376
		protected bool mIsBuilt;

		// Token: 0x02000148 RID: 328
		public class SPoint
		{
			// Token: 0x06000B19 RID: 2841 RVA: 0x00037E8B File Offset: 0x0003608B
			public SPoint()
			{
			}

			// Token: 0x06000B1A RID: 2842 RVA: 0x00037E93 File Offset: 0x00036093
			public SPoint(float inX, float inFx, float inFxPrime)
			{
				this.mX = inX;
				this.mFx = inFx;
				this.mFxPrime = inFxPrime;
			}

			// Token: 0x04000949 RID: 2377
			public float mX;

			// Token: 0x0400094A RID: 2378
			public float mFx;

			// Token: 0x0400094B RID: 2379
			public float mFxPrime;
		}

		// Token: 0x02000149 RID: 329
		protected class SPiece
		{
			// Token: 0x0400094C RID: 2380
			public float[] mCoeffs = new float[4];
		}
	}
}
