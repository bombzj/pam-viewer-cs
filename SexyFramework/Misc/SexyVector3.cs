using System;
using Microsoft.Xna.Framework;

namespace SexyFramework.Misc
{
	// Token: 0x02000150 RID: 336
	public struct SexyVector3
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x00039007 File Offset: 0x00037207
		// (set) Token: 0x06000BB0 RID: 2992 RVA: 0x00039014 File Offset: 0x00037214
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

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x00039022 File Offset: 0x00037222
		// (set) Token: 0x06000BB2 RID: 2994 RVA: 0x0003902F File Offset: 0x0003722F
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

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x0003903D File Offset: 0x0003723D
		// (set) Token: 0x06000BB4 RID: 2996 RVA: 0x0003904A File Offset: 0x0003724A
		public float z
		{
			get
			{
				return this.mVector.Z;
			}
			set
			{
				this.mVector.Z = value;
			}
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00039058 File Offset: 0x00037258
		public SexyVector3(float theX, float theY, float theZ)
		{
			this.mVector = new Vector3(theX, theY, theZ);
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00039068 File Offset: 0x00037268
		public SexyVector3(SexyVector3 rhs)
		{
			this.mVector = rhs.mVector;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00039077 File Offset: 0x00037277
		public SexyVector3(Vector3 rhs)
		{
			this.mVector = rhs;
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x00039080 File Offset: 0x00037280
		public float Dot(SexyVector3 v)
		{
			return Vector3.Dot(this.mVector, v.mVector);
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00039094 File Offset: 0x00037294
		public SexyVector3 Cross(SexyVector3 v)
		{
			return new SexyVector3(Vector3.Cross(this.mVector, v.mVector));
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x000390AD File Offset: 0x000372AD
		public SexyVector3 CopyFrom(SexyVector3 v)
		{
			this.mVector = v.mVector;
			return this;
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x000390C2 File Offset: 0x000372C2
		public static SexyVector3 operator -(SexyVector3 ImpliedObject)
		{
			return new SexyVector3(-ImpliedObject.x, -ImpliedObject.y, -ImpliedObject.z);
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x000390E1 File Offset: 0x000372E1
		public static SexyVector3 operator +(SexyVector3 ImpliedObject, SexyVector3 v)
		{
			return new SexyVector3(ImpliedObject.x + v.x, ImpliedObject.y + v.y, ImpliedObject.z + v.z);
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x00039115 File Offset: 0x00037315
		public static SexyVector3 operator -(SexyVector3 ImpliedObject, SexyVector3 v)
		{
			return new SexyVector3(ImpliedObject.x - v.x, ImpliedObject.y - v.y, ImpliedObject.z - v.z);
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x00039149 File Offset: 0x00037349
		public static SexyVector3 operator *(SexyVector3 ImpliedObject, float t)
		{
			return new SexyVector3(t * ImpliedObject.x, t * ImpliedObject.y, t * ImpliedObject.z);
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0003916B File Offset: 0x0003736B
		public static SexyVector3 operator *(SexyVector3 ImpliedObject, SexyVector3 v)
		{
			return new SexyVector3(Vector3.Multiply(ImpliedObject.mVector, v.mVector));
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00039185 File Offset: 0x00037385
		public static SexyVector3 operator /(SexyVector3 ImpliedObject, float t)
		{
			return new SexyVector3(Vector3.Divide(ImpliedObject.mVector, t));
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x00039199 File Offset: 0x00037399
		public static SexyVector3 operator /(SexyVector3 ImpliedObject, SexyVector3 v)
		{
			return new SexyVector3(Vector3.Divide(ImpliedObject.mVector, v.mVector));
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x000391B3 File Offset: 0x000373B3
		public float Magnitude()
		{
			return this.mVector.Length();
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x000391C0 File Offset: 0x000373C0
		public SexyVector3 Normalize()
		{
			this.mVector.Normalize();
			return this;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x000391D3 File Offset: 0x000373D3
		public bool ApproxEquals(SexyVector3 inV)
		{
			return this.ApproxEquals(inV, 0.001f);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x000391E4 File Offset: 0x000373E4
		public bool ApproxEquals(SexyVector3 inV, float inTol)
		{
			return SexyMath.ApproxEquals(this.x, inV.x, inTol) && SexyMath.ApproxEquals(this.y, inV.y, inTol) && SexyMath.ApproxEquals(this.z, inV.z, inTol);
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x00039230 File Offset: 0x00037430
		public bool ApproxZero()
		{
			return this.ApproxZero(0.001f);
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x0003923D File Offset: 0x0003743D
		public bool ApproxZero(float inTol)
		{
			return this.ApproxEquals(new SexyVector3(0f, 0f, 0f), inTol);
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0003925A File Offset: 0x0003745A
		public SexyVector3 Enter(SexyAxes3 inAxes)
		{
			return new SexyVector3(this.Dot(inAxes.vX), this.Dot(inAxes.vY), this.Dot(inAxes.vZ));
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x00039285 File Offset: 0x00037485
		public SexyVector3 Enter(SexyCoords3 inCoords)
		{
			return (this - inCoords.t.Enter(inCoords.r)) / inCoords.s;
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x000392B0 File Offset: 0x000374B0
		public SexyVector3 Leave(SexyAxes3 inAxes)
		{
			return new SexyVector3(this.x * inAxes.vX.x + this.y * inAxes.vY.x + this.z * inAxes.vZ.x, this.x * inAxes.vX.y + this.y * inAxes.vY.y + this.z * inAxes.vZ.y, this.x * inAxes.vX.z + this.y * inAxes.vY.z + this.z * inAxes.vZ.z);
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x0003936A File Offset: 0x0003756A
		public SexyVector3 Leave(SexyCoords3 inCoords)
		{
			return this * inCoords.s.Leave(inCoords.r) + inCoords.t;
		}

		// Token: 0x0400095F RID: 2399
		public Vector3 mVector;
	}
}
