using System;
using System.Collections.Generic;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000E7 RID: 231
	public class PIEmitterInstanceDef : IDisposable
	{
		// Token: 0x060006B9 RID: 1721 RVA: 0x0001CAB4 File Offset: 0x0001ACB4
		public PIEmitterInstanceDef()
		{
			this.mPosition = new PIValue2D();
			this.mValues = new PIValue[19];
			this.mPoints = new List<PIValue2D>();
			this.mFreeEmitterIndices = new List<int>();
			for (int i = 0; i < 19; i++)
			{
				this.mValues[i] = new PIValue();
			}
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001CB10 File Offset: 0x0001AD10
		public virtual void Dispose()
		{
			this.mFreeEmitterIndices.Clear();
			this.mPosition.Dispose();
			for (int i = 0; i < 19; i++)
			{
				this.mValues[i].Dispose();
			}
			foreach (PIValue2D pivalue2D in this.mPoints)
			{
				pivalue2D.Dispose();
			}
		}

		// Token: 0x0400061A RID: 1562
		public string mName;

		// Token: 0x0400061B RID: 1563
		public int mFramesToPreload;

		// Token: 0x0400061C RID: 1564
		public int mEmitterDefIdx;

		// Token: 0x0400061D RID: 1565
		public int mEmitterGeom;

		// Token: 0x0400061E RID: 1566
		public bool mEmitIn;

		// Token: 0x0400061F RID: 1567
		public bool mEmitOut;

		// Token: 0x04000620 RID: 1568
		public int mEmitAtPointsNum;

		// Token: 0x04000621 RID: 1569
		public int mEmitAtPointsNum2;

		// Token: 0x04000622 RID: 1570
		public bool mIsSuperEmitter;

		// Token: 0x04000623 RID: 1571
		public List<int> mFreeEmitterIndices;

		// Token: 0x04000624 RID: 1572
		public bool mInvertMask;

		// Token: 0x04000625 RID: 1573
		public PIValue2D mPosition;

		// Token: 0x04000626 RID: 1574
		public PIValue[] mValues;

		// Token: 0x04000627 RID: 1575
		public List<PIValue2D> mPoints;

		// Token: 0x020000E8 RID: 232
		public enum PIEmitterValue
		{
			// Token: 0x04000629 RID: 1577
			VALUE_LIFE,
			// Token: 0x0400062A RID: 1578
			VALUE_NUMBER,
			// Token: 0x0400062B RID: 1579
			VALUE_SIZE_X,
			// Token: 0x0400062C RID: 1580
			VALUE_VELOCITY,
			// Token: 0x0400062D RID: 1581
			VALUE_WEIGHT,
			// Token: 0x0400062E RID: 1582
			VALUE_SPIN,
			// Token: 0x0400062F RID: 1583
			VALUE_MOTION_RAND,
			// Token: 0x04000630 RID: 1584
			VALUE_BOUNCE,
			// Token: 0x04000631 RID: 1585
			VALUE_ZOOM,
			// Token: 0x04000632 RID: 1586
			VALUE_VISIBILITY,
			// Token: 0x04000633 RID: 1587
			VALUE_TINT_STRENGTH,
			// Token: 0x04000634 RID: 1588
			VALUE_EMISSION_ANGLE,
			// Token: 0x04000635 RID: 1589
			VALUE_EMISSION_RANGE,
			// Token: 0x04000636 RID: 1590
			VALUE_ACTIVE,
			// Token: 0x04000637 RID: 1591
			VALUE_ANGLE,
			// Token: 0x04000638 RID: 1592
			VALUE_XRADIUS,
			// Token: 0x04000639 RID: 1593
			VALUE_YRADIUS,
			// Token: 0x0400063A RID: 1594
			VALUE_SIZE_Y,
			// Token: 0x0400063B RID: 1595
			VALUE_UNKNOWN4,
			// Token: 0x0400063C RID: 1596
			NUM_VALUES
		}

		// Token: 0x020000E9 RID: 233
		public enum PIEmitterGEOM
		{
			// Token: 0x0400063E RID: 1598
			GEOM_POINT,
			// Token: 0x0400063F RID: 1599
			GEOM_LINE,
			// Token: 0x04000640 RID: 1600
			GEOM_ECLIPSE,
			// Token: 0x04000641 RID: 1601
			GEOM_AREA,
			// Token: 0x04000642 RID: 1602
			GEOM_CIRCLE
		}
	}
}
