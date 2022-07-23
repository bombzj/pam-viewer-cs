using System;
using System.Collections.Generic;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x02000020 RID: 32
	public abstract class RenderStateManager
	{
		// Token: 0x0600018A RID: 394 RVA: 0x0000567C File Offset: 0x0000387C
		protected virtual RenderStateManager.State.FCommitFunc GetCommitFunc(RenderStateManager.State inState)
		{
			return null;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00005680 File Offset: 0x00003880
		public RenderStateManager()
		{
			this.mDirtyDummyHead = new RenderStateManager.State();
			this.mContextDefDummyHead = new RenderStateManager.State();
			this.mWouldCommitStateDirty = false;
			this.mWouldCommitStateResult = false;
			this.mCurrentContext = this.mDefaultContext;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000056CE File Offset: 0x000038CE
		public virtual void Dispose()
		{
		}

		// Token: 0x0600018D RID: 397
		public abstract void Init();

		// Token: 0x0600018E RID: 398
		public abstract void Reset();

		// Token: 0x0600018F RID: 399 RVA: 0x000056D0 File Offset: 0x000038D0
		public virtual void Cleanup()
		{
			this.mCurrentContext.Unacquire();
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000056DD File Offset: 0x000038DD
		public bool IsDirty()
		{
			return this.mDirtyDummyHead.mDirtyNext != this.mDirtyDummyHead;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000056F8 File Offset: 0x000038F8
		public void ApplyContextDefaults()
		{
			for (RenderStateManager.State mContextDefNext = this.mContextDefDummyHead.mContextDefNext; mContextDefNext != this.mContextDefDummyHead; mContextDefNext = mContextDefNext.mContextDefNext)
			{
				mContextDefNext.mValue = mContextDefNext.mContextDefaultValue;
				mContextDefNext.SetDirty();
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00005738 File Offset: 0x00003938
		public bool WouldCommitState()
		{
			if (!this.mWouldCommitStateDirty)
			{
				return this.mWouldCommitStateResult;
			}
			this.mWouldCommitStateDirty = false;
			for (RenderStateManager.State mDirtyNext = this.mDirtyDummyHead.mDirtyNext; mDirtyNext != this.mDirtyDummyHead; mDirtyNext = mDirtyNext.mDirtyNext)
			{
				if (!(mDirtyNext.mValue == mDirtyNext.mLastCommittedValue) && mDirtyNext.mCommitFunc != null)
				{
					this.mWouldCommitStateResult = true;
					return this.mWouldCommitStateResult;
				}
			}
			this.mWouldCommitStateResult = false;
			return this.mWouldCommitStateResult;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000057B0 File Offset: 0x000039B0
		public bool CommitState()
		{
			bool flag = true;
			while (this.mDirtyDummyHead.mDirtyNext != this.mDirtyDummyHead)
			{
				RenderStateManager.State mDirtyNext = this.mDirtyDummyHead.mDirtyNext;
				if (mDirtyNext.mValue == mDirtyNext.mLastCommittedValue)
				{
					mDirtyNext.ClearDirty();
				}
				else
				{
					if (mDirtyNext.mCommitFunc != null)
					{
						flag &= mDirtyNext.mCommitFunc(mDirtyNext);
					}
					else
					{
						mDirtyNext.ClearDirty();
					}
					mDirtyNext.mLastCommittedValue = mDirtyNext.mValue;
				}
			}
			return flag;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00005827 File Offset: 0x00003A27
		public virtual void Flush()
		{
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00005829 File Offset: 0x00003A29
		public RenderStateManager.Context GetContext()
		{
			return this.mCurrentContext;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00005834 File Offset: 0x00003A34
		public void SetContext(RenderStateManager.Context inContext)
		{
			if (inContext == null)
			{
				inContext = this.mDefaultContext;
			}
			if (inContext == this.mCurrentContext)
			{
				return;
			}
			if (this.mCurrentContext.mParentContext == inContext)
			{
				this.mCurrentContext.Unacquire(true);
				this.mCurrentContext = inContext;
				return;
			}
			if (inContext.mParentContext == this.mCurrentContext)
			{
				this.mCurrentContext = inContext;
				this.mCurrentContext.Reacquire(true);
				return;
			}
			this.mCurrentContext.Unacquire();
			this.mCurrentContext = inContext;
			this.mCurrentContext.Reacquire();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000058B7 File Offset: 0x00003AB7
		public void RevertState()
		{
			this.mCurrentContext.RevertState();
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000058C4 File Offset: 0x00003AC4
		public virtual void PushState()
		{
			this.mCurrentContext.PushState();
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000058D1 File Offset: 0x00003AD1
		public virtual void PopState()
		{
			this.mCurrentContext.PopState();
		}

		// Token: 0x04000059 RID: 89
		protected RenderStateManager.State mDirtyDummyHead;

		// Token: 0x0400005A RID: 90
		protected RenderStateManager.State mContextDefDummyHead;

		// Token: 0x0400005B RID: 91
		protected RenderStateManager.Context mCurrentContext;

		// Token: 0x0400005C RID: 92
		protected RenderStateManager.Context mDefaultContext = new RenderStateManager.Context();

		// Token: 0x0400005D RID: 93
		protected bool mWouldCommitStateDirty;

		// Token: 0x0400005E RID: 94
		protected bool mWouldCommitStateResult;

		// Token: 0x02000021 RID: 33
		public class StateValue
		{
			// Token: 0x0600019A RID: 410 RVA: 0x000058DE File Offset: 0x00003ADE
			public StateValue()
			{
			}

			// Token: 0x0600019B RID: 411 RVA: 0x000058E6 File Offset: 0x00003AE6
			public StateValue(uint inDword)
			{
				this.mType = RenderStateManager.StateValue.EStateValueType.SV_Dword;
				this.mDword = inDword;
			}

			// Token: 0x0600019C RID: 412 RVA: 0x000058FC File Offset: 0x00003AFC
			public StateValue(float inFloat)
			{
				this.mType = RenderStateManager.StateValue.EStateValueType.SV_Float;
				this.mFloat = inFloat;
			}

			// Token: 0x0600019D RID: 413 RVA: 0x00005912 File Offset: 0x00003B12
			public StateValue(object inPtr)
			{
				this.mType = RenderStateManager.StateValue.EStateValueType.SV_Ptr;
				this.mPtr = inPtr;
			}

			// Token: 0x0600019E RID: 414 RVA: 0x00005928 File Offset: 0x00003B28
			public StateValue(float inX, float inY, float inZ, float inW)
			{
				this.mType = RenderStateManager.StateValue.EStateValueType.SV_Vector;
				this.mX = inX;
				this.mY = inY;
				this.mZ = inZ;
				this.mW = inW;
			}

			// Token: 0x0600019F RID: 415 RVA: 0x00005954 File Offset: 0x00003B54
			public StateValue(RenderStateManager.StateValue inValue)
			{
				this.mType = inValue.mType;
				switch (this.mType)
				{
				case RenderStateManager.StateValue.EStateValueType.SV_Dword:
					this.mDword = inValue.mDword;
					return;
				case RenderStateManager.StateValue.EStateValueType.SV_Float:
					this.mFloat = inValue.mFloat;
					return;
				case RenderStateManager.StateValue.EStateValueType.SV_Ptr:
					this.mPtr = inValue.mPtr;
					return;
				case RenderStateManager.StateValue.EStateValueType.SV_Vector:
					this.mX = inValue.mX;
					this.mY = inValue.mY;
					this.mZ = inValue.mZ;
					this.mW = inValue.mW;
					return;
				default:
					return;
				}
			}

			// Token: 0x060001A0 RID: 416 RVA: 0x000059E8 File Offset: 0x00003BE8
			public override bool Equals(object obj)
			{
				if (obj == null || !(obj is RenderStateManager.StateValue))
				{
					return false;
				}
				RenderStateManager.StateValue stateValue = (RenderStateManager.StateValue)obj;
				switch (this.mType)
				{
				case RenderStateManager.StateValue.EStateValueType.SV_Dword:
					return this.mDword == stateValue.mDword;
				case RenderStateManager.StateValue.EStateValueType.SV_Float:
					return this.mFloat == stateValue.mFloat;
				case RenderStateManager.StateValue.EStateValueType.SV_Ptr:
					return this.mPtr == stateValue.mPtr;
				case RenderStateManager.StateValue.EStateValueType.SV_Vector:
					return this.mX == stateValue.mX && this.mY == stateValue.mY && this.mZ == stateValue.mZ && this.mW == stateValue.mW;
				default:
					return false;
				}
			}

			// Token: 0x060001A1 RID: 417 RVA: 0x00005A97 File Offset: 0x00003C97
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x060001A2 RID: 418 RVA: 0x00005A9F File Offset: 0x00003C9F
			public uint GetDword()
			{
				return this.mDword;
			}

			// Token: 0x060001A3 RID: 419 RVA: 0x00005AA7 File Offset: 0x00003CA7
			public float GetFloat()
			{
				return this.mFloat;
			}

			// Token: 0x060001A4 RID: 420 RVA: 0x00005AAF File Offset: 0x00003CAF
			public object GetPtr()
			{
				return this.mPtr;
			}

			// Token: 0x060001A5 RID: 421 RVA: 0x00005AB7 File Offset: 0x00003CB7
			public void GetVector(ref float outX, ref float outY, ref float outZ, ref float outW)
			{
				outX = this.mX;
				outY = this.mY;
				outZ = this.mZ;
				outW = this.mW;
			}

			// Token: 0x060001A6 RID: 422 RVA: 0x00005ADA File Offset: 0x00003CDA
			public static bool operator ==(RenderStateManager.StateValue ImpliedObject, RenderStateManager.StateValue inValue)
			{
				if (ImpliedObject == null)
				{
					return inValue == null;
				}
				return ImpliedObject.Equals(inValue);
			}

			// Token: 0x060001A7 RID: 423 RVA: 0x00005AEB File Offset: 0x00003CEB
			public static bool operator !=(RenderStateManager.StateValue ImpliedObject, RenderStateManager.StateValue inValue)
			{
				return !(ImpliedObject == inValue);
			}

			// Token: 0x0400005F RID: 95
			public RenderStateManager.StateValue.EStateValueType mType;

			// Token: 0x04000060 RID: 96
			public uint mDword;

			// Token: 0x04000061 RID: 97
			public float mFloat;

			// Token: 0x04000062 RID: 98
			public object mPtr;

			// Token: 0x04000063 RID: 99
			public float mX;

			// Token: 0x04000064 RID: 100
			public float mY;

			// Token: 0x04000065 RID: 101
			public float mZ;

			// Token: 0x04000066 RID: 102
			public float mW;

			// Token: 0x02000022 RID: 34
			public enum EStateValueType
			{
				// Token: 0x04000068 RID: 104
				SV_Dword,
				// Token: 0x04000069 RID: 105
				SV_Float,
				// Token: 0x0400006A RID: 106
				SV_Ptr,
				// Token: 0x0400006B RID: 107
				SV_Vector
			}
		}

		// Token: 0x02000023 RID: 35
		public class State
		{
			// Token: 0x060001A8 RID: 424 RVA: 0x00005AF7 File Offset: 0x00003CF7
			public State(RenderStateManager inManager, uint inContext0, uint inContext1, uint inContext2)
				: this(inManager, inContext0, inContext1, inContext2, 0U)
			{
			}

			// Token: 0x060001A9 RID: 425 RVA: 0x00005B05 File Offset: 0x00003D05
			public State(RenderStateManager inManager, uint inContext0, uint inContext1)
				: this(inManager, inContext0, inContext1, 0U, 0U)
			{
			}

			// Token: 0x060001AA RID: 426 RVA: 0x00005B12 File Offset: 0x00003D12
			public State(RenderStateManager inManager, uint inContext0)
				: this(inManager, inContext0, 0U, 0U, 0U)
			{
			}

			// Token: 0x060001AB RID: 427 RVA: 0x00005B1F File Offset: 0x00003D1F
			public State(RenderStateManager inManager)
				: this(inManager, 0U, 0U, 0U, 0U)
			{
			}

			// Token: 0x060001AC RID: 428 RVA: 0x00005B2C File Offset: 0x00003D2C
			public State()
				: this(null, 0U, 0U, 0U, 0U)
			{
			}

			// Token: 0x060001AD RID: 429 RVA: 0x00005B3C File Offset: 0x00003D3C
			public State(RenderStateManager inManager, uint inContext0, uint inContext1, uint inContext2, uint inContext3)
			{
				this.mContext = new uint[4];
				this.mValue = new RenderStateManager.StateValue();
				this.mHardwareDefaultValue = new RenderStateManager.StateValue();
				this.mContextDefaultValue = new RenderStateManager.StateValue();
				this.mLastCommittedValue = new RenderStateManager.StateValue();
				this.mManager = inManager;
				this.mCommitFunc = null;
				this.mDirtyNext = this;
				this.mDirtyPrev = this;
				this.mContextDefNext = this;
				this.mContextDefPrev = this;
				this.mContext[0] = inContext0;
				this.mContext[1] = inContext1;
				this.mContext[2] = inContext2;
				this.mContext[3] = inContext3;
			}

			// Token: 0x060001AE RID: 430 RVA: 0x00005BDC File Offset: 0x00003DDC
			public State(RenderStateManager.State inState)
			{
				this.mContext = new uint[4];
				this.mValue = new RenderStateManager.StateValue();
				this.mHardwareDefaultValue = new RenderStateManager.StateValue();
				this.mContextDefaultValue = new RenderStateManager.StateValue();
				this.mLastCommittedValue = new RenderStateManager.StateValue();
				this.mManager = inState.mManager;
				this.mValue = new RenderStateManager.StateValue(inState.mValue);
				this.mHardwareDefaultValue = new RenderStateManager.StateValue(inState.mHardwareDefaultValue);
				this.mContextDefaultValue = new RenderStateManager.StateValue(inState.mContextDefaultValue);
				this.mLastCommittedValue = new RenderStateManager.StateValue(inState.mLastCommittedValue);
				this.mCommitFunc = inState.mCommitFunc;
				this.mDirtyNext = this;
				this.mDirtyPrev = this;
				this.mContextDefNext = this;
				this.mContextDefPrev = this;
				for (int i = 0; i < 4; i++)
				{
					this.mContext[i] = inState.mContext[i];
				}
			}

			// Token: 0x060001AF RID: 431 RVA: 0x00005CBF File Offset: 0x00003EBF
			public void Init(RenderStateManager.StateValue inDefaultValue, string inName)
			{
				this.Init(inDefaultValue, inName, null);
			}

			// Token: 0x060001B0 RID: 432 RVA: 0x00005CCA File Offset: 0x00003ECA
			public void Init(ulong inDefaultValue, string inName)
			{
				this.Init(new RenderStateManager.StateValue(inDefaultValue), inName);
			}

			// Token: 0x060001B1 RID: 433 RVA: 0x00005CDC File Offset: 0x00003EDC
			public void Init(RenderStateManager.StateValue inDefaultValue, string inName, string inValueEnumName)
			{
				this.mLastCommittedValue = inDefaultValue;
				this.mContextDefaultValue = inDefaultValue;
				this.mHardwareDefaultValue = inDefaultValue;
				this.mValue = inDefaultValue;
				this.mCommitFunc = this.mManager.GetCommitFunc(this);
				this.mName = inName;
			}

			// Token: 0x060001B2 RID: 434 RVA: 0x00005D24 File Offset: 0x00003F24
			public void Init(ulong inDefaultValue, string inName, string inValueEnumName)
			{
				this.Init(new RenderStateManager.StateValue(inDefaultValue), inName, inValueEnumName);
			}

			// Token: 0x060001B3 RID: 435 RVA: 0x00005D36 File Offset: 0x00003F36
			public void Init(RenderStateManager.StateValue inHardwareDefaultValue, RenderStateManager.StateValue inContextDefaultValue, string inName)
			{
				this.Init(inHardwareDefaultValue, inContextDefaultValue, inName, null);
			}

			// Token: 0x060001B4 RID: 436 RVA: 0x00005D42 File Offset: 0x00003F42
			public void Init(ulong inHardwareDefaultValue, ulong inContextDefaultValue, string inName)
			{
				this.Init(new RenderStateManager.StateValue(inHardwareDefaultValue), new RenderStateManager.StateValue(inContextDefaultValue), inName);
			}

			// Token: 0x060001B5 RID: 437 RVA: 0x00005D5C File Offset: 0x00003F5C
			public void Init(RenderStateManager.StateValue inHardwareDefaultValue, RenderStateManager.StateValue inContextDefaultValue, string inName, string inValueEnumName)
			{
				this.mLastCommittedValue = inHardwareDefaultValue;
				this.mHardwareDefaultValue = inHardwareDefaultValue;
				this.mValue = inHardwareDefaultValue;
				this.mContextDefaultValue = inContextDefaultValue;
				this.mCommitFunc = this.mManager.GetCommitFunc(this);
				this.mName = inName;
				this.mContextDefPrev = this.mManager.mContextDefDummyHead;
				this.mContextDefNext = this.mManager.mContextDefDummyHead.mContextDefNext;
				RenderStateManager.State state = this.mContextDefPrev;
				this.mContextDefNext.mContextDefPrev = this;
				state.mContextDefNext = this;
			}

			// Token: 0x060001B6 RID: 438 RVA: 0x00005DE3 File Offset: 0x00003FE3
			public void Init(ulong inHardwareDefaultValue, ulong inContextDefaultValue, string inName, string inValueEnumName)
			{
				this.Init(new RenderStateManager.StateValue(inHardwareDefaultValue), new RenderStateManager.StateValue(inContextDefaultValue), inName, inValueEnumName);
			}

			// Token: 0x060001B7 RID: 439 RVA: 0x00005DFE File Offset: 0x00003FFE
			public void Reset()
			{
				this.mLastCommittedValue = this.mHardwareDefaultValue;
			}

			// Token: 0x060001B8 RID: 440 RVA: 0x00005E0C File Offset: 0x0000400C
			public bool HasContextDefault()
			{
				return this.mContextDefPrev != this;
			}

			// Token: 0x060001B9 RID: 441 RVA: 0x00005E1A File Offset: 0x0000401A
			public bool IsDirty()
			{
				return this.mDirtyPrev != this;
			}

			// Token: 0x060001BA RID: 442 RVA: 0x00005E28 File Offset: 0x00004028
			public void SetDirty()
			{
				if (this.IsDirty())
				{
					return;
				}
				this.mDirtyPrev = this.mManager.mDirtyDummyHead;
				this.mDirtyNext = this.mManager.mDirtyDummyHead.mDirtyNext;
				RenderStateManager.State state = this.mDirtyPrev;
				this.mDirtyNext.mDirtyPrev = this;
				state.mDirtyNext = this;
				this.mManager.mWouldCommitStateDirty = true;
			}

			// Token: 0x060001BB RID: 443 RVA: 0x00005E8B File Offset: 0x0000408B
			public void ClearDirty()
			{
				this.ClearDirty(false);
			}

			// Token: 0x060001BC RID: 444 RVA: 0x00005E94 File Offset: 0x00004094
			public void ClearDirty(bool inActAsCommit)
			{
				if (!this.IsDirty())
				{
					return;
				}
				if (inActAsCommit)
				{
					this.mLastCommittedValue = this.mValue;
				}
				this.mDirtyPrev.mDirtyNext = this.mDirtyNext;
				this.mDirtyNext.mDirtyPrev = this.mDirtyPrev;
				this.mDirtyNext = this;
				this.mDirtyPrev = this;
				this.mManager.mWouldCommitStateDirty = true;
			}

			// Token: 0x060001BD RID: 445 RVA: 0x00005EF8 File Offset: 0x000040F8
			public void SetValue(RenderStateManager.StateValue inValue)
			{
				if (inValue == this.mValue)
				{
					return;
				}
				this.mManager.Flush();
				if (this.mManager.mCurrentContext != null)
				{
					this.mManager.mCurrentContext.SplitChildren();
					this.mManager.mCurrentContext.mJournal.Add(new RenderStateManager.Context.JournalEntry(this, this.mValue, inValue));
				}
				this.mValue = inValue;
				this.SetDirty();
			}

			// Token: 0x060001BE RID: 446 RVA: 0x00005F6B File Offset: 0x0000416B
			public void SetValue(uint inDword)
			{
				this.SetValue(new RenderStateManager.StateValue(inDword));
			}

			// Token: 0x060001BF RID: 447 RVA: 0x00005F79 File Offset: 0x00004179
			public void SetValue(float inFloat)
			{
				this.SetValue(new RenderStateManager.StateValue(inFloat));
			}

			// Token: 0x060001C0 RID: 448 RVA: 0x00005F87 File Offset: 0x00004187
			public void SetValue(IntPtr inPtr)
			{
				this.SetValue(new RenderStateManager.StateValue(inPtr));
			}

			// Token: 0x060001C1 RID: 449 RVA: 0x00005F9A File Offset: 0x0000419A
			public void SetValue(float inX, float inY, float inZ, float inW)
			{
				this.SetValue(new RenderStateManager.StateValue(inX, inY, inZ, inW));
			}

			// Token: 0x060001C2 RID: 450 RVA: 0x00005FAC File Offset: 0x000041AC
			public uint GetDword()
			{
				return this.mValue.GetDword();
			}

			// Token: 0x060001C3 RID: 451 RVA: 0x00005FB9 File Offset: 0x000041B9
			public float GetFloat()
			{
				return this.mValue.GetFloat();
			}

			// Token: 0x060001C4 RID: 452 RVA: 0x00005FC6 File Offset: 0x000041C6
			public object GetPtr()
			{
				return this.mValue.GetPtr();
			}

			// Token: 0x060001C5 RID: 453 RVA: 0x00005FD3 File Offset: 0x000041D3
			public void GetVector(ref float outX, ref float outY, ref float outZ, ref float outW)
			{
				this.mValue.GetVector(ref outX, ref outY, ref outZ, ref outW);
			}

			// Token: 0x0400006C RID: 108
			public RenderStateManager mManager;

			// Token: 0x0400006D RID: 109
			public uint[] mContext;

			// Token: 0x0400006E RID: 110
			public RenderStateManager.State mDirtyPrev;

			// Token: 0x0400006F RID: 111
			public RenderStateManager.State mDirtyNext;

			// Token: 0x04000070 RID: 112
			public RenderStateManager.StateValue mValue;

			// Token: 0x04000071 RID: 113
			public RenderStateManager.StateValue mHardwareDefaultValue;

			// Token: 0x04000072 RID: 114
			public RenderStateManager.StateValue mContextDefaultValue;

			// Token: 0x04000073 RID: 115
			public RenderStateManager.StateValue mLastCommittedValue;

			// Token: 0x04000074 RID: 116
			public RenderStateManager.State mContextDefPrev;

			// Token: 0x04000075 RID: 117
			public RenderStateManager.State mContextDefNext;

			// Token: 0x04000076 RID: 118
			public RenderStateManager.State.FCommitFunc mCommitFunc;

			// Token: 0x04000077 RID: 119
			public string mName;

			// Token: 0x02000024 RID: 36
			// (Invoke) Token: 0x060001C7 RID: 455
			public delegate bool FCommitFunc(RenderStateManager.State inState);
		}

		// Token: 0x02000025 RID: 37
		public class Context
		{
			// Token: 0x060001CA RID: 458 RVA: 0x00005FE5 File Offset: 0x000041E5
			public Context()
			{
				this.mParentContext = null;
				this.mJournalFloor = 0U;
				this.mParentContext = null;
			}

			// Token: 0x060001CB RID: 459 RVA: 0x00006024 File Offset: 0x00004224
			public virtual void Dispose()
			{
				this.SplitChildren();
				if (this.mParentContext != null)
				{
					int count = this.mParentContext.mChildContexts.Count;
					for (int i = 0; i < count; i++)
					{
						if (this.mParentContext.mChildContexts[i] == this)
						{
							this.mParentContext.mChildContexts.RemoveAt(i);
							return;
						}
					}
				}
			}

			// Token: 0x060001CC RID: 460 RVA: 0x00006082 File Offset: 0x00004282
			public void RevertState()
			{
			}

			// Token: 0x060001CD RID: 461 RVA: 0x00006084 File Offset: 0x00004284
			public void PushState()
			{
			}

			// Token: 0x060001CE RID: 462 RVA: 0x00006086 File Offset: 0x00004286
			public void PopState()
			{
			}

			// Token: 0x060001CF RID: 463 RVA: 0x00006088 File Offset: 0x00004288
			public void Unacquire()
			{
				this.Unacquire(false);
			}

			// Token: 0x060001D0 RID: 464 RVA: 0x00006091 File Offset: 0x00004291
			public void Unacquire(bool inIgnoreParent)
			{
			}

			// Token: 0x060001D1 RID: 465 RVA: 0x00006093 File Offset: 0x00004293
			public void Reacquire()
			{
				this.Reacquire(false);
			}

			// Token: 0x060001D2 RID: 466 RVA: 0x0000609C File Offset: 0x0000429C
			public void Reacquire(bool inIgnoreParent)
			{
			}

			// Token: 0x060001D3 RID: 467 RVA: 0x0000609E File Offset: 0x0000429E
			public void SplitChildren()
			{
			}

			// Token: 0x04000078 RID: 120
			public List<RenderStateManager.Context.JournalEntry> mJournal = new List<RenderStateManager.Context.JournalEntry>();

			// Token: 0x04000079 RID: 121
			public List<RenderStateManager.Context> mChildContexts = new List<RenderStateManager.Context>();

			// Token: 0x0400007A RID: 122
			public List<uint> mFloorStack = new List<uint>();

			// Token: 0x0400007B RID: 123
			public uint mJournalFloor;

			// Token: 0x0400007C RID: 124
			public RenderStateManager.Context mParentContext;

			// Token: 0x02000026 RID: 38
			public class JournalEntry
			{
				// Token: 0x060001D4 RID: 468 RVA: 0x000060A0 File Offset: 0x000042A0
				public JournalEntry()
				{
					this.mState = null;
				}

				// Token: 0x060001D5 RID: 469 RVA: 0x000060C5 File Offset: 0x000042C5
				public JournalEntry(RenderStateManager.State inState, RenderStateManager.StateValue inOldValue, RenderStateManager.StateValue inNewValue)
				{
					this.mState = inState;
					this.mOldValue = inOldValue;
					this.mNewValue = inNewValue;
				}

				// Token: 0x0400007D RID: 125
				public RenderStateManager.State mState;

				// Token: 0x0400007E RID: 126
				public RenderStateManager.StateValue mOldValue = new RenderStateManager.StateValue();

				// Token: 0x0400007F RID: 127
				public RenderStateManager.StateValue mNewValue = new RenderStateManager.StateValue();
			}
		}
	}
}
