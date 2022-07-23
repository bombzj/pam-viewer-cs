using System;
using System.Collections.Generic;
using SexyFramework.GraphicsLib;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001C2 RID: 450
	public class PASpriteInst : IDisposable
	{
		// Token: 0x0600104E RID: 4174 RVA: 0x0004D988 File Offset: 0x0004BB88
		public virtual void Dispose()
		{
			for (int i = 0; i < this.mChildren.Count; i++)
			{
				if (this.mChildren[i].mSpriteInst != null)
				{
					this.mChildren[i].mSpriteInst.Dispose();
				}
			}
			while (this.mParticleEffectVector.Count > 0)
			{
				if (this.mParticleEffectVector[this.mParticleEffectVector.Count - 1].mEffect != null)
				{
					this.mParticleEffectVector[this.mParticleEffectVector.Count - 1].mEffect.Dispose();
				}
				this.mParticleEffectVector.RemoveAt(this.mParticleEffectVector.Count - 1);
			}
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0004DA40 File Offset: 0x0004BC40
		public PAObjectInst GetObjectInst(string theName)
		{
			string theName2 = "";
			int num = theName.IndexOf('\\');
			string text;
			if (num != -1)
			{
				text = theName.Substring(0, num);
				theName2 = theName.Substring(num + 1);
			}
			else
			{
				text = theName;
			}
			int i = 0;
			while (i < this.mChildren.Count)
			{
				PAObjectInst paobjectInst = this.mChildren[i];
				if (paobjectInst.mName != null && paobjectInst.mName == text)
				{
					if (num == -1)
					{
						return paobjectInst;
					}
					if (paobjectInst.mSpriteInst == null)
					{
						return null;
					}
					return paobjectInst.mSpriteInst.GetObjectInst(theName2);
				}
				else
				{
					i++;
				}
			}
			return null;
		}

		// Token: 0x04000D26 RID: 3366
		public PASpriteInst mParent;

		// Token: 0x04000D27 RID: 3367
		public int mDelayFrames;

		// Token: 0x04000D28 RID: 3368
		public float mFrameNum;

		// Token: 0x04000D29 RID: 3369
		public int mFrameRepeats;

		// Token: 0x04000D2A RID: 3370
		public bool mOnNewFrame;

		// Token: 0x04000D2B RID: 3371
		public int mLastUpdated;

		// Token: 0x04000D2C RID: 3372
		public PATransform mCurTransform = new PATransform();

		// Token: 0x04000D2D RID: 3373
		public SexyColor mCurColor = default(SexyColor);

		// Token: 0x04000D2E RID: 3374
		public List<PAObjectInst> mChildren = new List<PAObjectInst>();

		// Token: 0x04000D2F RID: 3375
		public PASpriteDef mDef;

		// Token: 0x04000D30 RID: 3376
		public List<PAParticleEffect> mParticleEffectVector = new List<PAParticleEffect>();
	}
}
