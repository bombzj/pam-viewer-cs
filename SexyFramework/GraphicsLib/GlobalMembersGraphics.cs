using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000AB RID: 171
	public static class GlobalMembersGraphics
	{
		// Token: 0x0600050A RID: 1290 RVA: 0x0000EABC File Offset: 0x0000CCBC
		public static int WriteWordWrappedHelper(Graphics g, string theString, int theX, int theY, int theWidth, int theJustification, bool drawString, int theOffset, int theLength, int theOldColor, int theMaxChars)
		{
			if (theOffset + theLength > theMaxChars)
			{
				theLength = theMaxChars - theOffset;
				if (theLength <= 0)
				{
					return -1;
				}
			}
			return g.WriteString(theString, theX, theY, theWidth, theJustification, drawString, theOffset, theLength, theOldColor);
		}

		// Token: 0x0400045F RID: 1119
		public const int MAX_TEMP_SPANS = 8192;
	}
}
