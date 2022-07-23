using System;
using Microsoft.Xna.Framework.Input;
using SexyFramework.Drivers;

namespace SexyFramework
{
	// Token: 0x0200001D RID: 29
	internal class XNAGamepad : IGamepad
	{
		// Token: 0x06000173 RID: 371 RVA: 0x00005592 File Offset: 0x00003792
		public override bool IsConnected()
		{
			return false;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00005595 File Offset: 0x00003795
		public override int GetGamepadIndex()
		{
			return 0;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00005598 File Offset: 0x00003798
		public override bool IsButtonDown(GamepadButton button)
		{
			return XNAGamepad.mGamepadData.mButton[(int)button];
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000055A6 File Offset: 0x000037A6
		public override float GetButtonPressure(GamepadButton button)
		{
			return 0f;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000055AD File Offset: 0x000037AD
		public override float GetAxisXPosition()
		{
			return 0f;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000055B4 File Offset: 0x000037B4
		public override float GetAxisYPosition()
		{
			return 0f;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000055BB File Offset: 0x000037BB
		public override float GetRightAxisXPosition()
		{
			return 0f;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000055C2 File Offset: 0x000037C2
		public override float GetRightAxisYPosition()
		{
			return 0f;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000055CC File Offset: 0x000037CC
		public override void Update()
		{
			if (GamePad.GetState(0).Buttons.Back == ButtonState.Pressed)
			{
				XNAGamepad.mGamepadData.mButton[4] = true;
			}
			if (GamePad.GetState(0).Buttons.Back == ButtonState.Released)
			{
				XNAGamepad.mGamepadData.mButton[4] = false;
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00005624 File Offset: 0x00003824
		public override void AddRumbleEffect(float theLeft, float theRight, float theFadeTime)
		{
		}

		// Token: 0x04000056 RID: 86
		public static GamepadData mGamepadData = new GamepadData();
	}
}
