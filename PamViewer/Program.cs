using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using SexyFramework;
using SexyFramework.Drivers.App;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.Resource;
using SexyFramework.WidgetsLib;

namespace PamViewer
{
	public class Program : Game
	{
		static void Main()
		{
			Program game = new Program();
			game.Window.AllowUserResizing = true;
			game.Run();
		}

		public Program()
		{
            gameApp = new SexyApp();
			((WP7AppDriver)gameApp.mAppDriver).InitXNADriver(this);
			TouchPanel.EnableMouseTouchPoint = true;
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			base.Initialize();
			spriteBatch = new SpriteBatch(base.GraphicsDevice);
			gameApp.mAppDriver.Init();

			int paIndex = 0;
			foreach (string n in tempGroup)
            {
				string groupName = resourceManager.mCompositeResGroupMap[n].mSubGroups.Find(x => x.mArtRes == 0).mGroupName;
				ResGroup rg = resourceManager.mResGroupMap[groupName];
				PopAnimRes pam = (PopAnimRes)rg.mResList.Find(x => x.mType == ResType.ResType_PopAnim);
				popAnim[paIndex] = pam.mPopAnim.Duplicate();
				paIndex++;
			}
			RandomAnim();
		}

		protected override void LoadContent()
		{
			resourceManager = gameApp.mResourceManager = new SexyFramework.Resource.ResourceManager(gameApp);

			DirectoryInfo folder = new DirectoryInfo(contentRoot + "\\" + propertiesRoot);
			foreach (FileInfo file in folder.GetFiles())
			{
				if(file.Extension == ".json")
                {
					resourceManager.ParseResourcesFileJson(propertiesRoot + "\\" + file.Name);
				}
			}

			resourceManager.mBaseArtRes = artRes;
			resourceManager.mLeadArtRes = artRes;
			resourceManager.mCurArtRes = artRes;

			foreach(string n in tempGroup)
				resourceManager.LoadResources(n);
		}

		protected override void UnloadContent()
		{
		}

		protected override void Update(GameTime gameTime)
		{
			UpdateInput(gameTime);
			//bool isRunningSlowly = gameTime.IsRunningSlowly;
			base.Update(gameTime);
			elipseTime += gameTime.ElapsedGameTime.TotalSeconds;
			foreach (PopAnim pa in popAnim)
			{
				if (pa != null)
				{
					if (pa.mAnimRunning)
					{
						pa.Update();
					}
					else
					{
						pa.Play(pa.mLastPlayedFrameLabel);
					}
				}
			}
		}
		
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White);
			//spriteBatch.Begin();
			//spriteBatch.End();

			Graphics g = new Graphics();
			g.mTransX = -40;
			foreach (PopAnim pa in popAnim)
			{
                pa?.Draw(g);
                g.mTransX += 160;
				if (g.mTransX > 400)
				{
					g.mTransX = 0;
					g.mTransY += 160;
				}
			}
			g.ClearRenderContext();
			base.Draw(gameTime);
		}

		private void UpdateInput(GameTime gameTime)
		{
			TouchCollection state = TouchPanel.GetState();
			if (state.Count > 0)
			{
				using (TouchCollection.Enumerator enumerator = state.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TouchLocation touchLocation = enumerator.Current;
						if (currentTouchId == -1)
						{
							currentTouchId = touchLocation.Id;
						}
						else if (touchLocation.Id != currentTouchId)
						{
							continue;
						}
						float num = (touchLocation.Position.X - (float)gameOffsetX) * gameScaleRatio;
						float num2 = (touchLocation.Position.Y - (float)gameOffsetY) * gameScaleRatio;
						SexyPoint loc = new SexyPoint((int)num, (int)num2);
						switch (touchLocation.State)
						{
							case TouchLocationState.Moved:
								this.touch.SetTouchInfo(loc, _TouchPhase.TOUCH_ENDED, DateTime.Now.TimeOfDay.TotalMilliseconds);
								this.gameApp.TouchEnded(this.touch);
								currentTouchId = -1;
								break;
							case TouchLocationState.Pressed:
								RandomAnim();
								this.touch.SetTouchInfo(loc, _TouchPhase.TOUCH_BEGAN, DateTime.Now.TimeOfDay.TotalMilliseconds);
								this.gameApp.TouchBegan(this.touch);
								break;
							case TouchLocationState.Released:
								this.touch.SetTouchInfo(loc, _TouchPhase.TOUCH_MOVED, DateTime.Now.TimeOfDay.TotalMilliseconds);
								this.gameApp.TouchMoved(this.touch);
								break;
						}
					}
					return;
				}
			}
			currentTouchId = -1;
		}

		private Random rnd = new Random();
		private void RandomAnim()
		{
			foreach (PopAnim pa in popAnim)
				if (pa != null)
				{
					string[] labels = pa.mMainAnimDef.mMainSpriteDef.mLabels.Keys.ToArray<string>();
					pa.Play(labels[rnd.Next(labels.Length)]);
				}
		}

		protected override void OnExiting(object sender, EventArgs args)
		{

		}

		protected override void OnActivated(object sender, EventArgs args)
		{
			base.OnActivated(sender, args);
		}

		protected override void OnDeactivated(object sender, EventArgs args)
		{
			base.OnDeactivated(sender, args);
		}

		private string[] tempGroup = {"FutureMowerGroup", "SunBombChallengeModule",
			"ZombieEgyptBasicGroup", "PlantMelonpult", "PlantCherryBomb", "PlantThreepeater",
			"PlantSunflower", "PlantSnowPea" };


		private SexyApp gameApp;
		private PopAnim[] popAnim = new PopAnim[40];

		public static int artRes = 1200;
		public static string contentRoot = "Content";
		public static string propertiesRoot = "properties";
		public ResourceManager resourceManager;
		private SpriteBatch spriteBatch;

		private double elipseTime;
		private int currentTouchId = -1;
		private int gameOffsetX;
		private int gameOffsetY;
		private float gameScaleRatio = 1.33f;

		private SexyAppBase.Touch touch = new SexyAppBase.Touch();
	}
}
