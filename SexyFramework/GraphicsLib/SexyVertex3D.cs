using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000FE RID: 254
	public class SexyVertex3D : SexyVertex
	{
		// Token: 0x06000762 RID: 1890 RVA: 0x00026524 File Offset: 0x00024724
		public SexyVertex3D()
		{
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0002652C File Offset: 0x0002472C
		public SexyVertex3D(float theX, float theY, float theZ)
		{
			this.x = theX;
			this.y = theY;
			this.z = theZ;
			this.color = 0U;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00026550 File Offset: 0x00024750
		public SexyVertex3D(float theX, float theY, float theZ, float theU, float theV)
		{
			this.x = theX;
			this.y = theY;
			this.z = theZ;
			this.u = theU;
			this.v = theV;
			this.color = 0U;
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00026584 File Offset: 0x00024784
		public SexyVertex3D(float theX, float theY, float theZ, float theU, float theV, uint theColor)
		{
			this.x = theX;
			this.y = theY;
			this.z = theZ;
			this.u = theU;
			this.v = theV;
			this.color = theColor;
		}

		// Token: 0x040006ED RID: 1773
		public float x;

		// Token: 0x040006EE RID: 1774
		public float y;

		// Token: 0x040006EF RID: 1775
		public float z;

		// Token: 0x040006F0 RID: 1776
		public uint color;

		// Token: 0x040006F1 RID: 1777
		public float u;

		// Token: 0x040006F2 RID: 1778
		public float v;

		// Token: 0x040006F3 RID: 1779
		public static readonly int FVF = 322;
	}
}
