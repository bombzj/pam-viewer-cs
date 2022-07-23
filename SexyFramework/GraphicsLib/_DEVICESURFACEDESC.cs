using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000A8 RID: 168
	public class _DEVICESURFACEDESC
	{
		// Token: 0x04000454 RID: 1108
		public uint dwFlags;

		// Token: 0x04000455 RID: 1109
		public uint dwHeight;

		// Token: 0x04000456 RID: 1110
		public uint dwWidth;

		// Token: 0x04000457 RID: 1111
		public uint lPitch;

		// Token: 0x04000458 RID: 1112
		public IntPtr lpSurface;

		// Token: 0x04000459 RID: 1113
		public _DEVICEPIXELFORMAT ddpfPixelFormat = new _DEVICEPIXELFORMAT();
	}
}
