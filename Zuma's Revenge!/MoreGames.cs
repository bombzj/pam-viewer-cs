using System;
using SexyFramework.WidgetsLib;

namespace ZumasRevenge
{
	// Token: 0x02000122 RID: 290
	public class MoreGames : Widget, ButtonListener
	{
		// Token: 0x06000EF8 RID: 3832 RVA: 0x0009B9E8 File Offset: 0x00099BE8
		public MoreGames(GameApp gameApp)
		{
			this.gameApp = gameApp;
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0009B9F7 File Offset: 0x00099BF7
		public void ButtonPress(int theId)
		{
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0009B9F9 File Offset: 0x00099BF9
		public void ButtonPress(int theId, int theClickCount)
		{
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0009B9FB File Offset: 0x00099BFB
		public void ButtonDepress(int theId)
		{
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0009B9FD File Offset: 0x00099BFD
		public void ButtonDownTick(int theId)
		{
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0009B9FF File Offset: 0x00099BFF
		public void ButtonMouseEnter(int theId)
		{
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0009BA01 File Offset: 0x00099C01
		public void ButtonMouseLeave(int theId)
		{
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0009BA03 File Offset: 0x00099C03
		public void ButtonMouseMove(int theId, int theX, int theY)
		{
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x0009BA05 File Offset: 0x00099C05
		internal bool IsReadyForDelete()
		{
			return true;
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0009BA08 File Offset: 0x00099C08
		internal void DoSlide(bool p)
		{
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0009BA0A File Offset: 0x00099C0A
		internal void Init()
		{
		}

		// Token: 0x04000EA5 RID: 3749
		private GameApp gameApp;
	}
}
