using System;
using SexyFramework.Drivers.Profile;
using SexyFramework.Misc;

namespace SexyFramework.Drivers
{
	// Token: 0x02000060 RID: 96
	public abstract class ISaveGameDriver
	{
		// Token: 0x060003D7 RID: 983 RVA: 0x0000C3B6 File Offset: 0x0000A5B6
		public virtual void Dispose()
		{
		}

		// Token: 0x060003D8 RID: 984
		public abstract bool Init();

		// Token: 0x060003D9 RID: 985
		public abstract void Update();

		// Token: 0x060003DA RID: 986
		public abstract ISaveGameContext CreateSaveGameContext(UserProfile player, string saveName, ulong requiredBytes);

		// Token: 0x060003DB RID: 987
		public abstract bool BeginLoad(ISaveGameContext context, string segment, bool checkOnly);

		// Token: 0x060003DC RID: 988
		public abstract bool BeginSave(ISaveGameContext context, string segment, SexyBuffer data);

		// Token: 0x060003DD RID: 989
		public abstract bool BeginDelete(ISaveGameContext context, string segment);

		// Token: 0x060003DE RID: 990
		public abstract bool BeginSaveGameDelete(ISaveGameContext context);
	}
}
