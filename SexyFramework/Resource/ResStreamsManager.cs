using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.Resource
{
	// Token: 0x0200019C RID: 412
	public class ResStreamsManager
	{
		// Token: 0x06000E75 RID: 3701 RVA: 0x000498E1 File Offset: 0x00047AE1
		public int InitializeWithRSB(string theFileName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x000498E8 File Offset: 0x00047AE8
		public bool IsInitialized()
		{
			return false;
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x000498EB File Offset: 0x00047AEB
		public bool IsGroupLoaded(string theGroupName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x000498F2 File Offset: 0x00047AF2
		public bool IsGroupLoaded(int theGroupId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x000498F9 File Offset: 0x00047AF9
		public ResStreamsManager.GroupStatus GetGroupStatus(string theGroupName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00049900 File Offset: 0x00047B00
		public ResStreamsManager.GroupStatus GetGroupStatus(int theGroupId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00049907 File Offset: 0x00047B07
		public bool LoadGroup(string theGroupName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x0004990E File Offset: 0x00047B0E
		public bool LoadGroup(int theGroupId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00049915 File Offset: 0x00047B15
		public bool ForceLoadGroup(string theGroupName, string theDbgReason)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x0004991C File Offset: 0x00047B1C
		public bool ForceLoadGroup(int theGroupId, string theDbgReason)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00049923 File Offset: 0x00047B23
		public bool CanLoadGroup(string theGroupName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x0004992A File Offset: 0x00047B2A
		public bool CanLoadGroup(int theGroupId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x00049931 File Offset: 0x00047B31
		public bool DeleteGroup(string theGroupName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x00049938 File Offset: 0x00047B38
		public bool DeleteGroup(int theGroupId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0004993F File Offset: 0x00047B3F
		public bool HasError()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x00049946 File Offset: 0x00047B46
		public bool HasGlobalFileIndex()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x0004994D File Offset: 0x00047B4D
		public int GetGroupForFile(string theFileName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00049954 File Offset: 0x00047B54
		public int GetLoadedGroupForFile(string theFileName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x0004995B File Offset: 0x00047B5B
		public bool GetResidentFileBuffer(int theGroupId, string theFileName, ref byte[] theBuffer, ref int theSize)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00049962 File Offset: 0x00047B62
		public PFILE GetPakFileFromResidentBuffer(int theGroupId, string theFileName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00049969 File Offset: 0x00047B69
		public bool GetImage(int theGroupId, string theFileName, ref Image img)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00049970 File Offset: 0x00047B70
		public bool LoadResourcesManifest(ResourceManager theManager)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00049977 File Offset: 0x00047B77
		public int GetBytesLoadedForGroup(int theGroupId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x0004997E File Offset: 0x00047B7E
		public int GetTotalBytesForGroup(int theGroupId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00049985 File Offset: 0x00047B85
		public ResStreamsManager(SexyAppBase theApp)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00049992 File Offset: 0x00047B92
		public virtual void Dispose()
		{
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00049994 File Offset: 0x00047B94
		public void Update()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0004999B File Offset: 0x00047B9B
		public void DebugDraw(Graphics g, Rect aRegion)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x000499A2 File Offset: 0x00047BA2
		internal void ForceLoadGroup(string theGroup)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x000499A9 File Offset: 0x00047BA9
		internal int LookupGroup(string p)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0200019D RID: 413
		public enum GroupStatus
		{
			// Token: 0x04000BAE RID: 2990
			NOT_RESIDENT,
			// Token: 0x04000BAF RID: 2991
			DELETING,
			// Token: 0x04000BB0 RID: 2992
			PREPARING,
			// Token: 0x04000BB1 RID: 2993
			RESIDENT
		}
	}
}
