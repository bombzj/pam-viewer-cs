using System;
using SexyFramework.Drivers;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x0200008A RID: 138
	public class DeviceImage : MemoryImage
	{
		// Token: 0x060004D2 RID: 1234 RVA: 0x0000E798 File Offset: 0x0000C998
		protected void DeleteAllNonSurfaceData()
		{
			this.mBits = null;
			this.mBits = null;
			this.mNativeAlphaData = null;
			this.mNativeAlphaData = null;
			this.mRLAdditiveData = null;
			this.mRLAdditiveData = null;
			this.mRLAlphaData = null;
			this.mRLAlphaData = null;
			this.mColorTable = null;
			this.mColorTable = null;
			this.mColorIndices = null;
			this.mColorIndices = null;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0000E7F9 File Offset: 0x0000C9F9
		private void Init()
		{
			this.mSurface = null;
			this.mNoLock = false;
			this.mWantDeviceSurface = false;
			this.mDrawToBits = false;
			this.mSurfaceSet = false;
			this.mLockCount = 0;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0000E825 File Offset: 0x0000CA25
		public override DeviceImage AsDeviceImage()
		{
			return this;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0000E828 File Offset: 0x0000CA28
		public bool GenerateDeviceSurface()
		{
			return this.mSurface != null && false;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0000E835 File Offset: 0x0000CA35
		public void DeleteDeviceSurface()
		{
			if (this.mSurface != null)
			{
				if (this.mColorTable == null && this.mBits == null && base.GetRenderData() == null)
				{
					base.GetBits();
				}
				this.mSurface = null;
				this.mSurface = null;
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0000E86C File Offset: 0x0000CA6C
		public void ReInit()
		{
			if (this.mWantDeviceSurface)
			{
				this.GenerateDeviceSurface();
			}
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0000E87D File Offset: 0x0000CA7D
		public override void BitsChanged()
		{
			this.mSurface = null;
			this.mSurface = null;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0000E88D File Offset: 0x0000CA8D
		public void CommitBits()
		{
			DeviceSurface deviceSurface = this.mSurface;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0000E896 File Offset: 0x0000CA96
		public virtual bool LockSurface()
		{
			return true;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0000E899 File Offset: 0x0000CA99
		public virtual bool UnlockSurface()
		{
			return true;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0000E89C File Offset: 0x0000CA9C
		public virtual void SetSurface(IntPtr theSurface)
		{
			this.mSurfaceSet = true;
			if (this.mSurface != null)
			{
				int version = this.mDriver.GetVersion();
				if (this.mSurface.GetVersion() != version)
				{
					this.mSurface = null;
					this.mSurface = null;
				}
			}
			DeviceSurface deviceSurface = this.mSurface;
			this.mSurface.SetSurface(theSurface);
			this.mSurface.GetDimensions(this.mWidth, this.mHeight);
			this.mNoLock = false;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0000E911 File Offset: 0x0000CB11
		public override void Create(int theWidth, int theHeight)
		{
			base.Create(theWidth, theHeight);
			this.mBits = null;
			this.mWidth = theWidth;
			this.mHeight = theHeight;
			this.mBits = null;
			this.BitsChanged();
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0000E93D File Offset: 0x0000CB3D
		public void BltF(Image theImage, float theX, float theY, Rect theSrcRect, Rect theClipRect, SexyColor theColor, int theDrawMode)
		{
			theImage.mDrawn = true;
			this.CommitBits();
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0000E94C File Offset: 0x0000CB4C
		public void BltRotated(Image theImage, float theX, float theY, Rect theSrcRect, Rect theClipRect, SexyColor theColor, int theDrawMode, double theRot, float theRotCenterX, float theRotCenterY)
		{
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0000E94E File Offset: 0x0000CB4E
		public void BltStretched(Image theImage, Rect theDestRectOrig, Rect theSrcRectOrig, Rect theClipRect, SexyColor theColor, int theDrawMode, int fastStretch)
		{
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0000E950 File Offset: 0x0000CB50
		public void BltStretched(Image theImage, Rect theDestRectOrig, Rect theSrcRectOrig, Rect theClipRect, SexyColor theColor, int theDrawMode, int fastStretch, int mirror)
		{
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0000E952 File Offset: 0x0000CB52
		public override bool Palletize()
		{
			return false;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0000E955 File Offset: 0x0000CB55
		public override void PurgeBits()
		{
			if (this.mSurfaceSet)
			{
				return;
			}
			this.mPurgeBits = true;
			this.mBits = null;
			this.mBits = null;
			this.mColorIndices = null;
			this.mColorIndices = null;
			this.mColorTable = null;
			this.mColorTable = null;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0000E991 File Offset: 0x0000CB91
		public void DeleteNativeData()
		{
			if (this.mSurfaceSet)
			{
				return;
			}
			this.DeleteDeviceSurface();
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0000E9A2 File Offset: 0x0000CBA2
		public void DeleteExtraBuffers()
		{
			if (this.mSurfaceSet)
			{
				return;
			}
			this.DeleteDeviceSurface();
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0000E9B3 File Offset: 0x0000CBB3
		public static int CheckCache(string theSrcFile, string theAltData)
		{
			return 0;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0000E9B6 File Offset: 0x0000CBB6
		public static int SetCacheUpToDate(string theSrcFile, string theAltData)
		{
			return 0;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0000E9B9 File Offset: 0x0000CBB9
		public static DeviceImage ReadFromCache(string theSrcFile, string theAltData)
		{
			return null;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0000E9BC File Offset: 0x0000CBBC
		public virtual void WriteToCache(string theSrcFile, string theAltData)
		{
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0000E9BE File Offset: 0x0000CBBE
		internal void AddImageFlags(ImageFlags imageFlags)
		{
			this.mImageFlags |= imageFlags;
		}

		// Token: 0x040002D3 RID: 723
		public IGraphicsDriver mDriver;

		// Token: 0x040002D4 RID: 724
		public bool mSurfaceSet;

		// Token: 0x040002D5 RID: 725
		public bool mNoLock;

		// Token: 0x040002D6 RID: 726
		public bool mWantDeviceSurface;

		// Token: 0x040002D7 RID: 727
		public bool mDrawToBits;

		// Token: 0x040002D8 RID: 728
		public int mLockCount;

		// Token: 0x040002D9 RID: 729
		public DeviceSurface mSurface;
	}
}
