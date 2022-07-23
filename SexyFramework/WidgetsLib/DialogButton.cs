using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001B7 RID: 439
	public class DialogButton : ButtonWidget
	{
		// Token: 0x06001037 RID: 4151 RVA: 0x0004D194 File Offset: 0x0004B394
		public DialogButton(Image theComponentImage, int theId, ButtonListener theListener)
			: base(theId, theListener)
		{
			this.mComponentImage = theComponentImage;
			if (this.mComponentImage != null && this.mComponentImage.GetCelCount() == 3)
			{
				this.mNormalRect = this.mComponentImage.GetCelRect(0);
				this.mOverRect = this.mComponentImage.GetCelRect(1);
				this.mDownRect = this.mComponentImage.GetCelRect(2);
			}
			this.mTextOffsetX = (this.mTextOffsetY = 0);
			this.mTranslateX = (this.mTranslateY = 1);
			this.mDoFinger = true;
			this.SetColors3(GlobalMembers.gDialogButtonColors, 6);
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0004D23C File Offset: 0x0004B43C
		public override void Draw(Graphics g)
		{
			if (this.mBtnNoDraw)
			{
				return;
			}
			if (this.mComponentImage == null)
			{
				base.Draw(g);
				return;
			}
			bool flag = this.IsButtonDown();
			this.mBoundBox.mX = 0;
			this.mBoundBox.mY = 0;
			this.mBoundBox.mWidth = this.mWidth;
			this.mBoundBox.mHeight = this.mHeight;
			if (this.mNormalRect.mWidth == 0)
			{
				if (flag)
				{
					g.Translate(this.mTranslateX, this.mTranslateY);
				}
				g.DrawImageBox(this.mBoundBox, this.mComponentImage);
			}
			else
			{
				if (this.mDisabled && this.mDisabledRect.mWidth > 0 && this.mDisabledRect.mHeight > 0)
				{
					g.DrawImageBox(this.mDisabledRect, this.mBoundBox, this.mComponentImage);
				}
				else if (this.IsButtonDown())
				{
					g.DrawImageBox(this.mDownRect, this.mBoundBox, this.mComponentImage);
				}
				else if (this.mOverAlpha > 0.0)
				{
					if (this.mOverAlpha < 1.0)
					{
						g.DrawImageBox(this.mNormalRect, this.mBoundBox, this.mComponentImage);
					}
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)(this.mOverAlpha * 255.0));
					g.DrawImageBox(this.mOverRect, this.mBoundBox, this.mComponentImage);
					g.SetColorizeImages(false);
				}
				else if (this.mIsOver)
				{
					g.DrawImageBox(this.mOverRect, this.mBoundBox, this.mComponentImage);
				}
				else
				{
					g.DrawImageBox(this.mNormalRect, this.mBoundBox, this.mComponentImage);
				}
				if (flag)
				{
					g.Translate(this.mTranslateX, this.mTranslateY);
				}
			}
			if (this.mFont != null)
			{
				g.SetFont(this.mFont);
				if (this.mIsOver)
				{
					g.SetColor(this.mColors[1]);
				}
				else
				{
					g.SetColor(this.mColors[0]);
				}
				int num = (this.mWidth - this.mFont.StringWidth(this.mLabel)) / 2;
				int num2 = (this.mHeight + this.mFont.GetAscent() - this.mFont.GetAscentPadding() - this.mFont.GetAscent() / 6 - 1) / 2;
				if (this.mFont.StringWidth(this.mLabel) + 20 > this.mBoundBox.mWidth)
				{
					Rect theRect = this.mBoundBox;
					theRect.mWidth -= 20;
					theRect.mY = 15;
					theRect.mX = 25;
					g.WriteWordWrapped(theRect, this.mLabel, this.mFont.GetAscent() - 4);
				}
				else
				{
					g.DrawString(this.mLabel, num + this.mTextOffsetX, num2 + this.mTextOffsetY);
				}
			}
			if (this.mIconImage != null)
			{
				if (this.mIsOver)
				{
					g.SetColor(this.mColors[1]);
				}
				else
				{
					g.SetColor(this.mColors[0]);
				}
				int num3 = (this.mWidth - this.mIconImage.GetWidth()) / 2;
				int num4 = (this.mHeight - this.mIconImage.GetHeight()) / 2;
				g.DrawImage(this.mIconImage, num3 + this.mTextOffsetX, num4 + this.mTextOffsetY);
			}
			if (flag)
			{
				g.Translate(-this.mTranslateX, -this.mTranslateY);
			}
		}

		// Token: 0x04000CE7 RID: 3303
		public Image mComponentImage;

		// Token: 0x04000CE8 RID: 3304
		public int mTranslateX;

		// Token: 0x04000CE9 RID: 3305
		public int mTranslateY;

		// Token: 0x04000CEA RID: 3306
		public int mTextOffsetX;

		// Token: 0x04000CEB RID: 3307
		public int mTextOffsetY;

		// Token: 0x04000CEC RID: 3308
		private Rect mBoundBox = default(Rect);
	}
}
