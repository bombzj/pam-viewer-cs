using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x02000034 RID: 52
	public class EyeAnim
	{
		// Token: 0x060005C2 RID: 1474 RVA: 0x000201AC File Offset: 0x0001E3AC
		public EyeAnim()
		{
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x000201B4 File Offset: 0x0001E3B4
		public EyeAnim(EyeAnim rhs)
		{
			if (rhs == null || rhs == this)
			{
				return;
			}
			this.mEyeFlame = rhs.mEyeFlame;
			this.mFiring = rhs.mFiring;
			this.mUpdateCount = rhs.mUpdateCount;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x000201E8 File Offset: 0x0001E3E8
		public void Update(int x, int y, int alpha)
		{
			if (!this.mFiring && this.mEyeFlame.mCurNumParticles == 0)
			{
				return;
			}
			this.mUpdateCount++;
			this.mEyeFlame.mDrawTransform.LoadIdentity();
			float num = GameApp.DownScaleNum(1f);
			this.mEyeFlame.mDrawTransform.Scale(num, num);
			this.mEyeFlame.mDrawTransform.Translate((float)Common._S(x), (float)Common._S(y));
			this.mEyeFlame.mColor.mAlpha = alpha;
			this.mEyeFlame.Update();
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00020280 File Offset: 0x0001E480
		public void Draw(Graphics g)
		{
			if (!this.mFiring && this.mEyeFlame.mCurNumParticles == 0)
			{
				return;
			}
			this.mEyeFlame.Draw(g);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x000202A4 File Offset: 0x0001E4A4
		public void SyncState(DataSync sync)
		{
			sync.SyncLong(ref this.mUpdateCount);
			sync.SyncBoolean(ref this.mFiring);
			if (sync.isWrite())
			{
				Common.SerializePIEffect(this.mEyeFlame, sync);
				return;
			}
			Common.DeserializePIEffect(this.mEyeFlame, sync);
		}

		// Token: 0x040002A3 RID: 675
		public PIEffect mEyeFlame;

		// Token: 0x040002A4 RID: 676
		public bool mFiring;

		// Token: 0x040002A5 RID: 677
		public int mUpdateCount;
	}
}
