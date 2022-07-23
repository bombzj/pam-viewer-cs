using System;

namespace SexyFramework.Misc
{
	// Token: 0x0200013C RID: 316
	public class DPoint
	{
		// Token: 0x06000A6F RID: 2671 RVA: 0x0003545C File Offset: 0x0003365C
		public DPoint(double theX, double theY)
		{
			this.mX = theX;
			this.mY = theY;
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00035472 File Offset: 0x00033672
		public DPoint(DPoint theTPoint)
		{
			this.mX = theTPoint.mX;
			this.mY = theTPoint.mY;
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00035492 File Offset: 0x00033692
		public DPoint()
		{
			this.mX = 0.0;
			this.mY = 0.0;
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x000354B8 File Offset: 0x000336B8
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x000354C0 File Offset: 0x000336C0
		public override bool Equals(object obj)
		{
			if (obj != null && obj is DPoint)
			{
				DPoint dpoint = (DPoint)obj;
				return dpoint.mX == this.mX && dpoint.mY == this.mY;
			}
			return false;
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x000354FF File Offset: 0x000336FF
		public static bool operator ==(DPoint ImpliedObject, DPoint p)
		{
			if (ImpliedObject == null)
			{
				return p == null;
			}
			return ImpliedObject.Equals(p);
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00035510 File Offset: 0x00033710
		public static bool operator !=(DPoint ImpliedObject, DPoint p)
		{
			return !(ImpliedObject == p);
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0003551C File Offset: 0x0003371C
		public double Magnitude()
		{
			return Math.Sqrt(this.mX * this.mX + this.mY * this.mY);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0003553F File Offset: 0x0003373F
		public static DPoint operator +(DPoint ImpliedObject, DPoint p)
		{
			return new DPoint(ImpliedObject.mX + p.mX, ImpliedObject.mY + p.mY);
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00035560 File Offset: 0x00033760
		public static DPoint operator -(DPoint ImpliedObject, DPoint p)
		{
			return new DPoint(ImpliedObject.mX - p.mX, ImpliedObject.mY - p.mY);
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x00035581 File Offset: 0x00033781
		public static DPoint operator *(DPoint ImpliedObject, DPoint p)
		{
			return new DPoint(ImpliedObject.mX * p.mX, ImpliedObject.mY * p.mY);
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x000355A2 File Offset: 0x000337A2
		public static DPoint operator /(DPoint ImpliedObject, DPoint p)
		{
			return new DPoint(ImpliedObject.mX / p.mX, ImpliedObject.mY / p.mY);
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x000355C3 File Offset: 0x000337C3
		public static DPoint operator *(DPoint ImpliedObject, int s)
		{
			return new DPoint(ImpliedObject.mX * (double)s, ImpliedObject.mY * (double)s);
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x000355DE File Offset: 0x000337DE
		public static DPoint operator /(DPoint ImpliedObject, float s)
		{
			return new DPoint(ImpliedObject.mX / (double)s, ImpliedObject.mY / (double)s);
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x000355F9 File Offset: 0x000337F9
		public static DPoint operator *(DPoint ImpliedObject, double s)
		{
			return new DPoint(ImpliedObject.mX * s, ImpliedObject.mY * s);
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00035612 File Offset: 0x00033812
		public static DPoint operator /(DPoint ImpliedObject, double s)
		{
			return new DPoint(ImpliedObject.mX / s, ImpliedObject.mY / s);
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0003562B File Offset: 0x0003382B
		public static DPoint operator *(DPoint ImpliedObject, float s)
		{
			return new DPoint(ImpliedObject.mX * (double)s, ImpliedObject.mY * (double)s);
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00035646 File Offset: 0x00033846
		public static DPoint operator /(DPoint ImpliedObject, int s)
		{
			return new DPoint(ImpliedObject.mX / (double)s, ImpliedObject.mY / (double)s);
		}

		// Token: 0x04000923 RID: 2339
		public double mX;

		// Token: 0x04000924 RID: 2340
		public double mY;
	}
}
