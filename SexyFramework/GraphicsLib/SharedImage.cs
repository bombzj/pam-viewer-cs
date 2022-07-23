using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000F9 RID: 249
	public class SharedImage
	{
		// Token: 0x0600074E RID: 1870 RVA: 0x00026174 File Offset: 0x00024374
		public string ToString()
		{
			if (string.Concat(new object[] { "RefCount(", this.mRefCount, "):", this.mImage }) == null)
			{
				return "NULL";
			}
			return this.mImage.ToString();
		}

		// Token: 0x040006CF RID: 1743
		public DeviceImage mImage;

		// Token: 0x040006D0 RID: 1744
		public int mRefCount;

		// Token: 0x040006D1 RID: 1745
		public bool mLoading;
	}
}
