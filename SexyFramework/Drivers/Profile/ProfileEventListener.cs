using System;
using SexyFramework.Misc;

namespace SexyFramework.Drivers.Profile
{
	// Token: 0x0200006A RID: 106
	public interface ProfileEventListener
	{
		// Token: 0x06000435 RID: 1077
		uint GetProfileVersion();

		// Token: 0x06000436 RID: 1078
		void NotifyProfileChanged(UserProfile player);

		// Token: 0x06000437 RID: 1079
		UserProfile CreateUserProfile();

		// Token: 0x06000438 RID: 1080
		void OnProfileLoad(UserProfile player, SexyBuffer buffer);

		// Token: 0x06000439 RID: 1081
		void OnProfileSave(UserProfile player, SexyBuffer buffer);
	}
}
