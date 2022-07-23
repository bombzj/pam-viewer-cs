using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.PIL
{
	// Token: 0x02000165 RID: 357
	public class Force
	{
		// Token: 0x06000CA1 RID: 3233 RVA: 0x0003EC88 File Offset: 0x0003CE88
		public Force()
		{
			this.mCenterX = 0f;
			this.mCenterY = 0f;
			this.mLastAX = 0f;
			this.mLastAY = 0f;
			this.mTimeLine.mCurrentSettings = new ForceSettings();
			this.mLastSettings = (ForceSettings)this.mTimeLine.mCurrentSettings;
			this.mWaypointManager = new WaypointManager();
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0003ED03 File Offset: 0x0003CF03
		public virtual void Dispose()
		{
			this.mWaypointManager.Dispose();
			this.mWaypointManager = null;
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0003ED17 File Offset: 0x0003CF17
		public void ResetForReuse()
		{
			this.mTimeLine.mCurrentSettings = new ForceSettings();
			this.mLastSettings = (ForceSettings)this.mTimeLine.mCurrentSettings;
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0003ED40 File Offset: 0x0003CF40
		public void Update(int frame)
		{
			this.mTimeLine.Update(frame);
			float num = ModVal.M(2000f);
			this.mLastAX = this.mLastSettings.mStrength / num * (float)Math.Cos((double)(this.mLastSettings.mAngle + this.mLastSettings.mDirection));
			this.mLastAY = -(this.mLastSettings.mStrength / num) * (float)Math.Sin((double)(this.mLastSettings.mAngle + this.mLastSettings.mDirection));
			if (this.mWaypointManager.GetNumPoints() > 0)
			{
				this.mWaypointManager.Update(frame);
				this.mCenterX = this.mWaypointManager.GetLastPoint().X;
				this.mCenterY = this.mWaypointManager.GetLastPoint().Y;
			}
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0003EE0F File Offset: 0x0003D00F
		public void DebugDraw(Graphics g)
		{
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0003EE14 File Offset: 0x0003D014
		public void Apply(MovableObject p)
		{
			if (!p.CanInteract())
			{
				return;
			}
			Rect r = new Rect((int)(this.mCenterX - this.mLastSettings.mWidth / 2f), (int)(this.mCenterY - this.mLastSettings.mHeight / 2f), (int)this.mLastSettings.mWidth, (int)this.mLastSettings.mHeight);
			Rect r2 = new Rect((int)(p.GetX() - 1f), (int)(p.GetY() - 1f), 2, 2);
			if (Common.RotatedRectsIntersect(r, this.mLastSettings.mAngle, r2, -p.mAngle))
			{
				p.ApplyAcceleration(this.mLastAX, this.mLastAY);
			}
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0003EECB File Offset: 0x0003D0CB
		public void AddKeyFrame(int frame, ForceSettings p)
		{
			this.mTimeLine.AddKeyFrame(frame, p);
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x0003EEDA File Offset: 0x0003D0DA
		public void LoopTimeLine(bool l)
		{
			this.mTimeLine.mLoop = l;
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0003EEE8 File Offset: 0x0003D0E8
		public virtual void Serialize(SexyBuffer b)
		{
			this.mTimeLine.Serialize(b);
			this.mLastSettings.Serialize(b);
			b.WriteFloat(this.mLastAX);
			b.WriteFloat(this.mLastAY);
			b.WriteFloat(this.mCenterX);
			b.WriteFloat(this.mCenterY);
			this.mWaypointManager.Serialize(b);
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0003EF4C File Offset: 0x0003D14C
		public virtual void Deserialize(SexyBuffer b)
		{
			this.mTimeLine.Deserialize(b, new GlobalMembers.KFDInstantiateFunc(ForceSettings.Instantiate));
			this.mLastSettings.Deserialize(b);
			this.mLastAX = b.ReadFloat();
			this.mLastAY = b.ReadFloat();
			this.mCenterX = b.ReadFloat();
			this.mCenterY = b.ReadFloat();
			this.mWaypointManager.Deserialize(b);
		}

		// Token: 0x040009E6 RID: 2534
		protected TimeLine mTimeLine = new TimeLine();

		// Token: 0x040009E7 RID: 2535
		protected ForceSettings mLastSettings;

		// Token: 0x040009E8 RID: 2536
		protected float mLastAX;

		// Token: 0x040009E9 RID: 2537
		protected float mLastAY;

		// Token: 0x040009EA RID: 2538
		public float mCenterX;

		// Token: 0x040009EB RID: 2539
		public float mCenterY;

		// Token: 0x040009EC RID: 2540
		public WaypointManager mWaypointManager;

		// Token: 0x040009ED RID: 2541
		public System mSystem;
	}
}
