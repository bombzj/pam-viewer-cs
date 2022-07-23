using System;

namespace SexyFramework.Misc
{
	// Token: 0x0200014A RID: 330
	public interface SexyMatrix3
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000B1D RID: 2845
		// (set) Token: 0x06000B1C RID: 2844
		float m00 { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000B1F RID: 2847
		// (set) Token: 0x06000B1E RID: 2846
		float m01 { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000B21 RID: 2849
		// (set) Token: 0x06000B20 RID: 2848
		float m02 { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000B23 RID: 2851
		// (set) Token: 0x06000B22 RID: 2850
		float m10 { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000B25 RID: 2853
		// (set) Token: 0x06000B24 RID: 2852
		float m11 { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000B27 RID: 2855
		// (set) Token: 0x06000B26 RID: 2854
		float m12 { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000B29 RID: 2857
		// (set) Token: 0x06000B28 RID: 2856
		float m20 { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000B2B RID: 2859
		// (set) Token: 0x06000B2A RID: 2858
		float m21 { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000B2D RID: 2861
		// (set) Token: 0x06000B2C RID: 2860
		float m22 { get; set; }

		// Token: 0x06000B2E RID: 2862
		void ZeroMatrix();

		// Token: 0x06000B2F RID: 2863
		void LoadIdentity();
	}
}
