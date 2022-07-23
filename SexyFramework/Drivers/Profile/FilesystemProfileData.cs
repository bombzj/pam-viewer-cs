using System;
using SexyFramework.File;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.Drivers.Profile
{
	// Token: 0x02000067 RID: 103
	public class FilesystemProfileData : IProfileData
	{
		// Token: 0x060003F8 RID: 1016 RVA: 0x0000C3EC File Offset: 0x0000A5EC
		public FilesystemProfileData(UserProfile player)
		{
			this.mPlayer = player;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000C414 File Offset: 0x0000A614
		public override int GetId()
		{
			return this.mId;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000C41C File Offset: 0x0000A61C
		public override string GetName()
		{
			return this.mName;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000C424 File Offset: 0x0000A624
		public override uint GetGamepadIndex()
		{
			return this.mGamepad;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000C42C File Offset: 0x0000A62C
		public override void SetGamepadIndex(uint gamepad)
		{
			this.mGamepad = gamepad;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000C435 File Offset: 0x0000A635
		public override bool SignedIn()
		{
			return true;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000C438 File Offset: 0x0000A638
		public override bool IsSigningIn()
		{
			return false;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000C43B File Offset: 0x0000A63B
		public override bool IsOnline()
		{
			return false;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000C440 File Offset: 0x0000A640
		public override void DeleteUserFiles()
		{
			string theFileName = string.Format("userdata/user{0}.dat", this.GetId());
			StorageFile.DeleteFile(theFileName);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000C46C File Offset: 0x0000A66C
		public override EProfileIOState LoadDetails()
		{
			SexyBuffer buffer = new SexyBuffer();
			string theFileName = string.Format("userdata/user{0}.dat", this.GetId());
			if (!StorageFile.ReadBufferFromFile(theFileName, buffer))
			{
				if (!StorageFile.FileExists(theFileName))
				{
					this.mPlayer.Reset();
					this.mLoaded = true;
					return EProfileIOState.PROFILE_IO_SUCCESS;
				}
				this.mLoaded = false;
				return EProfileIOState.PROFILE_IO_ERROR;
			}
			else
			{
				if (!this.mPlayer.ReadProfileSettings(buffer))
				{
					this.mLoaded = false;
					return EProfileIOState.PROFILE_IO_ERROR;
				}
				this.mLoaded = true;
				return EProfileIOState.PROFILE_IO_SUCCESS;
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000C4E2 File Offset: 0x0000A6E2
		public override bool IsLoading()
		{
			return false;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000C4E5 File Offset: 0x0000A6E5
		public override bool IsLoaded()
		{
			return this.mLoaded;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000C4F0 File Offset: 0x0000A6F0
		public override EProfileIOState SaveDetails()
		{
			SexyBuffer buffer = new SexyBuffer();
			if (!this.mPlayer.WriteProfileSettings(buffer))
			{
				this.mSaved = false;
				return EProfileIOState.PROFILE_IO_ERROR;
			}
			StorageFile.MakeDir("userdata");
			string theFileName = string.Format("userdata/user{0}.dat", this.GetId());
			if (!StorageFile.WriteBufferToFile(theFileName, buffer))
			{
				this.mSaved = false;
				return EProfileIOState.PROFILE_IO_ERROR;
			}
			this.mSaved = true;
			return EProfileIOState.PROFILE_IO_SUCCESS;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000C554 File Offset: 0x0000A754
		public override bool IsSaving()
		{
			return false;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000C557 File Offset: 0x0000A757
		public override bool IsSaved()
		{
			return this.mSaved;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000C55F File Offset: 0x0000A75F
		public override bool HasError()
		{
			return false;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000C562 File Offset: 0x0000A762
		public bool ReadSummary(SexyBuffer data)
		{
			this.mName = data.ReadString();
			this.mId = (int)data.ReadLong();
			this.mUseSeq = (int)data.ReadLong();
			return true;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000C58B File Offset: 0x0000A78B
		public bool WriteSummary(SexyBuffer data)
		{
			data.WriteString(this.mName);
			data.WriteLong((long)this.mId);
			data.WriteLong((long)this.mUseSeq);
			return true;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000C5B4 File Offset: 0x0000A7B4
		public override Image GetPlayerIcon()
		{
			return null;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000C5B7 File Offset: 0x0000A7B7
		public override bool IsAchievementUnlocked(uint id)
		{
			return false;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000C5BA File Offset: 0x0000A7BA
		public override IAchievementContext StartUnlockAchievement(uint id)
		{
			return null;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000C5BD File Offset: 0x0000A7BD
		public void SetId(int id)
		{
			this.mId = id;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000C5C6 File Offset: 0x0000A7C6
		public void SetUseSeq(int useSeq)
		{
			this.mUseSeq = useSeq;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000C5CF File Offset: 0x0000A7CF
		public void SetName(string name)
		{
			this.mName = name;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000C5D8 File Offset: 0x0000A7D8
		public int getId()
		{
			return this.mId;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000C5E0 File Offset: 0x0000A7E0
		public int getUseSeq()
		{
			return this.mUseSeq;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000C5E8 File Offset: 0x0000A7E8
		public string getName()
		{
			return this.mName;
		}

		// Token: 0x04000218 RID: 536
		private UserProfile mPlayer;

		// Token: 0x04000219 RID: 537
		private int mId = -1;

		// Token: 0x0400021A RID: 538
		private int mUseSeq = -1;

		// Token: 0x0400021B RID: 539
		private string mName = "";

		// Token: 0x0400021C RID: 540
		private uint mGamepad;

		// Token: 0x0400021D RID: 541
		private bool mLoaded;

		// Token: 0x0400021E RID: 542
		private bool mSaved;
	}
}
