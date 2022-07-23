using System;
using System.Collections.Generic;
using SexyFramework.Misc;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000AE RID: 174
	public class GraphicsState
	{
		// Token: 0x06000510 RID: 1296 RVA: 0x0000EB2C File Offset: 0x0000CD2C
		public void CopyStateFrom(GraphicsState theState)
		{
			this.mDestImage = theState.mDestImage;
			this.mTransX = theState.mTransX;
			this.mTransY = theState.mTransY;
			this.mClipRect = theState.mClipRect;
			this.mFont = theState.mFont;
			this.mPushedColorVector = theState.mPushedColorVector;
			this.mColor = theState.mColor;
			this.mFinalColor = theState.mFinalColor;
			this.mDrawMode = theState.mDrawMode;
			this.mColorizeImages = theState.mColorizeImages;
			this.mFastStretch = theState.mFastStretch;
			this.mWriteColoredString = theState.mWriteColoredString;
			this.mLinearBlend = theState.mLinearBlend;
			this.mScaleX = theState.mScaleX;
			this.mScaleY = theState.mScaleY;
			this.mScaleOrigX = theState.mScaleOrigX;
			this.mScaleOrigY = theState.mScaleOrigY;
			this.mIs3D = theState.mIs3D;
		}

		// Token: 0x04000465 RID: 1125
		public static Image mStaticImage = new Image();

		// Token: 0x04000466 RID: 1126
		protected static SexyPoint[] mPFPoints = null;

		// Token: 0x04000467 RID: 1127
		public Image mDestImage;

		// Token: 0x04000468 RID: 1128
		public float mTransX;

		// Token: 0x04000469 RID: 1129
		public float mTransY;

		// Token: 0x0400046A RID: 1130
		public float mScaleX;

		// Token: 0x0400046B RID: 1131
		public float mScaleY;

		// Token: 0x0400046C RID: 1132
		public float mScaleOrigX;

		// Token: 0x0400046D RID: 1133
		public float mScaleOrigY;

		// Token: 0x0400046E RID: 1134
		public Rect mClipRect = default(Rect);

		// Token: 0x0400046F RID: 1135
		public Rect mDestRect = default(Rect);

		// Token: 0x04000470 RID: 1136
		public Rect mSrcRect = default(Rect);

		// Token: 0x04000471 RID: 1137
		public List<SexyColor> mPushedColorVector = new List<SexyColor>();

		// Token: 0x04000472 RID: 1138
		public SexyColor mFinalColor = default(SexyColor);

		// Token: 0x04000473 RID: 1139
		public SexyColor mColor = default(SexyColor);

		// Token: 0x04000474 RID: 1140
		public Font mFont;

		// Token: 0x04000475 RID: 1141
		public int mDrawMode;

		// Token: 0x04000476 RID: 1142
		public bool mColorizeImages;

		// Token: 0x04000477 RID: 1143
		public bool mFastStretch;

		// Token: 0x04000478 RID: 1144
		public bool mWriteColoredString;

		// Token: 0x04000479 RID: 1145
		public bool mLinearBlend;

		// Token: 0x0400047A RID: 1146
		public bool mIs3D;
	}
}
