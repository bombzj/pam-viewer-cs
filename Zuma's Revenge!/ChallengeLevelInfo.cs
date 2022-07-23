using System;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000044 RID: 68
	public class ChallengeLevelInfo : ZumaDialog, ButtonListener
	{
		// Token: 0x0600064F RID: 1615 RVA: 0x00029B14 File Offset: 0x00027D14
		public ChallengeLevelInfo(ChallengeMenu aMenu)
			: base(10, false, "", "", "", 2)
		{
			this.mChallengeMenu = aMenu;
			this.mChallengeLevelNum = -1;
			this.mChallengeZone = -1;
			this.mLevelInfo = new CSDisplayItem();
			this.mLevelInfo.mLevelStr = this.mChallengeMenu.mDefaultStringContainer.NothingSelected();
			this.mChallengeLevelName = "";
			if (this.mYesButton != null)
			{
				this.mYesButton.mLabel = TextManager.getInstance().getString(455);
			}
			if (this.mNoButton != null)
			{
				this.mNoButton.mLabel = TextManager.getInstance().getString(458);
			}
			if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_RU || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_PL || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_PGB || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_PG || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_SP || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_SPC || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_GR)
			{
				this.lang_offset = -65;
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00029C06 File Offset: 0x00027E06
		public void SetLevelInfo(string aLevelStr, string aScoreStr, string aAceStr, int aNum)
		{
			this.mLevelInfo.mLevelStr = aLevelStr;
			this.mLevelInfo.mScoreStr = aScoreStr;
			this.mLevelInfo.mAceStr = aAceStr;
			this.mLevelInfo.mNum = aNum;
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00029C3C File Offset: 0x00027E3C
		public override void Update()
		{
			if (this.mLevelInfo.mFadeIn && this.mLevelInfo.mAlpha < 255f)
			{
				this.mLevelInfo.mAlpha += (float)Common._M(10);
				this.MarkDirty();
				if (this.mLevelInfo.mAlpha > 255f)
				{
					this.mLevelInfo.mAlpha = 255f;
				}
			}
			else if (!this.mLevelInfo.mFadeIn)
			{
				this.MarkDirty();
				this.mLevelInfo.mAlpha -= (float)Common._M(10);
				if (this.mLevelInfo.mAlpha <= 0f)
				{
					this.mLevelInfo.mAlpha = 0f;
				}
			}
			base.Update();
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00029D04 File Offset: 0x00027F04
		public override void Draw(Graphics g)
		{
			g.PushState();
			base.Draw(g);
			if (this.mChallengeLevelNum != -1)
			{
				Image levelThumbnail = GameApp.gApp.GetLevelThumbnail(this.mChallengeLevelNum);
				int num = (int)(Common._DS(ChallengeLevelInfo.thumbScale) * (float)levelThumbnail.mWidth * 1.8f);
				int num2 = (int)(Common._DS(ChallengeLevelInfo.thumbScale) * (float)levelThumbnail.mHeight * 1.8f);
				int num3 = this.mWidth - num - Common._DS(100);
				int num4 = (this.mHeight - num2) / 2 + Common._DS(30);
				g.DrawImage(levelThumbnail, num3, num4, num, num2);
				Common.DrawCommonDialogBorder(g, num3 - Common._DS(16), num4 - Common._DS(16), num + Common._DS(32), num2 + Common._DS(32));
				int num5 = 0;
				int num6 = Common._DS(Common._M(435)) + num5 + this.lang_offset;
				int num7 = num4 + g.GetFont().GetHeight();
				float mAlpha = this.mLevelInfo.mAlpha;
				g.SetColor(new SexyColor(Common._M(214), Common._M1(143), Common._M2(7), (int)this.mLevelInfo.mAlpha));
				int num8 = Common._DS(Common._M(10)) - this.mX / 2 + (GameApp.gApp.GetScreenWidth() - GameApp.gApp.mScreenBounds.mWidth);
				if (Common.StrEquals(this.mLevelInfo.mLevelStr, this.mChallengeMenu.mDefaultStringContainer.mDefaultStr))
				{
					if (GameApp.gLastZone != -1)
					{
						if (GameApp.gLastZone == 7 && GameApp.gApp.mUserProfile.mChallengeUnlockState[GameApp.gLastZone - 1, 0] == 0)
						{
							g.WriteWordWrapped(new Rect(num6 + num8, num7 + ((GameApp.gLastZone == 7) ? Common._DS(Common._M(-40)) : Common._DS(Common._M1(-40))), Common._DS(Common._M2(320)), 10000), this.mLevelInfo.mLevelStr, -1, 0);
						}
					}
					else
					{
						g.WriteWordWrapped(new Rect(num6 + num8, num7 + ((GameApp.gLastZone == 7) ? Common._DS(Common._M(-22)) : Common._DS(Common._M1(-40))), Common._DS(Common._M2(362)), 10000), this.mLevelInfo.mLevelStr, -1, 0);
					}
				}
				else if (Common.StrEquals(this.mLevelInfo.mLevelStr, this.mChallengeMenu.mDefaultStringContainer.NothingSelected()))
				{
					g.WriteWordWrapped(new Rect(Common._DS(Common._M(608)) + num5 + num8, num7 + Common._DS(Common._M1(0)), Common._DS(Common._M2(362)), 10000), this.mLevelInfo.mLevelStr, -1, 0);
				}
				else
				{
					int value = -210;
					float value2 = 75f;
					int value3 = 50;
					string @string = TextManager.getInstance().getString(420);
					string string2 = TextManager.getInstance().getString(421);
					string string3 = TextManager.getInstance().getString(422);
					float num9 = (float)g.GetFont().StringWidth(@string);
					float num10 = (float)g.GetFont().StringWidth(string2);
					float num11 = (float)g.GetFont().StringWidth(string3);
					float num12 = Math.Max(num11, Math.Max(num9, num10));
					float num13 = (float)(num6 + Common._DS(value));
					float num14 = (float)g.GetFont().StringWidth(this.mLevelInfo.mLevelStr);
					float num15 = num13 + (num12 - num9) / 2f;
					float num16 = num15 + (num9 - num14) / 2f;
					g.DrawString(@string, (int)num15, num7);
					g.DrawString(this.mLevelInfo.mLevelStr, (int)num16, num7 + Common._DS(value3));
					float num17 = (float)g.GetFont().StringWidth(this.mLevelInfo.mAceStr);
					float num18 = num13 + (num12 - num10) / 2f;
					float num19 = num18 + (num10 - num17) / 2f;
					g.DrawString(string2, (int)num18, num7 + g.GetFont().GetHeight() + (int)Common._DS(value2));
					g.DrawString(this.mLevelInfo.mAceStr, (int)num19, num7 + g.GetFont().GetHeight() + (int)Common._DS(value2) + Common._DS(value3));
					g.PushState();
					g.SetColor(new SexyColor(Common._M(220), Common._M1(220), 0, (int)this.mLevelInfo.mAlpha));
					float num20 = (float)g.GetFont().StringWidth(this.mLevelInfo.mScoreStr);
					float num21 = num13 + (num12 - num11) / 2f;
					float num22 = num21 + (num11 - num20) / 2f;
					g.DrawString(string3, (int)num21, num7 + g.GetFont().GetHeight() * 2 + (int)Common._DS(value2) * 2);
					g.DrawString(this.mLevelInfo.mScoreStr, (int)num22, num7 + g.GetFont().GetHeight() * 2 + (int)(Common._DS(value2) * 2f + (float)Common._DS(value3)));
					g.PopState();
				}
				this.DrawLevelName(g);
			}
			g.PopState();
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0002A260 File Offset: 0x00028460
		public void DrawLevelName(Graphics g)
		{
			int num = this.mChallengeLevelNum % 10 + this.mChallengeZone * 10 + this.mChallengeZone;
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_STROKE));
			g.SetColor(SexyColor.White);
			string theString = this.mChallengeLevelNum + 1 + " - " + GameApp.gApp.GetLevelMgr().mLevels[num].mDisplayName;
			int num2 = g.GetFont().StringWidth(theString);
			g.DrawString(theString, (this.mWidth - num2) / 2, Common._DS(320));
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0002A2FC File Offset: 0x000284FC
		public new virtual void ButtonDepress(int theId)
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			if (theId == 2000 + this.mId || theId == 1000)
			{
				if (this.mChallengeLevelNum != -1)
				{
					GameApp.gLastLevel = this.mChallengeLevelNum + 1;
					GameApp.gLastZone = this.mChallengeMenu.mChallengeScrollWidget.GetPageHorizontal() - 1;
					GameApp.gApp.mMainMenu.mGauntletModLevel_id = this.GetChallengeLevelName();
					GameApp.gApp.mBambooTransition.mTransitionDelegate = new BambooTransition.BambooTransitionDelegate(GameApp.gApp.mMainMenu.StartChallengeGame);
					GameApp.gApp.ToggleBambooTransition();
					return;
				}
			}
			else if (theId == 3000 + this.mId || theId == 1001)
			{
				this.mChallengeMenu.HideChallengeLevelInfo();
			}
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0002A3D4 File Offset: 0x000285D4
		public void SetLevel(int theZoneNum, int theLevelNum, string theLevelName)
		{
			this.mChallengeLevelNum = theLevelNum;
			this.mChallengeLevelName = theLevelName;
			this.mChallengeZone = theZoneNum;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0002A3EB File Offset: 0x000285EB
		public string GetChallengeLevelName()
		{
			return this.mChallengeLevelName;
		}

		// Token: 0x04000372 RID: 882
		private ChallengeMenu mChallengeMenu;

		// Token: 0x04000373 RID: 883
		private int mChallengeLevelNum;

		// Token: 0x04000374 RID: 884
		private string mChallengeLevelName;

		// Token: 0x04000375 RID: 885
		private int mChallengeZone;

		// Token: 0x04000376 RID: 886
		private CSDisplayItem mLevelInfo;

		// Token: 0x04000377 RID: 887
		private static float thumbScale = 2.3f;

		// Token: 0x04000378 RID: 888
		private static int borderXOff = 12;

		// Token: 0x04000379 RID: 889
		private static int borderYOff = 12;

		// Token: 0x0400037A RID: 890
		private int lang_offset;
	}
}
