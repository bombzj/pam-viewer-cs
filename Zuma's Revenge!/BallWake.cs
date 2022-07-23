using System;
using System.Collections.Generic;
using System.Linq;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x02000055 RID: 85
	public class BallWake : Effect
	{
		// Token: 0x060006E0 RID: 1760 RVA: 0x0002E639 File Offset: 0x0002C839
		private void DebugCheck()
		{
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0002E63B File Offset: 0x0002C83B
		public BallWake()
		{
			this.mResGroup = "Underwater";
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0002E664 File Offset: 0x0002C864
		public override void Reset(string level_id)
		{
			base.Reset(level_id);
			this.mWake.Clear();
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0002E678 File Offset: 0x0002C878
		public override void Update()
		{
			List<WakeStruct> list = new List<WakeStruct>();
			for (int i = 0; i < Enumerable.Count<WakeStruct>(this.mWake); i++)
			{
				WakeStruct wakeStruct = this.mWake[i];
				wakeStruct.mUpdateCount++;
				wakeStruct.mX += wakeStruct.mVel.x;
				wakeStruct.mY += wakeStruct.mVel.y;
				int num = (int)((float)wakeStruct.mImage.mWidth * wakeStruct.mSize);
				int num2 = (int)((float)wakeStruct.mImage.mHeight * wakeStruct.mSize);
				Rect rect = new Rect((int)(wakeStruct.mX - (float)(num / 2)), (int)(wakeStruct.mY - (float)(num2 / 2)), num, num2);
				if (!rect.Intersects(new Rect(0, 0, Common._SS(GlobalMembers.gSexyApp.mWidth), Common._SS(GlobalMembers.gSexyApp.mHeight))))
				{
					this.mWake.RemoveAt(i);
					i--;
				}
				else if (wakeStruct.mExpanding || !wakeStruct.mIsHead)
				{
					if (wakeStruct.mIsHead)
					{
						WakeStruct wakeStruct2 = wakeStruct;
						wakeStruct2.mVel.x = wakeStruct2.mVel.x - wakeStruct.mVel.x / Common._M(2f);
						WakeStruct wakeStruct3 = wakeStruct;
						wakeStruct3.mVel.y = wakeStruct3.mVel.y - wakeStruct.mVel.y / Common._M(2f);
					}
					if (!wakeStruct.mIsHead)
					{
						if (wakeStruct.mAlphaInc > 0f)
						{
							wakeStruct.mAlpha += wakeStruct.mAlphaInc;
							float num3 = Common._M(255f);
							if (wakeStruct.mAlpha >= num3)
							{
								wakeStruct.mAlpha = num3;
								wakeStruct.mAlphaInc = 0f;
							}
						}
						else
						{
							wakeStruct.mAlpha -= Common._M(35f);
						}
					}
					else
					{
						wakeStruct.mAlpha -= Common._M(40f);
					}
					if (wakeStruct.mAlpha < 0f)
					{
						this.mWake.RemoveAt(i);
						i--;
					}
					else
					{
						wakeStruct.mSize += Common._M(0.03f);
					}
				}
				else if (wakeStruct.mIsHead && !wakeStruct.mExpanding && wakeStruct.mUpdateCount % Common._M(1) == 0)
				{
					WakeStruct wakeStruct4 = new WakeStruct();
					float t = Common._M(4f);
					float mAlphaInc = Common._M(40f);
					wakeStruct4.mBallId = wakeStruct.mBallId;
					wakeStruct4.mImage = Res.GetImageByID(ResID.IMAGE_FX_UNDERWATER_SIDEWAKE_DARK);
					wakeStruct4.mVel = wakeStruct.mVel.Perp() / t;
					SexyVector2 sexyVector = wakeStruct4.mVel.Normalize();
					wakeStruct4.mAngle = wakeStruct.mAngle;
					wakeStruct4.mX = wakeStruct.mX + Common._M(40f) * sexyVector.x;
					wakeStruct4.mY = wakeStruct.mY + Common._M(40f) * sexyVector.y;
					wakeStruct4.mAlpha = 0f;
					wakeStruct4.mAlphaInc = mAlphaInc;
					list.Add(wakeStruct4);
					WakeStruct wakeStruct5 = new WakeStruct();
					wakeStruct5.mBallId = wakeStruct.mBallId;
					wakeStruct5.mImage = Res.GetImageByID(ResID.IMAGE_FX_UNDERWATER_SIDEWAKE_LIGHT);
					wakeStruct5.mAdditive = true;
					wakeStruct5.mVel = wakeStruct.mVel.Perp() / t;
					sexyVector = wakeStruct5.mVel.Normalize();
					wakeStruct5.mAngle = wakeStruct.mAngle;
					wakeStruct5.mX = wakeStruct.mX + Common._M(15f) * sexyVector.x;
					wakeStruct5.mY = wakeStruct.mY + Common._M(15f) * sexyVector.y;
					wakeStruct5.mAlpha = 0f;
					wakeStruct5.mAlphaInc = mAlphaInc;
					list.Add(wakeStruct5);
					WakeStruct wakeStruct6 = new WakeStruct();
					wakeStruct6.mBallId = wakeStruct.mBallId;
					wakeStruct6.mImage = Res.GetImageByID(ResID.IMAGE_FX_UNDERWATER_SIDEWAKE_DARK);
					wakeStruct6.mVel = -wakeStruct.mVel.Perp() / t;
					sexyVector = wakeStruct6.mVel.Normalize();
					wakeStruct6.mAngle = wakeStruct.mAngle;
					wakeStruct6.mX = wakeStruct.mX + (float)Common._M(40) * sexyVector.x;
					wakeStruct6.mY = wakeStruct.mY + (float)Common._M(40) * sexyVector.y;
					wakeStruct6.mAlpha = 0f;
					wakeStruct6.mAlphaInc = mAlphaInc;
					list.Add(wakeStruct6);
					WakeStruct wakeStruct7 = new WakeStruct();
					wakeStruct7.mBallId = wakeStruct.mBallId;
					wakeStruct7.mImage = Res.GetImageByID(ResID.IMAGE_FX_UNDERWATER_SIDEWAKE_LIGHT);
					wakeStruct7.mAdditive = true;
					wakeStruct7.mVel = -wakeStruct.mVel.Perp() / t;
					sexyVector = wakeStruct7.mVel.Normalize();
					wakeStruct7.mAngle = wakeStruct.mAngle;
					wakeStruct7.mX = wakeStruct.mX + (float)Common._M(15) * sexyVector.x;
					wakeStruct7.mY = wakeStruct.mY + (float)Common._M(15) * sexyVector.y;
					wakeStruct7.mAlpha = 0f;
					wakeStruct7.mAlphaInc = mAlphaInc;
					list.Add(wakeStruct7);
				}
			}
			for (int j = 0; j < Enumerable.Count<WakeStruct>(list); j++)
			{
				this.mWake.Add(list[j]);
			}
			list.Clear();
			this.DebugCheck();
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0002EC0D File Offset: 0x0002CE0D
		public override string GetName()
		{
			return "BallWake";
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0002EC14 File Offset: 0x0002CE14
		public override void DrawAboveBalls(Graphics g)
		{
			if (!g.Is3D())
			{
				return;
			}
			this.DebugCheck();
			for (int i = 0; i < Enumerable.Count<WakeStruct>(this.mWake); i++)
			{
				WakeStruct wakeStruct = this.mWake[i];
				if (wakeStruct.mAdditive)
				{
					g.SetDrawMode(1);
				}
				if (wakeStruct.mAlpha != 255f)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)wakeStruct.mAlpha);
				}
				this.mGlobalTranform.Reset();
				this.mGlobalTranform.RotateRad(wakeStruct.mAngle);
				if (wakeStruct.mSize != 1f)
				{
					this.mGlobalTranform.Scale(wakeStruct.mSize, wakeStruct.mSize);
				}
				if (g.Is3D())
				{
					g.DrawImageTransformF(wakeStruct.mImage, this.mGlobalTranform, Common._S(wakeStruct.mX), Common._S(wakeStruct.mY));
				}
				else
				{
					g.DrawImageTransform(wakeStruct.mImage, this.mGlobalTranform, Common._S(wakeStruct.mX), Common._S(wakeStruct.mY));
				}
				g.SetColorizeImages(false);
				g.SetDrawMode(0);
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0002ED40 File Offset: 0x0002CF40
		public override void BulletFired(Bullet b)
		{
			WakeStruct wakeStruct = new WakeStruct();
			this.mWake.Add(wakeStruct);
			wakeStruct.mBallId = (uint)b.GetId();
			wakeStruct.mExpanding = false;
			wakeStruct.mVel = new SexyVector2(b.mVelX, b.mVelY);
			wakeStruct.mSize = 1f;
			wakeStruct.mAlpha = 255f;
			wakeStruct.mAdditive = true;
			wakeStruct.mImage = Res.GetImageByID(ResID.IMAGE_FX_UNDERWATER_BALL_WAKE);
			GameApp.gApp.GetBoard().GetGun();
			wakeStruct.mAngle = b.mAngleFired;
			SexyVector2 sexyVector = wakeStruct.mVel.Normalize();
			wakeStruct.mX = b.GetX() - 20f * sexyVector.x;
			wakeStruct.mY = b.GetY() - 20f * sexyVector.y;
			wakeStruct.mIsHead = true;
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0002EE1C File Offset: 0x0002D01C
		public override void BulletHit(Bullet b)
		{
			for (int i = 0; i < this.mWake.size<WakeStruct>(); i++)
			{
				WakeStruct wakeStruct = this.mWake[i];
				if ((ulong)wakeStruct.mBallId == (ulong)((long)b.GetId()))
				{
					wakeStruct.mExpanding = true;
				}
			}
		}

		// Token: 0x04000408 RID: 1032
		protected List<WakeStruct> mWake = new List<WakeStruct>();

		// Token: 0x04000409 RID: 1033
		protected Transform mGlobalTranform = new Transform();
	}
}
