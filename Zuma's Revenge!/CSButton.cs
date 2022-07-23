using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000040 RID: 64
	public class CSButton : ButtonWidget
	{
		// Token: 0x06000632 RID: 1586 RVA: 0x000286CC File Offset: 0x000268CC
		public CSButton(int id, ChallengeMenu theChallengeMenu, ButtonListener listener)
			: base(id, listener)
		{
			this.mChallengeMenu = theChallengeMenu;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00028722 File Offset: 0x00026922
		public override void Dispose()
		{
			if (this.mUnlockSparkles != null)
			{
				this.mUnlockSparkles.Dispose();
				this.mUnlockSparkles = null;
			}
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00028740 File Offset: 0x00026940
		public override void Draw(Graphics g)
		{
			if (g.mClipRect.mWidth <= 0 || g.mClipRect.mHeight <= 0)
			{
				return;
			}
			CSButton.last_uc = this.mUpdateCnt;
			bool flag = this.mIsDown && this.mIsOver && !this.mDisabled;
			flag ^= this.mInverted;
			bool flag2 = this.mId - 3 + 1 == GameApp.gLastLevel && this.mChallengeMenu.mCrownZoomType >= 0;
			int num = (flag ? Common._DS(Common._M(0)) : 0);
			int num2 = (flag ? Common._DS(Common._M(0)) : 0);
			Image image = null;
			if (this.mLevel != -1)
			{
				image = GameApp.gApp.GetLevelThumbnail(this.mLevel);
			}
			if (image != null)
			{
				g.DrawImage(image, GlobalChallenge.gScreenShake + num, GlobalChallenge.gScreenShake + num2, Common._DS(GlobalChallenge.CS_BTN_WIDTH), Common._DS(GlobalChallenge.CS_BTN_HEIGHT));
				if (this.mMouseOver)
				{
					g.PushState();
					g.SetColor(new SexyColor(255, 255, 255, Common._M(100)));
					g.SetColorizeImages(true);
					g.SetDrawMode(1);
					g.DrawImage(image, GlobalChallenge.gScreenShake + num, GlobalChallenge.gScreenShake + num2, Common._DS(GlobalChallenge.CS_BTN_WIDTH), Common._DS(GlobalChallenge.CS_BTN_HEIGHT));
					g.PopState();
				}
				if (flag)
				{
					g.DrawImage(Res.GetImageByID(ResID.IMAGE_UI_MAIN_MENU_CH_THUMBNAILOVERLAY), GlobalChallenge.gScreenShake, GlobalChallenge.gScreenShake, Common._DS(GlobalChallenge.CS_BTN_WIDTH + Common._M(0)), Common._DS(GlobalChallenge.CS_BTN_HEIGHT + Common._M1(0)));
				}
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_UI_MAIN_MENU_CS_LOCK_ANIMATION);
			if (this.mOpaque)
			{
				g.SetColor(new SexyColor(0, 0, 0, Common._M(191)));
				g.FillRect(0, 0, Common._DS(GlobalChallenge.CS_BTN_WIDTH), Common._DS(GlobalChallenge.CS_BTN_HEIGHT));
			}
			else if (this.mMedal == imageByID)
			{
				g.SetColor(new SexyColor(0, 0, 0, 120));
				g.FillRect(0, 0, Common._DS(GlobalChallenge.CS_BTN_WIDTH), Common._DS(GlobalChallenge.CS_BTN_HEIGHT));
			}
			Common.DrawCommonDialogBorder(g, GlobalChallenge.gScreenShake - Common._DS(15), GlobalChallenge.gScreenShake - Common._DS(15), this.mWidth + Common._DS(30), this.mHeight + Common._DS(30));
			if (this.mUnlockAlpha > 0)
			{
				Image image2 = imageByID;
				g.SetColorizeImages(true);
				g.SetColor(new SexyColor(255, 255, 255, this.mUnlockAlpha));
				g.DrawImageCel(image2, (this.mWidth - image2.GetCelWidth()) / 2 + GlobalChallenge.gScreenShake, (this.mHeight - image2.GetCelHeight()) / 2 + GlobalChallenge.gScreenShake, this.mLockCel);
				g.SetColorizeImages(false);
			}
			if (this.mMedal != null)
			{
				if (!flag2 || !g.Is3D())
				{
					if (this.mMedal == imageByID)
					{
						g.DrawImageCel(this.mMedal, (this.mWidth - this.mMedal.GetCelWidth()) / 2 + GlobalChallenge.gScreenShake + Common._DS(10), (this.mHeight - this.mMedal.GetCelHeight()) / 2 + GlobalChallenge.gScreenShake, 0);
					}
					else
					{
						g.DrawImageCel(this.mMedal, (this.mWidth - this.mMedal.GetCelWidth()) / 2 + GlobalChallenge.gScreenShake, (this.mHeight - this.mMedal.GetCelHeight()) / 2 + GlobalChallenge.gScreenShake, 0);
					}
				}
				else if (this.mMedal != null)
				{
					g.PushState();
					g.ClearClipRect();
					g.Translate(-this.mX, -this.mY);
					Image imageByID2 = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_LARGE_CROWN);
					if (this.mChallengeMenu.mCrownZoomType == 1)
					{
						g.DrawImage(imageByID2, this.mX + (this.mWidth - imageByID2.mWidth) / 2, this.mY + (this.mHeight - imageByID2.mHeight) / 2);
						imageByID2 = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_LARGE_ACECROWN);
					}
					g.SetColor(new SexyColor(255, 255, 255, (int)this.mChallengeMenu.mCrownAlpha));
					g.SetColorizeImages(true);
					SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
					sexyTransform2D.Scale(this.mChallengeMenu.mCrownSize, this.mChallengeMenu.mCrownSize);
					sexyTransform2D.Translate((float)this.mX + ((float)this.mWidth - (float)imageByID2.mWidth * this.mChallengeMenu.mCrownSize) / 2f, (float)this.mY + ((float)this.mHeight - (float)imageByID2.mHeight * this.mChallengeMenu.mCrownSize) / 2f);
					g.DrawImageMatrix(imageByID2, sexyTransform2D, (float)imageByID2.mWidth * this.mChallengeMenu.mCrownSize / 2f, (float)imageByID2.mHeight * this.mChallengeMenu.mCrownSize / 2f);
					g.PopState();
				}
			}
			if (!flag2 && this.mUnlockSparkles != null)
			{
				g.Is3D();
			}
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x00028C58 File Offset: 0x00026E58
		public override void Update()
		{
			this.mUpdateCnt++;
			bool flag = this.mChallengeMenu.mCrownZoomType >= 0;
			if (this.mUnlockSparkles != null && !flag)
			{
				this.mUnlockSparkles.Update();
				this.MarkDirty();
				if (this.mUnlockSparkles.mCurNumParticles == 0 && this.mUnlockSparkles.mFrameNum > 10f)
				{
					this.mUnlockSparkles.Dispose();
					this.mUnlockSparkles = null;
				}
			}
			if (!flag)
			{
				if (this.mLockCel < Res.GetImageByID(ResID.IMAGE_UI_MAIN_MENU_CS_LOCK_ANIMATION).mNumCols - 1 && this.mUpdateCnt % Common._M(8) == 0)
				{
					this.mLockCel++;
					return;
				}
				if (this.mUnlockAlpha > 0)
				{
					this.mUnlockAlpha -= Common._M(2);
				}
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00028D28 File Offset: 0x00026F28
		public override void MouseEnter()
		{
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00028D2A File Offset: 0x00026F2A
		public override void MouseLeave()
		{
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00028D2C File Offset: 0x00026F2C
		public void PreLoadImage()
		{
			if (this.mLevel != -1)
			{
				GameApp.gApp.GetLevelThumbnail(this.mLevel);
			}
		}

		// Token: 0x04000343 RID: 835
		private static int last_uc;

		// Token: 0x04000344 RID: 836
		public PIEffect mUnlockSparkles;

		// Token: 0x04000345 RID: 837
		public int mUnlockAlpha;

		// Token: 0x04000346 RID: 838
		public int mLockCel;

		// Token: 0x04000347 RID: 839
		public Image mMedal;

		// Token: 0x04000348 RID: 840
		public string mScoreStr = "";

		// Token: 0x04000349 RID: 841
		public string mLevelStr = "";

		// Token: 0x0400034A RID: 842
		public string mAceStr = "";

		// Token: 0x0400034B RID: 843
		public string mLevelId = "";

		// Token: 0x0400034C RID: 844
		public bool mMouseOver;

		// Token: 0x0400034D RID: 845
		public bool mOpaque = true;

		// Token: 0x0400034E RID: 846
		public int mLevel = -1;

		// Token: 0x0400034F RID: 847
		public ChallengeMenu mChallengeMenu;

		// Token: 0x02000041 RID: 65
		public enum BtnType
		{
			// Token: 0x04000351 RID: 849
			Btn_CS_Back,
			// Token: 0x04000352 RID: 850
			Btn_CS_PrevSet,
			// Token: 0x04000353 RID: 851
			Btn_CS_NextSet,
			// Token: 0x04000354 RID: 852
			Btn_First_Challenge
		}
	}
}
