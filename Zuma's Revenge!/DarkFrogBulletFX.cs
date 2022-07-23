using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x02000013 RID: 19
	public class DarkFrogBulletFX
	{
		// Token: 0x0600042D RID: 1069 RVA: 0x0000DB08 File Offset: 0x0000BD08
		public DarkFrogBulletFX()
		{
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000DB2D File Offset: 0x0000BD2D
		public DarkFrogBulletFX(int id)
		{
			this.mBulletId = id;
		}

		// Token: 0x040000DF RID: 223
		public PIEffect mBallEffect;

		// Token: 0x040000E0 RID: 224
		public PIEffect mBallExplosion;

		// Token: 0x040000E1 RID: 225
		public float mTwirlAngle;

		// Token: 0x040000E2 RID: 226
		public float mX = -1000f;

		// Token: 0x040000E3 RID: 227
		public float mY = -1000f;

		// Token: 0x040000E4 RID: 228
		public bool mExploding;

		// Token: 0x040000E5 RID: 229
		public int mBulletId = -1;
	}
}
