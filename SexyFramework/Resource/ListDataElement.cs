using System;
using System.Collections.Generic;

namespace SexyFramework.Resource
{
	// Token: 0x02000186 RID: 390
	public class ListDataElement : DataElement
	{
		// Token: 0x06000DB4 RID: 3508 RVA: 0x00044FAD File Offset: 0x000431AD
		public ListDataElement()
		{
			this.mIsList = true;
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00044FC8 File Offset: 0x000431C8
		public ListDataElement(ListDataElement theListDataElement)
		{
			this.mIsList = true;
			for (int i = 0; i < theListDataElement.mElementVector.Count; i++)
			{
				this.mElementVector.Add(theListDataElement.mElementVector[i].Duplicate());
			}
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00045020 File Offset: 0x00043220
		public override void Dispose()
		{
			for (int i = 0; i < this.mElementVector.Count; i++)
			{
				if (this.mElementVector[i] != null)
				{
					this.mElementVector[i].Dispose();
				}
			}
			base.Dispose();
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00045068 File Offset: 0x00043268
		public ListDataElement CopyFrom(ListDataElement theListDataElement)
		{
			for (int i = 0; i < this.mElementVector.Count; i++)
			{
				if (this.mElementVector[i] != null)
				{
					this.mElementVector[i].Dispose();
				}
			}
			this.mElementVector.Clear();
			for (int j = 0; j < theListDataElement.mElementVector.Count; j++)
			{
				this.mElementVector.Add(theListDataElement.mElementVector[j].Duplicate());
			}
			return this;
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x000450E8 File Offset: 0x000432E8
		public override DataElement Duplicate()
		{
			return new ListDataElement(this);
		}

		// Token: 0x04000B20 RID: 2848
		public List<DataElement> mElementVector = new List<DataElement>();
	}
}
