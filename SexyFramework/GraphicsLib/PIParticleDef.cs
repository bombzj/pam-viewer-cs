using System;
using Microsoft.Xna.Framework;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000DD RID: 221
	public class PIParticleDef
	{
		// Token: 0x060006AB RID: 1707 RVA: 0x0001C7C8 File Offset: 0x0001A9C8
		public PIParticleDef()
		{
			this.mValues = new PIValue[28];
			this.mRefPointOfs = default(Vector2);
			for (int i = 0; i < 28; i++)
			{
				this.mValues[i] = new PIValue();
			}
		}

		// Token: 0x04000572 RID: 1394
		public PIEmitter mParent;

		// Token: 0x04000573 RID: 1395
		public string mName;

		// Token: 0x04000574 RID: 1396
		public int mTextureIdx;

		// Token: 0x04000575 RID: 1397
		public PIValue[] mValues;

		// Token: 0x04000576 RID: 1398
		public Vector2 mRefPointOfs;

		// Token: 0x04000577 RID: 1399
		public bool mLockAspect;

		// Token: 0x04000578 RID: 1400
		public bool mIntense;

		// Token: 0x04000579 RID: 1401
		public bool mSingleParticle;

		// Token: 0x0400057A RID: 1402
		public bool mPreserveColor;

		// Token: 0x0400057B RID: 1403
		public bool mAttachToEmitter;

		// Token: 0x0400057C RID: 1404
		public int mAnimSpeed;

		// Token: 0x0400057D RID: 1405
		public bool mAnimStartOnRandomFrame;

		// Token: 0x0400057E RID: 1406
		public float mAttachVal;

		// Token: 0x0400057F RID: 1407
		public bool mFlipHorz;

		// Token: 0x04000580 RID: 1408
		public bool mFlipVert;

		// Token: 0x04000581 RID: 1409
		public int mRepeatColor;

		// Token: 0x04000582 RID: 1410
		public int mRepeatAlpha;

		// Token: 0x04000583 RID: 1411
		public bool mRandomGradientColor;

		// Token: 0x04000584 RID: 1412
		public bool mUseNextColorKey;

		// Token: 0x04000585 RID: 1413
		public bool mGetColorFromLayer;

		// Token: 0x04000586 RID: 1414
		public bool mUpdateColorFromLayer;

		// Token: 0x04000587 RID: 1415
		public bool mGetTransparencyFromLayer;

		// Token: 0x04000588 RID: 1416
		public bool mUpdateTransparencyFromLayer;

		// Token: 0x04000589 RID: 1417
		public int mNumberOfEachColor;

		// Token: 0x0400058A RID: 1418
		public bool mLinkTransparencyToColor;

		// Token: 0x0400058B RID: 1419
		public bool mUseKeyColorsOnly;

		// Token: 0x0400058C RID: 1420
		public bool mUseEmitterAngleAndRange;

		// Token: 0x0400058D RID: 1421
		public bool mAngleAlignToMotion;

		// Token: 0x0400058E RID: 1422
		public bool mAngleKeepAlignedToMotion;

		// Token: 0x0400058F RID: 1423
		public bool mAngleRandomAlign;

		// Token: 0x04000590 RID: 1424
		public int mAngleAlignOffset;

		// Token: 0x04000591 RID: 1425
		public int mAngleValue;

		// Token: 0x04000592 RID: 1426
		public int mAngleRange;

		// Token: 0x04000593 RID: 1427
		public int mAngleOffset;

		// Token: 0x04000594 RID: 1428
		public PIInterpolator mColor = new PIInterpolator();

		// Token: 0x04000595 RID: 1429
		public PIInterpolator mAlpha = new PIInterpolator();

		// Token: 0x020000DE RID: 222
		public enum PIParticleDefValue
		{
			// Token: 0x04000597 RID: 1431
			VALUE_LIFE,
			// Token: 0x04000598 RID: 1432
			VALUE_NUMBER,
			// Token: 0x04000599 RID: 1433
			VALUE_SIZE_X,
			// Token: 0x0400059A RID: 1434
			VALUE_VELOCITY,
			// Token: 0x0400059B RID: 1435
			VALUE_WEIGHT,
			// Token: 0x0400059C RID: 1436
			VALUE_SPIN,
			// Token: 0x0400059D RID: 1437
			VALUE_MOTION_RAND,
			// Token: 0x0400059E RID: 1438
			VALUE_BOUNCE,
			// Token: 0x0400059F RID: 1439
			VALUE_LIFE_VARIATION,
			// Token: 0x040005A0 RID: 1440
			VALUE_NUMBER_VARIATION,
			// Token: 0x040005A1 RID: 1441
			VALUE_SIZE_X_VARIATION,
			// Token: 0x040005A2 RID: 1442
			VALUE_VELOCITY_VARIATION,
			// Token: 0x040005A3 RID: 1443
			VALUE_WEIGHT_VARIATION,
			// Token: 0x040005A4 RID: 1444
			VALUE_SPIN_VARIATION,
			// Token: 0x040005A5 RID: 1445
			VALUE_MOTION_RAND_VARIATION,
			// Token: 0x040005A6 RID: 1446
			VALUE_BOUNCE_VARIATION,
			// Token: 0x040005A7 RID: 1447
			VALUE_SIZE_X_OVER_LIFE,
			// Token: 0x040005A8 RID: 1448
			VALUE_VELOCITY_OVER_LIFE,
			// Token: 0x040005A9 RID: 1449
			VALUE_WEIGHT_OVER_LIFE,
			// Token: 0x040005AA RID: 1450
			VALUE_SPIN_OVER_LIFE,
			// Token: 0x040005AB RID: 1451
			VALUE_MOTION_RAND_OVER_LIFE,
			// Token: 0x040005AC RID: 1452
			VALUE_BOUNCE_OVER_LIFE,
			// Token: 0x040005AD RID: 1453
			VALUE_VISIBILITY,
			// Token: 0x040005AE RID: 1454
			VALUE_EMISSION_ANGLE,
			// Token: 0x040005AF RID: 1455
			VALUE_EMISSION_RANGE,
			// Token: 0x040005B0 RID: 1456
			VALUE_SIZE_Y,
			// Token: 0x040005B1 RID: 1457
			VALUE_SIZE_Y_VARIATION,
			// Token: 0x040005B2 RID: 1458
			VALUE_SIZE_Y_OVER_LIFE,
			// Token: 0x040005B3 RID: 1459
			NUM_VALUES
		}
	}
}
