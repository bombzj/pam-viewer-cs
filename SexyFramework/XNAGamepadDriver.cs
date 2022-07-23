using System;
using SexyFramework.Drivers;

namespace SexyFramework
{
	// Token: 0x0200001F RID: 31
	internal class XNAGamepadDriver : IGamepadDriver
	{
		// Token: 0x06000184 RID: 388 RVA: 0x00005644 File Offset: 0x00003844
		public static IGamepadDriver CreateGamepadDriver()
		{
			XNAGamepadDriver.mXNAGamepad = new XNAGamepad();
			return new XNAGamepadDriver();
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00005655 File Offset: 0x00003855
		public override void Dispose()
		{
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00005657 File Offset: 0x00003857
		public override int InitGamepadDriver(SexyAppBase app)
		{
			this.gApp = app;
			return 1;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00005661 File Offset: 0x00003861
		public override IGamepad GetGamepad(int theIndex)
		{
			return XNAGamepadDriver.mXNAGamepad;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00005668 File Offset: 0x00003868
		public override void Update()
		{
			XNAGamepadDriver.mXNAGamepad.Update();
		}

		// Token: 0x04000057 RID: 87
		private static XNAGamepad mXNAGamepad;

		// Token: 0x04000058 RID: 88
		private SexyAppBase gApp;
	}
}
