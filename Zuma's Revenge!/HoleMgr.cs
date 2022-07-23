using System;
using System.Linq;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x020000ED RID: 237
	public class HoleMgr
	{
		// Token: 0x06000CD9 RID: 3289 RVA: 0x0007CBD5 File Offset: 0x0007ADD5
		public HoleMgr()
		{
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0007CBEC File Offset: 0x0007ADEC
		public HoleMgr(HoleMgr rhs)
		{
			if (rhs == null)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				this.mHoles[i] = new HoleInfo(rhs.mHoles[i]);
			}
			this.mNumHoles = rhs.mNumHoles;
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0007CC3C File Offset: 0x0007AE3C
		protected void SetupHole(ref int x, ref int y, ref float rot)
		{
			x -= 48;
			y -= 48;
			while (rot < 0f)
			{
				rot += 6.28318f;
			}
			while (rot > 6.28318f)
			{
				rot -= 6.28318f;
			}
			if ((double)Math.Abs(rot) < 0.2)
			{
				rot = 0f;
				return;
			}
			if ((double)Math.Abs(rot - 1.570795f) < 0.2)
			{
				rot = 1.570795f;
				return;
			}
			if ((double)Math.Abs(rot - 3.14159f) < 0.2)
			{
				rot = 3.14159f;
				return;
			}
			if ((double)Math.Abs(rot - 4.712385f) < 0.2)
			{
				rot = 4.712385f;
				return;
			}
			if ((double)Math.Abs(rot - 6.28318f) < 0.2)
			{
				rot = 0f;
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0007CD20 File Offset: 0x0007AF20
		public int PlaceHole(int curve_num, int x, int y, float rot, bool visible)
		{
			this.SetupHole(ref x, ref y, ref rot);
			HoleInfo holeInfo = new HoleInfo();
			holeInfo.mX = x;
			holeInfo.mY = y;
			holeInfo.mFrame = 0;
			holeInfo.mRotation = rot;
			holeInfo.mPercentOpen = 0f;
			holeInfo.mVisible = visible;
			holeInfo.mCurve = null;
			holeInfo.mCurveNum = curve_num;
			this.mHoles[this.mNumHoles++] = holeInfo;
			return this.mNumHoles - 1;
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0007CD9E File Offset: 0x0007AF9E
		public int PlaceHole(int curve_num, int x, int y, float rot)
		{
			return this.PlaceHole(curve_num, x, y, rot, true);
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0007CDAC File Offset: 0x0007AFAC
		public void UpdateHoleInfo(int hole_index, int x, int y, float rot, bool visible)
		{
			HoleInfo holeInfo = this.mHoles[hole_index];
			this.SetupHole(ref x, ref y, ref rot);
			holeInfo.mX = x;
			holeInfo.mY = y;
			holeInfo.mRotation = rot;
			holeInfo.mVisible = visible;
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0007CDEC File Offset: 0x0007AFEC
		public void UpdateHoleInfo(int hole_index, int x, int y, float rot)
		{
			this.UpdateHoleInfo(hole_index, x, y, rot, true);
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x0007CDFC File Offset: 0x0007AFFC
		public void Update()
		{
			for (int i = 0; i < this.mNumHoles; i++)
			{
				HoleInfo holeInfo = this.mHoles[i];
				for (int j = 0; j < holeInfo.mShared.Count; j++)
				{
					HoleInfo holeInfo2 = this.mHoles[holeInfo.mShared[j]];
					if (holeInfo.GetPctOpen() > holeInfo2.GetPctOpen())
					{
						holeInfo2.SetPctOpen(holeInfo.GetPctOpen());
					}
				}
				holeInfo.Update();
			}
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0007CE70 File Offset: 0x0007B070
		public void SetPctOpen(int curve_num, float pct_open)
		{
			this.mHoles[curve_num].SetPctOpen(pct_open);
			HoleInfo holeInfo = this.mHoles[curve_num];
			if (!holeInfo.mVisible)
			{
				for (int i = 0; i < Enumerable.Count<int>(holeInfo.mShared); i++)
				{
					HoleInfo holeInfo2 = this.mHoles[holeInfo.mShared[i]];
					if (holeInfo.GetPctOpen() > holeInfo2.GetPctOpen())
					{
						holeInfo2.SetPctOpen(holeInfo.GetPctOpen());
					}
				}
			}
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x0007CEE0 File Offset: 0x0007B0E0
		public void Draw(Graphics g)
		{
			float hilite_override = 0f;
			for (int i = 0; i < this.mNumHoles; i++)
			{
				if (!this.mHoles[i].mVisible && this.mHoles[i].mCurve != null && this.mHoles[i].mCurve.mInitialPathHilite)
				{
					hilite_override = this.mHoles[i].mCurve.mSkullHilite;
				}
			}
			for (int j = 0; j < this.mNumHoles; j++)
			{
				if (this.mHoles[j].mVisible)
				{
					this.mHoles[j].Draw(g, hilite_override);
				}
			}
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0007CF78 File Offset: 0x0007B178
		public void DrawRings(Graphics g)
		{
			for (int i = 0; i < this.mNumHoles; i++)
			{
				if (this.mHoles[i].mVisible)
				{
					this.mHoles[i].DrawRings(g);
				}
			}
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x0007CFB3 File Offset: 0x0007B1B3
		public int GetNumHoles()
		{
			return this.mNumHoles;
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0007CFBB File Offset: 0x0007B1BB
		public HoleInfo GetHole(int idx)
		{
			if (idx < 0 || idx >= this.mNumHoles)
			{
				return null;
			}
			return this.mHoles[idx];
		}

		// Token: 0x04000B6F RID: 2927
		protected HoleInfo[] mHoles = new HoleInfo[4];

		// Token: 0x04000B70 RID: 2928
		protected int mNumHoles;
	}
}
