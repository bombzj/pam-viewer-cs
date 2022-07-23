using System;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.PIL;

namespace ZumasRevenge
{
	// Token: 0x0200003A RID: 58
	public class Skull
	{
		// Token: 0x060005EF RID: 1519 RVA: 0x0002294C File Offset: 0x00020B4C
		public Skull(bool left)
		{
			this.mParticleAlpha = 1f;
			this.mLaunched = false;
			this.mUseLastPos = false;
			this.mLastX = (this.mLastY = (this.mLastAngle = 0f));
			this.mLeftEye = new SexyFramework.PIL.System(50, 50);
			this.mLeftEye.mScale = Common._S(1f);
			this.mLeftEye.WaitForEmitters(true);
			this.mRightEye = new SexyFramework.PIL.System(50, 50);
			this.mRightEye.mScale = Common._S(1f);
			this.mRightEye.WaitForEmitters(true);
			Emitter emitter = new Emitter();
			emitter.mCullingRect = new Rect(0, 0, Common._SS(GameApp.gApp.mWidth), Common._SS(GameApp.gApp.mHeight));
			emitter.mEmissionCoordsAreOffsets = true;
			emitter.AddScaleKeyFrame(0, new EmitterScale
			{
				mLifeScale = Common._M(0.3f),
				mNumberScale = Common._M(2.01f),
				mSizeXScale = Common._M(1.27f),
				mSizeYScale = Common._M(0.9f),
				mVelocityScale = Common._M(0.24f),
				mWeightScale = Common._M(0.48f),
				mZoom = Common._M(1.66f)
			});
			emitter.AddSettingsKeyFrame(0, new EmitterSettings
			{
				mVisibility = Common._M(0.51f)
			});
			ParticleType particleType = new ParticleType();
			particleType.mEmitterAttachPct = Common._M(1f);
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_FUZZY_CIRCLE);
			particleType.mAdditive = true;
			particleType.mColorKeyManager.AddColorKey(0f, new SexyColor(1, 255, 6));
			particleType.mColorKeyManager.AddColorKey(0.5f, new SexyColor(216, 255, 216));
			particleType.mColorKeyManager.AddColorKey(1f, new SexyColor(67, 255, 0));
			particleType.AddSettingsKeyFrame(0, new ParticleSettings
			{
				mLife = Common._M(21),
				mNumber = Common._M(18),
				mXSize = Common._M(15),
				mVelocity = Common._M(8),
				mWeight = (float)Common._M(-31),
				mGlobalVisibility = Common._M(0.5f)
			});
			particleType.AddVarianceKeyFrame(0, new ParticleVariance
			{
				mLifeVar = Common._M(39),
				mNumberVar = Common._M(2),
				mSizeXVar = Common._M(3),
				mVelocityVar = Common._M(5)
			});
			LifetimeSettings lifetimeSettings = new LifetimeSettings();
			lifetimeSettings.mSizeXMult = Common._M(0.2f);
			lifetimeSettings.mVelocityMult = Common._M(0.6f);
			particleType.AddSettingAtLifePct(0f, lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			lifetimeSettings.mSizeXMult = Common._M(1.3f);
			lifetimeSettings.mVelocityMult = Common._M(0.77f);
			particleType.AddSettingAtLifePct(0.77f, lifetimeSettings);
			particleType.AddSettingAtLifePct(1f, new LifetimeSettings(lifetimeSettings)
			{
				mVelocityMult = Common._M(0.8f)
			});
			emitter.AddParticleType(particleType);
			particleType = new ParticleType();
			particleType.mAdditive = true;
			particleType.mEmitterAttachPct = 1f;
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_FUZZY_CIRCLE);
			particleType.mColorKeyManager.AddColorKey(0f, new SexyColor(1, 255, 6));
			particleType.mColorKeyManager.AddColorKey(0.5f, new SexyColor(255, 233, 1));
			particleType.mColorKeyManager.AddColorKey(1f, new SexyColor(0, 255, 42));
			ParticleSettings particleSettings = new ParticleSettings();
			particleSettings.mLife = Common._M(9);
			particleSettings.mNumber = Common._M(40);
			particleSettings.mXSize = Common._M(13);
			particleSettings.mVelocity = Common._M(20);
			particleSettings.mWeight = (float)Common._M(-11);
			particleSettings.mGlobalVisibility = Common._M(0f);
			particleType.AddSettingsKeyFrame(0, particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mGlobalVisibility = Common._M(0.3f);
			particleType.AddSettingsKeyFrame(Common._M(200), particleSettings);
			particleType.AddVarianceKeyFrame(0, new ParticleVariance
			{
				mLifeVar = Common._M(39),
				mVelocityVar = Common._M(5)
			});
			lifetimeSettings = new LifetimeSettings();
			lifetimeSettings.mSizeXMult = Common._M(0.2f);
			particleType.AddSettingAtLifePct(0f, lifetimeSettings);
			particleType.AddSettingAtLifePct(0.75f, new LifetimeSettings(lifetimeSettings)
			{
				mSizeXMult = Common._M(1.9f)
			});
			particleType.AddSettingAtLifePct(1f, new LifetimeSettings
			{
				mSizeXMult = 0f
			});
			emitter.AddParticleType(particleType);
			emitter.mPreloadFrames = Common._M(100);
			this.mLeftEyeHandle = this.mLeftEye.AddEmitter(emitter);
			this.mRightEyeHandle = this.mRightEye.AddEmitter(new Emitter(emitter));
			this.mTrail = new SexyFramework.PIL.System(50, 50);
			this.mTrail.mScale = Common._S(1f);
			this.mTrail.WaitForEmitters(true);
			emitter = new Emitter();
			emitter.mCullingRect = new Rect(0, 0, Common._SS(GameApp.gApp.mWidth), Common._SS(GameApp.gApp.mHeight));
			emitter.mEmissionCoordsAreOffsets = true;
			emitter.AddScaleKeyFrame(0, new EmitterScale
			{
				mNumberScale = Common._M(0.7f),
				mSizeXScale = Common._M(0.89f)
			});
			particleType = new ParticleType();
			particleType.mImage = (this.mIsLeft ? Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_SKULL_LEFT_PARTICLE) : Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_SKULL_RIGHT_PARTICLE));
			particleType.mAdditive = true;
			particleType.mEmitterAttachPct = Common._M(0f);
			particleType.mColorKeyManager.AddColorKey(0f, new SexyColor(1, 255, 6));
			particleType.mColorKeyManager.AddColorKey(0.5f, new SexyColor(134, 255, 149));
			particleType.mColorKeyManager.AddColorKey(1f, new SexyColor(7, 255, 49));
			particleType.AddSettingsKeyFrame(0, new ParticleSettings
			{
				mLife = Common._M(10),
				mNumber = Common._M(10),
				mXSize = Common._M(30),
				mGlobalVisibility = Common._M(0.4f)
			});
			particleType.AddSettingAtLifePct(0f, new LifetimeSettings
			{
				mSizeXMult = Common._M(0.4f)
			});
			particleType.AddSettingAtLifePct(0.13f, new LifetimeSettings
			{
				mSizeXMult = Common._M(2f)
			});
			particleType.AddSettingAtLifePct(0.52f, new LifetimeSettings
			{
				mSizeXMult = Common._M(1.2f)
			});
			particleType.AddSettingAtLifePct(1f, new LifetimeSettings
			{
				mSizeXMult = 0f
			});
			this.mTrailPTHandle = emitter.AddParticleType(particleType);
			this.mTrailHandle = this.mTrail.AddEmitter(emitter);
			this.mFatHead = new SexyFramework.PIL.System(50, 50);
			this.mFatHead.mScale = Common._S(1f);
			this.mFatHead.WaitForEmitters(true);
			emitter = new Emitter();
			emitter.mCullingRect = new Rect(0, 0, Common._SS(GameApp.gApp.mWidth), Common._SS(GameApp.gApp.mHeight));
			emitter.mEmissionCoordsAreOffsets = true;
			emitter.AddScaleKeyFrame(0, new EmitterScale
			{
				mZoom = Common._M(1.61f)
			});
			EmitterSettings emitterSettings = new EmitterSettings();
			emitter.AddSettingsKeyFrame(0, emitterSettings);
			emitterSettings = new EmitterSettings();
			emitterSettings.mActive = false;
			emitter.AddSettingsKeyFrame(Common._M(99), emitterSettings);
			particleType = new ParticleType();
			particleType.mImage = (this.mIsLeft ? Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_SKULL_LEFT_PARTICLE) : Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_SKULL_RIGHT_PARTICLE));
			particleType.mAdditive = false;
			particleType.mEmitterAttachPct = Common._M(0f);
			particleSettings = new ParticleSettings();
			particleSettings.mLife = Common._M(99);
			particleSettings.mNumber = Common._M(17);
			particleSettings.mXSize = Common._M(9);
			particleType.AddSettingsKeyFrame(0, particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mXSize = Common._M(150);
			particleSettings.mGlobalVisibility = Common._M(0.16f);
			particleType.AddSettingsKeyFrame(Common._M(70), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mGlobalVisibility = 0f;
			particleType.AddSettingsKeyFrame(Common._M(99), particleSettings);
			emitter.mPreloadFrames = Common._M(10);
			emitter.AddParticleType(particleType);
			this.mFatHead.AddEmitter(emitter);
			this.mChunks = new SexyFramework.PIL.System(50, 50);
			this.mChunks.mScale = Common._S(1f);
			this.mChunks.WaitForEmitters(true);
			emitter = new Emitter();
			emitter.mCullingRect = new Rect(0, 0, Common._SS(GameApp.gApp.mWidth), Common._SS(GameApp.gApp.mHeight));
			emitter.mEmissionCoordsAreOffsets = true;
			int frame = Common._M(55);
			EmitterScale emitterScale = new EmitterScale();
			emitterScale.mNumberScale = Common._M(0.6f);
			emitterScale.mZoom = Common._M(1.6f);
			emitter.AddScaleKeyFrame(0, emitterScale);
			emitterScale = new EmitterScale(emitterScale);
			emitterScale.mZoom = 1f;
			emitter.AddScaleKeyFrame(Common._M(15), emitterScale);
			emitterSettings = new EmitterSettings();
			emitterSettings.mVisibility = 1f;
			emitter.AddSettingsKeyFrame(0, emitterSettings);
			emitterSettings = new EmitterSettings(emitterSettings);
			emitter.AddSettingsKeyFrame(Common._M(25), emitterSettings);
			emitter.AddSettingsKeyFrame(frame, new EmitterSettings(emitterSettings)
			{
				mVisibility = 0f,
				mActive = false
			});
			particleType = new ParticleType();
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_CHUNK1);
			particleType.mAdditive = false;
			particleType.AddSettingsKeyFrame(0, new ParticleSettings
			{
				mLife = Common._M(20),
				mNumber = Common._M(15),
				mXSize = Common._M(20),
				mVelocity = Common._M(250),
				mSpin = SexyFramework.Common.DegreesToRadians((float)Common._M(9))
			});
			particleType.AddVarianceKeyFrame(0, new ParticleVariance
			{
				mSizeXVar = Common._M(11)
			});
			emitter.AddParticleType(particleType);
			this.mChunks.AddEmitter(emitter);
			emitter = new Emitter(emitter);
			emitter.GetParticleTypeByIndex(0).mImage = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_CHUNK2);
			this.mChunks.AddEmitter(emitter);
			emitter = new Emitter(emitter);
			emitter.GetParticleTypeByIndex(0).mImage = Res.GetImageByID(ResID.IMAGE_BOSS_TIGER_CHUNK3);
			this.mChunks.AddEmitter(emitter);
			this.mClouds = new SexyFramework.PIL.System(50, 50);
			this.mClouds.mScale = Common._S(1f);
			this.mClouds.WaitForEmitters(true);
			emitter = new Emitter();
			emitter.mCullingRect = new Rect(0, 0, Common._SS(GameApp.gApp.mWidth), Common._SS(GameApp.gApp.mHeight));
			emitter.mEmissionCoordsAreOffsets = true;
			frame = Common._M(70);
			emitter.AddScaleKeyFrame(0, new EmitterScale
			{
				mLifeScale = Common._M(0.8f),
				mNumberScale = Common._M(0.48f),
				mSizeXScale = Common._M(3.7f),
				mVelocityScale = Common._M(0.82f)
			});
			emitterSettings = new EmitterSettings();
			emitterSettings.mVisibility = Common._M(0.5f);
			emitter.AddSettingsKeyFrame(0, emitterSettings);
			emitter.AddSettingsKeyFrame(Common._M(50), new EmitterSettings(emitterSettings));
			emitter.AddSettingsKeyFrame(frame, new EmitterSettings(emitterSettings)
			{
				mVisibility = 0f,
				mActive = false
			});
			particleType = new ParticleType();
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_ROUND);
			particleType.mColorKeyManager.SetFixedColor(new SexyColor(65, 70, 49));
			particleType.mAlphaKeyManager.SetFixedColor(new SexyColor(0, 0, 0, Common._M(98)));
			particleType.AddSettingsKeyFrame(0, new ParticleSettings
			{
				mLife = Common._M(59),
				mNumber = Common._M(10),
				mXSize = Common._M(30),
				mVelocity = Common._M(99)
			});
			emitter.AddParticleType(particleType);
			particleType = new ParticleType(particleType);
			particleType.mColorKeyManager.SetFixedColor(new SexyColor(SexyColor.White));
			particleType.mAlphaKeyManager.SetFixedColor(new SexyColor(SexyColor.White));
			emitter.AddParticleType(particleType);
			this.mClouds.AddEmitter(emitter);
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00023688 File Offset: 0x00021888
		public virtual void Dispose()
		{
			if (this.mLeftEye != null)
			{
				this.mLeftEye.Dispose();
				this.mLeftEye = null;
			}
			if (this.mRightEye != null)
			{
				this.mRightEye.Dispose();
				this.mRightEye = null;
			}
			if (this.mTrail != null)
			{
				this.mTrail.Dispose();
				this.mTrail = null;
			}
			if (this.mFatHead != null)
			{
				this.mFatHead.Dispose();
				this.mFatHead = null;
			}
			if (this.mChunks != null)
			{
				this.mChunks.Dispose();
				this.mChunks = null;
			}
			if (this.mClouds != null)
			{
				this.mClouds.Dispose();
				this.mClouds = null;
			}
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00023734 File Offset: 0x00021934
		public void Reset()
		{
			this.mParticleAlpha = 1f;
			this.mLaunched = false;
			this.mUseLastPos = false;
			this.mLastX = (this.mLastY = (this.mLastAngle = 0f));
			this.mHitPlayer = false;
			this.mAlpha = 0f;
			if (this.mLeftEye != null)
			{
				this.mLeftEye.ResetForReuse();
				this.mLeftEye.mScale = Common._S(1f);
				this.mLeftEye.WaitForEmitters(true);
			}
			if (this.mRightEye != null)
			{
				this.mRightEye.ResetForReuse();
				this.mRightEye.mScale = Common._S(1f);
				this.mRightEye.WaitForEmitters(true);
			}
			if (this.mTrail != null)
			{
				this.mTrail.ResetForReuse();
				this.mTrail.mScale = Common._S(1f);
				this.mTrail.WaitForEmitters(true);
			}
			if (this.mFatHead != null)
			{
				this.mFatHead.ResetForReuse();
				this.mFatHead.mScale = Common._S(1f);
				this.mFatHead.WaitForEmitters(true);
			}
			if (this.mChunks != null)
			{
				this.mChunks.ResetForReuse();
				this.mChunks.mScale = Common._S(1f);
				this.mChunks.WaitForEmitters(true);
			}
			if (this.mClouds != null)
			{
				this.mClouds.ResetForReuse();
				this.mClouds.mScale = Common._S(1f);
				this.mClouds.WaitForEmitters(true);
			}
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x000238C4 File Offset: 0x00021AC4
		public void Update(float x, float y, float angle)
		{
			if (this.mAlpha < 255f)
			{
				this.mAlpha += Common._M(5f);
				if (this.mAlpha > 255f)
				{
					this.mAlpha = 255f;
				}
			}
			if (!this.mLaunched)
			{
				return;
			}
			if (!this.mUseLastPos)
			{
				this.mLastX = x;
				this.mLastY = y;
				this.mLastAngle = angle;
			}
			else
			{
				x = this.mLastX;
				y = this.mLastY;
				angle = this.mLastAngle;
			}
			float mInitAngle = -Common.GetCanonicalAngleRad(angle);
			for (int i = 0; i < this.mLeftEye.GetEmitter(this.mLeftEyeHandle).GetNumParticleTypes(); i++)
			{
				this.mLeftEye.GetEmitter(this.mLeftEyeHandle).GetParticleTypeByIndex(i).mInitAngle = mInitAngle;
			}
			for (int j = 0; j < this.mRightEye.GetEmitter(this.mRightEyeHandle).GetNumParticleTypes(); j++)
			{
				this.mRightEye.GetEmitter(this.mRightEyeHandle).GetParticleTypeByIndex(j).mInitAngle = mInitAngle;
			}
			float num = (float)Common._M(11) * (float)Math.Cos((double)angle);
			float num2 = (float)Common._M(4) * -(float)Math.Sin((double)angle);
			this.mLeftEye.SetPos(x - num, y - num2);
			this.mLeftEye.Update();
			num = (float)Common._M(10) * (float)Math.Cos((double)angle);
			num2 = (float)Common._M(16) * -(float)Math.Sin((double)angle);
			this.mRightEye.SetPos(x + num, y + num2);
			this.mRightEye.Update();
			this.mTrail.GetEmitter(this.mTrailHandle).GetParticleType(this.mTrailPTHandle).mInitAngle = mInitAngle;
			this.mTrail.SetPos(x, y);
			this.mTrail.Update();
			if (this.mHitPlayer)
			{
				Gun gun = GameApp.gApp.GetBoard().GetGun();
				this.mFatHead.SetPos((float)(gun.GetCenterX() + Common._M(0)), (float)(gun.GetCenterY() + Common._M1(0)));
				this.mFatHead.Update();
				this.mChunks.SetPos((float)(gun.GetCenterX() + Common._M(0)), (float)(gun.GetCenterY() + Common._M1(0)));
				this.mClouds.SetPos((float)(gun.GetCenterX() + Common._M(0)), (float)(gun.GetCenterY() + Common._M1(0)));
				this.mChunks.Update();
				this.mClouds.Update();
			}
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00023B48 File Offset: 0x00021D48
		public void Draw(Graphics g, float x, float y, float angle)
		{
			g.PushState();
			this.mTrail.mAlphaPct = this.mParticleAlpha;
			this.mLeftEye.mAlphaPct = this.mParticleAlpha;
			this.mRightEye.mAlphaPct = this.mParticleAlpha;
			this.mFatHead.mAlphaPct = this.mParticleAlpha;
			this.mClouds.mAlphaPct = this.mParticleAlpha;
			this.mChunks.mAlphaPct = this.mParticleAlpha;
			if (this.mLaunched)
			{
				this.mTrail.Draw(g);
			}
			if (!this.mUseLastPos)
			{
				if (this.mAlpha < 255f)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)this.mAlpha);
				}
				g.DrawImageRotated(this.mImage, (int)x, (int)y, (double)angle);
				g.SetColorizeImages(false);
			}
			if (this.mLaunched)
			{
				this.mLeftEye.Draw(g);
				this.mRightEye.Draw(g);
			}
			if (this.mHitPlayer)
			{
				this.mFatHead.Draw(g);
				this.mClouds.Draw(g);
				this.mChunks.Draw(g);
			}
			g.PopState();
		}

		// Token: 0x040002E5 RID: 741
		public float mParticleAlpha;

		// Token: 0x040002E6 RID: 742
		public int mImageNum;

		// Token: 0x040002E7 RID: 743
		public int mLeftEyeHandle;

		// Token: 0x040002E8 RID: 744
		public int mRightEyeHandle;

		// Token: 0x040002E9 RID: 745
		public int mTrailPTHandle;

		// Token: 0x040002EA RID: 746
		public int mTrailHandle;

		// Token: 0x040002EB RID: 747
		public float mLastX;

		// Token: 0x040002EC RID: 748
		public float mLastY;

		// Token: 0x040002ED RID: 749
		public float mLastAngle;

		// Token: 0x040002EE RID: 750
		public bool mIsLeft;

		// Token: 0x040002EF RID: 751
		public float mAlpha;

		// Token: 0x040002F0 RID: 752
		public bool mLaunched;

		// Token: 0x040002F1 RID: 753
		public bool mUseLastPos;

		// Token: 0x040002F2 RID: 754
		public bool mHitPlayer;

		// Token: 0x040002F3 RID: 755
		public Image mImage;

		// Token: 0x040002F4 RID: 756
		public SexyFramework.PIL.System mLeftEye;

		// Token: 0x040002F5 RID: 757
		public SexyFramework.PIL.System mRightEye;

		// Token: 0x040002F6 RID: 758
		public SexyFramework.PIL.System mTrail;

		// Token: 0x040002F7 RID: 759
		public SexyFramework.PIL.System mFatHead;

		// Token: 0x040002F8 RID: 760
		public SexyFramework.PIL.System mChunks;

		// Token: 0x040002F9 RID: 761
		public SexyFramework.PIL.System mClouds;
	}
}
