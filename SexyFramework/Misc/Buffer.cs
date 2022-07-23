using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SexyFramework.Misc
{
	// Token: 0x02000124 RID: 292
	public class SexyBuffer
	{
		// Token: 0x0600093F RID: 2367 RVA: 0x00030F40 File Offset: 0x0002F140
		public SexyBuffer()
		{
			this.mData = new List<byte>();
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00030F53 File Offset: 0x0002F153
		public void SeekFront()
		{
			this.mReadBitPos = 0;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00030F5C File Offset: 0x0002F15C
		public void Clear()
		{
			this.mReadBitPos = 0;
			this.mWriteBitPos = 0;
			this.mDataBitSize = 0;
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00030F73 File Offset: 0x0002F173
		public void Resize(int bytes)
		{
			this.Clear();
			this.mDataBitSize = bytes * 8;
			this.mData.Resize(bytes);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00030F90 File Offset: 0x0002F190
		public void FromWebString(string theString)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00030F97 File Offset: 0x0002F197
		public string ToWebString()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x00030FA0 File Offset: 0x0002F1A0
		public string UTF8ToWideString()
		{
			byte[] dataPtr = this.GetDataPtr();
			int i = this.GetDataLen();
			bool flag = true;
			string text = "";
			int num = 0;
			while (i > 0)
			{
				string text2 = "";
				int nextUTF8CharFromStream = SexyBuffer.GetNextUTF8CharFromStream(dataPtr, num, i, ref text2);
				if (nextUTF8CharFromStream == 0)
				{
					break;
				}
				i -= nextUTF8CharFromStream;
				num += nextUTF8CharFromStream;
				if (flag)
				{
					flag = false;
					if (text2[0] == '\ufeff')
					{
						continue;
					}
				}
				text += text2;
			}
			return text;
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x00031010 File Offset: 0x0002F210
		public void WriteByte(byte theByte)
		{
			if (this.mWriteBitPos % 8 == 0)
			{
				this.mData.Add(theByte);
			}
			else
			{
				int num = this.mWriteBitPos % 8;
				List<byte> list;
				int num2;
				(list = this.mData)[num2 = this.mWriteBitPos / 8] = (byte)(list[num2] | (byte)(theByte << num));
				this.mData.Add((byte)(theByte >> 8 - num));
			}
			this.mWriteBitPos += 8;
			if (this.mWriteBitPos > this.mDataBitSize)
			{
				this.mDataBitSize = this.mWriteBitPos;
			}
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x000310A4 File Offset: 0x0002F2A4
		public void WriteNumBits(int theNum, int theBits)
		{
			for (int i = 0; i < theBits; i++)
			{
				if (this.mWriteBitPos % 8 == 0)
				{
					this.mData.Add(0);
				}
				if ((theNum & (1 << i)) != 0)
				{
					List<byte> list;
					int num;
					(list = this.mData)[num = this.mWriteBitPos / 8] = (byte)(list[num] | (byte)(1 << this.mWriteBitPos % 8));
				}
				this.mWriteBitPos++;
			}
			if (this.mWriteBitPos > this.mDataBitSize)
			{
				this.mDataBitSize = this.mWriteBitPos;
			}
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00031134 File Offset: 0x0002F334
		public static int GetBitsRequired(int theNum, bool isSigned)
		{
			if (theNum < 0)
			{
				theNum = -theNum - 1;
			}
			int num = 0;
			while (theNum >= 1 << num)
			{
				num++;
			}
			if (isSigned)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00031164 File Offset: 0x0002F364
		public void WriteBoolean(bool theBool)
		{
			this.WriteByte((byte)(theBool ? 1 : 0));
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00031173 File Offset: 0x0002F373
		public void WriteShort(short theShort)
		{
			this.WriteByte((byte)theShort);
			this.WriteByte((byte)(theShort >> 8));
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x00031187 File Offset: 0x0002F387
		public void WriteLong(long theLong)
		{
			this.WriteByte((byte)theLong);
			this.WriteByte((byte)(theLong >> 8));
			this.WriteByte((byte)(theLong >> 16));
			this.WriteByte((byte)(theLong >> 24));
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x000311B4 File Offset: 0x0002F3B4
		public void WriteFloat(float theFloat)
		{
			byte[] bytes = BitConverter.GetBytes(theFloat);
			this.WriteBytes(bytes, 4);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x000311D0 File Offset: 0x0002F3D0
		public void WriteDouble(double theDouble)
		{
			byte[] bytes = BitConverter.GetBytes(theDouble);
			this.WriteBytes(bytes, 8);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x000311EC File Offset: 0x0002F3EC
		public void WriteInt8(int theInt8)
		{
			this.WriteByte((byte)theInt8);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x000311F6 File Offset: 0x0002F3F6
		public void WriteInt16(short theInt16)
		{
			this.WriteByte((byte)theInt16);
			this.WriteByte((byte)(theInt16 >> 8));
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0003120A File Offset: 0x0002F40A
		public void WriteInt32(int theInt32)
		{
			this.WriteByte((byte)theInt32);
			this.WriteByte((byte)(theInt32 >> 8));
			this.WriteByte((byte)(theInt32 >> 16));
			this.WriteByte((byte)(theInt32 >> 24));
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00031234 File Offset: 0x0002F434
		public void WriteInt64(long theInt64)
		{
			this.WriteByte((byte)theInt64);
			this.WriteByte((byte)(theInt64 >> 8));
			this.WriteByte((byte)(theInt64 >> 16));
			this.WriteByte((byte)(theInt64 >> 24));
			this.WriteByte((byte)(theInt64 >> 32));
			this.WriteByte((byte)(theInt64 >> 40));
			this.WriteByte((byte)(theInt64 >> 48));
			this.WriteByte((byte)(theInt64 >> 56));
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x00031298 File Offset: 0x0002F498
		public void WriteTransform2D(SexyTransform2D theTrans)
		{
			this.WriteFloat(theTrans.m00);
			this.WriteFloat(theTrans.m01);
			this.WriteFloat(theTrans.m02);
			this.WriteFloat(theTrans.m10);
			this.WriteFloat(theTrans.m11);
			this.WriteFloat(theTrans.m12);
			this.WriteFloat(theTrans.m20);
			this.WriteFloat(theTrans.m21);
			this.WriteFloat(theTrans.m22);
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0003131A File Offset: 0x0002F51A
		public void WriteFPoint(Vector2 thePoint)
		{
			this.WriteDouble((double)thePoint.X);
			this.WriteDouble((double)thePoint.Y);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x00031338 File Offset: 0x0002F538
		public void WriteString(string theString)
		{
			this.WriteShort((short)theString.Length);
			for (int i = 0; i < theString.Length; i++)
			{
				this.WriteByte((byte)theString[i]);
			}
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x00031371 File Offset: 0x0002F571
		public void WriteUTF8String(string theString)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x00031378 File Offset: 0x0002F578
		public void WriteSexyString(string theString)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0003137F File Offset: 0x0002F57F
		public void WriteLine(string theString)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x00031388 File Offset: 0x0002F588
		public void WriteBuffer(List<byte> theBuffer)
		{
			this.WriteLong((long)theBuffer.Count);
			for (int i = 0; i < theBuffer.Count; i++)
			{
				this.WriteByte(theBuffer[i]);
			}
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x000313C0 File Offset: 0x0002F5C0
		public void WriteBuffer(SexyBuffer theBuffer)
		{
			this.WriteBuffer(theBuffer.mData);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x000313D0 File Offset: 0x0002F5D0
		public void WriteBytes(byte[] theByte, int theCount)
		{
			for (int i = 0; i < theCount; i++)
			{
				this.WriteByte(theByte[i]);
			}
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x000313F2 File Offset: 0x0002F5F2
		public void SetData(List<byte> theBuffer)
		{
			this.mData.Clear();
			this.mData = null;
			this.mData = new List<byte>(theBuffer);
			this.mDataBitSize = this.mData.Count * 8;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x00031428 File Offset: 0x0002F628
		public void SetData(byte[] thePtr, int theCount)
		{
			this.mData.Clear();
			for (int i = 0; i < theCount; i++)
			{
				this.WriteByte(thePtr[i]);
			}
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00031458 File Offset: 0x0002F658
		public byte ReadByte()
		{
			if ((this.mReadBitPos + 7) / 8 >= this.mData.Count)
			{
				return 0;
			}
			if (this.mReadBitPos % 8 == 0)
			{
				byte result = this.mData[this.mReadBitPos / 8];
				this.mReadBitPos += 8;
				return result;
			}
			int num = this.mReadBitPos % 8;
			byte b = (byte)(this.mData[this.mReadBitPos / 8] >> num);
			b |= (byte)(this.mData[this.mReadBitPos / 8 + 1] << 8 - num);
			this.mReadBitPos += 8;
			return b;
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x00031504 File Offset: 0x0002F704
		public int ReadNumBits(int theBits, bool isSigned)
		{
			int count = this.mData.Count;
			int num = 0;
			bool flag = false;
			for (int i = 0; i < theBits; i++)
			{
				int num2 = this.mReadBitPos / 8;
				if (num2 >= count)
				{
					break;
				}
				if (flag = ((int)this.mData[num2] & (1 << this.mReadBitPos % 8)) != 0)
				{
					num |= 1 << i;
				}
				this.mReadBitPos++;
			}
			if (isSigned && flag)
			{
				for (int j = theBits; j < 32; j++)
				{
					num |= 1 << j;
				}
			}
			return num;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0003159A File Offset: 0x0002F79A
		public bool ReadBoolean()
		{
			return this.ReadByte() != 0;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x000315A8 File Offset: 0x0002F7A8
		public short ReadShort()
		{
			byte[] array = new byte[2];
			this.ReadBytes(ref array, 2);
			return BitConverter.ToInt16(array, 0);
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x000315CC File Offset: 0x0002F7CC
		public long ReadLong()
		{
			return (long)this.ReadInt32();
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x000315D8 File Offset: 0x0002F7D8
		public float ReadFloat()
		{
			byte[] array = new byte[4];
			this.ReadBytes(ref array, 4);
			return BitConverter.ToSingle(array, 0);
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x00031600 File Offset: 0x0002F800
		public double ReadDouble()
		{
			byte[] array = new byte[8];
			this.ReadBytes(ref array, 8);
			return BitConverter.ToDouble(array, 0);
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00031628 File Offset: 0x0002F828
		public byte ReadInt8()
		{
			return this.ReadByte();
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0003163D File Offset: 0x0002F83D
		public short ReadInt16()
		{
			return this.ReadShort();
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x00031648 File Offset: 0x0002F848
		public int ReadInt32()
		{
			byte[] array = new byte[4];
			this.ReadBytes(ref array, 4);
			return BitConverter.ToInt32(array, 0);
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0003166C File Offset: 0x0002F86C
		public long ReadInt64()
		{
			byte[] array = new byte[8];
			this.ReadBytes(ref array, 8);
			return BitConverter.ToInt64(array, 0);
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00031690 File Offset: 0x0002F890
		public SexyTransform2D ReadTransform2D()
		{
			return new SexyTransform2D(false)
			{
				m00 = this.ReadFloat(),
				m01 = this.ReadFloat(),
				m02 = this.ReadFloat(),
				m10 = this.ReadFloat(),
				m11 = this.ReadFloat(),
				m12 = this.ReadFloat(),
				m20 = this.ReadFloat(),
				m21 = this.ReadFloat(),
				m22 = this.ReadFloat()
			};
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0003171C File Offset: 0x0002F91C
		public FPoint ReadFPoint()
		{
			return new FPoint
			{
				mX = (float)this.ReadDouble(),
				mY = (float)this.ReadDouble()
			};
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0003174C File Offset: 0x0002F94C
		public Vector2 ReadVector2()
		{
			Vector2 result = default(Vector2);
			result.X = (float)this.ReadDouble();
			result.Y = (float)this.ReadDouble();
			return result;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x00031780 File Offset: 0x0002F980
		public string ReadString()
		{
			string text = "";
			int num = (int)this.ReadShort();
			for (int i = 0; i < num; i++)
			{
				text += (char)this.ReadByte();
			}
			return text;
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x000317BC File Offset: 0x0002F9BC
		public string ReadUTF8String()
		{
			if ((this.mReadBitPos & 7) != 0)
			{
				this.mReadBitPos = (this.mReadBitPos + 8) & -8;
			}
			string text = "";
			int num = (int)this.ReadShort();
			if (num == 0)
			{
				return "";
			}
			int num2 = this.mReadBitPos / 8;
			byte[] theBuffer = this.mData.ToArray();
			int num3 = (this.mDataBitSize - this.mReadBitPos) / 8;
			int num4 = 0;
			while (num3 > 0 && num4 < num)
			{
				string text2 = "";
				int nextUTF8CharFromStream = SexyBuffer.GetNextUTF8CharFromStream(theBuffer, num2, num3, ref text2);
				if (nextUTF8CharFromStream == 0)
				{
					break;
				}
				num2 += nextUTF8CharFromStream;
				num3 -= nextUTF8CharFromStream;
				this.mReadBitPos += 8 * nextUTF8CharFromStream;
				text += text2;
				num4++;
			}
			return text;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x00031877 File Offset: 0x0002FA77
		public string ReadSexyString()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00031880 File Offset: 0x0002FA80
		public string ReadLine()
		{
			string text = "";
			for (;;)
			{
				byte b = this.ReadByte();
				if (b == 0 || b == 10)
				{
					break;
				}
				if (b != 13)
				{
					text += (char)b;
				}
			}
			return text;
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x000318B8 File Offset: 0x0002FAB8
		public void ReadBytes(ref byte[] theData, int theLen)
		{
			for (int i = 0; i < theLen; i++)
			{
				theData[i] = this.ReadByte();
			}
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x000318DC File Offset: 0x0002FADC
		public void ReadBuffer(List<byte> theByteVector)
		{
			theByteVector.Clear();
			long num = this.ReadLong();
			if (num > 0L)
			{
				theByteVector.AddRange(this.mData);
			}
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00031907 File Offset: 0x0002FB07
		public void ReadBuffer(SexyBuffer theBuffer)
		{
			this.ReadBuffer(theBuffer.mData);
			theBuffer.mDataBitSize = theBuffer.mData.Count * 8;
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00031928 File Offset: 0x0002FB28
		public byte[] GetDataPtr()
		{
			if (this.mData.Count == 0)
			{
				return null;
			}
			return this.mData.ToArray();
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00031944 File Offset: 0x0002FB44
		public int GetDataLen()
		{
			return (this.mDataBitSize + 7) / 8;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00031950 File Offset: 0x0002FB50
		public int GetDataLenBits()
		{
			return this.mDataBitSize;
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00031958 File Offset: 0x0002FB58
		public ulong GetCRC32(ulong theSeed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0003195F File Offset: 0x0002FB5F
		public bool AtEnd()
		{
			return this.mReadBitPos >= this.mDataBitSize;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00031972 File Offset: 0x0002FB72
		public bool PastEnd()
		{
			return this.mReadBitPos > this.mDataBitSize;
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00031982 File Offset: 0x0002FB82
		public int GetBitsAvailable()
		{
			if (!this.AtEnd())
			{
				return this.mDataBitSize - this.mReadBitPos;
			}
			return 0;
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0003199B File Offset: 0x0002FB9B
		public int GetBytesAvailable()
		{
			return this.GetBitsAvailable() / 8;
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x000319A5 File Offset: 0x0002FBA5
		private static int GetNextUTF8CharFromStream(byte[] theBuffer, int theLen, ref string theChar)
		{
			return SexyBuffer.GetNextUTF8CharFromStream(theBuffer, 0, theLen, ref theChar);
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x000319B0 File Offset: 0x0002FBB0
		private static int GetNextUTF8CharFromStream(byte[] theBuffer, int start, int theLen, ref string theChar)
		{
			if (theLen == 0)
			{
				return 0;
			}
			int num = 0;
			int num2 = 0;
			int num3 = (int)theBuffer[start + num++];
			if ((num3 & 128) != 0)
			{
				if ((num3 & 192) != 192)
				{
					return 0;
				}
				int[] array = new int[6];
				int num4 = 0;
				array[num4++] = num3;
				int num5 = 0;
				while (num5 < SexyBuffer.aMaskData.Length && (num3 & (int)SexyBuffer.aMaskData[num5]) != (((int)SexyBuffer.aMaskData[num5] << 1) & (int)SexyBuffer.aMaskData[num5]))
				{
					num5++;
				}
				if (num5 >= SexyBuffer.aMaskData.Length)
				{
					return 0;
				}
				num3 &= (int)(~(int)SexyBuffer.aMaskData[num5]);
				int num6 = num5 + 1;
				if (num6 < 2 || num6 > 6)
				{
					return 0;
				}
				while (num5 > 0 && num2 < theLen)
				{
					int num7 = (int)theBuffer[start + num++];
					if ((num7 & 192) != 128)
					{
						return 0;
					}
					array[num4++] = num7;
					num3 = (num3 << 6) | (num7 & 63);
					num5--;
					num2++;
				}
				if (num5 > 0)
				{
					return 0;
				}
				bool flag = true;
				switch (num6)
				{
				case 2:
					flag = (array[0] & 62) != 0;
					break;
				case 3:
					flag = (array[0] & 31) != 0 || (array[1] & 32) != 0;
					break;
				case 4:
					flag = (array[0] & 15) != 0 || (array[1] & 48) != 0;
					break;
				case 5:
					flag = (array[0] & 7) != 0 || (array[1] & 56) != 0;
					break;
				case 6:
					flag = (array[0] & 3) != 0 || (array[1] & 60) != 0;
					break;
				}
				if (!flag)
				{
					return 0;
				}
			}
			int result = num2;
			if ((num3 >= 55296 && num3 <= 57343) || (num3 >= 65534 && num3 <= 65535))
			{
				return 0;
			}
			theChar += num3;
			return result;
		}

		// Token: 0x0400084D RID: 2125
		public int mDataBitSize;

		// Token: 0x0400084E RID: 2126
		public int mReadBitPos;

		// Token: 0x0400084F RID: 2127
		public int mWriteBitPos;

		// Token: 0x04000850 RID: 2128
		public List<byte> mData;

		// Token: 0x04000851 RID: 2129
		private static ushort[] aMaskData = new ushort[] { 192, 224, 240, 248, 252 };
	}
}
