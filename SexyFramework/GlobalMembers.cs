using System;
using Microsoft.Xna.Framework;
using SexyFramework.Drivers;
using SexyFramework.GraphicsLib;
using SexyFramework.PIL;

namespace SexyFramework
{
	// Token: 0x0200007D RID: 125
	public static class GlobalMembers
	{
		// Token: 0x0400027C RID: 636
		public static SexyAppBase gSexyAppBase = null;

		// Token: 0x0400027D RID: 637
		public static SexyApp gSexyApp = null;

		// Token: 0x0400027E RID: 638
		public static bool gIs3D = false;

		// Token: 0x0400027F RID: 639
		public static IFileDriver gFileDriver = null;

		// Token: 0x04000280 RID: 640
		public static int gTotalGraphicsMemory = 0;

		// Token: 0x04000281 RID: 641
		public static float SEXYMATH_PI = 3.1415927f;

		// Token: 0x04000282 RID: 642
		public static float SEXYMATH_2PI = 6.2831855f;

		// Token: 0x04000283 RID: 643
		public static float SEXYMATH_E = 2.71828f;

		// Token: 0x04000284 RID: 644
		public static float SEXYMATH_EPSILON = 0.001f;

		// Token: 0x04000285 RID: 645
		public static float SEXYMATH_EPSILONSQ = 1E-06f;

		// Token: 0x04000286 RID: 646
		public static bool IsBackButtonPressed = false;

		// Token: 0x04000287 RID: 647
		public static Vector2 NO_TOUCH_MOUSE_POS = new Vector2(-1f, -1f);

		// Token: 0x04000288 RID: 648
		public static int[,] gButtonWidgetColors = new int[,]
		{
			{ 0, 0, 0 },
			{ 0, 0, 0 },
			{ 0, 0, 0 },
			{ 255, 255, 255 },
			{ 132, 132, 132 },
			{ 212, 212, 212 }
		};

		// Token: 0x04000289 RID: 649
		public static int[,] gDialogButtonColors = new int[,]
		{
			{ 255, 255, 255 },
			{ 255, 255, 255 },
			{ 0, 0, 0 },
			{ 255, 255, 255 },
			{ 132, 132, 132 },
			{ 212, 212, 212 }
		};

		// Token: 0x0400028A RID: 650
		public static int[,] gDialogColors = new int[,]
		{
			{ 255, 255, 255 },
			{ 255, 255, 0 },
			{ 255, 255, 255 },
			{ 255, 255, 255 },
			{ 255, 255, 255 },
			{ 80, 80, 80 },
			{ 255, 255, 255 }
		};

		// Token: 0x0400028B RID: 651
		public static string DIALOG_YES_STRING = "YES";

		// Token: 0x0400028C RID: 652
		public static string DIALOG_NO_STRING = "NO";

		// Token: 0x0400028D RID: 653
		public static string DIALOG_OK_STRING = "OK";

		// Token: 0x0400028E RID: 654
		public static string DIALOG_CANCEL_STRING = "CANCEL";

		// Token: 0x0200007E RID: 126
		// (Invoke) Token: 0x0600047B RID: 1147
		public delegate int GetIdByImageFunc(Image img);

		// Token: 0x0200007F RID: 127
		// (Invoke) Token: 0x0600047F RID: 1151
		public delegate Image GetImageByIdFunc(int id);

		// Token: 0x02000080 RID: 128
		// (Invoke) Token: 0x06000483 RID: 1155
		public delegate KeyFrameData KFDInstantiateFunc();
	}
}
