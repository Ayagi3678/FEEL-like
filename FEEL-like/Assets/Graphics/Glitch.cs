using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Graphics
{
    [Serializable]
    [VolumeComponentMenu("CustomEffect/Glitch")]
    public class Glitch : VolumeComponent , IPostProcessComponent
    {
        public bool IsActive() => noiseScale != 0;
        public bool IsTileCompatible() => false;

        public ClampedFloatParameter intensity=new (0,0,1);
        public ClampedFloatParameter speed = new(2, 0, 10);
        public FloatParameter noiseScale=new (100);
        public ClampedFloatParameter width = new(1, 0, 10);
    }
}
