using System;
using Microsoft.Xna.Framework;

namespace SexyFramework.Misc
{
	// Token: 0x0200014F RID: 335
	public struct SexyVector2
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x00038D93 File Offset: 0x00036F93
		// (set) Token: 0x06000B94 RID: 2964 RVA: 0x00038DA0 File Offset: 0x00036FA0
		public float x
		{
			get
			{
				return this.mVector.X;
			}
			set
			{
				this.mVector.X = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x00038DAE File Offset: 0x00036FAE
		// (set) Token: 0x06000B96 RID: 2966 RVA: 0x00038DBB File Offset: 0x00036FBB
		public float y
		{
			get
			{
				return this.mVector.Y;
			}
			set
			{
				this.mVector.Y = value;
			}
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x00038DC9 File Offset: 0x00036FC9
		public SexyVector2(bool init)
		{
			this.mVector = new Vector2(0f, 0f);
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00038DE0 File Offset: 0x00036FE0
		public SexyVector2(float theX, float theY)
		{
			this.mVector = new Vector2(theX, theY);
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x00038DEF File Offset: 0x00036FEF
		public SexyVector2(Vector2 v)
		{
			this.mVector = v;
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x00038DF8 File Offset: 0x00036FF8
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x00038E0B File Offset: 0x0003700B
		public bool Equals(SexyVector2 obj)
		{
			return this.mVector == obj.mVector;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x00038E1F File Offset: 0x0003701F
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00038E31 File Offset: 0x00037031
		public float Dot(SexyVector2 v)
		{
			return Vector2.Dot(this.mVector, v.mVector);
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00038E45 File Offset: 0x00037045
		public static SexyVector2 operator +(SexyVector2 ImpliedObject, SexyVector2 v)
		{
			return new SexyVector2(ImpliedObject.mVector + v.mVector);
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x00038E5F File Offset: 0x0003705F
		public static SexyVector2 operator -(SexyVector2 ImpliedObject, SexyVector2 v)
		{
			return new SexyVector2(ImpliedObject.mVector - v.mVector);
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x00038E79 File Offset: 0x00037079
		public static SexyVector2 operator -(SexyVector2 ImpliedObject)
		{
			return new SexyVector2(-ImpliedObject.mVector);
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x00038E8C File Offset: 0x0003708C
		public static SexyVector2 operator *(SexyVector2 ImpliedObject, float t)
		{
			return new SexyVector2(ImpliedObject.mVector * t);
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x00038EA0 File Offset: 0x000370A0
		public static SexyVector2 operator /(SexyVector2 ImpliedObject, float t)
		{
			return new SexyVector2(ImpliedObject.x / t, ImpliedObject.y / t);
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x00038EB9 File Offset: 0x000370B9
		public SexyVector2 AddSelf(SexyVector2 v)
		{
			this.x += v.x;
			this.y += v.y;
			return this;
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00038EE9 File Offset: 0x000370E9
		public SexyVector2 SubSelf(SexyVector2 v)
		{
			this.x -= v.x;
			this.y -= v.y;
			return this;
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x00038F19 File Offset: 0x00037119
		public SexyVector2 MulSelf(SexyVector2 v)
		{
			this.x *= v.x;
			this.y *= v.y;
			return this;
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x00038F49 File Offset: 0x00037149
		public SexyVector2 DivSelf(SexyVector2 v)
		{
			this.x /= v.x;
			this.y /= v.y;
			return this;
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00038F79 File Offset: 0x00037179
		public static bool operator ==(SexyVector2 ImpliedObject, SexyVector2 v)
		{
			return ImpliedObject.Equals(v);
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x00038F83 File Offset: 0x00037183
		public static bool operator !=(SexyVector2 ImpliedObject, SexyVector2 v)
		{
			return !(ImpliedObject.mVector == v.mVector);
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x00038F9B File Offset: 0x0003719B
		public float Magnitude()
		{
			return this.mVector.Length();
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x00038FA8 File Offset: 0x000371A8
		public float MagnitudeSquared()
		{
			return this.mVector.LengthSquared();
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x00038FB5 File Offset: 0x000371B5
		public SexyVector2 Normalize()
		{
			this.mVector.Normalize();
			return this;
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x00038FC8 File Offset: 0x000371C8
		public SexyVector2 Perp()
		{
			return new SexyVector2(-this.y, this.x);
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x00038FDC File Offset: 0x000371DC
		public static implicit operator Vector2(SexyVector2 ImpliedObject)
		{
			return new Vector2(ImpliedObject.x, ImpliedObject.y);
		}

		// Token: 0x0400095D RID: 2397
		public Vector2 mVector;

		// Token: 0x0400095E RID: 2398
		public static SexyVector2 Zero = new SexyVector2(0f, 0f);
	}
}
