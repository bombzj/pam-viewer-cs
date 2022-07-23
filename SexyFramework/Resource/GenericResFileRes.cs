using System;

namespace SexyFramework.Resource
{
	// Token: 0x02000194 RID: 404
	public class GenericResFileRes : BaseRes
	{
		// Token: 0x06000DE5 RID: 3557 RVA: 0x00045F71 File Offset: 0x00044171
		public GenericResFileRes()
		{
			this.mType = ResType.ResType_GenericResFile;
			this.mGenericResFile = null;
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00045F87 File Offset: 0x00044187
		public override void DeleteResource()
		{
			if (this.mResourceRef != null && this.mResourceRef.HasResource())
			{
				this.mResourceRef.Release();
			}
			this.mGenericResFile = null;
			if (this.mGlobalPtr != null)
			{
				this.mGlobalPtr.mResObject = null;
			}
		}

		// Token: 0x04000B72 RID: 2930
		public GenericResFile mGenericResFile;
	}
}
