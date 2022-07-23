using System;
using System.Collections.Generic;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x0200002A RID: 42
	public class BossMosquito : BossShoot
	{
		// Token: 0x06000587 RID: 1415 RVA: 0x0001C360 File Offset: 0x0001A560
		protected override void DrawBossSpecificArt(Graphics g)
		{
			if (this.mAlphaOverride < 255f)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)this.mAlphaOverride);
			}
			Image imageByID;
			if (this.mHitTimer > 0)
			{
				imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_MOSQUITO_HIT);
			}
			else if (this.mThrowTimer > 0)
			{
				imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_MOSQUITO_THROW);
			}
			else
			{
				imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_MOSQUITO_IDLE);
			}
			g.DrawImage(imageByID, (int)(Common._S(this.mX) - (float)(imageByID.mWidth / 2) + (float)Common._S(this.mShakeXOff)), (int)(Common._S(this.mY) - (float)(imageByID.mHeight / 2) + (float)Common._S(this.mShakeYOff)));
			Image[] array = new Image[]
			{
				Res.GetImageByID(ResID.IMAGE_BOSS_MOSQUITO_EYES_WIDE),
				Res.GetImageByID(ResID.IMAGE_BOSS_MOSQUITO_EYES_HALF),
				Res.GetImageByID(ResID.IMAGE_BOSS_MOSQUITO_EYES_CLOSED)
			};
			if (this.mHitTimer <= 0)
			{
				g.DrawImage(array[this.mEyeFrame], (int)(Common._S(this.mX) - (float)(imageByID.mWidth / 2) + (float)Common._S(this.mShakeXOff + Common._M(37))), (int)(Common._S(this.mY) - (float)(imageByID.mHeight / 2) + (float)Common._S(this.mShakeYOff + Common._M1(60))));
			}
			if (this.mHP > 0f && !this.mDoDeathExplosions)
			{
				if (this.mTeleportDir != 0)
				{
					g.PushState();
					g.ClearClipRect();
				}
				if (!this.mLevel.mBoard.IsPaused())
				{
					for (int i = 0; i < this.mBullets.Count; i++)
					{
						if (this.mBullets[i].mDelay <= 0)
						{
							BossBullet bossBullet = this.mBullets[i];
							MosquitoBall mosquitoBall = (MosquitoBall)bossBullet.mData;
							for (int j = 0; j < mosquitoBall.mMosquitoes.Count; j++)
							{
								Mosquito mosquito = mosquitoBall.mMosquitoes[j];
								g.DrawImage(mosquito.mImage, (int)(Common._S(bossBullet.mX - (float)Common._M(20) + mosquito.mRadius * (float)Math.Cos((double)mosquito.mAngle)) - (float)(mosquito.mImage.mWidth / 2)), (int)(Common._S(bossBullet.mY - (float)Common._M1(20) - mosquito.mRadius * (float)Math.Sin((double)mosquito.mAngle)) - (float)(mosquito.mImage.mHeight / 2)));
							}
						}
					}
				}
				if (this.mTeleportDir != 0)
				{
					g.PopState();
				}
				if (this.mChewing)
				{
					Image imageByID2 = Res.GetImageByID(ResID.IMAGE_BOSS_MOSQUITO_CHEW);
					g.DrawImageCel(imageByID2, (int)(Common._S(this.mX) - (float)(imageByID2.GetCelWidth() / 2) + (float)Common._S(this.mShakeXOff + Common._M(0))), (int)(Common._S(this.mY) - (float)(imageByID2.GetCelHeight() / 2) + (float)Common._S(this.mShakeYOff + Common._M1(0))), this.mChewFrame);
				}
				if (this.mBallType != -1)
				{
					ResID id = ResID.IMAGE_BLUE_BALL + this.mBallType;
					if (this.mApp.mColorblind && this.mBallType == 3)
					{
						id = ResID.IMAGE_GREEN_BALL_CBM;
					}
					else if (this.mApp.mColorblind && this.mBallType == 4)
					{
						id = ResID.IMAGE_PURPLE_BALL_CBM;
					}
					Image imageByID3 = Res.GetImageByID(id);
					int theWidth = (int)((float)imageByID3.GetCelWidth() * this.mBallSize);
					int theHeight = (int)((float)imageByID3.GetCelHeight() * this.mBallSize);
					Rect theDestRect = new Rect((int)Common._S(this.mBallX - (float)Common.GetDefaultBallRadius()), (int)Common._S(this.mBallY - (float)Common.GetDefaultBallRadius()), theWidth, theHeight);
					g.DrawImage(imageByID3, theDestRect, imageByID3.GetCelRect(this.mBallCel));
				}
				Image imageByID4 = Res.GetImageByID(ResID.IMAGE_BOSS_MOSQUITO_ROCK);
				for (int k = 0; k < this.mRockParticles.Count; k++)
				{
					RockParticle rockParticle = this.mRockParticles[k];
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)rockParticle.mAlpha);
					g.DrawImageCel(imageByID4, (int)Common._S(rockParticle.mX), (int)Common._S(rockParticle.mY), rockParticle.mCel);
					g.SetColorizeImages(false);
				}
			}
			if (this.mDoFlyAnim)
			{
				this.mFlies.Draw(g);
			}
			g.SetColorizeImages(false);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001C800 File Offset: 0x0001AA00
		protected override Rect GetBulletRect(BossBullet b)
		{
			int num = Common._M(20);
			int num2 = Common._M(20);
			return new Rect((int)(b.mX - (float)(num / 2)), (int)(b.mY - (float)(num2 / 2)), num, num2);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0001C83C File Offset: 0x0001AA3C
		protected override Rect GetFrogRect()
		{
			int centerX = this.mLevel.mFrog.GetCenterX();
			int centerY = this.mLevel.mFrog.GetCenterY();
			Rect result = new Rect(centerX - this.mLevel.mFrog.GetWidth() / 2 + Common._M(32), centerY - this.mLevel.mFrog.GetHeight() / 2 + Common._M1(12), Common._M2(78), Common._M3(110));
			return result;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001C8BC File Offset: 0x0001AABC
		protected override bool DoHit(Bullet b, bool from_prox_bomb)
		{
			bool result = base.DoHit(b, from_prox_bomb);
			if (from_prox_bomb)
			{
				this.mThrowTimer = 0;
				this.mHitTimer = Common._M(100);
			}
			return result;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001C8EA File Offset: 0x0001AAEA
		protected override void DidFire()
		{
			base.DidFire();
			if (this.mHitTimer == 0)
			{
				this.mThrowTimer = Common._M(100);
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001C908 File Offset: 0x0001AB08
		protected override BossBullet CreateBossBullet()
		{
			return new BossBullet
			{
				mData = this.MakeMosquitoBall()
			};
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001C928 File Offset: 0x0001AB28
		protected override void BossBulletDestroyed(BossBullet b, bool outofscreen)
		{
			if (b.mData != null)
			{
				b.mData = null;
			}
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001C93C File Offset: 0x0001AB3C
		protected override void BallEaten(Bullet b)
		{
			this.mNumBallsEaten++;
			if (this.mBallEatTimer <= 0)
			{
				this.mBallEatTimer = 2000;
			}
			if (this.mNumBallsEaten >= 20)
			{
				this.mApp.SetAchievement("foie_gras");
			}
			this.mChewing = true;
			if (this.mChewFrame < 0)
			{
				this.mChewFrame = 0;
			}
			this.mChewCount = 0;
			this.mBallType = b.GetColorType();
			this.mBallSize = 1f;
			this.mBallTimer = Common._M(20);
			this.mBallVX = (this.mX + (float)Common._M(15) - b.GetX()) / (float)this.mBallTimer;
			this.mBallVY = (this.mY + (float)Common._M(40) - b.GetY()) / (float)this.mBallTimer;
			this.mBallX = b.GetX();
			this.mBallY = b.GetY();
			this.mBallCel = b.mLastFrame;
			if (!this.mApp.IsHardMode())
			{
				this.mTauntQueue.Clear();
				TauntText tauntText = new TauntText();
				this.mTauntQueue.Add(tauntText);
				tauntText.mText = TextManager.getInstance().getString(390);
				tauntText.mTextId = 390;
				tauntText.mDelay = Common._M(500);
			}
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001CA90 File Offset: 0x0001AC90
		protected override void BulletHitPlayer(BossBullet b)
		{
			base.BulletHitPlayer(b);
			if (!this.mApp.GetLevelMgr().mBossesCanAttackFuckedFrog)
			{
				for (int i = 0; i < this.mBullets.Count; i++)
				{
					BossBullet bossBullet = this.mBullets[i];
					if (bossBullet.mDelay > 0)
					{
						bossBullet.mDeleteInstantly = true;
					}
				}
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001CAEC File Offset: 0x0001ACEC
		protected virtual MosquitoBall MakeMosquitoBall()
		{
			MosquitoBall mosquitoBall = new MosquitoBall();
			Image[] array = new Image[]
			{
				Res.GetImageByID(ResID.IMAGE_BOSS_MOSQUITO_BUG1),
				Res.GetImageByID(ResID.IMAGE_BOSS_MOSQUITO_BUG2),
				Res.GetImageByID(ResID.IMAGE_BOSS_MOSQUITO_BUG3)
			};
			for (int i = 0; i < Common._M(30); i++)
			{
				Mosquito mosquito = new Mosquito();
				mosquitoBall.mMosquitoes.Add(mosquito);
				mosquito.mImage = array[SexyFramework.Common.Rand() % 3];
				mosquito.mRadius = SexyFramework.Common.FloatRange(BossMosquito.MIN_RADIUS, BossMosquito.MAX_RADIUS);
				mosquito.mAngle = SexyFramework.Common.FloatRange(0f, 6.28318f);
				mosquito.mAngleInc = SexyFramework.Common.FloatRange(Common._M(0.07f), Common._M1(0.1f)) * (float)((SexyFramework.Common.Rand() % 2 == 0) ? 1 : (-1));
				mosquito.mRadInc = SexyFramework.Common.FloatRange(Common._M(0.4f), Common._M1(0.8f));
			}
			return mosquitoBall;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001CBE5 File Offset: 0x0001ADE5
		protected override void AppliedSlowTimer()
		{
			this.mLevel.mFrog.DoPlaguedState();
			this.mFlies.ResetAnim();
			this.mDoFlyAnim = true;
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001CC0C File Offset: 0x0001AE0C
		public BossMosquito(Level l)
			: base(l)
		{
			this.mBandagedXOff = Common._M(-8);
			this.mResGroup = "Boss4";
			this.mBossRadius = Common._M(50);
			this.mBulletRadius = Common._M(20);
			this.mResPrefix = "IMAGE_BOSS_MOSQUITO_";
			this.mBulletsUseSphereColl = false;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001CC96 File Offset: 0x0001AE96
		public BossMosquito()
			: this(null)
		{
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001CCA0 File Offset: 0x0001AEA0
		public override void Dispose()
		{
			base.Dispose();
			for (int i = 0; i < this.mBullets.Count; i++)
			{
				if (this.mBullets[i].mData != null)
				{
					this.mBullets[i].mData = null;
				}
			}
			this.mBullets.Clear();
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001CCFC File Offset: 0x0001AEFC
		public void CopyFrom(BossMosquito rhs)
		{
			base.CopyFrom(rhs);
			this.mNumBallsEaten = rhs.mNumBallsEaten;
			this.mBallEatTimer = rhs.mBallEatTimer;
			this.mHitTimer = rhs.mHitTimer;
			this.mThrowTimer = rhs.mThrowTimer;
			this.mEyeFrame = rhs.mEyeFrame;
			this.mBlink = rhs.mBlink;
			this.mBlinkClosed = rhs.mBlinkClosed;
			this.mChewing = rhs.mChewing;
			this.mChewFrame = rhs.mChewFrame;
			this.mChewCount = rhs.mChewCount;
			this.mBallType = rhs.mBallType;
			this.mBallTimer = rhs.mBallTimer;
			this.mBallCel = rhs.mBallCel;
			this.mBallSize = rhs.mBallSize;
			this.mBallX = rhs.mBallX;
			this.mBallY = rhs.mBallY;
			this.mBallVX = rhs.mBallVX;
			this.mBallVY = rhs.mBallVY;
			this.mFlies = rhs.mFlies;
			this.mDoFlyAnim = rhs.mDoFlyAnim;
			for (int i = 0; i < rhs.mRockParticles.Count; i++)
			{
				RockParticle rockParticle = new RockParticle(rhs.mRockParticles[i]);
				this.mRockParticles.Add(rockParticle);
			}
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001CE34 File Offset: 0x0001B034
		public override void Update(float f)
		{
			base.Update(f);
			if (this.mDoFlyAnim)
			{
				this.mFlies.mDrawTransform.LoadIdentity();
				float num = GameApp.DownScaleNum(1f);
				this.mFlies.mDrawTransform.Scale(num, num);
				this.mFlies.mDrawTransform.Translate((float)Common._S(this.mLevel.mFrog.GetCurX() + Common._M(0)), (float)Common._S(this.mLevel.mFrog.GetCurY() + Common._M1(0)));
				this.mFlies.Update();
				if (this.mFlies.mFrameNum >= (float)this.mFlies.mLastFrameNum && this.mFlies.mCurNumParticles == 0)
				{
					this.mDoFlyAnim = false;
				}
			}
			if (this.mBallEatTimer > 0 && --this.mBallEatTimer == 0)
			{
				this.mNumBallsEaten = 0;
			}
			if (this.mHitTimer > 0)
			{
				this.mHitTimer--;
			}
			if (this.mThrowTimer > 0)
			{
				this.mThrowTimer--;
			}
			if (this.mChewing && this.mUpdateCount % Common._M(7) == 0)
			{
				this.mChewFrame++;
				Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_MOSQUITO_CHEW);
				Image imageByID2 = Res.GetImageByID(ResID.IMAGE_BOSS_MOSQUITO_ROCK);
				if (this.mChewFrame >= imageByID.mNumCols)
				{
					for (int i = 0; i < Common._M(10); i++)
					{
						RockParticle rockParticle = new RockParticle();
						this.mRockParticles.Add(rockParticle);
						rockParticle.mAlpha = 255f;
						rockParticle.mCel = SexyFramework.Common.Rand() % imageByID2.mNumCols;
						rockParticle.mX = this.mX + (float)SexyFramework.Common.IntRange(Common._M(-30), Common._M1(30));
						rockParticle.mY = this.mY + (float)Common._M(10);
						rockParticle.mVX = SexyFramework.Common.FloatRange(Common._M(-2.5f), Common._M1(2.5f));
						rockParticle.mVY = SexyFramework.Common.FloatRange(Common._M(1.5f), Common._M1(2.5f));
					}
					this.mChewCount++;
					this.mChewFrame = 1;
					if (this.mChewCount >= Common._M(4))
					{
						this.mChewing = false;
					}
				}
			}
			for (int j = 0; j < this.mRockParticles.Count; j++)
			{
				RockParticle rockParticle2 = this.mRockParticles[j];
				if ((rockParticle2.mAlpha -= Common._M(6f)) <= 0f)
				{
					this.mRockParticles.RemoveAt(j);
					j--;
				}
				else
				{
					rockParticle2.mX += rockParticle2.mVX;
					rockParticle2.mY += rockParticle2.mVY;
				}
			}
			if (this.mBallType != -1)
			{
				this.mBallX += this.mBallVX;
				this.mBallY += this.mBallVY;
				this.mBallSize -= 1f / (float)this.mBallTimer;
				if (SexyFramework.Common._leq(this.mBallSize, 0f, 0.0001f))
				{
					this.mBallType = -1;
				}
			}
			for (int k = 0; k < this.mBullets.Count; k++)
			{
				BossBullet bossBullet = this.mBullets[k];
				MosquitoBall mosquitoBall = (MosquitoBall)bossBullet.mData;
				for (int l = 0; l < mosquitoBall.mMosquitoes.Count; l++)
				{
					Mosquito mosquito = mosquitoBall.mMosquitoes[l];
					mosquito.mRadius += mosquito.mRadInc;
					if (mosquito.mRadInc > 0f && mosquito.mRadius >= BossMosquito.MAX_RADIUS)
					{
						mosquito.mRadius = BossMosquito.MAX_RADIUS;
						mosquito.mRadInc *= -1f;
					}
					else if (mosquito.mRadInc < 0f && mosquito.mRadius <= BossMosquito.MIN_RADIUS)
					{
						mosquito.mRadius = BossMosquito.MIN_RADIUS;
						mosquito.mAngleInc = SexyFramework.Common.FloatRange(Common._M(0.07f), Common._M1(0.1f)) * (float)((SexyFramework.Common.Rand() % 2 == 0) ? 1 : (-1));
						mosquito.mRadInc = SexyFramework.Common.FloatRange(Common._M(0.4f), Common._M1(0.8f));
					}
					mosquito.mAngle += mosquito.mAngleInc;
				}
			}
			if (this.mHitTimer == 0 && !this.mBlink && SexyFramework.Common.Rand() % Common._M(400) == 0)
			{
				this.mBlink = (this.mBlinkClosed = true);
				return;
			}
			if (this.mBlink && this.mUpdateCount % Common._M(5) == 0)
			{
				if (this.mBlinkClosed && ++this.mEyeFrame >= 3)
				{
					this.mBlinkClosed = false;
					this.mEyeFrame = 2;
					return;
				}
				if (!this.mBlinkClosed && --this.mEyeFrame < 0)
				{
					this.mBlink = false;
					this.mEyeFrame = 0;
				}
			}
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001D37C File Offset: 0x0001B57C
		public override void Init(Level l)
		{
			this.mWidth = Common._M(139);
			this.mHeight = Common._M(149);
			base.Init(l);
			this.mBandagedImg = Res.GetImageByID(ResID.IMAGE_BOSS_MOSQUITO_BANDAGED);
			this.mFlies = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_FLYSWARM");
			this.mFlies.ResetAnim();
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001D3E8 File Offset: 0x0001B5E8
		public override void SyncState(DataSync sync)
		{
			base.SyncState(sync);
			if (sync.isRead())
			{
				for (int i = 0; i < this.mBullets.Count; i++)
				{
					this.mBullets[i].mData = this.MakeMosquitoBall();
				}
			}
			sync.SyncBoolean(ref this.mChewing);
			sync.SyncLong(ref this.mChewFrame);
			sync.SyncLong(ref this.mChewCount);
			sync.SyncLong(ref this.mBallType);
			sync.SyncLong(ref this.mBallTimer);
			sync.SyncLong(ref this.mBallCel);
			sync.SyncFloat(ref this.mBallSize);
			sync.SyncFloat(ref this.mBallX);
			sync.SyncFloat(ref this.mBallY);
			sync.SyncFloat(ref this.mBallVX);
			sync.SyncFloat(ref this.mBallVY);
			sync.SyncBoolean(ref this.mDoFlyAnim);
			if (sync.isWrite())
			{
				Common.SerializePIEffect(this.mFlies, sync);
			}
			else
			{
				if (this.mFlies == null)
				{
					this.mFlies = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_FLYSWARM");
				}
				Common.DeserializePIEffect(this.mFlies, sync);
			}
			this.SyncListRockParticles(sync, this.mRockParticles, true);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001D514 File Offset: 0x0001B714
		private void SyncListRockParticles(DataSync sync, List<RockParticle> theList, bool clear)
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
					RockParticle rockParticle = new RockParticle();
					rockParticle.SyncState(sync);
					theList.Add(rockParticle);
					num2++;
				}
				return;
			}
			sync.GetBuffer().WriteLong((long)theList.Count);
			foreach (RockParticle rockParticle2 in theList)
			{
				rockParticle2.SyncState(sync);
			}
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001D5B4 File Offset: 0x0001B7B4
		public override Boss Instantiate()
		{
			BossMosquito bossMosquito = new BossMosquito(this.mLevel);
			bossMosquito.CopyFrom(this);
			return bossMosquito;
		}

		// Token: 0x0400024F RID: 591
		private static float MIN_RADIUS = 1f;

		// Token: 0x04000250 RID: 592
		private static float MAX_RADIUS = 20f;

		// Token: 0x04000251 RID: 593
		protected int mNumBallsEaten;

		// Token: 0x04000252 RID: 594
		protected int mBallEatTimer = -1;

		// Token: 0x04000253 RID: 595
		protected int mHitTimer;

		// Token: 0x04000254 RID: 596
		protected int mThrowTimer;

		// Token: 0x04000255 RID: 597
		protected int mEyeFrame;

		// Token: 0x04000256 RID: 598
		protected bool mBlink;

		// Token: 0x04000257 RID: 599
		protected bool mBlinkClosed = true;

		// Token: 0x04000258 RID: 600
		protected bool mChewing;

		// Token: 0x04000259 RID: 601
		protected int mChewFrame = -1;

		// Token: 0x0400025A RID: 602
		protected int mChewCount;

		// Token: 0x0400025B RID: 603
		protected int mBallType = -1;

		// Token: 0x0400025C RID: 604
		protected int mBallTimer;

		// Token: 0x0400025D RID: 605
		protected int mBallCel;

		// Token: 0x0400025E RID: 606
		protected float mBallSize = 1f;

		// Token: 0x0400025F RID: 607
		protected float mBallX;

		// Token: 0x04000260 RID: 608
		protected float mBallY;

		// Token: 0x04000261 RID: 609
		protected float mBallVX;

		// Token: 0x04000262 RID: 610
		protected float mBallVY;

		// Token: 0x04000263 RID: 611
		protected List<RockParticle> mRockParticles = new List<RockParticle>();

		// Token: 0x04000264 RID: 612
		protected PIEffect mFlies;

		// Token: 0x04000265 RID: 613
		protected bool mDoFlyAnim;
	}
}
