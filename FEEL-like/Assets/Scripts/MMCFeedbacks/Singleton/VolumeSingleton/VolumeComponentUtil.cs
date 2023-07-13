using UnityEngine.Rendering;

namespace MMCFeedbacks.ScreenFX
{
    public static class VolumeComponentUtil
    {
        public static T AddVolumeComponent<T>(Volume volume) where T : VolumeComponent
        {
            if (volume.profile.TryGet(out T t)) return t;
            var component =
                volume.profile.Add(typeof(T));
            return component as T;
        }

        public static void ActiveAllProperty(VolumeComponent volumeComponent)
        {
            foreach (var t in volumeComponent.parameters) t.overrideState = true;
        }
    }
}