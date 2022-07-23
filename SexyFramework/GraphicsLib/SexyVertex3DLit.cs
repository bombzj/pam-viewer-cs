using System;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000FF RID: 255
	public class SexyVertex3DLit : SexyVertex
	{
		// Token: 0x06000767 RID: 1895 RVA: 0x000265C8 File Offset: 0x000247C8
		public void MakeDefaultNormal()
		{
			SexyVector3 sexyVector = new SexyVector3(this.x, this.y, this.z);
			sexyVector = sexyVector.Normalize();
			this.nx = sexyVector.x;
			this.ny = sexyVector.y;
			this.nz = sexyVector.z;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0002661D File Offset: 0x0002481D
		public SexyVertex3DLit()
		{
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00026628 File Offset: 0x00024828
		public SexyVertex3DLit(SexyVector3 thePos, SexyVector3 theNormal)
		{
			this.x = thePos.x;
			this.y = thePos.y;
			this.z = thePos.z;
			this.nx = theNormal.x;
			this.ny = theNormal.y;
			this.nz = theNormal.z;
			this.color = 0U;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00026690 File Offset: 0x00024890
		public SexyVertex3DLit(SexyVector3 thePos, SexyVector3 theNormal, float theU, float theV)
		{
			this.x = thePos.x;
			this.y = thePos.y;
			this.z = thePos.z;
			this.nx = theNormal.x;
			this.ny = theNormal.y;
			this.nz = theNormal.z;
			this.u = theU;
			this.v = theV;
			this.color = 0U;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00026708 File Offset: 0x00024908
		public SexyVertex3DLit(SexyVector3 thePos, SexyVector3 theNormal, float theU, float theV, uint theColor)
		{
			this.x = thePos.x;
			this.y = thePos.y;
			this.z = thePos.z;
			this.nx = theNormal.x;
			this.ny = theNormal.y;
			this.nz = theNormal.z;
			this.u = theU;
			this.v = theV;
			this.color = theColor;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00026780 File Offset: 0x00024980
		public SexyVertex3DLit(SexyVector3 thePos)
		{
			this.x = thePos.x;
			this.y = thePos.y;
			this.z = thePos.z;
			this.MakeDefaultNormal();
			this.color = 0U;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x000267BC File Offset: 0x000249BC
		public SexyVertex3DLit(SexyVector3 thePos, float theU, float theV)
		{
			this.x = thePos.x;
			this.y = thePos.y;
			this.z = thePos.z;
			this.u = theU;
			this.v = theV;
			this.MakeDefaultNormal();
			this.color = 0U;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00026814 File Offset: 0x00024A14
		public SexyVertex3DLit(SexyVector3 thePos, float theU, float theV, uint theColor)
		{
			this.x = thePos.x;
			this.y = thePos.y;
			this.z = thePos.z;
			this.u = theU;
			this.v = theV;
			this.color = theColor;
			this.MakeDefaultNormal();
		}

		// Token: 0x040006F4 RID: 1780
		public float x;

		// Token: 0x040006F5 RID: 1781
		public float y;

		// Token: 0x040006F6 RID: 1782
		public float z;

		// Token: 0x040006F7 RID: 1783
		public float nx;

		// Token: 0x040006F8 RID: 1784
		public float ny;

		// Token: 0x040006F9 RID: 1785
		public float nz;

		// Token: 0x040006FA RID: 1786
		public uint color;

		// Token: 0x040006FB RID: 1787
		public float u;

		// Token: 0x040006FC RID: 1788
		public float v;

		// Token: 0x040006FD RID: 1789
		public static readonly int FVF = 338;
	}
}
