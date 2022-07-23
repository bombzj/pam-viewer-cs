using System;

namespace SexyFramework.Misc
{
	// Token: 0x02000139 RID: 313
	public class PerfTimer
	{
		// Token: 0x06000A44 RID: 2628 RVA: 0x00034FF0 File Offset: 0x000331F0
		protected void CalcDuration()
		{
			this.mDuration = (ulong)Common.SexyTime() - this.mStart;
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00035007 File Offset: 0x00033207
		public PerfTimer()
		{
			this.mRunning = false;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00035016 File Offset: 0x00033216
		public void Start()
		{
			this.mStart = (ulong)Common.SexyTime();
			this.mRunning = true;
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0003502B File Offset: 0x0003322B
		public void Stop()
		{
			this.CalcDuration();
			this.mRunning = false;
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0003503A File Offset: 0x0003323A
		public double GetDuration()
		{
			if (this.mRunning)
			{
				this.CalcDuration();
			}
			return this.mDuration;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00035050 File Offset: 0x00033250
		public bool IsRunning()
		{
			return this.mRunning;
		}

		// Token: 0x0400091C RID: 2332
		protected ulong mStart;

		// Token: 0x0400091D RID: 2333
		protected double mDuration;

		// Token: 0x0400091E RID: 2334
		protected bool mRunning;
	}
}
