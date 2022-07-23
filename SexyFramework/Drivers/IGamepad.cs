using System;

namespace SexyFramework.Drivers
{
	// Token: 0x0200001C RID: 28
	public abstract class IGamepad
	{
		// Token: 0x06000167 RID: 359 RVA: 0x00005588 File Offset: 0x00003788
		public virtual void Dispose()
		{
		}

		// Token: 0x06000168 RID: 360
		public abstract bool IsConnected();

		// Token: 0x06000169 RID: 361
		public abstract int GetGamepadIndex();

		// Token: 0x0600016A RID: 362
		public abstract bool IsButtonDown(GamepadButton button);

		// Token: 0x0600016B RID: 363
		public abstract float GetButtonPressure(GamepadButton button);

		// Token: 0x0600016C RID: 364
		public abstract float GetAxisXPosition();

		// Token: 0x0600016D RID: 365
		public abstract float GetAxisYPosition();

		// Token: 0x0600016E RID: 366
		public abstract float GetRightAxisXPosition();

		// Token: 0x0600016F RID: 367
		public abstract float GetRightAxisYPosition();

		// Token: 0x06000170 RID: 368
		public abstract void Update();

		// Token: 0x06000171 RID: 369
		public abstract void AddRumbleEffect(float theLeft, float theRight, float theFadeTime);
	}
}
