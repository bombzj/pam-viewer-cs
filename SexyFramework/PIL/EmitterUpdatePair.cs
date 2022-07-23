using System;

namespace SexyFramework.PIL
{
	// Token: 0x0200017A RID: 378
	public class EmitterUpdatePair
	{
		// Token: 0x06000D5C RID: 3420 RVA: 0x00041575 File Offset: 0x0003F775
		public EmitterUpdatePair(Emitter emitter, int value)
		{
			this.emitter = emitter;
			this.value = value;
		}

		// Token: 0x04000AE7 RID: 2791
		public Emitter emitter;

		// Token: 0x04000AE8 RID: 2792
		public int value;
	}
}
