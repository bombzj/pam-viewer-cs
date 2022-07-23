using System;

namespace JeffLib
{
	// Token: 0x0200010F RID: 271
	public class Oscillator
	{
		// Token: 0x0600083B RID: 2107 RVA: 0x0002A194 File Offset: 0x00028394
		public void Init(float min_val, float max_val, bool start_at_max, float accel)
		{
			this.mAccel = accel;
			this.mMinVal = min_val;
			this.mMaxVal = max_val;
			this.mInc = 0f;
			if (start_at_max)
			{
				this.mVal = this.mMaxVal;
				this.mForward = false;
				return;
			}
			this.mVal = this.mMinVal;
			this.mForward = true;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0002A1EC File Offset: 0x000283EC
		public void Update()
		{
			if (this.mForward)
			{
				this.mInc += this.mAccel;
				this.mVal += this.mInc;
				if (this.mVal >= this.mMaxVal)
				{
					this.mVal = this.mMaxVal;
					this.mForward = false;
					return;
				}
			}
			else
			{
				this.mInc -= this.mAccel;
				this.mVal += this.mInc;
				if (this.mVal <= this.mMinVal)
				{
					this.mVal = this.mMinVal;
					this.mForward = true;
				}
			}
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0002A290 File Offset: 0x00028490
		public float GetVal()
		{
			return this.mVal;
		}

		// Token: 0x04000796 RID: 1942
		public float mVal;

		// Token: 0x04000797 RID: 1943
		public float mMinVal;

		// Token: 0x04000798 RID: 1944
		public float mMaxVal;

		// Token: 0x04000799 RID: 1945
		public float mInc;

		// Token: 0x0400079A RID: 1946
		public float mAccel;

		// Token: 0x0400079B RID: 1947
		public bool mForward;
	}
}
