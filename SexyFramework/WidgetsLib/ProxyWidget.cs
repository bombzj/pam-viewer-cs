using System;
using SexyFramework.GraphicsLib;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001CB RID: 459
	public class ProxyWidget : Widget
	{
		// Token: 0x060010A4 RID: 4260 RVA: 0x0005306A File Offset: 0x0005126A
		public ProxyWidget(ProxyWidgetListener listener)
		{
			this.mListener = listener;
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x00053079 File Offset: 0x00051279
		public override void Draw(Graphics g)
		{
			if (this.mListener != null)
			{
				this.mListener.DrawProxyWidget(g, this);
			}
		}

		// Token: 0x04000D79 RID: 3449
		public ProxyWidgetListener mListener;
	}
}
