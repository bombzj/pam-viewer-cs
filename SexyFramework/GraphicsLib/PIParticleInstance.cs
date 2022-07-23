using System;
using Microsoft.Xna.Framework;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000E1 RID: 225
	public class PIParticleInstance : IDisposable
	{
		// Token: 0x060006AD RID: 1709 RVA: 0x0001C86C File Offset: 0x0001AA6C
		public PIParticleInstance()
		{
			this.mPrev = null;
			this.mNext = null;
			this.mTransformScaleFactor = 1f;
			this.mImgIdx = 0;
			this.mBkgColor = uint.MaxValue;
			this.mSrcSizeXMult = 1f;
			this.mSrcSizeYMult = 1f;
			this.mParentFreeEmitter = null;
			this.mHasDrawn = false;
			PIParticleInstance.mCount++;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0001C92C File Offset: 0x0001AB2C
		public void Reset()
		{
			this.mPos.X = 0f;
			this.mPos.Y = 0f;
			this.mPrev = null;
			this.mNext = null;
			this.mParticleDef = null;
			this.mEmitterSrc = null;
			this.mParentFreeEmitter = null;
			this.mTransformScaleFactor = 1f;
			this.mImgIdx = 0;
			this.mBkgColor = uint.MaxValue;
			this.mSrcSizeXMult = 1f;
			this.mSrcSizeYMult = 1f;
			this.mParentFreeEmitter = null;
			this.mHasDrawn = false;
			this.mTicks = 0f;
			this.mLife = 0f;
			this.mLifePct = 0f;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0001C9DA File Offset: 0x0001ABDA
		public void Init()
		{
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0001C9DC File Offset: 0x0001ABDC
		public void Release()
		{
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0001C9DE File Offset: 0x0001ABDE
		public virtual void Dispose()
		{
			PIParticleInstance.mCount--;
		}

		// Token: 0x040005E6 RID: 1510
		public PIParticleInstance mPrev;

		// Token: 0x040005E7 RID: 1511
		public PIParticleInstance mNext;

		// Token: 0x040005E8 RID: 1512
		public PIParticleDef mParticleDef;

		// Token: 0x040005E9 RID: 1513
		public PIEmitter mEmitterSrc;

		// Token: 0x040005EA RID: 1514
		public int mNum;

		// Token: 0x040005EB RID: 1515
		public PIFreeEmitterInstance mParentFreeEmitter;

		// Token: 0x040005EC RID: 1516
		public Vector2 mPos = default(Vector2);

		// Token: 0x040005ED RID: 1517
		public Vector2 mOrigPos = default(Vector2);

		// Token: 0x040005EE RID: 1518
		public Vector2 mEmittedPos = default(Vector2);

		// Token: 0x040005EF RID: 1519
		public Vector2 mLastEmitterPos = default(Vector2);

		// Token: 0x040005F0 RID: 1520
		public Vector2 mVel = default(Vector2);

		// Token: 0x040005F1 RID: 1521
		public float mImgAngle;

		// Token: 0x040005F2 RID: 1522
		public float[] mVariationValues = new float[9];

		// Token: 0x040005F3 RID: 1523
		public float mZoom;

		// Token: 0x040005F4 RID: 1524
		public float mSrcSizeXMult;

		// Token: 0x040005F5 RID: 1525
		public float mSrcSizeYMult;

		// Token: 0x040005F6 RID: 1526
		public float mGradientRand;

		// Token: 0x040005F7 RID: 1527
		public float mOrigEmitterAng;

		// Token: 0x040005F8 RID: 1528
		public int mAnimFrameRand;

		// Token: 0x040005F9 RID: 1529
		public SexyTransform2D mTransform = new SexyTransform2D(false);

		// Token: 0x040005FA RID: 1530
		public float mTransformScaleFactor;

		// Token: 0x040005FB RID: 1531
		public int mImgIdx;

		// Token: 0x040005FC RID: 1532
		public float mThicknessHitVariation;

		// Token: 0x040005FD RID: 1533
		public float mTicks;

		// Token: 0x040005FE RID: 1534
		public float mLife;

		// Token: 0x040005FF RID: 1535
		public float mLifePct;

		// Token: 0x04000600 RID: 1536
		public bool mHasDrawn;

		// Token: 0x04000601 RID: 1537
		public uint mBkgColor;

		// Token: 0x04000602 RID: 1538
		public static int mCount;

		// Token: 0x020000E2 RID: 226
		public enum PIParticleVariation
		{
			// Token: 0x04000604 RID: 1540
			VARIATION_LIFE,
			// Token: 0x04000605 RID: 1541
			VARIATION_SIZE_X,
			// Token: 0x04000606 RID: 1542
			VARIATION_SIZE_Y,
			// Token: 0x04000607 RID: 1543
			VARIATION_VELOCITY,
			// Token: 0x04000608 RID: 1544
			VARIATION_WEIGHT,
			// Token: 0x04000609 RID: 1545
			VARIATION_SPIN,
			// Token: 0x0400060A RID: 1546
			VARIATION_MOTION_RAND,
			// Token: 0x0400060B RID: 1547
			VARIATION_BOUNCE,
			// Token: 0x0400060C RID: 1548
			VARIATION_ZOOM,
			// Token: 0x0400060D RID: 1549
			NUM_VARIATIONS
		}
	}
}
