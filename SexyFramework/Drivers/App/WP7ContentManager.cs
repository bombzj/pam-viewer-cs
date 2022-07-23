using System;
using Microsoft.Xna.Framework.Content;

namespace SexyFramework.Drivers.App
{
	// Token: 0x02000011 RID: 17
	public class WP7ContentManager : ContentManager
	{
		// Token: 0x06000072 RID: 114 RVA: 0x00003E14 File Offset: 0x00002014
		public WP7ContentManager(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.mCustom = new Action<IDisposable>(this.CustomDispose<IDisposable>);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003E2F File Offset: 0x0000202F
		public T LoadResDirectly<T>(string name)
		{
			return base.ReadAsset<T>(name, this.mCustom);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003E3E File Offset: 0x0000203E
		public void CustomDispose<IDisposable>(IDisposable obj)
		{
		}

		// Token: 0x0400003D RID: 61
		private Action<IDisposable> mCustom;
	}
}
