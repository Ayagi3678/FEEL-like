using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace MMCFeedbacks
{
    [Serializable]
    public class EventFeedback : IFeedback
    {
        public bool isActive = true;
        public string label = "Event";

        [FormerlySerializedAs("_event")] [Header("Events")]
        public UnityEvent @event;

        public string MenuString => "Event/Event";
        public string Label => label;
        public Color BarColor => Color.Lerp(Color.yellow, Color.red, 0.5f);

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
            @event.Invoke();
        }

        public void Stop()
        {
        }
    }
}