using System;

namespace SexyFramework.Drivers
{
	// Token: 0x02000058 RID: 88
	public abstract class INetworkStatusListener
	{
		// Token: 0x0600039F RID: 927 RVA: 0x0000C327 File Offset: 0x0000A527
		public virtual void Dispose()
		{
		}

		// Token: 0x060003A0 RID: 928
		public abstract void NetworkStatusChanged(IHttpDriver.NetworkStatus status);
	}
}
