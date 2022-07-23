using System;
using SexyFramework;
using SexyFramework.Drivers.App;

namespace ZumasRevenge
{
	// Token: 0x02000124 RID: 292
	public class Music : IDisposable
	{
		// Token: 0x06000F05 RID: 3845 RVA: 0x0009BA38 File Offset: 0x00099C38
		public Music(MusicInterface inMusicInterface)
		{
			this.mMusicInterface = inMusicInterface;
			this.mEnabled = false;
			this.mCurrentSong = Song.DefaultSong;
			this.mNextSong = Song.DefaultSong;
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x0009BA85 File Offset: 0x00099C85
		public void RegisterCallBack()
		{
			this.mMusicInterface.RegisterCallback(new SongChangedEventHandle(this.OnSongChanged));
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0009BA9E File Offset: 0x00099C9E
		public void OnSongChanged(object sender, SongChangedEventArgs args)
		{
			this.mCurrentSong = new Song(args.songID, args.loop, 1f);
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0009BABC File Offset: 0x00099CBC
		public void Dispose()
		{
			this.mMusicInterface.UnloadAllMusic();
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0009BAC9 File Offset: 0x00099CC9
		public void Enable(bool inEnable)
		{
			if (this.mEnabled && !inEnable)
			{
				this.mNextSong = this.mCurrentSong;
				this.mCurrentSong = Song.DefaultSong;
				this.mMusicInterface.StopAllMusic();
			}
			this.mEnabled = inEnable;
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x0009BAFF File Offset: 0x00099CFF
		public void LoadMusic(int inSongID, string inFileName)
		{
			this.mMusicInterface.LoadMusic(inSongID, inFileName, WP7AppDriver.sWP7AppDriverInstance.mContentManager);
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0009BB19 File Offset: 0x00099D19
		public void PlaySong(int inSongID, float inFadeSpeed, bool inLoop)
		{
			this.PlaySong(inSongID, inFadeSpeed, inLoop, false);
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0009BB28 File Offset: 0x00099D28
		public void PlaySongNoDelay(int inSongID, bool inLoop)
		{
			if (this.IsPlaying(inSongID, false))
			{
				return;
			}
			this.mCurrentSong = new Song(inSongID, inLoop, 1f);
			this.mMusicInterface.PlayMusic(this.mCurrentSong.mID, 0, !this.mCurrentSong.mLoop, 0L);
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0009BB7C File Offset: 0x00099D7C
		public void PlaySong(int inSongID, float inFadeSpeed, bool inLoop, bool inForce)
		{
			if (this.IsPlaying(inSongID, inForce))
			{
				return;
			}
			if (this.DelaySong(inSongID, inFadeSpeed, inLoop))
			{
				return;
			}
			this.mCurrentSong = new Song(inSongID, inLoop, 1f);
			this.mMusicInterface.PlayMusic(this.mCurrentSong.mID, 0, !this.mCurrentSong.mLoop, 0L);
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0009BBDA File Offset: 0x00099DDA
		public void FadeOut()
		{
			this.mCurrentSong = Song.DefaultSong;
			this.mNextSong = this.mCurrentSong;
			this.mMusicInterface.FadeOutAll();
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x0009BBFE File Offset: 0x00099DFE
		public void StopAll()
		{
			this.mMusicInterface.StopAllMusic();
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x0009BC0C File Offset: 0x00099E0C
		public void Update()
		{
			if (!this.mEnabled || this.mMusicInterface.IsPlaying(this.mCurrentSong.mID))
			{
				return;
			}
			if (this.mNextSong.mID != -1)
			{
				this.mMusicInterface.FadeIn(this.mNextSong.mID, 0, (double)this.mNextSong.mFadeSpeed, !this.mNextSong.mLoop);
			}
			this.mCurrentSong = this.mNextSong;
			this.mNextSong = Song.DefaultSong;
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x0009BC90 File Offset: 0x00099E90
		private bool IsPlaying(int inSongID, bool inForceStop)
		{
			return this.mMusicInterface.IsPlaying(inSongID) && !inForceStop;
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x0009BCA8 File Offset: 0x00099EA8
		private bool DelaySong(int inSongID, float inFadeSpeed, bool inLoop)
		{
			if (this.mEnabled && !this.mMusicInterface.IsPlaying(this.mCurrentSong.mID))
			{
				return false;
			}
			this.mNextSong = new Song(inSongID, inLoop, inFadeSpeed);
			if (this.mEnabled)
			{
				this.mMusicInterface.FadeOut(this.mCurrentSong.mID, true, (double)inFadeSpeed);
			}
			return true;
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0009BD07 File Offset: 0x00099F07
		public bool IsUserMusicPlaying()
		{
			return this.mMusicInterface.isPlayingUserMusic();
		}

		// Token: 0x04000EAA RID: 3754
		private MusicInterface mMusicInterface;

		// Token: 0x04000EAB RID: 3755
		private bool mEnabled;

		// Token: 0x04000EAC RID: 3756
		private Song mCurrentSong = Song.DefaultSong;

		// Token: 0x04000EAD RID: 3757
		private Song mNextSong = Song.DefaultSong;
	}
}
