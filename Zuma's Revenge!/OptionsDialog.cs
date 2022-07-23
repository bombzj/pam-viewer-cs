using System;
using System.Text;
using JeffLib;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000125 RID: 293
	internal class OptionsDialog : ZumaDialog, SliderListener
	{
		// Token: 0x06000F14 RID: 3860 RVA: 0x0009BD14 File Offset: 0x00099F14
		public OptionsDialog(bool inGame)
			: base(2, true, "", "", "", 0)
		{
			this.mLanguageButton = null;
			this.mInGame = inGame;
			this.mMusicEnabled = false;
			this.mMusicSliderOn = false;
			this.mHeightPad = ZumasRevenge.Common._S(ZumasRevenge.Common._M(272));
			this.mState = OptionsDialog.OptionState.OptionState_None;
			this.mAllowDrag = false;
			this.mClip = false;
			this.LoadResources();
			this.InitMusicSlider();
			this.InitSfxSlider();
			this.InitColorblindSlider();
			this.InitButtons();
			this.InitSize();
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0009BDA4 File Offset: 0x00099FA4
		~OptionsDialog()
		{
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x0009BDCC File Offset: 0x00099FCC
		public override void Resize(int theX, int theY, int theWidth, int theHeight)
		{
			base.Resize(theX, theY, theWidth, theHeight);
			if (this.mInGame)
			{
				this.LayoutAdventureDialog();
				return;
			}
			this.LayoutMainMenuDialog();
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0009BDF0 File Offset: 0x00099FF0
		public override void Update()
		{
			base.Update();
			if (this.mMusicVolumeSlider.mDisabled && !GameApp.gApp.mMusicInterface.m_isUserMusicOn)
			{
				this.mMusicVolumeSlider.mDisabled = false;
				this.mMusicEnabled = true;
				double musicVolume = GameApp.gApp.GetMusicVolume();
				this.mOriginMusicVolume = musicVolume;
				this.mMusicVolumeSlider.SetValue(musicVolume);
			}
			else if (!this.mMusicVolumeSlider.mDisabled && GameApp.gApp.mMusicInterface.m_isUserMusicOn)
			{
				this.mMusicVolumeSlider.mDisabled = true;
				this.mMusicEnabled = false;
				this.mMusicVolumeSlider.SetValue(0.0);
			}
			if (this.mState == OptionsDialog.OptionState.OptionState_OptionToMainMenuPrompt)
			{
				this.SetVisible(false);
				return;
			}
			this.SetVisible(true);
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x0009BEB4 File Offset: 0x0009A0B4
		public override void Draw(Graphics g)
		{
			if (GameApp.gApp.mCredits != null)
			{
				return;
			}
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGEXOTICA100_STROKE);
			Font fontByID2 = Res.GetFontByID(ResID.FONT_SHAGLOUNGE45_GAUNTLET);
			Font fontByID3 = Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_GAUNTLET);
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_LARGE_CROWN);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BOX_MAINMENU_CROWN_BOX);
			Image imageByID3 = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_LARGE_ACECROWN);
			Image imageByID4 = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_CROWN_HOLE);
			Image imageByID5 = Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BOX_ADVENTURE);
			g.PushState();
			g.Translate(-this.mX, -this.mY);
			g.SetColor(0, 0, 0, 130);
			g.FillRect(ZumasRevenge.Common._S(-80), 0, GameApp.gApp.mWidth + ZumasRevenge.Common._S(160), GameApp.gApp.mHeight);
			g.PopState();
			base.Draw(g);
			if (this.mInGame)
			{
				g.SetFont(fontByID);
				g.SetColor(255, 255, 255);
				Board board = GameApp.gApp.GetBoard();
				if (GameApp.gApp.GetBoard().GauntletMode())
				{
					g.SetFont(fontByID2);
					g.SetColorizeImages(true);
					g.SetColor(SexyColor.White);
					int num = ZumasRevenge.Common._S(100);
					int theY = ZumasRevenge.Common._S(120);
					if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_SP || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_SPC)
					{
						num -= 35;
					}
					string @string = TextManager.getInstance().getString(669);
					float num2 = (float)fontByID2.StringWidth(@string);
					g.DrawString(@string, num, theY);
					string theString = SexyFramework.Common.CommaSeperate(board.mScore);
					fontByID2.StringWidth(theString);
					float num3 = (float)num + num2 + (float)ZumasRevenge.Common._DS(20);
					g.DrawString(theString, (int)num3, theY);
					float num4 = (float)ZumasRevenge.Common._S(190);
					float num5 = (float)ZumasRevenge.Common._S(150);
					float num6 = (float)ZumasRevenge.Common._DS(15);
					float num7 = (float)ZumasRevenge.Common._DS(10);
					float num8 = 0.5f;
					float num9 = (float)imageByID.GetWidth() * num8;
					float num10 = (float)imageByID.GetHeight() * num8;
					float num11 = (float)imageByID.GetWidth() * num8;
					imageByID.GetHeight();
					g.DrawImage(imageByID2, ZumasRevenge.Common._S(40), ZumasRevenge.Common._S(129));
					g.DrawImage(imageByID, (int)num4, (int)num5, (int)num9, (int)num10);
					string theString2 = SexyFramework.Common.UCommaSeparate((uint)board.mLevel.mChallengePoints);
					g.DrawString(theString2, (int)(num4 + num9 + num7), (int)(num5 + (float)fontByID2.mAscent));
					g.DrawImage(imageByID3, (int)num4, (int)(num5 + num10 + num6), (int)num11, (int)num10);
					string theString3 = SexyFramework.Common.UCommaSeparate((uint)board.mLevel.mChallengeAcePoints);
					g.DrawString(theString3, (int)(num4 + num11 + num7), (int)(num5 + num10 + num6 + (float)fontByID2.mAscent));
					string text = JeffLib.Common.UpdateToTimeStr(board.mLevel.mGauntletCurTime);
					string text2 = JeffLib.Common.UpdateToTimeStr(((GameApp)GlobalMembers.gSexyApp).GetLevelMgr().mGauntletSessionLength);
					string text3 = string.Format(" {0} / {1}", text, text2);
					g.DrawString(TextManager.getInstance().getString(679) + text3, ZumasRevenge.Common._S(45), ZumasRevenge.Common._S(310));
					if (GameApp.gApp.mUserProfile != null && GameApp.gApp.mBoard != null && GameApp.gApp.mBoard.mLevel != null)
					{
						float num12 = (float)ZumasRevenge.Common._S(60);
						float num13 = (float)ZumasRevenge.Common._S(132);
						string text4 = "";
						Image image;
						if (board.mScore < board.mLevel.mChallengePoints)
						{
							image = imageByID4;
							text4 = TextManager.getInstance().getString(681);
						}
						else if (board.mScore < board.mLevel.mChallengeAcePoints)
						{
							image = imageByID;
						}
						else
						{
							image = imageByID3;
						}
						if (image != null)
						{
							if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_RU)
							{
								g.DrawImage(image, (int)num12 - 24, (int)num13 - 28, (int)((double)imageByID4.GetWidth() * 1.35), (int)((double)imageByID4.GetHeight() * 1.35));
							}
							else if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_SP || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_SPC || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_PL)
							{
								g.DrawImage(image, (int)num12 - 13, (int)num13 - 15, (int)((double)imageByID4.GetWidth() * 1.2), (int)((double)imageByID4.GetHeight() * 1.2));
							}
							else
							{
								g.DrawImage(image, (int)num12, (int)num13, imageByID4.GetWidth(), imageByID4.GetHeight());
							}
							g.SetColor(136, 156, 43, 255);
							g.SetFont(fontByID3);
							g.GetFont().StringWidth(text4);
							float num14 = num12 + (float)ZumasRevenge.Common._S(7);
							float num15 = num13 + (float)ZumasRevenge.Common._S(38);
							g.PushState();
							g.SetScale(0.7f, 0.7f, num14, num15);
							g.WriteWordWrapped(new Rect((int)num14 + ZumasRevenge.Common._S(15), (int)num15, imageByID4.GetWidth(), imageByID4.GetHeight()), text4, -1, 0);
							g.PopState();
						}
					}
					g.SetColorizeImages(false);
					return;
				}
				g.SetFont(fontByID2);
				g.SetColorizeImages(true);
				g.SetColor(SexyColor.White);
				g.DrawString(TextManager.getInstance().getString(670), ZumasRevenge.Common._S(120), ZumasRevenge.Common._S(120));
				int num16 = ZumasRevenge.Common._S(80);
				int num17 = ZumasRevenge.Common._S(130);
				g.DrawImage(imageByID5, num16, num17);
				int num18 = board.GetNumLives() - 1;
				if (num18 < 0)
				{
					num18 = 0;
				}
				else if (num18 > 99)
				{
					num18 = 99;
				}
				string theString4 = string.Format("x {0}", num18);
				fontByID2.StringWidth(theString4);
				float num19 = (float)(num16 + imageByID5.GetWidth() + ZumasRevenge.Common._S(10));
				float num20 = (float)(num17 + imageByID5.GetHeight() / 2);
				g.DrawString(theString4, (int)num19, (int)num20);
				if (GameApp.gApp.mBoard.mGameState != GameState.GameState_Losing)
				{
					string string2 = TextManager.getInstance().getString(679);
					float num21 = (float)fontByID2.StringWidth(string2);
					Level mLevel = board.mLevel;
					int num22 = 65;
					if (mLevel != null && mLevel.mBoss == null && mLevel.mIndex != num22)
					{
						StringBuilder stringBuilder = new StringBuilder(TextManager.getInstance().getString(671));
						stringBuilder.Replace("$1", JeffLib.Common.UpdateToTimeStr(mLevel.mParTime));
						string theString5 = stringBuilder.ToString();
						float num23 = (float)fontByID2.StringWidth(theString5);
						g.DrawString(theString5, ZumasRevenge.Common._S(120) + (int)(num21 - num23) / 2, (int)num20 + ZumasRevenge.Common._S(160));
					}
					string theString6;
					if (board.mGameState != GameState.GameState_Playing)
					{
						theString6 = JeffLib.Common.UpdateToTimeStr(board.mEndLevelStats.mTimePlayed);
					}
					else
					{
						theString6 = JeffLib.Common.UpdateToTimeStr(board.mStateCount - board.mIgnoreCount);
					}
					float num24 = (float)fontByID2.StringWidth(theString6);
					g.DrawString(string2, ZumasRevenge.Common._S(120), (int)num20 + ZumasRevenge.Common._S(80));
					g.DrawString(theString6, ZumasRevenge.Common._S(120) + (int)(num21 - num24) / 2, (int)num20 + ZumasRevenge.Common._S(120));
				}
			}
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x0009C5D8 File Offset: 0x0009A7D8
		public virtual void DrawAll(ref ModalFlags theFlags, Graphics g)
		{
			g.PushState();
			g.Translate(-this.mX, -this.mY);
			g.SetColor(0, 0, 0, 130);
			g.FillRect(ZumasRevenge.Common._S(-80), 0, GameApp.gApp.mWidth + ZumasRevenge.Common._S(160), GameApp.gApp.mHeight);
			g.PopState();
			base.DrawAll(theFlags, g);
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x0009C64C File Offset: 0x0009A84C
		public override void AddedToManager(WidgetManager theWidgetManager)
		{
			base.AddedToManager(theWidgetManager);
			this.AddWidget(this.mMusicVolumeSlider);
			this.AddWidget(this.mSfxVolumeSlider);
			this.AddWidget(this.mHelpButton);
			this.AddWidget(this.mMainMenuButton);
			this.AddWidget(this.mBackToGame);
			this.AddWidget(this.mCreditsButton);
			this.AddWidget(this.mColorBlindSlider);
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x0009C6B4 File Offset: 0x0009A8B4
		public override void RemovedFromManager(WidgetManager theWidgetManager)
		{
			base.RemovedFromManager(theWidgetManager);
			this.RemoveWidget(this.mMusicVolumeSlider);
			this.RemoveWidget(this.mSfxVolumeSlider);
			this.RemoveWidget(this.mHelpButton);
			this.RemoveWidget(this.mMainMenuButton);
			this.RemoveWidget(this.mBackToGame);
			this.RemoveWidget(this.mCreditsButton);
			this.RemoveWidget(this.mColorBlindSlider);
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x0009C71C File Offset: 0x0009A91C
		public void SliderVal(int theId, double theVal)
		{
			switch (theId)
			{
			case 0:
				if (GameApp.gApp.mMusicInterface.isPlayingUserMusic() && theVal > 0.0)
				{
					GameApp.gApp.mMusicInterface.stopUserMusic();
				}
				this.SetMusicSlider(theVal);
				return;
			case 1:
				this.SetSfxSlider(theVal);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0009C778 File Offset: 0x0009A978
		public void ProcessYesNo(int theId)
		{
			GameApp gameApp = (GameApp)GlobalMembers.gSexyApp;
			if (theId == 1000)
			{
				gameApp.KillDialog(this);
				gameApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.DoDeferredEndGame);
				gameApp.ToggleBambooTransition();
				gameApp.mMusic.StopAll();
			}
			this.mState = OptionsDialog.OptionState.OptionState_None;
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x0009C7D4 File Offset: 0x0009A9D4
		public override void ButtonDepress(int theId)
		{
			base.ButtonDepress(theId);
			GameApp gameApp = (GameApp)GlobalMembers.gSexyApp;
			if (theId == 3)
			{
				this.mState = OptionsDialog.OptionState.OptionState_OptionToMainMenuPrompt;
				int width_pad = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(20));
				string @string;
				if (((GameApp)GlobalMembers.gSexyApp).GetBoard().GauntletMode())
				{
					@string = TextManager.getInstance().getString(449);
				}
				else if (GameApp.gApp.GetBoard().IronFrogMode())
				{
					@string = TextManager.getInstance().getString(450);
				}
				else
				{
					@string = TextManager.getInstance().getString(451);
				}
				this.SetVisible(false);
				gameApp.DoYesNoDialog(TextManager.getInstance().getString(448), @string, true, TextManager.getInstance().getString(446), TextManager.getInstance().getString(447), false, ZumasRevenge.Common._S(ZumasRevenge.Common._M(50)), 1, width_pad);
				gameApp.mYesNoDialogDelegate = new GameApp.YesNoDialogDelegate(this.ProcessYesNo);
				this.SetVisible(true);
				return;
			}
			if (theId == 8)
			{
				this.mState = OptionsDialog.OptionState.OptionState_None;
				GameApp.gApp.FinishOptionsDialog(true);
				return;
			}
			if (theId == 2)
			{
				this.mState = OptionsDialog.OptionState.OptionState_Help;
				Board board = GameApp.gApp.GetBoard();
				GameApp.gApp.mColorblind = this.mColorBlindSlider.IsOn();
				if (board != null && board.GauntletMode())
				{
					board.ShowChallengeHelpScreen();
					return;
				}
				GameApp.gApp.mGenericHelp = new GenericHelp();
				GameApp.gApp.AddDialog(GameApp.gApp.mGenericHelp);
				return;
			}
			else
			{
				if (theId == 5)
				{
					this.mState = OptionsDialog.OptionState.OptionState_Credits;
					GameApp.gApp.DoCredits(true);
					return;
				}
				if (theId == 7)
				{
					this.mState = OptionsDialog.OptionState.OptionState_Legal;
					GameApp.gApp.ShowLegal();
				}
				return;
			}
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x0009C974 File Offset: 0x0009AB74
		public void DetectMusicSettings()
		{
			this.mMusicEnabled = GameApp.gApp.MusicEnabled();
			double num = (this.mMusicEnabled ? GameApp.gApp.GetMusicVolume() : 0.0);
			this.mOriginMusicVolume = num;
			this.mMusicVolumeSlider.SetValue(num);
			this.SetMusicSlider(num);
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x0009C9C9 File Offset: 0x0009ABC9
		private void LoadResources()
		{
			if (GameApp.gApp.mResourceManager.IsGroupLoaded("CommonGame"))
			{
				return;
			}
			if (!GameApp.gApp.mResourceManager.LoadResources("CommonGame"))
			{
				GameApp.gApp.Shutdown();
			}
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x0009CA02 File Offset: 0x0009AC02
		private void InitMusicSlider()
		{
			this.mMusicVolumeSlider = new ZumaSlider(0, this, TextManager.getInstance().getString(672));
			this.mMusicVolumeSlider.mFeedbackSoundID = Res.GetSoundByID(ResID.SOUND_BALLCLICK1);
			this.DetectMusicSettings();
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x0009CA3C File Offset: 0x0009AC3C
		private void InitSfxSlider()
		{
			this.mSfxVolumeSlider = new ZumaSlider(1, this, TextManager.getInstance().getString(673));
			this.mSfxVolumeSlider.mFeedbackSoundID = Res.GetSoundByID(ResID.SOUND_BALLCLICK1);
			this.mOriginSfxVolume = GlobalMembers.gSexyApp.GetSfxVolume();
			this.mSfxVolumeSlider.SetValue(this.mOriginSfxVolume);
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x0009CA9B File Offset: 0x0009AC9B
		private void InitColorblindSlider()
		{
			this.mColorBlindSlider = new ZumaSlideBox(this, 4, TextManager.getInstance().getString(680));
			this.mOriginColorBlind = GameApp.gApp.mColorblind;
			this.mColorBlindSlider.SetOnOff(this.mOriginColorBlind);
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x0009CADC File Offset: 0x0009ACDC
		private void InitButtons()
		{
			this.mMainMenuButton = this.InitButton(3, TextManager.getInstance().getString(676));
			this.mHelpButton = this.InitButton(2, TextManager.getInstance().getString(674));
			this.mBackToGame = this.InitButton(8, TextManager.getInstance().getString(675));
			this.mCreditsButton = this.InitButton(5, TextManager.getInstance().getString(677));
			this.HideButton(this.mMainMenuButton, !this.mInGame);
			this.HideButton(this.mCreditsButton, this.mInGame);
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x0009CB80 File Offset: 0x0009AD80
		private void InitSize()
		{
			if (this.mInGame)
			{
				this.Resize(0, 0, ZumasRevenge.Common._S(ZumasRevenge.Common._M(690)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(230)) + this.mHeightPad);
				return;
			}
			this.Resize(0, 0, ZumasRevenge.Common._S(ZumasRevenge.Common._M(600)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(230)) + this.mHeightPad - 80);
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0009CBF4 File Offset: 0x0009ADF4
		private ButtonWidget InitButton(int inButtonID, string inButtonName)
		{
			ButtonWidget buttonWidget = ZumasRevenge.Common.MakeButton(inButtonID, this, inButtonName);
			buttonWidget.mDoFinger = true;
			return buttonWidget;
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x0009CC12 File Offset: 0x0009AE12
		private void HideButton(ButtonWidget inButton, bool inHide)
		{
			inButton.SetVisible(!inHide);
			inButton.mDisabled = inHide;
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x0009CC28 File Offset: 0x0009AE28
		private void LayoutMainMenuDialog()
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BOX_MAINMENU_SLIDEBOXBACK);
			int num = base.GetLeft() - this.mX;
			int num2 = base.GetTop() - this.mY;
			int width = base.GetWidth();
			int num3 = width / 2;
			int num4 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(8));
			int theY = num2 - ZumasRevenge.Common._DS(ZumasRevenge.Common._M(70));
			this.mMusicVolumeSlider.Resize(num + num4 / 2 + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(10)), theY, num3 - ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(24)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M2(94)));
			this.mSfxVolumeSlider.Layout(17411, this.mMusicVolumeSlider, ZumasRevenge.Common._DS(ZumasRevenge.Common._M(25)), 0, 0, 0);
			this.mColorBlindSlider.Resize((this.mMusicVolumeSlider.mX + this.mSfxVolumeSlider.mX + this.mSfxVolumeSlider.mWidth) / 2 - imageByID.GetWidth() / 2, this.mMusicVolumeSlider.mY + ZumasRevenge.Common._S(45), imageByID.GetWidth(), imageByID.GetHeight());
			int num5 = ZumasRevenge.Common._DS(10);
			int theX = (this.mWidth - (OptionsDialog.OPTIONS_BUTTON_WIDTH * 3 + num5)) / 2;
			this.mCreditsButton.Resize(theX, this.mColorBlindSlider.mY + ZumasRevenge.Common._S(90), OptionsDialog.OPTIONS_BUTTON_WIDTH, OptionsDialog.OPTIONS_BUTTON_HEIGHT);
			this.mHelpButton.Resize(this.mCreditsButton.mX + this.mCreditsButton.mWidth + num5, this.mCreditsButton.mY, OptionsDialog.OPTIONS_BUTTON_WIDTH, OptionsDialog.OPTIONS_BUTTON_HEIGHT);
			this.HideButton(this.mHelpButton, true);
			int num6 = 200;
			this.mBackToGame.Resize(this.mHelpButton.mX + num6, this.mHelpButton.mY, OptionsDialog.OPTIONS_BUTTON_WIDTH, OptionsDialog.OPTIONS_BUTTON_HEIGHT);
			this.mMainMenuButton.Layout(16387, this.mBackToGame, 0, 0, 0, 0);
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0009CE1C File Offset: 0x0009B01C
		private void LayoutAdventureDialog()
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BOX_MAINMENU_SLIDEBOXBACK);
			int num = base.GetLeft() - this.mX;
			int num2 = base.GetTop() - this.mY;
			int width = base.GetWidth();
			int num3 = width / 2;
			ZumasRevenge.Common._S(ZumasRevenge.Common._M(8));
			int num4 = num2 - ZumasRevenge.Common._S(ZumasRevenge.Common._M(40));
			this.mMusicVolumeSlider.Resize(num + ZumasRevenge.Common._S(ZumasRevenge.Common._M(340)), num4 - 7, num3 - ZumasRevenge.Common._S(ZumasRevenge.Common._M1(24)), ZumasRevenge.Common._S(ZumasRevenge.Common._M2(44)));
			this.mSfxVolumeSlider.Layout(4611, this.mMusicVolumeSlider, ZumasRevenge.Common._S(ZumasRevenge.Common._M(0)), ZumasRevenge.Common._S(37), 0, 0);
			this.mColorBlindSlider.Resize(this.mSfxVolumeSlider.mX - ZumasRevenge.Common._S(80), this.mSfxVolumeSlider.mY + ZumasRevenge.Common._S(45), imageByID.GetWidth(), imageByID.GetHeight());
			this.mHelpButton.Resize(10 + this.mSfxVolumeSlider.mX + ZumasRevenge.Common._S(115), this.mColorBlindSlider.mY + ZumasRevenge.Common._S(100), OptionsDialog.OPTIONS_BUTTON_WIDTH, OptionsDialog.OPTIONS_BUTTON_HEIGHT);
			this.mBackToGame.Resize(10 + this.mHelpButton.mX - this.mHelpButton.mWidth - ZumasRevenge.Common._S(50), this.mHelpButton.mY, OptionsDialog.OPTIONS_BUTTON_WIDTH, OptionsDialog.OPTIONS_BUTTON_HEIGHT);
			this.mMainMenuButton.Resize(10 + this.mBackToGame.mX - this.mBackToGame.mWidth + ZumasRevenge.Common._S(-50), this.mBackToGame.mY, OptionsDialog.OPTIONS_BUTTON_WIDTH, OptionsDialog.OPTIONS_BUTTON_HEIGHT);
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x0009CFDC File Offset: 0x0009B1DC
		private void SetMusicSlider(double inVolume)
		{
			if (this.mMusicEnabled)
			{
				GameApp.gApp.SetMusicVolume(inVolume);
			}
			if (this.mMusicVolumeSlider.mDragging)
			{
				return;
			}
			this.mMusicSliderOn = this.mMusicEnabled && inVolume > 0.0;
			this.mMusicVolumeSlider.Label = (this.mMusicSliderOn ? TextManager.getInstance().getString(672) : TextManager.getInstance().getString(682));
			this.mMusicVolumeSlider.mDisabled = !this.mMusicEnabled;
			GameApp.gApp.mMusic.Enable(this.mMusicSliderOn);
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0009D083 File Offset: 0x0009B283
		private void SetSfxSlider(double inVolume)
		{
			GameApp.gApp.SetSfxVolume(inVolume);
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x0009D090 File Offset: 0x0009B290
		public void SliderReleased(int theId, double theVal)
		{
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x0009D092 File Offset: 0x0009B292
		public void OnLegalInfoHided()
		{
			this.mState = OptionsDialog.OptionState.OptionState_None;
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0009D09B File Offset: 0x0009B29B
		public void OnCreditsHided()
		{
			this.mState = OptionsDialog.OptionState.OptionState_None;
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0009D0A4 File Offset: 0x0009B2A4
		public void OnHelpHided()
		{
			this.mState = OptionsDialog.OptionState.OptionState_None;
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0009D0B0 File Offset: 0x0009B2B0
		public void ProcessHardwareBackButton()
		{
			switch (this.mState)
			{
			case OptionsDialog.OptionState.OptionState_OptionToMainMenuPrompt:
			{
				this.mState = OptionsDialog.OptionState.OptionState_None;
				Dialog dialog = GameApp.gApp.GetDialog(1);
				if (dialog != null)
				{
					dialog.ButtonDepress(1001);
				}
				GameApp.gApp.OnHardwareBackButtonPressProcessed();
				return;
			}
			case OptionsDialog.OptionState.OptionState_Credits:
				this.mState = OptionsDialog.OptionState.OptionState_None;
				GameApp.gApp.ReturnFromCredits();
				GameApp.gApp.OnHardwareBackButtonPressProcessed();
				return;
			case OptionsDialog.OptionState.OptionState_Help:
			{
				this.mState = OptionsDialog.OptionState.OptionState_None;
				Board board = GameApp.gApp.GetBoard();
				if (board != null && board.GauntletMode())
				{
					board.ChallengeHelpClosed();
				}
				else
				{
					GameApp.gApp.mGenericHelp.ButtonDepress(0);
				}
				GameApp.gApp.OnHardwareBackButtonPressProcessed();
				return;
			}
			case OptionsDialog.OptionState.OptionState_Legal:
				GameApp.gApp.mLegalInfo.ProcessHardwareBackButton();
				if (GameApp.gApp.mLegalInfo == null)
				{
					this.mState = OptionsDialog.OptionState.OptionState_None;
					return;
				}
				break;
			default:
				this.mState = OptionsDialog.OptionState.OptionState_None;
				this.SetMusicSlider(this.mOriginMusicVolume);
				this.SetSfxSlider(this.mOriginSfxVolume);
				GameApp.gApp.FinishOptionsDialog(false);
				GameApp.gApp.OnHardwareBackButtonPressProcessed();
				break;
			}
		}

		// Token: 0x04000EAE RID: 3758
		private const double MUSIC_SLIDER_THRESHOLD = 0.01;

		// Token: 0x04000EAF RID: 3759
		private static int OPTIONS_BUTTON_WIDTH = ZumasRevenge.Common._DS(372);

		// Token: 0x04000EB0 RID: 3760
		private static int OPTIONS_BUTTON_HEIGHT = ZumasRevenge.Common._DS(157);

		// Token: 0x04000EB1 RID: 3761
		private static int INCLUDE_LANGUAGE_BUTTON = 0;

		// Token: 0x04000EB2 RID: 3762
		public ZumaSlider mMusicVolumeSlider;

		// Token: 0x04000EB3 RID: 3763
		public ZumaSlider mSfxVolumeSlider;

		// Token: 0x04000EB4 RID: 3764
		public ZumaSlideBox mColorBlindSlider;

		// Token: 0x04000EB5 RID: 3765
		public double mOriginMusicVolume;

		// Token: 0x04000EB6 RID: 3766
		public double mOriginSfxVolume;

		// Token: 0x04000EB7 RID: 3767
		public bool mOriginColorBlind;

		// Token: 0x04000EB8 RID: 3768
		public ButtonWidget mHelpButton;

		// Token: 0x04000EB9 RID: 3769
		public ButtonWidget mMainMenuButton;

		// Token: 0x04000EBA RID: 3770
		public ButtonWidget mBackToGame;

		// Token: 0x04000EBB RID: 3771
		public ButtonWidget mCreditsButton;

		// Token: 0x04000EBC RID: 3772
		public ButtonWidget mLanguageButton;

		// Token: 0x04000EBD RID: 3773
		public bool mInGame;

		// Token: 0x04000EBE RID: 3774
		public bool mMusicEnabled;

		// Token: 0x04000EBF RID: 3775
		public bool mMusicSliderOn;

		// Token: 0x04000EC0 RID: 3776
		public int mHeightPad;

		// Token: 0x04000EC1 RID: 3777
		protected OptionsDialog.OptionState mState;

		// Token: 0x02000126 RID: 294
		public enum ControlId
		{
			// Token: 0x04000EC3 RID: 3779
			OptionsDialog_MusicVolume,
			// Token: 0x04000EC4 RID: 3780
			OptionsDialog_SfxVolume,
			// Token: 0x04000EC5 RID: 3781
			OptionsDialog_Help,
			// Token: 0x04000EC6 RID: 3782
			OptionsDialog_ToMainMenu,
			// Token: 0x04000EC7 RID: 3783
			OptionsDialog_Colorblind,
			// Token: 0x04000EC8 RID: 3784
			OptionsDialog_Credits,
			// Token: 0x04000EC9 RID: 3785
			OptionsDialog_Language,
			// Token: 0x04000ECA RID: 3786
			OptionsDialog_Legal,
			// Token: 0x04000ECB RID: 3787
			OptionsDialog_BackToGame
		}

		// Token: 0x02000127 RID: 295
		protected enum OptionState
		{
			// Token: 0x04000ECD RID: 3789
			OptionState_BackToMainMenuPrompt,
			// Token: 0x04000ECE RID: 3790
			OptionState_OptionToMainMenuPrompt,
			// Token: 0x04000ECF RID: 3791
			OptionState_Credits,
			// Token: 0x04000ED0 RID: 3792
			OptionState_Help,
			// Token: 0x04000ED1 RID: 3793
			OptionState_Legal,
			// Token: 0x04000ED2 RID: 3794
			OptionState_None
		}
	}
}
