using System;
using System.Collections.Generic;
using JeffLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x02000050 RID: 80
	public class DataSync : DataSyncBase
	{
		// Token: 0x060006AF RID: 1711 RVA: 0x0002DD20 File Offset: 0x0002BF20
		public DataSync(SexyBuffer buffer, bool isRead)
		{
			this.ResetPointerTable();
			this.m_buffer = buffer;
			this.m_isRead = isRead;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0002DDB1 File Offset: 0x0002BFB1
		public SexyBuffer GetBuffer()
		{
			return this.m_buffer;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0002DDB9 File Offset: 0x0002BFB9
		public bool isRead()
		{
			return this.m_isRead;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0002DDC1 File Offset: 0x0002BFC1
		public bool isWrite()
		{
			return !this.isRead();
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0002DDCC File Offset: 0x0002BFCC
		public void SyncBoolean(ref bool theBool)
		{
			if (this.m_isRead)
			{
				theBool = this.m_buffer.ReadBoolean();
				return;
			}
			this.m_buffer.WriteBoolean(theBool);
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0002DDF1 File Offset: 0x0002BFF1
		public void SyncShort(ref short theInt)
		{
			if (this.m_isRead)
			{
				theInt = this.m_buffer.ReadShort();
				return;
			}
			this.m_buffer.WriteShort(theInt);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0002DE16 File Offset: 0x0002C016
		public override void SyncLong(ref int theInt)
		{
			if (this.m_isRead)
			{
				theInt = (int)this.m_buffer.ReadLong();
				return;
			}
			this.m_buffer.WriteLong((long)theInt);
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0002DE3D File Offset: 0x0002C03D
		public void SyncLong(ref uint theInt)
		{
			if (this.m_isRead)
			{
				theInt = (uint)this.m_buffer.ReadLong();
				return;
			}
			this.m_buffer.WriteLong((long)((ulong)theInt));
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0002DE64 File Offset: 0x0002C064
		public void SyncLong(ref ushort theInt)
		{
			if (this.m_isRead)
			{
				theInt = (ushort)this.m_buffer.ReadLong();
				return;
			}
			this.m_buffer.WriteLong((long)((ulong)theInt));
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0002DE8B File Offset: 0x0002C08B
		public void SyncLong(ref long theLong)
		{
			if (this.m_isRead)
			{
				theLong = this.m_buffer.ReadLong();
				return;
			}
			this.m_buffer.WriteLong(theLong);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0002DEB0 File Offset: 0x0002C0B0
		public override void SyncFloat(ref float theFloat)
		{
			if (this.m_isRead)
			{
				theFloat = this.m_buffer.ReadFloat();
				return;
			}
			this.m_buffer.WriteFloat(theFloat);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0002DED8 File Offset: 0x0002C0D8
		public override void SyncListInt(List<int> theList)
		{
			if (this.m_isRead)
			{
				theList.Clear();
				long num = this.m_buffer.ReadLong();
				int num2 = 0;
				while ((long)num2 < num)
				{
					theList.Add((int)this.m_buffer.ReadLong());
					num2++;
				}
				return;
			}
			this.m_buffer.WriteLong((long)theList.Count);
			foreach (int num3 in theList)
			{
				this.m_buffer.WriteLong((long)num3);
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0002DF78 File Offset: 0x0002C178
		public override void SyncListFloat(List<float> theList)
		{
			if (this.m_isRead)
			{
				theList.Clear();
				long num = this.m_buffer.ReadLong();
				int num2 = 0;
				while ((long)num2 < num)
				{
					theList.Add(this.m_buffer.ReadFloat());
					num2++;
				}
				return;
			}
			this.m_buffer.WriteLong((long)theList.Count);
			foreach (float num3 in theList)
			{
				float theFloat = num3;
				this.m_buffer.WriteFloat(theFloat);
			}
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0002E018 File Offset: 0x0002C218
		private void ResetPointerTable()
		{
			this.mCurPointerIndex = 2;
			this.mIntToPointerMap_CurveMgr.Clear();
			this.mIntToPointerMap_Ball.Clear();
			this.mIntToPointerMap_Bullet.Clear();
			this.mPointerToIntMap_CurveMgr.Clear();
			this.mPointerToIntMap_Ball.Clear();
			this.mPointerToIntMap_Bullet.Clear();
			this.mPointerSyncList_ReversePowerEffect.Clear();
			this.mPointerSyncList_Ball.Clear();
			this.mPointerSyncList_Bullet.Clear();
			this.mIntToPointerMap_CurveMgr.Add(0, null);
			this.mIntToPointerMap_Ball.Add(0, null);
			this.mIntToPointerMap_Bullet.Add(0, null);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0002E0B8 File Offset: 0x0002C2B8
		public bool RegisterPointer(CurveMgr thePtr)
		{
			if (!this.mPointerToIntMap_CurveMgr.ContainsKey(thePtr))
			{
				int num = this.mCurPointerIndex++;
				this.mPointerToIntMap_CurveMgr.Add(thePtr, num);
				this.mIntToPointerMap_CurveMgr.Add(num, thePtr);
				return true;
			}
			return false;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0002E104 File Offset: 0x0002C304
		public bool RegisterPointer(Ball thePtr)
		{
			if (!this.mPointerToIntMap_Ball.ContainsKey(thePtr))
			{
				int num = this.mCurPointerIndex++;
				this.mPointerToIntMap_Ball.Add(thePtr, num);
				this.mIntToPointerMap_Ball.Add(num, thePtr);
				return true;
			}
			return false;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0002E150 File Offset: 0x0002C350
		public bool RegisterPointer(Bullet thePtr)
		{
			if (!this.mPointerToIntMap_Bullet.ContainsKey(thePtr))
			{
				int num = this.mCurPointerIndex++;
				this.mPointerToIntMap_Bullet.Add(thePtr, num);
				this.mIntToPointerMap_Bullet.Add(num, thePtr);
				return true;
			}
			return false;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0002E19A File Offset: 0x0002C39A
		public void SyncPointer(ReversePowerEffect thePtr)
		{
			this.mPointerSyncList_ReversePowerEffect.Add(thePtr);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0002E1A8 File Offset: 0x0002C3A8
		public void SyncPointer(Ball thePtr)
		{
			this.mPointerSyncList_Ball.Add(thePtr);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0002E1B6 File Offset: 0x0002C3B6
		public void SyncPointer(Bullet thePtr)
		{
			this.mPointerSyncList_Bullet.Add(thePtr);
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0002E1C4 File Offset: 0x0002C3C4
		public void SyncPointers()
		{
			if (this.m_isRead)
			{
				foreach (ReversePowerEffect reversePowerEffect in this.mPointerSyncList_ReversePowerEffect)
				{
					int num = (int)this.m_buffer.ReadLong();
					reversePowerEffect.mCurve = this.mIntToPointerMap_CurveMgr[num];
				}
				foreach (Ball ball in this.mPointerSyncList_Ball)
				{
					int num2 = (int)this.m_buffer.ReadLong();
					ball.mBullet = this.mIntToPointerMap_Bullet[num2];
				}
				using (List<Bullet>.Enumerator enumerator3 = this.mPointerSyncList_Bullet.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Bullet bullet = enumerator3.Current;
						int num3 = (int)this.m_buffer.ReadLong();
						bullet.mHitBall = this.mIntToPointerMap_Ball[num3];
					}
					goto IL_258;
				}
			}
			foreach (ReversePowerEffect reversePowerEffect2 in this.mPointerSyncList_ReversePowerEffect)
			{
				int num4 = 0;
				if (reversePowerEffect2.mCurve != null && this.mPointerToIntMap_CurveMgr.ContainsKey(reversePowerEffect2.mCurve))
				{
					num4 = this.mPointerToIntMap_CurveMgr[reversePowerEffect2.mCurve];
				}
				this.m_buffer.WriteLong((long)num4);
			}
			foreach (Ball ball2 in this.mPointerSyncList_Ball)
			{
				int num5 = 0;
				if (ball2.mBullet != null && this.mPointerToIntMap_Bullet.ContainsKey(ball2.mBullet))
				{
					num5 = this.mPointerToIntMap_Bullet[ball2.mBullet];
				}
				this.m_buffer.WriteLong((long)num5);
			}
			foreach (Bullet bullet2 in this.mPointerSyncList_Bullet)
			{
				int num6 = 0;
				if (bullet2.mHitBall != null && this.mPointerToIntMap_Ball.ContainsKey(bullet2.mHitBall))
				{
					num6 = this.mPointerToIntMap_Ball[bullet2.mHitBall];
				}
				this.m_buffer.WriteLong((long)num6);
			}
			IL_258:
			this.ResetPointerTable();
		}

		// Token: 0x040003EB RID: 1003
		private SexyBuffer m_buffer;

		// Token: 0x040003EC RID: 1004
		private bool m_isRead = true;

		// Token: 0x040003ED RID: 1005
		private int mCurPointerIndex;

		// Token: 0x040003EE RID: 1006
		private Dictionary<CurveMgr, int> mPointerToIntMap_CurveMgr = new Dictionary<CurveMgr, int>();

		// Token: 0x040003EF RID: 1007
		private Dictionary<int, CurveMgr> mIntToPointerMap_CurveMgr = new Dictionary<int, CurveMgr>();

		// Token: 0x040003F0 RID: 1008
		private List<ReversePowerEffect> mPointerSyncList_ReversePowerEffect = new List<ReversePowerEffect>();

		// Token: 0x040003F1 RID: 1009
		private Dictionary<Ball, int> mPointerToIntMap_Ball = new Dictionary<Ball, int>();

		// Token: 0x040003F2 RID: 1010
		private Dictionary<int, Ball> mIntToPointerMap_Ball = new Dictionary<int, Ball>();

		// Token: 0x040003F3 RID: 1011
		private List<Bullet> mPointerSyncList_Bullet = new List<Bullet>();

		// Token: 0x040003F4 RID: 1012
		private Dictionary<Bullet, int> mPointerToIntMap_Bullet = new Dictionary<Bullet, int>();

		// Token: 0x040003F5 RID: 1013
		private Dictionary<int, Bullet> mIntToPointerMap_Bullet = new Dictionary<int, Bullet>();

		// Token: 0x040003F6 RID: 1014
		private List<Ball> mPointerSyncList_Ball = new List<Ball>();
	}
}
