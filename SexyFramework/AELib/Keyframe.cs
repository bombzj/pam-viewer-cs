using System;
using System.Collections.Generic;

namespace SexyFramework.AELib
{
	// Token: 0x0200000D RID: 13
	public class Keyframe : IComparable
	{
		// Token: 0x0600005D RID: 93 RVA: 0x0000393C File Offset: 0x00001B3C
		public Keyframe()
		{
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003944 File Offset: 0x00001B44
		public Keyframe(Keyframe rhs)
		{
			this.mFrame = rhs.mFrame;
			this.mValue1 = rhs.mValue1;
			this.mValue2 = rhs.mValue2;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003970 File Offset: 0x00001B70
		public Keyframe(int frame, float v1, float v2)
		{
			this.mFrame = frame;
			this.mValue1 = v1;
			this.mValue2 = v2;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000398D File Offset: 0x00001B8D
		public Keyframe(int frame, float v1)
		{
			this.mFrame = frame;
			this.mValue1 = v1;
			this.mValue2 = 0f;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000039B0 File Offset: 0x00001BB0
		public int CompareTo(object obj)
		{
			Keyframe keyframe = obj as Keyframe;
			if (keyframe != null)
			{
				return this.mFrame - keyframe.mFrame;
			}
			throw new ArgumentException("object is not a Keyframe.");
		}

		// Token: 0x04000032 RID: 50
		public int mFrame;

		// Token: 0x04000033 RID: 51
		public float mValue1;

		// Token: 0x04000034 RID: 52
		public float mValue2;

		// Token: 0x0200000E RID: 14
		public class KeyFrameSort : Comparer<Keyframe>
		{
			// Token: 0x06000062 RID: 98 RVA: 0x000039DF File Offset: 0x00001BDF
			public override int Compare(Keyframe x, Keyframe y)
			{
				if (x.mFrame < y.mFrame)
				{
					return -1;
				}
				if (x.mFrame > y.mFrame)
				{
					return 1;
				}
				return 0;
			}
		}
	}
}
