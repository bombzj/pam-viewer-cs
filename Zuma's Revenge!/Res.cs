using System;
using System.Collections.Generic;
using System.Reflection;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.Resource;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000134 RID: 308
	public static class Res
	{
		// Token: 0x06000F60 RID: 3936 RVA: 0x0009EF72 File Offset: 0x0009D172
		public static void InitResources(GameApp app)
		{
			Res.mApp = app;
			Res.mResMgr = Res.mApp.mResourceManager;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0009EF8C File Offset: 0x0009D18C
		public static Image GetImageByID(ResID id)
		{
			if (Res.mGlobalRes[(int)id] != null && Res.mGlobalRes[(int)id].mResObject != null)
			{
				return Res.mGlobalRes[(int)id].mResObject as Image;
			}
			string text = id.ToString();
			Res.mGlobalRes[(int)id] = Res.mResMgr.RegisterGlobalPtr(text);
			if (Res.mGlobalRes[(int)id] == null)
			{
				List<string> allEnum = Res.GetAllEnum(id);
				for (int i = 0; i < allEnum.Count; i++)
				{
					string theId = allEnum[i];
					Res.mGlobalRes[(int)id] = Res.mResMgr.RegisterGlobalPtr(theId);
					if (Res.mGlobalRes[(int)id] != null)
					{
						break;
					}
				}
			}
			if (Res.mGlobalRes[(int)id] != null)
			{
				Res.mResMgr.LoadImage(text);
			}
			return Res.mGlobalRes[(int)id].mResObject as Image;
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x0009F050 File Offset: 0x0009D250
		public static List<string> GetAllEnum(ResID id)
		{
			List<string> list = new List<string>();
			foreach (FieldInfo fieldInfo in typeof(ResID).GetFields())
			{
				if (fieldInfo.IsLiteral && typeof(ResID).GetType() == typeof(ResID).GetType() && (int)fieldInfo.GetRawConstantValue() == (int)typeof(ResID).GetField(id.ToString()).GetRawConstantValue())
				{
					list.Add(fieldInfo.Name);
				}
			}
			return list;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x0009F0EC File Offset: 0x0009D2EC
		public static int GetIDByImage(Image img)
		{
			for (int i = 0; i < Res.mGlobalRes.Length; i++)
			{
				if (Res.mGlobalRes[i] != null && Res.mGlobalRes[i].mResObject == img)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0009F128 File Offset: 0x0009D328
		public static Font GetFontByID(ResID id)
		{
			if (Res.mGlobalRes[(int)id] != null && Res.mGlobalRes[(int)id].mResObject != null)
			{
				return Res.mGlobalRes[(int)id].mResObject as Font;
			}
			string text = id.ToString();
			Res.mGlobalRes[(int)id] = Res.mResMgr.RegisterGlobalPtr(text);
			if (Res.mGlobalRes[(int)id] != null)
			{
				Res.mResMgr.LoadFont(text);
			}
			return Res.mGlobalRes[(int)id].mResObject as Font;
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0009F1A4 File Offset: 0x0009D3A4
		public static int GetSoundByID(ResID id)
		{
			if (Res.mGlobalRes[(int)id] != null && Res.mGlobalRes[(int)id].mResObject != null)
			{
				return (int)Res.mGlobalRes[(int)id].mResObject;
			}
			string text = id.ToString();
			Res.mGlobalRes[(int)id] = Res.mResMgr.RegisterGlobalPtr(text);
			if (Res.mGlobalRes[(int)id] != null)
			{
				Res.mResMgr.LoadSound(text);
			}
			return (int)Res.mGlobalRes[(int)id].mResObject;
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0009F220 File Offset: 0x0009D420
		public static PIEffect GetPIEffectByID(ResID id)
		{
			if (Res.mGlobalRes[(int)id] != null && Res.mGlobalRes[(int)id].mResObject != null)
			{
				return Res.mGlobalRes[(int)id].mResObject as PIEffect;
			}
			string text = id.ToString();
			Res.mGlobalRes[(int)id] = Res.mResMgr.RegisterGlobalPtr(text);
			if (Res.mGlobalRes[(int)id] != null)
			{
				Res.mResMgr.LoadPIEffect(text);
			}
			return Res.mGlobalRes[(int)id].mResObject as PIEffect;
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x0009F29C File Offset: 0x0009D49C
		public static Effect GetEffectByID(ResID id)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x0009F2A4 File Offset: 0x0009D4A4
		public static PopAnim GetPopAnimByID(ResID id)
		{
			if (Res.mGlobalRes[(int)id] != null)
			{
				return Res.mGlobalRes[(int)id].mResObject as PopAnim;
			}
			string text = id.ToString();
			Res.mGlobalRes[(int)id] = Res.mResMgr.RegisterGlobalPtr(text);
			if (Res.mGlobalRes[(int)id] != null)
			{
				Res.mResMgr.LoadPopAnim(text);
			}
			return Res.mGlobalRes[(int)id].mResObject as PopAnim;
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x0009F314 File Offset: 0x0009D514
		public static int GetOffsetXByID(ResID id)
		{
			if (Res.mGlobalResOffset[(int)id] != null)
			{
				return Res.mGlobalResOffset[(int)id].mX;
			}
			string theId = id.ToString();
			SexyPoint offsetOfImage = Res.mResMgr.GetOffsetOfImage(theId);
			if (offsetOfImage != null)
			{
				Res.mGlobalResOffset[(int)id] = new SexyPoint(offsetOfImage);
				return Res.mGlobalResOffset[(int)id].mX;
			}
			return 0;
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0009F37C File Offset: 0x0009D57C
		public static int GetOffsetYByID(ResID id)
		{
			if (Res.mGlobalResOffset[(int)id] != null)
			{
				return Res.mGlobalResOffset[(int)id].mY;
			}
			string theId = id.ToString();
			SexyPoint offsetOfImage = Res.mResMgr.GetOffsetOfImage(theId);
			if (offsetOfImage != null)
			{
				Res.mGlobalResOffset[(int)id] = new SexyPoint(offsetOfImage);
				return Res.mGlobalResOffset[(int)id].mY;
			}
			return 0;
		}

		// Token: 0x040016A9 RID: 5801
		private static ResGlobalPtr[] mGlobalRes = new ResGlobalPtr[1850];

		// Token: 0x040016AA RID: 5802
		private static SexyPoint[] mGlobalResOffset = new SexyPoint[1850];

		// Token: 0x040016AB RID: 5803
		private static GameApp mApp = null;

		// Token: 0x040016AC RID: 5804
		private static ResourceManager mResMgr = null;
	}
}
