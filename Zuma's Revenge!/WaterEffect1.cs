using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x020000BA RID: 186
	public class WaterEffect1 : Effect
	{
		// Token: 0x06000AA3 RID: 2723 RVA: 0x00067D5B File Offset: 0x00065F5B
		public WaterEffect1()
		{
			this.mResGroup = "";
			this.Reset("");
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00067D79 File Offset: 0x00065F79
		protected void SetupShoreWaves(int x, int y, bool mirror, float vx, float vy)
		{
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00067D7B File Offset: 0x00065F7B
		public override string GetName()
		{
			return "WaterEffect1";
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x00067D82 File Offset: 0x00065F82
		public override void Update()
		{
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00067D84 File Offset: 0x00065F84
		public override void Reset(string level_id)
		{
			this.mUpdateCount++;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00067D94 File Offset: 0x00065F94
		public override void DrawPriority(Graphics g, int priority)
		{
		}
	}
}
