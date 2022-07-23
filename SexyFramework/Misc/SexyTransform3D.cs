using System;

namespace SexyFramework.Misc
{
	// Token: 0x0200014C RID: 332
	public class SexyTransform3D : SexyMatrix4
	{
		// Token: 0x06000B59 RID: 2905 RVA: 0x00038438 File Offset: 0x00036638
		public SexyTransform3D()
		{
			base.LoadIdentity();
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00038446 File Offset: 0x00036646
		private SexyTransform3D(bool loadIdentity)
		{
			if (loadIdentity)
			{
				base.LoadIdentity();
			}
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00038457 File Offset: 0x00036657
		private SexyTransform3D(SexyMatrix4 theMatrix)
			: base(theMatrix)
		{
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00038460 File Offset: 0x00036660
		private void Translate(float tx, float ty, float tz)
		{
			base.m30 += tx;
			base.m31 += ty;
			base.m32 += tz;
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x0003848C File Offset: 0x0003668C
		private void RotateRadX(float rot)
		{
			float num = (float)Math.Sin((double)rot);
			float num2 = (float)Math.Cos((double)rot);
			SexyMatrix4 sexyMatrix = new SexyMatrix4();
			sexyMatrix.LoadIdentity();
			sexyMatrix.m11 = num2;
			sexyMatrix.m21 = -num;
			sexyMatrix.m12 = num;
			sexyMatrix.m22 = num2;
			(this * sexyMatrix).Swap(this);
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x000384E4 File Offset: 0x000366E4
		private void RotateRadY(float rot)
		{
			float num = (float)Math.Sin((double)rot);
			float num2 = (float)Math.Cos((double)rot);
			SexyMatrix4 sexyMatrix = new SexyMatrix4();
			sexyMatrix.LoadIdentity();
			sexyMatrix.m00 = num2;
			sexyMatrix.m02 = -num;
			sexyMatrix.m20 = num;
			sexyMatrix.m22 = num2;
			(this * sexyMatrix).Swap(this);
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x0003853C File Offset: 0x0003673C
		private void RotateRadZ(float rot)
		{
			float num = (float)Math.Sin((double)rot);
			float num2 = (float)Math.Cos((double)rot);
			SexyMatrix4 sexyMatrix = new SexyMatrix4();
			sexyMatrix.LoadIdentity();
			sexyMatrix.m00 = num2;
			sexyMatrix.m01 = num;
			sexyMatrix.m10 = -num;
			sexyMatrix.m11 = num2;
			(this * sexyMatrix).Swap(this);
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00038591 File Offset: 0x00036791
		private void Scale(float sx, float sy, float sz)
		{
			base.m00 *= sx;
			base.m11 *= sy;
			base.m22 *= sz;
		}
	}
}
