using System;
using JeffLib;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x020000F4 RID: 244
	public class LegalInfo : DialogEx, SliderListener
	{
		// Token: 0x06000D0C RID: 3340 RVA: 0x0007F428 File Offset: 0x0007D628
		public LegalInfo()
			: base(null, null, 11, true, "", "", "", 0)
		{
			this.FONT_SHAGLOUNGE28_GREEN = Res.GetFontByID(ResID.FONT_SHAGLOUNGE28_GREEN);
			this.FONT_SHAGEXOTICA68_BASE = Res.GetFontByID(ResID.FONT_SHAGEXOTICA68_BASE);
			this.FONT_SHAGLOUNGE28_BROWN = Res.GetFontByID(ResID.FONT_SHAGLOUNGE28_BROWN);
			this.IMAGE_GUI_DIALOG_BOX_MAINMENU_CROWN_BOX = Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BOX_MAINMENU_CROWN_BOX);
			this.IMAGE_GUI_DIALOG_BOX_MAINMENU_SLIDEBOXBACK = Res.GetImageByID(ResID.IMAGE_GUI_DIALOG_BOX_MAINMENU_SLIDEBOXBACK);
			this.mEndUserLicenseAgreement = null;
			this.mPrivacyPolicy = null;
			this.mTermsOfService = null;
			this.mAboutBtn = null;
			this.mHelpBtn = null;
			this.mOKBtn = null;
			this.mExternalLinkDialog = null;
			int num = Common._DS(Common._M(304));
			int num2 = Common._DS(Common._M(162));
			int num3 = Common._DS(85);
			int num4 = Common._DS(25);
			int num5 = Common._DS(75);
			string @string = TextManager.getInstance().getString(1);
			string string2 = TextManager.getInstance().getString(862);
			string string3 = TextManager.getInstance().getString(480);
			int num6 = this.FONT_SHAGLOUNGE28_GREEN.StringWidth(string3);
			string string4 = TextManager.getInstance().getString(481);
			int num7 = this.FONT_SHAGLOUNGE28_GREEN.StringWidth(string4);
			string string5 = TextManager.getInstance().getString(482);
			int num8 = this.FONT_SHAGLOUNGE28_GREEN.StringWidth(string5);
			int num9 = Math.Max(num6, Math.Max(num7, num8));
			this.mAboutBtn = Common.MakeButton(4, this, string2);
			this.mAboutBtn.SetFont(this.FONT_SHAGLOUNGE28_GREEN);
			this.mAboutBtn.Resize(num5, Common._DS(Common._M(30)) + num4, num9 + num3, num2);
			this.AddWidget(this.mAboutBtn);
			this.mEndUserLicenseAgreement = Common.MakeButton(0, this, string3);
			this.mEndUserLicenseAgreement.SetFont(this.FONT_SHAGLOUNGE28_GREEN);
			this.mEndUserLicenseAgreement.Resize(num5, this.mAboutBtn.mY + this.mAboutBtn.mHeight + num4, num9 + num3, num2);
			int num10 = this.mEndUserLicenseAgreement.mWidth;
			this.AddWidget(this.mEndUserLicenseAgreement);
			this.mPrivacyPolicy = Common.MakeButton(1, this, string4);
			this.mPrivacyPolicy.SetFont(this.FONT_SHAGLOUNGE28_GREEN);
			this.mPrivacyPolicy.Resize(num5, this.mEndUserLicenseAgreement.mY + this.mEndUserLicenseAgreement.mHeight + num4, num9 + num3, num2);
			num10 = ((num10 < this.mPrivacyPolicy.mWidth) ? this.mPrivacyPolicy.mWidth : num10);
			this.AddWidget(this.mPrivacyPolicy);
			this.mTermsOfService = Common.MakeButton(2, this, string5);
			this.mTermsOfService.SetFont(this.FONT_SHAGLOUNGE28_GREEN);
			this.mTermsOfService.Resize(num5, this.mPrivacyPolicy.mY + this.mPrivacyPolicy.mHeight + num4, num9 + num3, num2);
			num10 = ((num10 < this.mTermsOfService.mWidth) ? this.mTermsOfService.mWidth : num10);
			this.AddWidget(this.mTermsOfService);
			this.mHelpBtn = Common.MakeButton(5, this, @string);
			this.mHelpBtn.SetFont(this.FONT_SHAGLOUNGE28_GREEN);
			this.mHelpBtn.Resize(num5, this.mTermsOfService.mY + this.mTermsOfService.mHeight + num4, num9 + num3, num2);
			this.AddWidget(this.mHelpBtn);
			int num11 = num10 + num5 * 2;
			int num12 = (int)((float)(this.mTermsOfService.mY + num4) + (float)num2 * 3f) + 20;
			this.Resize((GameApp.gApp.mWidth - num11) / 2, (GameApp.gApp.GetScreenRect().mHeight - num12) / 2, num11, num12);
			this.mVersionTextY = this.mHelpBtn.mY + this.mHelpBtn.mHeight + num4 + 29;
			this.mOKBtn = Common.MakeButton(3, this, TextManager.getInstance().getString(483));
			this.mOKBtn.SetFont(this.FONT_SHAGLOUNGE28_GREEN);
			int num13 = 10;
			this.mOKBtn.Resize((this.mWidth - num) / 2, this.mHeight - num2 - num13, num, num2);
			this.AddWidget(this.mOKBtn);
			this.mHasTransparencies = (this.mHasAlpha = true);
			this.mClip = false;
			this.mDrawScale.SetCurve(Common._MP("b+0,2,0.033333,1,####        cY### >P###"));
			this.mCurrentLanguage = Localization.GetCurrentLanguage();
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0007F894 File Offset: 0x0007DA94
		public override void Dispose()
		{
			this.RemoveAllWidgets(false, true);
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0007F89E File Offset: 0x0007DA9E
		public override void RemoveAllWidgets(bool doDelete, bool recursive)
		{
			base.RemoveAllWidgets(doDelete, recursive);
			this.mEndUserLicenseAgreement = null;
			this.mPrivacyPolicy = null;
			this.mTermsOfService = null;
			this.mOKBtn = null;
			this.mAboutBtn = null;
			this.mHelpBtn = null;
			this.mExternalLinkDialog = null;
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0007F8D9 File Offset: 0x0007DAD9
		public override void Draw(Graphics g)
		{
			Common.DrawCommonDialogBacking(g, 0, 0, this.mWidth, this.mHeight);
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0007F8EF File Offset: 0x0007DAEF
		public override void ButtonPress(int inButtonID)
		{
			GameApp.gApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BUTTON1));
			base.ButtonPress(inButtonID);
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0007F90C File Offset: 0x0007DB0C
		public void ProcessHardwareBackButton()
		{
			if (this.mExternalLinkDialog != null)
			{
				this.mExternalLinkDialog.ButtonDepress(1001);
			}
			else if (GameApp.gApp.mGenericHelp != null)
			{
				GameApp.gApp.mGenericHelp.ButtonDepress(1000);
			}
			else
			{
				this.ButtonDepress(3);
			}
			GameApp.gApp.OnHardwareBackButtonPressProcessed();
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0007F968 File Offset: 0x0007DB68
		public override void ButtonDepress(int inButtonID)
		{
			if (this.mExternalLinkDialog != null)
			{
				return;
			}
			string text = "";
			switch (this.mCurrentLanguage)
			{
			default:
				text += "en";
				break;
			case Localization.LanguageType.Language_FR:
				text += "fr";
				break;
			case Localization.LanguageType.Language_IT:
				text += "it";
				break;
			case Localization.LanguageType.Language_GR:
				text += "de";
				break;
			case Localization.LanguageType.Language_SP:
				text += "es";
				break;
			case Localization.LanguageType.Language_CH:
				text += "sc";
				break;
			case Localization.LanguageType.Language_RU:
				text += "ru";
				break;
			case Localization.LanguageType.Language_PL:
				text += "pl";
				break;
			case Localization.LanguageType.Language_PG:
				text += "pt";
				break;
			case Localization.LanguageType.Language_SPC:
				text += "es";
				break;
			case Localization.LanguageType.Language_CHT:
				text += "tc";
				break;
			case Localization.LanguageType.Language_PGB:
				text += "br";
				break;
			}
			if (this.mOKBtn != null && inButtonID == this.mOKBtn.mId)
			{
				this.mDrawScale.SetCurve(Common._MP("b+0,1,0.05,1,~###         ~#A5t"));
				this.mWidgetFlagsMod.mRemoveFlags |= 16;
				GameApp.gApp.HideLegal();
				return;
			}
			if (this.mEndUserLicenseAgreement != null && inButtonID == this.mEndUserLicenseAgreement.mId)
			{
				this.ShowExternalLinkInfo("http://tos.ea.com/legalapp/mobileeula/US/" + text + "/GM");
				return;
			}
			if (this.mTermsOfService != null && inButtonID == this.mTermsOfService.mId)
			{
				this.ShowExternalLinkInfo("http://tos.ea.com/legalapp/WEBTERMS/US/" + text + "/PC");
				return;
			}
			if (this.mPrivacyPolicy != null && inButtonID == this.mPrivacyPolicy.mId)
			{
				this.ShowExternalLinkInfo("http://tos.ea.com/legalapp/WEBPRIVACY/US/" + text + "/PC/");
				return;
			}
			if (this.mAboutBtn != null && inButtonID == this.mAboutBtn.mId)
			{
				GameApp.gApp.ShowAbout();
				return;
			}
			if (this.mHelpBtn != null && inButtonID == this.mHelpBtn.mId)
			{
				GameApp.gApp.mGenericHelp = new GenericHelp();
				GameApp.gApp.AddDialog(GameApp.gApp.mGenericHelp);
			}
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0007FB98 File Offset: 0x0007DD98
		public override void MouseDrag(int x, int y)
		{
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0007FB9A File Offset: 0x0007DD9A
		private void ShowExternalLinkInfo(string theURL)
		{
			if (this.mExternalLinkDialog == null)
			{
				this.mExternalLinkDialog = new LegalInfo.ExternalLinkDialog(this, theURL);
				Common.SetupDialog(this.mExternalLinkDialog);
				GameApp.gApp.AddDialog(this.mExternalLinkDialog);
			}
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0007FBCC File Offset: 0x0007DDCC
		public void HideExternalLinkInfo()
		{
			this.mExternalLinkDialog.mDrawScale.SetCurve(Common._MP("b+0,1,0.05,1,~###         ~#A5t"));
			this.mExternalLinkDialog.mWidgetFlagsMod.mRemoveFlags |= 16;
			this.mExternalLinkDialog = null;
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0007FC08 File Offset: 0x0007DE08
		public void SliderVal(int theId, double theVal)
		{
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0007FC0A File Offset: 0x0007DE0A
		public void SliderReleased(int theId, double theVal)
		{
		}

		// Token: 0x04000BC1 RID: 3009
		private DialogButton mEndUserLicenseAgreement;

		// Token: 0x04000BC2 RID: 3010
		private DialogButton mPrivacyPolicy;

		// Token: 0x04000BC3 RID: 3011
		private DialogButton mTermsOfService;

		// Token: 0x04000BC4 RID: 3012
		private DialogButton mAboutBtn;

		// Token: 0x04000BC5 RID: 3013
		private DialogButton mHelpBtn;

		// Token: 0x04000BC6 RID: 3014
		private DialogButton mOKBtn;

		// Token: 0x04000BC7 RID: 3015
		private int mVersionTextY;

		// Token: 0x04000BC8 RID: 3016
		private LegalInfo.ExternalLinkDialog mExternalLinkDialog;

		// Token: 0x04000BC9 RID: 3017
		private Font FONT_SHAGLOUNGE28_GREEN;

		// Token: 0x04000BCA RID: 3018
		private Font FONT_SHAGEXOTICA68_BASE;

		// Token: 0x04000BCB RID: 3019
		private Font FONT_SHAGLOUNGE28_BROWN;

		// Token: 0x04000BCC RID: 3020
		private Image IMAGE_GUI_DIALOG_BOX_MAINMENU_CROWN_BOX;

		// Token: 0x04000BCD RID: 3021
		private Image IMAGE_GUI_DIALOG_BOX_MAINMENU_SLIDEBOXBACK;

		// Token: 0x04000BCE RID: 3022
		private Localization.LanguageType mCurrentLanguage;

		// Token: 0x020000F5 RID: 245
		private enum LegalButtonIDs
		{
			// Token: 0x04000BD0 RID: 3024
			Legal_EndUserLicenseAgreementID,
			// Token: 0x04000BD1 RID: 3025
			Legal_PrivacyPolicyID,
			// Token: 0x04000BD2 RID: 3026
			Legal_TermsOfServiceID,
			// Token: 0x04000BD3 RID: 3027
			Legal_OKID,
			// Token: 0x04000BD4 RID: 3028
			Legal_AboutID,
			// Token: 0x04000BD5 RID: 3029
			Legal_HelpID,
			// Token: 0x04000BD6 RID: 3030
			Legal_MetricsSharingID
		}

		// Token: 0x020000F6 RID: 246
		private class ExternalLinkDialog : ZumaDialog
		{
			// Token: 0x06000D18 RID: 3352 RVA: 0x0007FC0C File Offset: 0x0007DE0C
			public ExternalLinkDialog(LegalInfo theLegalInfo, string theURL)
				: base(13, true, TextManager.getInstance().getString(486), TextManager.getInstance().getString(487), "", 2)
			{
				this.mLegalInfo = theLegalInfo;
				this.mURL = theURL;
			}

			// Token: 0x06000D19 RID: 3353 RVA: 0x0007FC4C File Offset: 0x0007DE4C
			~ExternalLinkDialog()
			{
			}

			// Token: 0x06000D1A RID: 3354 RVA: 0x0007FC74 File Offset: 0x0007DE74
			public override void Resize(int x, int y, int w, int h)
			{
				base.Resize(x, y, w, h);
				ButtonWidget[] inButtons = new ButtonWidget[] { this.mYesButton, this.mNoButton };
				Common.SizeButtonsToLabel(inButtons, 2, Common._S(20));
			}

			// Token: 0x06000D1B RID: 3355 RVA: 0x0007FCB8 File Offset: 0x0007DEB8
			public override void ButtonDepress(int id)
			{
				if (id == 2000 + this.mId || id == 1000)
				{
					GameApp.gApp.OpenURL(this.mURL);
					this.mLegalInfo.HideExternalLinkInfo();
					return;
				}
				if (id == 3000 + this.mId || id == 1001)
				{
					this.mLegalInfo.HideExternalLinkInfo();
				}
			}

			// Token: 0x04000BD7 RID: 3031
			private string mURL;

			// Token: 0x04000BD8 RID: 3032
			private LegalInfo mLegalInfo;
		}
	}
}
