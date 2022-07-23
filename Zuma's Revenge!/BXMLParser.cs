using System;
using SexyFramework;
using SexyFramework.Misc;
using SexyFramework.Resource;

namespace ZumasRevenge
{
	// Token: 0x02000088 RID: 136
	public class BXMLParser
	{
		// Token: 0x060008FF RID: 2303 RVA: 0x00050344 File Offset: 0x0004E544
		protected string UnpackString()
		{
			string text = "";
			for (int num = this.mSexyBuffer.ReadInt32(); num != 0; num = this.mSexyBuffer.ReadInt32())
			{
				text += (char)num;
			}
			return text;
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00050384 File Offset: 0x0004E584
		protected short UnpackShort()
		{
			return this.mSexyBuffer.ReadShort();
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0005039E File Offset: 0x0004E59E
		public BXMLParser()
		{
			this.mSexyBuffer = null;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x000503AD File Offset: 0x0004E5AD
		public virtual void Dispose()
		{
			this.mSexyBuffer = null;
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x000503B8 File Offset: 0x0004E5B8
		public virtual bool OpenFile(string filename)
		{
			PFILE pfile = new PFILE(filename, "rb");
			if (!pfile.Open())
			{
				return false;
			}
			byte[] data = pfile.GetData();
			this.mSexyBuffer = new SexyBuffer();
			this.mSexyBuffer.SetData(data, data.Length);
			return true;
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00050400 File Offset: 0x0004E600
		public virtual bool OpenStream(string filename)
		{
			this.mSexyBuffer = new SexyBuffer();
			return GlobalMembers.gSexyApp.ReadBufferFromStream(filename, ref this.mSexyBuffer);
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00050430 File Offset: 0x0004E630
		public virtual bool OpenBuffer(SexyBuffer buffer)
		{
			this.mSexyBuffer = buffer;
			return true;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0005043C File Offset: 0x0004E63C
		public virtual bool NextElement(ref BXMLElement theElement)
		{
			if (this.mSexyBuffer.AtEnd())
			{
				return false;
			}
			theElement.mType = 0;
			theElement.mValue = "";
			theElement.mAttributes.Clear();
			theElement.mType = (int)this.UnpackShort();
			theElement.mValue = this.UnpackString();
			int num = (int)this.UnpackShort();
			while (num-- > 0)
			{
				string text = this.UnpackString().ToLower();
				string text2 = this.UnpackString();
				theElement.mAttributes[text] = text2;
			}
			return true;
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x000504C5 File Offset: 0x0004E6C5
		public static bool CompileXML(string theSrcName, string theSrcDestName)
		{
			return true;
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x000504C8 File Offset: 0x0004E6C8
		public bool HasFailed()
		{
			return false;
		}

		// Token: 0x040006FE RID: 1790
		private SexyBuffer mSexyBuffer;
	}
}
