using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x020000AB RID: 171
	public class LavaShader : Effect
	{
		// Token: 0x06000A4A RID: 2634 RVA: 0x00061D48 File Offset: 0x0005FF48
		protected override void Init()
		{
			this.mDisabled = false;
			this.mFadeInFromDeath = (this.mFadeoutDistortion = false);
			if (this.mBuffer == null)
			{
				this.mBuffer = new DeviceImage();
				this.mBuffer.mApp = this.mApp;
				this.mBuffer.AddImageFlags(24U);
				this.mBuffer.SetImageMode(false, false);
			}
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x00061DAA File Offset: 0x0005FFAA
		protected void DoShader(Graphics g, DeviceImage buffer)
		{
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00061DAC File Offset: 0x0005FFAC
		public LavaShader()
		{
			this.mResGroup = "GamePlay";
			this.mApp = GameApp.gApp;
			this.mDisabled = false;
			this.mOrgDistAmt = (this.mDistAmt = (this.mScale = (this.mScroll = 0f)));
			this.mNeedFadein = false;
			this.mFadeInFromDeath = false;
			this.mAffectSkull = true;
			this.mFadeoutDistortion = false;
			this.mApplyTunnels = true;
			this.mApplyFullScene = false;
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00061E2C File Offset: 0x0006002C
		public override void Dispose()
		{
			base.Dispose();
			if (this.mBuffer != null)
			{
				this.mBuffer.Dispose();
				this.mBuffer = null;
			}
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00061E4E File Offset: 0x0006004E
		public override void LevelStarted(bool from_load)
		{
			if (this.mApplyFullScene && this.mOrgDistAmt > 0f)
			{
				this.mFadeoutDistortion = true;
				return;
			}
			this.mFadeoutDistortion = false;
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00061E74 File Offset: 0x00060074
		public override void Update()
		{
			this.Update(false);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x00061E80 File Offset: 0x00060080
		public void Update(bool only_check_shaders_supported)
		{
			Board board = this.mApp.GetBoard();
			if (this.mActivateOnMuMu && !board.mDoMuMuMode)
			{
				return;
			}
			if (this.mNeedFadein)
			{
				if (this.mApp.mBoard == null || this.mApp.mBoard.mTransitionScreenImage == null)
				{
					this.mDistAmt = Math.Min(this.mOrgDistAmt, this.mDistAmt + Common._M(5E-06f));
				}
				this.mDisabled = (double)this.mDistAmt <= 1E-09;
				this.mNeedFadein = this.mDistAmt < this.mOrgDistAmt;
			}
			else if (this.mFadeoutDistortion && this.mDistAmt > 0f && !board.mDoMuMuMode)
			{
				this.mDistAmt -= Common._M(1E-06f);
				if (this.mDistAmt <= 0f)
				{
					this.mDistAmt = 0f;
					this.mDisabled = true;
				}
			}
			else if (board.mDoMuMuMode)
			{
				this.mDistAmt = (this.mOrgDistAmt = Common._M(0.0005f));
				this.mScroll = Common._M(0.08f);
				this.mScale = Common._M(0.2f);
				this.mDisabled = false;
			}
			if (only_check_shaders_supported)
			{
				if (!this.mApp.ShadersSupported())
				{
					return;
				}
			}
			else
			{
				bool flag = !this.mDisabled && this.mApp.ShadersSupported() && board != null && !board.DoingMainDarkFrogSequence();
				if (this.mApp.mLoadingThreadStarted && !this.mApp.mLoadingThreadCompleted)
				{
					flag = false;
				}
				if (!flag)
				{
					return;
				}
			}
			this.mUpdateCount++;
			this.mTimer += Common._M(0.02f);
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x00062040 File Offset: 0x00060240
		public override void DrawUnderBackground(Graphics g)
		{
			Board board = this.mApp.GetBoard();
			bool flag = !this.mDisabled && this.mApp.ShadersSupported() && board != null && !board.DoingMainDarkFrogSequence() && (!this.mActivateOnMuMu || board.mDoMuMuMode);
			if (!this.mApp.mLoadingThreadStarted || !this.mApp.mLoadingThreadCompleted)
			{
			}
			int num = 1024;
			int theStretchedHeight = Common._DS(1200);
			g.DrawImage(this.mApp.mBoard.mBackgroundImage, (Common._S(800) - num) / 2 + GameApp.gScreenShakeX, GameApp.gScreenShakeY, num, theStretchedHeight);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x000620EC File Offset: 0x000602EC
		public override bool DrawTunnel(Graphics g, Image img, int x, int y)
		{
			Board board = this.mApp.GetBoard();
			if (!this.mDisabled && this.mApp.ShadersSupported() && board != null && !board.DoingMainDarkFrogSequence() && this.mActivateOnMuMu)
			{
				bool mDoMuMuMode = board.mDoMuMuMode;
			}
			return true;
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x00062135 File Offset: 0x00060335
		public bool DrawTunnel(Graphics g, Image img, int x, int y, float dist_amt, float scale, float scroll, float timer, float alpha_mult)
		{
			return false;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00062138 File Offset: 0x00060338
		public override void DrawFullScene(Graphics g)
		{
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0006213A File Offset: 0x0006033A
		public override void SetParams(string key, string value)
		{
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0006213C File Offset: 0x0006033C
		public override void NukeParams()
		{
			this.mActivateOnMuMu = false;
			this.mApplyTunnels = true;
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x0006214C File Offset: 0x0006034C
		public override bool DrawSkullPit(Graphics g, HoleMgr hole)
		{
			Board board = this.mApp.GetBoard();
			if (!this.mDisabled && this.mApp.ShadersSupported() && board != null)
			{
				board.DoingMainDarkFrogSequence();
			}
			return false;
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00062185 File Offset: 0x00060385
		public override void UserDied()
		{
			if (this.mFadeoutDistortion && this.mDistAmt < this.mOrgDistAmt)
			{
				this.mDisabled = false;
				this.mFadeInFromDeath = true;
			}
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x000621AB File Offset: 0x000603AB
		public override string GetName()
		{
			return "LavaShader";
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x000621B2 File Offset: 0x000603B2
		public override void CopyFrom(Effect e)
		{
		}

		// Token: 0x040008B1 RID: 2225
		public bool mActivateOnMuMu;

		// Token: 0x040008B2 RID: 2226
		public float mDistAmt;

		// Token: 0x040008B3 RID: 2227
		public float mOrgDistAmt;

		// Token: 0x040008B4 RID: 2228
		public float mScroll;

		// Token: 0x040008B5 RID: 2229
		public float mScale;

		// Token: 0x040008B6 RID: 2230
		public bool mAffectSkull;

		// Token: 0x040008B7 RID: 2231
		public bool mApplyFullScene;

		// Token: 0x040008B8 RID: 2232
		public bool mFadeoutDistortion;

		// Token: 0x040008B9 RID: 2233
		public bool mDisabled;

		// Token: 0x040008BA RID: 2234
		public bool mFadeInFromDeath;

		// Token: 0x040008BB RID: 2235
		public bool mNeedFadein;

		// Token: 0x040008BC RID: 2236
		public bool mApplyTunnels;

		// Token: 0x040008BD RID: 2237
		protected DeviceImage mBuffer;

		// Token: 0x040008BE RID: 2238
		protected GameApp mApp;

		// Token: 0x040008BF RID: 2239
		protected float mTimer;
	}
}
