using System;
using System.Collections.Generic;
using SexyFramework.Misc;

namespace SexyFramework.PIL
{
	// Token: 0x02000168 RID: 360
	public class FreeEmitter
	{
		// Token: 0x06000CD9 RID: 3289 RVA: 0x0003F2A0 File Offset: 0x0003D4A0
		public FreeEmitter()
		{
			this.mSettingsTimeLine.mCurrentSettings = new FreeEmitterSettings();
			this.mVarianceTimeLine.mCurrentSettings = new FreeEmitterVariance();
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0003F302 File Offset: 0x0003D502
		public FreeEmitter(FreeEmitter rhs)
			: this()
		{
			this.CopyFrom(rhs);
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0003F311 File Offset: 0x0003D511
		public virtual void Dispose()
		{
			if (this.mEmitter != null)
			{
				this.mEmitter.Dispose();
			}
			this.mEmitter = null;
			this.mLifePctSettings.Clear();
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0003F338 File Offset: 0x0003D538
		public void CopyFrom(FreeEmitter rhs)
		{
			if (rhs == null)
			{
				return;
			}
			this.mSettingsTimeLine = rhs.mSettingsTimeLine;
			this.mVarianceTimeLine = rhs.mVarianceTimeLine;
			this.mAspectLocked = rhs.mAspectLocked;
			if (this.mEmitter == null)
			{
				this.mEmitter = new Emitter(rhs.mEmitter);
			}
			else
			{
				this.mEmitter.CopyFrom(rhs.mEmitter);
			}
			for (int i = 0; i < this.mLifePctSettings.size<LifetimeSettingPct>(); i++)
			{
				this.mLifePctSettings[i].second = null;
			}
			this.mLifePctSettings.Clear();
			for (int j = 0; j < rhs.mLifePctSettings.size<LifetimeSettingPct>(); j++)
			{
				this.AddLifetimeKeyFrame(rhs.mLifePctSettings[j].first, new LifetimeSettings(rhs.mLifePctSettings[j].second));
			}
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0003F410 File Offset: 0x0003D610
		public void GetCreationParams(int frame, out int emitter_life, out float emit_frame, out FreeEmitterSettings settings, out FreeEmitterVariance variance)
		{
			this.mSettingsTimeLine.Update(frame);
			this.mVarianceTimeLine.Update(frame);
			if (this.mSettingsTimeLine.mKeyFrames.size<KeyFrame>() == 0)
			{
				settings = new FreeEmitterSettings();
				this.mSettingsTimeLine.mCurrentSettings = settings;
			}
			else
			{
				settings = (FreeEmitterSettings)this.mSettingsTimeLine.mCurrentSettings;
			}
			if (this.mVarianceTimeLine.mKeyFrames.size<KeyFrame>() == 0)
			{
				variance = new FreeEmitterVariance();
				this.mVarianceTimeLine.mCurrentSettings = variance;
			}
			else
			{
				variance = (FreeEmitterVariance)this.mVarianceTimeLine.mCurrentSettings;
			}
			emitter_life = this.GetRandomizedLife();
			emit_frame = 100f / (float)settings.mNumber;
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0003F4C8 File Offset: 0x0003D6C8
		public int GetRandomizedLife()
		{
			FreeEmitterSettings freeEmitterSettings = this.mSettingsTimeLine.mCurrentSettings as FreeEmitterSettings;
			FreeEmitterVariance freeEmitterVariance = this.mVarianceTimeLine.mCurrentSettings as FreeEmitterVariance;
			int num = freeEmitterSettings.mLife - (int)Common.SAFE_RAND((float)(freeEmitterVariance.mLifeVar / 2)) + (int)Common.SAFE_RAND((float)(freeEmitterVariance.mLifeVar / 2));
			return 10 * num;
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0003F524 File Offset: 0x0003D724
		public LifetimeSettings AddLifetimeKeyFrame(float pct, LifetimeSettings s, float second_frame_pct, bool make_new)
		{
			LifetimeSettingPct lifetimeSettingPct = new LifetimeSettingPct(pct, s);
			lifetimeSettingPct.second.mPct = pct;
			this.mLifePctSettings.Add(lifetimeSettingPct);
			this.mLifePctSettings.Sort(new LifePctSort());
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

		// Token: 0x06000CE0 RID: 3296 RVA: 0x0003F583 File Offset: 0x0003D783
		public LifetimeSettings AddLifetimeKeyFrame(float pct, LifetimeSettings s, float second_frame_pct)
		{
			return this.AddLifetimeKeyFrame(pct, s, second_frame_pct, false);
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0003F58F File Offset: 0x0003D78F
		public LifetimeSettings AddLifetimeKeyFrame(float pct, LifetimeSettings s)
		{
			return this.AddLifetimeKeyFrame(pct, s, -1f, false);
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x0003F59F File Offset: 0x0003D79F
		public FreeEmitterSettings AddSettingsKeyFrame(int frame, FreeEmitterSettings s, int second_frame_time, bool make_new)
		{
			this.mSettingsTimeLine.AddKeyFrame(frame, s);
			if (second_frame_time != -1)
			{
				this.mSettingsTimeLine.AddKeyFrame(second_frame_time, new FreeEmitterSettings(s));
				if (make_new)
				{
					return new FreeEmitterSettings(s);
				}
			}
			return null;
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0003F5D0 File Offset: 0x0003D7D0
		public FreeEmitterSettings AddSettingsKeyFrame(int frame, FreeEmitterSettings s, int second_frame_time)
		{
			return this.AddSettingsKeyFrame(frame, s, second_frame_time, false);
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x0003F5DC File Offset: 0x0003D7DC
		public FreeEmitterSettings AddSettingsKeyFrame(int frame, FreeEmitterSettings s)
		{
			return this.AddSettingsKeyFrame(frame, s, -1, false);
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0003F5E8 File Offset: 0x0003D7E8
		public FreeEmitterVariance AddVarianceKeyFrame(int frame, FreeEmitterVariance v, int second_frame_time, bool make_new)
		{
			this.mVarianceTimeLine.AddKeyFrame(frame, v);
			if (second_frame_time != -1)
			{
				this.mVarianceTimeLine.AddKeyFrame(second_frame_time, new FreeEmitterVariance(v));
				if (make_new)
				{
					return new FreeEmitterVariance(v);
				}
			}
			return null;
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x0003F619 File Offset: 0x0003D819
		public FreeEmitterVariance AddVarianceKeyFrame(int frame, FreeEmitterVariance v, int second_frame_time)
		{
			return this.AddVarianceKeyFrame(frame, v, second_frame_time, false);
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0003F625 File Offset: 0x0003D825
		public FreeEmitterVariance AddVarianceKeyFrame(int frame, FreeEmitterVariance v)
		{
			return this.AddVarianceKeyFrame(frame, v, -1, false);
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0003F631 File Offset: 0x0003D831
		public void LoopSettingsTimeLine(bool l)
		{
			this.mSettingsTimeLine.mLoop = l;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0003F63F File Offset: 0x0003D83F
		public void LoopVarianceTimeLine(bool l)
		{
			this.mVarianceTimeLine.mLoop = l;
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0003F650 File Offset: 0x0003D850
		public void Serialize(SexyBuffer b, GlobalMembers.GetIdByImageFunc f)
		{
			this.mSettingsTimeLine.Serialize(b);
			this.mVarianceTimeLine.Serialize(b);
			b.WriteBoolean(this.mAspectLocked);
			b.WriteLong((long)this.mSerialIndex);
			b.WriteLong((long)this.mLifePctSettings.Count);
			for (int i = 0; i < this.mLifePctSettings.Count; i++)
			{
				b.WriteFloat(this.mLifePctSettings[i].first);
				this.mLifePctSettings[i].second.Serialize(b);
			}
			this.mEmitter.Serialize(b, f);
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0003F6F4 File Offset: 0x0003D8F4
		public void Deserialize(SexyBuffer b, GlobalMembers.GetImageByIdFunc f)
		{
			this.mSettingsTimeLine.Deserialize(b, new GlobalMembers.KFDInstantiateFunc(FreeEmitterSettings.Instantiate));
			this.mVarianceTimeLine.Deserialize(b, new GlobalMembers.KFDInstantiateFunc(FreeEmitterVariance.Instantiate));
			this.mAspectLocked = b.ReadBoolean();
			this.mSerialIndex = (int)b.ReadLong();
			int num = (int)b.ReadLong();
			for (int i = 0; i < num; i++)
			{
				float f2 = b.ReadFloat();
				LifetimeSettings lifetimeSettings = new LifetimeSettings();
				lifetimeSettings.Deserialize(b);
				this.mLifePctSettings.Add(new LifetimeSettingPct(f2, lifetimeSettings));
			}
			Dictionary<int, Deflector> deflector_ptr_map = new Dictionary<int, Deflector>();
			Dictionary<int, FreeEmitter> fe_ptr_map = new Dictionary<int, FreeEmitter>();
			this.mEmitter.Deserialize(b, deflector_ptr_map, fe_ptr_map, f);
		}

		// Token: 0x040009EE RID: 2542
		public TimeLine mSettingsTimeLine = new TimeLine();

		// Token: 0x040009EF RID: 2543
		public TimeLine mVarianceTimeLine = new TimeLine();

		// Token: 0x040009F0 RID: 2544
		public List<LifetimeSettingPct> mLifePctSettings = new List<LifetimeSettingPct>();

		// Token: 0x040009F1 RID: 2545
		public Emitter mEmitter;

		// Token: 0x040009F2 RID: 2546
		public bool mAspectLocked = true;

		// Token: 0x040009F3 RID: 2547
		public int mSerialIndex = -1;
	}
}
