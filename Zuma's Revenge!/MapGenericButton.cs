using System;
using JeffLib;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x0200011E RID: 286
	public class MapGenericButton : ExtraSexyButton
	{
		// Token: 0x06000EB7 RID: 3767 RVA: 0x00098293 File Offset: 0x00096493
		public MapGenericButton(int theId, MapScreen theListener)
			: base(theId, theListener)
		{
			this.mUsesAnimators = false;
			this.mMapScreen = theListener;
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x000982AB File Offset: 0x000964AB
		public override void Draw(Graphics g)
		{
			g.SetColorizeImages(true);
			g.SetColor(this.mMapScreen.mAlpha);
			base.Draw(g);
		}

		// Token: 0x04000E66 RID: 3686
		public MapScreen mMapScreen;
	}
}
