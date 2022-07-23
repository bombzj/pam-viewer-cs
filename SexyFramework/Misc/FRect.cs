using System;
using Microsoft.Xna.Framework;

namespace SexyFramework.Misc
{
	// Token: 0x02000140 RID: 320
	public struct FRect
	{
		// Token: 0x06000AB6 RID: 2742 RVA: 0x0003616B File Offset: 0x0003436B
		public FRect(float theX, float theY, float theWidth, float theHeight)
		{
			this.mX = theX;
			this.mY = theY;
			this.mWidth = theWidth;
			this.mHeight = theHeight;
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x0003618A File Offset: 0x0003438A
		public FRect(Rect theTRect)
		{
			this.mX = (float)theTRect.mX;
			this.mY = (float)theTRect.mY;
			this.mWidth = (float)theTRect.mWidth;
			this.mHeight = (float)theTRect.mHeight;
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x000361C4 File Offset: 0x000343C4
		public override bool Equals(object obj)
		{
			if (obj != null && obj is FRect)
			{
				FRect frect = (FRect)obj;
				return frect.mX == this.mX && frect.mY == this.mY && frect.mWidth == this.mWidth && frect.mHeight == this.mHeight;
			}
			return false;
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x00036223 File Offset: 0x00034423
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x00036238 File Offset: 0x00034438
		public bool Intersects(FRect theTRect)
		{
			return theTRect.mX + theTRect.mWidth > this.mX && theTRect.mY + theTRect.mHeight > this.mY && theTRect.mX < this.mX + this.mWidth && theTRect.mY < this.mY + this.mHeight;
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x000362A4 File Offset: 0x000344A4
		public FRect Intersection(FRect theTRect)
		{
			float num = Math.Max(this.mX, theTRect.mX);
			float num2 = Math.Min(this.mX + this.mWidth, theTRect.mX + theTRect.mWidth);
			float num3 = Math.Max(this.mY, theTRect.mY);
			float num4 = Math.Min(this.mY + this.mHeight, theTRect.mY + theTRect.mHeight);
			if (num2 - num < 0f || num4 - num3 < 0f)
			{
				return new FRect(0f, 0f, 0f, 0f);
			}
			return new FRect(num, num3, num2 - num, num4 - num3);
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00036358 File Offset: 0x00034558
		public FRect Union(FRect theTRect)
		{
			float num = Math.Min(this.mX, theTRect.mX);
			float num2 = Math.Max(this.mX + this.mWidth, theTRect.mX + theTRect.mWidth);
			float num3 = Math.Min(this.mY, theTRect.mY);
			float num4 = Math.Max(this.mY + this.mHeight, theTRect.mY + theTRect.mHeight);
			return new FRect(num, num3, num2 - num, num4 - num3);
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x000363DC File Offset: 0x000345DC
		public bool Contains(float theX, float theY)
		{
			return theX >= this.mX && theX < this.mX + this.mWidth && theY >= this.mY && theY < this.mY + this.mHeight;
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x00036414 File Offset: 0x00034614
		public bool Contains(Vector2 thePoint)
		{
			return thePoint.X >= this.mX && thePoint.X < this.mX + this.mWidth && thePoint.Y >= this.mY && thePoint.Y < this.mY + this.mHeight;
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x0003646D File Offset: 0x0003466D
		public void Offset(float theX, float theY)
		{
			this.mX += theX;
			this.mY += theY;
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0003648B File Offset: 0x0003468B
		public void Offset(Vector2 thePoint)
		{
			this.mX += thePoint.X;
			this.mY += thePoint.Y;
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x000364B8 File Offset: 0x000346B8
		public FRect Inflate(float theX, float theY)
		{
			this.mX -= theX;
			this.mWidth += theX * 2f;
			this.mY -= theY;
			this.mHeight += theY * 2f;
			return this;
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00036510 File Offset: 0x00034710
		public void Scale(double theScaleX, double theScaleY)
		{
			this.mX = (float)((double)this.mX * theScaleX);
			this.mY = (float)((double)this.mY * theScaleY);
			this.mWidth = (float)((double)this.mWidth * theScaleX);
			this.mHeight = (float)((double)this.mHeight * theScaleY);
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x0003655D File Offset: 0x0003475D
		public void Scale(double theScaleX, double theScaleY, float theCenterX, float theCenterY)
		{
			this.Offset(-theCenterX, -theCenterY);
			this.Scale(theScaleX, theScaleY);
			this.Offset(theCenterX, theCenterY);
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0003657B File Offset: 0x0003477B
		public static bool operator ==(FRect ImpliedObject, FRect theRect)
		{
			if (ImpliedObject == null)
			{
				return theRect == null;
			}
			return ImpliedObject.Equals(theRect);
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x000365A2 File Offset: 0x000347A2
		public static bool operator !=(FRect ImpliedObject, FRect theRect)
		{
			return !(ImpliedObject == theRect);
		}

		// Token: 0x04000931 RID: 2353
		public float mX;

		// Token: 0x04000932 RID: 2354
		public float mY;

		// Token: 0x04000933 RID: 2355
		public float mWidth;

		// Token: 0x04000934 RID: 2356
		public float mHeight;

		// Token: 0x04000935 RID: 2357
		public static FRect ZeroRect = new FRect(0f, 0f, 0f, 0f);
	}
}
