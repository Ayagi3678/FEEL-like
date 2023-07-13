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
    public class BloomFeedback : IFeedback
    {
        public bool isActive = true;
        public string label = "Bloom";

        [Header("Animation")] public EaseType easeType = EaseType.Curve;

        public float duration = 1;

        [Space(10)] [ConditionalDisable(nameof(easeType), (int)EaseType.Curve, conditionalInvisible: true)]
        public AnimationCurve easeCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [ConditionalDisable(nameof(easeType), (int)EaseType.Easing, conditionalInvisible: true)]
        public Ease easing = Ease.Linear;

        [SerializeField] private float start;
        [SerializeField] private float end = 1;

        [Header("Bloom")] [SerializeField] private float threshold = .9f;

        [SerializeField] private float intensity = 1;
        [Range(0, 1)] [SerializeField] private float scatter = .7f;
        [SerializeField] private Color tint = Color.white;
        private Tween _tween;
        private Volume _volume;
        public string MenuString => "Volume/Bloom";
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
            var bloom = VolumeComponentUtil.AddVolumeComponent<Bloom>(_volume);
            bloom.threshold.value = threshold;
            bloom.scatter.value = scatter;
            bloom.tint.value = tint;
            VolumeComponentUtil.ActiveAllProperty(bloom);

            if (easeType == EaseType.Curve)
                _tween = DOVirtual.Float(intensity * end, intensity * start, duration,
                        value => bloom.intensity.value = value)
                    .SetEase(easeCurve)
                    .OnComplete(() =>
                    {
                        IsCompleted = true;
                        _volume.profile.components.Remove(bloom);
                    });
            else
                _tween = DOVirtual.Float(intensity * end, intensity * start, duration,
                        value => bloom.intensity.value = value)
                    .SetEase(easing)
                    .OnComplete(() =>
                    {
                        IsCompleted = true;
                        _volume.profile.components.Remove(bloom);
                    });
        }

        public void Stop()
        {
            _tween.Pause();
        }
    }
}