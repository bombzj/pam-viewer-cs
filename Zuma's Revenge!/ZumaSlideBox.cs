using System;
using JeffLib;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000153 RID: 339
	public class ZumaSlideBox : Widget, ScrollWidgetListener
	{
		// Token: 0x06001076 RID: 4214 RVA: 0x000A773C File Offset: 0x000A593C
		public ZumaSlideBox(DialogEx theDialog, int id, string label)
		{
			this.mLabel = label;
			this.mDialog = theDialog;
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BOX_MAINMENU_RED_LIGHT);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BOX_MAINMENU_SLIDEBOXBACK);
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_GAUNTLET);
			Rect theRect = default(Rect);
			theRect.mX = 0;
			theRect.mY = 0;
			theRect.mWidth = imageByID.GetWidth() * 2;
			theRect.mHeight = imageByID.GetHeight();
			this.mLabelFrame = default(Rect);
			this.mLabelFrame.mWidth = imageByID2.GetWidth() - theRect.mWidth - Common._S(9);
			this.mLabelFrame.mHeight = imageByID2.GetHeight();
			this.mLabelFrame.mX = 0;
			this.mLabelFrame.mY = (int)((float)(this.mLabelFrame.mHeight - fontByID.GetHeight()) * 0.5f);
			this.mSlideBoxButton = new ZumaSlideBoxButton(this);
			this.mSlideBoxButton.Resize(theRect);
			this.mScrollBox = new ScrollWidget(this);
			this.mScrollBox.Resize(this.mLabelFrame.mWidth, (this.mLabelFrame.mHeight - theRect.mHeight) / 2, theRect.mWidth, theRect.mHeight);
			this.mScrollBox.AddWidget(this.mSlideBoxButton);
			this.mScrollBox.SetScrollMode(ScrollWidget.ScrollMode.SCROLL_HORIZONTAL);
			this.mScrollBox.EnablePaging(true);
			this.AddWidget(this.mScrollBox);
			Insets insets = new Insets();
			insets.mLeft = 0;
			insets.mRight = this.mSlideBoxButton.mWidth / 2;
			insets.mTop = 0;
			insets.mBottom = 0;
			this.mScrollBox.SetScrollInsets(insets);
			this.mScrollBox.SetPageHorizontal(0, false);
			this.mScrollBox.EnableBounce(false);
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x000A790C File Offset: 0x000A5B0C
		~ZumaSlideBox()
		{
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x000A7934 File Offset: 0x000A5B34
		public override void Draw(Graphics g)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BOX_MAINMENU_SLIDEBOXBACK);
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_GAUNTLET);
			g.DrawImage(imageByID, 0, 0);
			g.SetFont(fontByID);
			g.SetColor(255, 255, 45, 255);
			g.WriteWordWrapped(this.mLabelFrame, this.mLabel, -1, 0);
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x000A7990 File Offset: 0x000A5B90
		public override void DrawOverlay(Graphics g)
		{
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x000A7992 File Offset: 0x000A5B92
		public void ScrollTargetReached(ScrollWidget scrollWidget)
		{
			this.mIsOff = scrollWidget.GetPageHorizontal() == 1;
			GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BUTTON1));
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x000A79BB File Offset: 0x000A5BBB
		public void ScrollTargetInterrupted(ScrollWidget scrollWidget)
		{
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x000A79BD File Offset: 0x000A5BBD
		public void SetOnOff(bool isOn)
		{
			this.mIsOff = !isOn;
			this.mScrollBox.SetPageHorizontal(this.mIsOff ? 1 : 0, false);
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x000A79E1 File Offset: 0x000A5BE1
		public bool IsOn()
		{
			return !this.mIsOff;
		}

		// Token: 0x04001AC2 RID: 6850
		public Rect mLabelFrame;

		// Token: 0x04001AC3 RID: 6851
		public string mLabel;

		// Token: 0x04001AC4 RID: 6852
		public bool mIsOff;

		// Token: 0x04001AC5 RID: 6853
		public ScrollWidget mScrollBox;

		// Token: 0x04001AC6 RID: 6854
		public ZumaSlideBoxButton mSlideBoxButton;

		// Token: 0x04001AC7 RID: 6855
		public DialogEx mDialog;
	}
}
