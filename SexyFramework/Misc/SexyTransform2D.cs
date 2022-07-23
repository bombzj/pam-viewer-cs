using System;
using Microsoft.Xna.Framework;

namespace SexyFramework.Misc
{
	// Token: 0x0200014D RID: 333
	public struct SexyTransform2D : SexyMatrix3
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000B62 RID: 2914 RVA: 0x000385CB File Offset: 0x000367CB
		// (set) Token: 0x06000B61 RID: 2913 RVA: 0x000385BD File Offset: 0x000367BD
		public float m00
		{
			get
			{
				return this.mMatrix.M11;
			}
			set
			{
				this.mMatrix.M11 = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x000385E6 File Offset: 0x000367E6
		// (set) Token: 0x06000B63 RID: 2915 RVA: 0x000385D8 File Offset: 0x000367D8
		public float m01
		{
			get
			{
				return this.mMatrix.M21;
			}
			set
			{
				this.mMatrix.M21 = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000B66 RID: 2918 RVA: 0x00038601 File Offset: 0x00036801
		// (set) Token: 0x06000B65 RID: 2917 RVA: 0x000385F3 File Offset: 0x000367F3
		public float m02
		{
			get
			{
				return this.mMatrix.M41;
			}
			set
			{
				this.mMatrix.M41 = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000B68 RID: 2920 RVA: 0x0003861C File Offset: 0x0003681C
		// (set) Token: 0x06000B67 RID: 2919 RVA: 0x0003860E File Offset: 0x0003680E
		public float m10
		{
			get
			{
				return this.mMatrix.M12;
			}
			set
			{
				this.mMatrix.M12 = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x00038637 File Offset: 0x00036837
		// (set) Token: 0x06000B69 RID: 2921 RVA: 0x00038629 File Offset: 0x00036829
		public float m11
		{
			get
			{
				return this.mMatrix.M22;
			}
			set
			{
				this.mMatrix.M22 = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x00038652 File Offset: 0x00036852
		// (set) Token: 0x06000B6B RID: 2923 RVA: 0x00038644 File Offset: 0x00036844
		public float m12
		{
			get
			{
				return this.mMatrix.M42;
			}
			set
			{
				this.mMatrix.M42 = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000B6E RID: 2926 RVA: 0x0003866D File Offset: 0x0003686D
		// (set) Token: 0x06000B6D RID: 2925 RVA: 0x0003865F File Offset: 0x0003685F
		public float m20
		{
			get
			{
				return this.mMatrix.M13;
			}
			set
			{
				this.mMatrix.M13 = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000B70 RID: 2928 RVA: 0x00038688 File Offset: 0x00036888
		// (set) Token: 0x06000B6F RID: 2927 RVA: 0x0003867A File Offset: 0x0003687A
		public float m21
		{
			get
			{
				return this.mMatrix.M23;
			}
			set
			{
				this.mMatrix.M23 = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x000386A3 File Offset: 0x000368A3
		// (set) Token: 0x06000B71 RID: 2929 RVA: 0x00038695 File Offset: 0x00036895
		public float m22
		{
			get
			{
				return this.mMatrix.M33;
			}
			set
			{
				this.mMatrix.M33 = value;
			}
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x000386B0 File Offset: 0x000368B0
		public SexyTransform2D(bool init)
		{
			this.mMatrix = Matrix.Identity;
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x000386BD File Offset: 0x000368BD
		public SexyTransform2D(Matrix mat)
		{
			this.mMatrix = mat;
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x000386C6 File Offset: 0x000368C6
		public void Swap(SexyTransform2D lhs)
		{
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x000386C8 File Offset: 0x000368C8
		public void CopyTo(SexyTransform2D lhs)
		{
			lhs.mMatrix = this.mMatrix;
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x000386D7 File Offset: 0x000368D7
		public SexyTransform2D(SexyMatrix3 rhs)
		{
			this.mMatrix = ((SexyTransform2D)rhs).mMatrix;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x000386EC File Offset: 0x000368EC
		public SexyTransform2D(float in00, float in01, float in02, float in10, float in11, float in12, float in20, float in21, float in22)
		{
			this.mMatrix = new Matrix(in00, in10, in20, 0f, in01, in11, in21, 0f, 0f, 0f, in22, 0f, in02, in12, 0f, 1f);
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00038738 File Offset: 0x00036938
		public void ZeroMatrix()
		{
			this.m00 = (this.m01 = (this.m02 = (this.m10 = (this.m11 = (this.m12 = (this.m20 = (this.m21 = (this.m22 = 0f))))))));
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x000387A0 File Offset: 0x000369A0
		public void LoadIdentity()
		{
			this.mMatrix = Matrix.Identity;
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x000387AD File Offset: 0x000369AD
		public void CopyFrom(SexyMatrix3 theMatrix)
		{
			this.mMatrix = ((SexyTransform2D)theMatrix).mMatrix;
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x000387C0 File Offset: 0x000369C0
		public static SexyVector2 operator *(SexyTransform2D ImpliedObject, SexyVector2 theVec)
		{
			return new SexyVector2(false)
			{
				mVector = Vector2.Transform(theVec.mVector, ImpliedObject.mMatrix)
			};
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x000387F0 File Offset: 0x000369F0
		public static Vector2 operator *(SexyTransform2D ImpliedObject, Vector2 theVec)
		{
			return Vector2.Transform(theVec, ImpliedObject.mMatrix);
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0003880C File Offset: 0x00036A0C
		public static SexyVector3 operator *(SexyTransform2D ImpliedObject, SexyVector3 theVec)
		{
			return new SexyVector3
			{
				mVector = Vector3.Transform(theVec.mVector, ImpliedObject.mMatrix)
			};
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0003883C File Offset: 0x00036A3C
		public void MulSelf(SexyMatrix3 theMat)
		{
			this.mMatrix *= ((SexyTransform2D)theMat).mMatrix;
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0003885C File Offset: 0x00036A5C
		public static SexyMatrix3 operator *(SexyTransform2D ImpliedObject, SexyMatrix3 theMat)
		{
			return new SexyTransform2D(false)
			{
				mMatrix = ((SexyTransform2D)theMat).mMatrix * ImpliedObject.mMatrix
			};
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x00038895 File Offset: 0x00036A95
		public static void Multiply(ref SexyTransform2D pOut, SexyMatrix3 pM1, SexyMatrix3 pM2)
		{
			pOut.mMatrix = ((SexyTransform2D)pM2).mMatrix * ((SexyTransform2D)pM1).mMatrix;
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x000388B8 File Offset: 0x00036AB8
		public void Translate(float tx, float ty)
		{
			this.mMatrix.M41 = this.mMatrix.M41 + tx;
			this.mMatrix.M42 = this.mMatrix.M42 + ty;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x000388E0 File Offset: 0x00036AE0
		public void RotateRad(float rot)
		{
			this.mMatrix = Matrix.Multiply(this.mMatrix, Matrix.CreateRotationZ(-rot));
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x000388FA File Offset: 0x00036AFA
		public void RotateDeg(float rot)
		{
			this.RotateRad(MathHelper.ToRadians(rot));
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x00038908 File Offset: 0x00036B08
		public void Scale(float sx, float sy)
		{
			this.mMatrix.M11 = this.mMatrix.M11 * sx;
			this.mMatrix.M21 = this.mMatrix.M21 * sx;
			this.mMatrix.M41 = this.mMatrix.M41 * sx;
			this.mMatrix.M22 = this.mMatrix.M22 * sy;
			this.mMatrix.M12 = this.mMatrix.M12 * sy;
			this.mMatrix.M42 = this.mMatrix.M42 * sy;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x00038988 File Offset: 0x00036B88
		public void SkewRad(float sx, float sy)
		{
			SexyTransform2D impliedObject = new SexyTransform2D(false);
			impliedObject.LoadIdentity();
			impliedObject.m01 = (float)Math.Tan((double)sx);
			impliedObject.m02 = (float)Math.Tan((double)sy);
			(impliedObject * this).Swap(this);
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x000389DC File Offset: 0x00036BDC
		public static SexyTransform2D operator *(SexyTransform2D ImpliedObject, SexyTransform2D theMat)
		{
			SexyTransform2D result = new SexyTransform2D(theMat.mMatrix * ImpliedObject.mMatrix);
			return result;
		}

		// Token: 0x0400094F RID: 2383
		public Matrix mMatrix;

		// Token: 0x04000950 RID: 2384
		public static SexyTransform2D DefaultMatrix = new SexyTransform2D(false);
	}
}
