using System;
using System.Collections.Generic;
using System.Linq;
using JeffLib;
using Microsoft.Xna.Framework;
using SexyFramework;
using SexyFramework.AELib;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.PIL;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x020000E7 RID: 231
	public class Gun : PopAnimListener
	{
		// Token: 0x06000C33 RID: 3123 RVA: 0x000718D4 File Offset: 0x0006FAD4
		public static void PreLayerDraw(Graphics g, Layer l, object data)
		{
			Gun gun = (Gun)data;
			gun.SickFrogPreLayerDraw(g, l);
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x000718F0 File Offset: 0x0006FAF0
		public void PopAnimPlaySample(string theSampleName, int thePan, double theVolume, double theNumSteps)
		{
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x000718F2 File Offset: 0x0006FAF2
		public PIEffect PopAnimLoadParticleEffect(string theEffectName)
		{
			return null;
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x000718F5 File Offset: 0x0006FAF5
		public bool PopAnimObjectPredraw(int theId, Graphics g, PASpriteInst theSpriteInst, PAObjectInst theObjectInst, PATransform theTransform, SexyColor theColor)
		{
			return true;
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x000718F8 File Offset: 0x0006FAF8
		public bool PopAnimObjectPostdraw(int theId, Graphics g, PASpriteInst theSpriteInst, PAObjectInst theObjectInst, PATransform theTransform, SexyColor theColor)
		{
			return true;
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x000718FB File Offset: 0x0006FAFB
		public ImagePredrawResult PopAnimImagePredraw(int theId, PASpriteInst theSpriteInst, PAObjectInst theObjectInst, PATransform theTransform, Image theImage, Graphics g, int theDrawCount)
		{
			return ImagePredrawResult.ImagePredraw_Normal;
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x000718FE File Offset: 0x0006FAFE
		public void PopAnimStopped(int theId)
		{
			if (this.mBullet == null || !this.mBullet.GetIsCannon())
			{
				this.mLightningEffect = null;
				return;
			}
			this.mLightningEffect.Play("Main");
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0007192E File Offset: 0x0006FB2E
		public void PopAnimCommand(int theId, string theCommand, string theParam)
		{
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x00071930 File Offset: 0x0006FB30
		public bool PopAnimCommand(int theId, PASpriteInst theSpriteInst, string theCommand, string theParam)
		{
			this.PopAnimCommand(theId, theCommand, theParam);
			return true;
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x00071940 File Offset: 0x0006FB40
		private static void RotateXY(ref float x, ref float y, float cx, float cy, float rad)
		{
			float num = x - cx;
			float num2 = y - cy;
			x = cx + num * (float)Math.Cos((double)rad) + num2 * (float)Math.Sin((double)rad);
			y = cy + num2 * (float)Math.Cos((double)rad) - num * (float)Math.Sin((double)rad);
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x00071990 File Offset: 0x0006FB90
		protected void CalcAngle()
		{
			if (this.mBullet == null)
			{
				return;
			}
			float num = this.mCurY + (float)this.mReloadPoint;
			float num2 = this.mCurY + (float)this.mFirePoint;
			float num3 = this.mCurY + (float)this.mBallPoint;
			float num4 = this.mCurX - 2f;
			float num5;
			if (this.mState == GunState.GunState_Normal)
			{
				num5 = num3;
			}
			else if (this.mState == GunState.GunState_Reloading)
			{
				num5 = num + (num3 - num) * this.mStatePercent;
			}
			else
			{
				if (this.mStatePercent > 0.6f)
				{
					return;
				}
				num5 = num3 + (num2 - num3) * this.mStatePercent / 0.6f;
			}
			Gun.RotateXY(ref num4, ref num5, this.mCurX, this.mCurY, this.mAngle);
			if (this.mState == GunState.GunState_Reloading && ZumasRevenge.Common.gSuckMode && (this.mBX != 0 || this.mBY != 0))
			{
				num4 = num4 * this.mStatePercent + (float)this.mBX * (1f - this.mStatePercent);
				num5 = num5 * this.mStatePercent + (float)this.mBY * (1f - this.mStatePercent);
			}
			this.mBullet.SetPos(num4, num5);
			this.mBullet.SetRotation(this.mAngle);
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x00071AC4 File Offset: 0x0006FCC4
		protected void SetAngleToDestAngle()
		{
			while (this.mDestAngle < 0f)
			{
				this.mDestAngle += 6.28318f;
			}
			while (this.mDestAngle > 6.28318f)
			{
				this.mDestAngle -= 6.28318f;
			}
			this.mAngle = this.mDestAngle;
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x00071B20 File Offset: 0x0006FD20
		protected void UpdateCannonFadeIn()
		{
			if (this.mCannonState == 1)
			{
				this.mCannonRuneAlpha += (int)ZumasRevenge.Common._M(15f);
				if (this.mCannonRuneAlpha >= 255)
				{
					this.mCannonRuneAlpha = 255;
					this.mCannonState++;
					return;
				}
			}
			else if (this.mCannonState == 2)
			{
				this.mCannonLightness += (int)ZumasRevenge.Common._M(5f);
				if (this.mCannonLightness >= 100)
				{
					this.mCannonLightness = 100;
					this.mCannonState++;
					return;
				}
			}
			else if (this.mCannonState == 3)
			{
				this.mCannonLightness -= (int)ZumasRevenge.Common._M(10f);
				if (this.mCannonLightness <= 0)
				{
					this.mCannonLightness = 0;
					this.mCannonState = -1;
				}
			}
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00071BF4 File Offset: 0x0006FDF4
		protected void SetupLazerBackPulse()
		{
			this.mLazerPulse.Clear();
			int num = ZumasRevenge.Common._M(40);
			this.mLazerPulse.Add(new Component(0f, 255f, this.mUpdateCount, this.mUpdateCount + num));
			this.mLazerPulse.Add(new Component(255f, 0f, this.mUpdateCount + num + 1, this.mUpdateCount + num * 2 + 1));
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x00071C6C File Offset: 0x0006FE6C
		protected void SetFrogType(FrogType t, bool current)
		{
			if (!GameApp.gApp.mShutdown)
			{
				GameApp.gApp.mSoundPlayer.Stop(Res.GetSoundByID(ResID.SOUND_LIGHTNING_LOOP));
			}
			FrogBody frogBody;
			if (current)
			{
				frogBody = this.mCurrentBody;
				frogBody.mAlpha = 255;
			}
			else
			{
				if (Enumerable.Count<FrogBody>(this.mFrogStack) > 0 && this.mFrogStack[Enumerable.Count<FrogBody>(this.mFrogStack) - 1].mType == t)
				{
					return;
				}
				if (this.mCurrentBody.mType == t)
				{
					this.mFrogStack.Clear();
					if (t == FrogType.FrogType_Lightning)
					{
						this.ResetBeams();
					}
					if (this.mDoElectricBeamShit)
					{
						this.mDoElectricBeamShit = false;
						this.mBoard.mLevel.DeactivateLightningEffects();
					}
					return;
				}
				frogBody = new FrogBody();
				this.mFrogStack.Add(frogBody);
				frogBody.mAlpha = 0;
			}
			frogBody.mTongueX = 52;
			frogBody.mCX = Gun.FROG_WIDTH / 2;
			frogBody.mCY = Gun.FROG_HEIGHT / 2;
			this.mReloadPoint = -20;
			this.mFirePoint = 8;
			this.mBallPoint = 31;
			this.mBallXOff = (this.mBallYOff = 0);
			frogBody.mNextBallX = (int)ZumasRevenge.Common._M(62f);
			frogBody.mNextBallY = (int)ZumasRevenge.Common._M(25f);
			frogBody.mShadow = Res.GetImageByID(ResID.IMAGE_FROG_SHADOW);
			frogBody.mTongue = Res.GetImageByID(ResID.IMAGE_FROG_TONGUE);
			frogBody.mType = t;
			if (this.mBullet != null && !this.mBullet.GetJustFired())
			{
				this.mBullet.SetIsCannon(false);
			}
			if (this.mNextBullet != null && !this.mNextBullet.GetJustFired())
			{
				this.mNextBullet.SetIsCannon(false);
			}
			if (t != FrogType.FrogType_Lazer && this.mBoard.GetGuideBall() != null)
			{
				this.mBoard.GetGuideBall().DoLaserAnim(false);
			}
			if (t == FrogType.FrogType_Normal)
			{
				frogBody.mEyes = Res.GetImageByID(ResID.IMAGE_FROG_NORMAL_EYES);
				frogBody.mLegs = Res.GetImageByID(ResID.IMAGE_FROG_NORMAL_LEGS);
				frogBody.mMouth = Res.GetImageByID(ResID.IMAGE_FROG_NORMAL_MOUTH);
				frogBody.mBody = Res.GetImageByID(ResID.IMAGE_FROG_NORMAL_BODY);
				frogBody.mMouthOffset = new SexyPoint(ZumasRevenge.Common._M(26), (t == FrogType.FrogType_Normal) ? ((int)ZumasRevenge.Common._M1(79f)) : 82);
				if (t == FrogType.FrogType_Normal)
				{
					frogBody.mLegsOffset = new SexyPoint(ZumasRevenge.Common._M(2), (int)ZumasRevenge.Common._M1(38f));
					this.mBallYOff = (int)ZumasRevenge.Common._M(0f);
				}
				else
				{
					frogBody.mLegsOffset = new SexyPoint(1, (int)ZumasRevenge.Common._M(41f));
				}
				frogBody.mBodyOffset = new SexyPoint((int)ZumasRevenge.Common._M(16f), (int)ZumasRevenge.Common._M1(4f));
				frogBody.mEyesOffset = new SexyPoint(32, (int)ZumasRevenge.Common._M(47f));
				this.mCannonState = -1;
				if (t == FrogType.FrogType_Lightning)
				{
					if (!GameApp.gApp.GetBoard().IsLoading())
					{
						GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_LIGHTNING_TRIGGER));
						GameApp.gApp.mSoundPlayer.Loop(Res.GetSoundByID(ResID.SOUND_LIGHTNING_LOOP));
					}
					this.InitBeams();
					return;
				}
			}
			else
			{
				if (t == FrogType.FrogType_Lightning)
				{
					frogBody.mEyes = null;
					frogBody.mLegs = Res.GetImageByID(ResID.IMAGE_FROG_LIGHTNING_BOTTOM);
					frogBody.mMouth = null;
					frogBody.mBody = Res.GetImageByID(ResID.IMAGE_FROG_LIGHTNING_TOP);
					frogBody.mLegsOffset = new SexyPoint((int)ZumasRevenge.Common._M(11f), (int)ZumasRevenge.Common._M1(78f));
					frogBody.mBodyOffset = new SexyPoint((int)ZumasRevenge.Common._M(0f), (int)ZumasRevenge.Common._M1(0f));
					frogBody.mEyesOffset = new SexyPoint((int)ZumasRevenge.Common._M(32f), (int)ZumasRevenge.Common._M1(47f));
					this.mCannonState = -1;
					if (!GameApp.gApp.GetBoard().IsLoading())
					{
						GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_LIGHTNING_TRIGGER));
						GameApp.gApp.mSoundPlayer.Loop(Res.GetSoundByID(ResID.SOUND_LIGHTNING_LOOP));
					}
					this.InitBeams();
					return;
				}
				if (t == FrogType.FrogType_Lazer)
				{
					for (int i = 0; i < 4; i++)
					{
						this.mLazerFrogBackPulseAlpha[i] = 0f;
					}
					if (!GameApp.gApp.GetBoard().IsLoading())
					{
						GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_LASER_TRIGGER));
					}
					this.mDoElectricBeamShit = false;
					frogBody.mEyes = null;
					frogBody.mLegs = null;
					frogBody.mMouth = null;
					frogBody.mBody = Res.GetImageByID(ResID.IMAGE_FROG_LAZER_BODY);
					frogBody.mBodyOffset = new SexyPoint((int)ZumasRevenge.Common._M(0f), (int)ZumasRevenge.Common._M1(6f));
					frogBody.mEyesOffset = new SexyPoint((int)ZumasRevenge.Common._M(31f), (int)ZumasRevenge.Common._M1(45f));
					frogBody.mLazerEyeLoop = Res.GetImageByID(ResID.IMAGE_FROG_LAZER_EYE_LOOP);
					frogBody.mCel = 0;
					if (this.mLazerPulse.Count == 0)
					{
						this.SetupLazerBackPulse();
					}
					this.mCannonState = -1;
					return;
				}
				if (t == FrogType.FrogType_Cannon)
				{
					if (!GameApp.gApp.GetBoard().IsLoading())
					{
						GameApp.gApp.PlaySample(this.mBoard.LevelIsSkeletonBoss() ? Res.GetSoundByID(ResID.SOUND_SKELETONHIT_POWERUP) : Res.GetSoundByID(ResID.SOUND_CANNON_POWERUP));
					}
					this.mDoElectricBeamShit = false;
					frogBody.mEyes = Res.GetImageByID(ResID.IMAGE_FROG_CANNON_EYES);
					frogBody.mLegs = Res.GetImageByID(ResID.IMAGE_FROG_CANNON_LEGS);
					frogBody.mMouth = Res.GetImageByID(ResID.IMAGE_FROG_CANNON_MOUTH);
					frogBody.mBody = Res.GetImageByID(ResID.IMAGE_FROG_CANNON_BODY);
					frogBody.mLegsOffset = new SexyPoint(1, 41);
					frogBody.mMouthOffset = new SexyPoint((int)ZumasRevenge.Common._M(26f), (int)ZumasRevenge.Common._M1(74f));
					frogBody.mBodyOffset = new SexyPoint(16, 5);
					frogBody.mEyesOffset = new SexyPoint(32, 47);
					frogBody.mNextBallX = (int)ZumasRevenge.Common._M(62f);
					frogBody.mNextBallY = (int)ZumasRevenge.Common._M(25f);
					for (int j = 0; j < Gun.NUM_CANNON_SHADOWS; j++)
					{
						this.mCannonBallShadows[j] = 0;
					}
					this.mCannonBallShadowPos = 0;
				}
			}
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x00072260 File Offset: 0x00070460
		protected void DrawFrogBase(Graphics g, FrogBody fb)
		{
			int num = fb.mAlpha;
			if (this.mBoard.GetDarkFrogLevelFadeInAlpha() >= 0)
			{
				num = 255 - this.mBoard.GetDarkFrogLevelFadeInAlpha();
				num = Math.Min(Math.Max(num, 0), 255);
			}
			g.PushState();
			if (num != 255 && num != -1)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, fb.mAlpha);
			}
			int num2 = (int)this.mCurX - fb.mCX + GameApp.gScreenShakeX;
			int num3 = (int)this.mCurY - fb.mCY + GameApp.gScreenShakeY;
			Rect celRect = fb.mShadow.GetCelRect(0);
			if (g.Is3D())
			{
				g.DrawImageRotatedF(fb.mShadow, (float)ZumasRevenge.Common._S(num2 + ZumasRevenge.Common._M(-2)), ZumasRevenge.Common._S((float)num3 - this.mRecoilAmt / 2f + (float)ZumasRevenge.Common._M1(3)), (double)this.mAngle, (float)ZumasRevenge.Common._S(fb.mCX), ZumasRevenge.Common._S((float)fb.mCY + this.mRecoilAmt / 2f), celRect);
			}
			else
			{
				g.DrawImageRotated(fb.mShadow, ZumasRevenge.Common._S(num2 + ZumasRevenge.Common._M(-2)), (int)ZumasRevenge.Common._S((float)num3 - this.mRecoilAmt / 2f + (float)ZumasRevenge.Common._M1(3)), (double)this.mAngle, ZumasRevenge.Common._S(fb.mCX), (int)ZumasRevenge.Common._S((float)fb.mCY + this.mRecoilAmt / 2f), celRect);
			}
			GameApp.gApp.GetBoard().DrawGuide(g);
			if (fb.mLegs != null)
			{
				if (this.mBossStateAlpha > 0f)
				{
					g.PushState();
					g.SetColorizeImages(true);
					int num4 = (int)(255f - this.mBossStateAlpha);
					num4 = Math.Min(Math.Max(0, num4), 255);
					g.SetColor(255, 255, 255, num4);
				}
				celRect = fb.mLegs.GetCelRect(0);
				if (g.Is3D())
				{
					g.DrawImageRotated(fb.mLegs, ZumasRevenge.Common._S(num2 + fb.mLegsOffset.mX), ZumasRevenge.Common._S(num3 + fb.mLegsOffset.mY), (double)this.mAngle, ZumasRevenge.Common._S(fb.mCX - fb.mLegsOffset.mX), ZumasRevenge.Common._S(fb.mCY - fb.mLegsOffset.mY), celRect);
				}
				else
				{
					g.DrawImageRotated(fb.mLegs, ZumasRevenge.Common._S(num2 + fb.mLegsOffset.mX), ZumasRevenge.Common._S(num3 + fb.mLegsOffset.mY), (double)this.mAngle, ZumasRevenge.Common._S(fb.mCX - fb.mLegsOffset.mX), ZumasRevenge.Common._S(fb.mCY - fb.mLegsOffset.mY), celRect);
				}
				if (this.mBossStateAlpha > 0f)
				{
					g.PopState();
				}
			}
			if (this.mLightningEffect != null)
			{
				this.mLightningEffect.Draw(g);
			}
			if (fb.mMouth != null)
			{
				int num5 = 0;
				if (fb.mMouth.mNumRows > 1 && this.mState != GunState.GunState_Normal)
				{
					if (fb.mType != FrogType.FrogType_Cannon)
					{
						num5 = 1;
						if (num5 >= fb.mMouth.mNumRows)
						{
							num5 = fb.mMouth.mNumRows - 1;
						}
					}
					else if (this.mState == GunState.GunState_Reloading)
					{
						num5 = 1;
					}
					else
					{
						num5 = 0;
					}
				}
				if (this.mBossStateAlpha > 0f)
				{
					g.PushState();
					g.SetColorizeImages(true);
					int alpha = (int)(255f - this.mBossStateAlpha);
					alpha = Math.Min(Math.Max(0, num), 255);
					g.SetColor(255, 255, 255, alpha);
				}
				celRect = fb.mMouth.GetCelRect(num5);
				if (g.Is3D())
				{
					g.DrawImageRotatedF(fb.mMouth, (float)ZumasRevenge.Common._S(num2 + fb.mMouthOffset.mX), ZumasRevenge.Common._S((float)(num3 + fb.mMouthOffset.mY) - this.mRecoilAmt), (double)this.mAngle, (float)ZumasRevenge.Common._S(fb.mCX - fb.mMouthOffset.mX), ZumasRevenge.Common._S((float)(fb.mCY - fb.mMouthOffset.mY) + this.mRecoilAmt), celRect);
				}
				else
				{
					g.DrawImageRotated(fb.mMouth, ZumasRevenge.Common._S(num2 + fb.mMouthOffset.mX), (int)ZumasRevenge.Common._S((float)(num3 + fb.mMouthOffset.mY) - this.mRecoilAmt), (double)this.mAngle, ZumasRevenge.Common._S(fb.mCX - fb.mMouthOffset.mX), (int)ZumasRevenge.Common._S((float)(fb.mCY - fb.mMouthOffset.mY) + this.mRecoilAmt), celRect);
				}
				if (this.mBossStateAlpha > 0f)
				{
					g.PopState();
					if (this.mBossStateAlpha < 255f)
					{
						g.SetColorizeImages(true);
					}
					g.SetColor(255, 255, 255, (int)this.mBossStateAlpha);
					Image[] array = new Image[]
					{
						Res.GetImageByID(ResID.IMAGE_FROG_INKED_BOTTOM),
						Res.GetImageByID(ResID.IMAGE_PLAGUE_FROG_BOTTOM)
					};
					SexyPoint[] array2 = new SexyPoint[]
					{
						new SexyPoint(ZumasRevenge.Common._M(42), ZumasRevenge.Common._M1(16)),
						new SexyPoint(ZumasRevenge.Common._M2(42), ZumasRevenge.Common._M3(16))
					};
					int mX = array2[this.mBossState].mX;
					int mY = array2[this.mBossState].mY;
					if (g.Is3D())
					{
						g.DrawImageRotatedF(array[this.mBossState], (float)ZumasRevenge.Common._S(num2 + mX), ZumasRevenge.Common._S((float)(num3 + mY) - this.mRecoilAmt), (double)this.mAngle, (float)ZumasRevenge.Common._S(fb.mCX - mX), ZumasRevenge.Common._S((float)(fb.mCY - mY) + this.mRecoilAmt));
					}
					else
					{
						g.DrawImageRotatedF(array[this.mBossState], (float)ZumasRevenge.Common._S(num2 + mX), ZumasRevenge.Common._S((float)(num3 + mY) - this.mRecoilAmt), (double)this.mAngle, (float)ZumasRevenge.Common._S(fb.mCX - mX), ZumasRevenge.Common._S((float)(fb.mCY - mY) + this.mRecoilAmt));
					}
					g.SetColorizeImages(false);
				}
			}
			g.PopState();
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x000728A4 File Offset: 0x00070AA4
		protected void DrawFrogTongue(Graphics g, FrogBody fb)
		{
			if (fb.mType == FrogType.FrogType_Cannon || fb.mType == FrogType.FrogType_Lazer)
			{
				return;
			}
			int num = fb.mAlpha;
			if (this.mBoard.GetDarkFrogLevelFadeInAlpha() >= 0)
			{
				num = 255 - this.mBoard.GetDarkFrogLevelFadeInAlpha();
				num = Math.Min(Math.Max(num, 0), 255);
			}
			g.PushState();
			if (num != 255 && num != -1)
			{
				g.SetColor(255, 255, 255, fb.mAlpha);
				g.SetColorizeImages(true);
			}
			int num2 = int.MaxValue;
			int num3 = (int)this.mCurX - fb.mCX + GameApp.gScreenShakeX;
			int num4 = (int)this.mCurY - fb.mCY + GameApp.gScreenShakeY;
			switch (this.mState)
			{
			case GunState.GunState_Normal:
				num2 = Gun.TONGUE_Y2;
				break;
			case GunState.GunState_Firing:
				num2 = (int)((float)Gun.TONGUE_Y1 * this.mStatePercent + (float)Gun.TONGUE_Y2 * (1f - this.mStatePercent));
				break;
			case GunState.GunState_Reloading:
				num2 = (int)((float)Gun.TONGUE_Y2 * this.mStatePercent + (float)Gun.TONGUE_Y1 * (1f - this.mStatePercent));
				break;
			}
			if (this.mBullet == null)
			{
				num2 = Gun.TONGUE_YNOBALL;
			}
			if (g.Is3D())
			{
				g.DrawImageRotatedF(fb.mTongue, (float)ZumasRevenge.Common._S(num3 + fb.mTongueX), ZumasRevenge.Common._S((float)(num4 + num2) - this.mRecoilAmt), (double)this.mAngle, (float)ZumasRevenge.Common._S(fb.mCX - fb.mTongueX), ZumasRevenge.Common._S((float)(fb.mCY - num2) + this.mRecoilAmt));
			}
			else
			{
				g.DrawImageRotated(fb.mTongue, ZumasRevenge.Common._S(num3 + fb.mTongueX), (int)ZumasRevenge.Common._S((float)(num4 + num2) - this.mRecoilAmt), (double)this.mAngle, ZumasRevenge.Common._S(fb.mCX - fb.mTongueX), (int)ZumasRevenge.Common._S((float)(fb.mCY - num2) + this.mRecoilAmt));
			}
			g.PopState();
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00072AA0 File Offset: 0x00070CA0
		protected void DrawFrogTop(Graphics g, FrogBody fb)
		{
			int num = (int)this.mCurX - fb.mCX + GameApp.gScreenShakeX;
			int num2 = (int)this.mCurY - fb.mCY + GameApp.gScreenShakeY;
			g.PushState();
			int num3 = fb.mAlpha;
			if (this.mBoard.GetDarkFrogLevelFadeInAlpha() >= 0)
			{
				num3 = 255 - this.mBoard.GetDarkFrogLevelFadeInAlpha();
				num3 = Math.Min(Math.Max(num3, 0), 255);
			}
			if (this.mDoingCannonBlast && this.mCannonBlast.IsActive())
			{
				g.PushState();
				this.mCannonBlast.Draw(g);
				g.PopState();
			}
			if (num3 != 255 && num3 != -1)
			{
				g.SetColor(255, 255, 255, fb.mAlpha);
				g.SetColorizeImages(true);
			}
			else if (this.mFrogStack.size<FrogBody>() > 0 && this.mFrogStack.back<FrogBody>().mAlpha > 0)
			{
				g.SetColor(255, 255, 255, 255 - this.mFrogStack.back<FrogBody>().mAlpha);
				g.SetColorizeImages(true);
			}
			if (this.mBossStateAlpha > 0f)
			{
				g.PushState();
				g.SetColorizeImages(true);
				int num4 = (int)(255f - this.mBossStateAlpha);
				num4 = Math.Min(Math.Max(0, num4), 255);
				g.SetColor(255, 255, 255, num4);
			}
			if (GameApp.gApp.mBoard != null && GameApp.gApp.mBoard.mIsHotFrogEnabled)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 242, 0, 255);
			}
			if (g.Is3D())
			{
				g.DrawImageRotatedF(fb.mBody, (float)ZumasRevenge.Common._S(num + fb.mBodyOffset.mX), ZumasRevenge.Common._S((float)(num2 + fb.mBodyOffset.mY) - this.mRecoilAmt), (double)this.mAngle, (float)ZumasRevenge.Common._S(fb.mCX - fb.mBodyOffset.mX), ZumasRevenge.Common._S((float)(fb.mCY - fb.mBodyOffset.mY) + this.mRecoilAmt));
			}
			else
			{
				g.DrawImageRotated(fb.mBody, ZumasRevenge.Common._S(num + fb.mBodyOffset.mX), (int)ZumasRevenge.Common._S((float)(num2 + fb.mBodyOffset.mY) - this.mRecoilAmt), (double)this.mAngle, ZumasRevenge.Common._S(fb.mCX - fb.mBodyOffset.mX), (int)ZumasRevenge.Common._S((float)(fb.mCY - fb.mBodyOffset.mY) + this.mRecoilAmt));
			}
			if (GameApp.gApp.mBoard != null && GameApp.gApp.mBoard.mIsHotFrogEnabled)
			{
				g.SetColorizeImages(false);
			}
			if (this.mBossStateAlpha > 0f)
			{
				g.PopState();
			}
			if (this.mFrogStack.size<FrogBody>() > 0 && this.mFrogStack.back<FrogBody>().mAlpha > 0)
			{
				g.SetColorizeImages(false);
			}
			if (this.mBossStateAlpha > 0f)
			{
				if (this.mBossStateAlpha < 255f)
				{
					g.SetColorizeImages(true);
				}
				g.SetColor(255, 255, 255, (int)this.mBossStateAlpha);
				Image[] array = new Image[]
				{
					Res.GetImageByID(ResID.IMAGE_FROG_INKED_TOP),
					Res.GetImageByID(ResID.IMAGE_PLAGUE_FROG_TOP)
				};
				SexyPoint[] array2 = new SexyPoint[]
				{
					new SexyPoint(ZumasRevenge.Common._M(0), ZumasRevenge.Common._M1(4)),
					new SexyPoint(ZumasRevenge.Common._M2(0), ZumasRevenge.Common._M3(4))
				};
				int mX = array2[this.mBossState].mX;
				int mY = array2[this.mBossState].mY;
				if (g.Is3D())
				{
					g.DrawImageRotatedF(array[this.mBossState], (float)ZumasRevenge.Common._S(num + mX), ZumasRevenge.Common._S((float)(num2 + mY) - this.mRecoilAmt), (double)this.mAngle, (float)ZumasRevenge.Common._S(fb.mCX - mX), ZumasRevenge.Common._S((float)(fb.mCY - mY) + this.mRecoilAmt));
				}
				else
				{
					g.DrawImageRotatedF(array[this.mBossState], (float)ZumasRevenge.Common._S(num + mX), ZumasRevenge.Common._S((float)(num2 + mY) - this.mRecoilAmt), (double)this.mAngle, (float)ZumasRevenge.Common._S(fb.mCX - mX), ZumasRevenge.Common._S((float)(fb.mCY - mY) + this.mRecoilAmt));
				}
				g.SetColorizeImages(false);
			}
			if (fb.mType == FrogType.FrogType_Cannon)
			{
				if (this.mState == GunState.GunState_Reloading)
				{
					Rect celRect = fb.mMouth.GetCelRect(2);
					if (g.Is3D())
					{
						g.DrawImageRotatedF(fb.mMouth, (float)ZumasRevenge.Common._S(num + fb.mMouthOffset.mX), ZumasRevenge.Common._S((float)(num2 + fb.mMouthOffset.mY) - this.mRecoilAmt), (double)this.mAngle, (float)ZumasRevenge.Common._S(fb.mCX - fb.mMouthOffset.mX), ZumasRevenge.Common._S((float)(fb.mCY - fb.mMouthOffset.mY) + this.mRecoilAmt), celRect);
					}
					else
					{
						g.DrawImageRotated(fb.mMouth, ZumasRevenge.Common._S(num + fb.mMouthOffset.mX), (int)ZumasRevenge.Common._S((float)(num2 + fb.mMouthOffset.mY) - this.mRecoilAmt), (double)this.mAngle, ZumasRevenge.Common._S(fb.mCX - fb.mMouthOffset.mX), (int)ZumasRevenge.Common._S((float)(fb.mCY - fb.mMouthOffset.mY) + this.mRecoilAmt), celRect);
					}
				}
			}
			else if (fb.mType == FrogType.FrogType_Lightning)
			{
				float num5 = (float)(ZumasRevenge.Common._M(50) + fb.mBodyOffset.mX);
				float num6 = (float)(ZumasRevenge.Common._M(-6) + fb.mBodyOffset.mY);
				int num7 = JeffLib.Common.GetAlphaFromUpdateCount(this.mUpdateCount, ZumasRevenge.Common._M(64)) * ZumasRevenge.Common._M1(4);
				if (num7 > 255)
				{
					num7 = 255;
				}
				if (g.Is3D())
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, num7);
					if (ZumasRevenge.Common._M(0) == 0)
					{
						g.SetDrawMode(1);
					}
					g.DrawImageRotatedF(Res.GetImageByID(ResID.IMAGE_FROG_BOLT), ZumasRevenge.Common._S((float)num + num5), ZumasRevenge.Common._S((float)num2 + num6 - this.mRecoilAmt), (double)this.mAngle, ZumasRevenge.Common._S((float)fb.mCX - num5), ZumasRevenge.Common._S((float)fb.mCY - num6 + this.mRecoilAmt));
					g.SetColorizeImages(false);
					g.SetDrawMode(0);
				}
				else
				{
					g.DrawImageRotated(Res.GetImageByID(ResID.IMAGE_FROG_BOLT), (int)ZumasRevenge.Common._S((float)num + num5), (int)ZumasRevenge.Common._S((float)num2 + num6 - this.mRecoilAmt), (double)this.mAngle, (int)ZumasRevenge.Common._S((float)fb.mCX - num5), (int)ZumasRevenge.Common._S((float)fb.mCY - num6 + this.mRecoilAmt));
				}
			}
			if ((this.mState != GunState.GunState_Normal || this.IsStunned() || this.mBlinkCount >= 0) && fb.mEyes != null && fb.mEyes.mNumRows > 1 && this.mBossStateAlpha <= 0f)
			{
				int num8 = 0;
				if (this.mBlinkCount >= 0)
				{
					num8 = ((this.mBlinkCount % 2 == 0) ? (-1) : 1);
				}
				else if (this.IsStunned())
				{
					num8 = ((fb.mEyes.mNumCols > fb.mEyes.mNumRows) ? (fb.mEyes.mNumCols - 1) : (fb.mEyes.mNumRows - 1));
				}
				else if (this.mState != GunState.GunState_Firing)
				{
					num8 = 1;
				}
				if (num8 >= 0)
				{
					Rect celRect2 = fb.mEyes.GetCelRect(num8);
					if (g.Is3D())
					{
						g.DrawImageRotatedF(fb.mEyes, (float)ZumasRevenge.Common._S(num + fb.mEyesOffset.mX), ZumasRevenge.Common._S((float)(num2 + fb.mEyesOffset.mY) - this.mRecoilAmt), (double)this.mAngle, (float)ZumasRevenge.Common._S(fb.mCX - fb.mEyesOffset.mX), ZumasRevenge.Common._S((float)(fb.mCY - fb.mEyesOffset.mY) + this.mRecoilAmt), celRect2);
					}
					else
					{
						g.DrawImageRotated(fb.mEyes, ZumasRevenge.Common._S(num + fb.mEyesOffset.mX), (int)ZumasRevenge.Common._S((float)(num2 + fb.mEyesOffset.mY) - this.mRecoilAmt), (double)this.mAngle, ZumasRevenge.Common._S(fb.mCX - fb.mEyesOffset.mX), (int)ZumasRevenge.Common._S((float)(fb.mCY - fb.mEyesOffset.mY) + this.mRecoilAmt), celRect2);
					}
				}
			}
			else if (fb.mType == FrogType.FrogType_Lazer)
			{
				bool flag = fb == this.mCurrentBody && this.mFrogStack.size<FrogBody>() > 0 && this.mFrogStack.back<FrogBody>().mAlpha > 0 && this.mFrogStack.back<FrogBody>().mType != this.mCurrentBody.mType;
				int num9 = 0;
				if (flag)
				{
					num9 = 255 - this.mFrogStack.back<FrogBody>().mAlpha;
				}
				int num10 = (int)Component.GetComponentValue(this.mLazerPulse, 0f, this.mUpdateCount);
				if (!flag)
				{
					if (fb.mAlpha >= 0)
					{
						Math.Min(fb.mAlpha, num10);
					}
				}
				else
				{
					Math.Min(num9, num10);
				}
				int num11 = ZumasRevenge.Common._M(0);
				int num12 = ZumasRevenge.Common._M(-4);
				Rect theSrcRect = default(Rect);
				g.PushState();
				if (flag)
				{
					g.SetColor(255, 255, 255, num9);
					g.SetColorizeImages(true);
				}
				else
				{
					num9 = ((fb.mAlpha >= 0) ? fb.mAlpha : 255);
				}
				int num13 = ZumasRevenge.Common._M(19);
				int num14 = ZumasRevenge.Common._M(33);
				g.SetColorizeImages(true);
				g.SetDrawMode(1);
				g.SetColor(255, 255, 0, num9);
				theSrcRect = Res.GetImageByID(ResID.IMAGE_FROG_LAZER_EYE_LOOP).GetCelRect(fb.mCel);
				if (g.Is3D())
				{
					g.DrawImageRotatedF(fb.mLazerEyeLoop, (float)ZumasRevenge.Common._S(num + num13), ZumasRevenge.Common._S((float)(num2 + num14) - this.mRecoilAmt), (double)this.mAngle, (float)ZumasRevenge.Common._S(fb.mCX - num13), ZumasRevenge.Common._S((float)(fb.mCY - num14) + this.mRecoilAmt), theSrcRect);
				}
				else
				{
					g.DrawImageRotated(fb.mLazerEyeLoop, ZumasRevenge.Common._S(num + num13), (int)ZumasRevenge.Common._S((float)(num2 + num14) - this.mRecoilAmt), (double)this.mAngle, ZumasRevenge.Common._S(fb.mCX - num13), (int)ZumasRevenge.Common._S((float)(fb.mCY - num14) + this.mRecoilAmt), theSrcRect);
				}
				g.PopState();
				if (flag)
				{
					g.SetColor(255, 255, 255, num9);
					g.SetColorizeImages(true);
				}
				num11 = ZumasRevenge.Common._M(40);
				num12 = ZumasRevenge.Common._M(6);
				Image imageByID = Res.GetImageByID(ResID.IMAGE_FROG_BACKFLASH);
				int num15 = imageByID.mNumCols * imageByID.mNumRows;
				int num16 = num15 - this.mLazerCount;
				if (num16 < 0)
				{
					num16 = 0;
				}
				if (num16 < num15)
				{
					theSrcRect = imageByID.GetCelRect(num16);
					if (g.Is3D())
					{
						g.DrawImageRotatedF(imageByID, (float)ZumasRevenge.Common._S(num + num11), ZumasRevenge.Common._S((float)(num2 + num12) - this.mRecoilAmt), (double)this.mAngle, (float)ZumasRevenge.Common._S(fb.mCX - num11), ZumasRevenge.Common._S((float)(fb.mCY - num12) + this.mRecoilAmt), theSrcRect);
					}
					else
					{
						g.DrawImageRotated(imageByID, ZumasRevenge.Common._S(num + num11), (int)ZumasRevenge.Common._S((float)(num2 + num12) - this.mRecoilAmt), (double)this.mAngle, ZumasRevenge.Common._S(fb.mCX - num11), (int)ZumasRevenge.Common._S((float)(fb.mCY - num12) + this.mRecoilAmt), theSrcRect);
					}
				}
				g.SetColorizeImages(false);
				Image imageByID2 = Res.GetImageByID(ResID.IMAGE_FROG_BACKPULSE);
				for (int i = 0; i < 4; i++)
				{
					if (this.mLazerFrogBackPulseAlpha[i] != 0f)
					{
						g.SetColorizeImages(true);
						g.SetColor(255, 255, 255, (int)(flag ? Math.Min(this.mLazerFrogBackPulseAlpha[i], (float)num9) : this.mLazerFrogBackPulseAlpha[i]));
						g.SetDrawMode(ZumasRevenge.Common._M(1));
						theSrcRect = imageByID2.GetCelRect(i);
						num11 = ZumasRevenge.Common._M(40) + ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_FROG_BACKPULSE));
						num12 = ZumasRevenge.Common._M(6) + ZumasRevenge.Common._DS(Res.GetOffsetXByID(ResID.IMAGE_FROG_BACKPULSE));
						if (g.Is3D())
						{
							g.DrawImageRotatedF(imageByID2, (float)ZumasRevenge.Common._S(num + num11), ZumasRevenge.Common._S((float)(num2 + num12) - this.mRecoilAmt), (double)this.mAngle, (float)ZumasRevenge.Common._S(fb.mCX - num11), ZumasRevenge.Common._S((float)(fb.mCY - num12) + this.mRecoilAmt), theSrcRect);
						}
						else
						{
							g.DrawImageRotated(imageByID2, ZumasRevenge.Common._S(num + num11), (int)ZumasRevenge.Common._S((float)(num2 + num12) - this.mRecoilAmt), (double)this.mAngle, ZumasRevenge.Common._S(fb.mCX - num11), (int)ZumasRevenge.Common._S((float)(fb.mCY - num12) + this.mRecoilAmt), theSrcRect);
						}
						g.SetColorizeImages(false);
						g.SetDrawMode(0);
					}
				}
			}
			if ((this.mCannonState > 0 || fb.mType == FrogType.FrogType_Cannon) && fb.mType != FrogType.FrogType_Lazer)
			{
				int num17 = ZumasRevenge.Common._M(0);
				int num18 = ZumasRevenge.Common._M(-8);
				g.PushState();
				if (fb.mType != FrogType.FrogType_Lazer)
				{
					if (this.mCannonRuneAlpha != 255)
					{
						g.SetColor(255, 255, 255, this.mCannonRuneAlpha);
						g.SetColorizeImages(true);
					}
					else
					{
						g.SetColorizeImages(false);
					}
					num17 = ZumasRevenge.Common._M(21);
					num18 = ZumasRevenge.Common._M(6);
					ResID resID = ResID.IMAGE_FROG_RUNE_BLUE;
					Image imageByID3 = Res.GetImageByID(resID + this.mCannonRuneColor);
					if (g.Is3D())
					{
						g.DrawImageRotatedF(imageByID3, (float)ZumasRevenge.Common._S(num + num17), ZumasRevenge.Common._S((float)(num2 + num18) - this.mRecoilAmt), (double)this.mAngle, (float)ZumasRevenge.Common._S(fb.mCX - num17), ZumasRevenge.Common._S((float)(fb.mCY - num18) + this.mRecoilAmt));
					}
					else
					{
						g.DrawImageRotated(imageByID3, ZumasRevenge.Common._S(num + num17), (int)ZumasRevenge.Common._S((float)(num2 + num18) - this.mRecoilAmt), (double)this.mAngle, ZumasRevenge.Common._S(fb.mCX - num17), (int)ZumasRevenge.Common._S((float)(fb.mCY - num18) + this.mRecoilAmt));
					}
				}
				g.PopState();
			}
			g.PopState();
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00073985 File Offset: 0x00071B85
		protected float GetBeamAngle()
		{
			return this.GetAngle() - 1.570795f;
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00073994 File Offset: 0x00071B94
		protected void UpdateBeamVec(List<BeamComponent> v, bool remove)
		{
			for (int i = 0; i < v.size<BeamComponent>(); i++)
			{
				BeamComponent beamComponent = v[i];
				beamComponent.mX += beamComponent.mVX;
				beamComponent.mY += beamComponent.mVY;
				beamComponent.mDistTraveled += beamComponent.mV0;
				if (beamComponent.mAlphaDelta != 0 && ((this.mElectricOrb.mVX == 0f && this.mElectricOrb.mVY == 0f) || beamComponent.mDistTraveled <= this.mElectricOrb.mDistTraveled))
				{
					BeamComponent beamComponent2 = beamComponent;
					beamComponent2.mColor.mAlpha = beamComponent2.mColor.mAlpha + beamComponent.mAlphaDelta;
					if (beamComponent.mColor.mAlpha > 255 && beamComponent.mAlphaDelta > 0)
					{
						beamComponent.mAlphaDelta *= -1;
						beamComponent.mColor.mAlpha = 255;
					}
					else if (beamComponent.mColor.mAlpha < beamComponent.mMinAlpha && beamComponent.mAlphaDelta < 0)
					{
						if (this.mElectricOrb.mVX == 0f && this.mElectricOrb.mVY == 0f)
						{
							beamComponent.mAlphaDelta *= -1;
						}
						beamComponent.mColor.mAlpha = beamComponent.mMinAlpha;
					}
				}
				if (remove && this.ShouldRemoveBeamComponent(beamComponent))
				{
					v.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00073B07 File Offset: 0x00071D07
		protected void UpdateBeamVec(List<BeamComponent> v)
		{
			this.UpdateBeamVec(v, true);
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x00073B14 File Offset: 0x00071D14
		protected void DrawBeamVec(Graphics g, List<BeamComponent> v, bool overlay)
		{
			for (int i = 0; i < v.size<BeamComponent>(); i++)
			{
				BeamComponent beamComponent = v[i];
				if (beamComponent.mDistTraveled < this.mBeamDistToTarget && (!overlay || beamComponent.mDistTraveled <= this.mElectricOrb.mDistTraveled))
				{
					g.PushState();
					if (beamComponent.mAdditive)
					{
						g.SetDrawMode(1);
					}
					if (beamComponent.mColor != SexyColor.White)
					{
						g.SetColor(beamComponent.mColor);
						g.SetColorizeImages(true);
					}
					SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
					sexyTransform2D.RotateRad(this.GetBeamAngle());
					g.DrawImageMatrix(beamComponent.mImage, sexyTransform2D, ZumasRevenge.Common._S(beamComponent.mX), ZumasRevenge.Common._S(beamComponent.mY));
					g.PopState();
				}
			}
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00073BE4 File Offset: 0x00071DE4
		protected void DrawBeamVec(Graphics g, List<BeamComponent> v)
		{
			this.DrawBeamVec(g, v, false);
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x00073BF0 File Offset: 0x00071DF0
		protected void EmitBeamA()
		{
			float num = 1.6f;
			float mVX = num * (float)Math.Cos((double)this.GetBeamAngle());
			float mVY = num * -(float)Math.Sin((double)this.GetBeamAngle());
			BeamComponent beamComponent = new BeamComponent();
			beamComponent.mX = (float)this.GetCenterX();
			beamComponent.mY = (float)this.GetCenterY();
			beamComponent.mDistTraveled = 0f;
			beamComponent.mV0 = num;
			beamComponent.mAdditive = true;
			beamComponent.mImage = (MemoryImage)Res.GetImageByID(ResID.IMAGE_FROG_BEAM_A);
			beamComponent.mColor = new SexyColor(SexyColor.White);
			beamComponent.mVY = mVY;
			beamComponent.mVX = mVX;
			beamComponent.mAlphaDelta = 0;
			this.mBeams[0].Insert(0, beamComponent);
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x00073CA8 File Offset: 0x00071EA8
		protected void EmitBeamB()
		{
			float num = 2.5f;
			float mVX = num * (float)Math.Cos((double)this.GetBeamAngle());
			float mVY = num * -(float)Math.Sin((double)this.GetBeamAngle());
			BeamComponent beamComponent = new BeamComponent();
			beamComponent.mV0 = num;
			beamComponent.mAdditive = true;
			beamComponent.mImage = (MemoryImage)Res.GetImageByID(ResID.IMAGE_FROG_BEAM_B);
			beamComponent.mColor = new SexyColor(255, 255, 255, 200);
			beamComponent.mVY = mVY;
			beamComponent.mVX = mVX;
			beamComponent.mAlphaDelta = 1;
			beamComponent.mMinAlpha = 200;
			beamComponent.mX = (float)this.GetCenterX();
			beamComponent.mY = (float)this.GetCenterY();
			beamComponent.mDistTraveled = 0f;
			this.mBeams[1].Insert(0, beamComponent);
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x00073D78 File Offset: 0x00071F78
		protected void EmitBeamC()
		{
			float num = 0.5f;
			float mVX = num * (float)Math.Cos((double)this.GetBeamAngle());
			float mVY = num * -(float)Math.Sin((double)this.GetBeamAngle());
			BeamComponent beamComponent = new BeamComponent();
			beamComponent.mV0 = num;
			beamComponent.mImage = (MemoryImage)Res.GetImageByID(ResID.IMAGE_FROG_BEAM_C);
			beamComponent.mColor = new SexyColor(96, 0, 150);
			beamComponent.mVY = mVY;
			beamComponent.mVX = mVX;
			beamComponent.mAlphaDelta = 0;
			beamComponent.mX = (float)this.GetCenterX();
			beamComponent.mY = (float)this.GetCenterY();
			beamComponent.mDistTraveled = 0f;
			beamComponent.mAdditive = true;
			this.mBeams[2].Insert(0, beamComponent);
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00073E34 File Offset: 0x00072034
		protected void StepBeamUpdate(int count, bool remove)
		{
			for (int i = 0; i < 3; i++)
			{
				this.UpdateBeamVec(this.mBeams[i], remove);
			}
			if (this.mElectricOrb.mVX == 0f && this.mElectricOrb.mVY == 0f)
			{
				if ((float)count % ZumasRevenge.Common._M(15f) == 0f)
				{
					this.EmitBeamA();
				}
				if ((float)count % (1f + (float)SexyFramework.Common.Rand() % ZumasRevenge.Common._M(6f)) == 0f)
				{
					this.EmitBeamB();
				}
				if ((float)count % ZumasRevenge.Common._M(75f) == 0f)
				{
					this.EmitBeamC();
				}
			}
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x00073EDB File Offset: 0x000720DB
		protected void StepBeamUpdate(int count)
		{
			this.StepBeamUpdate(count, true);
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00073EE8 File Offset: 0x000720E8
		protected bool ShouldRemoveBeamComponent(BeamComponent bc)
		{
			return (((bc.mX >= this.mBeamProjectedEndX && MathUtils._geq(bc.mVX, 0f)) || (bc.mX <= this.mBeamProjectedEndX && MathUtils._leq(bc.mVX, 0f))) && ((bc.mY >= this.mBeamProjectedEndY && MathUtils._geq(bc.mVY, 0f)) || (bc.mY <= this.mBeamProjectedEndY && MathUtils._leq(bc.mVY, 0f)))) || bc.mDistTraveled > ZumasRevenge.Common._SS(ZumasRevenge.Common._M(1400f));
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00073F90 File Offset: 0x00072190
		protected void RepositionBeamVec(List<BeamComponent> v, float delta)
		{
			if (Enumerable.Count<BeamComponent>(v) == 0)
			{
				return;
			}
			float num = (float)Math.Cos((double)this.GetBeamAngle());
			float num2 = (float)Math.Sin((double)this.GetBeamAngle());
			float mVX = v[0].mV0 * num;
			float mVY = v[0].mV0 * -num2;
			for (int i = 0; i < Enumerable.Count<BeamComponent>(v); i++)
			{
				BeamComponent beamComponent = v[i];
				beamComponent.mVX = mVX;
				beamComponent.mVY = mVY;
				beamComponent.mX = (float)this.GetCenterX() + num * beamComponent.mDistTraveled;
				beamComponent.mY = (float)this.GetCenterY() - num2 * beamComponent.mDistTraveled;
			}
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00074040 File Offset: 0x00072240
		protected void InitBeams()
		{
			for (int i = 0; i < 3; i++)
			{
				this.mBeams[i].Clear();
			}
			float p2x;
			float p2y;
			GameApp.gApp.GetBoard().GetGuideTargetCenter(out p2x, out p2y, true);
			this.mBeamDistToTarget = MathUtils.Distance((float)this.GetCenterX(), (float)this.GetCenterY(), p2x, p2y);
			this.ResetBeams();
			this.mBeamProjectedEndX = (float)this.GetCenterX() + (float)Math.Cos((double)this.GetBeamAngle()) * this.mFarthestDistance;
			this.mBeamProjectedEndY = (float)this.GetCenterY() - (float)Math.Sin((double)this.GetBeamAngle()) * this.mFarthestDistance;
			int num = 0;
			while (Enumerable.Count<BeamComponent>(this.mBeams[2]) == 0 || !this.ShouldRemoveBeamComponent(this.mBeams[2][Enumerable.Count<BeamComponent>(this.mBeams[2]) - 1]))
			{
				this.StepBeamUpdate(num++, false);
			}
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00074124 File Offset: 0x00072324
		protected void ResetBeams()
		{
			this.mDoElectricBeamShit = true;
			this.mElectricOrb.mImage = (MemoryImage)Res.GetImageByID(ResID.IMAGE_FROG_ELECTRIC_ORB);
			this.mElectricOrb.mX = (float)this.GetCenterX();
			this.mElectricOrb.mY = (float)this.GetCenterY();
			this.mElectricOrb.mDistTraveled = (this.mElectricOrb.mV0 = (this.mElectricOrb.mVX = (this.mElectricOrb.mVY = 0f)));
			this.mElectricOrb.mAdditive = true;
			this.mElectricOrb.mAlphaDelta = 0;
			this.mElectricOrb.mColor = new SexyColor(SexyColor.White);
			this.mElectricOrb.mCel = (this.mElectricOrb.mMinAlpha = 0);
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < Enumerable.Count<BeamComponent>(this.mBeams[i]); j++)
				{
					this.mBeams[i][j].mAlphaDelta = 0;
					this.mBeams[i][j].mMinAlpha = 0;
					this.mBeams[i][j].mColor.mAlpha = 255;
				}
			}
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00074264 File Offset: 0x00072464
		protected void DrawBeams(Graphics g)
		{
			this.DrawBeamVec(g, this.mBeams[2]);
			this.DrawBeamVec(g, this.mBeams[1]);
			this.DrawBeamVec(g, this.mBeams[0]);
			if (this.mElectricOrb.mVX != 0f || this.mElectricOrb.mVY != 0f)
			{
				this.DrawBeamVec(g, this.mBeams[2], true);
				this.DrawBeamVec(g, this.mBeams[1], true);
				this.DrawBeamVec(g, this.mBeams[0], true);
			}
			g.SetDrawMode(1);
			SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
			sexyTransform2D.LoadIdentity();
			sexyTransform2D.Scale(2f, 2f);
			g.DrawImageMatrix(this.mElectricOrb.mImage, sexyTransform2D, this.mElectricOrb.mImage.GetCelRect(this.mElectricOrb.mCel), ZumasRevenge.Common._S(this.mElectricOrb.mX), ZumasRevenge.Common._S(this.mElectricOrb.mY));
			g.SetDrawMode(0);
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00074374 File Offset: 0x00072574
		protected void UpdateBeams()
		{
			this.StepBeamUpdate(this.mUpdateCount);
			if (this.mUpdateCount % ZumasRevenge.Common._M(6) == 0)
			{
				this.mElectricOrb.mCel = (this.mElectricOrb.mCel + 1) % this.mElectricOrb.mImage.mNumRows;
			}
			this.mElectricOrb.mX += this.mElectricOrb.mVX;
			this.mElectricOrb.mY += this.mElectricOrb.mVY;
			this.mElectricOrb.mDistTraveled += this.mElectricOrb.mV0;
			if (this.mElectricOrb.mDistTraveled > this.mBeamDistToTarget)
			{
				this.mDoElectricBeamShit = false;
			}
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00074438 File Offset: 0x00072638
		protected void RetargetBeams(float angle_delta)
		{
			float num = -1f;
			float num2 = -1f;
			GameApp.gApp.GetBoard().GetGuideTargetCenter(out num, out num2, true);
			num = (float)((int)num);
			num2 = (float)((int)num2);
			if (num != (float)this.mLastGuideX || num2 != (float)this.mLastGuideY)
			{
				this.mLastGuideX = (int)num;
				this.mLastGuideY = (int)num2;
				this.mBeamDistToTarget = MathUtils.Distance((float)this.GetCenterX(), (float)this.GetCenterY(), num, num2);
			}
			else if (angle_delta == 0f)
			{
				return;
			}
			float num3 = (float)Math.Cos((double)this.GetBeamAngle());
			float num4 = (float)Math.Sin((double)this.GetBeamAngle());
			this.mBeamProjectedEndX = (float)this.GetCenterX() + num3 * this.mFarthestDistance;
			this.mBeamProjectedEndY = (float)this.GetCenterY() - num4 * this.mFarthestDistance;
			for (int i = 0; i < 3; i++)
			{
				this.RepositionBeamVec(this.mBeams[i], angle_delta);
			}
			this.mElectricOrb.mVX = this.mElectricOrb.mV0 * num3;
			this.mElectricOrb.mVY = this.mElectricOrb.mV0 * -num4;
			this.mElectricOrb.mX = (float)this.GetCenterX() + num3 * (this.mElectricOrb.mDistTraveled + ZumasRevenge.Common._M(20f));
			this.mElectricOrb.mY = (float)this.GetCenterY() - num4 * (this.mElectricOrb.mDistTraveled + ZumasRevenge.Common._M(20f));
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x000745A8 File Offset: 0x000727A8
		protected void DoBubbles(int num)
		{
			if ((this.mBoard.mLevel.mBoss != null && this.mBoard.mLevel.mZone != 5) || this.mBoard.DoingLevelTransition() || this.mBoard.GetGameState() != GameState.GameState_Playing)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				Bubble bubble = new Bubble();
				bubble.Init((float)ZumasRevenge.Common._M(0), MathUtils.FloatRange(ZumasRevenge.Common._M1(-1.2f), ZumasRevenge.Common._M2(-0.5f)), MathUtils.FloatRange(ZumasRevenge.Common._M3(0.05f), ZumasRevenge.Common._M4(0.2f)), (int)MathUtils.FloatRange(ZumasRevenge.Common._M5(15f), ZumasRevenge.Common._M6(25f)));
				bubble.SetAlphaFade(ZumasRevenge.Common._M(2f));
				float num2 = this.mCurX - (float)this.mCurrentBody.mCX + (float)this.mCurrentBody.mTongueX + (float)MathUtils.IntRange((int)ZumasRevenge.Common._M(0f), (int)ZumasRevenge.Common._M1(20f));
				float num3 = this.mCurY - (float)this.mCurrentBody.mCY + (float)Gun.TONGUE_Y2 + (float)ZumasRevenge.Common._M(0);
				num2 += (float)Math.Cos((double)this.mAngle - 1.5707950592041016) * (float)MathUtils.IntRange((int)ZumasRevenge.Common._M(30f), (int)ZumasRevenge.Common._M1(50f));
				num3 -= (float)Math.Sin((double)this.mAngle - 1.5707950592041016) * (float)MathUtils.IntRange((int)ZumasRevenge.Common._M(30f), (int)ZumasRevenge.Common._M1(50f));
				bubble.SetX(num2);
				bubble.SetY(num3);
				this.mBubbles.Add(bubble);
			}
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x00074760 File Offset: 0x00072960
		public Gun(Board b)
		{
			this.mBoard = b;
			for (int i = 0; i < 4; i++)
			{
				this.mLazerFrogBackPulseAlpha[i] = 0f;
			}
			this.mCannonBlast = null;
			this.mDarkFrogStunShort = true;
			this.mDarkFrogStun = null;
			this.mLightningEffect = null;
			this.mFlameStunShort = false;
			this.mFlameStun = null;
			this.mCenterX = 320f;
			this.mCenterY = 240f;
			this.mStunSpinFrame = -1;
			this.mStartingStunTime = 0;
			this.mBX = 0;
			this.mBY = 0;
			this.mDestCount = 0;
			this.mBlinkTimer = 0;
			this.mDestTime = 1;
			this.mRecoilAmt = 0f;
			this.mCannonCount = 0;
			this.mLazerPercent = 0f;
			this.mLazerCount = 0;
			this.mSpitVX = (this.mSpitVY = (this.mSpitAngle = 0f));
			this.mSlowTimer = 0;
			this.mDoElectricBeamShit = false;
			this.mSpitX = (this.mSpitY = (this.mSpitAlpha = 0f));
			this.mBlinkCount = -1;
			this.mFarthestDistance = 0f;
			this.mLastGuideX = (this.mLastGuideY = -999);
			this.mElectricOrb.mDistTraveled = (this.mElectricOrb.mV0 = (this.mElectricOrb.mVX = (this.mElectricOrb.mVY = 0f)));
			this.mStunTimer = 0;
			this.mDizzyStars = null;
			this.mDoingCannonBlast = false;
			this.mBossDeathTX = (this.mBossDeathTY = (this.mBossDeathVX = (this.mBossDeathVY = 0f)));
			this.mLazer = new DeviceImage();
			this.mLazer.mApp = GameApp.gApp;
			this.mLazer.SetImageMode(true, true);
			this.mLazer.AddImageFlags(16U);
			this.mLazer.Create(ZumasRevenge.Common._S(20), ZumasRevenge.Common._S(1000));
			Graphics graphics = new Graphics(this.mLazer);
			graphics.SetColor(0, 0, 0, 0);
			graphics.FillRect(0, 0, this.mLazer.mWidth, this.mLazer.mHeight);
			for (int j = 0; j < this.mLazer.mHeight; j += ZumasRevenge.Common._S(2))
			{
				graphics.DrawImage(Res.GetImageByID(ResID.IMAGE_FROG_LAZER), 0, j);
			}
			graphics.ClearRenderContext();
			this.mWidth = 108;
			this.mHeight = 108;
			this.mAngle = 0f;
			this.mDestAngle = 0f;
			this.mDoingHop = false;
			this.mFireVel = 8f;
			this.mState = GunState.GunState_Normal;
			this.mShieldAnimCel = 0;
			this.mBullet = null;
			this.mNextBullet = null;
			this.mShowNextBall = true;
			this.mUpdateCount = 0;
			this.mBossStateAlpha = 0f;
			this.mBossStateAlphaDir = 0;
			this.mBossStateHoldTimer = 0;
			this.mBossState = -1;
			this.mCannonRuneColor = -1;
			this.mCannonState = 0;
			this.mCannonRuneAlpha = (this.mCannonLightness = 0);
			this.mSickAnim = new Composition();
			this.mSickAnim.mLoadImageFunc = new AECommon.LoadCompImageFunc(GameApp.CompositionLoadFunc);
			this.mSickAnim.mPostLoadImageFunc = new AECommon.PostLoadCompImageFunc(GameApp.CompositionPostLoadFunc);
			GameApp.mCompositionResPrefix = "_BOSS_DARKFROG";
			this.mSickAnim.LoadFromFile("pax\\SICKO");
			this.mSickAnim.mPreLayerDrawData = this;
			this.mSickAnim.mPreLayerDrawFunc = new AECommon.PreLayerDrawFunc(Gun.PreLayerDraw);
			GameApp.mCompositionResPrefix = "";
			this.SetFrogType(FrogType.FrogType_Normal, true);
			for (int k = 0; k < Gun.NUM_CANNON_SHADOWS; k++)
			{
				this.mCannonBallShadows[k] = 0;
			}
			this.mCannonBallShadowPos = 0;
			Res.GetImageByID(ResID.IMAGE_FROG_SPIN_FRAMES);
			Res.GetSoundByID(ResID.SOUND_FROG_STUNNED);
			Res.GetSoundByID(ResID.SOUND_NEW_BURNINGFROGLOOP);
			Res.GetSoundByID(ResID.SOUND_NEW_FIREHITFROG);
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x00074C94 File Offset: 0x00072E94
		public virtual void Dispose()
		{
			this.mDizzyStars = null;
			this.mSickAnim = null;
			this.mFlameStun = null;
			this.mLazer = null;
			this.EmptyBullets();
			for (int i = 0; i < this.mBubbles.Count; i++)
			{
				this.mBubbles[i] = null;
			}
			this.mBubbles.Clear();
			for (int j = 0; j < this.mPowerRings.Count; j++)
			{
				this.mPowerRings[j] = null;
			}
			this.mPowerRings.Clear();
			for (int k = 0; k < this.mSmokeParticles.Count; k++)
			{
				this.mSmokeParticles[k] = null;
			}
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x00074D42 File Offset: 0x00072F42
		public bool NeedsReload()
		{
			return this.mNextBullet == null || this.mBullet == null;
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x00074D57 File Offset: 0x00072F57
		public void DeleteBullet()
		{
			if (this.mBullet != null)
			{
				this.mBullet = this.mNextBullet;
				this.mNextBullet = null;
			}
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00074D74 File Offset: 0x00072F74
		public void ClearBubbles()
		{
			for (int i = 0; i < this.mBubbles.Count; i++)
			{
				this.mBubbles[i] = null;
			}
			this.mBubbles.Clear();
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x00074DB0 File Offset: 0x00072FB0
		public void LevelReset()
		{
			this.mBossStateAlpha = 0f;
			this.mBossState = -1;
			this.mBossStateAlphaDir = 0;
			this.mBossStateHoldTimer = 0;
			this.mDizzyStars = null;
			this.SetCannonCount(0, false, -1);
			this.SetFrogType(FrogType.FrogType_Normal, true);
			this.DoLightningFrog(false);
			this.mDoElectricBeamShit = false;
			this.mFrogStack.Clear();
			this.mLazerCount = 0;
			this.mLazerPercent = 0f;
			this.mStunTimer = 0;
			this.mSlowTimer = 0;
			this.mState = GunState.GunState_Normal;
			this.mConfusionMarks.Clear();
			this.mShotCorrectionTarget.x = (this.mShotCorrectionTarget.y = 0f);
			this.mShotCorrectionRad = 0f;
			this.mPowerRings.Clear();
			this.ClearBubbles();
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x00074E7C File Offset: 0x0007307C
		public void SickFrogPreLayerDraw(Graphics g, Layer l)
		{
			int num = (int)this.mCurX - this.mCurrentBody.mCX + GameApp.gScreenShakeX;
			int num2 = (int)this.mCurY - this.mCurrentBody.mCY + GameApp.gScreenShakeY;
			if (this.mShowNextBall && this.mBoard.mLevel.mBoss.AllowFrogToFire() && this.mNextBullet != null && this.mState != GunState.GunState_Reloading && JeffLib.Common.StrFindNoCase(l.mLayerName, "top") != -1)
			{
				if (this.mBullet != null)
				{
					this.mBullet.Draw(g, this.mBallXOff + GameApp.gScreenShakeX - ZumasRevenge.Common._DS(ZumasRevenge.Common._M(3)), this.mBallYOff + GameApp.gScreenShakeY);
				}
				Image imageByID = Res.GetImageByID(ResID.IMAGE_NEXT_BALL);
				int theCel = this.mNextBullet.GetColorType();
				if (GameApp.gApp.mColorblind && this.mNextBullet.GetColorType() == 3)
				{
					theCel = 6;
				}
				else if (GameApp.gApp.mColorblind && this.mNextBullet.GetColorType() == 4)
				{
					theCel = 7;
				}
				Rect celRect = imageByID.GetCelRect(theCel);
				if (g.Is3D())
				{
					g.DrawImageRotatedF(imageByID, (float)ZumasRevenge.Common._S(num + this.mCurrentBody.mNextBallX), ZumasRevenge.Common._S((float)(num2 + this.mCurrentBody.mNextBallY) - this.mRecoilAmt), (double)this.mAngle, (float)ZumasRevenge.Common._S(this.mCurrentBody.mCX - this.mCurrentBody.mNextBallX), ZumasRevenge.Common._S((float)(this.mCurrentBody.mCY - this.mCurrentBody.mNextBallY) + this.mRecoilAmt), celRect);
					return;
				}
				g.DrawImageRotated(imageByID, ZumasRevenge.Common._S(num + this.mCurrentBody.mNextBallX), (int)ZumasRevenge.Common._S((float)(num2 + this.mCurrentBody.mNextBallY) - this.mRecoilAmt), (double)this.mAngle, ZumasRevenge.Common._S(this.mCurrentBody.mCX - this.mCurrentBody.mNextBallX), (int)ZumasRevenge.Common._S((float)(this.mCurrentBody.mCY - this.mCurrentBody.mNextBallY) + this.mRecoilAmt), celRect);
			}
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x000750A6 File Offset: 0x000732A6
		public void DoInkedState()
		{
			this.mBossStateAlphaDir = 1;
			this.mBossStateHoldTimer = ZumasRevenge.Common._M(500);
			this.mBossState = 0;
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x000750C6 File Offset: 0x000732C6
		public void DoPlaguedState()
		{
			this.mBossStateAlphaDir = 1;
			this.mBossStateHoldTimer = ZumasRevenge.Common._M(300);
			this.mBossState = 1;
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x000750E8 File Offset: 0x000732E8
		public void DrawConfusionMarks(Graphics g)
		{
			if (this.mBoard.GetGameState() == GameState.GameState_Losing)
			{
				return;
			}
			g.SetColorizeImages(true);
			for (int i = 0; i < this.mConfusionMarks.size<ConfusionMark>(); i++)
			{
				ConfusionMark confusionMark = this.mConfusionMarks[i];
				g.SetColor(255, 255, 255, (int)confusionMark.mAlpha);
				g.DrawImage(confusionMark.mImage, (int)ZumasRevenge.Common._S(confusionMark.mX + (float)this.GetCenterX()), (int)ZumasRevenge.Common._S(confusionMark.mY + (float)this.GetCenterY()), (int)(confusionMark.mSize * (float)confusionMark.mImage.mWidth), (int)(confusionMark.mSize * (float)confusionMark.mImage.mHeight));
			}
			g.SetColorizeImages(false);
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x000751B4 File Offset: 0x000733B4
		public void MoveToBossDeathPosition(float x, float y)
		{
			this.mBossDeathTX = x;
			this.mBossDeathTY = y;
			this.mBossDeathVX = (x - this.mCurX) / ZumasRevenge.Common._M(200f);
			this.mBossDeathVY = (y - this.mCurY) / ZumasRevenge.Common._M(200f);
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00075204 File Offset: 0x00073404
		public void Reload(int theType, bool delay, PowerType thePower)
		{
			Bullet bullet = new Bullet();
			bullet.mFrog = this;
			bullet.SetColorType(theType);
			bullet.SetPowerType(thePower, false);
			this.mStatePercent = 0f;
			this.mBullet = null;
			this.mBullet = this.mNextBullet;
			if (this.mCannonCount > 0 && this.mBullet != null && !this.mBullet.GetIsCannon())
			{
				this.mBullet.SetIsCannon(true);
				this.mCannonCount--;
			}
			this.mNextBullet = bullet;
			this.mState = GunState.GunState_Reloading;
			if (!delay)
			{
				this.mStatePercent = 1f;
				this.mState = GunState.GunState_Normal;
			}
			this.CalcAngle();
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x000752AC File Offset: 0x000734AC
		public void Reload(int theType)
		{
			this.Reload(theType, false, PowerType.PowerType_Max);
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x000752B8 File Offset: 0x000734B8
		public void Reload2(int theType, bool delay, PowerType thePower, int bx, int by)
		{
			Bullet bullet = new Bullet();
			bullet.mFrog = this;
			bullet.SetColorType(theType);
			bullet.SetPowerType(thePower, false);
			if (thePower == PowerType.PowerType_Cannon)
			{
				bullet.SetIsCannon(true);
			}
			this.mBX = bx;
			this.mBY = by;
			this.mStatePercent = 0f;
			this.mBullet = null;
			this.mBullet = bullet;
			this.mState = GunState.GunState_Reloading;
			if (!delay)
			{
				this.mStatePercent = 1f;
				this.mState = GunState.GunState_Normal;
			}
			this.CalcAngle();
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00075337 File Offset: 0x00073537
		public void Reload2(int theType)
		{
			this.Reload2(theType, false, PowerType.PowerType_Max, 0, 0);
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x00075348 File Offset: 0x00073548
		public void Reload3()
		{
			if (this.mBullet == null && this.mNextBullet != null)
			{
				this.mBX = 0;
				this.mBY = 0;
				this.mBullet = this.mNextBullet;
				this.mNextBullet = null;
				this.mStatePercent = 0f;
				this.mState = GunState.GunState_Reloading;
				this.CalcAngle();
			}
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x000753A0 File Offset: 0x000735A0
		public void ClearLaserState()
		{
			if (this.mLazerCount <= 0)
			{
				return;
			}
			this.mLazerCount = 0;
			if (Enumerable.Count<FrogBody>(this.mFrogStack) > 0 && this.mFrogStack[Enumerable.Count<FrogBody>(this.mFrogStack) - 1].mType == FrogType.FrogType_Lazer)
			{
				this.mFrogStack.Clear();
			}
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x000753F8 File Offset: 0x000735F8
		public void SetAngle(float theAngle)
		{
			this.mDestAngle = theAngle;
			this.mAngle = theAngle;
			this.CalcAngle();
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0007541C File Offset: 0x0007361C
		public void SetDestAngle(float theAngle)
		{
			while (this.mAngle < 0f)
			{
				this.mAngle += 6.28318f;
			}
			while (this.mAngle > 6.28318f)
			{
				this.mAngle -= 6.28318f;
			}
			float num = Math.Abs(theAngle - this.mAngle);
			if (num > 3.14159f)
			{
				if (theAngle < this.mAngle)
				{
					theAngle += 6.28318f;
				}
				else
				{
					theAngle -= 6.28318f;
				}
			}
			this.mDestAngle = theAngle;
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x000754A8 File Offset: 0x000736A8
		public void SetDestPos(int x, int y, int theSpeed, bool doingHop)
		{
			this.mDoingHop = doingHop;
			this.mDestX1 = (int)this.mCenterX;
			this.mDestY1 = (int)this.mCenterY;
			this.mDestX2 = x;
			this.mDestY2 = y;
			SexyVector2 sexyVector = new SexyVector2((float)(this.mDestX2 - this.mDestX1), (float)(this.mDestY2 - this.mDestY1));
			float num = sexyVector.Magnitude();
			this.mDestCount = (int)(num / (float)theSpeed);
			if (this.mDestCount < 1)
			{
				this.mDestCount = 1;
			}
			this.mDestTime = this.mDestCount;
			if (this.mBoard.mLevel.mZone == 5 && this.mBoard.mLevel.mNum != 10 && this.mDoingHop && this.mBoard.mLevel.mMoveType == 0)
			{
				this.DoBubbles((int)ZumasRevenge.Common._M(5f));
			}
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x00075589 File Offset: 0x00073789
		public void SetDestPos(int x, int y, int theSpeed)
		{
			this.SetDestPos(x, y, theSpeed, false);
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x00075598 File Offset: 0x00073798
		public void Draw(Graphics g, int clip_height)
		{
			if (!this.mBoard.CanDrawFrog())
			{
				return;
			}
			for (int i = 0; i < this.mSmokeParticles.size<LTSmokeParticle>(); i++)
			{
				BambooTransition.DrawSmokeParticle(g, this.mSmokeParticles[i]);
			}
			int num = (int)this.mCurX - this.mCurrentBody.mCX + GameApp.gScreenShakeX;
			int num2 = (int)this.mCurY - this.mCurrentBody.mCY + GameApp.gScreenShakeY;
			if (GameApp.gApp.GetBoard() != null)
			{
				GameApp.gApp.GetBoard().DrawFatFingerGuide(g);
			}
			if (this.mBullet != null && this.GetType() == 1 && !this.mBoard.IsPaused() && !this.mBoard.LevelIsSkeletonBoss())
			{
				g.SetColor(ZumasRevenge.Common._M(50), ZumasRevenge.Common._M1(50), ZumasRevenge.Common._M2(50), 100);
				float num3 = this.mDestAngle - 1.570795f;
				float num4 = num3 - this.mCannonAngle;
				float num5 = num3 + this.mCannonAngle;
				float num6 = (float)Math.Cos((double)num4);
				float num7 = -(float)Math.Sin((double)num4);
				float num8 = (float)Math.Cos((double)num5);
				float num9 = -(float)Math.Sin((double)num5);
				int num10 = (int)ZumasRevenge.Common._S(this.mBullet.GetX() + (float)this.mBallXOff);
				int num11 = (int)ZumasRevenge.Common._S(this.mBullet.GetY() + (float)this.mBallYOff);
				if (GameApp.mGameRes != 768)
				{
					num10 += (int)((float)this.mCurrentBody.mBody.mHeight * 0.5f);
				}
				if (g.Get3D() == null)
				{
					SexyPoint point = new SexyPoint((int)((float)ZumasRevenge.Common._S(ZumasRevenge.Common._M(1000)) * num6 + (float)num10), (int)((float)ZumasRevenge.Common._S(ZumasRevenge.Common._M1(1000)) * num7 + (float)num11));
					SexyPoint point2 = new SexyPoint((int)((float)ZumasRevenge.Common._S(ZumasRevenge.Common._M(1000)) * num8 + (float)num10), (int)((float)ZumasRevenge.Common._S(ZumasRevenge.Common._M1(1000)) * num9 + (float)num11));
					SexyPoint[] theVertexList = new SexyPoint[]
					{
						new SexyPoint(num10, num11),
						point,
						point2
					};
					if (this.mCannonAngle != 0f)
					{
						g.PolyFill(theVertexList, 3, false);
					}
				}
				else
				{
					this.DrawCannonPaths(g, num3, num10 + this.mBoard.mApp.mBoardOffsetX / 2 - 10, num11);
				}
				if (!g.Is3D())
				{
					g.SetColorizeImages(true);
					this.mVels[0].mX = num6;
					this.mVels[0].mY = num7;
					this.mVels[1].mX = (float)Math.Cos((double)num3);
					this.mVels[1].mY = -(float)Math.Sin((double)num3);
					this.mVels[2].mX = num8;
					this.mVels[2].mY = num9;
					Image imageByID = Res.GetImageByID(ResID.IMAGE_CANNON_BALL);
					for (int j = 0; j < Gun.NUM_CANNON_SHADOWS; j++)
					{
						if (this.mCannonBallShadows[j] > 0)
						{
							g.SetColor(ZumasRevenge.Common._M(128), ZumasRevenge.Common._M1(128), ZumasRevenge.Common._M2(128), this.mCannonBallShadows[j]);
							for (int k = 0; k < 3; k++)
							{
								g.DrawImage(imageByID, (int)((float)(num10 - imageByID.mWidth / 2) + this.mVels[k].mX * (float)(j + 1) * (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M(80))), (int)((float)(num11 - imageByID.mHeight / 2) + this.mVels[k].mY * (float)(j + 1) * (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M(80))));
							}
						}
					}
					g.SetColorizeImages(false);
				}
			}
			if ((this.mStunTimer > 0 && this.mBlinkCount < 0) || this.mFlameStun != null || this.mDarkFrogStun != null)
			{
				if (this.mFlameStun != null)
				{
					this.mFlameStun.Draw(g);
				}
				else if (this.mDarkFrogStun != null)
				{
					this.mDarkFrogStun.Draw(g);
				}
				else
				{
					Image imageByID2 = Res.GetImageByID(ResID.IMAGE_FROG_SPIN_FRAMES);
					Rect celRect = imageByID2.GetCelRect(this.mStunSpinFrame);
					g.DrawImageRotated(imageByID2, (int)(ZumasRevenge.Common._S(this.mCenterX) - (float)(celRect.mWidth / 2) + (float)ZumasRevenge.Common._M(0)), (int)(ZumasRevenge.Common._S(this.mCenterY) - (float)(celRect.mHeight / 2)), (double)this.mAngle, celRect);
				}
			}
			else if (!this.IsPoisoned())
			{
				g.PushState();
				if (clip_height != 0)
				{
					g.ClipRect(0, 0, GameApp.gApp.mWidth, (int)ZumasRevenge.Common._S(this.mCenterY + (float)this.mHeight + (float)clip_height));
				}
				this.DrawFrogBase(g, this.mCurrentBody);
				for (int l = 0; l < this.mFrogStack.size<FrogBody>(); l++)
				{
					if (this.mFrogStack[l].mAlpha > 0)
					{
						this.DrawFrogBase(g, this.mFrogStack[l]);
					}
				}
				this.DrawFrogTongue(g, this.mCurrentBody);
				for (int m = 0; m < this.mFrogStack.size<FrogBody>(); m++)
				{
					if (this.mFrogStack[m].mAlpha > 0)
					{
						this.DrawFrogTongue(g, this.mFrogStack[m]);
					}
				}
				g.PopState();
			}
			else
			{
				Rect celRect2 = this.mCurrentBody.mShadow.GetCelRect(0);
				ZumasRevenge.Common._S(this.mCenterX - this.mCurX);
				if (g.Is3D())
				{
					g.DrawImageRotatedF(this.mCurrentBody.mShadow, (float)ZumasRevenge.Common._S(num + ZumasRevenge.Common._M(-2)), ZumasRevenge.Common._S((float)num2 - this.mRecoilAmt / 2f + (float)ZumasRevenge.Common._M1(3)), (double)this.mAngle, (float)ZumasRevenge.Common._S(this.mCurrentBody.mCX), ZumasRevenge.Common._S((float)this.mCurrentBody.mCY + this.mRecoilAmt / 2f), celRect2);
				}
				else
				{
					g.DrawImageRotated(this.mCurrentBody.mShadow, ZumasRevenge.Common._S(num + ZumasRevenge.Common._M(-2)), (int)ZumasRevenge.Common._S((float)num2 - this.mRecoilAmt / 2f + (float)ZumasRevenge.Common._M1(3)), (double)this.mAngle, ZumasRevenge.Common._S(this.mCurrentBody.mCX), (int)ZumasRevenge.Common._S((float)this.mCurrentBody.mCY + this.mRecoilAmt / 2f), celRect2);
				}
				float tx = ZumasRevenge.Common._S(this.mCurX) + (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(0));
				float ty = ZumasRevenge.Common._S(this.mCurY) + (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(8));
				this.mCumTran.Reset();
				this.mCumTran.mTrans.Translate((float)(-(float)this.mSickAnim.mWidth) * ZumasRevenge.Common._DS(1f), (float)(-(float)this.mSickAnim.mHeight) * ZumasRevenge.Common._DS(1f));
				this.mCumTran.mTrans.RotateRad(this.mAngle);
				this.mCumTran.mTrans.Translate(tx, ty);
				this.mSickAnim.Draw(g, this.mCumTran, -1, ZumasRevenge.Common._DS(1f));
			}
			if (this.mSpitAlpha > 0f && this.mFlameStun == null && this.mDarkFrogStun == null)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)this.mSpitAlpha);
				this.mGlobalTranform.Reset();
				this.mGlobalTranform.RotateRad(this.mSpitAngle - 3.1415927f * ZumasRevenge.Common._M(0.5f));
				g.DrawImageTransform(Res.GetImageByID(ResID.IMAGE_FROG_SLOBBER), this.mGlobalTranform, ZumasRevenge.Common._S(this.mSpitX), ZumasRevenge.Common._S(this.mSpitY));
				g.SetColorizeImages(false);
			}
			if (this.mDoElectricBeamShit && this.mBoard.mChallengeHelp == null && GameApp.gApp.mGenericHelp == null && !this.mBoard.IsPaused())
			{
				this.DrawBeams(g);
			}
			g.PushState();
			if (clip_height != 0)
			{
				g.ClipRect(0, 0, GameApp.gApp.mWidth, (int)ZumasRevenge.Common._S(this.mCenterY + (float)this.mHeight + (float)clip_height));
			}
			if ((this.mStunTimer <= 0 || (this.mBlinkCount >= 0 && this.mFlameStun == null && this.mDarkFrogStun == null)) && !this.IsPoisoned())
			{
				if (this.mBullet != null && !this.LaserMode() && !this.LightningMode() && (this.mBoard.mLevel.mBoss == null || this.mBoard.mLevel.mBoss.AllowFrogToFire()))
				{
					this.mBullet.Draw(g, this.mBallXOff + GameApp.gScreenShakeX, this.mBallYOff + GameApp.gScreenShakeY);
				}
				g.DrawImageRotated(Res.GetImageByID(ResID.IMAGE_FROG_BALLBACK), ZumasRevenge.Common._S(num + this.mCurrentBody.mNextBallX), (int)ZumasRevenge.Common._S((float)(num2 + this.mCurrentBody.mNextBallY) - this.mRecoilAmt), (double)this.mAngle, ZumasRevenge.Common._S(this.mCurrentBody.mCX - this.mCurrentBody.mNextBallX), (int)ZumasRevenge.Common._S((float)(this.mCurrentBody.mCY - this.mCurrentBody.mNextBallY) + this.mRecoilAmt));
				if (this.mShowNextBall && (this.mBoard.mLevel.mBoss == null || this.mBoard.mLevel.mBoss.AllowFrogToFire()) && !this.LaserMode() && !this.LightningMode() && this.mNextBullet != null && (this.mState != GunState.GunState_Reloading || ZumasRevenge.Common.gSuckMode))
				{
					Image imageByID3 = Res.GetImageByID(ResID.IMAGE_NEXT_BALL);
					int theCel = this.mNextBullet.GetColorType();
					if (GameApp.gApp.mColorblind && this.mNextBullet.GetColorType() == 3)
					{
						theCel = 6;
					}
					else if (GameApp.gApp.mColorblind && this.mNextBullet.GetColorType() == 4)
					{
						theCel = 7;
					}
					Rect celRect3 = imageByID3.GetCelRect(theCel);
					if (g.Is3D())
					{
						g.DrawImageRotatedF(imageByID3, (float)ZumasRevenge.Common._S(num + this.mCurrentBody.mNextBallX), ZumasRevenge.Common._S((float)(num2 + this.mCurrentBody.mNextBallY) - this.mRecoilAmt), (double)this.mAngle, (float)ZumasRevenge.Common._S(this.mCurrentBody.mCX - this.mCurrentBody.mNextBallX), ZumasRevenge.Common._S((float)(this.mCurrentBody.mCY - this.mCurrentBody.mNextBallY) + this.mRecoilAmt), celRect3);
					}
					else
					{
						g.DrawImageRotated(imageByID3, ZumasRevenge.Common._S(num + this.mCurrentBody.mNextBallX), (int)ZumasRevenge.Common._S((float)(num2 + this.mCurrentBody.mNextBallY) - this.mRecoilAmt), (double)this.mAngle, ZumasRevenge.Common._S(this.mCurrentBody.mCX - this.mCurrentBody.mNextBallX), (int)ZumasRevenge.Common._S((float)(this.mCurrentBody.mCY - this.mCurrentBody.mNextBallY) + this.mRecoilAmt), celRect3);
					}
				}
				this.DrawFrogTop(g, this.mCurrentBody);
				for (int n = 0; n < this.mFrogStack.size<FrogBody>(); n++)
				{
					if (this.mFrogStack[n].mAlpha > 0)
					{
						this.DrawFrogTop(g, this.mFrogStack[n]);
					}
				}
			}
			g.PopState();
			if (this.LaserMode() && !this.mBoard.IsPaused())
			{
				PIEffect[] array = new PIEffect[]
				{
					this.mBoard.mLazerBeam[0],
					this.mBoard.mLazerBeam[1]
				};
				float value;
				float value2;
				GameApp.gApp.GetBoard().GetGuideTargetCenter(out value, out value2, true);
				float pAngle = this.GetAngle() - 3.14159f;
				this.mGp.mX = (int)ZumasRevenge.Common._S(value);
				this.mGp.mY = (int)ZumasRevenge.Common._S(value2);
				float num12 = (float)(ZumasRevenge.Common._S(this.GetCenterX()) - ZumasRevenge.Common._S(ZumasRevenge.Common._M(24)));
				float num13 = (float)(ZumasRevenge.Common._S(this.GetCenterY()) - ZumasRevenge.Common._S(ZumasRevenge.Common._M(0)));
				JeffLib.Common.RotatePoint(pAngle, ref num12, ref num13, (float)ZumasRevenge.Common._S(this.GetCenterX()), (float)ZumasRevenge.Common._S(this.GetCenterY()));
				this.mCP.mX = (int)num12;
				this.mCP.mY = (int)num13;
				float num14 = MathUtils.Distance(this.mCP, this.mGp, true);
				float num15 = num14;
				float num16 = num14 * this.mLazerPercent;
				Rect theSrcRect = new Rect(0, 0, ZumasRevenge.Common._S(20), (int)num16);
				if (num16 > (float)this.mLazer.mHeight)
				{
					num16 = (float)this.mLazer.mHeight;
				}
				theSrcRect.mHeight = (int)num16;
				float[] array2 = new float[2];
				array2[0] = CommonMath.AngleBetweenPoints(this.mCP, this.mGp);
				g.SetDrawMode(1);
				if (g.Is3D())
				{
					g.DrawImageRotatedF(this.mLazer, (float)ZumasRevenge.Common._S(this.GetCenterX() - ZumasRevenge.Common._M(32)), (float)ZumasRevenge.Common._S(this.GetCenterY()) - num16, (double)(array2[0] - 1.570795f), (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M1(32)), num16, theSrcRect);
				}
				else
				{
					g.DrawImageRotated(this.mLazer, ZumasRevenge.Common._S(this.GetCenterX() - ZumasRevenge.Common._M(32)), (int)((float)ZumasRevenge.Common._S(this.GetCenterY()) - num16), (double)(array2[0] - 1.570795f), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(32)), (int)num16, theSrcRect);
				}
				PIEmitterInstance emitter = array[0].GetLayer(0).GetEmitter(0);
				emitter.mEmitterInstanceDef.mPoints[1].mValuePoint2DVector[0].mValue = new Vector2(0f, -num15);
				num12 = (float)ZumasRevenge.Common._S(this.GetCenterX() - ZumasRevenge.Common._M(-26));
				num13 = (float)ZumasRevenge.Common._S(this.GetCenterY() - ZumasRevenge.Common._M(0));
				JeffLib.Common.RotatePoint(pAngle, ref num12, ref num13, (float)ZumasRevenge.Common._S(this.GetCenterX()), (float)ZumasRevenge.Common._S(this.GetCenterY()));
				this.mCP.mX = (int)num12;
				this.mCP.mY = (int)num13;
				array2[1] = CommonMath.AngleBetweenPoints(this.mCP, this.mGp);
				if (g.Is3D())
				{
					g.DrawImageRotatedF(this.mLazer, (float)ZumasRevenge.Common._S(this.GetCenterX() - ZumasRevenge.Common._M(-15)), (float)ZumasRevenge.Common._S(this.GetCenterY()) - num16, (double)(array2[1] - 1.570795f), (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M1(-15)), num16, theSrcRect);
				}
				else
				{
					g.DrawImageRotated(this.mLazer, ZumasRevenge.Common._S(this.GetCenterX() - ZumasRevenge.Common._M(-15)), (int)((float)ZumasRevenge.Common._S(this.GetCenterY()) - num16), (double)(array2[1] - 1.570795f), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(-15)), (int)num16, theSrcRect);
				}
				g.SetDrawMode(0);
				emitter = array[1].GetLayer(0).GetEmitter(0);
				emitter.mEmitterInstanceDef.mPoints[1].mValuePoint2DVector[0].mValue = new Vector2(0f, -num15);
				g.SetColor(SexyColor.White);
				g.FillRect(this.mGp.mX, this.mGp.mY, 4, 4);
				for (int num17 = 0; num17 <= 1; num17++)
				{
					emitter = array[num17].GetLayer(0).GetEmitter(0);
					for (PIParticleInstance piparticleInstance = emitter.mParticleGroup.mHead; piparticleInstance != null; piparticleInstance = piparticleInstance.mNext)
					{
						if (MathUtils.Distance(new SexyPoint(0, 0), new SexyPoint((int)piparticleInstance.mEmittedPos.X, (int)piparticleInstance.mEmittedPos.Y), true) > num15)
						{
							piparticleInstance.mLife = 0f;
						}
					}
					array[num17].mDrawTransform.LoadIdentity();
					float num18 = GameApp.DownScaleNum(1f);
					array[num17].mDrawTransform.Scale(num18, num18);
					array[num17].mDrawTransform.Translate((float)ZumasRevenge.Common._S((num17 == 0) ? (-22) : 24), 0f);
					array[num17].mDrawTransform.RotateRad(array2[num17] - 1.5705f);
					array[num17].mDrawTransform.Translate((float)ZumasRevenge.Common._S(this.GetCenterX()), (float)ZumasRevenge.Common._S(this.GetCenterY()));
					array[num17].Draw(g);
				}
				this.mBoard.mLazerBurn.mDrawTransform.LoadIdentity();
				this.mBoard.mLazerBurn.mDrawTransform.Scale(ZumasRevenge.Common._DS(1.4f), ZumasRevenge.Common._DS(1.4f));
				this.mBoard.mLazerBurn.mDrawTransform.Translate((float)this.mGp.mX, (float)this.mGp.mY);
				this.mBoard.mLazerBurn.Draw(g);
			}
			else if (this.LightningMode() && !this.mBoard.IsPaused())
			{
				float num19;
				float num20;
				GameApp.gApp.GetBoard().GetGuideTargetCenter(out num19, out num20, true);
				g.SetColor(0, 0, 255);
				float pAngle2 = this.GetAngle() - 3.14159f;
				float num21 = (float)ZumasRevenge.Common._S(this.GetCenterX() + ZumasRevenge.Common._M(0));
				float num22 = (float)ZumasRevenge.Common._S(this.GetCenterY() + ZumasRevenge.Common._M(-30));
				JeffLib.Common.RotatePoint(pAngle2, ref num21, ref num22, (float)this.GetCenterX(), (float)this.GetCenterY());
			}
			if (this.IsFuckedUp() && !this.mBoard.IsPaused())
			{
				this.DrawConfusionMarks(g);
			}
			if (!this.mBoard.IsPaused() && this.mBoard.GetGameState() != GameState.GameState_Losing)
			{
				Font fontByID = Res.GetFontByID(ResID.FONT_MAIN22);
				if (this.mBoard.mLevel.mInvertMouseTimer > 0)
				{
					g.SetFont(fontByID);
					g.SetColor(SexyColor.White);
					g.DrawString(TextManager.getInstance().getString(471), ZumasRevenge.Common._S(this.GetCenterX() - ZumasRevenge.Common._M(30)), ZumasRevenge.Common._S(this.GetCenterY() - ZumasRevenge.Common._M1(80)));
				}
				if (this.mSlowTimer > 0)
				{
					g.SetFont(fontByID);
					g.SetColor(SexyColor.White);
					g.DrawString(TextManager.getInstance().getString(472), ZumasRevenge.Common._S(this.GetCenterX() - ZumasRevenge.Common._M(47)), ZumasRevenge.Common._S(this.GetCenterY() - ZumasRevenge.Common._M1(80)));
				}
				if (this.mBoard.GetHallucinateTimer() > 0)
				{
					this.mTempText.Draw(g);
				}
			}
			if (this.mBoard.GetGameState() != GameState.GameState_Losing)
			{
				for (int num23 = 0; num23 < this.mBubbles.size<Bubble>(); num23++)
				{
					this.mBubbles[num23].Draw(g);
				}
			}
			for (int num24 = 0; num24 < this.mPowerOrbs.size<SkeletonPowerOrb>(); num24++)
			{
				SkeletonPowerOrb skeletonPowerOrb = this.mPowerOrbs[num24];
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)skeletonPowerOrb.mAlpha);
				Image imageByID4 = Res.GetImageByID(ResID.IMAGE_BOSS_SKELETON_GLOWBALL);
				int num25 = ZumasRevenge.Common._M(-25);
				int num26 = ZumasRevenge.Common._M(-20);
				float num27 = (float)imageByID4.GetCelWidth() * skeletonPowerOrb.mSize;
				float num28 = (float)imageByID4.GetCelHeight() * skeletonPowerOrb.mSize;
				g.DrawImage(imageByID4, (int)(ZumasRevenge.Common._S(this.mCurX + (float)num25) + (float)(imageByID4.GetCelWidth() / 2) - num27 / 2f), (int)(ZumasRevenge.Common._S(this.mCurY + (float)num26) + (float)(imageByID4.GetCelHeight() / 2) - num28 / 2f), (int)num27, (int)num28);
				g.SetDrawMode(1);
				g.DrawImage(imageByID4, (int)(ZumasRevenge.Common._S(this.mCurX + (float)num25) + (float)(imageByID4.GetCelWidth() / 2) - num27 / 2f), (int)(ZumasRevenge.Common._S(this.mCurY + (float)num26) + (float)(imageByID4.GetCelHeight() / 2) - num28 / 2f), (int)num27, (int)num28);
				g.SetColorizeImages(false);
				g.SetDrawMode(0);
			}
			for (int num29 = 0; num29 < this.mPowerRings.size<OrbPowerRing>(); num29++)
			{
				this.mPowerRings[num29].Draw(g, (float)((int)ZumasRevenge.Common._S(this.mCurX)), (float)((int)ZumasRevenge.Common._S(this.mCurY)));
			}
			if (this.mDizzyStars != null && this.mBoard.GetGameState() != GameState.GameState_Losing)
			{
				this.mDizzyStars.Draw(g);
			}
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x00076A7B File Offset: 0x00074C7B
		public void Draw(Graphics g)
		{
			this.Draw(g, 0);
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x00076A88 File Offset: 0x00074C88
		public void DrawCannonPaths(Graphics g, float inAngle, int inX, int inY)
		{
			FPoint pathOrigin = this.GetPathOrigin(inAngle, inX, inY);
			float num = ZumasRevenge.Common._M(0.21f);
			Graphics3D graphics3D = g.Get3D();
			g.SetColorizeImages(true);
			graphics3D.SetTexture(0, Res.GetImageByID(ResID.IMAGE_FROG_CANNON_CHEVRONS));
			graphics3D.SetTextureWrap(0, true);
			this.DrawBulletPath(graphics3D, inAngle - num, pathOrigin.mX, pathOrigin.mY);
			this.DrawBulletPath(graphics3D, inAngle, pathOrigin.mX, pathOrigin.mY);
			this.DrawBulletPath(graphics3D, inAngle + num, pathOrigin.mX, pathOrigin.mY);
			graphics3D.SetTextureWrap(0, false);
			g.SetColorizeImages(false);
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x00076B24 File Offset: 0x00074D24
		public FPoint GetPathOrigin(float inAngle, int inX, int inY)
		{
			float num = (float)Math.Pow((double)(this.mCurrentBody.mBody.mWidth / 2), 2.0);
			float num2 = (float)Math.Pow((double)(this.mCurrentBody.mBody.mHeight / 2), 2.0);
			float num3 = (float)Math.Sqrt((double)(num + num2));
			return new FPoint((float)inX - num3 * (float)Math.Cos((double)inAngle), (float)inY + num3 * (float)Math.Sin((double)inAngle));
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00076BA4 File Offset: 0x00074DA4
		public void DrawBulletPath(Graphics3D g3D, float inAngle, float inX, float inY)
		{
			float num = ZumasRevenge.Common._M(0.035f);
			float num2 = ZumasRevenge.Common._M(6f);
			float num3 = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(1300));
			float num4 = ZumasRevenge.Common._M(0.95f);
			Gun.aChevronSpeed += ZumasRevenge.Common._M(0.006f);
			SexyVertex2D sexyVertex2D = new SexyVertex2D(inX, inY, 0f, num2 + Gun.aChevronSpeed);
			SexyVertex2D sexyVertex2D2 = new SexyVertex2D(inX + num3 * (float)Math.Cos((double)inAngle), inY - num3 * (float)Math.Sin((double)inAngle), 1f, Gun.aChevronSpeed);
			SexyVertex2D sexyVertex2D3 = new SexyVertex2D(inX + num3 * num4 * (float)Math.Cos((double)(inAngle + num)), inY - num3 * num4 * (float)Math.Sin((double)(inAngle + num)), 0f, Gun.aChevronSpeed);
			SexyVertex2D sexyVertex2D4 = new SexyVertex2D(inX + num3 * num4 * (float)Math.Cos((double)(inAngle - num)), inY - num3 * num4 * (float)Math.Sin((double)(inAngle - num)), 0f, Gun.aChevronSpeed);
			SexyVertex2D[] theVertices = new SexyVertex2D[] { sexyVertex2D, sexyVertex2D2, sexyVertex2D3, sexyVertex2D, sexyVertex2D4, sexyVertex2D2 };
			g3D.DrawPrimitiveEx((uint)SexyVertex2D.FVF, Graphics3D.EPrimitiveType.PT_TriangleList, theVertices, 2, new SexyColor(255, 255, 255, ZumasRevenge.Common._M(160)), 0, 0f, 0f, true, 0U);
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00076D3C File Offset: 0x00074F3C
		public bool StartFire()
		{
			if (this.mState != GunState.GunState_Normal)
			{
				return false;
			}
			if (this.mBullet == null)
			{
				return false;
			}
			if (!this.LaserMode() && !this.LightningMode())
			{
				this.mStatePercent = 0f;
				this.mState = GunState.GunState_Firing;
				Bullet bullet = this.mBullet;
				bullet.SetJustFired(true);
				if (this.mCannonCount > 0 && !bullet.GetIsCannon())
				{
					bullet.SetIsCannon(true);
					this.mCannonCount--;
				}
				if (bullet.GetIsCannon())
				{
					GameApp.gApp.PlaySample(this.mBoard.LevelIsSkeletonBoss() ? Res.GetSoundByID(ResID.SOUND_ENERGYWEAPONFIRE) : Res.GetSoundByID(ResID.SOUND_CANNON_FIRE));
					this.mDoingCannonBlast = true;
				}
				float num = ZumasRevenge.Common._M(28f);
				float num2 = (this.IsPoisoned() ? this.mAngle : this.mDestAngle) - 1.570795f;
				if (this.mShotCorrectionTarget.x != 0f || this.mShotCorrectionTarget.y != 0f)
				{
					num2 = this.mShotCorrectionRad;
				}
				float num3 = (float)Math.Cos((double)num2);
				float num4 = -(float)Math.Sin((double)num2);
				float num5 = this.GetFireSpeed();
				if (this.mBoard.mLevel.mZone == 3 && this.mBoard.mLevel.mBoss != null && bullet.GetIsCannon())
				{
					num5 = num;
				}
				bullet.SetVelocity(num5 * num3, num5 * num4);
				bullet.mAngleFired = num2;
				if (this.mBoard.mLevel.mZone == 5 && this.mBoard.mLevel.mNum != 10)
				{
					this.DoBubbles(bullet.GetIsCannon() ? 15 : 5);
				}
				if (bullet.GetIsCannon() && this.mCannonAngle != 0f)
				{
					for (int i = 0; i < 2; i++)
					{
						float num6 = (float)((i == 0) ? (-1) : 1) * this.mCannonAngle;
						num3 = (float)Math.Cos((double)(num2 + num6));
						num4 = -(float)Math.Sin((double)(num2 + num6));
						Bullet bullet2 = new Bullet(bullet);
						bullet2.mFrog = this;
						bullet2.mAngleFired = num2 + num6;
						bullet2.SetVelocity(num5 * num3, num5 * num4);
						this.mCannonBullets.Add(bullet2);
					}
				}
				if (bullet.GetIsCannon() && !this.CannonMode())
				{
					this.SetFrogType(FrogType.FrogType_Normal, false);
					this.mBoard.CannonDisabled();
				}
				this.CalcAngle();
			}
			return true;
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x00076F9C File Offset: 0x0007519C
		private void SyncListComponents(DataSync sync, List<Component> theList, bool clear)
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
					Component component = new Component();
					component.SyncState(sync);
					theList.Add(component);
					num2++;
				}
				return;
			}
			sync.GetBuffer().WriteLong((long)theList.Count);
			foreach (Component component2 in theList)
			{
				component2.SyncState(sync);
			}
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x0007703C File Offset: 0x0007523C
		private void SyncListOrbPowerRings(DataSync sync, List<OrbPowerRing> theList, bool clear)
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
					OrbPowerRing orbPowerRing = new OrbPowerRing();
					orbPowerRing.SyncState(sync);
					theList.Add(orbPowerRing);
					num2++;
				}
				return;
			}
			sync.GetBuffer().WriteLong((long)theList.Count);
			foreach (OrbPowerRing orbPowerRing2 in theList)
			{
				orbPowerRing2.SyncState(sync);
			}
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x000770DC File Offset: 0x000752DC
		private void SyncListSkeletonPowerOrbs(DataSync sync, List<SkeletonPowerOrb> theList, bool clear)
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
					SkeletonPowerOrb skeletonPowerOrb = new SkeletonPowerOrb();
					skeletonPowerOrb.SyncState(sync);
					theList.Add(skeletonPowerOrb);
					num2++;
				}
				return;
			}
			sync.GetBuffer().WriteLong((long)theList.Count);
			foreach (SkeletonPowerOrb skeletonPowerOrb2 in theList)
			{
				skeletonPowerOrb2.SyncState(sync);
			}
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x0007717C File Offset: 0x0007537C
		private void SyncListBeamComponents(DataSync sync, List<BeamComponent> theList, bool clear)
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
					BeamComponent beamComponent = new BeamComponent();
					beamComponent.SyncState(sync);
					theList.Add(beamComponent);
					num2++;
				}
				return;
			}
			sync.GetBuffer().WriteLong((long)theList.Count);
			foreach (BeamComponent beamComponent2 in theList)
			{
				beamComponent2.SyncState(sync);
			}
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x0007721C File Offset: 0x0007541C
		public void SyncState(DataSync theSync)
		{
			SexyBuffer buffer = theSync.GetBuffer();
			theSync.SyncFloat(ref this.mBossStateAlpha);
			theSync.SyncLong(ref this.mBossStateHoldTimer);
			theSync.SyncLong(ref this.mBossStateAlphaDir);
			theSync.SyncLong(ref this.mBossState);
			theSync.SyncLong(ref this.mSickAnim.mUpdateCount);
			theSync.SyncFloat(ref this.mBossDeathVY);
			theSync.SyncFloat(ref this.mBossDeathVX);
			theSync.SyncFloat(ref this.mBossDeathTX);
			theSync.SyncFloat(ref this.mBossDeathTY);
			theSync.SyncFloat(ref this.mCannonAngle);
			theSync.SyncLong(ref this.mStunTimer);
			theSync.SyncFloat(ref this.mAngle);
			theSync.SyncFloat(ref this.mDestAngle);
			theSync.SyncFloat(ref this.mCenterX);
			theSync.SyncFloat(ref this.mCenterY);
			theSync.SyncLong(ref this.mDestX1);
			theSync.SyncLong(ref this.mDestY1);
			theSync.SyncLong(ref this.mDestX2);
			theSync.SyncLong(ref this.mDestY2);
			theSync.SyncLong(ref this.mSlowTimer);
			theSync.SyncLong(ref this.mDestTime);
			theSync.SyncLong(ref this.mDestCount);
			theSync.SyncFloat(ref this.mCurX);
			theSync.SyncFloat(ref this.mCurY);
			theSync.SyncLong(ref this.mWidth);
			theSync.SyncLong(ref this.mHeight);
			theSync.SyncLong(ref this.mCannonCount);
			theSync.SyncLong(ref this.mLazerCount);
			theSync.SyncLong(ref this.mBX);
			theSync.SyncLong(ref this.mBY);
			theSync.SyncFloat(ref this.mRecoilAmt);
			theSync.SyncFloat(ref this.mStatePercent);
			theSync.SyncFloat(ref this.mFireVel);
			theSync.SyncLong(ref this.mStunSpinFrame);
			theSync.SyncLong(ref this.mStartingStunTime);
			theSync.SyncLong(ref this.mBlinkCount);
			theSync.SyncLong(ref this.mBlinkTimer);
			theSync.SyncFloat(ref this.mLazerPercent);
			theSync.SyncBoolean(ref this.mDoingHop);
			theSync.SyncLong(ref this.mUpdateCount);
			int mType = (int)this.mState;
			theSync.SyncLong(ref mType);
			this.mState = (GunState)mType;
			this.SyncListComponents(theSync, this.mLazerPulse, true);
			if (theSync.isRead())
			{
				this.mPowerRings.Clear();
				if (buffer.ReadBoolean())
				{
					this.mFlameStun = (buffer.ReadBoolean() ? Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_FIREBREATHDE150) : Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_FIREBREATHDE250)).Duplicate();
					Transform transform = new Transform();
					float num = GameApp.DownScaleNum(1f);
					int num2 = (int)((float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(400)) * num);
					int num3 = (int)((float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(330)) * num);
					transform.Translate((float)(-(float)num2 / 2), (float)(-(float)num3 / 2));
					transform.RotateRad(this.mAngle);
					transform.Translate((float)(num2 / 2), (float)(num3 / 2));
					transform.Translate((float)ZumasRevenge.Common._S(this.GetCenterX() + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(-70))), (float)ZumasRevenge.Common._S(this.GetCenterY() + ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(-55))));
					this.mFlameStun.SetTransform(transform.GetMatrix());
					this.mFlameStun.Play((int)buffer.ReadLong());
				}
				if (buffer.ReadBoolean())
				{
					this.mDarkFrogStun = (buffer.ReadBoolean() ? Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_FROG_HIT_125) : Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_FROG_HIT_175));
					this.mDarkFrogStun.ResetAnim();
					Transform transform2 = new Transform();
					float num4 = ZumasRevenge.Common._DS(1f);
					int num5 = (int)((float)ZumasRevenge.Common._M(400) * num4);
					int num6 = (int)((float)ZumasRevenge.Common._M(330) * num4);
					transform2.Translate((float)(-(float)num5 / 2), (float)(-(float)num6 / 2));
					transform2.RotateRad(3.1415927f);
					transform2.Translate((float)(num5 / 2), (float)(num6 / 2));
					transform2.Translate((float)ZumasRevenge.Common._S(this.GetCenterX() + ZumasRevenge.Common._M(-52)), (float)ZumasRevenge.Common._S(this.GetCenterY() + ZumasRevenge.Common._M1(-13)));
					this.mDarkFrogStun.SetTransform(transform2.GetMatrix());
					this.mDarkFrogStun.Play((int)buffer.ReadLong());
				}
			}
			else
			{
				buffer.WriteBoolean(this.mFlameStun != null);
				if (this.mFlameStun != null)
				{
					buffer.WriteBoolean(this.mFlameStunShort);
					buffer.WriteLong((long)((int)this.mFlameStun.mMainSpriteInst.mFrameNum));
				}
				buffer.WriteBoolean(this.mDarkFrogStun != null);
				if (this.mDarkFrogStun != null)
				{
					buffer.WriteBoolean(this.mDarkFrogStunShort);
					buffer.WriteLong((long)((int)this.mDarkFrogStun.mMainSpriteInst.mFrameNum));
				}
			}
			this.SyncListOrbPowerRings(theSync, this.mPowerRings, true);
			this.SyncListSkeletonPowerOrbs(theSync, this.mPowerOrbs, true);
			theSync.SyncLong(ref this.mCannonRuneAlpha);
			theSync.SyncLong(ref this.mCannonLightness);
			theSync.SyncLong(ref this.mCannonRuneColor);
			theSync.SyncLong(ref this.mCannonState);
			mType = (int)this.mCurrentBody.mType;
			theSync.SyncLong(ref mType);
			this.mCurrentBody.mType = (FrogType)mType;
			if (theSync.isWrite())
			{
				this.mCurrentBody.SyncState(theSync);
				buffer.WriteShort((short)this.mFrogStack.Count);
				for (int i = 0; i < this.mFrogStack.size<FrogBody>(); i++)
				{
					buffer.WriteLong((long)this.mFrogStack[i].mType);
					this.mFrogStack[i].SyncState(theSync);
				}
				buffer.WriteBoolean(this.mDizzyStars != null);
				if (this.mDizzyStars != null)
				{
					ZumasRevenge.Common.SerializeParticleSystem(this.mDizzyStars, theSync);
				}
			}
			else
			{
				this.SetFrogType(this.mCurrentBody.mType, true);
				this.mCurrentBody.SyncState(theSync);
				int num7 = (int)buffer.ReadShort();
				for (int j = 0; j < num7; j++)
				{
					this.SetFrogType((FrogType)buffer.ReadLong(), false);
					this.mFrogStack.back<FrogBody>().SyncState(theSync);
				}
				this.mDizzyStars = null;
				if (buffer.ReadBoolean())
				{
					this.mDizzyStars = ZumasRevenge.Common.DeserializeParticleSystem(theSync);
				}
			}
			if (theSync.isRead())
			{
				this.EmptyBullets(false);
				if (buffer.ReadBoolean())
				{
					this.mBullet = new Bullet();
					this.mBullet.mFrog = this;
					this.mBullet.SyncState(theSync);
				}
				if (buffer.ReadBoolean())
				{
					this.mNextBullet = new Bullet();
					this.mNextBullet.mFrog = this;
					this.mNextBullet.SyncState(theSync);
				}
				int num8 = (int)buffer.ReadLong();
				for (int k = 0; k < num8; k++)
				{
					Bullet bullet = new Bullet();
					bullet.mFrog = this;
					bullet.SyncState(theSync);
					this.mCannonBullets.Add(bullet);
				}
			}
			else
			{
				buffer.WriteBoolean(this.mBullet != null);
				if (this.mBullet != null)
				{
					this.mBullet.SyncState(theSync);
				}
				buffer.WriteBoolean(this.mNextBullet != null);
				if (this.mNextBullet != null)
				{
					this.mNextBullet.SyncState(theSync);
				}
				buffer.WriteLong((long)this.mCannonBullets.Count);
				for (int l = 0; l < this.mCannonBullets.Count; l++)
				{
					this.mCannonBullets[l].SyncState(theSync);
				}
			}
			this.mElectricOrb.SyncState(theSync);
			if (theSync.isRead())
			{
				this.mElectricOrb.mImage = (MemoryImage)Res.GetImageByID(ResID.IMAGE_FROG_ELECTRIC_ORB);
			}
			for (int m = 0; m < 3; m++)
			{
				this.SyncListBeamComponents(theSync, this.mBeams[m], true);
				if (theSync.isRead())
				{
					Image image = null;
					switch (m)
					{
					case 0:
						image = Res.GetImageByID(ResID.IMAGE_FROG_BEAM_A);
						break;
					case 1:
						image = Res.GetImageByID(ResID.IMAGE_FROG_BEAM_B);
						break;
					case 2:
						image = Res.GetImageByID(ResID.IMAGE_FROG_BEAM_C);
						break;
					}
					for (int n = 0; n < this.mBeams[m].size<BeamComponent>(); n++)
					{
						this.mBeams[m][n].mImage = (MemoryImage)image;
					}
				}
			}
			theSync.SyncLong(ref this.mLastGuideX);
			theSync.SyncLong(ref this.mLastGuideY);
			theSync.SyncFloat(ref this.mBeamProjectedEndX);
			theSync.SyncFloat(ref this.mBeamProjectedEndY);
			theSync.SyncFloat(ref this.mBeamDistToTarget);
			theSync.SyncFloat(ref this.mFarthestDistance);
			theSync.SyncBoolean(ref this.mDoElectricBeamShit);
			if (theSync.isRead() && this.mBoard.LevelIsSkeletonBoss() && this.mBullet != null && this.mBullet.GetIsCannon())
			{
				this.SetCannonCount(1, false, 0, 0f);
			}
			if (theSync.isRead() && this.GetType() == 1)
			{
				this.mCannonBlast = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_CANNONBLAST).Duplicate();
			}
			if (theSync.isRead() && this.GetType() == 3)
			{
				GameApp.gApp.mSoundPlayer.Loop(Res.GetSoundByID(ResID.SOUND_LIGHTNING_LOOP));
			}
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00077B1C File Offset: 0x00075D1C
		public void FireElectricOrb()
		{
			float num = ZumasRevenge.Common._M(20f);
			this.mElectricOrb.mVX = num * (float)Math.Cos((double)this.GetBeamAngle());
			this.mElectricOrb.mVY = num * -(float)Math.Sin((double)this.GetBeamAngle());
			this.mElectricOrb.mV0 = num;
			int mAlphaDelta = (int)ZumasRevenge.Common._M(-20f);
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < Enumerable.Count<BeamComponent>(this.mBeams[i]); j++)
				{
					this.mBeams[i][j].mMinAlpha = 0;
					this.mBeams[i][j].mAlphaDelta = mAlphaDelta;
				}
			}
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00077BD0 File Offset: 0x00075DD0
		public void AddBubble(Bubble b)
		{
			if ((this.mBoard.mLevel.mBoss != null && this.mBoard.mLevel.mZone != 5) || this.mBoard.DoingLevelTransition() || this.mBoard.GetGameState() != GameState.GameState_Playing)
			{
				return;
			}
			this.mBubbles.Add(b);
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00077C2C File Offset: 0x00075E2C
		public Bullet GetFiredBullet()
		{
			if (this.mState != GunState.GunState_Firing || this.mStatePercent < ZumasRevenge.Common._M(0.9f))
			{
				return null;
			}
			if (this.mBullet.mSkip)
			{
				this.mBullet.mSkip = false;
				return null;
			}
			Bullet bullet = this.mBullet;
			if (this.mCannonBullets.size<Bullet>() > 0)
			{
				this.mBullet = Enumerable.First<Bullet>(this.mCannonBullets);
				this.mCannonBullets.Remove(this.mBullet);
			}
			else
			{
				this.mState = GunState.GunState_Normal;
				if (bullet.GetIsCannon())
				{
					bullet = new Bullet(this.mBullet);
					bullet.mFrog = this;
					this.mBullet.mSkip = true;
					this.mBullet.SetJustFired(false);
					this.mBullet.SetIsCannon(false);
				}
				else
				{
					this.mBullet = null;
				}
			}
			return bullet;
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00077D04 File Offset: 0x00075F04
		public void SetPos(int theX, int theY)
		{
			this.mCenterX = (float)theX;
			this.mCenterY = (float)theY;
			this.mDestCount = 0;
			this.mCurX = (float)theX;
			this.mCurY = (float)theY;
			this.CalcAngle();
			int num = ((this.GetCenterX() > ZumasRevenge.Common._SS(GameApp.gApp.mWidth) - this.GetCenterX()) ? 0 : ZumasRevenge.Common._SS(GameApp.gApp.mWidth));
			int num2 = ((this.GetCenterY() > ZumasRevenge.Common._SS(GameApp.gApp.mHeight) - this.GetCenterY()) ? 0 : ZumasRevenge.Common._SS(GameApp.gApp.mHeight));
			this.mFarthestDistance = MathUtils.Distance((float)this.GetCenterX(), (float)this.GetCenterY(), (float)num, (float)num2);
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00077DC0 File Offset: 0x00075FC0
		public void Update()
		{
			if (this.mBossStateAlphaDir != 0)
			{
				this.mBossStateAlpha += ZumasRevenge.Common._M(1.5f) * (float)this.mBossStateAlphaDir;
				if (this.mBossStateAlpha > 255f)
				{
					this.mBossStateAlphaDir = 0;
					this.mBossStateAlpha = 255f;
				}
				else if (this.mBossStateAlpha < 0f)
				{
					this.mBossStateAlphaDir = 0;
					this.mBossStateAlpha = 0f;
				}
			}
			else if (this.mBossStateHoldTimer > 0 && --this.mBossStateHoldTimer == 0)
			{
				this.mBossStateAlphaDir = -1;
			}
			if (this.mBoard.mLevel.mNum == 10 && this.mBoard.mLevel.mZone == 5 && Enumerable.Count<Bubble>(this.mBubbles) > 0)
			{
				this.mBubbles.Clear();
			}
			if (this.mLightningEffect != null)
			{
				this.mGlobalTranform.Reset();
				this.mGlobalTranform.Translate(ZumasRevenge.Common._S(this.mCenterX + (float)ZumasRevenge.Common._M(-45)), 0f);
				this.mLightningEffect.SetTransform(this.mGlobalTranform.GetMatrix());
				this.mLightningEffect.Update();
			}
			if (this.IsPoisoned())
			{
				int num = (int)ZumasRevenge.Common._M(134f);
				int num2 = this.mSickAnim.GetMaxDuration() - num;
				if (this.mBoard.mLevel.mInvertMouseTimer <= num2 || this.mSickAnim.GetUpdateCount() < num)
				{
					this.mSickAnim.Update();
				}
			}
			for (int i = 0; i < 4; i++)
			{
				if (this.mLazerFrogBackPulseAlpha[i] > 0f)
				{
					float num3 = this.mLazerFrogBackPulseAlpha[i];
					this.mLazerFrogBackPulseAlpha[i] -= ZumasRevenge.Common._M(10f);
					if (this.mLazerFrogBackPulseAlpha[i] < 0f)
					{
						this.mLazerFrogBackPulseAlpha[i] = 0f;
					}
					if (num3 > (float)ZumasRevenge.Common._M(200) && this.mLazerFrogBackPulseAlpha[i] < (float)ZumasRevenge.Common._M1(200) && i + 1 < 4 && this.mLazerFrogBackPulseAlpha[i + 1] == 0f)
					{
						this.mLazerFrogBackPulseAlpha[i + 1] = 255f;
					}
					if (this.mLazerFrogBackPulseAlpha[i] > (float)ZumasRevenge.Common._M(200))
					{
						break;
					}
				}
			}
			for (int j = 0; j < Enumerable.Count<LTSmokeParticle>(this.mSmokeParticles); j++)
			{
				if (BambooTransition.UpdateSmokeParticle(this.mSmokeParticles[j]))
				{
					this.mSmokeParticles[j] = null;
					this.mSmokeParticles.RemoveAt(j);
					j--;
				}
			}
			if (this.mDoingCannonBlast && this.mCannonBlast.IsActive())
			{
				this.mCannonBlast.Update();
				if (this.mCannonBlast.mFrameNum > (float)this.mCannonBlast.mLastFrameNum)
				{
					this.mDoingCannonBlast = false;
				}
				else
				{
					this.mCannonBlast.mDrawTransform.LoadIdentity();
					float num4 = GameApp.DownScaleNum(1f);
					this.mCannonBlast.mDrawTransform.Scale(num4, num4);
					this.mCannonBlast.mDrawTransform.RotateRad(this.mAngle);
					float num5 = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(0));
					float num6 = (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(185));
					JeffLib.Common.RotatePoint(this.mAngle, ref num5, ref num6, 0f, 0f);
					this.mCannonBlast.mDrawTransform.Translate(ZumasRevenge.Common._S(this.mCurX) + num5, ZumasRevenge.Common._S(this.mCurY) + num6);
				}
			}
			if (this.mDizzyStars != null)
			{
				this.mDizzyStars.Update();
				this.mDizzyStars.SetPos(this.mCenterX, this.mCenterY);
				if (this.mDizzyStars.GetUpdateCount() > 50 && this.mDizzyStars.GetTotalParticles() == 0)
				{
					this.mDizzyStars = null;
				}
			}
			this.mUpdateCount++;
			this.mCurX = this.mCenterX;
			this.mCurY = this.mCenterY;
			if (this.mFlameStun != null)
			{
				this.mGlobalTranform.Reset();
				float num7 = ZumasRevenge.Common._DS(1f);
				int num8 = (int)(ZumasRevenge.Common._M(400f) * num7);
				int num9 = (int)(ZumasRevenge.Common._M(330f) * num7);
				this.mGlobalTranform.Translate((float)(-(float)num8 / 2), (float)(-(float)num9 / 2));
				this.mGlobalTranform.RotateRad(this.mAngle);
				this.mGlobalTranform.Translate((float)(num8 / 2), (float)(num9 / 2));
				this.mGlobalTranform.Translate((float)ZumasRevenge.Common._S(this.GetCenterX() + ZumasRevenge.Common._M(-100)), (float)ZumasRevenge.Common._S(this.GetCenterY() + ZumasRevenge.Common._M(-80)));
				this.mFlameStun.SetTransform(this.mGlobalTranform.GetMatrix());
				this.mFlameStun.Update();
				if (!this.mFlameStun.IsActive())
				{
					GameApp.gApp.mSoundPlayer.Fade(Res.GetSoundByID(ResID.SOUND_NEW_BURNINGFROGLOOP));
					this.mFlameStun = null;
					this.mSpitAlpha = 0f;
					this.mStunTimer = 0;
				}
			}
			else if (this.mDarkFrogStun != null)
			{
				SexyTransform2D transform = new SexyTransform2D(false);
				float num10 = ZumasRevenge.Common._DS(1f);
				int num11 = (int)((float)ZumasRevenge.Common._M(400) * num10);
				int num12 = (int)((float)ZumasRevenge.Common._M(330) * num10);
				transform.Translate((float)(-(float)num11 / 2), (float)(-(float)num12 / 2));
				transform.RotateRad(3.1415927f);
				transform.Translate((float)(num11 / 2), (float)(num12 / 2));
				transform.Translate((float)ZumasRevenge.Common._S(this.GetCenterX() + ZumasRevenge.Common._M(-52)), (float)ZumasRevenge.Common._S(this.GetCenterY() + ZumasRevenge.Common._M1(-13)));
				this.mDarkFrogStun.SetTransform(transform);
				this.mDarkFrogStun.Update();
				if (!this.mDarkFrogStun.IsActive() || this.mDarkFrogStun.mMainSpriteInst.mFrameNum >= (float)(Enumerable.Count<PAFrame>(this.mDarkFrogStun.mMainSpriteInst.mDef.mFrames) - 1))
				{
					GameApp.gApp.mSoundPlayer.Fade(Res.GetSoundByID(ResID.SOUND_NEW_BURNINGFROGLOOP));
					this.mAngle = 3.1415927f;
					this.mDarkFrogStun = null;
					this.mSpitAlpha = 0f;
					this.mStunTimer = 0;
				}
			}
			if (this.mBossDeathVX != 0f || this.mBossDeathVY != 0f)
			{
				this.mCenterX += this.mBossDeathVX;
				this.mCenterY += this.mBossDeathVY;
				if ((this.mBossDeathVX > 0f && this.mCenterX >= this.mBossDeathTX) || (this.mBossDeathVX < 0f && this.mCenterX <= this.mBossDeathTX))
				{
					this.mCenterX = this.mBossDeathTX;
					this.mBossDeathVX = 0f;
				}
				if ((this.mBossDeathVY > 0f && this.mCenterY >= this.mBossDeathTY) || (this.mBossDeathVY < 0f && this.mCenterY <= this.mBossDeathTY))
				{
					this.mCenterY = this.mBossDeathTY;
					this.mBossDeathVY = 0f;
				}
			}
			if (this.mBoard.GetHallucinateTimer() > 0)
			{
				this.mTempText.Update();
				this.mTempText.SetX((float)ZumasRevenge.Common._S(this.GetCenterX() - ZumasRevenge.Common._M(32)));
				this.mTempText.SetY((float)ZumasRevenge.Common._S(this.GetCenterY() - ZumasRevenge.Common._M(100)));
			}
			for (int k = 0; k < Enumerable.Count<OrbPowerRing>(this.mPowerRings); k++)
			{
				this.mPowerRings[k].Update();
			}
			for (int l = 0; l < Enumerable.Count<SkeletonPowerOrb>(this.mPowerOrbs); l++)
			{
				SkeletonPowerOrb skeletonPowerOrb = this.mPowerOrbs[l];
				float num13 = ZumasRevenge.Common._M(2f);
				float num14 = ZumasRevenge.Common._M(0.1f);
				if (skeletonPowerOrb.mSize < num13)
				{
					skeletonPowerOrb.mSize += num14;
					if (skeletonPowerOrb.mSize > num13)
					{
						skeletonPowerOrb.mSize = num13;
					}
				}
				else if (skeletonPowerOrb.mAlpha > 0f)
				{
					skeletonPowerOrb.mAlpha -= ZumasRevenge.Common._M(3f);
					if (skeletonPowerOrb.mAlpha <= 0f)
					{
						this.mPowerOrbs.RemoveAt(l);
						l--;
					}
				}
			}
			if (this.mBullet != null)
			{
				this.mBullet.Update();
			}
			if (this.mNextBullet != null)
			{
				this.mNextBullet.Update();
			}
			for (int m = 0; m < Enumerable.Count<Bubble>(this.mBubbles); m++)
			{
				Bubble bubble = this.mBubbles[m];
				bubble.Update();
				if (bubble.GetAlpha() <= 0f)
				{
					this.mBubbles.RemoveAt(m);
					m--;
				}
			}
			if (this.mSlowTimer > 0)
			{
				this.mSlowTimer--;
			}
			if (this.IsFuckedUp() && !this.IsStunned() && this.mUpdateCount % ZumasRevenge.Common._M(10) == 0)
			{
				ConfusionMark confusionMark = new ConfusionMark();
				this.mConfusionMarks.Add(confusionMark);
				confusionMark.mImage = Res.GetImageByID(ResID.IMAGE_QUESTION_MARK);
				confusionMark.mX = 0f;
				confusionMark.mY = 0f;
				confusionMark.mSize = ZumasRevenge.Common._M(0.1f);
				confusionMark.mAlpha = 0f;
				confusionMark.mAlphaInc = ZumasRevenge.Common._M(6f);
				confusionMark.mVX = (ZumasRevenge.Common._M(0.1f) + (float)(SexyFramework.Common.Rand() % ZumasRevenge.Common._M1(1000)) / ZumasRevenge.Common._M2(1000f)) * (float)((SexyFramework.Common.Rand() % 100 < 50) ? 1 : (-1));
				confusionMark.mVY = (ZumasRevenge.Common._M(0.1f) + (float)(SexyFramework.Common.Rand() % ZumasRevenge.Common._M1(1000)) / ZumasRevenge.Common._M2(1000f)) * (float)((SexyFramework.Common.Rand() % 100 < 50) ? 1 : (-1));
			}
			if (this.mBlinkCount >= 0 && --this.mBlinkTimer == 0)
			{
				this.mBlinkCount--;
				this.mBlinkTimer = (int)ZumasRevenge.Common._M(15f);
			}
			if (this.mSpitAlpha > 0f)
			{
				this.mSpitAlpha -= ZumasRevenge.Common._M(6f);
				this.mSpitX += this.mSpitVX;
				this.mSpitY += this.mSpitVY;
			}
			if (this.mStunTimer > 0 && this.mFlameStun == null && this.mDarkFrogStun == null && this.mBlinkCount < 0)
			{
				int num15 = (int)ZumasRevenge.Common._M(12f);
				if (this.mStunSpinFrame == 1)
				{
					num15 *= 2;
				}
				Image imageByID = Res.GetImageByID(ResID.IMAGE_FROG_SPIN_FRAMES);
				if (this.mUpdateCount % num15 == 0)
				{
					if (this.mStunTimer >= this.mStartingStunTime - ZumasRevenge.Common._M(50) && this.mStunSpinFrame == 2)
					{
						this.mStunSpinFrame = 0;
					}
					else if (this.mStunTimer - 3 * num15 <= 0 && this.mStunSpinFrame < imageByID.mNumRows - 1)
					{
						this.mStunSpinFrame = ((this.mStunSpinFrame == 0) ? 1 : 2);
						if (this.mStunSpinFrame == 1)
						{
							this.mAngle = (this.mDestAngle = 3.1415927f);
							this.mSpitAlpha = 255f;
							this.mSpitAngle = MathUtils.DegreesToRadians((float)ZumasRevenge.Common._M(-60));
							this.mSpitVX = (float)ZumasRevenge.Common._M(1) * (float)Math.Cos((double)this.mSpitAngle);
							this.mSpitVY = (float)(-(float)ZumasRevenge.Common._M(1)) * (float)Math.Sin((double)this.mSpitAngle);
							this.mSpitX = this.mCenterX + (float)ZumasRevenge.Common._M(50);
							this.mSpitY = this.mCenterY + (float)ZumasRevenge.Common._M(30);
						}
					}
				}
				if (--this.mStunTimer == 0)
				{
					this.mAngle = this.mDestAngle;
					for (int n = 0; n < Enumerable.Count<ConfusionMark>(this.mConfusionMarks); n++)
					{
						this.mConfusionMarks[n].mAlphaInc = ZumasRevenge.Common._M(-4f);
					}
					this.mBlinkCount = 3;
				}
			}
			for (int num16 = 0; num16 < Enumerable.Count<ConfusionMark>(this.mConfusionMarks); num16++)
			{
				ConfusionMark confusionMark2 = this.mConfusionMarks[num16];
				if (!this.IsFuckedUp())
				{
					confusionMark2.mAlphaInc = ZumasRevenge.Common._M(-4f);
				}
				confusionMark2.mAlpha += confusionMark2.mAlphaInc;
				confusionMark2.mX += confusionMark2.mVX;
				confusionMark2.mY += confusionMark2.mVY;
				confusionMark2.mSize += ZumasRevenge.Common._M(0.01f);
				if (confusionMark2.mAlpha >= 255f && confusionMark2.mAlphaInc > 0f)
				{
					confusionMark2.mAlpha = 255f;
					confusionMark2.mAlphaInc *= ZumasRevenge.Common._M(-1f);
				}
				else if (confusionMark2.mAlpha <= 0f && confusionMark2.mAlphaInc < 0f)
				{
					this.mConfusionMarks.RemoveAt(num16);
					num16--;
				}
			}
			if (Enumerable.Count<FrogBody>(this.mFrogStack) > 0 && this.mFrogStack[Enumerable.Count<FrogBody>(this.mFrogStack) - 1].mAlpha != 255 && this.mFrogStack[Enumerable.Count<FrogBody>(this.mFrogStack) - 1].mAlpha != -1)
			{
				FrogBody frogBody = this.mFrogStack[Enumerable.Count<FrogBody>(this.mFrogStack) - 1];
				frogBody.mAlpha += (int)ZumasRevenge.Common._M(20f);
				if (frogBody.mAlpha >= 255)
				{
					frogBody.mAlpha = -1;
					this.mCurrentBody = frogBody;
					this.mFrogStack.Clear();
					if (!this.LaserMode())
					{
						this.mLazerPulse.Clear();
					}
				}
			}
			float num17 = ZumasRevenge.Common._M(1f);
			if (this.mLazerCount > 0 && this.mLazerPercent < num17)
			{
				this.mLazerPercent += ZumasRevenge.Common._M(0.07f);
				if (this.mLazerPercent > num17)
				{
					this.mLazerPercent = num17;
				}
			}
			else if (this.mLazerCount == 0 && this.mLazerPercent > 0f)
			{
				this.mLazerPercent -= ZumasRevenge.Common._M(0.06f);
				if (this.mLazerPercent < 0f)
				{
					this.mLazerPercent = 0f;
					if (Enumerable.Count<FrogBody>(this.mFrogStack) == 0 && this.GetType() == 2)
					{
						this.SetFrogType(FrogType.FrogType_Normal, false);
					}
				}
			}
			if (this.LaserMode())
			{
				FrogBody frogBody2;
				if (Enumerable.Count<FrogBody>(this.mFrogStack) > 0 && this.mFrogStack[Enumerable.Count<FrogBody>(this.mFrogStack) - 1].mType == FrogType.FrogType_Lazer)
				{
					frogBody2 = this.mFrogStack[Enumerable.Count<FrogBody>(this.mFrogStack) - 1];
				}
				else
				{
					frogBody2 = this.mCurrentBody;
				}
				Image imageByID2 = Res.GetImageByID(ResID.IMAGE_FROG_LAZER_EYE_LOOP);
				if (this.mUpdateCount % ZumasRevenge.Common._M(6) == 0)
				{
					frogBody2.mCel = (frogBody2.mCel + 1) % imageByID2.mNumRows * imageByID2.mNumCols;
				}
				if (Component.UpdateComponentVec(this.mLazerPulse, this.mUpdateCount))
				{
					this.SetupLazerBackPulse();
				}
			}
			else if (this.mDoElectricBeamShit)
			{
				if (this.mElectricOrb.mVX == this.mElectricOrb.mVY && this.mElectricOrb.mVX == 0f)
				{
					this.RetargetBeams(0f);
				}
				this.UpdateBeams();
			}
			if (this.IsStunned() && this.mStunSpinFrame == 0)
			{
				this.mAngle += ZumasRevenge.Common._M(-0.15f);
			}
			else if (!this.IsStunned() && this.mAngle != this.mDestAngle)
			{
				float num18 = 100f;
				if (this.mBoard.mLevel.mMoveType != 0)
				{
					num18 = ZumasRevenge.Common._M(0.87f);
				}
				float num19 = num18;
				float num20 = this.mAngle;
				if (this.mAngle < this.mDestAngle)
				{
					this.mAngle += num19;
					if (this.mAngle > this.mDestAngle)
					{
						this.SetAngleToDestAngle();
					}
				}
				else
				{
					this.mAngle -= num19;
					if (this.mAngle < this.mDestAngle)
					{
						this.SetAngleToDestAngle();
					}
				}
				if (this.mDoElectricBeamShit && this.mElectricOrb.mVX == this.mElectricOrb.mVY && this.mElectricOrb.mVX == 0f)
				{
					this.RetargetBeams(this.mAngle - num20);
				}
			}
			if (this.mDestCount > 0)
			{
				this.mDestCount--;
				float num21 = (float)this.mDestCount / (float)this.mDestTime;
				this.mCenterX = num21 * (float)this.mDestX1 + (1f - num21) * (float)this.mDestX2;
				this.mCenterY = num21 * (float)this.mDestY1 + (1f - num21) * (float)this.mDestY2;
				if (this.mBoard.mLevel.mZone == 5 && this.mBoard.mLevel.mNum != 10 && this.mBoard.mLevel.mMoveType == 0)
				{
					if (this.mDoingHop)
					{
						this.DoBubbles(1);
					}
					else if (this.mUpdateCount % ZumasRevenge.Common._M(10) == 0)
					{
						this.DoBubbles(1);
					}
				}
				if (this.mDoElectricBeamShit)
				{
					this.RetargetBeams(0f);
				}
				if (this.mDestCount == 0)
				{
					this.mDoingHop = false;
				}
			}
			float num22 = ((this.mSlowTimer > 0) ? ZumasRevenge.Common._M(4f) : 1f);
			int num23 = 1;
			GameApp gameApp = (GameApp)GlobalMembers.gSexyAppBase;
			if (gameApp.mBoard != null && gameApp.mBoard.mIsHotFrogEnabled)
			{
				num23 = 2;
			}
			if (this.mState == GunState.GunState_Firing)
			{
				this.mStatePercent += 0.15f * (float)num23;
				if (this.mStatePercent > 0.6f)
				{
					this.mBullet.Update();
					this.mRecoilAmt += ZumasRevenge.Common._M(2.33f);
				}
			}
			else
			{
				this.mStatePercent += 0.07f / num22 * (float)num23;
				if (this.mState == GunState.GunState_Reloading && (this.mRecoilAmt -= ZumasRevenge.Common._M(0.7f) / num22) < 0f)
				{
					this.mRecoilAmt = 0f;
				}
			}
			if (this.mStatePercent > 1f)
			{
				this.mStatePercent = 1f;
				if (this.mState == GunState.GunState_Reloading)
				{
					this.mState = GunState.GunState_Normal;
					this.mRecoilAmt = 0f;
				}
			}
			if (this.mState == GunState.GunState_Normal && this.mRecoilAmt > 0f && (this.mRecoilAmt -= ZumasRevenge.Common._M(0.7f) / num22) < 0f)
			{
				this.mRecoilAmt = 0f;
			}
			if (this.CannonMode() || (this.mBullet != null && this.mBullet.GetIsCannon()))
			{
				if (this.mUpdateCount % ZumasRevenge.Common._M(18) == 0)
				{
					this.mCannonBallShadowPos = (this.mCannonBallShadowPos + 1) % Gun.NUM_CANNON_SHADOWS;
					this.mCannonBallShadows[this.mCannonBallShadowPos] = 255;
				}
				for (int num24 = 0; num24 < Gun.NUM_CANNON_SHADOWS; num24++)
				{
					if (this.mCannonBallShadows[num24] > 0)
					{
						this.mCannonBallShadows[num24] = Math.Max(0, this.mCannonBallShadows[num24] - 6);
					}
				}
			}
			if (this.mCannonState > 0)
			{
				this.UpdateCannonFadeIn();
			}
			this.CalcAngle();
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00079198 File Offset: 0x00077398
		public void SwapBullets(bool playSound)
		{
			if (this.mState != GunState.GunState_Normal)
			{
				return;
			}
			if (this.mBullet == null || this.mNextBullet == null)
			{
				return;
			}
			if (this.mBullet.GetColorType() == this.mNextBullet.GetColorType())
			{
				return;
			}
			if (playSound)
			{
				GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BALLSWAP));
			}
			Bullet bullet = this.mBullet;
			this.mBullet = this.mNextBullet;
			this.mNextBullet = bullet;
			this.mBullet.SetIsCannon(this.mNextBullet.GetIsCannon());
			this.mNextBullet.SetIsCannon(false);
			this.CalcAngle();
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00079231 File Offset: 0x00077431
		public void SwapBullets()
		{
			this.SwapBullets(true);
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x0007923A File Offset: 0x0007743A
		public void EmptyBullets(bool reset_frog_type)
		{
			if (reset_frog_type)
			{
				this.mDoElectricBeamShit = false;
				this.SetFrogType(FrogType.FrogType_Normal, true);
			}
			this.mNextBullet = null;
			this.mBullet = null;
			this.mCannonBullets.Clear();
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00079267 File Offset: 0x00077467
		public void EmptyBullets()
		{
			this.EmptyBullets(true);
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00079270 File Offset: 0x00077470
		public void SetBulletType(int theType)
		{
			if (this.mBullet != null && theType != -1)
			{
				this.mBullet.SetColorType(theType);
			}
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0007928A File Offset: 0x0007748A
		public void SetNextBulletType(int theType)
		{
			if (this.mNextBullet != null && theType != -1)
			{
				this.mNextBullet.SetColorType(theType);
			}
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x000792A4 File Offset: 0x000774A4
		public Rect GetRect()
		{
			int num = this.mWidth - 10;
			int num2 = this.mHeight - 10;
			return new Rect(this.GetCenterX() - num / 2, this.GetCenterY() - num2 / 2, num, num2);
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x000792E0 File Offset: 0x000774E0
		public void SetCannonCount(int c, bool stack, int color_type, float cannon_angle)
		{
			if (this.LightningMode())
			{
				return;
			}
			this.mCannonBlast = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_CANNONBLAST).Duplicate();
			if (cannon_angle < 0f)
			{
				this.mCannonAngle = GameApp.gApp.GetLevelMgr().mCannonAngle;
			}
			else
			{
				this.mCannonAngle = cannon_angle;
			}
			if (c != 0)
			{
				if (this.mCannonCount == 0)
				{
					this.mCannonRuneColor = color_type;
					this.mCannonState = 1;
					this.mCannonRuneAlpha = (this.mCannonLightness = 0);
					if (this.mBoard.LevelIsSkeletonBoss())
					{
						this.mCannonBlast = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_BOSS3CANNONBLAST);
						this.mCannonRuneColor = 0;
						this.mLightningEffect = Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_BOLTEFFECT);
						this.mGlobalTranform.Reset();
						this.mGlobalTranform.Translate(ZumasRevenge.Common._S(this.mCenterX + (float)ZumasRevenge.Common._M(-45)), 0f);
						this.mLightningEffect.SetTransform(this.mGlobalTranform.GetMatrix());
						this.mLightningEffect.mListener = this;
						this.mLightningEffect.Play("Main");
					}
				}
				if (stack)
				{
					this.mCannonCount += c - 1;
				}
				else
				{
					this.mCannonCount = c - 1;
				}
			}
			else
			{
				this.mCannonCount = 0;
			}
			if (c == 0)
			{
				if (this.mBullet != null)
				{
					this.mBullet.SetIsCannon(false);
				}
				if (this.mNextBullet != null)
				{
					this.mNextBullet.SetIsCannon(false);
					return;
				}
			}
			else if (this.mBullet != null)
			{
				if (!this.mBullet.GetIsCannon())
				{
					this.SetFrogType(FrogType.FrogType_Cannon, false);
					if (!this.mBullet.GetJustFired())
					{
						this.mBullet.SetIsCannon(true);
						return;
					}
					if (!this.mNextBullet.GetJustFired())
					{
						this.mNextBullet.SetIsCannon(true);
						return;
					}
					this.mCannonCount++;
					return;
				}
				else
				{
					if (this.mNextBullet != null && !this.mNextBullet.GetIsCannon() && this.mState != GunState.GunState_Normal)
					{
						this.SetFrogType(FrogType.FrogType_Cannon, false);
						this.mNextBullet.SetIsCannon(true);
						return;
					}
					if (this.mState != GunState.GunState_Normal)
					{
						this.mCannonCount++;
					}
				}
			}
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x000794FC File Offset: 0x000776FC
		public void SetCannonCount(int c, bool stack, int color_type)
		{
			this.SetCannonCount(c, stack, color_type, -100f);
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0007950C File Offset: 0x0007770C
		public void DoLightningFrog(bool is_lightning)
		{
			if (is_lightning)
			{
				this.SetFrogType(FrogType.FrogType_Lightning, false);
				return;
			}
			this.SetFrogType(FrogType.FrogType_Normal, false);
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x00079522 File Offset: 0x00077722
		public void DoLightningFrog()
		{
			this.DoLightningFrog(true);
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x0007952B File Offset: 0x0007772B
		public void DoLazerFrog(int c, bool stack)
		{
			if (this.LightningMode())
			{
				return;
			}
			if (!stack)
			{
				this.mLazerCount = c;
			}
			else
			{
				this.mLazerCount += c - 1;
			}
			this.SetFrogType(FrogType.FrogType_Lazer, false);
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0007955C File Offset: 0x0007775C
		public void DecLazerCount()
		{
			if (this.mLazerCount > 0)
			{
				this.mLazerCount--;
				for (int i = 0; i < 4; i++)
				{
					this.mLazerFrogBackPulseAlpha[i] = 0f;
				}
				this.mLazerFrogBackPulseAlpha[0] = 255f;
			}
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x000795A6 File Offset: 0x000777A6
		public void ResetFrogType()
		{
			this.SetFrogType(FrogType.FrogType_Normal, false);
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x000795B0 File Offset: 0x000777B0
		public void PlayerDied()
		{
			this.mState = GunState.GunState_Normal;
			this.mRecoilAmt = 0f;
			this.mStatePercent = 1f;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x000795D0 File Offset: 0x000777D0
		public bool CanSpawnPowerUp(PowerType p)
		{
			return (p != PowerType.PowerType_Cannon || (!this.CannonMode() && (this.mBullet == null || !this.mBullet.GetIsCannon()))) && (p != PowerType.PowerType_ColorNuke || !this.LightningMode()) && (p != PowerType.PowerType_Laser || !this.LaserMode()) && p != PowerType.PowerType_Accuracy;
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00079624 File Offset: 0x00077824
		public int GetCenterX()
		{
			return (int)this.mCenterX;
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0007962D File Offset: 0x0007782D
		public int GetCenterY()
		{
			return (int)this.mCenterY;
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x00079636 File Offset: 0x00077836
		public int GetCurX()
		{
			return (int)this.mCurX;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x0007963F File Offset: 0x0007783F
		public int GetCurY()
		{
			return (int)this.mCurY;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00079648 File Offset: 0x00077848
		public int GetCenterXDest()
		{
			return (int)((this.mDestCount != 0) ? ((float)this.mDestX2) : this.mCenterX);
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00079662 File Offset: 0x00077862
		public int GetCenterYDest()
		{
			return (int)((this.mDestCount != 0) ? ((float)this.mDestY2) : this.mCenterY);
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x0007967C File Offset: 0x0007787C
		public int GetWidth()
		{
			return this.mWidth;
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x00079684 File Offset: 0x00077884
		public int GetHeight()
		{
			return this.mHeight;
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x0007968C File Offset: 0x0007788C
		public int GetType()
		{
			if (Enumerable.Count<FrogBody>(this.mFrogStack) <= 0)
			{
				return (int)this.mCurrentBody.mType;
			}
			return (int)this.mFrogStack[Enumerable.Count<FrogBody>(this.mFrogStack) - 1].mType;
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x000796C5 File Offset: 0x000778C5
		public int GetLazerCount()
		{
			return this.mLazerCount;
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x000796CD File Offset: 0x000778CD
		public Bullet GetBullet()
		{
			return this.mBullet;
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x000796D5 File Offset: 0x000778D5
		public Bullet GetNextBullet()
		{
			return this.mNextBullet;
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x000796DD File Offset: 0x000778DD
		public float GetAngle()
		{
			return this.mAngle;
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x000796E5 File Offset: 0x000778E5
		public float GetDestAngle()
		{
			return this.mDestAngle;
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x000796ED File Offset: 0x000778ED
		public bool IsInked()
		{
			return this.mBossStateAlpha > 0f;
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x000796FC File Offset: 0x000778FC
		public bool IsFiring()
		{
			return this.mState == GunState.GunState_Firing;
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x00079707 File Offset: 0x00077907
		public bool IsHopping()
		{
			return this.mDoingHop && this.mDestCount > 0;
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0007971C File Offset: 0x0007791C
		public bool IsMovingToDest()
		{
			return this.mDestCount > 0;
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x00079727 File Offset: 0x00077927
		public bool CannonMode()
		{
			return this.mCannonCount > 0;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00079732 File Offset: 0x00077932
		public bool LaserMode()
		{
			return this.GetType() == 2 && this.mLazerPercent > 0f;
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0007974C File Offset: 0x0007794C
		public bool LightningMode()
		{
			return this.GetType() == 3;
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x00079757 File Offset: 0x00077957
		public bool IsCannon()
		{
			return this.GetType() == 1;
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x00079762 File Offset: 0x00077962
		public bool IsStunned()
		{
			return this.mStunTimer > 0;
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0007976D File Offset: 0x0007796D
		public bool IsPoisoned()
		{
			return this.mBoard.mLevel.mInvertMouseTimer > 0;
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00079782 File Offset: 0x00077982
		public bool HasSmokeParticles()
		{
			return Enumerable.Count<LTSmokeParticle>(this.mSmokeParticles) > 0;
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x00079792 File Offset: 0x00077992
		public bool IsSlow()
		{
			return this.mSlowTimer > 0;
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x000797A0 File Offset: 0x000779A0
		public void ClearStun()
		{
			GameApp.gApp.mSoundPlayer.Fade(Res.GetSoundByID(ResID.SOUND_NEW_BURNINGFROGLOOP));
			this.mFlameStun = null;
			this.mFlameStun = null;
			this.mSpitAlpha = 0f;
			this.mStunTimer = 0;
			this.mDarkFrogStun = null;
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x000797ED File Offset: 0x000779ED
		public bool IsFuckedUp()
		{
			return this.IsStunned() || this.mSlowTimer > 0 || this.IsPoisoned() || GameApp.gApp.GetBoard().GetHallucinateTimer() > 0;
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x0007981C File Offset: 0x00077A1C
		public bool StunnedFromBoss6()
		{
			return this.mFlameStun != null || this.mDarkFrogStun != null;
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00079834 File Offset: 0x00077A34
		public void ToggleShowNextBall()
		{
			this.mShowNextBall = !this.mShowNextBall;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00079848 File Offset: 0x00077A48
		public float GetFireSpeed()
		{
			float num;
			if (this.mBoard.mAccuracyCount > 0)
			{
				num = 19f;
			}
			else
			{
				num = 9.6f;
			}
			if (this.mSlowTimer > 0)
			{
				num *= 0.25f;
			}
			return num;
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x00079889 File Offset: 0x00077A89
		public void SetFireSpeed(float theSpeed)
		{
			this.mFireVel = theSpeed;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00079894 File Offset: 0x00077A94
		public void Stun(int timer)
		{
			if (this.mStunTimer > 0)
			{
				return;
			}
			this.mBlinkCount = 1;
			this.mBlinkTimer = ZumasRevenge.Common._M(15);
			this.mStartingStunTime = timer;
			this.mStunSpinFrame = Res.GetImageByID(ResID.IMAGE_FROG_SPIN_FRAMES).mNumRows - 1;
			this.mStunTimer = timer;
			GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_FROG_STUNNED));
			if (this.mBoard.mLevel.mEndSequence == 5)
			{
				this.mDarkFrogStun = ((timer == 125) ? Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_FROG_HIT_125).Duplicate() : Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_FROG_HIT_175).Duplicate());
				this.mDarkFrogStun.ResetAnim();
				this.mGlobalTranform.Reset();
				float num = ZumasRevenge.Common._DS(1f);
				int num2 = (int)((float)ZumasRevenge.Common._M(400) * num);
				int num3 = (int)((float)ZumasRevenge.Common._M(330) * num);
				this.mGlobalTranform.Translate((float)(-(float)num2 / 2), (float)(-(float)num3 / 2));
				this.mGlobalTranform.RotateRad(3.1415927f);
				this.mGlobalTranform.Translate((float)(num2 / 2), (float)(num3 / 2));
				this.mGlobalTranform.Translate((float)ZumasRevenge.Common._S(this.GetCenterX() + ZumasRevenge.Common._M(-52)), (float)ZumasRevenge.Common._S(this.GetCenterY() + ZumasRevenge.Common._M1(-13)));
				this.mDarkFrogStun.SetTransform(this.mGlobalTranform.GetMatrix());
				this.mDarkFrogStun.Play("Hit");
				this.mDarkFrogStunShort = timer == 125;
				return;
			}
			if (this.mBoard.mLevel.mEndSequence != 3 && this.mDizzyStars == null)
			{
				this.mDizzyStars = new SexyFramework.PIL.System(50, 50);
				this.mDizzyStars.mScale = ZumasRevenge.Common._S(1f);
				this.mDizzyStars.WaitForEmitters(true);
				this.mDizzyStars.SetPos(this.mCenterX, this.mCenterY);
				Emitter emitter = new Emitter();
				emitter.mPreloadFrames = ZumasRevenge.Common._M(100);
				emitter.mCullingRect = new Rect(0, 0, ZumasRevenge.Common._SS(GameApp.gApp.mWidth), ZumasRevenge.Common._SS(GameApp.gApp.mHeight));
				emitter.mDeleteInvisParticles = true;
				emitter.AddScaleKeyFrame(0, new EmitterScale
				{
					mLifeScale = ZumasRevenge.Common._M(0.5f),
					mNumberScale = ZumasRevenge.Common._M(1.61f),
					mSizeXScale = ZumasRevenge.Common._M(1.4f),
					mWeightScale = 0f,
					mSpinScale = ZumasRevenge.Common._M(0.2f),
					mMotionRandScale = 0f,
					mZoom = ZumasRevenge.Common._M(0.14f)
				});
				EmitterSettings emitterSettings = new EmitterSettings();
				emitterSettings.mVisibility = 0f;
				emitter.AddSettingsKeyFrame(0, emitterSettings);
				emitterSettings = new EmitterSettings(emitterSettings);
				emitterSettings.mVisibility = 1f;
				emitter.AddSettingsKeyFrame(22, emitterSettings);
				emitterSettings = new EmitterSettings(emitterSettings);
				emitter.AddSettingsKeyFrame(this.mStunTimer, emitterSettings);
				emitterSettings = new EmitterSettings(emitterSettings);
				emitterSettings.mVisibility = 0f;
				emitter.AddSettingsKeyFrame(this.mStunTimer + ZumasRevenge.Common._M(150), emitterSettings);
				ParticleType particleType = new ParticleType();
				particleType.mEmitterAttachPct = ZumasRevenge.Common._M(1f);
				particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_STAR);
				particleType.mRefXOff = ZumasRevenge.Common._M(-162);
				particleType.mRefYOff = ZumasRevenge.Common._M(-162);
				particleType.mXOff = ZumasRevenge.Common._S(ZumasRevenge.Common._M(20));
				particleType.mYOff = ZumasRevenge.Common._S(ZumasRevenge.Common._M(10));
				particleType.mColorKeyManager.AddColorKey(0f, new SexyColor(59, 167, 93));
				particleType.mColorKeyManager.AddColorKey(0.2f, new SexyColor(116, 228, 19));
				particleType.mColorKeyManager.AddColorKey(0.4f, new SexyColor(233, 233, 0));
				particleType.mColorKeyManager.AddColorKey(0.8f, new SexyColor(20, 140, 233));
				particleType.mColorKeyManager.AddColorKey(1f, new SexyColor(26, 31, 167));
				particleType.mAlphaKeyManager.AddAlphaKey(0f, 0);
				particleType.mAlphaKeyManager.AddAlphaKey(0.25f, 255);
				particleType.mAlphaKeyManager.AddAlphaKey(0.75f, 255);
				particleType.mAlphaKeyManager.AddAlphaKey(1f, 0);
				ParticleSettings particleSettings = new ParticleSettings();
				particleSettings.mLife = ZumasRevenge.Common._M(46);
				particleSettings.mNumber = ZumasRevenge.Common._M(15);
				particleSettings.mXSize = ZumasRevenge.Common._M(139);
				particleSettings.mWeight = (float)ZumasRevenge.Common._M(101);
				particleSettings.mSpin = SexyFramework.Common.DegreesToRadians((float)ZumasRevenge.Common._M(40));
				particleType.AddSettingsKeyFrame(0, particleSettings);
				particleSettings = new ParticleSettings(particleSettings);
				particleType.AddSettingsKeyFrame(this.mStunTimer + ZumasRevenge.Common._M(100), particleSettings);
				particleSettings = new ParticleSettings(particleSettings);
				particleSettings.mLife = 0;
				particleType.AddSettingsKeyFrame(this.mStunTimer + ZumasRevenge.Common._M(150), particleSettings);
				particleType.AddVarianceKeyFrame(0, new ParticleVariance
				{
					mLifeVar = ZumasRevenge.Common._M(54),
					mSizeXVar = ZumasRevenge.Common._M(9),
					mWeightVar = ZumasRevenge.Common._M(37),
					mSpinVar = SexyFramework.Common.DegreesToRadians((float)ZumasRevenge.Common._M(80))
				});
				LifetimeSettings lifetimeSettings = new LifetimeSettings();
				lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(0.4f);
				lifetimeSettings.mSpinMult = ZumasRevenge.Common._M(1.4f);
				particleType.AddSettingAtLifePct(0f, lifetimeSettings);
				lifetimeSettings = new LifetimeSettings(lifetimeSettings);
				lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(1.1f);
				lifetimeSettings.mSpinMult = ZumasRevenge.Common._M(1.19f);
				particleType.AddSettingAtLifePct(0.42f, lifetimeSettings);
				particleType.AddSettingAtLifePct(1f, new LifetimeSettings(lifetimeSettings)
				{
					mSizeXMult = ZumasRevenge.Common._M(0.4f),
					mSpinMult = ZumasRevenge.Common._M(0.8f)
				});
				emitter.AddParticleType(particleType);
				this.mDizzyStars.AddEmitter(emitter);
				return;
			}
			if (this.mBoard.mLevel.mEndSequence == 3)
			{
				this.mFlameStun = ((timer < 200) ? Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_FIREBREATHDE150).Duplicate() : Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_FIREBREATHDE250).Duplicate());
				this.mFlameStunShort = timer < 200;
				this.mFlameStun.ResetAnim();
				this.mGlobalTranform.Reset();
				GameApp.DownScaleNum(1f);
				int num4 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(400));
				int num5 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(330));
				this.mGlobalTranform.Translate((float)(-(float)num4 / 2), (float)(-(float)num5 / 2));
				this.mGlobalTranform.RotateRad(this.mAngle);
				this.mGlobalTranform.Translate((float)(num4 / 2), (float)(num5 / 2));
				this.mGlobalTranform.Translate((float)ZumasRevenge.Common._S(this.GetCenterX() + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(-70))), (float)ZumasRevenge.Common._S(this.GetCenterY() + ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(-55))));
				this.mFlameStun.SetTransform(this.mGlobalTranform.GetMatrix());
				this.mFlameStun.Play("Main");
			}
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x0007A020 File Offset: 0x00078220
		public void Poison(int timer)
		{
			if (GameApp.gApp.GetBoard().mLevel.mInvertMouseTimer > 0)
			{
				return;
			}
			Board board = GameApp.gApp.GetBoard();
			board.ForceFlipFrog();
			GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_MINDWARP2));
			board.mLevel.mInvertMouseTimer = timer;
			this.mSickAnim.Reset();
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0007A084 File Offset: 0x00078284
		public void SetSlowTimer(int t)
		{
			this.mSlowTimer = t;
			Board board = GameApp.gApp.GetBoard();
			board.ForceFlipFrog();
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x0007A0A9 File Offset: 0x000782A9
		public void AddPowerRing(OrbPowerRing p)
		{
			this.mPowerRings.Add(p);
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0007A0B7 File Offset: 0x000782B7
		public void AddPowerOrb(SkeletonPowerOrb o)
		{
			this.mPowerOrbs.Add(o);
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0007A0C8 File Offset: 0x000782C8
		public void ForceX(int x)
		{
			this.mCenterX = (this.mCurX = (float)x);
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0007A0E8 File Offset: 0x000782E8
		public void ForceY(int y)
		{
			this.mCenterY = (this.mCurY = (float)y);
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0007A108 File Offset: 0x00078308
		public bool IsFrogShowingBall()
		{
			bool result = false;
			if (this.mShowNextBall && (this.mBoard.mLevel.mBoss == null || this.mBoard.mLevel.mBoss.AllowFrogToFire()) && !this.LaserMode() && !this.LightningMode() && this.mNextBullet != null && (this.mState != GunState.GunState_Reloading || ZumasRevenge.Common.gSuckMode))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0007A174 File Offset: 0x00078374
		public void UpdateShotCorrectionTarget()
		{
			if (this.mBoard.mApp.mAutoMonkey.IsEnabled())
			{
				return;
			}
			this.mShotCorrectionTarget = new SexyVector2(0f, 0f);
			SexyVector3 sexyVector = new SexyVector3((float)this.mBoard.mFrog.GetCurX(), (float)this.mBoard.mFrog.GetCurY(), 0f);
			SexyVector3 mGuideBallPoint = this.mBoard.mGuideBallPoint;
			float num = (float)Math.Atan2((double)(sexyVector.y - mGuideBallPoint.y), (double)(mGuideBallPoint.x - sexyVector.x));
			bool flag = true;
			flag = flag && this.mBoard.mBulletList.size<Bullet>() == 0;
			flag = flag && this.mBullet != null && this.GetType() == 0 && !this.IsSlow() && this.mBoard.mLevel.mMoveType == 0;
			float num2 = 1000000f;
			if (flag && this.mBoard.mCurTreasure != null && this.mBoard.LazerHitTreasure(sexyVector, mGuideBallPoint - sexyVector, ref num2))
			{
				flag = false;
			}
			if (flag && this.mBoard.LazerHitTorch(sexyVector, mGuideBallPoint - sexyVector, ref num2, (float)this.mBullet.GetRadius()))
			{
				flag = false;
			}
			if (flag && (this.mBoard.mGuideBall == null || this.mBoard.mGuideBall.GetColorType() != this.mBullet.GetColorType()))
			{
				float num3 = this.mBoard.mApp.mShotCorrectionAngleMax * 0.01745328f;
				List<Gun.BallShotInfo> list = new List<Gun.BallShotInfo>();
				for (int i = 0; i < this.mBoard.mLevel.mNumCurves; i++)
				{
					foreach (Ball ball in this.mBoard.mLevel.mCurveMgr[i].mBallList)
					{
						if (!ball.GetInTunnel() && !ball.GetIsExploding())
						{
							if (ball.GetColorType() == this.mBullet.GetColorType())
							{
								bool flag2 = false;
								SexyVector2 vShiftedPos = ball.GetPos() + this.ShiftTargetByEverything(ball, ref flag2);
								SexyVector3 vDir = new SexyVector3(vShiftedPos.x - sexyVector.x, vShiftedPos.y - sexyVector.y, 0f);
								if (this.IsTargetWithinShotCorrectionRange(vShiftedPos, num))
								{
									if ((flag2 && (ball.GetNextBall() == null || !ball.CollidesWithPhysically(ball.GetNextBall(), 1))) || (!flag2 && (ball.GetPrevBall() == null || !ball.CollidesWithPhysically(ball.GetPrevBall(), 1))))
									{
										SexyVector2 sexyVector2 = ball.GetPos() + this.ShiftTargetTowardFrog(ball);
										SexyVector3 vDir2 = new SexyVector3(sexyVector2.x - sexyVector.x, sexyVector2.y - sexyVector.y, 0f);
										Ball ball2 = this.CheckPotentialShot(vDir2);
										if (ball2 == ball)
										{
											list.Add(new Gun.BallShotInfo(ball, vShiftedPos));
										}
									}
									else
									{
										Ball ball3 = this.CheckPotentialShot(vDir);
										if (ball3 == ball || (flag2 && ball3 == ball.GetNextBall()) || (!flag2 && ball3 == ball.GetPrevBall()))
										{
											list.Add(new Gun.BallShotInfo(ball, vShiftedPos));
										}
									}
								}
							}
							else
							{
								bool flag3 = false;
								bool bForward = true;
								Ball nextBall = ball.GetNextBall();
								while (nextBall != null && nextBall.GetIsExploding())
								{
									nextBall = nextBall.GetNextBall();
								}
								if (nextBall != null && nextBall.GetColorType() != ball.GetColorType() && !ball.CollidesWithPhysically(nextBall, 1) && nextBall.GetColorType() == this.mBullet.GetColorType())
								{
									flag3 = true;
								}
								Ball prevBall = ball.GetPrevBall();
								while (prevBall != null && prevBall.GetIsExploding())
								{
									prevBall = prevBall.GetPrevBall();
								}
								if (prevBall != null && prevBall.GetColorType() != ball.GetColorType() && !ball.CollidesWithPhysically(prevBall, 1) && prevBall.GetColorType() == this.mBullet.GetColorType())
								{
									flag3 = true;
									bForward = false;
								}
								if (flag3)
								{
									SexyVector2 vShiftedPos2 = ball.GetPos() + this.ShiftTargetTowardFrog(ball);
									SexyVector3 vDir3 = new SexyVector3(vShiftedPos2.x - sexyVector.x, vShiftedPos2.y - sexyVector.y, 0f);
									Ball ball4 = this.CheckPotentialShot(vDir3);
									if (ball4 == ball)
									{
										vShiftedPos2 = ball.GetPos() + this.ShiftTargetTowardFrog(ball) + this.ShiftTargetTowardBall(ball, bForward) + this.ShiftTargetByBallSpeed(ball);
										if (this.IsTargetWithinShotCorrectionRange(vShiftedPos2, num))
										{
											list.Add(new Gun.BallShotInfo(ball, vShiftedPos2));
										}
									}
								}
							}
						}
					}
				}
				float num4 = 4.14159f;
				for (int j = 0; j < list.size<Gun.BallShotInfo>(); j++)
				{
					Ball mBall = list[j].mBall;
					SexyVector2 mShiftedPos = list[j].mShiftedPos;
					float num5 = (float)Math.Atan2((double)(sexyVector.y - mShiftedPos.y), (double)(mShiftedPos.x - sexyVector.x));
					num5 -= num;
					if (num5 < -3.14159f)
					{
						num5 += 6.28318f;
					}
					else if (num5 > 3.14159f)
					{
						num5 -= 6.28318f;
					}
					if (Math.Abs(num5) < num4)
					{
						bool flag4 = false;
						SexyVector2 sexyVector3 = mBall.GetPos() + this.ShiftTargetByEverything(mBall, ref flag4);
						SexyVector3 vDir4 = new SexyVector3(sexyVector3.x - sexyVector.x, sexyVector3.y - sexyVector.y, 0f);
						Ball ball5 = this.CheckPotentialShot(vDir4, true);
						if (ball5 == mBall || (ball5 != null && ((ball5.GetNextBall() != null && ball5.GetNextBall() == mBall) || (ball5.GetPrevBall() != null && ball5.GetPrevBall() == mBall))))
						{
							this.mShotCorrectionTarget = mShiftedPos;
							num4 = Math.Abs(num5);
						}
					}
				}
				if (this.mShotCorrectionTarget.x != 0f && this.mShotCorrectionTarget.y != 0f)
				{
					this.mShotCorrectionRad = (float)Math.Atan2((double)(sexyVector.y - this.mShotCorrectionTarget.y), (double)(this.mShotCorrectionTarget.x - sexyVector.x));
				}
			}
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0007A7C8 File Offset: 0x000789C8
		public void UpdateAutoMonkeyShotCorrection()
		{
			this.mShotCorrectionTarget = new SexyVector2(0f, 0f);
			SexyVector3 impliedObject = new SexyVector3((float)this.mBoard.mFrog.GetCurX(), (float)this.mBoard.mFrog.GetCurY(), 0f);
			bool flag = true;
			if (this.mBoard.mCurTreasure != null && !this.mBoard.mTreasureWasHit)
			{
				this.CheckMonkeyShot((float)this.mBoard.mCurTreasure.x, (float)this.mBoard.mCurTreasure.y, ref flag);
				SexyVector3 impliedObject2 = new SexyVector3((float)this.mBoard.mCurTreasure.x - impliedObject.x, (float)this.mBoard.mCurTreasure.y - impliedObject.y, 0f);
				float num = impliedObject2.Magnitude();
				impliedObject2 = impliedObject2.Normalize();
				float num2 = 0f;
				while (num2 < num && !flag)
				{
					SexyVector3 sexyVector = impliedObject + impliedObject2 * num2;
					if (this.mBoard.mLevel.PointIntersectsWall(sexyVector.x, sexyVector.y))
					{
						flag = true;
						this.mShotCorrectionTarget = new SexyVector2(0f, 0f);
					}
					num2 += ZumasRevenge.Common._M(5f);
				}
			}
			foreach (Torch torch in this.mBoard.mLevel.mTorches)
			{
				if (!torch.mWasHit)
				{
					this.CheckMonkeyShot((float)torch.mX, (float)torch.mY, ref flag);
					if (!flag)
					{
						break;
					}
				}
			}
			if (this.mBoard.mLevel.mBoss != null)
			{
				foreach (Tiki tiki in this.mBoard.mLevel.mBoss.mTikis)
				{
					if (!tiki.mWasHit)
					{
						this.CheckMonkeyShot(tiki.mX, tiki.mY, ref flag);
						if (!flag)
						{
							break;
						}
					}
				}
				if (flag && !this.mBoard.mLevel.mBoss.mEatsBalls)
				{
					this.CheckMonkeyShot((float)this.mBoard.mLevel.mBoss.GetX(), (float)this.mBoard.mLevel.mBoss.GetY(), ref flag);
				}
			}
			int num3 = 0;
			int num4 = 0;
			bool flag2 = false;
			Ball ball = null;
			Ball ball2 = null;
			SexyVector2 sexyVector2 = new SexyVector2(0f, 0f);
			SexyVector2 sexyVector3 = new SexyVector2(0f, 0f);
			int num5 = 0;
			while (num5 < this.mBoard.mLevel.mNumCurves && flag)
			{
				foreach (Ball ball3 in this.mBoard.mLevel.mCurveMgr[num5].mBallList)
				{
					if (ball3 != null && this.mBullet != null && !ball3.GetIsExploding() && ball3.GetColorType() == this.mBullet.GetColorType())
					{
						num3++;
						Ball nextBall = ball3.GetNextBall();
						while (nextBall != null && nextBall.GetIsExploding())
						{
							nextBall = nextBall.GetNextBall();
						}
						if (nextBall != null && nextBall.GetColorType() == ball3.GetColorType() && !ball3.CollidesWithPhysically(nextBall, 1))
						{
							flag2 = true;
						}
						Ball prevBall = ball3.GetPrevBall();
						while (prevBall != null && prevBall.GetIsExploding())
						{
							prevBall = prevBall.GetPrevBall();
						}
						if (prevBall != null && prevBall.GetColorType() == ball3.GetColorType() && !ball3.CollidesWithPhysically(prevBall, 1))
						{
							flag2 = true;
						}
						if (!flag2)
						{
							SexyVector2 sexyVector4 = ball3.GetPos() + this.ShiftTargetTowardFrog(ball3);
							SexyVector3 vDir = new SexyVector3(sexyVector4.x - impliedObject.x, sexyVector4.y - impliedObject.y, 0f);
							Ball ball4 = this.CheckPotentialShot(vDir);
							if (ball3 == ball4 && ball2 == null && !ball3.GetInTunnel())
							{
								bool flag3 = false;
								sexyVector3 = ball3.GetPos() + this.ShiftTargetByEverything(ball3, ref flag3);
								ball2 = ball3;
							}
						}
					}
					else
					{
						if (!flag2 && num3 > num4 && ball2 != null)
						{
							sexyVector2 = sexyVector3;
							num4 = num3;
							ball = ball2;
						}
						num3 = 0;
						ball2 = null;
						sexyVector3 = new SexyVector2(0f, 0f);
						flag2 = false;
					}
				}
				if (num3 > num4 && ball2 != null)
				{
					sexyVector2 = sexyVector3;
					num4 = num3;
					ball = ball2;
				}
				num5++;
			}
			if (ball != null)
			{
				this.mShotCorrectionTarget = sexyVector2;
			}
			if (this.mShotCorrectionTarget != SexyVector2.Zero)
			{
				this.mShotCorrectionRad = (float)Math.Atan2((double)(impliedObject.y - this.mShotCorrectionTarget.y), (double)(this.mShotCorrectionTarget.x - impliedObject.x));
			}
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0007AD10 File Offset: 0x00078F10
		public void CheckMonkeyShot(float x, float y, ref bool bDoNormalShot)
		{
			SexyVector3 sexyVector = new SexyVector3((float)this.mBoard.mFrog.GetCurX(), (float)this.mBoard.mFrog.GetCurY(), 0f);
			SexyVector2 sexyVector2 = new SexyVector2(x - sexyVector.x, y - sexyVector.y);
			Ball ball = this.CheckPotentialShot(new SexyVector3(sexyVector2.x, sexyVector2.y, 0f));
			SexyVector2 sexyVector3 = new SexyVector2(0f, 0f);
			if (ball != null)
			{
				sexyVector3 = new SexyVector2(ball.GetX() - sexyVector.x, ball.GetY() - sexyVector.y);
			}
			if (ball == null || sexyVector2.MagnitudeSquared() < sexyVector3.MagnitudeSquared())
			{
				this.mShotCorrectionTarget.x = x;
				this.mShotCorrectionTarget.y = y;
				bDoNormalShot = false;
			}
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0007ADE8 File Offset: 0x00078FE8
		public Ball CheckPotentialShot(SexyVector3 vDir, bool bWidthCheck)
		{
			SexyVector3 sexyVector = new SexyVector3((float)this.mBoard.mFrog.GetCurX(), (float)this.mBoard.mFrog.GetCurY(), 0f);
			SexyVector3 sexyVector2 = vDir.Normalize();
			SexyVector3 impliedObject = new SexyVector3(sexyVector2.y, sexyVector2.x, sexyVector2.z);
			Ball ball = null;
			float num = 1000000f;
			float num2 = num;
			float num3 = num;
			for (int i = 0; i < this.mBoard.mLevel.mNumCurves; i++)
			{
				Ball ball2 = this.mBoard.mLevel.mCurveMgr[i].CheckBallIntersection(sexyVector, sexyVector2, ref num, true);
				if (ball2 != null)
				{
					Ball prevBall = ball2.GetPrevBall();
					Ball nextBall = ball2.GetNextBall();
					if (bWidthCheck)
					{
						Ball ball3 = this.mBoard.mLevel.mCurveMgr[i].CheckBallIntersection(sexyVector - impliedObject * ((float)ball2.GetRadius() + 1f), sexyVector2, ref num2, true);
						Ball ball4 = this.mBoard.mLevel.mCurveMgr[i].CheckBallIntersection(sexyVector + impliedObject * ((float)ball2.GetRadius() + 1f), sexyVector2, ref num3, true);
						if ((ball3 == ball2 || ball3 == prevBall || ball3 == nextBall || num < num2) && (ball4 == ball2 || ball4 == prevBall || ball4 == nextBall || num < num3))
						{
							ball = ball2;
						}
					}
					else
					{
						ball = ball2;
					}
				}
			}
			float num4 = 0f;
			while (num4 < num && ball != null)
			{
				SexyVector3 sexyVector3 = sexyVector + sexyVector2 * num4;
				if (this.mBoard.mLevel.PointIntersectsWall(sexyVector3.x, sexyVector3.y))
				{
					ball = null;
				}
				if (bWidthCheck)
				{
					SexyVector3 sexyVector4 = sexyVector - impliedObject * (float)ball.GetRadius() + sexyVector2 * num4;
					if (this.mBoard.mLevel.PointIntersectsWall(sexyVector4.x, sexyVector4.y))
					{
						ball = null;
					}
					SexyVector3 sexyVector5 = sexyVector + impliedObject * (float)ball.GetRadius() + sexyVector2 * num4;
					if (this.mBoard.mLevel.PointIntersectsWall(sexyVector5.x, sexyVector5.y))
					{
						ball = null;
					}
				}
				num4 += ZumasRevenge.Common._M(5f);
			}
			return ball;
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0007B04B File Offset: 0x0007924B
		public Ball CheckPotentialShot(SexyVector3 vDir)
		{
			return this.CheckPotentialShot(vDir, false);
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0007B058 File Offset: 0x00079258
		public bool IsTargetWithinShotCorrectionRange(SexyVector2 vShiftedPos, float targetRad)
		{
			SexyVector2 impliedObject = new SexyVector2((float)this.mBoard.mFrog.GetCurX(), (float)this.mBoard.mFrog.GetCurY());
			SexyVector2 v = new SexyVector2(this.mBoard.mGuideBallPoint.x, this.mBoard.mGuideBallPoint.y);
			bool flag = (impliedObject - v).Magnitude() < this.mBoard.mApp.mShotCorrectionAngleToWidthDist;
			if (flag)
			{
				float num = (float)Math.Atan2((double)(impliedObject.y - vShiftedPos.y), (double)(vShiftedPos.x - impliedObject.x));
				num -= targetRad;
				if (num < -3.14159f)
				{
					num += 6.28318f;
				}
				else if (num > 3.14159f)
				{
					num -= 6.28318f;
				}
				float num2 = this.mBoard.mApp.mShotCorrectionAngleMax * 0.01745328f;
				return Math.Abs(num) <= num2;
			}
			float mShotCorrectionWidthMax = this.mBoard.mApp.mShotCorrectionWidthMax;
			return (vShiftedPos - v).Magnitude() <= mShotCorrectionWidthMax * mShotCorrectionWidthMax;
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0007B17C File Offset: 0x0007937C
		public SexyVector2 ShiftTargetTowardFrog(Ball aBall)
		{
			SexyVector2 v = new SexyVector2((float)this.mBoard.mFrog.GetCurX(), (float)this.mBoard.mFrog.GetCurY());
			SexyVector2 sexyVector = default(SexyVector2);
			SexyVector2 sexyVector2 = default(SexyVector2);
			SexyVector2 sexyVector3 = default(SexyVector2);
			aBall.GetCurve().GetXYFromWaypoint((int)aBall.GetWayPoint(), out sexyVector.mVector.X, out sexyVector.mVector.Y);
			aBall.GetCurve().GetXYFromWaypoint((int)aBall.GetWayPoint() - 1, out sexyVector2.mVector.X, out sexyVector2.mVector.Y);
			sexyVector3 = sexyVector - sexyVector2;
			sexyVector3.Normalize();
			SexyVector2 impliedObject = new SexyVector2(-sexyVector3.y, sexyVector3.x);
			sexyVector = impliedObject * (float)aBall.GetRadius();
			sexyVector2 = -impliedObject * (float)aBall.GetRadius();
			if ((aBall.GetPos() + sexyVector - v).Magnitude() < (aBall.GetPos() + sexyVector2 - v).Magnitude())
			{
				return sexyVector;
			}
			return sexyVector2;
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0007B2A8 File Offset: 0x000794A8
		public SexyVector2 ShiftTargetTowardGuide(Ball aBall, ref bool bShiftedInTrackDir)
		{
			SexyVector2 v = new SexyVector2((float)this.mBoard.mFrog.GetCurX(), (float)this.mBoard.mFrog.GetCurY());
			SexyVector2 sexyVector = default(SexyVector2);
			SexyVector2 sexyVector2 = default(SexyVector2);
			SexyVector2 impliedObject = default(SexyVector2);
			aBall.GetCurve().GetXYFromWaypoint((int)aBall.GetWayPoint(), out sexyVector.mVector.X, out sexyVector.mVector.Y);
			aBall.GetCurve().GetXYFromWaypoint((int)aBall.GetWayPoint() - 1, out sexyVector2.mVector.X, out sexyVector2.mVector.Y);
			impliedObject = sexyVector - sexyVector2;
			impliedObject.Normalize();
			sexyVector = impliedObject * (float)aBall.GetRadius();
			sexyVector2 = -impliedObject * (float)aBall.GetRadius();
			SexyVector2 impliedObject2 = new SexyVector2(this.mBoard.mGuideBallPoint.x, this.mBoard.mGuideBallPoint.y);
			(aBall.GetPos() + sexyVector - v).Normalize();
			(impliedObject2 - v).Normalize();
			(aBall.GetPos() + sexyVector2 - v).Normalize();
			float num = (aBall.GetPos() + sexyVector - v).Normalize().Dot((impliedObject2 - v).Normalize());
			float num2 = (aBall.GetPos() + sexyVector2 - v).Normalize().Dot((impliedObject2 - v).Normalize());
			if (Math.Abs(num) > Math.Abs(num2))
			{
				bShiftedInTrackDir = true;
				return sexyVector;
			}
			bShiftedInTrackDir = false;
			return sexyVector2;
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0007B478 File Offset: 0x00079678
		public SexyVector2 ShiftTargetTowardBall(Ball aBall, bool bForward)
		{
			SexyVector2 impliedObject = default(SexyVector2);
			SexyVector2 v = default(SexyVector2);
			SexyVector2 impliedObject2 = default(SexyVector2);
			aBall.GetCurve().GetXYFromWaypoint((int)aBall.GetWayPoint(), out impliedObject.mVector.X, out impliedObject.mVector.Y);
			aBall.GetCurve().GetXYFromWaypoint((int)aBall.GetWayPoint() - 1, out v.mVector.X, out v.mVector.Y);
			impliedObject2 = impliedObject - v;
			impliedObject2.Normalize();
			if (bForward)
			{
				return impliedObject2 * (float)aBall.GetRadius();
			}
			return -impliedObject2 * (float)aBall.GetRadius();
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0007B528 File Offset: 0x00079728
		public SexyVector2 ShiftTargetByBallSpeed(Ball aBall)
		{
			SexyVector2 v = new SexyVector2((float)this.mBoard.mFrog.GetCurX(), (float)this.mBoard.mFrog.GetCurY());
			float num = (aBall.GetPos() - v).Magnitude();
			float fireSpeed = this.GetFireSpeed();
			float num2 = num / fireSpeed;
			if (aBall.GetWayPointProgress() != 0f)
			{
				float num3 = aBall.GetWayPoint() + aBall.GetWayPointProgress() * num2;
				SexyVector2 impliedObject = new SexyVector2(0f, 0f);
				SexyVector2 v2 = new SexyVector2(0f, 0f);
				aBall.GetCurve().GetXYFromWaypoint((int)aBall.GetWayPoint(), out v2.mVector.X, out v2.mVector.Y);
				aBall.GetCurve().GetXYFromWaypoint((int)num3, out impliedObject.mVector.X, out impliedObject.mVector.Y);
				return impliedObject - v2;
			}
			return new SexyVector2(0f, 0f);
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0007B62E File Offset: 0x0007982E
		public SexyVector2 ShiftTargetByEverything(Ball aBall, ref bool bShiftedInTrackDir)
		{
			return this.ShiftTargetTowardFrog(aBall) + this.ShiftTargetTowardGuide(aBall, ref bShiftedInTrackDir) + this.ShiftTargetByBallSpeed(aBall);
		}

		// Token: 0x04000AD3 RID: 2771
		public static int TONGUE_Y1 = 50;

		// Token: 0x04000AD4 RID: 2772
		public static int TONGUE_Y2 = 57;

		// Token: 0x04000AD5 RID: 2773
		public static int TONGUE_YNOBALL = 60;

		// Token: 0x04000AD6 RID: 2774
		public static int FROG_WIDTH = 147;

		// Token: 0x04000AD7 RID: 2775
		public static int FROG_HEIGHT = 134;

		// Token: 0x04000AD8 RID: 2776
		protected static int NUM_CANNON_SHADOWS = 5;

		// Token: 0x04000AD9 RID: 2777
		protected static float aChevronSpeed = 0f;

		// Token: 0x04000ADA RID: 2778
		protected Transform mGlobalTranform = new Transform();

		// Token: 0x04000ADB RID: 2779
		private FPoint[] mVels = new FPoint[]
		{
			new FPoint(0f, 0f),
			new FPoint(0f, 0f),
			new FPoint(0f, 0f)
		};

		// Token: 0x04000ADC RID: 2780
		private CumulativeTransform mCumTran = new CumulativeTransform();

		// Token: 0x04000ADD RID: 2781
		private SexyPoint mCP = new SexyPoint();

		// Token: 0x04000ADE RID: 2782
		private SexyPoint mGp = new SexyPoint();

		// Token: 0x04000ADF RID: 2783
		public BonusText mTempText = new BonusText();

		// Token: 0x04000AE0 RID: 2784
		public List<LTSmokeParticle> mSmokeParticles = new List<LTSmokeParticle>();

		// Token: 0x04000AE1 RID: 2785
		public PIEffect mCannonBlast;

		// Token: 0x04000AE2 RID: 2786
		public Board mBoard;

		// Token: 0x04000AE3 RID: 2787
		protected PopAnim mDarkFrogStun;

		// Token: 0x04000AE4 RID: 2788
		protected bool mDarkFrogStunShort;

		// Token: 0x04000AE5 RID: 2789
		protected PopAnim mFlameStun;

		// Token: 0x04000AE6 RID: 2790
		protected bool mFlameStunShort;

		// Token: 0x04000AE7 RID: 2791
		private SexyFramework.PIL.System mDizzyStars;

		// Token: 0x04000AE8 RID: 2792
		private Composition mSickAnim;

		// Token: 0x04000AE9 RID: 2793
		protected List<BeamComponent>[] mBeams = new List<BeamComponent>[]
		{
			new List<BeamComponent>(),
			new List<BeamComponent>(),
			new List<BeamComponent>()
		};

		// Token: 0x04000AEA RID: 2794
		protected BeamComponent mElectricOrb = new BeamComponent();

		// Token: 0x04000AEB RID: 2795
		protected Bullet mBullet;

		// Token: 0x04000AEC RID: 2796
		protected Bullet mNextBullet;

		// Token: 0x04000AED RID: 2797
		protected GunState mState;

		// Token: 0x04000AEE RID: 2798
		protected DeviceImage mLazer;

		// Token: 0x04000AEF RID: 2799
		protected List<Bullet> mCannonBullets = new List<Bullet>();

		// Token: 0x04000AF0 RID: 2800
		protected List<Component> mLazerPulse = new List<Component>();

		// Token: 0x04000AF1 RID: 2801
		protected List<ConfusionMark> mConfusionMarks = new List<ConfusionMark>();

		// Token: 0x04000AF2 RID: 2802
		protected List<Bubble> mBubbles = new List<Bubble>();

		// Token: 0x04000AF3 RID: 2803
		protected List<OrbPowerRing> mPowerRings = new List<OrbPowerRing>();

		// Token: 0x04000AF4 RID: 2804
		protected List<SkeletonPowerOrb> mPowerOrbs = new List<SkeletonPowerOrb>();

		// Token: 0x04000AF5 RID: 2805
		public PopAnim mLightningEffect;

		// Token: 0x04000AF6 RID: 2806
		public int mBlinkCount;

		// Token: 0x04000AF7 RID: 2807
		public int mBlinkTimer;

		// Token: 0x04000AF8 RID: 2808
		public int[] mCannonBallShadows = new int[Gun.NUM_CANNON_SHADOWS];

		// Token: 0x04000AF9 RID: 2809
		public int mCannonBallShadowPos;

		// Token: 0x04000AFA RID: 2810
		public int mBX;

		// Token: 0x04000AFB RID: 2811
		public int mBY;

		// Token: 0x04000AFC RID: 2812
		public int mDestX1;

		// Token: 0x04000AFD RID: 2813
		public int mDestY1;

		// Token: 0x04000AFE RID: 2814
		public int mDestX2;

		// Token: 0x04000AFF RID: 2815
		public int mDestY2;

		// Token: 0x04000B00 RID: 2816
		public int mDestTime;

		// Token: 0x04000B01 RID: 2817
		public int mDestCount;

		// Token: 0x04000B02 RID: 2818
		public float mCenterX;

		// Token: 0x04000B03 RID: 2819
		public float mCenterY;

		// Token: 0x04000B04 RID: 2820
		public float mCurX;

		// Token: 0x04000B05 RID: 2821
		public float mCurY;

		// Token: 0x04000B06 RID: 2822
		public float mSpitX;

		// Token: 0x04000B07 RID: 2823
		public float mSpitY;

		// Token: 0x04000B08 RID: 2824
		public float mSpitAlpha;

		// Token: 0x04000B09 RID: 2825
		public float mSpitVX;

		// Token: 0x04000B0A RID: 2826
		public float mSpitVY;

		// Token: 0x04000B0B RID: 2827
		public float mSpitAngle;

		// Token: 0x04000B0C RID: 2828
		public float[] mLazerFrogBackPulseAlpha = new float[4];

		// Token: 0x04000B0D RID: 2829
		public int mWidth;

		// Token: 0x04000B0E RID: 2830
		public int mHeight;

		// Token: 0x04000B0F RID: 2831
		public int mCannonCount;

		// Token: 0x04000B10 RID: 2832
		public int mLazerCount;

		// Token: 0x04000B11 RID: 2833
		public int mUpdateCount;

		// Token: 0x04000B12 RID: 2834
		public int mLastGuideX;

		// Token: 0x04000B13 RID: 2835
		public int mLastGuideY;

		// Token: 0x04000B14 RID: 2836
		public int mStunTimer;

		// Token: 0x04000B15 RID: 2837
		public int mShieldAnimCel;

		// Token: 0x04000B16 RID: 2838
		public int mSlowTimer;

		// Token: 0x04000B17 RID: 2839
		public int mStunSpinFrame;

		// Token: 0x04000B18 RID: 2840
		public int mStartingStunTime;

		// Token: 0x04000B19 RID: 2841
		public float mBossStateAlpha;

		// Token: 0x04000B1A RID: 2842
		public int mBossStateAlphaDir;

		// Token: 0x04000B1B RID: 2843
		public int mBossStateHoldTimer;

		// Token: 0x04000B1C RID: 2844
		public int mBossState;

		// Token: 0x04000B1D RID: 2845
		public float mAngle;

		// Token: 0x04000B1E RID: 2846
		public float mDestAngle;

		// Token: 0x04000B1F RID: 2847
		public float mStatePercent;

		// Token: 0x04000B20 RID: 2848
		public float mFireVel;

		// Token: 0x04000B21 RID: 2849
		public float mRecoilAmt;

		// Token: 0x04000B22 RID: 2850
		public float mLazerPercent;

		// Token: 0x04000B23 RID: 2851
		public float mBeamProjectedEndX;

		// Token: 0x04000B24 RID: 2852
		public float mBeamProjectedEndY;

		// Token: 0x04000B25 RID: 2853
		public float mBeamDistToTarget;

		// Token: 0x04000B26 RID: 2854
		public float mFarthestDistance;

		// Token: 0x04000B27 RID: 2855
		public float mCannonAngle;

		// Token: 0x04000B28 RID: 2856
		public float mBossDeathTX;

		// Token: 0x04000B29 RID: 2857
		public float mBossDeathTY;

		// Token: 0x04000B2A RID: 2858
		public float mBossDeathVX;

		// Token: 0x04000B2B RID: 2859
		public float mBossDeathVY;

		// Token: 0x04000B2C RID: 2860
		public bool mShowNextBall;

		// Token: 0x04000B2D RID: 2861
		public bool mDoingHop;

		// Token: 0x04000B2E RID: 2862
		public bool mDoElectricBeamShit;

		// Token: 0x04000B2F RID: 2863
		public bool mDoingCannonBlast;

		// Token: 0x04000B30 RID: 2864
		public List<FrogBody> mFrogStack = new List<FrogBody>();

		// Token: 0x04000B31 RID: 2865
		public FrogBody mCurrentBody = new FrogBody();

		// Token: 0x04000B32 RID: 2866
		public int mReloadPoint;

		// Token: 0x04000B33 RID: 2867
		public int mFirePoint;

		// Token: 0x04000B34 RID: 2868
		public int mBallPoint;

		// Token: 0x04000B35 RID: 2869
		public int mBallXOff;

		// Token: 0x04000B36 RID: 2870
		public int mBallYOff;

		// Token: 0x04000B37 RID: 2871
		public int mCannonRuneColor;

		// Token: 0x04000B38 RID: 2872
		public int mCannonRuneAlpha;

		// Token: 0x04000B39 RID: 2873
		public int mCannonLightness;

		// Token: 0x04000B3A RID: 2874
		public int mCannonState;

		// Token: 0x04000B3B RID: 2875
		public SexyVector2 mShotCorrectionTarget = default(SexyVector2);

		// Token: 0x04000B3C RID: 2876
		public float mShotCorrectionRad;

		// Token: 0x020000E8 RID: 232
		public enum BossState
		{
			// Token: 0x04000B3E RID: 2878
			Inked,
			// Token: 0x04000B3F RID: 2879
			Plagued
		}

		// Token: 0x020000E9 RID: 233
		public class BallShotInfo
		{
			// Token: 0x06000CC0 RID: 3264 RVA: 0x0007B68B File Offset: 0x0007988B
			public BallShotInfo(Ball ball, SexyVector2 vShiftedPos)
			{
				this.mBall = ball;
				this.mShiftedPos = vShiftedPos;
			}

			// Token: 0x04000B40 RID: 2880
			public Ball mBall;

			// Token: 0x04000B41 RID: 2881
			public SexyVector2 mShiftedPos;
		}
	}
}
