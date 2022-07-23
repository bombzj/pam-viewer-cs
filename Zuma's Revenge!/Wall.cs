using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x020000FB RID: 251
	public class Wall
	{
		// Token: 0x06000D1E RID: 3358 RVA: 0x0007FD38 File Offset: 0x0007DF38
		public void SyncState(DataSync sync)
		{
			sync.SyncFloat(ref this.mX);
			sync.SyncFloat(ref this.mY);
			sync.SyncFloat(ref this.mWidth);
			sync.SyncFloat(ref this.mHeight);
			sync.SyncLong(ref this.mStrength);
			sync.SyncLong(ref this.mOrgStrength);
			sync.SyncLong(ref this.mMinRespawnTimer);
			sync.SyncLong(ref this.mMaxRespawnTimer);
			sync.SyncLong(ref this.mCurRespawnTimer);
			sync.SyncLong(ref this.mMinLifeTimer);
			sync.SyncLong(ref this.mMaxLifeTimer);
			sync.SyncLong(ref this.mCurLifeTimer);
			sync.SyncLong(ref this.mId);
			sync.SyncLong(ref this.mColor.mRed);
			sync.SyncLong(ref this.mColor.mGreen);
			sync.SyncLong(ref this.mColor.mBlue);
			sync.SyncLong(ref this.mColor.mAlpha);
			sync.SyncLong(ref this.mUpdateCount);
			sync.SyncLong(ref this.mState);
			sync.SyncLong(ref this.mSize);
			sync.SyncLong(ref this.mMaxSize);
			sync.SyncLong(ref this.mCel);
			sync.SyncLong(ref this.mExpCel);
			sync.SyncLong(ref this.mType);
			sync.SyncFloat(ref this.mVX);
			sync.SyncFloat(ref this.mVY);
			sync.SyncLong(ref this.mSpacing);
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0007FEA0 File Offset: 0x0007E0A0
		public void Update()
		{
			if (this.mCurLifeTimer > 0 && --this.mCurLifeTimer == 0)
			{
				this.mType = 0;
				this.mCurRespawnTimer = MathUtils.IntRange(this.mMinRespawnTimer, this.mMaxRespawnTimer);
				return;
			}
			if (this.mCurRespawnTimer > 0 && --this.mCurRespawnTimer == 0)
			{
				this.mType = 1;
				this.mCurLifeTimer = MathUtils.IntRange(this.mMinLifeTimer, this.mMaxLifeTimer);
			}
			if (this.mType == 0 && this.mCurRespawnTimer <= 0)
			{
				return;
			}
			this.mUpdateCount++;
			if (this.mVX != 0f)
			{
				this.mX += this.mVX;
			}
			if (this.mVY != 0f)
			{
				this.mY += this.mVY;
			}
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0007FF82 File Offset: 0x0007E182
		public void Draw(Graphics g)
		{
			if (this.mStrength != 0)
			{
				int num = this.mType;
			}
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0007FF93 File Offset: 0x0007E193
		public bool Hit()
		{
			return false;
		}

		// Token: 0x04000BE8 RID: 3048
		public float mX;

		// Token: 0x04000BE9 RID: 3049
		public float mY;

		// Token: 0x04000BEA RID: 3050
		public float mWidth;

		// Token: 0x04000BEB RID: 3051
		public float mHeight;

		// Token: 0x04000BEC RID: 3052
		public float mVX;

		// Token: 0x04000BED RID: 3053
		public float mVY;

		// Token: 0x04000BEE RID: 3054
		public int mSpacing;

		// Token: 0x04000BEF RID: 3055
		public int mStrength;

		// Token: 0x04000BF0 RID: 3056
		public int mOrgStrength;

		// Token: 0x04000BF1 RID: 3057
		public int mMinRespawnTimer;

		// Token: 0x04000BF2 RID: 3058
		public int mMaxRespawnTimer;

		// Token: 0x04000BF3 RID: 3059
		public int mCurRespawnTimer;

		// Token: 0x04000BF4 RID: 3060
		public int mMinLifeTimer;

		// Token: 0x04000BF5 RID: 3061
		public int mMaxLifeTimer;

		// Token: 0x04000BF6 RID: 3062
		public int mCurLifeTimer;

		// Token: 0x04000BF7 RID: 3063
		public int mId;

		// Token: 0x04000BF8 RID: 3064
		public int mUpdateCount;

		// Token: 0x04000BF9 RID: 3065
		public int mState;

		// Token: 0x04000BFA RID: 3066
		public int mSize = 1;

		// Token: 0x04000BFB RID: 3067
		public int mMaxSize = 1;

		// Token: 0x04000BFC RID: 3068
		public SexyColor mColor = default(SexyColor);

		// Token: 0x04000BFD RID: 3069
		public Image mImage;

		// Token: 0x04000BFE RID: 3070
		public int mCel;

		// Token: 0x04000BFF RID: 3071
		public int mExpCel;

		// Token: 0x04000C00 RID: 3072
		public int mType = 1;
	}
}
