using System;
using System.Collections.Generic;
using SexyFramework.Drivers;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001D8 RID: 472
	public class WidgetManager : WidgetContainer
	{
		// Token: 0x060010F2 RID: 4338 RVA: 0x00055940 File Offset: 0x00053B40
		public WidgetManager(SexyAppBase theApp)
		{
			this.mApp = theApp;
			this.mMinDeferredOverlayPriority = int.MaxValue;
			this.mWidgetManager = this;
			this.mMouseIn = false;
			this.mDefaultTab = null;
			this.mImage = null;
			this.mLastHadTransients = false;
			this.mPopupCommandWidget = null;
			this.mFocusWidget = null;
			this.mLastDownWidget = null;
			this.mOverWidget = null;
			this.mBaseModalWidget = null;
			this.mGamepadSelectionWidget = null;
			this.mDefaultBelowModalFlagsMod.mRemoveFlags = 48;
			this.mWidth = 0;
			this.mHeight = 0;
			this.mHasFocus = true;
			this.mUpdateCnt = 0;
			this.mLastDownButtonId = 0;
			this.mDownButtons = 0;
			this.mActualDownButtons = 0;
			this.mWidgetFlags = 61;
			for (int i = 0; i < 255; i++)
			{
				this.mKeyDown[i] = false;
			}
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00055A70 File Offset: 0x00053C70
		public override void Dispose()
		{
			this.FreeResources();
			base.Dispose();
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00055A7E File Offset: 0x00053C7E
		public void FreeResources()
		{
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x00055A80 File Offset: 0x00053C80
		public void AddBaseModal(Widget theWidget, FlagsMod theBelowFlagsMod)
		{
			PreModalInfo preModalInfo = new PreModalInfo();
			preModalInfo.mBaseModalWidget = theWidget;
			preModalInfo.mPrevBaseModalWidget = this.mBaseModalWidget;
			preModalInfo.mPrevFocusWidget = this.mFocusWidget;
			preModalInfo.mPrevBelowModalFlagsMod = this.mBelowModalFlagsMod;
			this.mPreModalInfoList.AddLast(preModalInfo);
			this.SetBaseModal(theWidget, theBelowFlagsMod);
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x00055AD3 File Offset: 0x00053CD3
		public void AddBaseModal(Widget theWidget)
		{
			this.AddBaseModal(theWidget, this.mDefaultBelowModalFlagsMod);
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00055AE4 File Offset: 0x00053CE4
		public void RemoveBaseModal(Widget theWidget)
		{
			bool flag = true;
			while (this.mPreModalInfoList.Count > 0)
			{
				PreModalInfo value = this.mPreModalInfoList.Last.Value;
				if (flag && value.mBaseModalWidget != theWidget)
				{
					return;
				}
				bool flag2 = value.mPrevBaseModalWidget != null || this.mPreModalInfoList.Count == 1;
				this.SetBaseModal(value.mPrevBaseModalWidget, value.mPrevBelowModalFlagsMod);
				if (this.mFocusWidget == null)
				{
					this.mFocusWidget = value.mPrevFocusWidget;
					if (this.mFocusWidget != null)
					{
						this.mFocusWidget.GotFocus();
					}
				}
				this.mPreModalInfoList.RemoveLast();
				if (flag2)
				{
					return;
				}
				flag = false;
			}
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00055B8B File Offset: 0x00053D8B
		public void Resize(Rect theMouseDestRect, Rect theMouseSourceRect)
		{
			this.mWidth = theMouseDestRect.mWidth + 2 * theMouseDestRect.mX;
			this.mHeight = theMouseDestRect.mHeight + 2 * theMouseDestRect.mY;
			this.mMouseDestRect = theMouseDestRect;
			this.mMouseSourceRect = theMouseSourceRect;
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00055BCC File Offset: 0x00053DCC
		public new void DisableWidget(Widget theWidget)
		{
			if (this.mOverWidget == theWidget)
			{
				Widget theWidget2 = this.mOverWidget;
				this.mOverWidget = null;
				this.MouseLeave(theWidget2);
			}
			if (this.mLastDownWidget == theWidget)
			{
				Widget theWidget3 = this.mLastDownWidget;
				this.mLastDownWidget = null;
				this.DoMouseUps(theWidget3, this.mDownButtons);
				this.mDownButtons = 0;
			}
			if (this.mFocusWidget == theWidget)
			{
				Widget widget = this.mFocusWidget;
				this.mFocusWidget = null;
				widget.LostFocus();
			}
			if (this.mBaseModalWidget == theWidget)
			{
				this.mBaseModalWidget = null;
			}
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x00055C50 File Offset: 0x00053E50
		public Widget GetAnyWidgetAt(int x, int y, ref int theWidgetX, ref int theWidgetY)
		{
			bool flag = false;
			return base.GetWidgetAtHelper(x, y, this.GetWidgetFlags(), ref flag, ref theWidgetX, ref theWidgetY);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00055C74 File Offset: 0x00053E74
		public Widget GetWidgetAt(int x, int y, ref int theWidgetX, ref int theWidgetY)
		{
			Widget widget = this.GetAnyWidgetAt(x, y, ref theWidgetX, ref theWidgetY);
			if (widget != null && widget.mDisabled)
			{
				widget = null;
			}
			return widget;
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00055C9C File Offset: 0x00053E9C
		public new void SetFocus(Widget aWidget)
		{
			if (aWidget == this.mFocusWidget)
			{
				return;
			}
			if (this.mFocusWidget != null)
			{
				this.mFocusWidget.LostFocus();
			}
			if (aWidget != null && aWidget.mWidgetManager == this)
			{
				this.mFocusWidget = aWidget;
				if (this.mHasFocus && this.mFocusWidget != null)
				{
					this.mFocusWidget.GotFocus();
					return;
				}
			}
			else
			{
				this.mFocusWidget = null;
			}
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x00055CFC File Offset: 0x00053EFC
		public void GotFocus()
		{
			if (!this.mHasFocus)
			{
				this.mHasFocus = true;
				if (this.mFocusWidget != null)
				{
					this.mFocusWidget.GotFocus();
				}
			}
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x00055D20 File Offset: 0x00053F20
		public void LostFocus()
		{
			if (!this.mHasFocus)
			{
				this.mActualDownButtons = 0;
				for (int i = 0; i < 255; i++)
				{
					if (this.mKeyDown[i])
					{
						this.KeyUp((KeyCode)i);
					}
				}
				this.mHasFocus = false;
				if (this.mFocusWidget != null)
				{
					this.mFocusWidget.LostFocus();
				}
			}
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00055D78 File Offset: 0x00053F78
		public void InitModalFlags(ModalFlags theModalFlags)
		{
			theModalFlags.mIsOver = this.mBaseModalWidget == null;
			theModalFlags.mOverFlags = this.GetWidgetFlags();
			theModalFlags.mUnderFlags = FlagsMod.GetModFlags(theModalFlags.mOverFlags, this.mBelowModalFlagsMod);
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00055DAC File Offset: 0x00053FAC
		public void DrawWidgetsTo(Graphics g)
		{
			g.Translate(this.mMouseDestRect.mX, this.mMouseDestRect.mY);
			this.mCurG = new Graphics(g);
			List<KeyValuePair<Widget, int>> list = this.mDeferredOverlayWidgets;
			this.mDeferredOverlayWidgets.Clear();
			ModalFlags modalFlags = new ModalFlags();
			this.InitModalFlags(modalFlags);
			foreach (Widget widget in this.mWidgets)
			{
				if (widget.mVisible)
				{
					g.PushState();
					g.SetFastStretch(!g.Is3D());
					g.SetLinearBlend(g.Is3D());
					g.Translate(-this.mMouseDestRect.mX, -this.mMouseDestRect.mY);
					g.Translate(widget.mX, widget.mY);
					widget.DrawAll(modalFlags, g);
					g.PopState();
				}
			}
			this.FlushDeferredOverlayWidgets(int.MaxValue);
			this.mDeferredOverlayWidgets = list;
			this.mCurG = null;
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00055EAC File Offset: 0x000540AC
		public void DoMouseUps(Widget theWidget, int theDownCode)
		{
			int[] array = new int[] { 1, -1, 3 };
			for (int i = 0; i < 3; i++)
			{
				if ((theDownCode & (1 << i)) != 0)
				{
					theWidget.mIsDown = false;
					theWidget.MouseUp(this.mLastMouseX - theWidget.mX, this.mLastMouseY - theWidget.mY, array[i]);
				}
			}
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00055F0B File Offset: 0x0005410B
		public void DoMouseUps()
		{
			if (this.mLastDownWidget != null && this.mDownButtons != 0)
			{
				this.DoMouseUps(this.mLastDownWidget, this.mDownButtons);
				this.mDownButtons = 0;
				this.mLastDownWidget = null;
			}
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00055F3D File Offset: 0x0005413D
		public void DeferOverlay(Widget theWidget, int thePriority)
		{
			theWidget.mIsFinishDrawOverlay = false;
			this.mDeferredOverlayWidgets.Add(new KeyValuePair<Widget, int>(theWidget, thePriority));
			if (thePriority < this.mMinDeferredOverlayPriority)
			{
				this.mMinDeferredOverlayPriority = thePriority;
			}
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00055F68 File Offset: 0x00054168
		public void FlushDeferredOverlayWidgets(int theMaxPriority)
		{
			if (this.mCurG == null)
			{
				return;
			}
			Graphics graphics = new Graphics(this.mCurG);
			while (this.mMinDeferredOverlayPriority <= theMaxPriority)
			{
				int num = int.MaxValue;
				for (int i = 0; i < this.mDeferredOverlayWidgets.Count; i++)
				{
					Widget key = this.mDeferredOverlayWidgets[i].Key;
					if (key != null && !key.mIsFinishDrawOverlay)
					{
						int value = this.mDeferredOverlayWidgets[i].Value;
						if (value == this.mMinDeferredOverlayPriority)
						{
							graphics.PushState();
							graphics.Translate(-this.mMouseDestRect.mX, -this.mMouseDestRect.mY);
							graphics.Translate(key.mX, key.mY);
							graphics.SetFastStretch(graphics.Is3D());
							graphics.SetLinearBlend(graphics.Is3D());
							this.mDeferredOverlayWidgets[i].Key.mIsFinishDrawOverlay = true;
							key.DrawOverlay(graphics, value);
							graphics.PopState();
						}
						else if (value < num)
						{
							num = value;
						}
					}
				}
				this.mMinDeferredOverlayPriority = num;
				if (num == 2147483647)
				{
					this.mDeferredOverlayWidgets.Clear();
					return;
				}
			}
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x000560A4 File Offset: 0x000542A4
		public bool DrawScreen()
		{
			ModalFlags modalFlags = new ModalFlags();
			this.InitModalFlags(modalFlags);
			bool result = false;
			this.mMinDeferredOverlayPriority = int.MaxValue;
			this.mDeferredOverlayWidgets.Clear();
			Graphics theGraphics = new Graphics(this.mImage);
			this.mCurG = theGraphics;
			DeviceImage deviceImage = null;
			bool flag = false;
			if (this.mImage != null)
			{
				deviceImage = this.mImage.AsDeviceImage();
				if (deviceImage != null)
				{
					flag = deviceImage.LockSurface();
				}
			}
			Graphics graphics = new Graphics(theGraphics);
			graphics.Translate(-this.mMouseDestRect.mX, -this.mMouseDestRect.mY);
			bool flag2 = this.mApp.Is3DAccelerated();
			foreach (Widget widget in this.mWidgets)
			{
				if (widget == this.mWidgetManager.mBaseModalWidget)
				{
					modalFlags.mIsOver = true;
				}
				if (widget.mVisible)
				{
					graphics.PushState();
					graphics.SetFastStretch(!flag2);
					graphics.SetLinearBlend(flag2);
					graphics.Translate(widget.mX, widget.mY);
					widget.DrawAll(modalFlags, graphics);
					result = true;
					widget.mDirty = false;
					graphics.PopState();
				}
			}
			this.FlushDeferredOverlayWidgets(int.MaxValue);
			if (deviceImage != null && flag)
			{
				deviceImage.UnlockSurface();
			}
			this.mCurG = null;
			return result;
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x000561F0 File Offset: 0x000543F0
		public bool UpdateFrame()
		{
			ModalFlags modalFlags = new ModalFlags();
			this.InitModalFlags(modalFlags);
			this.mUpdateCnt++;
			this.mLastWMUpdateCount = this.mUpdateCnt;
			this.UpdateAll(modalFlags);
			return this.mDirty;
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00056234 File Offset: 0x00054434
		public bool UpdateFrameF(float theFrac)
		{
			ModalFlags modalFlags = new ModalFlags();
			this.InitModalFlags(modalFlags);
			this.UpdateFAll(modalFlags, theFrac);
			return this.mDirty;
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x0005625C File Offset: 0x0005445C
		public void SetPopupCommandWidget(Widget theList)
		{
			this.mPopupCommandWidget = theList;
			this.AddWidget(this.mPopupCommandWidget);
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00056274 File Offset: 0x00054474
		public void RemovePopupCommandWidget()
		{
			if (this.mPopupCommandWidget != null)
			{
				Widget theWidget = this.mPopupCommandWidget;
				this.mPopupCommandWidget = null;
				this.RemoveWidget(theWidget);
			}
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x000562A0 File Offset: 0x000544A0
		public void MousePosition(int x, int y)
		{
			int num = this.mLastMouseX;
			int num2 = this.mLastMouseY;
			this.mLastMouseX = x;
			this.mLastMouseY = y;
			if (this.mLastMouseX == -1 && this.mLastMouseY == -1)
			{
				return;
			}
			int x2 = 0;
			int y2 = 0;
			Widget widgetAt = this.GetWidgetAt(x, y, ref x2, ref y2);
			if (widgetAt != this.mOverWidget)
			{
				Widget widget = this.mOverWidget;
				this.mOverWidget = null;
				if (widget != null)
				{
					this.MouseLeave(widget);
				}
				this.mOverWidget = widgetAt;
				if (widgetAt != null)
				{
					this.MouseEnter(widgetAt);
					widgetAt.MouseMove(x2, y2);
					return;
				}
			}
			else if ((num != x || num2 != y) && widgetAt != null)
			{
				widgetAt.MouseMove(x2, y2);
			}
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00056348 File Offset: 0x00054548
		public void RehupMouse()
		{
			if (this.mLastDownWidget != null)
			{
				if (this.mOverWidget != null)
				{
					int num = 0;
					int num2 = 0;
					Widget widgetAt = this.GetWidgetAt(this.mLastMouseX, this.mLastMouseY, ref num, ref num2);
					if (widgetAt != this.mLastDownWidget)
					{
						Widget theWidget = this.mOverWidget;
						this.mOverWidget = null;
						this.MouseLeave(theWidget);
						return;
					}
				}
			}
			else if (this.mMouseIn)
			{
				this.MousePosition(this.mLastMouseX, this.mLastMouseY);
			}
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x000563BC File Offset: 0x000545BC
		public void RemapMouse(ref int theX, ref int theY)
		{
			if (this.mMouseSourceRect.mWidth == 0 || this.mMouseSourceRect.mHeight == 0)
			{
				return;
			}
			theX = (theX - this.mMouseSourceRect.mX) * this.mMouseDestRect.mWidth / this.mMouseSourceRect.mWidth + this.mMouseDestRect.mX;
			theY = (theY - this.mMouseSourceRect.mY) * this.mMouseDestRect.mHeight / this.mMouseSourceRect.mHeight + this.mMouseDestRect.mY;
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x0005644C File Offset: 0x0005464C
		public bool MouseUp(int x, int y, int theClickCount)
		{
			this.mLastInputUpdateCnt = this.mUpdateCnt;
			int num;
			if (theClickCount < 0)
			{
				num = 2;
			}
			else if (theClickCount == 3)
			{
				num = 4;
			}
			else
			{
				num = 1;
			}
			this.mActualDownButtons &= ~num;
			if (this.mLastDownWidget != null && (this.mDownButtons & num) != 0)
			{
				Widget widget = this.mLastDownWidget;
				this.mDownButtons &= ~num;
				if (this.mDownButtons == 0)
				{
					this.mLastDownWidget = null;
				}
				widget.mIsDown = false;
				SexyPoint absPos = widget.GetAbsPos();
				widget.MouseUp(x - absPos.mX, y - absPos.mY, theClickCount);
			}
			else
			{
				this.mDownButtons &= ~num;
			}
			this.MousePosition(x, y);
			return true;
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x000564FC File Offset: 0x000546FC
		public bool MouseDown(int x, int y, int theClickCount)
		{
			this.mLastInputUpdateCnt = this.mUpdateCnt;
			if (theClickCount < 0)
			{
				this.mActualDownButtons |= 2;
			}
			else if (theClickCount == 3)
			{
				this.mActualDownButtons |= 4;
			}
			else
			{
				this.mActualDownButtons |= 1;
			}
			this.MousePosition(x, y);
			if (this.mPopupCommandWidget != null && this.mPopupCommandWidget.Contains(x, y))
			{
				this.RemovePopupCommandWidget();
			}
			int x2 = 0;
			int y2 = 0;
			Widget widgetAt = this.GetWidgetAt(x, y, ref x2, ref y2);
			if (this.mLastDownWidget != null)
			{
				widgetAt = this.mLastDownWidget;
			}
			if (theClickCount < 0)
			{
				this.mLastDownButtonId = -1;
				this.mDownButtons |= 2;
			}
			else if (theClickCount == 3)
			{
				this.mLastDownButtonId = 2;
				this.mDownButtons |= 4;
			}
			else
			{
				this.mLastDownButtonId = 1;
				this.mDownButtons |= 1;
			}
			this.mLastDownWidget = widgetAt;
			if (widgetAt != null)
			{
				if (widgetAt.WantsFocus())
				{
					this.SetFocus(widgetAt);
				}
				widgetAt.mIsDown = true;
				widgetAt.MouseDown(x2, y2, theClickCount);
			}
			return true;
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00056605 File Offset: 0x00054805
		public bool MouseMove(int x, int y)
		{
			this.mLastInputUpdateCnt = this.mUpdateCnt;
			if (this.mDownButtons != 0)
			{
				return this.MouseDrag(x, y);
			}
			this.mMouseIn = true;
			this.MousePosition(x, y);
			return true;
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00056634 File Offset: 0x00054834
		public bool MouseDrag(int x, int y)
		{
			this.mLastInputUpdateCnt = this.mUpdateCnt;
			this.mMouseIn = true;
			this.mLastMouseX = x;
			this.mLastMouseY = y;
			if (this.mOverWidget != null && this.mOverWidget != this.mLastDownWidget)
			{
				Widget theWidget = this.mOverWidget;
				this.mOverWidget = null;
				this.MouseLeave(theWidget);
			}
			if (this.mLastDownWidget != null)
			{
				SexyPoint absPos = this.mLastDownWidget.GetAbsPos();
				int x2 = x - absPos.mX;
				int y2 = y - absPos.mY;
				this.mLastDownWidget.MouseDrag(x2, y2);
				int num = 0;
				int num2 = 0;
				Widget widgetAt = this.GetWidgetAt(x, y, ref num, ref num2);
				if (widgetAt == this.mLastDownWidget && widgetAt != null)
				{
					if (this.mOverWidget == null)
					{
						this.mOverWidget = this.mLastDownWidget;
						this.MouseEnter(this.mOverWidget);
					}
				}
				else if (this.mOverWidget != null)
				{
					Widget theWidget2 = this.mOverWidget;
					this.mOverWidget = null;
					this.MouseLeave(theWidget2);
				}
			}
			return true;
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00056727 File Offset: 0x00054927
		public bool MouseExit(int x, int y)
		{
			this.mLastInputUpdateCnt = this.mUpdateCnt;
			this.mMouseIn = false;
			if (this.mOverWidget != null)
			{
				this.MouseLeave(this.mOverWidget);
				this.mOverWidget = null;
			}
			return true;
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00056758 File Offset: 0x00054958
		public void MouseWheel(int theDelta)
		{
			this.mLastInputUpdateCnt = this.mUpdateCnt;
			if (this.mFocusWidget != null)
			{
				this.mFocusWidget.MouseWheel(theDelta);
			}
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x0005677A File Offset: 0x0005497A
		public int KeyChar(sbyte theChar)
		{
			return 0;
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x0005677D File Offset: 0x0005497D
		public bool KeyDown(KeyCode key)
		{
			this.mLastInputUpdateCnt = this.mUpdateCnt;
			if (key >= KeyCode.KEYCODE_UNKNOWN && key < (KeyCode)255)
			{
				this.mKeyDown[(int)key] = true;
			}
			if (this.mFocusWidget != null)
			{
				this.mFocusWidget.KeyDown(key);
			}
			return true;
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x000567B8 File Offset: 0x000549B8
		public bool KeyUp(KeyCode key)
		{
			this.mLastInputUpdateCnt = this.mUpdateCnt;
			if (key >= KeyCode.KEYCODE_UNKNOWN && key < (KeyCode)255)
			{
				this.mKeyDown[(int)key] = false;
			}
			if (key == KeyCode.KEYCODE_TAB && this.mKeyDown[17])
			{
				return true;
			}
			if (this.mFocusWidget != null)
			{
				this.mFocusWidget.KeyUp(key);
			}
			return true;
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x0005680D File Offset: 0x00054A0D
		public bool IsLeftButtonDown()
		{
			return false;
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x00056810 File Offset: 0x00054A10
		public bool IsMiddleButtonDown()
		{
			return false;
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00056813 File Offset: 0x00054A13
		public bool IsRightButtonDown()
		{
			return false;
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00056818 File Offset: 0x00054A18
		public void TouchBegan(SexyAppBase.Touch touch)
		{
			this.mLastInputUpdateCnt = this.mUpdateCnt;
			this.mActualDownButtons |= 1;
			this.MousePosition(touch.location.mX, touch.location.mY);
			int num = 0;
			int num2 = 0;
			Widget widgetAt = this.GetWidgetAt(touch.location.mX, touch.location.mY, ref num, ref num2);
			if (this.mLastDownWidget != null)
			{
				widgetAt = this.mLastDownWidget;
			}
			if (widgetAt != null)
			{
				SexyPoint absPos = widgetAt.GetAbsPos();
				touch.location.mX -= absPos.mX;
				touch.location.mY -= absPos.mY;
				touch.previousLocation.mX -= absPos.mX;
				touch.previousLocation.mY -= absPos.mY;
			}
			this.mLastDownButtonId = 1;
			this.mDownButtons |= 1;
			this.mLastDownWidget = widgetAt;
			if (widgetAt != null)
			{
				if (widgetAt.WantsFocus())
				{
					this.SetFocus(widgetAt);
				}
				widgetAt.mIsDown = true;
				widgetAt.TouchBegan(touch);
			}
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00056938 File Offset: 0x00054B38
		public void TouchMoved(SexyAppBase.Touch touch)
		{
			this.mLastInputUpdateCnt = this.mUpdateCnt;
			this.mMouseIn = true;
			this.mLastMouseX = touch.location.mX;
			this.mLastMouseY = touch.location.mY;
			if (this.mLastDownWidget != null)
			{
				int num = 0;
				int num2 = 0;
				Widget widgetAt = this.GetWidgetAt(touch.location.mX, touch.location.mY, ref num, ref num2);
				SexyPoint absPos = this.mLastDownWidget.GetAbsPos();
				touch.location.mX -= absPos.mX;
				touch.location.mY -= absPos.mY;
				touch.previousLocation.mX -= absPos.mX;
				touch.previousLocation.mY -= absPos.mY;
				this.mLastDownWidget.TouchMoved(touch);
				if (widgetAt == this.mLastDownWidget && widgetAt != null)
				{
					if (this.mOverWidget == null)
					{
						this.mOverWidget = this.mLastDownWidget;
						this.MouseEnter(this.mOverWidget);
						return;
					}
				}
				else if (this.mOverWidget != null)
				{
					Widget theWidget = this.mOverWidget;
					this.mOverWidget = null;
					this.MouseLeave(theWidget);
				}
			}
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00056A70 File Offset: 0x00054C70
		public void TouchEnded(SexyAppBase.Touch touch)
		{
			this.mLastInputUpdateCnt = this.mUpdateCnt;
			int num = 1;
			this.mActualDownButtons &= ~num;
			if (this.mLastDownWidget != null && (this.mDownButtons & num) != 0)
			{
				Widget widget = this.mLastDownWidget;
				this.mDownButtons &= ~num;
				if (this.mDownButtons == 0)
				{
					this.mLastDownWidget = null;
				}
				SexyPoint absPos = widget.GetAbsPos();
				touch.location.mX -= absPos.mX;
				touch.location.mY -= absPos.mY;
				touch.previousLocation.mX -= absPos.mX;
				touch.previousLocation.mY -= absPos.mY;
				widget.mIsDown = false;
				widget.TouchEnded(touch);
			}
			else
			{
				this.mDownButtons &= ~num;
			}
			this.MousePosition((int)GlobalMembers.NO_TOUCH_MOUSE_POS.X, (int)GlobalMembers.NO_TOUCH_MOUSE_POS.Y);
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00056B79 File Offset: 0x00054D79
		public void TouchesCanceled()
		{
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x00056B7B File Offset: 0x00054D7B
		public Widget GetGamepadSelection()
		{
			return this.mGamepadSelectionWidget;
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00056B83 File Offset: 0x00054D83
		public void SetGamepadSelection(Widget theSelectedWidget, WidgetLinkDir theDirection)
		{
			Widget widget = this.mGamepadSelectionWidget;
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x00056B8E File Offset: 0x00054D8E
		public void GamepadButtonDown(GamepadButton theButton, int thePlayer, uint theFlags)
		{
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x00056B90 File Offset: 0x00054D90
		public void GamepadButtonUp(GamepadButton theButton, int thePlayer, uint theFlags)
		{
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x00056B92 File Offset: 0x00054D92
		public void GamepadAxisMove(GamepadAxis theAxis, int thePlayer, float theAxisValue)
		{
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x00056B94 File Offset: 0x00054D94
		public IGamepad GetGamepadForPlayer(int thePlayer)
		{
			return null;
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00056B97 File Offset: 0x00054D97
		public int GetWidgetFlags()
		{
			if (!this.mHasFocus)
			{
				return FlagsMod.GetModFlags(this.mWidgetFlags, this.mLostFocusFlagsMod);
			}
			return this.mWidgetFlags;
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x00056BB9 File Offset: 0x00054DB9
		protected void MouseEnter(Widget theWidget)
		{
			theWidget.mIsOver = true;
			theWidget.MouseEnter();
			if (theWidget.mDoFinger)
			{
				theWidget.ShowFinger(true);
			}
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x00056BD7 File Offset: 0x00054DD7
		protected void MouseLeave(Widget theWidget)
		{
			theWidget.mIsOver = false;
			theWidget.MouseLeave();
			if (theWidget.mDoFinger)
			{
				theWidget.ShowFinger(false);
			}
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x00056BF8 File Offset: 0x00054DF8
		protected void SetBaseModal(Widget theWidget, FlagsMod theBelowFlagsMod)
		{
			this.mBaseModalWidget = theWidget;
			this.mBelowModalFlagsMod = theBelowFlagsMod;
			if (this.mOverWidget != null && (this.mBelowModalFlagsMod.mRemoveFlags & 16) != 0 && this.IsBelow(this.mOverWidget, this.mBaseModalWidget))
			{
				Widget theWidget2 = this.mOverWidget;
				this.mOverWidget = null;
				this.MouseLeave(theWidget2);
			}
			if (this.mLastDownWidget != null && (this.mBelowModalFlagsMod.mRemoveFlags & 16) != 0 && this.IsBelow(this.mLastDownWidget, this.mBaseModalWidget))
			{
				Widget theWidget3 = this.mLastDownWidget;
				int theDownCode = this.mDownButtons;
				this.mDownButtons = 0;
				this.mLastDownWidget = null;
				this.DoMouseUps(theWidget3, theDownCode);
			}
			if (this.mFocusWidget != null && (this.mBelowModalFlagsMod.mRemoveFlags & 32) != 0 && this.IsBelow(this.mFocusWidget, this.mBaseModalWidget))
			{
				Widget widget = this.mFocusWidget;
				this.mFocusWidget = null;
				widget.LostFocus();
			}
		}

		// Token: 0x04000DF3 RID: 3571
		public Widget mDefaultTab;

		// Token: 0x04000DF4 RID: 3572
		public Graphics mCurG;

		// Token: 0x04000DF5 RID: 3573
		public SexyAppBase mApp;

		// Token: 0x04000DF6 RID: 3574
		public MemoryImage mImage;

		// Token: 0x04000DF7 RID: 3575
		public MemoryImage mTransientImage;

		// Token: 0x04000DF8 RID: 3576
		public bool mLastHadTransients;

		// Token: 0x04000DF9 RID: 3577
		public Widget mPopupCommandWidget;

		// Token: 0x04000DFA RID: 3578
		public List<KeyValuePair<Widget, int>> mDeferredOverlayWidgets = new List<KeyValuePair<Widget, int>>();

		// Token: 0x04000DFB RID: 3579
		public int mMinDeferredOverlayPriority;

		// Token: 0x04000DFC RID: 3580
		public bool mHasFocus;

		// Token: 0x04000DFD RID: 3581
		public Widget mFocusWidget;

		// Token: 0x04000DFE RID: 3582
		public Widget mLastDownWidget;

		// Token: 0x04000DFF RID: 3583
		public Widget mOverWidget;

		// Token: 0x04000E00 RID: 3584
		public Widget mBaseModalWidget;

		// Token: 0x04000E01 RID: 3585
		public Widget mGamepadSelectionWidget;

		// Token: 0x04000E02 RID: 3586
		public FlagsMod mLostFocusFlagsMod = new FlagsMod();

		// Token: 0x04000E03 RID: 3587
		public FlagsMod mBelowModalFlagsMod = new FlagsMod();

		// Token: 0x04000E04 RID: 3588
		public FlagsMod mDefaultBelowModalFlagsMod = new FlagsMod();

		// Token: 0x04000E05 RID: 3589
		public LinkedList<PreModalInfo> mPreModalInfoList = new LinkedList<PreModalInfo>();

		// Token: 0x04000E06 RID: 3590
		public Rect mMouseDestRect = default(Rect);

		// Token: 0x04000E07 RID: 3591
		public Rect mMouseSourceRect = default(Rect);

		// Token: 0x04000E08 RID: 3592
		public bool mMouseIn;

		// Token: 0x04000E09 RID: 3593
		public int mLastMouseX;

		// Token: 0x04000E0A RID: 3594
		public int mLastMouseY;

		// Token: 0x04000E0B RID: 3595
		public int mDownButtons;

		// Token: 0x04000E0C RID: 3596
		public int mActualDownButtons;

		// Token: 0x04000E0D RID: 3597
		public int mLastInputUpdateCnt;

		// Token: 0x04000E0E RID: 3598
		public bool[] mKeyDown = new bool[255];

		// Token: 0x04000E0F RID: 3599
		public int mLastDownButtonId;

		// Token: 0x04000E10 RID: 3600
		public int mWidgetFlags;
	}
}
