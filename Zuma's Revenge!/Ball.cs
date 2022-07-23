using System;
using System.Collections.Generic;
using System.Linq;
using JeffLib;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x02000011 RID: 17
	public class Ball
	{
		// Token: 0x060003A9 RID: 937 RVA: 0x00009D88 File Offset: 0x00007F88
		public void DrawStandardPower(Graphics g, int img_id, int cel, int thePowerType)
		{
			GameApp gApp = GameApp.gApp;
			ResID id = (ResID)(img_id + this.mColorType);
			if (gApp.mColorblind && this.mColorType == 3)
			{
				id = ResID.IMAGE_POWERUPS_GREEN_CBM;
			}
			else if (gApp.mColorblind && this.mColorType == 4)
			{
				id = ResID.IMAGE_POWERUPS_PURPLE_CBM;
			}
			else if (gApp.mColorblind && this.mColorType == 5)
			{
				id = ResID.IMAGE_POWERUPS_WHITE_CBM;
			}
			Image imageByID = Res.GetImageByID(id);
			Image imageByID2;
			if (this.mPowerType == PowerType.PowerType_MoveBackwards)
			{
				imageByID2 = Res.GetImageByID(ResID.IMAGE_POWERUP_REVERSE_ANYCOLOR);
			}
			else if (this.mPowerType == PowerType.PowerType_Laser)
			{
				imageByID2 = Res.GetImageByID(ResID.IMAGE_POWERUP_LAZER_ANYCOLOR);
			}
			else
			{
				imageByID2 = Res.GetImageByID(ResID.IMAGE_POWERUPS_PULSES);
			}
			float num = ZumasRevenge.Common._S(this.mX) - (float)(imageByID.GetCelWidth() / 2);
			float num2 = ZumasRevenge.Common._S(this.mY) - (float)(imageByID.GetCelHeight() / 2);
			float num3 = ((this.mPowerType == PowerType.PowerType_MoveBackwards) ? 1.570795f : (-1.570795f));
			bool flag = gApp.Is3DAccelerated();
			if (flag)
			{
				Rect celRect = imageByID.GetCelRect(cel);
				g.DrawImageRotatedF(imageByID, (float)((int)num), (float)((int)num2), (double)(this.mRotation + num3), celRect);
			}
			else
			{
				BlendedImage blendedImage = Ball.CreateBlendedPowerup(thePowerType, this.mColorType, imageByID, cel);
				blendedImage.Draw(g, num, num2);
			}
			if (this.mPowerType == PowerType.PowerType_MoveBackwards || this.mPowerType == PowerType.PowerType_Laser)
			{
				g.SetColorizeImages(true);
				g.SetDrawMode(1);
				g.SetColor(new SexyColor(ZumasRevenge.Common.gBrightBallColors[this.mColorType]));
				float num4 = (float)imageByID2.GetCelWidth() / 2f;
				float num5 = (float)imageByID2.GetCelHeight() / 2f;
				num = ZumasRevenge.Common._S(this.mX) - num4;
				num2 = ZumasRevenge.Common._S(this.mY) - num5;
				Rect celRect2 = imageByID2.GetCelRect(this.mCel);
				if (flag)
				{
					g.DrawImageRotatedF(imageByID2, num, num2 - (float)ZumasRevenge.Common._M(0), (double)(this.mRotation + num3), num4, num5 + (float)ZumasRevenge.Common._M1(0), celRect2);
				}
				g.SetDrawMode(0);
				g.SetColorizeImages(false);
				return;
			}
			if (this.mPulseState < 2)
			{
				g.SetColorizeImages(true);
				int mAlpha = 255 - this.mPulseTimer * ((this.mPulseState == 0) ? ZumasRevenge.Common._M(4) : ZumasRevenge.Common._M1(2));
				SexyColor color = new SexyColor(ZumasRevenge.Common.gBrightBallColors[this.mColorType]);
				if (gApp.mColorblind)
				{
					color = new SexyColor(SexyColor.White);
				}
				color.mAlpha = mAlpha;
				g.SetColor(color);
				g.SetDrawMode(1);
				float num6 = (float)imageByID2.GetCelWidth() / 2f;
				float num7 = (float)imageByID2.GetCelHeight() / 2f;
				num = ZumasRevenge.Common._S(this.mX) - num6;
				num2 = ZumasRevenge.Common._S(this.mY) - num7;
				Rect celRect3 = imageByID2.GetCelRect(cel);
				if (flag)
				{
					g.DrawImageRotatedF(imageByID2, num, num2 - (float)ZumasRevenge.Common._M(0), (double)(this.mRotation + num3), num6, num7 + (float)ZumasRevenge.Common._M1(0), celRect3);
				}
				g.SetDrawMode(0);
				g.SetColorizeImages(false);
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000A084 File Offset: 0x00008284
		public void DrawNewPower(Graphics g, char theLetter, int xoff, int yoff)
		{
			GameApp gApp = GameApp.gApp;
			bool flag = gApp.Is3DAccelerated();
			Image imageByID = Res.GetImageByID(ResID.IMAGE_BALL);
			float num = ZumasRevenge.Common._S(this.mX + (float)xoff) - (float)(imageByID.mWidth / 2);
			float num2 = ZumasRevenge.Common._S(this.mY + (float)yoff) - (float)(imageByID.mHeight / 2);
			g.SetColorizeImages(true);
			g.SetColor(new SexyColor(ZumasRevenge.Common.gBallColors[this.mColorType]));
			if (MathUtils._eq(this.mRadius, (float)ZumasRevenge.Common.GetDefaultBallRadius()))
			{
				if (flag)
				{
					g.DrawImageF(imageByID, num, num2);
				}
				else
				{
					g.DrawImage(imageByID, (int)num, (int)num2);
				}
			}
			else
			{
				this.mGlobalTransform.Reset();
				float num3 = this.mRadius / (float)ZumasRevenge.Common.GetDefaultBallRadius();
				this.mGlobalTransform.Scale(num3, num3);
				num = this.mX + (float)xoff;
				num2 = this.mY + (float)yoff;
				if (flag)
				{
					g.DrawImageTransformF(imageByID, this.mGlobalTransform, num, num2);
				}
				else
				{
					g.DrawImageTransform(imageByID, this.mGlobalTransform, num, num2);
				}
			}
			g.SetColorizeImages(false);
			g.SetColor(new SexyColor(ZumasRevenge.Common._M(16777215)));
			g.SetFont(Res.GetFontByID(ResID.FONT_MAIN22));
			string theString = theLetter.ToString();
			g.DrawString(theString, (int)(ZumasRevenge.Common._S(this.mX + (float)xoff) - (float)(g.GetFont().CharWidth(theLetter) / 2)), (int)(ZumasRevenge.Common._S(this.mY + (float)yoff) - (float)(g.GetFont().GetHeight() / 2) + (float)g.GetFont().GetAscent()));
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000A213 File Offset: 0x00008413
		public void DrawNewPower(Graphics g, char theLetter)
		{
			this.DrawNewPower(g, theLetter, 0, 0);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000A220 File Offset: 0x00008420
		public void DrawPower(Graphics g)
		{
			PowerType thePowerType = this.mPowerType;
			switch (thePowerType)
			{
			case PowerType.PowerType_ProximityBomb:
				this.DrawStandardPower(g, 870, 3, (int)thePowerType);
				return;
			case PowerType.PowerType_SlowDown:
				this.DrawStandardPower(g, 870, 2, (int)thePowerType);
				return;
			case PowerType.PowerType_Accuracy:
				this.DrawStandardPower(g, 870, 0, (int)thePowerType);
				return;
			case PowerType.PowerType_MoveBackwards:
				this.DrawStandardPower(g, 870, 4, (int)thePowerType);
				return;
			case PowerType.PowerType_Lob:
			case PowerType.PowerType_BombBullet:
			case PowerType.PowerType_BallEater:
			case PowerType.PowerType_Fireball:
			case PowerType.PowerType_ShieldFrog:
			case PowerType.PowerType_FreezeBoss:
				break;
			case PowerType.PowerType_Cannon:
				this.DrawStandardPower(g, 870, 5, (int)thePowerType);
				return;
			case PowerType.PowerType_ColorNuke:
				this.DrawStandardPower(g, 870, 1, (int)thePowerType);
				return;
			case PowerType.PowerType_Laser:
				this.DrawStandardPower(g, 870, 6, (int)thePowerType);
				return;
			case PowerType.PowerType_GauntletMultBall:
				this.DrawMultPowerup(g);
				break;
			default:
				return;
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000A2E5 File Offset: 0x000084E5
		public void DrawExplosion(Graphics g)
		{
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000A2E8 File Offset: 0x000084E8
		protected void DoDrawBase(Graphics g, int xoff, int yoff)
		{
			if (this.mPowerType != PowerType.PowerType_Max)
			{
				this.DrawPower(g);
				return;
			}
			int num = ((GameApp.gApp.GetBoard().GetHallucinateTimer() > 0) ? this.mDisplayType : this.mColorType);
			ResID id = ResID.IMAGE_BLUE_BALL + num;
			if (GameApp.gApp.mColorblind && this.mColorType == 3)
			{
				id = ResID.IMAGE_GREEN_BALL_CBM;
			}
			else if (GameApp.gApp.mColorblind && this.mColorType == 4)
			{
				id = ResID.IMAGE_PURPLE_BALL_CBM;
			}
			Image imageByID = Res.GetImageByID(id);
			float x = ZumasRevenge.Common._S(this.mX + (float)xoff - this.mRadius);
			float y = ZumasRevenge.Common._S(this.mY + (float)yoff - this.mRadius);
			int frame = this.GetFrame(imageByID);
			this.mLastFrame = frame;
			if (GameApp.gApp.Is3DAccelerated())
			{
				Rect celRect = imageByID.GetCelRect(frame);
				this.mGlobalTransform.Reset();
				this.mGlobalTransform.RotateRad(this.mRotation);
				if (this.mDrawScale != 1f)
				{
					this.mGlobalTransform.Scale(this.mDrawScale, this.mDrawScale);
				}
				g.DrawImageTransformF(imageByID, this.mGlobalTransform, celRect, ZumasRevenge.Common._S(this.mX + (float)xoff), ZumasRevenge.Common._S(this.mY + (float)yoff));
				return;
			}
			BlendedImage blendedImage = Ball.CreateBlendedBall(num);
			blendedImage.Draw(g, x, y);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000A444 File Offset: 0x00008644
		protected void DoDrawAdditive(Graphics g, int xoff, int yoff)
		{
			if (this.mPowerType != PowerType.PowerType_Max)
			{
				return;
			}
			int num = ((GameApp.gApp.GetBoard().GetHallucinateTimer() > 0) ? this.mDisplayType : this.mColorType);
			ResID id = ResID.IMAGE_BLUE_BALL + num;
			if (GameApp.gApp.mColorblind && this.mColorType == 3)
			{
				id = ResID.IMAGE_GREEN_BALL_CBM;
			}
			else if (GameApp.gApp.mColorblind && this.mColorType == 4)
			{
				id = ResID.IMAGE_PURPLE_BALL_CBM;
			}
			Image imageByID = Res.GetImageByID(id);
			ZumasRevenge.Common._S(this.mX + (float)xoff - this.mRadius);
			ZumasRevenge.Common._S(this.mY + (float)yoff - this.mRadius);
			int frame = this.GetFrame(imageByID);
			this.mLastFrame = frame;
			if (GameApp.gApp.Is3DAccelerated())
			{
				Rect celRect = imageByID.GetCelRect(frame);
				this.mGlobalTransform.Reset();
				this.mGlobalTransform.RotateRad(this.mRotation);
				if (this.mDrawScale != 1f)
				{
					this.mGlobalTransform.Scale(this.mDrawScale, this.mDrawScale);
				}
				if (this.mHilightPulse)
				{
					g.SetColorizeImages(true);
					g.SetDrawMode(1);
					g.SetColor(255, 255, 255);
					g.DrawImageTransformF(imageByID, this.mGlobalTransform, celRect, ZumasRevenge.Common._S(this.mX), ZumasRevenge.Common._S(this.mY));
					g.SetDrawMode(0);
					g.SetColorizeImages(false);
				}
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000A5B8 File Offset: 0x000087B8
		public void DoDraw(Graphics g, int xoff, int yoff)
		{
			if (this.mPowerType != PowerType.PowerType_Max)
			{
				this.DrawPower(g);
				return;
			}
			int num = ((GameApp.gApp.GetBoard().GetHallucinateTimer() > 0) ? this.mDisplayType : this.mColorType);
			ResID id = ResID.IMAGE_BLUE_BALL + num;
			if (GameApp.gApp.mColorblind && this.mColorType == 3)
			{
				id = ResID.IMAGE_GREEN_BALL_CBM;
			}
			else if (GameApp.gApp.mColorblind && this.mColorType == 4)
			{
				id = ResID.IMAGE_PURPLE_BALL_CBM;
			}
			Image imageByID = Res.GetImageByID(id);
			float x = ZumasRevenge.Common._S(this.mX + (float)xoff - this.mRadius);
			float y = ZumasRevenge.Common._S(this.mY + (float)yoff - this.mRadius);
			int frame = this.GetFrame(imageByID);
			this.mLastFrame = frame;
			if (GameApp.gApp.Is3DAccelerated())
			{
				Rect celRect = imageByID.GetCelRect(frame);
				this.mGlobalTransform.Reset();
				this.mGlobalTransform.RotateRad(this.mRotation);
				if (this.mDrawScale != 1f)
				{
					this.mGlobalTransform.Scale(this.mDrawScale, this.mDrawScale);
				}
				g.DrawImageTransformF(imageByID, this.mGlobalTransform, celRect, ZumasRevenge.Common._S(this.mX + (float)xoff), ZumasRevenge.Common._S(this.mY + (float)yoff));
				if (this.mHilightPulse)
				{
					g.SetColorizeImages(true);
					g.SetDrawMode(1);
					g.SetColor(255, 255, 255);
					g.DrawImageTransformF(imageByID, this.mGlobalTransform, celRect, ZumasRevenge.Common._S(this.mX), ZumasRevenge.Common._S(this.mY));
					g.SetDrawMode(0);
					g.SetColorizeImages(false);
					return;
				}
			}
			else
			{
				BlendedImage blendedImage = Ball.CreateBlendedBall(num);
				blendedImage.Draw(g, x, y);
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000A773 File Offset: 0x00008973
		public void DoDraw(Graphics g)
		{
			this.DoDraw(g, 0, 0);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000A780 File Offset: 0x00008980
		public void DrawMultPowerup(Graphics g)
		{
			GameApp gApp = GameApp.gApp;
			ResID id = ResID.IMAGE_MULTIPLIER_BALL_BLUE + this.GetColorType();
			bool flag = true;
			if (gApp.mColorblind && this.mColorType == 3)
			{
				flag = false;
				id = (g.Is3D() ? ResID.IMAGE_GREEN_BALL_CBM : ResID.IMAGE_MULTIPLIER_BALL_GREEN_CBM);
			}
			else if (gApp.mColorblind && this.mColorType == 4)
			{
				flag = false;
				id = (g.Is3D() ? ResID.IMAGE_PURPLE_BALL_CBM : ResID.IMAGE_MULTIPLIER_BALL_PURPLE_CBM);
			}
			Image imageByID = Res.GetImageByID(id);
			float num = ZumasRevenge.Common._S(this.mX) - (float)(imageByID.GetCelWidth() / 2);
			float num2 = ZumasRevenge.Common._S(this.mY) - (float)(imageByID.GetCelHeight() / 2);
			if (flag)
			{
				int multAlpha = Ball.GetMultAlpha(this.mMultBallCel);
				int num3 = ZumasRevenge.Common._M(255);
				if (multAlpha != num3)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, multAlpha);
				}
				BlendedImage blendedImage = null;
				BlendedImage blendedImage2 = null;
				if (!g.Is3D())
				{
					blendedImage = Ball.CreateBlendedPowerup(13, this.mColorType, imageByID, this.mMultBallCel);
					blendedImage2 = Ball.CreateBlendedPowerup(14, this.mColorType, imageByID, this.mMultBallCel2);
				}
				Rect celRect = imageByID.GetCelRect(this.mMultBallCel);
				if (g.Is3D())
				{
					g.DrawImageRotatedF(imageByID, num, num2, (double)this.mRotation, celRect);
				}
				else
				{
					blendedImage.Draw(g, num, num2);
				}
				g.SetColorizeImages(false);
				multAlpha = Ball.GetMultAlpha(this.mMultBallCel2);
				if (multAlpha != num3)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, multAlpha);
				}
				celRect = imageByID.GetCelRect(this.mMultBallCel2);
				if (g.Is3D())
				{
					g.DrawImageRotatedF(imageByID, num, num2, (double)this.mRotation, celRect);
				}
				else
				{
					blendedImage2.Draw(g, num, num2);
				}
				g.SetColorizeImages(false);
				g.SetDrawMode(1);
				g.SetColor(255, 255, 255, ZumasRevenge.Common._M(204));
				g.SetColorizeImages(true);
				Image imageByID2 = Res.GetImageByID(ResID.IMAGE_MULTIPLIER_BALL_OUTER);
				celRect = imageByID2.GetCelRect(this.GetFrame(imageByID2, ZumasRevenge.Common._M(2)));
				if (g.Is3D())
				{
					g.DrawImageRotatedF(imageByID2, num, num2, (double)this.mRotation, celRect);
				}
				else
				{
					g.DrawImageRotated(imageByID2, (int)num, (int)num2, (double)this.mRotation, celRect);
				}
				g.SetColorizeImages(false);
				g.SetDrawMode(0);
				return;
			}
			if (g.Is3D())
			{
				Rect celRect2 = imageByID.GetCelRect(0);
				g.DrawImageRotatedF(imageByID, num, num2, (double)this.mRotation, celRect2);
				return;
			}
			BlendedImage blendedImage3 = Ball.CreateBlendedPowerup(13, this.mColorType, imageByID, 0);
			blendedImage3.Draw(g, num, num2);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000AA2D File Offset: 0x00008C2D
		public void UpdateProxmityBombExplosion()
		{
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000AA30 File Offset: 0x00008C30
		public void UpdateRotation()
		{
			if (this.mRotationInc != 0f)
			{
				this.mRotation += this.mRotationInc;
				if ((this.mRotationInc > 0f && this.mRotation > this.mDestRotation) || (this.mRotationInc < 0f && this.mRotation < this.mDestRotation))
				{
					this.mRotation = this.mDestRotation;
					this.mRotationInc = 0f;
				}
			}
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000AAAC File Offset: 0x00008CAC
		public void SetupDefaultOverlayPulse()
		{
			int num = (int)Component.GetComponentValue(this.mOverlayPulse, 0f, this.mUpdateCount);
			this.mOverlayPulse.Clear();
			if (num == 128)
			{
				this.mOverlayPulse.Add(new Component(128f, 178f, this.mUpdateCount, this.mUpdateCount + 50));
				this.mOverlayPulse.Add(new Component(178f, 255f, this.mUpdateCount + 51, this.mUpdateCount + 60));
				this.mOverlayPulse.Add(new Component(255f, 128f, this.mUpdateCount + 61, this.mUpdateCount + 80));
				return;
			}
			this.mOverlayPulse.Add(new Component((float)num, 128f, this.mUpdateCount, this.mUpdateCount + 10));
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000AB90 File Offset: 0x00008D90
		public void SetupElectricOverlayPulse(bool force_fade_out)
		{
			int num = (int)Component.GetComponentValue(this.mOverlayPulse, 0f, this.mUpdateCount);
			this.mOverlayPulse.Clear();
			if (force_fade_out)
			{
				if (num == 0)
				{
					return;
				}
				this.mOverlayPulse.Add(new Component((float)num, 0f, this.mUpdateCount, this.mUpdateCount + 20));
				return;
			}
			else
			{
				if (num == 128)
				{
					this.mOverlayPulse.Add(new Component(128f, 255f, this.mUpdateCount, this.mUpdateCount + 20));
					this.mOverlayPulse.Add(new Component(255f, 128f, this.mUpdateCount + 21, this.mUpdateCount + 41));
					return;
				}
				this.mOverlayPulse.Add(new Component((float)num, 128f, this.mUpdateCount, this.mUpdateCount + 20));
				return;
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000AC72 File Offset: 0x00008E72
		public void SetupElectricOverlayPulse()
		{
			this.SetupElectricOverlayPulse(false);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000AC7B File Offset: 0x00008E7B
		public static void ResetIdGen()
		{
			Ball.mIdGen = 0;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000AC84 File Offset: 0x00008E84
		public Ball()
		{
			this.mFrog = null;
			this.mMultOverlayAlpha = 0;
			this.mMultFX = null;
			this.mInTunnel = false;
			this.mCannonFrame = -1;
			this.mId = ++Ball.mIdGen;
			this.mDoBossPulse = false;
			this.mBossBlinkTimer = 0;
			this.mDebugDrawID = false;
			this.mCurve = null;
			this.mUpdateCount = 0;
			this.mHilightPulse = false;
			this.mSuckFromCompacting = false;
			this.mX = 0f;
			this.mY = 0f;
			this.mColorType = 0;
			this.mDisplayType = 0;
			this.mRadius = (float)ZumasRevenge.Common.GetDefaultBallRadius();
			this.mSuckBack = true;
			this.mBullet = null;
			this.mCel = 0;
			this.mShouldRemove = false;
			this.mLastFrame = 0;
			this.mMultBallCel = 0;
			this.mMultBallCel2 = ZumasRevenge.Common._M(7);
			this.mIsCannon = false;
			this.mSpeedy = false;
			this.mElectricOverlayCel = 0;
			this.mList = null;
			this.mCollidesWithNext = false;
			this.mSuckCount = 0;
			this.mBackwardsCount = 0;
			this.mBackwardsSpeed = 0f;
			this.mComboCount = 0;
			this.mComboScore = 0;
			this.mRotation = 0f;
			this.mRotationInc = 0f;
			this.mNeedCheckCollision = false;
			this.mSuckPending = false;
			this.mShrinkClear = false;
			this.mIconCel = -1;
			this.mIconAppearScale = 1f;
			this.mIconScaleRate = 0f;
			this.mStartFrame = 0;
			this.mWayPoint = 0f;
			this.mPowerType = PowerType.PowerType_Max;
			this.mDestPowerType = PowerType.PowerType_Max;
			this.mPowerCount = 0;
			this.mPowerFade = 0;
			this.mGapBonus = 0;
			this.mNumGaps = 0;
			this.mParticles = null;
			this.mDrawScale = 1f;
			this.mExplodeFrame = 0;
			this.mPowerGracePeriod = 0;
			this.mLastPowerType = PowerType.PowerType_Max;
			this.mDoLaserAnim = false;
			this.mElectricExplodeOverlay.mLoopCount = (this.mElectricExplodeOverlay.mLayer1Cel = (this.mElectricExplodeOverlay.mLayer2Cel = 0));
			this.mExplodingFromLightning = false;
			this.mExploding = (this.mExplodingInTunnel = false);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000AED0 File Offset: 0x000090D0
		public virtual void CopyFrom(Ball other)
		{
			this.mInTunnel = other.mInTunnel;
			this.mMultOverlayAlpha = other.mMultOverlayAlpha;
			this.mMultFX = other.mMultFX;
			this.mColorType = other.mColorType;
			this.mDisplayType = other.mDisplayType;
			this.mWayPoint = other.mWayPoint;
			this.mLastWayPoint = other.mLastWayPoint;
			this.mRotation = other.mRotation;
			this.mDestRotation = other.mDestRotation;
			this.mRotationInc = other.mRotationInc;
			this.mX = other.mX;
			this.mY = other.mY;
			this.mLastX = other.mLastX;
			this.mLastY = other.mLastY;
			this.mDrawScale = other.mDrawScale;
			this.mRadius = other.mRadius;
			this.mPulseState = other.mPulseState;
			this.mPulseTimer = other.mPulseTimer;
			this.mOverlayPulse.Clear();
			this.mOverlayPulse.AddRange(other.mOverlayPulse.ToArray());
			this.mElectricOverlay.Clear();
			this.mElectricOverlay.AddRange(other.mElectricOverlay.ToArray());
			this.mElectricExplodeOverlay = other.mElectricExplodeOverlay;
			this.mElectricOverlayCel = other.mElectricOverlayCel;
			this.mList = other.mList;
			this.mCurve = other.mCurve;
			this.mCollidesWithNext = other.mCollidesWithNext;
			this.mSuckPending = other.mSuckPending;
			this.mShrinkClear = other.mShrinkClear;
			this.mSuckFromCompacting = other.mSuckFromCompacting;
			this.mExplodingInTunnel = other.mExplodingInTunnel;
			this.mExploding = other.mExploding;
			this.mExplodingFromLightning = other.mExplodingFromLightning;
			this.mExplodeFrame = other.mExplodeFrame;
			this.mShouldRemove = other.mShouldRemove;
			this.mSpeedy = other.mSpeedy;
			this.mSuckBack = other.mSuckBack;
			this.mPowerGracePeriod = other.mPowerGracePeriod;
			this.mLastPowerType = other.mLastPowerType;
			this.mCannonFrame = other.mCannonFrame;
			this.mIsCannon = other.mIsCannon;
			this.mDoLaserAnim = other.mDoLaserAnim;
			this.mUpdateCount = other.mUpdateCount;
			this.mCel = other.mCel;
			this.mBullet = other.mBullet;
			this.mSuckCount = other.mSuckCount;
			this.mBackwardsCount = other.mBackwardsCount;
			this.mComboCount = other.mComboCount;
			this.mBackwardsSpeed = other.mBackwardsSpeed;
			this.mPowerCount = other.mPowerCount;
			this.mComboScore = other.mComboScore;
			this.mStartFrame = other.mStartFrame;
			this.mPowerFade = other.mPowerFade;
			this.mGapBonus = other.mGapBonus;
			this.mNumGaps = other.mNumGaps;
			this.mIconAppearScale = other.mIconAppearScale;
			this.mIconScaleRate = other.mIconScaleRate;
			this.mIconCel = other.mIconCel;
			this.mMultBallCel = other.mMultBallCel;
			this.mMultBallCel2 = other.mMultBallCel2;
			this.mParticles = other.mParticles;
			this.mPowerType = other.mPowerType;
			this.mDestPowerType = other.mDestPowerType;
			this.mHilightPulse = other.mHilightPulse;
			this.mDebugDrawID = other.mDebugDrawID;
			this.mDoBossPulse = other.mDoBossPulse;
			this.mBossBlinkTimer = other.mBossBlinkTimer;
			this.mLastFrame = other.mLastFrame;
			this.mFrog = other.mFrog;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000B22C File Offset: 0x0000942C
		public virtual void Dispose()
		{
			if (this.mCurve != null && this.mCurve.mBoard != null)
			{
				this.mCurve.mBoard.BallDeleted(this);
			}
			Board board = GameApp.gApp.GetBoard();
			if (board != null && this == board.GetGuideBall())
			{
				board.GuideBallInvalidated();
			}
			this.mParticles = null;
			this.CleanUpMultiplierOverlays();
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000B289 File Offset: 0x00009489
		public void SetPos(float x, float y)
		{
			this.mX = x;
			this.mY = y;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000B299 File Offset: 0x00009499
		public void SetWayPoint(float thePoint, bool in_tunnel)
		{
			this.mWayPoint = thePoint;
			this.mInTunnel = in_tunnel;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000B2AC File Offset: 0x000094AC
		public int GetFrame(Image img, int div)
		{
			int num = ((img.mNumCols == 1) ? img.mNumRows : (img.mNumRows * img.mNumCols));
			int num2 = (int)this.mWayPoint;
			int num3 = (num2 / div + this.mStartFrame) % num;
			if (num3 < 0)
			{
				num3 = -num3;
			}
			else if (num3 >= num)
			{
				num3 = num - 1;
			}
			return num3;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000B301 File Offset: 0x00009501
		public int GetFrame(Image img)
		{
			return this.GetFrame(img, 1);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000B30B File Offset: 0x0000950B
		public void CleanUpMultiplierOverlays()
		{
			GameApp.gApp.ReleaseGenericCachedEffect(this.mMultFX);
			this.mMultFX = null;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000B324 File Offset: 0x00009524
		public void SetRotation(float theRot, bool immediate)
		{
			if (immediate)
			{
				this.mRotation = theRot;
				return;
			}
			if (MathUtils._eq(theRot, this.mRotation, 0.001f))
			{
				return;
			}
			while (Math.Abs(theRot - this.mRotation) > 3.14159f)
			{
				if (theRot > this.mRotation)
				{
					theRot -= 6.28318f;
				}
				else
				{
					theRot += 6.28318f;
				}
			}
			this.mDestRotation = theRot;
			this.mRotationInc = 0.10471967f;
			if (theRot < this.mRotation)
			{
				this.mRotationInc = -this.mRotationInc;
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000B3A9 File Offset: 0x000095A9
		public void SetRotation(float theRot)
		{
			this.SetRotation(theRot, false);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000B3B4 File Offset: 0x000095B4
		public virtual void DrawBase(Graphics g, int xoff, int yoff)
		{
			if (this.mDrawScale <= 0f || this.mColorType == -1)
			{
				return;
			}
			if (this.mExploding && !this.mShrinkClear && this.mExplodingInTunnel)
			{
				if (g.Is3D())
				{
					this.DrawExplosion(g);
				}
			}
			else if (!this.mExploding || this.mShrinkClear)
			{
				this.DoDrawBase(g, xoff, yoff);
			}
			g.SetColorizeImages(false);
			g.SetDrawMode(0);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000B428 File Offset: 0x00009628
		public virtual void DrawAdditive(Graphics g, int xoff, int yoff)
		{
			if (this.mDrawScale <= 0f || this.mColorType == -1)
			{
				return;
			}
			if (this.mExploding && !this.mShrinkClear && this.mExplodingInTunnel)
			{
				if (g.Is3D())
				{
					this.DrawExplosion(g);
				}
			}
			else if (!this.mExploding || this.mShrinkClear)
			{
				this.DoDrawAdditive(g, xoff, yoff);
				if (this.mPowerFade != 0 && (this.mCurve == null || this.mCurve.mPostZumaFlashTimer <= 0))
				{
					int num = ((this.mPowerType == PowerType.PowerType_GauntletMultBall) ? ((int)ZumasRevenge.Common._M(2f)) : 4);
					if (((this.mPowerFade >> num) & 1) != 0)
					{
						g.SetDrawMode(1);
						this.DoDrawBase(g, xoff, yoff);
						this.DoDrawAdditive(g, xoff, yoff);
					}
				}
				else if ((this.mDoBossPulse && (float)this.mBossBlinkTimer < ZumasRevenge.Common._M(10f)) || (this.mCurve != null && this.mCurve.mPostZumaFlashTimer > 0))
				{
					g.SetDrawMode(1);
					this.DoDrawBase(g, xoff, yoff);
					this.DoDrawAdditive(g, xoff, yoff);
				}
			}
			g.SetColorizeImages(false);
			g.SetDrawMode(0);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000B550 File Offset: 0x00009750
		public virtual void Draw(Graphics g, int xoff, int yoff)
		{
			if (this.mDrawScale <= 0f || this.mColorType == -1)
			{
				return;
			}
			if (this.mExploding && !this.mShrinkClear && this.mExplodingInTunnel)
			{
				if (g.Is3D())
				{
					this.DrawExplosion(g);
				}
			}
			else if (!this.mExploding || this.mShrinkClear)
			{
				this.DoDraw(g, xoff, yoff);
				if (this.mPowerFade != 0 && (this.mCurve == null || this.mCurve.mPostZumaFlashTimer <= 0))
				{
					int num = ((this.mPowerType == PowerType.PowerType_GauntletMultBall) ? ((int)ZumasRevenge.Common._M(2f)) : 4);
					if (((this.mPowerFade >> num) & 1) != 0)
					{
						g.SetDrawMode(1);
						this.DoDraw(g, xoff, yoff);
					}
				}
				else if ((this.mDoBossPulse && (float)this.mBossBlinkTimer < ZumasRevenge.Common._M(10f)) || (this.mCurve != null && this.mCurve.mPostZumaFlashTimer > 0))
				{
					g.SetDrawMode(1);
					this.DoDraw(g, xoff, yoff);
					g.SetDrawMode(0);
				}
			}
			g.SetColorizeImages(false);
			g.SetDrawMode(0);
			if (this.mDebugDrawID)
			{
				Font fontByID = Res.GetFontByID(ResID.FONT_MAIN22);
				g.SetFont(fontByID);
				g.SetColor(SexyColor.Black);
				g.FillRect((int)ZumasRevenge.Common._S(this.mX - 12f), (int)ZumasRevenge.Common._S(this.mY - 8f), ZumasRevenge.Common._S(24), ZumasRevenge.Common._S(16));
				g.SetColor(SexyColor.White);
				g.DrawString(string.Format("{0}", this.mId), (int)ZumasRevenge.Common._S(this.mX - 10f), (int)ZumasRevenge.Common._S(this.mY - 12f) + fontByID.GetAscent());
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000B71A File Offset: 0x0000991A
		public void Draw(Graphics g)
		{
			this.Draw(g, 0, 0);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000B725 File Offset: 0x00009925
		public void DrawProximityBombExplosion(Graphics g)
		{
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000B728 File Offset: 0x00009928
		public void DrawShadow(Graphics g)
		{
			if (!GlobalMembers.gSexyApp.Is3DAccelerated())
			{
				return;
			}
			if (this.mExploding)
			{
				return;
			}
			Transform transform = new Transform();
			float num = ZumasRevenge.Common._S(this.mX - 3f);
			float num2 = ZumasRevenge.Common._S(this.mY + 5f);
			if (this.mDrawScale > 1f)
			{
				num -= ZumasRevenge.Common._S(ZumasRevenge.Common._M(9f)) * (this.mDrawScale - 1f);
				num2 += ZumasRevenge.Common._S(ZumasRevenge.Common._M(15f)) * (this.mDrawScale - 1f);
				transform.Scale(this.mDrawScale, this.mDrawScale);
			}
			g.DrawImageTransformF(Res.GetImageByID(ResID.IMAGE_BALL_SHADOW), transform, num, num2);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000B7E8 File Offset: 0x000099E8
		public void DrawTopLayer(Graphics g)
		{
			Graphics3D graphics3D = g.Get3D();
			if ((this.mPowerType != PowerType.PowerType_Max || this.mElectricOverlay.size<Component>() > 0 || this.mOverlayPulse.size<Component>() > 0) && !this.GetIsExploding())
			{
				Image imageByID = Res.GetImageByID(ResID.IMAGE_BALL_GLOW);
				SexyColor color = Ball.gOverlayColors[this.mColorType];
				color.mAlpha = (int)Component.GetComponentValue(this.mOverlayPulse, 0f, this.mUpdateCount);
				g.SetColor(color);
				g.SetColorizeImages(true);
				g.SetDrawMode(1);
				int num = (int)ZumasRevenge.Common._S(this.mX - this.mRadius);
				int num2 = (int)ZumasRevenge.Common._S(this.mY - this.mRadius);
				num -= (imageByID.mWidth - ZumasRevenge.Common._S(ZumasRevenge.Common.GetDefaultBallSize())) / 2 - 1;
				num2 -= (imageByID.mHeight - ZumasRevenge.Common._S(ZumasRevenge.Common.GetDefaultBallSize())) / 2 - 1;
				if (!GameApp.gApp.mColorblind)
				{
					if (graphics3D != null)
					{
						g.DrawImageF(imageByID, (float)num, (float)num2);
					}
					else
					{
						g.DrawImage(imageByID, num, num2);
					}
				}
				g.SetDrawMode(0);
				g.SetColorizeImages(false);
			}
			if (this.mMultFX != null && (!GameApp.gApp.mColorblind || (this.mColorType != 3 && this.mColorType != 4)))
			{
				this.mMultFX.DrawLayer(g, this.mMultFX.GetLayer("Top"));
				this.mMultFX.DrawLayerNormal(g, this.mMultFX.GetLayer("Top"));
				this.mMultFX.DrawLayerAdditive(g, this.mMultFX.GetLayer("Top"));
				this.mMultFX.DrawPhisycalLayer(g, this.mMultFX.GetLayer("Top"));
			}
			if (this.mExplodingInTunnel)
			{
				this.DrawLightningExplosion(g);
			}
			if (this.mElectricOverlay.size<Component>() > 0)
			{
				int num3 = (int)Component.GetComponentValue(this.mElectricOverlay, 0f, this.mUpdateCount);
				g.SetDrawMode(1);
				if (num3 != 255)
				{
					g.SetColor(255, 255, 255, num3);
					g.SetColorizeImages(true);
				}
				g.SetDrawMode(0);
				if (num3 != 255)
				{
					g.SetColorizeImages(false);
				}
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000BA28 File Offset: 0x00009C28
		public void DrawBottomLayer(Graphics g)
		{
			Graphics3D graphics3D = g.Get3D();
			if (!this.mCurve.mWayPointMgr.InTunnel(this, true))
			{
				this.mCurve.mWayPointMgr.InTunnel(this, false);
			}
			if (this.mMultFX != null && graphics3D != null)
			{
				this.mMultFX.DrawLayer(g, this.mMultFX.GetLayer("Bottom"));
				this.mMultFX.DrawLayerNormal(g, this.mMultFX.GetLayer("Bottom"));
				this.mMultFX.DrawLayerAdditive(g, this.mMultFX.GetLayer("Bottom"));
				this.mMultFX.DrawPhisycalLayer(g, this.mMultFX.GetLayer("Bottom"));
			}
			if (this.mDoLaserAnim)
			{
				Image imageByID = Res.GetImageByID(ResID.IMAGE_LAZER_BURN);
				Rect celRect = imageByID.GetCelRect(Ball.mLaserAnimCel);
				float num = CommonMath.AngleBetweenPoints((float)this.mFrog.GetCenterX(), (float)this.mFrog.GetCenterY(), this.mX, this.mY) + 1.570795f;
				g.DrawImageRotated(imageByID, (int)ZumasRevenge.Common._S(this.mX + (float)ZumasRevenge.Common._M(-38)), (int)ZumasRevenge.Common._S(this.mY + (float)ZumasRevenge.Common._M1(-52)), (double)num, ZumasRevenge.Common._S(ZumasRevenge.Common._M2(38)), ZumasRevenge.Common._S(ZumasRevenge.Common._M3(52)), celRect);
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000BB7C File Offset: 0x00009D7C
		public void DrawAboveBalls(Graphics g)
		{
			if (this.mIconCel != -1 && MathUtils._geq(this.mIconAppearScale, 1f) && g.Is3D())
			{
				Image imageByID = Res.GetImageByID(ResID.IMAGE_POWERUPS_PULSES);
				float num = ((this.GetPowerOrDestType() == PowerType.PowerType_MoveBackwards) ? 0f : (-1.570795f));
				int num2 = (int)ZumasRevenge.Common._S(this.mX);
				int num3 = (int)ZumasRevenge.Common._S(this.mY);
				g.SetDrawMode(1);
				g.SetColorizeImages(true);
				g.SetColor(new SexyColor(ZumasRevenge.Common.gBallColors[this.mColorType]));
				this.mGlobalTransform.Reset();
				this.mGlobalTransform.Scale(this.mIconAppearScale, this.mIconAppearScale);
				this.mGlobalTransform.RotateRad(this.mRotation + num);
				g.DrawImageTransform(imageByID, this.mGlobalTransform, imageByID.GetCelRect(this.mIconCel), (float)num2, (float)num3);
				g.SetDrawMode(0);
				g.SetColorizeImages(false);
			}
			if (this.mExploding && !this.mShrinkClear && !this.mExplodingInTunnel)
			{
				this.DrawExplosion(g);
			}
			if (!this.mExplodingInTunnel)
			{
				this.DrawLightningExplosion(g);
			}
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000BCA4 File Offset: 0x00009EA4
		public void DrawLightningExplosion(Graphics g)
		{
			if (this.mElectricExplodeOverlay.mLayer1Alpha.size<Component>() > 0)
			{
				g.SetDrawMode(1);
				int num = (int)Component.GetComponentValue(this.mElectricExplodeOverlay.mLayer2Alpha, 255f, this.mUpdateCount);
				if (num != 255)
				{
					g.SetColor(255, 255, 255, num);
					g.SetColorizeImages(true);
				}
				g.SetColorizeImages(false);
				num = (int)Component.GetComponentValue(this.mElectricExplodeOverlay.mLayer1Alpha, 255f, this.mUpdateCount);
				Component.GetComponentValue(this.mElectricExplodeOverlay.mLayer1Scale, 1f, this.mUpdateCount);
				if (num != 255)
				{
					g.SetColor(255, 255, 255, num);
					g.SetColorizeImages(true);
				}
				g.SetDrawMode(0);
				if (num != 255)
				{
					g.SetColorizeImages(false);
				}
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000BD8C File Offset: 0x00009F8C
		public void DoElectricOverlay(bool val)
		{
			if (!val && Enumerable.Count<Component>(this.mElectricOverlay) > 0)
			{
				int num = (int)Component.GetComponentValue(this.mElectricOverlay, 0f, this.mUpdateCount);
				this.mElectricOverlay.Clear();
				int num2 = (int)(0.039215688f * (float)num);
				this.mElectricOverlay.Add(new Component((float)num, 0f, this.mUpdateCount, this.mUpdateCount + ((num2 < 1) ? 1 : num2)));
				this.SetupDefaultOverlayPulse();
				return;
			}
			if (val && Enumerable.Count<Component>(this.mElectricOverlay) == 0)
			{
				this.mElectricOverlay.Add(new Component(0f, 255f, this.mUpdateCount, this.mUpdateCount + 10));
				return;
			}
			if (!val)
			{
				if (!val)
				{
					this.SetupDefaultOverlayPulse();
				}
				return;
			}
			int num3 = (int)Component.GetComponentValue(this.mElectricOverlay, 0f, this.mUpdateCount);
			if (num3 == 255)
			{
				return;
			}
			this.mElectricOverlay.Clear();
			int num4 = (int)(0.039215688f * (float)num3);
			this.mElectricOverlay.Add(new Component((float)num3, 255f, this.mUpdateCount, this.mUpdateCount + ((num4 < 1) ? 1 : num4)));
			this.SetupElectricOverlayPulse();
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000BEBC File Offset: 0x0000A0BC
		public bool CollidesWithPhysically(Ball theBall, int thePad)
		{
			float num = theBall.GetX() - this.GetX();
			float num2 = theBall.GetY() - this.GetY();
			float num3 = (float)theBall.GetRadius() + (float)(thePad * 2) + (float)this.GetRadius();
			return num * num + num2 * num2 < num3 * num3;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000BF06 File Offset: 0x0000A106
		public bool CollidesWithPhysically(Ball theBall)
		{
			return this.CollidesWithPhysically(theBall, 0);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000BF10 File Offset: 0x0000A110
		public bool CollidesWith(Ball theBall, int thePad)
		{
			return Math.Abs((float)((int)this.mWayPoint) - (float)((int)theBall.mWayPoint)) < (float)((ZumasRevenge.Common.GetDefaultBallRadius() + thePad) * 2);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000BF34 File Offset: 0x0000A134
		public bool CollidesWith(Ball theBall)
		{
			return this.CollidesWith(theBall, 0);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000BF40 File Offset: 0x0000A140
		public bool CollidesWithPhysically(int pointx, int pointy, int radius)
		{
			float num = (float)pointx - this.GetX();
			float num2 = (float)pointy - this.GetY();
			float num3 = (float)radius + (float)this.GetRadius();
			return num * num + num2 * num2 < num3 * num3;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000BF78 File Offset: 0x0000A178
		public bool Intersects(SexyVector3 p1, SexyVector3 v1, ref float t)
		{
			SexyVector3 v2 = new SexyVector3(p1.x - this.mX, p1.y - this.mY, 0f);
			float num = this.mRadius - (float)ZumasRevenge.Common._M(1);
			float num2 = v1.Dot(v1);
			float num3 = 2f * v2.Dot(v1);
			float num4 = v2.Dot(v2) - num * 2f * (num * 2f);
			float num5 = num3 * num3 - 4f * num2 * num4;
			if (num5 < 0f)
			{
				return false;
			}
			num5 = (float)Math.Sqrt((double)num5);
			t = (-num3 - num5) / (2f * num2);
			return true;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000C024 File Offset: 0x0000A224
		public void SetBullet(Bullet theBullet)
		{
			this.mBullet = theBullet;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000C030 File Offset: 0x0000A230
		public void SetCollidesWithPrev(bool collidesWithPrev)
		{
			Ball prevBall = this.GetPrevBall();
			if (prevBall != null)
			{
				prevBall.SetCollidesWithNext(collidesWithPrev);
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000C050 File Offset: 0x0000A250
		public bool GetCollidesWithPrev()
		{
			Ball prevBall = this.GetPrevBall();
			return prevBall != null && prevBall.GetCollidesWithNext();
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000C070 File Offset: 0x0000A270
		public void UpdateCollisionInfo(int thePad)
		{
			Ball prevBall = this.GetPrevBall();
			Ball nextBall = this.GetNextBall();
			if (prevBall != null)
			{
				prevBall.SetCollidesWithNext(prevBall.CollidesWith(this, thePad));
			}
			if (nextBall != null)
			{
				this.SetCollidesWithNext(nextBall.CollidesWith(this, thePad));
				return;
			}
			this.SetCollidesWithNext(false);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000C0B5 File Offset: 0x0000A2B5
		public void UpdateCollisionInfo()
		{
			this.UpdateCollisionInfo(0);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000C0C0 File Offset: 0x0000A2C0
		public void SetPowerType(PowerType theType, bool delay)
		{
			this.mDoBossPulse = false;
			if (theType == this.mPowerType)
			{
				return;
			}
			this.mPulseState = 0;
			this.mPulseTimer = 0;
			this.mIconCel = -1;
			if (theType != PowerType.PowerType_Max)
			{
				this.mPowerGracePeriod = 0;
				this.mLastPowerType = PowerType.PowerType_Max;
			}
			if (delay)
			{
				this.mDestPowerType = theType;
				if (theType == PowerType.PowerType_Max && this.mPowerType == PowerType.PowerType_GauntletMultBall)
				{
					this.mPowerFade = 300;
				}
				else
				{
					this.mPowerFade = 100;
				}
				switch (theType)
				{
				case PowerType.PowerType_ProximityBomb:
					this.mIconCel = 3;
					break;
				case PowerType.PowerType_SlowDown:
					this.mIconCel = 2;
					break;
				case PowerType.PowerType_Accuracy:
					this.mIconCel = 0;
					break;
				case PowerType.PowerType_MoveBackwards:
					this.mIconCel = 4;
					break;
				case PowerType.PowerType_Cannon:
					this.mIconCel = 5;
					break;
				case PowerType.PowerType_ColorNuke:
					this.mIconCel = 1;
					break;
				case PowerType.PowerType_Laser:
					this.mIconCel = 6;
					break;
				}
				int soundByID = Res.GetSoundByID(ResID.SOUND_MULT_APPEAR);
				int soundByID2 = Res.GetSoundByID(ResID.SOUND_POWERUP_APPEARS);
				int soundByID3 = Res.GetSoundByID(ResID.SOUND_MULT_DISAPPEAR);
				int soundByID4 = Res.GetSoundByID(ResID.SOUND_POWERUP_DISAPPEARS);
				if (theType != PowerType.PowerType_Max)
				{
					if (theType == PowerType.PowerType_GauntletMultBall)
					{
						((GameApp)GlobalMembers.gSexyApp).PlaySample(soundByID);
					}
					else
					{
						((GameApp)GlobalMembers.gSexyApp).PlaySample(soundByID2);
					}
				}
				else if (this.GetPowerOrDestType() != PowerType.PowerType_Max)
				{
					if (this.GetPowerOrDestType() == PowerType.PowerType_GauntletMultBall)
					{
						((GameApp)GlobalMembers.gSexyApp).PlaySample(soundByID3);
					}
					else
					{
						((GameApp)GlobalMembers.gSexyApp).PlaySample(soundByID4);
					}
				}
				this.mIconAppearScale = 5f;
				this.mIconScaleRate = (this.mIconAppearScale - 1f) / (float)this.mPowerFade;
			}
			else
			{
				this.mDestPowerType = PowerType.PowerType_Max;
				this.mPowerType = theType;
			}
			if (theType != PowerType.PowerType_Max && this.mCurve != null)
			{
				this.mCurve.SetColorHasPowerup(this.mColorType, true);
			}
			this.SetupDefaultOverlayPulse();
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000C294 File Offset: 0x0000A494
		public void SetPowerType(PowerType theType)
		{
			this.SetPowerType(theType, true);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000C29E File Offset: 0x0000A49E
		public PowerType GetPowerOrDestType(bool include_grace_period)
		{
			if (this.mPowerType != PowerType.PowerType_Max)
			{
				return this.mPowerType;
			}
			if (this.mPowerGracePeriod > 0 && this.mLastPowerType != PowerType.PowerType_Max)
			{
				return this.mLastPowerType;
			}
			return this.mDestPowerType;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000C2D1 File Offset: 0x0000A4D1
		public PowerType GetPowerOrDestType()
		{
			return this.GetPowerOrDestType(true);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000C2DA File Offset: 0x0000A4DA
		public void RemoveFromList()
		{
			if (this.mList != null)
			{
				this.mList.Remove(this);
				this.mList = null;
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000C2F8 File Offset: 0x0000A4F8
		public int InsertInList(List<Ball> theList, int theInsertItr, CurveMgr cm)
		{
			this.mList = theList;
			theList.Insert(theInsertItr, this);
			this.mCurve = cm;
			return theInsertItr;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000C311 File Offset: 0x0000A511
		public SexyVector3 GetSpeed()
		{
			return new SexyVector3(this.mX - this.mLastX, this.mY - this.mLastY, 0f);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000C337 File Offset: 0x0000A537
		public float GetWayPointProgress()
		{
			return this.mWayPoint - this.mLastWayPoint;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000C348 File Offset: 0x0000A548
		public Ball GetPrevBall(bool mustCollide)
		{
			if (this.mList == null)
			{
				return null;
			}
			int listItr = this.GetListItr();
			if (listItr == 0)
			{
				return null;
			}
			if (!mustCollide)
			{
				return this.mList[listItr - 1];
			}
			Ball ball = this.mList[listItr - 1];
			if (ball.GetCollidesWithNext())
			{
				return ball;
			}
			return null;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000C39B File Offset: 0x0000A59B
		public Ball GetPrevBall()
		{
			return this.GetPrevBall(false);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000C3A4 File Offset: 0x0000A5A4
		public Ball GetNextBall(bool mustCollide)
		{
			if (this.mList == null)
			{
				return null;
			}
			int num = this.GetListItr();
			num++;
			if (num >= Enumerable.Count<Ball>(this.mList))
			{
				return null;
			}
			if (!mustCollide || this.GetCollidesWithNext())
			{
				return this.mList[num];
			}
			return null;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000C3EF File Offset: 0x0000A5EF
		public Ball GetNextBall()
		{
			return this.GetNextBall(false);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000C3F8 File Offset: 0x0000A5F8
		public CurveMgr GetCurve()
		{
			return this.mCurve;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000C400 File Offset: 0x0000A600
		public void Explode(bool in_tunnel, bool from_lightning_frog)
		{
			if (this.mExploding)
			{
				return;
			}
			this.mExploding = true;
			this.mExplodingInTunnel = in_tunnel;
			Board board = GameApp.gApp.GetBoard();
			if (!this.mExplodingInTunnel)
			{
				board.AddBallExplosionParticleEffect(this);
			}
			if (this.GetPowerOrDestType() == PowerType.PowerType_ProximityBomb)
			{
				PowerEffect powerEffect = new PowerEffect(this.mX, this.mY);
				powerEffect.AddDefaultEffectType(0, this.mColorType, this.mRotation);
				board.AddPowerEffect(powerEffect);
				board.AddProxBombExplosion(this.GetX(), this.GetY());
			}
			else if (this.GetPowerOrDestType() == PowerType.PowerType_Accuracy)
			{
				PowerEffect powerEffect2 = new PowerEffect(this.mX, this.mY);
				powerEffect2.AddDefaultEffectType(1, this.mColorType, this.mRotation);
				board.AddPowerEffect(powerEffect2);
			}
			else if (this.GetPowerOrDestType() == PowerType.PowerType_MoveBackwards)
			{
				PowerEffect powerEffect3 = new ReversePowerEffect(this.mX, this.mY, this);
				powerEffect3.AddDefaultEffectType(2, this.mColorType, this.mRotation);
				board.AddPowerEffect(powerEffect3);
			}
			else if (this.GetPowerOrDestType() == PowerType.PowerType_SlowDown)
			{
				PowerEffect powerEffect4 = new PowerEffect(this.mX, this.mY);
				powerEffect4.AddDefaultEffectType(3, this.mColorType, this.mRotation);
				board.AddPowerEffect(powerEffect4);
			}
			else if (this.GetPowerOrDestType() == PowerType.PowerType_Cannon)
			{
				PowerEffect powerEffect5 = new CannonPowerEffect(this);
				powerEffect5.AddDefaultEffectType(4, this.mColorType, this.mRotation);
				board.AddPowerEffect(powerEffect5);
			}
			else if (this.GetPowerOrDestType() == PowerType.PowerType_Laser)
			{
				PowerEffect powerEffect6 = new PowerEffect(this.mX, this.mY);
				powerEffect6.AddDefaultEffectType(5, this.mColorType, this.mRotation);
				board.AddPowerEffect(powerEffect6);
			}
			else if (this.GetPowerOrDestType() == PowerType.PowerType_GauntletMultBall)
			{
				this.CleanUpMultiplierOverlays();
			}
			if (this.GetPowerOrDestType() != PowerType.PowerType_Max)
			{
				this.mCurve.SetColorHasPowerup(this.mColorType, false);
			}
			if (from_lightning_frog)
			{
				this.mExplodingFromLightning = true;
				this.mElectricOverlay.Clear();
				this.mElectricOverlay.Add(new Component(255f, 0f, this.mUpdateCount, this.mUpdateCount + 10));
				this.mElectricExplodeOverlay.mLayer1Alpha.Add(new Component(0f, 0f, this.mUpdateCount, this.mUpdateCount + 20));
				this.mElectricExplodeOverlay.mLayer1Alpha.Add(new Component(25f, 255f, this.mUpdateCount + 21, this.mUpdateCount + 41));
				this.mElectricExplodeOverlay.mLayer1Scale.Add(new Component(0.5f, 1f, this.mUpdateCount + 21, this.mUpdateCount + 41));
				this.mElectricExplodeOverlay.mLayer2Alpha.Add(new Component(25f, 255f, this.mUpdateCount, this.mUpdateCount + 20));
				this.mElectricExplodeOverlay.mLoopCount = 0;
				return;
			}
			if (this.mElectricOverlay.size<Component>() > 0)
			{
				this.mElectricOverlay.Clear();
				this.mElectricOverlay.Add(new Component(255f, 0f, this.mUpdateCount, this.mUpdateCount + 5));
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000C71B File Offset: 0x0000A91B
		public void Explode(bool in_tunnel)
		{
			this.Explode(in_tunnel, false);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000C725 File Offset: 0x0000A925
		public void Explode()
		{
			this.Explode(false, false);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000C730 File Offset: 0x0000A930
		public void Update()
		{
			this.mUpdateCount++;
			this.mLastWayPoint = this.mWayPoint;
			this.mLastX = this.mX;
			this.mLastY = this.mY;
			GameApp gApp = GameApp.gApp;
			if (gApp.GetBoard().GetHallucinateTimer() > 0 && this.mUpdateCount % ZumasRevenge.Common._M(25) == 0)
			{
				this.mDisplayType = MathUtils.SafeRand() % 6;
			}
			if (this.mDoBossPulse && this.mBossBlinkTimer == 0)
			{
				this.mBossBlinkTimer = ZumasRevenge.Common._M(20);
			}
			else if (this.mBossBlinkTimer > 0)
			{
				this.mBossBlinkTimer--;
			}
			if (this.mUpdateCount % ZumasRevenge.Common._M(6) == 0 && (!gApp.mColorblind || (this.mColorType != 3 && this.mColorType != 4)))
			{
				Image imageByID = Res.GetImageByID(ResID.IMAGE_MULTIPLIER_BALL_BLUE);
				this.mMultBallCel = (this.mMultBallCel + 1) % (imageByID.mNumRows * imageByID.mNumCols);
				this.mMultBallCel2 = (this.mMultBallCel2 + 1) % (imageByID.mNumRows * imageByID.mNumCols);
			}
			if (this.mPowerFade > 0)
			{
				this.mIconAppearScale -= this.mIconScaleRate;
				if (this.mIconAppearScale < 1f)
				{
					this.mIconAppearScale = 1f;
				}
				if (this.mPowerType == PowerType.PowerType_GauntletMultBall && this.mDestPowerType == PowerType.PowerType_Max && this.mPowerFade < 51)
				{
					this.mMultOverlayAlpha -= 5;
					if (this.mMultOverlayAlpha < 0)
					{
						this.mMultOverlayAlpha = 0;
					}
				}
				this.mPowerFade--;
				if (this.mPowerFade == 0)
				{
					this.mPowerType = this.mDestPowerType;
					if (this.mPowerType == PowerType.PowerType_GauntletMultBall)
					{
						int num = this.mColorType;
						if (gApp.mColorblind && (this.mColorType == 3 || this.mColorType == 4))
						{
							num = 5;
						}
						this.mMultFX = gApp.mResourceManager.GetPIEffect(Ball.fx_files[num]).Duplicate();
						this.mMultFX.mEmitAfterTimeline = true;
						this.mMultOverlayAlpha = 0;
					}
					else if (this.mPowerType == PowerType.PowerType_Max)
					{
						this.CleanUpMultiplierOverlays();
					}
					this.mIconCel = -1;
					this.mDestPowerType = PowerType.PowerType_Max;
					if (this.mPowerType != PowerType.PowerType_Max && this.mPowerCount <= 0)
					{
						this.mPowerCount = (int)((float)ZumasRevenge.Common._M(2000) * GameApp.gDDS.mHandheldBalance.mFruitPowerupAdditionalDuration);
					}
				}
			}
			if (this.mMultFX != null)
			{
				this.mMultFX.mDrawTransform.LoadIdentity();
				float num2 = GameApp.DownScaleNum(1f);
				this.mMultFX.mDrawTransform.Scale(num2, num2);
				this.mMultFX.mDrawTransform.RotateRad(this.mRotation);
				this.mMultFX.mDrawTransform.Translate(ZumasRevenge.Common._S(this.mX), ZumasRevenge.Common._S(this.mY));
				this.mMultFX.mColor.mAlpha = this.mMultOverlayAlpha;
				this.mMultFX.Update();
			}
			if (this.mMultFX != null && (this.mDestPowerType != PowerType.PowerType_Max || this.mPowerFade >= 51 || this.mPowerFade == 0))
			{
				int num3 = ZumasRevenge.Common._M(3);
				if (this.mInTunnel && this.mMultOverlayAlpha > 0)
				{
					this.mMultOverlayAlpha -= num3;
				}
				else if (!this.mInTunnel && this.mMultOverlayAlpha < 255)
				{
					this.mMultOverlayAlpha += num3;
				}
			}
			this.mMultOverlayAlpha = Math.Min(Math.Max(this.mMultOverlayAlpha, 0), 255);
			if (this.mDoLaserAnim && this.mUpdateCount % ZumasRevenge.Common._M(4) == 0)
			{
				Ball.mLaserAnimCel = (Ball.mLaserAnimCel + 1) % Res.GetImageByID(ResID.IMAGE_LAZER_BURN).mNumCols;
			}
			if (this.mPowerCount > 0 && !this.mExploding && --this.mPowerCount <= 0)
			{
				this.mPowerGracePeriod = ZumasRevenge.Common._M(150);
				this.mLastPowerType = this.GetPowerOrDestType();
				this.mCurve.PowerupExpired(this.GetPowerOrDestType());
				this.mCurve.SetColorHasPowerup(this.mColorType, false);
				this.SetPowerType(PowerType.PowerType_Max);
			}
			if (this.mPowerGracePeriod > 0 && --this.mPowerGracePeriod == 0)
			{
				this.mLastPowerType = PowerType.PowerType_Max;
			}
			if (this.mElectricOverlay.size<Component>() > 0 && Component.UpdateComponentVec(this.mElectricOverlay, this.mUpdateCount) && MathUtils._eq(Component.GetComponentValue(this.mElectricOverlay, 0f, this.mUpdateCount), 0f, 0.0001f))
			{
				this.mElectricOverlay.Clear();
			}
			if (this.mElectricExplodeOverlay.mLayer1Alpha.size<Component>() > 0)
			{
				int num4 = this.mUpdateCount % ZumasRevenge.Common._M(7);
			}
			if (this.mExploding && this.mElectricExplodeOverlay.mLayer1Alpha.size<Component>() > 0)
			{
				Component.UpdateComponentVec(this.mElectricExplodeOverlay.mLayer2Alpha, this.mUpdateCount);
				Component.UpdateComponentVec(this.mElectricExplodeOverlay.mLayer1Scale, this.mUpdateCount);
				if (Component.UpdateComponentVec(this.mElectricExplodeOverlay.mLayer1Alpha, this.mUpdateCount))
				{
					if (++this.mElectricExplodeOverlay.mLoopCount == 1)
					{
						this.mElectricExplodeOverlay.mLayer1Alpha.Clear();
						this.mElectricExplodeOverlay.mLayer1Alpha.Add(new Component(255f, 255f, this.mUpdateCount, this.mUpdateCount + 30));
					}
					else if (this.mElectricExplodeOverlay.mLoopCount == 2)
					{
						this.mElectricExplodeOverlay.mLayer1Alpha.Clear();
						this.mElectricExplodeOverlay.mLayer1Alpha.Add(new Component(255f, 0f, this.mUpdateCount, this.mUpdateCount + 20));
						this.mElectricExplodeOverlay.mLayer2Alpha.Clear();
						this.mElectricExplodeOverlay.mLayer2Alpha.Add(new Component(255f, 0f, this.mUpdateCount, this.mUpdateCount + 20));
						this.mElectricExplodeOverlay.mLayer1Scale.Clear();
						this.mElectricExplodeOverlay.mLayer1Scale.Add(new Component(1f, 1f, this.mUpdateCount, this.mUpdateCount + 4));
						this.mElectricExplodeOverlay.mLayer1Scale.Add(new Component(1f, 0.2f, this.mUpdateCount + 5, this.mUpdateCount + 20));
					}
					else if (this.mElectricExplodeOverlay.mLoopCount == 3)
					{
						this.mElectricExplodeOverlay.mLayer1Scale.Clear();
						this.mElectricExplodeOverlay.mLayer1Alpha.Clear();
						this.mElectricExplodeOverlay.mLayer2Alpha.Clear();
					}
				}
			}
			if (this.mPowerType != PowerType.PowerType_Max)
			{
				if (Component.UpdateComponentVec(this.mOverlayPulse, this.mUpdateCount))
				{
					if (this.mElectricOverlay.size<Component>() == 0)
					{
						this.SetupDefaultOverlayPulse();
					}
					else
					{
						this.SetupElectricOverlayPulse();
					}
				}
				if (!this.mExploding)
				{
					this.mPulseTimer++;
					if (this.mPulseState == 0 && this.mPulseTimer >= ZumasRevenge.Common._M(30))
					{
						this.mPulseState++;
						this.mPulseTimer = 0;
					}
					else if (this.mPulseState == 1 && this.mPulseTimer >= 128)
					{
						this.mPulseTimer = 0;
						this.mPulseState++;
					}
					else if (this.mPulseState == 2 && this.mPulseTimer >= ZumasRevenge.Common._M(25))
					{
						this.mPulseState = 0;
						this.mPulseTimer = 0;
					}
				}
			}
			else if (this.mElectricOverlay.size<Component>() > 0 && Component.UpdateComponentVec(this.mOverlayPulse, this.mUpdateCount))
			{
				this.SetupElectricOverlayPulse();
			}
			else if (this.mElectricOverlay.size<Component>() == 0 && this.mOverlayPulse.size<Component>() > 0 && Component.UpdateComponentVec(this.mOverlayPulse, this.mUpdateCount))
			{
				this.mOverlayPulse.Clear();
			}
			this.UpdateRotation();
			if (this.mPowerType == PowerType.PowerType_MoveBackwards && this.mUpdateCount % ZumasRevenge.Common._M(4) == 0)
			{
				this.mCel = ((this.mCel == 0) ? (Res.GetImageByID(ResID.IMAGE_POWERUP_REVERSE_ANYCOLOR).mNumCols - 1) : (this.mCel - 1));
				return;
			}
			if (this.mPowerType == PowerType.PowerType_Laser && this.mUpdateCount % ZumasRevenge.Common._M(4) == 0)
			{
				this.mCel = (this.mCel + 1) % Res.GetImageByID(ResID.IMAGE_POWERUP_LAZER_ANYCOLOR).mNumRows;
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000CFB0 File Offset: 0x0000B1B0
		public void UpdateExplosion()
		{
			if (!this.mExploding)
			{
				return;
			}
			if (!this.mExplodingFromLightning && this.mUpdateCount % ZumasRevenge.Common._M(2) == 0)
			{
				this.mExplodeFrame++;
			}
			if (this.mExplodeFrame >= 20 || this.mElectricExplodeOverlay.mLoopCount >= 3)
			{
				this.mShouldRemove = true;
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000D00C File Offset: 0x0000B20C
		public void SetFrame(int theFrame)
		{
			ResID id = ResID.IMAGE_BLUE_BALL + this.mColorType;
			if (GameApp.gApp.mColorblind && this.mColorType == 3)
			{
				id = ResID.IMAGE_GREEN_BALL_CBM;
			}
			else if (GameApp.gApp.mColorblind && this.mColorType == 4)
			{
				id = ResID.IMAGE_PURPLE_BALL_CBM;
			}
			Image imageByID = Res.GetImageByID(id);
			int mNumRows = imageByID.mNumRows;
			int num = (int)this.mWayPoint + theFrame;
			num %= mNumRows;
			this.mStartFrame = mNumRows - num;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000D083 File Offset: 0x0000B283
		public void ForceFrame(int theFrame)
		{
			this.mStartFrame = theFrame;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000D08C File Offset: 0x0000B28C
		public void IncFrame(int theInc)
		{
			ResID id = ResID.IMAGE_BLUE_BALL + this.mColorType;
			if (GameApp.gApp.mColorblind && this.mColorType == 3)
			{
				id = ResID.IMAGE_GREEN_BALL_CBM;
			}
			else if (GameApp.gApp.mColorblind && this.mColorType == 4)
			{
				id = ResID.IMAGE_PURPLE_BALL_CBM;
			}
			Image imageByID = Res.GetImageByID(id);
			int mNumRows = imageByID.mNumRows;
			this.mStartFrame += theInc;
			this.mStartFrame %= mNumRows;
			if (this.mStartFrame < 0)
			{
				this.mStartFrame = mNumRows + this.mStartFrame;
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000D120 File Offset: 0x0000B320
		public void RandomizeFrame()
		{
			ResID id = ResID.IMAGE_BLUE_BALL + this.mColorType;
			if (GameApp.gApp.mColorblind && this.mColorType == 3)
			{
				id = ResID.IMAGE_GREEN_BALL_CBM;
			}
			else if (GameApp.gApp.mColorblind && this.mColorType == 4)
			{
				id = ResID.IMAGE_PURPLE_BALL_CBM;
			}
			Image imageByID = Res.GetImageByID(id);
			this.mStartFrame = MathUtils.SafeRand() % imageByID.mNumRows;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000D18C File Offset: 0x0000B38C
		public static void DeleteBallGlobals()
		{
			for (int i = 0; i < 8; i++)
			{
				Ball.gBlendedBalls[i] = null;
				if (i < 6)
				{
					Ball.gBlendedBombLights[i] = null;
				}
				for (int j = 0; j <= 14; j++)
				{
					Ball.gBlendedPowerups[j, i] = null;
				}
			}
			for (int j = 0; j < 14; j++)
			{
				Ball.gBlendedPowerupLights[j] = null;
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000D1EC File Offset: 0x0000B3EC
		public virtual void SyncState(DataSync sync)
		{
			sync.RegisterPointer(this);
			sync.SyncLong(ref this.mId);
			sync.SyncLong(ref this.mPowerGracePeriod);
			int num = (int)this.mLastPowerType;
			sync.SyncLong(ref num);
			this.mLastPowerType = (PowerType)num;
			sync.SyncLong(ref this.mColorType);
			sync.SyncFloat(ref this.mWayPoint);
			sync.SyncFloat(ref this.mRotation);
			sync.SyncFloat(ref this.mDestRotation);
			sync.SyncFloat(ref this.mRotationInc);
			sync.SyncFloat(ref this.mX);
			sync.SyncFloat(ref this.mY);
			sync.SyncBoolean(ref this.mInTunnel);
			sync.SyncLong(ref this.mMultOverlayAlpha);
			SexyBuffer buffer = sync.GetBuffer();
			if (sync.isWrite())
			{
				buffer.WriteBoolean(this.mMultFX != null);
				if (this.mMultFX != null)
				{
					ZumasRevenge.Common.SerializePIEffect(this.mMultFX, sync);
				}
			}
			else
			{
				this.mMultFX = null;
				if (buffer.ReadBoolean())
				{
					this.mMultFX = new PIEffect();
					ZumasRevenge.Common.DeserializePIEffect(this.mMultFX, sync);
				}
			}
			sync.SyncBoolean(ref this.mDoBossPulse);
			sync.SyncFloat(ref this.mRadius);
			sync.SyncLong(ref this.mPulseState);
			sync.SyncLong(ref this.mPulseTimer);
			sync.SyncLong(ref this.mCannonFrame);
			sync.SyncBoolean(ref this.mCollidesWithNext);
			sync.SyncBoolean(ref this.mNeedCheckCollision);
			sync.SyncBoolean(ref this.mSuckPending);
			sync.SyncBoolean(ref this.mShrinkClear);
			sync.SyncBoolean(ref this.mSuckFromCompacting);
			sync.SyncBoolean(ref this.mExplodingInTunnel);
			sync.SyncBoolean(ref this.mExploding);
			sync.SyncLong(ref this.mExplodeFrame);
			sync.SyncBoolean(ref this.mShouldRemove);
			sync.SyncBoolean(ref this.mIsCannon);
			sync.SyncLong(ref this.mUpdateCount);
			sync.SyncLong(ref this.mCel);
			sync.SyncLong(ref this.mSuckCount);
			sync.SyncBoolean(ref this.mSuckBack);
			sync.SyncLong(ref this.mBackwardsCount);
			sync.SyncFloat(ref this.mBackwardsSpeed);
			sync.SyncLong(ref this.mComboCount);
			sync.SyncLong(ref this.mComboScore);
			sync.SyncLong(ref this.mStartFrame);
			sync.SyncLong(ref this.mPowerCount);
			sync.SyncLong(ref this.mPowerFade);
			sync.SyncBoolean(ref this.mSpeedy);
			sync.SyncLong(ref this.mGapBonus);
			sync.SyncLong(ref this.mNumGaps);
			sync.SyncLong(ref this.mElectricOverlayCel);
			sync.SyncBoolean(ref this.mExplodingFromLightning);
			sync.SyncLong(ref this.mElectricExplodeOverlay.mLoopCount);
			sync.SyncLong(ref this.mElectricExplodeOverlay.mLayer2Cel);
			sync.SyncLong(ref this.mElectricExplodeOverlay.mLayer1Cel);
			this.SyncListComponents(sync, this.mOverlayPulse, true);
			this.SyncListComponents(sync, this.mElectricOverlay, true);
			this.SyncListComponents(sync, this.mElectricExplodeOverlay.mLayer1Alpha, true);
			this.SyncListComponents(sync, this.mElectricExplodeOverlay.mLayer2Alpha, true);
			this.SyncListComponents(sync, this.mElectricExplodeOverlay.mLayer1Scale, true);
			num = (int)this.mPowerType;
			sync.SyncLong(ref num);
			this.mPowerType = (PowerType)num;
			num = (int)this.mDestPowerType;
			sync.SyncLong(ref num);
			this.mDestPowerType = (PowerType)num;
			sync.SyncPointer(this);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000D528 File Offset: 0x0000B728
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

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000D5C8 File Offset: 0x0000B7C8
		public void SetColorType(int theType)
		{
			this.mColorType = theType;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000D5D1 File Offset: 0x0000B7D1
		public void SetCollidesWithNext(bool collidesWithNext)
		{
			this.mCollidesWithNext = collidesWithNext;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000D5DA File Offset: 0x0000B7DA
		public void DoLaserAnim(bool d, Gun g)
		{
			this.mDoLaserAnim = d;
			this.mFrog = g;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000D5EA File Offset: 0x0000B7EA
		public void DoLaserAnim(bool d)
		{
			this.DoLaserAnim(d, null);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000D5F4 File Offset: 0x0000B7F4
		public void SetShrinkClear(bool shrink)
		{
			this.mShrinkClear = shrink;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000D5FD File Offset: 0x0000B7FD
		public void SetSuckCount(int theCount, bool suck_back)
		{
			this.mSuckCount = theCount;
			this.mSuckBack = suck_back;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000D60D File Offset: 0x0000B80D
		public void SetSuckCount(int theCount)
		{
			this.SetSuckCount(theCount, true);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000D617 File Offset: 0x0000B817
		public void SetComboCount(int theCount, int theScore)
		{
			this.mComboCount = theCount;
			this.mComboScore = theScore;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000D627 File Offset: 0x0000B827
		public void SetBackwardsCount(int theCount)
		{
			this.mBackwardsCount = theCount;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000D630 File Offset: 0x0000B830
		public void SetBackwardsSpeed(float theSpeed)
		{
			this.mBackwardsSpeed = theSpeed;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000D639 File Offset: 0x0000B839
		public void SetNeedCheckCollision(bool needCheck)
		{
			this.mNeedCheckCollision = needCheck;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000D642 File Offset: 0x0000B842
		public void SetSuckPending(bool pending, bool compact)
		{
			this.mSuckPending = pending;
			this.mSuckFromCompacting = compact;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000D652 File Offset: 0x0000B852
		public void SetSuckPending(bool pending)
		{
			this.SetSuckPending(pending, false);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000D65C File Offset: 0x0000B85C
		public void SetGapBonus(int theBonus, int theNumGaps)
		{
			this.mGapBonus = (ushort)theBonus;
			this.mNumGaps = (ushort)theNumGaps;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000D66E File Offset: 0x0000B86E
		public void SetRadius(float r)
		{
			this.mRadius = r;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000D677 File Offset: 0x0000B877
		public void SetIsCannon(bool isCannon)
		{
			this.mIsCannon = isCannon;
			this.mCannonFrame = 0;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000D687 File Offset: 0x0000B887
		public void SetSpeedy(bool speedy)
		{
			this.mSpeedy = speedy;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000D690 File Offset: 0x0000B890
		public void SetPowerCount(int c)
		{
			this.mPowerCount = c;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000D699 File Offset: 0x0000B899
		public bool GetSuckBack()
		{
			return this.mSuckBack;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000D6A1 File Offset: 0x0000B8A1
		public bool GetSpeedy()
		{
			return this.mSpeedy;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000D6AC File Offset: 0x0000B8AC
		public bool Contains(int x, int y)
		{
			x -= (int)this.mX;
			y -= (int)this.mY;
			int num = this.GetRadius() - 3;
			return x * x + y * y < num * num;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000D6E7 File Offset: 0x0000B8E7
		public bool GetShouldRemove()
		{
			return this.mShouldRemove;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000D6EF File Offset: 0x0000B8EF
		public bool GetIsExploding()
		{
			return this.mExploding;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000D6F7 File Offset: 0x0000B8F7
		public bool GetIsCannon()
		{
			return this.mIsCannon;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000D6FF File Offset: 0x0000B8FF
		public static int GetIdGen()
		{
			return Ball.mIdGen;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000D706 File Offset: 0x0000B906
		public float GetX()
		{
			return this.mX;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000D70E File Offset: 0x0000B90E
		public float GetY()
		{
			return this.mY;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000D716 File Offset: 0x0000B916
		public float GetWayPoint()
		{
			return this.mWayPoint;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000D71E File Offset: 0x0000B91E
		public int GetColorType()
		{
			return this.mColorType;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000D726 File Offset: 0x0000B926
		public float GetRotation()
		{
			return this.mRotation;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000D72E File Offset: 0x0000B92E
		public float GetDestRotation()
		{
			return this.mDestRotation;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000D736 File Offset: 0x0000B936
		public Bullet GetBullet()
		{
			return this.mBullet;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000D73E File Offset: 0x0000B93E
		public bool GetCollidesWithNext()
		{
			return this.mCollidesWithNext;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000D746 File Offset: 0x0000B946
		public bool GetShrinkClear()
		{
			return this.mShrinkClear;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000D74E File Offset: 0x0000B94E
		public bool HasOverlays()
		{
			return this.mPowerType != PowerType.PowerType_Max || Enumerable.Count<Component>(this.mElectricOverlay) > 0 || Enumerable.Count<Component>(this.mElectricExplodeOverlay.mLayer1Alpha) > 0 || this.mMultFX != null;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000D789 File Offset: 0x0000B989
		public bool HasUnderlays()
		{
			return (this.mDoLaserAnim && !this.mExploding) || this.mMultFX != null;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000D7A9 File Offset: 0x0000B9A9
		public int GetSuckCount()
		{
			return this.mSuckCount;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000D7B1 File Offset: 0x0000B9B1
		public int GetComboCount()
		{
			return this.mComboCount;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000D7B9 File Offset: 0x0000B9B9
		public int GetComboScore()
		{
			return this.mComboScore;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000D7C1 File Offset: 0x0000B9C1
		public int GetBackwardsCount()
		{
			return this.mBackwardsCount;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000D7C9 File Offset: 0x0000B9C9
		public float GetBackwardsSpeed()
		{
			return this.mBackwardsSpeed;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000D7D1 File Offset: 0x0000B9D1
		public bool GetNeedCheckCollision()
		{
			return this.mNeedCheckCollision;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000D7D9 File Offset: 0x0000B9D9
		public bool GetSuckPending()
		{
			return this.mSuckPending;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000D7E1 File Offset: 0x0000B9E1
		public bool GetSuckFromCompacting()
		{
			return this.mSuckFromCompacting;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000D7E9 File Offset: 0x0000B9E9
		public PowerType GetPowerType()
		{
			return this.mPowerType;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000D7F1 File Offset: 0x0000B9F1
		public PowerType GetDestPowerType()
		{
			return this.mDestPowerType;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000D7F9 File Offset: 0x0000B9F9
		public int GetListItr()
		{
			if (this.mList == null)
			{
				return -1;
			}
			return this.mList.IndexOf(this);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000D811 File Offset: 0x0000BA11
		public int GetPowerCount()
		{
			return this.mPowerCount;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000D819 File Offset: 0x0000BA19
		public int GetGapBonus()
		{
			return (int)this.mGapBonus;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000D821 File Offset: 0x0000BA21
		public int GetNumGaps()
		{
			return (int)this.mNumGaps;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000D829 File Offset: 0x0000BA29
		public int GetStartFrame()
		{
			return this.mStartFrame;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000D831 File Offset: 0x0000BA31
		public int GetId()
		{
			return this.mId;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000D839 File Offset: 0x0000BA39
		public SexyVector2 GetPos()
		{
			return new SexyVector2(this.mX, this.mY);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000D84C File Offset: 0x0000BA4C
		public int GetRadius()
		{
			return (int)this.mRadius;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000D855 File Offset: 0x0000BA55
		public bool GetInTunnel()
		{
			return this.mInTunnel;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000D860 File Offset: 0x0000BA60
		private static BlendedImage CreateBlendedPowerup(int thePowerupType, int theType, Image theImage, int cel)
		{
			int num = theType;
			if (GameApp.gApp.mColorblind && theType == 3)
			{
				num = 6;
			}
			else if (GameApp.gApp.mColorblind && theType == 4)
			{
				num = 7;
			}
			if (Ball.gBlendedPowerups[thePowerupType, num] == null)
			{
				Rect celRect = theImage.GetCelRect(cel);
				Ball.gBlendedPowerups[thePowerupType, num] = new BlendedImage((MemoryImage)theImage, celRect, false);
			}
			return Ball.gBlendedPowerups[thePowerupType, num];
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000D8D0 File Offset: 0x0000BAD0
		private static BlendedImage CreateBlendedBall(int theType)
		{
			ResID id = ResID.IMAGE_BLUE_BALL + theType;
			int num = theType;
			if (GameApp.gApp.mColorblind && theType == 3)
			{
				id = ResID.IMAGE_GREEN_BALL_CBM;
				num = 6;
			}
			else if (GameApp.gApp.mColorblind && theType == 4)
			{
				num = 7;
				id = ResID.IMAGE_PURPLE_BALL_CBM;
			}
			if (Ball.gBlendedBalls[num] == null)
			{
				MemoryImage memoryImage = (MemoryImage)Res.GetImageByID(id);
				int num2 = memoryImage.mWidth / memoryImage.mNumCols;
				int num3 = memoryImage.mHeight / memoryImage.mNumRows;
				int theCel = memoryImage.mNumRows / 2;
				Rect celRect = memoryImage.GetCelRect(theCel);
				Ball.gBlendedBalls[num] = new BlendedImage(memoryImage, celRect, false);
			}
			return Ball.gBlendedBalls[num];
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000D974 File Offset: 0x0000BB74
		private static int GetMultAlpha(int cel)
		{
			int num = ZumasRevenge.Common._M(255);
			int num2 = ZumasRevenge.Common._M(5);
			int num3 = num;
			Image imageByID = Res.GetImageByID(ResID.IMAGE_MULTIPLIER_BALL_BLUE);
			int num4 = imageByID.mNumRows * imageByID.mNumCols - num2;
			if (cel < num2)
			{
				num3 = num / num2 * cel;
			}
			else if (cel > num4)
			{
				num3 = num - num / num2 * (cel - num4);
			}
			if (num3 > num)
			{
				num3 = num;
			}
			else if (num3 < 0)
			{
				num3 = 0;
			}
			return num3;
		}

		// Token: 0x0400008C RID: 140
		protected static int mIdGen = 0;

		// Token: 0x0400008D RID: 141
		protected static int mLaserAnimCel;

		// Token: 0x0400008E RID: 142
		protected Transform mGlobalTransform = new Transform();

		// Token: 0x0400008F RID: 143
		public static SexyColor[] gOverlayColors = new SexyColor[]
		{
			new SexyColor(0, 0, 255),
			new SexyColor(255, 255, 0),
			new SexyColor(255, 0, 0),
			new SexyColor(0, 255, 0),
			new SexyColor(255, 0, 255),
			new SexyColor(255, 255, 255)
		};

		// Token: 0x04000090 RID: 144
		public static string[] fx_files = new string[] { "PIEFFECT_NONRESIZE_BPI", "PIEFFECT_NONRESIZE_YPI", "PIEFFECT_NONRESIZE_RPI", "PIEFFECT_NONRESIZE_GPI", "PIEFFECT_NONRESIZE_PPI", "PIEFFECT_NONRESIZE_WPI" };

		// Token: 0x04000091 RID: 145
		private static BlendedImage[] gBlendedBalls = new BlendedImage[8];

		// Token: 0x04000092 RID: 146
		private static BlendedImage[,] gBlendedPowerups = new BlendedImage[15, 8];

		// Token: 0x04000093 RID: 147
		private static BlendedImage[] gBlendedPowerupLights = new BlendedImage[14];

		// Token: 0x04000094 RID: 148
		private static BlendedImage[] gBlendedBombLights = new BlendedImage[6];

		// Token: 0x04000095 RID: 149
		protected bool mInTunnel;

		// Token: 0x04000096 RID: 150
		protected int mMultOverlayAlpha;

		// Token: 0x04000097 RID: 151
		protected PIEffect mMultFX;

		// Token: 0x04000098 RID: 152
		protected int mId;

		// Token: 0x04000099 RID: 153
		protected int mColorType;

		// Token: 0x0400009A RID: 154
		protected int mDisplayType;

		// Token: 0x0400009B RID: 155
		protected float mWayPoint;

		// Token: 0x0400009C RID: 156
		protected float mLastWayPoint;

		// Token: 0x0400009D RID: 157
		protected float mRotation;

		// Token: 0x0400009E RID: 158
		protected float mDestRotation;

		// Token: 0x0400009F RID: 159
		protected float mRotationInc;

		// Token: 0x040000A0 RID: 160
		protected float mX;

		// Token: 0x040000A1 RID: 161
		protected float mY;

		// Token: 0x040000A2 RID: 162
		protected float mLastX;

		// Token: 0x040000A3 RID: 163
		protected float mLastY;

		// Token: 0x040000A4 RID: 164
		protected float mDrawScale;

		// Token: 0x040000A5 RID: 165
		protected float mRadius;

		// Token: 0x040000A6 RID: 166
		protected int mPulseState;

		// Token: 0x040000A7 RID: 167
		protected int mPulseTimer;

		// Token: 0x040000A8 RID: 168
		private List<Component> mOverlayPulse = new List<Component>();

		// Token: 0x040000A9 RID: 169
		private List<Component> mElectricOverlay = new List<Component>();

		// Token: 0x040000AA RID: 170
		private ElectricExplodeOverlay mElectricExplodeOverlay = new ElectricExplodeOverlay();

		// Token: 0x040000AB RID: 171
		protected int mElectricOverlayCel;

		// Token: 0x040000AC RID: 172
		protected List<Ball> mList;

		// Token: 0x040000AD RID: 173
		protected CurveMgr mCurve;

		// Token: 0x040000AE RID: 174
		protected bool mCollidesWithNext;

		// Token: 0x040000AF RID: 175
		protected bool mNeedCheckCollision;

		// Token: 0x040000B0 RID: 176
		protected bool mSuckPending;

		// Token: 0x040000B1 RID: 177
		protected bool mShrinkClear;

		// Token: 0x040000B2 RID: 178
		protected bool mSuckFromCompacting;

		// Token: 0x040000B3 RID: 179
		protected bool mExplodingInTunnel;

		// Token: 0x040000B4 RID: 180
		protected bool mExploding;

		// Token: 0x040000B5 RID: 181
		protected bool mExplodingFromLightning;

		// Token: 0x040000B6 RID: 182
		protected int mExplodeFrame;

		// Token: 0x040000B7 RID: 183
		protected bool mShouldRemove;

		// Token: 0x040000B8 RID: 184
		protected bool mSpeedy;

		// Token: 0x040000B9 RID: 185
		protected bool mSuckBack;

		// Token: 0x040000BA RID: 186
		protected int mPowerGracePeriod;

		// Token: 0x040000BB RID: 187
		protected PowerType mLastPowerType;

		// Token: 0x040000BC RID: 188
		protected int mCannonFrame;

		// Token: 0x040000BD RID: 189
		protected bool mIsCannon;

		// Token: 0x040000BE RID: 190
		protected bool mDoLaserAnim;

		// Token: 0x040000BF RID: 191
		protected int mUpdateCount;

		// Token: 0x040000C0 RID: 192
		protected int mCel;

		// Token: 0x040000C1 RID: 193
		public Bullet mBullet;

		// Token: 0x040000C2 RID: 194
		protected int mSuckCount;

		// Token: 0x040000C3 RID: 195
		protected int mBackwardsCount;

		// Token: 0x040000C4 RID: 196
		protected float mBackwardsSpeed;

		// Token: 0x040000C5 RID: 197
		protected int mComboCount;

		// Token: 0x040000C6 RID: 198
		protected int mComboScore;

		// Token: 0x040000C7 RID: 199
		protected int mStartFrame;

		// Token: 0x040000C8 RID: 200
		protected int mPowerCount;

		// Token: 0x040000C9 RID: 201
		protected int mPowerFade;

		// Token: 0x040000CA RID: 202
		protected ushort mGapBonus;

		// Token: 0x040000CB RID: 203
		protected ushort mNumGaps;

		// Token: 0x040000CC RID: 204
		protected float mIconAppearScale;

		// Token: 0x040000CD RID: 205
		protected float mIconScaleRate;

		// Token: 0x040000CE RID: 206
		protected int mIconCel;

		// Token: 0x040000CF RID: 207
		protected int mMultBallCel;

		// Token: 0x040000D0 RID: 208
		protected int mMultBallCel2;

		// Token: 0x040000D1 RID: 209
		protected List<Ball.Particle> mParticles;

		// Token: 0x040000D2 RID: 210
		protected PowerType mPowerType;

		// Token: 0x040000D3 RID: 211
		protected PowerType mDestPowerType;

		// Token: 0x040000D4 RID: 212
		public bool mHilightPulse;

		// Token: 0x040000D5 RID: 213
		public bool mDebugDrawID;

		// Token: 0x040000D6 RID: 214
		public bool mDoBossPulse;

		// Token: 0x040000D7 RID: 215
		public int mBossBlinkTimer;

		// Token: 0x040000D8 RID: 216
		public int mLastFrame;

		// Token: 0x040000D9 RID: 217
		public Gun mFrog;

		// Token: 0x02000012 RID: 18
		protected struct Particle
		{
			// Token: 0x040000DA RID: 218
			public float x;

			// Token: 0x040000DB RID: 219
			public float y;

			// Token: 0x040000DC RID: 220
			public float vx;

			// Token: 0x040000DD RID: 221
			public float vy;

			// Token: 0x040000DE RID: 222
			public int mSize;
		}
	}
}
