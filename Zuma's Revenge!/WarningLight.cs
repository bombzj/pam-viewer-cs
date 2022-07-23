using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x02000091 RID: 145
	public class WarningLight
	{
		// Token: 0x06000947 RID: 2375 RVA: 0x00053930 File Offset: 0x00051B30
		public WarningLight(float x, float y)
		{
			this.mX = x;
			this.mY = y;
			this.mAlpha = 0f;
			this.mUpdateCount = 0;
			this.mAngle = 0f;
			this.mState = 0;
			this.mWaypoint = -1f;
			this.mPulseAlpha = 0f;
			this.mPulseRate = 0f;
			this.mPriority = 0;
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x000539A0 File Offset: 0x00051BA0
		public bool Update()
		{
			this.mUpdateCount++;
			float num = Common._M(5f);
			if (this.mState == 1)
			{
				this.mAlpha = Math.Min(255f, this.mAlpha + num);
				if (this.mAlpha >= 255f)
				{
					this.mState = 0;
				}
			}
			else if (this.mState == -1)
			{
				this.mAlpha = Math.Max(0f, this.mAlpha - num);
				if (this.mAlpha <= 0f)
				{
					this.mState = 0;
				}
			}
			else if (this.mPulseRate != 0f)
			{
				this.mPulseAlpha += ((this.mPulseRate > 0f) ? (this.mPulseRate * 2f) : this.mPulseRate);
				if (this.mPulseRate < 0f && this.mPulseAlpha <= 0f)
				{
					this.mPulseRate = 0f;
					this.mPulseAlpha = 0f;
				}
				else if (this.mPulseAlpha >= 255f && this.mPulseRate > 0f)
				{
					this.mPulseRate *= -1f;
					this.mPulseAlpha = 255f;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00053AEC File Offset: 0x00051CEC
		public void Draw(Graphics g)
		{
			if (this.mAlpha == 0f)
			{
				return;
			}
			g.PushState();
			if (this.mAlpha != 0f)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)this.mAlpha);
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_SKULL_PATH);
			g.DrawImageRotated(imageByID, (int)(Common._S(this.mX) - (float)(imageByID.mWidth / 2)), (int)(Common._S(this.mY) - (float)(imageByID.mHeight / 2)), (double)(this.mAngle + 1.570795f));
			if (this.mPulseAlpha != 0f)
			{
				Image imageByID2 = Res.GetImageByID(ResID.IMAGE_SKULL_PATH_LIT);
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)this.mPulseAlpha);
				g.DrawImageRotated(imageByID2, (int)(Common._S(this.mX) - (float)(imageByID2.mWidth / 2)), (int)(Common._S(this.mY) - (float)(imageByID2.mHeight / 2)), (double)(this.mAngle + 1.570795f));
			}
			g.SetColorizeImages(false);
			g.PopState();
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00053C10 File Offset: 0x00051E10
		public void SyncState(DataSync sync)
		{
			sync.SyncFloat(ref this.mAlpha);
			sync.SyncFloat(ref this.mPulseAlpha);
			sync.SyncFloat(ref this.mPulseRate);
			sync.SyncLong(ref this.mState);
			sync.SyncLong(ref this.mUpdateCount);
		}

		// Token: 0x04000760 RID: 1888
		public float mX;

		// Token: 0x04000761 RID: 1889
		public float mY;

		// Token: 0x04000762 RID: 1890
		public float mAlpha;

		// Token: 0x04000763 RID: 1891
		public float mAngle;

		// Token: 0x04000764 RID: 1892
		public float mPulseAlpha;

		// Token: 0x04000765 RID: 1893
		public float mPulseRate;

		// Token: 0x04000766 RID: 1894
		public float mWaypoint;

		// Token: 0x04000767 RID: 1895
		public int mState;

		// Token: 0x04000768 RID: 1896
		public int mUpdateCount;

		// Token: 0x04000769 RID: 1897
		public int mPriority;
	}
}
