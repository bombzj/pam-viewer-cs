using System;

namespace SexyFramework
{
	// Token: 0x0200004A RID: 74
	public class IFBSessionListener
	{
		// Token: 0x06000363 RID: 867 RVA: 0x0000C2B7 File Offset: 0x0000A4B7
		public virtual void FacebookDidLogin(IFacebookDriver facebook)
		{
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000C2B9 File Offset: 0x0000A4B9
		public virtual void FacebookDidNotLogin(IFacebookDriver facebook, int canceled)
		{
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000C2BB File Offset: 0x0000A4BB
		public virtual void FacebookDidLogout(IFacebookDriver facebook)
		{
		}
	}
}
