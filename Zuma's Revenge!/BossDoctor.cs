using System;
using System.Collections.Generic;
using SexyFramework;
using SexyFramework.AELib;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.PIL;

namespace ZumasRevenge
{
	// Token: 0x02000020 RID: 32
	public class BossDoctor : BossShoot
	{
		// Token: 0x0600053C RID: 1340 RVA: 0x00017864 File Offset: 0x00015A64
		protected override void DrawBossSpecificArt(Graphics g)
		{
			int num = (int)(this.mX - (float)(this.mWidth / 2) + (float)this.mShakeXOff);
			int num2 = (int)(this.mY - (float)(this.mHeight / 2) + (float)this.mShakeYOff);
			if (this.mAlphaOverride == 0f)
			{
				return;
			}
			g.PushState();
			if (this.mAlphaOverride < 255f)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)this.mAlphaOverride);
			}
			g.DrawImageCel(Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_HAIR), Common._S(num + Common._M(3)), (int)((float)Common._S(num2 + Common._M1(2)) + this.mBossYOff), this.mHairCel);
			if (g.Is3D())
			{
				g.DrawImageRotatedF(Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_ROD_RIGHT), (float)Common._S(num + Common._M(130)), Common._S((float)(num2 + Common._M1(52)) + this.mStaff[0].mYOff), (double)this.mStaff[0].mAngle);
			}
			else
			{
				g.DrawImageRotated(Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_ROD_RIGHT), Common._S(num + Common._M(130)), (int)Common._S((float)(num2 + Common._M1(52)) + this.mStaff[0].mYOff), (double)this.mStaff[0].mAngle);
			}
			if (g.Is3D())
			{
				g.DrawImageRotatedF(Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_ROD_LEFT), (float)Common._S(num + Common._M(0)), Common._S((float)(num2 + Common._M1(52)) + this.mStaff[1].mYOff), (double)this.mStaff[1].mAngle);
			}
			else
			{
				g.DrawImageRotated(Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_ROD_LEFT), Common._S(num + Common._M(0)), (int)Common._S((float)(num2 + Common._M1(52)) + this.mStaff[1].mYOff), (double)this.mStaff[1].mAngle);
			}
			g.DrawImageCel(Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_FACES), Common._S(num + Common._M(55)), (int)Common._S((float)(num2 + Common._M1(37)) + this.mBossYOff), this.mFaceCel);
			this.DrawGlowingRings(g, this.mStaff[0].mRingSize, Common._S(this.mStaff[0].mYOff), (float)Common._S(Common._M(160)), (float)Common._S(Common._M1(70)), new SexyColor(Common._M2(255), Common._M3(224), Common._M4(0), (int)this.mStaff[0].mRingAlpha), new SexyColor(Common._M(255), Common._M1(255), Common._M2(0), (int)(this.mStaff[0].mRingAlpha / Common._M3(2f))), new SexyColor(Common._M4(255), Common._M5(0), Common._M6(0), (int)(this.mStaff[0].mRingAlpha / Common._M7(3f))));
			this.DrawGlowingRings(g, this.mStaff[1].mRingSize, Common._S(this.mStaff[1].mYOff), (float)Common._S(Common._M(15)), (float)Common._S(Common._M1(70)), new SexyColor(Common._M2(36), Common._M3(18), Common._M4(53), (int)this.mStaff[1].mRingAlpha), new SexyColor(Common._M(36), Common._M1(18), Common._M2(53), (int)(this.mStaff[1].mRingAlpha / Common._M3(2f))), new SexyColor(Common._M4(255), Common._M5(0), Common._M6(0), (int)(this.mStaff[1].mRingAlpha / Common._M7(3f))));
			g.PopState();
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00017C4E File Offset: 0x00015E4E
		protected override void DrawBerserk(Graphics g)
		{
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00017C50 File Offset: 0x00015E50
		protected override Rect GetBulletRect(BossBullet b)
		{
			Image image = BossDoctor.gBulletImages[b.mImageNum];
			int num = (int)((float)image.mWidth * b.mSize);
			int num2 = (int)((float)image.mHeight * b.mSize);
			return new Rect((int)(b.mX - (float)(num / 2)), (int)(b.mY - (float)(num2 / 2)), num, num2).Inflate(Common._M(0), Common._M1(0));
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00017CC0 File Offset: 0x00015EC0
		protected override void DidFire()
		{
			base.DidFire();
			if (this.mHairCel == 0 && this.mBossYOff == 0f && !this.mDoingHitAnim)
			{
				this.mStaff[0].mControlsExtras = true;
				this.mStaff[1].mControlsExtras = false;
			}
			if (!this.mIsFiring)
			{
				int num = this.mBullets.size<BossBullet>() - base.mMaxBulletsToFire;
				this.AddParticleSystem(this.mBullets[num].mId, (int)this.mBullets[num].mX, (int)this.mBullets[num].mY, 0, this.mBullets[num].mDelay);
			}
			this.mIsFiring = true;
			this.mStaff[0].mNumBullets += base.mMaxBulletsToFire;
			for (int i = this.mBullets.size<BossBullet>() - 1; i >= this.mBullets.size<BossBullet>() - base.mMaxBulletsToFire; i--)
			{
				this.mBullets[i].mImageNum = 0;
				this.mBullets[i].mSize = 0f;
				this.mBullets[i].mAlpha = 0f;
			}
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00017DFC File Offset: 0x00015FFC
		protected override void DidRetaliate(int num_shot)
		{
			if (!this.mIsRetaliating)
			{
				int num = this.mBullets.size<BossBullet>() - num_shot;
				this.AddParticleSystem(this.mBullets[num].mId, (int)this.mBullets[num].mX, (int)this.mBullets[num].mY, 1, this.mBullets[num].mDelay);
			}
			this.mIsRetaliating = true;
			this.mStaff[1].mNumBullets += num_shot;
			if (this.mHairCel == 0 && this.mBossYOff == 0f && !this.mDoingHitAnim)
			{
				this.mStaff[0].mControlsExtras = false;
				this.mStaff[1].mControlsExtras = true;
			}
			for (int i = this.mBullets.size<BossBullet>() - 1; i >= this.mBullets.size<BossBullet>() - num_shot; i--)
			{
				this.mBullets[i].mImageNum = 1;
				this.mBullets[i].mSize = 0f;
				this.mBullets[i].mAlpha = 0f;
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00017F24 File Offset: 0x00016124
		protected override bool PreBulletUpdate(BossBullet b, int index)
		{
			if (b.mState == 0)
			{
				if (!this.PreState0Helper(0, b) && !this.PreState0Helper(1, b))
				{
					return true;
				}
			}
			else
			{
				if (b.mDelay > 0)
				{
					b.mDelay--;
					if (!this.PreDelayHelper(0, b))
					{
						this.PreDelayHelper(1, b);
					}
					return true;
				}
				if (b.mDelay == 0 && b.mState > 0)
				{
					float num = Common._M(12f);
					float num2 = Common._M(0.1f);
					if (b.mSize < 1f)
					{
						b.mSize = Math.Min(b.mSize + num2, 1f);
						if (b.mImageNum == 0)
						{
							b.mX = this.mX + (float)Common._M(74);
						}
					}
					if (b.mAlpha < 255f)
					{
						b.mAlpha = Math.Min(b.mAlpha + num, 255f);
					}
				}
			}
			return false;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00018014 File Offset: 0x00016214
		protected void UpdateStaffState(ref bool state_bool, float ring_size_max, DoctorStaff d)
		{
			float num = Common._M(3f);
			float num2 = Common._M(4f);
			float num3 = (float)Common._M(-15);
			float num4 = -((float)BossDoctor.MAX_SPEAR_Y_OFF / num);
			float num5 = -((float)BossDoctor.MAX_SPEAR_Y_OFF / num2);
			float num6 = num3 / -num4;
			float num7 = num3 / -num5;
			int num8 = (int)(num5 / (float)(Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_HAIR).mNumCols - 2));
			float num9 = BossDoctor.MAX_ANGLE / num4;
			int num10 = (int)(num4 / 4f);
			int num11 = (int)(num5 / 4f);
			if (state_bool)
			{
				if (d.mNumBullets > 0 && d.mYOff > (float)BossDoctor.MAX_SPEAR_Y_OFF)
				{
					if (d.mControlsExtras && !this.mDoingHitAnim && this.mBossYOff > num3)
					{
						this.mBossYOff = Math.Max(this.mBossYOff - num6, num3);
					}
					d.mYOff = Math.Max(d.mYOff - num, (float)BossDoctor.MAX_SPEAR_Y_OFF);
					d.mAngle = Math.Min(d.mAngle + num9, BossDoctor.MAX_ANGLE);
					if (d.mControlsExtras && !this.mDoingHitAnim && this.mHairCel == 0 && d.mYOff <= (float)(BossDoctor.MAX_SPEAR_Y_OFF / 3))
					{
						this.mHairCel++;
					}
					else if (d.mControlsExtras && !this.mDoingHitAnim && this.mHairCel == 1 && d.mYOff <= (float)(2 * BossDoctor.MAX_SPEAR_Y_OFF / 3))
					{
						this.mHairCel++;
					}
					if (d.mControlsExtras && !this.mDoingHitAnim && this.mUpdateCount % num10 == 0 && this.mFaceCel < 3)
					{
						this.mFaceCel++;
						return;
					}
				}
				else if (d.mNumBullets > 0 && d.mYOff <= (float)BossDoctor.MAX_SPEAR_Y_OFF)
				{
					int num12 = Common._M(8);
					if (d.mRingAlpha < 255f)
					{
						d.mRingAlpha = Math.Min(d.mRingAlpha + (float)num12, 255f);
					}
					if (d.mRingSize < ring_size_max)
					{
						d.mRingSize += Common._M(0.05f);
						if (d.mRingSize > ring_size_max)
						{
							d.mRingSize = ring_size_max;
							return;
						}
					}
				}
				else if (d.mNumBullets == 0 && d.mYOff < 0f)
				{
					d.mYOff = Math.Min(d.mYOff + num2, 0f);
					d.mAngle = Math.Max(d.mAngle - num9, 0f);
					if (d.mRingSize != 0f && this.mBossYOff < 0f)
					{
						this.mBossYOff = Math.Min(this.mBossYOff + num7, 0f);
					}
					if (d.mControlsExtras && !this.mDoingHitAnim && this.mUpdateCount % num8 == 0)
					{
						this.mHairCel++;
					}
					if (d.mControlsExtras && !this.mDoingHitAnim && this.mHairCel >= Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_HAIR).mNumCols)
					{
						this.mHairCel = 0;
					}
					if (d.mControlsExtras && !this.mDoingHitAnim && this.mFaceCel > 0 && this.mUpdateCount % num11 == 0)
					{
						this.mFaceCel--;
					}
					int num13 = Common._M(12);
					if (d.mRingAlpha > 0f)
					{
						d.mRingAlpha = Math.Max(0f, d.mRingAlpha - (float)num13);
					}
					if (d.mYOff >= 0f)
					{
						if (d.mRingAlpha <= 0f)
						{
							d.mRingSize = 0f;
						}
						state_bool = false;
						if (d.mControlsExtras && !this.mDoingHitAnim)
						{
							this.mHairCel = 0;
							return;
						}
					}
				}
			}
			else if (d.mRingAlpha > 0f)
			{
				d.mRingAlpha = Math.Max(0f, d.mRingAlpha - (float)Common._M(6));
				if (d.mRingAlpha <= 0f)
				{
					d.mRingSize = 0f;
				}
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001840C File Offset: 0x0001660C
		protected bool PreState0Helper(int staffnum, BossBullet b)
		{
			if (b.mImageNum == staffnum && this.mStaff[staffnum].mYOff <= (float)BossDoctor.MAX_SPEAR_Y_OFF && this.mStaff[staffnum].mRingAlpha >= 255f)
			{
				if (b.mState != 1)
				{
					bool flag = false;
					for (int i = 0; i < this.mParticles.size<BossBulletParticleSystem>(); i++)
					{
						if (this.mParticles[i].mBulletId == b.mId)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.AddParticleSystem(b.mId, (int)b.mX, (int)b.mY, -1, b.mDelay);
					}
				}
				b.mState = 1;
				if (b.mDelay == 0)
				{
					this.mStaff[staffnum].mNumBullets--;
					b.mX = this.mX + (float)((staffnum == 0) ? Common._M(74) : Common._M1(-65));
					b.mY = this.mY + this.mStaff[staffnum].mYOff + (float)Common._M(16);
					for (int j = 0; j < this.mParticles.Count; j++)
					{
						if (this.mParticles[j].mBulletId == b.mId)
						{
							this.mParticles[j].mSystem.SetPos(b.mX - (float)Common._M(5), b.mY - (float)Common._M1(0));
							this.mParticles[j].mAttachedToStaff = -1;
							break;
						}
					}
					if (b.mShotType == 1)
					{
						base.FireBulletAtPlayer(b, SexyFramework.Common.FloatRange(base.mMinBulletSpeed, base.mMaxBulletSpeed), b.mX, b.mY);
						b.mTargetVX = b.mVX;
						b.mTargetVY = b.mVY;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x000185E0 File Offset: 0x000167E0
		protected bool PreDelayHelper(int staffnum, BossBullet b)
		{
			if (b.mImageNum == staffnum && b.mDelay == 0)
			{
				this.mStaff[staffnum].mNumBullets--;
				b.mX = this.mX + (float)((staffnum == 0) ? Common._M(74) : Common._M1(-45));
				b.mY = this.mY + this.mStaff[staffnum].mYOff + (float)Common._M(24);
				for (int i = 0; i < this.mParticles.Count; i++)
				{
					if (this.mParticles[i].mBulletId == b.mId)
					{
						this.mParticles[i].mSystem.SetPos(b.mX - (float)Common._M(5), b.mY - (float)Common._M1(0));
						break;
					}
				}
				if (b.mShotType == 1)
				{
					base.FireBulletAtPlayer(b, SexyFramework.Common.FloatRange(base.mMinBulletSpeed, base.mMaxBulletSpeed), b.mX, b.mY);
					b.mTargetVX = b.mVX;
					b.mTargetVY = b.mVY;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001870C File Offset: 0x0001690C
		protected override bool DoHit(Bullet b, bool from_prox_bomb)
		{
			if (this.mHP != this.mMaxHP && this.mTutorialState != 0)
			{
				return false;
			}
			if (!this.mIsFiring && !this.mIsRetaliating)
			{
				this.mDoingHitAnim = true;
				this.mFaceCel = 4;
				this.mHitAnimUp = true;
			}
			if (this.mTutorialState == 1)
			{
				((GameApp)GlobalMembers.gSexyApp).GetBoard().mPreventBallAdvancement = true;
				TauntText tauntText = new TauntText();
				this.mTauntQueue.Add(tauntText);
				tauntText.mText = TextManager.getInstance().getString(309);
				tauntText.mTextId = 309;
				tauntText.mDelay = Common._M(1000);
				tauntText = new TauntText();
				this.mTauntQueue.Add(tauntText);
				tauntText.mText = TextManager.getInstance().getString(310);
				tauntText.mTextId = 310;
				tauntText.mDelay = Common._M(1000);
				this.mMaterializeTimer = Common._M(100);
				this.mWalls[0].mAlphaFadeDir = 1;
				this.mTikis[0].mWasHit = false;
				this.mTikis[0].mAlphaFadeDir = 1;
				this.mWalls[1].mAlphaFadeDir = 1;
				this.mTikis[1].mWasHit = false;
				this.mTikis[1].mAlphaFadeDir = 1;
				this.mTutorialState = 2;
				this.mPauseMovement = true;
			}
			return base.DoHit(b, from_prox_bomb);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001888C File Offset: 0x00016A8C
		protected override void DrawWalls(Graphics g)
		{
			int[] array = new int[]
			{
				Common._DS(Common._M(-225)),
				Common._DS(Common._M1(-225))
			};
			int[] array2 = new int[]
			{
				Common._DS(Common._M(-50)),
				Common._DS(Common._M1(0))
			};
			for (int i = 0; i < this.mWalls.Count; i++)
			{
				BossWall bossWall = this.mWalls[i];
				if (bossWall.mAlpha > 0)
				{
					Composition composition = this.mShieldCompMgr.GetComposition((i == 1) ? "TriangleShieldAnim" : "CircleShieldAnim");
					int num = (int)(Common._S(this.mX) - (float)Common._DS(composition.mWidth / 2) + (float)array[i]);
					int num2 = (int)(Common._S(this.mY) + (float)Common._DS(this.mHeight / 2) + (float)array2[i]);
					CumulativeTransform cumulativeTransform = new CumulativeTransform();
					cumulativeTransform.mOpacity = (float)bossWall.mAlpha / 255f;
					if (this.mAlphaOverride <= 254f)
					{
						cumulativeTransform.mOpacity = this.mAlphaOverride / 255f;
					}
					cumulativeTransform.mTrans.Translate((float)num, (float)num2);
					composition.Draw(g, cumulativeTransform, -1, Common._DS(1f));
				}
			}
			if (this.mDoCircleExplosion && g.Is3D())
			{
				this.mCircleExplosion.Draw(g);
			}
			if (this.mDoTriangleExplosion && g.Is3D())
			{
				this.mTriangleExplosion.Draw(g);
			}
			for (int j = 0; j < this.mShieldZaps.Count; j++)
			{
				this.mShieldZaps[j].mDrawTransform.LoadIdentity();
				float num3 = GameApp.DownScaleNum(1f);
				this.mShieldZaps[j].mDrawTransform.Scale(num3, num3);
				int num4 = 0;
				if (this.mWalls[1].mAlpha <= 0)
				{
					num4 -= Common._M(30);
				}
				this.mShieldZaps[j].mDrawTransform.Translate(Common._S(this.mX), Common._S(this.mY) + (float)Common._S(this.mHeight / 2 + 30 + num4));
				this.mShieldZaps[j].Draw(g);
			}
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00018AF0 File Offset: 0x00016CF0
		protected override bool CollidesWithWall(Bullet b)
		{
			bool flag = false;
			for (int i = 0; i < this.mWalls.Count; i++)
			{
				if (this.mWalls[i].mAlphaFadeDir >= 0 && this.mWalls[i].mAlpha >= 1)
				{
					flag = true;
					break;
				}
			}
			if (!flag || this.mTutorialState != 0)
			{
				return false;
			}
			if (MathUtils.CirclesIntersect(b.GetX(), b.GetY(), this.mX, this.mY, (float)(b.GetRadius() + Common._M(125))))
			{
				PIEffect pieffect = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_SHIELD_ZAP).Duplicate();
				if (GameApp.gApp.mHiRes)
				{
					pieffect.mEmitterTransform.Translate(Common._S(b.GetX() - this.mX), 0f);
				}
				else
				{
					pieffect.mEmitterTransform.Translate(Common._S((b.GetX() - this.mX) * 2f), 0f);
				}
				Common.SetFXNumScale(pieffect, GameApp.gApp.Is3DAccelerated() ? 1f : Common._M(0.5f));
				this.mShieldZaps.Add(pieffect);
				GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BOSS2_SHIELD_HIT));
				this.mTauntQueue.Clear();
				TauntText tauntText = new TauntText();
				this.mTauntQueue.Add(tauntText);
				tauntText.mText = TextManager.getInstance().getString(311);
				tauntText.mTextId = 311;
				tauntText.mDelay = Common._M(500);
				return true;
			}
			return false;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00018C80 File Offset: 0x00016E80
		protected override void BulletHitPlayer(BossBullet b)
		{
			base.BulletHitPlayer(b);
			int num = Common._M(150);
			for (int i = 0; i < this.mParticles.Count; i++)
			{
				BossBulletParticleSystem bossBulletParticleSystem = this.mParticles[i];
				if (bossBulletParticleSystem.mBulletId == b.mId)
				{
					bossBulletParticleSystem.mBulletId = -1;
					bossBulletParticleSystem.mSystem.SetLife(bossBulletParticleSystem.mSystem.GetUpdateCount() + num);
					List<Particle> list = new List<Particle>();
					bossBulletParticleSystem.mSystem.GetEmitter(bossBulletParticleSystem.mEmitterHandle).GetParticlesOfType(bossBulletParticleSystem.mHead1Handle, ref list);
					bossBulletParticleSystem.mSystem.GetEmitter(bossBulletParticleSystem.mEmitterHandle).GetParticlesOfType(bossBulletParticleSystem.mHead2Handle, ref list);
					for (int j = 0; j < list.Count; j++)
					{
						Particle particle = list[j];
						particle.mLife = num;
						particle.mUpdateCount = 0;
						particle.ClearLifetimeFrames();
						particle.AddLifetimeKeyFrame(1f, new LifetimeSettings
						{
							mSpinMult = Common._M(2f),
							mSizeXMult = Common._M(4f)
						});
						particle.mAlphaKeyManager.ForceTransition(num, new SexyColor(255, 255, 255, 0));
					}
					break;
				}
			}
			if (!GameApp.gApp.GetLevelMgr().mBossesCanAttackFuckedFrog)
			{
				for (int k = 0; k < this.mBullets.Count; k++)
				{
					BossBullet bossBullet = this.mBullets[k];
					if (bossBullet.mDelay > 0)
					{
						bossBullet.mDeleteInstantly = true;
					}
				}
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00018E1E File Offset: 0x0001701E
		public override void DeleteAllBullets()
		{
			base.DeleteAllBullets();
			this.mParticles.Clear();
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00018E31 File Offset: 0x00017031
		protected void AddParticleSystem(int bullet_id, int x, int y, int attached)
		{
			this.AddParticleSystem(bullet_id, x, y, attached, 0);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00018E40 File Offset: 0x00017040
		protected void AddParticleSystem(int bullet_id, int x, int y, int attached, int delay)
		{
			BossBulletParticleSystem bossBulletParticleSystem = new BossBulletParticleSystem();
			this.mParticles.Add(bossBulletParticleSystem);
			bossBulletParticleSystem.mBulletId = bullet_id;
			bossBulletParticleSystem.mAttachedToStaff = attached;
			bossBulletParticleSystem.mSystem = new SexyFramework.PIL.System(50, 50);
			bossBulletParticleSystem.mSystem.mScale = Common._S(1f);
			bossBulletParticleSystem.mSystem.SetMinSpawnFrame(delay + 1);
			bossBulletParticleSystem.mSystem.WaitForEmitters(true);
			Emitter emitter = new Emitter();
			emitter.mCullingRect = new Rect(0, 0, Common._SS(GlobalMembers.gSexyApp.mWidth), Common._SS(GlobalMembers.gSexyApp.mHeight));
			emitter.mEmissionCoordsAreOffsets = true;
			emitter.AddScaleKeyFrame(0, new EmitterScale
			{
				mLifeScale = Common._M(0.27f),
				mNumberScale = Common._M(2f),
				mSizeXScale = Common._M(0.8f),
				mVelocityScale = Common._M(1.5f),
				mWeightScale = Common._M(3f),
				mSpinScale = Common._M(0.54f),
				mZoom = Common._M(3.63f)
			});
			EmitterSettings emitterSettings = new EmitterSettings();
			emitterSettings.mEmissionRange = SexyFramework.Common.DegreesToRadians(163f);
			emitterSettings.mXRadius = (emitterSettings.mYRadius = (float)Common._M(3));
			emitter.AddSettingsKeyFrame(0, emitterSettings);
			emitter.SetEmitterType(2);
			ParticleType particleType = new ParticleType();
			particleType.AddSettingsKeyFrame(0, new ParticleSettings
			{
				mLife = Common._M(6),
				mNumber = Common._M(9999),
				mXSize = Common._M(14),
				mSpin = SexyFramework.Common.DegreesToRadians((float)Common._M(70))
			});
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_RUNE_LEFT);
			particleType.mSingle = true;
			bossBulletParticleSystem.mHead1Handle = emitter.AddParticleType(particleType);
			ParticleType particleType2 = new ParticleType(particleType);
			particleType2.mAdditive = true;
			particleType2.GetSettingsKeyFrame(0).mSpin *= Common._M(-1f);
			particleType2.GetSettingsKeyFrame(0).mXSize = Common._M(20);
			particleType2.mColorKeyManager.AddColorKey(0f, new SexyColor(Common._M(50), Common._M1(100), Common._M2(255)));
			bossBulletParticleSystem.mHead2Handle = emitter.AddParticleType(particleType2);
			ParticleType particleType3 = new ParticleType();
			particleType3.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_RUNE_LEFT_OUTLINE);
			particleType3.mColorKeyManager.AddColorKey(0f, new SexyColor(Common._M(121), Common._M1(12), Common._M2(255)));
			particleType3.mColorKeyManager.AddColorKey(Common._M(0.2f), new SexyColor(Common._M1(121), Common._M2(12), Common._M3(255)));
			particleType3.mColorKeyManager.AddColorKey(1f, new SexyColor(255, 0, 0));
			particleType3.mAlphaKeyManager.AddAlphaKey(0f, 0);
			particleType3.mAlphaKeyManager.AddAlphaKey(Common._M(0.15f), 255);
			particleType3.mAlphaKeyManager.AddAlphaKey(Common._M(0.85f), 255);
			particleType3.mAlphaKeyManager.AddAlphaKey(1f, 0);
			particleType3.mAdditive = true;
			particleType3.AddSettingsKeyFrame(0, new ParticleSettings
			{
				mLife = Common._M(13),
				mNumber = Common._M(27),
				mXSize = Common._M(9),
				mSpin = SexyFramework.Common.DegreesToRadians((float)Common._M(50))
			});
			particleType3.AddVarianceKeyFrame(0, new ParticleVariance
			{
				mSizeXVar = Common._M(10),
				mSpinVar = SexyFramework.Common.DegreesToRadians((float)Common._M(50))
			});
			LifetimeSettings lifetimeSettings = new LifetimeSettings();
			particleType3.AddSettingAtLifePct(0f, lifetimeSettings);
			particleType3.AddSettingAtLifePct(1f, new LifetimeSettings(lifetimeSettings)
			{
				mSizeXMult = Common._M(1.5f)
			});
			emitter.AddParticleType(particleType3);
			bossBulletParticleSystem.mEmitterHandle = bossBulletParticleSystem.mSystem.AddEmitter(emitter);
			bossBulletParticleSystem.mSystem.SetPos((float)x, (float)y);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00019288 File Offset: 0x00017488
		protected void DrawGlowingRings(Graphics g, float size, float yoff, float ring_xoff, float ring_yoff, SexyColor c1, SexyColor c2, SexyColor c3)
		{
			int num = (int)Common._S(this.mX - (float)(this.mWidth / 2) + (float)this.mShakeXOff);
			int num2 = (int)Common._S(this.mY - (float)(this.mHeight / 2) + (float)this.mShakeYOff);
			SexyColor color = c1;
			SexyColor color2 = c2;
			SexyColor color3 = c3;
			color.mAlpha = (int)Math.Min((float)color.mAlpha, this.mAlphaOverride);
			color2.mAlpha = (int)Math.Min((float)color2.mAlpha, this.mAlphaOverride);
			color3.mAlpha = (int)Math.Min((float)color3.mAlpha, this.mAlphaOverride);
			if (Common._M(0) != 0)
			{
				g.SetDrawMode(1);
			}
			this.mGlobalTranform.Reset();
			this.mGlobalTranform.Scale(size, size);
			g.SetColorizeImages(true);
			g.SetColor(color);
			if (g.Is3D())
			{
				g.DrawImageTransformF(Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_GLOW_RINGS), this.mGlobalTranform, (float)num + ring_xoff, (float)num2 + ring_yoff + yoff);
			}
			else
			{
				g.DrawImageTransform(Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_GLOW_RINGS), this.mGlobalTranform, (float)num + ring_xoff, (float)num2 + ring_yoff + yoff);
			}
			this.mGlobalTranform.Reset();
			float num3 = size / Common._M(0.51f);
			this.mGlobalTranform.GetMatrix().Scale(num3, num3);
			g.SetColor(color2);
			if (g.Is3D())
			{
				g.DrawImageTransformF(Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_GLOW_CIRCLE), this.mGlobalTranform, (float)num + ring_xoff, (float)num2 + ring_yoff + yoff);
			}
			else
			{
				g.DrawImageTransform(Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_GLOW_CIRCLE), this.mGlobalTranform, (float)num + ring_xoff, (float)num2 + ring_yoff + yoff);
			}
			num3 = size / Common._M(1.1f);
			this.mGlobalTranform.Reset();
			this.mGlobalTranform.Scale(num3, num3);
			g.SetColor(color3);
			if (g.Is3D())
			{
				g.DrawImageTransformF(Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_GLOW_CIRCLE), this.mGlobalTranform, (float)num + ring_xoff, (float)num2 + ring_yoff + yoff);
			}
			else
			{
				g.DrawImageTransform(Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_GLOW_CIRCLE), this.mGlobalTranform, (float)num + ring_xoff, (float)num2 + ring_yoff + yoff);
			}
			g.SetDrawMode(0);
			g.SetColorizeImages(false);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x000194C4 File Offset: 0x000176C4
		protected override void BossBulletDestroyed(BossBullet b, bool outofscreen)
		{
			base.BossBulletDestroyed(b, outofscreen);
			for (int i = 0; i < this.mParticles.Count; i++)
			{
				if (this.mParticles[i].mBulletId == b.mId)
				{
					this.mParticles[i].mSystem.ForceStopEmitting(true);
					return;
				}
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00019520 File Offset: 0x00017720
		protected override bool CanFire()
		{
			return this.mTutorialState == 0;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0001952B File Offset: 0x0001772B
		protected override bool CanTaunt()
		{
			return this.mTutorialState == 0 && base.CanTaunt();
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001953D File Offset: 0x0001773D
		protected override bool CanRetaliate()
		{
			return this.mTutorialState == 0;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00019548 File Offset: 0x00017748
		protected override bool CanDecTikiHealthSpawnAmt()
		{
			return this.mTutorialState == 0;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00019554 File Offset: 0x00017754
		protected override void ResetWallAndTikis(int wall_index)
		{
			if (this.mHP <= 0f)
			{
				return;
			}
			string @string = TextManager.getInstance().getString(695);
			this.mWalls[wall_index].mAlphaFadeDir = 1;
			if (this.mWalls.Count == this.mTikis.Count)
			{
				this.mTikis[wall_index].mWasHit = false;
				this.mTikis[wall_index].mAlphaFadeDir = 1;
				if (this.mHP > 0f && this.mTutorialState == 0 && (this.mTauntQueue.Count == 0 || !string.Equals(this.mTauntQueue.back<TauntText>().mText, @string)) && this.mHP < 100f)
				{
					this.mTauntQueue.Clear();
					TauntText tauntText = new TauntText();
					this.mTauntQueue.Add(tauntText);
					tauntText.mText = @string;
					tauntText.mTextId = 695;
					tauntText.mDelay = Common._M(500);
				}
			}
			GlobalMembers.gSexyApp.PlaySample(Res.GetSoundByID(ResID.SOUND_TIKI_APPEAR), Common._M(50));
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00019671 File Offset: 0x00017871
		protected override void TikiHit(int idx)
		{
			if (this.mTikis[idx].mIsLeftTiki)
			{
				this.mDoCircleExplosion = true;
				this.mCircleExplosion.ResetAnim();
				return;
			}
			this.mDoTriangleExplosion = true;
			this.mTriangleExplosion.ResetAnim();
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000196AB File Offset: 0x000178AB
		public BossDoctor()
			: base(null)
		{
			this.Initialize();
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x000196DC File Offset: 0x000178DC
		public BossDoctor(Level l)
			: base(l)
		{
			this.Initialize();
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00019710 File Offset: 0x00017910
		private void Initialize()
		{
			this.mMaterializeTimer = BossDoctor.MIN_SPAWN_TIMER;
			this.mTikiCompMgr = null;
			this.mShieldCompMgr = null;
			this.mTriangleExplosion = null;
			this.mCircleExplosion = null;
			this.mDoCircleExplosion = false;
			this.mDoTriangleExplosion = false;
			this.mIsFiring = (this.mIsRetaliating = (this.mDoingHitAnim = false));
			this.mHairCel = (this.mFaceCel = 0);
			this.mHitAnimUp = true;
			this.mBossYOff = 0f;
			this.mBerserkBaseAlpha = 0f;
			this.mBerserkEyeFrame = 0;
			this.mBerserkCounter = 0;
			this.mBerserkBaseAlphaDir = Common._M(4.6f);
			this.mResGroup = "Boss2";
			this.mResPrefix = "IMAGE_BOSS_DOCTOR_";
			this.mBossRadius = Common._M(85);
			this.mBulletRadius = Common._M(25);
			this.mBandagedXOff = Common._M(10);
			this.mBandagedYOff = Common._M(15);
			if (GameApp.gApp != null && (GameApp.gApp.IsHardMode() || (GameApp.gApp.mUserProfile != null && GameApp.gApp.mUserProfile.GetAdvModeVars().mNumTimesZoneBeat[1] > 0)))
			{
				this.mTutorialState = 0;
			}
			else
			{
				this.mTutorialState = 1;
			}
			for (int i = 0; i < this.mStaff.Length; i++)
			{
				this.mStaff[i] = new DoctorStaff();
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00019868 File Offset: 0x00017A68
		public override void Dispose()
		{
			base.Dispose();
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00019870 File Offset: 0x00017A70
		public override void Update()
		{
			this.Update(1f);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00019880 File Offset: 0x00017A80
		public override void Update(float f)
		{
			base.Update(f);
			if (this.mTutorialState == 2 && this.mWalls[0].mAlpha == 255 && this.mWalls[1].mAlpha == 255 && this.mTikis[0].mAlpha == 255 && this.mTikis[1].mAlpha == 255)
			{
				this.mTutorialState = 0;
				this.mPauseMovement = false;
				((GameApp)GlobalMembers.gSexyApp).GetBoard().mPreventBallAdvancement = false;
				this.mMaterializeTimer = Common._M(20);
			}
			this.mShieldCompMgr.UpdateAll();
			this.mTikiCompMgr.UpdateAll();
			if (this.mDoCircleExplosion)
			{
				int num = (int)(Common._S(this.mX) + (float)Common._DS(Common._M(22)));
				int num2 = (int)(Common._S(this.mY) + (float)Common._DS(Common._M(105)));
				this.mCircleExplosion.mDrawTransform.LoadIdentity();
				float num3 = GameApp.ScaleNum(1f);
				this.mCircleExplosion.mDrawTransform.Scale(num3, num3);
				this.mCircleExplosion.mDrawTransform.Translate((float)num, (float)num2);
				this.mCircleExplosion.Update();
				if (this.mCircleExplosion.mFrameNum > (float)this.mCircleExplosion.mLastFrameNum)
				{
					this.mDoCircleExplosion = false;
				}
			}
			if (this.mDoTriangleExplosion)
			{
				int num4 = (int)(Common._S(this.mX) + (float)Common._DS(Common._M(30)));
				int num5 = (int)(Common._S(this.mY) + (float)Common._DS(Common._M(210)));
				this.mTriangleExplosion.mDrawTransform.LoadIdentity();
				float num6 = GameApp.ScaleNum(1f);
				this.mTriangleExplosion.mDrawTransform.Scale(num6, num6);
				this.mTriangleExplosion.mDrawTransform.Translate((float)num4, (float)num5);
				this.mTriangleExplosion.Update();
				if (this.mTriangleExplosion.mFrameNum > (float)this.mTriangleExplosion.mLastFrameNum)
				{
					this.mDoTriangleExplosion = false;
				}
			}
			for (int i = 0; i < this.mParticles.Count; i++)
			{
				BossBulletParticleSystem bossBulletParticleSystem = this.mParticles[i];
				BossBullet bossBullet = null;
				for (int j = 0; j < this.mBullets.Count; j++)
				{
					if (this.mBullets[j].mId == bossBulletParticleSystem.mBulletId)
					{
						bossBullet = this.mBullets[j];
						break;
					}
				}
				if (bossBullet != null && bossBullet.mDelay == 0 && bossBullet.mState > 0)
				{
					bossBulletParticleSystem.mSystem.Move(bossBullet.mVX, bossBullet.mVY);
				}
				else if (bossBulletParticleSystem.mBulletId == -1)
				{
					int centerX = this.mLevel.mFrog.GetCenterX();
					int centerY = this.mLevel.mFrog.GetCenterY();
					float xamt = 0f;
					float yamt = 0f;
					float num7 = Common._M(10f);
					if (!SexyFramework.Common._eq(bossBulletParticleSystem.mSystem.GetLastX(), (float)centerX, 0.1f))
					{
						xamt = ((float)centerX - bossBulletParticleSystem.mSystem.GetLastX()) / num7;
					}
					if (!SexyFramework.Common._eq(bossBulletParticleSystem.mSystem.GetLastY(), (float)centerY, 0.1f))
					{
						yamt = ((float)centerY - bossBulletParticleSystem.mSystem.GetLastY()) / num7;
					}
					bossBulletParticleSystem.mSystem.Move(xamt, yamt);
				}
				bossBulletParticleSystem.mSystem.Update();
				if (bossBulletParticleSystem.mSystem.Done())
				{
					this.mParticles.RemoveAt(i);
					i--;
				}
			}
			if (this.mDoDeathExplosions || this.mHP <= 0f || this.mLevel.mBoard.DoingBossIntro())
			{
				return;
			}
			if (this.mDoingHitAnim)
			{
				if (this.mUpdateCount % Common._M(6) == 0 && this.mFaceCel < Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_FACES).mNumCols - 1)
				{
					this.mFaceCel++;
				}
				if (this.mUpdateCount % Common._M(6) == 0 && this.mHairCel < Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_HAIR).mNumCols - 1)
				{
					this.mHairCel++;
				}
				if (this.mHitAnimUp)
				{
					this.mBossYOff -= Common._M(1f);
					if (this.mBossYOff <= (float)Common._M(-15))
					{
						this.mHitAnimUp = false;
						this.mBossYOff = (float)Common._M(-15);
					}
				}
				else
				{
					this.mBossYOff += Common._M(1.5f);
					if (this.mBossYOff >= 0f)
					{
						this.mBossYOff = 0f;
						this.mFaceCel = 0;
						this.mHairCel = 0;
						this.mDoingHitAnim = false;
					}
				}
			}
			else
			{
				this.UpdateStaffState(ref this.mIsFiring, Common._M(0.75f), this.mStaff[0]);
				this.UpdateStaffState(ref this.mIsRetaliating, Common._M(0.75f), this.mStaff[1]);
			}
			for (int k = 0; k < this.mParticles.size<BossBulletParticleSystem>(); k++)
			{
				if (this.mParticles[k].mAttachedToStaff != -1)
				{
					this.mParticles[k].mSystem.SetPos(this.mX + (float)((this.mParticles[k].mAttachedToStaff == 0) ? Common._M(74) : Common._M1(-65)), this.mY + this.mStaff[this.mParticles[k].mAttachedToStaff].mYOff + (float)Common._M2(16));
				}
			}
			if (this.mIsBerserk)
			{
				this.mBerserkCounter++;
				this.mBerserkBaseAlpha += this.mBerserkBaseAlphaDir;
				if (this.mBerserkBaseAlphaDir > 0f && this.mBerserkBaseAlpha >= (float)Common._M(102))
				{
					this.mBerserkBaseAlpha = (float)Common._M(102);
					this.mBerserkBaseAlphaDir *= -1f;
				}
				else if (this.mBerserkBaseAlphaDir < 0f && this.mBerserkBaseAlpha <= 0f)
				{
					this.mBerserkBaseAlpha = 0f;
					this.mBerserkBaseAlphaDir *= -1f;
				}
			}
			for (int l = 0; l < this.mShieldZaps.Count; l++)
			{
				this.mShieldZaps[l].Update();
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00019F20 File Offset: 0x00018120
		public override void SyncState(DataSync sync)
		{
			base.SyncState(sync);
			sync.SyncLong(ref this.mTutorialState);
			sync.SyncLong(ref this.mMaterializeTimer);
			sync.SyncBoolean(ref this.mIsFiring);
			sync.SyncBoolean(ref this.mIsRetaliating);
			sync.SyncBoolean(ref this.mDoingHitAnim);
			sync.SyncBoolean(ref this.mHitAnimUp);
			sync.SyncLong(ref this.mHairCel);
			sync.SyncLong(ref this.mFaceCel);
			sync.SyncLong(ref this.mBerserkEyeFrame);
			sync.SyncLong(ref this.mBerserkCounter);
			sync.SyncFloat(ref this.mBossYOff);
			sync.SyncFloat(ref this.mBerserkBaseAlpha);
			sync.SyncFloat(ref this.mBerserkBaseAlphaDir);
			for (int i = 0; i < 2; i++)
			{
				DoctorStaff doctorStaff = this.mStaff[i];
				sync.SyncFloat(ref doctorStaff.mYOff);
				sync.SyncFloat(ref doctorStaff.mAngle);
				sync.SyncFloat(ref doctorStaff.mRingSize);
				sync.SyncFloat(ref doctorStaff.mRingAlpha);
				sync.SyncBoolean(ref doctorStaff.mControlsExtras);
				sync.SyncLong(ref doctorStaff.mNumBullets);
			}
			SexyBuffer buffer = sync.GetBuffer();
			if (sync.isWrite())
			{
				buffer.WriteLong((long)this.mParticles.Count);
				for (int j = 0; j < this.mParticles.Count; j++)
				{
					BossBulletParticleSystem bossBulletParticleSystem = this.mParticles[j];
					buffer.WriteLong((long)bossBulletParticleSystem.mAttachedToStaff);
					buffer.WriteLong((long)bossBulletParticleSystem.mBulletId);
					buffer.WriteLong((long)bossBulletParticleSystem.mEmitterHandle);
					buffer.WriteLong((long)bossBulletParticleSystem.mHead1Handle);
					buffer.WriteLong((long)bossBulletParticleSystem.mHead2Handle);
					Common.SerializeParticleSystem(bossBulletParticleSystem.mSystem, sync);
				}
				buffer.WriteLong((long)this.mShieldZaps.size<PIEffect>());
				for (int k = 0; k < this.mShieldZaps.size<PIEffect>(); k++)
				{
					Common.SerializePIEffect(this.mShieldZaps[k], sync);
				}
				return;
			}
			int num = (int)buffer.ReadLong();
			this.mParticles.Clear();
			for (int l = 0; l < num; l++)
			{
				BossBulletParticleSystem bossBulletParticleSystem2 = new BossBulletParticleSystem();
				bossBulletParticleSystem2.mAttachedToStaff = (int)buffer.ReadLong();
				bossBulletParticleSystem2.mBulletId = (int)buffer.ReadLong();
				bossBulletParticleSystem2.mEmitterHandle = (int)buffer.ReadLong();
				bossBulletParticleSystem2.mHead1Handle = (int)buffer.ReadLong();
				bossBulletParticleSystem2.mHead2Handle = (int)buffer.ReadLong();
				bossBulletParticleSystem2.mSystem = Common.DeserializeParticleSystem(sync);
				this.mParticles.Add(bossBulletParticleSystem2);
			}
			num = (int)buffer.ReadLong();
			this.mShieldZaps.Clear();
			for (int m = 0; m < num; m++)
			{
				PIEffect pieffect = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_SHIELD_ZAP).Duplicate();
				this.mShieldZaps.Add(pieffect);
				Common.DeserializePIEffect(pieffect, sync);
				Common.SetFXNumScale(pieffect, GameApp.gApp.Is3DAccelerated() ? 1f : Common._M(0.5f));
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001A204 File Offset: 0x00018404
		public override void DrawTopLevel(Graphics g)
		{
			base.DrawTopLevel(g);
			if (this.mHP > 0f && !this.mDoDeathExplosions && !this.mLevel.mBoard.IsPaused())
			{
				if (this.mTeleportDir != 0)
				{
					g.PushState();
					g.ClearClipRect();
				}
				int num = 0;
				for (int i = 0; i < this.mParticles.Count; i++)
				{
					num += this.mParticles[i].mSystem.GetTotalParticles();
					this.mParticles[i].mSystem.mAlphaPct = this.mAlphaOverride / 255f;
					this.mParticles[i].mSystem.Draw(g);
				}
				if (this.mTeleportDir != 0)
				{
					g.PopState();
				}
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001A2D4 File Offset: 0x000184D4
		public override void Init(Level l)
		{
			this.mWidth = 185;
			this.mHeight = 153;
			base.Init(l);
			BossDoctor.gBulletImages[0] = Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_BALL);
			BossDoctor.gBulletImages[1] = Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_RUNE_LEFT);
			if (this.mTutorialState == 1)
			{
				for (int i = 0; i < this.mWalls.Count; i++)
				{
					this.mWalls[i].mAlphaFadeDir = -1;
					this.mWalls[i].mAlpha = 0;
				}
				for (int j = 0; j < this.mTikis.Count; j++)
				{
					this.mTikis[j].mAlphaFadeDir = -1;
					this.mTikis[j].mAlpha = 0;
				}
				this.mTauntQueue.Clear();
			}
			this.mShieldCompMgr = GameApp.gApp.LoadComposition("pax\\BossDoctorShields", "_BOSS_DOCTOR");
			this.mTikiCompMgr = GameApp.gApp.LoadComposition(GameApp.gApp.Is3DAccelerated() ? "pax\\Tikis" : "pax\\Tikis2D", "_BOSS_DOCTOR");
			List<Composition> list = new List<Composition>();
			this.mShieldCompMgr.GetAllCompositions(list);
			this.mTikiCompMgr.GetAllCompositions(list);
			for (int k = 0; k < list.Count; k++)
			{
				list[k].mLoop = true;
			}
			for (int m = 0; m < this.mTikis.Count; m++)
			{
				this.mTikis[m].mComp = this.mTikiCompMgr.GetComposition(this.mTikis[m].mIsLeftTiki ? "CircleTikiAnim" : "TriangleTikiAnim");
			}
			this.mTriangleExplosion = GameApp.gApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_TRIANGLEEXPLOSIONLINE").Duplicate();
			this.mCircleExplosion = GameApp.gApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_CIRCLEEXPLOSIONLINE").Duplicate();
			this.mTriangleExplosion.ResetAnim();
			this.mCircleExplosion.ResetAnim();
			this.mBandagedImg = Res.GetImageByID(ResID.IMAGE_BOSS_DOCTOR_BANDAGED);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001A4E8 File Offset: 0x000186E8
		public override Boss Instantiate()
		{
			BossDoctor bossDoctor = new BossDoctor(this.mLevel);
			int num = bossDoctor.mTutorialState;
			bossDoctor.CopyFrom(this);
			bossDoctor.mTutorialState = num;
			bossDoctor.mParticles.Clear();
			bossDoctor.mTikis.Clear();
			return bossDoctor;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001A530 File Offset: 0x00018730
		public void CopyFrom(BossDoctor rhs)
		{
			base.CopyFrom(rhs);
			this.mMaterializeTimer = rhs.mMaterializeTimer;
			this.mTikiCompMgr = rhs.mTikiCompMgr;
			this.mShieldCompMgr = rhs.mShieldCompMgr;
			this.mTriangleExplosion = rhs.mTriangleExplosion;
			this.mCircleExplosion = rhs.mCircleExplosion;
			this.mDoCircleExplosion = rhs.mDoCircleExplosion;
			this.mDoTriangleExplosion = rhs.mDoTriangleExplosion;
			this.mIsFiring = rhs.mIsFiring;
			this.mIsRetaliating = rhs.mIsRetaliating;
			this.mDoingHitAnim = rhs.mDoingHitAnim;
			this.mHairCel = rhs.mHairCel;
			this.mFaceCel = rhs.mFaceCel;
			this.mHitAnimUp = rhs.mHitAnimUp;
			this.mBossYOff = rhs.mBossYOff;
			this.mBerserkBaseAlpha = rhs.mBerserkBaseAlpha;
			this.mBerserkEyeFrame = rhs.mBerserkEyeFrame;
			this.mBerserkCounter = rhs.mBerserkCounter;
			this.mBerserkBaseAlphaDir = rhs.mBerserkBaseAlphaDir;
			this.mResGroup = rhs.mResGroup;
			this.mResPrefix = rhs.mResPrefix;
			this.mBossRadius = rhs.mBossRadius;
			this.mBulletRadius = rhs.mBulletRadius;
			this.mBandagedXOff = rhs.mBandagedXOff;
			this.mBandagedYOff = rhs.mBandagedYOff;
			this.mTutorialState = rhs.mTutorialState;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0001A670 File Offset: 0x00018870
		public override bool AllowFrogToFire()
		{
			if (this.mTutorialState == 0)
			{
				return base.AllowFrogToFire();
			}
			return this.mTutorialState == 1 && this.mLevel.HasReachedCruisingSpeed();
		}

		// Token: 0x040001FD RID: 509
		public static Image[] gBulletImages = new Image[2];

		// Token: 0x040001FE RID: 510
		public static int MAX_SPEAR_Y_OFF = -50;

		// Token: 0x040001FF RID: 511
		public static float MAX_ANGLE = 0.2617992f;

		// Token: 0x04000200 RID: 512
		public static int MIN_SPAWN_TIMER = 100;

		// Token: 0x04000201 RID: 513
		protected PIEffect mTriangleExplosion;

		// Token: 0x04000202 RID: 514
		protected PIEffect mCircleExplosion;

		// Token: 0x04000203 RID: 515
		protected List<PIEffect> mShieldZaps = new List<PIEffect>();

		// Token: 0x04000204 RID: 516
		protected CompositionMgr mTikiCompMgr;

		// Token: 0x04000205 RID: 517
		protected CompositionMgr mShieldCompMgr;

		// Token: 0x04000206 RID: 518
		protected DoctorStaff[] mStaff = new DoctorStaff[2];

		// Token: 0x04000207 RID: 519
		protected List<BossBulletParticleSystem> mParticles = new List<BossBulletParticleSystem>();

		// Token: 0x04000208 RID: 520
		protected bool mIsFiring;

		// Token: 0x04000209 RID: 521
		protected bool mIsRetaliating;

		// Token: 0x0400020A RID: 522
		protected bool mDoingHitAnim;

		// Token: 0x0400020B RID: 523
		protected bool mHitAnimUp;

		// Token: 0x0400020C RID: 524
		protected bool mDoCircleExplosion;

		// Token: 0x0400020D RID: 525
		protected bool mDoTriangleExplosion;

		// Token: 0x0400020E RID: 526
		protected int mHairCel;

		// Token: 0x0400020F RID: 527
		protected int mFaceCel;

		// Token: 0x04000210 RID: 528
		protected int mBerserkEyeFrame;

		// Token: 0x04000211 RID: 529
		protected int mBerserkCounter;

		// Token: 0x04000212 RID: 530
		protected int mTutorialState;

		// Token: 0x04000213 RID: 531
		protected int mMaterializeTimer;

		// Token: 0x04000214 RID: 532
		protected float mBossYOff;

		// Token: 0x04000215 RID: 533
		protected float mBerserkBaseAlpha;

		// Token: 0x04000216 RID: 534
		protected float mBerserkBaseAlphaDir;

		// Token: 0x02000021 RID: 33
		private enum ETutorial
		{
			// Token: 0x04000218 RID: 536
			Tutorial_None,
			// Token: 0x04000219 RID: 537
			Tutorial_StateNoShield,
			// Token: 0x0400021A RID: 538
			Tutorial_StatePhaseIn
		}
	}
}
