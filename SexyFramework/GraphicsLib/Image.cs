using System;
using Microsoft.Xna.Framework;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x02000083 RID: 131
	public class Image : IDisposable
	{
		// Token: 0x0600049A RID: 1178 RVA: 0x0000D9CC File Offset: 0x0000BBCC
		public Image()
		{
			this.mImageFlags = ImageFlags.ImageFlag_NONE;
			this.mRenderData = null;
			this.mAtlasImage = null;
			this.mAtlasStartX = 0;
			this.mAtlasStartY = 0;
			this.mAtlasEndX = 0;
			this.mAtlasEndY = 0;
			this.mWidth = 0;
			this.mHeight = 0;
			this.mNumRows = 1;
			this.mNumCols = 1;
			this.mAnimInfo = null;
			this.mDrawn = false;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0000DAB8 File Offset: 0x0000BCB8
		public Image(Image theImage)
		{
			this.mImageFlags = theImage.mImageFlags;
			this.mRenderData = null;
			this.mWidth = theImage.mWidth;
			this.mHeight = theImage.mHeight;
			this.mNumRows = theImage.mNumRows;
			this.mNumCols = theImage.mNumCols;
			this.mAtlasImage = theImage.mAtlasImage;
			this.mAtlasStartX = theImage.mAtlasStartX;
			this.mAtlasStartY = theImage.mAtlasStartY;
			this.mAtlasEndX = theImage.mAtlasEndX;
			this.mAtlasEndY = theImage.mAtlasEndY;
			this.mDrawn = false;
			if (theImage.mAnimInfo != null)
			{
				this.mAnimInfo = theImage.mAnimInfo;
				return;
			}
			this.mAnimInfo = null;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0000DBEC File Offset: 0x0000BDEC
		public void InitAtalasState()
		{
			if (this.mAtlasValidate)
			{
				return;
			}
			if (this.mAtlasImage != null)
			{
				float num = (float)this.mAtlasStartX / (float)this.mAtlasImage.mWidth;
				float num2 = (float)this.mAtlasStartY / (float)this.mAtlasImage.mHeight;
				float num3 = (float)this.mAtlasEndX / (float)this.mAtlasImage.mWidth;
				float num4 = (float)this.mAtlasEndY / (float)this.mAtlasImage.mHeight;
				this.mVectorBase = new Vector2(num, num2);
				if (num4 < num2)
				{
					this.mVectorU = new Vector2(num, num4) - this.mVectorBase;
					this.mVectorV = new Vector2(num3, num2) - this.mVectorBase;
				}
				else
				{
					this.mVectorU = new Vector2(num3, num2) - this.mVectorBase;
					this.mVectorV = new Vector2(num, num4) - this.mVectorBase;
				}
			}
			this.mAtlasValidate = true;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0000DCDB File Offset: 0x0000BEDB
		public virtual void Dispose()
		{
			if (this.mAnimInfo != null)
			{
				this.mAnimInfo.Dispose();
			}
			this.mAnimInfo = null;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000DCF7 File Offset: 0x0000BEF7
		public virtual MemoryImage AsMemoryImage()
		{
			return null;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0000DCFA File Offset: 0x0000BEFA
		public virtual DeviceImage AsDeviceImage()
		{
			return null;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0000DCFD File Offset: 0x0000BEFD
		public int GetWidth()
		{
			return this.mWidth;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0000DD05 File Offset: 0x0000BF05
		public int GetHeight()
		{
			return this.mHeight;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0000DD0D File Offset: 0x0000BF0D
		public Rect GetRect()
		{
			return new Rect(0, 0, this.mWidth, this.mHeight);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0000DD22 File Offset: 0x0000BF22
		public int GetCelWidth()
		{
			if (this.mCelWidth == -1)
			{
				this.mCelWidth = this.mWidth / this.mNumCols;
			}
			return this.mCelWidth;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000DD46 File Offset: 0x0000BF46
		public int GetCelHeight()
		{
			if (this.mCelHeight == -1)
			{
				this.mCelHeight = this.mHeight / this.mNumRows;
			}
			return this.mCelHeight;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0000DD6A File Offset: 0x0000BF6A
		public int GetCelCount()
		{
			return this.mNumCols * this.mNumRows;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0000DD79 File Offset: 0x0000BF79
		public int GetAnimCel(int theTime)
		{
			if (this.mAnimInfo == null)
			{
				return 0;
			}
			return this.mAnimInfo.GetCel(theTime);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0000DD94 File Offset: 0x0000BF94
		public Rect GetAnimCelRect(int theTime)
		{
			int animCel = this.GetAnimCel(theTime);
			int celWidth = this.GetCelWidth();
			int celHeight = this.GetCelHeight();
			if (this.mNumCols > 1)
			{
				return new Rect(animCel * celWidth, 0, celWidth, this.mHeight);
			}
			return new Rect(0, animCel * celHeight, this.mWidth, celHeight);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000DDE4 File Offset: 0x0000BFE4
		public Rect GetCelRect(int theCel)
		{
			this.mCelRect.mHeight = this.GetCelHeight();
			this.mCelRect.mWidth = this.GetCelWidth();
			this.mCelRect.mX = theCel % this.mNumCols * this.mCelRect.mWidth;
			this.mCelRect.mY = theCel / this.mNumCols * this.mCelRect.mHeight;
			return this.mCelRect;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0000DE58 File Offset: 0x0000C058
		public Rect GetCelRect(int theCol, int theRow)
		{
			this.mCelRect.mHeight = this.GetCelHeight();
			this.mCelRect.mWidth = this.GetCelWidth();
			this.mCelRect.mX = theCol * this.mCelRect.mWidth;
			this.mCelRect.mY = theRow * this.mCelRect.mHeight;
			return this.mCelRect;
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0000DEBD File Offset: 0x0000C0BD
		public void CopyAttributes(Image from)
		{
			this.mNumCols = from.mNumCols;
			this.mNumRows = from.mNumRows;
			this.mAnimInfo = null;
			if (from.mAnimInfo != null)
			{
				this.mAnimInfo = new AnimInfo(from.mAnimInfo);
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0000DEF7 File Offset: 0x0000C0F7
		public void ReplaceImageFlags(uint inFlags)
		{
			this.mImageFlags = (ImageFlags)inFlags;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0000DF00 File Offset: 0x0000C100
		public void AddImageFlags(uint inFlags)
		{
			this.mImageFlags |= (ImageFlags)inFlags;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0000DF10 File Offset: 0x0000C110
		public void RemoveImageFlags(uint inFlags)
		{
			this.mImageFlags &= (ImageFlags)(~(ImageFlags)inFlags);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0000DF21 File Offset: 0x0000C121
		public bool HasImageFlag(uint inFlag)
		{
			return (this.mImageFlags & (ImageFlags)inFlag) != ImageFlags.ImageFlag_NONE;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0000DF31 File Offset: 0x0000C131
		public ulong GetImageFlags()
		{
			return (ulong)((long)this.mImageFlags);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0000DF3A File Offset: 0x0000C13A
		public object GetRenderData()
		{
			return this.mRenderData;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0000DF42 File Offset: 0x0000C142
		public void SetRenderData(object inRenderData)
		{
			this.mRenderData = inRenderData;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0000DF4C File Offset: 0x0000C14C
		public bool CreateRenderData()
		{
			MemoryImage memoryImage = this.AsMemoryImage();
			if (memoryImage != null && GlobalMembers.gSexyAppBase.mGraphicsDriver != null && GlobalMembers.gSexyAppBase.mGraphicsDriver.GetRenderDevice3D() != null)
			{
				this.mRenderData = GlobalMembers.gSexyAppBase.mAppDriver.GetOptimizedRenderData(memoryImage.mFileName);
				if (this.mRenderData != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400029E RID: 670
		protected ImageFlags mImageFlags;

		// Token: 0x0400029F RID: 671
		public object mRenderData;

		// Token: 0x040002A0 RID: 672
		public string mFileName;

		// Token: 0x040002A1 RID: 673
		public string mNameForRes = "";

		// Token: 0x040002A2 RID: 674
		public bool mDrawn;

		// Token: 0x040002A3 RID: 675
		public string mFilePath;

		// Token: 0x040002A4 RID: 676
		public int mWidth;

		// Token: 0x040002A5 RID: 677
		public int mHeight;

		// Token: 0x040002A6 RID: 678
		public Rect mCelRect = default(Rect);

		// Token: 0x040002A7 RID: 679
		public Rect mRect = Rect.ZERO_RECT;

		// Token: 0x040002A8 RID: 680
		public int mNumRows = 1;

		// Token: 0x040002A9 RID: 681
		public int mNumCols = 1;

		// Token: 0x040002AA RID: 682
		public int mCelWidth = -1;

		// Token: 0x040002AB RID: 683
		public int mCelHeight = -1;

		// Token: 0x040002AC RID: 684
		public AnimInfo mAnimInfo;

		// Token: 0x040002AD RID: 685
		public Image mAtlasImage;

		// Token: 0x040002AE RID: 686
		public int mAtlasStartX;

		// Token: 0x040002AF RID: 687
		public int mAtlasStartY;

		// Token: 0x040002B0 RID: 688
		public int mAtlasEndX;

		// Token: 0x040002B1 RID: 689
		public int mAtlasEndY;

		// Token: 0x040002B2 RID: 690
		public Vector2 mVectorU = new Vector2(1f, 0f);

		// Token: 0x040002B3 RID: 691
		public Vector2 mVectorV = new Vector2(0f, 1f);

		// Token: 0x040002B4 RID: 692
		public Vector2 mVectorBase = new Vector2(0f, 0f);

		// Token: 0x040002B5 RID: 693
		public bool mAtlasValidate;
	}
}
