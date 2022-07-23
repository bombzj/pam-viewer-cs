using System;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x0200014E RID: 334
	public class WayPoint
	{
		// Token: 0x0600104F RID: 4175 RVA: 0x000A668F File Offset: 0x000A488F
		public WayPoint()
		{
			this.mHavePerpendicular = false;
			this.mHaveAvgRotation = false;
			this.mInTunnel = false;
			this.mHavePerpendicular = false;
			this.mPriority = 0;
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x000A66C8 File Offset: 0x000A48C8
		public WayPoint(float theX, float theY)
		{
			this.x = theX;
			this.y = theY;
			this.mHavePerpendicular = false;
			this.mHaveAvgRotation = false;
			this.mInTunnel = false;
			this.mHavePerpendicular = false;
			this.mPriority = 0;
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x000A6718 File Offset: 0x000A4918
		public static float GetCanonicalAngle(float r)
		{
			if (r > 0f)
			{
				while (r > 3.1415927f)
				{
					r -= 6.2831855f;
				}
			}
			else if (r < 0f)
			{
				while (r < -3.1415927f)
				{
					r += 6.2831855f;
				}
			}
			return r;
		}

		// Token: 0x04001AB0 RID: 6832
		public float x;

		// Token: 0x04001AB1 RID: 6833
		public float y;

		// Token: 0x04001AB2 RID: 6834
		public bool mHavePerpendicular;

		// Token: 0x04001AB3 RID: 6835
		public bool mHaveAvgRotation;

		// Token: 0x04001AB4 RID: 6836
		public SexyVector3 mPerpendicular = default(SexyVector3);

		// Token: 0x04001AB5 RID: 6837
		public float mRotation;

		// Token: 0x04001AB6 RID: 6838
		public float mAvgRotation;

		// Token: 0x04001AB7 RID: 6839
		public bool mInTunnel;

		// Token: 0x04001AB8 RID: 6840
		public byte mPriority;
	}
}
