using System;
using System.Collections.Generic;
using SexyFramework.Drivers.Profile;

namespace ZumasRevenge
{
	// Token: 0x02000131 RID: 305
	public class ZumaProfileMgr : ProfileManager
	{
		// Token: 0x06000F59 RID: 3929 RVA: 0x0009EEA8 File Offset: 0x0009D0A8
		public ZumaProfileMgr()
			: base(GameApp.gApp)
		{
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x0009EEB8 File Offset: 0x0009D0B8
		public void RenameTempProfile(string new_name)
		{
			if (this.GetProfile(".temp") != null)
			{
				bool flag = this.RenameProfile(".temp", new_name);
				if (flag)
				{
					return;
				}
			}
			this.AddProfile(new_name);
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0009EEEB File Offset: 0x0009D0EB
		public bool HasTempProfile()
		{
			return this.GetProfile(".temp") != null;
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x0009EF00 File Offset: 0x0009D100
		public void GetListOfUserNames(List<string> user_vec)
		{
			if (user_vec == null)
			{
				return;
			}
			int num = 0;
			while ((long)num < (long)((ulong)this.GetNumProfiles()))
			{
				UserProfile profile = this.GetProfile(num);
				user_vec.Insert(user_vec.Count, profile.GetName());
				num++;
			}
		}
	}
}
