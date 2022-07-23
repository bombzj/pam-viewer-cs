using System;
using System.Collections.Generic;
using System.Globalization;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework
{
	// Token: 0x02000128 RID: 296
	public class CurvedVal
	{
		// Token: 0x060009C8 RID: 2504 RVA: 0x000331EC File Offset: 0x000313EC
		private static int SIGN(int aVal)
		{
			if (aVal < 0)
			{
				return -1;
			}
			if (aVal <= 0)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x000331FB File Offset: 0x000313FB
		private static float SIGN(float aVal)
		{
			if (aVal < 0f)
			{
				return -1f;
			}
			if (aVal <= 0f)
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0003321E File Offset: 0x0003141E
		private static float CVCharToFloat(sbyte theChar)
		{
			if (theChar >= 92)
			{
				theChar -= 1;
			}
			return (float)(theChar - 35) / 90f;
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00033236 File Offset: 0x00031436
		private static int CVCharToInt(sbyte theChar)
		{
			if (theChar >= 92)
			{
				theChar -= 1;
			}
			return (int)(theChar - 35);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00033248 File Offset: 0x00031448
		private static float CVStrToAngle(string theStr)
		{
			int num = 0;
			num += CurvedVal.CVCharToInt((sbyte)theStr[0]);
			num *= 90;
			num += CurvedVal.CVCharToInt((sbyte)theStr[1]);
			num *= 90;
			num += CurvedVal.CVCharToInt((sbyte)theStr[2]);
			return (float)num * 360f / 729000f;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0003329F File Offset: 0x0003149F
		public static implicit operator double(CurvedVal ImpliedObject)
		{
			return ImpliedObject.GetOutVal();
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x000332A7 File Offset: 0x000314A7
		public static implicit operator SexyColor(CurvedVal ImpliedObject)
		{
			return new SexyColor(255, 255, 255, (int)(255.0 * ImpliedObject.GetOutVal()));
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x000332E2 File Offset: 0x000314E2
		// (set) Token: 0x060009CF RID: 2511 RVA: 0x000332CE File Offset: 0x000314CE
		public int mAppUpdateCountSrc
		{
			get
			{
				if (GlobalMembers.gSexyAppBase != null)
				{
					return GlobalMembers.gSexyAppBase.mUpdateCount;
				}
				return 0;
			}
			set
			{
				if (GlobalMembers.gSexyAppBase != null)
				{
					GlobalMembers.gSexyAppBase.mUpdateCount = value;
				}
			}
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x000332F8 File Offset: 0x000314F8
		protected void InitVarDefaults()
		{
			this.mMode = 0;
			this.mRamp = 0;
			this.mCurveCacheRecord = null;
			this.mSingleTrigger = false;
			this.mNoClip = false;
			this.mOutputSync = false;
			this.mTriggered = false;
			this.mIsHermite = false;
			this.mAutoInc = false;
			this.mInitAppUpdateCount = 0;
			this.mOutMin = 0.0;
			this.mOutMax = 1.0;
			this.mInMin = 0.0;
			this.mInMax = 1.0;
			this.mLinkedVal = null;
			this.mCurOutVal = 0.0;
			this.mInVal = 0.0;
			this.mPrevInVal = 0.0;
			this.mIncRate = 0.0;
			this.mPrevOutVal = 0.0;
			this.mDataP = "";
			this.mCurDataPStr = null;
			if (CurvedVal.mCurveCacheMap != null)
			{
				CurvedVal.mCurveCacheMap.Clear();
			}
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x000333FC File Offset: 0x000315FC
		protected bool CheckCurveChange()
		{
			if (this.mDataP != null && this.mDataP != this.mCurDataPStr)
			{
				this.mCurDataPStr = this.mDataP;
				this.ParseDataString(this.mCurDataPStr);
				return true;
			}
			return false;
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x00033434 File Offset: 0x00031634
		protected bool CheckClamping()
		{
			this.CheckCurveChange();
			if (this.mMode == 0)
			{
				if (this.mInVal < this.mInMin)
				{
					this.mInVal = this.mInMin;
					return false;
				}
				if (this.mInVal > this.mInMax)
				{
					this.mInVal = this.mInMax;
					return false;
				}
			}
			else if (this.mMode == 1 || this.mMode == 2)
			{
				double num = this.mInMax - this.mInMin;
				if (this.mInVal > this.mInMax || this.mInVal < this.mInMin)
				{
					this.mInVal = this.mInMin + Math.IEEERemainder(this.mInVal - this.mInMin + num, num);
				}
			}
			return true;
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x000334E8 File Offset: 0x000316E8
		protected void GenerateTable(List<CurvedVal.DataPoint> theDataPointVector, float[] theBuffer, int theSize)
		{
			BSpline bspline = new BSpline();
			for (int i = 0; i < theDataPointVector.Count; i++)
			{
				CurvedVal.DataPoint dataPoint = theDataPointVector[i];
				bspline.AddPoint(dataPoint.mX, dataPoint.mY);
			}
			bspline.CalculateSpline();
			bool flag = true;
			int num = 0;
			float num2 = 0f;
			for (int j = 1; j < theDataPointVector.Count; j++)
			{
				CurvedVal.DataPoint dataPoint2 = theDataPointVector[j - 1];
				CurvedVal.DataPoint dataPoint3 = theDataPointVector[j];
				int num3 = (int)((double)(dataPoint2.mX * (float)(theSize - 1)) + 0.5);
				int num4 = (int)((double)(dataPoint3.mX * (float)(theSize - 1)) + 0.5);
				for (int k = num3; k <= num4; k++)
				{
					float t = (float)(j - 1) + (float)(k - num3) / (float)(num4 - num3);
					float ypoint = bspline.GetYPoint(t);
					float xpoint = bspline.GetXPoint(t);
					int num5 = (int)((double)(xpoint * (float)(theSize - 1)) + 0.5);
					if (num5 >= num && num5 <= num4)
					{
						if (!flag)
						{
							if (num5 > num + 1)
							{
								for (int l = num; l <= num5; l++)
								{
									float num6 = (float)(l - num) / (float)(num5 - num);
									float num7 = num6 * ypoint + (1f - num6) * num2;
									if (!this.mNoClip)
									{
										num7 = Math.Min(Math.Max(num7, 0f), 1f);
									}
									theBuffer[l] = num7;
								}
							}
							else
							{
								float num8 = ypoint;
								if (!this.mNoClip)
								{
									num8 = Math.Min(Math.Max(num8, 0f), 1f);
								}
								theBuffer[num5] = num8;
							}
						}
						num = num5;
						num2 = ypoint;
						flag = false;
					}
				}
			}
			for (int m = 0; m < theDataPointVector.Count; m++)
			{
				CurvedVal.DataPoint dataPoint4 = theDataPointVector[m];
				int num9 = (int)((double)(dataPoint4.mX * (float)(theSize - 1)) + 0.5);
				theBuffer[num9] = dataPoint4.mY;
			}
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x000336E8 File Offset: 0x000318E8
		protected void ParseDataString(string theString)
		{
			this.mIncRate = 0.0;
			this.mOutMin = 0.0;
			this.mOutMax = 1.0;
			this.mSingleTrigger = false;
			this.mNoClip = false;
			this.mOutputSync = false;
			this.mIsHermite = false;
			this.mAutoInc = false;
			int i = 0;
			int num = 0;
			if (theString[0] >= 'a' && theString[0] <= 'b')
			{
				num = (int)(theString[0] - 'a');
			}
			i++;
			if (num >= 1)
			{
				int num2 = CurvedVal.CVCharToInt((sbyte)theString[i++]);
				this.mNoClip = (num2 & CurvedVal.DFLAG_NOCLIP) != 0;
				this.mSingleTrigger = (num2 & CurvedVal.DFLAG_SINGLETRIGGER) != 0;
				this.mOutputSync = (num2 & CurvedVal.DFLAG_OUTPUTSYNC) != 0;
				this.mIsHermite = (num2 & CurvedVal.DFLAG_HERMITE) != 0;
				this.mAutoInc = (num2 & CurvedVal.DFLAG_AUTOINC) != 0;
			}
			int num3 = theString.IndexOf(',', i);
			if (num3 == -1)
			{
				this.mIsHermite = true;
				return;
			}
			double num4 = 0.0;
			double.TryParse(theString.Substring(i, num3 - i), NumberStyles.Float, CultureInfo.InvariantCulture, out num4);
			this.mOutMin = (double)((float)num4);
			i = num3 + 1;
			num3 = theString.IndexOf(',', i);
			if (num3 == -1)
			{
				return;
			}
			num4 = 0.0;
			double.TryParse(theString.Substring(i, num3 - i), NumberStyles.Float, CultureInfo.InvariantCulture, out num4);
			this.mOutMax = (double)((float)num4);
			i = num3 + 1;
			num3 = theString.IndexOf(',', i);
			if (num3 == -1)
			{
				return;
			}
			num4 = 0.0;
			double.TryParse(theString.Substring(i, num3 - i), NumberStyles.Float, CultureInfo.InvariantCulture, out num4);
			this.mIncRate = (double)((float)num4);
			i = num3 + 1;
			if (num >= 1)
			{
				num3 = theString.IndexOf(',', i);
				if (num3 == -1)
				{
					return;
				}
				num4 = 0.0;
				double.TryParse(theString.Substring(i, num3 - i), NumberStyles.Float, CultureInfo.InvariantCulture, out num4);
				this.mInMax = (double)((float)num4);
				i = num3 + 1;
			}
			string text = theString.Substring(i);
			if (!CurvedVal.mCurveCacheMap.ContainsKey(text))
			{
				CurvedVal.CurveCacheRecord curveCacheRecord = new CurvedVal.CurveCacheRecord();
				CurvedVal.mCurveCacheMap.Add(text, curveCacheRecord);
				this.mCurveCacheRecord = curveCacheRecord;
				List<CurvedVal.DataPoint> list = new List<CurvedVal.DataPoint>();
				float num5 = 0f;
				while (i < theString.Length)
				{
					sbyte b = (sbyte)theString[i++];
					CurvedVal.DataPoint dataPoint = new CurvedVal.DataPoint();
					dataPoint.mX = num5;
					dataPoint.mY = CurvedVal.CVCharToFloat(b);
					if (this.mIsHermite)
					{
						string theStr = theString.Substring(i, 3);
						dataPoint.mAngleDeg = CurvedVal.CVStrToAngle(theStr);
						i += 3;
					}
					else
					{
						dataPoint.mAngleDeg = 0f;
					}
					list.Add(dataPoint);
					while (i < theString.Length)
					{
						b = (sbyte)theString[i++];
						if (b != 32)
						{
							num5 = Math.Min(num5 + CurvedVal.CVCharToFloat(b) * 0.1f, 1f);
							break;
						}
						num5 += 0.1f;
					}
				}
				this.GenerateTable(list, this.mCurveCacheRecord.mTable, CurvedVal.CV_NUM_SPLINE_POINTS);
				this.mCurveCacheRecord.mDataStr = theString;
				this.mCurveCacheRecord.mHermiteCurve.mPoints.Clear();
				for (int j = 0; j < list.Count; j++)
				{
					CurvedVal.DataPoint dataPoint2 = list[j];
					float inFxPrime = (float)Math.Tan((double)SexyMath.DegToRad(dataPoint2.mAngleDeg));
					this.mCurveCacheRecord.mHermiteCurve.mPoints.Add(new SexyMathHermite.SPoint(dataPoint2.mX, dataPoint2.mY, inFxPrime));
				}
				this.mCurveCacheRecord.mHermiteCurve.Rebuild();
				return;
			}
			this.mCurveCacheRecord = CurvedVal.mCurveCacheMap[text];
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x00033AC3 File Offset: 0x00031CC3
		public CurvedVal()
		{
			this.InitVarDefaults();
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x00033AD1 File Offset: 0x00031CD1
		public CurvedVal(string theData, CurvedVal theLinkedVal)
		{
			this.InitVarDefaults();
			this.SetCurve(theData, theLinkedVal);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x00033AE7 File Offset: 0x00031CE7
		public CurvedVal(string theDataP)
			: this(theDataP, null)
		{
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x00033AF4 File Offset: 0x00031CF4
		public void SetCurve(string theData, CurvedVal theLinkedVal)
		{
			this.mDataP = theData;
			this.mCurDataPStr = theData;
			if (this.mAppUpdateCountSrc != 0)
			{
				this.mInitAppUpdateCount = this.mAppUpdateCountSrc;
			}
			this.mTriggered = false;
			this.mLinkedVal = theLinkedVal;
			this.mRamp = 6;
			this.ParseDataString(theData);
			this.mInVal = this.mInMin;
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00033B4B File Offset: 0x00031D4B
		public void SetCurve(string theData)
		{
			this.SetCurve(theData, null);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00033B55 File Offset: 0x00031D55
		public void SetCurveMult(string theData)
		{
			this.SetCurveMult(theData, null);
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00033B60 File Offset: 0x00031D60
		public void SetCurveMult(string theData, CurvedVal theLinkedVal)
		{
			double outVal = this.GetOutVal();
			this.SetCurve(theData, theLinkedVal);
			this.mOutMax *= outVal;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00033B8C File Offset: 0x00031D8C
		public void SetConstant(double theValue)
		{
			this.mInVal = 0.0;
			this.mTriggered = false;
			this.mLinkedVal = null;
			this.mRamp = 1;
			this.mInMin = (this.mInMax = 0.0);
			this.mOutMax = theValue;
			this.mOutMin = theValue;
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00033BE5 File Offset: 0x00031DE5
		public bool IsInitialized()
		{
			return this.mRamp != 0;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x00033BF3 File Offset: 0x00031DF3
		public void SetMode(int theMode)
		{
			this.mMode = (byte)theMode;
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x00033BFD File Offset: 0x00031DFD
		public void SetRamp(int theRamp)
		{
			this.mRamp = (byte)theRamp;
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00033C07 File Offset: 0x00031E07
		public void SetOutRange(double theMin, double theMax)
		{
			this.mOutMin = theMin;
			this.mOutMax = theMax;
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00033C17 File Offset: 0x00031E17
		public void SetInRange(double theMin, double theMax)
		{
			this.mInMin = theMin;
			this.mInMax = theMax;
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00033C28 File Offset: 0x00031E28
		public double GetOutVal()
		{
			double outVal = this.GetOutVal(this.GetInVal());
			this.mCurOutVal = outVal;
			return outVal;
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00033C4C File Offset: 0x00031E4C
		public double GetOutVal(double theInVal)
		{
			switch (this.mRamp)
			{
			case 0:
			case 1:
				if (this.mMode == 2)
				{
					if (theInVal - this.mInMin <= (this.mInMax - this.mInMin) / 2.0)
					{
						return this.mOutMin + (theInVal - this.mInMin) / (this.mInMax - this.mInMin) * (this.mOutMax - this.mOutMin) * 2.0;
					}
					return this.mOutMin + (1.0 - (theInVal - this.mInMin) / (this.mInMax - this.mInMin)) * (this.mOutMax - this.mOutMin) * 2.0;
				}
				else
				{
					if (this.mInMin == this.mInMax)
					{
						return this.mOutMin;
					}
					return this.mOutMin + (theInVal - this.mInMin) / (this.mInMax - this.mInMin) * (this.mOutMax - this.mOutMin);
				}
				break;
			case 2:
			{
				double num = (theInVal - this.mInMin) / (this.mInMax - this.mInMin) * CurvedVal.PI / 2.0;
				if (this.mMode == 2)
				{
					num *= 2.0;
				}
				if (num > CurvedVal.PI / 2.0)
				{
					num = CurvedVal.PI - num;
				}
				return this.mOutMin + (1.0 - Math.Cos(num)) * (this.mOutMax - this.mOutMin);
			}
			case 3:
			{
				double num2 = (theInVal - this.mInMin) / (this.mInMax - this.mInMin) * CurvedVal.PI / 2.0;
				if (this.mMode == 2)
				{
					num2 *= 2.0;
				}
				return this.mOutMin + Math.Sin(num2) * (this.mOutMax - this.mOutMin);
			}
			case 4:
			{
				double num3 = (theInVal - this.mInMin) / (this.mInMax - this.mInMin) * CurvedVal.PI;
				if (this.mMode == 2)
				{
					num3 *= 2.0;
				}
				return this.mOutMin + (-Math.Cos(num3) + 1.0) / 2.0 * (this.mOutMax - this.mOutMin);
			}
			case 5:
			{
				double num4 = (theInVal - this.mInMin) / (this.mInMax - this.mInMin) * CurvedVal.PI;
				if (this.mMode == 2)
				{
					num4 *= 2.0;
				}
				if (num4 > CurvedVal.PI)
				{
					num4 = CurvedVal.PI * 2.0 - num4;
				}
				if (num4 < CurvedVal.PI / 2.0)
				{
					return this.mOutMin + Math.Sin(num4) / 2.0 * (this.mOutMax - this.mOutMin);
				}
				return this.mOutMin + (2.0 - Math.Sin(num4)) / 2.0 * (this.mOutMax - this.mOutMin);
			}
			case 6:
			{
				this.CheckCurveChange();
				if (this.mCurveCacheRecord == null)
				{
					return 0.0;
				}
				if (this.mInMax - this.mInMin == 0.0)
				{
					return 0.0;
				}
				float num5 = (float)Math.Min((theInVal - this.mInMin) / (this.mInMax - this.mInMin), 1.0);
				if (this.mMode == 2)
				{
					if (num5 > 0.5f)
					{
						num5 = (1f - num5) * 2f;
					}
					else
					{
						num5 *= 2f;
					}
				}
				if (this.mIsHermite)
				{
					double num6 = this.mOutMin + (double)this.mCurveCacheRecord.mHermiteCurve.Evaluate(num5) * (this.mOutMax - this.mOutMin);
					if (!this.mNoClip)
					{
						if (this.mOutMin < this.mOutMax)
						{
							num6 = Math.Min(Math.Max(num6, this.mOutMin), this.mOutMax);
						}
						else
						{
							num6 = Math.Max(Math.Min(num6, this.mOutMin), this.mOutMax);
						}
					}
					return num6;
				}
				float num7 = num5 * (float)(CurvedVal.CV_NUM_SPLINE_POINTS - 1);
				int num8 = (int)num7;
				if (num8 == CurvedVal.CV_NUM_SPLINE_POINTS - 1)
				{
					return this.mOutMin + (double)this.mCurveCacheRecord.mTable[num8] * (this.mOutMax - this.mOutMin);
				}
				float num9 = num7 - (float)num8;
				return this.mOutMin + (double)(this.mCurveCacheRecord.mTable[num8] * (1f - num9) + this.mCurveCacheRecord.mTable[num8 + 1] * num9) * (this.mOutMax - this.mOutMin);
			}
			default:
				return this.mOutMin;
			}
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x000340FF File Offset: 0x000322FF
		public double GetOutValDelta()
		{
			return this.GetOutVal() - this.mPrevOutVal;
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0003410E File Offset: 0x0003230E
		public double GetOutFinalVal()
		{
			return this.GetOutVal(this.mInMax);
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0003411C File Offset: 0x0003231C
		public double GetInVal()
		{
			double num = this.mInVal;
			if (this.mLinkedVal != null)
			{
				if (this.mLinkedVal.mOutputSync)
				{
					num = this.mLinkedVal.GetOutVal();
				}
				else
				{
					num = this.mLinkedVal.GetInVal();
				}
			}
			else if (this.mAutoInc)
			{
				if (this.mAppUpdateCountSrc != 0)
				{
					num = this.mInMin + (double)(this.mAppUpdateCountSrc - this.mInitAppUpdateCount) * this.mIncRate;
				}
				if (this.mMode == 1 || this.mMode == 2)
				{
					num = Math.IEEERemainder(num - this.mInMin, this.mInMax - this.mInMin) + this.mInMin;
				}
				else
				{
					num = Math.Min(num, this.mInMax);
				}
			}
			if (this.mMode != 2)
			{
				return num;
			}
			double num2 = (double)((float)((num - this.mInMin) / (this.mInMax - this.mInMin)));
			if (num2 > 0.5)
			{
				return this.mInMin + (1.0 - num2) * 2.0 * (this.mInMax - this.mInMin);
			}
			return this.mInMin + num2 * 2.0 * (this.mInMax - this.mInMin);
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0003424C File Offset: 0x0003244C
		public bool SetInVal(double theVal)
		{
			return this.SetInVal(theVal, false);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00034258 File Offset: 0x00032458
		public bool SetInVal(double theVal, bool theRealignAutoInc)
		{
			this.mPrevOutVal = this.GetOutVal();
			this.mTriggered = false;
			this.mPrevInVal = theVal;
			if (this.mAutoInc && theRealignAutoInc)
			{
				this.mInitAppUpdateCount -= (int)((theVal - this.mInVal) * 100.0);
			}
			this.mInVal = theVal;
			if (this.CheckClamping())
			{
				return true;
			}
			if (!this.mTriggered)
			{
				this.mTriggered = true;
				return false;
			}
			return this.mSingleTrigger;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x000342D4 File Offset: 0x000324D4
		public bool IncInVal(double theInc)
		{
			this.mPrevOutVal = this.GetOutVal();
			this.mPrevInVal = this.mInVal;
			this.mInVal += theInc;
			if (this.CheckClamping())
			{
				return true;
			}
			if (!this.mTriggered)
			{
				this.mTriggered = true;
				return false;
			}
			return this.mSingleTrigger;
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0003432A File Offset: 0x0003252A
		public bool IncInVal()
		{
			return this.mIncRate != 0.0 && this.IncInVal(this.mIncRate);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0003434C File Offset: 0x0003254C
		public void Intercept(string theDataP, CurvedVal theInterceptCv, double theCheckInIncrPct, bool theStopAtLocalMin)
		{
			double theTargetOutVal = ((theInterceptCv == null) ? this : theInterceptCv);
			this.SetCurve(theDataP);
			this.SetInVal(this.FindClosestInToOutVal(theTargetOutVal, theCheckInIncrPct, 0.0, 1.0, theStopAtLocalMin), true);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00034392 File Offset: 0x00032592
		public void Intercept(string theData, CurvedVal theInterceptCv, double theCheckInIncrPct)
		{
			this.Intercept(theData, theInterceptCv, theCheckInIncrPct, false);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0003439E File Offset: 0x0003259E
		public void Intercept(string theData, CurvedVal theInterceptCv)
		{
			this.Intercept(theData, theInterceptCv, 0.01, false);
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x000343B2 File Offset: 0x000325B2
		public void Intercept(string theData)
		{
			this.Intercept(theData, null, 0.01, false);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x000343C6 File Offset: 0x000325C6
		public double FindClosestInToOutVal(double theTargetOutVal, double theCheckInIncrPct, double theCheckInRangeMinPct, double theCheckInRangeMaxPct)
		{
			return this.FindClosestInToOutVal(theTargetOutVal, theCheckInIncrPct, theCheckInRangeMinPct, theCheckInRangeMaxPct, false);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x000343D4 File Offset: 0x000325D4
		public double FindClosestInToOutVal(double theTargetOutVal, double theCheckInIncrPct, double theCheckInRangeMinPct)
		{
			return this.FindClosestInToOutVal(theTargetOutVal, theCheckInIncrPct, theCheckInRangeMinPct, 1.0, false);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x000343E9 File Offset: 0x000325E9
		public double FindClosestInToOutVal(double theTargetOutVal, double theCheckInIncrPct)
		{
			return this.FindClosestInToOutVal(theTargetOutVal, theCheckInIncrPct, 0.0, 1.0, false);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00034406 File Offset: 0x00032606
		public double FindClosestInToOutVal(double theTargetOutVal)
		{
			return this.FindClosestInToOutVal(theTargetOutVal, 0.01, 0.0, 1.0, false);
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0003442C File Offset: 0x0003262C
		public double FindClosestInToOutVal(double theTargetOutVal, double theCheckInIncrPct, double theCheckInRangeMinPct, double theCheckInRangeMaxPct, bool theStopAtLocalMin)
		{
			double num = this.mInMax - this.mInMin;
			double num2 = this.mInMin + num * theCheckInRangeMaxPct;
			double num3 = 0.0;
			double num4 = -1.0;
			for (double num5 = this.mInMin + num * theCheckInRangeMinPct; num5 <= num2; num5 += num * theCheckInIncrPct)
			{
				double num6 = Math.Abs(theTargetOutVal - this.GetOutVal(num5));
				if (num4 < 0.0 || num6 < num3)
				{
					num3 = num6;
					num4 = num5;
				}
				else if (theStopAtLocalMin)
				{
					return num4;
				}
			}
			return num4;
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x000344B3 File Offset: 0x000326B3
		public double GetInValAtUpdate(int theUpdateCount)
		{
			return this.mInMin + (double)theUpdateCount * this.mIncRate;
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x000344C5 File Offset: 0x000326C5
		public int GetLengthInUpdates()
		{
			if (this.mIncRate == 0.0)
			{
				return -1;
			}
			return (int)Math.Ceiling((this.mInMax - this.mInMin) / this.mIncRate);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x000344F4 File Offset: 0x000326F4
		public bool CheckInThreshold(double theInVal)
		{
			double inVal = this.mInVal;
			double num = this.mPrevInVal;
			if (this.mAutoInc)
			{
				inVal = this.GetInVal();
				num = inVal - this.mIncRate * 1.5;
			}
			return theInVal > num && theInVal <= inVal;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0003453E File Offset: 0x0003273E
		public bool CheckUpdatesFromEndThreshold(int theUpdateCount)
		{
			return this.CheckInThreshold(this.GetInValAtUpdate(this.GetLengthInUpdates() - theUpdateCount));
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00034554 File Offset: 0x00032754
		public bool HasBeenTriggered()
		{
			if (this.mAutoInc)
			{
				this.mTriggered = this.GetInVal() == this.mInMax;
			}
			return this.mTriggered;
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00034578 File Offset: 0x00032778
		public void ClearTrigger()
		{
			this.mTriggered = false;
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x00034581 File Offset: 0x00032781
		public bool IsDoingCurve()
		{
			return this.GetInVal() != this.mInMax && this.mRamp != 0;
		}

		// Token: 0x0400085A RID: 2138
		public static int DFLAG_NOCLIP = 1;

		// Token: 0x0400085B RID: 2139
		public static int DFLAG_SINGLETRIGGER = 2;

		// Token: 0x0400085C RID: 2140
		public static int DFLAG_OUTPUTSYNC = 4;

		// Token: 0x0400085D RID: 2141
		public static int DFLAG_HERMITE = 8;

		// Token: 0x0400085E RID: 2142
		public static int DFLAG_AUTOINC = 16;

		// Token: 0x0400085F RID: 2143
		public static int CV_NUM_SPLINE_POINTS = 256;

		// Token: 0x04000860 RID: 2144
		public static double PI = 3.141590118408203;

		// Token: 0x04000861 RID: 2145
		public static Dictionary<string, CurvedVal.CurveCacheRecord> mCurveCacheMap = new Dictionary<string, CurvedVal.CurveCacheRecord>();

		// Token: 0x04000862 RID: 2146
		public static CurvedVal gsFastCurveData = new CurvedVal();

		// Token: 0x04000863 RID: 2147
		public double mIncRate;

		// Token: 0x04000864 RID: 2148
		public double mOutMin;

		// Token: 0x04000865 RID: 2149
		public double mOutMax;

		// Token: 0x04000866 RID: 2150
		public string mDataP;

		// Token: 0x04000867 RID: 2151
		public string mCurDataPStr;

		// Token: 0x04000868 RID: 2152
		public int mInitAppUpdateCount;

		// Token: 0x04000869 RID: 2153
		public CurvedVal mLinkedVal;

		// Token: 0x0400086A RID: 2154
		public CurvedVal.CurveCacheRecord mCurveCacheRecord;

		// Token: 0x0400086B RID: 2155
		public double mCurOutVal;

		// Token: 0x0400086C RID: 2156
		public double mPrevOutVal;

		// Token: 0x0400086D RID: 2157
		public double mInMin;

		// Token: 0x0400086E RID: 2158
		public double mInMax;

		// Token: 0x0400086F RID: 2159
		public byte mMode;

		// Token: 0x04000870 RID: 2160
		public byte mRamp;

		// Token: 0x04000871 RID: 2161
		public bool mNoClip;

		// Token: 0x04000872 RID: 2162
		public bool mSingleTrigger;

		// Token: 0x04000873 RID: 2163
		public bool mOutputSync;

		// Token: 0x04000874 RID: 2164
		public bool mTriggered;

		// Token: 0x04000875 RID: 2165
		public bool mIsHermite;

		// Token: 0x04000876 RID: 2166
		public bool mAutoInc;

		// Token: 0x04000877 RID: 2167
		public double mPrevInVal;

		// Token: 0x04000878 RID: 2168
		public double mInVal;

		// Token: 0x02000129 RID: 297
		public enum Mode
		{
			// Token: 0x0400087A RID: 2170
			MODE_CLAMP,
			// Token: 0x0400087B RID: 2171
			MODE_REPEAT,
			// Token: 0x0400087C RID: 2172
			MODE_PING_PONG
		}

		// Token: 0x0200012A RID: 298
		public enum Ramp
		{
			// Token: 0x0400087E RID: 2174
			RAMP_NONE,
			// Token: 0x0400087F RID: 2175
			RAMP_LINEAR,
			// Token: 0x04000880 RID: 2176
			RAMP_SLOW_TO_FAST,
			// Token: 0x04000881 RID: 2177
			RAMP_FAST_TO_SLOW,
			// Token: 0x04000882 RID: 2178
			RAMP_SLOW_FAST_SLOW,
			// Token: 0x04000883 RID: 2179
			RAMP_FAST_SLOW_FAST,
			// Token: 0x04000884 RID: 2180
			RAMP_CURVEDATA
		}

		// Token: 0x0200012B RID: 299
		public class DataPoint
		{
			// Token: 0x04000885 RID: 2181
			public float mX;

			// Token: 0x04000886 RID: 2182
			public float mY;

			// Token: 0x04000887 RID: 2183
			public float mAngleDeg;
		}

		// Token: 0x0200012C RID: 300
		public class CurveCacheRecord
		{
			// Token: 0x04000888 RID: 2184
			public float[] mTable = new float[CurvedVal.CV_NUM_SPLINE_POINTS];

			// Token: 0x04000889 RID: 2185
			public SexyMathHermite mHermiteCurve = new SexyMathHermite();

			// Token: 0x0400088A RID: 2186
			public string mDataStr;
		}
	}
}
