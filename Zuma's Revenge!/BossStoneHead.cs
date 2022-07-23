using System;
using System.Collections.Generic;
using SexyFramework;
using SexyFramework.AELib;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x02000038 RID: 56
	public class BossStoneHead : BossShoot
	{
		// Token: 0x060005CF RID: 1487 RVA: 0x00020790 File Offset: 0x0001E990
		protected override void DrawBossSpecificArt(Graphics g)
		{
			if (this.mStretchPct >= BossStoneHead.MAX_STONE_HEAD_STRETCH)
			{
				if (this.mExplodeComp.GetUpdateCount() < Common._M(150))
				{
					int num = -(this.mApp.mWidth / 2 - Common._S(this.GetX())) + Common._S(Common._M(-193)) + this.mApp.mBoardOffsetX;
					int num2 = -(this.mApp.mHeight / 2 - Common._S(this.GetY())) + Common._S(Common._M(-91));
					CumulativeTransform cumulativeTransform = new CumulativeTransform();
					cumulativeTransform.mTrans.Translate((float)num, (float)num2);
					int frame = ((this.mExplodeComp.mUpdateCount >= this.mExplodeComp.GetMaxDuration()) ? (this.mExplodeComp.GetMaxDuration() - 1) : (-1));
					this.mExplodeComp.Draw(g, cumulativeTransform, frame, Common._DS(1f));
				}
				else
				{
					this.mVolcanoBoss.Draw(g);
				}
			}
			float value = this.mX - (float)this.mWidth * this.mStretchPct / 2f + (float)this.mShakeXOff;
			float value2 = this.mY - (float)this.mHeight * this.mStretchPct / 2f + (float)this.mShakeYOff;
			for (int i = 0; i < this.mSteam.Count; i++)
			{
				Steam steam = this.mSteam[i];
				int num3 = (int)Math.Min(steam.mAlpha, this.mAlphaOverride);
				if (num3 != 255)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, num3);
				}
				if (!g.Is3D())
				{
					g.DrawImage(steam.mImage, (int)Common._S(this.mX + steam.mXOff + (float)Common._M(0)), (int)Common._S(this.mY + steam.mYOff + (float)Common._M1(0)), (int)(steam.mSize * (float)steam.mImage.mWidth), (int)(steam.mSize * (float)steam.mImage.mHeight));
				}
				else
				{
					this.mGlobalTranform.Reset();
					this.mGlobalTranform.Scale(steam.mSize, steam.mSize);
					this.mGlobalTranform.RotateRad(steam.mAngle);
					if (g.Is3D())
					{
						g.DrawImageTransformF(steam.mImage, this.mGlobalTranform, Common._S(this.mX + steam.mXOff + (float)Common._M(0)), Common._S(this.mY + (float)Common._M1(0) + steam.mYOff));
					}
					else
					{
						g.DrawImageTransform(steam.mImage, this.mGlobalTranform, Common._S(this.mX + steam.mXOff + (float)Common._M(0)), Common._S(this.mY + (float)Common._M1(0) + steam.mYOff));
					}
				}
				g.SetColorizeImages(false);
			}
			if (this.mStretchPct < BossStoneHead.MAX_STONE_HEAD_STRETCH)
			{
				int theCel = 0;
				if (this.mHP <= 0f)
				{
					theCel = 1;
				}
				else if (this.mHitTimer > Common._M(194))
				{
					theCel = 1;
				}
				else if (this.mHitTimer > 0)
				{
					theCel = 2;
				}
				if (this.IMAGE_BOSS_STONEHEAD_FACES != null)
				{
					if (this.mHP <= 0f)
					{
						Rect theDestRect = new Rect((int)Common._S(value), (int)Common._S(value2), (int)((float)this.IMAGE_BOSS_STONEHEAD_FACES.GetCelWidth() * this.mStretchPct), (int)((float)this.IMAGE_BOSS_STONEHEAD_FACES.GetCelHeight() * this.mStretchPct));
						g.DrawImage(this.IMAGE_BOSS_STONEHEAD_FACES, theDestRect, this.IMAGE_BOSS_STONEHEAD_FACES.GetCelRect(theCel));
					}
					else
					{
						g.PushState();
						if (!SexyFramework.Common._geq(this.mAlphaOverride, 255f))
						{
							g.SetColorizeImages(true);
							g.SetColor(255, 255, 255, (int)this.mAlphaOverride);
						}
						g.DrawImageCel(this.IMAGE_BOSS_STONEHEAD_FACES, (int)Common._S(value), (int)Common._S(value2), theCel);
						g.PopState();
					}
				}
			}
			g.PushState();
			if (!SexyFramework.Common._geq(this.mAlphaOverride, 255f))
			{
				g.SetColor(255, 255, 255, (int)this.mAlphaOverride);
				g.SetColorizeImages(true);
			}
			if (this.mHitTimer == 0 && !this.mDoingExplodeAnim && SexyFramework.Common._geq(this.mAlphaOverride, 255f))
			{
				int num4 = Common._M(-1);
				int num5 = Common._M(0);
				if (this.IMAGE_BOSS_STONEHEAD_EYES != null)
				{
					g.DrawImageCel(this.IMAGE_BOSS_STONEHEAD_EYES, (int)(Common._S(value) + Common._DSA(50f, (float)num4)), (int)(Common._S(value2) + Common._DSA(58f, (float)num5)), this.mEyeFrame);
				}
			}
			if (!this.mDoingExplodeAnim)
			{
				g.PushState();
				this.mLeftEye.Draw(g);
				g.PopState();
				g.PushState();
				this.mRightEye.Draw(g);
				g.PopState();
			}
			for (int j = 0; j < this.mRocks.Count; j++)
			{
				RockChunk rockChunk = this.mRocks[j];
				if (rockChunk.mAlpha != 255f)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)rockChunk.mAlpha);
				}
				if (this.IMAGE_BOSS_STONEHEAD_ROCKS != null)
				{
					g.DrawImage(this.IMAGE_BOSS_STONEHEAD_ROCKS, new Rect((int)Common._S(rockChunk.mX), (int)Common._S(rockChunk.mY), (int)((float)this.IMAGE_BOSS_STONEHEAD_ROCKS.GetCelWidth() * Common._M(0.5f)), (int)((float)this.IMAGE_BOSS_STONEHEAD_ROCKS.GetCelHeight() * Common._M1(0.5f))), this.IMAGE_BOSS_STONEHEAD_ROCKS.GetCelRect(rockChunk.mCol));
				}
				g.SetColorizeImages(false);
			}
			if (this.mTeleportDir != 0)
			{
				g.PushState();
				g.ClearClipRect();
			}
			if (!this.mDoingExplodeAnim && !this.mLevel.mBoard.IsPaused())
			{
				for (int k = 0; k < this.mBullets.Count; k++)
				{
					BossBullet bossBullet = this.mBullets[k];
					if (bossBullet.mDelay <= 0 && bossBullet.mState != 0)
					{
						EyeBullet eyeBullet = bossBullet.mData as EyeBullet;
						eyeBullet.Draw(g, (int)this.mAlphaOverride);
					}
				}
			}
			else if (this.mTextAlpha > 0f && this.mShowText)
			{
				g.SetFont(Res.GetFontByID(ResID.FONT_BOSS_TAUNT));
				g.SetColor(0, 0, 0, (int)Math.Min(this.mTextAlpha, 255f));
				float mTransX = g.mTransX;
				g.mTransX = 0f;
				if (!this.mLevel.mBoard.IsHardAdventureMode())
				{
					g.WriteString(TextManager.getInstance().getString(393), 0, Common._DS(Common._M(530)), this.mApp.mWidth, 0);
					g.WriteString(TextManager.getInstance().getString(394), 0, Common._DS(Common._M(630)), this.mApp.mWidth, 0);
					g.WriteString(TextManager.getInstance().getString(395), 0, Common._DS(Common._M(730)), this.mApp.mWidth, 0);
				}
				else
				{
					g.WriteString(TextManager.getInstance().getString(396), 0, Common._DS(Common._M(530)), this.mApp.mWidth, 0);
					g.WriteString(TextManager.getInstance().getString(397), 0, Common._DS(Common._M(630)), this.mApp.mWidth, 0);
					g.WriteString(TextManager.getInstance().getString(398), 0, Common._DS(Common._M(730)), this.mApp.mWidth, 0);
				}
				g.mTransX = mTransX;
			}
			if (this.mTeleportDir != 0)
			{
				g.PopState();
			}
			g.PopState();
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00020FBC File Offset: 0x0001F1BC
		protected override bool DoHit(Bullet b, bool from_prox_bomb)
		{
			if (this.mDoingExplodeAnim)
			{
				return false;
			}
			this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BOSS_STONE_HIT));
			this.mHitTimer = Common._M(200);
			int num = Common._M(6);
			int num2 = (int)(this.mX - (float)(this.mWidth / 2));
			int num3 = (int)(this.mY - (float)(this.mHeight / 2));
			int num4 = num3 - Common._M(0);
			int num5 = num3 + Common._M(150);
			int num6 = (int)((float)(num5 - num4) / ((float)num / 2f));
			int num7 = num2 - Common._M(10);
			int num8 = Common._M(100);
			for (int i = 0; i < num; i++)
			{
				RockChunk rockChunk = new RockChunk();
				this.mRocks.Add(rockChunk);
				rockChunk.mCol = SexyFramework.Common.Rand() % this.IMAGE_BOSS_STONEHEAD_ROCKS.mNumCols;
				rockChunk.mAlpha = 255f;
				rockChunk.mVX = 0f;
				rockChunk.mVY = SexyFramework.Common.FloatRange(Common._M(3f), Common._M1(4f));
				rockChunk.mY = (float)(num4 + i / 2 * num6);
				rockChunk.mX = (float)(num7 + ((i % 2 == 0) ? num8 : 0));
			}
			bool flag = base.DoHit(b, from_prox_bomb);
			if (flag && SexyFramework.Common._leq(this.mHP, 50f))
			{
				this.mVolcanoBoss = this.mLevel.mSecondaryBoss as BossVolcano;
				this.mVolcanoBoss.mIntro = true;
				this.mVolcanoBoss.SetXY((float)this.GetX(), (float)this.GetY());
				this.mApp.mBoard.mDrawBossUI = false;
				this.mApp.mBoard.mMenuButton.SetVisible(false);
				SoundAttribs soundAttribs = new SoundAttribs();
				soundAttribs.delay = 130;
				this.mApp.mSoundPlayer.Play(Res.GetSoundByID(ResID.SOUND_BOSS_STONE_TRANSFORM), soundAttribs);
				this.mApp.GetBoard().mPreventBallAdvancement = true;
				this.mPauseMovement = true;
				this.mDoingExplodeAnim = true;
				for (int j = 0; j < this.mBullets.Count; j++)
				{
					this.mBullets[j].mDeleteInstantly = true;
				}
				this.mTextAlpha = 0f;
				for (int k = 0; k < this.mHulaDancers.Count; k++)
				{
					this.mHulaDancers[k].mFadeOut = true;
				}
			}
			return flag;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00021240 File Offset: 0x0001F440
		protected override void DidFire()
		{
			base.DidFire();
			this.mFiring = true;
			if (this.mLeftEye.mEyeFlame.mCurNumParticles == 0)
			{
				this.mLeftEye.mEyeFlame.ResetAnim();
			}
			if (this.mRightEye.mEyeFlame.mCurNumParticles == 0)
			{
				this.mRightEye.mEyeFlame.ResetAnim();
			}
			this.mLeftEye.mFiring = (this.mRightEye.mFiring = true);
			this.mLeftEye.mEyeFlame.mEmitAfterTimeline = (this.mRightEye.mEyeFlame.mEmitAfterTimeline = true);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x000212DC File Offset: 0x0001F4DC
		protected override bool PreBulletUpdate(BossBullet b, int index)
		{
			if (b.mState == 0)
			{
				return true;
			}
			if (b.mDelay > 0)
			{
				b.mDelay--;
				return true;
			}
			if (b.mData != null && ((EyeBullet)b.mData).Update((int)b.mX, (int)b.mY, b.mBouncesLeft <= 0))
			{
				b.mDeleteInstantly = true;
			}
			return false;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00021348 File Offset: 0x0001F548
		protected override BossBullet CreateBossBullet()
		{
			BossBullet bossBullet = new BossBullet();
			EyeBullet eyeBullet = new EyeBullet();
			bossBullet.mData = eyeBullet;
			eyeBullet.mExplosion = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_STONEBOSSPROJEXPLOSION").Duplicate();
			eyeBullet.mProjectile = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_STONEBOSSPROJ").Duplicate();
			eyeBullet.mProjectile.mEmitAfterTimeline = true;
			eyeBullet.mSparks = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_STONEBOSSPROJSPARKS").Duplicate();
			return bossBullet;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x000213D8 File Offset: 0x0001F5D8
		protected override void BossBulletDestroyed(BossBullet b, bool outofscreen)
		{
			if (b.mData != null)
			{
				EyeBullet eyeBullet = (EyeBullet)b.mData;
				this.mApp.ReleaseGenericCachedEffect(eyeBullet.mSparks);
				this.mApp.ReleaseGenericCachedEffect(eyeBullet.mProjectile);
				this.mApp.ReleaseGenericCachedEffect(eyeBullet.mExplosion);
				b.mData = null;
			}
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00021434 File Offset: 0x0001F634
		protected override Rect GetBulletRect(BossBullet b)
		{
			if (b.mData == null)
			{
				return new Rect(0, 0, 0, 0);
			}
			EyeBullet eyeBullet = (EyeBullet)b.mData;
			return new Rect((int)(b.mX + (float)eyeBullet.mXOff), (int)(b.mY + (float)eyeBullet.mYOff), Common._DS(Common._M(14)), Common._DS(Common._M1(20)));
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0002149C File Offset: 0x0001F69C
		protected override void BulletHitPlayer(BossBullet b)
		{
			SoundAttribs soundAttribs = new SoundAttribs();
			soundAttribs.fadeout = 0.1f;
			this.mApp.mSoundPlayer.Loop(Res.GetSoundByID(ResID.SOUND_NEW_BURNINGFROGLOOP), soundAttribs);
			this.mApp.mSoundPlayer.Play(Res.GetSoundByID(ResID.SOUND_NEW_FIREHITFROG));
			if (!this.mApp.GetLevelMgr().mBossesCanAttackFuckedFrog)
			{
				this.mLevel.mFrog.SetSlowTimer(0);
				this.mLevel.mBoard.SetHallucinateTimer(0);
				for (int i = 0; i < this.mBullets.Count; i++)
				{
					BossBullet bossBullet = this.mBullets[i];
					if (bossBullet.mDelay > 0 || bossBullet.mState == 0)
					{
						bossBullet.mDeleteInstantly = true;
					}
				}
			}
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00021560 File Offset: 0x0001F760
		protected override void GetShotBounceOffs(BossBullet b, ref int x, ref int y)
		{
			x = (y = 0);
			if (b.mData == null)
			{
				return;
			}
			EyeBullet eyeBullet = (EyeBullet)b.mData;
			x += eyeBullet.mXOff + Common._DS(Common._M(0));
			y += eyeBullet.mYOff + Common._DS(Common._M(0));
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x000215B8 File Offset: 0x0001F7B8
		protected override bool CanFire()
		{
			return !this.mDoingExplodeAnim;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x000215C3 File Offset: 0x0001F7C3
		protected override bool CanSpawnHulaDancers()
		{
			return !this.mDoingExplodeAnim;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x000215D0 File Offset: 0x0001F7D0
		protected override void ShotBounced(BossBullet b)
		{
			EyeBullet eyeBullet = (EyeBullet)b.mData;
			this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BOSS_STONE_EYE_LASER_BOUNCE));
			if (b.mBouncesLeft == 0)
			{
				eyeBullet.mExplosion.ResetAnim();
				eyeBullet.mProjectile.mEmitAfterTimeline = false;
				return;
			}
			if (eyeBullet.mSparks.mCurNumParticles == 0)
			{
				eyeBullet.mSparkFirstFrame = true;
				eyeBullet.mSparks.ResetAnim();
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0002163D File Offset: 0x0001F83D
		protected override void BerserkActivated(int health_limit)
		{
			base.BerserkActivated(health_limit);
			this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BOSS_STONE_BERSERK));
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0002165B File Offset: 0x0001F85B
		protected override bool CanTaunt()
		{
			return !SexyFramework.Common._leq(this.mHP, 50f) && base.CanTaunt();
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00021678 File Offset: 0x0001F878
		public BossStoneHead(Level l)
			: base(l)
		{
			this.mShouldDoDeathExplosions = false;
			this.mBossRadius = Common._M(70);
			this.mBulletRadius = Common._M(5);
			this.mResGroup = "Boss6Common";
			this.mDrawDeathBGTikis = false;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00021707 File Offset: 0x0001F907
		public BossStoneHead()
			: this(null)
		{
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00021710 File Offset: 0x0001F910
		public override void Dispose()
		{
			if (this.mExplodeComp != null)
			{
				this.mExplodeComp.Dispose();
				this.mExplodeComp = null;
			}
			for (int i = 0; i < this.mBullets.Count; i++)
			{
				if (this.mBullets[i].mData != null)
				{
					EyeBullet eyeBullet = (EyeBullet)this.mBullets[i].mData;
					this.mApp.ReleaseGenericCachedEffect(eyeBullet.mSparks);
					this.mApp.ReleaseGenericCachedEffect(eyeBullet.mProjectile);
					this.mApp.ReleaseGenericCachedEffect(eyeBullet.mExplosion);
				}
			}
			this.mApp.ReleaseGenericCachedEffect(this.mLeftEye.mEyeFlame);
			this.mApp.ReleaseGenericCachedEffect(this.mRightEye.mEyeFlame);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x000217D8 File Offset: 0x0001F9D8
		public void CopyForm(BossStoneHead rhs)
		{
			base.CopyFrom(rhs);
			this.mShakeTime = rhs.mShakeTime;
			this.mEyeFlameAlpha = rhs.mEyeFlameAlpha;
			this.mBlink = rhs.mBlink;
			this.mBlinkClosed = rhs.mBlinkClosed;
			this.mFiring = rhs.mFiring;
			this.mDoingExplodeAnim = rhs.mDoingExplodeAnim;
			this.mTextAlpha = rhs.mTextAlpha;
			this.mShowText = rhs.mShowText;
			this.mEyeFrame = rhs.mEyeFrame;
			this.mHitTimer = rhs.mHitTimer;
			this.mLeftInUse = rhs.mLeftInUse;
			this.mRightInUse = rhs.mRightInUse;
			this.mExplodeComp = rhs.mExplodeComp;
			this.mVolcanoBoss = rhs.mVolcanoBoss;
			this.mLeftEye = new EyeAnim(rhs.mLeftEye);
			this.mRightEye = new EyeAnim(rhs.mRightEye);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000218B8 File Offset: 0x0001FAB8
		public override void Update(float f)
		{
			base.Update(f);
			if (this.mHitTimer > 0 && SexyFramework.Common._geq(this.mAlphaOverride, 255f))
			{
				this.mHitTimer--;
				if (this.mUpdateCount % Common._M(2) == 0)
				{
					Steam steam = new Steam();
					this.mSteam.Add(steam);
					steam.mAlphaDec = Common._M(4f);
					steam.mAngleInc = SexyFramework.Common.FloatRange(Common._M(0.01f), Common._M1(0.1f));
					steam.mVX = Common._M(-2f);
					steam.mVY = Common._M(-0.02f);
					steam.mImgNum = SexyFramework.Common.Rand() % 2;
					steam.mImage = ((steam.mImgNum == 0) ? Res.GetImageByID(ResID.IMAGE_BOSS_STONEHEAD_FOG1) : Res.GetImageByID(ResID.IMAGE_BOSS_STONEHEAD_FOG2));
					steam = new Steam();
					this.mSteam.Add(steam);
					steam.mAlphaDec = Common._M(4f);
					steam.mAngleInc = SexyFramework.Common.FloatRange(Common._M(0.01f), Common._M1(0.1f));
					steam.mVX = Common._M(2f);
					steam.mVY = Common._M(-0.02f);
					steam.mImgNum = SexyFramework.Common.Rand() % 2;
					steam.mImage = ((steam.mImgNum == 0) ? Res.GetImageByID(ResID.IMAGE_BOSS_STONEHEAD_FOG1) : Res.GetImageByID(ResID.IMAGE_BOSS_STONEHEAD_FOG2));
				}
			}
			for (int i = 0; i < this.mRocks.Count; i++)
			{
				RockChunk rockChunk = this.mRocks[i];
				rockChunk.mY += rockChunk.mVY;
				rockChunk.mX += rockChunk.mVX;
				rockChunk.mAlpha -= Common._M(4.5f);
				if (rockChunk.mAlpha <= 0f)
				{
					this.mRocks.RemoveAt(i);
					i--;
				}
			}
			for (int j = 0; j < this.mSteam.Count; j++)
			{
				Steam steam2 = this.mSteam[j];
				steam2.mXOff += steam2.mVX;
				steam2.mYOff += steam2.mVY;
				steam2.mAngle += steam2.mAngleInc;
				steam2.mSize += Common._M(0.01f);
				if (Math.Abs(steam2.mXOff) >= Common._M(-1f))
				{
					steam2.mAlpha -= steam2.mAlphaDec;
					if (steam2.mAlpha <= 0f)
					{
						this.mSteam.RemoveAt(j);
						j--;
					}
				}
			}
			if (!this.mBlink && !this.mFiring && SexyFramework.Common.Rand() % Common._M(400) == 0)
			{
				this.mBlink = (this.mBlinkClosed = true);
			}
			else if (this.mBlink && this.mUpdateCount % Common._M(5) == 0)
			{
				if (this.mBlinkClosed && ++this.mEyeFrame >= 3)
				{
					this.mBlinkClosed = false;
					this.mEyeFrame = 2;
				}
				else if (!this.mBlinkClosed && --this.mEyeFrame < 0)
				{
					this.mBlink = false;
					this.mEyeFrame = 0;
				}
			}
			if (this.mEyeFlameAlpha >= 255f && SexyFramework.Common._geq(this.mAlphaOverride, 255f))
			{
				float mX = this.mX - (float)this.mWidth / 2f + (float)this.mShakeXOff;
				float mY = this.mY - (float)this.mHeight / 2f + (float)this.mShakeYOff;
				for (int k = 0; k < this.mBullets.Count; k++)
				{
					BossBullet bossBullet = this.mBullets[k];
					EyeBullet eyeBullet = (EyeBullet)bossBullet.mData;
					if (bossBullet.mState == 0)
					{
						if (!this.mLeftInUse)
						{
							this.mLeftInUse = true;
							bossBullet.mState = -1;
							bossBullet.mX = mX;
							bossBullet.mY = mY;
							eyeBullet.mXOff = Common._M(49);
							eyeBullet.mYOff = Common._M(55);
						}
						else if (!this.mRightInUse)
						{
							this.mRightInUse = true;
							bossBullet.mState = 1;
							bossBullet.mX = mX;
							bossBullet.mY = mY;
							eyeBullet.mXOff = Common._M(89);
							eyeBullet.mYOff = Common._M(55);
						}
						this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BOSS_STONE_EYE_LASER));
						if (bossBullet.mState != 0 && (bossBullet.mShotType == 1 || bossBullet.mShotType == 3))
						{
							base.FireBulletAtPlayer(bossBullet, SexyFramework.Common.FloatRange(base.mMinBulletSpeed, base.mMaxBulletSpeed), bossBullet.mX + (float)eyeBullet.mXOff, bossBullet.mY + (float)eyeBullet.mYOff);
							bossBullet.mTargetVX = bossBullet.mVX;
							bossBullet.mTargetVY = bossBullet.mVY;
						}
					}
					else if (bossBullet.mUpdateCount > Common._M(50))
					{
						if (bossBullet.mState < 0)
						{
							this.mLeftInUse = false;
						}
						else
						{
							this.mRightInUse = false;
						}
					}
				}
				if (!this.mLeftInUse && !this.mRightInUse)
				{
					this.mLeftEye.mEyeFlame.mEmitAfterTimeline = (this.mRightEye.mEyeFlame.mEmitAfterTimeline = false);
					this.mLeftEye.mFiring = (this.mRightEye.mFiring = false);
					this.mFiring = false;
				}
			}
			if (this.mFiring)
			{
				if (this.mEyeFlameAlpha < 255f)
				{
					this.mEyeFlameAlpha += Common._M(4f);
					if (this.mEyeFlameAlpha > 255f)
					{
						this.mEyeFlameAlpha = 255f;
					}
				}
			}
			else if (this.mEyeFlameAlpha > 0f)
			{
				this.mEyeFlameAlpha -= Common._M(5f);
				if (this.mEyeFlameAlpha < 0f)
				{
					this.mEyeFlameAlpha = 0f;
				}
			}
			this.mLeftEye.Update((int)this.mX + BossStoneHead.LEFT_EYE_XOFF, (int)this.mY + BossStoneHead.EYE_YOFF, (int)this.mAlphaOverride);
			this.mRightEye.Update((int)this.mX + BossStoneHead.RIGHT_EYE_XOFF, (int)this.mY + BossStoneHead.EYE_YOFF, (int)this.mAlphaOverride);
			if (this.mDoingExplodeAnim)
			{
				bool flag = this.UpdateDeathSequence();
				if (flag)
				{
					this.mTextAlpha -= Common._M(1f);
					this.mExplodeComp.Update();
					if (this.mExplodeComp.GetUpdateCount() == 35)
					{
						this.DoDeathRockExplosionThing();
					}
					if (this.mExplodeComp.GetUpdateCount() >= Common._M(150))
					{
						this.mVolcanoBoss.Update();
					}
					if (this.mExplodeComp.Done() && this.mTextAlpha <= 0f)
					{
						this.mLevel.SwitchToSecondaryBoss();
						this.mVolcanoBoss.mIntro = false;
						this.mApp.GetBoard().mPreventBallAdvancement = false;
						this.mApp.mBoard.mDrawBossUI = true;
						this.mApp.mBoard.mMenuButton.SetVisible(true);
					}
				}
			}
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00022020 File Offset: 0x00020220
		public override void Init(Level l)
		{
			this.mWidth = Common._M(120);
			this.mHeight = Common._M(182);
			base.Init(l);
			if (this.mExplodeComp != null)
			{
				this.mExplodeComp.Dispose();
				this.mExplodeComp = null;
			}
			this.mExplodeComp = new Composition();
			this.mExplodeComp.mLoadImageFunc = new AECommon.LoadCompImageFunc(GameApp.CompositionLoadFunc);
			this.mExplodeComp.mPostLoadImageFunc = new AECommon.PostLoadCompImageFunc(GameApp.CompositionPostLoadFunc);
			this.mExplodeComp.LoadFromFile("pax\\BreakEasterIsland_FINAL");
			this.mLeftEye.mEyeFlame = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_STONEBOSSEYES").Duplicate();
			this.mRightEye.mEyeFlame = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_STONEBOSSEYES").Duplicate();
			this.IMAGE_BOSS_STONEHEAD_FACES = Res.GetImageByID(ResID.IMAGE_BOSS_STONEHEAD_FACES);
			this.IMAGE_BOSS_STONEHEAD_ROCKS = Res.GetImageByID(ResID.IMAGE_BOSS_STONEHEAD_ROCKS);
			this.IMAGE_BOSS_STONEHEAD_EYES = Res.GetImageByID(ResID.IMAGE_BOSS_STONEHEAD_EYES);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00022130 File Offset: 0x00020330
		public override Boss Instantiate()
		{
			BossStoneHead bossStoneHead = new BossStoneHead(this.mLevel);
			bossStoneHead.CopyFrom(this);
			bossStoneHead.mSteam.Clear();
			bossStoneHead.mRocks.Clear();
			return bossStoneHead;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00022168 File Offset: 0x00020368
		public override void SyncState(DataSync sync)
		{
			base.SyncState(sync);
			SexyBuffer buffer = sync.GetBuffer();
			sync.SyncBoolean(ref this.mDoingExplodeAnim);
			sync.SyncFloat(ref this.mTextAlpha);
			sync.SyncBoolean(ref this.mShowText);
			sync.SyncLong(ref this.mExplodeComp.mUpdateCount);
			sync.SyncFloat(ref this.mStretchPct);
			sync.SyncLong(ref this.mShakeTime);
			sync.SyncFloat(ref this.mEyeFlameAlpha);
			sync.SyncBoolean(ref this.mBlink);
			sync.SyncBoolean(ref this.mBlinkClosed);
			sync.SyncBoolean(ref this.mFiring);
			sync.SyncLong(ref this.mEyeFrame);
			sync.SyncLong(ref this.mHitTimer);
			sync.SyncBoolean(ref this.mLeftInUse);
			sync.SyncBoolean(ref this.mRightInUse);
			this.mLeftEye.SyncState(sync);
			this.mRightEye.SyncState(sync);
			this.SyncListRockChunks(sync, this.mRocks, true);
			this.SyncListSteams(sync, this.mSteam, true);
			for (int i = 0; i < this.mBullets.Count; i++)
			{
				if (sync.isWrite())
				{
					EyeBullet eyeBullet = (EyeBullet)this.mBullets[i].mData;
					eyeBullet.SyncState(sync);
				}
				else
				{
					EyeBullet eyeBullet2 = new EyeBullet();
					eyeBullet2.SyncState(sync);
					this.mBullets[i].mData = eyeBullet2;
				}
			}
			if (sync.isWrite())
			{
				buffer.WriteBoolean(this.mVolcanoBoss != null);
				if (this.mVolcanoBoss != null)
				{
					buffer.WriteLong((long)this.mVolcanoBoss.GetX());
					buffer.WriteLong((long)this.mVolcanoBoss.GetY());
					return;
				}
			}
			else if (sync.isRead())
			{
				if (buffer.ReadBoolean())
				{
					this.mVolcanoBoss = (BossVolcano)this.mLevel.mSecondaryBoss;
					this.mVolcanoBoss.mIntro = true;
					int num = (int)buffer.ReadLong();
					int num2 = (int)buffer.ReadLong();
					this.mVolcanoBoss.SetXY((float)num, (float)num2);
					return;
				}
				this.mVolcanoBoss = null;
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00022368 File Offset: 0x00020568
		private void SyncListRockChunks(DataSync sync, List<RockChunk> theList, bool clear)
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
					RockChunk rockChunk = new RockChunk();
					rockChunk.SyncState(sync);
					theList.Add(rockChunk);
					num2++;
				}
				return;
			}
			sync.GetBuffer().WriteLong((long)theList.Count);
			foreach (RockChunk rockChunk2 in theList)
			{
				rockChunk2.SyncState(sync);
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00022408 File Offset: 0x00020608
		private void SyncListSteams(DataSync sync, List<Steam> theList, bool clear)
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
					Steam steam = new Steam();
					steam.SyncState(sync);
					theList.Add(steam);
					num2++;
				}
				return;
			}
			sync.GetBuffer().WriteLong((long)theList.Count);
			foreach (Steam steam2 in theList)
			{
				steam2.SyncState(sync);
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x000224A8 File Offset: 0x000206A8
		public override bool AllowFrogToFire()
		{
			return base.AllowFrogToFire() && !this.mDoingExplodeAnim;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x000224C0 File Offset: 0x000206C0
		public bool UpdateDeathSequence()
		{
			if (this.mShakeTime > 0)
			{
				if (this.mShakeTime == Common._M(150))
				{
					this.mShowText = true;
				}
				else if (this.mShakeTime < Common._M(150))
				{
					this.mTextAlpha += Common._M(2.8f);
				}
				this.mShakeTime--;
				if (this.mShakeTime % Common._M(50) == 0)
				{
					this.mShakeXAmt++;
					this.mShakeYAmt++;
				}
				this.mShakeXOff = SexyFramework.Common.IntRange(0, this.mShakeXAmt);
				this.mShakeYOff = SexyFramework.Common.IntRange(0, this.mShakeYAmt);
			}
			else
			{
				this.mShakeXOff = (this.mShakeYOff = 0);
			}
			for (int i = 0; i < this.mRocks.Count; i++)
			{
				RockChunk rockChunk = this.mRocks[i];
				rockChunk.mY += rockChunk.mVY;
				rockChunk.mX += rockChunk.mVX;
				rockChunk.mVY += Common._M(0.2f);
				rockChunk.mAlpha -= Common._M(4.5f);
				if (rockChunk.mAlpha <= 0f)
				{
					this.mRocks.RemoveAt(i);
					i--;
				}
			}
			if (Boss.gBerserkTextAlpha > 0f)
			{
				Boss.gBerserkTextAlpha -= Common._M(1f);
				Boss.gBerserkTextY -= Common._M(1f);
			}
			for (int j = 0; j < this.mSteam.Count; j++)
			{
				Steam steam = this.mSteam[j];
				steam.mXOff += steam.mVX;
				steam.mYOff += steam.mVY;
				steam.mAngle += steam.mAngleInc;
				steam.mSize += Common._M(0.01f);
				if (Math.Abs(steam.mXOff) >= Common._M(-1f))
				{
					steam.mAlpha -= steam.mAlphaDec;
					if (steam.mAlpha <= 0f)
					{
						this.mSteam.RemoveAt(j);
						j--;
					}
				}
			}
			if (this.mShakeTime <= 0)
			{
				this.mStretchPct += (BossStoneHead.MAX_STONE_HEAD_STRETCH - 1f) / Common._M(25f);
				if (this.mStretchPct >= BossStoneHead.MAX_STONE_HEAD_STRETCH)
				{
					this.mStretchPct = BossStoneHead.MAX_STONE_HEAD_STRETCH;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00022768 File Offset: 0x00020968
		public void DoDeathRockExplosionThing()
		{
			for (int i = 45; i < 135; i += Common._M(2))
			{
				RockChunk rockChunk = new RockChunk();
				this.mRocks.Add(rockChunk);
				rockChunk.mCol = SexyFramework.Common.Rand() % this.IMAGE_BOSS_STONEHEAD_ROCKS.mNumCols;
				rockChunk.mAlpha = 255f;
				float num = SexyFramework.Common.DegreesToRadians((float)i);
				float num2 = SexyFramework.Common.FloatRange(Common._M(4f), Common._M1(6f));
				rockChunk.mVX = num2 * (float)Math.Cos((double)num);
				rockChunk.mVY = -num2 * (float)Math.Sin((double)num);
				rockChunk.mX = this.mX;
				rockChunk.mY = this.mY;
			}
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00022820 File Offset: 0x00020A20
		public override int GetTopLeftX()
		{
			return (int)(this.mX - (float)this.mWidth * this.mStretchPct / 2f);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0002283E File Offset: 0x00020A3E
		public override int GetTopLeftY()
		{
			return (int)(this.mY - (float)this.mHeight * this.mStretchPct / 2f);
		}

		// Token: 0x040002BE RID: 702
		protected static float MAX_STONE_HEAD_STRETCH = 1.001f;

		// Token: 0x040002BF RID: 703
		protected static int LEFT_EYE_XOFF = -16;

		// Token: 0x040002C0 RID: 704
		protected static int RIGHT_EYE_XOFF = 30;

		// Token: 0x040002C1 RID: 705
		protected static int EYE_YOFF = -35;

		// Token: 0x040002C2 RID: 706
		protected int mShakeTime = 300;

		// Token: 0x040002C3 RID: 707
		protected float mEyeFlameAlpha;

		// Token: 0x040002C4 RID: 708
		protected bool mBlink;

		// Token: 0x040002C5 RID: 709
		protected bool mBlinkClosed = true;

		// Token: 0x040002C6 RID: 710
		protected bool mFiring;

		// Token: 0x040002C7 RID: 711
		protected bool mDoingExplodeAnim;

		// Token: 0x040002C8 RID: 712
		protected float mTextAlpha;

		// Token: 0x040002C9 RID: 713
		protected bool mShowText;

		// Token: 0x040002CA RID: 714
		protected int mEyeFrame;

		// Token: 0x040002CB RID: 715
		protected int mHitTimer;

		// Token: 0x040002CC RID: 716
		protected bool mLeftInUse;

		// Token: 0x040002CD RID: 717
		protected bool mRightInUse;

		// Token: 0x040002CE RID: 718
		protected EyeAnim mLeftEye = new EyeAnim();

		// Token: 0x040002CF RID: 719
		protected EyeAnim mRightEye = new EyeAnim();

		// Token: 0x040002D0 RID: 720
		protected List<Steam> mSteam = new List<Steam>();

		// Token: 0x040002D1 RID: 721
		protected List<RockChunk> mRocks = new List<RockChunk>();

		// Token: 0x040002D2 RID: 722
		protected Composition mExplodeComp;

		// Token: 0x040002D3 RID: 723
		protected BossVolcano mVolcanoBoss;

		// Token: 0x040002D4 RID: 724
		private Image IMAGE_BOSS_STONEHEAD_FACES;

		// Token: 0x040002D5 RID: 725
		private Image IMAGE_BOSS_STONEHEAD_EYES;

		// Token: 0x040002D6 RID: 726
		private Image IMAGE_BOSS_STONEHEAD_ROCKS;

		// Token: 0x040002D7 RID: 727
		public float mStretchPct = 1f;
	}
}
