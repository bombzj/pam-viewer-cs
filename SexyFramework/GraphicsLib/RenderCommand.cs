using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000C5 RID: 197
	public class RenderCommand : IDisposable
	{
		// Token: 0x06000617 RID: 1559 RVA: 0x00015B3C File Offset: 0x00013D3C
		public RenderCommand()
		{
			this.mFontLayer = null;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00015B6F File Offset: 0x00013D6F
		public virtual void Dispose()
		{
			this.mFontLayer = null;
			this.mDest = null;
			this.mSrc = null;
		}

		// Token: 0x040004F1 RID: 1265
		public SexyColor mColor = default(SexyColor);

		// Token: 0x040004F2 RID: 1266
		public ActiveFontLayer mFontLayer;

		// Token: 0x040004F3 RID: 1267
		public int[] mDest = new int[2];

		// Token: 0x040004F4 RID: 1268
		public int[] mSrc = new int[4];

		// Token: 0x040004F5 RID: 1269
		public int mMode;
	}
}
