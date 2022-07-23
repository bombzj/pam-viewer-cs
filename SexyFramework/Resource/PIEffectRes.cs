using System;
using SexyFramework.GraphicsLib;

namespace SexyFramework.Resource
{
	// Token: 0x02000191 RID: 401
	public class PIEffectRes : BaseRes
	{
		// Token: 0x06000DDF RID: 3551 RVA: 0x00045E79 File Offset: 0x00044079
		public PIEffectRes()
		{
			this.mType = ResType.ResType_PIEffect;
			this.mPIEffect = null;
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x00045E90 File Offset: 0x00044090
		public override void DeleteResource()
		{
			if (this.mResourceRef != null && this.mResourceRef.HasResource())
			{
				this.mResourceRef.Release();
			}
			else if (this.mPIEffect != null)
			{
				this.mPIEffect.Dispose();
			}
			this.mPIEffect = null;
			if (this.mGlobalPtr != null)
			{
				this.mGlobalPtr.mResObject = null;
			}
		}

		// Token: 0x04000B6E RID: 2926
		public PIEffect mPIEffect;
	}
}
