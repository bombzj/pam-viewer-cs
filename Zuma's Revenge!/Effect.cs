using System;
using SexyFramework.GraphicsLib;

namespace ZumasRevenge
{
	// Token: 0x02000054 RID: 84
	public abstract class Effect : IDisposable
	{
		// Token: 0x060006C9 RID: 1737 RVA: 0x0002E4AE File Offset: 0x0002C6AE
		protected virtual void Init()
		{
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0002E4B0 File Offset: 0x0002C6B0
		public Effect()
		{
			this.mUpdateCount = 0;
			this.mIs3D = GameApp.gApp.mGraphicsDriver.Is3D();
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0002E4D4 File Offset: 0x0002C6D4
		public virtual void Dispose()
		{
		}

		// Token: 0x060006CC RID: 1740
		public abstract void Update();

		// Token: 0x060006CD RID: 1741
		public abstract string GetName();

		// Token: 0x060006CE RID: 1742 RVA: 0x0002E4D6 File Offset: 0x0002C6D6
		public virtual void DrawUnderBalls(Graphics g)
		{
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0002E4D8 File Offset: 0x0002C6D8
		public virtual void DrawAboveBalls(Graphics g)
		{
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0002E4DA File Offset: 0x0002C6DA
		public virtual void DrawUnderBackground(Graphics g)
		{
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0002E4DC File Offset: 0x0002C6DC
		public virtual void LevelStarted(bool from_load)
		{
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0002E4DE File Offset: 0x0002C6DE
		public virtual void DrawFullScene(Graphics g)
		{
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0002E4E0 File Offset: 0x0002C6E0
		public virtual void DrawFullSceneNoFrog(Graphics g)
		{
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0002E4E2 File Offset: 0x0002C6E2
		public virtual void DrawPriority(Graphics g, int priority)
		{
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0002E4E4 File Offset: 0x0002C6E4
		public virtual bool DrawTunnel(Graphics g, Image img, int x, int y)
		{
			return true;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0002E4E8 File Offset: 0x0002C6E8
		public virtual void Reset(string level_id)
		{
			if (level_id.Length == 0)
			{
				return;
			}
			char c = level_id[0];
			if (c >= 'a' && c <= 'z')
			{
				c -= ' ';
				this.mLevelId = c + level_id.Substring(1);
			}
			else
			{
				this.mLevelId = level_id;
			}
			if (this.mResGroup.Length > 0 && GameApp.gApp.mResourceManager.IsGroupLoaded(this.mResGroup))
			{
				this.Init();
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0002E564 File Offset: 0x0002C764
		public virtual void LoadResources()
		{
			if (this.mResGroup.Length == 0 || GameApp.gApp.mResourceManager.IsGroupLoaded(this.mResGroup))
			{
				return;
			}
			GameApp.gApp.mResourceManager.LoadResources(this.mResGroup);
			this.Init();
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0002E5B2 File Offset: 0x0002C7B2
		public virtual void DeleteResources()
		{
			if (this.mResGroup.Length == 0 || !GameApp.gApp.mResourceManager.IsGroupLoaded(this.mResGroup))
			{
				return;
			}
			GameApp.gApp.mResourceManager.DeleteResources(this.mResGroup);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0002E5EE File Offset: 0x0002C7EE
		public virtual void BulletFired(Bullet b)
		{
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0002E5F0 File Offset: 0x0002C7F0
		public virtual bool DrawSkullPit(Graphics g, HoleMgr hole)
		{
			return false;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0002E5F3 File Offset: 0x0002C7F3
		public virtual void UserDied()
		{
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0002E5F5 File Offset: 0x0002C7F5
		public virtual void NukeParams()
		{
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0002E5F7 File Offset: 0x0002C7F7
		public virtual void SetParams(string key, string value)
		{
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0002E5F9 File Offset: 0x0002C7F9
		public virtual void BulletHit(Bullet b)
		{
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0002E5FB File Offset: 0x0002C7FB
		public virtual void CopyFrom(Effect e)
		{
			this.Reset(this.mLevelId);
			this.mUpdateCount = e.mUpdateCount;
			this.mIs3D = e.mIs3D;
			this.mResGroup = e.mResGroup;
			this.mLevelId = e.mLevelId;
		}

		// Token: 0x04000404 RID: 1028
		protected int mUpdateCount;

		// Token: 0x04000405 RID: 1029
		protected bool mIs3D;

		// Token: 0x04000406 RID: 1030
		protected string mResGroup;

		// Token: 0x04000407 RID: 1031
		protected string mLevelId;
	}
}
