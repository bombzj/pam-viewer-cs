using System;
using SexyFramework.Drivers.Profile;

namespace ZumasRevenge.Profile
{
	// Token: 0x02000132 RID: 306
	public class Profile
	{
		// Token: 0x06000F5D RID: 3933 RVA: 0x0009EF3E File Offset: 0x0009D13E
		public void loadProfile()
		{
			this.fspd.LoadDetails();
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0009EF4C File Offset: 0x0009D14C
		public void saveAll()
		{
			this.fspd.SaveDetails();
		}

		// Token: 0x04000F5C RID: 3932
		private FilesystemProfileData fspd = new FilesystemProfileData(new UserProfile());

		// Token: 0x04000F5D RID: 3933
		private int aa;
	}
}
