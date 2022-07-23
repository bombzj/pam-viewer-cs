using System;
using JeffLib;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x020000EE RID: 238
	public class LeaderBoardsButtonWidget : ExtraSexyButton
	{
		// Token: 0x06000CE6 RID: 3302 RVA: 0x0007CFD4 File Offset: 0x0007B1D4
		public LeaderBoardsButtonWidget(int theId, LeaderBoards theListener)
			: base(theId, theListener)
		{
			this.mUsesAnimators = false;
			this.mLeaderBoards = theListener;
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0007CFEC File Offset: 0x0007B1EC
		public override void Draw(Graphics g)
		{
			base.Draw(g);
		}

		// Token: 0x04000B71 RID: 2929
		public LeaderBoards mLeaderBoards;
	}
}
