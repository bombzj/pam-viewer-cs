using System;

namespace SexyFramework.Misc
{
	// Token: 0x02000141 RID: 321
	public class SexyAxes3
	{
		// Token: 0x06000AC7 RID: 2759 RVA: 0x000365D0 File Offset: 0x000347D0
		public SexyAxes3()
		{
			this.vX = new SexyVector3(1f, 0f, 0f);
			this.vY = new SexyVector3(0f, 1f, 0f);
			this.vZ = new SexyVector3(0f, 0f, 1f);
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x00036658 File Offset: 0x00034858
		public SexyAxes3(SexyAxes3 inA)
		{
			this.vX = inA.vX;
			this.vY = inA.vY;
			this.vZ = inA.vZ;
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x000366B4 File Offset: 0x000348B4
		public SexyAxes3(SexyVector3 inX, SexyVector3 inY, SexyVector3 inZ)
		{
			this.vX = inX;
			this.vY = inY;
			this.vZ = inZ;
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00036700 File Offset: 0x00034900
		public void CopyFrom(SexyAxes3 inA)
		{
			this.vX = inA.vX;
			this.vY = inA.vY;
			this.vZ = inA.vZ;
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x00036726 File Offset: 0x00034926
		public SexyAxes3 Enter(SexyAxes3 inAxes)
		{
			return new SexyAxes3(this.vX.Enter(inAxes), this.vY.Enter(inAxes), this.vZ.Enter(inAxes));
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x00036751 File Offset: 0x00034951
		public SexyAxes3 Leave(SexyAxes3 inAxes)
		{
			return new SexyAxes3(this.vX.Leave(inAxes), this.vY.Leave(inAxes), this.vZ.Leave(inAxes));
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0003677C File Offset: 0x0003497C
		private void EndterSelf(SexyAxes3 inAxes)
		{
			this.vX = this.vX.Enter(inAxes);
			this.vY = this.vX.Enter(inAxes);
			this.vZ = this.vZ.Enter(inAxes);
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x000367B4 File Offset: 0x000349B4
		private void LeaveSelf(SexyAxes3 inAxes)
		{
			this.vX = this.vX.Leave(inAxes);
			this.vY = this.vX.Leave(inAxes);
			this.vZ = this.vZ.Leave(inAxes);
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x000367EC File Offset: 0x000349EC
		public SexyAxes3 Inverse()
		{
			return new SexyAxes3().Enter(this);
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x000367FC File Offset: 0x000349FC
		public SexyAxes3 OrthoNormalize()
		{
			SexyAxes3 sexyAxes = new SexyAxes3(this);
			sexyAxes.vX = sexyAxes.vY.Cross(sexyAxes.vZ).Normalize();
			sexyAxes.vY = sexyAxes.vZ.Cross(sexyAxes.vX).Normalize();
			sexyAxes.vZ = sexyAxes.vX.Cross(sexyAxes.vY).Normalize();
			return sexyAxes;
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0003686E File Offset: 0x00034A6E
		public SexyAxes3 DeltaTo(SexyAxes3 inAxes)
		{
			return inAxes.Inverse().Leave(this);
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0003687C File Offset: 0x00034A7C
		public SexyAxes3 SlerpTo(SexyAxes3 inAxes, float inAlpha)
		{
			return this.SlerpTo(inAxes, inAlpha, false);
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00036887 File Offset: 0x00034A87
		public SexyAxes3 SlerpTo(SexyAxes3 inAxes, float inAlpha, bool inFastButLessAccurate)
		{
			return SexyQuat3.Slerp(new SexyQuat3(this), new SexyQuat3(inAxes), inAlpha, inFastButLessAccurate);
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x000368A4 File Offset: 0x00034AA4
		public void RotateRadAxis(float inRot, SexyVector3 inNormalizedAxis)
		{
			SexyAxes3 inAxes = SexyQuat3.AxisAngle(inNormalizedAxis, inRot);
			this.LeaveSelf(inAxes);
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x000368C8 File Offset: 0x00034AC8
		public void RotateRadX(float inRot)
		{
			double num = Math.Sin((double)inRot);
			double num2 = Math.Cos((double)inRot);
			SexyAxes3 sexyAxes = new SexyAxes3();
			sexyAxes.vY.y = (float)num2;
			sexyAxes.vZ.y = (float)(-(float)num);
			sexyAxes.vY.z = (float)num;
			sexyAxes.vZ.z = (float)num2;
			this.LeaveSelf(sexyAxes);
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x00036928 File Offset: 0x00034B28
		public void RotateRadY(float inRot)
		{
			double num = Math.Sin((double)inRot);
			double num2 = Math.Cos((double)inRot);
			SexyAxes3 sexyAxes = new SexyAxes3();
			sexyAxes.vX.x = (float)num2;
			sexyAxes.vX.z = (float)(-(float)num);
			sexyAxes.vZ.x = (float)num;
			sexyAxes.vZ.z = (float)num2;
			this.LeaveSelf(sexyAxes);
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x00036988 File Offset: 0x00034B88
		public void RotateRadZ(float inRot)
		{
			double num = Math.Sin((double)inRot);
			double num2 = Math.Cos((double)inRot);
			SexyAxes3 sexyAxes = new SexyAxes3();
			sexyAxes.vX.x = (float)num2;
			sexyAxes.vX.y = (float)num;
			sexyAxes.vY.x = (float)(-(float)num);
			sexyAxes.vY.y = (float)num2;
			this.LeaveSelf(sexyAxes);
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x000369E8 File Offset: 0x00034BE8
		public void LookAt(SexyVector3 inTargetDir, SexyVector3 inUpVector)
		{
			SexyVector3 v = inTargetDir.Normalize();
			if (SexyMath.Fabs(inUpVector.Dot(v)) > 1f - GlobalMembers.SEXYMATH_EPSILON)
			{
				return;
			}
			SexyAxes3 sexyAxes = new SexyAxes3();
			sexyAxes.vZ = v;
			sexyAxes.vX = inUpVector.Cross(sexyAxes.vZ).Normalize();
			sexyAxes.vY = sexyAxes.vZ.Cross(sexyAxes.vX).Normalize();
			this.LeaveSelf(sexyAxes);
		}

		// Token: 0x04000936 RID: 2358
		public SexyVector3 vX = default(SexyVector3);

		// Token: 0x04000937 RID: 2359
		public SexyVector3 vY = default(SexyVector3);

		// Token: 0x04000938 RID: 2360
		public SexyVector3 vZ = default(SexyVector3);
	}
}
