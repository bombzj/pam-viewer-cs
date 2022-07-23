using System;
using Microsoft.Xna.Framework;
using SexyFramework.GraphicsLib;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001B6 RID: 438
	public class CursorWidget : Widget
	{
		// Token: 0x06001033 RID: 4147 RVA: 0x0004D0F9 File Offset: 0x0004B2F9
		public CursorWidget()
		{
			this.mImage = null;
			this.mMouseVisible = false;
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0004D10F File Offset: 0x0004B30F
		public override void Draw(Graphics g)
		{
			if (this.mImage != null)
			{
				g.DrawImage(this.mImage, 0, 0);
			}
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0004D127 File Offset: 0x0004B327
		public void SetImage(Image theImage)
		{
			this.mImage = theImage;
			if (this.mImage != null)
			{
				this.Resize(this.mX, this.mY, theImage.mWidth, theImage.mHeight);
			}
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0004D156 File Offset: 0x0004B356
		public Vector2 GetHotspot()
		{
			if (this.mImage == null)
			{
				return new Vector2(0f, 0f);
			}
			return new Vector2((float)(this.mImage.GetWidth() / 2), (float)(this.mImage.GetHeight() / 2));
		}

		// Token: 0x04000CE6 RID: 3302
		public Image mImage;
	}
}
