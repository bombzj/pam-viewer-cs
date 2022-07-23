using System;
using SexyFramework.GraphicsLib;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x020000D5 RID: 213
	public class CreditsHackWidget : Widget, ButtonListener
	{
		// Token: 0x06000BD3 RID: 3027 RVA: 0x0006FF54 File Offset: 0x0006E154
		public CreditsHackWidget()
		{
			Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_HOME_SELECT);
			Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_HOME);
			this.mPriority = 2147483646;
			this.mZOrder = 2147483646;
			this.mHasAlpha = (this.mHasTransparencies = true);
			this.mClip = false;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0006FFAA File Offset: 0x0006E1AA
		public override void Dispose()
		{
			this.RemoveAllWidgets(true, true);
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x0006FFB4 File Offset: 0x0006E1B4
		public virtual void ButtonPress(int theId, int theClickCount)
		{
			this.ButtonPress(theId);
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x0006FFBD File Offset: 0x0006E1BD
		public virtual void ButtonPress(int theId)
		{
			if (GameApp.gApp.mCredits != null)
			{
				GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BUTTON2));
			}
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x0006FFDF File Offset: 0x0006E1DF
		public virtual void ButtonDepress(int theId)
		{
			if (GameApp.gApp.mCredits != null)
			{
				GameApp.gApp.ReturnFromCredits();
			}
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x0006FFF7 File Offset: 0x0006E1F7
		public override void Update()
		{
			if (GameApp.gApp.mCredits != null && GameApp.gApp.mHasFocus)
			{
				this.MarkDirty();
				GameApp.gApp.mCredits.Update();
			}
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00070026 File Offset: 0x0006E226
		public override void Draw(Graphics g)
		{
			if (GameApp.gApp.mCredits != null)
			{
				GameApp.gApp.mCredits.Draw(g);
			}
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00070044 File Offset: 0x0006E244
		public override void MouseUp(int x, int y)
		{
			if (GameApp.gApp.mCredits != null)
			{
				GameApp.gApp.mCredits.mSpeedUp = false;
				if (GameApp.gApp.mCredits.AtEnd() && GameApp.gApp.mCredits.mTapDown)
				{
					GameApp.gApp.mCredits.mTapDown = false;
					OptionsDialog optionsDialog = GameApp.gApp.GetDialog(2) as OptionsDialog;
					if (optionsDialog != null)
					{
						optionsDialog.OnCreditsHided();
					}
					GameApp.gApp.ReturnFromCredits();
				}
			}
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x000700C3 File Offset: 0x0006E2C3
		public override void MouseDown(int x, int y, int theClickCount)
		{
			if (GameApp.gApp.mCredits != null)
			{
				GameApp.gApp.mCredits.mSpeedUp = true;
				if (GameApp.gApp.mCredits.AtEnd())
				{
					GameApp.gApp.mCredits.mTapDown = true;
				}
			}
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00070102 File Offset: 0x0006E302
		public virtual void ButtonDownTick(int x)
		{
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00070104 File Offset: 0x0006E304
		public virtual void ButtonMouseEnter(int x)
		{
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00070106 File Offset: 0x0006E306
		public virtual void ButtonMouseLeave(int x)
		{
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00070108 File Offset: 0x0006E308
		public virtual void ButtonMouseMove(int x, int y, int z)
		{
		}

		// Token: 0x04000A16 RID: 2582
		public ButtonWidget mContinueBtn;
	}
}
