using System;
using System.Collections.Generic;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace JeffLib
{
	// Token: 0x02000102 RID: 258
	public class Animator
	{
		// Token: 0x06000772 RID: 1906 RVA: 0x00026924 File Offset: 0x00024B24
		protected void UpdateFadeData()
		{
			if (this.mFadeData.mFadeState == 2)
			{
				if ((this.mFadeData.mVal += this.mFadeData.mFadeInRate) >= this.mFadeData.mFadeInTarget)
				{
					this.mFadeData.mVal = this.mFadeData.mFadeInTarget;
					if (this.mFadeData.mFadeCount > 0)
					{
						this.mFadeData.mFadeCount--;
					}
					this.mFadeData.mFadeState = 1;
				}
			}
			else if ((this.mFadeData.mVal -= this.mFadeData.mFadeOutRate) <= this.mFadeData.mFadeOutTarget)
			{
				this.mFadeData.mVal = this.mFadeData.mFadeOutTarget;
				if (this.mFadeData.mFadeCount > 0)
				{
					this.mFadeData.mFadeCount--;
				}
				this.mFadeData.mFadeState = 2;
			}
			if (this.mFadeData.mFadeCount == 0)
			{
				this.mFadeData.mFadeState = 0;
				if (this.mFadeData.mStopWhenDone)
				{
					this.mDone = true;
				}
			}
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00026A51 File Offset: 0x00024C51
		protected void _Init()
		{
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00026A53 File Offset: 0x00024C53
		public Animator()
		{
			this._Init();
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00026A82 File Offset: 0x00024C82
		public Animator(Animator a)
		{
			a.CopyTo(this);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00026AB4 File Offset: 0x00024CB4
		public virtual bool UpdateAnim(bool change_loop_count)
		{
			this.mUpdateCnt++;
			this.mFrameChanged = false;
			if (this.mTimeLimit > 0 && !this.mPaused && !this.mDone && ++this.mCurrentTime >= this.mTimeLimit)
			{
				this.mPaused = true;
				this.mDone = true;
			}
			if (!this.mPaused && !this.mDone)
			{
				int num;
				if (this.mPingPong)
				{
					num = (this.mAnimForward ? (-1) : 0);
				}
				else
				{
					num = ((this.mLoopDir > 0) ? 0 : (-1));
				}
				int num2 = this.mMaxFrames;
				int num3 = ((this.mFrameDelays.Count > 0 && this.mMaxFrames > 1) ? this.mFrameDelays[this.mCurrentFrame] : this.mFrameDelay);
				if (this.mLoopSubsection)
				{
					num = this.mLoopStart - 1;
					num2 = this.mLoopEnd + 1;
				}
				if (num3 != 0 && this.mUpdateCnt % num3 == 0 && (this.mNumIterations == 0 || this.mLoopCount <= this.mNumIterations))
				{
					this.mFrameChanged = true;
					if (!this.mPingPong || this.mDrawRandomly)
					{
						if (!this.mDrawRandomly)
						{
							if (this.mLoopDir >= 0)
							{
								if ((this.mCurrentFrame += this.mStepAmt) >= num2)
								{
									if (this.mLoopSubsection)
									{
										num = this.mLoopStart;
									}
									this.mCurrentFrame = num;
									if (change_loop_count)
									{
										this.mLoopCount++;
									}
									if (this.mStopWhenDone || (this.mLoopCount >= this.mNumIterations && this.mNumIterations > 0))
									{
										this.mPaused = true;
										this.mDone = true;
									}
								}
							}
							else if ((this.mCurrentFrame -= this.mStepAmt) <= num)
							{
								if (this.mLoopSubsection)
								{
									num2 = this.mLoopEnd;
								}
								this.mCurrentFrame = num2 - 1;
								if (change_loop_count)
								{
									this.mLoopCount++;
								}
								if (this.mStopWhenDone || (this.mLoopCount >= this.mNumIterations && this.mNumIterations > 0))
								{
									this.mPaused = true;
									this.mDone = true;
								}
							}
						}
						else
						{
							if (this.mRandomFrames.Count == 0)
							{
								if (change_loop_count)
								{
									this.mLoopCount++;
								}
								if (this.mStopWhenDone || (this.mLoopCount >= this.mNumIterations && this.mNumIterations > 0))
								{
									this.mPaused = true;
									this.mDone = true;
								}
								for (int i = 0; i < this.mMaxFrames; i++)
								{
									this.mRandomFrames.Add(i);
								}
							}
							this.mCurrentFrame = SexyFramework.Common.Rand() % this.mRandomFrames.Count;
							this.mRandomFrames.RemoveAt(this.mCurrentFrame);
						}
					}
					else if (!this.mAnimForward)
					{
						if ((this.mCurrentFrame += this.mStepAmt) >= num2)
						{
							if (change_loop_count)
							{
								this.mLoopCount++;
							}
							this.mCurrentFrame = num2 - 2;
							if (this.mMaxFrames == 1)
							{
								this.mCurrentFrame = 0;
							}
							this.mAnimForward = true;
							if (this.mLoopCount >= this.mNumIterations && this.mNumIterations > 0)
							{
								this.mPaused = (this.mDone = true);
							}
						}
					}
					else if (this.mAnimForward && (this.mCurrentFrame -= this.mStepAmt) <= num)
					{
						if (change_loop_count)
						{
							this.mLoopCount++;
						}
						if (this.mLoopSubsection)
						{
							this.mCurrentFrame = num + 2;
						}
						else
						{
							this.mCurrentFrame = ((num < 0) ? (num + 2) : (num + 1));
						}
						if (this.mMaxFrames == 1 && this.mCurrentFrame == 1)
						{
							this.mCurrentFrame--;
						}
						this.mAnimForward = false;
						if (this.mStopWhenDone || (this.mLoopCount >= this.mNumIterations && this.mNumIterations > 0))
						{
							this.mCurrentFrame = num;
							this.mPaused = true;
							this.mDone = true;
						}
					}
				}
				if (this.mFadeData.mFadeState != 0)
				{
					this.UpdateFadeData();
				}
			}
			if (this.mMaxFrames == 1)
			{
				this.mFrameChanged = false;
			}
			return this.mFrameChanged;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00026F01 File Offset: 0x00025101
		public virtual bool UpdateAnim()
		{
			return this.UpdateAnim(true);
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00026F0A File Offset: 0x0002510A
		public virtual void PauseAnim(bool pPause)
		{
			this.mPaused = pPause;
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00026F14 File Offset: 0x00025114
		public virtual void ResetAnim()
		{
			this.mPaused = false;
			this.mCurrentFrame = ((this.mLoopDir >= 0) ? 0 : (this.mMaxFrames - 1));
			this.mDone = false;
			this.mLoopCount = 0;
			this.mLoopStart = 0;
			this.mLoopEnd = this.mMaxFrames;
			this.mLoopSubsection = false;
			this.mCurrentTime = 0;
			this.mUpdateCnt = 0;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00026F78 File Offset: 0x00025178
		public virtual void Clear()
		{
			this.mPaused = true;
			this.mUpdateCnt = (this.mCurrentFrame = 0);
			this.mFrameDelay = 1;
			this.mMaxFrames = 1;
			this.mLoopDir = 1;
			this.mPingPong = false;
			this.mAnimForward = true;
			this.mLoopSubsection = false;
			this.mStopWhenDone = false;
			this.mDone = false;
			this.mFrameChanged = true;
			this.mLoopCount = (this.mNumIterations = 0);
			this.mStepAmt = 1;
			this.mImage = null;
			this.mXOff = (this.mYOff = 0f);
			this.mLoopStart = (this.mLoopEnd = 0);
			this.mId = -1;
			this.mPriority = 0;
			this.mResetOnStart = false;
			this.mCanRotate = false;
			this.mDrawAdditive = false;
			this.mDrawColorized = false;
			this.mDrawRandomly = false;
			this.mTimeLimit = (this.mCurrentTime = -1);
			this.mFadeData = new FadeData();
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0002706B File Offset: 0x0002526B
		public virtual void LoopSubsection(int pStartFrame, int pEndFrame)
		{
			this.mLoopSubsection = true;
			this.mLoopStart = pStartFrame;
			this.mLoopEnd = pEndFrame;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00027082 File Offset: 0x00025282
		public virtual void SetLoopDir(int pDir)
		{
			this.mLoopDir = pDir;
			if (pDir < 0)
			{
				this.mCurrentFrame = this.mMaxFrames - 1;
			}
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0002709D File Offset: 0x0002529D
		public virtual void SetTimeLimit(int t)
		{
			this.mTimeLimit = t;
			this.mCurrentTime = 0;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x000270B0 File Offset: 0x000252B0
		private void START_ADDITIVE(Graphics g, SexyColor val)
		{
			g.SetDrawMode(1);
			if (this.mFadeData.mFadeState == 0)
			{
				g.SetColor(val);
			}
			else
			{
				g.SetColor(val.mRed, val.mGreen, val.mBlue, this.mFadeData.mVal);
			}
			g.SetColorizeImages(true);
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00027107 File Offset: 0x00025307
		private void START_ADDITIVE(Graphics g)
		{
			g.SetDrawMode(0);
			g.SetColorizeImages(false);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00027118 File Offset: 0x00025318
		private void START_COLORIZED(Graphics g, SexyColor val)
		{
			g.SetColorizeImages(true);
			if (this.mFadeData.mFadeState == 0)
			{
				g.SetColor(val);
				return;
			}
			g.SetColor(val.mRed, val.mGreen, val.mBlue, this.mFadeData.mVal);
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00027167 File Offset: 0x00025367
		private void STOP_COLORIZED(Graphics g)
		{
			g.SetColorizeImages(false);
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00027170 File Offset: 0x00025370
		private void START_FADE(Graphics g)
		{
			if (!this.mDrawAdditive && !this.mDrawColorized && this.mFadeData.mFadeState != 0)
			{
				g.SetColor(255, 255, 255, this.mFadeData.mVal);
				g.SetColorizeImages(true);
			}
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x000271C1 File Offset: 0x000253C1
		private void STOP_FADE(Graphics g)
		{
			if (!this.mDrawAdditive && !this.mDrawColorized && this.mFadeData.mFadeState != 0)
			{
				g.SetColorizeImages(false);
			}
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x000271E7 File Offset: 0x000253E7
		private void SETUP_DRAWING(Graphics g)
		{
			this.START_FADE(g);
			if (this.mDrawAdditive)
			{
				this.START_ADDITIVE(g, this.mAdditiveColor);
				return;
			}
			if (this.mDrawColorized)
			{
				this.START_COLORIZED(g, this.mColorizeVal);
			}
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0002721B File Offset: 0x0002541B
		private void END_DRAWING(Graphics g)
		{
			this.STOP_FADE(g);
			if (this.mDrawAdditive)
			{
				this.START_ADDITIVE(g, this.mAdditiveColor);
				return;
			}
			if (this.mDrawColorized)
			{
				this.START_COLORIZED(g, this.mColorizeVal);
			}
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0002724F File Offset: 0x0002544F
		public virtual void DrawAdditively(SexyColor pColor)
		{
			this.StopDrawingColorized();
			this.mDrawAdditive = true;
			this.mAdditiveColor = pColor;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00027265 File Offset: 0x00025465
		public virtual void StopDrawingAdditively()
		{
			this.mDrawAdditive = false;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0002726E File Offset: 0x0002546E
		public virtual void DrawColorized(SexyColor pColor)
		{
			this.StopDrawingAdditively();
			this.mDrawColorized = true;
			this.mColorizeVal = pColor;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00027284 File Offset: 0x00025484
		public virtual void StopDrawingColorized()
		{
			this.mDrawColorized = false;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00027290 File Offset: 0x00025490
		public virtual void SetMaxFrames(int pMax)
		{
			this.mMaxFrames = pMax;
			this.mCurrentFrame = 0;
			if (this.mLoopDir < 0)
			{
				this.mCurrentFrame = this.mMaxFrames - 1;
			}
			this.mFrameDelays.Clear();
			for (int i = 0; i < this.mFrameDelays.Count; i++)
			{
				this.mFrameDelays.Add(this.mFrameDelay);
			}
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x000272F4 File Offset: 0x000254F4
		public virtual void SetImage(Image pImage)
		{
			this.mImage = null;
			this.mCurrentFrame = 0;
			if (pImage != null)
			{
				this.SetMaxFrames((pImage.mNumCols > pImage.mNumRows) ? pImage.mNumCols : pImage.mNumRows);
			}
			this.mImage = pImage;
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00027330 File Offset: 0x00025530
		public virtual void Draw(Graphics g, int pX, int pY)
		{
			if (this.mImage == null || this.IsPaused() || this.IsDone())
			{
				return;
			}
			this.SETUP_DRAWING(g);
			g.DrawImageCel(this.mImage, pX + (int)this.mXOff, pY + (int)this.mYOff, this.GetFrame());
			this.END_DRAWING(g);
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00027388 File Offset: 0x00025588
		public virtual void Draw(Graphics g, float pX, float pY, bool pSmooth)
		{
			if (this.mImage == null || this.IsPaused() || this.IsDone())
			{
				return;
			}
			this.SETUP_DRAWING(g);
			if (pSmooth)
			{
				int theX = 0;
				int theY = 0;
				if (this.mImage.mNumCols > this.mImage.mNumRows)
				{
					theX = this.GetFrame() * this.mImage.GetCelWidth();
					theY = 0;
				}
				else if (this.mImage.mNumRows > this.mImage.mNumCols)
				{
					theX = 0;
					theY = this.GetFrame() * this.mImage.GetCelHeight();
				}
				g.DrawImageF(this.mImage, pX + this.mXOff, pY + this.mYOff, new Rect(theX, theY, this.mImage.GetCelWidth(), this.mImage.GetCelHeight()));
			}
			else
			{
				g.DrawImageCel(this.mImage, (int)(pX + this.mXOff), (int)(pY + this.mYOff), this.GetFrame());
			}
			this.END_DRAWING(g);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00027484 File Offset: 0x00025684
		public virtual void DrawStretched(Graphics g, float pX, float pY, float pPct)
		{
			if (this.mImage == null || this.IsPaused() || this.IsDone())
			{
				return;
			}
			this.SETUP_DRAWING(g);
			float num = (float)this.mImage.GetCelWidth() * pPct;
			float num2 = (float)this.mImage.GetCelHeight() * pPct;
			Rect theDestRect = new Rect((int)(pX + this.mXOff), (int)(pY + this.mYOff), (int)num, (int)num2);
			g.DrawImageCel(this.mImage, theDestRect, this.GetFrame());
			this.END_DRAWING(g);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00027508 File Offset: 0x00025708
		public virtual void DrawStretched(Graphics g, float pX, float pY, int pWidth, int pHeight)
		{
			if (this.mImage == null || this.IsPaused() || this.IsDone())
			{
				return;
			}
			this.SETUP_DRAWING(g);
			Rect theDestRect = new Rect((int)(pX + this.mXOff), (int)(pY + this.mYOff), pWidth, pHeight);
			g.DrawImageCel(this.mImage, theDestRect, this.GetFrame());
			this.END_DRAWING(g);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0002756C File Offset: 0x0002576C
		public virtual void DrawRotated(Graphics g, float pX, float pY, float pAngle, bool pSmooth, float pCenterX, float pCenterY)
		{
			if (this.mImage == null || this.IsPaused() || this.IsDone())
			{
				return;
			}
			this.SETUP_DRAWING(g);
			if (pSmooth)
			{
				int theX = 0;
				int theY = 0;
				if (this.mImage.mNumCols > this.mImage.mNumRows)
				{
					theX = this.GetFrame() * this.mImage.GetCelWidth();
					theY = 0;
				}
				else if (this.mImage.mNumRows > this.mImage.mNumCols)
				{
					theX = 0;
					theY = this.GetFrame() * this.mImage.GetCelHeight();
				}
				Rect theSrcRect = new Rect(theX, theY, this.mImage.GetCelWidth(), this.mImage.GetCelHeight());
				g.DrawImageRotatedF(this.mImage, pX + this.mXOff, pY + this.mYOff, (double)pAngle, pCenterX, pCenterY, theSrcRect);
			}
			else
			{
				Rect celRect = this.mImage.GetCelRect(this.GetFrame());
				g.DrawImageRotated(this.mImage, (int)(pX + this.mXOff), (int)(pY + this.mYOff), (double)pAngle, (int)pCenterX, (int)pCenterY, celRect);
			}
			this.END_DRAWING(g);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00027688 File Offset: 0x00025888
		public virtual void DrawRandomly(bool pRandom)
		{
			this.mDrawRandomly = pRandom;
			if (this.mDrawRandomly)
			{
				this.mRandomFrames.Clear();
				for (int i = 0; i < this.mMaxFrames; i++)
				{
					this.mRandomFrames.Add(i);
				}
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x000276CC File Offset: 0x000258CC
		public virtual void SetNumIterations(int aIt)
		{
			this.mNumIterations = aIt;
			this.mLoopCount = 0;
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x000276DC File Offset: 0x000258DC
		public virtual void SetFrame(int pFrame)
		{
			this.mCurrentFrame = pFrame;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x000276E8 File Offset: 0x000258E8
		public virtual void SetDelay(int pDelay)
		{
			this.mFrameDelay = pDelay;
			if (this.mFrameDelays.Count > 0)
			{
				for (int i = 0; i < this.mFrameDelays.Count; i++)
				{
					this.mFrameDelays[i] = pDelay;
				}
			}
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0002772D File Offset: 0x0002592D
		public virtual void SetDelay(int pDelay, int pFrame)
		{
			this.mFrameDelays[pFrame] = pDelay;
			this.mFrameDelay = pDelay;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00027743 File Offset: 0x00025943
		public virtual bool FrameChanged()
		{
			if (this.mFrameChanged)
			{
				this.mFrameChanged = false;
				return true;
			}
			return false;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00027758 File Offset: 0x00025958
		public virtual void CopyTo(Animator rhs)
		{
			if (this == rhs)
			{
				return;
			}
			this.mImage = null;
			if (rhs.mFadeData == null)
			{
				this.mFadeData = null;
			}
			else
			{
				this.mFadeData = new FadeData(rhs.mFadeData);
			}
			this.mTimeLimit = rhs.mTimeLimit;
			this.mCurrentTime = rhs.mCurrentTime;
			this.mUpdateCnt = rhs.mUpdateCnt;
			this.mAnimForward = rhs.mAnimForward;
			this.mDone = rhs.mDone;
			this.mFrameChanged = rhs.mFrameChanged;
			this.mFrameDelay = rhs.mFrameDelay;
			this.SetMaxFrames(rhs.mMaxFrames);
			this.mPaused = rhs.mPaused;
			this.mPingPong = rhs.mPingPong;
			this.mStopWhenDone = rhs.mStopWhenDone;
			if (rhs.mImage != null)
			{
				this.SetImage(rhs.mImage);
			}
			this.mNumIterations = rhs.mNumIterations;
			this.mLoopCount = rhs.mLoopCount;
			this.mLoopStart = rhs.mLoopStart;
			this.mLoopEnd = rhs.mLoopEnd;
			this.mLoopDir = rhs.mLoopDir;
			this.mStepAmt = rhs.mStepAmt;
			this.mLoopSubsection = rhs.mLoopSubsection;
			this.mXOff = rhs.mXOff;
			this.mYOff = rhs.mYOff;
			this.mPriority = rhs.mPriority;
			this.mId = rhs.mId;
			this.mDrawAdditive = rhs.mDrawAdditive;
			this.mAdditiveColor = rhs.mAdditiveColor;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x000278C7 File Offset: 0x00025AC7
		public bool IsDone()
		{
			return this.mDone || (this.mNumIterations > 0 && this.mLoopCount >= this.mNumIterations);
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x000278EF File Offset: 0x00025AEF
		public bool IsPaused()
		{
			return this.mPaused;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x000278F7 File Offset: 0x00025AF7
		public bool PingPongs()
		{
			return this.mPingPong;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x000278FF File Offset: 0x00025AFF
		public bool IsPlaying()
		{
			return !this.IsDone() && !this.IsPaused();
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x00027914 File Offset: 0x00025B14
		public bool StopWhenDone()
		{
			return this.mStopWhenDone;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0002791C File Offset: 0x00025B1C
		public int GetFrame()
		{
			if (this.mMaxFrames != 1)
			{
				return this.mCurrentFrame;
			}
			return 0;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0002792F File Offset: 0x00025B2F
		public int GetMaxFrames()
		{
			return this.mMaxFrames;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00027937 File Offset: 0x00025B37
		public int GetDelay()
		{
			return this.mFrameDelay;
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0002793F File Offset: 0x00025B3F
		public int GetStepAmt()
		{
			return this.mStepAmt;
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x00027947 File Offset: 0x00025B47
		public int GetId()
		{
			return this.mId;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0002794F File Offset: 0x00025B4F
		public int GetPriority()
		{
			return this.mPriority;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00027957 File Offset: 0x00025B57
		public int GetTimeLimit()
		{
			return this.mTimeLimit;
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0002795F File Offset: 0x00025B5F
		public int GetLoopStart()
		{
			return this.mLoopStart;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00027967 File Offset: 0x00025B67
		public int GetLoopEnd()
		{
			return this.mLoopEnd;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0002796F File Offset: 0x00025B6F
		public float GetXOff()
		{
			return this.mXOff;
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x00027977 File Offset: 0x00025B77
		public float GetYOff()
		{
			return this.mYOff;
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0002797F File Offset: 0x00025B7F
		public Image GetImage()
		{
			return this.mImage;
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00027987 File Offset: 0x00025B87
		public void SetPingPong(bool pPong)
		{
			this.mPingPong = pPong;
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00027990 File Offset: 0x00025B90
		public void StopWhenDone(bool pStop)
		{
			this.mStopWhenDone = pStop;
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00027999 File Offset: 0x00025B99
		public void SetStepAmount(int pStep)
		{
			this.mStepAmt = pStep;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x000279A2 File Offset: 0x00025BA2
		public void SetXOffset(float x)
		{
			this.mXOff = x;
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x000279AB File Offset: 0x00025BAB
		public void SetYOffset(float y)
		{
			this.mYOff = y;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x000279B4 File Offset: 0x00025BB4
		public void SetXYOffset(float x, float y)
		{
			this.SetXOffset(x);
			this.SetYOffset(y);
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x000279C4 File Offset: 0x00025BC4
		public void SetId(int id)
		{
			this.mId = id;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x000279CD File Offset: 0x00025BCD
		public void SetPriority(int p)
		{
			this.mPriority = p;
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x000279D6 File Offset: 0x00025BD6
		public void SetDone()
		{
			this.mDone = true;
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x000279DF File Offset: 0x00025BDF
		public void ResetTime()
		{
			this.mCurrentTime = 0;
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x000279E8 File Offset: 0x00025BE8
		public FadeData GetFadeData()
		{
			return this.mFadeData;
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x000279F0 File Offset: 0x00025BF0
		public int GetFadeOutRate()
		{
			return this.mFadeData.mFadeOutRate;
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x000279FD File Offset: 0x00025BFD
		public int GetFadeOutTarget()
		{
			return this.mFadeData.mFadeOutTarget;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00027A0A File Offset: 0x00025C0A
		public int GetFadeInRate()
		{
			return this.mFadeData.mFadeInRate;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00027A17 File Offset: 0x00025C17
		public int GetFadeInTarget()
		{
			return this.mFadeData.mFadeInTarget;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00027A24 File Offset: 0x00025C24
		public int GetFadeVal()
		{
			return this.mFadeData.mVal;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00027A31 File Offset: 0x00025C31
		public int GetFadeCount()
		{
			return this.mFadeData.mFadeCount;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00027A3E File Offset: 0x00025C3E
		public bool GetFadeStopWhenDone()
		{
			return this.mFadeData.mStopWhenDone;
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x00027A4B File Offset: 0x00025C4B
		public void SetFadeData(FadeData fd)
		{
			this.mFadeData = fd;
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x00027A54 File Offset: 0x00025C54
		public void SetFadeOutRate(int r)
		{
			this.mFadeData.mFadeOutRate = r;
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x00027A62 File Offset: 0x00025C62
		public void SetFadeOutTarget(int t)
		{
			this.mFadeData.mFadeOutTarget = t;
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00027A70 File Offset: 0x00025C70
		public void SetFadeInRate(int r)
		{
			this.mFadeData.mFadeInRate = r;
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00027A7E File Offset: 0x00025C7E
		public void SetFadeInTarget(int t)
		{
			this.mFadeData.mFadeInTarget = t;
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x00027A8C File Offset: 0x00025C8C
		public void SetFadeVal(int v)
		{
			this.mFadeData.mVal = v;
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00027A9A File Offset: 0x00025C9A
		public void SetFadeCount(int c)
		{
			this.mFadeData.mFadeCount = c;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00027AA8 File Offset: 0x00025CA8
		public void SetFadeStopWhenDone(bool d)
		{
			this.mFadeData.mStopWhenDone = d;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00027AB6 File Offset: 0x00025CB6
		public void FadeIn()
		{
			this.mFadeData.mFadeState = 2;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x00027AC4 File Offset: 0x00025CC4
		public void FadeIn(int rate, int target)
		{
			this.FadeIn();
			this.SetFadeInRate(rate);
			this.SetFadeInTarget(target);
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00027ADA File Offset: 0x00025CDA
		public void FadeOut()
		{
			this.mFadeData.mFadeState = 1;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00027AE8 File Offset: 0x00025CE8
		public void FadeOut(int rate, int target)
		{
			this.FadeOut();
			this.SetFadeOutRate(rate);
			this.SetFadeOutTarget(target);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00027AFE File Offset: 0x00025CFE
		public void StopFading()
		{
			this.mFadeData.mFadeState = 0;
		}

		// Token: 0x0400070A RID: 1802
		protected int mCurrentFrame;

		// Token: 0x0400070B RID: 1803
		protected int mMaxFrames;

		// Token: 0x0400070C RID: 1804
		protected int mFrameDelay;

		// Token: 0x0400070D RID: 1805
		protected int mNumIterations;

		// Token: 0x0400070E RID: 1806
		protected int mLoopCount;

		// Token: 0x0400070F RID: 1807
		protected int mLoopStart;

		// Token: 0x04000710 RID: 1808
		protected int mLoopEnd;

		// Token: 0x04000711 RID: 1809
		protected int mLoopDir;

		// Token: 0x04000712 RID: 1810
		protected int mStepAmt;

		// Token: 0x04000713 RID: 1811
		protected int mId;

		// Token: 0x04000714 RID: 1812
		protected int mPriority;

		// Token: 0x04000715 RID: 1813
		protected int mTimeLimit;

		// Token: 0x04000716 RID: 1814
		protected int mCurrentTime;

		// Token: 0x04000717 RID: 1815
		protected float mXOff;

		// Token: 0x04000718 RID: 1816
		protected float mYOff;

		// Token: 0x04000719 RID: 1817
		protected bool mPaused;

		// Token: 0x0400071A RID: 1818
		protected bool mPingPong;

		// Token: 0x0400071B RID: 1819
		protected bool mAnimForward;

		// Token: 0x0400071C RID: 1820
		protected bool mStopWhenDone;

		// Token: 0x0400071D RID: 1821
		protected bool mDone;

		// Token: 0x0400071E RID: 1822
		protected bool mFrameChanged;

		// Token: 0x0400071F RID: 1823
		protected bool mLoopSubsection;

		// Token: 0x04000720 RID: 1824
		protected bool mDrawAdditive;

		// Token: 0x04000721 RID: 1825
		protected bool mDrawColorized;

		// Token: 0x04000722 RID: 1826
		protected bool mDrawRandomly;

		// Token: 0x04000723 RID: 1827
		protected SexyColor mAdditiveColor;

		// Token: 0x04000724 RID: 1828
		protected SexyColor mColorizeVal;

		// Token: 0x04000725 RID: 1829
		protected Image mImage;

		// Token: 0x04000726 RID: 1830
		protected FadeData mFadeData = new FadeData();

		// Token: 0x04000727 RID: 1831
		protected List<int> mFrameDelays = new List<int>();

		// Token: 0x04000728 RID: 1832
		protected List<int> mRandomFrames = new List<int>();

		// Token: 0x04000729 RID: 1833
		public int mUpdateCnt;

		// Token: 0x0400072A RID: 1834
		public bool mCanRotate;

		// Token: 0x0400072B RID: 1835
		public bool mResetOnStart;
	}
}
