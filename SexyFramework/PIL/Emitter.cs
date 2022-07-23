using System;
using System.Collections.Generic;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.PIL
{
	// Token: 0x02000163 RID: 355
	public class Emitter : MovableObject
	{
		// Token: 0x06000C5C RID: 3164 RVA: 0x0003B9EC File Offset: 0x00039BEC
		public void Serialize(SexyBuffer b, GlobalMembers.GetIdByImageFunc f)
		{
			base.Serialize(b);
			b.WriteLong((long)this.mHandle);
			b.WriteBoolean(this.mDisableQuadRep);
			b.WriteBoolean(this.mUseAlternateCalcMethod);
			b.WriteFloat(this.mNumSpawnAccumulator);
			b.WriteLong((long)this.mStartFrame);
			b.WriteBoolean(this.mParticlesMustHaveBeenVisible);
			b.WriteLong((long)this.mLineEmitterPoints.Count);
			for (int i = 0; i < this.mLineEmitterPoints.Count; i++)
			{
				this.mLineEmitterPoints[i].Serialize(b);
			}
			this.mScaleTimeLine.Serialize(b);
			this.mSettingsTimeLine.Serialize(b);
			b.WriteLong((long)this.mParticleTypeInfo.Count);
			for (int j = 0; j < this.mParticleTypeInfo.Count; j++)
			{
				this.mParticleTypeInfo[j].first.Serialize(b, f);
				b.WriteLong((long)this.mParticleTypeInfo[j].second);
			}
			b.WriteLong((long)this.mParticles.Count);
			for (int k = 0; k < this.mParticles.Count; k++)
			{
				this.mParticles[k].Serialize(b);
			}
			b.WriteLong((long)this.mFreeEmitterInfo.Count);
			for (int l = 0; l < this.mFreeEmitterInfo.Count; l++)
			{
				this.mFreeEmitterInfo[l].first.Serialize(b, f);
				b.WriteLong((long)this.mFreeEmitterInfo[l].second);
			}
			b.WriteLong((long)this.mEmitters.Count);
			for (int m = 0; m < this.mEmitters.Count; m++)
			{
				this.mEmitters[m].Serialize(b, f);
			}
			b.WriteLong((long)this.mLastPTFrameSetting.Count);
			foreach (KeyValuePair<int, ParticleSettings> keyValuePair in this.mLastPTFrameSetting)
			{
				b.WriteLong((long)keyValuePair.Key);
				keyValuePair.Value.Serialize(b);
			}
			b.WriteLong((long)this.mLastFEFrameSetting.Count);
			foreach (KeyValuePair<FreeEmitter, FreeEmitterSettings> keyValuePair2 in this.mLastFEFrameSetting)
			{
				b.WriteLong((long)keyValuePair2.Key.mSerialIndex);
				keyValuePair2.Value.Serialize(b);
			}
			this.mLastSettings.Serialize(b);
			this.mLastScale.Serialize(b);
			if (this.mAreaMask != null)
			{
				b.WriteLong((long)f(this.mAreaMask));
			}
			else
			{
				b.WriteLong(-1L);
			}
			b.WriteBoolean(this.mInvertAreaMask);
			b.WriteLong((long)this.mFrameOffset);
			b.WriteLong((long)this.mEmitterType);
			b.WriteLong((long)this.mLastEmitAtX);
			b.WriteLong((long)this.mLastEmitAtY);
			this.mWaypointManager.Serialize(b);
			b.WriteLong((long)this.mCullingRect.mX);
			b.WriteLong((long)this.mCullingRect.mY);
			b.WriteLong((long)this.mCullingRect.mWidth);
			b.WriteLong((long)this.mCullingRect.mHeight);
			b.WriteLong((long)this.mClipRect.mX);
			b.WriteLong((long)this.mClipRect.mY);
			b.WriteLong((long)this.mClipRect.mWidth);
			b.WriteLong((long)this.mClipRect.mHeight);
			b.WriteString(this.mName);
			b.WriteBoolean(this.mDrawNewestFirst);
			b.WriteBoolean(this.mWaitForParticles);
			b.WriteBoolean(this.mDeleteInvisParticles);
			b.WriteBoolean(this.mEmissionCoordsAreOffsets);
			b.WriteLong((long)this.mPreloadFrames);
			b.WriteLong((long)this.mEmitDir);
			b.WriteLong((long)this.mEmitAtXPoints);
			b.WriteLong((long)this.mEmitAtYPoints);
			b.WriteLong((long)this.mSerialIndex);
			b.WriteBoolean(this.mLinearEmitAtPoints);
			b.WriteLong((long)this.mTintColor.ToInt());
			if (this.mParentEmitter == null)
			{
				b.WriteLong(-1L);
			}
			else
			{
				b.WriteLong((long)this.mParentEmitter.mSerialIndex);
			}
			if (this.mSuperEmitter == null)
			{
				b.WriteLong(-1L);
				return;
			}
			b.WriteLong((long)this.mSuperEmitter.mSerialIndex);
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x0003BEA0 File Offset: 0x0003A0A0
		public void Deserialize(SexyBuffer b, Dictionary<int, Deflector> deflector_ptr_map, Dictionary<int, FreeEmitter> fe_ptr_map, GlobalMembers.GetImageByIdFunc f)
		{
			base.Deserialize(b, deflector_ptr_map);
			this.Clear();
			this.mHandle = (int)b.ReadLong();
			this.mDisableQuadRep = b.ReadBoolean();
			this.mUseAlternateCalcMethod = b.ReadBoolean();
			this.mNumSpawnAccumulator = b.ReadFloat();
			this.mStartFrame = (int)b.ReadLong();
			this.mParticlesMustHaveBeenVisible = b.ReadBoolean();
			this.mSettingsTimeLine.mCurrentSettings = null;
			this.mScaleTimeLine.mCurrentSettings = null;
			this.Init();
			int num = (int)b.ReadLong();
			for (int i = 0; i < num; i++)
			{
				LineEmitterPoint lineEmitterPoint = new LineEmitterPoint();
				lineEmitterPoint.Deserialize(b);
				this.mLineEmitterPoints.Add(lineEmitterPoint);
			}
			this.mScaleTimeLine.Deserialize(b, new GlobalMembers.KFDInstantiateFunc(EmitterScale.Instantiate));
			this.mSettingsTimeLine.Deserialize(b, new GlobalMembers.KFDInstantiateFunc(EmitterSettings.Instantiate));
			num = (int)b.ReadLong();
			Dictionary<int, ParticleType> dictionary = new Dictionary<int, ParticleType>();
			for (int j = 0; j < num; j++)
			{
				ParticleType particleType = new ParticleType();
				particleType.Deserialize(b, f);
				dictionary.Add(particleType.mSerialIndex, particleType);
				int s = (int)b.ReadLong();
				this.mParticleTypeInfo.Add(new ParticleTypeInfo(particleType, s));
			}
			num = (int)b.ReadLong();
			for (int k = 0; k < num; k++)
			{
				Particle particle = this.mSystem.AllocateParticle();
				particle.Deserialize(b, deflector_ptr_map, dictionary);
				this.mParticles.Add(particle);
			}
			Dictionary<int, FreeEmitter> dictionary2 = new Dictionary<int, FreeEmitter>();
			num = (int)b.ReadLong();
			for (int l = 0; l < num; l++)
			{
				FreeEmitter freeEmitter = new FreeEmitter();
				freeEmitter.Deserialize(b, f);
				int s2 = (int)b.ReadLong();
				this.mFreeEmitterInfo.Add(new FreeEmitterInfo(freeEmitter, s2));
				dictionary2.Add(freeEmitter.mSerialIndex, freeEmitter);
			}
			num = (int)b.ReadLong();
			for (int m = 0; m < num; m++)
			{
				Emitter emitter = new Emitter();
				emitter.Deserialize(b, deflector_ptr_map, dictionary2, f);
				this.mEmitters.Add(emitter);
			}
			num = (int)b.ReadLong();
			for (int n = 0; n < num; n++)
			{
				int num2 = (int)b.ReadLong();
				ParticleSettings particleSettings = new ParticleSettings();
				particleSettings.Deserialize(b);
				if (dictionary.ContainsKey(num2))
				{
					ParticleType particleType2 = dictionary[num2];
					this.mLastPTFrameSetting.Add(num2, particleSettings);
				}
			}
			num = (int)b.ReadLong();
			for (int num3 = 0; num3 < num; num3++)
			{
				int num4 = (int)b.ReadLong();
				FreeEmitterSettings freeEmitterSettings = new FreeEmitterSettings();
				freeEmitterSettings.Deserialize(b);
				this.mLastFEFrameSetting.Add(this.mFreeEmitterInfo[num4].first, freeEmitterSettings);
			}
			this.mLastSettings.Deserialize(b);
			this.mLastScale.Deserialize(b);
			int num5 = (int)b.ReadLong();
			this.mInvertAreaMask = b.ReadBoolean();
			if (num5 != -1)
			{
				this.mAreaMask = (MemoryImage)f(num5);
				this.SetAreaMask(this.mAreaMask, this.mInvertAreaMask);
			}
			this.mFrameOffset = (int)b.ReadLong();
			this.mEmitterType = (int)b.ReadLong();
			this.mLastEmitAtX = (int)b.ReadLong();
			this.mLastEmitAtY = (int)b.ReadLong();
			this.mWaypointManager.Deserialize(b);
			this.mCullingRect.mX = (int)b.ReadLong();
			this.mCullingRect.mY = (int)b.ReadLong();
			this.mCullingRect.mWidth = (int)b.ReadLong();
			this.mCullingRect.mHeight = (int)b.ReadLong();
			this.mClipRect.mX = (int)b.ReadLong();
			this.mClipRect.mY = (int)b.ReadLong();
			this.mClipRect.mWidth = (int)b.ReadLong();
			this.mClipRect.mHeight = (int)b.ReadLong();
			this.mName = b.ReadString();
			this.mDrawNewestFirst = b.ReadBoolean();
			this.mWaitForParticles = b.ReadBoolean();
			this.mDeleteInvisParticles = b.ReadBoolean();
			this.mEmissionCoordsAreOffsets = b.ReadBoolean();
			this.mPreloadFrames = (int)b.ReadLong();
			this.mEmitDir = (int)b.ReadLong();
			this.mEmitAtXPoints = (int)b.ReadLong();
			this.mEmitAtYPoints = (int)b.ReadLong();
			this.mSerialIndex = (int)b.ReadLong();
			this.mLinearEmitAtPoints = b.ReadBoolean();
			int theColor = (int)b.ReadLong();
			this.mTintColor = new SexyColor(theColor);
			int num6 = (int)b.ReadLong();
			if (num6 != -1 && fe_ptr_map.ContainsKey(num6))
			{
				this.mParentEmitter = fe_ptr_map[num6];
			}
			this.mSuperEmitterIndex = (int)b.ReadLong();
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x0003C348 File Offset: 0x0003A548
		protected void UpdateLineEmitter(int frame)
		{
			for (int i = 0; i < this.mLineEmitterPoints.size<LineEmitterPoint>(); i++)
			{
				LineEmitterPoint lineEmitterPoint = this.mLineEmitterPoints[i];
				PointKeyFrame pointKeyFrame = null;
				PointKeyFrame pointKeyFrame2 = null;
				for (int j = 0; j < lineEmitterPoint.mKeyFramePoints.size<PointKeyFrame>(); j++)
				{
					if (lineEmitterPoint.mKeyFramePoints[j].first > frame)
					{
						pointKeyFrame2 = lineEmitterPoint.mKeyFramePoints[j];
						break;
					}
					pointKeyFrame = lineEmitterPoint.mKeyFramePoints[j];
				}
				float num;
				if (pointKeyFrame2 == null)
				{
					num = 0f;
					pointKeyFrame2 = pointKeyFrame;
				}
				else
				{
					num = (float)(frame - pointKeyFrame.first) / (float)(pointKeyFrame2.first - pointKeyFrame.first);
				}
				lineEmitterPoint.mCurX = (float)pointKeyFrame.second.mX + (float)(pointKeyFrame2.second.mX - pointKeyFrame.second.mX) * num;
				lineEmitterPoint.mCurY = (float)pointKeyFrame.second.mY + (float)(pointKeyFrame2.second.mY - pointKeyFrame.second.mY) * num;
			}
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x0003C458 File Offset: 0x0003A658
		protected int GetEmissionCoord(ref float x, ref float y, ref float angle)
		{
			int num = -1;
			if (this.mEmitterType == 0)
			{
				x = this.mX;
				y = this.mY;
				num = -1;
			}
			else if (this.mEmitterType == 1)
			{
				if (this.mEmitAtXPoints == 0 || this.mLineEmitterPoints.size<LineEmitterPoint>() == 1)
				{
					if (this.mLineEmitterPoints.size<LineEmitterPoint>() == 1)
					{
						x = this.mLineEmitterPoints[0].mCurX;
						y = this.mLineEmitterPoints[0].mCurY;
						num = 0;
					}
					else
					{
						int num2 = Common.Rand() % (this.mLineEmitterPoints.size<LineEmitterPoint>() - 1);
						this.GetXYFromLineIdx(num2, Common.FloatRange(0f, 1f), ref x, ref y);
						num = num2;
					}
				}
				else
				{
					int num3 = 0;
					List<int> list = new List<int>();
					for (int i = 1; i < this.mLineEmitterPoints.size<LineEmitterPoint>(); i++)
					{
						int num4 = (int)Common.Distance(this.mLineEmitterPoints[i].mCurX, this.mLineEmitterPoints[i].mCurY, this.mLineEmitterPoints[i - 1].mCurX, this.mLineEmitterPoints[i - 1].mCurY, false);
						list.Add(num4);
						num3 += num4;
					}
					float num5 = ((this.mEmitAtXPoints == 1) ? 0f : ((float)num3 / (float)(this.mEmitAtXPoints - 1)));
					int num6 = (this.mLinearEmitAtPoints ? (this.mLastEmitAtX++ % this.mEmitAtXPoints) : (Common.Rand() % this.mEmitAtXPoints));
					int num7 = (int)Math.Ceiling((double)((float)num6 * num5));
					int num8 = this.mLineEmitterPoints.size<LineEmitterPoint>() - 2;
					for (int j = 1; j < this.mLineEmitterPoints.size<LineEmitterPoint>(); j++)
					{
						if (num7 <= list[j - 1])
						{
							num8 = j - 1;
							break;
						}
						num7 -= list[j - 1];
					}
					float pct = (float)num7 / (float)list[num8];
					this.GetXYFromLineIdx(num8, pct, ref x, ref y);
					num = num8;
				}
			}
			else if (this.mEmitterType == 2)
			{
				if (this.mEmitAtXPoints == 0)
				{
					angle = -Common.DegreesToRadians((float)(Common.Rand() % 360));
				}
				else
				{
					int num9 = (this.mLinearEmitAtPoints ? (this.mLastEmitAtX++ % this.mEmitAtXPoints) : (Common.Rand() % this.mEmitAtXPoints));
					float num10 = 360f / (float)this.mEmitAtXPoints;
					angle = -Common.DegreesToRadians((float)num9 * num10);
				}
				float num11 = (float)Math.Sin((double)(-(double)this.mLastSettings.mAngle));
				float num12 = (float)Math.Cos((double)(-(double)this.mLastSettings.mAngle));
				float num13 = (float)Math.Sin((double)angle);
				float num14 = (float)Math.Cos((double)angle);
				x = this.mLastSettings.mXRadius * num14 * num12 - this.mLastSettings.mYRadius * num13 * num11;
				y = this.mLastSettings.mXRadius * num14 * num11 + this.mLastSettings.mYRadius * num13 * num12;
				angle *= -1f;
				num = 0;
			}
			else if (this.mEmitterType == 3)
			{
				if (this.mMaskPoints != null)
				{
					int num15 = Common.Rand() % this.mNumMaskPoints;
					x = (float)this.mMaskPoints[num15].mX + this.mX - (float)this.mDebugMaskImage.mWidth / 2f;
					y = (float)this.mMaskPoints[num15].mY + this.mY - (float)this.mDebugMaskImage.mHeight / 2f;
					Rect rect = new Rect((int)(this.mX - this.mLastSettings.mXRadius / 2f), (int)(this.mY - this.mLastSettings.mYRadius / 2f), (int)this.mLastSettings.mXRadius, (int)this.mLastSettings.mYRadius);
					if (!rect.Contains((int)x, (int)y))
					{
						num = -1;
					}
				}
				else if (this.mEmitAtXPoints == 0 || this.mEmitAtYPoints == 0)
				{
					x = this.mX - this.mLastSettings.mXRadius / 2f + (float)(Common.Rand() % (int)this.mLastSettings.mXRadius);
					y = this.mY - this.mLastSettings.mYRadius / 2f + (float)(Common.Rand() % (int)this.mLastSettings.mYRadius);
					num = 0;
				}
				else
				{
					int num16 = (this.mLinearEmitAtPoints ? (this.mLastEmitAtX++ % this.mEmitAtXPoints) : (Common.Rand() % this.mEmitAtXPoints));
					int num17 = (this.mLinearEmitAtPoints ? (this.mLastEmitAtY++ % this.mEmitAtYPoints) : (Common.Rand() % this.mEmitAtYPoints));
					if (this.mEmitAtXPoints == 1)
					{
						x = this.mX;
					}
					else
					{
						float num18 = this.mLastSettings.mXRadius / (float)(this.mEmitAtXPoints - 1);
						x = this.mX - this.mLastSettings.mXRadius / 2f + (float)num16 * num18;
					}
					if (this.mEmitAtYPoints == 1)
					{
						y = this.mY;
					}
					else
					{
						float num19 = this.mLastSettings.mYRadius / (float)(this.mEmitAtYPoints - 1);
						y = this.mY - this.mLastSettings.mYRadius / 2f + (float)num17 * num19;
					}
					num = 0;
				}
				if (num != -1)
				{
					Common.RotatePoint(this.mLastSettings.mAngle, ref x, ref y, this.mX, this.mY);
				}
			}
			if (num != -1 && this.mEmitterType != 3 && (this.mParentEmitter != null || this.mEmissionCoordsAreOffsets))
			{
				x += this.mX;
				y += this.mY;
			}
			return num;
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x0003CA28 File Offset: 0x0003AC28
		protected void GetXYFromLineIdx(int idx, float pct, ref float x, ref float y)
		{
			int num = (int)(this.mLineEmitterPoints[idx + 1].mCurX - this.mLineEmitterPoints[idx].mCurX);
			int num2 = (int)(this.mLineEmitterPoints[idx + 1].mCurY - this.mLineEmitterPoints[idx].mCurY);
			x = this.mLineEmitterPoints[idx].mCurX + (float)num * pct;
			y = this.mLineEmitterPoints[idx].mCurY + (float)num2 * pct;
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0003CAB4 File Offset: 0x0003ACB4
		protected void Clear()
		{
			if (this.mWaypointManager != null)
			{
				this.mWaypointManager.Dispose();
			}
			for (int i = 0; i < this.mParticles.size<Particle>(); i++)
			{
				this.mSystem.DeleteParticle(this.mParticles[i]);
			}
			for (int j = 0; j < this.mEmitters.size<Emitter>(); j++)
			{
				if (this.mEmitters[j] != null)
				{
					this.mEmitters[j].Dispose();
				}
			}
			this.mParticles.Clear();
			this.mEmitters.Clear();
			this.mParticleTypeInfo.Clear();
			this.mFreeEmitterInfo.Clear();
			this.mLastFEFrameSetting.Clear();
			this.mLastFEFrameSetting.Clear();
			this.mWaypointManager = null;
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x0003CB80 File Offset: 0x0003AD80
		protected void Init()
		{
			this.mSettingsTimeLine.mCurrentSettings = new EmitterSettings();
			this.mScaleTimeLine.mCurrentSettings = new EmitterScale();
			this.mLastSettings = (EmitterSettings)this.mSettingsTimeLine.mCurrentSettings;
			this.mLastScale = (EmitterScale)this.mScaleTimeLine.mCurrentSettings;
			this.mWaypointManager = new WaypointManager();
			this.mParticles.Capacity = 500;
			this.mEmitters.Capacity = 100;
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x0003CC04 File Offset: 0x0003AE04
		protected void SpawnParticles(int frame)
		{
			for (int i = 0; i < this.mParticleTypeInfo.size<ParticleTypeInfo>(); i++)
			{
				ParticleTypeInfo particleTypeInfo = this.mParticleTypeInfo[i];
				ParticleType first = particleTypeInfo.first;
				if (!first.mSingle || first.mNumCreated <= 0)
				{
					int num = 0;
					float num2 = 0f;
					ParticleSettings particleSettings = null;
					ParticleVariance particleVariance = null;
					first.GetCreationParameters(frame, out num, out num2, out particleSettings, out particleVariance);
					this.mLastPTFrameSetting[first.mSerialIndex] = particleSettings;
					if ((float)(frame - particleTypeInfo.second) >= num2 || this.mUseAlternateCalcMethod)
					{
						particleTypeInfo.second = frame;
						int num3 = 0;
						if (!this.mUseAlternateCalcMethod)
						{
							num3 = (int)(Math.Ceiling((double)(1f / num2)) * (double)this.mLastScale.mNumberScale * (double)this.mCurrentLifetimeSettings.mNumberMult);
							if (!GlobalMembers.gSexyAppBase.Is3DAccelerated())
							{
								num3 = (int)((float)num3 * this.mSystem.mParticleScale2D);
							}
							if ((num3 <= 0 && this.mLastScale.mNumberScale > 0f && this.mCurrentLifetimeSettings.mNumberMult > 0f) || first.mSingle)
							{
								num3 = 1;
							}
						}
						else
						{
							float num4 = ((float)particleSettings.mNumber + Common.SAFE_RAND((float)particleVariance.mNumberVar)) * this.mLastScale.mNumberScale * this.mCurrentLifetimeSettings.mNumberMult / 6.6666665f;
							this.mNumSpawnAccumulator += num4 * (GlobalMembers.gSexyAppBase.Is3DAccelerated() ? 1f : this.mSystem.mParticleScale2D);
							if (this.mNumSpawnAccumulator >= 1f)
							{
								num3 = (int)this.mNumSpawnAccumulator;
								this.mNumSpawnAccumulator -= (float)num3;
							}
						}
						if (num != 0)
						{
							for (int j = 0; j < num3; j++)
							{
								first.mNumCreated++;
								num = first.GetRandomizedLife();
								float spawn_angle = 0f;
								float x = 0f;
								float y = 0f;
								if (this.GetLaunchSettings(ref spawn_angle, ref x, ref y))
								{
									float velocity = this.mLastScale.mVelocityScale * this.mLastScale.mZoom * ((float)particleSettings.mVelocity + Common.SAFE_RAND((float)particleVariance.mVelocityVar)) / 100f;
									Particle particle = this.mSystem.AllocateParticle();
									if (!first.mSingle)
									{
										particle.Reset(spawn_angle, velocity);
									}
									else
									{
										particle.Reset(0f, 0f);
									}
									particle.mColorKeyManager.CopyFrom(first.mColorKeyManager);
									particle.mAlphaKeyManager.CopyFrom(first.mAlphaKeyManager);
									particle.mParentType = first;
									particle.mLockSizeAspect = first.mLockSizeAspect;
									particle.mImage = first.mImage;
									particle.mImageRate = first.mImageRate;
									if (first.mRandomStartCel && first.mImage != null)
									{
										particle.mImageCel = ((particle.mImage.mNumCols > particle.mImage.mNumRows) ? (Common.Rand() % particle.mImage.mNumCols) : (Common.Rand() % particle.mImage.mNumRows));
									}
									particle.mAdditive = first.mAdditive;
									particle.mAdditiveWithNormal = first.mAdditiveWithNormal;
									if (num != -1)
									{
										particle.mLife = (first.mSingle ? (-1) : ((int)(this.mLastScale.mLifeScale * (float)num)));
									}
									else
									{
										particle.mLife = -1;
									}
									particle.mRefXOff = first.mRefXOff;
									particle.mRefYOff = first.mRefYOff;
									particle.mMotionAngleOffset = first.mMotionAngleOffset;
									particle.mAlignAngleToMotion = first.mAlignAngleToMotion;
									if (particle.mLife != -1)
									{
										particle.mColorKeyManager.SetLife(particle.mLife);
										particle.mAlphaKeyManager.SetLife(particle.mLife);
									}
									particle.mAngle = first.GetSpawnAngle();
									if (first.mNumSameColorKeyInRow > 0)
									{
										particle.mColorKeyManager.SetFixedColor(first.GetNextKeyColor());
									}
									particle.mMotionRand = (first.mSingle ? 0f : (this.mLastScale.mMotionRandScale * (particleSettings.mMotionRand + Common.FloatRange(0f, particleVariance.mMotionRandVar)) / 100f));
									particle.SetXY(x, y);
									particle.mParentName = first.mName;
									particle.mWeight = (first.mSingle ? 0f : (this.mLastScale.mWeightScale * (particleSettings.mWeight - Common.SAFE_RAND((float)(particleVariance.mWeightVar / 2)) + Common.SAFE_RAND((float)(particleVariance.mWeightVar / 2))) / ModVal.M(2000f)));
									particle.mSpin = this.mLastScale.mSpinScale * (particleSettings.mSpin - Common.FloatRange(0f, particleVariance.mSpinVar / 2f) + Common.FloatRange(0f, particleVariance.mSpinVar / 2f)) / 10f;
									particle.mBounce = this.mLastScale.mBounceScale * ((float)particleSettings.mBounce - Common.SAFE_RAND((float)(particleVariance.mBounceVar / 2)) + Common.SAFE_RAND((float)(particleVariance.mBounceVar / 2)));
									if (particle.mBounce < 0f)
									{
										particle.mBounce = 0f;
									}
									particle.mFlipX = first.mFlipX;
									particle.mFlipY = first.mFlipY;
									if (particle.mImage != null)
									{
										int num5 = (int)((float)particleSettings.mXSize * this.mSystem.mScale) + (int)Common.SAFE_RAND((float)particleVariance.mSizeXVar * this.mSystem.mScale);
										particle.mCurXSize = this.mLastScale.mSizeXScale * this.mLastScale.mZoom * ((float)num5 / (float)particle.mImage.GetCelWidth());
										if (!first.mLockSizeAspect)
										{
											num5 = (int)((float)particleSettings.mYSize * this.mSystem.mScale) + (int)Common.SAFE_RAND((float)particleVariance.mSizeYVar * this.mSystem.mScale);
										}
										particle.mCurYSize = this.mLastScale.mSizeYScale * this.mLastScale.mZoom * ((float)num5 / (float)particle.mImage.GetCelHeight());
									}
									for (int k = 0; k < first.mLifePctSettings.size<LifetimeSettingPct>(); k++)
									{
										particle.AddLifetimeKeyFrame(first.mLifePctSettings[k].first, new LifetimeSettings(first.mLifePctSettings[k].second));
									}
									this.mParticles.Add(particle);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x0003D2A0 File Offset: 0x0003B4A0
		protected void SpawnEmitters(int frame)
		{
			for (int i = 0; i < this.mFreeEmitterInfo.size<FreeEmitterInfo>(); i++)
			{
				FreeEmitterInfo freeEmitterInfo = this.mFreeEmitterInfo[i];
				FreeEmitter first = freeEmitterInfo.first;
				int num = 0;
				float num2 = 0f;
				FreeEmitterSettings freeEmitterSettings = null;
				FreeEmitterVariance freeEmitterVariance = null;
				first.GetCreationParams(frame, out num, out num2, out freeEmitterSettings, out freeEmitterVariance);
				float num3 = (float)freeEmitterSettings.mZoom / 100f;
				this.mLastFEFrameSetting[first] = freeEmitterSettings;
				if ((float)(frame - freeEmitterInfo.second) >= num2)
				{
					freeEmitterInfo.second = frame;
					int num4 = (int)(Math.Ceiling((double)(1f / num2)) * (double)this.mLastScale.mNumberScale * (double)this.mCurrentLifetimeSettings.mNumberMult);
					if (num4 <= 0 && this.mLastScale.mNumberScale > 0f && this.mCurrentLifetimeSettings.mNumberMult > 0f)
					{
						num4 = 1;
					}
					for (int j = 0; j < num4; j++)
					{
						num = first.GetRandomizedLife();
						float angle = 0f;
						float mX = 0f;
						float mY = 0f;
						if (this.GetLaunchSettings(ref angle, ref mX, ref mY))
						{
							float velocity = this.mLastScale.mVelocityScale * this.mLastScale.mZoom * num3 * ((float)freeEmitterSettings.mVelocity + Common.SAFE_RAND((float)freeEmitterVariance.mVelocityVar)) / 100f;
							Emitter emitter = new Emitter(first.mEmitter);
							emitter.mSystem = this.mSystem;
							this.mEmitters.Add(emitter);
							emitter.Launch(angle, velocity);
							emitter.mParentEmitter = first;
							emitter.mSuperEmitter = this;
							emitter.mLife = ((num == -1) ? (-1) : ((int)(this.mLastScale.mLifeScale * (float)num)));
							emitter.mX = mX;
							emitter.mY = mY;
							emitter.mMotionRand = this.mLastScale.mMotionRandScale * (float)(freeEmitterSettings.mMotionRand + Common.IntRange(0, freeEmitterVariance.mMotionRandVar)) / 100f;
							emitter.mWeight = this.mLastScale.mWeightScale * ((float)freeEmitterSettings.mWeight - Common.SAFE_RAND((float)(freeEmitterVariance.mWeightVar / 2)) + Common.SAFE_RAND((float)(freeEmitterVariance.mWeightVar / 2))) / ModVal.M(2000f);
							emitter.mSpin = this.mLastScale.mSpinScale * (freeEmitterSettings.mSpin - Common.FloatRange(0f, freeEmitterVariance.mSpinVar / 2f) + Common.FloatRange(0f, freeEmitterVariance.mSpinVar / 2f)) / 10f;
							emitter.mBounce = this.mLastScale.mBounceScale * ((float)freeEmitterSettings.mBounce - Common.SAFE_RAND((float)(freeEmitterVariance.mBounceVar / 2)) + Common.SAFE_RAND((float)freeEmitterVariance.mBounceVar));
							if (emitter.mBounce < 0f)
							{
								emitter.mBounce = 0f;
							}
							for (int k = 0; k < first.mLifePctSettings.size<LifetimeSettingPct>(); k++)
							{
								emitter.AddLifetimeKeyFrame(first.mLifePctSettings[k].second.mPct, new LifetimeSettings(first.mLifePctSettings[k].second));
							}
							for (int l = 0; l < emitter.mScaleTimeLine.mKeyFrames.size<KeyFrame>(); l++)
							{
								EmitterScale emitterScale = emitter.mScaleTimeLine.mKeyFrames[i].second as EmitterScale;
								emitterScale.mSizeXScale -= Common.SAFE_RAND((float)(freeEmitterVariance.mSizeXVar / 2)) / 100f;
								emitterScale.mSizeXScale += Common.SAFE_RAND((float)(freeEmitterVariance.mSizeXVar / 2)) / 100f;
								emitterScale.mSizeYScale -= Common.SAFE_RAND((float)((first.mAspectLocked ? freeEmitterVariance.mSizeXVar : freeEmitterVariance.mSizeYVar) / 2)) / 100f;
								emitterScale.mSizeYScale += Common.SAFE_RAND((float)((first.mAspectLocked ? freeEmitterVariance.mSizeXVar : freeEmitterVariance.mSizeYVar) / 2)) / 100f;
								emitterScale.mSizeXScale *= num3;
								emitterScale.mSizeYScale *= num3;
								if (emitterScale.mSizeXScale < 0f)
								{
									emitterScale.mSizeXScale = 0f;
								}
								if (emitterScale.mSizeYScale < 0f)
								{
									emitterScale.mSizeYScale = 0f;
								}
								emitterScale.mVelocityScale *= num3;
								emitterScale.mZoom -= Common.SAFE_RAND((float)(freeEmitterVariance.mZoomVar / 2)) / 100f;
								emitterScale.mZoom += Common.SAFE_RAND((float)(freeEmitterVariance.mZoomVar / 2)) / 100f;
								emitterScale.mZoom *= num3;
								if (emitterScale.mZoom < 0f)
								{
									emitterScale.mZoom = 0f;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x0003D7AC File Offset: 0x0003B9AC
		protected bool GetLaunchSettings(ref float angle, ref float x, ref float y)
		{
			float num = Common.FloatRange(0f, this.mLastSettings.mEmissionRange / 2f);
			float num2 = Common.FloatRange(0f, this.mLastSettings.mEmissionRange / 2f);
			angle = this.mLastSettings.mEmissionAngle - num + num2;
			x = this.mX;
			y = this.mY;
			float num3 = 0f;
			int emissionCoord = this.GetEmissionCoord(ref x, ref y, ref num3);
			if (this.mEmitterType == 1 && emissionCoord >= 0 && this.mLineEmitterPoints.size<LineEmitterPoint>() > 1)
			{
				float mCurX = this.mLineEmitterPoints[emissionCoord].mCurX;
				float mCurY = this.mLineEmitterPoints[emissionCoord].mCurY;
				float mCurX2 = this.mLineEmitterPoints[emissionCoord + 1].mCurX;
				float mCurY2 = this.mLineEmitterPoints[emissionCoord + 1].mCurY;
				if (this.mEmitDir == 0 || (this.mEmitDir == 2 && Common.Rand() % 100 < 50))
				{
					angle += Common.AngleBetweenPoints(mCurX, mCurY, mCurX2, mCurY2);
				}
				else
				{
					angle = Common.AngleBetweenPoints(mCurX, mCurY, mCurX2, mCurY2) - angle;
				}
			}
			else if (this.mEmitterType == 2)
			{
				if (this.mEmitDir == 1 || (this.mEmitDir == 2 && Common.Rand() % 100 < 50))
				{
					num3 += Common.JL_PI / 2f;
				}
				else
				{
					num3 -= Common.JL_PI / 2f;
				}
				angle = num3 - angle;
			}
			else if (this.mEmitterType == 3 && emissionCoord == -1)
			{
				return false;
			}
			angle += this.mLastSettings.mAngle;
			return true;
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x0003D94C File Offset: 0x0003BB4C
		public Emitter()
		{
			this.mPreloadFrames = 0;
			this.mDrawNewestFirst = false;
			this.mEmitterType = 0;
			this.mEmitAtXPoints = 0;
			this.mEmitAtYPoints = 0;
			this.mEmitDir = 2;
			this.mNumMaskPoints = 0;
			this.mMaskPoints = null;
			this.mDebugMaskImage = null;
			this.mLastSettings = null;
			this.mParentEmitter = null;
			this.mSuperEmitter = null;
			this.mFrameOffset = 0;
			this.mWaitForParticles = false;
			this.mSystem = null;
			this.mPoolIndex = -1;
			this.mEmissionCoordsAreOffsets = false;
			this.mLastEmitAtX = 0;
			this.mLastEmitAtY = 0;
			this.mDeleteInvisParticles = false;
			this.mParticlesMustHaveBeenVisible = false;
			this.mSerialIndex = -1;
			this.mAreaMask = null;
			this.mSuperEmitterIndex = -1;
			this.mHandle = -1;
			this.mDisableQuadRep = true;
			this.mUseAlternateCalcMethod = false;
			this.mNumSpawnAccumulator = 0f;
			this.mStartFrame = 0;
			this.Init();
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x0003DAC6 File Offset: 0x0003BCC6
		public Emitter(Emitter rhs)
			: this()
		{
			this.CopyFrom(rhs);
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x0003DAD5 File Offset: 0x0003BCD5
		public override void Dispose()
		{
			this.Clear();
			base.Dispose();
			this.mMaskPoints = null;
			this.mDebugMaskImage = null;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0003DAF4 File Offset: 0x0003BCF4
		public void CopyFrom(Emitter rhs)
		{
			base.CopyFrom(rhs);
			this.mTintColor = rhs.mTintColor;
			this.mEmitAtXPoints = rhs.mEmitAtXPoints;
			this.mEmitAtYPoints = rhs.mEmitAtYPoints;
			this.mEmitDir = rhs.mEmitDir;
			this.mPreloadFrames = rhs.mPreloadFrames;
			this.mDrawNewestFirst = rhs.mDrawNewestFirst;
			this.mClipRect = rhs.mClipRect;
			this.mCullingRect = rhs.mCullingRect;
			this.mNumMaskPoints = rhs.mNumMaskPoints;
			this.mEmitterType = rhs.mEmitterType;
			this.mParentEmitter = rhs.mParentEmitter;
			this.mWaitForParticles = rhs.mWaitForParticles;
			this.mSystem = rhs.mSystem;
			this.mDisableQuadRep = rhs.mDisableQuadRep;
			this.mDeleteInvisParticles = rhs.mDeleteInvisParticles;
			this.mUseAlternateCalcMethod = rhs.mUseAlternateCalcMethod;
			this.mNumSpawnAccumulator = rhs.mNumSpawnAccumulator;
			this.mStartFrame = rhs.mStartFrame;
			this.mParticlesMustHaveBeenVisible = rhs.mParticlesMustHaveBeenVisible;
			this.mMaskPoints = null;
			this.mDebugMaskImage = null;
			this.mAreaMask = rhs.mAreaMask;
			this.mInvertAreaMask = rhs.mInvertAreaMask;
			this.mNumMaskPoints = rhs.mNumMaskPoints;
			if (this.mNumMaskPoints > 0)
			{
				this.mMaskPoints = new SexyPoint[this.mNumMaskPoints];
				for (int i = 0; i < this.mNumMaskPoints; i++)
				{
					this.mMaskPoints[i] = new SexyPoint(rhs.mMaskPoints[i]);
				}
				this.mDebugMaskImage = new MemoryImage(rhs.mDebugMaskImage);
			}
			this.mScaleTimeLine = rhs.mScaleTimeLine;
			this.mSettingsTimeLine = rhs.mSettingsTimeLine;
			this.mLastScale = (EmitterScale)this.mScaleTimeLine.mCurrentSettings;
			this.mLastSettings = (EmitterSettings)this.mSettingsTimeLine.mCurrentSettings;
			this.Clear();
			for (int j = 0; j < rhs.mParticleTypeInfo.size<ParticleTypeInfo>(); j++)
			{
				this.AddParticleType(new ParticleType(rhs.mParticleTypeInfo[j].first));
			}
			for (int k = 0; k < rhs.mFreeEmitterInfo.size<FreeEmitterInfo>(); k++)
			{
				this.AddFreeEmitter(new FreeEmitter(rhs.mFreeEmitterInfo[k].first));
			}
			this.mLineEmitterPoints = rhs.mLineEmitterPoints;
			this.mWaypointManager = new WaypointManager(rhs.mWaypointManager);
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x0003DD3C File Offset: 0x0003BF3C
		public void ResetForReuse()
		{
			this.mPoolIndex = -1;
			this.mLastEmitAtX = 0;
			this.mLastEmitAtY = 0;
			this.mNumSpawnAccumulator = 0f;
			for (int i = 0; i < this.mParticles.size<Particle>(); i++)
			{
				this.mSystem.DeleteParticle(this.mParticles[i]);
			}
			this.mParticles.Clear();
			for (int j = 0; j < this.mEmitters.Count; j++)
			{
				this.mEmitters[j].ResetForReuse();
			}
			for (int k = 0; k < this.mParticleTypeInfo.Count; k++)
			{
				this.mParticleTypeInfo[k].first.ResetForReuse();
				this.mParticleTypeInfo[k].second = 0;
			}
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x0003DE06 File Offset: 0x0003C006
		public EmitterScale AddScaleKeyFrame(int frame, EmitterScale scale, int second_frame_time, bool make_new)
		{
			this.mScaleTimeLine.AddKeyFrame(frame, scale);
			if (second_frame_time != -1)
			{
				this.mScaleTimeLine.AddKeyFrame(second_frame_time, new EmitterScale(scale));
				if (make_new)
				{
					return new EmitterScale(scale);
				}
			}
			return null;
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x0003DE37 File Offset: 0x0003C037
		public EmitterScale AddScaleKeyFrame(int frame, EmitterScale scale, int second_frame_time)
		{
			return this.AddScaleKeyFrame(frame, scale, second_frame_time, false);
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x0003DE43 File Offset: 0x0003C043
		public EmitterScale AddScaleKeyFrame(int frame, EmitterScale scale)
		{
			return this.AddScaleKeyFrame(frame, scale, -1, false);
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x0003DE4F File Offset: 0x0003C04F
		public EmitterSettings AddSettingsKeyFrame(int frame, EmitterSettings settings, int second_frame_time, bool make_new)
		{
			this.mSettingsTimeLine.AddKeyFrame(frame, settings);
			if (second_frame_time != -1)
			{
				this.mSettingsTimeLine.AddKeyFrame(second_frame_time, new EmitterSettings(settings));
				if (make_new)
				{
					return new EmitterSettings(settings);
				}
			}
			return null;
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x0003DE80 File Offset: 0x0003C080
		public EmitterSettings AddSettingsKeyFrame(int frame, EmitterSettings settings, int second_frame_time)
		{
			return this.AddSettingsKeyFrame(frame, settings, second_frame_time, false);
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x0003DE8C File Offset: 0x0003C08C
		public EmitterSettings AddSettingsKeyFrame(int frame, EmitterSettings settings)
		{
			return this.AddSettingsKeyFrame(frame, settings, -1, false);
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x0003DE98 File Offset: 0x0003C098
		public int AddParticleType(ParticleType pt)
		{
			if (pt.mImage != null)
			{
				if (this.mDisableQuadRep)
				{
					((MemoryImage)pt.mImage).AddImageFlags(128U);
				}
				else
				{
					((MemoryImage)pt.mImage).RemoveImageFlags(128U);
				}
			}
			if (pt.mSingle)
			{
				pt.mEmitterAttachPct = 1f;
			}
			if (pt.GetSettingsTimeLineSize() == 0)
			{
				pt.AddSettingsKeyFrame(0, new ParticleSettings());
			}
			if (pt.GetVarTimeLineSize() == 0)
			{
				pt.AddVarianceKeyFrame(0, new ParticleVariance());
			}
			if (pt.mColorKeyManager.GetColorMode() == 0 && pt.mColorKeyManager.GetNumKeys() > 0)
			{
				pt.mColorKeyManager.SetColorMode(1);
			}
			if (pt.mColorKeyManager.GetColorMode() == 1 && !pt.mColorKeyManager.HasMaxIndex())
			{
				pt.mColorKeyManager.AddColorKey(1f, pt.mColorKeyManager.GetColorByIndex(pt.mColorKeyManager.GetNumKeys() - 1));
			}
			if (pt.mAlphaKeyManager.GetColorMode() == 0 && pt.mAlphaKeyManager.GetNumKeys() > 0)
			{
				pt.mAlphaKeyManager.SetColorMode(1);
			}
			if (pt.mAlphaKeyManager.GetColorMode() == 1 && !pt.mAlphaKeyManager.HasMaxIndex())
			{
				pt.mAlphaKeyManager.AddColorKey(1f, pt.mAlphaKeyManager.GetColorByIndex(pt.mAlphaKeyManager.GetNumKeys() - 1));
			}
			this.mParticleTypeInfo.Add(new ParticleTypeInfo(pt, 0));
			pt.mSerialIndex = this.mParticleTypeInfo.size<ParticleTypeInfo>() - 1;
			this.mLastPTFrameSetting[pt.mSerialIndex] = new ParticleSettings();
			return this.mParticleTypeInfo.size<ParticleTypeInfo>() - 1;
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x0003E03A File Offset: 0x0003C23A
		public void AddFreeEmitter(FreeEmitter f)
		{
			this.mFreeEmitterInfo.Add(new FreeEmitterInfo(f, 0));
			this.mLastFEFrameSetting[f] = new FreeEmitterSettings();
			f.mSerialIndex = this.mFreeEmitterInfo.size<FreeEmitterInfo>() - 1;
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x0003E074 File Offset: 0x0003C274
		public void Update(int frame, bool allow_creation)
		{
			if (this.mLife > 0 && this.mUpdateCount >= this.mLife)
			{
				allow_creation = false;
			}
			frame -= this.mFrameOffset;
			float mX = this.mX;
			float mY = this.mY;
			base.Update();
			float mX2 = this.mX;
			float mY2 = this.mY;
			if (this.Dead())
			{
				return;
			}
			if (this.mWaypointManager.GetNumPoints() > 0)
			{
				this.mWaypointManager.Update(frame);
				this.SetPos(this.mWaypointManager.GetLastPoint().X, this.mWaypointManager.GetLastPoint().Y);
			}
			else
			{
				this.mX = mX;
				this.mY = mY;
				this.Move(mX2 - mX, mY2 - mY);
			}
			this.mScaleTimeLine.Update(frame);
			this.mSettingsTimeLine.Update(frame);
			bool flag = this.mSettingsTimeLine.mKeyFrames.size<KeyFrame>() == 0 || frame >= this.mSettingsTimeLine.mKeyFrames.back<KeyFrame>().first;
			this.mLastScale.mSizeXScale *= this.mCurrentLifetimeSettings.mSizeXMult;
			this.mLastScale.mSizeYScale *= this.mCurrentLifetimeSettings.mSizeYMult;
			this.mLastScale.mZoom *= this.mCurrentLifetimeSettings.mZoomMult;
			if (!this.mLastSettings.mActive)
			{
				for (int i = 0; i < this.mParticles.size<Particle>(); i++)
				{
					this.mSystem.DeleteParticle(this.mParticles[i]);
				}
				this.mParticles.Clear();
			}
			if (this.mEmitterType == 1)
			{
				this.UpdateLineEmitter(frame);
			}
			if (allow_creation && this.mLastSettings.mActive)
			{
				this.SpawnEmitters(frame);
				this.SpawnParticles(frame);
			}
			for (int j = 0; j < this.mParticles.size<Particle>(); j++)
			{
				this.mParticles[j].Update();
				bool flag2 = flag && !this.mParticles[j].mLastFrameWasVisible;
				if (this.mParticles[j].Dead() || (this.mDeleteInvisParticles && (!this.mParticlesMustHaveBeenVisible || this.mParticles[j].mHasBeenVisible) && flag2) || (this.mCullingRect != Rect.ZERO_RECT && !this.mCullingRect.Intersects(this.mParticles[j].GetRect())))
				{
					this.mSystem.DeleteParticle(this.mParticles[j]);
					this.mParticles.RemoveAt(j);
					j--;
				}
			}
			for (int k = 0; k < this.mEmitters.size<Emitter>(); k++)
			{
				this.mEmitters[k].Update(frame, allow_creation);
				if (this.mEmitters[k].Dead())
				{
					this.mEmitters[k].Dispose();
					this.mEmitters.RemoveAt(k);
					k--;
				}
			}
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x0003E394 File Offset: 0x0003C594
		public void Draw(Graphics g, float vis_mult, float tint_mult)
		{
			if (this.mClipRect != Rect.ZERO_RECT)
			{
				g.PushState();
				g.ClipRect(this.mClipRect);
			}
			int num = (this.mDrawNewestFirst ? (this.mParticles.size<Particle>() - 1) : 0);
			int num2 = num;
			while (this.mDrawNewestFirst ? (num2 >= 0) : (num2 < this.mParticles.size<Particle>()))
			{
				Particle particle = this.mParticles[num2];
				if (!particle.Dead() && (!particle.mAdditive || (particle.mAdditive && particle.mAdditiveWithNormal)))
				{
					float alpha_pct = this.mSystem.mAlphaPct * this.mLastPTFrameSetting[particle.mParentType.mSerialIndex].mGlobalVisibility * this.mLastSettings.mVisibility * vis_mult;
					particle.Draw(g, alpha_pct, this.mTintColor, this.mLastSettings.mTintStrength * tint_mult, this.mSystem.mScale);
				}
				num2 += (this.mDrawNewestFirst ? (-1) : 1);
			}
			int num3 = num;
			while (this.mDrawNewestFirst ? (num3 >= 0) : (num3 < this.mParticles.size<Particle>()))
			{
				Particle particle2 = this.mParticles[num3];
				if (!particle2.Dead() && particle2.mAdditive)
				{
					float alpha_pct2 = this.mSystem.mAlphaPct * this.mLastPTFrameSetting[particle2.mParentType.mSerialIndex].mGlobalVisibility * this.mLastSettings.mVisibility * vis_mult;
					particle2.Draw(g, alpha_pct2, this.mTintColor, this.mLastSettings.mTintStrength * tint_mult, this.mSystem.mScale);
				}
				num3 += (this.mDrawNewestFirst ? (-1) : 1);
			}
			num = (this.mDrawNewestFirst ? (this.mEmitters.size<Emitter>() - 1) : 0);
			int num4 = num;
			while (this.mDrawNewestFirst ? (num4 >= 0) : (num4 < this.mEmitters.size<Emitter>()))
			{
				Emitter emitter = this.mEmitters[num4];
				if (!emitter.Dead())
				{
					emitter.Draw(g, this.mLastSettings.mVisibility, this.mLastSettings.mTintStrength);
				}
				num4 += (this.mDrawNewestFirst ? (-1) : 1);
			}
			if (this.mClipRect != Rect.ZERO_RECT)
			{
				g.PopState();
			}
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x0003E601 File Offset: 0x0003C801
		public void Draw(Graphics g, float vis_mult)
		{
			this.Draw(g, vis_mult, 1f);
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x0003E610 File Offset: 0x0003C810
		public void Draw(Graphics g)
		{
			this.Draw(g, 1f, 1f);
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x0003E624 File Offset: 0x0003C824
		public void Move(float xamt, float yamt)
		{
			if (this.mParentEmitter == null && this.mWaypointManager.GetNumPoints() > 0)
			{
				return;
			}
			this.mX += xamt;
			this.mY += yamt;
			for (int i = 0; i < this.mParticles.size<Particle>(); i++)
			{
				this.mParticles[i].SetX(this.mParticles[i].GetX() + xamt * this.mParticles[i].mParentType.mEmitterAttachPct);
				this.mParticles[i].SetY(this.mParticles[i].GetY() + yamt * this.mParticles[i].mParentType.mEmitterAttachPct);
			}
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x0003E6F4 File Offset: 0x0003C8F4
		public void SetPos(float x, float y)
		{
			float mX = this.mX;
			float mY = this.mY;
			this.mX = x;
			this.mY = y;
			for (int i = 0; i < this.mParticles.size<Particle>(); i++)
			{
				Particle particle = this.mParticles[i];
				particle.SetX(particle.GetX() + (x - mX) * particle.mParentType.mEmitterAttachPct);
				particle.SetY(particle.GetY() + (y - mY) * particle.mParentType.mEmitterAttachPct);
			}
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0003E778 File Offset: 0x0003C978
		public void SetAreaMask(MemoryImage mask, bool invert)
		{
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0003E77C File Offset: 0x0003C97C
		public void AddLineEmitterKeyFrame(int point_num, int frame, SexyPoint p)
		{
			if (point_num >= this.mLineEmitterPoints.size<LineEmitterPoint>())
			{
				this.mLineEmitterPoints.Resize(point_num + 1);
			}
			this.mLineEmitterPoints[point_num].mKeyFramePoints.Add(new PointKeyFrame(frame, p));
			this.mLineEmitterPoints[point_num].mKeyFramePoints.Sort(new SortPointKeyFrames());
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0003E7DD File Offset: 0x0003C9DD
		public void DebugDraw(Graphics g, int size)
		{
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x0003E7E0 File Offset: 0x0003C9E0
		public void ApplyForce(Force f)
		{
			for (int i = 0; i < this.mParticles.size<Particle>(); i++)
			{
				f.Apply(this.mParticles[i]);
			}
			for (int j = 0; j < this.mEmitters.size<Emitter>(); j++)
			{
				f.Apply(this.mEmitters[j]);
				this.mEmitters[j].ApplyForce(f);
			}
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x0003E850 File Offset: 0x0003CA50
		public void ApplyDeflector(Deflector d)
		{
			for (int i = 0; i < this.mParticles.size<Particle>(); i++)
			{
				d.Apply(this.mParticles[i]);
			}
			for (int j = 0; j < this.mEmitters.size<Emitter>(); j++)
			{
				d.Apply(this.mEmitters[j]);
				this.mEmitters[j].ApplyDeflector(d);
			}
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x0003E8C0 File Offset: 0x0003CAC0
		public void GetParticlesOfType(int particle_type_handle, ref List<Particle> particles)
		{
			ParticleType particleType = this.GetParticleType(particle_type_handle);
			for (int i = 0; i < this.mParticles.size<Particle>(); i++)
			{
				if (this.mParticles[i].mParentType == particleType)
				{
					particles.Add(this.mParticles[i]);
				}
			}
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x0003E912 File Offset: 0x0003CB12
		public ParticleType GetParticleType(int particle_type_handle)
		{
			return this.mParticleTypeInfo[particle_type_handle].first;
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0003E925 File Offset: 0x0003CB25
		public int GetNumParticleTypes()
		{
			return this.mParticleTypeInfo.size<ParticleTypeInfo>();
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0003E932 File Offset: 0x0003CB32
		public ParticleType GetParticleTypeByIndex(int idx)
		{
			return this.mParticleTypeInfo[idx].first;
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0003E945 File Offset: 0x0003CB45
		public int GetHandle()
		{
			return this.mHandle;
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x0003E950 File Offset: 0x0003CB50
		public int NumParticles()
		{
			int num = this.mParticles.size<Particle>();
			for (int i = 0; i < this.mEmitters.size<Emitter>(); i++)
			{
				num += this.mEmitters[i].NumParticles();
			}
			return num;
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x0003E994 File Offset: 0x0003CB94
		public void SetEmitterType(int t)
		{
			this.mEmitterType = t;
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0003E99D File Offset: 0x0003CB9D
		public void LoopSettingsTimeLine(bool l)
		{
			this.mSettingsTimeLine.mLoop = l;
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x0003E9AB File Offset: 0x0003CBAB
		public void LoopScaleTimeLine(bool l)
		{
			this.mScaleTimeLine.mLoop = l;
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x0003E9BC File Offset: 0x0003CBBC
		public override bool Dead()
		{
			bool flag = base.Dead();
			return (!this.mWaitForParticles || this.mParticles.size<Particle>() <= 0) && flag;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0003E9E9 File Offset: 0x0003CBE9
		public virtual bool Active()
		{
			return this.mLastSettings != null && this.mLastSettings.mActive;
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0003EA00 File Offset: 0x0003CC00
		public void DisableQuadRep(bool val)
		{
			this.mDisableQuadRep = val;
			for (int i = 0; i < this.mParticleTypeInfo.size<ParticleTypeInfo>(); i++)
			{
				if (this.mParticleTypeInfo[i].first.mImage != null)
				{
					if (val)
					{
						((MemoryImage)this.mParticleTypeInfo[i].first.mImage).AddImageFlags(128U);
					}
					else
					{
						((MemoryImage)this.mParticleTypeInfo[i].first.mImage).RemoveImageFlags(128U);
					}
				}
			}
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0003EA91 File Offset: 0x0003CC91
		public override float GetX()
		{
			if (this.mWaypointManager.GetNumPoints() == 0 || this.mParentEmitter != null)
			{
				return base.GetX();
			}
			return this.mWaypointManager.GetLastPoint().X;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0003EABF File Offset: 0x0003CCBF
		public override float GetY()
		{
			if (this.mWaypointManager.GetNumPoints() == 0 || this.mParentEmitter != null)
			{
				return base.GetY();
			}
			return this.mWaypointManager.GetLastPoint().Y;
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0003EAED File Offset: 0x0003CCED
		public override bool CanInteract()
		{
			return (this.mWaypointManager.GetNumPoints() == 0 || this.mParentEmitter != null) && base.CanInteract();
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0003EB0C File Offset: 0x0003CD0C
		public int GetNumFreeEmitters()
		{
			return this.mFreeEmitterInfo.size<FreeEmitterInfo>();
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x0003EB1C File Offset: 0x0003CD1C
		public int GetNumSingleParticles()
		{
			int num = 0;
			for (int i = 0; i < this.mParticles.size<Particle>(); i++)
			{
				if (this.mParticles[i].mParentType.mSingle)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x0003EB5E File Offset: 0x0003CD5E
		public Emitter GetEmitter(int idx)
		{
			return this.mFreeEmitterInfo[idx].first.mEmitter;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x0003EB76 File Offset: 0x0003CD76
		public System GetSystem()
		{
			return this.mSystem;
		}

		// Token: 0x040009B8 RID: 2488
		public int mSuperEmitterIndex;

		// Token: 0x040009B9 RID: 2489
		public int mPoolIndex;

		// Token: 0x040009BA RID: 2490
		public MemoryImage mAreaMask;

		// Token: 0x040009BB RID: 2491
		public bool mInvertAreaMask;

		// Token: 0x040009BC RID: 2492
		public System mSystem;

		// Token: 0x040009BD RID: 2493
		public List<LineEmitterPoint> mLineEmitterPoints = new List<LineEmitterPoint>();

		// Token: 0x040009BE RID: 2494
		public TimeLine mScaleTimeLine = new TimeLine();

		// Token: 0x040009BF RID: 2495
		public TimeLine mSettingsTimeLine = new TimeLine();

		// Token: 0x040009C0 RID: 2496
		public List<Particle> mParticles = new List<Particle>();

		// Token: 0x040009C1 RID: 2497
		public List<ParticleTypeInfo> mParticleTypeInfo = new List<ParticleTypeInfo>();

		// Token: 0x040009C2 RID: 2498
		public List<Emitter> mEmitters = new List<Emitter>();

		// Token: 0x040009C3 RID: 2499
		public List<FreeEmitterInfo> mFreeEmitterInfo = new List<FreeEmitterInfo>();

		// Token: 0x040009C4 RID: 2500
		public Dictionary<int, ParticleSettings> mLastPTFrameSetting = new Dictionary<int, ParticleSettings>();

		// Token: 0x040009C5 RID: 2501
		public Dictionary<FreeEmitter, FreeEmitterSettings> mLastFEFrameSetting = new Dictionary<FreeEmitter, FreeEmitterSettings>();

		// Token: 0x040009C6 RID: 2502
		public EmitterSettings mLastSettings;

		// Token: 0x040009C7 RID: 2503
		public EmitterScale mLastScale;

		// Token: 0x040009C8 RID: 2504
		public MemoryImage mDebugMaskImage;

		// Token: 0x040009C9 RID: 2505
		public SexyPoint[] mMaskPoints;

		// Token: 0x040009CA RID: 2506
		public FreeEmitter mParentEmitter;

		// Token: 0x040009CB RID: 2507
		public Emitter mSuperEmitter;

		// Token: 0x040009CC RID: 2508
		public float mNumSpawnAccumulator;

		// Token: 0x040009CD RID: 2509
		public bool mDisableQuadRep;

		// Token: 0x040009CE RID: 2510
		public int mFrameOffset;

		// Token: 0x040009CF RID: 2511
		public int mEmitterType;

		// Token: 0x040009D0 RID: 2512
		public int mNumMaskPoints;

		// Token: 0x040009D1 RID: 2513
		public int mLastEmitAtX;

		// Token: 0x040009D2 RID: 2514
		public int mLastEmitAtY;

		// Token: 0x040009D3 RID: 2515
		public int mHandle;

		// Token: 0x040009D4 RID: 2516
		public WaypointManager mWaypointManager;

		// Token: 0x040009D5 RID: 2517
		public Rect mCullingRect = default(Rect);

		// Token: 0x040009D6 RID: 2518
		public Rect mClipRect = default(Rect);

		// Token: 0x040009D7 RID: 2519
		public string mName = "";

		// Token: 0x040009D8 RID: 2520
		public bool mDrawNewestFirst;

		// Token: 0x040009D9 RID: 2521
		public bool mWaitForParticles;

		// Token: 0x040009DA RID: 2522
		public bool mUseAlternateCalcMethod;

		// Token: 0x040009DB RID: 2523
		public bool mDeleteInvisParticles;

		// Token: 0x040009DC RID: 2524
		public bool mParticlesMustHaveBeenVisible;

		// Token: 0x040009DD RID: 2525
		public bool mEmissionCoordsAreOffsets;

		// Token: 0x040009DE RID: 2526
		public int mPreloadFrames;

		// Token: 0x040009DF RID: 2527
		public int mStartFrame;

		// Token: 0x040009E0 RID: 2528
		public int mEmitDir;

		// Token: 0x040009E1 RID: 2529
		public int mEmitAtXPoints;

		// Token: 0x040009E2 RID: 2530
		public int mEmitAtYPoints;

		// Token: 0x040009E3 RID: 2531
		public int mSerialIndex;

		// Token: 0x040009E4 RID: 2532
		public bool mLinearEmitAtPoints;

		// Token: 0x040009E5 RID: 2533
		public SexyColor mTintColor = default(SexyColor);
	}
}
