using System;
using System.Collections.Generic;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000DF RID: 223
	public class PIEmitter
	{
		// Token: 0x060006AC RID: 1708 RVA: 0x0001C824 File Offset: 0x0001AA24
		public PIEmitter()
		{
			this.mValues = new PIValue[42];
			this.mParticleDefVector = new List<PIParticleDef>();
			for (int i = 0; i < 42; i++)
			{
				this.mValues[i] = new PIValue();
			}
		}

		// Token: 0x040005B4 RID: 1460
		public string mName;

		// Token: 0x040005B5 RID: 1461
		public PIValue[] mValues;

		// Token: 0x040005B6 RID: 1462
		public List<PIParticleDef> mParticleDefVector;

		// Token: 0x040005B7 RID: 1463
		public bool mKeepInOrder;

		// Token: 0x040005B8 RID: 1464
		public bool mOldestInFront;

		// Token: 0x040005B9 RID: 1465
		public bool mIsSuperEmitter;

		// Token: 0x020000E0 RID: 224
		public enum PIEmitterValue
		{
			// Token: 0x040005BB RID: 1467
			VALUE_F_LIFE,
			// Token: 0x040005BC RID: 1468
			VALUE_F_NUMBER,
			// Token: 0x040005BD RID: 1469
			VALUE_F_VELOCITY,
			// Token: 0x040005BE RID: 1470
			VALUE_F_WEIGHT,
			// Token: 0x040005BF RID: 1471
			VALUE_F_SPIN,
			// Token: 0x040005C0 RID: 1472
			VALUE_F_MOTION_RAND,
			// Token: 0x040005C1 RID: 1473
			VALUE_F_BOUNCE,
			// Token: 0x040005C2 RID: 1474
			VALUE_F_ZOOM,
			// Token: 0x040005C3 RID: 1475
			VALUE_LIFE,
			// Token: 0x040005C4 RID: 1476
			VALUE_NUMBER,
			// Token: 0x040005C5 RID: 1477
			VALUE_SIZE_X,
			// Token: 0x040005C6 RID: 1478
			VALUE_SIZE_Y,
			// Token: 0x040005C7 RID: 1479
			VALUE_VELOCITY,
			// Token: 0x040005C8 RID: 1480
			VALUE_WEIGHT,
			// Token: 0x040005C9 RID: 1481
			VALUE_SPIN,
			// Token: 0x040005CA RID: 1482
			VALUE_MOTION_RAND,
			// Token: 0x040005CB RID: 1483
			VALUE_BOUNCE,
			// Token: 0x040005CC RID: 1484
			VALUE_ZOOM,
			// Token: 0x040005CD RID: 1485
			VALUE_VISIBILITY,
			// Token: 0x040005CE RID: 1486
			VALUE_UNKNOWN3,
			// Token: 0x040005CF RID: 1487
			VALUE_TINT_STRENGTH,
			// Token: 0x040005D0 RID: 1488
			VALUE_EMISSION_ANGLE,
			// Token: 0x040005D1 RID: 1489
			VALUE_EMISSION_RANGE,
			// Token: 0x040005D2 RID: 1490
			VALUE_F_LIFE_VARIATION,
			// Token: 0x040005D3 RID: 1491
			VALUE_F_NUMBER_VARIATION,
			// Token: 0x040005D4 RID: 1492
			VALUE_F_SIZE_X_VARIATION,
			// Token: 0x040005D5 RID: 1493
			VALUE_F_SIZE_Y_VARIATION,
			// Token: 0x040005D6 RID: 1494
			VALUE_F_VELOCITY_VARIATION,
			// Token: 0x040005D7 RID: 1495
			VALUE_F_WEIGHT_VARIATION,
			// Token: 0x040005D8 RID: 1496
			VALUE_F_SPIN_VARIATION,
			// Token: 0x040005D9 RID: 1497
			VALUE_F_MOTION_RAND_VARIATION,
			// Token: 0x040005DA RID: 1498
			VALUE_F_BOUNCE_VARIATION,
			// Token: 0x040005DB RID: 1499
			VALUE_F_ZOOM_VARIATION,
			// Token: 0x040005DC RID: 1500
			VALUE_F_NUMBER_OVER_LIFE,
			// Token: 0x040005DD RID: 1501
			VALUE_F_SIZE_X_OVER_LIFE,
			// Token: 0x040005DE RID: 1502
			VALUE_F_SIZE_Y_OVER_LIFE,
			// Token: 0x040005DF RID: 1503
			VALUE_F_VELOCITY_OVER_LIFE,
			// Token: 0x040005E0 RID: 1504
			VALUE_F_WEIGHT_OVER_LIFE,
			// Token: 0x040005E1 RID: 1505
			VALUE_F_SPIN_OVER_LIFE,
			// Token: 0x040005E2 RID: 1506
			VALUE_F_MOTION_RAND_OVER_LIFE,
			// Token: 0x040005E3 RID: 1507
			VALUE_F_BOUNCE_OVER_LIFE,
			// Token: 0x040005E4 RID: 1508
			VALUE_F_ZOOM_OVER_LIFE,
			// Token: 0x040005E5 RID: 1509
			NUM_VALUES
		}
	}
}
