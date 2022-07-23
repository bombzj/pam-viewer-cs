using System;
using System.Collections.Generic;

namespace ZumasRevenge
{
	// Token: 0x02000087 RID: 135
	public class BXMLElement
	{
		// Token: 0x060008FD RID: 2301 RVA: 0x0005030E File Offset: 0x0004E50E
		public static bool GetAttribute(BXMLElement theElem, string theName, ref string theValue)
		{
			if (theElem.mAttributes.ContainsKey(theName))
			{
				theValue = theElem.mAttributes[theName];
				return true;
			}
			return false;
		}

		// Token: 0x040006FB RID: 1787
		public int mType;

		// Token: 0x040006FC RID: 1788
		public string mValue;

		// Token: 0x040006FD RID: 1789
		public Dictionary<string, string> mAttributes = new Dictionary<string, string>();
	}
}
