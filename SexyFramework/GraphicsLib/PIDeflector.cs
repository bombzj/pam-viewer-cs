using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000EB RID: 235
	public class PIDeflector : IDisposable
	{
		// Token: 0x060006BD RID: 1725 RVA: 0x0001CC2C File Offset: 0x0001AE2C
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
			this.mCurPoints.Clear();
		}

		// Token: 0x0400064E RID: 1614
		public string mName;

		// Token: 0x0400064F RID: 1615
		public float mBounce;

		// Token: 0x04000650 RID: 1616
		public float mHits;

		// Token: 0x04000651 RID: 1617
		public float mThickness;

		// Token: 0x04000652 RID: 1618
		public bool mVisible;

		// Token: 0x04000653 RID: 1619
		public PIValue2D mPos = new PIValue2D();

		// Token: 0x04000654 RID: 1620
		public PIValue mActive = new PIValue();

		// Token: 0x04000655 RID: 1621
		public PIValue mAngle = new PIValue();

		// Token: 0x04000656 RID: 1622
		public List<PIValue2D> mPoints = new List<PIValue2D>();

		// Token: 0x04000657 RID: 1623
		public List<Vector2> mCurPoints = new List<Vector2>();
	}
}
