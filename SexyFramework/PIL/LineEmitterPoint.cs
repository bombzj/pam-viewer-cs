using System;
using System.Collections.Generic;
using SexyFramework.Misc;

namespace SexyFramework.PIL
{
	// Token: 0x0200015F RID: 351
	public class LineEmitterPoint
	{
		// Token: 0x06000C14 RID: 3092 RVA: 0x0003AA34 File Offset: 0x00038C34
		public virtual void Serialize(SexyBuffer b)
		{
			b.WriteFloat(this.mCurX);
			b.WriteFloat(this.mCurY);
			b.WriteLong((long)this.mKeyFramePoints.Count);
			for (int i = 0; i < this.mKeyFramePoints.Count; i++)
			{
				b.WriteLong((long)this.mKeyFramePoints[i].first);
				b.WriteLong((long)this.mKeyFramePoints[i].second.mX);
				b.WriteLong((long)this.mKeyFramePoints[i].second.mY);
			}
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0003AAD4 File Offset: 0x00038CD4
		public virtual void Deserialize(SexyBuffer b)
		{
			this.mCurX = b.ReadFloat();
			this.mCurY = b.ReadFloat();
			int num = (int)b.ReadLong();
			this.mKeyFramePoints.Clear();
			for (int i = 0; i < num; i++)
			{
				int f = (int)b.ReadLong();
				int theX = (int)b.ReadLong();
				int theY = (int)b.ReadLong();
				this.mKeyFramePoints.Add(new PointKeyFrame(f, new SexyPoint(theX, theY)));
			}
		}

		// Token: 0x0400099F RID: 2463
		public List<PointKeyFrame> mKeyFramePoints = new List<PointKeyFrame>();

		// Token: 0x040009A0 RID: 2464
		public float mCurX;

		// Token: 0x040009A1 RID: 2465
		public float mCurY;
	}
}
