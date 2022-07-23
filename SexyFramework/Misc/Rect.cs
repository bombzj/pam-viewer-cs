using System;
using Microsoft.Xna.Framework;

namespace SexyFramework.Misc
{
	// Token: 0x0200013E RID: 318
	public struct Rect
	{
		// Token: 0x06000A90 RID: 2704 RVA: 0x00035807 File Offset: 0x00033A07
		public Rect(int theX, int theY, int theWidth, int theHeight)
		{
			this.mX = theX;
			this.mY = theY;
			this.mWidth = theWidth;
			this.mHeight = theHeight;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x00035826 File Offset: 0x00033A26
		public Rect(Rect theTRect)
		{
			this.mX = theTRect.mX;
			this.mY = theTRect.mY;
			this.mWidth = theTRect.mWidth;
			this.mHeight = theTRect.mHeight;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0003585C File Offset: 0x00033A5C
		public void SetValue(int theX, int theY, int theWidth, int theHeight)
		{
			this.mX = theX;
			this.mY = theY;
			this.mWidth = theWidth;
			this.mHeight = theHeight;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0003587C File Offset: 0x00033A7C
		public override bool Equals(object obj)
		{
			if (obj != null && obj is Rect)
			{
				Rect rect = (Rect)obj;
				return rect.mX == this.mX && rect.mY == this.mY && rect.mWidth == this.mWidth && rect.mHeight == this.mHeight;
			}
			return false;
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x000358DB File Offset: 0x00033ADB
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x000358ED File Offset: 0x00033AED
		public void setValue(ref int x, ref int y, ref int width, ref int height)
		{
			this.mX = x;
			this.mY = y;
			this.mWidth = width;
			this.mHeight = height;
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x00035910 File Offset: 0x00033B10
		public void setValue(int x, int y, int width, int height)
		{
			this.mX = x;
			this.mY = y;
			this.mWidth = width;
			this.mHeight = height;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00035930 File Offset: 0x00033B30
		public bool Intersects(Rect theTRect)
		{
			return theTRect.mX + theTRect.mWidth > this.mX && theTRect.mY + theTRect.mHeight > this.mY && theTRect.mX < this.mX + this.mWidth && theTRect.mY < this.mY + this.mHeight;
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x00035999 File Offset: 0x00033B99
		public bool Intersects(int x, int y, int w, int h)
		{
			return x + w > this.mX && y + h > this.mY && x < this.mX + this.mWidth && y < this.mY + this.mHeight;
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x000359D4 File Offset: 0x00033BD4
		public Rect Intersection(Rect theTRect)
		{
			int num = Math.Max(this.mX, theTRect.mX);
			int num2 = Math.Min(this.mX + this.mWidth, theTRect.mX + theTRect.mWidth);
			int num3 = Math.Max(this.mY, theTRect.mY);
			int num4 = Math.Min(this.mY + this.mHeight, theTRect.mY + theTRect.mHeight);
			if (num2 - num < 0 || num4 - num3 < 0)
			{
				return new Rect(0, 0, 0, 0);
			}
			return new Rect(num, num3, num2 - num, num4 - num3);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00035A70 File Offset: 0x00033C70
		public Rect Union(Rect theTRect)
		{
			int num = Math.Min(this.mX, theTRect.mX);
			int num2 = Math.Max(this.mX + this.mWidth, theTRect.mX + theTRect.mWidth);
			int num3 = Math.Min(this.mY, theTRect.mY);
			int num4 = Math.Max(this.mY + this.mHeight, theTRect.mY + theTRect.mHeight);
			return new Rect(num, num3, num2 - num, num4 - num3);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00035AF4 File Offset: 0x00033CF4
		public bool Contains(int theX, int theY)
		{
			return theX >= this.mX && theX < this.mX + this.mWidth && theY >= this.mY && theY < this.mY + this.mHeight;
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x00035B2C File Offset: 0x00033D2C
		public bool Contains(SexyPoint thePoint)
		{
			return thePoint.mX >= this.mX && thePoint.mX < this.mX + this.mWidth && thePoint.mY >= this.mY && thePoint.mY < this.mY + this.mHeight;
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x00035B81 File Offset: 0x00033D81
		public void Offset(int theX, int theY)
		{
			this.mX += theX;
			this.mY += theY;
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x00035B9F File Offset: 0x00033D9F
		public void Offset(Vector2 thePoint)
		{
			this.mX += (int)thePoint.X;
			this.mY += (int)thePoint.Y;
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00035BCC File Offset: 0x00033DCC
		public Rect Inflate(int theX, int theY)
		{
			this.mX -= theX;
			this.mWidth += theX * 2;
			this.mY -= theY;
			this.mHeight += theY * 2;
			return this;
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00035C1C File Offset: 0x00033E1C
		public void Scale(double theScaleX, double theScaleY)
		{
			this.mX = (int)((double)this.mX * theScaleX);
			this.mY = (int)((double)this.mY * theScaleY);
			this.mWidth = (int)((double)this.mWidth * theScaleX);
			this.mHeight = (int)((double)this.mHeight * theScaleY);
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x00035C69 File Offset: 0x00033E69
		public void Scale(double theScaleX, double theScaleY, int theCenterX, int theCenterY)
		{
			this.Offset(-theCenterX, -theCenterY);
			this.Scale(theScaleX, theScaleY);
			this.Offset(theCenterX, theCenterY);
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00035C88 File Offset: 0x00033E88
		public static bool operator ==(Rect ImpliedObject, Rect theRect)
		{
			return ImpliedObject.mX == theRect.mX && ImpliedObject.mY == theRect.mY && ImpliedObject.mWidth == theRect.mWidth && ImpliedObject.mHeight == theRect.mHeight;
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x00035CD7 File Offset: 0x00033ED7
		public static bool operator !=(Rect ImpliedObject, Rect theRect)
		{
			return !(ImpliedObject == theRect);
		}

		// Token: 0x04000927 RID: 2343
		public static Rect ZERO_RECT = new Rect(0, 0, 0, 0);

		// Token: 0x04000928 RID: 2344
		public static Rect INVALIDATE_RECT = new Rect(-1, -1, -1, -1);

		// Token: 0x04000929 RID: 2345
		public int mX;

		// Token: 0x0400092A RID: 2346
		public int mY;

		// Token: 0x0400092B RID: 2347
		public int mWidth;

		// Token: 0x0400092C RID: 2348
		public int mHeight;
	}
}
