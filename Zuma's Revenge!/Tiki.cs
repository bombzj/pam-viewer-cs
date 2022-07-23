using System;
using SexyFramework.AELib;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x02000142 RID: 322
	public class Tiki : IDisposable
	{
		// Token: 0x06001005 RID: 4101 RVA: 0x000A0AC5 File Offset: 0x0009ECC5
		public Tiki()
		{
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x000A0AE7 File Offset: 0x0009ECE7
		public Tiki(Tiki rhs)
			: this()
		{
			this.CopyFrom(rhs);
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x000A0AF8 File Offset: 0x0009ECF8
		public void CopyFrom(Tiki rhs)
		{
			this.mUpdateCount = rhs.mUpdateCount;
			this.mCollRect = new Rect(rhs.mCollRect);
			this.mDoExplosion = rhs.mDoExplosion;
			this.mBoss = rhs.mBoss;
			this.mRailStartX = rhs.mRailStartX;
			this.mRailStartY = rhs.mRailStartY;
			this.mRailEndX = rhs.mRailEndX;
			this.mRailEndY = rhs.mRailEndY;
			this.mTravelTime = rhs.mTravelTime;
			this.mId = rhs.mId;
			this.mAlphaFadeDir = rhs.mAlphaFadeDir;
			this.mAlpha = rhs.mAlpha;
			this.mX = rhs.mX;
			this.mY = rhs.mY;
			this.mWasHit = rhs.mWasHit;
			this.mIsLeftTiki = rhs.mIsLeftTiki;
			this.mVX = rhs.mVX;
			this.mComp = rhs.mComp;
			this.mExplosion = rhs.mExplosion;
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x000A0BEE File Offset: 0x0009EDEE
		public virtual void Dispose()
		{
			if (this.mExplosion != null)
			{
				this.mExplosion.Dispose();
				this.mExplosion = null;
			}
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x000A0C0A File Offset: 0x0009EE0A
		public void Init(Boss b)
		{
			this.mBoss = b;
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x000A0C14 File Offset: 0x0009EE14
		public void Update()
		{
			if (this.mDoExplosion)
			{
				this.mExplosion.mDrawTransform.LoadIdentity();
				float num = GameApp.DownScaleNum(1f);
				this.mExplosion.mDrawTransform.Scale(num, num);
				this.mExplosion.mDrawTransform.Translate(Common._S(this.mX) + (float)Common._DS(Common._M(80)), Common._S(this.mY) + (float)Common._DS(Common._M1(150)));
				this.mExplosion.Update();
				if (this.mExplosion.mFrameNum > (float)this.mExplosion.mLastFrameNum)
				{
					this.mDoExplosion = false;
				}
			}
			this.mComp.Update();
			this.mAlpha += this.mAlphaFadeDir * Common._M(12);
			if (this.mAlpha < 0)
			{
				this.mAlpha = 0;
			}
			else if (this.mAlpha > 255)
			{
				this.mAlpha = 255;
			}
			if (!this.mDoExplosion && ((this.mVX > 0f && this.mX + (float)this.mCollRect.mX > (float)this.mRailEndX) || (this.mVX < 0f && this.mX + (float)this.mCollRect.mX < (float)this.mRailStartX)))
			{
				this.mX = (float)((this.mVX > 0f) ? (this.mRailEndX - this.mCollRect.mX) : (this.mRailStartX - this.mCollRect.mX));
				this.mVX *= -1f;
			}
			this.mX += this.mVX;
			this.mUpdateCount++;
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x000A0DE4 File Offset: 0x0009EFE4
		public void Draw(Graphics g)
		{
			if (this.mAlpha > 0)
			{
				CumulativeTransform cumulativeTransform = new CumulativeTransform();
				cumulativeTransform.mOpacity = (float)this.mAlpha / 255f;
				if (this.mBoss != null && this.mBoss.mAlphaOverride <= 254f)
				{
					cumulativeTransform.mOpacity = this.mBoss.mAlphaOverride / 255f;
				}
				cumulativeTransform.mTrans.Translate(Common._S(this.mX - (float)this.mCollRect.mX), Common._S(this.mY - (float)this.mCollRect.mY));
				this.mComp.Draw(g, cumulativeTransform, -1, Common._DS(1f));
			}
			if (g.Is3D() && this.mDoExplosion)
			{
				if (this.mBoss != null && this.mBoss.mAlphaOverride <= 254f)
				{
					this.mExplosion.mColor.mAlpha = (int)(this.mBoss.mAlphaOverride / 255f);
				}
				this.mExplosion.Draw(g);
			}
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x000A0EF4 File Offset: 0x0009F0F4
		public void SetIsLeft(bool l)
		{
			this.mIsLeftTiki = l;
			this.mCollRect = new Rect(Common._M(74), Common._M1(70), Common._M2(75), Common._M4(104));
			this.mExplosion = null;
			this.mExplosion = (this.mIsLeftTiki ? GameApp.gApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_CIRCLEEXPLOSIONTIKI").Duplicate() : GameApp.gApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_TRIANGLEEXPLOSIONTIKI").Duplicate());
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x000A0F78 File Offset: 0x0009F178
		public bool Collides(Bullet b, ref bool should_destroy)
		{
			Rect rect = new Rect(this.mCollRect);
			rect.mX = (int)this.mX;
			rect.mY = (int)this.mY;
			Rect theTRect = new Rect((int)b.GetX() - b.GetRadius(), (int)b.GetY() - b.GetRadius(), b.GetRadius() * 2, b.GetRadius() * 2);
			should_destroy = false;
			if (this.mWasHit || this.mAlphaFadeDir < 0 || !rect.Intersects(theTRect))
			{
				return false;
			}
			should_destroy = true;
			this.mWasHit = true;
			this.mAlphaFadeDir = -1;
			this.mDoExplosion = true;
			this.mExplosion.ResetAnim();
			return true;
		}

		// Token: 0x040016E8 RID: 5864
		protected int mUpdateCount;

		// Token: 0x040016E9 RID: 5865
		protected Rect mCollRect = default(Rect);

		// Token: 0x040016EA RID: 5866
		protected bool mDoExplosion;

		// Token: 0x040016EB RID: 5867
		protected Boss mBoss;

		// Token: 0x040016EC RID: 5868
		public int mRailStartX;

		// Token: 0x040016ED RID: 5869
		public int mRailStartY;

		// Token: 0x040016EE RID: 5870
		public int mRailEndX;

		// Token: 0x040016EF RID: 5871
		public int mRailEndY;

		// Token: 0x040016F0 RID: 5872
		public int mTravelTime;

		// Token: 0x040016F1 RID: 5873
		public int mId = -1;

		// Token: 0x040016F2 RID: 5874
		public int mAlphaFadeDir = 1;

		// Token: 0x040016F3 RID: 5875
		public int mAlpha;

		// Token: 0x040016F4 RID: 5876
		public float mX;

		// Token: 0x040016F5 RID: 5877
		public float mY;

		// Token: 0x040016F6 RID: 5878
		public bool mWasHit;

		// Token: 0x040016F7 RID: 5879
		public bool mIsLeftTiki;

		// Token: 0x040016F8 RID: 5880
		public float mVX;

		// Token: 0x040016F9 RID: 5881
		public Composition mComp;

		// Token: 0x040016FA RID: 5882
		public PIEffect mExplosion;
	}
}
