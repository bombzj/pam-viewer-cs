using System;
using System.Collections.Generic;
using SexyFramework.GraphicsLib;

namespace JeffLib
{
	// Token: 0x02000107 RID: 263
	public class Component
	{
		// Token: 0x060007EF RID: 2031 RVA: 0x000285BC File Offset: 0x000267BC
		public Component()
		{
			this.mValue = 0f;
			this.mOriginalValue = 0f;
			this.mTargetValue = 0f;
			this.mStartFrame = 0;
			this.mEndFrame = 0;
			this.mValueDelta = 0f;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0002860C File Offset: 0x0002680C
		public Component(float val)
		{
			this.mTargetValue = val;
			this.mOriginalValue = val;
			this.mValue = val;
			this.mStartFrame = 0;
			this.mEndFrame = 0;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00028648 File Offset: 0x00026848
		public Component(float val, float target)
		{
			this.mOriginalValue = val;
			this.mValue = val;
			this.mTargetValue = target;
			this.mStartFrame = 0;
			this.mEndFrame = 0;
			this.mValueDelta = ((this.mEndFrame - this.mStartFrame == 0) ? 0f : ((target - val) / (float)(this.mEndFrame - this.mStartFrame)));
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x000286B0 File Offset: 0x000268B0
		public Component(float val, float target, int start)
		{
			this.mOriginalValue = val;
			this.mValue = val;
			this.mTargetValue = target;
			this.mStartFrame = start;
			this.mEndFrame = 0;
			this.mValueDelta = ((this.mEndFrame - this.mStartFrame == 0) ? 0f : ((target - val) / (float)(this.mEndFrame - this.mStartFrame)));
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00028718 File Offset: 0x00026918
		public Component(float val, float target, int start, int end)
		{
			this.mOriginalValue = val;
			this.mValue = val;
			this.mTargetValue = target;
			this.mStartFrame = start;
			this.mEndFrame = end;
			this.mValueDelta = ((this.mEndFrame - this.mStartFrame == 0) ? 0f : ((target - val) / (float)(this.mEndFrame - this.mStartFrame)));
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0002877F File Offset: 0x0002697F
		public bool Active(int count)
		{
			return count >= this.mStartFrame && count <= this.mEndFrame;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00028798 File Offset: 0x00026998
		public void SyncState(DataSyncBase sync)
		{
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x0002879C File Offset: 0x0002699C
		public void Update()
		{
			this.mValue += this.mValueDelta;
			if ((this.mValueDelta > 0f && this.mValue > this.mTargetValue) || (this.mValueDelta < 0f && this.mValue < this.mTargetValue))
			{
				this.mValue = this.mTargetValue;
			}
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00028800 File Offset: 0x00026A00
		public static bool UpdateComponentVec(List<Component> vec, int update_count)
		{
			bool result = true;
			for (int i = 0; i < vec.Count; i++)
			{
				Component component = vec[i];
				if (component.Active(update_count))
				{
					component.Update();
					return false;
				}
				if (update_count < component.mStartFrame)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00028848 File Offset: 0x00026A48
		public static bool UpdateComponentVec(List<KeyValuePair<Component, Image>> vec, int update_count)
		{
			bool result = true;
			for (int i = 0; i < vec.Count; i++)
			{
				Component key = vec[i].Key;
				if (key.Active(update_count))
				{
					key.Update();
					return false;
				}
				if (update_count < key.mStartFrame)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00028898 File Offset: 0x00026A98
		public static float GetComponentValue(List<Component> v, float def_value, int update_count)
		{
			for (int i = 0; i < v.Count; i++)
			{
				Component component = v[i];
				if (update_count < component.mStartFrame)
				{
					return component.mValue;
				}
				if (component.Active(update_count))
				{
					return component.mValue;
				}
				if (i == v.Count - 1)
				{
					return component.mValue;
				}
			}
			return def_value;
		}

		// Token: 0x04000757 RID: 1879
		public float mValue;

		// Token: 0x04000758 RID: 1880
		public float mOriginalValue;

		// Token: 0x04000759 RID: 1881
		public float mTargetValue;

		// Token: 0x0400075A RID: 1882
		public int mStartFrame;

		// Token: 0x0400075B RID: 1883
		public int mEndFrame;

		// Token: 0x0400075C RID: 1884
		public float mValueDelta;
	}
}
