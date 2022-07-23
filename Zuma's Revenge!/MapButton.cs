using System;
using JeffLib;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x0200011D RID: 285
	public class MapButton : ExtraSexyButton
	{
		// Token: 0x06000EB5 RID: 3765 RVA: 0x00097C2D File Offset: 0x00095E2D
		public MapButton(int id, ButtonListener l)
			: base(id, l)
		{
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x00097C38 File Offset: 0x00095E38
		public override void Draw(Graphics g)
		{
			bool flag = this.mIsDown && this.mIsOver && !this.mDisabled;
			flag ^= this.mInverted;
			int num = 0;
			if (flag)
			{
				num = 1;
			}
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGEXOTICA38_GREEN_STROKE);
			Font fontByID2 = Res.GetFontByID(ResID.FONT_SHAGEXOTICA38_BLACK_GLOW);
			Font fontByID3 = Res.GetFontByID(ResID.FONT_SHAGLOUNGE28_STROKE_GREEN);
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_MAP_CONTINUE_BUTTON);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_UI_MAP_CONTINUE);
			Image imageByID3 = Res.GetImageByID(ResID.IMAGE_UI_MAP_RIMGLOW);
			Image imageByID4 = Res.GetImageByID(ResID.IMAGE_UI_MAP_LEVELINFO);
			Image imageByID5 = Res.GetImageByID(ResID.IMAGE_UI_MAP_STARTNEWGAME);
			g.ClearClipRect();
			g.SetClipRect(0, 0, this.GetRect().mWidth, this.GetRect().mHeight);
			g.SetFont(fontByID2);
			g.SetColor(Common._M(255), Common._M1(255), Common._M2(255), (int)((float)Common._M3(255) * (float)this.mMapScreen.mAlpha));
			g.SetColorizeImages(true);
			int num2 = 0;
			int num3 = 6;
			g.DrawImage(imageByID, Common._DS(Common._M(0)), Common._DS(Common._M1(num3)));
			int num4 = Res.GetOffsetXByID(ResID.IMAGE_UI_MAP_CONTINUE) - Res.GetOffsetXByID(ResID.IMAGE_UI_MAP_CONTINUE_BUTTON);
			int num5 = Res.GetOffsetYByID(ResID.IMAGE_UI_MAP_CONTINUE) - Res.GetOffsetYByID(ResID.IMAGE_UI_MAP_CONTINUE_BUTTON);
			if (((!GameApp.gApp.mUserProfile.mFirstTimeReplayingNormalMode && !GameApp.gApp.mClickedHardMode) || (GameApp.gApp.mClickedHardMode && !GameApp.gApp.mUserProfile.mFirstTimeReplayingHardMode)) && !GameApp.gApp.mUserProfile.GetAdvModeVars().mFirstTimeInZone[0])
			{
				g.DrawImageCel(imageByID2, Common._DS(Common._M(num2 + num4)), Common._DS(Common._M1(num3 + num5)), num);
				if (num == 0)
				{
					int num6 = Common._M(0) + JeffLib.Common.GetAlphaFromUpdateCount(this.mUpdateCnt, Common._M1(128));
					if (num6 > 255)
					{
						num6 = 255;
					}
					else if (num6 < 0)
					{
						num6 = 0;
					}
					int num7 = 255;
					if (num7 < num6)
					{
						num6 = num7;
					}
					g.PushState();
					g.SetColor(255, 255, 0, num6);
					g.DrawImageCel(imageByID2, Common._DS(Common._M(num2 + num4)), Common._DS(Common._M1(num3 + num5)), num);
					g.SetDrawMode(1);
					g.SetColor(255, 255, 255, num6);
					g.DrawImage(imageByID3, Common._DS(Common._M(-2)), Common._DS(Common._M1(0)));
					g.PopState();
				}
				if (this.mScore.Length > 0)
				{
					g.DrawImage(imageByID4, Common._DS(Common._M(124)), Common._DS(Common._M1(134)));
				}
				int num8 = 0;
				if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CHT || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_RU || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_PL)
				{
					num8 = 10;
				}
				g.SetColor(253, 220, 0, (int)this.mMapScreen.mAlpha.GetOutVal() * 255);
				g.SetFont(fontByID);
				if (this.mLevel.Length > 0)
				{
					if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_RU || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_PL || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_FR)
					{
						int num9 = 128;
						int num10 = 120;
						float num11 = 0.75f;
						Graphics3D graphics3D = g.Get3D();
						SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
						sexyTransform2D.Scale(num11, num11);
						sexyTransform2D.Translate((float)num9, (float)num10);
						graphics3D.PushTransform(sexyTransform2D);
						g.DrawString(this.mLevel, (this.mWidth - g.GetFont().StringWidth(this.mLevel)) / 2, num8 + Common._S(Common._M1(108)));
						graphics3D.PopTransform();
					}
					else
					{
						g.DrawString(this.mLevel, (this.mWidth - g.GetFont().StringWidth(this.mLevel)) / 2, num8 + Common._S(Common._M1(108)));
					}
				}
				g.SetColor(253, 48, 0, (int)this.mMapScreen.mAlpha.GetOutVal() * 255);
				if (this.mLives.Length > 0)
				{
					g.DrawString(this.mLives, (this.mLives.Length == 2) ? Common._DS(Common._M(242)) : Common._DS(Common._M1(230)), Common._DS(Common._M2(326)));
				}
				g.SetFont(fontByID3);
				if (this.mScore.Length > 0)
				{
					g.DrawString(this.mScore, (this.mWidth - g.GetFont().StringWidth(this.mScore)) / 2, Common._S(Common._M1(133)));
					return;
				}
			}
			else if (!this.mMapScreen.mRemove)
			{
				if (num == 0)
				{
					int num12 = Common._M(0) + JeffLib.Common.GetAlphaFromUpdateCount(this.mUpdateCnt, Common._M1(128));
					if (num12 > 255)
					{
						num12 = 255;
					}
					else if (num12 < 0)
					{
						num12 = 0;
					}
					int num13 = (int)this.mMapScreen.mAlpha * 255;
					if (num13 < num12)
					{
						num12 = num13;
					}
					g.PushState();
					g.SetColor(255, 255, 255, num12);
					g.DrawImage(imageByID3, Common._DS(Common._M(-2)), Common._DS(Common._M1(0)));
					g.PopState();
				}
				int num14 = 0;
				int num15 = 0;
				if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_PGB || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_PG || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_SPC)
				{
					num14 = 20;
					num15 = -20;
				}
				else if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_PL)
				{
					num14 = 30;
					num15 = -30;
				}
				else if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_SP)
				{
					num14 = 15;
					num15 = -10;
				}
				else if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_GR)
				{
					num14 = 16;
					num15 = 0;
				}
				else if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_RU)
				{
					num14 = 16;
					num15 = -5;
				}
				else if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_FR)
				{
					num14 = 17;
					num15 = -20;
				}
				else if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH || Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CHT)
				{
					num14 = 15;
					num15 = -15;
				}
				g.DrawImage(imageByID5, Common._DS(Common._M(130)) + num15, Common._DS(Common._M1(110)) + num14);
			}
		}

		// Token: 0x04000E62 RID: 3682
		public string mLevel;

		// Token: 0x04000E63 RID: 3683
		public string mScore;

		// Token: 0x04000E64 RID: 3684
		public string mLives;

		// Token: 0x04000E65 RID: 3685
		public MapScreen mMapScreen;
	}
}
