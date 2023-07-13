using System;
using UnityEngine;

namespace MMCFeedbacks.Feedbacks.MMCFeedbacks
{
    [Serializable]
    public class MMCFeedbackerPlayerFeedback : IFeedback
    {
        public bool isActive = true;
        public string label = "MMCFeedbacker Player";

        [Header("MMCFeedbacker")] [SerializeField]
        private MMCFeedbacker mmcFeedbacker;

        public string MenuString => "MMCFeedback/MMCFeedback Player";
        public string Label => label;
        public Color BarColor => Color.black;

        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        public bool IsCompleted => true;

        public void Reset()
        {
        }

        public void Play()
        {
            mmcFeedbacker.Play();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}