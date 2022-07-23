using System;
using SexyFramework.GraphicsLib;

namespace SexyFramework
{
	// Token: 0x0200012D RID: 301
	public class FastCurve
	{
		// Token: 0x060009FF RID: 2559 RVA: 0x00034624 File Offset: 0x00032824
		protected void InitFromCurveData()
		{
			this.mTriggered = false;
			this.mOutputSync = CurvedVal.gsFastCurveData.mOutputSync;
			this.mSingleTrigger = CurvedVal.gsFastCurveData.mSingleTrigger;
			this.mOutMin = (float)CurvedVal.gsFastCurveData.mOutMin;
			this.mOutMax = (float)CurvedVal.gsFastCurveData.mOutMax;
			this.mInVal = (float)CurvedVal.gsFastCurveData.mInVal;
			this.mInMin = (float)CurvedVal.gsFastCurveData.mInMin;
			this.mInMax = (float)CurvedVal.gsFastCurveData.mInMax;
			this.mIncRate = (float)CurvedVal.gsFastCurveData.mIncRate;
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x000346C0 File Offset: 0x000328C0
		public FastCurve()
		{
			this.mOutMin = 0f;
			this.mOutMax = 1f;
			this.mInMin = 0f;
			this.mInMax = 1f;
			this.mTriggered = false;
			this.mIncRate = 0f;
			this.mInVal = 0f;
			this.mSingleTrigger = false;
			this.mOutputSync = false;
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0003472A File Offset: 0x0003292A
		public FastCurve(string theData)
			: this(theData, null)
		{
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x00034734 File Offset: 0x00032934
		public FastCurve(string theData, CurvedVal theLinkedVal)
		{
			CurvedVal.gsFastCurveData.SetCurve(theData);
			this.InitFromCurveData();
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0003474D File Offset: 0x0003294D
		public void SetCurve(string theDataP)
		{
			this.SetCurve(theDataP, null);
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x00034757 File Offset: 0x00032957
		public void SetCurve(string theDataP, CurvedVal theLinkedVal)
		{
			CurvedVal.gsFastCurveData.SetCurve(theDataP);
			this.InitFromCurveData();
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0003476C File Offset: 0x0003296C
		public void SetConstant(float theValue)
		{
			this.mInVal = 0f;
			this.mTriggered = false;
			this.mInMin = (this.mInMax = 0f);
			this.mOutMax = theValue;
			this.mOutMin = theValue;
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x000347AF File Offset: 0x000329AF
		public float GetOutVal()
		{
			return this.GetOutVal(this.mInVal);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x000347BD File Offset: 0x000329BD
		public float GetOutVal(float theInVal)
		{
			return this.mOutMax;
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x000347C5 File Offset: 0x000329C5
		public float GetOutFinalVal()
		{
			return this.GetOutVal(this.mInMax);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x000347D3 File Offset: 0x000329D3
		public void SetOutRange(float theMin, float theMax)
		{
			this.mOutMin = theMin;
			this.mOutMax = theMax;
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x000347E3 File Offset: 0x000329E3
		public void SetInRange(float theMin, float theMax)
		{
			this.mInMin = theMin;
			this.mInMax = theMax;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x000347F3 File Offset: 0x000329F3
		public float GetInVal()
		{
			return this.mInVal;
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x000347FB File Offset: 0x000329FB
		public bool SetInVal(float theVal)
		{
			return this.SetInVal(theVal, false);
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x00034805 File Offset: 0x00032A05
		public bool SetInVal(float theVal, bool theRealignAutoInc)
		{
			this.mInVal = theVal;
			return false;
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00034810 File Offset: 0x00032A10
		public bool IncInVal(float theInc)
		{
			this.mInVal += theInc;
			bool flag = false;
			if (this.mInVal > this.mInMax)
			{
				this.mInVal = this.mInMax;
				flag = true;
			}
			else if (this.mInVal < this.mInMin)
			{
				this.mInVal = this.mInMin;
				flag = true;
			}
			if (!flag)
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

		// Token: 0x06000A0F RID: 2575 RVA: 0x00034883 File Offset: 0x00032A83
		public bool IncInVal()
		{
			return this.IncInVal(this.mIncRate);
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00034891 File Offset: 0x00032A91
		public bool HasBeenTriggered()
		{
			return this.mTriggered;
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x00034899 File Offset: 0x00032A99
		public void ClearTrigger()
		{
			this.mTriggered = false;
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x000348A2 File Offset: 0x00032AA2
		public static implicit operator float(FastCurve ImpliedObject)
		{
			return ImpliedObject.GetOutVal();
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x000348AB File Offset: 0x00032AAB
		public static implicit operator SexyColor(FastCurve ImpliedObject)
		{
			return new SexyColor(255, 255, 255, (int)(255f * ImpliedObject.GetOutVal()));
		}

		// Token: 0x0400088B RID: 2187
		public float mOutMin;

		// Token: 0x0400088C RID: 2188
		public float mOutMax;

		// Token: 0x0400088D RID: 2189
		public float mInMin;

		// Token: 0x0400088E RID: 2190
		public float mInMax;

		// Token: 0x0400088F RID: 2191
		public float mIncRate;

		// Token: 0x04000890 RID: 2192
		public float mInVal;

		// Token: 0x04000891 RID: 2193
		public bool mTriggered;

		// Token: 0x04000892 RID: 2194
		public bool mSingleTrigger;

		// Token: 0x04000893 RID: 2195
		public bool mOutputSync;
	}
}
