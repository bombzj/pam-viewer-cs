using System;
using SexyFramework.Drivers;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x0200011C RID: 284
	public class ButtonWidget : Widget
	{
		// Token: 0x060008EA RID: 2282 RVA: 0x0002D630 File Offset: 0x0002B830
		public ButtonWidget(int theId, ButtonListener theButtonListener)
		{
			this.mId = theId;
			this.mFont = null;
			this.mLabelJustify = 0;
			this.mButtonImage = null;
			this.mIconImage = null;
			this.mOverImage = null;
			this.mDownImage = null;
			this.mDisabledImage = null;
			this.mInverted = false;
			this.mBtnNoDraw = false;
			this.mFrameNoDraw = false;
			this.mButtonListener = theButtonListener;
			this.mHasAlpha = true;
			this.mOverAlpha = 0.0;
			this.mOverAlphaSpeed = 0.0;
			this.mOverAlphaFadeInSpeed = 0.0;
			this.mLabelOffsetX = (this.mLabelOffsetY = 0);
			this.SetColors3(GlobalMembers.gButtonWidgetColors, 6);
			this.mLastPressedBy = -1;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0002D71E File Offset: 0x0002B91E
		public override void Dispose()
		{
			if (this.mFont != null)
			{
				this.mFont.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0002D739 File Offset: 0x0002B939
		public virtual void SetFont(Font theFont)
		{
			if (this.mFont != null)
			{
				this.mFont.Dispose();
			}
			this.mFont = theFont.Duplicate();
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0002D75A File Offset: 0x0002B95A
		public virtual bool IsButtonDown()
		{
			return this.mIsDown && this.mIsOver && !this.mDisabled;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0002D778 File Offset: 0x0002B978
		public override void Draw(Graphics g)
		{
			if (this.mBtnNoDraw)
			{
				return;
			}
			bool flag = this.IsButtonDown();
			flag ^= this.mInverted;
			int num = this.mLabelOffsetX;
			int num2 = this.mLabelOffsetY;
			if (this.mFont != null)
			{
				if (this.mLabelJustify == 0)
				{
					num += (this.mWidth - this.mFont.StringWidth(this.mLabel)) / 2;
				}
				else if (this.mLabelJustify == 1)
				{
					num += this.mWidth - this.mFont.StringWidth(this.mLabel);
				}
				num2 += (this.mHeight + this.mFont.GetAscent() - this.mFont.GetAscent() / 6 - 1) / 2;
			}
			int theX = 0;
			int theY = 0;
			if (this.mIconImage != null)
			{
				if (this.mLabelJustify == 0)
				{
					theX = (this.mWidth - this.mIconImage.GetWidth()) / 2 + this.mLabelOffsetX;
				}
				else if (this.mLabelJustify == 1)
				{
					theX = this.mWidth - this.mIconImage.GetWidth();
				}
				theY = (this.mHeight - this.mIconImage.GetHeight()) / 2 + this.mLabelOffsetY;
			}
			g.SetFont(this.mFont);
			if (this.mButtonImage == null && this.mDownImage == null)
			{
				if (!this.mFrameNoDraw)
				{
					g.SetColor(this.mColors[5]);
					g.FillRect(0, 0, this.mWidth, this.mHeight);
				}
				if (flag)
				{
					if (!this.mFrameNoDraw)
					{
						g.SetColor(this.mColors[2]);
						g.FillRect(0, 0, this.mWidth - 1, 1);
						g.FillRect(0, 0, 1, this.mHeight - 1);
						g.SetColor(this.mColors[3]);
						g.FillRect(0, this.mHeight - 1, this.mWidth, 1);
						g.FillRect(this.mWidth - 1, 0, 1, this.mHeight);
						g.SetColor(this.mColors[4]);
						g.FillRect(1, 1, this.mWidth - 3, 1);
						g.FillRect(1, 1, 1, this.mHeight - 3);
					}
					if (this.mIsOver)
					{
						g.SetColor(this.mColors[1]);
					}
					else
					{
						g.SetColor(this.mColors[0]);
					}
					if (this.mIconImage == null)
					{
						g.DrawString(this.mLabel, num, num2);
						return;
					}
					g.DrawImage(this.mIconImage, theX, theY);
					return;
				}
				else
				{
					if (!this.mFrameNoDraw)
					{
						g.SetColor(this.mColors[3]);
						g.FillRect(0, 0, this.mWidth - 1, 1);
						g.FillRect(0, 0, 1, this.mHeight - 1);
						g.SetColor(this.mColors[2]);
						g.FillRect(0, this.mHeight - 1, this.mWidth, 1);
						g.FillRect(this.mWidth - 1, 0, 1, this.mHeight);
						g.SetColor(this.mColors[4]);
						g.FillRect(1, this.mHeight - 2, this.mWidth - 2, 1);
						g.FillRect(this.mWidth - 2, 1, 1, this.mHeight - 2);
					}
					if (this.mIsOver)
					{
						g.SetColor(this.mColors[1]);
					}
					else
					{
						g.SetColor(this.mColors[0]);
					}
					if (this.mIconImage == null)
					{
						g.DrawString(this.mLabel, num, num2);
						return;
					}
					g.DrawImage(this.mIconImage, theX, theY);
					return;
				}
			}
			else if (!flag)
			{
				if (this.mDisabled && this.HaveButtonImage(this.mDisabledImage, this.mDisabledRect))
				{
					this.DrawButtonImage(g, this.mDisabledImage, this.mDisabledRect, 0, 0);
				}
				else if (this.mOverAlpha > 0.0 && this.HaveButtonImage(this.mOverImage, this.mOverRect))
				{
					if (this.HaveButtonImage(this.mButtonImage, this.mNormalRect) && this.mOverAlpha < 1.0)
					{
						this.DrawButtonImage(g, this.mButtonImage, this.mNormalRect, 0, 0);
					}
					g.SetColorizeImages(true);
					g.SetColor(new SexyColor(255, 255, 255, (int)(this.mOverAlpha * 255.0)));
					this.DrawButtonImage(g, this.mOverImage, this.mOverRect, 0, 0);
					g.SetColorizeImages(false);
				}
				else if ((this.mIsOver || this.mIsDown) && this.HaveButtonImage(this.mOverImage, this.mOverRect))
				{
					this.DrawButtonImage(g, this.mOverImage, this.mOverRect, 0, 0);
				}
				else if (this.HaveButtonImage(this.mButtonImage, this.mNormalRect))
				{
					this.DrawButtonImage(g, this.mButtonImage, this.mNormalRect, 0, 0);
				}
				if (this.mIsOver)
				{
					g.SetColor(this.mColors[1]);
				}
				else
				{
					g.SetColor(this.mColors[0]);
				}
				if (this.mIconImage == null)
				{
					g.DrawString(this.mLabel, num, num2);
					return;
				}
				g.DrawImage(this.mIconImage, theX, theY);
				return;
			}
			else
			{
				if (this.HaveButtonImage(this.mDownImage, this.mDownRect))
				{
					this.DrawButtonImage(g, this.mDownImage, this.mDownRect, 0, 0);
				}
				else if (this.HaveButtonImage(this.mOverImage, this.mOverRect))
				{
					this.DrawButtonImage(g, this.mOverImage, this.mOverRect, 0, 0);
				}
				else
				{
					this.DrawButtonImage(g, this.mButtonImage, this.mNormalRect, 0, 0);
				}
				g.SetColor(this.mColors[1]);
				if (this.mIconImage == null)
				{
					g.DrawString(this.mLabel, num, num2);
					return;
				}
				g.DrawImage(this.mIconImage, theX, theY);
				return;
			}
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0002DD4F File Offset: 0x0002BF4F
		public override void SetDisabled(bool isDisabled)
		{
			base.SetDisabled(isDisabled);
			if (this.HaveButtonImage(this.mDisabledImage, this.mDisabledRect))
			{
				this.MarkDirty();
			}
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0002DD74 File Offset: 0x0002BF74
		public override void MouseEnter()
		{
			base.MouseEnter();
			if (this.mOverAlphaFadeInSpeed == 0.0 && this.mOverAlpha > 0.0)
			{
				this.mOverAlpha = 0.0;
			}
			if (this.mIsDown || this.HaveButtonImage(this.mOverImage, this.mOverRect) || this.mColors[1] != this.mColors[0])
			{
				this.MarkDirty();
			}
			this.MarkDirty();
			this.mButtonListener.ButtonMouseEnter(this.mId);
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0002DE10 File Offset: 0x0002C010
		public override void MouseLeave()
		{
			base.MouseLeave();
			if (this.mOverAlphaSpeed == 0.0 && this.mOverAlpha > 0.0)
			{
				this.mOverAlpha = 0.0;
			}
			else if (this.mOverAlphaSpeed > 0.0 && this.mOverAlpha == 0.0 && this.mWidgetManager.mApp.mHasFocus)
			{
				this.mOverAlpha = Math.Min(1.0, this.mOverAlphaSpeed * 10.0);
			}
			if (this.mIsDown || this.HaveButtonImage(this.mOverImage, this.mOverRect) || this.mColors[1] != this.mColors[0])
			{
				this.MarkDirty();
			}
			this.mButtonListener.ButtonMouseLeave(this.mId);
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0002DF00 File Offset: 0x0002C100
		public override void MouseMove(int theX, int theY)
		{
			base.MouseMove(theX, theY);
			this.mButtonListener.ButtonMouseMove(this.mId, theX, theY);
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0002DF1D File Offset: 0x0002C11D
		public override void MouseDown(int theX, int theY, int theClickCount)
		{
			base.MouseDown(theX, theY, theClickCount);
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0002DF28 File Offset: 0x0002C128
		public override void MouseDown(int theX, int theY, int theBtnNum, int theClickCount)
		{
			base.MouseDown(theX, theY, theBtnNum, theClickCount);
			this.mButtonListener.ButtonPress(this.mId, theClickCount);
			this.MarkDirty();
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0002DF4E File Offset: 0x0002C14E
		public override void MouseUp(int theX, int theY)
		{
			base.MouseUp(theX, theY);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0002DF58 File Offset: 0x0002C158
		public override void MouseUp(int theX, int theY, int theClickCount)
		{
			base.MouseUp(theX, theY, theClickCount);
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0002DF63 File Offset: 0x0002C163
		public override void MouseUp(int theX, int theY, int theBtnNum, int theClickCount)
		{
			base.MouseUp(theX, theY, theBtnNum, theClickCount);
			if (this.mIsOver && this.mWidgetManager.mHasFocus)
			{
				this.mButtonListener.ButtonDepress(this.mId);
			}
			this.MarkDirty();
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0002DF9C File Offset: 0x0002C19C
		public override void Update()
		{
			base.Update();
			if (this.mIsDown && this.mIsOver)
			{
				this.mButtonListener.ButtonDownTick(this.mId);
			}
			if (!this.mIsDown && !this.mIsOver && this.mOverAlpha > 0.0)
			{
				if (this.mOverAlphaSpeed > 0.0)
				{
					this.mOverAlpha -= this.mOverAlphaSpeed;
					if (this.mOverAlpha < 0.0)
					{
						this.mOverAlpha = 0.0;
					}
				}
				else
				{
					this.mOverAlpha = 0.0;
				}
				this.MarkDirty();
				return;
			}
			if (this.mIsOver && this.mOverAlphaFadeInSpeed > 0.0 && this.mOverAlpha < 1.0)
			{
				this.mOverAlpha += this.mOverAlphaFadeInSpeed;
				if (this.mOverAlpha > 1.0)
				{
					this.mOverAlpha = 1.0;
				}
				this.MarkDirty();
			}
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0002E0B0 File Offset: 0x0002C2B0
		public override void GotGamepadSelection(WidgetLinkDir theDirection)
		{
			base.GotGamepadSelection(theDirection);
			this.mIsOver = true;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0002E0C0 File Offset: 0x0002C2C0
		public override void LostGamepadSelection()
		{
			base.LostGamepadSelection();
			this.mIsOver = false;
			this.mIsDown = false;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0002E0D8 File Offset: 0x0002C2D8
		public override void GamepadButtonDown(GamepadButton theButton, int thePlayer, uint theFlags)
		{
			if (theButton != GamepadButton.GAMEPAD_BUTTON_A)
			{
				if (this.mIsDown)
				{
					if (this.mGamepadParent != null)
					{
						this.mGamepadParent.GamepadButtonDown(theButton, thePlayer, theFlags);
						return;
					}
				}
				else
				{
					base.GamepadButtonDown(theButton, thePlayer, theFlags);
				}
				return;
			}
			if ((theFlags & 1U) != 0U)
			{
				return;
			}
			this.mLastPressedBy = thePlayer;
			this.OnPressed();
			this.mIsDown = true;
			if (this.mButtonListener != null)
			{
				this.mButtonListener.ButtonPress(this.mId, 1);
			}
			this.MarkDirty();
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0002E14C File Offset: 0x0002C34C
		public override void GamepadButtonUp(GamepadButton theButton, int thePlayer, uint theFlags)
		{
			if (theButton == GamepadButton.GAMEPAD_BUTTON_A)
			{
				if (this.mIsDown)
				{
					this.mLastPressedBy = thePlayer;
					if (this.mButtonListener != null)
					{
						this.mButtonListener.ButtonDepress(this.mId);
					}
					this.mIsDown = false;
					this.MarkDirty();
					return;
				}
			}
			else
			{
				base.GamepadButtonUp(theButton, thePlayer, theFlags);
			}
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0002E19C File Offset: 0x0002C39C
		public virtual void OnPressed()
		{
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0002E19E File Offset: 0x0002C39E
		public virtual bool HaveButtonImage(Image theImage, Rect theRect)
		{
			return theImage != null || theRect.mWidth != 0;
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0002E1B2 File Offset: 0x0002C3B2
		public virtual void DrawButtonImage(Graphics g, Image theImage, Rect theRect, int x, int y)
		{
			if (theRect.mWidth != 0)
			{
				g.DrawImage(theImage, x, y, theRect);
				return;
			}
			g.DrawImage(theImage, x, y);
		}

		// Token: 0x04000801 RID: 2049
		public int mId;

		// Token: 0x04000802 RID: 2050
		public string mLabel;

		// Token: 0x04000803 RID: 2051
		public int mLabelJustify;

		// Token: 0x04000804 RID: 2052
		public Font mFont;

		// Token: 0x04000805 RID: 2053
		public Image mButtonImage;

		// Token: 0x04000806 RID: 2054
		public Image mIconImage;

		// Token: 0x04000807 RID: 2055
		public Image mOverImage;

		// Token: 0x04000808 RID: 2056
		public Image mDownImage;

		// Token: 0x04000809 RID: 2057
		public Image mDisabledImage;

		// Token: 0x0400080A RID: 2058
		public Rect mNormalRect = default(Rect);

		// Token: 0x0400080B RID: 2059
		public Rect mOverRect = default(Rect);

		// Token: 0x0400080C RID: 2060
		public Rect mDownRect = default(Rect);

		// Token: 0x0400080D RID: 2061
		public Rect mDisabledRect = default(Rect);

		// Token: 0x0400080E RID: 2062
		public bool mInverted;

		// Token: 0x0400080F RID: 2063
		public bool mBtnNoDraw;

		// Token: 0x04000810 RID: 2064
		public bool mFrameNoDraw;

		// Token: 0x04000811 RID: 2065
		public ButtonListener mButtonListener;

		// Token: 0x04000812 RID: 2066
		public int mLastPressedBy;

		// Token: 0x04000813 RID: 2067
		public double mOverAlpha;

		// Token: 0x04000814 RID: 2068
		public double mOverAlphaSpeed;

		// Token: 0x04000815 RID: 2069
		public double mOverAlphaFadeInSpeed;

		// Token: 0x04000816 RID: 2070
		public int mLabelOffsetX;

		// Token: 0x04000817 RID: 2071
		public int mLabelOffsetY;

		// Token: 0x0200011D RID: 285
		public enum ButtonLabel
		{
			// Token: 0x04000819 RID: 2073
			BUTTON_LABEL_LEFT = -1,
			// Token: 0x0400081A RID: 2074
			BUTTON_LABEL_CENTER,
			// Token: 0x0400081B RID: 2075
			BUTTON_LABEL_RIGHT
		}

		// Token: 0x0200011E RID: 286
		public enum ButtonColor
		{
			// Token: 0x0400081D RID: 2077
			COLOR_LABEL,
			// Token: 0x0400081E RID: 2078
			COLOR_LABEL_HILITE,
			// Token: 0x0400081F RID: 2079
			COLOR_DARK_OUTLINE,
			// Token: 0x04000820 RID: 2080
			COLOR_LIGHT_OUTLINE,
			// Token: 0x04000821 RID: 2081
			COLOR_MEDIUM_OUTLINE,
			// Token: 0x04000822 RID: 2082
			COLOR_BKG,
			// Token: 0x04000823 RID: 2083
			NUM_COLORS
		}
	}
}
