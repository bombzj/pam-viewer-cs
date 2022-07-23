using System;
using Microsoft.Xna.Framework;

namespace SexyFramework.Misc
{
	// Token: 0x0200014E RID: 334
	public class Transform
	{
		// Token: 0x06000B89 RID: 2953 RVA: 0x00038A11 File Offset: 0x00036C11
		protected void MakeComplex()
		{
			if (!this.mComplex)
			{
				this.mComplex = true;
				this.CalcMatrix();
			}
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x00038A28 File Offset: 0x00036C28
		protected void CalcMatrix()
		{
			if (this.mNeedCalcMatrix)
			{
				this.mNeedCalcMatrix = false;
				this.mTransForm.LoadIdentity();
				this.mTransForm.Translate(this.mTransX1, this.mTransX2);
				this.mTransForm.m02 = this.mTransX1;
				this.mTransForm.m12 = this.mTransY1;
				this.mTransForm.m22 = 1f;
				if (this.mHaveScale)
				{
					this.mTransForm.m00 = this.mScaleX;
					this.mTransForm.m11 = this.mScaleY;
				}
				else if (this.mHaveRot)
				{
					this.mTransForm.RotateRad(this.mRot);
				}
				if (this.mTransX2 != 0f || this.mTransY2 != 0f)
				{
					this.mTransForm.Translate(this.mTransX2, this.mTransY2);
				}
			}
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00038B11 File Offset: 0x00036D11
		public Transform()
		{
			this.Reset();
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00038B2C File Offset: 0x00036D2C
		public void Reset()
		{
			this.mNeedCalcMatrix = true;
			this.mComplex = false;
			this.mTransX1 = (this.mTransY1 = 0f);
			this.mTransX2 = (this.mTransY2 = 0f);
			this.mScaleX = (this.mScaleY = 1f);
			this.mRot = 0f;
			this.mHaveRot = false;
			this.mHaveScale = false;
			this.mTransForm.LoadIdentity();
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00038BA8 File Offset: 0x00036DA8
		public void Translate(float tx, float ty)
		{
			if (this.mComplex)
			{
				this.mTransForm.Translate(tx, ty);
				return;
			}
			this.mNeedCalcMatrix = true;
			if (this.mHaveRot || this.mHaveScale)
			{
				this.mTransX2 += tx;
				this.mTransY2 += ty;
				return;
			}
			this.mTransX1 += tx;
			this.mTransY1 += ty;
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x00038C1C File Offset: 0x00036E1C
		public void RotateRad(float rot)
		{
			if (this.mComplex)
			{
				this.mTransForm.RotateRad(rot);
				return;
			}
			if (this.mHaveScale)
			{
				this.MakeComplex();
				this.mTransForm.RotateRad(rot);
				return;
			}
			this.mNeedCalcMatrix = true;
			this.mHaveRot = true;
			this.mRot += rot;
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x00038C75 File Offset: 0x00036E75
		public void RotateDeg(float rot)
		{
			this.RotateRad(MathHelper.ToRadians(rot));
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00038C84 File Offset: 0x00036E84
		public void Scale(float sx, float sy)
		{
			if (this.mComplex)
			{
				this.mTransForm.Scale(sx, sy);
				return;
			}
			if (this.mHaveRot || this.mTransX1 != 0f || this.mTransY1 != 0f || (sx < 0f && this.mScaleX * sx != -1f) || sy < 0f || ((this.mTransX2 != 0f || this.mTransY2 != 0f) && sx != sy))
			{
				this.MakeComplex();
				this.mTransForm.Scale(sx, sy);
				return;
			}
			this.mNeedCalcMatrix = true;
			this.mHaveScale = true;
			this.mScaleX *= sx;
			this.mScaleY *= sy;
			this.mTransX2 *= sx;
			this.mTransY2 *= sy;
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00038D63 File Offset: 0x00036F63
		public SexyTransform2D GetMatrix()
		{
			this.CalcMatrix();
			return this.mTransForm;
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00038D71 File Offset: 0x00036F71
		public void SetMatrix(SexyTransform2D mat)
		{
			this.mTransForm.mMatrix = mat.mMatrix;
			this.mNeedCalcMatrix = false;
			this.mComplex = true;
		}

		// Token: 0x04000951 RID: 2385
		protected SexyTransform2D mTransForm = new SexyTransform2D(false);

		// Token: 0x04000952 RID: 2386
		protected bool mNeedCalcMatrix;

		// Token: 0x04000953 RID: 2387
		public bool mComplex;

		// Token: 0x04000954 RID: 2388
		public bool mHaveRot;

		// Token: 0x04000955 RID: 2389
		public bool mHaveScale;

		// Token: 0x04000956 RID: 2390
		public float mTransX1;

		// Token: 0x04000957 RID: 2391
		public float mTransY1;

		// Token: 0x04000958 RID: 2392
		public float mTransX2;

		// Token: 0x04000959 RID: 2393
		public float mTransY2;

		// Token: 0x0400095A RID: 2394
		public float mScaleX;

		// Token: 0x0400095B RID: 2395
		public float mScaleY;

		// Token: 0x0400095C RID: 2396
		public float mRot;
	}
}
