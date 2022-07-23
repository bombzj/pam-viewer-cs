using System;
using SexyFramework.Drivers;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001D2 RID: 466
	public class Slider : Widget
	{
		// Token: 0x060010E0 RID: 4320 RVA: 0x00054F90 File Offset: 0x00053190
		public Slider(Image theTrackImage, Image theThumbImage, int theId, SliderListener theListener)
		{
			this.mTrackImage = theTrackImage;
			this.mThumbImage = theThumbImage;
			this.mId = theId;
			this.mListener = theListener;
			this.mVal = 0.0;
			this.mOutlineColor = new SexyColor(SexyColor.White);
			this.mBkgColor = new SexyColor(80, 80, 80);
			this.mSliderColor = new SexyColor(SexyColor.White);
			this.mKnobSize = 5;
			this.mDragging = false;
			this.mHorizontal = true;
			this.mRelX = (this.mRelY = 0);
			this.mSlideSpeed = 0.01f;
			this.mSlidingLeft = false;
			this.mSlidingRight = false;
			this.mStepSound = -1;
			this.mStepMode = false;
			this.mNumSteps = 1;
			this.mCurStep = 0;
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00055080 File Offset: 0x00053280
		public virtual void SetValue(double theValue)
		{
			double num = this.mVal;
			this.mVal = theValue;
			if (this.mVal < 0.0)
			{
				this.mVal = 0.0;
			}
			else if (this.mVal > 1.0)
			{
				this.mVal = 1.0;
			}
			if (this.mVal != num)
			{
				this.mListener.SliderVal(this.mId, this.mVal);
			}
			this.MarkDirtyFull();
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00055103 File Offset: 0x00053303
		public virtual void SetStepMode(int num_steps, int cur_step, int step_sound)
		{
			this.mStepMode = true;
			this.mNumSteps = num_steps;
			this.SetStepValue(cur_step);
			this.mStepSound = step_sound;
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x00055124 File Offset: 0x00053324
		public virtual void SetStepValue(int cur_step)
		{
			if (cur_step < 0)
			{
				cur_step = 0;
			}
			if (cur_step > this.mNumSteps)
			{
				cur_step = this.mNumSteps;
			}
			if (this.mCurStep != cur_step)
			{
				this.mCurStep = cur_step;
				this.SetValue((double)cur_step / (double)this.mNumSteps);
				if (this.mStepSound != -1)
				{
					GlobalMembers.gSexyApp.PlaySample(this.mStepSound);
				}
			}
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00055184 File Offset: 0x00053384
		public override void Update()
		{
			base.Update();
			if (this.mIsGamepadSelection)
			{
				if (this.mSlidingLeft)
				{
					this.SetValue(this.mVal - (double)this.mSlideSpeed);
				}
				if (this.mSlidingRight)
				{
					this.SetValue(this.mVal + (double)this.mSlideSpeed);
					return;
				}
			}
			else
			{
				this.mSlidingLeft = false;
				this.mSlidingRight = false;
			}
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x000551E6 File Offset: 0x000533E6
		public virtual bool HasTransparencies()
		{
			return true;
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x000551EC File Offset: 0x000533EC
		public override void Draw(Graphics g)
		{
			if (this.mTrackImage != null)
			{
				int num = (this.mHorizontal ? (this.mTrackImage.GetWidth() / 3) : this.mTrackImage.GetWidth());
				int num2 = (this.mHorizontal ? this.mTrackImage.GetHeight() : (this.mTrackImage.GetHeight() / 3));
				Rect theSrcRect = new Rect(0, 0, num, num2);
				if (this.mHorizontal)
				{
					int theY = (this.mHeight - num2) / 2;
					g.DrawImage(this.mTrackImage, 0, theY, theSrcRect);
					g.PushState();
					g.ClipRect(num, theY, this.mWidth - num * 2, num2);
					for (int i = 0; i < (this.mWidth - num * 2 + num - 1) / num; i++)
					{
						g.DrawImage(this.mTrackImage, num + i * num, theY, new Rect(num, 0, num, num2));
					}
					g.PopState();
					g.DrawImage(this.mTrackImage, this.mWidth - num, theY, new Rect(num * 2, 0, num, num2));
				}
				else
				{
					int theX = (this.mWidth - num) / 2;
					g.DrawImage(this.mTrackImage, theX, 0, theSrcRect);
					g.PushState();
					g.ClipRect(theX, num2, num, this.mHeight - num2 * 2);
					for (int j = 0; j < (this.mHeight - num2 * 2 + num2 - 1) / num2; j++)
					{
						g.DrawImage(this.mTrackImage, theX, num2 + j * num2, theSrcRect);
					}
					g.PopState();
					g.DrawImage(this.mTrackImage, theX, this.mHeight - num2, theSrcRect);
				}
			}
			else if (this.mTrackImage == null)
			{
				g.SetColor(this.mOutlineColor);
				g.FillRect(0, 0, this.mWidth, this.mHeight);
				g.SetColor(this.mBkgColor);
				g.FillRect(1, 1, this.mWidth - 2, this.mHeight - 2);
			}
			if (this.mHorizontal && this.mThumbImage != null)
			{
				g.DrawImage(this.mThumbImage, (int)(this.mVal * (double)(this.mWidth - this.mThumbImage.GetCelWidth())), (this.mHeight - this.mThumbImage.GetCelHeight()) / 2);
				return;
			}
			if (!this.mHorizontal && this.mThumbImage != null)
			{
				g.DrawImage(this.mThumbImage, (this.mWidth - this.mThumbImage.GetCelWidth()) / 2, (int)(this.mVal * (double)(this.mHeight - this.mThumbImage.GetCelHeight())));
				return;
			}
			if (this.mThumbImage == null)
			{
				g.SetColor(this.mSliderColor);
				if (this.mHorizontal)
				{
					g.FillRect((int)(this.mVal * (double)(this.mWidth - this.mKnobSize)), 0, this.mKnobSize, this.mHeight);
					return;
				}
				g.FillRect(0, (int)(this.mVal * (double)(this.mHeight - this.mKnobSize)), this.mWidth, this.mKnobSize);
			}
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x000554D8 File Offset: 0x000536D8
		public override Rect GetInsetRect()
		{
			return new Rect(this.mX + this.mMouseInsets.mLeft - 6, this.mY + this.mMouseInsets.mTop - 7, this.mWidth - this.mMouseInsets.mLeft - this.mMouseInsets.mRight + 12, this.mHeight - this.mMouseInsets.mTop - this.mMouseInsets.mBottom + 14);
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00055554 File Offset: 0x00053754
		public override void MouseMove(int x, int y)
		{
			if (this.mHorizontal)
			{
				int num = ((this.mThumbImage == null) ? this.mKnobSize : this.mThumbImage.GetCelWidth());
				int num2 = (int)(this.mVal * (double)(this.mWidth - num));
				if (x >= num2 && x < num2 + num)
				{
					this.mWidgetManager.mApp.SetCursor(ECURSOR.CURSOR_DRAGGING);
					return;
				}
				this.mWidgetManager.mApp.SetCursor(ECURSOR.CURSOR_POINTER);
				return;
			}
			else
			{
				int num3 = ((this.mThumbImage == null) ? this.mKnobSize : this.mThumbImage.GetCelHeight());
				int num4 = (int)(this.mVal * (double)(this.mHeight - num3));
				if (y >= num4 && y < num4 + num3)
				{
					this.mWidgetManager.mApp.SetCursor(ECURSOR.CURSOR_DRAGGING);
					return;
				}
				this.mWidgetManager.mApp.SetCursor(ECURSOR.CURSOR_POINTER);
				return;
			}
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00055620 File Offset: 0x00053820
		public override void MouseDown(int x, int y, int theClickCount)
		{
			if (this.mHorizontal)
			{
				int num = ((this.mThumbImage == null) ? this.mKnobSize : this.mThumbImage.GetCelWidth());
				int num2 = (int)(this.mVal * (double)(this.mWidth - num));
				this.mDragging = true;
				if (x >= num2 - 2 && x < num2 + num + 2)
				{
					this.mWidgetManager.mApp.SetCursor(ECURSOR.CURSOR_DRAGGING);
					this.mRelX = x - num2;
					return;
				}
				double value = (double)x / (double)this.mWidth;
				this.SetValue(value);
				return;
			}
			else
			{
				int num3 = ((this.mThumbImage == null) ? this.mKnobSize : this.mThumbImage.GetCelHeight());
				int num4 = (int)(this.mVal * (double)(this.mHeight - num3));
				if (y >= num4 && y < num4 + num3)
				{
					this.mWidgetManager.mApp.SetCursor(ECURSOR.CURSOR_DRAGGING);
					this.mDragging = true;
					this.mRelY = y - num4;
					return;
				}
				double value2 = (double)y / (double)this.mHeight;
				this.SetValue(value2);
				return;
			}
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x00055718 File Offset: 0x00053918
		public override void MouseDrag(int x, int y)
		{
			if (this.mDragging)
			{
				int num = ((this.mThumbImage == null) ? this.mKnobSize : this.mThumbImage.GetCelWidth());
				int mWidth = this.mWidth;
				double num2 = this.mVal;
				if (this.mHorizontal)
				{
					this.mVal = (double)(x - this.mRelX) / (double)(this.mWidth - num);
				}
				else
				{
					int num3 = ((this.mThumbImage == null) ? this.mKnobSize : this.mThumbImage.GetCelHeight());
					this.mVal = (double)(y - this.mRelY) / (double)(this.mHeight - num3);
				}
				if (this.mVal < 0.0)
				{
					this.mVal = 0.0;
				}
				if (this.mVal > 1.0)
				{
					this.mVal = 1.0;
				}
				if (this.mVal != num2)
				{
					this.mListener.SliderVal(this.mId, this.mVal);
					this.MarkDirtyFull();
				}
			}
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x00055818 File Offset: 0x00053A18
		public override void MouseUp(int x, int y)
		{
			this.mDragging = false;
			this.mWidgetManager.mApp.SetCursor(ECURSOR.CURSOR_POINTER);
			this.mListener.SliderVal(this.mId, this.mVal);
			this.mListener.SliderReleased(this.mId, this.mVal);
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0005586B File Offset: 0x00053A6B
		public override void MouseLeave()
		{
			if (!this.mDragging)
			{
				this.mWidgetManager.mApp.SetCursor(ECURSOR.CURSOR_POINTER);
			}
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00055888 File Offset: 0x00053A88
		public override void GamepadButtonDown(GamepadButton theButton, int player, uint flags)
		{
			switch (theButton)
			{
			case GamepadButton.GAMEPAD_BUTTON_LEFT:
				if (this.mStepMode)
				{
					this.SetStepValue(this.mCurStep - 1);
					return;
				}
				this.mSlidingLeft = true;
				return;
			case GamepadButton.GAMEPAD_BUTTON_RIGHT:
				if (this.mStepMode)
				{
					this.SetStepValue(this.mCurStep + 1);
					return;
				}
				this.mSlidingRight = true;
				return;
			default:
				base.GamepadButtonDown(theButton, player, flags);
				return;
			}
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x000558F0 File Offset: 0x00053AF0
		public override void GamepadButtonUp(GamepadButton theButton, int player, uint flags)
		{
			switch (theButton)
			{
			case GamepadButton.GAMEPAD_BUTTON_LEFT:
				this.mSlidingLeft = false;
				return;
			case GamepadButton.GAMEPAD_BUTTON_RIGHT:
				this.mSlidingRight = false;
				return;
			default:
				base.GamepadButtonUp(theButton, player, flags);
				return;
			}
		}

		// Token: 0x04000DB4 RID: 3508
		public SliderListener mListener;

		// Token: 0x04000DB5 RID: 3509
		public double mVal;

		// Token: 0x04000DB6 RID: 3510
		public int mId;

		// Token: 0x04000DB7 RID: 3511
		public Image mTrackImage;

		// Token: 0x04000DB8 RID: 3512
		public Image mThumbImage;

		// Token: 0x04000DB9 RID: 3513
		public bool mDragging;

		// Token: 0x04000DBA RID: 3514
		public int mRelX;

		// Token: 0x04000DBB RID: 3515
		public int mRelY;

		// Token: 0x04000DBC RID: 3516
		public bool mHorizontal;

		// Token: 0x04000DBD RID: 3517
		public float mSlideSpeed;

		// Token: 0x04000DBE RID: 3518
		public bool mSlidingLeft;

		// Token: 0x04000DBF RID: 3519
		public bool mSlidingRight;

		// Token: 0x04000DC0 RID: 3520
		public bool mStepMode;

		// Token: 0x04000DC1 RID: 3521
		public int mNumSteps;

		// Token: 0x04000DC2 RID: 3522
		public int mCurStep;

		// Token: 0x04000DC3 RID: 3523
		public int mStepSound;

		// Token: 0x04000DC4 RID: 3524
		public int mKnobSize;

		// Token: 0x04000DC5 RID: 3525
		public SexyColor mOutlineColor = default(SexyColor);

		// Token: 0x04000DC6 RID: 3526
		public SexyColor mBkgColor = default(SexyColor);

		// Token: 0x04000DC7 RID: 3527
		public SexyColor mSliderColor = default(SexyColor);
	}
}
