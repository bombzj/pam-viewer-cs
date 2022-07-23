using System;
using Microsoft.Xna.Framework;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000D5 RID: 213
	public static class GlobalPIEffect
	{
		// Token: 0x0600068B RID: 1675 RVA: 0x0001B8BC File Offset: 0x00019ABC
		internal static IntPtr GetData(PIEffect theEffect, IntPtr thePtr, int theSize)
		{
			return thePtr;
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001B8C0 File Offset: 0x00019AC0
		public static bool IsIdentityMatrix(SexyMatrix3 theMatrix)
		{
			return theMatrix.m01 == 0f && theMatrix.m02 == 0f && theMatrix.m10 == 0f && theMatrix.m12 == 0f && theMatrix.m20 == 0f && theMatrix.m21 == 0f && theMatrix.m00 == 1f && theMatrix.m11 == 1f && theMatrix.m22 == 1f;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001B944 File Offset: 0x00019B44
		public static float GetMatrixScale(SexyMatrix3 theMatrix)
		{
			return 0f;
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0001B94B File Offset: 0x00019B4B
		public static Vector2 TransformFPoint(SexyTransform2D theMatrix, Vector2 thePoint)
		{
			return Vector2.Transform(thePoint, theMatrix.mMatrix);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001B95A File Offset: 0x00019B5A
		internal static float WrapFloat(float theNum, int theRepeat)
		{
			if (theRepeat == 1)
			{
				return theNum;
			}
			theNum *= (float)theRepeat;
			return theNum - (float)((int)theNum);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001B96D File Offset: 0x00019B6D
		public static float DegToRad(float theDeg)
		{
			return theDeg * GlobalPIEffect.M_PI / 180f;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001B97C File Offset: 0x00019B7C
		public static uint InterpColor(int theColor1, int theColor2, float thePct)
		{
			uint num = (uint)(thePct * 255f);
			num = ((num < 0U) ? 0U : ((num > 255U) ? 255U : num));
			int num2 = (int)(255U - num);
			long num3 = (long)((((ulong)((uint)(theColor1 & -16777216) >> 24) * (ulong)((long)num2) + (ulong)(((uint)(theColor2 & -16777216) >> 24) * num) << 16) & 0xffffffffff000000) | (((ulong)((uint)(theColor1 & 16711680) >> 16) * (ulong)((long)num2) + (ulong)((long)((theColor2 & 16711680) >> 16) * (long)((ulong)num)) << 8) & 16711680UL) | (ulong)(((long)(((theColor1 & 65280) >> 8) * num2) + (long)((theColor2 & 65280) >> 8) * (long)((ulong)num)) & 65280L) | (ulong)(((long)((theColor1 & 255) * num2) + (long)(theColor2 & 255) * (long)((ulong)num) >> 8) & 255L));
			return (uint)num3;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001BA4C File Offset: 0x00019C4C
		public static bool LineSegmentIntersects(Vector2 aPtA1, Vector2 aPtA2, Vector2 aPtB1, Vector2 aPtB2, ref float thePos, Vector2 theIntersectionPoint)
		{
			double num = (double)((aPtB2.X - aPtB1.Y) * (aPtA2.X - aPtA1.X) - (aPtB2.X - aPtB1.X) * (aPtA2.Y - aPtA1.Y));
			if (num == 0.0)
			{
				return false;
			}
			double num2 = (double)((aPtB2.X - aPtB1.X) * (aPtA1.Y - aPtB1.Y) - (aPtB2.Y - aPtB1.Y) * (aPtA1.X - aPtB1.X)) / num;
			if (num2 < 0.0 || num2 > 1.0)
			{
				return false;
			}
			double num3 = (double)((aPtA2.X - aPtA1.X) * (aPtA1.Y - aPtB1.Y) - (aPtA2.Y - aPtA1.Y) * (aPtA1.X - aPtB1.Y)) / num;
			if (num3 >= 0.0 && num3 <= 1.0)
			{
				if (thePos != 0f)
				{
					thePos = (float)num2;
				}
				theIntersectionPoint = aPtA1 + (aPtA2 - aPtA1) * (float)num2;
				return true;
			}
			return false;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001BB94 File Offset: 0x00019D94
		internal static void GetBestStripSize(int theCount, int theCelWidth, int theCelHeight, ref int theNumCols, ref int theNumRows)
		{
			float num = 100f;
			theNumCols = theCount;
			theNumRows = 1;
			for (int i = 1; i <= theCount; i++)
			{
				int num2 = theCount / i;
				if (num2 * i == theCount)
				{
					float num3 = (float)(theCelWidth * num2) / (float)(theCelHeight * i);
					float num4 = Math.Max(num3, 1f / num3);
					if (num4 + 0.0001f < num)
					{
						theNumRows = i;
						theNumCols = num2;
						num = num4;
					}
				}
			}
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001BBF2 File Offset: 0x00019DF2
		public static float TIME_TO_X(float theTime, float aMinTime, float aMaxTime)
		{
			return (float)((double)((theTime - aMinTime) / (aMaxTime - aMinTime) * (float)(GlobalPIEffect.PI_QUANT_SIZE - 1)) + 0.5);
		}

		// Token: 0x04000555 RID: 1365
		public static float M_PI = 3.14159f;

		// Token: 0x04000556 RID: 1366
		public static int PI_BUFSIZE = 1024;

		// Token: 0x04000557 RID: 1367
		public static int PI_QUANT_SIZE = 256;
	}
}
