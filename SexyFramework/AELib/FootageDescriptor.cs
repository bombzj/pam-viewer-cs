using System;

namespace SexyFramework.AELib
{
	// Token: 0x02000002 RID: 2
	internal class FootageDescriptor
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public FootageDescriptor()
		{
			this.mShortName = "";
			this.mId = -1L;
			this.mWidth = 0L;
			this.mHeight = 0L;
			this.mFullName = "";
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002086 File Offset: 0x00000286
		public FootageDescriptor(string sn, long id, string fn, long w, long h)
		{
			this.mShortName = sn;
			this.mId = id;
			this.mFullName = fn;
			this.mWidth = w;
			this.mHeight = h;
		}

		// Token: 0x04000001 RID: 1
		public string mShortName;

		// Token: 0x04000002 RID: 2
		public long mId;

		// Token: 0x04000003 RID: 3
		public long mWidth;

		// Token: 0x04000004 RID: 4
		public long mHeight;

		// Token: 0x04000005 RID: 5
		public string mFullName;
	}
}
