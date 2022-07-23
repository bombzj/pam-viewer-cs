using System;
using SexyFramework.GraphicsLib;
using SexyFramework.WidgetsLib;

namespace SexyFramework.Resource
{
	// Token: 0x02000195 RID: 405
	public class ResourceRef
	{
		// Token: 0x06000DE7 RID: 3559 RVA: 0x00045FC4 File Offset: 0x000441C4
		public ResourceRef()
		{
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00045FCC File Offset: 0x000441CC
		public ResourceRef(ResourceRef theResourceRef)
		{
			this.mBaseResP = theResourceRef.mBaseResP;
			if (this.mBaseResP != null)
			{
				this.mBaseResP.mRefCount++;
			}
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00045FFB File Offset: 0x000441FB
		public virtual void Dispose()
		{
			this.Release();
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00046003 File Offset: 0x00044203
		public virtual ResourceRef CopyFrom(ResourceRef theResourceRef)
		{
			this.Release();
			this.mBaseResP = theResourceRef.mBaseResP;
			if (this.mBaseResP != null)
			{
				this.mBaseResP.mRefCount++;
			}
			return this;
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00046033 File Offset: 0x00044233
		public bool HasResource()
		{
			return this.mBaseResP != null;
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00046041 File Offset: 0x00044241
		public void Release()
		{
			if (this.mBaseResP != null && this.mBaseResP.mParent != null)
			{
				this.mBaseResP.mParent.Deref(this.mBaseResP);
			}
			this.mBaseResP = null;
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x00046075 File Offset: 0x00044275
		public string GetId()
		{
			if (this.mBaseResP == null)
			{
				return "";
			}
			return this.mBaseResP.mId;
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x00046090 File Offset: 0x00044290
		public SharedImageRef GetSharedImageRef()
		{
			if (this.mBaseResP == null || this.mBaseResP.mType != ResType.ResType_Image)
			{
				return null;
			}
			return ((ImageRes)this.mBaseResP).mImage;
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x000460B9 File Offset: 0x000442B9
		public Image GetImage()
		{
			if (this.mBaseResP == null || this.mBaseResP.mType != ResType.ResType_Image)
			{
				return null;
			}
			return ((ImageRes)this.mBaseResP).mImage.GetImage();
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x000460E7 File Offset: 0x000442E7
		public MemoryImage GetMemoryImage()
		{
			if (this.mBaseResP == null || this.mBaseResP.mType != ResType.ResType_Image)
			{
				return null;
			}
			return ((ImageRes)this.mBaseResP).mImage.GetMemoryImage();
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00046115 File Offset: 0x00044315
		public MemoryImage GetDeviceImage()
		{
			if (this.mBaseResP == null || this.mBaseResP.mType != ResType.ResType_Image)
			{
				return null;
			}
			return ((ImageRes)this.mBaseResP).mImage.GetDeviceImage();
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x00046143 File Offset: 0x00044343
		public int GetSoundID()
		{
			if (this.mBaseResP == null || this.mBaseResP.mType != ResType.ResType_Sound)
			{
				return 0;
			}
			return ((SoundRes)this.mBaseResP).mSoundId;
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0004616D File Offset: 0x0004436D
		public Font GetFont()
		{
			if (this.mBaseResP == null || this.mBaseResP.mType != ResType.ResType_Font)
			{
				return null;
			}
			return ((FontRes)this.mBaseResP).mFont;
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00046197 File Offset: 0x00044397
		public ImageFont GetImageFont()
		{
			if (this.mBaseResP == null || this.mBaseResP.mType != ResType.ResType_Font)
			{
				return null;
			}
			return (ImageFont)((FontRes)this.mBaseResP).mFont;
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x000461C6 File Offset: 0x000443C6
		public PopAnim GetPopAnim()
		{
			if (this.mBaseResP == null || this.mBaseResP.mType != ResType.ResType_PopAnim)
			{
				return null;
			}
			return ((PopAnimRes)this.mBaseResP).mPopAnim;
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x000461F0 File Offset: 0x000443F0
		public PIEffect GetPIEffect()
		{
			if (this.mBaseResP == null || this.mBaseResP.mType != ResType.ResType_PIEffect)
			{
				return null;
			}
			return ((PIEffectRes)this.mBaseResP).mPIEffect;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0004621A File Offset: 0x0004441A
		public RenderEffectDefinition GetRenderEffect()
		{
			if (this.mBaseResP == null || this.mBaseResP.mType != ResType.ResType_RenderEffect)
			{
				return null;
			}
			return ((RenderEffectRes)this.mBaseResP).mRenderEffectDefinition;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00046244 File Offset: 0x00044444
		public GenericResFile GetGenericResFile()
		{
			if (this.mBaseResP == null || this.mBaseResP.mType != ResType.ResType_GenericResFile)
			{
				return null;
			}
			return ((GenericResFileRes)this.mBaseResP).mGenericResFile;
		}

		// Token: 0x04000B73 RID: 2931
		public BaseRes mBaseResP;
	}
}
