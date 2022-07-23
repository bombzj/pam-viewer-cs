using System;
using System.Collections.Generic;
using System.Linq;
using SexyFramework;
using SexyFramework.Drivers;

namespace ZumasRevenge
{
	// Token: 0x0200000F RID: 15
	public class AutoMonkey
	{
		// Token: 0x06000397 RID: 919 RVA: 0x00008DE0 File Offset: 0x00006FE0
		public AutoMonkey(GameApp app)
		{
			this.mApp = app;
			this.mStateList.Add(MonkeyState.IntroScreen);
			this.mAllModesMode = MonkeyMode.PlayThroughGame;
			this.mAutoMonkeyMode = MonkeyMode.PlayThroughGame;
			this.mLastButtonPress = 0;
			this.mStateCount = 0;
			this.mRandomButtonPress = 0;
			this.mMoveDir = 2;
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_DPAD_UP);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_DPAD_DOWN);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_DPAD_LEFT);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_DPAD_RIGHT);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_UP);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_DOWN);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_LEFT);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_RIGHT);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_BACK);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_START);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_A);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_B);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_X);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_Y);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_LB);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_RB);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_LTRIGGER);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_RTRIGGER);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_LSTICK);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_RSTICK);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_DPAD_UP);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_DPAD_DOWN);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_DPAD_RIGHT);
			this.mAllowedButtons.Add(GamepadButton.GAMEPAD_BUTTON_DPAD_LEFT);
			this.mDirectionButtons.Add(GamepadButton.GAMEPAD_BUTTON_UP);
			this.mDirectionButtons.Add(GamepadButton.GAMEPAD_BUTTON_DOWN);
			this.mDirectionButtons.Add(GamepadButton.GAMEPAD_BUTTON_RIGHT);
			this.mDirectionButtons.Add(GamepadButton.GAMEPAD_BUTTON_LEFT);
			this.mAutoMonkeyDelay = 0.3f;
			this.mEnableAutoMonkey = false;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00008FC4 File Offset: 0x000071C4
		~AutoMonkey()
		{
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00008FEC File Offset: 0x000071EC
		public void Update()
		{
			this.mLastButtonPress++;
			this.mStateCount++;
			this.mRandomButtonPress++;
			switch (Enumerable.Last<MonkeyState>(this.mStateList))
			{
			case MonkeyState.IntroScreen:
				this.UpdateIntroScreen();
				return;
			case MonkeyState.MainMenu:
				this.UpdateMainMenu();
				return;
			case MonkeyState.ModalOkDialog:
				this.UpdateModalDialog();
				return;
			case MonkeyState.ModalYesNoDialog:
				this.UpdateYesNoDialog();
				return;
			case MonkeyState.PauseDialog:
				break;
			case MonkeyState.Playing:
				this.UpdatePlaying();
				break;
			default:
				return;
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00009070 File Offset: 0x00007270
		public void SetState(MonkeyState state)
		{
			this.mStateList.Add(state);
			this.mStateCount = 0;
			this.mLastButtonPress = 0;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000908C File Offset: 0x0000728C
		public void RemoveLastInstanceOfState(MonkeyState state)
		{
			bool flag = false;
			int num = this.mStateList.Count - 1;
			while (num >= 0 && !flag)
			{
				if (this.mStateList[num] == state)
				{
					this.mStateList.RemoveAt(num);
					flag = true;
				}
				num--;
			}
			if (!flag)
			{
				Console.WriteLine("Unable to find state '{0}' to remove from AutoMonkey!!", this.GetStateString(state));
			}
			this.mStateCount = 0;
			this.mLastButtonPress = 0;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x000090F5 File Offset: 0x000072F5
		public MonkeyMode GetMode()
		{
			return this.mAutoMonkeyMode;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x000090FD File Offset: 0x000072FD
		public bool IsEnabled()
		{
			return this.GetMode() != MonkeyMode.Disabled;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000910B File Offset: 0x0000730B
		protected void UpdateIntroScreen()
		{
			if (this.mAutoMonkeyDelay <= (float)this.mLastButtonPress / 100f)
			{
				this.PressButtonDown(GamepadButton.GAMEPAD_BUTTON_A, true);
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000912A File Offset: 0x0000732A
		protected void UpdateMainMenu()
		{
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000912C File Offset: 0x0000732C
		protected void UpdateModalDialog()
		{
			if (3f <= (float)this.mStateCount / 100f)
			{
				this.PressButton(GamepadButton.GAMEPAD_BUTTON_A, true);
			}
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000914C File Offset: 0x0000734C
		protected void UpdateYesNoDialog()
		{
			if (3f <= (float)this.mStateCount / 100f)
			{
				if (SexyFramework.Common.Rand() % 2 == 0)
				{
					this.PressButton(GamepadButton.GAMEPAD_BUTTON_RIGHT, true);
				}
				else
				{
					this.PressButton(GamepadButton.GAMEPAD_BUTTON_LEFT, true);
				}
				this.PressButton(GamepadButton.GAMEPAD_BUTTON_A, true);
			}
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00009194 File Offset: 0x00007394
		protected void UpdatePlaying()
		{
			if (this.mApp.mBoard == null)
			{
				return;
			}
			if (this.mApp.mMapScreen != null)
			{
				this.PressButton(GamepadButton.GAMEPAD_BUTTON_A, true);
			}
			bool flag = !this.mApp.mBoard.mDoingFirstTimeIntro && !this.mApp.mBoard.mDoingIronFrogWin && Enumerable.Count<ZumaTip>(this.mApp.mBoard.mZumaTips) == 0 && this.mApp.mBoard.mLevelTransition == null && !this.mApp.mBoard.mShowMapScreen;
			if (this.mAutoMonkeyDelay <= (float)this.mLastButtonPress / 100f)
			{
				if (this.mApp.mBoard.mDoingFirstTimeIntro || this.mApp.mBoard.mDoingIronFrogWin || this.mApp.mBoard.mLevelTransition != null || this.mApp.mBoard.mShowMapScreen)
				{
					this.mApp.mBoard.MouseDown(GameApp.gApp.GetScreenRect().mWidth / 2, GameApp.gApp.GetScreenRect().mHeight / 2, 1);
					this.mApp.mBoard.MouseUp(GameApp.gApp.GetScreenRect().mWidth / 2, GameApp.gApp.GetScreenRect().mHeight / 2, 1);
					this.PressButton(GamepadButton.GAMEPAD_BUTTON_A, true);
				}
				else if (this.mApp.mBoard.mZumaTips.Count != 0)
				{
					if (this.mApp.mBoard.mZumaTips[0].mId == ZumaProfile.FIRST_SHOT_HINT)
					{
						this.mApp.mBoard.mFrog.SetDestAngle(4.64f);
						this.mApp.mBoard.MouseUp((int)Common._S(this.mApp.mBoard.mFrog.mCurX - 150f), (int)Common._S(this.mApp.mBoard.mFrog.mCurY), 1);
					}
					else if (this.mApp.mBoard.mZumaTips[0].mId == ZumaProfile.ZUMA_BAR_HINT || this.mApp.mBoard.mZumaTips[0].mId == ZumaProfile.SKULL_PIT_HINT)
					{
						this.mApp.mBoard.MouseDown(GameApp.gApp.GetScreenRect().mWidth / 2, GameApp.gApp.GetScreenRect().mHeight / 2, 1);
						this.mApp.mBoard.MouseUp(GameApp.gApp.GetScreenRect().mWidth / 2, GameApp.gApp.GetScreenRect().mHeight / 2, 1);
						this.PressButton(GamepadButton.GAMEPAD_BUTTON_A, true);
					}
					else if (this.mApp.mBoard.mZumaTips[0].mId == ZumaProfile.LILLY_PAD_HINT)
					{
						if (this.mApp.mBoard.mLevel != null)
						{
							int gunPointFromPos = this.mApp.mBoard.mLevel.GetGunPointFromPos((int)this.mApp.mBoard.mFrog.mCurX, (int)this.mApp.mBoard.mFrog.mCurY);
							int num;
							do
							{
								num = SexyFramework.Common.Rand() % this.mApp.mBoard.mLevel.mNumFrogPoints;
							}
							while (num == gunPointFromPos);
							if (num >= 0 && num != this.mApp.mBoard.mLevel.mCurFrogPoint)
							{
								this.mApp.mBoard.mLevel.mCurFrogPoint = num;
								this.mApp.mBoard.mFrog.SetDestPos(this.mApp.mBoard.mLevel.mFrogX[num], this.mApp.mBoard.mLevel.mFrogY[num], this.mApp.mBoard.mLevel.mMoveSpeed, true);
								this.mApp.mBoard.mLevel.ChangedPad(num);
								this.mApp.mUserProfile.MarkHintAsSeen(ZumaProfile.LILLY_PAD_HINT);
								this.mApp.mBoard.mZumaTips.RemoveAt(0);
								if (Enumerable.Count<ZumaTip>(this.mApp.mBoard.mZumaTips) == 0)
								{
									this.mApp.mBoard.mPreventBallAdvancement = false;
								}
							}
						}
					}
					else if (this.mApp.mBoard.mZumaTips[0].mId == ZumaProfile.FRUIT_HINT)
					{
						this.mApp.mBoard.mFrog.SetDestAngle(4.2f);
						this.PressButton(GamepadButton.GAMEPAD_BUTTON_A, true);
						this.mApp.mBoard.MouseUp((int)Common._S(this.mApp.mBoard.mFrog.mCurX - 150f), (int)Common._S(this.mApp.mBoard.mFrog.mCurY - 100f), 1);
					}
					else if (this.mApp.mBoard.mZumaTips[0].mId == ZumaProfile.SWAP_BALL_HINT)
					{
						this.mApp.mBoard.MouseDown((int)Common._S(this.mApp.mBoard.mFrog.mCurX), (int)Common._S(this.mApp.mBoard.mFrog.mCurY), 1);
						this.mApp.mBoard.MouseUp((int)Common._S(this.mApp.mBoard.mFrog.mCurX), (int)Common._S(this.mApp.mBoard.mFrog.mCurY), 1);
					}
				}
				else if (this.mApp.mCredits != null)
				{
					if (this.mApp.mCredits.mInitialDelay >= Common._M(300))
					{
						this.mApp.ReturnFromCredits();
					}
				}
				else if (this.mApp.mBoard.mLevel != null && this.mApp.mBoard.mLevel.mFinalLevel && this.mApp.mBoard.mAdventureWinScreen && this.mApp.mBoard.mAdventureWinAlpha > 0f)
				{
					if (this.mApp.mBoard.mAdvWinBtn != null)
					{
						this.mApp.mBoard.ButtonDepress(this.mApp.mBoard.mAdvWinBtn.mId);
					}
				}
				else
				{
					bool flag2 = true;
					for (int i = 0; i < this.mApp.mBoard.mLevel.mNumCurves; i++)
					{
						if (Enumerable.Count<Bullet>(this.mApp.mBoard.mLevel.mCurveMgr[i].mBulletList) != 0)
						{
							flag2 = false;
							break;
						}
					}
					flag2 = flag2 && Enumerable.Count<Bullet>(this.mApp.mBoard.mBulletList) == 0;
					if (flag2)
					{
						this.mApp.mBoard.mFrog.UpdateAutoMonkeyShotCorrection();
						if (this.mApp.mBoard.mFrog.mShotCorrectionTarget.x != 0f && this.mApp.mBoard.mFrog.mShotCorrectionTarget.y != 0f)
						{
							this.mApp.mBoard.mFrog.SetDestAngle(this.mApp.mBoard.mFrog.mShotCorrectionRad + 1.570795f);
							this.mApp.mBoard.MouseUp((int)(Common._S(this.mApp.mBoard.mFrog.mCurX) + this.mApp.mBoard.mFrog.mShotCorrectionTarget.x), (int)(Common._S(this.mApp.mBoard.mFrog.mCurY) + this.mApp.mBoard.mFrog.mShotCorrectionTarget.y), 1);
						}
						else if (this.mApp.mBoard.mLevel.mNumFrogPoints > 1)
						{
							this.PressButton(GamepadButton.GAMEPAD_BUTTON_Y, true);
						}
						else
						{
							this.PressButton(GamepadButton.GAMEPAD_BUTTON_B, true);
							this.mApp.mBoard.SwapFrogBalls();
						}
					}
				}
			}
			if (flag && this.mApp.mBoard != null && this.mApp.mBoard.mLevel.mMoveType == 1 && this.mApp.mBoard.mLevel.mBoss != null)
			{
				int num2 = this.mApp.mBoard.mLevel.mFrogX[0];
				int num3 = num2 + this.mApp.mBoard.mLevel.mBarWidth;
				int curX = this.mApp.mBoard.mFrog.GetCurX();
				if (curX <= num2)
				{
					this.mMoveDir = 2;
					this.mApp.mBoard.mFrog.SetDestPos(num2 + this.mMoveDir, this.mApp.mBoard.mFrog.GetCurY(), this.mApp.mBoard.mLevel.mMoveSpeed, true);
				}
				else if (curX >= num3)
				{
					this.mMoveDir = -2;
					this.mApp.mBoard.mFrog.SetDestPos(num3 + this.mMoveDir, this.mApp.mBoard.mFrog.GetCurY(), this.mApp.mBoard.mLevel.mMoveSpeed, true);
				}
				this.mApp.mBoard.mFrog.SetDestPos(curX + this.mMoveDir, this.mApp.mBoard.mFrog.GetCurY(), this.mApp.mBoard.mLevel.mMoveSpeed, true);
			}
			if (flag && this.mApp.mBoard != null && this.mApp.mBoard.mCheckpointEffect != null)
			{
				this.mApp.mBoard.mCheckpointEffect.ButtonDepress(0);
			}
			if (!flag && this.mApp.mBoard != null && this.mApp.mBoard.mStatsContinueBtn != null)
			{
				this.mApp.mBoard.ButtonDepress(2);
			}
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00009BCB File Offset: 0x00007DCB
		protected void PressButtonDown(GamepadButton button, bool bResetTimer)
		{
			this.mApp.GamepadButtonDown(button, 0, 0U);
			if (bResetTimer)
			{
				this.mLastButtonPress = 0;
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00009BE5 File Offset: 0x00007DE5
		protected void PressButtonUp(GamepadButton button, bool bResetTimer)
		{
			this.mApp.GamepadButtonUp(button, 0, 0U);
			if (bResetTimer)
			{
				this.mLastButtonPress = 0;
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00009BFF File Offset: 0x00007DFF
		protected void PressButton(GamepadButton button, bool bResetTimer)
		{
			this.PressButtonDown(button, bResetTimer);
			this.PressButtonUp(button, bResetTimer);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00009C14 File Offset: 0x00007E14
		public string GetStateString(MonkeyState state)
		{
			switch (state)
			{
			case MonkeyState.IntroScreen:
				return "IntroScreen";
			case MonkeyState.MainMenu:
				return "MainMenu";
			case MonkeyState.ModalOkDialog:
				return "ModalOkDialog";
			case MonkeyState.ModalYesNoDialog:
				return "ModalYesNoDialog";
			case MonkeyState.PauseDialog:
				return "PauseDialog";
			case MonkeyState.Playing:
				return "Playing";
			case MonkeyState.None:
				return "None";
			default:
				return "";
			}
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00009C78 File Offset: 0x00007E78
		public string GetButtonString(GamepadButton button)
		{
			switch (button)
			{
			case GamepadButton.GAMEPAD_BUTTON_UP:
				return "GAMEPAD_BUTTON_UP";
			case GamepadButton.GAMEPAD_BUTTON_DOWN:
				return "GAMEPAD_BUTTON_DOWN";
			case GamepadButton.GAMEPAD_BUTTON_LEFT:
				return "GAMEPAD_BUTTON_LEFT";
			case GamepadButton.GAMEPAD_BUTTON_RIGHT:
				return "GAMEPAD_BUTTON_RIGHT";
			case GamepadButton.GAMEPAD_BUTTON_BACK:
				return "GAMEPAD_BUTTON_BACK";
			case GamepadButton.GAMEPAD_BUTTON_START:
				return "GAMEPAD_BUTTON_START";
			case GamepadButton.GAMEPAD_BUTTON_A:
				return "GAMEPAD_BUTTON_A";
			case GamepadButton.GAMEPAD_BUTTON_B:
				return "GAMEPAD_BUTTON_B";
			case GamepadButton.GAMEPAD_BUTTON_X:
				return "GAMEPAD_BUTTON_X";
			case GamepadButton.GAMEPAD_BUTTON_Y:
				return "GAMEPAD_BUTTON_Y";
			case GamepadButton.GAMEPAD_BUTTON_LB:
				return "GAMEPAD_BUTTON_LB";
			case GamepadButton.GAMEPAD_BUTTON_RB:
				return "GAMEPAD_BUTTON_RB";
			case GamepadButton.GAMEPAD_BUTTON_LTRIGGER:
				return "GAMEPAD_BUTTON_LTRIGGER";
			case GamepadButton.GAMEPAD_BUTTON_RTRIGGER:
				return "GAMEPAD_BUTTON_RTRIGGER";
			case GamepadButton.GAMEPAD_BUTTON_LSTICK:
				return "GAMEPAD_BUTTON_LSTICK";
			case GamepadButton.GAMEPAD_BUTTON_RSTICK:
				return "GAMEPAD_BUTTON_RSTICK";
			case GamepadButton.GAMEPAD_BUTTON_DPAD_UP:
				return "GAMEPAD_BUTTON_DPAD_UP";
			case GamepadButton.GAMEPAD_BUTTON_DPAD_DOWN:
				return "GAMEPAD_BUTTON_DPAD_DOWN";
			case GamepadButton.GAMEPAD_BUTTON_DPAD_LEFT:
				return "GAMEPAD_BUTTON_DPAD_LEFT";
			case GamepadButton.GAMEPAD_BUTTON_DPAD_RIGHT:
				return "GAMEPAD_BUTTON_DPAD_RIGHT";
			default:
				return "NONE";
			}
		}

		// Token: 0x0400007A RID: 122
		public MonkeyMode mAutoMonkeyMode;

		// Token: 0x0400007B RID: 123
		public float mAutoMonkeyDelay;

		// Token: 0x0400007C RID: 124
		public bool mEnableAutoMonkey;

		// Token: 0x0400007D RID: 125
		protected GameApp mApp;

		// Token: 0x0400007E RID: 126
		protected List<MonkeyState> mStateList = new List<MonkeyState>();

		// Token: 0x0400007F RID: 127
		protected int mStateCount;

		// Token: 0x04000080 RID: 128
		protected List<GamepadButton> mAllowedButtons = new List<GamepadButton>();

		// Token: 0x04000081 RID: 129
		protected List<GamepadButton> mDirectionButtons = new List<GamepadButton>();

		// Token: 0x04000082 RID: 130
		protected int mLastButtonPress;

		// Token: 0x04000083 RID: 131
		protected int mMoveDir;

		// Token: 0x04000084 RID: 132
		protected int mRandomButtonPress;

		// Token: 0x04000085 RID: 133
		protected MonkeyMode mAllModesMode;
	}
}
