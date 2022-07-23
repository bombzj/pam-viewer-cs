using System;
using System.Collections.Generic;
using System.Linq;
using JeffLib;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using ZumasRevenge.Achievement;

namespace ZumasRevenge
{
	// Token: 0x02000093 RID: 147
	public class CurveMgr
	{
		// Token: 0x0600094C RID: 2380 RVA: 0x00053C58 File Offset: 0x00051E58
		protected bool CanSpawnPowerUp(PowerType ptype)
		{
			if (this.mBoard.GauntletMode() && ptype == PowerType.PowerType_MoveBackwards && this.GetFarthestBallPercent() < 25)
			{
				return false;
			}
			if (ptype == PowerType.PowerType_ProximityBomb && this.mLevel.mBoss != null && this.mLevel.mBoss.mBombFreqMin > 0)
			{
				return false;
			}
			int num = ((Board.gDebugCurveData == null) ? this.mCurveDesc.mVals.mMaxNumPowerUps[(int)ptype] : Board.gDebugCurveData.mVals.mMaxNumPowerUps[(int)ptype]);
			if (ptype == PowerType.PowerType_ColorNuke && this.mApp.GetLevelMgr().mMaxZumaPctForColorNuke > 0f && this.mLevel.GetBarPercent() > this.mApp.GetLevelMgr().mMaxZumaPctForColorNuke)
			{
				return false;
			}
			if (ptype == PowerType.PowerType_ColorNuke && !this.mApp.GetLevelMgr().mAllowColorNukeAfterZuma && this.mBoard.HasAchievedZuma())
			{
				return false;
			}
			if ((ptype == PowerType.PowerType_ColorNuke || ptype == PowerType.PowerType_Cannon || ptype == PowerType.PowerType_Laser) && this.mBoard.HasAchievedZuma())
			{
				return false;
			}
			if ((ptype == PowerType.PowerType_ColorNuke && (this.mLevel.HasPowerup(PowerType.PowerType_Cannon) || this.mLevel.mFrog.CannonMode())) || (ptype == PowerType.PowerType_Cannon && (this.mLevel.HasPowerup(PowerType.PowerType_ColorNuke) || this.mLevel.mFrog.LightningMode())))
			{
				return false;
			}
			if (ptype == PowerType.PowerType_Accuracy)
			{
				return false;
			}
			if (!this.mApp.GetLevelMgr().mCapAffectsPowerupsSpawned)
			{
				return this.mNumPowerupsActivated[(int)ptype] < num;
			}
			return this.mNumPowerUpsThisLevel[(int)ptype] < num;
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00053DC8 File Offset: 0x00051FC8
		protected int GetNumPendingMatches(int t)
		{
			int num = 0;
			for (int i = Enumerable.Count<Ball>(this.mPendingBalls) - 1; i >= 0; i--)
			{
				Ball ball = this.mPendingBalls[i];
				if (ball.GetColorType() != t)
				{
					break;
				}
				num++;
			}
			if (Enumerable.Count<Ball>(this.mPendingBalls) > 0)
			{
				return num;
			}
			foreach (Ball ball2 in this.mBallList)
			{
				if (ball2.GetColorType() != t)
				{
					break;
				}
				num++;
			}
			return num;
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x00053E68 File Offset: 0x00052068
		protected int GetNumPendingMatches()
		{
			if (Enumerable.Count<Ball>(this.mPendingBalls) == 0 && Enumerable.Count<Ball>(this.mBallList) == 0)
			{
				return 0;
			}
			int t = ((Enumerable.Count<Ball>(this.mPendingBalls) > 0) ? Enumerable.Last<Ball>(this.mPendingBalls).GetColorType() : Enumerable.First<Ball>(this.mBallList).GetColorType());
			return this.GetNumPendingMatches(t);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00053ECC File Offset: 0x000520CC
		protected int GetNumPendingSingles(int theNumGroups)
		{
			int num = 0;
			int num2 = -1;
			int result = 0;
			int num3 = 0;
			int num4 = Enumerable.Count<Ball>(this.mPendingBalls) - 1;
			while (num4 >= 0 && num <= theNumGroups)
			{
				CurveMgr.GetNumPendingSinglesHelper(this.mPendingBalls[num4].GetColorType(), ref num, ref num2, ref result, ref num3);
				num4--;
			}
			int num5 = 0;
			while (num5 < Enumerable.Count<Ball>(this.mBallList) && num <= theNumGroups)
			{
				CurveMgr.GetNumPendingSinglesHelper(this.mBallList[num5].GetColorType(), ref num, ref num2, ref result, ref num3);
				num5++;
			}
			return result;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00053F60 File Offset: 0x00052160
		public void Reset()
		{
			this.mCurveDesc.mVals.mAccelerationRate = this.mCurveDesc.mVals.mOrgAccelerationRate;
			this.mHasReachedCruisingSpeed = false;
			this.mNumBallsCreated = 0;
			this.mNeedsSpeedup = false;
			this.mOverrideSpeed = -1f;
			this.mProxBombCounter = -1;
			this.mHasReachedRolloutPoint = false;
			this.mCanCheckForSpeedup = false;
			this.mLastPowerupTime = 0;
			this.mLastPathHiliteWP = 0;
			this.mLastPathHilitePitch = ((this.mCurveNum == 1) ? (-20) : 0);
			this.mInitialPathHilite = true;
			this.mSkullHilite = 0f;
			this.mDoingClearCurveRollout = false;
			this.mNumMultBallsToSpawn = 0;
			this.mSkullHiliteDir = 0f;
			for (int i = 0; i < 14; i++)
			{
				this.mNumPowerUpsThisLevel[i] = 0;
				this.mNumPowerupsActivated[i] = 0;
			}
			for (int j = 0; j < 6; j++)
			{
				this.mBallColorHasPowerup[j] = 0;
			}
			for (int k = 0; k < Enumerable.Count<WarningLight>(this.mWarningLights); k++)
			{
				this.mWarningLights[k].mState = -1;
				if (this.mWarningLights[k].mPulseRate > 0f)
				{
					this.mWarningLights[k].mPulseRate *= -1f;
				}
			}
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x000540A0 File Offset: 0x000522A0
		private void DeleteBullet(Bullet theBullet)
		{
			if (theBullet == null)
			{
				return;
			}
			theBullet.Dispose();
			this.mBulletList.Remove(theBullet);
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x000540BC File Offset: 0x000522BC
		private void DeleteBall(Ball theBall)
		{
			Bullet bullet = theBall.GetBullet();
			if (bullet != null)
			{
				bullet.MergeFully();
				int num = this.mBulletList.IndexOf(bullet);
				if (num >= 0)
				{
					this.AdvanceMergingBullet(ref num);
				}
			}
			this.DeleteBullet(theBall.GetBullet());
			this.mBoard.CheckShouldClearGuideBall(theBall);
			theBall.SetCollidesWithPrev(false);
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x00054114 File Offset: 0x00052314
		public void SyncState(DataSync theSync)
		{
			SexyBuffer buffer = theSync.GetBuffer();
			theSync.RegisterPointer(this);
			theSync.SyncLong(ref this.mProxBombCounter);
			theSync.SyncBoolean(ref this.mDoingClearCurveRollout);
			theSync.SyncFloat(ref this.mSkullHilite);
			theSync.SyncFloat(ref this.mSkullHiliteDir);
			theSync.SyncBoolean(ref this.mInitialPathHilite);
			theSync.SyncLong(ref this.mLastPathHiliteWP);
			if (theSync.isRead())
			{
				if (buffer.ReadBoolean())
				{
					this.mLevel.mHoleMgr.SetPctOpen(this.mCurveNum, buffer.ReadFloat());
				}
				this.DeleteBalls();
				short num = buffer.ReadShort();
				for (int i = 0; i < (int)num; i++)
				{
					Bullet bullet = new Bullet();
					bullet.SyncState(theSync);
					this.mBulletList.Add(bullet);
				}
				num = buffer.ReadShort();
				for (int j = 0; j < (int)num; j++)
				{
					Ball ball = new Ball();
					ball.SyncState(theSync);
					this.mPendingBalls.Add(ball);
				}
				num = buffer.ReadShort();
				for (int k = 0; k < (int)num; k++)
				{
					Ball ball2 = new Ball();
					ball2.SyncState(theSync);
					ball2.InsertInList(this.mBallList, this.mBallList.Count, this);
				}
				buffer.ReadLong();
				num = (short)buffer.ReadLong();
				for (int l = 0; l < (int)num; l++)
				{
					PathSparkle pathSparkle = new PathSparkle();
					pathSparkle.mCel = (int)buffer.ReadLong();
					pathSparkle.mX = (int)buffer.ReadLong();
					pathSparkle.mY = (int)buffer.ReadLong();
					pathSparkle.mUpdateCount = (int)buffer.ReadLong();
					pathSparkle.mPri = (int)buffer.ReadLong();
					this.mSparkles.Add(pathSparkle);
				}
				this.mInkSpots.Clear();
				int num2 = (int)buffer.ReadLong();
				for (int m = 0; m < num2; m++)
				{
					InkBlot inkBlot = new InkBlot();
					inkBlot.mX = buffer.ReadFloat();
					inkBlot.mY = buffer.ReadFloat();
					inkBlot.mRadius = buffer.ReadFloat();
					inkBlot.mAlpha = buffer.ReadFloat();
					inkBlot.mAlphaDec = buffer.ReadFloat();
					inkBlot.mAngle = buffer.ReadFloat();
					inkBlot.mDelay = (int)buffer.ReadLong();
					inkBlot.mFadeDelayTimer = (int)buffer.ReadLong();
					this.mInkSpots.Add(inkBlot);
				}
			}
			else
			{
				if (this.mBoard.GetGameState() == GameState.GameState_Losing)
				{
					buffer.WriteBoolean(true);
					float pctOpen = this.mLevel.mHoleMgr.GetHole(this.mCurveNum).GetPctOpen();
					buffer.WriteFloat(pctOpen);
				}
				else
				{
					buffer.WriteBoolean(false);
				}
				buffer.WriteShort((short)this.mBulletList.Count);
				for (int n = 0; n < this.mBulletList.Count; n++)
				{
					this.mBulletList[n].SyncState(theSync);
				}
				buffer.WriteShort((short)this.mPendingBalls.Count);
				for (int num3 = 0; num3 < this.mPendingBalls.Count; num3++)
				{
					this.mPendingBalls[num3].SyncState(theSync);
				}
				buffer.WriteShort((short)this.mBallList.Count);
				for (int num4 = 0; num4 < this.mBallList.Count; num4++)
				{
					this.mBallList[num4].SyncState(theSync);
				}
				buffer.WriteLong((long)this.mWarningLights.Count);
				buffer.WriteLong((long)this.mSparkles.Count);
				for (int num5 = 0; num5 < this.mSparkles.Count; num5++)
				{
					PathSparkle pathSparkle2 = this.mSparkles[num5];
					buffer.WriteLong((long)pathSparkle2.mCel);
					buffer.WriteLong((long)pathSparkle2.mX);
					buffer.WriteLong((long)pathSparkle2.mY);
					buffer.WriteLong((long)pathSparkle2.mUpdateCount);
					buffer.WriteLong((long)pathSparkle2.mPri);
				}
				buffer.WriteLong((long)this.mInkSpots.Count);
				for (int num6 = 0; num6 < this.mInkSpots.Count; num6++)
				{
					InkBlot inkBlot2 = this.mInkSpots[num6];
					buffer.WriteFloat(inkBlot2.mX);
					buffer.WriteFloat(inkBlot2.mY);
					buffer.WriteFloat(inkBlot2.mRadius);
					buffer.WriteFloat(inkBlot2.mAlpha);
					buffer.WriteFloat(inkBlot2.mAlphaDec);
					buffer.WriteFloat(inkBlot2.mAngle);
					buffer.WriteLong((long)inkBlot2.mDelay);
					buffer.WriteLong((long)inkBlot2.mFadeDelayTimer);
				}
			}
			theSync.SyncBoolean(ref this.mNeedsSpeedup);
			theSync.SyncBoolean(ref this.mCanCheckForSpeedup);
			theSync.SyncBoolean(ref this.mHasReachedRolloutPoint);
			for (int num7 = 0; num7 < this.mWarningLights.Count; num7++)
			{
				this.mWarningLights[num7].SyncState(theSync);
			}
			for (int num8 = 0; num8 < 14; num8++)
			{
				theSync.SyncLong(ref this.mLastSpawnedPowerUpFrame[num8]);
				theSync.SyncLong(ref this.mNumPowerUpsThisLevel[num8]);
				theSync.SyncLong(ref this.mNumPowerupsActivated[num8]);
			}
			for (int num9 = 0; num9 < 6; num9++)
			{
				theSync.SyncLong(ref this.mBallColorHasPowerup[num9]);
			}
			theSync.SyncLong(ref this.mStopTime);
			theSync.SyncLong(ref this.mSlowCount);
			theSync.SyncLong(ref this.mBackwardCount);
			theSync.SyncLong(ref this.mTotalBalls);
			theSync.SyncFloat(ref this.mAdvanceSpeed);
			theSync.SyncLong(ref this.mFirstChainEnd);
			theSync.SyncBoolean(ref this.mFirstBallMovedBackwards);
			theSync.SyncBoolean(ref this.mHaveSets);
			theSync.SyncLong(ref this.mPathLightEndFrame);
			theSync.SyncBoolean(ref this.mHadPowerUp);
			theSync.SyncLong(ref this.mLastPathShowTick);
			theSync.SyncLong(ref this.mLastClearedBallPoint);
			theSync.SyncBoolean(ref this.mStopAddingBalls);
			theSync.SyncBoolean(ref this.mInDanger);
			theSync.SyncBoolean(ref this.mHasReachedCruisingSpeed);
			theSync.SyncBoolean(ref this.mHadPowerUp);
			theSync.SyncLong(ref this.mNumBallsCreated);
			theSync.SyncFloat(ref this.mSpeedScale);
			theSync.SyncFloat(ref this.mOverrideSpeed);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x00054750 File Offset: 0x00052950
		public void SetFarthestBall(int thePoint)
		{
			if (this.mBoard.GetGameState() == GameState.GameState_Losing)
			{
				return;
			}
			int num = this.mDangerPoint;
			if (num < 0)
			{
				num = 0;
			}
			float pct_open = 0f;
			if (thePoint >= num)
			{
				pct_open = (float)(thePoint - num) / (float)(this.mWayPointMgr.GetNumPoints() - num);
			}
			this.mLevel.mHoleMgr.SetPctOpen(this.mCurveNum, pct_open);
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x000547B0 File Offset: 0x000529B0
		private int GetNumInARow(Ball theBall, int theColor, ref Ball theNextEnd, ref Ball thePrevEnd)
		{
			if (theBall.GetColorType() != theColor)
			{
				return 0;
			}
			Ball ball = theBall;
			int num = 1;
			for (;;)
			{
				Ball nextBall = ball.GetNextBall(true);
				if (nextBall == null || nextBall.GetColorType() != theColor)
				{
					break;
				}
				ball = nextBall;
				num++;
			}
			Ball ball2 = theBall;
			for (;;)
			{
				Ball prevBall = ball2.GetPrevBall(true);
				if (prevBall == null || prevBall.GetColorType() != theColor)
				{
					break;
				}
				ball2 = prevBall;
				num++;
			}
			theNextEnd = ball;
			thePrevEnd = ball2;
			return num;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x00054820 File Offset: 0x00052A20
		private bool CheckSet(Ball theBall)
		{
			this.mHadPowerUp = false;
			Ball ball = null;
			Ball ball2 = null;
			int comboCount = theBall.GetComboCount();
			int numInARow = this.GetNumInARow(theBall, theBall.GetColorType(), ref ball, ref ball2);
			if (numInARow >= 3)
			{
				this.mBoard.SetNumCleared(0);
				this.mBoard.SetCurComboCount(comboCount);
				this.mBoard.SetCurComboScore(theBall.GetComboScore());
				this.mBoard.mNeedComboCount.Clear();
				for (int i = 0; i < 14; i++)
				{
					ZumasRevenge.Common.gGotPowerUp[i] = false;
				}
				int num = 0;
				int num2 = 0;
				Ball nextBall = ball.GetNextBall();
				for (Ball ball3 = ball2; ball3 != nextBall; ball3 = ball3.GetNextBall())
				{
					if (ball3.GetSuckPending())
					{
						ball3.SetSuckPending(false);
						this.mBoard.IncNumClearsInARow(1);
					}
					this.StartExploding(ball3);
					num += ball3.GetGapBonus();
					if (ball3.GetNumGaps() > num2)
					{
						num2 = ball3.GetNumGaps();
					}
					ball3.SetGapBonus(0, 0);
				}
				this.DoScoring(theBall, this.mBoard.GetNumCleared(), comboCount, num, num2);
				GameStats levelStats = this.mBoard.GetLevelStats();
				if (this.mBoard.GetCurComboCount() > levelStats.mMaxCombo || (this.mBoard.GetCurComboCount() == levelStats.mMaxCombo && this.mBoard.GetCurComboScore() >= levelStats.mMaxComboScore))
				{
					levelStats.mMaxCombo = this.mBoard.GetCurComboCount();
					levelStats.mMaxComboScore = this.mBoard.GetCurComboScore();
					if (levelStats.mMaxCombo >= 4)
					{
						this.mBoard.UnlockAchievement(EAchievementType.POWER_PLAYER);
					}
				}
				for (Ball ball3 = ball2; ball3 != nextBall; ball3 = ball3.GetNextBall())
				{
					ball3.SetComboCount(comboCount, this.mBoard.GetCurComboScore());
				}
				foreach (Ball ball4 in this.mBoard.mNeedComboCount)
				{
					ball4.SetComboCount(comboCount, this.mBoard.GetCurComboScore());
				}
				this.mBoard.mNeedComboCount.Clear();
				this.PlayComboSound(comboCount);
				this.mBoard.SetCurComboCount(0);
				this.mBoard.SetCurComboScore(0);
				return true;
			}
			return false;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x00054A64 File Offset: 0x00052C64
		private void DoScoring(Ball theBall, int theNumBalls, int theComboCount, int theGapBonus, int theNumGaps)
		{
			if (theNumBalls == 0)
			{
				return;
			}
			int num = theNumBalls * 10 + theGapBonus + theComboCount * 100;
			bool flag = false;
			int num2 = (ZumasRevenge.Common.gSuckMode ? 10 : 4);
			int num3 = 0;
			if (this.mBoard.GetNumClearsInARow() > num2 && theComboCount == 0)
			{
				flag = true;
				num3 = (ZumasRevenge.Common.gSuckMode ? 10 : 100) + 10 * (this.mBoard.GetNumClearsInARow() - (num2 + 1));
				num += num3;
				if (this.mLevel.AllowPointsFromBalls())
				{
					this.mBoard.IncCurInARowBonus(num3);
				}
			}
			if (this.mLevel.AllowPointsFromBalls())
			{
				this.mBoard.IncCurComboScore(num);
				this.mBoard.IncScore(num, true);
				GameStats levelStats = this.mBoard.GetLevelStats();
				if (theComboCount > 0)
				{
					levelStats.mNumCombos++;
				}
				if (theGapBonus > 0)
				{
					levelStats.mNumGaps++;
				}
			}
			Board board = ((GameApp)GlobalMembers.gSexyApp).GetBoard();
			BonusTextElement bonusTextElement = null;
			if (this.mLevel.AllowPointsFromBalls())
			{
				bonusTextElement = board.AddText(string.Format("+{0}", num), (int)theBall.GetX(), (int)theBall.GetY());
				if (bonusTextElement != null)
				{
					bonusTextElement.mBonus.mSolidColor = (this.mLastScoreColor = new SexyColor(ZumasRevenge.Common.gBallColors[theBall.GetColorType()]));
				}
			}
			if (theComboCount > 0)
			{
				BonusText bonusText = null;
				if (this.mLevel.AllowPointsFromBalls())
				{
					float num4 = ZumasRevenge.Common._M(1.5f);
					float num5 = 1f + (num4 - 1f) / 10f * (float)(theComboCount + 1);
					if (num5 < 1f)
					{
						num5 = 1f;
					}
					BonusTextElement bonusTextElement2 = board.AddText(TextManager.getInstance().getString(100) + string.Format("x{0}", theComboCount + 1), (int)theBall.GetX(), (int)theBall.GetY(), num5, (bonusTextElement != null) ? bonusTextElement.mHandle : (-1), null);
					if (bonusTextElement2 != null)
					{
						bonusText = bonusTextElement2.mBonus;
					}
					this.mBoard.GetBetaStats().Combo(theComboCount * 100, theComboCount + 1);
				}
				if (bonusText != null)
				{
					bonusText.mSolidColor = this.mLastScoreColor;
				}
				this.mLevel.MadeCombo(theComboCount);
			}
			if (theNumGaps >= 1)
			{
				this.mBoard.GetBetaStats().GapShot(theGapBonus, theNumGaps);
				this.mBoard.UnlockAchievement(EAchievementType.THREAD_THE_NEEDLE);
			}
			if (theGapBonus > 0)
			{
				this.mLevel.MadeGapShot(theNumGaps);
				if (this.mLevel.AllowPointsFromBalls())
				{
					int num6 = ZumasRevenge.Common._DS(20);
					int attach_handle = ((bonusTextElement == null || theComboCount > 0) ? (-1) : bonusTextElement.mHandle);
					BonusTextElement bonusTextElement3;
					if (theNumGaps > 1)
					{
						if (theNumGaps > 3)
						{
							this.mApp.SetAchievement("double_gap");
							bonusTextElement3 = board.AddText(TextManager.getInstance().getString(101), (int)theBall.GetX(), (int)theBall.GetY() + num6, ZumasRevenge.Common._M(1.5f), attach_handle, null);
							this.mApp.mUserProfile.mNumTripleGapShots++;
							this.mApp.mUserProfile.mNumDoubleGapShots++;
						}
						else if (theNumGaps > 2)
						{
							this.mApp.SetAchievement("double_gap");
							bonusTextElement3 = board.AddText(TextManager.getInstance().getString(102), (int)theBall.GetX(), (int)theBall.GetY() + num6, ZumasRevenge.Common._M(1.35f), attach_handle, null);
							this.mApp.mUserProfile.mNumTripleGapShots++;
							this.mApp.mUserProfile.mNumDoubleGapShots++;
						}
						else
						{
							this.mApp.SetAchievement("double_gap");
							this.mApp.mUserProfile.mNumDoubleGapShots++;
							bonusTextElement3 = board.AddText(TextManager.getInstance().getString(103), (int)theBall.GetX(), (int)theBall.GetY() + num6, ZumasRevenge.Common._M(1.2f), attach_handle, null);
						}
					}
					else
					{
						bonusTextElement3 = board.AddText(TextManager.getInstance().getString(104), (int)theBall.GetX(), (int)theBall.GetY() + num6, ZumasRevenge.Common._M(1.1f), attach_handle, null);
					}
					if (bonusTextElement3 != null)
					{
						bonusTextElement3.mBonus.mSolidColor = this.mLastScoreColor;
					}
				}
				if (this.mLevel.mBoss == null && !this.mLevel.IsFinalBossLevel())
				{
					board.mApp.mSoundPlayer.Play(Res.GetSoundByID(ResID.SOUND_GAP_BONUS));
					for (int i = 1; i < theNumGaps; i++)
					{
						SoundAttribs soundAttribs = new SoundAttribs();
						soundAttribs.pitch = (float)(i * 2);
						soundAttribs.delay = i * 10;
						board.mApp.mSoundPlayer.Play(Res.GetSoundByID(ResID.SOUND_GAP_BONUS), soundAttribs);
					}
				}
			}
			this.mBoard.GetBetaStats().ChainShot(num3, this.mBoard.GetNumClearsInARow() + 1);
			if (flag && this.mLevel.AllowPointsFromBalls())
			{
				float num7 = ZumasRevenge.Common._M(1.5f);
				float num8 = 1f + (num7 - 1f) / 10f * (float)(this.mBoard.GetNumClearsInARow() + 1 - num2);
				if (num8 < 1f)
				{
					num8 = 1f;
				}
				int attach_handle2 = ((bonusTextElement == null || theGapBonus > 0 || theComboCount > 0) ? (-1) : bonusTextElement.mHandle);
				BonusTextElement bonusTextElement4 = this.mBoard.AddText(TextManager.getInstance().getString(105) + string.Format(" x{0}", this.mBoard.GetNumClearsInARow() + 1), (int)theBall.GetX(), (int)theBall.GetY(), num8, attach_handle2, null);
				if (bonusTextElement4 != null)
				{
					bonusTextElement4.mBonus.mSolidColor = this.mLastScoreColor;
				}
				int num9 = this.mBoard.GetNumClearsInARow() - 5;
				if (num9 > 10)
				{
					num9 = 10;
				}
				if (this.mLevel.mBoss != null && !this.mLevel.IsFinalBossLevel())
				{
					SoundAttribs soundAttribs2 = new SoundAttribs();
					soundAttribs2.pitch = (float)num9;
					soundAttribs2.delay = 1;
					this.mBoard.mApp.mSoundPlayer.Play(Res.GetSoundByID(ResID.SOUND_CHAIN_BONUS), soundAttribs2);
				}
			}
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x00055084 File Offset: 0x00053284
		private void PlayComboSound(int inComboCount)
		{
			if (this.mHadPowerUp)
			{
				return;
			}
			SoundAttribs soundAttribs = new SoundAttribs();
			soundAttribs.pitch = 0f;
			soundAttribs.volume = 0.4f + 0.2f * (float)inComboCount;
			if (soundAttribs.volume > 1f)
			{
				soundAttribs.volume = 1f;
			}
			int soundByID = Res.GetSoundByID(ResID.SOUND_BALLDESTROYED5);
			int soundByID2 = Res.GetSoundByID(ResID.SOUND_COMBO);
			switch (inComboCount)
			{
			case 0:
				soundByID = Res.GetSoundByID(ResID.SOUND_BALLDESTROYED1);
				break;
			case 1:
				soundByID = Res.GetSoundByID(ResID.SOUND_BALLDESTROYED2);
				soundAttribs.pitch = 2f;
				break;
			case 2:
				soundByID = Res.GetSoundByID(ResID.SOUND_BALLDESTROYED3);
				soundByID2 = Res.GetSoundByID(ResID.SOUND_COMBO3X);
				break;
			case 3:
				soundByID = Res.GetSoundByID(ResID.SOUND_BALLDESTROYED4);
				break;
			default:
				soundByID2 = Res.GetSoundByID(ResID.SOUND_COMBO3X);
				soundAttribs.pitch = 2f;
				break;
			}
			this.mApp.mSoundPlayer.Play(soundByID);
			this.mApp.mSoundPlayer.Play(soundByID2, soundAttribs);
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0005518C File Offset: 0x0005338C
		private void AddPendingBall()
		{
			Ball ball = new Ball();
			int mNumColors = this.mCurveDesc.mVals.mNumColors;
			int num;
			if (Enumerable.Count<Ball>(this.mPendingBalls) > 0)
			{
				num = this.mPendingBalls[this.mPendingBalls.Count - 1].GetColorType();
			}
			else if (Enumerable.Count<Ball>(this.mBallList) > 0)
			{
				num = Enumerable.First<Ball>(this.mBallList).GetColorType();
			}
			else
			{
				num = this.mLevel.GetRandomPendingBallColor(mNumColors);
			}
			if (num >= mNumColors)
			{
				num = this.mLevel.GetRandomPendingBallColor(mNumColors);
			}
			int numPendingMatches = this.GetNumPendingMatches();
			int num2 = (GameApp.gApp.Is3DAccelerated() ? ZumasRevenge.Common._M(30) : ZumasRevenge.Common._M1(33));
			int num3 = (GameApp.gApp.Is3DAccelerated() ? ZumasRevenge.Common._M(4) : ZumasRevenge.Common._M1(4));
			int num6;
			if (!this.mApp.mUserProfile.HasSeenHint(ZumaProfile.FIRST_SHOT_HINT) && this.mLevel.mNum == 1 && this.mLevel.mZone == 1 && Enumerable.Count<Ball>(this.mBallList) == num2 - 1)
			{
				ball.SetColorType(1);
			}
			else if (!this.mApp.mUserProfile.HasSeenHint(ZumaProfile.FIRST_SHOT_HINT) && this.mLevel.mNum == 1 && this.mLevel.mZone == 1 && Enumerable.Count<Ball>(this.mBallList) == num2 + num3 + 1)
			{
				ball.SetColorType(0);
			}
			else if (!this.mApp.mUserProfile.HasSeenHint(ZumaProfile.FIRST_SHOT_HINT) && this.mLevel.mNum == 1 && this.mLevel.mZone == 1 && Enumerable.Count<Ball>(this.mBallList) >= num2 && Enumerable.Count<Ball>(this.mBallList) <= num2 + num3)
			{
				ball.SetColorType(2);
			}
			else
			{
				int maxSingles = GameApp.gDDS.GetMaxSingles(this.mCurveNum);
				int num4 = GameApp.gDDS.GetBallRepeat(this.mCurveNum) * (int)GameApp.gDDS.mHandheldBalance.mChanceOfSameColorBallIncrease;
				int maxClumps = GameApp.gDDS.GetMaxClumps(this.mCurveNum);
				int num5 = MathUtils.SafeRand() % 100;
				if (num5 <= num4 && numPendingMatches < maxClumps)
				{
					num6 = num;
				}
				else if (maxSingles < 10 && this.GetNumPendingSingles(1) == 1 && (maxSingles == 0 || this.GetNumPendingSingles(10) > maxSingles))
				{
					num6 = num;
				}
				else
				{
					do
					{
						num6 = this.mLevel.GetRandomPendingBallColor(mNumColors);
					}
					while (num6 == num);
				}
				ball.SetColorType(num6);
			}
			num6 = ball.GetColorType();
			ball.RandomizeFrame();
			this.mPendingBalls.Add(ball);
			Ball guideBall = this.mBoard.GetGuideBall();
			if (guideBall != null && guideBall.GetColorType() == num6 && this.mBoard.GetGun().LightningMode())
			{
				ball.DoElectricOverlay(true);
			}
			this.mNumBallsCreated++;
			if (this.mCurveDesc.mVals.mNumBalls > 0 && this.mNumBallsCreated >= this.mCurveDesc.mVals.mNumBalls)
			{
				this.mStopAddingBalls = true;
			}
			this.mLevel.BallCreatedCallback(ball, this.mNumBallsCreated);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x000554A0 File Offset: 0x000536A0
		private void AddBall()
		{
			if (!ZumasRevenge.Common.gAddBalls || (this.mLevel.mNum == 2147483647 && this.mLevel.mZone == 1 && this.mBoard.mPreventBallAdvancement))
			{
				return;
			}
			if (Enumerable.Count<Ball>(this.mPendingBalls) == 0)
			{
				if (this.mStopAddingBalls || (this.mLevel.mBoss != null && !this.mLevel.mBoss.CanAdvanceBalls()))
				{
					return;
				}
				this.AddPendingBall();
			}
			Ball ball = Enumerable.First<Ball>(this.mPendingBalls);
			this.mWayPointMgr.SetWayPoint(ball, 1f, this.mLevel.mLoopAtEnd);
			if (Enumerable.Count<Ball>(this.mBallList) > 0)
			{
				Ball ball2 = Enumerable.First<Ball>(this.mBallList);
				if (this.mAdvanceSpeed > (float)ball2.GetRadius() && ball2.GetWayPoint() >= 0f)
				{
					if (ball2.GetWayPoint() - this.mAdvanceSpeed < 5f)
					{
						float num = ball2.GetWayPoint() - (float)ball2.GetRadius() - (float)ball.GetRadius() - 0.001f;
						ball.SetWayPoint(num, this.mWayPointMgr.InTunnel((int)num));
					}
				}
				else if (ball.GetWayPoint() > ball2.GetWayPoint() || ball.CollidesWith(ball2))
				{
					return;
				}
			}
			ball.InsertInList(this.mBallList, 0, this);
			ball.UpdateCollisionInfo(5 + (int)this.mAdvanceSpeed);
			ball.SetNeedCheckCollision(true);
			ball.SetRotation(this.mWayPointMgr.GetRotationForPoint((int)ball.GetWayPoint()));
			ball.SetBackwardsCount(0);
			ball.SetSuckCount(0);
			ball.SetGapBonus(0, 0);
			ball.SetComboCount(0, 0);
			if (this.mProxBombCounter > 0 && --this.mProxBombCounter == 0)
			{
				this.mProxBombCounter = MathUtils.IntRange(this.mLevel.mBoss.mBombFreqMin, this.mLevel.mBoss.mBombFreqMax);
				ball.SetPowerType(PowerType.PowerType_ProximityBomb);
				ball.SetPowerCount(this.mLevel.mBoss.mBombDuration);
			}
			this.mPendingBalls.RemoveAt(0);
			if (ball.GetWayPoint() > 1f)
			{
				this.AddBall();
			}
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x000556C0 File Offset: 0x000538C0
		private void UpdateBalls()
		{
			LevelMgr levelMgr = ((GameApp)GlobalMembers.gSexyApp).GetLevelMgr();
			foreach (Ball ball in this.mBallList)
			{
				ball.Update();
				if (levelMgr.mColorNukeTimeAfterZuma >= 0 && ball.GetPowerOrDestType() == PowerType.PowerType_ColorNuke && this.mBoard.HasAchievedZuma() && ball.GetPowerCount() > levelMgr.mColorNukeTimeAfterZuma)
				{
					ball.SetPowerCount(levelMgr.mColorNukeTimeAfterZuma);
				}
			}
			foreach (Bullet bullet in this.mBulletList)
			{
				bullet.Update();
			}
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x000557A0 File Offset: 0x000539A0
		private void AdvanceBalls()
		{
			if (this.mBallList.Count == 0)
			{
				return;
			}
			bool flag = this.mLevel.CanAdvanceBalls();
			int num = this.mDangerPoint;
			float num3;
			if (!this.mBoard.GauntletMode())
			{
				int num2 = this.mLevel.mZone;
				if (num2 >= DDS.NUM_ADVENTURE_ZONES)
				{
					num2 = DDS.NUM_ADVENTURE_ZONES - 1;
				}
				num3 = GameApp.gDDS.mHandheldBalance.mAdventureModeSpeedDelta[num2];
			}
			else
			{
				int num4 = this.mLevel.mGauntletMultipliersEarned;
				if (num4 >= DDS.NUM_CHALLENGE_LEVELS)
				{
					num4 = DDS.NUM_CHALLENGE_LEVELS - 1;
				}
				num3 = GameApp.gDDS.mHandheldBalance.mChallengeModeSpeedDelta[num4];
			}
			float num5 = GameApp.gDDS.GetSpeed(this.mCurveNum) * this.mSpeedScale * num3;
			if (this.mCurveDesc.mVals.mAccelerationRate != 0f)
			{
				this.mCurveDesc.mCurAcceleration += this.mCurveDesc.mVals.mAccelerationRate;
				num5 += this.mCurveDesc.mCurAcceleration;
				if (num5 > this.mCurveDesc.mVals.mMaxSpeed)
				{
					num5 = this.mCurveDesc.mVals.mMaxSpeed;
				}
			}
			if (this.mSlowCount != 0 || (this.mBoard != null && this.mBoard.GetGun() != null && this.mBoard.GetGun().LaserMode()))
			{
				num5 /= 4f;
			}
			float slowFactor = GameApp.gDDS.GetSlowFactor(this.mCurveNum);
			float num6 = this.mLevel.mPostZumaTimeSpeedInc * num5 + num5;
			float num7 = slowFactor - this.mLevel.mPostZumaTimeSlowInc * this.mCurveDesc.mVals.mSlowFactor;
			if (num7 < 1f)
			{
				num7 = 1f;
			}
			if (this.mBoard.GauntletMode() || this.mApp.GetLevelMgr().mPostZumaTime == 0 || !this.mBoard.HasAchievedZuma())
			{
				num6 = num5;
				num7 = slowFactor;
			}
			if (ZumasRevenge.Common.gDieAtEnd)
			{
				if (this.mFirstChainEnd < this.mDangerPoint - GameApp.gDDS.GetSlowDistance(this.mCurveNum) || !this.mHasReachedCruisingSpeed)
				{
					num5 = num6;
				}
				else if (this.mFirstChainEnd < this.mDangerPoint)
				{
					float num8 = (float)(this.mFirstChainEnd - (this.mDangerPoint - GameApp.gDDS.GetSlowDistance(this.mCurveNum))) / (float)GameApp.gDDS.GetSlowDistance(this.mCurveNum);
					num5 = (1f - num8) * num6 + num8 * num6 / num7;
				}
				else
				{
					num5 /= num7;
					this.mBoard.SetRollingInDangerZone();
				}
			}
			if (this.mBoard.GauntletMode() && this.mBoard.GetStateCount() > 300)
			{
				int farthestBallPercent = this.GetFarthestBallPercent(false);
				int gauntletHurryDist = GameApp.gDDS.GetGauntletHurryDist(this.mLevel.mNumCurves);
				if (farthestBallPercent < gauntletHurryDist)
				{
					ZumasRevenge.Common._M(1.6f);
					num5 += (float)(gauntletHurryDist - farthestBallPercent) * GameApp.gDDS.GetGauntletHurryMaxSpeed(this.mLevel.mNumCurves) / (float)gauntletHurryDist;
					if (this.mAdvanceSpeed < num5)
					{
						this.mAdvanceSpeed += 0.03f;
					}
				}
			}
			bool flag2 = false;
			if (this.mDoingClearCurveRollout)
			{
				int i = this.mBallList.Count - 1;
				while (i >= 0)
				{
					Ball ball = this.mBallList[i];
					if (!ball.GetIsExploding())
					{
						if (ball.GetWayPoint() / (float)this.mWayPointMgr.GetEndPoint() >= this.mApp.GetLevelMgr().mClearCurveRolloutPct)
						{
							flag2 = true;
							break;
						}
						break;
					}
					else
					{
						i--;
					}
				}
			}
			if (this.mAdvanceSpeed > num5 && this.mBoard != null && !this.mBoard.mPreventBallAdvancement && (!this.mDoingClearCurveRollout || flag2))
			{
				this.mDoingClearCurveRollout = false;
				this.mAdvanceSpeed -= 0.1f;
			}
			else if (this.mAdvanceSpeed <= num5 && flag2)
			{
				this.mDoingClearCurveRollout = false;
			}
			if (this.mAdvanceSpeed < num5)
			{
				this.mAdvanceSpeed += 0.005f;
				if (this.mAdvanceSpeed >= num5)
				{
					this.mAdvanceSpeed = num5;
				}
			}
			float num9 = (flag ? this.mAdvanceSpeed : 0f);
			if (this.mOverrideSpeed >= 0f)
			{
				num9 += this.mOverrideSpeed;
			}
			if (this.mBoard != null && this.mBoard.mPreventBallAdvancement)
			{
				num9 = 0f;
			}
			Ball ball2 = this.mBallList[0];
			float num10 = ball2.GetWayPoint();
			if (!this.mFirstBallMovedBackwards && this.mStopTime == 0)
			{
				this.mWayPointMgr.SetWayPoint(ball2, num10 + num9, this.mLevel.mLoopAtEnd);
			}
			if (ZumasRevenge.Common.gSuckMode && this.mStopAddingBalls && this.mBallList.Count > 0)
			{
				Ball ball3 = this.mBallList[0];
				this.mAdvanceSpeed = num5;
				if (ball3.GetSpeedy())
				{
					Ball nextBall = ball3.GetNextBall();
					if (nextBall != null)
					{
						if (nextBall.GetSpeedy())
						{
							this.mAdvanceSpeed = 20f;
						}
						else
						{
							float num11 = nextBall.GetWayPoint() - ball3.GetWayPoint();
							this.mAdvanceSpeed = num11 / 10f;
						}
						if (this.mAdvanceSpeed > 20f)
						{
							this.mAdvanceSpeed = 20f;
						}
						else if (this.mAdvanceSpeed < num5)
						{
							this.mAdvanceSpeed = num5;
						}
					}
				}
			}
			bool flag3 = false;
			int num12 = 0;
			Ball ball4 = null;
			while (num12 != Enumerable.Count<Ball>(this.mBallList))
			{
				Ball ball5 = this.mBallList[num12];
				if (++num12 >= this.mBallList.Count)
				{
					break;
				}
				Ball ball6 = this.mBallList[num12];
				float wayPoint = ball6.GetWayPoint();
				float wayPoint2 = ball5.GetWayPoint();
				bool flag4 = false;
				if (wayPoint2 > wayPoint - (float)ball5.GetRadius() - (float)ball6.GetRadius())
				{
					this.mWayPointMgr.SetWayPoint(ball6, wayPoint2 + (float)ball5.GetRadius() + (float)ball6.GetRadius(), this.mLevel.mLoopAtEnd);
					flag4 = true;
				}
				if (flag4)
				{
					if (!ball5.GetCollidesWithNext())
					{
						if (ball5.GetSpeedy() && !ball6.GetSpeedy())
						{
							for (Ball ball7 = ball5; ball7 != null; ball7 = ball7.GetPrevBall(true))
							{
								ball7.SetSpeedy(false);
							}
						}
						ball5.SetCollidesWithNext(true);
						this.mBoard.PlayBallClick(Res.GetSoundByID(ResID.SOUND_BALLCLICK1));
					}
					ball6.GetWayPoint();
					ball5.SetNeedCheckCollision(false);
				}
				if (ball4 == null && !ball5.GetCollidesWithNext())
				{
					ball4 = ball5;
					int startDistance = GameApp.gDDS.GetStartDistance(this.mCurveNum);
					if (ball4.GetWayPoint() < (float)startDistance / 100f * (float)this.GetCurveLength())
					{
						flag3 = true;
					}
				}
			}
			if (!flag3 && this.mLevel.mTempSpeedupTimer <= 0)
			{
				this.mCanCheckForSpeedup = true;
				this.mOverrideSpeed = -1f;
			}
			if (!this.mHasReachedRolloutPoint && this.mBackwardCount <= 0 && this.mBallList[this.mBallList.Count - 1].GetWayPoint() >= (float)GameApp.gDDS.GetStartDistance(this.mCurveNum) / 100f * (float)this.GetCurveLength())
			{
				this.mHasReachedRolloutPoint = true;
			}
			if (ball4 == null)
			{
				ball4 = this.mBallList[this.mBallList.Count - 1];
				this.mCanCheckForSpeedup = true;
				if (this.HasReachedCruisingSpeed())
				{
					flag3 = true;
				}
			}
			this.mFirstChainEnd = (int)ball4.GetWayPoint();
			if (flag3 && this.mCanCheckForSpeedup && this.mLevel.mHurryToRolloutAmt > 0f)
			{
				int startDistance2 = GameApp.gDDS.GetStartDistance(this.mCurveNum);
				if ((float)this.mFirstChainEnd < (float)startDistance2 / 100f * (float)this.GetCurveLength())
				{
					this.mCanCheckForSpeedup = false;
					this.mOverrideSpeed = this.mLevel.mHurryToRolloutAmt;
				}
				else
				{
					this.mCanCheckForSpeedup = true;
					this.mOverrideSpeed = -1f;
				}
			}
			bool flag5 = this.mFirstChainEnd >= num;
			if (flag5 && ZumasRevenge.Common.gDieAtEnd)
			{
				uint boardTickCount = ZumasRevenge.Common.GetBoardTickCount();
				float num13 = (float)(this.GetCurveLength() - this.mFirstChainEnd) / (float)(this.GetCurveLength() - this.mDangerPoint);
				uint num14 = (uint)(100f + 4000f * num13);
				int stateCount = this.mBoard.GetStateCount();
				if (stateCount >= this.mPathLightEndFrame && boardTickCount - this.mLastPathShowTick >= num14)
				{
					int num15 = 0;
					this.mLevel.GetFarthestBallPercent(ref num15, false);
					if (num15 == this.mCurveNum)
					{
						SoundAttribs soundAttribs = new SoundAttribs();
						soundAttribs.stagger = 10;
						this.mApp.mSoundPlayer.Play(Res.GetSoundByID(ResID.SOUND_WARNING), soundAttribs);
					}
					this.mLastPathShowTick = boardTickCount;
					this.mPathLightEndFrame = stateCount;
					if (Enumerable.Count<WarningLight>(this.mWarningLights) > 0)
					{
						int num16 = Enumerable.Count<WarningLight>(this.mWarningLights);
						for (int j = 0; j < Enumerable.Count<WarningLight>(this.mWarningLights); j++)
						{
							WarningLight warningLight = this.mWarningLights[j];
							Image imageByID = Res.GetImageByID(ResID.IMAGE_SKULL_PATH);
							if (warningLight.mWaypoint + (float)(imageByID.mWidth / 2) > (float)this.mFirstChainEnd)
							{
								num16 = j;
								break;
							}
						}
						if (num16 < Enumerable.Count<WarningLight>(this.mWarningLights))
						{
							this.mWarningLights[num16].mPulseRate = ZumasRevenge.Common._M(30f) * (1f - num13);
							float num17 = ZumasRevenge.Common._M(10f);
							if (this.mWarningLights[num16].mPulseRate < num17)
							{
								this.mWarningLights[num16].mPulseRate = num17;
							}
						}
					}
				}
			}
			bool flag6 = this.mInDanger;
			this.mInDanger = this.mBallList[this.mBallList.Count - 1].GetWayPoint() >= (float)this.mDangerPoint && ZumasRevenge.Common.gDieAtEnd;
			if (flag6 != this.mInDanger && Enumerable.Count<WarningLight>(this.mWarningLights) > 0)
			{
				for (int k = 0; k < Enumerable.Count<WarningLight>(this.mWarningLights); k++)
				{
					this.mWarningLights[k].mState = (this.mInDanger ? 1 : (-1));
				}
			}
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x000561C4 File Offset: 0x000543C4
		private void AdvanceBackwardBalls()
		{
			if (this.mLevel.mBoss != null && !this.mLevel.mBoss.CanAdvanceBalls())
			{
				return;
			}
			this.mFirstBallMovedBackwards = false;
			if (this.mBallList.Count == 0)
			{
				return;
			}
			int num = this.mBallList.Count - 1;
			bool flag = false;
			float num2 = 0f;
			if (this.mBackwardCount > 0)
			{
				this.mBallList[this.mBallList.Count - 1].SetBackwardsSpeed(1f * this.mSpeedScale);
				this.mBallList[this.mBallList.Count - 1].SetBackwardsCount(1);
			}
			for (;;)
			{
				Ball ball = this.mBallList[num];
				int backwardsCount = ball.GetBackwardsCount();
				if (backwardsCount > 0)
				{
					num2 = ball.GetBackwardsSpeed();
					this.mWayPointMgr.SetWayPoint(ball, ball.GetWayPoint() - num2, this.mLevel.mLoopAtEnd);
					ball.SetBackwardsCount(backwardsCount - 1);
					flag = true;
				}
				if (num == 0)
				{
					break;
				}
				Ball ball2 = this.mBallList[--num];
				if (flag)
				{
					if (ball2.GetCollidesWithNext())
					{
						this.mWayPointMgr.SetWayPoint(ball2, ball2.GetWayPoint() - num2, this.mLevel.mLoopAtEnd);
					}
					else
					{
						float num3 = ball.GetWayPoint() - (float)ball.GetRadius() - (float)ball2.GetRadius();
						if (ball2.GetWayPoint() > num3)
						{
							flag = true;
							if (!ball2.GetCollidesWithNext())
							{
								ball2.SetCollidesWithNext(true);
								this.mBoard.PlayBallClick(Res.GetSoundByID(ResID.SOUND_BALLCLICK1));
							}
							num2 = ball2.GetWayPoint() - num3;
							ball2.SetWayPoint(num3, this.mWayPointMgr.InTunnel((int)num3));
						}
						else
						{
							flag = false;
						}
					}
				}
			}
			if (flag)
			{
				this.mFirstBallMovedBackwards = true;
				if (this.mStopTime < 20)
				{
					this.mStopTime = 20;
				}
			}
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x00056398 File Offset: 0x00054598
		private void UpdateSuckingBalls()
		{
			if (this.mLevel.mBoss != null && !this.mLevel.mBoss.CanAdvanceBalls())
			{
				return;
			}
			int num = 0;
			while (num != Enumerable.Count<Ball>(this.mBallList))
			{
				Ball ball = this.mBallList[num];
				if (!ball.GetSuckBack() && ball.GetSuckCount() > 0)
				{
					this.UpdateForwardSuckingBalls();
					return;
				}
				int suckCount = ball.GetSuckCount();
				float num2 = (float)(suckCount >> 3) * this.mSpeedScale;
				if (ball.GetSuckCount() > 0)
				{
					Ball ball2 = null;
					while (num != Enumerable.Count<Ball>(this.mBallList))
					{
						ball2 = this.mBallList[num++];
						ball2.SetSuckCount(0);
						this.mWayPointMgr.SetWayPoint(ball2, ball2.GetWayPoint() - num2, this.mLevel.mLoopAtEnd);
						Bullet bullet = ball2.GetBullet();
						if (bullet != null && !bullet.mDoNewMerge)
						{
							Ball pushBall = bullet.GetPushBall();
							if (pushBall != null)
							{
								this.mWayPointMgr.FindFreeWayPoint(pushBall, bullet, false, this.mLevel.mLoopAtEnd);
							}
							bullet.UpdateHitPos();
						}
						if (!ball2.GetCollidesWithNext())
						{
							break;
						}
					}
					Ball ball3 = ball2;
					ball.SetSuckCount(suckCount + 1);
					Ball prevBall = ball.GetPrevBall();
					if (this.mLevel.mZone == 5 && this.mLevel.mNum != 10 && this.mBoard.GetGameState() == GameState.GameState_Playing && this.mBoard.mUpdateCnt % ZumasRevenge.Common._M(2) == 0)
					{
						for (int i = 0; i < ZumasRevenge.Common._M(1); i++)
						{
							Bubble bubble = new Bubble();
							bubble.Init((float)ZumasRevenge.Common._M(0), MathUtils.FloatRange(ZumasRevenge.Common._M1(-1.5f), ZumasRevenge.Common._M2(-0.75f)), MathUtils.FloatRange(ZumasRevenge.Common._M3(0.05f), ZumasRevenge.Common._M4(0.2f)), (int)MathUtils.FloatRange(ZumasRevenge.Common._M5(15f), ZumasRevenge.Common._M6(25f)));
							bubble.SetAlphaFade(ZumasRevenge.Common._M(2f));
							bubble.SetX(ball.GetX() + (float)(-10 + MathUtils.SafeRand() % 20));
							bubble.SetY(ball.GetY());
							this.mLevel.mFrog.AddBubble(bubble);
						}
					}
					if (prevBall != null && prevBall.GetColorType() == ball.GetColorType() && prevBall.GetIsExploding())
					{
						while (prevBall != null)
						{
							prevBall = prevBall.GetPrevBall();
							if (prevBall != null && !prevBall.GetIsExploding())
							{
								break;
							}
						}
					}
					bool flag = prevBall != null && prevBall.GetColorType() == ball.GetColorType();
					if (flag)
					{
						bool flag2 = false;
						float num3 = ball.GetWayPoint() - (float)ball.GetRadius() - (float)prevBall.GetRadius();
						if (prevBall.GetWayPoint() > num3)
						{
							this.mWayPointMgr.SetWayPoint(prevBall, num3, this.mLevel.mLoopAtEnd);
							flag2 = true;
						}
						if (flag2)
						{
							this.mBoard.PlayBallClick(Res.GetSoundByID(ResID.SOUND_BALLCLICK1));
							prevBall.SetCollidesWithNext(true);
							ball.SetSuckCount(0);
							int num4 = 5 + 5 * ball.GetComboCount();
							bool flag3 = true;
							if (!this.CheckSet(ball) || ball.GetSuckFromCompacting())
							{
								ball.SetComboCount(0, 0);
							}
							if (num4 > 40)
							{
								num4 = 40;
							}
							num4 *= 3;
							if (flag3 && ball3.GetBackwardsCount() == 0)
							{
								ball3.SetBackwardsCount(30);
								float num5 = (float)ball.GetComboCount() * 1.5f;
								if (num5 <= 0.5f)
								{
									num5 = 0.5f;
								}
								ball3.SetBackwardsSpeed(num5);
							}
							this.ClearPendingSucks(ball3);
						}
					}
					else
					{
						ball.SetSuckCount(0);
					}
				}
				else
				{
					num++;
				}
			}
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0005673C File Offset: 0x0005493C
		private void UpdateForwardSuckingBalls()
		{
			int num = Enumerable.Count<Ball>(this.mBallList);
			num--;
			for (;;)
			{
				Ball ball = this.mBallList[num];
				int suckCount = ball.GetSuckCount();
				float num2 = (float)(suckCount >> 3) * this.mSpeedScale;
				if (ball.GetSuckCount() > 0)
				{
					Ball ball2;
					for (;;)
					{
						ball2 = this.mBallList[num];
						ball2.SetSuckCount(0, false);
						this.mWayPointMgr.SetWayPoint(ball2, ball2.GetWayPoint() + num2, this.mLevel.mLoopAtEnd);
						Bullet bullet = ball2.GetBullet();
						if (bullet != null && !bullet.mDoNewMerge)
						{
							Ball pushBall = bullet.GetPushBall();
							if (pushBall != null)
							{
								this.mWayPointMgr.FindFreeWayPoint(pushBall, bullet, false, this.mLevel.mLoopAtEnd);
							}
							bullet.UpdateHitPos();
						}
						if (!ball2.GetCollidesWithPrev() || num == 0)
						{
							break;
						}
						num--;
					}
					Ball theEndBall = ball2;
					ball.SetSuckCount(suckCount + 1, false);
					Ball nextBall = ball.GetNextBall();
					if (nextBall != null)
					{
						bool flag = false;
						float num3 = ball.GetWayPoint() + (float)ball.GetRadius() + (float)nextBall.GetRadius();
						if (nextBall.GetWayPoint() < num3)
						{
							this.mWayPointMgr.SetWayPoint(nextBall, num3, this.mLevel.mLoopAtEnd);
							flag = true;
						}
						if (flag)
						{
							nextBall.SetCollidesWithPrev(true);
							ball.SetSuckCount(0, true);
							if (!this.CheckSet(ball))
							{
								ball.SetComboCount(0, 0);
							}
							this.ClearPendingSucks(theEndBall);
						}
					}
					else
					{
						ball.SetSuckCount(0, true);
					}
				}
				else
				{
					if (num == 0)
					{
						break;
					}
					num--;
				}
			}
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x000568C8 File Offset: 0x00054AC8
		public void AddInkSpots(int num, float minrad, float maxrad, float minfade, float maxfade, int fadedelay, int target_mode)
		{
			List<float> list = new List<float>();
			if (target_mode == 1)
			{
				foreach (Ball ball in this.mBallList)
				{
					list.Add(ball.GetWayPoint());
				}
			}
			for (int i = 0; i < num; i++)
			{
				InkBlot inkBlot = new InkBlot();
				inkBlot.mAlpha = 255f;
				inkBlot.mAlphaDec = MathUtils.FloatRange(minfade, maxfade);
				inkBlot.mRadius = ZumasRevenge.Common._DS(MathUtils.FloatRange(minrad, maxrad));
				inkBlot.mDelay = MathUtils.SafeRand() % ZumasRevenge.Common._M(100);
				inkBlot.mFadeDelayTimer = fadedelay;
				inkBlot.mAngle = MathUtils.DegreesToRadians((float)(MathUtils.SafeRand() % 360));
				if (target_mode != 2)
				{
					float num2;
					if (target_mode != 1 || Enumerable.Count<float>(list) == 0)
					{
						num2 = (float)(ZumasRevenge.Common._M(100) + MathUtils.SafeRand() % (this.mWayPointMgr.GetNumPoints() - ZumasRevenge.Common._M1(300)));
					}
					else
					{
						int num3 = MathUtils.SafeRand() % Enumerable.Count<float>(list);
						num2 = list[num3];
						list.RemoveAt(num3);
					}
					this.GetXYFromWaypoint((int)num2, out inkBlot.mX, out inkBlot.mY);
				}
				else
				{
					inkBlot.mX = inkBlot.mRadius + (float)(MathUtils.SafeRand() % (int)((float)GlobalMembers.gSexyApp.mWidth - inkBlot.mRadius));
					inkBlot.mY = inkBlot.mRadius + (float)(MathUtils.SafeRand() % (int)((float)GlobalMembers.gSexyApp.mHeight - inkBlot.mRadius));
				}
				this.mInkSpots.Add(inkBlot);
			}
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x00056A70 File Offset: 0x00054C70
		public void QuicklyFadeInkSpots()
		{
			for (int i = 0; i < this.mInkSpots.Count; i++)
			{
				InkBlot inkBlot = this.mInkSpots[i];
				inkBlot.mDelay = (inkBlot.mFadeDelayTimer = 0);
				inkBlot.mAlphaDec *= (float)ZumasRevenge.Common._M(4);
			}
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x00056AC4 File Offset: 0x00054CC4
		public void PowerupExpired(PowerType p)
		{
			this.mLastCompletedPowerUpFrame[(int)p] = this.mBoard.GetStateCount();
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x00056ADC File Offset: 0x00054CDC
		public void SetColorHasPowerup(int c, bool val)
		{
			if (val)
			{
				this.mBallColorHasPowerup[c]++;
				return;
			}
			if (--this.mBallColorHasPowerup[c] < 0)
			{
				this.mBallColorHasPowerup[c] = 0;
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00056B30 File Offset: 0x00054D30
		public void UpdateSets()
		{
			this.mHaveSets = false;
			int i = 0;
			while (i < Enumerable.Count<Ball>(this.mBallList))
			{
				Ball ball = this.mBallList[i];
				bool isExploding = ball.GetIsExploding();
				if (isExploding)
				{
					this.mHaveSets = true;
				}
				if (ball.GetShouldRemove())
				{
					Ball nextBall = ball.GetNextBall();
					Ball prevBall = ball.GetPrevBall();
					if (nextBall != null && !nextBall.GetIsExploding() && prevBall != null && !prevBall.GetShouldRemove() && nextBall.GetColorType() == prevBall.GetColorType())
					{
						nextBall.SetSuckCount(10);
						nextBall.SetComboCount(ball.GetComboCount() + 1, ball.GetComboScore());
						if (this.mLevel.mZone == 5)
						{
							this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_UNDERWATER_ROLL));
						}
					}
					if (i == 0)
					{
						this.mAdvanceSpeed = 0f;
						if (this.mStopTime < 40)
						{
							this.mStopTime = 40;
						}
					}
					this.DeleteBall(ball);
					this.mBallList.RemoveAt(i);
				}
				else
				{
					if (isExploding)
					{
						ball.UpdateExplosion();
					}
					else
					{
						this.mBoard.mBallColorMap[ball.GetColorType()]++;
					}
					i++;
				}
			}
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00056C64 File Offset: 0x00054E64
		public void UpdatePowerUps()
		{
			if (this.mBallList.Count == 0 || this.mBoard.mPreventBallAdvancement)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < this.mNumMultBallsToSpawn; i++)
			{
				Ball ball = this.AddPowerUp(PowerType.PowerType_GauntletMultBall);
				if (ball != null)
				{
					flag = true;
					this.mNumMultBallsToSpawn--;
					this.mBoard.MultiplierBallAdded(ball);
				}
			}
			if (flag)
			{
				return;
			}
			int stateCount = this.mBoard.GetStateCount();
			if (stateCount < this.mApp.GetLevelMgr().mPowerDelay)
			{
				return;
			}
			int mPowerupSpawnDelay = this.mApp.GetLevelMgr().mPowerupSpawnDelay;
			if (mPowerupSpawnDelay > 0 && stateCount - this.mLastPowerupTime < mPowerupSpawnDelay)
			{
				return;
			}
			int num = GameApp.gDDS.GetOverallPowerupChance(this.mCurveNum);
			if (num == 0)
			{
				return;
			}
			num -= (int)((float)num * this.mLevel.GetPowerIncPct());
			if (num <= 0)
			{
				num = 1;
			}
			if (this.mApp.GetLevelMgr().mUniquePowerupColor)
			{
				int num2 = 0;
				for (int j = 0; j < this.mCurveDesc.mVals.mNumColors; j++)
				{
					if (this.mBallColorHasPowerup[j] > 0)
					{
						num2++;
					}
				}
				if (num2 == this.mCurveDesc.mVals.mNumColors)
				{
					return;
				}
			}
			if (MathUtils.SafeRand() % num == 0)
			{
				int num3 = 0;
				for (int k = 0; k < 14; k++)
				{
					if (this.CanSpawnPowerUp((PowerType)k))
					{
						num3 += GameApp.gDDS.GetPowerFreq(k, this.mCurveNum);
					}
				}
				if (num3 == 0)
				{
					return;
				}
				int num4 = MathUtils.SafeRand() % num3;
				int num5 = 0;
				for (int l = 0; l < 14; l++)
				{
					if (this.CanSpawnPowerUp((PowerType)l))
					{
						int powerFreq = GameApp.gDDS.GetPowerFreq(l, this.mCurveNum);
						if (num4 < num5 + powerFreq)
						{
							int num6 = this.mLastSpawnedPowerUpFrame[l];
							bool flag2 = l == 0 && this.mLevel.GetBossBombDelay() > 0;
							if ((l != 8 || !this.mLevel.HasPowerup((PowerType)l)) && (!this.HasPowerup((PowerType)l) || flag2))
							{
								int num7 = this.mApp.GetLevelMgr().mPowerCooldown;
								if (flag2)
								{
									num7 = this.mLevel.GetBossBombDelay();
								}
								if (stateCount - num6 < num7)
								{
									return;
								}
								if (stateCount - this.mLastCompletedPowerUpFrame[l] < num7 && this.mLastCompletedPowerUpFrame[l] > 0)
								{
									return;
								}
								if (!this.mBoard.GetGun().CanSpawnPowerUp((PowerType)l))
								{
									return;
								}
								this.AddPowerUp((PowerType)l);
								this.mBoard.GetBetaStats().SpawnedPowerup(l);
								this.mLastPowerupTime = stateCount;
								this.mLastSpawnedPowerUpFrame[l] = stateCount;
								this.mNumPowerUpsThisLevel[l]++;
								return;
							}
						}
						else
						{
							num5 += powerFreq;
						}
					}
				}
			}
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x00056F2C File Offset: 0x0005512C
		public void RemoveBallsAtEnd()
		{
			if (ZumasRevenge.Common.gDieAtEnd || this.mLevel.mLoopAtEnd)
			{
				return;
			}
			if (Enumerable.Count<Ball>(this.mBallList) == 0)
			{
				return;
			}
			int endPoint = this.mWayPointMgr.GetEndPoint();
			int num = this.mBallList.Count;
			num--;
			bool flag = false;
			while (!flag)
			{
				Ball ball = this.mBallList[num];
				if (ball.GetWayPoint() < (float)endPoint)
				{
					break;
				}
				if (!this.mLevel.mLoopAtEnd || ball.GetIsExploding())
				{
					if (num != 0)
					{
						num--;
					}
					else
					{
						flag = true;
					}
					this.DeleteBullet(ball.GetBullet());
					ball.RemoveFromList();
					this.DeleteBall(ball);
				}
			}
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x00056FD0 File Offset: 0x000551D0
		public void RemoveBallsAtFront()
		{
			if (!this.mHasReachedCruisingSpeed || this.mBoard.GauntletMode())
			{
				return;
			}
			int i = 0;
			while (i < Enumerable.Count<Ball>(this.mBallList))
			{
				Ball ball = this.mBallList[i];
				if ((ball.GetWayPoint() >= (float)this.mCurveDesc.mCutoffPoint || !this.mStopAddingBalls) && ball.GetWayPoint() >= 1f)
				{
					break;
				}
				i++;
				this.DeleteBullet(ball.GetBullet());
				ball.RemoveFromList();
				if (!ball.GetIsExploding() && !this.mStopAddingBalls)
				{
					this.mPendingBalls.Add(ball);
				}
				else
				{
					if (this.mStopAddingBalls)
					{
						this.mBoard.IncScore(10, true);
					}
					this.DeleteBall(ball);
					if (Enumerable.Count<Ball>(this.mBallList) == 0 && this.mStopAddingBalls)
					{
						this.mLastClearedBallPoint = 0;
					}
				}
			}
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x000570B0 File Offset: 0x000552B0
		public void AdvanceMergingBullet(ref int theBulletItr)
		{
			Bullet bullet = this.mBulletList[theBulletItr];
			this.DoMerge(bullet);
			if ((double)bullet.GetHitPercent() >= 1.0)
			{
				Ball hitBall = bullet.GetHitBall();
				int num = hitBall.GetListItr();
				bool hitInFront = bullet.GetHitInFront();
				if (hitInFront)
				{
					num++;
				}
				Ball ball = new Ball();
				ball.SetRotation(bullet.GetRotation());
				ball.SetColorType(bullet.GetColorType());
				ball.SetPowerType(bullet.GetPowerType(), false);
				this.mWayPointMgr.SetWayPoint(ball, bullet.GetWayPoint(), this.mLevel.mLoopAtEnd);
				ball.ForceFrame(-ball.GetFrame(Res.GetImageByID(ResID.IMAGE_BLUE_BALL)));
				ball.InsertInList(this.mBallList, num, this);
				int num2 = bullet.GetMinGapDist();
				int numGaps = bullet.GetNumGaps();
				bullet.Dispose();
				this.mBulletList.RemoveAt(theBulletItr++);
				this.mTotalBalls++;
				Ball prevBall = ball.GetPrevBall();
				Ball nextBall = ball.GetNextBall();
				ball.UpdateCollisionInfo(5);
				ball.SetNeedCheckCollision(true);
				if (prevBall != null && ball.GetCollidesWithPrev())
				{
					prevBall.SetNeedCheckCollision(true);
				}
				if (num2 > 0)
				{
					num2 -= 64;
					if (num2 < 0)
					{
						num2 = 0;
					}
					int num3 = (this.mBoard.IsEndless() ? 250 : 500);
					int num4 = num3 * (CurveMgr.MAX_GAP_SIZE - num2) / CurveMgr.MAX_GAP_SIZE;
					num4 = num4 / 10 * 10;
					if (num4 < 10)
					{
						num4 = 10;
					}
					if (num4 > 0)
					{
						if (numGaps > 1)
						{
							num4 *= numGaps;
						}
						ball.SetGapBonus(num4, numGaps);
					}
				}
				if (this.CheckSet(ball))
				{
					this.mBoard.IncNumClearsInARow(1);
					return;
				}
				if (prevBall != null && !prevBall.GetCollidesWithNext() && prevBall.GetColorType() == ball.GetColorType() && prevBall.GetBullet() == null && !prevBall.GetIsExploding())
				{
					ball.SetSuckPending(true);
					ball.SetSuckCount(1);
					return;
				}
				if (nextBall == null || ball.GetCollidesWithNext() || nextBall.GetColorType() != ball.GetColorType() || nextBall.GetBullet() != null || nextBall.GetIsExploding())
				{
					this.mBoard.ResetInARowBonus();
					ball.SetGapBonus(0, 0);
					return;
				}
				ball.SetSuckPending(true);
				if (nextBall.GetSuckCount() <= 0)
				{
					nextBall.SetSuckCount(1);
					return;
				}
			}
			else
			{
				this.mBoard.mBallColorMap[bullet.GetColorType()]++;
				theBulletItr++;
			}
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x00057340 File Offset: 0x00055540
		public void DoMerge(Bullet theBullet)
		{
			theBullet.CheckSetHitBallToPrevBall();
			Ball hitBall = theBullet.GetHitBall();
			float rotation = theBullet.GetRotation();
			this.mWayPointMgr.SetWayPoint(theBullet, hitBall.GetWayPoint(), this.mLevel.mLoopAtEnd);
			this.mWayPointMgr.FindFreeWayPoint(hitBall, theBullet, theBullet.GetHitInFront(), this.mLevel.mLoopAtEnd);
			theBullet.SetDestPos(theBullet.GetX(), theBullet.GetY());
			theBullet.SetRotation(rotation, true);
			theBullet.SetRotation(this.mWayPointMgr.GetRotationForPoint((int)theBullet.GetWayPoint()), false);
			theBullet.Update();
			Ball pushBall = theBullet.GetPushBall();
			if (pushBall != null)
			{
				float num = 1f - theBullet.GetHitPercent();
				int thePad = (int)((float)(-(float)theBullet.GetRadius()) * num / 2f);
				float wayPoint = pushBall.GetWayPoint();
				float num2 = theBullet.GetHitPercent() * theBullet.GetHitPercent() * (float)(pushBall.GetRadius() + theBullet.GetRadius());
				this.mWayPointMgr.FindFreeWayPoint(theBullet, theBullet.GetPushBall(), true, this.mLevel.mLoopAtEnd, thePad);
				if (pushBall.GetWayPoint() - theBullet.GetWayPoint() > num2)
				{
					float num3 = theBullet.GetWayPoint() + num2;
					if (num3 > wayPoint)
					{
						this.mWayPointMgr.SetWayPoint(pushBall, num3, this.mLevel.mLoopAtEnd);
					}
					else
					{
						this.mWayPointMgr.SetWayPoint(pushBall, wayPoint, this.mLevel.mLoopAtEnd);
					}
				}
				pushBall.SetNeedCheckCollision(true);
				if (!CurveMgr.gStopSuckbackImmediately)
				{
					Ball nextBall = hitBall.GetNextBall();
					bool hitInFront = theBullet.GetHitInFront();
					if (hitInFront && nextBall != null && nextBall.GetSuckBack() && nextBall.GetSuckCount() > 0 && nextBall.GetColorType() != theBullet.GetColorType())
					{
						nextBall.SetSuckCount(0);
						return;
					}
					if (!hitInFront && hitBall != null && hitBall.GetSuckBack() && hitBall.GetSuckCount() > 0 && hitBall.GetColorType() != theBullet.GetColorType())
					{
						hitBall.SetSuckCount(0);
					}
				}
			}
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00057520 File Offset: 0x00055720
		public void AdvanceBullets()
		{
			int i = 0;
			while (i < this.mBulletList.Count)
			{
				this.AdvanceMergingBullet(ref i);
			}
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x00057548 File Offset: 0x00055748
		public void StartExploding(Ball theBall, bool from_lightning_frog, bool record_levelstats)
		{
			if (theBall.GetIsExploding())
			{
				return;
			}
			if (record_levelstats)
			{
				GameStats levelStats = this.mBoard.GetLevelStats();
				levelStats.mNumBallsCleared++;
				this.mBoard.IncNumCleared(1);
				this.mLevel.BallExploded(theBall.GetColorType());
			}
			this.mLastClearedBallPoint = (int)theBall.GetWayPoint();
			if (theBall.GetSuckPending())
			{
				theBall.SetSuckPending(false);
			}
			theBall.Explode(this.mWayPointMgr.InTunnel((int)theBall.GetWayPoint()), from_lightning_frog);
			if (theBall.GetPowerOrDestType() != PowerType.PowerType_Max)
			{
				this.mNumPowerupsActivated[(int)theBall.GetPowerOrDestType()]++;
				this.mBoard.ActivatePower(theBall);
				this.mLastCompletedPowerUpFrame[(int)theBall.GetPowerOrDestType()] = this.mBoard.GetStateCount();
				this.mHadPowerUp = true;
			}
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x00057620 File Offset: 0x00055820
		public void StartExploding(Ball theBall)
		{
			this.StartExploding(theBall, false, true);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0005762C File Offset: 0x0005582C
		public void ActivateFrontBomb()
		{
			int num = 0;
			int num2 = this.mBallList.Count - 1;
			while (num2 >= 0 && num < 7)
			{
				num++;
				Ball ball = this.mBallList[num2];
				if (!ball.GetIsExploding())
				{
					ball.SetComboCount(this.mBoard.GetCurComboCount(), this.mBoard.GetCurComboScore());
					this.mBoard.mNeedComboCount.Add(ball);
					this.StartExploding(ball);
				}
				num2--;
			}
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x000576A4 File Offset: 0x000558A4
		public void ClearPendingSucks(Ball theEndBall)
		{
			Ball ball = theEndBall;
			bool flag = true;
			while (ball != null)
			{
				if (ball.GetSuckPending())
				{
					ball.SetSuckPending(false);
					this.mBoard.ResetInARowBonus();
					ball.SetGapBonus(0, 0);
				}
				ball = ball.GetPrevBall();
				if (ball == null)
				{
					return;
				}
				if (!ball.GetCollidesWithNext())
				{
					flag = false;
				}
				if (!flag && ball.GetSuckCount() > 0)
				{
					return;
				}
			}
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x000576FE File Offset: 0x000558FE
		public void ChangeBallColors(Ball theBall)
		{
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x00057700 File Offset: 0x00055900
		public void DestroyBallsInRegion(int x, int y, int width, int height)
		{
			foreach (Ball ball in this.mBallList)
			{
				if (!ball.GetIsExploding())
				{
					int radius = ball.GetRadius();
					int num = (int)ball.GetX() - radius;
					int num2 = (int)ball.GetY() - radius;
					if (num + radius >= x && num <= x + width && num2 + radius >= y && num2 <= y + height)
					{
						this.StartExploding(ball, false, false);
					}
				}
			}
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00057794 File Offset: 0x00055994
		public void CompactCurve(bool suck_back)
		{
			foreach (Ball ball in this.mBallList)
			{
				if (!ball.GetIsExploding())
				{
					Ball nextBall = ball.GetNextBall();
					if (nextBall != null && !ball.GetCollidesWithNext() && nextBall.GetBullet() == null && !nextBall.GetIsExploding())
					{
						Ball ball2;
						if (suck_back)
						{
							ball2 = nextBall;
						}
						else
						{
							ball2 = ball;
						}
						ball2.SetSuckPending(true, true);
						ball2.SetSuckCount(1, suck_back);
					}
					else if (ball.GetSuckCount() > 0)
					{
						ball.SetSuckCount(0);
					}
				}
			}
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00057838 File Offset: 0x00055A38
		public void CompactCurve()
		{
			this.CompactCurve(true);
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00057844 File Offset: 0x00055A44
		public void RollBallsIn(bool from_load)
		{
			this.mHasReachedCruisingSpeed = false;
			float num = GameApp.gDDS.GetSpeed(this.mCurveNum) * this.mSpeedScale;
			int num2 = (this.mBoard.IsEndless() ? 50 : GameApp.gDDS.GetStartDistance(this.mCurveNum));
			float num3 = (float)(this.GetCurveLength() * num2 / 100);
			if (this.mFirstChainEnd > 0)
			{
				num3 -= (float)this.mFirstChainEnd / (float)this.GetCurveLength();
				if (num3 <= 0f)
				{
					this.mAdvanceSpeed = GameApp.gDDS.GetSpeed(this.mCurveNum) * this.mSpeedScale;
					return;
				}
			}
			float num4 = 20f * num + 1f;
			float num5 = -20f * num3;
			int num6 = (int)(((double)(-(double)num4) + Math.Sqrt((double)(num4 * num4 - 4f * num5))) / 2.0);
			this.mAdvanceSpeed = num + (float)num6 * 0.1f;
			if (!ZumasRevenge.Common.gAddBalls)
			{
				this.mHasReachedCruisingSpeed = true;
				this.mAdvanceSpeed = GameApp.gDDS.GetSpeed(this.mCurveNum) * this.mSpeedScale;
			}
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00057956 File Offset: 0x00055B56
		public void RollBallsIn()
		{
			this.RollBallsIn(false);
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00057960 File Offset: 0x00055B60
		public CurveMgr(Board theBoard, int curve_num)
		{
			this.mApp = GameApp.gApp;
			this.mBoard = theBoard;
			this.mCurveNum = curve_num;
			this.mCurveDesc = new CurveDesc();
			this.mWayPointMgr = new WayPointMgr();
			this.mIsLoaded = false;
			this.mPostZumaFlashTimer = 0;
			this.Reset();
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00057A44 File Offset: 0x00055C44
		public virtual void Dispose()
		{
			this.DeleteBalls();
			this.mWayPointMgr = null;
			this.mCurveDesc = null;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00057A5C File Offset: 0x00055C5C
		public void Copy(CurveMgr src, Level l, Board b)
		{
			this.mCurveDesc = null;
			this.mWayPointMgr = null;
			this.mBoard = b;
			this.mWayPointMgr = ((src.mWayPointMgr != null) ? new WayPointMgr(src.mWayPointMgr) : null);
			this.mCurveDesc = ((src.mCurveDesc != null) ? new CurveDesc(src.mCurveDesc) : null);
			this.mLevel = l;
			this.mBulletList.Clear();
			this.mBallList.Clear();
			this.mPendingBalls.Clear();
			this.mIsLoaded = src.mIsLoaded;
			this.mApp = src.mApp;
			this.mSpeedScale = src.mSpeedScale;
			this.mLastScoreColor = src.mLastScoreColor;
			this.mSparkles.Clear();
			this.mSparkles.AddRange(src.mSparkles.ToArray());
			this.mWarningLights.Clear();
			this.mWarningLights.AddRange(src.mWarningLights.ToArray());
			this.mPostZumaFlashTimer = src.mPostZumaFlashTimer;
			this.mLastPowerupTime = src.mLastPowerupTime;
			Array.Copy(src.mLastSpawnedPowerUpFrame, this.mLastSpawnedPowerUpFrame, src.mLastSpawnedPowerUpFrame.Length);
			Array.Copy(src.mLastCompletedPowerUpFrame, this.mLastCompletedPowerUpFrame, src.mLastCompletedPowerUpFrame.Length);
			Array.Copy(src.mNumPowerUpsThisLevel, this.mNumPowerUpsThisLevel, src.mNumPowerUpsThisLevel.Length);
			Array.Copy(src.mNumPowerupsActivated, this.mNumPowerupsActivated, src.mNumPowerupsActivated.Length);
			Array.Copy(src.mBallColorHasPowerup, this.mBallColorHasPowerup, src.mBallColorHasPowerup.Length);
			this.mNumBallsCreated = src.mNumBallsCreated;
			this.mCurveNum = src.mCurveNum;
			this.mStopTime = src.mStopTime;
			this.mProxBombCounter = src.mProxBombCounter;
			this.mSlowCount = src.mSlowCount;
			this.mBackwardCount = src.mBackwardCount;
			this.mTotalBalls = src.mTotalBalls;
			this.mAdvanceSpeed = src.mAdvanceSpeed;
			this.mSkullHilite = src.mSkullHilite;
			this.mSkullHiliteDir = src.mSkullHiliteDir;
			this.mFirstChainEnd = src.mFirstChainEnd;
			this.mFirstBallMovedBackwards = src.mFirstBallMovedBackwards;
			this.mHaveSets = src.mHaveSets;
			this.mDoingClearCurveRollout = src.mDoingClearCurveRollout;
			this.mInitialPathHilite = src.mInitialPathHilite;
			this.mLastPathHiliteWP = src.mLastPathHiliteWP;
			this.mLastPathHilitePitch = src.mLastPathHilitePitch;
			this.mDangerPoint = src.mDangerPoint;
			this.mPathLightEndFrame = src.mPathLightEndFrame;
			this.mLastClearedBallPoint = src.mLastClearedBallPoint;
			this.mOverrideSpeed = src.mOverrideSpeed;
			this.mHadPowerUp = src.mHadPowerUp;
			this.mStopAddingBalls = src.mStopAddingBalls;
			this.mInDanger = src.mInDanger;
			this.mHasReachedCruisingSpeed = src.mHasReachedCruisingSpeed;
			this.mHasReachedRolloutPoint = src.mHasReachedRolloutPoint;
			this.mNeedsSpeedup = src.mNeedsSpeedup;
			this.mCanCheckForSpeedup = src.mCanCheckForSpeedup;
			this.mLastPathShowTick = src.mLastPathShowTick;
			this.mNumMultBallsToSpawn = src.mNumMultBallsToSpawn;
			this.mInkSpots.Clear();
			this.mInkSpots.AddRange(src.mInkSpots.ToArray());
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00057D70 File Offset: 0x00055F70
		public void CopyCurveDataFrom(CurveMgr src)
		{
			string mPath = this.mCurveDesc.mPath;
			this.mCurveDesc = new CurveDesc(src.mCurveDesc);
			this.mCurveDesc.mPath = mPath;
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00057DA8 File Offset: 0x00055FA8
		public void SetLosing()
		{
			this.mBulletList.Clear();
			foreach (Ball ball in this.mBallList)
			{
				ball.CleanUpMultiplierOverlays();
				ball.SetSuckCount((int)this.mAdvanceSpeed * 4, true);
			}
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00057E18 File Offset: 0x00056018
		public bool LoadCurve(MirrorType theMirror)
		{
			this.mSpeedScale = 1f;
			float num = 1f;
			float num2 = 1f;
			float num3 = 0.5f;
			float num4 = 0.67f;
			float num5 = (num4 - num2) / (num3 - num);
			if (this.mSpeedScale < 1.25f)
			{
				this.mSpeedScale = num5 * this.mSpeedScale - num5 * num + num2;
			}
			string curvePath = this.mLevel.GetCurvePath(this.mCurveNum);
			if (!this.mWayPointMgr.LoadCurve(curvePath, this.mCurveDesc, theMirror))
			{
				return false;
			}
			List<WayPoint> wayPointList = this.mWayPointMgr.GetWayPointList();
			int x = 0;
			int y = 0;
			float num6 = (float)this.mCurveDesc.mVals.mSkullRotation;
			if (num6 >= 0f)
			{
				num6 = SexyMath.DegToRad(num6);
			}
			if (wayPointList.Count > 0)
			{
				this.mWayPointMgr.CalcPerpendicularForPoint(wayPointList.Count - 1);
				WayPoint wayPoint = Enumerable.Last<WayPoint>(wayPointList);
				x = (int)wayPoint.x;
				y = (int)wayPoint.y;
				if (num6 < 0f)
				{
					num6 = wayPoint.mRotation;
				}
			}
			this.mLevel.mHoleMgr.PlaceHole(this.mCurveNum, x, y, num6, this.mCurveDesc.mVals.mDrawPit);
			this.mDangerPoint = this.mWayPointMgr.GetNumPoints() - this.mCurveDesc.mDangerDistance;
			if (this.mDangerPoint >= this.mWayPointMgr.GetNumPoints())
			{
				this.mDangerPoint = this.mWayPointMgr.GetNumPoints() - 1;
			}
			int num7 = ZumasRevenge.Common._M(0);
			for (int i = (int)((float)this.mDangerPoint * this.mLevel.mPotPct); i <= this.mWayPointMgr.GetEndPoint() - ZumasRevenge.Common._M(50); i += ZumasRevenge.Common._M1(50))
			{
				SexyVector2 pointPos = this.mWayPointMgr.GetPointPos((float)i);
				SexyVector3 impliedObject = new SexyVector3(pointPos.x, pointPos.y, 0f);
				SexyVector3 v = this.mWayPointMgr.CalcPerpendicular((float)i) * (float)num7;
				SexyVector3 sexyVector = impliedObject + v;
				WarningLight warningLight = new WarningLight(sexyVector.x, sexyVector.y);
				this.mWarningLights.Add(warningLight);
				warningLight.mAngle = this.mWayPointMgr.GetRotationForPoint(i);
				warningLight.mWaypoint = (float)i;
				warningLight.mPriority = this.mWayPointMgr.GetPriority(i);
			}
			this.mIsLoaded = true;
			return true;
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00058086 File Offset: 0x00056286
		public bool LoadCurve()
		{
			return this.LoadCurve(MirrorType.MirrorType_None);
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x00058090 File Offset: 0x00056290
		public void StartLevel(bool from_load)
		{
			if ((!from_load || this.mProxBombCounter <= 0) && this.mLevel.mBoss != null && this.mLevel.mBoss.mBombFreqMin > 0)
			{
				this.mProxBombCounter = MathUtils.IntRange(this.mLevel.mBoss.mBombFreqMin, this.mLevel.mBoss.mBombFreqMax);
			}
			this.mPathLightEndFrame = 0;
			this.mLastPathShowTick = Math.Min(0U, ZumasRevenge.Common.GetBoardTickCount() - 1000000U);
			this.mLastClearedBallPoint = 0;
			for (int i = 0; i < 14; i++)
			{
				this.mLastSpawnedPowerUpFrame[i] = this.mBoard.GetStateCount() - 1000;
				this.mLastCompletedPowerUpFrame[i] = this.mBoard.GetStateCount() - 1000;
			}
			this.DeleteBalls();
			int num = ((this.mCurveDesc.mVals.mNumBalls > 0 && this.mCurveDesc.mVals.mNumBalls < 10) ? this.mCurveDesc.mVals.mNumBalls : 10);
			for (int i = 0; i < num; i++)
			{
				this.AddPendingBall();
			}
			this.mStopTime = 0;
			this.mSlowCount = 0;
			this.mBackwardCount = 0;
			this.mTotalBalls = num;
			this.mNumBallsCreated = this.mPendingBalls.Count;
			this.mStopAddingBalls = false;
			if (this.mCurveDesc.mVals.mNumBalls > 0 && this.mNumBallsCreated >= this.mCurveDesc.mVals.mNumBalls)
			{
				this.mStopAddingBalls = true;
			}
			this.mInDanger = false;
			this.mFirstChainEnd = 0;
			this.mFirstBallMovedBackwards = false;
			ZumasRevenge.Common.gDieAtEnd = !this.mLevel.mLoopAtEnd && this.mCurveDesc.mVals.mDieAtEnd;
			this.RollBallsIn(from_load);
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x00058253 File Offset: 0x00056453
		public void StartLevel()
		{
			this.StartLevel(false);
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0005825C File Offset: 0x0005645C
		public bool UpdatePlaying()
		{
			if (this.mStopAddingBalls && this.mPostZumaFlashTimer > 0)
			{
				this.mPostZumaFlashTimer--;
			}
			bool result = false;
			int num = ((this.mBallList.Count == 0) ? 0 : ((int)Enumerable.Last<Ball>(this.mBallList).GetWayPoint()));
			bool flag = this.mBallList.Count == 0 || num < this.mCurveDesc.mCutoffPoint;
			if (this.mStopTime > 0)
			{
				this.mStopTime--;
				if (flag)
				{
					this.mStopTime = 0;
				}
				if (this.mStopTime == 0)
				{
					this.mAdvanceSpeed = 0f;
				}
			}
			for (int i = 0; i < this.mInkSpots.Count; i++)
			{
				InkBlot inkBlot = this.mInkSpots[i];
				if (inkBlot.mDelay > 0)
				{
					inkBlot.mDelay--;
				}
				else if (inkBlot.mFadeDelayTimer > 0)
				{
					inkBlot.mFadeDelayTimer--;
				}
				else
				{
					inkBlot.mAlpha -= inkBlot.mAlphaDec;
					if (inkBlot.mAlpha <= 0f)
					{
						this.mInkSpots.RemoveAt(i);
						i--;
					}
				}
			}
			if (this.mInitialPathHilite && !this.mBoard.mPreventBallAdvancement && this.mLastPathHiliteWP < this.mWayPointMgr.GetNumPoints() && this.mSkullHiliteDir == 0f)
			{
				PathSparkle pathSparkle = new PathSparkle();
				this.mSparkles.Add(pathSparkle);
				this.GetPoint(this.mLastPathHiliteWP, out pathSparkle.mX, out pathSparkle.mY, out pathSparkle.mPri);
				this.mLastPathHiliteWP += ZumasRevenge.Common._M(10);
				if (this.mLastPathHiliteWP >= this.mWayPointMgr.GetNumPoints())
				{
					result = true;
					this.mSkullHiliteDir = ZumasRevenge.Common._M(12f);
				}
				if (this.mBoard.mUpdateCnt % ZumasRevenge.Common._M(25) == 0)
				{
					if (this.mCurveNum != 1 && this.mLastPathHilitePitch > -20)
					{
						this.mLastPathHilitePitch--;
					}
					else if (this.mCurveNum == 1 && this.mLastPathHilitePitch < 0)
					{
						this.mLastPathHilitePitch++;
					}
					SoundAttribs soundAttribs = new SoundAttribs();
					soundAttribs.pitch = (float)this.mLastPathHilitePitch;
					this.mApp.mSoundPlayer.Play(Res.GetSoundByID(ResID.SOUND_TRAIL_LIGHT), soundAttribs);
				}
				if (this.mLastPathHiliteWP >= this.mWayPointMgr.GetNumPoints())
				{
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_TRAIL_LIGHT_END));
				}
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_PATH_SPARKLES);
			for (int j = 0; j < this.mSparkles.Count; j++)
			{
				PathSparkle pathSparkle2 = this.mSparkles[j];
				pathSparkle2.mUpdateCount++;
				if (pathSparkle2.mUpdateCount % ZumasRevenge.Common._M(3) == 0 && ++pathSparkle2.mCel >= imageByID.mNumCols)
				{
					this.mSparkles.RemoveAt(j);
					j--;
				}
			}
			this.mSkullHilite += this.mSkullHiliteDir;
			if (this.mSkullHiliteDir > 0f && this.mSkullHilite >= 255f)
			{
				this.mSkullHilite = 255f;
				this.mSkullHiliteDir *= -1f;
			}
			else if (this.mSkullHiliteDir < 0f && this.mSkullHilite <= 0f)
			{
				this.mSkullHilite = (this.mSkullHiliteDir = 0f);
			}
			bool flag2 = false;
			for (int k = 0; k < this.mWarningLights.Count; k++)
			{
				if (flag2)
				{
					this.mWarningLights[k].mPulseRate = -this.mWarningLights[k - 1].mPulseRate;
				}
				flag2 = this.mWarningLights[k].Update();
				if (this.mInitialPathHilite)
				{
					this.mWarningLights[k].mPulseAlpha -= (float)ZumasRevenge.Common._M(5);
					if (this.mWarningLights[k].mPulseAlpha < 0f)
					{
						this.mWarningLights[k].mPulseAlpha = 0f;
					}
				}
			}
			if (this.mInitialPathHilite)
			{
				return result;
			}
			if (this.mSlowCount > 0)
			{
				this.mSlowCount--;
				if (flag && !this.mStopAddingBalls)
				{
					this.mSlowCount = 0;
				}
			}
			if (this.mBackwardCount > 0)
			{
				this.mBackwardCount--;
				if (flag && !this.mStopAddingBalls)
				{
					this.mBackwardCount = 0;
				}
			}
			if (ZumasRevenge.Common.gAddBalls)
			{
				this.AddBall();
			}
			this.UpdateBalls();
			this.AdvanceBullets();
			this.UpdateSuckingBalls();
			if (!this.mDoingClearCurveRollout && !this.mLevel.IsFinalBossLevel() && !this.mBoard.HasAchievedZuma() && !this.mLevel.mIsEndless && this.mLevel.mBoss == null && this.mHasReachedCruisingSpeed && !this.mInitialPathHilite && !this.mBoard.mPreventBallAdvancement)
			{
				bool flag3 = this.mBallList.Count == 0;
				if (!flag3)
				{
					Ball ball = null;
					for (int l = this.mBallList.Count - 1; l >= 0; l--)
					{
						if (!this.mBallList[l].GetIsExploding())
						{
							ball = this.mBallList[l];
							break;
						}
					}
					if (ball != null)
					{
						int num2 = (int)(ball.GetWayPoint() + (float)ball.GetRadius());
						float num3;
						float num4;
						if (this.GetXYFromWaypoint(num2, out num3, out num4))
						{
							Rect rect = new Rect(0, 0, ZumasRevenge.Common._SS(this.mApp.mWidth), ZumasRevenge.Common._SS(this.mApp.mHeight));
							if (!rect.Contains((int)num3, (int)num4))
							{
								flag3 = true;
							}
							else if (!this.mLevel.mOffscreenClearBonus)
							{
								int num5 = -1;
								List<WayPoint> wayPointList = this.mWayPointMgr.GetWayPointList();
								int num6 = 0;
								while (num6 < wayPointList.Count && wayPointList[num6].mInTunnel)
								{
									num5 = num6;
									num6++;
								}
								if (num5 != -1 && num2 <= num5)
								{
									flag3 = true;
								}
							}
						}
					}
					else
					{
						flag3 = true;
					}
				}
				if (flag3)
				{
					this.mDoingClearCurveRollout = true;
					this.mBoard.DoClearCurveBonus(this.mCurveNum);
					this.mSlowCount = (this.mBackwardCount = 0);
					this.mAdvanceSpeed = GameApp.gDDS.GetSpeed(this.mCurveNum) * this.mSpeedScale * this.mApp.GetLevelMgr().mClearCurveSpeedMult;
				}
			}
			this.AdvanceBalls();
			this.AdvanceBackwardBalls();
			this.RemoveBallsAtFront();
			this.RemoveBallsAtEnd();
			this.UpdateSets();
			this.UpdatePowerUps();
			if (!this.mHasReachedRolloutPoint && !this.mBoard.DisplayingTip() && this.mBoard.GetGameState() == GameState.GameState_Playing && this.mBallList.Count > 0 && !this.mHasReachedCruisingSpeed && this.mLevel.mZone == 5 && this.mLevel.mNum != 10 && this.mBoard.mUpdateCnt % ZumasRevenge.Common._M(2) == 0)
			{
				Ball ball2 = Enumerable.Last<Ball>(this.mBallList);
				Bubble bubble = new Bubble();
				bubble.Init(0f, MathUtils.FloatRange(ZumasRevenge.Common._M(-0.75f), ZumasRevenge.Common._M1(-0.5f)), MathUtils.FloatRange(ZumasRevenge.Common._M2(0.05f), ZumasRevenge.Common._M3(0.2f)), (int)MathUtils.FloatRange((float)ZumasRevenge.Common._M4(15), (float)ZumasRevenge.Common._M5(25)));
				bubble.SetAlphaFade(ZumasRevenge.Common._M(1f));
				bubble.SetX(ball2.GetX());
				bubble.SetY(ball2.GetY());
				this.mLevel.mFrog.AddBubble(bubble);
			}
			if (this.mBallList.Count > 0)
			{
				this.SetFarthestBall((int)Enumerable.Last<Ball>(this.mBallList).GetWayPoint());
			}
			else
			{
				this.SetFarthestBall(0);
			}
			if (!this.mHasReachedCruisingSpeed && this.mAdvanceSpeed - GameApp.gDDS.GetSpeed(this.mCurveNum) * this.mSpeedScale < 0.1f)
			{
				this.mHasReachedCruisingSpeed = true;
			}
			return result;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00058AC0 File Offset: 0x00056CC0
		public void UpdateLosing()
		{
			int i = 0;
			int endPoint = this.mWayPointMgr.GetEndPoint();
			while (i < Enumerable.Count<Ball>(this.mBallList))
			{
				Ball ball = this.mBallList[i];
				if (ball.GetWayPoint() >= (float)endPoint)
				{
					int suckCount = ball.GetSuckCount();
					if (suckCount >= 0)
					{
						this.mBallList.RemoveAt(i);
						continue;
					}
					ball.SetSuckCount(suckCount + 1);
				}
				else
				{
					this.mWayPointMgr.SetWayPoint(ball, ball.GetWayPoint() + (float)(ball.GetSuckCount() >> 2), this.mLevel.mLoopAtEnd);
					ball.SetSuckCount(ball.GetSuckCount() + 1);
					if (ball.GetWayPoint() >= (float)endPoint)
					{
						ball.SetSuckCount(0);
					}
				}
				i++;
			}
			if (Enumerable.Count<Ball>(this.mBallList) > 0)
			{
				this.SetFarthestBall((int)this.mBallList[this.mBallList.Count - 1].GetWayPoint());
			}
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00058BA8 File Offset: 0x00056DA8
		public void DrawMisc(Graphics g, int thePriority, bool highest_priority)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_PATH_SPARKLES);
			for (int i = 0; i < Enumerable.Count<PathSparkle>(this.mSparkles); i++)
			{
				g.SetDrawMode(1);
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 0);
				PathSparkle pathSparkle = this.mSparkles[i];
				if (pathSparkle.mPri == thePriority)
				{
					g.DrawImageCel(imageByID, ZumasRevenge.Common._S(pathSparkle.mX) - imageByID.GetCelWidth() / 2, ZumasRevenge.Common._S(pathSparkle.mY) - imageByID.GetCelHeight() / 2, pathSparkle.mCel);
				}
				g.SetColorizeImages(false);
				g.SetDrawMode(0);
			}
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00058C51 File Offset: 0x00056E51
		public void DrawMisc(Graphics g, int thePriority)
		{
			this.DrawMisc(g, thePriority, false);
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x00058C5C File Offset: 0x00056E5C
		public void DrawBalls(BallDrawer theDrawer)
		{
			int num = 0;
			foreach (Ball ball in this.mBallList)
			{
				int priority = this.mWayPointMgr.GetPriority(ball);
				int thePriority = priority;
				Ball nextBall = ball.GetNextBall(true);
				if (nextBall != null && this.mWayPointMgr.GetPriority(nextBall) < priority)
				{
					thePriority = this.mWayPointMgr.GetPriority(nextBall);
				}
				theDrawer.AddBall(ball, priority);
				theDrawer.AddShadow(ball, thePriority);
				if (priority > num)
				{
					num = priority;
				}
				if (ball.HasOverlays())
				{
					theDrawer.AddOverlay(ball, priority);
				}
				if (ball.HasUnderlays())
				{
					theDrawer.AddUnderlay(ball, priority);
				}
			}
			foreach (Bullet bullet in this.mBulletList)
			{
				int priority2 = this.mWayPointMgr.GetPriority(bullet);
				theDrawer.AddBall(bullet, priority2);
				theDrawer.AddShadow(bullet, priority2);
			}
			theDrawer.mMaxBallPriority = num;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00058D88 File Offset: 0x00056F88
		public void DrawAboveBalls(Graphics g)
		{
			if (this.mBoard.IsPaused())
			{
				return;
			}
			foreach (Ball ball in this.mBallList)
			{
				ball.DrawAboveBalls(g);
			}
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x00058DEC File Offset: 0x00056FEC
		public void DrawUnderBalls(Graphics g)
		{
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x00058DF0 File Offset: 0x00056FF0
		public void DrawTopLevel(Graphics g)
		{
			for (int i = 0; i < Enumerable.Count<InkBlot>(this.mInkSpots); i++)
			{
				InkBlot inkBlot = this.mInkSpots[i];
				if (inkBlot.mDelay <= 0)
				{
					int num = (int)inkBlot.mAlpha;
					if (this.mLevel.mBoss != null && !MathUtils._eq(this.mLevel.mBoss.mAlphaOverride, 255f))
					{
						num = Math.Min(num, (int)this.mLevel.mBoss.mAlphaOverride);
					}
					if (num != 255)
					{
						g.SetColor(255, 255, 255, num);
						g.SetColorizeImages(true);
					}
					Transform transform = new Transform();
					if (g.Is3D())
					{
						transform.RotateRad(inkBlot.mAngle);
					}
					Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_SQUID_SPLAT);
					float num2 = inkBlot.mRadius / (float)imageByID.mWidth;
					float num3 = (255f - inkBlot.mAlpha) / 255f * ZumasRevenge.Common._M(1f);
					transform.Scale(num2, num2 + num3);
					if (g.Is3D())
					{
						g.DrawImageTransformF(imageByID, transform, inkBlot.mX, inkBlot.mY + num3 * (float)imageByID.mHeight / 2f);
					}
					else
					{
						g.DrawImageTransform(imageByID, transform, inkBlot.mX, inkBlot.mY);
					}
					g.SetColorizeImages(false);
				}
			}
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00058F54 File Offset: 0x00057154
		public void DrawSkullPathShit(Graphics g, int priority)
		{
			foreach (WarningLight warningLight in this.mWarningLights)
			{
				if (warningLight.mPriority == priority)
				{
					warningLight.Draw(g);
				}
			}
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x00058FB0 File Offset: 0x000571B0
		public bool CheckCollision(Bullet theBullet, bool should_add)
		{
			Ball ball = null;
			bool flag = false;
			int num = -1;
			for (num = 0; num != Enumerable.Count<Bullet>(this.mBulletList); num++)
			{
				Bullet bullet = this.mBulletList[num];
				if (theBullet.CollidesWithPhysically(bullet))
				{
					bullet.Update();
					this.AdvanceMergingBullet(ref num);
					break;
				}
			}
			bool flag2 = false;
			int num2;
			for (num2 = 0; num2 != Enumerable.Count<Ball>(this.mBallList); num2++)
			{
				ball = this.mBallList[num2];
				if (ball.CollidesWithPhysically(theBullet, 0) && ball.GetBullet() == null && !ball.GetIsExploding())
				{
					Ball prevBall = ball.GetPrevBall(true);
					if (prevBall == null || prevBall.GetBullet() == null)
					{
						Ball nextBall = ball.GetNextBall(true);
						if (nextBall == null || nextBall.GetBullet() == null)
						{
							SexyVector3 v = new SexyVector3(ball.GetX(), ball.GetY(), 0f);
							SexyVector3 impliedObject = new SexyVector3(theBullet.GetX(), theBullet.GetY(), 0f);
							SexyVector3 v2 = this.mWayPointMgr.CalcPerpendicular(ball.GetWayPoint());
							flag = (impliedObject - v).Cross(v2).z < 0f;
							if (!this.mWayPointMgr.InTunnel(ball, flag))
							{
								if (!theBullet.GetIsCannon())
								{
									break;
								}
								this.mBoard.GetBetaStats().BallExplodedFromPowerup(7);
								this.mBoard.IncScore(10, true);
								if (ball.GetPowerOrDestType() != PowerType.PowerType_Max)
								{
									this.mApp.SetAchievement("trigger_powerup");
								}
								this.StartExploding(ball);
								this.mBoard.PlaySmallExplosionSound();
							}
						}
					}
				}
				if (flag2)
				{
					return true;
				}
			}
			if (num2 == Enumerable.Count<Ball>(this.mBallList))
			{
				return false;
			}
			if (theBullet.GetIsCannon())
			{
				return true;
			}
			theBullet.SetHitBall(ball, flag);
			theBullet.SetMergeSpeed(this.mCurveDesc.mMergeSpeed);
			Ball nextBall2 = ball.GetNextBall();
			if (!flag)
			{
				theBullet.RemoveGapInfoForBall(ball.GetId());
			}
			else if (nextBall2 != null)
			{
				theBullet.RemoveGapInfoForBall(nextBall2.GetId());
			}
			if (CurveMgr.gStopSuckbackImmediately)
			{
				if (flag && nextBall2 != null && nextBall2.GetSuckBack() && nextBall2.GetSuckCount() > 0 && nextBall2.GetColorType() != theBullet.GetColorType())
				{
					nextBall2.SetSuckCount(0);
				}
				else if (!flag && ball != null && ball.GetSuckBack() && ball.GetSuckCount() > 0 && ball.GetColorType() != theBullet.GetColorType())
				{
					ball.SetSuckCount(0);
				}
			}
			this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BALLCLICK2));
			float wayPoint = ball.GetWayPoint();
			int num3 = ZumasRevenge.Common._M(80);
			if (this.mWayPointMgr.CheckDiscontinuity((int)wayPoint - num3, 2 * num3))
			{
				theBullet.mDoNewMerge = true;
			}
			this.mBulletList.Add(theBullet);
			return true;
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x00059275 File Offset: 0x00057475
		public bool CheckCollision(Bullet theBullet)
		{
			return this.CheckCollision(theBullet, true);
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00059280 File Offset: 0x00057480
		public bool CheckGapShot(Bullet theBullet)
		{
			int num = theBullet.GetRadius() * 2;
			float num2 = (float)theBullet.GetRadius();
			float num3 = num2 * num2;
			float x = theBullet.GetX();
			float y = theBullet.GetY();
			List<WayPoint> wayPointList = this.mWayPointMgr.GetWayPointList();
			int num4 = Enumerable.Count<WayPoint>(wayPointList);
			int curCurvePoint = theBullet.GetCurCurvePoint(this.mCurveNum);
			if (curCurvePoint > 0 && curCurvePoint < num4)
			{
				WayPoint wayPoint = wayPointList[curCurvePoint];
				float num5 = wayPoint.x - x;
				float num6 = wayPoint.y - y;
				if (num5 * num5 + num6 * num6 < num3)
				{
					return false;
				}
				theBullet.SetCurCurvePoint(this.mCurveNum, 0);
			}
			for (int i = 1; i < num4; i += num)
			{
				WayPoint wayPoint2 = wayPointList[i];
				if (!wayPoint2.mInTunnel)
				{
					float num7 = wayPoint2.x - x;
					float num8 = wayPoint2.y - y;
					if (num7 * num7 + num8 * num8 < num3)
					{
						theBullet.SetCurCurvePoint(this.mCurveNum, i);
						int num9 = 0;
						int theBallId = 0;
						int j = 0;
						while (j < this.mBallList.Count)
						{
							Ball ball = this.mBallList[j];
							if (ball.GetWayPoint() > (float)i)
							{
								Ball prevBall = ball.GetPrevBall();
								if (prevBall != null)
								{
									if (ball.GetIsExploding())
									{
										int num10 = j;
										num10++;
										bool flag = false;
										while (num10 != this.mBallList.Count)
										{
											Ball ball2 = this.mBallList[num10];
											if (!ball2.GetIsExploding())
											{
												flag = true;
												break;
											}
											num10++;
										}
										if (!flag)
										{
											break;
										}
									}
									num9 = (int)(ball.GetWayPoint() - prevBall.GetWayPoint());
									theBallId = ball.GetId();
									break;
								}
								break;
							}
							else
							{
								j++;
							}
						}
						return num9 > 0 && theBullet.AddGapInfo(this.mCurveNum, num9, theBallId);
					}
				}
			}
			return false;
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x00059458 File Offset: 0x00057658
		public bool RemoveBall(Ball theBall)
		{
			if (theBall.GetBullet() != null || theBall.GetIsExploding())
			{
				return false;
			}
			if (this.mBallList.IndexOf(theBall) != -1)
			{
				Ball nextBall = theBall.GetNextBall();
				Ball prevBall = theBall.GetPrevBall();
				if (nextBall != null)
				{
					nextBall.SetCollidesWithPrev(false);
				}
				if (prevBall != null)
				{
					prevBall.SetCollidesWithNext(false);
				}
				theBall.RemoveFromList();
				this.DeleteBall(theBall);
				return true;
			}
			return false;
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x000594B8 File Offset: 0x000576B8
		public void AddPendingBall(Ball theBall)
		{
			this.mPendingBalls.Add(theBall);
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x000594C6 File Offset: 0x000576C6
		public void DoBackwards()
		{
			if (Enumerable.Count<Ball>(this.mBallList) != 0)
			{
				this.mBackwardCount = 300;
			}
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x000594E0 File Offset: 0x000576E0
		public void DoSlowdown()
		{
			if (this.mSlowCount < 1000)
			{
				this.mSlowCount = 800;
			}
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x000594FC File Offset: 0x000576FC
		public int GetRandomPendingBallColor(int theMaxNumBalls)
		{
			int num = MathUtils.SafeRand() % theMaxNumBalls;
			if (num >= Enumerable.Count<Ball>(this.mPendingBalls))
			{
				num = Enumerable.Count<Ball>(this.mPendingBalls) - 1;
			}
			return this.mPendingBalls[num].GetColorType();
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00059540 File Offset: 0x00057740
		public bool HasPendingBallOfType(int theType, int theMaxNumBalls)
		{
			int num = 0;
			int num2 = 0;
			while (num2 != Enumerable.Count<Ball>(this.mPendingBalls) && num < theMaxNumBalls)
			{
				if (this.mPendingBalls[num2].GetColorType() == theType)
				{
					return true;
				}
				num2++;
				num++;
			}
			return false;
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00059584 File Offset: 0x00057784
		public bool IsLosing()
		{
			if (this.mHaveSets || Enumerable.Count<Ball>(this.mBallList) == 0 || this.mLevel.mLoopAtEnd || this.mBallList[Enumerable.Count<Ball>(this.mBallList) - 1].GetWayPoint() < (float)this.mWayPointMgr.GetEndPoint() || Enumerable.Count<Bullet>(this.mBulletList) != 0 || this.mBackwardCount > 0)
			{
				return false;
			}
			for (Ball ball = this.mBallList[Enumerable.Count<Ball>(this.mBallList) - 1]; ball != null; ball = ball.GetPrevBall(true))
			{
				if (ball.GetSuckCount() > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00059628 File Offset: 0x00057828
		public bool IsWinning()
		{
			bool flag = Enumerable.Count<Ball>(this.mBallList) == 0 && Enumerable.Count<Ball>(this.mPendingBalls) == 0;
			return flag && this.mLevel.mBoss == null;
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00059667 File Offset: 0x00057867
		public bool CanRestart()
		{
			return Enumerable.Count<Ball>(this.mBallList) == 0;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00059678 File Offset: 0x00057878
		public bool CanFire()
		{
			return Enumerable.Count<Ball>(this.mBallList) == 0 || this.mBallList[Enumerable.Count<Ball>(this.mBallList) - 1].GetWayPoint() < (float)this.mWayPointMgr.GetEndPoint() || this.mLevel.mLoopAtEnd;
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x000596CC File Offset: 0x000578CC
		public bool CanCompact()
		{
			foreach (Ball ball in this.mBallList)
			{
				if (ball.GetIsExploding() || ball.GetBullet() != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x00059730 File Offset: 0x00057930
		public bool IsStationary()
		{
			foreach (Ball ball in this.mBallList)
			{
				if (ball.GetIsExploding() || ball.GetSuckCount() > 0 || ball.GetBackwardsCount() > 0 || ball.GetSuckPending())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x000597A8 File Offset: 0x000579A8
		public bool IsInDanger()
		{
			return this.mInDanger;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x000597B0 File Offset: 0x000579B0
		public bool HasGaps()
		{
			int numPoints = this.mWayPointMgr.GetNumPoints();
			foreach (Ball ball in this.mBallList)
			{
				Ball nextBall = ball.GetNextBall();
				Ball prevBall = ball.GetPrevBall();
				if (nextBall != null && (int)nextBall.GetWayPoint() % numPoints > (int)ball.GetWayPoint() % numPoints && (!nextBall.CollidesWithPhysically(ball, 1) || nextBall.GetIsExploding()) && nextBall.GetBullet() == null)
				{
					return true;
				}
				if (prevBall != null && (int)prevBall.GetWayPoint() % numPoints < (int)ball.GetWayPoint() % numPoints && (!prevBall.CollidesWithPhysically(ball, 1) || prevBall.GetIsExploding()) && prevBall.GetBullet() == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00059890 File Offset: 0x00057A90
		public int GetDistanceToDeath()
		{
			if (!this.mInDanger || Enumerable.Count<Ball>(this.mBallList) == 0)
			{
				return -1;
			}
			int num = this.mWayPointMgr.GetNumPoints() - (int)this.mBallList[Enumerable.Count<Ball>(this.mBallList) - 1].GetWayPoint();
			if (num < 0)
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x000598E6 File Offset: 0x00057AE6
		public int GetDangerDistance()
		{
			return this.mWayPointMgr.GetNumPoints() - this.mDangerPoint;
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x000598FA File Offset: 0x00057AFA
		public void SetPath(string s)
		{
			this.mCurveDesc.mPath = s;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x00059908 File Offset: 0x00057B08
		public string GetPath()
		{
			return this.mCurveDesc.mPath;
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00059918 File Offset: 0x00057B18
		public Ball CheckBallIntersection(SexyVector3 p1, SexyVector3 v1, ref float t, bool skip_exploding)
		{
			Ball result = null;
			int num = 0;
			int num2 = 0;
			while (num2 != Enumerable.Count<Ball>(this.mBallList))
			{
				Ball ball = this.mBallList[num2];
				if (!this.mWayPointMgr.InTunnel((int)ball.GetWayPoint()))
				{
					float num3 = 0f;
					if ((!ball.GetIsExploding() || !skip_exploding) && ball.Intersects(p1, v1, ref num3) && num3 < t && num3 > 0f)
					{
						result = ball;
						t = num3;
					}
				}
				num2++;
				num++;
			}
			return result;
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00059999 File Offset: 0x00057B99
		public Ball CheckBallIntersection(SexyVector3 p1, SexyVector3 v1, ref float t)
		{
			return this.CheckBallIntersection(p1, v1, ref t, false);
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x000599A8 File Offset: 0x00057BA8
		public void ActivateProximityBomb(Ball theBall)
		{
			int num = 56;
			foreach (Ball ball in this.mBallList)
			{
				if (!ball.GetIsExploding() && ball.CollidesWithPhysically(theBall, num))
				{
					ball.SetComboCount(this.mBoard.GetCurComboCount(), this.mBoard.GetCurComboScore());
					this.mBoard.mNeedComboCount.Add(ball);
					this.StartExploding(ball);
					this.mBoard.GetBetaStats().BallExplodedFromPowerup(0);
				}
			}
			if (this.mLevel.mZone == 5 && this.mLevel.mNum != 10 && this.mBoard.GetGameState() == GameState.GameState_Playing)
			{
				int num2 = (int)ZumasRevenge.Common._M(15f);
				float num3 = 6.28318f / (float)num2;
				for (int i = 0; i < num2; i++)
				{
					Bubble bubble = new Bubble();
					float vx = (float)Math.Cos((double)num3 * (double)i) * MathUtils.FloatRange(ZumasRevenge.Common._M(1f), ZumasRevenge.Common._M1(1.25f));
					float vy;
					if (ZumasRevenge.Common._M(0f) != 0f)
					{
						vy = -(float)Math.Sin((double)num3 * (double)i) * MathUtils.FloatRange(ZumasRevenge.Common._M(0.75f), ZumasRevenge.Common._M1(1.25f));
					}
					else
					{
						vy = MathUtils.FloatRange(ZumasRevenge.Common._M(-0.75f), ZumasRevenge.Common._M1(-0.5f));
					}
					bubble.Init(vx, vy, MathUtils.FloatRange(ZumasRevenge.Common._M(0.05f), ZumasRevenge.Common._M1(0.2f)), (int)MathUtils.FloatRange(ZumasRevenge.Common._M2(15f), ZumasRevenge.Common._M3(25f)));
					bubble.SetAlphaFade(ZumasRevenge.Common._M(3f));
					if (ZumasRevenge.Common._M(0f) != 0f)
					{
						bubble.SetX(theBall.GetX() + (float)(-10 + MathUtils.SafeRand() % 20));
						bubble.SetY(theBall.GetY());
					}
					else
					{
						float num4 = MathUtils.FloatRange(0f, 6.28318f);
						float num5 = (float)(MathUtils.SafeRand() % num);
						bubble.SetX(num5 * (float)Math.Cos((double)num4) + theBall.GetX());
						bubble.SetY(num5 * (float)Math.Sin((double)num4) * (float)ZumasRevenge.Common._M(1) + theBall.GetY());
					}
					bubble.SetDelay(ZumasRevenge.Common._M(15));
					this.mLevel.mFrog.AddBubble(bubble);
				}
			}
			this.mLevel.ProximityBombActivated(theBall.GetX(), theBall.GetY(), 56);
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00059C50 File Offset: 0x00057E50
		public void ActivateProximityBomb(int centerx, int centery, int radius)
		{
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x00059C54 File Offset: 0x00057E54
		public void ActivatePower(Ball theBall)
		{
			PowerType powerOrDestType = theBall.GetPowerOrDestType();
			CurveMgr.gGotPowerUp[(int)powerOrDestType] = true;
			if (powerOrDestType == PowerType.PowerType_ProximityBomb)
			{
				this.ActivateProximityBomb(theBall);
				return;
			}
			if (powerOrDestType == PowerType.PowerType_MoveBackwards)
			{
				this.DoBackwards();
				return;
			}
			if (powerOrDestType == PowerType.PowerType_SlowDown)
			{
				this.DoSlowdown();
			}
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x00059C90 File Offset: 0x00057E90
		public bool DoLazerExplosion(Ball b)
		{
			if (b.GetBullet() != null || b.GetIsExploding())
			{
				return false;
			}
			if (this.mBallList.IndexOf(b) != -1)
			{
				if (this.mLevel.AllowPointsFromBalls())
				{
					this.mBoard.IncScore(10, true);
				}
				this.mBoard.GetBetaStats().BallExplodedFromPowerup(9);
				if (b.GetPowerOrDestType() != PowerType.PowerType_Max)
				{
					this.mApp.SetAchievement("trigger_powerup");
				}
				this.StartExploding(b);
				return true;
			}
			return false;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00059D10 File Offset: 0x00057F10
		public void DrawCurve(Graphics g)
		{
			this.mWayPointMgr.DrawCurve(g, new SexyColor(255, 0, 0), this.mDangerPoint);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00059D30 File Offset: 0x00057F30
		public void DrawTunnel(Graphics g, int priority)
		{
			this.mWayPointMgr.DrawTunnel(g, priority);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00059D3F File Offset: 0x00057F3F
		public void DeleteBalls()
		{
			this.mBallList.Clear();
			this.mPendingBalls.Clear();
			this.mBulletList.Clear();
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x00059D64 File Offset: 0x00057F64
		public void GetPoint(int thePoint, out int x, out int y, out int pri)
		{
			List<WayPoint> wayPointList = this.mWayPointMgr.GetWayPointList();
			if (thePoint < 0)
			{
				thePoint = 0;
			}
			if (thePoint >= Enumerable.Count<WayPoint>(wayPointList))
			{
				thePoint = Enumerable.Count<WayPoint>(wayPointList) - 1;
			}
			WayPoint wayPoint = wayPointList[thePoint];
			x = (int)wayPoint.x;
			y = (int)wayPoint.y;
			pri = (int)wayPoint.mPriority;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x00059DBA File Offset: 0x00057FBA
		public int GetCurveLength()
		{
			return this.mWayPointMgr.GetNumPoints();
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00059DC7 File Offset: 0x00057FC7
		public int GetTotalBalls()
		{
			if (this.mCurveDesc.mVals.mNumBalls == 0)
			{
				return 0;
			}
			return this.mTotalBalls;
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00059DE4 File Offset: 0x00057FE4
		public void ZumaAchieved(bool stop)
		{
			if (this.mStopAddingBalls == stop)
			{
				return;
			}
			if (this.GetFarthestBallPercent() > GameApp.gDDS.GetRollbackPct(this.mCurveNum))
			{
				this.mBackwardCount = GameApp.gDDS.GetZumaBack(this.mCurveNum);
				if (!this.mBoard.GauntletMode())
				{
					this.mSlowCount = GameApp.gDDS.GetZumaSlow(this.mCurveNum);
				}
			}
			if (!this.mBoard.GauntletMode())
			{
				this.mStopAddingBalls = stop;
				this.mPostZumaFlashTimer = ZumasRevenge.Common._M(50);
				if (stop)
				{
					this.mPendingBalls.Clear();
					return;
				}
			}
			else
			{
				this.mHasReachedCruisingSpeed = false;
				this.mHasReachedRolloutPoint = false;
				this.mCanCheckForSpeedup = false;
			}
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00059E91 File Offset: 0x00058091
		public void DoEndlessZumaEffect()
		{
			if (this.GetFarthestBallPercent() > 50)
			{
				this.mBackwardCount = GameApp.gDDS.GetZumaBack(this.mCurveNum);
			}
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x00059EB3 File Offset: 0x000580B3
		private static void GetNumPendingSinglesHelper(int aColor, ref int aNumGroups, ref int aPrevColor, ref int aNumSingles, ref int aGroupCount)
		{
			if (aColor != aPrevColor)
			{
				if (aGroupCount == 1)
				{
					aNumSingles++;
				}
				aGroupCount = 1;
				aNumGroups++;
				aPrevColor = aColor;
				return;
			}
			aGroupCount++;
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x00059EDC File Offset: 0x000580DC
		public void DetonateBalls(int theType, bool from_lightning_frog, bool allow_powerups)
		{
			foreach (Ball ball in this.mBallList)
			{
				if (!ball.GetIsExploding() && (theType == -1 || ball.GetColorType() == theType))
				{
					if (!allow_powerups)
					{
						ball.Explode(true, from_lightning_frog);
					}
					else
					{
						if (ball.GetPowerOrDestType() != PowerType.PowerType_Max)
						{
							this.mApp.SetAchievement("trigger_powerup");
						}
						this.StartExploding(ball, from_lightning_frog, true);
					}
					if (this.mLevel.AllowPointsFromBalls() && from_lightning_frog)
					{
						this.mBoard.IncScore(10, true);
					}
					if (from_lightning_frog)
					{
						this.mLevel.IncNumBallsExploded(1);
					}
					if (from_lightning_frog)
					{
						this.mBoard.GetBetaStats().BallExplodedFromPowerup(8);
					}
				}
			}
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x00059FB8 File Offset: 0x000581B8
		public void DetonateBalls()
		{
			this.DetonateBalls(-1, false, false);
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x00059FC4 File Offset: 0x000581C4
		public int GetFarthestBallPercent(bool ignore_gaps)
		{
			if (Enumerable.Count<Ball>(this.mBallList) == 0)
			{
				return 0;
			}
			float num = Enumerable.Last<Ball>(this.mBallList).GetWayPoint();
			if (!ignore_gaps)
			{
				foreach (Ball ball in this.mBallList)
				{
					if (!ball.GetCollidesWithNext())
					{
						num = ball.GetWayPoint();
						break;
					}
				}
			}
			return (int)(num * 100f / (float)this.mWayPointMgr.GetNumPoints());
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0005A05C File Offset: 0x0005825C
		public int GetFarthestBallPercent()
		{
			return this.GetFarthestBallPercent(true);
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0005A068 File Offset: 0x00058268
		public int GetNearestBallPercent()
		{
			if (Enumerable.Count<Ball>(this.mBallList) == 0)
			{
				return 0;
			}
			float wayPoint = Enumerable.First<Ball>(this.mBallList).GetWayPoint();
			return (int)(wayPoint * 100f / (float)this.mWayPointMgr.GetNumPoints());
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0005A0AC File Offset: 0x000582AC
		public void GetNearestBallXY(out float x, out float y)
		{
			if (Enumerable.Count<Ball>(this.mBallList) == 0)
			{
				x = (y = -2.1474836E+09f);
				return;
			}
			x = Enumerable.First<Ball>(this.mBallList).GetX();
			y = Enumerable.First<Ball>(this.mBallList).GetY();
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0005A0F8 File Offset: 0x000582F8
		public void GetFarthestBallXY(out float x, out float y)
		{
			if (Enumerable.Count<Ball>(this.mBallList) == 0)
			{
				x = (y = -2.1474836E+09f);
				return;
			}
			x = Enumerable.Last<Ball>(this.mBallList).GetX();
			y = Enumerable.Last<Ball>(this.mBallList).GetY();
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0005A143 File Offset: 0x00058343
		public int GetLastClearedBallPoint()
		{
			return this.mLastClearedBallPoint;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0005A14B File Offset: 0x0005834B
		public bool HasReachedCruisingSpeed()
		{
			return this.mHasReachedCruisingSpeed;
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0005A153 File Offset: 0x00058353
		public bool HasReachedRolloutPoint()
		{
			return this.mHasReachedRolloutPoint;
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0005A15C File Offset: 0x0005835C
		public Ball AddPowerUp(PowerType thePower)
		{
			if (this.mApp.GetLevelMgr().mUniquePowerupColor || thePower == PowerType.PowerType_GauntletMultBall)
			{
				List<Ball> list = new List<Ball>();
				List<int> list2 = new List<int>();
				for (int i = 0; i < this.mCurveDesc.mVals.mNumColors; i++)
				{
					if (this.mBallColorHasPowerup[i] == 0 || thePower == PowerType.PowerType_GauntletMultBall)
					{
						list2.Add(i);
					}
				}
				Ball ball = null;
				while (Enumerable.Count<int>(list2) > 0 && ball == null)
				{
					int num = MathUtils.SafeRand() % Enumerable.Count<int>(list2);
					int num2 = list2[num];
					foreach (Ball ball2 in this.mBallList)
					{
						if (ball2.GetPowerType() == PowerType.PowerType_Max && ball2.GetDestPowerType() == PowerType.PowerType_Max && ball2.GetColorType() == num2 && !ball2.GetIsExploding() && (thePower != PowerType.PowerType_GauntletMultBall || (thePower == PowerType.PowerType_GauntletMultBall && !this.mWayPointMgr.InTunnel(ball2, true) && !this.mWayPointMgr.InTunnel(ball2, false) && ball2.GetWayPoint() / (float)this.mWayPointMgr.GetEndPoint() >= this.mApp.GetLevelMgr().mMinMultBallDistance)))
						{
							list.Add(ball2);
						}
					}
					if (Enumerable.Count<PowerupRegion>(this.mLevel.mPowerupRegions) > 0)
					{
						foreach (PowerupRegion powerupRegion in this.mLevel.mPowerupRegions)
						{
							if (powerupRegion.mCurveNum == this.mCurveNum && SexyFramework.Common.Rand(powerupRegion.mChance) == 0)
							{
								int num3 = (int)powerupRegion.mCurvePctStart * this.mWayPointMgr.GetNumPoints();
								int num4 = (int)powerupRegion.mCurvePctEnd * this.mWayPointMgr.GetNumPoints();
								using (List<Ball>.Enumerator enumerator3 = list.GetEnumerator())
								{
									while (enumerator3.MoveNext())
									{
										Ball ball3 = enumerator3.Current;
										if (ball3.GetWayPoint() >= (float)num3 && ball3.GetWayPoint() <= (float)num4)
										{
											ball = ball3;
											break;
										}
									}
									break;
								}
							}
						}
					}
					if (Enumerable.Count<Ball>(list) == 0)
					{
						list2.RemoveAt(num);
					}
					else if (ball == null)
					{
						ball = list[MathUtils.SafeRand() % Enumerable.Count<Ball>(list)];
					}
				}
				if (ball != null)
				{
					ball.SetPowerType(thePower);
					if (thePower == PowerType.PowerType_GauntletMultBall)
					{
						ball.SetPowerCount(this.mApp.GetLevelMgr().mMultBallLife);
					}
				}
				return ball;
			}
			int num5 = MathUtils.SafeRand() % Enumerable.Count<Ball>(this.mBallList);
			Ball ball4 = this.mBallList[num5];
			if (ball4.GetPowerType() == PowerType.PowerType_Max && ball4.GetDestPowerType() == PowerType.PowerType_Max)
			{
				ball4.SetPowerType(thePower);
				return ball4;
			}
			return null;
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0005A450 File Offset: 0x00058650
		public void ElectrifyBalls(int theType, bool val)
		{
			foreach (Ball ball in this.mBallList)
			{
				if (!ball.GetIsExploding() && (ball.GetColorType() == theType || theType == -1))
				{
					ball.DoElectricOverlay(val);
				}
			}
			foreach (Ball ball2 in this.mPendingBalls)
			{
				if (!ball2.GetIsExploding() && (ball2.GetColorType() == theType || theType == -1))
				{
					ball2.DoElectricOverlay(val);
				}
			}
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0005A510 File Offset: 0x00058710
		public Ball GetBallFromWaypoint(int wp)
		{
			List<WayPoint> wayPointList = this.mWayPointMgr.GetWayPointList();
			WayPoint wayPoint = wayPointList[wp];
			foreach (Ball ball in this.mBallList)
			{
				if (ball.Contains((int)wayPoint.x, (int)wayPoint.y))
				{
					return ball;
				}
			}
			return null;
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0005A590 File Offset: 0x00058790
		public bool AtRest()
		{
			if (Enumerable.Count<Ball>(this.mBallList) == 0)
			{
				return true;
			}
			foreach (Ball ball in this.mBallList)
			{
				if (ball.GetIsExploding() || ball.GetSuckCount() > 0 || ball.GetBullet() != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0005A60C File Offset: 0x0005880C
		public int GetWaypointFromXY(float x, float y, ref bool is_tunnel, uint skip_amt, uint dist_squared)
		{
			List<WayPoint> wayPointList = this.mWayPointMgr.GetWayPointList();
			for (int i = 1; i < Enumerable.Count<WayPoint>(wayPointList); i++)
			{
				WayPoint wayPoint = wayPointList[i];
				float num = wayPoint.x - x;
				float num2 = wayPoint.y - y;
				if (num * num + num2 * num2 <= dist_squared)
				{
					is_tunnel = wayPoint.mInTunnel;
					return i;
				}
				i += (int)skip_amt;
			}
			return -1;
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0005A670 File Offset: 0x00058870
		public int GetWaypointFromXY(float x, float y, ref bool is_tunnel)
		{
			return this.GetWaypointFromXY(x, y, ref is_tunnel, 0U, 25U);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0005A680 File Offset: 0x00058880
		public bool GetXYFromWaypoint(int waypoint, out float x, out float y)
		{
			List<WayPoint> wayPointList = this.mWayPointMgr.GetWayPointList();
			if (waypoint >= Enumerable.Count<WayPoint>(wayPointList) || waypoint < 0)
			{
				x = 0f;
				y = 0f;
				return false;
			}
			WayPoint wayPoint = wayPointList[waypoint];
			x = wayPoint.x;
			y = wayPoint.y;
			return true;
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0005A6CF File Offset: 0x000588CF
		public bool AllowsPowerup(PowerType p)
		{
			return this.mCurveDesc.mVals.mPowerUpFreq[(int)p] > 0;
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0005A6E8 File Offset: 0x000588E8
		public bool HasPowerup(PowerType p)
		{
			foreach (Ball ball in this.mBallList)
			{
				if (ball.GetPowerOrDestType() == p)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0005A744 File Offset: 0x00058944
		public void StopAddingBalls()
		{
			this.mStopAddingBalls = true;
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0005A74D File Offset: 0x0005894D
		public bool HasBall(Ball b)
		{
			return this.mBallList.IndexOf(b) != -1;
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0005A764 File Offset: 0x00058964
		public int GetBallIndex(Ball b)
		{
			if (!this.HasBall(b))
			{
				return -1;
			}
			int result = 0;
			foreach (Ball ball in this.mBallList)
			{
				if (ball == b)
				{
					return result;
				}
			}
			return -1;
		}

		// Token: 0x0400076F RID: 1903
		public static bool[] gGotPowerUp = new bool[14];

		// Token: 0x04000770 RID: 1904
		public static bool gStopSuckbackImmediately = false;

		// Token: 0x04000771 RID: 1905
		public static int MAX_GAP_SIZE = 300;

		// Token: 0x04000772 RID: 1906
		public GameApp mApp;

		// Token: 0x04000773 RID: 1907
		public Board mBoard;

		// Token: 0x04000774 RID: 1908
		public WayPointMgr mWayPointMgr;

		// Token: 0x04000775 RID: 1909
		public Level mLevel;

		// Token: 0x04000776 RID: 1910
		public CurveDesc mCurveDesc;

		// Token: 0x04000777 RID: 1911
		public float mSpeedScale;

		// Token: 0x04000778 RID: 1912
		public bool mIsLoaded;

		// Token: 0x04000779 RID: 1913
		public SexyColor mLastScoreColor = default(SexyColor);

		// Token: 0x0400077A RID: 1914
		public List<PathSparkle> mSparkles = new List<PathSparkle>();

		// Token: 0x0400077B RID: 1915
		public List<WarningLight> mWarningLights = new List<WarningLight>();

		// Token: 0x0400077C RID: 1916
		public List<Bullet> mBulletList = new List<Bullet>();

		// Token: 0x0400077D RID: 1917
		public List<Ball> mBallList = new List<Ball>();

		// Token: 0x0400077E RID: 1918
		public List<Ball> mPendingBalls = new List<Ball>();

		// Token: 0x0400077F RID: 1919
		public int mPostZumaFlashTimer;

		// Token: 0x04000780 RID: 1920
		public int mLastPowerupTime;

		// Token: 0x04000781 RID: 1921
		public int[] mLastSpawnedPowerUpFrame = new int[14];

		// Token: 0x04000782 RID: 1922
		public int[] mLastCompletedPowerUpFrame = new int[14];

		// Token: 0x04000783 RID: 1923
		public int[] mNumPowerUpsThisLevel = new int[14];

		// Token: 0x04000784 RID: 1924
		public int[] mNumPowerupsActivated = new int[14];

		// Token: 0x04000785 RID: 1925
		public int[] mBallColorHasPowerup = new int[6];

		// Token: 0x04000786 RID: 1926
		public int mNumBallsCreated;

		// Token: 0x04000787 RID: 1927
		public int mCurveNum;

		// Token: 0x04000788 RID: 1928
		public int mStopTime;

		// Token: 0x04000789 RID: 1929
		public int mProxBombCounter;

		// Token: 0x0400078A RID: 1930
		public int mSlowCount;

		// Token: 0x0400078B RID: 1931
		public int mBackwardCount;

		// Token: 0x0400078C RID: 1932
		public int mTotalBalls;

		// Token: 0x0400078D RID: 1933
		public float mAdvanceSpeed;

		// Token: 0x0400078E RID: 1934
		public float mSkullHilite;

		// Token: 0x0400078F RID: 1935
		public float mSkullHiliteDir;

		// Token: 0x04000790 RID: 1936
		public int mFirstChainEnd;

		// Token: 0x04000791 RID: 1937
		public bool mFirstBallMovedBackwards;

		// Token: 0x04000792 RID: 1938
		public bool mHaveSets;

		// Token: 0x04000793 RID: 1939
		public bool mDoingClearCurveRollout;

		// Token: 0x04000794 RID: 1940
		public bool mInitialPathHilite;

		// Token: 0x04000795 RID: 1941
		public int mLastPathHiliteWP;

		// Token: 0x04000796 RID: 1942
		public int mLastPathHilitePitch;

		// Token: 0x04000797 RID: 1943
		public int mDangerPoint;

		// Token: 0x04000798 RID: 1944
		public int mPathLightEndFrame;

		// Token: 0x04000799 RID: 1945
		public int mLastClearedBallPoint;

		// Token: 0x0400079A RID: 1946
		public float mOverrideSpeed;

		// Token: 0x0400079B RID: 1947
		public bool mHadPowerUp;

		// Token: 0x0400079C RID: 1948
		public bool mStopAddingBalls;

		// Token: 0x0400079D RID: 1949
		public bool mInDanger;

		// Token: 0x0400079E RID: 1950
		public bool mHasReachedCruisingSpeed;

		// Token: 0x0400079F RID: 1951
		public bool mHasReachedRolloutPoint;

		// Token: 0x040007A0 RID: 1952
		public bool mNeedsSpeedup;

		// Token: 0x040007A1 RID: 1953
		public bool mCanCheckForSpeedup;

		// Token: 0x040007A2 RID: 1954
		public uint mLastPathShowTick;

		// Token: 0x040007A3 RID: 1955
		public int mNumMultBallsToSpawn;

		// Token: 0x040007A4 RID: 1956
		public List<InkBlot> mInkSpots = new List<InkBlot>();
	}
}
