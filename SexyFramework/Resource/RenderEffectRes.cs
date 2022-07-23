using System;
using SexyFramework.GraphicsLib;

namespace SexyFramework.Resource
{
	// Token: 0x02000192 RID: 402
	public class RenderEffectRes : BaseRes
	{
		// Token: 0x06000DE1 RID: 3553 RVA: 0x00045EED File Offset: 0x000440ED
		public RenderEffectRes()
		{
			this.mType = ResType.ResType_RenderEffect;
			this.mRenderEffectDefinition = null;
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00045F04 File Offset: 0x00044104
		public override void DeleteResource()
		{
			if (this.mResourceRef != null && this.mResourceRef.HasResource())
			{
				this.mResourceRef.Release();
			}
			else if (this.mRenderEffectDefinition != null)
			{
				this.mRenderEffectDefinition.Dispose();
			}
			this.mRenderEffectDefinition = null;
			if (this.mGlobalPtr != null)
			{
				this.mGlobalPtr.mResObject = null;
			}
		}

		// Token: 0x04000B6F RID: 2927
		public RenderEffectDefinition mRenderEffectDefinition;

		// Token: 0x04000B70 RID: 2928
		public string mSrcFilePath;
	}
}
