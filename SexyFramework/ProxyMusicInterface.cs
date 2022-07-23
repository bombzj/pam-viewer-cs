using System;
using Microsoft.Xna.Framework.Content;

namespace SexyFramework
{
	// Token: 0x020001B0 RID: 432
	public class ProxyMusicInterface : MusicInterface
	{
		// Token: 0x06000FC2 RID: 4034 RVA: 0x0004C898 File Offset: 0x0004AA98
		public ProxyMusicInterface(MusicInterface theTargetInterface, bool deleteTarget)
		{
			this.mTargetInterface = theTargetInterface;
			this.mDeleteTarget = deleteTarget;
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0004C8AE File Offset: 0x0004AAAE
		public override void Dispose()
		{
			if (this.mDeleteTarget && this.mTargetInterface != null)
			{
				this.mTargetInterface.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0004C8D1 File Offset: 0x0004AAD1
		public override bool LoadMusic(int theSongId, string theFileName, ContentManager content)
		{
			return this.mTargetInterface.LoadMusic(theSongId, theFileName, content);
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0004C8E1 File Offset: 0x0004AAE1
		public override void PlayMusic(int theSongId, int theOffset, bool noLoop)
		{
			this.PlayMusic(theSongId, theOffset, noLoop, 0L);
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0004C8EE File Offset: 0x0004AAEE
		public override void PlayMusic(int theSongId, int theOffset)
		{
			this.PlayMusic(theSongId, theOffset, false, 0L);
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0004C8FB File Offset: 0x0004AAFB
		public override void PlayMusic(int theSongId)
		{
			this.PlayMusic(theSongId, 0, false, 0L);
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0004C908 File Offset: 0x0004AB08
		public override void PlayMusic(int theSongId, int theOffset, bool noLoop, long theStartPos)
		{
			this.mTargetInterface.PlayMusic(theSongId, theOffset, noLoop, theStartPos);
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0004C91A File Offset: 0x0004AB1A
		public override void StopMusic(int theSongId)
		{
			this.mTargetInterface.StopMusic(theSongId);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0004C928 File Offset: 0x0004AB28
		public override void PauseMusic(int theSongId)
		{
			this.mTargetInterface.PauseMusic(theSongId);
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x0004C936 File Offset: 0x0004AB36
		public override void ResumeMusic(int theSongId)
		{
			this.mTargetInterface.ResumeMusic(theSongId);
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x0004C944 File Offset: 0x0004AB44
		public override void StopAllMusic()
		{
			this.mTargetInterface.StopAllMusic();
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0004C951 File Offset: 0x0004AB51
		public override void UnloadMusic(int theSongId)
		{
			this.mTargetInterface.UnloadMusic(theSongId);
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0004C95F File Offset: 0x0004AB5F
		public override void UnloadAllMusic()
		{
			this.mTargetInterface.UnloadAllMusic();
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x0004C96C File Offset: 0x0004AB6C
		public override void PauseAllMusic()
		{
			this.mTargetInterface.PauseAllMusic();
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0004C979 File Offset: 0x0004AB79
		public override void ResumeAllMusic()
		{
			this.mTargetInterface.ResumeAllMusic();
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0004C986 File Offset: 0x0004AB86
		public override void FadeIn(int theSongId, int theOffset, double theSpeed)
		{
			this.FadeIn(theSongId, theOffset, theSpeed, false);
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0004C992 File Offset: 0x0004AB92
		public override void FadeIn(int theSongId, int theOffset)
		{
			this.FadeIn(theSongId, theOffset, 0.002, false);
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0004C9A6 File Offset: 0x0004ABA6
		public override void FadeIn(int theSongId)
		{
			this.FadeIn(theSongId, -1, 0.002, false);
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x0004C9BA File Offset: 0x0004ABBA
		public override void FadeIn(int theSongId, int theOffset, double theSpeed, bool noLoop)
		{
			this.mTargetInterface.FadeIn(theSongId, theOffset, theSpeed, noLoop);
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x0004C9CC File Offset: 0x0004ABCC
		public override void FadeOut(int theSongId, bool stopSong)
		{
			this.FadeOut(theSongId, stopSong, 0.004);
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0004C9DF File Offset: 0x0004ABDF
		public override void FadeOut(int theSongId)
		{
			this.FadeOut(theSongId, true, 0.004);
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x0004C9F2 File Offset: 0x0004ABF2
		public override void FadeOut(int theSongId, bool stopSong, double theSpeed)
		{
			this.mTargetInterface.FadeOut(theSongId, stopSong, theSpeed);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0004CA02 File Offset: 0x0004AC02
		public override void FadeOutAll(bool stopSong)
		{
			this.FadeOutAll(stopSong, 0.004);
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0004CA14 File Offset: 0x0004AC14
		public override void FadeOutAll()
		{
			this.FadeOutAll(true, 0.004);
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0004CA26 File Offset: 0x0004AC26
		public override void FadeOutAll(bool stopSong, double theSpeed)
		{
			this.mTargetInterface.FadeOutAll(stopSong, theSpeed);
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0004CA35 File Offset: 0x0004AC35
		public override void SetSongVolume(int theSongId, double theVolume)
		{
			this.mTargetInterface.SetSongVolume(theSongId, theVolume);
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0004CA44 File Offset: 0x0004AC44
		public override void SetSongMaxVolume(int theSongId, double theMaxVolume)
		{
			this.mTargetInterface.SetSongMaxVolume(theSongId, theMaxVolume);
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0004CA53 File Offset: 0x0004AC53
		public override bool IsPlaying(int theSongId)
		{
			return this.mTargetInterface.IsPlaying(theSongId);
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0004CA61 File Offset: 0x0004AC61
		public override void SetVolume(double theVolume)
		{
			this.mTargetInterface.SetVolume(theVolume);
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0004CA6F File Offset: 0x0004AC6F
		public override void SetMusicAmplify(int theSongId, double theAmp)
		{
			this.mTargetInterface.SetMusicAmplify(theSongId, theAmp);
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0004CA7E File Offset: 0x0004AC7E
		public override void Update()
		{
			this.mTargetInterface.Update();
		}

		// Token: 0x04000CD1 RID: 3281
		private MusicInterface mTargetInterface;

		// Token: 0x04000CD2 RID: 3282
		private bool mDeleteTarget;
	}
}
