using System;

namespace SexyFramework.Drivers
{
	// Token: 0x02000019 RID: 25
	public abstract class IFileDriver
	{
		// Token: 0x0600013B RID: 315 RVA: 0x00005451 File Offset: 0x00003651
		public virtual void Dispose()
		{
		}

		// Token: 0x0600013C RID: 316
		public abstract bool InitFileDriver(SexyAppBase theApp);

		// Token: 0x0600013D RID: 317
		public abstract void InitSaveDataFolder();

		// Token: 0x0600013E RID: 318 RVA: 0x00005453 File Offset: 0x00003653
		public virtual string FixPath(string theFileName)
		{
			return theFileName;
		}

		// Token: 0x0600013F RID: 319
		public abstract string GetSaveDataPath();

		// Token: 0x06000140 RID: 320
		public abstract string GetCurPath();

		// Token: 0x06000141 RID: 321
		public abstract string GetLoadDataPath();

		// Token: 0x06000142 RID: 322
		public abstract IFile CreateFile(string thePath);

		// Token: 0x06000143 RID: 323
		public abstract IFile CreateFileWithBuffer(string thePath, byte theBuffer, uint theBufferSize);

		// Token: 0x06000144 RID: 324
		public abstract IFile CreateFileDirect(string thePath);

		// Token: 0x06000145 RID: 325 RVA: 0x00005456 File Offset: 0x00003656
		public virtual bool SupportsMemoryMappedFiles()
		{
			return false;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005459 File Offset: 0x00003659
		public virtual IFile CreateFileMemoryMapped(string thePath)
		{
			return null;
		}

		// Token: 0x06000147 RID: 327
		public abstract long GetFileSize(string thePath);

		// Token: 0x06000148 RID: 328
		public abstract long GetFileTime(string thePath);

		// Token: 0x06000149 RID: 329
		public abstract bool FileExists(string thePath, ref bool isFolder);

		// Token: 0x0600014A RID: 330
		public abstract bool MakeFolders(string theFolder);

		// Token: 0x0600014B RID: 331
		public abstract bool DeleteTree(string thePath);

		// Token: 0x0600014C RID: 332
		public abstract bool DeleteFile(string thePath);

		// Token: 0x0600014D RID: 333 RVA: 0x0000545C File Offset: 0x0000365C
		public virtual bool MoveFile(string thePathSrc, string thePathDest)
		{
			return false;
		}

		// Token: 0x0600014E RID: 334
		public abstract IFileSearch FileSearchStart(string theCriteria, FileSearchInfo outInfo);

		// Token: 0x0600014F RID: 335
		public abstract bool FileSearchNext(IFileSearch theSearch, FileSearchInfo theInfo);

		// Token: 0x06000150 RID: 336
		public abstract bool FileSearchEnd(IFileSearch theInfo);
	}
}
