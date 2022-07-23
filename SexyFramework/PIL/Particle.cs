using System;
using System.Collections.Generic;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.PIL
{
	// Token: 0x0200016F RID: 367
	public class Particle : MovableObject
	{
		// Token: 0x06000CFB RID: 3323 RVA: 0x0003FACF File Offset: 0x0003DCCF
		public override void Reset()
		{
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x0003FAD1 File Offset: 0x0003DCD1
		public override void Deserialize(SexyBuffer b, Dictionary<int, Deflector> deflector_ptr_map)
		{
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0003FAD4 File Offset: 0x0003DCD4
		public Particle()
		{
			this.mColorKeyManager = new ColorKeyManager();
			this.mAlphaKeyManager = new ColorKeyManager();
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x0003FB40 File Offset: 0x0003DD40
		public Particle(float spawn_angle, float velocity)
		{
			this.Reset(spawn_angle, velocity);
			this.mColorKeyManager = new ColorKeyManager();
			this.mAlphaKeyManager = new ColorKeyManager();
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0003FBB3 File Offset: 0x0003DDB3
		public override void Dispose()
		{
			this.mColorKeyManager = null;
			this.mAlphaKeyManager = null;
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0003FBC4 File Offset: 0x0003DDC4
		private void DoDraw(Graphics g, Transform t, float scale)
		{
			if (g.Is3D())
			{
				g.DrawImageTransformF(this.mImage, t, this.mImage.GetCelRect(this.mImageCel), (this.mX + (float)this.mParentType.mXOff * this.mCurXSize + (float)this.mRefXOff * this.mCurXSize) * scale, (this.mY + (float)this.mParentType.mYOff * this.mCurYSize + (float)this.mRefYOff * this.mCurYSize) * scale);
				return;
			}
			g.DrawImageTransform(this.mImage, t, this.mImage.GetCelRect(this.mImageCel), (this.mX + (float)this.mParentType.mXOff * this.mCurXSize + (float)this.mRefXOff * this.mCurXSize) * scale, (this.mY + (float)this.mParentType.mYOff * this.mCurYSize + (float)this.mRefYOff * this.mCurYSize) * scale);
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0003FCC2 File Offset: 0x0003DEC2
		private void DoDraw(Graphics g, Transform t)
		{
			this.DoDraw(g, t, 1f);
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0003FCD4 File Offset: 0x0003DED4
		public virtual void Reset(float spawn_angle, float velocity)
		{
			base.Reset();
			this.mForceDeletion = false;
			this.mForceFadeoutRate = 0f;
			this.mImage = null;
			this.mCurXSize = 1f;
			this.mCurYSize = 1f;
			this.mLockSizeAspect = true;
			this.mOriginalXSize = 1f;
			this.mOriginalYSize = 1f;
			this.mParentType = null;
			this.mAdditive = (this.mAdditiveWithNormal = false);
			this.mFlipX = (this.mFlipY = false);
			this.mRefXOff = (this.mRefYOff = 0);
			this.mLife = -1;
			this.mAlignAngleToMotion = false;
			this.mMotionAngleOffset = 0f;
			this.mHasBeenVisible = false;
			this.mLastFrameWasVisible = true;
			this.Launch(spawn_angle, velocity);
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0003FD9C File Offset: 0x0003DF9C
		public override void Update()
		{
			if (!this.mInitialized)
			{
				this.mOriginalXSize = this.mCurXSize;
				this.mOriginalYSize = this.mCurYSize;
			}
			base.Update();
			if (this.Dead())
			{
				return;
			}
			if (this.mImage != null && this.mImageRate > 0 && this.mUpdateCount % this.mImageRate == 0)
			{
				this.mImageCel = (this.mImageCel + 1) % (this.mImage.mNumCols * this.mImage.mNumRows);
			}
			if (this.mAlignAngleToMotion)
			{
				float num = Common.AngleBetweenPoints(this.mVX, -this.mVY, 0f, 0f);
				this.mAngle = num - this.mMotionAngleOffset;
			}
			this.mColorKeyManager.Update(this.mX, this.mY);
			if (this.mForceFadeoutRate <= 0f)
			{
				this.mAlphaKeyManager.Update(this.mX, this.mY);
			}
			else
			{
				if (this.mForcedAlpha < 0f)
				{
					this.mForcedAlpha = (float)this.mAlphaKeyManager.GetColor().mAlpha;
				}
				this.mForcedAlpha -= this.mForceFadeoutRate;
				if (this.mForcedAlpha <= 0f)
				{
					this.mForceDeletion = true;
				}
			}
			LifetimeSettings interpLifetimeSettings = base.GetInterpLifetimeSettings();
			this.mCurXSize = this.mOriginalXSize * interpLifetimeSettings.mSizeXMult;
			this.mCurYSize = this.mOriginalYSize * interpLifetimeSettings.mSizeYMult;
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0003FF08 File Offset: 0x0003E108
		public virtual void Draw(Graphics g, float alpha_pct, SexyColor tint_color, float tint_strength, float scale)
		{
			if (this.mImage == null || alpha_pct == 0f || this.mCurXSize == 0f || (!this.mLockSizeAspect && this.mCurYSize == 0f))
			{
				this.mLastFrameWasVisible = false;
				return;
			}
			this.mGlobalTransform.Reset();
			float num = (this.mLockSizeAspect ? this.mCurXSize : this.mCurYSize);
			if (!Common._eq(this.mCurXSize * scale, 1f, 1E-06f) || (!Common._eq(this.mCurYSize * scale, 1f, 1E-06f) && !this.mLockSizeAspect) || this.mFlipX || this.mFlipY)
			{
				this.mGlobalTransform.Scale(this.mFlipX ? (-this.mCurXSize) : this.mCurXSize, num * (this.mFlipY ? (-1f) : 1f));
			}
			if (this.mAngle != 0f)
			{
				this.mGlobalTransform.Translate((float)this.mRefXOff * this.mCurXSize, (float)this.mRefYOff * num);
				this.mGlobalTransform.RotateRad(-this.mAngle);
				this.mGlobalTransform.Translate((float)(-(float)this.mRefXOff) * this.mCurXSize, (float)(-(float)this.mRefYOff) * num);
			}
			SexyColor color = this.mColorKeyManager.GetColor();
			color.mRed -= (int)((float)(color.mRed - tint_color.mRed) * tint_strength);
			color.mGreen -= (int)((float)(color.mGreen - tint_color.mGreen) * tint_strength);
			color.mBlue -= (int)((float)(color.mBlue - tint_color.mBlue) * tint_strength);
			if (this.mForcedAlpha > 0f)
			{
				color.mAlpha = (int)this.mForcedAlpha;
			}
			else
			{
				color.mAlpha = this.mAlphaKeyManager.GetColor().mAlpha;
			}
			if (!Common._eq(alpha_pct, 1f, 1E-06f) || color != SexyColor.White)
			{
				g.SetColorizeImages(true);
				color.mAlpha = (int)((float)color.mAlpha * alpha_pct);
				g.SetColor(color);
			}
			if (color.mAlpha > 0)
			{
				this.mHasBeenVisible = true;
			}
			this.mLastFrameWasVisible = color.mAlpha != 0;
			if (this.mAdditive)
			{
				if (this.mAdditiveWithNormal)
				{
					this.DoDraw(g, this.mGlobalTransform, scale);
				}
				g.SetDrawMode(1);
			}
			this.DoDraw(g, this.mGlobalTransform, scale);
			g.SetColorizeImages(false);
			g.SetDrawMode(0);
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x000401A6 File Offset: 0x0003E3A6
		public virtual void Draw(Graphics g, float alpha_pct, SexyColor tint_color, float tint_strength)
		{
			this.Draw(g, alpha_pct, tint_color, tint_strength, 1f);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x000401B8 File Offset: 0x0003E3B8
		public override bool Dead()
		{
			return this.mForceDeletion || base.Dead();
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x000401CA File Offset: 0x0003E3CA
		public virtual float GetWidth()
		{
			return (float)this.mImage.GetWidth() * this.mCurXSize;
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x000401DF File Offset: 0x0003E3DF
		public virtual float GetHeight()
		{
			if (this.mLockSizeAspect)
			{
				return (float)this.mImage.GetHeight() * this.mCurXSize;
			}
			return (float)this.mImage.GetHeight() * this.mCurYSize;
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00040210 File Offset: 0x0003E410
		public virtual Rect GetRect()
		{
			float width = this.GetWidth();
			float height = this.GetHeight();
			this.mRect.mX = (int)(this.mX - width / 2f);
			this.mRect.mY = (int)(this.mY - height / 2f);
			this.mRect.mWidth = (int)width;
			this.mRect.mHeight = (int)height;
			return this.mRect;
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00040280 File Offset: 0x0003E480
		public override void Serialize(SexyBuffer b)
		{
			base.Serialize(b);
			b.WriteFloat(this.mOriginalXSize);
			b.WriteFloat(this.mOriginalYSize);
			b.WriteLong((long)this.mImageCel);
			b.WriteLong((long)this.mParentType.mSerialIndex);
			this.mColorKeyManager.Serialize(b);
			this.mAlphaKeyManager.Serialize(b);
			b.WriteLong((long)this.mImageRate);
			b.WriteLong((long)this.mRefXOff);
			b.WriteLong((long)this.mRefYOff);
			b.WriteBoolean(this.mAdditive);
			b.WriteBoolean(this.mAdditiveWithNormal);
			b.WriteBoolean(this.mLockSizeAspect);
			b.WriteBoolean(this.mFlipX);
			b.WriteBoolean(this.mFlipY);
			b.WriteBoolean(this.mAlignAngleToMotion);
			b.WriteFloat(this.mMotionAngleOffset);
			b.WriteFloat(this.mCurXSize);
			b.WriteFloat(this.mCurYSize);
			b.WriteBoolean(this.mHasBeenVisible);
			b.WriteBoolean(this.mLastFrameWasVisible);
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00040390 File Offset: 0x0003E590
		public void Deserialize(SexyBuffer b, Dictionary<int, Deflector> deflector_ptr_map, Dictionary<int, ParticleType> ptype_ptr_map)
		{
			base.Deserialize(b, deflector_ptr_map);
			this.mOriginalXSize = b.ReadFloat();
			this.mOriginalYSize = b.ReadFloat();
			this.mImageCel = (int)b.ReadLong();
			int num = (int)b.ReadLong();
			if (ptype_ptr_map.ContainsKey(num))
			{
				this.mParentType = ptype_ptr_map[num];
			}
			this.mImage = this.mParentType.mImage;
			this.mColorKeyManager.Deserialize(b);
			this.mAlphaKeyManager.Deserialize(b);
			this.mImageRate = (int)b.ReadLong();
			this.mRefXOff = (int)b.ReadLong();
			this.mRefYOff = (int)b.ReadLong();
			this.mAdditive = b.ReadBoolean();
			this.mAdditiveWithNormal = b.ReadBoolean();
			this.mLockSizeAspect = b.ReadBoolean();
			this.mFlipX = b.ReadBoolean();
			this.mFlipY = b.ReadBoolean();
			this.mAlignAngleToMotion = b.ReadBoolean();
			this.mMotionAngleOffset = b.ReadFloat();
			this.mCurXSize = b.ReadFloat();
			this.mCurYSize = b.ReadFloat();
			this.mHasBeenVisible = b.ReadBoolean();
			this.mLastFrameWasVisible = b.ReadBoolean();
		}

		// Token: 0x04000A04 RID: 2564
		public int mPoolIndex = -1;

		// Token: 0x04000A05 RID: 2565
		public float mForcedAlpha = -100f;

		// Token: 0x04000A06 RID: 2566
		public float mOriginalXSize;

		// Token: 0x04000A07 RID: 2567
		public float mOriginalYSize;

		// Token: 0x04000A08 RID: 2568
		public string mParentName = "";

		// Token: 0x04000A09 RID: 2569
		public Image mImage;

		// Token: 0x04000A0A RID: 2570
		public ParticleType mParentType;

		// Token: 0x04000A0B RID: 2571
		public ColorKeyManager mColorKeyManager;

		// Token: 0x04000A0C RID: 2572
		public ColorKeyManager mAlphaKeyManager;

		// Token: 0x04000A0D RID: 2573
		public int mImageCel;

		// Token: 0x04000A0E RID: 2574
		public int mImageRate = 1;

		// Token: 0x04000A0F RID: 2575
		public int mRefXOff;

		// Token: 0x04000A10 RID: 2576
		public int mRefYOff;

		// Token: 0x04000A11 RID: 2577
		public Rect mRect = default(Rect);

		// Token: 0x04000A12 RID: 2578
		public int mWidth;

		// Token: 0x04000A13 RID: 2579
		public int mHeight;

		// Token: 0x04000A14 RID: 2580
		public bool mHasBeenVisible;

		// Token: 0x04000A15 RID: 2581
		public bool mLastFrameWasVisible = true;

		// Token: 0x04000A16 RID: 2582
		public bool mAdditive;

		// Token: 0x04000A17 RID: 2583
		public bool mAdditiveWithNormal;

		// Token: 0x04000A18 RID: 2584
		public bool mLockSizeAspect;

		// Token: 0x04000A19 RID: 2585
		public bool mFlipX;

		// Token: 0x04000A1A RID: 2586
		public bool mFlipY;

		// Token: 0x04000A1B RID: 2587
		public bool mAlignAngleToMotion;

		// Token: 0x04000A1C RID: 2588
		public bool mForceDeletion;

		// Token: 0x04000A1D RID: 2589
		public float mForceFadeoutRate;

		// Token: 0x04000A1E RID: 2590
		public float mMotionAngleOffset;

		// Token: 0x04000A1F RID: 2591
		public float mCurXSize;

		// Token: 0x04000A20 RID: 2592
		public float mCurYSize;

		// Token: 0x04000A21 RID: 2593
		protected Transform mGlobalTransform = new Transform();
	}
}
