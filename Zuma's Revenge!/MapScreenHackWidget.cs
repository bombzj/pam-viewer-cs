using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x020000CE RID: 206
	public class MapScreenHackWidget : Widget
	{
		// Token: 0x06000AEA RID: 2794 RVA: 0x0006B163 File Offset: 0x00069363
		public MapScreenHackWidget()
		{
			this.mClip = false;
			this.mApp = GameApp.gApp;
			this.mDelay = 0;
			this.mToggledAdventureMode = false;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x0006B18C File Offset: 0x0006938C
		public override void Update()
		{
			if (this.mApp.mMapScreen != null && this.mApp.mMapScreen.mDirty)
			{
				this.MarkDirty();
			}
			if (this.mDelay == 0)
			{
				this.mApp.mMapScreen.Update();
				if (this.mApp.mMapScreen != null && this.mApp.mMapScreen.mRemove)
				{
					if (this.mApp.mMapScreen.mSelectedZone == -1)
					{
						this.mDelay = Common._M(10);
						return;
					}
					this.mDelay = Common._M(40);
					return;
				}
			}
			else if (this.mApp.mMapScreen != null)
			{
				this.mDelay--;
				if (this.mDelay == 0 && !this.mToggledAdventureMode)
				{
					this.mToggledAdventureMode = true;
					this.mApp.mMapScreen.CleanButtons();
					this.mApp.mForceZoneRestart = this.mApp.mMapScreen.mSelectedZone;
					this.mApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(this.mApp.StartAdventureMode);
					this.mApp.ToggleBambooTransition();
				}
			}
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x0006B2B3 File Offset: 0x000694B3
		public override void Draw(Graphics g)
		{
			if (this.mApp.mMapScreen == null)
			{
				return;
			}
			this.mApp.mMapScreen.Draw(g);
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x0006B2D4 File Offset: 0x000694D4
		public override void DrawAll(ModalFlags theFlags, Graphics g)
		{
			if (g != null)
			{
				g.Get3D();
			}
			base.DrawAll(theFlags, g);
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0006B2E8 File Offset: 0x000694E8
		public override void MouseMove(int x, int y)
		{
			if (this.mApp.mMapScreen == null || this.mApp.mDialogMap.Count > 0 || this.mDelay > 0)
			{
				return;
			}
			this.mApp.mMapScreen.MouseMove(x, y);
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0006B326 File Offset: 0x00069526
		public override void MouseDrag(int x, int y)
		{
			if (this.mApp.mMapScreen == null || this.mApp.mDialogMap.Count > 0 || this.mDelay > 0)
			{
				return;
			}
			this.mApp.mMapScreen.MouseMove(x, y);
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0006B364 File Offset: 0x00069564
		public override void MouseDown(int x, int y, int cc)
		{
			if (this.mApp.mMapScreen == null || this.mApp.mDialogMap.Count > 0 || this.mDelay > 0)
			{
				return;
			}
			this.mApp.mMapScreen.MouseDown(x, y);
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0006B3A2 File Offset: 0x000695A2
		public override void MouseUp(int x, int y)
		{
			if (this.mApp.mMapScreen == null || this.mApp.mDialogMap.Count > 0 || this.mDelay > 0)
			{
				return;
			}
			this.mApp.mMapScreen.MouseUp(x, y);
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0006B3E0 File Offset: 0x000695E0
		public override void MouseLeave()
		{
			this.mApp.mMapScreen.MouseLeave();
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0006B3F2 File Offset: 0x000695F2
		public override void KeyChar(char theChar)
		{
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0006B3F4 File Offset: 0x000695F4
		public override void GotFocus()
		{
			base.GotFocus();
			if (this.mWidgetManager != null && this.mApp.mMapScreen != null && this.mApp.mMapScreen.mContinueBtn != null)
			{
				this.mWidgetManager.SetGamepadSelection(this.mApp.mMapScreen.mContinueBtn, WidgetLinkDir.LINK_DIR_NONE);
			}
		}

		// Token: 0x04000991 RID: 2449
		public GameApp mApp;

		// Token: 0x04000992 RID: 2450
		public int mDelay;

		// Token: 0x04000993 RID: 2451
		public bool mToggledAdventureMode;
	}
}
