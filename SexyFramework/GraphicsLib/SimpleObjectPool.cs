using System;
using System.Collections.Generic;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000F1 RID: 241
	public class SimpleObjectPool<T> : IDisposable where T : new()
	{
		// Token: 0x060006CC RID: 1740 RVA: 0x0001D0F1 File Offset: 0x0001B2F1
		public SimpleObjectPool()
		{
			this.mNumPools = 0;
			this.mNumAvailObjects = 0;
			this.mFreeData = null;
			this.mDataPools = null;
			this.mDataPools = new List<T>();
			this.mFreeData = new List<T>();
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001D12B File Offset: 0x0001B32B
		public virtual void Dispose()
		{
			this.mDataPools.Clear();
			this.mFreeData.Clear();
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001D143 File Offset: 0x0001B343
		public void GrowPool()
		{
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0001D148 File Offset: 0x0001B348
		public T Alloc()
		{
			T t = default(T);
			t = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
			this.mDataPools.Add(t);
			return t;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001D18B File Offset: 0x0001B38B
		public void Free(T thePtr)
		{
			this.mDataPools.Remove(thePtr);
		}

		// Token: 0x0400067C RID: 1660
		public static int OBJECT_POOL_SIZE = 8192;

		// Token: 0x0400067D RID: 1661
		public int mNumPools;

		// Token: 0x0400067E RID: 1662
		public int mNumAvailObjects;

		// Token: 0x0400067F RID: 1663
		public List<T> mDataPools;

		// Token: 0x04000680 RID: 1664
		public List<T> mFreeData;
	}
}
