using System;
using System.Collections.Generic;
using SexyFramework;
using SexyFramework.AELib;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x02000032 RID: 50
	public class BossSquid : BossShoot
	{
		// Token: 0x060005AB RID: 1451 RVA: 0x0001D93B File Offset: 0x0001BB3B
		public static int FPS_ADJUST(float fps)
		{
			return (int)(fps * 100f / 30f);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001D94C File Offset: 0x0001BB4C
		protected override void DrawBossSpecificArt(Graphics g)
		{
			int num = (int)this.mX - this.mWidth / 2;
			int num2 = (int)this.mY - this.mHeight / 2;
			if (this.mAlphaOverride < 255f)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)this.mAlphaOverride);
			}
			g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_HEAD), Common._S(num + Common._M(40) + this.mShakeXOff), Common._S(num2 + Common._M1(11) + this.mShakeYOff));
			if (this.mHitTimer > 0)
			{
				if (this.mHitTimer < 255)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)Math.Min((float)this.mHitTimer, this.mAlphaOverride));
				}
				g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_HEAD_DARK), Common._S(num + Common._M(40) + this.mShakeXOff), Common._S(num2 + Common._M1(11) + this.mShakeYOff));
				g.SetColorizeImages(false);
			}
			if (this.mAlphaOverride < 255f)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)this.mAlphaOverride);
			}
			this.mBody.Draw(g, (float)Common._S(num + this.mShakeXOff), (float)Common._S(num2 + this.mShakeYOff));
			if (this.mState != 1)
			{
				this.mLeftArm.Draw(g, (float)Common._S(num + this.mShakeXOff + Common._M(0)), (float)Common._S(num2 + this.mShakeYOff + Common._M1(0)));
				this.mRightArm.Draw(g, (float)Common._S(num + this.mShakeXOff + Common._M(0)), (float)Common._S(num2 + this.mShakeYOff + Common._M1(0)));
			}
			else if (this.mState == 1)
			{
				if (this.mLeftThrowCel != -1)
				{
					g.DrawImageCel(Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_THROW_LEFT), Common._S(num + Common._M(3) + this.mShakeXOff), Common._S(num2 + Common._M1(45) + this.mShakeYOff), this.mLeftThrowCel);
				}
				else
				{
					this.mLeftArm.Draw(g, (float)Common._S(num + this.mShakeXOff + Common._M(0)), (float)Common._S(num2 + this.mShakeYOff + Common._M1(0)));
				}
				if (this.mRightThrowCel != -1)
				{
					g.DrawImageCel(Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_THROW_RIGHT), Common._S(num + Common._M(102) + this.mShakeXOff), Common._S(num2 + Common._M1(58) + this.mShakeYOff), this.mRightThrowCel);
				}
				else
				{
					this.mRightArm.Draw(g, (float)Common._S(num + this.mShakeXOff + Common._M(0)), (float)Common._S(num2 + this.mShakeYOff + Common._M1(0)));
				}
			}
			if (this.mHP > 0f)
			{
				if (this.mTeleportDir != 0)
				{
					g.PushState();
					g.ClearClipRect();
				}
				List<InkParticle> list = new List<InkParticle>();
				list.AddRange(this.mInk.ToArray());
				if (!this.mLevel.mBoard.IsPaused())
				{
					for (int i = 0; i < this.mBullets.Count; i++)
					{
						BossBullet bossBullet = this.mBullets[i];
						if (bossBullet.mData != null)
						{
							list.Add((InkParticle)bossBullet.mData);
						}
					}
				}
				Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_GLOBULE1);
				Image imageByID2 = Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_GLOBULE2);
				float[] array = new float[]
				{
					Common._M(0.75f),
					Common._M1(0.5f),
					Common._M2(0.84f)
				};
				float[] array2 = new float[]
				{
					(float)Common._M(20),
					(float)Common._M1(30),
					(float)Common._M2(30)
				};
				float[] array3 = new float[]
				{
					Common._M(0.5f),
					default(float),
					Common._M1(-0.5f)
				};
				this.mGlobalTranform.Reset();
				for (int j = 0; j < list.Count; j++)
				{
					InkParticle inkParticle = list[j];
					if (inkParticle.mAlpha != 255f)
					{
						g.SetColorizeImages(true);
						g.SetColor(255, 255, 255, (int)inkParticle.mAlpha);
					}
					if (inkParticle.mImage == imageByID && inkParticle.mPostHitCount < Common._M(5))
					{
						for (int k = 0; k < 3; k++)
						{
							this.mGlobalTranform.Reset();
							this.mGlobalTranform.Scale(inkParticle.mWidthPct * array[k], inkParticle.mHeightPct * array[k]);
							this.mGlobalTranform.RotateRad(inkParticle.mAngle);
							float num3 = inkParticle.mX + array2[k] * (float)Math.Cos((double)(inkParticle.mAngle + 1.5707964f + array3[k]));
							float num4 = inkParticle.mY - array2[k] * (float)Math.Sin((double)(inkParticle.mAngle + 1.5707964f + array3[k]));
							num3 += (float)inkParticle.mPostHitCount * (float)Math.Cos((double)(inkParticle.mAngle - 1.5707964f)) * inkParticle.mInitSpeed;
							num4 -= (float)inkParticle.mPostHitCount * (float)Math.Sin((double)(inkParticle.mAngle - 1.5707964f)) * inkParticle.mInitSpeed;
							g.DrawImageTransform(imageByID2, this.mGlobalTranform, Common._S(num3), Common._S(num4));
						}
					}
					this.mGlobalTranform.Reset();
					this.mGlobalTranform.Scale(inkParticle.mWidthPct, inkParticle.mHeightPct);
					this.mGlobalTranform.RotateRad(inkParticle.mAngle);
					g.DrawImageTransform(inkParticle.mImage, this.mGlobalTranform, Common._S(inkParticle.mX), Common._S(inkParticle.mY));
					g.SetColorizeImages(false);
				}
				if (this.mTeleportDir != 0)
				{
					g.PopState();
				}
			}
			Gun gun = this.mApp.GetBoard().GetGun();
			if (gun.IsInked())
			{
				Composition composition = this.mInkFrogComp.GetComposition("Main");
				if (!composition.Done())
				{
					CumulativeTransform cumulativeTransform = new CumulativeTransform();
					float num5 = Common._M(0.5f);
					cumulativeTransform.mTrans.Scale(num5, num5);
					cumulativeTransform.mTrans.RotateDeg(90f);
					cumulativeTransform.mTrans.Translate((float)Common._S(gun.GetCenterX() + Common._M(-146)), (float)Common._S(gun.GetCenterY() + Common._M1(200)));
					composition.Draw(g, cumulativeTransform, -1, Common._DS(1f));
				}
			}
			Image imageByID3 = Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_DARK_CLOUD);
			for (int l = 0; l < this.mInkClouds.Count; l++)
			{
				InkCloud inkCloud = this.mInkClouds[l];
				this.mGlobalTranform.Reset();
				this.mGlobalTranform.Scale(inkCloud.mSize * 4f, inkCloud.mSize * 4f);
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)inkCloud.mAlpha);
				g.DrawImageTransform(imageByID3, this.mGlobalTranform, Common._S(inkCloud.mX), Common._S(inkCloud.mY));
				g.SetColorizeImages(false);
			}
			if (this.mAlphaOverride < 255f)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)this.mAlphaOverride);
			}
			if (this.mJawCount > 0)
			{
				g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_MOUTH_OPEN), Common._S(num + Common._M(53) + this.mShakeXOff), Common._S(num2 + Common._M1(89) + this.mShakeYOff));
				if (this.mHitTimer > 0)
				{
					if (this.mHitTimer < 255)
					{
						g.SetColorizeImages(true);
						g.SetColor(255, 255, 255, (int)Math.Min((float)this.mHitTimer, this.mAlphaOverride));
					}
					g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_MOUTH_OPEN_DARK), Common._S(num + Common._M(51) + this.mShakeXOff), Common._S(num2 + Common._M1(88) + this.mShakeYOff));
					g.SetColorizeImages(false);
				}
			}
			if (this.mAlphaOverride < 255f)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)this.mAlphaOverride);
			}
			if (this.mBlinkCel >= 0)
			{
				g.DrawImageCel(Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_EYES), Common._S(num + Common._M(59) + this.mShakeXOff), Common._S(num2 + Common._M1(72) + this.mShakeYOff), this.mBlinkCel);
			}
			g.SetColorizeImages(false);
			if (this.mHP > 0f)
			{
				Image imageByID4 = Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_SQUIRT);
				for (int m = 0; m < this.mSweat.Count; m++)
				{
					SquidSweat squidSweat = this.mSweat[m];
					Rect celRect = imageByID4.GetCelRect(squidSweat.mCel);
					g.DrawImageRotated(imageByID4, (int)Common._S(squidSweat.mX), (int)Common._S(squidSweat.mY), (double)squidSweat.mAngle, celRect);
				}
			}
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001E308 File Offset: 0x0001C508
		protected override void DrawShield(Graphics g)
		{
			if (this.mHP <= 0f || this.mDoDeathExplosions || this.mLevel.mBoard.DoingBossIntro())
			{
				return;
			}
			for (int i = 0; i < Common._M(4); i++)
			{
				ShieldQuadrantData shieldQuadrantData = (ShieldQuadrantData)this.mShieldQuadrant[i].mData;
				if (this.mShieldQuadrant[i].mTimer <= 51 || shieldQuadrantData.mDoExplodeAnim)
				{
					float rot = this.mShieldAngle + (float)i * 3.14159f / 2f;
					float num = (float)Common._M(0);
					float num2 = (float)Common._M(0);
					if (!shieldQuadrantData.mDoExplodeAnim)
					{
						int mTimer = this.mShieldQuadrant[i].mTimer;
						Composition composition = shieldQuadrantData.mCompMgr.GetComposition("NormalQuad");
						CumulativeTransform cumulativeTransform = new CumulativeTransform();
						cumulativeTransform.mOpacity = ((mTimer > 0) ? ((float)(255 - mTimer * 5) / 255f) : 1f);
						if (!SexyFramework.Common._eq(this.mAlphaOverride, 255f))
						{
							cumulativeTransform.mOpacity = this.mAlphaOverride / 255f;
						}
						cumulativeTransform.mTrans.Translate((float)(-(float)composition.mWidth) * Common._DS(1f), (float)(-(float)composition.mHeight) * Common._DS(1f));
						cumulativeTransform.mTrans.RotateRad(rot);
						cumulativeTransform.mTrans.Translate(Common._S(this.mX - num2), Common._S(this.mY - num));
						composition.Draw(g, cumulativeTransform, -1, Common._DS(1f));
						if (g.Is3D())
						{
							g.PushState();
							if (!SexyFramework.Common._eq(this.mAlphaOverride, 255f))
							{
								shieldQuadrantData.mSparkles.mColor.mAlpha = (int)this.mAlphaOverride;
							}
							shieldQuadrantData.mSparkles.Draw(g);
							g.PopState();
						}
					}
					else
					{
						Composition composition2 = shieldQuadrantData.mCompMgr.GetComposition("ExplodeQuad");
						CumulativeTransform cumulativeTransform2 = new CumulativeTransform();
						cumulativeTransform2.mTrans.Translate((float)(-(float)composition2.mWidth) * Common._DS(1f), (float)(-(float)composition2.mHeight) * Common._DS(1f));
						cumulativeTransform2.mTrans.RotateRad(rot);
						cumulativeTransform2.mTrans.Translate(Common._S(this.mX - num2), Common._S(this.mY - num));
						if (!SexyFramework.Common._eq(this.mAlphaOverride, 255f))
						{
							cumulativeTransform2.mOpacity = this.mAlphaOverride / 255f;
						}
						composition2.Draw(g, cumulativeTransform2, -1, Common._DS(1f));
					}
					if (shieldQuadrantData.mDoHitAnim)
					{
						Composition composition3 = shieldQuadrantData.mCompMgr.GetComposition("HitQuad");
						CumulativeTransform cumulativeTransform3 = new CumulativeTransform();
						cumulativeTransform3.mTrans.Translate((float)(-(float)composition3.mWidth) * Common._DS(1f), (float)(-(float)composition3.mHeight) * Common._DS(1f));
						cumulativeTransform3.mTrans.RotateRad(rot);
						cumulativeTransform3.mTrans.Translate(Common._S(this.mX - num2), Common._S(this.mY - num));
						if (!SexyFramework.Common._eq(this.mAlphaOverride, 255f))
						{
							cumulativeTransform3.mOpacity = this.mAlphaOverride / 255f;
						}
						composition3.Draw(g, cumulativeTransform3, -1, Common._DS(1f));
					}
				}
			}
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001E675 File Offset: 0x0001C875
		public override void DrawBelowBalls(Graphics g)
		{
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001E678 File Offset: 0x0001C878
		protected override bool PreBulletUpdate(BossBullet b, int index)
		{
			if (b.mState == 0)
			{
				if (this.mLeftThrowCel == -1 && this.mRightThrowCel == -1)
				{
					if ((float)this.mLevel.mFrog.GetCenterX() > this.mX)
					{
						this.mLeftThrowCel = 0;
						b.mState = -1;
					}
					else
					{
						this.mRightThrowCel = 0;
						b.mState = 1;
					}
				}
				else if (this.mLeftThrowCel == -1)
				{
					this.mLeftThrowCel = 0;
					b.mState = -1;
				}
				else
				{
					if (this.mRightThrowCel != -1)
					{
						return true;
					}
					this.mLeftThrowCel = 0;
					b.mState = 1;
				}
				this.mState = 1;
				b.mVX = (b.mVY = 0f);
			}
			else if (Math.Abs(b.mState) == 2)
			{
				b.mState += Math.Sign(b.mState);
				b.mX = this.mX - (float)(this.mWidth / 2) + (float)((b.mState < 0) ? Common._M(15) : Common._M1(153));
				b.mY = this.mY - (float)(this.mHeight / 2) + (float)((b.mState < 0) ? Common._M(105) : Common._M1(105)) + b.mY;
				float num = 0f;
				if (b.mShotType == 1)
				{
					num = base.FireBulletAtPlayer(b, SexyFramework.Common.FloatRange(base.mMinBulletSpeed, base.mMaxBulletSpeed), b.mX, b.mY);
					b.mTargetVX = b.mVX;
					b.mTargetVY = b.mVY;
				}
				else if (b.mShotType == 0)
				{
					b.mVY = SexyFramework.Common.FloatRange(base.mMinBulletSpeed, base.mMaxBulletSpeed);
				}
				InkParticle inkParticle = new InkParticle();
				b.mData = inkParticle;
				inkParticle.mX = b.mX;
				inkParticle.mY = b.mY;
				inkParticle.mWidthPct = Common._M(0.57f);
				inkParticle.mHeightPct = Common._M(0.27f);
				inkParticle.mAngle = ((b.mShotType == 0) ? (-3.1415927f / Common._M(2f)) : num);
				if (b.mVX < 0f)
				{
					inkParticle.mAngle += 3.1415927f * Common._M(0.5f);
				}
				else
				{
					inkParticle.mAngle += 3.1415927f * Common._M(0.5f);
				}
				inkParticle.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_GLOBULE1);
				inkParticle.mVX = (inkParticle.mVY = (inkParticle.mGravity = 0f));
				inkParticle.mAlpha = 255f;
				inkParticle.mAlphaRate = 0f;
				inkParticle.mInitSpeed = b.mInitialSpeed;
			}
			return b.mState == 0;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001E93C File Offset: 0x0001CB3C
		protected override Rect GetBulletRect(BossBullet b)
		{
			int num = Common._M(17);
			int num2 = Common._M(30);
			return new Rect((int)b.mX - num / 2, (int)b.mY - num2 / 2, num, num2);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001E978 File Offset: 0x0001CB78
		protected override bool DoHit(Bullet b, bool from_prox_bomb)
		{
			bool flag = base.DoHit(b, from_prox_bomb);
			if (from_prox_bomb)
			{
				if (from_prox_bomb && !this.mHasBeenHitByProxBomb && !this.mApp.IsHardMode() && this.mApp.mUserProfile.GetAdvModeVars().mNumTimesZoneBeat[4] == 0)
				{
					this.mHasBeenHitByProxBomb = true;
					this.mState = 2;
					this.mApp.GetBoard().mLevel.FadeInkSpots();
					this.mApp.GetBoard().mPreventBallAdvancement = true;
					if (this.mTauntQueue.Count > 1)
					{
						this.mTauntQueue.RemoveRange(1, this.mTauntQueue.Count - 1);
					}
					TauntText tauntText = new TauntText();
					this.mTauntQueue.Add(tauntText);
					tauntText.mText = TextManager.getInstance().getString(391);
					tauntText.mDelay = Common._M(300);
					tauntText.mTextId = 391;
					tauntText = new TauntText();
					this.mTauntQueue.Add(tauntText);
					tauntText.mText = TextManager.getInstance().getString(392);
					tauntText.mDelay = Common._M(1000);
					tauntText.mTextId = 392;
					this.mPauseMovement = true;
					this.mPauseShieldRegen = true;
				}
				else if (from_prox_bomb && this.mApp.mUserProfile.GetAdvModeVars().mNumTimesZoneBeat[4] > 0)
				{
					this.mHasBeenHitByProxBomb = true;
				}
				return flag;
			}
			this.mHitTimer = Common._M(300);
			if (flag && this.mState == 2)
			{
				this.mState = 0;
				this.mPauseShieldRegen = (this.mPauseMovement = false);
				base.mShieldPauseTime = 0;
				this.mApp.GetBoard().mPreventBallAdvancement = false;
				for (int i = 0; i < 4; i++)
				{
					this.mShieldQuadrant[i].mTimer = 0;
					this.mShieldQuadrant[i].mHP = base.mShieldHP;
				}
			}
			return flag;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001EB60 File Offset: 0x0001CD60
		protected override void BulletHitPlayer(BossBullet b)
		{
			if (b.mData != null)
			{
				InkParticle inkParticle = (InkParticle)b.mData;
				b.mData = null;
				this.mInk.Add(inkParticle);
				int num = Common._M(8);
				float num2 = SexyFramework.Common.DegreesToRadians((float)Common._M(45));
				float num3 = SexyFramework.Common.DegreesToRadians((float)Common._M(235));
				float num4 = (num3 - num2) / (float)num;
				Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_GLOBULE2);
				for (int i = 0; i < num; i++)
				{
					InkParticle inkParticle2 = new InkParticle();
					this.mInk.Add(inkParticle2);
					inkParticle2.mImage = imageByID;
					inkParticle2.mX = inkParticle.mX;
					inkParticle2.mY = inkParticle.mY;
					inkParticle2.mWidthPct = (inkParticle2.mHeightPct = SexyFramework.Common.FloatRange(Common._M(0.25f), 0.75f));
					inkParticle2.mGravity = SexyFramework.Common.FloatRange(Common._M(0.08f), Common._M1(0.13f));
					inkParticle2.mAngle = num2 + num4 * (float)i;
					float num5 = Common._M(3.5f);
					inkParticle2.mVX = num5 * (float)Math.Cos((double)inkParticle2.mAngle);
					inkParticle2.mVY = -num5 * (float)Math.Sin((double)inkParticle2.mAngle);
					inkParticle2.mAlpha = 255f;
					inkParticle2.mAlphaRate = Common._M(2f);
					inkParticle2.mJiggleDir = ((i % 2 == 0) ? 1 : (-1));
					inkParticle2.mJiggleRate = SexyFramework.Common.FloatRange(Common._M(0.02f), Common._M1(0.03f));
				}
				InkCloud inkCloud = new InkCloud();
				this.mInkClouds.Add(inkCloud);
				inkCloud.mAlpha = 0f;
				inkCloud.mFadeIn = true;
				inkCloud.mSize = Common._M(0.2f);
				inkCloud.mX = b.mX;
				inkCloud.mY = b.mY;
				this.mInkFrogComp.GetComposition("Main").Reset();
				this.mApp.GetBoard().GetGun().DoInkedState();
			}
			if (!this.mApp.GetLevelMgr().mBossesCanAttackFuckedFrog)
			{
				for (int j = 0; j < this.mBullets.Count; j++)
				{
					BossBullet bossBullet = this.mBullets[j];
					if (bossBullet.mData == null)
					{
						bossBullet.mDeleteInstantly = true;
					}
				}
			}
			base.BulletHitPlayer(b);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001EDD0 File Offset: 0x0001CFD0
		protected override void ShieldQuadrantHit(int quad)
		{
			ShieldQuadrantData shieldQuadrantData = (ShieldQuadrantData)this.mShieldQuadrant[quad].mData;
			shieldQuadrantData.mDoHitAnim = true;
			base.PlaySound(9);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001EDFF File Offset: 0x0001CFFF
		protected override void BossBulletDestroyed(BossBullet b, bool outofscreen)
		{
			b.mData = null;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001EE08 File Offset: 0x0001D008
		protected override bool CanFire()
		{
			return this.mApp.mUserProfile.GetAdvModeVars().mNumTimesZoneBeat[4] > 0 || this.mHasBeenHitByProxBomb || this.mApp.IsHardMode();
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001EE3E File Offset: 0x0001D03E
		protected override void QuadHitByProxBomb(int quad)
		{
			((ShieldQuadrantData)this.mShieldQuadrant[quad].mData).mDoExplodeAnim = true;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001EE58 File Offset: 0x0001D058
		public BossSquid(Level l)
			: base(l)
		{
			this.mShieldRadius = Common._M(100);
			this.mProxBombRadius = Common._M(140);
			this.mResGroup = "Boss5";
			this.mResPrefix = "IMAGE_BOSS_SQUID_";
			this.mBossRadius = Common._M(70);
			this.mDrawHeartsBelowMisc = false;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001EF2D File Offset: 0x0001D12D
		public BossSquid()
			: this(null)
		{
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001EF38 File Offset: 0x0001D138
		public override void Dispose()
		{
			base.Dispose();
			if (this.mInkFrogComp != null)
			{
				this.mInkFrogComp = null;
			}
			if (this.mApp.mResourceManager.IsGroupLoaded("Underwater"))
			{
				this.mApp.mResourceManager.DeleteResources("Underwater");
			}
			for (int i = 0; i < this.mBullets.Count; i++)
			{
				this.mBullets[i].mData = null;
			}
			for (int j = 0; j < this.mInk.Count; j++)
			{
				this.mInk[j] = null;
			}
			for (int k = 0; k < 4; k++)
			{
				this.mShieldQuadrant[k].mData = null;
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001EFEC File Offset: 0x0001D1EC
		protected void CopyFrom(BossSquid rhs)
		{
			base.CopyFrom(rhs);
			this.mBody = new SquidAnim(rhs.mBody);
			this.mLeftArm = new SquidAnim(rhs.mLeftArm);
			this.mRightArm = new SquidAnim(rhs.mRightArm);
			this.mHasBeenHitByProxBomb = rhs.mHasBeenHitByProxBomb;
			this.mState = rhs.mState;
			this.mJawCount = rhs.mJawCount;
			this.mJawDelay = rhs.mJawDelay;
			this.mBlinkDelay = rhs.mBlinkDelay;
			this.mBlinkCel = rhs.mBlinkCel;
			this.mLeftThrowCel = rhs.mLeftThrowCel;
			this.mRightThrowCel = rhs.mRightThrowCel;
			this.mHitTimer = rhs.mHitTimer;
			this.mTutorialShieldRotateAmt = rhs.mTutorialShieldRotateAmt;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001F0AC File Offset: 0x0001D2AC
		public override void SyncState(DataSync sync)
		{
			base.SyncState(sync);
			SexyBuffer buffer = sync.GetBuffer();
			sync.SyncBoolean(ref this.mHasBeenHitByProxBomb);
			sync.SyncLong(ref this.mState);
			this.SyncListInkClouds(sync, this.mInkClouds, true);
			this.SyncListInkParticles(sync, this.mInk, true);
			for (int i = 0; i < 4; i++)
			{
				ShieldQuadrantData shieldQuadrantData = (ShieldQuadrantData)this.mShieldQuadrant[i].mData;
				sync.SyncBoolean(ref shieldQuadrantData.mDoHitAnim);
				sync.SyncBoolean(ref shieldQuadrantData.mDoExplodeAnim);
			}
			for (int j = 0; j < this.mBullets.Count; j++)
			{
				if (sync.isWrite())
				{
					InkParticle inkParticle = (InkParticle)this.mBullets[j].mData;
					if (inkParticle == null)
					{
						buffer.WriteBoolean(false);
					}
					else
					{
						buffer.WriteBoolean(true);
						inkParticle.SyncState(sync);
					}
				}
				else if (buffer.ReadBoolean())
				{
					InkParticle inkParticle2 = new InkParticle();
					inkParticle2.SyncState(sync);
					this.mBullets[j].mData = inkParticle2;
				}
			}
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001F1B0 File Offset: 0x0001D3B0
		private void SyncListInkClouds(DataSync sync, List<InkCloud> theList, bool clear)
		{
			if (sync.isRead())
			{
				if (clear)
				{
					theList.Clear();
				}
				long num = sync.GetBuffer().ReadLong();
				int num2 = 0;
				while ((long)num2 < num)
				{
					InkCloud inkCloud = new InkCloud();
					inkCloud.SyncState(sync);
					theList.Add(inkCloud);
					num2++;
				}
				return;
			}
			sync.GetBuffer().WriteLong((long)theList.Count);
			foreach (InkCloud inkCloud2 in theList)
			{
				inkCloud2.SyncState(sync);
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001F250 File Offset: 0x0001D450
		private void SyncListInkParticles(DataSync sync, List<InkParticle> theList, bool clear)
		{
			if (sync.isRead())
			{
				if (clear)
				{
					theList.Clear();
				}
				long num = sync.GetBuffer().ReadLong();
				int num2 = 0;
				while ((long)num2 < num)
				{
					InkParticle inkParticle = new InkParticle();
					inkParticle.SyncState(sync);
					theList.Add(inkParticle);
					num2++;
				}
				return;
			}
			sync.GetBuffer().WriteLong((long)theList.Count);
			foreach (InkParticle inkParticle2 in theList)
			{
				inkParticle2.SyncState(sync);
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001F2F0 File Offset: 0x0001D4F0
		public override void Update(float f)
		{
			base.Update(f);
			if (this.mDoDeathExplosions || this.mHP <= 0f || this.mLevel.mBoard.DoingBossIntro())
			{
				return;
			}
			this.mBody.Update();
			this.mRightArm.Update();
			this.mLeftArm.Update();
			if (this.mApp.GetBoard().GetGun().IsInked())
			{
				Composition composition = this.mInkFrogComp.GetComposition("Main");
				if (!composition.Done())
				{
					composition.Update();
				}
			}
			if (this.mHitTimer > 0)
			{
				this.mHitTimer--;
				if (this.mUpdateCount % Common._M(20) == 0 && this.mHitTimer > Common._M1(128))
				{
					int num = 2 + SexyFramework.Common.Rand() % Common._M(1);
					for (int i = 0; i < num; i++)
					{
						SquidSweat squidSweat = new SquidSweat();
						this.mSweat.Add(squidSweat);
						squidSweat.mAngle = SexyFramework.Common.DegreesToRadians((float)(Common._M(45) + SexyFramework.Common.Rand() % Common._M1(90)));
						float num2 = SexyFramework.Common.FloatRange(Common._M(2.5f), Common._M1(3f));
						squidSweat.mVX = (float)Math.Cos((double)squidSweat.mAngle) * num2;
						squidSweat.mVY = -(float)Math.Sin((double)squidSweat.mAngle) * num2;
						squidSweat.mX = this.mX - (float)Common._M(30);
						squidSweat.mY = this.mY - (float)Common._M(70);
						squidSweat.mX += squidSweat.mVX * (float)Common._M(20);
						squidSweat.mY += squidSweat.mVY * (float)Common._M(0);
					}
				}
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_SQUIRT);
			for (int j = 0; j < this.mSweat.Count; j++)
			{
				SquidSweat squidSweat2 = this.mSweat[j];
				squidSweat2.mX += squidSweat2.mVX;
				squidSweat2.mY += squidSweat2.mVY;
				if (this.mUpdateCount % Common._M(6) == 0 && ++squidSweat2.mCel >= imageByID.mNumCols)
				{
					this.mSweat.RemoveAt(j);
					j--;
				}
			}
			if (this.mState == 1)
			{
				int[] array = new int[] { this.mLeftThrowCel, this.mRightThrowCel };
				Image[] array2 = new Image[]
				{
					Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_THROW_LEFT),
					Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_THROW_RIGHT)
				};
				for (int k = 0; k < 2; k++)
				{
					if (array[k] >= 0 && this.mUpdateCount % Common._M(6) == 0)
					{
						if (++array[k] >= array2[k].mNumCols)
						{
							array[k] = -1;
						}
						if (array[k] == 1)
						{
							for (int l = 0; l < this.mBullets.Count; l++)
							{
								BossBullet bossBullet = this.mBullets[l];
								if (bossBullet.mState == ((k == 0) ? (-1) : 1))
								{
									bossBullet.mState = ((k == 0) ? (-2) : 2);
									bossBullet.mX = (bossBullet.mY = 0f);
									break;
								}
							}
						}
					}
				}
				this.mLeftThrowCel = array[0];
				this.mRightThrowCel = array[1];
				if (this.mLeftThrowCel == this.mRightThrowCel && this.mLeftThrowCel == -1)
				{
					this.mState = 0;
				}
			}
			else
			{
				int num3 = this.mState;
			}
			float num4 = Common._M(0.7f);
			float num5 = Common._M(0.8f);
			for (int m = 0; m < this.mBullets.Count; m++)
			{
				if (this.mBullets[m].mData != null)
				{
					InkParticle inkParticle = (InkParticle)this.mBullets[m].mData;
					float num6 = Common._M(0.04f);
					float num7 = Common._M(0.04f);
					inkParticle.mWidthPct += num6;
					inkParticle.mHeightPct += num7;
					if (inkParticle.mWidthPct > num4)
					{
						inkParticle.mWidthPct = num4;
					}
					if (inkParticle.mHeightPct > num5)
					{
						inkParticle.mHeightPct = num5;
					}
					inkParticle.mX = this.mBullets[m].mX;
					inkParticle.mY = this.mBullets[m].mY;
				}
			}
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_GLOBULE1);
			for (int n = 0; n < this.mInk.Count; n++)
			{
				InkParticle inkParticle2 = this.mInk[n];
				if (inkParticle2.mImage == imageByID2)
				{
					inkParticle2.mPostHitCount++;
					float num8 = Common._M(1.11f);
					float num9 = Common._M(0f);
					float num10 = (num8 - num4) / Common._M(10f);
					float num11 = (num9 - num5) / Common._M(10f);
					inkParticle2.mWidthPct += num10;
					inkParticle2.mHeightPct += num11;
					if (inkParticle2.mWidthPct > num8)
					{
						inkParticle2.mWidthPct = num8;
					}
					if (inkParticle2.mHeightPct < num9)
					{
						inkParticle2.mHeightPct = num9;
					}
					if (inkParticle2.mWidthPct >= num8 && inkParticle2.mHeightPct <= num9)
					{
						this.mInk.RemoveAt(n);
						n--;
					}
				}
				else
				{
					inkParticle2.mX += inkParticle2.mVX;
					inkParticle2.mY += inkParticle2.mVY;
					inkParticle2.mVY += inkParticle2.mGravity;
					inkParticle2.mAlpha -= inkParticle2.mAlphaRate;
					if (inkParticle2.mJiggleDir > 0)
					{
						inkParticle2.mWidthPct += inkParticle2.mJiggleRate;
						inkParticle2.mHeightPct += inkParticle2.mJiggleRate;
						if (inkParticle2.mWidthPct > 0.75f || inkParticle2.mHeightPct > 0.75f)
						{
							inkParticle2.mWidthPct = (inkParticle2.mHeightPct = 0.75f);
							inkParticle2.mJiggleDir *= -1;
						}
					}
					else
					{
						inkParticle2.mWidthPct -= inkParticle2.mJiggleRate;
						inkParticle2.mHeightPct -= inkParticle2.mJiggleRate;
						if (inkParticle2.mWidthPct < 0.25f || inkParticle2.mHeightPct < 0.25f)
						{
							inkParticle2.mWidthPct = (inkParticle2.mHeightPct = 0.25f);
							inkParticle2.mJiggleDir *= -1;
						}
					}
					if (inkParticle2.mAlpha <= 0f)
					{
						this.mInk.RemoveAt(n);
						n--;
					}
				}
			}
			for (int num12 = 0; num12 < this.mInkClouds.Count; num12++)
			{
				InkCloud inkCloud = this.mInkClouds[num12];
				inkCloud.mSize += Common._M(0.004f);
				if (inkCloud.mFadeIn)
				{
					inkCloud.mAlpha += Common._M(5f);
					if (inkCloud.mAlpha >= 255f)
					{
						inkCloud.mAlpha = 255f;
						inkCloud.mFadeIn = false;
					}
				}
				else
				{
					inkCloud.mAlpha -= Common._M(4f);
					if (inkCloud.mAlpha <= 0f)
					{
						this.mInkClouds.RemoveAt(num12);
						num12--;
					}
				}
			}
			if (this.mJawCount > 0)
			{
				if (--this.mJawCount == 0)
				{
					this.mJawDelay = Common._M(400) + SexyFramework.Common.Rand() % Common._M1(400);
				}
			}
			else if (this.mUpdateCount % this.mJawDelay == 0)
			{
				this.mJawCount = Common._M(20) + SexyFramework.Common.Rand() % Common._M1(50);
			}
			if (this.mBlinkCel < 0 && this.mUpdateCount % this.mBlinkDelay == 0)
			{
				this.mBlinkCel = 0;
				this.mBlinkDelay = Common._M(300) + SexyFramework.Common.Rand() % Common._M1(200);
			}
			else if (this.mBlinkCel >= 0 && this.mUpdateCount % Common._M(15) == 0 && ++this.mBlinkCel >= Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_EYES).mNumCols)
			{
				this.mBlinkCel = -1;
			}
			for (int num13 = 0; num13 < 4; num13++)
			{
				ShieldQuadrantData shieldQuadrantData = (ShieldQuadrantData)this.mShieldQuadrant[num13].mData;
				float rot = this.mShieldAngle + (float)num13 * 3.14159f / 2f;
				int num14 = Common._DS(Common._M(200));
				shieldQuadrantData.mSparkles.mDrawTransform.LoadIdentity();
				float num15 = GameApp.DownScaleNum(1f);
				shieldQuadrantData.mSparkles.mDrawTransform.Scale(num15, num15);
				shieldQuadrantData.mSparkles.mDrawTransform.Translate(0f, (float)(-(float)num14));
				shieldQuadrantData.mSparkles.mDrawTransform.RotateRad(rot);
				shieldQuadrantData.mSparkles.mDrawTransform.Translate(Common._S(this.mX), (float)num14 + Common._S(this.mY - (float)Common._M(100)));
				shieldQuadrantData.mSparkles.Update();
				if (!shieldQuadrantData.mDoExplodeAnim && this.mShieldQuadrant[num13].mTimer <= 0)
				{
					Composition composition2 = shieldQuadrantData.mCompMgr.GetComposition("NormalQuad");
					composition2.mLoop = true;
					composition2.Update();
				}
				else if (shieldQuadrantData.mDoExplodeAnim)
				{
					Composition composition3 = shieldQuadrantData.mCompMgr.GetComposition("ExplodeQuad");
					composition3.Update();
					if (composition3.Done())
					{
						shieldQuadrantData.mDoExplodeAnim = false;
						composition3.Reset();
					}
				}
				if (shieldQuadrantData.mDoHitAnim)
				{
					Composition composition4 = shieldQuadrantData.mCompMgr.GetComposition("HitQuad");
					composition4.Update();
					if (composition4.Done())
					{
						shieldQuadrantData.mDoHitAnim = false;
						composition4.Reset();
					}
				}
			}
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001FD88 File Offset: 0x0001DF88
		public override void Init(Level l)
		{
			this.mWidth = Common._M(185);
			this.mHeight = Common._M(172);
			base.Init(l);
			if (!this.mApp.mResourceManager.IsGroupLoaded("Underwater") && !this.mApp.mResourceManager.LoadResources("Underwater"))
			{
				this.mApp.ShowResourceError(true);
				this.mApp.Shutdown();
				return;
			}
			this.mBandagedImg = Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_BANDAGED);
			for (int i = 0; i < 4; i++)
			{
				this.mShieldQuadrant[i].mData = new ShieldQuadrantData(this.mApp.LoadComposition("pax\\SquidBossShields", "_BOSS_SQUID"), this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_SHIELDSPARKLES").Duplicate());
				ShieldQuadrantData shieldQuadrantData = (ShieldQuadrantData)this.mShieldQuadrant[i].mData;
				shieldQuadrantData.mSparkles.mEmitAfterTimeline = true;
			}
			this.mInkFrogComp = this.mApp.LoadComposition("pax\\ink frog", "_BOSS_SQUID");
			this.mBody.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_BODY);
			this.mBody.mX = 42f;
			this.mBody.mY = 102f;
			this.mBody.AddAnimInfo(0, BossSquid.FPS_ADJUST(3f));
			this.mBody.AddAnimInfo(1, BossSquid.FPS_ADJUST(3f));
			this.mBody.AddAnimInfo(2, BossSquid.FPS_ADJUST(5f));
			this.mBody.AddAnimInfo(1, BossSquid.FPS_ADJUST(3f));
			this.mBody.AddAnimInfo(0, BossSquid.FPS_ADJUST(10f));
			this.mLeftArm.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_LEG_LEFT);
			this.mLeftArm.mX = 7f;
			this.mLeftArm.mY = 43f;
			this.mLeftArm.AddAnimInfo(0, BossSquid.FPS_ADJUST(2f));
			this.mLeftArm.AddAnimInfo(1, BossSquid.FPS_ADJUST(2f));
			this.mLeftArm.AddAnimInfo(2, BossSquid.FPS_ADJUST(2f));
			this.mLeftArm.AddAnimInfo(3, BossSquid.FPS_ADJUST(5f));
			this.mLeftArm.AddAnimInfo(2, BossSquid.FPS_ADJUST(2f));
			this.mLeftArm.AddAnimInfo(1, BossSquid.FPS_ADJUST(2f));
			this.mLeftArm.AddAnimInfo(0, BossSquid.FPS_ADJUST(2f));
			this.mLeftArm.AddAnimInfo(4, BossSquid.FPS_ADJUST(5f));
			this.mRightArm.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_LEG_RIGHT);
			this.mRightArm.mX = 102f;
			this.mRightArm.mY = 58f;
			this.mRightArm.AddAnimInfo(0, BossSquid.FPS_ADJUST(2f));
			this.mRightArm.AddAnimInfo(1, BossSquid.FPS_ADJUST(2f));
			this.mRightArm.AddAnimInfo(2, BossSquid.FPS_ADJUST(2f));
			this.mRightArm.AddAnimInfo(3, BossSquid.FPS_ADJUST(2f));
			this.mRightArm.AddAnimInfo(4, BossSquid.FPS_ADJUST(5f));
			this.mRightArm.AddAnimInfo(3, BossSquid.FPS_ADJUST(2f));
			this.mRightArm.AddAnimInfo(2, BossSquid.FPS_ADJUST(2f));
			this.mRightArm.AddAnimInfo(1, BossSquid.FPS_ADJUST(2f));
			this.mRightArm.AddAnimInfo(5, BossSquid.FPS_ADJUST(2f));
			this.mRightArm.AddAnimInfo(6, BossSquid.FPS_ADJUST(2f));
			this.mRightArm.AddAnimInfo(5, BossSquid.FPS_ADJUST(2f));
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00020148 File Offset: 0x0001E348
		public override Boss Instantiate()
		{
			BossSquid bossSquid = new BossSquid(this.mLevel);
			bossSquid.CopyFrom(this);
			return bossSquid;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0002016C File Offset: 0x0001E36C
		protected override void ReInit()
		{
			base.ReInit();
			for (int i = 0; i < 4; i++)
			{
				this.mShieldQuadrant[i].mTimer = 0;
				this.mShieldQuadrant[i].mHP = base.mShieldHP;
			}
		}

		// Token: 0x0400028E RID: 654
		public CompositionMgr mInkFrogComp;

		// Token: 0x0400028F RID: 655
		protected SquidAnim mBody = new SquidAnim();

		// Token: 0x04000290 RID: 656
		protected SquidAnim mLeftArm = new SquidAnim();

		// Token: 0x04000291 RID: 657
		protected SquidAnim mRightArm = new SquidAnim();

		// Token: 0x04000292 RID: 658
		protected List<SquidSweat> mSweat = new List<SquidSweat>();

		// Token: 0x04000293 RID: 659
		protected List<InkParticle> mInk = new List<InkParticle>();

		// Token: 0x04000294 RID: 660
		protected List<InkCloud> mInkClouds = new List<InkCloud>();

		// Token: 0x04000295 RID: 661
		protected bool mHasBeenHitByProxBomb;

		// Token: 0x04000296 RID: 662
		protected int mState;

		// Token: 0x04000297 RID: 663
		protected int mJawCount;

		// Token: 0x04000298 RID: 664
		protected int mJawDelay = 1 + SexyFramework.Common.SafeRand() % 50;

		// Token: 0x04000299 RID: 665
		protected int mBlinkDelay = 1 + SexyFramework.Common.SafeRand() % 150;

		// Token: 0x0400029A RID: 666
		protected int mBlinkCel = -1;

		// Token: 0x0400029B RID: 667
		protected int mLeftThrowCel = -1;

		// Token: 0x0400029C RID: 668
		protected int mRightThrowCel = -1;

		// Token: 0x0400029D RID: 669
		protected int mHitTimer;

		// Token: 0x0400029E RID: 670
		protected float mTutorialShieldRotateAmt;

		// Token: 0x02000033 RID: 51
		public enum State
		{
			// Token: 0x040002A0 RID: 672
			State_Idle,
			// Token: 0x040002A1 RID: 673
			State_Throwing,
			// Token: 0x040002A2 RID: 674
			State_Tutorial
		}
	}
}
