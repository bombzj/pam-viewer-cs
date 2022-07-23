using System;
using SexyFramework.Drivers;

namespace SexyFramework.Resource
{
	// Token: 0x02000188 RID: 392
	public class PFILE
	{
		// Token: 0x06000DBF RID: 3519 RVA: 0x000451AD File Offset: 0x000433AD
		public PFILE(string theFilename, string mode)
		{
			this.mFilename = theFilename;
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x000451BC File Offset: 0x000433BC
		internal void Close()
		{
			try
			{
				if (this.mFileHandler != null)
				{
					this.mFileHandler.Close();
					this.mFileHandler = null;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x000451F8 File Offset: 0x000433F8
		public bool Open()
		{
			bool result;
			try
			{
				this.mFileHandler = Common.GetGameFileDriver().CreateFile(this.mFilename);
				this.mFileHandler.ForceLoad();
				result = !this.mFileHandler.HasError();
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x00045250 File Offset: 0x00043450
		public bool Open<T>()
		{
			bool result;
			try
			{
				this.mFileHandler = Common.GetGameFileDriver().CreateFile(this.mFilename);
				this.mFileHandler.ForceLoadObject<T>();
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0004529C File Offset: 0x0004349C
		public byte[] GetData()
		{
			if (this.mFileHandler != null)
			{
				return this.mFileHandler.GetBuffer();
			}
			return null;
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x000452B3 File Offset: 0x000434B3
		public object GetObject()
		{
			if (this.mFileHandler != null)
			{
				return this.mFileHandler.GetObject();
			}
			return null;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x000452CA File Offset: 0x000434CA
		public bool IsEndOfFile()
		{
			return true;
		}

		// Token: 0x04000B24 RID: 2852
		private string mFilename;

		// Token: 0x04000B25 RID: 2853
		private IFile mFileHandler;
	}
}
