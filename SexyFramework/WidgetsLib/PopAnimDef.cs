using System;
using System.Collections.Generic;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001C4 RID: 452
	public class PopAnimDef
	{
		// Token: 0x06001055 RID: 4181 RVA: 0x0004E000 File Offset: 0x0004C200
		public PopAnimDef()
		{
			this.mRefCount = 0;
			//this.mMainSpriteDef = null;
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0004E02C File Offset: 0x0004C22C
		public void Dispose()
		{
			if (this.mMainSpriteDef != null)
			{
				this.mMainSpriteDef.Dispose();
			}
		}

		// Token: 0x04000D34 RID: 3380
		public PASpriteDef mMainSpriteDef = new PASpriteDef();

		// Token: 0x04000D35 RID: 3381
		public List<PASpriteDef> mSpriteDefVector = new List<PASpriteDef>();

		// Token: 0x04000D36 RID: 3382
		public LinkedList<string> mObjectNamePool = new LinkedList<string>();

		// Token: 0x04000D37 RID: 3383
		public int mRefCount;
	}
}
