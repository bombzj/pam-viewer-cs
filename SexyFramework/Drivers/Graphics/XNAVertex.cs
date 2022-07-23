using System;
using Microsoft.Xna.Framework;
using SexyFramework.GraphicsLib;

namespace SexyFramework.Drivers.Graphics
{
	// Token: 0x02000032 RID: 50
	public class XNAVertex
	{
		// Token: 0x06000203 RID: 515 RVA: 0x00007348 File Offset: 0x00005548
		public static float GetCoord(SexyVertex2D theVertex, int theCoord)
		{
			switch (theCoord)
			{
			case 0:
				return theVertex.x;
			case 1:
				return theVertex.y;
			case 2:
				return theVertex.z;
			case 3:
				return theVertex.u;
			case 4:
				return theVertex.v;
			default:
				return 0f;
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x000073A0 File Offset: 0x000055A0
		public static SexyColor UnPackColor(uint color)
		{
			return new SexyColor(((int)color >> 16) & 255, ((int)color >> 8) & 255, (int)(color & 255U), ((int)color >> 24) & 255);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x000073CB File Offset: 0x000055CB
		public SexyColor GetXNAColor()
		{
			return new SexyColor(this.mColor.mRed, this.mColor.mGreen, this.mColor.mBlue, this.mColor.mAlpha);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x000073FE File Offset: 0x000055FE
		public void SetPosition(float theX, float theY, float theZ)
		{
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00007400 File Offset: 0x00005600
		public static uint TexCoordOffset()
		{
			return 24U;
		}

		// Token: 0x04000118 RID: 280
		public SexyColor mColor;
	}
}
