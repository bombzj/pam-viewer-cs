using System;
using System.Collections.Generic;

namespace SexyFramework.WidgetsLib
{
	// Token: 0x020001BF RID: 447
	public class PASpriteDef
	{
		// Token: 0x06001048 RID: 4168 RVA: 0x0004D803 File Offset: 0x0004BA03
		public virtual void Dispose()
		{
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x0004D808 File Offset: 0x0004BA08
		public int GetLabelFrame(string theLabel)
		{
			string text = theLabel.ToUpper();
			if (!this.mLabels.ContainsKey(text))
			{
				return -1;
			}
			return this.mLabels[text];
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0004D838 File Offset: 0x0004BA38
		public void GetLabelFrameRange(string theLabel, ref int theStart, ref int theEnd)
		{
			theStart = this.GetLabelFrame(theLabel);
			theEnd = -1;
			if (theStart == -1)
			{
				return;
			}
			string text = theLabel.ToUpper();
			Dictionary<string, int>.Enumerator enumerator = this.mLabels.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string text2 = text;
				KeyValuePair<string, int> keyValuePair = enumerator.Current;
				if (text2 != keyValuePair.Key)
				{
					KeyValuePair<string, int> keyValuePair2 = enumerator.Current;
					if (keyValuePair2.Value > theStart)
					{
						if (theEnd >= 0)
						{
							KeyValuePair<string, int> keyValuePair3 = enumerator.Current;
							if (keyValuePair3.Value >= theEnd)
							{
								continue;
							}
						}
						KeyValuePair<string, int> keyValuePair4 = enumerator.Current;
						theEnd = keyValuePair4.Value - 1;
					}
				}
			}
			if (theEnd < 0)
			{
				theEnd = this.mFrames.Count - 1;
			}
		}

		// Token: 0x04000D0C RID: 3340
		public string mName;

		// Token: 0x04000D0D RID: 3341
		public List<PAFrame> mFrames = new List<PAFrame>();

		// Token: 0x04000D0E RID: 3342
		public int mWorkAreaStart;

		// Token: 0x04000D0F RID: 3343
		public int mWorkAreaDuration;

		// Token: 0x04000D10 RID: 3344
		public Dictionary<string, int> mLabels = new Dictionary<string, int>();

		// Token: 0x04000D11 RID: 3345
		public List<PAObjectDef> mObjectDefVector = new List<PAObjectDef>();

		// Token: 0x04000D12 RID: 3346
		public float mAnimRate;
	}
}
