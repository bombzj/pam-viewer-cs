using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.PIL;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x0200003E RID: 62
	public class HulaDancer
	{
		// Token: 0x06000620 RID: 1568 RVA: 0x00027978 File Offset: 0x00025B78
		public HulaDancer()
		{
			this.mHasProjectile = false;
			this.mX = 0f;
			this.mY = 0f;
			this.mProjX = 0f;
			this.mProjY = 0f;
			this.mProjVY = 0f;
			this.mFireProjectile = false;
			this.mProjectileDestroyed = false;
			this.mCel = 0;
			this.mUpdateCount = 0;
			this.mSystem = null;
			this.mFadeOut = false;
			this.mFadeAlpha = 255f;
			this.mDeathEffect = null;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00027A1C File Offset: 0x00025C1C
		public HulaDancer(HulaDancer rhs)
		{
			this.mHasProjectile = rhs.mHasProjectile;
			this.mX = rhs.mX;
			this.mY = rhs.mY;
			this.mProjX = rhs.mProjX;
			this.mProjY = rhs.mProjY;
			this.mProjVY = rhs.mProjVY;
			this.mFireProjectile = rhs.mFireProjectile;
			this.mProjectileDestroyed = rhs.mProjectileDestroyed;
			this.mCel = rhs.mCel;
			this.mUpdateCount = rhs.mUpdateCount;
			this.mSystem = rhs.mSystem;
			this.mFadeOut = rhs.mFadeOut;
			this.mFadeAlpha = rhs.mFadeAlpha;
			this.mDeathEffect = rhs.mDeathEffect;
			this.mRect = new Rect(rhs.mRect);
			this.mImage = rhs.mImage;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00027B0B File Offset: 0x00025D0B
		public virtual void Dispose()
		{
			if (this.mSystem != null)
			{
				this.mSystem.Dispose();
				this.mSystem = null;
			}
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00027B28 File Offset: 0x00025D28
		public void Setup(bool has_proj, float y, float proj_vy)
		{
			this.mHasProjectile = has_proj;
			if (this.mHasProjectile)
			{
				this.mCel = 3;
			}
			this.mY = y;
			this.mProjVY = proj_vy;
			this.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_STONEHEAD_HULA_GIRL);
			this.mRect = new Rect((int)this.mX, (int)this.mY, Common._SS(this.mImage.GetCelWidth()), Common._SS(this.mImage.GetCelHeight()));
			this.mRect.Inflate(Common._M(-5), Common._M1(-5));
			this.mX = (float)(-(float)this.mImage.GetCelWidth());
			this.mSystem = new SexyFramework.PIL.System(100, 50);
			this.mSystem.mScale = Common._S(1f);
			this.mSystem.WaitForEmitters(true);
			Emitter emitter = new Emitter();
			emitter.mPreloadFrames = Common._M(200);
			emitter.mCullingRect = new Rect(-100, -100, Common._SS(GameApp.gApp.mWidth) + 200, Common._SS(GameApp.gApp.mHeight) + 200);
			emitter.mEmissionCoordsAreOffsets = true;
			emitter.SetEmitterType(2);
			emitter.AddScaleKeyFrame(0, new EmitterScale
			{
				mLifeScale = Common._M(1f),
				mNumberScale = Common._M(1f),
				mSizeXScale = Common._M(1.27f),
				mSizeYScale = Common._M(0.9f),
				mVelocityScale = Common._M(1f),
				mWeightScale = Common._M(0.48f),
				mZoom = Common._M(1.66f)
			});
			emitter.AddSettingsKeyFrame(0, new EmitterSettings
			{
				mVisibility = Common._M(0.51f),
				mXRadius = (float)Common._M(10),
				mYRadius = (float)Common._M(12)
			});
			ParticleType particleType = new ParticleType();
			particleType.mLockSizeAspect = false;
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_FUZZY_CIRCLE);
			particleType.mColorKeyManager.AddColorKey(0f, new SexyColor(16, 255, 0));
			particleType.mColorKeyManager.AddColorKey(0.5f, new SexyColor(255, 233, 0));
			particleType.mColorKeyManager.AddColorKey(1f, new SexyColor(0, 255, 42));
			particleType.mAdditive = true;
			particleType.mEmitterAttachPct = Common._M(1f);
			ParticleSettings particleSettings = new ParticleSettings();
			particleSettings.mLife = Common._M(10);
			particleSettings.mNumber = Common._M(40);
			particleSettings.mXSize = (particleSettings.mYSize = Common._M(13));
			particleSettings.mVelocity = Common._M(5);
			particleSettings.mWeight = (float)Common._M(0);
			particleSettings.mGlobalVisibility = Common._M(1f);
			particleType.AddSettingsKeyFrame(0, particleSettings);
			ParticleVariance particleVariance = new ParticleVariance();
			particleVariance.mLifeVar = Common._M(5);
			particleVariance.mNumberVar = Common._M(2);
			particleVariance.mSizeXVar = (particleVariance.mSizeYVar = Common._M(3));
			particleVariance.mVelocityVar = Common._M(5);
			particleType.AddVarianceKeyFrame(0, particleVariance);
			LifetimeSettings lifetimeSettings = new LifetimeSettings();
			lifetimeSettings.mSizeXMult = (lifetimeSettings.mSizeYMult = Common._M(0.2f));
			particleType.AddSettingAtLifePct(0f, lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			lifetimeSettings.mSizeXMult = (lifetimeSettings.mSizeYMult = Common._M(1.9f));
			particleType.AddSettingAtLifePct(0.75f, lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			lifetimeSettings.mSizeXMult = (lifetimeSettings.mSizeYMult = 0f);
			particleType.AddSettingAtLifePct(1f, lifetimeSettings);
			emitter.AddParticleType(particleType);
			this.mSystem.AddEmitter(emitter);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00027F12 File Offset: 0x00026112
		public void Setup(bool has_proj, float y)
		{
			this.Setup(has_proj, y, 0f);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00027F24 File Offset: 0x00026124
		public bool CanRemove()
		{
			if (this.mDeathEffect != null)
			{
				return false;
			}
			if (this.mFadeAlpha <= 0f)
			{
				return true;
			}
			if ((!this.mHasProjectile && !this.mFireProjectile) || this.mProjectileDestroyed)
			{
				return this.mX > (float)(Common._SS(GameApp.gApp.mWidth) + 80);
			}
			return this.mX > (float)(Common._SS(GameApp.gApp.mWidth) + 80) && this.mProjY > (float)(Common._SS(GameApp.gApp.mHeight) + Common._M(100));
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00027FBC File Offset: 0x000261BC
		public void Update(float vx)
		{
			if (this.mFadeOut)
			{
				this.mFadeAlpha -= Common._M(2f);
			}
			this.mUpdateCount++;
			if (this.mFireProjectile && !this.mProjectileDestroyed && !this.mFadeOut)
			{
				this.mProjY += this.mProjVY;
			}
			if (this.mSystem != null)
			{
				this.mSystem.Update();
				if (this.mFireProjectile)
				{
					this.mSystem.SetPos(this.mProjX + (float)Common._M(10), this.mProjY + (float)Common._M1(10));
				}
				else
				{
					this.mSystem.SetPos(this.mX + (float)Common._M(50), this.mY + (float)Common._M1(10));
				}
			}
			if (this.mUpdateCount % Common._M(5) == 0)
			{
				if (!this.mHasProjectile)
				{
					this.mCel = (this.mCel + 1) % 3;
				}
				else if (++this.mCel >= this.mImage.mNumCols)
				{
					this.mCel = 3;
				}
			}
			if (this.mDeathEffect != null)
			{
				this.mGlobalTranform.Reset();
				this.mGlobalTranform.Translate(Common._S(this.mX + (float)Common._M(0)), Common._S(this.mY + (float)Common._M1(0)));
				this.mDeathEffect.SetTransform(this.mGlobalTranform.GetMatrix());
				this.mDeathEffect.Update();
				if (!this.mDeathEffect.IsActive())
				{
					this.mDeathEffect = null;
					this.mX = 9999999f;
				}
			}
			this.mX += vx;
			this.mRect.mX = (int)this.mX;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00028184 File Offset: 0x00026384
		public void Draw(Graphics g)
		{
			if (this.mFadeAlpha <= 0f)
			{
				return;
			}
			if (this.mFadeAlpha != 255f)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)this.mFadeAlpha);
			}
			if (g.Is3D())
			{
				if (this.mDeathEffect == null)
				{
					g.DrawImageCel(this.mImage, (int)Common._S(this.mX), (int)Common._S(this.mY), this.mCel);
				}
				else
				{
					this.mDeathEffect.Draw(g);
				}
				if (this.mFireProjectile && !this.mProjectileDestroyed && !GameApp.gApp.GetBoard().IsPaused())
				{
					g.DrawImageF(Res.GetImageByID(ResID.IMAGE_BOSS_STONEHEAD_COCONUT), (float)((int)Common._S(this.mProjX)), (float)((int)Common._S(this.mProjY)));
				}
			}
			else
			{
				if (this.mDeathEffect == null)
				{
					g.DrawImageCel(this.mImage, (int)Common._S(this.mX), (int)Common._S(this.mY), this.mCel);
				}
				else
				{
					this.mDeathEffect.Draw(g);
				}
				if (this.mFireProjectile && !this.mProjectileDestroyed && !GameApp.gApp.GetBoard().IsPaused())
				{
					g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_STONEHEAD_COCONUT), (int)Common._S(this.mProjX), (int)Common._S(this.mProjY));
				}
			}
			if (this.mSystem != null && !GameApp.gApp.GetBoard().IsPaused())
			{
				this.mSystem.mAlphaPct = this.mFadeAlpha / 255f;
				this.mSystem.Draw(g);
			}
			g.SetColorizeImages(false);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0002833F File Offset: 0x0002653F
		public bool Collided(Rect r)
		{
			return r.Intersects(this.mRect) && !this.mFadeOut && this.mDeathEffect == null;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00028364 File Offset: 0x00026564
		public bool ProjectileCollided(Rect gun_rect)
		{
			if (!this.mFireProjectile || this.mProjectileDestroyed)
			{
				return false;
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_STONEHEAD_COCONUT);
			bool flag = gun_rect.Intersects(new Rect((int)this.mProjX, (int)this.mProjY, Common._SS(imageByID.mWidth), Common._SS(imageByID.mHeight)));
			if (flag && this.mSystem != null)
			{
				this.mSystem.ForceStopEmitting(true);
			}
			return flag;
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x000283D7 File Offset: 0x000265D7
		public bool HasFired()
		{
			return this.mFireProjectile;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x000283E0 File Offset: 0x000265E0
		public void Fire()
		{
			if (this.mFireProjectile || !this.mHasProjectile || this.mProjectileDestroyed || this.mDeathEffect != null)
			{
				return;
			}
			GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BOSS_STONE_COCONUT_DROP));
			this.mHasProjectile = false;
			this.mCel -= 3;
			this.mFireProjectile = true;
			this.mProjX = this.mX + (float)Common._M(40);
			this.mProjY = this.mY + (float)Common._M(0);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00028468 File Offset: 0x00026668
		public void SyncState(DataSync sync)
		{
			sync.SyncBoolean(ref this.mFadeOut);
			sync.SyncFloat(ref this.mFadeAlpha);
			sync.SyncFloat(ref this.mX);
			sync.SyncFloat(ref this.mY);
			sync.SyncFloat(ref this.mProjX);
			sync.SyncFloat(ref this.mProjY);
			sync.SyncFloat(ref this.mProjVY);
			sync.SyncBoolean(ref this.mFireProjectile);
			sync.SyncBoolean(ref this.mHasProjectile);
			sync.SyncBoolean(ref this.mProjectileDestroyed);
			sync.SyncLong(ref this.mCel);
			sync.SyncLong(ref this.mUpdateCount);
			if (sync.isRead())
			{
				this.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_STONEHEAD_HULA_GIRL);
				this.mRect = new Rect((int)this.mX, (int)this.mY, this.mImage.GetCelWidth(), this.mImage.GetCelWidth());
				this.mRect.Inflate(Common._M(-5), Common._M1(-5));
			}
			SexyBuffer buffer = sync.GetBuffer();
			if (sync.isWrite())
			{
				Common.SerializeParticleSystem(this.mSystem, sync);
				buffer.WriteBoolean(this.mDeathEffect != null);
				if (this.mDeathEffect != null)
				{
					buffer.WriteLong((long)((int)this.mDeathEffect.mMainSpriteInst.mFrameNum));
					return;
				}
			}
			else
			{
				this.mSystem = Common.DeserializeParticleSystem(sync);
				if (buffer.ReadBoolean())
				{
					this.mDeathEffect = Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_HULAGIRLDEATH);
					this.mDeathEffect.ResetAnim();
					this.mDeathEffect.Play((int)buffer.ReadLong());
				}
			}
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x000285F8 File Offset: 0x000267F8
		public void Disable()
		{
			if (this.mDeathEffect == null)
			{
				this.mDeathEffect = Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_HULAGIRLDEATH);
				this.mDeathEffect.ResetAnim();
				this.mDeathEffect.Play("Main");
				if (!this.mFireProjectile)
				{
					this.mFadeOut = true;
				}
			}
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00028648 File Offset: 0x00026848
		public float GetX()
		{
			return this.mX;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00028650 File Offset: 0x00026850
		public void DestroyBullet()
		{
			this.mProjectileDestroyed = true;
		}

		// Token: 0x04000327 RID: 807
		protected bool mHasProjectile;

		// Token: 0x04000328 RID: 808
		protected bool mFireProjectile;

		// Token: 0x04000329 RID: 809
		protected bool mProjectileDestroyed;

		// Token: 0x0400032A RID: 810
		protected float mX;

		// Token: 0x0400032B RID: 811
		protected float mY;

		// Token: 0x0400032C RID: 812
		protected float mProjX;

		// Token: 0x0400032D RID: 813
		protected float mProjY;

		// Token: 0x0400032E RID: 814
		protected float mProjVY;

		// Token: 0x0400032F RID: 815
		protected float mFadeAlpha;

		// Token: 0x04000330 RID: 816
		protected int mCel;

		// Token: 0x04000331 RID: 817
		protected int mUpdateCount;

		// Token: 0x04000332 RID: 818
		protected Rect mRect = default(Rect);

		// Token: 0x04000333 RID: 819
		protected Image mImage;

		// Token: 0x04000334 RID: 820
		protected SexyFramework.PIL.System mSystem;

		// Token: 0x04000335 RID: 821
		protected PopAnim mDeathEffect;

		// Token: 0x04000336 RID: 822
		protected Transform mGlobalTranform = new Transform();

		// Token: 0x04000337 RID: 823
		public bool mFadeOut;
	}
}
