using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.Resource;

namespace SexyFramework.Drivers.Graphics
{
	// Token: 0x02000043 RID: 67
	public class XNAGraphicsDriver : IGraphicsDriver
	{
		// Token: 0x06000320 RID: 800 RVA: 0x0000BE6C File Offset: 0x0000A06C
		public override int GetVersion()
		{
			return 0;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000BE6F File Offset: 0x0000A06F
		public override bool Is3D()
		{
			return true;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000BE72 File Offset: 0x0000A072
		public void ClearColorBuffer(SexyColor color)
		{
			this.mXNARenderDevice.ClearColorBuffer(color);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000BE80 File Offset: 0x0000A080
		public override ulong GetRenderModeFlags()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000BE87 File Offset: 0x0000A087
		public override void SetRenderModeFlags(ulong flag)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000BE8E File Offset: 0x0000A08E
		public override IGraphicsDriver.ERenderMode GetRenderMode()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000BE95 File Offset: 0x0000A095
		public override void SetRenderMode(IGraphicsDriver.ERenderMode inRenderMode)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000BE9C File Offset: 0x0000A09C
		public override string GetRenderModeString(IGraphicsDriver.ERenderMode inRenderMode, ulong inRenderModeFlags, bool inIgnoreMode, bool inIgnoreFlags)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000BEA3 File Offset: 0x0000A0A3
		public override void AddDeviceImage(DeviceImage theDDImage)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000BEAA File Offset: 0x0000A0AA
		public override void RemoveDeviceImage(DeviceImage theDDImage)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000BEB1 File Offset: 0x0000A0B1
		public override void Remove3DData(MemoryImage theImage)
		{
			if (theImage == null)
			{
				return;
			}
			this.mXNARenderDevice.RemoveImageRenderData(theImage);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000BEC3 File Offset: 0x0000A0C3
		public override DeviceImage GetScreenImage()
		{
			return this.mScreenImage;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000BECB File Offset: 0x0000A0CB
		public override int GetScreenWidth()
		{
			return this.mWidth;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000BED3 File Offset: 0x0000A0D3
		public override int GetScreenHeight()
		{
			return this.mHeight;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000BEDB File Offset: 0x0000A0DB
		public override void WindowResize(int theWidth, int theHeight)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000BEE4 File Offset: 0x0000A0E4
		public override bool Redraw(Rect theClipRect)
		{
			Rect rect = new Rect(0, 0, this.mWidth, this.mHeight);
			this.mXNARenderDevice.Present(rect, rect);
			return true;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000BF15 File Offset: 0x0000A115
		public override void RemapMouse(ref int theX, ref int theY)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000BF1C File Offset: 0x0000A11C
		public override bool SetCursorImage(Image theImage)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000BF23 File Offset: 0x0000A123
		public override void SetCursorPos(int theCursorX, int theCursorY)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000BF2A File Offset: 0x0000A12A
		public override void RemoveShader(object theShader)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000BF31 File Offset: 0x0000A131
		public override DeviceSurface CreateDeviceSurface()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000BF38 File Offset: 0x0000A138
		public override NativeDisplay GetNativeDisplayInfo()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000BF3F File Offset: 0x0000A13F
		public override RenderDevice GetRenderDevice()
		{
			return this.mXNARenderDevice;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000BF47 File Offset: 0x0000A147
		public override RenderDevice3D GetRenderDevice3D()
		{
			return this.mXNARenderDevice;
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000BF4F File Offset: 0x0000A14F
		public override Ratio GetAspectRatio()
		{
			return new Ratio(4, 3);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000BF58 File Offset: 0x0000A158
		public override int GetDisplayWidth()
		{
			return this.mWidth;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000BF60 File Offset: 0x0000A160
		public override int GetDisplayHeight()
		{
			return this.mHeight;
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000BF68 File Offset: 0x0000A168
		public override CritSect GetCritSect()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000BF6F File Offset: 0x0000A16F
		public override Mesh LoadMesh(string thePath, MeshListener theListener)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000BF76 File Offset: 0x0000A176
		public override void AddMesh(Mesh theMesh)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000BF80 File Offset: 0x0000A180
		public XNAGraphicsDriver(Game game, SexyAppBase app)
		{
			this.mMainGame = game;
			this.mApp = app;
			this.mWidth = this.mApp.mWidth;
			this.mHeight = this.mApp.mHeight;
			this.mXNARenderDevice = new BaseXNARenderDevice(this.mMainGame);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000BFD4 File Offset: 0x0000A1D4
		public override void Dispose()
		{
			this.mXNARenderDevice = null;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000BFDD File Offset: 0x0000A1DD
		public Game GetMainGame()
		{
			return this.mMainGame;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000BFE8 File Offset: 0x0000A1E8
		public virtual void Init()
		{
			this.mXNARenderDevice.Init(this.mWidth, this.mHeight);
			this.mScreenImage = new DeviceImage();
			this.mScreenImage.AddImageFlags(ImageFlags.ImageFlag_RenderTarget);
			Texture2D texture2D = this.mXNARenderDevice.CreateTexture2D(this.mWidth, this.mHeight, PixelFormat.PixelFormat_A8R8G8B8, true, null, null);
			this.mScreenImage.mWidth = texture2D.Width;
			this.mScreenImage.mHeight = texture2D.Height;
			this.mScreenImage.mHasAlpha = true;
			XNATextureData xnatextureData = new XNATextureData(this.mXNARenderDevice);
			this.mScreenImage.SetRenderData(xnatextureData);
			xnatextureData.mWidth = texture2D.Width;
			xnatextureData.mHeight = texture2D.Height;
			xnatextureData.mTexPieceWidth = texture2D.Width;
			xnatextureData.mTexPieceHeight = texture2D.Height;
			xnatextureData.mTexVecWidth = 1;
			xnatextureData.mTexVecHeight = 1;
			xnatextureData.mPixelFormat = PixelFormat.PixelFormat_A8R8G8B8;
			xnatextureData.mMaxTotalU = 1f;
			xnatextureData.mMaxTotalV = 1f;
			xnatextureData.mImageFlags = this.mScreenImage.GetImageFlags();
			xnatextureData.mOptimizedLoad = true;
			xnatextureData.mTextures[0].mWidth = texture2D.Width;
			xnatextureData.mTextures[0].mHeight = texture2D.Height;
			xnatextureData.mTextures[0].mTexture = texture2D;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000C12D File Offset: 0x0000A32D
		public void Update(long gameTime)
		{
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000C12F File Offset: 0x0000A32F
		public void Draw(long gameTime)
		{
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000C134 File Offset: 0x0000A334
		public virtual object GetOptimizedRenderData(string theFileName)
		{
			PFILE pfile = new PFILE(theFileName, "");
			pfile.Open<Texture2D>();
			return pfile.GetObject();
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000C15C File Offset: 0x0000A35C
		public virtual DeviceImage GetOptimizedImage(string theFileName, bool commitBits, bool allowTriReps)
		{
			PFILE pfile = new PFILE(theFileName, "");
			pfile.Open<Texture2D>();
			Texture2D texture2D = pfile.GetObject() as Texture2D;
			texture2D.Name = theFileName;
			return this.mXNARenderDevice.GetOptimizedImage(pfile.GetObject() as Texture2D, commitBits, allowTriReps);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000C1A8 File Offset: 0x0000A3A8
		public virtual DeviceImage GetOptimizedImage(Stream stream, bool commitBits, bool allowTriReps)
		{
			if (stream != null)
			{
				try
				{
					Texture2D texture = Texture2D.FromStream(this.mXNARenderDevice.mDevice.GraphicsDevice, stream);
					return this.mXNARenderDevice.GetOptimizedImage(texture, commitBits, allowTriReps);
				}
				catch (Exception)
				{
					return null;
				}
			}
			return null;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000C1F8 File Offset: 0x0000A3F8
		public virtual DeviceImage GetOptimizedImageFromData(string theFileName, bool commitBits, bool allowTriReps, int width, int height)
		{
			PFILE pfile = new PFILE(theFileName, "");
			pfile.Open();
			Texture2D texture = this.mXNARenderDevice.CreateTexture2DFromData(pfile.GetData());
			return this.mXNARenderDevice.GetOptimizedImage(texture, commitBits, allowTriReps);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000C238 File Offset: 0x0000A438
		public virtual SpriteBatch GetSpriteBatch()
		{
			return this.mXNARenderDevice.mSpriteBatch;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000C245 File Offset: 0x0000A445
		public override void SetRenderRect(int theX, int theY, int theWidth, int theHeight)
		{
			this.mXNARenderDevice.SetRenderRect(theX, theY, theWidth, theHeight);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000C257 File Offset: 0x0000A457
		public void SetOrientation(int Orientation)
		{
			this.mXNARenderDevice.SetOrientation(Orientation);
		}

		// Token: 0x0400019E RID: 414
		private Game mMainGame;

		// Token: 0x0400019F RID: 415
		private SexyAppBase mApp;

		// Token: 0x040001A0 RID: 416
		public BaseXNARenderDevice mXNARenderDevice;

		// Token: 0x040001A1 RID: 417
		private DeviceImage mScreenImage;

		// Token: 0x040001A2 RID: 418
		public int mWidth;

		// Token: 0x040001A3 RID: 419
		public int mHeight;
	}
}
