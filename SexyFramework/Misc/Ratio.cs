using System;

namespace SexyFramework.Misc
{
	// Token: 0x0200013D RID: 317
	public class Ratio
	{
		// Token: 0x06000A81 RID: 2689 RVA: 0x00035661 File Offset: 0x00033861
		public Ratio()
		{
			this.mNumerator = 0;
			this.mDenominator = 0;
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00035677 File Offset: 0x00033877
		public Ratio(int theNumerator, int theDenominator)
		{
			this.Set(theNumerator, theDenominator);
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00035688 File Offset: 0x00033888
		public override bool Equals(object obj)
		{
			if (obj != null && obj is Ratio)
			{
				Ratio ratio = (Ratio)obj;
				return this.mNumerator == ratio.mNumerator && this.mDenominator == ratio.mDenominator;
			}
			return false;
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x000356C7 File Offset: 0x000338C7
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x000356D0 File Offset: 0x000338D0
		public void Set(int theNumerator, int theDenominator)
		{
			int num = theNumerator;
			int num2 = theDenominator;
			while (num2 != 0)
			{
				int num3 = num2;
				num2 = num % num2;
				num = num3;
			}
			this.mNumerator = theNumerator / num;
			this.mDenominator = theDenominator / num;
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00035700 File Offset: 0x00033900
		public static bool operator ==(Ratio ImpliedObject, Ratio theRatio)
		{
			if (ImpliedObject == null)
			{
				return theRatio == null;
			}
			return ImpliedObject.Equals(theRatio);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00035711 File Offset: 0x00033911
		public static bool operator !=(Ratio ImpliedObject, Ratio theRatio)
		{
			return !(ImpliedObject == theRatio);
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0003571D File Offset: 0x0003391D
		public static bool operator <(Ratio ImpliedObject, Ratio theRatio)
		{
			return ImpliedObject.mNumerator * theRatio.mDenominator / ImpliedObject.mDenominator < theRatio.mNumerator || ImpliedObject.mNumerator < theRatio.mNumerator * ImpliedObject.mDenominator / theRatio.mDenominator;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00035759 File Offset: 0x00033959
		public static bool operator >(Ratio ImpliedObject, Ratio theRatio)
		{
			return ImpliedObject.mNumerator * theRatio.mDenominator / ImpliedObject.mDenominator > theRatio.mNumerator || ImpliedObject.mNumerator > theRatio.mNumerator * ImpliedObject.mDenominator / theRatio.mDenominator;
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00035795 File Offset: 0x00033995
		public static int operator *(Ratio ImpliedObject, int t)
		{
			return t * ImpliedObject.mNumerator / ImpliedObject.mDenominator;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x000357A6 File Offset: 0x000339A6
		public static int operator *(Ratio ImpliedObject, float t)
		{
			return (int)(t * (float)ImpliedObject.mNumerator / (float)ImpliedObject.mDenominator);
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x000357BA File Offset: 0x000339BA
		public static int operator *(Ratio ImpliedObject, double t)
		{
			return (int)(t * (double)ImpliedObject.mNumerator / (double)ImpliedObject.mDenominator);
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x000357CE File Offset: 0x000339CE
		public static int operator /(Ratio ImpliedObject, int t)
		{
			return t * ImpliedObject.mDenominator / ImpliedObject.mNumerator;
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x000357DF File Offset: 0x000339DF
		public static int operator /(Ratio ImpliedObject, float t)
		{
			return (int)(t * (float)ImpliedObject.mDenominator / (float)ImpliedObject.mNumerator);
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x000357F3 File Offset: 0x000339F3
		public static int operator /(Ratio ImpliedObject, double t)
		{
			return (int)(t * (double)ImpliedObject.mDenominator / (double)ImpliedObject.mNumerator);
		}

		// Token: 0x04000925 RID: 2341
		public int mNumerator;

		// Token: 0x04000926 RID: 2342
		public int mDenominator;
	}
}
