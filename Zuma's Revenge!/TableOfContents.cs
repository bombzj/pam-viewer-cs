using System;
using System.Collections.Generic;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000048 RID: 72
	public class TableOfContents : Widget, ButtonListener
	{
		// Token: 0x0600066F RID: 1647 RVA: 0x0002B490 File Offset: 0x00029690
		public TableOfContents(ChallengeMenu aChallengeMenu)
		{
			this.mChallengeMenu = aChallengeMenu;
			for (int i = 0; i < GlobalChallenge.NUM_CHALLENGE_ZONES; i++)
			{
				this.mChallengeZoneBtns[i] = null;
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES);
			this.Resize(0, 0, imageByID.GetWidth(), imageByID.GetHeight());
			this.mIsAwardingMedal = false;
			this.mMedalSize = 1f;
			this.mMedalAlpha = 255f;
			this.mAwardedMedal = -1;
			this.mIsAwardAce = false;
			this.mTimer = 0;
			this.mSmokeParticles = new List<LTSmokeParticle>();
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0002B530 File Offset: 0x00029730
		public override void Dispose()
		{
			base.Dispose();
			this.RemoveAllWidgets(false, true);
			for (int i = 0; i < GlobalChallenge.NUM_CHALLENGE_ZONES; i++)
			{
				if (this.mChallengeZoneBtns[i] != null)
				{
					this.mChallengeZoneBtns[i].Dispose();
				}
				this.mChallengeZoneBtns[i] = null;
			}
			for (int j = 0; j < this.mSmokeParticles.Count; j++)
			{
				this.mSmokeParticles[j] = null;
			}
			this.mSmokeParticles.Clear();
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0002B5AC File Offset: 0x000297AC
		public void AwardMedal(int theZone, bool isAced)
		{
			this.mIsAwardingMedal = true;
			this.mMedalSize = 15f;
			this.mMedalAlpha = 0f;
			this.mAwardedMedal = theZone;
			this.mIsAwardAce = isAced;
			this.mTimer = 0;
			for (int i = 0; i < GlobalChallenge.NUM_CHALLENGE_ZONES; i++)
			{
				IndexMedal indexMedal = this.mChallengeZoneBtns[i];
				if (indexMedal != null)
				{
					indexMedal.SetVisible(false);
					indexMedal.SetDisabled(true);
				}
			}
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0002B618 File Offset: 0x00029818
		public void Init()
		{
			int[,] array = new int[6, 2];
			array[0, 0] = Common._DS(300);
			array[0, 1] = Common._DS(250);
			array[1, 0] = Common._DS(600);
			array[1, 1] = Common._DS(250);
			array[2, 0] = Common._DS(900);
			array[2, 1] = Common._DS(250);
			array[3, 0] = Common._DS(300);
			array[3, 1] = Common._DS(600);
			array[4, 0] = Common._DS(600);
			array[4, 1] = Common._DS(600);
			array[5, 0] = Common._DS(900);
			array[5, 1] = Common._DS(600);
			int[,] array2 = array;
			for (int i = 0; i < GlobalChallenge.NUM_CHALLENGE_ZONES; i++)
			{
				this.mChallengeZoneBtns[i] = new IndexMedal(this.mChallengeMenu.HasAcedZone(i), 10101 + i, this);
				Image imageByID;
				if (this.mChallengeMenu.HasAcedZone(i))
				{
					imageByID = Res.GetImageByID(ResID.IMAGE_UI_MAIN_MENU_LEAVES_CUPICON_ZONE_1 + (i + 1) * 3);
				}
				else if (this.mChallengeMenu.HasBeatZone(i))
				{
					imageByID = Res.GetImageByID(ResID.IMAGE_UI_MAIN_MENU_LEAVES_CUPICON_ZONE_1_STONE + (i + 1) * 3);
				}
				else
				{
					imageByID = Res.GetImageByID(ResID.IMAGE_UI_MAIN_MENU_LEAVES_CUPICON_ZONE_1_WOOD + (i + 1) * 3);
				}
				this.mChallengeZoneBtns[i].mButtonImage = imageByID;
				this.mChallengeZoneBtns[i].Resize(array2[i, 0] + GameApp.gApp.GetScreenRect().mX / 2, array2[i, 1], imageByID.GetWidth(), imageByID.GetHeight());
				this.mChallengeZoneBtns[i].SetVisible(true);
				this.mChallengeZoneBtns[i].SetDisabled(false);
				this.mChallengeZoneBtns[i].Init();
				this.AddWidget(this.mChallengeZoneBtns[i]);
			}
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0002B814 File Offset: 0x00029A14
		public override void Update()
		{
			if (!this.mIsAwardingMedal)
			{
				return;
			}
			if (GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			for (int i = 0; i < this.mSmokeParticles.Count; i++)
			{
				if (BambooTransition.UpdateSmokeParticle(this.mSmokeParticles[i]))
				{
					this.mSmokeParticles.RemoveAt(i);
					i--;
				}
			}
			this.MarkDirty();
			this.mTimer++;
			int num = Common._M(75) - this.mTimer;
			float num2 = 255f / (float)num;
			this.mMedalAlpha += num2;
			if (this.mMedalAlpha > 255f)
			{
				this.mMedalAlpha = 255f;
			}
			num2 = Common._M(15f) / (float)num;
			this.mMedalSize -= num2;
			if (this.mMedalSize <= 1f)
			{
				if (this.mIsAwardAce)
				{
					GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_MINI_CROWN_IMPACT));
				}
				else
				{
					GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_ACE_MINI_CROWN_IMPACT));
				}
				GlobalChallenge.gScreenShakeTimer = Common._M(15);
				this.mMedalSize = 1f;
				this.mMedalAlpha = 255f;
				if (GameApp.gApp.mUserProfile.mDoChallengeAceCupComplete)
				{
					GameApp.gApp.mUserProfile.mDoChallengeAceCupComplete = false;
				}
				else if (GameApp.gApp.mUserProfile.mDoChallengeCupComplete)
				{
					GameApp.gApp.mUserProfile.mDoChallengeCupComplete = false;
				}
				Image imageByID;
				if (this.mIsAwardAce)
				{
					imageByID = Res.GetImageByID(ResID.IMAGE_UI_MAIN_MENU_LEAVES_CUPICON_ZONE_1 + (this.mAwardedMedal + 1) * 3);
					this.mChallengeZoneBtns[this.mAwardedMedal].SetAced();
				}
				else
				{
					imageByID = Res.GetImageByID(ResID.IMAGE_UI_MAIN_MENU_LEAVES_CUPICON_ZONE_1_STONE + (this.mAwardedMedal + 1) * 3);
				}
				this.mChallengeZoneBtns[this.mAwardedMedal].mButtonImage = imageByID;
				this.mIsAwardingMedal = false;
				this.mIsAwardAce = false;
				this.mMedalSize = 1f;
				this.mMedalAlpha = 255f;
				for (int j = 0; j < 40; j++)
				{
					float x = (float)this.mChallengeZoneBtns[this.mAwardedMedal].mX + (float)this.mChallengeZoneBtns[this.mAwardedMedal].mWidth / 2f;
					float y = (float)this.mChallengeZoneBtns[this.mAwardedMedal].mY + (float)this.mChallengeZoneBtns[this.mAwardedMedal].mHeight / 2f;
					this.mSmokeParticles.Add(BambooTransition.SpawnSmokeParticle(x, y, false, false));
				}
				for (int k = 0; k < GlobalChallenge.NUM_CHALLENGE_ZONES; k++)
				{
					ButtonWidget buttonWidget = this.mChallengeZoneBtns[k];
					if (buttonWidget != null)
					{
						buttonWidget.SetVisible(true);
						buttonWidget.SetDisabled(false);
					}
				}
			}
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0002BAC4 File Offset: 0x00029CC4
		public override void Draw(Graphics g)
		{
			string @string = TextManager.getInstance().getString(426);
			g.SetFont(Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_STROKE));
			g.SetColor(SexyColor.White);
			float num = (float)g.GetFont().StringWidth(@string);
			g.DrawString(@string, (int)((float)(GameApp.gApp.GetScreenRect().mX + this.mWidth) - num) / 2, Common._DS(150));
			float[,] array = new float[6, 2];
			array[0, 0] = (float)Common._DS(5);
			array[0, 1] = (float)Common._DS(-5);
			array[1, 0] = (float)Common._DS(11);
			array[1, 1] = (float)Common._DS(2);
			array[2, 0] = (float)Common._DS(-8);
			array[2, 1] = (float)Common._DS(5);
			array[3, 0] = (float)Common._DS(-7);
			array[3, 1] = (float)Common._DS(-1);
			array[4, 0] = (float)Common._DS(0);
			array[4, 1] = (float)Common._DS(0);
			array[5, 0] = (float)Common._DS(0);
			array[5, 1] = (float)Common._DS(0);
			float[,] array2 = array;
			for (int i = 0; i < GlobalChallenge.NUM_CHALLENGE_ZONES; i++)
			{
				if (this.mChallengeZoneBtns[i] != null)
				{
					Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_MAIN_MENU_LEAVES_LEAVES1 + i);
					float num2 = array2[i, 0] + (float)this.mChallengeZoneBtns[i].mX - (float)((imageByID.GetWidth() - this.mChallengeZoneBtns[i].mButtonImage.GetWidth()) / 2);
					float num3 = array2[i, 1] + (float)this.mChallengeZoneBtns[i].mY - (float)((imageByID.GetHeight() - this.mChallengeZoneBtns[i].mButtonImage.GetHeight()) / 2);
					g.DrawImage(imageByID, (int)num2, (int)num3);
					if (this.mIsAwardingMedal)
					{
						g.DrawImage(this.mChallengeZoneBtns[i].mButtonImage, this.mChallengeZoneBtns[i].mX, this.mChallengeZoneBtns[i].mY);
					}
					for (int j = 0; j < this.mSmokeParticles.Count; j++)
					{
						BambooTransition.DrawSmokeParticle(g, this.mSmokeParticles[j]);
					}
				}
			}
			if (this.mIsAwardingMedal)
			{
				g.PushState();
				g.ClearClipRect();
				Image imageByID2;
				if (this.mIsAwardAce)
				{
					imageByID2 = Res.GetImageByID(ResID.IMAGE_UI_MAIN_MENU_LEAVES_CUPICON_ZONE_1 + (this.mAwardedMedal + 1) * 3);
				}
				else
				{
					imageByID2 = Res.GetImageByID(ResID.IMAGE_UI_MAIN_MENU_LEAVES_CUPICON_ZONE_1_STONE + (this.mAwardedMedal + 1) * 3);
				}
				g.SetColor(new SexyColor(255, 255, 255, (int)this.mMedalAlpha));
				g.SetColorizeImages(true);
				SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
				sexyTransform2D.Scale(this.mMedalSize, this.mMedalSize);
				sexyTransform2D.Translate((float)this.mChallengeZoneBtns[this.mAwardedMedal].mX + ((float)this.mChallengeZoneBtns[this.mAwardedMedal].mButtonImage.mWidth - (float)imageByID2.mWidth * this.mMedalSize) / 2f, (float)this.mChallengeZoneBtns[this.mAwardedMedal].mY + ((float)this.mChallengeZoneBtns[this.mAwardedMedal].mButtonImage.mHeight - (float)imageByID2.mHeight * this.mMedalSize) / 2f);
				g.DrawImageMatrix(imageByID2, sexyTransform2D, (float)imageByID2.mWidth * this.mMedalSize / 2f, (float)imageByID2.mHeight * this.mMedalSize / 2f);
				g.PopState();
			}
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0002BE74 File Offset: 0x0002A074
		public virtual void ButtonDepress(int id)
		{
			if (GameApp.gApp.mBambooTransition != null && GameApp.gApp.mBambooTransition.IsInProgress())
			{
				return;
			}
			GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BUTTON2));
			switch (id)
			{
			case 10101:
				this.mChallengeMenu.mChallengeScrollWidget.SetPageHorizontal(1, true);
				return;
			case 10102:
				this.mChallengeMenu.mChallengeScrollWidget.SetPageHorizontal(2, true);
				return;
			case 10103:
				this.mChallengeMenu.mChallengeScrollWidget.SetPageHorizontal(3, true);
				return;
			case 10104:
				this.mChallengeMenu.mChallengeScrollWidget.SetPageHorizontal(4, true);
				return;
			case 10105:
				this.mChallengeMenu.mChallengeScrollWidget.SetPageHorizontal(5, true);
				return;
			case 10106:
				this.mChallengeMenu.mChallengeScrollWidget.SetPageHorizontal(6, true);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0002BF4B File Offset: 0x0002A14B
		public virtual void ButtonDownTick(int x)
		{
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0002BF4D File Offset: 0x0002A14D
		public virtual void ButtonMouseEnter(int x)
		{
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0002BF4F File Offset: 0x0002A14F
		public virtual void ButtonMouseLeave(int x)
		{
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0002BF51 File Offset: 0x0002A151
		public virtual void ButtonMouseMove(int x, int y, int z)
		{
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0002BF53 File Offset: 0x0002A153
		public virtual void ButtonPress(int id)
		{
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0002BF55 File Offset: 0x0002A155
		public virtual void ButtonPress(int id, int count)
		{
		}

		// Token: 0x0400038D RID: 909
		private bool mIsAwardingMedal;

		// Token: 0x0400038E RID: 910
		private bool mIsAwardAce;

		// Token: 0x0400038F RID: 911
		private float mMedalSize;

		// Token: 0x04000390 RID: 912
		private float mMedalAlpha;

		// Token: 0x04000391 RID: 913
		private int mAwardedMedal;

		// Token: 0x04000392 RID: 914
		private int mTimer;

		// Token: 0x04000393 RID: 915
		private IndexMedal[] mChallengeZoneBtns = new IndexMedal[GlobalChallenge.NUM_CHALLENGE_ZONES];

		// Token: 0x04000394 RID: 916
		private ChallengeMenu mChallengeMenu;

		// Token: 0x04000395 RID: 917
		private List<LTSmokeParticle> mSmokeParticles;

		// Token: 0x02000049 RID: 73
		private enum ChallengeZonePages
		{
			// Token: 0x04000397 RID: 919
			ContentId_MettleOfTheMonkey = 10101,
			// Token: 0x04000398 RID: 920
			ContentId_RoosterRumble,
			// Token: 0x04000399 RID: 921
			ContentId_JackalJam,
			// Token: 0x0400039A RID: 922
			ContentId_MarshMadness,
			// Token: 0x0400039B RID: 923
			ContentId_UnderseaUndertaking,
			// Token: 0x0400039C RID: 924
			ContentId_SerpentScuffle,
			// Token: 0x0400039D RID: 925
			NUM_CHALLENGE_ZONE_PAGES
		}
	}
}
