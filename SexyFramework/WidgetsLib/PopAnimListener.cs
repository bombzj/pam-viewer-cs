using System;
using SexyFramework.GraphicsLib;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001C8 RID: 456
	public interface PopAnimListener
	{
		// Token: 0x0600109B RID: 4251
		void PopAnimPlaySample(string theSampleName, int thePan, double theVolume, double theNumSteps);

		// Token: 0x0600109C RID: 4252
		PIEffect PopAnimLoadParticleEffect(string theEffectName);

		// Token: 0x0600109D RID: 4253
		bool PopAnimObjectPredraw(int theId, Graphics g, PASpriteInst theSpriteInst, PAObjectInst theObjectInst, PATransform theTransform, SexyColor theColor);

		// Token: 0x0600109E RID: 4254
		bool PopAnimObjectPostdraw(int theId, Graphics g, PASpriteInst theSpriteInst, PAObjectInst theObjectInst, PATransform theTransform, SexyColor theColor);

		// Token: 0x0600109F RID: 4255
		ImagePredrawResult PopAnimImagePredraw(int theId, PASpriteInst theSpriteInst, PAObjectInst theObjectInst, PATransform theTransform, Image theImage, Graphics g, int theDrawCount);

		// Token: 0x060010A0 RID: 4256
		void PopAnimStopped(int theId);

		// Token: 0x060010A1 RID: 4257
		void PopAnimCommand(int theId, string theCommand, string theParam);

		// Token: 0x060010A2 RID: 4258
		bool PopAnimCommand(int theId, PASpriteInst theSpriteInst, string theCommand, string theParam);
	}
}
