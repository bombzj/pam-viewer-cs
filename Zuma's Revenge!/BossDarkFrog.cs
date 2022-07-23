using System;
using System.Collections.Generic;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x0200001C RID: 28
	public class BossDarkFrog : BossShoot
	{
		// Token: 0x06000527 RID: 1319 RVA: 0x000160A2 File Offset: 0x000142A2
		private static bool AnimPlaying(PopAnim p)
		{
			return p.mMainSpriteInst.mDef != null && p.mMainSpriteInst.mFrameNum < (float)(p.mMainSpriteInst.mDef.mFrames.Count - 1);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x000160D8 File Offset: 0x000142D8
		protected override void DidFire()
		{
			this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BOSS_DARK_FROG_FIRES));
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x000160F0 File Offset: 0x000142F0
		protected override bool DoHit(Bullet b, bool from_prox_bomb)
		{
			bool flag = base.DoHit(b, from_prox_bomb);
			if (flag)
			{
				this.mHitAnim.ResetAnim();
				this.mHitAnim.Play("MAIN");
				this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BOSS_DARK_FROG_HIT));
				if (this.mHP <= 0f)
				{
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BOSS_DARK_FROG_DIES));
					this.mLevel.mFrog.ClearStun();
				}
			}
			return flag;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00016170 File Offset: 0x00014370
		protected override void DrawBossSpecificArt(Graphics g)
		{
			if (g.Is3D())
			{
				this.mDeathAura.mColor.mAlpha = (int)this.mAlphaOverride;
				this.mDeathAura.DrawLayer(g, this.mDeathAura.GetLayer("lower"));
			}
			int num = (int)this.mX * 2 - BossDarkFrog.CANVAS_W / 2;
			int num2 = (int)this.mY * 2 - BossDarkFrog.CANVAS_H / 2;
			int num3 = (int)Math.Min(this.mBodyAlpha, this.mAlphaOverride);
			g.PushState();
			g.SetColor(255, 255, 255, num3);
			if (num3 < 255)
			{
				g.SetColorizeImages(true);
			}
			if (!BossDarkFrog.AnimPlaying(this.mHitAnim))
			{
				g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_BACK), Common._DS(num + Res.GetOffsetXByID(ResID.IMAGE_BOSS_DARKFROG_BACK)), (int)(Common._S(this.mRecoilY) + (float)Common._DS(num2 + Res.GetOffsetYByID(ResID.IMAGE_BOSS_DARKFROG_BACK))));
			}
			g.PopState();
			if (this.mAlpha > 0f && !BossDarkFrog.AnimPlaying(this.mHitAnim))
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)Math.Min(this.mAlpha, this.mAlphaOverride));
				g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_TAT2), Common._DS(num + Res.GetOffsetXByID(ResID.IMAGE_BOSS_DARKFROG_TAT2)), (int)((float)Common._DS(num2 + Res.GetOffsetYByID(ResID.IMAGE_BOSS_DARKFROG_TAT2)) + Common._S(this.mRecoilY)));
				g.SetColorizeImages(false);
			}
			g.PushState();
			g.SetColor(255, 255, 255, num3);
			if (num3 < 255)
			{
				g.SetColorizeImages(true);
			}
			if (!BossDarkFrog.AnimPlaying(this.mHitAnim))
			{
				g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_TONGUE), Common._DS(num + Res.GetOffsetXByID(ResID.IMAGE_BOSS_DARKFROG_TONGUE)), (int)((float)Common._DS(num2 + Res.GetOffsetYByID(ResID.IMAGE_BOSS_DARKFROG_TONGUE)) + Common._S(this.mTongueYOff + this.mRecoilY)));
			}
			g.PopState();
			if (this.mHP > 0f && !this.mDoDeathExplosions && !this.mLevel.mBoard.IsPaused())
			{
				Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_LB_HALO);
				Image imageByID2 = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_LB_TWIRLLIGHT);
				Image imageByID3 = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_LAVABALL_BOTTOM_ADDITIVE);
				Image imageByID4 = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_LAVABALL_TOP_NORMAL);
				for (int i = 0; i < this.mBulletFX.Count; i++)
				{
					DarkFrogBulletFX darkFrogBulletFX = this.mBulletFX[i];
					if (!darkFrogBulletFX.mExploding)
					{
						g.PushState();
						darkFrogBulletFX.mBallEffect.mColor.mAlpha = (int)this.mAlphaOverride;
						darkFrogBulletFX.mBallEffect.Draw(g);
						g.PopState();
						g.PushState();
						if (!SexyFramework.Common._eq(this.mAlphaOverride, 255f))
						{
							g.SetColorizeImages(true);
							g.SetColor(255, 255, 255, (int)this.mAlphaOverride);
						}
						g.SetDrawMode(1);
						g.DrawImage(imageByID, (int)(darkFrogBulletFX.mX - (float)(imageByID.mWidth / 2)), (int)(darkFrogBulletFX.mY - (float)(imageByID.mHeight / 2)));
						g.DrawImageRotated(imageByID2, (int)(darkFrogBulletFX.mX - (float)(imageByID2.mWidth / 2)), (int)(darkFrogBulletFX.mY - (float)(imageByID2.mHeight / 2)), (double)darkFrogBulletFX.mTwirlAngle);
						g.DrawImage(imageByID3, (int)(darkFrogBulletFX.mX - (float)(imageByID3.mWidth / 2)), (int)(darkFrogBulletFX.mY - (float)(imageByID3.mHeight / 2)));
						g.SetDrawMode(0);
						g.DrawImage(imageByID4, (int)(darkFrogBulletFX.mX - (float)(imageByID4.mWidth / 2)), (int)(darkFrogBulletFX.mY - (float)(imageByID4.mHeight / 2)));
						g.PopState();
					}
					if (darkFrogBulletFX.mExploding)
					{
						g.PushState();
						darkFrogBulletFX.mBallExplosion.mColor.mAlpha = (int)this.mAlphaOverride;
						darkFrogBulletFX.mBallExplosion.Draw(g);
						g.PopState();
					}
				}
			}
			g.PushState();
			g.SetColor(255, 255, 255, num3);
			if (num3 < 255)
			{
				g.SetColorizeImages(true);
			}
			if (!BossDarkFrog.AnimPlaying(this.mHitAnim))
			{
				g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_TOP), Common._DS(num + Res.GetOffsetXByID(ResID.IMAGE_BOSS_DARKFROG_TOP)), (int)(Common._S(this.mRecoilY) + (float)Common._DS(num2 + Res.GetOffsetYByID(ResID.IMAGE_BOSS_DARKFROG_TOP))));
			}
			if (this.mBlinkFrame != -1 && !BossDarkFrog.AnimPlaying(this.mHitAnim))
			{
				ResID id = ((this.mBlinkFrame == 0) ? ResID.IMAGE_BOSS_DARKFROG_BLINK2 : ResID.IMAGE_BOSS_DARKFROG_BLINK1);
				Image imageByID5 = Res.GetImageByID(id);
				g.DrawImage(imageByID5, Common._DS(num + Res.GetOffsetXByID(id)), (int)((float)Common._DS(num2 + Res.GetOffsetYByID(id)) + Common._S(this.mRecoilY)));
			}
			g.PopState();
			if (this.mAlpha > 0f && !BossDarkFrog.AnimPlaying(this.mHitAnim))
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)Math.Min(this.mAlpha, this.mAlphaOverride));
				g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_TAT1), Common._DS(num + Res.GetOffsetXByID(ResID.IMAGE_BOSS_DARKFROG_TAT1)), (int)((float)Common._DS(num2 + Res.GetOffsetYByID(ResID.IMAGE_BOSS_DARKFROG_TAT1)) + Common._S(this.mRecoilY)));
				g.SetColorizeImages(false);
			}
			if (BossDarkFrog.AnimPlaying(this.mHitAnim))
			{
				this.mHitAnim.mColor.mAlpha = (int)this.mAlphaOverride;
				this.mHitAnim.Draw(g);
			}
			if (g.Is3D())
			{
				this.mDeathAura.mColor.mAlpha = (int)this.mAlphaOverride;
				this.mDeathAura.DrawLayer(g, this.mDeathAura.GetLayer("upper"));
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00016778 File Offset: 0x00014978
		protected override bool PreBulletUpdate(BossBullet b, int index)
		{
			if (this.mFiringState != 0 && b.mState < 2)
			{
				return true;
			}
			if (b.mDelay > 0 && b.mState == 0)
			{
				b.mDelay--;
				return true;
			}
			if (b.mState == 0)
			{
				b.mState = 1;
				this.mFiringState = 1;
				this.mTongueYOff = 0f;
				this.mTimer = 0;
				b.mX = (b.mY = 0f);
			}
			return b.mState < 2;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00016804 File Offset: 0x00014A04
		protected override void BulletHitPlayer(BossBullet b)
		{
			SoundAttribs soundAttribs = new SoundAttribs();
			soundAttribs.fadeout = 0.1f;
			this.mApp.mSoundPlayer.Loop(Res.GetSoundByID(ResID.SOUND_NEW_BURNINGFROGLOOP), soundAttribs);
			this.mApp.mSoundPlayer.Play(Res.GetSoundByID(ResID.SOUND_NEW_FIREHITFROG));
			for (int i = 0; i < this.mBulletFX.Count; i++)
			{
				DarkFrogBulletFX darkFrogBulletFX = this.mBulletFX[i];
				if (darkFrogBulletFX.mBulletId == b.mId)
				{
					darkFrogBulletFX.mExploding = false;
					darkFrogBulletFX.mBallExplosion = this.GetFreeBallExplosion(this.mApp.Is3DAccelerated());
					darkFrogBulletFX.mBallExplosion.mDrawTransform.LoadIdentity();
					float num = GameApp.DownScaleNum(1f);
					darkFrogBulletFX.mBallExplosion.mDrawTransform.Scale(num, num);
					darkFrogBulletFX.mBallExplosion.mDrawTransform.Translate((float)Common._S(this.mLevel.mBoard.GetGun().GetCenterX() + Common._M(0)), (float)Common._S(this.mLevel.mBoard.GetGun().GetCenterY() + Common._M1(0)));
				}
			}
			if (!this.mApp.GetLevelMgr().mBossesCanAttackFuckedFrog)
			{
				for (int j = 0; j < this.mBullets.Count; j++)
				{
					BossBullet bossBullet = this.mBullets[j];
					if (bossBullet.mState < 2)
					{
						bossBullet.mDeleteInstantly = true;
					}
				}
			}
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00016980 File Offset: 0x00014B80
		protected override void BossBulletDestroyed(BossBullet b, bool outofscreen)
		{
			for (int i = 0; i < this.mBulletFX.Count; i++)
			{
				DarkFrogBulletFX darkFrogBulletFX = this.mBulletFX[i];
				if (darkFrogBulletFX.mBulletId == b.mId && !darkFrogBulletFX.mExploding)
				{
					this.ReleaseEffects(darkFrogBulletFX);
					this.mBulletFX.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000169DC File Offset: 0x00014BDC
		protected PIEffect GetFreeBallEffect(bool particle_3d)
		{
			int i = 0;
			while (i < this.mFXCache.Count)
			{
				FXCache fxcache = this.mFXCache[i];
				if (!fxcache.mBallEffectInUse)
				{
					fxcache.mBallEffectInUse = true;
					if (!particle_3d)
					{
						return fxcache.mBallEffect2D;
					}
					return fxcache.mBallEffect;
				}
				else
				{
					i++;
				}
			}
			return null;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00016A30 File Offset: 0x00014C30
		protected PIEffect GetFreeBallExplosion(bool particle_3d)
		{
			int i = 0;
			while (i < this.mFXCache.Count)
			{
				FXCache fxcache = this.mFXCache[i];
				if (!fxcache.mBallExplosionInUse)
				{
					fxcache.mBallExplosionInUse = true;
					if (!particle_3d)
					{
						return fxcache.mBallExplosion2D;
					}
					return fxcache.mBallExplosion;
				}
				else
				{
					i++;
				}
			}
			return null;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00016A84 File Offset: 0x00014C84
		protected void ReleaseEffects(DarkFrogBulletFX fx)
		{
			for (int i = 0; i < this.mFXCache.Count; i++)
			{
				FXCache fxcache = this.mFXCache[i];
				if (fx.mBallEffect == fxcache.mBallEffect || fx.mBallEffect == fxcache.mBallEffect2D)
				{
					fx.mBallEffect = null;
					fxcache.mBallEffectInUse = false;
					fxcache.mBallEffect.ResetAnim();
				}
				if (fx.mBallExplosion == fxcache.mBallExplosion || fx.mBallExplosion == fxcache.mBallExplosion2D)
				{
					fx.mBallExplosion = null;
					fxcache.mBallExplosionInUse = false;
					fxcache.mBallExplosion.ResetAnim();
				}
				if (fx.mBallExplosion == null && fx.mBallEffect == null)
				{
					return;
				}
			}
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00016B38 File Offset: 0x00014D38
		public BossDarkFrog(Level l)
			: base(l)
		{
			this.mDrawHeartsBelowBoss = true;
			this.mBossRadius = Common._M(55);
			this.mBulletRadius = Common._M(10);
			this.mResGroup = "Boss6_DarkFrog";
			this.mTongueYOff = (float)Common._M(0);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00016BBF File Offset: 0x00014DBF
		public BossDarkFrog()
			: this(null)
		{
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00016BC8 File Offset: 0x00014DC8
		public override void Dispose()
		{
			base.Dispose();
			for (int i = 0; i < this.mFXCache.Count; i++)
			{
				if (this.mFXCache[i].mBallEffect != null)
				{
					this.mFXCache[i].mBallEffect.Dispose();
					this.mFXCache[i].mBallEffect = null;
				}
				if (this.mFXCache[i].mBallExplosion != null)
				{
					this.mFXCache[i].mBallExplosion.Dispose();
					this.mFXCache[i].mBallExplosion = null;
				}
				if (this.mFXCache[i].mBallEffect2D != null)
				{
					this.mFXCache[i].mBallEffect2D.Dispose();
					this.mFXCache[i].mBallEffect2D = null;
				}
				if (this.mFXCache[i].mBallExplosion2D != null)
				{
					this.mFXCache[i].mBallExplosion2D.Dispose();
					this.mFXCache[i].mBallExplosion2D = null;
				}
			}
			this.mFXCache.Clear();
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00016CF0 File Offset: 0x00014EF0
		public void CopyFrom(BossDarkFrog rhs)
		{
			base.CopyFrom(rhs);
			this.mDeathAura = rhs.mDeathAura;
			this.mHitAnim = rhs.mHitAnim;
			this.mTimer = rhs.mTimer;
			this.mFiringState = rhs.mFiringState;
			this.mBlinkFrame = rhs.mBlinkFrame;
			this.mTongueYOff = rhs.mTongueYOff;
			this.mBlinkForward = rhs.mBlinkForward;
			this.mAlpha = rhs.mAlpha;
			this.mRecoilY = rhs.mRecoilY;
			this.mBodyAlpha = rhs.mBodyAlpha;
			this.mTattooAlpha = rhs.mTattooAlpha;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00016D88 File Offset: 0x00014F88
		public override void Update(float f)
		{
			base.Update(f);
			if (this.mHP <= 0f && !this.mDoDeathExplosions && this.mDeathExplosions.Count == 0 && this.mBodyAlpha > 0f)
			{
				this.mBodyAlpha -= Common._M(1.5f);
				if (this.mBodyAlpha < 0f)
				{
					this.mBodyAlpha = 0f;
				}
				this.mAlpha = 255f - this.mBodyAlpha;
			}
			if (this.mHP <= 0f)
			{
				this.mAlphaOverride = 255f;
			}
			if (BossDarkFrog.AnimPlaying(this.mHitAnim))
			{
				this.mGlobalTranform.Reset();
				this.mGlobalTranform.Translate(Common._S(this.mX + (float)Common._M(-185)), Common._S(this.mY + (float)Common._M(-160)));
				this.mHitAnim.SetTransform(this.mGlobalTranform.GetMatrix());
				this.mHitAnim.Update();
			}
			this.mDeathAura.mDrawTransform.LoadIdentity();
			float num = GameApp.DownScaleNum(1f);
			this.mDeathAura.mDrawTransform.Scale(num, num);
			this.mDeathAura.mDrawTransform.Translate(Common._S(this.mX + (float)Common._M(0)), Common._S(this.mY + (float)Common._M1(0)));
			if (!this.mLevel.mBoard.HasDarkFrogSequence())
			{
				this.mDeathAura.Update();
			}
			for (int i = 0; i < this.mBulletFX.Count; i++)
			{
				PIEffect pieffect = (this.mBulletFX[i].mExploding ? this.mBulletFX[i].mBallExplosion : this.mBulletFX[i].mBallEffect);
				pieffect.Update();
				if (this.mBulletFX[i].mBulletId != -1 && !this.mBulletFX[i].mExploding)
				{
					for (int j = 0; j < this.mBullets.Count; j++)
					{
						if (this.mBullets[j].mId == this.mBulletFX[i].mBulletId)
						{
							this.mBulletFX[i].mX = Common._S(this.mBullets[j].mX + (float)Common._M(0));
							this.mBulletFX[i].mY = Common._S(this.mBullets[j].mY + (float)Common._M(0));
							pieffect.mDrawTransform.LoadIdentity();
							float num2 = GameApp.DownScaleNum(1f);
							pieffect.mDrawTransform.Scale(num2, num2);
							pieffect.mDrawTransform.Translate(Common._S(this.mBullets[j].mX + (float)Common._M(0)), Common._S(this.mBullets[j].mY + (float)Common._M1(0)));
							break;
						}
					}
				}
				else if (this.mBulletFX[i].mExploding)
				{
					pieffect.mDrawTransform.LoadIdentity();
					float num3 = GameApp.DownScaleNum(1f);
					pieffect.mDrawTransform.Scale(num3, num3);
					pieffect.mDrawTransform.Translate((float)Common._S(this.mLevel.mBoard.GetGun().GetCenterX() + Common._M(0)), (float)Common._S(this.mLevel.mBoard.GetGun().GetCenterY() + Common._M1(0)));
				}
				this.mBulletFX[i].mTwirlAngle += Common._M(0.1f);
				if ((this.mBulletFX[i].mExploding && pieffect.mFrameNum >= (float)pieffect.mLastFrameNum) || this.mBulletFX[i].mY > (float)(this.mApp.mHeight + 150))
				{
					this.ReleaseEffects(this.mBulletFX[i]);
					this.mBulletFX.RemoveAt(i);
					i--;
				}
			}
			int num4 = Common._M(14);
			int num5 = (int)Common._M(10f);
			if (this.mFiringState == 1)
			{
				if (this.mAlpha < 255f && this.mHP > 0f)
				{
					this.mAlpha = Math.Min(this.mAlpha + (float)num5, 255f);
				}
				this.mTongueYOff += Common._M(4f);
				if (this.mTongueYOff >= (float)num4)
				{
					this.mTongueYOff = (float)num4;
					this.mFiringState = 2;
					for (int k = 0; k < this.mBullets.Count; k++)
					{
						BossBullet bossBullet = this.mBullets[k];
						if (bossBullet.mState == 1)
						{
							bossBullet.mState = 2;
							bossBullet.mX = this.mX + (float)Common._S(Common._M(0));
							bossBullet.mY = this.mY + (float)Common._S(Common._M(18));
							DarkFrogBulletFX darkFrogBulletFX = new DarkFrogBulletFX();
							darkFrogBulletFX.mBallEffect = this.GetFreeBallEffect(this.mApp.Is3DAccelerated());
							darkFrogBulletFX.mBulletId = bossBullet.mId;
							darkFrogBulletFX.mX = Common._S(bossBullet.mX);
							darkFrogBulletFX.mY = Common._S(bossBullet.mY);
							this.mBulletFX.Add(darkFrogBulletFX);
						}
					}
				}
			}
			else if (this.mFiringState == 2 && ++this.mTimer >= Common._M(50))
			{
				if (this.mAlpha < 255f && this.mHP > 0f)
				{
					this.mAlpha = Math.Min(this.mAlpha + (float)num5, 255f);
				}
				this.mFiringState = 3;
				this.mTimer = 0;
			}
			else if (this.mFiringState == 3)
			{
				this.mTongueYOff -= Common._M(4f);
				if (this.mTongueYOff <= 0f)
				{
					this.mTongueYOff = 0f;
					this.mFiringState = 0;
				}
			}
			if ((this.mFiringState == 3 || this.mFiringState == 0) && this.mAlpha > 0f && this.mHP > 0f)
			{
				this.mAlpha = Math.Max(this.mAlpha - (float)num5, 0f);
			}
			if (this.mBlinkFrame == -1 && this.mUpdateCount % Common._M(500) == 0)
			{
				this.mBlinkFrame = 0;
				this.mBlinkForward = true;
			}
			if (this.mBlinkFrame != -1 && this.mUpdateCount % Common._M(10) == 0)
			{
				if (this.mBlinkForward && ++this.mBlinkFrame >= 2)
				{
					this.mBlinkFrame = 1;
					this.mBlinkForward = false;
					return;
				}
				if (!this.mBlinkForward)
				{
					this.mBlinkFrame--;
				}
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x000174A4 File Offset: 0x000156A4
		public override void Init(Level l)
		{
			for (int i = 0; i < 5; i++)
			{
				FXCache fxcache = new FXCache();
				fxcache.mBallEffect = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_LB_PARTICLES").Duplicate();
				fxcache.mBallEffect.mEmitAfterTimeline = true;
				fxcache.mBallExplosion = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_LB_EXPLOSION").Duplicate();
				fxcache.mBallEffect2D = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_LB_PARTICLES_2D").Duplicate();
				fxcache.mBallEffect2D.mEmitAfterTimeline = true;
				fxcache.mBallExplosion2D = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_LB_EXPLOSION_2D").Duplicate();
				this.mFXCache.Add(fxcache);
			}
			this.mWidth = Common._M(110);
			this.mHeight = Common._M(120);
			base.Init(l);
			this.mHitAnim = Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_SHADOW_DAMAGE);
			this.mDeathAura = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_DEATH_AURA);
			this.mDeathAura.ResetAnim();
			this.mDeathAura.mEmitAfterTimeline = true;
			Common.SetFXNumScale(this.mDeathAura, this.mApp.Is3DAccelerated() ? 1f : Common._M(0.25f));
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x000175F0 File Offset: 0x000157F0
		public override Boss Instantiate()
		{
			BossDarkFrog bossDarkFrog = new BossDarkFrog(this.mLevel);
			bossDarkFrog.CopyFrom(this);
			return bossDarkFrog;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00017614 File Offset: 0x00015814
		public override void SyncState(DataSync sync)
		{
			base.SyncState(sync);
			sync.SyncLong(ref this.mFiringState);
			sync.SyncLong(ref this.mBlinkFrame);
			sync.SyncBoolean(ref this.mBlinkForward);
			sync.SyncFloat(ref this.mAlpha);
			sync.SyncFloat(ref this.mRecoilY);
			sync.SyncFloat(ref this.mTongueYOff);
			sync.SyncFloat(ref this.mTattooAlpha);
			sync.SyncFloat(ref this.mBodyAlpha);
			SexyBuffer buffer = sync.GetBuffer();
			if (sync.isWrite())
			{
				buffer.WriteBoolean(GameApp.gApp.Is3DAccelerated());
				buffer.WriteLong((long)this.mBulletFX.Count);
				for (int i = 0; i < this.mBulletFX.Count; i++)
				{
					DarkFrogBulletFX darkFrogBulletFX = this.mBulletFX[i];
					buffer.WriteLong((long)darkFrogBulletFX.mBulletId);
					buffer.WriteBoolean(darkFrogBulletFX.mExploding);
					buffer.WriteFloat(darkFrogBulletFX.mX);
					buffer.WriteFloat(darkFrogBulletFX.mY);
					buffer.WriteFloat(darkFrogBulletFX.mTwirlAngle);
					buffer.WriteBoolean(darkFrogBulletFX.mBallEffect != null);
					if (darkFrogBulletFX.mBallEffect != null)
					{
						Common.SerializePIEffect(darkFrogBulletFX.mBallEffect, sync);
					}
					buffer.WriteBoolean(darkFrogBulletFX.mBallExplosion != null);
					if (darkFrogBulletFX.mBallExplosion != null)
					{
						Common.SerializePIEffect(darkFrogBulletFX.mBallExplosion, sync);
					}
				}
				return;
			}
			this.mBulletFX.Clear();
			bool particle_3d = buffer.ReadBoolean();
			int num = (int)buffer.ReadLong();
			for (int j = 0; j < num; j++)
			{
				DarkFrogBulletFX darkFrogBulletFX2 = new DarkFrogBulletFX();
				this.mBulletFX.Add(darkFrogBulletFX2);
				darkFrogBulletFX2.mBulletId = (int)buffer.ReadLong();
				darkFrogBulletFX2.mExploding = buffer.ReadBoolean();
				darkFrogBulletFX2.mX = buffer.ReadFloat();
				darkFrogBulletFX2.mY = buffer.ReadFloat();
				darkFrogBulletFX2.mTwirlAngle = buffer.ReadFloat();
				if (buffer.ReadBoolean())
				{
					darkFrogBulletFX2.mBallEffect = this.GetFreeBallEffect(particle_3d);
					Common.DeserializePIEffect(darkFrogBulletFX2.mBallEffect, sync);
				}
				if (buffer.ReadBoolean())
				{
					darkFrogBulletFX2.mBallExplosion = this.GetFreeBallExplosion(particle_3d);
					Common.DeserializePIEffect(darkFrogBulletFX2.mBallExplosion, sync);
				}
			}
		}

		// Token: 0x040001DD RID: 477
		private static int CANVAS_W = 293;

		// Token: 0x040001DE RID: 478
		private static int CANVAS_H = 268;

		// Token: 0x040001DF RID: 479
		protected List<FXCache> mFXCache = new List<FXCache>();

		// Token: 0x040001E0 RID: 480
		protected List<DarkFrogBulletFX> mBulletFX = new List<DarkFrogBulletFX>();

		// Token: 0x040001E1 RID: 481
		protected PIEffect mDeathAura;

		// Token: 0x040001E2 RID: 482
		protected PopAnim mHitAnim;

		// Token: 0x040001E3 RID: 483
		protected int mTimer;

		// Token: 0x040001E4 RID: 484
		protected int mFiringState;

		// Token: 0x040001E5 RID: 485
		protected int mBlinkFrame = -1;

		// Token: 0x040001E6 RID: 486
		protected float mTongueYOff;

		// Token: 0x040001E7 RID: 487
		protected bool mBlinkForward = true;

		// Token: 0x040001E8 RID: 488
		protected float mAlpha;

		// Token: 0x040001E9 RID: 489
		protected float mRecoilY;

		// Token: 0x040001EA RID: 490
		protected float mBodyAlpha = 255f;

		// Token: 0x040001EB RID: 491
		protected float mTattooAlpha = 255f;

		// Token: 0x0200001D RID: 29
		private enum FireState
		{
			// Token: 0x040001ED RID: 493
			FiringState_NULL,
			// Token: 0x040001EE RID: 494
			FiringState_ShowTongue,
			// Token: 0x040001EF RID: 495
			FiringState_Launching,
			// Token: 0x040001F0 RID: 496
			FiringState_HideTongue
		}
	}
}
