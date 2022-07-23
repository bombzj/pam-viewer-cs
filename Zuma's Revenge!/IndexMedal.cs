using System;
using JeffLib;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000046 RID: 70
	public class IndexMedal : ButtonWidget
	{
		// Token: 0x06000667 RID: 1639 RVA: 0x0002AFF4 File Offset: 0x000291F4
		public IndexMedal(bool theIsAced, int theId, ButtonListener theButtonListener)
			: base(theId, theButtonListener)
		{
			this.mIsAced = theIsAced;
			SexyFramework.Common.SRand(SexyFramework.Common.SexyTime());
			for (int i = 0; i < IndexMedal.MAX_NUM_BUTTON_SPARKLES; i++)
			{
				this.mSparkles[i].mEffect = null;
				this.mSparkles[i].mOffsetX = -1f;
				this.mSparkles[i].mOffsetY = -1f;
			}
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0002B078 File Offset: 0x00029278
		public override void Dispose()
		{
			base.Dispose();
			for (int i = 0; i < IndexMedal.MAX_NUM_BUTTON_SPARKLES; i++)
			{
				PIEffect mEffect = this.mSparkles[i].mEffect;
				if (mEffect != null)
				{
					mEffect.Dispose();
				}
				this.mSparkles[i].mEffect = null;
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0002B0C8 File Offset: 0x000292C8
		public void FindRandomOffsetsInRadius(float theRadius, ref float theOffsetX, ref float theOffsetY)
		{
			float num = (float)SexyFramework.Common.Rand() / (float)QRand.RAND_MAX * (theRadius * 0.9f);
			int num2 = ((SexyFramework.Common.Rand() % 2 == 0) ? (-1) : 1);
			float num3 = (float)SexyFramework.Common.Rand() / (float)QRand.RAND_MAX * 6.2831855f;
			float num4 = num * (float)Math.Cos((double)num3) * (float)num2;
			float num5 = num * (float)Math.Sin((double)num3) * (float)num2;
			theOffsetX = theRadius + num4;
			theOffsetY = theRadius + num5;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0002B138 File Offset: 0x00029338
		public override void Update()
		{
			base.Update();
			if (!this.mIsAced)
			{
				return;
			}
			for (int i = 0; i < IndexMedal.MAX_NUM_BUTTON_SPARKLES; i++)
			{
				PIEffect mEffect = this.mSparkles[i].mEffect;
				if (mEffect != null)
				{
					mEffect.mDrawTransform.LoadIdentity();
					mEffect.mDrawTransform.Scale(ZumasRevenge.Common._DS(1.4f), ZumasRevenge.Common._DS(1.4f));
					mEffect.mDrawTransform.Translate(this.mSparkles[i].mOffsetX, this.mSparkles[i].mOffsetY);
					mEffect.Update();
					if (SexyFramework.Common.Rand(500) == 0 && mEffect.mCurNumParticles == 0 && MathUtils._geq(mEffect.mFrameNum, (float)mEffect.mLastFrameNum))
					{
						mEffect.ResetAnim();
						mEffect.mRandSeeds.Clear();
						mEffect.mRandSeeds.Add(SexyFramework.Common.Rand(1000));
						this.FindRandomOffsetsInRadius((float)this.mButtonImage.mWidth / 2f, ref this.mSparkles[i].mOffsetX, ref this.mSparkles[i].mOffsetY);
					}
				}
				else if (SexyFramework.Common.Rand(500) == 0)
				{
					this.mSparkles[i].mEffect = GameApp.gApp.mResourceManager.GetPIEffect("PIEFFECT_MM_SPARKLE").Duplicate();
					this.mSparkles[i].mEffect.mEmitAfterTimeline = false;
					ZumasRevenge.Common.SetFXNumScale(this.mSparkles[i].mEffect, 3f);
					this.FindRandomOffsetsInRadius((float)this.mButtonImage.mWidth / 2f, ref this.mSparkles[i].mOffsetX, ref this.mSparkles[i].mOffsetY);
				}
			}
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0002B31C File Offset: 0x0002951C
		public override void Draw(Graphics g)
		{
			base.Draw(g);
			if (!this.mIsAced)
			{
				return;
			}
			for (int i = 0; i < IndexMedal.MAX_NUM_BUTTON_SPARKLES; i++)
			{
				PIEffect mEffect = this.mSparkles[i].mEffect;
				if (mEffect != null)
				{
					mEffect.Draw(g);
				}
			}
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0002B368 File Offset: 0x00029568
		public void SetAced()
		{
			this.mIsAced = true;
			for (int i = 0; i < IndexMedal.MAX_NUM_BUTTON_SPARKLES; i++)
			{
				this.mSparkles[i].mEffect = GameApp.gApp.mResourceManager.GetPIEffect("PIEFFECT_MM_SPARKLE").Duplicate();
				this.mSparkles[i].mEffect.mEmitAfterTimeline = true;
				ZumasRevenge.Common.SetFXNumScale(this.mSparkles[i].mEffect, 3f);
				this.FindRandomOffsetsInRadius((float)this.mButtonImage.mWidth / 2f, ref this.mSparkles[i].mOffsetX, ref this.mSparkles[i].mOffsetY);
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0002B428 File Offset: 0x00029628
		public void Init()
		{
			if (!this.mIsAced)
			{
				return;
			}
			for (int i = 0; i < IndexMedal.MAX_NUM_BUTTON_SPARKLES; i++)
			{
				this.FindRandomOffsetsInRadius((float)this.mButtonImage.mWidth / 2f, ref this.mSparkles[i].mOffsetX, ref this.mSparkles[i].mOffsetY);
			}
		}

		// Token: 0x04000386 RID: 902
		private static int MAX_NUM_BUTTON_SPARKLES = 2;

		// Token: 0x04000387 RID: 903
		public IndexMedal.AceSparkle[] mSparkles = new IndexMedal.AceSparkle[IndexMedal.MAX_NUM_BUTTON_SPARKLES];

		// Token: 0x04000388 RID: 904
		public bool mIsAced;

		// Token: 0x04000389 RID: 905
		public float mRadius;

		// Token: 0x02000047 RID: 71
		public struct AceSparkle
		{
			// Token: 0x0400038A RID: 906
			public PIEffect mEffect;

			// Token: 0x0400038B RID: 907
			public float mOffsetX;

			// Token: 0x0400038C RID: 908
			public float mOffsetY;
		}
	}
}
