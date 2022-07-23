using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x02000082 RID: 130
	public struct SexyColor
	{
		// Token: 0x06000487 RID: 1159 RVA: 0x0000D5D0 File Offset: 0x0000B7D0
		public SexyColor(SexyColor color)
		{
			this.mRed = color.mRed;
			this.mGreen = color.mGreen;
			this.mBlue = color.mBlue;
			this.mAlpha = color.mAlpha;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0000D608 File Offset: 0x0000B808
		public SexyColor(int theColor)
		{
			this.mAlpha = (theColor >> 24) & 255;
			this.mRed = (theColor >> 16) & 255;
			this.mGreen = (theColor >> 8) & 255;
			this.mBlue = theColor & 255;
			if (this.mAlpha == 0)
			{
				this.mAlpha = 255;
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0000D664 File Offset: 0x0000B864
		public SexyColor(int theColor, int theAlpha)
		{
			this.mRed = (theColor >> 16) & 255;
			this.mGreen = (theColor >> 8) & 255;
			this.mBlue = theColor & 255;
			this.mAlpha = theAlpha;
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0000D699 File Offset: 0x0000B899
		public SexyColor(int theRed, int theGreen, int theBlue)
		{
			this.mRed = theRed;
			this.mGreen = theGreen;
			this.mBlue = theBlue;
			this.mAlpha = 255;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0000D6BB File Offset: 0x0000B8BB
		public SexyColor(int theRed, int theGreen, int theBlue, int theAlpha)
		{
			this.mRed = theRed;
			this.mGreen = theGreen;
			this.mBlue = theBlue;
			this.mAlpha = theAlpha;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0000D6DA File Offset: 0x0000B8DA
		public SexyColor(SexyRGBA theColor)
		{
			this.mRed = (int)theColor.r;
			this.mGreen = (int)theColor.g;
			this.mBlue = (int)theColor.b;
			this.mAlpha = (int)theColor.a;
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000D70C File Offset: 0x0000B90C
		public SexyColor(int[] theElements)
		{
			this.mRed = theElements[0];
			this.mGreen = theElements[1];
			this.mBlue = theElements[2];
			this.mAlpha = 255;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0000D734 File Offset: 0x0000B934
		public override string ToString()
		{
			return string.Concat(new object[] { "(", this.mRed, ",", this.mGreen, ",", this.mBlue, ",", this.mAlpha, ")" });
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000D7AF File Offset: 0x0000B9AF
		public SexyColor Clone()
		{
			return new SexyColor(this);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0000D7BC File Offset: 0x0000B9BC
		public override bool Equals(object obj)
		{
			if (obj != null && obj is SexyColor)
			{
				SexyColor color = (SexyColor)obj;
				return this.mRed == color.mRed && this.mBlue == color.mBlue && this.mGreen == color.mGreen && this.mAlpha == color.mAlpha;
			}
			return false;
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000D81B File Offset: 0x0000BA1B
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000D82D File Offset: 0x0000BA2D
		public int ToInt()
		{
			return (this.mAlpha << 24) | (this.mRed << 16) | (this.mGreen << 8) | this.mBlue;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0000D852 File Offset: 0x0000BA52
		public SexyRGBA ToRGBA()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0000D859 File Offset: 0x0000BA59
		public static bool operator ==(SexyColor theColor1, SexyColor theColor2)
		{
			if (theColor1 == null)
			{
				return theColor2 == null;
			}
			return theColor1.Equals(theColor2);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0000D880 File Offset: 0x0000BA80
		public static bool operator !=(SexyColor theColor1, SexyColor theColor2)
		{
			return !(theColor1 == theColor2);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0000D88C File Offset: 0x0000BA8C
		public static SexyColor operator *(SexyColor theColor1, SexyColor theColor2)
		{
			return new SexyColor(theColor1.mRed * theColor2.mRed / 255, theColor1.mGreen * theColor2.mGreen / 255, theColor1.mBlue * theColor2.mBlue / 255, theColor1.mAlpha * theColor2.mAlpha / 255);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0000D8F2 File Offset: 0x0000BAF2
		public static SexyColor operator *(SexyColor theColor1, float theAlphaPct)
		{
			return new SexyColor(theColor1.mRed, theColor1.mGreen, theColor1.mBlue, (int)((float)theColor1.mAlpha * theAlphaPct) / 255);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0000D91F File Offset: 0x0000BB1F
		public void SetColor(int p, int p_2, int p_3, int p_4)
		{
			this.mRed = p;
			this.mGreen = p_2;
			this.mBlue = p_3;
			this.mAlpha = p_4;
		}

		// Token: 0x04000293 RID: 659
		public int mRed;

		// Token: 0x04000294 RID: 660
		public int mGreen;

		// Token: 0x04000295 RID: 661
		public int mBlue;

		// Token: 0x04000296 RID: 662
		public int mAlpha;

		// Token: 0x04000297 RID: 663
		public static SexyColor Zero = new SexyColor(0, 0, 0, 0);

		// Token: 0x04000298 RID: 664
		public static readonly SexyColor Black = new SexyColor(0, 0, 0);

		// Token: 0x04000299 RID: 665
		public static readonly SexyColor White = new SexyColor(255, 255, 255);

		// Token: 0x0400029A RID: 666
		public static readonly SexyColor Red = new SexyColor(255, 0, 0);

		// Token: 0x0400029B RID: 667
		public static readonly SexyColor Green = new SexyColor(0, 255, 0);

		// Token: 0x0400029C RID: 668
		public static readonly SexyColor Blue = new SexyColor(0, 0, 255);

		// Token: 0x0400029D RID: 669
		public static readonly SexyColor Yellow = new SexyColor(255, 255, 0);
	}
}
