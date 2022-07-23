using System;
using System.Collections.Generic;
using System.Linq;
using SexyFramework;

namespace JeffLib
{
	// Token: 0x02000112 RID: 274
	public class QRand
	{
		// Token: 0x06000847 RID: 2119 RVA: 0x0002A5EE File Offset: 0x000287EE
		private void Init()
		{
			QRand.RandomNumbers.Seed(0);
			this.mUpdateCnt = 0;
			this.mLastIndex = -1;
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0002A604 File Offset: 0x00028804
		public QRand()
		{
			this.Init();
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0002A640 File Offset: 0x00028840
		public QRand(float value)
		{
			this.Init();
			List<float> list = new List<float>();
			list.Add(value);
			this.SetWeights(list);
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0002A69C File Offset: 0x0002889C
		public QRand(List<float> initial_weights)
		{
			this.Init();
			this.SetWeights(initial_weights);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0002A6E8 File Offset: 0x000288E8
		public void SetWeights(List<float> v)
		{
			this.mWeights.Clear();
			if (v.Count == 1)
			{
				this.mWeights.Add(1f - v[0]);
				this.mWeights.Add(v[0]);
			}
			else
			{
				float num = 0f;
				for (int i = 0; i < v.Count; i++)
				{
					this.mWeights.Add(v[i]);
					num += this.mWeights[i];
				}
				for (int j = 0; j < this.mWeights.Count; j++)
				{
					List<float> list;
					int num2;
					(list = this.mWeights)[num2 = j] = list[num2] / num;
				}
			}
			for (int k = this.mLastHitUpdate.Count; k < this.mWeights.Count; k++)
			{
				this.mLastHitUpdate.Add(0);
				this.mPrevLastHitUpdate.Add(0);
			}
			this.mCurSway.Clear();
			this.mCurSway.Resize(this.mWeights.Count);
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0002A7FC File Offset: 0x000289FC
		public int Next()
		{
			this.mUpdateCnt++;
			float num = 0f;
			for (int i = 0; i < Enumerable.Count<float>(this.mWeights); i++)
			{
				float num2 = this.mWeights[i];
				if (num2 != 0f)
				{
					float num3 = 1f / num2;
					float num4 = 1f + ((float)(this.mUpdateCnt - this.mLastHitUpdate[i]) - num3) / num3;
					float num5 = 1f + ((float)(this.mUpdateCnt - this.mPrevLastHitUpdate[i]) - num3 * 2f) / (num3 * 2f);
					float num6 = num2 * Math.Max(Math.Min(num4 * 0.75f + num5 * 0.25f, 3f), 0.333f);
					this.mCurSway[i] = num6;
					num += num6;
				}
				else
				{
					this.mCurSway[i] = 0f;
				}
			}
			float num7 = (float)QRand.RandomNumbers.NextNumber(1, QRand.RAND_MAX) / (float)QRand.RAND_MAX * num;
			QRand.gDebugFirstRand = (int)num7;
			QRand.gSwaySize = this.mCurSway.Count;
			int num8 = 0;
			while (num8 < this.mCurSway.Count && num7 > this.mCurSway[num8])
			{
				num7 -= this.mCurSway[num8];
				num8++;
			}
			if (num8 >= this.mCurSway.Count)
			{
				num8--;
			}
			this.mPrevLastHitUpdate[num8] = this.mLastHitUpdate[num8];
			this.mLastHitUpdate[num8] = this.mUpdateCnt;
			this.mLastIndex = num8;
			return num8;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0002A9AC File Offset: 0x00028BAC
		public int NumWeights()
		{
			return this.mWeights.Count;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0002A9BC File Offset: 0x00028BBC
		public int NumNonZeroWeights()
		{
			int num = 0;
			for (int i = 0; i < this.mWeights.Count; i++)
			{
				if (this.mWeights[i] != 0f)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0002A9F9 File Offset: 0x00028BF9
		public void Clear()
		{
			this.mWeights.Clear();
			this.mCurSway.Clear();
			this.mLastHitUpdate.Clear();
			this.mPrevLastHitUpdate.Clear();
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0002AA27 File Offset: 0x00028C27
		public bool HasWeight(int idx)
		{
			return idx < this.mWeights.Count && this.mWeights[idx] > 0f;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0002AA4C File Offset: 0x00028C4C
		public void SyncState(DataSyncBase sync)
		{
			sync.SyncLong(ref this.mUpdateCnt);
			sync.SyncLong(ref this.mLastIndex);
			sync.SyncListFloat(this.mWeights);
			sync.SyncListFloat(this.mCurSway);
			sync.SyncListInt(this.mLastHitUpdate);
			sync.SyncListInt(this.mPrevLastHitUpdate);
		}

		// Token: 0x0400079C RID: 1948
		public static int RAND_MAX = 32767;

		// Token: 0x0400079D RID: 1949
		public static int gDebugFirstRand = 0;

		// Token: 0x0400079E RID: 1950
		public static int gSwaySize = 0;

		// Token: 0x0400079F RID: 1951
		protected int mUpdateCnt;

		// Token: 0x040007A0 RID: 1952
		protected int mLastIndex;

		// Token: 0x040007A1 RID: 1953
		protected List<float> mWeights = new List<float>();

		// Token: 0x040007A2 RID: 1954
		protected List<float> mCurSway = new List<float>();

		// Token: 0x040007A3 RID: 1955
		protected List<int> mLastHitUpdate = new List<int>();

		// Token: 0x040007A4 RID: 1956
		protected List<int> mPrevLastHitUpdate = new List<int>();

		// Token: 0x02000113 RID: 275
		internal static class RandomNumbers
		{
			// Token: 0x06000853 RID: 2131 RVA: 0x0002AAB9 File Offset: 0x00028CB9
			internal static int NextNumber()
			{
				if (QRand.RandomNumbers.r == null)
				{
					QRand.RandomNumbers.Seed();
				}
				return QRand.RandomNumbers.r.Next();
			}

			// Token: 0x06000854 RID: 2132 RVA: 0x0002AAD1 File Offset: 0x00028CD1
			internal static int NextNumber(int ceiling)
			{
				if (QRand.RandomNumbers.r == null)
				{
					QRand.RandomNumbers.Seed();
				}
				return QRand.RandomNumbers.r.Next(ceiling);
			}

			// Token: 0x06000855 RID: 2133 RVA: 0x0002AAEA File Offset: 0x00028CEA
			internal static int NextNumber(int min, int max)
			{
				if (QRand.RandomNumbers.r == null)
				{
					QRand.RandomNumbers.Seed();
				}
				return QRand.RandomNumbers.r.Next(min, max);
			}

			// Token: 0x06000856 RID: 2134 RVA: 0x0002AB04 File Offset: 0x00028D04
			internal static void Seed()
			{
				QRand.RandomNumbers.r = new Random();
			}

			// Token: 0x06000857 RID: 2135 RVA: 0x0002AB10 File Offset: 0x00028D10
			internal static void Seed(int seed)
			{
				QRand.RandomNumbers.r = new Random(seed);
			}

			// Token: 0x040007A5 RID: 1957
			private static Random r;
		}
	}
}
