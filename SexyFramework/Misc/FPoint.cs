using System;

namespace SexyFramework.Misc
{
	// Token: 0x0200013B RID: 315
	public class FPoint
	{
		// Token: 0x06000A5C RID: 2652 RVA: 0x00035249 File Offset: 0x00033449
		public FPoint(float theX, float theY)
		{
			this.mX = theX;
			this.mY = theY;
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0003525F File Offset: 0x0003345F
		public FPoint(FPoint theTPoint)
		{
			this.mX = theTPoint.mX;
			this.mY = theTPoint.mY;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0003527F File Offset: 0x0003347F
		public FPoint()
		{
			this.mX = 0f;
			this.mY = 0f;
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x0003529D File Offset: 0x0003349D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x000352A8 File Offset: 0x000334A8
		public override bool Equals(object obj)
		{
			if (obj != null && obj is FPoint)
			{
				SexyPoint point = (SexyPoint)obj;
				return (float)point.mX == this.mX && (float)point.mY == this.mY;
			}
			return false;
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x000352E9 File Offset: 0x000334E9
		public static bool operator ==(FPoint ImpliedObject, FPoint p)
		{
			if (ImpliedObject == null)
			{
				return p == null;
			}
			return ImpliedObject.Equals(p);
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x000352FA File Offset: 0x000334FA
		public static bool operator !=(FPoint ImpliedObject, FPoint p)
		{
			return !(ImpliedObject == p);
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00035306 File Offset: 0x00033506
		public float Magnitude()
		{
			return (float)Math.Sqrt((double)(this.mX * this.mX + this.mY * this.mY));
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x0003532A File Offset: 0x0003352A
		public static FPoint operator +(FPoint ImpliedObject, FPoint p)
		{
			return new FPoint(ImpliedObject.mX + p.mX, ImpliedObject.mY + p.mY);
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x0003534B File Offset: 0x0003354B
		public static FPoint operator -(FPoint ImpliedObject, FPoint p)
		{
			return new FPoint(ImpliedObject.mX - p.mX, ImpliedObject.mY - p.mY);
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0003536C File Offset: 0x0003356C
		public static FPoint operator *(FPoint ImpliedObject, FPoint p)
		{
			return new FPoint(ImpliedObject.mX * p.mX, ImpliedObject.mY * p.mY);
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0003538D File Offset: 0x0003358D
		public static FPoint operator /(FPoint ImpliedObject, FPoint p)
		{
			return new FPoint(ImpliedObject.mX / p.mX, ImpliedObject.mY / p.mY);
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x000353AE File Offset: 0x000335AE
		public static FPoint operator *(FPoint ImpliedObject, int s)
		{
			return new FPoint(ImpliedObject.mX * (float)s, ImpliedObject.mY * (float)s);
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x000353C9 File Offset: 0x000335C9
		public static FPoint operator /(FPoint ImpliedObject, float s)
		{
			return new FPoint(ImpliedObject.mX / s, ImpliedObject.mY / s);
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x000353E2 File Offset: 0x000335E2
		public static FPoint operator *(FPoint ImpliedObject, double s)
		{
			return new FPoint((float)((double)ImpliedObject.mX * s), (float)((double)ImpliedObject.mY * s));
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x000353FD File Offset: 0x000335FD
		public static FPoint operator /(FPoint ImpliedObject, double s)
		{
			return new FPoint((float)((double)ImpliedObject.mX / s), (float)((double)ImpliedObject.mY / s));
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00035418 File Offset: 0x00033618
		public static FPoint operator *(FPoint ImpliedObject, float s)
		{
			return new FPoint(ImpliedObject.mX * s, ImpliedObject.mY * s);
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00035431 File Offset: 0x00033631
		public static FPoint operator /(FPoint ImpliedObject, int s)
		{
			return new FPoint(ImpliedObject.mX / (float)s, ImpliedObject.mY / (float)s);
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x0003544C File Offset: 0x0003364C
		internal void SetValue(float p, float p_2)
		{
			this.mX = p;
			this.mY = p_2;
		}

		// Token: 0x04000921 RID: 2337
		public float mX;

		// Token: 0x04000922 RID: 2338
		public float mY;
	}
}
