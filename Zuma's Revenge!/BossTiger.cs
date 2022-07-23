using System;
using System.Collections.Generic;
using System.Linq;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.PIL;

namespace ZumasRevenge
{
	// Token: 0x0200003B RID: 59
	public class BossTiger : BossShoot
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x00023C7C File Offset: 0x00021E7C
		protected override bool PreBulletUpdate(BossBullet b, int index)
		{
			if (b.mDelay > 0)
			{
				b.mDelay--;
			}
			else if (b.mState == 0)
			{
				int num = (int)(this.mX - (float)(this.mWidth / 2));
				int num2 = ((this.mLevel.mFrog.GetCenterX() > num + BossTiger.PAW_LEFT_X_OFF) ? 1 : (-1));
				int num3 = ((this.mLevel.mFrog.GetCenterX() > num + BossTiger.PAW_RIGHT_X_OFF) ? 1 : (-1));
				BossFiringState bossFiringState = null;
				int num4 = 0;
				int num5 = 0;
				if (num3 == -1 && this.mRightPaw.mState == 0)
				{
					this.mRightPaw = new BossFiringState();
					bossFiringState = this.mRightPaw;
					b.mImageNum = 2;
					num4 = BossTiger.SKULL_RIGHT_X_OFF;
					num5 = BossTiger.SKULL_RIGHT_Y_OFF;
					if (this.mPawModifyingHeadAngle == 0)
					{
						this.mPawModifyingHeadAngle = 1;
					}
				}
				else if (num2 == 1 && this.mLeftPaw.mState == 0)
				{
					this.mLeftPaw = new BossFiringState();
					bossFiringState = this.mLeftPaw;
					b.mImageNum = 1;
					num4 = BossTiger.SKULL_LEFT_X_OFF;
					num5 = BossTiger.SKULL_LEFT_Y_OFF;
					if (this.mPawModifyingHeadAngle == 0)
					{
						this.mPawModifyingHeadAngle = -1;
					}
				}
				if (bossFiringState != null)
				{
					bossFiringState.mBulletId = b.mId;
					bossFiringState.mState = 1;
					b.mState++;
					Image image = BossTiger.gTigerBulletImages[b.mImageNum];
					float num6 = 3.14159f - Common.GetCanonicalAngleRad(base.FireBulletAtPlayer(b, SexyFramework.Common.FloatRange(base.mMinBulletSpeed, base.mMaxBulletSpeed), this.mX - (float)(this.mWidth / 2) + (float)(image.mWidth / 2) + (float)num4, this.mY - (float)(this.mHeight / 2) + (float)(image.mHeight / 2) + (float)num5)) + 1.570795f;
					num6 *= -1f;
					b.mTargetVX = b.mVX;
					b.mTargetVY = b.mVY;
					bossFiringState.mTargetSkullAngle = num6;
					bossFiringState.mSkullAngle = 0f;
					bossFiringState.mSkullAngleInc = num6 / (float)Common._M(12);
					Skull skull = this.GetSkull(bossFiringState == this.mLeftPaw);
					skull.mImageNum = b.mImageNum;
					skull.mImage = BossTiger.gFiringImages[b.mImageNum];
					b.mData = skull;
				}
			}
			return b.mState <= 1;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00023ECA File Offset: 0x000220CA
		private void FreeSkull(Skull temp)
		{
			if (temp != null)
			{
				temp.Dispose();
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00023ED8 File Offset: 0x000220D8
		private Skull GetSkull(bool p)
		{
			Skull skull;
			if (this.mSkullsPool.Count > 0)
			{
				skull = Enumerable.Last<Skull>(this.mSkullsPool);
				this.mSkullsPool.RemoveAt(this.mSkullsPool.Count - 1);
				skull.mIsLeft = p;
			}
			else
			{
				skull = new Skull(p);
			}
			return skull;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00023F2C File Offset: 0x0002212C
		protected override void DrawBossSpecificArt(Graphics g)
		{
			int num = (int)(this.mX - (float)(this.mWidth / 2) + (float)this.mShakeXOff);
			int num2 = (int)(this.mY - (float)(this.mHeight / 2) + (float)this.mShakeYOff);
			if (this.mHP > 0f && !this.mDoDeathExplosions && !this.mLevel.mBoard.IsPaused())
			{
				if (this.mTeleportDir != 0)
				{
					g.PushState();
					g.ClearClipRect();
				}
				if (this.mAlphaOverride < 255f)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)this.mAlphaOverride);
				}
				for (int i = 0; i < this.mBullets.Count; i++)
				{
					BossBullet bossBullet = this.mBullets[i];
					if (bossBullet.mDelay <= 0 && bossBullet.mState >= 2)
					{
						Image image = BossTiger.gTigerBulletImages[bossBullet.mImageNum];
						int mWidth = image.mWidth;
						int mHeight = image.mHeight;
						Skull skull = (Skull)bossBullet.mData;
						if (skull == null || skull.mAlpha < 255f)
						{
							g.DrawImageRotated(image, (int)(Common._S(bossBullet.mX) - (float)(mWidth / 2)), (int)(Common._S(bossBullet.mY) - (float)(mHeight / 2)), (double)bossBullet.mAngle);
						}
						if (skull != null)
						{
							skull.Draw(g, Common._S(bossBullet.mX) - (float)(mWidth / 2), Common._S(bossBullet.mY) - (float)(mHeight / 2), bossBullet.mAngle);
						}
					}
				}
				g.SetColorizeImages(false);
				if (this.mTeleportDir != 0)
				{
					g.PopState();
				}
			}
			for (int j = 0; j < this.mSkulls.Count; j++)
			{
				this.mSkulls[j].mParticleAlpha = this.mAlphaOverride / 255f;
				this.mSkulls[j].mAlpha = this.mAlphaOverride;
				this.mSkulls[j].Draw(g, 0f, 0f, 0f);
			}
			if (this.mAlphaOverride < 255f)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)this.mAlphaOverride);
			}
			if (this.mLeftPaw.mState < 4 || this.mLeftPaw.mState >= 6)
			{
				Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_PAWBACK_LEFT);
				g.DrawImage(imageByID, Common._S(num + BossTiger.PAW_LEFT_X_OFF), (int)Common._S((float)(num2 + Common._M(53)) + this.mLeftPaw.mPawYOffset));
			}
			else if (this.mLeftPaw.mState < 6)
			{
				Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_SKULL_LEFT);
				if (this.mLeftPaw.mState == 4 && this.mLeftPaw.mTimer == 0)
				{
					g.DrawImageRotated(imageByID, (int)Common._S((float)(num + BossTiger.SKULL_LEFT_X_OFF) + this.mLeftPaw.mSkullXOffset), (int)Common._S((float)(num2 + BossTiger.SKULL_LEFT_Y_OFF) + this.mLeftPaw.mSkullYOffset), (double)this.mLeftPaw.mSkullAngle);
				}
				imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_SWIPE);
				g.DrawImageMirror(imageByID, Common._S(num + Common._M(15)), Common._S(num2 + Common._M1(71)), imageByID.GetCelRect(this.mLeftPaw.mSwipeFrame));
			}
			if (this.mRightPaw.mState < 4 || this.mRightPaw.mState >= 6)
			{
				Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_PAWBACK_RIGHT);
				g.DrawImage(imageByID, Common._S(num + BossTiger.PAW_RIGHT_X_OFF), (int)Common._S((float)(num2 + Common._M(55)) + this.mRightPaw.mPawYOffset));
			}
			else if (this.mRightPaw.mState < 6)
			{
				Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_SKULL_RIGHT);
				if (this.mRightPaw.mState == 4 && this.mRightPaw.mTimer == 0)
				{
					g.DrawImageRotated(imageByID, (int)Common._S((float)(num + BossTiger.SKULL_RIGHT_X_OFF) + this.mRightPaw.mSkullXOffset), (int)Common._S((float)(num2 + BossTiger.SKULL_RIGHT_Y_OFF) + this.mRightPaw.mSkullYOffset), (double)this.mRightPaw.mSkullAngle);
				}
				imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_SWIPE);
				g.DrawImage(imageByID, Common._S(num + Common._M(125)), Common._S(num2 + Common._M1(71)), imageByID.GetCelRect(this.mRightPaw.mSwipeFrame));
			}
			int num3;
			int num4;
			Image imageByID2;
			if (this.mDoExplosion)
			{
				num3 = Common._S(Common._M(33));
				num4 = Common._S(Common._M(-8));
				imageByID2 = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_HIT);
			}
			else
			{
				num3 = Common._S(Common._M(44));
				num4 = Common._S(Common._M(0));
				imageByID2 = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_HEAD);
			}
			if (this.mPawModifyingHeadAngle == 0)
			{
				g.DrawImage(imageByID2, Common._S(num) + num3, Common._S(num2) + num4);
			}
			else
			{
				g.DrawImageRotated(imageByID2, Common._S(num) + num3, Common._S(num2) + num4, (double)((this.mPawModifyingHeadAngle == -1) ? this.mLeftPaw.mHeadAngle : this.mRightPaw.mHeadAngle));
			}
			float num5 = 0f;
			if (this.mLeftPaw.mState < 4 || this.mLeftPaw.mState >= 6)
			{
				Image imageByID3 = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_SKULL_LEFT);
				g.PushState();
				if (!SexyFramework.Common._eq(this.mLeftPaw.mSkullGrowPct, 1f, 0.001f))
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)Math.Min(this.mAlphaOverride, Math.Min(this.mLeftPaw.mSkullGrowPct * 255f, 255f)));
					num5 = (1f - this.mLeftPaw.mSkullGrowPct) * (float)imageByID3.mWidth / 2f;
				}
				float num6 = (float)imageByID3.mWidth * this.mLeftPaw.mSkullGrowPct;
				float num7 = (float)imageByID3.mHeight * this.mLeftPaw.mSkullGrowPct;
				g.DrawImage(imageByID3, (int)((float)Common._S(num + BossTiger.SKULL_LEFT_X_OFF) + num5 + Common._S(this.mLeftPaw.mSkullXOffset)), (int)Common._S((float)(num2 + BossTiger.SKULL_LEFT_Y_OFF) + this.mLeftPaw.mSkullYOffset), (int)num6, (int)num7);
				g.PopState();
			}
			if (this.mLeftPaw.mState >= 2 && this.mLeftPaw.mState <= 4)
			{
				BossBullet bulletById = this.GetBulletById(this.mLeftPaw.mBulletId);
				Skull skull2 = (Skull)bulletById.mData;
				if (skull2 != null && (this.mLeftPaw.mState != 4 || bulletById.mDelay > 0 || bulletById.mState < 2))
				{
					skull2.Draw(g, (float)Common._S(num + BossTiger.SKULL_LEFT_X_OFF) + num5 + Common._S(this.mLeftPaw.mSkullXOffset), (float)((int)Common._S((float)(num2 + BossTiger.SKULL_LEFT_Y_OFF) + this.mLeftPaw.mSkullYOffset)), this.mLeftPaw.mSkullAngle);
				}
			}
			num5 = 0f;
			if (this.mRightPaw.mState < 4 || this.mRightPaw.mState >= 6)
			{
				Image imageByID4 = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_SKULL_RIGHT);
				g.PushState();
				if (!SexyFramework.Common._eq(this.mRightPaw.mSkullGrowPct, 1f, 0.001f))
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)Math.Min(this.mAlphaOverride, Math.Min(this.mRightPaw.mSkullGrowPct * 255f, 255f)));
					num5 = (1f - this.mRightPaw.mSkullGrowPct) * (float)imageByID4.mWidth / 2f;
				}
				float num8 = (float)imageByID4.mWidth * this.mRightPaw.mSkullGrowPct;
				float num9 = (float)imageByID4.mHeight * this.mRightPaw.mSkullGrowPct;
				g.DrawImage(imageByID4, (int)((float)Common._S(num + BossTiger.SKULL_RIGHT_X_OFF) + num5 + (float)((int)Common._S(this.mRightPaw.mSkullXOffset))), (int)Common._S((float)(num2 + BossTiger.SKULL_RIGHT_Y_OFF) + this.mRightPaw.mSkullYOffset), (int)num8, (int)num9);
				g.PopState();
			}
			if (this.mRightPaw.mState >= 2 && this.mRightPaw.mState <= 4)
			{
				BossBullet bulletById2 = this.GetBulletById(this.mRightPaw.mBulletId);
				Skull skull3 = (Skull)bulletById2.mData;
				if (skull3 != null && (this.mRightPaw.mState != 4 || bulletById2.mDelay > 0 || bulletById2.mState < 2))
				{
					skull3.Draw(g, (float)Common._S(num + BossTiger.SKULL_RIGHT_X_OFF) + num5 + Common._S(this.mRightPaw.mSkullXOffset), (float)((int)Common._S((float)(num2 + BossTiger.SKULL_RIGHT_Y_OFF) + this.mRightPaw.mSkullYOffset)), this.mRightPaw.mSkullAngle);
				}
			}
			if (this.mLeftPaw.mState < 4 || this.mLeftPaw.mState >= 6)
			{
				g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_PAWFRONT_LEFT), Common._S(num), (int)Common._S((float)(num2 + Common._M(53)) + this.mLeftPaw.mPawYOffset));
			}
			if (this.mRightPaw.mState < 4 || this.mRightPaw.mState >= 6)
			{
				g.DrawImage(Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_PAWFRONT_RIGHT), Common._S(num + Common._M(163)), (int)Common._S((float)(num2 + Common._M1(55)) + this.mRightPaw.mPawYOffset));
			}
			if (this.mLeftPaw.mStreaksAlpha > 0f)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)Math.Min(this.mLeftPaw.mStreaksAlpha, this.mAlphaOverride));
				int num10;
				int num11;
				Image imageByID5;
				if (this.mLeftPaw.mState == 4)
				{
					num10 = Common._S(Common._M(11));
					num11 = Common._S(Common._M(74));
					imageByID5 = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_STREAKS_DOWN);
				}
				else
				{
					num10 = Common._S(Common._M(3));
					num11 = Common._S(Common._M(80));
					imageByID5 = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_STREAKS_UP);
				}
				g.DrawImage(imageByID5, Common._S(num) + num10, Common._S(num2) + num11);
				g.SetColorizeImages(false);
			}
			if (this.mRightPaw.mStreaksAlpha > 0f)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)Math.Min(this.mRightPaw.mStreaksAlpha, this.mAlphaOverride));
				int num12;
				int num13;
				Image imageByID6;
				if (this.mRightPaw.mState == 4)
				{
					num12 = Common._S(Common._M(119));
					num13 = Common._S(Common._M(75));
					imageByID6 = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_STREAKS_DOWN);
				}
				else
				{
					num12 = Common._S(Common._M(130));
					num13 = Common._S(Common._M(80));
					imageByID6 = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_STREAKS_UP);
				}
				g.DrawImageMirror(imageByID6, Common._S(num) + num12, Common._S(num2) + num13);
				g.SetColorizeImages(false);
			}
			if (this.mAlphaOverride < 255f)
			{
				g.SetColorizeImages(false);
			}
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00024AAA File Offset: 0x00022CAA
		protected override void BulletErased(int index)
		{
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00024AAC File Offset: 0x00022CAC
		protected override Rect GetBulletRect(BossBullet b)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_SKULL_LEFT);
			int num = (int)((float)imageByID.mWidth * 0.75f);
			int num2 = (int)((float)imageByID.mHeight * 0.75f);
			return new Rect((int)(b.mX - (float)(num / 2)), (int)(b.mY - (float)(num2 / 2)), num, num2);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00024B04 File Offset: 0x00022D04
		protected override void BulletHitPlayer(BossBullet b)
		{
			base.BulletHitPlayer(b);
			if (b.mData != null)
			{
				Skull skull = (Skull)b.mData;
				skull.mHitPlayer = true;
				skull.mChunks.SetPos((float)(this.mLevel.mFrog.GetCenterX() + Common._M(0)), (float)(this.mLevel.mFrog.GetCenterY() + Common._M1(0)));
				skull.mClouds.SetPos((float)(this.mLevel.mFrog.GetCenterX() + Common._M(0)), (float)(this.mLevel.mFrog.GetCenterY() + Common._M1(0)));
			}
			if (!GameApp.gApp.GetLevelMgr().mBossesCanAttackFuckedFrog)
			{
				for (int i = 0; i < this.mBullets.Count; i++)
				{
					BossBullet bossBullet = this.mBullets[i];
					if (bossBullet.mDelay > 0 || bossBullet.mState < 2)
					{
						bossBullet.mDeleteInstantly = true;
					}
				}
			}
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00024BFC File Offset: 0x00022DFC
		protected override void BossBulletDestroyed(BossBullet b, bool outofscreen)
		{
			base.BossBulletDestroyed(b, outofscreen);
			if (this.mLeftPaw.mBulletId == b.mId)
			{
				this.mLeftPaw = new BossFiringState();
			}
			if (this.mRightPaw.mBulletId == b.mId)
			{
				this.mRightPaw = new BossFiringState();
			}
			if (b.mData != null)
			{
				Skull skull = (Skull)b.mData;
				if (outofscreen)
				{
					this.FreeSkull(skull);
				}
				else
				{
					skull.mTrail.ForceStopEmitting(true);
					skull.mLeftEye.ForceStopEmitting(true);
					skull.mRightEye.ForceStopEmitting(true);
					skull.mUseLastPos = true;
					this.mSkulls.Add(skull);
				}
				b.mData = null;
			}
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00024CAC File Offset: 0x00022EAC
		protected void SyncSkull(DataSync sync, Skull s)
		{
			SexyBuffer buffer = sync.GetBuffer();
			if (sync.isWrite())
			{
				bool flag = false;
				for (int i = 0; i < this.mBullets.Count; i++)
				{
					if (this.mBullets[i].mData != null && (Skull)this.mBullets[i].mData == s)
					{
						flag = true;
						buffer.WriteLong((long)i);
						break;
					}
				}
				if (!flag)
				{
					buffer.WriteLong(-1L);
				}
				buffer.WriteBoolean(s.mIsLeft);
				buffer.WriteLong((long)s.mImageNum);
				buffer.WriteLong((long)s.mLeftEyeHandle);
				buffer.WriteLong((long)s.mRightEyeHandle);
				buffer.WriteLong((long)s.mTrailPTHandle);
				buffer.WriteLong((long)s.mTrailHandle);
				buffer.WriteFloat(s.mLastX);
				buffer.WriteFloat(s.mLastY);
				buffer.WriteFloat(s.mLastAngle);
				buffer.WriteFloat(s.mAlpha);
				buffer.WriteBoolean(s.mLaunched);
				buffer.WriteBoolean(s.mUseLastPos);
				buffer.WriteBoolean(s.mHitPlayer);
				Common.SerializeParticleSystem(s.mLeftEye, sync);
				Common.SerializeParticleSystem(s.mRightEye, sync);
				Common.SerializeParticleSystem(s.mTrail, sync);
				Common.SerializeParticleSystem(s.mChunks, sync);
				Common.SerializeParticleSystem(s.mClouds, sync);
				return;
			}
			int num = (int)buffer.ReadLong();
			Skull skull = new Skull(buffer.ReadBoolean());
			skull.mImageNum = (int)buffer.ReadLong();
			skull.mImage = BossTiger.gFiringImages[skull.mImageNum];
			skull.mLeftEyeHandle = (int)buffer.ReadLong();
			skull.mRightEyeHandle = (int)buffer.ReadLong();
			skull.mTrailPTHandle = (int)buffer.ReadLong();
			skull.mTrailHandle = (int)buffer.ReadLong();
			skull.mLastX = buffer.ReadFloat();
			skull.mLastY = buffer.ReadFloat();
			skull.mLastAngle = buffer.ReadFloat();
			skull.mAlpha = buffer.ReadFloat();
			skull.mLaunched = buffer.ReadBoolean();
			skull.mUseLastPos = buffer.ReadBoolean();
			skull.mHitPlayer = buffer.ReadBoolean();
			if (num != -1)
			{
				this.mBullets[num].mData = skull;
			}
			else
			{
				this.mSkulls.Add(skull);
			}
			skull.mLeftEye = Common.DeserializeParticleSystem(sync);
			skull.mRightEye = Common.DeserializeParticleSystem(sync);
			skull.mTrail = Common.DeserializeParticleSystem(sync);
			skull.mFatHead = new SexyFramework.PIL.System(50, 50);
			skull.mFatHead.mScale = Common._S(1f);
			skull.mChunks = Common.DeserializeParticleSystem(sync);
			skull.mClouds = Common.DeserializeParticleSystem(sync);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00024F5B File Offset: 0x0002315B
		protected override bool CanTaunt()
		{
			return this.mTutorialState == 0;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00024F68 File Offset: 0x00023168
		public override bool Collides(Bullet b)
		{
			bool flag = base.Collides(b);
			if (this.mTutorialState == 2)
			{
				if (flag)
				{
					this.mCleanHeart = false;
					this.mTauntQueue.Clear();
					this.mTutorialState = 3;
				}
			}
			else if (this.mTutorialState == 5 && flag)
			{
				this.mTauntQueue.Clear();
				this.mTutorialState = 0;
				GameApp.gApp.GetBoard().mPreventBallAdvancement = false;
				TauntText tauntText = new TauntText();
				this.mTauntQueue.Add(tauntText);
				tauntText.mText = TextManager.getInstance().getString(399);
				tauntText.mDelay = Common._M(200);
				tauntText.mTextId = 399;
				tauntText = new TauntText();
				this.mTauntQueue.Add(tauntText);
				tauntText.mText = TextManager.getInstance().getString(400);
				tauntText.mDelay = Common._M(200);
				tauntText.mTextId = 400;
			}
			return flag;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00025063 File Offset: 0x00023263
		protected override bool CanFire()
		{
			return this.mTutorialState == 0;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0002506E File Offset: 0x0002326E
		public override void DeleteAllBullets()
		{
			base.DeleteAllBullets();
			this.mLeftPaw = new BossFiringState();
			this.mRightPaw = new BossFiringState();
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0002508C File Offset: 0x0002328C
		protected BossBullet GetBulletById(int id)
		{
			for (int i = 0; i < this.mBullets.Count; i++)
			{
				if (this.mBullets[i].mId == id)
				{
					return this.mBullets[i];
				}
			}
			return this.mBullets[0];
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x000250DC File Offset: 0x000232DC
		public BossTiger(Level l)
			: base(l)
		{
			this.mPawModifyingHeadAngle = 0;
			this.mStompVX = 0f;
			this.mStompVY = 0f;
			this.mStompAY = 0f;
			this.mStompRestingY = 0f;
			this.mStompPause = 0;
			this.mStompCount = 0;
			this.mDrawHeartsBelowBoss = true;
			this.mBossRadius = Common._M(100);
			this.mBulletRadius = Common._M(25);
			this.mBossRadiusYOff = Common._M(-30);
			this.mResGroup = "Boss1";
			this.mResPrefix = "IMAGE_BOSS_TIGER_";
			if (GameApp.gApp != null && (GameApp.gApp.IsHardMode() || (GameApp.gApp.mUserProfile != null && GameApp.gApp.mUserProfile.GetAdvModeVars().mNumTimesZoneBeat[0] > 0)))
			{
				this.mTutorialState = 0;
				return;
			}
			this.mTutorialState = 1;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x000251E8 File Offset: 0x000233E8
		public BossTiger()
			: this(null)
		{
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x000251F4 File Offset: 0x000233F4
		public override void Dispose()
		{
			base.Dispose();
			for (int i = 0; i < this.mBullets.Count; i++)
			{
				if (this.mBullets[i].mData != null)
				{
					((Skull)this.mBullets[i].mData).Dispose();
					this.mBullets[i].mData = null;
				}
			}
			for (int j = 0; j < this.mSkulls.Count; j++)
			{
				if (this.mSkulls[j] != null)
				{
					this.mSkulls[j].Dispose();
					this.mSkulls[j] = null;
				}
			}
			this.mSkulls.Clear();
			BossTiger.gTigerBulletImages[0] = null;
			BossTiger.gTigerBulletImages[1] = null;
			BossTiger.gTigerBulletImages[2] = null;
			BossTiger.gFiringImages[0] = null;
			BossTiger.gFiringImages[1] = null;
			BossTiger.gFiringImages[2] = null;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x000252DA File Offset: 0x000234DA
		public bool ShouldEraseBullets()
		{
			return this.mTutorialState <= 2 && this.mTutorialState > 0 && this.mTauntQueue.Count <= 1;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00025304 File Offset: 0x00023504
		public override bool AllowFrogToFire()
		{
			if (this.mTutorialState == 0)
			{
				return base.AllowFrogToFire();
			}
			return (this.mTutorialState == 2 && this.mTauntQueue.Count <= 1) || (this.mTutorialState == 5 && GameApp.gApp.GetBoard().mPreventBallAdvancement && this.mLevel.AllCurvesAtRolloutPoint());
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00025363 File Offset: 0x00023563
		public override int GetFrogReloadType()
		{
			if (this.mTutorialState == 2)
			{
				return SexyFramework.Common.Rand() % 4;
			}
			return base.GetFrogReloadType();
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0002537C File Offset: 0x0002357C
		public override void Update(float f)
		{
			GameApp gApp = GameApp.gApp;
			if (gApp.GetBoard().IsAboutToDoCheckpointEffect())
			{
				return;
			}
			BossTiger.PAW_LEFT_X_OFF = Common._M(-4);
			BossTiger.PAW_RIGHT_X_OFF = Common._M(151);
			BossTiger.SKULL_LEFT_X_OFF = Common._M(2);
			BossTiger.SKULL_LEFT_Y_OFF = Common._M(72);
			BossTiger.SKULL_RIGHT_X_OFF = Common._M(154);
			BossTiger.SKULL_RIGHT_Y_OFF = Common._M(73);
			if (SexyFramework.Common._geq(this.mAlphaOverride, 255f))
			{
				for (int i = 0; i < this.mBullets.Count; i++)
				{
					BossBullet bossBullet = this.mBullets[i];
					if (bossBullet.mData != null)
					{
						((Skull)bossBullet.mData).Update(bossBullet.mX, bossBullet.mY, bossBullet.mAngle);
					}
				}
				for (int j = 0; j < this.mSkulls.Count; j++)
				{
					Skull skull = this.mSkulls[j];
					skull.Update(0f, 0f, 0f);
					if (skull.mLeftEye.Done() && skull.mRightEye.Done() && skull.mTrail.Done() && (!skull.mHitPlayer || (skull.mFatHead.Done() && skull.mChunks.Done() && skull.mClouds.Done())))
					{
						this.FreeSkull(skull);
						this.mSkulls.RemoveAt(j);
						j--;
					}
				}
			}
			if (this.mDoDeathExplosions || this.mHP <= 0f || this.mLevel.mBoard.DoingBossIntro())
			{
				base.Update(f);
				return;
			}
			if (this.mTutorialState == 1)
			{
				this.mAlphaOverride = 255f;
				if (this.mStompPause == 0)
				{
					this.mX += this.mStompVX;
					this.mY += this.mStompVY;
					this.mStompVY += this.mStompAY;
					if (this.mY >= this.mStompRestingY && this.mStompVY > 0f)
					{
						this.mStompCount++;
						gApp.GetBoard().ShakeScreen(Common._M(50), Common._M1(2), Common._M2(2));
						gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_TIGER_BOSS_SMASH));
						if (this.mStompCount == 1)
						{
							this.mY = this.mStompRestingY;
							this.mStompPause = Common._M(25);
							float num = (float)Common._M(50);
							this.mStompVX = ((float)(Common._SS(gApp.mWidth) / 2) - this.mX) / num;
							this.mStompVY = Common._M(-2f);
							this.mStompAY = ((float)Common._M(150) - this.mStompVY * num) / (num * num);
						}
						else
						{
							this.mTutorialState = 2;
							TauntText tauntText = new TauntText();
							this.mTauntQueue.Add(tauntText);
							tauntText.mText = TextManager.getInstance().getString(401);
							tauntText.mDelay = Common._M(300);
							tauntText.mTextId = 401;
							tauntText = new TauntText();
							this.mTauntQueue.Add(tauntText);
							tauntText.mText = TextManager.getInstance().getString(402);
							tauntText.mDelay = Common._M(500);
							tauntText.mTextId = 402;
						}
					}
				}
				else
				{
					this.mStompPause--;
				}
			}
			else if (this.mTutorialState == 3)
			{
				if (GameApp.USE_TRIAL_VERSION)
				{
					if (GameApp.gApp.mBoard != null)
					{
						GameApp.gApp.mBoard.Pause(true, true);
					}
					string @string = TextManager.getInstance().getString(833);
					int width_pad = Common._DS(Common._M(20));
					GameApp.gApp.DoYesNoDialog(TextManager.getInstance().getString(448), @string, true, TextManager.getInstance().getString(446), TextManager.getInstance().getString(447), false, Common._S(Common._M(50)), 1, width_pad);
					GameApp.gApp.mYesNoDialogDelegate = new GameApp.YesNoDialogDelegate(this.ProcessYesNo);
					return;
				}
				float num2 = (float)Common._M(-120);
				if (this.mY > num2)
				{
					this.mY -= Common._M(12f);
					if (this.mY <= num2)
					{
						this.mY = num2;
						gApp.GetBoard().ShakeScreen(Common._M(100), Common._M1(3), Common._M2(3));
						this.mStompCount = 0;
						this.mTutorialState = 4;
						this.mStompPause = Common._M(150);
					}
				}
			}
			else if (this.mTutorialState == 4)
			{
				if (this.mStompPause > 0)
				{
					if (--this.mStompPause == 0 && this.mStompCount == 0)
					{
						this.mTauntQueue.Clear();
						TauntText tauntText2 = new TauntText();
						this.mTauntQueue.Add(tauntText2);
						tauntText2.mText = TextManager.getInstance().getString(403);
						tauntText2.mDelay = Common._M(300);
						tauntText2.mTextId = 403;
					}
				}
				else
				{
					this.mY += Common._M(10f);
					if (this.mStompCount == 0 && this.mY >= this.mStompRestingY / Common._M(1.8f))
					{
						gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_TIGER_BOSS_SMASH));
						this.mY = this.mStompRestingY / Common._M(1.8f);
						this.mStompPause = Common._M(25);
						gApp.GetBoard().ShakeScreen(Common._M(50), Common._M1(3), Common._M2(3));
						this.mStompCount++;
					}
					else if (this.mStompCount == 1 && this.mY >= this.mStompRestingY)
					{
						gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_TIGER_BOSS_SMASH));
						gApp.GetBoard().ShakeScreen(Common._M(30), Common._M1(3), Common._M2(3));
						this.mY = this.mStompRestingY;
						TauntText tauntText3 = new TauntText();
						this.mTauntQueue.Add(tauntText3);
						tauntText3.mText = TextManager.getInstance().getString(404);
						tauntText3.mDelay = Common._M(400);
						tauntText3.mTextId = 404;
						this.mTutorialState = 5;
					}
				}
			}
			else if (this.mTutorialState == 5 && this.mTauntQueue.Count <= 1 && !this.mLevel.AllCurvesAtRolloutPoint())
			{
				gApp.GetBoard().mPreventBallAdvancement = false;
			}
			if (this.mLevel.AllCurvesAtRolloutPoint())
			{
				if (this.mTutorialState == 5)
				{
					if (!gApp.GetBoard().mPreventBallAdvancement)
					{
						gApp.GetBoard().mPreventBallAdvancement = true;
						TauntText tauntText4 = new TauntText();
						this.mTauntQueue.Add(tauntText4);
						tauntText4.mText = TextManager.getInstance().getString(405);
						tauntText4.mDelay = Common._M(1000);
						tauntText4.mTextId = 405;
					}
				}
				else if (SexyFramework.Common._geq(this.mAlphaOverride, 255f))
				{
					BossFiringState[] array = new BossFiringState[] { this.mLeftPaw, this.mRightPaw };
					int[] array2 = new int[] { -1, 1 };
					for (int k = 0; k < 2; k++)
					{
						if (array[k].mState == 1)
						{
							array[k].mPawYOffset -= Common._M(3f);
							array[k].mSkullYOffset -= Common._M(3f);
							float num3 = Common._M(-15f);
							if (array[k].mPawYOffset < num3)
							{
								array[k].mSkullYOffset = num3;
								array[k].mPawYOffset = num3;
								array[k].mState++;
							}
							if (this.mPawModifyingHeadAngle == array2[k])
							{
								array[k].mHeadAngle += (float)array2[k] * Common._M(0.005f);
							}
						}
						else if (array[k].mState == 2)
						{
							if (++array[k].mTimer >= Common._M(5))
							{
								array[k].mTimer = 0;
								array[k].mState++;
							}
							if (this.mPawModifyingHeadAngle == array2[k])
							{
								array[k].mHeadAngle += (float)array2[k] * Common._M(0.005f);
							}
						}
						else if (array[k].mState == 3)
						{
							array[k].mSkullYOffset += Common._M(2f);
							if ((array[k].mPawYOffset += Common._M(2f)) >= 0f)
							{
								array[k].mPawYOffset = 0f;
								array[k].mSkullYOffset = 0f;
								array[k].mState++;
							}
							if (this.mPawModifyingHeadAngle == array2[k])
							{
								array[k].mHeadAngle -= (float)array2[k] * Common._M(0.01f);
							}
						}
						else if (array[k].mState == 4)
						{
							int num4 = Common._M(15);
							float num5 = 255f / (float)(num4 + Common._M(4));
							if (array[k].mTimer == 0)
							{
								if (this.mPawModifyingHeadAngle == array2[k])
								{
									array[k].mHeadAngle -= (float)array2[k] * Common._M(0.01f);
								}
								BossBullet bossBullet2 = null;
								for (int l = 0; l < this.mBullets.Count; l++)
								{
									if (this.mBullets[l].mId == array[k].mBulletId)
									{
										bossBullet2 = this.mBullets[l];
										break;
									}
								}
								array[k].mSkullYOffset += Common._M(7.5f);
								array[k].mSkullXOffset += bossBullet2.mVX;
								array[k].mSkullAngle += array[k].mSkullAngleInc;
								if (this.mUpdateCount % Common._M(4) == 0)
								{
									if (array[k].mSwipeFrame == 1)
									{
										array[k].mStreaksAlpha = 255f;
									}
									if (++array[k].mSwipeFrame >= Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_SWIPE).mNumCols)
									{
										array[k].mSwipeFrame--;
										Image image = BossTiger.gTigerBulletImages[bossBullet2.mImageNum];
										int num6 = Common._SS(image.mWidth);
										int num7 = Common._SS(image.mHeight);
										bossBullet2.mState++;
										if (bossBullet2.mData != null)
										{
											((Skull)bossBullet2.mData).mLaunched = true;
										}
										bossBullet2.mAngle = array[k].mSkullAngle;
										bossBullet2.mX = this.mX - (float)(this.mWidth / 2) + (float)(num6 / 2) + (float)((bossBullet2.mImageNum == 2) ? BossTiger.SKULL_RIGHT_X_OFF : BossTiger.SKULL_LEFT_X_OFF) + array[k].mSkullXOffset;
										bossBullet2.mY = this.mY - (float)(this.mHeight / 2) + (float)(num7 / 2) + (float)((bossBullet2.mImageNum == 2) ? BossTiger.SKULL_RIGHT_Y_OFF : BossTiger.SKULL_LEFT_Y_OFF) + array[k].mSkullYOffset;
										array[k].mBulletId = this.mBullets[0].mId;
										array[k].mSkullYOffset = (array[k].mSkullXOffset = 0f);
										array[k].mSkullAngle = (array[k].mTargetSkullAngle = (array[k].mSkullAngleInc = 0f));
										array[k].mSkullGrowPct = 0f;
										array[k].mTimer = num4;
									}
								}
							}
							else if (--array[k].mTimer == 0)
							{
								array[k].mState++;
							}
							if (array[k].mSwipeFrame == 2 && (array[k].mStreaksAlpha -= num5) < 0f)
							{
								array[k].mStreaksAlpha = 0f;
							}
						}
						else if (array[k].mState == 5)
						{
							int num8 = Common._M(5);
							float num9 = 255f / (float)(num8 + Common._M(5));
							if (array[k].mTimer > 0)
							{
								array[k].mTimer--;
							}
							else if (this.mUpdateCount % Common._M(5) == 0)
							{
								if (--array[k].mSwipeFrame < 0)
								{
									array[k].mState++;
								}
								else if (array[k].mSwipeFrame == 0)
								{
									array[k].mTimer = num8;
									array[k].mStreaksAlpha = 255f;
								}
							}
							if (array[k].mSwipeFrame == 0 && (array[k].mStreaksAlpha -= num9) < 0f)
							{
								array[k].mStreaksAlpha = 0f;
							}
							if (array[k].mTimer == 0 && this.mPawModifyingHeadAngle == array2[k] && array[k].mSwipeFrame < 2)
							{
								array[k].mHeadAngle += (float)array2[k] * Common._M(0.04f);
								if ((array2[k] > 0 && array[k].mHeadAngle >= 0f) || (array2[k] < 0 && array[k].mHeadAngle <= 0f))
								{
									array[k].mHeadAngle = 0f;
								}
							}
						}
						else if (array[k].mState == 6)
						{
							array[k].mSkullGrowPct += Common._M(0.075f);
							if (array[k].mSkullGrowPct >= Common._M(1.1f))
							{
								array[k].mState++;
							}
						}
						else if (array[k].mState == 7)
						{
							array[k].mSkullGrowPct -= Common._M(0.008f);
							if (array[k].mSkullGrowPct <= 1f)
							{
								array[k].mSkullGrowPct = 1f;
								array[k].mState = 0;
								if (this.mPawModifyingHeadAngle == array2[k])
								{
									this.mPawModifyingHeadAngle = 0;
								}
							}
						}
					}
				}
			}
			base.Update(f);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x000262BC File Offset: 0x000244BC
		public override void SyncState(DataSync sync)
		{
			base.SyncState(sync);
			sync.SyncLong(ref this.mPawModifyingHeadAngle);
			BossFiringState[] array = new BossFiringState[] { this.mLeftPaw, this.mRightPaw };
			for (int i = 0; i < 2; i++)
			{
				sync.SyncLong(ref array[i].mState);
				sync.SyncFloat(ref array[i].mPawYOffset);
				sync.SyncFloat(ref array[i].mSkullXOffset);
				sync.SyncFloat(ref array[i].mSkullYOffset);
				sync.SyncFloat(ref array[i].mSkullAngle);
				sync.SyncFloat(ref array[i].mHeadAngle);
				sync.SyncFloat(ref array[i].mSkullGrowPct);
				sync.SyncFloat(ref array[i].mTargetSkullAngle);
				sync.SyncFloat(ref array[i].mSkullAngleInc);
				sync.SyncFloat(ref array[i].mStreaksAlpha);
				sync.SyncLong(ref array[i].mSwipeFrame);
				sync.SyncLong(ref array[i].mTimer);
				sync.SyncLong(ref array[i].mBulletId);
			}
			sync.SyncLong(ref this.mTutorialState);
			sync.SyncFloat(ref this.mStompVX);
			sync.SyncFloat(ref this.mStompVY);
			sync.SyncFloat(ref this.mStompRestingY);
			sync.SyncFloat(ref this.mStompAY);
			sync.SyncLong(ref this.mStompPause);
			sync.SyncLong(ref this.mStompCount);
			SexyBuffer buffer = sync.GetBuffer();
			if (sync.isWrite())
			{
				int num = 0;
				for (int j = 0; j < this.mBullets.Count; j++)
				{
					if (this.mBullets[j].mData != null)
					{
						num++;
					}
				}
				buffer.WriteLong((long)num);
				for (int k = 0; k < this.mBullets.Count; k++)
				{
					if (this.mBullets[k].mData != null)
					{
						this.SyncSkull(sync, (Skull)this.mBullets[k].mData);
					}
				}
				buffer.WriteLong((long)this.mSkulls.Count);
				for (int l = 0; l < this.mSkulls.Count; l++)
				{
					this.SyncSkull(sync, this.mSkulls[l]);
				}
			}
			else
			{
				int num2 = (int)buffer.ReadLong();
				for (int m = 0; m < num2; m++)
				{
					this.SyncSkull(sync, null);
				}
				int num3 = (int)buffer.ReadLong();
				for (int n = 0; n < num3; n++)
				{
					this.SyncSkull(sync, null);
				}
			}
			if (this.mTutorialState == 2)
			{
				this.mTauntQueue.Clear();
				TauntText tauntText = new TauntText();
				this.mTauntQueue.Add(tauntText);
				tauntText.mText = TextManager.getInstance().getString(406);
				tauntText.mDelay = Common._M(300);
				tauntText.mTextId = 406;
				tauntText = new TauntText();
				this.mTauntQueue.Add(tauntText);
				tauntText.mText = TextManager.getInstance().getString(407);
				tauntText.mDelay = Common._M(300);
				tauntText.mTextId = 407;
				tauntText = new TauntText();
				this.mTauntQueue.Add(tauntText);
				tauntText.mText = TextManager.getInstance().getString(408);
				tauntText.mDelay = Common._M(500);
				tauntText.mTextId = 408;
			}
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00026620 File Offset: 0x00024820
		public override void Init(Level l)
		{
			this.mWidth = 197;
			this.mHeight = 134;
			GameApp gApp = GameApp.gApp;
			base.Init(l);
			this.mBandagedImg = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_BANDAGED);
			BossTiger.gTigerBulletImages[0] = null;
			BossTiger.gTigerBulletImages[1] = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_SKULL_LEFT);
			BossTiger.gTigerBulletImages[2] = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_SKULL_RIGHT);
			BossTiger.gFiringImages[0] = null;
			BossTiger.gFiringImages[1] = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_SKULL_LEFT_PARTICLE);
			BossTiger.gFiringImages[2] = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_SKULL_RIGHT_PARTICLE);
			if (this.mTutorialState != 0)
			{
				gApp.GetBoard().mPreventBallAdvancement = true;
				if (this.mTutorialState == 1)
				{
					this.mStompRestingY = this.mY;
					this.mX = (float)Common._M(0);
					this.mY = (float)Common._M(-150);
					float num = (float)(Common._SS(gApp.mWidth) / 2 - Common._M(150));
					float num2 = (float)Common._M(40);
					this.mStompVX = (num - this.mX) / num2;
					this.mStompVY = (this.mStompRestingY - this.mY) / num2;
					this.mStompAY = 0f;
				}
				this.mTauntQueue.Clear();
			}
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0002675C File Offset: 0x0002495C
		public override Boss Instantiate()
		{
			BossTiger bossTiger = new BossTiger(this.mLevel);
			bossTiger.CopyFrom(this);
			if (GameApp.gApp.IsHardMode() || (GameApp.gApp.mUserProfile != null && GameApp.gApp.mUserProfile.GetAdvModeVars().mNumTimesZoneBeat[0] > 0))
			{
				bossTiger.mTutorialState = 0;
			}
			else
			{
				bossTiger.mTutorialState = 1;
			}
			bossTiger.mSkulls.Clear();
			return bossTiger;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x000267CC File Offset: 0x000249CC
		protected void CopyFrom(BossTiger rhs)
		{
			base.CopyFrom(rhs);
			this.mPawModifyingHeadAngle = rhs.mPawModifyingHeadAngle;
			this.mTutorialState = rhs.mTutorialState;
			this.mStompVX = rhs.mStompVX;
			this.mStompVY = rhs.mStompVY;
			this.mStompAY = rhs.mStompAY;
			this.mStompRestingY = rhs.mStompRestingY;
			this.mStompPause = rhs.mStompPause;
			this.mStompCount = rhs.mStompCount;
			this.mSkulls.Clear();
			this.mSkulls.AddRange(rhs.mSkulls.ToArray());
			this.mLeftPaw = new BossFiringState(rhs.mLeftPaw);
			this.mRightPaw = new BossFiringState(rhs.mRightPaw);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00026884 File Offset: 0x00024A84
		public void ProcessYesNo(int theId)
		{
			if (theId == 1000)
			{
				GameApp.gApp.ToMarketPlace();
				return;
			}
			if (theId == 1001)
			{
				GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.DoDeferredEndGame);
				GameApp.gApp.ToggleBambooTransition();
				GameApp.gApp.mMusic.StopAll();
			}
		}

		// Token: 0x040002FA RID: 762
		protected static int PAW_LEFT_X_OFF = 0;

		// Token: 0x040002FB RID: 763
		protected static int PAW_RIGHT_X_OFF = 151;

		// Token: 0x040002FC RID: 764
		protected static int SKULL_LEFT_X_OFF = 2;

		// Token: 0x040002FD RID: 765
		protected static int SKULL_LEFT_Y_OFF = 72;

		// Token: 0x040002FE RID: 766
		protected static int SKULL_RIGHT_X_OFF = 147;

		// Token: 0x040002FF RID: 767
		protected static int SKULL_RIGHT_Y_OFF = 73;

		// Token: 0x04000300 RID: 768
		protected static Image[] gTigerBulletImages = new Image[3];

		// Token: 0x04000301 RID: 769
		protected static Image[] gFiringImages = new Image[3];

		// Token: 0x04000302 RID: 770
		protected List<Skull> mSkulls = new List<Skull>();

		// Token: 0x04000303 RID: 771
		protected BossFiringState mLeftPaw = new BossFiringState();

		// Token: 0x04000304 RID: 772
		protected BossFiringState mRightPaw = new BossFiringState();

		// Token: 0x04000305 RID: 773
		protected int mPawModifyingHeadAngle;

		// Token: 0x04000306 RID: 774
		protected int mTutorialState;

		// Token: 0x04000307 RID: 775
		protected float mStompVX;

		// Token: 0x04000308 RID: 776
		protected float mStompVY;

		// Token: 0x04000309 RID: 777
		protected float mStompAY;

		// Token: 0x0400030A RID: 778
		protected float mStompRestingY;

		// Token: 0x0400030B RID: 779
		protected int mStompPause;

		// Token: 0x0400030C RID: 780
		protected int mStompCount;

		// Token: 0x0400030D RID: 781
		protected List<Skull> mSkullsPool = new List<Skull>();

		// Token: 0x0200003C RID: 60
		private enum Turtorial
		{
			// Token: 0x0400030F RID: 783
			Tutorial_None,
			// Token: 0x04000310 RID: 784
			Tutorial_HitMeNoBalls,
			// Token: 0x04000311 RID: 785
			Tutorial_NoBallsTaunting,
			// Token: 0x04000312 RID: 786
			Tutorial_NoBallsRecoiling,
			// Token: 0x04000313 RID: 787
			Tutorial_NoBallsMovingBack,
			// Token: 0x04000314 RID: 788
			Tutorial_HitMeBalls
		}
	}
}
