using System;

namespace ZumasRevenge
{
	// Token: 0x02000052 RID: 82
	public interface NewUserDialogListener
	{
		// Token: 0x060006C5 RID: 1733
		void BlankNameEntered();

		// Token: 0x060006C6 RID: 1734
		void NameIsAllSpaces();

		// Token: 0x060006C7 RID: 1735
		void FinishedNewUser(bool canceled);
	}
}
