using System;
using Microsoft.Xna.Framework;

namespace SexyFramework.Misc
{
	// Token: 0x0200013F RID: 319
	public class DRect
	{
		// Token: 0x06000AA5 RID: 2725 RVA: 0x00035D01 File Offset: 0x00033F01
		public DRect(double theX, double theY, double theWidth, double theHeight)
		{
			this.mX = theX;
			this.mY = theY;
			this.mWidth = theWidth;
			this.mHeight = theHeight;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x00035D26 File Offset: 0x00033F26
		public DRect(DRect theTRect)
		{
			this.mX = theTRect.mX;
			this.mY = theTRect.mY;
			this.mWidth = theTRect.mWidth;
			this.mHeight = theTRect.mHeight;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00035D60 File Offset: 0x00033F60
		public DRect()
		{
			this.mX = 0.0;
			this.mY = 0.0;
			this.mWidth = 0.0;
			this.mHeight = 0.0;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00035DB0 File Offset: 0x00033FB0
		public override bool Equals(object obj)
		{
			if (obj != null && obj is DRect)
			{
				DRect drect = (DRect)obj;
				return drect.mX == this.mX && drect.mY == this.mY && drect.mWidth == this.mWidth && drect.mHeight == this.mHeight;
			}
			return false;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00035E0B File Offset: 0x0003400B
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x00035E14 File Offset: 0x00034014
		public bool Intersects(DRect theTRect)
		{
			return theTRect.mX + theTRect.mWidth > this.mX && theTRect.mY + theTRect.mHeight > this.mY && theTRect.mX < this.mX + this.mWidth && theTRect.mY < this.mY + this.mHeight;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00035E78 File Offset: 0x00034078
		public DRect Intersection(DRect theTRect)
		{
			double num = Math.Max(this.mX, theTRect.mX);
			double num2 = Math.Min(this.mX + this.mWidth, theTRect.mX + theTRect.mWidth);
			double num3 = Math.Max(this.mY, theTRect.mY);
			double num4 = Math.Min(this.mY + this.mHeight, theTRect.mY + theTRect.mHeight);
			if (num2 - num < 0.0 || num4 - num3 < 0.0)
			{
				return new DRect(0.0, 0.0, 0.0, 0.0);
			}
			return new DRect(num, num3, num2 - num, num4 - num3);
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x00035F3C File Offset: 0x0003413C
		public DRect Union(DRect theTRect)
		{
			double num = Math.Min(this.mX, theTRect.mX);
			double num2 = Math.Max(this.mX + this.mWidth, theTRect.mX + theTRect.mWidth);
			double num3 = Math.Min(this.mY, theTRect.mY);
			double num4 = Math.Max(this.mY + this.mHeight, theTRect.mY + theTRect.mHeight);
			return new DRect(num, num3, num2 - num, num4 - num3);
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00035FBA File Offset: 0x000341BA
		public bool Contains(double theX, double theY)
		{
			return theX >= this.mX && theX < this.mX + this.mWidth && theY >= this.mY && theY < this.mY + this.mHeight;
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x00035FF0 File Offset: 0x000341F0
		public bool Contains(Vector2 thePoint)
		{
			return (double)thePoint.X >= this.mX && (double)thePoint.X < this.mX + this.mWidth && (double)thePoint.Y >= this.mY && (double)thePoint.Y < this.mY + this.mHeight;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0003604D File Offset: 0x0003424D
		public void Offset(double theX, double theY)
		{
			this.mX += theX;
			this.mY += theY;
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0003606B File Offset: 0x0003426B
		public void Offset(Vector2 thePoint)
		{
			this.mX += (double)thePoint.X;
			this.mY += (double)thePoint.Y;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00036098 File Offset: 0x00034298
		public DRect Inflate(double theX, double theY)
		{
			this.mX -= theX;
			this.mWidth += theX * 2.0;
			this.mY -= theY;
			this.mHeight += theY * 2.0;
			return this;
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x000360F2 File Offset: 0x000342F2
		public void Scale(double theScaleX, double theScaleY)
		{
			this.mX *= theScaleX;
			this.mY *= theScaleY;
			this.mWidth *= theScaleX;
			this.mHeight *= theScaleY;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00036130 File Offset: 0x00034330
		public void Scale(double theScaleX, double theScaleY, double theCenterX, double theCenterY)
		{
			this.Offset(-theCenterX, -theCenterY);
			this.Scale(theScaleX, theScaleY);
			this.Offset(theCenterX, theCenterY);
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0003614E File Offset: 0x0003434E
		public static bool operator ==(DRect ImpliedObject, DRect theRect)
		{
			if (ImpliedObject == null)
			{
				return theRect == null;
			}
			return ImpliedObject.Equals(theRect);
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0003615F File Offset: 0x0003435F
		public static bool operator !=(DRect ImpliedObject, DRect theRect)
		{
			return !(ImpliedObject == theRect);
		}

		// Token: 0x0400092D RID: 2349
		public double mX;

		// Token: 0x0400092E RID: 2350
		public double mY;

		// Token: 0x0400092F RID: 2351
		public double mWidth;

		// Token: 0x04000930 RID: 2352
		public double mHeight;
	}
}
