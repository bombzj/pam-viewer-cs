using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x0200005F RID: 95
	public class BlendedImage : IDisposable
	{
		// Token: 0x06000726 RID: 1830 RVA: 0x00030742 File Offset: 0x0002E942
		public void Dispose()
		{
			this.DeleteImages();
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0003074C File Offset: 0x0002E94C
		public void DeleteImages()
		{
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					this.mImages[i, j].Dispose();
					this.mImages[i, j] = null;
				}
			}
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00030794 File Offset: 0x0002E994
		public BlendedImage(MemoryImage theImage, Rect theSrcRect, bool rotated)
		{
			int theWidth = theSrcRect.mWidth + 3;
			int theHeight = theSrcRect.mHeight + 3;
			MemoryImage memoryImage = new MemoryImage();
			memoryImage.Create(theWidth, theHeight);
			Graphics graphics = new Graphics(memoryImage);
			graphics.DrawImage(theImage, 1, 1, theSrcRect);
			graphics.ClearRenderContext();
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					MemoryImage memoryImage2 = new MemoryImage();
					memoryImage2.Create(theWidth, theHeight);
					Graphics graphics2 = new Graphics(memoryImage2);
					if (!rotated)
					{
						graphics2.DrawImageF(memoryImage, (float)i / 4f * 0.9f + 0.1f, (float)j / 4f * 0.9f + 0.1f);
					}
					else
					{
						graphics2.DrawImageRotatedF(memoryImage, (float)i / 4f * 0.9f + 0.1f, (float)j / 4f * 0.9f + 0.1f, -1.5707000494003296);
					}
					this.mImages[i, j] = memoryImage2;
					graphics2.ClearRenderContext();
				}
			}
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x000308BC File Offset: 0x0002EABC
		public void Draw(Graphics g, float x, float y)
		{
			int num = (int)(((double)x - Math.Floor((double)x)) * 4.0);
			int num2 = (int)(((double)y - Math.Floor((double)y)) * 4.0);
			g.DrawImage(this.mImages[num, num2], (int)x, (int)y);
		}

		// Token: 0x0400046F RID: 1135
		protected const int NUM_BLENDS = 4;

		// Token: 0x04000470 RID: 1136
		protected Image[,] mImages = new Image[4, 4];
	}
}
