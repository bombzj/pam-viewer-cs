using System;
using JeffLib;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x02000003 RID: 3
	public class AchievementsButtonWidget : ExtraSexyButton
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000023CD File Offset: 0x000005CD
		public AchievementsButtonWidget(int theId, Achievements theListener)
			: base(theId, theListener)
		{
			this.mUsesAnimators = false;
			this.mAchievements = theListener;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000023E5 File Offset: 0x000005E5
		public override void Draw(Graphics g)
		{
			base.Draw(g);
		}

		// Token: 0x0400000A RID: 10
		public Achievements mAchievements;
	}
}
