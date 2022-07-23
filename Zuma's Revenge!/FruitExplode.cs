using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x020000AA RID: 170
	public class FruitExplode
	{
		// Token: 0x06000A45 RID: 2629 RVA: 0x00061AB6 File Offset: 0x0005FCB6
		public FruitExplode(Board board)
		{
			this.mBoard = board;
			this.mAnim = null;
			this.Reset();
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00061ADD File Offset: 0x0005FCDD
		public virtual void Dispose()
		{
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00061AE0 File Offset: 0x0005FCE0
		public void Reset()
		{
			this.mDone = false;
			if (this.mBoard.mLevel == null || this.mBoard.mCurTreasure == null)
			{
				return;
			}
			switch (this.mBoard.mLevel.mZone)
			{
			case 1:
				this.mAnim = Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_PINEAPPLEMUSH);
				break;
			case 2:
				this.mAnim = Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_BANANAMUSH);
				break;
			case 3:
				this.mAnim = Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_COCOAMUSH);
				break;
			case 4:
				this.mAnim = Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_MANGOMUSH);
				break;
			case 5:
				this.mAnim = Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_COCONUTMUSH);
				break;
			case 6:
			case 7:
				this.mAnim = Res.GetPopAnimByID(ResID.POPANIM_NONRESIZE_ACORNMUSH);
				break;
			default:
				this.mAnim = null;
				break;
			}
			this.mAnim.Play("Main");
			int num = (int)Common._S((float)this.mBoard.mCurTreasure.x + ModVal.M(-130f));
			int num2 = (int)Common._S((float)this.mBoard.mCurTreasure.y + ModVal.M(-120f));
			this.mGlobalTranform.Reset();
			this.mGlobalTranform.Translate((float)num, (float)num2);
			this.mAnim.SetTransform(this.mGlobalTranform.GetMatrix());
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00061C40 File Offset: 0x0005FE40
		public void Update()
		{
			if (this.mDone || this.mAnim == null || this.mBoard.mCurTreasure == null)
			{
				return;
			}
			int num = (int)Common._S((float)this.mBoard.mCurTreasure.x + ModVal.M(-130f));
			int num2 = (int)Common._S((float)this.mBoard.mCurTreasure.y + ModVal.M(-120f));
			this.mGlobalTranform.Reset();
			this.mGlobalTranform.Translate((float)num, (float)num2);
			this.mAnim.SetTransform(this.mGlobalTranform.GetMatrix());
			this.mAnim.Update();
			if (!this.mAnim.IsActive() || this.mAnim.mMainSpriteInst.mFrameNum >= (float)(this.mAnim.mMainSpriteInst.mDef.mFrames.Count - 1))
			{
				this.mDone = true;
			}
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00061D2F File Offset: 0x0005FF2F
		public void Draw(Graphics g)
		{
			if (this.mAnim != null)
			{
				this.mAnim.Draw(g);
			}
		}

		// Token: 0x040008AD RID: 2221
		protected PopAnim mAnim;

		// Token: 0x040008AE RID: 2222
		protected Board mBoard;

		// Token: 0x040008AF RID: 2223
		protected Transform mGlobalTranform = new Transform();

		// Token: 0x040008B0 RID: 2224
		public bool mDone;
	}
}
