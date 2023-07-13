using System;
using DG.Tweening;
using MMCFeedbacks.Attribute;
using UnityEngine;
using static MimicFeedBacker.Feedbacks.FeedbackEnums;

namespace MMCFeedbacks
{
    [Serializable]
    public class PositionFeedback : IFeedback
    {
        public bool isActive = true;
        public string label = "Position";

        [Header("Transform")] public Transform transform;

        public SpaceType spaceType;

        [Header("Animation")] public EaseType easeType = EaseType.Curve;

        public float duration = 1;

        [Space(10)] [ConditionalDisable(nameof(easeType), (int)EaseType.Curve, conditionalInvisible: true)]
        public AnimationCurve easeCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [ConditionalDisable(nameof(easeType), (int)EaseType.Easing, conditionalInvisible: true)]
        public Ease easing = Ease.Linear;

        public Vector3 movePosition;
        public bool isRevers = true;
        private Transform _initialTransform;

        private Tween _tween;
        public string MenuString => "Transform/Position";
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

            if (spaceType == SpaceType.World)
            {
                var position = transform.position;
                if (easeType == EaseType.Curve)
                    _tween = transform.DOMove(position + movePosition, duration)
                        .SetEase(easeCurve)
                        .OnComplete(() =>
                        {
                            if (isRevers) _initialTransform.position = position;
                            IsCompleted = true;
                        });
                else
                    _tween = transform.DOMove(position + movePosition, duration)
                        .SetEase(easing)
                        .OnComplete(() =>
                        {
                            if (isRevers) _initialTransform.position = position;
                            IsCompleted = true;
                        });
            }
            else
            {
                var position = transform.localPosition;
                if (easeType == EaseType.Curve)
                    _tween = transform.DOLocalMove(position + movePosition, duration)
                        .SetEase(easeCurve)
                        .OnComplete(() =>
                        {
                            if (isRevers) _initialTransform.localPosition = position;
                            IsCompleted = true;
                        });
                else
                    _tween = transform.DOLocalMove(position + movePosition, duration)
                        .SetEase(easing)
                        .OnComplete(() =>
                        {
                            if (isRevers) _initialTransform.localPosition = position;
                            IsCompleted = true;
                        });
            }
        }

        public void Stop()
        {
            _tween.Pause();
        }
    }
}