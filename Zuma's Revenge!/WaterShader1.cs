using System;
using System.Collections.Generic;
using SexyFramework;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x020000BC RID: 188
	public class WaterShader1 : Effect
	{
		// Token: 0x06000AAA RID: 2730 RVA: 0x00067DA8 File Offset: 0x00065FA8
		protected override void Init()
		{
			for (int i = 0; i < this.mImages.Count; i++)
			{
				if (this.mImages[i].mFileName.Length > 0)
				{
					if (this.mImages[i].mImage != null)
					{
						this.mImages[i].mImage.Dispose();
						this.mImages[i].mImage = null;
					}
				}
				else
				{
					GameApp.gApp.mResourceManager.DeleteResources(this.mImages[i].mResId);
				}
				this.mImages[i].mImage = null;
			}
			for (int j = 0; j < this.mImages.Count; j++)
			{
				WaterShaderImage waterShaderImage = this.mImages[j];
				if (waterShaderImage.mImage == null)
				{
					if (waterShaderImage.mFileName.Length > 0)
					{
						waterShaderImage.mImage = GameApp.gApp.GetImage(GameApp.gApp.GetResImagesDir() + waterShaderImage.mFileName, true, true, false);
					}
					else
					{
						SharedImageRef sharedImageRef = GameApp.gApp.mResourceManager.LoadImage(waterShaderImage.mResId);
						if (sharedImageRef != null)
						{
							waterShaderImage.mImage = (DeviceImage)sharedImageRef.GetImage();
						}
					}
				}
			}
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00067EE9 File Offset: 0x000660E9
		public override void Update()
		{
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x00067EEC File Offset: 0x000660EC
		public override void DrawUnderBackground(Graphics g)
		{
			bool flag = GameApp.gApp.ShadersSupported();
			if (!GameApp.gApp.mLoadingThreadStarted || !GameApp.gApp.mLoadingThreadCompleted)
			{
			}
			flag = false;
			for (int i = 0; i < this.mImages.Count; i++)
			{
				if (!flag || this.mImages[i].mBypass)
				{
					int theX = (this.mImages[i].mScale ? Common._S(this.mImages[i].mX) : Common._DS(this.mImages[i].mX - 160));
					int theY = (this.mImages[i].mScale ? Common._S(this.mImages[i].mY) : Common._DS(this.mImages[i].mY));
					g.DrawImage(this.mImages[i].mImage, theX, theY);
				}
			}
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00067FFC File Offset: 0x000661FC
		public override void SetParams(string key, string value)
		{
			if (key.IndexOf("image") != 0 && key.IndexOf("resid") != 0)
			{
				return;
			}
			int i;
			for (i = 5; i < key.Length; i++)
			{
				try
				{
					float.Parse(string.Concat(key[i]));
				}
				catch (Exception)
				{
					break;
				}
			}
			string text = key.Substring(5, i - 5);
			int num = SexyFramework.Common.StrToInt(text);
			WaterShaderImage waterShaderImage = null;
			for (int j = 0; j < this.mImages.Count; j++)
			{
				if (this.mImages[j].mId == num)
				{
					waterShaderImage = this.mImages[j];
					break;
				}
			}
			if (waterShaderImage == null)
			{
				waterShaderImage = new WaterShaderImage();
				this.mImages.Add(waterShaderImage);
				waterShaderImage.mId = num;
			}
			char c = key[key.Length - 1];
			if (text.Length + 5 == key.Length)
			{
				if (key.IndexOf("image") == 0)
				{
					waterShaderImage.mFileName = value;
					return;
				}
				waterShaderImage.mResId = "IMAGE_LEVELS_" + value;
				return;
			}
			else
			{
				if (c == 'x' || c == 'X')
				{
					waterShaderImage.mX = SexyFramework.Common.StrToInt(value);
					return;
				}
				if (c == 'y' || c == 'Y')
				{
					waterShaderImage.mY = SexyFramework.Common.StrToInt(value);
					return;
				}
				if (Common.StrEquals(key.Substring(text.Length + 5, key.Length), "scale"))
				{
					waterShaderImage.mScale = bool.Parse(value);
					return;
				}
				if (Common.StrEquals(key.Substring(text.Length + 5, key.Length), "bypass"))
				{
					waterShaderImage.mBypass = bool.Parse(value);
				}
				return;
			}
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x000681A4 File Offset: 0x000663A4
		public override void NukeParams()
		{
			for (int i = 0; i < this.mImages.Count; i++)
			{
				if (this.mImages[i].mFileName.Length > 0)
				{
					if (this.mImages[i].mImage != null)
					{
						this.mImages[i].mImage.Dispose();
						this.mImages[i].mImage = null;
					}
				}
				else
				{
					GameApp.gApp.mResourceManager.DeleteResources(this.mImages[i].mResId);
				}
			}
			this.mImages.Clear();
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0006824B File Offset: 0x0006644B
		public override string GetName()
		{
			return "WaterShader1";
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00068252 File Offset: 0x00066452
		public override void CopyFrom(Effect e)
		{
		}

		// Token: 0x04000920 RID: 2336
		protected List<WaterShaderImage> mImages = new List<WaterShaderImage>();
	}
}
