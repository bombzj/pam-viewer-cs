using System;
using System.Collections.Generic;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000F3 RID: 243
	public class PIEffectBatch
	{
		// Token: 0x0600071D RID: 1821 RVA: 0x00025BD5 File Offset: 0x00023DD5
		public PIEffectBatch()
		{
			this.mPIEffectList = new List<PIEffect>();
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00025BE8 File Offset: 0x00023DE8
		public void AddEffect(PIEffect item)
		{
			this.mPIEffectList.Add(item);
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00025BF6 File Offset: 0x00023DF6
		public void Clear()
		{
			this.mPIEffectList.Clear();
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00025C03 File Offset: 0x00023E03
		public void RemoveAt(int index)
		{
			this.mPIEffectList.RemoveAt(index);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00025C11 File Offset: 0x00023E11
		public void Remove(PIEffect effect)
		{
			this.mPIEffectList.Remove(effect);
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00025C20 File Offset: 0x00023E20
		public void DrawBatch(Graphics g)
		{
			for (int i = 0; i < this.mPIEffectList.Count; i++)
			{
				PIEffect pieffect = this.mPIEffectList[i];
				if (pieffect.mInUse)
				{
					for (int j = 0; j < pieffect.mDef.mLayerDefVector.Count; j++)
					{
						PILayer pilayer = pieffect.mLayerVector[j];
						if (pilayer.mVisible)
						{
							pieffect.DrawLayer(g, pilayer);
						}
					}
				}
			}
			for (int k = 0; k < this.mPIEffectList.Count; k++)
			{
				PIEffect pieffect2 = this.mPIEffectList[k];
				if (pieffect2.mInUse)
				{
					for (int l = 0; l < pieffect2.mDef.mLayerDefVector.Count; l++)
					{
						PILayer pilayer2 = pieffect2.mLayerVector[l];
						if (pilayer2.mVisible)
						{
							pieffect2.DrawLayerNormal(g, pilayer2);
						}
					}
				}
			}
			for (int m = 0; m < this.mPIEffectList.Count; m++)
			{
				PIEffect pieffect3 = this.mPIEffectList[m];
				if (pieffect3.mInUse)
				{
					for (int n = 0; n < pieffect3.mDef.mLayerDefVector.Count; n++)
					{
						PILayer pilayer3 = pieffect3.mLayerVector[n];
						if (pilayer3.mVisible)
						{
							pieffect3.DrawLayerAdditive(g, pilayer3);
						}
					}
				}
			}
			for (int num = 0; num < this.mPIEffectList.Count; num++)
			{
				PIEffect pieffect4 = this.mPIEffectList[num];
				if (pieffect4.mInUse)
				{
					for (int num2 = 0; num2 < pieffect4.mDef.mLayerDefVector.Count; num2++)
					{
						PILayer pilayer4 = pieffect4.mLayerVector[num2];
						if (pilayer4.mVisible)
						{
							pieffect4.DrawPhisycalLayer(g, pilayer4);
						}
					}
				}
			}
		}

		// Token: 0x040006BC RID: 1724
		public List<PIEffect> mPIEffectList;
	}
}
