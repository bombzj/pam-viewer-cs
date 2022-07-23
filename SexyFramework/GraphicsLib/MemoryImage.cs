using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SexyFramework.Drivers.Graphics;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x02000084 RID: 132
	public class MemoryImage : Image
	{
		// Token: 0x060004B3 RID: 1203 RVA: 0x0000DFA6 File Offset: 0x0000C1A6
		public MemoryImage()
		{
			this.mApp = GlobalMembers.gSexyAppBase;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0000DFD0 File Offset: 0x0000C1D0
		public MemoryImage(MemoryImage rhs)
			: base(rhs)
		{
			this.mApp = rhs.mApp;
			this.mHasAlpha = rhs.mHasAlpha;
			this.mHasTrans = rhs.mHasTrans;
			this.mBitsChanged = rhs.mBitsChanged;
			this.mIsVolatile = rhs.mIsVolatile;
			this.mPurgeBits = rhs.mPurgeBits;
			this.mWantPal = rhs.mWantPal;
			this.mBitsChangedCount = rhs.mBitsChangedCount;
			if (rhs.mBits == null && rhs.mColorTable == null)
			{
				rhs.GetBits();
			}
			if (rhs.mBits == null)
			{
				this.mBits = null;
			}
			if (rhs.mColorTable == null)
			{
				this.mColorTable = null;
			}
			if (rhs.mColorIndices == null)
			{
				this.mColorIndices = null;
			}
			if (rhs.mNativeAlphaData != null)
			{
				if (rhs.mColorTable == null)
				{
				}
			}
			else
			{
				this.mNativeAlphaData = null;
			}
			if (rhs.mRLAlphaData != null)
			{
				this.mRLAlphaData = new byte[this.mWidth * this.mHeight];
			}
			else
			{
				this.mRLAlphaData = null;
			}
			if (rhs.mRLAdditiveData != null)
			{
				this.mRLAdditiveData = new byte[this.mWidth * this.mHeight];
			}
			else
			{
				this.mRLAdditiveData = null;
			}
			this.mApp.AddMemoryImage(this);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0000E11D File Offset: 0x0000C31D
		public override void Dispose()
		{
			base.Dispose();
			if (this.mRenderData != null)
			{
				GlobalMembers.gSexyAppBase.mGraphicsDriver.Remove3DData(this);
			}
			this.mRenderData = null;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0000E144 File Offset: 0x0000C344
		private void Init()
		{
			this.mBits = null;
			this.mColorTable = null;
			this.mColorIndices = null;
			this.mNativeAlphaData = null;
			this.mRLAlphaData = null;
			this.mRLAdditiveData = null;
			this.mHasTrans = false;
			this.mHasAlpha = false;
			this.mBitsChanged = false;
			this.mForcedMode = false;
			this.mIsVolatile = false;
			this.mBitsChangedCount = 0;
			this.mPurgeBits = false;
			this.mWantPal = false;
			this.mDither16 = false;
			this.mApp.AddMemoryImage(this);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0000E1C6 File Offset: 0x0000C3C6
		public virtual object GetNativeAlphaData(NativeDisplay theDisplay)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0000E1CD File Offset: 0x0000C3CD
		public virtual byte[] GetRLAlphaData()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0000E1D4 File Offset: 0x0000C3D4
		public virtual byte[] GetRLAdditiveData(NativeDisplay theNative)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0000E1DC File Offset: 0x0000C3DC
		public virtual void PurgeBits()
		{
			this.mPurgeBits = true;
			if (this.mApp.Is3DAccelerated())
			{
				if (base.GetRenderData() == null)
				{
					return;
				}
			}
			else
			{
				if (this.mBits == null && this.mColorIndices == null)
				{
					return;
				}
				this.GetNativeAlphaData(GlobalMembers.gSexyAppBase.mGraphicsDriver.GetNativeDisplayInfo());
			}
			this.mBits = null;
			this.mBits = null;
			if (base.GetRenderData() != null)
			{
				this.mColorIndices = null;
				this.mColorIndices = null;
				this.mColorTable = null;
				this.mColorTable = null;
			}
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0000E260 File Offset: 0x0000C460
		public virtual void DeleteSWBuffers()
		{
			if (this.mNativeAlphaData == null && this.mRLAdditiveData == null && this.mRLAlphaData == null)
			{
				return;
			}
			if (this.mBits == null && this.mColorIndices == null)
			{
				this.GetBits();
			}
			this.mNativeAlphaData = null;
			this.mNativeAlphaData = null;
			this.mRLAdditiveData = null;
			this.mRLAdditiveData = null;
			this.mRLAlphaData = null;
			this.mRLAlphaData = null;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0000E2C7 File Offset: 0x0000C4C7
		public virtual void Create(int theWidth, int theHeight)
		{
			this.mBits = null;
			this.mBits = null;
			this.mWidth = theWidth;
			this.mHeight = theHeight;
			this.mHasTrans = true;
			this.mHasAlpha = true;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0000E2F3 File Offset: 0x0000C4F3
		public override MemoryImage AsMemoryImage()
		{
			return this;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0000E2F8 File Offset: 0x0000C4F8
		public uint[] GetBits()
		{
			if (this.mBits == null)
			{
				int num = this.mWidth * this.mHeight;
				this.mBits = new uint[num];
				if (this.mColorTable != null)
				{
					for (int i = 0; i < num; i++)
					{
						this.mBits[i] = this.mColorTable[(int)this.mColorIndices[i]];
					}
					this.mColorIndices = null;
					this.mColorIndices = null;
					this.mColorTable = null;
					this.mColorTable = null;
					this.mNativeAlphaData = null;
					this.mNativeAlphaData = null;
				}
				else if (this.mNativeAlphaData == null)
				{
					if (base.GetRenderData() != null && (base.GetRenderData() as XNATextureData).mTextures[0].mTexture != null)
					{
						(base.GetRenderData() as XNATextureData).mTextures[0].mTexture.GetData<uint>(this.mBits);
					}
					else
					{
						MemoryImage memoryImage = ((this.mAtlasImage != null) ? this.mAtlasImage.AsMemoryImage() : null);
						if (memoryImage != null)
						{
							uint[] bits = memoryImage.GetBits();
							Array.Copy(bits, this.mAtlasStartY * memoryImage.mWidth + this.mAtlasStartX, this.mBits, 0, this.mBits.Length);
						}
					}
				}
			}
			return this.mBits;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0000E424 File Offset: 0x0000C624
		public virtual void SetBits(uint[] theBits, int theWidth, int theHeight)
		{
			this.SetBits(theBits, theWidth, theHeight, true);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0000E430 File Offset: 0x0000C630
		public virtual void SetBits(uint[] theBits, int theWidth, int theHeight, bool commitBits)
		{
			this.mColorIndices = null;
			this.mColorIndices = null;
			this.mColorTable = null;
			this.mColorTable = null;
			this.mBits = null;
			this.mBits = new uint[theWidth * theHeight];
			this.mWidth = theWidth;
			this.mHeight = theHeight;
			this.mBits = theBits;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0000E483 File Offset: 0x0000C683
		public virtual bool Palletize()
		{
			return true;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0000E486 File Offset: 0x0000C686
		public static int GetWinding(int p0x, int p0y, int p1x, int p1y, int p2x, int p2y)
		{
			return (p1x - p0x) * (p2y - p0y) - (p1y - p0y) * (p2x - p0x);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0000E499 File Offset: 0x0000C699
		public static void AddTri(ref List<MemoryImage.TriRep.Tri> outTris, Vector2[] inTri, int inWidth, int inHeight, int inGroup)
		{
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0000E49B File Offset: 0x0000C69B
		public virtual void SetImageMode(bool hasTrans, bool hasAlpha)
		{
			this.mForcedMode = true;
			this.mHasTrans = hasTrans;
			this.mHasAlpha = hasAlpha;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0000E4B2 File Offset: 0x0000C6B2
		public virtual void BitsChanged()
		{
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0000E4B4 File Offset: 0x0000C6B4
		internal void Clear()
		{
		}

		// Token: 0x040002B6 RID: 694
		public uint[] mBits;

		// Token: 0x040002B7 RID: 695
		public int mBitsChangedCount;

		// Token: 0x040002B8 RID: 696
		public uint[] mColorTable;

		// Token: 0x040002B9 RID: 697
		public byte[] mColorIndices;

		// Token: 0x040002BA RID: 698
		public bool mForcedMode;

		// Token: 0x040002BB RID: 699
		public bool mHasTrans;

		// Token: 0x040002BC RID: 700
		public bool mHasAlpha;

		// Token: 0x040002BD RID: 701
		public bool mIsVolatile;

		// Token: 0x040002BE RID: 702
		public bool mPurgeBits;

		// Token: 0x040002BF RID: 703
		public bool mWantPal;

		// Token: 0x040002C0 RID: 704
		public bool mDither16;

		// Token: 0x040002C1 RID: 705
		public uint[] mNativeAlphaData;

		// Token: 0x040002C2 RID: 706
		public byte[] mRLAlphaData;

		// Token: 0x040002C3 RID: 707
		public byte[] mRLAdditiveData;

		// Token: 0x040002C4 RID: 708
		public bool mBitsChanged;

		// Token: 0x040002C5 RID: 709
		public SexyAppBase mApp;

		// Token: 0x040002C6 RID: 710
		public MemoryImage.TriRep mNormalTriRep = new MemoryImage.TriRep();

		// Token: 0x040002C7 RID: 711
		public MemoryImage.TriRep mAdditiveTriRep = new MemoryImage.TriRep();

		// Token: 0x02000085 RID: 133
		public class TriRep
		{
			// Token: 0x060004C7 RID: 1223 RVA: 0x0000E4B6 File Offset: 0x0000C6B6
			public MemoryImage.TriRep.Level GetMinLevel()
			{
				if (this.mLevels.Count != 0)
				{
					return this.mLevels[0];
				}
				return null;
			}

			// Token: 0x060004C8 RID: 1224 RVA: 0x0000E4D3 File Offset: 0x0000C6D3
			public MemoryImage.TriRep.Level GetMaxLevel()
			{
				if (this.mLevels.Count != 0)
				{
					return this.mLevels[this.mLevels.Count - 1];
				}
				return null;
			}

			// Token: 0x060004C9 RID: 1225 RVA: 0x0000E4FC File Offset: 0x0000C6FC
			public MemoryImage.TriRep.Level GetLevelForScreenSpaceUsage(float inUsageFrac, int inAllowRotation)
			{
				if (this.mLevels.Count == 0)
				{
					return null;
				}
				for (int i = this.mLevels.Count - 1; i >= 0; i--)
				{
					MemoryImage.TriRep.Level result = this.mLevels[i];
					if (inUsageFrac > 0.001f)
					{
						return result;
					}
				}
				return null;
			}

			// Token: 0x040002C8 RID: 712
			public List<MemoryImage.TriRep.Level> mLevels = new List<MemoryImage.TriRep.Level>();

			// Token: 0x02000086 RID: 134
			public class Tri
			{
				// Token: 0x060004CB RID: 1227 RVA: 0x0000E55B File Offset: 0x0000C75B
				public Tri()
				{
				}

				// Token: 0x060004CC RID: 1228 RVA: 0x0000E570 File Offset: 0x0000C770
				public Tri(float inU0, float inV0, float inU1, float inV1, float inU2, float inV2)
				{
					this.p[0].u = inU0;
					this.p[0].v = inV0;
					this.p[1].u = inU1;
					this.p[1].v = inV1;
					this.p[2].u = inU2;
					this.p[2].v = inV2;
				}

				// Token: 0x040002C9 RID: 713
				public MemoryImage.TriRep.Tri.Point[] p = new MemoryImage.TriRep.Tri.Point[3];

				// Token: 0x02000087 RID: 135
				public class Point
				{
					// Token: 0x040002CA RID: 714
					public float u;

					// Token: 0x040002CB RID: 715
					public float v;
				}
			}

			// Token: 0x02000088 RID: 136
			public class Level
			{
				// Token: 0x060004CE RID: 1230 RVA: 0x0000E5F0 File Offset: 0x0000C7F0
				public void GetRegionTris(ref List<MemoryImage.TriRep.Tri> outTris, MemoryImage inImage, Rect inSrcRect, int inAllowRotation)
				{
					if (this.mRegions.Count == 0)
					{
						return;
					}
					if (this.mRegionWidth != inImage.mNumCols || this.mRegionHeight != inImage.mNumRows)
					{
						return;
					}
					int num = inImage.mWidth / this.mRegionWidth;
					int num2 = inImage.mHeight / this.mRegionHeight;
					if (inSrcRect.mWidth != num || inSrcRect.mHeight != num2)
					{
						return;
					}
					int num3 = inSrcRect.mX / num;
					int num4 = inSrcRect.mY / num2;
					if (num3 < this.mRegionWidth && num4 < this.mRegionHeight)
					{
						MemoryImage.TriRep.Level.Region region = this.mRegions[num4 * this.mRegionWidth + num3];
						outTris = region.mTris;
					}
				}

				// Token: 0x060004CF RID: 1231 RVA: 0x0000E6A0 File Offset: 0x0000C8A0
				public MemoryImage.TriRep.Tri GetRegionTrisPtr(ref int outTriCount, MemoryImage inImage, Rect inSrcRect, int inAllowRotation)
				{
					if (this.mRegions.Count == 0)
					{
						return null;
					}
					if (this.mRegionWidth != inImage.mNumCols || this.mRegionHeight != inImage.mNumRows)
					{
						return null;
					}
					int num = inImage.mWidth / this.mRegionWidth;
					int num2 = inImage.mHeight / this.mRegionHeight;
					if (inSrcRect.mWidth != num || inSrcRect.mHeight != num2)
					{
						return null;
					}
					int num3 = inSrcRect.mX / num;
					int num4 = inSrcRect.mY / num2;
					if (num3 < this.mRegionWidth && num4 < this.mRegionHeight)
					{
						MemoryImage.TriRep.Level.Region region = this.mRegions[num4 * this.mRegionWidth + num3];
						outTriCount = region.mTris.Count;
						return region.mTris[0];
					}
					return null;
				}

				// Token: 0x040002CC RID: 716
				public int mDetailX;

				// Token: 0x040002CD RID: 717
				public int mDetailY;

				// Token: 0x040002CE RID: 718
				public int mRegionWidth;

				// Token: 0x040002CF RID: 719
				public int mRegionHeight;

				// Token: 0x040002D0 RID: 720
				public List<MemoryImage.TriRep.Level.Region> mRegions = new List<MemoryImage.TriRep.Level.Region>();

				// Token: 0x02000089 RID: 137
				public class Region
				{
					// Token: 0x040002D1 RID: 721
					public Rect mRect = default(Rect);

					// Token: 0x040002D2 RID: 722
					public List<MemoryImage.TriRep.Tri> mTris = new List<MemoryImage.TriRep.Tri>();
				}
			}
		}
	}
}
