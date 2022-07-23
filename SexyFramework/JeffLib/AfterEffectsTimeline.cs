using System;
using System.Collections.Generic;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace JeffLib
{
	// Token: 0x02000108 RID: 264
	public class AfterEffectsTimeline
	{
		// Token: 0x060007FB RID: 2043 RVA: 0x0002896C File Offset: 0x00026B6C
		public void AddPosX(Component x)
		{
			x.mStartFrame += this.mStartFrame;
			x.mEndFrame += this.mStartFrame;
			this.mPosX.Add(x);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x000289B0 File Offset: 0x00026BB0
		public void AddPosY(Component y)
		{
			y.mStartFrame += this.mStartFrame;
			y.mEndFrame += this.mStartFrame;
			this.mPosY.Add(y);
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x000289F4 File Offset: 0x00026BF4
		public void AddScaleX(Component c)
		{
			c.mStartFrame += this.mStartFrame;
			c.mEndFrame += this.mStartFrame;
			this.mScaleX.Add(c);
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00028A38 File Offset: 0x00026C38
		public void AddScaleY(Component c)
		{
			c.mStartFrame += this.mStartFrame;
			c.mEndFrame += this.mStartFrame;
			this.mScaleY.Add(c);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00028A7C File Offset: 0x00026C7C
		public void AddAngle(Component c)
		{
			c.mStartFrame += this.mStartFrame;
			c.mEndFrame += this.mStartFrame;
			this.mAngle.Add(c);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00028AC0 File Offset: 0x00026CC0
		public void AddOpacity(Component c)
		{
			c.mStartFrame += this.mStartFrame;
			c.mEndFrame += this.mStartFrame;
			this.mOpacity.Add(c);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00028B04 File Offset: 0x00026D04
		public void Update()
		{
			if (this.mUpdateCount <= this.mEndFrame)
			{
				this.mUpdateCount++;
			}
			if (this.mUpdateCount < this.mStartFrame || this.mUpdateCount > this.mEndFrame)
			{
				return;
			}
			Component.UpdateComponentVec(this.mPosX, this.mUpdateCount);
			Component.UpdateComponentVec(this.mPosY, this.mUpdateCount);
			Component.UpdateComponentVec(this.mScaleX, this.mUpdateCount);
			Component.UpdateComponentVec(this.mScaleY, this.mUpdateCount);
			Component.UpdateComponentVec(this.mAngle, this.mUpdateCount);
			Component.UpdateComponentVec(this.mOpacity, this.mUpdateCount);
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00028BB8 File Offset: 0x00026DB8
		public void Draw(Graphics g, int force_alpha)
		{
			if (this.mImage == null)
			{
				return;
			}
			if (this.mUpdateCount < this.mStartFrame || (this.mUpdateCount > this.mEndFrame && !this.mHoldLastFrame))
			{
				return;
			}
			int num = (int)(Component.GetComponentValue(this.mOpacity, 1f, this.mUpdateCount) * this.mOverallAlphaPct * 255f);
			if (num <= 0)
			{
				return;
			}
			if (num > 255)
			{
				num = 255;
			}
			if (force_alpha >= 0)
			{
				num = force_alpha;
			}
			if (num != 255)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, num);
			}
			Transform transform = new Transform();
			float componentValue = Component.GetComponentValue(this.mAngle, 0f, this.mUpdateCount);
			if (componentValue != 0f)
			{
				transform.RotateRad(componentValue);
			}
			float num3;
			float num2 = (num3 = Component.GetComponentValue(this.mScaleX, 1f, this.mUpdateCount) * this.mOverallXScale);
			if (this.mScaleY.size<Component>() > 0)
			{
				num2 = Component.GetComponentValue(this.mScaleY, 1f, this.mUpdateCount) * this.mOverallYScale;
			}
			if (this.mMirror)
			{
				num3 *= -1f;
			}
			if (!SexyFramework.Common._eq(num3, 1f) || !SexyFramework.Common._eq(num2, 1f))
			{
				transform.Scale(num3, num2);
			}
			float componentValue2 = Component.GetComponentValue(this.mPosX, 0f, this.mUpdateCount);
			float componentValue3 = Component.GetComponentValue(this.mPosY, 0f, this.mUpdateCount);
			Rect celRect = this.mImage.GetCelRect(this.mCel);
			if (g.Is3D())
			{
				g.DrawImageTransformF(this.mImage, transform, celRect, componentValue2, componentValue3);
			}
			else
			{
				g.DrawImageTransform(this.mImage, transform, celRect, componentValue2, componentValue3);
			}
			g.SetColorizeImages(false);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00028D7E File Offset: 0x00026F7E
		public void Draw(Graphics g)
		{
			this.Draw(g, -1);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00028D88 File Offset: 0x00026F88
		public bool Done()
		{
			return this.mUpdateCount > this.mEndFrame;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00028D98 File Offset: 0x00026F98
		public void Reset()
		{
			this.mUpdateCount = 0;
			for (int i = 0; i < this.mPosX.size<Component>(); i++)
			{
				this.mPosX[i].mValue = this.mPosX[i].mOriginalValue;
			}
			for (int j = 0; j < this.mPosY.size<Component>(); j++)
			{
				this.mPosY[j].mValue = this.mPosY[j].mOriginalValue;
			}
			for (int k = 0; k < this.mScaleX.size<Component>(); k++)
			{
				this.mScaleX[k].mValue = this.mScaleX[k].mOriginalValue;
			}
			for (int l = 0; l < this.mScaleY.size<Component>(); l++)
			{
				this.mScaleY[l].mValue = this.mScaleY[l].mOriginalValue;
			}
			for (int m = 0; m < this.mAngle.size<Component>(); m++)
			{
				this.mAngle[m].mValue = this.mAngle[m].mOriginalValue;
			}
			for (int n = 0; n < this.mOpacity.size<Component>(); n++)
			{
				this.mOpacity[n].mValue = this.mOpacity[n].mOriginalValue;
			}
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00028F08 File Offset: 0x00027108
		public int GetUpdateCount()
		{
			return this.mUpdateCount;
		}

		// Token: 0x0400075D RID: 1885
		protected int mUpdateCount;

		// Token: 0x0400075E RID: 1886
		protected List<Component> mPosX = new List<Component>();

		// Token: 0x0400075F RID: 1887
		protected List<Component> mPosY = new List<Component>();

		// Token: 0x04000760 RID: 1888
		protected List<Component> mScaleX = new List<Component>();

		// Token: 0x04000761 RID: 1889
		protected List<Component> mScaleY = new List<Component>();

		// Token: 0x04000762 RID: 1890
		protected List<Component> mAngle = new List<Component>();

		// Token: 0x04000763 RID: 1891
		protected List<Component> mOpacity = new List<Component>();

		// Token: 0x04000764 RID: 1892
		public Image mImage;

		// Token: 0x04000765 RID: 1893
		public float mOverallAlphaPct = 1f;

		// Token: 0x04000766 RID: 1894
		public float mOverallXScale = 1f;

		// Token: 0x04000767 RID: 1895
		public float mOverallYScale = 1f;

		// Token: 0x04000768 RID: 1896
		public int mCel;

		// Token: 0x04000769 RID: 1897
		public bool mMirror;

		// Token: 0x0400076A RID: 1898
		public bool mHoldLastFrame;

		// Token: 0x0400076B RID: 1899
		public int mStartFrame;

		// Token: 0x0400076C RID: 1900
		public int mEndFrame;
	}
}
