using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.AELib
{
	// Token: 0x0200000A RID: 10
	public class Layer
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002A8D File Offset: 0x00000C8D
		public Layer()
		{
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002ACC File Offset: 0x00000CCC
		public Layer(Layer other)
		{
			this.CopyFrom(other);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002B20 File Offset: 0x00000D20
		public void CopyFrom(Layer rhs)
		{
			this.mAdditive = rhs.mAdditive;
			this.mLayerName = rhs.mLayerName;
			this.mWidth = rhs.mWidth;
			this.mHeight = rhs.mHeight;
			this.mImage = rhs.mImage;
			this.mXOff = rhs.mXOff;
			this.mYOff = rhs.mYOff;
			this.mAnchorPoint = new Timeline(rhs.mAnchorPoint);
			this.mPosition = new Timeline(rhs.mPosition);
			this.mScale = new Timeline(rhs.mScale);
			this.mRotation = new Timeline(rhs.mRotation);
			this.mOpacity = new Timeline(rhs.mOpacity);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002BD6 File Offset: 0x00000DD6
		public virtual void Reset()
		{
			this.mAnchorPoint.Reset();
			this.mPosition.Reset();
			this.mScale.Reset();
			this.mRotation.Reset();
			this.mOpacity.Reset();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002C0F File Offset: 0x00000E0F
		public void AddAnchorPoint(int frame, float x, float y)
		{
			this.mAnchorPoint.AddKeyframe(frame, x, y);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002C1F File Offset: 0x00000E1F
		public void AddPosition(int frame, float x, float y)
		{
			this.mPosition.AddKeyframe(frame, x, y);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002C2F File Offset: 0x00000E2F
		public void AddScale(int frame, float sx, float sy)
		{
			this.mScale.AddKeyframe(frame, sx, sy);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002C3F File Offset: 0x00000E3F
		public void AddRotation(int frame, float angle_radians)
		{
			this.mRotation.AddKeyframe(frame, angle_radians);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002C4E File Offset: 0x00000E4E
		public void AddOpacity(int frame, float pct)
		{
			this.mOpacity.AddKeyframe(frame, pct);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002C60 File Offset: 0x00000E60
		public void EnsureTimelineDefaults(long comp_width, long comp_height)
		{
			if (!this.mOpacity.HasInitialValue())
			{
				this.mOpacity.AddKeyframe(0, 1f);
			}
			if (!this.mRotation.HasInitialValue())
			{
				this.mRotation.AddKeyframe(0, 0f);
			}
			if (!this.mScale.HasInitialValue())
			{
				this.mScale.AddKeyframe(0, 1f, 1f);
			}
			if (!this.mPosition.HasInitialValue())
			{
				this.mPosition.AddKeyframe(0, (float)comp_width / 2f, (float)comp_height / 2f);
			}
			if (this.mImage != null && this.mImage.GetImage() != null && !this.mAnchorPoint.HasInitialValue())
			{
				this.mAnchorPoint.AddKeyframe(0, (float)this.mImage.GetImage().GetCelWidth() / 2f, (float)this.mImage.GetImage().GetCelHeight() / 2f);
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002D51 File Offset: 0x00000F51
		public virtual Layer Duplicate()
		{
			return new Layer(this);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002D59 File Offset: 0x00000F59
		public virtual Image GetImage()
		{
			return this.mImage.GetImage();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002D66 File Offset: 0x00000F66
		public virtual bool IsLayerBase()
		{
			return true;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002D69 File Offset: 0x00000F69
		public virtual bool NeedsTranslatedFrame()
		{
			return false;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002D6C File Offset: 0x00000F6C
		public void SetImage(SharedImageRef img)
		{
			this.mImage = img;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002D75 File Offset: 0x00000F75
		public virtual void Draw(Graphics g)
		{
			this.Draw(g, null);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002D7F File Offset: 0x00000F7F
		public virtual void Draw(Graphics g, CumulativeTransform ctrans)
		{
			this.Draw(g, ctrans, -1);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002D8A File Offset: 0x00000F8A
		public virtual void Draw(Graphics g, CumulativeTransform ctrans, int frame)
		{
			this.Draw(g, ctrans, frame, 1f);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002D9A File Offset: 0x00000F9A
		public virtual bool isValid()
		{
			return this.mImage.mSharedImage != null || this.mImage.mUnsharedImage != null;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002DBC File Offset: 0x00000FBC
		public virtual void Draw(Graphics g, CumulativeTransform ctrans, int frame, float scale)
		{
			float num = 0f;
			this.mOpacity.GetValue(frame, ref num);
			float num2 = 255f * num;
			if (ctrans != null)
			{
				num2 *= ctrans.mOpacity;
			}
			if (num2 <= 0f)
			{
				return;
			}
			if (num2 != 255f)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, (int)num2);
			}
			float num3 = 0f;
			float num4 = 0f;
			this.mAnchorPoint.GetValue(frame, ref num3, ref num4);
			num3 *= scale;
			num4 *= scale;
			int num5 = this.mImage.mWidth / 2;
			int num6 = this.mImage.mHeight / 2;
			num3 -= (float)num5;
			num4 -= (float)num6;
			float sx = 0f;
			float sy = 0f;
			this.mScale.GetValue(frame, ref sx, ref sy);
			float num7 = 0f;
			this.mRotation.GetValue(frame, ref num7);
			float num8 = 0f;
			float num9 = 0f;
			this.mPosition.GetValue(frame, ref num8, ref num9);
			num8 *= scale;
			num9 *= scale;
			SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
			sexyTransform2D.Translate(-num3 + (float)this.mXOff, -num4 + (float)this.mYOff);
			sexyTransform2D.Scale(sx, sy);
			if (num7 != 0f)
			{
				sexyTransform2D.RotateRad(-num7);
			}
			sexyTransform2D.Translate(num8, num9);
			if (this.mAdditive || ctrans.mForceAdditive)
			{
				g.SetDrawMode(1);
			}
			sexyTransform2D = ctrans.mTrans * sexyTransform2D;
			g.DrawImageMatrix(this.mImage.GetImage(), sexyTransform2D);
			g.SetDrawMode(0);
			g.SetColorizeImages(false);
		}

		// Token: 0x04000019 RID: 25
		protected SharedImageRef mImage;

		// Token: 0x0400001A RID: 26
		public Timeline mAnchorPoint = new Timeline();

		// Token: 0x0400001B RID: 27
		public Timeline mPosition = new Timeline();

		// Token: 0x0400001C RID: 28
		public Timeline mScale = new Timeline();

		// Token: 0x0400001D RID: 29
		public Timeline mRotation = new Timeline();

		// Token: 0x0400001E RID: 30
		public Timeline mOpacity = new Timeline();

		// Token: 0x0400001F RID: 31
		public string mLayerName;

		// Token: 0x04000020 RID: 32
		public int mWidth;

		// Token: 0x04000021 RID: 33
		public int mHeight;

		// Token: 0x04000022 RID: 34
		public int mXOff;

		// Token: 0x04000023 RID: 35
		public int mYOff;

		// Token: 0x04000024 RID: 36
		public bool mAdditive;
	}
}
