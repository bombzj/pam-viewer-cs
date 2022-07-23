using System;
using System.Collections.Generic;
using System.Linq;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000C8 RID: 200
	public class CharDataHashTable
	{
		// Token: 0x0600061E RID: 1566 RVA: 0x00015CAA File Offset: 0x00013EAA
		public CharDataHashTable()
		{
			this.mOrderedHash = false;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00015CC4 File Offset: 0x00013EC4
		public CharData GetCharData(char inChar, bool inAllowAdd)
		{
			CharData result;
			if (this.mCharDataHash.TryGetValue(inChar, out result))
			{
				return result;
			}
			if (!inAllowAdd)
			{
				return null;
			}
			CharData charData = new CharData();
			this.mCharDataHash[inChar] = charData;
			charData.mChar = (ushort)inChar;
			return charData;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00015D03 File Offset: 0x00013F03
		public int CharCount()
		{
			return this.mCharDataHash.Count;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00015D10 File Offset: 0x00013F10
		public CharData[] ToArray()
		{
			return Enumerable.ToArray<CharData>(this.mCharDataHash.Values);
		}

		// Token: 0x04000501 RID: 1281
		private Dictionary<char, CharData> mCharDataHash = new Dictionary<char, CharData>();

		// Token: 0x04000502 RID: 1282
		public bool mOrderedHash;
	}
}
