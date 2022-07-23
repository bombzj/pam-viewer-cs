using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000FD RID: 253
	public struct SexyVertex2D : SexyVertex
	{
		// Token: 0x0600075D RID: 1885 RVA: 0x000263B8 File Offset: 0x000245B8
		public SexyVertex2D(float theX, float theY)
		{
			this.x = theX;
			this.y = theY;
			this.z = 0f;
			this.u = 0f;
			this.v = 0f;
			this.rhw = 1f;
			this.color = SexyColor.Zero;
			this.specular = 0U;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00026414 File Offset: 0x00024614
		public SexyVertex2D(float theX, float theY, float theU, float theV)
		{
			this.x = theX;
			this.y = theY;
			this.u = theU;
			this.v = theV;
			this.z = 0f;
			this.rhw = 1f;
			this.color = SexyColor.Zero;
			this.specular = 0U;
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00026468 File Offset: 0x00024668
		public SexyVertex2D(float theX, float theY, float theU, float theV, uint theColor)
		{
			this.x = theX;
			this.y = theY;
			this.u = theU;
			this.v = theV;
			this.color = new SexyColor((int)theColor);
			this.z = 0f;
			this.rhw = 1f;
			this.specular = 0U;
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x000264BC File Offset: 0x000246BC
		public SexyVertex2D(float theX, float theY, float theZ, float theU, float theV, uint theColor)
		{
			this.x = theX;
			this.y = theY;
			this.z = theZ;
			this.u = theU;
			this.v = theV;
			this.color = new SexyColor((int)theColor);
			this.z = 0f;
			this.rhw = 1f;
			this.specular = 0U;
		}

		// Token: 0x040006E4 RID: 1764
		public float x;

		// Token: 0x040006E5 RID: 1765
		public float y;

		// Token: 0x040006E6 RID: 1766
		public float z;

		// Token: 0x040006E7 RID: 1767
		public float rhw;

		// Token: 0x040006E8 RID: 1768
		public SexyColor color;

		// Token: 0x040006E9 RID: 1769
		public uint specular;

		// Token: 0x040006EA RID: 1770
		public float u;

		// Token: 0x040006EB RID: 1771
		public float v;

		// Token: 0x040006EC RID: 1772
		public static readonly int FVF = 452;
	}
}
