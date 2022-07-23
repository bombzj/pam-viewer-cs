using System;
using System.Collections.Generic;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x0200002C RID: 44
	public class SquidAnim
	{
		// Token: 0x0600059D RID: 1437 RVA: 0x0001D5F3 File Offset: 0x0001B7F3
		public SquidAnim()
		{
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001D608 File Offset: 0x0001B808
		public SquidAnim(SquidAnim rhs)
		{
			this.mImage = rhs.mImage;
			this.mUpdateCount = rhs.mUpdateCount;
			this.mCurCel = rhs.mCurCel;
			this.mX = rhs.mX;
			this.mY = rhs.mY;
			for (int i = 0; i < rhs.mCels.Count; i++)
			{
				SquidAnimCel squidAnimCel = new SquidAnimCel();
				squidAnimCel.mCelNum = rhs.mCels[i].mCelNum;
				squidAnimCel.mDelay = rhs.mCels[i].mDelay;
				this.mCels.Add(squidAnimCel);
			}
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001D6B8 File Offset: 0x0001B8B8
		public void AddAnimInfo(int cel_num, int delay)
		{
			SquidAnimCel squidAnimCel = new SquidAnimCel();
			this.mCels.Add(squidAnimCel);
			squidAnimCel.mCelNum = cel_num;
			squidAnimCel.mDelay = delay;
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001D6E8 File Offset: 0x0001B8E8
		public void Update()
		{
			if (++this.mUpdateCount >= this.mCels[this.mCurCel].mDelay)
			{
				this.mUpdateCount = 0;
				this.mCurCel = (this.mCurCel + 1) % this.mCels.Count;
			}
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001D73F File Offset: 0x0001B93F
		public void Draw(Graphics g, float x, float y)
		{
			g.DrawImageCel(this.mImage, (int)(x + Common._S(this.mX)), (int)(y + Common._S(this.mY)), this.mCels[this.mCurCel].mCelNum);
		}

		// Token: 0x04000268 RID: 616
		public Image mImage;

		// Token: 0x04000269 RID: 617
		public List<SquidAnimCel> mCels = new List<SquidAnimCel>();

		// Token: 0x0400026A RID: 618
		public int mUpdateCount;

		// Token: 0x0400026B RID: 619
		public int mCurCel;

		// Token: 0x0400026C RID: 620
		public float mX;

		// Token: 0x0400026D RID: 621
		public float mY;
	}
}
