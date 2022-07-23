using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000E6 RID: 230
	public class PIFreeEmitterInstance : PIParticleInstance
	{
		// Token: 0x060006B7 RID: 1719 RVA: 0x0001CA7D File Offset: 0x0001AC7D
		public PIFreeEmitterInstance()
		{
			this.mEmitter.mParticleGroup.mWasEmitted = true;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001CAA1 File Offset: 0x0001ACA1
		public override void Dispose()
		{
			base.Dispose();
			this.mEmitter.Dispose();
		}

		// Token: 0x04000619 RID: 1561
		public PIEmitterBase mEmitter = new PIEmitterBase();
	}
}
