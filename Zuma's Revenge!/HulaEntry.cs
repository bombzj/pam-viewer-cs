using System;

namespace ZumasRevenge
{
	// Token: 0x02000075 RID: 117
	public class HulaEntry
	{
		// Token: 0x06000896 RID: 2198 RVA: 0x0004C753 File Offset: 0x0004A953
		public HulaEntry()
		{
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0004C75C File Offset: 0x0004A95C
		public HulaEntry(HulaEntry rhs)
		{
			this.mBerserkAmt = rhs.mBerserkAmt;
			this.mAmnesty = rhs.mAmnesty;
			this.mVX = rhs.mVX;
			this.mProjVY = rhs.mProjVY;
			this.mSpawnY = rhs.mSpawnY;
			this.mSpawnRate = rhs.mSpawnRate;
			this.mProjChance = rhs.mProjChance;
			this.mAttackType = rhs.mAttackType;
			this.mAttackTime = rhs.mAttackTime;
			this.mProjRange = rhs.mProjRange;
		}

		// Token: 0x0400064D RID: 1613
		public int mBerserkAmt;

		// Token: 0x0400064E RID: 1614
		public int mAmnesty;

		// Token: 0x0400064F RID: 1615
		public float mVX;

		// Token: 0x04000650 RID: 1616
		public float mProjVY;

		// Token: 0x04000651 RID: 1617
		public int mSpawnY;

		// Token: 0x04000652 RID: 1618
		public int mSpawnRate;

		// Token: 0x04000653 RID: 1619
		public int mProjChance;

		// Token: 0x04000654 RID: 1620
		public int mAttackType;

		// Token: 0x04000655 RID: 1621
		public int mAttackTime;

		// Token: 0x04000656 RID: 1622
		public int mProjRange;

		// Token: 0x02000076 RID: 118
		public enum AttackType
		{
			// Token: 0x04000658 RID: 1624
			Attack_None,
			// Token: 0x04000659 RID: 1625
			Attack_Stun,
			// Token: 0x0400065A RID: 1626
			Attack_Poison,
			// Token: 0x0400065B RID: 1627
			Attack_Hallucinate,
			// Token: 0x0400065C RID: 1628
			Attack_Slow
		}
	}
}
