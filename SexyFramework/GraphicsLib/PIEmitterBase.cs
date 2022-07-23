using System;
using System.Collections.Generic;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000E5 RID: 229
	public class PIEmitterBase : IDisposable
	{
		// Token: 0x060006B5 RID: 1717 RVA: 0x0001CA4B File Offset: 0x0001AC4B
		public virtual void Dispose()
		{
			this.mParticleGroup = null;
			this.mParticleDefInstanceVector.Clear();
		}

		// Token: 0x04000617 RID: 1559
		public List<PIParticleDefInstance> mParticleDefInstanceVector = new List<PIParticleDefInstance>();

		// Token: 0x04000618 RID: 1560
		public PIParticleGroup mParticleGroup = new PIParticleGroup();
	}
}
