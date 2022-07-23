using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x0200004F RID: 79
	public class CheatWidget : Widget
	{
		// Token: 0x060006A9 RID: 1705 RVA: 0x0002D9D4 File Offset: 0x0002BBD4
		public CheatWidget(Widget theTarget, string theCheats, Font theFont)
		{
			float num = ((GameApp.mGameRes == 768) ? 1f : ((GameApp.mGameRes == 640) ? 2f : 1.5f));
			this.mButtonSize = (int)((float)CheatWidget.BUTTON_SIZE * num);
			this.mClient = theTarget;
			this.mCheatChars = theCheats;
			int length = theCheats.Length;
			this.mButtonsPerRow = (GameApp.gApp.GetScreenRect().mWidth + GameApp.gApp.GetScreenRect().mX - GameApp.gApp.mBoardOffsetX * 2) / this.mButtonSize;
			this.mCols = this.mButtonsPerRow;
			this.mRows = (length + this.mButtonsPerRow - 1) / this.mButtonsPerRow;
			this.mWidth = this.mCols * this.mButtonSize + 1;
			this.mHeight = this.mRows * this.mButtonSize + 1;
			this.mAlignment = true;
			this.mEnable = true;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0002DACC File Offset: 0x0002BCCC
		public override void Draw(Graphics g)
		{
			if (!this.mEnable)
			{
				return;
			}
			g.SetColor(new SexyColor(255, 200));
			g.FillRect(0, 0, this.mWidth, this.mHeight);
			int num = 0;
			for (int i = 0; i < this.mRows; i++)
			{
				for (int j = 0; j < this.mCols; j++)
				{
					Rect theRect = new Rect(j * this.mButtonSize + 1, i * this.mButtonSize + 1, this.mButtonSize - 2, this.mButtonSize - 2);
					g.SetColor(20, 20, 20);
					g.FillRect(theRect);
				}
				int num2 = 0;
				while (num < this.mCheatChars.Length && num2 < this.mCols)
				{
					((GameMain)GameApp.gApp.mGameMain).DrawSysString(string.Concat(this.mCheatChars[num]), (float)(num2 * this.mButtonSize + this.mButtonSize / 2 - 10) * 800f / 1066f, (float)(i * this.mButtonSize + this.mY + this.mButtonSize / 2 - 10) * 800f / 1066f);
					num2++;
					num++;
				}
			}
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0002DC14 File Offset: 0x0002BE14
		public override void MouseDown(int x, int y, int theClickCount)
		{
			if (!this.mEnable)
			{
				return;
			}
			int num = y / this.mButtonSize;
			int num2 = x / this.mButtonSize;
			int num3 = num * this.mButtonsPerRow + num2;
			if (num3 < this.mCheatChars.Length)
			{
				if (this.mCheatChars[num3] == 'X')
				{
					GameApp.gApp.mStepMode = 0;
					GameApp.gApp.ClearUpdateBacklog(false);
					this.mEnable = false;
					this.SetVisible(false);
					return;
				}
				if (this.mCheatChars[num3] == 'j')
				{
					this.SwapAlignment();
					return;
				}
				this.mClient.KeyChar(this.mCheatChars[num3]);
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0002DCB8 File Offset: 0x0002BEB8
		public override void MouseUp(int x, int y, int theClickCount)
		{
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0002DCBC File Offset: 0x0002BEBC
		public void SwapAlignment()
		{
			if (this.mAlignment)
			{
				this.Move(this.mX, GameApp.gApp.GetScreenRect().mHeight - this.mHeight);
				this.mAlignment = false;
				return;
			}
			this.Move(this.mX, 0);
			this.mAlignment = true;
		}

		// Token: 0x040003E2 RID: 994
		public string mCheatChars;

		// Token: 0x040003E3 RID: 995
		public Widget mClient;

		// Token: 0x040003E4 RID: 996
		public int mRows;

		// Token: 0x040003E5 RID: 997
		public int mCols;

		// Token: 0x040003E6 RID: 998
		public int mButtonsPerRow;

		// Token: 0x040003E7 RID: 999
		public int mButtonSize;

		// Token: 0x040003E8 RID: 1000
		public bool mAlignment;

		// Token: 0x040003E9 RID: 1001
		public bool mEnable;

		// Token: 0x040003EA RID: 1002
		private static int BUTTON_SIZE = Common._DS(80);
	}
}
