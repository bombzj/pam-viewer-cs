using System;
using SexyFramework;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x02000065 RID: 101
	public class EndLevelExplosion : IDisposable
	{
		// Token: 0x06000739 RID: 1849 RVA: 0x00030CB0 File Offset: 0x0002EEB0
		public EndLevelExplosion()
		{
			this.mPIEffect = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_END_LEVEL_EXPLOSION).Duplicate();
			Common.SetFXNumScale(this.mPIEffect, GlobalMembers.gSexyAppBase.Is3DAccelerated() ? 1f : Common._M(0.5f));
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00030D00 File Offset: 0x0002EF00
		public virtual void Dispose()
		{
			if (this.mPIEffect != null)
			{
				this.mPIEffect.Dispose();
			}
			this.mPIEffect = null;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00030D1C File Offset: 0x0002EF1C
		public void SetPos(int x, int y)
		{
			this.mPIEffect.mDrawTransform.LoadIdentity();
			float num = GameApp.DownScaleNum(1f);
			this.mPIEffect.mDrawTransform.Scale(num, num);
			this.mPIEffect.mDrawTransform.Translate((float)Common._S(x), (float)Common._S(y));
		}

		// Token: 0x0400048B RID: 1163
		public int mDelay;

		// Token: 0x0400048C RID: 1164
		public int mX;

		// Token: 0x0400048D RID: 1165
		public int mY;

		// Token: 0x0400048E RID: 1166
		public PIEffect mPIEffect;
	}
}
