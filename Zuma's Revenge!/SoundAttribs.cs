using System;

namespace ZumasRevenge
{
	// Token: 0x02000140 RID: 320
	public class SoundAttribs
	{
		// Token: 0x06000FE5 RID: 4069 RVA: 0x000A068C File Offset: 0x0009E88C
		public SoundAttribs()
		{
			this.pan = 0;
			this.pitch = 0f;
			this.fadein = 1f;
			this.fadeout = 1f;
			this.delay = 0;
			this.stagger = 0;
			this.volume = 1f;
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x000A06E0 File Offset: 0x0009E8E0
		// (set) Token: 0x06000FE7 RID: 4071 RVA: 0x000A06E8 File Offset: 0x0009E8E8
		public int pan { get; set; }

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x000A06F1 File Offset: 0x0009E8F1
		// (set) Token: 0x06000FE9 RID: 4073 RVA: 0x000A06F9 File Offset: 0x0009E8F9
		public int delay { get; set; }

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000FEA RID: 4074 RVA: 0x000A0702 File Offset: 0x0009E902
		// (set) Token: 0x06000FEB RID: 4075 RVA: 0x000A070A File Offset: 0x0009E90A
		public int stagger { get; set; }

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000FEC RID: 4076 RVA: 0x000A0713 File Offset: 0x0009E913
		// (set) Token: 0x06000FED RID: 4077 RVA: 0x000A071B File Offset: 0x0009E91B
		public float fadein { get; set; }

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000FEE RID: 4078 RVA: 0x000A0724 File Offset: 0x0009E924
		// (set) Token: 0x06000FEF RID: 4079 RVA: 0x000A072C File Offset: 0x0009E92C
		public float fadeout { get; set; }

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x000A0735 File Offset: 0x0009E935
		// (set) Token: 0x06000FF1 RID: 4081 RVA: 0x000A073D File Offset: 0x0009E93D
		public float pitch { get; set; }

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x000A0746 File Offset: 0x0009E946
		// (set) Token: 0x06000FF3 RID: 4083 RVA: 0x000A074E File Offset: 0x0009E94E
		public float volume { get; set; }
	}
}
