using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.Resource
{
	// Token: 0x0200018D RID: 397
	public class ImageRes : BaseRes
	{
		// Token: 0x06000DD4 RID: 3540 RVA: 0x00045944 File Offset: 0x00043B44
		public ImageRes()
		{
			this.mType = ResType.ResType_Image;
			this.mAtlasName = null;
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x000459AC File Offset: 0x00043BAC
		public override void DeleteResource()
		{
			if (this.mResourceRef != null && this.mResourceRef.HasResource())
			{
				this.mResourceRef.Release();
			}
			if (this.mGlobalPtr != null)
			{
				this.mGlobalPtr.mResObject = null;
			}
			this.mImage.Release();
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x000459F8 File Offset: 0x00043BF8
		public override void ApplyConfig()
		{
			if (this.mResourceRef != null && this.mResourceRef.HasResource())
			{
				return;
			}
			DeviceImage deviceImage = this.mImage.GetDeviceImage();
			if (deviceImage == null)
			{
				return;
			}
			deviceImage.ReplaceImageFlags(0U);
			if (this.mNoTriRep)
			{
				deviceImage.AddImageFlags(ImageFlags.ImageFlag_NoTriRep);
			}
			deviceImage.mNumRows = this.mRows;
			deviceImage.mNumCols = this.mCols;
			if (this.mDither16)
			{
				deviceImage.mDither16 = true;
			}
			if (this.mA4R4G4B4)
			{
				deviceImage.AddImageFlags(ImageFlags.ImageFlag_UseA4R4G4B4);
			}
			if (this.mA8R8G8B8)
			{
				deviceImage.AddImageFlags(ImageFlags.ImageFlag_UseA8R8G8B8);
			}
			if (this.mMinimizeSubdivisions)
			{
				deviceImage.AddImageFlags(ImageFlags.ImageFlag_MinimizeNumSubdivisions);
			}
			if (this.mCubeMap)
			{
				deviceImage.AddImageFlags(ImageFlags.ImageFlag_CubeMap);
			}
			else if (this.mVolumeMap)
			{
				deviceImage.AddImageFlags(ImageFlags.ImageFlag_VolumeMap);
			}
			if (this.mAnimInfo.mAnimType != AnimType.AnimType_None)
			{
				deviceImage.mAnimInfo = new AnimInfo(this.mAnimInfo);
			}
			if (this.mIsAtlas)
			{
				deviceImage.AddImageFlags(513U);
			}
			if (this.mAtlasName != null)
			{
				deviceImage.mAtlasImage = GlobalMembers.gSexyAppBase.mResourceManager.LoadImage(this.mAtlasName).GetImage();
				deviceImage.mAtlasStartX = this.mAtlasX;
				deviceImage.mAtlasStartY = this.mAtlasY;
				deviceImage.mAtlasEndX = this.mAtlasX + this.mAtlasW;
				deviceImage.mAtlasEndY = this.mAtlasY + this.mAtlasH;
			}
			deviceImage.CommitBits();
			deviceImage.mPurgeBits = this.mPurgeBits;
			if (this.mDDSurface)
			{
				deviceImage.CommitBits();
				if (!deviceImage.mHasAlpha)
				{
					deviceImage.mWantDeviceSurface = true;
					deviceImage.mPurgeBits = true;
				}
			}
			if (deviceImage.mPurgeBits)
			{
				new AutoCrit(GlobalMembers.gSexyAppBase.mImageSetCritSect);
				deviceImage.PurgeBits();
			}
		}

		// Token: 0x04000B45 RID: 2885
		public SharedImageRef mImage = new SharedImageRef();

		// Token: 0x04000B46 RID: 2886
		public string mAlphaImage = "";

		// Token: 0x04000B47 RID: 2887
		public string mAlphaGridImage = "";

		// Token: 0x04000B48 RID: 2888
		public string mVariant = "";

		// Token: 0x04000B49 RID: 2889
		public SexyPoint mOffset;

		// Token: 0x04000B4A RID: 2890
		public bool mAutoFindAlpha;

		// Token: 0x04000B4B RID: 2891
		public bool mPalletize;

		// Token: 0x04000B4C RID: 2892
		public bool mA4R4G4B4;

		// Token: 0x04000B4D RID: 2893
		public bool mA8R8G8B8;

		// Token: 0x04000B4E RID: 2894
		public bool mDither16;

		// Token: 0x04000B4F RID: 2895
		public bool mDDSurface;

		// Token: 0x04000B50 RID: 2896
		public bool mPurgeBits;

		// Token: 0x04000B51 RID: 2897
		public bool mMinimizeSubdivisions;

		// Token: 0x04000B52 RID: 2898
		public bool mCubeMap;

		// Token: 0x04000B53 RID: 2899
		public bool mVolumeMap;

		// Token: 0x04000B54 RID: 2900
		public bool mNoTriRep;

		// Token: 0x04000B55 RID: 2901
		public bool m2DBig;

		// Token: 0x04000B56 RID: 2902
		public bool mIsAtlas;

		// Token: 0x04000B57 RID: 2903
		public int mRows = 1;

		// Token: 0x04000B58 RID: 2904
		public int mCols = 1;

		// Token: 0x04000B59 RID: 2905
		public int mAlphaColor;

		// Token: 0x04000B5A RID: 2906
		public AnimInfo mAnimInfo = new AnimInfo();

		// Token: 0x04000B5B RID: 2907
		public string mAtlasName;

		// Token: 0x04000B5C RID: 2908
		public int mAtlasX;

		// Token: 0x04000B5D RID: 2909
		public int mAtlasY;

		// Token: 0x04000B5E RID: 2910
		public int mAtlasW;

		// Token: 0x04000B5F RID: 2911
		public int mAtlasH;
	}
}
