using System;
using System.Collections.Generic;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000F0 RID: 240
	public class PIEffectDef
	{
		// Token: 0x060006CA RID: 1738 RVA: 0x0001D04B File Offset: 0x0001B24B
		public PIEffectDef()
		{
			this.mRefCount = 1;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001D088 File Offset: 0x0001B288
		public virtual void Dispose()
		{
			for (int i = 0; i < this.mEmitterVector.Count; i++)
			{
				this.mEmitterVector[i] = null;
			}
			for (int j = 0; j < this.mTextureVector.Count; j++)
			{
				this.mTextureVector[j] = null;
			}
			this.mEmitterVector.Clear();
			this.mTextureVector.Clear();
		}

		// Token: 0x04000677 RID: 1655
		public int mRefCount;

		// Token: 0x04000678 RID: 1656
		public List<PIEmitter> mEmitterVector = new List<PIEmitter>();

		// Token: 0x04000679 RID: 1657
		public List<PITexture> mTextureVector = new List<PITexture>();

		// Token: 0x0400067A RID: 1658
		public List<PILayerDef> mLayerDefVector = new List<PILayerDef>();

		// Token: 0x0400067B RID: 1659
		public Dictionary<int, int> mEmitterRefMap = new Dictionary<int, int>();
	}
}
