using System;

namespace SexyFramework
{
	// Token: 0x020001A6 RID: 422
	public class SexyApp : SexyAppBase
	{
		// Token: 0x06000F5F RID: 3935 RVA: 0x0004BDCF File Offset: 0x00049FCF
		public SexyApp()
		{
			GlobalMembers.gSexyApp = this;
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x0004BDDD File Offset: 0x00049FDD
		public override void Dispose()
		{
			GlobalMembers.gSexyApp = null;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0004BDE5 File Offset: 0x00049FE5
		public void WriteRegistrationInfo(string theRegUser, string theRegCode, int theTimesPlayed, int theTimesExecuted)
		{
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x0004BDE7 File Offset: 0x00049FE7
		public void ReadRegistrationInfo(ref string theUser, ref string theKey, ref int theTimesPlayed, ref int theTimesExecuted)
		{
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x0004BDE9 File Offset: 0x00049FE9
		public bool Validate(string theUserName, string theRegCode)
		{
			return true;
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0004BDEC File Offset: 0x00049FEC
		public override void UpdateFrames()
		{
			base.UpdateFrames();
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0004BDF4 File Offset: 0x00049FF4
		public virtual bool ShouldCheckForUpdate()
		{
			return false;
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0004BDF7 File Offset: 0x00049FF7
		public virtual void UpdateCheckQueried()
		{
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x0004BDF9 File Offset: 0x00049FF9
		public virtual bool OpenRegisterPage(DefinesMap theDefinesMap)
		{
			return false;
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x0004BDFC File Offset: 0x00049FFC
		public virtual bool OpenRegisterPage()
		{
			return false;
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x0004BDFF File Offset: 0x00049FFF
		public override void Init()
		{
			base.Init();
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0004BE07 File Offset: 0x0004A007
		public virtual bool OpenHTMLTemplate(string theTemplateFile, DefinesMap theDefinesMap)
		{
			return false;
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0004BE0A File Offset: 0x0004A00A
		public virtual void OpenUpdateURL()
		{
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0004BE0C File Offset: 0x0004A00C
		public virtual void HandleNotifyGameMessageCommandLine(string theCommandLine)
		{
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0004BE0E File Offset: 0x0004A00E
		public virtual void GetSEHWebParams(ref DefinesMap theDefinesMap)
		{
		}
	}
}
