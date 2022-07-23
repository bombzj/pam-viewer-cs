using System;
using SexyFramework.Misc;

namespace SexyFramework.Drivers.Profile
{
	// Token: 0x0200006B RID: 107
	public class UserProfile
	{
		// Token: 0x0600043A RID: 1082 RVA: 0x0000CC45 File Offset: 0x0000AE45
		public UserProfile()
		{
			this.mPlatformData = IProfileData.CreateProfileData(this);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000CC59 File Offset: 0x0000AE59
		public virtual int GetId()
		{
			return this.mPlatformData.GetId();
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000CC66 File Offset: 0x0000AE66
		public virtual string GetName()
		{
			return this.mPlatformData.GetName();
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000CC73 File Offset: 0x0000AE73
		public virtual uint GetGamepadIndex()
		{
			return this.mPlatformData.GetGamepadIndex();
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000CC80 File Offset: 0x0000AE80
		public virtual void SetGamepadIndex(uint gamepad)
		{
			this.mPlatformData.SetGamepadIndex(gamepad);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000CC8E File Offset: 0x0000AE8E
		public virtual void Reset()
		{
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000CC90 File Offset: 0x0000AE90
		public virtual void DeleteUserFiles()
		{
			this.mPlatformData.DeleteUserFiles();
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000CC9D File Offset: 0x0000AE9D
		public virtual EProfileIOState LoadDetails()
		{
			return this.mPlatformData.LoadDetails();
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000CCAA File Offset: 0x0000AEAA
		public virtual bool IsLoading()
		{
			return this.mPlatformData.IsLoading();
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000CCB7 File Offset: 0x0000AEB7
		public virtual bool IsLoaded()
		{
			return this.mPlatformData.IsLoaded();
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000CCC4 File Offset: 0x0000AEC4
		public virtual EProfileIOState SaveDetails()
		{
			return this.mPlatformData.SaveDetails();
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000CCD1 File Offset: 0x0000AED1
		public virtual bool IsSaving()
		{
			return this.mPlatformData.IsSaving();
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000CCDE File Offset: 0x0000AEDE
		public virtual bool IsSaved()
		{
			return this.mPlatformData.IsSaved();
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000CCEB File Offset: 0x0000AEEB
		public virtual bool HasError()
		{
			return this.mPlatformData.HasError();
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000CCF8 File Offset: 0x0000AEF8
		public virtual bool SignedIn()
		{
			return this.mPlatformData.SignedIn();
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0000CD05 File Offset: 0x0000AF05
		public virtual bool IsSigningIn()
		{
			return this.mPlatformData.IsSigningIn();
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000CD12 File Offset: 0x0000AF12
		public virtual bool IsOnline()
		{
			return this.mPlatformData.IsOnline();
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0000CD1F File Offset: 0x0000AF1F
		public virtual bool ReadProfileSettings(SexyBuffer theData)
		{
			theData.ReadInt32();
			return true;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000CD29 File Offset: 0x0000AF29
		public virtual bool WriteProfileSettings(SexyBuffer theData)
		{
			theData.WriteInt32(1234);
			return true;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000CD37 File Offset: 0x0000AF37
		public bool IsAchievementUnlocked(uint id)
		{
			return this.mPlatformData.IsAchievementUnlocked(id);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000CD45 File Offset: 0x0000AF45
		public IAchievementContext StartUnlockAchievement(uint id)
		{
			return this.mPlatformData.StartUnlockAchievement(id);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000CD53 File Offset: 0x0000AF53
		public IProfileData GetPlatformData()
		{
			return this.mPlatformData;
		}

		// Token: 0x04000223 RID: 547
		private IProfileData mPlatformData;
	}
}
