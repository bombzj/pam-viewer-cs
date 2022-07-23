using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000A9 RID: 169
	public abstract class DeviceSurface
	{
		// Token: 0x060004EE RID: 1262 RVA: 0x0000E9F1 File Offset: 0x0000CBF1
		public virtual void Dispose()
		{
		}

		// Token: 0x060004EF RID: 1263
		public abstract int Lock(_DEVICESURFACEDESC NamelessParameter);

		// Token: 0x060004F0 RID: 1264
		public abstract void Unlock(IntPtr NamelessParameter);

		// Token: 0x060004F1 RID: 1265
		public abstract int GetVersion();

		// Token: 0x060004F2 RID: 1266
		public abstract bool GenerateDeviceSurface(DeviceImage theImage);

		// Token: 0x060004F3 RID: 1267
		public abstract int HasSurface();

		// Token: 0x060004F4 RID: 1268
		public abstract IntPtr GetSurfacePtr();

		// Token: 0x060004F5 RID: 1269
		public abstract void AddRef();

		// Token: 0x060004F6 RID: 1270
		public abstract void Release();

		// Token: 0x060004F7 RID: 1271
		public abstract uint GetBits(DeviceImage theImage);

		// Token: 0x060004F8 RID: 1272
		public abstract void SetSurface(IntPtr theSurface);

		// Token: 0x060004F9 RID: 1273
		public abstract void GetDimensions(int theWidth, int theHeight);

		// Token: 0x0400045A RID: 1114
		public uint mImageFlags;
	}
}
