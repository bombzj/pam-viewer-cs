using System;
using SexyFramework.GraphicsLib;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001B8 RID: 440
	public class PageControl : Widget
	{
		// Token: 0x06001039 RID: 4153 RVA: 0x0004D5C4 File Offset: 0x0004B7C4
		public PageControl(Image partsImage)
		{
			this.mPartsImage = partsImage;
			this.mNumberOfPages = 0;
			this.mCurrentPage = 0;
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0004D5E4 File Offset: 0x0004B7E4
		public void SetNumberOfPages(int count)
		{
			if (count != this.mNumberOfPages)
			{
				this.mNumberOfPages = count;
				int theWidth = this.mPartsImage.GetCelWidth() * count;
				int celHeight = this.mPartsImage.GetCelHeight();
				this.Resize(this.mX, this.mY, theWidth, celHeight);
			}
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0004D62F File Offset: 0x0004B82F
		public void SetCurrentPage(int page)
		{
			this.mCurrentPage = page;
			this.MarkDirtyFull();
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0004D63E File Offset: 0x0004B83E
		public int GetCurrentPage()
		{
			return this.mCurrentPage;
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x0004D648 File Offset: 0x0004B848
		public override void Draw(Graphics g)
		{
			int celWidth = this.mPartsImage.GetCelWidth();
			int celHeight = this.mPartsImage.GetCelHeight();
			int num = celWidth * this.mNumberOfPages;
			int num2 = (this.mWidth - num) / 2;
			int theY = (this.mHeight - celHeight) / 2;
			for (int i = 0; i < this.mNumberOfPages; i++)
			{
				int theCel = ((this.mCurrentPage == i) ? 0 : 1);
				g.DrawImageCel(this.mPartsImage, num2, theY, theCel);
				num2 += celWidth;
			}
		}

		// Token: 0x04000CED RID: 3309
		protected Image mPartsImage;

		// Token: 0x04000CEE RID: 3310
		protected int mNumberOfPages;

		// Token: 0x04000CEF RID: 3311
		protected int mCurrentPage;
	}
}
