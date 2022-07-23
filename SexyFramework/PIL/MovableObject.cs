using System;
using System.Collections.Generic;
using SexyFramework.Misc;

namespace SexyFramework.PIL
{
	// Token: 0x02000162 RID: 354
	public class MovableObject : IDisposable
	{
		// Token: 0x06000C46 RID: 3142 RVA: 0x0003AE70 File Offset: 0x00039070
		protected LifetimeSettings GetInterpLifetimeSettings()
		{
			if (this.mLifetimeKeyFrames.size<LifetimeSettingKeyFrame>() == 0)
			{
				return new LifetimeSettings();
			}
			if (this.mKeyFrameIndex == this.mLifetimeKeyFrames.size<LifetimeSettingKeyFrame>() || this.mKeyFrameIndex + 1 == this.mLifetimeKeyFrames.size<LifetimeSettingKeyFrame>())
			{
				return this.mLifetimeKeyFrames[this.mKeyFrameIndex].second;
			}
			LifetimeSettings second = this.mLifetimeKeyFrames[this.mKeyFrameIndex + 1].second;
			LifetimeSettings second2 = this.mLifetimeKeyFrames[this.mKeyFrameIndex].second;
			float num = (float)(this.mUpdateCount - this.mLifetimeKeyFrames[this.mKeyFrameIndex].first) / (float)(this.mLifetimeKeyFrames[this.mKeyFrameIndex + 1].first - this.mLifetimeKeyFrames[this.mKeyFrameIndex].first);
			this.mInterpLifetimeSettings.Reset();
			this.mInterpLifetimeSettings.mMotionRandMult += (second.mMotionRandMult - second2.mMotionRandMult) * num;
			this.mInterpLifetimeSettings.mSizeXMult += (second.mSizeXMult - second2.mSizeXMult) * num;
			this.mInterpLifetimeSettings.mSizeYMult += (second.mSizeYMult - second2.mSizeYMult) * num;
			this.mInterpLifetimeSettings.mSpinMult += (second.mSpinMult - second2.mSpinMult) * num;
			this.mInterpLifetimeSettings.mVelocityMult += (second.mVelocityMult - second2.mVelocityMult) * num;
			this.mInterpLifetimeSettings.mWeightMult += (second.mWeightMult - second2.mWeightMult) * num;
			this.mInterpLifetimeSettings.mZoomMult += (second.mZoomMult - second2.mZoomMult) * num;
			this.mInterpLifetimeSettings.mNumberMult += (second.mNumberMult - second2.mNumberMult) * num;
			return this.mInterpLifetimeSettings;
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x0003B068 File Offset: 0x00039268
		public MovableObject()
		{
			this.Reset();
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x0003B0A2 File Offset: 0x000392A2
		public MovableObject(MovableObject rhs)
			: this()
		{
			this.CopyFrom(rhs);
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x0003B0B4 File Offset: 0x000392B4
		public virtual void Dispose()
		{
			for (int i = 0; i < this.mLifetimeKeyFrames.size<LifetimeSettingKeyFrame>(); i++)
			{
				this.mLifetimeKeyFrames[i].second = null;
			}
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x0003B0EC File Offset: 0x000392EC
		public void CopyFrom(MovableObject rhs)
		{
			if (this == rhs)
			{
				return;
			}
			this.mLife = rhs.mLife;
			this.mVX = rhs.mVX;
			this.mVY = rhs.mVY;
			this.mMotionRand = rhs.mMotionRand;
			this.mX = rhs.mX;
			this.mY = rhs.mY;
			this.mWeight = rhs.mWeight;
			this.mAngle = rhs.mAngle;
			this.mSpin = rhs.mSpin;
			this.mBounce = rhs.mBounce;
			this.mUpdateCount = 0;
			this.mKeyFrameIndex = 0;
			this.mOriginalWeight = rhs.mOriginalWeight;
			this.mOriginalBounce = rhs.mOriginalBounce;
			this.mMotionRandAccum = 0f;
			this.mAX = 0f;
			this.mAY = 0f;
			this.mInitialized = rhs.mInitialized;
			for (int i = 0; i < this.mLifetimeKeyFrames.Count; i++)
			{
				this.mLifetimeKeyFrames[i].second = null;
			}
			this.mLifetimeKeyFrames.Clear();
			for (int j = 0; j < rhs.mLifetimeKeyFrames.size<LifetimeSettingKeyFrame>(); j++)
			{
				this.AddLifetimeKeyFrame((this.mLife == 0) ? 0f : ((float)rhs.mLifetimeKeyFrames[j].first / (float)this.mLife), new LifetimeSettings(rhs.mLifetimeKeyFrames[j].second));
			}
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x0003B258 File Offset: 0x00039458
		public virtual void Serialize(SexyBuffer b)
		{
			b.WriteLong((long)this.mKeyFrameIndex);
			b.WriteFloat(this.mOriginalWeight);
			b.WriteFloat(this.mOriginalBounce);
			b.WriteFloat(this.mMotionRandAccum);
			b.WriteFloat(this.mAX);
			b.WriteFloat(this.mAY);
			b.WriteFloat(this.mX);
			b.WriteFloat(this.mY);
			b.WriteBoolean(this.mInitialized);
			b.WriteLong((long)this.mUpdateCount);
			b.WriteLong((long)this.mLife);
			b.WriteFloat(this.mVX);
			b.WriteFloat(this.mVY);
			b.WriteFloat(this.mMotionRand);
			b.WriteFloat(this.mWeight);
			b.WriteFloat(this.mAngle);
			b.WriteFloat(this.mSpin);
			b.WriteFloat(this.mBounce);
			this.mCurrentLifetimeSettings.Serialize(b);
			b.WriteLong((long)this.mLifetimeKeyFrames.Count);
			for (int i = 0; i < this.mLifetimeKeyFrames.Count; i++)
			{
				b.WriteLong((long)this.mLifetimeKeyFrames[i].first);
				this.mLifetimeKeyFrames[i].second.Serialize(b);
			}
			b.WriteLong((long)this.mDeflectorCollMap.Count);
			foreach (KeyValuePair<Deflector, DeflectorCollInfo> keyValuePair in this.mDeflectorCollMap)
			{
				b.WriteLong((long)keyValuePair.Key.mSerialIndex);
				b.WriteLong((long)keyValuePair.Value.mLastCollFrame);
				b.WriteBoolean(keyValuePair.Value.mIgnoresDeflector);
			}
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x0003B42C File Offset: 0x0003962C
		public virtual void Deserialize(SexyBuffer b, Dictionary<int, Deflector> deflector_ptr_map)
		{
			this.mKeyFrameIndex = (int)b.ReadLong();
			this.mOriginalWeight = b.ReadFloat();
			this.mOriginalBounce = b.ReadFloat();
			this.mMotionRandAccum = b.ReadFloat();
			this.mAX = b.ReadFloat();
			this.mAY = b.ReadFloat();
			this.mX = b.ReadFloat();
			this.mY = b.ReadFloat();
			this.mInitialized = b.ReadBoolean();
			this.mUpdateCount = (int)b.ReadLong();
			this.mLife = (int)b.ReadLong();
			this.mVX = b.ReadFloat();
			this.mVY = b.ReadFloat();
			this.mMotionRand = b.ReadFloat();
			this.mWeight = b.ReadFloat();
			this.mAngle = b.ReadFloat();
			this.mSpin = b.ReadFloat();
			this.mBounce = b.ReadFloat();
			this.mCurrentLifetimeSettings.Deserialize(b);
			int num = (int)b.ReadLong();
			this.mLifetimeKeyFrames.Clear();
			for (int i = 0; i < num; i++)
			{
				int f = (int)b.ReadLong();
				LifetimeSettings lifetimeSettings = new LifetimeSettings();
				lifetimeSettings.Deserialize(b);
				this.mLifetimeKeyFrames.Add(new LifetimeSettingKeyFrame(f, lifetimeSettings));
			}
			this.mDeflectorCollMap.Clear();
			num = (int)b.ReadLong();
			for (int j = 0; j < num; j++)
			{
				int num2 = (int)b.ReadLong();
				int f2 = (int)b.ReadLong();
				bool b2 = b.ReadBoolean();
				if (deflector_ptr_map.ContainsKey(num2))
				{
					Deflector deflector = deflector_ptr_map[num2];
					this.mDeflectorCollMap.Add(deflector, new DeflectorCollInfo(f2, b2));
				}
			}
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x0003B5CD File Offset: 0x000397CD
		public virtual void Launch(float angle, float velocity)
		{
			this.mVX = (float)Math.Cos((double)angle) * velocity;
			this.mVY = -(float)Math.Sin((double)angle) * velocity;
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x0003B5F0 File Offset: 0x000397F0
		public virtual LifetimeSettings AddLifetimeKeyFrame(float pct, LifetimeSettings s, float second_frame_pct, bool make_new)
		{
			LifetimeSettingKeyFrame lifetimeSettingKeyFrame = new LifetimeSettingKeyFrame();
			lifetimeSettingKeyFrame.first = (int)(pct * (float)this.mLife);
			lifetimeSettingKeyFrame.second = s;
			lifetimeSettingKeyFrame.second.mPct = pct;
			this.mLifetimeKeyFrames.Add(lifetimeSettingKeyFrame);
			this.mLifetimeKeyFrames.Sort(new LifeFrameSort());
			if (second_frame_pct >= 0f)
			{
				this.AddLifetimeKeyFrame(second_frame_pct, new LifetimeSettings(s));
				if (make_new)
				{
					return new LifetimeSettings(s);
				}
			}
			return null;
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x0003B664 File Offset: 0x00039864
		public virtual LifetimeSettings AddLifetimeKeyFrame(float pct, LifetimeSettings s, float second_frame_pct)
		{
			return this.AddLifetimeKeyFrame(pct, s, second_frame_pct, false);
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x0003B670 File Offset: 0x00039870
		public virtual LifetimeSettings AddLifetimeKeyFrame(float pct, LifetimeSettings s)
		{
			return this.AddLifetimeKeyFrame(pct, s, -1f, false);
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x0003B680 File Offset: 0x00039880
		public virtual void ClearLifetimeFrames()
		{
			this.mLifetimeKeyFrames.Clear();
			this.AddLifetimeKeyFrame(0f, new LifetimeSettings(this.mCurrentLifetimeSettings));
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x0003B6A4 File Offset: 0x000398A4
		public virtual void Reset()
		{
			this.mLife = -1;
			this.mUpdateCount = 0;
			this.mMotionRand = 0f;
			this.mWeight = 0f;
			this.mAngle = 0f;
			this.mSpin = 0f;
			this.mX = 0f;
			this.mY = 0f;
			this.mKeyFrameIndex = 0;
			this.mOriginalWeight = 1f;
			this.mOriginalBounce = 0f;
			this.mInitialized = false;
			this.mMotionRandAccum = 0f;
			this.mAX = 0f;
			this.mAY = 0f;
			this.mVX = 0f;
			this.mVY = 0f;
			for (int i = 0; i < this.mLifetimeKeyFrames.size<LifetimeSettingKeyFrame>(); i++)
			{
				this.mLifetimeKeyFrames[i] = null;
			}
			this.mLifetimeKeyFrames.Clear();
			this.mCurrentLifetimeSettings = new LifetimeSettings();
			this.mDeflectorCollMap.Clear();
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x0003B7A0 File Offset: 0x000399A0
		public virtual void Update()
		{
			if (!this.mInitialized)
			{
				this.mInitialized = true;
				this.mOriginalWeight = this.mWeight;
				this.mOriginalBounce = this.mBounce;
			}
			this.mUpdateCount++;
			if (this.Dead())
			{
				return;
			}
			if (this.mLifetimeKeyFrames.size<LifetimeSettingKeyFrame>() > 0 && this.mKeyFrameIndex + 1 < this.mLifetimeKeyFrames.size<LifetimeSettingKeyFrame>() && this.mUpdateCount >= this.mLifetimeKeyFrames[this.mKeyFrameIndex + 1].first)
			{
				this.mKeyFrameIndex++;
			}
			this.mCurrentLifetimeSettings = this.GetInterpLifetimeSettings();
			this.mBounce = this.mOriginalBounce * this.mCurrentLifetimeSettings.mBounceMult;
			this.mWeight = this.mOriginalWeight * this.mCurrentLifetimeSettings.mWeightMult;
			this.mVY += this.mWeight;
			this.mVX += this.mAX;
			this.mVY += this.mAY;
			this.mX += this.mVX * this.mCurrentLifetimeSettings.mVelocityMult + this.mMotionRandAccum;
			this.mY += this.mVY * this.mCurrentLifetimeSettings.mVelocityMult + this.mMotionRandAccum;
			if (this.mMotionRand > 0f)
			{
				this.mMotionRandAccum += (-this.mMotionRand / 2f + Common.FloatRange(0f, this.mMotionRand)) * this.mCurrentLifetimeSettings.mMotionRandMult / 10f;
			}
			this.mAngle += this.mSpin * this.mCurrentLifetimeSettings.mSpinMult;
			this.mAX = (this.mAY = 0f);
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0003B97A File Offset: 0x00039B7A
		public virtual bool Dead()
		{
			return this.mLife >= 0 && this.mUpdateCount >= this.mLife;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0003B998 File Offset: 0x00039B98
		public virtual void ApplyAcceleration(float ax, float ay)
		{
			this.mAX += ax;
			this.mAY += ay;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0003B9B6 File Offset: 0x00039BB6
		public virtual float GetX()
		{
			return this.mX;
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x0003B9BE File Offset: 0x00039BBE
		public virtual float GetY()
		{
			return this.mY;
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x0003B9C6 File Offset: 0x00039BC6
		public virtual void SetX(float x)
		{
			this.mX = x;
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x0003B9CF File Offset: 0x00039BCF
		public virtual void SetY(float y)
		{
			this.mY = y;
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0003B9D8 File Offset: 0x00039BD8
		public virtual void SetXY(float x, float y)
		{
			this.SetX(x);
			this.SetY(y);
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0003B9E8 File Offset: 0x00039BE8
		public virtual bool CanInteract()
		{
			return true;
		}

		// Token: 0x040009A2 RID: 2466
		protected List<LifetimeSettingKeyFrame> mLifetimeKeyFrames = new List<LifetimeSettingKeyFrame>();

		// Token: 0x040009A3 RID: 2467
		protected LifetimeSettings mCurrentLifetimeSettings = new LifetimeSettings();

		// Token: 0x040009A4 RID: 2468
		protected int mKeyFrameIndex;

		// Token: 0x040009A5 RID: 2469
		protected float mOriginalWeight;

		// Token: 0x040009A6 RID: 2470
		protected float mOriginalBounce;

		// Token: 0x040009A7 RID: 2471
		protected float mMotionRandAccum;

		// Token: 0x040009A8 RID: 2472
		protected float mAX;

		// Token: 0x040009A9 RID: 2473
		protected float mAY;

		// Token: 0x040009AA RID: 2474
		protected float mX;

		// Token: 0x040009AB RID: 2475
		protected float mY;

		// Token: 0x040009AC RID: 2476
		protected bool mInitialized;

		// Token: 0x040009AD RID: 2477
		public Dictionary<Deflector, DeflectorCollInfo> mDeflectorCollMap = new Dictionary<Deflector, DeflectorCollInfo>();

		// Token: 0x040009AE RID: 2478
		public int mUpdateCount;

		// Token: 0x040009AF RID: 2479
		public int mLife;

		// Token: 0x040009B0 RID: 2480
		public float mVX;

		// Token: 0x040009B1 RID: 2481
		public float mVY;

		// Token: 0x040009B2 RID: 2482
		public float mMotionRand;

		// Token: 0x040009B3 RID: 2483
		public float mWeight;

		// Token: 0x040009B4 RID: 2484
		public float mAngle;

		// Token: 0x040009B5 RID: 2485
		public float mSpin;

		// Token: 0x040009B6 RID: 2486
		public float mBounce;

		// Token: 0x040009B7 RID: 2487
		protected LifetimeSettings mInterpLifetimeSettings = new LifetimeSettings();
	}
}
