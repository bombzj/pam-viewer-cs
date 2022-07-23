using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000D9 RID: 217
	public class PIValue2D : IDisposable
	{
		// Token: 0x060006A1 RID: 1697 RVA: 0x0001C390 File Offset: 0x0001A590
		public PIValue2D()
		{
			this.mLastTime = -1f;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0001C3DC File Offset: 0x0001A5DC
		public virtual void Dispose()
		{
			this.mBezier.Dispose();
			this.mValuePoint2DVector.Clear();
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001C3F4 File Offset: 0x0001A5F4
		public Vector2 GetValueAt(float theTime)
		{
			if (this.mLastTime == theTime)
			{
				return this.mLastPoint;
			}
			this.mLastTime = theTime;
			if (this.mValuePoint2DVector.Count == 1)
			{
				return this.mLastPoint = this.mValuePoint2DVector[0].mValue;
			}
			if (this.mBezier.IsInitialized())
			{
				return this.mLastPoint = this.mBezier.Evaluate(theTime);
			}
			for (int i = 1; i < this.mValuePoint2DVector.Count; i++)
			{
				PIValuePoint2D pivaluePoint2D = this.mValuePoint2DVector[i - 1];
				PIValuePoint2D pivaluePoint2D2 = this.mValuePoint2DVector[i];
				if ((theTime >= pivaluePoint2D.mTime && theTime <= pivaluePoint2D2.mTime) || i == this.mValuePoint2DVector.Count - 1)
				{
					return this.mLastPoint = pivaluePoint2D.mValue + (pivaluePoint2D2.mValue - pivaluePoint2D.mValue) * Math.Min(1f, (theTime - pivaluePoint2D.mTime) / (pivaluePoint2D2.mTime - pivaluePoint2D.mTime));
				}
			}
			return this.mLastPoint = new Vector2(0f, 0f);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001C528 File Offset: 0x0001A728
		public Vector2 GetVelocityAt(float theTime)
		{
			if (this.mLastVelocityTime == theTime)
			{
				return this.mLastVelocity;
			}
			this.mLastVelocityTime = theTime;
			if (this.mValuePoint2DVector.Count <= 1)
			{
				return new Vector2(0f, 0f);
			}
			if (this.mBezier.IsInitialized())
			{
				return this.mLastVelocity = this.mBezier.Velocity(theTime, false);
			}
			for (int i = 1; i < this.mValuePoint2DVector.Count; i++)
			{
				PIValuePoint2D pivaluePoint2D = this.mValuePoint2DVector[i - 1];
				PIValuePoint2D pivaluePoint2D2 = this.mValuePoint2DVector[i];
				if ((theTime >= pivaluePoint2D.mTime && theTime <= pivaluePoint2D2.mTime) || i == this.mValuePoint2DVector.Count - 1)
				{
					return this.mLastVelocity = pivaluePoint2D2.mValue - pivaluePoint2D.mValue;
				}
			}
			return this.mLastVelocity = new Vector2(0f, 0f);
		}

		// Token: 0x04000563 RID: 1379
		public List<PIValuePoint2D> mValuePoint2DVector = new List<PIValuePoint2D>();

		// Token: 0x04000564 RID: 1380
		public Bezier mBezier = new Bezier();

		// Token: 0x04000565 RID: 1381
		public float mLastTime;

		// Token: 0x04000566 RID: 1382
		public Vector2 mLastPoint = default(Vector2);

		// Token: 0x04000567 RID: 1383
		public float mLastVelocityTime;

		// Token: 0x04000568 RID: 1384
		public Vector2 mLastVelocity = default(Vector2);
	}
}
