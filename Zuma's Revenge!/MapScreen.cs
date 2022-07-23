using System;
using System.Collections.Generic;
using System.Linq;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000120 RID: 288
	public class MapScreen : ButtonListener, IDisposable
	{
		// Token: 0x06000EBA RID: 3770 RVA: 0x00098334 File Offset: 0x00096534
		protected bool MouseOverCard(int idx)
		{
			return this.mCards[idx].Contains(this.mLastMouseX, this.mLastMouseY) && (idx == 0 || this.mOverlays[idx - 1].mUnlocked) && Enumerable.Count<KeyValuePair<int, Dialog>>(GameApp.gApp.mDialogMap) == 0;
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x00098388 File Offset: 0x00096588
		protected void DrawDesaturatedZone(Graphics g, int theZoneId, float theDesaturationPct)
		{
			if (theDesaturationPct <= 0f)
			{
				return;
			}
			g.Get3D();
			ResID id = ResID.IMAGE_UI_MAP_JUNGLE_OVERLAY + theZoneId - 1;
			Image imageByID = Res.GetImageByID(id);
			g.SetColor(255, 255, 255, (int)((double)(255f * theDesaturationPct) * this.mAlpha));
			g.DrawImage(imageByID, Common._DS(Res.GetOffsetXByID(id)) + Common._S(0) + (int)this.mUnlockScrollAmt, Common._DS(Res.GetOffsetYByID(id)));
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x00098410 File Offset: 0x00096610
		public MapScreen()
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_MAP_OPENBOOK_PAGES);
			if (!GameApp.gApp.mResourceManager.IsGroupLoaded("CommonGame") && !GameApp.gApp.mResourceManager.LoadResources("CommonGame"))
			{
				GameApp.gApp.ShowResourceError(true);
				GameApp.gApp.Shutdown();
			}
			if (!GameApp.gApp.mResourceManager.IsGroupLoaded("Map") && !GameApp.gApp.mResourceManager.LoadResources("Map"))
			{
				GameApp.gApp.ShowResourceError(true);
				GameApp.gApp.Shutdown();
			}
			this.mParent = GlobalMembers.gSexyAppBase.mWidgetManager;
			this.mAlpha.SetConstant(1.0);
			this.mDirty = true;
			this.mUnlockScrollAmt = 0f;
			this.mHighestDot = 1;
			this.mSelectedZone = -1;
			this.mNewZoneTextSize = 1f;
			this.mNewZoneTextImg = null;
			this.mZoneEffect = null;
			this.mSlideDir = 0;
			this.mXOff = 0f;
			this.SetupClouds();
			this.mContinueBtn = null;
			this.mBackBtn = null;
			this.mSelectZoneBackBtn = null;
			this.mZoneBtn = null;
			this.mZoneOverPct = 0f;
			this.mExtrasAlpha.SetConstant(1.0);
			this.mIsTrialEnd = false;
			int num = (GameApp.gApp.mWidth - imageByID.mWidth) / 2 + Common._DS(Common._M(0)) - GameApp.gApp.mWideScreenXOffset / 2;
			int num2 = (GameApp.gApp.mHeight - imageByID.mHeight) / 2;
			int num3 = Common._DS(Common._M(200));
			for (int i = 0; i < 6; i++)
			{
				int theX = -46 + num + ((i % 2 == 0) ? Common._DS(Common._M(250)) : Common._DS(Common._M1(845)));
				int num4 = Common._DS(Common._M(290)) + 10;
				int theY = num2 + num3 + num4 * (i / 2) - 8;
				this.mCards[i] = new Rect(theX, theY, (int)(266f * Common._S(0.55f)), (int)(200f * Common._S(0.55f)));
			}
			this.mDisplayZoneAlpha = 0f;
			this.mIncDisplayZoneAlpha = true;
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x00098894 File Offset: 0x00096A94
		public virtual void Dispose()
		{
			if (this.mContinueBtn != null)
			{
				this.mParent.RemoveWidget(this.mContinueBtn);
			}
			if (this.mZoneBtn != null)
			{
				this.mParent.RemoveWidget(this.mZoneBtn);
			}
			if (this.mBackBtn != null)
			{
				this.mParent.RemoveWidget(this.mBackBtn);
			}
			if (this.mSelectZoneBackBtn != null)
			{
				this.mParent.RemoveWidget(this.mSelectZoneBackBtn);
			}
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00098908 File Offset: 0x00096B08
		public void SetupClouds()
		{
			FPoint[] array = new FPoint[]
			{
				new FPoint((float)Common._M(900), (float)Common._M1(125)),
				new FPoint((float)Common._M2(870), (float)Common._M3(183)),
				new FPoint((float)Common._M4(850), (float)Common._M5(285)),
				new FPoint((float)Common._M(815), (float)Common._M1(380)),
				new FPoint((float)Common._M2(828), (float)Common._M3(477)),
				new FPoint((float)Common._M4(850), (float)Common._M5(570)),
				new FPoint((float)Common._M(430), (float)Common._M1(330)),
				new FPoint((float)Common._M2(470), (float)Common._M3(459)),
				new FPoint((float)Common._M4(544), (float)Common._M5(569)),
				new FPoint((float)Common._M(340), (float)Common._M1(530)),
				new FPoint((float)Common._M2(390), (float)Common._M3(596)),
				new FPoint((float)Common._M4(390), (float)Common._M5(704)),
				new FPoint((float)Common._M(540), (float)Common._M1(91)),
				new FPoint((float)Common._M2(463), (float)Common._M3(165)),
				new FPoint((float)Common._M4(460), (float)Common._M5(269))
			};
			FPoint[] array2 = new FPoint[]
			{
				new FPoint(Common._M(1.2f), Common._M1(1f)),
				new FPoint(Common._M2(1.1f), Common._M3(1.1f)),
				new FPoint(Common._M4(1.4f), Common._M5(1.3f)),
				new FPoint(Common._M(1.3f), Common._M1(1.35f)),
				new FPoint(Common._M2(1.1f), Common._M3(1f)),
				new FPoint(Common._M4(1.2f), Common._M5(1.25f)),
				new FPoint(Common._M(1.17f), Common._M1(1.4f)),
				new FPoint(Common._M2(0.9f), Common._M3(1f)),
				new FPoint(Common._M4(0.9f), Common._M5(1.2f)),
				new FPoint(Common._M(0.5f), Common._M1(1f)),
				new FPoint(Common._M2(0.5f), Common._M3(1.2f)),
				new FPoint(Common._M4(1f), Common._M5(1.5f)),
				new FPoint(Common._M(1.05f), Common._M1(1.1f)),
				new FPoint(Common._M2(1.05f), Common._M3(1.1f)),
				new FPoint(Common._M4(1.25f), Common._M5(1.15f))
			};
			for (int i = 0; i < 5; i++)
			{
				string theStringId = string.Format("IMAGE_UI_MAP_{0}", LevelMgr.GetTerseZoneName(i + 2).ToUpper());
				Common.GetIdByStringId(theStringId);
				this.mOverlays[i].mAlpha = (float)Common._M(255);
				this.mOverlays[i].mUnlocked = false;
				for (int j = 0; j < 3; j++)
				{
					this.mOverlays[i].mCloudSizes[j] = array2[i * 3 + j];
					this.mOverlays[i].mCloudPoints[j] = array[i * 3 + j];
				}
			}
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x00098D4E File Offset: 0x00096F4E
		public void CloseDone()
		{
			this.mClosing = false;
			this.CleanButtons();
			this.mRemove = true;
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00098D64 File Offset: 0x00096F64
		public void CleanButtons()
		{
			this.mParent.RemoveWidget(this.mContinueBtn);
			GameApp.gApp.SafeDeleteWidget(this.mContinueBtn);
			this.mContinueBtn = null;
			this.mParent.RemoveWidget(this.mZoneBtn);
			GameApp.gApp.SafeDeleteWidget(this.mZoneBtn);
			this.mZoneBtn = null;
			this.mParent.RemoveWidget(this.mBackBtn);
			GameApp.gApp.SafeDeleteWidget(this.mBackBtn);
			this.mBackBtn = null;
			this.mParent.RemoveWidget(this.mSelectZoneBackBtn);
			GameApp.gApp.SafeDeleteWidget(this.mSelectZoneBackBtn);
			this.mSelectZoneBackBtn = null;
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x00098E14 File Offset: 0x00097014
		public void Hide(bool h)
		{
			if (this.mContinueBtn != null)
			{
				this.mContinueBtn.SetVisible(!h);
				this.mContinueBtn.SetDisabled(h);
			}
			if (this.mZoneBtn != null)
			{
				this.mZoneBtn.SetVisible(!h);
				this.mZoneBtn.SetDisabled(h);
			}
			ButtonWidget buttonWidget = this.mBackBtn;
			MapGenericButton mapGenericButton = this.mSelectZoneBackBtn;
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x00098E90 File Offset: 0x00097090
		public void Init(bool zone_completed, int disp_zone, int disp_level, bool from_checkpoint, bool from_load)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_HOME);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_HOME_SELECT);
			Image imageByID3 = Res.GetImageByID(ResID.IMAGE_UI_MAP_OPENBOOK_BACK);
			Image imageByID4 = Res.GetImageByID(ResID.IMAGE_UI_MAP_OPENBOOK_BACK_DWN);
			Image imageByID5 = Res.GetImageByID(ResID.IMAGE_UI_MAP_OPENBOOK_PAGES);
			Image imageByID6 = Res.GetImageByID(ResID.IMAGE_UI_MAP_CONTINUE_BUTTON);
			Image imageByID7 = Res.GetImageByID(ResID.IMAGE_UI_MAP_BOOK_ANIM);
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_STROKE);
			this.mClosing = false;
			this.mIntroClosing = false;
			this.mExtrasAlpha.SetConstant(1.0);
			this.mUpdateCount = 0;
			this.mFromIntro = !zone_completed && disp_zone == 1 && disp_level == 1 && !from_load;
			this.mHasPlayedZoneUnlockedSound = false;
			int num = (GameApp.gApp.mClickedHardMode ? GameApp.gApp.mUserProfile.mHeroicModeVars.mHighestZoneBeat : GameApp.gApp.mUserProfile.mAdvModeVars.mHighestZoneBeat);
			this.mBeatGame = num >= 6;
			this.mUnlockScrollAmt = 0f;
			this.mDisplayedZone = disp_zone;
			this.mCompletedZone = zone_completed;
			this.mContinueFromCheckpoint = false;
			this.mFromCheckpoint = from_checkpoint && !from_load;
			this.mContinueGoesToCheckpoint = from_load && from_checkpoint;
			if (this.mContinueBtn != null)
			{
				this.mParent.RemoveWidget(this.mContinueBtn);
			}
			if (this.mZoneBtn != null)
			{
				this.mParent.RemoveWidget(this.mZoneBtn);
			}
			if (this.mBackBtn != null)
			{
				this.mParent.RemoveWidget(this.mBackBtn);
			}
			if (this.mSelectZoneBackBtn != null)
			{
				this.mParent.RemoveWidget(this.mSelectZoneBackBtn);
			}
			this.mBackBtn = null;
			this.mContinueBtn = null;
			this.mZoneBtn = null;
			this.mSelectZoneBackBtn = null;
			this.mBackBtn = null;
			this.mContinueBtn = null;
			this.mZoneBtn = null;
			this.mSelectZoneBackBtn = null;
			this.mDisplayingZones = false;
			int theValue = ((GameApp.gApp.GetBoard() == null) ? GameApp.gApp.mUserProfile.GetAdvModeVars().mCurrentAdvScore : GameApp.gApp.GetBoard().mScore);
			if (from_checkpoint && from_load)
			{
				Level checkpointLevel = GameApp.gApp.GetBoard().GetCheckpointLevel();
				disp_level = checkpointLevel.mNum;
				disp_zone = checkpointLevel.mZone;
				theValue = GameApp.gApp.GetBoard().GetCheckpointScore();
			}
			int[] array = new int[] { 25, 51, 85, 115, 145, 175 };
			if (disp_level == 2147483647 || disp_level == 10)
			{
				this.mHighestDot = array[disp_zone - 1];
			}
			else if (disp_zone == 1)
			{
				this.mHighestDot = (int)((float)disp_level / 10f * (float)array[0]);
			}
			else
			{
				this.mHighestDot = (int)((float)disp_level / 10f * (float)(array[disp_zone - 1] - array[disp_zone - 2]) + (float)array[disp_zone - 2]);
			}
			if (!zone_completed && !this.mFromIntro)
			{
				this.mContinueBtn = new MapButton(1, this);
				this.mContinueBtn.mUsesAnimators = false;
				this.mContinueBtn.mMapScreen = this;
				this.mContinueBtn.mDoFinger = true;
				this.mContinueBtn.mPriority = 2;
				this.mContinueBtn.Resize(Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_MAP_CONTINUE_BUTTON) - 80) + Common._DS(14), Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAP_CONTINUE_BUTTON)) - Common._DS(Common._M1(6)), imageByID6.GetCelWidth(), imageByID6.GetCelHeight() + Common._DS(Common._M3(40)));
				this.mParent.AddWidget(this.mContinueBtn);
				string @string = TextManager.getInstance().getString(778);
				string string2 = TextManager.getInstance().getString(798);
				if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CHT)
				{
					string text = @string.Substring(0, @string.IndexOf("$"));
					string text2 = string2.Substring(0, string2.IndexOf("（"));
					text2 = text2.Replace("$1", "{0}");
					this.mContinueBtn.mLevel = ((disp_level == int.MaxValue) ? string.Format(text + " {0}", disp_zone) : string.Format(text2, (disp_zone - 1) * 10 + disp_level));
				}
				else
				{
					string text3 = @string.Substring(0, @string.IndexOf(" "));
					string text4 = string2.Substring(0, string2.IndexOf(" "));
					this.mContinueBtn.mLevel = ((disp_level == int.MaxValue) ? string.Format(text3 + " {0}", disp_zone) : string.Format(text4 + " {0}", (disp_zone - 1) * 10 + disp_level));
				}
				if (GameApp.USE_TRIAL_VERSION && disp_level == 2147483647)
				{
					this.mIsTrialEnd = true;
				}
				string string3 = TextManager.getInstance().getString(863);
				this.mContinueBtn.mScore = string.Format("{0} " + string3, SexyFramework.Common.CommaSeperate(theValue));
				int num2 = ((GameApp.gApp.GetBoard() == null) ? GameApp.gApp.mUserProfile.GetAdvModeVars().mCurrentAdvLives : GameApp.gApp.GetBoard().GetNumLives());
				if (num2 > 99)
				{
					num2 = 99;
				}
				num2--;
				if (num2 < 0)
				{
					num2 = 2;
				}
				this.mContinueBtn.mLives = string.Format("x{0}", num2);
				this.mZoneBtn = new MapGenericButton(2, this);
				this.mZoneBtn.mUsesAnimators = false;
				this.mZoneBtn.mDoFinger = true;
				this.mZoneBtn.mPriority = 2;
				this.mZoneBtn.mButtonImage = (this.mZoneBtn.mOverImage = (this.mZoneBtn.mDownImage = imageByID7));
				this.mZoneBtn.mNormalRect = this.mZoneBtn.mButtonImage.GetCelRect(0);
				this.mZoneBtn.mOverRect = this.mZoneBtn.mButtonImage.GetCelRect(0);
				this.mZoneBtn.mDownRect = this.mZoneBtn.mButtonImage.GetCelRect(1);
				this.mZoneBtn.Resize(Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_MAP_BOOK_ANIM) - 80), Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAP_BOOK_ANIM)), this.mZoneBtn.mButtonImage.GetCelWidth(), this.mZoneBtn.mButtonImage.GetCelHeight());
				this.mParent.AddWidget(this.mZoneBtn);
				this.mBackBtn = new ButtonWidget(3, this);
				this.mBackBtn.SetVisible(false);
				this.mBackBtn.SetDisabled(true);
				this.mBackBtn.mDoFinger = true;
				this.mBackBtn.mPriority = 2;
				this.mBackBtn.mButtonImage = imageByID;
				this.mBackBtn.mDownImage = imageByID2;
				float num3 = (float)(imageByID2.GetWidth() - imageByID.GetWidth()) / 2f;
				float num4 = (float)(imageByID2.GetHeight() - imageByID.GetHeight()) / 2f;
				this.mBackBtn.Resize(Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_HOME_SELECT)) - Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_CHALLENGESCREEN_HOME_SELECT)) + (int)num3, Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_CHALLENGESCREEN_HOME_SELECT)) + (int)num4, imageByID2.GetWidth(), imageByID2.GetHeight());
				this.mBackBtn.mNormalRect = new Rect(0, 0, imageByID.GetWidth(), imageByID.GetHeight());
				float num5 = (float)((imageByID2.GetWidth() - imageByID.GetWidth()) / 2);
				float num6 = (float)((imageByID2.GetHeight() - imageByID.GetHeight()) / 2);
				this.mBackBtn.mDownRect = new Rect((int)num5, (int)num6, imageByID2.GetWidth() - (int)num5, imageByID2.GetHeight() - (int)num6);
				this.mParent.AddWidget(this.mBackBtn);
				this.mSelectZoneBackBtn = new MapGenericButton(4, this);
				this.mSelectZoneBackBtn.SetVisible(false);
				this.mSelectZoneBackBtn.SetDisabled(true);
				this.mSelectZoneBackBtn.mUsesAnimators = false;
				this.mSelectZoneBackBtn.mDoFinger = true;
				this.mSelectZoneBackBtn.mPriority = 2;
				this.mSelectZoneBackBtn.mButtonImage = imageByID3;
				this.mSelectZoneBackBtn.mDownImage = imageByID4;
				int num7 = (GameApp.gApp.GetScreenRect().mWidth - imageByID5.mWidth) / 2;
				this.mSelectZoneBackBtn.Resize(num7 + Common._DS(Res.GetOffsetXByID(ResID.IMAGE_UI_MAP_OPENBOOK_BACK)), Common._DS(Res.GetOffsetYByID(ResID.IMAGE_UI_MAP_OPENBOOK_BACK)), imageByID3.GetWidth(), imageByID3.GetHeight());
				this.mParent.AddWidget(this.mSelectZoneBackBtn);
				this.mSelectZoneBackBtn.SetVisible(false);
				this.mSelectZoneBackBtn.SetDisabled(true);
				MapScreen.gZoneNames[0] = TextManager.getInstance().getString(839);
				MapScreen.gZoneNames[1] = TextManager.getInstance().getString(840);
				MapScreen.gZoneNames[2] = TextManager.getInstance().getString(841);
				MapScreen.gZoneNames[3] = TextManager.getInstance().getString(842);
				MapScreen.gZoneNames[4] = TextManager.getInstance().getString(843);
				MapScreen.gZoneNames[5] = TextManager.getInstance().getString(844);
				int num8 = ((disp_zone == 0) ? disp_zone : (disp_zone - 1));
				string theString = MapScreen.gZoneNames[num8];
				this.mNewZoneTextImg = new DeviceImage();
				this.mNewZoneTextImg.SetImageMode(true, true);
				this.mNewZoneTextImg.AddImageFlags(16U);
				this.mNewZoneTextImg.Create(fontByID.StringWidth(theString) + 60, fontByID.mHeight * 2);
				Graphics graphics = new Graphics(this.mNewZoneTextImg);
				graphics.Get3D().ClearColorBuffer(new SexyColor(0, 0));
				graphics.SetFont(fontByID);
				graphics.SetColor(SexyColor.White);
				graphics.DrawString(theString, 0, fontByID.GetAscent());
				graphics.ClearRenderContext();
			}
			else if (disp_zone > 1 || this.mFromIntro)
			{
				this.mNewZoneTextImg = new DeviceImage();
				this.mNewZoneTextImg.SetImageMode(true, true);
				this.mNewZoneTextImg.AddImageFlags(16U);
				this.mNewZoneTextImg.Create(fontByID.StringWidth("Underwater Grotto!") + 60, fontByID.mHeight * 2);
				Graphics graphics2 = new Graphics(this.mNewZoneTextImg);
				graphics2.Get3D().ClearColorBuffer(new SexyColor(0, 0));
				graphics2.SetFont(fontByID);
				graphics2.SetColor(SexyColor.White);
				string theString2 = string.Format("{0} {1}", LevelMgr.GetZoneName(disp_zone - 1), this.mBeatGame ? "..." : "");
				if (this.mFromIntro)
				{
					graphics2.DrawString(TextManager.getInstance().getString(661), (this.mNewZoneTextImg.mWidth - fontByID.StringWidth(TextManager.getInstance().getString(661))) / 2, fontByID.GetAscent());
					graphics2.DrawString(TextManager.getInstance().getString(662), (this.mNewZoneTextImg.mWidth - fontByID.StringWidth(TextManager.getInstance().getString(662))) / 2, fontByID.GetAscent() + fontByID.mHeight - Common._DS(Common._M(30)) + Localization.GetCurrentFontOffsetY());
				}
				else if (!this.mBeatGame)
				{
					graphics2.DrawString(theString2, (this.mNewZoneTextImg.mWidth - fontByID.StringWidth(theString2)) / 2, fontByID.GetAscent());
					graphics2.DrawString(TextManager.getInstance().getString(663), (this.mNewZoneTextImg.mWidth - fontByID.StringWidth(TextManager.getInstance().getString(663))) / 2, fontByID.GetAscent() + fontByID.mHeight - Common._DS(Common._M(10)));
				}
				else
				{
					graphics2.DrawString(TextManager.getInstance().getString(661), (this.mNewZoneTextImg.mWidth - fontByID.StringWidth(TextManager.getInstance().getString(661))) / 2, fontByID.GetAscent());
					graphics2.DrawString(theString2, (this.mNewZoneTextImg.mWidth - fontByID.StringWidth(theString2)) / 2, fontByID.GetAscent() + fontByID.mHeight - Common._DS(Common._M(30)));
				}
				this.mNewZoneTextSize = 0f;
				this.mNewZoneTextBounceCount = 0;
				graphics2.ClearRenderContext();
			}
			for (int i = 0; i < 5; i++)
			{
				this.mOverlays[i].mUnlocked = i + 1 <= num;
				if (this.mOverlays[i].mUnlocked)
				{
					this.mOverlays[i].mAlpha = 0f;
				}
			}
			this.zone_effects[0] = null;
			this.zone_effects[1] = GameApp.gApp.GetPIEffect("goldsparkle_area_L2");
			this.zone_effects[2] = GameApp.gApp.GetPIEffect("goldsparkle_area_L3");
			this.zone_effects[3] = GameApp.gApp.GetPIEffect("goldsparkle_area_L4");
			this.zone_effects[4] = GameApp.gApp.GetPIEffect("goldsparkle_area_L5");
			this.zone_effects[5] = GameApp.gApp.GetPIEffect("goldsparkle_area_L6");
			this.mUnlockNameAlpha.SetConstant(1.0);
			this.mUnlockNameHilite.SetConstant(1.0);
			this.mUnlockOutlineAlpha.SetConstant(1.0);
			this.mUnlockIconAlpha.SetConstant(1.0);
			this.mClickToEnterAlpha.SetConstant(1.0);
			if (zone_completed)
			{
				this.mUnlockScrollAmt = (float)Common._DS(Common._M(-206));
				this.mZoneEffect = this.zone_effects[disp_zone - 1];
				this.mZoneEffect.ResetAnim();
				this.mZoneEffect.mEmitAfterTimeline = true;
				ResID resID = ResID.IMAGE_UI_MAP_JUNGLE_OVERLAY + disp_zone - 1;
				this.mZoneEffect.GetLayer("general sparkle").GetEmitter("sparkle area").mMaskImage = GameApp.gApp.mResourceManager.GetResourceRef(0, resID.ToString()).GetSharedImageRef();
				this.mZoneEffect.mDrawTransform.LoadIdentity();
				float num9 = GameApp.DownScaleNum(1f);
				this.mZoneEffect.mDrawTransform.Scale(num9, num9);
				this.mZoneEffect.mDrawTransform.Translate((float)(Common._DS(Res.GetOffsetXByID(resID)) - Common._DS(80)), (float)Common._DS(Res.GetOffsetYByID(resID)));
				for (int j = 0; j < disp_zone - 1; j++)
				{
					this.mOverlays[j].mUnlocked = true;
				}
				if (disp_zone > 1 && num < 6)
				{
					this.mOverlays[disp_zone - 2].mAlpha = 255f;
				}
				this.mUnlockNameAlpha.SetCurve(Common._MP("b;0,1,0.002,1,#########   z#### K~###  (~###  T~###"));
				this.mUnlockNameHilite.SetCurve(Common._MP("b+0,1,0,1,####  R#### =~m&F    a#### Q####"), this.mUnlockNameAlpha);
				this.mUnlockOutlineAlpha.SetCurve(Common._MP("b;0,1,0.001429,1,#########   }%###      $f###"));
				this.mUnlockIconAlpha.SetCurve(Common._MP("b+0,1,0,1,####  b#### .?### R#### oL###jL###  (####"), this.mUnlockNameAlpha);
				this.mClickToEnterAlpha.SetCurve(Common._MP("b+0,1,0,1,####     y#### b~###  D~###"), this.mUnlockNameAlpha);
				this.mDisableInput = true;
			}
			if (this.mFromIntro)
			{
				this.mUnlockNameAlpha.SetCurve(Common._MP("b+0,1,0.002,1,#########      :#### &~###  d~###"));
				this.mUnlockNameHilite.SetCurve(Common._MP("b+0,1,0,1,####     r#### T~### h####o####"), this.mUnlockNameAlpha);
				this.mUnlockOutlineAlpha.SetCurve(Common._MP("b+0,1,0.001429,1,#########   }%###      $N###"));
				this.mUnlockIconAlpha.SetCurve(Common._MP("b+0,1,0,1,####  b#### .?### R#### oL###jL###  (####"), this.mUnlockNameAlpha);
				this.mClickToEnterAlpha.SetCurve(Common._MP("b+0,1,0,1,####       w####h~### @~####~###"), this.mUnlockNameAlpha);
			}
			this.mRemove = false;
			this.mSelectedZone = -1;
			if (this.mFromCheckpoint)
			{
				this.ButtonDepress(this.mZoneBtn.mId);
			}
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x00099E4C File Offset: 0x0009804C
		public void Update()
		{
			if (GameApp.gApp.IsHardwareBackButtonPressed())
			{
				this.ProcessHardwareBackButton();
			}
			this.mDirty = false;
			int num = Common._DS(Common._M(-206));
			foreach (Widget widget in this.mParent.mWidgets)
			{
				widget.SetDisabled(this.mAlpha.mRamp == 6);
			}
			if (this.mAlpha.mRamp == 6)
			{
				this.mDirty = true;
				if (!this.mAlpha.IncInVal())
				{
					if (this.mAlpha == 0.0)
					{
						this.CleanButtons();
						GameApp.gApp.mClickedHardMode = false;
						GameApp.gApp.HideAdventureModeMapScreen();
						return;
					}
					this.mAlpha.SetConstant(this.mAlpha);
				}
				GameApp.gApp.mMainMenu.MarkAllDirty();
				return;
			}
			if (this.mSlideDir != 0)
			{
				this.mDirty = true;
				float num2 = Common._M(60f);
				this.mXOff += (float)this.mSlideDir * num2;
				this.mZoneBtn.mX += (int)(num2 * (float)this.mSlideDir);
				this.mBackBtn.mX += (int)(num2 * (float)this.mSlideDir);
				this.mContinueBtn.mX += (int)(num2 * (float)this.mSlideDir);
				if (this.mSlideDir == -1)
				{
					if (this.mXOff <= 0f)
					{
						this.mSlideDir = 0;
						float num3 = -this.mXOff;
						this.mXOff = 0f;
						this.mZoneBtn.mX += (int)num3;
						this.mBackBtn.mX += (int)num3;
						this.mContinueBtn.mX += (int)num3;
					}
				}
				else if (this.mSlideDir == 1 && this.mXOff >= (float)(GameApp.gApp.mWidth + Common._S(80)))
				{
					this.mXOff = (float)(GameApp.gApp.mWidth + Common._S(80));
					this.mSlideDir = 0;
					this.CleanButtons();
					GameApp.gApp.mClickedHardMode = false;
					GameApp.gApp.HideAdventureModeMapScreen();
				}
				GameApp.gApp.mMainMenu.MarkAllDirty();
				return;
			}
			if (this.mIncDisplayZoneAlpha)
			{
				this.mDisplayZoneAlpha += 5f;
				if (this.mDisplayZoneAlpha >= 255f)
				{
					this.mDisplayZoneAlpha = 255f;
					this.mIncDisplayZoneAlpha = false;
				}
			}
			else
			{
				this.mDisplayZoneAlpha -= 5f;
				if (this.mDisplayZoneAlpha <= 0f)
				{
					this.mDisplayZoneAlpha = 0f;
					this.mIncDisplayZoneAlpha = true;
				}
			}
			Board board = GameApp.gApp.GetBoard();
			if (board != null && board.mEndBossFadeAmt > 0f)
			{
				this.mDirty = true;
				board.mEndBossFadeAmt -= Common._M(2f);
				if (board.mEndBossFadeAmt >= 0f)
				{
					return;
				}
				board.mEndBossFadeAmt = 0f;
			}
			this.mUpdateCount++;
			Common._M(50);
			Common._M(60);
			if (this.mUnlockScrollAmt <= (float)num && !this.mHasPlayedZoneUnlockedSound && this.mNewZoneTextImg != null && !this.mBeatGame && this.mUpdateCount >= Common._M(150))
			{
				this.mHasPlayedZoneUnlockedSound = true;
			}
			if (this.mFromIntro && this.mUpdateCount >= Common._M(100))
			{
				this.mUnlockNameAlpha.IncInVal();
				this.mUnlockOutlineAlpha.IncInVal();
			}
			if (this.mUpdateCount >= Common._M(60) && (this.mUnlockScrollAmt <= (float)num || (this.mFromIntro && this.mUpdateCount >= Common._M(130))))
			{
				if (this.mNewZoneTextBounceCount < Common._M(5))
				{
					this.mDirty = true;
					float num4 = Common._M(0.1f) * (float)((this.mNewZoneTextBounceCount % 2 == 0) ? 1 : (-1));
					float num5 = Common._M(1.5f);
					if (this.mNewZoneTextBounceCount >= 2)
					{
						num5 /= 2f * (float)(this.mNewZoneTextBounceCount / 2);
					}
					this.mNewZoneTextSize += num4;
					if (num4 > 0f && this.mNewZoneTextSize > 1f + num5)
					{
						this.mNewZoneTextSize = 1f + num5;
						this.mNewZoneTextBounceCount++;
					}
					else if (num4 < 0f && this.mNewZoneTextSize <= 1f)
					{
						this.mNewZoneTextSize = 1f;
						this.mNewZoneTextBounceCount++;
					}
				}
				else if (this.mDisableInput)
				{
					this.mDisableInput = false;
				}
				if (!this.mFromIntro)
				{
					this.mZoneEffect.mDrawTransform.LoadIdentity();
					float num6 = GameApp.DownScaleNum(1f);
					this.mZoneEffect.mDrawTransform.Scale(num6, num6);
					this.mZoneEffect.mDrawTransform.Translate((float)(this.mAreaCoords[this.mDisplayedZone - 1].mX - Common._DS(80)) + this.mUnlockScrollAmt, (float)(this.mAreaCoords[this.mDisplayedZone - 1].mY + Common._DS(this.mZoneEffect.mHeight / 2)));
					this.mZoneEffect.Update();
					if (this.mZoneEffect.mCurNumParticles > 0)
					{
						this.mDirty = true;
					}
				}
			}
			if (this.mUpdateCount >= Common._M(25))
			{
				for (int i = 0; i < 5; i++)
				{
					if (this.mOverlays[i].mUnlocked && this.mOverlays[i].mAlpha > 0f)
					{
						if (this.mOverlays[i].mAlpha > 0f)
						{
							this.mDirty = true;
						}
						this.mOverlays[i].mAlpha -= Common._M(0.95f);
						if (this.mOverlays[i].mAlpha < 0f)
						{
							this.mOverlays[i].mAlpha = 0f;
						}
						for (int j = 0; j < 3; j++)
						{
							float num7 = Common._M(0.35f) + (float)j * Common._M1(0.15f);
							if (j == 1)
							{
								num7 *= -1f;
							}
							this.mOverlays[i].mCloudPoints[j].mX -= num7;
						}
					}
				}
			}
			if (this.mZoneOver)
			{
				this.mZoneOverPct = Math.Min(1f, this.mZoneOverPct + Common._M(0.05f));
			}
			else
			{
				this.mZoneOverPct = Math.Max(0f, this.mZoneOverPct - Common._M(0.075f));
			}
			if (GameApp.gApp.mHasFocus)
			{
				this.mDirty = true;
			}
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0009A550 File Offset: 0x00098750
		public void Draw(Graphics g)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_MAP_BKGRND);
			Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_HOME);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_HOME_BACKING);
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGLOUNGE28_BASE);
			g.SetColorizeImages(true);
			g.SetColor(this.mAlpha);
			g.DrawImage(imageByID, (int)((float)Common._S(0) + this.mXOff + this.mUnlockScrollAmt), 0);
			for (int i = 1; i < 7; i++)
			{
				if (i == this.mDisplayedZone)
				{
					g.SetColor(this.mAlpha);
					int id = 1294 + i - 1;
					int theX = Common._DS(Res.GetOffsetXByID((ResID)id)) + Common._S(0) + (int)this.mUnlockScrollAmt;
					int theY = Common._DS(Res.GetOffsetYByID((ResID)id));
					Image imageByID3 = Res.GetImageByID((ResID)id);
					double num = 0.0;
					g.SetColor(255, 255, 255, (int)(255.0 * Math.Min(1.0, num)));
					g.DrawImage(imageByID3, theX, theY);
					if (this.mBackBtn != null)
					{
						bool mVisible = this.mBackBtn.mVisible;
					}
					bool flag = false;
					if (GameApp.gApp.mBoard != null && GameApp.gApp.mBoard.mDoingFirstTimeIntro && GameApp.gApp.mBoard.mShowMapScreen && GameApp.gApp.mBoard.mIntroMapScale == 0.0 && !GameApp.gApp.mBoard.mDoIntroFrogJump)
					{
						flag = false;
					}
					else if (GameApp.gApp.mBoard != null && GameApp.gApp.mBoard.mDoingFirstTimeIntro && GameApp.gApp.mBoard.mShowMapScreen)
					{
						flag = true;
					}
					bool flag2 = false;
					if (GameApp.gApp.mBoard != null)
					{
						flag2 = GameApp.gApp.mBoard.mDoingFirstTimeIntro;
					}
					if ((this.mCompletedZone || flag2) && !flag)
					{
						g.SetDrawMode(1);
						g.SetColor(255, 255, 255, (int)this.mDisplayZoneAlpha);
						g.DrawImage(imageByID3, theX, theY);
						g.SetDrawMode(0);
					}
				}
				else
				{
					this.DrawDesaturatedZone(g, i, Common._M(1f));
				}
			}
			g.SetColor(this.mAlpha);
			for (int j = 0; j < 5; j++)
			{
				if (this.mOverlays[j].mAlpha > 0f)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)((double)this.mOverlays[j].mAlpha * this.mAlpha));
					for (int k = 0; k < 3; k++)
					{
						Image imageByID4 = Res.GetImageByID(ResID.IMAGE_UI_MAP_FOG1 + k);
						float num2 = (float)imageByID4.mWidth * this.mOverlays[j].mCloudSizes[k].mX * 2f;
						float num3 = (float)imageByID4.mHeight * this.mOverlays[j].mCloudSizes[k].mY * 2f;
						g.DrawImage(imageByID4, (int)(Common._DS(this.mOverlays[j].mCloudPoints[k].mX - 0f) + this.mXOff + this.mUnlockScrollAmt), (int)Common._DS(this.mOverlays[j].mCloudPoints[k].mY), (int)num2, (int)num3);
					}
				}
			}
			if (this.mDisplayingZones)
			{
				g.SetFont(fontByID);
				this.DrawZoneSelectBackground(g);
				for (int l = 0; l < 6; l++)
				{
					Image inZoneImage;
					Rect inZoneRect;
					this.GetZoneImage(l, out inZoneImage, out inZoneRect);
					this.DrawZoneImage(g, l, inZoneImage, inZoneRect);
					this.DrawZoneName(g, l, inZoneRect);
					this.DrawZoneLockedOverlay(g, l, inZoneRect);
					Common.DrawCommonDialogBorder(g, inZoneRect.mX - Common._DS(10), inZoneRect.mY - Common._DS(7), inZoneRect.mWidth + Common._DS(20), inZoneRect.mHeight + Common._DS(14));
				}
			}
			else
			{
				this.DrawMapZoneName(g);
			}
			if (this.mBackBtn != null && this.mBackBtn.mVisible)
			{
				g.DrawImage(imageByID2, -84, 0);
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0009A99C File Offset: 0x00098B9C
		public void MouseMove(int x, int y)
		{
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0009A99E File Offset: 0x00098B9E
		public void ButtonPress(int theId, int theClickCount)
		{
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0009A9A0 File Offset: 0x00098BA0
		public void MouseDown(int x, int y)
		{
			if (this.mDisableInput)
			{
				return;
			}
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			this.mLastMouseX = x;
			this.mLastMouseY = y;
			if (this.mFromIntro)
			{
				if (this.mZoneOver)
				{
					GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_MAPZOOMIN));
					GameApp.gApp.mUserProfile.mNeedsFirstTimeIntro = false;
					GameApp.gApp.PlaySong(12);
				}
				return;
			}
			if (this.mSlideDir != 0)
			{
				return;
			}
			if (this.mRemove)
			{
				return;
			}
			if (this.mCompletedZone)
			{
				GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_MAPZOOMIN));
				GameApp.gApp.SetCursor(ECURSOR.CURSOR_POINTER);
				this.mClosing = true;
			}
			this.OnZoneCardSelected();
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0009AA69 File Offset: 0x00098C69
		public void MouseLeave()
		{
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0009AA6B File Offset: 0x00098C6B
		public void ButtonMouseMove(int theId, int theX, int theY)
		{
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0009AA6D File Offset: 0x00098C6D
		public void ButtonDownTick(int theId)
		{
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x0009AA6F File Offset: 0x00098C6F
		public void ButtonMouseLeave(int theId)
		{
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x0009AA74 File Offset: 0x00098C74
		public void MouseUp(int x, int y)
		{
			if (this.mDisableInput)
			{
				return;
			}
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			if (this.mSelectedZone == -1)
			{
				return;
			}
			if (this.mCards[this.mSelectedZone - 1].Contains(x, y) && (this.mSelectedZone - 1 == 0 || this.mOverlays[this.mSelectedZone - 2].mUnlocked) && Enumerable.Count<KeyValuePair<int, Dialog>>(GameApp.gApp.mDialogMap) == 0)
			{
				GameApp.gApp.DoYesNoDialog("", TextManager.getInstance().getString(452), true, TextManager.getInstance().getString(446), TextManager.getInstance().getString(447), false, -1, 1, 0);
				GameApp.gApp.mYesNoDialogDelegate = new GameApp.YesNoDialogDelegate(this.ProcessYesNo);
				return;
			}
			this.mSelectedZone = -1;
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x0009AB62 File Offset: 0x00098D62
		public void ProcessYesNo(int theId)
		{
			GameApp gameApp = (GameApp)GlobalMembers.gSexyApp;
			if (theId == 1000)
			{
				this.mRemove = true;
				this.CleanButtons();
				return;
			}
			this.mSelectedZone = -1;
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x0009AB8C File Offset: 0x00098D8C
		public void DoSlide(bool slide_in)
		{
			if (slide_in)
			{
				this.mAlpha.SetCurve(Common._MP("b-0,1,0.02,1,####        n~### 3~###"));
				return;
			}
			this.mFadingOut = true;
			this.mAlpha.SetCurve(Common._MP("b-0,1,0.02,1,~###         ~####"));
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x0009ABC3 File Offset: 0x00098DC3
		public SexyPoint GetZoneCenter(int theZoneNum)
		{
			return this.mZoneCenters[theZoneNum - 1];
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0009ABD0 File Offset: 0x00098DD0
		public virtual void ButtonDepress(int id)
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			if (this.mSlideDir != 0)
			{
				return;
			}
			if (id == this.mContinueBtn.mId)
			{
				if (GameApp.USE_TRIAL_VERSION && this.mIsTrialEnd)
				{
					if (GameApp.gApp.mBoard != null)
					{
						GameApp.gApp.mBoard.Pause(true, true);
					}
					string @string = TextManager.getInstance().getString(832);
					int width_pad = Common._DS(Common._M(20));
					GameApp.gApp.DoYesNoDialog(TextManager.getInstance().getString(448), @string, true, TextManager.getInstance().getString(446), TextManager.getInstance().getString(447), false, Common._S(Common._M(50)), 1, width_pad);
					GameApp.gApp.mYesNoDialogDelegate = new GameApp.YesNoDialogDelegate(this.ProcessTrialYesNo);
					this.mIsTryAndBuyDialogShowing = true;
					return;
				}
				if (GameApp.gApp.mResourceManager.IsGroupLoaded("MenuRelated"))
				{
					GameApp.gApp.mResourceManager.DeleteResources("MenuRelated");
				}
				this.mContinueBtn.mLevel = "";
				this.mContinueBtn.mScore = "";
				this.mContinueBtn.mLives = "";
				this.mRemove = true;
				if (this.mContinueGoesToCheckpoint)
				{
					this.mContinueFromCheckpoint = true;
					return;
				}
			}
			else
			{
				if (id == this.mBackBtn.mId)
				{
					if (GameApp.gApp.GetBoard() != null)
					{
						this.CleanButtons();
						GameApp.gApp.mClickedHardMode = false;
						GameApp.gApp.EndCurrentGame();
						GameApp.gApp.ShowMainMenu();
					}
					else
					{
						this.CleanButtons();
						GameApp.gApp.mClickedHardMode = false;
						GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.HideAdventureModeMapScreen);
					}
					GameApp.gApp.ToggleBambooTransition();
					return;
				}
				if (id == this.mSelectZoneBackBtn.mId)
				{
					if (!this.mFromCheckpoint)
					{
						this.mZoneBtn.mDisabled = (this.mContinueBtn.mDisabled = false);
						this.mZoneBtn.mVisible = (this.mContinueBtn.mVisible = true);
						this.mDisplayingZones = false;
						return;
					}
					this.CleanButtons();
					this.mRemove = true;
					return;
				}
				else if (id == this.mZoneBtn.mId)
				{
					this.mDisplayingZones = true;
					this.mZoneBtn.mDisabled = (this.mContinueBtn.mDisabled = true);
					this.mZoneBtn.mVisible = (this.mContinueBtn.mVisible = false);
				}
			}
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0009AE68 File Offset: 0x00099068
		public void ButtonPress(int id)
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			int soundByID = Res.GetSoundByID(ResID.SOUND_BUTTON1);
			int soundByID2 = Res.GetSoundByID(ResID.SOUND_BUTTON2);
			int soundByID3 = Res.GetSoundByID(ResID.SOUND_BUTTON3);
			if (this.mContinueBtn.mId == id)
			{
				GameApp.gApp.PlaySample(soundByID3);
				return;
			}
			if (this.mBackBtn.mId == id || this.mZoneBtn.mId == id)
			{
				GameApp.gApp.PlaySample(soundByID2);
				return;
			}
			GameApp.gApp.PlaySample(soundByID);
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0009AF04 File Offset: 0x00099104
		public virtual void ButtonMouseEnter(int id)
		{
			this.mLastMouseX = (this.mLastMouseY = -1);
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0009AF24 File Offset: 0x00099124
		public void ProcessHardwareBackButton()
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				GameApp.gApp.OnHardwareBackButtonPressProcessed();
				return;
			}
			if (this.mIsTryAndBuyDialogShowing)
			{
				Dialog dialog = GameApp.gApp.GetDialog(1);
				if (dialog != null)
				{
					dialog.ButtonDepress(1001);
					GameApp.gApp.OnHardwareBackButtonPressProcessed();
					return;
				}
			}
			if (this.mDisplayingZones)
			{
				if (this.mSelectedZone != -1)
				{
					Dialog dialog2 = GameApp.gApp.GetDialog(1);
					if (dialog2 != null)
					{
						dialog2.ButtonDepress(1001);
						GameApp.gApp.OnHardwareBackButtonPressProcessed();
						return;
					}
				}
				this.mZoneBtn.mDisabled = (this.mContinueBtn.mDisabled = false);
				this.mZoneBtn.mVisible = (this.mContinueBtn.mVisible = true);
				this.mDisplayingZones = false;
				GameApp.gApp.OnHardwareBackButtonPressProcessed();
				return;
			}
			if (GameApp.gApp.GetBoard() != null)
			{
				this.CleanButtons();
				GameApp.gApp.mClickedHardMode = false;
				GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.DoDeferredEndGame);
			}
			else
			{
				this.CleanButtons();
				GameApp.gApp.mClickedHardMode = false;
				GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.HideAdventureModeMapScreen);
			}
			GameApp.gApp.ToggleBambooTransition();
			GameApp.gApp.OnHardwareBackButtonPressProcessed();
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0009B084 File Offset: 0x00099284
		private void DrawZoneSelectBackground(Graphics g)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_MAP_OPENBOOK_PAGES);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_BOOKTEXT);
			int theX = (GameApp.gApp.GetScreenRect().mWidth - imageByID.mWidth) / 2 + Common._DS(Common._M(0));
			int theY = (GameApp.gApp.mHeight - imageByID.mHeight) / 2;
			g.DrawImage(imageByID, theX, theY);
			int num = 4;
			g.DrawImage(imageByID2, theX, theY, imageByID2.mWidth - num, imageByID2.mHeight);
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0009B104 File Offset: 0x00099304
		private void GetZoneImage(int inZoneID, out Image outZoneImage, out Rect outZoneRect)
		{
			Image levelThumbnail = GameApp.gApp.GetLevelThumbnail(inZoneID * 10);
			Rect rect = this.mCards[inZoneID];
			rect.mWidth = (int)(Common._S(2f) * Common._S(0.55f) * (float)levelThumbnail.mWidth);
			rect.mHeight = (int)(Common._S(2f) * Common._S(0.55f) * (float)levelThumbnail.mHeight);
			outZoneImage = levelThumbnail;
			outZoneRect = rect;
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0009B188 File Offset: 0x00099388
		private void DrawZoneImage(Graphics g, int inZoneID, Image inZoneImage, Rect inZoneRect)
		{
			g.PushState();
			g.SetColorizeImages(false);
			g.DrawImage(inZoneImage, inZoneRect.mX, inZoneRect.mY, inZoneRect.mWidth, inZoneRect.mHeight);
			if (this.mSelectedZone - 1 == inZoneID)
			{
				g.PushState();
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)((double)Common._M(100) * this.mAlpha));
				g.SetDrawMode(1);
				g.DrawImage(inZoneImage, inZoneRect.mX, inZoneRect.mY, inZoneRect.mWidth, inZoneRect.mHeight);
				g.PopState();
			}
			g.PopState();
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0009B240 File Offset: 0x00099440
		private void DrawZoneName(Graphics g, int inZoneID, Rect inZoneRect)
		{
			string theString = string.Format("{0} - {1}", inZoneID + 1, MapScreen.gZoneNames[inZoneID]);
			int num = g.GetFont().StringWidth(theString);
			int theX = inZoneRect.mX + (inZoneRect.mWidth - num) / 2;
			int theY = inZoneRect.mY + inZoneRect.mHeight + Common._DS(50);
			g.SetColor(SexyColor.Black);
			g.DrawString(theString, theX, theY);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0009B2B4 File Offset: 0x000994B4
		private void DrawZoneLockedOverlay(Graphics g, int inZoneID, Rect inZoneRect)
		{
			if (inZoneID <= 0 || this.mOverlays[inZoneID - 1].mUnlocked)
			{
				return;
			}
			g.SetColor(0, 0, 0, 191);
			g.FillRect(inZoneRect);
			string @string = TextManager.getInstance().getString(664);
			g.SetColor(Common._M(255), Common._M1(0), Common._M2(0), (int)((double)Common._M3(255) * this.mAlpha));
			int num = g.GetFont().StringWidth(@string);
			int mHeight = g.GetFont().mHeight;
			int num2 = inZoneRect.mX + (inZoneRect.mWidth - num) / 2 + Common._DS(15);
			int num3 = inZoneRect.mY + Common._DS(Common._M(119));
			float num4 = 0f;
			if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_RU)
			{
				num4 = 0.8f;
			}
			Graphics3D graphics3D = g.Get3D();
			if (graphics3D != null)
			{
				num2 += Common._DS(Common._M(20));
				SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
				sexyTransform2D.Translate((float)(-(float)num2 - num / 2 + GlobalMembers.gSexyApp.mScreenBounds.mX), (float)(-(float)num3 - mHeight / 2));
				sexyTransform2D.RotateDeg((float)Common._M(45));
				if (num4 != 0f)
				{
					sexyTransform2D.Scale(num4, num4);
				}
				sexyTransform2D.Translate((float)(num2 + num / 2 - GlobalMembers.gSexyApp.mScreenBounds.mX), (float)(num3 + mHeight / 2));
				graphics3D.PushTransform(sexyTransform2D);
				g.DrawString(@string, num2, num3);
				graphics3D.PopTransform();
				return;
			}
			g.DrawString(@string, num2, num3);
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0009B450 File Offset: 0x00099650
		private void DrawMapZoneName(Graphics g)
		{
			g.SetColor(255, 255, 255, (int)(255.0 * this.mExtrasAlpha * this.mAlpha));
			if (this.mNewZoneTextSize > 0f)
			{
				this.mGlobalTranform.Reset();
				if (g.Is3D())
				{
					this.mGlobalTranform.Scale(this.mNewZoneTextSize, this.mNewZoneTextSize);
				}
				int num = (int)((this.mXOff + (float)GameApp.gApp.GetScreenRect().mWidth + (float)GameApp.gApp.GetScreenRect().mX) / 2f);
				if (this.mFromIntro)
				{
					g.DrawImageTransform(this.mNewZoneTextImg, this.mGlobalTranform, (float)num, (float)Common._DS(Common._M1(1000)));
				}
				else if (this.mCompletedZone)
				{
					g.DrawImageTransform(this.mNewZoneTextImg, this.mGlobalTranform, (float)num, (float)Common._DS(Common._M1(960)));
				}
				else
				{
					g.DrawImageTransform(this.mNewZoneTextImg, this.mGlobalTranform, (float)(Common._DS(1000) + GameApp.gApp.GetScreenRect().mX - GameApp.gApp.mWideScreenXOffset), (float)Common._DS(Common._M1(150)));
				}
			}
			if (this.mZoneEffect != null && this.mUpdateCount > Common._M(50) && !this.mFromIntro)
			{
				this.mZoneEffect.Draw(g);
			}
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0009B5D0 File Offset: 0x000997D0
		private void OnZoneCardSelected()
		{
			if (!this.mDisplayingZones)
			{
				return;
			}
			for (int i = 0; i < 6; i++)
			{
				if (this.MouseOverCard(i))
				{
					this.mSelectedZone = i + 1;
					GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BUTTON1));
					return;
				}
			}
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0009B61C File Offset: 0x0009981C
		public void ProcessTrialYesNo(int theId)
		{
			if (theId == 1000)
			{
				GameApp.gApp.ToMarketPlace();
				this.mIsTryAndBuyDialogShowing = false;
				return;
			}
			if (theId == 1001)
			{
				if (GameApp.gApp.GetBoard() != null)
				{
					this.CleanButtons();
					GameApp.gApp.mClickedHardMode = false;
					GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.DoDeferredEndGame);
				}
				else
				{
					this.CleanButtons();
					GameApp.gApp.mClickedHardMode = false;
					GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.HideAdventureModeMapScreen);
				}
				GameApp.gApp.ToggleBambooTransition();
				this.mIsTryAndBuyDialogShowing = false;
			}
		}

		// Token: 0x04000E6B RID: 3691
		internal const float POSTCARD_PCT = 0.55f;

		// Token: 0x04000E6C RID: 3692
		internal static readonly string[] gZoneNames = new string[] { "Jungle of Mystery", "Quiet Village", "Lost City", "Mosquito Coast", "Underwater Grotto", "Volcano Temple" };

		// Token: 0x04000E6D RID: 3693
		protected Transform mGlobalTranform = new Transform();

		// Token: 0x04000E6E RID: 3694
		private SexyPoint[] mAreaCoords = new SexyPoint[]
		{
			new SexyPoint(0, 0),
			new SexyPoint(Common._DS(Common._M(1150)), Common._DS(Common._M1(120))),
			new SexyPoint(Common._DS(Common._M2(1080)), Common._DS(Common._M3(396))),
			new SexyPoint(Common._DS(Common._M4(660)), Common._DS(Common._M5(341))),
			new SexyPoint(Common._DS(Common._M(600)), Common._DS(Common._M1(305))),
			new SexyPoint(Common._DS(Common._M2(720)), Common._DS(Common._M3(40)))
		};

		// Token: 0x04000E6F RID: 3695
		private SexyPoint[] mZoneCenters = new SexyPoint[]
		{
			new SexyPoint(Common._M(1458), Common._M1(653)),
			new SexyPoint(Common._M2(1120), Common._M3(305)),
			new SexyPoint(Common._M4(1077), Common._M5(585)),
			new SexyPoint(Common._M6(692), Common._M7(532)),
			new SexyPoint(Common._M(600), Common._M1(830)),
			new SexyPoint(Common._M2(728), Common._M3(290))
		};

		// Token: 0x04000E70 RID: 3696
		public PIEffect mZoneEffect;

		// Token: 0x04000E71 RID: 3697
		public MemoryImage mNewZoneTextImg;

		// Token: 0x04000E72 RID: 3698
		public float mNewZoneTextSize;

		// Token: 0x04000E73 RID: 3699
		public int mNewZoneTextBounceCount;

		// Token: 0x04000E74 RID: 3700
		public MapButton mContinueBtn;

		// Token: 0x04000E75 RID: 3701
		public MapGenericButton mZoneBtn;

		// Token: 0x04000E76 RID: 3702
		public ButtonWidget mBackBtn;

		// Token: 0x04000E77 RID: 3703
		public MapGenericButton mSelectZoneBackBtn;

		// Token: 0x04000E78 RID: 3704
		public MapOverlay[] mOverlays = SexyFramework.Common.CreateObjectArray<MapOverlay>(5);

		// Token: 0x04000E79 RID: 3705
		public bool mDisplayingZones;

		// Token: 0x04000E7A RID: 3706
		public bool mFromCheckpoint;

		// Token: 0x04000E7B RID: 3707
		public bool mFromIntro;

		// Token: 0x04000E7C RID: 3708
		public bool mContinueGoesToCheckpoint;

		// Token: 0x04000E7D RID: 3709
		public bool mBeatGame;

		// Token: 0x04000E7E RID: 3710
		public int mHighestDot;

		// Token: 0x04000E7F RID: 3711
		public int mUpdateCount;

		// Token: 0x04000E80 RID: 3712
		public int mSlideDir;

		// Token: 0x04000E81 RID: 3713
		public float mXOff;

		// Token: 0x04000E82 RID: 3714
		public float mUnlockScrollAmt;

		// Token: 0x04000E83 RID: 3715
		public int mLastMouseX;

		// Token: 0x04000E84 RID: 3716
		public int mLastMouseY;

		// Token: 0x04000E85 RID: 3717
		public bool mHasPlayedZoneUnlockedSound;

		// Token: 0x04000E86 RID: 3718
		public CurvedVal mUnlockNameAlpha = new CurvedVal();

		// Token: 0x04000E87 RID: 3719
		public CurvedVal mUnlockNameHilite = new CurvedVal();

		// Token: 0x04000E88 RID: 3720
		public CurvedVal mUnlockOutlineAlpha = new CurvedVal();

		// Token: 0x04000E89 RID: 3721
		public CurvedVal mUnlockIconAlpha = new CurvedVal();

		// Token: 0x04000E8A RID: 3722
		public CurvedVal mClickToEnterAlpha = new CurvedVal();

		// Token: 0x04000E8B RID: 3723
		public CurvedVal mExtrasAlpha = new CurvedVal();

		// Token: 0x04000E8C RID: 3724
		public CurvedVal mDotSubtract = new CurvedVal();

		// Token: 0x04000E8D RID: 3725
		public bool mDisableInput;

		// Token: 0x04000E8E RID: 3726
		public bool mFadingOut;

		// Token: 0x04000E8F RID: 3727
		public float mDisplayZoneAlpha;

		// Token: 0x04000E90 RID: 3728
		public bool mIncDisplayZoneAlpha;

		// Token: 0x04000E91 RID: 3729
		public WidgetContainer mParent;

		// Token: 0x04000E92 RID: 3730
		public CurvedVal mAlpha = new CurvedVal();

		// Token: 0x04000E93 RID: 3731
		public Rect[] mCards = SexyFramework.Common.CreateObjectArray<Rect>(6);

		// Token: 0x04000E94 RID: 3732
		public bool mCompletedZone;

		// Token: 0x04000E95 RID: 3733
		public bool mDirty;

		// Token: 0x04000E96 RID: 3734
		public bool mRemove;

		// Token: 0x04000E97 RID: 3735
		public bool mContinueFromCheckpoint;

		// Token: 0x04000E98 RID: 3736
		public int mSelectedZone;

		// Token: 0x04000E99 RID: 3737
		public int mDisplayedZone;

		// Token: 0x04000E9A RID: 3738
		public int mMapOffsetX = 53;

		// Token: 0x04000E9B RID: 3739
		public bool mIntroClosing;

		// Token: 0x04000E9C RID: 3740
		public bool mClosing;

		// Token: 0x04000E9D RID: 3741
		public bool mZoneOver;

		// Token: 0x04000E9E RID: 3742
		public float mZoneOverPct;

		// Token: 0x04000E9F RID: 3743
		public PIEffect[] zone_effects = new PIEffect[6];

		// Token: 0x04000EA0 RID: 3744
		public bool mIsTryAndBuyDialogShowing;

		// Token: 0x04000EA1 RID: 3745
		public bool mIsTrialEnd;
	}
}
