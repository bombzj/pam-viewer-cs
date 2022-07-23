using System;

namespace SexyFramework.Misc
{
	// Token: 0x02000144 RID: 324
	public static class SexyLocale
	{
		// Token: 0x06000AFD RID: 2813 RVA: 0x000379AB File Offset: 0x00035BAB
		public static void SetSeperators(string theGrouping, string theSeperator)
		{
			SexyLocale.gGrouping = theGrouping;
			SexyLocale.gThousandSep = theSeperator;
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x000379B9 File Offset: 0x00035BB9
		public static void SetLocale(string theLocale)
		{
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x000379BB File Offset: 0x00035BBB
		public static string StringToUpper(string theString)
		{
			return theString.ToUpper();
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x000379C3 File Offset: 0x00035BC3
		public static string StringToLower(string theString)
		{
			return theString.ToLower();
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x000379CB File Offset: 0x00035BCB
		public static bool isalnum(char theChar)
		{
			return char.IsNumber(theChar);
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x000379D3 File Offset: 0x00035BD3
		public static string CommaSeparate(int theValue)
		{
			if (theValue < 0)
			{
				return "-" + SexyLocale.UCommaSeparate((uint)(-(uint)theValue));
			}
			return SexyLocale.UCommaSeparate((uint)theValue);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x000379F4 File Offset: 0x00035BF4
		public static string UCommaSeparate(uint theValue)
		{
			char[] array = new char[64];
			if (theValue == 0U)
			{
				return "0";
			}
			string text = SexyLocale.gGrouping;
			int num = 64;
			int num2 = 0;
			if (text[num2] != SexyLocale.CHAR_MAX && text[num2] > '\0')
			{
				char c = SexyLocale.gThousandSep[0];
				int num3 = 0;
				while (theValue != 0U)
				{
					array[--num] = (char)(48U + theValue % 10U);
					theValue /= 10U;
					if (theValue != 0U && ++num3 == (int)text[num2])
					{
						array[--num] = c;
						num3 = 0;
						if (text[num2 + 1] > '\0')
						{
							num2++;
						}
					}
				}
			}
			else
			{
				while (theValue != 0U)
				{
					array[--num] = (char)(48U + theValue % 10U);
					theValue /= 10U;
				}
			}
			return new string(array, num, 64 - num);
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x00037AB3 File Offset: 0x00035CB3
		public static string CommaSeparate64(long theValue)
		{
			if (theValue < 0L)
			{
				return "-" + SexyLocale.UCommaSeparate64((ulong)(-theValue));
			}
			return SexyLocale.UCommaSeparate64((ulong)theValue);
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x00037AD2 File Offset: 0x00035CD2
		public static string UCommaSeparate64(ulong theValue)
		{
			return "";
		}

		// Token: 0x0400093E RID: 2366
		public static string gGrouping = "\\3";

		// Token: 0x0400093F RID: 2367
		public static string gThousandSep = ",";

		// Token: 0x04000940 RID: 2368
		private static char CHAR_MAX = '\u007f';
	}
}
