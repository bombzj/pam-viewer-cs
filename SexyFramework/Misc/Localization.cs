using System;

namespace SexyFramework.Misc
{
	// Token: 0x02000133 RID: 307
	public static class Localization
	{
		// Token: 0x06000A1E RID: 2590 RVA: 0x000349B4 File Offset: 0x00032BB4
		public static void InitLanguage()
		{
			SexyAppBase gSexyAppBase = GlobalMembers.gSexyAppBase;
			Localization.sCurrentLanguage = gSexyAppBase.mAppDriver.GetAppLanguage();
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x000349D7 File Offset: 0x00032BD7
		public static void ChangeLanguage(Localization.LanguageType lan)
		{
			Localization.sCurrentLanguage = lan;
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x000349DF File Offset: 0x00032BDF
		public static Localization.LanguageType GetCurrentLanguage()
		{
			return Localization.sCurrentLanguage;
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x000349E6 File Offset: 0x00032BE6
		public static int GetCurrentFontOffsetY()
		{
			if (Localization.sCurrentLanguage == Localization.LanguageType.Language_CH || Localization.sCurrentLanguage == Localization.LanguageType.Language_CHT)
			{
				return 10;
			}
			return 0;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x000349FD File Offset: 0x00032BFD
		public static string GetLanguageSuffix(Localization.LanguageType lan)
		{
			return Localization.LanguageSuffix[(int)lan];
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00034A08 File Offset: 0x00032C08
		public static string GetCurrentThousandSep()
		{
			if (Localization.sCurrentLanguage == Localization.LanguageType.Language_FR || Localization.sCurrentLanguage == Localization.LanguageType.Language_RU)
			{
				return " ";
			}
			if (Localization.sCurrentLanguage == Localization.LanguageType.Language_GR || Localization.sCurrentLanguage == Localization.LanguageType.Language_SP || Localization.sCurrentLanguage == Localization.LanguageType.Language_SPC || Localization.sCurrentLanguage == Localization.LanguageType.Language_IT || Localization.sCurrentLanguage == Localization.LanguageType.Language_PG || Localization.sCurrentLanguage == Localization.LanguageType.Language_PGB)
			{
				return ".";
			}
			if (Localization.sCurrentLanguage == Localization.LanguageType.Language_PL)
			{
				return "";
			}
			return ",";
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x00034A76 File Offset: 0x00032C76
		public static int GetCurrentSeperateCount()
		{
			return 3;
		}

		// Token: 0x040008FD RID: 2301
		public static string[] LanguageSuffix = new string[]
		{
			"_EN", "_FR", "_IT", "_GR", "_SP", "_CH", "_RU", "_PL", "_PG", "_SPC",
			"_CHT", "_PGB", ""
		};

		// Token: 0x040008FE RID: 2302
		private static Localization.LanguageType sCurrentLanguage = Localization.LanguageType.Language_EN;

		// Token: 0x02000134 RID: 308
		public enum LanguageType
		{
			// Token: 0x04000900 RID: 2304
			Language_EN,
			// Token: 0x04000901 RID: 2305
			Language_FR,
			// Token: 0x04000902 RID: 2306
			Language_IT,
			// Token: 0x04000903 RID: 2307
			Language_GR,
			// Token: 0x04000904 RID: 2308
			Language_SP,
			// Token: 0x04000905 RID: 2309
			Language_CH,
			// Token: 0x04000906 RID: 2310
			Language_RU,
			// Token: 0x04000907 RID: 2311
			Language_PL,
			// Token: 0x04000908 RID: 2312
			Language_PG,
			// Token: 0x04000909 RID: 2313
			Language_SPC,
			// Token: 0x0400090A RID: 2314
			Language_CHT,
			// Token: 0x0400090B RID: 2315
			Language_PGB,
			// Token: 0x0400090C RID: 2316
			Language_UNKNOWN
		}
	}
}
