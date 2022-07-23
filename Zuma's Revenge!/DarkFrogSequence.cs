using System;
using System.Collections.Generic;
using System.Linq;
using JeffLib;
using Microsoft.Xna.Framework;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;
using SexyFramework.PIL;

namespace ZumasRevenge
{
	// Token: 0x02000098 RID: 152
	public class DarkFrogSequence : IDisposable
	{
		// Token: 0x060009CF RID: 2511 RVA: 0x0005B187 File Offset: 0x00059387
		public static float GetScale()
		{
			return ZumasRevenge.Common._M(1f);
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0005B193 File Offset: 0x00059393
		public static float FS(float x)
		{
			return x * DarkFrogSequence.GetScale();
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0005B19C File Offset: 0x0005939C
		protected void SetupStart()
		{
			int num = (int)ZumasRevenge.Common._S(DarkFrogSequence.DEST_X);
			int num2 = (int)ZumasRevenge.Common._S(DarkFrogSequence.DEST_Y);
			AfterEffectsTimeline afterEffectsTimeline = new AfterEffectsTimeline();
			afterEffectsTimeline.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_1);
			afterEffectsTimeline.mStartFrame = 0;
			afterEffectsTimeline.mEndFrame = (int)DarkFrogSequence.FS(73f);
			afterEffectsTimeline.AddPosX(new Component((float)num, (float)num, 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
			afterEffectsTimeline.AddPosY(new Component((float)num2, (float)num2, 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
			this.mTimeline.Add(afterEffectsTimeline);
			int num3 = ZumasRevenge.Common._S(ZumasRevenge.Common._M(-1));
			int num4 = ZumasRevenge.Common._S(ZumasRevenge.Common._M(-5));
			afterEffectsTimeline = new AfterEffectsTimeline();
			afterEffectsTimeline.mImage = Res.GetImageByID(ResID.IMAGE_FROG_NORMAL_EYES);
			afterEffectsTimeline.mCel = 1;
			afterEffectsTimeline.mStartFrame = (int)DarkFrogSequence.FS(31f);
			afterEffectsTimeline.mEndFrame = (int)DarkFrogSequence.FS(42f);
			afterEffectsTimeline.AddPosX(new Component((float)(num + num3), (float)(num + num3), 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
			afterEffectsTimeline.AddPosY(new Component((float)(num2 + num4), (float)(num2 + num4), 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
			this.mTimeline.Add(afterEffectsTimeline);
			afterEffectsTimeline = new AfterEffectsTimeline();
			afterEffectsTimeline.mImage = Res.GetImageByID(ResID.IMAGE_FROG_NORMAL_EYES);
			afterEffectsTimeline.mCel = 1;
			afterEffectsTimeline.mStartFrame = (int)DarkFrogSequence.FS(59f);
			afterEffectsTimeline.mEndFrame = (int)DarkFrogSequence.FS(73f);
			afterEffectsTimeline.AddPosX(new Component((float)(num + num3), (float)(num + num3), 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
			afterEffectsTimeline.AddPosY(new Component((float)(num2 + num4), (float)(num2 + num4), 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
			this.mTimeline.Add(afterEffectsTimeline);
			this.SetupFrogLooks(0, false);
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0005B378 File Offset: 0x00059578
		protected void SetupShakeItOff()
		{
			int num = (int)ZumasRevenge.Common._S(DarkFrogSequence.DEST_X);
			int num2 = (int)ZumasRevenge.Common._S(DarkFrogSequence.DEST_Y);
			int num3 = (int)((float)ZumasRevenge.Common._M(261) * DarkFrogSequence.GetScale());
			Image[] array = new Image[]
			{
				Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_3),
				Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_7),
				Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_3),
				Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_2),
				Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_7),
				Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_2),
				Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_3),
				Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_7),
				Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_3),
				Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_2),
				Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_7),
				Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_2),
				Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_3)
			};
			int[] array2 = new int[]
			{
				(int)DarkFrogSequence.FS(20f),
				(int)DarkFrogSequence.FS(6f),
				(int)DarkFrogSequence.FS(6f),
				(int)DarkFrogSequence.FS(4f),
				(int)DarkFrogSequence.FS(6f),
				(int)DarkFrogSequence.FS(4f),
				(int)DarkFrogSequence.FS(4f),
				(int)DarkFrogSequence.FS(6f),
				(int)DarkFrogSequence.FS(6f),
				(int)DarkFrogSequence.FS(4f),
				(int)DarkFrogSequence.FS(6f),
				(int)DarkFrogSequence.FS(4f),
				(int)DarkFrogSequence.FS(4f)
			};
			int num4 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				AfterEffectsTimeline afterEffectsTimeline = new AfterEffectsTimeline();
				afterEffectsTimeline.mImage = array[i];
				afterEffectsTimeline.mStartFrame = num3 + num4;
				afterEffectsTimeline.mEndFrame = afterEffectsTimeline.mStartFrame + array2[i];
				afterEffectsTimeline.AddPosX(new Component((float)num));
				afterEffectsTimeline.AddPosY(new Component((float)num2));
				if (i == 4 || i == 10)
				{
					afterEffectsTimeline.mMirror = true;
				}
				num4 += array2[i];
				this.mTimeline.Add(afterEffectsTimeline);
			}
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0005B5DC File Offset: 0x000597DC
		protected void SetupFrogLooks(int start_time, bool hold)
		{
			int num = (int)ZumasRevenge.Common._S(DarkFrogSequence.DEST_X);
			int num2 = (int)ZumasRevenge.Common._S(DarkFrogSequence.DEST_Y);
			float num3 = (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M(0));
			float num4 = (float)(hold ? ZumasRevenge.Common._S(ZumasRevenge.Common._M(-12)) : 0);
			AfterEffectsTimeline afterEffectsTimeline = new AfterEffectsTimeline();
			afterEffectsTimeline.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_4);
			afterEffectsTimeline.mStartFrame = (int)((float)start_time + DarkFrogSequence.FS(73f));
			afterEffectsTimeline.mEndFrame = (int)((float)start_time + DarkFrogSequence.FS(137f));
			afterEffectsTimeline.mHoldLastFrame = hold;
			afterEffectsTimeline.AddPosX(new Component((float)num + num3, (float)num + num3, 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
			afterEffectsTimeline.AddPosY(new Component((float)num2 + num4, (float)num2 + num4, 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
			this.mTimeline.Add(afterEffectsTimeline);
			afterEffectsTimeline = new AfterEffectsTimeline();
			afterEffectsTimeline.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_4_PUPIL);
			afterEffectsTimeline.mStartFrame = (int)((float)start_time + DarkFrogSequence.FS(73f));
			afterEffectsTimeline.mEndFrame = (int)((float)start_time + DarkFrogSequence.FS(137f));
			SexyPoint[] array = new SexyPoint[]
			{
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M(-22)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(-5))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M2(-26)), ZumasRevenge.Common._S(ZumasRevenge.Common._M3(-8))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M4(-21)), ZumasRevenge.Common._S(ZumasRevenge.Common._M5(-12))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M6(-13)), ZumasRevenge.Common._S(ZumasRevenge.Common._M7(-3))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M(-22)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(-3))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M2(-26)), ZumasRevenge.Common._S(ZumasRevenge.Common._M3(-9))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M(-16)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(-7))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M2(-22)), ZumasRevenge.Common._S(ZumasRevenge.Common._M3(-5)))
			};
			int[] array2 = new int[]
			{
				(int)DarkFrogSequence.FS(0f),
				(int)DarkFrogSequence.FS(7f),
				(int)DarkFrogSequence.FS(16f),
				(int)DarkFrogSequence.FS(28f),
				(int)DarkFrogSequence.FS(34f),
				(int)DarkFrogSequence.FS(40f),
				(int)DarkFrogSequence.FS(48f),
				(int)DarkFrogSequence.FS(60f),
				afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame
			};
			int num5 = (hold ? 1 : array.Length);
			for (int i = 0; i < num5; i++)
			{
				float val = (float)(num + array[0].mX);
				float num6 = (float)(num2 + array[0].mY);
				if (i > 0)
				{
					val = (float)(num + array[i - 1].mX);
					num6 = (float)(num2 + array[i - 1].mY);
				}
				afterEffectsTimeline.AddPosX(new Component(val, (float)(num + array[i].mX), array2[i], array2[i + 1]));
				afterEffectsTimeline.AddPosY(new Component(num6 + num4, (float)(num2 + array[i].mY) + num4, array2[i], array2[i + 1]));
			}
			afterEffectsTimeline.mHoldLastFrame = hold;
			this.mTimeline.Add(afterEffectsTimeline);
			afterEffectsTimeline = new AfterEffectsTimeline();
			afterEffectsTimeline.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_4_PUPIL);
			afterEffectsTimeline.mStartFrame = (int)((float)start_time + DarkFrogSequence.FS(73f));
			afterEffectsTimeline.mEndFrame = (int)((float)start_time + DarkFrogSequence.FS(137f));
			SexyPoint[] array3 = new SexyPoint[]
			{
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M(20)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(-5))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M2(16)), ZumasRevenge.Common._S(ZumasRevenge.Common._M3(-8))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M4(21)), ZumasRevenge.Common._S(ZumasRevenge.Common._M5(-12))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M6(29)), ZumasRevenge.Common._S(ZumasRevenge.Common._M7(-3))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M(20)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(-3))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M2(16)), ZumasRevenge.Common._S(ZumasRevenge.Common._M3(-9))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M(26)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(-7))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M2(20)), ZumasRevenge.Common._S(ZumasRevenge.Common._M3(-5)))
			};
			for (int j = 0; j < num5; j++)
			{
				float val2 = (float)(num + array3[0].mX);
				float num7 = (float)(num2 + array3[0].mY);
				if (j > 0)
				{
					val2 = (float)(num + array3[j - 1].mX);
					num7 = (float)(num2 + array3[j - 1].mY);
				}
				afterEffectsTimeline.AddPosX(new Component(val2, (float)(num + array3[j].mX), array2[j], array2[j + 1]));
				afterEffectsTimeline.AddPosY(new Component(num7 + num4, (float)(num2 + array3[j].mY) + num4, array2[j], array2[j + 1]));
			}
			afterEffectsTimeline.mHoldLastFrame = hold;
			this.mTimeline.Add(afterEffectsTimeline);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0005BB94 File Offset: 0x00059D94
		protected void SetupInflato(int start_time, int end_time, bool fade, bool blink)
		{
			int num = (int)ZumasRevenge.Common._S(DarkFrogSequence.DEST_X);
			int num2 = (int)ZumasRevenge.Common._S(DarkFrogSequence.DEST_Y);
			AfterEffectsTimeline afterEffectsTimeline = new AfterEffectsTimeline();
			afterEffectsTimeline.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_4);
			afterEffectsTimeline.mStartFrame = start_time;
			afterEffectsTimeline.mEndFrame = (int)((float)start_time + DarkFrogSequence.FS(36f));
			afterEffectsTimeline.AddPosX(new Component((float)num, (float)num, 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
			afterEffectsTimeline.AddPosY(new Component((float)num2, (float)num2, 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
			this.mTimeline.Add(afterEffectsTimeline);
			float num3 = (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M(-22));
			float num4 = (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M(-5));
			afterEffectsTimeline = new AfterEffectsTimeline();
			afterEffectsTimeline.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_4_PUPIL);
			afterEffectsTimeline.mStartFrame = start_time;
			afterEffectsTimeline.mEndFrame = (int)((float)start_time + DarkFrogSequence.FS(16f));
			afterEffectsTimeline.AddPosX(new Component((float)num + num3, (float)num + num3, 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
			afterEffectsTimeline.AddPosY(new Component((float)num2 + num4, (float)num2 + num4, 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
			this.mTimeline.Add(afterEffectsTimeline);
			num3 = (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M(20));
			afterEffectsTimeline = new AfterEffectsTimeline();
			afterEffectsTimeline.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_4_PUPIL);
			afterEffectsTimeline.mStartFrame = start_time;
			afterEffectsTimeline.mEndFrame = (int)((float)start_time + DarkFrogSequence.FS(16f));
			afterEffectsTimeline.AddPosX(new Component((float)num + num3, (float)num + num3, 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
			afterEffectsTimeline.AddPosY(new Component((float)num2 + num4, (float)num2 + num4, 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
			this.mTimeline.Add(afterEffectsTimeline);
			afterEffectsTimeline = new AfterEffectsTimeline();
			afterEffectsTimeline.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_5);
			afterEffectsTimeline.mStartFrame = (int)((float)start_time + DarkFrogSequence.FS(36f));
			afterEffectsTimeline.mEndFrame = (int)((float)start_time + DarkFrogSequence.FS(124f));
			afterEffectsTimeline.AddScaleY(new Component(1f, ZumasRevenge.Common._M(0.75f), (int)DarkFrogSequence.FS(39f), (int)DarkFrogSequence.FS(88f)));
			afterEffectsTimeline.AddPosX(new Component((float)num, (float)num, 0, (int)DarkFrogSequence.FS(124f)));
			afterEffectsTimeline.AddPosY(new Component((float)num2, (float)num2, 0, (int)DarkFrogSequence.FS(75f)));
			if (fade)
			{
				afterEffectsTimeline.AddOpacity(new Component(1f, 0f, (int)DarkFrogSequence.FS(84f), (int)DarkFrogSequence.FS(88f)));
			}
			this.mTimeline.Add(afterEffectsTimeline);
			afterEffectsTimeline = new AfterEffectsTimeline();
			afterEffectsTimeline.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_BUGEYE);
			afterEffectsTimeline.mStartFrame = (int)((float)start_time + DarkFrogSequence.FS(16f));
			afterEffectsTimeline.mEndFrame = (int)((float)start_time + DarkFrogSequence.FS(107f));
			float[] array = new float[]
			{
				ZumasRevenge.Common._M(0.26f),
				ZumasRevenge.Common._M1(0.45f),
				ZumasRevenge.Common._M2(1.5f),
				ZumasRevenge.Common._M3(1.5f),
				ZumasRevenge.Common._M4(0.169f),
				ZumasRevenge.Common._M5(0.169f)
			};
			float[] array2 = new float[]
			{
				DarkFrogSequence.FS(33f),
				DarkFrogSequence.FS(40f),
				DarkFrogSequence.FS(46f),
				DarkFrogSequence.FS(75f),
				DarkFrogSequence.FS(107f),
				(float)(afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame)
			};
			for (int i = 0; i < array.Length - 1; i++)
			{
				float target = 1f + array[i + 1] - 0.26f;
				float val = 1f + array[i] - 0.26f;
				afterEffectsTimeline.AddScaleX(new Component(val, target, (int)(array2[i] - DarkFrogSequence.FS(16f)), (int)(array2[i + 1] - DarkFrogSequence.FS(16f))));
			}
			SexyPoint[] array3 = new SexyPoint[]
			{
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M(-23)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(-2))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M2(-29)), ZumasRevenge.Common._S(ZumasRevenge.Common._M3(-2))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M4(-29)), ZumasRevenge.Common._S(ZumasRevenge.Common._M5(-3))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M6(-29)), ZumasRevenge.Common._S(ZumasRevenge.Common._M7(-9))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M(-29)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(-9))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M2(-28)), ZumasRevenge.Common._S(ZumasRevenge.Common._M3(-9)))
			};
			int[] array4 = new int[]
			{
				(int)DarkFrogSequence.FS(35f),
				(int)DarkFrogSequence.FS(38f),
				(int)DarkFrogSequence.FS(75f),
				(int)DarkFrogSequence.FS(95f),
				(int)DarkFrogSequence.FS(104f),
				(int)DarkFrogSequence.FS(106f),
				(int)DarkFrogSequence.FS(106f)
			};
			for (int j = 0; j < array3.Length; j++)
			{
				float val2 = (float)(num + array3[0].mX);
				float val3 = (float)(num2 + array3[0].mY);
				if (j > 0)
				{
					val2 = (float)(num + array3[j - 1].mX);
					val3 = (float)(num2 + array3[j - 1].mY);
				}
				afterEffectsTimeline.AddPosX(new Component(val2, (float)(num + array3[j].mX), array4[j] - (int)DarkFrogSequence.FS(16f), array4[j + 1] - (int)DarkFrogSequence.FS(16f)));
				afterEffectsTimeline.AddPosY(new Component(val3, (float)(num2 + array3[j].mY), array4[j] - (int)DarkFrogSequence.FS(16f), array4[j + 1] - (int)DarkFrogSequence.FS(16f)));
			}
			this.mTimeline.Add(afterEffectsTimeline);
			afterEffectsTimeline = new AfterEffectsTimeline();
			afterEffectsTimeline.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_BUGEYE);
			afterEffectsTimeline.mStartFrame = (int)((float)start_time + DarkFrogSequence.FS(16f));
			afterEffectsTimeline.mEndFrame = (int)((float)start_time + DarkFrogSequence.FS(107f));
			for (int k = 0; k < array.Length - 1; k++)
			{
				float target2 = 1f + array[k + 1] - 0.26f;
				float val4 = 1f + array[k] - 0.26f;
				afterEffectsTimeline.AddScaleX(new Component(val4, target2, (int)(array2[k] - DarkFrogSequence.FS(16f)), (int)(array2[k + 1] - DarkFrogSequence.FS(16f))));
			}
			SexyPoint[] array5 = new SexyPoint[]
			{
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M(21)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(-2))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M2(29)), ZumasRevenge.Common._S(ZumasRevenge.Common._M3(-2))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M4(29)), ZumasRevenge.Common._S(ZumasRevenge.Common._M5(-3))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M6(29)), ZumasRevenge.Common._S(ZumasRevenge.Common._M7(-9))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M(29)), ZumasRevenge.Common._S(ZumasRevenge.Common._M1(-9))),
				new SexyPoint(ZumasRevenge.Common._S(ZumasRevenge.Common._M2(-27)), ZumasRevenge.Common._S(ZumasRevenge.Common._M3(-9)))
			};
			for (int l = 0; l < array5.Length; l++)
			{
				float val5 = (float)(num + array5[0].mX);
				float val6 = (float)(num2 + array5[0].mY);
				if (l > 0)
				{
					val5 = (float)(num + array5[l - 1].mX);
					val6 = (float)(num2 + array5[l - 1].mY);
				}
				afterEffectsTimeline.AddPosX(new Component(val5, (float)(num + array5[l].mX), array4[l] - (int)DarkFrogSequence.FS(16f), array4[l + 1] - (int)DarkFrogSequence.FS(16f)));
				afterEffectsTimeline.AddPosY(new Component(val6, (float)(num2 + array5[l].mY), array4[l] - (int)DarkFrogSequence.FS(16f), array4[l + 1] - (int)DarkFrogSequence.FS(16f)));
			}
			this.mTimeline.Add(afterEffectsTimeline);
			if (blink)
			{
				int num5 = ZumasRevenge.Common._S(ZumasRevenge.Common._M(-6));
				int num6 = ZumasRevenge.Common._S(ZumasRevenge.Common._M(12));
				int num7 = (int)((float)ZumasRevenge.Common._M(125) * DarkFrogSequence.GetScale());
				int num8 = (int)((float)ZumasRevenge.Common._M(153) * DarkFrogSequence.GetScale());
				afterEffectsTimeline = new AfterEffectsTimeline();
				afterEffectsTimeline.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_FRAME_4);
				afterEffectsTimeline.mStartFrame = start_time + num7;
				afterEffectsTimeline.mEndFrame = start_time + num8;
				afterEffectsTimeline.AddPosX(new Component((float)num, (float)num, 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
				afterEffectsTimeline.AddPosY(new Component((float)(num2 + num5), (float)(num2 - num6), 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
				afterEffectsTimeline.AddScaleX(new Component(ZumasRevenge.Common._M(1.03f), ZumasRevenge.Common._M1(1f), 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
				afterEffectsTimeline.AddScaleY(new Component(ZumasRevenge.Common._M(0.887f), ZumasRevenge.Common._M1(1f), 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
				this.mTimeline.Add(afterEffectsTimeline);
				afterEffectsTimeline = new AfterEffectsTimeline();
				afterEffectsTimeline.mImage = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_4_BLINK);
				afterEffectsTimeline.mStartFrame = start_time + num7;
				afterEffectsTimeline.mEndFrame = start_time + num8;
				num3 = (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M(-1));
				num4 = (float)ZumasRevenge.Common._S(ZumasRevenge.Common._M(-5));
				afterEffectsTimeline.AddPosX(new Component((float)num + num3, (float)num + num3, 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
				afterEffectsTimeline.AddPosY(new Component((float)(num2 + num5) + num4, (float)num2 + num4 - (float)num6, 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
				afterEffectsTimeline.AddScaleX(new Component(ZumasRevenge.Common._M(1.03f), ZumasRevenge.Common._M1(1f), 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
				afterEffectsTimeline.AddScaleY(new Component(ZumasRevenge.Common._M(0.887f), ZumasRevenge.Common._M1(1f), 0, afterEffectsTimeline.mEndFrame - afterEffectsTimeline.mStartFrame));
				this.mTimeline.Add(afterEffectsTimeline);
			}
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0005C64C File Offset: 0x0005A84C
		protected void SetupGenieSmokeTrail()
		{
			this.mGenieSmoke = new SexyFramework.PIL.System(350, 50);
			if (!GameApp.gApp.Is3DAccelerated())
			{
				this.mGenieSmoke.mHighWatermark = ZumasRevenge.Common._M(80);
				this.mGenieSmoke.mLowWatermark = ZumasRevenge.Common._M(30);
				this.mGenieSmoke.mFPSCallback = new SexyFramework.PIL.System.FPSCallback(SexyFramework.PIL.System.FadeParticlesFPSCallback);
			}
			this.mGenieSmoke.mScale = ZumasRevenge.Common._S(1f);
			this.mGenieSmoke.WaitForEmitters(true);
			this.mGenieSmoke.SetLife((int)((float)ZumasRevenge.Common._M(350) * DarkFrogSequence.frame_mult));
			Emitter emitter = new Emitter();
			emitter.mCullingRect = new Rect(0, 0, ZumasRevenge.Common._SS(GameApp.gApp.mWidth), ZumasRevenge.Common._SS(GameApp.gApp.mHeight));
			emitter.mEmissionCoordsAreOffsets = true;
			this.SetupPaths(emitter.mWaypointManager, ZumasRevenge.Common._M(2f));
			emitter.mPreloadFrames = ZumasRevenge.Common._M(0);
			emitter.AddScaleKeyFrame(0, new EmitterScale
			{
				mNumberScale = ZumasRevenge.Common._M(1f),
				mSizeXScale = ZumasRevenge.Common._M(1.5f)
			});
			emitter.AddSettingsKeyFrame(0, new EmitterSettings
			{
				mVisibility = ZumasRevenge.Common._M(0.5f),
				mEmissionAngle = SexyFramework.Common.DegreesToRadians((float)ZumasRevenge.Common._M(90)),
				mEmissionRange = SexyFramework.Common.DegreesToRadians((float)ZumasRevenge.Common._M(333))
			});
			ParticleType particleType = new ParticleType();
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_SMOKE_ANIM);
			particleType.mRandomStartCel = true;
			particleType.mImageRate = 0;
			particleType.mAlignAngleToMotion = ZumasRevenge.Common._M(0) == 1;
			particleType.mColorKeyManager.AddColorKey(0f, new SexyColor(84, 0, 0));
			particleType.mColorKeyManager.AddColorKey(0.125f, new SexyColor(SexyColor.Black));
			particleType.mColorKeyManager.AddColorKey(0.25f, new SexyColor(255, 0, 0));
			particleType.mColorKeyManager.AddColorKey(0.375f, new SexyColor(14, 0, 0));
			particleType.mColorKeyManager.AddColorKey(0.5f, new SexyColor(63, 29, 255));
			particleType.mColorKeyManager.AddColorKey(0.75f, new SexyColor(148, 0, 255));
			particleType.mColorKeyManager.AddColorKey(1f, new SexyColor(SexyColor.Black));
			particleType.mAlphaKeyManager.AddAlphaKey(0f, 255);
			particleType.mAlphaKeyManager.AddAlphaKey(ZumasRevenge.Common._M(0.5f), 255);
			particleType.mAlphaKeyManager.AddAlphaKey(1f, 0);
			ParticleSettings particleSettings = new ParticleSettings();
			particleSettings.mLife = ZumasRevenge.Common._M(30);
			particleSettings.mNumber = (int)((float)ZumasRevenge.Common._M(50) * DarkFrogSequence.GENIE_SMOKE_TRAIL_PARTICLE_REDUCTION_PERCENT);
			particleSettings.mXSize = ZumasRevenge.Common._M(18);
			particleSettings.mVelocity = ZumasRevenge.Common._M(5);
			particleSettings.mWeight = (float)ZumasRevenge.Common._M(-4);
			particleType.AddSettingsKeyFrame(0, particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mNumber = (int)((float)ZumasRevenge.Common._M(81) * DarkFrogSequence.GENIE_SMOKE_TRAIL_PARTICLE_REDUCTION_PERCENT);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(15), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mNumber = (int)((float)ZumasRevenge.Common._M(46) * DarkFrogSequence.GENIE_SMOKE_TRAIL_PARTICLE_REDUCTION_PERCENT);
			particleSettings.mXSize = ZumasRevenge.Common._M(36);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(101), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mNumber = (int)((float)ZumasRevenge.Common._M(83) * DarkFrogSequence.GENIE_SMOKE_TRAIL_PARTICLE_REDUCTION_PERCENT);
			particleSettings.mXSize = ZumasRevenge.Common._M(43);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(134), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mXSize = ZumasRevenge.Common._M(21);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(148), particleSettings);
			particleSettings = new ParticleSettings(particleSettings);
			particleSettings.mXSize = ZumasRevenge.Common._M(55);
			particleType.AddSettingsKeyFrame(ZumasRevenge.Common._M(199), particleSettings);
			particleType.AddVarianceKeyFrame(0, new ParticleVariance
			{
				mLifeVar = ZumasRevenge.Common._M(9),
				mNumberVar = ZumasRevenge.Common._M(44),
				mSizeXVar = ZumasRevenge.Common._M(3),
				mVelocityVar = ZumasRevenge.Common._M(10),
				mWeightVar = ZumasRevenge.Common._M(6)
			});
			LifetimeSettings lifetimeSettings = new LifetimeSettings();
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(2f);
			particleType.AddSettingAtLifePct(0f, lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(1.3f);
			particleType.AddSettingAtLifePct(0.62f, lifetimeSettings);
			particleType.AddSettingAtLifePct(1f, new LifetimeSettings(lifetimeSettings)
			{
				mSizeXMult = 0f
			});
			emitter.AddParticleType(particleType);
			this.mGenieSmoke.AddEmitter(emitter);
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0005CB30 File Offset: 0x0005AD30
		protected void SetupBoilingSmoke()
		{
			this.mBoilingSmoke = new SexyFramework.PIL.System(100, 50);
			if (!GameApp.gApp.Is3DAccelerated())
			{
				this.mBoilingSmoke.mHighWatermark = ZumasRevenge.Common._M(80);
				this.mBoilingSmoke.mLowWatermark = ZumasRevenge.Common._M(30);
				this.mBoilingSmoke.mFPSCallback = new SexyFramework.PIL.System.FPSCallback(SexyFramework.PIL.System.FadeParticlesFPSCallback);
			}
			this.mBoilingSmoke.mScale = ZumasRevenge.Common._S(1f);
			this.mBoilingSmoke.WaitForEmitters(true);
			this.mBoilingSmoke.SetLife((int)((float)ZumasRevenge.Common._M(240) * DarkFrogSequence.frame_mult));
			Emitter emitter = new Emitter();
			emitter.mCullingRect = new Rect(0, 0, ZumasRevenge.Common._SS(GameApp.gApp.mWidth), ZumasRevenge.Common._SS(GameApp.gApp.mHeight));
			emitter.mEmissionCoordsAreOffsets = true;
			this.SetupPaths(emitter.mWaypointManager, ZumasRevenge.Common._M(2f));
			emitter.mPreloadFrames = ZumasRevenge.Common._M(0);
			EmitterScale emitterScale = new EmitterScale();
			emitterScale.mLifeScale = ZumasRevenge.Common._M(0.79f);
			emitterScale.mNumberScale = ZumasRevenge.Common._M(0.45f);
			emitterScale.mSizeXScale = ZumasRevenge.Common._M(0.31f);
			emitterScale.mZoom = ZumasRevenge.Common._M(1.49f);
			emitter.AddScaleKeyFrame(0, emitterScale);
			emitterScale = new EmitterScale(emitterScale);
			emitterScale.mSizeXScale = ZumasRevenge.Common._M(2.04f);
			emitter.AddScaleKeyFrame((int)((float)ZumasRevenge.Common._M(110) * DarkFrogSequence.frame_mult), emitterScale);
			emitter.AddSettingsKeyFrame(0, new EmitterSettings
			{
				mEmissionAngle = SexyFramework.Common.DegreesToRadians((float)ZumasRevenge.Common._M(92))
			});
			ParticleType particleType = new ParticleType();
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_SMOKE_COLOR);
			particleType.mAngleRange = 6.2831855f;
			particleType.mFlipY = true;
			particleType.mColorKeyManager.AddColorKey(0f, new SexyColor(120, 120, 120));
			particleType.mColorKeyManager.AddColorKey(1f, new SexyColor(SexyColor.Black));
			particleType.mAlphaKeyManager.AddAlphaKey(0f, 0);
			particleType.mAlphaKeyManager.AddAlphaKey(ZumasRevenge.Common._M(0.1f), 255);
			particleType.mAlphaKeyManager.AddAlphaKey(ZumasRevenge.Common._M(0.75f), 255);
			particleType.mAlphaKeyManager.AddAlphaKey(1f, 0);
			particleType.AddSettingsKeyFrame(0, new ParticleSettings
			{
				mLife = ZumasRevenge.Common._M(9),
				mNumber = ZumasRevenge.Common._M(60),
				mXSize = ZumasRevenge.Common._M(30),
				mVelocity = ZumasRevenge.Common._M(16),
				mWeight = (float)ZumasRevenge.Common._M(-13)
			});
			ParticleVariance particleVariance = new ParticleVariance();
			particleVariance.mLifeVar = ZumasRevenge.Common._M(9);
			particleVariance.mNumberVar = ZumasRevenge.Common._M(48);
			particleVariance.mSizeXVar = ZumasRevenge.Common._M(3);
			particleVariance.mVelocityVar = ZumasRevenge.Common._M(10);
			particleVariance.mSpinVar = SexyFramework.Common.DegreesToRadians((float)ZumasRevenge.Common._M(12));
			particleVariance.mMotionRandVar = (float)ZumasRevenge.Common._M(18);
			particleType.AddVarianceKeyFrame(0, particleVariance);
			LifetimeSettings lifetimeSettings = new LifetimeSettings();
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(0.6f);
			particleType.AddSettingAtLifePct(0f, lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(2f);
			lifetimeSettings.mWeightMult = 0f;
			particleType.AddSettingAtLifePct(0.5f, lifetimeSettings);
			particleType.AddSettingAtLifePct(1f, new LifetimeSettings(lifetimeSettings)
			{
				mWeightMult = 1f
			});
			emitter.AddParticleType(particleType);
			particleType = new ParticleType();
			particleType.mImage = Res.GetImageByID(ResID.IMAGE_PARTICLE_BLOTCHES);
			particleType.mFlipY = true;
			particleType.mRandomStartCel = true;
			particleType.mImageRate = ZumasRevenge.Common._M(4);
			particleType.mAngleRange = 6.2831855f;
			particleType.mColorKeyManager.AddColorKey(0f, new SexyColor(56, 56, 56));
			particleType.mColorKeyManager.AddColorKey(1f, new SexyColor(SexyColor.Black));
			particleType.mAlphaKeyManager.AddAlphaKey(0f, 0);
			particleType.mAlphaKeyManager.AddAlphaKey(ZumasRevenge.Common._M(0.1f), 255);
			particleType.mAlphaKeyManager.AddAlphaKey(ZumasRevenge.Common._M(0.75f), 255);
			particleType.mAlphaKeyManager.AddAlphaKey(1f, 0);
			particleType.AddSettingsKeyFrame(0, new ParticleSettings
			{
				mLife = ZumasRevenge.Common._M(9),
				mNumber = ZumasRevenge.Common._M(60),
				mXSize = ZumasRevenge.Common._M(20),
				mVelocity = ZumasRevenge.Common._M(16),
				mWeight = (float)ZumasRevenge.Common._M(-13)
			});
			particleVariance = new ParticleVariance(particleVariance);
			particleType.AddVarianceKeyFrame(0, particleVariance);
			lifetimeSettings = new LifetimeSettings();
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(0.6f);
			particleType.AddSettingAtLifePct(0f, lifetimeSettings);
			lifetimeSettings = new LifetimeSettings(lifetimeSettings);
			lifetimeSettings.mSizeXMult = ZumasRevenge.Common._M(2f);
			lifetimeSettings.mWeightMult = 0f;
			particleType.AddSettingAtLifePct(0.5f, lifetimeSettings);
			particleType.AddSettingAtLifePct(1f, new LifetimeSettings(lifetimeSettings)
			{
				mWeightMult = 1f
			});
			emitter.AddParticleType(particleType);
			DarkFrogSequence.gDebugEmitterHandle = this.mBoilingSmoke.AddEmitter(emitter);
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0005D08C File Offset: 0x0005B28C
		protected void SetupPaths(WaypointManager w, float mult_override)
		{
			float num = ((mult_override == 0f) ? DarkFrogSequence.frame_mult : mult_override);
			int num2 = ZumasRevenge.Common._M(0);
			int num3 = ZumasRevenge.Common._M(0);
			SexyVector2[] array = new SexyVector2[]
			{
				new SexyVector2((float)ZumasRevenge.Common._M(400), (float)ZumasRevenge.Common._M1(530)),
				new SexyVector2((float)ZumasRevenge.Common._M2(553), (float)ZumasRevenge.Common._M3(554)),
				new SexyVector2((float)ZumasRevenge.Common._M4(638), (float)ZumasRevenge.Common._M5(467)),
				new SexyVector2((float)ZumasRevenge.Common._M6(619), (float)ZumasRevenge.Common._M7(327)),
				new SexyVector2((float)ZumasRevenge.Common._M8(558), (float)ZumasRevenge.Common._M9(244)),
				new SexyVector2((float)ZumasRevenge.Common._M(439), (float)ZumasRevenge.Common._M1(199)),
				new SexyVector2((float)ZumasRevenge.Common._M2(400), (float)ZumasRevenge.Common._M3(98))
			};
			int[] array2 = new int[]
			{
				ZumasRevenge.Common._M(0),
				ZumasRevenge.Common._M1(38),
				ZumasRevenge.Common._M2(76),
				ZumasRevenge.Common._M3(114),
				ZumasRevenge.Common._M4(152),
				ZumasRevenge.Common._M5(190),
				ZumasRevenge.Common._M6(228)
			};
			for (int i = 0; i < array.Length; i++)
			{
				w.AddPoint((int)((float)array2[i] * num), new Vector2(array[i].x + (float)num2, array[i].y + (float)num3), true);
			}
			w.Init(true);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0005D287 File Offset: 0x0005B487
		protected void SetupPaths(WaypointManager w)
		{
			this.SetupPaths(w, 0f);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0005D298 File Offset: 0x0005B498
		public DarkFrogSequence()
		{
			this.mUpdateCount = 0;
			this.mFrog = null;
			this.mState = 0;
			this.mVX = (this.mVY = 0f);
			this.mTimer = 0;
			this.mGenieSmoke = null;
			this.mBoilingSmoke = null;
			this.mTransportFlash = null;
			this.mFadingOut = false;
			this.mInitialDelayTarget = 1;
			this.mBGShader = null;
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0005D328 File Offset: 0x0005B528
		public virtual void Dispose()
		{
			if (this.mGenieSmoke != null)
			{
				this.mGenieSmoke.Dispose();
				this.mGenieSmoke = null;
			}
			if (this.mBoilingSmoke != null)
			{
				this.mBoilingSmoke.Dispose();
				this.mBoilingSmoke = null;
			}
			if (this.mTransportFlash != null)
			{
				this.mTransportFlash.Dispose();
				this.mTransportFlash = null;
			}
			if (this.mBGShader != null)
			{
				this.mBGShader = null;
			}
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0005D394 File Offset: 0x0005B594
		public void Update()
		{
			this.mInitialDelay++;
			if (this.mInitialDelay < this.mInitialDelayTarget)
			{
				return;
			}
			if (this.mState == 3 && !Enumerable.Last<SimpleFadeText>(this.mText).mFadeIn && Enumerable.Last<SimpleFadeText>(this.mText).mAlpha <= 0f)
			{
				for (int i = 0; i < this.mBGElementParams.Count; i++)
				{
					BGElementParams bgelementParams = this.mBGElementParams[i];
					bgelementParams.mDistAmt += bgelementParams.mDistAmtInc;
					bgelementParams.mScroll += bgelementParams.mScrollAmtInc;
					bgelementParams.mScale += bgelementParams.mScaleAmtInc;
				}
			}
			if (this.mState == 0)
			{
				this.mCurXDist += Math.Abs(this.mVX);
				this.mCurYDist += Math.Abs(this.mVY);
				if (++this.mTimer == (int)DarkFrogSequence.MOVE_TIME || (this.mCurXDist >= Math.Abs(this.mXDist) && this.mCurYDist >= Math.Abs(this.mYDist)))
				{
					this.mState = 1;
					this.mFrog.SetPos((int)DarkFrogSequence.DEST_X, (int)DarkFrogSequence.DEST_Y);
				}
				this.mXTrans = -this.mXDist + this.mVX * (float)this.mTimer;
				this.mYTrans = -this.mYDist + this.mVY * (float)this.mTimer;
			}
			else if (this.mState == 1)
			{
				this.mUpdateCount++;
				if (this.mSceneRotation.Active(this.mUpdateCount))
				{
					this.mSceneRotation.Update();
				}
				for (int j = 0; j < this.mTimeline.Count; j++)
				{
					this.mTimeline[j].Update();
				}
			}
			else if (this.mState == 2)
			{
				this.mUpdateCount++;
				this.mDarkFrogX += this.mDarkFrogVX;
				this.mDarkFrogY += this.mDarkFrogVY;
				if (++this.mTimer == ZumasRevenge.Common._M(10))
				{
					this.mFadingOut = true;
					this.mState++;
					this.mTimer = 0;
				}
				this.mXTrans += this.mVX;
				this.mYTrans += this.mVY;
			}
			else if (this.mState == 3)
			{
				for (int k = 0; k < this.mText.Count; k++)
				{
					SimpleFadeText simpleFadeText = this.mText[k];
					if (simpleFadeText.mFadeIn)
					{
						simpleFadeText.mAlpha += ZumasRevenge.Common._M(1.5f);
						if (simpleFadeText.mAlpha > 255f)
						{
							simpleFadeText.mAlpha = 255f;
						}
						if (simpleFadeText.mAlpha < (float)ZumasRevenge.Common._M(128))
						{
							break;
						}
					}
					else
					{
						simpleFadeText.mAlpha -= ZumasRevenge.Common._M(2f);
						if (simpleFadeText.mAlpha <= 0f)
						{
							simpleFadeText.mAlpha = 0f;
						}
					}
				}
				if (!Enumerable.Last<SimpleFadeText>(this.mText).mFadeIn && Enumerable.Last<SimpleFadeText>(this.mText).mAlpha <= 0f && ++this.mTimer == ZumasRevenge.Common._M(170))
				{
					this.mState = 4;
				}
				if (Enumerable.Last<SimpleFadeText>(this.mText).mFadeIn && Enumerable.Last<SimpleFadeText>(this.mText).mAlpha >= 255f && ++this.mTimer >= ZumasRevenge.Common._M(300))
				{
					for (int l = 0; l < this.mText.Count; l++)
					{
						this.mText[l].mFadeIn = false;
					}
					this.mTimer = 0;
				}
			}
			if ((float)this.mUpdateCount > (float)ZumasRevenge.Common._M(450) * DarkFrogSequence.GetScale())
			{
				this.mGenieSmoke.Update();
				this.mBoilingSmoke.Update();
			}
			if ((float)this.mUpdateCount > (float)ZumasRevenge.Common._M(1250) * DarkFrogSequence.GetScale())
			{
				this.mTattooAlpha -= ZumasRevenge.Common._M(3f);
			}
			if ((float)this.mUpdateCount > (float)ZumasRevenge.Common._M(500) * DarkFrogSequence.GetScale())
			{
				if ((float)this.mUpdateCount > (float)ZumasRevenge.Common._M(1000) * DarkFrogSequence.GetScale())
				{
					this.mDarkFrogAlpha += ZumasRevenge.Common._M(2f);
				}
				if ((float)this.mUpdateCount > (float)ZumasRevenge.Common._M(1200) * DarkFrogSequence.GetScale() && (float)this.mUpdateCount < (float)ZumasRevenge.Common._M1(1250) * DarkFrogSequence.GetScale() && this.mUpdateCount % ZumasRevenge.Common._M2(5) == 0)
				{
					this.mBlinkCel--;
				}
				if ((float)this.mUpdateCount > (float)ZumasRevenge.Common._M(950) * DarkFrogSequence.GetScale())
				{
					this.mTransportFlash.mDrawTransform.LoadIdentity();
					float num = GameApp.DownScaleNum(1f);
					this.mTransportFlash.mDrawTransform.Scale(num, num);
					this.mTransportFlash.mDrawTransform.Translate((float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M(800)), (float)ZumasRevenge.Common._DS(ZumasRevenge.Common._M1(220)));
					this.mTransportFlash.Update();
				}
				if ((float)this.mUpdateCount == (float)ZumasRevenge.Common._M(1300) * DarkFrogSequence.GetScale())
				{
					this.mBlinkCel = 0;
					this.mDoTongueFlick = true;
				}
				if (this.mDoTongueFlick)
				{
					float num2 = ZumasRevenge.Common._M(2f);
					if (this.mMoveTongueDown && (this.mTongueYOff += num2) >= (float)ZumasRevenge.Common._M(60))
					{
						this.mBlinkCel = -1;
						this.mMoveTongueDown = false;
						return;
					}
					if (!this.mMoveTongueDown && (this.mTongueYOff -= num2) <= 0f)
					{
						this.mDoTongueFlick = false;
						this.mState = 2;
						this.mTimer = 0;
						this.mCurXDist = (this.mCurYDist = 0f);
						this.mVX = (DarkFrogSequence.FROG_CENTERX - DarkFrogSequence.DEST_X) / DarkFrogSequence.MOVE_TIME;
						this.mVY = 0f;
						this.mDarkFrogVX = (DarkFrogSequence.FROG_CENTERX - this.mDarkFrogX) / DarkFrogSequence.MOVE_TIME;
						this.mDarkFrogVY = (DarkFrogSequence.DARK_FROG_CENTERY - this.mDarkFrogY) / DarkFrogSequence.MOVE_TIME;
					}
				}
			}
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0005DA4C File Offset: 0x0005BC4C
		public void Draw(Graphics g)
		{
			if (this.mInitialDelay < this.mInitialDelayTarget)
			{
				return;
			}
			float bgalpha = this.GetBGAlpha();
			g.SetColor(0, 0, 0, (int)bgalpha);
			if (ZumasRevenge.Common._M(1) == 1)
			{
				g.FillRect(ZumasRevenge.Common._S(-80), 0, GameApp.gApp.mWidth + ZumasRevenge.Common._S(160), GameApp.gApp.mHeight);
			}
			DarkFrogSequence.timer += ZumasRevenge.Common._M(0.01f);
			for (int i = 0; i < 9; i++)
			{
				BGElementParams bgelementParams = this.mBGElementParams[i];
				if (bgalpha != 255f && bgalpha != 0f)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, (int)bgalpha);
				}
				g.DrawImage(bgelementParams.mImg, bgelementParams.mX, bgelementParams.mY);
				g.SetColorizeImages(false);
			}
			Graphics3D graphics3D = g.Get3D();
			this.mBoilingSmoke.Draw(g);
			this.mGenieSmoke.Draw(g);
			float num = (this.mSceneRotation.Active(this.mUpdateCount) ? this.mSceneRotation.mValue : 0f);
			SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
			if (num != 0f)
			{
				float num2 = (float)ZumasRevenge.Common._DS(100);
				Ratio aspectRatio = GameApp.gApp.mGraphicsDriver.GetAspectRatio();
				if (aspectRatio.mNumerator == 3 && aspectRatio.mDenominator == 4)
				{
					num2 = 0f;
				}
				if (graphics3D != null)
				{
					sexyTransform2D.Translate((float)ZumasRevenge.Common._S(-this.mFrog.GetCenterX()) - num2, (float)ZumasRevenge.Common._S(-this.mFrog.GetCenterY()));
					sexyTransform2D.RotateDeg(num);
					sexyTransform2D.Translate((float)ZumasRevenge.Common._S(this.mFrog.GetCenterX()) + num2, (float)ZumasRevenge.Common._S(this.mFrog.GetCenterY()));
					graphics3D.PushTransform(sexyTransform2D);
				}
			}
			SexyTransform2D sexyTransform2D2 = new SexyTransform2D(false);
			if (this.mState != 1)
			{
				if (graphics3D != null)
				{
					sexyTransform2D2.Translate(ZumasRevenge.Common._S(this.mXTrans), ZumasRevenge.Common._S(this.mYTrans));
					graphics3D.PushTransform(sexyTransform2D2);
				}
				else
				{
					g.PushState();
					g.Translate((int)ZumasRevenge.Common._S(this.mXTrans), (int)ZumasRevenge.Common._S(this.mYTrans));
				}
			}
			for (int j = 0; j < this.mTimeline.Count; j++)
			{
				this.mTimeline[j].Draw(g, (int)bgalpha);
			}
			if (graphics3D != null)
			{
				if (num != 0f)
				{
					graphics3D.PopTransform();
				}
				if (this.mState != 1)
				{
					graphics3D.PopTransform();
				}
			}
			else if (this.mState != 1)
			{
				g.PopState();
			}
			int num3 = ((this.mDarkFrogAlpha < 255f) ? ((int)this.mDarkFrogAlpha) : 255);
			if (num3 != 255)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, num3);
			}
			int num4 = (int)(this.mDarkFrogX * 2f - (float)(DarkFrogSequence.CANVAS_W / 2));
			int num5 = (int)(this.mDarkFrogY * 2f - (float)(DarkFrogSequence.CANVAS_H / 2));
			Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_BACK);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_TOP);
			g.DrawImage(imageByID, ZumasRevenge.Common._DS(num4 + Res.GetOffsetXByID(ResID.IMAGE_BOSS_DARKFROG_BACK)), ZumasRevenge.Common._DS(num5 + Res.GetOffsetYByID(ResID.IMAGE_BOSS_DARKFROG_BACK)));
			g.DrawImage(imageByID2, ZumasRevenge.Common._DS(num4 + Res.GetOffsetXByID(ResID.IMAGE_BOSS_DARKFROG_TOP)), ZumasRevenge.Common._DS(num5 + Res.GetOffsetYByID(ResID.IMAGE_BOSS_DARKFROG_TOP)));
			if (this.mBlinkCel >= 0)
			{
				ResID id = ((this.mBlinkCel == 0) ? ResID.IMAGE_BOSS_DARKFROG_BLINK2 : ResID.IMAGE_BOSS_DARKFROG_BLINK1);
				Image imageByID3 = Res.GetImageByID(id);
				g.DrawImage(imageByID3, ZumasRevenge.Common._DS(num4 + Res.GetOffsetXByID(id)), ZumasRevenge.Common._DS(num5 + Res.GetOffsetYByID(id)));
			}
			g.SetColorizeImages(false);
			g.PushState();
			this.mTransportFlash.Draw(g);
			g.PopState();
			if (this.mDarkFrogAlpha >= 255f)
			{
				num3 = (int)this.mTattooAlpha;
				if (num3 < 0)
				{
					num3 = 0;
				}
			}
			if (num3 != 255)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, num3);
			}
			Image imageByID4 = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_TAT1);
			Image imageByID5 = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_TAT2);
			Image imageByID6 = Res.GetImageByID(ResID.IMAGE_BOSS_DARKFROG_TONGUE);
			g.DrawImage(imageByID4, ZumasRevenge.Common._DS(num4 + Res.GetOffsetXByID(ResID.IMAGE_BOSS_DARKFROG_TAT1)), ZumasRevenge.Common._DS(num5 + Res.GetOffsetYByID(ResID.IMAGE_BOSS_DARKFROG_TAT1)));
			g.DrawImage(imageByID5, ZumasRevenge.Common._DS(num4 + Res.GetOffsetXByID(ResID.IMAGE_BOSS_DARKFROG_TAT2)), ZumasRevenge.Common._DS(num5 + Res.GetOffsetYByID(ResID.IMAGE_BOSS_DARKFROG_TAT2)));
			num3 = ((this.mDarkFrogAlpha < 255f) ? ((int)this.mDarkFrogAlpha) : 255);
			g.SetColor(255, 255, 255, num3);
			g.DrawImage(imageByID6, ZumasRevenge.Common._DS(num4 + Res.GetOffsetXByID(ResID.IMAGE_BOSS_DARKFROG_TONGUE)), ZumasRevenge.Common._DS(ZumasRevenge.Common._M(0) + num5 + Res.GetOffsetYByID(ResID.IMAGE_BOSS_DARKFROG_TONGUE)));
			g.SetColorizeImages(false);
			if (this.mState == 3)
			{
				Font fontByID = Res.GetFontByID(ResID.FONT_BOSS_TAUNT);
				for (int k = 0; k < this.mText.Count; k++)
				{
					if (this.mText[k].mAlpha > 0f)
					{
						g.SetFont(fontByID);
						g.SetColor(255, 255, 255, (int)this.mText[k].mAlpha);
						g.DrawString(this.mText[k].mString, (GameApp.gApp.mWidth - fontByID.StringWidth(this.mText[k].mString)) / 2 - GameApp.gApp.mBoardOffsetX, ZumasRevenge.Common._S(ZumasRevenge.Common._M(300)) + k * fontByID.mHeight);
					}
				}
			}
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0005E058 File Offset: 0x0005C258
		public void Init()
		{
			string[] array = new string[]
			{
				TextManager.getInstance().getString(436),
				TextManager.getInstance().getString(437),
				TextManager.getInstance().getString(438)
			};
			if (GameApp.gApp.GetBoard().IsHardAdventureMode())
			{
				array[0] = TextManager.getInstance().getString(439);
				array[1] = TextManager.getInstance().getString(440);
				array[2] = TextManager.getInstance().getString(441);
			}
			for (int i = 0; i < array.Length; i++)
			{
				SimpleFadeText simpleFadeText = new SimpleFadeText();
				this.mText.Add(simpleFadeText);
				simpleFadeText.mString = array[i];
				simpleFadeText.mAlpha = 0f;
				simpleFadeText.mFadeIn = true;
			}
			this.mInitialDelay = 0;
			this.mFadingOut = false;
			DarkFrogSequence.DARK_FROG_CENTERY = ZumasRevenge.Common._M(98f);
			DarkFrogSequence.FROG_CENTERX = ZumasRevenge.Common._M(400f);
			DarkFrogSequence.MOVE_TIME = ZumasRevenge.Common._M(200f);
			this.mTimer = 0;
			this.mStartNextLevel = true;
			this.mDoTongueFlick = false;
			this.mDarkFrogAlpha = 0f;
			this.mDarkFrogX = DarkFrogSequence.FROG_CENTERX;
			this.mDarkFrogY = DarkFrogSequence.DARK_FROG_CENTERY;
			this.mXTrans = (this.mYTrans = 0f);
			this.mDarkFrogVX = (this.mDarkFrogVY = 0f);
			this.mBlinkCel = 1;
			this.mTongueYOff = 0f;
			this.mTattooAlpha = 255f;
			this.mMoveTongueDown = true;
			this.mState = 0;
			this.mUpdateCount = 0;
			this.mFrog = GameApp.gApp.GetBoard().GetGun();
			this.mTimeline.Clear();
			this.mCurXDist = (this.mCurYDist = 0f);
			this.mXDist = DarkFrogSequence.DEST_X - (float)this.mFrog.GetCenterX();
			this.mYDist = DarkFrogSequence.DEST_Y - (float)this.mFrog.GetCenterY();
			this.mVX = this.mXDist / DarkFrogSequence.MOVE_TIME;
			this.mVY = this.mYDist / DarkFrogSequence.MOVE_TIME;
			this.SetupStart();
			this.SetupShakeItOff();
			this.SetupInflato((int)DarkFrogSequence.FS(137f), (int)DarkFrogSequence.FS(244f), false, false);
			this.SetupFrogLooks((int)((float)ZumasRevenge.Common._M(269) * DarkFrogSequence.GetScale()), false);
			int num = ZumasRevenge.Common._M(374);
			this.SetupInflato((int)((float)num * DarkFrogSequence.GetScale()), (int)DarkFrogSequence.FS((float)(num + 153)), false, true);
			this.SetupFrogLooks((int)((float)num + (float)ZumasRevenge.Common._M(80) * DarkFrogSequence.GetScale()), true);
			this.mSceneRotation = new Component(0f, (float)ZumasRevenge.Common._M(360), (int)DarkFrogSequence.FS(179f), (int)DarkFrogSequence.FS(261f));
			this.SetupGenieSmokeTrail();
			this.SetupBoilingSmoke();
			this.mTransportFlash = GameApp.gApp.mResourceManager.GetPIEffect("PIEFFECT_NONRESIZE_FROGFOG").Duplicate();
			for (int j = 0; j < 9; j++)
			{
				BGElementParams bgelementParams = new BGElementParams();
				this.mBGElementParams.Add(bgelementParams);
				ResID id = ResID.IMAGE_BOSS_DARKFROG_BG_ITEM_1 + j;
				bgelementParams.mImg = Res.GetImageByID(id);
				bgelementParams.mX = ZumasRevenge.Common._DS(Res.GetOffsetXByID(id) - 160);
				bgelementParams.mY = ZumasRevenge.Common._DS(Res.GetOffsetYByID(id));
				bgelementParams.mDistAmt = SexyFramework.Common.FloatRange(ZumasRevenge.Common._M(0.0005f), ZumasRevenge.Common._M1(0.001f));
				bgelementParams.mScale = SexyFramework.Common.FloatRange(ZumasRevenge.Common._M(0.05f), ZumasRevenge.Common._M1(0.1f));
				bgelementParams.mScroll = SexyFramework.Common.FloatRange(ZumasRevenge.Common._M(0.1f), ZumasRevenge.Common._M1(0.15f));
				float num2 = 170f;
				bgelementParams.mDistAmtInc = (ZumasRevenge.Common._M(0.01f) - bgelementParams.mDistAmt) / num2;
				bgelementParams.mScaleAmtInc = (ZumasRevenge.Common._M(0.01f) - bgelementParams.mScale) / num2;
				bgelementParams.mScrollAmtInc = (ZumasRevenge.Common._M(0.5f) - bgelementParams.mScroll) / num2;
			}
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0005E490 File Offset: 0x0005C690
		public float GetBGAlpha()
		{
			int num;
			if (this.mState == 3 && !Enumerable.Last<SimpleFadeText>(this.mText).mFadeIn)
			{
				num = 255 - (int)((float)this.mTimer * ZumasRevenge.Common._M(1.5f));
			}
			else if (this.mState == 0)
			{
				num = (int)((float)this.mTimer * ZumasRevenge.Common._M(1.5f));
			}
			else
			{
				num = 255;
			}
			if (num < 0)
			{
				num = 0;
			}
			else if (num > 255)
			{
				num = 255;
			}
			return (float)num;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0005E514 File Offset: 0x0005C714
		public float GetMoveXAmt()
		{
			if (this.mState == 0)
			{
				return DarkFrogSequence.DEST_X - this.mXDist + this.mVX * (float)this.mTimer;
			}
			if (this.mState > 0)
			{
				return DarkFrogSequence.DEST_X - this.mXDist + this.mVX * DarkFrogSequence.MOVE_TIME;
			}
			return 0f;
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0005E570 File Offset: 0x0005C770
		public float GetMoveYAmt()
		{
			if (this.mState == 0)
			{
				return DarkFrogSequence.DEST_Y - this.mYDist + this.mVY * (float)this.mTimer;
			}
			if (this.mState > 0)
			{
				return DarkFrogSequence.DEST_Y - this.mYDist + this.mVY * DarkFrogSequence.MOVE_TIME;
			}
			return 0f;
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0005E5C9 File Offset: 0x0005C7C9
		public bool Done()
		{
			return this.mState == 4;
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0005E5D4 File Offset: 0x0005C7D4
		public bool CanStartNextLevel()
		{
			if (this.mFadingOut && this.mStartNextLevel)
			{
				this.mStartNextLevel = false;
				this.mFrog.SetPos((int)DarkFrogSequence.FROG_CENTERX, (int)DarkFrogSequence.DEST_Y);
				this.mFrog.SetDestAngle(-3.14159f);
				GameApp.gApp.GetBoard().mContinueNextLevelOnLoadProfile = false;
				return true;
			}
			return false;
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0005E632 File Offset: 0x0005C832
		public bool FadingOut()
		{
			return this.mFadingOut;
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0005E63A File Offset: 0x0005C83A
		public bool FadingIn()
		{
			return this.mState == 0 && (float)this.mTimer < 255f / ZumasRevenge.Common._M(1.5f);
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0005E65F File Offset: 0x0005C85F
		public bool FadingToLevel()
		{
			return this.mState == 3 && this.GetBGAlpha() < 255f;
		}

		// Token: 0x040007D9 RID: 2009
		public static float MOVE_TIME = 200f;

		// Token: 0x040007DA RID: 2010
		public static float DEST_X = 400f;

		// Token: 0x040007DB RID: 2011
		public static float DEST_Y = 532f;

		// Token: 0x040007DC RID: 2012
		public static float FROG_CENTERX = 400f;

		// Token: 0x040007DD RID: 2013
		public static float DARK_FROG_CENTERY = 98f;

		// Token: 0x040007DE RID: 2014
		public static int gDebugEmitterHandle = 0;

		// Token: 0x040007DF RID: 2015
		public static float frame_mult = 1.5f;

		// Token: 0x040007E0 RID: 2016
		public static float GENIE_SMOKE_TRAIL_PARTICLE_REDUCTION_PERCENT = 0.5f;

		// Token: 0x040007E1 RID: 2017
		protected List<BGElementParams> mBGElementParams = new List<BGElementParams>();

		// Token: 0x040007E2 RID: 2018
		protected Gun mFrog;

		// Token: 0x040007E3 RID: 2019
		protected int mUpdateCount;

		// Token: 0x040007E4 RID: 2020
		protected int mState;

		// Token: 0x040007E5 RID: 2021
		protected int mBlinkCel;

		// Token: 0x040007E6 RID: 2022
		protected int mTimer;

		// Token: 0x040007E7 RID: 2023
		protected float mXDist;

		// Token: 0x040007E8 RID: 2024
		protected float mYDist;

		// Token: 0x040007E9 RID: 2025
		protected float mCurXDist;

		// Token: 0x040007EA RID: 2026
		protected float mCurYDist;

		// Token: 0x040007EB RID: 2027
		protected float mVX;

		// Token: 0x040007EC RID: 2028
		protected float mVY;

		// Token: 0x040007ED RID: 2029
		protected float mDarkFrogAlpha;

		// Token: 0x040007EE RID: 2030
		protected float mDarkFrogX;

		// Token: 0x040007EF RID: 2031
		protected float mDarkFrogY;

		// Token: 0x040007F0 RID: 2032
		protected float mDarkFrogVX;

		// Token: 0x040007F1 RID: 2033
		protected float mDarkFrogVY;

		// Token: 0x040007F2 RID: 2034
		protected float mXTrans;

		// Token: 0x040007F3 RID: 2035
		protected float mYTrans;

		// Token: 0x040007F4 RID: 2036
		protected float mTongueYOff;

		// Token: 0x040007F5 RID: 2037
		protected float mTattooAlpha;

		// Token: 0x040007F6 RID: 2038
		protected bool mMoveTongueDown;

		// Token: 0x040007F7 RID: 2039
		protected bool mDoTongueFlick;

		// Token: 0x040007F8 RID: 2040
		protected bool mFadingOut;

		// Token: 0x040007F9 RID: 2041
		protected bool mStartNextLevel;

		// Token: 0x040007FA RID: 2042
		protected LavaShader mBGShader;

		// Token: 0x040007FB RID: 2043
		protected List<AfterEffectsTimeline> mTimeline = new List<AfterEffectsTimeline>();

		// Token: 0x040007FC RID: 2044
		protected Component mSceneRotation;

		// Token: 0x040007FD RID: 2045
		protected SexyFramework.PIL.System mGenieSmoke;

		// Token: 0x040007FE RID: 2046
		protected SexyFramework.PIL.System mBoilingSmoke;

		// Token: 0x040007FF RID: 2047
		protected PIEffect mTransportFlash;

		// Token: 0x04000800 RID: 2048
		protected List<SimpleFadeText> mText = new List<SimpleFadeText>();

		// Token: 0x04000801 RID: 2049
		public int mInitialDelay;

		// Token: 0x04000802 RID: 2050
		public int mInitialDelayTarget;

		// Token: 0x04000803 RID: 2051
		private static float timer = 0f;

		// Token: 0x04000804 RID: 2052
		private static int CANVAS_W = 293;

		// Token: 0x04000805 RID: 2053
		private static int CANVAS_H = 268;

		// Token: 0x02000099 RID: 153
		public enum State
		{
			// Token: 0x04000807 RID: 2055
			State_MoveToPosition,
			// Token: 0x04000808 RID: 2056
			State_FreakingOut,
			// Token: 0x04000809 RID: 2057
			State_MovingForDialog,
			// Token: 0x0400080A RID: 2058
			State_Dialog,
			// Token: 0x0400080B RID: 2059
			State_Done
		}
	}
}
