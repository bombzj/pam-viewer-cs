using System;
using JeffLib;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x020000B7 RID: 183
	public class CannonPowerEffect : PowerEffect
	{
		// Token: 0x06000A97 RID: 2711 RVA: 0x0006774C File Offset: 0x0006594C
		public CannonPowerEffect(Ball b)
		{
			int radius = b.GetRadius();
			int num = (int)b.GetX() - radius;
			int num2 = (int)b.GetY() - radius;
			this.mRings[0].mX = (float)(num + 18);
			this.mRings[0].mY = (float)(num2 + 11);
			this.mRings[1].mX = (float)(num + 11);
			this.mRings[1].mY = (float)(num2 + 23);
			this.mRings[2].mX = (float)(num + 24);
			this.mRings[2].mY = (float)(num2 + 22);
			this.mBallRotation = b.GetRotation();
			for (int i = 0; i < 3; i++)
			{
				SexyFramework.Common.RotatePoint(this.mBallRotation - 1.5707645f, ref this.mRings[i].mX, ref this.mRings[i].mY, (float)(num + radius), (float)(num2 + radius));
			}
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0006785C File Offset: 0x00065A5C
		public CannonPowerEffect()
		{
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00067898 File Offset: 0x00065A98
		public override void Update()
		{
			if (this.IsDone())
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < 3; i++)
			{
				if (this.mRings[i].mSizePct < 1f)
				{
					this.mRings[i].mSizePct += 0.06666667f;
					if (this.mRings[i].mSizePct >= 1f)
					{
						this.mRings[i].mSizePct = 1f;
						float num2 = MathUtils.DegreesToRadians(this.mBallRotation + (float)(120 * i));
						this.mRings[i].mVX = (float)Math.Cos((double)num2) * 1.2f;
						this.mRings[i].mVY = -(float)Math.Sin((double)num2) * 1.2f;
						this.mRings[i].mTX = this.mRings[i].mX + this.mRings[i].mVX * 15f;
						this.mRings[i].mTY = this.mRings[i].mY + this.mRings[i].mVY * 15f;
					}
				}
				else if (this.mRings[i].mVX != 0f || this.mRings[i].mVY != 0f)
				{
					this.mRings[i].mX += this.mRings[i].mVX;
					this.mRings[i].mY += this.mRings[i].mVY;
					if (JeffLib.Common.DoneMoving(this.mRings[i].mX, this.mRings[i].mVX, this.mRings[i].mTX))
					{
						this.mRings[i].mX = this.mRings[i].mTX;
						this.mRings[i].mVX = 0f;
					}
					if (JeffLib.Common.DoneMoving(this.mRings[i].mY, this.mRings[i].mVY, this.mRings[i].mTY))
					{
						this.mRings[i].mY = this.mRings[i].mTY;
						this.mRings[i].mVY = 0f;
					}
				}
				else if (this.mRings[i].mAlpha != 0f)
				{
					this.mRings[i].mSizePct += 1f / (float)Common._M(25);
					this.mRings[i].mAlpha -= Common._M(6f);
					if (this.mRings[i].mAlpha < 0f)
					{
						num++;
						this.mRings[i].mAlpha = 0f;
					}
				}
				else
				{
					num++;
				}
			}
			if (num == 3)
			{
				this.mDone = true;
			}
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00067B80 File Offset: 0x00065D80
		public override void Draw(Graphics g)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_CANNON_RING_BLUE + this.mColorType);
			g.SetDrawMode(1);
			for (int i = 0; i < 3; i++)
			{
				int num = (int)(this.mRings[i].mSizePct * (float)imageByID.mWidth);
				int num2 = (int)(this.mRings[i].mSizePct * (float)imageByID.mHeight);
				if (this.mRings[i].mAlpha != 255f)
				{
					g.SetColor(255, 255, 255, (int)this.mRings[i].mAlpha);
					g.SetColorizeImages(true);
				}
				g.DrawImage(imageByID, (int)(Common._S(this.mRings[i].mX) - (float)(num / 2)), (int)(Common._S(this.mRings[i].mY) - (float)(num2 / 2)), num, num2);
				g.SetColorizeImages(false);
			}
			g.SetDrawMode(0);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00067C6C File Offset: 0x00065E6C
		public override void SyncState(DataSync sync)
		{
			base.SyncState(sync);
			sync.SyncFloat(ref this.mBallRotation);
			for (int i = 0; i < 3; i++)
			{
				sync.SyncFloat(ref this.mRings[i].mX);
				sync.SyncFloat(ref this.mRings[i].mY);
				sync.SyncFloat(ref this.mRings[i].mVX);
				sync.SyncFloat(ref this.mRings[i].mVY);
				sync.SyncFloat(ref this.mRings[i].mTX);
				sync.SyncFloat(ref this.mRings[i].mTY);
				sync.SyncFloat(ref this.mRings[i].mSizePct);
				sync.SyncFloat(ref this.mRings[i].mAlpha);
			}
		}

		// Token: 0x0400090E RID: 2318
		protected CannonPowerEffect.CannonRing[] mRings = new CannonPowerEffect.CannonRing[]
		{
			new CannonPowerEffect.CannonRing(),
			new CannonPowerEffect.CannonRing(),
			new CannonPowerEffect.CannonRing()
		};

		// Token: 0x0400090F RID: 2319
		protected float mBallRotation;

		// Token: 0x020000B8 RID: 184
		protected class CannonRing
		{
			// Token: 0x04000910 RID: 2320
			public float mX;

			// Token: 0x04000911 RID: 2321
			public float mY;

			// Token: 0x04000912 RID: 2322
			public float mVX;

			// Token: 0x04000913 RID: 2323
			public float mVY;

			// Token: 0x04000914 RID: 2324
			public float mTX;

			// Token: 0x04000915 RID: 2325
			public float mTY;

			// Token: 0x04000916 RID: 2326
			public float mSizePct;

			// Token: 0x04000917 RID: 2327
			public float mAlpha = 255f;
		}
	}
}
