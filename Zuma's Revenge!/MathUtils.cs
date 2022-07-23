using System;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x02000121 RID: 289
	public static class MathUtils
	{
		// Token: 0x06000EDD RID: 3805 RVA: 0x0009B716 File Offset: 0x00099916
		public static int SafeRand()
		{
			return MathUtils.mRandomGen.Next();
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0009B722 File Offset: 0x00099922
		public static int Rand(int range)
		{
			return MathUtils.mRandomGen.Next() % range;
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0009B730 File Offset: 0x00099930
		public static void Seed(int seed)
		{
			MathUtils.mRandomGen = new Random(seed);
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0009B73D File Offset: 0x0009993D
		public static int Rand()
		{
			return MathUtils.mRandomGen.Next();
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0009B749 File Offset: 0x00099949
		public static float RadiansToDegrees(float pRads)
		{
			return pRads * 57.29694f;
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0009B752 File Offset: 0x00099952
		public static float DegreesToRadians(float pDegs)
		{
			return pDegs * 0.017452938f;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0009B75B File Offset: 0x0009995B
		public static int Sign(int val)
		{
			if (val >= 0)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0009B764 File Offset: 0x00099964
		public static float Sign(float val)
		{
			if (val >= 0f)
			{
				return 1f;
			}
			return -1f;
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0009B779 File Offset: 0x00099979
		public static bool _eq(float n1, float n2, float tolerance)
		{
			return Math.Abs(n1 - n2) <= tolerance;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0009B789 File Offset: 0x00099989
		public static bool _leq(float n1, float n2, float tolerance)
		{
			return MathUtils._eq(n1, n2, tolerance) || n1 < n2;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0009B79B File Offset: 0x0009999B
		public static bool _geq(float n1, float n2, float tolerance)
		{
			return MathUtils._eq(n1, n2, tolerance) || n1 > n2;
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0009B7AD File Offset: 0x000999AD
		public static bool _eq(float n1, float n2)
		{
			return Math.Abs(n1 - n2) <= float.Epsilon;
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0009B7C1 File Offset: 0x000999C1
		public static bool _leq(float n1, float n2)
		{
			return MathUtils._eq(n1, n2, float.Epsilon) || n1 < n2;
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0009B7D7 File Offset: 0x000999D7
		public static bool _geq(float n1, float n2)
		{
			return MathUtils._eq(n1, n2, float.Epsilon) || n1 > n2;
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0009B7ED File Offset: 0x000999ED
		public static int IntRange(int min_val, int max_val)
		{
			if (min_val == max_val)
			{
				return min_val;
			}
			if (min_val < 0 && max_val < 0)
			{
				return min_val + MathUtils.SafeRand() % (Math.Abs(min_val) - Math.Abs(max_val));
			}
			return min_val + MathUtils.SafeRand() % (max_val - min_val + 1);
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0009B820 File Offset: 0x00099A20
		public static float FloatRange(float min_val, float max_val)
		{
			if (min_val == max_val)
			{
				return min_val;
			}
			if (min_val < 0f && max_val < 0f)
			{
				return min_val + (float)(MathUtils.SafeRand() % (int)((Math.Abs(min_val) - Math.Abs(max_val)) * 100000000f + 1f)) / 100000000f;
			}
			return min_val + (float)(MathUtils.SafeRand() % (int)((max_val - min_val) * 100000000f + 1f)) / 100000000f;
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0009B88C File Offset: 0x00099A8C
		public static void Clamp(ref int value, int min_val, int max_val)
		{
			if (value < min_val)
			{
				value = min_val;
				return;
			}
			if (value > max_val)
			{
				value = max_val;
			}
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0009B89F File Offset: 0x00099A9F
		public static bool IncrementAndClamp(ref float val, float target, float inc)
		{
			val += inc;
			if (inc > 0f && val >= target)
			{
				val = target;
				return true;
			}
			if (inc < 0f && val <= target)
			{
				val = target;
				return true;
			}
			return false;
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0009B8CC File Offset: 0x00099ACC
		public static int GetClosestPowerOf2Above(int theNum)
		{
			int i;
			for (i = 1; i < theNum; i <<= 1)
			{
			}
			return i;
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x0009B8E8 File Offset: 0x00099AE8
		public static bool IsPowerOf2(int theNum)
		{
			int num = 0;
			while (theNum > 0)
			{
				num += theNum & 1;
				theNum >>= 1;
			}
			return num == 1;
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x0009B90C File Offset: 0x00099B0C
		public static float Distance(SexyPoint p1, SexyPoint p2, bool sqrt)
		{
			float num = (float)(p2.mX - p1.mX);
			float num2 = (float)(p2.mY - p1.mY);
			float num3 = num * num + num2 * num2;
			if (!sqrt)
			{
				return num3;
			}
			return (float)Math.Sqrt((double)num3);
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0009B94C File Offset: 0x00099B4C
		public static float Distance(SexyPoint p1, SexyPoint p2)
		{
			return MathUtils.Distance(p1, p2, true);
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x0009B958 File Offset: 0x00099B58
		public static float Distance(float p1x, float p1y, float p2x, float p2y, bool sqrt)
		{
			float num = p2x - p1x;
			float num2 = p2y - p1y;
			float num3 = num * num + num2 * num2;
			if (!sqrt)
			{
				return num3;
			}
			return (float)Math.Sqrt((double)num3);
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x0009B983 File Offset: 0x00099B83
		public static float Distance(float p1x, float p1y, float p2x, float p2y)
		{
			return MathUtils.Distance(p1x, p1y, p2x, p2y, true);
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x0009B990 File Offset: 0x00099B90
		public static bool CirclesIntersect(float x1, float y1, float x2, float y2, float total_radius, ref float seperation)
		{
			float num = x1 - x2;
			float num2 = y1 - y2;
			float num3 = num * num + num2 * num2;
			seperation = num3;
			return num3 < total_radius * total_radius;
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x0009B9BC File Offset: 0x00099BBC
		public static bool CirclesIntersect(float x1, float y1, float x2, float y2, float total_radius)
		{
			float num = 0f;
			return MathUtils.CirclesIntersect(x1, y1, x2, y2, total_radius, ref num);
		}

		// Token: 0x04000EA2 RID: 3746
		public const float EPSILON = 1E-06f;

		// Token: 0x04000EA3 RID: 3747
		public const float JL_PI = 3.1415927f;

		// Token: 0x04000EA4 RID: 3748
		private static Random mRandomGen = new Random();
	}
}
