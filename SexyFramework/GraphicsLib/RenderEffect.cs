using System;

namespace SexyFramework.GraphicsLib
{
	// Token: 0x020000F7 RID: 247
	public abstract class RenderEffect
	{
		// Token: 0x0600072B RID: 1835 RVA: 0x00025E61 File Offset: 0x00024061
		public virtual void Dispose()
		{
		}

		// Token: 0x0600072C RID: 1836
		public abstract RenderDevice3D GetDevice();

		// Token: 0x0600072D RID: 1837
		public abstract RenderEffectDefinition GetDefinition();

		// Token: 0x0600072E RID: 1838
		public abstract void SetParameter(string inParamName, float[] inFloatData, uint inFloatCount);

		// Token: 0x0600072F RID: 1839
		public abstract void SetParameter(string inParamName, float inFloatData);

		// Token: 0x06000730 RID: 1840 RVA: 0x00025E63 File Offset: 0x00024063
		public void SetFloat(string inParamName, float inValue)
		{
			this.SetParameter(inParamName, inValue);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00025E6D File Offset: 0x0002406D
		public void SetVector4(string inParamName, float[] inValue)
		{
			this.SetParameter(inParamName, inValue, 4U);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00025E78 File Offset: 0x00024078
		public void SetVector3(string inParamName, float[] inValue)
		{
			this.SetVector4(inParamName, new float[]
			{
				inValue[0],
				inValue[1],
				inValue[2],
				1f
			});
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00025EAE File Offset: 0x000240AE
		public virtual void SetMatrix(string inParamName, float[] inValue)
		{
			this.SetParameter(inParamName, inValue, 16U);
		}

		// Token: 0x06000734 RID: 1844
		public abstract void GetParameterBySemantic(uint inSemantic, float[] outFloatData, uint inMaxFloatCount);

		// Token: 0x06000735 RID: 1845 RVA: 0x00025EBA File Offset: 0x000240BA
		public void SetCurrentTechnique(string inName)
		{
			this.SetCurrentTechnique(inName, true);
		}

		// Token: 0x06000736 RID: 1846
		public abstract void SetCurrentTechnique(string inName, bool inCheckValid);

		// Token: 0x06000737 RID: 1847
		public abstract string GetCurrentTechniqueName();

		// Token: 0x06000738 RID: 1848 RVA: 0x00025EC4 File Offset: 0x000240C4
		public int Begin(out object outRunHandle)
		{
			return this.Begin(out outRunHandle, new HRenderContext());
		}

		// Token: 0x06000739 RID: 1849
		public abstract int Begin(out object outRunHandle, HRenderContext inRenderContext);

		// Token: 0x0600073A RID: 1850
		public abstract void BeginPass(object inRunHandle, int inPass);

		// Token: 0x0600073B RID: 1851
		public abstract void EndPass(object inRunHandle, int inPass);

		// Token: 0x0600073C RID: 1852
		public abstract void End(object inRunHandle);

		// Token: 0x0600073D RID: 1853
		public abstract bool PassUsesVertexShader(int inPass);

		// Token: 0x0600073E RID: 1854
		public abstract bool PassUsesPixelShader(int inPass);
	}
}
