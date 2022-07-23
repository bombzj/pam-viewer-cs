using System;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace JeffLib
{
	// Token: 0x0200011B RID: 283
	public class DialogEx : Dialog
	{
		// Token: 0x060008E4 RID: 2276 RVA: 0x0002D460 File Offset: 0x0002B660
		public DialogEx(Image theComponentImage, Image theButtonComponentImage, int theId, bool isModal, string theDialogHeader, string theDialogLines, string theDialogFooter, int theButtonMode)
			: base(theComponentImage, theButtonComponentImage, theId, isModal, theDialogHeader, theDialogLines, theDialogFooter, theButtonMode)
		{
			this.mFlushPriority = -1;
			this.mDrawScale.SetConstant(1.0);
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0002D4A8 File Offset: 0x0002B6A8
		public virtual void PreDraw(Graphics g)
		{
			this.mWidgetManager.FlushDeferredOverlayWidgets(this.mFlushPriority);
			Graphics3D graphics3D = ((g != null) ? g.Get3D() : null);
			if (this.mDrawScale != 1.0 && graphics3D != null)
			{
				SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
				sexyTransform2D.Translate(-g.mTransX - (float)(this.mWidth / 2), -g.mTransY - (float)(this.mHeight / 2));
				sexyTransform2D.Scale((float)this.mDrawScale, (float)this.mDrawScale);
				sexyTransform2D.Translate(g.mTransX + (float)(this.mWidth / 2), g.mTransY + (float)(this.mHeight / 2));
				graphics3D.PushTransform(sexyTransform2D);
			}
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0002D57A File Offset: 0x0002B77A
		public override void DrawAll(ModalFlags theFlags, Graphics g)
		{
			this.PreDraw(g);
			base.DrawAll(theFlags, g);
			this.PostDraw(g);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0002D594 File Offset: 0x0002B794
		public virtual void PostDraw(Graphics g)
		{
			Graphics3D graphics3D = ((g != null) ? g.Get3D() : null);
			if (this.mDrawScale != 1.0 && graphics3D != null)
			{
				graphics3D.PopTransform();
			}
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0002D5D0 File Offset: 0x0002B7D0
		public override void Update()
		{
			base.Update();
			if (!this.mDrawScale.HasBeenTriggered())
			{
				this.MarkDirty();
			}
			if (!this.mDrawScale.IncInVal() && this.mDrawScale == 0.0)
			{
				this.CloseDialog();
				GlobalMembers.gSexyAppBase.KillDialog(this);
			}
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0002D62C File Offset: 0x0002B82C
		public virtual void CloseDialog()
		{
		}

		// Token: 0x040007FF RID: 2047
		public int mFlushPriority;

		// Token: 0x04000800 RID: 2048
		public CurvedVal mDrawScale = new CurvedVal();
	}
}
