using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000C1 RID: 193
	public enum ImageFlags
	{
		// Token: 0x040004D2 RID: 1234
		ImageFlag_NONE,
		// Token: 0x040004D3 RID: 1235
		ImageFlag_MinimizeNumSubdivisions,
		// Token: 0x040004D4 RID: 1236
		ImageFlag_Use64By64Subdivisions,
		// Token: 0x040004D5 RID: 1237
		ImageFlag_UseA4R4G4B4 = 4,
		// Token: 0x040004D6 RID: 1238
		ImageFlag_UseA8R8G8B8 = 8,
		// Token: 0x040004D7 RID: 1239
		ImageFlag_RenderTarget = 16,
		// Token: 0x040004D8 RID: 1240
		ImageFlag_CubeMap = 32,
		// Token: 0x040004D9 RID: 1241
		ImageFlag_VolumeMap = 64,
		// Token: 0x040004DA RID: 1242
		ImageFlag_NoTriRep = 128,
		// Token: 0x040004DB RID: 1243
		ImageFlag_NoQuadRep = 128,
		// Token: 0x040004DC RID: 1244
		ImageFlag_RTUseDefaultRenderMode = 256,
		// Token: 0x040004DD RID: 1245
		ImageFlag_Atlas = 512,
		// Token: 0x040004DE RID: 1246
		REFLECT_ATTR_ENUM_FLAGS
	}
}
