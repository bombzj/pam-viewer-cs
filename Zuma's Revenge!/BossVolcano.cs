using System;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x0200003D RID: 61
	public class BossVolcano : BossShoot
	{
		// Token: 0x0600060F RID: 1551 RVA: 0x00026938 File Offset: 0x00024B38
		protected override void BossBulletDestroyed(BossBullet b, bool outofscreen)
		{
			if (b.mData != null)
			{
				PIEffect fx = (PIEffect)b.mData;
				this.mApp.ReleaseVolcanoEffect(fx);
			}
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00026968 File Offset: 0x00024B68
		protected override void DrawBossSpecificArt(Graphics g)
		{
			float num = this.mX - (float)this.mWidth / 2f + (float)this.mShakeXAmt;
			float num2 = this.mY - (float)this.mHeight / 2f + (float)this.mShakeYAmt;
			g.PushState();
			if (!SexyFramework.Common._geq(this.mAlphaOverride, 255f))
			{
				g.SetColor(255, 255, 255, (int)this.mAlphaOverride);
				g.SetColorizeImages(true);
			}
			if (this.mHitCel == -1)
			{
				g.DrawImageCel(Res.GetImageByID(ResID.IMAGE_BOSS_VOLCANO_WINGS), (int)Common._S(num + (float)Common._M(28)), (int)Common._S(num2 + (float)Common._M1(39)), BossVolcano.WING_CELS[this.mWingIndex]);
				g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_VOLCANO_HEAD_BOWL), (int)Common._S(num + (float)Common._M(77)), (int)Common._S(num2 + (float)Common._M1(36)));
				g.PushState();
				if (this.mBoilingLava == null)
				{
					this.mBoilingLava = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_BOILING_DEVIL_HEAD");
					this.mBoilingLava.mEmitAfterTimeline = true;
				}
				this.mBoilingLava.mColor.mAlpha = (int)this.mAlphaOverride;
				this.mBoilingLava.Draw(g);
				g.PopState();
				g.DrawImageCel(Res.GetImageByID(ResID.IMAGE_BOSS_VOLCANO_HAND), (int)Common._S(num + (float)Common._M(55)), (int)Common._S(num2 + (float)Common._M1(87)), BossVolcano.HAND_CELS[this.mLeftHandIndex]);
				g.DrawImageMirror(Res.GetImageByID(ResID.IMAGE_BOSS_VOLCANO_HAND), (int)Common._S(num + (float)Common._M(135)), (int)Common._S(num2 + (float)Common._M1(87)), Res.GetImageByID(ResID.IMAGE_BOSS_VOLCANO_HAND).GetCelRect(BossVolcano.HAND_CELS[this.mRightHandIndex]));
			}
			if (this.mTeleportDir != 0)
			{
				g.PushState();
				g.ClearClipRect();
			}
			if (!this.mLevel.mBoard.IsPaused())
			{
				for (int i = 0; i < this.mBullets.Count; i++)
				{
					BossBullet bossBullet = this.mBullets[i];
					if (bossBullet.mDelay <= 0 && bossBullet.mOffscreenPause > 0)
					{
						PIEffect pieffect = (PIEffect)bossBullet.mData;
						g.PushState();
						g.ClipRect(0, 0, GameApp.gApp.mWidth, Common._DS(Common._M(200)));
						pieffect.mColor.mAlpha = (int)this.mAlphaOverride;
						pieffect.Draw(g);
						g.PopState();
					}
				}
			}
			if (this.mTeleportDir != 0)
			{
				g.PopState();
			}
			if (this.mHitCel == -1)
			{
				g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_VOLCANO_HEAD), (int)Common._S(num + (float)Common._M(55)), (int)Common._S(num2 + (float)Common._M1(23)));
				g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_VOLCANO_JAW), (int)Common._S(num + (float)Common._M(79)), (int)(Common._S(num2 + (float)Common._M1(143)) + this.mJawYOff));
			}
			else
			{
				g.DrawImageCel(Res.GetImageByID(ResID.IMAGE_BOSS_VOLCANO_HIT), (int)Common._S(num + (float)Common._M(-12)), (int)Common._S(num2 + (float)Common._M1(19)), this.mHitCel);
			}
			if (!this.mLevel.mBoard.IsPaused())
			{
				for (int j = 0; j < this.mBullets.Count; j++)
				{
					BossBullet bossBullet2 = this.mBullets[j];
					if (bossBullet2.mDelay <= 0 && bossBullet2.mOffscreenPause <= 0)
					{
						PIEffect pieffect2 = (PIEffect)bossBullet2.mData;
						g.PushState();
						pieffect2.mColor.mAlpha = 255;
						pieffect2.Draw(g);
						g.PopState();
					}
				}
			}
			g.PopState();
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00026D44 File Offset: 0x00024F44
		protected override bool DoHit(Bullet b, bool from_prox_bomb)
		{
			if (this.mHitCel == -1)
			{
				this.mHitCel = 0;
			}
			bool flag = base.DoHit(b, from_prox_bomb);
			if (flag)
			{
				this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BOSS_DEVIL_HIT));
			}
			if (flag && this.mHP <= 0f)
			{
				this.mApp.GetBoard().mContinueNextLevelOnLoadProfile = true;
				this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BOSS_DEVIL_DEATH));
			}
			return flag;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00026DB9 File Offset: 0x00024FB9
		protected override Rect GetBulletRect(BossBullet b)
		{
			return new Rect((int)b.mX - Common._M(15), (int)b.mY + Common._M1(20), Common._M2(20), Common._M3(55));
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00026DEC File Offset: 0x00024FEC
		protected override bool CheckBulletHitPlayer(BossBullet b)
		{
			if (!b.mCanHitPlayer)
			{
				return false;
			}
			float y = (float)(this.mLevel.mFrog.GetCenterY() - 5);
			float x = (float)(this.mLevel.mFrog.GetCenterX() + 2);
			return MathUtils.CirclesIntersect(x, y, b.mX, b.mY, (float)(this.mBossRadius + Common._M(10)));
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00026E50 File Offset: 0x00025050
		protected override void BulletHitPlayer(BossBullet b)
		{
			SoundAttribs soundAttribs = new SoundAttribs();
			soundAttribs.fadeout = 0.1f;
			this.mApp.mSoundPlayer.Loop(Res.GetSoundByID(ResID.SOUND_NEW_BURNINGFROGLOOP), soundAttribs);
			this.mApp.mSoundPlayer.Play(Res.GetSoundByID(ResID.SOUND_NEW_FIREHITFROG));
			if (!this.mApp.GetLevelMgr().mBossesCanAttackFuckedFrog)
			{
				for (int i = 0; i < this.mBullets.Count; i++)
				{
					BossBullet bossBullet = this.mBullets[i];
					if (bossBullet.mOffscreenPause > 0)
					{
						bossBullet.mDeleteInstantly = true;
					}
				}
			}
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00026EE8 File Offset: 0x000250E8
		protected override BossBullet CreateBossBullet()
		{
			BossBullet bossBullet = base.CreateBossBullet();
			bossBullet.mData = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_DEVIL_PROJECTILE").Duplicate();
			return bossBullet;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00026F1D File Offset: 0x0002511D
		protected override void DidFire()
		{
			this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BOSS_DEVIL_FIRES));
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00026F34 File Offset: 0x00025134
		public BossVolcano(Level l)
			: base(l)
		{
			this.mTauntTextYOff = Common._DS(Common._M(20));
			this.mBoilingLava = null;
			this.mBulletsUseSphereColl = true;
			this.mBossRadius = Common._M(70);
			this.mBulletRadius = Common._M(25);
			this.mDrawDeathBGTikis = false;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00026F97 File Offset: 0x00025197
		public BossVolcano()
			: this(null)
		{
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00026FA0 File Offset: 0x000251A0
		public override void Dispose()
		{
			base.Dispose();
			for (int i = 0; i < this.mBullets.Count; i++)
			{
				BossBullet bossBullet = this.mBullets[i];
				if (bossBullet.mData != null)
				{
					this.mApp.ReleaseVolcanoEffect((PIEffect)bossBullet.mData);
				}
			}
			this.mBoilingLava = null;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00026FFC File Offset: 0x000251FC
		protected void CopyFrom(BossVolcano rhs)
		{
			base.CopyFrom(rhs);
			this.mBoilingLava = rhs.mBoilingLava;
			this.mWingIndex = rhs.mWingIndex;
			this.mLeftHandIndex = rhs.mLeftHandIndex;
			this.mRightHandIndex = rhs.mRightHandIndex;
			this.mJawCount = rhs.mJawCount;
			this.mHitCel = rhs.mHitCel;
			this.mJawYOff = rhs.mJawYOff;
			this.mJawRate = rhs.mJawRate;
			this.mAnimateHands = rhs.mAnimateHands;
			this.mIntro = rhs.mIntro;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00027088 File Offset: 0x00025288
		public override void Update(float f)
		{
			if (!this.mIntro)
			{
				base.Update(f);
			}
			else
			{
				this.mUpdateCount++;
			}
			Common._M(0.3f);
			Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_VOLCANO_HIT);
			if (this.mHitCel >= 0 && !this.mIntro && this.mUpdateCount % BossVolcano.HIT_TIMES[this.mHitCel] == 0 && ++this.mHitCel >= imageByID.mNumCols)
			{
				this.mHitCel = -1;
			}
			if (this.mBoilingLava == null)
			{
				this.mBoilingLava = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_BOILING_DEVIL_HEAD");
				this.mBoilingLava.mEmitAfterTimeline = true;
			}
			this.mBoilingLava.mDrawTransform.LoadIdentity();
			float num = GameApp.DownScaleNum(1.4f);
			this.mBoilingLava.mDrawTransform.Scale(num, num);
			this.mBoilingLava.mDrawTransform.Translate(Common._S(this.mX + (float)Common._M(10)), Common._S(this.mY + (float)Common._M1(-40)));
			this.mBoilingLava.Update();
			if (this.mHP > 0f)
			{
				if (this.mUpdateCount % Common._M(15) == 0)
				{
					this.mWingIndex = (this.mWingIndex + 1) % BossVolcano.NUM_WING_FRAMES;
				}
				if (this.mUpdateCount % Common._M(8) == 0 && this.mAnimateHands)
				{
					this.mLeftHandIndex = (this.mLeftHandIndex + 1) % BossVolcano.NUM_HAND_FRAMES;
					this.mRightHandIndex = (this.mRightHandIndex + 1) % BossVolcano.NUM_HAND_FRAMES;
					if (this.mLeftHandIndex == 0)
					{
						this.mAnimateHands = false;
					}
				}
				if (this.mJawRate == 0f && SexyFramework.Common.Rand(400) == 0)
				{
					this.mJawRate = Common._M(-1f);
				}
				if (SexyFramework.Common.Rand(100) == 0 && this.mLeftHandIndex == 0)
				{
					this.mAnimateHands = true;
				}
			}
			if (this.mJawRate != 0f)
			{
				this.mJawYOff += this.mJawRate;
				if (this.mJawRate < 0f && this.mJawYOff <= -8f)
				{
					this.mJawYOff = -8f;
					this.mJawRate *= -1f;
				}
				else if (this.mJawRate > 0f && this.mJawYOff >= 0f)
				{
					if (++this.mJawCount == 2)
					{
						this.mJawCount = 0;
						this.mJawYOff = (this.mJawRate = 0f);
					}
					else
					{
						this.mJawYOff = 0f;
						this.mJawRate *= -1f;
					}
				}
			}
			if (!this.mIntro && SexyFramework.Common._geq(this.mAlphaOverride, 255f))
			{
				for (int i = 0; i < this.mBullets.Count; i++)
				{
					BossBullet bossBullet = this.mBullets[i];
					PIEffect pieffect = (PIEffect)bossBullet.mData;
					if (pieffect != null && !pieffect.mEmitAfterTimeline)
					{
						pieffect.mEmitAfterTimeline = true;
						Common.SetFXNumScale(pieffect, 3f);
					}
					if (bossBullet.mState == 0 && bossBullet.mDelay <= 0)
					{
						bossBullet.mState++;
						bossBullet.mCanHitPlayer = false;
						bool flag = this.mX > this.mDestX;
						bossBullet.mX = this.mX + (float)(flag ? Common._M(5) : Common._M1(30));
						bossBullet.mY = this.mY + (float)Common._M(50);
						pieffect.mDrawTransform.LoadIdentity();
						num = GameApp.DownScaleNum(1.4f);
						pieffect.mDrawTransform.Scale(num, num);
						pieffect.mDrawTransform.Scale(1f, -1f);
						pieffect.mDrawTransform.Translate(Common._S(bossBullet.mX + (float)Common._M(0)), Common._S(bossBullet.mY + (float)Common._M1(0)));
						pieffect.Update();
					}
					else if (bossBullet.mState == 1 && bossBullet.mY >= (float)(this.mLevel.mFrog.GetCenterY() - Common._M(0)))
					{
						bossBullet.mData = null;
						bossBullet.mState++;
						bossBullet.mCanHitPlayer = true;
						bossBullet.mVY = (bossBullet.mTargetVY = 0f);
						PIEffect pieffect2 = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_DEVIL_EXPLOSION").Duplicate();
						pieffect2.mEmitAfterTimeline = true;
						pieffect2.mDrawTransform.LoadIdentity();
						num = GameApp.DownScaleNum(1.4f);
						pieffect2.mDrawTransform.Scale(num, num);
						pieffect2.mDrawTransform.Translate(Common._S(bossBullet.mX + (float)Common._M(0)), Common._S(bossBullet.mY + (float)Common._M1(0)));
						pieffect2.Update();
						this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BOSS_DEVIL_EXPLODES));
						bossBullet.mData = pieffect2;
					}
					else if (bossBullet.mState == 1)
					{
						pieffect.mDrawTransform.LoadIdentity();
						num = GameApp.DownScaleNum(1.4f);
						pieffect.mDrawTransform.Scale(num, num);
						if (bossBullet.mOffscreenPause > 0)
						{
							bool flag2 = this.mX > this.mDestX;
							bossBullet.mX = this.mX + (float)(flag2 ? Common._M(5) : Common._M1(5));
							pieffect.mDrawTransform.Scale(1f, -1f);
							pieffect.mDrawTransform.Translate(Common._S(bossBullet.mX + (float)Common._M(0)), Common._S(bossBullet.mY + (float)Common._M1(-30)));
						}
						else
						{
							pieffect.mDrawTransform.Scale(1f, 1f);
							pieffect.mDrawTransform.Translate(Common._S(bossBullet.mX + (float)Common._M(0)), Common._S(bossBullet.mY + (float)Common._M1(0)));
						}
						pieffect.Update();
					}
					else if (bossBullet.mState == 2)
					{
						PIEffect pieffect3 = (PIEffect)bossBullet.mData;
						pieffect3.Update();
						if (pieffect3.mFrameNum > (float)(pieffect3.mLastFrameNum - Common._M(20)))
						{
							bossBullet.mCanHitPlayer = false;
							this.mApp.ReleaseVolcanoEffect(pieffect3);
							bossBullet.mData = null;
							this.BossBulletDestroyed(bossBullet, false);
							this.mBullets.RemoveAt(i);
							i--;
						}
					}
				}
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00027714 File Offset: 0x00025914
		public override void Init(Level l)
		{
			this.mWidth = Common._M(225);
			this.mHeight = Common._M(225);
			base.Init(l);
			for (int i = 0; i < Boss.NUM_HEARTS; i++)
			{
				this.mHeartCels[i] = 0;
			}
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00027764 File Offset: 0x00025964
		public override Boss Instantiate()
		{
			BossVolcano bossVolcano = new BossVolcano();
			bossVolcano.CopyFrom(this);
			return bossVolcano;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00027780 File Offset: 0x00025980
		public override void SyncState(DataSync sync)
		{
			base.SyncState(sync);
			sync.SyncLong(ref this.mWingIndex);
			sync.SyncLong(ref this.mLeftHandIndex);
			sync.SyncLong(ref this.mRightHandIndex);
			sync.SyncLong(ref this.mJawCount);
			sync.SyncLong(ref this.mHitCel);
			sync.SyncFloat(ref this.mJawYOff);
			sync.SyncFloat(ref this.mJawRate);
			sync.SyncBoolean(ref this.mAnimateHands);
			SexyBuffer buffer = sync.GetBuffer();
			if (sync.isWrite())
			{
				for (int i = 0; i < this.mBullets.Count; i++)
				{
					BossBullet bossBullet = this.mBullets[i];
					PIEffect s = (PIEffect)bossBullet.mData;
					buffer.WriteBoolean(bossBullet.mState == 2);
					Common.SerializePIEffect(s, sync);
				}
				return;
			}
			for (int j = 0; j < this.mBullets.Count; j++)
			{
				PIEffect pieffect;
				if (buffer.ReadBoolean())
				{
					pieffect = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_DEVIL_EXPLOSION").Duplicate();
				}
				else
				{
					pieffect = this.mApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_DEVIL_PROJECTILE").Duplicate();
				}
				Common.DeserializePIEffect(pieffect, sync);
				pieffect.mEmitAfterTimeline = true;
				this.mBullets[j].mData = pieffect;
			}
		}

		// Token: 0x04000315 RID: 789
		private static int BV_WIDTH = 225;

		// Token: 0x04000316 RID: 790
		private static int BV_HEIGHT = 225;

		// Token: 0x04000317 RID: 791
		private static int NUM_WING_FRAMES = 4;

		// Token: 0x04000318 RID: 792
		private static int[] WING_CELS = new int[] { 1, 2, 1, 0 };

		// Token: 0x04000319 RID: 793
		private static int NUM_HAND_FRAMES = 4;

		// Token: 0x0400031A RID: 794
		private static int[] HAND_CELS = new int[] { 1, 2, 3, 0 };

		// Token: 0x0400031B RID: 795
		private static int NUM_HIT_FRAMES = 4;

		// Token: 0x0400031C RID: 796
		private static int[] HIT_TIMES = new int[] { 8, 8, 8, 15 };

		// Token: 0x0400031D RID: 797
		public bool mIntro;

		// Token: 0x0400031E RID: 798
		protected PIEffect mBoilingLava;

		// Token: 0x0400031F RID: 799
		protected int mWingIndex;

		// Token: 0x04000320 RID: 800
		protected int mLeftHandIndex;

		// Token: 0x04000321 RID: 801
		protected int mRightHandIndex = 2;

		// Token: 0x04000322 RID: 802
		protected int mJawCount;

		// Token: 0x04000323 RID: 803
		protected int mHitCel = -1;

		// Token: 0x04000324 RID: 804
		protected float mJawYOff;

		// Token: 0x04000325 RID: 805
		protected float mJawRate;

		// Token: 0x04000326 RID: 806
		protected bool mAnimateHands;
	}
}
