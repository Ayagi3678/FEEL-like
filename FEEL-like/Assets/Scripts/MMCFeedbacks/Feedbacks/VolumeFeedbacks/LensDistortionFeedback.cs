using System;
using DG.Tweening;
using MMCFeedbacks.Attribute;
using MMCFeedbacks.ScreenFX;
using MMCFeedbacks.Singleton;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static MimicFeedBacker.Feedbacks.FeedbackEnums;

namespace MMCFeedbacks
{
    [Serializable]
    public class LensDistortionFeedback : IFeedback
    {
        public bool isActive = true;
        public string label = "Lens Distortion";

        [Header("Animation")] public EaseType easeType = EaseType.Curve;

        public float duration = 1;

        [Space(10)] [ConditionalDisable(nameof(easeType), (int)EaseType.Curve, conditionalInvisible: true)]
        public AnimationCurve easeCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [ConditionalDisable(nameof(easeType), (int)EaseType.Easing, conditionalInvisible: true)]
        public Ease easing = Ease.Linear;

        [SerializeField] private float start = 1;
        [SerializeField] private float end;

        [Header("LensDistortion")] [Range(-1, 1)] [SerializeField]
        private float intensity = .5f;

        [Range(-1, 1)] [SerializeField] private float xMultiplier = 1;
        [Range(-1, 1)] [SerializeField] private float yMultiplier = 1;
        public Vector2 center = Vector2.one * .5f;
        private Tween _tween;
        private Volume _volume;
        public string MenuString => "Volume/LensDistortion";
        public string Label => label;
        public Color BarColor => Color.green;

        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        public bool IsCompleted { get; private set; }

        public void Reset()
        {
            _tween.Complete();
            IsCompleted = false;
        }

        public void Play()
        {
            _tween.Complete();
            _volume = VolumeSingleton.Instance.volume;
            var lensDistortion = VolumeComponentUtil.AddVolumeComponent<LensDistortion>(_volume);
            lensDistortion.xMultiplier.value = xMultiplier;
            lensDistortion.yMultiplier.value = yMultiplier;
            lensDistortion.center.value = center;
            VolumeComponentUtil.ActiveAllProperty(lensDistortion);
            if (easeType == EaseType.Curve)
                _tween = DOVirtual.Float(intensity * start, intensity * end, duration,
                        value => lensDistortion.intensity.value = value)
                    .SetEase(easeCurve)
                    .OnComplete(() =>
                    {
                        IsCompleted = true;
                        _volume.profile.Remove(typeof(LensDistortionFeedback));
                    });
            else
                _tween = DOVirtual.Float(intensity * start, intensity * end, duration,
                        value => lensDistortion.intensity.value = value)
                    .SetEase(easing)
                    .OnComplete(() =>
                    {
                        IsCompleted = true;
                        _volume.profile.Remove(typeof(LensDistortionFeedback));
                    });
        }

        public void Stop()
        {
            _tween.Pause();
        }
    }
}