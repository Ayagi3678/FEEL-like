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
    public class VignetteFeedback : IFeedback
    {
        public bool isActive = true;
        public string label = "Vignette";

        [Header("Animation")] public EaseType easeType = EaseType.Curve;

        public float duration = 1;

        [Space(10)] [ConditionalDisable(nameof(easeType), (int)EaseType.Curve, conditionalInvisible: true)]
        public AnimationCurve easeCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [ConditionalDisable(nameof(easeType), (int)EaseType.Easing, conditionalInvisible: true)]
        public Ease easing = Ease.Linear;

        [SerializeField] private float start = 1;
        [SerializeField] private float end;

        [Header("Vignette")] [SerializeField] private Color color = new(0, 0, 0, 0);

        [SerializeField] private Vector2 center = Vector2.one * .5f;
        [Range(0, 1)] [SerializeField] private float intensity = .5f;
        [Range(0, 1)] [SerializeField] private float smoothness = 0.2f;
        private Tween _tween;
        private Volume _volume;
        public string MenuString => "Volume/Vignette";
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
            var vignette = VolumeComponentUtil.AddVolumeComponent<Vignette>(_volume);
            vignette.color.value = color;
            vignette.center.value = center;
            vignette.smoothness.value = smoothness;
            VolumeComponentUtil.ActiveAllProperty(vignette);
            if (easeType == EaseType.Curve)
                _tween = DOVirtual.Float(intensity * start, intensity * end, duration,
                        value => vignette.intensity.value = value)
                    .SetEase(easeCurve)
                    .OnComplete(() =>
                    {
                        IsCompleted = true;
                        _volume.profile.components.Remove(vignette);
                    });
            else
                _tween = DOVirtual.Float(intensity * start, intensity * end, duration,
                        value => vignette.intensity.value = value)
                    .SetEase(easing)
                    .OnComplete(() =>
                    {
                        IsCompleted = true;
                        _volume.profile.components.Remove(vignette);
                    });
        }

        public void Stop()
        {
            _tween.Pause();
        }
    }
}