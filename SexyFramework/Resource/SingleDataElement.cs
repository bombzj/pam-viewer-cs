using System;
using System.Text;

namespace SexyFramework.Resource
{
	// Token: 0x02000185 RID: 389
	public class SingleDataElement : DataElement
	{
		// Token: 0x06000DB0 RID: 3504 RVA: 0x00044F02 File Offset: 0x00043102
		public SingleDataElement()
		{
			this.mIsList = false;
			this.mValue = null;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00044F23 File Offset: 0x00043123
		public SingleDataElement(string theString)
		{
			this.mString = new StringBuilder(theString);
			this.mIsList = false;
			this.mValue = null;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00044F50 File Offset: 0x00043150
		public override void Dispose()
		{
			if (this.mValue != null && this.mValue != null)
			{
				this.mValue.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00044F74 File Offset: 0x00043174
		public override DataElement Duplicate()
		{
			SingleDataElement singleDataElement = new SingleDataElement();
			singleDataElement.mString = this.mString;
			if (this.mValue != null)
			{
				singleDataElement.mValue = this.mValue.Duplicate();
			}
			return singleDataElement;
		}

		// Token: 0x04000B1E RID: 2846
		public StringBuilder mString = new StringBuilder();

		// Token: 0x04000B1F RID: 2847
		public DataElement mValue;
	}
}
