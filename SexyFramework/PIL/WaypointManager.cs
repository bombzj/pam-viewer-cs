using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.PIL
{
	// Token: 0x02000182 RID: 386
	public class WaypointManager : IDisposable
	{
		// Token: 0x06000D98 RID: 3480 RVA: 0x00044220 File Offset: 0x00042420
		protected void Clean()
		{
			if (this.mCurve != null)
			{
				this.mCurve.Dispose();
			}
			this.mWaypoints.Clear();
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00044240 File Offset: 0x00042440
		public WaypointManager()
		{
			this.mTotalTime = 0f;
			this.mLoop = false;
			this.mTotalFrames = 0;
			this.mLastFrameWasEnd = false;
			this.mCurve = new Bezier();
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00044295 File Offset: 0x00042495
		public WaypointManager(WaypointManager rhs)
		{
			this.CopyFrom(rhs);
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x000442BB File Offset: 0x000424BB
		public virtual void Dispose()
		{
			this.Clean();
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x000442C4 File Offset: 0x000424C4
		public void CopyFrom(WaypointManager rhs)
		{
			if (this == rhs || rhs == null)
			{
				return;
			}
			this.Clean();
			this.mTotalTime = rhs.mTotalTime;
			this.mTotalFrames = rhs.mTotalFrames;
			this.mLoop = rhs.mLoop;
			this.mLastPoint = rhs.mLastPoint;
			this.mCurve = new Bezier(rhs.mCurve);
			this.mLastFrameWasEnd = rhs.mLastFrameWasEnd;
			for (int i = 0; i < rhs.mWaypoints.size<Waypoint>(); i++)
			{
				Waypoint waypoint = new Waypoint();
				waypoint.CopyFrom(rhs.mWaypoints[i]);
				this.mWaypoints.Add(waypoint);
			}
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00044368 File Offset: 0x00042568
		public void Serialize(SexyBuffer b)
		{
			this.mCurve.Serialize(b);
			b.WriteFloat(this.mTotalTime);
			b.WriteLong((long)this.mTotalFrames);
			b.WriteBoolean(this.mLastFrameWasEnd);
			b.WriteFloat(this.mLastPoint.X);
			b.WriteFloat(this.mLastPoint.Y);
			b.WriteLong((long)this.mWaypoints.Count);
			for (int i = 0; i < this.mWaypoints.Count; i++)
			{
				this.mWaypoints[i].Serialize(b);
			}
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00044404 File Offset: 0x00042604
		public void Deserialize(SexyBuffer b)
		{
			this.mCurve.Deserialize(b);
			this.mTotalTime = b.ReadFloat();
			this.mTotalFrames = (int)b.ReadLong();
			this.mLastFrameWasEnd = b.ReadBoolean();
			this.mLastPoint.X = b.ReadFloat();
			this.mLastPoint.Y = b.ReadFloat();
			this.mWaypoints.Clear();
			int num = (int)b.ReadLong();
			for (int i = 0; i < num; i++)
			{
				Waypoint waypoint = new Waypoint();
				waypoint.Deserialize(b);
				this.mWaypoints.Add(waypoint);
			}
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0004449C File Offset: 0x0004269C
		public void AddPoint(int frame, Vector2 p, bool linear, Vector2 c1)
		{
			Waypoint waypoint = new Waypoint();
			this.mWaypoints.Add(waypoint);
			waypoint.mTime = (float)frame / 100f;
			waypoint.mFrame = frame;
			float num = waypoint.mTime;
			int num2 = frame;
			if (this.mWaypoints.size<Waypoint>() > 1)
			{
				num -= this.mWaypoints[this.mWaypoints.size<Waypoint>() - 2].mTime;
				num2 -= this.mWaypoints[this.mWaypoints.size<Waypoint>() - 2].mFrame;
			}
			this.mTotalTime += num;
			this.mTotalFrames += num2;
			waypoint.mLinear = linear;
			waypoint.mPoint = p;
			waypoint.mControl1 = c1;
			Vector2 vector = c1 - p;
			waypoint.mControl2 = p - vector;
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0004456F File Offset: 0x0004276F
		public void AddPoint(int frame, Vector2 p, bool linear)
		{
			this.AddPoint(frame, p, linear, Vector2.Zero);
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0004457F File Offset: 0x0004277F
		public void AddPoint(int frame, Vector2 p)
		{
			this.AddPoint(frame, p, false);
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0004458C File Offset: 0x0004278C
		public void Init(bool make_curve_image)
		{
			Vector2[] array = new Vector2[this.mWaypoints.size<Waypoint>()];
			Vector2[] array2 = new Vector2[2 * (this.mWaypoints.size<Waypoint>() - 1)];
			float[] array3 = new float[this.mWaypoints.size<Waypoint>()];
			int num = 0;
			for (int i = 0; i < this.mWaypoints.size<Waypoint>(); i++)
			{
				Waypoint waypoint = this.mWaypoints[i];
				array3[i] = waypoint.mTime;
				array[i] = waypoint.mPoint;
				if (!waypoint.mLinear)
				{
					array2[num] = waypoint.mControl1;
					if (i > 0 && i < this.mWaypoints.size<Waypoint>() - 1)
					{
						array2[num + 1] = waypoint.mControl2;
					}
				}
				else if (i == 0 && this.mWaypoints.size<Waypoint>() == 1)
				{
					array2[0] = waypoint.mPoint;
				}
				else
				{
					Vector2 vector = default(Vector2);
					if (i < this.mWaypoints.size<Waypoint>() - 1)
					{
						vector = this.mWaypoints[i + 1].mPoint - this.mWaypoints[i].mPoint;
						array2[(i == 0) ? 0 : (num + 1)] = this.mWaypoints[i].mPoint + vector / 2f;
					}
					if (i > 0)
					{
						vector = this.mWaypoints[i].mPoint - this.mWaypoints[i - 1].mPoint;
						array2[num] = this.mWaypoints[i - 1].mPoint + vector / 2f;
					}
				}
				num++;
				if (i > 0)
				{
					num++;
				}
			}
			this.mCurve.Init(array, array2, array3, this.mWaypoints.size<Waypoint>());
			if (make_curve_image)
			{
				this.mCurve.GenerateCurveImage(SexyColor.White, 10000);
			}
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x000447BC File Offset: 0x000429BC
		public void Update(int frame)
		{
			if (this.mCurve.GetNumPoints() == 0)
			{
				return;
			}
			if (frame > 0 && frame % this.mTotalFrames == 0)
			{
				this.mLastFrameWasEnd = true;
			}
			if (this.mLoop)
			{
				frame %= this.mTotalFrames;
			}
			else if (frame > this.mTotalFrames)
			{
				return;
			}
			this.mLastPoint = this.mCurve.Evaluate((float)frame / (float)this.mTotalFrames * this.mTotalTime);
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0004482C File Offset: 0x00042A2C
		public void DebugDraw(Graphics g, float scale)
		{
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0004482E File Offset: 0x00042A2E
		public void DebugDraw(Graphics g)
		{
			this.DebugDraw(g, 1f);
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0004483C File Offset: 0x00042A3C
		public Vector2 GetLastPoint()
		{
			return this.mLastPoint;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00044844 File Offset: 0x00042A44
		public int GetNumPoints()
		{
			return this.mCurve.GetNumPoints();
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00044851 File Offset: 0x00042A51
		public bool AtEnd()
		{
			return this.mLastFrameWasEnd;
		}

		// Token: 0x04000B15 RID: 2837
		protected Bezier mCurve;

		// Token: 0x04000B16 RID: 2838
		protected List<Waypoint> mWaypoints = new List<Waypoint>();

		// Token: 0x04000B17 RID: 2839
		protected float mTotalTime;

		// Token: 0x04000B18 RID: 2840
		protected int mTotalFrames;

		// Token: 0x04000B19 RID: 2841
		protected Vector2 mLastPoint = default(Vector2);

		// Token: 0x04000B1A RID: 2842
		protected bool mLastFrameWasEnd;

		// Token: 0x04000B1B RID: 2843
		public bool mLoop;
	}
}
