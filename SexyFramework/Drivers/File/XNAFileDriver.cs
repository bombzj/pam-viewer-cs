using System;
using Microsoft.Xna.Framework.Content;
using SexyFramework.Drivers.App;

namespace SexyFramework.Drivers.File
{
	// Token: 0x0200001A RID: 26
	public class XNAFileDriver : IFileDriver
	{
		// Token: 0x06000153 RID: 339 RVA: 0x0000546F File Offset: 0x0000366F
		public ContentManager GetContentManager()
		{
			return this.mContentManager;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00005477 File Offset: 0x00003677
		public override bool InitFileDriver(SexyAppBase theApp)
		{
			this.mApp = theApp;
			this.mContentManager = ((WP7AppDriver)this.mApp.mAppDriver).mContentManager;
			return this.mContentManager != null;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000054A7 File Offset: 0x000036A7
		public override void InitSaveDataFolder()
		{
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000054A9 File Offset: 0x000036A9
		public override string GetSaveDataPath()
		{
			return "";
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000054B0 File Offset: 0x000036B0
		public override string GetCurPath()
		{
			return "";
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000054B7 File Offset: 0x000036B7
		public override string GetLoadDataPath()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000054C0 File Offset: 0x000036C0
		public override IFile CreateFile(string thePath)
		{
			return new XNAFile(thePath, this);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000054D6 File Offset: 0x000036D6
		public override IFile CreateFileWithBuffer(string thePath, byte theBuffer, uint theBufferSize)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000054DD File Offset: 0x000036DD
		public override IFile CreateFileDirect(string thePath)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000054E4 File Offset: 0x000036E4
		public override long GetFileSize(string thePath)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000054EB File Offset: 0x000036EB
		public override long GetFileTime(string thePath)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000054F2 File Offset: 0x000036F2
		public override bool FileExists(string thePath, ref bool isFolder)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000054F9 File Offset: 0x000036F9
		public override bool MakeFolders(string theFolder)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00005500 File Offset: 0x00003700
		public override bool DeleteTree(string thePath)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00005507 File Offset: 0x00003707
		public override bool DeleteFile(string thePath)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000550E File Offset: 0x0000370E
		public override bool MoveFile(string thePathSrc, string thePathDest)
		{
			return false;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00005511 File Offset: 0x00003711
		public override IFileSearch FileSearchStart(string theCriteria, FileSearchInfo outInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00005518 File Offset: 0x00003718
		public override bool FileSearchNext(IFileSearch theSearch, FileSearchInfo theInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000551F File Offset: 0x0000371F
		public override bool FileSearchEnd(IFileSearch theInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400004E RID: 78
		protected SexyAppBase mApp;

		// Token: 0x0400004F RID: 79
		protected ContentManager mContentManager;
	}
}
