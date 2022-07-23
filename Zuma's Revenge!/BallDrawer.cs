using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x0200008E RID: 142
	public class BallDrawer
	{
		// Token: 0x0600093C RID: 2364 RVA: 0x00052FA0 File Offset: 0x000511A0
		public void Reset()
		{
			this.mMaxBallPriority = 0;
			for (int i = 0; i < 5; i++)
			{
				this.mNumBalls[i] = 0;
				this.mNumShadows[i] = 0;
				this.mNumOverlays[i] = 0;
				this.mNumUnderlays[i] = 0;
			}
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00052FE4 File Offset: 0x000511E4
		public void AddBall(Ball theBall, int thePriority)
		{
			int num = this.mNumBalls[thePriority]++;
			this.mBalls[thePriority, num] = theBall;
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0005301C File Offset: 0x0005121C
		public void AddShadow(Ball theBall, int thePriority)
		{
			int num = this.mNumShadows[thePriority]++;
			this.mShadows[thePriority, num] = theBall;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00053054 File Offset: 0x00051254
		public void AddOverlay(Ball theBall, int thePriority)
		{
			int num = this.mNumOverlays[thePriority]++;
			this.mOverlays[thePriority, num] = theBall;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0005308C File Offset: 0x0005128C
		public void AddUnderlay(Ball theBall, int thePriority)
		{
			int num = this.mNumUnderlays[thePriority]++;
			this.mUnderlays[thePriority, num] = theBall;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x000530C4 File Offset: 0x000512C4
		public void Draw(Graphics g, Board theBoard)
		{
			g.Get3D();
			for (int i = 0; i < 5; i++)
			{
				if (i != 0)
				{
					theBoard.DrawTunnels(g, i, true);
				}
				theBoard.mLevel.DrawPriority(g, i);
				for (int j = 0; j < theBoard.mLevel.mNumCurves; j++)
				{
					theBoard.mLevel.mCurveMgr[j].DrawMisc(g, i);
					theBoard.mLevel.mCurveMgr[j].DrawSkullPathShit(g, i);
				}
				if (!Board.gHideBalls)
				{
					int num = this.mNumShadows[i];
					for (int j = 0; j < num; j++)
					{
						this.mShadows[i, j].DrawShadow(g);
					}
					theBoard.DrawTunnels(g, i, false);
					num = this.mNumUnderlays[i];
					for (int j = 0; j < num; j++)
					{
						this.mUnderlays[i, j].DrawBottomLayer(g);
					}
					num = this.mNumBalls[i];
					for (int j = 0; j < num; j++)
					{
						this.mBalls[i, j].DrawBase(g, 0, 0);
					}
					for (int j = 0; j < num; j++)
					{
						this.mBalls[i, j].DrawAdditive(g, 0, 0);
					}
					if (g.Is3D())
					{
						num = this.mNumOverlays[i];
						for (int j = 0; j < num; j++)
						{
							this.mOverlays[i, j].DrawTopLayer(g);
						}
					}
				}
			}
			for (int j = 0; j < theBoard.mLevel.mNumCurves; j++)
			{
				theBoard.mLevel.mCurveMgr[j].DrawAboveBalls(g);
			}
		}

		// Token: 0x04000748 RID: 1864
		public int mMaxBallPriority;

		// Token: 0x04000749 RID: 1865
		public int[] mNumBalls = new int[5];

		// Token: 0x0400074A RID: 1866
		public int[] mNumShadows = new int[5];

		// Token: 0x0400074B RID: 1867
		public int[] mNumOverlays = new int[5];

		// Token: 0x0400074C RID: 1868
		public int[] mNumUnderlays = new int[5];

		// Token: 0x0400074D RID: 1869
		private Ball[,] mBalls = new Ball[5, 1024];

		// Token: 0x0400074E RID: 1870
		private Ball[,] mShadows = new Ball[5, 1024];

		// Token: 0x0400074F RID: 1871
		private Ball[,] mOverlays = new Ball[5, 1024];

		// Token: 0x04000750 RID: 1872
		private Ball[,] mUnderlays = new Ball[5, 1024];
	}
}
