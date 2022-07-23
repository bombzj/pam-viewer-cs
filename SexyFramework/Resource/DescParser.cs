using System;
using System.Collections.Generic;
using System.Text;

namespace SexyFramework.Resource
{
	// Token: 0x020000CF RID: 207
	public abstract class DescParser : EncodingParser
	{
		// Token: 0x06000636 RID: 1590 RVA: 0x000163EB File Offset: 0x000145EB
		public virtual bool Error(string theError)
		{
			this.mError = this.mError + "\n" + theError;
			return false;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00016408 File Offset: 0x00014608
		public virtual DataElement Dereference(string theString)
		{
			string text = theString.ToUpper();
			if (this.mDefineMap.ContainsKey(text))
			{
				return this.mDefineMap[text];
			}
			return null;
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00016438 File Offset: 0x00014638
		public bool IsImmediate(string theString)
		{
			return (theString[0] >= '0' && theString[0] <= '9') || theString[0] == '-' || theString[0] == '+' || theString[0] == '\'' || theString[0] == '"';
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0001648C File Offset: 0x0001468C
		public string Unquote(string theQuotedString)
		{
			if (theQuotedString[0] == '\'' || theQuotedString[0] == '"')
			{
				char c = theQuotedString[0];
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = true;
				bool flag2 = false;
				for (int i = 1; i < theQuotedString.Length - 1; i++)
				{
					if (flag2)
					{
						char c2 = theQuotedString[i];
						char c3 = c2;
						if (c3 != 'n')
						{
							if (c3 == 't')
							{
								c2 = '\t';
							}
						}
						else
						{
							c2 = '\n';
						}
						stringBuilder.Append(c2);
						flag2 = false;
					}
					else if (theQuotedString[i] == c)
					{
						if (flag)
						{
							stringBuilder.Append(c);
						}
						flag = true;
					}
					else if (theQuotedString[i] == '\\')
					{
						flag2 = true;
						flag = false;
					}
					else
					{
						stringBuilder.Append(theQuotedString[i]);
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}
			return theQuotedString;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001655C File Offset: 0x0001475C
		public bool GetValues(ListDataElement theSource, ListDataElement theValues)
		{
			theValues.mElementVector.Clear();
			for (int i = 0; i < theSource.mElementVector.Count; i++)
			{
				if (theSource.mElementVector[i].mIsList)
				{
					ListDataElement listDataElement = new ListDataElement();
					theValues.mElementVector.Add(listDataElement);
					if (!this.GetValues((ListDataElement)theSource.mElementVector[i], listDataElement))
					{
						return false;
					}
				}
				else
				{
					string text = ((SingleDataElement)theSource.mElementVector[i]).mString.ToString();
					if (text.Length > 0)
					{
						if (text[0] == '\'' || text[0] == '"')
						{
							SingleDataElement singleDataElement = new SingleDataElement(this.Unquote(text));
							theValues.mElementVector.Add(singleDataElement);
						}
						else if (this.IsImmediate(text))
						{
							theValues.mElementVector.Add(new SingleDataElement(text));
						}
						else
						{
							string text2 = text.ToUpper();
							if (!this.mDefineMap.ContainsKey(text2))
							{
								this.Error("Unable to Dereference \"" + text + "\"");
								return false;
							}
							theValues.mElementVector.Add(this.mDefineMap[text2].Duplicate());
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0001669C File Offset: 0x0001489C
		public string DataElementToString(DataElement theDataElement, bool enclose)
		{
			if (theDataElement.mIsList)
			{
				ListDataElement listDataElement = (ListDataElement)theDataElement;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(enclose ? "(" : "");
				for (int i = 0; i < listDataElement.mElementVector.Count; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append(enclose ? ", " : " ");
					}
					stringBuilder.Append(this.DataElementToString(listDataElement.mElementVector[i], true));
				}
				stringBuilder.Append(enclose ? ")" : "");
				return stringBuilder.ToString();
			}
			SingleDataElement singleDataElement = (SingleDataElement)theDataElement;
			if (singleDataElement.mValue != null)
			{
				return singleDataElement.mString + "=" + this.DataElementToString(singleDataElement.mValue, true);
			}
			return singleDataElement.mString.ToString();
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00016778 File Offset: 0x00014978
		public bool DataToString(DataElement theSource, ref string theString)
		{
			theString = "";
			if (theSource.mIsList)
			{
				return false;
			}
			if (((SingleDataElement)theSource).mValue != null)
			{
				return false;
			}
			string text = ((SingleDataElement)theSource).mString.ToString();
			DataElement dataElement = this.Dereference(text);
			if (dataElement != null)
			{
				if (dataElement.mIsList)
				{
					return false;
				}
				theString = this.Unquote(((SingleDataElement)dataElement).mString.ToString());
			}
			else
			{
				theString = this.Unquote(text);
			}
			return true;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x000167F0 File Offset: 0x000149F0
		public bool DataToKeyAndValue(DataElement theSource, ref string theKey, ref DataElement theValue)
		{
			theKey = "";
			if (theSource.mIsList)
			{
				return false;
			}
			if (((SingleDataElement)theSource).mValue == null)
			{
				return false;
			}
			theValue = ((SingleDataElement)theSource).mValue;
			string text = ((SingleDataElement)theSource).mString.ToString();
			DataElement dataElement = this.Dereference(text);
			if (dataElement != null)
			{
				if (dataElement.mIsList)
				{
					return false;
				}
				theKey = this.Unquote(((SingleDataElement)dataElement).mString.ToString());
			}
			else
			{
				theKey = this.Unquote(text);
			}
			return true;
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00016874 File Offset: 0x00014A74
		public bool DataToInt(DataElement theSource, ref int theInt)
		{
			theInt = 0;
			string theString = "";
			return this.DataToString(theSource, ref theString) && Common.StringToInt(theString, ref theInt);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x000168A4 File Offset: 0x00014AA4
		public bool DataToDouble(DataElement theSource, ref double theDouble)
		{
			theDouble = 0.0;
			string aTempString = "";
			return this.DataToString(theSource, ref aTempString) && Common.StringToDouble(aTempString, ref theDouble);
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x000168DC File Offset: 0x00014ADC
		public bool DataToBoolean(DataElement theSource, ref bool theBool)
		{
			theBool = false;
			string text = "";
			if (!this.DataToString(theSource, ref text))
			{
				return false;
			}
			if (text == "false" || text == "no" || text == "0")
			{
				theBool = false;
				return true;
			}
			if (text == "true" || text == "yes" || text == "1")
			{
				theBool = true;
				return true;
			}
			return false;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00016958 File Offset: 0x00014B58
		public bool DataToStringVector(DataElement theSource, ref List<string> theStringVector)
		{
			theStringVector.Clear();
			ListDataElement listDataElement = new ListDataElement();
			ListDataElement listDataElement2;
			if (theSource.mIsList)
			{
				if (!this.GetValues((ListDataElement)theSource, listDataElement))
				{
					return false;
				}
				listDataElement2 = listDataElement;
			}
			else
			{
				string text = ((SingleDataElement)theSource).mString.ToString();
				DataElement dataElement = this.Dereference(text);
				if (dataElement == null)
				{
					this.Error("Unable to Dereference \"" + text + "\"");
					return false;
				}
				if (!dataElement.mIsList)
				{
					return false;
				}
				listDataElement2 = (ListDataElement)dataElement;
			}
			for (int i = 0; i < listDataElement2.mElementVector.Count; i++)
			{
				if (listDataElement2.mElementVector[i].mIsList)
				{
					theStringVector.Clear();
					return false;
				}
				SingleDataElement singleDataElement = (SingleDataElement)listDataElement2.mElementVector[i];
				theStringVector.Add(singleDataElement.mString.ToString());
			}
			return true;
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00016A38 File Offset: 0x00014C38
		public bool DataToList(DataElement theSource, ref ListDataElement theValues)
		{
			if (theSource.mIsList)
			{
				return this.GetValues((ListDataElement)theSource, theValues);
			}
			DataElement dataElement = this.Dereference(((SingleDataElement)theSource).mString.ToString());
			if (dataElement == null || !dataElement.mIsList)
			{
				return false;
			}
			ListDataElement listDataElement = (ListDataElement)dataElement;
			theValues = listDataElement;
			return true;
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x00016A8C File Offset: 0x00014C8C
		public bool DataToIntVector(DataElement theSource, ref List<int> theIntVector)
		{
			theIntVector.Clear();
			List<string> list = new List<string>();
			if (!this.DataToStringVector(theSource, ref list))
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				int num = 0;
				if (!Common.StringToInt(list[i], ref num))
				{
					return false;
				}
				theIntVector.Add(num);
			}
			return true;
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00016AE4 File Offset: 0x00014CE4
		public bool DataToDoubleVector(DataElement theSource, ref List<double> theDoubleVector)
		{
			theDoubleVector.Clear();
			List<string> list = new List<string>();
			if (!this.DataToStringVector(theSource, ref list))
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				double num = 0.0;
				if (!Common.StringToDouble(list[i], ref num))
				{
					return false;
				}
				theDoubleVector.Add(num);
			}
			return true;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00016B44 File Offset: 0x00014D44
		public bool ParseToList(string theString, ref ListDataElement theList, bool expectListEnd, ref int theStringPos)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			SingleDataElement singleDataElement = null;
			SingleDataElement singleDataElement2 = null;
			int num = 0;
			if (theStringPos == 0)
			{
				theStringPos = num;
			}
			while (theStringPos < theString.Length)
			{
				bool flag5 = false;
				char c = theString[theStringPos++];
				bool flag6 = c == ' ' || c == '\t' || c == '\n' || c == ',';
				if (flag3)
				{
					flag5 = true;
				}
				else
				{
					if (c == '\'' && !flag2)
					{
						flag = !flag;
					}
					else if (c == '"' && !flag)
					{
						flag2 = !flag2;
					}
					if (c == '\\')
					{
						flag3 = true;
					}
					else if (!flag && !flag2)
					{
						if (c == ')')
						{
							if (expectListEnd)
							{
								return true;
							}
							this.Error("Unexpected List End");
							return false;
						}
						else if (c == '(')
						{
							if (flag4)
							{
								singleDataElement2 = null;
								flag4 = false;
							}
							if (singleDataElement2 != null)
							{
								this.Error("Unexpected List Start");
								return false;
							}
							ListDataElement listDataElement = new ListDataElement();
							if (!this.ParseToList(theString, ref listDataElement, true, ref theStringPos))
							{
								return false;
							}
							if (singleDataElement != null)
							{
								singleDataElement.mValue = listDataElement;
								singleDataElement = null;
							}
							else
							{
								theList.mElementVector.Add(listDataElement);
							}
						}
						else if (c == '=')
						{
							singleDataElement = singleDataElement2;
							flag4 = true;
						}
						else if (flag6)
						{
							if (singleDataElement2 != null && singleDataElement2.mString.Length > 0)
							{
								flag4 = true;
							}
						}
						else
						{
							if (flag4)
							{
								singleDataElement2 = null;
								flag4 = false;
							}
							flag5 = true;
						}
					}
					else
					{
						if (flag4)
						{
							singleDataElement2 = null;
							flag4 = false;
						}
						flag5 = true;
					}
				}
				if (flag5)
				{
					if (singleDataElement2 == null)
					{
						singleDataElement2 = new SingleDataElement();
						if (singleDataElement != null)
						{
							singleDataElement.mValue = singleDataElement2;
							singleDataElement = null;
						}
						else
						{
							theList.mElementVector.Add(singleDataElement2);
						}
					}
					if (flag3)
					{
						singleDataElement2.mString.Append("\\");
						flag3 = false;
					}
					singleDataElement2.mString.Append(c);
				}
			}
			if (flag)
			{
				this.Error("Unterminated Single Quotes");
				return false;
			}
			if (flag2)
			{
				this.Error("Unterminated Double Quotes");
				return false;
			}
			if (expectListEnd)
			{
				this.Error("Unterminated List");
				return false;
			}
			return true;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00016D34 File Offset: 0x00014F34
		public bool ParseDescriptorLine(string theDescriptorLine)
		{
			ListDataElement listDataElement = new ListDataElement();
			int num = 0;
			if (!this.ParseToList(theDescriptorLine, ref listDataElement, false, ref num))
			{
				return false;
			}
			if (listDataElement.mElementVector.Count > 0)
			{
				if (listDataElement.mElementVector[0].mIsList)
				{
					this.Error("Missing Command");
					return false;
				}
				if (!this.HandleCommand(listDataElement))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000647 RID: 1607
		public abstract bool HandleCommand(ListDataElement theParams);

		// Token: 0x06000648 RID: 1608 RVA: 0x00016D94 File Offset: 0x00014F94
		public DescParser()
		{
			this.mCmdSep = 1;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00016DC4 File Offset: 0x00014FC4
		public override void Dispose()
		{
			base.Dispose();
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00016DCC File Offset: 0x00014FCC
		public virtual bool LoadDescriptor(string theFileName)
		{
			this.mCurrentLineNum = 0;
			int num = 0;
			bool flag = false;
			this.mError = "";
			this.mCurrentLine.Clear();
			if (!base.OpenFile(theFileName))
			{
				return this.Error("Unable to open file: " + theFileName);
			}
			while (!this.EndOfFile())
			{
				char c = '0';
				bool flag2 = false;
				bool flag3 = true;
				bool flag4 = false;
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				for (;;)
				{
					EncodingParser.GetCharReturnType @char = this.GetChar(ref c);
					if (@char == EncodingParser.GetCharReturnType.END_OF_FILE)
					{
						break;
					}
					if (@char == EncodingParser.GetCharReturnType.INVALID_CHARACTER)
					{
						goto Block_3;
					}
					if (@char != EncodingParser.GetCharReturnType.SUCCESSFUL)
					{
						goto Block_4;
					}
					if (c != '\r')
					{
						if (c == '\n')
						{
							num++;
						}
						if ((c == ' ' || c == '\t') && flag3)
						{
							flag7 = true;
						}
						if (!flag3 || (c != ' ' && c != '\t' && c != '\n'))
						{
							if (flag3)
							{
								if ((this.mCmdSep & 2) != 0 && !flag7 && this.mCurrentLine.Length > 0)
								{
									goto Block_15;
								}
								if (c == '#')
								{
									flag2 = true;
								}
								flag3 = false;
							}
							if (c == '\n')
							{
								flag7 = false;
								flag3 = true;
							}
							if (c == '\n' && flag2)
							{
								flag2 = false;
							}
							else if (!flag2)
							{
								if (c == '\\' && (flag4 || flag5) && !flag6)
								{
									flag6 = true;
								}
								else
								{
									if (c == '\'' && !flag5 && !flag6)
									{
										flag4 = !flag4;
									}
									if (c == '"' && !flag4 && !flag6)
									{
										flag5 = !flag5;
									}
									if (c == ';' && (this.mCmdSep & 1) != 0 && !flag4 && !flag5)
									{
										break;
									}
									if (flag6)
									{
										this.mCurrentLine.Append('\\');
										flag6 = false;
									}
									if (this.mCurrentLine.Length == 0)
									{
										this.mCurrentLineNum = num + 1;
									}
									this.mCurrentLine.Append(c);
								}
							}
						}
					}
				}
				IL_1A5:
				if (this.mCurrentLine.Length <= 0)
				{
					continue;
				}
				if (!this.ParseDescriptorLine(this.mCurrentLine.ToString()))
				{
					flag = true;
					break;
				}
				this.mCurrentLine.Clear();
				continue;
				Block_3:
				return this.Error("Invalid Character");
				Block_15:
				this.PutChar(c);
				goto IL_1A5;
				Block_4:
				return this.Error("Internal Error");
			}
			this.mCurrentLine.Clear();
			this.mCurrentLineNum = 0;
			this.CloseFile();
			return !flag;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00016FD8 File Offset: 0x000151D8
		public virtual bool LoadDescriptor(byte[] buffer)
		{
			this.mCurrentLineNum = 0;
			int num = 0;
			bool flag = false;
			this.mError = "";
			this.mCurrentLine.Clear();
			if (buffer == null)
			{
				return this.Error("Unable to open file: ");
			}
			this.SetBytes(buffer);
			while (!this.EndOfFile())
			{
				char c = '0';
				bool flag2 = false;
				bool flag3 = true;
				bool flag4 = false;
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				for (;;)
				{
					EncodingParser.GetCharReturnType @char = this.GetChar(ref c);
					if (@char == EncodingParser.GetCharReturnType.END_OF_FILE)
					{
						break;
					}
					if (@char == EncodingParser.GetCharReturnType.INVALID_CHARACTER)
					{
						goto Block_3;
					}
					if (@char != EncodingParser.GetCharReturnType.SUCCESSFUL)
					{
						goto Block_4;
					}
					if (c != '\r')
					{
						if (c == '\n')
						{
							num++;
						}
						if ((c == ' ' || c == '\t') && flag3)
						{
							flag7 = true;
						}
						if (!flag3 || (c != ' ' && c != '\t' && c != '\n'))
						{
							if (flag3)
							{
								if ((this.mCmdSep & 2) != 0 && !flag7 && this.mCurrentLine.Length > 0)
								{
									goto Block_15;
								}
								if (c == '#')
								{
									flag2 = true;
								}
								flag3 = false;
							}
							if (c == '\n')
							{
								flag7 = false;
								flag3 = true;
							}
							if (c == '\n' && flag2)
							{
								flag2 = false;
							}
							else if (!flag2)
							{
								if (c == '\\' && (flag4 || flag5) && !flag6)
								{
									flag6 = true;
								}
								else
								{
									if (c == '\'' && !flag5 && !flag6)
									{
										flag4 = !flag4;
									}
									if (c == '"' && !flag4 && !flag6)
									{
										flag5 = !flag5;
									}
									if (c == ';' && (this.mCmdSep & 1) != 0 && !flag4 && !flag5)
									{
										break;
									}
									if (flag6)
									{
										this.mCurrentLine.Append('\\');
										flag6 = false;
									}
									if (this.mCurrentLine.Length == 0)
									{
										this.mCurrentLineNum = num + 1;
									}
									this.mCurrentLine.Append(c);
								}
							}
						}
					}
				}
				IL_1A2:
				if (this.mCurrentLine.Length > 0)
				{
					if (!this.ParseDescriptorLine(this.mCurrentLine.ToString()))
					{
						flag = true;
					}
					this.mCurrentLine.Clear();
					continue;
				}
				continue;
				Block_3:
				return this.Error("Invalid Character");
				Block_15:
				this.PutChar(c);
				goto IL_1A2;
				Block_4:
				return this.Error("Internal Error");
			}
			this.mCurrentLine.Clear();
			this.mCurrentLineNum = 0;
			return !flag;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x000171D8 File Offset: 0x000153D8
		public virtual bool LoadDescriptorBuffered(string theFileName)
		{
			this.mCurrentLineNum = 0;
			int num = 0;
			bool flag = false;
			this.mCurrentLine.Clear();
			List<string> list = new List<string>();
			List<int> list2 = new List<int>();
			if (!base.OpenFile(theFileName))
			{
				return false;
			}
			while (!this.EndOfFile())
			{
				char c = '0';
				bool flag2 = false;
				bool flag3 = true;
				bool flag4 = false;
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				for (;;)
				{
					EncodingParser.GetCharReturnType @char = this.GetChar(ref c);
					if (@char == EncodingParser.GetCharReturnType.END_OF_FILE)
					{
						break;
					}
					if (@char == EncodingParser.GetCharReturnType.INVALID_CHARACTER)
					{
						return false;
					}
					if (@char != EncodingParser.GetCharReturnType.SUCCESSFUL)
					{
						return false;
					}
					if (c != '\r')
					{
						if (c == '\n')
						{
							num++;
						}
						if ((c == ' ' || c == '\t') && flag3)
						{
							flag7 = true;
						}
						if (!flag3 || (c != ' ' && c != '\t' && c != '\n'))
						{
							if (flag3)
							{
								if ((this.mCmdSep & 2) != 0 && !flag7 && this.mCurrentLine.Length > 0)
								{
									goto Block_15;
								}
								if (c == '#')
								{
									flag2 = true;
								}
								flag3 = false;
							}
							if (c == '\n')
							{
								flag7 = false;
								flag3 = true;
							}
							if (c == '\n' && flag2)
							{
								flag2 = false;
							}
							else if (!flag2)
							{
								if (c == '\\' && (flag4 || flag5) && !flag6)
								{
									flag6 = true;
								}
								else
								{
									if (c == '\'' && !flag5 && !flag6)
									{
										flag4 = !flag4;
									}
									if (c == '"' && !flag4 && !flag6)
									{
										flag5 = !flag5;
									}
									if (c == ';' && (this.mCmdSep & 1) != 0 && !flag4 && !flag5)
									{
										break;
									}
									if (flag6)
									{
										this.mCurrentLine.Append('\\');
										flag6 = false;
									}
									if (this.mCurrentLine.Length == 0)
									{
										this.mCurrentLineNum = num + 1;
									}
									this.mCurrentLine.Append(c);
								}
							}
						}
					}
				}
				IL_198:
				if (this.mCurrentLine.Length > 0)
				{
					list.Add(this.mCurrentLine.ToString());
					list2.Add(this.mCurrentLineNum);
					this.mCurrentLine.Clear();
					continue;
				}
				continue;
				Block_15:
				this.PutChar(c);
				goto IL_198;
			}
			this.mCurrentLine.Clear();
			this.mCurrentLineNum = 0;
			this.CloseFile();
			num = list.Count;
			for (int i = 0; i < num; i++)
			{
				this.mCurrentLineNum = list2[i];
				this.mCurrentLine.Clear();
				this.mCurrentLine.AppendLine(list[i]);
				if (!this.ParseDescriptorLine(this.mCurrentLine.ToString()))
				{
					flag = true;
					break;
				}
			}
			this.mCurrentLine.Clear();
			this.mCurrentLineNum = 0;
			return !flag;
		}

		// Token: 0x0400052E RID: 1326
		public int mCmdSep;

		// Token: 0x0400052F RID: 1327
		public string mError = "";

		// Token: 0x04000530 RID: 1328
		public int mCurrentLineNum;

		// Token: 0x04000531 RID: 1329
		public StringBuilder mCurrentLine = new StringBuilder();

		// Token: 0x04000532 RID: 1330
		public Dictionary<string, DataElement> mDefineMap = new Dictionary<string, DataElement>();

		// Token: 0x020000D0 RID: 208
		public enum ECMDSEP
		{
			// Token: 0x04000534 RID: 1332
			CMDSEP_SEMICOLON = 1,
			// Token: 0x04000535 RID: 1333
			CMDSEP_NO_INDENT
		}
	}
}
