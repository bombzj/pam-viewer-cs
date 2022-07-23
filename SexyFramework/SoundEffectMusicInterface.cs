using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace SexyFramework
{
	// Token: 0x020001AF RID: 431
	public class SoundEffectMusicInterface : MusicInterface
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000FAA RID: 4010 RVA: 0x0004BF9C File Offset: 0x0004A19C
		// (remove) Token: 0x06000FAB RID: 4011 RVA: 0x0004BFD4 File Offset: 0x0004A1D4
		public event SongChangedEventHandle mHandle;

		// Token: 0x06000FAC RID: 4012 RVA: 0x0004C009 File Offset: 0x0004A209
		public void SongChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x0004C00B File Offset: 0x0004A20B
		public SoundEffectMusicInterface()
		{
			this.m_isUserMusicOn = !MediaPlayer.GameHasControl;
			MediaPlayer.MediaStateChanged += new EventHandler<EventArgs>(this.OnMediaPlayerStateChanged);
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x0004C045 File Offset: 0x0004A245
		public override bool isPlayingUserMusic()
		{
			return !MediaPlayer.GameHasControl;
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x0004C050 File Offset: 0x0004A250
		public override void stopUserMusic()
		{
			this.m_PauseByFunction = true;
			MediaPlayer.Pause();
			if (this.m_CurrSong != null)
			{
				this.m_CurrSong.play();
				MediaPlayer.IsRepeating = !this.m_CurrSong.mStopOnFade;
				MediaPlayer.Volume = Common.CaculatePowValume(this.m_MusicVolume);
			}
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x0004C0A0 File Offset: 0x0004A2A0
		public override bool LoadMusic(int theSongId, string theFileName, ContentManager content)
		{
			Song s = null;
			try
			{
				s = content.Load<Song>(theFileName);
			}
			catch (Exception)
			{
				s = null;
				return false;
			}
			if (this.m_SoundDict.ContainsKey(theSongId))
			{
				this.m_SoundDict[theSongId].load(s);
			}
			else
			{
				this.m_SoundDict.Add(theSongId, new SoundEffectWrapper(s));
			}
			return true;
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0004C108 File Offset: 0x0004A308
		public override void UnloadAllMusic()
		{
			this.m_SoundDict.Clear();
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0004C118 File Offset: 0x0004A318
		public override void PlayMusic(int theSongId, int theOffset, bool noLoop, long theStartPos)
		{
			this.StopAllMusic();
			if (this.m_SoundDict.ContainsKey(theSongId))
			{
				this.m_CurrSongID = theSongId;
				this.m_CurrSong = this.m_SoundDict[theSongId];
				this.m_SoundDict[theSongId].mStopOnFade = noLoop;
				this.m_SoundDict[theSongId].mVolume = (this.m_SoundDict[theSongId].mVolumeCap = (double)this.m_MusicVolume);
				this.m_SoundDict[theSongId].mVolumeAdd = 0.0;
				if (MediaPlayer.GameHasControl)
				{
					this.m_SoundDict[theSongId].play();
					MediaPlayer.IsRepeating = !noLoop;
					MediaPlayer.Volume = Common.CaculatePowValume(this.m_MusicVolume);
				}
			}
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0004C1DE File Offset: 0x0004A3DE
		public override void SetVolume(double theVolume)
		{
			this.m_MusicVolume = (float)theVolume;
			if (this.m_isUserMusicOn)
			{
				return;
			}
			MediaPlayer.Volume = Common.CaculatePowValume(this.m_MusicVolume);
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0004C204 File Offset: 0x0004A404
		public override void StopAllMusic()
		{
			if (this.m_isUserMusicOn)
			{
				return;
			}
			MediaPlayer.Stop();
			foreach (SoundEffectWrapper soundEffectWrapper in this.m_SoundDict.Values)
			{
				soundEffectWrapper.mVolumeAdd = 0.0;
				soundEffectWrapper.mVolume = 0.0;
				soundEffectWrapper.m_isPlaying = false;
			}
			this.m_CurrSong = null;
			this.m_CurrSongID = -1;
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0004C298 File Offset: 0x0004A498
		public override void PauseAllMusic()
		{
			if (this.m_isUserMusicOn || MediaPlayer.State == MediaState.Paused)
			{
				return;
			}
			if (this.m_CurrSong != null)
			{
				this.m_PauseByFunction = true;
				MediaPlayer.Pause();
			}
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0004C2C0 File Offset: 0x0004A4C0
		public override void ResumeAllMusic()
		{
			if (this.m_isUserMusicOn)
			{
				return;
			}
			if (this.m_PauseByFunction && this.m_CurrSong != null)
			{
				this.m_PauseByFunction = false;
				MediaQueue queue = MediaPlayer.Queue;
				Song activeSong = queue.ActiveSong;
				if (this.m_CurrSong.m_Song.Name != activeSong.Name)
				{
					this.m_CurrSong.play();
					MediaPlayer.IsRepeating = !this.m_CurrSong.mStopOnFade;
					MediaPlayer.Volume = Common.CaculatePowValume(this.m_MusicVolume);
					SongChangedEventArgs songChangedEventArgs = new SongChangedEventArgs();
					songChangedEventArgs.songID = this.m_CurrSongID;
					songChangedEventArgs.loop = !this.m_CurrSong.mStopOnFade;
					if (this.mHandle != null)
					{
						this.mHandle(this, songChangedEventArgs);
						return;
					}
				}
				else
				{
					MediaPlayer.Resume();
				}
			}
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0004C38C File Offset: 0x0004A58C
		public override void FadeIn(int theSongId, int theOffset, double theSpeed, bool noLoop)
		{
			this.PlayMusic(theSongId, theOffset, noLoop);
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0004C398 File Offset: 0x0004A598
		public override void FadeOut(int theSongId, bool stopSong, double theSpeed)
		{
			this.StopAllMusic();
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0004C3A0 File Offset: 0x0004A5A0
		public override void FadeOutAll(bool stopSong, double theSpeed)
		{
			if (this.m_isUserMusicOn)
			{
				return;
			}
			this.StopAllMusic();
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0004C3B1 File Offset: 0x0004A5B1
		public override bool IsPlaying(int theSongId)
		{
			return !this.m_isUserMusicOn && this.m_SoundDict.ContainsKey(theSongId) && this.m_SoundDict[theSongId].isPlaying();
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0004C3E0 File Offset: 0x0004A5E0
		public override void Update()
		{
			if (this.mGameEvent == MusicInterface.EGameEvent.State_OnDeactive || this.mServiceEvent == MusicInterface.EServiceEvent.State_OnServiceDeactive)
			{
				return;
			}
			if ((this.mCurState == MusicInterface.EMusicInterfaceState.State_GameMusicStopedInGame || this.mCurState == MusicInterface.EMusicInterfaceState.State_UserMusicStoppedInGame) && this.m_onDeactive && !this.m_onServiceDeactive && this.mUpdateCount > 1)
			{
				this.mCurState = MusicInterface.EMusicInterfaceState.State_None;
				this.m_onDeactive = false;
				MediaPlayer.Resume();
				this.m_isUserMusicOn = !MediaPlayer.GameHasControl;
			}
			else if (this.mCurState == MusicInterface.EMusicInterfaceState.State_UserMusicStoppedInGame && !this.m_onDeactive && !this.m_onServiceDeactive && this.mUpdateCount > 100)
			{
				this.mCurState = MusicInterface.EMusicInterfaceState.State_None;
				this.mUpdateCount = 0;
				if (this.m_CurrSong == null || this.m_CurrSong.mStopOnFade)
				{
					return;
				}
				MediaQueue queue = MediaPlayer.Queue;
				Song activeSong = queue.ActiveSong;
				if (this.m_CurrSong.m_Song.Name != activeSong.Name)
				{
					this.m_CurrSong.play();
					MediaPlayer.IsRepeating = !this.m_CurrSong.mStopOnFade;
					MediaPlayer.Volume = Common.CaculatePowValume(this.m_MusicVolume);
					SongChangedEventArgs songChangedEventArgs = new SongChangedEventArgs();
					songChangedEventArgs.songID = this.m_CurrSongID;
					songChangedEventArgs.loop = !this.m_CurrSong.mStopOnFade;
					if (this.mHandle != null)
					{
						this.mHandle(this, songChangedEventArgs);
					}
				}
				else
				{
					MediaPlayer.Resume();
				}
				this.m_isUserMusicOn = !MediaPlayer.GameHasControl;
			}
			else if (this.mCurState == MusicInterface.EMusicInterfaceState.State_GameMusicStopedInGame && !this.m_onDeactive && !this.m_onServiceDeactive && this.mUpdateCount > 1)
			{
				this.mCurState = MusicInterface.EMusicInterfaceState.State_None;
				this.mUpdateCount = 0;
				if (this.m_CurrSong == null || this.m_CurrSong.mStopOnFade)
				{
					return;
				}
				MediaQueue queue2 = MediaPlayer.Queue;
				Song activeSong2 = queue2.ActiveSong;
				if (this.m_CurrSong.m_Song.Name != activeSong2.Name)
				{
					this.m_CurrSong.play();
					MediaPlayer.IsRepeating = !this.m_CurrSong.mStopOnFade;
					MediaPlayer.Volume = Common.CaculatePowValume(this.m_MusicVolume);
					SongChangedEventArgs songChangedEventArgs2 = new SongChangedEventArgs();
					songChangedEventArgs2.songID = this.m_CurrSongID;
					songChangedEventArgs2.loop = !this.m_CurrSong.mStopOnFade;
					if (this.mHandle != null)
					{
						this.mHandle(this, songChangedEventArgs2);
					}
				}
				else
				{
					MediaPlayer.Resume();
				}
				this.m_isUserMusicOn = !MediaPlayer.GameHasControl;
			}
			else if (this.mCurState == MusicInterface.EMusicInterfaceState.State_UserMusicStopedOutGame && this.m_isUserMusicOn && MediaPlayer.State != MediaState.Playing && this.m_onDeactive && this.m_onServiceDeactive && this.mUpdateCount > 1)
			{
				this.m_onDeactive = false;
				this.m_onServiceDeactive = false;
				this.mCurState = MusicInterface.EMusicInterfaceState.State_None;
				if (this.m_CurrSong == null || this.m_CurrSong.mStopOnFade)
				{
					return;
				}
				MediaQueue queue3 = MediaPlayer.Queue;
				Song activeSong3 = queue3.ActiveSong;
				if (this.m_CurrSong.m_Song.Name != activeSong3.Name)
				{
					this.m_CurrSong.play();
					MediaPlayer.IsRepeating = !this.m_CurrSong.mStopOnFade;
					MediaPlayer.Volume = Common.CaculatePowValume(this.m_MusicVolume);
					SongChangedEventArgs songChangedEventArgs3 = new SongChangedEventArgs();
					songChangedEventArgs3.songID = this.m_CurrSongID;
					songChangedEventArgs3.loop = !this.m_CurrSong.mStopOnFade;
					if (this.mHandle != null)
					{
						this.mHandle(this, songChangedEventArgs3);
					}
				}
				else
				{
					MediaPlayer.Resume();
				}
				this.m_isUserMusicOn = !MediaPlayer.GameHasControl;
			}
			else if (this.mCurState == MusicInterface.EMusicInterfaceState.State_None)
			{
				this.m_onServiceDeactive = false;
				this.m_onDeactive = false;
				this.mUpdateCount = 0;
			}
			else
			{
				this.mUpdateCount++;
			}
			bool isUserMusicOn = this.m_isUserMusicOn;
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0004C7AC File Offset: 0x0004A9AC
		public override void OnDeactived()
		{
			this.m_onDeactive = true;
			this.mGameEvent = MusicInterface.EGameEvent.State_OnDeactive;
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0004C7BC File Offset: 0x0004A9BC
		public override void OnActived()
		{
			if (!MediaPlayer.GameHasControl)
			{
				this.m_isUserMusicOn = true;
				this.mCurState = MusicInterface.EMusicInterfaceState.State_None;
				this.m_onDeactive = false;
			}
			this.mActiveCount = 1;
			this.mGameEvent = MusicInterface.EGameEvent.State_OnActived;
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0004C7E8 File Offset: 0x0004A9E8
		public override void OnServiceDeactived()
		{
			this.m_onServiceDeactive = true;
			this.m_onServiceActive = false;
			this.mServiceEvent = MusicInterface.EServiceEvent.State_OnServiceDeactive;
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0004C7FF File Offset: 0x0004A9FF
		public override void OnServiceActived()
		{
			this.m_onServiceActive = true;
			this.mServiceEvent = MusicInterface.EServiceEvent.State_OnServiceActived;
			if (this.mActiveCount == 1 && this.m_isUserMusicOn && MediaPlayer.State != MediaState.Playing)
			{
				this.mCurState = MusicInterface.EMusicInterfaceState.State_UserMusicStopedOutGame;
			}
			this.mActiveCount = 0;
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0004C836 File Offset: 0x0004AA36
		public override void RegisterCallback(SongChangedEventHandle handle)
		{
			this.mHandle += handle;
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0004C840 File Offset: 0x0004AA40
		public override void OnMediaPlayerStateChanged(object sender, EventArgs e)
		{
			MediaState state = MediaPlayer.State;
			if (this.m_isUserMusicOn && state == MediaState.Paused)
			{
				this.mCurState = MusicInterface.EMusicInterfaceState.State_UserMusicStoppedInGame;
				this.mUpdateCount = 0;
			}
			else if (state == MediaState.Paused)
			{
				this.mCurState = MusicInterface.EMusicInterfaceState.State_GameMusicStopedInGame;
				this.mUpdateCount = 0;
			}
			else
			{
				this.mCurState = MusicInterface.EMusicInterfaceState.State_None;
			}
			this.m_isUserMusicOn = !MediaPlayer.GameHasControl;
		}

		// Token: 0x04000CC7 RID: 3271
		protected Dictionary<int, SoundEffectWrapper> m_SoundDict = new Dictionary<int, SoundEffectWrapper>();

		// Token: 0x04000CC9 RID: 3273
		private SoundEffectWrapper m_CurrSong;

		// Token: 0x04000CCA RID: 3274
		private int m_CurrSongID = -1;

		// Token: 0x04000CCB RID: 3275
		private bool m_PauseByFunction;

		// Token: 0x04000CCC RID: 3276
		protected bool m_onDeactive;

		// Token: 0x04000CCD RID: 3277
		protected bool m_onServiceDeactive;

		// Token: 0x04000CCE RID: 3278
		protected bool m_onServiceActive;

		// Token: 0x04000CCF RID: 3279
		public int mUpdateCount;

		// Token: 0x04000CD0 RID: 3280
		public int mActiveCount;
	}
}
