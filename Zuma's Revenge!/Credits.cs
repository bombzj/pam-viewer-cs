using System;
using System.Collections.Generic;
using System.Globalization;
using JeffLib;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.Resource;

namespace ZumasRevenge
{
	// Token: 0x0200008C RID: 140
	public class Credits
	{
		// Token: 0x06000929 RID: 2345 RVA: 0x00051FD8 File Offset: 0x000501D8
		public Credits(bool isFromMainMenu)
		{
			this.mYScrollAmt = 0f;
			this.mAlpha = 0f;
			this.mTitleFont = null;
			this.mNameFont = null;
			this.mSpaceAfterTitle = 0;
			this.mSpaceAfterName = 0;
			this.mSpaceAfterImage = 0;
			this.mScrollSpeed = 0f;
			this.mFFAlpha = 0f;
			this.mInitialDelay = 0;
			this.mSpeedUp = false;
			this.mFromMainMenu = isFromMainMenu;
			this.mEntries = new List<Credits.CreditEntry>();
			this.FONT_SHAGLOUNGE28_SHADOW = Res.GetFontByID(ResID.FONT_SHAGLOUNGE28_SHADOW);
			this.IMAGE_CREDITS_IMAGES_POLAROID = Res.GetImageByID(ResID.IMAGE_CREDITS_IMAGES_POLAROID);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0005207E File Offset: 0x0005027E
		public virtual void Dispose()
		{
			if (GameApp.gApp.mResourceManager.IsGroupLoaded("Credits"))
			{
				GameApp.gApp.mResourceManager.DeleteResources("Credits");
			}
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x000520AA File Offset: 0x000502AA
		private bool GetAttribute(XMLElement elem, string theName, ref string theValue)
		{
			if (elem.GetAttributeMap().ContainsKey(theName))
			{
				theValue = elem.GetAttributeMap()[theName];
				return true;
			}
			return false;
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x000520CC File Offset: 0x000502CC
		public void Init(bool advmode)
		{
			if (!GameApp.gApp.mResourceManager.IsGroupLoaded("Credits"))
			{
				GameApp.gApp.mResourceManager.LoadResources("Credits");
			}
			XMLParser xmlparser = new XMLParser();
			string languageSuffix = Localization.GetLanguageSuffix(Localization.GetCurrentLanguage());
			string theFilename = "properties/credits/credits" + languageSuffix + ".xml";
			xmlparser.OpenFile(theFilename);
			XMLElement xmlelement = new XMLElement();
			while (!xmlparser.HasFailed() && xmlparser.NextElement(xmlelement))
			{
				if (xmlelement.mType == XMLElement.XMLElementType.TYPE_START)
				{
					if (xmlelement.mValue.ToString() != this._S("Credits"))
					{
						break;
					}
					while (xmlparser.NextElement(xmlelement))
					{
						if (xmlelement.mType == XMLElement.XMLElementType.TYPE_START)
						{
							if (this.StrEquals(xmlelement.mValue.ToString(), this._S("Defaults")))
							{
								string text = "";
								if (this.GetAttribute(xmlelement, this._S("SpaceAfterTitle"), ref text))
								{
									this.mSpaceAfterTitle = this.StrToInt(text);
								}
								if (this.GetAttribute(xmlelement, this._S("SpaceAfterName"), ref text))
								{
									this.mSpaceAfterName = this.StrToInt(text);
								}
								if (this.GetAttribute(xmlelement, this._S("SpaceAfterPic"), ref text))
								{
									this.mSpaceAfterImage = this.StrToInt(text);
								}
								if (this.GetAttribute(xmlelement, this._S("ScrollSpeed"), ref text))
								{
									this.mScrollSpeed = Common._DS(this.StrToFloat(text));
								}
								if (this.GetAttribute(xmlelement, this._S("TitleColor"), ref text))
								{
									this.mTitleColor = new SexyColor((int)JeffLib.Common.StrToHex(this.ToString(text)));
								}
								if (this.GetAttribute(xmlelement, this._S("NameColor"), ref text))
								{
									this.mNameColor = new SexyColor((int)JeffLib.Common.StrToHex(this.ToString(text)));
								}
								if (this.GetAttribute(xmlelement, this._S("TitleFont"), ref text))
								{
									this.mTitleFont = GameApp.gApp.mResourceManager.LoadFont(text);
								}
								if (this.GetAttribute(xmlelement, this._S("NameFont"), ref text))
								{
									this.mNameFont = GameApp.gApp.mResourceManager.LoadFont(text);
								}
							}
							else if (this.StrEquals(xmlelement.mValue.ToString(), this._S("Text")))
							{
								string text2 = "";
								Credits.CreditEntry creditEntry = new Credits.CreditEntry();
								creditEntry.mSpaceAfterTitle = this.mSpaceAfterTitle;
								creditEntry.mSpaceAfterName = this.mSpaceAfterName;
								creditEntry.mSpaceAfterImage = this.mSpaceAfterImage;
								creditEntry.mTitleColor = this.mTitleColor;
								creditEntry.mNameColor = this.mNameColor;
								creditEntry.mTitleFont = this.mTitleFont;
								creditEntry.mNameFont = this.mNameFont;
								if (this.GetAttribute(xmlelement, this._S("mode"), ref text2))
								{
									creditEntry.mAlwaysShow = false;
									creditEntry.mAdvMode = this.StrEquals(text2, this._S("adventure"));
								}
								if (creditEntry.mAdvMode == advmode || creditEntry.mAlwaysShow)
								{
									if (this.GetAttribute(xmlelement, this._S("Title"), ref text2))
									{
										creditEntry.mTitle = text2;
									}
									if (this.GetAttribute(xmlelement, this._S("Name"), ref text2))
									{
										creditEntry.mName = text2;
									}
									if (this.GetAttribute(xmlelement, this._S("TitleFont"), ref text2))
									{
										creditEntry.mTitleFont = GameApp.gApp.mResourceManager.LoadFont(text2);
									}
									if (this.GetAttribute(xmlelement, this._S("NameFont"), ref text2))
									{
										creditEntry.mNameFont = GameApp.gApp.mResourceManager.LoadFont(text2);
									}
									if (this.GetAttribute(xmlelement, this._S("YOff"), ref text2))
									{
										creditEntry.mYOff = this.StrToInt(text2);
									}
									if (this.GetAttribute(xmlelement, this._S("XCenterOff"), ref text2))
									{
										creditEntry.mXCenterOff = Common._S(this.StrToInt(text2));
									}
									if (this.GetAttribute(xmlelement, this._S("TitleColor"), ref text2))
									{
										creditEntry.mTitleColor = new SexyColor((int)JeffLib.Common.StrToHex(this.ToString(text2)));
									}
									if (this.GetAttribute(xmlelement, this._S("NameColor"), ref text2))
									{
										creditEntry.mNameColor = new SexyColor((int)JeffLib.Common.StrToHex(this.ToString(text2)));
									}
									if (this.GetAttribute(xmlelement, this._S("SpaceAfterTitle"), ref text2))
									{
										creditEntry.mSpaceAfterTitle = this.StrToInt(text2);
									}
									if (this.GetAttribute(xmlelement, this._S("SpaceAfterName"), ref text2))
									{
										creditEntry.mSpaceAfterName = this.StrToInt(text2);
									}
									if (this.GetAttribute(xmlelement, this._S("SpaceAfterPic"), ref text2))
									{
										creditEntry.mSpaceAfterImage = this.StrToInt(text2);
									}
									this.mEntries.Add(creditEntry);
								}
							}
							else if (this.StrEquals(xmlelement.mValue.ToString(), this._S("Image")))
							{
								string text3 = "";
								Credits.CreditEntry creditEntry2 = new Credits.CreditEntry();
								if (this.GetAttribute(xmlelement, this._S("resid"), ref text3))
								{
									creditEntry2.mImage = GameApp.gApp.mResourceManager.LoadImage(text3).GetImage();
								}
								if (this.GetAttribute(xmlelement, this._S("YOff"), ref text3))
								{
									creditEntry2.mYOff = this.StrToInt(text3);
								}
								if (this.GetAttribute(xmlelement, this._S("xflip"), ref text3))
								{
									creditEntry2.mXFlip = this.StrToBool(text3);
								}
								if (this.GetAttribute(xmlelement, this._S("polaroid"), ref text3))
								{
									creditEntry2.mDoPolaroid = this.StrToBool(text3);
									if (!creditEntry2.mDoPolaroid)
									{
										creditEntry2.mImgAlpha = 255f;
									}
								}
								if (this.GetAttribute(xmlelement, this._S("SpaceAfterPic"), ref text3))
								{
									creditEntry2.mSpaceAfterImage = this.StrToInt(text3);
								}
								if (this.GetAttribute(xmlelement, this._S("x"), ref text3))
								{
									if (this.StrEquals(text3, this._S("center")))
									{
										creditEntry2.mXCenterOff = -creditEntry2.mImage.mWidth / 2;
									}
									else
									{
										creditEntry2.mXCenterOff = -GameApp.gApp.mWidth / 2 + Common._S(this.StrToInt(text3));
									}
								}
								this.mEntries.Add(creditEntry2);
							}
						}
					}
				}
			}
			xmlparser.CloseFile();
			int num = GameApp.gApp.mHeight;
			for (int i = 0; i < this.mEntries.Count; i++)
			{
				Credits.CreditEntry creditEntry3 = this.mEntries[i];
				num += Common._S(creditEntry3.mYOff);
				creditEntry3.mInitialY = num;
				if (creditEntry3.mImage == null)
				{
					if (creditEntry3.mTitle.Length > 0)
					{
						num += creditEntry3.mTitleFont.GetHeight() + Common._S(creditEntry3.mSpaceAfterTitle);
					}
					if (creditEntry3.mName.Length > 0)
					{
						num += creditEntry3.mNameFont.GetHeight() + Common._S(creditEntry3.mSpaceAfterName);
					}
				}
				else
				{
					num += Common._S(creditEntry3.mSpaceAfterImage) + creditEntry3.mImage.mHeight;
				}
			}
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x00052810 File Offset: 0x00050A10
		public bool AtEnd()
		{
			Credits.CreditEntry creditEntry = this.mEntries[this.mEntries.Count - 1];
			return (creditEntry.mImage != null && (float)creditEntry.mInitialY + this.mYScrollAmt <= (float)(GameApp.gApp.mHeight / 2 - creditEntry.mImage.mHeight / 2 - Common._DS(Common._M(200)))) || (creditEntry.mImage == null && (float)creditEntry.mInitialY + this.mYScrollAmt <= (float)(GameApp.gApp.mHeight / 2 - creditEntry.mTitleFont.mHeight / 2));
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x000528B4 File Offset: 0x00050AB4
		public void Update()
		{
			if (GameApp.gApp.IsHardwareBackButtonPressed() && !this.mFromMainMenu)
			{
				this.ProcessHardwareBackButton();
			}
			if (this.mAlpha < 255f)
			{
				this.mAlpha += Common._M(8f);
				if (this.mAlpha > 255f)
				{
					this.mAlpha = 255f;
					return;
				}
			}
			else
			{
				for (int i = 0; i < this.mEntries.Count; i++)
				{
					Credits.CreditEntry creditEntry = this.mEntries[i];
					int num = (int)((float)creditEntry.mInitialY + this.mYScrollAmt);
					if (creditEntry.mImage != null && creditEntry.mDoPolaroid && num <= Common._DS(Common._M(900)))
					{
						if (creditEntry.mImgAlpha < 255f)
						{
							creditEntry.mImgAlpha += Common._M(0.5f);
						}
						if (creditEntry.mImgAlpha > 255f)
						{
							creditEntry.mImgAlpha = 255f;
						}
					}
				}
				if (!this.AtEnd())
				{
					if (++this.mInitialDelay >= Common._M(100))
					{
						this.mYScrollAmt -= this.mScrollSpeed * (float)(this.mSpeedUp ? Common._M(4) : 1);
					}
					if (this.mInitialDelay >= Common._M(300))
					{
						this.mFFAlpha += Common._M(2f) * (float)(this.mSpeedUp ? Common._M1(4) : 1);
						if (this.mFFAlpha > 255f)
						{
							this.mFFAlpha = 255f;
							return;
						}
					}
				}
				else
				{
					this.mFFAlpha -= Common._M(2f);
					if (this.mFFAlpha < 0f)
					{
						this.mFFAlpha = 0f;
					}
				}
			}
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x00052A80 File Offset: 0x00050C80
		public void Draw(Graphics g)
		{
			g.SetColor(new SexyColor(0, 0, 0, (int)this.mAlpha));
			g.FillRect(Common._S(-80), 0, GameApp.gApp.mWidth + Common._S(160), GameApp.gApp.mHeight);
			for (int i = 0; i < this.mEntries.Count; i++)
			{
				Credits.CreditEntry creditEntry = this.mEntries[i];
				int num = (int)((float)creditEntry.mInitialY + this.mYScrollAmt);
				if (this.AtEnd() && i == this.mEntries.Count - 1)
				{
					num = ((creditEntry.mImage != null) ? ((GameApp.gApp.mHeight - creditEntry.mImage.mHeight) / 2 - Common._DS(Common._M(200))) : ((GameApp.gApp.mHeight - creditEntry.mTitleFont.mHeight) / 2));
				}
				g.PushState();
				bool flag = GameApp.gApp.mUserProfile.mAdvModeVars.mHighestZoneBeat >= 6 || !this.mFromMainMenu;
				if (flag && creditEntry.mImage != null)
				{
					if (num > -350 && num < 700)
					{
						g.PushState();
						float num2 = (float)(creditEntry.mXFlip ? Common._DS(Common._M(this.mRoll)) : 0);
						if (creditEntry.mDoPolaroid)
						{
							g.DrawImageMirror(this.IMAGE_CREDITS_IMAGES_POLAROID, (int)((float)(GameApp.gApp.mWidth / 2 + creditEntry.mXCenterOff - Common._DS(Common._M(60))) - num2), num - Common._DS(Common._M1(36)), creditEntry.mXFlip);
							g.SetColorizeImages(true);
							g.SetColor(new SexyColor(255, 255, 255, (int)creditEntry.mImgAlpha));
						}
						g.DrawImageMirror(creditEntry.mImage, GameApp.gApp.mWidth / 2 + creditEntry.mXCenterOff, num, creditEntry.mXFlip);
						g.PopState();
					}
				}
				else if (num > -100 && num < 700)
				{
					int theAlpha = 255;
					if (creditEntry.mTitle.Length > 0)
					{
						g.SetFont(creditEntry.mTitleFont);
						g.SetColor(new SexyColor(creditEntry.mTitleColor.mRed, creditEntry.mTitleColor.mGreen, creditEntry.mTitleColor.mBlue, theAlpha));
						g.WriteString(creditEntry.mTitle, creditEntry.mXCenterOff, num + creditEntry.mTitleFont.GetAscent(), GameApp.gApp.mWidth, 0);
						num += creditEntry.mSpaceAfterTitle + creditEntry.mTitleFont.GetHeight();
					}
					if (creditEntry.mName.Length > 0)
					{
						g.SetFont(creditEntry.mNameFont);
						g.SetColor(new SexyColor(creditEntry.mNameColor.mRed, creditEntry.mNameColor.mGreen, creditEntry.mNameColor.mBlue, theAlpha));
						g.WriteString(creditEntry.mName, creditEntry.mXCenterOff, num + creditEntry.mNameFont.GetAscent(), GameApp.gApp.mWidth, 0);
					}
				}
				g.PopState();
			}
			g.SetFont(this.FONT_SHAGLOUNGE28_SHADOW);
			if (!this.AtEnd())
			{
				g.SetColor(new SexyColor(Common._M(255), Common._M1(255), Common._M2(255), (int)(this.mFFAlpha * Common._M3(0.5f))));
				g.DrawString(TextManager.getInstance().getString(435), Common._DS(Common._M(750)), Common._DS(Common._M1(1176)));
				return;
			}
			g.SetColor(new SexyColor(Common._M(255), Common._M1(255), Common._M2(255), 200));
			g.DrawString(TextManager.getInstance().getString(433), Common._DS(Common._M(750)), Common._DS(Common._M1(1176)));
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x00052E85 File Offset: 0x00051085
		public void ProcessHardwareBackButton()
		{
			GameApp.gApp.ReturnFromCredits();
			GameApp.gApp.OnHardwareBackButtonPressProcessed();
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00052E9B File Offset: 0x0005109B
		private float StrToFloat(string str)
		{
			if (str.Length == 0)
			{
				return 0f;
			}
			return float.Parse(str, NumberStyles.Float, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x00052EBB File Offset: 0x000510BB
		private int StrToInt(string str)
		{
			if (str.Length == 0)
			{
				return 0;
			}
			return int.Parse(str);
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00052ECD File Offset: 0x000510CD
		private bool StrToBool(string str)
		{
			return str.Length != 0 && bool.Parse(str);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x00052EDF File Offset: 0x000510DF
		private string ToString(string str)
		{
			return str;
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x00052EE2 File Offset: 0x000510E2
		private string _S(string str)
		{
			return str;
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00052EE5 File Offset: 0x000510E5
		private int sexyatoi(string str)
		{
			return this.StrToInt(str);
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x00052EEE File Offset: 0x000510EE
		private float sexyatof(string str)
		{
			return this.StrToFloat(str);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x00052EF7 File Offset: 0x000510F7
		private bool StrEquals(string str, string cmp)
		{
			return str == cmp;
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00052F00 File Offset: 0x00051100
		private string StringToUpper(string str)
		{
			return str.ToUpper();
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x00052F08 File Offset: 0x00051108
		private string StringToLower(string str)
		{
			return str.ToLower();
		}

		// Token: 0x04000723 RID: 1827
		public List<Credits.CreditEntry> mEntries;

		// Token: 0x04000724 RID: 1828
		public float mYScrollAmt;

		// Token: 0x04000725 RID: 1829
		public float mAlpha;

		// Token: 0x04000726 RID: 1830
		public float mFFAlpha;

		// Token: 0x04000727 RID: 1831
		public Font mTitleFont;

		// Token: 0x04000728 RID: 1832
		public Font mNameFont;

		// Token: 0x04000729 RID: 1833
		public int mSpaceAfterTitle;

		// Token: 0x0400072A RID: 1834
		public int mSpaceAfterName;

		// Token: 0x0400072B RID: 1835
		public int mSpaceAfterImage;

		// Token: 0x0400072C RID: 1836
		public SexyColor mTitleColor;

		// Token: 0x0400072D RID: 1837
		public SexyColor mNameColor;

		// Token: 0x0400072E RID: 1838
		public int mRoll = -12;

		// Token: 0x0400072F RID: 1839
		public float mScrollSpeed;

		// Token: 0x04000730 RID: 1840
		public int mInitialDelay;

		// Token: 0x04000731 RID: 1841
		public bool mSpeedUp;

		// Token: 0x04000732 RID: 1842
		public bool mFromMainMenu;

		// Token: 0x04000733 RID: 1843
		public bool mTapDown;

		// Token: 0x04000734 RID: 1844
		private Font FONT_SHAGLOUNGE28_SHADOW;

		// Token: 0x04000735 RID: 1845
		private Image IMAGE_CREDITS_IMAGES_POLAROID;

		// Token: 0x0200008D RID: 141
		public class CreditEntry
		{
			// Token: 0x0600093B RID: 2363 RVA: 0x00052F10 File Offset: 0x00051110
			public CreditEntry()
			{
				this.mImage = null;
				this.mTitleFont = null;
				this.mXFlip = false;
				this.mImgAlpha = 0f;
				this.mDoPolaroid = true;
				this.mNameFont = null;
				this.mYOff = 0;
				this.mSpaceAfterTitle = 0;
				this.mSpaceAfterName = 0;
				this.mSpaceAfterImage = 0;
				this.mAdvMode = true;
				this.mAlwaysShow = true;
				this.mXCenterOff = 0;
				this.mInitialY = 0;
				this.mTitle = "";
				this.mName = "";
			}

			// Token: 0x04000736 RID: 1846
			public string mTitle;

			// Token: 0x04000737 RID: 1847
			public string mName;

			// Token: 0x04000738 RID: 1848
			public Image mImage;

			// Token: 0x04000739 RID: 1849
			public Font mTitleFont;

			// Token: 0x0400073A RID: 1850
			public Font mNameFont;

			// Token: 0x0400073B RID: 1851
			public int mXCenterOff;

			// Token: 0x0400073C RID: 1852
			public int mYOff;

			// Token: 0x0400073D RID: 1853
			public int mInitialY;

			// Token: 0x0400073E RID: 1854
			public int mSpaceAfterTitle;

			// Token: 0x0400073F RID: 1855
			public int mSpaceAfterName;

			// Token: 0x04000740 RID: 1856
			public int mSpaceAfterImage;

			// Token: 0x04000741 RID: 1857
			public SexyColor mTitleColor;

			// Token: 0x04000742 RID: 1858
			public SexyColor mNameColor;

			// Token: 0x04000743 RID: 1859
			public bool mAdvMode;

			// Token: 0x04000744 RID: 1860
			public bool mAlwaysShow;

			// Token: 0x04000745 RID: 1861
			public float mImgAlpha;

			// Token: 0x04000746 RID: 1862
			public bool mDoPolaroid;

			// Token: 0x04000747 RID: 1863
			public bool mXFlip;
		}
	}
}
