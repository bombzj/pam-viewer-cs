using System;
using System.Collections.Generic;
using System.Linq;
using SexyFramework;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x02000057 RID: 87
	public class BambooColumn
	{
		// Token: 0x060006E9 RID: 1769 RVA: 0x0002EE81 File Offset: 0x0002D081
		public BambooColumn()
		{
			this.Reset();
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0002EEB4 File Offset: 0x0002D0B4
		public void Reset()
		{
			this.IMAGE_BAMBOO_PIECE_A = Res.GetImageByID(ResID.IMAGE_BAMBOO_PIECE_A);
			this.IMAGE_BAMBOO_PIECE_B = Res.GetImageByID(ResID.IMAGE_BAMBOO_PIECE_B);
			this.IMAGE_BAMBOO_PIECE_C = Res.GetImageByID(ResID.IMAGE_BAMBOO_PIECE_C);
			this.IMAGE_BAMBOO_PIECE_D = Res.GetImageByID(ResID.IMAGE_BAMBOO_PIECE_D);
			this.mState = BambooColumn.BambooState.Init;
			float num = (float)GameApp.gApp.GetScreenRect().mHeight / 2f;
			float num2 = (float)(SexyFramework.Common.Rand() % Common._DS(400) - Common._DS(200));
			this.mTopEnd.mFinalY = num + num2;
			this.mTopEnd.mY = (float)(-(float)this.IMAGE_BAMBOO_PIECE_C.GetHeight());
			this.mTopEnd.mVelocityY = (this.mTopEnd.mFinalY - this.mTopEnd.mY) / 20f;
			this.mBotEnd.mFinalY = this.mTopEnd.mFinalY + (float)this.IMAGE_BAMBOO_PIECE_C.GetHeight();
			this.mBotEnd.mY = (float)(GameApp.gApp.GetScreenRect().mHeight + this.IMAGE_BAMBOO_PIECE_D.GetHeight());
			this.mBotEnd.mVelocityY = (this.mBotEnd.mFinalY - this.mBotEnd.mY) / 20f;
			this.mGravity = 0.1f;
			this.mSmoke.Clear();
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0002F014 File Offset: 0x0002D214
		public void Draw(Graphics g)
		{
			this.mDrawed = true;
			g.DrawImage(this.IMAGE_BAMBOO_PIECE_C, (int)(this.mX + (float)Common._DS(4)), (int)this.mTopEnd.mY);
			float num = this.mTopEnd.mY;
			bool flag = false;
			while (num >= 0f)
			{
				Image image;
				if (flag)
				{
					image = this.IMAGE_BAMBOO_PIECE_B;
					num -= (float)image.GetHeight();
				}
				else
				{
					image = this.IMAGE_BAMBOO_PIECE_A;
					num -= (float)image.GetHeight();
				}
				g.DrawImage(image, (int)this.mX, (int)num);
				flag = !flag;
			}
			g.DrawImage(this.IMAGE_BAMBOO_PIECE_D, (int)this.mX, (int)this.mBotEnd.mY);
			float num2 = this.mBotEnd.mY;
			flag = false;
			while (num2 <= (float)GameApp.gApp.GetScreenRect().mHeight)
			{
				Image image2;
				if (flag)
				{
					image2 = this.IMAGE_BAMBOO_PIECE_B;
					num2 += (float)image2.GetHeight();
				}
				else
				{
					image2 = this.IMAGE_BAMBOO_PIECE_A;
					num2 += (float)image2.GetHeight();
				}
				g.DrawImage(image2, (int)this.mX, (int)num2);
				flag = !flag;
			}
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0002F12C File Offset: 0x0002D32C
		public void DrawSmoke(Graphics g)
		{
			if (Enumerable.Count<LTSmokeParticle>(this.mSmoke) > 0)
			{
				for (int i = 0; i < Enumerable.Count<LTSmokeParticle>(this.mSmoke); i++)
				{
					BambooTransition.DrawSmokeParticle(g, this.mSmoke[i]);
				}
			}
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0002F170 File Offset: 0x0002D370
		public void Update(bool sound)
		{
			switch (this.mState)
			{
			case BambooColumn.BambooState.Falling:
				this.mTopEnd.mY = this.mTopEnd.mY + this.mTopEnd.mVelocityY;
				this.mBotEnd.mY = this.mBotEnd.mY + this.mBotEnd.mVelocityY;
				if (this.mTopEnd.mY + (float)this.IMAGE_BAMBOO_PIECE_C.GetHeight() >= this.mBotEnd.mY)
				{
					this.mTopEnd.mY = this.mBotEnd.mY - (float)this.IMAGE_BAMBOO_PIECE_C.GetHeight() - 1f;
					this.mState = BambooColumn.BambooState.Bouncing;
					if (sound)
					{
						this.PlayBambooSound(0.2f);
					}
				}
				break;
			case BambooColumn.BambooState.Bouncing:
			{
				float num = -(this.mTopEnd.mVelocityY / Common._M(10f) - this.mGravity);
				float num2 = -(this.mBotEnd.mVelocityY / Common._M(10f) + this.mGravity);
				this.mTopEnd.mY = this.mTopEnd.mY + num;
				this.mBotEnd.mY = this.mBotEnd.mY + num2;
				this.mGravity += 0.1f;
				if (this.mTopEnd.mY + (float)this.IMAGE_BAMBOO_PIECE_C.GetHeight() >= this.mBotEnd.mY)
				{
					this.mTopEnd.mY = this.mBotEnd.mY - (float)this.IMAGE_BAMBOO_PIECE_C.GetHeight() + (float)Common._DS(7);
					this.mState = BambooColumn.BambooState.Closed;
					if (sound)
					{
						this.PlayBambooSound(0.1f);
					}
				}
				break;
			}
			case BambooColumn.BambooState.Opening:
			{
				this.mTopEnd.mY = this.mTopEnd.mY - this.mTopEnd.mVelocityY;
				this.mBotEnd.mY = this.mBotEnd.mY - this.mBotEnd.mVelocityY;
				bool flag = this.mTopEnd.mY + (float)this.IMAGE_BAMBOO_PIECE_C.GetHeight() < -20f;
				bool flag2 = this.mBotEnd.mY >= (float)(GameApp.gApp.GetScreenRect().mHeight + 20);
				if (flag && flag2 && this.mDrawed)
				{
					this.mState = BambooColumn.BambooState.Open;
				}
				break;
			}
			}
			this.mDrawed = false;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0002F3CC File Offset: 0x0002D5CC
		public void UpdateSmokeParticle()
		{
			if (this.mState != BambooColumn.BambooState.Init && this.mState != BambooColumn.BambooState.Falling)
			{
				for (int i = 0; i < Enumerable.Count<LTSmokeParticle>(this.mSmoke); i++)
				{
					LTSmokeParticle s = this.mSmoke[i];
					if (BambooTransition.UpdateSmokeParticle(s))
					{
						this.mSmoke.RemoveAt(i);
						i--;
					}
				}
			}
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0002F425 File Offset: 0x0002D625
		public void SetColumnX(float theX)
		{
			this.mX = theX;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0002F42E File Offset: 0x0002D62E
		public void Close()
		{
			if (this.mState == BambooColumn.BambooState.Open)
			{
				this.Reset();
			}
			if (this.mState == BambooColumn.BambooState.Init)
			{
				this.mState = BambooColumn.BambooState.Falling;
			}
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0002F44E File Offset: 0x0002D64E
		public void Open()
		{
			if (this.mState == BambooColumn.BambooState.Closed)
			{
				this.mState = BambooColumn.BambooState.Opening;
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0002F460 File Offset: 0x0002D660
		public bool IsClosed()
		{
			return this.mState == BambooColumn.BambooState.Closed;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0002F46B File Offset: 0x0002D66B
		public bool IsOpened()
		{
			return this.mState == BambooColumn.BambooState.Open;
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0002F476 File Offset: 0x0002D676
		public float GetColumnX()
		{
			return this.mX;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0002F47E File Offset: 0x0002D67E
		public float GetCollisionY()
		{
			return this.mTopEnd.mFinalY;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0002F48B File Offset: 0x0002D68B
		public void AddSmokeParticle(LTSmokeParticle s)
		{
			this.mSmoke.Add(s);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0002F49C File Offset: 0x0002D69C
		private void PlayBambooSound(float inVolume)
		{
			SoundAttribs soundAttribs = new SoundAttribs();
			soundAttribs.volume = inVolume;
			GameApp.gApp.mSoundPlayer.Play(Res.GetSoundByID(ResID.SOUND_BAMBOO_CLOSE), soundAttribs);
		}

		// Token: 0x04000413 RID: 1043
		public const int BAMBOO_TRANSITION_FADE_TIME = 100;

		// Token: 0x04000414 RID: 1044
		public const int BAMBOO_TRANSITION_PAUSE_TIME = 100;

		// Token: 0x04000415 RID: 1045
		public const float BAMBOO_TRANSITION_FALL_TIME = 20f;

		// Token: 0x04000416 RID: 1046
		public const float BAMBOO_BOUNCE_GRAVITY = 0.1f;

		// Token: 0x04000417 RID: 1047
		public const int BAMBOO_CLOSE_UPDATE_WAIT_COUNT = 10;

		// Token: 0x04000418 RID: 1048
		public const float BAMBOO_V_DIV = 10f;

		// Token: 0x04000419 RID: 1049
		private BambooColumn.BambooEnd mTopEnd = default(BambooColumn.BambooEnd);

		// Token: 0x0400041A RID: 1050
		private BambooColumn.BambooEnd mBotEnd = default(BambooColumn.BambooEnd);

		// Token: 0x0400041B RID: 1051
		private BambooColumn.BambooState mState;

		// Token: 0x0400041C RID: 1052
		private float mX;

		// Token: 0x0400041D RID: 1053
		private float mGravity;

		// Token: 0x0400041E RID: 1054
		private List<LTSmokeParticle> mSmoke = new List<LTSmokeParticle>();

		// Token: 0x0400041F RID: 1055
		private Image IMAGE_BAMBOO_PIECE_A;

		// Token: 0x04000420 RID: 1056
		private Image IMAGE_BAMBOO_PIECE_B;

		// Token: 0x04000421 RID: 1057
		private Image IMAGE_BAMBOO_PIECE_C;

		// Token: 0x04000422 RID: 1058
		private Image IMAGE_BAMBOO_PIECE_D;

		// Token: 0x04000423 RID: 1059
		private bool mDrawed;

		// Token: 0x02000058 RID: 88
		private enum BambooState
		{
			// Token: 0x04000425 RID: 1061
			Init,
			// Token: 0x04000426 RID: 1062
			Falling,
			// Token: 0x04000427 RID: 1063
			Bouncing,
			// Token: 0x04000428 RID: 1064
			Closed,
			// Token: 0x04000429 RID: 1065
			Opening,
			// Token: 0x0400042A RID: 1066
			Open
		}

		// Token: 0x02000059 RID: 89
		private struct BambooEnd
		{
			// Token: 0x0400042B RID: 1067
			public float mY;

			// Token: 0x0400042C RID: 1068
			public float mFinalY;

			// Token: 0x0400042D RID: 1069
			public float mVelocityY;
		}
	}
}
