using System;

namespace SexyFramework.Drivers
{
	// Token: 0x0200001E RID: 30
	public abstract class IGamepadDriver
	{
		// Token: 0x0600017F RID: 383 RVA: 0x0000563A File Offset: 0x0000383A
		public virtual void Dispose()
		{
		}

		// Token: 0x06000180 RID: 384
		public abstract int InitGamepadDriver(SexyAppBase NamelessParameter);

		// Token: 0x06000181 RID: 385
		public abstract IGamepad GetGamepad(int theIndex);

		// Token: 0x06000182 RID: 386
		public abstract void Update();
	}
}
