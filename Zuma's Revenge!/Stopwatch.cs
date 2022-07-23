using System;

namespace ZumasRevenge
{
	// Token: 0x020000C8 RID: 200
	public class Stopwatch
	{
		// Token: 0x06000ADE RID: 2782 RVA: 0x0006AFD4 File Offset: 0x000691D4
		public Stopwatch(string msg)
		{
			this.text = msg;
			this.start = DateTime.Now.Millisecond;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0006B004 File Offset: 0x00069204
		~Stopwatch()
		{
		}

		// Token: 0x04000982 RID: 2434
		private string text;

		// Token: 0x04000983 RID: 2435
		private int start;
	}
}
