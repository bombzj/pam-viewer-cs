using System;

namespace ZumasRevenge
{
	// Token: 0x0200012C RID: 300
	public class AdvModeVars
	{
		// Token: 0x06000F3D RID: 3901 RVA: 0x0009D578 File Offset: 0x0009B778
		public AdvModeVars()
		{
			for (int i = 0; i < 6; i++)
			{
				this.mCheckpointScores[i] = new CheckpointScores();
			}
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0009D5D8 File Offset: 0x0009B7D8
		public void CopyFrom(AdvModeVars rhs)
		{
			this.mHighestZoneBeat = rhs.mHighestZoneBeat;
			this.mHighestLevelBeat = rhs.mHighestLevelBeat;
			for (int i = 0; i < 6; i++)
			{
				this.mFirstTimeInZone[i] = rhs.mFirstTimeInZone[i];
			}
			this.mNumDeathsCurLevel = rhs.mNumDeathsCurLevel;
			this.mNumZumasCurLevel = rhs.mNumZumasCurLevel;
			this.mPerfectZone = rhs.mPerfectZone;
			for (int j = 0; j < 6; j++)
			{
				this.mNumTimesZoneBeat[j] = rhs.mNumTimesZoneBeat[j];
			}
			this.mDDSTier = rhs.mDDSTier;
			this.mRestartDDSTier = rhs.mRestartDDSTier;
			this.mCurrentAdvScore = rhs.mCurrentAdvScore;
			this.mCurrentAdvLevel = rhs.mCurrentAdvLevel;
			this.mCurrentAdvZone = rhs.mCurrentAdvZone;
			this.mCurrentAdvLives = rhs.mCurrentAdvLives;
			for (int k = 0; k < 60; k++)
			{
				this.mBestLevelTime[k] = rhs.mBestLevelTime[k];
			}
			for (int l = 0; l < 6; l++)
			{
				this.mCheckpointScores[l].CopyFrom(rhs.mCheckpointScores[l]);
			}
		}

		// Token: 0x04000EED RID: 3821
		public int mHighestZoneBeat;

		// Token: 0x04000EEE RID: 3822
		public int mHighestLevelBeat;

		// Token: 0x04000EEF RID: 3823
		public bool[] mFirstTimeInZone = new bool[6];

		// Token: 0x04000EF0 RID: 3824
		public int mNumDeathsCurLevel;

		// Token: 0x04000EF1 RID: 3825
		public int mNumZumasCurLevel;

		// Token: 0x04000EF2 RID: 3826
		public bool mPerfectZone;

		// Token: 0x04000EF3 RID: 3827
		public int[] mNumTimesZoneBeat = new int[6];

		// Token: 0x04000EF4 RID: 3828
		public int mDDSTier;

		// Token: 0x04000EF5 RID: 3829
		public int mRestartDDSTier;

		// Token: 0x04000EF6 RID: 3830
		public int mCurrentAdvScore;

		// Token: 0x04000EF7 RID: 3831
		public int mCurrentAdvLevel;

		// Token: 0x04000EF8 RID: 3832
		public int mCurrentAdvZone;

		// Token: 0x04000EF9 RID: 3833
		public int mCurrentAdvLives;

		// Token: 0x04000EFA RID: 3834
		public int[] mBestLevelTime = new int[60];

		// Token: 0x04000EFB RID: 3835
		public CheckpointScores[] mCheckpointScores = new CheckpointScores[6];
	}
}
