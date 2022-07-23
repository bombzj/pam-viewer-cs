using System;
using System.IO;

namespace SexyFramework.AELib
{
	// Token: 0x02000004 RID: 4
	internal class ParseByteArray
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000020BC File Offset: 0x000002BC
		public ParseByteArray(byte[] d)
		{
			if (d != null)
			{
				this.stream = new MemoryStream(d);
				this.reader = new BinaryReader(this.stream);
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020E4 File Offset: 0x000002E4
		public bool isEnd()
		{
			return this.stream.Position >= this.stream.Length;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002101 File Offset: 0x00000301
		public bool readBoolean(ref bool value)
		{
			value = this.reader.ReadBoolean();
			return true;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002111 File Offset: 0x00000311
		public bool readInt32(ref int value)
		{
			value = this.reader.ReadInt32();
			return true;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002121 File Offset: 0x00000321
		public bool readLong(ref long value)
		{
			value = (long)this.reader.ReadInt32();
			return true;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002132 File Offset: 0x00000332
		public bool readDouble(ref double value)
		{
			value = this.reader.ReadDouble();
			return true;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002144 File Offset: 0x00000344
		public bool readString(ref string value, int len)
		{
			char[] array = this.reader.ReadChars(len);
			string text = "";
			int num = 0;
			while (num < array.Length && array[num] != '\0')
			{
				text += array[num];
				num++;
			}
			value = text;
			return true;
		}

		// Token: 0x04000006 RID: 6
		private MemoryStream stream;

		// Token: 0x04000007 RID: 7
		private BinaryReader reader;
	}
}
