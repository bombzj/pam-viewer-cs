using System;
using System.Collections.Generic;
using System.IO;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.Drivers
{
	// Token: 0x02000012 RID: 18
	public abstract class IAppDriver
	{
		// Token: 0x06000075 RID: 117 RVA: 0x00003E40 File Offset: 0x00002040
		public virtual void Dispose()
		{
		}

		// Token: 0x06000076 RID: 118
		public abstract bool InitAppDriver();

		// Token: 0x06000077 RID: 119
		public abstract void Start();

		// Token: 0x06000078 RID: 120
		public abstract void Init();

		// Token: 0x06000079 RID: 121
		public abstract bool UpdateAppStep(ref bool updated);

		// Token: 0x0600007A RID: 122
		public abstract void ClearUpdateBacklog(bool relaxForASecond);

		// Token: 0x0600007B RID: 123
		public abstract void Shutdown();

		// Token: 0x0600007C RID: 124
		public abstract void DoExit(int theCode);

		// Token: 0x0600007D RID: 125
		public abstract void Remove3DData(MemoryImage theMemoryImage);

		// Token: 0x0600007E RID: 126
		public abstract void BeginPopup();

		// Token: 0x0600007F RID: 127
		public abstract void EndPopup();

		// Token: 0x06000080 RID: 128
		public abstract int MsgBox(string theText, string theTitle, int theFlags);

		// Token: 0x06000081 RID: 129
		public abstract void Popup(string theString);

		// Token: 0x06000082 RID: 130
		public abstract bool OpenURL(string theURL, bool shutdownOnOpen);

		// Token: 0x06000083 RID: 131
		public abstract string GetGameSEHInfo();

		// Token: 0x06000084 RID: 132
		public abstract void SEHOccured();

		// Token: 0x06000085 RID: 133
		public abstract void GetSEHWebParams(DefinesMap theDefinesMap);

		// Token: 0x06000086 RID: 134
		public abstract void DoParseCmdLine();

		// Token: 0x06000087 RID: 135
		public abstract void ParseCmdLine(string theCmdLine);

		// Token: 0x06000088 RID: 136
		public abstract void HandleCmdLineParam(string theParamName, string theParamValue);

		// Token: 0x06000089 RID: 137
		public abstract void StartLoadingThread();

		// Token: 0x0600008A RID: 138
		public abstract double GetLoadingThreadProgress();

		// Token: 0x0600008B RID: 139
		public abstract void CopyToClipboard(string theString);

		// Token: 0x0600008C RID: 140
		public abstract string GetClipboard();

		// Token: 0x0600008D RID: 141
		public abstract void SetCursor(int theCursorNum);

		// Token: 0x0600008E RID: 142
		public abstract int GetCursor();

		// Token: 0x0600008F RID: 143
		public abstract void EnableCustomCursors(bool enabled);

		// Token: 0x06000090 RID: 144
		public abstract void SetCursorImage(int theCursorNum, Image theImage);

		// Token: 0x06000091 RID: 145
		public abstract void SwitchScreenMode();

		// Token: 0x06000092 RID: 146
		public abstract void SwitchScreenMode(bool wantWindowed);

		// Token: 0x06000093 RID: 147
		public abstract void SwitchScreenMode(bool wantWindowed, bool is3d, bool force);

		// Token: 0x06000094 RID: 148
		public abstract bool KeyDown(int theKey);

		// Token: 0x06000095 RID: 149
		public abstract bool DebugKeyDown(int theKey);

		// Token: 0x06000096 RID: 150
		public abstract bool DebugKeyDownAsync(int theKey, bool ctrlDown, bool altDown);

		// Token: 0x06000097 RID: 151
		public abstract bool Is3DAccelerated();

		// Token: 0x06000098 RID: 152
		public abstract bool Is3DAccelerationSupported();

		// Token: 0x06000099 RID: 153
		public abstract bool Is3DAccelerationRecommended();

		// Token: 0x0600009A RID: 154
		public abstract void Set3DAcclerated(bool is3D, bool reinit);

		// Token: 0x0600009B RID: 155
		public abstract bool IsUIOrientationAllowed(UI_ORIENTATION theOrientation);

		// Token: 0x0600009C RID: 156
		public abstract UI_ORIENTATION GetUIOrientation();

		// Token: 0x0600009D RID: 157
		public abstract bool IsSystemUIShowing();

		// Token: 0x0600009E RID: 158
		public abstract void ShowKeyboard();

		// Token: 0x0600009F RID: 159
		public abstract void HideKeyboard();

		// Token: 0x060000A0 RID: 160
		public abstract bool CheckSignature(SexyBuffer theBuffer, string theFileName);

		// Token: 0x060000A1 RID: 161
		public abstract bool ReloadAllResources();

		// Token: 0x060000A2 RID: 162
		public abstract bool ConfigGetSubKeys(string theKeyName, List<string> theSubKeys);

		// Token: 0x060000A3 RID: 163
		public abstract bool ConfigReadString(string theValueName, ref string theString);

		// Token: 0x060000A4 RID: 164
		public abstract bool ConfigReadInteger(string theValueName, ref int theValue);

		// Token: 0x060000A5 RID: 165
		public abstract bool ConfigReadBoolean(string theValueName, ref bool theValue);

		// Token: 0x060000A6 RID: 166
		public abstract bool ConfigReadData(string theValueName, ref byte[] theValue, ref ulong theLength);

		// Token: 0x060000A7 RID: 167
		public abstract bool ConfigWriteString(string theValueName, string theString);

		// Token: 0x060000A8 RID: 168
		public abstract bool ConfigWriteInteger(string theValueName, int theValue);

		// Token: 0x060000A9 RID: 169
		public abstract bool ConfigWriteBoolean(string theValueName, bool theValue);

		// Token: 0x060000AA RID: 170
		public abstract bool ConfigWriteData(string theValueName, byte[] theValue, ulong theLength);

		// Token: 0x060000AB RID: 171
		public abstract bool ConfigEraseKey(string theKeyName);

		// Token: 0x060000AC RID: 172
		public abstract void ConfigEraseValue(string theValueName);

		// Token: 0x060000AD RID: 173
		public abstract void ReadFromConfig();

		// Token: 0x060000AE RID: 174
		public abstract void WriteToConfig();

		// Token: 0x060000AF RID: 175
		public abstract bool WriteBufferToFile(string theFileName, SexyBuffer theBuffer);

		// Token: 0x060000B0 RID: 176
		public abstract bool ReadBufferFromFile(string theFileName, SexyBuffer theBuffer, bool dontWriteToDemo);

		// Token: 0x060000B1 RID: 177
		public abstract bool WriteBytesToFile(string theFileName, object theData, ulong theDataLen);

		// Token: 0x060000B2 RID: 178
		public abstract DeviceImage GetOptimizedImage(string theFileName, bool commitBits, bool allowTriReps);

		// Token: 0x060000B3 RID: 179
		public abstract DeviceImage GetOptimizedImage(Stream stream, bool commitBits, bool allowTriReps);

		// Token: 0x060000B4 RID: 180
		public abstract object GetOptimizedRenderData(string theFileName);

		// Token: 0x060000B5 RID: 181
		public abstract DeviceImage GetOptimizedImageFromData(string theFileName, bool commitBits, bool allowTriReps, int width, int height);

		// Token: 0x060000B6 RID: 182
		public abstract bool ShouldPauseUpdates();

		// Token: 0x060000B7 RID: 183
		public abstract void Draw();

		// Token: 0x060000B8 RID: 184
		public abstract int GetAppTime();

		// Token: 0x060000B9 RID: 185
		public abstract Localization.LanguageType GetAppLanguage();
	}
}
