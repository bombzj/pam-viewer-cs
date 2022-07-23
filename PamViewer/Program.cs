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
			game.Run();
		}

		public Program()
		{
            this.mGameApp = new SexyApp();
			((WP7AppDriver)mGameApp.mAppDriver).InitXNADriver(this);
			TouchPanel.EnableMouseTouchPoint = true;
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			base.Initialize();
			this.spriteBatch = new SpriteBatch(base.GraphicsDevice);
			mGameApp.mAppDriver.Init();

			int paIndex = 0;
			foreach (string n in tempGroup)
            {
				string groupName = mResourceManager.mCompositeResGroupMap[n].mSubGroups.Find(x => x.mArtRes == 0).mGroupName;
				ResGroup rg = mResourceManager.mResGroupMap[groupName];
				PopAnimRes pam = (PopAnimRes)rg.mResList.Find(x => x.mType == ResType.ResType_PopAnim);
				popAnim[paIndex] = pam.mPopAnim.Duplicate();
				paIndex++;
			}
			RandomAnim();
		}

		protected override void LoadContent()
		{
			mResourceManager = mGameApp.mResourceManager = new SexyFramework.Resource.ResourceManager(mGameApp);

			DirectoryInfo folder = new DirectoryInfo(contentRoot + "\\" + propertiesRoot);
			foreach (FileInfo file in folder.GetFiles())
			{
				if(file.Extension == ".json")
                {
					mResourceManager.ParseResourcesFileJson(propertiesRoot + "\\" + file.Name);
				}
			}

			mResourceManager.mBaseArtRes = gameRes;
			mResourceManager.mLeadArtRes = gameRes;
			mResourceManager.mCurArtRes = gameRes;

			foreach(string n in tempGroup)
				mResourceManager.LoadResources(n);
		}

		protected override void UnloadContent()
		{
		}

		protected override void Update(GameTime gameTime)
		{
			UpdateInput(gameTime);
			//bool isRunningSlowly = gameTime.IsRunningSlowly;
			base.Update(gameTime);
			this.mElipseTime += gameTime.ElapsedGameTime.TotalSeconds;
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
			base.GraphicsDevice.Clear(Color.White);
			//this.spriteBatch.Begin();
			//this.spriteBatch.End();

			Graphics g = new Graphics();
			g.mTransX = 0;
			foreach (PopAnim pa in popAnim)
			{
				pa?.Draw(g);
				g.mTransX += 180;
				if (g.mTransX > 600)
				{
					g.mTransX = 0;
					g.mTransY += 180;
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
								this.mGameApp.TouchEnded(this.touch);
								this.mCurrentTouchId = -1;
								break;
							case TouchLocationState.Pressed:
								RandomAnim();
								this.touch.SetTouchInfo(loc, _TouchPhase.TOUCH_BEGAN, DateTime.Now.TimeOfDay.TotalMilliseconds);
								this.mGameApp.TouchBegan(this.touch);
								break;
							case TouchLocationState.Released:
								this.touch.SetTouchInfo(loc, _TouchPhase.TOUCH_MOVED, DateTime.Now.TimeOfDay.TotalMilliseconds);
								this.mGameApp.TouchMoved(this.touch);
								break;
						}
					}
					return;
				}
			}
			this.mCurrentTouchId = -1;
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


		private SexyApp mGameApp;
		private PopAnim[] popAnim = new PopAnim[40];

		public static int gameRes = 1200;
		public static string contentRoot = "Content";
		public static string propertiesRoot = "properties";
		public ResourceManager mResourceManager;
		private SpriteBatch spriteBatch;
		private double mElipseTime;
		private int mCurrentTouchId = -1;

		private int GameOffsetX;

		private int GameOffsetY;

		private float GameScaleRatio = 1.33f;

		private SexyAppBase.Touch touch = new SexyAppBase.Touch();
	}
}
