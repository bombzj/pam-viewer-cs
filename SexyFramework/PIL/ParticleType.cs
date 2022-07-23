using System;
using System.Collections.Generic;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.PIL
{
	// Token: 0x02000172 RID: 370
	public class ParticleType
	{
		// Token: 0x06000D3E RID: 3390 RVA: 0x000407E8 File Offset: 0x0003E9E8
		public ParticleType()
		{
			this.mColorKeyManager = new ColorKeyManager();
			this.mAlphaKeyManager = new ColorKeyManager();
			this.mSettingsTimeLine.mCurrentSettings = new ParticleSettings();
			this.mVarTimeLine.mCurrentSettings = new ParticleVariance();
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00040888 File Offset: 0x0003EA88
		public ParticleType(ParticleType rhs)
			: this()
		{
			this.CopyFrom(rhs);
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00040897 File Offset: 0x0003EA97
		public virtual void Dispose()
		{
			this.mColorKeyManager = null;
			this.mAlphaKeyManager = null;
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x000408A8 File Offset: 0x0003EAA8
		public void CopyFrom(ParticleType rhs)
		{
			if (this == rhs || rhs == null)
			{
				return;
			}
			this.mSettingsTimeLine = rhs.mSettingsTimeLine;
			this.mVarTimeLine = rhs.mVarTimeLine;
			this.mSameColorKeyCount = rhs.mSameColorKeyCount;
			this.mLastColorKeyIndex = rhs.mLastColorKeyIndex;
			this.mImageSetByPINLoader = rhs.mImageSetByPINLoader;
			this.mInitAngleStep = rhs.mInitAngleStep;
			this.mLastSpawnAngle = rhs.mLastSpawnAngle;
			this.mLifePctSettings.Clear();
			for (int i = 0; i < rhs.mLifePctSettings.size<LifetimeSettingPct>(); i++)
			{
				this.AddSettingAtLifePct(rhs.mLifePctSettings[i].first, new LifetimeSettings(rhs.mLifePctSettings[i].second));
			}
			this.mImageRate = rhs.mImageRate;
			if (this.mImageSetByPINLoader)
			{
				this.mImage = GlobalMembers.gSexyAppBase.CopyImage(rhs.mImage);
			}
			else
			{
				this.mImage = rhs.mImage;
			}
			this.mImageName = rhs.mImageName;
			this.mName = rhs.mName;
			this.mColorKeyManager = rhs.mColorKeyManager;
			this.mAlphaKeyManager = rhs.mAlphaKeyManager;
			this.mXOff = rhs.mXOff;
			this.mYOff = rhs.mYOff;
			this.mRandomStartCel = rhs.mRandomStartCel;
			this.mLockSizeAspect = rhs.mLockSizeAspect;
			this.mAdditive = rhs.mAdditive;
			this.mAdditiveWithNormal = rhs.mAdditiveWithNormal;
			this.mFlipX = rhs.mFlipX;
			this.mFlipY = rhs.mFlipY;
			this.mLoopTimeline = rhs.mLoopTimeline;
			this.mAlignAngleToMotion = rhs.mAlignAngleToMotion;
			this.mSingle = rhs.mSingle;
			this.mMotionAngleOffset = rhs.mMotionAngleOffset;
			this.mAngleRange = rhs.mAngleRange;
			this.mInitAngle = rhs.mInitAngle;
			this.mEmitterAttachPct = rhs.mEmitterAttachPct;
			this.mNumSameColorKeyInRow = rhs.mNumSameColorKeyInRow;
			this.mNumCreated = rhs.mNumCreated;
			this.mRefXOff = rhs.mRefXOff;
			this.mRefYOff = rhs.mRefYOff;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00040AAC File Offset: 0x0003ECAC
		public void ResetForReuse()
		{
			this.mNumCreated = 0;
			this.mSameColorKeyCount = 0;
			this.mLastColorKeyIndex = 0;
			this.mLastSpawnAngle = float.MaxValue;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00040AD0 File Offset: 0x0003ECD0
		public void Serialize(SexyBuffer b, GlobalMembers.GetIdByImageFunc f)
		{
			this.mSettingsTimeLine.Serialize(b);
			this.mVarTimeLine.Serialize(b);
			b.WriteLong((long)this.mSameColorKeyCount);
			b.WriteLong((long)this.mLastColorKeyIndex);
			b.WriteBoolean(this.mImageSetByPINLoader);
			b.WriteLong((long)this.mLifePctSettings.Count);
			for (int i = 0; i < this.mLifePctSettings.Count; i++)
			{
				b.WriteFloat(this.mLifePctSettings[i].first);
				this.mLifePctSettings[i].second.Serialize(b);
			}
			b.WriteBoolean(this.mImage != null);
			if (this.mImage != null)
			{
				b.WriteLong((long)f(this.mImage));
			}
			b.WriteString(this.mImageName);
			b.WriteString(this.mName);
			this.mColorKeyManager.Serialize(b);
			this.mAlphaKeyManager.Serialize(b);
			b.WriteBoolean(this.mLockSizeAspect);
			b.WriteBoolean(this.mAdditive);
			b.WriteBoolean(this.mAdditiveWithNormal);
			b.WriteBoolean(this.mFlipX);
			b.WriteBoolean(this.mFlipY);
			b.WriteBoolean(this.mLoopTimeline);
			b.WriteBoolean(this.mAlignAngleToMotion);
			b.WriteBoolean(this.mSingle);
			b.WriteFloat(this.mMotionAngleOffset);
			b.WriteFloat(this.mInitAngle);
			b.WriteFloat(this.mAngleRange);
			b.WriteFloat(this.mEmitterAttachPct);
			b.WriteLong((long)this.mNumSameColorKeyInRow);
			b.WriteLong((long)this.mNumCreated);
			b.WriteLong((long)this.mRefXOff);
			b.WriteLong((long)this.mRefYOff);
			b.WriteLong((long)this.mImageRate);
			b.WriteLong((long)this.mSerialIndex);
			b.WriteFloat(this.mInitAngleStep);
			b.WriteFloat(this.mLastSpawnAngle);
			b.WriteBoolean(this.mRandomStartCel);
			b.WriteLong((long)this.mXOff);
			b.WriteLong((long)this.mYOff);
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00040CEC File Offset: 0x0003EEEC
		public void Deserialize(SexyBuffer b, GlobalMembers.GetImageByIdFunc f)
		{
			this.mSettingsTimeLine.Deserialize(b, new GlobalMembers.KFDInstantiateFunc(ParticleSettings.Instantiate));
			this.mVarTimeLine.Deserialize(b, new GlobalMembers.KFDInstantiateFunc(ParticleVariance.Instantiate));
			this.mSameColorKeyCount = (int)b.ReadLong();
			this.mLastColorKeyIndex = (int)b.ReadLong();
			this.mImageSetByPINLoader = b.ReadBoolean();
			int num = (int)b.ReadLong();
			for (int i = 0; i < num; i++)
			{
				float f2 = b.ReadFloat();
				LifetimeSettings lifetimeSettings = new LifetimeSettings();
				lifetimeSettings.Deserialize(b);
				this.mLifePctSettings.Add(new LifetimeSettingPct(f2, lifetimeSettings));
			}
			this.mImage = null;
			if (b.ReadBoolean())
			{
				this.mImage = f((int)b.ReadLong());
			}
			this.mImageName = b.ReadString();
			this.mName = b.ReadString();
			this.mColorKeyManager.Deserialize(b);
			this.mAlphaKeyManager.Deserialize(b);
			this.mLockSizeAspect = b.ReadBoolean();
			this.mAdditive = b.ReadBoolean();
			this.mAdditiveWithNormal = b.ReadBoolean();
			this.mFlipX = b.ReadBoolean();
			this.mFlipY = b.ReadBoolean();
			this.mLoopTimeline = b.ReadBoolean();
			this.mAlignAngleToMotion = b.ReadBoolean();
			this.mSingle = b.ReadBoolean();
			this.mMotionAngleOffset = b.ReadFloat();
			this.mInitAngle = b.ReadFloat();
			this.mAngleRange = b.ReadFloat();
			this.mEmitterAttachPct = b.ReadFloat();
			this.mNumSameColorKeyInRow = (int)b.ReadLong();
			this.mNumCreated = (int)b.ReadLong();
			this.mRefXOff = (int)b.ReadLong();
			this.mRefYOff = (int)b.ReadLong();
			this.mImageRate = (int)b.ReadLong();
			this.mSerialIndex = (int)b.ReadLong();
			this.mInitAngleStep = b.ReadFloat();
			this.mLastSpawnAngle = b.ReadFloat();
			this.mRandomStartCel = b.ReadBoolean();
			this.mXOff = (int)b.ReadLong();
			this.mYOff = (int)b.ReadLong();
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x00040EF8 File Offset: 0x0003F0F8
		public void GetCreationParameters(int current_frame, out int life_frames, out float emit_frame, out ParticleSettings kfdata, out ParticleVariance vardata)
		{
			this.mSettingsTimeLine.Update(current_frame);
			this.mVarTimeLine.Update(current_frame);
			if (this.mSettingsTimeLine.mKeyFrames.size<KeyFrame>() == 0)
			{
				kfdata = new ParticleSettings();
				this.mSettingsTimeLine.mCurrentSettings = kfdata;
			}
			else
			{
				kfdata = (ParticleSettings)this.mSettingsTimeLine.mCurrentSettings;
			}
			if (this.mVarTimeLine.mKeyFrames.size<KeyFrame>() == 0)
			{
				vardata = new ParticleVariance();
				this.mVarTimeLine.mCurrentSettings = vardata;
			}
			else
			{
				vardata = (ParticleVariance)this.mVarTimeLine.mCurrentSettings;
			}
			if (kfdata.mLife == -1)
			{
				life_frames = -1;
			}
			else
			{
				int mLife = kfdata.mLife;
				Common.SAFE_RAND((float)(vardata.mLifeVar / 2));
				Common.SAFE_RAND((float)(vardata.mLifeVar / 2));
				life_frames = this.GetRandomizedLife();
				if (life_frames < 0)
				{
					life_frames = 0;
				}
			}
			float num = (float)(kfdata.mNumber + (int)Common.SAFE_RAND((float)vardata.mNumberVar));
			if (num == 0f)
			{
				emit_frame = float.MaxValue;
				return;
			}
			emit_frame = 100f / num;
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00041018 File Offset: 0x0003F218
		public int GetRandomizedLife()
		{
			ParticleSettings particleSettings = this.mSettingsTimeLine.mCurrentSettings as ParticleSettings;
			ParticleVariance particleVariance = this.mVarTimeLine.mCurrentSettings as ParticleVariance;
			if (particleSettings.mLife == -1)
			{
				return -1;
			}
			int num;
			if (!this.mUseNewLifeRandomization)
			{
				num = particleSettings.mLife - (int)Common.SAFE_RAND((float)(particleVariance.mLifeVar / 2)) + (int)Common.SAFE_RAND((float)(particleVariance.mLifeVar / 2));
			}
			else
			{
				num = particleSettings.mLife + (int)Common.SAFE_RAND((float)particleVariance.mLifeVar);
			}
			if (num < 0)
			{
				num = 0;
			}
			return 10 * num;
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x000410A4 File Offset: 0x0003F2A4
		public ParticleSettings AddSettingsKeyFrame(int frame_time, ParticleSettings @params, int second_frame_time, bool make_new)
		{
			this.mSettingsTimeLine.AddKeyFrame(frame_time, @params);
			if (second_frame_time != -1)
			{
				ParticleSettings k = new ParticleSettings(@params);
				this.mSettingsTimeLine.AddKeyFrame(second_frame_time, k);
				if (make_new)
				{
					return new ParticleSettings(@params);
				}
			}
			return null;
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x000410E2 File Offset: 0x0003F2E2
		public ParticleSettings AddSettingsKeyFrame(int frame_time, ParticleSettings @params, int second_frame_time)
		{
			return this.AddSettingsKeyFrame(frame_time, @params, second_frame_time, false);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x000410EE File Offset: 0x0003F2EE
		public ParticleSettings AddSettingsKeyFrame(int frame_time, ParticleSettings @params)
		{
			return this.AddSettingsKeyFrame(frame_time, @params, -1, false);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x000410FC File Offset: 0x0003F2FC
		public ParticleVariance AddVarianceKeyFrame(int frame_time, ParticleVariance @params, int second_frame_time, bool make_new)
		{
			this.mVarTimeLine.AddKeyFrame(frame_time, @params);
			if (second_frame_time != -1)
			{
				ParticleVariance k = new ParticleVariance(@params);
				this.mSettingsTimeLine.AddKeyFrame(second_frame_time, k);
				if (make_new)
				{
					return new ParticleVariance(@params);
				}
			}
			return null;
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0004113A File Offset: 0x0003F33A
		public ParticleVariance AddVarianceKeyFrame(int frame_time, ParticleVariance @params, int second_frame_time)
		{
			return this.AddVarianceKeyFrame(frame_time, @params, second_frame_time, false);
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00041146 File Offset: 0x0003F346
		public ParticleVariance AddVarianceKeyFrame(int frame_time, ParticleVariance @params)
		{
			return this.AddVarianceKeyFrame(frame_time, @params, -1, false);
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00041154 File Offset: 0x0003F354
		public ParticleSettings GetSettingsKeyFrame(int frame_time)
		{
			for (int i = 0; i < this.mSettingsTimeLine.mKeyFrames.size<KeyFrame>(); i++)
			{
				if (this.mSettingsTimeLine.mKeyFrames[i].first == frame_time)
				{
					return (ParticleSettings)this.mSettingsTimeLine.mKeyFrames[i].second;
				}
			}
			return null;
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x000411B4 File Offset: 0x0003F3B4
		public ParticleVariance GetVarianceKeyFrame(int frame_time)
		{
			for (int i = 0; i < this.mVarTimeLine.mKeyFrames.size<KeyFrame>(); i++)
			{
				if (this.mVarTimeLine.mKeyFrames[i].first == frame_time)
				{
					return (ParticleVariance)this.mVarTimeLine.mKeyFrames[i].second;
				}
			}
			return null;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00041212 File Offset: 0x0003F412
		public bool EndOfSettingsTimeLine(int frame)
		{
			return this.mSettingsTimeLine.mKeyFrames.size<KeyFrame>() == 0 || frame >= this.mSettingsTimeLine.mKeyFrames.back<KeyFrame>().first;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00041243 File Offset: 0x0003F443
		public bool EndOfVarianceTimeLine(int frame)
		{
			return this.mVarTimeLine.mKeyFrames.size<KeyFrame>() == 0 || frame >= this.mVarTimeLine.mKeyFrames.back<KeyFrame>().first;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00041274 File Offset: 0x0003F474
		public float GetSpawnAngle()
		{
			if (this.mInitAngleStep == 0f)
			{
				return this.mInitAngle - Common.FloatRange(0f, this.mAngleRange / 2f) + Common.FloatRange(0f, this.mAngleRange / 2f);
			}
			if (Common._eq(this.mLastSpawnAngle, 3.4028235E+38f))
			{
				this.mLastSpawnAngle = this.mInitAngle - Common.FloatRange(0f, this.mAngleRange / 2f) + Common.FloatRange(0f, this.mAngleRange / 2f);
			}
			else
			{
				this.mLastSpawnAngle += this.mInitAngleStep;
				if (Common._geq(this.mLastSpawnAngle, this.mInitAngle + this.mAngleRange / 2f))
				{
					this.mLastSpawnAngle -= this.mAngleRange;
				}
			}
			return this.mLastSpawnAngle;
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0004135D File Offset: 0x0003F55D
		public void LoopTimeLine(bool l)
		{
			this.mSettingsTimeLine.mLoop = l;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0004136B File Offset: 0x0003F56B
		public void LoopVarTimeLine(bool l)
		{
			this.mVarTimeLine.mLoop = l;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0004137C File Offset: 0x0003F57C
		public LifetimeSettings AddSettingAtLifePct(float pct, LifetimeSettings s, float second_frame_pct, bool make_new)
		{
			this.mLifePctSettings.Add(new LifetimeSettingPct(pct, s));
			this.mLifePctSettings.Sort(new LifePctSort());
			if (second_frame_pct >= 0f)
			{
				this.AddSettingAtLifePct(second_frame_pct, new LifetimeSettings(s), -1f, false);
				if (make_new)
				{
					return new LifetimeSettings(s);
				}
			}
			return null;
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x000413D3 File Offset: 0x0003F5D3
		public LifetimeSettings AddSettingAtLifePct(float pct, LifetimeSettings s, float second_frame_pct)
		{
			return this.AddSettingAtLifePct(pct, s, second_frame_pct, false);
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x000413DF File Offset: 0x0003F5DF
		public LifetimeSettings AddSettingAtLifePct(float pct, LifetimeSettings s)
		{
			return this.AddSettingAtLifePct(pct, s, -1f, false);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x000413EF File Offset: 0x0003F5EF
		public int GetSettingsTimeLineSize()
		{
			return this.mSettingsTimeLine.mKeyFrames.size<KeyFrame>();
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00041401 File Offset: 0x0003F601
		public int GetVarTimeLineSize()
		{
			return this.mVarTimeLine.mKeyFrames.size<KeyFrame>();
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00041414 File Offset: 0x0003F614
		public SexyColor GetNextKeyColor()
		{
			if (this.mNumSameColorKeyInRow <= 0)
			{
				return SexyColor.White;
			}
			if (++this.mSameColorKeyCount >= this.mNumSameColorKeyInRow)
			{
				this.mSameColorKeyCount = 0;
				this.mLastColorKeyIndex = (this.mLastColorKeyIndex + 1) % this.mColorKeyManager.GetNumKeys();
			}
			return this.mColorKeyManager.GetColorByIndex(this.mLastColorKeyIndex);
		}

		// Token: 0x04000A22 RID: 2594
		public TimeLine mSettingsTimeLine = new TimeLine();

		// Token: 0x04000A23 RID: 2595
		public TimeLine mVarTimeLine = new TimeLine();

		// Token: 0x04000A24 RID: 2596
		public int mSameColorKeyCount;

		// Token: 0x04000A25 RID: 2597
		public int mLastColorKeyIndex;

		// Token: 0x04000A26 RID: 2598
		public float mLastSpawnAngle = float.MaxValue;

		// Token: 0x04000A27 RID: 2599
		public bool mImageSetByPINLoader;

		// Token: 0x04000A28 RID: 2600
		public List<LifetimeSettingPct> mLifePctSettings = new List<LifetimeSettingPct>();

		// Token: 0x04000A29 RID: 2601
		public Image mImage;

		// Token: 0x04000A2A RID: 2602
		public string mImageName = "";

		// Token: 0x04000A2B RID: 2603
		public string mName = "";

		// Token: 0x04000A2C RID: 2604
		public ColorKeyManager mColorKeyManager;

		// Token: 0x04000A2D RID: 2605
		public ColorKeyManager mAlphaKeyManager;

		// Token: 0x04000A2E RID: 2606
		public bool mUseNewLifeRandomization;

		// Token: 0x04000A2F RID: 2607
		public bool mLockSizeAspect = true;

		// Token: 0x04000A30 RID: 2608
		public bool mAdditive;

		// Token: 0x04000A31 RID: 2609
		public bool mAdditiveWithNormal;

		// Token: 0x04000A32 RID: 2610
		public bool mFlipX;

		// Token: 0x04000A33 RID: 2611
		public bool mFlipY;

		// Token: 0x04000A34 RID: 2612
		public bool mLoopTimeline;

		// Token: 0x04000A35 RID: 2613
		public bool mAlignAngleToMotion;

		// Token: 0x04000A36 RID: 2614
		public bool mSingle;

		// Token: 0x04000A37 RID: 2615
		public float mMotionAngleOffset;

		// Token: 0x04000A38 RID: 2616
		public float mInitAngle;

		// Token: 0x04000A39 RID: 2617
		public float mAngleRange;

		// Token: 0x04000A3A RID: 2618
		public float mInitAngleStep;

		// Token: 0x04000A3B RID: 2619
		public float mEmitterAttachPct;

		// Token: 0x04000A3C RID: 2620
		public int mNumSameColorKeyInRow;

		// Token: 0x04000A3D RID: 2621
		public int mNumCreated;

		// Token: 0x04000A3E RID: 2622
		public int mRefXOff;

		// Token: 0x04000A3F RID: 2623
		public int mRefYOff;

		// Token: 0x04000A40 RID: 2624
		public int mImageRate = 1;

		// Token: 0x04000A41 RID: 2625
		public int mXOff;

		// Token: 0x04000A42 RID: 2626
		public int mYOff;

		// Token: 0x04000A43 RID: 2627
		public bool mRandomStartCel;

		// Token: 0x04000A44 RID: 2628
		public int mSerialIndex = -1;
	}
}
