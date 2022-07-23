using System;
using Microsoft.Xna.Framework;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000ED RID: 237
	public class PIForce : IDisposable
	{
		// Token: 0x060006C1 RID: 1729 RVA: 0x0001CDAC File Offset: 0x0001AFAC
		public virtual void Dispose()
		{
			this.mPos.Dispose();
			this.mActive.Dispose();
			this.mAngle.Dispose();
			this.mStrength.Dispose();
			this.mWidth.Dispose();
			this.mHeight.Dispose();
			this.mDirection.Dispose();
		}

		// Token: 0x0400065E RID: 1630
		public string mName;

		// Token: 0x0400065F RID: 1631
		public bool mVisible;

		// Token: 0x04000660 RID: 1632
		public PIValue2D mPos = new PIValue2D();

		// Token: 0x04000661 RID: 1633
		public PIValue mStrength = new PIValue();

		// Token: 0x04000662 RID: 1634
		public PIValue mDirection = new PIValue();

		// Token: 0x04000663 RID: 1635
		public PIValue mActive = new PIValue();

		// Token: 0x04000664 RID: 1636
		public PIValue mAngle = new PIValue();

		// Token: 0x04000665 RID: 1637
		public PIValue mWidth = new PIValue();

		// Token: 0x04000666 RID: 1638
		public PIValue mHeight = new PIValue();

		// Token: 0x04000667 RID: 1639
		public Vector2[] mCurPoints = new Vector2[5];
	}
}
