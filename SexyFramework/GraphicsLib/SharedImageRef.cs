using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000FA RID: 250
	public class SharedImageRef
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x000261D0 File Offset: 0x000243D0
		public int mWidth
		{
			get
			{
				if (this.mSharedImage != null)
				{
					return this.mSharedImage.mImage.mWidth;
				}
				return 0;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x000261EC File Offset: 0x000243EC
		public int mHeight
		{
			get
			{
				if (this.mSharedImage != null)
				{
					return this.mSharedImage.mImage.mHeight;
				}
				return 0;
			}
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00026208 File Offset: 0x00024408
		public SharedImageRef()
		{
			this.mSharedImage = new SharedImage();
			this.mUnsharedImage = null;
			this.mOwnsUnshared = false;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0002622C File Offset: 0x0002442C
		public SharedImageRef(SharedImageRef theSharedImageRef)
		{
			this.mSharedImage = theSharedImageRef.mSharedImage;
			if (this.mSharedImage != null)
			{
				this.mSharedImage.mRefCount++;
			}
			this.mUnsharedImage = theSharedImageRef.mUnsharedImage;
			this.mOwnsUnshared = false;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x00026279 File Offset: 0x00024479
		public SharedImageRef(SharedImage theSharedImage)
		{
			this.mSharedImage = theSharedImage;
			if (theSharedImage != null)
			{
				this.mSharedImage.mRefCount++;
			}
			this.mUnsharedImage = null;
			this.mOwnsUnshared = false;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x000262AC File Offset: 0x000244AC
		public virtual void Dispose()
		{
			this.Release();
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x000262B4 File Offset: 0x000244B4
		public void CopyFrom(SharedImageRef theSharedImageRef)
		{
			this.Release();
			this.mSharedImage = theSharedImageRef.mSharedImage;
			this.mUnsharedImage = theSharedImageRef.mUnsharedImage;
			if (this.mSharedImage != null)
			{
				this.mSharedImage.mRefCount++;
			}
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x000262EF File Offset: 0x000244EF
		public void CopyFrom(SharedImage theSharedImage)
		{
			this.Release();
			this.mSharedImage = theSharedImage;
			this.mSharedImage.mRefCount++;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00026311 File Offset: 0x00024511
		public void CopyFrom(MemoryImage theUnsharedImage)
		{
			this.Release();
			this.mUnsharedImage = theUnsharedImage;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00026320 File Offset: 0x00024520
		public void Release()
		{
			if (this.mOwnsUnshared)
			{
				if (this.mUnsharedImage != null)
				{
					this.mUnsharedImage.Dispose();
				}
				this.mUnsharedImage = null;
			}
			if (this.mSharedImage != null && --this.mSharedImage.mRefCount == 0)
			{
				GlobalMembers.gSexyAppBase.mCleanupSharedImages = true;
			}
			this.mSharedImage = null;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00026381 File Offset: 0x00024581
		internal DeviceImage GetDeviceImage()
		{
			if (this.mSharedImage != null)
			{
				return this.mSharedImage.mImage;
			}
			return null;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00026398 File Offset: 0x00024598
		public Image GetImage()
		{
			return this.GetMemoryImage();
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x000263A0 File Offset: 0x000245A0
		public MemoryImage GetMemoryImage()
		{
			if (this.mUnsharedImage != null)
			{
				return this.mUnsharedImage;
			}
			return this.GetDeviceImage();
		}

		// Token: 0x040006D2 RID: 1746
		public SharedImage mSharedImage;

		// Token: 0x040006D3 RID: 1747
		public MemoryImage mUnsharedImage;

		// Token: 0x040006D4 RID: 1748
		public bool mOwnsUnshared;
	}
}
