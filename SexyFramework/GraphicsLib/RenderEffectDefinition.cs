using System;
using System.Collections.Generic;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000F6 RID: 246
	public class RenderEffectDefinition
	{
		// Token: 0x06000727 RID: 1831 RVA: 0x00025E37 File Offset: 0x00024037
		public bool LoadFromMem(uint inDataLen, byte[] inData, string inSrcFileName, string inDataFormat)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00025E3E File Offset: 0x0002403E
		public bool LoadFromFile(string inFileName, string inSrcFileName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00025E45 File Offset: 0x00024045
		public virtual void Dispose()
		{
			this.mData = null;
		}

		// Token: 0x040006C8 RID: 1736
		public List<byte> mData = new List<byte>();

		// Token: 0x040006C9 RID: 1737
		public string mSrcFileName;

		// Token: 0x040006CA RID: 1738
		public string mDataFormat;
	}
}
