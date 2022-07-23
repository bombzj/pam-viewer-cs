using System;
using System.Collections.Generic;
using System.Linq;
using JeffLib;
using SexyFramework;
using SexyFramework.GraphicsLib;
using SexyFramework.Misc;

namespace ZumasRevenge
{
	// Token: 0x02000015 RID: 21
	public abstract class Boss : IDisposable
	{
		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x0000DB61 File Offset: 0x0000BD61
		// (set) Token: 0x06000431 RID: 1073 RVA: 0x0000DB6E File Offset: 0x0000BD6E
		public int mWallDownTime
		{
			get
			{
				return this.mDWallDownTime.value;
			}
			set
			{
				this.mDWallDownTime.value = value;
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x0000DB7C File Offset: 0x0000BD7C
		// (set) Token: 0x06000433 RID: 1075 RVA: 0x0000DB89 File Offset: 0x0000BD89
		public float mHPDecPerHit
		{
			get
			{
				return this.mDHPDecPerHit.value;
			}
			set
			{
				this.mDHPDecPerHit.value = value;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x0000DB97 File Offset: 0x0000BD97
		// (set) Token: 0x06000435 RID: 1077 RVA: 0x0000DBA4 File Offset: 0x0000BDA4
		public float mHPDecPerProxBomb
		{
			get
			{
				return this.mDHPDecPerProxBomb.value;
			}
			set
			{
				this.mDHPDecPerProxBomb.value = value;
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0000DBB2 File Offset: 0x0000BDB2
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x0000DBBF File Offset: 0x0000BDBF
		public int mTikiHealthRespawnAmt
		{
			get
			{
				return this.mDTikiHealthRespawnAmt.value;
			}
			set
			{
				this.mDTikiHealthRespawnAmt.value = value;
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000DBD0 File Offset: 0x0000BDD0
		private void InitParamPointers()
		{
			Dictionary<string, ParamData<float>>.Enumerator enumerator = this.mFParamPointerMap.GetEnumerator();
			while (enumerator.MoveNext())
			{
				DDS gDDS = GameApp.gDDS;
				KeyValuePair<string, ParamData<float>> keyValuePair = enumerator.Current;
				if (gDDS.HasBossParam(keyValuePair.Key))
				{
					ParamData<float> paramData = new ParamData<float>();
					ParamData<float> paramData2 = paramData;
					DDS gDDS2 = GameApp.gDDS;
					KeyValuePair<string, ParamData<float>> keyValuePair2 = enumerator.Current;
					paramData2.value = gDDS2.GetBossParam(keyValuePair2.Key);
					Dictionary<string, ParamData<float>> dictionary = this.mFParamPointerMap;
					KeyValuePair<string, ParamData<float>> keyValuePair3 = enumerator.Current;
					dictionary[keyValuePair3.Key] = paramData;
				}
			}
			Dictionary<string, ParamData<int>>.Enumerator enumerator2 = this.mIParamPointerMap.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				DDS gDDS3 = GameApp.gDDS;
				KeyValuePair<string, ParamData<int>> keyValuePair4 = enumerator2.Current;
				if (gDDS3.HasBossParam(keyValuePair4.Key))
				{
					ParamData<int> paramData3 = new ParamData<int>();
					ParamData<int> paramData4 = paramData3;
					DDS gDDS4 = GameApp.gDDS;
					KeyValuePair<string, ParamData<int>> keyValuePair5 = enumerator2.Current;
					paramData4.value = (int)gDDS4.GetBossParam(keyValuePair5.Key);
					Dictionary<string, ParamData<int>> dictionary2 = this.mIParamPointerMap;
					KeyValuePair<string, ParamData<int>> keyValuePair6 = enumerator2.Current;
					dictionary2[keyValuePair6.Key] = paramData3;
				}
			}
			for (int i = 0; i < this.mBerserkTiers.size<BerserkTier>(); i++)
			{
				BerserkTier berserkTier = this.mBerserkTiers[i];
				for (int j = 0; j < berserkTier.mParams.size<BerserkModifier>(); j++)
				{
					BerserkModifier berserkModifier = berserkTier.mParams[j];
					string text = berserkModifier.mParamName.ToLower();
					if (this.mFParamPointerMap.ContainsKey(text))
					{
						berserkModifier.AddPointerFloat(this.mFParamPointerMap[text]);
					}
					if (this.mIParamPointerMap.ContainsKey(text))
					{
						berserkModifier.AddPointerInt(this.mIParamPointerMap[text]);
					}
					if (this.mBParamPointerMap.ContainsKey(text))
					{
						berserkModifier.AddPointerBool(this.mBParamPointerMap[text]);
					}
				}
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000DDA0 File Offset: 0x0000BFA0
		protected virtual void DecHearts(int amount)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_HEARTS);
			for (int i = 0; i < Boss.NUM_HEARTS; i++)
			{
				if (this.mHeartCels[i] < imageByID.mNumCols - 1)
				{
					int num = this.mHeartCels[i];
					this.mHeartCels[i] += amount;
					if (this.mHeartCels[i] <= imageByID.mNumCols - 1)
					{
						break;
					}
					this.mHeartCels[i] = imageByID.mNumCols - 1;
					amount -= this.mHeartCels[i] - num;
				}
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000DE2C File Offset: 0x0000C02C
		protected virtual void ResetWallAndTikis(int wall_index)
		{
			if (this.mHP <= 0f)
			{
				return;
			}
			if (Enumerable.Count<BossWall>(this.mWalls) == Enumerable.Count<Tiki>(this.mTikis))
			{
				this.mTikis[wall_index].mWasHit = false;
				this.mTikis[wall_index].mAlphaFadeDir = 1;
				this.mWalls[wall_index].mAlphaFadeDir = 1;
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000DE98 File Offset: 0x0000C098
		protected virtual bool DoHit(Bullet b, bool from_prox_bomb)
		{
			float mPrevHP = this.mHP;
			float num = (from_prox_bomb ? this.mHPDecPerProxBomb : this.mHPDecPerHit);
			int amount = (from_prox_bomb ? this.mHeartPieceDecAmtProxBomb : this.mHeartPieceDecAmt);
			if (num <= 0f)
			{
				return false;
			}
			this.mHP -= num;
			if (this.mTikiHealthRespawnAmt > 0 && this.CanDecTikiHealthSpawnAmt())
			{
				this.mCurrTikiBossHealthRemoved += (int)num;
				if (this.mCurrTikiBossHealthRemoved >= this.mTikiHealthRespawnAmt)
				{
					this.mCurrTikiBossHealthRemoved = 0;
					for (int i = 0; i < Enumerable.Count<BossWall>(this.mWalls); i++)
					{
						this.ResetWallAndTikis(i);
					}
				}
			}
			if (this.mHP <= 0f)
			{
				this.mHP = 0f;
				this.mDeathTimer = 0;
				this.PlaySound(0);
				this.mApp.GetBoard().BossDied();
			}
			else
			{
				this.PlaySound(3);
			}
			this.mDoExplosion = true;
			if (this.mAllowCompacting)
			{
				this.mNeedsCompacting = true;
			}
			this.DecHearts(amount);
			if (this.mHP > 0f)
			{
				this.CheckIfShouldGoBerserk(mPrevHP);
			}
			else
			{
				this.mTauntQueue.Clear();
			}
			return true;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000DFB8 File Offset: 0x0000C1B8
		protected virtual bool CompactCurves()
		{
			for (int i = 0; i < this.mLevel.mNumCurves; i++)
			{
				if (!this.mLevel.mCurveMgr[i].CanCompact())
				{
					return false;
				}
			}
			for (int j = 0; j < this.mLevel.mNumCurves; j++)
			{
				this.mLevel.mCurveMgr[j].CompactCurve(false);
			}
			return true;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000E01C File Offset: 0x0000C21C
		protected virtual void DrawHearts(Graphics g)
		{
			if (this.mHP <= 0f || this.mDoDeathExplosions || this.mLevel.mBoard.DoingBossIntro())
			{
				return;
			}
			g.PushState();
			if (this.mAlphaOverride <= 254f)
			{
				g.SetColor(255, 255, 255, (int)this.mAlphaOverride);
				g.SetColorizeImages(true);
			}
			Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_HEARTS);
			for (int i = 0; i < Boss.NUM_HEARTS; i++)
			{
				g.DrawImageCel(imageByID, (int)(ZumasRevenge.Common._S(this.mX + (float)this.mHeartXOff) + (float)(i * imageByID.GetCelWidth())), (int)ZumasRevenge.Common._S(this.mY + (float)this.mHeartYOff), this.mHeartCels[i]);
			}
			g.PopState();
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000E0EC File Offset: 0x0000C2EC
		protected virtual void DrawMisc(Graphics g)
		{
			if (this.mHP > 0f && !this.mDoDeathExplosions && !this.mLevel.mBoard.DoingBossIntro())
			{
				for (int i = 0; i < this.mTikis.size<Tiki>(); i++)
				{
					this.mTikis[i].Draw(g);
				}
				this.DrawWalls(g);
			}
			Font fontByID = Res.GetFontByID(ResID.FONT_SHAGLOUNGE28_STROKE);
			if (Boss.gBerserkTextAlpha > 0f)
			{
				g.SetFont(fontByID);
				g.SetColor(ZumasRevenge.Common._M(255), ZumasRevenge.Common._M1(0), ZumasRevenge.Common._M2(0), (int)Boss.gBerserkTextAlpha);
				string @string = TextManager.getInstance().getString(150);
				int num = g.GetFont().StringWidth(@string);
				g.DrawString(@string, (this.mApp.mWidth - num) / 2, (int)Boss.gBerserkTextY);
			}
			if (Boss.gImpatientTextAlpha > 0f)
			{
				g.SetFont(fontByID);
				g.SetColor(ZumasRevenge.Common._M(0), ZumasRevenge.Common._M1(0), ZumasRevenge.Common._M2(0), (int)Boss.gImpatientTextAlpha);
				string string2 = TextManager.getInstance().getString(151);
				int num2 = g.GetFont().StringWidth(string2);
				g.DrawString(string2, (this.mApp.mWidth - num2) / 2, (int)Boss.gImpatientTextY);
			}
			if (this.mHP <= 0f && this.mBandagedImg != null)
			{
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, 255 - (int)this.mAlphaOverride);
				g.DrawImage(this.mBandagedImg, (int)(ZumasRevenge.Common._S(this.mX) - (float)(this.mBandagedImg.mWidth / 2) + (float)this.mShakeXOff + (float)ZumasRevenge.Common._S(this.mBandagedXOff)), (int)(ZumasRevenge.Common._S(this.mY) - (float)(this.mBandagedImg.mHeight / 2) + (float)this.mShakeYOff + (float)ZumasRevenge.Common._S(this.mBandagedYOff)));
				g.SetColorizeImages(false);
			}
			if (this.mShouldDoDeathExplosions)
			{
				for (int j = 0; j < this.mDeathExplosions.size<PIEffect>(); j++)
				{
					PIEffect pieffect = this.mDeathExplosions[j];
					pieffect.mDrawTransform.LoadIdentity();
					float num3 = GameApp.DownScaleNum(1f);
					pieffect.mDrawTransform.Scale(num3, num3);
					pieffect.mDrawTransform.Translate(ZumasRevenge.Common._S(this.mX), ZumasRevenge.Common._S(this.mY));
					pieffect.Draw(g);
				}
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000E36D File Offset: 0x0000C56D
		protected virtual bool BulletIntersectsBoss(Bullet b)
		{
			return MathUtils.CirclesIntersect(b.GetX(), b.GetY(), this.mX, this.mY + (float)this.mBossRadiusYOff, (float)(this.mBossRadius + b.GetRadius()));
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000E3A2 File Offset: 0x0000C5A2
		protected void AddParamPointer(string p, ParamData<float> v)
		{
			this.mFParamPointerMap[p.ToLower()] = v;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000E3B6 File Offset: 0x0000C5B6
		protected void AddParamPointer(string p, ParamData<int> v)
		{
			this.mIParamPointerMap[p.ToLower()] = v;
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000E3CA File Offset: 0x0000C5CA
		protected void AddParamPointer(string p, ParamData<bool> v)
		{
			this.mBParamPointerMap[p.ToLower()] = v;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000E3E0 File Offset: 0x0000C5E0
		protected void CheckIfShouldGoBerserk(float mPrevHP)
		{
			foreach (BerserkTier berserkTier in this.mBerserkTiers)
			{
				if (mPrevHP >= (float)berserkTier.mHealthLimit && this.mHP < (float)berserkTier.mHealthLimit)
				{
					for (int i = 0; i < Enumerable.Count<BerserkModifier>(berserkTier.mParams); i++)
					{
						berserkTier.mParams[i].ModifyVariable();
					}
					this.BerserkActivated(berserkTier.mHealthLimit);
					this.ReInit();
					break;
				}
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000E480 File Offset: 0x0000C680
		protected virtual void ReInit()
		{
			this.mHeartPieceDecAmt = (int)((float)(Boss.NUM_HEARTS * 4) / (this.mMaxHP / this.mHPDecPerHit));
			this.mHeartPieceDecAmtProxBomb = (int)((float)(Boss.NUM_HEARTS * 4) / (this.mMaxHP / this.mHPDecPerProxBomb));
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000E4BC File Offset: 0x0000C6BC
		protected virtual void BerserkActivated(int health_limit)
		{
			Boss.gBerserkTextAlpha = 255f;
			Boss.gBerserkTextY = (float)(this.mApp.mHeight / 2);
			this.mIsBerserk = true;
			this.PlaySound(1);
			foreach (HulaEntry hulaEntry in this.mHulaEntryVec)
			{
				if (hulaEntry.mBerserkAmt == health_limit)
				{
					this.mCurrentHulaEntry = hulaEntry;
					break;
				}
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000E548 File Offset: 0x0000C748
		protected virtual void BallEaten(Bullet b)
		{
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000E54A File Offset: 0x0000C74A
		protected virtual bool CanSpawnHulaDancers()
		{
			return true;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000E54D File Offset: 0x0000C74D
		protected virtual void DrawWalls(Graphics g)
		{
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0000E54F File Offset: 0x0000C74F
		protected virtual Rect GetWallRect(BossWall w)
		{
			return new Rect(w.mX, w.mY, w.mWidth, w.mHeight);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000E570 File Offset: 0x0000C770
		protected virtual bool CollidesWithWall(Bullet b)
		{
			float num = (float)b.GetRadius() * 0.75f;
			Rect theTRect = new Rect((int)(b.GetX() - num), (int)(b.GetY() - num), (int)(num * 2f), (int)(num * 2f));
			foreach (BossWall bossWall in this.mWalls)
			{
				if (bossWall.mAlphaFadeDir >= 0 && this.GetWallRect(bossWall).Intersects(theTRect))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0000E618 File Offset: 0x0000C818
		protected virtual bool CanDecTikiHealthSpawnAmt()
		{
			return true;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000E61B File Offset: 0x0000C81B
		protected virtual bool CanTaunt()
		{
			return true;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000E61E File Offset: 0x0000C81E
		protected virtual void TikiHit(int idx)
		{
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000E620 File Offset: 0x0000C820
		public Boss()
			: this(null)
		{
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000E62C File Offset: 0x0000C82C
		public Boss(Level l)
		{
			this.mX = 0f;
			this.mY = 0f;
			this.mMaxHP = 0f;
			this.mHP = 0f;
			this.mWidth = 101;
			this.mHeight = 78;
			this.mUpdateCount = 0;
			this.mHPDecPerHit = 0f;
			this.mHPDecPerProxBomb = 0f;
			this.mLevel = l;
			this.mShakeXAmt = 0;
			this.mShakeYAmt = 0;
			this.mShouldDoDeathExplosions = true;
			this.mShakeXOff = 0;
			this.mShakeYOff = 0;
			this.mAllowLevelDDS = false;
			this.mDoExplosion = false;
			this.mNeedsCompacting = false;
			this.mAllowCompacting = false;
			this.mHeartXOff = 0;
			this.mHeartYOff = 150;
			this.mResetWallTimerOnTikiHit = false;
			this.mResetWallsOnBossHit = false;
			this.mWallDownTime = 0;
			this.mCurWallDownTime = 0;
			this.mStunTime = 0;
			this.mCurrTikiBossHealthRemoved = 0;
			this.mTikiHealthRespawnAmt = 0;
			this.mNum = 0;
			this.mIsBerserk = false;
			this.mApp = GameApp.gApp;
			this.mEatsBalls = false;
			this.mImpatientTimer = -1;
			this.mBombFreqMax = 0;
			this.mBombFreqMin = 0;
			this.mBombDuration = 0;
			this.mProxBombRadius = 80;
			this.mDrawRadius = false;
			this.mBossRadius = 70;
			this.mNeedsIntroSound = false;
			this.mBombInRange = false;
			this.mRadiusColorChangeMode = 1;
			this.mDoDeathExplosions = false;
			this.mDeathTimer = 0;
			this.mWordBubbleTimer = 300;
			this.mSepiaImage = null;
			this.mDeathTX = 0f;
			this.mDeathTY = 0f;
			this.mDeathVX = 0f;
			this.mDeathVY = 0f;
			this.mExplosionRate = 4;
			this.mBossRadiusYOff = 0;
			this.mHulaAmnesty = 0;
			this.mBandagedImg = null;
			this.mAlphaOverride = 255f;
			this.mBandagedXOff = 0;
			this.mBandagedYOff = 0;
			this.mDrawDeathBGTikis = true;
			this.mTauntTextYOff = 0;
			Boss.gBerserkTextAlpha = 0f;
			Boss.gBerserkTextY = 0f;
			Boss.gImpatientTextAlpha = 0f;
			Boss.gImpatientTextY = 0f;
			this.mResPrefix = "IMAGE_";
			this.mHitEffect = null;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000E968 File Offset: 0x0000CB68
		public virtual void Dispose()
		{
			this.mSepiaImage = null;
			for (int i = 0; i < this.mHulaDancers.size<HulaDancer>(); i++)
			{
				this.mHulaDancers[i] = null;
			}
			for (int j = 0; j < this.mDeathExplosions.Count; j++)
			{
				if (this.mDeathExplosions[j] != null)
				{
					this.mDeathExplosions[j].Dispose();
				}
			}
			this.mDeathExplosions.Clear();
			if (this.mHitEffect != null)
			{
				this.mHitEffect.Dispose();
				this.mHitEffect = null;
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000E9FC File Offset: 0x0000CBFC
		public void AddTiki(int x, int y, int id, int rail_w, int rail_h, int travel_time)
		{
			Tiki tiki = new Tiki();
			this.mTikis.Add(tiki);
			tiki.mId = id;
			tiki.mX = (float)x;
			tiki.mY = (float)y;
			tiki.mRailStartX = x;
			tiki.mRailStartY = y;
			tiki.mRailEndX = x + rail_w;
			tiki.mRailEndY = y + rail_h;
			tiki.mTravelTime = travel_time;
			if (travel_time != 0)
			{
				tiki.mVX = (float)(tiki.mRailEndX - tiki.mRailStartX) / (float)travel_time;
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000EA78 File Offset: 0x0000CC78
		public void AddTiki(int x, int y, int id)
		{
			this.AddTiki(x, y, id, 0, 0, 0);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000EA88 File Offset: 0x0000CC88
		public void AddWall(int x, int y, int w, int h, int id)
		{
			BossWall bossWall = new BossWall();
			bossWall.mX = x;
			bossWall.mY = y;
			bossWall.mWidth = w;
			bossWall.mHeight = h;
			bossWall.mId = id;
			bossWall.mAlphaFadeDir = 1;
			bossWall.mAlpha = 0;
			this.mWalls.Add(bossWall);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000EADA File Offset: 0x0000CCDA
		public List<BossWall> getWalls()
		{
			return this.mWalls;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000EAE4 File Offset: 0x0000CCE4
		public void ForceNextTauntText()
		{
			this.mTauntQueue.Clear();
			Boss.FNTT_last_idx = (Boss.FNTT_last_idx + 1) % Enumerable.Count<TauntText>(this.mTauntText);
			if (Boss.FNTT_last_idx > Enumerable.Count<TauntText>(this.mTauntText))
			{
				Boss.FNTT_last_idx = 0;
			}
			this.mTauntQueue.Add(this.mTauntText[Boss.FNTT_last_idx]);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000EB48 File Offset: 0x0000CD48
		public virtual void Init(Level l)
		{
			this.mMaxHP = (this.mHP = 100f);
			if (l != null)
			{
				this.mLevel = l;
				for (int i = 0; i < this.mTikis.size<Tiki>(); i++)
				{
					this.mTikis[i].Init(this);
				}
			}
			if (this.mResGroup.Length > 0 && !this.mApp.mResourceManager.IsGroupLoaded(this.mResGroup) && !this.mApp.mResourceManager.LoadResources(this.mResGroup))
			{
				this.mApp.ShowResourceError(true);
				this.mApp.Shutdown();
				return;
			}
			if (!this.mApp.mResourceManager.IsGroupLoaded("Bosses") && !this.mApp.mResourceManager.LoadResources("Bosses"))
			{
				this.mApp.ShowResourceError(true);
				this.mApp.Shutdown();
				return;
			}
			if (this.mNum == 6 && !this.mApp.mResourceManager.IsGroupLoaded("Boss6Common") && !this.mApp.mResourceManager.LoadResources("Boss6Common"))
			{
				this.mApp.ShowResourceError(true);
				this.mApp.Shutdown();
				return;
			}
			this.mHitEffect = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_DEATH_EXPLOSION).Duplicate();
			ZumasRevenge.Common.SetFXNumScale(this.mHitEffect, this.mApp.Is3DAccelerated() ? 1f : ZumasRevenge.Common._M(0.25f));
			Image imageByID = Res.GetImageByID(ResID.IMAGE_BOSS_HEARTS);
			this.ReInit();
			for (int j = 0; j < Boss.NUM_HEARTS; j++)
			{
				this.mHeartCels[j] = imageByID.mNumCols - 1;
			}
			this.InitParamPointers();
			if (this.mWalls.size<BossWall>() == this.mTikis.size<Tiki>())
			{
				for (int k = 0; k < this.mWalls.size<BossWall>(); k++)
				{
					BossWall bossWall = this.mWalls[k];
					bossWall.mAlphaFadeDir = 1;
					bossWall.mAlpha = 0;
				}
			}
			if (this.mTikis.size<Tiki>() == 2)
			{
				this.mTikis[0].SetIsLeft(this.mTikis[0].mX < this.mTikis[1].mX);
				this.mTikis[1].SetIsLeft(this.mTikis[1].mX < this.mTikis[0].mX);
			}
			this.mSounds[6] = -1;
			this.mSounds[7] = -1;
			this.mSounds[8] = -1;
			this.mSounds[9] = -1;
			if (this.mNum < 6)
			{
				this.mSounds[0] = this.mApp.mResourceManager.LoadSound("SOUND_BOSS" + this.mNum + "_DIE");
				this.mSounds[1] = this.mApp.mResourceManager.LoadSound("SOUND_BOSS" + this.mNum + "_ENRAGE");
				this.mSounds[2] = this.mApp.mResourceManager.LoadSound("SOUND_BOSS" + this.mNum + "_FIRE");
				this.mSounds[3] = this.mApp.mResourceManager.LoadSound("SOUND_BOSS" + this.mNum + "_HIT");
				this.mSounds[4] = this.mApp.mResourceManager.LoadSound("SOUND_BOSS" + this.mNum + "_PLAYER_HIT");
				this.mSounds[5] = this.mApp.mResourceManager.LoadSound("SOUND_BOSS" + this.mNum + "_INTRO");
				if (this.mNum == 4)
				{
					this.mSounds[6] = this.mApp.mResourceManager.LoadSound("SOUND_BOSS4_EAT_BALL");
					this.mSounds[8] = this.mApp.mResourceManager.LoadSound("SOUND_BOSS4_TELEPORT");
				}
				else if (this.mNum == 1)
				{
					this.mSounds[7] = this.mApp.mResourceManager.LoadSound("SOUND_BOSS1_ROAR");
				}
				else if (this.mNum == 5)
				{
					this.mSounds[9] = this.mApp.mResourceManager.LoadSound("SOUND_BOSS5_SHIELD_HIT");
				}
			}
			else
			{
				this.mSounds[0] = this.mApp.mResourceManager.LoadSound("SOUND_BOSS_DIE" + (1 + SexyFramework.Common.Rand() % 3));
				this.mSounds[1] = -1;
				this.mSounds[2] = this.mApp.mResourceManager.LoadSound("SOUND_BOSS_FIRE");
				this.mSounds[3] = this.mApp.mResourceManager.LoadSound("SOUND_BOSS_HIT" + (1 + SexyFramework.Common.Rand() % 4));
				this.mSounds[4] = this.mApp.mResourceManager.LoadSound("SOUND_BULLET_HIT");
				this.mSounds[5] = this.mApp.mResourceManager.LoadSound("SOUND_BOSS_INTRO" + SexyFramework.Common.Rand() % 3);
			}
			for (int m = 0; m < this.mHulaEntryVec.size<HulaEntry>(); m++)
			{
				if (this.mHulaEntryVec[m].mBerserkAmt >= 100)
				{
					this.mCurrentHulaEntry = this.mHulaEntryVec[m];
					break;
				}
			}
			int num = -1;
			for (int n = 0; n < this.mTauntText.size<TauntText>(); n++)
			{
				if (this.mTauntText[n].mMinDeaths <= this.mApp.mUserProfile.GetAdvModeVars().mNumDeathsCurLevel && this.mTauntText[n].mMinDeaths > num)
				{
					num = this.mTauntText[n].mMinDeaths;
				}
			}
			for (int num2 = 0; num2 < this.mTauntText.size<TauntText>(); num2++)
			{
				TauntText tauntText = this.mTauntText[num2];
				if (tauntText.mCondition == 0 && tauntText.mMinDeaths == num)
				{
					this.mTauntQueue.Add(tauntText);
				}
			}
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0000F190 File Offset: 0x0000D390
		public virtual void Update(float f)
		{
			if (this.mHP <= 0f || this.mLevel.mBoard.GetGameState() == GameState.GameState_Losing)
			{
				float num = ((this.mHP <= 0f) ? ZumasRevenge.Common._M(1f) : ZumasRevenge.Common._M1(3f));
				this.mAlphaOverride -= num;
				if (this.mAlphaOverride < 0f)
				{
					this.mAlphaOverride = 0f;
				}
			}
			else if (this.mLevel.DoingInitialPathHilite() && this.mLevel.mBoard.GetGameState() != GameState.GameState_BossIntro && this.mUpdateCount % ZumasRevenge.Common._M(8) == 0 && this.mCleanHeart)
			{
				for (int i = 0; i < Boss.NUM_HEARTS; i++)
				{
					if (this.mHeartCels[i] != 0)
					{
						this.mHeartCels[i]--;
						break;
					}
				}
			}
			if (this.mAlphaOverride < 255f && this.mLevel.mBoard.GetGameState() != GameState.GameState_Losing && this.mLevel.mBoard.GetGameState() != GameState.GameState_BossDead)
			{
				this.mAlphaOverride += ZumasRevenge.Common._M(3f);
				if (this.mAlphaOverride > 255f)
				{
					this.mAlphaOverride = 255f;
				}
			}
			this.mUpdateCount++;
			if (this.mDoExplosion)
			{
				this.mHitEffect.Update();
				if (!this.mHitEffect.IsActive())
				{
					this.mHitEffect.ResetAnim();
					this.mDoExplosion = false;
				}
			}
			if (this.mCurWallDownTime > 0 && --this.mCurWallDownTime == 0)
			{
				for (int j = 0; j < this.mWalls.size<BossWall>(); j++)
				{
					this.ResetWallAndTikis(j);
				}
			}
			if (this.mWordBubbleTimer > 0 && !this.mLevel.mBoard.DoingBossIntro())
			{
				this.mWordBubbleTimer--;
			}
			if (this.mDoDeathExplosions && this.mShouldDoDeathExplosions && this.mHP <= 0f && this.mUpdateCount % ZumasRevenge.Common._M(25) == 0)
			{
				PIEffect pieffect = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_DEATH_EXPLOSION).Duplicate();
				this.mDeathExplosions.Add(pieffect);
				ZumasRevenge.Common.SetFXNumScale(pieffect, this.mApp.Is3DAccelerated() ? 1f : ZumasRevenge.Common._M(0.25f));
				SexyTransform2D sexyTransform2D = new SexyTransform2D(false);
				sexyTransform2D.Translate((float)ZumasRevenge.Common._S(-this.mWidth / 3 + SexyFramework.Common.Rand() % (int)((double)this.mWidth / 1.5)), (float)ZumasRevenge.Common._S(-this.mHeight / 3 + SexyFramework.Common.Rand() % (int)((double)this.mHeight / 1.5)));
				pieffect.mEmitterTransform.CopyFrom(sexyTransform2D);
			}
			for (int k = 0; k < this.mDeathExplosions.size<PIEffect>(); k++)
			{
				PIEffect pieffect2 = this.mDeathExplosions[k];
				pieffect2.Update();
				if (!pieffect2.IsActive())
				{
					pieffect2.Dispose();
					this.mDeathExplosions.RemoveAt(k);
					k--;
				}
			}
			for (int l = 0; l < this.mTauntQueue.size<TauntText>(); l++)
			{
				TauntText tauntText = this.mTauntQueue[l];
				tauntText.mUpdateCount++;
				if (tauntText.mUpdateCount < tauntText.mDelay)
				{
					break;
				}
				this.mTauntQueue.RemoveAt(l);
				l--;
			}
			if (this.mTauntQueue.size<TauntText>() == 0 && this.mApp.GetLevelMgr().mBossTauntChance > 0 && this.CanTaunt() && SexyFramework.Common._geq(this.mAlphaOverride, 255f))
			{
				List<int> list = new List<int>();
				for (int m = 0; m < this.mTauntText.size<TauntText>(); m++)
				{
					TauntText tauntText2 = this.mTauntText[m];
					if (this.mUpdateCount > tauntText2.mMinTime && SexyFramework.Common.Rand() % this.mApp.GetLevelMgr().mBossTauntChance == 0 && (tauntText2.mCondition != 1 || (SexyFramework.Common._eq(this.mHP, this.mMaxHP) && tauntText2.mCondition != 0)) && (tauntText2.mMinDeaths < 0 || tauntText2.mMinDeaths == this.mApp.mUserProfile.GetAdvModeVars().mNumDeathsCurLevel))
					{
						list.Add(m);
					}
				}
				if (list.size<int>() > 0)
				{
					this.mTauntQueue.Add(this.mTauntText[list[SexyFramework.Common.Rand() % list.size<int>()]]);
				}
			}
			if (this.mDoExplosion || this.mDoDeathExplosions)
			{
				this.mShakeXOff = SexyFramework.Common.IntRange(0, this.mShakeXAmt);
				this.mShakeYOff = SexyFramework.Common.IntRange(0, this.mShakeYAmt);
			}
			if (Boss.gBerserkTextAlpha > 0f)
			{
				Boss.gBerserkTextAlpha -= ZumasRevenge.Common._M(1f);
				Boss.gBerserkTextY -= ZumasRevenge.Common._M(1f);
			}
			if (Boss.gImpatientTextAlpha > 0f)
			{
				Boss.gImpatientTextAlpha -= ZumasRevenge.Common._M(1f);
				Boss.gImpatientTextY -= ZumasRevenge.Common._M(1f);
			}
			if (this.mLevel.mBoard.DoingBossIntro())
			{
				return;
			}
			if (this.mHP <= 0f)
			{
				if ((!this.mLevel.mFinalLevel || !this.mLevel.mBoard.mAdventureWinScreen) && Boss.last_idx >= 4)
				{
					Boss.last_idx = 0;
				}
				if (!this.mDoDeathExplosions)
				{
					for (int n = 0; n < this.mDeathText.size<BossText>(); n++)
					{
						BossText bossText = this.mDeathText[n];
						if (bossText.mAlpha < 255f)
						{
							bool flag = n == this.mDeathText.size<BossText>() - 1 && bossText.mAlpha < 255f;
							bossText.mAlpha = Math.Min(255f, bossText.mAlpha + 3f);
							if (flag && bossText.mAlpha >= 255f)
							{
								this.mApp.SetCursor(ECURSOR.CURSOR_HAND);
							}
						}
						if (bossText.mAlpha < (float)ZumasRevenge.Common._M(200))
						{
							break;
						}
					}
				}
				this.mX += this.mDeathVX;
				this.mY += this.mDeathVY;
				if ((this.mDeathVX > 0f && this.mX >= this.mDeathTX) || (this.mDeathVX < 0f && this.mX <= this.mDeathTX))
				{
					this.mX = this.mDeathTX;
					this.mDeathVX = 0f;
				}
				if ((this.mDeathVY > 0f && this.mY >= this.mDeathTY) || (this.mDeathVY < 0f && this.mY <= this.mDeathTY))
				{
					this.mY = this.mDeathTY;
					this.mDeathVY = 0f;
				}
				return;
			}
			bool flag2 = this.mLevel.AllCurvesAtRolloutPoint();
			if (this.mNeedsIntroSound && flag2 && !this.mApp.GetBoard().DoingIntros())
			{
				this.mNeedsIntroSound = false;
				this.PlaySound(5);
			}
			if (this.IsStunned())
			{
				this.mStunTime--;
			}
			if (this.mNeedsCompacting && !this.IsStunned() && this.CompactCurves())
			{
				this.mNeedsCompacting = false;
			}
			if (this.mHulaAmnesty > 0)
			{
				this.mHulaAmnesty--;
			}
			else if (this.mCurrentHulaEntry.mSpawnRate > 0 && this.mUpdateCount % this.mCurrentHulaEntry.mSpawnRate == 0 && SexyFramework.Common._geq(this.mAlphaOverride, 255f) && this.CanSpawnHulaDancers())
			{
				HulaDancer hulaDancer = new HulaDancer();
				this.mHulaDancers.Add(hulaDancer);
				bool has_proj = SexyFramework.Common.Rand() % 100 < this.mCurrentHulaEntry.mProjChance;
				hulaDancer.Setup(has_proj, (float)this.mCurrentHulaEntry.mSpawnY, this.mCurrentHulaEntry.mProjVY);
			}
			for (int num2 = 0; num2 < this.mHulaDancers.size<HulaDancer>(); num2++)
			{
				HulaDancer hulaDancer2 = this.mHulaDancers[num2];
				if (!SexyFramework.Common._eq(this.mAlphaOverride, 255f))
				{
					hulaDancer2.mFadeOut = true;
				}
				hulaDancer2.Update(this.mCurrentHulaEntry.mVX);
				if (hulaDancer2.CanRemove())
				{
					this.mHulaDancers[num2].Dispose();
					this.mHulaDancers.RemoveAt(num2);
					num2--;
				}
				else if (hulaDancer2.ProjectileCollided(this.mLevel.mFrog.GetRect()))
				{
					if (!this.mLevel.mFrog.IsFuckedUp())
					{
						this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_SLOW));
						switch (this.mCurrentHulaEntry.mAttackType)
						{
						case 1:
							this.mLevel.mFrog.Stun(this.mCurrentHulaEntry.mAttackTime);
							break;
						case 2:
							this.mLevel.mFrog.Poison(this.mCurrentHulaEntry.mAttackTime);
							break;
						case 3:
							this.mLevel.mBoard.SetHallucinateTimer(this.mCurrentHulaEntry.mAttackTime);
							break;
						case 4:
							this.mLevel.mFrog.SetSlowTimer(this.mCurrentHulaEntry.mAttackTime);
							break;
						}
					}
					hulaDancer2.DestroyBullet();
				}
				else if (!hulaDancer2.HasFired() && this.CanSpawnHulaDancers() && hulaDancer2.GetX() > (float)(this.mLevel.mFrog.GetCenterX() + this.mCurrentHulaEntry.mProjRange))
				{
					hulaDancer2.Fire();
				}
			}
			if (this.mImpatientTimer > 0 && flag2 && --this.mImpatientTimer == 0)
			{
				Boss.gImpatientTextAlpha = 255f;
				Boss.gImpatientTextY = (float)(this.mApp.mHeight / 2);
			}
			if (this.mDrawRadius && flag2)
			{
				this.mBombInRange = false;
				int num3 = this.mProxBombRadius + 56 + ZumasRevenge.Common.GetDefaultBallRadius();
				int num4 = num3 * num3;
				int num5 = 0;
				while (num5 < this.mLevel.mNumCurves && !this.mBombInRange)
				{
					for (int num6 = 0; num6 < this.mLevel.mCurveMgr[num5].mBallList.Count; num6++)
					{
						Ball ball = this.mLevel.mCurveMgr[num5].mBallList[num6];
						if (ball.GetPowerOrDestType(false) == PowerType.PowerType_ProximityBomb)
						{
							if ((this.mRadiusColorChangeMode != 2 || ball.GetY() > this.mY - (float)(this.mHeight / 2) + (float)ZumasRevenge.Common._M(0)) && SexyFramework.Common.Distance(ball.GetX(), ball.GetY(), this.mX, this.mY, false) <= (float)num4)
							{
								ball.mDoBossPulse = true;
								this.mBombInRange = true;
							}
							else
							{
								ball.mDoBossPulse = false;
							}
						}
					}
					num5++;
				}
			}
			if (this.IsImpatient())
			{
				for (int num7 = 0; num7 < this.mLevel.mNumCurves; num7++)
				{
					this.mLevel.mCurveMgr[num7].mSpeedScale += 0.000100000005f;
				}
			}
			for (int num8 = 0; num8 < this.mTikis.size<Tiki>(); num8++)
			{
				this.mTikis[num8].Update();
			}
			for (int num9 = 0; num9 < this.mWalls.size<BossWall>(); num9++)
			{
				BossWall bossWall = this.mWalls[num9];
				int mAlpha = bossWall.mAlpha;
				bossWall.mAlpha += bossWall.mAlphaFadeDir * ZumasRevenge.Common._M(8);
				if (bossWall.mAlpha < 0)
				{
					bossWall.mAlpha = 0;
				}
				else if (bossWall.mAlpha > 255)
				{
					bossWall.mAlpha = 255;
				}
				if (bossWall.mAlphaFadeDir == 1 && bossWall.mAlpha >= 255 && mAlpha < bossWall.mAlpha)
				{
					bossWall.mAlphaFadeDir = 0;
					this.ResetWallAndTikis(num9);
				}
			}
			if (this.mBombInRange)
			{
				Boss.gWackColorFade += Boss.gWackColorFadeDir;
				if (Boss.gWackColorFade >= 255 && Boss.gWackColorFadeDir > 0)
				{
					Boss.gWackColorFade = 255;
					Boss.gWackColorFadeDir *= -1;
					return;
				}
				if (Boss.gWackColorFade <= 0 && Boss.gWackColorFadeDir < 0)
				{
					Boss.gWackColorFade = 0;
					Boss.gWackColorFadeDir *= -1;
				}
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000FE4F File Offset: 0x0000E04F
		public virtual void Update()
		{
			this.Update(1f);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000FE5C File Offset: 0x0000E05C
		public virtual void DrawDeathBGTikis(Graphics g)
		{
			if (this.mHP <= 0f && this.mDrawDeathBGTikis)
			{
				int num = (int)((255f - this.mAlphaOverride) / (float)ZumasRevenge.Common._M(11));
				if (num > 255)
				{
					num = 255;
				}
				g.PushState();
				g.SetColorizeImages(true);
				g.SetColor(255, 255, 255, num);
				for (int i = 0; i < 13; i++)
				{
					ResID id = ResID.IMAGE_BOSSES_DEATH_BG_TIKIS_1 + i;
					Image imageByID = Res.GetImageByID(id);
					int num2 = ZumasRevenge.Common._DS(Res.GetOffsetXByID(id) - 160);
					int theY = ZumasRevenge.Common._DS(Res.GetOffsetYByID(id));
					g.DrawImage(imageByID, num2, theY);
					if (i != 7 && i != 5)
					{
						g.DrawImageMirror(imageByID, num2 + imageByID.GetWidth(), theY);
					}
				}
				g.PopState();
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000FF34 File Offset: 0x0000E134
		public virtual void Draw(Graphics g)
		{
			if (this.mHP > 0f && !this.mDoDeathExplosions && !this.mLevel.mBoard.DoingBossIntro())
			{
				for (int i = 0; i < this.mHulaDancers.size<HulaDancer>(); i++)
				{
					this.mHulaDancers[i].Draw(g);
				}
			}
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0000FF90 File Offset: 0x0000E190
		public void DrawDeathText(Graphics g, int alpha_override)
		{
			bool flag = false;
			for (int i = 0; i < this.mDeathText.size<BossText>(); i++)
			{
				BossText bossText = this.mDeathText[i];
				if (bossText.mAlpha <= 0f)
				{
					break;
				}
				if (i == this.mDeathText.size<BossText>() - 1 && bossText.mAlpha >= (float)ZumasRevenge.Common._M(200))
				{
					flag = true;
				}
				Font fontByID = Res.GetFontByID(ResID.FONT_BOSS_TAUNT);
				if (Localization.GetCurrentLanguage() == Localization.LanguageType.Language_CH)
				{
					fontByID.mAscent = 25;
				}
				g.SetFont(fontByID);
				int num = ZumasRevenge.Common._S(ZumasRevenge.Common._M(200)) + i * ZumasRevenge.Common._S(ZumasRevenge.Common._M1(30));
				g.SetColor(ZumasRevenge.Common._M(255), ZumasRevenge.Common._M1(255), ZumasRevenge.Common._M2(255), (int)((alpha_override == -1) ? bossText.mAlpha : ((float)alpha_override)));
				g.WriteWordWrapped(new Rect(0, num + Localization.GetCurrentFontOffsetY() * i, this.mApp.mWidth, this.mApp.mHeight), bossText.mText, -1, 0);
			}
			if (flag)
			{
				if (alpha_override != -1)
				{
					g.SetColorizeImages(true);
					g.SetColor(255, 255, 255, alpha_override);
				}
				Image imageByID = Res.GetImageByID(ResID.IMAGE_FROG_RIBBIT);
				g.DrawImage(imageByID, (this.mApp.mWidth - imageByID.mWidth) / 2, ZumasRevenge.Common._S(ZumasRevenge.Common._M(330)));
				g.SetColorizeImages(false);
				Font fontByID2 = Res.GetFontByID(ResID.FONT_SHAGLOUNGE28_STROKE);
				g.SetFont(fontByID2);
				g.SetColor(ZumasRevenge.Common._M(255), ZumasRevenge.Common._M1(255), ZumasRevenge.Common._M2(255));
				g.WriteString(TextManager.getInstance().getString(433), 0, ZumasRevenge.Common._DS(ZumasRevenge.Common._M(1170)), this.mApp.mWidth, 0);
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0001016F File Offset: 0x0000E36F
		public void DrawDeathText(Graphics g)
		{
			this.DrawDeathText(g, -1);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00010179 File Offset: 0x0000E379
		public virtual void DrawTopLevel(Graphics g)
		{
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001017B File Offset: 0x0000E37B
		public virtual void DrawBottomLevel(Graphics g)
		{
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00010180 File Offset: 0x0000E380
		public virtual void DrawBelowBalls(Graphics g)
		{
			if (this.mDrawRadius && this.mHP > 0f && !this.mDoDeathExplosions && !this.mLevel.mBoard.DoingBossIntro())
			{
				SexyColor color = new SexyColor(0, 0, 255, ZumasRevenge.Common._M(125));
				if (this.mRadiusColorChangeMode != 0 && this.mBombInRange)
				{
					color = new SexyColor(255, 0, 0, ZumasRevenge.Common._M(200));
				}
				g.SetColor(color);
				CommonGraphics.DrawCircle(g, ZumasRevenge.Common._S(this.mX), ZumasRevenge.Common._S(this.mY), (float)this.mProxBombRadius, ZumasRevenge.Common._S(ZumasRevenge.Common._M(30)));
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00010238 File Offset: 0x0000E438
		public virtual void DrawWordBubble(Graphics g)
		{
			if (this.mTauntQueue.size<TauntText>() == 0)
			{
				return;
			}
			TauntText tauntText = this.mTauntQueue[0];
			int wordBubbleAlpha = this.GetWordBubbleAlpha(tauntText);
			if (wordBubbleAlpha < 0)
			{
				return;
			}
			Font fontByID = Res.GetFontByID(ResID.FONT_MAIN22);
			Image theComponentImage;
			Rect theDest;
			Rect theRect;
			this.SetWordBubbleLayout(tauntText.mText, fontByID, out theComponentImage, out theDest, out theRect);
			g.SetFont(fontByID);
			g.SetColor(255, 255, 255, wordBubbleAlpha);
			if ((wordBubbleAlpha != 255 && this.mTauntQueue.size<TauntText>() == 1) || this.mAlphaOverride <= 254f)
			{
				g.SetColorizeImages(true);
			}
			g.DrawImageBox(theDest, theComponentImage);
			g.SetColor(0, 0, 0, wordBubbleAlpha);
			g.WriteWordWrapped(theRect, tauntText.mText, -1, 0);
			g.SetColorizeImages(false);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000102FC File Offset: 0x0000E4FC
		public int GetWordBubbleAlpha(TauntText inTauntText)
		{
			int num = inTauntText.mDelay - inTauntText.mUpdateCount;
			int num2 = 255;
			if (num <= 20)
			{
				num2 -= 26 * (20 - num);
			}
			if (this.mAlphaOverride <= 254f)
			{
				num2 = (int)Math.Min((float)num2, this.mAlphaOverride);
			}
			return num2;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001034C File Offset: 0x0000E54C
		public void SetWordBubbleLayout(string inText, Font inFont, out Image outBubbleBkg, out Rect outBubble, out Rect outInset)
		{
			Image imageByID = Res.GetImageByID(ResID.IMAGE_GUI_INGAME_BOSSUI);
			Image imageByID2 = Res.GetImageByID(ResID.IMAGE_BOSS_WORD_BUBBLE);
			Image imageByID3 = Res.GetImageByID(ResID.IMAGE_BOSS_WORD_BUBBLE_MIRROR);
			int num = (int)((float)this.mApp.GetScreenRect().mWidth - (float)imageByID.GetWidth() * 1.5f);
			int num2 = (int)((float)(num - ZumasRevenge.Common._S(this.mWidth)) * 0.4f);
			int num3 = ZumasRevenge.Common._S(5);
			int num4 = num2 - num3 * 2;
			int num5 = ZumasRevenge.Common._GetWordWrappedHeight(inText, inFont, num4);
			int num6 = num5 + num3 * 2;
			Image image = imageByID2;
			int num7 = (int)((float)image.GetWidth() * 0.23f);
			int num8 = (int)((float)image.GetHeight() * 0.22f);
			Rect rect = default(Rect);
			rect.mX = (int)ZumasRevenge.Common._S(this.mX + (float)this.mWidth * 0.5f);
			rect.mY = (int)(ZumasRevenge.Common._S(this.mY - (float)this.mHeight * 0.5f) + (float)this.mTauntTextYOff);
			rect.mWidth = num2 + num7 * 2;
			rect.mHeight = num6 + num8 * 2;
			int num9 = this.mApp.GetScreenRect().mX + num;
			if (rect.mX + rect.mWidth >= num9)
			{
				int num10 = rect.mWidth + ZumasRevenge.Common._S(this.mWidth);
				if (rect.mX - num10 >= 0)
				{
					image = imageByID3;
					rect.mX -= num10;
				}
			}
			outBubbleBkg = image;
			outBubble = rect;
			outInset = new Rect(rect.mX + num7 + num3, rect.mY + num8 + num3, num4, num5);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000104FD File Offset: 0x0000E6FD
		public virtual void FrogInitialized(Gun g)
		{
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000104FF File Offset: 0x0000E6FF
		public virtual void MouseDownDuringNoFire(int x, int y)
		{
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00010501 File Offset: 0x0000E701
		public virtual bool AllowFrogToFire()
		{
			return this.mLevel.HasReachedCruisingSpeed();
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0001050E File Offset: 0x0000E70E
		public virtual int GetFrogReloadType()
		{
			return -1;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00010511 File Offset: 0x0000E711
		public virtual void MoveToDeathPosition(float x, float y)
		{
			this.mDeathTX = x;
			this.mDeathTY = y;
			this.mDeathVX = (x - this.mX) / 200f;
			this.mDeathVY = (y - this.mY) / 200f;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0001054C File Offset: 0x0000E74C
		public void ShowAllDeathText()
		{
			this.mApp.SetCursor(ECURSOR.CURSOR_HAND);
			for (int i = 0; i < Enumerable.Count<BossText>(this.mDeathText); i++)
			{
				this.mDeathText[i].mAlpha = 255f;
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00010594 File Offset: 0x0000E794
		public void AddHulaEntry(float vx, float projvy, int spawn, int spawny, int proj_chance, int berserk_amt, int proj_range, int atype, int atime, int amnesty)
		{
			HulaEntry hulaEntry = new HulaEntry();
			hulaEntry.mBerserkAmt = berserk_amt;
			hulaEntry.mAmnesty = amnesty;
			hulaEntry.mProjVY = projvy;
			hulaEntry.mSpawnRate = spawn;
			hulaEntry.mVX = vx;
			hulaEntry.mSpawnY = spawny;
			hulaEntry.mProjChance = proj_chance;
			hulaEntry.mAttackTime = atime;
			hulaEntry.mAttackType = atype;
			hulaEntry.mProjRange = proj_range;
			this.mHulaEntryVec.Add(hulaEntry);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00010600 File Offset: 0x0000E800
		public List<HulaEntry> getHulaEntryList()
		{
			return this.mHulaEntryVec;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00010608 File Offset: 0x0000E808
		public void PlaySound(int soundid)
		{
			if (this.mApp.GetBoard().DoingIntros())
			{
				return;
			}
			if (this.mSounds[soundid] != -1)
			{
				this.mApp.PlaySample(this.mSounds[soundid]);
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0001063B File Offset: 0x0000E83B
		public virtual void ProximityBombActivated(float x, float y, int radius)
		{
			this.ForceActivation(true);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00010644 File Offset: 0x0000E844
		public void AddBerserkValue(int health_limit, string param_name, string value, ref string minval, ref string maxval, bool _override)
		{
			BerserkModifier berserkModifier = new BerserkModifier(param_name, value, minval, maxval, _override);
			bool flag = param_name.Length == 0;
			for (int i = 0; i < Enumerable.Count<BerserkTier>(this.mBerserkTiers); i++)
			{
				if (this.mBerserkTiers[i].mHealthLimit == health_limit)
				{
					if (!flag)
					{
						this.mBerserkTiers[i].mParams.Add(berserkModifier);
					}
					return;
				}
			}
			BerserkTier berserkTier = new BerserkTier(health_limit);
			if (!flag)
			{
				berserkTier.mParams.Add(berserkModifier);
			}
			for (int j = 0; j < Enumerable.Count<BerserkTier>(this.mBerserkTiers); j++)
			{
				if (health_limit > this.mBerserkTiers[j].mHealthLimit)
				{
					this.mBerserkTiers.Insert(j, berserkTier);
					return;
				}
			}
			this.mBerserkTiers.Add(berserkTier);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00010710 File Offset: 0x0000E910
		public List<BerserkTier> getBerserkTiers()
		{
			return this.mBerserkTiers;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00010718 File Offset: 0x0000E918
		public void AddBerserkValue(int health_limit, string param_name, string value)
		{
			string text = "";
			this.AddBerserkValue(health_limit, param_name, value, ref text, ref text, false);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0001073C File Offset: 0x0000E93C
		public virtual void SyncState(DataSync sync)
		{
			sync.SyncBoolean(ref this.mEatsBalls);
			sync.SyncFloat(ref this.mX);
			sync.SyncFloat(ref this.mY);
			sync.SyncFloat(ref this.mMaxHP);
			sync.SyncFloat(ref this.mHP);
			sync.SyncLong(ref this.mHulaAmnesty);
			sync.SyncFloat(ref this.mDHPDecPerHit.value);
			sync.SyncFloat(ref this.mDHPDecPerProxBomb.value);
			sync.SyncBoolean(ref this.mNeedsIntroSound);
			sync.SyncBoolean(ref this.mIsBerserk);
			sync.SyncLong(ref this.mWidth);
			sync.SyncLong(ref this.mHeight);
			sync.SyncLong(ref this.mUpdateCount);
			sync.SyncBoolean(ref this.mBombInRange);
			sync.SyncBoolean(ref this.mDoExplosion);
			if (sync.isWrite())
			{
				ZumasRevenge.Common.SerializePIEffect(this.mHitEffect, sync);
			}
			else
			{
				ZumasRevenge.Common.DeserializePIEffect(this.mHitEffect, sync);
				ZumasRevenge.Common.SetFXNumScale(this.mHitEffect, GameApp.gApp.Is3DAccelerated() ? 1f : 0.25f);
			}
			sync.SyncBoolean(ref this.mNeedsCompacting);
			sync.SyncLong(ref this.mStunTime);
			sync.SyncLong(ref this.mCurrTikiBossHealthRemoved);
			sync.SyncLong(ref this.mDTikiHealthRespawnAmt.value);
			sync.SyncFloat(ref this.mDeathVX);
			sync.SyncFloat(ref this.mDeathVY);
			sync.SyncFloat(ref this.mDeathTX);
			sync.SyncFloat(ref this.mDeathTY);
			sync.SyncLong(ref this.mWordBubbleTimer);
			sync.SyncLong(ref this.mDeathTimer);
			sync.SyncBoolean(ref this.mDoDeathExplosions);
			sync.SyncLong(ref this.mDWallDownTime.value);
			sync.SyncLong(ref this.mCurWallDownTime);
			sync.SyncLong(ref this.mImpatientTimer);
			sync.SyncLong(ref this.mCurrentHulaEntry.mBerserkAmt);
			sync.SyncFloat(ref this.mCurrentHulaEntry.mVX);
			sync.SyncFloat(ref this.mCurrentHulaEntry.mProjVY);
			sync.SyncLong(ref this.mCurrentHulaEntry.mSpawnRate);
			sync.SyncLong(ref this.mCurrentHulaEntry.mSpawnY);
			sync.SyncLong(ref this.mCurrentHulaEntry.mProjChance);
			sync.SyncLong(ref this.mCurrentHulaEntry.mAttackType);
			sync.SyncLong(ref this.mCurrentHulaEntry.mAttackTime);
			sync.SyncLong(ref this.mCurrentHulaEntry.mProjRange);
			sync.SyncLong(ref this.mCurrentHulaEntry.mAmnesty);
			this.SyncTauntTexts(sync, true);
			SexyBuffer buffer = sync.GetBuffer();
			if (sync.isWrite())
			{
				buffer.WriteLong((long)this.mHulaDancers.Count);
				for (int i = 0; i < this.mHulaDancers.Count; i++)
				{
					this.mHulaDancers[i].SyncState(sync);
				}
				buffer.WriteLong((long)this.mDeathText.Count);
				for (int j = 0; j < this.mDeathText.Count; j++)
				{
					buffer.WriteFloat(this.mDeathText[j].mAlpha);
				}
				buffer.WriteLong((long)this.mDeathExplosions.Count);
				for (int k = 0; k < this.mDeathExplosions.Count; k++)
				{
					ZumasRevenge.Common.SerializePIEffect(this.mDeathExplosions[k], sync);
				}
			}
			else
			{
				int num = (int)buffer.ReadLong();
				for (int l = 0; l < num; l++)
				{
					HulaDancer hulaDancer = new HulaDancer();
					hulaDancer.SyncState(sync);
					this.mHulaDancers.Add(hulaDancer);
				}
				int num2 = (int)buffer.ReadLong();
				for (int m = 0; m < num2; m++)
				{
					this.mDeathText[m].mAlpha = buffer.ReadFloat();
				}
				num2 = (int)buffer.ReadLong();
				for (int n = 0; n < num2; n++)
				{
					PIEffect pieffect = Res.GetPIEffectByID(ResID.PIEFFECT_NONRESIZE_DEATH_EXPLOSION).Duplicate();
					this.mDeathExplosions.Add(pieffect);
					ZumasRevenge.Common.DeserializePIEffect(pieffect, sync);
					ZumasRevenge.Common.SetFXNumScale(pieffect, GameApp.gApp.Is3DAccelerated() ? 1f : ZumasRevenge.Common._M(0.25f));
				}
			}
			for (int num3 = 0; num3 < this.mWalls.Count; num3++)
			{
				sync.SyncLong(ref this.mWalls[num3].mAlpha);
				sync.SyncLong(ref this.mWalls[num3].mAlphaFadeDir);
			}
			for (int num4 = 0; num4 < this.mTikis.Count; num4++)
			{
				sync.SyncLong(ref this.mTikis[num4].mAlphaFadeDir);
				sync.SyncFloat(ref this.mTikis[num4].mX);
				sync.SyncFloat(ref this.mTikis[num4].mY);
				sync.SyncBoolean(ref this.mTikis[num4].mWasHit);
				sync.SyncLong(ref this.mTikis[num4].mAlpha);
			}
			for (int num5 = 0; num5 < Boss.NUM_HEARTS; num5++)
			{
				sync.SyncLong(ref this.mHeartCels[num5]);
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00010C4C File Offset: 0x0000EE4C
		private void SyncTauntTexts(DataSync sync, bool clear)
		{
			if (sync.isRead())
			{
				if (clear)
				{
					this.mTauntQueue.Clear();
				}
				long num = sync.GetBuffer().ReadLong();
				int num2 = 0;
				while ((long)num2 < num)
				{
					TauntText tauntText = new TauntText();
					tauntText.SyncState(sync);
					this.mTauntQueue.Add(tauntText);
					num2++;
				}
				return;
			}
			sync.GetBuffer().WriteLong((long)this.mTauntQueue.Count);
			foreach (TauntText tauntText2 in this.mTauntQueue)
			{
				tauntText2.SyncState(sync);
			}
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00010D00 File Offset: 0x0000EF00
		public virtual bool Collides(Bullet b)
		{
			float num = (float)b.GetRadius() * ZumasRevenge.Common._M(0.75f);
			Rect r = new Rect((int)(b.GetX() - num), (int)(b.GetY() - num), (int)(num * 2f), (int)(num * 2f));
			bool flag = false;
			if (this.AllowFrogToFire())
			{
				flag = this.BulletIntersectsBoss(b);
				if (flag && !this.mEatsBalls)
				{
					flag = this.DoHit(b, false);
				}
				else if (flag && this.mEatsBalls)
				{
					this.BallEaten(b);
					this.PlaySound(6);
					return true;
				}
			}
			if (this.CollidesWithWall(b))
			{
				return true;
			}
			for (int i = 0; i < Enumerable.Count<HulaDancer>(this.mHulaDancers); i++)
			{
				if (this.mHulaDancers[i].Collided(r))
				{
					this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_NEW_HULAGIRLHIT));
					this.mHulaAmnesty = this.mCurrentHulaEntry.mAmnesty;
					this.mHulaDancers[i].Disable();
					return true;
				}
			}
			bool flag2 = false;
			if (this.AllowFrogToFire())
			{
				for (int j = 0; j < Enumerable.Count<Tiki>(this.mTikis); j++)
				{
					if (!this.mTikis[j].mWasHit && this.mTikis[j].mAlphaFadeDir >= 0)
					{
						bool flag3 = false;
						if (this.mTikis[j].Collides(b, ref flag3))
						{
							this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_TIKI_HIT));
							if (Enumerable.Count<Tiki>(this.mTikis) == Enumerable.Count<BossWall>(this.mWalls))
							{
								BossWall bossWall = this.mWalls[j];
								bossWall.mAlphaFadeDir = -1;
								this.TikiHit(j);
								int num2 = 0;
								for (int k = 0; k < Enumerable.Count<Tiki>(this.mTikis); k++)
								{
									if (this.mTikis[k].mWasHit)
									{
										num2++;
									}
								}
								if (num2 == Enumerable.Count<Tiki>(this.mTikis))
								{
									this.mCurWallDownTime = this.mWallDownTime;
								}
							}
							return true;
						}
					}
				}
			}
			if (flag && this.mResetWallsOnBossHit)
			{
				for (int l = 0; l < Enumerable.Count<BossWall>(this.mWalls); l++)
				{
					this.mWalls[l].mAlphaFadeDir = 1;
				}
				for (int m = 0; m < Enumerable.Count<Tiki>(this.mTikis); m++)
				{
					this.mTikis[m].mAlphaFadeDir = 1;
					this.mTikis[m].mWasHit = false;
				}
			}
			return flag || flag2;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00010F96 File Offset: 0x0000F196
		public virtual void ForceActivation(bool from_prox_bomb)
		{
			this.DoHit(null, from_prox_bomb);
		}

		// Token: 0x06000474 RID: 1140
		public abstract Boss Instantiate();

		// Token: 0x06000475 RID: 1141 RVA: 0x00010FA4 File Offset: 0x0000F1A4
		public virtual void PostInstantiationHook(Boss source_boss)
		{
			this.mFParamPointerMap.Clear();
			this.mIParamPointerMap.Clear();
			this.mBParamPointerMap.Clear();
			this.AddParamPointer("WallDownTime", this.mDWallDownTime);
			this.AddParamPointer("HPDecPerHit", this.mDHPDecPerHit);
			this.AddParamPointer("HPDecPerProxBomb", this.mDHPDecPerProxBomb);
			this.AddParamPointer("TikiHealthRespawn", this.mDTikiHealthRespawnAmt);
			this.mTikis.Clear();
			foreach (Tiki tiki in source_boss.mTikis)
			{
				this.AddTiki((int)tiki.mX, (int)tiki.mY, tiki.mId, tiki.mRailEndX - tiki.mRailStartX, tiki.mRailEndY - tiki.mRailStartY, tiki.mTravelTime);
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0001109C File Offset: 0x0000F29C
		public virtual bool CanAdvanceBalls()
		{
			return true;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0001109F File Offset: 0x0000F29F
		public virtual void PlayerStartedFiring()
		{
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000110A1 File Offset: 0x0000F2A1
		public virtual void SetXY(float x, float y)
		{
			this.mX = x;
			this.mY = y;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x000110B1 File Offset: 0x0000F2B1
		public virtual void SetX(float x)
		{
			this.mX = x;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x000110BA File Offset: 0x0000F2BA
		public virtual void SetY(float y)
		{
			this.mY = y;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x000110C3 File Offset: 0x0000F2C3
		public virtual void SetHPDecPerHit(float hp)
		{
			this.mHPDecPerHit = hp;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x000110CC File Offset: 0x0000F2CC
		public virtual void SetHPDecPerHitProxBomb(float hp)
		{
			this.mHPDecPerProxBomb = hp;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000110D5 File Offset: 0x0000F2D5
		public virtual void Stun(int stime)
		{
			this.mStunTime = stime;
			this.mApp.PlaySample(Res.GetSoundByID(ResID.SOUND_BOSS_STUNNED));
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x000110F4 File Offset: 0x0000F2F4
		public virtual void SetHP(float hp)
		{
			float num = this.mHP;
			this.mHP = hp;
			int num2 = (int)((num - this.mHP) / this.mHPDecPerHit);
			for (int i = 0; i < num2; i++)
			{
				this.DecHearts(this.mHeartPieceDecAmt);
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00011138 File Offset: 0x0000F338
		public bool IsStunned()
		{
			return this.mStunTime > 0;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00011143 File Offset: 0x0000F343
		public float GetHP()
		{
			return this.mHP;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0001114B File Offset: 0x0000F34B
		public virtual int GetX()
		{
			return (int)this.mX;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00011154 File Offset: 0x0000F354
		public virtual int GetY()
		{
			return (int)this.mY;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0001115D File Offset: 0x0000F35D
		public virtual int GetTopLeftX()
		{
			return (int)this.mX - this.mWidth / 2;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001116F File Offset: 0x0000F36F
		public virtual int GetTopLeftY()
		{
			return (int)this.mY - this.mHeight / 2;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00011181 File Offset: 0x0000F381
		public int GetWidth()
		{
			return this.mWidth;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00011189 File Offset: 0x0000F389
		public int GetHeight()
		{
			return this.mHeight;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00011191 File Offset: 0x0000F391
		public bool IsImpatient()
		{
			return this.mImpatientTimer == 0;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0001119C File Offset: 0x0000F39C
		public bool IsHitByExplosion(float x, float y, int radius)
		{
			return MathUtils.CirclesIntersect(x, y, this.mX, this.mY, (float)(this.mProxBombRadius + 56 + ZumasRevenge.Common.GetDefaultBallRadius()));
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x000111C1 File Offset: 0x0000F3C1
		public virtual void InitParam()
		{
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x000111C4 File Offset: 0x0000F3C4
		public void CopyFrom(Boss rhs)
		{
			this.mX = rhs.mX;
			this.mY = rhs.mY;
			this.mMaxHP = rhs.mMaxHP;
			this.mHP = rhs.mHP;
			this.mWidth = rhs.mWidth;
			this.mHeight = rhs.mHeight;
			this.mUpdateCount = rhs.mUpdateCount;
			this.mHPDecPerHit = rhs.mHPDecPerHit;
			this.mHPDecPerProxBomb = rhs.mHPDecPerProxBomb;
			this.mShakeXAmt = rhs.mShakeXAmt;
			this.mShakeYAmt = rhs.mShakeYAmt;
			this.mShouldDoDeathExplosions = rhs.mShouldDoDeathExplosions;
			this.mShakeXOff = rhs.mShakeXOff;
			this.mShakeYOff = rhs.mShakeYOff;
			this.mAllowLevelDDS = rhs.mAllowLevelDDS;
			this.mDoExplosion = rhs.mDoExplosion;
			this.mNeedsCompacting = rhs.mNeedsCompacting;
			this.mAllowCompacting = rhs.mAllowCompacting;
			this.mHeartXOff = rhs.mHeartXOff;
			this.mHeartYOff = rhs.mHeartYOff;
			this.mResetWallTimerOnTikiHit = rhs.mResetWallTimerOnTikiHit;
			this.mResetWallsOnBossHit = rhs.mResetWallsOnBossHit;
			this.mWallDownTime = rhs.mWallDownTime;
			this.mCurWallDownTime = rhs.mCurWallDownTime;
			this.mStunTime = rhs.mStunTime;
			this.mCurrTikiBossHealthRemoved = rhs.mCurrTikiBossHealthRemoved;
			this.mTikiHealthRespawnAmt = rhs.mTikiHealthRespawnAmt;
			this.mNum = rhs.mNum;
			this.mIsBerserk = rhs.mIsBerserk;
			this.mApp = GameApp.gApp;
			this.mEatsBalls = rhs.mEatsBalls;
			this.mImpatientTimer = rhs.mImpatientTimer;
			this.mBombFreqMax = rhs.mBombFreqMax;
			this.mBombFreqMin = rhs.mBombFreqMin;
			this.mBombDuration = rhs.mBombDuration;
			this.mProxBombRadius = rhs.mProxBombRadius;
			this.mDrawRadius = rhs.mDrawRadius;
			this.mBossRadius = rhs.mBossRadius;
			this.mNeedsIntroSound = rhs.mNeedsIntroSound;
			this.mBombInRange = rhs.mBombInRange;
			this.mRadiusColorChangeMode = rhs.mRadiusColorChangeMode;
			this.mDoDeathExplosions = rhs.mDoDeathExplosions;
			this.mDeathTimer = rhs.mDeathTimer;
			this.mWordBubbleTimer = rhs.mWordBubbleTimer;
			this.mSepiaImage = rhs.mSepiaImage;
			this.mDeathTX = rhs.mDeathTX;
			this.mDeathTY = rhs.mDeathTY;
			this.mDeathVX = rhs.mDeathVX;
			this.mDeathVY = rhs.mDeathVY;
			this.mExplosionRate = rhs.mExplosionRate;
			this.mBossRadiusYOff = rhs.mBossRadiusYOff;
			this.mHulaAmnesty = rhs.mHulaAmnesty;
			this.mBandagedImg = rhs.mBandagedImg;
			this.mAlphaOverride = rhs.mAlphaOverride;
			this.mBandagedXOff = rhs.mBandagedXOff;
			this.mBandagedYOff = rhs.mBandagedYOff;
			this.mDrawDeathBGTikis = rhs.mDrawDeathBGTikis;
			this.mTauntTextYOff = rhs.mTauntTextYOff;
			this.mResPrefix = rhs.mResPrefix;
			this.mHitEffect = rhs.mHitEffect;
			this.mDeathText.Clear();
			this.mDeathText.AddRange(rhs.mDeathText.ToArray());
			this.mTauntText.Clear();
			this.mTauntText.AddRange(rhs.mTauntText.ToArray());
			this.mTikis.Clear();
			for (int i = 0; i < rhs.mTikis.Count; i++)
			{
				this.mTikis.Add(new Tiki(rhs.mTikis[i]));
			}
			this.mHulaDancers.Clear();
			for (int j = 0; j < rhs.mHulaDancers.Count; j++)
			{
				this.mHulaDancers.Add(new HulaDancer(rhs.mHulaDancers[j]));
			}
			this.mHulaEntryVec.Clear();
			for (int k = 0; k < rhs.mHulaEntryVec.Count; k++)
			{
				this.mHulaEntryVec.Add(new HulaEntry(rhs.mHulaEntryVec[k]));
			}
			if (rhs.mCurrentHulaEntry != null)
			{
				this.mCurrentHulaEntry = new HulaEntry(rhs.mCurrentHulaEntry);
			}
			Dictionary<string, ParamData<float>>.Enumerator enumerator = rhs.mFParamPointerMap.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Dictionary<string, ParamData<float>> dictionary = this.mFParamPointerMap;
				KeyValuePair<string, ParamData<float>> keyValuePair = enumerator.Current;
				if (dictionary[keyValuePair.Key] != null)
				{
					Dictionary<string, ParamData<float>> dictionary2 = this.mFParamPointerMap;
					KeyValuePair<string, ParamData<float>> keyValuePair2 = enumerator.Current;
					ParamData<float> paramData = dictionary2[keyValuePair2.Key];
					KeyValuePair<string, ParamData<float>> keyValuePair3 = enumerator.Current;
					paramData.value = keyValuePair3.Value.value;
				}
			}
			Dictionary<string, ParamData<int>>.Enumerator enumerator2 = rhs.mIParamPointerMap.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				Dictionary<string, ParamData<int>> dictionary3 = this.mIParamPointerMap;
				KeyValuePair<string, ParamData<int>> keyValuePair4 = enumerator2.Current;
				if (dictionary3[keyValuePair4.Key] != null)
				{
					Dictionary<string, ParamData<int>> dictionary4 = this.mIParamPointerMap;
					KeyValuePair<string, ParamData<int>> keyValuePair5 = enumerator2.Current;
					ParamData<int> paramData2 = dictionary4[keyValuePair5.Key];
					KeyValuePair<string, ParamData<int>> keyValuePair6 = enumerator2.Current;
					paramData2.value = keyValuePair6.Value.value;
				}
			}
			Dictionary<string, ParamData<bool>>.Enumerator enumerator3 = rhs.mBParamPointerMap.GetEnumerator();
			while (enumerator3.MoveNext())
			{
				Dictionary<string, ParamData<bool>> dictionary5 = this.mBParamPointerMap;
				KeyValuePair<string, ParamData<bool>> keyValuePair7 = enumerator3.Current;
				if (dictionary5[keyValuePair7.Key] != null)
				{
					Dictionary<string, ParamData<bool>> dictionary6 = this.mBParamPointerMap;
					KeyValuePair<string, ParamData<bool>> keyValuePair8 = enumerator3.Current;
					ParamData<bool> paramData3 = dictionary6[keyValuePair8.Key];
					KeyValuePair<string, ParamData<bool>> keyValuePair9 = enumerator3.Current;
					paramData3.value = keyValuePair9.Value.value;
				}
			}
			this.mBerserkTiers.Clear();
			for (int l = 0; l < rhs.mBerserkTiers.Count; l++)
			{
				this.mBerserkTiers.Add(new BerserkTier(rhs.mBerserkTiers[l]));
			}
			this.mWalls.Clear();
			for (int m = 0; m < rhs.mWalls.Count; m++)
			{
				this.mWalls.Add(new BossWall(rhs.mWalls[m]));
			}
			this.mDeathExplosions.Clear();
			for (int n = 0; n < rhs.mDeathExplosions.Count; n++)
			{
				this.mDeathExplosions.Add(this.mDeathExplosions[n]);
			}
			this.mTauntQueue.Clear();
			for (int num = 0; num < rhs.mTauntQueue.Count; num++)
			{
				this.mTauntQueue.Add(new TauntText(this.mTauntQueue[num]));
			}
			for (int num2 = 0; num2 < rhs.mSounds.Length; num2++)
			{
				this.mSounds[num2] = rhs.mSounds[num2];
			}
			for (int num3 = 0; num3 < rhs.mHeartCels.Length; num3++)
			{
				this.mHeartCels[num3] = rhs.mHeartCels[num3];
			}
		}

		// Token: 0x040000EC RID: 236
		public static float gBerserkTextAlpha;

		// Token: 0x040000ED RID: 237
		public static float gBerserkTextY;

		// Token: 0x040000EE RID: 238
		public static float gImpatientTextAlpha;

		// Token: 0x040000EF RID: 239
		public static float gImpatientTextY;

		// Token: 0x040000F0 RID: 240
		protected static int gWackColorFade = 0;

		// Token: 0x040000F1 RID: 241
		protected static int gWackColorFadeDir = 2;

		// Token: 0x040000F2 RID: 242
		protected static int NUM_HEARTS = 5;

		// Token: 0x040000F3 RID: 243
		protected static int FNTT_last_idx = 0;

		// Token: 0x040000F4 RID: 244
		protected static int last_idx = 0;

		// Token: 0x040000F5 RID: 245
		protected Dictionary<string, ParamData<float>> mFParamPointerMap = new Dictionary<string, ParamData<float>>();

		// Token: 0x040000F6 RID: 246
		protected Dictionary<string, ParamData<int>> mIParamPointerMap = new Dictionary<string, ParamData<int>>();

		// Token: 0x040000F7 RID: 247
		protected Dictionary<string, ParamData<bool>> mBParamPointerMap = new Dictionary<string, ParamData<bool>>();

		// Token: 0x040000F8 RID: 248
		protected ParamData<int> mDWallDownTime = new ParamData<int>();

		// Token: 0x040000F9 RID: 249
		protected ParamData<float> mDHPDecPerHit = new ParamData<float>();

		// Token: 0x040000FA RID: 250
		protected ParamData<float> mDHPDecPerProxBomb = new ParamData<float>();

		// Token: 0x040000FB RID: 251
		protected ParamData<int> mDTikiHealthRespawnAmt = new ParamData<int>();

		// Token: 0x040000FC RID: 252
		public bool mShouldDoDeathExplosions;

		// Token: 0x040000FD RID: 253
		public bool mDoDeathExplosions;

		// Token: 0x040000FE RID: 254
		public bool mNeedsIntroSound;

		// Token: 0x040000FF RID: 255
		public bool mEatsBalls;

		// Token: 0x04000100 RID: 256
		public GameApp mApp;

		// Token: 0x04000101 RID: 257
		public Level mLevel;

		// Token: 0x04000102 RID: 258
		public bool mAllowCompacting;

		// Token: 0x04000103 RID: 259
		public int mShakeXAmt;

		// Token: 0x04000104 RID: 260
		public int mShakeYAmt;

		// Token: 0x04000105 RID: 261
		public int mHeartXOff;

		// Token: 0x04000106 RID: 262
		public int mHeartYOff;

		// Token: 0x04000107 RID: 263
		public bool mResetWallsOnBossHit;

		// Token: 0x04000108 RID: 264
		public bool mResetWallTimerOnTikiHit;

		// Token: 0x04000109 RID: 265
		public bool mAllowLevelDDS;

		// Token: 0x0400010A RID: 266
		public bool mDrawRadius;

		// Token: 0x0400010B RID: 267
		public int mRadiusColorChangeMode;

		// Token: 0x0400010C RID: 268
		public int mCurWallDownTime;

		// Token: 0x0400010D RID: 269
		public int mCurrTikiBossHealthRemoved;

		// Token: 0x0400010E RID: 270
		public int mImpatientTimer;

		// Token: 0x0400010F RID: 271
		public int mNum;

		// Token: 0x04000110 RID: 272
		public string mName = "";

		// Token: 0x04000111 RID: 273
		public string mResPrefix = "";

		// Token: 0x04000112 RID: 274
		public int mBombFreqMin;

		// Token: 0x04000113 RID: 275
		public int mBombFreqMax;

		// Token: 0x04000114 RID: 276
		public int mBombDuration;

		// Token: 0x04000115 RID: 277
		public int mProxBombRadius;

		// Token: 0x04000116 RID: 278
		public int mBossRadius;

		// Token: 0x04000117 RID: 279
		public int mBossRadiusYOff;

		// Token: 0x04000118 RID: 280
		public int mVolcanoOffscreenDelay;

		// Token: 0x04000119 RID: 281
		public List<BossText> mDeathText = new List<BossText>();

		// Token: 0x0400011A RID: 282
		public string mWordBubbleText = "";

		// Token: 0x0400011B RID: 283
		public string mSepiaImagePath = "";

		// Token: 0x0400011C RID: 284
		public string mResGroup = "";

		// Token: 0x0400011D RID: 285
		public List<TauntText> mTauntText = new List<TauntText>();

		// Token: 0x0400011E RID: 286
		public DeviceImage mSepiaImage;

		// Token: 0x0400011F RID: 287
		public PIEffect mHitEffect;

		// Token: 0x04000120 RID: 288
		public float mAlphaOverride;

		// Token: 0x04000121 RID: 289
		public List<Tiki> mTikis = new List<Tiki>();

		// Token: 0x04000122 RID: 290
		protected int mTauntTextYOff;

		// Token: 0x04000123 RID: 291
		protected Image mBandagedImg;

		// Token: 0x04000124 RID: 292
		protected int mBandagedXOff;

		// Token: 0x04000125 RID: 293
		protected int mBandagedYOff;

		// Token: 0x04000126 RID: 294
		protected List<HulaDancer> mHulaDancers = new List<HulaDancer>();

		// Token: 0x04000127 RID: 295
		protected List<HulaEntry> mHulaEntryVec = new List<HulaEntry>();

		// Token: 0x04000128 RID: 296
		protected HulaEntry mCurrentHulaEntry = new HulaEntry();

		// Token: 0x04000129 RID: 297
		protected List<BerserkTier> mBerserkTiers = new List<BerserkTier>();

		// Token: 0x0400012A RID: 298
		protected List<BossWall> mWalls = new List<BossWall>();

		// Token: 0x0400012B RID: 299
		protected List<PIEffect> mDeathExplosions = new List<PIEffect>();

		// Token: 0x0400012C RID: 300
		protected List<TauntText> mTauntQueue = new List<TauntText>();

		// Token: 0x0400012D RID: 301
		protected int[] mSounds = new int[10];

		// Token: 0x0400012E RID: 302
		protected int mExplosionRate;

		// Token: 0x0400012F RID: 303
		protected bool mDrawDeathBGTikis;

		// Token: 0x04000130 RID: 304
		protected float mX;

		// Token: 0x04000131 RID: 305
		protected float mY;

		// Token: 0x04000132 RID: 306
		protected float mMaxHP;

		// Token: 0x04000133 RID: 307
		protected float mHP;

		// Token: 0x04000134 RID: 308
		protected float mDeathTX;

		// Token: 0x04000135 RID: 309
		protected float mDeathTY;

		// Token: 0x04000136 RID: 310
		protected float mDeathVX;

		// Token: 0x04000137 RID: 311
		protected float mDeathVY;

		// Token: 0x04000138 RID: 312
		protected int mHulaAmnesty;

		// Token: 0x04000139 RID: 313
		protected int mWidth;

		// Token: 0x0400013A RID: 314
		protected int mHeight;

		// Token: 0x0400013B RID: 315
		protected int mShakeXOff;

		// Token: 0x0400013C RID: 316
		protected int mShakeYOff;

		// Token: 0x0400013D RID: 317
		protected int mUpdateCount;

		// Token: 0x0400013E RID: 318
		protected int mHeartPieceDecAmt;

		// Token: 0x0400013F RID: 319
		protected int mHeartPieceDecAmtProxBomb;

		// Token: 0x04000140 RID: 320
		protected int[] mHeartCels = new int[Boss.NUM_HEARTS];

		// Token: 0x04000141 RID: 321
		protected int mStunTime;

		// Token: 0x04000142 RID: 322
		protected int mDeathTimer;

		// Token: 0x04000143 RID: 323
		protected bool mDoExplosion;

		// Token: 0x04000144 RID: 324
		protected bool mNeedsCompacting;

		// Token: 0x04000145 RID: 325
		protected bool mIsBerserk;

		// Token: 0x04000146 RID: 326
		protected bool mBombInRange;

		// Token: 0x04000147 RID: 327
		protected int mWordBubbleTimer;

		// Token: 0x04000148 RID: 328
		protected bool mCleanHeart = true;

		// Token: 0x02000016 RID: 22
		public enum Sound
		{
			// Token: 0x0400014A RID: 330
			Sound_Die,
			// Token: 0x0400014B RID: 331
			Sound_Enrage,
			// Token: 0x0400014C RID: 332
			Sound_Fire,
			// Token: 0x0400014D RID: 333
			Sound_BossHit,
			// Token: 0x0400014E RID: 334
			Sound_PlayerHit,
			// Token: 0x0400014F RID: 335
			Sound_Intro,
			// Token: 0x04000150 RID: 336
			Sound_EatBalls,
			// Token: 0x04000151 RID: 337
			Sound_Roar,
			// Token: 0x04000152 RID: 338
			Sound_Teleport,
			// Token: 0x04000153 RID: 339
			Sound_ShieldHit,
			// Token: 0x04000154 RID: 340
			Max_Sounds
		}

		// Token: 0x02000017 RID: 23
		public enum ColorChange
		{
			// Token: 0x04000156 RID: 342
			ColorChange_Never,
			// Token: 0x04000157 RID: 343
			ColorChange_BombInRange,
			// Token: 0x04000158 RID: 344
			ColorChange_NotBehind
		}
	}
}
