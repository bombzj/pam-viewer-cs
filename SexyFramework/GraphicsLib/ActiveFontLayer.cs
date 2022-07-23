using System;
using System.Collections.Generic;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000D2 RID: 210
	public class ActiveFontLayer
	{
		// Token: 0x06000658 RID: 1624 RVA: 0x00019381 File Offset: 0x00017581
		public ActiveFontLayer()
		{
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x000193AC File Offset: 0x000175AC
		public ActiveFontLayer(ActiveFontLayer theActiveFontLayer)
		{
			this.mBaseFontLayer = theActiveFontLayer.mBaseFontLayer;
			this.mUseAlphaCorrection = theActiveFontLayer.mUseAlphaCorrection;
			this.mScaledCharImageRects = theActiveFontLayer.mScaledCharImageRects;
			this.mColorStack = theActiveFontLayer.mColorStack;
			for (int i = 0; i < 8; i++)
			{
				this.mScaledImages[i] = theActiveFontLayer.mScaledImages[i];
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0001942D File Offset: 0x0001762D
		public virtual void Dispose()
		{
			this.mScaledCharImageRects.Clear();
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0001943C File Offset: 0x0001763C
		public SharedImageRef GenerateAlphaCorrectedImage(int thePalette)
		{
			bool flag = false;
			this.mScaledImages[thePalette] = GlobalMembers.gSexyAppBase.GetSharedImage("!" + this.mScaledImages[7].GetMemoryImage().mFilePath, string.Format("AltFontImage{0}", thePalette), ref flag, true, false);
			this.mScaledImages[thePalette].GetMemoryImage().Create(this.mScaledImages[7].mWidth, this.mScaledImages[7].mHeight);
			int num = this.mScaledImages[7].mWidth * this.mScaledImages[7].mHeight;
			this.mScaledImages[thePalette].GetMemoryImage().mColorTable = new uint[256];
			this.mScaledImages[thePalette].GetMemoryImage().mColorIndices = new byte[num];
			if (this.mScaledImages[7].GetMemoryImage().mColorTable != null)
			{
				Array.Copy(this.mScaledImages[thePalette].GetMemoryImage().mColorIndices, this.mScaledImages[7].GetMemoryImage().mColorIndices, num);
			}
			else
			{
				uint[] bits = this.mScaledImages[7].GetMemoryImage().GetBits();
				for (int i = 0; i < num; i++)
				{
					this.mScaledImages[thePalette].GetMemoryImage().mColorIndices[i] = (byte)(bits[i] >> 24);
				}
			}
			Array.Copy(this.mScaledImages[thePalette].GetMemoryImage().mColorTable, GlobalImageFont.FONT_PALETTES[thePalette], 1024);
			return this.mScaledImages[thePalette];
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x000195B4 File Offset: 0x000177B4
		public void PushColor(SexyColor theColor)
		{
			if (this.mColorStack.Count == 0)
			{
				this.mColorStack.Add(theColor);
				return;
			}
			SexyColor color = this.mColorStack[this.mColorStack.Count - 1];
			SexyColor color2 = new SexyColor(theColor.mRed * color.mRed / 255, theColor.mGreen * color.mGreen / 255, theColor.mBlue * color.mBlue / 255, theColor.mAlpha * color.mAlpha / 255);
			this.mColorStack.Add(color2);
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0001965B File Offset: 0x0001785B
		public void PopColor()
		{
			if (this.mColorStack.Count != 0)
			{
				this.mColorStack.RemoveAt(this.mColorStack.Count - 1);
			}
		}

		// Token: 0x04000540 RID: 1344
		public FontLayer mBaseFontLayer;

		// Token: 0x04000541 RID: 1345
		public SharedImageRef[] mScaledImages = new SharedImageRef[8];

		// Token: 0x04000542 RID: 1346
		public bool mUseAlphaCorrection;

		// Token: 0x04000543 RID: 1347
		public bool mOwnsImage;

		// Token: 0x04000544 RID: 1348
		public Dictionary<char, Rect> mScaledCharImageRects = new Dictionary<char, Rect>();

		// Token: 0x04000545 RID: 1349
		public List<SexyColor> mColorStack = new List<SexyColor>();
	}
}
