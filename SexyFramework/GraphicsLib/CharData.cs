using System;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000C6 RID: 198
	public class CharData
	{
		// Token: 0x06000619 RID: 1561 RVA: 0x00015B86 File Offset: 0x00013D86
		public CharData()
		{
			this.mKerningFirst = 0;
			this.mKerningCount = 0;
			this.mWidth = 0;
			this.mOrder = 0;
		}

		// Token: 0x040004F6 RID: 1270
		public ushort mChar;

		// Token: 0x040004F7 RID: 1271
		public Rect mImageRect = default(Rect);

		// Token: 0x040004F8 RID: 1272
		public SexyPoint mOffset = new SexyPoint();

		// Token: 0x040004F9 RID: 1273
		public ushort mKerningFirst;

		// Token: 0x040004FA RID: 1274
		public ushort mKerningCount;

		// Token: 0x040004FB RID: 1275
		public int mWidth;

		// Token: 0x040004FC RID: 1276
		public int mOrder;

		// Token: 0x040004FD RID: 1277
		public int mHashEntryIndex;
	}
}
