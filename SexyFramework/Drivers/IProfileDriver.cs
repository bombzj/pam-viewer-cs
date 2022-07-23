using System;
using SexyFramework.Drivers.Profile;

namespace SexyFramework.Drivers
{
	// Token: 0x0200005D RID: 93
	public abstract class IProfileDriver
	{
		// Token: 0x060003B5 RID: 949 RVA: 0x0000C38F File Offset: 0x0000A58F
		public static IProfileDriver CreateProfileDriver()
		{
			return new FilesystemProfileDriver();
		}

		// Token: 0x060003B6 RID: 950
		public abstract bool Init();

		// Token: 0x060003B7 RID: 951
		public abstract void Update();

		// Token: 0x060003B8 RID: 952
		public abstract uint GetNumProfiles();

		// Token: 0x060003B9 RID: 953
		public abstract bool HasProfile(string theName);

		// Token: 0x060003BA RID: 954
		public abstract UserProfile GetProfile(int index);

		// Token: 0x060003BB RID: 955
		public abstract UserProfile GetProfile(string theName);

		// Token: 0x060003BC RID: 956
		public abstract UserProfile GetAnyProfile();

		// Token: 0x060003BD RID: 957
		public abstract UserProfile AddProfile(string theName);

		// Token: 0x060003BE RID: 958
		public abstract bool DeleteProfile(string theName);

		// Token: 0x060003BF RID: 959
		public abstract bool RenameProfile(string theOldName, string theNewName);

		// Token: 0x060003C0 RID: 960
		public abstract void ClearProfiles();
	}
}
