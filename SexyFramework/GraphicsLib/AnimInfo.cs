using System;
using System.Collections.Generic;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000C3 RID: 195
	public class AnimInfo : IDisposable
	{
		// Token: 0x06000607 RID: 1543 RVA: 0x0001329C File Offset: 0x0001149C
		public AnimInfo()
		{
			this.mAnimType = AnimType.AnimType_None;
			this.mFrameDelay = 1;
			this.mNumCels = 1;
			this.mBeginDelay = 0;
			this.mEndDelay = 0;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x000132E8 File Offset: 0x000114E8
		public AnimInfo(AnimInfo rhs)
		{
			this.mAnimType = rhs.mAnimType;
			this.mFrameDelay = rhs.mFrameDelay;
			this.mNumCels = rhs.mNumCels;
			this.mPerFrameDelay.AddRange(rhs.mPerFrameDelay.ToArray());
			this.mFrameMap.AddRange(rhs.mFrameMap.ToArray());
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00013361 File Offset: 0x00011561
		public virtual void Dispose()
		{
			this.mPerFrameDelay.Clear();
			this.mFrameMap.Clear();
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00013379 File Offset: 0x00011579
		public void SetPerFrameDelay(int theFrame, int theTime)
		{
			if (this.mPerFrameDelay.Count <= theFrame)
			{
				this.mPerFrameDelay.Resize(theFrame + 1);
			}
			this.mPerFrameDelay[theFrame] = theTime;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x000133A4 File Offset: 0x000115A4
		public void Compute(int theNumCels)
		{
			this.Compute(theNumCels, 0, 0);
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x000133AF File Offset: 0x000115AF
		public void Compute(int theNumCels, int theBeginFrameTime)
		{
			this.Compute(theNumCels, theBeginFrameTime, 0);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x000133BC File Offset: 0x000115BC
		public void Compute(int theNumCels, int theBeginFrameTime, int theEndFrameTime)
		{
			this.mNumCels = theNumCels;
			if (this.mNumCels <= 0)
			{
				this.mNumCels = 1;
			}
			if (this.mFrameDelay <= 0)
			{
				this.mFrameDelay = 1;
			}
			if (this.mAnimType == AnimType.AnimType_PingPong && this.mNumCels > 1)
			{
				this.mFrameMap.Resize(theNumCels * 2 - 2);
				int num = 0;
				for (int i = 0; i < theNumCels; i++)
				{
					this.mFrameMap[num++] = i;
				}
				for (int i = theNumCels - 2; i >= 1; i--)
				{
					this.mFrameMap[num++] = i;
				}
			}
			if (this.mFrameMap.Count != 0)
			{
				this.mNumCels = this.mFrameMap.Count;
			}
			if (theBeginFrameTime > 0)
			{
				this.SetPerFrameDelay(0, theBeginFrameTime);
			}
			if (theEndFrameTime > 0)
			{
				this.SetPerFrameDelay(this.mNumCels - 1, theEndFrameTime);
			}
			if (this.mPerFrameDelay.Count != 0)
			{
				this.mTotalAnimTime = 0;
				this.mPerFrameDelay.Resize(this.mNumCels);
				for (int i = 0; i < this.mNumCels; i++)
				{
					if (this.mPerFrameDelay[i] <= 0)
					{
						this.mPerFrameDelay[i] = this.mFrameDelay;
					}
					this.mTotalAnimTime += this.mPerFrameDelay[i];
				}
			}
			else
			{
				this.mTotalAnimTime = this.mFrameDelay * this.mNumCels;
			}
			if (this.mFrameMap.Count != 0)
			{
				this.mFrameMap.Resize(this.mNumCels);
			}
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00013530 File Offset: 0x00011730
		public int GetPerFrameCel(int theTime)
		{
			for (int i = 0; i < this.mNumCels; i++)
			{
				theTime -= this.mPerFrameDelay[i];
				if (theTime < 0)
				{
					return i;
				}
			}
			return this.mNumCels - 1;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0001356C File Offset: 0x0001176C
		public int GetCel(int theTime)
		{
			if (this.mAnimType == AnimType.AnimType_Once && theTime >= this.mTotalAnimTime)
			{
				if (this.mFrameMap.Count != 0)
				{
					return this.mFrameMap[this.mFrameMap.Count - 1];
				}
				return this.mNumCels - 1;
			}
			else
			{
				theTime %= this.mTotalAnimTime;
				int num;
				if (this.mPerFrameDelay.Count != 0)
				{
					num = this.GetPerFrameCel(theTime);
				}
				else
				{
					num = theTime / this.mFrameDelay % this.mNumCels;
				}
				if (this.mFrameMap.Count == 0)
				{
					return num;
				}
				return this.mFrameMap[num];
			}
		}

		// Token: 0x040004E4 RID: 1252
		public AnimType mAnimType;

		// Token: 0x040004E5 RID: 1253
		public int mFrameDelay;

		// Token: 0x040004E6 RID: 1254
		public int mBeginDelay;

		// Token: 0x040004E7 RID: 1255
		public int mEndDelay;

		// Token: 0x040004E8 RID: 1256
		public int mNumCels;

		// Token: 0x040004E9 RID: 1257
		public List<int> mPerFrameDelay = new List<int>();

		// Token: 0x040004EA RID: 1258
		public List<int> mFrameMap = new List<int>();

		// Token: 0x040004EB RID: 1259
		public int mTotalAnimTime;
	}
}
