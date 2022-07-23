using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x0200014B RID: 331
	public class TextManager
	{
		// Token: 0x06001033 RID: 4147 RVA: 0x000A5DE3 File Offset: 0x000A3FE3
		public static TextManager getInstance()
		{
			return TextManager.instance;
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x000A5DEC File Offset: 0x000A3FEC
		protected TextManager()
		{
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x000A5E84 File Offset: 0x000A4084
		public bool init()
		{
			Localization.LanguageType currentLanguage = Localization.GetCurrentLanguage();
			CultureInfo currentCulture = new CultureInfo(this.sLangFiles[(int)currentLanguage]);
			Thread.CurrentThread.CurrentCulture = currentCulture;
			return true;
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x000A5EB4 File Offset: 0x000A40B4
		public bool LoadTextKitFromStream(Stream s)
		{
			bool result = true;
			try
			{
				using (StreamReader streamReader = new StreamReader(s))
				{
					for (string text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
					{
						text = text.Replace("\\n", "\n");
						text = text.Replace("&cr;", "\n");
						this.mStringList.Add(text);
					}
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x000A5F3C File Offset: 0x000A413C
		public bool LoadTextKit(string file)
		{
			bool result = true;
			Stream stream = null;
			try
			{
				stream = TitleContainer.OpenStream("Content\\" + file);
				using (StreamReader streamReader = new StreamReader(stream))
				{
					for (string text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
					{
						text = text.Replace("\\n", "\n");
						text = text.Replace("&cr;", "\n");
						this.mStringList.Add(text);
					}
				}
			}
			catch (Exception)
			{
				result = false;
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
			return result;
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x000A5FF0 File Offset: 0x000A41F0
		public void releaseTextKit()
		{
			this.mStringList.Clear();
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x000A6000 File Offset: 0x000A4200
		public string getString(int id)
		{
			string stringNameByID = StringID.GetStringNameByID(id);
			return AppResources.ResourceManager.GetString(stringNameByID, Thread.CurrentThread.CurrentCulture);
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x000A602C File Offset: 0x000A422C
		public int getIdByString(string s)
		{
			if (s == "")
			{
				return -1;
			}
			for (int i = 0; i < this.mStringList.Count; i++)
			{
				if (this.mStringList[i].Trim() == s.Trim())
				{
					return i;
				}
			}
			throw new Exception("failed to find string - " + s);
		}

		// Token: 0x04001A9E RID: 6814
		protected static TextManager instance = new TextManager();

		// Token: 0x04001A9F RID: 6815
		private string[] sLangFiles = new string[]
		{
			"en-US", "fr-FR", "it-IT", "de-DE", "es-ES", "zh-CN", "ru-RU", "pl-PL", "pt-PT", "es-CO",
			"zh-TW", "pt-BR"
		};

		// Token: 0x04001AA0 RID: 6816
		protected List<string> mStringList = new List<string>(300);
	}
}
