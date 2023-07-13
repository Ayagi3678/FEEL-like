using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace MMCFeedbacks
{
    [Serializable]
    public class ColorAdjustmentFeedback : IFeedback
    {
        public bool isActive = true;
        public string label = "Color Adjustment";
        private Tween _tween;
        private Volume _volume;
        public string MenuString => "Volume/Color Adjustment";
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
        }

        public void Stop()
        {
            _tween.Pause();
        }
    }
}