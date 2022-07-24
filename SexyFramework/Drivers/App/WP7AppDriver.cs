using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SexyFramework.Drivers.Graphics;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.Drivers.App
{
	// Token: 0x02000013 RID: 19
	public class WP7AppDriver : IAppDriver
	{
		// Token: 0x060000BB RID: 187 RVA: 0x00003E4A File Offset: 0x0000204A
		public static WP7AppDriver CreateAppDriver(SexyAppBase App)
		{
			if (WP7AppDriver.sWP7AppDriverInstance == null)
			{
				WP7AppDriver.sWP7AppDriverInstance = new WP7AppDriver(App);
			}
			return WP7AppDriver.sWP7AppDriverInstance;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003E63 File Offset: 0x00002063
		public WP7AppDriver(SexyAppBase appBase)
		{
			this.mApp = appBase;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00003E72 File Offset: 0x00002072
		public override void Dispose()
		{
			this.Shutdown();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003E7C File Offset: 0x0000207C
		public override bool InitAppDriver()
		{
			this.mApp.mNotifyGameMessage = 0U;
			this.mApp.mOnlyAllowOneCopyToRun = true;
			this.mApp.mNoDefer = false;
			this.mApp.mFullScreenPageFlip = true;
			this.mApp.mTimeLoaded = this.GetTickCount();
			this.mApp.mSEHOccured = false;
			this.mApp.mProdName = "Product";
			this.mApp.mShutdown = false;
			this.mApp.mExitToTop = false;
			this.mApp.mWidth = 600;
			this.mApp.mHeight = 600;
			this.mApp.mFullscreenBits = 16;
			this.mApp.mIsWindowed = true;
			this.mApp.mIsPhysWindowed = true;
			this.mApp.mFullScreenWindow = false;
			this.mApp.mPreferredX = -1;
			this.mApp.mPreferredY = -1;
			this.mApp.mPreferredWidth = -1;
			this.mApp.mPreferredHeight = -1;
			this.mApp.mIsScreenSaver = false;
			this.mApp.mAllowMonitorPowersave = true;
			this.mApp.mWantsDialogCompatibility = false;
			this.mApp.mFrameTime = 10f;
			this.mApp.mNonDrawCount = 0;
			this.mApp.mDrawCount = 0;
			this.mApp.mSleepCount = 0;
			this.mApp.mUpdateCount = 0;
			this.mApp.mUpdateAppState = 0;
			this.mApp.mUpdateAppDepth = 0;
			this.mApp.mPendingUpdatesAcc = 0.0;
			this.mApp.mUpdateFTimeAcc = 0.0;
			this.mApp.mHasPendingDraw = true;
			this.mApp.mIsDrawing = false;
			this.mApp.mLastDrawWasEmpty = false;
			this.mApp.mLastTimeCheck = 0;
			this.mApp.mUpdateMultiplier = 1.0;
			this.mApp.mMaxNonDrawCount = 10;
			this.mApp.mPaused = false;
			this.mApp.mFastForwardToUpdateNum = 0;
			this.mApp.mFastForwardToMarker = false;
			this.mApp.mFastForwardStep = false;
			this.mApp.mCursorNum = 13;
			this.mApp.mMouseIn = false;
			this.mApp.mRunning = false;
			this.mApp.mActive = true;
			this.mApp.mProcessInTimer = false;
			this.mApp.mMinimized = false;
			this.mApp.mPhysMinimized = false;
			this.mApp.mIsDisabled = false;
			this.mApp.mLoaded = false;
			this.mApp.mReloadingResources = false;
			this.mApp.mReloadPct = 0f;
			this.mApp.mYieldMainThread = false;
			this.mApp.mLoadingFailed = false;
			this.mApp.mLoadingThreadStarted = false;
			this.mApp.mAutoStartLoadingThread = true;
			this.mApp.mLoadingThreadCompleted = false;
			this.mApp.mCursorThreadRunning = false;
			this.mApp.mNumLoadingThreadTasks = 0;
			this.mApp.mCompletedLoadingThreadTasks = 0;
			this.mApp.mLastDrawTick = this.timeGetTime();
			this.mApp.mNextDrawTick = this.timeGetTime();
			this.mApp.mSysCursor = true;
			this.mApp.mForceFullscreen = false;
			this.mApp.mForceWindowed = false;
			this.mApp.mHasFocus = true;
			this.mApp.mIsOpeningURL = false;
			this.mApp.mInitialized = false;
			this.mApp.mLastShutdownWasGraceful = true;
			this.mApp.mReadFromRegistry = false;
			this.mApp.mCmdLineParsed = false;
			this.mApp.mSkipSignatureChecks = false;
			this.mApp.mCtrlDown = false;
			this.mApp.mAltDown = false;
			this.mApp.mAllowAltEnter = true;
			this.mApp.mStepMode = 1;
			this.mApp.mCleanupSharedImages = false;
			this.mApp.mStandardWordWrap = true;
			this.mApp.mbAllowExtendedChars = true;
			this.mApp.mEnableMaximizeButton = false;
			this.mApp.mWriteToSexyCache = true;
			this.mApp.mSexyCacheBuffers = false;
			this.mApp.mWriteFontCacheDir = true;
			this.mApp.mMusicVolume = 0.85;
			this.mApp.mSfxVolume = 0.85;
			this.mApp.mMuteCount = 0;
			this.mApp.mAutoMuteCount = 0;
			this.mApp.mDemoMute = false;
			this.mApp.mMuteOnLostFocus = true;
			this.mApp.mFPSTime = 0;
			this.mApp.mFPSStartTick = this.GetTickCount();
			this.mApp.mFPSFlipCount = 0;
			this.mApp.mFPSCount = 0;
			this.mApp.mFPSDirtyCount = 0;
			this.mApp.mShowFPS = false;
			this.mApp.mShowFPSMode = 0;
			this.mApp.mVFPSUpdateTimes = 0.0;
			this.mApp.mVFPSUpdateCount = 0;
			this.mApp.mVFPSDrawTimes = 0.0;
			this.mApp.mVFPSDrawCount = 0;
			this.mApp.mCurVFPS = 0f;
			this.mApp.mDrawTime = 0;
			this.mApp.mScreenBltTime = 0;
			this.mApp.mDebugKeysEnabled = false;
			this.mApp.mNoSoundNeeded = false;
			this.mApp.mWantFMod = false;
			this.mApp.mSyncRefreshRate = 100;
			this.mApp.mVSyncUpdates = false;
			this.mApp.mNoVSync = true;
			this.mApp.mVSyncBroken = false;
			this.mApp.mVSyncBrokenCount = 0;
			this.mApp.mVSyncBrokenTestStartTick = 0L;
			this.mApp.mVSyncBrokenTestUpdates = 0L;
			this.mApp.mWaitForVSync = false;
			this.mApp.mSoftVSyncWait = true;
			this.mApp.mAutoEnable3D = false;
			this.mApp.mTest3D = false;
			this.mApp.mNoD3D9 = false;
			this.mApp.mMinVidMemory3D = 6U;
			this.mApp.mRecommendedVidMemory3D = 14U;
			this.mApp.mRelaxUpdateBacklogCount = 0;
			this.mApp.mWidescreenAware = false;
			this.mApp.mWidescreenTranslate = true;
			this.mApp.mEnableWindowAspect = false;
			this.mApp.mIsWideWindow = false;
			this.mApp.mOrigScreenWidth = 800;
			this.mApp.mOrigScreenHeight = 400;
			this.mApp.mIsSizeCursor = false;
			for (int i = 0; i < 13; i++)
			{
				this.mApp.mCursorImages[i] = null;
			}
			for (int i = 0; i < 256; i++)
			{
				this.mApp.mAdd8BitMaxTable[i] = (byte)i;
			}
			for (int i = 256; i < 512; i++)
			{
				this.mApp.mAdd8BitMaxTable[i] = byte.MaxValue;
			}
			this.mApp.mPrimaryThreadId = 0U;
			this.mApp.mShowWidgetInspector = false;
			this.mApp.mWidgetInspectorCurWidget = null;
			this.mApp.mWidgetInspectorScrollOffset = 0;
			this.mApp.mWidgetInspectorPickWidget = null;
			this.mApp.mWidgetInspectorPickMode = false;
			this.mApp.mWidgetInspectorLeftAnchor = false;
			GlobalMembers.gIs3D = true;
			return true;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000045B7 File Offset: 0x000027B7
		public override void Start()
		{
			if (this.mApp.mShutdown)
			{
				return;
			}
			if (this.mApp.mAutoStartLoadingThread)
			{
				this.StartLoadingThread();
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000045DC File Offset: 0x000027DC
		public override void Init()
		{
			if (this.mApp.mShutdown)
			{
				return;
			}
			this.mApp.mFileDriver.InitFileDriver(this.mApp);
			this.mApp.mFileDriver.InitSaveDataFolder();
			this.mApp.mRandSeed = (uint)this.GetTickCount();
			this.mXNAGraphicsDriver.Init();
			this.mApp.mSoundManager = this.mApp.mAudioDriver.CreateSoundManager();
			this.mApp.mMusicInterface = new SoundEffectMusicInterface();
			this.mApp.SetMusicVolume(this.mApp.mMusicVolume);
			this.IsScreenSaver();
			this.mApp.mScreenBounds.mWidth = this.mApp.mWidth;
			this.mApp.mScreenBounds.mHeight = this.mApp.mHeight;
			this.mApp.mWidgetManager.Resize(this.mApp.mScreenBounds, this.mApp.mScreenBounds);
			this.mApp.mWidgetManager.mImage = null;
			this.mApp.mWidgetManager.MarkAllDirty();
			this.mApp.mInitialized = true;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000470C File Offset: 0x0000290C
		public override bool UpdateAppStep(ref bool updated)
		{
			updated = false;
			if (this.mApp.mExitToTop)
			{
				return false;
			}
			this.mApp.mUpdateAppState = 1;
			this.mApp.mUpdateAppDepth++;
			if (this.mApp.mStepMode != 0)
			{
				this.DoUpdateFrames();
				this.DoUpdateFrames();
				this.DoUpdateFramesF(2f);
				updated = true;
			}
			else
			{
				int mUpdateCount = this.mApp.mUpdateCount;
				this.DoUpdateFrames();
				this.DoUpdateFrames();
				this.DoUpdateFramesF(2f);
				updated = this.mApp.mUpdateCount != mUpdateCount;
				updated = true;
			}
			this.mApp.mUpdateAppDepth--;
			this.mApp.ProcessSafeDeleteList();
			return true;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000047CF File Offset: 0x000029CF
		public override void ClearUpdateBacklog(bool relaxForASecond)
		{
			this.mApp.mLastTimeCheck = this.timeGetTime();
			this.mApp.mUpdateFTimeAcc = 0.0;
			if (relaxForASecond)
			{
				this.mApp.mRelaxUpdateBacklogCount = 1000;
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004809 File Offset: 0x00002A09
		public override void Shutdown()
		{
			this.mWP7Game.Exit();
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004816 File Offset: 0x00002A16
		public override void DoExit(int theCode)
		{
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004818 File Offset: 0x00002A18
		public override void Remove3DData(MemoryImage theMemoryImage)
		{
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000481A File Offset: 0x00002A1A
		public override void BeginPopup()
		{
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000481C File Offset: 0x00002A1C
		public override void EndPopup()
		{
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000481E File Offset: 0x00002A1E
		public override int MsgBox(string theText, string theTitle, int theFlags)
		{
			return 0;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004821 File Offset: 0x00002A21
		public override void Popup(string theString)
		{
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004823 File Offset: 0x00002A23
		public override bool OpenURL(string theURL, bool shutdownOnOpen)
		{
			return true;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004826 File Offset: 0x00002A26
		public override string GetGameSEHInfo()
		{
			return "";
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000482D File Offset: 0x00002A2D
		public override void SEHOccured()
		{
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000482F File Offset: 0x00002A2F
		public override void GetSEHWebParams(DefinesMap theDefinesMap)
		{
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004831 File Offset: 0x00002A31
		public override void DoParseCmdLine()
		{
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004833 File Offset: 0x00002A33
		public override void ParseCmdLine(string theCmdLine)
		{
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004835 File Offset: 0x00002A35
		public override void HandleCmdLineParam(string theParamName, string theParamValue)
		{
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004837 File Offset: 0x00002A37
		public override void StartLoadingThread()
		{
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004839 File Offset: 0x00002A39
		public override double GetLoadingThreadProgress()
		{
			return 1.0;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004844 File Offset: 0x00002A44
		public override void CopyToClipboard(string theString)
		{
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004846 File Offset: 0x00002A46
		public override string GetClipboard()
		{
			return "";
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000484D File Offset: 0x00002A4D
		public override void SetCursor(int theCursorNum)
		{
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000484F File Offset: 0x00002A4F
		public override int GetCursor()
		{
			return 0;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004852 File Offset: 0x00002A52
		public override void EnableCustomCursors(bool enabled)
		{
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004854 File Offset: 0x00002A54
		public override void SetCursorImage(int theCursorNum, Image theImage)
		{
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004856 File Offset: 0x00002A56
		public override void SwitchScreenMode()
		{
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004858 File Offset: 0x00002A58
		public override void SwitchScreenMode(bool wantWindowed)
		{
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000485A File Offset: 0x00002A5A
		public override void SwitchScreenMode(bool wantWindowed, bool is3d, bool force)
		{
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000485C File Offset: 0x00002A5C
		public override bool KeyDown(int theKey)
		{
			return false;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000485F File Offset: 0x00002A5F
		public override bool DebugKeyDown(int theKey)
		{
			return false;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004862 File Offset: 0x00002A62
		public override bool DebugKeyDownAsync(int theKey, bool ctrlDown, bool altDown)
		{
			return false;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004865 File Offset: 0x00002A65
		public override bool Is3DAccelerated()
		{
			return true;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004868 File Offset: 0x00002A68
		public override bool Is3DAccelerationSupported()
		{
			return true;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000486B File Offset: 0x00002A6B
		public override bool Is3DAccelerationRecommended()
		{
			return true;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000486E File Offset: 0x00002A6E
		public override void Set3DAcclerated(bool is3D, bool reinit)
		{
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004870 File Offset: 0x00002A70
		public override bool IsUIOrientationAllowed(UI_ORIENTATION theOrientation)
		{
			return false;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004873 File Offset: 0x00002A73
		public override UI_ORIENTATION GetUIOrientation()
		{
			return UI_ORIENTATION.UI_ORIENTATION_LANDSCAPE_RIGHT;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004876 File Offset: 0x00002A76
		public override bool IsSystemUIShowing()
		{
			return false;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004879 File Offset: 0x00002A79
		public override void ShowKeyboard()
		{
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000487B File Offset: 0x00002A7B
		public override void HideKeyboard()
		{
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000487D File Offset: 0x00002A7D
		public override bool CheckSignature(SexyBuffer theBuffer, string theFileName)
		{
			return true;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004880 File Offset: 0x00002A80
		public override bool ReloadAllResources()
		{
			return true;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00004883 File Offset: 0x00002A83
		public override bool ConfigGetSubKeys(string theKeyName, List<string> theSubKeys)
		{
			return true;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004886 File Offset: 0x00002A86
		public override bool ConfigReadString(string theValueName, ref string theString)
		{
			return true;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004889 File Offset: 0x00002A89
		public override bool ConfigReadInteger(string theValueName, ref int theValue)
		{
			return true;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000488C File Offset: 0x00002A8C
		public override bool ConfigReadBoolean(string theValueName, ref bool theValue)
		{
			return true;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000488F File Offset: 0x00002A8F
		public override bool ConfigReadData(string theValueName, ref byte[] theValue, ref ulong theLength)
		{
			return true;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004892 File Offset: 0x00002A92
		public override bool ConfigWriteString(string theValueName, string theString)
		{
			return true;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004895 File Offset: 0x00002A95
		public override bool ConfigWriteInteger(string theValueName, int theValue)
		{
			return true;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004898 File Offset: 0x00002A98
		public override bool ConfigWriteBoolean(string theValueName, bool theValue)
		{
			return true;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000489B File Offset: 0x00002A9B
		public override bool ConfigWriteData(string theValueName, byte[] theValue, ulong theLength)
		{
			return true;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000489E File Offset: 0x00002A9E
		public override bool ConfigEraseKey(string theKeyName)
		{
			return true;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000048A1 File Offset: 0x00002AA1
		public override void ConfigEraseValue(string theValueName)
		{
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000048A3 File Offset: 0x00002AA3
		public override void ReadFromConfig()
		{
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000048A5 File Offset: 0x00002AA5
		public override void WriteToConfig()
		{
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000048A7 File Offset: 0x00002AA7
		public override bool WriteBufferToFile(string theFileName, SexyBuffer theBuffer)
		{
			return true;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000048AA File Offset: 0x00002AAA
		public override bool ReadBufferFromFile(string theFileName, SexyBuffer theBuffer, bool dontWriteToDemo)
		{
			return true;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000048AD File Offset: 0x00002AAD
		public override bool WriteBytesToFile(string theFileName, object theData, ulong theDataLen)
		{
			return true;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000048B0 File Offset: 0x00002AB0
		public override DeviceImage GetOptimizedImage(string theFileName, bool commitBits, bool allowTriReps)
		{
			return ((XNAGraphicsDriver)this.mApp.mGraphicsDriver).GetOptimizedImage(theFileName, commitBits, allowTriReps);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000048CA File Offset: 0x00002ACA
		public override DeviceImage GetOptimizedImage(Stream stream, bool commitBits, bool allowTriReps)
		{
			return ((XNAGraphicsDriver)this.mApp.mGraphicsDriver).GetOptimizedImage(stream, commitBits, allowTriReps);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000048E4 File Offset: 0x00002AE4
		public override DeviceImage GetOptimizedImageFromData(string theFileName, bool commitBits, bool allowTriReps, int width, int height)
		{
			return ((XNAGraphicsDriver)this.mApp.mGraphicsDriver).GetOptimizedImageFromData(theFileName, commitBits, allowTriReps, width, height);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004902 File Offset: 0x00002B02
		public override object GetOptimizedRenderData(string theFileName)
		{
			return ((XNAGraphicsDriver)this.mApp.mGraphicsDriver).GetOptimizedRenderData(theFileName);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000491A File Offset: 0x00002B1A
		public override bool ShouldPauseUpdates()
		{
			return false;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000491D File Offset: 0x00002B1D
		public override void Draw()
		{
			this.mXNAGraphicsDriver.ClearColorBuffer(SexyColor.Black);
			this.DrawDirtyStuff();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004938 File Offset: 0x00002B38
		public override int GetAppTime()
		{
			return (int)DateTime.Now.TimeOfDay.TotalMilliseconds;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000495C File Offset: 0x00002B5C
		public override Localization.LanguageType GetAppLanguage()
		{
			Localization.LanguageType result = Localization.LanguageType.Language_EN;
			string twoLetterISOLanguageName = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
			string name = CultureInfo.CurrentCulture.Name;
			bool flag = false;
			string[] array = new string[] { "es-CO", "pt-BR", "zh-TW" };
			Localization.LanguageType[] array2 = new Localization.LanguageType[]
			{
				Localization.LanguageType.Language_SPC,
				Localization.LanguageType.Language_PGB,
				Localization.LanguageType.Language_CHT
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (name.Equals(array[i]))
				{
					result = array2[i];
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				string[] array3 = new string[] { "en", "fr", "it", "de", "es", "zh", "ru", "pl", "pt" };
				for (int j = 0; j < array3.Length; j++)
				{
					if (twoLetterISOLanguageName.Equals(array3[j]))
					{
						result = (Localization.LanguageType)j;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00004A7C File Offset: 0x00002C7C
		public void InitXNADriver(Game game)
		{
			this.mWP7Game = game;
			this.mXNAGraphicsDriver = new XNAGraphicsDriver(this.mWP7Game, this.mApp);
			this.mContentManager = this.mWP7Game.Content;
			this.mGameTime = new GameTime();
			this.mApp.mGraphicsDriver = this.mXNAGraphicsDriver;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00004AD4 File Offset: 0x00002CD4
		public int timeGetTime()
		{
			if (this.mGameTime != null)
			{
				return this.mGameTime.TotalGameTime.Milliseconds;
			}
			return 0;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00004AFE File Offset: 0x00002CFE
		public int GetTickCount()
		{
			return 0;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004B01 File Offset: 0x00002D01
		public bool IsScreenSaver()
		{
			return this.mApp.mIsScreenSaver;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00004B0E File Offset: 0x00002D0E
		public bool AppCanRestore()
		{
			return !this.mApp.mIsDisabled;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004B1E File Offset: 0x00002D1E
		public void ReloadAllResources_DrawStateUpdate(string theHeader, string theSubText, float thePct)
		{
			MemoryImage mImage = this.mApp.mWidgetManager.mImage;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00004B31 File Offset: 0x00002D31
		public void ReloadAllResourcesProc()
		{
			this.mApp.mReloadingResources = false;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00004B3F File Offset: 0x00002D3F
		public void ReloadAllResourcesProcStub(IntPtr theArg)
		{
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00004B44 File Offset: 0x00002D44
		public string GetProductVersion(string thePath)
		{
			string fullName = Assembly.GetCallingAssembly().FullName;
			return "v" + fullName.Split(new char[] { '=' })[1].Split(new char[] { ',' })[0];
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00004B90 File Offset: 0x00002D90
		private bool Process()
		{
			if (this.mApp.mLoadingFailed)
			{
				this.mApp.Shutdown();
			}
			bool flag = this.mApp.mVSyncUpdates && !this.mApp.mLastDrawWasEmpty && !this.mApp.mVSyncBroken && (!this.mApp.mIsPhysWindowed || (this.mApp.mIsPhysWindowed && this.mApp.mWaitForVSync && !this.mApp.mSoftVSyncWait));
			double num;
			double num2;
			if (this.mApp.mVSyncUpdates)
			{
				num = 1000.0 / (double)this.mApp.mSyncRefreshRate / this.mApp.mUpdateMultiplier;
				num2 = (double)((float)(1000.0 / (double)(this.mApp.mFrameTime * (float)this.mApp.mSyncRefreshRate)));
			}
			else
			{
				num = (double)this.mApp.mFrameTime / this.mApp.mUpdateMultiplier;
				num2 = 1.0;
			}
			if (!this.mApp.mPaused && this.mApp.mUpdateMultiplier > 0.0)
			{
				int num3 = this.timeGetTime();
				int num4 = 0;
				if (!flag)
				{
					this.UpdateFTimeAcc();
				}
				bool flag2 = false;
				if (this.mApp.mUpdateAppState == 1)
				{
					if (++this.mApp.mNonDrawCount < (int)Math.Ceiling((double)this.mApp.mMaxNonDrawCount * this.mApp.mUpdateMultiplier) || !this.mApp.mLoaded)
					{
						bool flag3 = true;
						if (flag3)
						{
							if (this.mApp.mUpdateMultiplier == 1.0)
							{
								this.mApp.mVSyncBrokenTestUpdates += 1L;
								if ((float)this.mApp.mVSyncBrokenTestUpdates >= (1000f + this.mApp.mFrameTime - 1f) / this.mApp.mFrameTime)
								{
									if ((long)num3 - this.mApp.mVSyncBrokenTestStartTick <= 800L)
									{
										this.mApp.mVSyncBrokenCount++;
										if (this.mApp.mVSyncBrokenCount >= 3)
										{
											this.mApp.mVSyncBroken = true;
										}
									}
									else
									{
										this.mApp.mVSyncBrokenCount = 0;
									}
									this.mApp.mVSyncBrokenTestStartTick = (long)num3;
									this.mApp.mVSyncBrokenTestUpdates = 0L;
								}
							}
							bool flag4 = this.DoUpdateFrames();
							if (flag4)
							{
								this.mApp.mUpdateAppState = 2;
							}
							this.mApp.mHasPendingDraw = true;
							flag2 = true;
						}
					}
				}
				else if (this.mApp.mUpdateAppState == 2)
				{
					this.mApp.mUpdateAppState = 3;
					this.mApp.mPendingUpdatesAcc += num2;
					this.mApp.mPendingUpdatesAcc -= 1.0;
					while (this.mApp.mPendingUpdatesAcc >= 1.0)
					{
						this.mApp.mNonDrawCount++;
						bool flag5 = this.DoUpdateFrames();
						if (!flag5)
						{
							break;
						}
						this.mApp.mPendingUpdatesAcc -= 1.0;
					}
					this.DoUpdateFramesF((float)num2);
					if (flag)
					{
						this.mApp.mUpdateFTimeAcc = Math.Max(this.mApp.mUpdateFTimeAcc - num - 0.20000000298023224, 0.0);
					}
					else
					{
						this.mApp.mUpdateFTimeAcc -= num;
					}
					if (this.mApp.mRelaxUpdateBacklogCount > 0)
					{
						this.mApp.mUpdateFTimeAcc = 0.0;
					}
					flag2 = true;
				}
				if (!flag2)
				{
					this.mApp.mUpdateAppState = 3;
					this.mApp.mNonDrawCount = 0;
					if (this.mApp.mHasPendingDraw)
					{
						this.DrawDirtyStuff();
					}
					else
					{
						int num5 = (int)num - (int)this.mApp.mUpdateFTimeAcc;
						if (num5 > 0)
						{
							this.mApp.mSleepCount++;
							Thread.Sleep(num5);
							num4 += num5;
						}
					}
				}
				if (this.mApp.mYieldMainThread)
				{
					int num6 = this.timeGetTime();
					int num7 = num6 - num3 - num4;
					int num8 = Math.Min(250, num7 * 2 - num4);
					if (num8 >= 0)
					{
						Thread.Sleep(num8);
					}
				}
			}
			return true;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005000 File Offset: 0x00003200
		private void UpdateFTimeAcc()
		{
			int num = this.timeGetTime();
			if (this.mApp.mLastTimeCheck != 0)
			{
				int num2 = num - this.mApp.mLastTimeCheck;
				this.mApp.mUpdateFTimeAcc = Math.Min(this.mApp.mUpdateFTimeAcc + (double)num2, (double)((float)this.mApp.mMaxUpdateBacklog));
				if (this.mApp.mRelaxUpdateBacklogCount > 0)
				{
					this.mApp.mRelaxUpdateBacklogCount = Math.Max(this.mApp.mRelaxUpdateBacklogCount - num2, 0);
				}
			}
			this.mApp.mLastTimeCheck = num;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005092 File Offset: 0x00003292
		private void ReDraw()
		{
			this.mXNAGraphicsDriver.Redraw(Rect.ZERO_RECT);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000050A8 File Offset: 0x000032A8
		private bool DrawDirtyStuff()
		{
			int num = this.timeGetTime();
			this.mApp.mIsDrawing = true;
			bool flag = this.mApp.mWidgetManager.DrawScreen();
			this.mApp.mIsDrawing = false;
			if ((flag || num - this.mApp.mLastDrawTick >= 1000 || this.mApp.mCustomCursorDirty) && num - this.mApp.mNextDrawTick >= 0)
			{
				this.mApp.mLastDrawWasEmpty = false;
				this.mApp.mDrawCount++;
				int num2 = this.timeGetTime();
				this.mApp.mFPSCount++;
				this.mApp.mFPSTime += num2 - num;
				this.mApp.mDrawTime += num2 - num;
				int num3 = this.timeGetTime();
				this.mApp.mLastDrawTick = num3;
				this.ReDraw();
				int num4 = this.timeGetTime();
				this.mApp.mScreenBltTime = num4 - num3;
				if (this.mApp.mLoadingThreadStarted && !this.mApp.mLoadingThreadCompleted)
				{
					int num5 = num4 - num;
					this.mApp.mNextDrawTick += 35 + Math.Max(num5, 15);
					if (num4 - this.mApp.mNextDrawTick >= 0)
					{
						this.mApp.mNextDrawTick = num4;
					}
				}
				else
				{
					this.mApp.mNextDrawTick = num4;
				}
				this.mApp.mHasPendingDraw = false;
				this.mApp.mCustomCursorDirty = false;
				return true;
			}
			this.mApp.mHasPendingDraw = false;
			this.mApp.mLastDrawWasEmpty = true;
			return false;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005250 File Offset: 0x00003450
		private void DoUpdateFramesF(float theFrac)
		{
			if (this.mApp.mVSyncUpdates && !this.mApp.mMinimized)
			{
				this.mApp.mWidgetManager.UpdateFrameF(theFrac);
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005280 File Offset: 0x00003480
		private bool DoUpdateFrames()
		{
			if (this.mApp.mLoadingThreadCompleted && !this.mApp.mLoaded)
			{
				this.mApp.mLoaded = true;
				this.mApp.mYieldMainThread = false;
				this.mApp.LoadingThreadCompleted();
			}
			this.mApp.UpdateFrames();
			return true;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000052D6 File Offset: 0x000034D6
		public void SetOrientation(int Orientation)
		{
			this.mXNAGraphicsDriver.SetOrientation(Orientation);
		}

		// Token: 0x0400003E RID: 62
		public static WP7AppDriver sWP7AppDriverInstance;

		// Token: 0x0400003F RID: 63
		private SexyAppBase mApp;

		// Token: 0x04000040 RID: 64
		private Game mWP7Game;

		// Token: 0x04000041 RID: 65
		private GameTime mGameTime;

		// Token: 0x04000042 RID: 66
		public ContentManager mContentManager;

		// Token: 0x04000043 RID: 67
		private XNAGraphicsDriver mXNAGraphicsDriver;
	}
}
