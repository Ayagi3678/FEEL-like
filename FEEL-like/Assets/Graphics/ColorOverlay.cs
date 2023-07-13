using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Graphics
{
    [Serializable]
    [VolumeComponentMenu("CustomEffect/ColorOverlay")]
    public class ColorOverlay : VolumeComponent,IPostProcessComponent
    {
        public bool IsActive() => tintColor != Color.white;
        public bool IsTileCompatible() => false;

        public ClampedFloatParameter intensity = new (0,0,1);
        public ColorParameter tintColor = new (Color.white);
    }
}