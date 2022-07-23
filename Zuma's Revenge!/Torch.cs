using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x02000143 RID: 323
	public class Torch : IDisposable
	{
		// Token: 0x0600100E RID: 4110 RVA: 0x000A1025 File Offset: 0x0009F225
		public Torch()
		{
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x000A1034 File Offset: 0x0009F234
		public Torch(Torch rhs)
		{
			this.mOverlayAlpha = rhs.mOverlayAlpha;
			this.mWasHit = rhs.mWasHit;
			this.mDraw = rhs.mDraw;
			this.mX = rhs.mX;
			this.mY = rhs.mY;
			this.mWidth = rhs.mWidth;
			this.mHeight = rhs.mHeight;
			this.mActive = rhs.mActive;
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x000A10AE File Offset: 0x0009F2AE
		public virtual void Dispose()
		{
			GameApp.gApp.ReleaseTorchEffect(this.mFlame);
			GameApp.gApp.ReleaseTorchEffect(this.mFlameOut);
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x000A10D0 File Offset: 0x0009F2D0
		public void Update()
		{
			if (this.mFlame == null)
			{
				this.mFlame = GameApp.gApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_TORCHFLAME").Duplicate();
				this.mFlame.mEmitAfterTimeline = true;
				Common.SetFXNumScale(this.mFlame, GameApp.gApp.Is3DAccelerated() ? 1f : Common._M(0.5f));
			}
			if (this.mFlameOut == null)
			{
				this.mFlameOut = GameApp.gApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_TORCHFLAMEOUT").Duplicate();
				Common.SetFXNumScale(this.mFlameOut, GameApp.gApp.Is3DAccelerated() ? 1f : Common._M(0.5f));
			}
			if (this.mDraw)
			{
				if (this.mActive)
				{
					this.mFlame.mDrawTransform.LoadIdentity();
					float num = GameApp.DownScaleNum(1f);
					this.mFlame.mDrawTransform.Scale(num, num);
					if (this.mX > Common._DS(600))
					{
						this.mFlame.mDrawTransform.RotateDeg((float)Common._M(-75));
					}
					this.mFlame.mDrawTransform.Translate((float)(Common._S(this.mX) + Common._DS(Common._M(50))), (float)(Common._S(this.mY) + Common._DS(Common._M1(130))));
					this.mFlame.Update();
					return;
				}
				if (this.mFlameOut.mFrameNum <= (float)this.mFlameOut.mLastFrameNum)
				{
					this.mFlameOut.mDrawTransform.LoadIdentity();
					float num2 = GameApp.DownScaleNum(1f);
					this.mFlameOut.mDrawTransform.Scale(num2, num2);
					this.mFlameOut.mDrawTransform.Translate((float)(Common._S(this.mX) + Common._DS(Common._M(400))), (float)(Common._S(this.mY) + Common._DS(Common._M1(320))));
					this.mFlameOut.Update();
				}
			}
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x000A12E1 File Offset: 0x0009F4E1
		public void Draw(Graphics g)
		{
			if (this.mDraw && this.mActive && this.mFlame != null)
			{
				g.PushState();
				this.mFlame.Draw(g);
				g.PopState();
			}
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x000A1314 File Offset: 0x0009F514
		public void DrawAbove(Graphics g)
		{
			if (this.mDraw && this.mFlameOut != null && !this.mActive && this.mFlameOut.mFrameNum <= (float)this.mFlameOut.mLastFrameNum)
			{
				g.PushState();
				this.mFlameOut.Draw(g);
				g.PopState();
			}
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x000A136C File Offset: 0x0009F56C
		public bool CheckCollision(Rect r)
		{
			if (this.mActive && r.Intersects(new Rect(this.mX, this.mY, this.mWidth, this.mHeight)))
			{
				GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_TORCH_EXTINGUISHED));
				this.mActive = false;
				this.mWasHit = true;
				this.mFlame.mEmitAfterTimeline = false;
				this.mFlameOut.ResetAnim();
				return true;
			}
			return false;
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x000A13E4 File Offset: 0x0009F5E4
		public void SyncState(DataSync sync)
		{
			sync.SyncBoolean(ref this.mActive);
			sync.SyncLong(ref this.mX);
			sync.SyncLong(ref this.mY);
			sync.SyncLong(ref this.mWidth);
			sync.SyncLong(ref this.mHeight);
			sync.SyncBoolean(ref this.mWasHit);
			sync.SyncBoolean(ref this.mDraw);
			sync.SyncLong(ref this.mOverlayAlpha);
			if (sync.isRead() && this.mWasHit)
			{
				this.mDraw = (this.mActive = false);
			}
		}

		// Token: 0x040016FB RID: 5883
		public PIEffect mFlame;

		// Token: 0x040016FC RID: 5884
		public PIEffect mFlameOut;

		// Token: 0x040016FD RID: 5885
		public int mX;

		// Token: 0x040016FE RID: 5886
		public int mY;

		// Token: 0x040016FF RID: 5887
		public int mWidth;

		// Token: 0x04001700 RID: 5888
		public int mHeight;

		// Token: 0x04001701 RID: 5889
		public int mOverlayAlpha;

		// Token: 0x04001702 RID: 5890
		public bool mActive;

		// Token: 0x04001703 RID: 5891
		public bool mDraw = true;

		// Token: 0x04001704 RID: 5892
		public bool mWasHit;
	}
}
