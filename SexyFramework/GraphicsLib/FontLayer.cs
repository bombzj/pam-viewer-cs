using System;
using System.Collections.Generic;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000C9 RID: 201
	public class FontLayer
	{
		// Token: 0x06000622 RID: 1570 RVA: 0x00015D24 File Offset: 0x00013F24
		public FontLayer()
		{
			this.mFontData = null;
			this.mDrawMode = -1;
			this.mSpacing = 0;
			this.mPointSize = 0;
			this.mAscent = 0;
			this.mAscentPadding = 0;
			this.mMinPointSize = -1;
			this.mMaxPointSize = -1;
			this.mHeight = 0;
			this.mDefaultHeight = 0;
			this.mColorMult = new SexyColor(SexyColor.White);
			this.mColorAdd = new SexyColor(0, 0, 0, 0);
			this.mLineSpacingOffset = 0;
			this.mBaseOrder = 0;
			this.mImageIsWhite = false;
			this.mUseAlphaCorrection = true;
			this.mCharDataHashTable.mOrderedHash = ImageFont.mOrderedHash;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00015E30 File Offset: 0x00014030
		public FontLayer(FontData theFontData)
		{
			this.mFontData = theFontData;
			this.mDrawMode = -1;
			this.mSpacing = 0;
			this.mPointSize = 0;
			this.mAscent = 0;
			this.mAscentPadding = 0;
			this.mMinPointSize = -1;
			this.mMaxPointSize = -1;
			this.mHeight = 0;
			this.mDefaultHeight = 0;
			this.mColorMult = new SexyColor(SexyColor.White);
			this.mColorAdd = new SexyColor(0, 0, 0, 0);
			this.mLineSpacingOffset = 0;
			this.mBaseOrder = 0;
			this.mImageIsWhite = false;
			this.mUseAlphaCorrection = true;
			this.mCharDataHashTable.mOrderedHash = ImageFont.mOrderedHash;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00015F3C File Offset: 0x0001413C
		public FontLayer(FontLayer theFontLayer)
		{
			this.mFontData = theFontLayer.mFontData;
			this.mRequiredTags = theFontLayer.mRequiredTags;
			this.mExcludedTags = theFontLayer.mExcludedTags;
			this.mImage = new SharedImageRef(theFontLayer.mImage);
			this.mImageIsWhite = theFontLayer.mImageIsWhite;
			this.mDrawMode = theFontLayer.mDrawMode;
			this.mOffset = theFontLayer.mOffset;
			this.mSpacing = theFontLayer.mSpacing;
			this.mMinPointSize = theFontLayer.mMinPointSize;
			this.mMaxPointSize = theFontLayer.mMaxPointSize;
			this.mPointSize = theFontLayer.mPointSize;
			this.mAscent = theFontLayer.mAscent;
			this.mAscentPadding = theFontLayer.mAscentPadding;
			this.mHeight = theFontLayer.mHeight;
			this.mDefaultHeight = theFontLayer.mDefaultHeight;
			this.mColorMult = new SexyColor(theFontLayer.mColorMult);
			this.mColorAdd = new SexyColor(theFontLayer.mColorAdd);
			this.mLineSpacingOffset = theFontLayer.mLineSpacingOffset;
			this.mBaseOrder = theFontLayer.mBaseOrder;
			this.mExtendedInfo = theFontLayer.mExtendedInfo;
			this.mKerningData = theFontLayer.mKerningData;
			this.mCharDataHashTable = theFontLayer.mCharDataHashTable;
			this.mUseAlphaCorrection = theFontLayer.mUseAlphaCorrection;
			this.mLayerName = theFontLayer.mLayerName;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000160E3 File Offset: 0x000142E3
		public CharData GetCharData(char theChar)
		{
			return this.mCharDataHashTable.GetCharData(theChar, true);
		}

		// Token: 0x04000503 RID: 1283
		public string mLayerName;

		// Token: 0x04000504 RID: 1284
		public FontData mFontData;

		// Token: 0x04000505 RID: 1285
		public Dictionary<string, string> mExtendedInfo = new Dictionary<string, string>();

		// Token: 0x04000506 RID: 1286
		public List<string> mRequiredTags = new List<string>();

		// Token: 0x04000507 RID: 1287
		public List<string> mExcludedTags = new List<string>();

		// Token: 0x04000508 RID: 1288
		public List<FontLayer.KerningValue> mKerningData = new List<FontLayer.KerningValue>();

		// Token: 0x04000509 RID: 1289
		public CharDataHashTable mCharDataHashTable = new CharDataHashTable();

		// Token: 0x0400050A RID: 1290
		public SexyColor mColorMult = default(SexyColor);

		// Token: 0x0400050B RID: 1291
		public SexyColor mColorAdd = default(SexyColor);

		// Token: 0x0400050C RID: 1292
		public SharedImageRef mImage = new SharedImageRef();

		// Token: 0x0400050D RID: 1293
		public bool mImageIsWhite;

		// Token: 0x0400050E RID: 1294
		public string mImageFileName;

		// Token: 0x0400050F RID: 1295
		public int mDrawMode;

		// Token: 0x04000510 RID: 1296
		public SexyPoint mOffset = new SexyPoint();

		// Token: 0x04000511 RID: 1297
		public int mSpacing;

		// Token: 0x04000512 RID: 1298
		public int mMinPointSize;

		// Token: 0x04000513 RID: 1299
		public int mMaxPointSize;

		// Token: 0x04000514 RID: 1300
		public int mPointSize;

		// Token: 0x04000515 RID: 1301
		public int mAscent;

		// Token: 0x04000516 RID: 1302
		public int mAscentPadding;

		// Token: 0x04000517 RID: 1303
		public int mHeight;

		// Token: 0x04000518 RID: 1304
		public int mDefaultHeight;

		// Token: 0x04000519 RID: 1305
		public int mLineSpacingOffset;

		// Token: 0x0400051A RID: 1306
		public int mBaseOrder;

		// Token: 0x0400051B RID: 1307
		public bool mUseAlphaCorrection;

		// Token: 0x020000CA RID: 202
		public struct KerningValue
		{
			// Token: 0x0400051C RID: 1308
			public int mInt;

			// Token: 0x0400051D RID: 1309
			public ushort mChar;

			// Token: 0x0400051E RID: 1310
			public short mOffset;
		}
	}
}
