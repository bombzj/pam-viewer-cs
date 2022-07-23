using System;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x020000CD RID: 205
	public class WidescreenBoardWidget : Widget
	{
		// Token: 0x06000AE4 RID: 2788 RVA: 0x0006B05A File Offset: 0x0006925A
		public WidescreenBoardWidget()
		{
			this.mWidgetFlagsMod.mRemoveFlags |= 5;
			this.mApp = GameApp.gApp;
			this.mZOrder = 2147483646;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x0006B08B File Offset: 0x0006928B
		public override void MouseDown(int x, int y, int theClickCount)
		{
			if (this.mApp.GetBoard() != null)
			{
				this.mApp.GetBoard().MouseDown(x - Common._S(80), y, theClickCount);
			}
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0006B0B5 File Offset: 0x000692B5
		public override void MouseUp(int x, int y, int theClickCount)
		{
			if (this.mApp.GetBoard() != null)
			{
				this.mApp.GetBoard().MouseUp(x - Common._S(80), y, theClickCount);
			}
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0006B0DF File Offset: 0x000692DF
		public override bool IsPointVisible(int x, int y)
		{
			return this.mApp.GetBoard() != null && (x < Common._S(80) || x > this.mApp.mWidth + Common._S(80));
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0006B111 File Offset: 0x00069311
		public override void MouseMove(int x, int y)
		{
			if (this.mApp.GetBoard() != null)
			{
				this.mApp.GetBoard().MouseMove(x - Common._S(80), y);
			}
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0006B13A File Offset: 0x0006933A
		public override void MouseDrag(int x, int y)
		{
			if (this.mApp.GetBoard() != null)
			{
				this.mApp.GetBoard().MouseMove(x - Common._S(80), y);
			}
		}

		// Token: 0x04000990 RID: 2448
		public GameApp mApp;
	}
}
