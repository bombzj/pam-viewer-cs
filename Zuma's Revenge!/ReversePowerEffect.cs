using System;
using System.Linq;
using JeffLib;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x020000B6 RID: 182
	public class ReversePowerEffect : PowerEffect
	{
		// Token: 0x06000A91 RID: 2705 RVA: 0x000673FE File Offset: 0x000655FE
		public ReversePowerEffect()
		{
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00067408 File Offset: 0x00065608
		public ReversePowerEffect(float x, float y, Ball b)
			: base(x, y)
		{
			this.mScale = 1f;
			this.mCurve = GameApp.gApp.GetBoard().GetCurve(b);
			this.mStartWaypoint = (this.mWaypoint = b.GetWayPoint());
			SexyVector2 pointPos = this.mCurve.mWayPointMgr.GetPointPos(this.mWaypoint);
			this.mX = pointPos.x;
			this.mY = pointPos.y;
			this.mRotation = this.mCurve.mWayPointMgr.GetRotationForPoint((int)this.mWaypoint);
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x000674A4 File Offset: 0x000656A4
		public override void Update()
		{
			if (this.IsDone())
			{
				return;
			}
			base.Update();
			if (!this.mDone)
			{
				return;
			}
			this.mWaypoint -= (float)Common._M(20);
			SexyVector2 pointPos = this.mCurve.mWayPointMgr.GetPointPos(this.mWaypoint);
			this.mX = pointPos.x;
			this.mY = pointPos.y;
			this.mRotation = this.mCurve.mWayPointMgr.GetRotationForPoint((int)this.mWaypoint);
			this.mScale = this.mWaypoint / this.mStartWaypoint;
			if (this.mScale < 0f)
			{
				this.mDone = true;
			}
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x00067558 File Offset: 0x00065758
		public override void Draw(Graphics g)
		{
			if (this.IsDone())
			{
				return;
			}
			g.PushState();
			g.SetColorizeImages(true);
			g.SetDrawMode(1);
			int num = (this.mDrawReverse ? (Enumerable.Count<EffectItem>(this.mItems) - 1) : 0);
			int num2 = (this.mDrawReverse ? 0 : Enumerable.Count<EffectItem>(this.mItems));
			int num3 = num;
			while (this.mDrawReverse ? (num3 >= num2) : (num3 < num2))
			{
				EffectItem effectItem = this.mItems[num3];
				SexyColor mColor = effectItem.mColor;
				mColor.mAlpha = (int)Component.GetComponentValue(effectItem.mOpacity, 255f, this.mUpdateCount);
				if (mColor.mAlpha != 0)
				{
					float num4 = (this.mDone ? this.mScale : Component.GetComponentValue(effectItem.mScale, 1f, this.mUpdateCount));
					g.SetColor(mColor);
					this.mGlobalTranform.Reset();
					this.mGlobalTranform.RotateRad(this.mRotation);
					this.mGlobalTranform.Scale(num4, num4);
					Rect celRect = effectItem.mImage.GetCelRect(effectItem.mCel);
					if (g.Is3D())
					{
						g.DrawImageTransformF(effectItem.mImage, this.mGlobalTranform, celRect, Common._S(this.mX), Common._S(this.mY));
					}
					else
					{
						g.DrawImageTransform(effectItem.mImage, this.mGlobalTranform, celRect, Common._S(this.mX), Common._S(this.mY));
					}
				}
				num3 += (this.mDrawReverse ? (-1) : 1);
			}
			g.PopState();
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x000676F3 File Offset: 0x000658F3
		public override bool IsDone()
		{
			return this.mDone && this.mWaypoint < 0f;
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0006770C File Offset: 0x0006590C
		public override void SyncState(DataSync sync)
		{
			base.SyncState(sync);
			sync.SyncFloat(ref this.mWaypoint);
			sync.SyncFloat(ref this.mStartWaypoint);
			sync.SyncFloat(ref this.mRotation);
			sync.SyncFloat(ref this.mScale);
			sync.SyncPointer(this);
		}

		// Token: 0x04000909 RID: 2313
		protected float mWaypoint;

		// Token: 0x0400090A RID: 2314
		protected float mStartWaypoint;

		// Token: 0x0400090B RID: 2315
		protected float mRotation;

		// Token: 0x0400090C RID: 2316
		protected float mScale;

		// Token: 0x0400090D RID: 2317
		public CurveMgr mCurve;
	}
}
