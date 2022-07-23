using System;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x020000C4 RID: 196
	public class FrogFlyOff
	{
		// Token: 0x06000ACA RID: 2762 RVA: 0x0006A879 File Offset: 0x00068A79
		public FrogFlyOff()
		{
			this.mPlayThud = false;
			this.mFrogJumpTime = Common._M(80);
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0006A8A0 File Offset: 0x00068AA0
		public virtual void Dispose()
		{
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0006A8A4 File Offset: 0x00068AA4
		public void JumpOut(Gun frog, int dest_x, int dest_y, int start_x, int start_y, float angle)
		{
			FrogFlyOff.FROG_START_SCALE = Common._M(0.5f);
			this.mTimer = 0;
			this.mJumpOut = true;
			this.mFrog = frog;
			this.mFrogX = (float)((start_x == int.MaxValue) ? this.mFrog.GetCenterX() : start_x);
			this.mFrogY = (float)((start_y == int.MaxValue) ? this.mFrog.GetCenterY() : start_y);
			Image imageByID = Res.GetImageByID(ResID.IMAGE_LARGE_FROG);
			if (dest_x == 2147483647)
			{
				dest_x = GlobalMembers.gSexyApp.mWidth - imageByID.mWidth / 2;
			}
			if (dest_y == 2147483647)
			{
				dest_y = -(int)this.mFrogY - imageByID.mHeight / 2;
			}
			dest_x -= (int)this.mFrogX;
			this.mFrogVX = (float)dest_x / (float)this.mFrogJumpTime;
			this.mFrogVY = (float)dest_y / (float)this.mFrogJumpTime;
			this.mScaleDelta = (Common._M(2f) - FrogFlyOff.FROG_START_SCALE) / (float)this.mFrogJumpTime;
			this.mFrogScale = FrogFlyOff.FROG_START_SCALE;
			this.mFrogAngle = (this.mDestFrogAngle = (MathUtils._eq(angle, float.MaxValue) ? this.mFrog.GetAngle() : angle));
			this.mFrogAngleDelta = Common._M(0.15f);
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0006A9E6 File Offset: 0x00068BE6
		public void JumpOut(Gun frog, int dest_x, int dest_y, int start_x, int start_y)
		{
			this.JumpOut(frog, dest_x, dest_y, start_x, start_y, float.MaxValue);
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0006A9FA File Offset: 0x00068BFA
		public void JumpOut(Gun frog, int dest_x, int dest_y, int start_x)
		{
			this.JumpOut(frog, dest_x, dest_y, start_x, int.MaxValue);
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0006AA0C File Offset: 0x00068C0C
		public void JumpOut(Gun frog, int dest_x, int dest_y)
		{
			this.JumpOut(frog, dest_x, dest_y, int.MaxValue);
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0006AA1C File Offset: 0x00068C1C
		public void JumpOut(Gun frog, int dest_x)
		{
			this.JumpOut(frog, dest_x, int.MaxValue, int.MaxValue);
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0006AA30 File Offset: 0x00068C30
		public void JumpOut(Gun frog)
		{
			this.JumpOut(frog, int.MaxValue);
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0006AA40 File Offset: 0x00068C40
		public void JumpIn(Gun frog, int dest_x, int dest_y, bool continue_from_jump_out, int jump_to_x, int jump_to_y)
		{
			FrogFlyOff.FROG_START_SCALE = Common._M(0.5f);
			if (!continue_from_jump_out)
			{
				this.JumpOut(frog, jump_to_x, jump_to_y);
				this.mFrogX += this.mFrogVX * (float)this.mFrogJumpTime;
				this.mFrogY += this.mFrogVY * (float)this.mFrogJumpTime;
				this.mFrogAngle += this.mFrogAngleDelta * (float)this.mFrogJumpTime;
			}
			this.mTimer = 0;
			this.mFrog = frog;
			this.mJumpOut = false;
			this.mPlayThud = true;
			this.mFrogScale = Common._M(2f);
			this.mScaleDelta *= -1f;
			this.RehupFrogPosition(dest_x, dest_y);
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0006AB02 File Offset: 0x00068D02
		public void JumpIn(Gun frog, int dest_x, int dest_y, bool continue_from_jump_out, int jump_to_x)
		{
			this.JumpIn(frog, dest_x, dest_y, continue_from_jump_out, jump_to_x, int.MaxValue);
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0006AB16 File Offset: 0x00068D16
		public void JumpIn(Gun frog, int dest_x, int dest_y, bool continue_from_jump_out)
		{
			this.JumpIn(frog, dest_x, dest_y, continue_from_jump_out, int.MaxValue);
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0006AB28 File Offset: 0x00068D28
		public void JumpIn(Gun frog, int dest_x, int dest_y)
		{
			this.JumpIn(frog, dest_x, dest_y, true);
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0006AB34 File Offset: 0x00068D34
		public bool HasCompletedFlyOff()
		{
			return this.mTimer > this.mFrogJumpTime;
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0006AB44 File Offset: 0x00068D44
		public void RehupFrogPosition(int dest_x, int dest_y)
		{
			this.RehupFrogPosition(dest_x, dest_y, this.mFrog.GetAngle());
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0006AB5C File Offset: 0x00068D5C
		public void RehupFrogPosition(int dest_x, int dest_y, float forced_dest_angle)
		{
			this.mFrogAngleDelta = -(this.mFrogAngle - forced_dest_angle) / (float)this.mFrogJumpTime;
			this.mFrogVX = -(this.mFrogX - (float)dest_x) / (float)this.mFrogJumpTime;
			this.mFrogVY = -(this.mFrogY - (float)dest_y) / (float)this.mFrogJumpTime;
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0006ABB0 File Offset: 0x00068DB0
		public void Update()
		{
			if (this.mTimer > this.mFrogJumpTime)
			{
				return;
			}
			this.mTimer++;
			if (this.mJumpOut)
			{
				if (this.mFrogScale < 1f)
				{
					if (this.mTimer == 1)
					{
						GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_FROG_LAUNCH));
					}
					this.mFrogScale += this.mScaleDelta;
					if (this.mFrogScale > 1f)
					{
						this.mFrogScale = 1f;
					}
				}
				this.mFrogAngle += this.mFrogAngleDelta;
				this.mFrogX += this.mFrogVX;
				this.mFrogY += this.mFrogVY;
				return;
			}
			this.mFrogAngle += this.mFrogAngleDelta;
			this.mFrogX += this.mFrogVX;
			this.mFrogY += this.mFrogVY;
			if (this.mTimer >= this.mFrogJumpTime)
			{
				this.mFrogAngle = this.mDestFrogAngle;
			}
			if (this.mFrogScale > FrogFlyOff.FROG_START_SCALE)
			{
				this.mFrogScale += this.mScaleDelta;
				this.PlayFrogLandingSound();
				if (this.mFrogScale < FrogFlyOff.FROG_START_SCALE)
				{
					this.mFrogScale = FrogFlyOff.FROG_START_SCALE;
				}
			}
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0006AD08 File Offset: 0x00068F08
		public void Draw(Graphics g)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_LARGE_FROG);
			if (this.mFrogY + (float)(imageByID.mHeight / 2) >= 0f)
			{
				Image imageByID2 = Res.GetImageByID(ResID.IMAGE_FROG_SHADOW);
				this.mGlobalTranform.Reset();
				this.mGlobalTranform.RotateRad(this.mFrogAngle);
				float num = (float)this.mTimer / (float)this.mFrogJumpTime;
				if (num > 1f)
				{
					num = 1f;
				}
				float num2 = Common._M(1f);
				float num3 = Common._M(3f);
				float num4 = Common._M(1f);
				float num5 = Common._M(0f);
				float num6 = Common._M(0f);
				float num7 = Common._M(150f);
				float num8;
				float num9;
				float num10;
				if (this.mJumpOut)
				{
					num8 = num2 + (num3 - num2) * num;
					num9 = num4 + (num5 - num4) * num;
					num10 = num6 + (num7 - num6) * num;
				}
				else
				{
					num8 = num3 - (num3 - num2) * num;
					num9 = num5 - (num5 - num4) * num;
					num10 = num7 - (num7 - num6) * num;
				}
				this.mGlobalTranform.Scale(num8, num8);
				g.SetColorizeImages(true);
				g.SetColor(0, 0, 0, (int)(num9 * 255f));
				g.DrawImageTransform(imageByID2, this.mGlobalTranform, imageByID2.GetCelRect(0), Common._S(this.mFrogX - num10), Common._S(this.mFrogY + num10));
				g.SetColorizeImages(false);
				this.mGlobalTranform.Reset();
				this.mGlobalTranform.RotateRad(this.mFrogAngle);
				this.mGlobalTranform.Scale(this.mFrogScale, this.mFrogScale);
				g.DrawImageTransform(imageByID, this.mGlobalTranform, Common._S(this.mFrogX), Common._S(this.mFrogY));
			}
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0006AECC File Offset: 0x000690CC
		public void SyncState(DataSync sync)
		{
			sync.SyncFloat(ref this.mFrogScale);
			sync.SyncFloat(ref this.mFrogX);
			sync.SyncFloat(ref this.mFrogY);
			sync.SyncFloat(ref this.mFrogAngle);
			sync.SyncFloat(ref this.mFrogAngleDelta);
			sync.SyncFloat(ref this.mFrogVX);
			sync.SyncFloat(ref this.mFrogVY);
			sync.SyncFloat(ref this.mScaleDelta);
			sync.SyncFloat(ref this.mDestFrogAngle);
			sync.SyncLong(ref this.mFrogJumpTime);
			sync.SyncLong(ref this.mTimer);
			sync.SyncBoolean(ref this.mJumpOut);
			if (sync.isRead())
			{
				this.mFrog = GameApp.gApp.mBoard.mFrog;
			}
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0006AF86 File Offset: 0x00069186
		private void PlayFrogLandingSound()
		{
			if (!this.mPlayThud || this.mFrogScale > FrogFlyOff.FROG_START_SCALE - this.mScaleDelta * 15f)
			{
				return;
			}
			GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_FROG_FALL));
			this.mPlayThud = false;
		}

		// Token: 0x04000966 RID: 2406
		public float mFrogScale;

		// Token: 0x04000967 RID: 2407
		public float mFrogX;

		// Token: 0x04000968 RID: 2408
		public float mFrogY;

		// Token: 0x04000969 RID: 2409
		public float mFrogAngle;

		// Token: 0x0400096A RID: 2410
		public float mFrogAngleDelta;

		// Token: 0x0400096B RID: 2411
		public float mFrogVX;

		// Token: 0x0400096C RID: 2412
		public float mFrogVY;

		// Token: 0x0400096D RID: 2413
		public float mScaleDelta;

		// Token: 0x0400096E RID: 2414
		public float mDestFrogAngle;

		// Token: 0x0400096F RID: 2415
		public int mFrogJumpTime;

		// Token: 0x04000970 RID: 2416
		public int mTimer;

		// Token: 0x04000971 RID: 2417
		public bool mJumpOut;

		// Token: 0x04000972 RID: 2418
		public bool mPlayThud;

		// Token: 0x04000973 RID: 2419
		public Gun mFrog;

		// Token: 0x04000974 RID: 2420
		protected Transform mGlobalTranform = new Transform();

		// Token: 0x04000975 RID: 2421
		private static float FROG_START_SCALE = 0.26f;
	}
}
