using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x0200007F RID: 127
	public class Skeleton : IDisposable
	{
		// Token: 0x060008AE RID: 2222 RVA: 0x0004D2C8 File Offset: 0x0004B4C8
		public Skeleton()
		{
			this.mAlpha = 0f;
			this.mIncAlpha = true;
			this.mActivated = false;
			this.mEffectDone = false;
			this.mFadeOut = false;
			this.mFadeAlpha = 255f;
			this.mOrbSize = 1f;
			this.mOrbSizeDec = 0f;
			this.mRibCel = 0;
			this.mHeadYOff = 0f;
			this.mHeadVY = 0f;
			this.mHeadBounceCount = 0;
			this.mUpdateCount = 0;
			this.mExplosionCel = 0;
			this.mRings[0] = (this.mRings[1] = null);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0004D375 File Offset: 0x0004B575
		public virtual void Dispose()
		{
			if (this.mRings[0] != null)
			{
				this.mRings[0].Dispose();
			}
			if (this.mRings[1] != null)
			{
				this.mRings[1].Dispose();
			}
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0004D3A8 File Offset: 0x0004B5A8
		public void Update()
		{
			if (this.mDelay > 0)
			{
				this.mDelay--;
				return;
			}
			this.mUpdateCount++;
			this.mX += this.mVX;
			this.mY += this.mVY;
			if (this.mHasPowerup)
			{
				int num = (int)Common._M(14f);
				if (this.mIncAlpha)
				{
					this.mAlpha += (float)num;
					if (this.mAlpha >= 255f)
					{
						this.mAlpha = 255f;
						this.mIncAlpha = false;
					}
				}
				else
				{
					this.mAlpha -= (float)num;
					if (this.mAlpha <= 0f)
					{
						this.mAlpha = 0f;
						this.mIncAlpha = true;
					}
				}
				if (this.mActivated)
				{
					if (this.mUpdateCount % Common._M(4) == 0)
					{
						this.mRibCel++;
					}
					if (this.mHeadBounceCount < Common._M(5))
					{
						float num2 = (float)Common._M(25);
						float num3 = num2 - (float)Common._M(10);
						if (this.mHeadBounceCount % 2 == 0)
						{
							this.mHeadYOff += this.mHeadVY;
							if (this.mHeadYOff >= num2)
							{
								this.mHeadYOff = num2;
								this.mHeadBounceCount++;
							}
						}
						else
						{
							this.mHeadYOff -= this.mHeadVY;
							if (this.mHeadYOff <= num3)
							{
								this.mHeadYOff = num3;
								this.mHeadBounceCount++;
							}
						}
					}
				}
			}
			else if (this.mActivated)
			{
				if (this.mUpdateCount % Common._M(2) == 0)
				{
					this.mExplosionCel++;
				}
				if (this.mExplosionCel == Common._M(3))
				{
					this.mFadeOut = true;
				}
				Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_MINI_SKELE_EXPLODE);
				if (this.mExplosionCel >= imageByID.mNumCols * imageByID.mNumRows)
				{
					this.mEffectDone = true;
				}
			}
			if (this.mRings[0] != null)
			{
				if (this.mOrbSize > 0f)
				{
					this.mOrbSize -= this.mOrbSizeDec;
					if (this.mOrbSize < 0f)
					{
						this.mOrbSize = 0f;
					}
				}
				this.mRings[0].Update();
				this.mRings[1].Update();
				if (this.mRings[0].IsDone() && this.mRings[1].IsDone())
				{
					this.mEffectDone = true;
				}
				else if (!this.mRings[0].IsExpanding())
				{
					this.mFadeOut = true;
				}
			}
			if (this.mFadeOut)
			{
				this.mFadeAlpha -= 255f / Common._M(15f);
				if (this.mFadeAlpha < 0f)
				{
					this.mFadeAlpha = 0f;
				}
			}
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0004D67C File Offset: 0x0004B87C
		public void DoHit()
		{
			float max_radius = Common._M(30f);
			float alpha_fade = 255f / Common._M(20f);
			float size_fade = 1f / Common._M(50f);
			float angle_inc = Common._M(0.2f);
			this.mRings[0] = new OrbPowerRing(0f, max_radius, alpha_fade, size_fade, angle_inc);
			this.mRings[1] = new OrbPowerRing(3.14159f, max_radius, alpha_fade, size_fade, angle_inc);
			this.mOrbSizeDec = 1f / Common._M(50f);
			this.mHeadVY = Common._M(2f);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0004D713 File Offset: 0x0004B913
		public void SetupFade(Graphics g)
		{
			if (this.mFadeOut)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)this.mFadeAlpha);
			}
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0004D740 File Offset: 0x0004B940
		public void Draw(Graphics g)
		{
			if (this.mDelay > 0 || (this.mFadeOut && this.mFadeAlpha <= 0f))
			{
				return;
			}
			this.SetupFade(g);
			if (!this.mHasPowerup)
			{
				g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_MINI_SKELETON), (int)Common._S(this.mX), (int)Common._S(this.mY));
			}
			g.SetColorizeImages(false);
			if (this.mHasPowerup)
			{
				this.SetupFade(g);
				if (this.mRibCel < Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_MINI_SKELE_RIBS).mNumCols)
				{
					g.DrawImageCel(Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_MINI_SKELE_RIBS), (int)Common._S(this.mX + (float)Common._M(1)), (int)Common._S(this.mY + (float)Common._M1(34)), this.mRibCel);
				}
				Image theImage = (this.mActivated ? Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_MINI_SKELE_HEAD_CLOSED) : Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_MINI_SKELE_HEAD));
				g.DrawImage(theImage, (int)Common._S(this.mX + (float)Common._M(0)), (int)Common._S(this.mY + (float)Common._M1(0) + this.mHeadYOff));
				g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_MINI_SKELE_JAW), (int)Common._S(this.mX + (float)Common._M(34)), (int)Common._S(this.mY + (float)Common._M1(82)));
				Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_GLOWBALL);
				float num = (float)imageByID.GetCelWidth() * this.mOrbSize;
				float num2 = (float)imageByID.GetCelHeight() * this.mOrbSize;
				g.DrawImage(imageByID, (int)(Common._S(this.mX + (float)Common._M(28)) + (float)(imageByID.GetCelWidth() / 2) - num / 2f), (int)(Common._S(this.mY + (float)Common._M1(40)) + (float)(imageByID.GetCelHeight() / 2) - num2 / 2f), (int)num, (int)num2);
				g.SetColorizeImages(true);
				g.SetDrawMode(1);
				g.SetColor(255, 255, 255, (int)((this.mFadeAlpha < this.mAlpha) ? this.mFadeAlpha : this.mAlpha));
				g.DrawImage(imageByID, (int)(Common._S(this.mX + (float)Common._M(28)) + (float)(imageByID.GetCelWidth() / 2) - num / 2f), (int)(Common._S(this.mY + (float)Common._M1(40)) + (float)(imageByID.GetCelHeight() / 2) - num2 / 2f), (int)num, (int)num2);
				g.SetColorizeImages(false);
				g.SetDrawMode(0);
				this.SetupFade(g);
				Image imageByID2 = Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_MINI_SKELE_RIBS_SHADOW);
				if (this.mRibCel < imageByID2.mNumCols)
				{
					g.DrawImageCel(imageByID2, (int)Common._S(this.mX + (float)Common._M(1)), (int)Common._S(this.mY + (float)Common._M1(34)), this.mRibCel);
				}
				theImage = (this.mActivated ? Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_MINI_SKELE_HEAD_CLOSED) : Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_MINI_SKELE_HEAD_SHADOW));
				g.DrawImage(theImage, (int)Common._S(this.mX + (float)Common._M(0)), (int)Common._S(this.mY + (float)Common._M1(0) + this.mHeadYOff));
				g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_MINI_SKELE_JAW_SHADOW), (int)Common._S(this.mX + (float)Common._M(34)), (int)Common._S(this.mY + (float)Common._M1(82)));
				g.SetColorizeImages(false);
			}
			this.SetupFade(g);
			if (!this.mHasPowerup)
			{
				g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_MINI_SKELETON_NOSHADOW), (int)Common._S(this.mX), (int)Common._S(this.mY));
			}
			g.SetColorizeImages(false);
			Image imageByID3 = Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_MINI_SKELE_EXPLODE);
			if (this.mActivated && !this.mHasPowerup && this.mExplosionCel < imageByID3.mNumCols * imageByID3.mNumRows)
			{
				Rect celRect = imageByID3.GetCelRect(this.mExplosionCel);
				int theWidth = celRect.mWidth * 4;
				int theHeight = celRect.mHeight * 4;
				g.DrawImage(imageByID3, new Rect((int)Common._S(this.mX + (float)Common._M(-50)), (int)Common._S(this.mY + (float)Common._M1(-50)), theWidth, theHeight), celRect);
			}
			if (this.mRings[0] != null)
			{
				Image imageByID4 = Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_MINI_SKELETON);
				for (int i = 0; i < 2; i++)
				{
					this.mRings[i].Draw(g, Common._S(this.mX) + (float)(imageByID4.GetCelWidth() / 2), Common._S(this.mY) + (float)(imageByID4.GetCelHeight() / 2));
				}
			}
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0004DBF0 File Offset: 0x0004BDF0
		public void SyncState(DataSync sync)
		{
			sync.SyncBoolean(ref this.mHasPowerup);
			sync.SyncFloat(ref this.mVX);
			sync.SyncFloat(ref this.mVY);
			sync.SyncFloat(ref this.mX);
			sync.SyncFloat(ref this.mY);
			sync.SyncLong(ref this.mDelay);
			sync.SyncFloat(ref this.mOrbSize);
			sync.SyncFloat(ref this.mOrbSizeDec);
			sync.SyncFloat(ref this.mFadeAlpha);
			sync.SyncBoolean(ref this.mFadeOut);
			sync.SyncLong(ref this.mRibCel);
			sync.SyncFloat(ref this.mHeadYOff);
			sync.SyncFloat(ref this.mHeadVY);
			sync.SyncLong(ref this.mHeadBounceCount);
			sync.SyncLong(ref this.mUpdateCount);
			sync.SyncLong(ref this.mExplosionCel);
			sync.SyncBoolean(ref this.mActivated);
			SexyBuffer buffer = sync.GetBuffer();
			if (sync.isRead())
			{
				if (buffer.ReadBoolean())
				{
					this.mRings[0] = new OrbPowerRing(0f, 0f, 0f, 0f, 0f);
					this.mRings[1] = new OrbPowerRing(0f, 0f, 0f, 0f, 0f);
					for (int i = 0; i < 2; i++)
					{
						this.mRings[i].SyncState(sync);
					}
					return;
				}
			}
			else
			{
				if (this.mRings[0] == null)
				{
					buffer.WriteBoolean(false);
					return;
				}
				buffer.WriteBoolean(true);
				for (int j = 0; j < 2; j++)
				{
					this.mRings[j].SyncState(sync);
				}
			}
		}

		// Token: 0x040006A2 RID: 1698
		public bool mHasPowerup;

		// Token: 0x040006A3 RID: 1699
		public float mVX;

		// Token: 0x040006A4 RID: 1700
		public float mVY;

		// Token: 0x040006A5 RID: 1701
		public float mX;

		// Token: 0x040006A6 RID: 1702
		public float mY;

		// Token: 0x040006A7 RID: 1703
		public int mDelay;

		// Token: 0x040006A8 RID: 1704
		public float mOrbSize;

		// Token: 0x040006A9 RID: 1705
		public float mOrbSizeDec;

		// Token: 0x040006AA RID: 1706
		public float mAlpha;

		// Token: 0x040006AB RID: 1707
		public float mFadeAlpha;

		// Token: 0x040006AC RID: 1708
		public bool mIncAlpha;

		// Token: 0x040006AD RID: 1709
		public bool mActivated;

		// Token: 0x040006AE RID: 1710
		public bool mEffectDone;

		// Token: 0x040006AF RID: 1711
		public bool mFadeOut;

		// Token: 0x040006B0 RID: 1712
		public int mRibCel;

		// Token: 0x040006B1 RID: 1713
		public float mHeadYOff;

		// Token: 0x040006B2 RID: 1714
		public float mHeadVY;

		// Token: 0x040006B3 RID: 1715
		public int mHeadBounceCount;

		// Token: 0x040006B4 RID: 1716
		public int mUpdateCount;

		// Token: 0x040006B5 RID: 1717
		public int mExplosionCel;

		// Token: 0x040006B6 RID: 1718
		public OrbPowerRing[] mRings = new OrbPowerRing[2];
	}
}
