using System;
using System.Collections.Generic;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000EC RID: 236
	public class PIBlocker : IDisposable
	{
		// Token: 0x060006BF RID: 1727 RVA: 0x0001CCF8 File Offset: 0x0001AEF8
		public virtual void Dispose()
		{
			this.mPos.Dispose();
			this.mActive.Dispose();
			this.mAngle.Dispose();
			foreach (PIValue2D pivalue2D in this.mPoints)
			{
				pivalue2D.Dispose();
			}
			this.mPoints.Clear();
		}

		// Token: 0x04000658 RID: 1624
		public string mName;

		// Token: 0x04000659 RID: 1625
		public int mUseLayersBelowForBg;

		// Token: 0x0400065A RID: 1626
		public PIValue2D mPos = new PIValue2D();

		// Token: 0x0400065B RID: 1627
		public PIValue mActive = new PIValue();

		// Token: 0x0400065C RID: 1628
		public PIValue mAngle = new PIValue();

		// Token: 0x0400065D RID: 1629
		public List<PIValue2D> mPoints = new List<PIValue2D>();
	}
}
