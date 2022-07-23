using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using SexyFramework.Drivers;
using SexyFramework.Misc;

namespace SexyFramework
{
	// Token: 0x02000125 RID: 293
	public static class Common
	{
		// Token: 0x0600097D RID: 2429 RVA: 0x00031BC2 File Offset: 0x0002FDC2
		public static uint SexyTime()
		{
			return (uint)GlobalMembers.gSexyAppBase.mAppDriver.GetAppTime();
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x00031BD3 File Offset: 0x0002FDD3
		public static string StringToWString(string theString)
		{
			return theString;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00031BD6 File Offset: 0x0002FDD6
		public static string WStringToString(string theString)
		{
			return theString;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00031BDC File Offset: 0x0002FDDC
		public static bool StringToInt(string theString, ref int theIntVal)
		{
			theIntVal = 0;
			if (theString.Length == 0)
			{
				return false;
			}
			int num = 10;
			bool flag = false;
			int i = 0;
			if (theString[i] == '-')
			{
				flag = true;
				i++;
			}
			while (i < theString.Length)
			{
				char c = theString[i];
				if (num == 10 && c >= '0' && c <= '9')
				{
					theIntVal = theIntVal * 10 + (int)(c - '0');
				}
				else if (num == 16 && ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f')))
				{
					if (c <= '9')
					{
						theIntVal = theIntVal * 16 + (int)(c - '0');
					}
					else if (c <= 'F')
					{
						theIntVal = theIntVal * 16 + (int)(c - 'A') + 10;
					}
					else
					{
						theIntVal = theIntVal * 16 + (int)(c - 'a') + 10;
					}
				}
				else
				{
					if ((c != 'x' && c != 'X') || i != 1 || theIntVal != 0)
					{
						theIntVal = 0;
						return false;
					}
					num = 16;
				}
				i++;
			}
			if (flag)
			{
				theIntVal = -theIntVal;
			}
			return true;
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00031CD0 File Offset: 0x0002FED0
		public static bool StringToDouble(string aTempString, ref double theDouble)
		{
			theDouble = 0.0;
			try
			{
				theDouble = double.Parse(aTempString, NumberStyles.Float, CultureInfo.InvariantCulture);
				return true;
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x00031D14 File Offset: 0x0002FF14
		public static string XMLDecodeString(string theString)
		{
			StringBuilder stringBuilder = new StringBuilder("");
			for (int i = 0; i < theString.Length; i++)
			{
				char c = theString[i];
				if (c == '&')
				{
					int num = theString.IndexOf(';', i);
					if (num != -1)
					{
						string text = theString.Substring(i + 1, num - i - 1);
						i = num;
						if (text == "lt")
						{
							c = '<';
						}
						else if (text == "amp")
						{
							c = '&';
						}
						else if (text == "gt")
						{
							c = '>';
						}
						else if (text == "quot")
						{
							c = '"';
						}
						else if (text == "apos")
						{
							c = '\'';
						}
						else if (text == "nbsp")
						{
							c = ' ';
						}
						else if (text == "cr")
						{
							c = '\n';
						}
						else if (text[0] == '#' && text.Length > 1)
						{
							int num2 = (int)c;
							if (text[1] == 'x')
							{
								Common.StringToInt("0x" + text.Substring(2), ref num2);
							}
							else
							{
								Common.StringToInt(text.Substring(1), ref num2);
							}
							c = (char)num2;
						}
					}
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00031E6C File Offset: 0x0003006C
		public static string GetFileName(string thePath, bool noExtension)
		{
			int num = Math.Max(thePath.LastIndexOf('\\'), thePath.LastIndexOf('/'));
			if (noExtension)
			{
				int num2 = thePath.LastIndexOf('.');
				if (num2 > num)
				{
					return thePath.Substring(num + 1, num2 - num - 1);
				}
			}
			if (num == -1)
			{
				return thePath;
			}
			return thePath.Substring(num + 1);
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x00031EC0 File Offset: 0x000300C0
		public static string RemoveTrailingSlash(string theDirectory)
		{
			int length = theDirectory.Length;
			if (length > 0 && (theDirectory[length - 1] == '\\' || theDirectory[length - 1] == '/'))
			{
				return theDirectory.Substring(0, length - 1);
			}
			return theDirectory;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x00031EFE File Offset: 0x000300FE
		public static string GetCurDir()
		{
			return Common.GetGameFileDriver().GetCurPath();
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00031F0A File Offset: 0x0003010A
		public static string GetFullPath(string theRelPath)
		{
			return Common.GetPathFrom(theRelPath, Common.GetCurDir());
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x00031F18 File Offset: 0x00030118
		public static string GetFileDir(string thePath, bool withSlash)
		{
			int num = Math.Max(thePath.LastIndexOf('\\'), thePath.LastIndexOf('/'));
			if (num == -1)
			{
				return "";
			}
			if (withSlash)
			{
				return thePath.Substring(0, num + 1);
			}
			return thePath.Substring(0, num);
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x00031F5C File Offset: 0x0003015C
		public static string GetPathFrom(string theRelPath, string theDir)
		{
			string text = "";
			string text2 = new string(theDir.ToCharArray());
			if (theRelPath.Length >= 2 && theRelPath[1] == ':')
			{
				return theRelPath;
			}
			char c = '/';
			if (theRelPath.IndexOf('\\') != -1 || theDir.IndexOf('\\') != -1)
			{
				c = '\\';
			}
			if (text2.Length >= 2 && text2[1] == ':')
			{
				text = text2.Substring(0, 2);
				text2 = text2.Remove(0, 2);
			}
			if (text2.Length > 0 && text2[text2.Length - 1] != '\\' && text2[text2.Length - 1] != '/')
			{
				text2 += c;
			}
			string text3 = new string(theRelPath.ToCharArray());
			while (text2.Length != 0)
			{
				int num = text3.IndexOf('\\');
				int num2 = text3.IndexOf('/');
				if (num == -1 || (num2 != -1 && num2 < num))
				{
					num = num2;
				}
				if (num == -1)
				{
					break;
				}
				string text4 = text3.Substring(0, num);
				text3 = text3.Remove(0, num + 1);
				if (text4 == "..")
				{
					int num3 = Math.Max(text2.LastIndexOf('\\'), text2.LastIndexOf('/'));
					string text5;
					if (num3 != -1)
					{
						text5 = text2.Substring(num3, text2.Length - num3 - 1);
					}
					else
					{
						text5 = text2;
					}
					if (text5 == "..")
					{
						text2 += "..";
						text2 += c;
					}
					else
					{
						text2 = text2.Remove(num3);
					}
				}
				else
				{
					if (text4 == "")
					{
						text2 += c;
						break;
					}
					if (text4 != ".")
					{
						text2 = text2 + text4 + c;
						break;
					}
				}
			}
			text2 = text + text2 + text3;
			if (c == '/')
			{
				text2.Replace('\\', '/');
			}
			else
			{
				text2.Replace('/', '\\');
			}
			return text2;
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00032150 File Offset: 0x00030350
		public static bool isSpace(char c)
		{
			return c == ' ' || c == '\t';
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x00032160 File Offset: 0x00030360
		public static string Trim(string theString)
		{
			int num = 0;
			while (num < theString.Length && Common.isSpace(theString[num]))
			{
				num++;
			}
			int num2 = theString.Length - 1;
			while (num2 >= 0 && Common.isSpace(theString[num2]))
			{
				num2--;
			}
			return theString.Substring(num, num2 - num + 1);
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x000321BC File Offset: 0x000303BC
		public static bool DividePoly(Vector2[] v, int n, Vector2[,] theTris, int theMaxTris, ref int theNumTris)
		{
			int[] array = new int[Common.V_MAX];
			if (n > Common.V_MAX)
			{
				return false;
			}
			theNumTris = 0;
			int num = Common.orientation(n, v);
			for (int i = 0; i < n; i++)
			{
				array[i] = i;
			}
			int j = n;
			while (j > 3)
			{
				float num2 = (float)Common.BIG;
				int num3 = 0;
				int num4;
				int num5;
				for (int k = 0; k < j; k++)
				{
					num4 = k - 1;
					num5 = k + 1;
					if (k == 0)
					{
						num4 = j - 1;
					}
					else if (k == j - 1)
					{
						num5 = 0;
					}
					float num6;
					if (Common.determinant(array[num4], array[k], array[num5], v) == num && Common.no_interior(array[num4], array[k], array[num5], v, array, j, num) != 0 && (num6 = Common.distance2(v[array[num4]].X, v[array[num4]].Y, v[array[num5]].X, v[array[num5]].Y)) < num2)
					{
						num2 = num6;
						num3 = k;
					}
				}
				if ((double)num2 == Common.BIG)
				{
					return false;
				}
				num4 = num3 - 1;
				num5 = num3 + 1;
				if (num3 == 0)
				{
					num4 = j - 1;
				}
				else if (num3 == j - 1)
				{
					num5 = 0;
				}
				if (theNumTris >= theMaxTris)
				{
					return false;
				}
				theTris[theNumTris, 0].X = v[array[num4]].X;
				theTris[theNumTris, 0].Y = v[array[num4]].Y;
				theTris[theNumTris, 1].X = v[array[num3]].X;
				theTris[theNumTris, 1].Y = v[array[num3]].Y;
				theTris[theNumTris, 2].X = v[array[num5]].X;
				theTris[theNumTris, 2].Y = v[array[num5]].Y;
				theNumTris++;
				j--;
				for (int i = num3; i < j; i++)
				{
					array[i] = array[i + 1];
				}
			}
			if (theNumTris >= theMaxTris)
			{
				return false;
			}
			theTris[theNumTris, 0].X = v[array[0]].X;
			theTris[theNumTris, 0].Y = v[array[0]].Y;
			theTris[theNumTris, 1].X = v[array[1]].X;
			theTris[theNumTris, 1].Y = v[array[1]].Y;
			theTris[theNumTris, 2].X = v[array[2]].X;
			theTris[theNumTris, 2].Y = v[array[2]].Y;
			theNumTris++;
			return true;
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0003248C File Offset: 0x0003068C
		private static int orientation(int n, Vector2[] v)
		{
			float num = v[n - 1].X * v[0].Y - v[0].X * v[n - 1].Y;
			for (int i = 0; i < n - 1; i++)
			{
				num += v[i].X * v[i + 1].Y - v[i + 1].X * v[i].Y;
			}
			if ((double)num >= 0.0)
			{
				return Common.COUNTER_CLOCKWISE;
			}
			return Common.CLOCKWISE;
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x00032534 File Offset: 0x00030734
		private static int determinant(int p1, int p2, int p3, Vector2[] v)
		{
			float num = v[p1].X;
			float num2 = v[p1].Y;
			float num3 = v[p2].X;
			float num4 = v[p2].Y;
			float num5 = v[p3].X;
			float num6 = v[p3].Y;
			float num7 = (num3 - num) * (num6 - num2) - (num5 - num) * (num4 - num2);
			if ((double)num7 >= 0.0)
			{
				return Common.COUNTER_CLOCKWISE;
			}
			return Common.CLOCKWISE;
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x000325C4 File Offset: 0x000307C4
		private static int no_interior(int p1, int p2, int p3, Vector2[] v, int[] vp, int n, int poly_or)
		{
			for (int i = 0; i < n; i++)
			{
				int num = vp[i];
				if (num != p1 && num != p2 && num != p3 && Common.determinant(p2, p1, num, v) != poly_or && Common.determinant(p1, p3, num, v) != poly_or && Common.determinant(p3, p2, num, v) != poly_or)
				{
					return 0;
				}
			}
			return 1;
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0003261C File Offset: 0x0003081C
		private static float distance2(float x1, float y1, float x2, float y2)
		{
			float num = x1 - x2;
			float num2 = y1 - y2;
			return num * num + num2 * num2;
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0003263A File Offset: 0x0003083A
		public static int Rand()
		{
			return (int)Common.gCommonMTRand.Next();
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00032646 File Offset: 0x00030846
		public static int Rand(int range)
		{
			return (int)Common.gCommonMTRand.Next((uint)range);
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00032653 File Offset: 0x00030853
		public static float Rand(float range)
		{
			return Common.gCommonMTRand.Next(range);
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00032660 File Offset: 0x00030860
		public static void SRand(uint theSeed)
		{
			Common.gCommonMTRand.SRand(theSeed);
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0003266D File Offset: 0x0003086D
		public static int SafeRand()
		{
			return (int)Common.gCommonMTRand.Next();
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x00032679 File Offset: 0x00030879
		public static string CommaSeperate(int theValue)
		{
			if (theValue < 0)
			{
				return "-" + Common.UCommaSeparate64((uint)(-(uint)theValue));
			}
			return Common.UCommaSeparate64((uint)theValue);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00032697 File Offset: 0x00030897
		public static string UCommaSeparate(uint theValue)
		{
			return Common.UCommaSeparate64(theValue);
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0003269F File Offset: 0x0003089F
		public static string CommaSeperate64(long theValue)
		{
			if (theValue < 0L)
			{
				return "-" + Common.UCommaSeparate64((uint)(-(uint)theValue));
			}
			return Common.UCommaSeparate64((uint)theValue);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x000326C0 File Offset: 0x000308C0
		public static string UCommaSeparate64(uint theValue)
		{
			if (theValue == 0U)
			{
				return "0";
			}
			Common.aPunctBuffer.Clear();
			Common.sbuider.Clear();
			string currentThousandSep = Localization.GetCurrentThousandSep();
			int currentSeperateCount = Localization.GetCurrentSeperateCount();
			int num = 0;
			while (theValue > 0U)
			{
				uint num2 = theValue % 10U;
				theValue /= 10U;
				Common.aPunctBuffer.Add(num2.ToString());
				if (theValue > 0U)
				{
					num++;
					if (num == currentSeperateCount && currentThousandSep.Length > 0)
					{
						Common.aPunctBuffer.Add(currentThousandSep);
						num = 0;
					}
				}
			}
			for (int i = Common.aPunctBuffer.Count - 1; i >= 0; i--)
			{
				Common.sbuider.Append(Common.aPunctBuffer[i]);
			}
			return Common.sbuider.ToString();
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0003277C File Offset: 0x0003097C
		public static void SexySleep(int milliseconds)
		{
			try
			{
				Thread.Sleep(milliseconds);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x000327A4 File Offset: 0x000309A4
		public static int size<T>(this List<T> list)
		{
			return list.Count;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x000327AC File Offset: 0x000309AC
		public static T back<T>(this List<T> list)
		{
			return Enumerable.Last<T>(list);
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x000327B4 File Offset: 0x000309B4
		public static T front<T>(this List<T> list)
		{
			return Enumerable.First<T>(list);
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x000327BC File Offset: 0x000309BC
		public static void Reserve<T>(this List<T> list, int newSize)
		{
			if (list.Count > newSize)
			{
				list.RemoveRange(newSize, list.Count - newSize);
				return;
			}
			int num = newSize - list.Count;
			for (int i = 0; i < num; i++)
			{
				list.Add(default(T));
			}
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x00032808 File Offset: 0x00030A08
		public static void Resize<T>(this List<T> list, int newSize)
		{
			if (list.Count > newSize)
			{
				list.RemoveRange(newSize, list.Count - newSize);
				return;
			}
			list.Capacity = newSize;
			int num = newSize - list.Count;
			if (typeof(T).IsValueType)
			{
				for (int i = 0; i < num; i++)
				{
					list.Add(default(T));
				}
				return;
			}
			for (int j = 0; j < num; j++)
			{
				list.Add(Activator.CreateInstance<T>());
			}
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00032884 File Offset: 0x00030A84
		public static T[] CreateObjectArray<T>(int size)
		{
			T[] array = new T[size];
			if (!typeof(T).IsValueType)
			{
				for (int i = 0; i < size; i++)
				{
					array[i] = Activator.CreateInstance<T>();
				}
			}
			return array;
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x000328C2 File Offset: 0x00030AC2
		public static IFileDriver GetGameFileDriver()
		{
			return GlobalMembers.gFileDriver;
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x000328C9 File Offset: 0x00030AC9
		public static string GetAppDataFolder()
		{
			return GlobalMembers.gFileDriver.GetSaveDataPath();
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x000328D5 File Offset: 0x00030AD5
		public static string SetAppDataFolder(string thePath)
		{
			return "";
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x000328DC File Offset: 0x00030ADC
		public static int IntRange(int min_val, int max_val)
		{
			if (min_val == max_val)
			{
				return min_val;
			}
			if (min_val < 0 && max_val < 0)
			{
				return min_val + Common.SafeRand() % (Math.Abs(min_val) - Math.Abs(max_val));
			}
			return min_val + Common.SafeRand() % (max_val - min_val + 1);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00032910 File Offset: 0x00030B10
		public static float FloatRange(float min_val, float max_val)
		{
			if (min_val == max_val)
			{
				return min_val;
			}
			if (min_val < 0f && max_val < 0f)
			{
				return min_val + (float)(Common.SafeRand() % (int)((Math.Abs(min_val) - Math.Abs(max_val)) * 100000000f + 1f)) / 100000000f;
			}
			return min_val + (float)(Common.SafeRand() % (int)((max_val - min_val) * 100000000f + 1f)) / 100000000f;
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0003297C File Offset: 0x00030B7C
		public static float SAFE_RAND(float val)
		{
			if (val != 0f)
			{
				return (float)Common.Rand() % val;
			}
			return 0f;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x00032994 File Offset: 0x00030B94
		public static bool _eq(float n1, float n2, float tolerance)
		{
			return Math.Abs(n1 - n2) <= tolerance;
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x000329A4 File Offset: 0x00030BA4
		public static bool _leq(float n1, float n2, float tolerance)
		{
			return Common._eq(n1, n2, tolerance) || n1 < n2;
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x000329B6 File Offset: 0x00030BB6
		public static bool _geq(float n1, float n2, float tolerance)
		{
			return Common._eq(n1, n2, tolerance) || n1 > n2;
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x000329C8 File Offset: 0x00030BC8
		public static bool _eq(float n1, float n2)
		{
			return Math.Abs(n1 - n2) <= float.Epsilon;
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x000329DC File Offset: 0x00030BDC
		public static bool _leq(float n1, float n2)
		{
			return Common._eq(n1, n2, float.Epsilon) || n1 < n2;
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x000329F2 File Offset: 0x00030BF2
		public static bool _geq(float n1, float n2)
		{
			return Common._eq(n1, n2, float.Epsilon) || n1 > n2;
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x00032A08 File Offset: 0x00030C08
		public static int Sign(int val)
		{
			if (val >= 0)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x00032A11 File Offset: 0x00030C11
		public static float Sign(float val)
		{
			if (val >= 0f)
			{
				return 1f;
			}
			return -1f;
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x00032A26 File Offset: 0x00030C26
		public static float AngleBetweenPoints(float p1x, float p1y, float p2x, float p2y)
		{
			return (float)Math.Atan2((double)(-(double)(p2y - p1y)), (double)(p2x - p1x));
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x00032A37 File Offset: 0x00030C37
		public static float AngleBetweenPoints(SexyPoint p1, SexyPoint p2)
		{
			return Common.AngleBetweenPoints((float)p1.mX, (float)p1.mY, (float)p2.mX, (float)p2.mY);
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x00032A5C File Offset: 0x00030C5C
		public static SexyVector2 RotatePoint(float pAngle, SexyVector2 pVector, SexyVector2 pCenter)
		{
			float num = pVector.x - pCenter.x;
			float num2 = pVector.y - pCenter.y;
			float theX = (float)((double)pCenter.x + (double)num * Math.Cos((double)pAngle) + (double)num2 * Math.Sin((double)pAngle));
			float theY = (float)((double)pCenter.y + (double)num2 * Math.Cos((double)pAngle) - (double)num * Math.Sin((double)pAngle));
			return new SexyVector2(theX, theY);
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00032AD0 File Offset: 0x00030CD0
		public static SexyVector2 RotatePoint(float pAngle, SexyVector2 pVector)
		{
			return Common.RotatePoint(pAngle, pVector, new SexyVector2(0f, 0f));
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x00032AE8 File Offset: 0x00030CE8
		public static void RotatePoint(float pAngle, ref float x, ref float y, float cx, float cy)
		{
			SexyVector2 sexyVector = Common.RotatePoint(pAngle, new SexyVector2(x, y), new SexyVector2(cx, cy));
			x = sexyVector.x;
			y = sexyVector.y;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x00032B20 File Offset: 0x00030D20
		public static void _RotatePointClockwise(SexyPoint p, float angle)
		{
			float num = (float)Math.Cos((double)angle);
			float num2 = (float)Math.Sin((double)angle);
			float num3 = (float)p.mX;
			p.mX = (int)(num3 * num + (float)p.mY * num2);
			p.mY = (int)(-num3 * num2 + (float)p.mY * num);
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00032B70 File Offset: 0x00030D70
		public static bool RotatedRectsIntersect(Rect r1, float r1_angle, Rect r2, float r2_angle)
		{
			r1_angle = -r1_angle;
			r2_angle = -r2_angle;
			float num = r1_angle - r2_angle;
			float num2 = (float)Math.Cos((double)num);
			float num3 = (float)Math.Sin((double)num);
			SexyPoint point = new SexyPoint(r1.mWidth / 2, r1.mHeight / 2);
			SexyPoint point2 = new SexyPoint(r2.mWidth / 2, r2.mHeight / 2);
			SexyPoint p = new SexyPoint(r1.mX + point.mX, r1.mY + point.mY);
			SexyPoint theTPoint = new SexyPoint(r2.mX + point2.mX, r2.mY + point2.mY);
			SexyPoint point3 = new SexyPoint(theTPoint);
			point3 -= p;
			Common._RotatePointClockwise(point3, r2_angle);
			SexyPoint point4 = new SexyPoint(point3);
			SexyPoint point5 = new SexyPoint(point3);
			point4 -= point2;
			point5 += point2;
			SexyPoint point6 = new SexyPoint();
			SexyPoint point7 = new SexyPoint();
			point6.mX = (int)((float)(-(float)point.mY) * num3);
			point7.mX = point6.mX;
			float num4 = (float)point.mX * num2;
			point6.mX += (int)num4;
			point7.mX -= (int)num4;
			point6.mY = (int)((float)point.mY * num2);
			point7.mY = point6.mY;
			num4 = (float)point.mX * num3;
			point6.mY += (int)num4;
			point7.mY -= (int)num4;
			num4 = num3 * num2;
			if (num4 < 0f)
			{
				num4 = (float)point6.mX;
				point6.mX = point7.mX;
				point7.mX = (int)num4;
				num4 = (float)point6.mY;
				point6.mY = point7.mY;
				point7.mY = (int)num4;
			}
			if (num3 < 0f)
			{
				point7.mX = -point7.mX;
				point7.mY = -point7.mY;
			}
			if (point7.mX > point5.mX || point7.mX > -point4.mX)
			{
				return false;
			}
			float num5;
			float num6;
			if (num4 == 0f)
			{
				num5 = (float)point6.mY;
				num6 = -num5;
			}
			else
			{
				float num7 = (float)(point4.mX - point6.mX);
				float num8 = (float)(point5.mX - point6.mX);
				num5 = (float)point6.mY;
				if (num8 * num7 > 0f)
				{
					float num9 = (float)point6.mX;
					if (num7 < 0f)
					{
						num9 -= (float)point7.mX;
						num5 -= (float)point7.mY;
						num7 = num8;
					}
					else
					{
						num9 += (float)point7.mX;
						num5 += (float)point7.mY;
					}
					num5 *= num7;
					num5 /= num9;
					num5 += (float)point6.mY;
				}
				num7 = (float)(point4.mX + point6.mX);
				num8 = (float)(point5.mX + point6.mX);
				num6 = (float)(-(float)point6.mY);
				if (num8 * num7 > 0f)
				{
					float num9 = (float)(-(float)point6.mX);
					if (num7 < 0f)
					{
						num9 -= (float)point7.mX;
						num6 -= (float)point7.mY;
						num7 = num8;
					}
					else
					{
						num9 += (float)point7.mX;
						num6 += (float)point7.mY;
					}
					num6 *= num7;
					num6 /= num9;
					num6 -= (float)point6.mY;
				}
			}
			return (num5 >= (float)point4.mY || num6 >= (float)point4.mY) && (num5 <= (float)point5.mY || num6 <= (float)point5.mY);
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x00032F30 File Offset: 0x00031130
		public static float DistFromPointToLine(SexyPoint line_p1, SexyPoint line_p2, SexyPoint p, ref float t)
		{
			return Common.DistFromPointToLine(new FPoint((float)line_p1.mX, (float)line_p1.mY), new FPoint((float)line_p2.mX, (float)line_p2.mY), new FPoint((float)p.mX, (float)p.mY), ref t);
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x00032F7C File Offset: 0x0003117C
		public static float DistFromPointToLine(FPoint line_p1, FPoint line_p2, FPoint p, ref float t)
		{
			SexyVector2 v = new SexyVector2(p.mX - line_p1.mX, p.mY - line_p1.mY);
			SexyVector2 v2 = new SexyVector2(line_p2.mX - line_p1.mX, line_p2.mY - line_p1.mY);
			float num = v.Dot(v2);
			if (num <= 0f)
			{
				t = 0f;
				return v.Dot(v);
			}
			float num2 = v2.Dot(v2);
			if (num >= num2)
			{
				t = 1f;
				return v.Dot(v) - 2f * num + num2;
			}
			t = num / num2;
			return v.Dot(v) - t * num;
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x00033028 File Offset: 0x00031228
		public static float DistFromPointToLine(Vector2 line_p1, Vector2 line_p2, Vector2 p, ref float t)
		{
			Vector2 vector = line_p2 - line_p1;
			return Vector2.Distance(p, vector);
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x00033044 File Offset: 0x00031244
		public static float Distance(float p1x, float p1y, float p2x, float p2y, bool sqrt)
		{
			float num = p2x - p1x;
			float num2 = p2y - p1y;
			float num3 = num * num + num2 * num2;
			if (!sqrt)
			{
				return num3;
			}
			return (float)Math.Sqrt((double)num3);
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0003306F File Offset: 0x0003126F
		public static float Distance(float p1x, float p1y, float p2x, float p2y)
		{
			return Common.Distance(p1x, p1y, p2x, p2y, true);
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0003307B File Offset: 0x0003127B
		public static float StrToFloat(string str)
		{
			if (str.Length == 0)
			{
				return 0f;
			}
			return float.Parse(str, NumberStyles.Float, CultureInfo.InvariantCulture);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0003309B File Offset: 0x0003129B
		public static int StrToInt(string str)
		{
			if (str.Length == 0)
			{
				return 0;
			}
			return int.Parse(str);
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x000330AD File Offset: 0x000312AD
		public static bool StrEquals(string str1, string str2, bool pIgnoreCase)
		{
			if (!pIgnoreCase)
			{
				return str1 == str2;
			}
			return string.Compare(str1, str2, true) == 0;
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x000330C5 File Offset: 0x000312C5
		public static bool StrEquals(string str1, string str2)
		{
			return Common.StrEquals(str1, str2, false);
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x000330CF File Offset: 0x000312CF
		public static float RadiansToDegrees(float pRads)
		{
			return pRads * 57.29694f;
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x000330D8 File Offset: 0x000312D8
		public static float DegreesToRadians(float pDegs)
		{
			return pDegs * 0.017452938f;
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x000330E4 File Offset: 0x000312E4
		public static float CaculatePowValume(float volume)
		{
			double num = (double)volume;
			num *= 8.0;
			float num2 = 2f;
			double num3 = Math.Pow((double)num2, num) - 1.0;
			return (float)(num3 / (Math.Pow((double)num2, 8.0) - 1.0));
		}

		// Token: 0x04000852 RID: 2130
		public static int V_MAX = 100;

		// Token: 0x04000853 RID: 2131
		public static int COUNTER_CLOCKWISE = 0;

		// Token: 0x04000854 RID: 2132
		public static int CLOCKWISE = 1;

		// Token: 0x04000855 RID: 2133
		public static double BIG = 1E+30;

		// Token: 0x04000856 RID: 2134
		private static MTRand gCommonMTRand = new MTRand();

		// Token: 0x04000857 RID: 2135
		private static List<string> aPunctBuffer = new List<string>();

		// Token: 0x04000858 RID: 2136
		private static StringBuilder sbuider = new StringBuilder();

		// Token: 0x04000859 RID: 2137
		public static float JL_PI = 3.1415927f;

		// Token: 0x02000126 RID: 294
		public class Set<T> : List<T>
		{
			// Token: 0x060009C2 RID: 2498 RVA: 0x0003318E File Offset: 0x0003138E
			private void Add(T item)
			{
			}

			// Token: 0x060009C3 RID: 2499 RVA: 0x00033190 File Offset: 0x00031390
			public bool AddUnique(T obj)
			{
				if (typeof(T).IsValueType)
				{
					if (base.BinarySearch(obj) >= 0)
					{
						return false;
					}
					this.Add(obj);
					base.Sort();
				}
				else
				{
					if (base.IndexOf(obj) >= 0)
					{
						return false;
					}
					this.Add(obj);
				}
				return true;
			}
		}
	}
}
