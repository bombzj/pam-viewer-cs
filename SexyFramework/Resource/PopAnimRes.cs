using System;
using SexyFramework.WidgetsLib;

namespace SexyFramework.Resource
{
	// Token: 0x02000190 RID: 400
	public class PopAnimRes : BaseRes
	{
		// Token: 0x06000DDD RID: 3549 RVA: 0x00045E03 File Offset: 0x00044003
		public PopAnimRes()
		{
			this.mType = ResType.ResType_PopAnim;
			this.mPopAnim = null;
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x00045E1C File Offset: 0x0004401C
		public override void DeleteResource()
		{
			if (this.mResourceRef != null && this.mResourceRef.HasResource())
			{
				this.mResourceRef.Release();
			}
			else if (this.mPopAnim != null)
			{
				this.mPopAnim.Dispose();
			}
			this.mPopAnim = null;
			if (this.mGlobalPtr != null)
			{
				this.mGlobalPtr.mResObject = null;
			}
		}

		// Token: 0x04000B6D RID: 2925
		public PopAnim mPopAnim;
	}
}
