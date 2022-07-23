using System;
using System.Collections.Generic;

namespace SexyFramework.Resource
{
	// Token: 0x02000189 RID: 393
	public class PropertiesParser
	{
		// Token: 0x06000DC6 RID: 3526 RVA: 0x000452D0 File Offset: 0x000434D0
		protected void Fail(string theErrorText)
		{
			if (!this.mHasFailed)
			{
				this.mHasFailed = true;
				int currentLineNum = this.mXMLParser.GetCurrentLineNum();
				this.mError = theErrorText;
				if (currentLineNum > 0)
				{
					this.mError = this.mError + " on Line " + currentLineNum;
				}
				if (this.mXMLParser.GetFileName().Length <= 0)
				{
					this.mError = this.mError + " in File " + this.mXMLParser.GetFileName();
				}
			}
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x00045354 File Offset: 0x00043554
		protected bool ParseSingleElement(string aString)
		{
			while (this.mXMLParser.NextElement(this.mXMLElement))
			{
				if (this.mXMLElement.mType == XMLElement.XMLElementType.TYPE_START)
				{
					this.Fail("Unexpected Section: '" + this.mXMLElement.mValue + "'");
					return false;
				}
				if (this.mXMLElement.mType == XMLElement.XMLElementType.TYPE_ELEMENT)
				{
					aString = this.mXMLElement.mValue.ToString();
				}
				else if (this.mXMLElement.mType == XMLElement.XMLElementType.TYPE_END)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x000453E0 File Offset: 0x000435E0
		protected bool ParseStringArray(List<string> theStringVector)
		{
			theStringVector.Clear();
			while (this.mXMLParser.NextElement(this.mXMLElement))
			{
				if (this.mXMLElement.mType == XMLElement.XMLElementType.TYPE_START)
				{
					if (!(this.mXMLElement.mValue.ToString() == "String"))
					{
						this.Fail("Invalid Section '" + this.mXMLElement.mValue + "'");
						return false;
					}
					string text = "";
					if (!this.ParseSingleElement(text))
					{
						return false;
					}
					theStringVector.Add(text);
				}
				else
				{
					if (this.mXMLElement.mType == XMLElement.XMLElementType.TYPE_ELEMENT)
					{
						this.Fail("Element Not Expected '" + this.mXMLElement.mValue + "'");
						return false;
					}
					if (this.mXMLElement.mType == XMLElement.XMLElementType.TYPE_END)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x000454B0 File Offset: 0x000436B0
		protected bool ParseProperties()
		{
			while (this.mXMLParser.NextElement(this.mXMLElement))
			{
				if (this.mXMLElement.mType == XMLElement.XMLElementType.TYPE_START)
				{
					if (this.mXMLElement.mValue.ToString() == "String")
					{
						string text = "";
						if (!this.ParseSingleElement(text))
						{
							return false;
						}
						string attribute = this.mXMLElement.GetAttribute("id");
						this.mApp.SetString(attribute, text);
					}
					else if (this.mXMLElement.mValue.ToString() == "StringArray")
					{
						List<string> list = new List<string>();
						if (!this.ParseStringArray(list))
						{
							return false;
						}
						string attribute2 = this.mXMLElement.GetAttribute("id");
						this.mApp.mStringVectorProperties[attribute2] = list;
					}
					else if (this.mXMLElement.mValue.ToString() == "Boolean")
					{
						string text2 = "";
						if (!this.ParseSingleElement(text2))
						{
							return false;
						}
						text2 = text2.ToUpper();
						bool boolValue;
						if (text2 == "1" || text2 == "YES" || text2 == "ON" || text2 == "TRUE")
						{
							boolValue = true;
						}
						else
						{
							if (!(text2 == "0") && !(text2 == "NO") && !(text2 == "OFF") && !(text2 == "FALSE"))
							{
								this.Fail("Invalid Boolean Value: '" + text2 + "'");
								return false;
							}
							boolValue = false;
						}
						string attribute3 = this.mXMLElement.GetAttribute("id");
						this.mApp.SetBoolean(attribute3, boolValue);
					}
					else if (this.mXMLElement.mValue.ToString() == "Integer")
					{
						string text3 = "";
						if (!this.ParseSingleElement(text3))
						{
							return false;
						}
						int anInt = 0;
						if (!Common.StringToInt(text3, ref anInt))
						{
							this.Fail("Invalid Integer Value: '" + text3 + "'");
							return false;
						}
						string attribute4 = this.mXMLElement.GetAttribute("id");
						this.mApp.SetInteger(attribute4, anInt);
					}
					else
					{
						if (!(this.mXMLElement.mValue.ToString() == "Double"))
						{
							this.Fail("Invalid Section '" + this.mXMLElement.mValue + "'");
							return false;
						}
						string text4 = "";
						if (!this.ParseSingleElement(text4))
						{
							return false;
						}
						double aDouble = 0.0;
						if (!Common.StringToDouble(text4, ref aDouble))
						{
							this.Fail("Invalid Double Value: '" + text4 + "'");
							return false;
						}
						string attribute5 = this.mXMLElement.GetAttribute("id");
						this.mApp.SetDouble(attribute5, aDouble);
					}
				}
				else
				{
					if (this.mXMLElement.mType == XMLElement.XMLElementType.TYPE_ELEMENT)
					{
						this.Fail("Element Not Expected '" + this.mXMLElement.mValue + "'");
						return false;
					}
					if (this.mXMLElement.mType == XMLElement.XMLElementType.TYPE_END)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x000457E0 File Offset: 0x000439E0
		protected bool DoParseProperties()
		{
			if (!this.mXMLParser.HasFailed())
			{
				XMLElement xmlelement;
				for (;;)
				{
					xmlelement = new XMLElement();
					if (!this.mXMLParser.NextElement(xmlelement))
					{
						break;
					}
					if (xmlelement.mType == XMLElement.XMLElementType.TYPE_START)
					{
						if (!(xmlelement.mValue.ToString() == "Properties"))
						{
							goto IL_4B;
						}
						if (!this.ParseProperties())
						{
							break;
						}
					}
					else if (xmlelement.mType == XMLElement.XMLElementType.TYPE_ELEMENT)
					{
						goto Block_5;
					}
				}
				goto IL_8C;
				IL_4B:
				this.Fail("Invalid Section '" + xmlelement.mValue + "'");
				goto IL_8C;
				Block_5:
				this.Fail("Element Not Expected '" + xmlelement.mValue + "'");
			}
			IL_8C:
			if (this.mXMLParser.HasFailed())
			{
				this.Fail(this.mXMLParser.GetErrorText());
			}
			this.mXMLParser = null;
			return !this.mHasFailed;
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x000458A7 File Offset: 0x00043AA7
		public PropertiesParser(SexyAppBase theApp)
		{
			this.mApp = theApp;
			this.mHasFailed = false;
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x000458D3 File Offset: 0x00043AD3
		public virtual void Dispose()
		{
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x000458D5 File Offset: 0x00043AD5
		public bool ParsePropertiesFile(string theFilename)
		{
			this.mXMLParser = new XMLParser();
			this.mXMLParser.OpenFile(theFilename);
			return this.DoParseProperties();
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x000458F5 File Offset: 0x00043AF5
		public bool ParsePropertiesBuffer(byte[] theBuffer)
		{
			this.mXMLParser = new XMLParser();
			this.mXMLParser.SetBytes(theBuffer);
			return this.DoParseProperties();
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x00045914 File Offset: 0x00043B14
		public string GetErrorText()
		{
			return this.mError;
		}

		// Token: 0x04000B26 RID: 2854
		public SexyAppBase mApp;

		// Token: 0x04000B27 RID: 2855
		public string mError = "";

		// Token: 0x04000B28 RID: 2856
		public bool mHasFailed;

		// Token: 0x04000B29 RID: 2857
		private XMLParser mXMLParser;

		// Token: 0x04000B2A RID: 2858
		private XMLElement mXMLElement = new XMLElement();
	}
}
