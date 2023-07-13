using UnityEngine;

namespace MMCFeedbacks
{
    public interface IFeedback
    {
        public string MenuString { get; }
        public string Label { get; }
        public Color BarColor { get; }
        public bool IsActive { get; set; }
        bool IsCompleted { get; }

        public bool IsComplete()
        {
            return IsCompleted;
        }

        void Reset();
        void Play();
        void Stop();
    }
}