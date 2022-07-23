using System;
using System.Collections.Generic;
using System.Linq;
using SexyFramework.File;
using SexyFramework.Misc;

namespace SexyFramework.Drivers.Profile
{
	// Token: 0x02000068 RID: 104
	public class FilesystemProfileDriver : IProfileDriver
	{
		// Token: 0x06000413 RID: 1043 RVA: 0x0000C5F0 File Offset: 0x0000A7F0
		public FilesystemProfileDriver()
		{
			this.ClearProfiles();
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000C609 File Offset: 0x0000A809
		public override bool Init()
		{
			if (GlobalMembers.gSexyAppBase.mProfileManager == null)
			{
				return false;
			}
			this.Load();
			return true;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000C620 File Offset: 0x0000A820
		public override void Update()
		{
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000C622 File Offset: 0x0000A822
		public bool HasError()
		{
			return false;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000C625 File Offset: 0x0000A825
		public override uint GetNumProfiles()
		{
			return (uint)this.mProfileMap.Count;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000C632 File Offset: 0x0000A832
		public override bool HasProfile(string theName)
		{
			return this.mProfileMap.ContainsKey(theName) && this.mProfileMap[theName] != null;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000C658 File Offset: 0x0000A858
		public override UserProfile GetProfile(int index)
		{
			if (index >= this.mProfileMap.Count)
			{
				return null;
			}
			UserProfile userProfile = Enumerable.ElementAt<UserProfile>(this.mProfileMap.Values, index);
			if (userProfile == null)
			{
				return null;
			}
			FilesystemProfileData filesystemProfileData = (FilesystemProfileData)userProfile.GetPlatformData();
			userProfile.LoadDetails();
			filesystemProfileData.SetUseSeq((int)this.mNextProfileUseSeq++);
			return userProfile;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000C6B8 File Offset: 0x0000A8B8
		public override UserProfile GetProfile(string theName)
		{
			if (!this.mProfileMap.ContainsKey(theName))
			{
				return null;
			}
			UserProfile userProfile = this.mProfileMap[theName];
			if (userProfile == null)
			{
				return null;
			}
			FilesystemProfileData filesystemProfileData = (FilesystemProfileData)userProfile.GetPlatformData();
			userProfile.LoadDetails();
			filesystemProfileData.SetUseSeq((int)this.mNextProfileUseSeq++);
			return userProfile;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000C712 File Offset: 0x0000A912
		public override UserProfile GetAnyProfile()
		{
			return this.GetProfile(0);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000C71C File Offset: 0x0000A91C
		public override UserProfile AddProfile(string theName)
		{
			if (this.mProfileMap.ContainsKey(theName))
			{
				return null;
			}
			UserProfile userProfile = GlobalMembers.gSexyAppBase.mProfileManager.CreateUserProfile();
			FilesystemProfileData filesystemProfileData = (FilesystemProfileData)userProfile.GetPlatformData();
			filesystemProfileData.SetName(theName);
			filesystemProfileData.SetId((int)this.mNextProfileId++);
			filesystemProfileData.SetUseSeq((int)this.mNextProfileUseSeq++);
			this.mProfileMap.Add(theName, userProfile);
			this.DeleteOldProfiles();
			this.Save();
			return userProfile;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000C7A4 File Offset: 0x0000A9A4
		public override bool DeleteProfile(string theName)
		{
			if (this.mProfileMap.ContainsKey(theName))
			{
				this.mProfileMap[theName].DeleteUserFiles();
				this.mProfileMap.Remove(theName);
				this.Save();
				return true;
			}
			return false;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000C7DC File Offset: 0x0000A9DC
		public override bool RenameProfile(string theOldName, string theNewName)
		{
			if (!this.mProfileMap.ContainsKey(theOldName))
			{
				return false;
			}
			if (string.Compare(theOldName.ToLower(), theNewName.ToLower()) == 0)
			{
				FilesystemProfileData filesystemProfileData = (FilesystemProfileData)this.mProfileMap[theOldName].GetPlatformData();
				filesystemProfileData.SetName(theNewName);
				return true;
			}
			if (this.mProfileMap.ContainsKey(theNewName))
			{
				return false;
			}
			this.mProfileMap.Add(theNewName, this.mProfileMap[theOldName]);
			this.mProfileMap.Remove(theOldName);
			FilesystemProfileData filesystemProfileData2 = (FilesystemProfileData)this.mProfileMap[theNewName].GetPlatformData();
			filesystemProfileData2.SetName(theNewName);
			this.Save();
			return true;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000C886 File Offset: 0x0000AA86
		public override void ClearProfiles()
		{
			this.mProfileMap.Clear();
			this.mNextProfileId = 1U;
			this.mNextProfileUseSeq = 1U;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000C8A4 File Offset: 0x0000AAA4
		protected void Load()
		{
			SexyBuffer buffer = new SexyBuffer();
			string theFileName = "userdata/users.dat";
			if (!StorageFile.ReadBufferFromFile(theFileName, buffer))
			{
				return;
			}
			if (!this.ReadState(buffer))
			{
				this.ClearProfiles();
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000C8D8 File Offset: 0x0000AAD8
		protected void Save()
		{
			SexyBuffer buffer = new SexyBuffer();
			if (!this.WriteState(buffer))
			{
				return;
			}
			StorageFile.MakeDir("userdata");
			string theFileName = "userdata/users.dat";
			StorageFile.WriteBufferToFile(theFileName, buffer);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000C910 File Offset: 0x0000AB10
		protected void DeleteOldestProfile()
		{
			if (this.mProfileMap.Count == 0)
			{
				return;
			}
			string text = null;
			FilesystemProfileData filesystemProfileData = null;
			foreach (KeyValuePair<string, UserProfile> keyValuePair in this.mProfileMap)
			{
				if (text == null)
				{
					text = keyValuePair.Key;
					filesystemProfileData = (FilesystemProfileData)keyValuePair.Value.GetPlatformData();
				}
				else
				{
					FilesystemProfileData filesystemProfileData2 = (FilesystemProfileData)keyValuePair.Value.GetPlatformData();
					if (filesystemProfileData2.getUseSeq() < filesystemProfileData.getUseSeq())
					{
						text = keyValuePair.Key;
						filesystemProfileData = filesystemProfileData2;
					}
				}
			}
			this.mProfileMap[text].DeleteUserFiles();
			this.mProfileMap.Remove(text);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000C9D8 File Offset: 0x0000ABD8
		protected void DeleteOldProfiles()
		{
			while (this.mProfileMap.Count > 200)
			{
				this.DeleteOldestProfile();
			}
			this.Save();
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000C9FC File Offset: 0x0000ABFC
		protected bool ReadState(SexyBuffer data)
		{
			int num = (int)data.ReadLong();
			if ((long)num != (long)((ulong)GlobalMembers.gSexyAppBase.mProfileManager.GetProfileVersion()))
			{
				return false;
			}
			this.mProfileMap.Clear();
			uint num2 = 0U;
			uint num3 = 0U;
			int num4 = (int)data.ReadShort();
			for (int i = 0; i < num4; i++)
			{
				UserProfile userProfile = GlobalMembers.gSexyAppBase.mProfileManager.CreateUserProfile();
				FilesystemProfileData filesystemProfileData = (FilesystemProfileData)userProfile.GetPlatformData();
				if (!filesystemProfileData.ReadSummary(data))
				{
					return false;
				}
				if ((long)filesystemProfileData.getUseSeq() > (long)((ulong)num2))
				{
					num2 = (uint)filesystemProfileData.getUseSeq();
				}
				if ((long)userProfile.GetId() > (long)((ulong)num3))
				{
					num3 = (uint)userProfile.GetId();
				}
				this.mProfileMap.Add(userProfile.GetName(), userProfile);
			}
			this.mNextProfileId = num3 + 1U;
			this.mNextProfileUseSeq = num2 + 1U;
			return true;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000CAC8 File Offset: 0x0000ACC8
		protected bool WriteState(SexyBuffer data)
		{
			data.WriteLong((long)((ulong)GlobalMembers.gSexyAppBase.mProfileManager.GetProfileVersion()));
			data.WriteShort((short)this.mProfileMap.Count);
			foreach (UserProfile userProfile in this.mProfileMap.Values)
			{
				FilesystemProfileData filesystemProfileData = (FilesystemProfileData)userProfile.GetPlatformData();
				if (!filesystemProfileData.WriteSummary(data))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400021F RID: 543
		private Dictionary<string, UserProfile> mProfileMap = new Dictionary<string, UserProfile>();

		// Token: 0x04000220 RID: 544
		private uint mNextProfileId;

		// Token: 0x04000221 RID: 545
		private uint mNextProfileUseSeq;
	}
}
