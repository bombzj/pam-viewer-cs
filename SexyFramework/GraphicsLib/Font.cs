using System;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000AA RID: 170
	public abstract class Font
	{
		// Token: 0x060004FB RID: 1275 RVA: 0x0000E9FB File Offset: 0x0000CBFB
		public Font()
		{
			this.mAscent = 0;
			this.mHeight = 0;
			this.mAscentPadding = 0;
			this.mLineSpacingOffset = 0;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0000EA1F File Offset: 0x0000CC1F
		public Font(Font theFont)
		{
			this.mAscent = theFont.mAscent;
			this.mHeight = theFont.mHeight;
			this.mAscentPadding = theFont.mAscentPadding;
			this.mLineSpacingOffset = theFont.mLineSpacingOffset;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0000EA57 File Offset: 0x0000CC57
		public virtual void Dispose()
		{
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0000EA59 File Offset: 0x0000CC59
		public virtual ImageFont AsImageFont()
		{
			return null;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0000EA5C File Offset: 0x0000CC5C
		public virtual int GetAscent()
		{
			return this.mAscent;
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0000EA64 File Offset: 0x0000CC64
		public virtual int GetAscentPadding()
		{
			return this.mAscentPadding;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0000EA6C File Offset: 0x0000CC6C
		public virtual int GetDescent()
		{
			return this.mHeight - this.mAscent;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0000EA7B File Offset: 0x0000CC7B
		public virtual int GetHeight()
		{
			return this.mHeight;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0000EA83 File Offset: 0x0000CC83
		public virtual int GetLineSpacingOffset()
		{
			return this.mLineSpacingOffset;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0000EA8B File Offset: 0x0000CC8B
		public virtual int GetLineSpacing()
		{
			return this.mHeight + this.mLineSpacingOffset;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0000EA9A File Offset: 0x0000CC9A
		public virtual int StringWidth(string theString)
		{
			return 0;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0000EA9D File Offset: 0x0000CC9D
		public virtual int CharWidth(char theChar)
		{
			return this.StringWidth(string.Concat(theChar));
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0000EAB0 File Offset: 0x0000CCB0
		public virtual int CharWidthKern(char theChar, char thePrevChar)
		{
			return this.CharWidth(theChar);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0000EAB9 File Offset: 0x0000CCB9
		public virtual void DrawString(Graphics g, int theX, int theY, string theString, SexyColor theColor, Rect theClipRect)
		{
		}

		// Token: 0x06000509 RID: 1289
		public abstract Font Duplicate();

		// Token: 0x0400045B RID: 1115
		public int mAscent;

		// Token: 0x0400045C RID: 1116
		public int mAscentPadding;

		// Token: 0x0400045D RID: 1117
		public int mHeight;

		// Token: 0x0400045E RID: 1118
		public int mLineSpacingOffset;
	}
}
