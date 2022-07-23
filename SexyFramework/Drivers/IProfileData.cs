using System;
using SexyFramework.Drivers.Profile;
using SexyFramework.GraphicsLib;

namespace SexyFramework.Drivers
{
	// Token: 0x0200005E RID: 94
	public abstract class IProfileData
	{
		// Token: 0x060003C2 RID: 962 RVA: 0x0000C39E File Offset: 0x0000A59E
		public static IProfileData CreateProfileData(UserProfile player)
		{
			return new FilesystemProfileData(player);
		}

		// Token: 0x060003C3 RID: 963
		public abstract int GetId();

		// Token: 0x060003C4 RID: 964
		public abstract string GetName();

		// Token: 0x060003C5 RID: 965
		public abstract Image GetPlayerIcon();

		// Token: 0x060003C6 RID: 966
		public abstract uint GetGamepadIndex();

		// Token: 0x060003C7 RID: 967
		public abstract void SetGamepadIndex(uint gamepad);

		// Token: 0x060003C8 RID: 968
		public abstract bool SignedIn();

		// Token: 0x060003C9 RID: 969
		public abstract bool IsSigningIn();

		// Token: 0x060003CA RID: 970
		public abstract bool IsOnline();

		// Token: 0x060003CB RID: 971
		public abstract EProfileIOState LoadDetails();

		// Token: 0x060003CC RID: 972
		public abstract bool IsLoading();

		// Token: 0x060003CD RID: 973
		public abstract bool IsLoaded();

		// Token: 0x060003CE RID: 974
		public abstract EProfileIOState SaveDetails();

		// Token: 0x060003CF RID: 975
		public abstract bool IsSaving();

		// Token: 0x060003D0 RID: 976
		public abstract bool IsSaved();

		// Token: 0x060003D1 RID: 977
		public abstract bool HasError();

		// Token: 0x060003D2 RID: 978
		public abstract void DeleteUserFiles();

		// Token: 0x060003D3 RID: 979
		public abstract bool IsAchievementUnlocked(uint id);

		// Token: 0x060003D4 RID: 980
		public abstract IAchievementContext StartUnlockAchievement(uint id);
	}
}
