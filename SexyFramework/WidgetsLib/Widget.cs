using System;
using System.Collections.Generic;
using SexyFramework.Drivers;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x02000115 RID: 277
	public abstract class Widget : WidgetContainer
	{
		// Token: 0x0600087C RID: 2172 RVA: 0x0002B95C File Offset: 0x00029B5C
		public Widget()
		{
			this.mWidgetManager = null;
			this.mVisible = true;
			this.mDisabled = false;
			this.mIsDown = false;
			this.mIsOver = false;
			this.mDoFinger = false;
			this.mMouseVisible = true;
			this.mHasFocus = false;
			this.mHasTransparencies = false;
			this.mWantsFocus = false;
			this.mTabPrev = null;
			this.mTabNext = null;
			this.mIsGamepadSelection = false;
			this.mGamepadParent = null;
			this.mGamepadLinkUp = null;
			this.mGamepadLinkDown = null;
			this.mGamepadLinkLeft = null;
			this.mGamepadLinkRight = null;
			this.mDataMenuId = -1;
			this.mColors = new List<SexyColor>();
			this.mMouseInsets = new Insets();
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0002BA2C File Offset: 0x00029C2C
		public override void Dispose()
		{
			this.mColors.Clear();
			base.Dispose();
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0002BA40 File Offset: 0x00029C40
		public void CopyFrom(Widget rhs)
		{
			base.CopyFrom(rhs);
			this.mVisible = rhs.mVisible;
			this.mMouseVisible = rhs.mMouseVisible;
			this.mDisabled = rhs.mDisabled;
			this.mHasFocus = rhs.mHasFocus;
			this.mIsDown = rhs.mIsDown;
			this.mIsOver = rhs.mIsOver;
			this.mHasTransparencies = rhs.mHasTransparencies;
			this.mDoFinger = rhs.mDoFinger;
			this.mWantsFocus = rhs.mWantsFocus;
			this.mIsGamepadSelection = rhs.mIsGamepadSelection;
			this.mDataMenuId = rhs.mDataMenuId;
			this.mTabPrev = rhs.mTabPrev;
			this.mTabNext = rhs.mTabNext;
			this.mGamepadParent = rhs.mGamepadParent;
			this.mGamepadLinkUp = rhs.mGamepadLinkUp;
			this.mGamepadLinkDown = rhs.mGamepadLinkDown;
			this.mGamepadLinkLeft = rhs.mGamepadLinkLeft;
			this.mGamepadLinkRight = rhs.mGamepadLinkRight;
			this.mMouseInsets.mLeft = rhs.mMouseInsets.mLeft;
			this.mMouseInsets.mRight = rhs.mMouseInsets.mRight;
			this.mMouseInsets.mBottom = rhs.mMouseInsets.mBottom;
			this.mMouseInsets.mTop = rhs.mMouseInsets.mTop;
			this.mColors.Clear();
			for (int i = 0; i < rhs.mColors.Count; i++)
			{
				this.mColors.Add(new SexyColor(rhs.mColors[i]));
			}
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0002BBC1 File Offset: 0x00029DC1
		public virtual void OrderInManagerChanged()
		{
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0002BBC3 File Offset: 0x00029DC3
		public virtual void SetVisible(bool isVisible)
		{
			if (this.mVisible == isVisible)
			{
				return;
			}
			this.mVisible = isVisible;
			if (this.mVisible)
			{
				this.MarkDirty();
			}
			else
			{
				this.MarkDirtyFull();
			}
			if (this.mWidgetManager != null)
			{
				this.mWidgetManager.RehupMouse();
			}
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0002BC00 File Offset: 0x00029E00
		public virtual void SetColors3(int[,] theColors, int theNumColors)
		{
			this.mColors.Clear();
			for (int i = 0; i < theNumColors; i++)
			{
				this.SetColor(i, new SexyColor(theColors[i, 0], theColors[i, 1], theColors[i, 2]));
			}
			this.MarkDirty();
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0002BC50 File Offset: 0x00029E50
		public virtual void SetColors4(int[,] theColors, int theNumColors)
		{
			this.mColors.Clear();
			for (int i = 0; i < theNumColors; i++)
			{
				this.SetColor(i, new SexyColor(theColors[i, 0], theColors[i, 1], theColors[i, 2], theColors[i, 3]));
			}
			this.MarkDirty();
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0002BCA6 File Offset: 0x00029EA6
		public virtual void SetColor(int theIdx, SexyColor theColor)
		{
			if (theIdx >= this.mColors.Count)
			{
				this.mColors.Capacity = theIdx + 1;
			}
			this.mColors.Add(theColor);
			this.MarkDirty();
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0002BCD6 File Offset: 0x00029ED6
		public virtual SexyColor GetColor(int theIdx)
		{
			if (theIdx < this.mColors.Count)
			{
				return this.mColors[theIdx];
			}
			return this.GetColor_aColor;
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0002BCF9 File Offset: 0x00029EF9
		public virtual SexyColor GetColor(int theIdx, SexyColor theDefaultColor)
		{
			if (theIdx < this.mColors.Count)
			{
				return this.mColors[theIdx];
			}
			return theDefaultColor;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0002BD18 File Offset: 0x00029F18
		public virtual void SetDisabled(bool isDisabled)
		{
			if (this.mDisabled == isDisabled)
			{
				return;
			}
			this.mDisabled = isDisabled;
			if (isDisabled && this.mWidgetManager != null)
			{
				this.mWidgetManager.DisableWidget(this);
			}
			this.MarkDirty();
			if (!isDisabled && this.mWidgetManager != null && this.Contains(this.mWidgetManager.mLastMouseX, this.mWidgetManager.mLastMouseY))
			{
				this.mWidgetManager.MousePosition(this.mWidgetManager.mLastMouseX, this.mWidgetManager.mLastMouseY);
			}
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0002BD9D File Offset: 0x00029F9D
		public virtual void ShowFinger(bool on)
		{
			WidgetManager mWidgetManager = this.mWidgetManager;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0002BDA8 File Offset: 0x00029FA8
		public virtual void Resize(int theX, int theY, int theWidth, int theHeight)
		{
			if (this.mX == theX && this.mY == theY && this.mWidth == theWidth && this.mHeight == theHeight)
			{
				return;
			}
			this.MarkDirtyFull();
			this.mX = theX;
			this.mY = theY;
			this.mWidth = theWidth;
			this.mHeight = theHeight;
			this.MarkDirty();
			if (this.mWidgetManager != null)
			{
				this.mWidgetManager.RehupMouse();
			}
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0002BE17 File Offset: 0x0002A017
		public virtual void Resize(Rect theRect)
		{
			this.Resize(theRect.mX, theRect.mY, theRect.mWidth, theRect.mHeight);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0002BE3B File Offset: 0x0002A03B
		public virtual void Move(int theNewX, int theNewY)
		{
			this.Resize(theNewX, theNewY, this.mWidth, this.mHeight);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0002BE51 File Offset: 0x0002A051
		public virtual bool WantsFocus()
		{
			return this.mWantsFocus;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0002BE59 File Offset: 0x0002A059
		public override void Draw(Graphics g)
		{
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0002BE5B File Offset: 0x0002A05B
		public virtual void DrawOverlay(Graphics g)
		{
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0002BE5D File Offset: 0x0002A05D
		public virtual void DrawOverlay(Graphics g, int thePriority)
		{
			this.DrawOverlay(g);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0002BE66 File Offset: 0x0002A066
		public override void Update()
		{
			base.Update();
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0002BE6E File Offset: 0x0002A06E
		public override void UpdateF(float theFrac)
		{
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0002BE70 File Offset: 0x0002A070
		public virtual void GotFocus()
		{
			this.mHasFocus = true;
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0002BE79 File Offset: 0x0002A079
		public virtual void LostFocus()
		{
			this.mHasFocus = false;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0002BE82 File Offset: 0x0002A082
		public virtual bool IsPointVisible(int x, int y)
		{
			return true;
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0002BE85 File Offset: 0x0002A085
		public virtual void KeyChar(char theChar)
		{
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0002BE88 File Offset: 0x0002A088
		public virtual void KeyDown(KeyCode theKey)
		{
			if (theKey == KeyCode.KEYCODE_TAB)
			{
				if (this.mWidgetManager.mKeyDown[16])
				{
					if (this.mTabPrev != null)
					{
						this.mWidgetManager.SetFocus(this.mTabPrev);
						return;
					}
				}
				else if (this.mTabNext != null)
				{
					this.mWidgetManager.SetFocus(this.mTabNext);
				}
			}
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0002BEDD File Offset: 0x0002A0DD
		public virtual void KeyUp(KeyCode theKey)
		{
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0002BEDF File Offset: 0x0002A0DF
		public virtual void MouseEnter()
		{
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0002BEE1 File Offset: 0x0002A0E1
		public virtual void MouseLeave()
		{
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0002BEE3 File Offset: 0x0002A0E3
		public virtual void MouseMove(int x, int y)
		{
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0002BEE5 File Offset: 0x0002A0E5
		public virtual void MouseDown(int x, int y, int theClickCount)
		{
			if (theClickCount == 3)
			{
				this.MouseDown(x, y, 2, 1);
				return;
			}
			if (theClickCount >= 0)
			{
				this.MouseDown(x, y, 0, theClickCount);
				return;
			}
			this.MouseDown(x, y, 1, -theClickCount);
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0002BF10 File Offset: 0x0002A110
		public virtual void MouseDown(int x, int y, int theBtnNum, int theClickCount)
		{
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0002BF12 File Offset: 0x0002A112
		public virtual void MouseUp(int x, int y)
		{
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0002BF14 File Offset: 0x0002A114
		public virtual void MouseUp(int x, int y, int theLastDownButtonId)
		{
			this.MouseUp(x, y);
			if (theLastDownButtonId == 3)
			{
				this.MouseUp(x, y, 2, 1);
				return;
			}
			if (theLastDownButtonId >= 0)
			{
				this.MouseUp(x, y, 0, theLastDownButtonId);
				return;
			}
			this.MouseUp(x, y, 1, -theLastDownButtonId);
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0002BF47 File Offset: 0x0002A147
		public virtual void MouseUp(int x, int y, int theBtnNum, int theClickCount)
		{
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0002BF49 File Offset: 0x0002A149
		public virtual void MouseDrag(int x, int y)
		{
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0002BF4B File Offset: 0x0002A14B
		public virtual void MouseWheel(int theDelta)
		{
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0002BF50 File Offset: 0x0002A150
		public virtual void TouchBegan(SexyAppBase.Touch touch)
		{
			int mX = touch.location.mX;
			int mY = touch.location.mY;
			this.MouseDown(mX, mY, 1);
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0002BF80 File Offset: 0x0002A180
		public virtual void TouchMoved(SexyAppBase.Touch touch)
		{
			int mX = touch.location.mX;
			int mY = touch.location.mY;
			this.MouseDrag(mX, mY);
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0002BFB0 File Offset: 0x0002A1B0
		public virtual void TouchEnded(SexyAppBase.Touch touch)
		{
			int mX = touch.location.mX;
			int mY = touch.location.mY;
			this.MouseUp(mX, mY, 1);
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0002BFDE File Offset: 0x0002A1DE
		public virtual void TouchesCanceled()
		{
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0002BFE0 File Offset: 0x0002A1E0
		public virtual void SetGamepadLinks(Widget up, Widget down, Widget left, Widget right)
		{
			this.mGamepadLinkUp = up;
			this.mGamepadLinkDown = down;
			this.mGamepadLinkLeft = left;
			this.mGamepadLinkRight = right;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0002BFFF File Offset: 0x0002A1FF
		public virtual void SetGamepadParent(Widget theParent)
		{
			this.mGamepadParent = theParent;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0002C008 File Offset: 0x0002A208
		public virtual void GotGamepadSelection(WidgetLinkDir theDirection)
		{
			this.mIsGamepadSelection = true;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0002C011 File Offset: 0x0002A211
		public virtual void LostGamepadSelection()
		{
			this.mIsGamepadSelection = false;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0002C01C File Offset: 0x0002A21C
		public virtual void GamepadButtonDown(GamepadButton theButton, int thePlayer, uint theFlags)
		{
			switch (theButton)
			{
			case GamepadButton.GAMEPAD_BUTTON_UP:
				if (this.mGamepadLinkUp != null && this.mWidgetManager != null)
				{
					Widget widget = this.mGamepadLinkUp;
					while (widget != null && !widget.mVisible)
					{
						widget = widget.mGamepadLinkUp;
					}
					if (widget != null)
					{
						this.mWidgetManager.SetGamepadSelection(widget, WidgetLinkDir.LINK_DIR_UP);
					}
				}
				break;
			case GamepadButton.GAMEPAD_BUTTON_DOWN:
				if (this.mGamepadLinkDown != null && this.mWidgetManager != null)
				{
					Widget widget2 = this.mGamepadLinkDown;
					while (widget2 != null && !widget2.mVisible)
					{
						widget2 = widget2.mGamepadLinkDown;
					}
					if (widget2 != null)
					{
						this.mWidgetManager.SetGamepadSelection(widget2, WidgetLinkDir.LINK_DIR_DOWN);
					}
				}
				break;
			case GamepadButton.GAMEPAD_BUTTON_LEFT:
				if (this.mGamepadLinkLeft != null && this.mWidgetManager != null)
				{
					Widget widget3 = this.mGamepadLinkLeft;
					while (widget3 != null && !widget3.mVisible)
					{
						widget3 = widget3.mGamepadLinkLeft;
					}
					if (widget3 != null)
					{
						this.mWidgetManager.SetGamepadSelection(widget3, WidgetLinkDir.LINK_DIR_LEFT);
					}
				}
				break;
			case GamepadButton.GAMEPAD_BUTTON_RIGHT:
				if (this.mGamepadLinkRight != null && this.mWidgetManager != null)
				{
					Widget widget4 = this.mGamepadLinkRight;
					while (widget4 != null && !widget4.mVisible)
					{
						widget4 = widget4.mGamepadLinkRight;
					}
					if (widget4 != null)
					{
						this.mWidgetManager.SetGamepadSelection(widget4, WidgetLinkDir.LINK_DIR_RIGHT);
					}
				}
				break;
			}
			if (this.mGamepadParent != null)
			{
				this.mGamepadParent.GamepadButtonDown(theButton, thePlayer, theFlags);
			}
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0002C165 File Offset: 0x0002A365
		public virtual void GamepadButtonUp(GamepadButton theButton, int thePlayer, uint theFlags)
		{
			if (this.mGamepadParent != null)
			{
				this.mGamepadParent.GamepadButtonUp(theButton, thePlayer, theFlags);
			}
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0002C17D File Offset: 0x0002A37D
		public virtual void GamepadAxisMove(GamepadAxis theAxis, int thePlayer, float theAxisValue)
		{
			if (this.mGamepadParent != null)
			{
				this.mGamepadParent.GamepadAxisMove(theAxis, thePlayer, theAxisValue);
			}
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0002C198 File Offset: 0x0002A398
		public virtual Rect WriteCenteredLine(Graphics g, int anOffset, string theLine)
		{
			Font font = g.GetFont();
			int num = font.StringWidth(theLine);
			int theX = (this.mWidth - num) / 2;
			g.DrawString(theLine, theX, anOffset);
			return new Rect(theX, anOffset - font.GetAscent(), num, font.GetHeight());
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0002C1E0 File Offset: 0x0002A3E0
		public virtual int WriteString(Graphics g, string theString, int theX, int theY, int theWidth, int theJustification, bool drawString, int theOffset, int theLength)
		{
			bool flag = g.mWriteColoredString;
			g.mWriteColoredString = Widget.mWriteColoredString;
			int result = g.WriteString(theString, theX, theY, theWidth, theJustification, drawString, theOffset, theLength);
			g.mWriteColoredString = flag;
			return result;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0002C21C File Offset: 0x0002A41C
		public virtual int WriteWordWrapped(Graphics g, Rect theRect, string theLine, int theLineSpacing, int theJustification)
		{
			bool flag = g.mWriteColoredString;
			g.mWriteColoredString = Widget.mWriteColoredString;
			int result = g.WriteWordWrapped(theRect, theLine, theLineSpacing, theJustification);
			g.mWriteColoredString = flag;
			return result;
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0002C250 File Offset: 0x0002A450
		public int GetWordWrappedHeight(Graphics g, int theWidth, string theLine, int aLineSpacing)
		{
			int num = 0;
			int num2 = 0;
			return g.GetWordWrappedHeight(theWidth, theLine, aLineSpacing, ref num, ref num2);
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0002C270 File Offset: 0x0002A470
		public virtual int GetNumDigits(int theNumber)
		{
			int num = 10;
			int num2 = 1;
			while (theNumber >= num)
			{
				num2++;
				num *= 10;
			}
			return num2;
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0002C294 File Offset: 0x0002A494
		public virtual void WriteNumberFromStrip(Graphics g, int theNumber, int theX, int theY, Image theNumberStrip, int aSpacing)
		{
			int num = 10;
			int num2 = 1;
			while (theNumber >= num)
			{
				num2++;
				num *= 10;
			}
			if (theNumber == 0)
			{
				num = 10;
			}
			int num3 = theNumberStrip.GetWidth() / 10;
			for (int i = 0; i < num2; i++)
			{
				num /= 10;
				int num4 = theNumber / num % 10;
				g.PushState();
				g.ClipRect(theX + i * (num3 + aSpacing), theY, num3, theNumberStrip.GetHeight());
				g.DrawImage(theNumberStrip, theX + i * (num3 + aSpacing) - num4 * num3, theY);
				g.PopState();
			}
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0002C31A File Offset: 0x0002A51A
		public virtual bool Contains(int theX, int theY)
		{
			return theX >= this.mX && theX < this.mX + this.mWidth && theY >= this.mY && theY < this.mY + this.mHeight;
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0002C350 File Offset: 0x0002A550
		public virtual Rect GetInsetRect()
		{
			return new Rect(this.mX + this.mMouseInsets.mLeft, this.mY + this.mMouseInsets.mTop, this.mWidth - this.mMouseInsets.mLeft - this.mMouseInsets.mRight, this.mHeight - this.mMouseInsets.mTop - this.mMouseInsets.mBottom);
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0002C3C2 File Offset: 0x0002A5C2
		public void DeferOverlay(int thePriority)
		{
			this.mWidgetManager.DeferOverlay(this, thePriority);
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0002C3D4 File Offset: 0x0002A5D4
		public void WidgetRemovedHelper()
		{
			if (this.mWidgetManager == null)
			{
				return;
			}
			foreach (Widget widget in this.mWidgets)
			{
				widget.WidgetRemovedHelper();
			}
			this.mWidgetManager.DisableWidget(this);
			foreach (PreModalInfo preModalInfo in this.mWidgetManager.mPreModalInfoList)
			{
				if (preModalInfo.mPrevBaseModalWidget == this)
				{
					preModalInfo.mPrevBaseModalWidget = null;
				}
				if (preModalInfo.mPrevFocusWidget == this)
				{
					preModalInfo.mPrevFocusWidget = null;
				}
			}
			this.RemovedFromManager(this.mWidgetManager);
			this.MarkDirtyFull(this);
			if (this.mWidgetManager.GetGamepadSelection() == this)
			{
				this.mWidgetManager.SetGamepadSelection(null, WidgetLinkDir.LINK_DIR_NONE);
			}
			this.mWidgetManager = null;
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0002C492 File Offset: 0x0002A692
		public int Left()
		{
			return this.mX;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0002C49A File Offset: 0x0002A69A
		public int Top()
		{
			return this.mY;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0002C4A2 File Offset: 0x0002A6A2
		public int Right()
		{
			return this.mX + this.mWidth;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0002C4B1 File Offset: 0x0002A6B1
		public int Bottom()
		{
			return this.mY + this.mHeight;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0002C4C0 File Offset: 0x0002A6C0
		public int Width()
		{
			return this.mWidth;
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0002C4C8 File Offset: 0x0002A6C8
		public int Height()
		{
			return this.mHeight;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0002C4D0 File Offset: 0x0002A6D0
		public void Layout(int theLayoutFlags, Widget theRelativeWidget, int theLeftPad, int theTopPad, int theWidthPad, int theHeightPad)
		{
			int num = theRelativeWidget.Left();
			int num2 = theRelativeWidget.Top();
			if (theRelativeWidget == this.mParent)
			{
				num = 0;
				num2 = 0;
			}
			int num3 = theRelativeWidget.Width();
			int num4 = theRelativeWidget.Height();
			int num5 = num + num3;
			int num6 = num2 + num4;
			int num7 = this.Left();
			int num8 = this.Top();
			int num9 = this.Width();
			int num10 = this.Height();
			for (int i = 1; i < 4194304; i <<= 1)
			{
				if ((theLayoutFlags & i) != 0)
				{
					LayoutFlags layoutFlags = (LayoutFlags)i;
					if (layoutFlags <= LayoutFlags.LAY_Left)
					{
						if (layoutFlags <= LayoutFlags.LAY_SetWidth)
						{
							if (layoutFlags <= LayoutFlags.LAY_SetLeft)
							{
								switch (layoutFlags)
								{
								case LayoutFlags.LAY_SameWidth:
									num9 = num3 + theWidthPad;
									break;
								case LayoutFlags.LAY_SameHeight:
									num10 = num4 + theHeightPad;
									break;
								default:
									if (layoutFlags == LayoutFlags.LAY_SetLeft)
									{
										num7 = theLeftPad;
									}
									break;
								}
							}
							else if (layoutFlags != LayoutFlags.LAY_SetTop)
							{
								if (layoutFlags == LayoutFlags.LAY_SetWidth)
								{
									num9 = theWidthPad;
								}
							}
							else
							{
								num8 = theTopPad;
							}
						}
						else if (layoutFlags <= LayoutFlags.LAY_Above)
						{
							if (layoutFlags != LayoutFlags.LAY_SetHeight)
							{
								if (layoutFlags == LayoutFlags.LAY_Above)
								{
									num8 = num2 - num10 + theTopPad;
								}
							}
							else
							{
								num10 = theHeightPad;
							}
						}
						else if (layoutFlags != LayoutFlags.LAY_Below)
						{
							if (layoutFlags != LayoutFlags.LAY_Right)
							{
								if (layoutFlags == LayoutFlags.LAY_Left)
								{
									num7 = num - num9 + theLeftPad;
								}
							}
							else
							{
								num7 = num5 + theLeftPad;
							}
						}
						else
						{
							num8 = num6 + theTopPad;
						}
					}
					else if (layoutFlags <= LayoutFlags.LAY_GrowToRight)
					{
						if (layoutFlags <= LayoutFlags.LAY_SameRight)
						{
							if (layoutFlags != LayoutFlags.LAY_SameLeft)
							{
								if (layoutFlags == LayoutFlags.LAY_SameRight)
								{
									num7 = num5 - num9 + theLeftPad;
								}
							}
							else
							{
								num7 = num + theLeftPad;
							}
						}
						else if (layoutFlags != LayoutFlags.LAY_SameTop)
						{
							if (layoutFlags != LayoutFlags.LAY_SameBottom)
							{
								if (layoutFlags == LayoutFlags.LAY_GrowToRight)
								{
									num9 = num5 - num7 + theWidthPad;
								}
							}
							else
							{
								num8 = num6 - num10 + theTopPad;
							}
						}
						else
						{
							num8 = num2 + theTopPad;
						}
					}
					else if (layoutFlags <= LayoutFlags.LAY_GrowToTop)
					{
						if (layoutFlags != LayoutFlags.LAY_GrowToLeft)
						{
							if (layoutFlags == LayoutFlags.LAY_GrowToTop)
							{
								num10 = num2 - num8 + theHeightPad;
							}
						}
						else
						{
							num9 = num - num7 + theWidthPad;
						}
					}
					else if (layoutFlags != LayoutFlags.LAY_GrowToBottom)
					{
						if (layoutFlags != LayoutFlags.LAY_HCenter)
						{
							if (layoutFlags == LayoutFlags.LAY_VCenter)
							{
								num8 = num2 + (num4 - num10) / 2 + theTopPad;
							}
						}
						else
						{
							num7 = num + (num3 - num9) / 2 + theLeftPad;
						}
					}
					else
					{
						num10 = num6 - num8 + theHeightPad;
					}
				}
			}
			this.Resize(num7, num8, num9, num10);
		}

		// Token: 0x040007BA RID: 1978
		public bool mVisible;

		// Token: 0x040007BB RID: 1979
		public bool mMouseVisible;

		// Token: 0x040007BC RID: 1980
		public bool mDisabled;

		// Token: 0x040007BD RID: 1981
		public bool mHasFocus;

		// Token: 0x040007BE RID: 1982
		public bool mIsDown;

		// Token: 0x040007BF RID: 1983
		public bool mIsOver;

		// Token: 0x040007C0 RID: 1984
		public bool mHasTransparencies;

		// Token: 0x040007C1 RID: 1985
		public bool mDoFinger;

		// Token: 0x040007C2 RID: 1986
		public bool mWantsFocus;

		// Token: 0x040007C3 RID: 1987
		public bool mIsGamepadSelection;

		// Token: 0x040007C4 RID: 1988
		public int mDataMenuId;

		// Token: 0x040007C5 RID: 1989
		public List<SexyColor> mColors = new List<SexyColor>();

		// Token: 0x040007C6 RID: 1990
		public Insets mMouseInsets = new Insets();

		// Token: 0x040007C7 RID: 1991
		public Widget mTabPrev;

		// Token: 0x040007C8 RID: 1992
		public Widget mTabNext;

		// Token: 0x040007C9 RID: 1993
		public Widget mGamepadParent;

		// Token: 0x040007CA RID: 1994
		public Widget mGamepadLinkUp;

		// Token: 0x040007CB RID: 1995
		public Widget mGamepadLinkDown;

		// Token: 0x040007CC RID: 1996
		public Widget mGamepadLinkLeft;

		// Token: 0x040007CD RID: 1997
		public Widget mGamepadLinkRight;

		// Token: 0x040007CE RID: 1998
		public static bool mWriteColoredString = true;

		// Token: 0x040007CF RID: 1999
		private SexyColor GetColor_aColor = default(SexyColor);

		// Token: 0x040007D0 RID: 2000
		public bool mIsFinishDrawOverlay;
	}
}
