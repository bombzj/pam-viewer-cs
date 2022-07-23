using System;
using Microsoft.Xna.Framework;
using SexyFramework.Misc;

namespace SexyFramework.PIL
{
	// Token: 0x02000181 RID: 385
	public class Waypoint
	{
		// Token: 0x06000D94 RID: 3476 RVA: 0x0004406C File Offset: 0x0004226C
		public void CopyFrom(Waypoint rhs)
		{
			this.mLinear = rhs.mLinear;
			this.mControl1 = rhs.mControl1;
			this.mControl2 = rhs.mControl2;
			this.mPoint = rhs.mPoint;
			this.mTime = rhs.mTime;
			this.mFrame = rhs.mFrame;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x000440C4 File Offset: 0x000422C4
		public void Serialize(SexyBuffer b)
		{
			b.WriteBoolean(this.mLinear);
			b.WriteFloat(this.mTime);
			b.WriteLong((long)this.mFrame);
			b.WriteFloat(this.mControl1.X);
			b.WriteFloat(this.mControl1.Y);
			b.WriteFloat(this.mControl2.X);
			b.WriteFloat(this.mControl2.Y);
			b.WriteFloat(this.mPoint.X);
			b.WriteFloat(this.mPoint.Y);
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0004415C File Offset: 0x0004235C
		public void Deserialize(SexyBuffer b)
		{
			this.mLinear = b.ReadBoolean();
			this.mTime = b.ReadFloat();
			this.mFrame = (int)b.ReadLong();
			this.mControl1.X = b.ReadFloat();
			this.mControl1.Y = b.ReadFloat();
			this.mControl2.X = b.ReadFloat();
			this.mControl2.Y = b.ReadFloat();
			this.mPoint.X = b.ReadFloat();
			this.mPoint.Y = b.ReadFloat();
		}

		// Token: 0x04000B0F RID: 2831
		public bool mLinear;

		// Token: 0x04000B10 RID: 2832
		public Vector2 mControl1 = default(Vector2);

		// Token: 0x04000B11 RID: 2833
		public Vector2 mControl2 = default(Vector2);

		// Token: 0x04000B12 RID: 2834
		public Vector2 mPoint = default(Vector2);

		// Token: 0x04000B13 RID: 2835
		public float mTime;

		// Token: 0x04000B14 RID: 2836
		public int mFrame;
	}
}
