using System;
using SexyFramework;
using SexyFramework.GraphicsLib;

namespace JeffLib
{
	// Token: 0x0200010B RID: 267
	public class FColor
	{
		// Token: 0x06000825 RID: 2085 RVA: 0x000298C0 File Offset: 0x00027AC0
		public FColor()
		{
			this.mRed = (this.mGreen = (this.mBlue = 0f));
			this.mAlpha = 255f;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x000298FB File Offset: 0x00027AFB
		public FColor(float r, float g, float b)
		{
			this.mRed = r;
			this.mGreen = g;
			this.mBlue = b;
			this.mAlpha = 255f;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00029923 File Offset: 0x00027B23
		public FColor(float r, float g, float b, float a)
		{
			this.mRed = r;
			this.mGreen = g;
			this.mBlue = b;
			this.mAlpha = a;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00029948 File Offset: 0x00027B48
		public SexyColor ToColor()
		{
			return new SexyColor((int)this.mRed, (int)this.mGreen, (int)this.mBlue, (int)this.mAlpha);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0002996B File Offset: 0x00027B6B
		public FColor(SexyColor rhs)
		{
			this.CopyFrom(rhs);
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0002997A File Offset: 0x00027B7A
		private void CopyFrom(SexyColor rhs)
		{
			this.mRed = (float)rhs.mRed;
			this.mGreen = (float)rhs.mGreen;
			this.mBlue = (float)rhs.mBlue;
			this.mAlpha = (float)rhs.mAlpha;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x000299B4 File Offset: 0x00027BB4
		public static implicit operator SexyColor(FColor ImpliedObject)
		{
			return new SexyColor((int)ImpliedObject.mRed, (int)ImpliedObject.mGreen, (int)ImpliedObject.mBlue, (int)ImpliedObject.mAlpha);
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x000299D7 File Offset: 0x00027BD7
		public static bool operator ==(FColor a, FColor b)
		{
			if (a == null)
			{
				return b == null;
			}
			return a.Equals(b);
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x000299E8 File Offset: 0x00027BE8
		public static bool operator !=(FColor a, FColor b)
		{
			return !(a == b);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x000299F4 File Offset: 0x00027BF4
		public override bool Equals(object obj)
		{
			if (obj != null && obj is FColor)
			{
				FColor fcolor = (FColor)obj;
				return SexyFramework.Common._eq(this.mAlpha, fcolor.mAlpha) && SexyFramework.Common._eq(this.mGreen, fcolor.mGreen) && SexyFramework.Common._eq(this.mBlue, fcolor.mBlue) && SexyFramework.Common._eq(this.mRed, fcolor.mRed);
			}
			return false;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00029A61 File Offset: 0x00027C61
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000781 RID: 1921
		public float mRed;

		// Token: 0x04000782 RID: 1922
		public float mGreen;

		// Token: 0x04000783 RID: 1923
		public float mBlue;

		// Token: 0x04000784 RID: 1924
		public float mAlpha;
	}
}
