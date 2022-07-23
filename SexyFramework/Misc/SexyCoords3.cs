using System;

namespace SexyFramework.Misc
{
	// Token: 0x02000142 RID: 322
	public class SexyCoords3
	{
		// Token: 0x06000AD9 RID: 2777 RVA: 0x00036A68 File Offset: 0x00034C68
		public SexyCoords3()
		{
			this.t = new SexyVector3(0f, 0f, 0f);
			this.r = new SexyAxes3();
			this.s = new SexyVector3(1f, 1f, 1f);
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x00036AE0 File Offset: 0x00034CE0
		public SexyCoords3(SexyCoords3 inC)
		{
			this.t = inC.t;
			this.r = new SexyAxes3(inC.r);
			this.s = inC.s;
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x00036B40 File Offset: 0x00034D40
		public SexyCoords3(SexyAxes3 inR)
		{
			this.t = new SexyVector3(0f, 0f, 0f);
			this.r = new SexyAxes3(inR);
			this.s = new SexyVector3(1f, 1f, 1f);
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x00036BB8 File Offset: 0x00034DB8
		public SexyCoords3(SexyVector3 inT, SexyAxes3 inR, SexyVector3 inS)
		{
			this.t = inT;
			this.r = new SexyAxes3(inR);
			this.s = inS;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x00036C08 File Offset: 0x00034E08
		public SexyCoords3 CopyFrom(SexyCoords3 inC)
		{
			this.t = inC.t;
			this.r = inC.r;
			this.s = inC.s;
			return this;
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x00036C2F File Offset: 0x00034E2F
		public SexyCoords3 Enter(SexyCoords3 inCoords)
		{
			return new SexyCoords3(this.t.Enter(inCoords), this.r.Enter(inCoords.r), this.s / inCoords.s);
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x00036C64 File Offset: 0x00034E64
		public SexyCoords3 Leave(SexyCoords3 inCoords)
		{
			return new SexyCoords3(this.t.Leave(inCoords), this.r.Leave(inCoords.r), this.s * inCoords.s);
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x00036C99 File Offset: 0x00034E99
		public SexyCoords3 Inverse()
		{
			return new SexyCoords3().Enter(this);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00036CA6 File Offset: 0x00034EA6
		public SexyCoords3 DeltaTo(SexyCoords3 inCoords)
		{
			return inCoords.Inverse().Leave(this);
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x00036CB4 File Offset: 0x00034EB4
		public void Translate(float inX, float inY, float inZ)
		{
			this.t += new SexyVector3(inX, inY, inZ);
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00036CCF File Offset: 0x00034ECF
		public void RotateRadAxis(float inRot, SexyVector3 inNormalizedAxis)
		{
			this.r.RotateRadAxis(inRot, inNormalizedAxis);
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00036CDE File Offset: 0x00034EDE
		public void RotateRadX(float inRot)
		{
			this.r.RotateRadX(inRot);
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00036CEC File Offset: 0x00034EEC
		public void RotateRadY(float inRot)
		{
			this.r.RotateRadY(inRot);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00036CFA File Offset: 0x00034EFA
		public void RotateRadZ(float inRot)
		{
			this.r.RotateRadZ(inRot);
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00036D08 File Offset: 0x00034F08
		public void Scale(float inX, float inY, float inZ)
		{
			this.s *= new SexyVector3(inX, inY, inZ);
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00036D24 File Offset: 0x00034F24
		public bool LookAt(SexyVector3 inTargetPos, SexyVector3 inUpVector)
		{
			SexyVector3 sexyVector = this.t - inTargetPos;
			if (sexyVector.ApproxZero())
			{
				return false;
			}
			sexyVector = sexyVector.Normalize();
			if (SexyMath.Fabs(inUpVector.Dot(sexyVector)) > 1f - GlobalMembers.SEXYMATH_EPSILON)
			{
				return false;
			}
			this.r.vZ = sexyVector;
			this.r.vX = inUpVector.Cross(this.r.vZ).Normalize();
			this.r.vY = this.r.vZ.Cross(this.r.vX).Normalize();
			return true;
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x00036DCD File Offset: 0x00034FCD
		public bool LookAt(SexyVector3 inViewPos, SexyVector3 inTargetPos, SexyVector3 inUpVector)
		{
			this.t = inViewPos;
			return this.LookAt(inTargetPos, inUpVector);
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x00036DE0 File Offset: 0x00034FE0
		public void GetInboundMatrix(SexyMatrix4 outM)
		{
			if (outM == null)
			{
				return;
			}
			SexyVector3 sexyVector = new SexyVector3(-this.t);
			SexyVector3 v = new SexyVector3(this.r.vX / this.s.x);
			SexyVector3 v2 = new SexyVector3(this.r.vY / this.s.y);
			SexyVector3 v3 = new SexyVector3(this.r.vZ / this.s.z);
			outM.m[0, 0] = v.x;
			outM.m[0, 1] = v2.x;
			outM.m[0, 2] = v3.x;
			outM.m[0, 3] = 0f;
			outM.m[1, 0] = v.y;
			outM.m[1, 1] = v2.y;
			outM.m[1, 2] = v3.y;
			outM.m[1, 3] = 0f;
			outM.m[2, 0] = v.z;
			outM.m[2, 1] = v2.z;
			outM.m[2, 2] = v3.z;
			outM.m[2, 3] = 0f;
			outM.m[3, 0] = sexyVector.Dot(v);
			outM.m[3, 1] = sexyVector.Dot(v2);
			outM.m[3, 2] = sexyVector.Dot(v3);
			outM.m[3, 3] = 1f;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00036FA4 File Offset: 0x000351A4
		public void GetOutboundMatrix(SexyMatrix4 outM)
		{
			if (outM == null)
			{
				return;
			}
			outM.m[0, 0] = this.r.vX.x * this.s.x;
			outM.m[0, 1] = this.r.vX.y * this.s.x;
			outM.m[0, 2] = this.r.vX.z * this.s.x;
			outM.m[0, 3] = 0f;
			outM.m[1, 0] = this.r.vY.x * this.s.y;
			outM.m[1, 1] = this.r.vY.y * this.s.y;
			outM.m[1, 2] = this.r.vY.z * this.s.y;
			outM.m[1, 3] = 0f;
			outM.m[2, 0] = this.r.vZ.x * this.s.z;
			outM.m[2, 1] = this.r.vZ.y * this.s.z;
			outM.m[2, 2] = this.r.vZ.z * this.s.z;
			outM.m[2, 3] = 0f;
			outM.m[3, 0] = this.t.x;
			outM.m[3, 1] = this.t.y;
			outM.m[3, 2] = this.t.z;
			outM.m[3, 3] = 1f;
		}

		// Token: 0x04000939 RID: 2361
		public SexyVector3 t = default(SexyVector3);

		// Token: 0x0400093A RID: 2362
		public SexyAxes3 r = new SexyAxes3();

		// Token: 0x0400093B RID: 2363
		public SexyVector3 s = default(SexyVector3);
	}
}
