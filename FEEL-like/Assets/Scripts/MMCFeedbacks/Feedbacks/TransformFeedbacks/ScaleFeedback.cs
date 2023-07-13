using System;
using DG.Tweening;
using MMCFeedbacks.Attribute;
using UnityEngine;
using static MimicFeedBacker.Feedbacks.FeedbackEnums;

namespace MMCFeedbacks
{
    [Serializable]
    public class ScaleFeedback : IFeedback
    {
        public bool isActive = true;
        public string label = "Scale";

        [Header("Transform")] public Transform transform;

        [Header("Animation")] public EaseType easeType = EaseType.Curve;
        public ChangeType changeType = ChangeType.Target;
        public float duration = 1;

        [Space(10)] [ConditionalDisable(nameof(easeType), (int)EaseType.Curve, conditionalInvisible: true)]
        public AnimationCurve easeCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [ConditionalDisable(nameof(easeType), (int)EaseType.Easing, conditionalInvisible: true)]
        public Ease easing = Ease.Linear;

        [ConditionalDisable("changeType", (int)ChangeType.Target, conditionalInvisible: true)]
        public Vector3 targetScale = Vector3.one;

        [ConditionalDisable("changeType", (int)ChangeType.Add, conditionalInvisible: true)]
        public Vector3 addScale = Vector3.zero;

        public bool isRevers = true;
        private Transform _initialTransform;
        private Tween _tween;
        public string MenuString => "Transform/Scale";
        public string Label => label;
        public Color BarColor => Color.cyan;

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
            _initialTransform = transform;
        }

        public void Play()
        {
            _tween.Complete();
            var scale = transform.localScale;
            if (easeType == EaseType.Curve)
                _tween = transform.DOScale(changeType == ChangeType.Add ? scale + addScale : targetScale, duration)
                    .SetEase(easeCurve)
                    .OnComplete(() =>
                    {
                        if (isRevers) _initialTransform.localScale = scale;
                    });
            else
                _tween = transform.DOScale(changeType == ChangeType.Add ? scale + addScale : targetScale, duration)
                    .SetEase(easing)
                    .OnComplete(() =>
                    {
                        if (isRevers) _initialTransform.localScale = scale;
                    });
        }

        public void Stop()
        {
            _tween.Pause();
        }
    }
}