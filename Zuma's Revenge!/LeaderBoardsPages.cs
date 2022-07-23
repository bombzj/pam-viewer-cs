using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.GamerServices;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x020000F0 RID: 240
	public class LeaderBoardsPages : Widget
	{
		// Token: 0x06000CE9 RID: 3305 RVA: 0x0007D034 File Offset: 0x0007B234
		public LeaderBoardsPages(LeaderBoards theLeaderBoards)
		{
			this.mLeaderBoards = theLeaderBoards;
			this.mNumPages = 0;
			this.mHeaderFont = Res.GetFontByID(ResID.FONT_SHAGLOUNGE45_GAUNTLET);
			this.mStatsFont = Res.GetFontByID(ResID.FONT_SHAGLOUNGE38_STROKE);
			this.mText = new List<LeaderBoardText>();
			for (int i = 0; i < this.mText.Count; i++)
			{
				this.mText[i].mAlpha = 255f;
			}
			this.Resize(0, 0, this.IMAGE_UI_LEADERBOARDS_SHADOW.GetWidth(), (this.IMAGE_UI_LEADERBOARDS_SHADOW.GetHeight() + 30) * this.mNumPages);
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0007D115 File Offset: 0x0007B315
		public int NumPages()
		{
			return this.mNumPages;
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0007D120 File Offset: 0x0007B320
		public void AddPage(int page, bool isUpdate, LeaderboardReader reader)
		{
			int num = (this.IMAGE_UI_LEADERBOARDS_SHADOW.GetHeight() - 40) * (this.mNumPages + 1);
			this.SetupLeaderboardsTextXLive(ref num, page, isUpdate, reader);
			if (!isUpdate)
			{
				this.mNumPages++;
			}
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0007D164 File Offset: 0x0007B364
		public void AddPage(int page, bool isUpdate)
		{
			int num = (this.IMAGE_UI_LEADERBOARDS_SHADOW.GetHeight() - 40) * (this.mNumPages + 1);
			this.SetupLeaderboardsText(ref num, page, isUpdate);
			if (!isUpdate)
			{
				this.mNumPages++;
			}
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0007D1A4 File Offset: 0x0007B3A4
		public override void Draw(Graphics g)
		{
			Graphics3D graphics3D = g.Get3D();
			graphics3D.SetMasking(Graphics3D.EMaskMode.MASKMODE_TEST_OUTSIDE);
			int num = 0;
			for (int i = 0; i < this.mText.Count; i++)
			{
				LeaderBoardText leaderBoardText = this.mText[i];
				int num2 = 0;
				int num3 = 0;
				if (leaderBoardText.mIcon == null && leaderBoardText.mShowIcon)
				{
					leaderBoardText.mIcon = Res.GetImageByID(ResID.IMAGE_UI_AVATAR);
				}
				if (leaderBoardText.mIcon != null)
				{
					num2 = leaderBoardText.mIcon.GetWidth() + 20;
					num3 = 35;
					g.DrawImage(leaderBoardText.mIcon, leaderBoardText.mX, leaderBoardText.mY);
				}
				if (leaderBoardText.mValueStr.Length == 0)
				{
					num++;
					g.SetFont(this.mHeaderFont);
					g.SetColor(255, 249, 161);
					if (num == 2)
					{
						if (leaderBoardText.mIcon == null)
						{
							num2 = 84;
						}
						int num4 = 30;
						Rect theRect = new Rect(num2 + leaderBoardText.mX + (int)this.mLeaderBoards.mXOff, leaderBoardText.mY + g.GetFont().GetAscent() - num4, 250, this.mStatsFont.GetHeight() * 2);
						g.WriteWordWrapped(theRect, leaderBoardText.mHeaderStr, 25, -1);
					}
					else if (num == 3)
					{
						int num5 = 170;
						g.DrawString(leaderBoardText.mHeaderStr, num2 + leaderBoardText.mX + (int)this.mLeaderBoards.mXOff + num5 - g.GetFont().StringWidth(leaderBoardText.mHeaderStr), num3 + leaderBoardText.mY + g.GetFont().GetAscent());
					}
					else
					{
						g.DrawString(leaderBoardText.mHeaderStr, num2 + leaderBoardText.mX + (int)this.mLeaderBoards.mXOff, num3 + leaderBoardText.mY + g.GetFont().GetAscent());
					}
					if ((i + 1) % 3 == 0)
					{
						num = 0;
					}
				}
				else
				{
					g.SetFont(this.mStatsFont);
					g.SetColor(166, 158, 255);
					g.WriteString(leaderBoardText.mHeaderStr, num2 + leaderBoardText.mX + (int)this.mLeaderBoards.mXOff, num3 + leaderBoardText.mY + this.mStatsFont.GetAscent(), 0, 1);
					g.SetColor(89, 187, 149);
					g.WriteString(leaderBoardText.mValueStr, num2 + leaderBoardText.mX + Common._DS(Common._M(20)) + (int)this.mLeaderBoards.mXOff, num3 + leaderBoardText.mY + this.mStatsFont.GetAscent(), this.mWidth, -1);
				}
			}
			int num6 = this.NumPages() - 1;
			int num7 = this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth() + 30;
			int theY = (this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetHeight() - this.IMAGE_UI_CHALLENGESCREEN_DIVIDER.GetHeight()) / 2;
			for (int j = 0; j < num6; j++)
			{
				g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_DIVIDER, num7 - this.IMAGE_UI_CHALLENGESCREEN_DIVIDER.GetWidth(), theY);
				num7 += this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth() + 30;
			}
			graphics3D.SetMasking(Graphics3D.EMaskMode.MASKMODE_NONE);
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0007D4C0 File Offset: 0x0007B6C0
		public override void Update()
		{
			float num = Common._M(10f);
			for (int i = 0; i < this.mText.size<LeaderBoardText>(); i++)
			{
				LeaderBoardText leaderBoardText = this.mText[i];
				if (leaderBoardText.mFadeIn && leaderBoardText.mAlpha < 255f)
				{
					this.MarkDirty();
					leaderBoardText.mAlpha += num;
					if (leaderBoardText.mAlpha > 255f)
					{
						leaderBoardText.mAlpha = 255f;
					}
				}
				else if (!leaderBoardText.mFadeIn && leaderBoardText.mAlpha > 0f)
				{
					this.MarkDirty();
					leaderBoardText.mAlpha -= num;
					if (leaderBoardText.mAlpha <= 0f)
					{
						this.mText.RemoveAt(i);
						i--;
					}
				}
			}
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0007D58C File Offset: 0x0007B78C
		private void SetupLeaderboardsTextXLive(ref int theStartY, int page, bool update, LeaderboardReader reader)
		{
			int num = Common._DS(Common._M(80));
			int num2 = 40 + this.mStatsFont.mHeight;
			LeaderBoardText leaderBoardText = null;
			int count = reader.Entries.Count;
			if (!update)
			{
				int num3 = Common._DS(Common._M(0));
				num3 += 45;
				for (int i = 0; i < count; i++)
				{
					LeaderboardEntry leaderboardEntry = reader.Entries[i];
					this.mText.Add(new LeaderBoardText());
					leaderBoardText = this.mText.back<LeaderBoardText>();
					leaderBoardText.mHeaderStr = " " + (1 + i + reader.PageStart);
					leaderBoardText.mValueStr = "";
					leaderBoardText.mX = num;
					leaderBoardText.mY = num3 + theStartY;
					this.mText.Add(new LeaderBoardText());
					leaderBoardText = this.mText.back<LeaderBoardText>();
					leaderBoardText.mShowIcon = true;
					try
					{
						Stream gamerPicture = leaderboardEntry.Gamer.GetProfile().GetGamerPicture();
						leaderBoardText.mIcon = GameApp.gApp.mAppDriver.GetOptimizedImage(gamerPicture, true, true);
					}
					catch (Exception)
					{
						leaderBoardText.mIcon = Res.GetImageByID(ResID.IMAGE_UI_AVATAR);
					}
					leaderBoardText.mHeaderStr = leaderboardEntry.Gamer.Gamertag;
					leaderBoardText.mValueStr = "";
					leaderBoardText.mX = num + 45;
					leaderBoardText.mY = num3 + theStartY;
					this.mText.Add(new LeaderBoardText());
					leaderBoardText = this.mText.back<LeaderBoardText>();
					leaderBoardText.mHeaderStr = SexyFramework.Common.CommaSeperate(leaderboardEntry.Columns.GetValueInt32("BestScore"));
					leaderBoardText.mValueStr = "";
					leaderBoardText.mX = num + 380;
					leaderBoardText.mY = num3 + theStartY;
					num3 += num2;
				}
				return;
			}
			for (int j = 0; j < count; j++)
			{
				LeaderboardEntry leaderboardEntry2 = reader.Entries[j];
				leaderBoardText = this.mText[j * 3];
				leaderBoardText.mHeaderStr = " " + (1 + j + reader.PageStart);
				leaderBoardText.mValueStr = "";
				leaderBoardText = this.mText[j * 3 + 1];
				leaderBoardText.mShowIcon = true;
				if (leaderBoardText.mIcon != null)
				{
					if (leaderBoardText.mIcon != Res.GetImageByID(ResID.IMAGE_UI_AVATAR))
					{
						Image mIcon = leaderBoardText.mIcon;
						leaderBoardText.mIcon = null;
						mIcon.Dispose();
					}
					else
					{
						leaderBoardText.mIcon = null;
					}
				}
				try
				{
					Stream gamerPicture2 = leaderboardEntry2.Gamer.GetProfile().GetGamerPicture();
					leaderBoardText.mIcon = GameApp.gApp.mAppDriver.GetOptimizedImage(gamerPicture2, true, true);
				}
				catch (Exception)
				{
					leaderBoardText.mIcon = Res.GetImageByID(ResID.IMAGE_UI_AVATAR);
				}
				leaderBoardText.mHeaderStr = leaderboardEntry2.Gamer.Gamertag;
				leaderBoardText.mValueStr = "";
				leaderBoardText = this.mText[j * 3 + 2];
				leaderBoardText.mHeaderStr = SexyFramework.Common.CommaSeperate(leaderboardEntry2.Columns.GetValueInt32("BestScore"));
				leaderBoardText.mValueStr = "";
			}
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0007D8C0 File Offset: 0x0007BAC0
		public void UpdatePage(int page)
		{
			for (int i = 0; i < this.mNumLines; i++)
			{
				LeaderBoardText leaderBoardText = this.mText[i * 3];
				leaderBoardText.mHeaderStr = " " + (i + 1 + page * this.mNumLines);
				leaderBoardText.mValueStr = "";
				leaderBoardText = this.mText[i * 3 + 1];
				if (leaderBoardText.mIcon != null)
				{
					leaderBoardText.mIcon.Dispose();
				}
				leaderBoardText.mIcon = Res.GetImageByID(ResID.IMAGE_UI_ACHIEVEMENTS_FROGSTATUE);
				leaderBoardText.mHeaderStr = "KKKKKKKKKK ";
				leaderBoardText.mValueStr = "";
				leaderBoardText = this.mText[i * 3 + 2];
				leaderBoardText.mHeaderStr = "1000000";
				leaderBoardText.mValueStr = "";
			}
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0007D994 File Offset: 0x0007BB94
		private void SetupLeaderboardsText(ref int theStartY, int page, bool update)
		{
			int num = Common._DS(Common._M(80));
			int num2 = 40 + this.mStatsFont.mHeight;
			if (!update)
			{
				int num3 = Common._DS(Common._M(0));
				num3 += 45;
				for (int i = 0; i < this.mNumLines; i++)
				{
					this.mText.Add(new LeaderBoardText());
					LeaderBoardText leaderBoardText = this.mText.back<LeaderBoardText>();
					leaderBoardText.mHeaderStr = " " + (i + 1 + page * this.mNumLines);
					leaderBoardText.mValueStr = "";
					leaderBoardText.mX = num;
					leaderBoardText.mY = num3 + theStartY;
					this.mText.Add(new LeaderBoardText());
					leaderBoardText = this.mText.back<LeaderBoardText>();
					leaderBoardText.mIcon = Res.GetImageByID(ResID.IMAGE_UI_AVATAR);
					leaderBoardText.mHeaderStr = "KKKKKKKKKKkkkkkkkkkkkkkkk ";
					leaderBoardText.mValueStr = "";
					leaderBoardText.mX = num + 45;
					leaderBoardText.mY = num3 + theStartY;
					this.mText.Add(new LeaderBoardText());
					leaderBoardText = this.mText.back<LeaderBoardText>();
					leaderBoardText.mHeaderStr = SexyFramework.Common.CommaSeperate(1000000);
					leaderBoardText.mValueStr = "";
					leaderBoardText.mX = num + 380;
					leaderBoardText.mY = num3 + theStartY;
					num3 += num2;
				}
				return;
			}
			for (int j = 0; j < this.mNumLines; j++)
			{
				LeaderBoardText leaderBoardText = this.mText[j * 3];
				leaderBoardText.mHeaderStr = " " + (j + 1 + page * this.mNumLines);
				leaderBoardText.mValueStr = "";
				leaderBoardText = this.mText[j * 3 + 1];
				if (leaderBoardText.mIcon != null)
				{
					leaderBoardText.mIcon.Dispose();
				}
				leaderBoardText.mIcon = Res.GetImageByID(ResID.IMAGE_UI_ACHIEVEMENTS_FROGSTATUE);
				leaderBoardText.mHeaderStr = "KKKKKKKKKK ";
				leaderBoardText.mValueStr = "";
				leaderBoardText = this.mText[j * 3 + 2];
				leaderBoardText.mHeaderStr = SexyFramework.Common.CommaSeperate(1000000);
				leaderBoardText.mValueStr = "";
			}
		}

		// Token: 0x04000B7A RID: 2938
		private List<LeaderBoardText> mText;

		// Token: 0x04000B7B RID: 2939
		private Font mHeaderFont;

		// Token: 0x04000B7C RID: 2940
		private Font mStatsFont;

		// Token: 0x04000B7D RID: 2941
		private LeaderBoards mLeaderBoards;

		// Token: 0x04000B7E RID: 2942
		public int mNumPages;

		// Token: 0x04000B7F RID: 2943
		public int mNumLines = 4;

		// Token: 0x04000B80 RID: 2944
		private Image IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES);

		// Token: 0x04000B81 RID: 2945
		private Image IMAGE_UI_CHALLENGESCREEN_DIVIDER = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_DIVIDER);

		// Token: 0x04000B82 RID: 2946
		private Image IMAGE_UI_AVATAR = Res.GetImageByID(ResID.IMAGE_UI_AVATAR);

		// Token: 0x04000B83 RID: 2947
		public Image IMAGE_UI_LEADERBOARDS_SHADOW = Res.GetImageByID(ResID.IMAGE_UI_LEADERBOARDS_SHADOW);

		// Token: 0x020000F1 RID: 241
		private enum TextField
		{
			// Token: 0x04000B85 RID: 2949
			txtNumber,
			// Token: 0x04000B86 RID: 2950
			txtGameTag,
			// Token: 0x04000B87 RID: 2951
			txtGameScore,
			// Token: 0x04000B88 RID: 2952
			txtTotal
		}
	}
}
