using System;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x020000B1 RID: 177
	public class OrbParticle
	{
		// Token: 0x06000A73 RID: 2675 RVA: 0x0006576B File Offset: 0x0006396B
		public OrbParticle()
		{
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00065780 File Offset: 0x00063980
		public OrbParticle(float angle, float radius, float alpha_fade, float size_fade)
		{
			this.mAngle = angle;
			this.mAlphaFade = alpha_fade;
			this.mSizeFade = size_fade;
			this.mAlpha = 255f;
			this.mRadius = radius;
			this.mSize = 1f;
			this.mRotation = 0f;
			this.mRed = 255f;
			this.mGreen = 255f;
			float num = 255f / this.mAlphaFade;
			this.mRedFade = 255f / num;
			this.mGreenFade = Common._M(54f) / num;
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00065820 File Offset: 0x00063A20
		public void Update()
		{
			this.mAlpha -= this.mAlphaFade;
			this.mSize -= this.mSizeFade;
			this.mRed -= this.mRedFade;
			this.mGreen -= this.mGreenFade;
			if (this.mRed < 0f)
			{
				this.mRed = 0f;
			}
			if (this.mGreen < 0f)
			{
				this.mGreen = 0f;
			}
			if (this.mAlpha < 0f)
			{
				this.mAlpha = 0f;
			}
			if (this.mSize < 0f)
			{
				this.mSize = 0f;
			}
			this.mRotation += Common._M(0.1f);
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x000658F0 File Offset: 0x00063AF0
		public void Draw(Graphics g, float x, float y)
		{
			g.SetColorizeImages(true);
			g.SetColor((int)this.mRed, (int)this.mGreen, 255, (int)this.mAlpha);
			this.mGlobalTranform.Reset();
			this.mGlobalTranform.RotateRad(this.mRotation);
			this.mGlobalTranform.Scale(this.mSize, this.mSize);
			Image imageByID = Res.GetImageByID(ResID.IMAGE_PART_FAT);
			g.DrawImageTransform(imageByID, this.mGlobalTranform, x + this.mRadius * (float)Math.Cos((double)this.mAngle), y - this.mRadius * (float)Math.Sin((double)this.mAngle));
			g.SetColorizeImages(false);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x000659A2 File Offset: 0x00063BA2
		public bool IsDone()
		{
			return this.mAlpha <= 0f;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x000659B4 File Offset: 0x00063BB4
		public void SyncState(DataSync sync)
		{
			sync.SyncFloat(ref this.mAlpha);
			sync.SyncFloat(ref this.mAngle);
			sync.SyncFloat(ref this.mRadius);
			sync.SyncFloat(ref this.mRotation);
			sync.SyncFloat(ref this.mSize);
			sync.SyncFloat(ref this.mAlphaFade);
			sync.SyncFloat(ref this.mSizeFade);
			sync.SyncFloat(ref this.mRedFade);
			sync.SyncFloat(ref this.mGreenFade);
			sync.SyncFloat(ref this.mRed);
			sync.SyncFloat(ref this.mGreen);
		}

		// Token: 0x040008DB RID: 2267
		protected float mAlpha;

		// Token: 0x040008DC RID: 2268
		protected float mAngle;

		// Token: 0x040008DD RID: 2269
		protected float mRadius;

		// Token: 0x040008DE RID: 2270
		protected float mRotation;

		// Token: 0x040008DF RID: 2271
		protected float mSize;

		// Token: 0x040008E0 RID: 2272
		protected float mAlphaFade;

		// Token: 0x040008E1 RID: 2273
		protected float mSizeFade;

		// Token: 0x040008E2 RID: 2274
		protected float mRedFade;

		// Token: 0x040008E3 RID: 2275
		protected float mGreenFade;

		// Token: 0x040008E4 RID: 2276
		protected float mRed;

		// Token: 0x040008E5 RID: 2277
		protected float mGreen;

		// Token: 0x040008E6 RID: 2278
		protected Transform mGlobalTranform = new Transform();
	}
}
