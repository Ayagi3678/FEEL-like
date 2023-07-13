using UnityEngine;
using UnityEngine.Rendering;

namespace Graphics
{
    public class ColorOverlayFeature : CustomPostProcessFeature<ColorOverlay>
    {
        private readonly int _thresholdPropertyId = Shader.PropertyToID("_Threshold");
        private readonly int _tintColorPropertyId = Shader.PropertyToID("_TintColor");
        private static readonly int _mainTexId = Shader.PropertyToID("_MainTex");
        protected override void SetMaterialProperty(ref RTHandle source, ref CommandBuffer cmd,ref Material material, ref ColorOverlay t)
        {
            material.SetColor(_tintColorPropertyId, t.tintColor.value);
            material.SetFloat(_thresholdPropertyId, t.intensity.value);
            cmd.SetGlobalTexture(_mainTexId,source);
        }
    }
}