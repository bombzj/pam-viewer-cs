using System;

namespace SexyFramework.Resource
{
	// Token: 0x0200018E RID: 398
	public class SoundRes : BaseRes
	{
		// Token: 0x06000DD7 RID: 3543 RVA: 0x00045BA6 File Offset: 0x00043DA6
		public SoundRes()
		{
			this.mType = ResType.ResType_Sound;
			this.mSoundId = -1;
			this.mVolume = -1.0;
			this.mPanning = 0;
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00045BD4 File Offset: 0x00043DD4
		public override void DeleteResource()
		{
			if (this.mResourceRef != null && this.mResourceRef.HasResource())
			{
				this.mResourceRef.Release();
			}
			else if (this.mSoundId >= 0)
			{
				GlobalMembers.gSexyAppBase.mSoundManager.ReleaseSound(this.mSoundId);
			}
			this.mSoundId = -1;
			if (this.mGlobalPtr != null)
			{
				this.mGlobalPtr.mResObject = -1;
			}
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00045C44 File Offset: 0x00043E44
		public override void ApplyConfig()
		{
			if (this.mSoundId == -1)
			{
				return;
			}
			if (this.mResourceRef != null && this.mResourceRef.HasResource())
			{
				return;
			}
			if (this.mVolume >= 0.0)
			{
				GlobalMembers.gSexyAppBase.mSoundManager.SetBaseVolume((uint)this.mSoundId, this.mVolume);
			}
			if (this.mPanning != 0)
			{
				GlobalMembers.gSexyAppBase.mSoundManager.SetBasePan((uint)this.mSoundId, this.mPanning);
			}
		}

		// Token: 0x04000B60 RID: 2912
		public int mSoundId;

		// Token: 0x04000B61 RID: 2913
		public double mVolume;

		// Token: 0x04000B62 RID: 2914
		public int mPanning;
	}
}
