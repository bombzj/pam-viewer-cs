using System;

namespace SexyFramework.Misc
{
	// Token: 0x02000127 RID: 295
	public struct CritSect
	{
		// Token: 0x060009C5 RID: 2501 RVA: 0x000331E5 File Offset: 0x000313E5
		public bool TryLock()
		{
			return false;
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x000331E8 File Offset: 0x000313E8
		public void Lock()
		{
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x000331EA File Offset: 0x000313EA
		public void Unlock()
		{
		}
	}
}
