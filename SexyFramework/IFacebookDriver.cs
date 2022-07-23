using System;

namespace SexyFramework
{
	// Token: 0x0200004C RID: 76
	public abstract class IFacebookDriver
	{
		// Token: 0x0600036C RID: 876 RVA: 0x0000C2D6 File Offset: 0x0000A4D6
		public virtual void Dispose()
		{
		}

		// Token: 0x0600036D RID: 877
		public abstract void InitWithAppId(string app_id);

		// Token: 0x0600036E RID: 878
		public abstract void SetUserDataFields(string userDataFields);

		// Token: 0x0600036F RID: 879
		public abstract void Resume(IFBSessionListener listener);

		// Token: 0x06000370 RID: 880
		public abstract void Authorize(string permissions, IFBSessionListener listener);

		// Token: 0x06000371 RID: 881
		public abstract void Logout(IFBSessionListener listener);

		// Token: 0x06000372 RID: 882
		public abstract void Update();

		// Token: 0x06000373 RID: 883
		public abstract int IsAuthorized();

		// Token: 0x06000374 RID: 884
		public abstract int IsAuthorizing();

		// Token: 0x06000375 RID: 885
		public abstract string GetUserId();

		// Token: 0x06000376 RID: 886
		public abstract StructuredData GetUserData();

		// Token: 0x06000377 RID: 887
		public abstract string GetAccessToken();

		// Token: 0x06000378 RID: 888
		public abstract int GetExpirationDate();

		// Token: 0x06000379 RID: 889
		public abstract NetworkServiceProfile ServiceProfile();

		// Token: 0x0600037A RID: 890
		public abstract void Dialog(string name, IFBDialogListener listener);

		// Token: 0x0600037B RID: 891
		public abstract void Dialog(string name, StructuredData @params, IFBDialogListener listener);
	}
}
