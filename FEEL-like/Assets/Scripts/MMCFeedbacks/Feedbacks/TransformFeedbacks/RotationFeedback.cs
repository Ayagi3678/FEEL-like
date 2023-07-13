using System;
using DG.Tweening;
using MMCFeedbacks.Attribute;
using UnityEngine;
using static MimicFeedBacker.Feedbacks.FeedbackEnums;

namespace MMCFeedbacks
{
    [Serializable]
    public class RotationFeedback : IFeedback
    {
        public bool isActive = true;
        public string label = "Rotation";
        [Header("Transform")] public Transform transform;
        public SpaceType spaceType;

        [Header("Animation")] public EaseType easeType = EaseType.Curve;

        public ChangeType changeType = ChangeType.Target;
        public float duration = 1;

        [Space(10)] [ConditionalDisable(nameof(easeType), (int)EaseType.Curve, conditionalInvisible: true)]
        public AnimationCurve easeCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [ConditionalDisable(nameof(easeType), (int)EaseType.Easing, conditionalInvisible: true)]
        public Ease easing = Ease.Linear;

        public Vector3 targetRotation;
        public bool isRevers = true;
        private Transform _initialTransform;
        private Tween _tween;
        public string MenuString => "Transform/Rotation";
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
                var rotation = transform.eulerAngles;
                if (easeType == EaseType.Curve)
                    _tween = DOVirtual.Vector3(rotation,
                            changeType == ChangeType.Target ? targetRotation : rotation + targetRotation, duration,
                            value => transform.eulerAngles = value)
                        .SetEase(easeCurve)
                        .OnComplete(() =>
                        {
                            if (isRevers) _initialTransform.eulerAngles = rotation;
                        });
                else
                    _tween = DOVirtual.Vector3(rotation,
                            changeType == ChangeType.Target ? targetRotation : rotation + targetRotation, duration,
                            value => transform.eulerAngles = value)
                        .SetEase(easing)
                        .OnComplete(() =>
                        {
                            if (isRevers) _initialTransform.eulerAngles = rotation;
                        });
            }
            else
            {
                var rotation = transform.localEulerAngles;
                if (easeType == EaseType.Curve)
                    _tween = DOVirtual.Vector3(rotation,
                            changeType == ChangeType.Target ? targetRotation : rotation + targetRotation, duration,
                            value => transform.localEulerAngles = value)
                        .SetEase(easeCurve)
                        .OnComplete(() =>
                        {
                            if (isRevers) _initialTransform.localEulerAngles = rotation;
                        });
                else
                    _tween = DOVirtual.Vector3(rotation,
                            changeType == ChangeType.Target ? targetRotation : rotation + targetRotation, duration,
                            value => transform.localEulerAngles = value)
                        .SetEase(easing)
                        .OnComplete(() =>
                        {
                            if (isRevers) _initialTransform.localEulerAngles = rotation;
                        });
            }
        }

        public void Stop()
        {
            _tween.Pause();
        }
    }
}