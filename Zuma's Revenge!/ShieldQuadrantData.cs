using System;
using SexyFramework.AELib;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x02000031 RID: 49
	public class ShieldQuadrantData
	{
		// Token: 0x060005A8 RID: 1448 RVA: 0x0001D8F2 File Offset: 0x0001BAF2
		public ShieldQuadrantData()
		{
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001D8FA File Offset: 0x0001BAFA
		public ShieldQuadrantData(CompositionMgr cm, PIEffect s)
		{
			this.mCompMgr = cm;
			this.mSparkles = s;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001D910 File Offset: 0x0001BB10
		public virtual void Dispose()
		{
			if (this.mCompMgr != null)
			{
				this.mCompMgr = null;
			}
			if (this.mSparkles != null)
			{
				this.mSparkles.Dispose();
				this.mSparkles = null;
			}
		}

		// Token: 0x0400028A RID: 650
		public CompositionMgr mCompMgr;

		// Token: 0x0400028B RID: 651
		public PIEffect mSparkles;

		// Token: 0x0400028C RID: 652
		public bool mDoHitAnim;

		// Token: 0x0400028D RID: 653
		public bool mDoExplodeAnim;
	}
}
