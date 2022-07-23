using System;
using SexyFramework.GraphicsLib;

namespace SexyFramework.Resource
{
	// Token: 0x0200018F RID: 399
	public class FontRes : BaseRes
	{
		// Token: 0x06000DDA RID: 3546 RVA: 0x00045CC0 File Offset: 0x00043EC0
		public FontRes()
		{
			this.mType = ResType.ResType_Font;
			this.mSysFont = false;
			this.mFont = null;
			this.mImage = null;
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x00045CF0 File Offset: 0x00043EF0
		public override void DeleteResource()
		{
			if (this.mResourceRef != null && this.mResourceRef.HasResource())
			{
				this.mResourceRef.Release();
			}
			if (this.mFont != null)
			{
				this.mFont.Dispose();
				this.mFont = null;
			}
			if (this.mImage != null)
			{
				this.mImage.Dispose();
				this.mImage = null;
			}
			if (this.mGlobalPtr != null)
			{
				this.mGlobalPtr.mResObject = null;
			}
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x00045D68 File Offset: 0x00043F68
		public override void ApplyConfig()
		{
			if (this.mFont == null)
			{
				return;
			}
			if (!this.mSysFont)
			{
				ImageFont imageFont = (ImageFont)this.mFont;
				if (this.mTags.Length > 0)
				{
					this.mTags.ToCharArray();
					string[] array = this.mTags.Split(new char[] { ',', ' ', '\r', '\t', '\n' });
					foreach (string theTagName in array)
					{
						imageFont.AddTag(theTagName);
					}
					imageFont.Prepare();
				}
			}
		}

		// Token: 0x04000B63 RID: 2915
		public Font mFont;

		// Token: 0x04000B64 RID: 2916
		public Image mImage;

		// Token: 0x04000B65 RID: 2917
		public string mImagePath;

		// Token: 0x04000B66 RID: 2918
		public string mTags = "";

		// Token: 0x04000B67 RID: 2919
		public bool mSysFont;

		// Token: 0x04000B68 RID: 2920
		public bool mBold;

		// Token: 0x04000B69 RID: 2921
		public bool mItalic;

		// Token: 0x04000B6A RID: 2922
		public bool mUnderline;

		// Token: 0x04000B6B RID: 2923
		public bool mShadow;

		// Token: 0x04000B6C RID: 2924
		public int mSize;
	}
}
