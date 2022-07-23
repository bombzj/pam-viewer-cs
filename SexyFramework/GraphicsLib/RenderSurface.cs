using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000F5 RID: 245
	public class RenderSurface
	{
		// Token: 0x06000723 RID: 1827 RVA: 0x00025DEF File Offset: 0x00023FEF
		public RenderSurface()
		{
			this.mRefCount = 0U;
			this.mData = 0;
			this.mPtr = null;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00025E0C File Offset: 0x0002400C
		public virtual void Dispose()
		{
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00025E0E File Offset: 0x0002400E
		public void AddRef()
		{
			this.mRefCount += 1U;
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00025E1E File Offset: 0x0002401E
		public void Release()
		{
			this.mRefCount -= 1U;
			uint num = this.mRefCount;
		}

		// Token: 0x040006C5 RID: 1733
		public int mData;

		// Token: 0x040006C6 RID: 1734
		public object mPtr;

		// Token: 0x040006C7 RID: 1735
		private uint mRefCount;
	}
}
