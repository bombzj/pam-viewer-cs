using System;
using SexyFramework.Misc;

namespace SexyFramework.AELib
{
	// Token: 0x02000010 RID: 16
	public class CumulativeTransform : IDisposable
	{
		// Token: 0x0600006D RID: 109 RVA: 0x00003D94 File Offset: 0x00001F94
		public CumulativeTransform()
		{
			this.mTrans = new SexyTransform2D(true);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003DB3 File Offset: 0x00001FB3
		public CumulativeTransform(CumulativeTransform rhs)
		{
			this.CopyFrom(rhs);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003DCD File Offset: 0x00001FCD
		public virtual void Dispose()
		{
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003DCF File Offset: 0x00001FCF
		public void CopyFrom(CumulativeTransform other)
		{
			this.mOpacity = other.mOpacity;
			this.mForceAdditive = other.mForceAdditive;
			this.mTrans = other.mTrans;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003DF5 File Offset: 0x00001FF5
		public void Reset()
		{
			this.mOpacity = 1f;
			this.mForceAdditive = false;
			this.mTrans.LoadIdentity();
		}

		// Token: 0x0400003A RID: 58
		public float mOpacity = 1f;

		// Token: 0x0400003B RID: 59
		public bool mForceAdditive;

		// Token: 0x0400003C RID: 60
		public SexyTransform2D mTrans;
	}
}
