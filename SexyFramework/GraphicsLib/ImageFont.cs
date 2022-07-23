﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SexyFramework.Misc;
using SexyFramework.Resource;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000D4 RID: 212
	public class ImageFont : Font
	{
		// Token: 0x0600065F RID: 1631 RVA: 0x00019692 File Offset: 0x00017892
		public static void EnableAlphaCorrection()
		{
			ImageFont.EnableAlphaCorrection(true);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0001969A File Offset: 0x0001789A
		public static void EnableAlphaCorrection(bool alphaCorrect)
		{
			ImageFont.mAlphaCorrectionEnabled = alphaCorrect;
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x000196A2 File Offset: 0x000178A2
		public static void SetOrderedHashing()
		{
			ImageFont.SetOrderedHashing(true);
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x000196AA File Offset: 0x000178AA
		public static void SetOrderedHashing(bool orderedHash)
		{
			ImageFont.mOrderedHash = orderedHash;
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x000196B2 File Offset: 0x000178B2
		public static bool CheckCache(string theSrcFile, string theAltData)
		{
			return false;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x000196B5 File Offset: 0x000178B5
		public static bool SetCacheUpToDate(string theSrcFile, string theAltData)
		{
			return false;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x000196B8 File Offset: 0x000178B8
		public static ImageFont ReadFromCache(string theSrcFile, string theAltData)
		{
			return null;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x000196BC File Offset: 0x000178BC
		private ImageFont()
		{
			new AutoCrit(GlobalMembers.gSexyAppBase.mImageSetCritSect);
			GlobalMembers.gSexyAppBase.mImageFontSet.Add(this);
			this.mFontImage = null;
			this.mScale = 1.0;
			this.mWantAlphaCorrection = false;
			this.mFontData = new FontData();
			this.mFontData.Ref();
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00019744 File Offset: 0x00017944
		public ImageFont(string fileName, byte[] buffer)
		{
			this.mFontImage = null;
			this.mScale = 1.0;
			this.mWantAlphaCorrection = false;
			this.mFontData = new FontData();
			this.mFontData.Ref();
			if (!false)
			{
				this.mFontData.Load(buffer);
				this.mPointSize = this.mFontData.mDefaultPointSize;
				this.mActivateAllLayers = false;
				this.mActiveListValid = true;
				this.mForceScaledImagesWhite = false;
				this.GenerateActiveFontLayers();
			}
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x000197EC File Offset: 0x000179EC
		public ImageFont(SexyAppBase theSexyApp, string theFontDescFileName, string theImagePathPrefix)
		{
			new AutoCrit(GlobalMembers.gSexyAppBase.mImageSetCritSect);
			GlobalMembers.gSexyAppBase.mImageFontSet.Add(this);
			this.mFontImage = null;
			this.mScale = 1.0;
			this.mWantAlphaCorrection = false;
			this.mFontData = new FontData();
			this.mFontData.Ref();
			this.mFontData.mImagePathPrefix = theImagePathPrefix;
			string text = theFontDescFileName + ".cfw2";
			string text2 = "cached\\" + text;
			string theFileName = text2;
			SexyBuffer buffer = new SexyBuffer();
			bool flag = false;
			if (theSexyApp.ReadBufferFromStream(text, ref buffer) && buffer.GetDataLen() >= 16)
			{
				flag = true;
			}
			else if (theSexyApp.ReadBufferFromStream(text2, ref buffer) && buffer.GetDataLen() >= 16)
			{
				flag = true;
			}
			else if (theSexyApp.ReadBufferFromStream(theFileName, ref buffer) && buffer.GetDataLen() >= 16)
			{
				flag = true;
			}
			bool flag2 = false;
			if (flag)
			{
				if (theSexyApp.mResStreamsManager != null && theSexyApp.mResStreamsManager.IsInitialized())
				{
					flag2 = true;
				}
				else
				{
					this.SerializeRead(buffer.GetDataPtr(), buffer.GetDataLen() - 16, 16);
					flag2 = true;
				}
			}
			if (!flag2)
			{
				this.mFontData.Load(theSexyApp, theFontDescFileName);
				this.mPointSize = this.mFontData.mDefaultPointSize;
				this.mActivateAllLayers = false;
				this.GenerateActiveFontLayers();
				this.mActiveListValid = true;
				this.mForceScaledImagesWhite = false;
				bool mWriteFontCacheDir = theSexyApp.mWriteFontCacheDir;
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00019970 File Offset: 0x00017B70
		public ImageFont(Image theFontImage)
		{
			new AutoCrit(GlobalMembers.gSexyAppBase.mImageSetCritSect);
			GlobalMembers.gSexyAppBase.mImageFontSet.Add(this);
			this.mScale = 1.0;
			this.mWantAlphaCorrection = false;
			this.mFontData = new FontData();
			this.mFontData.Ref();
			this.mFontData.mInitialized = true;
			this.mPointSize = this.mFontData.mDefaultPointSize;
			this.mActiveListValid = false;
			this.mForceScaledImagesWhite = false;
			this.mActivateAllLayers = false;
			this.mFontData.mFontLayerList.AddLast(new FontLayer(this.mFontData));
			FontLayer value = this.mFontData.mFontLayerList.Last.Value;
			this.mFontData.mFontLayerMap.Add("", value);
			this.mFontImage = (MemoryImage)theFontImage;
			value.mImage.mUnsharedImage = this.mFontImage;
			value.mDefaultHeight = value.mImage.GetImage().GetHeight();
			value.mAscent = value.mImage.GetImage().GetHeight();
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00019AB4 File Offset: 0x00017CB4
		public ImageFont(ImageFont theImageFont)
			: base(theImageFont)
		{
			this.mScale = theImageFont.mScale;
			this.mFontData = theImageFont.mFontData;
			this.mPointSize = theImageFont.mPointSize;
			this.mTagVector = theImageFont.mTagVector;
			this.mActiveListValid = theImageFont.mActiveListValid;
			this.mForceScaledImagesWhite = theImageFont.mForceScaledImagesWhite;
			this.mWantAlphaCorrection = theImageFont.mWantAlphaCorrection;
			this.mActivateAllLayers = theImageFont.mActivateAllLayers;
			this.mFontImage = theImageFont.mFontImage;
			new AutoCrit(GlobalMembers.gSexyAppBase.mImageSetCritSect);
			GlobalMembers.gSexyAppBase.mImageFontSet.Add(this);
			this.mFontData.Ref();
			if (this.mActiveListValid)
			{
				this.mActiveLayerList = theImageFont.mActiveLayerList;
			}
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00019B94 File Offset: 0x00017D94
		public ImageFont(Image theFontImage, string theFontDescFileName)
		{
			new AutoCrit(GlobalMembers.gSexyAppBase.mImageSetCritSect);
			GlobalMembers.gSexyAppBase.mImageFontSet.Add(this);
			this.mScale = 1.0;
			this.mFontImage = null;
			this.mFontData = new FontData();
			this.mFontData.Ref();
			this.mFontData.LoadLegacy(theFontImage, theFontDescFileName);
			this.mPointSize = this.mFontData.mDefaultPointSize;
			this.mActivateAllLayers = false;
			this.GenerateActiveFontLayers();
			this.mActiveListValid = true;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00019C47 File Offset: 0x00017E47
		public override void Dispose()
		{
			new AutoCrit(GlobalMembers.gSexyAppBase.mImageSetCritSect);
			GlobalMembers.gSexyAppBase.mImageFontSet.Remove(this);
			this.mFontData.DeRef();
			base.Dispose();
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00019C7B File Offset: 0x00017E7B
		public override Font Duplicate()
		{
			return new ImageFont(this);
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00019C84 File Offset: 0x00017E84
		public virtual void GenerateActiveFontLayers()
		{
			if (!this.mFontData.mInitialized)
			{
				return;
			}
			this.mActiveLayerList.Clear();
			this.mAscent = 0;
			this.mAscentPadding = 0;
			this.mHeight = 0;
			this.mLineSpacingOffset = 0;
			LinkedList<FontLayer>.Enumerator enumerator = this.mFontData.mFontLayerList.GetEnumerator();
			bool flag = true;
			while (enumerator.MoveNext())
			{
				FontLayer fontLayer = enumerator.Current;
				if (this.mPointSize >= fontLayer.mMinPointSize && (this.mPointSize <= fontLayer.mMaxPointSize || fontLayer.mMaxPointSize == -1))
				{
					bool flag2 = true;
					for (int i = 0; i < fontLayer.mRequiredTags.Count; i++)
					{
						if (this.mTagVector.IndexOf(fontLayer.mRequiredTags[i]) == -1)
						{
							flag2 = false;
						}
					}
					for (int i = 0; i < this.mTagVector.Count; i++)
					{
						if (fontLayer.mExcludedTags.IndexOf(this.mTagVector[i]) != -1)
						{
							flag2 = false;
						}
					}
					flag2 |= this.mActivateAllLayers;
					if (flag2)
					{
						this.mActiveLayerList.Add(new ActiveFontLayer());
						ActiveFontLayer activeFontLayer = Enumerable.Last<ActiveFontLayer>(this.mActiveLayerList);
						activeFontLayer.mBaseFontLayer = fontLayer;
						activeFontLayer.mUseAlphaCorrection = this.mWantAlphaCorrection & fontLayer.mImageIsWhite;
						double num = 1.0;
						double num2 = this.mScale;
						if (this.mScale == 1.0 && (fontLayer.mPointSize == 0 || this.mPointSize == fontLayer.mPointSize))
						{
							activeFontLayer.mScaledImages[7] = fontLayer.mImage;
							if (this.mFontImage != null)
							{
								activeFontLayer.mScaledImages[7].mUnsharedImage = this.mFontImage;
							}
							int num3 = fontLayer.mCharDataHashTable.CharCount();
							CharData[] array = fontLayer.mCharDataHashTable.ToArray();
							for (int j = 0; j < num3; j++)
							{
								activeFontLayer.mScaledCharImageRects.Add((char)array[j].mChar, array[j].mImageRect);
							}
						}
						else
						{
							if (fontLayer.mPointSize != 0)
							{
								num = (double)fontLayer.mPointSize;
								num2 = (double)this.mPointSize * this.mScale;
							}
							MemoryImage memoryImage = new MemoryImage();
							int num4 = 0;
							bool flag3 = true;
							int num5 = 0;
							int num6 = 0;
							int num7 = fontLayer.mCharDataHashTable.CharCount();
							CharData[] array2 = fontLayer.mCharDataHashTable.ToArray();
							for (int k = 0; k < num7; k++)
							{
								Rect mImageRect = array2[k].mImageRect;
								int mY = array2[k].mOffset.mY;
								int num8 = mY + mImageRect.mHeight;
								num5 = Math.Min(mY, num5);
								num6 = Math.Max(num8, num6);
								if (num5 != mY || num6 != num8)
								{
									flag3 = false;
								}
								num4 += mImageRect.mWidth + 2;
							}
							if (!flag3)
							{
								MemoryImage memoryImage2 = new MemoryImage();
								memoryImage2.Create(num4, num6 - num5);
								Graphics graphics = new Graphics(memoryImage2);
								num4 = 0;
								num7 = fontLayer.mCharDataHashTable.CharCount();
								array2 = fontLayer.mCharDataHashTable.ToArray();
								for (int l = 0; l < num7; l++)
								{
									Rect mImageRect2 = array2[l].mImageRect;
									if (fontLayer.mImage.GetImage() != null)
									{
										graphics.DrawImage(fontLayer.mImage.GetImage(), num4, array2[l].mOffset.mY - num5, mImageRect2);
									}
									array2[l].mOffset.mY = num5;
									array2[l].mOffset.mX--;
									mImageRect2 = new Rect(num4, 0, mImageRect2.mWidth + 2, num6 - num5);
									num4 += mImageRect2.mWidth;
								}
								fontLayer.mImage.mUnsharedImage = memoryImage2;
								fontLayer.mImage.mOwnsUnshared = true;
								graphics.ClearRenderContext();
							}
							num4 = 0;
							int num9 = 0;
							num7 = fontLayer.mCharDataHashTable.CharCount();
							array2 = fontLayer.mCharDataHashTable.ToArray();
							for (int m = 0; m < num7; m++)
							{
								Rect mImageRect3 = array2[m].mImageRect;
								int num10 = (int)Math.Floor((double)array2[m].mOffset.mX * num2 / (double)((float)num));
								int num11 = (int)Math.Ceiling((double)(array2[m].mOffset.mX + mImageRect3.mWidth) * num2 / (double)((float)num));
								int theWidth = Math.Max(0, num11 - num10 - 1);
								int num12 = (int)Math.Floor((double)array2[m].mOffset.mY * num2 / (double)((float)num));
								int num13 = (int)Math.Ceiling((double)(array2[m].mOffset.mY + mImageRect3.mHeight) * num2 / (double)((float)num));
								int theHeight = Math.Max(0, num13 - num12 - 1);
								Rect rect = new Rect(num4, 0, theWidth, theHeight);
								if (rect.mHeight > num9)
								{
									num9 = rect.mHeight;
								}
								activeFontLayer.mScaledCharImageRects.Add((char)array2[m].mChar, rect);
								num4 += rect.mWidth;
							}
							activeFontLayer.mScaledImages[7].mUnsharedImage = memoryImage;
							activeFontLayer.mScaledImages[7].mOwnsUnshared = true;
							memoryImage.Create(num4, num9);
							Graphics graphics2 = new Graphics(memoryImage);
							num7 = fontLayer.mCharDataHashTable.CharCount();
							array2 = fontLayer.mCharDataHashTable.ToArray();
							for (int n = 0; n < num7; n++)
							{
								if (fontLayer.mImage.GetImage() != null)
								{
									graphics2.DrawImage(fontLayer.mImage.GetImage(), activeFontLayer.mScaledCharImageRects[(char)array2[n].mChar], array2[n].mImageRect);
								}
							}
							if (this.mForceScaledImagesWhite)
							{
								int num14 = memoryImage.mWidth * memoryImage.mHeight;
								uint[] bits = memoryImage.GetBits();
								for (int num15 = 0; num15 < num14; num15++)
								{
									bits[num15] |= 16777215U;
								}
							}
							memoryImage.AddImageFlags(128U);
							memoryImage.Palletize();
							graphics2.ClearRenderContext();
						}
						int num16 = (((double)fontLayer.mAscent * num2 / (double)((float)num) >= 0.0) ? ((int)((double)fontLayer.mAscent * num2 / (double)((float)num) + 0.501)) : ((int)((double)fontLayer.mAscent * num2 / (double)((float)num) - 0.501)));
						if (num16 > this.mAscent)
						{
							this.mAscent = num16;
						}
						if (fontLayer.mHeight != 0)
						{
							int num17 = (((double)fontLayer.mHeight * num2 / (double)((float)num) >= 0.0) ? ((int)((double)fontLayer.mHeight * num2 / (double)((float)num) + 0.501)) : ((int)((double)fontLayer.mHeight * num2 / (double)((float)num) - 0.501)));
							if (num17 > this.mHeight)
							{
								this.mHeight = num17;
							}
						}
						else
						{
							int num18 = (((double)fontLayer.mDefaultHeight * num2 / (double)((float)num) >= 0.0) ? ((int)((double)fontLayer.mDefaultHeight * num2 / (double)((float)num) + 0.501)) : ((int)((double)fontLayer.mDefaultHeight * num2 / (double)((float)num) - 0.501)));
							if (num18 > this.mHeight)
							{
								this.mHeight = num18;
							}
						}
						int num19 = (((double)fontLayer.mAscentPadding * num2 / (double)((float)num) >= 0.0) ? ((int)((double)fontLayer.mAscentPadding * num2 / (double)((float)num) + 0.501)) : ((int)((double)fontLayer.mAscentPadding * num2 / (double)((float)num) - 0.501)));
						if (flag || num19 < this.mAscentPadding)
						{
							this.mAscentPadding = num19;
						}
						int num20 = (((double)fontLayer.mLineSpacingOffset * num2 / (double)((float)num) >= 0.0) ? ((int)((double)fontLayer.mLineSpacingOffset * num2 / (double)((float)num) + 0.501)) : ((int)((double)fontLayer.mLineSpacingOffset * num2 / (double)((float)num) - 0.501)));
						if (flag || num20 > this.mLineSpacingOffset)
						{
							this.mLineSpacingOffset = num20;
						}
					}
					flag = false;
				}
			}
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0001A484 File Offset: 0x00018684
		public virtual void DrawStringEx(Graphics g, int theX, int theY, string theString, SexyColor theColor, Rect theClipRect, LinkedList<Rect> theDrawnAreas, ref int theWidth)
		{
			new AutoCrit(GlobalMembers.gSexyAppBase.mImageSetCritSect);
			if (theDrawnAreas != null)
			{
				theDrawnAreas.Clear();
			}
			if (!this.mFontData.mInitialized)
			{
				theWidth = 0;
				return;
			}
			this.Prepare();
			bool colorizeImages = g.GetColorizeImages();
			g.SetColorizeImages(true);
			int num = theX;
			int num2 = 0;
			for (int i = 0; i < theString.Length; i++)
			{
				char mappedChar = this.GetMappedChar(theString[i]);
				char c = '\0';
				if (i < theString.Length - 1)
				{
					c = this.GetMappedChar(theString[i + 1]);
				}
				int num3 = num;
				for (int j = 0; j < this.mActiveLayerList.Count; j++)
				{
					ActiveFontLayer activeFontLayer = this.mActiveLayerList[j];
					CharData charData = activeFontLayer.mBaseFontLayer.GetCharData(mappedChar);
					int num4 = num;
					int num5 = activeFontLayer.mBaseFontLayer.mPointSize;
					double num6 = this.mScale;
					if (num5 != 0)
					{
						num6 *= (double)this.mPointSize / (double)num5;
					}
					int num7;
					int num8;
					int num9;
					int num10;
					if (num6 == 1.0)
					{
						num7 = num4 + activeFontLayer.mBaseFontLayer.mOffset.mX + charData.mOffset.mX;
						num8 = theY - (activeFontLayer.mBaseFontLayer.mAscent - activeFontLayer.mBaseFontLayer.mOffset.mY - charData.mOffset.mY);
						num9 = charData.mWidth;
						if (c != '\0')
						{
							num10 = activeFontLayer.mBaseFontLayer.mSpacing;
							if (charData.mKerningCount != 0)
							{
								int mKerningCount = (int)charData.mKerningCount;
								for (int k = 0; k < mKerningCount; k++)
								{
									ushort mChar = activeFontLayer.mBaseFontLayer.mKerningData[k].mChar;
								}
							}
						}
						else
						{
							num10 = 0;
						}
					}
					else
					{
						num7 = num4 + (int)Math.Floor((double)(activeFontLayer.mBaseFontLayer.mOffset.mX + charData.mOffset.mX) * num6);
						num8 = theY - (int)Math.Floor((double)(activeFontLayer.mBaseFontLayer.mAscent - activeFontLayer.mBaseFontLayer.mOffset.mY - charData.mOffset.mY) * num6);
						num9 = (int)((double)charData.mWidth * num6);
						if (c != '\0')
						{
							num10 = activeFontLayer.mBaseFontLayer.mSpacing;
							if (charData.mKerningCount != 0)
							{
								int mKerningCount2 = (int)charData.mKerningCount;
								for (int l = 0; l < mKerningCount2; l++)
								{
									ushort mChar2 = activeFontLayer.mBaseFontLayer.mKerningData[l].mChar;
								}
							}
						}
						else
						{
							num10 = 0;
						}
					}
					SexyColor mColor = default(SexyColor);
					if (activeFontLayer.mColorStack.Count == 0)
					{
						mColor.mRed = Math.Min(theColor.mRed * activeFontLayer.mBaseFontLayer.mColorMult.mRed / 255 + activeFontLayer.mBaseFontLayer.mColorAdd.mRed, 255);
						mColor.mGreen = Math.Min(theColor.mGreen * activeFontLayer.mBaseFontLayer.mColorMult.mGreen / 255 + activeFontLayer.mBaseFontLayer.mColorAdd.mGreen, 255);
						mColor.mBlue = Math.Min(theColor.mBlue * activeFontLayer.mBaseFontLayer.mColorMult.mBlue / 255 + activeFontLayer.mBaseFontLayer.mColorAdd.mBlue, 255);
						mColor.mAlpha = Math.Min(theColor.mAlpha * activeFontLayer.mBaseFontLayer.mColorMult.mAlpha / 255 + activeFontLayer.mBaseFontLayer.mColorAdd.mAlpha, 255);
					}
					else
					{
						SexyColor color = activeFontLayer.mColorStack[activeFontLayer.mColorStack.Count - 1];
						mColor.mRed = Math.Min(theColor.mRed * activeFontLayer.mBaseFontLayer.mColorMult.mRed * color.mRed / 65025 + activeFontLayer.mBaseFontLayer.mColorAdd.mRed * color.mRed / 255, 255);
						mColor.mGreen = Math.Min(theColor.mGreen * activeFontLayer.mBaseFontLayer.mColorMult.mGreen * color.mGreen / 65025 + activeFontLayer.mBaseFontLayer.mColorAdd.mGreen * color.mGreen / 255, 255);
						mColor.mBlue = Math.Min(theColor.mBlue * activeFontLayer.mBaseFontLayer.mColorMult.mBlue * color.mBlue / 65025 + activeFontLayer.mBaseFontLayer.mColorAdd.mBlue * color.mBlue / 255, 255);
						mColor.mAlpha = Math.Min(theColor.mAlpha * activeFontLayer.mBaseFontLayer.mColorMult.mAlpha * color.mAlpha / 65025 + activeFontLayer.mBaseFontLayer.mColorAdd.mAlpha * color.mAlpha / 255, 255);
					}
					int num11 = activeFontLayer.mBaseFontLayer.mBaseOrder + charData.mOrder;
					if (num2 >= 1024)
					{
						break;
					}
					RenderCommand renderCommand = GlobalImageFont.GetRenderCommandPool()[num2++];
					renderCommand.mFontLayer = activeFontLayer;
					renderCommand.mColor = mColor;
					renderCommand.mDest[0] = num7;
					renderCommand.mDest[1] = num8;
					Rect rect = activeFontLayer.mScaledCharImageRects[mappedChar];
					renderCommand.mSrc[0] = rect.mX;
					renderCommand.mSrc[1] = rect.mY;
					renderCommand.mSrc[2] = rect.mWidth;
					renderCommand.mSrc[3] = rect.mHeight;
					renderCommand.mMode = activeFontLayer.mBaseFontLayer.mDrawMode;
					int orderedZ = Math.Min(Math.Max(num11 + 128, 0), 255);
					GlobalImageFont.AddRenderCommand(renderCommand, orderedZ);
					if (theDrawnAreas != null)
					{
						Rect rect2 = new Rect(num7, num8, rect.mWidth, rect.mHeight);
						theDrawnAreas.AddLast(rect2);
					}
					num4 += num9 + num10;
					if (num4 > num3)
					{
						num3 = num4;
					}
				}
				num = num3;
			}
			theWidth = num - theX;
			SexyColor color2 = g.GetColor();
			GlobalImageFont.DrawAllRenderCommand(g, ImageFont.mAlphaCorrectionEnabled);
			GlobalImageFont.ClearRenderCommand();
			g.SetColor(color2);
			g.SetColorizeImages(colorizeImages);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0001AB18 File Offset: 0x00018D18
		public char GetMappedChar(char theChar)
		{
			char result;
			if (this.mFontData.mCharMap.TryGetValue(theChar, out result))
			{
				return result;
			}
			return theChar;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0001AB3D File Offset: 0x00018D3D
		public override ImageFont AsImageFont()
		{
			return this;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0001AB40 File Offset: 0x00018D40
		public override int CharWidth(char theChar)
		{
			return this.CharWidthKern(theChar, '\0');
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001AB4C File Offset: 0x00018D4C
		public override int CharWidthKern(char theChar, char thePrevChar)
		{
			this.Prepare();
			int num = 0;
			double num2 = (double)this.mPointSize * this.mScale;
			theChar = this.GetMappedChar(theChar);
			if (thePrevChar != '\0')
			{
				thePrevChar = this.GetMappedChar(thePrevChar);
			}
			Dictionary<char, int> dictionary;
			if (this.mCachedCharPair.TryGetValue(theChar, out dictionary))
			{
				if (dictionary.TryGetValue(thePrevChar, out num))
				{
					return num;
				}
			}
			else
			{
				this.mCachedCharPair.Add(theChar, new Dictionary<char, int>());
			}
			for (int i = 0; i < this.mActiveLayerList.Count; i++)
			{
				ActiveFontLayer activeFontLayer = this.mActiveLayerList[i];
				CharData charData = activeFontLayer.mBaseFontLayer.GetCharData(theChar);
				int num3 = 0;
				int num4 = activeFontLayer.mBaseFontLayer.mPointSize;
				int num5;
				int num6;
				if (num4 == 0)
				{
					num5 = (((double)charData.mWidth * this.mScale >= 0.0) ? ((int)((double)charData.mWidth * this.mScale + 0.501)) : ((int)((double)charData.mWidth * this.mScale - 0.501)));
					if (thePrevChar != '\0')
					{
						num6 = activeFontLayer.mBaseFontLayer.mSpacing;
						CharData charData2 = activeFontLayer.mBaseFontLayer.GetCharData(thePrevChar);
						if (charData2.mKerningCount != 0)
						{
							int mKerningCount = (int)charData2.mKerningCount;
							for (int j = 0; j < mKerningCount; j++)
							{
								ushort mChar = activeFontLayer.mBaseFontLayer.mKerningData[j].mChar;
							}
						}
					}
					else
					{
						num6 = 0;
					}
				}
				else
				{
					num5 = (((double)charData.mWidth * num2 / (double)((float)num4) >= 0.0) ? ((int)((double)charData.mWidth * num2 / (double)((float)num4) + 0.501)) : ((int)((double)charData.mWidth * num2 / (double)((float)num4) - 0.501)));
					if (thePrevChar != '\0')
					{
						num6 = activeFontLayer.mBaseFontLayer.mSpacing;
						CharData charData3 = activeFontLayer.mBaseFontLayer.GetCharData(thePrevChar);
						if (charData3.mKerningCount != 0)
						{
							int mKerningCount2 = (int)charData3.mKerningCount;
							for (int k = 0; k < mKerningCount2; k++)
							{
								ushort mChar2 = activeFontLayer.mBaseFontLayer.mKerningData[k].mChar;
							}
						}
					}
					else
					{
						num6 = 0;
					}
				}
				num3 += num5 + num6;
				if (num3 > num)
				{
					num = num3;
				}
			}
			this.mCachedCharPair[theChar].Add(thePrevChar, num);
			return num;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001ADA8 File Offset: 0x00018FA8
		public override int StringWidth(string theString)
		{
			int num = 0;
			char thePrevChar = '\0';
			for (int i = 0; i < theString.Length; i++)
			{
				char c = theString[i];
				num += this.CharWidthKern(c, thePrevChar);
				thePrevChar = c;
			}
			return num;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001ADE0 File Offset: 0x00018FE0
		public override void DrawString(Graphics g, int theX, int theY, string theString, SexyColor theColor, Rect theClipRect)
		{
			int num = 0;
			this.DrawStringEx(g, theX, theY, theString, theColor, theClipRect, null, ref num);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001AE01 File Offset: 0x00019001
		public virtual void SetPointSize(int thePointSize)
		{
			this.mPointSize = thePointSize;
			this.mActiveListValid = false;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001AE11 File Offset: 0x00019011
		public virtual int GetPointSize()
		{
			return this.mPointSize;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001AE19 File Offset: 0x00019019
		public virtual void SetScale(double theScale)
		{
			this.mScale = theScale;
			this.mActiveListValid = false;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001AE29 File Offset: 0x00019029
		public virtual int GetDefaultPointSize()
		{
			return this.mFontData.mDefaultPointSize;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001AE38 File Offset: 0x00019038
		public virtual bool AddTag(string theTagName)
		{
			if (this.HasTag(theTagName))
			{
				return false;
			}
			string text = theTagName.ToUpper();
			this.mTagVector.Add(text);
			this.mActiveListValid = false;
			return true;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0001AE6C File Offset: 0x0001906C
		public virtual bool RemoveTag(string theTagName)
		{
			string text = theTagName.ToUpper();
			if (this.mTagVector.Remove(text))
			{
				this.mActiveListValid = false;
				return true;
			}
			return false;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001AE98 File Offset: 0x00019098
		public virtual bool HasTag(string theTagName)
		{
			return this.mTagVector.Contains(theTagName);
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0001AEA8 File Offset: 0x000190A8
		public virtual string GetDefine(string theName)
		{
			DataElement dataElement = this.mFontData.Dereference(theName);
			if (dataElement == null)
			{
				return "";
			}
			return this.mFontData.DataElementToString(dataElement, true);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0001AED8 File Offset: 0x000190D8
		public virtual void Prepare()
		{
			if (!this.mActiveListValid)
			{
				this.GenerateActiveFontLayers();
				this.mActiveListValid = true;
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0001AEEF File Offset: 0x000190EF
		public virtual void WriteToCache(string theSrcFile, string theAltData)
		{
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0001AEF4 File Offset: 0x000190F4
		public string SerializeReadStr(byte[] thePtr, int theStartIndex, int size)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < size; i++)
			{
				stringBuilder.Append((char)thePtr[theStartIndex + i]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0001AF28 File Offset: 0x00019128
		public bool SerializeRead(byte[] thePtr, int theSize, int theStartIndex)
		{
			if (thePtr == null)
			{
				return false;
			}
			bool result = false;
			this.mAscent = BitConverter.ToInt32(thePtr, theStartIndex);
			int num = theStartIndex + 4;
			this.mAscentPadding = BitConverter.ToInt32(thePtr, num);
			num += 4;
			this.mHeight = BitConverter.ToInt32(thePtr, num);
			num += 4;
			this.mLineSpacingOffset = BitConverter.ToInt32(thePtr, num);
			num += 4;
			this.mFontData.mApp = GlobalMembers.gSexyAppBase;
			this.mFontData.mInitialized = BitConverter.ToBoolean(thePtr, num);
			num++;
			this.mFontData.mDefaultPointSize = BitConverter.ToInt32(thePtr, num);
			num += 4;
			int num2 = BitConverter.ToInt32(thePtr, num);
			num += 4;
			for (int i = 0; i < num2; i++)
			{
				ushort num3 = BitConverter.ToUInt16(thePtr, num);
				num += 2;
				ushort num4 = BitConverter.ToUInt16(thePtr, num);
				num += 2;
				this.mFontData.mCharMap.Add((char)num3, (char)num4);
			}
			int num5 = BitConverter.ToInt32(thePtr, num);
			num += 4;
			for (int j = 0; j < num5; j++)
			{
				this.mFontData.mFontLayerList.AddLast(new FontLayer(this.mFontData));
				FontLayer value = this.mFontData.mFontLayerList.Last.Value;
				int num6 = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mLayerName = this.SerializeReadStr(thePtr, num, num6);
				num += num6;
				this.mFontData.mFontLayerMap.Add(value.mLayerName, value);
				int num7 = BitConverter.ToInt32(thePtr, num);
				num += 4;
				for (int k = 0; k < num7; k++)
				{
					int num8 = BitConverter.ToInt32(thePtr, num);
					num += 4;
					string text = this.SerializeReadStr(thePtr, num, num8);
					num += num8;
					value.mRequiredTags.Add(text);
				}
				num7 = BitConverter.ToInt32(thePtr, num);
				num += 4;
				for (int l = 0; l < num7; l++)
				{
					int num9 = BitConverter.ToInt32(thePtr, num);
					num += 4;
					string text2 = this.SerializeReadStr(thePtr, num, num9);
					num += num9;
					value.mExcludedTags.Add(text2);
				}
				int num10 = BitConverter.ToInt32(thePtr, num);
				num += 4;
				if (num10 != 0)
				{
					value.mKerningData.Clear();
					for (int m = 0; m < num10; m++)
					{
						FontLayer.KerningValue kerningValue = default(FontLayer.KerningValue);
						kerningValue.mInt = BitConverter.ToInt32(thePtr, num);
						num += 4;
						kerningValue.mChar = (ushort)((kerningValue.mInt >> 16) & 255);
						kerningValue.mOffset = (short)(kerningValue.mInt & 255);
						value.mKerningData.Add(kerningValue);
					}
				}
				int num11 = BitConverter.ToInt32(thePtr, num);
				num += 4;
				for (int n = 0; n < num11; n++)
				{
					ushort inChar = BitConverter.ToUInt16(thePtr, num);
					num += 2;
					CharData charData = value.mCharDataHashTable.GetCharData((char)inChar, true);
					charData.mImageRect.mX = BitConverter.ToInt32(thePtr, num);
					num += 4;
					charData.mImageRect.mY = BitConverter.ToInt32(thePtr, num);
					num += 4;
					charData.mImageRect.mWidth = BitConverter.ToInt32(thePtr, num);
					num += 4;
					charData.mImageRect.mHeight = BitConverter.ToInt32(thePtr, num);
					num += 4;
					charData.mOffset.mX = BitConverter.ToInt32(thePtr, num);
					num += 4;
					charData.mOffset.mY = BitConverter.ToInt32(thePtr, num);
					num += 4;
					charData.mKerningFirst = BitConverter.ToUInt16(thePtr, num);
					num += 2;
					charData.mKerningCount = BitConverter.ToUInt16(thePtr, num);
					num += 2;
					charData.mWidth = BitConverter.ToInt32(thePtr, num);
					num += 4;
					charData.mOrder = BitConverter.ToInt32(thePtr, num);
					num += 4;
				}
				value.mColorMult.mRed = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mColorMult.mGreen = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mColorMult.mBlue = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mColorMult.mAlpha = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mColorAdd.mRed = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mColorAdd.mGreen = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mColorAdd.mBlue = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mColorAdd.mAlpha = BitConverter.ToInt32(thePtr, num);
				num += 4;
				int num12 = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mImageFileName = this.SerializeReadStr(thePtr, num, num12);
				num += num12;
				bool flag = false;
				SharedImageRef sharedImageRef = new SharedImageRef();
				if (GlobalMembers.gSexyAppBase.mResourceManager != null && string.IsNullOrEmpty(this.mFontData.mImagePathPrefix))
				{
					string idByPath = GlobalMembers.gSexyAppBase.mResourceManager.GetIdByPath(value.mImageFileName);
					if (!string.IsNullOrEmpty(idByPath))
					{
						sharedImageRef = GlobalMembers.gSexyAppBase.mResourceManager.GetImage(idByPath);
						if (sharedImageRef.GetDeviceImage() == null)
						{
							sharedImageRef = GlobalMembers.gSexyAppBase.mResourceManager.LoadImage(idByPath);
						}
						if (sharedImageRef.GetDeviceImage() != null)
						{
							flag = true;
						}
					}
				}
				if (!flag)
				{
					sharedImageRef = GlobalMembers.gSexyAppBase.GetSharedImage(this.mFontData.mImagePathPrefix + value.mImageFileName);
				}
				value.mImage = new SharedImageRef(sharedImageRef);
				if (value.mImage.GetDeviceImage() == null)
				{
					result = true;
				}
				value.mDrawMode = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mOffset.mX = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mOffset.mY = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mSpacing = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mMinPointSize = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mMaxPointSize = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mPointSize = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mAscent = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mAscentPadding = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mHeight = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mDefaultHeight = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mLineSpacingOffset = BitConverter.ToInt32(thePtr, num);
				num += 4;
				value.mBaseOrder = BitConverter.ToInt32(thePtr, num);
				num += 4;
			}
			int num13 = BitConverter.ToInt32(thePtr, num);
			num += 4;
			this.mFontData.mSourceFile = this.SerializeReadStr(thePtr, num, num13);
			num += num13;
			int num14 = BitConverter.ToInt32(thePtr, num);
			num += 4;
			this.mFontData.mFontErrorHeader = this.SerializeReadStr(thePtr, num, num14);
			num += num14;
			this.mPointSize = BitConverter.ToInt32(thePtr, num);
			num += 4;
			int num15 = BitConverter.ToInt32(thePtr, num);
			num += 4;
			for (int num16 = 0; num16 < num15; num16++)
			{
				int num17 = BitConverter.ToInt32(thePtr, num);
				num += 4;
				string text3 = this.SerializeReadStr(thePtr, num, num17);
				num += num17;
				this.mTagVector.Add(text3);
			}
			this.mScale = BitConverter.ToDouble(thePtr, num);
			num += 8;
			this.mForceScaledImagesWhite = BitConverter.ToBoolean(thePtr, num);
			num++;
			this.mActivateAllLayers = BitConverter.ToBoolean(thePtr, num);
			num++;
			this.mActiveListValid = false;
			return result;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0001B63F File Offset: 0x0001983F
		public bool SerializeReadEndian(IntPtr thePtr, int theSize)
		{
			return false;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0001B642 File Offset: 0x00019842
		public bool SerializeWrite(IntPtr thePtr)
		{
			return this.SerializeWrite(thePtr, 0);
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001B64C File Offset: 0x0001984C
		public bool SerializeWrite(IntPtr thePtr, int theSizeIfKnown)
		{
			return false;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001B650 File Offset: 0x00019850
		public int GetLayerCount()
		{
			LinkedList<FontLayer>.Enumerator enumerator = this.mFontData.mFontLayerList.GetEnumerator();
			int num = 0;
			while (enumerator.MoveNext())
			{
				FontLayer fontLayer = enumerator.Current;
				if (fontLayer.mLayerName.Length < 6 || fontLayer.mLayerName.Substring(fontLayer.mLayerName.Length - 5) != "__MOD")
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0001B6BC File Offset: 0x000198BC
		public void PushLayerColor(string theLayerName, SexyColor theColor)
		{
			this.Prepare();
			string text = theLayerName + "__MOD";
			for (int i = 0; i < this.mActiveLayerList.Count; i++)
			{
				ActiveFontLayer activeFontLayer = this.mActiveLayerList[i];
				if (activeFontLayer.mBaseFontLayer.mLayerName.ToLower() == theLayerName.ToLower() || activeFontLayer.mBaseFontLayer.mLayerName.ToLower() == text.ToLower())
				{
					activeFontLayer.PushColor(theColor);
				}
			}
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0001B740 File Offset: 0x00019940
		public void PushLayerColor(int theLayer, SexyColor theColor)
		{
			this.Prepare();
			LinkedList<FontLayer>.Enumerator enumerator = this.mFontData.mFontLayerList.GetEnumerator();
			int num = 0;
			while (enumerator.MoveNext())
			{
				FontLayer fontLayer = enumerator.Current;
				if (fontLayer.mLayerName.Length < 6 || fontLayer.mLayerName.Substring(fontLayer.mLayerName.Length - 5) != "__MOD")
				{
					if (num == theLayer)
					{
						this.PushLayerColor(fontLayer.mLayerName, theColor);
						return;
					}
					num++;
				}
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0001B7C4 File Offset: 0x000199C4
		public void PopLayerColor(string theLayerName)
		{
			string text = theLayerName + "__MOD";
			for (int i = 0; i < this.mActiveLayerList.Count; i++)
			{
				ActiveFontLayer activeFontLayer = this.mActiveLayerList[i];
				if (activeFontLayer.mBaseFontLayer.mLayerName.ToLower() == theLayerName.ToLower() || activeFontLayer.mBaseFontLayer.mLayerName.ToLower() == text.ToLower())
				{
					activeFontLayer.PopColor();
				}
			}
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001B840 File Offset: 0x00019A40
		public void PopLayerColor(int theLayer)
		{
			LinkedList<FontLayer>.Enumerator enumerator = this.mFontData.mFontLayerList.GetEnumerator();
			int num = 0;
			while (enumerator.MoveNext())
			{
				FontLayer fontLayer = enumerator.Current;
				if (fontLayer.mLayerName.Length < 6 || fontLayer.mLayerName.Substring(fontLayer.mLayerName.Length - 5) != "__MOD")
				{
					if (num == theLayer)
					{
						this.PopLayerColor(fontLayer.mLayerName);
						return;
					}
					num++;
				}
			}
		}

		// Token: 0x04000548 RID: 1352
		public static bool mAlphaCorrectionEnabled;

		// Token: 0x04000549 RID: 1353
		public static bool mOrderedHash;

		// Token: 0x0400054A RID: 1354
		public FontData mFontData;

		// Token: 0x0400054B RID: 1355
		public int mPointSize;

		// Token: 0x0400054C RID: 1356
		public List<string> mTagVector = new List<string>();

		// Token: 0x0400054D RID: 1357
		public bool mActivateAllLayers;

		// Token: 0x0400054E RID: 1358
		public bool mActiveListValid;

		// Token: 0x0400054F RID: 1359
		public List<ActiveFontLayer> mActiveLayerList = new List<ActiveFontLayer>();

		// Token: 0x04000550 RID: 1360
		public double mScale;

		// Token: 0x04000551 RID: 1361
		public bool mForceScaledImagesWhite;

		// Token: 0x04000552 RID: 1362
		public bool mWantAlphaCorrection;

		// Token: 0x04000553 RID: 1363
		public MemoryImage mFontImage;

		// Token: 0x04000554 RID: 1364
		protected Dictionary<char, Dictionary<char, int>> mCachedCharPair = new Dictionary<char, Dictionary<char, int>>();
	}
}
