using System;
using System.Collections.Generic;
using System.Text;
using JeffLib;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000146 RID: 326
	public class TikiTemplePages : Widget
	{
		// Token: 0x06001019 RID: 4121 RVA: 0x000A14D0 File Offset: 0x0009F6D0
		public TikiTemplePages(TikiTemple theTikiTemple)
		{
			this.mTikiTemple = theTikiTemple;
			this.mNumPages = 0;
			this.mHeaderFont = Res.GetFontByID(ResID.FONT_SHAGEXOTICA38_BLACK_GLOW);
			this.mStatsFont = Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_STROKE);
			this.mText = new List<TempleText>();
			this.AddPage(TikiTemplePages.PageInfo.TikiTemple_PageAdventure);
			this.AddPage(TikiTemplePages.PageInfo.TikiTemple_PageChallenge);
			this.AddPage(TikiTemplePages.PageInfo.TikiTemple_PageStats);
			this.AddPage(TikiTemplePages.PageInfo.TikiTemple_PageMoreStats);
			for (int i = 0; i < this.mText.Count; i++)
			{
				this.mText[i].mAlpha = 255f;
			}
			this.Resize(0, 0, (this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth() + 30) * this.mNumPages, this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetHeight());
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x000A15A6 File Offset: 0x0009F7A6
		public int NumPages()
		{
			return this.mNumPages;
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x000A15B0 File Offset: 0x0009F7B0
		private void AddPage(TikiTemplePages.PageInfo thePage)
		{
			int num = (this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth() + 30) * this.mNumPages + (int)this.mTikiTemple.GetTitleXOffset() + 55;
			switch (thePage)
			{
			case TikiTemplePages.PageInfo.TikiTemple_PageStats:
				this.SetupStatsText(ref num);
				break;
			case TikiTemplePages.PageInfo.TikiTemple_PageMoreStats:
				this.SetupMoreStatsText(ref num);
				break;
			case TikiTemplePages.PageInfo.TikiTemple_PageChallenge:
				this.SetupChallengeText(ref num);
				break;
			case TikiTemplePages.PageInfo.TikiTemple_PageAdventure:
				this.SetupAdventureText(false, ref num);
				break;
			}
			this.mNumPages++;
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x000A1634 File Offset: 0x0009F834
		public override void Draw(Graphics g)
		{
			for (int i = 0; i < this.mText.Count; i++)
			{
				TempleText templeText = this.mText[i];
				if (templeText.mValueStr.Length == 0)
				{
					g.SetFont(this.mHeaderFont);
					g.SetColor(255, 249, 161);
					g.DrawString(templeText.mHeaderStr, templeText.mX + (int)this.mTikiTemple.mXOff, templeText.mY + g.GetFont().GetAscent());
				}
				else
				{
					g.SetFont(this.mStatsFont);
					g.SetColor(166, 158, 255);
					g.WriteString(templeText.mHeaderStr, templeText.mX + (int)this.mTikiTemple.mXOff, templeText.mY + this.mStatsFont.GetAscent(), 0, 1);
					g.SetColor(89, 187, 149);
					g.WriteString(templeText.mValueStr, templeText.mX + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(20)) + (int)this.mTikiTemple.mXOff, templeText.mY + this.mStatsFont.GetAscent(), this.mWidth, -1);
				}
			}
			int num = this.NumPages() - 1;
			int num2 = this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth() + 30;
			int theY = (this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetHeight() - this.IMAGE_UI_CHALLENGESCREEN_DIVIDER.GetHeight()) / 2;
			for (int j = 0; j < num; j++)
			{
				g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_DIVIDER, num2 - this.IMAGE_UI_CHALLENGESCREEN_DIVIDER.GetWidth(), theY);
				num2 += this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth() + 30;
			}
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x000A17EC File Offset: 0x0009F9EC
		public override void Update()
		{
			float num = ZumasRevenge.Common._M(10f);
			for (int i = 0; i < this.mText.size<TempleText>(); i++)
			{
				TempleText templeText = this.mText[i];
				if (templeText.mFadeIn && templeText.mAlpha < 255f)
				{
					this.MarkDirty();
					templeText.mAlpha += num;
					if (templeText.mAlpha > 255f)
					{
						templeText.mAlpha = 255f;
					}
				}
				else if (!templeText.mFadeIn && templeText.mAlpha > 0f)
				{
					this.MarkDirty();
					templeText.mAlpha -= num;
					if (templeText.mAlpha <= 0f)
					{
						this.mText.RemoveAt(i);
						i--;
					}
				}
			}
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x000A18B8 File Offset: 0x0009FAB8
		private void SetupIronFrogText(ref int theStartX)
		{
			int mX = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(832)) - this.mX + (GameApp.gApp.GetScreenWidth() - GameApp.gApp.mScreenBounds.mWidth);
			int num = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(6)) + this.mStatsFont.mHeight;
			IronFrogTempleStats mIronFrogStats = GameApp.gApp.mUserProfile.mIronFrogStats;
			this.mText.Add(new TempleText());
			TempleText templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(791);
			templeText.mX = (GameApp.gApp.GetScreenWidth() - this.mHeaderFont.StringWidth(templeText.mHeaderStr)) / 2;
			templeText.mY = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(64));
			int num2 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(416));
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(792);
			templeText.mValueStr = string.Format("{0:D}", mIronFrogStats.mNumAttempts);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(793);
			templeText.mValueStr = string.Format("{0:D}", mIronFrogStats.mNumVictories);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(794);
			templeText.mValueStr = ((mIronFrogStats.mBestTime == 0) ? "None" : JeffLib.Common.UpdateToTimeStr(mIronFrogStats.mBestTime, false));
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(795);
			templeText.mValueStr = SexyFramework.Common.CommaSeperate(mIronFrogStats.mBestScore);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(796);
			templeText.mValueStr = string.Format("{0:D}", mIronFrogStats.mHighestLevel);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(797);
			int num3 = 0;
			int num4 = -1;
			for (int i = 0; i < 10; i++)
			{
				if (mIronFrogStats.mLevelDeaths[i] > num3)
				{
					num3 = mIronFrogStats.mLevelDeaths[i];
					num4 = i;
				}
			}
			if (num4 == -1)
			{
				templeText.mValueStr = TextManager.getInstance().getString(771);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder(TextManager.getInstance().getString(798));
				stringBuilder.Replace("$1", (num4 + 1).ToString());
				stringBuilder.Replace("$2", num3.ToString());
				templeText.mValueStr = stringBuilder.ToString();
			}
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(799);
			templeText.mValueStr = JeffLib.Common.UpdateToTimeStr(mIronFrogStats.mTotalTimePlayed, true);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x000A1CB8 File Offset: 0x0009FEB8
		private void SetupStatsText(ref int theStartX)
		{
			int mX = theStartX + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(732));
			int num = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(6)) + this.mStatsFont.mHeight;
			ChallengeTempleStats mChallengeStats = GameApp.gApp.mUserProfile.mChallengeStats;
			this.mText.Add(new TempleText());
			TempleText templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(800);
			templeText.mX = theStartX - ZumasRevenge.Common._DS(60) + (this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth() + 30 - this.mHeaderFont.StringWidth(templeText.mHeaderStr)) / 2;
			templeText.mY = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(64));
			int num2 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(140));
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(801);
			templeText.mValueStr = string.Format("{0:D}", GameApp.gApp.mUserProfile.mLargestChainShot);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(802);
			templeText.mValueStr = string.Format("{0:D}", GameApp.gApp.mUserProfile.mLargestCombo);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(803);
			templeText.mValueStr = string.Format("{0:D}", GameApp.gApp.mUserProfile.mHighestGapShotScore);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(804);
			templeText.mValueStr = string.Format("{0:D}", GameApp.gApp.mUserProfile.mNumGapShots);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(805);
			templeText.mValueStr = string.Format("{0:D}", GameApp.gApp.mUserProfile.mNumDoubleGapShots);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(806);
			templeText.mValueStr = string.Format("{0:D}", GameApp.gApp.mUserProfile.mNumTripleGapShots);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(807);
			templeText.mValueStr = string.Format("{0:D}", GameApp.gApp.mUserProfile.mNumFruits);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(808);
			templeText.mValueStr = string.Format("{0:D}", GameApp.gApp.mUserProfile.mNumTimesActivatedPowerup[0]);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(809);
			templeText.mValueStr = string.Format("{0:D}", GameApp.gApp.mUserProfile.mNumTimesActivatedPowerup[9]);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(810);
			templeText.mValueStr = string.Format("{0:D}", GameApp.gApp.mUserProfile.mNumTimesActivatedPowerup[8]);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(811);
			templeText.mValueStr = string.Format("{0:D}", GameApp.gApp.mUserProfile.mNumTimesActivatedPowerup[7]);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x000A21FC File Offset: 0x000A03FC
		private void SetupMoreStatsText(ref int theStartX)
		{
			int mX = theStartX + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(732));
			int num = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(6)) + this.mStatsFont.mHeight;
			ChallengeTempleStats mChallengeStats = GameApp.gApp.mUserProfile.mChallengeStats;
			this.mText.Add(new TempleText());
			TempleText templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(812);
			templeText.mX = theStartX - ZumasRevenge.Common._DS(60) + (this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth() + 30 - this.mHeaderFont.StringWidth(templeText.mHeaderStr)) / 2;
			templeText.mY = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(64));
			int num2 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(140));
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(813);
			templeText.mValueStr = string.Format("{0:D}", GameApp.gApp.mUserProfile.mBallsSwapped);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(814);
			templeText.mValueStr = string.Format("{0:D}", GameApp.gApp.mUserProfile.mBallsFired);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			int num3 = 0;
			int t = 0;
			for (int i = 0; i < 14; i++)
			{
				if (!ZumasRevenge.Common.IsDeprecatedPowerUp((PowerType)i) && GameApp.gApp.mUserProfile.mNumTimesActivatedPowerup[i] > num3)
				{
					num3 = GameApp.gApp.mUserProfile.mNumTimesActivatedPowerup[i];
					t = i;
				}
			}
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(815);
			templeText.mValueStr = ((num3 <= 0) ? TextManager.getInstance().getString(771) : string.Format("{0} ({1:D}x)", ZumasRevenge.Common.PowerupToStr((PowerType)t, false), num3));
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(816);
			templeText.mValueStr = SexyFramework.Common.CommaSeperate(GameApp.gApp.mUserProfile.mPointsFromCombos);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(817);
			templeText.mValueStr = SexyFramework.Common.CommaSeperate(GameApp.gApp.mUserProfile.mPointsFromChainShots);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(818);
			templeText.mValueStr = SexyFramework.Common.CommaSeperate(GameApp.gApp.mUserProfile.mPointsFromGapShots);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			int theValue = GameApp.gApp.mUserProfile.mPointsFromCannon + GameApp.gApp.mUserProfile.mPointsFromColorNuke + GameApp.gApp.mUserProfile.mPointsFromLaser + GameApp.gApp.mUserProfile.mPointsFromProxBomb;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(819);
			templeText.mValueStr = SexyFramework.Common.CommaSeperate(theValue);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			int u = GameApp.gApp.mUserProfile.mAdventureStats.mTotalTimePlayed + GameApp.gApp.mUserProfile.mHeroicStats.mTotalTimePlayed + GameApp.gApp.mUserProfile.mIronFrogStats.mTotalTimePlayed + GameApp.gApp.mUserProfile.mChallengeStats.mTotalTime;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(820);
			templeText.mValueStr = JeffLib.Common.UpdateToTimeStr(u, true);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(821);
			templeText.mValueStr = string.Format("{0:D}", GameApp.gApp.mUserProfile.mFruitBombed);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(822);
			templeText.mValueStr = string.Format("{0:D}", GameApp.gApp.mUserProfile.mBallsTossed);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(823);
			templeText.mValueStr = string.Format("{0:D}", GameApp.gApp.mUserProfile.mDeathsAfterZuma);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x000A27E4 File Offset: 0x000A09E4
		private void SetupChallengeText(ref int theStartX)
		{
			int mX = theStartX + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(732));
			int num = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(6)) + this.mStatsFont.mHeight;
			ChallengeTempleStats mChallengeStats = GameApp.gApp.mUserProfile.mChallengeStats;
			this.mText.Add(new TempleText());
			TempleText templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(782);
			templeText.mX = theStartX - ZumasRevenge.Common._DS(60) + (this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth() + 30 - this.mHeaderFont.StringWidth(templeText.mHeaderStr)) / 2;
			templeText.mY = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(64));
			int num2 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(140));
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(783);
			templeText.mValueStr = SexyFramework.Common.CommaSeperate(mChallengeStats.mHighestScore);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < 7; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					if (GameApp.gApp.mUserProfile.mChallengeUnlockState[i, j] == 4)
					{
						num3++;
					}
					else if (GameApp.gApp.mUserProfile.mChallengeUnlockState[i, j] == 5)
					{
						num3++;
						num4++;
					}
				}
			}
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(784);
			templeText.mValueStr = string.Format("{0:D} / 60", num3);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(785);
			templeText.mValueStr = string.Format("{0:D} / 60", num4);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			int num5 = 0;
			int num6 = 0;
			for (int k = 1; k < 7; k++)
			{
				if (GameApp.gApp.mUserProfile.ChallengeCupComplete(k) == 2)
				{
					num5++;
					num6++;
				}
				else if (GameApp.gApp.mUserProfile.ChallengeCupComplete(k) == 1)
				{
					num5++;
				}
			}
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(786);
			templeText.mValueStr = string.Format("{0:D} / 6", num5);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(787);
			templeText.mValueStr = string.Format("{0:D} / 6", num6);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(788);
			templeText.mValueStr = string.Format("x{0:D}", mChallengeStats.mHighestMult);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(789);
			int num7 = 0;
			int num8 = 0;
			for (int l = 0; l < 70; l++)
			{
				if (mChallengeStats.mNumTimesPlayedCurve[l] > num7)
				{
					num7 = mChallengeStats.mNumTimesPlayedCurve[l];
					num8 = l + 1;
				}
			}
			StringBuilder stringBuilder = new StringBuilder(TextManager.getInstance().getString(790));
			stringBuilder.Replace("$1", num8.ToString());
			stringBuilder.Replace("$2", num7.ToString());
			templeText.mValueStr = stringBuilder.ToString();
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(780);
			templeText.mValueStr = JeffLib.Common.UpdateToTimeStr(mChallengeStats.mTotalTime, true);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x000A2CC0 File Offset: 0x000A0EC0
		private void SetupAdventureText(bool hard_mode, ref int theStartX)
		{
			int mX = theStartX + ZumasRevenge.Common._DS(ZumasRevenge.Common._M(732));
			int num = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(6)) + this.mStatsFont.mHeight;
			AdvModeTempleStats advModeTempleStats = (hard_mode ? GameApp.gApp.mUserProfile.mHeroicStats : GameApp.gApp.mUserProfile.mAdventureStats);
			this.mText.Add(new TempleText());
			TempleText templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = (hard_mode ? TextManager.getInstance().getString(766) : TextManager.getInstance().getString(767));
			templeText.mX = theStartX - ZumasRevenge.Common._DS(60) + (this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth() + 30 - this.mHeaderFont.StringWidth(templeText.mHeaderStr)) / 2;
			templeText.mY = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(64));
			int num2 = ZumasRevenge.Common._DS(ZumasRevenge.Common._M(140));
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(768);
			templeText.mValueStr = string.Format("{0:D}", advModeTempleStats.mHighestLevel);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(769);
			templeText.mValueStr = ((advModeTempleStats.mBestTime <= 0 || advModeTempleStats.mBestTime == int.MaxValue) ? TextManager.getInstance().getString(771) : JeffLib.Common.UpdateToTimeStr(advModeTempleStats.mBestTime, true));
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(770);
			templeText.mValueStr = ((advModeTempleStats.mBestScore <= 0) ? TextManager.getInstance().getString(771) : SexyFramework.Common.CommaSeperate(advModeTempleStats.mBestScore));
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(772);
			templeText.mValueStr = string.Format("{0:D}", advModeTempleStats.mNumLevelsAced);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(773);
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			for (int i = 0; i < 60; i++)
			{
				num3 += advModeTempleStats.mLevelDeaths[i];
				if (advModeTempleStats.mLevelDeaths[i] > num5)
				{
					num5 = advModeTempleStats.mLevelDeaths[i];
					num4 = i + 1;
				}
			}
			templeText.mValueStr = string.Format("{0:D}", num3);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(774);
			templeText.mValueStr = string.Format("{0:D}", advModeTempleStats.mNumPerfectLevels);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(775);
			templeText.mValueStr = string.Format("{0:D}", advModeTempleStats.mNumClearCurves);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(776);
			int num6 = 0;
			int num7 = 0;
			for (int j = 0; j < 6; j++)
			{
				if (advModeTempleStats.mBossDeaths[j] > num7)
				{
					num6 = j + 1;
					num7 = advModeTempleStats.mBossDeaths[j];
				}
			}
			if (num6 == 0)
			{
				templeText.mValueStr = TextManager.getInstance().getString(771);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder(TextManager.getInstance().getString(778));
				stringBuilder.Replace("$1", num6.ToString());
				stringBuilder.Replace("$2", num7.ToString());
				templeText.mValueStr = stringBuilder.ToString();
			}
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(777);
			if (num4 == 0)
			{
				templeText.mValueStr = TextManager.getInstance().getString(771);
			}
			else
			{
				StringBuilder stringBuilder2 = new StringBuilder(TextManager.getInstance().getString(779));
				stringBuilder2.Replace("$1", num4.ToString());
				stringBuilder2.Replace("$2", num5.ToString());
				templeText.mValueStr = stringBuilder2.ToString();
			}
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
			this.mText.Add(new TempleText());
			templeText = this.mText.back<TempleText>();
			templeText.mHeaderStr = TextManager.getInstance().getString(780);
			templeText.mValueStr = JeffLib.Common.UpdateToTimeStr(advModeTempleStats.mTotalTimePlayed, true);
			templeText.mX = mX;
			templeText.mY = num2;
			num2 += num;
		}

		// Token: 0x0400170C RID: 5900
		private List<TempleText> mText;

		// Token: 0x0400170D RID: 5901
		private Font mHeaderFont;

		// Token: 0x0400170E RID: 5902
		private Font mStatsFont;

		// Token: 0x0400170F RID: 5903
		private TikiTemple mTikiTemple;

		// Token: 0x04001710 RID: 5904
		private int mNumPages;

		// Token: 0x04001711 RID: 5905
		private Image IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES);

		// Token: 0x04001712 RID: 5906
		private Image IMAGE_UI_CHALLENGESCREEN_DIVIDER = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_DIVIDER);

		// Token: 0x02000147 RID: 327
		private enum PageInfo
		{
			// Token: 0x04001714 RID: 5908
			TikiTemple_PageStats,
			// Token: 0x04001715 RID: 5909
			TikiTemple_PageMoreStats,
			// Token: 0x04001716 RID: 5910
			TikiTemple_PageChallenge,
			// Token: 0x04001717 RID: 5911
			TikiTemple_PageAdventure
		}
	}
}
