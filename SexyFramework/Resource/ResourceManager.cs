using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace SexyFramework.Resource
{
	// Token: 0x02000199 RID: 409
	public class ResourceManager
	{
		// Token: 0x06000DFC RID: 3580 RVA: 0x0004629C File Offset: 0x0004449C
		public bool Fail(string theErrorText)
		{
			if (!this.mHasFailed)
			{
				this.mHasFailed = true;
				if (this.mXMLParser == null)
				{
					this.mError = theErrorText;
					return false;
				}
				int currentLineNum = this.mXMLParser.GetCurrentLineNum();
				this.mError = theErrorText;
				if (currentLineNum > 0)
				{
					this.mError = this.mError + " on Line " + currentLineNum;
				}
				if (this.mXMLParser.GetFileName().Length > 0)
				{
					this.mError = this.mError + " in File '" + this.mXMLParser.GetFileName() + "'";
				}
			}
			return false;
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0004633C File Offset: 0x0004453C
		protected virtual bool ParseCommonResource(XMLElement theElement, BaseRes theRes, Dictionary<string, BaseRes> theMap)
		{
			this.mHadAlreadyDefinedError = false;
			theRes.mParent = this;
			theRes.mGlobalPtr = null;
			string attribute = theElement.GetAttribute("path");
			if (attribute.Length <= 0)
			{
				return this.Fail("No path specified.");
			}
			theRes.mXMLAttributes = theElement.GetAttributeMap();
			theRes.mFromProgram = false;
			if (attribute[0] == '!')
			{
				theRes.mPath = attribute;
				if (attribute == "!program")
				{
					theRes.mFromProgram = true;
				}
			}
			else
			{
				theRes.mPath = this.mDefaultPath + attribute;
				this.mResFromPathMap[theRes.mPath.ToUpper()] = theRes;
			}
			string text;
			if (theElement.GetAttribute("id").Length > 0)
			{
				text = this.mDefaultIdPrefix + theElement.GetAttribute("id");
			}
			else
			{
				text = this.mDefaultIdPrefix + Common.GetFileName(theRes.mPath, true);
			}
			if (this.mCurResGroupArtRes != 0)
			{
				text = text + "|" + this.mCurResGroupArtRes;
			}
			if (this.mCurResGroupLocSet != 0U)
			{
				text = text + "||" + string.Format("{0:x}", this.mCurResGroupLocSet);
			}
			theRes.mResGroup = this.mCurResGroup;
			theRes.mCompositeResGroup = this.mCurCompositeResGroup;
			theRes.mId = text;
			theRes.mArtRes = this.mCurResGroupArtRes;
			theRes.mLocSet = this.mCurResGroupLocSet;
			if (theMap.ContainsKey(text))
			{
				this.mHadAlreadyDefinedError = true;
				return this.Fail("Resource already defined.");
			}
			theMap[theRes.mId] = theRes;
			this.mCurResGroupList.mResList.Add(theRes);
			return true;
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x000464E8 File Offset: 0x000446E8
		protected virtual bool ParseSoundResource(XMLElement theElement)
		{
			SoundRes soundRes = new SoundRes();
			soundRes.mSoundId = -1;
			soundRes.mVolume = -1.0;
			soundRes.mPanning = 0;
			if (!this.ParseCommonResource(theElement, soundRes, this.mResMaps[1]))
			{
				if (!this.mHadAlreadyDefinedError || !this.mAllowAlreadyDefinedResources)
				{
					return false;
				}
				this.mError = "";
				this.mHasFailed = false;
				SoundRes soundRes2 = soundRes;
				soundRes = (SoundRes)this.mResMaps[1][soundRes2.mId];
				soundRes.mPath = soundRes2.mPath;
				soundRes.mXMLAttributes = soundRes2.mXMLAttributes;
			}
			if (theElement.HasAttribute("volume"))
			{
				double.TryParse(theElement.GetAttribute("volume"), NumberStyles.Float, CultureInfo.InvariantCulture, out soundRes.mVolume);
			}
			if (theElement.HasAttribute("pan"))
			{
				int.TryParse(theElement.GetAttribute("pan"), out soundRes.mPanning);
			}
			soundRes.ApplyConfig();
			soundRes.mReloadIdx = this.mReloadIdx;
			return true;
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x000465EC File Offset: 0x000447EC
		protected virtual bool ParseImageResource(XMLElement theElement)
		{
			string attribute = theElement.GetAttribute("id");
			if (attribute.Length <= 0)
			{
				return true;
			}
			string attribute2 = theElement.GetAttribute("path");
			if (attribute2.Length <= 0)
			{
				return true;
			}
			ImageRes imageRes = new ImageRes();
			if (!this.ParseCommonResource(theElement, imageRes, this.mResMaps[0]))
			{
				if (!this.mHadAlreadyDefinedError || !this.mAllowAlreadyDefinedResources)
				{
					return false;
				}
				this.mError = "";
				this.mHasFailed = false;
				ImageRes imageRes2 = imageRes;
				imageRes = (ImageRes)this.mResMaps[0][imageRes2.mId];
				imageRes.mPath = imageRes2.mPath;
				imageRes.mXMLAttributes = imageRes2.mXMLAttributes;
			}
			imageRes.mPalletize = !theElement.GetAttributeBool("nopal", false);
			imageRes.mA4R4G4B4 = theElement.GetAttributeBool("a4r4g4b4", false);
			imageRes.mDDSurface = theElement.GetAttributeBool("ddsurface", false);
			bool flag = true;
			imageRes.mPurgeBits = theElement.GetAttributeBool("nobits", false) || (flag && theElement.GetAttributeBool("nobits3d", false)) || (!flag && theElement.GetAttributeBool("nobits2d", false));
			imageRes.mA8R8G8B8 = theElement.GetAttributeBool("a8r8g8b8", false);
			imageRes.mDither16 = theElement.GetAttributeBool("dither16", false);
			imageRes.mMinimizeSubdivisions = theElement.GetAttributeBool("minsubdivide", false);
			imageRes.mAutoFindAlpha = !theElement.GetAttributeBool("noalpha", false);
			imageRes.mCubeMap = theElement.GetAttributeBool("cubemap", false);
			imageRes.mVolumeMap = theElement.GetAttributeBool("volumemap", false);
			imageRes.mNoTriRep = theElement.GetAttributeBool("notrirep", false) || theElement.GetAttributeBool("noquadrep", false);
			imageRes.m2DBig = theElement.GetAttributeBool("2dbig", false);
			imageRes.mIsAtlas = theElement.GetAttributeBool("atlas", false);
			if (theElement.HasAttribute("alphaimage"))
			{
				imageRes.mAlphaImage = this.mDefaultPath + theElement.GetAttribute("alphaimage");
			}
			imageRes.mAlphaColor = 16777215;
			if (theElement.HasAttribute("alphacolor"))
			{
				imageRes.mAlphaColor = int.Parse(string.Format("x", theElement.GetAttribute("alphacolor")));
			}
			imageRes.mOffset = new SexyPoint(0, 0);
			if (theElement.HasAttribute("x"))
			{
				imageRes.mOffset.mX = int.Parse(theElement.GetAttribute("x"));
			}
			if (theElement.HasAttribute("y"))
			{
				imageRes.mOffset.mY = int.Parse(theElement.GetAttribute("y"));
			}
			if (theElement.HasAttribute("variant"))
			{
				imageRes.mVariant = theElement.GetAttribute("variant");
			}
			if (theElement.HasAttribute("alphagrid"))
			{
				imageRes.mAlphaGridImage = this.mDefaultPath + theElement.GetAttribute("alphagrid");
			}
			if (theElement.HasAttribute("rows"))
			{
				imageRes.mRows = int.Parse(theElement.GetAttribute("rows"));
			}
			if (theElement.HasAttribute("cols"))
			{
				imageRes.mCols = int.Parse(theElement.GetAttribute("cols"));
			}
			if (theElement.HasAttribute("parent"))
			{
				imageRes.mAtlasName = theElement.GetAttribute("parent");
				imageRes.mAtlasX = int.Parse(theElement.GetAttribute("ax"));
				imageRes.mAtlasY = int.Parse(theElement.GetAttribute("ay"));
				imageRes.mAtlasW = int.Parse(theElement.GetAttribute("aw"));
				imageRes.mAtlasH = int.Parse(theElement.GetAttribute("ah"));
			}
			if (imageRes.mCubeMap)
			{
				if (imageRes.mRows * imageRes.mCols != 6)
				{
					this.Fail("Invalid CubeMap definition; must have 6 cells (check rows & cols values).");
					return false;
				}
			}
			else if (imageRes.mVolumeMap)
			{
				int num = imageRes.mRows * imageRes.mCols;
				if (num == 0 || (num & (num - 1)) != 0)
				{
					this.Fail("Invalid VolumeMap definition; must have a pow2 cell count (check rows & cols values).");
					return false;
				}
			}
			AnimType animType = AnimType.AnimType_None;
			if (theElement.HasAttribute("anim"))
			{
				string attribute3 = theElement.GetAttribute("anim");
				if (attribute3.ToLower() == "none")
				{
					animType = AnimType.AnimType_None;
				}
				else if (attribute3.ToLower() == "once")
				{
					animType = AnimType.AnimType_Once;
				}
				else if (attribute3.ToLower() == "loop")
				{
					animType = AnimType.AnimType_Loop;
				}
				else
				{
					if (!(attribute3.ToLower() == "pingpong"))
					{
						this.Fail("Invalid animation type.");
						return false;
					}
					animType = AnimType.AnimType_PingPong;
				}
			}
			imageRes.mAnimInfo.mAnimType = animType;
			if (animType != AnimType.AnimType_None)
			{
				int theNumCels = Math.Max(imageRes.mRows, imageRes.mCols);
				int theBeginFrameTime = 0;
				int theEndFrameTime = 0;
				if (theElement.HasAttribute("framedelay"))
				{
					imageRes.mAnimInfo.mFrameDelay = int.Parse(theElement.GetAttribute("framedelay"));
				}
				if (theElement.HasAttribute("begindelay"))
				{
					theBeginFrameTime = (imageRes.mAnimInfo.mBeginDelay = int.Parse(theElement.GetAttribute("begindelay")));
				}
				if (theElement.HasAttribute("enddelay"))
				{
					theEndFrameTime = (imageRes.mAnimInfo.mEndDelay = int.Parse(theElement.GetAttribute("enddelay")));
				}
				if (theElement.HasAttribute("perframedelay"))
				{
					ResourceManager.ReadIntVector(theElement.GetAttribute("perframedelay"), imageRes.mAnimInfo.mPerFrameDelay);
				}
				if (theElement.HasAttribute("framemap"))
				{
					ResourceManager.ReadIntVector(theElement.GetAttribute("framemap"), imageRes.mAnimInfo.mFrameMap);
				}
				imageRes.mAnimInfo.Compute(theNumCels, theBeginFrameTime, theEndFrameTime);
			}
			imageRes.ApplyConfig();
			imageRes.mReloadIdx = this.mReloadIdx;
			return true;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00046BA0 File Offset: 0x00044DA0
		protected virtual bool ParseFontResource(XMLElement theElement)
		{
			FontRes fontRes = new FontRes();
			fontRes.mFont = null;
			fontRes.mImage = null;
			if (!this.ParseCommonResource(theElement, fontRes, this.mResMaps[2]))
			{
				if (!this.mHadAlreadyDefinedError || !this.mAllowAlreadyDefinedResources)
				{
					return false;
				}
				this.mError = "";
				this.mHasFailed = false;
				FontRes fontRes2 = fontRes;
				fontRes = (FontRes)this.mResMaps[2][fontRes2.mId];
				fontRes.mPath = fontRes2.mPath;
				fontRes.mXMLAttributes = fontRes2.mXMLAttributes;
			}
			fontRes.mImagePath = "";
			if (theElement.HasAttribute("image"))
			{
				fontRes.mImagePath = theElement.GetAttribute("image");
			}
			if (theElement.HasAttribute("tags"))
			{
				fontRes.mTags = theElement.GetAttribute("tags");
			}
			if (fontRes.mImagePath.StartsWith("!sys:"))
			{
				fontRes.mSysFont = true;
				string mPath = fontRes.mPath;
				fontRes.mPath = mPath.Substring(5);
				if (!theElement.HasAttribute("size"))
				{
					return this.Fail("SysFont needs point size");
				}
				fontRes.mSize = int.Parse(theElement.GetAttribute("size"));
				if (fontRes.mSize <= 0)
				{
					return this.Fail("SysFont needs point size");
				}
				fontRes.mBold = theElement.GetAttributeBool("bold", false);
				fontRes.mItalic = theElement.GetAttributeBool("italic", false);
				fontRes.mShadow = theElement.GetAttributeBool("shadow", false);
				fontRes.mUnderline = theElement.GetAttributeBool("underline", false);
			}
			else
			{
				fontRes.mSysFont = false;
			}
			fontRes.ApplyConfig();
			fontRes.mReloadIdx = this.mReloadIdx;
			return true;
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00046D50 File Offset: 0x00044F50
		protected virtual bool ParsePopAnimResource(XMLElement theElement)
		{
			PopAnimRes popAnimRes = new PopAnimRes();
			if (!this.ParseCommonResource(theElement, popAnimRes, this.mResMaps[3]))
			{
				if (!this.mHadAlreadyDefinedError || !this.mAllowAlreadyDefinedResources)
				{
					return false;
				}
				this.mError = "";
				this.mHasFailed = false;
				PopAnimRes popAnimRes2 = popAnimRes;
				popAnimRes = (PopAnimRes)this.mResMaps[3][popAnimRes2.mId];
				popAnimRes.mPath = popAnimRes2.mPath;
				popAnimRes.mXMLAttributes = popAnimRes2.mXMLAttributes;
			}
			popAnimRes.ApplyConfig();
			popAnimRes.mReloadIdx = this.mReloadIdx;
			return true;
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x00046DE8 File Offset: 0x00044FE8
		protected virtual bool ParsePIEffectResource(XMLElement theElement)
		{
			PIEffectRes pieffectRes = new PIEffectRes();
			if (!this.ParseCommonResource(theElement, pieffectRes, this.mResMaps[4]))
			{
				if (!this.mHadAlreadyDefinedError || !this.mAllowAlreadyDefinedResources)
				{
					return false;
				}
				this.mError = "";
				this.mHasFailed = false;
				PIEffectRes pieffectRes2 = pieffectRes;
				pieffectRes = (PIEffectRes)this.mResMaps[4][pieffectRes2.mId];
				pieffectRes.mPath = pieffectRes2.mPath;
				pieffectRes.mXMLAttributes = pieffectRes2.mXMLAttributes;
			}
			pieffectRes.ApplyConfig();
			pieffectRes.mReloadIdx = this.mReloadIdx;
			return true;
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00046E80 File Offset: 0x00045080
		protected virtual bool ParseRenderEffectResource(XMLElement theElement)
		{
			RenderEffectRes renderEffectRes = new RenderEffectRes();
			if (!this.ParseCommonResource(theElement, renderEffectRes, this.mResMaps[5]))
			{
				if (!this.mHadAlreadyDefinedError || !this.mAllowAlreadyDefinedResources)
				{
					return false;
				}
				this.mError = "";
				this.mHasFailed = false;
				RenderEffectRes renderEffectRes2 = renderEffectRes;
				renderEffectRes = (RenderEffectRes)this.mResMaps[5][renderEffectRes2.mId];
				renderEffectRes.mPath = renderEffectRes2.mPath;
				renderEffectRes.mXMLAttributes = renderEffectRes2.mXMLAttributes;
			}
			renderEffectRes.mSrcFilePath = "";
			if (theElement.HasAttribute("srcpath") && theElement.GetAttribute("srcpath").Length > 0)
			{
				renderEffectRes.mSrcFilePath = this.mDefaultPath + theElement.GetAttribute("srcpath");
			}
			renderEffectRes.ApplyConfig();
			renderEffectRes.mReloadIdx = this.mReloadIdx;
			return true;
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x00046F5C File Offset: 0x0004515C
		protected virtual bool ParseGenericResFileResource(XMLElement theElement)
		{
			GenericResFileRes genericResFileRes = new GenericResFileRes();
			if (!this.ParseCommonResource(theElement, genericResFileRes, this.mResMaps[6]))
			{
				if (!this.mHadAlreadyDefinedError || !this.mAllowAlreadyDefinedResources)
				{
					return false;
				}
				this.mError = "";
				this.mHasFailed = false;
				GenericResFileRes genericResFileRes2 = genericResFileRes;
				genericResFileRes = (GenericResFileRes)this.mResMaps[6][genericResFileRes2.mId];
				genericResFileRes.mPath = genericResFileRes2.mPath;
				genericResFileRes.mXMLAttributes = genericResFileRes2.mXMLAttributes;
			}
			genericResFileRes.ApplyConfig();
			genericResFileRes.mReloadIdx = this.mReloadIdx;
			return true;
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00046FF4 File Offset: 0x000451F4
		protected virtual bool ParseSetDefaults(XMLElement theElement)
		{
			if (theElement.HasAttribute("path"))
			{
				this.mDefaultPath = Common.RemoveTrailingSlash(theElement.GetAttribute("path")) + "/";
			}
			if (theElement.HasAttribute("idprefix"))
			{
				this.mDefaultIdPrefix = Common.RemoveTrailingSlash(theElement.GetAttribute("idprefix"));
			}
			return true;
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00047054 File Offset: 0x00045254
		public virtual bool ParseResources()
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
					if (xmlelement.mValue.ToString() == "Image")
					{
						if (!this.ParseImageResource(xmlelement))
						{
							return false;
						}
						if (!this.mXMLParser.NextElement(xmlelement))
						{
							return false;
						}
						if (xmlelement.mType != XMLElement.XMLElementType.TYPE_END)
						{
							goto Block_5;
						}
					}
					else if (xmlelement.mValue.ToString() == "Sound")
					{
						if (!this.ParseSoundResource(xmlelement))
						{
							return false;
						}
						if (!this.mXMLParser.NextElement(xmlelement))
						{
							return false;
						}
						if (xmlelement.mType != XMLElement.XMLElementType.TYPE_END)
						{
							goto Block_9;
						}
					}
					else if (xmlelement.mValue.ToString() == "Font")
					{
						if (!this.ParseFontResource(xmlelement))
						{
							return false;
						}
						if (!this.mXMLParser.NextElement(xmlelement))
						{
							return false;
						}
						if (xmlelement.mType != XMLElement.XMLElementType.TYPE_END)
						{
							goto Block_13;
						}
					}
					else if (xmlelement.mValue.ToString() == "PopAnim")
					{
						if (!this.ParsePopAnimResource(xmlelement))
						{
							return false;
						}
						if (!this.mXMLParser.NextElement(xmlelement))
						{
							return false;
						}
						if (xmlelement.mType != XMLElement.XMLElementType.TYPE_END)
						{
							goto Block_17;
						}
					}
					else if (xmlelement.mValue.ToString() == "PIEffect")
					{
						if (!this.ParsePIEffectResource(xmlelement))
						{
							return false;
						}
						if (!this.mXMLParser.NextElement(xmlelement))
						{
							return false;
						}
						if (xmlelement.mType != XMLElement.XMLElementType.TYPE_END)
						{
							goto Block_21;
						}
					}
					else if (xmlelement.mValue.ToString() == "RenderEffect")
					{
						if (!this.ParseRenderEffectResource(xmlelement))
						{
							return false;
						}
						if (!this.mXMLParser.NextElement(xmlelement))
						{
							return false;
						}
						if (xmlelement.mType != XMLElement.XMLElementType.TYPE_END)
						{
							goto Block_25;
						}
					}
					else if (xmlelement.mValue.ToString() == "File")
					{
						if (!this.ParseGenericResFileResource(xmlelement))
						{
							return false;
						}
						if (!this.mXMLParser.NextElement(xmlelement))
						{
							return false;
						}
						if (xmlelement.mType != XMLElement.XMLElementType.TYPE_END)
						{
							goto Block_29;
						}
					}
					else
					{
						if (!(xmlelement.mValue.ToString() == "SetDefaults"))
						{
							goto IL_26F;
						}
						if (!this.ParseSetDefaults(xmlelement))
						{
							return false;
						}
						if (!this.mXMLParser.NextElement(xmlelement))
						{
							return false;
						}
						if (xmlelement.mType != XMLElement.XMLElementType.TYPE_END)
						{
							goto Block_33;
						}
					}
				}
				else
				{
					if (xmlelement.mType == XMLElement.XMLElementType.TYPE_ELEMENT)
					{
						goto Block_34;
					}
					if (xmlelement.mType == XMLElement.XMLElementType.TYPE_END)
					{
						return true;
					}
				}
			}
			return false;
			Block_5:
			return this.Fail("Unexpected element found.");
			Block_9:
			return this.Fail("Unexpected element found.");
			Block_13:
			return this.Fail("Unexpected element found.");
			Block_17:
			return this.Fail("Unexpected element found.");
			Block_21:
			return this.Fail("Unexpected element found.");
			Block_25:
			return this.Fail("Unexpected element found.");
			Block_29:
			return this.Fail("Unexpected element found.");
			Block_33:
			return this.Fail("Unexpected element found.");
			IL_26F:
			this.Fail("Invalid Section '" + xmlelement.mValue + "'");
			return false;
			Block_34:
			this.Fail("Element Not Expected '" + xmlelement.mValue + "'");
			return false;
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x00047324 File Offset: 0x00045524
		public bool DoParseResources()
		{
			if (!this.mXMLParser.HasFailed())
			{
				XMLElement xmlelement;
				XMLElement xmlelement2;
				for (;;)
				{
					xmlelement = new XMLElement();
					if (!this.mXMLParser.NextElement(xmlelement))
					{
						goto IL_395;
					}
					if (xmlelement.mType == XMLElement.XMLElementType.TYPE_START)
					{
						if (xmlelement.mValue.ToString() == "Resources")
						{
							this.mCurResGroup = xmlelement.GetAttribute("id");
							if (this.mCurResGroup.Length <= 0)
							{
								break;
							}
							if (this.mResGroupMap.ContainsKey(this.mCurResGroup))
							{
								this.mCurResGroupList = this.mResGroupMap[this.mCurResGroup];
							}
							else
							{
								this.mCurResGroupList = new ResGroup();
								this.mResGroupMap[this.mCurResGroup] = this.mCurResGroupList;
							}
							this.mCurCompositeResGroup = xmlelement.GetAttribute("parent");
							string attribute = xmlelement.GetAttribute("res");
							this.mCurResGroupArtRes = ((attribute.Length <= 0) ? 0 : int.Parse(attribute));
							string attribute2 = xmlelement.GetAttribute("loc");
							this.mCurResGroupLocSet = (uint)((attribute2.Length < 4) ? '\0' : (((uint)attribute2[0] << 24) | ((uint)attribute2[1] << 16) | ((uint)attribute2[2] << 8) | attribute2[3]));
							if (!this.ParseResources())
							{
								goto Block_8;
							}
						}
						else
						{
							if (!(xmlelement.mValue.ToString() == "CompositeResources"))
							{
								goto IL_34F;
							}
							string attribute3 = xmlelement.GetAttribute("id");
							if (attribute3.Length <= 0)
							{
								goto Block_10;
							}
							CompositeResGroup compositeResGroup;
							if (this.mCompositeResGroupMap.ContainsKey(attribute3))
							{
								compositeResGroup = this.mCompositeResGroupMap[attribute3];
							}
							else
							{
								compositeResGroup = new CompositeResGroup();
								this.mCompositeResGroupMap[attribute3] = compositeResGroup;
							}
							for (;;)
							{
								xmlelement2 = new XMLElement();
								if (!this.mXMLParser.NextElement(xmlelement2))
								{
									return false;
								}
								if (xmlelement2.mType == XMLElement.XMLElementType.TYPE_START)
								{
									if (!(xmlelement2.mValue.ToString() == "Group"))
									{
										goto IL_2ED;
									}
									string attribute4 = xmlelement2.GetAttribute("id");
									int mArtRes = 0;
									string attribute5 = xmlelement2.GetAttribute("res");
									if (attribute5.Length > 0)
									{
										mArtRes = int.Parse(attribute5);
									}
									uint mLocSet = 0U;
									string attribute6 = xmlelement2.GetAttribute("loc");
									if (attribute6.Length >= 4)
									{
										mLocSet = (uint)(((uint)attribute6[0] << 24) | ((uint)attribute6[1] << 16) | ((uint)attribute6[2] << 8) | attribute6[3]);
									}
									SubGroup subGroup = new SubGroup();
									subGroup.mGroupName = attribute4;
									subGroup.mArtRes = mArtRes;
									subGroup.mLocSet = mLocSet;
									compositeResGroup.mSubGroups.Add(subGroup);
									if (!this.mXMLParser.NextElement(xmlelement2))
									{
										goto Block_17;
									}
									if (xmlelement2.mType != XMLElement.XMLElementType.TYPE_END)
									{
										goto Block_18;
									}
								}
								else
								{
									if (xmlelement2.mType == XMLElement.XMLElementType.TYPE_ELEMENT)
									{
										goto Block_19;
									}
									if (xmlelement2.mType == XMLElement.XMLElementType.TYPE_END)
									{
										break;
									}
								}
							}
							IL_342:
							if (this.mHasFailed)
							{
								goto Block_20;
							}
							continue;
							Block_18:
							this.Fail("Unexpected element found.");
							goto IL_342;
							Block_17:
							this.Fail("Group end expected");
							goto IL_342;
							IL_2ED:
							this.Fail("Invalid Section '" + xmlelement2.mValue + "' within CompositeGroup");
							goto IL_342;
						}
					}
					else if (xmlelement.mType == XMLElement.XMLElementType.TYPE_ELEMENT)
					{
						goto Block_21;
					}
				}
				this.Fail("No id specified.");
				Block_8:
				goto IL_395;
				Block_10:
				this.Fail("No id specified on CompositeGroup.");
				goto IL_395;
				Block_19:
				this.Fail("Element Not Expected '" + xmlelement2.mValue + "'");
				return false;
				Block_20:
				goto IL_395;
				IL_34F:
				this.Fail("Invalid Section '" + xmlelement.mValue + "'");
				goto IL_395;
				Block_21:
				this.Fail("Element Not Expected '" + xmlelement.mValue + "'");
			}
			IL_395:
			if (this.mXMLParser.HasFailed())
			{
				this.Fail(this.mXMLParser.GetErrorText());
			}
			this.mXMLParser.Dispose();
			this.mXMLParser = null;
			return !this.mHasFailed;
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x00047700 File Offset: 0x00045900
		public void DeleteMap(Dictionary<string, BaseRes> theMap)
		{
			foreach (KeyValuePair<string, BaseRes> keyValuePair in theMap)
			{
				keyValuePair.Value.DeleteResource();
			}
			theMap.Clear();
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0004773C File Offset: 0x0004593C
		public virtual void DeleteResources(Dictionary<string, BaseRes> theMap, string theGroup)
		{
			if (theGroup.Length <= 0)
			{
				foreach (KeyValuePair<string, BaseRes> keyValuePair in theMap)
				{
					if (keyValuePair.Value.mDirectLoaded)
					{
						keyValuePair.Value.mDirectLoaded = false;
						this.Deref(keyValuePair.Value);
					}
				}
				return;
			}
			if (this.mCompositeResGroupMap.ContainsKey(theGroup))
			{
				CompositeResGroup compositeResGroup = this.mCompositeResGroupMap[theGroup];
				int count = compositeResGroup.mSubGroups.Count;
				using (List<SubGroup>.Enumerator enumerator2 = compositeResGroup.mSubGroups.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						SubGroup subGroup = enumerator2.Current;
						if (subGroup.mGroupName.Length > 0)
						{
							foreach (KeyValuePair<string, BaseRes> keyValuePair4 in theMap)
							{
								if (keyValuePair4.Value.mDirectLoaded)
								{
									if (keyValuePair4.Value.mResGroup == subGroup.mGroupName)
									{
										keyValuePair4.Value.mDirectLoaded = false;
										this.Deref(keyValuePair4.Value);
									}
								}
							}
						}
					}
					return;
				}
			}
			foreach (KeyValuePair<string, BaseRes> keyValuePair8 in theMap)
			{
				if (keyValuePair8.Value.mDirectLoaded)
				{
					Dictionary<string, BaseRes>.Enumerator enumerator4;
					if (keyValuePair8.Value.mResGroup == theGroup)
					{
						keyValuePair8.Value.mDirectLoaded = false;
						this.Deref(keyValuePair8.Value);
					}
				}
			}
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00047914 File Offset: 0x00045B14
		public BaseRes GetBaseRes(int theType, string theId)
		{
			if (this.mCurArtResKey.Length <= 0)
			{
				this.mCurArtResKey = "|" + this.mCurArtRes;
				this.mCurLocSetKey = "||" + string.Format("x", this.mCurLocSet);
				this.mCurArtResAndLocSetKey = this.mCurArtRes + "|||" + string.Format("x", this.mCurLocSet);
			}
			if (this.mResMaps[theType].ContainsKey(theId + this.mCurArtResAndLocSetKey))
			{
				return this.mResMaps[theType][theId + this.mCurArtResAndLocSetKey];
			}
			if (this.mResMaps[theType].ContainsKey(theId + this.mCurArtResKey))
			{
				return this.mResMaps[theType][theId + this.mCurArtResKey];
			}
			if (this.mResMaps[theType].ContainsKey(theId + this.mCurLocSetKey))
			{
				return this.mResMaps[theType][theId + this.mCurLocSetKey];
			}
			if (this.mResMaps[theType].ContainsKey(theId))
			{
				return this.mResMaps[theType][theId];
			}
			return null;
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00047A5E File Offset: 0x00045C5E
		public void Deref(BaseRes theRes)
		{
			theRes.mRefCount--;
			if (theRes.mRefCount == 0)
			{
				theRes.DeleteResource();
			}
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x00047A7C File Offset: 0x00045C7C
		public bool LoadAlphaGridImage(ImageRes theRes, DeviceImage theImage)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00047A83 File Offset: 0x00045C83
		public bool LoadAlphaImage(ImageRes theRes, DeviceImage theImage)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00047A8C File Offset: 0x00045C8C
		public virtual bool DoLoadImage(ImageRes theRes)
		{
			new AutoCrit(this.mLoadCrit);
			string mPath = theRes.mPath;
			if (mPath.StartsWith("!ref:"))
			{
				string text = mPath.Substring(5);
				theRes.mResourceRef = this.GetImageRef(text);
				SharedImageRef sharedImageRef = theRes.mResourceRef.GetSharedImageRef();
				if (sharedImageRef.GetImage() == null)
				{
					sharedImageRef = this.LoadImage(text);
				}
				if (sharedImageRef.GetImage() == null)
				{
					return this.Fail("Ref Image not found: " + text);
				}
				theRes.mImage = sharedImageRef;
				theRes.mGlobalPtr = this.RegisterGlobalPtr(text);
				return true;
			}
			else
			{
				bool flag = theRes.mAtlasName != null;
				bool flag2 = false;
				SharedImageRef sharedImageRef2 = this.mApp.CheckSharedImage(mPath, theRes.mVariant);
				if (sharedImageRef2.GetDeviceImage() != null)
				{
					flag2 = true;
				}
				else if (!flag)
				{
					DeviceImage deviceImage = DeviceImage.ReadFromCache(Common.GetFullPath(mPath), "ResMan");
					if (deviceImage != null)
					{
						sharedImageRef2 = this.mApp.SetSharedImage(mPath, theRes.mVariant, deviceImage);
						theRes.mImage = sharedImageRef2;
						flag2 = true;
					}
				}
				bool flag3 = false;
				bool mWriteToSexyCache = this.mApp.mWriteToSexyCache;
				this.mApp.mWriteToSexyCache = false;
				if (!flag2)
				{
					sharedImageRef2 = this.mApp.GetSharedImage(mPath, theRes.mVariant, ref flag3, !theRes.mNoTriRep, flag);
				}
				this.mApp.mWriteToSexyCache = mWriteToSexyCache;
				DeviceImage deviceImage2 = sharedImageRef2.GetDeviceImage();
				if (deviceImage2 == null)
				{
					return this.Fail("Failed to load image: " + mPath);
				}
				if (flag3)
				{
					if (flag)
					{
						deviceImage2.mWidth = theRes.mAtlasW;
						deviceImage2.mHeight = theRes.mAtlasH;
					}
					if (theRes.mAlphaImage.Length > 0 && !this.LoadAlphaImage(theRes, deviceImage2))
					{
						return false;
					}
					if (theRes.mAlphaGridImage.Length > 0 && !this.LoadAlphaGridImage(theRes, deviceImage2))
					{
						return false;
					}
				}
				if (theRes.mPalletize && !flag2)
				{
					if (deviceImage2.mSurface == null)
					{
						deviceImage2.Palletize();
					}
					else
					{
						deviceImage2.mWantPal = true;
					}
				}
				theRes.mImage = sharedImageRef2;
				theRes.ApplyConfig();
				theRes.mImage.GetImage().mNameForRes = theRes.mId;
				if (theRes.mGlobalPtr != null)
				{
					theRes.mGlobalPtr.mResObject = deviceImage2;
				}
				if (!flag2 && !flag)
				{
					deviceImage2.WriteToCache(Common.GetFullPath(mPath), "ResMan");
				}
				this.ResourceLoadedHook(theRes);
				return true;
			}
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00047CCC File Offset: 0x00045ECC
		public virtual bool DoLoadFont(FontRes theRes)
		{
			new AutoCrit(this.mLoadCrit);
			Font font = null;
			string text = theRes.mPath;
			string text2 = string.Format("path{0}", this.mCurArtRes);
			if (theRes.mXMLAttributes.ContainsKey(text2))
			{
				text = theRes.mXMLAttributes[text2];
			}
			if (!theRes.mSysFont)
			{
				if (string.IsNullOrEmpty(theRes.mImagePath))
				{
					if (string.Compare(text, 0, "!ref:", 0, 5) == 0)
					{
						string text3 = text.Substring(5);
						theRes.mResourceRef = this.GetFontRef(text3);
						Font font2 = theRes.mResourceRef.GetFont();
						if (font2 == null)
						{
							return this.Fail("Ref Font not found: " + text3);
						}
						font = (theRes.mFont = font2.Duplicate());
					}
					else
					{
						ImageFont imageFont = ImageFont.ReadFromCache(Common.GetFullPath(text), "ResMan");
						if (imageFont != null)
						{
							font = imageFont;
						}
						else
						{
							imageFont = new ImageFont(this.mApp, text, "");
							font = imageFont;
						}
					}
				}
				else
				{
					Image image = this.mApp.GetImage(theRes.mImagePath, false, false, false);
					if (image == null)
					{
						return this.Fail(string.Format("Failed to load image: {0}", theRes.mImagePath));
					}
					theRes.mImage = image;
				}
			}
			ImageFont imageFont2 = font.AsImageFont();
			if (imageFont2 != null)
			{
				if (imageFont2.mFontData == null || !imageFont2.mFontData.mInitialized)
				{
					if (font != null)
					{
						font.Dispose();
					}
					return this.Fail(string.Format("Failed to load font: {0}", text));
				}
				imageFont2.mTagVector.Clear();
				imageFont2.mActiveListValid = false;
				if (!string.IsNullOrEmpty(theRes.mTags))
				{
					string[] array = theRes.mTags.Split(", \r\n\t".ToCharArray());
					for (int i = 0; i < array.Length; i++)
					{
						imageFont2.AddTag(array[i]);
					}
					imageFont2.Prepare();
				}
			}
			theRes.mFont = imageFont2;
			if (theRes.mGlobalPtr != null)
			{
				theRes.mGlobalPtr.mResObject = font;
			}
			theRes.ApplyConfig();
			this.ResourceLoadedHook(theRes);
			return true;
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00047ED0 File Offset: 0x000460D0
		public virtual bool DoLoadSound(SoundRes theRes)
		{
			new AutoCrit(this.mLoadCrit);
			string mPath = theRes.mPath;
			if (theRes.mPath.StartsWith("!ref:"))
			{
				string text = mPath.Substring(5);
				theRes.mResourceRef = this.GetSoundRef(text);
				int soundID = theRes.mResourceRef.GetSoundID();
				if (soundID == -1)
				{
					return this.Fail("Ref sound not found: " + text);
				}
				theRes.mSoundId = soundID;
				return true;
			}
			else
			{
				int freeSoundId = this.mApp.mSoundManager.GetFreeSoundId();
				if (freeSoundId < 0)
				{
					return this.Fail("Out of free sound ids");
				}
				if (!this.mApp.mSoundManager.LoadSound((uint)freeSoundId, theRes.mPath))
				{
					return this.Fail("Failed to load sound: " + theRes.mPath);
				}
				theRes.mSoundId = freeSoundId;
				if (theRes.mGlobalPtr != null)
				{
					theRes.mGlobalPtr.mResObject = freeSoundId;
				}
				theRes.ApplyConfig();
				this.ResourceLoadedHook(theRes);
				return true;
			}
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00047FCC File Offset: 0x000461CC
		public virtual bool DoLoadPopAnim(PopAnimRes theRes)
		{
			PopAnim popAnim = new PopAnim(0, null);
			popAnim.mImgScale = (float)this.mCurArtRes / (float)this.mLeadArtRes;
			popAnim.mDrawScale = (float)this.mCurArtRes / (float)this.mLeadArtRes;
			popAnim.LoadFile(theRes.mPath);
			if (popAnim.mError.Length > 0)
			{
				this.Fail("PopAnim loading error: " + popAnim.mError + " on file " + theRes.mPath);
				popAnim.Dispose();
				return false;
			}
			if (theRes.mGlobalPtr != null)
			{
				theRes.mGlobalPtr.mResObject = popAnim;
			}
			theRes.mPopAnim = popAnim;
			return true;
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0004806C File Offset: 0x0004626C
		public virtual bool DoLoadPIEffect(PIEffectRes theRes)
		{
			PIEffect pieffect = new PIEffect();
			pieffect.LoadEffect(theRes.mPath);
			if (pieffect.mError.Length > 0)
			{
				this.Fail("PIEffect loading error: " + pieffect.mError + " on file " + theRes.mPath);
				pieffect.Dispose();
				return false;
			}
			if (theRes.mGlobalPtr != null)
			{
				theRes.mGlobalPtr.mResObject = pieffect;
			}
			theRes.mPIEffect = pieffect;
			return true;
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x000480E2 File Offset: 0x000462E2
		public virtual bool DoLoadRenderEffect(RenderEffectRes theRes)
		{
			return true;
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x000480E5 File Offset: 0x000462E5
		public virtual bool DoLoadGenericResFile(GenericResFileRes theRes)
		{
			return true;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x000480E8 File Offset: 0x000462E8
		public int GetNumResources(string theGroup, Dictionary<string, BaseRes> theMap, bool curArtResOnly, bool curLocSetOnly)
		{
			int num = 0;
			if (theGroup.Length <= 0)
			{
				if (!curArtResOnly && !curLocSetOnly)
				{
					return theMap.Count;
				}
				foreach (KeyValuePair<string, BaseRes> keyValuePair in theMap)
				{
					BaseRes value = keyValuePair.Value;
					if ((!curArtResOnly || value.mArtRes == 0 || value.mArtRes == this.mCurArtRes) && (!curLocSetOnly || value.mLocSet == 0U || value.mLocSet == this.mCurLocSet) && !value.mFromProgram)
					{
						num++;
					}
				}
			}
			else
			{
				foreach (KeyValuePair<string, BaseRes> keyValuePair2 in theMap)
				{
					BaseRes value2 = keyValuePair2.Value;
					if ((!curArtResOnly || value2.mArtRes == 0 || value2.mArtRes == this.mCurArtRes) && (!curLocSetOnly || value2.mLocSet == 0U || value2.mLocSet == this.mCurLocSet) && (value2.mResGroup == theGroup || value2.mCompositeResGroup == theGroup) && !value2.mFromProgram)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x000481FC File Offset: 0x000463FC
		public ResourceManager(SexyAppBase theApp)
		{
			this.mApp = theApp;
			for (int i = 0; i < 7; i++)
			{
				this.mResMaps[i] = new Dictionary<string, BaseRes>();
			}
			this.mBaseArtRes = 0;
			this.mLeadArtRes = 0;
			this.mCurArtRes = 0;
			this.mCurLocSet = 1162761555U;
			this.mHasFailed = false;
			this.mXMLParser = null;
			this.mResGenMajorVersion = 0;
			this.mResGenMinorVersion = 0;
			this.mAllowMissingProgramResources = false;
			this.mAllowAlreadyDefinedResources = false;
			this.mCurResGroupList = null;
			this.mReloadIdx = 0;
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00048304 File Offset: 0x00046504
		public virtual void Dispose()
		{
			for (int i = 0; i < 7; i++)
			{
				this.DeleteMap(this.mResMaps[i]);
			}
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0004832C File Offset: 0x0004652C
		public bool ParseResourcesFileBinary(byte[] data)
		{
			this.mXMLParser = new XMLParser();
			this.mXMLParser.checkEncodingType(data);
			this.mXMLParser.SetBytes(data);
			XMLElement xmlelement = new XMLElement();
			while (!this.mXMLParser.HasFailed())
			{
				if (!this.mXMLParser.NextElement(xmlelement))
				{
					this.Fail(this.mXMLParser.GetErrorText());
				}
				if (xmlelement.mType == XMLElement.XMLElementType.TYPE_START)
				{
					if (!(xmlelement.mValue.ToString() != "ResourceManifest"))
					{
						if (xmlelement.GetAttribute("version").Length > 0)
						{
							int.Parse(xmlelement.GetAttribute("version"));
						}
						return this.DoParseResources();
					}
					break;
				}
			}
			this.Fail("Expecting ResourceManifest tag");
			return this.DoParseResources();
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x000483F0 File Offset: 0x000465F0
		public bool ParseResourcesFile(string theFilename)
		{
			this.mLastXMLFileName = theFilename;
			if (this.mApp.mResStreamsManager != null && this.mApp.mResStreamsManager.IsInitialized())
			{
				return this.mApp.mResStreamsManager.LoadResourcesManifest(this);
			}
			this.mXMLParser = new XMLParser();
			if (!this.mXMLParser.OpenFile(theFilename))
			{
				this.Fail("Resource file not found: " + theFilename);
			}
			XMLElement xmlelement = new XMLElement();
			while (!this.mXMLParser.HasFailed())
			{
				if (!this.mXMLParser.NextElement(xmlelement))
				{
					this.Fail(this.mXMLParser.GetErrorText());
				}
				if (xmlelement.mType == XMLElement.XMLElementType.TYPE_START)
				{
					if (!(xmlelement.mValue.ToString() != "ResourceManifest"))
					{
						if (xmlelement.GetAttribute("version").Length > 0)
						{
							int.Parse(xmlelement.GetAttribute("version"));
						}
						return this.DoParseResources();
					}
					break;
				}
			}
			this.Fail("Expecting ResourceManifest tag");
			return this.DoParseResources();
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x000484F4 File Offset: 0x000466F4
		public bool ReparseResourcesFile(string theFilename)
		{
			bool flag = this.mAllowAlreadyDefinedResources;
			this.mAllowAlreadyDefinedResources = true;
			this.mReloadIdx++;
			bool result = this.ParseResourcesFile(theFilename);
			for (int i = 0; i < 7; i++)
			{
				foreach (KeyValuePair<string, BaseRes> keyValuePair in this.mResMaps[i])
				{
					BaseRes value = keyValuePair.Value;
					if (value.mReloadIdx != this.mReloadIdx)
					{
						value.DeleteResource();
					}
				}
			}
			this.mAllowAlreadyDefinedResources = flag;
			return result;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0004857C File Offset: 0x0004677C
		public ResGlobalPtr RegisterGlobalPtr(string theId)
		{
			for (int i = 0; i < 7; i++)
			{
				BaseRes baseRes = this.GetBaseRes(i, theId);
				if (baseRes != null)
				{
					return baseRes.mGlobalPtr;
				}
			}
			return null;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x000485AC File Offset: 0x000467AC
		public void ReapplyConfigs()
		{
			for (int i = 0; i < 7; i++)
			{
				foreach (KeyValuePair<string, BaseRes> keyValuePair in this.mResMaps[i])
				{
					keyValuePair.Value.ApplyConfig();
				}
			}
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x000485F2 File Offset: 0x000467F2
		public string GetErrorText()
		{
			return this.mError;
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x000485FA File Offset: 0x000467FA
		public bool HadError()
		{
			return this.mHasFailed;
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00048602 File Offset: 0x00046802
		public bool IsGroupLoaded(string theGroup)
		{
			return this.mLoadedGroups.Contains(theGroup);
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00048610 File Offset: 0x00046810
		public bool IsResourceLoaded(string theId)
		{
			return this.GetImage(theId).GetDeviceImage() != null || this.GetFont(theId) != null || this.GetSound(theId) != -1;
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0004863A File Offset: 0x0004683A
		public int GetNumImages(string theGroup, bool curArtResOnly, bool curLocSetOnly)
		{
			return this.GetNumResources(theGroup, this.mResMaps[0], curArtResOnly, curLocSetOnly);
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0004864D File Offset: 0x0004684D
		public int GetNumSounds(string theGroup, bool curArtResOnly, bool curLocSetOnly)
		{
			return this.GetNumResources(theGroup, this.mResMaps[1], curArtResOnly, curLocSetOnly);
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x00048660 File Offset: 0x00046860
		public int GetNumFonts(string theGroup, bool curArtResOnly, bool curLocSetOnly)
		{
			return this.GetNumResources(theGroup, this.mResMaps[2], curArtResOnly, curLocSetOnly);
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x00048674 File Offset: 0x00046874
		public int GetNumResources(string theGroup, bool curArtResOnly, bool curLocSetOnly)
		{
			int num = 0;
			for (int i = 0; i < 7; i++)
			{
				num += this.GetNumResources(theGroup, this.mResMaps[i], curArtResOnly, curLocSetOnly);
			}
			return num;
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x000486A4 File Offset: 0x000468A4
		public virtual bool DoLoadResource(BaseRes theRes, out bool skipped)
		{
			skipped = false;
			if (theRes.mFromProgram)
			{
				skipped = true;
				return true;
			}
			switch (theRes.mType)
			{
			case ResType.ResType_Image:
			{
				ImageRes theRes2 = (ImageRes)theRes;
				return this.DoLoadImage(theRes2);
			}
			case ResType.ResType_Sound:
			{
				SoundRes theRes3 = (SoundRes)theRes;
				return this.DoLoadSound(theRes3);
			}
			case ResType.ResType_Font:
			{
				FontRes theRes4 = (FontRes)theRes;
				return this.DoLoadFont(theRes4);
			}
			case ResType.ResType_PopAnim:
			{
				PopAnimRes theRes5 = (PopAnimRes)theRes;
				return this.DoLoadPopAnim(theRes5);
			}
			case ResType.ResType_PIEffect:
			{
				PIEffectRes theRes6 = (PIEffectRes)theRes;
				return this.DoLoadPIEffect(theRes6);
			}
			case ResType.ResType_RenderEffect:
			{
				RenderEffectRes theRes7 = (RenderEffectRes)theRes;
				return this.DoLoadRenderEffect(theRes7);
			}
			case ResType.ResType_GenericResFile:
			{
				GenericResFileRes theRes8 = (GenericResFileRes)theRes;
				return this.DoLoadGenericResFile(theRes8);
			}
			default:
				return false;
			}
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00048760 File Offset: 0x00046960
		public virtual bool LoadNextResource()
		{
			if (this.HadError())
			{
				return false;
			}
			if (this.mCurResGroupList == null)
			{
				return false;
			}
			while (this.mCurResGroupListItr.MoveNext())
			{
				bool result = true;
				bool flag = true;
				BaseRes baseRes = this.mCurResGroupListItr.Current;
				if (baseRes.mRefCount == 0)
				{
					result = this.DoLoadResource(baseRes, out flag);
				}
				baseRes.mDirectLoaded = true;
				baseRes.mRefCount = 1;
				if (!flag)
				{
					return result;
				}
			}
			if (this.mCurCompositeResGroup.Length > 0 && this.mCompositeResGroupMap.ContainsKey(this.mCurCompositeResGroup))
			{
				CompositeResGroup compositeResGroup = this.mCompositeResGroupMap[this.mCurCompositeResGroup];
				int count = compositeResGroup.mSubGroups.Count;
				for (int i = this.mCurCompositeSubGroupIndex + 1; i < count; i++)
				{
					SubGroup subGroup = compositeResGroup.mSubGroups[i];
					if (subGroup.mGroupName.Length > 0 && (subGroup.mArtRes == 0 || subGroup.mArtRes == this.mCurArtRes) && (subGroup.mLocSet == 0U || subGroup.mLocSet == this.mCurLocSet))
					{
						this.mCurCompositeSubGroupIndex = i;
						this.StartLoadResources(subGroup.mGroupName, true);
						return this.LoadNextResource();
					}
				}
			}
			return false;
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0004888D File Offset: 0x00046A8D
		public virtual void ResourceLoadedHook(BaseRes theRes)
		{
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0004888F File Offset: 0x00046A8F
		public virtual void PrepareLoadResources(string theGroup)
		{
			if (this.mApp.mResStreamsManager != null && this.mApp.mResStreamsManager.IsInitialized())
			{
				this.mApp.mResStreamsManager.LoadGroup(theGroup);
			}
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x000488C4 File Offset: 0x00046AC4
		public virtual void StartLoadResources(string theGroup, bool fromComposite)
		{
			if (!fromComposite)
			{
				this.mError = "";
				this.mHasFailed = false;
				this.mCurCompositeResGroup = "";
				this.mCurCompositeSubGroupIndex = 0;
				if (this.mCompositeResGroupMap.ContainsKey(theGroup))
				{
					this.mCurResGroup = "";
					this.mCurResGroupList = null;
					this.mCurCompositeResGroup = theGroup;
					CompositeResGroup compositeResGroup = this.mCompositeResGroupMap[theGroup];
					int count = compositeResGroup.mSubGroups.Count;
					for (int i = 0; i < count; i++)
					{
						SubGroup subGroup = compositeResGroup.mSubGroups[i];
						if (subGroup.mGroupName.Length > 0 && (subGroup.mArtRes == 0 || subGroup.mArtRes == this.mCurArtRes) && (subGroup.mLocSet == 0U || subGroup.mLocSet == this.mCurLocSet))
						{
							this.mCurCompositeSubGroupIndex = i;
							this.StartLoadResources(subGroup.mGroupName, true);
							return;
						}
					}
					return;
				}
			}
			if (this.mResGroupMap.ContainsKey(theGroup))
			{
				this.mCurResGroup = theGroup;
				this.mCurResGroupList = this.mResGroupMap[theGroup];
				this.mCurResGroupListItr = this.mCurResGroupList.mResList.GetEnumerator();
			}
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x000489E4 File Offset: 0x00046BE4
		public virtual bool LoadResources(string theGroup)
		{
			if (this.mApp.mResStreamsManager != null && this.mApp.mResStreamsManager.IsInitialized())
			{
				this.mApp.mResStreamsManager.ForceLoadGroup(theGroup);
			}
			this.mError = "";
			this.mHasFailed = false;
			this.StartLoadResources(theGroup, false);
			while (this.LoadNextResource())
			{
			}
			if (!this.HadError())
			{
				this.mLoadedGroups.Add(theGroup);
				return true;
			}
			return false;
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00048A5A File Offset: 0x00046C5A
		public bool ReplaceImage(string theId, Image theImage)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x00048A61 File Offset: 0x00046C61
		public bool ReplaceSound(string theId, int theSound)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00048A68 File Offset: 0x00046C68
		public bool ReplaceFont(string theId, Font theFont)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00048A6F File Offset: 0x00046C6F
		public bool ReplacePopAnim(string theId, PopAnim theFont)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00048A76 File Offset: 0x00046C76
		public bool ReplacePIEffect(string theId, PIEffect theFont)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00048A7D File Offset: 0x00046C7D
		public bool ReplaceRenderEffect(string theId, RenderEffectDefinition theDefinition)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00048A84 File Offset: 0x00046C84
		public bool ReplaceGenericResFile(string theId, GenericResFile theFile)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00048A8C File Offset: 0x00046C8C
		public void DeleteImage(string theName)
		{
			BaseRes baseRes = this.GetBaseRes(0, theName);
			if (baseRes != null && baseRes.mDirectLoaded)
			{
				baseRes.mDirectLoaded = false;
				this.Deref(baseRes);
			}
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00048ABC File Offset: 0x00046CBC
		public SharedImageRef LoadImage(string theName)
		{
			new AutoCrit(this.mLoadCrit);
			ImageRes imageRes = (ImageRes)this.GetBaseRes(0, theName);
			if (imageRes == null)
			{
				return null;
			}
			if (!imageRes.mDirectLoaded)
			{
				imageRes.mRefCount++;
				imageRes.mDirectLoaded = true;
			}
			if (imageRes.mImage.GetDeviceImage() != null)
			{
				return imageRes.mImage;
			}
			if (imageRes.mFromProgram)
			{
				return null;
			}
			if (!this.DoLoadImage(imageRes))
			{
				return null;
			}
			return imageRes.mImage;
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00048B40 File Offset: 0x00046D40
		public SexyPoint GetImageOffset(string theName)
		{
			ImageRes imageRes = (ImageRes)this.GetBaseRes(0, theName);
			if (imageRes != null)
			{
				return imageRes.mOffset;
			}
			return ResourceManager.aEmptyPoint;
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00048B6A File Offset: 0x00046D6A
		public void DeleteFont(string theName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00048B74 File Offset: 0x00046D74
		public Font LoadFont(string theName)
		{
			new AutoCrit(this.mLoadCrit);
			FontRes fontRes = (FontRes)this.GetBaseRes(2, theName);
			if (fontRes == null)
			{
				return null;
			}
			if (!fontRes.mDirectLoaded)
			{
				fontRes.mRefCount++;
				fontRes.mDirectLoaded = true;
			}
			if (fontRes.mFont != null)
			{
				return fontRes.mFont;
			}
			if (fontRes.mFromProgram)
			{
				return null;
			}
			if (!this.DoLoadFont(fontRes))
			{
				return null;
			}
			return fontRes.mFont;
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00048BE8 File Offset: 0x00046DE8
		public int LoadSound(string theName)
		{
			new AutoCrit(this.mLoadCrit);
			SoundRes soundRes = (SoundRes)this.GetBaseRes(1, theName);
			if (soundRes == null)
			{
				return -1;
			}
			if (!soundRes.mDirectLoaded)
			{
				soundRes.mRefCount++;
				soundRes.mDirectLoaded = true;
			}
			if (soundRes.mSoundId != 0)
			{
				return soundRes.mSoundId;
			}
			if (soundRes.mFromProgram)
			{
				return -1;
			}
			if (!this.DoLoadSound(soundRes))
			{
				return -1;
			}
			return soundRes.mSoundId;
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00048C5B File Offset: 0x00046E5B
		public void DeleteSound(string theName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00048C62 File Offset: 0x00046E62
		public void DeletePopAnim(string theName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00048C6C File Offset: 0x00046E6C
		public PopAnim LoadPopAnim(string theName)
		{
			PopAnimRes popAnimRes = (PopAnimRes)this.GetBaseRes(3, theName);
			if (popAnimRes == null)
			{
				return null;
			}
			if (!popAnimRes.mDirectLoaded)
			{
				popAnimRes.mRefCount++;
				popAnimRes.mDirectLoaded = true;
			}
			if (popAnimRes.mPopAnim != null)
			{
				return popAnimRes.mPopAnim;
			}
			if (popAnimRes.mFromProgram)
			{
				return null;
			}
			if (!this.DoLoadPopAnim(popAnimRes))
			{
				return null;
			}
			return popAnimRes.mPopAnim;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00048CD3 File Offset: 0x00046ED3
		public void DeletePIEffect(string theName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00048CDC File Offset: 0x00046EDC
		public PIEffect LoadPIEffect(string theName)
		{
			PIEffectRes pieffectRes = (PIEffectRes)this.GetBaseRes(4, theName);
			if (pieffectRes == null)
			{
				return null;
			}
			if (!pieffectRes.mDirectLoaded)
			{
				pieffectRes.mRefCount++;
				pieffectRes.mDirectLoaded = true;
			}
			if (pieffectRes.mPIEffect != null)
			{
				return pieffectRes.mPIEffect;
			}
			if (pieffectRes.mFromProgram)
			{
				return null;
			}
			if (!this.DoLoadPIEffect(pieffectRes))
			{
				return null;
			}
			return pieffectRes.mPIEffect;
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00048D43 File Offset: 0x00046F43
		public void DeleteRenderEffect(string theName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x00048D4A File Offset: 0x00046F4A
		public RenderEffectDefinition LoadRenderEffect(string theName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00048D51 File Offset: 0x00046F51
		public void DeleteGenericResFile(string theName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00048D58 File Offset: 0x00046F58
		public GenericResFile LoadGenericResFile(string theName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00048D60 File Offset: 0x00046F60
		public SharedImageRef GetImage(string theId)
		{
			ImageRes imageRes = (ImageRes)this.GetBaseRes(0, theId);
			if (imageRes == null)
			{
				return null;
			}
			return imageRes.mImage;
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x00048D88 File Offset: 0x00046F88
		public int GetSound(string theId)
		{
			SoundRes soundRes = (SoundRes)this.GetBaseRes(1, theId);
			if (soundRes == null)
			{
				return 0;
			}
			return soundRes.mSoundId;
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x00048DB0 File Offset: 0x00046FB0
		public Font GetFont(string theId)
		{
			FontRes fontRes = (FontRes)this.GetBaseRes(2, theId);
			if (fontRes == null)
			{
				return null;
			}
			return fontRes.mFont;
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x00048DD8 File Offset: 0x00046FD8
		public PopAnim GetPopAnim(string theId)
		{
			PopAnimRes popAnimRes = (PopAnimRes)this.GetBaseRes(3, theId);
			if (popAnimRes == null)
			{
				return null;
			}
			return popAnimRes.mPopAnim;
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00048E00 File Offset: 0x00047000
		public PIEffect GetPIEffect(string theId)
		{
			PIEffectRes pieffectRes = (PIEffectRes)this.GetBaseRes(4, theId);
			if (pieffectRes == null)
			{
				return null;
			}
			return pieffectRes.mPIEffect;
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00048E28 File Offset: 0x00047028
		public RenderEffectDefinition GetRenderEffect(string theId)
		{
			RenderEffectRes renderEffectRes = (RenderEffectRes)this.GetBaseRes(5, theId);
			if (renderEffectRes == null)
			{
				return null;
			}
			return renderEffectRes.mRenderEffectDefinition;
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00048E50 File Offset: 0x00047050
		public GenericResFile GetGenericResFile(string theId)
		{
			GenericResFileRes genericResFileRes = (GenericResFileRes)this.GetBaseRes(6, theId);
			if (genericResFileRes == null)
			{
				return null;
			}
			return genericResFileRes.mGenericResFile;
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x00048E78 File Offset: 0x00047078
		public string GetIdByPath(string thePath)
		{
			string text = thePath.Replace('/', '\\');
			for (int i = 0; i < 7; i++)
			{
				foreach (KeyValuePair<string, BaseRes> keyValuePair in this.mResMaps[i])
				{
					if (keyValuePair.Value.mPath == text)
					{
						return keyValuePair.Value.mId;
					}
				}
			}
			text = text.ToUpper();
			for (int j = 0; j < 7; j++)
			{
				foreach (KeyValuePair<string, BaseRes> keyValuePair3 in this.mResMaps[j])
				{
					if (keyValuePair3.Value.mPath.ToUpper() == text)
					{
						return keyValuePair3.Value.mId;
					}
				}
			}
			return "";
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x00048F54 File Offset: 0x00047154
		public Dictionary<string, string> GetImageAttributes(string theId)
		{
			ImageRes imageRes = (ImageRes)this.GetBaseRes(0, theId);
			if (imageRes != null)
			{
				return imageRes.mXMLAttributes;
			}
			return null;
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00048F7C File Offset: 0x0004717C
		public virtual SharedImageRef GetImageThrow(string theId, int artRes, bool optional)
		{
			if (this.mApp.mShutdown)
			{
				return null;
			}
			if (artRes != 0 && artRes != this.mCurArtRes)
			{
				this.Fail(string.Concat(new object[] { "Attempted to load image of incorrect art resolution ", artRes, " (expected ", this.mCurArtRes, "):", theId }));
				throw new ResourceManagerException(this.GetErrorText());
			}
			ImageRes imageRes = (ImageRes)this.GetBaseRes(0, theId);
			if (imageRes != null)
			{
				if (imageRes.mImage.GetMemoryImage() != null)
				{
					return imageRes.mImage;
				}
				if (this.mAllowMissingProgramResources && imageRes.mFromProgram)
				{
					return null;
				}
			}
			else if (optional)
			{
				return null;
			}
			this.Fail("Image resource not found:" + theId);
			imageRes = (ImageRes)this.GetBaseRes(0, theId);
			throw new ResourceManagerException(this.GetErrorText());
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0004905E File Offset: 0x0004725E
		public virtual int GetSoundThrow(string theId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00049065 File Offset: 0x00047265
		public virtual Font GetFontThrow(string theId, int artRes)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0004906C File Offset: 0x0004726C
		public virtual PopAnim GetPopAnimThrow(string theId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x00049073 File Offset: 0x00047273
		public virtual PIEffect GetPIEffectThrow(string theId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0004907A File Offset: 0x0004727A
		public virtual RenderEffectDefinition GetRenderEffectThrow(string theId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00049081 File Offset: 0x00047281
		public virtual GenericResFile GetGenericResFileThrow(string theId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x00049088 File Offset: 0x00047288
		public ResourceRef GetResourceRef(BaseRes theBaseRes)
		{
			ResourceRef resourceRef = new ResourceRef();
			bool flag = false;
			resourceRef.mBaseResP = theBaseRes;
			if (theBaseRes.mRefCount == 0)
			{
				this.DoLoadResource(theBaseRes, out flag);
			}
			theBaseRes.mRefCount = 1;
			return resourceRef;
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x000490C0 File Offset: 0x000472C0
		public ResourceRef GetResourceRef(int theType, string theId)
		{
			BaseRes baseRes = this.GetBaseRes(theType, theId);
			if (baseRes != null)
			{
				return this.GetResourceRef(baseRes);
			}
			return null;
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x000490E4 File Offset: 0x000472E4
		public ResourceRef GetResourceRefFromPath(string theFileName)
		{
			string text = theFileName.ToUpper();
			if (text.IndexOf(".") != -1)
			{
				text = text.Substring(0, text.IndexOf("."));
			}
			if (this.mResFromPathMap.ContainsKey(text))
			{
				return this.GetResourceRef(this.mResFromPathMap[text]);
			}
			return null;
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x0004913C File Offset: 0x0004733C
		public SexyPoint GetOffsetOfImage(string theId)
		{
			ImageRes imageRes = (ImageRes)this.GetBaseRes(0, theId);
			if (imageRes == null)
			{
				return null;
			}
			return imageRes.mOffset;
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00049162 File Offset: 0x00047362
		public ResourceRef GetImageRef(string theId)
		{
			return this.GetResourceRef(0, theId);
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x0004916C File Offset: 0x0004736C
		public ResourceRef GetImageRef(Image theGlobalPtrRef)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00049173 File Offset: 0x00047373
		public ResourceRef GetSoundRef(string theId)
		{
			return this.GetResourceRef(1, theId);
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x0004917D File Offset: 0x0004737D
		public ResourceRef GetSoundRef(int theGlobalPtrRef)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00049184 File Offset: 0x00047384
		public ResourceRef GetFontRef(string theId)
		{
			return this.GetResourceRef(2, theId);
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0004918E File Offset: 0x0004738E
		public ResourceRef GetPopAnimRef(string theId)
		{
			return this.GetResourceRef(3, theId);
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00049198 File Offset: 0x00047398
		public ResourceRef GetPopAnimRef(PopAnim theGlobalPtrRef)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0004919F File Offset: 0x0004739F
		public ResourceRef GetPIEffectRef(string theId)
		{
			return this.GetResourceRef(4, theId);
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x000491A9 File Offset: 0x000473A9
		public ResourceRef GetPIEffectRef(PIEffect theGlobalPtrRef)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x000491B0 File Offset: 0x000473B0
		public ResourceRef GetRenderEffectRef(string theId)
		{
			return this.GetResourceRef(5, theId);
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x000491BA File Offset: 0x000473BA
		public ResourceRef GetRenderEffectRef(RenderEffectDefinition theGlobalPtrRef)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x000491C1 File Offset: 0x000473C1
		public ResourceRef GetGenericResFileRef(string theId)
		{
			return this.GetResourceRef(6, theId);
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x000491CB File Offset: 0x000473CB
		public ResourceRef GetGenericResFileRef(GenericResFile theGlobalPtrRef)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x000491D2 File Offset: 0x000473D2
		public void SetAllowMissingProgramImages(bool allow)
		{
			this.mAllowMissingProgramResources = allow;
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x000491DC File Offset: 0x000473DC
		public virtual void DeleteResources(string theGroup)
		{
			for (int i = 0; i < 7; i++)
			{
				this.DeleteResources(this.mResMaps[i], theGroup);
			}
			this.mLoadedGroups.Remove(theGroup);
			if (this.mApp.mResStreamsManager != null && this.mApp.mResStreamsManager.IsInitialized())
			{
				this.mApp.mResStreamsManager.DeleteGroup(theGroup);
			}
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x00049242 File Offset: 0x00047442
		public void DeleteExtraImageBuffers(string theGroup)
		{
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00049244 File Offset: 0x00047444
		public ResGroup GetCurResGroupList()
		{
			return this.mCurResGroupList;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0004924C File Offset: 0x0004744C
		public string GetCurResGroup()
		{
			return this.mCurResGroup;
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x00049254 File Offset: 0x00047454
		public void DumpCurResGroup(ref string theDestStr)
		{
			ResGroup resGroup = this.mResGroupMap[this.mCurResGroup];
			List<BaseRes>.Enumerator enumerator = resGroup.mResList.GetEnumerator();
			object obj = theDestStr;
			theDestStr = string.Concat(new object[]
			{
				obj,
				"\n About to dump ",
				resGroup.mResList.Count,
				" elements from current res group name \r\n"
			});
			while (enumerator.MoveNext())
			{
				BaseRes baseRes = enumerator.Current;
				string text = baseRes.mId + ":" + baseRes.mPath + "\r\n";
				theDestStr += text;
				if (baseRes.mFromProgram)
				{
					theDestStr += "     res is from program\r\n";
				}
				else if (baseRes.mType == ResType.ResType_Image)
				{
					theDestStr += "     res is an image\r\n";
				}
				else if (baseRes.mType == ResType.ResType_Sound)
				{
					theDestStr += "     res is a sound\r\n";
				}
				else if (baseRes.mType == ResType.ResType_Font)
				{
					theDestStr += "     res is a font\r\n";
				}
				else if (baseRes.mType == ResType.ResType_PopAnim)
				{
					theDestStr += "     res is a popanim\r\n";
				}
				else if (baseRes.mType == ResType.ResType_PIEffect)
				{
					theDestStr += "     res is a pieffect\r\n";
				}
				else if (baseRes.mType == ResType.ResType_RenderEffect)
				{
					theDestStr += "     res is a rendereffectdefinition\r\n";
				}
				else if (baseRes.mType == ResType.ResType_GenericResFile)
				{
					theDestStr += "     res is a genericresfile\r\n";
				}
				if (enumerator.Current == this.mCurResGroupListItr.Current)
				{
					theDestStr += "iterator has reached mCurResGroupItr\r\n";
				}
			}
			theDestStr += "Done dumping resources\r\n";
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x000493FC File Offset: 0x000475FC
		public void DumpAllGroup(ref string theDestStr)
		{
			string text = this.mCurResGroup;
			foreach (KeyValuePair<string, ResGroup> keyValuePair in this.mResGroupMap)
			{
				this.mCurResGroup = keyValuePair.Key;
				this.DumpCurResGroup(ref theDestStr);
			}
			this.mCurResGroup = text;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0004944C File Offset: 0x0004764C
		public string GetLocaleFolder(bool addTrailingSlash)
		{
			uint num = this.mCurLocSet;
			if (num == 0U)
			{
				return "";
			}
			string text = string.Concat(new object[]
			{
				"locales/",
				(num >> 24) & 255U,
				(num >> 16) & 255U,
				"-",
				(num >> 8) & 255U,
				num & 255U
			});
			if (addTrailingSlash)
			{
				text += '/';
			}
			return text;
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x000494DD File Offset: 0x000476DD
		public uint GetLocSetForLocaleName(string theLocaleName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x000494E4 File Offset: 0x000476E4
		public void PrepareLoadResourcesList(string[] theGroups)
		{
			if (this.mApp.mResStreamsManager != null && this.mApp.mResStreamsManager.IsInitialized())
			{
				int num = 0;
				while (theGroups[num] != null)
				{
					int num2 = this.mApp.mResStreamsManager.LookupGroup(theGroups[num]);
					if (num2 != -1)
					{
						this.mApp.mResStreamsManager.LoadGroup(num2);
					}
					num++;
				}
			}
			this.LoadGroupAsyn(theGroups);
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00049550 File Offset: 0x00047750
		public float GetLoadResourcesListProgress(string[] theGroups)
		{
			if (this.mTotalResNum == 0)
			{
				return 0f;
			}
			if (!this.mLoadFinished && this.mCurLoadedResNum != this.mTotalResNum)
			{
				return (float)this.mCurLoadedResNum / (float)this.mTotalResNum;
			}
			if (!this.mLoadFinished)
			{
				return 0.99f;
			}
			return 1f;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x000495A4 File Offset: 0x000477A4
		public bool IsLoadSuccess()
		{
			return this.mLoadSuccess;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x000495AC File Offset: 0x000477AC
		public void LoadGroupAsyn(string[] group)
		{
			if (this.mIsLoading)
			{
				return;
			}
			this.mError = "";
			this.mHasFailed = false;
			this.mLoadFinished = false;
			this.mGroupToLoad.Clear();
			this.mTotalResNum = 0;
			this.mCurLoadedResNum = 0;
			for (int i = 0; i < group.Length; i++)
			{
				this.GetLoadingGroup(group[i]);
				foreach (ResourceManager.GroupLoadInfo groupLoadInfo in this.mGroupToLoad)
				{
					this.mTotalResNum += groupLoadInfo.mTotalFile;
				}
			}
			this.mIsLoading = true;
			this.mLoadSuccess = false;
			this.mLoadingProc = new ParameterizedThreadStart(this.LoadingProc);
			this.mLoadingThread = new Thread(this.mLoadingProc);
			this.mLoadingThread.Start(group);
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x0004969C File Offset: 0x0004789C
		private void GetLoadingGroup(string theGroup)
		{
			this.mGroupToLoad.Clear();
			this.mError = "";
			this.mHasFailed = false;
			if (theGroup == null)
			{
				return;
			}
			if (this.mCompositeResGroupMap.ContainsKey(theGroup))
			{
				CompositeResGroup compositeResGroup = this.mCompositeResGroupMap[theGroup];
				int count = compositeResGroup.mSubGroups.Count;
				for (int i = 0; i < count; i++)
				{
					SubGroup subGroup = compositeResGroup.mSubGroups[i];
					if (subGroup.mGroupName.Length > 0 && (subGroup.mArtRes == 0 || subGroup.mArtRes == this.mCurArtRes) && (subGroup.mLocSet == 0U || subGroup.mLocSet == this.mCurLocSet))
					{
						this.mCurCompositeSubGroupIndex = i;
						if (this.mResGroupMap.ContainsKey(subGroup.mGroupName))
						{
							ResGroup resGroup = this.mResGroupMap[subGroup.mGroupName];
							this.mGroupToLoad.Add(new ResourceManager.GroupLoadInfo(subGroup.mGroupName, resGroup.mResList.Count));
						}
					}
				}
				return;
			}
			if (this.mResGroupMap.ContainsKey(theGroup))
			{
				ResGroup resGroup2 = this.mResGroupMap[theGroup];
				this.mGroupToLoad.Add(new ResourceManager.GroupLoadInfo(theGroup, resGroup2.mResList.Count));
			}
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x000497D8 File Offset: 0x000479D8
		private void LoadingProc(object theGroup)
		{
			string[] array = (string[])theGroup;
			for (int i = 0; i < array.Length - 1; i++)
			{
				this.StartLoadResources(array[i], false);
				while (this.LoadNextResource())
				{
					this.mCurLoadedResNum++;
					Thread.Sleep(3);
				}
				if (this.HadError())
				{
					this.mLoadSuccess = false;
					break;
				}
				this.mLoadedGroups.Add(array[i]);
				this.mLoadSuccess = true;
			}
			this.mIsLoading = false;
			this.mLoadFinished = true;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x0004985C File Offset: 0x00047A5C
		public static void ReadIntVector(string theVal, List<int> theVector)
		{
			theVector.Clear();
			string[] array = theVal.Split(new char[] { ',' });
			foreach (string text in array)
			{
				theVector.Add(int.Parse(text));
			}
		}

		// Token: 0x04000B79 RID: 2937
		public List<string> mLoadedGroups = new List<string>();

		// Token: 0x04000B7A RID: 2938
		public Dictionary<string, BaseRes>[] mResMaps = new Dictionary<string, BaseRes>[7];

		// Token: 0x04000B7B RID: 2939
		public Dictionary<string, BaseRes> mResFromPathMap = new Dictionary<string, BaseRes>();

		// Token: 0x04000B7C RID: 2940
		public Dictionary<string, ResGroup> mResGroupMap = new Dictionary<string, ResGroup>();

		// Token: 0x04000B7D RID: 2941
		public Dictionary<string, CompositeResGroup> mCompositeResGroupMap = new Dictionary<string, CompositeResGroup>();

		// Token: 0x04000B7E RID: 2942
		public List<int> mSupportedLocSets = new List<int>();

		// Token: 0x04000B7F RID: 2943
		public ResGroup mCurResGroupList;

		// Token: 0x04000B80 RID: 2944
		public List<BaseRes>.Enumerator mCurResGroupListItr;

		// Token: 0x04000B81 RID: 2945
		public CritSect mLoadCrit = default(CritSect);

		// Token: 0x04000B82 RID: 2946
		public SexyAppBase mApp;

		// Token: 0x04000B83 RID: 2947
		public string mLastXMLFileName;

		// Token: 0x04000B84 RID: 2948
		public string mResGenExePath;

		// Token: 0x04000B85 RID: 2949
		public string mResPropsUsed;

		// Token: 0x04000B86 RID: 2950
		public string mResWatchFileUsed;

		// Token: 0x04000B87 RID: 2951
		public string mResGenTargetName;

		// Token: 0x04000B88 RID: 2952
		public string mResGenRelSrcRootFromDist;

		// Token: 0x04000B89 RID: 2953
		public string mError;

		// Token: 0x04000B8A RID: 2954
		public string mCurCompositeResGroup;

		// Token: 0x04000B8B RID: 2955
		public string mCurResGroup;

		// Token: 0x04000B8C RID: 2956
		public string mDefaultPath;

		// Token: 0x04000B8D RID: 2957
		public string mDefaultIdPrefix;

		// Token: 0x04000B8E RID: 2958
		public int mResGenMajorVersion;

		// Token: 0x04000B8F RID: 2959
		public int mResGenMinorVersion;

		// Token: 0x04000B90 RID: 2960
		public int mCurResGroupArtRes;

		// Token: 0x04000B91 RID: 2961
		public int mReloadIdx;

		// Token: 0x04000B92 RID: 2962
		public int mCurCompositeSubGroupIndex;

		// Token: 0x04000B93 RID: 2963
		public int mBaseArtRes;

		// Token: 0x04000B94 RID: 2964
		public int mCurArtRes;

		// Token: 0x04000B95 RID: 2965
		public int mLeadArtRes;

		// Token: 0x04000B96 RID: 2966
		public uint mCurLocSet;

		// Token: 0x04000B97 RID: 2967
		public uint mCurResGroupLocSet;

		// Token: 0x04000B98 RID: 2968
		public int mTotalResNum;

		// Token: 0x04000B99 RID: 2969
		public int mCurLoadedResNum;

		// Token: 0x04000B9A RID: 2970
		public bool mHasFailed;

		// Token: 0x04000B9B RID: 2971
		public bool mAllowMissingProgramResources;

		// Token: 0x04000B9C RID: 2972
		public bool mAllowAlreadyDefinedResources;

		// Token: 0x04000B9D RID: 2973
		public bool mHadAlreadyDefinedError;

		// Token: 0x04000B9E RID: 2974
		public XMLParser mXMLParser;

		// Token: 0x04000B9F RID: 2975
		private string mCurArtResKey = "";

		// Token: 0x04000BA0 RID: 2976
		private string mCurLocSetKey = "";

		// Token: 0x04000BA1 RID: 2977
		private string mCurArtResAndLocSetKey = "";

		// Token: 0x04000BA2 RID: 2978
		private static SexyPoint aEmptyPoint = new SexyPoint();

		// Token: 0x04000BA3 RID: 2979
		public bool mIsLoading;

		// Token: 0x04000BA4 RID: 2980
		private Thread mLoadingThread;

		// Token: 0x04000BA5 RID: 2981
		private ParameterizedThreadStart mLoadingProc;

		// Token: 0x04000BA6 RID: 2982
		private bool mLoadSuccess;

		// Token: 0x04000BA7 RID: 2983
		private List<ResourceManager.GroupLoadInfo> mGroupToLoad = new List<ResourceManager.GroupLoadInfo>();

		// Token: 0x04000BA8 RID: 2984
		private bool mLoadFinished;

		// Token: 0x0200019A RID: 410
		private class GroupLoadInfo
		{
			// Token: 0x06000E73 RID: 3699 RVA: 0x000498B4 File Offset: 0x00047AB4
			public GroupLoadInfo(string name, int totalFiles)
			{
				this.mName = name;
				this.mTotalFile = totalFiles;
				this.mCurrentFile = 0;
			}

			// Token: 0x04000BA9 RID: 2985
			public string mName;

			// Token: 0x04000BAA RID: 2986
			public int mTotalFile;

			// Token: 0x04000BAB RID: 2987
			public int mCurrentFile;
		}
	}
}
