using System;
using System.Collections.Generic;

namespace SexyFramework.Misc
{
	// Token: 0x02000122 RID: 290
	public class BSpline
	{
		// Token: 0x06000928 RID: 2344 RVA: 0x00030574 File Offset: 0x0002E774
		protected float GetPoint(float t, ref List<float> theCoef)
		{
			int num = (int)Math.Floor((double)t);
			if (num < 0)
			{
				num = 0;
				t = 0f;
			}
			else if (num >= this.mXPoints.Count - 1)
			{
				num = this.mXPoints.Count - 2;
				t = (float)(num + 1);
			}
			float num2 = t - (float)num;
			num *= 4;
			float num3 = theCoef[num];
			float num4 = theCoef[num + 1];
			float num5 = theCoef[num + 2];
			float num6 = theCoef[num + 3];
			float num7 = num2 * num2;
			float num8 = num7 * num2;
			return num3 * num8 + num4 * num7 + num5 * num2 + num6;
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x00030610 File Offset: 0x0002E810
		protected void CalculateSplinePrv(ref List<float> thePoints, ref List<float> theCoef)
		{
			if (thePoints.Count < 2)
			{
				return;
			}
			int num = thePoints.Count - 1;
			int theNumVariables = num * 4;
			EquationSystem equationSystem = new EquationSystem(theNumVariables);
			equationSystem.SetCoefficient(2, 1f);
			equationSystem.NextEquation();
			int num2 = 0;
			int i = 0;
			while (i < num)
			{
				equationSystem.SetCoefficient(num2 + 3, 1f);
				equationSystem.SetConstantTerm(thePoints[i]);
				equationSystem.NextEquation();
				equationSystem.SetCoefficient(num2, 1f);
				equationSystem.SetCoefficient(num2 + 1, 1f);
				equationSystem.SetCoefficient(num2 + 2, 1f);
				equationSystem.SetConstantTerm(thePoints[i + 1] - thePoints[i]);
				equationSystem.NextEquation();
				equationSystem.SetCoefficient(num2, 3f);
				equationSystem.SetCoefficient(num2 + 1, 2f);
				equationSystem.SetCoefficient(num2 + 2, 1f);
				if (i < num - 1)
				{
					equationSystem.SetCoefficient(num2 + 6, -1f);
				}
				equationSystem.NextEquation();
				if (i < num - 1)
				{
					equationSystem.SetCoefficient(num2, 6f);
					equationSystem.SetCoefficient(num2 + 1, 2f);
					equationSystem.SetCoefficient(num2 + 5, -2f);
					equationSystem.NextEquation();
				}
				i++;
				num2 += 4;
			}
			equationSystem.Solve();
			theCoef = equationSystem.sol;
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x00030764 File Offset: 0x0002E964
		protected void CalculateSplinePrvLinear(ref List<float> thePoints, ref List<float> theCoef)
		{
			if (thePoints.Count < 2)
			{
				return;
			}
			int num = thePoints.Count - 1;
			int num2 = num * 4;
			theCoef.Clear();
			for (int i = 0; i < num2; i++)
			{
				theCoef.Add(0f);
			}
			for (int j = 0; j < num; j++)
			{
				int num3 = j * 4;
				float num4 = thePoints[j];
				float num5 = thePoints[j + 1];
				theCoef[num3] = 0f;
				theCoef[num3 + 1] = 0f;
				theCoef[num3 + 2] = num5 - num4;
				theCoef[num3 + 3] = num4;
			}
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0003080C File Offset: 0x0002EA0C
		protected void CalculateSplinePrvSemiLinear(ref List<float> thePoints, ref List<float> theCoef)
		{
			if (thePoints.Count < 2)
			{
				return;
			}
			int num = thePoints.Count - 1;
			List<float> list = new List<float>();
			for (int i = 0; i < num; i++)
			{
				float num2 = this.mArcLengths[i];
				if (num2 > 100f)
				{
					num2 = 100f / num2;
				}
				num2 = 0.3f;
				float num3 = thePoints[i];
				float num4 = thePoints[i + 1];
				if (i > 0)
				{
					list.Add(num2 * num4 + (1f - num2) * num3);
				}
				else
				{
					list.Add(num3);
				}
				if (i < num - 1)
				{
					list.Add(num2 * num3 + (1f - num2) * num4);
				}
				else
				{
					list.Add(num4);
				}
			}
			thePoints = list;
			num = list.Count - 1;
			int num5 = num * 4;
			theCoef.Clear();
			for (int j = 0; j < num5; j++)
			{
				theCoef.Add(0f);
			}
			for (int i = 0; i < num; i++)
			{
				float num6 = list[i];
				float num7 = list[i + 1];
				int num8 = i * 4;
				if ((i & 1) != 0 && i < num - 1)
				{
					float num9 = list[i - 1];
					float num10 = list[i + 2];
					float num11 = num6;
					float num12 = num6 - num9;
					float num13 = -2f * (num7 - 2f * num6 + num9) - num12 + (num10 - num7);
					float num14 = -num13 + num7 - 2f * num6 + num9;
					theCoef[num8] = num13;
					theCoef[num8 + 1] = num14;
					theCoef[num8 + 2] = num12;
					theCoef[num8 + 3] = num11;
				}
				else
				{
					theCoef[num8] = 0f;
					theCoef[num8 + 1] = 0f;
					theCoef[num8 + 2] = num7 - num6;
					theCoef[num8 + 3] = num6;
				}
			}
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x00030A04 File Offset: 0x0002EC04
		protected void CalcArcLengths()
		{
			this.mArcLengths.Clear();
			int num = this.mXPoints.Count - 1;
			for (int i = 0; i < num; i++)
			{
				float num2 = this.mXPoints[i];
				float num3 = this.mYPoints[i];
				float num4 = this.mXPoints[i + 1];
				float num5 = this.mYPoints[i + 1];
				float num6 = (float)Math.Sqrt((double)((num4 - num2) * (num4 - num2) + (num5 - num3) * (num5 - num3)));
				this.mArcLengths.Add(num6);
			}
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x00030AD8 File Offset: 0x0002ECD8
		public void Reset()
		{
			this.mXPoints.Clear();
			this.mYPoints.Clear();
			this.mArcLengths.Clear();
			this.mXCoef.Clear();
			this.mYCoef.Clear();
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x00030B11 File Offset: 0x0002ED11
		public void AddPoint(float x, float y)
		{
			this.mXPoints.Add(x);
			this.mYPoints.Add(y);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x00030B2B File Offset: 0x0002ED2B
		public void CalculateSpline()
		{
			this.CalculateSpline(false);
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00030B34 File Offset: 0x0002ED34
		public void CalculateSpline(bool linear)
		{
			this.CalcArcLengths();
			if (linear)
			{
				this.CalculateSplinePrvLinear(ref this.mXPoints, ref this.mXCoef);
				this.CalculateSplinePrvLinear(ref this.mYPoints, ref this.mYCoef);
			}
			else
			{
				this.CalculateSplinePrv(ref this.mXPoints, ref this.mXCoef);
				this.CalculateSplinePrv(ref this.mYPoints, ref this.mYCoef);
			}
			this.CalcArcLengths();
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x00030B9A File Offset: 0x0002ED9A
		public float GetXPoint(float t)
		{
			return this.GetPoint(t, ref this.mXCoef);
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00030BA9 File Offset: 0x0002EDA9
		public float GetYPoint(float t)
		{
			return this.GetPoint(t, ref this.mYCoef);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x00030BB8 File Offset: 0x0002EDB8
		public bool GetNextPoint(ref float x, ref float y, ref float t)
		{
			int num = (int)Math.Floor((double)t);
			if (num < 0 || num >= this.mXPoints.Count - 1)
			{
				x = this.GetXPoint(t);
				y = this.GetYPoint(t);
				return false;
			}
			float num2 = 1f / (this.mArcLengths[num] * 100f);
			float xpoint = this.GetXPoint(t);
			float ypoint = this.GetYPoint(t);
			float num3 = t;
			float xpoint2;
			float ypoint2;
			float num4;
			do
			{
				num3 += num2;
				xpoint2 = this.GetXPoint(num3);
				ypoint2 = this.GetYPoint(num3);
				num4 = (xpoint2 - xpoint) * (xpoint2 - xpoint) + (ypoint2 - ypoint) * (ypoint2 - ypoint);
			}
			while (num4 < 1f && num3 <= (float)(this.mXPoints.Count - 1));
			x = xpoint2;
			y = ypoint2;
			t = num3;
			return true;
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x00030C7F File Offset: 0x0002EE7F
		public List<float> GetXPoints()
		{
			return this.mXPoints;
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00030C87 File Offset: 0x0002EE87
		public List<float> GetYPoints()
		{
			return this.mYPoints;
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x00030C8F File Offset: 0x0002EE8F
		public int GetMaxT()
		{
			return this.mXPoints.Count - 1;
		}

		// Token: 0x04000844 RID: 2116
		protected List<float> mXPoints = new List<float>();

		// Token: 0x04000845 RID: 2117
		protected List<float> mYPoints = new List<float>();

		// Token: 0x04000846 RID: 2118
		protected List<float> mArcLengths = new List<float>();

		// Token: 0x04000847 RID: 2119
		protected List<float> mXCoef = new List<float>();

		// Token: 0x04000848 RID: 2120
		protected List<float> mYCoef = new List<float>();
	}
}
