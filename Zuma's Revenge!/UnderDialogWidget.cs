using System;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x0200014C RID: 332
	public class UnderDialogWidget : Widget
	{
		// Token: 0x0600103C RID: 4156 RVA: 0x000A609A File Offset: 0x000A429A
		public UnderDialogWidget()
		{
			this.mMouseVisible = false;
			this.mHasAlpha = true;
			this.mShrunkScreen1 = null;
			this.mShrunkScreen2 = null;
			this.mClip = false;
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x000A60C8 File Offset: 0x000A42C8
		~UnderDialogWidget()
		{
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x000A60F0 File Offset: 0x000A42F0
		public void CreateImages()
		{
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x000A60F2 File Offset: 0x000A42F2
		public void DrawPaused(Graphics g)
		{
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x000A60F4 File Offset: 0x000A42F4
		public override void Update()
		{
			base.Update();
			if (GameApp.gApp.mDialogObscurePct > 0f && GlobalMembers.gSexyAppBase.mHasFocus)
			{
				this.MarkDirty();
			}
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x000A611F File Offset: 0x000A431F
		public override void Draw(Graphics g)
		{
		}

		// Token: 0x04001AA1 RID: 6817
		public DeviceImage mShrunkScreen1;

		// Token: 0x04001AA2 RID: 6818
		public DeviceImage mShrunkScreen2;
	}
}
