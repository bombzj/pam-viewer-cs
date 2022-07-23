using System;
using SexyFramework.Drivers.Profile;
using SexyFramework.Misc;

namespace SexyFramework.Drivers
{
	// Token: 0x02000061 RID: 97
	public abstract class ISaveGameContext
	{
		// Token: 0x060003E0 RID: 992 RVA: 0x0000C3C0 File Offset: 0x0000A5C0
		public virtual void Dispose()
		{
		}

		// Token: 0x060003E1 RID: 993
		public abstract UserProfile GetPlayer();

		// Token: 0x060003E2 RID: 994
		public abstract string GetSaveName();

		// Token: 0x060003E3 RID: 995
		public abstract string GetSegmentName();

		// Token: 0x060003E4 RID: 996
		public abstract bool IsLoading();

		// Token: 0x060003E5 RID: 997
		public abstract bool IsSaving();

		// Token: 0x060003E6 RID: 998
		public abstract bool IsDeleting();

		// Token: 0x060003E7 RID: 999
		public abstract void Update();

		// Token: 0x060003E8 RID: 1000
		public abstract bool HasError();

		// Token: 0x060003E9 RID: 1001
		public abstract bool IsDone();

		// Token: 0x060003EA RID: 1002
		public abstract SexyBuffer GetBuffer();

		// Token: 0x060003EB RID: 1003
		public abstract void SetDisplayName(string name);

		// Token: 0x060003EC RID: 1004
		public abstract string GetDisplayName();

		// Token: 0x060003ED RID: 1005
		public abstract void SetDisplayDetails(string name);

		// Token: 0x060003EE RID: 1006
		public abstract string GetDisplayDetails();

		// Token: 0x060003EF RID: 1007
		public abstract void SetIconFilename(string iconFile);

		// Token: 0x060003F0 RID: 1008
		public abstract string GetIconFilename();

		// Token: 0x060003F1 RID: 1009
		public abstract void Destroy();
	}
}
