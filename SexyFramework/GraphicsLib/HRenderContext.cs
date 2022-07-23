using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000AC RID: 172
	public class HRenderContext
	{
		// Token: 0x0600050B RID: 1291 RVA: 0x0000EAF5 File Offset: 0x0000CCF5
		public HRenderContext()
		{
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0000EAFD File Offset: 0x0000CCFD
		public HRenderContext(object inHandlePtr)
		{
			this.mHandlePtr = inHandlePtr;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0000EB0C File Offset: 0x0000CD0C
		public bool IsValid()
		{
			return this.mHandlePtr != null;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0000EB1A File Offset: 0x0000CD1A
		public object GetPointer()
		{
			return this.mHandlePtr;
		}

		// Token: 0x04000460 RID: 1120
		public object mHandlePtr;
	}
}
