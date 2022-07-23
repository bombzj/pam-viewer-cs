using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000D3 RID: 211
	public struct CacheableCharWidthPair
	{
		// Token: 0x0600065E RID: 1630 RVA: 0x00019682 File Offset: 0x00017882
		public CacheableCharWidthPair(char theChar, int theWidth)
		{
			this.charData = theChar;
			this.width = theWidth;
		}

		// Token: 0x04000546 RID: 1350
		public char charData;

		// Token: 0x04000547 RID: 1351
		public int width;
	}
}
