using System;
using System.Collections.Generic;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Sound;
using SexyFramework.WidgetsLib;

namespace JeffLib
{
	// Token: 0x0200011F RID: 287
	public class ExtraSexyButton : ButtonWidget
	{
		// Token: 0x06000900 RID: 2304 RVA: 0x0002E1D8 File Offset: 0x0002C3D8
		public ExtraSexyButton(int theId, ButtonListener theButtonListener)
			: base(theId, theButtonListener)
		{
			this.mUsesAnimators = true;
			this.mGIFMaskIgnoreColor = 0;
			this.mGIFMask = null;
			this.mDraw = true;
			this.mBlink = false;
			this.mSyncFrames = false;
			this.mStopExcludedSounds = true;
			this.mButtonAnimation.PauseAnim(false);
			this.mButtonAnimation.SetPingPong(false);
			this.mButtonAnimation.SetDelay(10);
			this.mButtonAnimation.SetMaxFrames(1);
			this.mButtonAnimation.StopWhenDone(false);
			this.mOverAnimation.PauseAnim(true);
			this.mOverAnimation.SetPingPong(false);
			this.mOverAnimation.SetDelay(10);
			this.mOverAnimation.SetMaxFrames(1);
			this.mOverAnimation.StopWhenDone(false);
			this.mDownAnimation.PauseAnim(true);
			this.mDownAnimation.SetPingPong(false);
			this.mDownAnimation.SetDelay(10);
			this.mDownAnimation.SetMaxFrames(1);
			this.mDownAnimation.StopWhenDone(false);
			this.mMouseOverSnd = null;
			this.mMaskWidth = 0;
			this.mMouseOverSndID = -1;
			this.mDownImage = null;
			this.mOverImage = null;
			this.mButtonImage = null;
			this.mDoFinger = true;
			this.mPitchShift = int.MaxValue;
			this.mAdditiveDown = (this.mAdditiveOver = false);
			this.mOverColor = (this.mDownColor = new SexyColor(0, 0, 0, 0));
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0002E37D File Offset: 0x0002C57D
		public override void Dispose()
		{
			base.Dispose();
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0002E388 File Offset: 0x0002C588
		public override bool IsPointVisible(int pX, int pY)
		{
			if (pX >= this.mWidth || pY >= this.mHeight || pX < 0 || pY < 0)
			{
				return false;
			}
			if (this.mMask == null && this.mGIFMask == null)
			{
				return true;
			}
			if (this.mMask != null)
			{
				int num = pY * this.mMaskWidth + pX;
				uint num2 = this.mMask[num];
				if (num2 == 4294967295U)
				{
					return true;
				}
			}
			else if (this.mGIFMask != null)
			{
				int num3 = pY * this.mGIFMask.mWidth + pX;
				uint num4;
				if (this.mGIFMask.mColorIndices != null)
				{
					byte b = this.mGIFMask.mColorIndices[num3];
					num4 = this.mGIFMask.mColorTable[(int)b];
				}
				else
				{
					num4 = this.mGIFMask.GetBits()[num3];
				}
				if ((ulong)num4 != (ulong)((long)this.mGIFMaskIgnoreColor))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0002E448 File Offset: 0x0002C648
		public override void MouseEnter()
		{
			base.MouseEnter();
			if (this.mButtonListener != null)
			{
				this.mButtonListener.ButtonMouseEnter(this.mId);
			}
			if (this.mMouseOverSnd != null && !this.mMouseOverSnd.IsPlaying())
			{
				bool flag = true;
				for (int i = 0; i < this.mMouseOverExclusionList.Count; i++)
				{
					if (this.mMouseOverExclusionList[i].IsPlaying())
					{
						if (!this.mStopExcludedSounds)
						{
							flag = false;
							break;
						}
						this.mMouseOverExclusionList[i].Stop();
					}
				}
				if (flag || this.mStopExcludedSounds)
				{
					if (this.mPitchShift != 2147483647)
					{
						this.mMouseOverSnd.AdjustPitch((double)this.mPitchShift);
					}
					this.mMouseOverSnd.Play(false, false);
				}
			}
			else if (this.mMouseOverSndID != -1)
			{
				if (this.mPitchShift != 2147483647)
				{
					SoundInstance soundInstance = GlobalMembers.gSexyAppBase.mSoundManager.GetSoundInstance(this.mMouseOverSndID);
					if (soundInstance != null)
					{
						soundInstance.AdjustPitch((double)this.mPitchShift);
						soundInstance.Play(false, true);
					}
				}
				else
				{
					GlobalMembers.gSexyApp.PlaySample(this.mMouseOverSndID);
				}
			}
			if (this.mOverAnimation.IsPaused() && !this.mIsDown)
			{
				this.mOverAnimation.ResetAnim();
				this.mOverAnimation.PauseAnim(false);
				if (this.mSyncFrames && this.mOverAnimation.GetMaxFrames() > this.mButtonAnimation.GetFrame())
				{
					this.mOverAnimation.mUpdateCnt = this.mButtonAnimation.mUpdateCnt;
					this.mOverAnimation.SetFrame(this.mButtonAnimation.GetFrame());
				}
				this.mButtonAnimation.PauseAnim(true);
				return;
			}
			if (this.mIsDown && this.mDownAnimation.IsPaused())
			{
				if (this.mDownImage != null)
				{
					this.mOverAnimation.PauseAnim(true);
				}
				else
				{
					this.mOverAnimation.ResetAnim();
					this.mOverAnimation.PauseAnim(false);
					if (this.mSyncFrames && this.mOverAnimation.GetMaxFrames() > this.mButtonAnimation.GetFrame())
					{
						this.mOverAnimation.mUpdateCnt = this.mButtonAnimation.mUpdateCnt;
						this.mOverAnimation.SetFrame(this.mButtonAnimation.GetFrame());
					}
				}
				this.mButtonAnimation.PauseAnim(true);
				this.mDownAnimation.ResetAnim();
				this.mDownAnimation.PauseAnim(false);
				if (this.mSyncFrames && this.mDownAnimation.GetMaxFrames() > this.mOverAnimation.GetFrame())
				{
					this.mDownAnimation.mUpdateCnt = this.mButtonAnimation.mUpdateCnt;
					this.mDownAnimation.SetFrame(this.mButtonAnimation.GetFrame());
				}
			}
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0002E6F8 File Offset: 0x0002C8F8
		public override void MouseLeave()
		{
			base.MouseLeave();
			if (this.mButtonListener != null)
			{
				this.mButtonListener.ButtonMouseLeave(this.mId);
			}
			Animator animator = null;
			if (!this.mOverAnimation.IsPaused())
			{
				animator = this.mOverAnimation;
			}
			else if (!this.mDownAnimation.IsPaused())
			{
				animator = this.mDownAnimation;
			}
			this.mOverAnimation.PauseAnim(true);
			this.mDownAnimation.PauseAnim(true);
			this.mButtonAnimation.ResetAnim();
			this.mButtonAnimation.PauseAnim(false);
			if (this.mSyncFrames && this.mButtonAnimation.GetMaxFrames() > animator.GetFrame())
			{
				this.mButtonAnimation.mUpdateCnt = animator.mUpdateCnt;
				this.mButtonAnimation.SetFrame(animator.GetFrame());
			}
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0002E7BC File Offset: 0x0002C9BC
		public override void MouseDown(int pX, int pY, int pClickCount)
		{
			base.MouseDown(pX, pY, pClickCount);
			if (this.mDownImage != null)
			{
				this.mOverAnimation.PauseAnim(true);
			}
			else
			{
				this.mOverAnimation.ResetAnim();
				this.mOverAnimation.PauseAnim(false);
				if (this.mSyncFrames && this.mOverAnimation.GetMaxFrames() > this.mButtonAnimation.GetFrame())
				{
					this.mOverAnimation.mUpdateCnt = this.mButtonAnimation.mUpdateCnt;
					this.mOverAnimation.SetFrame(this.mButtonAnimation.GetFrame());
				}
			}
			this.mButtonAnimation.PauseAnim(true);
			this.mDownAnimation.ResetAnim();
			this.mDownAnimation.PauseAnim(false);
			if (this.mSyncFrames && this.mDownAnimation.GetMaxFrames() > this.mOverAnimation.GetFrame())
			{
				this.mDownAnimation.mUpdateCnt = this.mOverAnimation.mUpdateCnt;
				this.mDownAnimation.SetFrame(this.mOverAnimation.GetFrame());
			}
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0002E8BC File Offset: 0x0002CABC
		public override void MouseUp(int pX, int pY)
		{
			base.MouseUp(pX, pY);
			if (this.mIsOver)
			{
				Animator animator = null;
				if (!this.mButtonAnimation.IsPaused())
				{
					animator = this.mButtonAnimation;
				}
				else if (!this.mDownAnimation.IsPaused())
				{
					animator = this.mDownAnimation;
				}
				this.mDownAnimation.PauseAnim(true);
				this.mButtonAnimation.PauseAnim(true);
				this.mOverAnimation.ResetAnim();
				this.mOverAnimation.PauseAnim(false);
				if (this.mSyncFrames && animator != null && this.mOverAnimation.GetMaxFrames() > animator.GetFrame())
				{
					this.mOverAnimation.mUpdateCnt = animator.mUpdateCnt;
					this.mOverAnimation.SetFrame(animator.GetFrame());
				}
			}
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0002E978 File Offset: 0x0002CB78
		public override void Update()
		{
			base.Update();
			if (this.mDisabled || !GlobalMembers.gSexyApp.mHasFocus)
			{
				return;
			}
			this.mButtonAnimation.UpdateAnim();
			this.mOverAnimation.UpdateAnim();
			this.mDownAnimation.UpdateAnim();
			if (this.mButtonAnimation.FrameChanged() || this.mOverAnimation.FrameChanged() || this.mDownAnimation.FrameChanged() || this.mBlink)
			{
				this.MarkDirty();
			}
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0002E9FC File Offset: 0x0002CBFC
		public override void Draw(Graphics pGfx)
		{
			if (!this.mDraw)
			{
				return;
			}
			if (!this.mUsesAnimators)
			{
				base.Draw(pGfx);
				return;
			}
			int theX = 0;
			int theY = 0;
			if (this.mFont != null)
			{
				theX = (this.mWidth - this.mFont.StringWidth(this.mLabel)) / 2;
				theY = (this.mHeight + this.mFont.GetAscent() - this.mFont.GetAscent() / 6 - 1) / 2;
			}
			Image image = ((this.mOverImage == null) ? this.mButtonImage : this.mOverImage);
			if (this.mDisabled)
			{
				base.Draw(pGfx);
			}
			else if (!this.mButtonAnimation.IsPaused() && this.mButtonImage != null)
			{
				pGfx.DrawImageCel(this.mButtonImage, 0, 0, this.mButtonAnimation.GetFrame());
			}
			else if (!this.mDownAnimation.IsPaused() && this.mDownImage != null)
			{
				if (!this.mBlink && this.mAdditiveDown)
				{
					pGfx.SetDrawMode(0);
					pGfx.SetColorizeImages(true);
					pGfx.SetColor(this.mDownColor);
				}
				pGfx.DrawImageCel(this.mDownImage, 0, 0, this.mDownAnimation.GetFrame());
				if (!this.mBlink && this.mAdditiveDown)
				{
					pGfx.SetDrawMode(0);
					pGfx.SetColorizeImages(false);
				}
			}
			else if (!this.mDownAnimation.IsPaused() && this.mDownImage == null)
			{
				if (image != null)
				{
					if (!this.mBlink && this.mAdditiveDown)
					{
						pGfx.SetDrawMode(1);
						pGfx.SetColorizeImages(true);
						pGfx.SetColor(this.mDownColor);
					}
					pGfx.DrawImageCel(image, 1, 1, this.mOverAnimation.GetFrame());
					if (!this.mBlink && this.mAdditiveDown)
					{
						pGfx.SetDrawMode(0);
						pGfx.SetColorizeImages(false);
					}
				}
			}
			else if (!this.mOverAnimation.IsPaused() && image != null)
			{
				if (!this.mBlink && this.mAdditiveOver)
				{
					pGfx.SetDrawMode(1);
					pGfx.SetColorizeImages(true);
					pGfx.SetColor(this.mOverColor);
				}
				pGfx.DrawImageCel(image, 0, 0, this.mOverAnimation.GetFrame());
				if (!this.mBlink && this.mAdditiveOver)
				{
					pGfx.SetDrawMode(0);
					pGfx.SetColorizeImages(false);
				}
			}
			if (this.mBlink && !this.mIsOver)
			{
				this.mIsOver = true;
				int num = this.mUpdateCnt % 100;
				if (num > 50)
				{
					num = 100 - num;
				}
				pGfx.SetColor(255, 255, 255, 255 * num / 50);
				pGfx.SetColorizeImages(true);
				pGfx.SetDrawMode(1);
				if (this.mDisabled)
				{
					base.Draw(pGfx);
				}
				else if (!this.mButtonAnimation.IsPaused() && this.mButtonImage != null)
				{
					pGfx.DrawImageCel(this.mButtonImage, 0, 0, this.mButtonAnimation.GetFrame());
				}
				else if (!this.mButtonAnimation.IsPaused() && this.mButtonImage == null && this.mOverImage != null)
				{
					num = this.mUpdateCnt % 254;
					if (num > 127)
					{
						num = 254 - num;
					}
					pGfx.SetColor(255, 255, 255, num);
					pGfx.DrawImageCel(this.mOverImage, 0, 0, 0);
				}
				else if (!this.mDownAnimation.IsPaused() && this.mDownImage != null)
				{
					pGfx.DrawImageCel(this.mDownImage, 0, 0, this.mDownAnimation.GetFrame());
				}
				else if (!this.mDownAnimation.IsPaused() && this.mDownImage == null && image != null)
				{
					pGfx.DrawImageCel(image, 1, 1, this.mOverAnimation.GetFrame());
				}
				else if (!this.mOverAnimation.IsPaused() && image != null)
				{
					pGfx.DrawImageCel(image, 0, 0, this.mOverAnimation.GetFrame());
				}
				pGfx.SetDrawMode(0);
				pGfx.SetColorizeImages(false);
				this.mIsOver = false;
			}
			if (this.mFont != null)
			{
				if (this.mIsOver)
				{
					pGfx.SetColor(this.mColors[1]);
				}
				else
				{
					pGfx.SetColor(this.mColors[0]);
				}
				pGfx.SetFont(this.mFont);
				pGfx.DrawString(this.mLabel, theX, theY);
			}
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0002EE22 File Offset: 0x0002D022
		public void DrawBoundingBox(Graphics pGfx)
		{
			pGfx.SetColor(255, 255, 255, 128);
			pGfx.DrawRect(0, 0, this.mWidth, this.mHeight);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0002EE52 File Offset: 0x0002D052
		public void SetMask(uint[] pMask, int pWidth)
		{
			this.mMask = pMask;
			this.mMaskWidth = pWidth;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0002EE62 File Offset: 0x0002D062
		public void SetMask(MemoryImage gif_mask, int ignore_color)
		{
			this.mGIFMask = gif_mask;
			this.mGIFMaskIgnoreColor = ignore_color;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0002EE72 File Offset: 0x0002D072
		public void SetDraw(bool pDraw)
		{
			this.mDraw = pDraw;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0002EE7B File Offset: 0x0002D07B
		public void SetStopExcludedSounds(bool pS)
		{
			this.mStopExcludedSounds = pS;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0002EE84 File Offset: 0x0002D084
		public void SetBlink(bool pBlink)
		{
			this.mBlink = pBlink;
		}

		// Token: 0x04000824 RID: 2084
		protected uint[] mMask;

		// Token: 0x04000825 RID: 2085
		protected MemoryImage mGIFMask;

		// Token: 0x04000826 RID: 2086
		protected int mGIFMaskIgnoreColor;

		// Token: 0x04000827 RID: 2087
		protected int mMaskWidth;

		// Token: 0x04000828 RID: 2088
		protected bool mDraw;

		// Token: 0x04000829 RID: 2089
		protected bool mStopExcludedSounds;

		// Token: 0x0400082A RID: 2090
		protected bool mBlink;

		// Token: 0x0400082B RID: 2091
		protected List<SoundInstance> mMouseOverExclusionList = new List<SoundInstance>();

		// Token: 0x0400082C RID: 2092
		public Animator mOverAnimation = new Animator();

		// Token: 0x0400082D RID: 2093
		public Animator mDownAnimation = new Animator();

		// Token: 0x0400082E RID: 2094
		public Animator mButtonAnimation = new Animator();

		// Token: 0x0400082F RID: 2095
		public SexyColor mOverColor = default(SexyColor);

		// Token: 0x04000830 RID: 2096
		public SexyColor mDownColor = default(SexyColor);

		// Token: 0x04000831 RID: 2097
		public SoundInstance mMouseOverSnd;

		// Token: 0x04000832 RID: 2098
		public int mMouseOverSndID;

		// Token: 0x04000833 RID: 2099
		public int mPitchShift;

		// Token: 0x04000834 RID: 2100
		public bool mAdditiveOver;

		// Token: 0x04000835 RID: 2101
		public bool mAdditiveDown;

		// Token: 0x04000836 RID: 2102
		public bool mSyncFrames;

		// Token: 0x04000837 RID: 2103
		public bool mUsesAnimators;
	}
}
