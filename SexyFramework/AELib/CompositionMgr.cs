using System;
using System.Collections.Generic;
using SexyFramework.GraphicsLib;

namespace SexyFramework.AELib
{
	// Token: 0x0200000C RID: 12
	public class CompositionMgr
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00003516 File Offset: 0x00001716
		public CompositionMgr()
		{
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000354D File Offset: 0x0000174D
		public CompositionMgr(CompositionMgr other)
		{
			this.CopyFrom(other);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000358C File Offset: 0x0000178C
		public bool isValid()
		{
			foreach (Composition composition in this.mCompositions.Values)
			{
				if (!composition.isValid())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000035EC File Offset: 0x000017EC
		public void CopyFrom(CompositionMgr other)
		{
			this.mLoadImageFunc = other.mLoadImageFunc;
			this.mPostLoadImageFunc = other.mPostLoadImageFunc;
			this.mPreLayerDrawFunc = other.mPreLayerDrawFunc;
			this.mCompositions = new Dictionary<string, Composition>();
			foreach (KeyValuePair<string, Composition> keyValuePair in other.mCompositions)
			{
				this.mCompositions.Add(keyValuePair.Key, new Composition(keyValuePair.Value));
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003688 File Offset: 0x00001888
		public bool LoadFromFile(string file_name)
		{
			List<Composition> list = new List<Composition>();
			if (!AECommon.LoadPAX(file_name, list, this.mLoadImageFunc, this.mPostLoadImageFunc))
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				string text = list[i].mLayerName.ToLower();
				if (!this.mCompositions.ContainsKey(text))
				{
					this.mCompositions.Add(text, null);
				}
				this.mCompositions[text] = new Composition(list[i]);
			}
			return true;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000370C File Offset: 0x0000190C
		public void UpdateAll()
		{
			foreach (KeyValuePair<string, Composition> keyValuePair in this.mCompositions)
			{
				keyValuePair.Value.Update();
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003764 File Offset: 0x00001964
		public void Update(string comp_name)
		{
			if (this.mCompositions.ContainsKey(comp_name))
			{
				this.mCompositions[comp_name].Update();
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003785 File Offset: 0x00001985
		public void DrawAll(Graphics g)
		{
			this.DrawAll(g, null);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000378F File Offset: 0x0000198F
		public void DrawAll(Graphics g, CumulativeTransform ctrans)
		{
			this.DrawAll(g, ctrans, 1f);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000037A0 File Offset: 0x000019A0
		public void DrawAll(Graphics g, CumulativeTransform ctrans, float scale)
		{
			foreach (KeyValuePair<string, Composition> keyValuePair in this.mCompositions)
			{
				keyValuePair.Value.Draw(g, ctrans, -1, scale);
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000037FC File Offset: 0x000019FC
		public void Draw(Graphics g, string comp_name)
		{
			this.Draw(g, comp_name, null);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003807 File Offset: 0x00001A07
		public void Draw(Graphics g, string comp_name, CumulativeTransform ctrans)
		{
			this.Draw(g, comp_name, ctrans, -1);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003813 File Offset: 0x00001A13
		public void Draw(Graphics g, string comp_name, CumulativeTransform ctrans, int frame)
		{
			this.Draw(g, comp_name, ctrans, frame, 1f);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003825 File Offset: 0x00001A25
		public void Draw(Graphics g, string comp_name, CumulativeTransform ctrans, int frame, float scale)
		{
			if (this.mCompositions.ContainsKey(comp_name.ToLower()))
			{
				this.mCompositions[comp_name.ToLower()].Draw(g, ctrans, frame, scale);
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003856 File Offset: 0x00001A56
		public Composition GetComposition(string comp_name)
		{
			if (this.mCompositions.ContainsKey(comp_name.ToLower()))
			{
				return this.mCompositions[comp_name.ToLower()];
			}
			return null;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003880 File Offset: 0x00001A80
		public void GetListOfComps(List<string> comp_names)
		{
			foreach (KeyValuePair<string, Composition> keyValuePair in this.mCompositions)
			{
				comp_names.Add(keyValuePair.Key);
			}
			comp_names.Sort();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000038E0 File Offset: 0x00001AE0
		public void GetAllCompositions(List<Composition> comps)
		{
			foreach (KeyValuePair<string, Composition> keyValuePair in this.mCompositions)
			{
				comps.Add(keyValuePair.Value);
			}
		}

		// Token: 0x0400002E RID: 46
		protected Dictionary<string, Composition> mCompositions = new Dictionary<string, Composition>();

		// Token: 0x0400002F RID: 47
		public AECommon.LoadCompImageFunc mLoadImageFunc = new AECommon.LoadCompImageFunc(Composition.DefaultLoadImageFunc);

		// Token: 0x04000030 RID: 48
		public AECommon.PostLoadCompImageFunc mPostLoadImageFunc = new AECommon.PostLoadCompImageFunc(Composition.DefaultPostLoadImageFunc);

		// Token: 0x04000031 RID: 49
		public AECommon.PreLayerDrawFunc mPreLayerDrawFunc;
	}
}
