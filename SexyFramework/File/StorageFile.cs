using System;
using System.IO.IsolatedStorage;
using System.Text;
using SexyFramework.Misc;

namespace SexyFramework.File
{
	// Token: 0x02000074 RID: 116
	public class StorageFile
	{
		// Token: 0x06000454 RID: 1108 RVA: 0x0000CE20 File Offset: 0x0000B020
		public static void DeleteFile(string theFileName)
		{
			IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
			userStoreForApplication.DeleteFile(theFileName);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000CE3C File Offset: 0x0000B03C
		public static bool FileExists(string theFileName)
		{
			IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
			return userStoreForApplication.FileExists(theFileName);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000CE58 File Offset: 0x0000B058
		public static void MakeDir(string theFolderName)
		{
			IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
			if (!userStoreForApplication.DirectoryExists(theFolderName))
			{
				userStoreForApplication.CreateDirectory(theFolderName);
			}
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0000CE7C File Offset: 0x0000B07C
		public static bool ReadBufferFromFile(string theFileName, SexyBuffer theBuffer)
		{
			IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
			if (!userStoreForApplication.FileExists(theFileName))
			{
				return false;
			}
			IsolatedStorageFileStream isolatedStorageFileStream = userStoreForApplication.OpenFile(theFileName, System.IO.FileMode.Open);
			if (isolatedStorageFileStream == null)
			{
				return false;
			}
			int num = (int)isolatedStorageFileStream.Length;
			byte[] array = new byte[num];
			isolatedStorageFileStream.Read(array, 0, num);
			theBuffer.Clear();
			theBuffer.SetData(array, num);
			isolatedStorageFileStream.Close();
			return true;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000CED8 File Offset: 0x0000B0D8
		public static bool WriteBufferToFile(string theFileName, SexyBuffer theBuffer)
		{
			IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
			if (!userStoreForApplication.DirectoryExists("/users"))
			{
				userStoreForApplication.CreateDirectory("/users");
			}
			IsolatedStorageFileStream isolatedStorageFileStream = userStoreForApplication.OpenFile(theFileName, System.IO.FileMode.Create);
			if (isolatedStorageFileStream == null)
			{
				return false;
			}
			byte[] dataPtr = theBuffer.GetDataPtr();
			isolatedStorageFileStream.Write(dataPtr, 0, dataPtr.Length);
			isolatedStorageFileStream.Close();
			return true;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000CF2A File Offset: 0x0000B12A
		public void clear()
		{
			if (this.m_nMode != FileMode.MODE_NONE)
			{
				this.close();
			}
			this.fp = null;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000CF41 File Offset: 0x0000B141
		public void close()
		{
			if (this.fp != null)
			{
				this.fp.Close();
				this.fp = null;
				this.m_nMode = FileMode.MODE_NONE;
			}
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0000CF64 File Offset: 0x0000B164
		public bool openRead(string fName)
		{
			return this.openRead(fName, false, true);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000CF70 File Offset: 0x0000B170
		public bool openRead(string fName, bool bSilent, bool bFromDocs)
		{
			this.clear();
			if (!this.m_userStore.FileExists(fName))
			{
				return false;
			}
			this.fp = this.m_userStore.OpenFile(fName, System.IO.FileMode.Open);
			if (this.fp == null)
			{
				return false;
			}
			this.m_nMode = FileMode.MODE_READ;
			return true;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0000CFBA File Offset: 0x0000B1BA
		public bool getBool()
		{
			return this.getChar() == '\u0001';
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000CFC8 File Offset: 0x0000B1C8
		public char getChar()
		{
			byte[] array = null;
			this.read(ref array, 1U);
			return (char)array[0];
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0000CFE4 File Offset: 0x0000B1E4
		public short getShort()
		{
			byte[] array = null;
			this.read(ref array, 2U);
			if (BitConverter.IsLittleEndian)
			{
				array = this.ReverseBytes(array);
			}
			return BitConverter.ToInt16(array, 0);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0000D014 File Offset: 0x0000B214
		public int getInt()
		{
			byte[] array = null;
			this.read(ref array, 4U);
			if (BitConverter.IsLittleEndian)
			{
				array = this.ReverseBytes(array);
			}
			return BitConverter.ToInt32(array, 0);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0000D044 File Offset: 0x0000B244
		public ulong getLong()
		{
			byte[] array = null;
			this.read(ref array, 8U);
			if (BitConverter.IsLittleEndian)
			{
				array = this.ReverseBytes(array);
			}
			return BitConverter.ToUInt64(array, 0);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000D074 File Offset: 0x0000B274
		public float getFloat()
		{
			byte[] array = null;
			this.read(ref array, 4U);
			if (BitConverter.IsLittleEndian)
			{
				array = this.ReverseBytes(array);
			}
			return BitConverter.ToSingle(array, 0);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000D0A5 File Offset: 0x0000B2A5
		public bool getEof()
		{
			return this.fp.Position == this.fp.Length;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000D0C0 File Offset: 0x0000B2C0
		public int getStr(ref string s, int nBufferSize)
		{
			int num = 0;
			byte[] array = new byte[nBufferSize + 1];
			byte b;
			while ((b = (byte)this.getChar()) != 0 && !this.getEof())
			{
				if (num < nBufferSize - 1)
				{
					array[num] = b;
					num++;
				}
				else
				{
					int num2 = nBufferSize - 1;
				}
			}
			array[num] = 0;
			s = Encoding.UTF8.GetString(array, 0, num);
			return num;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000D119 File Offset: 0x0000B319
		public bool openWrite(string name)
		{
			return this.openWrite(name, true);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0000D123 File Offset: 0x0000B323
		public bool openWrite(string fName, bool bFromDocs)
		{
			this.clear();
			this.fp = this.m_userStore.OpenFile(fName, System.IO.FileMode.Create);
			if (this.fp == null)
			{
				return false;
			}
			this.m_nMode = FileMode.MODE_WRITE;
			return true;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0000D150 File Offset: 0x0000B350
		public void setBool(bool b)
		{
			if (b)
			{
				this.setChar(1);
				return;
			}
			this.setChar(0);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000D164 File Offset: 0x0000B364
		public void setChar(byte c)
		{
			this.write(new byte[] { c }, 1U);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0000D184 File Offset: 0x0000B384
		public void setShort(short s)
		{
			byte[] array = BitConverter.GetBytes(s);
			if (BitConverter.IsLittleEndian)
			{
				array = this.ReverseBytes(array);
			}
			this.write(array, 2U);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0000D1B0 File Offset: 0x0000B3B0
		public void setInt(int i)
		{
			byte[] array = BitConverter.GetBytes(i);
			if (BitConverter.IsLittleEndian)
			{
				array = this.ReverseBytes(array);
			}
			this.write(array, 4U);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0000D1DC File Offset: 0x0000B3DC
		public void setLong(ulong l)
		{
			byte[] array = BitConverter.GetBytes(l);
			if (BitConverter.IsLittleEndian)
			{
				array = this.ReverseBytes(array);
			}
			this.write(array, 8U);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0000D208 File Offset: 0x0000B408
		public void setFloat(float f)
		{
			byte[] array = BitConverter.GetBytes(f);
			if (BitConverter.IsLittleEndian)
			{
				array = this.ReverseBytes(array);
			}
			this.write(array, 4U);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0000D234 File Offset: 0x0000B434
		public void setStr(string s)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			bool isLittleEndian = BitConverter.IsLittleEndian;
			int byteCount = Encoding.UTF8.GetByteCount(s);
			byte[] array = new byte[byteCount + 1];
			Array.Copy(bytes, array, byteCount);
			this.write(array, (uint)(byteCount + 1));
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000D27A File Offset: 0x0000B47A
		public void useBool(ref bool b)
		{
			if (this.m_nMode == FileMode.MODE_READ)
			{
				b = this.getBool();
				return;
			}
			this.setBool(b);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000D296 File Offset: 0x0000B496
		public void useShort(ref short s)
		{
			if (this.m_nMode == FileMode.MODE_READ)
			{
				s = this.getShort();
				return;
			}
			this.setShort(s);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0000D2B2 File Offset: 0x0000B4B2
		public void useInt(ref int i)
		{
			if (this.m_nMode == FileMode.MODE_READ)
			{
				i = this.getInt();
				return;
			}
			this.setInt(i);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000D2CE File Offset: 0x0000B4CE
		public void useLong(ref ulong l)
		{
			if (this.m_nMode == FileMode.MODE_READ)
			{
				l = this.getLong();
				return;
			}
			this.setLong(l);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000D2EA File Offset: 0x0000B4EA
		public void useFloat(ref float f)
		{
			if (this.m_nMode == FileMode.MODE_READ)
			{
				f = this.getFloat();
				return;
			}
			this.setFloat(f);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000D306 File Offset: 0x0000B506
		public void useStr(ref byte[] str, int nBufferSize)
		{
			throw new InvalidOperationException("TOTO_WP7 useStr() not implement!");
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0000D312 File Offset: 0x0000B512
		public void useBuffer(byte[] p, int nSize)
		{
			if (this.m_nMode == FileMode.MODE_READ)
			{
				this.read(ref p, (uint)nSize);
				return;
			}
			this.write(p, (uint)nSize);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000D32F File Offset: 0x0000B52F
		public int getFileSize()
		{
			return (int)(this.fp.Seek(0L, (System.IO.SeekOrigin)(int)this.fp.Length) - 12L);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000D350 File Offset: 0x0000B550
		private void read(ref byte[] pData, uint nDataSize)
		{
			byte[] array = new byte[nDataSize];
			this.fp.Read(array, 0, (int)nDataSize);
			pData = array;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000D378 File Offset: 0x0000B578
		private void write(byte[] pData, uint nDataSize)
		{
			byte[] array = new byte[64];
			for (uint num = 0U; num < nDataSize; num += 64U)
			{
				int num2 = (int)(nDataSize - num);
				num2 = ((num2 > 64) ? 64 : num2);
				Array.Copy(pData, array, num2);
				this.fp.Write(array, 0, num2);
			}
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000D3C4 File Offset: 0x0000B5C4
		private byte[] ReverseBytes(byte[] inArray)
		{
			int num = inArray.Length - 1;
			for (int i = 0; i < inArray.Length / 2; i++)
			{
				byte b = inArray[i];
				inArray[i] = inArray[num];
				inArray[num] = b;
				num--;
			}
			return inArray;
		}

		// Token: 0x04000238 RID: 568
		private FileMode m_nMode;

		// Token: 0x04000239 RID: 569
		private IsolatedStorageFile m_userStore = IsolatedStorageFile.GetUserStoreForApplication();

		// Token: 0x0400023A RID: 570
		private IsolatedStorageFileStream fp;
	}
}
