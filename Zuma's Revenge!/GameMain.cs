using System;
//using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using SexyFramework;
using SexyFramework.Drivers.App;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x020000EA RID: 234
	public class GameMain : Game
	{
		// Token: 0x06000CC1 RID: 3265 RVA: 0x0007B6A4 File Offset: 0x000798A4
		public GameMain()
		{
			base.Content = new WP7ContentManager(base.Services);
			base.Content.RootDirectory = "Content";
			base.TargetElapsedTime = TimeSpan.FromTicks(166666L);
			base.IsFixedTimeStep = true;
			this.SexyZuma = new GameApp(this, false);
			GlobalMembers.gSexyApp = this.SexyZuma;
			GlobalMembers.gSexyAppBase = this.SexyZuma;
			//this.gApplicationService = PhoneApplicationService.Current;
			//this.gApplicationService.Deactivated += new EventHandler<DeactivatedEventArgs>(this.OnServiceDeactivated);
			//this.gApplicationService.Activated += new EventHandler<ActivatedEventArgs>(this.OnServiceActivated);
			base.Components.Add(new GamerServicesComponent(this));
			Guide.SimulateTrialMode = false;
			TouchPanel.EnableMouseTouchPoint = true;
			IsMouseVisible = true;
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0007B7E8 File Offset: 0x000799E8
		protected override void Initialize()
		{
			base.Initialize();
			this.spriteBatch = new SpriteBatch(base.GraphicsDevice);
			this.mSpriteFont = base.Content.Load<SpriteFont>("Arial_20");
			base.Window.OrientationChanged += new EventHandler<EventArgs>(this.OrientationChanged);
			this.SexyZuma.InitText();
			SexyZuma.mAppDriver.Init();
			if (Localization.GetCurrentLanguage() != Localization.LanguageType.Language_FR)
			{
				this.splash = base.Content.Load<Texture2D>("Default-Landscape");
				return;
			}
			this.splash = base.Content.Load<Texture2D>("LoadingImage_DarkFrog_French");
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0007B879 File Offset: 0x00079A79
		protected override void LoadContent()
		{
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0007B87B File Offset: 0x00079A7B
		protected override void UnloadContent()
		{
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0007B880 File Offset: 0x00079A80
		protected override void Update(GameTime gameTime)
		{
			if (GameApp.mExit)
			{
				base.Exit();
			}
			bool isRunningSlowly = gameTime.IsRunningSlowly;
			try
			{
				if (!Guide.IsVisible)
				{
					base.Update(gameTime);
				}
			}
			catch (GameUpdateRequiredException ex)
			{
				if (GameApp.USE_XBOX_SERVICE)
				{
					this.SexyZuma.HandleGameUpdateRequired(ex);
				}
			}
			this.UpdateInput(gameTime);
			try
			{
				if (Guide.IsVisible)
				{
					return;
				}
			}
			catch (Exception)
			{
			}
			if (!this.isLoading)
			{
				this.SexyZuma.Update(gameTime.ElapsedGameTime.Seconds);
				return;
			}
			this.mElipseTime += gameTime.ElapsedGameTime.TotalSeconds;
			if (!this.mInitBegin)
			{
				GC.Collect();
				this.SexyZuma.StartThreadInit();
				this.mInitBegin = true;
				return;
			}
			if (this.SexyZuma.mInitFinished && this.mElipseTime >= 4.0)
			{
				this.SexyZuma.ShowLoadingScreen();
				this.isLoading = false;
			}
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x0007B98C File Offset: 0x00079B8C
		protected override void Draw(GameTime gameTime)
		{
			if (this.isLoading)
			{
				base.GraphicsDevice.Clear(Color.Black);
				this.spriteBatch.Begin();
				this.mColor = new Color((int)((byte)MathHelper.Clamp(this.mAlpha, 0f, 255f)), (int)((byte)MathHelper.Clamp(this.mAlpha, 0f, 255f)), (int)((byte)MathHelper.Clamp(this.mAlpha, 0f, 255f)), (int)((byte)MathHelper.Clamp(this.mAlpha, 0f, 255f)));
				this.spriteBatch.Draw(this.splash, new Rectangle(0, 0, 800, 480), this.mColor);
				this.spriteBatch.End();
			}
			else
			{
				if (this.splash != null)
				{
					this.splash.Dispose();
					this.splash = null;
				}
				this.SexyZuma.Draw(0);
			}
			base.Draw(gameTime);
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0007BA84 File Offset: 0x00079C84
		protected override void OnExiting(object sender, EventArgs args)
		{
			this.SexyZuma.OnExiting();
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0007BA91 File Offset: 0x00079C91
		protected override void OnActivated(object sender, EventArgs args)
		{
			this.SexyZuma.OnActivated();
			base.OnActivated(sender, args);
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0007BAA8 File Offset: 0x00079CA8
		protected override void OnDeactivated(object sender, EventArgs args)
		{
			if (!this.SexyZuma.mInitFinished)
			{
				this.mElipseTime -= 2.0;
			}
			this.SexyZuma.OnExiting();
			this.SexyZuma.OnDeactivated();
			base.OnDeactivated(sender, args);
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x0007BAF6 File Offset: 0x00079CF6
		protected void OnServiceActivated(object sender, EventArgs args)
		{
			this.SexyZuma.OnServiceActivated();
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x0007BB03 File Offset: 0x00079D03
		protected void OnServiceDeactivated(object sender, EventArgs args)
		{
			this.SexyZuma.OnServiceDeactivated();
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0007BB10 File Offset: 0x00079D10
		private void UpdateInput(GameTime gameTime)
		{
			if (GamePad.GetState(0).Buttons.Back == ButtonState.Pressed)
			{
				if (this.isLoading)
				{
					base.Exit();
				}
				else
				{
					this.SexyZuma.OnHardwareBackButtonPressed();
				}
			}
			TouchCollection state = TouchPanel.GetState();
			this.SexyZuma.GetTouchInputOffset(ref this.GameOffsetX, ref this.GameOffsetY);
			if (state.Count > 0)
			{
				using (TouchCollection.Enumerator enumerator = state.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TouchLocation touchLocation = enumerator.Current;
						if (this.mCurrentTouchId == -1)
						{
							this.mCurrentTouchId = touchLocation.Id;
						}
						else if (touchLocation.Id != this.mCurrentTouchId)
						{
							continue;
						}
						float num = (touchLocation.Position.X - (float)this.GameOffsetX) * this.GameScaleRatio;
						float num2 = (touchLocation.Position.Y - (float)this.GameOffsetY) * this.GameScaleRatio;
						SexyPoint loc = new SexyPoint((int)num, (int)num2);
						switch (touchLocation.State)
						{
						case TouchLocationState.Moved:
							this.touch.SetTouchInfo(loc, _TouchPhase.TOUCH_ENDED, DateTime.Now.TimeOfDay.TotalMilliseconds);
							this.SexyZuma.TouchEnded(this.touch);
							this.mCurrentTouchId = -1;
							break;
						case TouchLocationState.Pressed:
							this.touch.SetTouchInfo(loc, _TouchPhase.TOUCH_BEGAN, DateTime.Now.TimeOfDay.TotalMilliseconds);
							this.SexyZuma.TouchBegan(this.touch);
							break;
						case TouchLocationState.Released:
							this.touch.SetTouchInfo(loc, _TouchPhase.TOUCH_MOVED, DateTime.Now.TimeOfDay.TotalMilliseconds);
							this.SexyZuma.TouchMoved(this.touch);
							break;
						}
					}
					return;
				}
			}
			this.mCurrentTouchId = -1;
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0007BD10 File Offset: 0x00079F10
		public void DrawSysString(string str, float x, float y)
		{
			this.spriteBatch.Begin();
			this.spriteBatch.DrawString(this.mSpriteFont, str, new Vector2(x, y), Color.Yellow);
			this.spriteBatch.End();
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x0007BD46 File Offset: 0x00079F46
		public void OrientationChanged(object sender, EventArgs e)
		{
			if (base.Window.CurrentOrientation == DisplayOrientation.LandscapeLeft)
			{
				if (this.SexyZuma != null)
				{
					this.SexyZuma.SetOrientation(0);
					return;
				}
			}
			else if (this.SexyZuma != null)
			{
				this.SexyZuma.SetOrientation(1);
			}
		}

		// Token: 0x04000B42 RID: 2882
		private GameApp SexyZuma;

		// Token: 0x04000B43 RID: 2883
		private Texture2D splashEA;

		// Token: 0x04000B44 RID: 2884
		private Texture2D splash;

		// Token: 0x04000B45 RID: 2885
		private Color mColor = new Color(255, 255, 255, 255);

		// Token: 0x04000B46 RID: 2886
		private float mAlpha = 255f;

		// Token: 0x04000B47 RID: 2887
		private float mAlphaInc = -6f;

		// Token: 0x04000B48 RID: 2888
		private double mAlphaDelay = 1.0;

		// Token: 0x04000B49 RID: 2889
		private int mSplashId = 1;

		// Token: 0x04000B4A RID: 2890
		private SpriteBatch spriteBatch;

		// Token: 0x04000B4B RID: 2891
		private bool isLoading = true;

		// Token: 0x04000B4C RID: 2892
		private bool mInitBegin;

		// Token: 0x04000B4D RID: 2893
		private int FirstLoad;

		// Token: 0x04000B4E RID: 2894
		private SpriteFont mSpriteFont;

		// Token: 0x04000B4F RID: 2895
		private double mElipseTime;

		// Token: 0x04000B50 RID: 2896
		private int mCurrentTouchId = -1;

		// Token: 0x04000B51 RID: 2897
		private static int frames = 0;

		// Token: 0x04000B52 RID: 2898
		private static DateTime now;

		// Token: 0x04000B53 RID: 2899
		private static DateTime preFPSTime;

		// Token: 0x04000B54 RID: 2900
		private static string fpsDisplayText = "";

		// Token: 0x04000B55 RID: 2901
		//public PhoneApplicationService gApplicationService;

		// Token: 0x04000B56 RID: 2902
		private long totalBytes;

		// Token: 0x04000B57 RID: 2903
		private long currentBytes;

		// Token: 0x04000B58 RID: 2904
		private long peakBytes;

		// Token: 0x04000B59 RID: 2905
		private long limitBytes;

		// Token: 0x04000B5A RID: 2906
		private Vector2 mFPSPos = new Vector2(60f, 10f);

		// Token: 0x04000B5B RID: 2907
		private int GameOffsetX;

		// Token: 0x04000B5C RID: 2908
		private int GameOffsetY;

		// Token: 0x04000B5D RID: 2909
		private float GameScaleRatio = 1.33f;

		// Token: 0x04000B5E RID: 2910
		private SexyAppBase.Touch touch = new SexyAppBase.Touch();
	}
}
