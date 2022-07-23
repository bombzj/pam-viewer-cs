using System;
using System.Collections.Generic;

namespace SexyFramework.AELib
{
	// Token: 0x0200000F RID: 15
	public class Timeline
	{
		// Token: 0x06000064 RID: 100 RVA: 0x00003A0A File Offset: 0x00001C0A
		public Timeline()
		{
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003A32 File Offset: 0x00001C32
		public Timeline(Timeline rhs)
		{
			this.CopyFrom(rhs);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003A64 File Offset: 0x00001C64
		public void CopyFrom(Timeline rhs)
		{
			this.mPingForward = rhs.mPingForward;
			this.mLastPingPongFrame = rhs.mLastPingPongFrame;
			this.mLoopType = rhs.mLoopType;
			this.mLoopFrame = rhs.mLoopFrame;
			this.mKeyframes.Clear();
			for (int i = 0; i < rhs.mKeyframes.Count; i++)
			{
				this.mKeyframes.Add(new Keyframe(rhs.mKeyframes[i]));
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003ADE File Offset: 0x00001CDE
		public void Reset()
		{
			this.mPingForward = false;
			this.mLastPingPongFrame = -1;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003AEE File Offset: 0x00001CEE
		public bool HasInitialValue()
		{
			return this.mKeyframes.Count > 0 && this.mKeyframes[0].mFrame == 0;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003B14 File Offset: 0x00001D14
		public void AddKeyframe(int frame, float value1)
		{
			this.AddKeyframe(frame, value1, 0f);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003B24 File Offset: 0x00001D24
		public void AddKeyframe(int frame, float value1, float value2)
		{
			for (int i = 0; i < this.mKeyframes.Count; i++)
			{
				if (this.mKeyframes[i].mFrame == frame)
				{
					this.mKeyframes[i].mValue1 = value1;
					this.mKeyframes[i].mValue2 = value2;
					return;
				}
			}
			this.mKeyframes.Add(new Keyframe(frame, value1, value2));
			this.mKeyframes.Sort(new Keyframe.KeyFrameSort());
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003BA4 File Offset: 0x00001DA4
		public void GetValue(int frame, ref float value1)
		{
			float num = 0f;
			this.GetValue(frame, ref value1, ref num);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003BC4 File Offset: 0x00001DC4
		public void GetValue(int frame, ref float value1, ref float value2)
		{
			if (this.mKeyframes.Count == 0)
			{
				return;
			}
			if (this.mKeyframes.Count == 1)
			{
				value1 = this.mKeyframes[0].mValue1;
				value2 = this.mKeyframes[0].mValue2;
				return;
			}
			int mFrame = this.mKeyframes[this.mKeyframes.Count - 1].mFrame;
			if (this.mLoopType == 10 && frame > mFrame)
			{
				frame = this.mLoopFrame + frame % (mFrame - this.mLoopFrame);
			}
			else if (this.mLoopType == 11 && frame > mFrame)
			{
				frame = this.mLoopFrame + frame % (mFrame - this.mLoopFrame);
				if (frame < this.mLastPingPongFrame)
				{
					this.mPingForward = !this.mPingForward;
				}
				this.mLastPingPongFrame = frame;
				if (!this.mPingForward)
				{
					frame = mFrame - frame + this.mLoopFrame;
				}
				else if (frame >= mFrame || frame < this.mLastPingPongFrame)
				{
					this.mPingForward = !this.mPingForward;
				}
			}
			for (int i = 1; i < this.mKeyframes.Count; i++)
			{
				Keyframe keyframe = this.mKeyframes[i];
				if (keyframe.mFrame > frame)
				{
					Keyframe keyframe2 = this.mKeyframes[i - 1];
					float num = (float)(frame - keyframe2.mFrame) / (float)(keyframe.mFrame - keyframe2.mFrame);
					value1 = (1f - num) * keyframe2.mValue1 + num * keyframe.mValue1;
					value2 = (1f - num) * keyframe2.mValue2 + num * keyframe.mValue2;
					return;
				}
			}
			value1 = this.mKeyframes[this.mKeyframes.Count - 1].mValue1;
			value2 = this.mKeyframes[this.mKeyframes.Count - 1].mValue2;
		}

		// Token: 0x04000035 RID: 53
		protected List<Keyframe> mKeyframes = new List<Keyframe>();

		// Token: 0x04000036 RID: 54
		protected bool mPingForward;

		// Token: 0x04000037 RID: 55
		protected int mLastPingPongFrame = -1;

		// Token: 0x04000038 RID: 56
		public int mLoopType = -1;

		// Token: 0x04000039 RID: 57
		public int mLoopFrame = -1;
	}
}
