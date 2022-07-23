using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.PIL
{
	// Token: 0x02000154 RID: 340
	public class ColorKey
	{
		// Token: 0x06000BCF RID: 3023 RVA: 0x000393D4 File Offset: 0x000375D4
		public ColorKey()
		{
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x000393DC File Offset: 0x000375DC
		public ColorKey(SexyColor c)
		{
			this.mColor = c;
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x000393EB File Offset: 0x000375EB
		public virtual void Dispose()
		{
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x000393F0 File Offset: 0x000375F0
		public SexyColor GetInterpolatedColor(ColorKey next_color, float pct)
		{
			SexyColor result = new SexyColor(this.mColor);
			result.mRed += (int)((float)(next_color.mColor.mRed - this.mColor.mRed) * pct);
			result.mGreen += (int)((float)(next_color.mColor.mGreen - this.mColor.mGreen) * pct);
			result.mBlue += (int)((float)(next_color.mColor.mBlue - this.mColor.mBlue) * pct);
			result.mAlpha += (int)((float)(next_color.mColor.mAlpha - this.mColor.mAlpha) * pct);
			result.mRed = Math.Max(Math.Min(255, result.mRed), 0);
			result.mGreen = Math.Max(Math.Min(255, result.mGreen), 0);
			result.mBlue = Math.Max(Math.Min(255, result.mBlue), 0);
			result.mAlpha = Math.Max(Math.Min(255, result.mAlpha), 0);
			return result;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00039527 File Offset: 0x00037727
		public SexyColor GetColor()
		{
			return this.mColor;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0003952F File Offset: 0x0003772F
		public void SetColor(SexyColor c)
		{
			this.mColor = c;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00039538 File Offset: 0x00037738
		public void Serialize(SexyBuffer b)
		{
			b.WriteLong((long)this.mColor.ToInt());
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x0003954C File Offset: 0x0003774C
		public void Deserialize(SexyBuffer b)
		{
			this.mColor = new SexyColor((int)b.ReadLong());
		}

		// Token: 0x04000968 RID: 2408
		protected SexyColor mColor;
	}
}
