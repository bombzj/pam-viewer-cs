using System;

namespace ZumasRevenge
{
	// Token: 0x0200007C RID: 124
	public class BossBullet : IDisposable
	{
		// Token: 0x060008A7 RID: 2215 RVA: 0x0004CE30 File Offset: 0x0004B030
		public BossBullet()
		{
			this.mDelay = (this.mBouncesLeft = (this.mUpdateCount = (this.mOffscreenPause = 0)));
			this.mGravity = (this.mTargetVX = (this.mTargetVY = 0f));
			this.mDeleteInstantly = false;
			this.mSize = 1f;
			this.mShotType = 0;
			this.mId = -1;
			this.mInitialSpeed = 0f;
			this.mVolcanoShot = (this.mHoming = false);
			this.mAmp = (this.mFreq = 0f);
			this.mSineMotion = false;
			this.mCanHitPlayer = true;
			this.mState = 0;
			this.mImageNum = 0;
			this.mAngle = 0f;
			this.mAlpha = 255f;
			this.mCel = 0;
			this.mData = null;
			this.mBossShoot = null;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0004CF20 File Offset: 0x0004B120
		public BossBullet(BossBullet rhs)
			: this()
		{
			if (rhs == null)
			{
				return;
			}
			this.mDelay = rhs.mDelay;
			this.mBouncesLeft = rhs.mBouncesLeft;
			this.mUpdateCount = rhs.mUpdateCount;
			this.mOffscreenPause = rhs.mOffscreenPause;
			this.mGravity = rhs.mGravity;
			this.mTargetVX = rhs.mTargetVX;
			this.mTargetVY = rhs.mTargetVY;
			this.mDeleteInstantly = rhs.mDeleteInstantly;
			this.mSize = rhs.mSize;
			this.mShotType = rhs.mShotType;
			this.mId = rhs.mId;
			this.mInitialSpeed = rhs.mInitialSpeed;
			this.mVolcanoShot = rhs.mVolcanoShot;
			this.mHoming = rhs.mHoming;
			this.mAmp = rhs.mAmp;
			this.mFreq = rhs.mFreq;
			this.mSineMotion = rhs.mSineMotion;
			this.mCanHitPlayer = rhs.mCanHitPlayer;
			this.mState = rhs.mState;
			this.mImageNum = rhs.mImageNum;
			this.mAngle = rhs.mAngle;
			this.mAlpha = rhs.mAlpha;
			this.mCel = rhs.mCel;
			this.mData = rhs.mData;
			this.mBossShoot = rhs.mBossShoot;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0004D063 File Offset: 0x0004B263
		public virtual void Dispose()
		{
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0004D068 File Offset: 0x0004B268
		public void SyncState(DataSync sync)
		{
			sync.SyncFloat(ref this.mVX);
			sync.SyncFloat(ref this.mVY);
			sync.SyncFloat(ref this.mX);
			sync.SyncFloat(ref this.mY);
			sync.SyncFloat(ref this.mAmp);
			sync.SyncFloat(ref this.mFreq);
			sync.SyncBoolean(ref this.mSineMotion);
			sync.SyncLong(ref this.mUpdateCount);
			sync.SyncLong(ref this.mDelay);
			sync.SyncLong(ref this.mState);
			sync.SyncLong(ref this.mImageNum);
			sync.SyncFloat(ref this.mAngle);
			sync.SyncBoolean(ref this.mHoming);
			sync.SyncFloat(ref this.mTargetVX);
			sync.SyncBoolean(ref this.mCanHitPlayer);
			sync.SyncFloat(ref this.mTargetVY);
			sync.SyncFloat(ref this.mInitialSpeed);
			sync.SyncLong(ref this.mOffscreenPause);
			sync.SyncBoolean(ref this.mVolcanoShot);
			sync.SyncFloat(ref this.mSize);
			sync.SyncFloat(ref this.mAlpha);
			sync.SyncLong(ref this.mShotType);
			sync.SyncLong(ref this.mCel);
			sync.SyncLong(ref this.mBouncesLeft);
			sync.SyncLong(ref this.mId);
		}

		// Token: 0x0400067A RID: 1658
		public float mVX;

		// Token: 0x0400067B RID: 1659
		public float mVY;

		// Token: 0x0400067C RID: 1660
		public float mInitialSpeed;

		// Token: 0x0400067D RID: 1661
		public float mTargetVX;

		// Token: 0x0400067E RID: 1662
		public float mTargetVY;

		// Token: 0x0400067F RID: 1663
		public float mX;

		// Token: 0x04000680 RID: 1664
		public float mY;

		// Token: 0x04000681 RID: 1665
		public float mAmp;

		// Token: 0x04000682 RID: 1666
		public float mFreq;

		// Token: 0x04000683 RID: 1667
		public float mGravity;

		// Token: 0x04000684 RID: 1668
		public float mAngle;

		// Token: 0x04000685 RID: 1669
		public float mSize;

		// Token: 0x04000686 RID: 1670
		public float mAlpha;

		// Token: 0x04000687 RID: 1671
		public bool mSineMotion;

		// Token: 0x04000688 RID: 1672
		public bool mHoming;

		// Token: 0x04000689 RID: 1673
		public bool mCanHitPlayer;

		// Token: 0x0400068A RID: 1674
		public bool mDeleteInstantly;

		// Token: 0x0400068B RID: 1675
		public int mBouncesLeft;

		// Token: 0x0400068C RID: 1676
		public int mId;

		// Token: 0x0400068D RID: 1677
		public int mUpdateCount;

		// Token: 0x0400068E RID: 1678
		public int mDelay;

		// Token: 0x0400068F RID: 1679
		public int mState;

		// Token: 0x04000690 RID: 1680
		public int mImageNum;

		// Token: 0x04000691 RID: 1681
		public int mOffscreenPause;

		// Token: 0x04000692 RID: 1682
		public int mShotType;

		// Token: 0x04000693 RID: 1683
		public int mCel;

		// Token: 0x04000694 RID: 1684
		public bool mVolcanoShot;

		// Token: 0x04000695 RID: 1685
		public object mData;

		// Token: 0x04000696 RID: 1686
		public BossShoot mBossShoot;
	}
}
