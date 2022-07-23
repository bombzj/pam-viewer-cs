using System;
using Microsoft.Xna.Framework.Content;

namespace SexyFramework
{
	// Token: 0x020001AA RID: 426
	public class MusicInterface
	{
		// Token: 0x06000F74 RID: 3956 RVA: 0x0004BE40 File Offset: 0x0004A040
		public virtual void Dispose()
		{
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0004BE42 File Offset: 0x0004A042
		public virtual bool LoadMusic(int theSongId, string theFileName, ContentManager content)
		{
			return false;
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0004BE45 File Offset: 0x0004A045
		public virtual void PlayMusic(int theSongId, int theOffset, bool noLoop)
		{
			this.PlayMusic(theSongId, theOffset, noLoop, 0L);
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0004BE52 File Offset: 0x0004A052
		public virtual void PlayMusic(int theSongId, int theOffset)
		{
			this.PlayMusic(theSongId, theOffset, false, 0L);
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0004BE5F File Offset: 0x0004A05F
		public virtual void PlayMusic(int theSongId)
		{
			this.PlayMusic(theSongId, 0, false, 0L);
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0004BE6C File Offset: 0x0004A06C
		public virtual void PlayMusic(int theSongId, int theOffset, bool inLoop, long theStartPos)
		{
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0004BE6E File Offset: 0x0004A06E
		public virtual void StopMusic(int theSongId)
		{
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0004BE70 File Offset: 0x0004A070
		public virtual void PauseMusic(int theSongId)
		{
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0004BE72 File Offset: 0x0004A072
		public virtual void ResumeMusic(int theSongId)
		{
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0004BE74 File Offset: 0x0004A074
		public virtual void StopAllMusic()
		{
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0004BE76 File Offset: 0x0004A076
		public virtual void UnloadMusic(int theSongId)
		{
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0004BE78 File Offset: 0x0004A078
		public virtual void UnloadAllMusic()
		{
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0004BE7A File Offset: 0x0004A07A
		public virtual void PauseAllMusic()
		{
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x0004BE7C File Offset: 0x0004A07C
		public virtual void ResumeAllMusic()
		{
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x0004BE7E File Offset: 0x0004A07E
		public virtual void OnMediaPlayerStateChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0004BE80 File Offset: 0x0004A080
		public virtual void FadeIn(int theSongId, int theOffset, double theSpeed)
		{
			this.FadeIn(theSongId, theOffset, theSpeed, false);
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0004BE8C File Offset: 0x0004A08C
		public virtual void FadeIn(int theSongId, int theOffset)
		{
			this.FadeIn(theSongId, theOffset, 0.002, false);
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0004BEA0 File Offset: 0x0004A0A0
		public virtual void FadeIn(int theSongId)
		{
			this.FadeIn(theSongId, -1, 0.002, false);
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x0004BEB4 File Offset: 0x0004A0B4
		public virtual void FadeIn(int theSongId, int theOffset, double theSpeed, bool noLoop)
		{
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0004BEB6 File Offset: 0x0004A0B6
		public virtual void FadeOut(int theSongId, bool stopSong)
		{
			this.FadeOut(theSongId, stopSong, 0.004);
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0004BEC9 File Offset: 0x0004A0C9
		public virtual void FadeOut(int theSongId)
		{
			this.FadeOut(theSongId, true, 0.004);
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x0004BEDC File Offset: 0x0004A0DC
		public virtual void FadeOut(int theSongId, bool stopSong, double theSpeed)
		{
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0004BEDE File Offset: 0x0004A0DE
		public virtual void FadeOutAll(bool stopSong)
		{
			this.FadeOutAll(stopSong, 0.004);
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x0004BEF0 File Offset: 0x0004A0F0
		public virtual void FadeOutAll()
		{
			this.FadeOutAll(true, 0.004);
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0004BF02 File Offset: 0x0004A102
		public virtual void FadeOutAll(bool stopSong, double theSpeed)
		{
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x0004BF04 File Offset: 0x0004A104
		public virtual void SetSongVolume(int theSongId, double theVolume)
		{
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0004BF06 File Offset: 0x0004A106
		public virtual void SetSongMaxVolume(int theSongId, double theMaxVolume)
		{
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0004BF08 File Offset: 0x0004A108
		public virtual bool IsPlaying(int theSongId)
		{
			return false;
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x0004BF0B File Offset: 0x0004A10B
		public virtual void SetVolume(double theVolume)
		{
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0004BF0D File Offset: 0x0004A10D
		public virtual void SetMusicAmplify(int theSongId, double theAmp)
		{
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0004BF0F File Offset: 0x0004A10F
		public virtual void Update()
		{
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0004BF11 File Offset: 0x0004A111
		public virtual void OnDeactived()
		{
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0004BF13 File Offset: 0x0004A113
		public virtual void OnActived()
		{
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0004BF15 File Offset: 0x0004A115
		public virtual void OnServiceDeactived()
		{
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0004BF17 File Offset: 0x0004A117
		public virtual void OnServiceActived()
		{
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0004BF19 File Offset: 0x0004A119
		public virtual void RegisterCallback(SongChangedEventHandle handle)
		{
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0004BF1B File Offset: 0x0004A11B
		public virtual bool isPlayingUserMusic()
		{
			return false;
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0004BF1E File Offset: 0x0004A11E
		public virtual void stopUserMusic()
		{
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0004BF20 File Offset: 0x0004A120
		public virtual int GetMusicTempo(int theSongId)
		{
			return -1;
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0004BF23 File Offset: 0x0004A123
		public virtual void SetMusicTempo(int theSongId, int theTempo)
		{
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x0004BF25 File Offset: 0x0004A125
		public virtual int GetMusicOrder(int theSongId)
		{
			return -1;
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x0004BF28 File Offset: 0x0004A128
		public virtual void SetMusicOrder(int theSongId, int theOrder)
		{
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0004BF2A File Offset: 0x0004A12A
		public virtual int GetMusicRow(int theSongId)
		{
			return -1;
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0004BF2D File Offset: 0x0004A12D
		public virtual void SetMusicRow(int theSongId, int theOrder)
		{
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0004BF2F File Offset: 0x0004A12F
		public virtual int GetMusicChannelVolume(int theSongId, int theChannelId)
		{
			return -1;
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0004BF32 File Offset: 0x0004A132
		public virtual void SetMusicChannelVolume(int theSongId, int theChannelId, int theVolume)
		{
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0004BF34 File Offset: 0x0004A134
		public virtual void OnMusicPosCallback(int theMusicId, int theOrder, int theRow)
		{
		}

		// Token: 0x04000CA9 RID: 3241
		public const float MUSIC_FADE_SPEED = 0.005f;

		// Token: 0x04000CAA RID: 3242
		public const int MUSIC_TRACK_1 = 1;

		// Token: 0x04000CAB RID: 3243
		public const int MUSIC_TRACK_2 = 2;

		// Token: 0x04000CAC RID: 3244
		public MusicInterface.EMusicInterfaceState mCurState = MusicInterface.EMusicInterfaceState.State_None;

		// Token: 0x04000CAD RID: 3245
		public MusicInterface.EGameEvent mGameEvent = MusicInterface.EGameEvent.State_No;

		// Token: 0x04000CAE RID: 3246
		public MusicInterface.EServiceEvent mServiceEvent = MusicInterface.EServiceEvent.State_ServiceNo;

		// Token: 0x04000CAF RID: 3247
		protected float m_MusicVolume = 1f;

		// Token: 0x04000CB0 RID: 3248
		public bool m_isUserMusicOn;

		// Token: 0x04000CB1 RID: 3249
		public bool m_isInterruptByOtherSound;

		// Token: 0x020001AB RID: 427
		public enum EMusicInterfaceState
		{
			// Token: 0x04000CB3 RID: 3251
			State_PlayingUserMusic,
			// Token: 0x04000CB4 RID: 3252
			State_UserMusicStoppedInGame,
			// Token: 0x04000CB5 RID: 3253
			State_UserMusicStopedOutGame,
			// Token: 0x04000CB6 RID: 3254
			State_GameMusicStopedInGame,
			// Token: 0x04000CB7 RID: 3255
			State_PlayingGameMusic,
			// Token: 0x04000CB8 RID: 3256
			State_None
		}

		// Token: 0x020001AC RID: 428
		public enum EGameEvent
		{
			// Token: 0x04000CBA RID: 3258
			State_OnActived,
			// Token: 0x04000CBB RID: 3259
			State_OnDeactive,
			// Token: 0x04000CBC RID: 3260
			State_No
		}

		// Token: 0x020001AD RID: 429
		public enum EServiceEvent
		{
			// Token: 0x04000CBE RID: 3262
			State_OnServiceActived,
			// Token: 0x04000CBF RID: 3263
			State_OnServiceDeactive,
			// Token: 0x04000CC0 RID: 3264
			State_ServiceNo
		}
	}
}
