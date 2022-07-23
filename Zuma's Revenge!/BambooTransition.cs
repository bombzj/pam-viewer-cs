using System;
using System.Collections.Generic;
using System.Linq;
using JeffLib;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x0200005A RID: 90
	public class BambooTransition : Widget
	{
		// Token: 0x060006F8 RID: 1784 RVA: 0x0002F4D0 File Offset: 0x0002D6D0
		public BambooTransition()
		{
			this.Reset();
			this.mZOrder = int.MaxValue;
			this.mUpdateNum = 0;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0002F4FC File Offset: 0x0002D6FC
		public void Reset()
		{
			this.IMAGE_BAMBOO_PIECE_A = Res.GetImageByID(ResID.IMAGE_BAMBOO_PIECE_A);
			this.IMAGE_BAMBOO_PIECE_B = Res.GetImageByID(ResID.IMAGE_BAMBOO_PIECE_B);
			this.IMAGE_BAMBOO_PIECE_C = Res.GetImageByID(ResID.IMAGE_BAMBOO_PIECE_C);
			this.IMAGE_BAMBOO_PIECE_D = Res.GetImageByID(ResID.IMAGE_BAMBOO_PIECE_D);
			this.mState = BambooTransition.BambooTransitionState.BAMBOO_TRANSITION_INIT;
			this.mBambooCloseWaitCount = 0;
			this.mLoadStartTime = ulong.MaxValue;
			if (Enumerable.Count<BambooColumn>(this.mBambooColumns) == 0)
			{
				float num = (float)GameApp.gApp.GetScreenRect().mX - 10f;
				for (float num2 = num; num2 <= (float)GameApp.gApp.GetScreenRect().mWidth; num2 += (float)(this.IMAGE_BAMBOO_PIECE_A.GetWidth() - ZumasRevenge.Common._DS(19)))
				{
					this.mBambooColumns.Add(new BambooColumn());
					Enumerable.Last<BambooColumn>(this.mBambooColumns).SetColumnX(num2);
				}
			}
			else
			{
				for (int i = 0; i < Enumerable.Count<BambooColumn>(this.mBambooColumns); i++)
				{
					this.mBambooColumns[i].Reset();
				}
			}
			this.SetupBambooSmoke();
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0002F601 File Offset: 0x0002D801
		public override void Draw(Graphics g)
		{
			if (this.mState != BambooTransition.BambooTransitionState.BAMBOO_TRANSITION_INIT)
			{
				base.DeferOverlay(10);
			}
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0002F614 File Offset: 0x0002D814
		public override void DrawOverlay(Graphics g)
		{
			int alpha = (int)(255f * (this.mFadeCount / 40f));
			g.SetColor(0, 0, 0, alpha);
			g.FillRect(GameApp.gApp.GetScreenRect().mX, GameApp.gApp.GetScreenRect().mY, GameApp.gApp.GetScreenRect().mWidth, GameApp.gApp.GetScreenRect().mHeight);
			if (this.mBambooColumns.Count > 0)
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
			this.mUpdateNum = 0;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0002F6E8 File Offset: 0x0002D8E8
		public override void Update()
		{
			this.mUpdateNum++;
			for (int i = 0; i < Enumerable.Count<BambooColumn>(this.mBambooColumns); i++)
			{
				this.mBambooColumns[i].UpdateSmokeParticle();
			}
			if (this.mUpdateNum > 2)
			{
				return;
			}
			for (int j = 0; j < Enumerable.Count<BambooColumn>(this.mBambooColumns); j++)
			{
				bool sound = false;
				if (j == Enumerable.Count<BambooColumn>(this.mBambooColumns) - 1)
				{
					sound = true;
				}
				this.mBambooColumns[j].Update(sound);
			}
			switch (this.mState)
			{
			case BambooTransition.BambooTransitionState.BAMBOO_TRANSITION_CLOSING:
			{
				if (this.mFadeCount < 40f)
				{
					this.mFadeCount += 1UL;
				}
				bool flag = true;
				if (Enumerable.Count<BambooColumn>(this.mBambooColumns) > 0)
				{
					for (int k = 0; k < Enumerable.Count<BambooColumn>(this.mBambooColumns); k++)
					{
						flag &= this.mBambooColumns[k].IsClosed();
					}
				}
				if (flag)
				{
					this.mBambooCloseWaitCount++;
					if (this.mBambooCloseWaitCount >= 10)
					{
						this.mState = BambooTransition.BambooTransitionState.BAMBOO_TRANSITION_CLOSED;
						return;
					}
				}
				break;
			}
			case BambooTransition.BambooTransitionState.BAMBOO_TRANSITION_CLOSED:
				if (this.mTransitionDelegate != null)
				{
					this.mTransitionDelegate();
				}
				this.mFadeCount = 40UL;
				this.mState = BambooTransition.BambooTransitionState.BAMBOO_TRANSITION_PAUSE;
				this.mLoadStartTime = (ulong)SexyFramework.Common.SexyTime();
				return;
			case BambooTransition.BambooTransitionState.BAMBOO_TRANSITION_PAUSE:
			{
				ulong num = (ulong)SexyFramework.Common.SexyTime() - this.mLoadStartTime;
				if (num >= 100UL)
				{
					this.mState = BambooTransition.BambooTransitionState.BAMBOO_TRANSITION_OPENING;
					if (Enumerable.Count<BambooColumn>(this.mBambooColumns) > 0)
					{
						for (int l = 0; l < Enumerable.Count<BambooColumn>(this.mBambooColumns); l++)
						{
							this.mBambooColumns[l].Open();
						}
						return;
					}
				}
				break;
			}
			case BambooTransition.BambooTransitionState.BAMBOO_TRANSITION_OPENING:
			{
				if (this.mFadeCount > 0UL)
				{
					this.mFadeCount -= 1UL;
				}
				bool flag2 = true;
				if (Enumerable.Count<BambooColumn>(this.mBambooColumns) > 0)
				{
					for (int m = 0; m < Enumerable.Count<BambooColumn>(this.mBambooColumns); m++)
					{
						flag2 &= this.mBambooColumns[m].IsOpened();
					}
				}
				if (flag2)
				{
					this.mState = BambooTransition.BambooTransitionState.BAMBOO_TRANSITION_OPEN;
					return;
				}
				break;
			}
			case BambooTransition.BambooTransitionState.BAMBOO_TRANSITION_OPEN:
				this.mState = BambooTransition.BambooTransitionState.BAMBOO_TRANSITION_INIT;
				GameApp.gApp.BambooTransitionOpened();
				break;
			default:
				return;
			}
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0002F924 File Offset: 0x0002DB24
		public void StartTransition()
		{
			if (this.mState != BambooTransition.BambooTransitionState.BAMBOO_TRANSITION_INIT)
			{
				Console.WriteLine("\n >>>>> WARNING: Attempting to start bamboo transition while a transition is occurring\n ");
				return;
			}
			this.mFadeCount = 0UL;
			this.mState = BambooTransition.BambooTransitionState.BAMBOO_TRANSITION_CLOSING;
			if (Enumerable.Count<BambooColumn>(this.mBambooColumns) > 0)
			{
				for (int i = 0; i < Enumerable.Count<BambooColumn>(this.mBambooColumns); i++)
				{
					this.mBambooColumns[i].Close();
				}
			}
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0002F988 File Offset: 0x0002DB88
		public bool IsInProgress()
		{
			return this.mState != BambooTransition.BambooTransitionState.BAMBOO_TRANSITION_INIT;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0002F998 File Offset: 0x0002DB98
		private void SetupBambooSmoke()
		{
			int i = ZumasRevenge.Common._M(4);
			List<int> list = new List<int>();
			for (int j = 0; j < Enumerable.Count<BambooColumn>(this.mBambooColumns); j++)
			{
				list.Add(j);
			}
			while (i > 0)
			{
				int num = SexyFramework.Common.Rand() % Enumerable.Count<int>(list);
				for (int k = 0; k < ZumasRevenge.Common._M(20); k++)
				{
					BambooColumn bambooColumn = this.mBambooColumns[list[num]];
					bambooColumn.AddSmokeParticle(BambooTransition.SpawnSmokeParticle(bambooColumn.GetColumnX(), bambooColumn.GetCollisionY(), false, false));
				}
				list.RemoveAt(num);
				i--;
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0002FA33 File Offset: 0x0002DC33
		public static LTSmokeParticle SpawnSmokeParticle(float x, float y)
		{
			return BambooTransition.SpawnSmokeParticle(x, y, false);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0002FA3D File Offset: 0x0002DC3D
		public static LTSmokeParticle SpawnSmokeParticle(float x, float y, bool fast)
		{
			return BambooTransition.SpawnSmokeParticle(x, y, fast, false);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0002FA48 File Offset: 0x0002DC48
		public static LTSmokeParticle SpawnSmokeParticle(float x, float y, bool fast, bool slow_fade)
		{
			LTSmokeParticle ltsmokeParticle = new LTSmokeParticle();
			ltsmokeParticle.mX = x;
			ltsmokeParticle.mY = y;
			ltsmokeParticle.mFadingIn = true;
			ltsmokeParticle.mSize = MathUtils.FloatRange(ZumasRevenge.Common._M(0.22f), ZumasRevenge.Common._M1(0.45f));
			float num = (fast ? MathUtils.FloatRange(ZumasRevenge.Common._M(1.5f), ZumasRevenge.Common._M1(2.5f)) : MathUtils.FloatRange(ZumasRevenge.Common._M2(0.75f), ZumasRevenge.Common._M3(1.5f)));
			float num2 = MathUtils.FloatRange(0f, 6.2831855f);
			ltsmokeParticle.mVX = num * (float)Math.Cos((double)num2);
			ltsmokeParticle.mVY = -num * (float)Math.Sin((double)num2);
			ltsmokeParticle.mAlpha.mColor = new FColor(0f, 0f, 0f, 0f);
			ltsmokeParticle.mAlpha.mFadeRate = (float)MathUtils.IntRange(ZumasRevenge.Common._M(10), ZumasRevenge.Common._M1(20));
			ltsmokeParticle.mAlphaFadeOutTime = (slow_fade ? MathUtils.IntRange(ZumasRevenge.Common._M(50), ZumasRevenge.Common._M1(75)) : MathUtils.IntRange(ZumasRevenge.Common._M2(10), ZumasRevenge.Common._M3(20)));
			if (SexyFramework.Common.Rand() % 100 == 0)
			{
				ltsmokeParticle.mColorFader.mColor = (ltsmokeParticle.mColorFader.mMinColor = new FColor(249f, 255f, 249f));
				ltsmokeParticle.mColorFader.mMaxColor = new FColor(205f, 208f, 148f);
			}
			else
			{
				ltsmokeParticle.mColorFader.mColor = (ltsmokeParticle.mColorFader.mMinColor = new FColor(212f, 217f, 212f));
				ltsmokeParticle.mColorFader.mMaxColor = new FColor(153f, 148f, 99f);
			}
			ltsmokeParticle.mColorFader.FadeOverTime((int)((float)ltsmokeParticle.mAlphaFadeOutTime + 255f / ltsmokeParticle.mAlpha.mFadeRate));
			return ltsmokeParticle;
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0002FC38 File Offset: 0x0002DE38
		public static void DrawSmokeParticle(Graphics g, LTSmokeParticle s)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_PARTICLE_FUZZ);
			g.SetColorizeImages(true);
			SexyColor color = s.mColorFader.mColor.ToColor();
			color.mAlpha = (int)s.mAlpha.mColor.mAlpha;
			g.SetColor(color);
			g.DrawImage(imageByID, (int)ZumasRevenge.Common._S(s.mX), (int)ZumasRevenge.Common._S(s.mY), (int)((float)imageByID.mWidth * s.mSize), (int)((float)imageByID.mHeight * s.mSize));
			g.SetColorizeImages(false);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0002FCCC File Offset: 0x0002DECC
		public static bool UpdateSmokeParticle(LTSmokeParticle s)
		{
			s.mAlpha.Update();
			s.mColorFader.Update();
			s.mX += s.mVX;
			s.mY += s.mVY;
			if (!s.mFadingIn && s.mAlpha.mColor.mAlpha <= 0f)
			{
				return true;
			}
			if (s.mAlpha.mColor.mAlpha == (float)s.mAlpha.mMax && s.mFadingIn)
			{
				s.mFadingIn = false;
				s.mAlpha.mMin = 0;
				s.mAlpha.mFadeRate = -255f / (float)s.mAlphaFadeOutTime;
			}
			return false;
		}

		// Token: 0x0400042E RID: 1070
		private BambooTransition.BambooTransitionState mState;

		// Token: 0x0400042F RID: 1071
		private List<BambooColumn> mBambooColumns = new List<BambooColumn>();

		// Token: 0x04000430 RID: 1072
		private ulong mLoadStartTime;

		// Token: 0x04000431 RID: 1073
		private ulong mFadeCount;

		// Token: 0x04000432 RID: 1074
		private int mBambooCloseWaitCount;

		// Token: 0x04000433 RID: 1075
		public BambooTransition.BambooTransitionDelegate mTransitionDelegate;

		// Token: 0x04000434 RID: 1076
		private Image IMAGE_BAMBOO_PIECE_A;

		// Token: 0x04000435 RID: 1077
		private Image IMAGE_BAMBOO_PIECE_B;

		// Token: 0x04000436 RID: 1078
		private Image IMAGE_BAMBOO_PIECE_C;

		// Token: 0x04000437 RID: 1079
		private Image IMAGE_BAMBOO_PIECE_D;

		// Token: 0x04000438 RID: 1080
		private int mUpdateNum;

		// Token: 0x0200005B RID: 91
		private enum BambooTransitionState
		{
			// Token: 0x0400043A RID: 1082
			BAMBOO_TRANSITION_INIT,
			// Token: 0x0400043B RID: 1083
			BAMBOO_TRANSITION_CLOSING,
			// Token: 0x0400043C RID: 1084
			BAMBOO_TRANSITION_CLOSED,
			// Token: 0x0400043D RID: 1085
			BAMBOO_TRANSITION_PAUSE,
			// Token: 0x0400043E RID: 1086
			BAMBOO_TRANSITION_OPENING,
			// Token: 0x0400043F RID: 1087
			BAMBOO_TRANSITION_OPEN,
			// Token: 0x04000440 RID: 1088
			NUM_BAMBOO_TRANSITION_STATES
		}

		// Token: 0x0200005C RID: 92
		// (Invoke) Token: 0x06000706 RID: 1798
		public delegate void BambooTransitionDelegate();
	}
}
