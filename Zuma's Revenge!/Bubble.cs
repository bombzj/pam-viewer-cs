using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x02000083 RID: 131
	public class Bubble
	{
		// Token: 0x060008D0 RID: 2256 RVA: 0x0004F788 File Offset: 0x0004D988
		public void Init(float vx, float vy, float jiggle_speed, int jiggle_timer)
		{
			this.mVX = vx;
			this.mVY = vy;
			this.mJiggleSpeed = jiggle_speed;
			this.mJiggleTimer = jiggle_timer;
			this.mDefJiggleTimer = jiggle_timer;
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0004F7BC File Offset: 0x0004D9BC
		public void Update()
		{
			if (this.mDelay > 0)
			{
				this.mDelay--;
				return;
			}
			this.mX += this.mVX;
			this.mY += this.mVY;
			if (this.mJiggleLeft)
			{
				this.mX -= this.mJiggleSpeed;
			}
			else
			{
				this.mX += this.mJiggleSpeed;
			}
			if (--this.mJiggleTimer <= 0)
			{
				this.mJiggleLeft = !this.mJiggleLeft;
				this.mJiggleTimer = this.mDefJiggleTimer;
			}
			this.mAlpha -= this.mAlphaDec;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0004F879 File Offset: 0x0004DA79
		public void Draw(Graphics g)
		{
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0004F87B File Offset: 0x0004DA7B
		public void SetX(float x)
		{
			this.mX = x;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0004F884 File Offset: 0x0004DA84
		public void SetY(float y)
		{
			this.mY = y;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0004F88D File Offset: 0x0004DA8D
		public void SetAlphaFade(float f)
		{
			this.mAlphaDec = f;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0004F896 File Offset: 0x0004DA96
		public void SetDelay(int d)
		{
			this.mDelay = d;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0004F89F File Offset: 0x0004DA9F
		public float GetAlpha()
		{
			return this.mAlpha;
		}

		// Token: 0x040006D2 RID: 1746
		protected float mX;

		// Token: 0x040006D3 RID: 1747
		protected float mY;

		// Token: 0x040006D4 RID: 1748
		protected float mVX;

		// Token: 0x040006D5 RID: 1749
		protected float mVY;

		// Token: 0x040006D6 RID: 1750
		protected float mJiggleSpeed;

		// Token: 0x040006D7 RID: 1751
		protected bool mJiggleLeft;

		// Token: 0x040006D8 RID: 1752
		protected int mJiggleTimer;

		// Token: 0x040006D9 RID: 1753
		protected int mDefJiggleTimer;

		// Token: 0x040006DA RID: 1754
		protected int mDelay;

		// Token: 0x040006DB RID: 1755
		protected float mAlpha;

		// Token: 0x040006DC RID: 1756
		protected float mAlphaDec;
	}
}
