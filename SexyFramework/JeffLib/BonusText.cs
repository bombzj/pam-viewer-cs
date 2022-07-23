using System;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace JeffLib
{
	// Token: 0x02000104 RID: 260
	public class BonusText
	{
		// Token: 0x060007C9 RID: 1993 RVA: 0x00027B10 File Offset: 0x00025D10
		public BonusText(string pText, Font pFont, float pX, float pY, float pSpeed, int pAlphaDelay)
		{
			this.mBulgePct = 1f;
			this.mBulgeAmt = (this.mBulgeDec = 0f);
			this.mBulgeDir = 0;
			this.mAlphaFadeDelay = pAlphaDelay;
			this.mUseSolidColor = false;
			this.mAlpha = 255f;
			this.mHue = 0;
			this.mLife = 250;
			this.mSpeed = pSpeed;
			this.mAlphaDecRate = 1f;
			this.mSolidColor = new SexyColor(SexyColor.White);
			this.mX = ((pX <= -1f) ? ((float)((GlobalMembers.gSexyAppBase.mWidth - pFont.StringWidth(pText)) / 2)) : pX);
			this.mY = ((pY <= -1f) ? ((float)((GlobalMembers.gSexyAppBase.mHeight - pFont.GetHeight()) / 2)) : pY);
			this.mText = pText;
			this.mFont = pFont;
			this.mImage = null;
			this.mLeftJustifyImg = true;
			this.mImageUsesSolidColor = true;
			this.mImageDrawMode = 0;
			this.mSpace = 5;
			this.mImageColor = new SexyColor(SexyColor.White);
			this.mImageCel = 0;
			this.mDone = false;
			this.mUpdateCnt = 0;
			this.mSize = 1f;
			int num = this.mFont.StringWidth(pText);
			this.mTextWidth = num;
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00027C5C File Offset: 0x00025E5C
		public BonusText()
		{
			this.mSize = 1f;
			this.mBulgePct = 1f;
			this.mBulgeAmt = (this.mBulgeDec = 0f);
			this.mBulgeDir = 0;
			this.mX = (this.mY = (float)(this.mUpdateCnt = (this.mTextWidth = (this.mHue = (this.mLife = (this.mAlphaFadeDelay = (this.mImageDrawMode = 0)))))));
			this.mLeftJustifyImg = (this.mDone = (this.mUseSolidColor = (this.mImageUsesSolidColor = false)));
			this.mFont = null;
			this.mImage = null;
			this.mAlpha = (this.mAlphaDecRate = (this.mSpeed = 0f));
			this.mSpace = (this.mImageCel = 0);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00027D58 File Offset: 0x00025F58
		public void Bulge(float pct, float rate, int count)
		{
			this.mSize = 1f;
			this.mBulgePct = pct;
			this.mBulgeAmt = rate;
			this.mBulgeDir = 1;
			this.mBulgeDec = (pct - 1f) / (float)count;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00027D8C File Offset: 0x00025F8C
		public void Update()
		{
			this.mUpdateCnt++;
			if (this.mDone)
			{
				return;
			}
			if (this.mBulgeDir != 0)
			{
				this.mSize += (float)this.mBulgeDir * this.mBulgeAmt;
				if (this.mBulgeDir > 0 && this.mSize >= this.mBulgePct)
				{
					this.mSize = this.mBulgePct;
					this.mBulgeDir = -1;
					this.mBulgePct -= this.mBulgeDec;
				}
				else if (this.mBulgeDir < 0 && this.mSize <= 1f)
				{
					this.mSize = 1f;
					this.mBulgeDir = 1;
				}
				if (SexyFramework.Common._eq(this.mSize, 1f) && SexyFramework.Common._leq(this.mBulgePct, 1f))
				{
					this.mSize = 1f;
					this.mBulgeDir = 0;
				}
			}
			this.mY -= this.mSpeed;
			if (this.mY < (float)(-(float)this.mFont.mHeight))
			{
				this.mDone = true;
			}
			if (!this.mUseSolidColor || !this.mImageUsesSolidColor)
			{
				this.mHue = (this.mHue + 7) % 255;
				if (--this.mLife <= 0)
				{
					this.mLife = 0;
					if (this.mAlpha <= 0f)
					{
						this.mDone = true;
					}
				}
			}
			if (--this.mAlphaFadeDelay <= 0)
			{
				this.mAlpha -= this.mAlphaDecRate;
				if (this.mAlpha < 0f)
				{
					this.mAlpha = 0f;
					if (this.mUseSolidColor || this.mLife <= 0)
					{
						this.mDone = true;
					}
				}
			}
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00027F4C File Offset: 0x0002614C
		public void Draw(Graphics g)
		{
			if (!this.mDone)
			{
				float num = (float)this.mLife / 18f;
				if (num > 1f)
				{
					num = 1f;
				}
				int theY = 0;
				int theX = 0;
				int num2;
				int num3;
				if (this.mImage != null)
				{
					if (this.mImage.mHeight > this.mFont.GetHeight())
					{
						num2 = (int)(this.mY + (float)((this.mImage.GetCelHeight() - this.mFont.GetHeight()) / 2) + (float)this.mFont.GetAscent());
						theY = (int)this.mY;
					}
					else
					{
						num2 = (int)(this.mY + (float)this.mFont.GetAscent());
						theY = (int)(this.mY + (float)((this.mFont.GetHeight() - this.mImage.mHeight) / 2));
					}
					num3 = (int)(this.mLeftJustifyImg ? (this.mX + (float)this.mImage.GetCelWidth() + (float)this.mSpace) : this.mX);
					theX = (int)(this.mLeftJustifyImg ? this.mX : (this.mX + (float)this.mTextWidth + (float)this.mSpace));
				}
				else
				{
					num2 = (int)(this.mY + (float)this.mFont.GetAscent());
					num3 = (int)this.mX;
				}
				Graphics3D graphics3D = g.Get3D();
				if (!SexyFramework.Common._eq(this.mSize, 1f) && graphics3D != null)
				{
					int num4 = 0;
					if (this.mWidth == 0 || this.mHeight == 0)
					{
						this.mWidth = g.GetFont().StringWidth(this.mText);
						this.mHeight = g.GetWordWrappedHeight(1000000, this.mText, -1, ref num4, ref num4);
					}
					SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
					sexyTransform2D.Translate((float)(-(float)num3 - this.mWidth / 2 + GlobalMembers.gSexyAppBase.mScreenBounds.mX), (float)(-(float)num2 - this.mHeight / 2));
					sexyTransform2D.Scale(this.mSize, this.mSize);
					sexyTransform2D.Translate((float)(num3 + this.mWidth / 2 - GlobalMembers.gSexyAppBase.mScreenBounds.mX), (float)(num2 + this.mHeight / 2));
					graphics3D.PushTransform(sexyTransform2D);
				}
				g.SetFont(this.mFont);
				SexyColor color = ((!this.mUseSolidColor) ? new SexyColor((int)((GlobalMembers.gSexyAppBase.HSLToRGB(this.mHue, 255, 128) & 16777215UL) | (ulong)((ulong)((uint)(num * 255f)) << 24))) : new SexyColor(this.mSolidColor));
				color.mAlpha = (int)this.mAlpha;
				g.SetColor(color);
				g.DrawString(this.mText, num3, num2);
				if (this.mImage != null)
				{
					g.PushState();
					g.SetDrawMode(this.mImageDrawMode);
					if (!this.mImageUsesSolidColor)
					{
						g.SetColorizeImages(true);
						g.SetColor(color);
					}
					else if (this.mImageColor != SexyColor.White)
					{
						g.SetColorizeImages(true);
						g.SetColor(new SexyColor(this.mImageColor)
						{
							mAlpha = (int)num
						});
					}
					g.DrawImageCel(this.mImage, theX, theY, this.mImageCel);
					g.PopState();
				}
				if (!SexyFramework.Common._eq(this.mSize, 1f) && graphics3D != null)
				{
					graphics3D.PopTransform();
				}
			}
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0002829F File Offset: 0x0002649F
		public void AddImage(Image img, bool solid_color, bool left_justify, int img_draw_mode)
		{
			this.mImage = img;
			this.mImageUsesSolidColor = solid_color;
			this.mLeftJustifyImg = left_justify;
			this.mImageDrawMode = img_draw_mode;
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x000282BE File Offset: 0x000264BE
		public void AddImage(Image img, bool solid_color, bool left_justify)
		{
			this.AddImage(img, solid_color, left_justify, 0);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x000282CA File Offset: 0x000264CA
		public bool IsDone()
		{
			return this.mDone;
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x000282D2 File Offset: 0x000264D2
		public void SetAlpha(int a)
		{
			this.mAlpha = (float)a;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x000282DC File Offset: 0x000264DC
		public void SetAlphaDelay(int d)
		{
			this.mAlphaFadeDelay = d;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x000282E5 File Offset: 0x000264E5
		public void SetAlphaDecRate(float d)
		{
			this.mAlphaDecRate = d;
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x000282EE File Offset: 0x000264EE
		public void SetMaxLife(int l)
		{
			this.mLife = l;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x000282F7 File Offset: 0x000264F7
		public void NoHSL()
		{
			this.mUseSolidColor = true;
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00028300 File Offset: 0x00026500
		public void SetX(float x)
		{
			this.mX = x;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00028309 File Offset: 0x00026509
		public void SetY(float y)
		{
			this.mY = y;
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00028312 File Offset: 0x00026512
		public int GetWidth()
		{
			if (this.mImage == null)
			{
				return this.mTextWidth;
			}
			return this.mTextWidth + this.mImage.GetCelWidth() + this.mSpace;
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0002833C File Offset: 0x0002653C
		public float GetX()
		{
			return this.mX;
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00028344 File Offset: 0x00026544
		public float GetY()
		{
			return this.mY;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0002834C File Offset: 0x0002654C
		public Font GetFont()
		{
			return this.mFont;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00028354 File Offset: 0x00026554
		public string GetString()
		{
			return this.mText;
		}

		// Token: 0x0400072C RID: 1836
		public const int CHANGE_AMOUNT = 8;

		// Token: 0x0400072D RID: 1837
		protected float mBulgePct;

		// Token: 0x0400072E RID: 1838
		protected float mBulgeAmt;

		// Token: 0x0400072F RID: 1839
		protected float mBulgeDec;

		// Token: 0x04000730 RID: 1840
		protected float mSize;

		// Token: 0x04000731 RID: 1841
		protected int mBulgeDir;

		// Token: 0x04000732 RID: 1842
		protected float mX;

		// Token: 0x04000733 RID: 1843
		protected float mY;

		// Token: 0x04000734 RID: 1844
		protected int mUpdateCnt;

		// Token: 0x04000735 RID: 1845
		protected int mTextWidth;

		// Token: 0x04000736 RID: 1846
		protected int mHue;

		// Token: 0x04000737 RID: 1847
		protected int mLife;

		// Token: 0x04000738 RID: 1848
		protected int mAlphaFadeDelay;

		// Token: 0x04000739 RID: 1849
		protected int mImageDrawMode;

		// Token: 0x0400073A RID: 1850
		protected bool mLeftJustifyImg;

		// Token: 0x0400073B RID: 1851
		protected bool mDone;

		// Token: 0x0400073C RID: 1852
		protected bool mUseSolidColor;

		// Token: 0x0400073D RID: 1853
		protected bool mImageUsesSolidColor;

		// Token: 0x0400073E RID: 1854
		protected string mText;

		// Token: 0x0400073F RID: 1855
		protected int mWidth;

		// Token: 0x04000740 RID: 1856
		protected int mHeight;

		// Token: 0x04000741 RID: 1857
		protected Font mFont;

		// Token: 0x04000742 RID: 1858
		protected Image mImage;

		// Token: 0x04000743 RID: 1859
		protected float mAlpha;

		// Token: 0x04000744 RID: 1860
		protected float mAlphaDecRate;

		// Token: 0x04000745 RID: 1861
		protected float mSpeed;

		// Token: 0x04000746 RID: 1862
		public int mSpace;

		// Token: 0x04000747 RID: 1863
		public int mImageCel;

		// Token: 0x04000748 RID: 1864
		public SexyColor mImageColor;

		// Token: 0x04000749 RID: 1865
		public SexyColor mSolidColor;
	}
}
