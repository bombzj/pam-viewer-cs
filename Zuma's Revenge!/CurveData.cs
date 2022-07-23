using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x02000096 RID: 150
	public class CurveData
	{
		// Token: 0x060009C6 RID: 2502 RVA: 0x0005A920 File Offset: 0x00058B20
		public CurveData()
		{
			this.Clear();
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0005A94F File Offset: 0x00058B4F
		public virtual void Dispose()
		{
			this.mPointList = null;
			this.mVals = null;
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0005A95F File Offset: 0x00058B5F
		protected bool Fail(string theString)
		{
			this.mErrorString = theString;
			return false;
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0005A969 File Offset: 0x00058B69
		public bool Save(string theFilePath)
		{
			return false;
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0005A96C File Offset: 0x00058B6C
		public bool Load(string theFilePath)
		{
			this.Clear();
			SexyBuffer buffer = new SexyBuffer();
			if (!GameApp.gApp.ReadBufferFromStream(theFilePath + ".dat", ref buffer))
			{
				return false;
			}
			MemoryStream memoryStream = new MemoryStream(buffer.GetDataPtr());
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 4; i++)
			{
				stringBuilder.Append((char)binaryReader.ReadByte());
			}
			if (stringBuilder.ToString() != "CURV")
			{
				Console.WriteLine("Invalid file header");
				return false;
			}
			this.mVersion = binaryReader.ReadInt32();
			if (this.mVersion < 1 || this.mVersion > CurveData.gVersion)
			{
				Console.WriteLine("Invalid file header");
				return false;
			}
			if (this.mVersion >= 8)
			{
				this.mLinear = binaryReader.ReadBoolean();
			}
			if (this.mVersion >= 7)
			{
				this.mVals.mStartDistance = (int)binaryReader.ReadUInt32();
				this.mVals.mNumBalls = (int)binaryReader.ReadUInt32();
				this.mVals.mBallRepeat = (int)binaryReader.ReadUInt32();
				this.mVals.mMaxSingle = (int)binaryReader.ReadUInt32();
				this.mVals.mNumColors = (int)binaryReader.ReadUInt32();
				if (this.mVersion <= 10)
				{
					binaryReader.ReadInt32();
					binaryReader.ReadSingle();
				}
				this.mVals.mSpeed = binaryReader.ReadSingle();
				this.mVals.mSlowDistance = (int)binaryReader.ReadUInt32();
				this.mVals.mAccelerationRate = binaryReader.ReadSingle();
				this.mVals.mOrgAccelerationRate = this.mVals.mAccelerationRate;
				this.mVals.mMaxSpeed = binaryReader.ReadSingle();
				this.mVals.mOrgMaxSpeed = this.mVals.mMaxSpeed;
				this.mVals.mScoreTarget = (int)binaryReader.ReadUInt32();
				this.mVals.mSkullRotation = (int)binaryReader.ReadUInt32();
				this.mVals.mZumaBack = (int)binaryReader.ReadUInt32();
				this.mVals.mZumaSlow = (int)binaryReader.ReadUInt32();
				if (this.mVersion >= 13)
				{
					this.mVals.mSlowFactor = binaryReader.ReadSingle();
				}
				else
				{
					this.mVals.mSlowFactor = 4f;
				}
				if (this.mVersion >= 14)
				{
					this.mVals.mMaxClumpSize = (int)binaryReader.ReadUInt32();
				}
				else
				{
					this.mVals.mMaxClumpSize = 10;
				}
				int num = (int)binaryReader.ReadUInt32();
				for (int j = 0; j < 14; j++)
				{
					this.mVals.mPowerUpFreq[j] = 0;
					this.mVals.mMaxNumPowerUps[j] = 100000000;
				}
				int num2 = 0;
				while (num2 < num && num2 < 14)
				{
					if (Common.IsDeprecatedPowerUp((PowerType)num2))
					{
						this.mVals.mMaxNumPowerUps[num2] = 0;
						binaryReader.ReadInt32();
						if (this.mVersion >= 12)
						{
							binaryReader.ReadInt32();
						}
					}
					else
					{
						this.mVals.mPowerUpFreq[num2] = (int)binaryReader.ReadUInt32();
						if (this.mVersion >= 12)
						{
							this.mVals.mMaxNumPowerUps[num2] = (int)binaryReader.ReadUInt32();
						}
					}
					num2++;
				}
				if (this.mVersion >= 12)
				{
					this.mVals.mPowerUpChance = (int)binaryReader.ReadUInt32();
				}
				else
				{
					this.mVals.mPowerUpChance = 0;
				}
				this.mDrawCurve = binaryReader.ReadBoolean();
				this.mVals.mDrawTunnels = binaryReader.ReadBoolean();
				this.mVals.mDestroyAll = binaryReader.ReadBoolean();
				if (this.mVersion > 8)
				{
					this.mVals.mDrawPit = binaryReader.ReadBoolean();
				}
				if (this.mVersion > 9)
				{
					this.mVals.mDieAtEnd = binaryReader.ReadBoolean();
				}
			}
			bool flag = false;
			bool flag2 = true;
			if (this.mVersion >= 3)
			{
				flag = binaryReader.ReadBoolean();
				flag2 = binaryReader.ReadBoolean();
			}
			if (!flag)
			{
				this.mEditType = (int)binaryReader.ReadUInt32();
				int num3 = (int)binaryReader.ReadUInt32();
				if (num3 > 1000000)
				{
					Console.WriteLine("File is corrupt");
					return false;
				}
				binaryReader.ReadBytes(num3);
			}
			else
			{
				this.mEditType = 0;
			}
			int num4 = (int)binaryReader.ReadUInt32();
			if (num4 <= 0)
			{
				return true;
			}
			if (this.mVersion < 2)
			{
				for (int k = 0; k < num4; k++)
				{
					PathPoint pathPoint = new PathPoint();
					pathPoint.x = binaryReader.ReadSingle();
					pathPoint.y = binaryReader.ReadSingle();
					pathPoint.mInTunnel = binaryReader.ReadBoolean();
					pathPoint.mPriority = binaryReader.ReadByte();
					this.mPointList.Add(pathPoint);
				}
			}
			else if (this.mVersion < 4)
			{
				PathPoint pathPoint2 = new PathPoint();
				pathPoint2.x = binaryReader.ReadSingle();
				pathPoint2.y = binaryReader.ReadSingle();
				if (flag2)
				{
					pathPoint2.mInTunnel = binaryReader.ReadBoolean();
					pathPoint2.mPriority = binaryReader.ReadByte();
				}
				num4--;
				float x = pathPoint2.x;
				float y = pathPoint2.y;
				this.mPointList.Add(pathPoint2);
				for (int l = 0; l < num4; l++)
				{
					PathPoint pathPoint3 = new PathPoint();
					sbyte b = binaryReader.ReadSByte();
					sbyte b2 = binaryReader.ReadSByte();
					pathPoint3.x = x + (float)b * CurveData.INV_SUBPIXEL_MULT;
					pathPoint3.y = y + (float)b2 * CurveData.INV_SUBPIXEL_MULT;
					if (flag2)
					{
						pathPoint3.mInTunnel = binaryReader.ReadBoolean();
						pathPoint3.mPriority = binaryReader.ReadByte();
					}
					x = pathPoint3.x;
					y = pathPoint3.y;
					this.mPointList.Add(pathPoint3);
				}
			}
			else
			{
				float num5 = 0f;
				float num6 = 0f;
				for (int m = 0; m < num4; m++)
				{
					PathPoint pathPoint4 = new PathPoint();
					this.mPointList.Add(pathPoint4);
					byte b3 = binaryReader.ReadByte();
					pathPoint4.mInTunnel = (b3 & 1) != 0;
					bool flag3 = (b3 & 2) != 0;
					if (flag2 || this.mVersion >= 15)
					{
						pathPoint4.mPriority = binaryReader.ReadByte();
					}
					if (flag3)
					{
						pathPoint4.x = binaryReader.ReadSingle();
						pathPoint4.y = binaryReader.ReadSingle();
					}
					else
					{
						sbyte b4 = binaryReader.ReadSByte();
						sbyte b5 = binaryReader.ReadSByte();
						pathPoint4.x = num5 + (float)b4 * CurveData.INV_SUBPIXEL_MULT;
						pathPoint4.y = num6 + (float)b5 * CurveData.INV_SUBPIXEL_MULT;
					}
					num5 = pathPoint4.x;
					num6 = pathPoint4.y;
				}
			}
			return true;
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0005AFB9 File Offset: 0x000591B9
		public void Copy(CurveData dest)
		{
			dest.mDrawCurve = this.mDrawCurve;
			dest.mVals = this.mVals;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0005AFD4 File Offset: 0x000591D4
		public void Clear()
		{
			this.mPointList.Clear();
			this.mEditType = 0;
			this.mLinear = false;
			this.mVals.mDieAtEnd = true;
			this.mVals.mStartDistance = 50;
			this.mVals.mNumBalls = 0;
			this.mVals.mBallRepeat = 50;
			this.mVals.mMaxSingle = 10;
			this.mVals.mNumColors = 4;
			this.mVals.mSlowDistance = 200;
			this.mVals.mScoreTarget = 1000;
			this.mVals.mSkullRotation = -1;
			this.mVals.mZumaBack = 300;
			this.mVals.mZumaSlow = 1100;
			this.mVals.mSlowFactor = 4f;
			this.mVals.mMaxClumpSize = 10;
			this.mVals.mSpeed = 0.5f;
			this.mVals.mAccelerationRate = 0f;
			this.mVals.mMaxSpeed = 100f;
			for (int i = 0; i < 14; i++)
			{
				this.mVals.mPowerUpFreq[i] = 0;
				this.mVals.mMaxNumPowerUps[i] = 100000000;
			}
			this.mVals.mPowerUpChance = 1200;
			this.mVals.mDrawPit = true;
			this.mDrawCurve = true;
			this.mVals.mDrawTunnels = true;
			this.mVals.mDestroyAll = true;
		}

		// Token: 0x040007C6 RID: 1990
		public static int gVersion = 15;

		// Token: 0x040007C7 RID: 1991
		public static float SUBPIXEL_MULT = 100f;

		// Token: 0x040007C8 RID: 1992
		public static float INV_SUBPIXEL_MULT = 1f / CurveData.SUBPIXEL_MULT;

		// Token: 0x040007C9 RID: 1993
		public List<PathPoint> mPointList = new List<PathPoint>();

		// Token: 0x040007CA RID: 1994
		public int mEditType;

		// Token: 0x040007CB RID: 1995
		public int mVersion = 268435455;

		// Token: 0x040007CC RID: 1996
		public string mErrorString;

		// Token: 0x040007CD RID: 1997
		public BasicCurveVals mVals = new BasicCurveVals();

		// Token: 0x040007CE RID: 1998
		public bool mDrawCurve;

		// Token: 0x040007CF RID: 1999
		public bool mLinear;
	}
}
