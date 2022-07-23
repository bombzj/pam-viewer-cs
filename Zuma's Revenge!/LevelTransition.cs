using System;
using System.Collections.Generic;
using System.Linq;
using SexyFramework;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x02000107 RID: 263
	public class LevelTransition : IDisposable
	{
		// Token: 0x06000E03 RID: 3587 RVA: 0x0008E5E4 File Offset: 0x0008C7E4
		protected void SetupBambooSmoke()
		{
			int i = Common._M(4);
			List<int> list = new List<int>();
			for (int j = 0; j < Enumerable.Count<BambooColumn>(this.mBambooColumns); j++)
			{
				list.Add(j);
			}
			while (i > 0)
			{
				int num = SexyFramework.Common.Rand() % Enumerable.Count<int>(list);
				for (int k = 0; k < Common._M(20); k++)
				{
					BambooColumn bambooColumn = this.mBambooColumns[list[num]];
					bambooColumn.AddSmokeParticle(BambooTransition.SpawnSmokeParticle(bambooColumn.GetColumnX(), bambooColumn.GetCollisionY(), false, false));
				}
				list.RemoveAt(num);
				i--;
			}
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0008E680 File Offset: 0x0008C880
		public LevelTransition(int next_level_override, bool dont_record_stats)
		{
			if (!GameApp.gApp.mResourceManager.IsGroupLoaded("AdventureStats"))
			{
				GameApp.gApp.mResourceManager.LoadResources("AdventureStats");
			}
			this.mFrog = GameApp.gApp.GetBoard().GetGun();
			this.mFrogEffect = new FrogFlyOff();
			this.Reset(true);
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0008E6FB File Offset: 0x0008C8FB
		public LevelTransition(int next_level_override)
			: this(next_level_override, false)
		{
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0008E705 File Offset: 0x0008C905
		public LevelTransition()
			: this(-1, false)
		{
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0008E70F File Offset: 0x0008C90F
		public void Dispose()
		{
			GameApp.gApp.mResourceManager.DeleteResources("AdventureStats");
			this.mFrogEffect = null;
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0008E72C File Offset: 0x0008C92C
		public bool Update()
		{
			this.mTimer++;
			if (this.mDone)
			{
				return false;
			}
			if (Enumerable.Count<BambooColumn>(this.mBambooColumns) > 0)
			{
				for (int i = 0; i < Enumerable.Count<BambooColumn>(this.mBambooColumns); i++)
				{
					bool sound = false;
					if (i == Enumerable.Count<BambooColumn>(this.mBambooColumns) - 1)
					{
						sound = true;
					}
					this.mBambooColumns[i].Update(sound);
				}
			}
			if (this.mState == 0)
			{
				if (this.mIntro)
				{
					this.mFrogEffect.Update();
					for (int j = 0; j < Enumerable.Count<LTSmokeParticle>(this.mFrogSmoke); j++)
					{
						LTSmokeParticle s = this.mFrogSmoke[j];
						if (BambooTransition.UpdateSmokeParticle(s))
						{
							this.mFrogSmoke.RemoveAt(j);
							j--;
						}
					}
				}
				if ((this.mFrogEffect.mTimer >= this.mFrogEffect.mFrogJumpTime / 2 && this.mIntro) || !this.mIntro)
				{
					if (this.mFrogEffect.HasCompletedFlyOff() && Enumerable.Count<BambooColumn>(this.mBambooColumns) > 0)
					{
						for (int k = 0; k < Enumerable.Count<BambooColumn>(this.mBambooColumns); k++)
						{
							this.mBambooColumns[k].Close();
						}
					}
					this.mBGAlpha += 255f / (float)this.mBambooTime;
					if (this.mBGAlpha > 255f)
					{
						this.mBGAlpha = 255f;
					}
					bool flag = true;
					if (Enumerable.Count<BambooColumn>(this.mBambooColumns) > 0)
					{
						for (int l = 0; l < Enumerable.Count<BambooColumn>(this.mBambooColumns); l++)
						{
							flag &= this.mBambooColumns[l].IsClosed();
						}
					}
					if (flag)
					{
						this.mTimer = 0;
						this.mState++;
						if (GameApp.gApp.mBoard.mLevel.mNum != 10)
						{
							int mNum = GameApp.gApp.mBoard.mLevel.mNum;
							int num = GameApp.gApp.mBoard.mLevel.mZone - 1;
							int num2 = num;
							int index = mNum + num * 10 + num2;
							string text = GameApp.gApp.GetLevelMgr().GetLevelId(index);
							text = char.ToUpper(text[0]) + text.Substring(1);
							string theGroup = "Levels_" + text;
							if (!GameApp.gApp.mResourceManager.IsGroupLoaded(theGroup))
							{
								GameApp.gApp.mResourceManager.PrepareLoadResources(theGroup);
							}
						}
					}
				}
			}
			else
			{
				if (this.mState == 1)
				{
					if (++this.mDelay == Common._M(20))
					{
						this.mTimer = 0;
					}
					this.mFrogEffect.Update();
					if (GameApp.gApp.mBoard.mGameState == GameState.GameState_BossIntro && GameApp.gApp.mBoard.mBossIntroBGAlpha.GetOutVal() == 1.0)
					{
						this.mDone = true;
					}
					return this.mDelay == Common._M(19);
				}
				if (this.mState == 2)
				{
					this.mFrogEffect.Update();
					if ((!this.mIntro && this.mFrogEffect.mTimer >= this.mFrogEffect.mFrogJumpTime / 2) || this.mIntro)
					{
						this.mBGAlpha -= 255f / (float)this.mBambooTime;
						if (this.mBGAlpha < 0f)
						{
							this.mBGAlpha = 0f;
						}
						bool flag2 = true;
						if (Enumerable.Count<BambooColumn>(this.mBambooColumns) > 0)
						{
							for (int m = 0; m < Enumerable.Count<BambooColumn>(this.mBambooColumns); m++)
							{
								flag2 &= this.mBambooColumns[m].IsOpened();
							}
						}
						if (flag2 && (this.mIntro || this.mFrogEffect.mTimer >= this.mFrogEffect.mFrogJumpTime))
						{
							GameApp.gApp.mBoard.CueLevelTransition();
							this.mDone = true;
							if (!this.mIntro && this.mDrawFrogEffect)
							{
								for (int n = 0; n < Common._M(20); n++)
								{
									this.mFrog.mSmokeParticles.Add(BambooTransition.SpawnSmokeParticle((float)this.mFrog.GetCenterX(), (float)this.mFrog.GetCenterY(), false, true));
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0008EB8C File Offset: 0x0008CD8C
		public void DrawOverlay(Graphics g)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_LARGE_FROG);
			if (this.mDone)
			{
				return;
			}
			if (this.mBGAlpha > 0f)
			{
				g.SetColor(0, 0, 0, (int)this.mBGAlpha);
				g.FillRect(GameApp.gApp.GetScreenRect());
			}
			if (Enumerable.Count<BambooColumn>(this.mBambooColumns) > 0)
			{
				for (int i = 0; i < Enumerable.Count<BambooColumn>(this.mBambooColumns); i++)
				{
					this.mBambooColumns[i].Draw(g);
				}
				for (int j = 0; j < Enumerable.Count<BambooColumn>(this.mBambooColumns); j++)
				{
					this.mBambooColumns[j].DrawSmoke(g);
				}
			}
			if (((this.mState == 0 && this.mIntro) || (this.mState == 2 && !this.mIntro)) && this.mDrawFrogEffect && this.mFrogEffect.mFrogY + (float)(imageByID.mHeight / 2) + (float)Common._M(0) >= 0f)
			{
				if (this.mIntro)
				{
					for (int k = 0; k < Enumerable.Count<LTSmokeParticle>(this.mFrogSmoke); k++)
					{
						BambooTransition.DrawSmokeParticle(g, this.mFrogSmoke[k]);
					}
				}
				this.mFrogEffect.Draw(g);
			}
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0008ECC4 File Offset: 0x0008CEC4
		public void Draw(Graphics g)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_LARGE_FROG);
			if (this.mDone)
			{
				return;
			}
			if (this.mBGAlpha > 0f)
			{
				g.SetColor(0, 0, 0, (int)this.mBGAlpha);
				g.FillRect(GameApp.gApp.GetScreenRect());
			}
			if (Enumerable.Count<BambooColumn>(this.mBambooColumns) > 0)
			{
				for (int i = 0; i < Enumerable.Count<BambooColumn>(this.mBambooColumns); i++)
				{
					this.mBambooColumns[i].Draw(g);
				}
				for (int j = 0; j < Enumerable.Count<BambooColumn>(this.mBambooColumns); j++)
				{
					this.mBambooColumns[j].DrawSmoke(g);
				}
			}
			if (((this.mState == 0 && this.mIntro) || (this.mState == 2 && !this.mIntro)) && this.mDrawFrogEffect && this.mFrogEffect.mFrogY + (float)(imageByID.mHeight / 2) + (float)Common._M(0) >= 0f)
			{
				if (this.mIntro)
				{
					for (int k = 0; k < Enumerable.Count<LTSmokeParticle>(this.mFrogSmoke); k++)
					{
						BambooTransition.DrawSmokeParticle(g, this.mFrogSmoke[k]);
					}
				}
				this.mFrogEffect.Draw(g);
			}
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0008EDFC File Offset: 0x0008CFFC
		public void Reset(bool intro)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_BAMBOO_PIECE_A);
			this.mDrawFrogEffect = true;
			this.mIntro = intro;
			this.mState = 0;
			this.mDone = false;
			this.mDelay = 0;
			this.mIntroDelay = 0;
			this.mFrogSmoke.Clear();
			this.mSilent = false;
			if (this.mIntro)
			{
				this.mFrogEffect.JumpOut(this.mFrog);
				for (int i = 0; i < Common._M(20); i++)
				{
					this.mFrogSmoke.Add(BambooTransition.SpawnSmokeParticle(this.mFrogEffect.mFrogX, this.mFrogEffect.mFrogY, true, false));
				}
			}
			this.mBGAlpha = 0f;
			if (Enumerable.Count<BambooColumn>(this.mBambooColumns) == 0)
			{
				float num = (float)GameApp.gApp.GetScreenRect().mX - 10f;
				for (float num2 = num; num2 <= (float)GameApp.gApp.GetScreenRect().mWidth; num2 += (float)(imageByID.GetWidth() - Common._DS(19)))
				{
					this.mBambooColumns.Add(new BambooColumn());
					Enumerable.Last<BambooColumn>(this.mBambooColumns).SetColumnX(num2);
				}
			}
			else
			{
				for (int j = 0; j < Enumerable.Count<BambooColumn>(this.mBambooColumns); j++)
				{
					this.mBambooColumns[j].Reset();
				}
			}
			this.SetupBambooSmoke();
			this.mTimer = 0;
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0008EF57 File Offset: 0x0008D157
		public void RehupFrogPosition()
		{
			this.mFrogEffect.RehupFrogPosition(this.mFrog.GetCenterX(), this.mFrog.GetCenterY());
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x0008EF7C File Offset: 0x0008D17C
		public void Open()
		{
			this.mState = 2;
			if (Enumerable.Count<BambooColumn>(this.mBambooColumns) > 0)
			{
				for (int i = 0; i < Enumerable.Count<BambooColumn>(this.mBambooColumns); i++)
				{
					this.mBambooColumns[i].Open();
				}
			}
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0008EFC5 File Offset: 0x0008D1C5
		public bool IsDone()
		{
			return this.mDone;
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0008EFCD File Offset: 0x0008D1CD
		public int GetState()
		{
			return this.mState;
		}

		// Token: 0x04000D0B RID: 3339
		public List<LTSmokeParticle> mFrogSmoke = new List<LTSmokeParticle>();

		// Token: 0x04000D0C RID: 3340
		public FrogFlyOff mFrogEffect;

		// Token: 0x04000D0D RID: 3341
		public Gun mFrog;

		// Token: 0x04000D0E RID: 3342
		public int mBambooTime;

		// Token: 0x04000D0F RID: 3343
		public int mDelay;

		// Token: 0x04000D10 RID: 3344
		public int mState;

		// Token: 0x04000D11 RID: 3345
		public int mTimer;

		// Token: 0x04000D12 RID: 3346
		public bool mDone;

		// Token: 0x04000D13 RID: 3347
		public bool mIntro;

		// Token: 0x04000D14 RID: 3348
		public float mBGAlpha;

		// Token: 0x04000D15 RID: 3349
		public List<BambooColumn> mBambooColumns = new List<BambooColumn>();

		// Token: 0x04000D16 RID: 3350
		public int mIntroDelay;

		// Token: 0x04000D17 RID: 3351
		public int mNextLevelOverride;

		// Token: 0x04000D18 RID: 3352
		public bool mDontRecordStats;

		// Token: 0x04000D19 RID: 3353
		public bool mTransitionToStats;

		// Token: 0x04000D1A RID: 3354
		public bool mDrawFrogEffect;

		// Token: 0x04000D1B RID: 3355
		public bool mSilent;

		// Token: 0x04000D1C RID: 3356
		public bool mDidFirstBounce;

		// Token: 0x02000108 RID: 264
		public enum State
		{
			// Token: 0x04000D1E RID: 3358
			BambooClose,
			// Token: 0x04000D1F RID: 3359
			Delay,
			// Token: 0x04000D20 RID: 3360
			BambooOpen
		}
	}
}
