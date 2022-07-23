using System;

namespace SexyFramework.Drivers
{
	// Token: 0x02000056 RID: 86
	public abstract class IHttpDriver
	{
		// Token: 0x06000397 RID: 919 RVA: 0x0000C31D File Offset: 0x0000A51D
		public virtual void Dispose()
		{
		}

		// Token: 0x06000398 RID: 920
		public abstract void Update();

		// Token: 0x06000399 RID: 921
		public abstract IHttpTransaction CreateHttpTransaction(string method, string url);

		// Token: 0x0600039A RID: 922
		public abstract IHttpDriver.NetworkStatus GetNetworkStatus();

		// Token: 0x0600039B RID: 923
		public abstract void AddNetworkStatusListener(INetworkStatusListener listener);

		// Token: 0x0600039C RID: 924
		public abstract void RemoveNetworkStatusListener(INetworkStatusListener listener);

		// Token: 0x0600039D RID: 925
		public abstract string GetPrimaryMACAddress();

		// Token: 0x02000057 RID: 87
		public enum NetworkStatus
		{
			// Token: 0x04000206 RID: 518
			NET_NOT_REACHABLE,
			// Token: 0x04000207 RID: 519
			NET_REACHABLE_WWAN,
			// Token: 0x04000208 RID: 520
			NET_REACHABLE_WIFI,
			// Token: 0x04000209 RID: 521
			NET_REACHABILITY_UNKNOWN
		}
	}
}
