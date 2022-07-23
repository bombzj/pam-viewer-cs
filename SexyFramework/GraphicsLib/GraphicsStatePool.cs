using System;
using System.Collections.Generic;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000AF RID: 175
	public static class GraphicsStatePool
	{
		// Token: 0x06000513 RID: 1299 RVA: 0x0000EC80 File Offset: 0x0000CE80
		public static GraphicsState CreateState()
		{
			GraphicsState result;
			if (GraphicsStatePool.mFreeStates.Count > 0)
			{
				result = GraphicsStatePool.mFreeStates.Pop();
			}
			else
			{
				result = new GraphicsState();
			}
			return result;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0000ECB0 File Offset: 0x0000CEB0
		public static void ReleaseState(GraphicsState state)
		{
			GraphicsStatePool.mFreeStates.Push(state);
		}

		// Token: 0x0400047B RID: 1147
		private static Stack<GraphicsState> mFreeStates = new Stack<GraphicsState>();
	}
}
