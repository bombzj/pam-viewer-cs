using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using PopStudio.Plugin;
using PopStudio.RTON;
using SexyFramework;
using SexyFramework.Drivers.App;
using SexyFramework.Drivers.Graphics;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.Resource;
using SexyFramework.WidgetsLib;

namespace PamViewer
{
	public class Program : Game
	{
		[STAThread]
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
			form = new PamForm(this);

			spriteBatch = new SpriteBatch(base.GraphicsDevice);
			gameApp.mAppDriver.Init();

			//int paIndex = 0;
			//foreach (string n in tempGroup)
   //         {
			//	string groupName = resourceManager.mCompositeResGroupMap[n].mSubGroups.Find(x => x.mArtRes == 0).mGroupName;
			//	ResGroup rg = resourceManager.mResGroupMap[groupName];
			//	PopAnimRes pam = (PopAnimRes)rg.mResList.Find(x => x.mType == ResType.ResType_PopAnim);
			//	popAnim[paIndex] = pam.mPopAnim.Duplicate();
			//	paIndex++;
			//}
			//RandomAnim();
		}

		protected override void LoadContent()
		{
			resourceManager = gameApp.mResourceManager = new SexyFramework.Resource.ResourceManager(gameApp);

			DirectoryInfo folder = new DirectoryInfo(contentRoot + "\\" + propertiesRoot);
			foreach (FileInfo file in folder.GetFiles("RESOURCES*.json"))
				resourceManager.ParseResourcesFileJson(propertiesRoot + "\\" + file.Name);

			foreach (FileInfo file in folder.GetFiles("RESOURCES*.rton"))
            {
				using FileStream stream = new FileStream(contentRoot + "\\" + propertiesRoot + "\\" + file.Name, FileMode.Open);
				BinaryStream bs = new BinaryStream(stream);
                using MemoryStream ms = new MemoryStream(100000);
                RTON.Decode(bs, ms);
				ms.Position = 0;
				using JsonDocument json = JsonDocument.Parse(ms, new JsonDocumentOptions { AllowTrailingCommas = true });

				JsonElement root = json.RootElement;
				resourceManager.ParseResourcesFileJson(root);
			}

			resourceManager.mBaseArtRes = artRes;
			resourceManager.mLeadArtRes = artRes;
			resourceManager.mCurArtRes = artRes;

			//foreach(string n in tempGroup)
			//	resourceManager.LoadResources(n);
		}

		protected override void UnloadContent()
		{
		}

		protected override void Update(GameTime gameTime)
		{
			//UpdateInput(gameTime);
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
						pa.Play(pa.mLastPlayedFrameLabel, false);
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
			g.Translate(-40, 0);
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
			//g.mTransX = g.mTransY = 50;
			//ImageRes res = resourceManager.mResMaps[0]["IMAGE_MOWERS_MOWER_FUTURE_MOWER_FUTURE_119X40"] as ImageRes;
			//DeviceImage deviceImage = (DeviceImage)res.mImage.GetImage();
			//SexyTransform2D mMatrix = new SexyTransform2D(false);
			//Rect rect = new Rect(0, 0, deviceImage.mWidth, deviceImage.mHeight);
			//g.DrawImageMatrix(deviceImage, mMatrix, rect);

			((RenderDevice3D)gameApp.mGraphicsDriver.GetRenderDevice()).Flush();
			base.Draw(gameTime);
		}

		public List<string> searchGroup(string str)
        {
			str = str.ToLower();
			List<string> list = new List<string>();
			foreach (var cgroup in resourceManager.mCompositeResGroupMap)
				if(cgroup.Key.ToLower().IndexOf(str) != -1)
                {
					list.Add(cgroup.Key);
				}
			return list;
        }

		public List<string> listPam(string groupName)
        {
			resourceManager.LoadResources(groupName);
			var group = resourceManager.mCompositeResGroupMap[groupName];
			var rg = resourceManager.mResGroupMap[group.mSubGroups.Find(x => x.mArtRes == 0).mGroupName];
			var pams = rg.mResList.FindAll(x => x.mType == ResType.ResType_PopAnim);
			List<string> list = new List<string>();
			foreach (var pam in pams)
			{
				list.Add(pam.mId);
			}
			return list;
		}

		public List<string> listSprite(string pamName)
        {
			var pam = ((PopAnimRes)resourceManager.mResMaps[(int)ResType.ResType_PopAnim][pamName]).mPopAnim;
			List<string> list = new List<string>();
			foreach(var name in pam.mMainAnimDef.mMainSpriteDef.mLabels.Keys)
            {
				list.Add(name);
			}
			return list;
		}

		public void playSprite(string groupName, string pamName, string spriteName)
        {
			//if(popAnim[0] != null)
			//         {

			//         } else
			//         {

			//         }
			var pam = ((PopAnimRes)resourceManager.mResMaps[(int)ResType.ResType_PopAnim][pamName]).mPopAnim;
			popAnim[0] = pam.Duplicate();
			popAnim[0].Play(spriteName);
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

		//private Random rnd = new Random();
		//public void RandomAnim()
		//{
		//	foreach (PopAnim pa in popAnim)
		//		if (pa != null)
		//		{
		//			string[] labels = pa.mMainAnimDef.mMainSpriteDef.mLabels.Keys.ToArray<string>();
		//			pa.BlendTo(labels[0], 16);
		//		}
		//}

		protected override void OnExiting(object sender, EventArgs args)
		{
			form.Dispose();
		}

		protected override void OnActivated(object sender, EventArgs args)
		{
			base.OnActivated(sender, args);
		}

		protected override void OnDeactivated(object sender, EventArgs args)
		{
			base.OnDeactivated(sender, args);
		}

		//private string[] tempGroup = { "PlantMelonpult" };

		private PamForm form;
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
