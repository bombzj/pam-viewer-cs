using System;
using SexyFramework.GraphicsLib;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000151 RID: 337
	public class ZumaSlider : Slider
	{
		// Token: 0x1700039D RID: 925
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x000A74E2 File Offset: 0x000A56E2
		// (set) Token: 0x0600106E RID: 4206 RVA: 0x000A74EC File Offset: 0x000A56EC
		public string Label
		{
			get
			{
				return this.mLabel;
			}
			set
			{
				this.mLabel = value;
				Font fontByID = Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_BASE);
				this.mLabelWidth = fontByID.StringWidth(this.mLabel);
			}
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x000A751C File Offset: 0x000A571C
		public ZumaSlider(int id, SliderListener listener, string label)
			: base(Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BOX_MAINMENU_THUMB), Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BOX_MAINMENU_SLIDER), id, listener)
		{
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_BASE);
			this.mFeedbackSoundID = -1;
			this.mLabel = label;
			this.mLabelWidth = fontByID.StringWidth(this.mLabel);
			this.mHasAlpha = (this.mHasTransparencies = true);
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x000A7580 File Offset: 0x000A5780
		public override void Draw(Graphics g)
		{
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_BASE);
			g.PushState();
			g.ClearClipRect();
			g.SetFont(fontByID);
			g.SetColor(255, 255, 64, 255);
			int num = Common._S(Common._M(20));
			int num2 = Common._S(Common._M(-35));
			g.DrawString(this.mLabel, (this.mWidth + num - this.mLabelWidth) / 2, g.mFont.mAscent + this.mHeight + num2 - Common._S(Common._M(12)) - g.mFont.mHeight);
			g.PopState();
			base.Draw(g);
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x000A7630 File Offset: 0x000A5830
		public override void MouseEnter()
		{
			base.MouseEnter();
			this.MarkDirty();
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x000A763E File Offset: 0x000A583E
		public override void MouseLeave()
		{
			base.MouseLeave();
			this.MarkDirty();
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x000A764C File Offset: 0x000A584C
		public override void MouseUp(int x, int y)
		{
			base.MouseUp(x, y);
			if (this.mFeedbackSoundID >= 0)
			{
				GameApp.gApp.PlaySample(this.mFeedbackSoundID);
			}
		}

		// Token: 0x04001ABE RID: 6846
		public string mLabel;

		// Token: 0x04001ABF RID: 6847
		public int mLabelWidth;

		// Token: 0x04001AC0 RID: 6848
		public int mFeedbackSoundID;
	}
}
