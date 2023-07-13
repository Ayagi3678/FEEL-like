using UnityEngine;
using UnityEngine.Rendering;

namespace Graphics
{
    public class GlitchFeature : CustomPostProcessFeature<Glitch>
    {
        private readonly int _thresholdPropertyId = Shader.PropertyToID("_Threshold");
        private readonly int _noiseAmountPropertyId = Shader.PropertyToID("_NoiseAmount");
        private readonly int _widthPropertyId = Shader.PropertyToID("_Width");
        private readonly int _speedPropertyId = Shader.PropertyToID("_Speed");

        private static readonly int MainTexId = Shader.PropertyToID("_MainTex");
        protected override void SetMaterialProperty(ref RTHandle source, ref CommandBuffer cmd, ref Material material, ref Glitch t)
        {
            material.SetFloat(_thresholdPropertyId, t.intensity.value);
            material.SetFloat(_noiseAmountPropertyId, t.noiseScale.value);
            material.SetFloat(_widthPropertyId,t.width.value);
            material.SetFloat(_speedPropertyId,t.speed.value);
            cmd.SetGlobalTexture(MainTexId,source);
        }
    }
}