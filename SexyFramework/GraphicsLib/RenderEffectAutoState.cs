using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000F8 RID: 248
	public class RenderEffectAutoState
	{
		// Token: 0x06000740 RID: 1856 RVA: 0x00025EDA File Offset: 0x000240DA
		public RenderEffectAutoState(Graphics inGraphics, RenderEffect inEffect)
			: this(inGraphics, inEffect, 1)
		{
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00025EE5 File Offset: 0x000240E5
		public RenderEffectAutoState(Graphics inGraphics)
			: this(inGraphics, null, 1)
		{
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00025EF0 File Offset: 0x000240F0
		public RenderEffectAutoState()
			: this(null, null, 1)
		{
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00025EFC File Offset: 0x000240FC
		public RenderEffectAutoState(Graphics inGraphics, RenderEffect inEffect, int inDefaultPassCount)
		{
			this.mEffect = inEffect;
			this.mPassCount = inDefaultPassCount;
			this.mCurrentPass = 0;
			if (this.mEffect == null)
			{
				return;
			}
			this.mPassCount = this.mEffect.Begin(out this.mRunHandle, (inGraphics != null) ? inGraphics.GetRenderContext() : new HRenderContext());
			if (this.mCurrentPass < this.mPassCount)
			{
				this.mEffect.BeginPass(this.mRunHandle, this.mCurrentPass);
			}
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00025F7C File Offset: 0x0002417C
		public virtual void Dispose()
		{
			if (this.mEffect == null)
			{
				return;
			}
			if (this.mCurrentPass < this.mPassCount)
			{
				this.mEffect.EndPass(this.mRunHandle, this.mCurrentPass);
			}
			this.mEffect.End(this.mRunHandle);
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00025FC8 File Offset: 0x000241C8
		public void Reset(Graphics inGraphics, RenderEffect inEffect)
		{
			this.Reset(inGraphics, inEffect, 1);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00025FD3 File Offset: 0x000241D3
		public void Reset(Graphics inGraphics)
		{
			this.Reset(inGraphics, null, 1);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00025FDE File Offset: 0x000241DE
		public void Reset()
		{
			this.Reset(null, null, 1);
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00025FEC File Offset: 0x000241EC
		public void Reset(Graphics inGraphics, RenderEffect inEffect, int inDefaultPassCount)
		{
			if (this.mEffect != null)
			{
				if (this.mCurrentPass < this.mPassCount)
				{
					this.mEffect.EndPass(this.mRunHandle, this.mCurrentPass);
				}
				this.mEffect.End(this.mRunHandle);
			}
			this.mEffect = inEffect;
			this.mPassCount = inDefaultPassCount;
			this.mCurrentPass = 0;
			if (this.mEffect != null)
			{
				this.mPassCount = this.mEffect.Begin(out this.mRunHandle, (inGraphics != null) ? inGraphics.GetRenderContext() : new HRenderContext(null));
				if (this.mCurrentPass < this.mPassCount)
				{
					this.mEffect.BeginPass(this.mRunHandle, this.mCurrentPass);
				}
			}
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x000260A4 File Offset: 0x000242A4
		public void NextPass()
		{
			if (this.mEffect != null && this.mCurrentPass < this.mPassCount)
			{
				this.mEffect.EndPass(this.mRunHandle, this.mCurrentPass);
			}
			this.mCurrentPass++;
			if (this.mEffect != null && this.mCurrentPass < this.mPassCount)
			{
				this.mEffect.BeginPass(this.mRunHandle, this.mCurrentPass);
			}
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00026119 File Offset: 0x00024319
		public bool IsDone()
		{
			return this.mCurrentPass >= this.mPassCount;
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0002612C File Offset: 0x0002432C
		public bool PassUsesVertexShader()
		{
			return this.mEffect != null && this.mEffect.PassUsesVertexShader(this.mCurrentPass);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00026149 File Offset: 0x00024349
		public bool PassUsesPixelShader()
		{
			return this.mEffect != null && this.mEffect.PassUsesPixelShader(this.mCurrentPass);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00026166 File Offset: 0x00024366
		public static implicit operator bool(RenderEffectAutoState ImpliedObject)
		{
			return !ImpliedObject.IsDone();
		}

		// Token: 0x040006CB RID: 1739
		protected RenderEffect mEffect;

		// Token: 0x040006CC RID: 1740
		protected object mRunHandle;

		// Token: 0x040006CD RID: 1741
		protected int mPassCount;

		// Token: 0x040006CE RID: 1742
		protected int mCurrentPass;
	}
}
