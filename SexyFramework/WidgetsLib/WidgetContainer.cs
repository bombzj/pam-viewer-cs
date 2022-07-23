using System;
using System.Collections.Generic;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x02000114 RID: 276
	public class WidgetContainer
	{
		// Token: 0x06000858 RID: 2136 RVA: 0x0002AB20 File Offset: 0x00028D20
		public WidgetContainer()
		{
			this.mX = 0;
			this.mY = 0;
			this.mWidth = 0;
			this.mHeight = 0;
			this.mParent = null;
			this.mWidgetManager = null;
			this.mUpdateIteratorModified = false;
			this.mLastWMUpdateCount = 0;
			this.mUpdateCnt = 0;
			this.mDirty = false;
			this.mHasAlpha = false;
			this.mClip = true;
			this.mPriority = 0;
			this.mZOrder = 0;
			this.mUpdateIterator = null;
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0002ABCA File Offset: 0x00028DCA
		public virtual void Dispose()
		{
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0002ABCC File Offset: 0x00028DCC
		public void CopyFrom(WidgetContainer rhs)
		{
			if (rhs == null)
			{
				return;
			}
			this.mWidgetManager = rhs.mWidgetManager;
			this.mParent = rhs.mParent;
			this.mUpdateIteratorModified = rhs.mUpdateIteratorModified;
			this.mLastWMUpdateCount = rhs.mLastWMUpdateCount;
			this.mUpdateCnt = rhs.mUpdateCnt;
			this.mX = rhs.mX;
			this.mY = rhs.mY;
			this.mWidth = rhs.mWidth;
			this.mHeight = rhs.mHeight;
			this.mPriority = rhs.mPriority;
			this.mZOrder = rhs.mZOrder;
			this.mDirty = rhs.mDirty;
			this.mHasAlpha = rhs.mHasAlpha;
			this.mClip = rhs.mClip;
			this.mUpdateIterator = rhs.mUpdateIterator;
			this.mWidgetFlagsMod.mAddFlags = rhs.mWidgetFlagsMod.mAddFlags;
			this.mWidgetFlagsMod.mRemoveFlags = rhs.mWidgetFlagsMod.mRemoveFlags;
			this.mRect.SetValue(rhs.mRect.mX, rhs.mRect.mY, rhs.mRect.mWidth, rhs.mRect.mWidth);
			this.mHelperRect.SetValue(rhs.mHelperRect.mX, rhs.mHelperRect.mY, rhs.mHelperRect.mWidth, rhs.mHelperRect.mWidth);
			this.mWidgets.Clear();
			foreach (Widget widget in rhs.mWidgets)
			{
				this.mWidgets.AddLast(widget);
			}
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0002AD60 File Offset: 0x00028F60
		public virtual Rect GetRect()
		{
			this.mRect.mX = this.mX;
			this.mRect.mY = this.mY;
			this.mRect.mWidth = this.mWidth;
			this.mRect.mHeight = this.mHeight;
			return this.mRect;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0002ADB8 File Offset: 0x00028FB8
		public virtual bool Intersects(WidgetContainer theWidget)
		{
			return this.GetRect().Intersects(theWidget.GetRect());
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0002ADDC File Offset: 0x00028FDC
		public virtual void AddWidget(Widget theWidget)
		{
			if (!this.mWidgets.Contains(theWidget))
			{
				this.InsertWidgetHelper(this.mWidgets.Last, theWidget);
				theWidget.mWidgetManager = this.mWidgetManager;
				theWidget.mParent = this;
				if (this.mWidgetManager != null)
				{
					theWidget.AddedToManager(this.mWidgetManager);
					theWidget.MarkDirtyFull();
					this.mWidgetManager.RehupMouse();
				}
				this.MarkDirty();
			}
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0002AE48 File Offset: 0x00029048
		public virtual void RemoveWidget(Widget theWidget)
		{
			if (this.mWidgets.Contains(theWidget))
			{
				LinkedListNode<Widget> linkedListNode = this.mWidgets.Find(theWidget);
				theWidget.WidgetRemovedHelper();
				theWidget.mParent = null;
				if (linkedListNode == this.mUpdateIterator)
				{
					this.mUpdateIterator = linkedListNode.Next;
					this.mUpdateIteratorModified = true;
				}
				this.mWidgets.Remove(linkedListNode);
			}
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0002AEA5 File Offset: 0x000290A5
		public virtual bool HasWidget(Widget theWidget)
		{
			return this.mWidgets.Contains(theWidget);
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0002AEB3 File Offset: 0x000290B3
		public virtual void DisableWidget(Widget theWidget)
		{
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0002AEB8 File Offset: 0x000290B8
		public virtual void RemoveAllWidgets(bool doDelete, bool recursive)
		{
			while (this.mWidgets.Count > 0)
			{
				Widget value = this.mWidgets.First.Value;
				this.RemoveWidget(value);
				if (recursive)
				{
					value.RemoveAllWidgets(doDelete, recursive);
				}
				if (doDelete && value != null)
				{
					value.Dispose();
				}
			}
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0002AF04 File Offset: 0x00029104
		public virtual void SetFocus(Widget theWidget)
		{
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0002AF08 File Offset: 0x00029108
		public virtual bool IsBelow(Widget theWidget1, Widget theWidget2)
		{
			bool flag = false;
			return this.IsBelowHelper(theWidget1, theWidget2, ref flag);
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0002AF24 File Offset: 0x00029124
		public virtual void MarkAllDirty()
		{
			this.MarkDirty();
			foreach (Widget widget in this.mWidgets)
			{
				widget.mDirty = true;
				widget.MarkAllDirty();
			}
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0002AF68 File Offset: 0x00029168
		public virtual void BringToFront(Widget theWidget)
		{
			if (this.mWidgets.Contains(theWidget))
			{
				LinkedListNode<Widget> linkedListNode = this.mWidgets.Find(theWidget);
				if (linkedListNode == this.mUpdateIterator)
				{
					this.mUpdateIterator = this.mUpdateIterator.Next;
					this.mUpdateIteratorModified = true;
				}
				this.mWidgets.Remove(linkedListNode);
				this.InsertWidgetHelper(null, theWidget);
				theWidget.OrderInManagerChanged();
			}
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0002AFCC File Offset: 0x000291CC
		public virtual void BringToBack(Widget theWidget)
		{
			if (this.mWidgets.Contains(theWidget))
			{
				LinkedListNode<Widget> linkedListNode = this.mWidgets.Find(theWidget);
				if (linkedListNode == this.mUpdateIterator)
				{
					this.mUpdateIterator = this.mUpdateIterator.Next;
					this.mUpdateIteratorModified = true;
				}
				this.mWidgets.Remove(linkedListNode);
				this.InsertWidgetHelper(this.mWidgets.First, theWidget);
				theWidget.OrderInManagerChanged();
			}
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0002B03C File Offset: 0x0002923C
		public virtual void PutBehind(Widget theWidget, Widget theRefWidget)
		{
			if (theRefWidget != null)
			{
				theWidget.mZOrder = theRefWidget.mZOrder;
			}
			if (this.mWidgets.Contains(theWidget))
			{
				LinkedListNode<Widget> linkedListNode = this.mWidgets.Find(theWidget);
				if (linkedListNode == this.mUpdateIterator)
				{
					this.mUpdateIterator = this.mUpdateIterator.Next;
					this.mUpdateIteratorModified = true;
				}
				this.mWidgets.Remove(linkedListNode);
				LinkedListNode<Widget> where = this.mWidgets.Find(theRefWidget);
				this.InsertWidgetHelper(where, theWidget);
				theWidget.OrderInManagerChanged();
			}
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0002B0BC File Offset: 0x000292BC
		public virtual void PutInfront(Widget theWidget, Widget theRefWidget)
		{
			if (theRefWidget != null)
			{
				theWidget.mZOrder = theRefWidget.mZOrder;
			}
			if (this.mWidgets.Contains(theWidget))
			{
				LinkedListNode<Widget> linkedListNode = this.mWidgets.Find(theWidget);
				if (linkedListNode == this.mUpdateIterator)
				{
					this.mUpdateIterator = this.mUpdateIterator.Next;
					this.mUpdateIteratorModified = true;
				}
				this.mWidgets.Remove(linkedListNode);
				LinkedListNode<Widget> linkedListNode2 = this.mWidgets.Find(theRefWidget);
				linkedListNode2 = linkedListNode2.Next;
				this.InsertWidgetHelper(linkedListNode2, theWidget);
				theWidget.OrderInManagerChanged();
			}
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0002B144 File Offset: 0x00029344
		public virtual SexyPoint GetAbsPos()
		{
			if (this.mParent == null)
			{
				return new SexyPoint(this.mX, this.mY);
			}
			return new SexyPoint(this.mX + this.mParent.GetAbsPos().mX, this.mY + this.mParent.GetAbsPos().mY);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0002B19E File Offset: 0x0002939E
		public virtual void MarkDirty()
		{
			if (this.mParent != null)
			{
				this.mParent.MarkDirty(this);
				return;
			}
			this.mDirty = true;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0002B1BC File Offset: 0x000293BC
		public virtual void MarkDirty(WidgetContainer theWidget)
		{
			if (theWidget.mDirty)
			{
				return;
			}
			this.MarkDirty();
			theWidget.mDirty = true;
			if (this.mParent != null)
			{
				return;
			}
			if (theWidget.mHasAlpha)
			{
				this.MarkDirtyFull(theWidget);
				return;
			}
			bool flag = false;
			foreach (Widget widget in this.mWidgets)
			{
				if (widget == theWidget)
				{
					flag = true;
				}
				else if (flag && widget.mVisible && widget.Intersects(theWidget))
				{
					this.MarkDirty(widget);
				}
			}
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0002B23C File Offset: 0x0002943C
		public virtual void MarkDirtyFull()
		{
			if (this.mParent != null)
			{
				this.mParent.MarkDirtyFull(this);
				return;
			}
			this.mDirty = true;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0002B25C File Offset: 0x0002945C
		public virtual void MarkDirtyFull(WidgetContainer theWidget)
		{
			this.MarkDirtyFull();
			theWidget.mDirty = true;
			if (this.mParent != null)
			{
				return;
			}
			LinkedList<Widget>.Enumerator enumerator = this.mWidgets.GetEnumerator();
			LinkedListNode<Widget> linkedListNode = null;
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == theWidget)
				{
					linkedListNode = this.mWidgets.Find(enumerator.Current);
					break;
				}
			}
			if (linkedListNode == null)
			{
				return;
			}
			LinkedListNode<Widget> linkedListNode2 = linkedListNode;
			for (linkedListNode2 = linkedListNode2.Previous; linkedListNode2 != null; linkedListNode2 = linkedListNode2.Previous)
			{
				Widget value = linkedListNode2.Value;
				if (value.mVisible)
				{
					if (value.mHasTransparencies && value.mHasAlpha)
					{
						this.mHelperRect.setValue(0, 0, this.mWidth, this.mHeight);
						Rect rect = theWidget.GetRect().Intersection(this.mHelperRect);
						if (value.Contains(rect.mX, rect.mY) && value.Contains(rect.mX + rect.mWidth - 1, rect.mY + rect.mHeight - 1))
						{
							value.MarkDirty();
							break;
						}
					}
					if (value.Intersects(theWidget))
					{
						this.MarkDirty(value);
					}
				}
			}
			linkedListNode2 = linkedListNode;
			while (linkedListNode2.Next != null)
			{
				Widget value2 = linkedListNode2.Value;
				if (value2.mVisible && value2.Intersects(theWidget))
				{
					this.MarkDirty(value2);
				}
				linkedListNode2 = linkedListNode2.Next;
			}
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0002B3BC File Offset: 0x000295BC
		public virtual void AddedToManager(WidgetManager theWidgetManager)
		{
			foreach (Widget widget in this.mWidgets)
			{
				widget.mWidgetManager = theWidgetManager;
				widget.AddedToManager(theWidgetManager);
				this.MarkDirty();
			}
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x0002B3FC File Offset: 0x000295FC
		public virtual void RemovedFromManager(WidgetManager theWidgetManager)
		{
			foreach (Widget widget in this.mWidgets)
			{
				theWidgetManager.DisableWidget(widget);
				widget.RemovedFromManager(theWidgetManager);
				widget.mWidgetManager = null;
			}
			if (theWidgetManager.mPopupCommandWidget == this)
			{
				theWidgetManager.mPopupCommandWidget = null;
			}
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x0002B44D File Offset: 0x0002964D
		public virtual void Update()
		{
			this.mUpdateCnt++;
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0002B460 File Offset: 0x00029660
		public virtual void UpdateAll(ModalFlags theFlags)
		{
			new AutoModalFlags(theFlags, this.mWidgetFlagsMod);
			if ((theFlags.GetFlags() & 2) != 0)
			{
				this.MarkDirty();
			}
			WidgetManager widgetManager = this.mWidgetManager;
			if (widgetManager == null)
			{
				return;
			}
			if ((theFlags.GetFlags() & 1) != 0 && this.mLastWMUpdateCount != this.mWidgetManager.mUpdateCnt)
			{
				this.mLastWMUpdateCount = this.mWidgetManager.mUpdateCnt;
				this.Update();
			}
			this.mUpdateIterator = this.mWidgets.First;
			while (this.mUpdateIterator != null)
			{
				this.mUpdateIteratorModified = false;
				Widget value = this.mUpdateIterator.Value;
				if (value == widgetManager.mBaseModalWidget)
				{
					theFlags.mIsOver = true;
				}
				value.UpdateAll(theFlags);
				if (!this.mUpdateIteratorModified)
				{
					this.mUpdateIterator = this.mUpdateIterator.Next;
				}
			}
			this.mUpdateIteratorModified = true;
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0002B52E File Offset: 0x0002972E
		public virtual void UpdateF(float theFrac)
		{
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0002B530 File Offset: 0x00029730
		public virtual void UpdateFAll(ModalFlags theFlags, float theFrac)
		{
			new AutoModalFlags(theFlags, this.mWidgetFlagsMod);
			WidgetManager widgetManager = this.mWidgetManager;
			if (widgetManager == null)
			{
				return;
			}
			if ((theFlags.GetFlags() & 1) != 0)
			{
				this.UpdateF(theFrac);
			}
			this.mUpdateIterator = this.mWidgets.First;
			while (this.mUpdateIterator != null)
			{
				this.mUpdateIteratorModified = false;
				Widget value = this.mUpdateIterator.Value;
				if (value == widgetManager.mBaseModalWidget)
				{
					theFlags.mIsOver = true;
				}
				value.UpdateFAll(theFlags, theFrac);
				if (!this.mUpdateIteratorModified)
				{
					this.mUpdateIterator = this.mUpdateIterator.Next;
				}
			}
			this.mUpdateIteratorModified = true;
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0002B5CC File Offset: 0x000297CC
		public virtual void Draw(Graphics g)
		{
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0002B5D0 File Offset: 0x000297D0
		public virtual void DrawAll(ModalFlags theFlags, Graphics g)
		{
			if (this.mWidgetManager != null && this.mPriority > this.mWidgetManager.mMinDeferredOverlayPriority)
			{
				this.mWidgetManager.FlushDeferredOverlayWidgets(this.mPriority);
			}
			new AutoModalFlags(theFlags, this.mWidgetFlagsMod);
			if (this.mClip && (theFlags.GetFlags() & 8) != 0)
			{
				g.ClipRect(0, 0, this.mWidth, this.mHeight);
			}
			if (this.mWidgets.Count == 0)
			{
				if ((theFlags.GetFlags() & 4) != 0)
				{
					this.Draw(g);
				}
				return;
			}
			if ((theFlags.GetFlags() & 4) != 0)
			{
				g.PushState();
				this.Draw(g);
				g.PopState();
			}
			foreach (Widget widget in this.mWidgets)
			{
				if (widget.mVisible)
				{
					if (this.mWidgetManager != null && widget == this.mWidgetManager.mBaseModalWidget)
					{
						theFlags.mIsOver = true;
					}
					g.PushState();
					g.Translate(widget.mX, widget.mY);
					widget.DrawAll(theFlags, g);
					widget.mDirty = false;
					g.PopState();
				}
			}
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0002B6EC File Offset: 0x000298EC
		public virtual void SysColorChangedAll()
		{
			this.SysColorChanged();
			if (this.mWidgets.Count > 0)
			{
				WidgetContainer.aDepthCount++;
			}
			foreach (Widget widget in this.mWidgets)
			{
				widget.SysColorChangedAll();
			}
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0002B73E File Offset: 0x0002993E
		public virtual void SysColorChanged()
		{
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0002B740 File Offset: 0x00029940
		public Widget GetWidgetAtHelper(int x, int y, int theFlags, ref bool found, ref int theWidgetX, ref int theWidgetY)
		{
			bool flag = false;
			FlagsMod.ModFlags(ref theFlags, this.mWidgetFlagsMod);
			for (LinkedListNode<Widget> linkedListNode = this.mWidgets.Last; linkedListNode != null; linkedListNode = linkedListNode.Previous)
			{
				Widget value = linkedListNode.Value;
				int num = theFlags;
				FlagsMod.ModFlags(ref num, value.mWidgetFlagsMod);
				if (flag)
				{
					FlagsMod.ModFlags(ref num, this.mWidgetManager.mBelowModalFlagsMod);
				}
				if ((num & 16) != 0 && value.mVisible)
				{
					bool flag2 = false;
					Widget widgetAtHelper = value.GetWidgetAtHelper(x - value.mX, y - value.mY, num, ref flag2, ref theWidgetX, ref theWidgetY);
					if (widgetAtHelper != null || flag2)
					{
						found = true;
						return widgetAtHelper;
					}
					if (value.mMouseVisible && value.GetInsetRect().Contains(x, y))
					{
						found = true;
						if (value.IsPointVisible(x - value.mX, y - value.mY))
						{
							if (theWidgetX != 0)
							{
								theWidgetX = x - value.mX;
							}
							if (theWidgetY != 0)
							{
								theWidgetY = y - value.mY;
							}
							return value;
						}
					}
				}
				flag |= value == this.mWidgetManager.mBaseModalWidget;
			}
			found = false;
			return null;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0002B85C File Offset: 0x00029A5C
		public bool IsBelowHelper(Widget theWidget1, Widget theWidget2, ref bool found)
		{
			foreach (Widget widget in this.mWidgets)
			{
				if (widget == theWidget1)
				{
					found = true;
					return true;
				}
				if (widget == theWidget2)
				{
					found = true;
					return false;
				}
				bool result = widget.IsBelowHelper(theWidget1, theWidget2, ref found);
				if (found)
				{
					return result;
				}
			}
			return false;
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0002B8AC File Offset: 0x00029AAC
		public void InsertWidgetHelper(LinkedListNode<Widget> where, Widget theWidget)
		{
			LinkedListNode<Widget> linkedListNode;
			for (linkedListNode = where; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				Widget value = linkedListNode.Value;
				if (value.mZOrder >= theWidget.mZOrder)
				{
					if (linkedListNode != this.mWidgets.First)
					{
						value = linkedListNode.Value;
						if (value.mZOrder > theWidget.mZOrder)
						{
							break;
						}
					}
					this.mWidgets.AddAfter(linkedListNode, theWidget);
					return;
				}
			}
			if (linkedListNode == null)
			{
				linkedListNode = this.mWidgets.Last;
			}
			while (linkedListNode != null)
			{
				Widget value2 = linkedListNode.Value;
				if (value2.mZOrder <= theWidget.mZOrder)
				{
					this.mWidgets.AddAfter(linkedListNode, theWidget);
					return;
				}
				linkedListNode = linkedListNode.Previous;
			}
			this.mWidgets.AddFirst(theWidget);
		}

		// Token: 0x040007A6 RID: 1958
		public LinkedList<Widget> mWidgets = new LinkedList<Widget>();

		// Token: 0x040007A7 RID: 1959
		public WidgetManager mWidgetManager;

		// Token: 0x040007A8 RID: 1960
		public WidgetContainer mParent;

		// Token: 0x040007A9 RID: 1961
		public bool mUpdateIteratorModified;

		// Token: 0x040007AA RID: 1962
		public LinkedListNode<Widget> mUpdateIterator;

		// Token: 0x040007AB RID: 1963
		public int mLastWMUpdateCount;

		// Token: 0x040007AC RID: 1964
		public int mUpdateCnt;

		// Token: 0x040007AD RID: 1965
		public int mX;

		// Token: 0x040007AE RID: 1966
		public int mY;

		// Token: 0x040007AF RID: 1967
		public int mWidth;

		// Token: 0x040007B0 RID: 1968
		public int mHeight;

		// Token: 0x040007B1 RID: 1969
		public Rect mRect = default(Rect);

		// Token: 0x040007B2 RID: 1970
		public Rect mHelperRect = default(Rect);

		// Token: 0x040007B3 RID: 1971
		public int mPriority;

		// Token: 0x040007B4 RID: 1972
		public int mZOrder;

		// Token: 0x040007B5 RID: 1973
		public bool mDirty;

		// Token: 0x040007B6 RID: 1974
		public bool mHasAlpha;

		// Token: 0x040007B7 RID: 1975
		public bool mClip;

		// Token: 0x040007B8 RID: 1976
		public FlagsMod mWidgetFlagsMod = new FlagsMod();

		// Token: 0x040007B9 RID: 1977
		private static int aDepthCount;
	}
}
