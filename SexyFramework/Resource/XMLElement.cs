using System;
using System.Collections.Generic;
using System.Text;

namespace SexyFramework.Resource
{
	// Token: 0x0200019F RID: 415
	public class XMLElement
	{
		// Token: 0x06000E94 RID: 3732 RVA: 0x000499B8 File Offset: 0x00047BB8
		public bool GetAttributeBool(string theKey, bool theDefaultValue)
		{
			if (!this.HasAttribute(theKey))
			{
				return theDefaultValue;
			}
			string text = this.mAttributes[theKey];
			return text.Length == 0 || (text == "true" || text == "1") || (!(text == "false") && !(text == "0") && theDefaultValue);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00049A20 File Offset: 0x00047C20
		public string GetAttribute(string key)
		{
			if (!this.HasAttribute(key))
			{
				return "";
			}
			return this.mAttributes[key];
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x00049A3D File Offset: 0x00047C3D
		public bool HasAttribute(string key)
		{
			return this.mAttributes.ContainsKey(key);
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00049A4B File Offset: 0x00047C4B
		public Dictionary<string, string> GetAttributeMap()
		{
			return this.mAttributes;
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00049A53 File Offset: 0x00047C53
		public void ClearAttributes()
		{
			this.mAttributes.Clear();
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00049A60 File Offset: 0x00047C60
		public void AddAttribute(string key, string value)
		{
			this.mAttributes[key] = value;
		}

		// Token: 0x04000BB4 RID: 2996
		public XMLElement.XMLElementType mType;

		// Token: 0x04000BB5 RID: 2997
		public StringBuilder mSection;

		// Token: 0x04000BB6 RID: 2998
		public StringBuilder mValue;

		// Token: 0x04000BB7 RID: 2999
		public StringBuilder mValueEncoded;

		// Token: 0x04000BB8 RID: 3000
		public StringBuilder mInstruction;

		// Token: 0x04000BB9 RID: 3001
		private Dictionary<string, string> mAttributes = new Dictionary<string, string>();

		// Token: 0x04000BBA RID: 3002
		public Dictionary<string, string> mAttributesEncoded = new Dictionary<string, string>();

		// Token: 0x020001A0 RID: 416
		public enum XMLElementType
		{
			// Token: 0x04000BBC RID: 3004
			TYPE_NONE,
			// Token: 0x04000BBD RID: 3005
			TYPE_START,
			// Token: 0x04000BBE RID: 3006
			TYPE_END,
			// Token: 0x04000BBF RID: 3007
			TYPE_ELEMENT,
			// Token: 0x04000BC0 RID: 3008
			TYPE_INSTRUCTION,
			// Token: 0x04000BC1 RID: 3009
			TYPE_COMMENT
		}
	}
}
