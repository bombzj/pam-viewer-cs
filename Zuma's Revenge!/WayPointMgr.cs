using System;
using System.Collections.Generic;
using System.Linq;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x0200014F RID: 335
	public class WayPointMgr
	{
		// Token: 0x06001052 RID: 4178 RVA: 0x000A6754 File Offset: 0x000A4954
		protected void DrawCurvePiece(Graphics g, int theWayPoint, float theThickness)
		{
			if (theWayPoint == 0)
			{
				return;
			}
			WayPoint wayPoint = this.mWayPoints[theWayPoint - 1];
			WayPoint wayPoint2 = this.mWayPoints[theWayPoint];
			if (Math.Abs(wayPoint.x - wayPoint2.x) > 5f || Math.Abs(wayPoint.y - wayPoint2.y) > 5f)
			{
				return;
			}
			SexyVector3 v = this.CalcPerpendicular((float)(theWayPoint - 1)) * theThickness;
			SexyVector3 v2 = this.CalcPerpendicular((float)theWayPoint) * theThickness;
			SexyVector3 sexyVector = new SexyVector3(Common._S(wayPoint.x), Common._S(wayPoint.y), 0f) - v;
			SexyVector3 sexyVector2 = new SexyVector3(Common._S(wayPoint.x), Common._S(wayPoint.y), 0f) + v;
			SexyVector3 sexyVector3 = new SexyVector3(Common._S(wayPoint2.x), Common._S(wayPoint2.y), 0f) + v2;
			SexyVector3 sexyVector4 = new SexyVector3(Common._S(wayPoint2.x), Common._S(wayPoint2.y), 0f) - v2;
			SexyPoint[] theVertexList = new SexyPoint[]
			{
				new SexyPoint((int)sexyVector.x, (int)sexyVector.y),
				new SexyPoint((int)sexyVector2.x, (int)sexyVector2.y),
				new SexyPoint((int)sexyVector3.x, (int)sexyVector3.y),
				new SexyPoint((int)sexyVector4.x, (int)sexyVector4.y)
			};
			g.PolyFill(theVertexList, 4, false);
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x000A68EE File Offset: 0x000A4AEE
		public WayPointMgr()
		{
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x000A6901 File Offset: 0x000A4B01
		public WayPointMgr(WayPointMgr rhs)
		{
			if (rhs == null)
			{
				return;
			}
			this.mWayPoints.AddRange(rhs.mWayPoints.ToArray());
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x000A692E File Offset: 0x000A4B2E
		public virtual void Dispose()
		{
			this.mWayPoints.Clear();
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x000A693C File Offset: 0x000A4B3C
		public void SetWayPoint(Ball theBall, float thePoint, bool loop_at_end)
		{
			if (this.mWayPoints.Count == 0)
			{
				return;
			}
			int num = (int)thePoint;
			int num2;
			if (num < 0)
			{
				num = 0;
				num2 = 1;
			}
			else if (num >= this.mWayPoints.Count)
			{
				if (!loop_at_end)
				{
					num = this.mWayPoints.Count - 1;
					num2 = num + 1;
				}
				else
				{
					num = (int)thePoint % this.mWayPoints.Count;
					num2 = ((int)thePoint + 1) % this.mWayPoints.Count;
				}
			}
			else
			{
				num2 = num + 1;
			}
			WayPoint wayPoint = this.mWayPoints[num];
			WayPoint wayPoint2 = wayPoint;
			if (num2 < this.mWayPoints.Count)
			{
				wayPoint2 = this.mWayPoints[num2];
			}
			float x = theBall.GetX();
			float y = theBall.GetY();
			if (Math.Abs(wayPoint2.x - wayPoint.x) > 5f || Math.Abs(wayPoint2.y - wayPoint.y) > 5f)
			{
				theBall.SetPos(wayPoint.x, wayPoint.y);
			}
			else
			{
				float num3 = thePoint - (float)((int)thePoint);
				theBall.SetPos(num3 * (wayPoint2.x - wayPoint.x) + wayPoint.x, num3 * (wayPoint2.y - wayPoint.y) + wayPoint.y);
			}
			bool immediate = Math.Abs(theBall.GetX() - x) + Math.Abs(theBall.GetY() - y) > 10f;
			this.CalcAvgRotationForPoint(num);
			theBall.SetRotation(wayPoint.mAvgRotation, immediate);
			theBall.SetWayPoint(thePoint, wayPoint.mInTunnel);
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x000A6AB4 File Offset: 0x000A4CB4
		public void SetWayPointInt(Ball theBall, int thePoint, bool loop_at_end)
		{
			if (this.mWayPoints.Count == 0)
			{
				return;
			}
			int num = thePoint;
			if (num < 0)
			{
				num = 0;
			}
			else if (num >= this.mWayPoints.Count)
			{
				if (loop_at_end)
				{
					num = thePoint % this.mWayPoints.Count;
				}
				else
				{
					num = this.mWayPoints.Count - 1;
				}
			}
			WayPoint wayPoint = this.mWayPoints[num];
			this.CalcAvgRotationForPoint(num);
			theBall.SetPos(wayPoint.x, wayPoint.y);
			theBall.SetWayPoint((float)thePoint, wayPoint.mInTunnel);
			theBall.SetRotation(wayPoint.mAvgRotation, false);
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x000A6B4C File Offset: 0x000A4D4C
		public void FindFreeWayPoint(Ball theExistingBall, Ball theNewBall, bool inFront, bool loop_at_end, int thePad)
		{
			int num = (inFront ? 1 : (-1));
			int num2 = (int)theExistingBall.GetWayPoint();
			if (inFront && theNewBall.GetWayPoint() > (float)num2)
			{
				num2 = (int)theNewBall.GetWayPoint();
			}
			else if (!inFront && theNewBall.GetWayPoint() < (float)num2)
			{
				num2 = (int)theNewBall.GetWayPoint();
			}
			while (num2 >= 0 && (loop_at_end || num2 < this.mWayPoints.Count))
			{
				WayPoint wayPoint = this.mWayPoints[num2 % this.mWayPoints.Count];
				theNewBall.SetPos(wayPoint.x, wayPoint.y);
				if (!theExistingBall.CollidesWithPhysically(theNewBall, thePad))
				{
					break;
				}
				int num3 = num2 % this.mWayPoints.Count;
				num2 += num;
				if (loop_at_end && num3 + num < 0)
				{
					num2 -= num;
					break;
				}
			}
			this.SetWayPointInt(theNewBall, num2, loop_at_end);
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x000A6C11 File Offset: 0x000A4E11
		public void FindFreeWayPoint(Ball theExistingBall, Ball theNewBall, bool inFront, bool loop_at_end)
		{
			this.FindFreeWayPoint(theExistingBall, theNewBall, inFront, loop_at_end, 0);
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x000A6C20 File Offset: 0x000A4E20
		public SexyVector3 CalcPerpendicular(float theWayPoint)
		{
			int num = (int)theWayPoint;
			if (num < 0)
			{
				num = 0;
			}
			if (num >= this.mWayPoints.Count)
			{
				num = this.mWayPoints.Count - 1;
			}
			WayPoint wayPoint = this.mWayPoints[num];
			this.CalcPerpendicularForPoint(num);
			return wayPoint.mPerpendicular;
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x000A6C6C File Offset: 0x000A4E6C
		public SexyVector2 GetPointPos(float thePoint)
		{
			int num = (int)thePoint;
			if (num < 0)
			{
				num = 0;
			}
			else if (num >= this.mWayPoints.Count)
			{
				num = this.mWayPoints.Count - 1;
			}
			WayPoint wayPoint = this.mWayPoints[num];
			return new SexyVector2(wayPoint.x, wayPoint.y);
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x000A6CC0 File Offset: 0x000A4EC0
		public float GetRotationForPoint(int theWayPoint)
		{
			if (theWayPoint < 0)
			{
				theWayPoint = 0;
			}
			if (theWayPoint >= Enumerable.Count<WayPoint>(this.mWayPoints) - 1)
			{
				theWayPoint = Enumerable.Count<WayPoint>(this.mWayPoints) - 1;
			}
			WayPoint wayPoint = this.mWayPoints[theWayPoint];
			this.CalcPerpendicularForPoint(theWayPoint);
			return wayPoint.mRotation;
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x000A6D10 File Offset: 0x000A4F10
		public void CalcPerpendicularForPoint(int theWayPoint)
		{
			WayPoint wayPoint = this.mWayPoints[theWayPoint];
			if (wayPoint.mHavePerpendicular)
			{
				return;
			}
			bool flag = false;
			WayPoint wayPoint2;
			if (theWayPoint + 1 < Enumerable.Count<WayPoint>(this.mWayPoints))
			{
				wayPoint2 = this.mWayPoints[theWayPoint + 1];
				if ((Math.Abs(wayPoint.x - wayPoint2.x) > 5f || Math.Abs(wayPoint.y - wayPoint2.y) > 5f) && theWayPoint > 0)
				{
					flag = true;
					wayPoint2 = this.mWayPoints[theWayPoint - 1];
				}
			}
			else
			{
				wayPoint2 = this.mWayPoints[theWayPoint - 1];
				if ((Math.Abs(wayPoint.x - wayPoint2.x) > 5f || Math.Abs(wayPoint.y - wayPoint2.y) > 5f) && theWayPoint + 1 < Enumerable.Count<WayPoint>(this.mWayPoints))
				{
					wayPoint2 = this.mWayPoints[theWayPoint + 1];
				}
				else
				{
					flag = true;
				}
			}
			if (flag)
			{
				wayPoint.mPerpendicular = new SexyVector3(wayPoint.y - wayPoint2.y, wayPoint2.x - wayPoint.x, 0f);
			}
			else
			{
				wayPoint.mPerpendicular = new SexyVector3(wayPoint2.y - wayPoint.y, wayPoint.x - wayPoint2.x, 0f);
			}
			wayPoint.mPerpendicular = wayPoint.mPerpendicular.Normalize();
			wayPoint.mRotation = (float)Math.Acos((double)wayPoint.mPerpendicular.Dot(new SexyVector3(1f, 0f, 0f)));
			if (wayPoint.mPerpendicular.y > 0f)
			{
				wayPoint.mRotation *= -1f;
			}
			if (wayPoint.mRotation < 0f)
			{
				wayPoint.mRotation += 6.2831855f;
			}
			wayPoint.mHavePerpendicular = true;
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x000A6EE4 File Offset: 0x000A50E4
		public void CalcAvgRotationForPoint(int theWayPoint)
		{
			WayPoint wayPoint = this.mWayPoints[theWayPoint];
			if (wayPoint.mHaveAvgRotation)
			{
				return;
			}
			this.CalcPerpendicularForPoint(theWayPoint);
			wayPoint.mHaveAvgRotation = true;
			wayPoint.mAvgRotation = wayPoint.mRotation;
			int num = theWayPoint - 10;
			int num2 = theWayPoint + 10;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 >= Enumerable.Count<WayPoint>(this.mWayPoints))
			{
				num2 = Enumerable.Count<WayPoint>(this.mWayPoints) - 1;
			}
			this.CalcPerpendicularForPoint(num);
			for (int i = num + 1; i < num2; i++)
			{
				this.CalcPerpendicularForPoint(i);
				float canonicalAngle = WayPoint.GetCanonicalAngle(this.mWayPoints[i].mRotation - this.mWayPoints[i - 1].mRotation);
				if (canonicalAngle > 0.1f || canonicalAngle < -0.1f)
				{
					WayPoint wayPoint2 = this.mWayPoints[i];
					WayPoint wayPoint3 = this.mWayPoints[i - 1];
					if (Math.Abs(wayPoint2.x - wayPoint3.x) <= 5f && Math.Abs(wayPoint2.y - wayPoint3.y) <= 5f)
					{
						float num3 = 1f - (float)(i - num) / (float)(num2 - num);
						wayPoint.mAvgRotation = this.mWayPoints[num].mRotation + num3 * canonicalAngle;
						return;
					}
				}
			}
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x000A703C File Offset: 0x000A523C
		public int GetPriority(Ball theBall)
		{
			int priority = this.GetPriority((int)(theBall.GetWayPoint() - (float)theBall.GetRadius()));
			int priority2 = this.GetPriority((int)(theBall.GetWayPoint() + (float)theBall.GetRadius()));
			return Math.Max(priority, priority2);
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x000A707C File Offset: 0x000A527C
		public int GetPriority(int thePoint)
		{
			if (thePoint < 0 || thePoint >= Enumerable.Count<WayPoint>(this.mWayPoints))
			{
				return 0;
			}
			return (int)this.mWayPoints[thePoint].mPriority;
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x000A70A3 File Offset: 0x000A52A3
		public int GetPriority(Bullet theBullet)
		{
			if (theBullet.GetWayPoint() == 0f || theBullet.GetHitPercent() < 0.7f)
			{
				return 4;
			}
			return this.GetPriority(theBullet);
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x000A70C8 File Offset: 0x000A52C8
		public bool LoadCurve(string theFile, CurveDesc desc, MirrorType theMirror)
		{
			string text = theFile;
			if (-1 != text.LastIndexOf('.'))
			{
				text = text.Substring(0, text.LastIndexOf('.'));
			}
			this.mWayPoints.Clear();
			CurveData curveData = new CurveData();
			if (!curveData.Load(text))
			{
				Console.WriteLine("FAILED TO OPEN FILE %s\n", theFile);
				for (int i = 0; i < 400; i++)
				{
					this.mWayPoints.Add(new WayPoint((float)i, 100f));
				}
			}
			desc.GetValuesFrom(curveData);
			bool flag = false;
			List<PathPoint> mPointList = curveData.mPointList;
			bool flag2 = Enumerable.First<PathPoint>(mPointList).mInTunnel;
			int num = 15;
			foreach (PathPoint pathPoint in mPointList)
			{
				float x = pathPoint.x;
				float y = pathPoint.y;
				if (!flag && x >= 0f && y >= 0f && x <= 800f && y <= 600f)
				{
					flag = true;
				}
				WayPoint wayPoint = new WayPoint(pathPoint.x, pathPoint.y);
				this.mWayPoints.Add(wayPoint);
				wayPoint.mInTunnel = pathPoint.mInTunnel;
				if (flag2 && pathPoint.mInTunnel)
				{
					num = this.mWayPoints.Count;
				}
				else
				{
					flag2 = false;
				}
				int num2 = (int)pathPoint.mPriority;
				if (num2 < 0)
				{
					num2 = 0;
				}
				if (num2 >= 5)
				{
				}
				wayPoint.mPriority = pathPoint.mPriority;
			}
			desc.mCutoffPoint = num - Common.GetDefaultBallRadius();
			if (theMirror != MirrorType.MirrorType_None)
			{
				for (int j = 0; j < Enumerable.Count<WayPoint>(this.mWayPoints); j++)
				{
					WayPoint wayPoint2 = this.mWayPoints[j];
					Common.MirrorPoint(ref wayPoint2.x, ref wayPoint2.y, theMirror);
				}
			}
			return true;
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x000A72A4 File Offset: 0x000A54A4
		public void DrawCurve(Graphics g, SexyColor theColor, int theDangerPoint)
		{
			if (this.mWayPoints.Count == 0)
			{
				return;
			}
			float theThickness = 5f;
			for (int i = 1; i < this.mWayPoints.Count; i++)
			{
				g.SetColor(theColor);
				this.DrawCurvePiece(g, i, theThickness);
			}
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x000A72EC File Offset: 0x000A54EC
		public void DrawTunnel(Graphics g, int priority)
		{
			if (this.mWayPoints.Count == 0)
			{
				return;
			}
			for (int i = 1; i < this.mWayPoints.Count; i++)
			{
				WayPoint wayPoint = this.mWayPoints[i];
				if (wayPoint.mInTunnel && (int)wayPoint.mPriority == priority)
				{
					g.SetColor(0, 0, 0, 160);
					this.DrawCurvePiece(g, i, 25f);
				}
			}
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x000A7356 File Offset: 0x000A5556
		public bool InTunnel(int theWayPoint)
		{
			return theWayPoint < 0 || (theWayPoint < Enumerable.Count<WayPoint>(this.mWayPoints) && this.mWayPoints[theWayPoint].mInTunnel);
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x000A7380 File Offset: 0x000A5580
		public bool InTunnel(Ball theBall, bool inFront)
		{
			int num = (int)theBall.GetWayPoint();
			if (inFront)
			{
				num += theBall.GetRadius();
			}
			else
			{
				num -= theBall.GetRadius();
			}
			return this.InTunnel(num);
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x000A73B8 File Offset: 0x000A55B8
		public bool InTunnel(Bullet theBullet)
		{
			Ball hitBall = theBullet.GetHitBall();
			if (hitBall == null)
			{
				return false;
			}
			int num = (int)hitBall.GetWayPoint();
			if (theBullet.GetHitInFront())
			{
				num += 3 * hitBall.GetRadius();
			}
			else
			{
				num -= 3 * hitBall.GetRadius();
			}
			return this.InTunnel(num);
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x000A7400 File Offset: 0x000A5600
		public bool CheckDiscontinuity(int thePoint, int theDist)
		{
			int i = thePoint;
			int num = thePoint + theDist;
			if (i < 0)
			{
				i = 0;
			}
			if (i > Enumerable.Count<WayPoint>(this.mWayPoints))
			{
				i = Enumerable.Count<WayPoint>(this.mWayPoints);
			}
			if (num < 0)
			{
				num = 0;
			}
			if (num > Enumerable.Count<WayPoint>(this.mWayPoints))
			{
				num = Enumerable.Count<WayPoint>(this.mWayPoints);
			}
			if (i >= num)
			{
				return false;
			}
			WayPoint wayPoint = this.mWayPoints[i++];
			while (i < num)
			{
				WayPoint wayPoint2 = this.mWayPoints[i];
				float num2 = Math.Abs(wayPoint.x - wayPoint2.x) + Math.Abs(wayPoint.y - wayPoint2.y);
				if (num2 > 10f)
				{
					return true;
				}
				wayPoint = wayPoint2;
				i++;
			}
			return false;
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x000A74B6 File Offset: 0x000A56B6
		public int GetNumPoints()
		{
			return Enumerable.Count<WayPoint>(this.mWayPoints);
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x000A74C3 File Offset: 0x000A56C3
		public int GetEndPoint()
		{
			return Enumerable.Count<WayPoint>(this.mWayPoints) - 1;
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x000A74D2 File Offset: 0x000A56D2
		public List<WayPoint> GetWayPointList()
		{
			return this.mWayPoints;
		}

		// Token: 0x04001AB9 RID: 6841
		protected List<WayPoint> mWayPoints = new List<WayPoint>();
	}
}
