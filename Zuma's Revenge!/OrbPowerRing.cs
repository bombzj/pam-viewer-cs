using System;
using System.Collections.Generic;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x020000B2 RID: 178
	public class OrbPowerRing : IDisposable
	{
		// Token: 0x06000A79 RID: 2681 RVA: 0x00065A45 File Offset: 0x00063C45
		public OrbPowerRing()
		{
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00065A58 File Offset: 0x00063C58
		public OrbPowerRing(float angle, float max_radius, float alpha_fade, float size_fade, float angle_inc)
		{
			this.mAlphaFade = alpha_fade;
			this.mSizeFade = size_fade;
			this.mAngle = angle;
			this.mRadius = 0f;
			this.mMaxRadius = max_radius;
			this.mExpanding = true;
			this.mUpdateCount = 0;
			this.mDone = false;
			this.mAngleInc = angle_inc;
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00065ABC File Offset: 0x00063CBC
		public virtual void Dispose()
		{
			for (int i = 0; i < this.mParticles.Count; i++)
			{
				this.mParticles[i] = null;
			}
			this.mParticles.Clear();
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00065AF8 File Offset: 0x00063CF8
		public void Update()
		{
			if (this.mDone)
			{
				return;
			}
			this.mAngle += this.mAngleInc;
			this.mUpdateCount++;
			if (this.mUpdateCount > Common._M(50))
			{
				this.mExpanding = false;
			}
			if (this.mExpanding && this.mRadius < this.mMaxRadius)
			{
				this.mRadius += this.mMaxRadius / Common._M(30f);
			}
			else if (!this.mExpanding && this.mRadius > 0f)
			{
				this.mRadius -= this.mMaxRadius / Common._M(15f);
				if (this.mRadius < 0f)
				{
					this.mRadius = 0f;
				}
			}
			if ((this.mExpanding || this.mRadius > 0f) && this.mUpdateCount % Common._M(1) == 0)
			{
				this.mParticles.Add(new OrbParticle(this.mAngle, this.mRadius, this.mAlphaFade, this.mSizeFade));
			}
			bool flag = true;
			for (int i = 0; i < this.mParticles.Count; i++)
			{
				OrbParticle orbParticle = this.mParticles[i];
				orbParticle.Update();
				if (!orbParticle.IsDone())
				{
					flag = false;
				}
				else
				{
					this.mParticles.RemoveAt(i);
					i--;
				}
			}
			if (!this.mExpanding && flag)
			{
				this.mDone = true;
			}
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00065C70 File Offset: 0x00063E70
		public void Draw(Graphics g, float x, float y)
		{
			if (this.mDone)
			{
				return;
			}
			for (int i = 0; i < this.mParticles.Count; i++)
			{
				this.mParticles[i].Draw(g, x, y);
			}
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00065CB0 File Offset: 0x00063EB0
		public bool IsDone()
		{
			return this.mDone;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00065CB8 File Offset: 0x00063EB8
		public bool IsExpanding()
		{
			return this.mExpanding;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00065CC0 File Offset: 0x00063EC0
		public void SyncState(DataSync sync)
		{
			sync.SyncFloat(ref this.mAngle);
			sync.SyncFloat(ref this.mRadius);
			sync.SyncFloat(ref this.mMaxRadius);
			sync.SyncFloat(ref this.mAlphaFade);
			sync.SyncFloat(ref this.mSizeFade);
			sync.SyncFloat(ref this.mAngleInc);
			sync.SyncBoolean(ref this.mExpanding);
			sync.SyncBoolean(ref this.mDone);
			sync.SyncLong(ref this.mUpdateCount);
			this.SyncListOrbParticles(sync, true);
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00065D44 File Offset: 0x00063F44
		private void SyncListOrbParticles(DataSync sync, bool clear)
		{
			if (sync.isRead())
			{
				if (clear)
				{
					this.mParticles.Clear();
				}
				long num = sync.GetBuffer().ReadLong();
				int num2 = 0;
				while ((long)num2 < num)
				{
					OrbParticle orbParticle = new OrbParticle();
					orbParticle.SyncState(sync);
					this.mParticles.Add(orbParticle);
					num2++;
				}
				return;
			}
			sync.GetBuffer().WriteLong((long)this.mParticles.Count);
			foreach (OrbParticle orbParticle2 in this.mParticles)
			{
				orbParticle2.SyncState(sync);
			}
		}

		// Token: 0x040008E7 RID: 2279
		protected List<OrbParticle> mParticles = new List<OrbParticle>();

		// Token: 0x040008E8 RID: 2280
		protected float mAngle;

		// Token: 0x040008E9 RID: 2281
		protected float mRadius;

		// Token: 0x040008EA RID: 2282
		protected float mMaxRadius;

		// Token: 0x040008EB RID: 2283
		protected float mAlphaFade;

		// Token: 0x040008EC RID: 2284
		protected float mSizeFade;

		// Token: 0x040008ED RID: 2285
		protected float mAngleInc;

		// Token: 0x040008EE RID: 2286
		protected bool mExpanding;

		// Token: 0x040008EF RID: 2287
		protected bool mDone;

		// Token: 0x040008F0 RID: 2288
		protected int mUpdateCount;
	}
}
