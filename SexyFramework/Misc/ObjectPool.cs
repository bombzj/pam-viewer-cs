using System;
using System.Collections.Generic;

namespace SexyFramework.Misc
{
	// Token: 0x02000138 RID: 312
	public class ObjectPool<T> where T : IDisposable, new()
	{
		// Token: 0x06000A40 RID: 2624 RVA: 0x00034EB8 File Offset: 0x000330B8
		public ObjectPool(int size)
		{
			this.mNumWant = 0;
			this.mNumAvailObjects = 0;
			this.mNextAvailIndex = 0;
			this.mPoolSize = size;
			this.mFreePools = new List<T>();
			this.mNumAvailObjects += this.mPoolSize;
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00034F08 File Offset: 0x00033108
		public virtual void Dispose()
		{
			for (int i = 0; i < this.mFreePools.Count; i++)
			{
				if (this.mFreePools[i] != null)
				{
					T t = this.mFreePools[i];
					t.Dispose();
				}
			}
			this.mFreePools.Clear();
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x00034F64 File Offset: 0x00033164
		public T Alloc()
		{
			if (this.mFreePools.Count > 0)
			{
				T result = this.mFreePools[this.mFreePools.Count - 1];
				this.mFreePools.RemoveAt(this.mFreePools.Count - 1);
				return result;
			}
			this.mNumWant++;
			return (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00034FE2 File Offset: 0x000331E2
		public void Free(T thePtr)
		{
			this.mFreePools.Add(thePtr);
		}

		// Token: 0x04000917 RID: 2327
		public int mPoolSize;

		// Token: 0x04000918 RID: 2328
		public int mNumWant;

		// Token: 0x04000919 RID: 2329
		public int mNumAvailObjects;

		// Token: 0x0400091A RID: 2330
		public List<T> mFreePools;

		// Token: 0x0400091B RID: 2331
		public int mNextAvailIndex;
	}
}
