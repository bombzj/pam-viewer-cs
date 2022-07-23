using System;
using Microsoft.Xna.Framework;

namespace SexyFramework.Misc
{
	// Token: 0x0200014B RID: 331
	public class SexyMatrix4
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000B31 RID: 2865 RVA: 0x00037ED4 File Offset: 0x000360D4
		// (set) Token: 0x06000B30 RID: 2864 RVA: 0x00037EC4 File Offset: 0x000360C4
		public float m00
		{
			get
			{
				return this.m[0, 0];
			}
			set
			{
				this.m[0, 0] = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x00037EF3 File Offset: 0x000360F3
		// (set) Token: 0x06000B32 RID: 2866 RVA: 0x00037EE3 File Offset: 0x000360E3
		public float m01
		{
			get
			{
				return this.m[0, 1];
			}
			set
			{
				this.m[0, 1] = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x00037F12 File Offset: 0x00036112
		// (set) Token: 0x06000B34 RID: 2868 RVA: 0x00037F02 File Offset: 0x00036102
		public float m02
		{
			get
			{
				return this.m[0, 2];
			}
			set
			{
				this.m[0, 2] = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x00037F31 File Offset: 0x00036131
		// (set) Token: 0x06000B36 RID: 2870 RVA: 0x00037F21 File Offset: 0x00036121
		public float m03
		{
			get
			{
				return this.m[0, 3];
			}
			set
			{
				this.m[0, 3] = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000B39 RID: 2873 RVA: 0x00037F50 File Offset: 0x00036150
		// (set) Token: 0x06000B38 RID: 2872 RVA: 0x00037F40 File Offset: 0x00036140
		public float m10
		{
			get
			{
				return this.m[1, 0];
			}
			set
			{
				this.m[1, 0] = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000B3B RID: 2875 RVA: 0x00037F6F File Offset: 0x0003616F
		// (set) Token: 0x06000B3A RID: 2874 RVA: 0x00037F5F File Offset: 0x0003615F
		public float m11
		{
			get
			{
				return this.m[1, 1];
			}
			set
			{
				this.m[1, 1] = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x00037F8E File Offset: 0x0003618E
		// (set) Token: 0x06000B3C RID: 2876 RVA: 0x00037F7E File Offset: 0x0003617E
		public float m12
		{
			get
			{
				return this.m[1, 2];
			}
			set
			{
				this.m[1, 2] = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000B3F RID: 2879 RVA: 0x00037FAD File Offset: 0x000361AD
		// (set) Token: 0x06000B3E RID: 2878 RVA: 0x00037F9D File Offset: 0x0003619D
		public float m13
		{
			get
			{
				return this.m[1, 3];
			}
			set
			{
				this.m[1, 3] = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000B41 RID: 2881 RVA: 0x00037FCC File Offset: 0x000361CC
		// (set) Token: 0x06000B40 RID: 2880 RVA: 0x00037FBC File Offset: 0x000361BC
		public float m20
		{
			get
			{
				return this.m[2, 0];
			}
			set
			{
				this.m[2, 0] = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x00037FEB File Offset: 0x000361EB
		// (set) Token: 0x06000B42 RID: 2882 RVA: 0x00037FDB File Offset: 0x000361DB
		public float m21
		{
			get
			{
				return this.m[2, 1];
			}
			set
			{
				this.m[2, 1] = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x0003800A File Offset: 0x0003620A
		// (set) Token: 0x06000B44 RID: 2884 RVA: 0x00037FFA File Offset: 0x000361FA
		public float m22
		{
			get
			{
				return this.m[2, 2];
			}
			set
			{
				this.m[2, 2] = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000B47 RID: 2887 RVA: 0x00038029 File Offset: 0x00036229
		// (set) Token: 0x06000B46 RID: 2886 RVA: 0x00038019 File Offset: 0x00036219
		public float m23
		{
			get
			{
				return this.m[2, 3];
			}
			set
			{
				this.m[2, 3] = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x00038048 File Offset: 0x00036248
		// (set) Token: 0x06000B48 RID: 2888 RVA: 0x00038038 File Offset: 0x00036238
		public float m30
		{
			get
			{
				return this.m[3, 0];
			}
			set
			{
				this.m[3, 0] = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000B4B RID: 2891 RVA: 0x00038067 File Offset: 0x00036267
		// (set) Token: 0x06000B4A RID: 2890 RVA: 0x00038057 File Offset: 0x00036257
		public float m31
		{
			get
			{
				return this.m[3, 1];
			}
			set
			{
				this.m[3, 1] = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000B4D RID: 2893 RVA: 0x00038086 File Offset: 0x00036286
		// (set) Token: 0x06000B4C RID: 2892 RVA: 0x00038076 File Offset: 0x00036276
		public float m32
		{
			get
			{
				return this.m[3, 2];
			}
			set
			{
				this.m[3, 2] = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x000380A5 File Offset: 0x000362A5
		// (set) Token: 0x06000B4E RID: 2894 RVA: 0x00038095 File Offset: 0x00036295
		public float m33
		{
			get
			{
				return this.m[3, 3];
			}
			set
			{
				this.m[3, 3] = value;
			}
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x000380B4 File Offset: 0x000362B4
		public SexyMatrix4()
		{
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x000380CC File Offset: 0x000362CC
		public void Swap(SexyMatrix4 lhs)
		{
			float[,] array = lhs.m;
			lhs.m = this.m;
			this.m = array;
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x000380F4 File Offset: 0x000362F4
		public void CopyTo(SexyMatrix4 lhs)
		{
			for (int i = 0; i < 4; i++)
			{
				lhs.m[i, 0] = this.m[i, 0];
				lhs.m[i, 1] = this.m[i, 1];
				lhs.m[i, 2] = this.m[i, 2];
				lhs.m[i, 3] = this.m[i, 3];
			}
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x00038175 File Offset: 0x00036375
		public SexyMatrix4(SexyMatrix4 rhs)
		{
			rhs.CopyTo(this);
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x00038194 File Offset: 0x00036394
		public SexyMatrix4(float in00, float in01, float in02, float in03, float in10, float in11, float in12, float in13, float in20, float in21, float in22, float in23, float in30, float in31, float in32, float in33)
		{
			this.m00 = in00;
			this.m01 = in00;
			this.m02 = in02;
			this.m03 = in03;
			this.m10 = in10;
			this.m11 = in11;
			this.m12 = in12;
			this.m13 = in13;
			this.m20 = in20;
			this.m21 = in21;
			this.m22 = in22;
			this.m23 = in23;
			this.m30 = in30;
			this.m31 = in31;
			this.m32 = in32;
			this.m32 = in33;
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x00038234 File Offset: 0x00036434
		public void LoadIdentity()
		{
			this.m[0, 1] = (this.m[0, 2] = (this.m[0, 3] = (this.m[1, 0] = (this.m[1, 2] = (this.m[1, 3] = (this.m[2, 0] = (this.m[2, 1] = (this.m[2, 3] = (this.m[3, 0] = (this.m[3, 1] = (this.m[3, 2] = 0f)))))))))));
			this.m[0, 0] = (this.m[1, 1] = (this.m[2, 2] = (this.m[3, 3] = 1f)));
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0003835C File Offset: 0x0003655C
		public static SexyVector3 operator *(SexyMatrix4 ImpliedObject, SexyVector2 theVec)
		{
			SexyVector3 result = default(SexyVector3);
			Vector3 vector = new Vector3(theVec.mVector, 0f);
			result.mVector = Vector3.Transform(vector, ImpliedObject.mMatrix);
			return result;
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00038398 File Offset: 0x00036598
		public static SexyVector3 operator *(SexyMatrix4 ImpliedObject, SexyVector3 theVec)
		{
			return new SexyVector3
			{
				mVector = Vector3.Transform(theVec.mVector, ImpliedObject.mMatrix)
			};
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x000383C8 File Offset: 0x000365C8
		public static SexyMatrix4 operator *(SexyMatrix4 ImpliedObject, SexyMatrix4 theMat)
		{
			SexyMatrix4 sexyMatrix = new SexyMatrix4();
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					float num = 0f;
					for (int k = 0; k < 4; k++)
					{
						num += ImpliedObject.m[i, k] * theMat.m[k, j];
					}
					sexyMatrix.m[i, j] = num;
				}
			}
			return sexyMatrix;
		}

		// Token: 0x0400094D RID: 2381
		public Matrix mMatrix;

		// Token: 0x0400094E RID: 2382
		public float[,] m = new float[4, 4];
	}
}
