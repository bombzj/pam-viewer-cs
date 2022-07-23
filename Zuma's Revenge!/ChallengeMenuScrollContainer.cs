using System;
using System.Collections.Generic;
using SexyFramework.GraphicsLib;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x0200004A RID: 74
	public class ChallengeMenuScrollContainer : Widget
	{
		// Token: 0x0600067C RID: 1660 RVA: 0x0002BF58 File Offset: 0x0002A158
		public ChallengeMenuScrollContainer(ChallengeMenu aChallengeMenu)
		{
			this.mChallengeMenu = aChallengeMenu;
			this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES);
			this.IMAGE_UI_CHALLENGESCREEN_DIVIDER = Res.GetImageByID(ResID.IMAGE_UI_CHALLENGESCREEN_DIVIDER);
			this.mTableOfContents = new TableOfContents(this.mChallengeMenu);
			this.mTableOfContents.Init();
			this.mTableOfContents.Resize(0, 0, this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth(), this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetHeight());
			this.AddWidget(this.mTableOfContents);
			int num = 4095;
			int num2 = this.mTableOfContents.mWidth;
			int num3 = 0;
			for (int i = 0; i < GlobalChallenge.NUM_CHALLENGE_ZONES; i++)
			{
				ZoneFrame zoneFrame = new ZoneFrame(this.mChallengeMenu, num3++, num);
				this.mZoneFrames.Add(zoneFrame);
				num += num;
				this.mZoneFrames[i].Move(num2, 0);
				num2 += this.mZoneFrames[i].mWidth;
				this.AddWidget(this.mZoneFrames[i]);
			}
			this.Resize(0, 0, this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth() * (this.mZoneFrames.Count + 1), this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetHeight());
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0002C098 File Offset: 0x0002A298
		public override void Dispose()
		{
			for (int i = 0; i < this.mZoneFrames.Count; i++)
			{
				this.RemoveWidget(this.mZoneFrames[i]);
				if (this.mZoneFrames[i] != null)
				{
					this.mZoneFrames[i].Dispose();
				}
				this.mZoneFrames[i] = null;
			}
			this.mZoneFrames.Clear();
			this.RemoveWidget(this.mTableOfContents);
			if (this.mTableOfContents != null)
			{
				this.mTableOfContents.Dispose();
			}
			this.mTableOfContents = null;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0002C12C File Offset: 0x0002A32C
		public void RehupChallengeButtons()
		{
			if (this.mZoneFrames.Count != 0)
			{
				for (int i = 0; i < this.mZoneFrames.Count; i++)
				{
					if (this.mZoneFrames[i] != null)
					{
						this.mZoneFrames[i].RehupChallengeButtons();
					}
				}
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0002C17C File Offset: 0x0002A37C
		public override void Draw(Graphics g)
		{
			int num = this.NumPages() - 1;
			int num2 = this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth() + GameApp.gApp.GetScreenRect().mX / 2;
			int theY = (this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetHeight() - this.IMAGE_UI_CHALLENGESCREEN_DIVIDER.GetHeight()) / 2;
			for (int i = 0; i < num; i++)
			{
				g.DrawImage(this.IMAGE_UI_CHALLENGESCREEN_DIVIDER, num2 - this.IMAGE_UI_CHALLENGESCREEN_DIVIDER.GetWidth() / 2, theY);
				num2 += this.IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES.GetWidth();
			}
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0002C200 File Offset: 0x0002A400
		public int NumPages()
		{
			return this.mZoneFrames.Count + 1;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0002C20F File Offset: 0x0002A40F
		public void AwardMedal(int theZone, bool isAce)
		{
			this.mTableOfContents.AwardMedal(theZone, isAce);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0002C220 File Offset: 0x0002A420
		public void PreloadButtonImage(int theZone)
		{
			ZoneFrame zoneFrame = this.mZoneFrames[theZone];
			if (zoneFrame != null)
			{
				zoneFrame.PreLoadButtonsImage();
			}
		}

		// Token: 0x0400039E RID: 926
		private ChallengeMenu mChallengeMenu;

		// Token: 0x0400039F RID: 927
		private List<ZoneFrame> mZoneFrames = new List<ZoneFrame>();

		// Token: 0x040003A0 RID: 928
		private Image IMAGE_UI_CHALLENGESCREEN_TAPESTRY_LEAVES;

		// Token: 0x040003A1 RID: 929
		private Image IMAGE_UI_CHALLENGESCREEN_DIVIDER;

		// Token: 0x040003A2 RID: 930
		private TableOfContents mTableOfContents;
	}
}
