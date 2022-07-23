using System;
using SexyFramework.Drivers.App;

namespace SexyFramework.Drivers.File
{
	// Token: 0x02000018 RID: 24
	public class XNAFile : IFile
	{
		// Token: 0x0600012C RID: 300 RVA: 0x00005321 File Offset: 0x00003521
		public XNAFile(string name, XNAFileDriver driver)
		{
			this.mFileName = name;
			this.mIsLoaded = false;
			this.mData = null;
			this.mStatus = IFile.Status.READ_PENDING;
			this.mFileDriver = driver;
			this.mDataObject = null;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005353 File Offset: 0x00003553
		public override void Dispose()
		{
			this.mData = null;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000535C File Offset: 0x0000355C
		public override bool IsLoaded()
		{
			return this.mIsLoaded;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00005364 File Offset: 0x00003564
		public override bool HasError()
		{
			return this.mStatus == IFile.Status.READ_ERROR;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000536F File Offset: 0x0000356F
		public override void AsyncLoad()
		{
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005374 File Offset: 0x00003574
		public override bool ForceLoad()
		{
			bool result;
			try
			{
				this.mData = this.mFileDriver.GetContentManager().Load<byte[]>(this.mFileName);
				this.mStatus = IFile.Status.READ_COMPLETE;
				result = true;
			}
			catch (Exception)
			{
				this.mStatus = IFile.Status.READ_ERROR;
				result = false;
			}
			return result;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000053C8 File Offset: 0x000035C8
		public override byte[] GetBuffer()
		{
			return this.mData;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000053D0 File Offset: 0x000035D0
		public override bool ForceLoadObject<T>()
		{
			bool result;
			try
			{
				this.mDataObject = ((WP7ContentManager)this.mFileDriver.GetContentManager()).LoadResDirectly<T>(this.mFileName);
				this.mStatus = IFile.Status.READ_COMPLETE;
				result = true;
			}
			catch (Exception)
			{
				this.mStatus = IFile.Status.READ_ERROR;
				result = false;
			}
			return result;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000542C File Offset: 0x0000362C
		public override object GetObject()
		{
			return this.mDataObject;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005434 File Offset: 0x00003634
		public override uint GetSize()
		{
			return (uint)this.mData.Length;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000543E File Offset: 0x0000363E
		public override void Close()
		{
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005440 File Offset: 0x00003640
		public override void DirectSeek(long theSeekPoint)
		{
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005442 File Offset: 0x00003642
		public override bool DirectRead(byte theBuffer, long theReadSize)
		{
			return false;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005445 File Offset: 0x00003645
		public override IFile.Status DirectReadStatus()
		{
			return this.mStatus;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000544D File Offset: 0x0000364D
		public override long DirectReadBlockSize()
		{
			return 0L;
		}

		// Token: 0x04000048 RID: 72
		protected string mFileName;

		// Token: 0x04000049 RID: 73
		protected bool mIsLoaded;

		// Token: 0x0400004A RID: 74
		protected byte[] mData;

		// Token: 0x0400004B RID: 75
		protected IFile.Status mStatus;

		// Token: 0x0400004C RID: 76
		protected object mDataObject;

		// Token: 0x0400004D RID: 77
		protected XNAFileDriver mFileDriver;
	}
}
