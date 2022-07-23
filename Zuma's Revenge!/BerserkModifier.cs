using System;
using System.Globalization;

namespace ZumasRevenge
{
	// Token: 0x02000078 RID: 120
	public class BerserkModifier
	{
		// Token: 0x0600089B RID: 2203 RVA: 0x0004C8F8 File Offset: 0x0004AAF8
		public BerserkModifier(BerserkModifier rhs)
		{
			this.mParamName = rhs.mParamName;
			this.mStringValue = rhs.mStringValue;
			this.mMinStr = rhs.mMinStr;
			this.mMaxStr = rhs.mMaxStr;
			this.mOverride = rhs.mOverride;
			this.mParamType = rhs.mParamType;
			this.mHasMin = rhs.mHasMin;
			this.mHasMax = rhs.mHasMax;
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0004C96C File Offset: 0x0004AB6C
		public BerserkModifier(string p, string value, string minval, string maxval, bool _override)
		{
			this.mParamName = p;
			this.mStringValue = value;
			this.mHasMin = (this.mHasMax = false);
			this.mOverride = _override;
			if (minval != null && minval.Length > 0)
			{
				this.mHasMin = true;
				this.mMinStr = minval;
			}
			if (maxval != null && maxval.Length > 0)
			{
				this.mHasMax = true;
				this.mMaxStr = maxval;
			}
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0004C9DC File Offset: 0x0004ABDC
		public BerserkModifier(string p, string value)
		{
			this.mParamName = p;
			this.mStringValue = value;
			this.mHasMin = (this.mHasMax = false);
			this.mOverride = false;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0004CA14 File Offset: 0x0004AC14
		public void AddPointerFloat(object fptr)
		{
			this.mParamType = 1;
			this.mVariablePtr = fptr;
			if (this.mStringValue[0] == '.')
			{
				this.mStringValue = "0" + this.mStringValue;
			}
			double num = 0.0;
			double.TryParse(this.mStringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out num);
			this.mValue = num;
			if (this.mHasMin)
			{
				this.mMin = Convert.ToSingle(this.mMinStr);
			}
			if (this.mHasMax)
			{
				this.mMax = Convert.ToSingle(this.mMaxStr);
			}
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0004CAC0 File Offset: 0x0004ACC0
		public void AddPointerInt(object iptr)
		{
			this.mParamType = 0;
			this.mVariablePtr = iptr;
			try
			{
				this.mValue = Convert.ToInt32(this.mStringValue);
			}
			catch (Exception)
			{
				this.mValue = 0;
			}
			if (this.mHasMin)
			{
				this.mMin = Convert.ToInt32(this.mMinStr);
			}
			if (this.mHasMax)
			{
				this.mMax = Convert.ToInt32(this.mMaxStr);
			}
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0004CB50 File Offset: 0x0004AD50
		public void AddPointerBool(object bptr)
		{
			this.mParamType = 2;
			this.mVariablePtr = bptr;
			this.mValue = Convert.ToBoolean(this.mStringValue);
			if (this.mHasMin)
			{
				this.mMin = Convert.ToBoolean(this.mMinStr);
			}
			if (this.mHasMax)
			{
				this.mMax = Convert.ToBoolean(this.mMaxStr);
			}
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0004CBC0 File Offset: 0x0004ADC0
		public void ModifyVariable()
		{
			if (this.mParamType == 1)
			{
				ParamData<float> paramData = this.mVariablePtr as ParamData<float>;
				if (this.mOverride)
				{
					paramData.value = Convert.ToSingle(this.mValue);
					return;
				}
				paramData.value += Convert.ToSingle(this.mValue);
				if (this.mHasMin && paramData.value < Convert.ToSingle(this.mMin))
				{
					paramData.value = Convert.ToSingle(this.mMin);
					return;
				}
				if (this.mHasMax && paramData.value > Convert.ToSingle(this.mMax))
				{
					paramData.value = Convert.ToSingle(this.mMax);
					return;
				}
			}
			else if (this.mParamType == 0)
			{
				ParamData<int> paramData2 = this.mVariablePtr as ParamData<int>;
				if (this.mOverride)
				{
					paramData2.value = Convert.ToInt32(this.mValue);
					return;
				}
				paramData2.value += Convert.ToInt32(this.mValue);
				if (this.mHasMin && paramData2.value < Convert.ToInt32(this.mMin))
				{
					paramData2.value = Convert.ToInt32(this.mMin);
					return;
				}
				if (this.mHasMax && paramData2.value > Convert.ToInt32(this.mMax))
				{
					paramData2.value = Convert.ToInt32(this.mMax);
					return;
				}
			}
			else if (this.mParamType == 2)
			{
				ParamData<bool> paramData3 = this.mVariablePtr as ParamData<bool>;
				paramData3.value = Convert.ToBoolean(this.mValue);
			}
		}

		// Token: 0x04000664 RID: 1636
		public string mParamName;

		// Token: 0x04000665 RID: 1637
		public string mStringValue;

		// Token: 0x04000666 RID: 1638
		public string mMinStr;

		// Token: 0x04000667 RID: 1639
		public string mMaxStr;

		// Token: 0x04000668 RID: 1640
		public bool mOverride;

		// Token: 0x04000669 RID: 1641
		public int mParamType;

		// Token: 0x0400066A RID: 1642
		protected object mValue;

		// Token: 0x0400066B RID: 1643
		protected object mVariablePtr;

		// Token: 0x0400066C RID: 1644
		protected object mMin;

		// Token: 0x0400066D RID: 1645
		protected object mMax;

		// Token: 0x0400066E RID: 1646
		protected bool mHasMin;

		// Token: 0x0400066F RID: 1647
		protected bool mHasMax;

		// Token: 0x02000079 RID: 121
		public enum DataType
		{
			// Token: 0x04000671 RID: 1649
			Type_Int,
			// Token: 0x04000672 RID: 1650
			Type_Float,
			// Token: 0x04000673 RID: 1651
			Type_Bool
		}
	}
}
