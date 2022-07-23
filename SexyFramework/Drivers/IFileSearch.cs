using System;

namespace SexyFramework.Drivers
{
	// Token: 0x0200004E RID: 78
	public class IFileSearch
	{
		// Token: 0x0600037E RID: 894 RVA: 0x0000C2E8 File Offset: 0x0000A4E8
		public virtual void Dispose()
		{
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000C2EA File Offset: 0x0000A4EA
		public IFileSearch.SearchType GetSearchType()
		{
			return this.mSearchType;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000C2F2 File Offset: 0x0000A4F2
		protected IFileSearch()
		{
			this.mSearchType = IFileSearch.SearchType.UNKNOWN;
		}

		// Token: 0x040001AA RID: 426
		protected IFileSearch.SearchType mSearchType;

		// Token: 0x0200004F RID: 79
		public enum SearchType
		{
			// Token: 0x040001AC RID: 428
			UNKNOWN,
			// Token: 0x040001AD RID: 429
			PAK_FILE_INTERNAL,
			// Token: 0x040001AE RID: 430
			DRIVER_INTERNAL
		}
	}
}
