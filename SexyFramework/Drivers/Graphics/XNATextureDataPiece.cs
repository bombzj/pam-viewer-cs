using System;
using Microsoft.Xna.Framework.Graphics;

namespace SexyFramework.Drivers.Graphics
{
	// Token: 0x02000033 RID: 51
	public class XNATextureDataPiece
	{
		// Token: 0x06000208 RID: 520 RVA: 0x00007404 File Offset: 0x00005604
		public XNATextureDataPiece()
		{
			this.mTexture = null;
			this.mCubeTexture = null;
			this.mVolumeTexture = null;
			this.mTexFormat = 0;
			this.mWidth = 0;
			this.mHeight = 0;
		}

		// Token: 0x04000119 RID: 281
		public Texture2D mTexture;

		// Token: 0x0400011A RID: 282
		public TextureCube mCubeTexture;

		// Token: 0x0400011B RID: 283
		public Texture3D mVolumeTexture;

		// Token: 0x0400011C RID: 284
		public int mTexFormat;

		// Token: 0x0400011D RID: 285
		public int mWidth;

		// Token: 0x0400011E RID: 286
		public int mHeight;
	}
}
