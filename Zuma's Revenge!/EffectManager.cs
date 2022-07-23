using System;
using System.Collections.Generic;
using SexyFramework;

namespace ZumasRevenge
{
	// Token: 0x020000A3 RID: 163
	public class EffectManager : IDisposable
	{
		// Token: 0x06000A22 RID: 2594 RVA: 0x0006093D File Offset: 0x0005EB3D
		public EffectManager()
		{
			this.Reset();
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00060958 File Offset: 0x0005EB58
		public virtual void Dispose()
		{
			for (int i = 0; i < this.mEffects.Count; i++)
			{
				if (this.mEffects[i] != null)
				{
					this.mEffects[i].Dispose();
					this.mEffects[i] = null;
				}
			}
			this.mEffects.Clear();
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x000609B4 File Offset: 0x0005EBB4
		public void Reset()
		{
			if (GameApp.gApp != null && GameApp.gApp.mShutdown)
			{
				return;
			}
			for (int i = 0; i < this.mEffects.Count; i++)
			{
				if (this.mEffects[i] != null)
				{
					this.mEffects[i].Dispose();
					this.mEffects[i] = null;
				}
			}
			this.mEffects.Clear();
			this.mEffects.Add(new WaterEffect1());
			this.mEffects.Add(new WillOWisp());
			this.mEffects.Add(new BallWake());
			this.mEffects.Add(new Fog());
			this.mEffects.Add(new WaterShader1());
			this.mEffects.Add(new LavaShader());
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x00060A84 File Offset: 0x0005EC84
		public Effect GetEffect(string fx_name, string level_id, Level copy_effects_from)
		{
			int i = 0;
			while (i < this.mEffects.Count)
			{
				Effect effect = this.mEffects[i];
				if (Common.StrEquals(effect.GetName(), fx_name, true))
				{
					Effect effect2 = null;
					if (copy_effects_from != null)
					{
						for (int j = 0; j < copy_effects_from.mEffects.Count; j++)
						{
							if (Common.StrEquals(copy_effects_from.mEffects[j].GetName(), fx_name, true))
							{
								effect2 = copy_effects_from.mEffects[j];
								break;
							}
						}
					}
					if (effect2 != null)
					{
						return effect2;
					}
					effect.Reset(level_id);
					return effect;
				}
				else
				{
					i++;
				}
			}
			return null;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00060B16 File Offset: 0x0005ED16
		private Effect GetEffect(string fx_name, string level_id)
		{
			return this.GetEffect(fx_name, level_id);
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x00060B20 File Offset: 0x0005ED20
		private void CopyFrom(EffectManager m)
		{
			this.Reset();
			for (int i = 0; i < m.mEffects.Count; i++)
			{
				this.mEffects[i].CopyFrom(m.mEffects[i]);
			}
		}

		// Token: 0x0400086D RID: 2157
		protected List<Effect> mEffects = new List<Effect>();
	}
}
