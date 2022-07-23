using System;

namespace SexyFramework.Drivers
{
	// Token: 0x02000055 RID: 85
	public abstract class IHttpTransaction
	{
		// Token: 0x06000387 RID: 903
		public abstract void SetListener(IHttpListener listener);

		// Token: 0x06000388 RID: 904
		public abstract void SetUserData(IntPtr userData);

		// Token: 0x06000389 RID: 905
		public abstract void SetRequestHeader(string name, string value);

		// Token: 0x0600038A RID: 906
		public abstract void SetRequestBody(IntPtr data, uint length);

		// Token: 0x0600038B RID: 907
		public abstract void SetTimeout(int seconds);

		// Token: 0x0600038C RID: 908
		public abstract void Start();

		// Token: 0x0600038D RID: 909
		public abstract void Release();

		// Token: 0x0600038E RID: 910
		public abstract IntPtr GetUserData();

		// Token: 0x0600038F RID: 911
		public abstract int GetStatusCode();

		// Token: 0x06000390 RID: 912
		public abstract string GetStatusLine();

		// Token: 0x06000391 RID: 913
		public abstract int GetResponseLength();

		// Token: 0x06000392 RID: 914
		public abstract string GetResponseHeader(string key);

		// Token: 0x06000393 RID: 915
		public abstract string GetSerializedRequest();

		// Token: 0x06000394 RID: 916
		public abstract string GetErrorMessage();

		// Token: 0x06000395 RID: 917 RVA: 0x0000C313 File Offset: 0x0000A513
		public virtual void Dispose()
		{
		}
	}
}
