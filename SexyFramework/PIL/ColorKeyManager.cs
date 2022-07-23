using System;
using System.Collections.Generic;
using System.Linq;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.PIL
{
	// Token: 0x02000155 RID: 341
	public class ColorKeyManager
	{
		// Token: 0x06000BD7 RID: 3031 RVA: 0x00039560 File Offset: 0x00037760
		public ColorKeyManager()
		{
			this.mColorMode = 0;
			this.mLife = 0;
			this.mUpdateCount = 0;
			this.mUpdateImagePosColor = false;
			this.mGradientRepeat = 1;
			this.mCurrentColor = new SexyColor(SexyColor.White);
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x000395D4 File Offset: 0x000377D4
		public void CopyFrom(ColorKeyManager rhs)
		{
			if (rhs == null)
			{
				return;
			}
			this.mCurrentColor = rhs.mCurrentColor.Clone();
			this.mImage = rhs.mImage;
			this.mImgFileName = rhs.mImgFileName;
			this.mImgVariant = rhs.mImgVariant;
			this.mColorMode = rhs.mColorMode;
			this.mLife = rhs.mLife;
			this.mUpdateCount = rhs.mUpdateCount;
			this.mUpdateImagePosColor = rhs.mUpdateImagePosColor;
			this.mGradientRepeat = rhs.mGradientRepeat;
			this.mTimeline.Clear();
			this.mTimeline.AddRange(rhs.mTimeline.ToArray());
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00039677 File Offset: 0x00037877
		public virtual void Dispose()
		{
			this.mTimeline.Clear();
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00039684 File Offset: 0x00037884
		public void Serialize(SexyBuffer b)
		{
			b.WriteLong((long)this.mColorMode);
			b.WriteLong((long)this.mLife);
			b.WriteLong((long)this.mUpdateCount);
			b.WriteLong((long)this.mCurrentColor.ToInt());
			b.WriteLong((long)this.mTimeline.Count);
			for (int i = 0; i < this.mTimeline.Count; i++)
			{
				b.WriteFloat(this.mTimeline[i].first);
				this.mTimeline[i].second.Serialize(b);
			}
			b.WriteString(this.mImgFileName);
			b.WriteString(this.mImgVariant);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00039738 File Offset: 0x00037938
		public void Deserialize(SexyBuffer b)
		{
			this.mColorMode = (int)b.ReadLong();
			this.mLife = (int)b.ReadLong();
			this.mUpdateCount = (int)b.ReadLong();
			long num = b.ReadLong();
			this.mCurrentColor = new SexyColor((int)num);
			int num2 = (int)b.ReadLong();
			this.mTimeline.Clear();
			for (int i = 0; i < num2; i++)
			{
				float pt = b.ReadFloat();
				ColorKey colorKey = new ColorKey();
				colorKey.Deserialize(b);
				this.mTimeline.Add(new ColorKeyTimeEntry(pt, colorKey));
			}
			this.mImgFileName = b.ReadString();
			this.mImgVariant = b.ReadString();
			if (this.mImgFileName.Length > 0)
			{
				bool flag = false;
				this.mImage = GlobalMembers.gSexyAppBase.GetSharedImage(this.mImgFileName, this.mImgVariant, ref flag, true, false);
			}
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00039814 File Offset: 0x00037A14
		public void Update(float x, float y)
		{
			if (++this.mUpdateCount >= this.mLife || this.mColorMode == 0)
			{
				return;
			}
			if (this.mUpdateCount == 1 && this.mColorMode == 2)
			{
				int num = Common.Rand() % this.mTimeline.size<ColorKeyTimeEntry>();
				int num2 = num;
				if (this.mTimeline.size<ColorKeyTimeEntry>() > 1)
				{
					while (num2 == num)
					{
						num2 = Common.Rand() % this.mTimeline.size<ColorKeyTimeEntry>();
					}
				}
				this.mCurrentColor = this.mTimeline[num].second.GetInterpolatedColor(this.mTimeline[num2].second, Common.FloatRange(0f, 1f));
				return;
			}
			if (this.mUpdateCount == 1 && this.mColorMode == 3)
			{
				this.mCurrentColor = this.mTimeline[Common.Rand() % this.mTimeline.size<ColorKeyTimeEntry>()].second.GetColor();
				return;
			}
			if (this.mColorMode == 4)
			{
				if (this.mUpdateCount == 1)
				{
					return;
				}
				if (this.mUpdateImagePosColor)
				{
					return;
				}
			}
			if (this.mColorMode == 1)
			{
				float num3 = (float)this.mUpdateCount / (float)this.mLife;
				float num4 = 1f / (float)this.mGradientRepeat;
				float num5 = num3 - (float)((int)(num3 / num4)) * num4;
				num5 = Math.Max(Math.Min(num4, num5), 0f);
				int num6 = -1;
				int num7 = -1;
				for (int i = 0; i < this.mTimeline.size<ColorKeyTimeEntry>(); i++)
				{
					if (num5 < this.mTimeline[i].first / (float)this.mGradientRepeat)
					{
						num7 = i;
						break;
					}
					num6 = i;
				}
				if (num7 == -1)
				{
					num7 = num6;
				}
				float num8 = this.mTimeline[num6].first / (float)this.mGradientRepeat;
				float num9 = this.mTimeline[num7].first / (float)this.mGradientRepeat;
				float pct = (num5 - num8) / (num9 - num8);
				this.mCurrentColor = this.mTimeline[num6].second.GetInterpolatedColor(this.mTimeline[num7].second, pct);
			}
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00039A40 File Offset: 0x00037C40
		public void SetFixedColor(SexyColor c)
		{
			this.mCurrentColor = c;
			this.mColorMode = 3;
			this.mUpdateCount = 2;
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00039A58 File Offset: 0x00037C58
		public void AddColorKey(float pct, SexyColor c)
		{
			this.mTimeline.Add(new ColorKeyTimeEntry(pct, new ColorKey(c)));
			this.mTimeline.Sort(new SortColorKeys());
			this.mCurrentColor = this.mTimeline[0].second.GetColor();
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00039AA8 File Offset: 0x00037CA8
		public void AddAlphaKey(float pct, int alpha)
		{
			this.AddColorKey(pct, new SexyColor(255, 255, 255, alpha));
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00039AC6 File Offset: 0x00037CC6
		public void ForceTransition(int new_life, SexyColor final_color)
		{
			this.mUpdateCount = 0;
			this.mLife = new_life;
			this.mColorMode = 1;
			this.AddColorKey(0f, this.mCurrentColor);
			this.AddColorKey(1f, final_color);
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00039AFA File Offset: 0x00037CFA
		public SexyColor GetColor()
		{
			return this.mCurrentColor;
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00039B02 File Offset: 0x00037D02
		public void SetLife(int l)
		{
			this.mLife = l;
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00039B0B File Offset: 0x00037D0B
		public void SetColorMode(int m)
		{
			this.mColorMode = m;
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00039B14 File Offset: 0x00037D14
		public void SetImage(SharedImageRef r, string filename, string variant)
		{
			this.mImage = r;
			this.mImgFileName = filename;
			this.mImgVariant = variant;
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00039B2B File Offset: 0x00037D2B
		public bool HasMaxIndex()
		{
			return this.mTimeline.size<ColorKeyTimeEntry>() != 0 && Common._eq(Enumerable.Last<ColorKeyTimeEntry>(this.mTimeline).first, 1f, 1E-06f);
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00039B5B File Offset: 0x00037D5B
		public int GetNumKeys()
		{
			return this.mTimeline.size<ColorKeyTimeEntry>();
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00039B68 File Offset: 0x00037D68
		public int GetColorMode()
		{
			return this.mColorMode;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x00039B70 File Offset: 0x00037D70
		public SexyColor GetColorByIndex(int i)
		{
			return this.mTimeline[i].second.GetColor();
		}

		// Token: 0x04000969 RID: 2409
		protected List<ColorKeyTimeEntry> mTimeline = new List<ColorKeyTimeEntry>();

		// Token: 0x0400096A RID: 2410
		protected SexyColor mCurrentColor = default(SexyColor);

		// Token: 0x0400096B RID: 2411
		protected SharedImageRef mImage;

		// Token: 0x0400096C RID: 2412
		protected string mImgFileName = "";

		// Token: 0x0400096D RID: 2413
		protected string mImgVariant = "";

		// Token: 0x0400096E RID: 2414
		protected int mColorMode;

		// Token: 0x0400096F RID: 2415
		protected int mLife;

		// Token: 0x04000970 RID: 2416
		protected int mUpdateCount;

		// Token: 0x04000971 RID: 2417
		public bool mUpdateImagePosColor;

		// Token: 0x04000972 RID: 2418
		public int mGradientRepeat;
	}
}
