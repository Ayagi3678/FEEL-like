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
    public class ChromaticAberrationFeedback : IFeedback
    {
        public bool isActive = true;
        public string label = "ChromaticAberration";

        [Header("Animation")] public EaseType easeType = EaseType.Curve;

        public float duration = 1;

        [Space(10)] [ConditionalDisable(nameof(easeType), (int)EaseType.Curve, conditionalInvisible: true)]
        public AnimationCurve easeCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [ConditionalDisable(nameof(easeType), (int)EaseType.Easing, conditionalInvisible: true)]
        public Ease easing = Ease.Linear;

        [SerializeField] private float start;
        [SerializeField] private float end = 1;

        [Header("ChromaticAberration")] [Range(0, 1)] [SerializeField]
        private float intensity = 1;

        private Tween _tween;
        private Volume _volume;
        public string MenuString => "Volume/ChromaticAberration";
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
            var chromaticAberration = VolumeComponentUtil.AddVolumeComponent<ChromaticAberration>(_volume);
            VolumeComponentUtil.ActiveAllProperty(chromaticAberration);
            if (easeType == EaseType.Curve)
                _tween = DOVirtual.Float(intensity * end, intensity * start, duration,
                        value => chromaticAberration.intensity.value = value)
                    .SetEase(easeCurve)
                    .OnComplete(() =>
                    {
                        IsCompleted = true;
                        _volume.profile.components.Remove(chromaticAberration);
                    });
            else
                _tween = DOVirtual.Float(intensity * end, intensity * start, duration,
                        value => chromaticAberration.intensity.value = value)
                    .SetEase(easing)
                    .OnComplete(() =>
                    {
                        IsCompleted = true;
                        _volume.profile.components.Remove(chromaticAberration);
                    });
        }

        public void Stop()
        {
            _tween.Pause();
        }
    }
}