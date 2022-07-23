using System;

namespace SexyFramework.Drivers.Profile
{
	// Token: 0x02000069 RID: 105
	public class ProfileManager
	{
		// Token: 0x06000426 RID: 1062 RVA: 0x0000CB60 File Offset: 0x0000AD60
		public ProfileManager(ProfileEventListener listener)
		{
			this.mListener = listener;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000CB6F File Offset: 0x0000AD6F
		public virtual bool Init()
		{
			return true;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000CB72 File Offset: 0x0000AD72
		public virtual void Update()
		{
			GlobalMembers.gSexyAppBase.mProfileDriver.Update();
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000CB83 File Offset: 0x0000AD83
		public virtual uint GetNumProfiles()
		{
			return GlobalMembers.gSexyAppBase.mProfileDriver.GetNumProfiles();
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000CB94 File Offset: 0x0000AD94
		public virtual bool HasProfile(string theName)
		{
			return GlobalMembers.gSexyAppBase.mProfileDriver.HasProfile(theName);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000CBA6 File Offset: 0x0000ADA6
		public virtual UserProfile GetProfile(int index)
		{
			return GlobalMembers.gSexyAppBase.mProfileDriver.GetProfile(index);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000CBB8 File Offset: 0x0000ADB8
		public virtual UserProfile GetProfile(string theName)
		{
			return GlobalMembers.gSexyAppBase.mProfileDriver.GetProfile(theName);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000CBCA File Offset: 0x0000ADCA
		public virtual UserProfile GetAnyProfile()
		{
			return GlobalMembers.gSexyAppBase.mProfileDriver.GetAnyProfile();
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000CBDB File Offset: 0x0000ADDB
		public virtual void ClearProfiles()
		{
			GlobalMembers.gSexyAppBase.mProfileDriver.ClearProfiles();
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000CBEC File Offset: 0x0000ADEC
		public virtual UserProfile AddProfile(string theName)
		{
			return GlobalMembers.gSexyAppBase.mProfileDriver.AddProfile(theName);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000CBFE File Offset: 0x0000ADFE
		public virtual bool DeleteProfile(string theName)
		{
			return GlobalMembers.gSexyAppBase.mProfileDriver.DeleteProfile(theName);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000CC10 File Offset: 0x0000AE10
		public virtual bool RenameProfile(string theOldName, string theNewName)
		{
			return GlobalMembers.gSexyAppBase.mProfileDriver.RenameProfile(theOldName, theNewName);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000CC23 File Offset: 0x0000AE23
		public uint GetProfileVersion()
		{
			return this.mListener.GetProfileVersion();
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000CC30 File Offset: 0x0000AE30
		public ProfileEventListener GetListener()
		{
			return this.mListener;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000CC38 File Offset: 0x0000AE38
		public UserProfile CreateUserProfile()
		{
			return this.mListener.CreateUserProfile();
		}

		// Token: 0x04000222 RID: 546
		protected ProfileEventListener mListener;
	}
}
