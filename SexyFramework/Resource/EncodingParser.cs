using System;
using System.Collections.Generic;
using System.Text;

namespace SexyFramework.Resource
{
	// Token: 0x020000CB RID: 203
	public class EncodingParser : IDisposable
	{
		// Token: 0x06000626 RID: 1574 RVA: 0x000160F2 File Offset: 0x000142F2
		public EncodingParser()
		{
			this.mFile = null;
			this.mForcedEncodingType = false;
			this.mEncodingType = EncodingParser.EncodingType.UTF_8;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0001611A File Offset: 0x0001431A
		public virtual void Dispose()
		{
			this.mBufferedText.Clear();
			this.mBufferedText = null;
			if (this.mFile != null)
			{
				this.mFile.Close();
			}
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00016141 File Offset: 0x00014341
		public virtual void SetEncodingType(EncodingParser.EncodingType theEncoding)
		{
			this.mEncodingType = theEncoding;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0001614C File Offset: 0x0001434C
		public virtual void checkEncodingType(byte[] data)
		{
			this.mEncodingType = EncodingParser.EncodingType.ASCII;
			if (data.Length >= 2)
			{
				int num = (int)data[0];
				int num2 = (int)data[1];
				if ((num == 255 && num2 == 254) || (num == 254 && num2 == 255))
				{
					this.mEncodingType = EncodingParser.EncodingType.UTF_16;
				}
			}
			if (this.mEncodingType == EncodingParser.EncodingType.ASCII && data.Length >= 3)
			{
				int num3 = (int)data[0];
				int num4 = (int)data[1];
				int num5 = (int)data[2];
				if (num3 == 239 && num4 == 187 && num5 == 191)
				{
					this.mEncodingType = EncodingParser.EncodingType.UTF_8;
				}
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x000161D0 File Offset: 0x000143D0
		public virtual bool OpenFile(string theFilename)
		{
			this.mFile = new PFILE(theFilename, "rb");
			if (!this.mFile.Open())
			{
				this.mFile = null;
				return false;
			}
			byte[] data = this.mFile.GetData();
			if (data == null)
			{
				return false;
			}
			if (!this.mForcedEncodingType)
			{
				this.checkEncodingType(data);
			}
			this.SetBytes(data);
			return true;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001622C File Offset: 0x0001442C
		public virtual bool CloseFile()
		{
			if (this.mFile != null)
			{
				this.mFile.Close();
			}
			return true;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00016242 File Offset: 0x00014442
		public virtual bool EndOfFile()
		{
			return this.mBufferedText.Count <= 0 && (this.mFile == null || this.mFile.IsEndOfFile());
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001626C File Offset: 0x0001446C
		public virtual void SetStringSource(string theString)
		{
			int length = theString.Length;
			this.mBufferedText.Clear();
			for (int i = 0; i < length; i++)
			{
				this.mBufferedText.Push(theString[length - i - 1]);
			}
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x000162B0 File Offset: 0x000144B0
		public virtual void SetBytes(byte[] data)
		{
			switch (this.mEncodingType)
			{
			case EncodingParser.EncodingType.ASCII:
			{
				char[] array = new char[data.Length];
				for (int i = 0; i < data.Length; i++)
				{
					array[i] = (char)data[i];
				}
				this.SetStringSource(new string(array));
				return;
			}
			case EncodingParser.EncodingType.UTF_8:
			{
				char[] chars = Encoding.UTF8.GetChars(data);
				this.SetStringSource(new string(chars));
				return;
			}
			case EncodingParser.EncodingType.UTF_16:
			{
				char[] chars2 = Encoding.Unicode.GetChars(data);
				this.SetStringSource(new string(chars2));
				return;
			}
			case EncodingParser.EncodingType.UTF_16_LE:
				throw new NotImplementedException();
			case EncodingParser.EncodingType.UTF_16_BE:
				throw new NotImplementedException();
			default:
				return;
			}
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00016348 File Offset: 0x00014548
		public virtual EncodingParser.GetCharReturnType GetChar(ref char theChar)
		{
			if (this.mBufferedText.Count != 0)
			{
				theChar = this.mBufferedText.Peek();
				this.mBufferedText.Pop();
				return EncodingParser.GetCharReturnType.SUCCESSFUL;
			}
			if (this.mFile == null || (this.mFile != null && !this.mFile.Open()))
			{
				return EncodingParser.GetCharReturnType.END_OF_FILE;
			}
			bool flag = false;
			if (flag)
			{
				return EncodingParser.GetCharReturnType.INVALID_CHARACTER;
			}
			return EncodingParser.GetCharReturnType.END_OF_FILE;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x000163A4 File Offset: 0x000145A4
		public virtual bool PutChar(char theChar)
		{
			this.mBufferedText.Push(theChar);
			return true;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x000163B4 File Offset: 0x000145B4
		public virtual bool PutString(string theString)
		{
			int length = theString.Length;
			for (int i = 0; i < length; i++)
			{
				this.mBufferedText.Push(theString[length - i - 1]);
			}
			return true;
		}

		// Token: 0x0400051F RID: 1311
		protected PFILE mFile;

		// Token: 0x04000520 RID: 1312
		private Stack<char> mBufferedText = new Stack<char>();

		// Token: 0x04000521 RID: 1313
		private bool mForcedEncodingType;

		// Token: 0x04000522 RID: 1314
		private EncodingParser.EncodingType mEncodingType;

		// Token: 0x020000CC RID: 204
		public enum EncodingType
		{
			// Token: 0x04000524 RID: 1316
			ASCII,
			// Token: 0x04000525 RID: 1317
			UTF_8,
			// Token: 0x04000526 RID: 1318
			UTF_16,
			// Token: 0x04000527 RID: 1319
			UTF_16_LE,
			// Token: 0x04000528 RID: 1320
			UTF_16_BE
		}

		// Token: 0x020000CD RID: 205
		public enum GetCharReturnType
		{
			// Token: 0x0400052A RID: 1322
			SUCCESSFUL,
			// Token: 0x0400052B RID: 1323
			INVALID_CHARACTER,
			// Token: 0x0400052C RID: 1324
			END_OF_FILE,
			// Token: 0x0400052D RID: 1325
			FAILURE
		}

		// Token: 0x020000CE RID: 206
		// (Invoke) Token: 0x06000633 RID: 1587
		private delegate bool GetCharFunc(char theChar, ref bool error);
	}
}
