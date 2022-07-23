using System;

namespace ZumasRevenge
{
	// Token: 0x02000094 RID: 148
	public class PathPoint
	{
		// Token: 0x060009C2 RID: 2498 RVA: 0x0005A7E8 File Offset: 0x000589E8
		public PathPoint(float tx, float ty, float dist)
		{
			this.x = tx;
			this.y = ty;
			this.mDist = dist;
			this.t = 0f;
			this.mPriority = 0;
			this.mInTunnel = false;
			this.mEndPoint = false;
			this.mSplinePoint = false;
			this.mSelected = false;
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0005A840 File Offset: 0x00058A40
		public PathPoint(float tx, float ty)
		{
			this.x = tx;
			this.y = ty;
			this.mDist = 0f;
			this.t = 0f;
			this.mPriority = 0;
			this.mInTunnel = false;
			this.mEndPoint = false;
			this.mSplinePoint = false;
			this.mSelected = false;
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0005A89C File Offset: 0x00058A9C
		public PathPoint()
		{
			this.x = 0f;
			this.y = 0f;
			this.mDist = 0f;
			this.t = 0f;
			this.mPriority = 0;
			this.mInTunnel = false;
			this.mEndPoint = false;
			this.mSplinePoint = false;
			this.mSelected = false;
		}

		// Token: 0x040007A5 RID: 1957
		public float x;

		// Token: 0x040007A6 RID: 1958
		public float y;

		// Token: 0x040007A7 RID: 1959
		public float mDist;

		// Token: 0x040007A8 RID: 1960
		public float t;

		// Token: 0x040007A9 RID: 1961
		public byte mPriority;

		// Token: 0x040007AA RID: 1962
		public bool mInTunnel;

		// Token: 0x040007AB RID: 1963
		public bool mEndPoint;

		// Token: 0x040007AC RID: 1964
		public bool mSplinePoint;

		// Token: 0x040007AD RID: 1965
		public bool mSelected;
	}
}
