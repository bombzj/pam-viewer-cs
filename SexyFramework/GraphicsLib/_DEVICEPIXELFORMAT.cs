using System;
using System.Runtime.InteropServices;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000A1 RID: 161
	public class _DEVICEPIXELFORMAT
	{
		// Token: 0x04000434 RID: 1076
		public uint dwSize;

		// Token: 0x04000435 RID: 1077
		public uint dwFlags;

		// Token: 0x04000436 RID: 1078
		public uint dwFourCC;

		// Token: 0x020000A2 RID: 162
		[StructLayout(LayoutKind.Explicit)]
		public struct AnonymousStruct8
		{
			// Token: 0x04000437 RID: 1079
			[FieldOffset(0)]
			public uint dwRGBBitCount;

			// Token: 0x04000438 RID: 1080
			[FieldOffset(0)]
			public uint dwYUVBitCount;

			// Token: 0x04000439 RID: 1081
			[FieldOffset(0)]
			public uint dwZBufferBitDepth;

			// Token: 0x0400043A RID: 1082
			[FieldOffset(0)]
			public uint dwAlphaBitDepth;

			// Token: 0x0400043B RID: 1083
			[FieldOffset(0)]
			public uint dwLuminanceBitCount;

			// Token: 0x0400043C RID: 1084
			[FieldOffset(0)]
			public uint dwBumpBitCount;

			// Token: 0x0400043D RID: 1085
			[FieldOffset(0)]
			public uint dwPrivateFormatBitCount;
		}

		// Token: 0x020000A3 RID: 163
		[StructLayout(LayoutKind.Explicit)]
		public struct AnonymousStruct9
		{
			// Token: 0x0400043E RID: 1086
			[FieldOffset(0)]
			public uint dwRBitMask;

			// Token: 0x0400043F RID: 1087
			[FieldOffset(0)]
			public uint dwYBitMask;

			// Token: 0x04000440 RID: 1088
			[FieldOffset(0)]
			public uint dwStencilBitDepth;

			// Token: 0x04000441 RID: 1089
			[FieldOffset(0)]
			public uint dwLuminanceBitMask;

			// Token: 0x04000442 RID: 1090
			[FieldOffset(0)]
			public uint dwBumpDuBitMask;

			// Token: 0x04000443 RID: 1091
			[FieldOffset(0)]
			public uint dwOperations;
		}

		// Token: 0x020000A4 RID: 164
		[StructLayout(LayoutKind.Explicit)]
		public struct AnonymousStruct10
		{
			// Token: 0x04000444 RID: 1092
			[FieldOffset(0)]
			public uint dwGBitMask;

			// Token: 0x04000445 RID: 1093
			[FieldOffset(0)]
			public uint dwUBitMask;

			// Token: 0x04000446 RID: 1094
			[FieldOffset(0)]
			public uint dwZBitMask;

			// Token: 0x04000447 RID: 1095
			[FieldOffset(0)]
			public uint dwBumpDvBitMask;

			// Token: 0x04000448 RID: 1096
			[FieldOffset(0)]
			public _DEVICEPIXELFORMAT.AnonymousStruct10.AnonymousClass10 MultiSampleCaps;

			// Token: 0x020000A5 RID: 165
			public struct AnonymousClass10
			{
				// Token: 0x04000449 RID: 1097
				public ushort wFlipMSTypes;

				// Token: 0x0400044A RID: 1098
				public ushort wBltMSTypes;
			}
		}

		// Token: 0x020000A6 RID: 166
		[StructLayout(LayoutKind.Explicit)]
		public struct AnonymousStruct11
		{
			// Token: 0x0400044B RID: 1099
			[FieldOffset(0)]
			public uint dwBBitMask;

			// Token: 0x0400044C RID: 1100
			[FieldOffset(0)]
			public uint dwVBitMask;

			// Token: 0x0400044D RID: 1101
			[FieldOffset(0)]
			public uint dwStencilBitMask;

			// Token: 0x0400044E RID: 1102
			[FieldOffset(0)]
			public uint dwBumpLuminanceBitMask;
		}

		// Token: 0x020000A7 RID: 167
		[StructLayout(LayoutKind.Explicit)]
		public struct AnonymousStruct12
		{
			// Token: 0x0400044F RID: 1103
			[FieldOffset(0)]
			public uint dwRGBAlphaBitMask;

			// Token: 0x04000450 RID: 1104
			[FieldOffset(0)]
			public uint dwYUVAlphaBitMask;

			// Token: 0x04000451 RID: 1105
			[FieldOffset(0)]
			public uint dwLuminanceAlphaBitMask;

			// Token: 0x04000452 RID: 1106
			[FieldOffset(0)]
			public uint dwRGBZBitMask;

			// Token: 0x04000453 RID: 1107
			[FieldOffset(0)]
			public uint dwYUVZBitMask;
		}
	}
}
