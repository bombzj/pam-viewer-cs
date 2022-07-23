using System;
using System.Collections.Generic;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x020000A9 RID: 169
	public class Fog : Effect
	{
		// Token: 0x06000A38 RID: 2616 RVA: 0x000618D5 File Offset: 0x0005FAD5
		private static float GetAlphaTimeRange()
		{
			return (float)SexyFramework.Common.IntRange(Common._M(150), Common._M1(500));
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x000618F1 File Offset: 0x0005FAF1
		private static float GetSizeTimeRange()
		{
			return (float)SexyFramework.Common.IntRange(Common._M(200), Common._M1(500));
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0006190D File Offset: 0x0005FB0D
		private static float GetSizeRange()
		{
			return SexyFramework.Common.FloatRange(Common._M(0.75f), Common._M1(1.25f));
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00061928 File Offset: 0x0005FB28
		protected override void Init()
		{
			this.mFogElements.Clear();
			Rect[] array = new Rect[]
			{
				new Rect(0, 0, Common._S(Common._M(50)), GameApp.gApp.mHeight),
				new Rect(0, 0, GameApp.gApp.mWidth, Common._S(Common._M1(50))),
				new Rect(GameApp.gApp.mWidth - Common._S(Common._M(50)), 0, Common._S(Common._M1(50)), GameApp.gApp.mHeight),
				new Rect(0, GameApp.gApp.mHeight - Common._S(Common._M2(50)), GameApp.gApp.mWidth, Common._S(Common._M3(50)))
			};
			for (int i = 0; i < 4; i++)
			{
				this.SetupSide(array[i]);
			}
			this.mFogElements.Sort(new ImageSort());
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x00061A47 File Offset: 0x0005FC47
		protected void SetupSide(Rect r)
		{
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x00061A49 File Offset: 0x0005FC49
		protected void DoDraw(Graphics g, bool under)
		{
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x00061A4B File Offset: 0x0005FC4B
		public Fog()
		{
			this.mResGroup = "Boss6_StoneHead";
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x00061A69 File Offset: 0x0005FC69
		public override string GetName()
		{
			return "Fog";
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00061A70 File Offset: 0x0005FC70
		public override void DrawUnderBackground(Graphics g)
		{
			if (!g.Is3D() || this.mForceAllDrawOverBalls)
			{
				return;
			}
			this.DoDraw(g, true);
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00061A8B File Offset: 0x0005FC8B
		public override void DrawAboveBalls(Graphics g)
		{
			if (!g.Is3D() || this.mForceAllDrawOverBalls)
			{
				return;
			}
			this.DoDraw(g, true);
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x00061AA6 File Offset: 0x0005FCA6
		public override void Update()
		{
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00061AA8 File Offset: 0x0005FCA8
		public override void SetParams(string key, string value)
		{
		}

		// Token: 0x040008AA RID: 2218
		private static int MAX_ALPHA = 220;

		// Token: 0x040008AB RID: 2219
		protected List<FogElement> mFogElements = new List<FogElement>();

		// Token: 0x040008AC RID: 2220
		protected bool mForceAllDrawOverBalls;
	}
}
