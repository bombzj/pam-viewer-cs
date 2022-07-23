using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.Drivers
{
	// Token: 0x0200003F RID: 63
	public abstract class IGraphicsDriver
	{
		// Token: 0x060002FF RID: 767 RVA: 0x0000BE00 File Offset: 0x0000A000
		public static string ResultToString(int theResult)
		{
			switch (theResult)
			{
			case 0:
				return "RESULT_OK";
			case 1:
				return "RESULT_FAIL";
			case 2:
				return "RESULT_DD_CREATE_FAIL";
			case 3:
				return "RESULT_SURFACE_FAIL";
			case 4:
				return "RESULT_EXCLUSIVE_FAIL";
			case 5:
				return "RESULT_DISPCHANGE_FAIL";
			case 6:
				return "RESULT_INVALID_COLORDEPTH";
			default:
				return "RESULT_UNKNOWN";
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000BE62 File Offset: 0x0000A062
		public virtual void Dispose()
		{
		}

		// Token: 0x06000301 RID: 769
		public abstract bool Is3D();

		// Token: 0x06000302 RID: 770
		public abstract int GetVersion();

		// Token: 0x06000303 RID: 771
		public abstract ulong GetRenderModeFlags();

		// Token: 0x06000304 RID: 772
		public abstract void SetRenderModeFlags(ulong flag);

		// Token: 0x06000305 RID: 773
		public abstract IGraphicsDriver.ERenderMode GetRenderMode();

		// Token: 0x06000306 RID: 774
		public abstract void SetRenderMode(IGraphicsDriver.ERenderMode inRenderMode);

		// Token: 0x06000307 RID: 775
		public abstract string GetRenderModeString(IGraphicsDriver.ERenderMode inRenderMode, ulong inRenderModeFlags, bool inIgnoreMode, bool inIgnoreFlags);

		// Token: 0x06000308 RID: 776
		public abstract void AddDeviceImage(DeviceImage theDDImage);

		// Token: 0x06000309 RID: 777
		public abstract void RemoveDeviceImage(DeviceImage theDDImage);

		// Token: 0x0600030A RID: 778
		public abstract void Remove3DData(MemoryImage theImage);

		// Token: 0x0600030B RID: 779
		public abstract DeviceImage GetScreenImage();

		// Token: 0x0600030C RID: 780
		public abstract int GetScreenWidth();

		// Token: 0x0600030D RID: 781
		public abstract int GetScreenHeight();

		// Token: 0x0600030E RID: 782
		public abstract void WindowResize(int theWidth, int theHeight);

		// Token: 0x0600030F RID: 783
		public abstract bool Redraw(Rect theClipRect);

		// Token: 0x06000310 RID: 784
		public abstract void RemapMouse(ref int theX, ref int theY);

		// Token: 0x06000311 RID: 785
		public abstract bool SetCursorImage(Image theImage);

		// Token: 0x06000312 RID: 786
		public abstract void SetCursorPos(int theCursorX, int theCursorY);

		// Token: 0x06000313 RID: 787
		public abstract void RemoveShader(object theShader);

		// Token: 0x06000314 RID: 788
		public abstract DeviceSurface CreateDeviceSurface();

		// Token: 0x06000315 RID: 789
		public abstract NativeDisplay GetNativeDisplayInfo();

		// Token: 0x06000316 RID: 790
		public abstract RenderDevice GetRenderDevice();

		// Token: 0x06000317 RID: 791
		public abstract RenderDevice3D GetRenderDevice3D();

		// Token: 0x06000318 RID: 792
		public abstract Ratio GetAspectRatio();

		// Token: 0x06000319 RID: 793
		public abstract int GetDisplayWidth();

		// Token: 0x0600031A RID: 794
		public abstract int GetDisplayHeight();

		// Token: 0x0600031B RID: 795
		public abstract CritSect GetCritSect();

		// Token: 0x0600031C RID: 796
		public abstract void SetRenderRect(int theX, int theY, int theWidth, int theHeight);

		// Token: 0x0600031D RID: 797
		public abstract Mesh LoadMesh(string thePath, MeshListener theListener);

		// Token: 0x0600031E RID: 798
		public abstract void AddMesh(Mesh theMesh);

		// Token: 0x02000040 RID: 64
		public enum EResult
		{
			// Token: 0x04000181 RID: 385
			RESULT_OK,
			// Token: 0x04000182 RID: 386
			RESULT_FAIL,
			// Token: 0x04000183 RID: 387
			RESULT_DD_CREATE_FAIL,
			// Token: 0x04000184 RID: 388
			RESULT_SURFACE_FAIL,
			// Token: 0x04000185 RID: 389
			RESULT_EXCLUSIVE_FAIL,
			// Token: 0x04000186 RID: 390
			RESULT_DISPCHANGE_FAIL,
			// Token: 0x04000187 RID: 391
			RESULT_INVALID_COLORDEPTH,
			// Token: 0x04000188 RID: 392
			RESULT_3D_FAIL,
			// Token: 0x04000189 RID: 393
			RESULT_3D_NOTREADY
		}

		// Token: 0x02000041 RID: 65
		public enum ERenderMode
		{
			// Token: 0x0400018B RID: 395
			RENDERMODE_Default,
			// Token: 0x0400018C RID: 396
			RENDERMODE_Overdraw,
			// Token: 0x0400018D RID: 397
			RENDERMODE_PseudoOverdraw,
			// Token: 0x0400018E RID: 398
			RENDERMODE_BatchSize,
			// Token: 0x0400018F RID: 399
			RENDERMODE_Wireframe,
			// Token: 0x04000190 RID: 400
			RENDERMODE_WastedOverdraw,
			// Token: 0x04000191 RID: 401
			RENDERMODE_TextureHash,
			// Token: 0x04000192 RID: 402
			RENDERMODE_OverdrawExact,
			// Token: 0x04000193 RID: 403
			RENDERMODE_COUNT,
			// Token: 0x04000194 RID: 404
			RENDERMODE_CYCLE_END = 7
		}

		// Token: 0x02000042 RID: 66
		public enum ERenderModeFlags
		{
			// Token: 0x04000196 RID: 406
			RENDERMODEF_NoBatching = 1,
			// Token: 0x04000197 RID: 407
			RENDERMODEF_HalfTris,
			// Token: 0x04000198 RID: 408
			RENDERMODEF_NoDynVB = 4,
			// Token: 0x04000199 RID: 409
			RENDERMODEF_PreventLag = 8,
			// Token: 0x0400019A RID: 410
			RENDERMODEF_NoTriRep = 16,
			// Token: 0x0400019B RID: 411
			RENDERMODEF_NoStretchRectFromTextures = 32,
			// Token: 0x0400019C RID: 412
			RENDERMODEF_HalfPresent = 64,
			// Token: 0x0400019D RID: 413
			RENDERMODEF_USEDBITS = 7
		}
	}
}
