using System;
using System.Collections.Generic;

namespace SexyFramework.Resource
{
	// Token: 0x0200018C RID: 396
	public abstract class BaseRes
	{
		// Token: 0x06000DD1 RID: 3537 RVA: 0x00045924 File Offset: 0x00043B24
		public BaseRes()
		{
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0004593E File Offset: 0x00043B3E
		public virtual void DeleteResource()
		{
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x00045940 File Offset: 0x00043B40
		public virtual void ApplyConfig()
		{
		}

		// Token: 0x04000B35 RID: 2869
		public ResourceManager mParent;

		// Token: 0x04000B36 RID: 2870
		public ResourceRef mResourceRef;

		// Token: 0x04000B37 RID: 2871
		public ResGlobalPtr mGlobalPtr = new ResGlobalPtr();

		// Token: 0x04000B38 RID: 2872
		public int mGlobalIndex;

		// Token: 0x04000B39 RID: 2873
		public ResType mType = ResType.Num_ResTypes;

		// Token: 0x04000B3A RID: 2874
		public int mRefCount;

		// Token: 0x04000B3B RID: 2875
		public int mReloadIdx;

		// Token: 0x04000B3C RID: 2876
		public int mArtRes;

		// Token: 0x04000B3D RID: 2877
		public uint mLocSet;

		// Token: 0x04000B3E RID: 2878
		public bool mDirectLoaded;

		// Token: 0x04000B3F RID: 2879
		public bool mFromProgram;

		// Token: 0x04000B40 RID: 2880
		public string mId;

		// Token: 0x04000B41 RID: 2881
		public string mResGroup;

		// Token: 0x04000B42 RID: 2882
		public string mCompositeResGroup;

		// Token: 0x04000B43 RID: 2883
		public string mPath;

		// Token: 0x04000B44 RID: 2884
		public Dictionary<string, string> mXMLAttributes;
	}
}
