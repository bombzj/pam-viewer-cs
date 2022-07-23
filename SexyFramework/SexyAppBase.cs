using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using SexyFramework.Drivers;
using SexyFramework.Drivers.App;
using SexyFramework.Drivers.Audio;
using SexyFramework.Drivers.File;
using SexyFramework.Drivers.Leaderboard;
using SexyFramework.Drivers.Profile;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.Resource;
using SexyFramework.Sound;
using SexyFramework.WidgetsLib;

namespace SexyFramework
{
	// Token: 0x020001A3 RID: 419
	public class SexyAppBase : ButtonListener, DialogListener
	{
		// Token: 0x06000EAB RID: 3755 RVA: 0x0004A2DC File Offset: 0x000484DC
		public SexyAppBase()
		{
			this.mFirstLaunch = false;
			this.mAppUpdated = false;
			this.mResStreamsManager = null;
			this.mGamepadLocked = -1;
			this.mAllowSwapScreenImage = true;
			this.mMaxUpdateBacklog = 200;
			this.mPauseWhenMoving = true;
			this.mGraphicsDriver = null;
			this.mProfileManager = null;
			this.mLeaderboardManager = null;
			this.InitFileDriver();
			this.mAppDriver = WP7AppDriver.CreateAppDriver(this);
			this.mGamepadDriver = XNAGamepadDriver.CreateGamepadDriver();
			this.mAudioDriver = WP7AudioDriver.CreateAudioDriver(this);
			this.mProfileDriver = IProfileDriver.CreateProfileDriver();
			GlobalMembers.gSexyAppBase = this;
			this.mAppDriver.InitAppDriver();
			this.mWidgetManager = new WidgetManager(this);
			Localization.InitLanguage();
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0004A484 File Offset: 0x00048684
		public virtual void Dispose()
		{
			foreach (KeyValuePair<int, Dialog> keyValuePair in this.mDialogMap)
			{
				Widget value = keyValuePair.Value;
				if (value.mParent != null)
				{
					value.mParent.RemoveWidget(value);
				}
			}
			this.mDialogMap.Clear();
			this.mDialogList.Clear();
			this.mWidgetManager = null;
			this.mResourceManager = null;
			foreach (KeyValuePair<KeyValuePair<string, string>, SharedImage> keyValuePair2 in this.mSharedImageMap)
			{
				SharedImage value2 = keyValuePair2.Value;
				if (value2.mImage != null)
				{
					value2.mImage.Dispose();
					value2.mImage = null;
				}
			}
			this.mSharedImageMap.Clear();
			this.mAppDriver.Shutdown();
			this.mProfileManager = null;
			this.mLeaderboardManager = null;
			this.mAudioDriver = null;
			this.mGamepadDriver = null;
			this.mSaveGameDriver = null;
			this.mProfileDriver = null;
			this.mHttpDriver = null;
			this.mLeaderboardDriver = null;
			this.mAchievementDriver = null;
			this.mAppDriver = null;
			this.mResStreamsManager = null;
			this.mFileDriver = null;
			if (GlobalMembers.gFileDriver != null)
			{
				GlobalMembers.gFileDriver.Dispose();
			}
			GlobalMembers.gFileDriver = null;
			GlobalMembers.gSexyAppBase = null;
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0004A5BA File Offset: 0x000487BA
		public virtual void ClearUpdateBacklog(bool relaxForASecond)
		{
			this.mAppDriver.ClearUpdateBacklog(relaxForASecond);
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x0004A5C8 File Offset: 0x000487C8
		public virtual bool IsScreenSaver()
		{
			return this.mIsScreenSaver;
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0004A5D0 File Offset: 0x000487D0
		public virtual bool AppCanRestore()
		{
			return !this.mIsDisabled;
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0004A5DC File Offset: 0x000487DC
		public virtual Dialog NewDialog(int theDialogId, bool isModal, string theDialogHeader, string theDialogLines, string theDialogFooter, int theButtonMode)
		{
			return new Dialog(null, null, theDialogId, isModal, theDialogHeader, theDialogLines, theDialogFooter, theButtonMode);
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x0004A5FC File Offset: 0x000487FC
		public virtual Dialog DoDialog(int theDialogId, bool isModal, string theDialogHeader, string theDialogLines, string theDialogFooter, int theButtonMode)
		{
			this.KillDialog(theDialogId);
			Dialog dialog = this.NewDialog(theDialogId, isModal, theDialogHeader, theDialogLines, theDialogFooter, theButtonMode);
			this.AddDialog(theDialogId, dialog);
			return dialog;
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0004A62A File Offset: 0x0004882A
		public Dialog GetDialog(int theDialogId)
		{
			if (this.mDialogMap.ContainsKey(theDialogId))
			{
				return this.mDialogMap[theDialogId];
			}
			return null;
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0004A648 File Offset: 0x00048848
		public virtual bool KillDialog(int theDialogId, bool removeWidget, bool deleteWidget)
		{
			if (this.mDialogMap.ContainsKey(theDialogId))
			{
				Dialog dialog = this.mDialogMap[theDialogId];
				if (dialog.mResult == -1)
				{
					dialog.mResult = 0;
				}
				if (this.mDialogList.Contains(dialog))
				{
					this.mDialogList.Remove(dialog);
				}
				this.mDialogMap.Remove(theDialogId);
				if ((removeWidget || deleteWidget) && dialog.mParent != null)
				{
					dialog.mParent.RemoveWidget(dialog);
				}
				if (dialog.IsModal())
				{
					this.ModalClose();
					this.mWidgetManager.RemoveBaseModal(dialog);
				}
				if (deleteWidget)
				{
					this.SafeDeleteWidget(dialog);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x0004A6EC File Offset: 0x000488EC
		public virtual bool KillDialog(int theDialogId)
		{
			return this.KillDialog(theDialogId, true, true);
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0004A6F7 File Offset: 0x000488F7
		public virtual bool KillDialog(Dialog theDialog)
		{
			return this.KillDialog(theDialog.mId);
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0004A705 File Offset: 0x00048905
		public virtual int GetDialogCount()
		{
			return this.mDialogMap.Count;
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x0004A714 File Offset: 0x00048914
		public virtual void AddDialog(int theDialogId, Dialog theDialog, FlagsMod belowModalFlagsMod)
		{
			this.KillDialog(theDialogId);
			if (theDialog.mWidth == 0)
			{
				int num = this.mWidth / 2;
				theDialog.Resize((this.mWidth - num) / 2, this.mHeight / 5, num, theDialog.GetPreferredHeight(num));
			}
			this.mDialogMap[theDialogId] = theDialog;
			this.mDialogList.AddLast(theDialog);
			this.mWidgetManager.AddWidget(theDialog);
			if (theDialog.IsModal())
			{
				this.mWidgetManager.AddBaseModal(theDialog, belowModalFlagsMod);
				this.ModalOpen();
			}
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x0004A79B File Offset: 0x0004899B
		public virtual void AddDialog(int theDialogId, Dialog theDialog)
		{
			this.AddDialog(theDialogId, theDialog, this.mWidgetManager.mDefaultBelowModalFlagsMod);
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0004A7B0 File Offset: 0x000489B0
		public virtual void AddDialog(Dialog theDialog)
		{
			this.AddDialog(theDialog.mId, theDialog);
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0004A7BF File Offset: 0x000489BF
		public virtual void ModalOpen()
		{
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x0004A7C1 File Offset: 0x000489C1
		public virtual void ModalClose()
		{
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0004A7C3 File Offset: 0x000489C3
		public virtual void DialogButtonPress(int theDialogId, int theButtonId)
		{
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x0004A7C5 File Offset: 0x000489C5
		public virtual void DialogButtonDepress(int theDialogId, int theButtonId)
		{
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x0004A7C7 File Offset: 0x000489C7
		public virtual void GotFocus()
		{
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x0004A7C9 File Offset: 0x000489C9
		public virtual void LostFocus()
		{
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x0004A7CB File Offset: 0x000489CB
		public virtual void URLOpenFailed(string theURL)
		{
			this.mIsOpeningURL = false;
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0004A7D4 File Offset: 0x000489D4
		public void URLOpenSucceeded(string theURL)
		{
			this.mIsOpeningURL = false;
			if (this.mShutdownOnURLOpen)
			{
				this.Shutdown();
			}
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0004A7EB File Offset: 0x000489EB
		public bool OpenURL(string theURL, bool shutdownOnOpen)
		{
			return this.mAppDriver.OpenURL(theURL, shutdownOnOpen);
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0004A7FA File Offset: 0x000489FA
		public virtual void SetCursorImage(int theCursorNum, Image theImage)
		{
			this.mAppDriver.SetCursorImage(theCursorNum, theImage);
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0004A809 File Offset: 0x00048A09
		public virtual void SetCursor(ECURSOR eCURSOR)
		{
			this.mAppDriver.SetCursor((int)eCURSOR);
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0004A817 File Offset: 0x00048A17
		public virtual int GetCursor()
		{
			return this.mAppDriver.GetCursor();
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0004A824 File Offset: 0x00048A24
		public virtual void EnableCustomCursors(bool enabled)
		{
			this.mAppDriver.EnableCustomCursors(enabled);
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0004A832 File Offset: 0x00048A32
		public virtual double GetLoadingThreadProgress()
		{
			return this.mAppDriver.GetLoadingThreadProgress();
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0004A83F File Offset: 0x00048A3F
		public virtual bool RegistryWriteString(string theValueName, string theString)
		{
			return this.mAppDriver.ConfigWriteString(theValueName, theString);
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0004A84E File Offset: 0x00048A4E
		public virtual bool RegistryWriteInteger(string theValueName, int theValue)
		{
			return this.mAppDriver.ConfigWriteInteger(theValueName, theValue);
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0004A85D File Offset: 0x00048A5D
		public virtual bool RegistryWriteBoolean(string theValueName, bool theValue)
		{
			return this.mAppDriver.ConfigWriteBoolean(theValueName, theValue);
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x0004A86C File Offset: 0x00048A6C
		public virtual bool RegistryWriteData(string theValueName, byte[] theValue, ulong theLength)
		{
			return this.mAppDriver.ConfigWriteData(theValueName, theValue, theLength);
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x0004A87C File Offset: 0x00048A7C
		public virtual void WriteToRegistry()
		{
			this.RegistryWriteInteger("MusicVolume", (int)(this.mMusicVolume * 100.0));
			this.RegistryWriteInteger("SfxVolume", (int)(this.mSfxVolume * 100.0));
			this.RegistryWriteInteger("Muted", (this.mMuteCount - this.mAutoMuteCount > 0) ? 1 : 0);
			this.RegistryWriteInteger("ScreenMode", this.mIsWindowed ? 0 : 1);
			this.RegistryWriteInteger("PreferredX", this.mPreferredX);
			this.RegistryWriteInteger("PreferredY", this.mPreferredY);
			this.RegistryWriteInteger("PreferredWidth", this.mPreferredWidth);
			this.RegistryWriteInteger("PreferredHeight", this.mPreferredHeight);
			this.RegistryWriteInteger("CustomCursors", this.mCustomCursorsEnabled ? 1 : 0);
			this.RegistryWriteInteger("InProgress", 0);
			this.RegistryWriteBoolean("WaitForVSync", this.mWaitForVSync);
			this.mAppDriver.WriteToConfig();
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x0004A985 File Offset: 0x00048B85
		public virtual bool RegistryEraseKey(string _theKeyName)
		{
			return this.mAppDriver.ConfigEraseKey(_theKeyName);
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x0004A993 File Offset: 0x00048B93
		public virtual void RegistryEraseValue(string _theValueName)
		{
			this.mAppDriver.ConfigEraseValue(_theValueName);
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x0004A9A1 File Offset: 0x00048BA1
		public virtual bool RegistryGetSubKeys(string theKeyName, List<string> theSubKeys)
		{
			return this.mAppDriver.ConfigGetSubKeys(theKeyName, theSubKeys);
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0004A9B0 File Offset: 0x00048BB0
		public virtual bool RegistryReadString(string theKey, ref string theString)
		{
			return this.mAppDriver.ConfigReadString(theKey, ref theString);
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0004A9BF File Offset: 0x00048BBF
		public virtual bool RegistryReadInteger(string theKey, ref int theValue)
		{
			return this.mAppDriver.ConfigReadInteger(theKey, ref theValue);
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0004A9CE File Offset: 0x00048BCE
		public virtual bool RegistryReadBoolean(string theKey, ref bool theValue)
		{
			return this.mAppDriver.ConfigReadBoolean(theKey, ref theValue);
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0004A9DD File Offset: 0x00048BDD
		public virtual bool RegistryReadData(string theKey, byte[] theValue, ref ulong theLength)
		{
			return this.mAppDriver.ConfigReadData(theKey, ref theValue, ref theLength);
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0004A9F0 File Offset: 0x00048BF0
		public virtual void ReadFromRegistry()
		{
			this.mReadFromRegistry = true;
			this.mRegKey = this.GetString("RegistryKey", this.mRegKey);
			if (this.mRegKey.Length == 0)
			{
				return;
			}
			int num = 0;
			if (this.RegistryReadInteger("MusicVolume", ref num))
			{
				this.mMusicVolume = (double)num / 100.0;
			}
			if (this.RegistryReadInteger("SfxVolume", ref num))
			{
				this.mSfxVolume = (double)num / 100.0;
			}
			if (this.RegistryReadInteger("Muted", ref num))
			{
				this.mMuteCount = num;
			}
			if (this.RegistryReadInteger("ScreenMode", ref num))
			{
				this.mIsWindowed = num == 0 && !this.mForceFullscreen;
			}
			this.RegistryReadInteger("PreferredX", ref this.mPreferredX);
			this.RegistryReadInteger("PreferredY", ref this.mPreferredY);
			this.RegistryReadInteger("PreferredWidth", ref this.mPreferredWidth);
			this.RegistryReadInteger("PreferredHeight", ref this.mPreferredHeight);
			if (this.RegistryReadInteger("CustomCursors", ref num))
			{
				this.EnableCustomCursors(num != 0);
			}
			this.RegistryReadBoolean("WaitForVSync", ref this.mWaitForVSync);
			if (this.RegistryReadInteger("InProgress", ref num))
			{
				this.mLastShutdownWasGraceful = num == 0;
			}
			if (!this.IsScreenSaver())
			{
				this.RegistryWriteInteger("InProgress", 1);
			}
			this.mAppDriver.ReadFromConfig();
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0004AB56 File Offset: 0x00048D56
		public virtual bool WriteBytesToFile(string theFileName, byte[] theData, ulong theDataLen)
		{
			return this.mAppDriver.WriteBytesToFile(theFileName, theData, theDataLen);
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0004AB66 File Offset: 0x00048D66
		public virtual bool WriteBufferToFile(string theFileName, SexyBuffer theBuffer)
		{
			return this.WriteBytesToFile(theFileName, theBuffer.GetDataPtr(), (ulong)((long)theBuffer.GetDataLen()));
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0004AB7C File Offset: 0x00048D7C
		public bool ReadBufferFromStream(string theFileName, ref SexyBuffer theBuffer)
		{
			theBuffer.Clear();
			try
			{
				Stream stream = TitleContainer.OpenStream("Content\\" + theFileName);
				byte[] array = new byte[stream.Length];
				stream.Read(array, 0, (int)stream.Length);
				stream.Close();
				theBuffer.SetData(array, array.Length);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0004ABEC File Offset: 0x00048DEC
		public bool ReadBufferFromFile(string theFileName, ref SexyBuffer theBuffer)
		{
			PFILE pfile = new PFILE(theFileName, "rb");
			if (!pfile.Open())
			{
				return false;
			}
			byte[] data = pfile.GetData();
			int theCount = data.Length;
			theBuffer.Clear();
			theBuffer.SetData(data, theCount);
			pfile.Close();
			return true;
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0004AC34 File Offset: 0x00048E34
		public virtual bool FileExists(string theFileName)
		{
			bool flag = false;
			return this.mFileDriver.FileExists(theFileName, ref flag);
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0004AC51 File Offset: 0x00048E51
		public virtual bool EraseFile(string theFileName)
		{
			return this.mFileDriver.DeleteFile(theFileName);
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0004AC5F File Offset: 0x00048E5F
		public virtual void ShutdownHook()
		{
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0004AC61 File Offset: 0x00048E61
		public virtual void Shutdown()
		{
			this.mAppDriver.Shutdown();
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0004AC6E File Offset: 0x00048E6E
		public virtual void DoExit(int theCode)
		{
			this.mAppDriver.DoExit(theCode);
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0004AC7C File Offset: 0x00048E7C
		public virtual void UpdateFrames()
		{
			this.mUpdateCount++;
			this.mGamepadDriver.Update();
			if (!this.mMinimized && this.mWidgetManager.UpdateFrame())
			{
				this.mFPSDirtyCount++;
			}
			if (this.mResStreamsManager != null)
			{
				this.mResStreamsManager.Update();
			}
			if (this.mSoundManager != null)
			{
				this.mSoundManager.Update();
			}
			if (this.mMusicInterface != null)
			{
				this.mMusicInterface.Update();
			}
			if (this.mSaveGameDriver != null)
			{
				this.mSaveGameDriver.Update();
			}
			if (this.mProfileManager != null)
			{
				this.mProfileManager.Update();
			}
			if (this.mHttpDriver != null)
			{
				this.mHttpDriver.Update();
			}
			if (this.mLeaderboardManager != null)
			{
				this.mLeaderboardManager.Update();
			}
			if (this.mAchievementDriver != null)
			{
				this.mAchievementDriver.Update();
			}
			this.CleanSharedImages();
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0004AD63 File Offset: 0x00048F63
		public virtual void BeginPopup()
		{
			this.mAppDriver.BeginPopup();
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0004AD70 File Offset: 0x00048F70
		public virtual void EndPopup()
		{
			this.mAppDriver.EndPopup();
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0004AD7D File Offset: 0x00048F7D
		public virtual int MsgBox(string theText, string theTitle, int theFlags)
		{
			return 0;
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0004AD80 File Offset: 0x00048F80
		public virtual void Popup(string theString)
		{
			this.mAppDriver.Popup(theString);
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0004AD90 File Offset: 0x00048F90
		public void SafeDeleteWidget(Widget theWidget)
		{
			SexyAppBase.WidgetSafeDeleteInfo widgetSafeDeleteInfo = new SexyAppBase.WidgetSafeDeleteInfo();
			widgetSafeDeleteInfo.mUpdateAppDepth = this.mUpdateAppDepth;
			widgetSafeDeleteInfo.mWidget = theWidget;
			this.mSafeDeleteList.AddLast(widgetSafeDeleteInfo);
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0004ADC3 File Offset: 0x00048FC3
		public virtual bool KeyDown(int theKey)
		{
			return this.mAppDriver.KeyDown(theKey);
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0004ADD1 File Offset: 0x00048FD1
		public virtual bool DebugKeyDown(int theKey)
		{
			return this.mAppDriver.DebugKeyDown(theKey);
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0004ADDF File Offset: 0x00048FDF
		public virtual bool DebugKeyDownAsync(int theKey, bool ctrlDown, bool altDown)
		{
			return false;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0004ADE2 File Offset: 0x00048FE2
		public virtual void ShowKeyboard()
		{
			this.mAppDriver.ShowKeyboard();
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0004ADEF File Offset: 0x00048FEF
		public virtual void HideKeyboard()
		{
			this.mAppDriver.HideKeyboard();
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0004ADFC File Offset: 0x00048FFC
		public virtual void TouchBegan(SexyAppBase.Touch theTouch)
		{
			this.mWidgetManager.TouchBegan(theTouch);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0004AE0A File Offset: 0x0004900A
		public virtual void TouchEnded(SexyAppBase.Touch theTouch)
		{
			this.mWidgetManager.TouchEnded(theTouch);
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0004AE18 File Offset: 0x00049018
		public virtual void TouchMoved(SexyAppBase.Touch theTouch)
		{
			this.mWidgetManager.TouchMoved(theTouch);
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0004AE26 File Offset: 0x00049026
		public virtual void TouchesCanceled()
		{
			this.mWidgetManager.TouchesCanceled();
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0004AE33 File Offset: 0x00049033
		public virtual void GamepadButtonDown(GamepadButton theButton, int thePlayer, uint theFlags)
		{
			this.mWidgetManager.GamepadButtonDown(theButton, thePlayer, theFlags);
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0004AE43 File Offset: 0x00049043
		public virtual void GamepadButtonUp(GamepadButton theButton, int thePlayer, uint theFlags)
		{
			this.mWidgetManager.GamepadButtonUp(theButton, thePlayer, theFlags);
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0004AE53 File Offset: 0x00049053
		public virtual void GamepadAxisMove(GamepadAxis theAxis, int thePlayer, float theAxisValue)
		{
			this.mWidgetManager.GamepadAxisMove(theAxis, thePlayer, theAxisValue);
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x0004AE63 File Offset: 0x00049063
		public virtual bool IsUIOrientationAllowed(UI_ORIENTATION theOrientation)
		{
			return this.mAppDriver.IsUIOrientationAllowed(theOrientation);
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x0004AE71 File Offset: 0x00049071
		public virtual void UIOrientationChanged(UI_ORIENTATION theOrientation)
		{
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0004AE73 File Offset: 0x00049073
		public virtual UI_ORIENTATION GetUIOrientation()
		{
			return this.mAppDriver.GetUIOrientation();
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x0004AE80 File Offset: 0x00049080
		public virtual void CloseRequestAsync()
		{
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x0004AE82 File Offset: 0x00049082
		public virtual void Done3dTesting()
		{
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x0004AE84 File Offset: 0x00049084
		public virtual string NotifyCrashHook()
		{
			return "";
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x0004AE8B File Offset: 0x0004908B
		public virtual void DeleteNativeImageData()
		{
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x0004AE8D File Offset: 0x0004908D
		public virtual void DeleteExtraImageData()
		{
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x0004AE8F File Offset: 0x0004908F
		public virtual void ReInitImages()
		{
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0004AE91 File Offset: 0x00049091
		public virtual void LoadingThreadProc()
		{
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0004AE93 File Offset: 0x00049093
		public virtual void LoadingThreadCompleted()
		{
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0004AE95 File Offset: 0x00049095
		public virtual void StartLoadingThread()
		{
			this.mAppDriver.StartLoadingThread();
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0004AEA2 File Offset: 0x000490A2
		public virtual void SwitchScreenMode(bool wantWindowed, bool is3d, bool force)
		{
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0004AEA4 File Offset: 0x000490A4
		public virtual void SwitchScreenMode(bool wantWindowed)
		{
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0004AEA6 File Offset: 0x000490A6
		public virtual void SwitchScreenMode()
		{
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0004AEA8 File Offset: 0x000490A8
		public void ProcessSafeDeleteList()
		{
			foreach (SexyAppBase.WidgetSafeDeleteInfo widgetSafeDeleteInfo in this.mSafeDeleteList)
			{
				if (widgetSafeDeleteInfo.mWidget != null)
				{
					widgetSafeDeleteInfo.mWidget.Dispose();
					widgetSafeDeleteInfo.mWidget = null;
				}
			}
			this.mSafeDeleteList.Clear();
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x0004AEF9 File Offset: 0x000490F9
		public virtual bool UpdateAppStep(ref bool updated)
		{
			return this.mAppDriver.UpdateAppStep(ref updated);
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0004AF08 File Offset: 0x00049108
		public virtual bool Update(int gameTime)
		{
			bool flag = false;
			while (this.UpdateAppStep(ref flag))
			{
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0004AF27 File Offset: 0x00049127
		public virtual void Draw(int time)
		{
			if (this.mAppDriver != null)
			{
				this.mAppDriver.Draw();
			}
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x0004AF3C File Offset: 0x0004913C
		public virtual string GetGameSEHInfo()
		{
			return this.mAppDriver.GetGameSEHInfo();
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0004AF49 File Offset: 0x00049149
		public virtual void PreTerminate()
		{
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0004AF4B File Offset: 0x0004914B
		public virtual void Start()
		{
			this.mAppDriver.Start();
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x0004AF58 File Offset: 0x00049158
		public virtual bool CheckSignature(SexyBuffer theBuffer, string theFileName)
		{
			return this.mAppDriver.CheckSignature(theBuffer, theFileName);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0004AF68 File Offset: 0x00049168
		public virtual bool LoadProperties(string theFileName, bool required, bool checkSig, bool needsLocaleCorrection)
		{
			SexyBuffer buffer = new SexyBuffer();
			if (!this.ReadBufferFromFile(theFileName, ref buffer))
			{
				bool flag = false;
				if (needsLocaleCorrection && this.mResourceManager != null)
				{
					buffer.Clear();
					flag = this.ReadBufferFromFile(this.mResourceManager.GetLocaleFolder(true) + theFileName, ref buffer);
				}
				if (!flag)
				{
					if (!required)
					{
						return true;
					}
					this.Popup(this.GetString("UNABLE_OPEN_PROPERTIES", "Unable to open properties file ") + theFileName);
					return false;
				}
			}
			if (checkSig && !this.CheckSignature(buffer, theFileName))
			{
				this.Popup(this.GetString("PROPERTIES_SIG_FAILED", "Signature check failed on ") + theFileName + "'");
				return false;
			}
			PropertiesParser propertiesParser = new PropertiesParser(this);
			if (!propertiesParser.ParsePropertiesBuffer(buffer.GetDataPtr()))
			{
				this.Popup(propertiesParser.GetErrorText());
				return false;
			}
			return true;
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0004B02F File Offset: 0x0004922F
		public virtual bool LoadProperties()
		{
			return this.LoadProperties("properties/default.xml", true, false, true);
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0004B03F File Offset: 0x0004923F
		public virtual void LoadResourceManifest()
		{
			if (!this.mResourceManager.ParseResourcesFile("properties/resources.xml"))
			{
				this.ShowResourceError(true);
			}
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x0004B05A File Offset: 0x0004925A
		public virtual void ShowResourceError(bool doExit)
		{
			this.Popup(this.mResourceManager.GetErrorText());
			if (doExit)
			{
				this.DoExit(0);
			}
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0004B077 File Offset: 0x00049277
		public virtual bool ReloadAllResources()
		{
			return this.mAppDriver.ReloadAllResources();
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0004B084 File Offset: 0x00049284
		public virtual bool GetBoolean(string theId, bool theDefault)
		{
			if (this.mBoolProperties.ContainsKey(theId))
			{
				return this.mBoolProperties[theId];
			}
			return theDefault;
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0004B0A2 File Offset: 0x000492A2
		public virtual bool GetBoolean(string theId)
		{
			return this.GetBoolean(theId, false);
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0004B0AC File Offset: 0x000492AC
		public virtual int GetInteger(string theId)
		{
			return this.GetInteger(theId, 0);
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x0004B0B6 File Offset: 0x000492B6
		public virtual int GetInteger(string theId, int theDefault)
		{
			if (this.mIntProperties.ContainsKey(theId))
			{
				return this.mIntProperties[theId];
			}
			return theDefault;
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x0004B0D4 File Offset: 0x000492D4
		public virtual double GetDouble(string theId)
		{
			return this.GetDouble(theId, 0.0);
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x0004B0E6 File Offset: 0x000492E6
		public virtual double GetDouble(string theId, double theDefault)
		{
			if (this.mDoubleProperties.ContainsKey(theId))
			{
				return this.mDoubleProperties[theId];
			}
			return theDefault;
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x0004B104 File Offset: 0x00049304
		public virtual string GetString(string theId, string theDefault)
		{
			if (this.mStringProperties.ContainsKey(theId))
			{
				return this.mStringProperties[theId];
			}
			return theDefault;
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0004B122 File Offset: 0x00049322
		public virtual string GetString(string theId)
		{
			return this.GetString(theId, "");
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x0004B130 File Offset: 0x00049330
		public virtual List<string> GetStringVector(string theId)
		{
			if (this.mStringVectorProperties.ContainsKey(theId))
			{
				return this.mStringVectorProperties[theId];
			}
			return new List<string>();
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0004B152 File Offset: 0x00049352
		public virtual void SetString(string anID, string value)
		{
			if (!this.mStringProperties.ContainsKey(anID))
			{
				this.mStringProperties[anID] = value;
			}
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x0004B16F File Offset: 0x0004936F
		public virtual void SetBoolean(string anID, bool boolValue)
		{
			if (!this.mBoolProperties.ContainsKey(anID))
			{
				this.mBoolProperties[anID] = boolValue;
			}
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0004B18C File Offset: 0x0004938C
		public virtual void SetInteger(string anID, int anInt)
		{
			if (!this.mIntProperties.ContainsKey(anID))
			{
				this.mIntProperties[anID] = anInt;
			}
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x0004B1A9 File Offset: 0x000493A9
		public virtual void SetDouble(string anID, double aDouble)
		{
			if (!this.mDoubleProperties.ContainsKey(anID))
			{
				this.mDoubleProperties[anID] = aDouble;
			}
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x0004B1C6 File Offset: 0x000493C6
		public virtual void DoParseCmdLine()
		{
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x0004B1C8 File Offset: 0x000493C8
		public virtual void ParseCmdLine(string theCmdLine)
		{
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x0004B1CA File Offset: 0x000493CA
		public virtual void HandleCmdLineParam(string theParamName, string theParamValue)
		{
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x0004B1CC File Offset: 0x000493CC
		public virtual void PreDisplayHook()
		{
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0004B1CE File Offset: 0x000493CE
		public virtual void PreDDInterfaceInitHook()
		{
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x0004B1D0 File Offset: 0x000493D0
		public virtual void PostDDInterfaceInitHook()
		{
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x0004B1D2 File Offset: 0x000493D2
		public virtual bool ChangeDirHook(string theIntendedPath)
		{
			return false;
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x0004B1D5 File Offset: 0x000493D5
		public virtual void InitPropertiesHook()
		{
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x0004B1D7 File Offset: 0x000493D7
		public virtual void InitHook()
		{
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x0004B1D9 File Offset: 0x000493D9
		public virtual MusicInterface CreateMusicInterface()
		{
			if (this.mNoSoundNeeded || this.mAudioDriver == null)
			{
				return new MusicInterface();
			}
			return this.mAudioDriver.CreateMusicInterface();
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x0004B1FC File Offset: 0x000493FC
		public virtual void Init()
		{
			if (this.mAudioDriver != null)
			{
				this.mAudioDriver.InitAudioDriver();
			}
			if (this.mAppDriver != null)
			{
				//this.mAppDriver.Init();
			}
			if (this.mProfileDriver != null)
			{
				this.mProfileDriver.Init();
			}
			if (this.mSaveGameDriver != null)
			{
				this.mSaveGameDriver.Init();
			}
			if (this.mLeaderboardDriver != null)
			{
				this.mLeaderboardDriver.Init();
			}
			if (this.mAchievementDriver != null)
			{
				this.mAchievementDriver.Init();
			}
			if (this.mGamepadDriver != null)
			{
				this.mGamepadDriver.InitGamepadDriver(this);
			}
			long ticks = DateTime.Now.Ticks;
			string languageSuffix = Localization.GetLanguageSuffix(Localization.GetCurrentLanguage());
			string text = "properties/resources/resources" + languageSuffix + ".xml";
			this.mResourceManager = ((XNAFileDriver)this.mFileDriver).GetContentManager().Load<ResourceManager>(text);
			this.mResourceManager.mApp = this;
			long ticks2 = DateTime.Now.Ticks;
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x0004B2F5 File Offset: 0x000494F5
		public virtual void HandleGameAlreadyRunning()
		{
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x0004B2F8 File Offset: 0x000494F8
		public virtual DeviceImage GetImage(string theFileName, bool commitBits, bool allowTriReps, bool isInAtlas)
		{
			if (isInAtlas)
			{
				allowTriReps = false;
			}
			if (!isInAtlas && this.mResStreamsManager != null && this.mResStreamsManager.IsInitialized())
			{
				string theFileName2 = theFileName + ".ptx";
				int groupForFile = this.mResStreamsManager.GetGroupForFile(theFileName2);
				if (groupForFile != -1 && (this.mResStreamsManager.IsGroupLoaded(groupForFile) || this.mResStreamsManager.ForceLoadGroup(groupForFile, theFileName)))
				{
					Image image = new Image();
					if (this.mResStreamsManager.GetImage(groupForFile, theFileName2, ref image))
					{
						return (DeviceImage)image;
					}
				}
			}
			if (!isInAtlas)
			{
				//DeviceImage optimizedImage = this.mAppDriver.GetOptimizedImage(theFileName, commitBits, allowTriReps);
				DeviceImage optimizedImage = this.mAppDriver.GetOptimizedImage(System.IO.File.OpenRead("Content\\" + theFileName + ".png"), commitBits, allowTriReps);
				if (optimizedImage != null)
				{
					return optimizedImage;
				}
			}
			if (isInAtlas)
			{
				DeviceImage deviceImage = new DeviceImage();
				if (!allowTriReps)
				{
					deviceImage.AddImageFlags(ImageFlags.ImageFlag_NoTriRep);
				}
				deviceImage.mWidth = (deviceImage.mHeight = 0);
				deviceImage.mFilePath = theFileName;
				return deviceImage;
			}
			throw new NotSupportedException();
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0004B3DC File Offset: 0x000495DC
		protected virtual MemoryImage GetImageByName(string name)
		{
			SharedImageRef sharedImageRef = this.mResourceManager.LoadImage(name);
			if (sharedImageRef != null)
			{
				return sharedImageRef.GetMemoryImage();
			}
			return null;
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x0004B401 File Offset: 0x00049601
		public virtual void ColorizeImage(SharedImageRef theImage, SexyColor theColor)
		{
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x0004B403 File Offset: 0x00049603
		public virtual DeviceImage CreateColorizedImage(Image theImage, SexyColor theColor)
		{
			return null;
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0004B406 File Offset: 0x00049606
		public virtual DeviceImage CopyImage(Image theImage, Rect theRect)
		{
			return null;
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x0004B409 File Offset: 0x00049609
		public virtual DeviceImage CopyImage(Image theImage)
		{
			return null;
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0004B40C File Offset: 0x0004960C
		public virtual void MirrorImage(Image theImage)
		{
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x0004B40E File Offset: 0x0004960E
		public virtual void FlipImage(Image theImage)
		{
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x0004B410 File Offset: 0x00049610
		public virtual void RotateImageHue(MemoryImage theImage, int theDelta)
		{
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0004B412 File Offset: 0x00049612
		public virtual void AddMemoryImage(MemoryImage memoryImage)
		{
			if (this.mGraphicsDriver == null)
			{
				return;
			}
			new AutoCrit(this.mImageSetCritSect);
			this.mMemoryImageSet.Add(memoryImage);
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0004B435 File Offset: 0x00049635
		public virtual void RemoveMemoryImage(MemoryImage theMemoryImage)
		{
			if (this.mGraphicsDriver == null)
			{
				return;
			}
			new AutoCrit(this.mImageSetCritSect);
			if (this.mMemoryImageSet.Contains(theMemoryImage))
			{
				this.mMemoryImageSet.Remove(theMemoryImage);
			}
			this.Remove3DData(theMemoryImage);
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0004B46E File Offset: 0x0004966E
		public virtual void Remove3DData(MemoryImage theMemoryImage)
		{
			this.mAppDriver.Remove3DData(theMemoryImage);
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0004B47C File Offset: 0x0004967C
		public virtual SharedImageRef SetSharedImage(string theFileName, string theVariant, DeviceImage anImage)
		{
			bool flag = false;
			return this.SetSharedImage(theFileName, theVariant, anImage, ref flag);
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0004B498 File Offset: 0x00049698
		public virtual SharedImageRef SetSharedImage(string theFileName, string theVariant, DeviceImage theImage, ref bool isNew)
		{
			string text = theFileName.ToUpper();
			string text2 = theVariant.ToUpper();
			KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>(text, text2);
			SharedImageRef sharedImageRef = new SharedImageRef();
			if (this.mSharedImageMap.ContainsKey(keyValuePair))
			{
				SharedImage sharedImage = new SharedImage();
				new AutoCrit(this.mCritSect);
				this.mSharedImageMap[keyValuePair] = sharedImage;
				sharedImageRef.mSharedImage = sharedImage;
				isNew = true;
			}
			else
			{
				sharedImageRef.mSharedImage = this.mSharedImageMap[keyValuePair];
			}
			if (sharedImageRef.mSharedImage.mImage != theImage)
			{
				sharedImageRef.mSharedImage.mImage.Dispose();
				sharedImageRef.mSharedImage.mImage = theImage;
			}
			return sharedImageRef;
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x0004B540 File Offset: 0x00049740
		public virtual SharedImageRef CheckSharedImage(string theFileName, string theVariant)
		{
			int num = theFileName.IndexOf('|');
			string text;
			if (num != -1)
			{
				ResourceRef imageRef = this.mResourceManager.GetImageRef(theFileName.Substring(num + 1));
				if (imageRef.HasResource())
				{
					return imageRef.GetSharedImageRef();
				}
				text = theFileName.Substring(0, num);
			}
			else
			{
				text = theFileName;
			}
			string text2 = text.ToUpper();
			string text3 = theVariant.ToUpper();
			SharedImageRef result = new SharedImageRef();
			new AutoCrit(this.mCritSect);
			KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>(text2, text3);
			if (this.mSharedImageMap.ContainsKey(keyValuePair))
			{
				result = new SharedImageRef(this.mSharedImageMap[keyValuePair]);
			}
			return result;
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x0004B5E4 File Offset: 0x000497E4
		public virtual SharedImageRef GetSharedImage(string aPath)
		{
			bool flag = false;
			return this.GetSharedImage(aPath, "", ref flag, true, false);
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x0004B604 File Offset: 0x00049804
		public virtual SharedImageRef GetSharedImage(string theFileName, string theVariant, ref bool isNew, bool allowTriReps, bool isInAtlas)
		{
			int num = theFileName.IndexOf('|');
			string text;
			if (num != -1)
			{
				ResourceRef imageRef = this.mResourceManager.GetImageRef(theFileName.Substring(num + 1));
				if (imageRef.HasResource())
				{
					return imageRef.GetSharedImageRef();
				}
				text = theFileName.Substring(0, num);
			}
			else
			{
				text = theFileName;
			}
			string text2 = text.ToUpper();
			string text3 = theVariant.ToUpper();
			new AutoCrit(this.mCritSect);
			KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>(text2, text3);
			SharedImageRef sharedImageRef;
			if (this.mSharedImageMap.ContainsKey(keyValuePair))
			{
				sharedImageRef = new SharedImageRef(this.mSharedImageMap[keyValuePair]);
				sharedImageRef.mSharedImage.mLoading = true;
			}
			else
			{
				this.mSharedImageMap[keyValuePair] = new SharedImage();
				sharedImageRef = new SharedImageRef(this.mSharedImageMap[keyValuePair]);
				sharedImageRef.mSharedImage.mLoading = true;
				isNew = true;
			}
			if (sharedImageRef != null)
			{
				if (isInAtlas)
				{
					allowTriReps = false;
				}
				if (text.Length > 0 && text[0] == '!')
				{
					sharedImageRef.mSharedImage.mImage = new DeviceImage();
					if (!allowTriReps)
					{
						sharedImageRef.mSharedImage.mImage.AddImageFlags(ImageFlags.ImageFlag_NoTriRep);
					}
				}
				else
				{
					sharedImageRef.mSharedImage.mImage = this.GetImage(text, false, allowTriReps, isInAtlas);
				}
				sharedImageRef.mSharedImage.mLoading = false;
			}
			return sharedImageRef;
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x0004B758 File Offset: 0x00049958
		public virtual void CleanSharedImages()
		{
			new AutoCrit(this.mCritSect);
			if (this.mCleanupSharedImages)
			{
				foreach (KeyValuePair<KeyValuePair<string, string>, SharedImage> keyValuePair in this.mSharedImageMap)
				{
					SharedImage value = keyValuePair.Value;
					if (value.mImage != null && value.mRefCount == 0)
					{
						value.mImage.Dispose();
						value.mImage = null;
						List<KeyValuePair<string, string>> list = this.mRemoveList;
						list.Add(keyValuePair.Key);
					}
				}
				for (int i = 0; i < this.mRemoveList.Count; i++)
				{
					this.mSharedImageMap.Remove(this.mRemoveList[i]);
				}
				this.mRemoveList.Clear();
				this.mCleanupSharedImages = false;
			}
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x0004B820 File Offset: 0x00049A20
		public ulong HSLToRGB(int h, int s, int l)
		{
			double num = (double)((l < 128) ? (l * (255 + s) / 255) : (l + s - l * s / 255));
			int num2 = (int)((double)(2 * l) - num);
			int num3 = 6 * h / 256;
			int num4 = (int)((double)num2 + (num - (double)num2) * (double)((h - num3 * 256 / 6) * 6) / 255.0);
			if (num4 > 255)
			{
				num4 = 255;
			}
			int num5 = (int)(num - (num - (double)num2) * (double)((h - num3 * 256 / 6) * 6) / 255.0);
			if (num5 < 0)
			{
				num5 = 0;
			}
			int num6;
			int num7;
			int num8;
			switch (num3)
			{
			case 0:
				num6 = (int)num;
				num7 = num4;
				num8 = num2;
				break;
			case 1:
				num6 = num5;
				num7 = (int)num;
				num8 = num2;
				break;
			case 2:
				num6 = num2;
				num7 = (int)num;
				num8 = num4;
				break;
			case 3:
				num6 = num2;
				num7 = num5;
				num8 = (int)num;
				break;
			case 4:
				num6 = num4;
				num7 = num2;
				num8 = (int)num;
				break;
			case 5:
				num6 = (int)num;
				num7 = num2;
				num8 = num5;
				break;
			default:
				num6 = (int)num;
				num7 = num4;
				num8 = num2;
				break;
			}
			return (ulong)(0xffffffffff000000) | (ulong)((ulong)((long)num6) << 16) | (ulong)((ulong)((long)num7) << 8) | (ulong)((long)num8);
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x0004B958 File Offset: 0x00049B58
		public ulong RGBToHSL(int r, int g, int b)
		{
			int num = Math.Max(r, Math.Max(g, b));
			int num2 = Math.Min(r, Math.Min(g, b));
			int num3 = 0;
			int num4 = 0;
			int num5 = (num2 + num) / 2;
			int num6 = num - num2;
			if (num6 != 0)
			{
				num4 = num6 * 256 / ((num5 <= 128) ? (num2 + num) : (512 - num - num2));
				if (r == num)
				{
					num3 = ((g == num2) ? (1280 + (num - b) * 256 / num6) : (256 - (num - g) * 256 / num6));
				}
				else if (g == num)
				{
					num3 = ((b == num2) ? (256 + (num - r) * 256 / num6) : (768 - (num - b) * 256 / num6));
				}
				else
				{
					num3 = ((r == num2) ? (768 + (num - g) * 256 / num6) : (1280 - (num - r) * 256 / num6));
				}
				num3 /= 6;
			}
			return (ulong)(0xffffffffff000000) | (ulong)((long)num3) | (ulong)((long)((long)num4 << 8)) | (ulong)((long)((long)num5 << 16));
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0004BA60 File Offset: 0x00049C60
		public void HSLToRGB(ulong[] theSource, ulong[] theDest, int theSize)
		{
			for (int i = 0; i < theSize; i++)
			{
				ulong num = theSource[i];
				theDest[i] = (num & (ulong)(0xffffffffff000000)) | (this.HSLToRGB((int)(num & 255UL), (int)(num >> 8) & 255, (int)(num >> 16) & 255) & 16777215UL);
			}
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x0004BAB4 File Offset: 0x00049CB4
		public void RGBToHSL(ulong[] theSource, ulong[] theDest, int theSize)
		{
			for (int i = 0; i < theSize; i++)
			{
				ulong num = theSource[i];
				theDest[i] = (num & (ulong)(0xffffffffff000000)) | (this.RGBToHSL((int)((num >> 16) & 255UL), (int)(num >> 8) & 255, (int)(num & 255UL)) & 16777215UL);
			}
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x0004BB0C File Offset: 0x00049D0C
		public virtual void PlaySample(int theSoundNum)
		{
			if (this.mSoundManager == null)
			{
				return;
			}
			SoundInstance soundInstance = this.mSoundManager.GetSoundInstance(theSoundNum);
			if (soundInstance != null)
			{
				soundInstance.Play(false, true);
			}
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x0004BB3C File Offset: 0x00049D3C
		public virtual void PlaySample(int theSoundNum, int thePan)
		{
			if (this.mSoundManager == null)
			{
				return;
			}
			SoundInstance soundInstance = this.mSoundManager.GetSoundInstance(theSoundNum);
			if (soundInstance != null)
			{
				soundInstance.SetPan(thePan);
				soundInstance.Play(false, true);
			}
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x0004BB72 File Offset: 0x00049D72
		public bool IsMuted()
		{
			return this.mMuteCount > 0;
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0004BB7D File Offset: 0x00049D7D
		public void Mute(bool autoMute)
		{
			this.mMuteCount++;
			if (autoMute)
			{
				this.mAutoMuteCount++;
			}
			this.SetMusicVolume(this.mMusicVolume);
			this.SetSfxVolume(this.mSfxVolume);
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x0004BBB8 File Offset: 0x00049DB8
		public void Unmute(bool autoMute)
		{
			if (this.mMuteCount > 0)
			{
				this.mMuteCount--;
				if (autoMute)
				{
					this.mAutoMuteCount--;
				}
			}
			this.SetMusicVolume(this.mMusicVolume);
			this.SetSfxVolume(this.mSfxVolume);
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0004BC05 File Offset: 0x00049E05
		public virtual double GetMusicVolume()
		{
			if (this.mMusicInterface.isPlayingUserMusic())
			{
				return 0.0;
			}
			return this.mMusicVolume;
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0004BC24 File Offset: 0x00049E24
		public virtual void SetMusicVolume(double theVolume)
		{
			this.mMusicVolume = theVolume;
			if (this.mMusicInterface != null)
			{
				this.mMusicInterface.SetVolume((this.mMuteCount > 0) ? 0.0 : this.mMusicVolume);
			}
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0004BC5A File Offset: 0x00049E5A
		public virtual double GetSfxVolume()
		{
			return this.mSfxVolume;
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x0004BC62 File Offset: 0x00049E62
		public virtual void SetSfxVolume(double theVolume)
		{
			if (this.mSoundManager != null)
			{
				this.mSoundManager.SetMasterVolume(theVolume);
			}
			this.mSfxVolume = theVolume;
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x0004BC7F File Offset: 0x00049E7F
		public virtual double GetMasterVolume()
		{
			if (this.mSoundManager != null)
			{
				return this.mSoundManager.GetMasterVolume();
			}
			return 0.0;
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0004BC9E File Offset: 0x00049E9E
		public virtual void SetMasterVolume(double theMasterVolume)
		{
			if (this.mSoundManager != null)
			{
				this.mSoundManager.SetMasterVolume(theMasterVolume);
			}
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0004BCB4 File Offset: 0x00049EB4
		public virtual bool Is3DAccelerated()
		{
			return this.mAppDriver.Is3DAccelerated();
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x0004BCC1 File Offset: 0x00049EC1
		public virtual bool Is3DAccelerationSupported()
		{
			return this.mAppDriver.Is3DAccelerationSupported();
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0004BCCE File Offset: 0x00049ECE
		public virtual bool Is3DAccelerationRecommended()
		{
			return this.mAppDriver.Is3DAccelerationRecommended();
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x0004BCDB File Offset: 0x00049EDB
		public virtual void Set3DAcclerated(bool is3D, bool reinit)
		{
			this.mAppDriver.Set3DAcclerated(is3D, reinit);
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x0004BCEA File Offset: 0x00049EEA
		public virtual void LowMemoryWarning()
		{
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x0004BCEC File Offset: 0x00049EEC
		public virtual bool InitFileDriver()
		{
			if (GlobalMembers.gFileDriver == null)
			{
				GlobalMembers.gFileDriver = new XNAFileDriver();
			}
			this.mFileDriver = GlobalMembers.gFileDriver;
			return true;
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0004BD0B File Offset: 0x00049F0B
		public virtual void ButtonPress(int theId)
		{
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x0004BD0D File Offset: 0x00049F0D
		public virtual void ButtonPress(int theId, int theClickCount)
		{
			this.ButtonPress(theId);
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0004BD16 File Offset: 0x00049F16
		public virtual void ButtonDepress(int theId)
		{
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0004BD18 File Offset: 0x00049F18
		public virtual void ButtonDownTick(int theId)
		{
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0004BD1A File Offset: 0x00049F1A
		public virtual void ButtonMouseEnter(int theId)
		{
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0004BD1C File Offset: 0x00049F1C
		public virtual void ButtonMouseLeave(int theId)
		{
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0004BD1E File Offset: 0x00049F1E
		public virtual void ButtonMouseMove(int theId, int theX, int theY)
		{
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0004BD20 File Offset: 0x00049F20
		public void SetFirstLaunchStatus(bool theStatus)
		{
			this.mFirstLaunch = theStatus;
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0004BD29 File Offset: 0x00049F29
		public bool GetFirstLaunchStatus()
		{
			return this.mFirstLaunch;
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0004BD31 File Offset: 0x00049F31
		public void SetAppUpdateStatus(bool theStatus)
		{
			this.mAppUpdated = theStatus;
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0004BD3A File Offset: 0x00049F3A
		public bool GetAppUpdateStatus()
		{
			return this.mAppUpdated;
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0004BD42 File Offset: 0x00049F42
		protected virtual PIEffect GetPIEffectByName(string name)
		{
			return this.mResourceManager.LoadPIEffect(name);
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x0004BD50 File Offset: 0x00049F50
		protected virtual Font GetFontByName(string name)
		{
			return this.mResourceManager.LoadFont(name);
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x0004BD5E File Offset: 0x00049F5E
		protected virtual int GetSoundIDByName(string name)
		{
			return this.mResourceManager.LoadSound(name);
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x0004BD6C File Offset: 0x00049F6C
		protected virtual PopAnim GetPopAnimByName(string name)
		{
			return this.mResourceManager.LoadPopAnim(name);
		}

		// Token: 0x04000BC8 RID: 3016
		public const int DEMO_FILE_ID = 1119809400;

		// Token: 0x04000BC9 RID: 3017
		public const int DEMO_VERSION = 2;

		// Token: 0x04000BCA RID: 3018
		public IAppDriver mAppDriver;

		// Token: 0x04000BCB RID: 3019
		public IAudioDriver mAudioDriver;

		// Token: 0x04000BCC RID: 3020
		public IGraphicsDriver mGraphicsDriver;

		// Token: 0x04000BCD RID: 3021
		public IFileDriver mFileDriver;

		// Token: 0x04000BCE RID: 3022
		public IGamepadDriver mGamepadDriver;

		// Token: 0x04000BCF RID: 3023
		public IResStreamsDriver mResStreamsDriver;

		// Token: 0x04000BD0 RID: 3024
		public IProfileDriver mProfileDriver;

		// Token: 0x04000BD1 RID: 3025
		public ISaveGameDriver mSaveGameDriver;

		// Token: 0x04000BD2 RID: 3026
		public IHttpDriver mHttpDriver;

		// Token: 0x04000BD3 RID: 3027
		public ILeaderboardDriver mLeaderboardDriver;

		// Token: 0x04000BD4 RID: 3028
		public IAchievementDriver mAchievementDriver;

		// Token: 0x04000BD5 RID: 3029
		public uint mRandSeed;

		// Token: 0x04000BD6 RID: 3030
		public string mCompanyName;

		// Token: 0x04000BD7 RID: 3031
		public string mFullCompanyName;

		// Token: 0x04000BD8 RID: 3032
		public string mProdName;

		// Token: 0x04000BD9 RID: 3033
		public string mRegKey;

		// Token: 0x04000BDA RID: 3034
		public string mChangeDirTo;

		// Token: 0x04000BDB RID: 3035
		public int mRelaxUpdateBacklogCount;

		// Token: 0x04000BDC RID: 3036
		public int mMaxUpdateBacklog;

		// Token: 0x04000BDD RID: 3037
		public bool mPauseWhenMoving;

		// Token: 0x04000BDE RID: 3038
		public int mPreferredX;

		// Token: 0x04000BDF RID: 3039
		public int mPreferredY;

		// Token: 0x04000BE0 RID: 3040
		public int mPreferredWidth;

		// Token: 0x04000BE1 RID: 3041
		public int mPreferredHeight;

		// Token: 0x04000BE2 RID: 3042
		public int mWidth;

		// Token: 0x04000BE3 RID: 3043
		public int mHeight;

		// Token: 0x04000BE4 RID: 3044
		public int mFullscreenBits;

		// Token: 0x04000BE5 RID: 3045
		public double mMusicVolume;

		// Token: 0x04000BE6 RID: 3046
		public double mSfxVolume;

		// Token: 0x04000BE7 RID: 3047
		public double mDemoMusicVolume;

		// Token: 0x04000BE8 RID: 3048
		public double mDemoSfxVolume;

		// Token: 0x04000BE9 RID: 3049
		public bool mNoSoundNeeded;

		// Token: 0x04000BEA RID: 3050
		public bool mWantFMod;

		// Token: 0x04000BEB RID: 3051
		public bool mCmdLineParsed;

		// Token: 0x04000BEC RID: 3052
		public bool mSkipSignatureChecks;

		// Token: 0x04000BED RID: 3053
		public bool mStandardWordWrap;

		// Token: 0x04000BEE RID: 3054
		public bool mbAllowExtendedChars;

		// Token: 0x04000BEF RID: 3055
		public bool mOnlyAllowOneCopyToRun;

		// Token: 0x04000BF0 RID: 3056
		public uint mNotifyGameMessage;

		// Token: 0x04000BF1 RID: 3057
		public CritSect mCritSect = default(CritSect);

		// Token: 0x04000BF2 RID: 3058
		public CritSect mGetImageCritSect = default(CritSect);

		// Token: 0x04000BF3 RID: 3059
		public int mBetaValidate;

		// Token: 0x04000BF4 RID: 3060
		public byte[] mAdd8BitMaxTable = new byte[512];

		// Token: 0x04000BF5 RID: 3061
		public WidgetManager mWidgetManager;

		// Token: 0x04000BF6 RID: 3062
		public Dictionary<int, Dialog> mDialogMap = new Dictionary<int, Dialog>();

		// Token: 0x04000BF7 RID: 3063
		public LinkedList<Dialog> mDialogList = new LinkedList<Dialog>();

		// Token: 0x04000BF8 RID: 3064
		public uint mPrimaryThreadId;

		// Token: 0x04000BF9 RID: 3065
		public bool mSEHOccured;

		// Token: 0x04000BFA RID: 3066
		public bool mShutdown;

		// Token: 0x04000BFB RID: 3067
		public bool mExitToTop;

		// Token: 0x04000BFC RID: 3068
		public bool mIsWindowed;

		// Token: 0x04000BFD RID: 3069
		public bool mIsPhysWindowed;

		// Token: 0x04000BFE RID: 3070
		public bool mFullScreenWindow;

		// Token: 0x04000BFF RID: 3071
		public bool mForceFullscreen;

		// Token: 0x04000C00 RID: 3072
		public bool mForceWindowed;

		// Token: 0x04000C01 RID: 3073
		public bool mInitialized;

		// Token: 0x04000C02 RID: 3074
		public bool mProcessInTimer;

		// Token: 0x04000C03 RID: 3075
		public int mTimeLoaded;

		// Token: 0x04000C04 RID: 3076
		public bool mIsScreenSaver;

		// Token: 0x04000C05 RID: 3077
		public bool mAllowMonitorPowersave;

		// Token: 0x04000C06 RID: 3078
		public bool mWantsDialogCompatibility;

		// Token: 0x04000C07 RID: 3079
		public bool mNoDefer;

		// Token: 0x04000C08 RID: 3080
		public bool mFullScreenPageFlip;

		// Token: 0x04000C09 RID: 3081
		public bool mTabletPC;

		// Token: 0x04000C0A RID: 3082
		public MusicInterface mMusicInterface;

		// Token: 0x04000C0B RID: 3083
		public bool mReadFromRegistry;

		// Token: 0x04000C0C RID: 3084
		public string mRegisterLink;

		// Token: 0x04000C0D RID: 3085
		public string mProductVersion;

		// Token: 0x04000C0E RID: 3086
		public Image[] mCursorImages = new Image[13];

		// Token: 0x04000C0F RID: 3087
		public bool mIsOpeningURL;

		// Token: 0x04000C10 RID: 3088
		public bool mShutdownOnURLOpen;

		// Token: 0x04000C11 RID: 3089
		public string mOpeningURL;

		// Token: 0x04000C12 RID: 3090
		public uint mOpeningURLTime;

		// Token: 0x04000C13 RID: 3091
		public uint mLastTimerTime;

		// Token: 0x04000C14 RID: 3092
		public uint mLastBigDelayTime;

		// Token: 0x04000C15 RID: 3093
		public double mUnmutedMusicVolume;

		// Token: 0x04000C16 RID: 3094
		public double mUnmutedSfxVolume;

		// Token: 0x04000C17 RID: 3095
		public int mMuteCount;

		// Token: 0x04000C18 RID: 3096
		public int mAutoMuteCount;

		// Token: 0x04000C19 RID: 3097
		public bool mDemoMute;

		// Token: 0x04000C1A RID: 3098
		public bool mMuteOnLostFocus;

		// Token: 0x04000C1B RID: 3099
		public List<MemoryImage> mMemoryImageSet = new List<MemoryImage>();

		// Token: 0x04000C1C RID: 3100
		public List<ImageFont> mImageFontSet = new List<ImageFont>();

		// Token: 0x04000C1D RID: 3101
		public List<PIEffect> mPIEffectSet = new List<PIEffect>();

		// Token: 0x04000C1E RID: 3102
		public List<PopAnim> mPopAnimSet = new List<PopAnim>();

		// Token: 0x04000C1F RID: 3103
		public Dictionary<KeyValuePair<string, string>, SharedImage> mSharedImageMap = new Dictionary<KeyValuePair<string, string>, SharedImage>();

		// Token: 0x04000C20 RID: 3104
		public CritSect mImageSetCritSect = default(CritSect);

		// Token: 0x04000C21 RID: 3105
		public bool mCleanupSharedImages;

		// Token: 0x04000C22 RID: 3106
		public int mNonDrawCount;

		// Token: 0x04000C23 RID: 3107
		public float mFrameTime;

		// Token: 0x04000C24 RID: 3108
		public bool mIsDrawing;

		// Token: 0x04000C25 RID: 3109
		public bool mLastDrawWasEmpty;

		// Token: 0x04000C26 RID: 3110
		public bool mHasPendingDraw;

		// Token: 0x04000C27 RID: 3111
		public double mPendingUpdatesAcc;

		// Token: 0x04000C28 RID: 3112
		public double mUpdateFTimeAcc;

		// Token: 0x04000C29 RID: 3113
		public int mLastTimeCheck;

		// Token: 0x04000C2A RID: 3114
		public int mLastTime;

		// Token: 0x04000C2B RID: 3115
		public int mLastUserInputTick;

		// Token: 0x04000C2C RID: 3116
		public int mSleepCount;

		// Token: 0x04000C2D RID: 3117
		public int mDrawCount;

		// Token: 0x04000C2E RID: 3118
		public int mUpdateCount;

		// Token: 0x04000C2F RID: 3119
		public int mUpdateAppState;

		// Token: 0x04000C30 RID: 3120
		public int mUpdateAppDepth;

		// Token: 0x04000C31 RID: 3121
		public int mMaxNonDrawCount;

		// Token: 0x04000C32 RID: 3122
		public double mUpdateMultiplier;

		// Token: 0x04000C33 RID: 3123
		public bool mPaused;

		// Token: 0x04000C34 RID: 3124
		public int mFastForwardToUpdateNum;

		// Token: 0x04000C35 RID: 3125
		public bool mFastForwardToMarker;

		// Token: 0x04000C36 RID: 3126
		public bool mFastForwardStep;

		// Token: 0x04000C37 RID: 3127
		public int mLastDrawTick;

		// Token: 0x04000C38 RID: 3128
		public int mNextDrawTick;

		// Token: 0x04000C39 RID: 3129
		public int mStepMode;

		// Token: 0x04000C3A RID: 3130
		public int mCursorNum;

		// Token: 0x04000C3B RID: 3131
		public SoundManager mSoundManager;

		// Token: 0x04000C3C RID: 3132
		public LinkedList<SexyAppBase.WidgetSafeDeleteInfo> mSafeDeleteList = new LinkedList<SexyAppBase.WidgetSafeDeleteInfo>();

		// Token: 0x04000C3D RID: 3133
		public bool mMouseIn;

		// Token: 0x04000C3E RID: 3134
		public bool mRunning;

		// Token: 0x04000C3F RID: 3135
		public bool mActive;

		// Token: 0x04000C40 RID: 3136
		public bool mMinimized;

		// Token: 0x04000C41 RID: 3137
		public bool mPhysMinimized;

		// Token: 0x04000C42 RID: 3138
		public bool mIsDisabled;

		// Token: 0x04000C43 RID: 3139
		public int mDrawTime;

		// Token: 0x04000C44 RID: 3140
		public int mFPSStartTick;

		// Token: 0x04000C45 RID: 3141
		public int mFPSFlipCount;

		// Token: 0x04000C46 RID: 3142
		public int mFPSDirtyCount;

		// Token: 0x04000C47 RID: 3143
		public int mFPSTime;

		// Token: 0x04000C48 RID: 3144
		public int mFPSCount;

		// Token: 0x04000C49 RID: 3145
		public bool mShowFPS;

		// Token: 0x04000C4A RID: 3146
		public int mShowFPSMode;

		// Token: 0x04000C4B RID: 3147
		public double mVFPSUpdateTimes;

		// Token: 0x04000C4C RID: 3148
		public int mVFPSUpdateCount;

		// Token: 0x04000C4D RID: 3149
		public double mVFPSDrawTimes;

		// Token: 0x04000C4E RID: 3150
		public int mVFPSDrawCount;

		// Token: 0x04000C4F RID: 3151
		public float mCurVFPS;

		// Token: 0x04000C50 RID: 3152
		public int mScreenBltTime;

		// Token: 0x04000C51 RID: 3153
		public bool mAutoStartLoadingThread;

		// Token: 0x04000C52 RID: 3154
		public bool mLoadingThreadStarted;

		// Token: 0x04000C53 RID: 3155
		public bool mLoadingThreadCompleted;

		// Token: 0x04000C54 RID: 3156
		public bool mLoaded;

		// Token: 0x04000C55 RID: 3157
		public bool mReloadingResources;

		// Token: 0x04000C56 RID: 3158
		public float mReloadPct;

		// Token: 0x04000C57 RID: 3159
		public string mReloadText;

		// Token: 0x04000C58 RID: 3160
		public string mReloadSubText;

		// Token: 0x04000C59 RID: 3161
		public bool mYieldMainThread;

		// Token: 0x04000C5A RID: 3162
		public bool mLoadingFailed;

		// Token: 0x04000C5B RID: 3163
		public bool mCursorThreadRunning;

		// Token: 0x04000C5C RID: 3164
		public bool mSysCursor;

		// Token: 0x04000C5D RID: 3165
		public bool mCustomCursorsEnabled;

		// Token: 0x04000C5E RID: 3166
		public bool mCustomCursorDirty;

		// Token: 0x04000C5F RID: 3167
		public bool mLastShutdownWasGraceful;

		// Token: 0x04000C60 RID: 3168
		public bool mIsWideWindow;

		// Token: 0x04000C61 RID: 3169
		public bool mWriteToSexyCache;

		// Token: 0x04000C62 RID: 3170
		public bool mSexyCacheBuffers;

		// Token: 0x04000C63 RID: 3171
		public bool mWriteFontCacheDir;

		// Token: 0x04000C64 RID: 3172
		public int mNumLoadingThreadTasks;

		// Token: 0x04000C65 RID: 3173
		public int mCompletedLoadingThreadTasks;

		// Token: 0x04000C66 RID: 3174
		public bool mDebugKeysEnabled;

		// Token: 0x04000C67 RID: 3175
		public bool mEnableMaximizeButton;

		// Token: 0x04000C68 RID: 3176
		public bool mCtrlDown;

		// Token: 0x04000C69 RID: 3177
		public bool mAltDown;

		// Token: 0x04000C6A RID: 3178
		public bool mAllowAltEnter;

		// Token: 0x04000C6B RID: 3179
		public int mSyncRefreshRate;

		// Token: 0x04000C6C RID: 3180
		public bool mVSyncUpdates;

		// Token: 0x04000C6D RID: 3181
		public bool mNoVSync;

		// Token: 0x04000C6E RID: 3182
		public bool mVSyncBroken;

		// Token: 0x04000C6F RID: 3183
		public int mVSyncBrokenCount;

		// Token: 0x04000C70 RID: 3184
		public long mVSyncBrokenTestStartTick;

		// Token: 0x04000C71 RID: 3185
		public long mVSyncBrokenTestUpdates;

		// Token: 0x04000C72 RID: 3186
		public bool mWaitForVSync;

		// Token: 0x04000C73 RID: 3187
		public bool mSoftVSyncWait;

		// Token: 0x04000C74 RID: 3188
		public bool mAutoEnable3D;

		// Token: 0x04000C75 RID: 3189
		public bool mTest3D;

		// Token: 0x04000C76 RID: 3190
		public bool mNoD3D9;

		// Token: 0x04000C77 RID: 3191
		public uint mMinVidMemory3D;

		// Token: 0x04000C78 RID: 3192
		public uint mRecommendedVidMemory3D;

		// Token: 0x04000C79 RID: 3193
		public bool mWidescreenAware;

		// Token: 0x04000C7A RID: 3194
		public bool mWidescreenTranslate;

		// Token: 0x04000C7B RID: 3195
		public Rect mScreenBounds = default(Rect);

		// Token: 0x04000C7C RID: 3196
		public bool mEnableWindowAspect;

		// Token: 0x04000C7D RID: 3197
		public bool mAllowWindowResize;

		// Token: 0x04000C7E RID: 3198
		public int mOrigScreenWidth;

		// Token: 0x04000C7F RID: 3199
		public int mOrigScreenHeight;

		// Token: 0x04000C80 RID: 3200
		public bool mIsSizeCursor;

		// Token: 0x04000C81 RID: 3201
		public bool mFirstLaunch;

		// Token: 0x04000C82 RID: 3202
		public bool mAppUpdated;

		// Token: 0x04000C83 RID: 3203
		public Dictionary<string, string> mStringProperties = new Dictionary<string, string>();

		// Token: 0x04000C84 RID: 3204
		public Dictionary<string, bool> mBoolProperties = new Dictionary<string, bool>();

		// Token: 0x04000C85 RID: 3205
		public Dictionary<string, int> mIntProperties = new Dictionary<string, int>();

		// Token: 0x04000C86 RID: 3206
		public Dictionary<string, double> mDoubleProperties = new Dictionary<string, double>();

		// Token: 0x04000C87 RID: 3207
		public Dictionary<string, List<string>> mStringVectorProperties = new Dictionary<string, List<string>>();

		// Token: 0x04000C88 RID: 3208
		public ResourceManager mResourceManager;

		// Token: 0x04000C89 RID: 3209
		public EShowCompatInfoMode mShowCompatInfoMode;

		// Token: 0x04000C8A RID: 3210
		public bool mShowWidgetInspector;

		// Token: 0x04000C8B RID: 3211
		public bool mWidgetInspectorPickMode;

		// Token: 0x04000C8C RID: 3212
		public bool mWidgetInspectorLeftAnchor;

		// Token: 0x04000C8D RID: 3213
		public WidgetContainer mWidgetInspectorPickWidget;

		// Token: 0x04000C8E RID: 3214
		public WidgetContainer mWidgetInspectorCurWidget;

		// Token: 0x04000C8F RID: 3215
		public int mWidgetInspectorScrollOffset;

		// Token: 0x04000C90 RID: 3216
		public FPoint mWidgetInspectorClickPos = new FPoint();

		// Token: 0x04000C91 RID: 3217
		public ResStreamsManager mResStreamsManager;

		// Token: 0x04000C92 RID: 3218
		public ProfileManager mProfileManager;

		// Token: 0x04000C93 RID: 3219
		public LeaderboardManager mLeaderboardManager;

		// Token: 0x04000C94 RID: 3220
		public bool mAllowSwapScreenImage;

		// Token: 0x04000C95 RID: 3221
		public static bool sAttemptingNonRecommended3D;

		// Token: 0x04000C96 RID: 3222
		public int mGamepadLocked;

		// Token: 0x04000C97 RID: 3223
		public bool mHasFocus;

		// Token: 0x04000C98 RID: 3224
		private List<KeyValuePair<string, string>> mRemoveList = new List<KeyValuePair<string, string>>();

		// Token: 0x020001A4 RID: 420
		public class Touch
		{
			// Token: 0x06000F5C RID: 3932 RVA: 0x0004BD7C File Offset: 0x00049F7C
			public void SetTouchInfo(SexyPoint loc, _TouchPhase _phase, double _timestamp)
			{
				this.previousLocation = this.location;
				this.location = loc;
				this.phase = _phase;
				this.timestamp = _timestamp / 1000.0;
			}

			// Token: 0x04000C99 RID: 3225
			public IntPtr ident;

			// Token: 0x04000C9A RID: 3226
			public IntPtr _event;

			// Token: 0x04000C9B RID: 3227
			public SexyPoint location = new SexyPoint();

			// Token: 0x04000C9C RID: 3228
			public SexyPoint previousLocation = new SexyPoint();

			// Token: 0x04000C9D RID: 3229
			public int tapCount;

			// Token: 0x04000C9E RID: 3230
			public double timestamp;

			// Token: 0x04000C9F RID: 3231
			public _TouchPhase phase;
		}

		// Token: 0x020001A5 RID: 421
		public class WidgetSafeDeleteInfo
		{
			// Token: 0x04000CA0 RID: 3232
			public int mUpdateAppDepth;

			// Token: 0x04000CA1 RID: 3233
			public Widget mWidget;
		}
	}
}
