using System;
using SexyFramework;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x02000064 RID: 100
	public class BallExplosion : IDisposable
	{
		// Token: 0x06000732 RID: 1842 RVA: 0x00030BB0 File Offset: 0x0002EDB0
		public BallExplosion()
		{
			this.mPIEffect = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_BALL_EXPLODE).Duplicate();
			Common.SetFXNumScale(this.mPIEffect, GlobalMembers.gSexyAppBase.Is3DAccelerated() ? 1f : Common._M(0.3f));
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00030C00 File Offset: 0x0002EE00
		public virtual void Dispose()
		{
			if (this.mPIEffect != null)
			{
				this.mPIEffect.Dispose();
			}
			this.mPIEffect = null;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00030C1C File Offset: 0x0002EE1C
		public void Init()
		{
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00030C1E File Offset: 0x0002EE1E
		public void Release()
		{
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00030C20 File Offset: 0x0002EE20
		public bool Update()
		{
			if (this.mPIEffect == null)
			{
				return true;
			}
			this.mPIEffect.Update();
			return !this.mPIEffect.IsActive();
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00030C47 File Offset: 0x0002EE47
		public void Draw(Graphics g)
		{
			this.mPIEffect.Draw(g);
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00030C58 File Offset: 0x0002EE58
		public void SetPos(int x, int y)
		{
			this.mPIEffect.mDrawTransform.LoadIdentity();
			float num = GameApp.DownScaleNum(1f);
			this.mPIEffect.mDrawTransform.Scale(num, num);
			this.mPIEffect.mDrawTransform.Translate((float)Common._S(x), (float)Common._S(y));
		}

		// Token: 0x0400048A RID: 1162
		public PIEffect mPIEffect;
	}
}
