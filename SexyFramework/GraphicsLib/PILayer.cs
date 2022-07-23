using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000EF RID: 239
	public class PILayer
	{
		// Token: 0x060006C5 RID: 1733 RVA: 0x0001CF4C File Offset: 0x0001B14C
		public PILayer()
		{
			this.mVisible = true;
			this.mColor = new SexyColor(SexyColor.White);
			this.mBkgImage = null;
			this.mBkgTransform.LoadIdentity();
			this.mEmitterInstanceVector.Capacity = 10;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001CFC4 File Offset: 0x0001B1C4
		public void SetVisible(bool isVisible)
		{
			this.mVisible = isVisible;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0001CFCD File Offset: 0x0001B1CD
		public PIEmitterInstance GetEmitter()
		{
			return this.GetEmitter(0);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001CFD6 File Offset: 0x0001B1D6
		public PIEmitterInstance GetEmitter(int theIdx)
		{
			if (theIdx < this.mEmitterInstanceVector.Count)
			{
				return this.mEmitterInstanceVector[theIdx];
			}
			return null;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001CFF4 File Offset: 0x0001B1F4
		public PIEmitterInstance GetEmitter(string theName)
		{
			for (int i = 0; i < this.mEmitterInstanceVector.Count; i++)
			{
				if (theName.Length == 0 || this.mEmitterInstanceVector[i].mEmitterInstanceDef.mName == theName)
				{
					return this.mEmitterInstanceVector[i];
				}
			}
			return null;
		}

		// Token: 0x04000670 RID: 1648
		public PILayerDef mLayerDef;

		// Token: 0x04000671 RID: 1649
		public List<PIEmitterInstance> mEmitterInstanceVector = new List<PIEmitterInstance>();

		// Token: 0x04000672 RID: 1650
		public bool mVisible;

		// Token: 0x04000673 RID: 1651
		public SexyColor mColor = default(SexyColor);

		// Token: 0x04000674 RID: 1652
		public DeviceImage mBkgImage;

		// Token: 0x04000675 RID: 1653
		public Vector2 mBkgImgDrawOfs = default(Vector2);

		// Token: 0x04000676 RID: 1654
		public SexyTransform2D mBkgTransform = new SexyTransform2D(false);
	}
}
