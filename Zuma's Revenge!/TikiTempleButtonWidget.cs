using System;
using JeffLib;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x02000144 RID: 324
	public class TikiTempleButtonWidget : ExtraSexyButton
	{
		// Token: 0x06001016 RID: 4118 RVA: 0x000A1471 File Offset: 0x0009F671
		public TikiTempleButtonWidget(int theId, TikiTemple theListener)
			: base(theId, theListener)
		{
			this.mUsesAnimators = false;
			this.mTikiTemple = theListener;
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x000A1489 File Offset: 0x0009F689
		public override void Draw(Graphics g)
		{
			base.Draw(g);
		}

		// Token: 0x04001705 RID: 5893
		public TikiTemple mTikiTemple;
	}
}
