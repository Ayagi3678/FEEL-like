using System;
using MMCFeedbacks.Singleton;
using UnityEngine;
using UnityEngine.Serialization;

namespace MMCFeedbacks.Feedbacks.TimeFeedbacks
{
    [Serializable]
    public class TimeEffectFeedback : IFeedback
    {
        public bool isActive = true;

        public string label = "Time Effect";
        [Header("Time Effect")] 
        public int priority;
        public int durationFrame;
        public float timeScale = 0;

        public string MenuString => "Time/Time Effect";
        public string Label => label;
        public Color BarColor => Color.gray;

        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        public bool IsCompleted => true;

        private TimeEffect _timeEffect;
        
        public void Reset()
        {
            _timeEffect?.OnDiscardCallback();
        }

        public void Play()
        { 
            _timeEffect = new TimeEffect(priority,timeScale, durationFrame);
            TimeEffectSingleton.Instance.SetTimeEffect(_timeEffect);
        }

        public void Stop(){}
    }
}