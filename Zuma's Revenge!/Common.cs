using System;
using System.Collections.Generic;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.PIL;
using SexyFramework.Resource;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x020000DE RID: 222
	public static class Common
	{
		// Token: 0x06000BE2 RID: 3042 RVA: 0x00070133 File Offset: 0x0006E333
		public static bool IsDeprecatedPowerUp(PowerType ptype)
		{
			return ptype == PowerType.PowerType_Fireball || ptype == PowerType.PowerType_ShieldFrog || ptype == PowerType.PowerType_FreezeBoss || ptype == PowerType.PowerType_BallEater || ptype == PowerType.PowerType_BombBullet || ptype == PowerType.PowerType_Lob;
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00070152 File Offset: 0x0006E352
		public static bool StrEquals(string str1, string str2, bool pIgnoreCase)
		{
			if (!pIgnoreCase)
			{
				return str1 == str2;
			}
			return string.Compare(str1, str2, true) == 0;
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x0007016A File Offset: 0x0006E36A
		public static bool StrEquals(string str1, string str2)
		{
			return Common.StrEquals(str1, str2, true);
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00070174 File Offset: 0x0006E374
		public static bool StrICaseEquals(string str1, string str2)
		{
			return string.Compare(str1, str2, true) == 0;
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00070181 File Offset: 0x0006E381
		public static int GetDefaultBallRadius()
		{
			if (GameApp.gApp.mGraphicsDriver.Is3D())
			{
				return 18;
			}
			return 17;
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00070199 File Offset: 0x0006E399
		public static int GetDefaultBallSize()
		{
			return Common.GetDefaultBallRadius() * 2;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x000701A4 File Offset: 0x0006E3A4
		public static void MirrorPoint(ref float x, ref float y, MirrorType theMirror)
		{
			switch (theMirror)
			{
			case MirrorType.MirrorType_X:
				x = (float)GameApp.gApp.mWidth - x;
				return;
			case MirrorType.MirrorType_Y:
				y = (float)GameApp.gApp.mHeight - y;
				return;
			case MirrorType.MirrorType_XY:
				x = (float)GameApp.gApp.mWidth - x;
				y = (float)GameApp.gApp.mHeight - y;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x0007020C File Offset: 0x0006E40C
		public static void SetupDialog(Dialog theDialog)
		{
			theDialog.SetHeaderFont(Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_YELLOW));
			theDialog.SetLinesFont(Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_YELLOW));
			theDialog.SetColor(0, new SexyColor(203, 201, 187));
			theDialog.SetColor(1, new SexyColor(244, 148, 28));
			theDialog.mPriority = 1;
			Common.SetupDialogButton(theDialog.mYesButton);
			Common.SetupDialogButton(theDialog.mNoButton);
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00070284 File Offset: 0x0006E484
		public static void SetupDialogButton(DialogButton theButton)
		{
			if (theButton == null)
			{
				return;
			}
			theButton.mTranslateX = -1;
			theButton.mTranslateY = 1;
			int mNumCols = theButton.mComponentImage.mNumCols;
			int num = theButton.mComponentImage.mWidth / mNumCols;
			int mHeight = theButton.mComponentImage.mHeight;
			if (mNumCols == 3)
			{
				theButton.mNormalRect = new Rect(0, 0, num, mHeight);
				theButton.mOverRect = new Rect(num, 0, num, mHeight);
				theButton.mDownRect = new Rect(num * 2, 0, num, mHeight);
			}
			theButton.SetFont(Res.GetFontByID(ResID.FONT_SHAGLOUNGE28_GREEN));
			theButton.SetColor(1, new SexyColor(16777215));
			theButton.mHasAlpha = true;
			theButton.mHasTransparencies = true;
			if (theButton.mWidth == 0)
			{
				int mX = theButton.mX;
				int mY = theButton.mY;
				int theWidth = theButton.mFont.StringWidth(theButton.mLabel);
				int mHeight2 = theButton.mComponentImage.mHeight;
				theButton.Resize(mX, mY, theWidth, mHeight2);
			}
			theButton.mIsDown = false;
			theButton.mIsOver = false;
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x0007037C File Offset: 0x0006E57C
		public static DialogButton MakeButton(int theId, ButtonListener theListener, string theText)
		{
			DialogButton dialogButton = new DialogButton(Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BUTTON), theId, theListener);
			dialogButton.mLabel = theText;
			Common.SetupDialogButton(dialogButton);
			return dialogButton;
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x000703AC File Offset: 0x0006E5AC
		public static DialogButton MakeButton(int theId, Image theButtonImage, ButtonListener theListener, string theText)
		{
			DialogButton dialogButton = new DialogButton(theButtonImage, theId, theListener);
			dialogButton.mLabel = theText;
			Common.SetupDialogButton(dialogButton);
			return dialogButton;
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x000703D0 File Offset: 0x0006E5D0
		public static void SizeButtonsToLabel(ButtonWidget[] inButtons, int inButtonCount, int inXPadding)
		{
			int num = 0;
			for (int i = 0; i < inButtonCount; i++)
			{
				ButtonWidget buttonWidget = inButtons[i];
				if (buttonWidget.mFont == null)
				{
					return;
				}
				int num2 = buttonWidget.mFont.StringWidth(buttonWidget.mLabel);
				if (num2 > num)
				{
					num = num2;
				}
			}
			num += inXPadding * 2;
			for (int j = 0; j < inButtonCount; j++)
			{
				ButtonWidget buttonWidget2 = inButtons[j];
				buttonWidget2.Resize((int)((float)buttonWidget2.mX - (float)(num - buttonWidget2.mWidth) * 0.5f), buttonWidget2.mY, num, buttonWidget2.mHeight);
			}
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x0007045C File Offset: 0x0006E65C
		public static void SetFXNumScale(PIEffect p, float scale)
		{
			if (p == null)
			{
				return;
			}
			int num = 0;
			for (;;)
			{
				PILayer layer = p.GetLayer(num);
				if (layer == null)
				{
					break;
				}
				int num2 = 0;
				for (;;)
				{
					PIEmitterInstance emitter = layer.GetEmitter(num2);
					if (emitter == null)
					{
						break;
					}
					emitter.mNumberScale = scale;
					num2++;
				}
				num++;
			}
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x0007049C File Offset: 0x0006E69C
		public static void DrawCommonDialogBorder(Graphics g, int x, int y, int width, int height)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_BAMBOOTOPEDGE);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_GUI_BAMBOOBOTEDGE);
			Image imageByID3 = Res.GetImageByID(ResID.IMAGE_GUI_BAMBOOBOT);
			Image imageByID4 = Res.GetImageByID(ResID.IMAGE_GUI_BAMBOOSIDE);
			g.SetColorizeImages(false);
			g.ClearClipRect();
			g.DrawImage(imageByID, x, y);
			g.DrawImageMirror(imageByID, x + width - imageByID.GetWidth(), y);
			g.DrawImage(imageByID2, x, y + height - imageByID2.GetHeight());
			g.DrawImageMirror(imageByID2, x + width - imageByID2.GetWidth(), y + height - imageByID2.GetHeight());
			g.SetClipRect(x + imageByID.GetWidth(), y, width - imageByID.GetWidth() * 2, height);
			for (int i = x + imageByID.GetWidth(); i < x + width - imageByID.GetWidth(); i += imageByID3.GetWidth())
			{
				g.DrawImage(imageByID3, i, y - 1);
				g.DrawImage(imageByID3, i, y + height - imageByID3.GetHeight() + 1);
			}
			g.ClearClipRect();
			g.SetClipRect(x, y + imageByID.GetHeight(), width, height - imageByID.GetHeight() * 2);
			for (int j = y + imageByID.GetHeight(); j < y + height - imageByID.GetHeight(); j += imageByID4.GetHeight())
			{
				g.DrawImage(imageByID4, x, j);
				g.DrawImage(imageByID4, x + width - imageByID4.GetWidth(), j);
			}
			g.ClearClipRect();
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x000705F8 File Offset: 0x0006E7F8
		public static int _GetWordWrappedHeight(string inText, Font inFont, int inWidth)
		{
			List<string> list = Common.Split(inText);
			int num = 0;
			int num2 = 1;
			int num3 = inFont.CharWidth(' ');
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == "\n")
				{
					num2++;
					num = 0;
				}
				else
				{
					int num4 = inFont.StringWidth(list[i]);
					if (num + num4 + num3 <= inWidth)
					{
						num += num4 + num3;
					}
					else if (num + num4 <= inWidth)
					{
						num += num4;
					}
					else
					{
						num2++;
						num = num4 + num3;
					}
				}
			}
			int num5 = inFont.GetHeight() - inFont.GetAscent();
			return num2 * inFont.GetHeight() - num5;
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x000706A4 File Offset: 0x0006E8A4
		public static void DrawCommonDialogBacking(Graphics g, int x, int y, int width, int height)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BOX_MAINMENU_FRAME_WOOD);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_GUI_BAMBOOSIDE);
			Image imageByID3 = Res.GetImageByID(ResID.IMAGE_GUI_BAMBOOBOT);
			Image imageByID4 = Res.GetImageByID(ResID.IMAGE_GUI_BAMBOOTOPEDGE);
			Image imageByID5 = Res.GetImageByID(ResID.IMAGE_GUI_BAMBOOBOTEDGE);
			g.ClearClipRect();
			g.ClipRect(x + imageByID2.GetWidth() - 2, y + imageByID3.GetHeight() - 3, width + 4 - imageByID2.GetWidth() * 2, height + 10 - imageByID3.GetHeight() * 2);
			int i = x;
			int j = y;
			bool flag = false;
			while (j <= y + height + imageByID.GetHeight())
			{
				while (i < x + width + imageByID.GetWidth())
				{
					if (flag)
					{
						g.DrawImageMirror(imageByID, i, j);
					}
					else
					{
						g.DrawImage(imageByID, i, j);
					}
					i += imageByID.GetWidth();
					flag = !flag;
				}
				i = x;
				j += imageByID.GetHeight();
			}
			g.ClearClipRect();
			g.DrawImage(imageByID4, x, y);
			g.DrawImageMirror(imageByID4, x + width - imageByID4.GetWidth(), y);
			g.DrawImage(imageByID5, x, y + height - imageByID5.GetHeight());
			g.DrawImageMirror(imageByID5, x + width - imageByID5.GetWidth(), y + height - imageByID5.GetHeight());
			g.SetClipRect(x + imageByID4.GetWidth(), y, width - imageByID4.GetWidth() * 2, height);
			for (int k = x + imageByID4.GetWidth(); k < x + width - imageByID4.GetWidth(); k += imageByID3.GetWidth())
			{
				g.DrawImage(imageByID3, k, y - 1);
				g.DrawImage(imageByID3, k, y + height - imageByID3.GetHeight() + 1);
			}
			g.ClearClipRect();
			g.SetClipRect(x, y + imageByID4.GetHeight(), width, height - imageByID4.GetHeight() * 2);
			for (int l = y + imageByID4.GetHeight(); l < y + height - imageByID4.GetHeight(); l += imageByID2.GetHeight())
			{
				g.DrawImage(imageByID2, x, l);
				g.DrawImage(imageByID2, x + width - imageByID2.GetWidth(), l);
			}
			g.ClearClipRect();
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x000708A8 File Offset: 0x0006EAA8
		public static bool ExtractAdventureStatsResources(ResourceManager res)
		{
			return true;
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x000708AB File Offset: 0x0006EAAB
		public static int GetIdByStringId(string theStringId)
		{
			return 0;
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x000708B0 File Offset: 0x0006EAB0
		public static int GetBoardStateCount()
		{
			Board board = ((GameApp)GlobalMembers.gSexyApp).GetBoard();
			if (board == null)
			{
				return 0;
			}
			return board.GetStateCount();
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x000708D8 File Offset: 0x0006EAD8
		public static uint GetBoardTickCount()
		{
			return (uint)(Common.GetBoardStateCount() * 10);
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x000708E2 File Offset: 0x0006EAE2
		public static float _S(float value)
		{
			return GameApp.ScaleNum(value);
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x000708EA File Offset: 0x0006EAEA
		public static int _S(int value)
		{
			return GameApp.ScaleNum(value);
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x000708F2 File Offset: 0x0006EAF2
		public static float _SS(float value)
		{
			return GameApp.ScreenScaleNum(value);
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x000708FA File Offset: 0x0006EAFA
		public static int _SS(int value)
		{
			return GameApp.ScreenScaleNum(value);
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x00070902 File Offset: 0x0006EB02
		public static string _MP(string value)
		{
			return value;
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x00070905 File Offset: 0x0006EB05
		public static float _M(float value)
		{
			return value;
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x00070908 File Offset: 0x0006EB08
		public static float _M1(float value)
		{
			return Common._M(value);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00070910 File Offset: 0x0006EB10
		public static float _M2(float value)
		{
			return Common._M(value);
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00070918 File Offset: 0x0006EB18
		public static float _M3(float value)
		{
			return Common._M(value);
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00070920 File Offset: 0x0006EB20
		public static float _M4(float value)
		{
			return Common._M(value);
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00070928 File Offset: 0x0006EB28
		public static float _M5(float value)
		{
			return Common._M(value);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00070930 File Offset: 0x0006EB30
		public static float _M6(float value)
		{
			return Common._M(value);
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00070938 File Offset: 0x0006EB38
		public static float _M7(float value)
		{
			return Common._M(value);
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00070940 File Offset: 0x0006EB40
		public static int _M(int value)
		{
			return value;
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00070943 File Offset: 0x0006EB43
		public static int _M1(int value)
		{
			return Common._M(value);
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0007094B File Offset: 0x0006EB4B
		public static int _M2(int value)
		{
			return Common._M(value);
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00070953 File Offset: 0x0006EB53
		public static int _M3(int value)
		{
			return Common._M(value);
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0007095B File Offset: 0x0006EB5B
		public static int _M4(int value)
		{
			return Common._M(value);
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00070963 File Offset: 0x0006EB63
		public static int _M5(int value)
		{
			return Common._M(value);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0007096B File Offset: 0x0006EB6B
		public static int _M6(int value)
		{
			return Common._M(value);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00070973 File Offset: 0x0006EB73
		public static int _M7(int value)
		{
			return Common._M(value);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0007097B File Offset: 0x0006EB7B
		public static int _M8(int value)
		{
			return Common._M(value);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00070983 File Offset: 0x0006EB83
		public static int _M9(int value)
		{
			return Common._M(value);
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x0007098B File Offset: 0x0006EB8B
		public static float _SA(float value, float add)
		{
			return value;
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0007098E File Offset: 0x0006EB8E
		public static float _DS(float value)
		{
			return GameApp.DownScaleNum(value);
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00070996 File Offset: 0x0006EB96
		public static int _DS(int value)
		{
			return GameApp.DownScaleNum(value);
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0007099E File Offset: 0x0006EB9E
		public static float _DSA(float value, float add)
		{
			return GameApp.DownScaleNum(value, add);
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x000709A8 File Offset: 0x0006EBA8
		public static List<string> Split(string inText)
		{
			Common.mTotalWords.Clear();
			string[] array = inText.Split(new char[] { '\n' });
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[] { ' ' });
				for (int j = 0; j < array2.Length; j++)
				{
					Common.mTotalWords.Add(array2[j]);
				}
				if (array.Length > 1)
				{
					Common.mTotalWords.Add("\n");
				}
			}
			return Common.mTotalWords;
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00070A32 File Offset: 0x0006EC32
		public static bool BossLevel(Level level)
		{
			return level != null && (level.IsFinalBossLevel() || level.mBoss != null || level.mEndSequence > 0);
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00070A54 File Offset: 0x0006EC54
		public static string PowerupToStr(PowerType t, bool all_caps)
		{
			int id = 0;
			switch (t)
			{
			case PowerType.PowerType_ProximityBomb:
				id = (all_caps ? 696 : 697);
				break;
			case PowerType.PowerType_SlowDown:
				id = (all_caps ? 698 : 699);
				break;
			case PowerType.PowerType_Accuracy:
				id = (all_caps ? 700 : 701);
				break;
			case PowerType.PowerType_MoveBackwards:
				id = (all_caps ? 702 : 703);
				break;
			case PowerType.PowerType_Cannon:
				id = (all_caps ? 704 : 705);
				break;
			case PowerType.PowerType_ColorNuke:
				id = (all_caps ? 706 : 707);
				break;
			case PowerType.PowerType_Laser:
				id = (all_caps ? 708 : 709);
				break;
			case PowerType.PowerType_GauntletMultBall:
				id = (all_caps ? 710 : 711);
				break;
			}
			return TextManager.getInstance().getString(id);
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00070B49 File Offset: 0x0006ED49
		public static bool LinesIntersect(FPoint a1, FPoint a2, FPoint b1, FPoint b2)
		{
			return Common.LinesIntersect(a1, a2, b1, b2, null);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00070B58 File Offset: 0x0006ED58
		public static bool LinesIntersect(FPoint a1, FPoint a2, FPoint b1, FPoint b2, FPoint intersectFPoint)
		{
			if ((a1.mX == a2.mX && a1.mY == a2.mY) || (b1.mX == b2.mX && b1.mY == b2.mY))
			{
				return false;
			}
			a2.mX -= a1.mX;
			a2.mY -= a1.mY;
			b1.mX -= a1.mX;
			b1.mY -= a1.mY;
			b2.mX -= a1.mX;
			b2.mY -= a1.mY;
			double num = Math.Sqrt((double)(a2.mX * a2.mX + a2.mY * a2.mY));
			double num2 = (double)a2.mX / num;
			double num3 = (double)a2.mY / num;
			double num4 = (double)b1.mX * num2 + (double)b1.mY * num3;
			b1.mY = (float)((double)b1.mY * num2 - (double)b1.mX * num3);
			b1.mX = (float)num4;
			num4 = (double)b2.mX * num2 + (double)b2.mY * num3;
			b2.mY = (float)((double)b2.mY * num2 - (double)b2.mX * num3);
			b2.mX = (float)num4;
			if ((b1.mY < 0f && b2.mY < 0f) || (b1.mY >= 0f && b2.mY >= 0f))
			{
				return false;
			}
			double num5 = (double)(b2.mX + (b1.mX - b2.mX) * b2.mY / (b2.mY - b1.mY));
			if (num5 < 0.0 || num5 > num)
			{
				return false;
			}
			if (intersectFPoint != null)
			{
				intersectFPoint.mX = (float)((double)a1.mX + num5 * num2);
				intersectFPoint.mY = (float)((double)a1.mY + num5 * num3);
			}
			return true;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00070D5C File Offset: 0x0006EF5C
		public static float GetCanonicalAngleRad(float theRad)
		{
			if (theRad >= 0f && theRad < 6.2831855f)
			{
				return theRad;
			}
			return Common.AceModF(theRad, 6.2831855f);
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x00070D7B File Offset: 0x0006EF7B
		private static float AceModF(float x, float y)
		{
			if (x < 0f)
			{
				return y - -x % y;
			}
			return x % y;
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00070D8F File Offset: 0x0006EF8F
		public static string PILGetNameByImage(Image img)
		{
			return img.mNameForRes;
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00070D98 File Offset: 0x0006EF98
		public static Image PILGetImageByName(string name)
		{
			SharedImageRef sharedImageRef = GameApp.gApp.mResourceManager.LoadImage(name);
			if (sharedImageRef != null)
			{
				return sharedImageRef.GetImage();
			}
			return null;
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00070DC1 File Offset: 0x0006EFC1
		public static int PILGetIDByImage(Image img)
		{
			return Res.GetIDByImage(img);
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00070DC9 File Offset: 0x0006EFC9
		public static Image PILGetImageByID(int id)
		{
			return Res.GetImageByID((ResID)id);
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00070DD4 File Offset: 0x0006EFD4
		public static void SerializePIEffect(PIEffect s, DataSync sync)
		{
			SexyBuffer buffer = new SexyBuffer();
			s.SaveState(buffer);
			SexyBuffer buffer2 = sync.GetBuffer();
			buffer2.WriteLong((long)buffer.GetDataLen());
			buffer2.WriteBytes(buffer.GetDataPtr(), buffer.GetDataLen());
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00070E18 File Offset: 0x0006F018
		public static void DeserializePIEffect(PIEffect s, DataSync sync)
		{
			SexyBuffer buffer = sync.GetBuffer();
			int num = (int)buffer.ReadLong();
			byte[] thePtr = new byte[num];
			buffer.ReadBytes(ref thePtr, num);
			SexyBuffer buffer2 = new SexyBuffer();
			buffer2.SetData(thePtr, num);
			s.LoadState(buffer2);
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00070E5C File Offset: 0x0006F05C
		public static void SerializeParticleSystem(SexyFramework.PIL.System s, DataSync sync)
		{
			SexyBuffer buffer = new SexyBuffer();
			s.Serialize(buffer, new GlobalMembers.GetIdByImageFunc(Common.PILGetIDByImage));
			SexyBuffer buffer2 = sync.GetBuffer();
			buffer2.WriteLong((long)buffer.GetDataLen());
			buffer2.WriteBytes(buffer.GetDataPtr(), buffer.GetDataLen());
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00070EA8 File Offset: 0x0006F0A8
		public static SexyFramework.PIL.System DeserializeParticleSystem(DataSync sync)
		{
			SexyBuffer buffer = sync.GetBuffer();
			int num = (int)buffer.ReadLong();
			byte[] thePtr = new byte[num];
			buffer.ReadBytes(ref thePtr, num);
			SexyBuffer buffer2 = new SexyBuffer();
			buffer2.SetData(thePtr, num);
			SexyFramework.PIL.System system = SexyFramework.PIL.System.Deserialize(buffer2, new GlobalMembers.GetImageByIdFunc(Common.PILGetImageByID));
			system.mScale = Common._S(1f);
			return system;
		}

		// Token: 0x04000A5F RID: 2655
		public const int MIN_LEVEL_FOR_BRONZE = 5;

		// Token: 0x04000A60 RID: 2656
		public const int MIN_LEVEL_FOR_SILVER = 10;

		// Token: 0x04000A61 RID: 2657
		public const int MIN_LEVEL_FOR_GOLD = 15;

		// Token: 0x04000A62 RID: 2658
		public const int MAX_DRAW_PRIORITY = 5;

		// Token: 0x04000A63 RID: 2659
		public const float MY_PI = 3.14159f;

		// Token: 0x04000A64 RID: 2660
		public const int MAX_CURVES = 4;

		// Token: 0x04000A65 RID: 2661
		public const int MAX_GUN_POINTS = 5;

		// Token: 0x04000A66 RID: 2662
		public const int POINTS_FOR_EXTRA_LIFE = 50000;

		// Token: 0x04000A67 RID: 2663
		public const int HOLE_SIZE = 96;

		// Token: 0x04000A68 RID: 2664
		public const int PROXIMITY_BOMB_RADIUS = 56;

		// Token: 0x04000A69 RID: 2665
		public const float EPSILON = 1E-06f;

		// Token: 0x04000A6A RID: 2666
		public const float JL_PI = 3.1415927f;

		// Token: 0x04000A6B RID: 2667
		public const float M_PI = 3.14159f;

		// Token: 0x04000A6C RID: 2668
		public const float FLT_MAX = 3.4028235E+38f;

		// Token: 0x04000A6D RID: 2669
		public const int MUSIC_LOADING = 0;

		// Token: 0x04000A6E RID: 2670
		public const int MUSIC_MENU = 1;

		// Token: 0x04000A6F RID: 2671
		public const int MUSIC_TUNE1 = 12;

		// Token: 0x04000A70 RID: 2672
		public const int MUSIC_TUNE2 = 24;

		// Token: 0x04000A71 RID: 2673
		public const int MUSIC_TUNE3 = 35;

		// Token: 0x04000A72 RID: 2674
		public const int MUSIC_TUNE4 = 45;

		// Token: 0x04000A73 RID: 2675
		public const int MUSIC_TUNE5 = 58;

		// Token: 0x04000A74 RID: 2676
		public const int MUSIC_TUNE6 = 71;

		// Token: 0x04000A75 RID: 2677
		public const int MUSIC_INTRO1 = 12;

		// Token: 0x04000A76 RID: 2678
		public const int MUSIC_INTRO2 = 24;

		// Token: 0x04000A77 RID: 2679
		public const int MUSIC_INTRO3 = 35;

		// Token: 0x04000A78 RID: 2680
		public const int MUSIC_INTRO4 = 45;

		// Token: 0x04000A79 RID: 2681
		public const int MUSIC_INTRO5 = 58;

		// Token: 0x04000A7A RID: 2682
		public const int MUSIC_INTRO6 = 71;

		// Token: 0x04000A7B RID: 2683
		public const int MUSIC_HI_SCORE = 116;

		// Token: 0x04000A7C RID: 2684
		public const int MUSIC_GAME_OVER = 126;

		// Token: 0x04000A7D RID: 2685
		public const int MUSIC_WON1 = 120;

		// Token: 0x04000A7E RID: 2686
		public const int MUSIC_WON2 = 121;

		// Token: 0x04000A7F RID: 2687
		public const int MUSIC_WON3 = 122;

		// Token: 0x04000A80 RID: 2688
		public const int MUSIC_WON4 = 123;

		// Token: 0x04000A81 RID: 2689
		public const int MUSIC_WON5 = 124;

		// Token: 0x04000A82 RID: 2690
		public const int MUSIC_WON6 = 125;

		// Token: 0x04000A83 RID: 2691
		public const int MUSIC_BOSS = 127;

		// Token: 0x04000A84 RID: 2692
		public const int MUSIC_BOSS_WIN = 137;

		// Token: 0x04000A85 RID: 2693
		public const int MUSIC_BONUS = 138;

		// Token: 0x04000A86 RID: 2694
		public const int MUSIC_WON_GAME = 144;

		// Token: 0x04000A87 RID: 2695
		public const int MUSIC_MISC1 = 95;

		// Token: 0x04000A88 RID: 2696
		public const int MUSIC_MISC2 = 100;

		// Token: 0x04000A89 RID: 2697
		public const int MUSIC_MISC3 = 105;

		// Token: 0x04000A8A RID: 2698
		public const int MUSIC_MISC4 = 110;

		// Token: 0x04000A8B RID: 2699
		public const int MUSIC_DANGER1 = 32;

		// Token: 0x04000A8C RID: 2700
		public const int MUSIC_DANGER2 = 33;

		// Token: 0x04000A8D RID: 2701
		public const int MUSIC_DANGER3 = 34;

		// Token: 0x04000A8E RID: 2702
		public static List<string> mTotalWords = new List<string>();

		// Token: 0x04000A8F RID: 2703
		public static bool[] gGotPowerUp = new bool[14];

		// Token: 0x04000A90 RID: 2704
		public static bool gSuckMode = false;

		// Token: 0x04000A91 RID: 2705
		public static bool gDieAtEnd = true;

		// Token: 0x04000A92 RID: 2706
		public static bool gAddBalls = true;

		// Token: 0x04000A93 RID: 2707
		public static int[] gBallColors = new int[] { 1671423, 16776960, 16711680, 65280, 16711935, 16777215 };

		// Token: 0x04000A94 RID: 2708
		public static int[] gBrightBallColors = new int[] { 8454143, 16777024, 16755370, 8454016, 16744703, 16777215 };

		// Token: 0x04000A95 RID: 2709
		public static int[] gDarkBallColors = new int[] { 2299513, 6312202, 10489620, 2114594, 5641795, 3676962 };

		// Token: 0x04000A96 RID: 2710
		public static int[] gTextBallColors = new int[] { 2984959, 16776960, 16711680, 65280, 16711935, 16777215 };
	}
}
