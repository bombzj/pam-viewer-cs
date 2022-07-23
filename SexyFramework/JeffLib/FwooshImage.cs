using System;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace JeffLib
{
	// Token: 0x02000109 RID: 265
	public class FwooshImage
	{
		// Token: 0x06000807 RID: 2055 RVA: 0x00028F10 File Offset: 0x00027110
		public FwooshImage()
		{
			this.mImage = null;
			this.mIsDelaying = false;
			this.mAlpha = 255f;
			this.mX = 0;
			this.mY = 0;
			this.mDelay = 0;
			this.mSize = 0f;
			this.mIncText = true;
			this.mAlphaDec = 2f;
			this.mSizeInc = 0.07f;
			this.mMaxSize = 1.2f;
			this.mForward = true;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00028F98 File Offset: 0x00027198
		public void Update()
		{
			if (this.mAlpha < 255f && this.mAlpha > 0f)
			{
				this.mAlpha -= this.mAlphaDec;
				if (this.mAlpha < 0f)
				{
					this.mAlpha = 0f;
					return;
				}
			}
			else if (this.mIncText && this.mSize < this.mMaxSize)
			{
				this.mSize += this.mSizeInc;
				if (this.mSize >= this.mMaxSize)
				{
					this.mIncText = false;
					return;
				}
			}
			else if (!this.mIncText && (this.mSize > 1f || !this.mForward || this.mDelay > 0))
			{
				if ((this.mSize > 1f || this.mDelay == 0 || !this.mForward) && !this.mIsDelaying)
				{
					if (!this.mForward && this.mDelay > 0)
					{
						this.mDelay--;
					}
					if (this.mForward || this.mDelay <= 0)
					{
						this.mSize -= this.mSizeInc;
					}
					if (!this.mForward && this.mSize < 0f)
					{
						this.mSize = 0f;
					}
					else if (this.mDelay > 0 && this.mSize <= 1f)
					{
						this.mSize = 1f;
						this.mIsDelaying = true;
					}
				}
				else if (this.mDelay > 0)
				{
					this.mDelay--;
					if (this.mDelay == 0)
					{
						this.mIsDelaying = false;
					}
				}
				if (this.mSize <= 1f && this.mDelay == 0 && this.mForward)
				{
					this.mSize = Math.Min(1f, this.mMaxSize);
					this.mAlpha -= this.mAlphaDec;
				}
			}
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00029184 File Offset: 0x00027384
		public void Draw(Graphics g)
		{
			if (this.mImage == null || this.mAlpha <= 0f || this.mSize <= 0f)
			{
				return;
			}
			this.mGlobalTransform.Reset();
			if (!SexyFramework.Common._eq(this.mSize, 1f))
			{
				this.mGlobalTransform.Scale(this.mSize, this.mSize);
			}
			g.PushState();
			if (this.mAlpha != 255f)
			{
				g.SetColor(255, 255, 255, (int)this.mAlpha);
				g.SetColorizeImages(true);
			}
			if (!g.Is3D())
			{
				g.DrawImageTransform(this.mImage, this.mGlobalTransform, (float)this.mX, (float)this.mY);
			}
			else
			{
				g.DrawImageTransformF(this.mImage, this.mGlobalTransform, (float)this.mX, (float)this.mY);
			}
			g.SetColorizeImages(false);
			g.PopState();
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00029275 File Offset: 0x00027475
		public void Reverse()
		{
			this.mForward = false;
			this.mIncText = true;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00029285 File Offset: 0x00027485
		public static void inlineUpper(ref string theData)
		{
			theData = theData.ToUpper();
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00029290 File Offset: 0x00027490
		public static void inlineLower(ref string theData)
		{
			theData = theData.ToLower();
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0002929B File Offset: 0x0002749B
		public static void inlineLTrim(ref string theData)
		{
			FwooshImage.inlineLTrim(ref theData, " \t\r\n");
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x000292A8 File Offset: 0x000274A8
		public static void inlineLTrim(ref string theData, string theChars)
		{
			for (int i = 0; i < theData.Length; i++)
			{
				if (theChars.IndexOf(theData[i]) < 0)
				{
					theData = theData.Remove(0, i);
					return;
				}
			}
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x000292E4 File Offset: 0x000274E4
		public static void inlineRTrim(ref string theData)
		{
			FwooshImage.inlineRTrim(ref theData, " \t\r\n");
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x000292F4 File Offset: 0x000274F4
		public static void inlineRTrim(ref string theData, string theChars)
		{
			for (int i = theData.Length - 1; i >= 0; i--)
			{
				if (theChars.IndexOf(theData[i]) < 0)
				{
					theData = theData.Remove(i + 1);
					return;
				}
			}
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00029333 File Offset: 0x00027533
		public static void inlineTrim(ref string theData)
		{
			FwooshImage.inlineTrim(ref theData, " \t\r\n");
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00029340 File Offset: 0x00027540
		public static void inlineTrim(ref string theData, string theChars)
		{
			FwooshImage.inlineRTrim(ref theData, theChars);
			FwooshImage.inlineLTrim(ref theData, theChars);
		}

		// Token: 0x0400076D RID: 1901
		public const uint SEXY_RAND_MAX = 2147483647U;

		// Token: 0x0400076E RID: 1902
		public MemoryImage mImage;

		// Token: 0x0400076F RID: 1903
		public float mAlpha;

		// Token: 0x04000770 RID: 1904
		public int mX;

		// Token: 0x04000771 RID: 1905
		public int mY;

		// Token: 0x04000772 RID: 1906
		public int mDelay;

		// Token: 0x04000773 RID: 1907
		public float mSize;

		// Token: 0x04000774 RID: 1908
		public bool mIncText;

		// Token: 0x04000775 RID: 1909
		public bool mIsDelaying;

		// Token: 0x04000776 RID: 1910
		public float mSizeInc;

		// Token: 0x04000777 RID: 1911
		public float mAlphaDec;

		// Token: 0x04000778 RID: 1912
		public float mMaxSize;

		// Token: 0x04000779 RID: 1913
		public bool mForward;

		// Token: 0x0400077A RID: 1914
		public Transform mGlobalTransform = new Transform();
	}
}
