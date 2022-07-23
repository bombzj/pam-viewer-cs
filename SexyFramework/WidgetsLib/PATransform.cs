using System;
using SexyFramework.Misc;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001B9 RID: 441
	public class PATransform
	{
		// Token: 0x0600103E RID: 4158 RVA: 0x0004D6C8 File Offset: 0x0004B8C8
		public PATransform Clone()
		{
			PATransform patransform = new PATransform();
			this.mMatrix.CopyTo(patransform.mMatrix);
			return patransform;
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0004D6ED File Offset: 0x0004B8ED
		public PATransform()
		{
			this.mMatrix.LoadIdentity();
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0004D70C File Offset: 0x0004B90C
		public void CopyFrom(PATransform rhs)
		{
			this.mMatrix.CopyFrom(rhs.mMatrix);
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x0004D724 File Offset: 0x0004B924
		public void TransformSrc(PATransform theSrcTransform, ref PATransform outTran)
		{
			outTran.mMatrix.CopyFrom(this.mMatrix * theSrcTransform.mMatrix);
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0004D748 File Offset: 0x0004B948
		public void InterpolateTo(PATransform theNextTransform, float thePct, ref PATransform outTran)
		{
			outTran.mMatrix.mMatrix = this.mMatrix.mMatrix * (1f - thePct) + theNextTransform.mMatrix.mMatrix * thePct;
		}

		// Token: 0x04000CF0 RID: 3312
		public SexyTransform2D mMatrix = new SexyTransform2D(false);
	}
}
