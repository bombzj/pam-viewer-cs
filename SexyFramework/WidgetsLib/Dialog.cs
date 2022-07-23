using System;
using SexyFramework.Drivers;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x02000117 RID: 279
	public class Dialog : Widget, ButtonListener
	{
		// Token: 0x060008C5 RID: 2245 RVA: 0x0002C77F File Offset: 0x0002A97F
		public Dialog()
		{
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0002C794 File Offset: 0x0002A994
		public Dialog(Image theComponentImage, Image theButtonComponentImage, int theId, bool isModal, string theDialogHeader, string theDialogLines, string theDialogFooter, int theButtonMode)
		{
			this.mId = theId;
			this.mResult = int.MaxValue;
			this.mComponentImage = theComponentImage;
			this.mStretchBG = false;
			this.mIsModal = isModal;
			this.mContentInsets = new Insets(24, 24, 24, 24);
			this.mTextAlign = 0;
			this.mLineSpacingOffset = 0;
			this.mSpaceAfterHeader = 10;
			this.mButtonSidePadding = 0;
			this.mButtonHorzSpacing = 8;
			this.mDialogListener = GlobalMembers.gSexyAppBase;
			this.mDialogHeader = theDialogHeader;
			this.mDialogFooter = theDialogFooter;
			this.mButtonMode = theButtonMode;
			SexyAppBase gSexyAppBase = GlobalMembers.gSexyAppBase;
			if (this.mButtonMode == 1 || this.mButtonMode == 2)
			{
				this.mYesButton = new DialogButton(theButtonComponentImage, 1000, this);
				this.AddWidget(this.mYesButton);
				this.mNoButton = new DialogButton(theButtonComponentImage, 1001, this);
				this.AddWidget(this.mNoButton);
				this.mYesButton.SetGamepadParent(this);
				this.mNoButton.SetGamepadParent(this);
				this.mYesButton.SetGamepadLinks(null, null, null, this.mNoButton);
				this.mNoButton.SetGamepadLinks(null, null, this.mYesButton, null);
				if (this.mButtonMode == 1)
				{
					this.mYesButton.mLabel = gSexyAppBase.GetString("DIALOG_BUTTON_YES", GlobalMembers.DIALOG_YES_STRING);
					this.mNoButton.mLabel = gSexyAppBase.GetString("DIALOG_BUTTON_NO", GlobalMembers.DIALOG_NO_STRING);
				}
				else
				{
					this.mYesButton.mLabel = gSexyAppBase.GetString("DIALOG_BUTTON_OK", GlobalMembers.DIALOG_OK_STRING);
					this.mNoButton.mLabel = gSexyAppBase.GetString("DIALOG_BUTTON_CANCEL", GlobalMembers.DIALOG_CANCEL_STRING);
				}
			}
			else if (this.mButtonMode == 3)
			{
				this.mYesButton = new DialogButton(theButtonComponentImage, 1000, this);
				this.mYesButton.mLabel = this.mDialogFooter;
				this.mYesButton.SetGamepadParent(this);
				this.AddWidget(this.mYesButton);
				this.mNoButton = null;
			}
			else
			{
				this.mYesButton = null;
				this.mNoButton = null;
				this.mNumButtons = 0;
			}
			this.mDialogLines = theDialogLines;
			this.mButtonHeight = ((theButtonComponentImage == null) ? 24 : theButtonComponentImage.GetCelHeight());
			this.mHasTransparencies = true;
			this.mHasAlpha = true;
			this.mHeaderFont = null;
			this.mLinesFont = null;
			this.mDragging = false;
			this.mPriority = 1;
			if (theButtonComponentImage == null)
			{
				GlobalMembers.gDialogColors[3, 0] = 0;
				GlobalMembers.gDialogColors[3, 1] = 0;
				GlobalMembers.gDialogColors[3, 2] = 0;
				GlobalMembers.gDialogColors[4, 0] = 0;
				GlobalMembers.gDialogColors[4, 1] = 0;
				GlobalMembers.gDialogColors[4, 2] = 0;
			}
			else
			{
				GlobalMembers.gDialogColors[3, 0] = 255;
				GlobalMembers.gDialogColors[3, 1] = 255;
				GlobalMembers.gDialogColors[3, 2] = 255;
				GlobalMembers.gDialogColors[4, 0] = 255;
				GlobalMembers.gDialogColors[4, 1] = 255;
				GlobalMembers.gDialogColors[4, 2] = 255;
			}
			this.SetColors3(GlobalMembers.gDialogColors, 7);
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0002CAB3 File Offset: 0x0002ACB3
		public override void Dispose()
		{
			this.RemoveAllWidgets(true, false);
			if (this.mHeaderFont != null)
			{
				this.mHeaderFont.Dispose();
			}
			if (this.mLinesFont != null)
			{
				this.mLinesFont.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0002CAE9 File Offset: 0x0002ACE9
		public virtual void SetButtonFont(Font theFont)
		{
			if (this.mYesButton != null)
			{
				this.mYesButton.SetFont(theFont);
			}
			if (this.mNoButton != null)
			{
				this.mNoButton.SetFont(theFont);
			}
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0002CB13 File Offset: 0x0002AD13
		public virtual void SetHeaderFont(Font theFont)
		{
			if (this.mHeaderFont != null)
			{
				this.mHeaderFont.Dispose();
			}
			this.mHeaderFont = theFont.Duplicate();
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0002CB34 File Offset: 0x0002AD34
		public virtual void SetLinesFont(Font theFont)
		{
			if (this.mLinesFont != null)
			{
				this.mLinesFont.Dispose();
			}
			this.mLinesFont = theFont.Duplicate();
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0002CB58 File Offset: 0x0002AD58
		public override void SetColor(int theIdx, SexyColor theColor)
		{
			base.SetColor(theIdx, theColor);
			if (theIdx == 3)
			{
				if (this.mYesButton != null)
				{
					this.mYesButton.SetColor(0, theColor);
				}
				if (this.mNoButton != null)
				{
					this.mNoButton.SetColor(0, theColor);
					return;
				}
			}
			else if (theIdx == 4)
			{
				if (this.mYesButton != null)
				{
					this.mYesButton.SetColor(1, theColor);
				}
				if (this.mNoButton != null)
				{
					this.mNoButton.SetColor(1, theColor);
				}
			}
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0002CBCC File Offset: 0x0002ADCC
		public virtual int GetPreferredHeight(int theWidth)
		{
			this.EnsureFonts();
			int num = this.mContentInsets.mTop + this.mContentInsets.mBottom + this.mBackgroundInsets.mTop + this.mBackgroundInsets.mBottom;
			bool flag = false;
			if (this.mDialogHeader.Length > 0 && this.mHeaderFont != null)
			{
				num += this.mHeaderFont.GetHeight() - this.mHeaderFont.GetAscentPadding();
				flag = true;
			}
			if (this.mDialogLines.Length > 0 && this.mLinesFont != null)
			{
				if (flag)
				{
					num += this.mSpaceAfterHeader;
				}
				Graphics graphics = new Graphics();
				graphics.SetFont(this.mLinesFont);
				num += base.GetWordWrappedHeight(graphics, theWidth - this.mContentInsets.mLeft - this.mContentInsets.mRight - this.mBackgroundInsets.mLeft - this.mBackgroundInsets.mRight - 4, this.mDialogLines, this.mLinesFont.GetLineSpacing() + this.mLineSpacingOffset);
				flag = true;
			}
			if (this.mDialogFooter.Length != 0 && this.mButtonMode != 3 && this.mHeaderFont != null)
			{
				if (flag)
				{
					num += 8;
				}
				num += this.mHeaderFont.GetLineSpacing();
				flag = true;
			}
			if (this.mYesButton != null)
			{
				if (flag)
				{
					num += 8;
				}
				num += this.mButtonHeight + 8;
			}
			return num;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0002CD20 File Offset: 0x0002AF20
		public override void Draw(Graphics g)
		{
			this.EnsureFonts();
			Rect rect = new Rect(this.mBackgroundInsets.mLeft, this.mBackgroundInsets.mTop, this.mWidth - this.mBackgroundInsets.mLeft - this.mBackgroundInsets.mRight, this.mHeight - this.mBackgroundInsets.mTop - this.mBackgroundInsets.mBottom);
			if (this.mComponentImage != null)
			{
				if (!this.mStretchBG)
				{
					g.DrawImageBox(rect, this.mComponentImage);
				}
				else
				{
					g.DrawImage(this.mComponentImage, rect, new Rect(0, 0, this.mComponentImage.mWidth, this.mComponentImage.mHeight));
				}
			}
			else
			{
				int theRed = GlobalMembers.gDialogColors[6, 0];
				int theGreen = GlobalMembers.gDialogColors[6, 1];
				int theBlue = GlobalMembers.gDialogColors[6, 1];
				g.SetColor(this.GetColor(6, new SexyColor(theRed, theGreen, theBlue)));
				g.DrawRect(12, 12, this.mWidth - 24 - 1, this.mHeight - 24 - 1);
				int theRed2 = GlobalMembers.gDialogColors[5, 0];
				int theGreen2 = GlobalMembers.gDialogColors[5, 1];
				int theBlue2 = GlobalMembers.gDialogColors[5, 1];
				g.SetColor(this.GetColor(5, new SexyColor(theRed2, theGreen2, theBlue2)));
				g.FillRect(13, 13, this.mWidth - 24 - 2, this.mHeight - 24 - 2);
				g.SetColor(0, 0, 0, 128);
				g.FillRect(this.mWidth - 12, 24, 12, this.mHeight - 36);
				g.FillRect(24, this.mHeight - 12, this.mWidth - 24, 12);
			}
			int num = this.mContentInsets.mTop + this.mBackgroundInsets.mTop;
			if (this.mDialogHeader.Length > 0)
			{
				num += this.mHeaderFont.GetAscent() - this.mHeaderFont.GetAscentPadding();
				g.SetFont(this.mHeaderFont);
				g.SetColor(this.mColors[0]);
				this.WriteCenteredLine(g, num, this.mDialogHeader);
				num += this.mHeaderFont.GetHeight() - this.mHeaderFont.GetAscent();
				num += this.mSpaceAfterHeader;
			}
			g.SetFont(this.mLinesFont);
			g.SetColor(this.mColors[1]);
			Rect theRect = new Rect(this.mBackgroundInsets.mLeft + this.mContentInsets.mLeft + 2, num, this.mWidth - this.mContentInsets.mLeft - this.mContentInsets.mRight - this.mBackgroundInsets.mLeft - this.mBackgroundInsets.mRight - 4, 0);
			num += this.WriteWordWrapped(g, theRect, this.mDialogLines, this.mLinesFont.GetLineSpacing() + this.mLineSpacingOffset, this.mTextAlign);
			if (this.mDialogFooter.Length != 0 && this.mButtonMode != 3)
			{
				num += 8;
				num += this.mHeaderFont.GetLineSpacing();
				g.SetFont(this.mHeaderFont);
				g.SetColor(this.mColors[2]);
				this.WriteCenteredLine(g, num, this.mDialogFooter);
			}
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0002D078 File Offset: 0x0002B278
		public override void Resize(int theX, int theY, int theWidth, int theHeight)
		{
			base.Resize(theX, theY, theWidth, theHeight);
			if (this.mYesButton != null && this.mNoButton != null)
			{
				int num = (this.mWidth - this.mContentInsets.mLeft - this.mContentInsets.mRight - this.mBackgroundInsets.mLeft - this.mBackgroundInsets.mRight - this.mButtonSidePadding * 2 - this.mButtonHorzSpacing) / 2;
				int num2 = this.mButtonHeight;
				this.mYesButton.Resize(this.mBackgroundInsets.mLeft + this.mContentInsets.mLeft + this.mButtonSidePadding, this.mHeight - this.mContentInsets.mBottom - this.mBackgroundInsets.mBottom - num2, num, num2);
				this.mNoButton.Resize(this.mYesButton.mX + num + this.mButtonHorzSpacing, this.mYesButton.mY, num, num2);
				return;
			}
			if (this.mYesButton != null)
			{
				int num3 = this.mButtonHeight;
				this.mYesButton.Resize(this.mContentInsets.mLeft + this.mBackgroundInsets.mLeft, this.mHeight - this.mContentInsets.mBottom - this.mBackgroundInsets.mBottom - num3, this.mWidth - this.mContentInsets.mLeft - this.mContentInsets.mRight - this.mBackgroundInsets.mLeft - this.mBackgroundInsets.mRight, num3);
			}
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0002D1F9 File Offset: 0x0002B3F9
		public override void MouseDown(int x, int y, int theClickCount)
		{
			base.MouseDown(x, y, theClickCount);
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0002D204 File Offset: 0x0002B404
		public override void MouseDown(int x, int y, int theBtnNum, int theClickCount)
		{
			if (theClickCount == 1)
			{
				this.mWidgetManager.mApp.SetCursor(ECURSOR.CURSOR_DRAGGING);
				this.mDragging = true;
				this.mDragMouseX = x;
				this.mDragMouseY = y;
			}
			base.MouseDown(x, y, theBtnNum, theClickCount);
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0002D23C File Offset: 0x0002B43C
		public override void MouseDrag(int x, int y)
		{
			if (this.mDragging)
			{
				int num = this.mX + x - this.mDragMouseX;
				int num2 = this.mY + y - this.mDragMouseY;
				if (num < -8)
				{
					num = -8;
				}
				else if (num + this.mWidth > this.mWidgetManager.mWidth + 8)
				{
					num = this.mWidgetManager.mWidth - this.mWidth + 8;
				}
				if (num2 < -8)
				{
					num2 = -8;
				}
				else if (num2 + this.mHeight > this.mWidgetManager.mHeight + 8)
				{
					num2 = this.mWidgetManager.mHeight - this.mHeight + 8;
				}
				this.mDragMouseX = this.mX + x - num;
				this.mDragMouseY = this.mY + y - num2;
				if (this.mDragMouseX < 8)
				{
					this.mDragMouseX = 8;
				}
				else if (this.mDragMouseX > this.mWidth - 9)
				{
					this.mDragMouseX = this.mWidth - 9;
				}
				if (this.mDragMouseY < 8)
				{
					this.mDragMouseY = 8;
				}
				else if (this.mDragMouseY > this.mHeight - 9)
				{
					this.mDragMouseY = this.mHeight - 9;
				}
				this.Move(num, num2);
			}
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0002D36C File Offset: 0x0002B56C
		public override void MouseUp(int x, int y)
		{
			base.MouseUp(x, y);
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0002D376 File Offset: 0x0002B576
		public override void MouseUp(int x, int y, int theClickCount)
		{
			base.MouseUp(x, y, theClickCount);
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0002D381 File Offset: 0x0002B581
		public override void MouseUp(int x, int y, int theBtnNum, int theClickCount)
		{
			if (this.mDragging)
			{
				this.mWidgetManager.mApp.SetCursor(ECURSOR.CURSOR_POINTER);
				this.mDragging = false;
			}
			base.MouseUp(x, y, theBtnNum, theClickCount);
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0002D3AE File Offset: 0x0002B5AE
		public override void Update()
		{
			base.Update();
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0002D3B6 File Offset: 0x0002B5B6
		public virtual bool IsModal()
		{
			return this.mIsModal;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0002D3BE File Offset: 0x0002B5BE
		public virtual int WaitForResult(bool autoKill)
		{
			if (autoKill)
			{
				GlobalMembers.gSexyAppBase.KillDialog(this.mId);
			}
			return this.mResult;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0002D3DA File Offset: 0x0002B5DA
		public virtual void GameAxisMove(GamepadAxis theAxis, int theMovement, int player)
		{
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0002D3DC File Offset: 0x0002B5DC
		public virtual void GameButtonDown(GamepadButton theButton, int player, uint flags)
		{
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0002D3DE File Offset: 0x0002B5DE
		public virtual void GameButtonUp(GamepadButton theButton, int player, uint flags)
		{
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0002D3E0 File Offset: 0x0002B5E0
		public override void GotFocus()
		{
			base.GotFocus();
			if (this.mYesButton != null)
			{
				this.mWidgetManager.SetGamepadSelection(this.mYesButton, WidgetLinkDir.LINK_DIR_NONE);
				return;
			}
			if (this.mNoButton != null)
			{
				this.mWidgetManager.SetGamepadSelection(this.mNoButton, WidgetLinkDir.LINK_DIR_NONE);
			}
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0002D41D File Offset: 0x0002B61D
		public void EnsureFonts()
		{
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0002D41F File Offset: 0x0002B61F
		public virtual void ButtonPress(int theId)
		{
			this.mDialogListener.DialogButtonPress(this.mId, theId);
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0002D433 File Offset: 0x0002B633
		public virtual void ButtonDepress(int theId)
		{
			this.mResult = theId;
			this.mDialogListener.DialogButtonDepress(this.mId, theId);
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0002D44E File Offset: 0x0002B64E
		public void ButtonPress(int theId, int theClickCount)
		{
			this.ButtonPress(theId);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0002D457 File Offset: 0x0002B657
		public void ButtonDownTick(int theId)
		{
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0002D459 File Offset: 0x0002B659
		public void ButtonMouseEnter(int theId)
		{
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0002D45B File Offset: 0x0002B65B
		public void ButtonMouseLeave(int theId)
		{
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0002D45D File Offset: 0x0002B65D
		public void ButtonMouseMove(int theId, int theX, int theY)
		{
		}

		// Token: 0x040007D1 RID: 2001
		public DialogListener mDialogListener;

		// Token: 0x040007D2 RID: 2002
		public Image mComponentImage;

		// Token: 0x040007D3 RID: 2003
		public bool mStretchBG;

		// Token: 0x040007D4 RID: 2004
		public DialogButton mYesButton;

		// Token: 0x040007D5 RID: 2005
		public DialogButton mNoButton;

		// Token: 0x040007D6 RID: 2006
		public int mNumButtons;

		// Token: 0x040007D7 RID: 2007
		public string mDialogHeader;

		// Token: 0x040007D8 RID: 2008
		public string mDialogFooter;

		// Token: 0x040007D9 RID: 2009
		public string mDialogLines;

		// Token: 0x040007DA RID: 2010
		public int mButtonMode;

		// Token: 0x040007DB RID: 2011
		public Font mHeaderFont;

		// Token: 0x040007DC RID: 2012
		public Font mLinesFont;

		// Token: 0x040007DD RID: 2013
		public int mTextAlign;

		// Token: 0x040007DE RID: 2014
		public int mLineSpacingOffset;

		// Token: 0x040007DF RID: 2015
		public int mButtonHeight;

		// Token: 0x040007E0 RID: 2016
		public Insets mBackgroundInsets = new Insets();

		// Token: 0x040007E1 RID: 2017
		public Insets mContentInsets;

		// Token: 0x040007E2 RID: 2018
		public int mSpaceAfterHeader;

		// Token: 0x040007E3 RID: 2019
		public bool mDragging;

		// Token: 0x040007E4 RID: 2020
		public int mDragMouseX;

		// Token: 0x040007E5 RID: 2021
		public int mDragMouseY;

		// Token: 0x040007E6 RID: 2022
		public int mId;

		// Token: 0x040007E7 RID: 2023
		public bool mIsModal;

		// Token: 0x040007E8 RID: 2024
		public int mResult;

		// Token: 0x040007E9 RID: 2025
		public int mButtonHorzSpacing;

		// Token: 0x040007EA RID: 2026
		public int mButtonSidePadding;

		// Token: 0x02000118 RID: 280
		public enum ButtonLabel
		{
			// Token: 0x040007EC RID: 2028
			BUTTONS_NONE,
			// Token: 0x040007ED RID: 2029
			BUTTONS_YES_NO,
			// Token: 0x040007EE RID: 2030
			BUTTONS_OK_CANCEL,
			// Token: 0x040007EF RID: 2031
			BUTTONS_FOOTER
		}

		// Token: 0x02000119 RID: 281
		public enum ButtonID
		{
			// Token: 0x040007F1 RID: 2033
			ID_YES = 1000,
			// Token: 0x040007F2 RID: 2034
			ID_NO,
			// Token: 0x040007F3 RID: 2035
			ID_OK = 1000,
			// Token: 0x040007F4 RID: 2036
			ID_CANCEL,
			// Token: 0x040007F5 RID: 2037
			ID_FOOTER = 1000
		}

		// Token: 0x0200011A RID: 282
		public enum ButtonColor
		{
			// Token: 0x040007F7 RID: 2039
			COLOR_HEADER,
			// Token: 0x040007F8 RID: 2040
			COLOR_LINES,
			// Token: 0x040007F9 RID: 2041
			COLOR_FOOTER,
			// Token: 0x040007FA RID: 2042
			COLOR_BUTTON_TEXT,
			// Token: 0x040007FB RID: 2043
			COLOR_BUTTON_TEXT_HILITE,
			// Token: 0x040007FC RID: 2044
			COLOR_BKG,
			// Token: 0x040007FD RID: 2045
			COLOR_OUTLINE,
			// Token: 0x040007FE RID: 2046
			NUM_COLORS
		}
	}
}
