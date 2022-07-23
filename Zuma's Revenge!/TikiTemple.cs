using System;
using SexyFramework.GraphicsLib;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000148 RID: 328
	public class TikiTemple : Widget, ButtonListener
	{
		// Token: 0x06001023 RID: 4131 RVA: 0x000A32B4 File Offset: 0x000A14B4
		public TikiTemple()
		{
			if (!GameApp.gApp.mResourceManager.IsGroupLoaded("CommonGame") && !GameApp.gApp.mResourceManager.LoadResources("CommonGame"))
			{
				GameApp.gApp.Shutdown();
			}
			this.mDisplayMode = -1;
			this.mClip = false;
			this.mSelectedScreenState = 0;
			this.mHomeButton = null;
			if (GameApp.mGameRes == 768)
			{
				this.mTitleXOffset = 30f;
			}
			else
			{
				this.mTitleXOffset = 20f;
			}
			this.mNeedsInitScroll = true;
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x000A345C File Offset: 0x000A165C
		public override void Dispose()
		{
			this.RemoveAllWidgets(true, true);
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x000A3468 File Offset: 0x000A1668
		public void Init()
		{
			this.mTikiTemplePages = new TikiTemplePages(this);
			this.mTikiTempleScrollWidget = new ScrollWidget();
			this.mTikiTempleScrollWidget.Resize(Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES)) - GameApp.gApp.mWideScreenXOffset + Common._DS(30), Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES)), this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth() + 30, this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetHeight());
			this.mTikiTempleScrollWidget.SetScrollMode(ScrollWidget.ScrollMode.SCROLL_HORIZONTAL);
			this.mTikiTempleScrollWidget.EnableBounce(true);
			this.mTikiTempleScrollWidget.EnablePaging(true);
			this.mTikiTempleScrollWidget.AddWidget(this.mTikiTemplePages);
			this.mTikiTemplePageControl = new PageControl(this.IMAGE_UI_CHALLENGE_PAGE_INDICATOR);
			this.IMAGE_UI_CHALLENGE_PAGE_INDICATOR.GetCelWidth();
			this.mTikiTemplePages.NumPages();
			this.mTikiTemplePageControl.SetNumberOfPages(this.mTikiTemplePages.NumPages());
			this.mTikiTemplePageControl.Move((int)this.mTitleXOffset + (this.mWidth - this.mTikiTemplePageControl.mWidth) / 2, Common._DS(145));
			this.mTikiTemplePageControl.SetCurrentPage(0);
			this.AddWidget(this.mTikiTemplePageControl);
			this.mTikiTempleScrollWidget.SetPageControl(this.mTikiTemplePageControl);
			this.AddWidget(this.mTikiTempleScrollWidget);
			this.mTikiTempleScrollWidget.SetPageHorizontal(0, false);
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x000A35C8 File Offset: 0x000A17C8
		public override void Update()
		{
			if (!GameApp.gApp.mBambooTransition.IsInProgress() && this.mNeedsInitScroll)
			{
				this.mTikiTempleScrollWidget.SetPageHorizontal(0, true);
				this.mNeedsInitScroll = false;
			}
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				this.mTikiTempleScrollWidget.SetVisible(false);
				return;
			}
			this.mTikiTempleScrollWidget.SetVisible(true);
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x000A3638 File Offset: 0x000A1838
		public float GetTitleXOffset()
		{
			return this.mTitleXOffset;
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x000A3640 File Offset: 0x000A1840
		public override void Draw(Graphics g)
		{
			if (g != null)
			{
				g.Get3D();
			}
			g.Translate(this.mX / 2, 0);
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE, -GameApp.gApp.mWideScreenXOffset, 0, this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE.GetWidth() + 21, this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE.GetHeight());
			g.DrawImageMirror(this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE, -GameApp.gApp.mWideScreenXOffset + this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE.GetWidth() + 21, 0, this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE.GetWidth() + 21, this.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE.GetHeight());
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_BG_FLOOR, 0, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_FLOOR)), GameApp.gApp.GetScreenWidth(), this.IMAGE_UI_CHALLENGESCREEN_BG_FLOOR.GetHeight());
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGLOUNGE28_STROKE));
			int num = Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END));
			int num2 = GameApp.gApp.GetScreenWidth() - num - this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END.GetWidth() + GameApp.gApp.mWideScreenXOffset;
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP, num + this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END.GetWidth(), Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP)), num2 - num - this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END.GetWidth(), this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP.GetHeight());
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END, num, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END)));
			g.DrawImageMirror(this.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END, num2, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END)));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES)) - GameApp.gApp.mWideScreenXOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES)), this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth() + 30, this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetHeight());
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_WOOD, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_WOOD)) - GameApp.gApp.mWideScreenXOffset + this.mAspectOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_WOOD)));
			g.Translate(-this.mX / 2, 0);
			base.DeferOverlay(9);
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x000A3864 File Offset: 0x000A1A64
		public override void DrawOverlay(Graphics g)
		{
			g.Translate(this.mX / 2, 0);
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_BG_SIDE, -GameApp.gApp.mWideScreenXOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_SIDE)));
			g.DrawImageMirror(this.IMAGE_UI_CHALLENGESCREEN_BG_SIDE, GameApp.gApp.GetScreenWidth() + GameApp.gApp.mWideScreenXOffset - this.IMAGE_UI_CHALLENGESCREEN_BG_SIDE.GetWidth(), Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_SIDE)));
			g.DrawImageMirror(this.IMAGE_GUI_TIKITEMPLE_PEDESTAL, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1)) - GameApp.gApp.mWideScreenXOffset - Common._DS(30), Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1)) + this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1.GetHeight() + Common._DS(15));
			g.DrawImage(this.IMAGE_GUI_TIKITEMPLE_PEDESTAL, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2)) - GameApp.gApp.mWideScreenXOffset - Common._DS(20) + this.mAspectOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2)) + this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2.GetHeight() - Common._DS(15));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1)) - GameApp.gApp.mWideScreenXOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1)) + Common._DS(120));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2)) - GameApp.gApp.mWideScreenXOffset + this.mAspectOffset, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2)) + Common._DS(120));
			g.SetColor(255, 255, 255, 255);
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGEXOTICA100_GAUNTLET));
			string @string = TextManager.getInstance().getString(781);
			float num = (float)g.GetFont().StringWidth(@string);
			g.DrawString(@string, (int)this.mTitleXOffset + Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_WOOD)) - GameApp.gApp.mWideScreenXOffset + (int)(((float)this.IMAGE_UI_CHALLENGESCREEN_WOOD.GetWidth() - num) / 2f), Common._DS(135));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_DRUMS, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_DRUMS)) - GameApp.gApp.mWideScreenXOffset + this.mAspectOffset + 85, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_DRUMS)));
			g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_FRUIT, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_FRUIT)) - GameApp.gApp.mWideScreenXOffset - 66, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_FRUIT)));
			g.DrawImage(this.IMAGE_UI_LEADERBOARDS_LEAVES2, Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_LEADERBOARDS_LEAVES2)) - GameApp.gApp.mWideScreenXOffset + GameApp.gApp.GetScreenRect().mX / 2 + this.mAspectOffset + 10, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_LEADERBOARDS_LEAVES2)));
			g.Translate(-this.mX / 2, 0);
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x000A3B72 File Offset: 0x000A1D72
		public void ProcessHardwareBackButton()
		{
			GameApp.gApp.ToggleBambooTransition();
			GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.mMainMenu.HideTikiTemple);
			GameApp.gApp.OnHardwareBackButtonPressProcessed();
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x000A3BAC File Offset: 0x000A1DAC
		public void ButtonDepress(int id)
		{
			if (GameApp.gApp.mBambooTransition != null)
			{
				GameApp.gApp.mBambooTransition.IsInProgress();
			}
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x000A3BCA File Offset: 0x000A1DCA
		public void ButtonPress(int id)
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			GameApp.gApp.PlaySample(1768);
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x000A3BF9 File Offset: 0x000A1DF9
		public void ButtonPress(int theId, int theClickCount)
		{
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x000A3BFB File Offset: 0x000A1DFB
		public void ButtonMouseEnter(int id)
		{
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x000A3BFD File Offset: 0x000A1DFD
		public void ButtonDownTick(int theId)
		{
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x000A3BFF File Offset: 0x000A1DFF
		public void ButtonMouseLeave(int theId)
		{
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x000A3C01 File Offset: 0x000A1E01
		public void ButtonMouseMove(int theId, int theX, int theY)
		{
		}

		// Token: 0x04001718 RID: 5912
		private int mSelectedScreenState;

		// Token: 0x04001719 RID: 5913
		protected ButtonWidget mHomeButton;

		// Token: 0x0400171A RID: 5914
		protected int mDisplayMode;

		// Token: 0x0400171B RID: 5915
		protected int mBounceCount;

		// Token: 0x0400171C RID: 5916
		protected TikiTemplePages mTikiTemplePages;

		// Token: 0x0400171D RID: 5917
		protected PageControl mTikiTemplePageControl;

		// Token: 0x0400171E RID: 5918
		protected ScrollWidget mTikiTempleScrollWidget;

		// Token: 0x0400171F RID: 5919
		protected bool mNeedsInitScroll;

		// Token: 0x04001720 RID: 5920
		protected float mTitleXOffset;

		// Token: 0x04001721 RID: 5921
		protected int mAspectOffset = 30;

		// Token: 0x04001722 RID: 5922
		protected Image IMAGE_UI_CHALLENGESCREEN_HOME_SELECT = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_HOME_SELECT);

		// Token: 0x04001723 RID: 5923
		protected Image IMAGE_UI_CHALLENGESCREEN_HOME = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_HOME);

		// Token: 0x04001724 RID: 5924
		protected Image IMAGE_UI_CHALLENGE_PAGE_INDICATOR = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGE_PAGE_INDICATOR);

		// Token: 0x04001725 RID: 5925
		protected Image IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_CEILING_PIECE);

		// Token: 0x04001726 RID: 5926
		protected Image IMAGE_UI_CHALLENGESCREEN_BG_FLOOR = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_FLOOR);

		// Token: 0x04001727 RID: 5927
		protected Image IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_END);

		// Token: 0x04001728 RID: 5928
		protected Image IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_POLE_BROKEN_UP);

		// Token: 0x04001729 RID: 5929
		protected Image IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES);

		// Token: 0x0400172A RID: 5930
		protected Image IMAGE_UI_CHALLENGESCREEN_WOOD = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_WOOD);

		// Token: 0x0400172B RID: 5931
		protected Image IMAGE_UI_LEADERBOARDS_LEAVES2 = Res.GetImageByID(ResID.IMAGE_UI_LEADERBOARDS_LEAVES2);

		// Token: 0x0400172C RID: 5932
		protected Image IMAGE_UI_CHALLENGESCREEN_BG_SIDE = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_BG_SIDE);

		// Token: 0x0400172D RID: 5933
		protected Image IMAGE_GUI_TIKITEMPLE_PEDESTAL = Res.GetImageByID(ResID.IMAGE_GUI_TIKITEMPLE_PEDESTAL);

		// Token: 0x0400172E RID: 5934
		protected Image IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1 = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_1);

		// Token: 0x0400172F RID: 5935
		protected Image IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2 = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TIKI_POLE_2);

		// Token: 0x04001730 RID: 5936
		protected Image IMAGE_UI_CHALLENGESCREEN_DRUMS = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_DRUMS);

		// Token: 0x04001731 RID: 5937
		protected Image IMAGE_UI_CHALLENGESCREEN_FRUIT = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_FRUIT);

		// Token: 0x04001732 RID: 5938
		protected Image IMAGE_UI_CHALLENGESCREEN_HOME_BACKING = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_HOME_BACKING);

		// Token: 0x04001733 RID: 5939
		public float mXOff;

		// Token: 0x02000149 RID: 329
		private enum ButtonState
		{
			// Token: 0x04001735 RID: 5941
			AdvStats_Btn,
			// Token: 0x04001736 RID: 5942
			HardAdvStats_Btn,
			// Token: 0x04001737 RID: 5943
			Challenge_Btn,
			// Token: 0x04001738 RID: 5944
			IronFrog_Btn,
			// Token: 0x04001739 RID: 5945
			MoreStats_Btn,
			// Token: 0x0400173A RID: 5946
			Back_Btn,
			// Token: 0x0400173B RID: 5947
			Next_Btn,
			// Token: 0x0400173C RID: 5948
			Prev_Btn
		}
	}
}
