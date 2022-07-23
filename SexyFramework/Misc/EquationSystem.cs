using System;
using System.Collections.Generic;

namespace SexyFramework.Misc
{
	// Token: 0x02000123 RID: 291
	internal class EquationSystem
	{
		// Token: 0x06000938 RID: 2360 RVA: 0x00030CA0 File Offset: 0x0002EEA0
		public EquationSystem(int theNumVariables)
		{
			this.mRowSize = theNumVariables + 1;
			this.mCurRow = 0;
			this.eqs.Resize(this.mRowSize * theNumVariables);
			this.sol.Resize(theNumVariables);
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00030CF8 File Offset: 0x0002EEF8
		public void SetCoefficient(int theRow, int theCol, float theValue)
		{
			int num = this.mRowSize * theRow + theCol;
			this.eqs[num] = theValue;
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x00030D20 File Offset: 0x0002EF20
		public void SetConstantTerm(int theRow, float theValue)
		{
			int num = this.mRowSize * theRow + this.mRowSize - 1;
			this.eqs[num] = theValue;
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00030D4C File Offset: 0x0002EF4C
		public void SetCoefficient(int theCol, float theValue)
		{
			this.SetCoefficient(this.mCurRow, theCol, theValue);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00030D5C File Offset: 0x0002EF5C
		public void SetConstantTerm(float theValue)
		{
			this.SetConstantTerm(this.mCurRow, theValue);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00030D6B File Offset: 0x0002EF6B
		public void NextEquation()
		{
			this.mCurRow++;
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00030D7C File Offset: 0x0002EF7C
		public void Solve()
		{
			int num = this.mRowSize;
			int num2 = this.mRowSize - 1;
			for (int i = 0; i < num2; i++)
			{
				int num3 = i;
				for (int j = i + 1; j < num2; j++)
				{
					if (Math.Abs(this.eqs[j * num + i]) > Math.Abs(this.eqs[num3 * num + i]))
					{
						num3 = j;
					}
				}
				for (int k = 0; k < num2 + 1; k++)
				{
					float num4 = this.eqs[i * num + k];
					this.eqs[i * num + k] = this.eqs[num3 * num + k];
					this.eqs[num3 * num + k] = num4;
				}
				for (int j = i + 1; j < num2; j++)
				{
					float num5 = this.eqs[j * num + i] / this.eqs[i * num + i];
					if (num5 != 0f)
					{
						for (int k = num2; k >= i; k--)
						{
							List<float> list;
							int num6;
							(list = this.eqs)[num6 = j * num + k] = list[num6] - this.eqs[i * num + k] * num5;
						}
					}
				}
			}
			for (int j = num2 - 1; j >= 0; j--)
			{
				float num7 = 0f;
				for (int k = j + 1; k < num2; k++)
				{
					num7 += this.eqs[j * num + k] * this.sol[k];
				}
				this.sol[j] = (this.eqs[j * num + num2] - num7) / this.eqs[j * num + j];
			}
		}

		// Token: 0x04000849 RID: 2121
		public List<float> eqs = new List<float>();

		// Token: 0x0400084A RID: 2122
		public List<float> sol = new List<float>();

		// Token: 0x0400084B RID: 2123
		public int mRowSize;

		// Token: 0x0400084C RID: 2124
		public int mCurRow;
	}
}
