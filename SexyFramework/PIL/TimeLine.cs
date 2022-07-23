using System;
using System.Collections.Generic;
using SexyFramework.Misc;

namespace SexyFramework.PIL
{
	// Token: 0x02000180 RID: 384
	public class TimeLine
	{
		// Token: 0x06000D8D RID: 3469 RVA: 0x00043CBF File Offset: 0x00041EBF
		public TimeLine()
		{
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x00043CD4 File Offset: 0x00041ED4
		public TimeLine(TimeLine rhs)
		{
			if (rhs == null)
			{
				return;
			}
			this.mCurrentSettings = rhs.mCurrentSettings.Clone();
			this.mLoop = rhs.mLoop;
			this.mPrev = null;
			this.mNext = null;
			for (int i = 0; i < rhs.mKeyFrames.size<KeyFrame>(); i++)
			{
				KeyFrameData k = rhs.mKeyFrames[i].second.Clone();
				this.AddKeyFrame(rhs.mKeyFrames[i].first, k);
			}
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00043D68 File Offset: 0x00041F68
		public virtual void Dispose()
		{
			for (int i = 0; i < this.mKeyFrames.size<KeyFrame>(); i++)
			{
				this.mKeyFrames[i] = null;
			}
			this.mKeyFrames.Clear();
			this.mCurrentSettings = null;
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00043DAC File Offset: 0x00041FAC
		public void Update(int frame)
		{
			this.mPrev = (this.mNext = null);
			if (this.mKeyFrames.size<KeyFrame>() == 0)
			{
				return;
			}
			frame = ((this.mLoop && this.mKeyFrames.size<KeyFrame>() > 1) ? (frame % this.mKeyFrames[this.mKeyFrames.size<KeyFrame>() - 1].first) : frame);
			for (int i = 0; i < this.mKeyFrames.size<KeyFrame>(); i++)
			{
				if (this.mKeyFrames[i].first > frame)
				{
					this.mNext = this.mKeyFrames[i];
					break;
				}
				this.mPrev = this.mKeyFrames[i];
			}
			this.mCurrentSettings.CopyFrom(this.mPrev.second);
			if (this.mNext != null)
			{
				int num = this.mNext.first - this.mPrev.first;
				float num2 = (float)(frame - this.mPrev.first) / (float)num;
				for (int j = 0; j < this.mCurrentSettings.mNumInts; j++)
				{
					this.mCurrentSettings.mIntData[j] += (int)((float)(this.mNext.second.mIntData[j] - this.mPrev.second.mIntData[j]) * num2);
				}
				for (int k = 0; k < this.mCurrentSettings.mNumFloats; k++)
				{
					this.mCurrentSettings.mFloatData[k] += (this.mNext.second.mFloatData[k] - this.mPrev.second.mFloatData[k]) * num2;
				}
			}
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00043F6D File Offset: 0x0004216D
		public void AddKeyFrame(int frame, KeyFrameData k)
		{
			this.mKeyFrames.Add(new KeyFrame(frame, k));
			this.mKeyFrames.Sort(new KeyFrameSort());
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00043F94 File Offset: 0x00042194
		public virtual void Serialize(SexyBuffer b)
		{
			b.WriteBoolean(this.mLoop);
			this.mCurrentSettings.Serialize(b);
			b.WriteLong((long)this.mKeyFrames.Count);
			for (int i = 0; i < this.mKeyFrames.Count; i++)
			{
				b.WriteLong((long)this.mKeyFrames[i].first);
				this.mKeyFrames[i].second.Serialize(b);
			}
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00044010 File Offset: 0x00042210
		public virtual void Deserialize(SexyBuffer b, GlobalMembers.KFDInstantiateFunc f)
		{
			this.mKeyFrames.Clear();
			this.mLoop = b.ReadBoolean();
			this.mCurrentSettings.Deserialize(b);
			int num = (int)b.ReadLong();
			for (int i = 0; i < num; i++)
			{
				b.ReadLong();
				KeyFrameData keyFrameData = f();
				keyFrameData.Deserialize(b);
			}
		}

		// Token: 0x04000B0A RID: 2826
		public List<KeyFrame> mKeyFrames = new List<KeyFrame>();

		// Token: 0x04000B0B RID: 2827
		public KeyFrame mPrev;

		// Token: 0x04000B0C RID: 2828
		public KeyFrame mNext;

		// Token: 0x04000B0D RID: 2829
		public KeyFrameData mCurrentSettings;

		// Token: 0x04000B0E RID: 2830
		public bool mLoop;
	}
}
