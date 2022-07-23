using System;

namespace SexyFramework.AELib
{
	// Token: 0x02000009 RID: 9
	public class CompLayer
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002A1A File Offset: 0x00000C1A
		public CompLayer()
		{
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002A22 File Offset: 0x00000C22
		public CompLayer(Layer l, int start_frame, int dur, int offs)
		{
			this.mSource = l;
			this.mStartFrameOnComp = start_frame;
			this.mDuration = dur;
			this.mLayerOffsetStart = offs;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002A47 File Offset: 0x00000C47
		public CompLayer(CompLayer other)
		{
			this.CopyFrom(other);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002A56 File Offset: 0x00000C56
		public void CopyFrom(CompLayer other)
		{
			this.mSource = other.mSource.Duplicate();
			this.mStartFrameOnComp = other.mStartFrameOnComp;
			this.mDuration = other.mDuration;
			this.mLayerOffsetStart = other.mLayerOffsetStart;
		}

		// Token: 0x04000015 RID: 21
		public Layer mSource;

		// Token: 0x04000016 RID: 22
		public int mStartFrameOnComp;

		// Token: 0x04000017 RID: 23
		public int mDuration;

		// Token: 0x04000018 RID: 24
		public int mLayerOffsetStart;
	}
}
