using System;

namespace SexyFramework.Drivers
{
	// Token: 0x02000016 RID: 22
	public abstract class IFile
	{
		// Token: 0x0600011D RID: 285 RVA: 0x00005317 File Offset: 0x00003517
		public virtual void Dispose()
		{
		}

		// Token: 0x0600011E RID: 286
		public abstract bool IsLoaded();

		// Token: 0x0600011F RID: 287
		public abstract bool HasError();

		// Token: 0x06000120 RID: 288
		public abstract void AsyncLoad();

		// Token: 0x06000121 RID: 289
		public abstract bool ForceLoad();

		// Token: 0x06000122 RID: 290
		public abstract bool ForceLoadObject<T>();

		// Token: 0x06000123 RID: 291
		public abstract byte[] GetBuffer();

		// Token: 0x06000124 RID: 292
		public abstract object GetObject();

		// Token: 0x06000125 RID: 293
		public abstract uint GetSize();

		// Token: 0x06000126 RID: 294
		public abstract void Close();

		// Token: 0x06000127 RID: 295
		public abstract void DirectSeek(long theSeekPoint);

		// Token: 0x06000128 RID: 296
		public abstract bool DirectRead(byte theBuffer, long theReadSize);

		// Token: 0x06000129 RID: 297
		public abstract IFile.Status DirectReadStatus();

		// Token: 0x0600012A RID: 298
		public abstract long DirectReadBlockSize();

		// Token: 0x02000017 RID: 23
		public enum Status
		{
			// Token: 0x04000045 RID: 69
			READ_COMPLETE,
			// Token: 0x04000046 RID: 70
			READ_PENDING,
			// Token: 0x04000047 RID: 71
			READ_ERROR
		}
	}
}
