using System;
using System.Collections.Generic;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace SexyFramework.AELib
{
	// Token: 0x0200000B RID: 11
	public class Composition : Layer
	{
		// Token: 0x06000031 RID: 49 RVA: 0x00002F67 File Offset: 0x00001167
		public Composition()
		{
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002FA8 File Offset: 0x000011A8
		public Composition(Composition other)
		{
			this.CopyFrom(other);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002FF8 File Offset: 0x000011F8
		public void CopyFrom(Composition other)
		{
			base.CopyFrom(other);
			this.CopyLayersFrom(other);
			this.mMaxDuration = other.mMaxDuration;
			this.mLoadImageFunc = other.mLoadImageFunc;
			this.mPostLoadImageFunc = other.mPostLoadImageFunc;
			this.mPreLayerDrawFunc = other.mPreLayerDrawFunc;
			this.mPreLayerDrawData = other.mPreLayerDrawData;
			this.mUpdateCount = other.mUpdateCount;
			this.mLoop = other.mLoop;
			this.mMaxFrame = other.mMaxFrame;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003074 File Offset: 0x00001274
		public void CopyLayersFrom(Composition other)
		{
			this.mLayers.Clear();
			if (other.mLayers != null)
			{
				for (int i = 0; i < other.mLayers.Count; i++)
				{
					CompLayer compLayer = other.mLayers[i];
					this.mLayers.Add(new CompLayer(compLayer.mSource.Duplicate(), compLayer.mStartFrameOnComp, compLayer.mDuration, compLayer.mLayerOffsetStart));
				}
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000030E4 File Offset: 0x000012E4
		public override Layer Duplicate()
		{
			return new Composition(this);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000030EC File Offset: 0x000012EC
		public bool Done()
		{
			return this.mUpdateCount >= this.mMaxDuration;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003100 File Offset: 0x00001300
		public override bool isValid()
		{
			foreach (CompLayer compLayer in this.mLayers)
			{
				if (!compLayer.mSource.isValid())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003160 File Offset: 0x00001360
		public bool LoadFromFile(string file_name)
		{
			return this.LoadFromFile(file_name, "Main");
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003170 File Offset: 0x00001370
		public bool LoadFromFile(string file_name, string main_composition_name)
		{
			List<Composition> list = new List<Composition>();
			if (!AECommon.LoadPAX(file_name, list, this.mLoadImageFunc, this.mPostLoadImageFunc) || list.Count == 0)
			{
				return false;
			}
			Composition composition = null;
			for (int i = 0; i < list.Count; i++)
			{
				Composition composition2 = list[i];
				if (composition2.mLayerName.ToLower().Equals(main_composition_name.ToLower()))
				{
					composition = composition2;
					break;
				}
			}
			if (composition == null)
			{
				composition = list[0];
			}
			this.CopyFrom(composition);
			return true;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000031EC File Offset: 0x000013EC
		public void AddLayer(CompLayer c)
		{
			this.mLayers.Add(new CompLayer(c));
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000031FF File Offset: 0x000013FF
		public void AddLayer(Layer l, int start_frame, int duration, int layer_offset)
		{
			this.mLayers.Add(new CompLayer(l, start_frame, duration, layer_offset));
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003218 File Offset: 0x00001418
		public void Update()
		{
			this.mUpdateCount++;
			if (this.mMaxFrame == -1)
			{
				this.mMaxFrame = this.mMaxDuration;
			}
			if (this.mLoop && this.mUpdateCount >= this.mMaxFrame)
			{
				this.Reset();
				this.mUpdateCount = 1;
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000326B File Offset: 0x0000146B
		public override void Draw(Graphics g)
		{
			this.Draw(g, null);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003275 File Offset: 0x00001475
		public override void Draw(Graphics g, CumulativeTransform ctrans)
		{
			this.Draw(g, ctrans, -1);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003280 File Offset: 0x00001480
		public override void Draw(Graphics g, CumulativeTransform ctrans, int frame)
		{
			this.Draw(g, ctrans, frame, 1f);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003290 File Offset: 0x00001490
		public override void Draw(Graphics g, CumulativeTransform ctrans, int frame, float scale)
		{
			if (frame == -1)
			{
				frame = this.mUpdateCount;
			}
			bool flag = false;
			for (int i = this.mLayers.Count - 1; i >= 0; i--)
			{
				CompLayer compLayer = this.mLayers[i];
				if (frame >= compLayer.mStartFrameOnComp && frame < compLayer.mStartFrameOnComp + compLayer.mDuration)
				{
					int frame2 = frame - compLayer.mStartFrameOnComp + compLayer.mLayerOffsetStart;
					CumulativeTransform cumulativeTransform = ctrans;
					CumulativeTransform cumulativeTransform2 = new CumulativeTransform();
					if (ctrans == null)
					{
						ctrans = cumulativeTransform2;
					}
					else if (!flag)
					{
						flag = true;
						float num = 1f;
						float num2 = 0f;
						this.mOpacity.GetValue(frame, ref num);
						ctrans.mOpacity *= num;
						SexyTransform2D theMat = new SexyTransform2D(false);
						this.mAnchorPoint.GetValue(frame, ref num, ref num2);
						float num3 = num * scale;
						float num4 = num2 * scale;
						theMat.Translate(-num3, -num4);
						float sx = 1f;
						float sy = 1f;
						this.mScale.GetValue(frame, ref sx, ref sy);
						theMat.Scale(sx, sy);
						this.mRotation.GetValue(frame, ref num);
						if (num != 0f)
						{
							theMat.RotateRad(-num);
						}
						this.mPosition.GetValue(frame, ref num, ref num2);
						theMat.Translate(num * scale, num2 * scale);
						ctrans.mTrans *= theMat;
					}
					if (compLayer.mSource.IsLayerBase() && this.mPreLayerDrawFunc != null)
					{
						this.mPreLayerDrawFunc(g, compLayer.mSource, this.mPreLayerDrawData);
					}
					CumulativeTransform other = new CumulativeTransform(ctrans);
					if (this.mAdditive)
					{
						ctrans.mForceAdditive = true;
					}
					if (compLayer.mSource.NeedsTranslatedFrame())
					{
						compLayer.mSource.Draw(g, ctrans, frame2, scale);
					}
					else
					{
						compLayer.mSource.Draw(g, ctrans, frame, scale);
					}
					ctrans.CopyFrom(other);
					ctrans = cumulativeTransform;
				}
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003480 File Offset: 0x00001680
		public override void Reset()
		{
			this.mUpdateCount = 0;
			for (int i = 0; i < this.mLayers.Count; i++)
			{
				this.mLayers[i].mSource.Reset();
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000034C0 File Offset: 0x000016C0
		public Layer GetLayerAtIdx(int idx)
		{
			return this.mLayers[idx].mSource;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000034D3 File Offset: 0x000016D3
		public override bool NeedsTranslatedFrame()
		{
			return true;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000034D6 File Offset: 0x000016D6
		public int GetMaxDuration()
		{
			return this.mMaxDuration;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000034DE File Offset: 0x000016DE
		public int GetUpdateCount()
		{
			return this.mUpdateCount;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000034E6 File Offset: 0x000016E6
		public int GetNumLayers()
		{
			return this.mLayers.Count;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000034F3 File Offset: 0x000016F3
		public override bool IsLayerBase()
		{
			return false;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000034F6 File Offset: 0x000016F6
		public void SetMaxDuration(int m)
		{
			this.mMaxDuration = m;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000034FF File Offset: 0x000016FF
		public static void DefaultPostLoadImageFunc(SharedImageRef img, Layer l)
		{
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003501 File Offset: 0x00001701
		public static SharedImageRef DefaultLoadImageFunc(string file_dir, string file_name)
		{
			return GlobalMembers.gSexyApp.GetSharedImage(file_dir + file_name);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003514 File Offset: 0x00001714
		public void Dispose()
		{
		}

		// Token: 0x04000025 RID: 37
		protected List<CompLayer> mLayers = new List<CompLayer>();

		// Token: 0x04000026 RID: 38
		protected int mMaxDuration;

		// Token: 0x04000027 RID: 39
		public AECommon.LoadCompImageFunc mLoadImageFunc = new AECommon.LoadCompImageFunc(Composition.DefaultLoadImageFunc);

		// Token: 0x04000028 RID: 40
		public AECommon.PostLoadCompImageFunc mPostLoadImageFunc = new AECommon.PostLoadCompImageFunc(Composition.DefaultPostLoadImageFunc);

		// Token: 0x04000029 RID: 41
		public AECommon.PreLayerDrawFunc mPreLayerDrawFunc;

		// Token: 0x0400002A RID: 42
		public object mPreLayerDrawData;

		// Token: 0x0400002B RID: 43
		public int mUpdateCount;

		// Token: 0x0400002C RID: 44
		public bool mLoop;

		// Token: 0x0400002D RID: 45
		public int mMaxFrame = -1;
	}
}
