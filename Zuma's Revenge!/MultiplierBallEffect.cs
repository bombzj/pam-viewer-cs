using System;
using System.Collections.Generic;
using JeffLib;
using Microsoft.Xna.Framework;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.PIL;

namespace ZumasRevenge
{
	// Token: 0x020000B0 RID: 176
	public class MultiplierBallEffect
	{
		// Token: 0x06000A64 RID: 2660 RVA: 0x0006260C File Offset: 0x0006080C
		protected void InitSpawnEffects()
		{
			if (this.mMultBall == null)
			{
				return;
			}
			this.mSpawnEffect = new SpawnEffect();
			SexyFramework.PIL.System system = this.mSpawnEffect.mRings;
			system.SetLife(ZumasRevenge.Common._M(100));
			Emitter emitter = new Emitter();
			emitter.mDeleteInvisParticles = true;
			emitter.mCullingRect = new Rect(0, 0, ZumasRevenge.Common._SS(GlobalMembers.gSexyAppBase.mWidth), ZumasRevenge.Common._SS(GlobalMembers.gSexyAppBase.mHeight));
			EmitterScale emitterScale = new EmitterScale();
			emitter.mTintColor = MultiplierBallEffect.gSpawnColors[this.mMultBall.GetColorType()].mRings;
			emitterScale.mLifeScale = ZumasRevenge.Common._M(0.125f);
			emitterScale.mNumberScale = ZumasRevenge.Common._M(0.96f);
			emitterScale.mSizeXScale = ZumasRevenge.Common._M(0.5f);
			emitterScale.mVelocityScale = ZumasRevenge.Common._M(6.52f);
			emitter.AddScaleKeyFrame(0, emitterScale);
			emitterScale = new EmitterScale(emitterScale);
			emitterScale.mSizeXScale = ZumasRevenge.Common._M(1.87f);
			emitter.AddScaleKeyFrame(ZumasRevenge.Common._M(52), emitterScale);
			EmitterSettings emitterSettings = new EmitterSettings();
			emitterSettings.mTintStrength = 1f;
			emitterSettings.mVisibility = ZumasRevenge.Common._M(0.76f);
			emitter.AddSettingsKeyFrame(0, emitterSettings);
			emitterSettings = new EmitterSettings(emitterSettings);
			emitterSettings.mVisibility = ZumasRevenge.Common._M(0.69f);
			emitter.AddSettingsKeyFrame(ZumasRevenge.Common._M(21), emitterSettings);
			emitterSettings = new EmitterSettings(emitterSettings);
			emitterSettings.mVisibility = 0f;
			emitter.AddSettingsKeyFrame(ZumasRevenge.Common._M(60), emitterSettings);
			ParticleType particleType = new ParticleType();
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_RING);
			particleType.mAdditive = true;
			particleType.mEmitterAttachPct = 1f;
			particleType.mColorKeyManager.AddColorKey(0f, SexyColor.White);
			particleType.mColorKeyManager.AddColorKey(1f, SexyColor.Black);
			particleType.mAlphaKeyManager.AddAlphaKey(0f, 255);
			particleType.mAlphaKeyManager.AddAlphaKey(ZumasRevenge.Common._M(0.5f), 255);
			particleType.mAlphaKeyManager.AddAlphaKey(1f, 0);
			particleType.AddSettingsKeyFrame(0, new ParticleSettings
			{
				mLife = ZumasRevenge.Common._M(96),
				mNumber = (int)((float)ZumasRevenge.Common._M(6) * MultiplierBallEffect.MULTIPLIER_BALL_EFFECT_PARTICLE_COUNT_SCALE),
				mXSize = ZumasRevenge.Common._M(69)
			});
			LifetimeSettings lifetimeSettings = new LifetimeSettings();
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(2f);
			lifetimeSettings.mVelocityMult = ZumasRevenge.Common._M(1.6f);
			particleType.AddSettingAtLifePct(0f, lifetimeSettings);
			particleType.AddSettingAtLifePct(1f, new LifetimeSettings(lifetimeSettings)
			{
				mVelocityMult = ZumasRevenge.Common._M(1f)
			});
			emitter.AddParticleType(particleType);
			system.AddEmitter(emitter);
			system = this.mSpawnEffect.mSwirl;
			system.SetLife(ZumasRevenge.Common._M(56));
			emitter = new Emitter();
			emitter.mDeleteInvisParticles = true;
			emitter.mEmissionCoordsAreOffsets = true;
			emitter.mCullingRect = new Rect(0, 0, ZumasRevenge.Common._SS(GlobalMembers.gSexyAppBase.mWidth), ZumasRevenge.Common._SS(GlobalMembers.gSexyAppBase.mHeight));
			emitterScale = new EmitterScale();
			emitter.mTintColor = MultiplierBallEffect.gSpawnColors[this.mMultBall.GetColorType()].mSwirl;
			emitter.SetEmitterType(2);
			emitter.mEmitDir = 1;
			emitter.mEmitAtXPoints = ZumasRevenge.Common._M(20);
			emitter.mLinearEmitAtPoints = true;
			emitterScale.mLifeScale = ZumasRevenge.Common._M(0.1f);
			emitterScale.mNumberScale = ZumasRevenge.Common._M(20f);
			emitterScale.mSizeXScale = ZumasRevenge.Common._M(5f);
			emitterScale.mVelocityScale = ZumasRevenge.Common._M(2f);
			emitterScale.mZoom = ZumasRevenge.Common._M(0.5f);
			emitter.AddScaleKeyFrame(0, emitterScale);
			emitterSettings = new EmitterSettings();
			emitterSettings.mTintStrength = ZumasRevenge.Common._M(0.81f);
			emitterSettings.mEmissionAngle = 0f;
			emitterSettings.mEmissionRange = SexyFramework.Common.DegreesToRadians(1f);
			emitterSettings.mXRadius = (emitterSettings.mYRadius = 5f);
			emitter.AddSettingsKeyFrame(0, emitterSettings);
			particleType = new ParticleType();
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_STARBURST);
			particleType.mAdditive = true;
			particleType.mEmitterAttachPct = 1f;
			particleType.mColorKeyManager.AddColorKey(0f, SexyColor.White);
			particleType.mColorKeyManager.AddColorKey(ZumasRevenge.Common._M(0.9f), SexyColor.White);
			particleType.mColorKeyManager.AddColorKey(1f, SexyColor.Black);
			particleType.mAlphaKeyManager.AddAlphaKey(0f, 255);
			particleType.mAlphaKeyManager.AddAlphaKey(ZumasRevenge.Common._M(0.9f), 255);
			particleType.mAlphaKeyManager.AddAlphaKey(1f, 0);
			ParticleSettings particleSettings = new ParticleSettings();
			particleSettings.mLife = ZumasRevenge.Common._M(70);
			particleSettings.mNumber = (int)((float)ZumasRevenge.Common._M(10) * MultiplierBallEffect.MULTIPLIER_BALL_EFFECT_PARTICLE_COUNT_SCALE);
			particleSettings.mXSize = ZumasRevenge.Common._M(11);
			particleSettings.mVelocity = ZumasRevenge.Common._M(83);
			particleSettings.mWeight = (float)ZumasRevenge.Common._M(2);
			particleType.AddSettingsKeyFrame(0, particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mLife = ZumasRevenge.Common._M(70);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(10), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mLife = 0;
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(22), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mGlobalVisibility = 1f;
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(48), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mGlobalVisibility = 0f;
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(56), particleSettings);
			lifetimeSettings = new LifetimeSettings();
			lifetimeSettings.mVelocityMult = ZumasRevenge.Common._M(2f);
			particleType.AddSettingAtLifePct(0f, lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			particleType.AddSettingAtLifePct(ZumasRevenge.Common._M(0.78f), lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(2f);
			lifetimeSettings.mVelocityMult = ZumasRevenge.Common._M(1.7f);
			particleType.AddSettingAtLifePct(ZumasRevenge.Common._M(0.92f), lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			lifetimeSettings.mVelocityMult = ZumasRevenge.Common._M(1.65f);
			particleType.AddSettingAtLifePct(ZumasRevenge.Common._M(0.94f), lifetimeSettings);
			particleType.AddSettingAtLifePct(1f, new LifetimeSettings(lifetimeSettings)
			{
				mVelocityMult = 0f
			});
			emitter.AddParticleType(particleType);
			system.AddEmitter(emitter);
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00062CA8 File Offset: 0x00060EA8
		protected void UpdateStateSpawn()
		{
			this.mSpawnTimer++;
			if (this.mSpawnTimer % ZumasRevenge.Common._M(30) == 0 && this.mMultBall != null && this.mSpawnTimer < ZumasRevenge.Common._M1(50))
			{
				AlphaFadeInfo alphaFadeInfo = new AlphaFadeInfo();
				alphaFadeInfo.second = false;
				alphaFadeInfo.first = new AlphaFader();
				alphaFadeInfo.first.mColor = new FColor(MultiplierBallEffect.gSpawnColors[this.mMultBall.GetColorType()].mBeam);
				alphaFadeInfo.first.mColor.mAlpha = 0f;
				alphaFadeInfo.first.mMin = 0;
				alphaFadeInfo.first.mMax = 255;
				alphaFadeInfo.first.mFadeRate = ZumasRevenge.Common._M(6f);
				this.mBeamAlphas.Add(alphaFadeInfo);
			}
			for (int i = 0; i < this.mBeamAlphas.size<AlphaFadeInfo>(); i++)
			{
				AlphaFadeInfo alphaFadeInfo2 = this.mBeamAlphas[i];
				alphaFadeInfo2.first.Update();
				if (!alphaFadeInfo2.second && SexyFramework.Common._eq(alphaFadeInfo2.first.mColor.mAlpha, (float)alphaFadeInfo2.first.mMax))
				{
					alphaFadeInfo2.second = true;
					alphaFadeInfo2.first.mFadeRate = ZumasRevenge.Common._M(-10f);
				}
				else if (alphaFadeInfo2.second && SexyFramework.Common._leq(alphaFadeInfo2.first.mColor.mAlpha, (float)alphaFadeInfo2.first.mMin))
				{
					this.mBeamAlphas.RemoveAt(i);
					i--;
				}
			}
			if (this.mSpawnTimer >= ZumasRevenge.Common._M(50))
			{
				this.mSpawnEffect.mRings.SetPos(this.mLastBallX, this.mLastBallY);
				this.mSpawnEffect.mRings.Update();
				this.mSpawnEffect.mSwirl.SetPos(this.mLastBallX, this.mLastBallY);
				this.mSpawnEffect.mSwirl.Update();
			}
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00062EA4 File Offset: 0x000610A4
		protected void DrawStateSpawn(Graphics g)
		{
			if (this.mSpawnTimer >= ZumasRevenge.Common._M(50))
			{
				this.mSpawnEffect.mRings.Draw(g);
				this.mSpawnEffect.mSwirl.Draw(g);
			}
			int num = ZumasRevenge.Common._M(228);
			int num2 = ZumasRevenge.Common._M(21);
			float rot = SexyFramework.Common.AngleBetweenPoints(this.mLastBallX, this.mLastBallY, (float)num, (float)num2) - SexyFramework.Common.JL_PI / 2f;
			Image imageByID = Res.GetImageByID(ResID.IMAGE_PARTICLE_BEAM);
			int mWidth = imageByID.mWidth;
			int mHeight = imageByID.mHeight;
			float num3 = ZumasRevenge.Common._S(SexyFramework.Common.Distance(this.mLastBallX, this.mLastBallY, (float)num, (float)num2) + (float)ZumasRevenge.Common._M(17));
			float num4 = num3 / (float)mHeight;
			float num5 = (float)mHeight * num4;
			this.mGlobalTranform.Reset();
			this.mGlobalTranform.Scale(1f, num4);
			this.mGlobalTranform.Translate((float)ZumasRevenge.Common._M(0), num5 / 2f);
			this.mGlobalTranform.RotateRad(rot);
			this.mGlobalTranform.Translate((float)ZumasRevenge.Common._M(0), -num5 / 2f);
			for (int i = 0; i < this.mBeamAlphas.size<AlphaFadeInfo>(); i++)
			{
				g.SetColorizeImages(true);
				g.SetColor(this.mBeamAlphas[i].first.mColor);
				g.DrawImageTransform(imageByID, this.mGlobalTranform, (float)ZumasRevenge.Common._S(num), (float)ZumasRevenge.Common._S(num2) + num3 / 2f);
				g.SetDrawMode(1);
				g.DrawImageTransform(imageByID, this.mGlobalTranform, (float)ZumasRevenge.Common._S(num), (float)ZumasRevenge.Common._S(num2) + num3 / 2f);
				g.SetDrawMode(0);
				g.SetColorizeImages(false);
			}
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0006306C File Offset: 0x0006126C
		protected void InitTriggeredEffects()
		{
			if (this.mTriggeredEffect != null)
			{
				return;
			}
			this.mTriggeredEffect = new TriggeredEffect();
			SexyFramework.PIL.System system = this.mTriggeredEffect.mRings;
			system.SetLife(ZumasRevenge.Common._M(44));
			Emitter emitter = new Emitter();
			emitter.mDeleteInvisParticles = true;
			emitter.mCullingRect = new Rect(0, 0, ZumasRevenge.Common._SS(GlobalMembers.gSexyAppBase.mWidth), ZumasRevenge.Common._SS(GlobalMembers.gSexyAppBase.mHeight));
			emitter.mDeleteInvisParticles = true;
			EmitterScale emitterScale = new EmitterScale();
			emitterScale.mLifeScale = ZumasRevenge.Common._M(0.3f);
			emitterScale.mNumberScale = ZumasRevenge.Common._M(1.06f);
			emitterScale.mSizeXScale = ZumasRevenge.Common._M(1.55f);
			emitterScale.mVelocityScale = ZumasRevenge.Common._M(0.79f);
			emitter.AddScaleKeyFrame(0, emitterScale);
			emitterScale = new EmitterScale(emitterScale);
			emitterScale.mSizeXScale = ZumasRevenge.Common._M(2.79f);
			emitter.AddScaleKeyFrame(ZumasRevenge.Common._M(29), emitterScale);
			EmitterSettings emitterSettings = new EmitterSettings();
			emitterSettings.mVisibility = ZumasRevenge.Common._M(0.36f);
			emitter.AddSettingsKeyFrame(0, emitterSettings);
			emitterSettings = new EmitterSettings(emitterSettings);
			emitterSettings.mVisibility = ZumasRevenge.Common._M(1f);
			emitter.AddSettingsKeyFrame(ZumasRevenge.Common._M(19), emitterSettings);
			emitterSettings = new EmitterSettings(emitterSettings);
			emitterSettings.mVisibility = 0f;
			emitter.AddSettingsKeyFrame(ZumasRevenge.Common._M(44), emitterSettings);
			ParticleType particleType = new ParticleType();
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_RING);
			particleType.mName = "Rings";
			particleType.mAdditive = true;
			particleType.mEmitterAttachPct = 1f;
			particleType.mColorKeyManager.AddColorKey(0f, new SexyColor(96, 255, 139));
			particleType.mColorKeyManager.AddColorKey(0.12f, new SexyColor(213, 255, 87));
			particleType.mColorKeyManager.AddColorKey(0.28f, new SexyColor(255, 0, 0));
			particleType.mColorKeyManager.AddColorKey(0.54f, new SexyColor(0, 72, 255));
			particleType.mColorKeyManager.AddColorKey(1f, new SexyColor(12, 0, 255));
			particleType.mAlphaKeyManager.AddAlphaKey(0f, 255);
			particleType.mAlphaKeyManager.AddAlphaKey(ZumasRevenge.Common._M(0.6f), 255);
			particleType.mAlphaKeyManager.AddAlphaKey(1f, 0);
			particleType.AddSettingsKeyFrame(0, new ParticleSettings
			{
				mLife = ZumasRevenge.Common._M(6),
				mNumber = (int)((float)ZumasRevenge.Common._M(10) * MultiplierBallEffect.MULTIPLIER_BALL_EFFECT_PARTICLE_COUNT_SCALE),
				mXSize = ZumasRevenge.Common._S(ZumasRevenge.Common._M(69))
			});
			particleType.AddSettingAtLifePct(0f, new LifetimeSettings
			{
				mSizeXMult = ZumasRevenge.Common._M(0.38f),
				mVelocityMult = ZumasRevenge.Common._M(1.7f)
			});
			particleType.AddSettingAtLifePct(1f, new LifetimeSettings
			{
				mSizeXMult = ZumasRevenge.Common._M(1.4f),
				mVelocityMult = ZumasRevenge.Common._M(1f)
			});
			emitter.AddParticleType(particleType);
			system.AddEmitter(emitter);
			int num = ZumasRevenge.Common._M(100);
			system = this.mTriggeredEffect.mRainbow;
			system.SetLife(num);
			emitter = new Emitter();
			emitter.mDeleteInvisParticles = true;
			emitter.mCullingRect = new Rect(0, 0, ZumasRevenge.Common._SS(GlobalMembers.gSexyAppBase.mWidth), ZumasRevenge.Common._SS(GlobalMembers.gSexyAppBase.mHeight));
			emitterScale = new EmitterScale();
			emitterScale.mLifeScale = ZumasRevenge.Common._M(1f);
			emitterScale.mNumberScale = ZumasRevenge.Common._M(0.5f);
			emitterScale.mSpinScale = ZumasRevenge.Common._M(0.19f);
			emitterScale.mSizeXScale = ZumasRevenge.Common._M(1.24f);
			emitterScale.mSizeYScale = ZumasRevenge.Common._M(0.9f);
			emitter.AddScaleKeyFrame(0, emitterScale);
			emitterScale = new EmitterScale(emitterScale);
			emitter.AddScaleKeyFrame(ZumasRevenge.Common._M(9), emitterScale);
			emitterScale = new EmitterScale(emitterScale);
			emitterScale.mSizeXScale = ZumasRevenge.Common._M(1.24f);
			emitterScale.mSizeYScale = ZumasRevenge.Common._M(0.71f);
			emitter.AddScaleKeyFrame(ZumasRevenge.Common._M(15), emitterScale);
			emitterScale = new EmitterScale(emitterScale);
			emitterScale.mSizeXScale = ZumasRevenge.Common._M(1f);
			emitterScale.mSizeYScale = 0f;
			emitter.AddScaleKeyFrame(ZumasRevenge.Common._M(33), emitterScale);
			emitter.AddScaleKeyFrame(num, new EmitterScale(emitterScale)
			{
				mSizeXScale = 0f
			});
			emitter.AddSettingsKeyFrame(0, new EmitterSettings
			{
				mVisibility = ZumasRevenge.Common._M(0.58f)
			});
			emitterSettings = new EmitterSettings();
			emitterSettings.mVisibility = ZumasRevenge.Common._M(1f);
			emitter.AddSettingsKeyFrame(ZumasRevenge.Common._M(7), emitterSettings);
			emitterSettings = new EmitterSettings();
			emitter.AddSettingsKeyFrame(ZumasRevenge.Common._M(19), emitterSettings);
			emitterSettings = new EmitterSettings();
			emitterSettings.mVisibility = ZumasRevenge.Common._M(0.29f);
			emitter.AddSettingsKeyFrame(ZumasRevenge.Common._M(50), emitterSettings);
			emitter.AddSettingsKeyFrame(num, new EmitterSettings
			{
				mVisibility = 0f
			});
			particleType = new ParticleType();
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_BEAM);
			particleType.mName = "Rainbow beam crap";
			particleType.mAdditive = true;
			particleType.mAdditiveWithNormal = true;
			particleType.mRefYOff = (int)(4f * (float)ZumasRevenge.Common._M(-300));
			particleType.mInitAngle = 3.1415927f;
			particleType.mAngleRange = 6.2831855f;
			particleType.mInitAngleStep = ZumasRevenge.Common._M(0.5f);
			particleType.mLockSizeAspect = false;
			particleType.mAlphaKeyManager.AddAlphaKey(0f, 0);
			particleType.mAlphaKeyManager.AddAlphaKey(ZumasRevenge.Common._M(0.25f), 255);
			particleType.mAlphaKeyManager.AddAlphaKey(ZumasRevenge.Common._M(0.75f), 255);
			particleType.mAlphaKeyManager.AddAlphaKey(1f, 0);
			particleType.mColorKeyManager.AddColorKey(0f, new SexyColor(0, 220, 250));
			particleType.mColorKeyManager.AddColorKey(0.25f, new SexyColor(51, 0, 255));
			particleType.mColorKeyManager.AddColorKey(0.375f, new SexyColor(225, 0, 255));
			particleType.mColorKeyManager.AddColorKey(0.5f, new SexyColor(255, 0, 0));
			particleType.mColorKeyManager.AddColorKey(0.675f, new SexyColor(225, 123, 0));
			particleType.mColorKeyManager.AddColorKey(0.75f, new SexyColor(7, 255, 12));
			particleType.mColorKeyManager.AddColorKey(1f, new SexyColor(229, 255, 0));
			particleType.AddSettingsKeyFrame(0, new ParticleSettings
			{
				mLife = ZumasRevenge.Common._M(9),
				mNumber = (int)((float)ZumasRevenge.Common._M(80) * MultiplierBallEffect.MULTIPLIER_BALL_EFFECT_PARTICLE_COUNT_SCALE),
				mXSize = ZumasRevenge.Common._M(50),
				mYSize = ZumasRevenge.Common._M(159),
				mSpin = SexyFramework.Common.DegreesToRadians((float)ZumasRevenge.Common._M(-70))
			});
			particleType.AddVarianceKeyFrame(0, new ParticleVariance
			{
				mSpinVar = SexyFramework.Common.DegreesToRadians((float)ZumasRevenge.Common._M(68))
			});
			LifetimeSettings lifetimeSettings = new LifetimeSettings();
			lifetimeSettings.mSizeXMult = 1f;
			particleType.AddSettingAtLifePct(0f, lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			particleType.AddSettingAtLifePct(0.7f, lifetimeSettings);
			particleType.AddSettingAtLifePct(1f, new LifetimeSettings(lifetimeSettings)
			{
				mSizeXMult = 0f
			});
			emitter.AddParticleType(particleType);
			system.AddEmitter(emitter);
			system = this.mTriggeredEffect.mGas;
			system.SetLife(ZumasRevenge.Common._M(200));
			emitter = new Emitter();
			emitter.mDeleteInvisParticles = true;
			emitter.mPreloadFrames = ZumasRevenge.Common._M(0);
			emitter.mCullingRect = new Rect(0, 0, ZumasRevenge.Common._SS(GlobalMembers.gSexyAppBase.mWidth), ZumasRevenge.Common._SS(GlobalMembers.gSexyAppBase.mHeight));
			emitterScale = new EmitterScale();
			emitterScale.mLifeScale = ZumasRevenge.Common._M(0.56f);
			emitterScale.mNumberScale = ZumasRevenge.Common._M(0.45f);
			emitterScale.mSizeXScale = ZumasRevenge.Common._M(2.87f);
			emitterScale.mVelocityScale = ZumasRevenge.Common._M(0.18f);
			emitterScale.mZoom = ZumasRevenge.Common._M(0.33f);
			emitterScale.mSpinScale = ZumasRevenge.Common._M(1.75f);
			emitter.AddScaleKeyFrame(0, emitterScale);
			emitterScale = new EmitterScale(emitterScale);
			emitterScale.mZoom = ZumasRevenge.Common._M(0.5f);
			emitter.AddScaleKeyFrame(ZumasRevenge.Common._M(25), emitterScale);
			emitterScale = new EmitterScale(emitterScale);
			emitter.AddScaleKeyFrame(ZumasRevenge.Common._M(75), emitterScale);
			emitterScale = new EmitterScale(emitterScale);
			emitterScale.mVelocityScale = ZumasRevenge.Common._M(0.48f);
			emitter.AddScaleKeyFrame(ZumasRevenge.Common._M(87), emitterScale);
			emitter.AddSettingsKeyFrame(0, new EmitterSettings
			{
				mVisibility = ZumasRevenge.Common._M(0.56f)
			});
			particleType = new ParticleType();
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_SPIKEYCIRCLE);
			particleType.mAdditive = true;
			particleType.mColorKeyManager.AddColorKey(0f, new SexyColor(232, 255, 32));
			particleType.mColorKeyManager.AddColorKey(ZumasRevenge.Common._M(0.375f), new SexyColor(98, 254, 255));
			particleType.mColorKeyManager.AddColorKey(ZumasRevenge.Common._M(0.675f), new SexyColor(255, 101, 206));
			particleType.mColorKeyManager.AddColorKey(1f, new SexyColor(21, 9, 34));
			particleType.mAlphaKeyManager.AddAlphaKey(0f, 255);
			particleType.mAlphaKeyManager.AddAlphaKey(ZumasRevenge.Common._M(0.75f), 255);
			particleType.mAlphaKeyManager.AddAlphaKey(1f, 0);
			particleType.mColorKeyManager.SetColorMode(2);
			particleType.mName = "clouds";
			ParticleSettings particleSettings = new ParticleSettings();
			particleSettings.mLife = 0;
			particleSettings.mNumber = (int)((float)ZumasRevenge.Common._M(10) * MultiplierBallEffect.MULTIPLIER_BALL_EFFECT_PARTICLE_COUNT_SCALE);
			particleSettings.mXSize = ZumasRevenge.Common._M(30);
			particleSettings.mVelocity = ZumasRevenge.Common._M(243);
			particleSettings.mMotionRand = (float)ZumasRevenge.Common._M(57);
			particleSettings.mGlobalVisibility = ZumasRevenge.Common._M(0.56f);
			particleType.AddSettingsKeyFrame(0, particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mVelocity = ZumasRevenge.Common._M(281);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(12), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mLife = ZumasRevenge.Common._M(30);
			particleSettings.mVelocity = ZumasRevenge.Common._M(333);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(30), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mLife = 0;
			particleSettings.mVelocity = ZumasRevenge.Common._M(346);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(39), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mVelocity = ZumasRevenge.Common._M(419);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(89), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mGlobalVisibility = 0f;
			particleSettings.mVelocity = ZumasRevenge.Common._M(435);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(100), particleSettings);
			particleType.AddVarianceKeyFrame(0, new ParticleVariance
			{
				mSizeXVar = ZumasRevenge.Common._M(20),
				mVelocityVar = ZumasRevenge.Common._M(26),
				mWeightVar = ZumasRevenge.Common._M(9)
			});
			lifetimeSettings = new LifetimeSettings();
			lifetimeSettings.mSizeXMult = 0f;
			lifetimeSettings.mVelocityMult = ZumasRevenge.Common._M(0.85f);
			particleType.AddSettingAtLifePct(0f, lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(1.8f);
			lifetimeSettings.mVelocityMult = ZumasRevenge.Common._M(1.2f);
			particleType.AddSettingAtLifePct(ZumasRevenge.Common._M(0.3f), lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(0.3f);
			lifetimeSettings.mVelocityMult = ZumasRevenge.Common._M(1.6f);
			particleType.AddSettingAtLifePct(ZumasRevenge.Common._M(0.68f), lifetimeSettings);
			particleType.AddSettingAtLifePct(1f, new LifetimeSettings(lifetimeSettings)
			{
				mSizeXMult = 0f,
				mVelocityMult = 2f
			});
			emitter.AddParticleType(particleType);
			particleType = new ParticleType();
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_GAS);
			particleType.mRefXOff = (int)(4f * (float)ZumasRevenge.Common._M(-3));
			particleType.mRefYOff = (int)(4f * (float)ZumasRevenge.Common._M(-12));
			particleType.mColorKeyManager.AddColorKey(0f, new SexyColor(255, 0, 0));
			particleType.mColorKeyManager.AddColorKey(0.125f, new SexyColor(148, 0, 255));
			particleType.mColorKeyManager.AddColorKey(0.375f, new SexyColor(0, 33, 255));
			particleType.mColorKeyManager.AddColorKey(0.5f, new SexyColor(7, 222, 255));
			particleType.mColorKeyManager.AddColorKey(0.675f, new SexyColor(0, 255, 42));
			particleType.mColorKeyManager.AddColorKey(0.75f, new SexyColor(9, 156, 26));
			particleType.mColorKeyManager.AddColorKey(0.9f, new SexyColor(255, 144, 0));
			particleType.mColorKeyManager.AddColorKey(1f, new SexyColor(255, 255, 255));
			particleType.mAlphaKeyManager.AddAlphaKey(0f, 255);
			particleType.mAlphaKeyManager.AddAlphaKey(ZumasRevenge.Common._M(0.8f), 255);
			particleType.mAlphaKeyManager.AddAlphaKey(1f, 150);
			particleType.mColorKeyManager.SetColorMode(2);
			particleSettings = new ParticleSettings();
			particleSettings.mLife = ZumasRevenge.Common._M(40);
			particleSettings.mNumber = (int)((float)ZumasRevenge.Common._M(29) * MultiplierBallEffect.MULTIPLIER_BALL_EFFECT_PARTICLE_COUNT_SCALE);
			particleSettings.mXSize = ZumasRevenge.Common._M(60);
			particleSettings.mVelocity = ZumasRevenge.Common._M(3);
			particleSettings.mWeight = (float)ZumasRevenge.Common._M(0);
			particleSettings.mSpin = SexyFramework.Common.DegreesToRadians((float)ZumasRevenge.Common._M(6));
			particleSettings.mGlobalVisibility = 0f;
			particleType.AddSettingsKeyFrame(0, particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mNumber = (int)((float)ZumasRevenge.Common._M(8) * MultiplierBallEffect.MULTIPLIER_BALL_EFFECT_PARTICLE_COUNT_SCALE);
			particleSettings.mGlobalVisibility = ZumasRevenge.Common._M(0.22f);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(12), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mNumber = 0;
			particleSettings.mLife = ZumasRevenge.Common._M(53);
			particleSettings.mGlobalVisibility = ZumasRevenge.Common._M(0.3f);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(16), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mGlobalVisibility = 1f;
			particleSettings.mLife = ZumasRevenge.Common._M(40);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(29), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mLife = ZumasRevenge.Common._M(16);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(51), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mGlobalVisibility = ZumasRevenge.Common._M(0.2f);
			particleSettings.mLife = 0;
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(70), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mGlobalVisibility = 0f;
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(90), particleSettings);
			ParticleVariance particleVariance = new ParticleVariance();
			particleVariance.mLifeVar = ZumasRevenge.Common._M(63);
			particleVariance.mSizeXVar = ZumasRevenge.Common._M(7);
			particleVariance.mWeightVar = ZumasRevenge.Common._M(0);
			particleVariance.mSpinVar = SexyFramework.Common.DegreesToRadians((float)ZumasRevenge.Common._M(30));
			particleVariance.mMotionRandVar = (float)ZumasRevenge.Common._M(0);
			particleType.AddVarianceKeyFrame(0, particleVariance);
			particleVariance = new ParticleVariance(particleVariance);
			particleType.AddVarianceKeyFrame(ZumasRevenge.Common._M(19), particleVariance);
			particleVariance = new ParticleVariance(particleVariance);
			particleVariance.mLifeVar = 0;
			particleType.AddVarianceKeyFrame(ZumasRevenge.Common._M(35), particleVariance);
			lifetimeSettings = new LifetimeSettings();
			lifetimeSettings.mSizeXMult = 1f;
			lifetimeSettings.mVelocityMult = 0f;
			particleType.AddSettingAtLifePct(0f, lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(1.8f);
			lifetimeSettings.mVelocityMult = ZumasRevenge.Common._M(0.6f);
			particleType.AddSettingAtLifePct(0.1f, lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(1.9f);
			lifetimeSettings.mVelocityMult = ZumasRevenge.Common._M(1.2f);
			particleType.AddSettingAtLifePct(0.21f, lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(2f);
			lifetimeSettings.mVelocityMult = ZumasRevenge.Common._M(1.3f);
			particleType.AddSettingAtLifePct(0.28f, lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			lifetimeSettings.mVelocityMult = ZumasRevenge.Common._M(1.4f);
			particleType.AddSettingAtLifePct(0.42f, lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(1f);
			lifetimeSettings.mVelocityMult = ZumasRevenge.Common._M(1.8f);
			particleType.AddSettingAtLifePct(0.75f, lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(0.4f);
			lifetimeSettings.mVelocityMult = ZumasRevenge.Common._M(1.9f);
			particleType.AddSettingAtLifePct(0.88f, lifetimeSettings);
			particleType.AddSettingAtLifePct(1f, new LifetimeSettings(lifetimeSettings)
			{
				mSizeXMult = ZumasRevenge.Common._M(0.2f),
				mVelocityMult = ZumasRevenge.Common._M(2f)
			});
			emitter.AddParticleType(particleType);
			system.AddEmitter(emitter);
			system = this.mTriggeredEffect.mFlare;
			emitter = new Emitter();
			emitter.mCullingRect = new Rect(0, 0, ZumasRevenge.Common._SS(GlobalMembers.gSexyAppBase.mWidth), ZumasRevenge.Common._SS(GlobalMembers.gSexyAppBase.mHeight));
			emitter.mDeleteInvisParticles = true;
			emitterScale = new EmitterScale();
			emitterScale.mLifeScale = ZumasRevenge.Common._M(1.5f);
			emitterScale.mNumberScale = ZumasRevenge.Common._M(3.15f);
			emitterScale.mZoom = ZumasRevenge.Common._M(3.12f);
			emitterScale.mSizeXScale = ZumasRevenge.Common._M(2.03f);
			emitter.AddScaleKeyFrame(0, emitterScale);
			emitterScale = new EmitterScale(emitterScale);
			emitterScale.mSizeXScale = ZumasRevenge.Common._M(2.37f);
			emitter.AddScaleKeyFrame(ZumasRevenge.Common._M(9), emitterScale);
			emitterScale = new EmitterScale(emitterScale);
			emitterScale.mSizeXScale = ZumasRevenge.Common._M(2.06f);
			emitter.AddScaleKeyFrame(ZumasRevenge.Common._M(16), emitterScale);
			emitterScale = new EmitterScale(emitterScale);
			emitterScale.mSizeXScale = (float)ZumasRevenge.Common._M(0);
			emitter.AddScaleKeyFrame(ZumasRevenge.Common._M(60), emitterScale);
			emitter.AddSettingsKeyFrame(0, new EmitterSettings
			{
				mVisibility = ZumasRevenge.Common._M(0.5f)
			});
			emitter.AddSettingsKeyFrame(20, new EmitterSettings
			{
				mVisibility = ZumasRevenge.Common._M(1f)
			});
			emitter.AddSettingsKeyFrame(35, new EmitterSettings
			{
				mVisibility = ZumasRevenge.Common._M(0f)
			});
			particleType = new ParticleType();
			particleType.mAdditive = true;
			particleType.mEmitterAttachPct = 1f;
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_STARBURST);
			particleType.mColorKeyManager.AddColorKey(0f, new SexyColor(10, 255, 88));
			particleType.mColorKeyManager.AddColorKey(0.25f, new SexyColor(25, 0, 255));
			particleType.mColorKeyManager.AddColorKey(0.5f, new SexyColor(255, 0, 161));
			particleType.mColorKeyManager.AddColorKey(0.75f, new SexyColor(255, 0, 0));
			particleType.mColorKeyManager.AddColorKey(1f, new SexyColor(254, 255, 0));
			particleType.mAlphaKeyManager.AddAlphaKey(0f, 255);
			particleType.AddSettingsKeyFrame(0, new ParticleSettings
			{
				mLife = ZumasRevenge.Common._M(8),
				mNumber = (int)((float)ZumasRevenge.Common._M(5) * MultiplierBallEffect.MULTIPLIER_BALL_EFFECT_PARTICLE_COUNT_SCALE),
				mXSize = ZumasRevenge.Common._M(28),
				mVelocity = ZumasRevenge.Common._M(4),
				mGlobalVisibility = ZumasRevenge.Common._M(0.76f)
			});
			particleType.AddVarianceKeyFrame(0, new ParticleVariance
			{
				mLifeVar = ZumasRevenge.Common._M(14),
				mVelocityVar = ZumasRevenge.Common._M(6)
			});
			particleType.AddSettingAtLifePct(0f, new LifetimeSettings
			{
				mSizeXMult = 0f
			});
			particleType.AddSettingAtLifePct(0.25f, new LifetimeSettings
			{
				mSizeXMult = ZumasRevenge.Common._M(1.5f)
			});
			particleType.AddSettingAtLifePct(0.37f, new LifetimeSettings
			{
				mSizeXMult = ZumasRevenge.Common._M(0.6f)
			});
			particleType.AddSettingAtLifePct(1f, new LifetimeSettings
			{
				mSizeXMult = ZumasRevenge.Common._M(0.1f)
			});
			emitter.AddParticleType(particleType);
			particleType = new ParticleType();
			particleType.mAdditive = true;
			particleType.mEmitterAttachPct = 1f;
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_BIG_STAR);
			particleType.mAlphaKeyManager.AddAlphaKey(0f, 255);
			particleType.mColorKeyManager.AddColorKey(0f, new SexyColor(255, 0, 0));
			particleType.mColorKeyManager.AddColorKey(0.4f, new SexyColor(255, 246, 1));
			particleType.mColorKeyManager.AddColorKey(0.675f, new SexyColor(242, 255, 0));
			particleType.mColorKeyManager.AddColorKey(0.8f, new SexyColor(SexyColor.White));
			particleType.mColorKeyManager.AddColorKey(1f, new SexyColor(SexyColor.White));
			particleType.AddSettingsKeyFrame(0, new ParticleSettings
			{
				mLife = ZumasRevenge.Common._M(8),
				mNumber = (int)((float)ZumasRevenge.Common._M(2) * MultiplierBallEffect.MULTIPLIER_BALL_EFFECT_PARTICLE_COUNT_SCALE),
				mXSize = ZumasRevenge.Common._M(16),
				mGlobalVisibility = ZumasRevenge.Common._M(0.41f)
			});
			particleType.AddVarianceKeyFrame(0, new ParticleVariance
			{
				mLifeVar = ZumasRevenge.Common._M(14)
			});
			particleType.AddSettingAtLifePct(0f, new LifetimeSettings
			{
				mSizeXMult = 0f
			});
			particleType.AddSettingAtLifePct(0.25f, new LifetimeSettings
			{
				mSizeXMult = ZumasRevenge.Common._M(1.5f)
			});
			particleType.AddSettingAtLifePct(0.37f, new LifetimeSettings
			{
				mSizeXMult = ZumasRevenge.Common._M(0.6f)
			});
			particleType.AddSettingAtLifePct(1f, new LifetimeSettings
			{
				mSizeXMult = ZumasRevenge.Common._M(0.1f)
			});
			emitter.AddParticleType(particleType);
			system.AddEmitter(emitter);
			system = this.mTriggeredEffect.mTrail;
			system.SetLife(ZumasRevenge.Common._M(200));
			emitter = new Emitter();
			emitter.mCullingRect = new Rect(0, 0, ZumasRevenge.Common._SS(GlobalMembers.gSexyAppBase.mWidth), ZumasRevenge.Common._SS(GlobalMembers.gSexyAppBase.mHeight));
			emitter.mDeleteInvisParticles = true;
			emitterScale = new EmitterScale();
			emitterScale.mLifeScale = ZumasRevenge.Common._M(1f);
			emitterScale.mNumberScale = ZumasRevenge.Common._M(1f);
			emitterScale.mSizeXScale = ZumasRevenge.Common._M(0.59f);
			emitterScale.mVelocityScale = ZumasRevenge.Common._M(1f);
			emitterScale.mWeightScale = ZumasRevenge.Common._M(3f);
			emitterScale.mSpinScale = ZumasRevenge.Common._M(0.54f);
			emitterScale.mZoom = ZumasRevenge.Common._M(3.63f);
			emitter.AddScaleKeyFrame(0, emitterScale);
			emitterScale = new EmitterScale(emitterScale);
			emitter.AddScaleKeyFrame(ZumasRevenge.Common._M(36), emitterScale);
			emitterScale = new EmitterScale(emitterScale);
			emitterScale.mLifeScale = 0f;
			emitter.AddScaleKeyFrame(ZumasRevenge.Common._M(51), emitterScale);
			emitterSettings = new EmitterSettings();
			emitterSettings.mVisibility = 1f;
			emitter.AddSettingsKeyFrame(0, emitterSettings);
			emitterSettings = new EmitterSettings(emitterSettings);
			emitter.AddSettingsKeyFrame(ZumasRevenge.Common._M(37), emitterSettings);
			emitterSettings = new EmitterSettings(emitterSettings);
			emitterSettings.mVisibility = 0f;
			emitter.AddSettingsKeyFrame(ZumasRevenge.Common._M(96), emitterSettings);
			particleType = new ParticleType();
			particleType.mAdditive = true;
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_BIG_STAR);
			particleType.mColorKeyManager.AddColorKey(0f, new SexyColor(0, 255, 4));
			particleType.mColorKeyManager.AddColorKey(0.25f, new SexyColor(242, 255, 0));
			particleType.mColorKeyManager.AddColorKey(0.45f, new SexyColor(255, 0, 0));
			particleType.mColorKeyManager.AddColorKey(0.65f, new SexyColor(38, 0, 255));
			particleType.mColorKeyManager.AddColorKey(1f, new SexyColor(113, 38, 255));
			particleType.mAlphaKeyManager.SetFixedColor(new SexyColor(SexyColor.White));
			particleType.mColorKeyManager.SetColorMode(2);
			particleSettings = new ParticleSettings();
			particleSettings.mLife = ZumasRevenge.Common._M(29);
			particleSettings.mNumber = (int)((float)ZumasRevenge.Common._M(83) * MultiplierBallEffect.MULTIPLIER_BALL_EFFECT_PARTICLE_COUNT_SCALE);
			particleSettings.mXSize = ZumasRevenge.Common._M(10);
			particleSettings.mVelocity = ZumasRevenge.Common._M(3);
			particleSettings.mWeight = (float)ZumasRevenge.Common._M(-8);
			particleSettings.mSpin = SexyFramework.Common.DegreesToRadians((float)ZumasRevenge.Common._M(3));
			particleType.AddSettingsKeyFrame(0, particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(34), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mVelocity = ZumasRevenge.Common._M(72);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(39), particleSettings);
			particleType.AddVarianceKeyFrame(0, new ParticleVariance
			{
				mNumberVar = ZumasRevenge.Common._M(28),
				mSizeXVar = ZumasRevenge.Common._M(7),
				mVelocityVar = ZumasRevenge.Common._M(24),
				mSpinVar = SexyFramework.Common.DegreesToRadians((float)ZumasRevenge.Common._M(210)),
				mMotionRandVar = (float)ZumasRevenge.Common._M(44)
			});
			particleType.AddSettingAtLifePct(0f, new LifetimeSettings
			{
				mSizeXMult = ZumasRevenge.Common._M(1.6f)
			});
			lifetimeSettings = new LifetimeSettings();
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(1.4f);
			particleType.AddSettingAtLifePct(ZumasRevenge.Common._M(0.43f), lifetimeSettings);
			lifetimeSettings = new LifetimeSettings();
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(1.2f);
			particleType.AddSettingAtLifePct(ZumasRevenge.Common._M(0.63f), lifetimeSettings);
			lifetimeSettings = new LifetimeSettings();
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(0.7f);
			particleType.AddSettingAtLifePct(ZumasRevenge.Common._M(0.8f), lifetimeSettings);
			particleType.AddSettingAtLifePct(1f, new LifetimeSettings
			{
				mSizeXMult = 0f
			});
			emitter.AddParticleType(particleType);
			particleType = new ParticleType();
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_ROUND);
			particleType.mColorKeyManager.SetFixedColor(new SexyColor(21, 0, 211));
			particleType.mAlphaKeyManager.SetFixedColor(new SexyColor(255, 255, 255, 211));
			particleType.mAdditive = true;
			particleType.mSingle = true;
			particleSettings = new ParticleSettings();
			particleSettings.mLife = ZumasRevenge.Common._M(100);
			particleSettings.mNumber = (int)((float)ZumasRevenge.Common._M(33) * MultiplierBallEffect.MULTIPLIER_BALL_EFFECT_PARTICLE_COUNT_SCALE);
			particleSettings.mXSize = ZumasRevenge.Common._M(100);
			particleType.AddSettingsKeyFrame(0, particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(24), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mXSize = ZumasRevenge.Common._M(266);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(45), particleSettings);
			emitter.AddParticleType(particleType);
			float num2 = -1f;
			float num3 = -1f;
			if (this.mMultBall.GetX() < (float)(ZumasRevenge.Common._SS(GlobalMembers.gSexyAppBase.mWidth) / 2))
			{
				num2 = 1f;
			}
			if (this.mMultBall.GetY() < (float)(ZumasRevenge.Common._SS(GlobalMembers.gSexyAppBase.mHeight) / 2))
			{
				num3 = 1f;
			}
			Vector2 c = new Vector2(num2 * (float)ZumasRevenge.Common._M(150) + this.mMultBall.GetX(), num3 * (float)ZumasRevenge.Common._M1(250) + this.mMultBall.GetY());
			emitter.mWaypointManager.AddPoint(0, new Vector2(this.mMultBall.GetX(), this.mMultBall.GetY()), false, c);
			c.X = (float)ZumasRevenge.Common._M(649);
			c.Y = (float)ZumasRevenge.Common._M(169);
			Gun gun = GameApp.gApp.GetBoard().GetGun();
			emitter.mWaypointManager.AddPoint(ZumasRevenge.Common._M(45), new Vector2((float)(ZumasRevenge.Common._M1(0) + gun.GetCenterX()), (float)(ZumasRevenge.Common._M2(-50) + gun.GetCenterY())), false, c);
			bool make_curve_image = false;
			emitter.mWaypointManager.Init(make_curve_image);
			MultiplierBallEffect.gTrailHandle = system.AddEmitter(emitter);
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00064E88 File Offset: 0x00063088
		protected void UpdateStateTriggered()
		{
			this.mTriggerTimer++;
			this.mTriggeredEffect.mRings.SetPos(this.mLastBallX, this.mLastBallY);
			this.mTriggeredEffect.mRings.Update();
			if (this.mTriggerTimer >= ZumasRevenge.Common._M(17))
			{
				this.mTriggeredEffect.mRainbow.SetPos(this.mLastBallX, this.mLastBallY);
				this.mTriggeredEffect.mRainbow.Update();
			}
			this.mTriggeredEffect.mGas.SetPos(this.mLastBallX, this.mLastBallY);
			this.mTriggeredEffect.mGas.Update();
			this.mTriggeredEffect.mFlare.SetPos(this.mLastBallX, this.mLastBallY);
			this.mTriggeredEffect.mFlare.Update();
			this.mTriggeredEffect.mTrail.SetPos(this.mLastBallX, this.mLastBallY);
			this.mTriggeredEffect.mTrail.Update();
			if (this.mDoMultFlash && this.mTriggeredEffect.mTrail.GetEmitter(MultiplierBallEffect.gTrailHandle).mWaypointManager.AtEnd())
			{
				this.mDoMultFlash = false;
			}
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00064FC0 File Offset: 0x000631C0
		protected void DrawStateTriggered(Graphics g)
		{
			this.mTriggeredEffect.mRings.Draw(g);
			this.mTriggeredEffect.mRainbow.Draw(g);
			this.mTriggeredEffect.mGas.Draw(g);
			this.mTriggeredEffect.mFlare.Draw(g);
			this.mTriggeredEffect.mTrail.Draw(g);
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00065024 File Offset: 0x00063224
		public MultiplierBallEffect(Ball mult_ball, bool spawn)
		{
			this.mMultBall = mult_ball;
			this.mSpawnTimer = 0;
			this.mTriggerTimer = 0;
			this.mSpawnEffect = null;
			this.mDoMultFlash = false;
			this.mTriggeredEffect = null;
			if (spawn)
			{
				this.InitSpawnEffects();
				return;
			}
			this.InitTriggeredEffects();
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00065087 File Offset: 0x00063287
		public virtual void Dispose()
		{
			if (this.mSpawnEffect != null)
			{
				this.mSpawnEffect.Dispose();
				this.mSpawnEffect = null;
			}
			if (this.mTriggeredEffect != null)
			{
				this.mTriggeredEffect.Dispose();
				this.mTriggeredEffect = null;
			}
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x000650C0 File Offset: 0x000632C0
		public void Update()
		{
			if (this.mMultBall != null)
			{
				this.mLastBallX = this.mMultBall.GetX();
				this.mLastBallY = this.mMultBall.GetY();
			}
			if (this.mSpawnEffect != null)
			{
				this.UpdateStateSpawn();
				if (this.mSpawnEffect.mRings.Done() && this.mSpawnEffect.mSwirl.Done())
				{
					this.mSpawnEffect.Dispose();
					this.mSpawnEffect = null;
				}
			}
			if (this.mTriggeredEffect != null)
			{
				this.UpdateStateTriggered();
				if (this.mTriggeredEffect.mRings.Done() && this.mTriggeredEffect.mRainbow.Done() && this.mTriggeredEffect.mGas.Done() && this.mTriggeredEffect.mTrail.Done())
				{
					this.mTriggeredEffect.Dispose();
					this.mTriggeredEffect = null;
				}
			}
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x000651A3 File Offset: 0x000633A3
		public void Draw(Graphics g)
		{
			if (this.mSpawnEffect != null)
			{
				this.DrawStateSpawn(g);
			}
			if (this.mTriggeredEffect != null)
			{
				this.DrawStateTriggered(g);
			}
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x000651C3 File Offset: 0x000633C3
		public void BallDestroyed(Ball mult_ball)
		{
			this.mMultBall = mult_ball;
			this.mLastBallX = this.mMultBall.GetX();
			this.mLastBallY = this.mMultBall.GetY();
			this.InitTriggeredEffects();
			this.mMultBall = null;
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x000651FB File Offset: 0x000633FB
		public Ball GetBall()
		{
			return this.mMultBall;
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00065204 File Offset: 0x00063404
		public void SyncState(DataSync sync)
		{
			sync.SyncBoolean(ref this.mDoMultFlash);
			sync.SyncLong(ref this.mState);
			sync.SyncLong(ref this.mSpawnTimer);
			sync.SyncLong(ref this.mTriggerTimer);
			sync.SyncFloat(ref this.mLastBallX);
			sync.SyncFloat(ref this.mLastBallY);
			SexyBuffer buffer = sync.GetBuffer();
			if (sync.isWrite())
			{
				buffer.WriteLong((long)this.mBeamAlphas.Count);
				for (int i = 0; i < this.mBeamAlphas.Count; i++)
				{
					buffer.WriteBoolean(this.mBeamAlphas[i].second);
					AlphaFader first = this.mBeamAlphas[i].first;
					buffer.WriteFloat(first.mFadeRate);
					buffer.WriteLong((long)first.mFadeCount);
					buffer.WriteLong((long)first.mMin);
					buffer.WriteLong((long)first.mMax);
					buffer.WriteFloat(first.mColor.mRed);
					buffer.WriteFloat(first.mColor.mGreen);
					buffer.WriteFloat(first.mColor.mBlue);
					buffer.WriteFloat(first.mColor.mAlpha);
				}
				buffer.WriteBoolean(this.mTriggeredEffect != null);
				if (this.mTriggeredEffect != null)
				{
					ZumasRevenge.Common.SerializeParticleSystem(this.mTriggeredEffect.mRings, sync);
					ZumasRevenge.Common.SerializeParticleSystem(this.mTriggeredEffect.mRainbow, sync);
					ZumasRevenge.Common.SerializeParticleSystem(this.mTriggeredEffect.mGas, sync);
					ZumasRevenge.Common.SerializeParticleSystem(this.mTriggeredEffect.mFlare, sync);
					ZumasRevenge.Common.SerializeParticleSystem(this.mTriggeredEffect.mTrail, sync);
				}
				buffer.WriteBoolean(this.mSpawnEffect != null);
				if (this.mSpawnEffect != null)
				{
					ZumasRevenge.Common.SerializeParticleSystem(this.mSpawnEffect.mRings, sync);
					ZumasRevenge.Common.SerializeParticleSystem(this.mSpawnEffect.mSwirl, sync);
				}
				buffer.WriteBoolean(this.mMultBall != null);
				if (this.mMultBall != null)
				{
					buffer.WriteLong((long)this.mMultBall.GetId());
					return;
				}
			}
			else
			{
				int num = (int)buffer.ReadLong();
				this.mBeamAlphas.Clear();
				for (int j = 0; j < num; j++)
				{
					AlphaFadeInfo alphaFadeInfo = new AlphaFadeInfo(new AlphaFader(), false);
					alphaFadeInfo.second = buffer.ReadBoolean();
					alphaFadeInfo.first.mFadeRate = buffer.ReadFloat();
					alphaFadeInfo.first.mFadeCount = (int)buffer.ReadLong();
					alphaFadeInfo.first.mMin = (int)buffer.ReadLong();
					alphaFadeInfo.first.mMax = (int)buffer.ReadLong();
					alphaFadeInfo.first.mColor.mRed = buffer.ReadFloat();
					alphaFadeInfo.first.mColor.mGreen = buffer.ReadFloat();
					alphaFadeInfo.first.mColor.mBlue = buffer.ReadFloat();
					alphaFadeInfo.first.mColor.mAlpha = buffer.ReadFloat();
					this.mBeamAlphas.Add(alphaFadeInfo);
				}
				this.mTriggeredEffect = null;
				this.mSpawnEffect = null;
				if (buffer.ReadBoolean())
				{
					this.mTriggeredEffect = new TriggeredEffect(false);
					this.mTriggeredEffect.mRings = ZumasRevenge.Common.DeserializeParticleSystem(sync);
					this.mTriggeredEffect.mRainbow = ZumasRevenge.Common.DeserializeParticleSystem(sync);
					this.mTriggeredEffect.mGas = ZumasRevenge.Common.DeserializeParticleSystem(sync);
					this.mTriggeredEffect.mFlare = ZumasRevenge.Common.DeserializeParticleSystem(sync);
					this.mTriggeredEffect.mTrail = ZumasRevenge.Common.DeserializeParticleSystem(sync);
				}
				if (buffer.ReadBoolean())
				{
					this.mSpawnEffect = new SpawnEffect(false);
					this.mSpawnEffect.mRings = ZumasRevenge.Common.DeserializeParticleSystem(sync);
					this.mSpawnEffect.mSwirl = ZumasRevenge.Common.DeserializeParticleSystem(sync);
				}
				if (buffer.ReadBoolean())
				{
					int id = (int)buffer.ReadLong();
					this.mMultBall = ((GameApp)GlobalMembers.gSexyApp).GetBoard().mLevel.GetBallById(id);
				}
			}
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x000655EA File Offset: 0x000637EA
		public bool Done()
		{
			return this.mSpawnEffect == null && this.mTriggeredEffect == null;
		}

		// Token: 0x040008CC RID: 2252
		public static SpawnColors[] gSpawnColors = new SpawnColors[]
		{
			new SpawnColors(new SexyColor(1, 108, 222), new SexyColor(12, 0, 255), new SexyColor(0, 195, 255)),
			new SpawnColors(new SexyColor(246, 236, 4), new SexyColor(206, 151, 15), new SexyColor(254, 255, 0)),
			new SpawnColors(new SexyColor(222, 0, 0), new SexyColor(250, 14, 124), new SexyColor(255, 131, 0)),
			new SpawnColors(new SexyColor(0, 236, 51), new SexyColor(25, 125, 115), new SexyColor(135, 235, 15)),
			new SpawnColors(new SexyColor(155, 17, 236), new SexyColor(11, 10, 255), new SexyColor(216, 21, 255)),
			new SpawnColors(new SexyColor(250, 240, 238), new SexyColor(SexyColor.White), new SexyColor(SexyColor.White))
		};

		// Token: 0x040008CD RID: 2253
		public static int[] gMaxParticles = new int[7];

		// Token: 0x040008CE RID: 2254
		public static int gTrailHandle = 0;

		// Token: 0x040008CF RID: 2255
		public static float MULTIPLIER_BALL_EFFECT_PARTICLE_COUNT_SCALE = 0.5f;

		// Token: 0x040008D0 RID: 2256
		protected Ball mMultBall;

		// Token: 0x040008D1 RID: 2257
		protected SpawnEffect mSpawnEffect;

		// Token: 0x040008D2 RID: 2258
		protected TriggeredEffect mTriggeredEffect;

		// Token: 0x040008D3 RID: 2259
		protected List<AlphaFadeInfo> mBeamAlphas = new List<AlphaFadeInfo>();

		// Token: 0x040008D4 RID: 2260
		protected float mLastBallX;

		// Token: 0x040008D5 RID: 2261
		protected float mLastBallY;

		// Token: 0x040008D6 RID: 2262
		protected int mSpawnTimer;

		// Token: 0x040008D7 RID: 2263
		protected int mTriggerTimer;

		// Token: 0x040008D8 RID: 2264
		protected int mState;

		// Token: 0x040008D9 RID: 2265
		protected bool mDoMultFlash;

		// Token: 0x040008DA RID: 2266
		protected Transform mGlobalTranform = new Transform();
	}
}
