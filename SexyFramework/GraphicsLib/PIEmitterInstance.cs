using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000EA RID: 234
	public class PIEmitterInstance : PIEmitterBase
	{
		// Token: 0x060006BB RID: 1723 RVA: 0x0001CB94 File Offset: 0x0001AD94
		public PIEmitterInstance()
		{
			this.mWasActive = false;
			this.mWithinLifeFrame = true;
			this.mSuperEmitterGroup.mIsSuperEmitter = true;
			this.mTransform.LoadIdentity();
			this.mNumberScale = 1f;
			this.mVisible = true;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0001CC23 File Offset: 0x0001AE23
		public void SetVisible(bool isVisible)
		{
			this.mVisible = isVisible;
		}

		// Token: 0x04000643 RID: 1603
		public PIEmitterInstanceDef mEmitterInstanceDef;

		// Token: 0x04000644 RID: 1604
		public bool mWasActive;

		// Token: 0x04000645 RID: 1605
		public bool mWithinLifeFrame;

		// Token: 0x04000646 RID: 1606
		public List<PIParticleDefInstance> mSuperEmitterParticleDefInstanceVector = new List<PIParticleDefInstance>();

		// Token: 0x04000647 RID: 1607
		public PIParticleGroup mSuperEmitterGroup = new PIParticleGroup();

		// Token: 0x04000648 RID: 1608
		public SexyColor mTintColor = default(SexyColor);

		// Token: 0x04000649 RID: 1609
		public SharedImageRef mMaskImage = new SharedImageRef();

		// Token: 0x0400064A RID: 1610
		public SexyTransform2D mTransform = new SexyTransform2D(false);

		// Token: 0x0400064B RID: 1611
		public Vector2 mOffset = default(Vector2);

		// Token: 0x0400064C RID: 1612
		public float mNumberScale;

		// Token: 0x0400064D RID: 1613
		public bool mVisible;
	}
}
