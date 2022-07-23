﻿using System;
using Microsoft.Xna.Framework.Graphics;
using SexyFramework.GraphicsLib;

namespace SexyFramework.Drivers.Graphics
{
	// Token: 0x02000034 RID: 52
	public class XNATextureData : IDisposable
	{
		// Token: 0x06000209 RID: 521 RVA: 0x00007438 File Offset: 0x00005638
		public XNATextureData(BaseXNARenderDevice theDevice)
		{
			this.mDevice = theDevice;
			this.mWidth = 0;
			this.mHeight = 0;
			this.mTexVecWidth = 0;
			this.mTexVecHeight = 0;
			this.mBitsChangedCount = 0;
			this.mTexMemSize = 0;
			this.mTexMemOriginalSize = 0;
			this.mTexMemFlushRevision = 0UL;
			this.mTexPieceWidth = 64;
			this.mTexPieceHeight = 64;
			this.mTextures = new XNATextureDataPiece[3];
			this.mTextures[0] = new XNATextureDataPiece();
			this.mPaletteIndex = -1;
			this.mPixelFormat = PixelFormat.PixelFormat_Unknown;
			this.mImageFlags = 0UL;
			this.mOptimizedLoad = false;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x000074D1 File Offset: 0x000056D1
		public virtual void Dispose()
		{
			this.ReleaseTextures();
		}

		// Token: 0x0600020B RID: 523 RVA: 0x000074DC File Offset: 0x000056DC
		private void ReleaseTextures()
		{
			for (int i = 0; i < this.mTextures.Length; i++)
			{
				if (this.mTextures[i] != null)
				{
					if (this.mTextures[i].mTexture != null)
					{
						GlobalMembers.gTotalGraphicsMemory -= this.mTextures[i].mTexture.Width * this.mTextures[i].mTexture.Height * 4;
						this.mTextures[i].mTexture.Dispose();
						this.mTextures[i].mTexture = null;
					}
					if (this.mTextures[i].mCubeTexture != null)
					{
						this.mTextures[i].mCubeTexture.Dispose();
						this.mTextures[i].mCubeTexture = null;
					}
					if (this.mTextures[i].mVolumeTexture != null)
					{
						this.mTextures[i].mVolumeTexture.Dispose();
						this.mTextures[i].mVolumeTexture = null;
					}
				}
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x000075D0 File Offset: 0x000057D0
		public void CreateTextures(ref MemoryImage theImage, BaseXNARenderDevice theDevice, bool commitBits)
		{
			theImage.DeleteSWBuffers();
			PixelFormat pixelFormat = PixelFormat.PixelFormat_A8R8G8B8;
			if (!theImage.mHasAlpha && !theImage.mHasTrans)
			{
				pixelFormat = PixelFormat.PixelFormat_X8R8G8B8;
			}
			if (theImage.HasImageFlag(4U) && pixelFormat == PixelFormat.PixelFormat_A8R8G8B8 && (theDevice.mSupportedTextureFormats & 2U) != 0U)
			{
				pixelFormat = PixelFormat.PixelFormat_A4R4G4B4;
			}
			if (pixelFormat == PixelFormat.PixelFormat_A8R8G8B8 && (theDevice.mSupportedTextureFormats & 1U) == 0U)
			{
				pixelFormat = PixelFormat.PixelFormat_A4R4G4B4;
			}
			bool flag = false;
			if (this.mWidth != theImage.mWidth || this.mHeight != theImage.mHeight || pixelFormat != this.mPixelFormat || theImage.GetImageFlags() != this.mImageFlags)
			{
				this.ReleaseTextures();
				this.mPixelFormat = pixelFormat;
				this.mImageFlags = theImage.GetImageFlags();
				flag = true;
			}
			int height = theImage.GetHeight();
			int width = theImage.GetWidth();
			if (this.mPaletteIndex != -1)
			{
				this.mTexMemSize += 1024;
				this.mTexMemOriginalSize += 1024;
			}
			int num = 4;
			if (pixelFormat == PixelFormat.PixelFormat_Palette8)
			{
				num = 1;
			}
			else if (pixelFormat == PixelFormat.PixelFormat_R5G6B5)
			{
				num = 2;
			}
			else if (pixelFormat == PixelFormat.PixelFormat_A4R4G4B4)
			{
				num = 2;
			}
			if ((this.mImageFlags & 32UL) != 0UL)
			{
				XNATextureDataPiece xnatextureDataPiece = this.mTextures[0];
				if (flag)
				{
					if (xnatextureDataPiece.mCubeTexture == null)
					{
						this.mPixelFormat = PixelFormat.PixelFormat_Unknown;
						return;
					}
					int num2 = theImage.GetWidth() * theImage.GetHeight() * num;
					this.mTexMemSize += num2;
					this.mTexMemOriginalSize += num2;
				}
				this.mWidth = theImage.GetWidth();
				this.mHeight = theImage.GetHeight();
				this.mBitsChangedCount = theImage.mBitsChangedCount;
				this.mPixelFormat = pixelFormat;
				return;
			}
			if ((this.mImageFlags & 64UL) != 0UL)
			{
				XNATextureDataPiece xnatextureDataPiece2 = this.mTextures[0];
				if (flag)
				{
					if (xnatextureDataPiece2.mVolumeTexture == null)
					{
						this.mPixelFormat = PixelFormat.PixelFormat_Unknown;
						return;
					}
					int num3 = theImage.GetWidth() * theImage.GetHeight() * num;
					this.mTexMemSize += num3;
					this.mTexMemOriginalSize += num3;
				}
				this.mWidth = theImage.GetWidth();
				this.mHeight = theImage.GetHeight();
				this.mBitsChangedCount = theImage.mBitsChangedCount;
				this.mPixelFormat = pixelFormat;
				return;
			}
			int num4 = 0;
			for (int i = 0; i < height; i += this.mTexPieceHeight)
			{
				int j = 0;
				while (j < width)
				{
					XNATextureDataPiece xnatextureDataPiece3 = this.mTextures[num4];
					if (flag)
					{
						xnatextureDataPiece3.mTexture = theDevice.CreateTexture2D(xnatextureDataPiece3.mWidth, xnatextureDataPiece3.mHeight, pixelFormat, false, this, this.mTextures);
						if (xnatextureDataPiece3.mTexture == null)
						{
							this.mPixelFormat = PixelFormat.PixelFormat_Unknown;
							return;
						}
						this.mTexMemSize += xnatextureDataPiece3.mWidth * xnatextureDataPiece3.mHeight * num;
					}
					if (theImage.HasImageFlag(16U))
					{
						if (theImage.mBits != null)
						{
						}
					}
					else if (commitBits)
					{
						this.mDevice.CopyImageToTexture(ref xnatextureDataPiece3.mTexture, xnatextureDataPiece3.mTexFormat, theImage, j, i, xnatextureDataPiece3.mWidth, xnatextureDataPiece3.mHeight, pixelFormat);
					}
					j += this.mTexPieceWidth;
					num4++;
				}
			}
			if (flag)
			{
				this.mTexMemOriginalSize += theImage.GetWidth() * theImage.GetHeight() * num;
			}
			this.mWidth = theImage.mWidth;
			this.mHeight = theImage.mHeight;
			this.mBitsChangedCount = theImage.mBitsChangedCount;
			this.mPixelFormat = pixelFormat;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00007924 File Offset: 0x00005B24
		private void CheckCreateTextures(ref MemoryImage theImage, ref BaseXNARenderDevice theDevice)
		{
			if (this.mPixelFormat == PixelFormat.PixelFormat_Unknown || theImage.mWidth != this.mWidth || theImage.mHeight != this.mHeight || theImage.mBitsChangedCount != this.mBitsChangedCount || theImage.GetImageFlags() != this.mImageFlags)
			{
				if (this.mOptimizedLoad)
				{
					this.mImageFlags = theImage.GetImageFlags();
					return;
				}
				this.CreateTextures(ref theImage, theDevice, true);
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00007998 File Offset: 0x00005B98
		public Texture2D GetTexture(MemoryImage theOrigImage, int x, int y, ref int width, ref int height, ref float u1, ref float v1, ref float u2, ref float v2)
		{
			if ((this.mImageFlags & 96UL) != 0UL)
			{
				return null;
			}
			int num = x / this.mTexPieceWidth;
			int num2 = y / this.mTexPieceHeight;
			XNATextureDataPiece xnatextureDataPiece = this.mTextures[num2 * this.mTexVecWidth + num];
			int num3 = x % this.mTexPieceWidth;
			int num4 = y % this.mTexPieceHeight;
			int num5 = num3 + width;
			int num6 = num4 + height;
			if (num5 > xnatextureDataPiece.mWidth)
			{
				num5 = xnatextureDataPiece.mWidth;
			}
			if (num6 > xnatextureDataPiece.mHeight)
			{
				num6 = xnatextureDataPiece.mHeight;
			}
			width = num5 - num3;
			height = num6 - num4;
			if ((this.mImageFlags & 512UL) != 0UL)
			{
				u1 = (float)num3 / (float)theOrigImage.mWidth;
				v1 = (float)num4 / (float)theOrigImage.mHeight;
				u2 = (float)num5 / (float)theOrigImage.mWidth;
				v2 = (float)num6 / (float)theOrigImage.mHeight;
			}
			else
			{
				u1 = (float)num3 / (float)xnatextureDataPiece.mWidth;
				v1 = (float)num4 / (float)xnatextureDataPiece.mHeight;
				u2 = (float)num5 / (float)xnatextureDataPiece.mWidth;
				v2 = (float)num6 / (float)xnatextureDataPiece.mHeight;
			}
			return xnatextureDataPiece.mTexture;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00007AB8 File Offset: 0x00005CB8
		private Texture2D GetTextureF(float x, float y, ref float width, ref float height, ref float u1, ref float v1, ref float u2, ref float v2)
		{
			if ((this.mImageFlags & 96UL) != 0UL)
			{
				return null;
			}
			int num = (int)(x / (float)this.mTexPieceWidth);
			int num2 = (int)(y / (float)this.mTexPieceHeight);
			XNATextureDataPiece xnatextureDataPiece = this.mTextures[num2 * this.mTexVecWidth + num];
			float num3 = x - (float)(num * this.mTexPieceWidth);
			float num4 = y - (float)(num2 * this.mTexPieceHeight);
			float num5 = num3 + width;
			float num6 = num4 + height;
			if (num5 > (float)xnatextureDataPiece.mWidth)
			{
				num5 = (float)xnatextureDataPiece.mWidth;
			}
			if (num6 > (float)xnatextureDataPiece.mHeight)
			{
				num6 = (float)xnatextureDataPiece.mHeight;
			}
			width = num5 - num3;
			height = num6 - num4;
			u1 = num3 / (float)xnatextureDataPiece.mWidth;
			v1 = num4 / (float)xnatextureDataPiece.mHeight;
			u2 = num5 / (float)xnatextureDataPiece.mWidth;
			v2 = num6 / (float)xnatextureDataPiece.mHeight;
			return xnatextureDataPiece.mTexture;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00007B97 File Offset: 0x00005D97
		private TextureCube GetCubeTexture()
		{
			if ((this.mImageFlags & 32UL) == 0UL)
			{
				return null;
			}
			return this.mTextures[0].mCubeTexture;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00007BB6 File Offset: 0x00005DB6
		private Texture3D GetVolumeTexture()
		{
			if ((this.mImageFlags & 64UL) == 0UL)
			{
				return null;
			}
			return this.mTextures[0].mVolumeTexture;
		}

		// Token: 0x0400011F RID: 287
		public XNATextureDataPiece[] mTextures;

		// Token: 0x04000120 RID: 288
		private BaseXNARenderDevice mDevice;

		// Token: 0x04000121 RID: 289
		public int mPaletteIndex;

		// Token: 0x04000122 RID: 290
		public bool mOptimizedLoad;

		// Token: 0x04000123 RID: 291
		public int mWidth;

		// Token: 0x04000124 RID: 292
		public int mHeight;

		// Token: 0x04000125 RID: 293
		public int mTexVecWidth;

		// Token: 0x04000126 RID: 294
		public int mTexVecHeight;

		// Token: 0x04000127 RID: 295
		public int mTexPieceWidth;

		// Token: 0x04000128 RID: 296
		public int mTexPieceHeight;

		// Token: 0x04000129 RID: 297
		public int mBitsChangedCount;

		// Token: 0x0400012A RID: 298
		public int mTexMemSize;

		// Token: 0x0400012B RID: 299
		public int mTexMemOriginalSize;

		// Token: 0x0400012C RID: 300
		public ulong mTexMemFlushRevision;

		// Token: 0x0400012D RID: 301
		public float mMaxTotalU;

		// Token: 0x0400012E RID: 302
		public float mMaxTotalV;

		// Token: 0x0400012F RID: 303
		public PixelFormat mPixelFormat;

		// Token: 0x04000130 RID: 304
		public ulong mImageFlags;
	}
}
