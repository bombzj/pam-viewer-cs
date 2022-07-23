using System;

namespace JeffLib
{
	// Token: 0x0200010C RID: 268
	public class CommonColorFader
	{
		// Token: 0x06000830 RID: 2096 RVA: 0x00029A6C File Offset: 0x00027C6C
		public CommonColorFader()
		{
			this.mForward = true;
			this.mEnabled = true;
			this.mDuration = -1;
			this.mRedChange = (this.mGreenChange = (this.mBlueChange = (this.mAlphaChange = 0f)));
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00029ADC File Offset: 0x00027CDC
		public bool Update()
		{
			if (!this.mEnabled)
			{
				return false;
			}
			int num = 0;
			if (this.mForward)
			{
				this.mColor.mRed += this.mRedChange;
				this.mColor.mGreen += this.mGreenChange;
				this.mColor.mBlue += this.mBlueChange;
				this.mColor.mAlpha += this.mAlphaChange;
				if (Common._ATLIMIT(this.mColor.mRed, this.mMaxColor.mRed, this.mRedChange))
				{
					num++;
					this.mColor.mRed = this.mMaxColor.mRed;
				}
				if (Common._ATLIMIT(this.mColor.mGreen, this.mMaxColor.mGreen, this.mGreenChange))
				{
					num++;
					this.mColor.mGreen = this.mMaxColor.mGreen;
				}
				if (Common._ATLIMIT(this.mColor.mBlue, this.mMaxColor.mBlue, this.mBlueChange))
				{
					num++;
					this.mColor.mBlue = this.mMaxColor.mBlue;
				}
				if (Common._ATLIMIT(this.mColor.mAlpha, this.mMaxColor.mAlpha, this.mAlphaChange))
				{
					num++;
					this.mColor.mAlpha = this.mMaxColor.mAlpha;
				}
			}
			else
			{
				this.mColor.mRed -= this.mRedChange;
				this.mColor.mGreen -= this.mGreenChange;
				this.mColor.mBlue -= this.mBlueChange;
				this.mColor.mAlpha -= this.mAlphaChange;
				if (Common._ATLIMIT(this.mColor.mRed, this.mMinColor.mRed, -this.mRedChange))
				{
					num++;
					this.mColor.mRed = this.mMinColor.mRed;
				}
				if (Common._ATLIMIT(this.mColor.mGreen, this.mMinColor.mGreen, -this.mGreenChange))
				{
					num++;
					this.mColor.mGreen = this.mMinColor.mGreen;
				}
				if (Common._ATLIMIT(this.mColor.mBlue, this.mMinColor.mBlue, -this.mBlueChange))
				{
					num++;
					this.mColor.mBlue = this.mMinColor.mBlue;
				}
				if (Common._ATLIMIT(this.mColor.mAlpha, this.mMinColor.mAlpha, -this.mAlphaChange))
				{
					num++;
					this.mColor.mAlpha = this.mMinColor.mAlpha;
				}
			}
			if (num != 4)
			{
				return false;
			}
			if (this.mDuration > 0 && --this.mDuration <= 0)
			{
				this.mEnabled = false;
				return true;
			}
			this.mForward = !this.mForward;
			return true;
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00029DF0 File Offset: 0x00027FF0
		public void SetSpeed(int s)
		{
			this.mRedChange = (this.mGreenChange = (this.mBlueChange = (this.mAlphaChange = (float)s)));
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00029E20 File Offset: 0x00028020
		public void FadeOverTime(int frames)
		{
			this.mRedChange = (this.mMaxColor.mRed - this.mMinColor.mRed) / (float)frames;
			this.mGreenChange = (this.mMaxColor.mGreen - this.mMinColor.mGreen) / (float)frames;
			this.mBlueChange = (this.mMaxColor.mBlue - this.mMinColor.mBlue) / (float)frames;
			this.mAlphaChange = (this.mMaxColor.mAlpha - this.mMinColor.mAlpha) / (float)frames;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00029EB0 File Offset: 0x000280B0
		public void AlphaFadeIn(int arate)
		{
			this.mEnabled = true;
			this.mForward = true;
			this.mDuration = 1;
			this.mAlphaChange = (float)Math.Abs(arate);
			this.mRedChange = (this.mGreenChange = (this.mBlueChange = 0f));
			this.mColor.mRed = (this.mColor.mGreen = (this.mColor.mBlue = 255f));
			this.mColor.mAlpha = 0f;
			this.mMinColor = this.mColor;
			this.mMaxColor.mRed = (this.mMaxColor.mGreen = (this.mMaxColor.mBlue = (this.mMaxColor.mAlpha = 255f)));
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00029F84 File Offset: 0x00028184
		public void AlphaFadeOut(int arate)
		{
			this.mEnabled = true;
			this.mForward = false;
			this.mDuration = 1;
			this.mAlphaChange = (float)Math.Abs(arate);
			this.mRedChange = (this.mGreenChange = (this.mBlueChange = 0f));
			this.mColor.mRed = (this.mColor.mGreen = (this.mColor.mBlue = (this.mColor.mAlpha = 255f)));
			this.mMaxColor = this.mColor;
			this.mMinColor.mRed = (this.mMinColor.mGreen = (this.mMinColor.mBlue = 255f));
			this.mMinColor.mAlpha = 0f;
		}

		// Token: 0x04000785 RID: 1925
		public FColor mColor = new FColor();

		// Token: 0x04000786 RID: 1926
		public FColor mMaxColor = new FColor();

		// Token: 0x04000787 RID: 1927
		public FColor mMinColor = new FColor();

		// Token: 0x04000788 RID: 1928
		public bool mForward;

		// Token: 0x04000789 RID: 1929
		public bool mEnabled;

		// Token: 0x0400078A RID: 1930
		public float mRedChange;

		// Token: 0x0400078B RID: 1931
		public float mGreenChange;

		// Token: 0x0400078C RID: 1932
		public float mBlueChange;

		// Token: 0x0400078D RID: 1933
		public float mAlphaChange;

		// Token: 0x0400078E RID: 1934
		public int mDuration;
	}
}
