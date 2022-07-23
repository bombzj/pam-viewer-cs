using System;
using System.Collections.Generic;
using System.Linq;
using JeffLib;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x02000085 RID: 133
	public class Bullet : Ball
	{
		// Token: 0x060008DB RID: 2267 RVA: 0x0004F8E0 File Offset: 0x0004DAE0
		public Bullet()
		{
			this.mVelX = 0f;
			this.mVelY = 0f;
			this.mHitBall = null;
			this.mHitPercent = 0f;
			this.mMergeSpeed = Common._M(0.025f);
			this.mJustFired = false;
			this.mDoNewMerge = false;
			this.mUpdateCount = 0;
			this.mHitDX = 0f;
			this.mHitDY = 0f;
			this.mAngleFired = 0f;
			this.mSkip = false;
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0004F980 File Offset: 0x0004DB80
		public Bullet(Bullet other)
		{
			base.CopyFrom(other);
			this.mHitBall = other.mHitBall;
			this.mVelX = other.mVelX;
			this.mVelY = other.mVelY;
			this.mHitX = other.mHitX;
			this.mHitY = other.mHitY;
			this.mHitDX = other.mHitDX;
			this.mHitDY = other.mHitDY;
			this.mDestX = other.mDestX;
			this.mDestY = other.mDestY;
			this.mHitPercent = other.mHitPercent;
			this.mMergeSpeed = other.mMergeSpeed;
			this.mAngleFired = other.mAngleFired;
			this.mUpdateCount = other.mUpdateCount;
			this.mHitInFront = other.mHitInFront;
			this.mHaveSetPrevBall = other.mHaveSetPrevBall;
			this.mJustFired = other.mJustFired;
			this.mDoNewMerge = other.mDoNewMerge;
			this.mSkip = other.mSkip;
			this.mGapInfo.AddRange(other.mGapInfo.ToArray());
			Array.Copy(other.mCurCurvePoint, this.mCurCurvePoint, this.mCurCurvePoint.Length);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0004FAB8 File Offset: 0x0004DCB8
		public override void Dispose()
		{
			this.SetBallInfo(null);
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0004FAC1 File Offset: 0x0004DCC1
		public void SetBallInfo(Bullet theBullet)
		{
			if (this.mHitBall != null)
			{
				this.mHitBall.SetBullet(theBullet);
			}
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0004FAD7 File Offset: 0x0004DCD7
		public void SetVelocity(float vx, float vy)
		{
			this.mVelX = vx;
			this.mVelY = vy;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0004FAE8 File Offset: 0x0004DCE8
		public void SetHitBall(Ball theBall, bool hitInFront)
		{
			this.SetBallInfo(null);
			this.mHaveSetPrevBall = false;
			this.mHitBall = theBall;
			this.mHitX = this.mX;
			this.mHitY = this.mY;
			this.mHitDX = this.mX - theBall.GetX();
			this.mHitDY = this.mY - theBall.GetY();
			this.mHitPercent = 0f;
			this.mHitInFront = hitInFront;
			this.SetBallInfo(this);
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0004FB64 File Offset: 0x0004DD64
		public void CheckSetHitBallToPrevBall()
		{
			if (this.mHaveSetPrevBall || this.mHitBall == null)
			{
				return;
			}
			Ball prevBall = this.mHitBall.GetPrevBall();
			if (prevBall == null)
			{
				return;
			}
			if (prevBall.CollidesWithPhysically(this) && !prevBall.GetIsExploding())
			{
				this.mHaveSetPrevBall = true;
				this.SetBallInfo(null);
				this.mHitBall = prevBall;
				this.mHitInFront = true;
				this.mHitX = this.mX;
				this.mHitY = this.mY;
				this.mHitDX = this.mX - prevBall.GetX();
				this.mHitDY = this.mY - prevBall.GetY();
				this.mHitPercent = 0f;
				this.SetBallInfo(this);
			}
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0004FC0F File Offset: 0x0004DE0F
		public void SetDestPos(float x, float y)
		{
			this.mDestX = x;
			this.mDestY = y;
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0004FC20 File Offset: 0x0004DE20
		public void SetDXPos()
		{
			float num = 1f - this.mHitPercent;
			this.mX += this.mHitDX * num;
			this.mY += this.mHitDY * num;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0004FC64 File Offset: 0x0004DE64
		public void Update(float theAmount)
		{
			this.mUpdateCount++;
			this.mDisplayType = this.mColorType;
			if (this.mHitBall == null)
			{
				float num = this.mVelX * theAmount;
				float num2 = this.mVelY * theAmount;
				this.mX += num;
				this.mY += num2;
			}
			else if (!this.mExploding)
			{
				this.mHitPercent += this.mMergeSpeed;
				if (this.mHitPercent > 1f)
				{
					this.mHitPercent = 1f;
				}
				if (!this.mDoNewMerge)
				{
					this.mX = this.mHitX + this.mHitPercent * (this.mDestX - this.mHitX);
					this.mY = this.mHitY + this.mHitPercent * (this.mDestY - this.mHitY);
				}
			}
			base.UpdateRotation();
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0004FD46 File Offset: 0x0004DF46
		public new void Update()
		{
			this.Update(1f);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0004FD53 File Offset: 0x0004DF53
		public void MergeFully()
		{
			this.mHitPercent = 1f;
			this.Update();
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0004FD68 File Offset: 0x0004DF68
		public Ball GetPushBall()
		{
			if (this.mHitBall == null)
			{
				return null;
			}
			Ball ball = (this.mHitInFront ? this.mHitBall.GetNextBall() : this.mHitBall);
			if (ball != null && (this.mDoNewMerge || ball.CollidesWithPhysically(this)))
			{
				return ball;
			}
			return null;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0004FDB4 File Offset: 0x0004DFB4
		public void UpdateHitPos()
		{
			this.mHitX = this.mX;
			this.mHitY = this.mY;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0004FDCE File Offset: 0x0004DFCE
		public void SetCurCurvePoint(int theCurveNum, int thePoint)
		{
			this.mCurCurvePoint[theCurveNum] = thePoint;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0004FDD9 File Offset: 0x0004DFD9
		public int GetCurCurvePoint(int theCurveNum)
		{
			return this.mCurCurvePoint[theCurveNum];
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0004FDE4 File Offset: 0x0004DFE4
		public bool AddGapInfo(int theCurve, int theDist, int theBallId)
		{
			foreach (GapInfo gapInfo in this.mGapInfo)
			{
				if (gapInfo.mBallId == theBallId)
				{
					return false;
				}
			}
			GapInfo gapInfo2 = new GapInfo();
			gapInfo2.mBallId = theBallId;
			gapInfo2.mDist = theDist;
			gapInfo2.mCurve = theCurve;
			this.mGapInfo.Add(gapInfo2);
			return true;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0004FE68 File Offset: 0x0004E068
		public int GetCurGapBall(int theCurveNum)
		{
			int result = 0;
			foreach (GapInfo gapInfo in this.mGapInfo)
			{
				if (gapInfo.mCurve == theCurveNum)
				{
					result = gapInfo.mBallId;
				}
			}
			return result;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0004FEC8 File Offset: 0x0004E0C8
		public int GetMinGapDist()
		{
			int num = 0;
			foreach (GapInfo gapInfo in this.mGapInfo)
			{
				if (num == 0 || gapInfo.mDist < num)
				{
					num = gapInfo.mDist;
				}
			}
			return num;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0004FF2C File Offset: 0x0004E12C
		public void RemoveGapInfoForBall(int theBallId)
		{
			int num = 0;
			while (num != Enumerable.Count<GapInfo>(this.mGapInfo))
			{
				if (this.mGapInfo[num].mBallId == theBallId)
				{
					this.mGapInfo.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0004FF74 File Offset: 0x0004E174
		public override void SyncState(DataSync theSync)
		{
			base.SyncState(theSync);
			theSync.SyncFloat(ref this.mVelX);
			theSync.SyncFloat(ref this.mVelY);
			theSync.SyncBoolean(ref this.mHitInFront);
			theSync.SyncBoolean(ref this.mHaveSetPrevBall);
			theSync.SyncFloat(ref this.mHitX);
			theSync.SyncFloat(ref this.mHitY);
			theSync.SyncFloat(ref this.mDestX);
			theSync.SyncFloat(ref this.mDestY);
			theSync.SyncFloat(ref this.mHitDX);
			theSync.SyncFloat(ref this.mHitDY);
			theSync.SyncLong(ref this.mUpdateCount);
			theSync.SyncBoolean(ref this.mHitInFront);
			theSync.SyncBoolean(ref this.mHaveSetPrevBall);
			theSync.SyncBoolean(ref this.mJustFired);
			theSync.SyncBoolean(ref this.mDoNewMerge);
			theSync.SyncFloat(ref this.mHitPercent);
			theSync.SyncFloat(ref this.mMergeSpeed);
			theSync.SyncFloat(ref this.mAngleFired);
			theSync.SyncBoolean(ref this.mSkip);
			for (int i = 0; i < 4; i++)
			{
				theSync.SyncLong(ref this.mCurCurvePoint[i]);
			}
			theSync.SyncPointer(this);
			this.SyncListGapInfos(theSync, true);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0005009C File Offset: 0x0004E29C
		private void SyncListGapInfos(DataSync sync, bool clear)
		{
			if (sync.isRead())
			{
				if (clear)
				{
					this.mGapInfo.Clear();
				}
				long num = sync.GetBuffer().ReadLong();
				int num2 = 0;
				while ((long)num2 < num)
				{
					GapInfo gapInfo = new GapInfo();
					gapInfo.SyncState(sync);
					this.mGapInfo.Add(gapInfo);
					num2++;
				}
				return;
			}
			sync.GetBuffer().WriteLong((long)this.mGapInfo.Count);
			foreach (GapInfo gapInfo2 in this.mGapInfo)
			{
				gapInfo2.SyncState(sync);
			}
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x00050150 File Offset: 0x0004E350
		public override void Draw(Graphics g, int xoff, int yoff)
		{
			if (!this.mIsCannon)
			{
				float mWayPoint = this.mWayPoint;
				this.mWayPoint = 0f;
				base.Draw(g, xoff, yoff);
				this.mWayPoint = mWayPoint;
				return;
			}
			if (this.mFrog.mBoard.LevelIsSkeletonBoss())
			{
				Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_GLOWBALL);
				float num = Common._S(this.mX) - (float)(imageByID.mWidth / 2);
				float num2 = Common._S(this.mY) - (float)(imageByID.mHeight / 2);
				g.DrawImage(imageByID, (int)num, (int)num2);
				g.PushState();
				g.SetColorizeImages(true);
				g.SetDrawMode(1);
				int alphaFromUpdateCount = JeffLib.Common.GetAlphaFromUpdateCount(this.mUpdateCount, Common._M(64));
				g.SetColor(255, 255, 255, alphaFromUpdateCount);
				g.DrawImage(imageByID, (int)num, (int)num2);
				g.PopState();
				return;
			}
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_CANNON_BALL);
			float num3 = Common._S(this.mX) - (float)(imageByID2.mWidth / 2);
			float num4 = Common._S(this.mY) - (float)(imageByID2.mHeight / 2);
			if (g.Is3D())
			{
				g.DrawImageRotatedF(imageByID2, num3, num4, (double)(this.mRotation + 3.14159f));
				return;
			}
			g.DrawImageRotated(imageByID2, (int)num3, (int)num4, (double)(this.mRotation + 3.14159f));
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x000502AC File Offset: 0x0004E4AC
		public new void Draw(Graphics g)
		{
			this.Draw(g, 0, 0);
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x000502B7 File Offset: 0x0004E4B7
		public Ball GetHitBall()
		{
			return this.mHitBall;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x000502BF File Offset: 0x0004E4BF
		public float GetHitPercent()
		{
			return this.mHitPercent;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x000502C7 File Offset: 0x0004E4C7
		public float GetVelX()
		{
			return this.mVelX;
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x000502CF File Offset: 0x0004E4CF
		public float GetVelY()
		{
			return this.mVelY;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x000502D7 File Offset: 0x0004E4D7
		public bool GetHitInFront()
		{
			return this.mHitInFront;
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x000502DF File Offset: 0x0004E4DF
		public bool GetJustFired()
		{
			return this.mJustFired;
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x000502E7 File Offset: 0x0004E4E7
		public new int GetNumGaps()
		{
			return Enumerable.Count<GapInfo>(this.mGapInfo);
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x000502F4 File Offset: 0x0004E4F4
		public int GetUpdateCount()
		{
			return this.mUpdateCount;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x000502FC File Offset: 0x0004E4FC
		public void SetJustFired(bool fired)
		{
			this.mJustFired = fired;
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00050305 File Offset: 0x0004E505
		public void SetMergeSpeed(float theSpeed)
		{
			this.mMergeSpeed = theSpeed;
		}

		// Token: 0x040006E0 RID: 1760
		public Ball mHitBall;

		// Token: 0x040006E1 RID: 1761
		public float mVelX;

		// Token: 0x040006E2 RID: 1762
		public float mVelY;

		// Token: 0x040006E3 RID: 1763
		public float mHitX;

		// Token: 0x040006E4 RID: 1764
		public float mHitY;

		// Token: 0x040006E5 RID: 1765
		public float mHitDX;

		// Token: 0x040006E6 RID: 1766
		public float mHitDY;

		// Token: 0x040006E7 RID: 1767
		public float mDestX;

		// Token: 0x040006E8 RID: 1768
		public float mDestY;

		// Token: 0x040006E9 RID: 1769
		public float mHitPercent;

		// Token: 0x040006EA RID: 1770
		public float mMergeSpeed;

		// Token: 0x040006EB RID: 1771
		public float mAngleFired;

		// Token: 0x040006EC RID: 1772
		public new int mUpdateCount;

		// Token: 0x040006ED RID: 1773
		public bool mHitInFront;

		// Token: 0x040006EE RID: 1774
		public bool mHaveSetPrevBall;

		// Token: 0x040006EF RID: 1775
		public bool mJustFired;

		// Token: 0x040006F0 RID: 1776
		public bool mDoNewMerge;

		// Token: 0x040006F1 RID: 1777
		public bool mSkip;

		// Token: 0x040006F2 RID: 1778
		public List<GapInfo> mGapInfo = new List<GapInfo>();

		// Token: 0x040006F3 RID: 1779
		public int[] mCurCurvePoint = new int[4];
	}
}
