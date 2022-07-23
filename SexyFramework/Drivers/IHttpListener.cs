using System;

namespace SexyFramework.Drivers
{
	// Token: 0x02000054 RID: 84
	public class IHttpListener
	{
		// Token: 0x06000381 RID: 897 RVA: 0x0000C301 File Offset: 0x0000A501
		public virtual void Dispose()
		{
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000C303 File Offset: 0x0000A503
		public virtual void HttpReceivedResponse(IHttpTransaction http)
		{
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000C305 File Offset: 0x0000A505
		public virtual void HttpReceivedData(IHttpTransaction http, IntPtr data, uint length)
		{
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000C307 File Offset: 0x0000A507
		public virtual void HttpTransactionComplete(IHttpTransaction http)
		{
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000C309 File Offset: 0x0000A509
		public virtual void HttpTransactionError(IHttpTransaction http)
		{
		}
	}
}
