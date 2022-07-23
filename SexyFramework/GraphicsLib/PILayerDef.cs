using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000EE RID: 238
	public class PILayerDef : IDisposable
	{
		// Token: 0x060006C3 RID: 1731 RVA: 0x0001CE74 File Offset: 0x0001B074
		public virtual void Dispose()
		{
			this.mOffset.Dispose();
			this.mAngle.Dispose();
			foreach (PIEmitterInstanceDef piemitterInstanceDef in this.mEmitterInstanceDefVector)
			{
				piemitterInstanceDef.Dispose();
			}
			this.mEmitterInstanceDefVector.Clear();
		}

		// Token: 0x04000668 RID: 1640
		public string mName;

		// Token: 0x04000669 RID: 1641
		public List<PIEmitterInstanceDef> mEmitterInstanceDefVector = new List<PIEmitterInstanceDef>();

		// Token: 0x0400066A RID: 1642
		public List<PIDeflector> mDeflectorVector = new List<PIDeflector>();

		// Token: 0x0400066B RID: 1643
		public List<PIBlocker> mBlockerVector = new List<PIBlocker>();

		// Token: 0x0400066C RID: 1644
		public List<PIForce> mForceVector = new List<PIForce>();

		// Token: 0x0400066D RID: 1645
		public PIValue2D mOffset = new PIValue2D();

		// Token: 0x0400066E RID: 1646
		public Vector2 mOrigOffset = default(Vector2);

		// Token: 0x0400066F RID: 1647
		public PIValue mAngle = new PIValue();
	}
}
